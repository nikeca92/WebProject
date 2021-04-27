using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Common
{
    public class ClInformation : IDropDownItem
    {
        private string clInfoKey;        
        private string clInfoName;

        public string ClInfoKey
        {
            get { return clInfoKey; }
            set { clInfoKey = value; }
        }

        public string ClInfoName
        {
            get { return clInfoName; }
            set { clInfoName = value; }
        }      

        public string Text()
        {
            return ClInfoName;
        }

        public string Value()
        {
            return ClInfoKey;
        }
       
    }

    public class ClInformationUtil
    {
        private static string RV_DOMAIN_NATO = "NIVO_KL_INF_NATO";
        private static string RV_DOMAIN_BG = "NIVO_KL_INF";
        private static string RV_DOMAIN_EU = "NIVO_KL_INF_EU";

        public static ClInformation GetClInformation(string key, string domain, User currentUser)
        {
            ClInformation clInformation = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" SELECT a.RV_LOW_VALUE as ClInfoKey, a.RV_MEANING as ClInfoName
                                FROM VS_OWNER.CG_REF_CODES a
                                WHERE a.RV_LOW_VALUE = :Key AND a.RV_DOMAIN = :Domain";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("Key", OracleType.VarChar).Value = key;
                cmd.Parameters.Add("Domain", OracleType.VarChar).Value = domain;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    clInformation = new ClInformation();
                    clInformation.ClInfoKey = dr["ClInfoKey"].ToString();
                    clInformation.ClInfoName = dr["ClInfoName"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return clInformation;
        }

        public static ClInformation GetClInformationNATO(string key, User currentUser)
        {
            return GetClInformation(key, RV_DOMAIN_NATO, currentUser);
        }

        public static ClInformation GetClInformationBG(string key, User currentUser)
        {
            return GetClInformation(key, RV_DOMAIN_BG, currentUser);
        }

        public static ClInformation GetClInformationEU(string key, User currentUser)
        {
            return GetClInformation(key, RV_DOMAIN_EU, currentUser);
        }

        public static List<ClInformation> GetAllClInformation(string domain, User currentUser)
        {
            List<ClInformation> clInformationList = new List<ClInformation>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" SELECT a.RV_LOW_VALUE as ClInfoKey, a.RV_MEANING as ClInfoName
                                FROM VS_OWNER.CG_REF_CODES a
                                WHERE a.RV_DOMAIN = :Domain";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("Domain", OracleType.VarChar).Value = domain;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ClInformation clInformation = new ClInformation();
                    clInformation.ClInfoKey = dr["ClInfoKey"].ToString();
                    clInformation.ClInfoName = dr["ClInfoName"].ToString();

                    clInformationList.Add(clInformation);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }
            return clInformationList;
        }

        public static List<ClInformation> GetAllClInformationNATO(User currentUser)
        {
            return GetAllClInformation(RV_DOMAIN_NATO, currentUser);
        }

        public static List<ClInformation> GetAllClInformationBG(User currentUser)
        {
            return GetAllClInformation(RV_DOMAIN_BG, currentUser);
        }

        public static List<ClInformation> GetAllClInformationEU(User currentUser)
        {
            return GetAllClInformation(RV_DOMAIN_EU, currentUser);
        }
    }
}
