using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Applicants.Common
{
    public class ReportVacAnnApplListParticipateBlock : BaseDbObject
    {
        private string identityNumber = "";

        public string IdentityNumber
        {
            get { return identityNumber; }
            set { identityNumber = value; }
        }

        private string applicantName = "";

        public string ApplicantName
        {
            get { return applicantName; }
            set { applicantName = value; }
        }

        private string address = "";

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        private string militaryDepartment = "";

        public string MilitaryDepartment
        {
            get { return militaryDepartment; }
            set { militaryDepartment = value; }
        }

        private string remark = "";

        public string Remark
        {
            get { return remark; }
            set { remark = value; }
        }

        public ReportVacAnnApplListParticipateBlock(User user)
            : base(user)
        {

        }

    }

    public class ReportVacAnnApplListParticipateBlockFilter
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


        private ApplicantExamStatus applicantExamStatus = null;

        public ApplicantExamStatus ApplicantExamStatus
        {
            get { return applicantExamStatus; }
            set { applicantExamStatus = value; }
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

    public class ReportVacAnnApplListParticipateBlockUtil
    {
        //This method creates and returns a ApplicantDocumentStatus object. It extracts the data from a DataReader.
        public static ReportVacAnnApplListParticipateBlock ExtractVacAnnApplListParticipateBlockFromDataReader(OracleDataReader dr, User currentUser)
        {
            ReportVacAnnApplListParticipateBlock vacAnnApplListParticipateBlock = new ReportVacAnnApplListParticipateBlock(currentUser);

            vacAnnApplListParticipateBlock.IdentityNumber = dr["IdentityNumber"].ToString();
            vacAnnApplListParticipateBlock.ApplicantName = dr["ApplicantName"].ToString();
            vacAnnApplListParticipateBlock.Address = dr["Address"].ToString();
            vacAnnApplListParticipateBlock.MilitaryDepartment = dr["MilitaryDepartment"].ToString();

            if (dr["ExamStatusKey"].ToString() == "NOTRATED")
            {
                vacAnnApplListParticipateBlock.Remark = dr["ExamSatusName"].ToString();
            }
            else if (dr["PositionStatusKey"].ToString() == "DOCUMENTSREJECTED")  
            {
                vacAnnApplListParticipateBlock.Remark = dr["PositionStatusName"].ToString();
            }
           

            //  vacAnnApplListParticipateBlock.Remark = "ApplStatus - " + dr["ApplStatus"].ToString() + " ExamStatus -" + dr["ExamStatus"] + " ResponceMilitary - " + dr["ResponceMilitary"].ToString(); 

            return vacAnnApplListParticipateBlock;
        }

        //This method get List of Reports
        public static List<ReportVacAnnApplListParticipateBlock> GetListVacAnnApplListParticipateBlockSearch(ReportVacAnnApplListParticipateBlockFilter filter, int rowsPerPage, User currentUser)
        {
            ReportVacAnnApplListParticipateBlock vacAnnApplListParticipateBlock;
            List<ReportVacAnnApplListParticipateBlock> listVacAnnApplListParticipateBlock = new List<ReportVacAnnApplListParticipateBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //add special filter - Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("APPL_REPORTS_VACANNAPPL_LIST_PARTICIPATE", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.CreatedBy = " + currentUser.UserId.ToString();
                }

                //add special filter - for ResponsibleMilitaryUnit

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    where += (where == "" ? "" : " AND ") +
                             @" l.ResponsibleMilitaryunitId IN (" + currentUser.MilitaryUnitIDs + @")
                              ";

                //add special filter - for MilitaryDepartmentID
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

                if (filter.ResponsibleMilitaryUnitId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " l.ResponsibleMilitaryunitId = " + filter.ResponsibleMilitaryUnitId.Value + " ";
                }

                if (filter.ApplicantExamStatus == null)
                {
                    //We are search in both criteria

                    List<ApplicantExamStatus> listCustomItem = GetCustomListStatuses(currentUser);

                    where += (where == "" ? "" : " AND ") +
          " ( t.ApplicantExamStatusId = " + listCustomItem[0].StatusId + " OR " + " k.StatusId = " + listCustomItem[1].StatusId + " ) ";
                }
                else
                {
                    // We are search only in one of them
                    ApplicantExamStatus applicantExamStatus = filter.ApplicantExamStatus;

                    if (filter.ApplicantExamStatus.StatusKey == "NOTRATED")
                    {
                        where += (where == "" ? "" : " AND ") +
            " t.ApplicantExamStatusId= " + filter.ApplicantExamStatus.StatusId + " ";
                    }
                    else
                    {
                        //filter.ApplicantExamStatus.StatusKey ==  "DOCUMENTSREJECTED"

                        where += (where == "" ? "" : " AND ") +
          "  k.StatusId = " + filter.ApplicantExamStatus.StatusId + " ";

                    }
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
                        orderBySQL = "d.Egn";
                        break;
                    case 2:
                        orderBySQL = "d.Ime " + orderByDir + ", d.Fam";
                        break;
                    case 3:
                        orderBySQL = "d.adres";
                        break;
                    case 4:
                        orderBySQL = "c.MilitaryDepartmentName";
                        break;
                    case 5:
                        break;
                    default:
                        orderBySQL = "d.Ime " + orderByDir + ", d.Fam";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                //                string SQL = @"SELECT * FROM (
                //                                                  SELECT 
                //
                //k.StatusId || ' ' ||   k.statusname as ApplStatus,
                //m.vpn || ' ' ||    m.imees as ResponceMilitary,
                // t.APPLICANTEXAMSTATUSID || ' ' ||   t.APPLICANTEXAMSTATUSNAME   as ExamStatus,
                //
                //                                                        d.Egn as IdentityNumber, 
                //                                                        d.Ime || ' ' || d.Fam as ApplicantName,
                //                                                        p.pk || ' ' || p.ime_nma || ' ' || d.adres as Address,
                //                                                        c.MilitaryDepartmentName as MilitaryDepartment,
                //                                                        RANK() OVER (ORDER BY " + orderBySQL + @", a.ApplicantPositionId) as RowNumber 
                //                                                        FROM PMIS_APPL.ApplicantPositions a
                //                                                        INNER JOIN PMIS_APPL.Applicants b on a.ApplicantId= b.ApplicantId
                //                                                        INNER JOIN PMIS_ADM.MilitaryDepartments c on  c.MilitaryDepartmentId= b.MilitaryDepartmentId
                //                                                        INNER JOIN VS_OWNER.vs_ls d on d.Personid= b.Personid                                                       
                //                                                        INNER JOIN PMIS_APPL.VacancyAnnouncePositions l on 
                //                                                        l.VacancyAnnouncePositionId= a.VacancyAnnouncePositionId
                //
                //  INNER JOIN UKAZ_OWNER.Mir m on    m.Kod_Mir = l.ResponsibleMilitaryunitId
                //                                                      
                //
                //                                                        INNER JOIN UKAZ_OWNER.KL_NMA p on d.KOD_NMA_MJ= p.kod_nma
                //                                                        LEFT OUTER JOIN PMIS_APPL.ApplicantExamStatus r on r.ApplicantId= b.ApplicantId
                //                                                        LEFT OUTER JOIN PMIS_APPL.ApplicantExamStatuses t on t.ApplicantExamStatusId= r.Status                               
                //                                                        INNER JOIN PMIS_APPL.ApplicantPositionStatus k on 
                //                                                        k.StatusId = a.ApplicantStatusId
                //                                                  " + where + @"    
                //                                                  ORDER BY " + orderBySQL + @", a.ApplicantPositionId                             
                //                                               ) tmp
                //                                               " + pageWhere;


                string SQL = @"SELECT * FROM (
                                                  SELECT 
                                                        d.Egn as IdentityNumber, 
                                                        d.Ime || ' ' || d.Fam as ApplicantName,
                                                        p.pk || ' ' || p.ime_nma || ' ' || d.adres as Address,
                                                        c.MilitaryDepartmentName as MilitaryDepartment,
                                                        k.StatusKey as PositionStatusKey, k.statusname as PositionStatusName,
                                                        t.ApplicantExamStatusKey as ExamStatusKey, t.ApplicantExamStatusName as ExamSatusName, 
                                                        RANK() OVER (ORDER BY " + orderBySQL + @", a.ApplicantPositionId) as RowNumber 
                                                        FROM PMIS_APPL.ApplicantPositions a
                                                        INNER JOIN PMIS_APPL.Applicants b on a.ApplicantId= b.ApplicantId
                                                        INNER JOIN PMIS_ADM.MilitaryDepartments c on  c.MilitaryDepartmentId= b.MilitaryDepartmentId
                                                        INNER JOIN VS_OWNER.vs_ls d on d.Personid= b.Personid                                                       
                                                        INNER JOIN PMIS_APPL.VacancyAnnouncePositions l on 
                                                        l.VacancyAnnouncePositionId= a.VacancyAnnouncePositionId
                                                        INNER JOIN UKAZ_OWNER.KL_NMA p on d.KOD_NMA_MJ= p.kod_nma
                                                        LEFT OUTER JOIN PMIS_APPL.ApplicantExamStatus r on r.ApplicantId= b.ApplicantId
                                                        LEFT OUTER JOIN PMIS_APPL.ApplicantExamStatuses t on t.ApplicantExamStatusId= r.Status                               
                                                        INNER JOIN PMIS_APPL.ApplicantPositionStatus k on 
                                                        k.StatusId = a.ApplicantStatusId
                                                  " + where + @"    
                                                  ORDER BY " + orderBySQL + @", a.ApplicantPositionId                             
                                               ) tmp
                                               " + pageWhere;


                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    vacAnnApplListParticipateBlock = ExtractVacAnnApplListParticipateBlockFromDataReader(dr, currentUser);

                    //BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, potencialApplicant);

                    listVacAnnApplListParticipateBlock.Add(vacAnnApplListParticipateBlock);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listVacAnnApplListParticipateBlock;
        }

        //This method get Count List of Reports
        public static int GetAllVacAnnApplListParticipateBlockCount(ReportVacAnnApplListParticipateBlockFilter filter, User currentUser)
        {
            int reportsCount = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //add special user filter - Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("APPL_REPORTS_VACANNAPPL_LIST_PARTICIPATE", currentUser, false, currentUser.Role.RoleId, null)[0];
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

                if (filter.ResponsibleMilitaryUnitId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " l.ResponsibleMilitaryunitId = " + filter.ResponsibleMilitaryUnitId.Value + " ";
                }

                if (filter.ApplicantExamStatus == null)
                {
                    //We are search in both criteria

                    List<ApplicantExamStatus> listCustomItem = GetCustomListStatuses(currentUser);

                    where += (where == "" ? "" : " AND ") +
          " ( t.ApplicantExamStatusId = " + listCustomItem[0].StatusId + " OR " + " k.StatusId = " + listCustomItem[1].StatusId + " ) ";
                }
                else
                {
                    // We are search only in one of them
                    ApplicantExamStatus applicantExamStatus = filter.ApplicantExamStatus;

                    if (filter.ApplicantExamStatus.StatusKey == "NOTRATED")
                    {
                        where += (where == "" ? "" : " AND ") +
          " t.ApplicantExamStatusId = " + filter.ApplicantExamStatus.StatusId + " ";
                    }
                    else
                    {
                        //filter.ApplicantExamStatus.StatusKey ==  "DOCUMENTSREJECTED"
                        where += (where == "" ? "" : " AND ") +
             " k.StatusId = " + filter.ApplicantExamStatus.StatusId + " ";
                    }
                }

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @"SELECT  count(*) AS Cnt
                                FROM PMIS_APPL.ApplicantPositions a
                                INNER JOIN PMIS_APPL.Applicants b on a.ApplicantId= b.ApplicantId
                                INNER JOIN PMIS_ADM.MilitaryDepartments c on  c.MilitaryDepartmentId= b.MilitaryDepartmentId
                                INNER JOIN VS_OWNER.vs_ls d on d.Personid= b.Personid                                                       
                                INNER JOIN PMIS_APPL.VacancyAnnouncePositions l on 
                                l.VacancyAnnouncePositionId= a.VacancyAnnouncePositionId
                                INNER JOIN UKAZ_OWNER.KL_NMA p on d.KOD_NMA_MJ= p.kod_nma
                                LEFT OUTER JOIN PMIS_APPL.ApplicantExamStatus r on r.ApplicantId= b.ApplicantId
                                LEFT OUTER JOIN PMIS_APPL.ApplicantExamStatuses t on t.ApplicantExamStatusId= r.Status                               
                                INNER JOIN PMIS_APPL.ApplicantPositionStatus k on 
                                k.StatusId = a.ApplicantStatusId
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

        //This method get List of Reports
        public static List<ApplicantExamStatus> GetCustomListStatuses(User currentUser)
        {
            List<ApplicantExamStatus> listCustomItem = new List<ApplicantExamStatus>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT APPLICANTEXAMSTATUSID,
                                APPLICANTEXAMSTATUSNAME,
                                APPLICANTEXAMSTATUSKEY
                                FROM PMIS_APPL.APPLICANTEXAMSTATUSES a
                                WHERE a.applicantexamstatuskey='NOTRATED'
                                UNION ALL
                                SELECT STATUSID, STATUSNAME, STATUSKEY FROM PMIS_APPL.APPLICANTPOSITIONSTATUS b
                                WHERE b.statuskey='DOCUMENTSREJECTED'";


                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    listCustomItem.Add(ApplicantExamStatusUtil.ExtractApplicantExamStatusFromDataReader(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listCustomItem;
        }

        public static List<MilitaryUnitForVacAnn> GetListMilitaryUnitsForVacAnn(int vacancyAnnounceId, User currentUser)
        {

            MilitaryUnitForVacAnn militaryUnitsForVacAnn;
            List<MilitaryUnitForVacAnn> listMilitaryUnitsForVacAnn = new List<MilitaryUnitForVacAnn>();

            string where = "";
            //add special user filter - for ResponsibleMilitaryUnit

            if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                where += " WHERE " +
                         @" b.ResponsibleMilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @")
                              ";

            where += (where == "" ? " WHERE " : " AND ") +
                            " b.VacancyAnnounceId = " + vacancyAnnounceId + " ";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT DISTINCT ResponsibleMilitaryUnitName, ResponsibleMilitaryUnitId FROM
                               (SELECT a.vpn || ' ' || a.IMEES AS ResponsibleMilitaryUnitName,
                                b.ResponsibleMilitaryUnitId AS ResponsibleMilitaryUnitId
                                FROM UKAZ_OWNER.MIR a
                                INNER JOIN PMIS_APPL.VACANCYANNOUNCEPOSITIONS b ON 
                                a.Kod_Mir= b.ResponsibleMilitaryUnitId"
                               + where
                              + @"  ORDER BY a.VPN, a.IMEES)";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    militaryUnitsForVacAnn = new MilitaryUnitForVacAnn(currentUser);
                    militaryUnitsForVacAnn.MilitaryUnitId = DBCommon.GetInt(dr["ResponsibleMilitaryUnitId"]);
                    militaryUnitsForVacAnn.MilitaryUnitName = dr["ResponsibleMilitaryUnitName"].ToString();

                    listMilitaryUnitsForVacAnn.Add(militaryUnitsForVacAnn);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listMilitaryUnitsForVacAnn;
        }

    }
}
