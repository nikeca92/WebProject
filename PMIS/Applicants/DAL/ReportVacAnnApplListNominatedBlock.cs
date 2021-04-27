using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;
using System.Web.UI.WebControls;
using System.Linq;

namespace PMIS.Applicants.Common
{
    public class ReportVacAnnApplListNominatedBlock : BaseDbObject
    {
        private string applicantName = "";

        public string ApplicantName
        {
            get { return applicantName; }
            set { applicantName = value; }
        }

        private string identityNumber = "";

        public string IdentityNumber
        {
            get { return identityNumber; }
            set { identityNumber = value; }
        }

        private string militaryUnit = "";

        public string MilitaryUnit
        {
            get { return militaryUnit; }
            set { militaryUnit = value; }
        }

        private string position = "";

        public string Position
        {
            get { return position; }
            set { position = value; }
        }

        private string positionCode = "";

        public string PositionCode
        {
            get { return positionCode; }
            set { positionCode = value; }
        }

        private string accessLevel = "";

        public string AccessLevel
        {
            get { return accessLevel; }
            set { accessLevel = value; }
        }

        private string totalPoints = "";

        public string TotalPoints
        {
            get { return totalPoints; }
            set { totalPoints = value; }
        }

        private string status = "";

        public string Status
        {
            get { return status; }
            set { status = value; }
        }


        public ReportVacAnnApplListNominatedBlock(User user)
            : base(user)
        {
        }
    }

    public class ReportVacAnnApplListNominatedBlockFilter
    {
        private int? vacancyAnnounceId;
        public int? VacancyAnnounceId
        {
            get { return vacancyAnnounceId; }
            set { vacancyAnnounceId = value; }
        }

        private int? responsibleMilitaryUnitId;
        public int? ResponsibleMilitaryUnitId
        {
            get { return responsibleMilitaryUnitId; }
            set { responsibleMilitaryUnitId = value; }
        }

        private int orderBy;
        public int OrderBy
        {
            get { return orderBy; }
            set { orderBy = value; }
        }

        private int pageIndex;
        public int PageIndex
        {
            get { return pageIndex; }
            set { pageIndex = value; }
        }

        private int pageCount;
        public int PageCount
        {
            get { return pageCount; }
            set { pageCount = value; }
        }

    }

    public class ReportVacAnnApplListNominatedUtil
    {

        //This method creates and returns a ApplicantDocumentStatus object. It extracts the data from a DataReader.
        public static ReportVacAnnApplListNominatedBlock ExtractVacAnnApplDetailedReportBlockFromDataReader(OracleDataReader dr, User currentUser)
        {
            ReportVacAnnApplListNominatedBlock vacAnnApplListNominatedBlock = new ReportVacAnnApplListNominatedBlock(currentUser);

            vacAnnApplListNominatedBlock.ApplicantName = dr["ApplicantName"].ToString();
            vacAnnApplListNominatedBlock.IdentityNumber = dr["IdentityNumber"].ToString();
            vacAnnApplListNominatedBlock.MilitaryUnit = dr["MilitaryUnit"].ToString();
            vacAnnApplListNominatedBlock.Position = dr["Position"].ToString();
            vacAnnApplListNominatedBlock.PositionCode = dr["PositionCode"].ToString();
            vacAnnApplListNominatedBlock.AccessLevel = dr["AccessLevel"].ToString();
            vacAnnApplListNominatedBlock.TotalPoints = dr["TotalPoints"].ToString();
            vacAnnApplListNominatedBlock.Status = dr["Status"].ToString();

            return vacAnnApplListNominatedBlock;
        }

        //This method get List of Reports
        public static List<ReportVacAnnApplListNominatedBlock> GetListDetailedReportSearch(ReportVacAnnApplListNominatedBlockFilter filter, int rowsPerPage, User currentUser)
        {
            ReportVacAnnApplListNominatedBlock vacAnnApplListNominatedBlock;
            List<ReportVacAnnApplListNominatedBlock> listVacAnnApplListNominatedBlock = new List<ReportVacAnnApplListNominatedBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //add special user filter - Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("APPL_REPORTS_VACANNAPPL_REPORT_DETAILED", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.CreatedBy = " + currentUser.UserId.ToString();
                }

                //add special user filter - for ResponsibleMilitaryUnit

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    where += (where == "" ? "" : " AND ") +
                             @" c.ResponsibleMilitaryUnitId IN (" + currentUser.MilitaryUnitIDs + @")
                              ";

                //add special user filter - for MilitaryDepartmentID
                if (!string.IsNullOrEmpty(currentUser.MilitaryDepartmentIDs))
                {
                    where += (where == "" ? " " : " AND ");
                    where += @" b.MilitaryDepartmentId IN (" + currentUser.MilitaryDepartmentIDs + @")
                            ";
                }

                //add value from filter
                if (filter.VacancyAnnounceId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " c.VacancyAnnounceId = " + filter.VacancyAnnounceId.Value + " ";
                }

                if (filter.ResponsibleMilitaryUnitId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " c.ResponsibleMilitaryUnitId = " + filter.ResponsibleMilitaryUnitId.Value + " ";
                }

                //add custom condition for our case
                where += (where == "" ? "" : " AND ") + "k.STATUSKEY in('APPOINTED', 'RESERVE') ";

                //format where
                where = (where == "" ? "" : " WHERE ") + where;

                string pageWhere = "";

                if (filter.PageIndex > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE RowNumber BETWEEN (" + filter.PageIndex.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + filter.PageIndex.ToString() + @" * " + rowsPerPage.ToString() + @" ";

                string orderBySQL = "";
                string orderByDir = " ASC ";

                if (filter.OrderBy > 100)
                {
                    filter.OrderBy -= 100;
                    orderByDir = " DESC ";
                }
                switch (filter.OrderBy)
                {
                    case 1:
                        orderBySQL = "h.Ime " + orderByDir + ", h.Fam ";
                        orderBySQL += ", (p.Points + a.Rating) DESC";
                        break;
                    case 2:
                        orderBySQL = "h.Egn " + orderByDir;
                        orderBySQL += ", (p.Points + a.Rating) DESC";
                        break;
                    case 3:
                        orderBySQL = "m.Vpn " + orderByDir + ", m.Imees " + orderByDir;
                        //add additional custom sort condition for this case
                        orderBySQL += ", c.PositionName" + orderByDir;
                        orderBySQL += ", k.StatusName" + orderByDir;
                        orderBySQL += ", (p.Points + a.Rating) DESC";

                        break;
                    case 4:
                        orderBySQL = "c.PositionName" + orderByDir;
                        orderBySQL += ", (p.Points + a.Rating) DESC";
                        break;
                    case 5:
                        orderBySQL = "c.PositionCode" + orderByDir;
                        orderBySQL += ", (p.Points + a.Rating) DESC";
                        break;
                    case 6:
                        orderBySQL = "a.ClinFormationAccLevelBg" + orderByDir;
                        orderBySQL += ", (p.Points + a.Rating) DESC";
                        break;
                    case 7:
                        orderBySQL = "(p.Points + a.Rating)" + orderByDir;
                        break;
                    case 8:
                        orderBySQL = "k.StatusName" + orderByDir;
                        orderBySQL += ", (p.Points + a.Rating) DESC";
                        break;
                    default:
                        orderBySQL = "m.Vpn " + orderByDir + ", m.Imees " + orderByDir;
                        //add additional custom sort condition for this case
                        orderBySQL += ", c.PositionName" + orderByDir;
                        orderBySQL += ", k.StatusName" + orderByDir;
                        orderBySQL += ", (p.Points + a.Rating) DESC";
                        break;
                }
                
                orderBySQL += " " + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                                  SELECT  a.ApplicantID,
                                                          h.Ime || ' ' || h.Fam as ApplicantName,                                
                                                          h.EGN as IdentityNumber,
                                                          m.vpn || ' ' || m.imees as MilitaryUnit,
                                                          c.PositionName as Position,  
                                                          c.PositionCode as PositionCode,
                                                          a.ClinFormationAccLevelBg as AccessLevel,
                                                          (p.Points + a.Rating) as TotalPoints,
                                                          k.StatusName as Status,
                                                          b.CreatedBy,
                                                         RANK() OVER (ORDER BY " + orderBySQL + @", a.ApplicantID) as RowNumber 
                                                        FROM PMIS_APPL.ApplicantPositions a
                                                       INNER JOIN PMIS_APPL.Applicants b on a.ApplicantID = b.ApplicantID
                                                       INNER JOIN PMIS_ADM.MilitaryDepartments dep ON b.MilitaryDepartmentID = dep.MilitaryDepartmentID
                                                       INNER JOIN PMIS_APPL.VacancyAnnouncePositions c on a.VacancyAnnouncePositionID = c.VacancyAnnouncePositionID 
                                                       INNER JOIN UKAZ_OWNER.Mir m on m.Kod_Mir = c.MILITARYUNITID                                               
                                                       INNER JOIN VS_OWNER.VS_LS h ON b.PersonID = h.PersonID
                                                       LEFT OUTER JOIN (
                                                       SELECT   a.ApplicantID, a.ResponsibleMilitaryUnitId, b.VacancyAnnounceId,
                                                       SUM(a.points) as Points
                                                       FROM PMIS_APPL.APPLICANTEXAMMARKS a
                                                       INNER JOIN PMIS_APPL.VACANCYANNOUNCEEXAMS b ON a.VacancyAnnounceExamId = b.VacancyAnnounceExamId
                                                       GROUP BY a.ApplicantID, a.ResponsibleMilitaryUnitId, b.VacancyAnnounceId
                                                       ) p ON p.ApplicantID = b.ApplicantID and p.ResponsibleMilitaryUnitId=c.ResponsibleMilitaryUnitId
                                                      and p.VacancyAnnounceId=c.VacancyAnnounceId
                                                      INNER JOIN PMIS_APPL.APPLICANTPOSITIONSTATUS k on k.STATUSID=a.APPLICANTSTATUSID
                                                  " + where + @"    
                                                  ORDER BY " + orderBySQL + @", a.ApplicantID                             
                                               ) tmp
                                               " + pageWhere;


                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    vacAnnApplListNominatedBlock = ExtractVacAnnApplDetailedReportBlockFromDataReader(dr, currentUser);

                    listVacAnnApplListNominatedBlock.Add(vacAnnApplListNominatedBlock);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listVacAnnApplListNominatedBlock;
        }

        //This method get Count List of Reports
        public static int GetAllDetailedReportsCount(ReportVacAnnApplListNominatedBlockFilter filter, User currentUser)
        {
            int reportsCount = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //add special user filter - Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("APPL_REPORTS_VACANNAPPL_REPORT_DETAILED", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.CreatedBy = " + currentUser.UserId.ToString();
                }

                //add special user filter - for ResponsibleMilitaryUnit

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    where += (where == "" ? "" : " AND ") +
                             @" c.ResponsibleMilitaryUnitId IN (" + currentUser.MilitaryUnitIDs + @")
                              ";

                //add special user filter - for MilitaryDepartmentID
                if (!string.IsNullOrEmpty(currentUser.MilitaryDepartmentIDs))
                {
                    where += (where == "" ? " " : " AND ");
                    where += @" b.MilitaryDepartmentId IN (" + currentUser.MilitaryDepartmentIDs + @")
                            ";
                }

                //add value from filter
                if (filter.VacancyAnnounceId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " c.VacancyAnnounceId = " + filter.VacancyAnnounceId.Value + " ";
                }

                if (filter.ResponsibleMilitaryUnitId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " c.ResponsibleMilitaryUnitId = " + filter.ResponsibleMilitaryUnitId.Value + " ";
                }

                //add custom condition for our case
                where += (where == "" ? "" : " AND ") + "k.STATUSKEY in('APPOINTED', 'RESERVE') ";

                //format where
                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @"SELECT  count(*) AS Cnt
                                 FROM PMIS_APPL.ApplicantPositions a
                                                       INNER JOIN PMIS_APPL.Applicants b on a.ApplicantID = b.ApplicantID
                                                       INNER JOIN PMIS_ADM.MilitaryDepartments dep ON b.MilitaryDepartmentID = dep.MilitaryDepartmentID
                                                       INNER JOIN PMIS_APPL.VacancyAnnouncePositions c on a.VacancyAnnouncePositionID = c.VacancyAnnouncePositionID 
                                                       INNER JOIN UKAZ_OWNER.Mir m on m.Kod_Mir = c.MILITARYUNITID                                               
                                                       INNER JOIN VS_OWNER.VS_LS h ON b.PersonID = h.PersonID
                                                       LEFT OUTER JOIN (
                                                       SELECT   a.ApplicantID, a.ResponsibleMilitaryUnitId, b.VacancyAnnounceId,
                                                       SUM(a.points) as Points
                                                       FROM PMIS_APPL.APPLICANTEXAMMARKS a
                                                       INNER JOIN PMIS_APPL.VACANCYANNOUNCEEXAMS b ON a.VacancyAnnounceExamId = b.VacancyAnnounceExamId
                                                       GROUP BY a.ApplicantID, a.ResponsibleMilitaryUnitId, b.VacancyAnnounceId
                                                       ) p ON p.ApplicantID = b.ApplicantID and p.ResponsibleMilitaryUnitId=c.ResponsibleMilitaryUnitId
                                                      and p.VacancyAnnounceId=c.VacancyAnnounceId
                                                      INNER JOIN PMIS_APPL.APPLICANTPOSITIONSTATUS k on k.STATUSID=a.APPLICANTSTATUSID
                                                      
                               " + where + @"
                               ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        reportsCount = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return reportsCount;
        }


    }
}
