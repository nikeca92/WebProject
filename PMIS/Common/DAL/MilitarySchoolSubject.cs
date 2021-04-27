using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using PMIS.Common;

namespace PMIS.Common
{
    //Represents single person education from VS_OWNER.KLV_VSP table
    public class MilitarySchoolSubject : IDropDownItem
    {
        private int militarySchoolSubjectId;
        private string militarySchoolSubjectCode;
        private string militarySchoolSubjectName;
        private bool isActive;

        public int MilitarySchoolSubjectId
        {
            get { return militarySchoolSubjectId; }
            set { militarySchoolSubjectId = value; }
        }

        public string MilitarySchoolSubjectCode
        {
            get { return militarySchoolSubjectCode; }
            set { militarySchoolSubjectCode = value; }
        }

        public string MilitarySchoolSubjectName
        {
            get { return militarySchoolSubjectName; }
            set { militarySchoolSubjectName = value; }
        }

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        //IDropDownItem Members
        public string Text()
        {
            return militarySchoolSubjectName;
        }

        public string Value()
        {
            return militarySchoolSubjectCode;
        }

    }

    public static class MilitarySchoolSubjectUtil
    {
        public static MilitarySchoolSubject GetMilitarySchoolSubject(int militarySchoolSubjectId, User currentUser)
        {
            MilitarySchoolSubject militarySchoolSubject = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.VSP_KOD as MilitarySchoolSubjectCode, a.VSP_IME as MilitarySchoolSubjectName, a.IsActive
                               FROM VS_OWNER.KLV_VSP a
                               WHERE a.VSPID = :MilitarySchoolSubjectID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitarySchoolSubjectID", OracleType.Number).Value = militarySchoolSubjectId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    militarySchoolSubject = new MilitarySchoolSubject();
                    militarySchoolSubject.MilitarySchoolSubjectId = militarySchoolSubjectId;
                    militarySchoolSubject.MilitarySchoolSubjectCode = dr["MilitarySchoolSubjectCode"].ToString();
                    militarySchoolSubject.MilitarySchoolSubjectName = dr["MilitarySchoolSubjectName"].ToString();
                    militarySchoolSubject.IsActive = (dr["IsActive"].ToString() == "1" ? true : false);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militarySchoolSubject;
        }

        public static List<MilitarySchoolSubject> GetAllMilitarySchoolSubjects(User currentUser)
        {
            List<MilitarySchoolSubject> militarySchoolSubjects = new List<MilitarySchoolSubject>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.VSP_KOD as MilitarySchoolSubjectCode, a.VSP_IME as MilitarySchoolSubjectName, 
                                a.VSPID as MilitarySchoolSubjectID, a.IsActive
                               FROM VS_OWNER.KLV_VSP a
                               ORDER BY a.VSP_IME";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    MilitarySchoolSubject militarySchoolSubject = new MilitarySchoolSubject();
                    militarySchoolSubject.MilitarySchoolSubjectId = DBCommon.GetInt(dr["MilitarySchoolSubjectID"]);
                    militarySchoolSubject.MilitarySchoolSubjectCode = dr["MilitarySchoolSubjectCode"].ToString();
                    militarySchoolSubject.MilitarySchoolSubjectName = dr["MilitarySchoolSubjectName"].ToString();
                    militarySchoolSubject.IsActive = (dr["IsActive"].ToString() == "1" ? true : false);

                    militarySchoolSubjects.Add(militarySchoolSubject);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militarySchoolSubjects;
        }

        public static List<MilitarySchoolSubject> GetAllMilitarySchoolSubjectsByMilitarySchoolID(int militarySchoolId, int year, User currentUser)
        {
            List<MilitarySchoolSubject> militarySchoolSubjects = new List<MilitarySchoolSubject>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT DISTINCT(a.VSP_KOD), a.VSP_KOD as MilitarySchoolSubjectCode, a.VSP_IME as MilitarySchoolSubjectName, 
                              a.VSPID as MilitarySchoolSubjectID, a.IsActive
                             FROM VS_OWNER.KLV_VSP a
                             INNER JOIN PMIS_APPL.Specializations b ON a.VSPID = b.MilitarySchoolSubjectID
                             INNER JOIN PMIS_APPL.MilitarySchoolSpecializations c ON b.SpecializationID = c.SpecializationID
                             WHERE c.MilitarySchoolID = :MilitarySchoolID AND c.Year = :Year
                             ORDER BY a.VSP_IME";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitarySchoolID", OracleType.Number).Value = militarySchoolId;
                cmd.Parameters.Add("Year", OracleType.Number).Value = year;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    MilitarySchoolSubject militarySchoolSubject = new MilitarySchoolSubject();
                    militarySchoolSubject.MilitarySchoolSubjectId = DBCommon.GetInt(dr["MilitarySchoolSubjectID"]);
                    militarySchoolSubject.MilitarySchoolSubjectCode = dr["MilitarySchoolSubjectCode"].ToString();
                    militarySchoolSubject.MilitarySchoolSubjectName = dr["MilitarySchoolSubjectName"].ToString();
                    militarySchoolSubject.IsActive = (dr["IsActive"].ToString() == "1" ? true : false);

                    militarySchoolSubjects.Add(militarySchoolSubject);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militarySchoolSubjects;
        }
    }
}
