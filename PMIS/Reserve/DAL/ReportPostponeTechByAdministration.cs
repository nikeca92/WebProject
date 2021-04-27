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
    public class ReportPostponeTechByAdministrationFilter
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

    public class ReportPostponeTechByAdministrationBlock
    {
        public int AdministrationId { get; set; }
        public string AdministrationName { get; set; }
        public int TechnicsTypeId { get; set; }
        public string TechnicsTypeName { get; set; }
        
        public int PostponeAbsolutely { get; set; }
        public int FulfilAbsolutely { get; set; }
        public int PostponeConditioned { get; set; }
        public int FulfilConditioned { get; set; }

        public int AnyDataForThisAdministration { get; set; }
    }

    public class ReportPostponeTechByAdministrationResult
    {
        public List<ReportPostponeTechByAdministrationBlock> AllBlocks { get; set; }
        public ReportPostponeTechByAdministrationFilter Filter { get; set; }
    }

    public static class ReportPostponeTechByAdministrationUtil
    {
        public static ReportPostponeTechByAdministrationResult GetReportPostponeTechByAdministration(ReportPostponeTechByAdministrationFilter filter, User currentUser)
        {
            ReportPostponeTechByAdministrationResult reportResult = new ReportPostponeTechByAdministrationResult();
            List<ReportPostponeTechByAdministrationBlock> reportBlocks = new List<ReportPostponeTechByAdministrationBlock>();

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
                             @" c.TechnicsPostpone_Year IN ( " + CommonFunctions.AvoidSQLInjForListOfIDs(filter.PostponeYear) + ") ";
                }

                if (!string.IsNullOrEmpty(filter.Region))
                {
                    wherePostpone += (wherePostpone == "" ? "" : " AND ") +
                             @" d.CityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBL IN ( " + filter.Region + ") ) ";

                    whereFulfil += (whereFulfil == "" ? "" : " AND ") +
                             @" d.CityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBL IN ( " + filter.Region + ") ) ";
                }

                if (!string.IsNullOrEmpty(filter.Municipality))
                {
                    wherePostpone += (wherePostpone == "" ? "" : " AND ") +
                             @" d.CityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBS IN ( " + filter.Municipality + ") ) ";

                    whereFulfil += (whereFulfil == "" ? "" : " AND ") +
                             @" d.CityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBS IN ( " + filter.Municipality + ") ) ";
                }

                if (!string.IsNullOrEmpty(filter.City))
                {
                    wherePostpone += (wherePostpone == "" ? "" : " AND ") +
                             @" d.CityID IN ( " + filter.City + ") ";

                    whereFulfil += (whereFulfil == "" ? "" : " AND ") +
                             @" d.CityID IN ( " + filter.City + ") ";
                }

                wherePostpone = (wherePostpone == "" ? "" : " WHERE ") + wherePostpone;
                whereFulfil = (whereFulfil == "" ? "" : " AND ") + whereFulfil;

                string SQL = @"
SELECT a.AdministrationID,
       a.AdministrationName,
       b.TechnicsTypeID,
       b.TechnicsTypeName,
       NVL(p.PostponeAbsolutely, 0) as PostponeAbsolutely,
       NVL(f.FulfilAbsolutely, 0) as FulfilAbsolutely,
       NVL(f.FulfilConditioned, 0) as FulfilConditioned,
       NVL(p.PostponeConditioned, 0) as PostponeConditioned,
       CASE WHEN p.AdministrationID IS NOT NULL OR f.AdministrationID IS NOT NULL THEN 1 ELSE 0 END as AnyDataForThisAdministration
FROM PMIS_ADM.Administrations a
CROSS JOIN PMIS_RES.TechnicsTypes b
LEFT OUTER JOIN (
    SELECT d.AdministrationID,
           e.TechnicsTypeID,
           SUM(a.PostponeAbsolutely) as PostponeAbsolutely,
           SUM(a.PostponeConditioned) as PostponeConditioned
    FROM PMIS_RES.PostponeTechItems a
    INNER JOIN PMIS_RES.PostponeTechCompanies b ON a.PostponeTechCompanyID = b.PostponeTechCompanyID
    INNER JOIN PMIS_RES.PostponeTech c On b.PostponeTechID = c.PostponeTechID
    INNER JOIN PMIS_ADM.Companies d ON b.CompanyID = d.CompanyID
    INNER JOIN PMIS_RES.TechnicsSubTypes e ON a.TechnicsSubTypeID = e.TechnicsSubTypeID
    " + wherePostpone + @"
    GROUP BY d.AdministrationID, e.TechnicsTypeID
) p ON a.AdministrationID = p.AdministrationID AND b.TechnicsTypeID = p.TechnicsTypeID
LEFT OUTER JOIN (
    SELECT d.AdministrationID,
           a.TechnicsTypeID,
           SUM(CASE WHEN g.TechnicsPostponeTypeKey = 'CONDITIONED' THEN 1 ELSE 0 END) as FulfilConditioned,
           SUM(CASE WHEN g.TechnicsPostponeTypeKey = 'ABSOLUTELY' THEN 1 ELSE 0 END) as FulfilAbsolutely
    FROM PMIS_RES.Technics a
    INNER JOIN PMIS_RES.TechnicsMilRepStatus c ON a.TechnicsID = c.TechnicsID AND c.IsCurrent = 1
    INNER JOIN PMIS_RES.TechMilitaryReportStatuses c1 ON c.TechMilitaryReportStatusID = c1.TechMilitaryReportStatusID
    INNER JOIN PMIS_RES.TechnicsPostponeTypes g ON c.TechnicsPostpone_TypeID = g.TechnicsPostponeTypeID
    INNER JOIN PMIS_ADM.Companies d ON a.OwnershipCompanyID = d.CompanyID
    WHERE c1.TechMilitaryReportStatusKey = 'POSTPONED'
    " + whereFulfil + @"
    GROUP BY d.AdministrationID, a.TechnicsTypeID
) f ON a.AdministrationID = f.AdministrationID AND b.TechnicsTypeID = f.TechnicsTypeID
WHERE NVL(b.Active, 0) = 1
ORDER BY a.AdministrationGroupSeq, a.AdministrationName, b.Seq, b.TechnicsTypeName
";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ReportPostponeTechByAdministrationBlock reportBlock = new ReportPostponeTechByAdministrationBlock();

                    reportBlock.AdministrationId = DBCommon.GetInt(dr["AdministrationID"]);
                    reportBlock.AdministrationName = dr["AdministrationName"].ToString();
                    reportBlock.TechnicsTypeId = DBCommon.GetInt(dr["TechnicsTypeID"]);
                    reportBlock.TechnicsTypeName = dr["TechnicsTypeName"].ToString();
                    
                    reportBlock.PostponeAbsolutely = DBCommon.GetInt(dr["PostponeAbsolutely"]);
                    reportBlock.FulfilConditioned = DBCommon.GetInt(dr["FulfilConditioned"]);
                    reportBlock.PostponeConditioned = DBCommon.GetInt(dr["PostponeConditioned"]);
                    reportBlock.FulfilAbsolutely = DBCommon.GetInt(dr["FulfilAbsolutely"]);

                    reportBlock.AnyDataForThisAdministration = DBCommon.GetInt(dr["AnyDataForThisAdministration"]);
                    
                    reportBlocks.Add(reportBlock);
                }

                dr.Close();

                //Show only administration for which there is some data (either postpone or fulfil)
                var administrationsWithData = reportBlocks.GroupBy(x => x.AdministrationId).Select(g => new
                {
                    AdministrationId = g.Key,
                    AnyDataForThisAdministration = g.Max(row => row.AnyDataForThisAdministration)
                }).Where(a => a.AnyDataForThisAdministration == 1).Select(x => x.AdministrationId);

                reportBlocks.RemoveAll(x => !administrationsWithData.Contains(x.AdministrationId));
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