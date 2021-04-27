using System.Collections.Generic;
using System.Data.OracleClient;

namespace PMIS.Common
{
    public class MaritalStatus : IDropDownItem
    {
        private string maritalStatusKey;
        private string maritalStatusName;

        public string MaritalStatusKey
        {
            get
            {
                return maritalStatusKey;
            }
            set
            {
                maritalStatusKey = value;
            }
        }

        public string MaritalStatusName
        {
            get
            {
                return maritalStatusName;
            }
            set
            {
                maritalStatusName = value;
            }
        }


        //IDropDownItem Members
        public string Text()
        {
            return maritalStatusName;
        }

        public string Value()
        {
            return maritalStatusKey;
        }
    }

    public static class MaritalStatusUtil
    {
        //Get a single MaritalStatus object by its key
        public static MaritalStatus GetMaritalStatus(User currentUser, string maritalStatusKey)
        {
            MaritalStatus maritalStatus = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.MaritalStatusKey, a.MaritalStatusName
                               FROM PMIS_ADM.MaritalStatuses a
                               WHERE a.MaritalStatusKey = :MaritalStatusKey";

                OracleCommand cmd = new OracleCommand(SQL, conn);
                cmd.Parameters.Add("MaritalStatusKey", OracleType.VarChar).Value = maritalStatusKey;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    maritalStatus = new MaritalStatus();
                    maritalStatus.MaritalStatusKey = dr["MaritalStatusKey"].ToString();
                    maritalStatus.MaritalStatusName = dr["MaritalStatusName"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return maritalStatus;
        }

        //Get a list of all Genders
        public static List<MaritalStatus> GetMaritalStatuses(User currentUser)
        {
            List<MaritalStatus> listMaritalStatuses = new List<MaritalStatus>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.MaritalStatusKey, a.MaritalStatusName
                               FROM PMIS_ADM.MaritalStatuses a
                               ORDER BY a.MaritalStatusName ASC";

                OracleCommand cmd = new OracleCommand(SQL, conn);
                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    MaritalStatus maritalStatus = new MaritalStatus();
                    maritalStatus.MaritalStatusKey = dr["MaritalStatusKey"].ToString();
                    maritalStatus.MaritalStatusName = dr["MaritalStatusName"].ToString();
                    listMaritalStatuses.Add(maritalStatus);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listMaritalStatuses;
        }
    }
}
