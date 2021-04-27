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
    public class ReportPostponeTechFilter
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

    public class ReportPostponeTechBlock
    {
        public int TechnicsTypeId { get; set; }
        public string TechnicsTypeName { get; set; }
        public string TechnicsSubTypeName { get; set; }

        public int MilitaryReportTotal { get; set; }
        public int PostponeAbsolutely { get; set; }
        public int FulfilAbsolutely { get; set; }
        public int PostponeConditioned { get; set; }
        public int FulfilConditioned { get; set; }
    }

    public class ReportPostponeTechResult
    {
        public List<ReportPostponeTechBlock> AllBlocks { get; set; }
        public ReportPostponeTechFilter Filter { get; set; }
    }

    public static class ReportPostponeTechUtil
    {
        public static ReportPostponeTechResult GetReportPostponeTech(ReportPostponeTechFilter filter, User currentUser)
        {
            ReportPostponeTechResult reportResult = new ReportPostponeTechResult();
            List<ReportPostponeTechBlock> reportBlocks = new List<ReportPostponeTechBlock>();

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
                string whereMilitaryReport = "";

                if (!String.IsNullOrEmpty(filter.MilitaryDepartmentIds))
                {
                    wherePostpone += (wherePostpone == "" ? "" : " AND ") +
                                     " c.MilitaryDepartmentID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryDepartmentIds) + ") ";

                    whereFulfil += (whereFulfil == "" ? "" : " AND ") +
                                   " c.SourceMilDepartmentID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryDepartmentIds) + ") ";

                    whereMilitaryReport += (whereMilitaryReport == "" ? "" : " AND ") +
                                           " c.SourceMilDepartmentID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryDepartmentIds) + ") ";
                }

                wherePostpone += (wherePostpone == "" ? "" : " AND ") +
                         @" (c.MilitaryDepartmentID IS NULL OR c.MilitaryDepartmentID IN ( " + currentUser.MilitaryDepartmentIDs + ")) ";

                whereFulfil += (whereFulfil == "" ? "" : " AND ") +
                         @" (c.SourceMilDepartmentID IS NULL OR c.SourceMilDepartmentID IN ( " + currentUser.MilitaryDepartmentIDs + ")) ";

                whereMilitaryReport += (whereMilitaryReport == "" ? "" : " AND ") +
                         @" (c.SourceMilDepartmentID IS NULL OR c.SourceMilDepartmentID IN ( " + currentUser.MilitaryDepartmentIDs + ")) ";
                                               
                
                if (!string.IsNullOrEmpty(filter.PostponeYear))
                {
                    wherePostpone += (wherePostpone == "" ? "" : " AND ") +
                             @" c.PostponeYear IN ( " + CommonFunctions.AvoidSQLInjForListOfIDs(filter.PostponeYear) + ") ";

                    whereFulfil += (whereFulfil == "" ? "" : " AND ") +
                             @" c.TechnicsPostpone_Year IN ( " + CommonFunctions.AvoidSQLInjForListOfIDs(filter.PostponeYear) + ") ";
                }

                if (!String.IsNullOrEmpty(filter.CompanyIds))
                {
                    wherePostpone += (wherePostpone == "" ? "" : " AND ") +
                                     " b.CompanyID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.CompanyIds) + ") ";

                    whereFulfil += (whereFulfil == "" ? "" : " AND ") +
                                    " a.OwnershipCompanyID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.CompanyIds) + ") ";

                    whereMilitaryReport += (whereMilitaryReport == "" ? "" : " AND ") +
                                           " a.OwnershipCompanyID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.CompanyIds) + ") ";
                }

                wherePostpone = (wherePostpone == "" ? "" : " WHERE ") + wherePostpone;
                whereFulfil = (whereFulfil == "" ? "" : " AND ") + whereFulfil;
                whereMilitaryReport = (whereMilitaryReport == "" ? "" : " AND ") + whereMilitaryReport;


                string SQL = @"
SELECT b.TechnicsTypeID,
       b.TechnicsTypeName,
       a.TechnicsSubTypeName,
       NVL(mr.MilitaryReportTotal, 0) as MilitaryReportTotal,
       NVL(p.PostponeAbsolutely, 0) as PostponeAbsolutely,
       NVL(f.FulfilConditioned, 0) as FulfilConditioned,
       NVL(p.PostponeConditioned, 0) as PostponeConditioned,
       NVL(f.FulfilAbsolutely, 0) as FulfilAbsolutely
FROM PMIS_RES.TechnicsSubTypes a
INNER JOIN PMIS_RES.TechnicsTypes b ON a.TechnicsTypeID = b.TechnicsTypeID
LEFT OUTER JOIN (
    SELECT a.TechnicsSubTypeID,
           SUM(a.PostponeAbsolutely) as PostponeAbsolutely,
           SUM(a.PostponeConditioned) as PostponeConditioned
    FROM PMIS_RES.PostponeTechItems a
    INNER JOIN PMIS_RES.PostponeTechCompanies b ON a.PostponeTechCompanyID = b.PostponeTechCompanyID
    INNER JOIN PMIS_RES.PostponeTech c On b.PostponeTechID = c.PostponeTechID
    " + wherePostpone + @"
    GROUP BY a.TechnicsSubTypeID
) p ON a.TechnicsSubTypeID = p.TechnicsSubTypeID
LEFT OUTER JOIN (
    SELECT n.TechnicsSubTypeID,
           SUM(CASE WHEN g.TechnicsPostponeTypeKey = 'CONDITIONED' THEN 1 ELSE 0 END) as FulfilConditioned,
           SUM(CASE WHEN g.TechnicsPostponeTypeKey = 'ABSOLUTELY' THEN 1 ELSE 0 END) as FulfilAbsolutely
    FROM PMIS_RES.Technics a
    INNER JOIN PMIS_RES.TechnicsMilRepStatus c ON a.TechnicsID = c.TechnicsID AND c.IsCurrent = 1
    INNER JOIN PMIS_RES.TechMilitaryReportStatuses c1 ON c.TechMilitaryReportStatusID = c1.TechMilitaryReportStatusID
    INNER JOIN PMIS_RES.TechnicsPostponeTypes g ON c.TechnicsPostpone_TypeID = g.TechnicsPostponeTypeID
    INNER JOIN PMIS_RES.NormativeTechnics n ON a.NormativeTechnicsID = n.NormativeTechnicsID
    WHERE n.TechnicsSubTypeID IS NOT NULL AND
          c1.TechMilitaryReportStatusKey = 'POSTPONED'
    " + whereFulfil + @"
    GROUP BY n.TechnicsSubTypeID
) f ON a.TechnicsSubTypeID = f.TechnicsSubTypeID
LEFT OUTER JOIN (
    SELECT n.TechnicsSubTypeID,
           SUM(1) as MilitaryReportTotal
    FROM PMIS_RES.Technics a
    INNER JOIN PMIS_RES.TechnicsMilRepStatus c ON a.TechnicsID = c.TechnicsID AND c.IsCurrent = 1
    INNER JOIN PMIS_RES.TechMilitaryReportStatuses c1 ON c.TechMilitaryReportStatusID = c1.TechMilitaryReportStatusID
    INNER JOIN PMIS_RES.NormativeTechnics n ON a.NormativeTechnicsID = n.NormativeTechnicsID
    WHERE n.TechnicsSubTypeID IS NOT NULL AND
          c1.TechMilitaryReportStatusKey <> 'REMOVED'
    " + whereMilitaryReport + @"
    GROUP BY n.TechnicsSubTypeID
) mr ON a.TechnicsSubTypeID = mr.TechnicsSubTypeID
WHERE NVL(a.IsActive, 0) = 1 AND NVL(b.Active, 0) = 1
ORDER BY b.Seq, a.Seq
";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ReportPostponeTechBlock reportBlock = new ReportPostponeTechBlock();

                    reportBlock.TechnicsTypeId = DBCommon.GetInt(dr["TechnicsTypeID"]);
                    reportBlock.TechnicsTypeName = dr["TechnicsTypeName"].ToString();
                    reportBlock.TechnicsSubTypeName = dr["TechnicsSubTypeName"].ToString();

                    reportBlock.MilitaryReportTotal = DBCommon.GetInt(dr["MilitaryReportTotal"]);
                    reportBlock.PostponeAbsolutely = DBCommon.GetInt(dr["PostponeAbsolutely"]);
                    reportBlock.FulfilConditioned = DBCommon.GetInt(dr["FulfilConditioned"]);
                    reportBlock.PostponeConditioned = DBCommon.GetInt(dr["PostponeConditioned"]);
                    reportBlock.FulfilAbsolutely = DBCommon.GetInt(dr["FulfilAbsolutely"]);
                    
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
        public static List<Company> GetCompaniesList(User currentUser, ReportPostponeTechFilter filter)
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