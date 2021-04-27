using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using PMIS.Common;

namespace PMIS.Common
{
    //Represents single conviction
    public class Conviction : IDropDownItem
    {
        private string convictionCode;
        private string convictionName;

        public string ConvictionCode
        {
            get { return convictionCode; }
            set { convictionCode = value; }
        }

        public string ConvictionName
        {
            get { return convictionName; }
            set { convictionName = value; }
        }

        //IDropDownItem Members
        public string Text()
        {
            return ConvictionName;
        }

        public string Value()
        {
            return ConvictionCode;
        }

    }

    public static class ConvictionUtil
    {
        public static Conviction GetConviction(string convictionCode, User currentUser)
        {
            Conviction conviction = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.SDM_KOD as ConvictionCode, a.SDM_IME as ConvictionName
                               FROM VS_OWNER.KLV_SDM a
                               WHERE a.SDM_KOD = :ConvictionCode";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ConvictionCode", OracleType.VarChar).Value = convictionCode;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    conviction = new Conviction();
                    conviction.ConvictionCode = dr["ConvictionCode"].ToString();
                    conviction.ConvictionName = dr["ConvictionName"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return conviction;
        }

        public static List<Conviction> GetAllConvictions(User currentUser)
        {
            List<Conviction> listConviction = new List<Conviction>();
            Conviction conviction;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.SDM_KOD as ConvictionCode, a.SDM_IME as ConvictionName
                               FROM VS_OWNER.KLV_SDM a
                               ORDER BY a.SDM_IME";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    conviction = new Conviction();
                    conviction.ConvictionCode = dr["ConvictionCode"].ToString();
                    conviction.ConvictionName = dr["ConvictionName"].ToString();

                    listConviction.Add(conviction);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listConviction;
        }
    }
}
