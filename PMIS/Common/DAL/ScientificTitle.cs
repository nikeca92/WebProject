using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using PMIS.Common;

namespace PMIS.Common
{
    public class ScientificTitle : IDropDownItem
    {
        public string ScientificTitleKey { get; set; }
        public string ScientificTitleName { get; set; }

        //IDropDownItem Members
        public string Text()
        {
            return ScientificTitleName;
        }
        public string Value()
        {
            return ScientificTitleKey;
        }
    }

    public class ScientificTitleUtil
    {
        public static ScientificTitle GetScientificTitle(string ScientificTitleKey, User currentUser)
        {
            ScientificTitle ScientificTitle = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" SELECT NZV_KOD as ScientificTitleKey, NZV_IME as ScientificTitleName 
                                FROM VS_OWNER.KLV_NZV
                                WHERE NZV_KOD = :ScientificTitleKey";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ScientificTitleKey", OracleType.VarChar).Value = ScientificTitleKey;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    ScientificTitle = new ScientificTitle();
                    ScientificTitle.ScientificTitleKey = dr["ScientificTitleKey"].ToString();
                    ScientificTitle.ScientificTitleName = dr["ScientificTitleName"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return ScientificTitle;
        }

        public static List<ScientificTitle> GetAllScientificTitles(User currentUser)
        {
            List<ScientificTitle> listScientificTitles = new List<ScientificTitle>();
            ScientificTitle ScientificTitle;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @" SELECT NZV_KOD as ScientificTitleKey, NZV_IME as ScientificTitleName 
                                FROM VS_OWNER.KLV_NZV
                               ORDER BY  NZV_IME";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ScientificTitle = new ScientificTitle();
                    ScientificTitle.ScientificTitleKey = dr["ScientificTitleKey"].ToString();
                    ScientificTitle.ScientificTitleName = dr["ScientificTitleName"].ToString();

                    listScientificTitles.Add(ScientificTitle);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }
            return listScientificTitles;
        }
    }
}


