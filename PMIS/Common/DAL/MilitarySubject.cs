using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using PMIS.Common;

namespace PMIS.Common
{
    //Represents single person education from VS_OWNER.KLV_SVA table
  
    public class MilitarySubject : IDropDownItem
    {
        private string militarySubjectCode;
                private string militarySubjectName;
                private bool isActive;


        public string MilitarySubjectCode
        {
            get { return militarySubjectCode; }
            set { militarySubjectCode = value; }
        }

        public string MilitarySubjectName
        {
            get { return militarySubjectName; }
            set { militarySubjectName = value; }
        }


        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        //IDropDownItem Members
        public string Text()
        {
            return MilitarySubjectName;
        }

        public string Value()
        {
            return MilitarySubjectCode;
        }

    }

    public static class MilitarySubjectUtil
    {
        public static MilitarySubject GetMilitarySubject(string militarySubjectCode, User currentUser)
        {
            MilitarySubject militarySubject = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.SVA_KOD as MilitarySubjectCode, a.SVA_IME as MilitarySubjectName
                               FROM VS_OWNER.KLV_SVA a
                               WHERE a.SVA_KOD = :MilitarySubjectCode";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitarySubjectCode", OracleType.VarChar).Value = militarySubjectCode;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    militarySubject = new MilitarySubject();
                    militarySubject.MilitarySubjectCode = dr["MilitarySubjectCode"].ToString();
                    militarySubject.MilitarySubjectName = dr["MilitarySubjectName"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militarySubject;
        }

        public static List<MilitarySubject> GetAllMilitarySubjects(User currentUser)
        {
            List<MilitarySubject> listMilitarySubject = new List<MilitarySubject>();
            MilitarySubject militarySubject;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.SVA_KOD as MilitarySubjectCode, a.SVA_IME as MilitarySubjectName
                               FROM VS_OWNER.KLV_SVA a
                               ORDER BY a.SVA_IME";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    militarySubject = new MilitarySubject();
                    militarySubject.MilitarySubjectCode = dr["MilitarySubjectCode"].ToString();
                    militarySubject.MilitarySubjectName = dr["MilitarySubjectName"].ToString();

                    listMilitarySubject.Add(militarySubject);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listMilitarySubject;
        }
    }
}
