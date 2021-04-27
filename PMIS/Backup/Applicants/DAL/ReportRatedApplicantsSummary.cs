using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using System.Web.UI.WebControls;

using PMIS.Common;

namespace PMIS.Applicants.Common
{
    public class ReportRatedApplicantsSummaryBlock : BaseDbObject
    {
        public string DisplayText { get; set; }
        public int RowType { get; set; }
        public int ClassA_M_Cnt { get; set; }
        public int ClassA_F_Cnt { get; set; }
        public int ClassA_Total_Cnt { get; set; }
        public int ClassB_M_Cnt { get; set; }
        public int ClassB_F_Cnt { get; set; }
        public int ClassB_Total_Cnt { get; set; }
        public int ClassC_M_Cnt { get; set; }
        public int ClassC_F_Cnt { get; set; }
        public int ClassC_Total_Cnt { get; set; }
        public int Total_M_Cnt { get; set; }
        public int Total_F_Cnt { get; set; }
        public int Total_Total_Cnt { get; set; }
        public int WithoutMilitary_M_Cnt { get; set; }
        public int WithoutMilitary_F_Cnt { get; set; }
        public int WithoutMilitary_Total_Cnt { get; set; }

        public ReportRatedApplicantsSummaryBlock(User user)
            : base(user)
        {

        }
    }

    public class ReportRatedApplicantsSummaryFilter
    {
        public int? VacancyAnnounceId { get; set; }
        public int? MilitaryUnitId { get; set; }
        public string Position { get; set; }
        public int? MilitaryDepartmentId { get; set; }
        public string Status { get; set; }

        public int PageIndex { get; set; }
        public int PageCount { get; set; }
    }

    public class ReportRatedApplicantsSummaryUtil
    {
        public static ReportRatedApplicantsSummaryBlock ExtractReportRatedApplicantsSummaryBlockFromDataReader(OracleDataReader dr, User currentUser)
        {
            ReportRatedApplicantsSummaryBlock reportRatedApplicantsSummaryBlock = new ReportRatedApplicantsSummaryBlock(currentUser);

            reportRatedApplicantsSummaryBlock.DisplayText = dr["DisplayText"].ToString();
            reportRatedApplicantsSummaryBlock.RowType = DBCommon.GetInt(dr["RowType"]);
            reportRatedApplicantsSummaryBlock.ClassA_M_Cnt = DBCommon.GetInt(dr["ClassA_M_Cnt"]);
            reportRatedApplicantsSummaryBlock.ClassA_F_Cnt = DBCommon.GetInt(dr["ClassA_F_Cnt"]);
            reportRatedApplicantsSummaryBlock.ClassA_Total_Cnt = DBCommon.GetInt(dr["ClassA_Total_Cnt"]);
            reportRatedApplicantsSummaryBlock.ClassB_M_Cnt = DBCommon.GetInt(dr["ClassB_M_Cnt"]);
            reportRatedApplicantsSummaryBlock.ClassB_F_Cnt = DBCommon.GetInt(dr["ClassB_F_Cnt"]);
            reportRatedApplicantsSummaryBlock.ClassB_Total_Cnt = DBCommon.GetInt(dr["ClassB_Total_Cnt"]);
            reportRatedApplicantsSummaryBlock.ClassC_M_Cnt = DBCommon.GetInt(dr["ClassC_M_Cnt"]);
            reportRatedApplicantsSummaryBlock.ClassC_F_Cnt = DBCommon.GetInt(dr["ClassC_F_Cnt"]);
            reportRatedApplicantsSummaryBlock.ClassC_Total_Cnt = DBCommon.GetInt(dr["ClassC_Total_Cnt"]);
            reportRatedApplicantsSummaryBlock.Total_M_Cnt = DBCommon.GetInt(dr["Total_M_Cnt"]);
            reportRatedApplicantsSummaryBlock.Total_F_Cnt = DBCommon.GetInt(dr["Total_F_Cnt"]);
            reportRatedApplicantsSummaryBlock.Total_Total_Cnt = DBCommon.GetInt(dr["Total_Total_Cnt"]);
            reportRatedApplicantsSummaryBlock.WithoutMilitary_M_Cnt = DBCommon.GetInt(dr["WithoutMilitary_M_Cnt"]);
            reportRatedApplicantsSummaryBlock.WithoutMilitary_F_Cnt = DBCommon.GetInt(dr["WithoutMilitary_F_Cnt"]);
            reportRatedApplicantsSummaryBlock.WithoutMilitary_Total_Cnt = DBCommon.GetInt(dr["WithoutMilitary_Total_Cnt"]);

            return reportRatedApplicantsSummaryBlock;
        }

        //This method get List of Reports
        public static List<ReportRatedApplicantsSummaryBlock> GetReportRatedApplicantsSummarySearch(ReportRatedApplicantsSummaryFilter filter, int rowsPerPage, User currentUser)
        {
            ReportRatedApplicantsSummaryBlock block;
            List<ReportRatedApplicantsSummaryBlock> listBlocks = new List<ReportRatedApplicantsSummaryBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //add special user filter - Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("APPL_REPORTS_REPORT_RATED_APPLICANTS_SUMMARY", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.CreatedBy = " + currentUser.UserId.ToString();
                }

                //add special user filter - for ResponsibleMilitaryUnit

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    where += (where == "" ? "" : " AND ") +
                             @" d.ResponsibleMilitaryunitId IN (" + currentUser.MilitaryUnitIDs + @")
                              ";

                //add special user filter - for MilitaryDepartmentID
                if (!string.IsNullOrEmpty(currentUser.MilitaryDepartmentIDs))
                {
                    where += (where == "" ? " " : " AND ");
                    where += @" a.MilitaryDepartmentId IN (" + currentUser.MilitaryDepartmentIDs + @")
                            ";
                }


                if (filter.VacancyAnnounceId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " d.VacancyAnnounceId = " + filter.VacancyAnnounceId.Value + " ";
                }
                else
                {
                    where += (where == "" ? "" : " AND ") +
                             " 1 = 2 ";
                }

                if (filter.MilitaryUnitId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " d.MilitaryUnitId = " + filter.MilitaryUnitId.Value + " ";
                }

                if (!String.IsNullOrEmpty(filter.Position))
                {
                    where += (where == "" ? "" : " AND ") +
                             " LOWER(d.PositionName) = '" + filter.Position.ToLower().Replace("'", "''") + "' ";
                }

                if (filter.MilitaryDepartmentId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MilitaryDepartmentId = " + filter.MilitaryDepartmentId.Value + " ";
                }

                if (!String.IsNullOrEmpty(filter.Status))
                {
                    where += (where == "" ? "" : " AND ") +
                             " PMIS_APPL.APPL_Functions.GetCombinedApplicantStatusKey(c.ApplicantPositionID) = '" + filter.Status.Replace("'", "''") + "' ";
                }

                where = (where == "" ? "" : " AND ") + where;

                string pageWhere = "";

                if (filter.PageIndex > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE RowNumber BETWEEN (" + filter.PageIndex.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + filter.PageIndex.ToString() + @" * " + rowsPerPage.ToString() + @" ";

                string SQL = @"SELECT * FROM (
                                                SELECT  CASE a.RowType
                                                           WHEN 2 THEN ' - ' || a.PositionName
                                                           WHEN 1 THEN a.VPN || ' ' || a.MilitaryUnitName
                                                           WHEN 3 THEN 'ОБЩО:'
                                                        END as DisplayText,
                                                        a.RowType,
                                                        SUM(CASE WHEN a.Age > 0 AND a.Age <= 35 AND LOWER(NVL(a.GenderName, 'мъж')) = 'мъж' THEN 1 ELSE 0 END) as ClassA_M_Cnt,
                                                        SUM(CASE WHEN a.Age > 0 AND a.Age <= 35 AND LOWER(a.GenderName) = 'жена' THEN 1 ELSE 0 END) as ClassA_F_Cnt,
                                                        SUM(CASE WHEN a.Age > 0 AND a.Age <= 35 THEN 1 ELSE 0 END) as ClassA_Total_Cnt,
                                                        
                                                        SUM(CASE WHEN a.Age > 35 AND a.Age <= 45 AND LOWER(NVL(a.GenderName, 'мъж')) = 'мъж' THEN 1 ELSE 0 END) as ClassB_M_Cnt,
                                                        SUM(CASE WHEN a.Age > 35 AND a.Age <= 45 AND LOWER(a.GenderName) = 'жена' THEN 1 ELSE 0 END) as ClassB_F_Cnt,
                                                        SUM(CASE WHEN a.Age > 35 AND a.Age <= 45 THEN 1 ELSE 0 END) as ClassB_Total_Cnt,
                                                        
                                                        SUM(CASE WHEN a.Age > 45 AND LOWER(NVL(a.GenderName, 'мъж')) = 'мъж' THEN 1 ELSE 0 END) as ClassC_M_Cnt,
                                                        SUM(CASE WHEN a.Age > 45 AND LOWER(a.GenderName) = 'жена' THEN 1 ELSE 0 END) as ClassC_F_Cnt,
                                                        SUM(CASE WHEN a.Age > 45 THEN 1 ELSE 0 END) as ClassC_Total_Cnt,
                                                        
                                                        SUM(CASE WHEN LOWER(NVL(a.GenderName, 'мъж')) = 'мъж' THEN 1 ELSE 0 END) as Total_M_Cnt,
                                                        SUM(CASE WHEN LOWER(a.GenderName) = 'жена' THEN 1 ELSE 0 END) as Total_F_Cnt,
                                                        SUM(1) as Total_Total_Cnt,
                                                        
                                                        SUM(CASE WHEN a.MilitaryService = 2 AND LOWER(NVL(a.GenderName, 'мъж')) = 'мъж' THEN 1 ELSE 0 END) as WithoutMilitary_M_Cnt,
                                                        SUM(CASE WHEN a.MilitaryService = 2 AND LOWER(a.GenderName) = 'жена' THEN 1 ELSE 0 END) as WithoutMilitary_F_Cnt,
                                                        SUM(CASE WHEN a.MilitaryService = 2 THEN 1 ELSE 0 END) as WithoutMilitary_Total_Cnt,
                                                        
                                                        RANK() OVER (ORDER BY a.VPN NULLS LAST, a.RowType) as RowNumber
                                                FROM
                                                (
                                                   SELECT DISTINCT d.PositionName, e.IMEES as MilitaryUnitName, e.VPN as VPN, a.ApplicantID,
                                                                   PMIS_ADM.COMMONFUNCTIONS.GetAgeFromEGN(b.EGN) as Age, g.GenderName, f.MilitaryService,
                                                                   2 as RowType
                                                   FROM PMIS_APPL.Applicants a
                                                   INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                                                   INNER JOIN PMIS_ADM.Persons f ON a.PersonID = f.PersonID
                                                   INNER JOIN PMIS_APPL.ApplicantPositions c ON a.ApplicantID = c.ApplicantID
                                                   INNER JOIN PMIS_APPL.VacancyAnnouncePositions d ON c.VacancyAnnouncePositionID = d.VacancyAnnouncePositionID
                                                   INNER JOIN UKAZ_OWNER.MIR e ON d.MilitaryUnitID = e.KOD_MIR
                                                   LEFT OUTER JOIN PMIS_ADM.Gender g ON f.GenderID = g.GenderID
                                                   WHERE 1 = 1 " + where + @"

                                                   UNION ALL

                                                   SELECT DISTINCT NULL as PositionName, e.IMEES as MilitaryUnitName, e.VPN as VPN, a.ApplicantID,
                                                                   PMIS_ADM.COMMONFUNCTIONS.GetAgeFromEGN(b.EGN) as Age, g.GenderName, f.MilitaryService,
                                                                   1 as RowType
                                                   FROM PMIS_APPL.Applicants a
                                                   INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                                                   INNER JOIN PMIS_ADM.Persons f ON a.PersonID = f.PersonID
                                                   INNER JOIN PMIS_APPL.ApplicantPositions c ON a.ApplicantID = c.ApplicantID
                                                   INNER JOIN PMIS_APPL.VacancyAnnouncePositions d ON c.VacancyAnnouncePositionID = d.VacancyAnnouncePositionID
                                                   INNER JOIN UKAZ_OWNER.MIR e ON d.MilitaryUnitID = e.KOD_MIR
                                                   LEFT OUTER JOIN PMIS_ADM.Gender g ON f.GenderID = g.GenderID
                                                   WHERE 1 = 1 " + where + @"

                                                   UNION ALL

                                                   SELECT DISTINCT NULL as PositionName, NULL as MilitaryUnitName, NULL as VPN, a.ApplicantID,
                                                                   PMIS_ADM.COMMONFUNCTIONS.GetAgeFromEGN(b.EGN) as Age, g.GenderName, f.MilitaryService,
                                                                   3 as RowType
                                                   FROM PMIS_APPL.Applicants a
                                                   INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                                                   INNER JOIN PMIS_ADM.Persons f ON a.PersonID = f.PersonID
                                                   INNER JOIN PMIS_APPL.ApplicantPositions c ON a.ApplicantID = c.ApplicantID
                                                   INNER JOIN PMIS_APPL.VacancyAnnouncePositions d ON c.VacancyAnnouncePositionID = d.VacancyAnnouncePositionID
                                                   INNER JOIN UKAZ_OWNER.MIR e ON d.MilitaryUnitID = e.KOD_MIR
                                                   LEFT OUTER JOIN PMIS_ADM.Gender g ON f.GenderID = g.GenderID
                                                   WHERE 1 = 1 " + where + @"
                                                ) a
                                                GROUP BY a.PositionName, a.MilitaryUnitName, a.VPN, a.RowType
                                                ORDER BY a.VPN NULLS LAST, a.RowType
                                             ) tmp
                                             " + pageWhere;


                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    block = ExtractReportRatedApplicantsSummaryBlockFromDataReader(dr, currentUser);
                    listBlocks.Add(block);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listBlocks;
        }

        //This method get Count List of Reports
        public static int GetAllReportRatedApplicantsSummaryCount(ReportRatedApplicantsSummaryFilter filter, User currentUser)
        {
            int recordsCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //add special user filter - Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("APPL_REPORTS_REPORT_RATED_APPLICANTS_SUMMARY", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.CreatedBy = " + currentUser.UserId.ToString();
                }

                //add special user filter - for ResponsibleMilitaryUnit

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    where += (where == "" ? "" : " AND ") +
                             @" d.ResponsibleMilitaryunitId IN (" + currentUser.MilitaryUnitIDs + @")
                              ";

                //add special user filter - for MilitaryDepartmentID
                if (!string.IsNullOrEmpty(currentUser.MilitaryDepartmentIDs))
                {
                    where += (where == "" ? " " : " AND ");
                    where += @" a.MilitaryDepartmentId IN (" + currentUser.MilitaryDepartmentIDs + @")
                            ";
                }


                if (filter.VacancyAnnounceId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " d.VacancyAnnounceId = " + filter.VacancyAnnounceId.Value + " ";
                }
                else
                {
                    where += (where == "" ? "" : " AND ") +
                             " 1 = 2 ";
                }

                if (filter.MilitaryUnitId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " d.MilitaryUnitId = " + filter.MilitaryUnitId.Value + " ";
                }

                if (!String.IsNullOrEmpty(filter.Position))
                {
                    where += (where == "" ? "" : " AND ") +
                             " LOWER(d.PositionName) = '" + filter.Position.ToLower().Replace("'", "''") + "' ";
                }

                if (filter.MilitaryDepartmentId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MilitaryDepartmentId = " + filter.MilitaryDepartmentId.Value + " ";
                }

                if (!String.IsNullOrEmpty(filter.Status))
                {
                    where += (where == "" ? "" : " AND ") +
                             " PMIS_APPL.APPL_Functions.GetCombinedApplicantStatusKey(c.ApplicantPositionID) = '" + filter.Status.Replace("'", "''") + "' ";
                }

                where = (where == "" ? "" : " AND ") + where;

                string SQL = @"SELECT COUNT(*) AS Cnt
                               FROM
                               (
                                   SELECT * FROM
                                   (
                                       SELECT DISTINCT d.PositionName, e.IMEES as MilitaryUnitName, e.VPN as VPN, a.ApplicantID,
                                                       PMIS_ADM.COMMONFUNCTIONS.GetAgeFromEGN(b.EGN) as Age, g.GenderName, f.MilitaryService,
                                                       2 as RowType
                                       FROM PMIS_APPL.Applicants a
                                       INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                                       INNER JOIN PMIS_ADM.Persons f ON a.PersonID = f.PersonID
                                       INNER JOIN PMIS_APPL.ApplicantPositions c ON a.ApplicantID = c.ApplicantID
                                       INNER JOIN PMIS_APPL.VacancyAnnouncePositions d ON c.VacancyAnnouncePositionID = d.VacancyAnnouncePositionID
                                       INNER JOIN UKAZ_OWNER.MIR e ON d.MilitaryUnitID = e.KOD_MIR
                                       LEFT OUTER JOIN PMIS_ADM.Gender g ON f.GenderID = g.GenderID
                                       WHERE 1 = 1 " + where + @"

                                       UNION ALL

                                       SELECT DISTINCT NULL as PositionName, e.IMEES as MilitaryUnitName, e.VPN as VPN, a.ApplicantID,
                                                       PMIS_ADM.COMMONFUNCTIONS.GetAgeFromEGN(b.EGN) as Age, g.GenderName, f.MilitaryService,
                                                       1 as RowType
                                       FROM PMIS_APPL.Applicants a
                                       INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                                       INNER JOIN PMIS_ADM.Persons f ON a.PersonID = f.PersonID
                                       INNER JOIN PMIS_APPL.ApplicantPositions c ON a.ApplicantID = c.ApplicantID
                                       INNER JOIN PMIS_APPL.VacancyAnnouncePositions d ON c.VacancyAnnouncePositionID = d.VacancyAnnouncePositionID
                                       INNER JOIN UKAZ_OWNER.MIR e ON d.MilitaryUnitID = e.KOD_MIR
                                       LEFT OUTER JOIN PMIS_ADM.Gender g ON f.GenderID = g.GenderID
                                       WHERE 1 = 1 " + where + @"

                                       UNION ALL

                                       SELECT DISTINCT NULL as PositionName, NULL as MilitaryUnitName, NULL as VPN, a.ApplicantID,
                                                       PMIS_ADM.COMMONFUNCTIONS.GetAgeFromEGN(b.EGN) as Age, g.GenderName, f.MilitaryService,
                                                       3 as RowType
                                       FROM PMIS_APPL.Applicants a
                                       INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                                       INNER JOIN PMIS_ADM.Persons f ON a.PersonID = f.PersonID
                                       INNER JOIN PMIS_APPL.ApplicantPositions c ON a.ApplicantID = c.ApplicantID
                                       INNER JOIN PMIS_APPL.VacancyAnnouncePositions d ON c.VacancyAnnouncePositionID = d.VacancyAnnouncePositionID
                                       INNER JOIN UKAZ_OWNER.MIR e ON d.MilitaryUnitID = e.KOD_MIR
                                       LEFT OUTER JOIN PMIS_ADM.Gender g ON f.GenderID = g.GenderID
                                       WHERE 1 = 1 " + where + @"
                                   ) a
                                   GROUP BY a.PositionName, a.MilitaryUnitName, a.VPN, a.RowType
                               ) b
                               ";


                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        recordsCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return recordsCnt;
        }
    }
}
