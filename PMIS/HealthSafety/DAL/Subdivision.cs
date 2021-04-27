using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.HealthSafety.Common
{
    public class Subdivision : BaseDbObject
    {
        private int subdivisionId;
        private int? militaryUnitId;
        private MilitaryUnit militaryUnit;
        private string subdivisionName;
        private List<Position> positions;

        public int SubdivisionId
        {
            get { return subdivisionId; }
            set { subdivisionId = value; }
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

        public string SubdivisionName
        {
            get { return subdivisionName; }
            set { subdivisionName = value; }
        }

        public List<Position> Positions
        {
            get
            {
                if (positions == null)
                    positions = PositionUtil.GetAllPositionsBySubdivisionId(subdivisionId, CurrentUser);

                return positions;
            }
            set
            {
                positions = value;
            }
        }

        public bool CanDelete
        {
            get
            {
                if (Positions.Count > 0)
                    return false;
                else
                    return true;
            }

        }

        public Subdivision(User user)
            : base(user)
        {
        }
    }

    public static class SubdivisionUtil
    {
        private static Subdivision ExtractSubdivisionFromDR(OracleDataReader dr, User currentUser)
        {
            Subdivision subdivision = new Subdivision(currentUser);

            subdivision.SubdivisionId = DBCommon.GetInt(dr["SubdivisionID"]);            
            subdivision.MilitaryUnitId = (DBCommon.IsInt(dr["MilitaryUnitID"]) ? (int?)DBCommon.GetInt(dr["MilitaryUnitID"]) : null);
            subdivision.SubdivisionName = dr["SubdivisionName"].ToString();

            BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, subdivision);

            return subdivision;
        }

        public static Subdivision GetSubdivision(int subdivisionId, User currentUser)
        {
            Subdivision subdivision = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role - TO DO
                //UIItem uiItem = UIItemUtil.GetUIItems("HS_SUBDIVISIONS", currentUser, false, currentUser.Role.RoleId, null)[0];
                //if (uiItem.AccessOnlyOwnData)
                //{
                //    where = " AND a.CreatedBy = " + currentUser.UserId.ToString();
                //}

                string SQL = @"SELECT a.SubdivisionID as SubdivisionID,                                     
                                      a.MilitaryUnitID as MilitaryUnitID,
                                      a.SubdivisionName as SubdivisionName,
                                      a.CreatedBy as CreatedBy,
                                      a.CreatedDate as CreatedDate, 
                                      a.LastModifiedBy as LastModifiedBy, 
                                      a.LastModifiedDate as LastModifiedDate
                               FROM PMIS_HS.Subdivisions a
                               WHERE a.SubdivisionID = :SubdivisionID " + where;

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    SQL += @" AND (a.MilitaryUnitID IS NULL OR a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @"))
                            ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("SubdivisionID", OracleType.Number).Value = subdivisionId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    subdivision = ExtractSubdivisionFromDR(dr, currentUser);               
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return subdivision;
        }

        public static List<Subdivision> GetAllSubdivisions(User currentUser)
        {
            List<Subdivision> subdivisions = new List<Subdivision>();

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

                string SQL = @"SELECT a.SubdivisionID as SubdivisionID,                                     
                                      a.MilitaryUnitID as MilitaryUnitID,
                                      a.SubdivisionName as SubdivisionName,
                                      a.CreatedBy as CreatedBy,
                                      a.CreatedDate as CreatedDate, 
                                      a.LastModifiedBy as LastModifiedBy, 
                                      a.LastModifiedDate as LastModifiedDate
                               FROM PMIS_HS.Subdivisions a " + where;

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
                    if (DBCommon.IsInt(dr["SubdivisionID"]))
                        subdivisions.Add(ExtractSubdivisionFromDR(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return subdivisions;
        }

        public static List<Subdivision> GetAllSubdivisionsByMilitaryUnitID(int militaryUnitID, User currentUser)
        {
            List<Subdivision> subdivisions = new List<Subdivision>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                ////Restric the user to access only his own records if this is set for the particular role
                //UIItem uiItem = UIItemUtil.GetUIItems("HS_COMMITTEE", currentUser, false, currentUser.Role.RoleId, null)[0];
                //if (uiItem.AccessOnlyOwnData)
                //{
                //    where = " AND a.CreatedBy = " + currentUser.UserId.ToString();
                //}

                string SQL = @"SELECT a.SubdivisionID as SubdivisionID,                                     
                                      a.MilitaryUnitID as MilitaryUnitID,
                                      a.SubdivisionName as SubdivisionName,
                                      a.CreatedBy as CreatedBy,
                                      a.CreatedDate as CreatedDate, 
                                      a.LastModifiedBy as LastModifiedBy, 
                                      a.LastModifiedDate as LastModifiedDate
                               FROM PMIS_HS.Subdivisions a 
                               WHERE a.MilitaryUnitID = :MilitaryUnitID" + where;

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    where += @" AND (a.MilitaryUnitID IS NULL OR a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @"))
                            ";                    

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitaryUnitID", OracleType.Number).Value = militaryUnitID;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["SubdivisionID"]))
                        subdivisions.Add(ExtractSubdivisionFromDR(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return subdivisions;
        }

        public static void SetSubdivisionModified(int subdivisionId, User currentUser)
        {
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"UPDATE PMIS_HS.Subdivisions SET
                                  LastModifiedBy = :LastModifiedBy,
                                  LastModifiedDate = :LastModifiedDate
                               WHERE SubdivisionID = :SubdivisionID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("SubdivisionID", OracleType.Number).Value = subdivisionId;

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }

        public static bool SaveSubdivision(Subdivision subdivision, User currentUser, Change changeEntry)
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
                if (subdivision.SubdivisionId == 0)
                {
                    SQL += @"INSERT INTO PMIS_HS.Subdivisions (SubdivisionName, MilitaryUnitID,                                      
                                                              CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate)
                            VALUES (:SubdivisionName, :MilitaryUnitID, 
                                    :CreatedBy, :CreatedDate, :LastModifiedBy, :LastModifiedDate);

                            SELECT PMIS_HS.Subdivisions_ID_SEQ.currval INTO :SubdivisionID FROM dual;

                            ";

                    changeEvent = new ChangeEvent("HS_Subdivisions_AddSubdivision", "", subdivision.MilitaryUnit, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("HS_Subdivisions_SubdivisionName", "", subdivision.SubdivisionName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_Subdivisions_MilitaryUnit", "", subdivision.MilitaryUnit != null ? subdivision.MilitaryUnit.DisplayTextForSelection : "", currentUser));                    
                }
                else
                {
                    SQL += @"UPDATE PMIS_HS.Subdivisions SET
                               SubdivisionName = :SubdivisionName, 
                               MilitaryUnitID = :MilitaryUnitID,
                               LastModifiedBy = CASE WHEN :AnyActualChanges = 1 
                                                     THEN :LastModifiedBy
                                                     ELSE LastModifiedBy
                                                END, 
                               LastModifiedDate = CASE WHEN :AnyActualChanges = 1 
                                                       THEN :LastModifiedDate
                                                       ELSE LastModifiedDate
                                                  END
                            WHERE SubdivisionID = :SubdivisionID ;                       

                            ";

                    changeEvent = new ChangeEvent("HS_Subdivisions_EditSubdivision", "", subdivision.MilitaryUnit, null, currentUser);

                    Subdivision oldSubdivision = SubdivisionUtil.GetSubdivision(subdivision.SubdivisionId, currentUser);

                    if (oldSubdivision.SubdivisionName != subdivision.SubdivisionName)
                        changeEvent.AddDetail(new ChangeEventDetail("HS_Subdivisions_SubdivisionName", oldSubdivision.SubdivisionName, subdivision.SubdivisionName, currentUser));

                    if ((oldSubdivision.MilitaryUnit != null ? oldSubdivision.MilitaryUnit.DisplayTextForSelection : "") != (subdivision.MilitaryUnit != null ? subdivision.MilitaryUnit.DisplayTextForSelection : ""))
                        changeEvent.AddDetail(new ChangeEventDetail("HS_Subdivisions_MilitaryUnit", oldSubdivision.MilitaryUnit != null ? oldSubdivision.MilitaryUnit.DisplayTextForSelection : "", subdivision.MilitaryUnit != null ? subdivision.MilitaryUnit.DisplayTextForSelection : "", currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramSubdivisionID = new OracleParameter();
                paramSubdivisionID.ParameterName = "SubdivisionID";
                paramSubdivisionID.OracleType = OracleType.Number;

                if (subdivision.SubdivisionId != 0)
                {
                    paramSubdivisionID.Direction = ParameterDirection.Input;
                    paramSubdivisionID.Value = subdivision.SubdivisionId;
                }
                else
                {
                    paramSubdivisionID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramSubdivisionID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "SubdivisionName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = subdivision.SubdivisionName;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryUnitID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (subdivision.MilitaryUnitId.HasValue)
                    param.Value = subdivision.MilitaryUnitId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                if (subdivision.SubdivisionId == 0)
                {
                    BaseDbObjectUtil.SetCreatedParams(cmd, currentUser);
                }
                else
                {
                    BaseDbObjectUtil.SetAnyActualChanges(cmd, changeEvent);
                }

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();

                if (subdivision.SubdivisionId == 0)
                    subdivision.SubdivisionId = DBCommon.GetInt(paramSubdivisionID.Value);

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

        public static bool DeleteSubdivision(int subdivisionId, User currentUser, Change changeEntry)
        {
            bool result = false;

            Subdivision oldSubdivision = SubdivisionUtil.GetSubdivision(subdivisionId, currentUser);

            ChangeEvent changeEvent = new ChangeEvent("HS_Subdivisions_DeleteSubdivision", "", oldSubdivision.MilitaryUnit, null, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("HS_Subdivisions_SubdivisionName", oldSubdivision.SubdivisionName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_Subdivisions_MilitaryUnit", oldSubdivision.MilitaryUnit != null ? oldSubdivision.MilitaryUnit.DisplayTextForSelection : "", "", currentUser));                    
          
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" BEGIN
                                DELETE FROM PMIS_HS.Positions WHERE SubdivisionID = :SubdivisionID;
                                
                                DELETE FROM PMIS_HS.Subdivisions WHERE SubdivisionID = :SubdivisionID;
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("SubdivisionID", OracleType.Number).Value = subdivisionId;

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