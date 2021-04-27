using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using PMIS.Common;

namespace PMIS.Common
{
    //Represents single discharge reason
    public class DischargeReason : IDropDownItem
    {
        public string DischargeReasonCode { get; set; }
        public string DischargeReasonName { get; set; }

        //IDropDownItem Members
        public string Text()
        {
            return DischargeReasonName;
        }

        public string Value()
        {
            return DischargeReasonCode;
        }

    }

    public static class DischargeReasonUtil
    {
        public static DischargeReason GetDischargeReason(string DischargeReasonCode, User currentUser)
        {
            DischargeReason dischargeReason = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.SNO_KOD as DischargeReasonCode, a.SNO_IME as DischargeReasonName
                               FROM VS_OWNER.KLV_SNO a
                               WHERE a.SNO_KOD = :DischargeReasonCode";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("DischargeReasonCode", OracleType.VarChar).Value = DischargeReasonCode;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    dischargeReason = new DischargeReason();
                    dischargeReason.DischargeReasonCode = dr["DischargeReasonCode"].ToString();
                    dischargeReason.DischargeReasonName = dr["DischargeReasonName"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return dischargeReason;
        }

        public static List<DischargeReason> GetAllDischargeReasons(User currentUser)
        {
            List<DischargeReason> listDischargeReason = new List<DischargeReason>();
            DischargeReason dischargeReason;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.SNO_KOD as DischargeReasonCode, a.SNO_IME as DischargeReasonName
                               FROM VS_OWNER.KLV_SNO a
                               ORDER BY a.SNO_IME";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    dischargeReason = new DischargeReason();
                    dischargeReason.DischargeReasonCode = dr["DischargeReasonCode"].ToString();
                    dischargeReason.DischargeReasonName = dr["DischargeReasonName"].ToString();

                    listDischargeReason.Add(dischargeReason);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listDischargeReason;
        }

        public static string DischargeReasonSelector_SearchDischargeReasons(string searchType, string searchText, int maxRowNumbers, User currentUser)
        {
            StringBuilder sb = new StringBuilder();
            string SQL = "";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);

            conn.Open();

            sb.Append("<response>");

            try
            {
                SQL = @"SELECT * 
                        FROM (SELECT a.SNO_KOD as DischargeReasonCode, 
                                     a.SNO_IME as DischargeReasonName
                              FROM VS_OWNER.KLV_SNO a
                              WHERE UPPER(a.SNO_IME) LIKE UPPER(:searchText)
                              ORDER BY UPPER(a.SNO_IME)
                             ) tmp
                        ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                string searchTextLIKE = "";
                switch (searchType)
                {
                    case "starts_with":
                        searchTextLIKE = searchText.Trim() + "%";
                        break;
                    case "contains":
                        searchTextLIKE = "%" + searchText.Trim() + "%";
                        break;
                    case "ends_with":
                        searchTextLIKE = "%" + searchText.Trim();
                        break;
                }

                cmd.Parameters.Add("searchText", OracleType.VarChar).Value = searchTextLIKE;

                OracleDataReader dr = cmd.ExecuteReader();

                sb.Append("<result>");
                while (dr.Read())
                {
                    sb.Append("<dischargeReason>");
                    sb.Append("<dischargeReasonName>");
                    sb.Append(AJAXTools.EncodeForXML(dr["DischargeReasonName"].ToString()));
                    sb.Append("</dischargeReasonName>");
                    sb.Append("<dischargeReasonCode>");
                    sb.Append(AJAXTools.EncodeForXML(dr["DischargeReasonCode"].ToString()));
                    sb.Append("</dischargeReasonCode>");
                    sb.Append("</dischargeReason>");
                }

                dr.Close();

                SQL = @"SELECT COUNT(*) as Cnt
                        FROM VS_OWNER.KLV_SNO a
                              WHERE UPPER(a.SNO_IME) LIKE UPPER(:searchText)
                        ";

                cmd = new OracleCommand(SQL, conn);
                cmd.Parameters.Add("searchText", OracleType.VarChar).Value = searchTextLIKE;

                dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    sb.Append("<totalRowsCount>");
                    sb.Append(AJAXTools.EncodeForXML(dr["Cnt"].ToString()));
                    sb.Append("</totalRowsCount>");
                }

                dr.Close();

                sb.Append("</result>");
            }
            finally
            {
                conn.Close();
            }

            sb.Append("</response>");

            return sb.ToString();
        }
    }
}
