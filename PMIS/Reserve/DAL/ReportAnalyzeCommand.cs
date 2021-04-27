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
    public class ReportAnalyzeCommandFilter
    {
        public string MilitaryDepartmentIds { get; set; }
        public int MilitaryReadinessID { get; set; }  
        public int MilitaryCommandId { get; set; }
        public string MilitaryCommandSuffix { get; set; }
        public string ReportType { get; set; }        
    }

    public class ReportOverallAnalyzeCommandResult
    {
        public int RequestOfficers { get; set; }
        public int RequestSergeants { get; set; }
        public int RequestSoldiers { get; set; }
        public int RequestOffCand { get; set; }
        public int FulfiledOfficers { get; set; }
        public int FulfiledSergeants { get; set; }
        public int FulfiledSoldiers { get; set; }
        public int FulfiledOffCand { get; set; }
        public int ReserveOfficers { get; set; }
        public int ReserveSergeants { get; set; }
        public int ReserveSoldiers { get; set; }
        public int ReserveOffCand { get; set; }
    }

    public class ReportMilRepSpecAnalyzeCommandBlock
    {
        public string MilRepSpecName { get; set; }
        public int RequestOfficers { get; set; }
        public int RequestSergeants { get; set; }
        public int RequestSoldiers { get; set; }
        public int RequestOffCand { get; set; }
        public int FulfiledOfficers { get; set; }
        public int FulfiledSergeants { get; set; }
        public int FulfiledSoldiers { get; set; }
        public int FulfiledOffCand { get; set; }
        public int ChangesOfficers { get; set; }
        public int ChangesSergeants { get; set; }
        public int ChangesSoldiers { get; set; }
        public int ChangesOffCand { get; set; }
        public int ReserveOfficers { get; set; }
        public int ReserveSergeants { get; set; }
        public int ReserveSoldiers { get; set; }
        public int ReserveOffCand { get; set; }
    }

    public class ReportPositionMRSAnalyzeMRSBlock
    {
        public string MilitaryReportSpecilityName { get; set; }
        public int Cnt { get; set; }
    }

    public class ReportPositionMRSAnalyzePositionBlock
    {
        public string Position { get; set; }
        public List<ReportPositionMRSAnalyzeMRSBlock> MRS { get; set; }
    }


    public class ReportPositionDeliveryAnalyzeBlock
    {
        public string MuniciplaityName { get; set; }
        public string DistrictName { get; set; }
        
        public int FulfiledOfficers { get; set; }
        public int FulfiledSergeants { get; set; }
        public int FulfiledSoldiers { get; set; }
        public int FulfiledOffCand { get; set; }
        
        public int ReserveOfficers { get; set; }
        public int ReserveSergeants { get; set; }
        public int ReserveSoldiers { get; set; }
        public int ReserveOffCand { get; set; }
    }

    public class ReportPositionFulfilAnalyzeBlock
    {
        public string Position { get; set; }
        public int VisheEducation { get; set; }
        public int PoluvisheEducation { get; set; }
        public int SrednoEducation { get; set; }
        public int VAEducation { get; set; }
        public int VUEducation { get; set; }
        public int SZHOEducation { get; set; }
        public int NoMilEducation { get; set; }
        public int AgeUnder35 { get; set; }
        public int AgeUnder45 { get; set; }
        public int AgeAbove45 { get; set; }
        public int MilitaryTraining { get; set; }
        public int NeedCourse { get; set; }
    }

    public class ReportMilRepSpecAndPositionAnalyzeCommandBlock
    {
        public string MilRepSpecName { get; set; }
        public bool IsPrimary { get; set; }
        public string Position { get; set; }
        public int RequestOfficers { get; set; }
        public int RequestSergeants { get; set; }
        public int RequestSoldiers { get; set; }
        public int RequestOffCand { get; set; }
        public int FulfiledOfficers { get; set; }
        public int FulfiledSergeants { get; set; }
        public int FulfiledSoldiers { get; set; }
        public int FulfiledOffCand { get; set; }
        public int ChangesOfficers { get; set; }
        public int ChangesSergeants { get; set; }
        public int ChangesSoldiers { get; set; }
        public int ChangesOffCand { get; set; }
        public int ReserveOfficers { get; set; }
        public int ReserveSergeants { get; set; }
        public int ReserveSoldiers { get; set; }
        public int ReserveOffCand { get; set; }
    }

    public class ReportAgeMRSAnalyzeMRSBlock
    {
        public string MilitaryReportSpecilityName { get; set; }
        public int Cnt { get; set; }
    }

    public class ReportAgeMRSAnalyzeBlock
    {
        public string Age { get; set; }
        public List<ReportAgeMRSAnalyzeMRSBlock> MRS { get; set; }
    }


    public class ReportAnalyzeCommandResult
    {
        public ReportAnalyzeCommandFilter Filter { get; set; }
        public ReportOverallAnalyzeCommandResult OverallResult { get; set; }
        public List<ReportMilRepSpecAnalyzeCommandBlock> MilRepSpecResult { get; set; }
        public List<ReportPositionMRSAnalyzePositionBlock> PositionMRSResult { get; set; }
        public List<ReportPositionDeliveryAnalyzeBlock> PositionDeliveryResult { get; set; }
        public List<ReportPositionFulfilAnalyzeBlock> PositionFulfilResult { get; set; }
        public List<ReportMilRepSpecAndPositionAnalyzeCommandBlock> MilRepSpecAndPositionResult { get; set; }
        public List<ReportAgeMRSAnalyzeBlock> AgeMRSResult { get; set; }
    }    

    public static class ReportAnalyzeCommandUtil
    {
        public static ReportAnalyzeCommandResult GetReportAnalyzeCommand(ReportAnalyzeCommandFilter filter, User currentUser)
        {
            ReportAnalyzeCommandResult reportResult = new ReportAnalyzeCommandResult();

            reportResult.OverallResult = ReportAnalyzeCommandUtil.GetReportOverallAnalyzeCommand(filter, currentUser);

            if (filter.ReportType.Contains("MilRepSpecAnalyze"))
                reportResult.MilRepSpecResult = ReportAnalyzeCommandUtil.GetReportMilRepSpecAnalyzeCommand(filter, currentUser);

            if (filter.ReportType.Contains("MilRepSpecPositionAnalyze"))
                reportResult.PositionMRSResult = ReportAnalyzeCommandUtil.GetReportPositionMRSAnalyze(filter, currentUser);

            if (filter.ReportType.Contains("DevilveryAnalyze"))
                reportResult.PositionDeliveryResult = ReportAnalyzeCommandUtil.GetReportPositionDeliveryAnalyze(filter, currentUser);

            if (filter.ReportType.Contains("PositionFulfilAnalyze"))
                reportResult.PositionFulfilResult = ReportAnalyzeCommandUtil.GetReportPositionFulfilAnalyze(filter, currentUser);

            if (filter.ReportType.Contains("MilRepSpecAndPositionAnalyze"))
                reportResult.MilRepSpecAndPositionResult = ReportAnalyzeCommandUtil.GetReportMilRepSpecAndPositionAnalyzeCommand(filter, currentUser);

            if (filter.ReportType.Contains("AgeMilRepSpecFulfilAnalyze"))
                reportResult.AgeMRSResult = ReportAnalyzeCommandUtil.GetReportAgeMRSAnalyze(filter, currentUser);

            return reportResult;
        }

        public static ReportOverallAnalyzeCommandResult GetReportOverallAnalyzeCommand(ReportAnalyzeCommandFilter filter, User currentUser)
        {
            ReportOverallAnalyzeCommandResult reportResult = new ReportOverallAnalyzeCommandResult();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string whereClause = "";
                string whereClause2 = "";

                if (!String.IsNullOrEmpty(filter.MilitaryDepartmentIds))
                {
                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                                    " x.MilitaryDepartmentId IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryDepartmentIds) + ") ";

                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " d.MilitaryDepartmentId IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryDepartmentIds) + ") ";
                }

                whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                         @" (x.MilitaryDepartmentId IS NULL OR x.MilitaryDepartmentId IN ( " + currentUser.MilitaryDepartmentIDs + ")) ";

                whereClause += (whereClause == "" ? "" : " AND ") +
                        @" (d.MilitaryDepartmentId IS NULL OR d.MilitaryDepartmentId IN ( " + currentUser.MilitaryDepartmentIDs + ")) ";

                if (filter.MilitaryCommandId > -1)
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " b.MilitaryCommandId = " + filter.MilitaryCommandId.ToString();

                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                                    " b.MilitaryCommandId = " + filter.MilitaryCommandId.ToString();
                }

                if (filter.MilitaryReadinessID > -1)
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " b.milreadinessid = " + filter.MilitaryReadinessID.ToString();

                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                                    " b.milreadinessid = " + filter.MilitaryReadinessID.ToString();
                }

                if (!String.IsNullOrEmpty(filter.MilitaryCommandSuffix) && filter.MilitaryCommandSuffix != "-1")
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " b.MilitaryCommandSuffix = '" + filter.MilitaryCommandSuffix + "' ";

                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                                    " b.MilitaryCommandSuffix = '" + filter.MilitaryCommandSuffix + "' ";
                }

                whereClause = (whereClause == "" ? "" : " WHERE ") + whereClause;

                whereClause2 = (whereClause2 == "" ? "" : " WHERE ") + whereClause2;

                string SQL = @"
                               SELECT   SUM(CASE WHEN i.MilitaryRankCategory = 2 THEN NVL(x.ReservistsCount, 0) ELSE 0 END) As RequestOfficers,
                                        SUM(CASE WHEN i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 1 THEN NVL(x.ReservistsCount, 0) ELSE 0 END) As RequestSergeants,
                                        SUM(CASE WHEN i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 2 THEN NVL(x.ReservistsCount, 0) ELSE 0 END) As RequestSoldiers,
                                        SUM(CASE WHEN i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 3 THEN NVL(x.ReservistsCount, 0) ELSE 0 END) As RequestOffCand,
                                        NULL as FulfiledOfficers,
                                        NULL as FulfiledSergeants,
                                        NULL as FulfiledSoldiers,
                                        NULL as FulfiledOffCand,
                                        NULL as ReserveOfficers,
                                        NULL as ReserveSergeants,
                                        NULL as ReserveSoldiers,
                                        NULL as ReserveOffCand
                               FROM UKAZ_OWNER.VVR a
                               INNER JOIN PMIS_RES.REQUESTSCOMMANDS b ON b.MilitaryCommandId = a.KOD_VVR
                               INNER JOIN PMIS_RES.REQUESTCOMMANDPOSITIONS c ON c.RequestsCommandID = b.RequestsCommandID
                               LEFT OUTER JOIN PMIS_RES.CommandPositionMilRanks mr ON c.RequestCommandPositionID = mr.RequestCommandPositionID AND mr.IsPrimary = 1
                               LEFT OUTER JOIN VS_OWNER.KLV_ZVA h ON h.ZVA_KOD = mr.MilitaryRankID
                               LEFT OUTER JOIN PMIS_ADM.MilitaryRankCategories i ON i.ZVA_KAT_KOD = h.ZVA_KAT_KOD
                               LEFT OUTER JOIN PMIS_RES.RequestCommandPositionsMilDept x ON x.RequestCommandPositionID = c.RequestCommandPositionID
                               " + whereClause2 + @"

                               UNION ALL

                               SELECT   NULL As RequestOfficers,
                                        NULL As RequestSergeants,
                                        NULL As RequestSoldiers,
                                        NULL As RequestOffCand,
                                        SUM(CASE WHEN i.MilitaryRankCategory = 2 THEN 1 ELSE 0 END) As FulfiledOfficers,
                                        SUM(CASE WHEN i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 1 THEN 1 ELSE 0 END) As FulfiledSergeants,
                                        SUM(CASE WHEN i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 2 THEN 1 ELSE 0 END) As FulfiledSoldiers,
                                        SUM(CASE WHEN i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 3 THEN 1 ELSE 0 END) As FulfiledOffCand,
                                        NULL as ReserveOfficers,
                                        NULL as ReserveSergeants,
                                        NULL as ReserveSoldiers,
                                        NULL as ReserveOffCand
                               FROM UKAZ_OWNER.VVR a
                               INNER JOIN PMIS_RES.REQUESTSCOMMANDS b ON b.MilitaryCommandId = a.KOD_VVR
                               INNER JOIN PMIS_RES.REQUESTCOMMANDPOSITIONS c ON c.RequestsCommandID = b.RequestsCommandID
                               INNER JOIN PMIS_RES.FILLRESERVISTSREQUEST d ON d.RequestCommandPositionId = c.RequestCommandPositionId AND d.ReservistReadinessID = 1 AND NVL(d.AppointmentIsDelivered, 0) = 1
                               INNER JOIN PMIS_RES.RESERVISTS f ON f.ReservistId = d.ReservistId
                               INNER JOIN VS_OWNER.VS_LS g ON g.PersonId = f.PersonId                               
                               INNER JOIN VS_OWNER.KLV_ZVA h ON h.ZVA_KOD = g.KOD_ZVA                               
                               INNER JOIN PMIS_ADM.MilitaryRankCategories i ON i.ZVA_KAT_KOD = h.ZVA_KAT_KOD
                               " + whereClause + @"

                               UNION ALL

                               SELECT   NULL As RequestOfficers,
                                        NULL As RequestSergeants,
                                        NULL As RequestSoldiers,
                                        NULL As RequestOffCand,
                                        NULL As FulfiledOfficers,
                                        NULL As FulfiledSergeants,
                                        NULL As FulfiledSoldiers,
                                        NULL As FulfiledOffCand,
                                        SUM(CASE WHEN i.MilitaryRankCategory = 2 THEN 1 ELSE 0 END) as ReserveOfficers,
                                        SUM(CASE WHEN i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 1 THEN 1 ELSE 0 END) as ReserveSergeants,
                                        SUM(CASE WHEN i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 2 THEN 1 ELSE 0 END) as ReserveSoldiers,
                                        SUM(CASE WHEN i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 3 THEN 1 ELSE 0 END) as ReserveOffCand
                               FROM UKAZ_OWNER.VVR a
                               INNER JOIN PMIS_RES.REQUESTSCOMMANDS b ON b.MilitaryCommandId = a.KOD_VVR
                               INNER JOIN PMIS_RES.REQUESTCOMMANDPOSITIONS c ON c.RequestsCommandID = b.RequestsCommandID
                               INNER JOIN PMIS_RES.FILLRESERVISTSREQUEST d ON d.RequestCommandPositionId = c.RequestCommandPositionId AND d.ReservistReadinessID = 2 AND NVL(d.AppointmentIsDelivered, 0) = 1
                               INNER JOIN PMIS_RES.RESERVISTS f ON f.ReservistId = d.ReservistId
                               INNER JOIN VS_OWNER.VS_LS g ON g.PersonId = f.PersonId                               
                               INNER JOIN VS_OWNER.KLV_ZVA h ON h.ZVA_KOD = g.KOD_ZVA                               
                               INNER JOIN PMIS_ADM.MilitaryRankCategories i ON i.ZVA_KAT_KOD = h.ZVA_KAT_KOD
                               " + whereClause + @"

                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (dr["RequestOfficers"] is decimal)
                        reportResult.RequestOfficers =  int.Parse(dr["RequestOfficers"].ToString());

                    if (dr["RequestSergeants"] is decimal)
                        reportResult.RequestSergeants = int.Parse(dr["RequestSergeants"].ToString());

                    if (dr["RequestSoldiers"] is decimal)
                        reportResult.RequestSoldiers = int.Parse(dr["RequestSoldiers"].ToString());

                    if (dr["RequestOffCand"] is decimal)
                        reportResult.RequestOffCand = int.Parse(dr["RequestOffCand"].ToString());

                    if (dr["FulfiledOfficers"] is decimal)
                        reportResult.FulfiledOfficers = int.Parse(dr["FulfiledOfficers"].ToString());

                    if (dr["FulfiledSergeants"] is decimal)
                        reportResult.FulfiledSergeants = int.Parse(dr["FulfiledSergeants"].ToString());

                    if (dr["FulfiledSoldiers"] is decimal)
                        reportResult.FulfiledSoldiers = int.Parse(dr["FulfiledSoldiers"].ToString());

                    if (dr["FulfiledOffCand"] is decimal)
                        reportResult.FulfiledOffCand = int.Parse(dr["FulfiledOffCand"].ToString());

                    if (dr["ReserveOfficers"] is decimal)
                        reportResult.ReserveOfficers = int.Parse(dr["ReserveOfficers"].ToString());

                    if (dr["ReserveSergeants"] is decimal)
                        reportResult.ReserveSergeants = int.Parse(dr["ReserveSergeants"].ToString());

                    if (dr["ReserveSoldiers"] is decimal)
                        reportResult.ReserveSoldiers = int.Parse(dr["ReserveSoldiers"].ToString());

                    if (dr["ReserveOffCand"] is decimal)
                        reportResult.ReserveOffCand = int.Parse(dr["ReserveOffCand"].ToString());
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return reportResult;
        }

        public static List<ReportMilRepSpecAnalyzeCommandBlock> GetReportMilRepSpecAnalyzeCommand(ReportAnalyzeCommandFilter filter, User currentUser)
        {
            List<ReportMilRepSpecAnalyzeCommandBlock> reportResult = new List<ReportMilRepSpecAnalyzeCommandBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string whereClause = "";
                string whereClause2 = "";

                if (!String.IsNullOrEmpty(filter.MilitaryDepartmentIds))
                {
                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                                    " x.MilitaryDepartmentId IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryDepartmentIds) + ") ";

                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " d.MilitaryDepartmentId IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryDepartmentIds) + ") ";
                }

                whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                         @" (x.MilitaryDepartmentId IS NULL OR x.MilitaryDepartmentId IN ( " + currentUser.MilitaryDepartmentIDs + ")) ";

                whereClause += (whereClause == "" ? "" : " AND ") +
                        @" (d.MilitaryDepartmentId IS NULL OR d.MilitaryDepartmentId IN ( " + currentUser.MilitaryDepartmentIDs + ")) ";

                if (filter.MilitaryReadinessID > -1)
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " b.milreadinessid = " + filter.MilitaryReadinessID.ToString();

                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                                    " b.milreadinessid = " + filter.MilitaryReadinessID.ToString();
                }

                if (filter.MilitaryCommandId > -1)
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " b.MilitaryCommandId = " + filter.MilitaryCommandId.ToString();

                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                                    " b.MilitaryCommandId = " + filter.MilitaryCommandId.ToString();
                }

                if (!String.IsNullOrEmpty(filter.MilitaryCommandSuffix) && filter.MilitaryCommandSuffix != "-1")
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " b.MilitaryCommandSuffix = '" + filter.MilitaryCommandSuffix + "' ";

                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                                    " b.MilitaryCommandSuffix = '" + filter.MilitaryCommandSuffix + "' ";
                }

                whereClause = (whereClause == "" ? "" : " WHERE ") + whereClause;

                whereClause2 = (whereClause2 == "" ? "" : " WHERE ") + whereClause2;

                string SQL = @"
                               SELECT	RES.MilReportSpecialityID, 
		                                MRS.MilReportingSpecialityCode || ' ' || MRS.MilReportingSpecialityName as MilReportingSpecialityName, 
		                                SUM(NVL(RequestOfficers, 0)) as RequestOfficers, 
		                                SUM(NVL(RequestSergeants, 0)) as RequestSergeants, 
		                                SUM(NVL(RequestSoldiers, 0)) as RequestSoldiers, 
		                                SUM(NVL(RequestOffCand, 0)) as RequestOffCand, 
		                                SUM(NVL(FulfiledOfficers, 0)) as FulfiledOfficers, 
		                                SUM(NVL(FulfiledSergeants, 0)) as FulfiledSergeants, 
		                                SUM(NVL(FulfiledSoldiers, 0)) as FulfiledSoldiers, 
		                                SUM(NVL(FulfiledOffCand, 0)) as FulfiledOffCand, 
		                                SUM(NVL(ChangesOfficers, 0)) as ChangesOfficers, 
		                                SUM(NVL(ChangesSergeants, 0)) as ChangesSergeants, 
		                                SUM(NVL(ChangesSoldiers, 0)) as ChangesSoldiers,
		                                SUM(NVL(ChangesOffCand, 0)) as ChangesOffCand,
		                                SUM(NVL(ReserveOfficers, 0)) as ReserveOfficers, 
		                                SUM(NVL(ReserveSergeants, 0)) as ReserveSergeants, 
		                                SUM(NVL(ReserveSoldiers, 0)) as ReserveSoldiers,
		                                SUM(NVL(ReserveOffCand, 0)) as ReserveOffCand 
                                FROM
                                (
                                                            SELECT   y.MilReportSpecialityID, SUM(CASE WHEN i.MilitaryRankCategory = 2 THEN NVL(x.ReservistsCount, 0) ELSE 0 END) As RequestOfficers,
                                                                                              SUM(CASE WHEN i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 1 THEN NVL(x.ReservistsCount, 0) ELSE 0 END) As RequestSergeants,
                                                                                              SUM(CASE WHEN i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 2 THEN NVL(x.ReservistsCount, 0) ELSE 0 END) As RequestSoldiers,
                                                                                              SUM(CASE WHEN i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 3 THEN NVL(x.ReservistsCount, 0) ELSE 0 END) As RequestOffCand,
                                                                                              NULL as FulfiledOfficers,
                                                                                              NULL as FulfiledSergeants,
															                                  NULL as FulfiledSoldiers,
															                                  NULL as FulfiledOffCand,
                                                                                              NULL as ChangesOfficers,
                                                                                              NULL as ChangesSergeants,
															                                  NULL as ChangesSoldiers,
															                                  NULL as ChangesOffCand,
															                                  NULL as ReserveOfficers,
															                                  NULL as ReserveSergeants,
														                                      NULL as ReserveSoldiers,
														                                      NULL as ReserveOffCand
                                                               FROM UKAZ_OWNER.VVR a
                                                               INNER JOIN PMIS_RES.REQUESTSCOMMANDS b ON b.MilitaryCommandId = a.KOD_VVR
                                                               INNER JOIN PMIS_RES.REQUESTCOMMANDPOSITIONS c ON c.RequestsCommandID = b.RequestsCommandID
                                                               LEFT OUTER JOIN PMIS_RES.CommandPositionMilRanks mr ON c.RequestCommandPositionID = mr.RequestCommandPositionID AND mr.IsPrimary = 1
                                                               LEFT OUTER JOIN VS_OWNER.KLV_ZVA h ON h.ZVA_KOD = mr.MilitaryRankID
                                                               LEFT OUTER JOIN PMIS_ADM.MilitaryRankCategories i ON i.ZVA_KAT_KOD = h.ZVA_KAT_KOD
                                                               LEFT OUTER JOIN PMIS_RES.RequestCommandPositionsMilDept x ON x.RequestCommandPositionID = c.RequestCommandPositionID
                                                               INNER JOIN PMIS_RES.CommandPositionMRSpecialities y ON c.RequestCommandPositionID = y.RequestCommandPositionID
                                                               " + whereClause2 + @"
                                                               GROUP BY y.MilReportSpecialityID

                                                               UNION ALL

                                                               SELECT   y.MilReportSpecialityID, 
                                                                        NULL As RequestOfficers,
                                                                        NULL As RequestSergeants,
                                                                        NULL As RequestSoldiers,
                                                                        NULL As RequestOffCand,
                                                                        SUM(CASE WHEN i.MilitaryRankCategory = 2 THEN 1 ELSE 0 END) As FulfiledOfficers,
                                                                        SUM(CASE WHEN i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 1 THEN 1 ELSE 0 END) As FulfiledSergeants,
                                                                        SUM(CASE WHEN i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 2 THEN 1 ELSE 0 END) As FulfiledSoldiers,
                                                                        SUM(CASE WHEN i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 3 THEN 1 ELSE 0 END) As FulfiledOffCand,
                                                                        NULL as ChangesOfficers,
                                                                        NULL as ChangesSergeants,
										                                NULL as ChangesSoldiers,
										                                NULL as ChangesOffCand,
                                                                        NULL as ReserveOfficers,
                                                                        NULL as ReserveSergeants,
                                                                        NULL as ReserveSoldiers,
                                                                        NULL as ReserveOffCand
                                                               FROM UKAZ_OWNER.VVR a
                                                               INNER JOIN PMIS_RES.REQUESTSCOMMANDS b ON b.MilitaryCommandId = a.KOD_VVR
                                                               INNER JOIN PMIS_RES.REQUESTCOMMANDPOSITIONS c ON c.RequestsCommandID = b.RequestsCommandID
                                                               INNER JOIN PMIS_RES.FILLRESERVISTSREQUEST d ON d.RequestCommandPositionId = c.RequestCommandPositionId AND d.ReservistReadinessID = 1 AND NVL(d.AppointmentIsDelivered, 0) = 1
                                                               INNER JOIN PMIS_RES.RESERVISTS f ON f.ReservistId = d.ReservistId
                                                               INNER JOIN VS_OWNER.VS_LS g ON g.PersonId = f.PersonId                               
                                                               INNER JOIN VS_OWNER.KLV_ZVA h ON h.ZVA_KOD = g.KOD_ZVA                               
                                                               INNER JOIN PMIS_ADM.MilitaryRankCategories i ON i.ZVA_KAT_KOD = h.ZVA_KAT_KOD
                                                               INNER JOIN PMIS_RES.CommandPositionMRSpecialities y ON c.RequestCommandPositionID = y.RequestCommandPositionID AND d.MilReportSpecialityID = y.MilReportSpecialityID
                                                               " + whereClause + @"
                                                               GROUP BY y.MilReportSpecialityID
                                							   
							                                   UNION ALL

                                                               SELECT   y.MilReportSpecialityID, 
                                                                        NULL As RequestOfficers,
                                                                        NULL As RequestSergeants,
                                                                        NULL As RequestSoldiers,
                                                                        NULL As RequestOffCand,
                                                                        NULL As FulfiledOfficers,
                                                                        NULL As FulfiledSergeants,
                                                                        NULL As FulfiledSoldiers,
                                                                        NULL As FulfiledOffCand,
                                                                        SUM(CASE WHEN i.MilitaryRankCategory = 2 AND z.PersonMilRepSpecID IS NULL THEN 1 ELSE 0 END) As ChangesOfficers,
                                                                        SUM(CASE WHEN i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 1 AND z.PersonMilRepSpecID IS NULL THEN 1 ELSE 0 END) As ChangesSergeants,
                                                                        SUM(CASE WHEN i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 2 AND z.PersonMilRepSpecID IS NULL THEN 1 ELSE 0 END) As ChangesSoldiers,
                                                                        SUM(CASE WHEN i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 3 AND z.PersonMilRepSpecID IS NULL THEN 1 ELSE 0 END) As ChangesOffCand,
                                                                        NULL as ReserveOfficers,
                                                                        NULL as ReserveSergeants,
                                                                        NULL as ReserveSoldiers,
                                                                        NULL as ReserveOffCand
                                                               FROM UKAZ_OWNER.VVR a
                                                               INNER JOIN PMIS_RES.REQUESTSCOMMANDS b ON b.MilitaryCommandId = a.KOD_VVR
                                                               INNER JOIN PMIS_RES.REQUESTCOMMANDPOSITIONS c ON c.RequestsCommandID = b.RequestsCommandID
                                                               INNER JOIN PMIS_RES.FILLRESERVISTSREQUEST d ON d.RequestCommandPositionId = c.RequestCommandPositionId AND d.ReservistReadinessID = 1 AND NVL(d.AppointmentIsDelivered, 0) = 1
                                                               INNER JOIN PMIS_RES.RESERVISTS f ON f.ReservistId = d.ReservistId
                                                               INNER JOIN VS_OWNER.VS_LS g ON g.PersonId = f.PersonId                               
                                                               INNER JOIN VS_OWNER.KLV_ZVA h ON h.ZVA_KOD = g.KOD_ZVA                               
                                                               INNER JOIN PMIS_ADM.MilitaryRankCategories i ON i.ZVA_KAT_KOD = h.ZVA_KAT_KOD
                                                               INNER JOIN PMIS_RES.CommandPositionMRSpecialities y ON c.RequestCommandPositionID = y.RequestCommandPositionID AND d.MilReportSpecialityID = y.MilReportSpecialityID
							                                   LEFT OUTER JOIN PMIS_ADM.PersonMilRepSpec z ON z.PersonID = f.PersonID AND z.MilReportSpecialityID = d.MilReportSpecialityID
                                                               " + whereClause + @"
                                                               GROUP BY y.MilReportSpecialityID

                                                               UNION ALL

                                                               SELECT  y.MilReportSpecialityID, 
                                                                        NULL As RequestOfficers,
                                                                        NULL As RequestSergeants,
                                                                        NULL As RequestSoldiers,
                                                                        NULL As RequestOffCand,
                                                                        NULL As FulfiledOfficers,
                                                                        NULL As FulfiledSergeants,
                                                                        NULL As FulfiledSoldiers,
                                                                        NULL As FulfiledOffCand,
                                                                        NULL as ChangesOfficers,
                                                                        NULL as ChangesSergeants,
										                                NULL as ChangesSoldiers,
										                                NULL as ChangesOffCand,
                                                                        SUM(CASE WHEN i.MilitaryRankCategory = 2 THEN 1 ELSE 0 END) as ReserveOfficers,
                                                                        SUM(CASE WHEN i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 1 THEN 1 ELSE 0 END) as ReserveSergeants,
                                                                        SUM(CASE WHEN i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 2 THEN 1 ELSE 0 END) as ReserveSoldiers,
                                                                        SUM(CASE WHEN i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 3 THEN 1 ELSE 0 END) as ReserveOffCand
                                                               FROM UKAZ_OWNER.VVR a
                                                               INNER JOIN PMIS_RES.REQUESTSCOMMANDS b ON b.MilitaryCommandId = a.KOD_VVR
                                                               INNER JOIN PMIS_RES.REQUESTCOMMANDPOSITIONS c ON c.RequestsCommandID = b.RequestsCommandID
                                                               INNER JOIN PMIS_RES.FILLRESERVISTSREQUEST d ON d.RequestCommandPositionId = c.RequestCommandPositionId AND d.ReservistReadinessID = 2 AND NVL(d.AppointmentIsDelivered, 0) = 1
                                                               INNER JOIN PMIS_RES.RESERVISTS f ON f.ReservistId = d.ReservistId
                                                               INNER JOIN VS_OWNER.VS_LS g ON g.PersonId = f.PersonId                               
                                                               INNER JOIN VS_OWNER.KLV_ZVA h ON h.ZVA_KOD = g.KOD_ZVA                               
                                                               INNER JOIN PMIS_ADM.MilitaryRankCategories i ON i.ZVA_KAT_KOD = h.ZVA_KAT_KOD
                                                               INNER JOIN PMIS_RES.CommandPositionMRSpecialities y ON c.RequestCommandPositionID = y.RequestCommandPositionID AND d.MilReportSpecialityID = y.MilReportSpecialityID
                                                               " + whereClause + @"
                                                               GROUP BY y.MilReportSpecialityID
                                  ) RES
                                  INNER JOIN PMIS_ADM.MilitaryReportSpecialities MRS ON RES.MilReportSpecialityID = MRS.MilReportSpecialityID
                                  GROUP BY RES.MilReportSpecialityID, MRS.MilReportingSpecialityCode, MRS.MilReportingSpecialityName
                                  ORDER BY MRS.MilReportingSpecialityCode
                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ReportMilRepSpecAnalyzeCommandBlock block = new ReportMilRepSpecAnalyzeCommandBlock();

                    block.MilRepSpecName = dr["MilReportingSpecialityName"].ToString();

                    if (dr["RequestOfficers"] is decimal)
                        block.RequestOfficers = int.Parse(dr["RequestOfficers"].ToString());

                    if (dr["RequestOffCand"] is decimal)
                        block.RequestOffCand = int.Parse(dr["RequestOffCand"].ToString());

                    if (dr["RequestSergeants"] is decimal)
                        block.RequestSergeants = int.Parse(dr["RequestSergeants"].ToString());

                    if (dr["RequestSoldiers"] is decimal)
                        block.RequestSoldiers = int.Parse(dr["RequestSoldiers"].ToString());

                    if (dr["FulfiledOfficers"] is decimal)
                        block.FulfiledOfficers = int.Parse(dr["FulfiledOfficers"].ToString());

                    if (dr["FulfiledOffCand"] is decimal)
                        block.FulfiledOffCand = int.Parse(dr["FulfiledOffCand"].ToString());

                    if (dr["FulfiledSergeants"] is decimal)
                        block.FulfiledSergeants = int.Parse(dr["FulfiledSergeants"].ToString());

                    if (dr["FulfiledSoldiers"] is decimal)
                        block.FulfiledSoldiers = int.Parse(dr["FulfiledSoldiers"].ToString());

                    if (dr["ChangesOfficers"] is decimal)
                        block.ChangesOfficers = int.Parse(dr["ChangesOfficers"].ToString());

                    if (dr["ChangesOffCand"] is decimal)
                        block.ChangesOffCand = int.Parse(dr["ChangesOffCand"].ToString());

                    if (dr["ChangesSergeants"] is decimal)
                        block.ChangesSergeants = int.Parse(dr["ChangesSergeants"].ToString());

                    if (dr["ChangesSoldiers"] is decimal)
                        block.ChangesSoldiers = int.Parse(dr["ChangesSoldiers"].ToString());

                    if (dr["ReserveOfficers"] is decimal)
                        block.ReserveOfficers = int.Parse(dr["ReserveOfficers"].ToString());

                    if (dr["ReserveOffCand"] is decimal)
                        block.ReserveOffCand = int.Parse(dr["ReserveOffCand"].ToString());

                    if (dr["ReserveSergeants"] is decimal)
                        block.ReserveSergeants = int.Parse(dr["ReserveSergeants"].ToString());

                    if (dr["ReserveSoldiers"] is decimal)
                        block.ReserveSoldiers = int.Parse(dr["ReserveSoldiers"].ToString());

                    reportResult.Add(block);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return reportResult;
        }

        public static List<ReportPositionMRSAnalyzePositionBlock> GetReportPositionMRSAnalyze(ReportAnalyzeCommandFilter filter, User currentUser)
        {
            List<ReportPositionMRSAnalyzePositionBlock> reportResult = new List<ReportPositionMRSAnalyzePositionBlock>();

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

                if (filter.MilitaryReadinessID > -1)
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " b.milreadinessid = " + filter.MilitaryReadinessID.ToString();
                }


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

                whereClause = (whereClause == "" ? "" : " WHERE ") + whereClause;                

                string SQL = @"
                               
                                SELECT a.Position, MRS.MilReportingSpecialityCode || ' ' || MRS.MilReportingSpecialityName as MilReportingSpecialityName, SUM(CASE WHEN a.MilReportSpecialityID = d.MilReportSpecialityID THEN 1 ELSE 0 END) as Cnt
                                FROM
                                (
                                  SELECT c.Position, y.MilReportSpecialityID
                                  FROM UKAZ_OWNER.VVR a
                                  INNER JOIN PMIS_RES.REQUESTSCOMMANDS b ON b.MilitaryCommandId = a.KOD_VVR
                                  INNER JOIN PMIS_RES.REQUESTCOMMANDPOSITIONS c ON c.RequestsCommandID = b.RequestsCommandID
                                  INNER JOIN PMIS_RES.FILLRESERVISTSREQUEST d ON d.RequestCommandPositionId = c.RequestCommandPositionId AND d.ReservistReadinessID = 1 AND NVL(d.AppointmentIsDelivered, 0) = 1
                                  INNER JOIN PMIS_RES.RESERVISTS f ON f.ReservistId = d.ReservistId
                                  INNER JOIN VS_OWNER.VS_LS g ON g.PersonId = f.PersonId
                                  INNER JOIN PMIS_RES.CommandPositionMRSpecialities y ON c.RequestCommandPositionID = y.RequestCommandPositionID
                                  " + whereClause + @"
                                  GROUP BY c.Position, y.MilReportSpecialityID  
                                ) a
                                INNER JOIN PMIS_RES.REQUESTCOMMANDPOSITIONS c ON a.Position = c.Position
                                INNER JOIN PMIS_RES.REQUESTSCOMMANDS b ON b.RequestsCommandID = c.RequestsCommandID
                                INNER JOIN PMIS_RES.FILLRESERVISTSREQUEST d ON d.RequestCommandPositionId = c.RequestCommandPositionId AND d.ReservistReadinessID = 1 AND NVL(d.AppointmentIsDelivered, 0) = 1
                                INNER JOIN PMIS_RES.RESERVISTS f ON f.ReservistId = d.ReservistId
                                INNER JOIN PMIS_RES.CommandPositionMRSpecialities y ON c.RequestCommandPositionID = y.RequestCommandPositionID AND a.MilReportSpecialityID = y.MilReportSpecialityID
                                INNER JOIN PMIS_ADM.MilitaryReportSpecialities MRS ON a.MilReportSpecialityID = MRS.MilReportSpecialityID
                                " + whereClause + @"
                                GROUP BY a.Position, MRS.MilReportingSpecialityCode, MRS.MilReportingSpecialityName
                                ORDER BY a.Position
                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                ReportPositionMRSAnalyzePositionBlock positionBlock = null;
                ReportPositionMRSAnalyzeMRSBlock mrsBlock = null;

                string position = "";
                string militaryReportSpecialityName = "";
                int cnt = 0;

                while (dr.Read())
                {
                    if (position != dr["Position"].ToString() && positionBlock != null)
                        reportResult.Add(positionBlock);

                    if (position != dr["Position"].ToString())
                    {
                        positionBlock = new ReportPositionMRSAnalyzePositionBlock();
                        positionBlock.MRS = new List<ReportPositionMRSAnalyzeMRSBlock>();
                    }

                    position = dr["Position"].ToString();
                    militaryReportSpecialityName = dr["MilReportingSpecialityName"].ToString();
                    cnt = int.Parse(dr["Cnt"].ToString());

                    positionBlock.Position = position;

                    mrsBlock = new ReportPositionMRSAnalyzeMRSBlock();
                    mrsBlock.MilitaryReportSpecilityName = militaryReportSpecialityName;
                    mrsBlock.Cnt = cnt;

                    positionBlock.MRS.Add(mrsBlock);
                }

                dr.Close();

                if (positionBlock != null)
                    reportResult.Add(positionBlock);
            }
            finally
            {
                conn.Close();
            }

            return reportResult;
        }

        public static List<ReportPositionDeliveryAnalyzeBlock> GetReportPositionDeliveryAnalyze(ReportAnalyzeCommandFilter filter, User currentUser)
        {
            List<ReportPositionDeliveryAnalyzeBlock> reportResult = new List<ReportPositionDeliveryAnalyzeBlock>();

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

                if (filter.MilitaryReadinessID > -1)
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " b.milreadinessid = " + filter.MilitaryReadinessID.ToString();
                }

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

                whereClause = (whereClause == "" ? "" : " WHERE ") + whereClause;

                string SQL = @"                               
                                SELECT
	                               p.IME_OBS as MunicipalityName,
                                    dis.DistrictName,

                                    SUM(CASE WHEN d.ReservistReadinessID = 1 AND i.MilitaryRankCategory = 2 THEN 1 ELSE 0 END) As FulfiledOfficers,
                                    SUM(CASE WHEN d.ReservistReadinessID = 1 AND i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 1 THEN 1 ELSE 0 END) As FulfiledSergeants,
                                    SUM(CASE WHEN d.ReservistReadinessID = 1 AND i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 2 THEN 1 ELSE 0 END) As FulfiledSoldiers,
                                    SUM(CASE WHEN d.ReservistReadinessID = 1 AND i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 3 THEN 1 ELSE 0 END) As FulfiledOffCand,
                                    
                                    SUM(CASE WHEN d.ReservistReadinessID = 2 AND i.MilitaryRankCategory = 2 THEN 1 ELSE 0 END) as ReserveOfficers,
                                    SUM(CASE WHEN d.ReservistReadinessID = 2 AND i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 1 THEN 1 ELSE 0 END) as ReserveSergeants,
                                    SUM(CASE WHEN d.ReservistReadinessID = 2 AND i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 2 THEN 1 ELSE 0 END) as ReserveSoldiers,
                                    SUM(CASE WHEN d.ReservistReadinessID = 2 AND i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 3 THEN 1 ELSE 0 END) as ReserveOffCand
                                FROM UKAZ_OWNER.VVR a
                                INNER JOIN PMIS_RES.REQUESTSCOMMANDS b ON b.MilitaryCommandId = a.KOD_VVR
                                INNER JOIN PMIS_RES.REQUESTCOMMANDPOSITIONS c ON c.RequestsCommandID = b.RequestsCommandID
                                INNER JOIN PMIS_RES.FILLRESERVISTSREQUEST d ON d.RequestCommandPositionId = c.RequestCommandPositionId AND NVL(d.AppointmentIsDelivered, 0) = 1
                                INNER JOIN PMIS_RES.RESERVISTS f ON f.ReservistId = d.ReservistId
                                INNER JOIN PMIS_ADM.Persons per ON per.PersonID = f.PersonID
                                INNER JOIN VS_OWNER.VS_LS g ON g.PersonId = f.PersonId
                                INNER JOIN UKAZ_OWNER.KL_NMA O ON O.KOD_NMA = G.KOD_NMA_MJ
                                INNER JOIN UKAZ_OWNER.KL_OBS p ON p.KOD_OBS = o.KOD_OBS

                                INNER JOIN VS_OWNER.KLV_ZVA h ON h.ZVA_KOD = g.KOD_ZVA                               
                                INNER JOIN PMIS_ADM.MilitaryRankCategories i ON i.ZVA_KAT_KOD = h.ZVA_KAT_KOD                            
    
                                LEFT OUTER JOIN UKAZ_OWNER.Districts dis ON dis.DistrictID = g.PermAddrDistrictID
                                " + whereClause + @"
                                GROUP BY p.IME_OBS, dis.DistrictName
                                ORDER BY p.IME_OBS, dis.DistrictName
                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();
               

                while (dr.Read())
                {
                    ReportPositionDeliveryAnalyzeBlock block = new ReportPositionDeliveryAnalyzeBlock();

                    block.MuniciplaityName = dr["MunicipalityName"].ToString();
                    block.DistrictName = dr["DistrictName"].ToString();
                    
                    block.FulfiledOfficers = int.Parse(dr["FulfiledOfficers"].ToString());
                    block.FulfiledSergeants = int.Parse(dr["FulfiledSergeants"].ToString());
                    block.FulfiledSoldiers = int.Parse(dr["FulfiledSoldiers"].ToString());
                    block.FulfiledOffCand = int.Parse(dr["FulfiledOffCand"].ToString());
                    
                    block.ReserveOfficers = int.Parse(dr["ReserveOfficers"].ToString());
                    block.ReserveSergeants = int.Parse(dr["ReserveSergeants"].ToString());
                    block.ReserveSoldiers = int.Parse(dr["ReserveSoldiers"].ToString());
                    block.ReserveOffCand = int.Parse(dr["ReserveOffCand"].ToString());

                    reportResult.Add(block);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return reportResult;
        }

        public static List<ReportPositionFulfilAnalyzeBlock> GetReportPositionFulfilAnalyze(ReportAnalyzeCommandFilter filter, User currentUser)
        {
            List<ReportPositionFulfilAnalyzeBlock> reportResult = new List<ReportPositionFulfilAnalyzeBlock>();

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

                if (filter.MilitaryReadinessID > -1)
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " b.milreadinessid = " + filter.MilitaryReadinessID.ToString();                    
                }

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

                whereClause = (whereClause == "" ? "" : " WHERE ") + whereClause;

                string SQL = @"                               
                                SELECT 	c.Position, 
		                                SUM ( CASE WHEN z.Age <= 35 THEN 1 ELSE 0 END) as AgeUnder35,
		                                SUM ( CASE WHEN z.Age > 35  AND z.Age <= 45 THEN 1 ELSE 0 END) as AgeUnder45,
                                        SUM ( CASE WHEN z.Age > 45 THEN 1 ELSE 0 END) as AgeAbove45, 
                                        SUM ( CASE WHEN g.OBR_VISHE = 'Y' THEN 1 ELSE 0 END) as VisheEducation,
                                        SUM ( CASE WHEN y.OBRG_KOD = '4' AND NVL(g.OBR_VISHE, '_') <> 'Y' THEN 1 ELSE 0 END) as PoluvisheEducation,
                                        SUM ( CASE WHEN g.OBR_SR = 'Y' AND NVL(y.OBRG_KOD, '_') <> '4' AND NVL(g.OBR_VISHE, '_') <> 'Y' THEN 1 ELSE 0 END) as SrednoEducation,
                                        SUM ( CASE WHEN g.OBR_VA = 'Y' THEN 1 ELSE 0 END) as VAEducation,
                                        SUM ( CASE WHEN g.OBR_VU = 'Y' THEN 1 ELSE 0 END) as VUEducation,
                                        SUM ( CASE WHEN g.OBR_SHZO = 'Y' THEN 1 ELSE 0 END) as SZHOEducation, 
                                        SUM ( CASE WHEN NVL(g.OBR_VA, 'N') = 'N' AND NVL(g.OBR_VU, 'N') = 'N' AND NVL(g.OBR_SHZO, 'N') = 'N' THEN 1 ELSE 0 END) as NoMilEducation, 
                                        SUM ( CASE WHEN NVL(d.NeedCourse, 0) = 1 THEN 1 ELSE 0 END) as NeedCourse,
                                        SUM ( CASE WHEN NVL(h.MilitaryTraining, 0) = 1 THEN 1 ELSE 0 END) as MilitaryTraining
                                FROM UKAZ_OWNER.VVR a
                                INNER JOIN PMIS_RES.REQUESTSCOMMANDS b ON b.MilitaryCommandId = a.KOD_VVR
                                INNER JOIN PMIS_RES.REQUESTCOMMANDPOSITIONS c ON c.RequestsCommandID = b.RequestsCommandID
                                INNER JOIN PMIS_RES.FILLRESERVISTSREQUEST d ON d.RequestCommandPositionId = c.RequestCommandPositionId AND d.ReservistReadinessID = 1 AND NVL(d.AppointmentIsDelivered, 0) = 1
                                INNER JOIN PMIS_RES.RESERVISTS f ON f.ReservistId = d.ReservistId
                                INNER JOIN VS_OWNER.VS_LS g ON g.PersonId = f.PersonId
                                INNER JOIN PMIS_ADM.Persons h ON h.PersonId = f.PersonId
                                LEFT OUTER JOIN (SELECT OBRG_EGNLS, MIN(OBRG_KOD) as OBRG_KOD FROM VS_OWNER.VS_OBRG GROUP BY OBRG_EGNLS) y ON g.EGN = y.OBRG_EGNLS
                                INNER JOIN (SELECT EGN as EGN, PMIS_ADM.COMMONFUNCTIONS.GetAgeFromEGN(EGN) as Age FROM VS_OWNER.VS_LS) z ON z.EGN = g.EGN
                                " + whereClause + @"
                                GROUP BY c.Position
                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();


                while (dr.Read())
                {
                    ReportPositionFulfilAnalyzeBlock block = new ReportPositionFulfilAnalyzeBlock();

                    block.Position = dr["Position"].ToString();
                    block.AgeUnder35 = int.Parse(dr["AgeUnder35"].ToString());
                    block.AgeUnder45 = int.Parse(dr["AgeUnder45"].ToString());
                    block.AgeAbove45 = int.Parse(dr["AgeAbove45"].ToString());
                    block.VisheEducation = int.Parse(dr["VisheEducation"].ToString());
                    block.PoluvisheEducation = int.Parse(dr["PoluvisheEducation"].ToString());
                    block.SrednoEducation = int.Parse(dr["SrednoEducation"].ToString());
                    block.VAEducation = int.Parse(dr["VAEducation"].ToString());
                    block.VUEducation = int.Parse(dr["VUEducation"].ToString());
                    block.SZHOEducation = int.Parse(dr["SZHOEducation"].ToString());
                    block.NoMilEducation = int.Parse(dr["NoMilEducation"].ToString());
                    block.NeedCourse = int.Parse(dr["NeedCourse"].ToString());
                    block.MilitaryTraining = int.Parse(dr["MilitaryTraining"].ToString());

                    reportResult.Add(block);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return reportResult;
        }

        public static List<ReportMilRepSpecAndPositionAnalyzeCommandBlock> GetReportMilRepSpecAndPositionAnalyzeCommand(ReportAnalyzeCommandFilter filter, User currentUser)
        {
            List<ReportMilRepSpecAndPositionAnalyzeCommandBlock> reportResult = new List<ReportMilRepSpecAndPositionAnalyzeCommandBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string whereClause = "";
                string whereClause2 = "";

                if (!String.IsNullOrEmpty(filter.MilitaryDepartmentIds))
                {
                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                                    " x.MilitaryDepartmentId IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryDepartmentIds) + ") ";

                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " d.MilitaryDepartmentId IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryDepartmentIds) + ") ";
                }

                whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                         @" (x.MilitaryDepartmentId IS NULL OR x.MilitaryDepartmentId IN ( " + currentUser.MilitaryDepartmentIDs + ")) ";

                whereClause += (whereClause == "" ? "" : " AND ") +
                        @" (d.MilitaryDepartmentId IS NULL OR d.MilitaryDepartmentId IN ( " + currentUser.MilitaryDepartmentIDs + ")) ";

                if (filter.MilitaryReadinessID > -1)
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " b.milreadinessid = " + filter.MilitaryReadinessID.ToString();

                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                                    " b.milreadinessid = " + filter.MilitaryReadinessID.ToString();
                }

                if (filter.MilitaryCommandId > -1)
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " b.MilitaryCommandId = " + filter.MilitaryCommandId.ToString();

                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                                    " b.MilitaryCommandId = " + filter.MilitaryCommandId.ToString();
                }

                if (!String.IsNullOrEmpty(filter.MilitaryCommandSuffix) && filter.MilitaryCommandSuffix != "-1")
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " b.MilitaryCommandSuffix = '" + filter.MilitaryCommandSuffix + "' ";

                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                                    " b.MilitaryCommandSuffix = '" + filter.MilitaryCommandSuffix + "' ";
                }

                whereClause = (whereClause == "" ? "" : " WHERE ") + whereClause;

                whereClause2 = (whereClause2 == "" ? "" : " WHERE ") + whereClause2;

                string SQL = @"
                               SELECT	RES.MilReportSpecialityID, 
		                                MRS.MilReportingSpecialityCode || ' ' || MRS.MilReportingSpecialityName as MilReportingSpecialityName, 
                                        NVL(RES.IsPrimary, 0) as IsPrimary,
                                        RES.Position,
		                                SUM(NVL(RequestOfficers, 0)) as RequestOfficers, 
		                                SUM(NVL(RequestSergeants, 0)) as RequestSergeants, 
		                                SUM(NVL(RequestSoldiers, 0)) as RequestSoldiers, 
		                                SUM(NVL(RequestOffCand, 0)) as RequestOffCand, 
		                                SUM(NVL(FulfiledOfficers, 0)) as FulfiledOfficers, 
		                                SUM(NVL(FulfiledSergeants, 0)) as FulfiledSergeants, 
		                                SUM(NVL(FulfiledSoldiers, 0)) as FulfiledSoldiers, 
		                                SUM(NVL(FulfiledOffCand, 0)) as FulfiledOffCand, 
		                                SUM(NVL(ChangesOfficers, 0)) as ChangesOfficers, 
		                                SUM(NVL(ChangesSergeants, 0)) as ChangesSergeants, 
		                                SUM(NVL(ChangesSoldiers, 0)) as ChangesSoldiers,
		                                SUM(NVL(ChangesOffCand, 0)) as ChangesOffCand,
		                                SUM(NVL(ReserveOfficers, 0)) as ReserveOfficers, 
		                                SUM(NVL(ReserveSergeants, 0)) as ReserveSergeants, 
		                                SUM(NVL(ReserveSoldiers, 0)) as ReserveSoldiers,
		                                SUM(NVL(ReserveOffCand, 0)) as ReserveOffCand 
                                FROM
                                (
                                                            SELECT  y.MilReportSpecialityID,
                                                                    y.IsPrimary,
                                                                    c.Position, 
                                                                    SUM(CASE WHEN i.MilitaryRankCategory = 2 THEN NVL(x.ReservistsCount, 0) ELSE 0 END) As RequestOfficers,
                                                                    SUM(CASE WHEN i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 1 THEN NVL(x.ReservistsCount, 0) ELSE 0 END) As RequestSergeants,
                                                                    SUM(CASE WHEN i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 2 THEN NVL(x.ReservistsCount, 0) ELSE 0 END) As RequestSoldiers,
                                                                    SUM(CASE WHEN i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 3 THEN NVL(x.ReservistsCount, 0) ELSE 0 END) As RequestOffCand,
                                                                    NULL as FulfiledOfficers,
                                                                    NULL as FulfiledSergeants,
								                                    NULL as FulfiledSoldiers,
								                                    NULL as FulfiledOffCand,
                                                                    NULL as ChangesOfficers,
                                                                    NULL as ChangesSergeants,
								                                    NULL as ChangesSoldiers,
								                                    NULL as ChangesOffCand,
								                                    NULL as ReserveOfficers,
								                                    NULL as ReserveSergeants,
							                                        NULL as ReserveSoldiers,
							                                        NULL as ReserveOffCand
                                                               FROM UKAZ_OWNER.VVR a
                                                               INNER JOIN PMIS_RES.REQUESTSCOMMANDS b ON b.MilitaryCommandId = a.KOD_VVR
                                                               INNER JOIN PMIS_RES.REQUESTCOMMANDPOSITIONS c ON c.RequestsCommandID = b.RequestsCommandID
                                                               LEFT OUTER JOIN PMIS_RES.CommandPositionMilRanks mr ON c.RequestCommandPositionID = mr.RequestCommandPositionID AND mr.IsPrimary = 1
                                                               LEFT OUTER JOIN VS_OWNER.KLV_ZVA h ON h.ZVA_KOD = mr.MilitaryRankID
                                                               LEFT OUTER JOIN PMIS_ADM.MilitaryRankCategories i ON i.ZVA_KAT_KOD = h.ZVA_KAT_KOD
                                                               LEFT OUTER JOIN PMIS_RES.RequestCommandPositionsMilDept x ON x.RequestCommandPositionID = c.RequestCommandPositionID
                                                               INNER JOIN PMIS_RES.CommandPositionMRSpecialities y ON c.RequestCommandPositionID = y.RequestCommandPositionID
                                                               " + whereClause2 + @"
                                                               GROUP BY y.MilReportSpecialityID, y.IsPrimary, c.Position

                                                               UNION ALL

                                                               SELECT   y.MilReportSpecialityID, 
                                                                        y.IsPrimary,
                                                                        c.Position,
                                                                        NULL As RequestOfficers,
                                                                        NULL As RequestSergeants,
                                                                        NULL As RequestSoldiers,
                                                                        NULL As RequestOffCand,
                                                                        SUM(CASE WHEN i.MilitaryRankCategory = 2 THEN 1 ELSE 0 END) As FulfiledOfficers,
                                                                        SUM(CASE WHEN i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 1 THEN 1 ELSE 0 END) As FulfiledSergeants,
                                                                        SUM(CASE WHEN i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 2 THEN 1 ELSE 0 END) As FulfiledSoldiers,
                                                                        SUM(CASE WHEN i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 3 THEN 1 ELSE 0 END) As FulfiledOffCand,
                                                                        NULL as ChangesOfficers,
                                                                        NULL as ChangesSergeants,
										                                NULL as ChangesSoldiers,
										                                NULL as ChangesOffCand,
                                                                        NULL as ReserveOfficers,
                                                                        NULL as ReserveSergeants,
                                                                        NULL as ReserveSoldiers,
                                                                        NULL as ReserveOffCand
                                                               FROM UKAZ_OWNER.VVR a
                                                               INNER JOIN PMIS_RES.REQUESTSCOMMANDS b ON b.MilitaryCommandId = a.KOD_VVR
                                                               INNER JOIN PMIS_RES.REQUESTCOMMANDPOSITIONS c ON c.RequestsCommandID = b.RequestsCommandID
                                                               INNER JOIN PMIS_RES.FILLRESERVISTSREQUEST d ON d.RequestCommandPositionId = c.RequestCommandPositionId AND d.ReservistReadinessID = 1 AND NVL(d.AppointmentIsDelivered, 0) = 1
                                                               INNER JOIN PMIS_RES.RESERVISTS f ON f.ReservistId = d.ReservistId
                                                               INNER JOIN VS_OWNER.VS_LS g ON g.PersonId = f.PersonId                               
                                                               INNER JOIN VS_OWNER.KLV_ZVA h ON h.ZVA_KOD = g.KOD_ZVA                               
                                                               INNER JOIN PMIS_ADM.MilitaryRankCategories i ON i.ZVA_KAT_KOD = h.ZVA_KAT_KOD
                                                               INNER JOIN PMIS_RES.CommandPositionMRSpecialities y ON c.RequestCommandPositionID = y.RequestCommandPositionID AND d.MilReportSpecialityID = y.MilReportSpecialityID
                                                               " + whereClause + @"
                                                               GROUP BY y.MilReportSpecialityID, y.IsPrimary, c.Position
                                							   
							                                   UNION ALL

                                                               SELECT   y.MilReportSpecialityID,
                                                                        y.IsPrimary,
                                                                        c.Position, 
                                                                        NULL As RequestOfficers,
                                                                        NULL As RequestSergeants,
                                                                        NULL As RequestSoldiers,
                                                                        NULL As RequestOffCand,
                                                                        NULL As FulfiledOfficers,
                                                                        NULL As FulfiledSergeants,
                                                                        NULL As FulfiledSoldiers,
                                                                        NULL As FulfiledOffCand,
                                                                        SUM(CASE WHEN i.MilitaryRankCategory = 2 AND z.PersonMilRepSpecID IS NULL THEN 1 ELSE 0 END) As ChangesOfficers,
                                                                        SUM(CASE WHEN i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 1 AND z.PersonMilRepSpecID IS NULL THEN 1 ELSE 0 END) As ChangesSergeants,
                                                                        SUM(CASE WHEN i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 2 AND z.PersonMilRepSpecID IS NULL THEN 1 ELSE 0 END) As ChangesSoldiers,
                                                                        SUM(CASE WHEN i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 3 AND z.PersonMilRepSpecID IS NULL THEN 1 ELSE 0 END) As ChangesOffCand,
                                                                        NULL as ReserveOfficers,
                                                                        NULL as ReserveSergeants,
                                                                        NULL as ReserveSoldiers,
                                                                        NULL as ReserveOffCand
                                                               FROM UKAZ_OWNER.VVR a
                                                               INNER JOIN PMIS_RES.REQUESTSCOMMANDS b ON b.MilitaryCommandId = a.KOD_VVR
                                                               INNER JOIN PMIS_RES.REQUESTCOMMANDPOSITIONS c ON c.RequestsCommandID = b.RequestsCommandID
                                                               INNER JOIN PMIS_RES.FILLRESERVISTSREQUEST d ON d.RequestCommandPositionId = c.RequestCommandPositionId AND d.ReservistReadinessID = 1 AND NVL(d.AppointmentIsDelivered, 0) = 1
                                                               INNER JOIN PMIS_RES.RESERVISTS f ON f.ReservistId = d.ReservistId
                                                               INNER JOIN VS_OWNER.VS_LS g ON g.PersonId = f.PersonId                               
                                                               INNER JOIN VS_OWNER.KLV_ZVA h ON h.ZVA_KOD = g.KOD_ZVA                               
                                                               INNER JOIN PMIS_ADM.MilitaryRankCategories i ON i.ZVA_KAT_KOD = h.ZVA_KAT_KOD
                                                               INNER JOIN PMIS_RES.CommandPositionMRSpecialities y ON c.RequestCommandPositionID = y.RequestCommandPositionID AND d.MilReportSpecialityID = y.MilReportSpecialityID
							                                   LEFT OUTER JOIN PMIS_ADM.PersonMilRepSpec z ON z.PersonID = f.PersonID AND z.MilReportSpecialityID = d.MilReportSpecialityID
                                                               " + whereClause + @"
                                                               GROUP BY y.MilReportSpecialityID, y.IsPrimary, c.Position

                                                               UNION ALL

                                                               SELECT  y.MilReportSpecialityID,
                                                                       y.IsPrimary,
                                                                       c.Position,
                                                                        NULL As RequestOfficers,
                                                                        NULL As RequestSergeants,
                                                                        NULL As RequestSoldiers,
                                                                        NULL As RequestOffCand,
                                                                        NULL As FulfiledOfficers,
                                                                        NULL As FulfiledSergeants,
                                                                        NULL As FulfiledSoldiers,
                                                                        NULL As FulfiledOffCand,
                                                                        NULL as ChangesOfficers,
                                                                        NULL as ChangesSergeants,
										                                NULL as ChangesSoldiers,
										                                NULL as ChangesOffCand,
                                                                        SUM(CASE WHEN i.MilitaryRankCategory = 2 THEN 1 ELSE 0 END) as ReserveOfficers,
                                                                        SUM(CASE WHEN i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 1 THEN 1 ELSE 0 END) as ReserveSergeants,
                                                                        SUM(CASE WHEN i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 2 THEN 1 ELSE 0 END) as ReserveSoldiers,
                                                                        SUM(CASE WHEN i.MilitaryRankCategory = 1 AND i.MilitaryRankSubCategory = 3 THEN 1 ELSE 0 END) as ReserveOffCand
                                                               FROM UKAZ_OWNER.VVR a
                                                               INNER JOIN PMIS_RES.REQUESTSCOMMANDS b ON b.MilitaryCommandId = a.KOD_VVR
                                                               INNER JOIN PMIS_RES.REQUESTCOMMANDPOSITIONS c ON c.RequestsCommandID = b.RequestsCommandID
                                                               INNER JOIN PMIS_RES.FILLRESERVISTSREQUEST d ON d.RequestCommandPositionId = c.RequestCommandPositionId AND d.ReservistReadinessID = 2 AND NVL(d.AppointmentIsDelivered, 0) = 1
                                                               INNER JOIN PMIS_RES.RESERVISTS f ON f.ReservistId = d.ReservistId
                                                               INNER JOIN VS_OWNER.VS_LS g ON g.PersonId = f.PersonId                               
                                                               INNER JOIN VS_OWNER.KLV_ZVA h ON h.ZVA_KOD = g.KOD_ZVA                               
                                                               INNER JOIN PMIS_ADM.MilitaryRankCategories i ON i.ZVA_KAT_KOD = h.ZVA_KAT_KOD
                                                               INNER JOIN PMIS_RES.CommandPositionMRSpecialities y ON c.RequestCommandPositionID = y.RequestCommandPositionID AND d.MilReportSpecialityID = y.MilReportSpecialityID
                                                               " + whereClause + @"
                                                               GROUP BY y.MilReportSpecialityID, y.IsPrimary, c.Position
                                  ) RES
                                  INNER JOIN PMIS_ADM.MilitaryReportSpecialities MRS ON RES.MilReportSpecialityID = MRS.MilReportSpecialityID

                                  /*Ticket: 192
                                    The REP_SEQ subselect provide the correct order of the MilitaryReportSpecialities.
                                  */
                                                                
                                  LEFT OUTER JOIN  (
                                    SELECT a.Position,
                                           ROW_NUMBER() OVER( ORDER BY  a.zva_order DESC NULLS LAST, a.zva_kod ASC NULLS LAST, a.seq ASC) as Seq
                                    FROM (
                                    SELECT a.position, b.zva_kod, b.zva_order, a.SEQ,
                                           ROW_NUMBER() OVER(PARTITION BY a.position ORDER BY b.zva_order DESC NULLS LAST, b.zva_kod ASC NULLS LAST, a.seq ASC) as MilRankRank
                                    FROM PMIS_RES.RequestCommandPositions a
                                    LEFT OUTER JOIN PMIS_RES.CommandPositionMilRanks mr ON a.RequestCommandPositionID = mr.RequestCommandPositionID AND mr.IsPrimary = 1
                                    LEFT OUTER JOIN VS_OWNER.KLV_ZVA b ON b.zva_kod = mr.militaryrankid  
                                 
                                    ) a
                                    WHERE a.MilRankRank = 1  
                                  ) REP_SEQ ON REP_SEQ.Position = RES.Position


                                  GROUP BY RES.MilReportSpecialityID, MRS.MilReportingSpecialityCode, MRS.MilReportingSpecialityName, NVL(RES.IsPrimary, 0), RES.Position, REP_SEQ.Seq 
                                  ORDER BY REP_SEQ.Seq ASC, RES.Position, NVL(RES.IsPrimary, 0) DESC, MRS.MilReportingSpecialityCode
                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ReportMilRepSpecAndPositionAnalyzeCommandBlock block = new ReportMilRepSpecAndPositionAnalyzeCommandBlock();

                    block.MilRepSpecName = dr["MilReportingSpecialityName"].ToString();
                    block.IsPrimary = dr["IsPrimary"].ToString() == "1";
                    block.Position = dr["Position"].ToString(); ;

                    if (dr["RequestOfficers"] is decimal)
                        block.RequestOfficers = int.Parse(dr["RequestOfficers"].ToString());

                    if (dr["RequestOffCand"] is decimal)
                        block.RequestOffCand = int.Parse(dr["RequestOffCand"].ToString());

                    if (dr["RequestSergeants"] is decimal)
                        block.RequestSergeants = int.Parse(dr["RequestSergeants"].ToString());

                    if (dr["RequestSoldiers"] is decimal)
                        block.RequestSoldiers = int.Parse(dr["RequestSoldiers"].ToString());

                    if (dr["FulfiledOfficers"] is decimal)
                        block.FulfiledOfficers = int.Parse(dr["FulfiledOfficers"].ToString());

                    if (dr["FulfiledOffCand"] is decimal)
                        block.FulfiledOffCand = int.Parse(dr["FulfiledOffCand"].ToString());

                    if (dr["FulfiledSergeants"] is decimal)
                        block.FulfiledSergeants = int.Parse(dr["FulfiledSergeants"].ToString());

                    if (dr["FulfiledSoldiers"] is decimal)
                        block.FulfiledSoldiers = int.Parse(dr["FulfiledSoldiers"].ToString());

                    if (dr["ChangesOfficers"] is decimal)
                        block.ChangesOfficers = int.Parse(dr["ChangesOfficers"].ToString());

                    if (dr["ChangesOffCand"] is decimal)
                        block.ChangesOffCand = int.Parse(dr["ChangesOffCand"].ToString());

                    if (dr["ChangesSergeants"] is decimal)
                        block.ChangesSergeants = int.Parse(dr["ChangesSergeants"].ToString());

                    if (dr["ChangesSoldiers"] is decimal)
                        block.ChangesSoldiers = int.Parse(dr["ChangesSoldiers"].ToString());

                    if (dr["ReserveOfficers"] is decimal)
                        block.ReserveOfficers = int.Parse(dr["ReserveOfficers"].ToString());

                    if (dr["ReserveOffCand"] is decimal)
                        block.ReserveOffCand = int.Parse(dr["ReserveOffCand"].ToString());

                    if (dr["ReserveSergeants"] is decimal)
                        block.ReserveSergeants = int.Parse(dr["ReserveSergeants"].ToString());

                    if (dr["ReserveSoldiers"] is decimal)
                        block.ReserveSoldiers = int.Parse(dr["ReserveSoldiers"].ToString());

                    reportResult.Add(block);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return reportResult;
        }

        public static List<ReportAgeMRSAnalyzeBlock> GetReportAgeMRSAnalyze(ReportAnalyzeCommandFilter filter, User currentUser)
        {
            List<ReportAgeMRSAnalyzeBlock> reportResult = new List<ReportAgeMRSAnalyzeBlock>();

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

                if (filter.MilitaryReadinessID > -1)
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " b.milreadinessid = " + filter.MilitaryReadinessID.ToString();
                }
                                
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

                whereClause = (whereClause == "" ? "" : " WHERE ") + whereClause;

                string SQL = @"SELECT CASE WHEN z.Age <= 35 
                                           THEN 'До 35 г.' 
                                           ELSE CASE WHEN z.Age > 35 AND z.Age <= 45 
                                                     THEN 'До 45 г.' 
                                                     ELSE 'Над 45 г.'
                                                END
                                      END as Age, 
                                      d.MilReportSpecialityID,
                                      MRS.MilReportingSpecialityCode || ' ' || MRS.MilReportingSpecialityName as MilReportingSpecialityName,
                                      SUM(1) as Cnt
                               FROM UKAZ_OWNER.VVR a
                               INNER JOIN PMIS_RES.REQUESTSCOMMANDS b ON b.MilitaryCommandId = a.KOD_VVR
                               INNER JOIN PMIS_RES.REQUESTCOMMANDPOSITIONS c ON c.RequestsCommandID = b.RequestsCommandID
                               INNER JOIN PMIS_RES.FILLRESERVISTSREQUEST d ON d.RequestCommandPositionId = c.RequestCommandPositionId AND d.ReservistReadinessID = 1 AND NVL(d.AppointmentIsDelivered, 0) = 1
                               INNER JOIN PMIS_RES.RESERVISTS f ON f.ReservistId = d.ReservistId
                               INNER JOIN VS_OWNER.VS_LS g ON g.PersonId = f.PersonId
                               INNER JOIN (SELECT EGN as EGN, PMIS_ADM.COMMONFUNCTIONS.GetAgeFromEGN(EGN) as Age FROM VS_OWNER.VS_LS) z ON z.EGN = g.EGN
                               INNER JOIN PMIS_ADM.MilitaryReportSpecialities MRS ON d.MilReportSpecialityID = MRS.MilReportSpecialityID
                               " + whereClause + @"
                               GROUP BY CASE WHEN z.Age <= 35 
                                             THEN 'До 35 г.' 
                                             ELSE CASE WHEN z.Age > 35 AND z.Age <= 45 
                                                       THEN 'До 45 г.' 
                                                       ELSE 'Над 45 г.'
                                                  END
                                        END, 
                                        d.MilReportSpecialityID, MRS.MilReportingSpecialityCode || ' ' || MRS.MilReportingSpecialityName
                               ORDER BY 1, 3
                              ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                ReportAgeMRSAnalyzeBlock ageBlock = null;
                ReportAgeMRSAnalyzeMRSBlock mrsBlock = null;

                string age = "";
                string militaryReportSpecialityName = "";
                int cnt = 0;

                while (dr.Read())
                {
                    if (age != dr["Age"].ToString() && ageBlock != null)
                        reportResult.Add(ageBlock);

                    if (age != dr["Age"].ToString())
                    {
                        ageBlock = new ReportAgeMRSAnalyzeBlock();
                        ageBlock.MRS = new List<ReportAgeMRSAnalyzeMRSBlock>();
                    }

                    age = dr["Age"].ToString();
                    militaryReportSpecialityName = dr["MilReportingSpecialityName"].ToString();
                    cnt = int.Parse(dr["Cnt"].ToString());

                    ageBlock.Age = age;

                    mrsBlock = new ReportAgeMRSAnalyzeMRSBlock();
                    mrsBlock.MilitaryReportSpecilityName = militaryReportSpecialityName;
                    mrsBlock.Cnt = cnt;

                    ageBlock.MRS.Add(mrsBlock);
                }

                dr.Close();

                if (ageBlock != null)
                    reportResult.Add(ageBlock);

                //The above code is based on the GetReportPositionMRSAnalyze() code. The difference is that here instead of Position we use "Age".
                //However, it lists only the "ages" for which there is a fultilment. In this case (when the "ages" are the rows) it is not suitable to have "missing" ages
                //That is why here we are adding all the possible ages into the results (with empty data by MRS)
                //Also, we decided to put all MRSs on each row. We are leaving the By Positions table to work without change because it is more likely to have a different MRS per Position
                List<string> allPossibleAges = new List<string>();
                allPossibleAges.Add("До 35 г.");
                allPossibleAges.Add("До 45 г.");
                allPossibleAges.Add("Над 45 г.");

                List<string> allFoundMRSs = new List<string>();
                foreach (ReportAgeMRSAnalyzeBlock row in reportResult)
                    foreach (ReportAgeMRSAnalyzeMRSBlock col in row.MRS)
                        if (!allFoundMRSs.Contains(col.MilitaryReportSpecilityName))
                            allFoundMRSs.Add(col.MilitaryReportSpecilityName);

                for (int i = 0; i < allPossibleAges.Count; i++)
                {
                    age = allPossibleAges[i];
                    if (!reportResult.Any(x => x.Age == age))
                    {
                        ageBlock = new ReportAgeMRSAnalyzeBlock();
                        ageBlock.Age = age;
                        ageBlock.MRS = new List<ReportAgeMRSAnalyzeMRSBlock>();
                        reportResult.Insert(i, ageBlock);
                    }
                }

                foreach (ReportAgeMRSAnalyzeBlock row in reportResult)
                {
                    for (int i = 0; i < allFoundMRSs.Count; i++)
                    {
                        string mrs = allFoundMRSs[i];
                        if (!row.MRS.Any(x => x.MilitaryReportSpecilityName == mrs))
                        {
                            mrsBlock = new ReportAgeMRSAnalyzeMRSBlock();
                            mrsBlock.MilitaryReportSpecilityName = mrs;
                            mrsBlock.Cnt = 0;

                            row.MRS.Insert(i, mrsBlock);
                        }
                    }

                    row.MRS = row.MRS.OrderBy(x => x.MilitaryReportSpecilityName).ToList();
                }
            }
            finally
            {
                conn.Close();
            }

            return reportResult;
        }
    }
}