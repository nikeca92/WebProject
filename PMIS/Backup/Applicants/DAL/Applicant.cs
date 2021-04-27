using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Applicants.Common
{
    //Each applicant is identified as a particular Person and MilitaryDepartment combination
    public class Applicant : BaseDbObject
    {
        private int applicantId;
        private int personId;
        private Person person;
        private int militaryDepartmentId;
        private MilitaryDepartment militaryDepartment;
        private List<ApplicantPosition> applicantPositions;

        public int ApplicantId
        {
            get { return applicantId; }
            set { applicantId = value; }
        }

        public int PersonId
        {
            get { return personId; }
            set { personId = value; }
        }

        public Person Person
        {
            get
            {
                if (person == null)
                {
                    person = PersonUtil.GetPerson(personId, base.CurrentUser);
                }
                return person;

            }
            set { person = value; }
        }

        public int MilitaryDepartmentId
        {
            get { return militaryDepartmentId; }
            set { militaryDepartmentId = value; }
        }

        public MilitaryDepartment MilitaryDepartment
        {
            get
            {
                if (militaryDepartment == null)
                {
                    militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment(militaryDepartmentId, base.CurrentUser);
                }
                return militaryDepartment;

            }
            set { militaryDepartment = value; }
        }

        public List<ApplicantPosition> ApplicantPositions
        {
            get
            {
                if (applicantPositions == null)
                    applicantPositions = ApplicantPositionUtil.GetAllApplicantPositionByPersonID(personId, null, CurrentUser);

                return applicantPositions;
            }
            set
            {
                applicantPositions = value;
            }
        }

        public bool CanDelete
        {
            get
            {
                if (ApplicantPositions.Count > 0)
                    return false;
                else
                    return true;
            }
        }

        public Applicant(User user) :base(user)
        {
            applicantId = 0;
        }
    }

    public static class ApplicantUtil
    {
        public static Applicant GetApplicant(int applicantId, User currentUser)
        {
            Applicant applicant = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("APPL_APPL", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where = " AND a.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.ApplicantID, b.PersonID, d.MilitaryDepartmentID,
                                    a.CreatedBy, a.CreatedDate, a.LastModifiedBy, a.LastModifiedDate
                                  FROM PMIS_APPL.Applicants a
                                  JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                                  LEFT OUTER JOIN PMIS_ADM.Persons c ON b.PersonID = c.PersonID
                                  JOIN PMIS_ADM.MilitaryDepartments d ON a.MilitaryDepartmentID = d.MilitaryDepartmentID
                                  WHERE a.ApplicantID = :ApplicantID " + where;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ApplicantID", OracleType.Number).Value = applicantId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    applicant = new Applicant(currentUser);

                    if (DBCommon.IsInt(dr["ApplicantID"]))
                        applicant.ApplicantId = DBCommon.GetInt(dr["ApplicantID"]);

                    if (DBCommon.IsInt(dr["PersonID"]))
                        applicant.PersonId = DBCommon.GetInt(dr["PersonID"]);

                    if (DBCommon.IsInt(dr["MilitaryDepartmentID"]))  
                        applicant.MilitaryDepartmentId = DBCommon.GetInt(dr["MilitaryDepartmentID"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return applicant;
        }

        public static Applicant GetApplicant(int personId, int militaryDepartmentId, User currentUser)
        {
            Applicant applicant = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("APPL_APPL", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where = " AND a.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.ApplicantID, b.PersonID, d.MilitaryDepartmentID,
                                    a.CreatedBy, a.CreatedDate, a.LastModifiedBy, a.LastModifiedDate
                                  FROM PMIS_APPL.Applicants a
                                  JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                                  LEFT OUTER JOIN PMIS_ADM.Persons c ON b.PersonID = c.PersonID
                                  JOIN PMIS_ADM.MilitaryDepartments d ON a.MilitaryDepartmentID = d.MilitaryDepartmentID
                                  WHERE a.PersonID = :PersonID AND a.MilitaryDepartmentID = :MilitaryDepartmentID " + where;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Number).Value = personId;
                cmd.Parameters.Add("MilitaryDepartmentID", OracleType.Number).Value = militaryDepartmentId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    applicant = new Applicant(currentUser);

                    if (DBCommon.IsInt(dr["ApplicantID"]))
                        applicant.ApplicantId = DBCommon.GetInt(dr["ApplicantID"]);

                    if (DBCommon.IsInt(dr["PersonID"]))
                        applicant.PersonId = DBCommon.GetInt(dr["PersonID"]);

                    if (DBCommon.IsInt(dr["MilitaryDepartmentID"]))
                        applicant.MilitaryDepartmentId = DBCommon.GetInt(dr["MilitaryDepartmentID"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return applicant;
        }

        //This function checks if a particular Person and MilitaryDepartment combination has been already registered
        public static bool IsAlreadyRegistered(int personId, int militaryDepartmentId, User currentUser)
        {
            bool isRegistered = false;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT NULL
                               FROM PMIS_APPL.Applicants a
                               WHERE a.PersonID = :PersonID AND 
                                     a.MilitaryDepartmentID = :MilitaryDepartmentID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Number).Value = personId;
                cmd.Parameters.Add("MilitaryDepartmentID", OracleType.Number).Value = militaryDepartmentId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    isRegistered = true;
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return isRegistered;
        }

        public static void SetApplicantModified(int applicantId, User currentUser)
        {
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"UPDATE PMIS_APPL.Applicants SET
                                  LastModifiedBy = :LastModifiedBy,
                                  LastModifiedDate = :LastModifiedDate
                               WHERE ApplicantID = :ApplicantID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ApplicantID", OracleType.Number).Value = applicantId;

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }

        public static void SetApplicantsModified(List<int> applicantsId, User currentUser)
        {
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string applicantsIDs = "";

            foreach (int applicantId in applicantsId)
            {
                applicantsIDs += applicantsIDs == "" ? applicantId.ToString() : "," + applicantId.ToString();
            }

            try
            {
                string SQL = @"UPDATE PMIS_APPL.Applicants SET
                                  LastModifiedBy = :LastModifiedBy,
                                  LastModifiedDate = :LastModifiedDate
                               WHERE ApplicantID IN (" + applicantsIDs + ")";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }

        //Save a particular object into the DB
        public static bool SaveApplicant(Applicant applicant, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            string logDescription = "";
            logDescription += "Име: " + applicant.Person.FullName;
            logDescription += "<br />ЕГН: " + applicant.Person.IdentNumber;
            logDescription += "<br />Военно окръжие: " + applicant.MilitaryDepartment.MilitaryDepartmentName;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";
                if (applicant.ApplicantId == 0)
                {
                    SQL += @"INSERT INTO PMIS_APPL.Applicants (PersonID, MilitaryDepartmentID,
                                   CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate)
                            VALUES (:PersonID, :MilitaryDepartmentID,
                                   :CreatedBy, :CreatedDate, :LastModifiedBy, :LastModifiedDate);

                            SELECT PMIS_APPL.Applicants_ID_SEQ.currval INTO :ApplicantID FROM dual;

                            ";

                    changeEvent = new ChangeEvent("APPL_Applicants_Add", logDescription, null, applicant.Person, currentUser);
                }
                else
                {
                    SQL += @"UPDATE PMIS_APPL.Applicants SET
                               PersonID = :PersonID, 
                               MilitaryDepartmentID = :MilitaryDepartmentID,                                
                               LastModifiedBy = CASE WHEN :AnyActualChanges = 1 
                                                     THEN :LastModifiedBy
                                                     ELSE LastModifiedBy
                                                END, 
                               LastModifiedDate = CASE WHEN :AnyActualChanges = 1 
                                                       THEN :LastModifiedDate
                                                       ELSE LastModifiedDate
                                                  END

                            WHERE ApplicantID = :ApplicantID ;                       

                            ";

                    changeEvent = new ChangeEvent("APPL_Applicants_Edit", logDescription, null, applicant.Person, currentUser);
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramApplicantID = new OracleParameter();
                paramApplicantID.ParameterName = "ApplicantID";
                paramApplicantID.OracleType = OracleType.Number;

                if (applicant.ApplicantId != 0)
                {
                    paramApplicantID.Direction = ParameterDirection.Input;
                    paramApplicantID.Value = applicant.ApplicantId;
                }
                else
                {
                    paramApplicantID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramApplicantID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "PersonID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = applicant.PersonId;               
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryDepartmentID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = applicant.MilitaryDepartmentId;
                cmd.Parameters.Add(param);

                if (applicant.ApplicantId == 0)
                {
                    BaseDbObjectUtil.SetCreatedParams(cmd, currentUser);
                }
                else
                {
                    BaseDbObjectUtil.SetAnyActualChanges(cmd, changeEvent);
                }

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();

                if (applicant.ApplicantId == 0)
                    applicant.ApplicantId = DBCommon.GetInt(paramApplicantID.Value);

                result = true;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                changeEntry.AddEvent(changeEvent);
            }

            return result;
        }

        public static bool DeleteApplicant(int applicantId, User currentUser, Change changeEntry)
        {

            bool result = false;

            Applicant applicant = GetApplicant(applicantId, currentUser);

            string logDescription = "";
            logDescription += "Име: " + applicant.Person.FullName;
            logDescription += "<br />ЕГН: " + applicant.Person.IdentNumber;
            logDescription += "<br />Военно окръжие: " + applicant.MilitaryDepartment.MilitaryDepartmentName;

            ChangeEvent changeEvent = new ChangeEvent("APPL_Applicants_Delete", logDescription, null, applicant.Person, currentUser);
            
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" DECLARE Cnt NUMBER;
                                BEGIN
                                  SELECT COUNT(*)
                                  INTO Cnt
                                  FROM PMIS_APPL.ApplicantPositions
                                  WHERE ApplicantID = :ApplicantID;
                                  
                                  IF Cnt = 0 then
                                    DELETE FROM PMIS_APPL.Applicants WHERE ApplicantID = :ApplicantID;
                                  END IF;
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ApplicantID", OracleType.Number).Value = applicantId;

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
    }

    //This class represents all information about the filter, the order and the paging information on the screen
    public class ApplicantsSearchFilter
    {
        int? vacancyAnnounceId;
        int? militaryDepartmentId;
        string identityNumber;
        int orderBy;
        int pageIdx;

        public int? VacancyAnnounceId
        {
            get { return vacancyAnnounceId; }
            set { vacancyAnnounceId = value; }
        }

        public int? MilitaryDepartmentId
        {
            get { return militaryDepartmentId; }
            set { militaryDepartmentId = value; }
        }

        public int OrderBy
        {
            get { return orderBy; }
            set { orderBy = value; }
        }

        public int PageIdx
        {
            get { return pageIdx; }
            set { pageIdx = value; }
        }

        public string IdentityNumber
        {
            get { return identityNumber; }
            set { identityNumber = value; }
        }
    }

    //Each applicant search is identitified as a particular Person and MilitaryDepartment combination depend applied vacance announce
    public class ApplicantSearch : BaseDbObject
    {
        private int applicantId;
        private int personId;
        private string personName;
        private string personIdentNumber;
        private int vacancyAnnounceId;
        private string vacancyAnnounceOrderNumber;
        private DateTime? vacancyAnnounceOrderDate;
        private int militaryDepartmentId;
        private string militaryDepartmentName;
        private List<ApplicantPosition> applicantPositions;

        public int ApplicantId
        {
            get { return applicantId; }
            set { applicantId = value; }
        }

        public int PersonId
        {
            get { return personId; }
            set { personId = value; }
        }

        public string PersonName
        {
            get { return personName; }
            set { personName = value; }
        }

        public string PersonIdentNumber
        {
            get { return personIdentNumber; }
            set { personIdentNumber = value; }
        }

        public int VacancyAnnounceId
        {
            get { return vacancyAnnounceId; }
            set { vacancyAnnounceId = value; }
        }

        public string VacancyAnnounceOrderNumber
        {
            get { return vacancyAnnounceOrderNumber; }
            set { vacancyAnnounceOrderNumber = value; }
        }

        public DateTime? VacancyAnnounceOrderDate
        {
            get { return vacancyAnnounceOrderDate; }
            set { vacancyAnnounceOrderDate = value; }
        }

        public int MilitaryDepartmentId
        {
            get { return militaryDepartmentId; }
            set { militaryDepartmentId = value; }
        }

        public string MilitaryDepartmentName
        {
            get { return militaryDepartmentName; }
            set { militaryDepartmentName = value; }
        }

        public List<ApplicantPosition> ApplicantPositions
        {
            get
            {
                if (applicantPositions == null)
                    applicantPositions = ApplicantPositionUtil.GetAllApplicantPositionByPersonID(personId, null, CurrentUser);

                return applicantPositions;
            }
            set
            {
                applicantPositions = value;
            }
        }

        public bool CanDelete
        {
            get
            {
                if (ApplicantPositions.Count > 0)
                    return false;
                else
                    return true;
            }
        }

        public ApplicantSearch(User user)
            : base(user)
        {

        }
    }

    public static class ApplicantSearchUtil
    {
        public static List<ApplicantSearch> GetAllApplicantsBySearch(ApplicantsSearchFilter filter, int rowsPerPage, User currentUser)
        {
            List<ApplicantSearch> applicantSearches = new List<ApplicantSearch>();

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
                             " d.CreatedBy = " + currentUser.UserId.ToString();
                }

                if (!string.IsNullOrEmpty(filter.IdentityNumber))
                {
                    if (filter.IdentityNumber.Length == 10)
                        where += (where == "" ? "" : " AND ") +
                             " e.EGN = '" + filter.IdentityNumber.Replace("'", "''") + "' ";
                    else
                        where += (where == "" ? "" : " AND ") +
                                 " e.EGN LIKE '" + filter.IdentityNumber.Replace("'", "''") + "%' ";
                }

                if (filter.VacancyAnnounceId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.VacancyAnnounceId = " + filter.VacancyAnnounceId.Value + " ";
                }

                if (filter.MilitaryDepartmentId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " d.MilitaryDepartmentID = " + filter.MilitaryDepartmentId.Value + " ";
                }

//                if (!string.IsNullOrEmpty(currentUser.MilitaryDepartmentIDs))
//                {
//                    where += (where == "" ? " " : " AND ");
//                    where += @" d.MilitaryDepartmentID IN (" + currentUser.MilitaryDepartmentIDs + @")
//                        ";
//                }

                where = (where == "" ? "" : " WHERE ") + where;

                string pageWhere = "";

                if (filter.PageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE RowNumber BETWEEN (" + filter.PageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + filter.PageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

                string rankOrderBySQL = "";
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
                        rankOrderBySQL = "e.IME " + orderByDir + ", e.FAM";
                        orderBySQL = "PersonName";
                        break;
                    case 2:
                        rankOrderBySQL = "e.EGN";
                        orderBySQL = "e.EGN";
                        break;
                    case 3:
                        rankOrderBySQL = "a.OrderNum";
                        orderBySQL = "a.OrderNum";
                        break;
                    case 4:
                        rankOrderBySQL = "a.OrderDate";
                        orderBySQL = "a.OrderDate";
                        break;
                    case 5:
                        rankOrderBySQL = "j.MilitaryDepartmentName";
                        orderBySQL = "j.MilitaryDepartmentName";
                        break;
                    case 6:
                        rankOrderBySQL = "d.LastModifiedDate";
                        orderBySQL = "d.LastModifiedDate";
                        break;
                    default:
                        rankOrderBySQL = "e.IME " + orderByDir + ", e.FAM";
                        orderBySQL = "PersonName";
                        break;
                }

                rankOrderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);
                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                SELECT DISTINCT(a.VacancyAnnounceID), a.OrderNum as VacancyAnnounceOrderNumber, 
                                  a.OrderDate as VacancyAnnounceOrderDate, j.MilitaryDepartmentID, j.MilitaryDepartmentName,
                                  d.ApplicantID, e.PersonID, e.IME || ' ' || e.FAM as PersonName, e.EGN as PersonIdentNumber, 
                                  d.CreatedBy, d.CreatedDate, d.LastModifiedBy, d.LastModifiedDate,
                                  DENSE_RANK() OVER (ORDER BY " + rankOrderBySQL + @", d.ApplicantID, a.VacancyAnnounceID) as RowNumber 
                                FROM PMIS_APPL.VacancyAnnounces a
                                INNER JOIN PMIS_APPL.VacancyAnnouncePositions b ON a.VacancyAnnounceID = b.VacancyAnnounceID
                                INNER JOIN PMIS_APPL.ApplicantPositions c ON b.VacancyAnnouncePositionID = c.VacancyAnnouncePositionID
                                INNER JOIN PMIS_APPL.Applicants d ON c.ApplicantID = d.ApplicantID
                                INNER JOIN VS_OWNER.VS_LS e ON d.PersonID = e.PersonID
                                LEFT OUTER JOIN PMIS_ADM.Persons f ON e.PersonID = f.PersonID
                                INNER JOIN PMIS_ADM.MilitaryDepartments j ON d.MilitaryDepartmentID = j.MilitaryDepartmentID
 
                                  " + where + @"    
                                  ORDER BY " + orderBySQL + @", d.ApplicantID, a.VacancyAnnounceID                             
                               ) tmp
                               " + pageWhere;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ApplicantSearch applicantSearch = new ApplicantSearch(currentUser);

                    applicantSearch.ApplicantId = DBCommon.GetInt(dr["ApplicantID"]);
                    applicantSearch.PersonId = DBCommon.GetInt(dr["PersonID"]);
                    applicantSearch.PersonName = dr["PersonName"].ToString();
                    applicantSearch.PersonIdentNumber = dr["PersonIdentNumber"].ToString();
                    applicantSearch.VacancyAnnounceId = DBCommon.GetInt(dr["VacancyAnnounceID"]);
                    applicantSearch.VacancyAnnounceOrderNumber = dr["VacancyAnnounceOrderNumber"].ToString();

                    if (dr["VacancyAnnounceOrderDate"] is DateTime)
                        applicantSearch.VacancyAnnounceOrderDate = (DateTime)dr["VacancyAnnounceOrderDate"];

                    applicantSearch.MilitaryDepartmentId = DBCommon.GetInt(dr["MilitaryDepartmentID"]);
                    applicantSearch.MilitaryDepartmentName = dr["MilitaryDepartmentName"].ToString();

                    BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, applicantSearch);

                    applicantSearches.Add(applicantSearch);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return applicantSearches;
        }

        public static int GetAllApplicantsCount(ApplicantsSearchFilter filter, User currentUser)
        {
            int applicantsCnt = 0;

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

                if (!string.IsNullOrEmpty(filter.IdentityNumber))
                {
                    if (filter.IdentityNumber.Length == 10)
                        where += (where == "" ? "" : " AND ") +
                             " b.EGN = '" + filter.IdentityNumber.Replace("'", "''") + "' ";
                    else
                        where += (where == "" ? "" : " AND ") +
                                 " b.EGN LIKE '" + filter.IdentityNumber.Replace("'", "''") + "%' ";
                }

                if (filter.VacancyAnnounceId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " j.VacancyAnnounceId = " + filter.VacancyAnnounceId.Value + " ";
                }

                if (filter.MilitaryDepartmentId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MilitaryDepartmentID = " + filter.MilitaryDepartmentId.Value + " ";
                }

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @"SELECT COUNT(*) as Cnt
                               FROM (
                                SELECT DISTINCT a.ApplicantID, j.VacancyAnnounceID
                                FROM PMIS_APPL.Applicants a
                                INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                                LEFT OUTER JOIN PMIS_ADM.Persons c ON b.PersonID = c.PersonID
                                INNER JOIN PMIS_ADM.MilitaryDepartments d ON a.MilitaryDepartmentID = d.MilitaryDepartmentID
                                INNER JOIN PMIS_APPL.ApplicantPositions e ON a.ApplicantID = e.ApplicantID
                                INNER JOIN PMIS_APPL.VacancyAnnouncePositions f ON e.VacancyAnnouncePositionID = f.VacancyAnnouncePositionID
                                INNER JOIN PMIS_APPL.VacancyAnnounces j ON f.VacancyAnnounceID = j.VacancyAnnounceID
                               " + where + @") tmp
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