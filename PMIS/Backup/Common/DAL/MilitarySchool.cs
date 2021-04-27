using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using PMIS.Common;

namespace PMIS.Common
{
    //Represents single person education from VS_OWNER.KLV_VVU table
    public class MilitarySchool : IDropDownItem
    {
        private int militarySchoolId;
        private string militarySchoolCode;
        private string militarySchoolName;
        private bool isActive;

        public int MilitarySchoolId
        {
            get { return militarySchoolId; }
            set { militarySchoolId = value; }
        }

        public string MilitarySchoolCode
        {
            get { return militarySchoolCode; }
            set { militarySchoolCode = value; }
        }

        public string MilitarySchoolName
        {
            get { return militarySchoolName; }
            set { militarySchoolName = value; }
        }

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        //IDropDownItem Members
        public string Text()
        {
            return MilitarySchoolName;
        }

        public string Value()
        {
            return MilitarySchoolId.ToString();
        }
    }

    public static class MilitarySchoolUtil
    {
        public static MilitarySchool GetMilitarySchool(int militarySchoolId, User currentUser)
        {
            MilitarySchool militarySchool = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.VVU_KOD as MilitarySchoolCode, a.VVU_IME as MilitarySchoolName, a.IsActive
                               FROM VS_OWNER.KLV_VVU a
                               WHERE a.VVUID = :MilitarySchoolID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitarySchoolID", OracleType.Number).Value = militarySchoolId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    militarySchool = new MilitarySchool();
                    militarySchool.MilitarySchoolId = militarySchoolId;
                    militarySchool.MilitarySchoolCode = dr["MilitarySchoolCode"].ToString();
                    militarySchool.MilitarySchoolName = dr["MilitarySchoolName"].ToString();
                    militarySchool.IsActive = (dr["IsActive"].ToString() == "1" ? true : false);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militarySchool;
        }

        public static List<MilitarySchool> GetAllMilitarySchools(User currentUser, bool onlyActive)
        {
            List<MilitarySchool> militarySchools = new List<MilitarySchool>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                string whereClause = "";

                if (onlyActive)
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " NVL(a.IsActive, 0) = 1";
                }

                whereClause = (whereClause == "" ? "" : " WHERE ") + whereClause;

                conn.Open();
                string SQL = @"SELECT a.VVU_KOD as MilitarySchoolCode, a.VVU_IME as MilitarySchoolName, 
                               a.VVUID as MilitarySchoolID, a.IsActive
                               FROM VS_OWNER.KLV_VVU a
                               " + whereClause + @"
                               ORDER BY a.VVU_IME";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    MilitarySchool militarySchool = new MilitarySchool();
                    militarySchool.MilitarySchoolId = DBCommon.GetInt(dr["MilitarySchoolID"]);
                    militarySchool.MilitarySchoolCode = dr["MilitarySchoolCode"].ToString();
                    militarySchool.MilitarySchoolName = dr["MilitarySchoolName"].ToString();
                    militarySchool.IsActive = (dr["IsActive"].ToString() == "1" ? true : false);

                    militarySchools.Add(militarySchool);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militarySchools;
        }

        //Проф. серж. колеж
        public static string CollegeDBKey = "а";
    }
}
