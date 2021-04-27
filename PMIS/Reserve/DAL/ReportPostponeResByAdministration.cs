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
    public class ReportPostponeResByAdministrationFilter
    {
        public string MilitaryDepartmentIds { get; set; }
        public string PostponeYear { get; set; }
        public string Region { get; set; }
        public string Municipality { get; set; }
        public string City { get; set; }

        public string MilitaryDepartmentDisplayText { get; set; }
        public string PostponeYearDisplayText { get; set; }
        public string RegionDisplayText { get; set; }
        public string MunicipalityDisplayText { get; set; }
        public string CityDisplayText { get; set; }
    }

    public class ReportPostponeResByAdministrationBlock
    {
        public string AdministrationName { get; set; }
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

    public class ReportPostponeResByAdministrationResult
    {
        public List<ReportPostponeResByAdministrationBlock> AllBlocks { get; set; }
        public ReportPostponeResByAdministrationFilter Filter { get; set; }
    }

    public static class ReportPostponeResByAdministrationUtil
    {
        public static ReportPostponeResByAdministrationResult GetReportPostponeResByAdministration(ReportPostponeResByAdministrationFilter filter, User currentUser)
        {
            ReportPostponeResByAdministrationResult reportResult = new ReportPostponeResByAdministrationResult();
            List<ReportPostponeResByAdministrationBlock> reportBlocks = new List<ReportPostponeResByAdministrationBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
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

                if (!string.IsNullOrEmpty(filter.Region))
                {
                    wherePostpone += (wherePostpone == "" ? "" : " AND ") +
                             @" d.CityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBL IN ( " + filter.Region + ") ) ";

                    whereFulfil += (whereFulfil == "" ? "" : " AND ") +
                             @" h.CityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBL IN ( " + filter.Region + ") ) ";
                }

                if (!string.IsNullOrEmpty(filter.Municipality))
                {
                    wherePostpone += (wherePostpone == "" ? "" : " AND ") +
                             @" d.CityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBS IN ( " + filter.Municipality + ") ) ";

                    whereFulfil += (whereFulfil == "" ? "" : " AND ") +
                             @" h.CityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBS IN ( " + filter.Municipality + ") ) ";
                }

                if (!string.IsNullOrEmpty(filter.City))
                {
                    wherePostpone += (wherePostpone == "" ? "" : " AND ") +
                             @" d.CityID IN ( " + filter.City + ") ";

                    whereFulfil += (whereFulfil == "" ? "" : " AND ") +
                             @" h.CityID IN ( " + filter.City + ") ";
                }

                wherePostpone = (wherePostpone == "" ? "" : " WHERE ") + wherePostpone;
                whereFulfil = (whereFulfil == "" ? "" : " AND ") + whereFulfil;


                string SQL = @"
SELECT a.AdministrationName,
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
FROM PMIS_ADM.Administrations a
LEFT OUTER JOIN (
    SELECT d.AdministrationID,
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
    INNER JOIN PMIS_ADM.Companies d ON b.CompanyID = d.CompanyID
    INNER JOIN PMIS_RES.PostponeRes c On b.PostponeResID = c.PostponeResID
    " + wherePostpone + @"
    GROUP BY d.AdministrationID
) p ON a.AdministrationID = p.AdministrationID
LEFT OUTER JOIN (
    SELECT h.AdministrationID,
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
    INNER JOIN PMIS_ADM.Companies h ON b.WorkCompanyID = h.CompanyID
    WHERE b.WorkPositionNKPDID IS NOT NULL AND
          c1.MilitaryReportStatusKey = 'POSTPONED'
    " + whereFulfil + @"
    GROUP BY h.AdministrationID
) f ON a.AdministrationID = f.AdministrationID
WHERE p.AdministrationID IS NOT NULL OR f.AdministrationID IS NOT NULL /*Show only administration for which there is some data (either postpone or fulfil)*/
ORDER BY a.AdministrationGroupSeq, a.AdministrationName
";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ReportPostponeResByAdministrationBlock reportBlock = new ReportPostponeResByAdministrationBlock();

                    reportBlock.AdministrationName = dr["AdministrationName"].ToString();

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
    }
}