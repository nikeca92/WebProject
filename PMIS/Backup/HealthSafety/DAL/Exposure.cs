using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.HealthSafety.Common
{
    public class Exposure : BaseDbObject, IDropDownItem
    {
        private int exposureId;
        private string exposureName;
        private decimal exposureFactor;

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

        //IDropDownItem Members
        public string Text()
        {
            return "(" + exposureFactor + ") " + exposureName;
        }

        public string Value()
        {
            return exposureId.ToString();
        }

        public Exposure(User user)
            : base(user)
        {
        }
    }

    public static class ExposureUtil
    {
        private static Exposure ExtractExposureFromDR(OracleDataReader dr, User currentUser)
        {
            Exposure exposure = new Exposure(currentUser);

            exposure.ExposureId = DBCommon.GetInt(dr["ExposureID"]);
            exposure.ExposureName = dr["ExposureName"].ToString();
            exposure.ExposureFactor = DBCommon.GetDecimal(dr["ExposureFactor"]);

            return exposure;
        }

        public static Exposure GetExposure(int exposureId, User currentUser)
        {
            Exposure exposure = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.ExposureID, a.ExposureName, a.ExposureFactor
                               FROM PMIS_HS.Exposure a                       
                               WHERE a.ExposureID = :ExposureID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ExposureID", OracleType.Number).Value = exposureId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    exposure = ExtractExposureFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return exposure;
        }

        public static List<Exposure> GetAllExposures(User currentUser)
        {
            List<Exposure> exposures = new List<Exposure>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.ExposureID, a.ExposureName, a.ExposureFactor
                               FROM PMIS_HS.Exposure a";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    exposures.Add(ExtractExposureFromDR(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return exposures;
        }
    }

}