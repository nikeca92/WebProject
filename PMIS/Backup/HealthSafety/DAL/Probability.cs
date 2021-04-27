using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.HealthSafety.Common
{
    public class Probability : BaseDbObject, IDropDownItem
    {
        private int probabilityId;
        private string probabilityName;
        private decimal probabilityFactor;

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

        //IDropDownItem Members
        public string Text()
        {
            return "(" + probabilityFactor + ") " + probabilityName;
        }

        public string Value()
        {
            return probabilityId.ToString();
        }

        public Probability(User user)
            :base(user)
        {
        }  
    }

    public static class ProbabilityUtil
    {
        private static Probability ExtractProbabilityFromDR(OracleDataReader dr, User currentUser)
        {
            Probability probability = new Probability(currentUser);

            probability.ProbabilityId = DBCommon.GetInt(dr["ProbabilityID"]);
            probability.ProbabilityName = dr["ProbabilityName"].ToString();
            probability.ProbabilityFactor = DBCommon.GetDecimal(dr["ProbabilityFactor"]);

            return probability;
        }

        public static Probability GetProbability(int probabilityId, User currentUser)
        {
            Probability probability = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.ProbabilityID, a.ProbabilityName, a.ProbabilityFactor
                               FROM PMIS_HS.Probabilities a                       
                               WHERE a.ProbabilityID = :ProbabilityID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ProbabilityID", OracleType.Number).Value = probabilityId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    probability = ExtractProbabilityFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return probability;
        }

        public static List<Probability> GetAllProbabilities(User currentUser)
        {
            List<Probability> probabilities = new List<Probability>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.ProbabilityID, a.ProbabilityName, a.ProbabilityFactor
                               FROM PMIS_HS.Probabilities a";

                OracleCommand cmd = new OracleCommand(SQL, conn);                

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    probabilities.Add(ExtractProbabilityFromDR(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return probabilities;
        }
    }

}