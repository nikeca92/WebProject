using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;

namespace PMIS.Common
{
    //Represents single person education from VS_OWNER.KLV_OBR table
    public class PersonEducation : IDropDownItem
    {
        private string personEducationCode;
        private string personEducationName;

        public string PersonEducationCode
        {
            get { return personEducationCode; }
            set { personEducationCode = value; }
        }

        public string PersonEducationName
        {
            get { return personEducationName; }
            set { personEducationName = value; }
        }

        //IDropDownItem Members
        public string Text()
        {
            return personEducationName;
        }

        public string Value()
        {
            return personEducationCode;
        }
    }

    public static class PersonEducationUtil
    {
        public static PersonEducation GetPersonEducation(string personEducationCode, User currentUser)
        {
            PersonEducation personEducation = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.OBR_KOD as EducationCode, a.OBR_IME as EducationName
                               FROM VS_OWNER.KLV_OBR a
                               WHERE a.OBR_KOD = :EducationCode";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("EducationCode", OracleType.VarChar).Value = personEducationCode;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    personEducation = new PersonEducation();
                    personEducation.PersonEducationCode = dr["EducationCode"].ToString();
                    personEducation.PersonEducationName = dr["EducationName"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personEducation;
        }

        public static List<PersonEducation> GetAllPersonEducations(User currentUser)
        {
            List<PersonEducation> personEducations = new List<PersonEducation>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.OBR_KOD as EducationCode, a.OBR_IME as EducationName
                               FROM VS_OWNER.KLV_OBR a
                               ORDER BY a.OBR_KOD";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    PersonEducation personEducation = new PersonEducation();
                    personEducation.PersonEducationCode = dr["EducationCode"].ToString();
                    personEducation.PersonEducationName = dr["EducationName"].ToString();
                    personEducations.Add(personEducation);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personEducations;
        }

        //основно
        public static string OsnovnoDBKey = "B";

        //завършен X клас
        public static string Klas10DBKey = "A";
    }
}
