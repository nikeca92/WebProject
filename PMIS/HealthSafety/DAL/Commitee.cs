using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.HealthSafety.Common
{
    public class Committee : BaseDbObject
    {
        private int committeeId;        
        private int? committeeTypeId;
        private GTableItem committeeType;
        private int? militaryForceTypeId;        
        private int? militaryUnitId;
        private MilitaryUnit militaryUnit;
        private List<CommitteeMember> committeeMembers;
        private MilitaryForceType militaryForceType;

        public int CommitteeId
        {
            get { return committeeId; }
            set { committeeId = value; }
        }

        public int? CommitteeTypeId
        {
            get { return committeeTypeId; }
            set { committeeTypeId = value; }
        }

        public GTableItem CommitteeType
        {
            get
            {
                if (committeeType == null && committeeTypeId != null)
                {
                    committeeType = GTableItemUtil.GetTableItem("ComitteeTypes", (int)committeeTypeId, ModuleUtil.HS(), CurrentUser);
                }
                return committeeType;
            }
            set
            {
                committeeType = value;
            }
        }

        public int? MilitaryForceTypeId
        {
            get { return militaryForceTypeId; }
            set { militaryForceTypeId = value; }
        }        

        public int? MilitaryUnitId
        {
            get { return militaryUnitId; }
            set { militaryUnitId = value; }
        }

        public MilitaryUnit MilitaryUnit
        {
            get
            {
                if (militaryUnit == null && militaryUnitId != null)
                {
                    militaryUnit = MilitaryUnitUtil.GetMilitaryUnit(militaryUnitId.Value, CurrentUser);
                }
                return militaryUnit;
            }
            set
            {
                militaryUnit = value;
            }
        }

        public MilitaryForceType MilitaryForceType
        {
            get
            {
                if (militaryForceType == null && militaryForceTypeId != null)
                {
                    militaryForceType = MilitaryForceTypeUtil.GetMilitaryForceType((int)militaryForceTypeId, CurrentUser);
                }
                return militaryForceType;
            }
            set
            {
                militaryForceType = value;
            }
        }

         public List<CommitteeMember> CommitteeMembers
        {
            get 
            {
                if (committeeMembers == null)
                    committeeMembers = CommitteeMemberUtil.GetAllCommitteeMembersByCommittee(CommitteeId, CurrentUser);

                return committeeMembers; 
            }
            set { committeeMembers = value; }
        }

         public bool CanDelete
         {
             get { return true; }

         }

        public Committee(User user)
            : base(user)
        {
        }
    }

    public static class CommitteeUtil
    {
        private static Committee ExtractCommitteeFromDR(OracleDataReader dr, User currentUser)
        {
            Committee committee = new Committee(currentUser);

            committee.CommitteeId = DBCommon.GetInt(dr["CommitteeID"]);
            committee.CommitteeTypeId = (DBCommon.IsInt(dr["CommitteeTypeID"]) ? (int?)DBCommon.GetInt(dr["CommitteeTypeID"]) : null);
            committee.MilitaryForceTypeId = (DBCommon.IsInt(dr["MilitaryForceTypeID"]) ? (int?)DBCommon.GetInt(dr["MilitaryForceTypeID"]) : null);
            committee.MilitaryUnitId = (DBCommon.IsInt(dr["MilitaryUnitID"]) ? (int?)DBCommon.GetInt(dr["MilitaryUnitID"]) : null);

            BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, committee);

            return committee;
        }

        public static Committee GetCommittee(int committeeId, User currentUser)
        {
            Committee committee = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("HS_COMMITTEE", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where = " AND a.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.CommitteeID as CommitteeID,
                                      a.CommitteeTypeID as CommitteeTypeID,
                                      a.MilitaryForceTypeID as MilitaryForceTypeID,
                                      a.MilitaryUnitID as MilitaryUnitID,
                                      a.CreatedBy as CreatedBy,
                                      a.CreatedDate as CreatedDate, 
                                      a.LastModifiedBy as LastModifiedBy, 
                                      a.LastModifiedDate as LastModifiedDate
                               FROM PMIS_HS.Committees a
                               WHERE a.CommitteeID = :CommitteeID " + where;

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    SQL += @" AND (a.MilitaryUnitID IS NULL OR a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @"))
                            ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("CommitteeID", OracleType.Number).Value = committeeId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    committee = ExtractCommitteeFromDR(dr, currentUser);               
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return committee;
        }

        public static List<Committee> GetAllCommittees(User currentUser)
        {
            List<Committee> committees = new List<Committee>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("HS_COMMITTEE", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where = "WHERE a.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.CommitteeID as CommitteeID,
                                      a.CommitteeTypeID as CommitteeTypeID,
                                      a.MilitaryForceTypeID as MilitaryForceTypeID,
                                      a.MilitaryUnitID as MilitaryUnitID,
                                      a.CreatedBy as CreatedBy,
                                      a.CreatedDate as CreatedDate, 
                                      a.LastModifiedBy as LastModifiedBy, 
                                      a.LastModifiedDate as LastModifiedDate
                               FROM PMIS_HS.Committees a " + where;

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    if (where == "")
                    {
                        where += @" WHERE (a.MilitaryUnitID IS NULL OR a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @"))
                            ";
                    }
                    else
                    {
                        where += @" AND (a.MilitaryUnitID IS NULL OR a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @"))
                            ";
                    }

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["CommitteeID"]))
                        committees.Add(ExtractCommitteeFromDR(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return committees;
        }

        public static List<Committee> GetAllCommittees(string committeeTypeIds, string militaryForceTypeIds, string militaryUnitIds, int orderBy, int pageIdx, int rowsPerPage, User currentUser)
        {
            List<Committee> committees = new List<Committee>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("HS_COMMITTEE", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!String.IsNullOrEmpty(committeeTypeIds))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.CommitteeTypeId IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(committeeTypeIds) + ") ";
                }

                if (!String.IsNullOrEmpty(militaryForceTypeIds))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MilitaryForceTypeId IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(militaryForceTypeIds) + ") ";
                }

                if (!String.IsNullOrEmpty(militaryUnitIds))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MilitaryUnitId IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(militaryUnitIds) + ") ";
                }

                where = (where == "" ? "" : " WHERE ") + where;

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    if (where == "")
                    {
                        where += @" WHERE (a.MilitaryUnitID IS NULL OR a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @"))
                            ";
                    }
                    else
                    {
                        where += @" AND (a.MilitaryUnitID IS NULL OR a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @"))
                            ";
                    }

                string pageWhere = "";

                if (pageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE RowNumber BETWEEN (" + pageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + pageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

                string orderBySQL = "";
                string orderByDir = "ASC";

                if (orderBy > 100)
                {
                    orderBy -= 100;
                    orderByDir = "DESC";
                }

                switch (orderBy)
                {
                    case 1:
                        orderBySQL = "b.TableValue";
                        break;
                    case 2:
                        orderBySQL = "d.MilitaryForceTypeName";
                        break;
                    case 3:
                        orderBySQL = "c.IMEES";
                        break;                   
                    default:
                        orderBySQL = "b.TableValue";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT tmp.CommitteeID as CommitteeID,
                                      tmp.CommitteeTypeID as CommitteeTypeID,
                                      tmp.MilitaryForceTypeID as MilitaryForceTypeID,
                                      tmp.MilitaryUnitID as MilitaryUnitID,
                                      tmp.CreatedBy as CreatedBy,
                                      tmp.CreatedDate as CreatedDate, 
                                      tmp.LastModifiedBy as LastModifiedBy, 
                                      tmp.LastModifiedDate as LastModifiedDate,
                                      tmp.RowNumber as RowNumber  FROM (
                                  SELECT a.CommitteeID as CommitteeID,
                                         a.CommitteeTypeID as CommitteeTypeID,
                                         a.MilitaryForceTypeID as MilitaryForceTypeID,
                                         a.MilitaryUnitID as MilitaryUnitID,
                                         a.CreatedBy as CreatedBy,
                                         a.CreatedDate as CreatedDate, 
                                         a.LastModifiedBy as LastModifiedBy, 
                                         a.LastModifiedDate as LastModifiedDate,                                         
                                         RANK() OVER (ORDER BY " + orderBySQL + @", a.CommitteeID) as RowNumber 
                                  FROM PMIS_HS.Committees a                 
                                  LEFT OUTER JOIN PMIS_HS.Gtable b ON a.CommitteeTypeID = b.TableKey AND b.TableName = 'ComitteeTypes'
                                  LEFT OUTER JOIN UKAZ_OWNER.MIR c ON a.MilitaryUnitID = c.KOD_MIR           
                                  LEFT OUTER JOIN PMIS_ADM.MilitaryForceTypes d ON a.MilitaryForceTypeID = d.MilitaryForceTypeID
                                  " + where + @"    
                                  ORDER BY " + orderBySQL + @", CommitteeID                             
                               ) tmp
                               " + pageWhere;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["CommitteeID"]))
                        committees.Add(ExtractCommitteeFromDR(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return committees;
        }

        public static int GetAllCommitteesCount(string committeeTypeIds, string militaryForceTypeIds, string militaryUnitIds, User currentUser)
        {
            int committeesCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("HS_COMMITTEE", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!String.IsNullOrEmpty(committeeTypeIds))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.CommitteeTypeId IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(committeeTypeIds) + ") ";
                }

                if (!String.IsNullOrEmpty(militaryForceTypeIds))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MilitaryForceTypeId IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(militaryForceTypeIds) + ") ";
                }

                if (!String.IsNullOrEmpty(militaryUnitIds))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MilitaryUnitId IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(militaryUnitIds) + ") ";
                }

                where = (where == "" ? "" : " WHERE ") + where;

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    if (where == "")
                    {
                        where += @" WHERE (a.MilitaryUnitID IS NULL OR a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @"))
                            ";
                    }
                    else
                    {
                        where += @" AND (a.MilitaryUnitID IS NULL OR a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @"))
                            ";
                    }

                string SQL = @"SELECT COUNT(*) as Cnt
                               FROM PMIS_HS.Committees a
                               " + where + @"
                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        committeesCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return committeesCnt;
        }

        public static void SetCommitteeModified(int committeeId, User currentUser)
        {
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"UPDATE PMIS_HS.Committees SET
                                  LastModifiedBy = :LastModifiedBy,
                                  LastModifiedDate = :LastModifiedDate
                               WHERE CommitteeID = :CommitteeID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("CommitteeID", OracleType.Number).Value = committeeId;

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }

        public static bool SaveCommittee(Committee committee, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";
                if (committee.CommitteeId == 0)
                {
                    SQL += @"INSERT INTO PMIS_HS.Committees (CommitteeTypeID, MilitaryForceTypeID, MilitaryUnitID,
                                                            CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate)
                            VALUES (:CommitteeTypeID, :MilitaryForceTypeID, :MilitaryUnitID,
                                    :CreatedBy, :CreatedDate, :LastModifiedBy, :LastModifiedDate);

                            SELECT PMIS_HS.Committees_ID_SEQ.currval INTO :CommitteeID FROM dual;

                            ";

                    changeEvent = new ChangeEvent("HS_Committee_AddCommittee", "", committee.MilitaryUnit, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("HS_Committee_CommitteeType", "", committee.CommitteeType != null ? committee.CommitteeType.TableValue : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_Committee_MilitaryForceType", "", committee.MilitaryForceType != null ? committee.MilitaryForceType.MilitaryForceTypeName : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_Committee_MilitaryUnit", "", committee.MilitaryUnit != null ? committee.MilitaryUnit.DisplayTextForSelection : "", currentUser));                    
                }
                else
                {
                    SQL += @"UPDATE PMIS_HS.Committees SET
                               CommitteeTypeID = :CommitteeTypeID, 
                               MilitaryForceTypeID = :MilitaryForceTypeID, 
                               MilitaryUnitID = :MilitaryUnitID,
                               LastModifiedBy = CASE WHEN :AnyActualChanges = 1 
                                                     THEN :LastModifiedBy
                                                     ELSE LastModifiedBy
                                                END, 
                               LastModifiedDate = CASE WHEN :AnyActualChanges = 1 
                                                       THEN :LastModifiedDate
                                                       ELSE LastModifiedDate
                                                  END
                            WHERE CommitteeID = :CommitteeID ;                       

                            ";

                    changeEvent = new ChangeEvent("HS_Committee_EditCommittee", "", committee.MilitaryUnit, null, currentUser);

                    Committee oldCommittee = CommitteeUtil.GetCommittee(committee.CommitteeId, currentUser);

                    if ((oldCommittee.CommitteeType != null ? oldCommittee.CommitteeType.TableValue : "") != (committee.CommitteeType != null ? committee.CommitteeType.TableValue : ""))
                        changeEvent.AddDetail(new ChangeEventDetail("HS_Committee_CommitteeType", oldCommittee.CommitteeType != null ? oldCommittee.CommitteeType.TableValue : "", committee.CommitteeType != null ? committee.CommitteeType.TableValue : "", currentUser));

                    if ((oldCommittee.MilitaryForceType != null ? oldCommittee.MilitaryForceType.MilitaryForceTypeName : "") != (committee.MilitaryForceType != null ? committee.MilitaryForceType.MilitaryForceTypeName : ""))
                        changeEvent.AddDetail(new ChangeEventDetail("HS_Committee_MilitaryForceType", oldCommittee.MilitaryForceType != null ? oldCommittee.MilitaryForceType.MilitaryForceTypeName : "", committee.MilitaryForceType != null ? committee.MilitaryForceType.MilitaryForceTypeName : "", currentUser));

                    if ((oldCommittee.MilitaryUnit != null ? oldCommittee.MilitaryUnit.DisplayTextForSelection : "") != (committee.MilitaryUnit != null ? committee.MilitaryUnit.DisplayTextForSelection : ""))
                        changeEvent.AddDetail(new ChangeEventDetail("HS_Committee_MilitaryUnit", oldCommittee.MilitaryUnit != null ? oldCommittee.MilitaryUnit.DisplayTextForSelection : "", committee.MilitaryUnit != null ? committee.MilitaryUnit.DisplayTextForSelection : "", currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramCommitteeID = new OracleParameter();
                paramCommitteeID.ParameterName = "CommitteeID";
                paramCommitteeID.OracleType = OracleType.Number;

                if (committee.CommitteeId != 0)
                {
                    paramCommitteeID.Direction = ParameterDirection.Input;
                    paramCommitteeID.Value = committee.CommitteeId;
                }
                else
                {
                    paramCommitteeID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramCommitteeID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "CommitteeTypeID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (committee.CommitteeTypeId.HasValue)
                    param.Value = committee.CommitteeTypeId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryForceTypeID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (committee.MilitaryForceTypeId.HasValue)
                    param.Value = committee.MilitaryForceTypeId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryUnitID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (committee.MilitaryUnitId.HasValue)
                    param.Value = committee.MilitaryUnitId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);
                
                if (committee.CommitteeId == 0)
                {
                    BaseDbObjectUtil.SetCreatedParams(cmd, currentUser);
                }
                else
                {
                    BaseDbObjectUtil.SetAnyActualChanges(cmd, changeEvent);
                }

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();

                if (committee.CommitteeId == 0)
                    committee.CommitteeId = DBCommon.GetInt(paramCommitteeID.Value);

                result = true;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                if (changeEvent.ChangeEventDetails.Count > 0)
                    changeEntry.AddEvent(changeEvent);
            }

            return result;
        }

        public static bool DeleteCommittee(int committeeId, User currentUser, Change changeEntry)
        {
            bool result = false;

            Committee oldCommittee = CommitteeUtil.GetCommittee(committeeId, currentUser);

            ChangeEvent changeEvent = new ChangeEvent("HS_Committee_DeleteCommittee", "", oldCommittee.MilitaryUnit, null, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("HS_Committee_CommitteeType", oldCommittee.CommitteeType != null ? oldCommittee.CommitteeType.TableValue : "", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_Committee_MilitaryForceType", oldCommittee.MilitaryForceType != null ? oldCommittee.MilitaryForceType.MilitaryForceTypeName : "", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_Committee_MilitaryUnit", oldCommittee.MilitaryUnit != null ? oldCommittee.MilitaryUnit.DisplayTextForSelection : "", "", currentUser));                    
          
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" BEGIN
                                DELETE FROM PMIS_HS.CommitteeMembers WHERE CommitteeID = :CommitteeID;
                                
                                DELETE FROM PMIS_HS.Committees WHERE CommitteeID = :CommitteeID;
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("CommitteeID", OracleType.Number).Value = committeeId;

                result = cmd.ExecuteNonQuery() == 1;
            }
            finally
            {
                conn.Close();
            }

            if (result)
                changeEntry.AddEvent(changeEvent);

            return result;
        }
    }

}