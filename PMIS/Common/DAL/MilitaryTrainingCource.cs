using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using PMIS.Common;

namespace PMIS.Common
{
    public class MilitaryTrainingCource : IDropDownItem
    {
        private string militaryTrainingCourceCode;

        public string MilitaryTrainingCourceCode
        {
            get { return militaryTrainingCourceCode; }
            set { militaryTrainingCourceCode = value; }
        }

        private string militaryTrainingCourceName;

        public string MilitaryTrainingCourceName
        {
            get { return militaryTrainingCourceName; }
            set { militaryTrainingCourceName = value; }
        }

        //IDropDownItem Members
        public string Text()
        {
            return MilitaryTrainingCourceName;
        }

        public string Value()
        {
            return MilitaryTrainingCourceCode;
        }
    }

    public class MilitaryTrainingCourceUtil
    {
        public static MilitaryTrainingCource GetMilitaryTrainingCource(string militaryTrainingCourceCode, User currentUser)
        {
            MilitaryTrainingCource militaryTrainingCource = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.KUS_KOD as MilitaryTrainingCourceCode, a.KUS_IME as MilitaryTrainingCourceName
                               FROM VS_OWNER.KLV_KUS a
                               WHERE a.KUS_KOD = :MilitaryTrainingCourceCode";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitaryTrainingCourceCode", OracleType.VarChar).Value = militaryTrainingCourceCode;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    militaryTrainingCource = new MilitaryTrainingCource();
                    militaryTrainingCource.MilitaryTrainingCourceCode = dr["MilitaryTrainingCourceCode"].ToString();
                    militaryTrainingCource.MilitaryTrainingCourceName = dr["MilitaryTrainingCourceName"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryTrainingCource;
        }

        public static List<MilitaryTrainingCource> GetAllMilitaryTrainingCources(User currentUser)
        {
            List<MilitaryTrainingCource> listMilitaryTrainingCources = new List<MilitaryTrainingCource>();
            MilitaryTrainingCource militaryTrainingCource;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.KUS_KOD as MilitaryTrainingCourceCode, a.KUS_IME as MilitaryTrainingCourceName
                               FROM VS_OWNER.KLV_KUS a
                               ORDER BY a.KUS_IME";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    militaryTrainingCource = new MilitaryTrainingCource();
                    militaryTrainingCource.MilitaryTrainingCourceCode = dr["MilitaryTrainingCourceCode"].ToString();
                    militaryTrainingCource.MilitaryTrainingCourceName = dr["MilitaryTrainingCourceName"].ToString();

                    listMilitaryTrainingCources.Add(militaryTrainingCource);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listMilitaryTrainingCources;
        }
    }
}
