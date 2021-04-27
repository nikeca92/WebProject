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
    public class PrintPostponeTechnicsFilter
    {
        public string MilitaryDepartmentIds { get; set; }
        public string AdministrationId { get; set; }
        public string CompanyIds { get; set; }

        public int OrderBy { get; set; }
        public int PageIdx { get; set; }
    }

    public class PrintPostponeTechnicsResultBlock
    {
        public int TechnicsId { get; set; }
        public string TechnicsTypeName { get; set; }
        public string NormativeTechnics { get; set; }
        public string RegNumber { get; set; }
    }

    public class PrintPostponeTechnicsResult
    {
        private int rowsPerPage;
        private User currentUser;
        private PrintPostponeTechnicsFilter filter;

        public PrintPostponeTechnicsResult(int rowsPerPage, User currentUser)
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
                    allRecordsCount = PrintPostponeTechnicsUtil.GetPrintPostponeTechnicsResultsCount(filter, currentUser);
                }

                return allRecordsCount.Value;
            }
        }

        private List<PrintPostponeTechnicsResultBlock> overallResult = null;
        public List<PrintPostponeTechnicsResultBlock> OverallResult
        {
            get
            {
                if (overallResult == null)
                {
                    overallResult = PrintPostponeTechnicsUtil.GetPrintPostponeTechnicsResults(filter, rowsPerPage, currentUser);
                }

                return overallResult;
            }
        }

        public PrintPostponeTechnicsFilter Filter
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

    public static class PrintPostponeTechnicsUtil
    {
        public static List<PrintPostponeTechnicsResultBlock> GetPrintPostponeTechnicsResults(PrintPostponeTechnicsFilter filter, int rowsPerPage, User currentUser)
        {
            List<PrintPostponeTechnicsResultBlock> result = new List<PrintPostponeTechnicsResultBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string whereClause = "WHERE e.TechMilitaryReportStatusKey = 'POSTPONED'";

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
                        orderBySQL = "b.TechnicsTypeName";
                        break;
                    case 2:
                        orderBySQL = "c.NormativeCode";
                        break;
                    case 3:
                        orderBySQL = @"CASE b.TechnicsTypeKey
                                            WHEN 'VEHICLES' THEN f1.RegNumber
                                            WHEN 'TRAILERS' THEN f2.RegNumber
                                            WHEN 'TRACTORS' THEN f3.RegNumber
                                            WHEN 'ENG_EQUIP' THEN f4.RegNumber
                                            WHEN 'MOB_LIFT_EQUIP' THEN f5.RegNumber
                                            WHEN 'RAILWAY_EQUIP' THEN f6.InventoryNumber
                                            WHEN 'AVIATION_EQUIP' THEN f7.AirInvNumber
                                            WHEN 'VESSELS' THEN f8.InventoryNumber
                                            WHEN 'FUEL_CONTAINERS' THEN f9.InventoryNumber
                                            ELSE ''
                                       END";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"
                               SELECT tmp.*
                               FROM 
                                  (
                                    SELECT a.TechnicsId,
                                           b.TechnicsTypeName,
                                           c.NormativeCode || ' ' || c.NormativeName as NormativeTechnics,
                                           CASE b.TechnicsTypeKey
                                                WHEN 'VEHICLES' THEN f1.RegNumber
                                                WHEN 'TRAILERS' THEN f2.RegNumber
                                                WHEN 'TRACTORS' THEN f3.RegNumber
                                                WHEN 'ENG_EQUIP' THEN f4.RegNumber
                                                WHEN 'MOB_LIFT_EQUIP' THEN f5.RegNumber
                                                WHEN 'RAILWAY_EQUIP' THEN f6.InventoryNumber
                                                WHEN 'AVIATION_EQUIP' THEN f7.AirInvNumber
                                                WHEN 'VESSELS' THEN f8.InventoryNumber
                                                WHEN 'FUEL_CONTAINERS' THEN f9.InventoryNumber
                                                ELSE ''
                                           END as RegNumber,
                                           RANK() OVER (ORDER BY " + orderBySQL + @", a.TechnicsId) as RowNumber                                           
                                    FROM PMIS_RES.Technics a 
                                    INNER JOIN PMIS_RES.TechnicsTypes b ON a.TechnicsTypeID = b.TechnicsTypeID
                                    LEFT OUTER JOIN PMIS_RES.NormativeTechnics c ON a.NormativeTechnicsID = c.NormativeTechnicsID
                                    LEFT OUTER JOIN PMIS_RES.Vehicles f1 ON a.TechnicsID = f1.TechnicsID
                                    LEFT OUTER JOIN PMIS_RES.Trailers f2 ON a.TechnicsID = f2.TechnicsID
                                    LEFT OUTER JOIN PMIS_RES.Tractors f3 ON a.TechnicsID = f3.TechnicsID
                                    LEFT OUTER JOIN PMIS_RES.EngEquipment f4 ON a.TechnicsID = f4.TechnicsID
                                    LEFT OUTER JOIN PMIS_RES.MobileLiftingEquip f5 ON a.TechnicsID = f5.TechnicsID
                                    LEFT OUTER JOIN PMIS_RES.RailWayEquips f6 ON a.TechnicsID = f6.TechnicsID
                                    LEFT OUTER JOIN PMIS_RES.AviationEquipment f7 ON a.TechnicsID = f7.TechnicsID
                                    LEFT OUTER JOIN PMIS_RES.Vessels f8 ON a.TechnicsID = f8.TechnicsID
                                    LEFT OUTER JOIN PMIS_RES.FuelContainers f9 ON a.TechnicsID = f9.TechnicsID
                                    LEFT OUTER JOIN PMIS_RES.TechnicsMilRepStatus d ON a.TechnicsID = d.TechnicsID and d.IsCurrent = 1
                                    LEFT OUTER JOIN PMIS_RES.TechMilitaryReportStatuses e ON d.TechMilitaryReportStatusId = e.TechMilitaryReportStatusId
                                    LEFT OUTER JOIN PMIS_ADM.Companies g ON g.CompanyID = a.OwnershipCompanyId
                                    LEFT OUTER JOIN PMIS_ADM.Administrations h ON g.AdministrationId = h.AdministrationId
                                    LEFT OUTER JOIN PMIS_ADM.MilitaryDepartments k ON k.MilitaryDepartmentID = d.SourceMilDepartmentID
                                    " + whereClause + @"
                                  ) tmp
                                " + pageWhere + @"
                                ORDER BY RowNumber
                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    PrintPostponeTechnicsResultBlock block = new PrintPostponeTechnicsResultBlock();
                    block.TechnicsId = DBCommon.GetInt(dr["TechnicsId"]);
                    block.TechnicsTypeName = dr["TechnicsTypeName"].ToString();
                    block.NormativeTechnics = dr["NormativeTechnics"].ToString();
                    block.RegNumber = dr["RegNumber"].ToString();

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

        public static int GetPrintPostponeTechnicsResultsCount(PrintPostponeTechnicsFilter filter, User currentUser)
        {
            int cnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string whereClause = "WHERE e.TechMilitaryReportStatusKey = 'POSTPONED'";

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
                                   FROM PMIS_RES.Technics a 
                                    INNER JOIN PMIS_RES.TechnicsTypes b ON a.TechnicsTypeID = b.TechnicsTypeID
                                    LEFT OUTER JOIN PMIS_RES.NormativeTechnics c ON a.NormativeTechnicsID = c.NormativeTechnicsID
                                    LEFT OUTER JOIN PMIS_RES.Vehicles f1 ON a.TechnicsID = f1.TechnicsID
                                    LEFT OUTER JOIN PMIS_RES.Trailers f2 ON a.TechnicsID = f2.TechnicsID
                                    LEFT OUTER JOIN PMIS_RES.Tractors f3 ON a.TechnicsID = f3.TechnicsID
                                    LEFT OUTER JOIN PMIS_RES.EngEquipment f4 ON a.TechnicsID = f4.TechnicsID
                                    LEFT OUTER JOIN PMIS_RES.MobileLiftingEquip f5 ON a.TechnicsID = f5.TechnicsID
                                    LEFT OUTER JOIN PMIS_RES.RailWayEquips f6 ON a.TechnicsID = f6.TechnicsID
                                    LEFT OUTER JOIN PMIS_RES.AviationEquipment f7 ON a.TechnicsID = f7.TechnicsID
                                    LEFT OUTER JOIN PMIS_RES.Vessels f8 ON a.TechnicsID = f8.TechnicsID
                                    LEFT OUTER JOIN PMIS_RES.FuelContainers f9 ON a.TechnicsID = f9.TechnicsID
                                    LEFT OUTER JOIN PMIS_RES.TechnicsMilRepStatus d ON a.TechnicsID = d.TechnicsID and d.IsCurrent = 1
                                    LEFT OUTER JOIN PMIS_RES.TechMilitaryReportStatuses e ON d.TechMilitaryReportStatusId = e.TechMilitaryReportStatusId
                                    LEFT OUTER JOIN PMIS_ADM.Companies g ON g.CompanyID = a.OwnershipCompanyId
                                    LEFT OUTER JOIN PMIS_ADM.Administrations h ON g.AdministrationId = h.AdministrationId
                                    LEFT OUTER JOIN PMIS_ADM.MilitaryDepartments k ON k.MilitaryDepartmentID = d.SourceMilDepartmentID
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
        public static List<Company> GetCompaniesList(User currentUser, PrintPostponeTechnicsFilter filter)
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
                              INNER JOIN PMIS_RES.PostponeTechCompanies b ON a.CompanyID = b.CompanyID
                              INNER JOIN PMIS_RES.PostponeTech c ON b.PostponeTechID = c.PostponeTechID
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
