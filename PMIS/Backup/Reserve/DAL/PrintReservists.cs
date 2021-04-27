using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using System.Linq;

using PMIS.Common;
using System.Collections;

namespace PMIS.Reserve.Common
{
    //This class represents all information about the filter, the order and the paging information on the screen
    public class PrintReservistsFilter
    {
        public string MilitaryDepartmentIds { get; set; }
        public int MilitaryCommandId { get; set; }
        public string MilitaryCommandSuffix { get; set; }
        public string IdentNumber { get; set; }
        public int Readiness { get; set; }

        public int OrderBy { get; set; }
        public int PageIdx { get; set; }
    }

    public class PrintReservistsResultBlock
    {
        public int ReservistId { get; set; }
        public string IdentityNumber { get; set; }
        public string FullName { get; set; }
        public string MilitaryRankName { get; set; }
    }

    public class PrintReservistsResult
    {
        private int rowsPerPage;
        private User currentUser;
        private PrintReservistsFilter filter;

        public PrintReservistsResult(int rowsPerPage, User currentUser)
        {
            this.rowsPerPage = rowsPerPage;
            this.currentUser = currentUser;
        }

        private int? allRecordsCount = null;
        public int AllRecordsCount 
        {
            get
            {
                if (!allRecordsCount.HasValue)
                {
                    allRecordsCount = PrintReservistsUtil.GetPrintReservistsResultsCount(filter, currentUser);
                }

                return allRecordsCount.Value;
            }
        }

        private List<PrintReservistsResultBlock> overallResult = null;
        public List<PrintReservistsResultBlock> OverallResult
        {
            get
            {
                if (overallResult == null)
                {
                    overallResult = PrintReservistsUtil.GetPrintReservistsResults(filter, rowsPerPage, currentUser);
                }

                return overallResult;
            }
        }

        public PrintReservistsFilter Filter
        {
            get
            {
                return filter;
            }

            set
            {
                filter = value;
                allRecordsCount = null;
            }
        }
    }

    public static class PrintReservistsUtil
    {
        public static List<PrintReservistsResultBlock> GetPrintReservistsResults(PrintReservistsFilter filter, int rowsPerPage, User currentUser)
        {
            List<PrintReservistsResultBlock> result = new List<PrintReservistsResultBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string whereClause = "";

                if (!String.IsNullOrEmpty(filter.MilitaryDepartmentIds))
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " d.MilitaryDepartmentId IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryDepartmentIds) + ") ";
                }

                whereClause += (whereClause == "" ? "" : " AND ") +
                        @" (d.MilitaryDepartmentId IS NULL OR d.MilitaryDepartmentId IN ( " + currentUser.MilitaryDepartmentIDs + ")) ";


                if (filter.MilitaryCommandId > -1)
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " b.MilitaryCommandId = " + filter.MilitaryCommandId.ToString();

                }

                if (!String.IsNullOrEmpty(filter.MilitaryCommandSuffix) && filter.MilitaryCommandSuffix != "-1")
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " b.MilitaryCommandSuffix = '" + filter.MilitaryCommandSuffix + "' ";
                }

                if (!string.IsNullOrEmpty(filter.IdentNumber))
                {
                    filter.IdentNumber = filter.IdentNumber.Trim();

                    if (filter.IdentNumber.Length > 0)
                    {
                        whereClause += (whereClause == "" ? "(" : " AND (");

                        string[] identNumbers = filter.IdentNumber.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        for (int i = 0; i < identNumbers.Length; i++)
                        {
                            if (i < identNumbers.Length - 1)
                            {
                                whereClause += " g.EGN LIKE '" + identNumbers[i].Replace("'", "''") + "%' OR ";
                            }
                            else
                            {
                                whereClause += " g.EGN LIKE '" + identNumbers[i].Replace("'", "''") + "%') ";
                            }
                        }
                    }
                }
                
                if (filter.Readiness > -1)
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                         " d.RESERVISTREADINESSID = " + filter.Readiness + " ";
                  
                }
                
                whereClause = (whereClause == "" ? "" : " WHERE ") + whereClause;

                string pageWhere = "";

                if (filter.PageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE RowNumber BETWEEN (" + filter.PageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + filter.PageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

                string orderBySQL = "";
                string orderByDir = "ASC";

                if (filter.OrderBy > 100)
                {
                    filter.OrderBy -= 100;
                    orderByDir = "DESC";
                }

                switch (filter.OrderBy)
                {
                    case 1:
                        orderBySQL = "g.EGN";
                        break;
                    case 2:
                        orderBySQL = "g.IME || ' ' || g.FAM";
                        break;
                    case 3:
                        orderBySQL = "h.ZVA_IME";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"
                               SELECT tmp.*
                               FROM 
                                  (
                                    SELECT f.ReservistId,
                                           g.EGN as IdentityNumber,
                                           g.IME || ' ' || g.FAM as FullName,
                                           h.ZVA_IME as MilitaryRankName,
                                           RANK() OVER (ORDER BY " + orderBySQL + @", f.ReservistID) as RowNumber 
                                    FROM UKAZ_OWNER.VVR a
                                    INNER JOIN PMIS_RES.RequestsCommands b ON b.MilitaryCommandId = a.KOD_VVR
                                    INNER JOIN PMIS_RES.RequestCommandPositions c ON c.RequestsCommandID = b.RequestsCommandID
                                    INNER JOIN PMIS_RES.FillReservistsRequest d ON d.RequestCommandPositionID = c.RequestCommandPositionID 
                                    INNER JOIN PMIS_RES.Reservists f ON f.ReservistID = d.ReservistID
                                    INNER JOIN VS_OWNER.VS_LS g ON g.PersonID = f.PersonID
                                    LEFT OUTER JOIN VS_OWNER.KLV_ZVA h ON g.KOD_ZVA = h.ZVA_KOD
                                    " + whereClause + @"
                                  ) tmp
                                " + pageWhere + @"
                                ORDER BY RowNumber
                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    PrintReservistsResultBlock block = new PrintReservistsResultBlock();
                    block.ReservistId = DBCommon.GetInt(dr["ReservistId"]);
                    block.IdentityNumber = dr["IdentityNumber"].ToString();
                    block.FullName = dr["FullName"].ToString();
                    block.MilitaryRankName = dr["MilitaryRankName"].ToString();

                    result.Add(block);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public static int GetPrintReservistsResultsCount(PrintReservistsFilter filter, User currentUser)
        {
            int cnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string whereClause = "";

                if (!String.IsNullOrEmpty(filter.MilitaryDepartmentIds))
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " d.MilitaryDepartmentId IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryDepartmentIds) + ") ";
                }

                whereClause += (whereClause == "" ? "" : " AND ") +
                        @" (d.MilitaryDepartmentId IS NULL OR d.MilitaryDepartmentId IN ( " + currentUser.MilitaryDepartmentIDs + ")) ";


                if (filter.MilitaryCommandId > -1)
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " b.MilitaryCommandId = " + filter.MilitaryCommandId.ToString();

                }

                if (!String.IsNullOrEmpty(filter.MilitaryCommandSuffix) && filter.MilitaryCommandSuffix != "-1")
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " b.MilitaryCommandSuffix = '" + filter.MilitaryCommandSuffix + "' ";
                }

                if (!string.IsNullOrEmpty(filter.IdentNumber))
                {
                    if (filter.IdentNumber.Length == 10)
                        whereClause += (whereClause == "" ? "" : " AND ") +
                             " g.EGN = '" + filter.IdentNumber.Replace("'", "''") + "' ";
                    else
                        whereClause += (whereClause == "" ? "" : " AND ") +
                                 " g.EGN LIKE '" + filter.IdentNumber.Replace("'", "''") + "%' ";
                }

                if (filter.Readiness > -1)
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                         " d.RESERVISTREADINESSID = " + filter.Readiness + " ";

                }

                whereClause = (whereClause == "" ? "" : " WHERE ") + whereClause;

                string SQL = @"
                               SELECT COUNT(*) as Cnt
                               FROM UKAZ_OWNER.VVR a
                               INNER JOIN PMIS_RES.RequestsCommands b ON b.MilitaryCommandId = a.KOD_VVR
                               INNER JOIN PMIS_RES.RequestCommandPositions c ON c.RequestsCommandID = b.RequestsCommandID
                               INNER JOIN PMIS_RES.FillReservistsRequest d ON d.RequestCommandPositionID = c.RequestCommandPositionID 
                               INNER JOIN PMIS_RES.Reservists f ON f.ReservistID = d.ReservistID
                               INNER JOIN VS_OWNER.VS_LS g ON g.PersonID = f.PersonID
                               LEFT OUTER JOIN VS_OWNER.KLV_ZVA h ON g.KOD_ZVA = h.ZVA_KOD
                              " + whereClause;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        cnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return cnt;
        }
    }
}