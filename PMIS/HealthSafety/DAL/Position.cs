using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.HealthSafety.Common
{
    public class Position : BaseDbObject
    {
        private int positionId;
        private int subdivisionId;
        private Subdivision subdivision;
        private string positionName;
        private string activities;        
        private int? totalPersonsCnt;        
        private int? femaleCnt;        

        public int PositionId
        {
            get { return positionId; }
            set { positionId = value; }
        }

        public int SubdivisionId
        {
            get { return subdivisionId; }
            set { subdivisionId = value; }
        }

        public Subdivision Subdivision
        {
            get
            {
                if (subdivision == null)
                {
                    subdivision = SubdivisionUtil.GetSubdivision(subdivisionId, CurrentUser);
                }
                return subdivision;
            }
            set
            {
                subdivision = value;
            }
        }

        public string PositionName
        {
            get { return positionName; }
            set { positionName = value; }
        }

        public string Activities
        {
            get { return activities; }
            set { activities = value; }
        }

        public int? TotalPersonsCnt
        {
            get { return totalPersonsCnt; }
            set { totalPersonsCnt = value; }
        }

        public int? FemaleCnt
        {
            get { return femaleCnt; }
            set { femaleCnt = value; }
        }


        public bool CanDelete
        {
            get { return true; }

        }

        public Position(User user)
            : base(user)
        {
        }
    }

    public static class PositionUtil
    {
        private static Position ExtractPositionFromDR(OracleDataReader dr, User currentUser)
        {
            Position position = new Position(currentUser);

            position.PositionId = DBCommon.GetInt(dr["PositionID"]);
            position.SubdivisionId = DBCommon.GetInt(dr["SubdivisionID"]);
            position.PositionName = dr["PositionName"].ToString();
            position.Activities = dr["Activities"].ToString();
            position.TotalPersonsCnt = (DBCommon.IsInt(dr["TotalPersonsCnt"]) ? (int?)DBCommon.GetInt(dr["TotalPersonsCnt"]) : null);
            position.FemaleCnt = (DBCommon.IsInt(dr["FemaleCnt"]) ? (int?)DBCommon.GetInt(dr["FemaleCnt"]) : null);
            
            return position;
        }

        public static Position GetPosition(int positionId, User currentUser)
        {
            Position position = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {                        
                string SQL = @"SELECT a.PositionID as PositionID,                                     
                                      a.SubdivisionID as SubdivisionID,
                                      a.PositionName as PositionName,
                                      a.Activities as Activities,
                                      a.TotalPersonsCnt as TotalPersonsCnt, 
                                      a.FemaleCnt as FemaleCnt
                               FROM PMIS_HS.Positions a
                               WHERE a.PositionID = :PositionID ";                

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PositionID", OracleType.Number).Value = positionId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    position = ExtractPositionFromDR(dr, currentUser);               
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return position;
        }

        public static List<Position> GetAllPositions(User currentUser)
        {
            List<Position> positions = new List<Position>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.PositionID as PositionID,                                     
                                      a.SubdivisionID as SubdivisionID,
                                      a.PositionName as PositionName,
                                      a.Activities as Activities,
                                      a.TotalPersonsCnt as TotalPersonsCnt, 
                                      a.FemaleCnt as FemaleCnt
                               FROM PMIS_HS.Positions a ";               

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["PositionID"]))
                        positions.Add(ExtractPositionFromDR(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return positions;
        }

        public static List<Position> GetAllPositionsBySubdivisionId(int subdivisionId, User currentUser)
        {
            List<Position> positions = new List<Position>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.PositionID as PositionID,                                     
                                      a.SubdivisionID as SubdivisionID,
                                      a.PositionName as PositionName,
                                      a.Activities as Activities,
                                      a.TotalPersonsCnt as TotalPersonsCnt, 
                                      a.FemaleCnt as FemaleCnt
                               FROM PMIS_HS.Positions a 
                               WHERE a.SubdivisionID = :SubdivisionID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("SubdivisionID", OracleType.Number).Value = subdivisionId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["PositionID"]))
                        positions.Add(ExtractPositionFromDR(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return positions;
        }

        public static bool SavePosition(int subdivisionId, Position position, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            Subdivision subdivision = SubdivisionUtil.GetSubdivision(subdivisionId, currentUser);

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";
                if (position.PositionId == 0)
                {
                    SQL += @"INSERT INTO PMIS_HS.Positions (SubdivisionID, PositionName, Activities, TotalPersonsCnt, FemaleCnt)
                            VALUES (:SubdivisionID, :PositionName, :Activities, :TotalPersonsCnt, :FemaleCnt);

                            SELECT PMIS_HS.Positions_ID_SEQ.currval INTO :PositionID FROM dual;

                            ";

                    changeEvent = new ChangeEvent("HS_Positions_AddPosition", subdivision.SubdivisionName, subdivision.MilitaryUnit, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("HS_Positions_PositionName", "", position.PositionName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_Positions_Activities", "", position.Activities, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_Positions_TotalPersonsCnt", "", position.TotalPersonsCnt.ToString(), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_Positions_FemaleCnt", "", position.FemaleCnt.ToString(), currentUser));
                    
                }
                else
                {
                    SQL += @"UPDATE PMIS_HS.Positions SET
                               SubdivisionID = :SubdivisionID, 
                               PositionName = :PositionName,
                               Activities = :Activities, 
                               TotalPersonsCnt = :TotalPersonsCnt,
                               FemaleCnt = :FemaleCnt
                            WHERE PositionID = :PositionID ;                       

                            ";

                    changeEvent = new ChangeEvent("HS_Positions_EditPosition", subdivision.SubdivisionName, subdivision.MilitaryUnit, null, currentUser);

                    Position oldPosition = PositionUtil.GetPosition(position.PositionId, currentUser);

                    if (oldPosition.PositionName != position.PositionName)
                        changeEvent.AddDetail(new ChangeEventDetail("HS_Positions_PositionName", oldPosition.PositionName, position.PositionName, currentUser));

                    if (oldPosition.Activities != position.Activities)
                        changeEvent.AddDetail(new ChangeEventDetail("HS_Positions_Activities", oldPosition.Activities, position.Activities, currentUser));

                    if (oldPosition.TotalPersonsCnt != position.TotalPersonsCnt)
                        changeEvent.AddDetail(new ChangeEventDetail("HS_Positions_TotalPersonsCnt", oldPosition.TotalPersonsCnt.ToString(), position.TotalPersonsCnt.ToString(), currentUser));

                    if (oldPosition.FemaleCnt != position.FemaleCnt)
                        changeEvent.AddDetail(new ChangeEventDetail("HS_Positions_FemaleCnt", oldPosition.FemaleCnt.ToString(), position.FemaleCnt.ToString(), currentUser));
                    
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramPositionID = new OracleParameter();
                paramPositionID.ParameterName = "PositionID";
                paramPositionID.OracleType = OracleType.Number;

                if (position.PositionId != 0)
                {
                    paramPositionID.Direction = ParameterDirection.Input;
                    paramPositionID.Value = position.PositionId;
                }
                else
                {
                    paramPositionID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramPositionID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "SubdivisionID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = subdivisionId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PositionName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = position.PositionName;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Activities";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = position.Activities;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "TotalPersonsCnt";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (position.TotalPersonsCnt.HasValue)
                    param.Value = position.TotalPersonsCnt.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "FemaleCnt";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (position.FemaleCnt.HasValue)
                    param.Value = position.FemaleCnt.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);                

                cmd.ExecuteNonQuery();

                SubdivisionUtil.SetSubdivisionModified(subdivisionId, currentUser);

                if (subdivision.SubdivisionId == 0)
                    subdivision.SubdivisionId = DBCommon.GetInt(paramPositionID.Value);

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

        public static bool DeletePosition(int subdivisionId, int positionId, User currentUser, Change changeEntry)
        {
            bool result = false;

            Subdivision subdivision = SubdivisionUtil.GetSubdivision(subdivisionId, currentUser);
            Position oldPosition = PositionUtil.GetPosition(positionId, currentUser);

            ChangeEvent changeEvent = new ChangeEvent("HS_Positions_DeletePosition", subdivision.SubdivisionName, subdivision.MilitaryUnit, null, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("HS_Positions_PositionName", oldPosition.PositionName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_Positions_Activities", oldPosition.Activities, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_Positions_TotalPersonsCnt", oldPosition.TotalPersonsCnt.ToString(), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_Positions_FemaleCnt", oldPosition.FemaleCnt.ToString(), "", currentUser));
    
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" BEGIN
                                    DELETE FROM PMIS_HS.RiskCardItems 
                                    WHERE RiskCardID IN (SELECT RiskCardID 
                                                         FROM PMIS_HS.RiskCards 
                                                         WHERE PositionID = :PositionID);

                                    DELETE FROM PMIS_HS.RiskCards 
                                    WHERE PositionID = :PositionID;

                                    DELETE FROM PMIS_HS.Positions WHERE PositionID = :PositionID;
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PositionID", OracleType.Number).Value = positionId;

                result = cmd.ExecuteNonQuery() == 1;

                if (result)
                {
                    SubdivisionUtil.SetSubdivisionModified(subdivisionId, currentUser);
                }
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