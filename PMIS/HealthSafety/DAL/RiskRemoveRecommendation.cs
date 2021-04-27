using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.HealthSafety.Common
{
    public class RiskRemoveRecommendation : BaseDbObject
    {
        private int riskRemoveRecommendationId;
        private string recommendations;
        private DateTime dueDate;
        private DateTime? executionDate;

        public int RiskRemoveRecommendationId
        {
            get { return riskRemoveRecommendationId; }
            set { riskRemoveRecommendationId = value; }
        }

        public string Recommendations
        {
            get { return recommendations; }
            set { recommendations = value; }
        }

        public DateTime DueDate
        {
            get { return dueDate; }
            set { dueDate = value; }
        }

        public DateTime? ExecutionDate
        {
            get { return executionDate; }
            set { executionDate = value; }
        }

        public RiskRemoveRecommendation(User user)
            :base(user)
        {

        }
    }

    public static class RiskRemoveRecommendationUtil
    {
        public static RiskRemoveRecommendation GetRiskRemoveRecommendation(int riskRemoveRecommendationId, User currentUser)
        {
            RiskRemoveRecommendation riskRemoveRecommendation = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.RiskRemoveRecommendationID, a.Recommendations, a.DueDate, a.ExecutionDate
                               FROM PMIS_HS.RiskRemoveRecommendations a
                               WHERE a.RiskRemoveRecommendationID = :RiskRemoveRecommendationID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RiskRemoveRecommendationID", OracleType.Number).Value = riskRemoveRecommendationId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    riskRemoveRecommendation = new RiskRemoveRecommendation(currentUser);
                    riskRemoveRecommendation.RiskRemoveRecommendationId = riskRemoveRecommendationId;
                    riskRemoveRecommendation.Recommendations = dr["Recommendations"].ToString();

                    if (dr["DueDate"] is DateTime)
                        riskRemoveRecommendation.DueDate = (DateTime)dr["DueDate"];

                    if (dr["ExecutionDate"] is DateTime)
                        riskRemoveRecommendation.ExecutionDate = (DateTime)dr["ExecutionDate"];
                    else
                        riskRemoveRecommendation.ExecutionDate = null;
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return riskRemoveRecommendation;
        }

        public static List<RiskRemoveRecommendation> GetRiskRemoveRecommendationsByRiskAssesment(int riskAssessmentId, User currentUser)
        {
            List<RiskRemoveRecommendation> riskRemoveRecommendations = new List<RiskRemoveRecommendation>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.RiskRemoveRecommendationID, a.Recommendations, a.DueDate, a.ExecutionDate
                               FROM PMIS_HS.RiskRemoveRecommendations a
                               WHERE a.RiskAssessmentID = :RiskAssessmentID
                               ORDER BY a.RiskRemoveRecommendationID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RiskAssessmentID", OracleType.Number).Value = riskAssessmentId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    RiskRemoveRecommendation riskRemoveRecommendation = new RiskRemoveRecommendation(currentUser);

                    riskRemoveRecommendation.RiskRemoveRecommendationId = DBCommon.GetInt(dr["RiskRemoveRecommendationID"]);
                    riskRemoveRecommendation.Recommendations = dr["Recommendations"].ToString();

                    if (dr["DueDate"] is DateTime)
                        riskRemoveRecommendation.DueDate = (DateTime)dr["DueDate"];

                    if (dr["ExecutionDate"] is DateTime)
                        riskRemoveRecommendation.ExecutionDate = (DateTime)dr["ExecutionDate"];
                    else
                        riskRemoveRecommendation.ExecutionDate = null;

                    riskRemoveRecommendations.Add(riskRemoveRecommendation);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return riskRemoveRecommendations;
        }

        public static bool SaveRiskRemoveRecommendation(int riskAssessmentId, RiskRemoveRecommendation riskRemoveRecommendation, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                RiskAssessment riskAssessment = RiskAssessmentUtil.GetRiskAssessment(riskAssessmentId, currentUser);

                string logDescription = "";
                logDescription += "Регистрационен номер: " + riskAssessment.RegNumber;

                SQL = @"BEGIN
                        
                       ";

                if (riskRemoveRecommendation.RiskRemoveRecommendationId == 0)
                {
                    SQL += @"INSERT INTO PMIS_HS.RiskRemoveRecommendations (RiskAssessmentID, Recommendations, DueDate, ExecutionDate)
                            VALUES (:RiskAssessmentID, :Recommendations, :DueDate, :ExecutionDate);

                            SELECT PMIS_HS.RRRecommendations_ID_SEQ.currval INTO :RiskRemoveRecommendationID FROM dual;

                            ";

                    changeEvent = new ChangeEvent("HS_RiskRemoveRecom_AddRiskRemoveRecommendation", logDescription, riskAssessment.MilitaryUnit, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("HS_RiskRemoveRecom_Recommendation", "", String.IsNullOrEmpty(riskRemoveRecommendation.Recommendations) ? "" : riskRemoveRecommendation.Recommendations, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_RiskRemoveRecom_DueDate", "", CommonFunctions.FormatDate(riskRemoveRecommendation.DueDate), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_RiskRemoveRecom_ExecutionDate", "", CommonFunctions.FormatDate(riskRemoveRecommendation.ExecutionDate), currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_HS.RiskRemoveRecommendations SET
                               RiskAssessmentID = :RiskAssessmentID, 
                               Recommendations = :Recommendations, 
                               DueDate = :DueDate, 
                               ExecutionDate = :ExecutionDate
                            WHERE RiskRemoveRecommendationID = :RiskRemoveRecommendationID;                       

                            ";

                    changeEvent = new ChangeEvent("HS_RiskRemoveRecom_EditRiskRemoveRecommendation", logDescription, riskAssessment.MilitaryUnit, null, currentUser);

                    RiskRemoveRecommendation oldRiskRemoveRecommendation = RiskRemoveRecommendationUtil.GetRiskRemoveRecommendation(riskRemoveRecommendation.RiskRemoveRecommendationId, currentUser);

                    if (oldRiskRemoveRecommendation.Recommendations.Trim() != riskRemoveRecommendation.Recommendations.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_RiskRemoveRecom_Recommendation", oldRiskRemoveRecommendation.Recommendations, riskRemoveRecommendation.Recommendations, currentUser));

                    if (!CommonFunctions.IsEqual(oldRiskRemoveRecommendation.DueDate, riskRemoveRecommendation.DueDate))
                        changeEvent.AddDetail(new ChangeEventDetail("HS_RiskRemoveRecom_DueDate", CommonFunctions.FormatDate(oldRiskRemoveRecommendation.DueDate), CommonFunctions.FormatDate(riskRemoveRecommendation.DueDate), currentUser));

                    if (!CommonFunctions.IsEqual(oldRiskRemoveRecommendation.ExecutionDate, riskRemoveRecommendation.ExecutionDate))
                        changeEvent.AddDetail(new ChangeEventDetail("HS_RiskRemoveRecom_ExecutionDate", CommonFunctions.FormatDate(oldRiskRemoveRecommendation.ExecutionDate), CommonFunctions.FormatDate(riskRemoveRecommendation.ExecutionDate), currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramRiskRemoveRecommendationID = new OracleParameter();
                paramRiskRemoveRecommendationID.ParameterName = "RiskRemoveRecommendationID";
                paramRiskRemoveRecommendationID.OracleType = OracleType.Number;

                if (riskRemoveRecommendation.RiskRemoveRecommendationId != 0)
                {
                    paramRiskRemoveRecommendationID.Direction = ParameterDirection.Input;
                    paramRiskRemoveRecommendationID.Value = riskRemoveRecommendation.RiskRemoveRecommendationId;
                }
                else
                {
                    paramRiskRemoveRecommendationID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramRiskRemoveRecommendationID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "RiskAssessmentID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = riskAssessmentId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Recommendations";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(riskRemoveRecommendation.Recommendations))
                    param.Value = riskRemoveRecommendation.Recommendations;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "DueDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                param.Value = riskRemoveRecommendation.DueDate;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ExecutionDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (riskRemoveRecommendation.ExecutionDate.HasValue)
                    param.Value = riskRemoveRecommendation.ExecutionDate;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (riskRemoveRecommendation.RiskRemoveRecommendationId == 0)
                    riskRemoveRecommendation.RiskRemoveRecommendationId = DBCommon.GetInt(paramRiskRemoveRecommendationID.Value);

                result = true;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                if (changeEvent.ChangeEventDetails.Count > 0)
                {
                    changeEntry.AddEvent(changeEvent);
                    RiskAssessmentUtil.SetRiskAssessmentsLastModified(riskAssessmentId, currentUser);
                }
            }

            return result;
        }

        public static bool DeleteRiskRemoveRecommendation(int riskAssessmentId, int riskRemoveRecommendationId, User currentUser, Change changeEntry)
        {
            bool result = false;

            RiskAssessment riskAssessment = RiskAssessmentUtil.GetRiskAssessment(riskAssessmentId, currentUser);

            string logDescription = "";
            logDescription += "Регистрационен номер: " + riskAssessment.RegNumber;

            RiskRemoveRecommendation oldRiskRemoveRecommendation = RiskRemoveRecommendationUtil.GetRiskRemoveRecommendation(riskRemoveRecommendationId, currentUser);

            ChangeEvent changeEvent = new ChangeEvent("HS_RiskRemoveRecom_DeleteRiskRemoveRecommendation", logDescription, riskAssessment.MilitaryUnit, null, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("HS_RiskRemoveRecom_Recommendation", String.IsNullOrEmpty(oldRiskRemoveRecommendation.Recommendations) ? "" : oldRiskRemoveRecommendation.Recommendations, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_RiskRemoveRecom_DueDate", CommonFunctions.FormatDate(oldRiskRemoveRecommendation.DueDate), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_RiskRemoveRecom_ExecutionDate", CommonFunctions.FormatDate(oldRiskRemoveRecommendation.ExecutionDate), "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = "DELETE FROM PMIS_HS.RiskRemoveRecommendations WHERE RiskRemoveRecommendationID = :RiskRemoveRecommendationID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RiskRemoveRecommendationID", OracleType.Number).Value = riskRemoveRecommendationId;

                result = cmd.ExecuteNonQuery() == 1;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                changeEntry.AddEvent(changeEvent);

                RiskAssessmentUtil.SetRiskAssessmentsLastModified(riskAssessmentId, currentUser);
            }

            return result;
        }
    }
}
