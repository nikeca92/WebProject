using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;

namespace PMIS.Common
{
    //Represents single person language from VS_OWNER.KLV_EZK
    public class PersonLanguage : IDropDownItem
    {
        private string personLanguageCode;
        private string personLanguageName;

        public string PersonLanguageCode
        {
            get { return personLanguageCode; }
            set { personLanguageCode = value; }
        }

        public string PersonLanguageName
        {
            get { return personLanguageName; }
            set { personLanguageName = value; }
        }

        //IDropDownItem Members
        public string Text()
        {
            return personLanguageName;
        }

        public string Value()
        {
            return personLanguageCode;
        }
    }

    public static class PersonLanguageUtil
    {
        public static PersonLanguage GetPersonLanguage(string personLanguageCode, User currentUser)
        {
            PersonLanguage personLanguage = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.EZK_KOD as LanguageCode, a.EZK_IME as LanguageName
                               FROM VS_OWNER.KLV_EZK a
                               WHERE a.EZK_KOD = :LanguageCode";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("LanguageCode", OracleType.VarChar).Value = personLanguageCode;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    personLanguage = new PersonLanguage();
                    personLanguage.PersonLanguageCode = dr["LanguageCode"].ToString();
                    personLanguage.PersonLanguageName = dr["LanguageName"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personLanguage;
        }

        public static List<PersonLanguage> GetAllPersonLanguages(User currentUser)
        {
            List<PersonLanguage> personLanguages = new List<PersonLanguage>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.EZK_KOD as LanguageCode, a.EZK_IME as LanguageName
                               FROM VS_OWNER.KLV_EZK a
                               ORDER BY a.EZK_IME";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    PersonLanguage personLanguage = new PersonLanguage();
                    personLanguage.PersonLanguageCode = dr["LanguageCode"].ToString();
                    personLanguage.PersonLanguageName = dr["LanguageName"].ToString();
                    personLanguages.Add(personLanguage);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personLanguages;
        }
    }
}
