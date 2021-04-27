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
    public class VacancyAnnounce : BaseDbObject, IDropDownItem
    {
        private int vacancyAnnounceId;
        private string orderNum;
        private DateTime? orderDate;
        private DateTime? endDate;
        private int? maxPositions;
        private VacancyAnnounceStatus vacancyAnnounceStatus;
        private List<Document> listDocument;
        private List<Exam> listExam;
        private int vacancyAnnounceType;

        public int VacancyAnnounceId
        {
            get
            {
                return vacancyAnnounceId;
            }
            set
            {
                vacancyAnnounceId = value;
            }
        }

        public string OrderNum
        {
            get
            {
                return orderNum;
            }
            set
            {
                orderNum = value;
            }
        }

        public DateTime? OrderDate
        {
            get
            {
                return orderDate;
            }
            set
            {
                orderDate = value;
            }
        }

        public DateTime? EndDate
        {
            get
            {
                return endDate;
            }
            set
            {
                endDate = value;
            }
        }

        public int? MaxPositions
        {
            get
            {
                return maxPositions;
            }
            set
            {
                maxPositions = value;
            }
        }

        public VacancyAnnounceStatus VacancyAnnounceStatus
        {
            get
            {
                return vacancyAnnounceStatus;
            }
            set
            {
                vacancyAnnounceStatus = value;
            }
        }

        public bool CanDelete
        {
            get
            {
                return (ApplicantPositionUtil.CountAllApplicantPositionForVacancyAnnounce(vacancyAnnounceId, CurrentUser) == 0);
            }
        }

        public List<Document> ListDocument
        {
            get
            {
                if (listDocument == null)
                {
                    listDocument = DocumentUtil.GetDocumentsForVacancyAnnounce(vacancyAnnounceId, CurrentUser);
                }
                return listDocument;
            }
        }

        public List<Exam> ListExam
        {
            get
            {
                if (listExam == null)
                {
                    listExam = ExamUtil.GetExamsForVacancyAnnounce(vacancyAnnounceId, CurrentUser);
                }
                return listExam;
            }
        }

        public string OrderNumOrderDate
        {
            get
            {
                return VacancyAnnounceUtil.OrderNumOrderDate(OrderNum, OrderDate);
            }
        }

        public int VacancyAnnounceType
        {
            get { return vacancyAnnounceType; }
            set { vacancyAnnounceType = value; }
        }

        public VacancyAnnounce(User user)
            : base(user)
        {

        }

        //IDropDownItem Members
        public string Text()
        {
            return orderNum;
        }

        public string Value()
        {
            return vacancyAnnounceId.ToString();
        }
    }

    //This class represents all information about the filter, the order and the paging information on the screen
    public class VacancyAnnouncesFilter
    {
        string orderNum;
        DateTime? orderDateFrom;
        DateTime? orderDateTo;
        string vacancyAnnounceStatuses;
        DateTime? endDateFrom;
        DateTime? endDateTo;
        int orderBy;
        int pageIdx;

        public string OrderNum
        {
            get
            {
                return orderNum;
            }
            set
            {
                orderNum = value;
            }
        }

        public DateTime? OrderDateFrom
        {
            get
            {
                return orderDateFrom;
            }
            set
            {
                orderDateFrom = value;
            }
        }

        public DateTime? OrderDateTo
        {
            get
            {
                return orderDateTo;
            }
            set
            {
                orderDateTo = value;
            }
        }

        public string VacancyAnnounceStatuses
        {
            get
            {
                return vacancyAnnounceStatuses;
            }
            set
            {
                vacancyAnnounceStatuses = value;
            }
        }

        public DateTime? EndDateFrom
        {
            get
            {
                return endDateFrom;
            }
            set
            {
                endDateFrom = value;
            }
        }

        public DateTime? EndDateTo
        {
            get
            {
                return endDateTo;
            }
            set
            {
                endDateTo = value;
            }
        }

        public int OrderBy
        {
            get
            {
                return orderBy;
            }
            set
            {
                orderBy = value;
            }
        }

        public int PageIdx
        {
            get
            {
                return pageIdx;
            }
            set
            {
                pageIdx = value;
            }
        }
    }

    public static class VacancyAnnounceUtil
    {
        public static string OrderNumOrderDate(string orderNum, DateTime? orderDate)
        {
            return orderNum + (orderDate.HasValue ? "/" + CommonFunctions.FormatDate(orderDate) : "");
        }

        //This method creates and returns a VacancyAnnounce object. It extracts the data from a DataReader.
        //This is done in this way to have a signle place of creation of this object, however, pass the particular DataReader
        //object from various queries for better performance instead of executing a new call to the DB here to get the details for a specific VacancyAnnounceID, for example.
        public static VacancyAnnounce ExtractVacancyAnnounceFromDataReader(OracleDataReader dr, User currentUser)
        {
            int? vacancyAnnounceID = null;

            if (DBCommon.IsInt(dr["VacancyAnnounceID"]))
                vacancyAnnounceID = DBCommon.GetInt(dr["VacancyAnnounceID"]);

            string orderNum = dr["OrderNum"].ToString();
            DateTime? orderDate = null;
            if (dr["OrderDate"] is DateTime)
                orderDate = (DateTime)dr["OrderDate"];
            DateTime? endDate = null;
            if (dr["EndDate"] is DateTime)
                endDate = (DateTime)dr["EndDate"];
            int? maxPositions = null;
            if (DBCommon.IsInt(dr["MaxPositions"]))
                maxPositions = DBCommon.GetInt(dr["MaxPositions"]);

            int vacAnnType;
            vacAnnType = DBCommon.GetInt(dr["VacAnnType"]);

            VacancyAnnounce vacancyAnnounce = null;

            if (vacancyAnnounceID.HasValue)
            {
                vacancyAnnounce = new VacancyAnnounce(currentUser);
                vacancyAnnounce.VacancyAnnounceId = vacancyAnnounceID.Value;
                vacancyAnnounce.OrderNum = orderNum;
                vacancyAnnounce.OrderDate = orderDate;
                vacancyAnnounce.EndDate = endDate;
                vacancyAnnounce.MaxPositions = maxPositions;
                vacancyAnnounce.VacancyAnnounceStatus = VacancyAnnounceStatusUtil.ExtractVacancyAnnounceStatusFromDataReader(dr, currentUser);
                vacancyAnnounce.VacancyAnnounceType = vacAnnType;

                BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, vacancyAnnounce);
            }

            return vacancyAnnounce;
        }

        public static VacancyAnnounce GetVacancyAnnounce(int vacancyAnnounceId, User currentUser)
        {
            VacancyAnnounce vacancyAnnounce = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("APPL_VACANN", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where = " AND a.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.VacancyAnnounceID, a.OrderNum, a.OrderDate, a.EndDate, a.MaxPositions,
                                      b.VacancyAnnouncesStatusID, b.VacAnnStatusName, b.VacAnnStatusKey, a.VacAnnType, 
                                      a.CreatedBy, a.CreatedDate, a.LastModifiedBy, a.LastModifiedDate
                               FROM PMIS_APPL.VacancyAnnounces a
                               LEFT OUTER JOIN PMIS_APPL.VacancyAnnouncesStatuses b ON a.VacAnnStatusID = b.VacancyAnnouncesStatusID
                               WHERE a.VacancyAnnounceID = :VacancyAnnounceID " + where;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VacancyAnnounceID", OracleType.Number).Value = vacancyAnnounceId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    vacancyAnnounce = VacancyAnnounceUtil.ExtractVacancyAnnounceFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vacancyAnnounce;
        }

        public static List<VacancyAnnounce> GetAllVacancyAnnounces(VacancyAnnouncesFilter filt, int rowsPerPage, User currentUser)
        {
            List<VacancyAnnounce> vacancyAnnounces = new List<VacancyAnnounce>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("APPL_VACANN", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!String.IsNullOrEmpty(filt.VacancyAnnounceStatuses))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.VacannStatusID IN (" + filt.VacancyAnnounceStatuses + @") ";
                }

                if (!String.IsNullOrEmpty(filt.OrderNum))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.OrderNum LIKE '%" + filt.OrderNum.Replace("'", "''") + @"%' ";
                }

                if (filt.OrderDateFrom.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.OrderDate >= " + DBCommon.DateToDBCode(filt.OrderDateFrom.Value) + " ";
                }

                if (filt.OrderDateFrom.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.OrderDate < " + DBCommon.DateToDBCode(filt.OrderDateTo.Value.AddDays(1)) + " ";
                }

                if (filt.EndDateFrom.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.EndDate >= " + DBCommon.DateToDBCode(filt.EndDateFrom.Value) + " ";
                }

                if (filt.EndDateTo.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.EndDate < " + DBCommon.DateToDBCode(filt.EndDateTo.Value.AddDays(1)) + " ";
                }


                where = (where == "" ? "" : " WHERE ") + where;

                string pageWhere = "";

                if (filt.PageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE RowNumber BETWEEN (" + filt.PageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + filt.PageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

                string orderBySQL = "";
                string orderByDir = "ASC";

                if (filt.OrderBy > 100)
                {
                    filt.OrderBy -= 100;
                    orderByDir = "DESC";
                }

                switch (filt.OrderBy)
                {
                    case 1:
                        orderBySQL = "a.OrderNum";
                        break;
                    case 2:
                        orderBySQL = "a.OrderDate";
                        break;
                    case 3:
                        orderBySQL = "a.EndDate";
                        break;
                    case 4:
                        orderBySQL = "a.MaxPositions";
                        break;
                    case 5:
                        orderBySQL = "b.VacAnnStatusName";
                        break;
                    default:
                        orderBySQL = "a.OrderNum";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                  SELECT a.VacancyAnnounceID, a.OrderNum, a.OrderDate, a.EndDate, a.MaxPositions, a.VacAnnType,
                                         a.CreatedBy, a.CreatedDate, a.LastModifiedBy, a.LastModifiedDate,
                                         b.VacancyAnnouncesStatusID, b.VacAnnStatusName, b.VacAnnStatusKey,
                                         RANK() OVER (ORDER BY " + orderBySQL + @", a.VacancyAnnounceID) as RowNumber 
                                  FROM PMIS_APPL.VacancyAnnounces a
                                  LEFT OUTER JOIN PMIS_APPL.VacancyAnnouncesStatuses b ON a.VacAnnStatusID = b.VacancyAnnouncesStatusID
                                  " + where + @"    
                                  ORDER BY " + orderBySQL + @", VacancyAnnounceID                             
                               ) tmp
                               " + pageWhere;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    vacancyAnnounces.Add(ExtractVacancyAnnounceFromDataReader(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vacancyAnnounces;
        }

        public static List<ListItem> GetVacancyAnnouncesListItemsForAllowance(User currentUser)
        {
            List<ListItem> vacancyAnnounces = new List<ListItem>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("APPL_APPL", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    where += (where == "" ? "" : " AND ") +
                             @" b.ResponsibleMilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @")
                              ";

                where += (where == "" ? "" : " AND ") +
                         @" (a.VacannStatusID IN (SELECT VacancyAnnouncesStatusID FROM PMIS_APPL.VacancyAnnouncesStatuses WHERE VacannStatusKey IN ('ALLOWANCE', 'EXAM', 'RANK', 'NOMINATION')))
                         ";

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @" SELECT a.VacancyAnnounceID, a.OrderNum, a.OrderDate                                       
                                FROM PMIS_APPL.VacancyAnnounces a                                                                
                                INNER JOIN PMIS_APPL.VacancyAnnouncePositions b ON a.VacancyAnnounceID = b.VacancyAnnounceID                                  
                              " + where + @"
                                GROUP BY a.VacancyAnnounceID, a.OrderNum, a.OrderDate
                              ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    int vacancyAnnounceID;
                    if (DBCommon.IsInt(dr["VacancyAnnounceID"]))
                        vacancyAnnounceID = DBCommon.GetInt(dr["VacancyAnnounceID"]);
                    else
                        continue;

                    string orderNum = dr["OrderNum"].ToString();
                    DateTime? orderDate = null;
                    if (dr["OrderDate"] is DateTime)
                        orderDate = (DateTime)dr["OrderDate"];

                    ListItem li = new ListItem();
                    li.Text = OrderNumOrderDate(orderNum, orderDate);
                    li.Value = vacancyAnnounceID.ToString();
                    vacancyAnnounces.Add(li);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vacancyAnnounces;
        }

        public static List<ListItem> GetVacancyAnnouncesListItemsForExams(User currentUser)
        {
            List<ListItem> vacancyAnnounces = new List<ListItem>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("APPL_APPL", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    where += (where == "" ? "" : " AND ") +
                             @" b.ResponsibleMilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @")
                              ";

                where += (where == "" ? "" : " AND ") +
                         @" (a.VacannStatusID IN (SELECT VacancyAnnouncesStatusID FROM PMIS_APPL.VacancyAnnouncesStatuses WHERE VacannStatusKey IN ('EXAM', 'RANK', 'NOMINATION')))
                         ";

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @" SELECT a.VacancyAnnounceID, a.OrderNum, a.OrderDate                                       
                                FROM PMIS_APPL.VacancyAnnounces a                                                                
                                INNER JOIN PMIS_APPL.VacancyAnnouncePositions b ON a.VacancyAnnounceID = b.VacancyAnnounceID                                  
                              " + where + @"
                                GROUP BY a.VacancyAnnounceID, a.OrderNum, a.OrderDate
                              ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    int vacancyAnnounceID;
                    if (DBCommon.IsInt(dr["VacancyAnnounceID"]))
                        vacancyAnnounceID = DBCommon.GetInt(dr["VacancyAnnounceID"]);
                    else
                        continue;

                    string orderNum = dr["OrderNum"].ToString();
                    DateTime? orderDate = null;
                    if (dr["OrderDate"] is DateTime)
                        orderDate = (DateTime)dr["OrderDate"];

                    ListItem li = new ListItem();
                    li.Text = OrderNumOrderDate(orderNum, orderDate);
                    li.Value = vacancyAnnounceID.ToString();
                    vacancyAnnounces.Add(li);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vacancyAnnounces;
        }

        public static List<ListItem> GetVacancyAnnouncesListItemsForRanking(User currentUser)
        {
            List<ListItem> vacancyAnnounces = new List<ListItem>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("APPL_APPL", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    where += (where == "" ? "" : " AND ") +
                             @" b.ResponsibleMilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @")
                              ";

                where += (where == "" ? "" : " AND ") +
                         @" (a.VacannStatusID IN (SELECT VacancyAnnouncesStatusID FROM PMIS_APPL.VacancyAnnouncesStatuses WHERE VacannStatusKey IN ('RANK', 'NOMINATION')))
                         ";

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @" SELECT a.VacancyAnnounceID, a.OrderNum, a.OrderDate                                       
                                FROM PMIS_APPL.VacancyAnnounces a                                                                
                                INNER JOIN PMIS_APPL.VacancyAnnouncePositions b ON a.VacancyAnnounceID = b.VacancyAnnounceID                                  
                              " + where + @"
                                GROUP BY a.VacancyAnnounceID, a.OrderNum, a.OrderDate
                              ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    int vacancyAnnounceID;
                    if (DBCommon.IsInt(dr["VacancyAnnounceID"]))
                        vacancyAnnounceID = DBCommon.GetInt(dr["VacancyAnnounceID"]);
                    else
                        continue;

                    string orderNum = dr["OrderNum"].ToString();
                    DateTime? orderDate = null;
                    if (dr["OrderDate"] is DateTime)
                        orderDate = (DateTime)dr["OrderDate"];

                    ListItem li = new ListItem();
                    li.Text = OrderNumOrderDate(orderNum, orderDate);
                    li.Value = vacancyAnnounceID.ToString();
                    vacancyAnnounces.Add(li);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vacancyAnnounces;
        }

        public static List<ListItem> GetVacancyAnnouncesListItemsForNomination(User currentUser)
        {
            List<ListItem> vacancyAnnounces = new List<ListItem>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("APPL_APPL", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    where += (where == "" ? "" : " AND ") +
                             @" b.ResponsibleMilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @")
                              ";

                where += (where == "" ? "" : " AND ") +
                         @" (a.VacannStatusID IN (SELECT VacancyAnnouncesStatusID FROM PMIS_APPL.VacancyAnnouncesStatuses WHERE VacannStatusKey IN ('NOMINATION')))
                         ";

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @" SELECT a.VacancyAnnounceID, a.OrderNum, a.OrderDate                                       
                                FROM PMIS_APPL.VacancyAnnounces a                                                                
                                INNER JOIN PMIS_APPL.VacancyAnnouncePositions b ON a.VacancyAnnounceID = b.VacancyAnnounceID                                  
                              " + where + @"
                                GROUP BY a.VacancyAnnounceID, a.OrderNum, a.OrderDate
                              ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    int vacancyAnnounceID;
                    if (DBCommon.IsInt(dr["VacancyAnnounceID"]))
                        vacancyAnnounceID = DBCommon.GetInt(dr["VacancyAnnounceID"]);
                    else
                        continue;

                    string orderNum = dr["OrderNum"].ToString();
                    DateTime? orderDate = null;
                    if (dr["OrderDate"] is DateTime)
                        orderDate = (DateTime)dr["OrderDate"];

                    ListItem li = new ListItem();
                    li.Text = OrderNumOrderDate(orderNum, orderDate);
                    li.Value = vacancyAnnounceID.ToString();
                    vacancyAnnounces.Add(li);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vacancyAnnounces;
        }

        public static int GetAllVacancyAnnouncesCount(VacancyAnnouncesFilter filt, User currentUser)
        {
            int vacancyAnnouncesCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("APPL_VACANN", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!String.IsNullOrEmpty(filt.VacancyAnnounceStatuses))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.VacannStatusID IN (" + filt.VacancyAnnounceStatuses + @") ";
                }

                if (!String.IsNullOrEmpty(filt.OrderNum))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.OrderNum LIKE '%" + filt.OrderNum.Replace("'", "''") + @"%' ";
                }

                if (filt.OrderDateFrom.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.OrderDate >= " + DBCommon.DateToDBCode(filt.OrderDateFrom.Value) + " ";
                }

                if (filt.OrderDateFrom.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.OrderDate < " + DBCommon.DateToDBCode(filt.OrderDateTo.Value.AddDays(1)) + " ";
                }

                if (filt.EndDateFrom.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.EndDate >= " + DBCommon.DateToDBCode(filt.EndDateFrom.Value) + " ";
                }

                if (filt.EndDateTo.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.EndDate < " + DBCommon.DateToDBCode(filt.EndDateTo.Value.AddDays(1)) + " ";
                }

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @"SELECT COUNT(*) as Cnt
                               FROM PMIS_APPL.VacancyAnnounces a
                               " + where + @"
                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        vacancyAnnouncesCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vacancyAnnouncesCnt;
        }

        public static List<VacancyAnnounce> GetAllVacancyAnnouncesForDDL(User currentUser)
        {
            List<VacancyAnnounce> vacancyAnnounces = new List<VacancyAnnounce>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("APPL_VACANN", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where = " a.CreatedBy = " + currentUser.UserId.ToString();
                }

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @"SELECT a.VacancyAnnounceID, a.OrderNum 
                                  FROM PMIS_APPL.VacancyAnnounces a
                                  " + where + @"    
                                  ORDER BY a.OrderNum, a.VacancyAnnounceID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    VacancyAnnounce vacancyAnnounce = new VacancyAnnounce(currentUser);

                    if (DBCommon.IsInt(dr["VacancyAnnounceID"]))
                        vacancyAnnounce.VacancyAnnounceId = DBCommon.GetInt(dr["VacancyAnnounceID"]);

                    vacancyAnnounce.OrderNum = dr["OrderNum"].ToString();

                    vacancyAnnounces.Add(vacancyAnnounce);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vacancyAnnounces;
        }

        public static List<VacancyAnnounce> GetAllVacancyAnnouncesByApplicantID(int applicantId, User currentUser)
        {
            List<VacancyAnnounce> vacancyAnnounces = new List<VacancyAnnounce>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restrict the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("APPL_VACANN", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where = " a.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                {
                    where += (where == "" ? " " : " AND ");
                    where += @" (b.MilitaryUnitID IS NULL OR b.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @"))
                            ";
                }

                where = (where == "" ? " " : " WHERE ") + where;

                string SQL = @"SELECT DISTINCT(a.VacancyAnnounceID), a.OrderNum 
                                FROM PMIS_APPL.VacancyAnnounces a
                                JOIN PMIS_APPL.VacancyAnnouncePositions b ON a.VacancyAnnounceID = b.VacancyAnnounceID
                                JOIN PMIS_APPL.ApplicantPositions c ON b.VacancyAnnouncePositionID = c.VacancyAnnouncePositionID AND c.ApplicantID = :ApplicantID"
                                 + where +
                                "ORDER BY a.OrderNum, a.VacancyAnnounceID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ApplicantID", OracleType.Number).Value = applicantId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    VacancyAnnounce vacancyAnnounce = new VacancyAnnounce(currentUser);

                    vacancyAnnounce.VacancyAnnounceId = DBCommon.GetInt(dr["VacancyAnnounceID"]);
                    vacancyAnnounce.OrderNum = dr["OrderNum"].ToString();

                    vacancyAnnounces.Add(vacancyAnnounce);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vacancyAnnounces;
        }

        public static List<ListItem> GetDistinctRespMilitaryUnitsForVacancyAnnounceID(int vacancyAnnounceId, User currentUser)
        {
            List<ListItem> militaryUnits = new List<ListItem>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT DISTINCT a.ResponsibleMilitaryUnitID, b.IMEES as ShortName, b.VPN as VPN
                               FROM PMIS_APPL.VacancyAnnouncePositions a
                               INNER JOIN UKAZ_OWNER.MIR b ON a.ResponsibleMilitaryUnitID = b.KOD_MIR
                               WHERE a.VacancyAnnounceID = :VacancyAnnounceID";

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    SQL += @" AND a.ResponsibleMilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @")
                            ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VacancyAnnounceID", OracleType.Number).Value = vacancyAnnounceId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["ResponsibleMilitaryUnitID"]))
                    {
                        ListItem li = new ListItem();
                        li.Text = dr["VPN"].ToString() + " " + dr["ShortName"].ToString();
                        li.Value = DBCommon.GetInt(dr["ResponsibleMilitaryUnitID"]).ToString();
                        militaryUnits.Add(li);
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryUnits;
        }

        public static List<ListItem> GetDistinctMilitaryUnitsForVacancyAnnounceID(int vacancyAnnounceId, User currentUser)
        {
            List<ListItem> militaryUnits = new List<ListItem>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT DISTINCT a.MilitaryUnitID, b.IMEES as ShortName, b.VPN as VPN
                               FROM PMIS_APPL.VacancyAnnouncePositions a
                               INNER JOIN UKAZ_OWNER.MIR b ON a.MilitaryUnitID = b.KOD_MIR
                               WHERE a.VacancyAnnounceID = :VacancyAnnounceID";

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    SQL += @" AND a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @")
                            ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VacancyAnnounceID", OracleType.Number).Value = vacancyAnnounceId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["MilitaryUnitID"]))
                    {
                        ListItem li = new ListItem();
                        li.Text = dr["VPN"].ToString() + " " + dr["ShortName"].ToString();
                        li.Value = DBCommon.GetInt(dr["MilitaryUnitID"]).ToString();
                        militaryUnits.Add(li);
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryUnits;
        }

        public static List<ListItem> GetDistinctPositionsForVacancyAnnounceID(int vacancyAnnounceId, User currentUser)
        {
            List<ListItem> positions = new List<ListItem>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT DISTINCT a.PositionName
                               FROM PMIS_APPL.VacancyAnnouncePositions a
                               WHERE a.VacancyAnnounceID = :VacancyAnnounceID
                               ";

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    SQL += @" AND a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @")
                            ";

                SQL += @" ORDER BY a.PositionName";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VacancyAnnounceID", OracleType.Number).Value = vacancyAnnounceId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ListItem li = new ListItem();
                    li.Text = dr["PositionName"].ToString();
                    li.Value = dr["PositionName"].ToString();
                    positions.Add(li);                    
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return positions;
        }

        public static int GetApplicantsCount(int vacancyAnnounceId, User currentUser)
        {
            int applicantsCount = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.ApplicantsCount
                                FROM (
                                      SELECT a.VacancyAnnounceID, a.OrderNum, COUNT(DISTINCT d.ApplicantID) ApplicantsCount 
                                      FROM PMIS_APPL.VacancyAnnounces a
                                      LEFT OUTER JOIN PMIS_APPL.VacancyAnnouncePositions b ON b.VacancyAnnounceID = a.VacancyAnnounceID
                                      LEFT OUTER JOIN PMIS_APPL.ApplicantPositions c ON c.VacancyAnnouncePositionId = b.VacancyAnnouncePositionId
                                      LEFT OUTER JOIN PMIS_APPL.Applicants d ON d.ApplicantId = c.ApplicantId
                                      GROUP BY a.VacancyAnnounceID, a.OrderNum
                                      ) a
                                WHERE a.VacancyAnnounceID = :VacancyAnnounceID";

                OracleCommand cmd = new OracleCommand(SQL, conn);
                cmd.Parameters.Add("VacancyAnnounceID", OracleType.Number).Value = vacancyAnnounceId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["ApplicantsCount"]))
                    {
                        applicantsCount = DBCommon.GetInt(dr["ApplicantsCount"]);
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return applicantsCount;
        }

        public static void SetVacancyAnnounceModified(int vacancyAnnounceId, User currentUser)
        {
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"UPDATE PMIS_APPL.VacancyAnnounces SET
                                  LastModifiedBy = :LastModifiedBy,
                                  LastModifiedDate = :LastModifiedDate
                               WHERE VacancyAnnounceID = :VacancyAnnounceID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VacancyAnnounceID", OracleType.Number).Value = vacancyAnnounceId;

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }

        //Save a particular object into the DB
        public static bool SaveVacancyAnnounce(VacancyAnnounce vacancyAnnounce, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            string logDescription = "";
            logDescription += "Заповед №: " + vacancyAnnounce.OrderNum + " / Дата:" + CommonFunctions.FormatDate(vacancyAnnounce.OrderDate);

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";
                if (vacancyAnnounce.VacancyAnnounceId == 0)
                {
                    SQL += @"INSERT INTO PMIS_APPL.VacancyAnnounces (OrderNum, OrderDate, EndDate, MaxPositions, VacAnnStatusID, VacAnnType, 
                                   CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate)
                            VALUES (:OrderNum, :OrderDate, :EndDate, :MaxPositions, :VacAnnStatusID, :VacAnnType, 
                                   :CreatedBy, :CreatedDate, :LastModifiedBy, :LastModifiedDate);

                            SELECT PMIS_APPL.VacancyAnnounces_ID_SEQ.currval INTO :VacancyAnnounceID FROM dual;

                            ";

                    changeEvent = new ChangeEvent("APPL_VacAnn_AddVacAnn", logDescription, null, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnn_OrderNum", "", vacancyAnnounce.OrderNum, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnn_OrderDate", "", CommonFunctions.FormatDate(vacancyAnnounce.OrderDate), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnn_EndDate", "", CommonFunctions.FormatDate(vacancyAnnounce.EndDate), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnn_MaxPositions", "", vacancyAnnounce.MaxPositions.HasValue ? vacancyAnnounce.MaxPositions.Value.ToString() : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnn_Status", "", vacancyAnnounce.VacancyAnnounceStatus != null && vacancyAnnounce.VacancyAnnounceStatus.VacAnnStatusName != null ? vacancyAnnounce.VacancyAnnounceStatus.VacAnnStatusName : "", currentUser));

                    string vacancyAnnounceType = ((vacancyAnnounce.VacancyAnnounceType == 1) ? "кадрови" : "резерв");
                    changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnn_Type", "", vacancyAnnounceType, currentUser));

                }
                else
                {
                    SQL += @"UPDATE PMIS_APPL.VacancyAnnounces SET
                               OrderNum = :OrderNum, 
                               OrderDate = :OrderDate, 
                               EndDate = :EndDate, 
                               MaxPositions = :MaxPositions, 
                               VacAnnStatusID = :VacAnnStatusID,
                               VacAnnType = :VacAnnType,  
                               LastModifiedBy = CASE WHEN :AnyActualChanges = 1 
                                                     THEN :LastModifiedBy
                                                     ELSE LastModifiedBy
                                                END, 
                               LastModifiedDate = CASE WHEN :AnyActualChanges = 1 
                                                       THEN :LastModifiedDate
                                                       ELSE LastModifiedDate
                                                  END

                            WHERE VacancyAnnounceID = :VacancyAnnounceID ;                       

                            ";

                    changeEvent = new ChangeEvent("APPL_VacAnn_EditVacAnn", logDescription, null, null, currentUser);

                    VacancyAnnounce oldVacancyAnnounce = VacancyAnnounceUtil.GetVacancyAnnounce(vacancyAnnounce.VacancyAnnounceId, currentUser);

                    if (oldVacancyAnnounce.OrderNum.Trim() != vacancyAnnounce.OrderNum.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnn_OrderNum", oldVacancyAnnounce.OrderNum, vacancyAnnounce.OrderNum, currentUser));

                    if (!CommonFunctions.IsEqual(oldVacancyAnnounce.OrderDate, vacancyAnnounce.OrderDate))
                        changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnn_OrderDate", CommonFunctions.FormatDate(oldVacancyAnnounce.OrderDate), CommonFunctions.FormatDate(vacancyAnnounce.OrderDate), currentUser));

                    if (!CommonFunctions.IsEqual(oldVacancyAnnounce.EndDate, vacancyAnnounce.EndDate))
                        changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnn_EndDate", CommonFunctions.FormatDate(oldVacancyAnnounce.EndDate), CommonFunctions.FormatDate(vacancyAnnounce.EndDate), currentUser));

                    if (!CommonFunctions.IsEqualInt(oldVacancyAnnounce.MaxPositions, vacancyAnnounce.MaxPositions))
                        changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnn_MaxPositions", oldVacancyAnnounce.MaxPositions.HasValue ? oldVacancyAnnounce.MaxPositions.Value.ToString() : "", vacancyAnnounce.MaxPositions.HasValue ? vacancyAnnounce.MaxPositions.Value.ToString() : "", currentUser));

                    if ((oldVacancyAnnounce.VacancyAnnounceStatus != null && oldVacancyAnnounce.VacancyAnnounceStatus.VacAnnStatusName != null ? oldVacancyAnnounce.VacancyAnnounceStatus.VacAnnStatusName : "") !=
                        (vacancyAnnounce.VacancyAnnounceStatus != null && vacancyAnnounce.VacancyAnnounceStatus.VacAnnStatusName != null ? vacancyAnnounce.VacancyAnnounceStatus.VacAnnStatusName : ""))
                        changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnn_Status", oldVacancyAnnounce.VacancyAnnounceStatus != null && oldVacancyAnnounce.VacancyAnnounceStatus.VacAnnStatusName != null ? oldVacancyAnnounce.VacancyAnnounceStatus.VacAnnStatusName : "", vacancyAnnounce.VacancyAnnounceStatus != null && vacancyAnnounce.VacancyAnnounceStatus.VacAnnStatusName != null ? vacancyAnnounce.VacancyAnnounceStatus.VacAnnStatusName : "", currentUser));


                    if (oldVacancyAnnounce.VacancyAnnounceType != vacancyAnnounce.VacancyAnnounceType)
                    {
                        string oldVacancyAnnounceType = ((oldVacancyAnnounce.VacancyAnnounceType == 1) ? "кадрови" : "резерв");
                        string vacancyAnnounceType = ((vacancyAnnounce.VacancyAnnounceType == 1) ? "кадрови" : "резерв");
                        changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnn_Type", oldVacancyAnnounceType, vacancyAnnounceType, currentUser));
                    }

                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramVacancyAnnounceID = new OracleParameter();
                paramVacancyAnnounceID.ParameterName = "VacancyAnnounceID";
                paramVacancyAnnounceID.OracleType = OracleType.Number;

                if (vacancyAnnounce.VacancyAnnounceId != 0)
                {
                    paramVacancyAnnounceID.Direction = ParameterDirection.Input;
                    paramVacancyAnnounceID.Value = vacancyAnnounce.VacancyAnnounceId;
                }
                else
                {
                    paramVacancyAnnounceID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramVacancyAnnounceID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "OrderNum";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(vacancyAnnounce.OrderNum))
                    param.Value = vacancyAnnounce.OrderNum;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "OrderDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (vacancyAnnounce.OrderDate.HasValue)
                    param.Value = vacancyAnnounce.OrderDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "EndDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (vacancyAnnounce.EndDate.HasValue)
                    param.Value = vacancyAnnounce.EndDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MaxPositions";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (vacancyAnnounce.MaxPositions.HasValue)
                    param.Value = vacancyAnnounce.MaxPositions.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "VacAnnStatusID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (vacancyAnnounce.VacancyAnnounceStatus != null)
                    param.Value = vacancyAnnounce.VacancyAnnounceStatus.VacancyAnnouncesStatusId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "VacAnnType";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = vacancyAnnounce.VacancyAnnounceType;
                cmd.Parameters.Add(param);

                if (vacancyAnnounce.VacancyAnnounceId == 0)
                {
                    BaseDbObjectUtil.SetCreatedParams(cmd, currentUser);
                }
                else
                {
                    BaseDbObjectUtil.SetAnyActualChanges(cmd, changeEvent);
                }

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();

                if (vacancyAnnounce.VacancyAnnounceId == 0)
                    vacancyAnnounce.VacancyAnnounceId = DBCommon.GetInt(paramVacancyAnnounceID.Value);

                result = true;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                if (changeEvent.ChangeEventDetails.Count > 0)
                    changeEntry.AddEvent(changeEvent);
            }

            return result;
        }

        public static bool DeleteVacancyAnnounce(int vacancyAnnounceId, User currentUser, Change changeEntry)
        {
            bool result = false;

            VacancyAnnounce oldVacancyAnnounce = GetVacancyAnnounce(vacancyAnnounceId, currentUser);

            string logDescription = "";
            logDescription += "Заповед №: " + oldVacancyAnnounce.OrderNum + " / Дата:" + CommonFunctions.FormatDate(oldVacancyAnnounce.OrderDate);

            ChangeEvent changeEvent = new ChangeEvent("APPL_VacAnn_DeleteVacAnn", logDescription, null, null, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnn_OrderNum", oldVacancyAnnounce.OrderNum, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnn_OrderDate", CommonFunctions.FormatDate(oldVacancyAnnounce.OrderDate), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnn_EndDate", CommonFunctions.FormatDate(oldVacancyAnnounce.EndDate), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnn_MaxPositions", oldVacancyAnnounce.MaxPositions.HasValue ? oldVacancyAnnounce.MaxPositions.Value.ToString() : "", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnn_Status", oldVacancyAnnounce.VacancyAnnounceStatus != null ? oldVacancyAnnounce.VacancyAnnounceStatus.VacAnnStatusName : "", "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" BEGIN
                                   DELETE FROM PMIS_APPL.VacancyAnnounceExams WHERE VacancyAnnounceID = :VacancyAnnounceID;

                                   DELETE FROM PMIS_APPL.VacancyAnnounceDocuments WHERE VacancyAnnounceID = :VacancyAnnounceID;
    
                                   DELETE FROM PMIS_APPL.VacancyAnnouncePositions WHERE VacancyAnnounceID = :VacancyAnnounceID;

                                   DELETE FROM PMIS_APPL.VacancyAnnounces WHERE VacancyAnnounceID = :VacancyAnnounceID;
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VacancyAnnounceID", OracleType.Number).Value = vacancyAnnounceId;

                result = cmd.ExecuteNonQuery() == 1;
            }
            finally
            {
                conn.Close();
            }

            if (result)
                changeEntry.AddEvent(changeEvent);

            return result;
        }

        public static List<ListItem> GetVacancyAnnouncesListItems(string UiItemKey, bool militaryUnitFilter, User currentUser)
        {
            List<ListItem> vacancyAnnounces = new List<ListItem>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems(UiItemKey, currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (militaryUnitFilter)
                {
                    if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResponsibleMilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @")
                              ";
                }

                where += (where == "" ? "" : " AND ") +
                         @" (a.VacannStatusID NOT IN (SELECT VacancyAnnouncesStatusID FROM PMIS_APPL.VacancyAnnouncesStatuses WHERE VacannStatusKey IN ('CREATINGORDER')))
                         ";

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @" SELECT a.VacancyAnnounceID, a.OrderNum, a.OrderDate                                       
                                FROM PMIS_APPL.VacancyAnnounces a                                                                
                                INNER JOIN PMIS_APPL.VacancyAnnouncePositions b ON a.VacancyAnnounceID = b.VacancyAnnounceID                                  
                              " + where + @"
                                GROUP BY a.VacancyAnnounceID, a.OrderNum, a.OrderDate 
                                ORDER BY a.OrderDate DESC, a.OrderNum ASC
                              ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    int vacancyAnnounceID;
                    if (DBCommon.IsInt(dr["VacancyAnnounceID"]))
                        vacancyAnnounceID = DBCommon.GetInt(dr["VacancyAnnounceID"]);
                    else
                        continue;

                    string orderNum = dr["OrderNum"].ToString();
                    DateTime? orderDate = null;
                    if (dr["OrderDate"] is DateTime)
                        orderDate = (DateTime)dr["OrderDate"];

                    ListItem li = new ListItem();
                    li.Text = OrderNumOrderDate(orderNum, orderDate);
                    li.Value = vacancyAnnounceID.ToString();
                    vacancyAnnounces.Add(li);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vacancyAnnounces;
        }

        public static List<ListItem> GetVacancyAnnouncesListItemsForReports(string UiItemKey, bool militaryUnitFilter, User currentUser)
        {
            List<ListItem> vacancyAnnounces = new List<ListItem>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems(UiItemKey, currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (militaryUnitFilter)
                {
                    if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                        where += (where == "" ? "" : " AND ") +
                                 @" b.ResponsibleMilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @")
                              ";
                }

                where += (where == "" ? "" : " AND ") +
                         @" (a.VacannStatusID NOT IN (SELECT VacancyAnnouncesStatusID FROM PMIS_APPL.VacancyAnnouncesStatuses WHERE VacannStatusKey IN ('CREATINGORDER')))
                         ";

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @" SELECT a.VacancyAnnounceID, a.OrderNum, a.OrderDate                                       
                                FROM PMIS_APPL.VacancyAnnounces a                                                                
                                INNER JOIN PMIS_APPL.VacancyAnnouncePositions b ON a.VacancyAnnounceID = b.VacancyAnnounceID                                  
                              " + where + @"
                                GROUP BY a.VacancyAnnounceID, a.OrderNum, a.OrderDate
                                ORDER BY a.OrderDate DESC, a.OrderNum ASC
                              ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    int vacancyAnnounceID;
                    if (DBCommon.IsInt(dr["VacancyAnnounceID"]))
                        vacancyAnnounceID = DBCommon.GetInt(dr["VacancyAnnounceID"]);
                    else
                        continue;

                    string orderNum = dr["OrderNum"].ToString();
                    DateTime? orderDate = null;
                    if (dr["OrderDate"] is DateTime)
                        orderDate = (DateTime)dr["OrderDate"];

                    ListItem li = new ListItem();
                    li.Text = OrderNumOrderDate(orderNum, orderDate);
                    li.Value = vacancyAnnounceID.ToString();
                    vacancyAnnounces.Add(li);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vacancyAnnounces;
        }

        public static void SetVacancyAnnounceStatusFlow(int vacancyAnnounceId, string changeFrom_VacAnnStatusKey, User currentUser)
        {
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"PMIS_APPL.APPL_Functions.SetVacancyAnnounceStatusFlow";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("P_VacancyAnnounceID", OracleType.Number).Value = vacancyAnnounceId;
                cmd.Parameters.Add("P_ChangeFrom_VacAnnStatusKey", OracleType.VarChar).Value = changeFrom_VacAnnStatusKey;

                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }
    }
}