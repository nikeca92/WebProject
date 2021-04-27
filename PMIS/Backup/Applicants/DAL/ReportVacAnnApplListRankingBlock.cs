using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Applicants.Common
{
    public class ReportVacAnnApplListRankingBlock : BaseDbObject
    {
        private int personId;

        public int PersonId
        {
            get { return personId; }
            set { personId = value; }
        }

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

        private string position = "";

        public string Position
        {
            get { return position; }
            set { position = value; }
        }

        private List<ApplicantExamMarkBlock> marks;

        public List<ApplicantExamMarkBlock> Marks
        {
            get { return marks; }
            set { marks = value; }
        }

        public ReportVacAnnApplListRankingBlock(User user)
            : base(user)
        {

        }
    }

    public class ReportVacAnnApplListRankingBlockFilter
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

    public class ReportVacAnnApplListRankingBlockUtil
    {
        public static List<ReportVacAnnApplListRankingBlock> GetListVacAnnApplListRankingBlockSearch(ReportVacAnnApplListRankingBlockFilter filter, int rowsPerPage, User currentUser)
        {
            List<ReportVacAnnApplListRankingBlock> listVacAnnApplListParticipateBlock = new List<ReportVacAnnApplListRankingBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = " WHERE g.Status =(SELECT APPLICANTEXAMSTATUSID FROM PMIS_APPL.APPLICANTEXAMSTATUSES WHERE applicantexamstatuskey='RATED') ";

                //add special user filter - Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("APPL_REPORTS_VACANNAPPL_LIST_RANKING", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                     " b.CreatedBy = " + currentUser.UserId.ToString();

                }

                //add special user filter - for ResponsibleMilitaryUnit

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    where += (where == "" ? "" : " AND ") +
                             @" c.ResponsibleMilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @")
                              ";

                //add special user filter - for MilitaryDepartmentID
                if (!string.IsNullOrEmpty(currentUser.MilitaryDepartmentIDs))
                {
                    where += (where == "" ? " " : " AND ");
                    where += @" b.MilitaryDepartmentID IN (" + currentUser.MilitaryDepartmentIDs + @")
                            ";
                }

                if (filter.VacancyAnnounceId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " d.VacancyAnnounceID = " + filter.VacancyAnnounceId.Value + " ";
                }

                if (filter.ResponsibleMilitaryUnitId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " c.ResponsibleMilitaryUnitID = " + filter.ResponsibleMilitaryUnitId.Value + " ";
                }

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
                        orderBySQL = "h.Ime " + orderByDir + ", h.Fam";
                        break;
                    case 2:
                        orderBySQL = "h.EGN";
                        break;
                    case 3:
                        orderBySQL = "c.PositionName";
                        break;

                    default:
                        orderBySQL = "h.Ime " + orderByDir + ", h.Fam";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"
                                                  SELECT 
                                                          b.PersonID,                                
                                                          h.Ime || ' ' || h.Fam as ApplicantName,                                
                                                          h.EGN as IdentityNumber,
                                                          c.positionname as Position,              
                                                          examNames.ExamName as ExamName,
                                                          f.Mark as Mark,
                                                          f.Points as Points,
                                                          b.CreatedBy
                                                         
                                                        FROM PMIS_APPL.ApplicantPositions a
                                                       INNER JOIN PMIS_APPL.Applicants b on a.ApplicantID = b.ApplicantID
                                                       INNER JOIN PMIS_ADM.MilitaryDepartments dep ON b.MilitaryDepartmentID = dep.MilitaryDepartmentID
                                                       INNER JOIN PMIS_APPL.VacancyAnnouncePositions c on a.VacancyAnnouncePositionID = c.VacancyAnnouncePositionID 
                                                       INNER JOIN PMIS_APPL.VacancyAnnounceExams d on d.VacancyAnnounceID = c.VacancyAnnounceID
                                                       INNER JOIN PMIS_APPL.Exams examNames on examNames.ExamID = d.ApplicantExamID
                                                       LEFT OUTER JOIN PMIS_APPL.ApplicantExamMarks f ON a.ApplicantID = f.ApplicantID AND c.ResponsibleMilitaryUnitID = f.ResponsibleMilitaryUnitID AND d.VacancyAnnounceExamID = f.VacancyAnnounceExamID
                                                       LEFT OUTER JOIN PMIS_APPL.ApplicantExamStatus g ON b.ApplicantID = g.ApplicantID AND c.ResponsibleMilitaryUnitID = g.ResponsibleMilitaryUnitID AND c.VacancyAnnounceID = g.VacancyAnnounceID
                                                       INNER JOIN VS_OWNER.VS_LS h ON b.PersonID = h.PersonID                                
                                                       
                                                  " + where + @"    
                                                  ORDER BY " + orderBySQL + @", b.PersonID, c.PositionName, examNames.ExamName";


                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                ReportVacAnnApplListRankingBlock lastBlock = null;

                while (dr.Read())
                {

                    if (lastBlock != null && (lastBlock.PersonId != DBCommon.GetInt(dr["PersonID"]) || lastBlock.Position != dr["Position"].ToString()))
                    {
                        listVacAnnApplListParticipateBlock.Add(lastBlock);
                        lastBlock = null;
                    }

                    if (lastBlock == null)
                    {
                        lastBlock = new ReportVacAnnApplListRankingBlock(currentUser);

                        lastBlock.IdentityNumber = dr["IdentityNumber"].ToString();
                        lastBlock.ApplicantName = dr["ApplicantName"].ToString();
                        lastBlock.Position = dr["Position"].ToString();
                        lastBlock.PersonId = DBCommon.GetInt(dr["PersonID"]);

                        lastBlock.Marks = new List<ApplicantExamMarkBlock>();
                    }

                    ApplicantExamMarkBlock mark = new ApplicantExamMarkBlock();

                    mark.ExamName = dr["ExamName"].ToString();
                    mark.Mark = (DBCommon.IsInt(dr["Mark"]) ? (int?)DBCommon.GetInt(dr["Mark"]) : null);
                    mark.Points = (DBCommon.IsInt(dr["Points"]) ? (int?)DBCommon.GetInt(dr["Points"]) : null);
                    lastBlock.Marks.Add(mark);
                }

                if (lastBlock != null)
                    listVacAnnApplListParticipateBlock.Add(lastBlock);

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listVacAnnApplListParticipateBlock;
        }

//        // This method get Count List of Reports
//        public static int GetAllVacAnnApplListRankingBlockCount(VacAnnApplListRankingBlockFilter filter, User currentUser)
//        {
//            int applicantsCnt = 0;

//            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
//            conn.Open();

//            try
//            {
//                string where = " WHERE g.Status =(SELECT APPLICANTEXAMSTATUSID FROM PMIS_APPL.APPLICANTEXAMSTATUSES WHERE applicantexamstatuskey='RATED') ";

//                //add special user filter - Restric the user to access only his own records if this is set for the particular role
//                //add special user filter - Restric the user to access only his own records if this is set for the particular role
//                UIItem uiItem = UIItemUtil.GetUIItems("APPL_REPORTS_VACANNAPPL_LIST_RANKING", currentUser, false, currentUser.Role.RoleId, null)[0];
//                if (uiItem.AccessOnlyOwnData)
//                {
//                    where += (where == "" ? "" : " AND ") +
//                          " b.CreatedBy = " + currentUser.UserId.ToString();

//                }

//                //add special user filter - for ResponsibleMilitaryUnit
//                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
//                {
//                    where += (where == "" ? "" : " AND ") +
//                             @" c.ResponsibleMilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @") ";
//                }

//                //add special user filter - for MilitaryDepartmentID
//                if (!string.IsNullOrEmpty(currentUser.MilitaryDepartmentIDs))
//                {
//                    where += (where == "" ? "" : " AND ") +
//                            @" b.MilitaryDepartmentID IN (" + currentUser.MilitaryDepartmentIDs + @")";
//                }


//                if (filter.VacancyAnnounceId.HasValue)
//                {
//                    where += (where == "" ? "" : " AND ") +
//                             @" d.VacancyAnnounceID = " + filter.VacancyAnnounceId.Value;
//                }

//                if (filter.ResponsibleMilitaryUnitId.HasValue)
//                {
//                    where += (where == "" ? "" : " AND ") +
//                            @" c.ResponsibleMilitaryUnitID = " + filter.ResponsibleMilitaryUnitId.Value;

//                }

//                string SQL = @"  SELECT   Count(distinct(b.PersonID)) as Cnt                                  
//                               FROM PMIS_APPL.ApplicantPositions a
//                               INNER JOIN PMIS_APPL.Applicants b on a.ApplicantID = b.ApplicantID
//                               INNER JOIN PMIS_ADM.MilitaryDepartments dep ON b.MilitaryDepartmentID = dep.MilitaryDepartmentID
//                               INNER JOIN PMIS_APPL.VacancyAnnouncePositions c on a.VacancyAnnouncePositionID = c.VacancyAnnouncePositionID 
//                               INNER JOIN PMIS_APPL.VacancyAnnounceExams d on d.VacancyAnnounceID = c.VacancyAnnounceID
//                               INNER JOIN PMIS_APPL.Exams examNames on examNames.ExamID = d.ApplicantExamID
//                               LEFT OUTER JOIN PMIS_APPL.ApplicantExamMarks f ON a.ApplicantID = f.ApplicantID AND c.ResponsibleMilitaryUnitID = f.ResponsibleMilitaryUnitID AND d.VacancyAnnounceExamID = f.VacancyAnnounceExamID
//                               LEFT OUTER JOIN PMIS_APPL.ApplicantExamStatus g ON b.ApplicantID = g.ApplicantID AND c.ResponsibleMilitaryUnitID = g.ResponsibleMilitaryUnitID AND c.VacancyAnnounceID = g.VacancyAnnounceID
//                               INNER JOIN VS_OWNER.VS_LS h ON b.PersonID = h.PersonID                                 
//                                                       
//                               " + where + " ";


//                SQL = DBCommon.FixNewLines(SQL);

//                OracleCommand cmd = new OracleCommand(SQL, conn);

//                OracleDataReader dr = cmd.ExecuteReader();

//                if (dr.Read())
//                {
//                    if (DBCommon.IsInt(dr["Cnt"]))
//                        applicantsCnt = DBCommon.GetInt(dr["Cnt"]);
//                }

//                dr.Close();
//            }
//            finally
//            {
//                conn.Close();
//            }

//            return applicantsCnt;
//        }

        //This method get List of Reports

//        public static List<MilitaryUnitForVacAnn> GetListMilitaryUnitsForVacAnn(int vacancyAnnounceId, User currentUser)
//        {

//            MilitaryUnitForVacAnn militaryUnitsForVacAnn;
//            List<MilitaryUnitForVacAnn> listMilitaryUnitsForVacAnn = new List<MilitaryUnitForVacAnn>();

//            string where = "";
//            //add special user filter - for ResponsibleMilitaryUnit

//            if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
//                where += " WHERE " +
//                         @" b.ResponsibleMilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @")
//                              ";

//            where += (where == "" ? " WHERE " : " AND ") +
//                            " c.VacancyAnnounceId = " + vacancyAnnounceId + " ";

//            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
//            conn.Open();

//            try
//            {
//                string SQL = @"SELECT a.VPN || ' ' || a.IMEES AS RespMilitaryUnitName,
//                                b.ResponsibleMilitaryUnitId AS ResponsibleMilitaryUnitId
//                                FROM UKAZ_OWNER.MIR a
//                                LEFT OUTER JOIN PMIS_APPL.VACANCYANNOUNCEPOSITIONS b ON 
//                                a.Kod_Mir= b.ResponsibleMilitaryUnitId
//                                INNER JOIN PMIS_APPL.VACANCYANNOUNCES c on
//                                c.VacancyAnnounceId= b.VacancyAnnounceId"
//                               + where
//                              + @"  ORDER BY a.vpn, a.imees";

//                SQL = DBCommon.FixNewLines(SQL);

//                OracleCommand cmd = new OracleCommand(SQL, conn);

//                OracleDataReader dr = cmd.ExecuteReader();

//                while (dr.Read())
//                {
//                    militaryUnitsForVacAnn = new MilitaryUnitForVacAnn(currentUser);
//                    militaryUnitsForVacAnn.MilitaryUnitId = DBCommon.GetInt(dr["ResponsibleMilitaryUnitId"]);
//                    militaryUnitsForVacAnn.MilitaryUnitName = dr["RespMilitaryUnitName"].ToString();

//                    listMilitaryUnitsForVacAnn.Add(militaryUnitsForVacAnn);
//                }
//                dr.Close();
//            }
//            finally
//            {
//                conn.Close();
//            }

//            return listMilitaryUnitsForVacAnn;
//        }

    }

    public class MilitaryUnitForVacAnn : BaseDbObject, IDropDownItem
    {
        private int militaryUnitId;

        public int MilitaryUnitId
        {
            get { return militaryUnitId; }
            set { militaryUnitId = value; }
        }
        private string militaryUnitName;

        public string MilitaryUnitName
        {
            get { return militaryUnitName; }
            set { militaryUnitName = value; }
        }

        public MilitaryUnitForVacAnn(User currentUser)
            : base(currentUser)
        {
        }

        //IDropDownItem Members
        public string Text()
        {
            return militaryUnitName;
        }

        public string Value()
        {
            return militaryUnitId.ToString();
        }
    }
}
