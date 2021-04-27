using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.HealthSafety.Common
{
    public class RiskFactor : BaseDbObject, IDropDownItem
    {
        private int riskFactorId;
        private string riskFactorName;
        private int seq;
        private bool allowAddManually;        
        private List<Hazard> hazards;

        public int RiskFactorId
        {
            get { return riskFactorId; }
            set { riskFactorId = value; }
        }

        public string RiskFactorName
        {
            get { return riskFactorName; }
            set { riskFactorName = value; }
        }

        public int Seq
        {
            get { return seq; }
            set { seq = value; }
        }

        public bool AllowAddManually
        {
            get { return allowAddManually; }
            set { allowAddManually = value; }
        }

        public List<Hazard> Hazards
        {
            get
            {
                if (hazards == null)
                    hazards = HazardUtil.GetAllHazardsByFactor(riskFactorId, CurrentUser);

                return hazards;
            }
            set
            {
                hazards = value;
            }
        }

        public bool CanDelete
        {
            get
            {
                if (Hazards.Count > 0)
                    return false;
                else
                    return true;
            }
        }

        //IDropDownItem Members
        public string Text()
        {
            return riskFactorName;
        }

        public string Value()
        {
            return riskFactorId.ToString();
        }

        public RiskFactor(int riskFactorId, string riskFactorName, int seq, bool allowAddManually, User user)
            :base(user)
        {
            this.riskFactorId = riskFactorId;
            this.riskFactorName = riskFactorName;
            this.seq = seq;
            this.allowAddManually = allowAddManually;
        }

        public RiskFactor(User user)
            :base(user)
        {
        }  
    }

    public static class RiskFactorUtil
    {
        public static RiskFactor GetRiskFactor(int riskFactorId, User currentUser)
        {
            RiskFactor riskFactor = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.RiskFactorID as RiskFactorID, a.RiskFactorName as RiskFactorName, a.Seq as Seq, a.AllowAddManually as AllowAddManually
                               FROM PMIS_HS.RiskFactors a                       
                               WHERE a.RiskFactorID = :RiskFactorID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RiskFactorID", OracleType.Number).Value = riskFactorId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    riskFactor = new RiskFactor(currentUser);
                    riskFactor.RiskFactorId = riskFactorId;
                    riskFactor.RiskFactorName = dr["RiskFactorName"].ToString();
                    riskFactor.Seq = DBCommon.GetInt(dr["Seq"]);
                    riskFactor.AllowAddManually = DBCommon.GetInt(dr["AllowAddManually"]) == 1;
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return riskFactor;
        }

        public static List<RiskFactor> GetAllRiskFactors(User currentUser)
        {
            List<RiskFactor> riskFactors = new List<RiskFactor>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT  a.RiskFactorID as RiskFactorID, a.RiskFactorName as RiskFactorName, a.Seq as Seq, a.AllowAddManually as AllowAddManually
                               FROM PMIS_HS.RiskFactors a
                               ORDER BY a.Seq";

                OracleCommand cmd = new OracleCommand(SQL, conn);                

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["RiskFactorID"]))
                        riskFactors.Add(new RiskFactor(DBCommon.GetInt(dr["RiskFactorID"]), dr["RiskFactorName"].ToString(), DBCommon.GetInt(dr["Seq"]), DBCommon.GetInt(dr["AllowAddManually"]) == 1, currentUser));                  
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return riskFactors;
        }

        public static List<RiskFactor> GetAllRiskFactorsByType(int riskFactorTypeId, User currentUser)
        {
            List<RiskFactor> riskFactors = new List<RiskFactor>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT  a.RiskFactorID as RiskFactorID, a.RiskFactorName as RiskFactorName, a.Seq as Seq, a.AllowAddManually as AllowAddManually
                               FROM PMIS_HS.RiskFactors a
                               WHERE a.RiskFactorTypeID = :RiskFactorTypeID
                               ORDER BY a.Seq";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RiskFactorTypeID", OracleType.Number).Value = riskFactorTypeId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["RiskFactorID"]))
                        riskFactors.Add(new RiskFactor(DBCommon.GetInt(dr["RiskFactorID"]), dr["RiskFactorName"].ToString(), DBCommon.GetInt(dr["Seq"]), DBCommon.GetInt(dr["AllowAddManually"]) == 1, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return riskFactors;
        }

        public static bool SaveRiskFactor(int riskFactorTypeID, RiskFactor riskFactor, User currentUser, Change changeEntry)
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
                if (riskFactor.RiskFactorId == 0)
                {
                    SQL += @"INSERT INTO PMIS_HS.RiskFactors (RiskFactorTypeID, RiskFactorName, Seq, AllowAddManually)
                            VALUES (:RiskFactorTypeID, :RiskFactorName, :Seq, :AllowAddManually);

                            SELECT PMIS_HS.RiskFactors_ID_SEQ.currval INTO :RiskFactorID FROM dual;

                            ";

                    changeEvent = new ChangeEvent("HS_Lists_RiskFactors_AddRiskFactor", "Вид фактор: " + RiskFactorTypeUtil.GetRiskFactorType(riskFactorTypeID, currentUser).RiskFactorTypeName, null, null, currentUser);
                    
                    changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_RiskFactors_RiskFactorName", "", riskFactor.RiskFactorName, currentUser));                    
                    changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_RiskFactors_Seq", "", riskFactor.Seq.ToString(), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_RiskFactors_AllowAddManually", "", riskFactor.AllowAddManually ? "1" : "0", currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_HS.RiskFactors SET
                               RiskFactorTypeID = :RiskFactorTypeID,
                               RiskFactorName = :RiskFactorName,
                               Seq = :Seq,
                               AllowAddManually = :AllowAddManually
                            WHERE RiskFactorID = :RiskFactorID ;                            

                            ";

                    changeEvent = new ChangeEvent("HS_Lists_RiskFactors_EditRiskFactor", "Вид фактор: " + RiskFactorTypeUtil.GetRiskFactorType(riskFactorTypeID, currentUser).RiskFactorTypeName, null, null, currentUser);

                    RiskFactor oldRiskFactor = RiskFactorUtil.GetRiskFactor(riskFactor.RiskFactorId, currentUser);

                    if (oldRiskFactor.RiskFactorName.Trim() != riskFactor.RiskFactorName.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_RiskFactors_RiskFactorName", oldRiskFactor.RiskFactorName, riskFactor.RiskFactorName, currentUser));

                    if (oldRiskFactor.Seq != riskFactor.Seq)
                        changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_RiskFactors_Seq", oldRiskFactor.Seq.ToString(), riskFactor.Seq.ToString(), currentUser));

                    if (oldRiskFactor.AllowAddManually != riskFactor.AllowAddManually)
                        changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_RiskFactors_AllowAddManually", oldRiskFactor.AllowAddManually ? "1" : "0", riskFactor.AllowAddManually ? "1" : "0", currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramRiskFactorID = new OracleParameter();
                paramRiskFactorID.ParameterName = "RiskFactorID";
                paramRiskFactorID.OracleType = OracleType.Number;

                if (riskFactor.RiskFactorId != 0)
                {
                    paramRiskFactorID.Direction = ParameterDirection.Input;
                    paramRiskFactorID.Value = riskFactor.RiskFactorId;
                }
                else
                {
                    paramRiskFactorID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramRiskFactorID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "RiskFactorTypeID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = riskFactorTypeID;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "RiskFactorName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(riskFactor.RiskFactorName))
                    param.Value = riskFactor.RiskFactorName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Seq";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = riskFactor.Seq;                
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AllowAddManually";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = riskFactor.AllowAddManually ? 1 : 0;
                cmd.Parameters.Add(param); 

                cmd.ExecuteNonQuery();

                if (riskFactor.RiskFactorId == 0)
                {
                    riskFactor.RiskFactorId = DBCommon.GetInt(paramRiskFactorID.Value);
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

        public static bool DeleteRiskFactor(int riskFactorTypeID, int riskFactorId, User currentUser, Change changeEntry)
        {
            bool result = false;

            RiskFactor oldRiskFactor = RiskFactorUtil.GetRiskFactor(riskFactorId, currentUser);

            ChangeEvent changeEvent = new ChangeEvent("HS_Lists_RiskFactors_DeleteRiskFactor", "Вид фактор: " + RiskFactorTypeUtil.GetRiskFactorType(riskFactorTypeID, currentUser).RiskFactorTypeName, null, null, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_RiskFactors_RiskFactorName", oldRiskFactor.RiskFactorName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_RiskFactors_Seq", oldRiskFactor.Seq.ToString(), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_RiskFactors_AllowAddManually", oldRiskFactor.AllowAddManually ? "1" : "0", "", currentUser));
            
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" BEGIN
                                DELETE FROM PMIS_HS.Hazards
                                WHERE RiskFactorID = :RiskFactorID;

                                DELETE FROM PMIS_HS.RiskFactors WHERE RiskFactorID = :RiskFactorID;                                                                
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RiskFactorID", OracleType.Number).Value = riskFactorId;

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