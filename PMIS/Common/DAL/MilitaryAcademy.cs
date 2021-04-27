using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using PMIS.Common;

namespace PMIS.Common
{
    //Represents single person education from VS_OWNER.KLV_VVA table
    public class MilitaryAcademy : IDropDownItem
    {
        private string militaryAcademyCode;
        private string militaryAcademyName;

        public string MilitaryAcademyCode
        {
            get { return militaryAcademyCode; }
            set { militaryAcademyCode = value; }
        }

        public string MilitaryAcademyName
        {
            get { return militaryAcademyName; }
            set { militaryAcademyName = value; }
        }

        //IDropDownItem Members
        public string Text()
        {
            return MilitaryAcademyName;
        }

        public string Value()
        {
            return MilitaryAcademyCode;
        }

    }

    public static class MilitaryAcademyUtil
    {
        public static MilitaryAcademy GetMilitaryAcademy(string militaryAcademyCode, User currentUser)
        {
            MilitaryAcademy militaryAcademy = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.VVA_KOD as MilitaryAcademyCode, a.VVA_IME as MilitaryAcademyName
                               FROM VS_OWNER.KLV_VVA a
                               WHERE a.VVA_KOD = :MilitaryAcademyCode";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitaryAcademyCode", OracleType.VarChar).Value = militaryAcademyCode;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    militaryAcademy = new MilitaryAcademy();
                    militaryAcademy.MilitaryAcademyCode = dr["MilitaryAcademyCode"].ToString();
                    militaryAcademy.MilitaryAcademyName = dr["MilitaryAcademyName"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryAcademy;
        }

        public static List<MilitaryAcademy> GetAllMilitaryAcademys(User currentUser)
        {
            List<MilitaryAcademy> listMilitaryAcademy = new List<MilitaryAcademy>();
            MilitaryAcademy militaryAcademy;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.VVA_KOD as MilitaryAcademyCode, a.VVA_IME as MilitaryAcademyName
                               FROM VS_OWNER.KLV_VVA a
                               ORDER BY a.VVA_IME";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    militaryAcademy = new MilitaryAcademy();
                    militaryAcademy.MilitaryAcademyCode = dr["MilitaryAcademyCode"].ToString();
                    militaryAcademy.MilitaryAcademyName = dr["MilitaryAcademyName"].ToString();

                    listMilitaryAcademy.Add(militaryAcademy);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listMilitaryAcademy;
        }
    }
}
