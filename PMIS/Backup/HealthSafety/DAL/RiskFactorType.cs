using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.HealthSafety.Common
{
    public class RiskFactorType : BaseDbObject
    {
        private int riskFactorTypeId;
        private string riskFactorTypeName;
        private int seq;        
        private List<RiskFactor> riskFactors;

        public int RiskFactorTypeId
        {
            get { return riskFactorTypeId; }
            set { riskFactorTypeId = value; }
        }

        public string RiskFactorTypeName
        {
            get { return riskFactorTypeName; }
            set { riskFactorTypeName = value; }
        }

        public int Seq
        {
            get { return seq; }
            set { seq = value; }
        }

        public List<RiskFactor> RiskFactors
        {
            get
            {
                if (riskFactors == null)
                    riskFactors = RiskFactorUtil.GetAllRiskFactorsByType(riskFactorTypeId, CurrentUser);

                return riskFactors;
            }
            set
            {
                riskFactors = value;
            }
        }

        public bool CanDelete
        {
            get
            {
                if (RiskFactors.Count > 0)
                    return false;
                else
                    return true;
            }
        }

        public RiskFactorType(int riskFactorTypeId, string riskFactorTypeName, int seq, User user)
            :base(user)
        {
            this.riskFactorTypeId = riskFactorTypeId;
            this.riskFactorTypeName = riskFactorTypeName;
            this.seq = seq;
        }

        public RiskFactorType(User user)
            :base(user)
        {
        }  
    }

    public static class RiskFactorTypeUtil
    {
        public static RiskFactorType GetRiskFactorType(int riskFactorTypeId, User currentUser)
        {
            RiskFactorType riskFactorType = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.RiskFactorTypeID as RiskFactorTypeID, a.RiskFactorTypeName as RiskFactorTypeName, a.Seq as Seq
                               FROM PMIS_HS.RiskFactorTypes a                       
                               WHERE a.RiskFactorTypeID = :RiskFactorTypeID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RiskFactorTypeID", OracleType.Number).Value = riskFactorTypeId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    riskFactorType = new RiskFactorType(currentUser);
                    riskFactorType.RiskFactorTypeId = riskFactorTypeId;
                    riskFactorType.RiskFactorTypeName = dr["RiskFactorTypeName"].ToString();
                    riskFactorType.Seq = DBCommon.GetInt(dr["Seq"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return riskFactorType;
        }

        public static List<RiskFactorType> GetAllRiskFactorTypes(User currentUser)
        {
            List<RiskFactorType> riskFactorTypes = new List<RiskFactorType>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.RiskFactorTypeID as RiskFactorTypeID, a.RiskFactorTypeName as RiskFactorTypeName, a.Seq as Seq
                               FROM PMIS_HS.RiskFactorTypes a
                               ORDER BY a.Seq";

                OracleCommand cmd = new OracleCommand(SQL, conn);                

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["RiskFactorTypeID"]))
                        riskFactorTypes.Add(new RiskFactorType(DBCommon.GetInt(dr["RiskFactorTypeID"]), dr["RiskFactorTypeName"].ToString(), DBCommon.GetInt(dr["Seq"]), currentUser));                  
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return riskFactorTypes;
        }

        public static bool SaveRiskFactorType(RiskFactorType riskFactorType, User currentUser, Change changeEntry)
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
                if (riskFactorType.RiskFactorTypeId == 0)
                {
                    SQL += @"INSERT INTO PMIS_HS.RiskFactorTypes (RiskFactorTypeName, Seq)
                            VALUES (:RiskFactorTypeName, :Seq);

                            SELECT PMIS_HS.RiskFactorTypes_ID_SEQ.currval INTO :RiskFactorTypeID FROM dual;

                            ";

                    changeEvent = new ChangeEvent("HS_Lists_RiskFactorTypes_AddRiskFactorType", "", null, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_RiskFactorTypes_RiskFactorTypeName", "", riskFactorType.RiskFactorTypeName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_RiskFactorTypes_Seq", "", riskFactorType.Seq.ToString(), currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_HS.RiskFactorTypes SET
                               RiskFactorTypeName = :RiskFactorTypeName,
                               Seq = :Seq
                            WHERE RiskFactorTypeID = :RiskFactorTypeID ;                            

                            ";

                    changeEvent = new ChangeEvent("HS_Lists_RiskFactorTypes_EditRiskFactorType", "", null, null, currentUser);

                    RiskFactorType oldRiskFactorType = RiskFactorTypeUtil.GetRiskFactorType(riskFactorType.RiskFactorTypeId, currentUser);

                    if (oldRiskFactorType.RiskFactorTypeName.Trim() != riskFactorType.RiskFactorTypeName.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_RiskFactorTypes_RiskFactorTypeName", oldRiskFactorType.RiskFactorTypeName, riskFactorType.RiskFactorTypeName, currentUser));

                    if (oldRiskFactorType.Seq != riskFactorType.Seq)
                        changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_RiskFactorTypes_Seq", oldRiskFactorType.Seq.ToString(), riskFactorType.Seq.ToString(), currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramRiskFactorTypeID = new OracleParameter();
                paramRiskFactorTypeID.ParameterName = "RiskFactorTypeID";
                paramRiskFactorTypeID.OracleType = OracleType.Number;

                if (riskFactorType.RiskFactorTypeId!= 0)
                {
                    paramRiskFactorTypeID.Direction = ParameterDirection.Input;
                    paramRiskFactorTypeID.Value = riskFactorType.RiskFactorTypeId;
                }
                else
                {
                    paramRiskFactorTypeID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramRiskFactorTypeID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "RiskFactorTypeName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(riskFactorType.RiskFactorTypeName))
                    param.Value = riskFactorType.RiskFactorTypeName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Seq";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = riskFactorType.Seq;                
                cmd.Parameters.Add(param);              

                cmd.ExecuteNonQuery();

                if (riskFactorType.RiskFactorTypeId == 0)
                {
                    riskFactorType.RiskFactorTypeId = DBCommon.GetInt(paramRiskFactorTypeID.Value);
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

        public static bool DeleteRiskFactorType(int riskFactorTypeId, User currentUser, Change changeEntry)
        {
            bool result = false;

            RiskFactorType oldRiskFactorType = RiskFactorTypeUtil.GetRiskFactorType(riskFactorTypeId, currentUser);

            ChangeEvent changeEvent = new ChangeEvent("HS_Lists_RiskFactorTypes_DeleteRiskFactorType", "", null, null, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_RiskFactorTypes_RiskFactorTypeName", oldRiskFactorType.RiskFactorTypeName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_RiskFactorTypes_Seq", oldRiskFactorType.Seq.ToString(), "", currentUser));
            
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" BEGIN
                                DELETE FROM PMIS_HS.Hazards
                                WHERE HazardID IN (SELECT a.HazardID FROM PMIS_HS.Hazards a
                                                   INNER JOIN PMIS_HS.RiskFactors b ON a.RiskFactorID = b.RiskFactorID
                                                   WHERE b.RiskFactorTypeID = :RiskFactorTypeID);

                                DELETE FROM PMIS_HS.RiskFactors WHERE RiskFactorTypeID = :RiskFactorTypeID;
                                
                                DELETE FROM PMIS_HS.RiskFactorTypes WHERE RiskFactorTypeID = :RiskFactorTypeID;
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RiskFactorTypeID", OracleType.Number).Value = riskFactorTypeId;

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