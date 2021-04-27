using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.HealthSafety.Common
{
    public class RiskCard : BaseDbObject
    {
        private int riskCardId;
        private int positionId;
        private Position position;
        private List<RiskCardItem> riskCardItems;

        public int RiskCardId
        {
            get { return riskCardId; }
            set { riskCardId = value; }
        }

        public int PositionId
        {
            get { return positionId; }
            set { positionId = value; }
        }

        public Position Position
        {
            get
            {
                if (position == null)
                    position = PositionUtil.GetPosition(positionId, CurrentUser);

                return position;
            }
            set
            {
                position = value;
            }
        }

        public List<RiskCardItem> RiskCardItems
        {
            get
            {
                if (riskCardItems == null)
                    riskCardItems = RiskCardItemUtil.GetAllRiskCardItemsByPosition(positionId, CurrentUser);

                return riskCardItems;
            }
            set
            {
                riskCardItems = value;
            }
        }

        public bool CanDelete
        {
            get
            {
                if (RiskCardItems.Count > 0)
                    return false;
                else
                    return true;
            }
        }

        public RiskCard(User user)
            :base(user)
        {
        }  
    }

    public static class RiskCardUtil
    {
        private static RiskCard ExtractRiskCardFromDR(OracleDataReader dr, User currentUser)
        {
            RiskCard riskCard = new RiskCard(currentUser);

            riskCard.RiskCardId = DBCommon.GetInt(dr["RiskCardID"]);
            riskCard.PositionId = DBCommon.GetInt(dr["PositionID"]);

            BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, riskCard);

            return riskCard;
        }

        public static RiskCard GetRiskCard(int riskCardId, User currentUser)
        {
            RiskCard riskCard = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.RiskCardID, a.PositionID, a.CreatedBy, a.CreatedDate, 
                                a.LastModifiedBy, a.LastModifiedDate 
                               FROM PMIS_HS.RiskCards a WHERE a.RiskCardID = :RiskCardID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RiskCardID", OracleType.Number).Value = riskCardId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    riskCard = ExtractRiskCardFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return riskCard;
        }

        public static RiskCard GetRiskCardsByPosition(int positionId, User currentUser)
        {
            RiskCard riskCard = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.RiskCardID, a.PositionID, a.CreatedBy, a.CreatedDate,
                                a.LastModifiedBy, a.LastModifiedDate
                               FROM PMIS_HS.RiskCards a
                               WHERE a.PositionID = :PositionID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PositionID", OracleType.Number).Value = positionId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    riskCard = ExtractRiskCardFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return riskCard;
        }

        public static bool SaveRiskCard(RiskCard riskCard, User currentUser)
        {
            bool result = false;

            string SQL = "";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";
                if (riskCard.RiskCardId == 0)
                {
                    SQL += @"INSERT INTO PMIS_HS.RiskCards (PositionID, CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate)
                            VALUES (:PositionID, :CreatedBy, :CreatedDate, :LastModifiedBy, :LastModifiedDate);

                            SELECT PMIS_HS.RiskCards_ID_SEQ.currval INTO :RiskCardID FROM dual;

                            ";
                }
                else
                {
                    SQL += @"UPDATE PMIS_HS.RiskCards SET
                               PositionID = :RiskFactorTypeID,
                               RiskFactorName = :RiskFactorName,
                               CreatedBy = :CreatedBy,
                               CreatedDate = :CreatedDate,
                               LastModifiedBy = :LastModifiedBy,
                               LastModifiedDate = :LastModifiedDate
                            WHERE RiskFactorID = :RiskFactorID ;                            

                            ";
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramRiskCardID = new OracleParameter();
                paramRiskCardID.ParameterName = "RiskCardID";
                paramRiskCardID.OracleType = OracleType.Number;

                if (riskCard.RiskCardId != 0)
                {
                    paramRiskCardID.Direction = ParameterDirection.Input;
                    paramRiskCardID.Value = riskCard.RiskCardId;
                }
                else
                {
                    paramRiskCardID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramRiskCardID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "PositionID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = riskCard.PositionId;
                cmd.Parameters.Add(param);

                if (riskCard.RiskCardId == 0)
                {
                    BaseDbObjectUtil.SetCreatedParams(cmd, currentUser);
                }

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();

                if (riskCard.RiskCardId == 0)
                {
                    riskCard.RiskCardId = DBCommon.GetInt(paramRiskCardID.Value);
                }

                result = true;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public static bool DeleteRiskCard(int riskCardId, User currentUser)
        {
            bool result = false;
            
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = "DELETE FROM PMIS_HS.RiskCards WHERE RiskCardID = :RiskCardID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RiskCardID", OracleType.Number).Value = riskCardId;

                result = cmd.ExecuteNonQuery() == 1;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

//        public static decimal GetRiskCardMaxHazardValueByPosition(int positionId, User currentUser)
//        {
//            decimal maxHazardValue = 0;

//            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
//            conn.Open();

//            try
//            {
//                string SQL = @"SELECT MAX(c.ProbabilityFactor*d.ExposureFactor*e.EffectweightFactor) as MaxHazardValue
//                               FROM PMIS_HS.RiskCards a
//                               INNER JOIN PMIS_HS.RiskCardItems b ON a.RiskCardID = b.RiskCardID
//                               INNER JOIN PMIS_HS.Probabilities c ON b.ProbabilityID = c.ProbabilityID
//                               INNER JOIN PMIS_HS.Exposure d ON b.ExposureID = d.ExposureID
//                               INNER JOIN PMIS_HS.EffectWeight e ON b.EffectWeightID = e.EffectWeightID
//                               LEFT JOIN PMIS_HS.Hazards f ON b.HazardID = f.HazardID
//                               INNER JOIN PMIS_HS.RiskFactors g ON b.RiskFactorID = g.RiskFactorID
//                               WHERE a.PositionID = :PositionID";

//                OracleCommand cmd = new OracleCommand(SQL, conn);

//                cmd.Parameters.Add("PositionID", OracleType.Number).Value = positionId;

//                OracleDataReader dr = cmd.ExecuteReader();

//                while (dr.Read())
//                {
//                    maxHazardValue = (int)Math.Round(DBCommon.GetDecimal(dr["MaxHazardValue"]), MidpointRounding.AwayFromZero);
//                }

//                dr.Close();
//            }
//            finally
//            {
//                conn.Close();
//            }

//            return riskCardItems;
//        }
    }

}