using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using System.Linq;

using PMIS.Common;

namespace PMIS.Reserve.Common
{
    //This class represents all information about the filter
    public class ReportPostponeFilter
    {
        public string MilitaryDepartmentIds { get; set; }
        public string PostponeYear { get; set; }
        public string AdministrationIds { get; set; }
        public string Region { get; set; }
        public string Municipality { get; set; }
        public string City { get; set; }
        public string CompanyIds { get; set; }

        public string MilitaryDepartmentDisplayText { get; set; }
        public string PostponeYearDisplayText { get; set; }
        public string AdministrationDisplayText { get; set; }
        public string RegionDisplayText { get; set; }
        public string MunicipalityDisplayText { get; set; }
        public string CityDisplayText { get; set; }
        public string CompanyDisplayText { get; set; }
    }

    public class ReportPostponeResult
    {
        public List<ReportPostponeResBlock> ResBlock { get; set; }
        public List<ReportPostponeTechBlock> TechBlock { get; set; }
        public ReportPostponeFilter Filter { get; set; }
    }

    public static class ReportPostponeUtil
    {
        public static ReportPostponeResFilter TransformToResFilter(ReportPostponeFilter filter)
        {
            ReportPostponeResFilter resFilter = new ReportPostponeResFilter();

            resFilter.MilitaryDepartmentIds = filter.MilitaryDepartmentIds;
            resFilter.PostponeYear = filter.PostponeYear;
            resFilter.AdministrationIds = filter.AdministrationIds;
            resFilter.Region = filter.Region;
            resFilter.Municipality = filter.Municipality;
            resFilter.City = filter.City;
            resFilter.CompanyIds = filter.CompanyIds;
            resFilter.MilitaryDepartmentDisplayText = filter.MilitaryDepartmentDisplayText;
            resFilter.PostponeYearDisplayText = filter.PostponeYearDisplayText;
            resFilter.AdministrationDisplayText = filter.AdministrationDisplayText;
            resFilter.RegionDisplayText = filter.RegionDisplayText;
            resFilter.MunicipalityDisplayText = filter.MunicipalityDisplayText;
            resFilter.CityDisplayText = filter.CityDisplayText;
            resFilter.CompanyDisplayText = filter.CompanyDisplayText;

            return resFilter;
        }

        public static ReportPostponeTechFilter TransformToTechFilter(ReportPostponeFilter filter)
        {
            ReportPostponeTechFilter techFilter = new ReportPostponeTechFilter();

            techFilter.MilitaryDepartmentIds = filter.MilitaryDepartmentIds;
            techFilter.PostponeYear = filter.PostponeYear;
            techFilter.AdministrationIds = filter.AdministrationIds;
            techFilter.Region = filter.Region;
            techFilter.Municipality = filter.Municipality;
            techFilter.City = filter.City;
            techFilter.CompanyIds = filter.CompanyIds;
            techFilter.MilitaryDepartmentDisplayText = filter.MilitaryDepartmentDisplayText;
            techFilter.PostponeYearDisplayText = filter.PostponeYearDisplayText;
            techFilter.AdministrationDisplayText = filter.AdministrationDisplayText;
            techFilter.RegionDisplayText = filter.RegionDisplayText;
            techFilter.MunicipalityDisplayText = filter.MunicipalityDisplayText;
            techFilter.CityDisplayText = filter.CityDisplayText;
            techFilter.CompanyDisplayText = filter.CompanyDisplayText;

            return techFilter;
        }

        public static ReportPostponeResult GetReportPostpone(ReportPostponeFilter filter, User currentUser)
        {
            ReportPostponeResult reportResult = new ReportPostponeResult();

            ReportPostponeResResult resResult = ReportPostponeResUtil.GetReportPostponeRes(ReportPostponeUtil.TransformToResFilter(filter), currentUser);
            ReportPostponeTechResult techResult = ReportPostponeTechUtil.GetReportPostponeTech(ReportPostponeUtil.TransformToTechFilter(filter), currentUser);

            reportResult.ResBlock = resResult.AllBlocks;
            reportResult.TechBlock = techResult.AllBlocks;
            reportResult.Filter = filter;

            return reportResult;
        }

        //Get a list of all Companies for the selected filter
        public static List<Company> GetCompaniesList(User currentUser, ReportPostponeFilter filter)
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
                        " MilitaryDepartmentID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryDepartmentIds) + ") ";
                }
                else
                {
                    //If there is no military department selected then do not show any companies to prevent loading a huge list of companies
                    where += (where == "" ? "" : " AND ") +
                        " MilitaryDepartmentID IN (-1) ";
                }

                where += (where == "" ? "" : " AND ") +
                         @" MilitaryDepartmentID IN ( " + currentUser.MilitaryDepartmentIDs + ") ";


                if (!String.IsNullOrEmpty(filter.PostponeYear))
                {
                    where += (where == "" ? "" : " AND ") +
                        " PostponeYear IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.PostponeYear) + ") ";
                }

                if (!String.IsNullOrEmpty(filter.AdministrationIds))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.AdministrationID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.AdministrationIds) + ") ";
                }

                if (!string.IsNullOrEmpty(filter.Region))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" a.CityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBL IN ( " + filter.Region + ") ) ";
                }

                if (!string.IsNullOrEmpty(filter.Municipality))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" a.CityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBS IN ( " + filter.Municipality + ") ) ";
                }

                if (!string.IsNullOrEmpty(filter.City))
                {
                    where += (where == "" ? "" : " AND ") +
                             @" a.CityID IN ( " + filter.City + ") ";
                }

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @"SELECT DISTINCT 
                                      a.CompanyID, a.OwnershipTypeID, a.UnifiedIdentityCode,
                                      a.CompanyName, a.CityID, a.DistrictID, a.Address, a.PostCode, a.Phone, a.AdministrationID
                              FROM PMIS_ADM.Companies a
                              INNER JOIN PMIS_RES.PostponeResCompanies b ON a.CompanyID = b.CompanyID
                              INNER JOIN PMIS_RES.PostponeRes c ON b.PostponeResID = c.PostponeResID
                              " + where + @"
                              UNION
                              SELECT DISTINCT 
                                      a.CompanyID, a.OwnershipTypeID, a.UnifiedIdentityCode,
                                      a.CompanyName, a.CityID, a.DistrictID, a.Address, a.PostCode, a.Phone, a.AdministrationID
                              FROM PMIS_ADM.Companies a
                              INNER JOIN PMIS_RES.PostponeTechCompanies d ON a.CompanyID = d.CompanyID
                              INNER JOIN PMIS_RES.PostponeTech e ON d.PostponeTechID = e.PostponeTechID
                              " + where + @"
                              ORDER BY CompanyName";


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