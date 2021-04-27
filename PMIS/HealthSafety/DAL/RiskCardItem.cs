using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.HealthSafety.Common
{
    public class RiskCardItem : BaseDbObject
    {
        private int riskCardItemId;
        private int riskCardId;
        private int riskFactorTypeId;
        private int riskFactorId;
        private int riskFactorSeq;
        private int? hazardId;
        private string hazardName;
        private string otherHazard;
        private int? hazardSeq;
        private int probabilityId;
        private string probabilityName;
        private decimal probabilityFactor;
        private int exposureId;
        private string exposureName;
        private decimal exposureFactor;
        private int effectWeightId;
        private string effectWeightName;
        private decimal effectWeightFactor;

        private int hazardValue;
        private string riskRank;
        private string riskRankName;

        public int RiskCardItemId
        {
            get { return riskCardItemId; }
            set { riskCardItemId = value; }
        }

        public int RiskCardId
        {
            get { return riskCardId; }
            set { riskCardId = value; }
        }

        public int RiskFactorTypeId
        {
            get { return riskFactorTypeId; }
            set { riskFactorTypeId = value; }
        }

        public int RiskFactorId
        {
            get { return riskFactorId; }
            set { riskFactorId = value; }
        }

        public int RiskFactorSeq
        {
            get { return riskFactorSeq; }
            set { riskFactorSeq = value; }
        }

        public int? HazardId
        {
            get { return hazardId; }
            set { hazardId = value; }
        }

        public string HazardName
        {
            get { return hazardName; }
            set { hazardName = value; }
        }

        public string OtherHazard
        {
            get { return otherHazard; }
            set { otherHazard = value; }
        }

        public int? HazardSeq
        {
            get { return hazardSeq; }
            set { hazardSeq = value; }
        }

        public int ProbabilityId
        {
            get { return probabilityId; }
            set { probabilityId = value; }
        }

        public string ProbabilityName
        {
            get { return probabilityName; }
            set { probabilityName = value; }
        }

        public decimal ProbabilityFactor
        {
            get { return probabilityFactor; }
            set { probabilityFactor = value; }
        }

        public int ExposureId
        {
            get { return exposureId; }
            set { exposureId = value; }
        }

        public string ExposureName
        {
            get { return exposureName; }
            set { exposureName = value; }
        }

        public decimal ExposureFactor
        {
            get { return exposureFactor; }
            set { exposureFactor = value; }
        }

        public int EffectWeightId
        {
            get { return effectWeightId; }
            set { effectWeightId = value; }
        }

        public string EffectWeightName
        {
            get { return effectWeightName; }
            set { effectWeightName = value; }
        }

        public decimal EffectWeightFactor
        {
            get { return effectWeightFactor; }
            set { effectWeightFactor = value; }
        }

        public int HazardValue
        {
            get { return hazardValue; }
            set { hazardValue = value; }
        }

        public string RiskRank
        {
            get { return riskRank; }
            set { riskRank = value; }
        }

        public string RiskRankName
        {
            get { return riskRankName; }
            set { riskRankName = value; }
        }

        public RiskCardItem(User user)
            :base(user)
        {
        }  
    }

    public static class RiskCardItemUtil
    {
        private static RiskCardItem ExtractRiskCardItemFromDR(OracleDataReader dr, User currentUser)
        {
            RiskCardItem riskCardItem = new RiskCardItem(currentUser);

            riskCardItem.RiskCardItemId = DBCommon.GetInt(dr["RiskCardItemID"]);
            riskCardItem.RiskCardId = DBCommon.GetInt(dr["RiskCardID"]);
            riskCardItem.RiskFactorTypeId = DBCommon.GetInt(dr["RiskFactorTypeID"]);
            riskCardItem.RiskFactorId = DBCommon.GetInt(dr["RiskFactorID"]);
            riskCardItem.RiskFactorSeq = DBCommon.GetInt(dr["RiskFactorSeq"]);

            if (DBCommon.IsInt(dr["HazardID"]))
                riskCardItem.HazardId = DBCommon.GetInt(dr["HazardID"]);
            
            riskCardItem.HazardName = dr["HazardName"].ToString();
            riskCardItem.OtherHazard = dr["OtherHazard"].ToString();

            if (DBCommon.IsInt(dr["HazardSeq"]))
                riskCardItem.HazardSeq = DBCommon.GetInt(dr["HazardSeq"]);

            riskCardItem.ProbabilityId = DBCommon.GetInt(dr["ProbabilityID"]);
            riskCardItem.ProbabilityName = dr["ProbabilityName"].ToString();
            riskCardItem.ProbabilityFactor = DBCommon.GetDecimal(dr["ProbabilityFactor"]);
            riskCardItem.ExposureId = DBCommon.GetInt(dr["ExposureID"]);
            riskCardItem.ExposureName = dr["ExposureName"].ToString();
            riskCardItem.ExposureFactor = DBCommon.GetDecimal(dr["ExposureFactor"]);
            riskCardItem.EffectWeightId = DBCommon.GetInt(dr["EffectWeightID"]);
            riskCardItem.EffectWeightName = dr["EffectWeightName"].ToString();
            riskCardItem.EffectWeightFactor = DBCommon.GetDecimal(dr["EffectWeightFactor"]);

            riskCardItem.HazardValue = (int)Math.Round(DBCommon.GetDecimal(dr["HazardValue"]), MidpointRounding.AwayFromZero);
            riskCardItem.RiskRank = dr["RiskRank"].ToString();
            riskCardItem.RiskRankName = dr["RiskRankName"].ToString();

            return riskCardItem;
        }

        public static RiskCardItem GetRiskCardItem(int riskCardItemId, User currentUser)
        {
            RiskCardItem riskCardItem = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT b.RiskCardItemID, b.RiskCardID, b.RiskFactorTypeID, b.RiskFactorID, g.Seq as RiskFactorSeq,
                                f.HazardID, f.HazardName, f.Seq as HazardSeq, b.OtherHazard, c.ProbabilityID, c.ProbabilityName, c.ProbabilityFactor as ProbabilityFactor, 
                                d.ExposureID, d.ExposureName, d.ExposureFactor as ExposureFactor, 
                                e.EffectWeightID, e.EffectWeightName, e.EffectWeightFactor as EffectWeightFactor,
                                c.ProbabilityFactor*d.ExposureFactor*e.EffectweightFactor as HazardValue,
                                (SELECT RiskRank FROM PMIS_HS.RiskRank WHERE c.ProbabilityFactor*d.ExposureFactor*e.EffectweightFactor >= RiskRankFrom and c.ProbabilityFactor*d.ExposureFactor*e.EffectweightFactor <= RiskRankTo AND rownum = 1) as RiskRank,
                                (SELECT RiskRankName FROM PMIS_HS.RiskRank WHERE c.ProbabilityFactor*d.ExposureFactor*e.EffectweightFactor >= RiskRankFrom and c.ProbabilityFactor*d.ExposureFactor*e.EffectweightFactor <= RiskRankTo AND rownum = 1) as RiskRankName
                               FROM PMIS_HS.RiskCards a
                               INNER JOIN PMIS_HS.RiskCardItems b ON a.RiskCardID = b.RiskCardID
                               INNER JOIN PMIS_HS.Probabilities c ON b.ProbabilityID = c.ProbabilityID
                               INNER JOIN PMIS_HS.Exposure d ON b.ExposureID = d.ExposureID
                               INNER JOIN PMIS_HS.EffectWeight e ON b.EffectWeightID = e.EffectWeightID
                               LEFT JOIN PMIS_HS.Hazards f ON b.HazardID = f.HazardID
                               INNER JOIN PMIS_HS.RiskFactors g ON b.RiskFactorID = g.RiskFactorID
                               WHERE b.RiskCardItemID = :RiskCardItemID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RiskCardItemID", OracleType.Number).Value = riskCardItemId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    riskCardItem = ExtractRiskCardItemFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return riskCardItem;
        }

        public static List<RiskCardItem> GetAllRiskCardItemsByPosition(int positionId, User currentUser)
        {
            List<RiskCardItem> riskCardItems = new List<RiskCardItem>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT b.RiskCardItemID, b.RiskCardID, b.RiskFactorTypeID, b.RiskFactorID, g.Seq as RiskFactorSeq,
                                f.HazardID, f.HazardName, f.Seq as HazardSeq, b.OtherHazard, c.ProbabilityID, c.ProbabilityName, c.ProbabilityFactor as ProbabilityFactor, 
                                d.ExposureID, d.ExposureName, d.ExposureFactor as ExposureFactor, 
                                e.EffectWeightID, e.EffectWeightName, e.EffectWeightFactor as EffectWeightFactor,
                                c.ProbabilityFactor*d.ExposureFactor*e.EffectweightFactor as HazardValue,
                                (SELECT RiskRank FROM PMIS_HS.RiskRank WHERE c.ProbabilityFactor*d.ExposureFactor*e.EffectweightFactor >= RiskRankFrom and c.ProbabilityFactor*d.ExposureFactor*e.EffectweightFactor <= RiskRankTo AND rownum = 1) as RiskRank,
                                (SELECT RiskRankName FROM PMIS_HS.RiskRank WHERE c.ProbabilityFactor*d.ExposureFactor*e.EffectweightFactor >= RiskRankFrom and c.ProbabilityFactor*d.ExposureFactor*e.EffectweightFactor <= RiskRankTo AND rownum = 1) as RiskRankName
                               FROM PMIS_HS.RiskCards a
                               INNER JOIN PMIS_HS.RiskCardItems b ON a.RiskCardID = b.RiskCardID
                               INNER JOIN PMIS_HS.Probabilities c ON b.ProbabilityID = c.ProbabilityID
                               INNER JOIN PMIS_HS.Exposure d ON b.ExposureID = d.ExposureID
                               INNER JOIN PMIS_HS.EffectWeight e ON b.EffectWeightID = e.EffectWeightID
                               LEFT JOIN PMIS_HS.Hazards f ON b.HazardID = f.HazardID
                               INNER JOIN PMIS_HS.RiskFactors g ON b.RiskFactorID = g.RiskFactorID
                               WHERE a.PositionID = :PositionID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PositionID", OracleType.Number).Value = positionId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    riskCardItems.Add(ExtractRiskCardItemFromDR(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return riskCardItems;
        }

        public static bool SaveRiskCardItem(RiskCardItem riskCardItem, Position position, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string logDesc = "";

                logDesc += "Подразделение/обект: " + position.Subdivision.SubdivisionName + ";<br />";

                logDesc += "Длъжност: " + position.PositionName + ";<br />";

                RiskFactorType riskFactorType = RiskFactorTypeUtil.GetRiskFactorType(riskCardItem.RiskFactorTypeId, currentUser);

                logDesc += "Тип фактор: " + riskFactorType.RiskFactorTypeName + ";<br />";

                RiskFactor riskFactor = RiskFactorUtil.GetRiskFactor(riskCardItem.RiskFactorId, currentUser);

                logDesc += "Фактор: " + riskFactor.RiskFactorName + ";<br />";

                Hazard hazard = null;
                if (riskCardItem.HazardId.HasValue)
                {
                    hazard = HazardUtil.GetHazard(riskCardItem.HazardId.Value, currentUser);
                    logDesc += "Опасност: " + hazard.HazardName + ";";
                }

                SQL = @"BEGIN
                        
                       ";
                if (riskCardItem.RiskCardItemId == 0)
                {
                    SQL += @"INSERT INTO PMIS_HS.RiskCardItems (RiskCardID, RiskFactorTypeID, RiskFactorID, HazardID,
								   OtherHazard, ProbabilityID, ExposureID, EffectWeightID)
                            VALUES (:RiskCardID, :RiskFactorTypeID, :RiskFactorID, :HazardID,
								   :OtherHazard, :ProbabilityID, :ExposureID, :EffectWeightID);

                            SELECT PMIS_HS.RiskCardItems_ID_SEQ.currval INTO :RiskCardItemID FROM dual;

                            ";

                    changeEvent = new ChangeEvent("HS_RiskCard_AddRiskCardItem", logDesc, position.Subdivision.MilitaryUnit, null, currentUser);

                    if (!String.IsNullOrEmpty(riskCardItem.OtherHazard))
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("HS_RiskCard_OtherHazard", "", riskCardItem.OtherHazard, currentUser));   
                    }
                    
                    changeEvent.AddDetail(new ChangeEventDetail("HS_RiskCard_Probability", "", ProbabilityUtil.GetProbability(riskCardItem.ProbabilityId, currentUser).ProbabilityName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_RiskCard_Exposure", "", ExposureUtil.GetExposure(riskCardItem.ExposureId, currentUser).ExposureName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_RiskCard_EffectWeight", "", EffectWeightUtil.GetEffectWeight(riskCardItem.EffectWeightId, currentUser).EffectWeightName, currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_HS.RiskCardItems SET
                                HazardID = :HazardID,
                                OtherHazard = :OtherHazard,
                                ProbabilityID = :ProbabilityID,
                                ExposureID = :ExposureID,
                                EffectWeightID = :EffectWeightID
                            WHERE RiskCardItemID = :RiskCardItemID ;                            

                            ";

                    changeEvent = new ChangeEvent("HS_RiskCard_EditRiskCardItem", logDesc, position.Subdivision.MilitaryUnit, null, currentUser);

                    RiskCardItem oldRiskCardItem = RiskCardItemUtil.GetRiskCardItem(riskCardItem.RiskCardItemId, currentUser);

                    oldRiskCardItem.ProbabilityName = ProbabilityUtil.GetProbability(riskCardItem.ProbabilityId, currentUser).ProbabilityName;
                    oldRiskCardItem.ExposureName = ExposureUtil.GetExposure(riskCardItem.ExposureId, currentUser).ExposureName;
                    oldRiskCardItem.EffectWeightName = EffectWeightUtil.GetEffectWeight(riskCardItem.EffectWeightId, currentUser).EffectWeightName;

                    if (oldRiskCardItem.OtherHazard != riskCardItem.OtherHazard)
                        changeEvent.AddDetail(new ChangeEventDetail("HS_RiskCard_OtherHazard", oldRiskCardItem.OtherHazard, riskCardItem.OtherHazard, currentUser));

                    if (oldRiskCardItem.ProbabilityId != riskCardItem.ProbabilityId)
                        changeEvent.AddDetail(new ChangeEventDetail("HS_RiskCard_Probability", oldRiskCardItem.ProbabilityName, riskCardItem.ProbabilityName, currentUser));

                    if (oldRiskCardItem.ExposureId != riskCardItem.ExposureId)
                        changeEvent.AddDetail(new ChangeEventDetail("HS_RiskCard_Exposure", oldRiskCardItem.ExposureName, riskCardItem.ExposureName, currentUser));

                    if (oldRiskCardItem.EffectWeightId != riskCardItem.EffectWeightId)
                        changeEvent.AddDetail(new ChangeEventDetail("HS_RiskCard_EffectWeight", oldRiskCardItem.EffectWeightName, riskCardItem.EffectWeightName, currentUser));
                }

                SQL += @" END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramRiskCardItemID = new OracleParameter();
                paramRiskCardItemID.ParameterName = "RiskCardItemID";
                paramRiskCardItemID.OracleType = OracleType.Number;

                if (riskCardItem.RiskCardItemId != 0)
                {
                    paramRiskCardItemID.Direction = ParameterDirection.Input;
                    paramRiskCardItemID.Value = riskCardItem.RiskCardItemId;
                }
                else
                {
                    paramRiskCardItemID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramRiskCardItemID);

                OracleParameter param = new OracleParameter();

                if (riskCardItem.RiskCardItemId == 0)
                {
                    param = new OracleParameter();
                    param.ParameterName = "RiskCardID";
                    param.OracleType = OracleType.Number;
                    param.Direction = ParameterDirection.Input;
                    param.Value = riskCardItem.RiskCardId;
                    cmd.Parameters.Add(param);

                    param = new OracleParameter();
                    param.ParameterName = "RiskFactorTypeID";
                    param.OracleType = OracleType.Number;
                    param.Direction = ParameterDirection.Input;
                    param.Value = riskCardItem.RiskFactorTypeId;
                    cmd.Parameters.Add(param);

                    param = new OracleParameter();
                    param.ParameterName = "RiskFactorID";
                    param.OracleType = OracleType.Number;
                    param.Direction = ParameterDirection.Input;
                    param.Value = riskCardItem.RiskFactorId;
                    cmd.Parameters.Add(param);
                }

                param = new OracleParameter();
                param.ParameterName = "HazardID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (riskCardItem.HazardId.HasValue)
                    param.Value = riskCardItem.HazardId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "OtherHazard";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(riskCardItem.OtherHazard))
                    param.Value = riskCardItem.OtherHazard;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ProbabilityID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = riskCardItem.ProbabilityId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ExposureID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = riskCardItem.ExposureId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "EffectWeightID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = riskCardItem.EffectWeightId;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (riskCardItem.RiskFactorTypeId == 0)
                {
                    riskCardItem.RiskFactorTypeId = DBCommon.GetInt(paramRiskCardItemID.Value);
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

        public static bool DeleteRiskCardItem(int riskCardItemId, User currentUser, Change changeEntry)
        {
            bool result = false;

            RiskCardItem oldRiskCardItem = RiskCardItemUtil.GetRiskCardItem(riskCardItemId, currentUser);

            ChangeEvent changeEvent = new ChangeEvent("HS_RiskCard_DeleteRiskCardItem", "", null, null, currentUser);

            if (!String.IsNullOrEmpty(oldRiskCardItem.HazardName))
            {
                changeEvent.AddDetail(new ChangeEventDetail("HS_RiskCard_OtherHazard", oldRiskCardItem.HazardName, "", currentUser));                
            }

            changeEvent.AddDetail(new ChangeEventDetail("HS_RiskCard_Probability", oldRiskCardItem.ProbabilityName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_RiskCard_Exposure", oldRiskCardItem.ExposureName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_RiskCard_EffectWeight", oldRiskCardItem.EffectWeightName, "", currentUser));
            
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = "DELETE FROM PMIS_HS.RiskCardItems WHERE RiskCardItemID = :RiskCardItemID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RiskCardItemID", OracleType.Number).Value = riskCardItemId;

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

        public static int CalculateHazardValue(int probabilityId, int exposureId, int effectWeightId, User currentUser)
        {
            int hazardValue = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" 
                                DECLARE ProbabilityF PMIS_HS.Probabilities.ProbabilityFactor%TYPE;
                                ExposureF PMIS_HS.Exposure.ExposureFactor%TYPE;
                                EffectWeightF PMIS_HS.EffectWeight.EffectWeightFactor%TYPE;

                                BEGIN
                                  SELECT ProbabilityFactor INTO ProbabilityF FROM PMIS_HS.Probabilities WHERE ProbabilityID = :ProbabilityID;
                                  SELECT ExposureFactor INTO ExposureF FROM PMIS_HS.Exposure WHERE ExposureID = :ExposureID;
                                  SELECT EffectWeightFactor INTO EffectWeightF FROM PMIS_HS.EffectWeight WHERE EffectWeightID = :EffectWeightID;
                                  
                                  SELECT ProbabilityF*ExposureF*EffectWeightF INTO :HazardFactor FROM dual;
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramHazardFactor = new OracleParameter();
                paramHazardFactor.ParameterName = "HazardFactor";
                paramHazardFactor.OracleType = OracleType.Number;
                paramHazardFactor.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(paramHazardFactor);

                cmd.Parameters.Add("ProbabilityID", OracleType.Number).Value = probabilityId;
                cmd.Parameters.Add("ExposureID", OracleType.Number).Value = exposureId;
                cmd.Parameters.Add("EffectWeightID", OracleType.Number).Value = effectWeightId;

                //Execute command and get calculated hazard value
                cmd.ExecuteNonQuery();
                hazardValue = (int)Math.Round(DBCommon.GetDecimal(paramHazardFactor.Value), MidpointRounding.AwayFromZero);
            }
            finally
            {
                conn.Close();
            }

            return hazardValue;
        }
    }

}