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
    public class ReportRegisterFilter
    {
        public int? MilitaryDepartmentId { get; set; }
        public string Year { get; set; }
        public int PageIndex { get; set; }
    }

    public class ReportRegisterBlock : BaseDbObject
    {
        public int RegisterNumber { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string DocumentDateString
        {
            get
            {
                if (DocumentDate.HasValue)
                {
                    return DocumentDate.Value.ToString("dd.MM.yyyyг.");
                }

                return "";
            }
        }
        public string ApplicantFullName { get; set; }
        public string OrderNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public string OrderDateString
        {
            get
            {
                if (OrderDate.HasValue)
                {
                    return OrderDate.Value.ToString("dd.MM.yyyyг.");
                }

                return "";
            }
        }
        public string VPN { get; set; }
        public string PositionName { get; set; }
        public int? SEQ { get; set; }
        public string PageCount { get; set; }
        public string Notes { get; set; }

        public ReportRegisterBlock(User user) : base(user) { }
    }

    public class ReportRegisterUtil
    {
        public static List<ReportRegisterBlock> GetReportRegisterBlock(ReportRegisterFilter filter, int rowsPerPage, User currentUser)
        {
            ReportRegisterBlock block;
            List<ReportRegisterBlock> listBlocks = new List<ReportRegisterBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                if (filter.MilitaryDepartmentId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MilitaryDepartmentID = " + filter.MilitaryDepartmentId.Value + " ";
                }
                else
                {
                    where += (where == "" ? "" : " AND ") +
                             " 1 = 2 ";
                }

                if (!String.IsNullOrEmpty(filter.Year))
                {
                    where += (where == "" ? "" : " AND ") +
                             " TO_CHAR(d.DocumentDate, 'YYYY') = " + filter.Year + " ";
                }
                else
                {
                    where += (where == "" ? "" : " AND ") +
                             " 1 = 2 ";
                }

                string pageWhere = "";

                if (filter.PageIndex > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE RowNumber BETWEEN (" + filter.PageIndex.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + filter.PageIndex.ToString() + @" * " + rowsPerPage.ToString() + @" ";

                string SQL = @"SELECT * FROM (SELECT 
                                              d.RegisterNumber, 
                                              d.DocumentDate, 
                                              f.IME || ' ' || f.FAM as ApplicantFullName, 
                                              e.OrderNum, 
                                              e.OrderDate, 
                                              g.VPN, 
                                              c.PositionName,
                                              b.SEQ, 
                                              d.PageCount, 
                                              d.Notes,
                                              DENSE_RANK() OVER (ORDER BY d.RegisterNumber) as RowNumber
                                              FROM PMIS_APPL.Applicants a
                                              INNER JOIN PMIS_APPL.ApplicantPositions b ON b.ApplicantID = a.ApplicantID
                                              INNER JOIN PMIS_APPL.VacancyAnnouncePositions c ON c.VacancyAnnouncePositionID = b.VacancyAnnouncePositionID
                                              INNER JOIN PMIS_APPL.Register d ON d.ApplicantID = a.ApplicantID AND d.VacancyAnnounceID = c.VacancyAnnounceID AND d.ResponsibleMilitaryUnitID = c.ResponsibleMilitaryUnitID
                                              INNER JOIN PMIS_APPL.VacancyAnnounces e ON e.VacancyAnnounceID = c.VacancyAnnounceID
                                              INNER JOIN VS_OWNER.VS_LS f ON f.PersonID = a.PersonID
                                              INNER JOIN UKAZ_OWNER.MIR g ON g.KOD_MIR = c.MilitaryUnitID
                                              WHERE " + where + @"
                                             ) tmp
                                             " + pageWhere;


                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    block = ExtractReportRegisterBlockFromDataReader(dr, currentUser);
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

        public static int GetReportRegisterCount(ReportRegisterFilter filter, User currentUser)
        {
            int registerCount = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                if (filter.MilitaryDepartmentId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MilitaryDepartmentID = " + filter.MilitaryDepartmentId.Value + " ";
                }
                else
                {
                    where += (where == "" ? "" : " AND ") +
                             " 1 = 2 ";
                }

                if (!String.IsNullOrEmpty(filter.Year))
                {
                    where += (where == "" ? "" : " AND ") +
                             " TO_CHAR(d.DocumentDate, 'YYYY') = " + filter.Year + " ";
                }
                else
                {
                    where += (where == "" ? "" : " AND ") +
                             " 1 = 2 ";
                }


                string SQL = @"SELECT COUNT(DISTINCT d.RegisterNumber) as Cnt
                               FROM PMIS_APPL.Applicants a
                               INNER JOIN PMIS_APPL.ApplicantPositions b ON b.ApplicantID = a.ApplicantID
                               INNER JOIN PMIS_APPL.VacancyAnnouncePositions c ON c.VacancyAnnouncePositionID = b.VacancyAnnouncePositionID
                               INNER JOIN PMIS_APPL.Register d ON d.ApplicantID = a.ApplicantID AND d.VacancyAnnounceID = c.VacancyAnnounceID AND d.ResponsibleMilitaryUnitID = c.ResponsibleMilitaryUnitID
                               INNER JOIN PMIS_APPL.VacancyAnnounces e ON e.VacancyAnnounceID = c.VacancyAnnounceID
                               INNER JOIN VS_OWNER.VS_LS f ON f.PersonID = a.PersonID
                               INNER JOIN UKAZ_OWNER.MIR g ON g.KOD_MIR = c.MilitaryUnitID
                               WHERE " + where + @"
                              ";


                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        registerCount = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return registerCount;
        }

        public static ReportRegisterBlock ExtractReportRegisterBlockFromDataReader(OracleDataReader dr, User currentUser)
        {
            ReportRegisterBlock reportRegisterBlock = new ReportRegisterBlock(currentUser);

            reportRegisterBlock.RegisterNumber = DBCommon.GetInt(dr["RegisterNumber"]);
            if (dr["DocumentDate"] is DateTime)
                reportRegisterBlock.DocumentDate = (DateTime)dr["DocumentDate"];
            reportRegisterBlock.ApplicantFullName = dr["ApplicantFullName"].ToString();
            reportRegisterBlock.OrderNumber = dr["OrderNum"].ToString();
            if (dr["OrderDate"] is DateTime)
                reportRegisterBlock.OrderDate = (DateTime)dr["OrderDate"];
            reportRegisterBlock.VPN = dr["VPN"].ToString();
            reportRegisterBlock.PositionName = dr["PositionName"].ToString();
            reportRegisterBlock.SEQ = DBCommon.GetInt(dr["SEQ"]);
            reportRegisterBlock.PageCount = dr["PageCount"].ToString();
            reportRegisterBlock.Notes = dr["Notes"].ToString();

            return reportRegisterBlock;
        }
    }
}

