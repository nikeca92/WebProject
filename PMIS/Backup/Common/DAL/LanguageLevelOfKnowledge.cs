using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using PMIS.Common;

namespace PMIS.Common
{
   public class LanguageLevelOfKnowledge : IDropDownItem
    {
        public string LanguageLevelOfKnowledgeKey { get; set; }
        public string LanguageLevelOfKnowledgeName { get; set; }

        //IDropDownItem Members
        public string Text()
        {
            return LanguageLevelOfKnowledgeName;
        }
        public string Value()
        {
            return LanguageLevelOfKnowledgeKey;
        }
    }

    public class LanguageLevelOfKnowledgeUtil
    {
        public static LanguageLevelOfKnowledge GetLanguageLevelOfKnowledge(string languageLevelOfKnowledgeKey, User currentUser)
        {
            LanguageLevelOfKnowledge languageLevelOfKnowledge = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.LanguageLevelOfKnowledgeKey , a.LanguageLevelOfKnowledgeName 
                               FROM PMIS_ADM.LanguageLevelOfKnowledge a
                               WHERE a.LanguageLevelOfKnowledgeKey = :LanguageLevelOfKnowledgeKey";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("LanguageLevelOfKnowledgeKey", OracleType.VarChar).Value = languageLevelOfKnowledgeKey;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    languageLevelOfKnowledge = new LanguageLevelOfKnowledge();
                    languageLevelOfKnowledge.LanguageLevelOfKnowledgeKey = dr["LanguageLevelOfKnowledgeKey"].ToString();
                    languageLevelOfKnowledge.LanguageLevelOfKnowledgeName = dr["LanguageLevelOfKnowledgeName"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return languageLevelOfKnowledge;
        }

        public static List<LanguageLevelOfKnowledge> GetAllLanguageLevelOfKnowledges(User currentUser)
        {
            List<LanguageLevelOfKnowledge> listLanguageLevelOfKnowledges = new List<LanguageLevelOfKnowledge>();
            LanguageLevelOfKnowledge languageLevelOfKnowledge;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.LanguageLevelOfKnowledgeKey , a.LanguageLevelOfKnowledgeName 
                               FROM PMIS_ADM.LanguageLevelOfKnowledge a
                               ORDER BY a.LanguageLevelOfKnowledgeName";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    languageLevelOfKnowledge = new LanguageLevelOfKnowledge();
                    languageLevelOfKnowledge.LanguageLevelOfKnowledgeKey = dr["LanguageLevelOfKnowledgeKey"].ToString();
                    languageLevelOfKnowledge.LanguageLevelOfKnowledgeName = dr["LanguageLevelOfKnowledgeName"].ToString();

                    listLanguageLevelOfKnowledges.Add(languageLevelOfKnowledge);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }
            return listLanguageLevelOfKnowledges;
        }
    }
}
