using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using PMIS.Common;

namespace PMIS.Common
{
    public class LanguageFormOfKnowledge : IDropDownItem
    {
        public string LanguageFormOfKnowledgeKey { get; set; }
        public string LanguageFormOfKnowledgeName { get; set; }

        //IDropDownItem Members
        public string Text()
        {
            return LanguageFormOfKnowledgeName;
        }
        public string Value()
        {
            return LanguageFormOfKnowledgeKey;
        }
    }

    public class LanguageFormOfKnowledgeUtil
    {
        public static LanguageFormOfKnowledge GetLanguageFormOfKnowledge(string LanguageFormOfKnowledgeKey, User currentUser)
        {
            LanguageFormOfKnowledge LanguageFormOfKnowledge = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.LanguageFormOfKnowledgeKey , a.LanguageFormOfKnowledgeName 
                               FROM PMIS_ADM.LanguageFormOfKnowledge a
                               WHERE a.LanguageFormOfKnowledgeKey = :LanguageFormOfKnowledgeKey";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("LanguageFormOfKnowledgeKey", OracleType.VarChar).Value = LanguageFormOfKnowledgeKey;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    LanguageFormOfKnowledge = new LanguageFormOfKnowledge();
                    LanguageFormOfKnowledge.LanguageFormOfKnowledgeKey = dr["LanguageFormOfKnowledgeKey"].ToString();
                    LanguageFormOfKnowledge.LanguageFormOfKnowledgeName = dr["LanguageFormOfKnowledgeName"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return LanguageFormOfKnowledge;
        }

        public static List<LanguageFormOfKnowledge> GetAllLanguageFormOfKnowledges(User currentUser)
        {
            List<LanguageFormOfKnowledge> listLanguageFormOfKnowledges = new List<LanguageFormOfKnowledge>();
            LanguageFormOfKnowledge LanguageFormOfKnowledge;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.LanguageFormOfKnowledgeKey , a.LanguageFormOfKnowledgeName 
                               FROM PMIS_ADM.LanguageFormOfKnowledge a
                               ORDER BY a.LanguageFormOfKnowledgeName";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    LanguageFormOfKnowledge = new LanguageFormOfKnowledge();
                    LanguageFormOfKnowledge.LanguageFormOfKnowledgeKey = dr["LanguageFormOfKnowledgeKey"].ToString();
                    LanguageFormOfKnowledge.LanguageFormOfKnowledgeName = dr["LanguageFormOfKnowledgeName"].ToString();

                    listLanguageFormOfKnowledges.Add(LanguageFormOfKnowledge);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }
            return listLanguageFormOfKnowledges;
        }
    }
}

