using System.Collections.Generic;
using System.Data.OracleClient;

namespace PMIS.Common
{
    //This class represents the Gender (i.e. Male or Female)
    public class Gender : BaseDbObject, IDropDownItem
    {
        private int genderId;
        private string genderName;

        public int GenderId
        {
            get
            {
                return genderId;
            }
            set
            {
                genderId = value;
            }
        }

        public string GenderName
        {
            get
            {
                return genderName;
            }
            set
            {
                genderName = value;
            }
        }

        public Gender(User currentUser) : base(currentUser)
        {
        }

        //IDropDownItem Members
        public string Text()
        {
            return GenderName;
        }

        public string Value()
        {
            return GenderId.ToString();
        }
    }

    public static class GenderUtil
    {
        //Exstract a particular Gender object from a data reader. This method should be reused when pulling records from the DB
        public static Gender ExtractGenderFromDR(User currentUser, OracleDataReader dr)
        {
            Gender gender = new Gender(currentUser);

            gender.GenderId = (DBCommon.IsInt(dr["GenderID"]) ? DBCommon.GetInt(dr["GenderID"]) : 0);
            gender.GenderName = dr["GenderName"].ToString();

            return gender;
        }

        //Get a single Gender object by its ID
        public static Gender GetGender(User currentUser, int genderId)
        {
            Gender gender = null;
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.GenderID, a.GenderName
                               FROM PMIS_ADM.Gender a
                               WHERE a.GenderID = :GenderID";

                OracleCommand cmd = new OracleCommand(SQL, conn);
                cmd.Parameters.Add("GenderID", OracleType.Number).Value = genderId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    gender = ExtractGenderFromDR(currentUser, dr);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return gender;
        }

        //Get a list of all Genders
        public static List<Gender> GetGenders(User currentUser)
        {
            List<Gender> listGenders = new List<Gender>();
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.GenderID, a.GenderName
                               FROM PMIS_ADM.Gender a
                               ORDER BY a.GenderID ASC";

                OracleCommand cmd = new OracleCommand(SQL, conn);
                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Gender gender = ExtractGenderFromDR(currentUser, dr);
                    listGenders.Add(gender);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listGenders;
        }
    }
}