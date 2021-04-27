using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using PMIS.Common;

namespace PMIS.Common
{
    public class LanguageDiploma : IDropDownItem
    {
        public string LanguageDiplomaKey { get; set; }
        public string LanguageDiplomaName { get; set; }

        //IDropDownItem Members
        public string Text()
        {
            return LanguageDiplomaName;
        }
        public string Value()
        {
            return LanguageDiplomaKey;
        }
    }

    public class LanguageDiplomaUtil
    {
        public static LanguageDiploma GetLanguageDiploma(string LanguageDiplomaKey, User currentUser)
        {
            LanguageDiploma LanguageDiploma = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.LanguageDiplomaKey , a.LanguageDiplomaName 
                               FROM PMIS_ADM.LanguageDiploma a
                               WHERE a.LanguageDiplomaKey = :LanguageDiplomaKey";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("LanguageDiplomaKey", OracleType.VarChar).Value = LanguageDiplomaKey;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    LanguageDiploma = new LanguageDiploma();
                    LanguageDiploma.LanguageDiplomaKey = dr["LanguageDiplomaKey"].ToString();
                    LanguageDiploma.LanguageDiplomaName = dr["LanguageDiplomaName"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return LanguageDiploma;
        }

        public static List<LanguageDiploma> GetAllLanguageDiplomas(User currentUser)
        {
            List<LanguageDiploma> listLanguageDiplomas = new List<LanguageDiploma>();
            LanguageDiploma LanguageDiploma;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.LanguageDiplomaKey , a.LanguageDiplomaName 
                               FROM PMIS_ADM.LanguageDiploma a
                               ORDER BY a.LanguageDiplomaName";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    LanguageDiploma = new LanguageDiploma();
                    LanguageDiploma.LanguageDiplomaKey = dr["LanguageDiplomaKey"].ToString();
                    LanguageDiploma.LanguageDiplomaName = dr["LanguageDiplomaName"].ToString();

                    listLanguageDiplomas.Add(LanguageDiploma);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }
            return listLanguageDiplomas;
        }
    }
}

