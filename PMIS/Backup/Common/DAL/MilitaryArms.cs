using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;

namespace PMIS.Common
{
    public class MilitaryArms : IDropDownItem
    {
        private string militaryArmsCode;

        public string MilitaryArmsCode
        {
            get { return militaryArmsCode; }
            set { militaryArmsCode = value; }
        }
        private string militaryArmsName;

        public string MilitaryArmsName
        {
            get { return militaryArmsName; }
            set { militaryArmsName = value; }
        }

        //IDropDownItem Members
        public string Text()
        {
            return MilitaryArmsName;
        }

        public string Value()
        {
            return MilitaryArmsCode;
        }
    }
    public class MilitaryArmsUtil
    {
        public static MilitaryArms GetMilitaryArms(string militaryArmsCode, User currentUser)
        {
            MilitaryArms militaryArms = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.RVU_KOD as MilitaryArmsCode, a.RVU_IME as MilitaryArmsName
                               FROM VS_OWNER.KLV_RVU a
                               WHERE a.RVU_KOD = :MilitaryArmsCode";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitaryArmsCode", OracleType.VarChar).Value = militaryArmsCode;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    militaryArms = new MilitaryArms();
                    militaryArms.MilitaryArmsCode = militaryArmsCode;
                    militaryArms.MilitaryArmsName = dr["MilitaryArmsName"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryArms;
        }

        public static List<MilitaryArms> GetAllMilitaryArms(User currentUser)
        {
            List<MilitaryArms> listMilitaryArms = new List<MilitaryArms>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.RVU_KOD as MilitaryArmsCode, a.RVU_IME as MilitaryArmsName
                               FROM VS_OWNER.KLV_RVU a
                               ORDER BY a.RVU_IME";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    MilitaryArms militaryArms = new MilitaryArms();
                    militaryArms.MilitaryArmsCode = dr["MilitaryArmsCode"].ToString();
                    militaryArms.MilitaryArmsName = dr["MilitaryArmsName"].ToString();
                    listMilitaryArms.Add(militaryArms);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listMilitaryArms;
        }
    }
}
