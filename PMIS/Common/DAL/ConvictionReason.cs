using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using PMIS.Common;

namespace PMIS.Common
{
    //Represents single conviction reason
    public class ConvictionReason : IDropDownItem
    {
        private string convictionReasonCode;
        private string convictionReasonName;

        public string ConvictionReasonCode
        {
            get { return convictionReasonCode; }
            set { convictionReasonCode = value; }
        }

        public string ConvictionReasonName
        {
            get { return convictionReasonName; }
            set { convictionReasonName = value; }
        }

        //IDropDownItem Members
        public string Text()
        {
            return ConvictionReasonName;
        }

        public string Value()
        {
            return ConvictionReasonCode;
        }

    }

    public static class ConvictionReasonUtil
    {
        public static ConvictionReason GetConvictionReason(string convictionReasonCode, User currentUser)
        {
            ConvictionReason convictionReason = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.PSD_KOD as ConvictionReasonCode, a.PSD_IME as ConvictionReasonName
                               FROM VS_OWNER.KLV_PSD a
                               WHERE a.PSD_KOD = :ConvictionReasonCode";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ConvictionReasonCode", OracleType.VarChar).Value = convictionReasonCode;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    convictionReason = new ConvictionReason();
                    convictionReason.ConvictionReasonCode = dr["ConvictionReasonCode"].ToString();
                    convictionReason.ConvictionReasonName = dr["ConvictionReasonName"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return convictionReason;
        }

        public static List<ConvictionReason> GetAllConvictionReasons(User currentUser)
        {
            List<ConvictionReason> listConvictionReason = new List<ConvictionReason>();
            ConvictionReason convictionReason;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.PSD_KOD as ConvictionReasonCode, a.PSD_IME as ConvictionReasonName
                               FROM VS_OWNER.KLV_PSD a
                               ORDER BY a.PSD_IME";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    convictionReason = new ConvictionReason();
                    convictionReason.ConvictionReasonCode = dr["ConvictionReasonCode"].ToString();
                    convictionReason.ConvictionReasonName = dr["ConvictionReasonName"].ToString();

                    listConvictionReason.Add(convictionReason);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listConvictionReason;
        }
    }
}
