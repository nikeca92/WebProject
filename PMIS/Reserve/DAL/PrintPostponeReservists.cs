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
    public class PrintPostponeReservistsFilter
    {
        public string MilitaryDepartmentIds { get; set; }
        public string AdministrationId { get; set; }
        public string CompanyIds { get; set; }

        public int OrderBy { get; set; }
        public int PageIdx { get; set; }
    }

    public class PrintPostponeReservistsResultBlock
    {
        public int ReservistId { get; set; }
        public string IdentityNumber { get; set; }
        public string FullName { get; set; }
        public string MilitaryRankName { get; set; }
    }

    public class PrintPostponeReservistsResult
    {
        private int rowsPerPage;
        private User currentUser;
        private PrintPostponeReservistsFilter filter;

        public PrintPostponeReservistsResult(int rowsPerPage, User currentUser)
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
                    allRecordsCount = PrintPostponeReservistsUtil.GetPrintPostponeReservistsResultsCount(filter, currentUser);
                }

                return allRecordsCount.Value;
            }
        }

        private List<PrintPostponeReservistsResultBlock> overallResult = null;
        public List<PrintPostponeReservistsResultBlock> OverallResult
        {
            get
            {
                if (overallResult == null)
                {
                    overallResult = PrintPostponeReservistsUtil.GetPrintPostponeReservistsResults(filter, rowsPerPage, currentUser);
                }

                return overallResult;
            }
        }

        public PrintPostponeReservistsFilter Filter
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

    public static class PrintPostponeReservistsUtil
    {
        public static List<PrintPostponeReservistsResultBlock> GetPrintPostponeReservistsResults(PrintPostponeReservistsFilter filter, int rowsPerPage, User currentUser)
        {
            List<PrintPostponeReservistsResultBlock> result = new List<PrintPostponeReservistsResultBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string whereClause = "WHERE j.MilitaryReportStatusKey = 'POSTPONED'";

                if (!String.IsNullOrEmpty(filter.MilitaryDepartmentIds))
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " k.MilitaryDepartmentId IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryDepartmentIds) + ") ";
                }

                whereClause += (whereClause == "" ? "" : " AND ") +
                        @" (k.MilitaryDepartmentId IS NULL OR k.MilitaryDepartmentId IN ( " + currentUser.MilitaryDepartmentIDs + ")) ";


                if (!String.IsNullOrEmpty(filter.AdministrationId))
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " h.AdministrationId = " + filter.AdministrationId.ToString();

                }

                if (!String.IsNullOrEmpty(filter.CompanyIds))
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " g.CompanyId IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.CompanyIds) + ") ";
                }

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
                        orderBySQL = "b.EGN";
                        break;
                    case 2:
                        orderBySQL = "b.IME || ' ' || b.FAM";
                        break;
                    case 3:
                        orderBySQL = "c.ZVA_IME";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"
                               SELECT tmp.*
                               FROM 
                                  (
                                    SELECT a.ReservistId,
                                           b.EGN as IdentityNumber,
                                           b.IME || ' ' || b.FAM as FullName,
                                           c.ZVA_IME as MilitaryRankName,
                                           g.CompanyName as Company,                                        
                                           k.MilitaryDepartmentName as MilitaryDepartmentName,
                                           j.MilitaryReportStatusName,
                                           h.AdministrationName, 
                                           RANK() OVER (ORDER BY " + orderBySQL + @", a.ReservistID) as RowNumber
                                      
                                   FROM PMIS_RES.Reservists a
                                   INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                                   LEFT OUTER JOIN VS_OWNER.KLV_ZVA c ON b.KOD_ZVA = c.ZVA_KOD
                                   INNER JOIN PMIS_ADM.Persons d ON b.PersonID = d.PersonID
                                   LEFT OUTER JOIN PMIS_ADM.PersonMilRepSpec e ON e.PersonID = b.PersonID AND e.IsPrimary = 1
                                   LEFT OUTER JOIN PMIS_ADM.MilitaryReportSpecialities f ON f.MilReportSpecialityID = e.MilReportSpecialityID
                                   LEFT OUTER JOIN PMIS_ADM.Companies g ON g.CompanyID = d.WorkCompanyID
                                   LEFT OUTER JOIN PMIS_ADM.Administrations h ON g.AdministrationId = h.AdministrationId
                                   LEFT OUTER JOIN PMIS_RES.ReservistMilRepStatuses i ON i.ReservistID = a.ReservistID AND i.IsCurrent = 1
                                   LEFT OUTER JOIN PMIS_RES.MilitaryReportStatuses j ON i.MilitaryReportStatusId = j.MilitaryReportStatusId
                                   LEFT OUTER JOIN PMIS_ADM.MilitaryDepartments k ON k.MilitaryDepartmentID = i.SourceMilDepartmentID
                                   " + whereClause + @"
                                  ) tmp
                                " + pageWhere + @"
                                ORDER BY RowNumber
                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    PrintPostponeReservistsResultBlock block = new PrintPostponeReservistsResultBlock();
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

        public static int GetPrintPostponeReservistsResultsCount(PrintPostponeReservistsFilter filter, User currentUser)
        {
            int cnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string whereClause = "WHERE j.MilitaryReportStatusKey = 'POSTPONED'";

                if (!String.IsNullOrEmpty(filter.MilitaryDepartmentIds))
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " k.MilitaryDepartmentId IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryDepartmentIds) + ") ";
                }

                whereClause += (whereClause == "" ? "" : " AND ") +
                        @" (k.MilitaryDepartmentId IS NULL OR k.MilitaryDepartmentId IN ( " + currentUser.MilitaryDepartmentIDs + ")) ";


                if (!String.IsNullOrEmpty(filter.AdministrationId))
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " h.AdministrationId = " + filter.AdministrationId.ToString();

                }

                if (!String.IsNullOrEmpty(filter.CompanyIds))
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " g.CompanyId IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.CompanyIds) + ") ";
                }

                string SQL = @"
                                   SELECT COUNT(*) as Cnt
                                   FROM PMIS_RES.Reservists a
                                   INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                                   LEFT OUTER JOIN VS_OWNER.KLV_ZVA c ON b.KOD_ZVA = c.ZVA_KOD
                                   INNER JOIN PMIS_ADM.Persons d ON b.PersonID = d.PersonID
                                   LEFT OUTER JOIN PMIS_ADM.PersonMilRepSpec e ON e.PersonID = b.PersonID AND e.IsPrimary = 1
                                   LEFT OUTER JOIN PMIS_ADM.MilitaryReportSpecialities f ON f.MilReportSpecialityID = e.MilReportSpecialityID
                                   LEFT OUTER JOIN PMIS_ADM.Companies g ON g.CompanyID = d.WorkCompanyID
                                   LEFT OUTER JOIN PMIS_ADM.Administrations h ON g.AdministrationId = h.AdministrationId
                                   LEFT OUTER JOIN PMIS_RES.ReservistMilRepStatuses i ON i.ReservistID = a.ReservistID AND i.IsCurrent = 1
                                   LEFT OUTER JOIN PMIS_RES.MilitaryReportStatuses j ON i.MilitaryReportStatusId = j.MilitaryReportStatusId
                                   LEFT OUTER JOIN PMIS_ADM.MilitaryDepartments k ON k.MilitaryDepartmentID = i.SourceMilDepartmentID
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

        //Get a list of all Companies for the selected filter
        public static List<Company> GetCompaniesList(User currentUser, PrintPostponeReservistsFilter filter)
        {
            List<Company> companies = new List<Company>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                if (!String.IsNullOrEmpty(filter.MilitaryDepartmentIds))
                {
                    where += (where == "" ? "" : " AND ") +
                        " c.MilitaryDepartmentID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryDepartmentIds) + ") ";
                }
                else
                {
                    //If there is no military department selected then do not show any companies to prevent loading a huge list of companies
                    where += (where == "" ? "" : " AND ") +
                        " c.MilitaryDepartmentID IN (-1) ";
                }

                where += (where == "" ? "" : " AND ") +
                         @" c.MilitaryDepartmentID IN ( " + currentUser.MilitaryDepartmentIDs + ") ";
                
                
                if (!String.IsNullOrEmpty(filter.AdministrationId))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.AdministrationID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.AdministrationId) + ") ";
                }
                                
                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @"SELECT DISTINCT 
                                      a.CompanyID, a.OwnershipTypeID, a.UnifiedIdentityCode,
                                      a.CompanyName, a.CityID, a.DistrictID, a.Address, a.PostCode, a.Phone, a.AdministrationID
                              FROM PMIS_ADM.Companies a
                              INNER JOIN PMIS_RES.PostponeResCompanies b ON a.CompanyID = b.CompanyID
                              INNER JOIN PMIS_RES.PostponeRes c ON b.PostponeResID = c.PostponeResID
                              " + where + @"
                              ORDER BY a.CompanyName";


                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Company company = CompanyUtil.ExtractCompanyFromDR(dr, currentUser);
                    companies.Add(company);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return companies;
        }
    }
}
