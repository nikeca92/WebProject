using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.HealthSafety.Common
{
    public class RiskAssessment : BaseDbObject
    {
        private int riskAssessmentId;
        private DateTime preparationDate;
        private string regNumber;
        private int? militaryForceTypeId;
        private int? militaryUnitId;
        private MilitaryUnit militaryUnit;
        private string comments;
        private List<RiskRemoveRecommendation> riskRemoveRecommendationItems;
        private MilitaryForceType militaryForceType;

        public int RiskAssessmentId
        {
            get { return riskAssessmentId; }
            set { riskAssessmentId = value; }
        }

        public DateTime PreparationDate
        {
            get { return preparationDate; }
            set { preparationDate = value; }
        }

        public string RegNumber
        {
            get { return regNumber; }
            set { regNumber = value; }
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
                    militaryUnit = MilitaryUnitUtil.GetMilitaryUnit((int)militaryUnitId, CurrentUser);
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

        public string Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        public List<RiskRemoveRecommendation> RiskRemoveRecommendationItems
        {
            get
            {
                if (riskRemoveRecommendationItems == null)
                {
                    riskRemoveRecommendationItems = RiskRemoveRecommendationUtil.GetRiskRemoveRecommendationsByRiskAssesment(riskAssessmentId, CurrentUser);
                }
                return riskRemoveRecommendationItems;
            }
            set
            {
                riskRemoveRecommendationItems = value;
            }
        }

        public RiskAssessment(User user)
            : base(user)
        {

        }
    }

    public static class RiskAssessmentUtil
    {
        public static RiskAssessment GetRiskAssessment(int riskAssessmentId, User currentUser)
        {
            RiskAssessment riskAssessment = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("HS_RISKASSESSMENTS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where = " AND a.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.PreparationDate, a.RegNumber, 
                                      a.MilitaryForceTypeID, a.MilitaryUnitID, a.Comments,
                                      a.CreatedBy, a.CreatedDate, a.LastModifiedBy, a.LastModifiedDate
                               FROM PMIS_HS.RiskAssessments a
                               WHERE a.RiskAssessmentID = :RiskAssessmentID " + where;

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    SQL += @" AND (a.MilitaryUnitID IS NULL OR a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @"))
                            ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RiskAssessmentID", OracleType.Number).Value = riskAssessmentId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    riskAssessment = new RiskAssessment(currentUser);
                    riskAssessment.RiskAssessmentId = riskAssessmentId;

                    if (dr["PreparationDate"] is DateTime)
                        riskAssessment.PreparationDate = (DateTime)dr["PreparationDate"];

                    riskAssessment.RegNumber = dr["RegNumber"].ToString();

                    if (dr["MilitaryForceTypeID"] != null)
                        riskAssessment.MilitaryForceTypeId = DBCommon.GetInt(dr["MilitaryForceTypeID"]);
                    else
                        riskAssessment.MilitaryForceTypeId = null;

                    if (dr["MilitaryUnitID"] != null)
                        riskAssessment.MilitaryUnitId = DBCommon.GetInt(dr["MilitaryUnitID"]);
                    else
                        riskAssessment.MilitaryUnitId = null;

                    riskAssessment.Comments = dr["Comments"].ToString();

                    BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, riskAssessment);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return riskAssessment;
        }

        public static List<RiskAssessment> GetAllRiskAssessments(string regNumber, int? militaryUnitId, DateTime? dateFrom, DateTime? dateTo, int orderBy, int pageIdx, int rowsPerPage, User currentUser)
        {
            List<RiskAssessment> riskAssessments = new List<RiskAssessment>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("HS_RISKASSESSMENTS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!String.IsNullOrEmpty(regNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.RegNumber LIKE '%" + regNumber.Replace("'", "''") + @"%' ";
                }

                if (militaryUnitId != null)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MilitaryUnitID = " + militaryUnitId + " ";
                }

                if (dateFrom.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.PreparationDate >= " + DBCommon.DateToDBCode(dateFrom.Value) + " ";
                }

                if (dateTo.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.PreparationDate < " + DBCommon.DateToDBCode(dateTo.Value.AddDays(1)) + " ";
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
                        orderBySQL = "a.RegNumber";
                        break;
                    case 2:
                        orderBySQL = "a.PreparationDate";
                        break;
                    case 3:
                        orderBySQL = "a.MilitaryUnitID";
                        break;
                    default:
                        orderBySQL = "a.RegNumber";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT tmp.*  FROM (
                                  SELECT a.RiskAssessmentID, a.RegNumber, a.PreparationDate, a.MilitaryForceTypeID, a.MilitaryUnitID, a.Comments,
                                         a.CreatedBy, a.CreatedDate, a.LastModifiedBy, a.LastModifiedDate,
                                         RANK() OVER (ORDER BY " + orderBySQL + @", a.RiskAssessmentID) as RowNumber 
                                  FROM PMIS_HS.RiskAssessments a
                                  LEFT OUTER JOIN UKAZ_OWNER.MIR b ON a.MilitaryUnitID = b.KOD_MIR           
                                  " + where + @"    
                                  ORDER BY " + orderBySQL + @", a.RiskAssessmentID                             
                               ) tmp
                               " + pageWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    RiskAssessment riskAssessment = new RiskAssessment(currentUser);
                    riskAssessment.RiskAssessmentId = DBCommon.GetInt(dr["RiskAssessmentID"]);

                    if (dr["PreparationDate"] is DateTime)
                        riskAssessment.PreparationDate = (DateTime)dr["PreparationDate"];

                    riskAssessment.RegNumber = dr["RegNumber"].ToString();

                    if (dr["MilitaryForceTypeID"] != null)
                        riskAssessment.MilitaryForceTypeId = DBCommon.GetInt(dr["MilitaryForceTypeID"]);
                    else
                        riskAssessment.MilitaryForceTypeId = null;

                    if (dr["MilitaryUnitID"] != null)
                        riskAssessment.MilitaryUnitId = DBCommon.GetInt(dr["MilitaryUnitID"]);
                    else
                        riskAssessment.MilitaryUnitId = null;

                    riskAssessment.Comments = dr["Comments"].ToString();

                    BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, riskAssessment);

                    riskAssessments.Add(riskAssessment);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return riskAssessments;
        }

        public static int GetAllAssessmentsCount(string regNumber, int? militaryUnitId, DateTime? dateFrom, DateTime? dateTo, User currentUser)
        {
            int riskAssessmentsCount = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("HS_RISKASSESSMENTS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!String.IsNullOrEmpty(regNumber))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.RegNumber LIKE '%" + regNumber.Replace("'", "''") + @"%' ";
                }

                if (militaryUnitId != null)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MilitaryUnitID = " + militaryUnitId + " ";
                }

                if (dateFrom.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.PreparationDate >= " + DBCommon.DateToDBCode(dateFrom.Value) + " ";
                }

                if (dateTo.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.PreparationDate < " + DBCommon.DateToDBCode(dateTo.Value.AddDays(1)) + " ";
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

                string SQL = @"SELECT COUNT(*) as Count
                               FROM PMIS_HS.RiskAssessments a
                               " + where + @"
                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Count"]))
                        riskAssessmentsCount = DBCommon.GetInt(dr["Count"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return riskAssessmentsCount;
        }

        public static bool SaveRiskAssessment(RiskAssessment riskAssessment, User currentUser, Change changeEntry)
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

                if (riskAssessment.RiskAssessmentId == 0)
                {
                    SQL += @"INSERT INTO PMIS_HS.RiskAssessments (PreparationDate, RegNumber, MilitaryForceTypeID, MilitaryUnitID, Comments,
                                 CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate)
                             VALUES (:PreparationDate, :RegNumber, :MilitaryForceTypeID, :MilitaryUnitID, :Comments,
                                 :CreatedBy, :CreatedDate, :LastModifiedBy, :LastModifiedDate);

                            SELECT PMIS_HS.RiskAssessments_ID_SEQ.currval INTO :RiskAssessmentID FROM dual;

                            ";

                    changeEvent = new ChangeEvent("HS_RiskAssess_AddRiskAssessment", "", riskAssessment.MilitaryUnit, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("HS_RiskAssess_PreparDate", "", CommonFunctions.FormatDate(riskAssessment.PreparationDate), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_RiskAssess_RegNumber", "", riskAssessment.RegNumber, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_RiskAssess_MilitaryForceType", "", riskAssessment.MilitaryForceType != null ? riskAssessment.MilitaryForceType.MilitaryForceTypeName : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_RiskAssess_MilitaryUnit", "", riskAssessment.MilitaryUnit != null ? riskAssessment.MilitaryUnit.DisplayTextForSelection : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_RiskAssess_Comments", "", String.IsNullOrEmpty(riskAssessment.Comments) ? "" : riskAssessment.Comments, currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_HS.RiskAssessments SET
                               PreparationDate = :PreparationDate, 
                               RegNumber = :RegNumber, 
                               MilitaryForceTypeID = :MilitaryForceTypeID, 
                               MilitaryUnitID = :MilitaryUnitID, 
                               Comments = :Comments,
                               LastModifiedBy = CASE WHEN :AnyActualChanges = 1 
                                                     THEN :LastModifiedBy
                                                     ELSE LastModifiedBy
                                                END, 
                               LastModifiedDate = CASE WHEN :AnyActualChanges = 1 
                                                       THEN :LastModifiedDate
                                                       ELSE LastModifiedDate
                                                  END
                             WHERE RiskAssessmentID = :RiskAssessmentID;
                            ";

                    changeEvent = new ChangeEvent("HS_RiskAssess_EditRiskAssessment", "", riskAssessment.MilitaryUnit, null, currentUser);

                    RiskAssessment oldRiskAssessment = RiskAssessmentUtil.GetRiskAssessment(riskAssessment.RiskAssessmentId, currentUser);

                    if (!CommonFunctions.IsEqual(oldRiskAssessment.PreparationDate, riskAssessment.PreparationDate))
                        changeEvent.AddDetail(new ChangeEventDetail("HS_RiskAssess_PreparDate", CommonFunctions.FormatDate(oldRiskAssessment.PreparationDate), CommonFunctions.FormatDate(riskAssessment.PreparationDate), currentUser));

                    if (oldRiskAssessment.RegNumber.Trim() != riskAssessment.RegNumber.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_RiskAssess_RegNumber", oldRiskAssessment.RegNumber, riskAssessment.RegNumber, currentUser));

                    if ((oldRiskAssessment.MilitaryForceType != null ? oldRiskAssessment.MilitaryForceType.MilitaryForceTypeName : "") != (riskAssessment.MilitaryForceType != null ? riskAssessment.MilitaryForceType.MilitaryForceTypeName : ""))
                        changeEvent.AddDetail(new ChangeEventDetail("HS_RiskAssess_MilitaryForceType", oldRiskAssessment.MilitaryForceType != null ? oldRiskAssessment.MilitaryForceType.MilitaryForceTypeName : "", riskAssessment.MilitaryForceType != null ? riskAssessment.MilitaryForceType.MilitaryForceTypeName : "", currentUser));

                    if ((oldRiskAssessment.MilitaryUnit != null ? oldRiskAssessment.MilitaryUnit.DisplayTextForSelection : "") != (riskAssessment.MilitaryUnit != null ? riskAssessment.MilitaryUnit.DisplayTextForSelection : ""))
                        changeEvent.AddDetail(new ChangeEventDetail("HS_RiskAssess_MilitaryUnit", oldRiskAssessment.MilitaryUnit != null ? oldRiskAssessment.MilitaryUnit.DisplayTextForSelection : "", riskAssessment.MilitaryUnit != null ? riskAssessment.MilitaryUnit.DisplayTextForSelection : "", currentUser));

                    if (oldRiskAssessment.Comments.Trim() != riskAssessment.Comments.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_RiskAssess_Comments", oldRiskAssessment.Comments, riskAssessment.Comments, currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramRiskAssessmentID = new OracleParameter();
                paramRiskAssessmentID.ParameterName = "RiskAssessmentID";
                paramRiskAssessmentID.OracleType = OracleType.Number;

                if (riskAssessment.RiskAssessmentId != 0)
                {
                    paramRiskAssessmentID.Direction = ParameterDirection.Input;
                    paramRiskAssessmentID.Value = riskAssessment.RiskAssessmentId;
                }
                else
                {
                    paramRiskAssessmentID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramRiskAssessmentID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "PreparationDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                param.Value = riskAssessment.PreparationDate;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "RegNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = riskAssessment.RegNumber;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryForceTypeID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (riskAssessment.MilitaryForceTypeId != null)
                    param.Value = riskAssessment.MilitaryForceTypeId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryUnitID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;

                if (riskAssessment.MilitaryUnitId != null)
                    param.Value = riskAssessment.MilitaryUnitId;
                else
                    param.Value = DBNull.Value;
                
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Comments";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = riskAssessment.Comments;
                cmd.Parameters.Add(param);

                if (riskAssessment.RiskAssessmentId == 0)
                {
                    BaseDbObjectUtil.SetCreatedParams(cmd, currentUser);
                }
                else
                {
                    BaseDbObjectUtil.SetAnyActualChanges(cmd, changeEvent);
                }

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();

                if (riskAssessment.RiskAssessmentId == 0)
                    riskAssessment.RiskAssessmentId = DBCommon.GetInt(paramRiskAssessmentID.Value);

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

        public static bool DeleteRiskAssessment(int riskAssessmentId, User currentUser, Change changeEntry)
        {
            bool result = false;

            RiskAssessment oldRiskAssessment = RiskAssessmentUtil.GetRiskAssessment(riskAssessmentId, currentUser);

            ChangeEvent changeEvent = new ChangeEvent("HS_RiskAssess_DeleteRiskAssessment", "", oldRiskAssessment.MilitaryUnit, null, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("HS_RiskAssess_PreparDate", CommonFunctions.FormatDate(oldRiskAssessment.PreparationDate), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_RiskAssess_RegNumber", oldRiskAssessment.RegNumber, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_RiskAssess_MilitaryForceType", oldRiskAssessment.MilitaryForceType != null ? oldRiskAssessment.MilitaryForceType.MilitaryForceTypeName : "", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_RiskAssess_MilitaryUnit", oldRiskAssessment.MilitaryUnit != null ? oldRiskAssessment.MilitaryUnit.DisplayTextForSelection : "", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_RiskAssess_Comments", String.IsNullOrEmpty(oldRiskAssessment.Comments) ? "" : oldRiskAssessment.Comments, "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" BEGIN
                                DELETE FROM PMIS_HS.RiskRemoveRecommendations WHERE RiskAssessmentID = :RiskAssessmentID;
                                
                                DELETE FROM PMIS_HS.RiskAssessments WHERE RiskAssessmentID = :RiskAssessmentID;
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RiskAssessmentID", OracleType.Number).Value = riskAssessmentId;

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

        

        public static void SetRiskAssessmentsLastModified(int riskAssessmentId, User currentUser)
        {
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"UPDATE PMIS_HS.RiskAssessments SET
                                  LastModifiedBy = :LastModifiedBy,
                                  LastModifiedDate = :LastModifiedDate
                               WHERE RiskAssessmentID = :RiskAssessmentID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RiskAssessmentID", OracleType.Number).Value = riskAssessmentId;

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
