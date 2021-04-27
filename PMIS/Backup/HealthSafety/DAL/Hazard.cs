using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.HealthSafety.Common
{
    public class Hazard : BaseDbObject
    {
        private int hazardId;
        private string hazardName;
        private int seq;        

        public int HazardId
        {
            get { return hazardId; }
            set { hazardId = value; }
        }

        public string HazardName
        {
            get { return hazardName; }
            set { hazardName = value; }
        }

        public int Seq
        {
            get { return seq; }
            set { seq = value; }
        }       

        public bool CanDelete
        {
            get
            {
                return true;
            }
        }

        public Hazard(int hazardId, string hazardName, int seq, User user)
            :base(user)
        {
            this.hazardId = hazardId;
            this.hazardName = hazardName;
            this.seq = seq;
        }

        public Hazard(User user)
            :base(user)
        {
        }  
    }

    public static class HazardUtil
    {
        public static Hazard GetHazard(int hazardId, User currentUser)
        {
            Hazard hazard = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.HazardID as HazardID, a.HazardName as HazardName, a.Seq as Seq
                               FROM PMIS_HS.Hazards a                       
                               WHERE a.HazardID = :HazardID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("HazardID", OracleType.Number).Value = hazardId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    hazard = new Hazard(currentUser);
                    hazard.HazardId = hazardId;
                    hazard.HazardName = dr["HazardName"].ToString();
                    hazard.Seq = DBCommon.GetInt(dr["Seq"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return hazard;
        }

        public static List<Hazard> GetAllHazards(User currentUser)
        {
            List<Hazard> hazards = new List<Hazard>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.HazardID as HazardID, a.HazardName as HazardName, a.Seq as Seq
                               FROM PMIS_HS.Hazards a  
                               ORDER BY a.Seq";

                OracleCommand cmd = new OracleCommand(SQL, conn);                

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["HazardID"]))
                        hazards.Add(new Hazard(DBCommon.GetInt(dr["RiskFactorID"]), dr["HazardName"].ToString(), DBCommon.GetInt(dr["Seq"]), currentUser));                  
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return hazards;
        }

        public static List<Hazard> GetAllHazardsByFactor(int riskFactorId, User currentUser)
        {
            List<Hazard> hazards = new List<Hazard>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.HazardID as HazardID, a.HazardName as HazardName, a.Seq as Seq
                               FROM PMIS_HS.Hazards a 
                               WHERE a.RiskFactorID = :RiskFactorID 
                               ORDER BY a.Seq";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RiskFactorID", OracleType.Number).Value = riskFactorId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["HazardID"]))
                        hazards.Add(new Hazard(DBCommon.GetInt(dr["HazardID"]), dr["HazardName"].ToString(), DBCommon.GetInt(dr["Seq"]), currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return hazards;
        }

        public static List<Hazard> GetAllHazardsByPositionAndRiskFactor(int positionId, int riskFactorId, User currentUser)
        {
            List<Hazard> hazards = new List<Hazard>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.HazardID as HazardID, a.HazardName as HazardName, a.Seq as Seq
                               FROM PMIS_HS.Hazards a 
                               WHERE a.RiskFactorID = :RiskFactorID 
                             AND a.HazardID NOT IN 
                             (
                                SELECT DISTINCT 
                                CASE WHEN c.HazardID IS NULL
                                     THEN 0
                                     ELSE c.HazardID
                                END as HazardID
                                FROM PMIS_HS.RiskCards b
                                INNER JOIN PMIS_HS.RiskCardItems c ON b.RiskCardID = c.RiskCardID
                                WHERE b.PositionID = :PositionID 
                             )
                             ORDER BY a.Seq";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PositionID", OracleType.Number).Value = positionId;
                cmd.Parameters.Add("RiskFactorID", OracleType.Number).Value = riskFactorId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["HazardID"]))
                        hazards.Add(new Hazard(DBCommon.GetInt(dr["HazardID"]), dr["HazardName"].ToString(), DBCommon.GetInt(dr["Seq"]), currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return hazards;
        }

        public static bool SaveHazard(int riskFactorTypeID, int riskFactorID, Hazard hazard, User currentUser, Change changeEntry)
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
                if (hazard.HazardId == 0)
                {
                    SQL += @"INSERT INTO PMIS_HS.Hazards (RiskFactorID, HazardName, Seq)
                            VALUES (:RiskFactorID, :HazardName, :Seq);

                            SELECT PMIS_HS.Hazards_ID_SEQ.currval INTO :HazardID FROM dual;

                            ";

                    changeEvent = new ChangeEvent("HS_Lists_Hazards_AddHazard", "Вид фактор: " + RiskFactorTypeUtil.GetRiskFactorType(riskFactorTypeID, currentUser).RiskFactorTypeName + "; Фактор: " + RiskFactorUtil.GetRiskFactor(riskFactorID, currentUser).RiskFactorName, null, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_Hazards_HazardName", "", hazard.HazardName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_Hazards_Seq", "", hazard.Seq.ToString(), currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_HS.Hazards SET
                               RiskFactorID = :RiskFactorID,
                               HazardName = :HazardName,
                               Seq = :Seq
                            WHERE HazardID = :HazardID ;                            

                            ";

                    changeEvent = new ChangeEvent("HS_Lists_Hazards_EditHazard", "Вид фактор: " + RiskFactorTypeUtil.GetRiskFactorType(riskFactorTypeID, currentUser).RiskFactorTypeName + "; Фактор: " + RiskFactorUtil.GetRiskFactor(riskFactorID, currentUser).RiskFactorName, null, null, currentUser);

                    Hazard oldHazard = HazardUtil.GetHazard(hazard.HazardId, currentUser);

                    if (oldHazard.HazardName.Trim() != hazard.HazardName.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_Hazards_HazardName", oldHazard.HazardName, hazard.HazardName, currentUser));

                    if (oldHazard.Seq != hazard.Seq)
                        changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_Hazards_Seq", oldHazard.Seq.ToString(), hazard.Seq.ToString(), currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramHazardID = new OracleParameter();
                paramHazardID.ParameterName = "HazardID";
                paramHazardID.OracleType = OracleType.Number;

                if (hazard.HazardId != 0)
                {
                    paramHazardID.Direction = ParameterDirection.Input;
                    paramHazardID.Value = hazard.HazardId;
                }
                else
                {
                    paramHazardID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramHazardID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "RiskFactorID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = riskFactorID;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "HazardName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(hazard.HazardName))
                    param.Value = hazard.HazardName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Seq";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = hazard.Seq;                
                cmd.Parameters.Add(param);              

                cmd.ExecuteNonQuery();

                if (hazard.HazardId == 0)
                {
                    hazard.HazardId = DBCommon.GetInt(paramHazardID.Value);
                }

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

        public static bool DeleteHazard(int riskFactorTypeID, int riskFactorId, int hazardId, User currentUser, Change changeEntry)
        {
            bool result = false;

            Hazard oldHazard = HazardUtil.GetHazard(hazardId, currentUser);

            ChangeEvent changeEvent = new ChangeEvent("HS_Lists_Hazards_DeleteHazard", "Вид фактор: " + RiskFactorTypeUtil.GetRiskFactorType(riskFactorTypeID, currentUser).RiskFactorTypeName + "; Фактор: " + RiskFactorUtil.GetRiskFactor(riskFactorId, currentUser).RiskFactorName, null, null, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_Hazards_HazardName", oldHazard.HazardName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_Hazards_Seq", oldHazard.Seq.ToString(), "", currentUser));
            
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" BEGIN
                                    DELETE FROM PMIS_HS.Hazards
                                    WHERE HazardID = :HazardID;                                                                                               
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("HazardID", OracleType.Number).Value = hazardId;

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