using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.HealthSafety.Common
{
    public class EffectWeight : BaseDbObject, IDropDownItem
    {
        private int effectWeightId;
        private string effectWeightName;
        private string effectWeightDesc;
        private decimal effectWeightFactor;

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

        public string EffectWeightDesc
        {
            get { return effectWeightDesc; }
            set { effectWeightDesc = value; }
        }

        public decimal EffectWeightFactor
        {
            get { return effectWeightFactor; }
            set { effectWeightFactor = value; }
        }

        //IDropDownItem Members
        public string Text()
        {
            return "(" + effectWeightFactor + ") " + effectWeightName;
        }

        public string Value()
        {
            return effectWeightId.ToString();
        }

        public EffectWeight(User user)
            : base(user)
        {
        }
    }

    public static class EffectWeightUtil
    {
        private static EffectWeight ExtractEffectWeightFromDR(OracleDataReader dr, User currentUser)
        {
            EffectWeight effectWeight = new EffectWeight(currentUser);

            effectWeight.EffectWeightId = DBCommon.GetInt(dr["EffectWeightID"]);
            effectWeight.EffectWeightName = dr["EffectWeightName"].ToString();
            effectWeight.EffectWeightDesc = dr["EffectWeightDesc"].ToString();
            effectWeight.EffectWeightFactor = DBCommon.GetDecimal(dr["EffectWeightFactor"]);

            return effectWeight;
        }

        public static EffectWeight GetEffectWeight(int effectWeightId, User currentUser)
        {
            EffectWeight effectWeight = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.EffectWeightID, a.EffectWeightName, a.EffectWeightDesc, a.EffectWeightFactor
                               FROM PMIS_HS.EffectWeight a                       
                               WHERE a.EffectWeightID = :EffectWeightID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("EffectWeightID", OracleType.Number).Value = effectWeightId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    effectWeight = ExtractEffectWeightFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return effectWeight;
        }

        public static List<EffectWeight> GetAllEffectWeights(User currentUser)
        {
            List<EffectWeight> effectWeights = new List<EffectWeight>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.EffectWeightID, a.EffectWeightName, a.EffectWeightDesc, a.EffectWeightFactor
                               FROM PMIS_HS.EffectWeight a";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    effectWeights.Add(ExtractEffectWeightFromDR(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return effectWeights;
        }
    }

}