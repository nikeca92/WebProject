using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.OracleClient;
using PMIS.Common;
using System.Text;

namespace PMIS.Common
{
    public class NKPD : BaseDbObject
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Nickname { get; set; }
        public bool IsActive { get; set; }

        public string CodeDisplay
        {
            get
            {
                string codeDisplay = "";

                if (!String.IsNullOrEmpty(Code))
                {
                    codeDisplay = Code.Replace(" ", "");

                    if (codeDisplay.Length > 4)
                        codeDisplay = codeDisplay.Substring(0, 4) + " " + codeDisplay.Substring(4);
                }

                return codeDisplay;
            }
        }

        public string CodeAndNameDisplay
        {
            get
            {
                return CodeDisplay + " " + Name;
            }
        }

        public string ClassAndCodeAndNameDisplay
        {
            get
            {
                NKPD root = NKPDUtil.GetRootNKPD(Id, CurrentUser);
                return "(" + root.Nickname + ") " + CodeDisplay + " " + Name;
            }
        }

        public NKPD (User user)
            : base(user)
        {
        }
    }

    public class SearchNKPDFilter
    {
        public int? Level { get; set; }
        public string ParentIDs { get; set; }
        public string NKPDCode { get; set; }
        public string NKPDName { get; set; }
    }

    public class NKPDUtil
    {
        private static NKPD ExtractNKPDFromDR(OracleDataReader dr, User currentUser)
        {
            NKPD nkpd = new NKPD(currentUser);

            nkpd.Id = DBCommon.GetInt(dr["NKPDID"]);
            nkpd.ParentId = DBCommon.IsInt(dr["NKPDParentID"]) ? (int?)DBCommon.GetInt(dr["NKPDParentID"]) : null;
            nkpd.Code = dr["NKPDCode"].ToString();
            nkpd.Name = dr["NKPDName"].ToString();
            nkpd.Nickname = dr["NKPDNickname"].ToString();
            nkpd.IsActive = (DBCommon.IsInt(dr["IsActive"]) && DBCommon.GetInt(dr["IsActive"]) == 1);

            return nkpd;
        }

        public static NKPD GetNKPD(int id, User currentUser)
        {
            NKPD nkpd = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.NKPDID, a.NKPDParentID,
                                      a.NKPDCode, a.NKPDName, a.NKPDNickname,
                                      a.IsActive
                               FROM PMIS_ADM.NKPD a
                               WHERE a.NKPDID = :NKPDID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("NKPDID", OracleType.Number).Value = id;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    nkpd = ExtractNKPDFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return nkpd;
        }

        public static NKPD GetNKPDByCode(string code, User currentUser)
        {
            NKPD nkpd = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.NKPDID, a.NKPDParentID,
                                      a.NKPDCode, a.NKPDName, a.NKPDNickname,
                                      a.IsActive
                               FROM PMIS_ADM.NKPD a
                               WHERE a.NKPDCode = :NKPDCode AND 
                                     a.NKPDID NOT IN (SELECT NKPDParentID FROM PMIS_ADM.NKPD WHERE NKPDParentID IS NOT NULL)";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("NKPDCode", OracleType.VarChar).Value = code;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    nkpd = ExtractNKPDFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return nkpd;
        }

        public static NKPD GetRootNKPD(int id, User currentUser)
        {
            NKPD nkpd = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.NKPDID, a.NKPDParentID,
                                      a.NKPDCode, a.NKPDName, a.NKPDNickname,
                                      a.IsActive
                               FROM PMIS_ADM.NKPD a
                               WHERE a.NKPDID = PMIS_ADM.CommonFunctions.GetNKPDRootID(:NKPDID)";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("NKPDID", OracleType.Number).Value = id;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    nkpd = ExtractNKPDFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return nkpd;
        }

        //We use this function for two things:
        // - when populating the drop-down lists of parent NKPD elements for the filter area
        // - when populating the NKPD items for the result list
        public static List<NKPD> GetNKPDListForSearch(SearchNKPDFilter filter, User currentUser)
        {
            List<NKPD> NKPDs = new List<NKPD>();

            string whereClause = "";

            whereClause += (whereClause == "" ? "" : " AND ") + " NVL(a.IsActive, 0) = 1 ";

            //If the Level (from filter) is not set then the select will return only the lowest level NKPD records (the profession).
            if (filter.Level.HasValue)
            {
                whereClause += (whereClause == "" ? "" : " AND ") + " PMIS_ADM.CommonFunctions.GetNKPDLevel(a.NKPDID) = " + filter.Level.Value + " ";
            }
            else
            {
                whereClause += (whereClause == "" ? "" : " AND ") + " a.NKPDID NOT IN (SELECT NKPDParentID FROM PMIS_ADM.NKPD WHERE NKPDParentID IS NOT NULL) ";
            }

            string[] parentIDs = filter.ParentIDs.Split(new[]{","}, StringSplitOptions.RemoveEmptyEntries);
            foreach (string parentID in parentIDs)
            {
                int intParentID = int.Parse(parentID);
                whereClause += (whereClause == "" ? "" : " AND ") + "PMIS_ADM.CommonFunctions.IsNKPDChildOf(a.NKPDID, " + intParentID + ") = 1";
            }

            if (!String.IsNullOrEmpty(filter.NKPDCode))
            {
                whereClause += (whereClause == "" ? "" : " AND ") + " UPPER(a.NKPDCode) LIKE '%' || UPPER('" + filter.NKPDCode.Replace("'", "''") + "') || '%' ";
            }

            if (!String.IsNullOrEmpty(filter.NKPDName))
            {
                whereClause += (whereClause == "" ? "" : " AND ") + " UPPER(a.NKPDName) LIKE '%' || UPPER('" + filter.NKPDName.Replace("'", "''") + "') || '%' ";
            }

            whereClause = (whereClause == "" ? "" : " WHERE ") + whereClause;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.NKPDID, a.NKPDParentID,
                                      a.NKPDCode, a.NKPDName, a.NKPDNickname,
                                      a.IsActive
                               FROM PMIS_ADM.NKPD a " + whereClause + @"
                               ORDER BY a.NKPDCode";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    NKPD nkpd = ExtractNKPDFromDR(dr, currentUser);
                    NKPDs.Add(nkpd);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return NKPDs;
        }
    }
}
