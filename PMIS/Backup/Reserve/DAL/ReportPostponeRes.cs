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
    public class ReportPostponeResFilter
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

    public class ReportPostponeResBlock
    {
        public string NKPDNickname { get; set; }
        public int TotalEmployeesCnt { get; set; }
        public int OfficersConditionedPostpone { get; set; }
        public int OfficersConditionedFulfil { get; set; }
        public int OfficersAbsolutelyPostpone { get; set; }
        public int OfficersAbsolutelyFulfil { get; set; }
        public int OfCandConditionedPostpone { get; set; }
        public int OfCandConditionedFulfil { get; set; }
        public int OfCandAbsolutelyPostpone { get; set; }
        public int OfCandAbsolutelyFulfil { get; set; }
        public int SergeantsConditionedPostpone { get; set; }
        public int SergeantsConditionedFulfil { get; set; }
        public int SergeantsAbsolutelyPostpone { get; set; }
        public int SergeantsAbsolutelyFulfil { get; set; }
        public int SoldiersConditionedPostpone { get; set; }
        public int SoldiersConditionedFulfil { get; set; }
        public int SoldiersAbsolutelyPostpone { get; set; }
        public int SoldiersAbsolutelyFulfil { get; set; }
    }

    public class ReportPostponeResResult
    {
        public List<ReportPostponeResBlock> AllBlocks { get; set; }
        public ReportPostponeResFilter Filter { get; set; }
    }

    public static class ReportPostponeResUtil
    {
        public static ReportPostponeResResult GetReportPostponeRes(ReportPostponeResFilter filter, User currentUser)
        {
            ReportPostponeResResult reportResult = new ReportPostponeResResult();
            List<ReportPostponeResBlock> reportBlocks = new List<ReportPostponeResBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                //In general, the filter is by CompanyID. Companies are a key object when entering postpones.
                //On the report we force the user to select one or more companies from the list and then we load the report
                //for those companies. The other filter on the report are used to filter the list of companies.
                //However, we need to filter the report resutls by Military Department and Postpone Year because
                //there could be the same company added for multiple military departments (maybe this is not a common case) and
                //for multiple years (this should be the common scenario).

                string wherePostpone = "";
                string whereFulfil = "";

                if (!String.IsNullOrEmpty(filter.MilitaryDepartmentIds))
                {
                    wherePostpone += (wherePostpone == "" ? "" : " AND ") +
                                    " c.MilitaryDepartmentID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryDepartmentIds) + ") ";

                    whereFulfil += (whereFulfil == "" ? "" : " AND ") +
                                    " c.SourceMilDepartmentID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryDepartmentIds) + ") ";
                }

                wherePostpone += (wherePostpone == "" ? "" : " AND ") +
                         @" (c.MilitaryDepartmentID IS NULL OR c.MilitaryDepartmentID IN ( " + currentUser.MilitaryDepartmentIDs + ")) ";

                whereFulfil += (whereFulfil == "" ? "" : " AND ") +
                         @" (c.SourceMilDepartmentID IS NULL OR c.SourceMilDepartmentID IN ( " + currentUser.MilitaryDepartmentIDs + ")) ";
                                               
                
                if (!string.IsNullOrEmpty(filter.PostponeYear))
                {
                    wherePostpone += (wherePostpone == "" ? "" : " AND ") +
                             @" c.PostponeYear IN ( " + CommonFunctions.AvoidSQLInjForListOfIDs(filter.PostponeYear) + ") ";

                    whereFulfil += (whereFulfil == "" ? "" : " AND ") +
                             @" c.Postpone_Year IN ( " + CommonFunctions.AvoidSQLInjForListOfIDs(filter.PostponeYear) + ") ";
                }

                if (!String.IsNullOrEmpty(filter.CompanyIds))
                {
                    wherePostpone += (wherePostpone == "" ? "" : " AND ") +
                                    " b.CompanyID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.CompanyIds) + ") ";

                    whereFulfil += (whereFulfil == "" ? "" : " AND ") +
                                    " b.WorkCompanyID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.CompanyIds) + ") ";
                }

                wherePostpone = (wherePostpone == "" ? "" : " WHERE ") + wherePostpone;
                whereFulfil = (whereFulfil == "" ? "" : " AND ") + whereFulfil;


                string SQL = @"
SELECT a.NKPDNickname,
       NVL(pc.TotalEmployeesCnt, 0) as TotalEmployeesCnt,
       NVL(p.OfficersConditioned, 0) as OfficersConditionedPostpone,
       NVL(f.OfficersConditioned, 0) as OfficersConditionedFulfil,
       NVL(p.OfficersAbsolutely, 0) as OfficersAbsolutelyPostpone,
       NVL(f.OfficersAbsolutely, 0) as OfficersAbsolutelyFulfil,
       NVL(p.OfCandConditioned, 0) as OfCandConditionedPostpone,
       NVL(f.OfCandConditioned, 0) as OfCandConditionedFulfil,
       NVL(p.OfCandAbsolutely, 0) as OfCandAbsolutelyPostpone,
       NVL(f.OfCandAbsolutely, 0) as OfCandAbsolutelyFulfil,
       NVL(p.SergeantsConditioned, 0) as SergeantsConditionedPostpone,
       NVL(f.SergeantsConditioned, 0) as SergeantsConditionedFulfil,
       NVL(p.SergeantsAbsolutely, 0) as SergeantsAbsolutelyPostpone,
       NVL(f.SergeantsAbsolutely, 0) as SergeantsAbsolutelyFulfil,
       NVL(p.SoldiersConditioned, 0) as SoldiersConditionedPostpone,
       NVL(f.SoldiersConditioned, 0) as SoldiersConditionedFulfil,
       NVL(p.SoldiersAbsolutely, 0) as SoldiersAbsolutelyPostpone,
       NVL(f.SoldiersAbsolutely, 0) as SoldiersAbsolutelyFulfil
FROM PMIS_ADM.NKPD a
LEFT OUTER JOIN (
    SELECT a.NKPDID,
           SUM(a.OfficersConditioned) as OfficersConditioned,
           SUM(a.OfficersAbsolutely) as OfficersAbsolutely,
           SUM(a.OfCandConditioned) as OfCandConditioned,
           SUM(a.OfCandAbsolutely) as OfCandAbsolutely,
           SUM(a.SergeantsConditioned) as SergeantsConditioned,
           SUM(a.SergeantsAbsolutely) as SergeantsAbsolutely,
           SUM(a.SoldiersConditioned) as SoldiersConditioned,
           SUM(a.SoldiersAbsolutely) as SoldiersAbsolutely
    FROM PMIS_RES.PostponeResItems a
    INNER JOIN PMIS_RES.PostponeResCompanies b ON a.PostponeResCompanyID = b.PostponeResCompanyID
    INNER JOIN PMIS_RES.PostponeRes c On b.PostponeResID = c.PostponeResID
    " + wherePostpone + @"
    GROUP BY a.NKPDID
) p ON a.NKPDID = p.NKPDID
LEFT OUTER JOIN (
    SELECT SUM(b.EmployeesCnt) as TotalEmployeesCnt
    FROM PMIS_RES.PostponeResCompanies b
    INNER JOIN PMIS_RES.PostponeRes c On b.PostponeResID = c.PostponeResID
    " + wherePostpone + @"
) pc ON 1 = 1
LEFT OUTER JOIN (
    SELECT PMIS_ADM.CommonFunctions.GetNKPDRootID(b.WorkPositionNKPDID) as NKPDID,
           SUM(CASE WHEN f.MilitaryRankCategory = 2 AND g.PostponeTypeKey = 'CONDITIONED' THEN 1 ELSE 0 END) as OfficersConditioned,
           SUM(CASE WHEN f.MilitaryRankCategory = 2 AND g.PostponeTypeKey = 'ABSOLUTELY' THEN 1 ELSE 0 END) as OfficersAbsolutely,
           SUM(CASE WHEN f.MilitaryRankCategory = 1 AND f.MilitaryRankSubCategory = 3 AND g.PostponeTypeKey = 'CONDITIONED' THEN 1 ELSE 0 END) as OfCandConditioned,
           SUM(CASE WHEN f.MilitaryRankCategory = 1 AND f.MilitaryRankSubCategory = 3 AND g.PostponeTypeKey = 'ABSOLUTELY' THEN 1 ELSE 0 END) as OfCandAbsolutely,
           SUM(CASE WHEN f.MilitaryRankCategory = 1 AND f.MilitaryRankSubCategory = 1 AND g.PostponeTypeKey = 'CONDITIONED' THEN 1 ELSE 0 END) as SergeantsConditioned,
           SUM(CASE WHEN f.MilitaryRankCategory = 1 AND f.MilitaryRankSubCategory = 1 AND g.PostponeTypeKey = 'ABSOLUTELY' THEN 1 ELSE 0 END) as SergeantsAbsolutely,
           SUM(CASE WHEN f.MilitaryRankCategory = 1 AND f.MilitaryRankSubCategory = 2 AND g.PostponeTypeKey = 'CONDITIONED' THEN 1 ELSE 0 END) as SoldiersConditioned,
           SUM(CASE WHEN f.MilitaryRankCategory = 1 AND f.MilitaryRankSubCategory = 2 AND g.PostponeTypeKey = 'ABSOLUTELY' THEN 1 ELSE 0 END) as SoldiersAbsolutely
    FROM PMIS_RES.Reservists a
    INNER JOIN PMIS_ADM.Persons b ON a.PersonID = b.PersonID
    INNER JOIN PMIS_RES.ReservistMilRepStatuses c ON a.ReservistID = c.ReservistID AND c.IsCurrent = 1
    INNER JOIN PMIS_RES.MilitaryReportStatuses c1 ON c.MilitaryReportStatusID = c1.MilitaryReportStatusID
    INNER JOIN VS_OWNER.VS_LS d ON a.PersonID = d.PersonID
	INNER JOIN VS_OWNER.KLV_ZVA e ON d.KOD_ZVA = e.ZVA_KOD
	INNER JOIN PMIS_ADM.MilitaryRankCategories f ON e.ZVA_KAT_KOD = f.ZVA_KAT_KOD
    INNER JOIN PMIS_RES.PostponeTypes g ON c.Postpone_TypeID = g.PostponeTypeID
    WHERE b.WorkPositionNKPDID IS NOT NULL AND
          c1.MilitaryReportStatusKey = 'POSTPONED'
    " + whereFulfil + @"
    GROUP BY PMIS_ADM.CommonFunctions.GetNKPDRootID(b.WorkPositionNKPDID)
) f ON a.NKPDID = f.NKPDID
WHERE a.NKPDParentID IS NULL AND NVL(a.IsActive, 0) = 1
ORDER BY a.NKPDCode
";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ReportPostponeResBlock reportBlock = new ReportPostponeResBlock();

                    reportBlock.NKPDNickname = dr["NKPDNickname"].ToString();

                    reportBlock.TotalEmployeesCnt = DBCommon.GetInt(dr["TotalEmployeesCnt"]);
                    reportBlock.OfficersConditionedPostpone = DBCommon.GetInt(dr["OfficersConditionedPostpone"]);
                    reportBlock.OfficersConditionedFulfil = DBCommon.GetInt(dr["OfficersConditionedFulfil"]);
                    reportBlock.OfficersAbsolutelyPostpone = DBCommon.GetInt(dr["OfficersAbsolutelyPostpone"]);
                    reportBlock.OfficersAbsolutelyFulfil = DBCommon.GetInt(dr["OfficersAbsolutelyFulfil"]);
                    reportBlock.OfCandConditionedPostpone = DBCommon.GetInt(dr["OfCandConditionedPostpone"]);
                    reportBlock.OfCandConditionedFulfil = DBCommon.GetInt(dr["OfCandConditionedFulfil"]);
                    reportBlock.OfCandAbsolutelyPostpone = DBCommon.GetInt(dr["OfCandAbsolutelyPostpone"]);
                    reportBlock.OfCandAbsolutelyFulfil = DBCommon.GetInt(dr["OfCandAbsolutelyFulfil"]);
                    reportBlock.SergeantsConditionedPostpone = DBCommon.GetInt(dr["SergeantsConditionedPostpone"]);
                    reportBlock.SergeantsConditionedFulfil = DBCommon.GetInt(dr["SergeantsConditionedFulfil"]);
                    reportBlock.SergeantsAbsolutelyPostpone = DBCommon.GetInt(dr["SergeantsAbsolutelyPostpone"]);
                    reportBlock.SergeantsAbsolutelyFulfil = DBCommon.GetInt(dr["SergeantsAbsolutelyFulfil"]);
                    reportBlock.SoldiersConditionedPostpone = DBCommon.GetInt(dr["SoldiersConditionedPostpone"]);
                    reportBlock.SoldiersConditionedFulfil = DBCommon.GetInt(dr["SoldiersConditionedFulfil"]);
                    reportBlock.SoldiersAbsolutelyPostpone = DBCommon.GetInt(dr["SoldiersAbsolutelyPostpone"]);
                    reportBlock.SoldiersAbsolutelyFulfil = DBCommon.GetInt(dr["SoldiersAbsolutelyFulfil"]);

                    reportBlocks.Add(reportBlock);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            reportResult.AllBlocks = reportBlocks;
            reportResult.Filter = filter;

            return reportResult;
        }

        //Get a list of all Companies for the selected filter
        public static List<Company> GetCompaniesList(User currentUser, ReportPostponeResFilter filter)
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


                if (!String.IsNullOrEmpty(filter.PostponeYear))
                {
                    where += (where == "" ? "" : " AND ") +
                        " c.PostponeYear IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.PostponeYear) + ") ";
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