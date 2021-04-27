using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;
using System.Web.UI.WebControls;

namespace PMIS.Applicants.Common
{
    public class ReportVacAnnApplDetailedBlock : BaseDbObject
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

        private string permAddress = "";

        public string PermAddress
        {
            get { return permAddress; }
            set { permAddress = value; }
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

        private string militaryUnitResponsable = "";

        public string MilitaryUnitResponsable
        {
            get { return militaryUnitResponsable; }
            set { militaryUnitResponsable = value; }
        }


        private string militaryDepartment = "";

        public string MilitaryDepartment
        {
            get { return militaryDepartment; }
            set { militaryDepartment = value; }
        }
       

        private string applicantDocumentStatus = "";

        public string ApplicantDocumentStatus
        {
            get { return applicantDocumentStatus; }
            set { applicantDocumentStatus = value; }
        }

        private string applicantStatus = "";

        public string ApplicantStatus
        {
            get { return applicantStatus; }
            set { applicantStatus = value; }
        }


        public ReportVacAnnApplDetailedBlock(User user)
            : base(user)
        {

        }
    }

    public class ReportVacAnnApplDetailedBlockFilter
    {
        private int? vacancyAnnounceId;

        public int? VacancyAnnounceId
        {
            get { return vacancyAnnounceId; }
            set { vacancyAnnounceId = value; }
        }
        private int? militaryDepartmentId;

        public int? MilitaryDepartmentId
        {
            get { return militaryDepartmentId; }
            set { militaryDepartmentId = value; }
        }

        private int? militaryUnitId;

        public int? MilitaryUnitId
        {
            get { return militaryUnitId; }
            set { militaryUnitId = value; }
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

    public class ReportVacAnnApplDetailedUtil
    {

        //This method creates and returns a ApplicantDocumentStatus object. It extracts the data from a DataReader.
        public static ReportVacAnnApplDetailedBlock ExtractReportVacAnnApplDetailedBlockFromDataReader(OracleDataReader dr, User currentUser)
        {
            ReportVacAnnApplDetailedBlock reportVacAnnApplDetailedBlock = new ReportVacAnnApplDetailedBlock(currentUser);

            reportVacAnnApplDetailedBlock.ApplicantName = dr["ApplicantName"].ToString();
            reportVacAnnApplDetailedBlock.IdentityNumber = dr["IdentityNumber"].ToString();
            reportVacAnnApplDetailedBlock.PermAddress = dr["PermAddress"].ToString();
            reportVacAnnApplDetailedBlock.MilitaryUnit = dr["MilitaryUnit"].ToString();
            reportVacAnnApplDetailedBlock.Position = dr["Position"].ToString();
            reportVacAnnApplDetailedBlock.MilitaryUnitResponsable = dr["MilitaryUnitResponsable"].ToString();
            reportVacAnnApplDetailedBlock.MilitaryDepartment = dr["MilitaryDepartment"].ToString();
            reportVacAnnApplDetailedBlock.ApplicantDocumentStatus = dr["ApplDocumentStatus"].ToString();
            reportVacAnnApplDetailedBlock.ApplicantStatus = dr["CombinedApplicantStatus"].ToString();

            return reportVacAnnApplDetailedBlock;
        }

        //This method get List of Reports
        public static List<ReportVacAnnApplDetailedBlock> GetListDetailedReportSearch(ReportVacAnnApplDetailedBlockFilter filter, int rowsPerPage, User currentUser)
        {
            ReportVacAnnApplDetailedBlock vacAnnApplDetailedReportBlock;
            List<ReportVacAnnApplDetailedBlock> listDetailedReports = new List<ReportVacAnnApplDetailedBlock>();

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
                             @" l.ResponsibleMilitaryunitId IN (" + currentUser.MilitaryUnitIDs + @")
                              ";

                //add special user filter - for MilitaryDepartmentID
                if (!string.IsNullOrEmpty(currentUser.MilitaryDepartmentIDs))
                {
                    where += (where == "" ? " " : " AND ");
                    where += @" c.MilitaryDepartmentId IN (" + currentUser.MilitaryDepartmentIDs + @")
                            ";
                }


                if (filter.VacancyAnnounceId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " l.VacancyAnnounceId = " + filter.VacancyAnnounceId.Value + " ";
                }
                else
                {
                    where += (where == "" ? "" : " AND ") +
                             " 1 = 2 ";
                }

                if (filter.MilitaryDepartmentId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " c.MilitaryDepartmentId = " + filter.MilitaryDepartmentId.Value + " ";
                }

                if (filter.MilitaryUnitId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " n.Kod_Mir = " + filter.MilitaryUnitId.Value + " ";
                }

                if (filter.ResponsibleMilitaryUnitId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " m.Kod_Mir = " + filter.ResponsibleMilitaryUnitId.Value + " ";
                }

                where = (where == "" ? "" : " AND ") + where;

                string pageWhere = "";

                if (filter.PageIndex > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE RowNumber BETWEEN (" + filter.PageIndex.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + filter.PageIndex.ToString() + @" * " + rowsPerPage.ToString() + @" ";

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
                        orderBySQL = "d.Ime " + orderByDir + ", d.Fam";
                        break;
                    case 2:
                        orderBySQL = "d.Egn";
                        break;
                    case 3:
                        orderBySQL = "PMIS_ADM.CommonFunctions.GetFullAddress(d.KOD_NMA_MJ, d.PermAddrDistrictID, d.ADRES)";
                        break;
                    case 4:
                        orderBySQL = "n.Vpn " + orderByDir + ", n.Imees";
                        break;
                    case 5:
                        orderBySQL = "l.PositionName";
                        break;
                    case 6:
                        orderBySQL = "m.Vpn " + orderByDir + ", m.Imees";
                        break;
                    case 7:
                        orderBySQL = "c.MilitaryDepartmentName";
                        break;
                    case 8:
                        orderBySQL = "e.StatusName";
                        break;
                    case 9:
                        orderBySQL = "CombinedApplicantStatus";
                        break;
                    default:
                        orderBySQL = "d.Ime " + orderByDir + ", d.Fam"; 
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                                  SELECT  d.Ime || ' ' || d.Fam as ApplicantName,
                                                          d.Egn as IdentityNumber, 
                                                          PMIS_ADM.CommonFunctions.GetFullAddress(d.KOD_NMA_MJ, d.PermAddrDistrictID, d.ADRES) as PermAddress,
                                                          n.Vpn || ' ' || n.Imees as MilitaryUnit,
                                                          l.PositionName as Position,
                                                          m.Vpn || ' ' || m.Imees as MilitaryUnitResponsable,
                                                          c.MilitaryDepartmentName as MilitaryDepartment,
                                                          e.StatusName as ApplDocumentStatus,
                                                          PMIS_APPL.APPL_Functions.GetCombinedApplicantStatus(a.ApplicantPositionID) as CombinedApplicantStatus,
                                                          b.CreatedBy as CreatedBy ,
                                                         RANK() OVER (ORDER BY " + orderBySQL + @", a.ApplicantPositionId) as RowNumber 
                                                        FROM PMIS_APPL.ApplicantPositions a
                                                        INNER JOIN PMIS_APPL.Applicants b on a.ApplicantId = b.ApplicantId
                                                        INNER JOIN PMIS_ADM.MilitaryDepartments c on  c.MilitaryDepartmentId = b.MilitaryDepartmentId
                                                        INNER JOIN VS_OWNER.vs_ls d on d.Personid = b.Personid
                                                        INNER JOIN PMIS_APPL.ApplicantPositionDocStatus e on   e.StatusId = a.ApplicantDocumentsStatusId
                                                        left outer JOIN PMIS_APPL.ApplicantPositionStatus k on   k.statusid = a.applicantstatusid
                                                        INNER JOIN PMIS_APPL.VacancyAnnouncePositions l on   l.VacancyAnnouncePositionId= a.VacancyAnnouncePositionId
                                                        INNER JOIN UKAZ_OWNER.Mir m on 
                                                        m.Kod_Mir = l.ResponsibleMilitaryunitId
                                                        INNER JOIN UKAZ_OWNER.MIR n on 
                                                        n.Kod_Mir = l.MilitaryUnitId
                                                  " + where + @"    
                                                  ORDER BY " + orderBySQL + @", a.ApplicantPositionId                             
                                               ) tmp
                                               " + pageWhere;


                //                string SQL = @"SELECT  d.Ime as FisrtName,  d.Fam as Family,
                //                                d.Egn as IdentityNumber, 
                //                                n.Vpn as MilitaryUnitVpn, n.Imees as MilitaryUnitName,
                //                                l.PositionName as PositionName,
                //                                m.Vpn as ResponsibleMilitaryUnitVpn, m.Imees as ResponsibleMilitaryUnitName,
                //                                c.MilitaryDepartmentName as MilitaryDepartmentName,
                //                                e.StatusName as ApplDocumentStatus,
                //                                k.StatusName as ApplicantStatus,
                //                                b.CreatedBy as CreatedBy 
                //                                FROM PMIS_APPL.ApplicantPositions a
                //                                INNER JOIN PMIS_APPL.Applicants b on a.ApplicantId= b.ApplicantId
                //                                INNER JOIN PMIS_ADM.MilitaryDepartments c on  c.MilitaryDepartmentId= b.MilitaryDepartmentId
                //                                INNER JOIN VS_OWNER.vs_ls d on d.Personid= b.Personid
                //                                INNER JOIN PMIS_APPL.ApplicantPositionDocStatus e on
                //                                e.StatusId= a.ApplicantDocumentsStatusId
                //                                left outer JOIN PMIS_APPL.ApplicantPositionStatus k on 
                //                                k.statusid = a.applicantstatusid
                //                                INNER JOIN PMIS_APPL.VacancyAnnouncePositions l on 
                //                                l.VacancyAnnouncePositionId= a.VacancyAnnouncePositionId
                //                                INNER JOIN UKAZ_OWNER.Mir m on 
                //                                m.Kod_Mir = l.ResponsibleMilitaryunitId
                //                                INNER JOIN UKAZ_OWNER.MIR n on 
                //                                n.Kod_Mir = l.MilitaryUnitId
                //                               " + where + @"
                //                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    vacAnnApplDetailedReportBlock = ExtractReportVacAnnApplDetailedBlockFromDataReader(dr, currentUser);

                    //BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, potencialApplicant);

                    listDetailedReports.Add(vacAnnApplDetailedReportBlock);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listDetailedReports;
        }

        //This method get Count List of Reports
        public static int GetAllDetailedReportsCount(ReportVacAnnApplDetailedBlockFilter filter, User currentUser)
        {
            int applicantsCnt = 0;

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
                             @" l.ResponsibleMilitaryunitId IN (" + currentUser.MilitaryUnitIDs + @")
                              ";

                //add special user filter - for MilitaryDepartmentID
                if (!string.IsNullOrEmpty(currentUser.MilitaryDepartmentIDs))
                {
                    where += (where == "" ? " " : " AND ");
                    where += @" c.MilitaryDepartmentId IN (" + currentUser.MilitaryDepartmentIDs + @")
                            ";
                }

                if (filter.VacancyAnnounceId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " l.VacancyAnnounceId = " + filter.VacancyAnnounceId.Value + " ";
                }

                if (filter.MilitaryDepartmentId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " c.MilitaryDepartmentId = " + filter.MilitaryDepartmentId.Value + " ";
                }

                if (filter.MilitaryUnitId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " n.Kod_Mir = " + filter.MilitaryUnitId.Value + " ";
                }

                if (filter.ResponsibleMilitaryUnitId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " m.Kod_Mir = " + filter.ResponsibleMilitaryUnitId.Value + " ";
                }

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @"SELECT  count(*) AS Cnt
                                FROM PMIS_APPL.ApplicantPositions a
                                INNER JOIN PMIS_APPL.Applicants b on a.ApplicantId= b.ApplicantId
                                INNER JOIN PMIS_ADM.MilitaryDepartments c on  c.MilitaryDepartmentId= b.MilitaryDepartmentId
                                INNER JOIN VS_OWNER.vs_ls d on d.Personid= b.Personid
                                INNER JOIN PMIS_APPL.ApplicantPositionDocStatus e on
                                e.StatusId= a.ApplicantDocumentsStatusId
                                left outer JOIN PMIS_APPL.ApplicantPositionStatus k on 
                                k.StatusId = a.ApplicantStatusId
                                INNER JOIN PMIS_APPL.VacancyAnnouncePositions l on 
                                l.VacancyAnnouncePositionId= a.VacancyAnnouncePositionId
                                INNER JOIN UKAZ_OWNER.Mir m on 
                                m.Kod_Mir = l.ResponsibleMilitaryunitId
                                INNER JOIN UKAZ_OWNER.MIR n on 
                                n.Kod_Mir = l.MilitaryUnitId
                               " + where + @"
                               ";


                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        applicantsCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return applicantsCnt;
        }

    
    }
}
