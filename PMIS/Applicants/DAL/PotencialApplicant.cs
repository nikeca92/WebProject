using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Applicants.Common
{
    //Each applicant is identitified as a particular Person and MilitaryDepartment combination
    public class PotencialApplicant : BaseDbObject
    {
        private int potencialApplicantId;
        private int personId;
        private Person person;
        private int militaryDepartmentId;
        private MilitaryDepartment militaryDepartment;
        private string comments;
        private DateTime? lastAppearance;
        private List<ServiceType> serviceTypes;
        private List<MilitaryTrainingCourse> militaryTrainingCourses;

        public int PotencialApplicantId
        {
            get { return potencialApplicantId; }
            set { potencialApplicantId = value; }
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

        public string Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        public DateTime? LastAppearance
        {
            get { return lastAppearance; }
            set { lastAppearance = value; }
        }

        public PotencialApplicant(User user)
            : base(user)
        {
            potencialApplicantId = 0;
        }

        public List<ServiceType> ServiceTypes
        {
            get
            {
                //Lazy initialization. Use it only when the list of specialities isn't already loaded
                //When loading the entire list of position we pull the specialities too
                if (serviceTypes == null)
                    serviceTypes = ServiceTypeUtil.GetAssignedServiceTypes(potencialApplicantId, CurrentUser);

                return serviceTypes;
            }
            set
            {
                serviceTypes = value;
            }
        }

        public string ServiceTypesString
        {
            get
            {
                string serviceTypesString = "";

                foreach (ServiceType serviceTypes in ServiceTypes)
                {
                    serviceTypesString += (serviceTypesString == "" ? "" : ", ") + serviceTypes.ServiceTypeName;
                }

                return serviceTypesString;
            }
        }

        public List<MilitaryTrainingCourse> MilitaryTrainingCourses
        {
            get
            {
                //Lazy initialization. Use it only when the list of specialities isn't already loaded
                //When loading the entire list of position we pull the specialities too
                if (militaryTrainingCourses == null)
                    militaryTrainingCourses = MilitaryTrainingCourseUtil.GetAssignedMilitaryTrainingCourses(potencialApplicantId, CurrentUser);

                return militaryTrainingCourses;
            }
            set
            {
                militaryTrainingCourses = value;
            }
        }

        public string MilitaryTrainingCoursesString
        {
            get
            {
                string militaryTrainingCoursesString = "";

                foreach (MilitaryTrainingCourse militaryTrainingCourses in MilitaryTrainingCourses)
                {
                    militaryTrainingCoursesString += (militaryTrainingCoursesString == "" ? "" : ", ") + militaryTrainingCourses.MilitaryTrainingCourseName;
                }

                return militaryTrainingCoursesString;
            }
        }
    }

    //This class represents all information about the filter, the order and the paging information on the screen
    public class PotencialApplicantsFilter
    {
        int? vacancyAnnounceId;
        int? militaryDepartmentId;
        string comment;
        string drivingLicense;
        string serviceType;
        string identityNumber;
        DateTime? lastApperianceFrom;
        DateTime? lastApperianceTo;
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

        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        public string DrivingLicense
        {
            get { return drivingLicense; }
            set { drivingLicense = value; }
        }

        public string ServiceType
        {
            get { return serviceType; }
            set{ serviceType = value;}
        }

        public DateTime? LastApperianceFrom
        {
            get { return lastApperianceFrom; }
            set { lastApperianceFrom = value; }
        }

        public DateTime? LastApperianceTo
        {
            get { return lastApperianceTo; }
            set { lastApperianceTo = value; }
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

    public static class PotencialApplicantUtil
    {

        //This method creates and returns a ApplicantDocumentStatus object. It extracts the data from a DataReader.
        public static PotencialApplicant ExtractPotencialApplicantFromDataReader(OracleDataReader dr, User currentUser)
        {
            PotencialApplicant potencialApplicant = new PotencialApplicant(currentUser);

            if (DBCommon.IsInt(dr["PotencialApplicantID"]))
                potencialApplicant.PotencialApplicantId = DBCommon.GetInt(dr["PotencialApplicantID"]);

            if (DBCommon.IsInt(dr["PersonID"]))
                potencialApplicant.PersonId = DBCommon.GetInt(dr["PersonID"]);

            if (DBCommon.IsInt(dr["MilitaryDepartmentID"]))
                potencialApplicant.MilitaryDepartmentId = DBCommon.GetInt(dr["MilitaryDepartmentID"]);

            potencialApplicant.Comments = dr["Comments"].ToString();

            if (dr["LastAppearance"] is DateTime) potencialApplicant.LastAppearance = (DateTime)dr["LastAppearance"];

            return potencialApplicant;
        }

        public static PotencialApplicant GetPotencialApplicant(int potencialApplicantId, User currentUser)
        {
            PotencialApplicant potencialApplicant = null;

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

                string SQL = @"SELECT a.PotencialApplicantID, b.PersonID, d.MilitaryDepartmentID,
                                    a.Comments, a.LastAppearance,
                                    a.CreatedBy, a.CreatedDate, a.LastModifiedBy, a.LastModifiedDate
                                  FROM PMIS_APPL.PotencialApplicants a
                                  JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                                  LEFT OUTER JOIN PMIS_ADM.Persons c ON b.PersonID = c.PersonID
                                  JOIN PMIS_ADM.MilitaryDepartments d ON a.MilitaryDepartmentID = d.MilitaryDepartmentID
                                  WHERE a.PotencialApplicantID = :PotencialApplicantID " + where;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PotencialApplicantID", OracleType.Number).Value = potencialApplicantId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    potencialApplicant = ExtractPotencialApplicantFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return potencialApplicant;
        }

        public static PotencialApplicant GetPotencialApplicant(int personId, int militaryDepartmentId, User currentUser)
        {
            PotencialApplicant potencialApplicant = null;

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

                string SQL = @"SELECT a.PotencialApplicantID, b.PersonID, d.MilitaryDepartmentID,
                                      a.Comments, a.LastAppearance, 
                                      a.CreatedBy, a.CreatedDate, a.LastModifiedBy, a.LastModifiedDate
                                  FROM PMIS_APPL.PotencialApplicants a
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
                    potencialApplicant = ExtractPotencialApplicantFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return potencialApplicant;
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
                               FROM PMIS_APPL.PotencialApplicants a
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

        public static List<PotencialApplicant> GetAllPotencialApplicants(PotencialApplicantsFilter filter, int rowsPerPage, User currentUser)
        {
            PotencialApplicant potencialApplicant;
            List<PotencialApplicant> listPotencialApplicants = new List<PotencialApplicant>();

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

                if (filter.MilitaryDepartmentId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MilitaryDepartmentID = " + filter.MilitaryDepartmentId.Value + " ";
                }

//                if (!string.IsNullOrEmpty(currentUser.MilitaryDepartmentIDs))
//                {
//                    where += (where == "" ? " " : " AND ");
//                    where += @" a.MilitaryDepartmentID IN (" + currentUser.MilitaryDepartmentIDs + @")
//                                        ";
//                }

                if (!string.IsNullOrEmpty(filter.Comment))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(a.Comments) LIKE UPPER('%" + filter.Comment + "%') ";
                }

                if (!string.IsNullOrEmpty(filter.DrivingLicense))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.PersonID IN (SELECT PersonID FROM PMIS_ADM.PersonDrivingLicenseCategories WHERE DrivingLicenseCategoryID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.DrivingLicense) + ") ) ";
                }

                if (!string.IsNullOrEmpty(filter.ServiceType))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.PotencialApplicantID IN ( SELECT PotApplID FROM PMIS_APPL.PotApplServiceTypes WHERE ServiceTypeID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.ServiceType) + ")) ";
                }

                if (filter.LastApperianceFrom.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.LastAppearance >= " + DBCommon.DateToDBCode(filter.LastApperianceFrom.Value) + " ";
                }

                if (filter.LastApperianceTo.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.LastAppearance < " + DBCommon.DateToDBCode(filter.LastApperianceTo.Value.AddDays(1)) + " ";
                }

                where = (where == "" ? "" : " WHERE ") + where;

                string pageWhere = "";

                if (filter.PageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE RowNumber BETWEEN (" + filter.PageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + filter.PageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

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
                        orderBySQL = "b.IME " + orderByDir + ", b.FAM";
                        break;
                    case 2:
                        orderBySQL = "b.EGN";
                        break;
                    case 3:
                        orderBySQL = "k.MilitaryDepartmentName";
                        break;
                    case 4:
                        orderBySQL = "c.LastModifiedDate";
                        break;
                    case 5:
                        orderBySQL = "a.LastAppearance";
                        break;
                    case 6:
                        orderBySQL = "PMIS_ADM.COMMONFUNCTIONS.GetDrivingLicensesPerPerson(a.PersonID)";
                        break;
                    case 7:
                        orderBySQL = "a.Comments";
                        break;
                    case 8:
                        orderBySQL = "PMIS_APPL.APPL_Functions.GetServiceTypesPerPotAppl(a.PotencialApplicantID)";
                        break;
                    default:
                        orderBySQL = "b.IME " + orderByDir + ", b.FAM";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                //                string SQL = @"SELECT * FROM (
                //                                  SELECT a.PotencialApplicantID, b.PersonID, d.MilitaryDepartmentID,
                //                                         a.Comments, a.LastAppearance, 
                //                                         a.CreatedBy, a.CreatedDate, a.LastModifiedBy, a.LastModifiedDate,
                //                                         RANK() OVER (ORDER BY " + orderBySQL + @", a.PotencialApplicantID) as RowNumber 
                //                                  FROM PMIS_APPL.PotencialApplicants a
                //                                  JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                //                                  LEFT OUTER JOIN PMIS_ADM.Persons c ON b.PersonID = c.PersonID
                //                                  JOIN PMIS_ADM.MilitaryDepartments d ON a.MilitaryDepartmentID = d.MilitaryDepartmentID
                //                                  " + where + @"    
                //                                  ORDER BY " + orderBySQL + @", PotencialApplicantID                             
                //                               ) tmp
                //                               " + pageWhere;

                string SQL = @"SELECT * FROM (
                                  SELECT a.PotencialApplicantID, b.PersonID, a.MilitaryDepartmentID,
                                         a.Comments, a.LastAppearance, 
                                         a.CreatedBy, a.CreatedDate, a.LastModifiedBy, a.LastModifiedDate, PMIS_ADM.COMMONFUNCTIONS.GetDrivingLicensesPerPerson(a.PersonID) as DrivingLicenses,
                                         PMIS_APPL.APPL_Functions.GetServiceTypesPerPotAppl(a.PotencialApplicantID) as ServiceTypes,
                                         RANK() OVER (ORDER BY " + orderBySQL + @", a.PotencialApplicantID) as RowNumber 
                                  FROM PMIS_APPL.PotencialApplicants a
                                  JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                                  LEFT OUTER JOIN PMIS_ADM.Persons c ON b.PersonID = c.PersonID
                                  --LEFT OUTER JOIN PMIS_APPL.PotApplServiceTypes d ON d.PotApplID = a.PotencialApplicantID
                                  JOIN PMIS_ADM.MilitaryDepartments k ON a.MilitaryDepartmentID = k.MilitaryDepartmentID 
                                  " + where + @"    
                                  ORDER BY " + orderBySQL + @", PotencialApplicantID                             
                               ) tmp
                               " + pageWhere;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    potencialApplicant = ExtractPotencialApplicantFromDataReader(dr, currentUser);

                    BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, potencialApplicant);

                    listPotencialApplicants.Add(potencialApplicant);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listPotencialApplicants;
        }

        public static int GetAllPotencialApplicantsCount(PotencialApplicantsFilter filter, User currentUser)
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

                if (filter.MilitaryDepartmentId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MilitaryDepartmentID = " + filter.MilitaryDepartmentId.Value + " ";
                }

                if (!string.IsNullOrEmpty(filter.Comment))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(a.Comments) LIKE UPPER('%" + filter.Comment + "%') ";
                }

                if (!string.IsNullOrEmpty(filter.DrivingLicense))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.PersonID IN (SELECT PersonID FROM PMIS_ADM.PersonDrivingLicenseCategories WHERE DrivingLicenseCategoryID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.DrivingLicense) + ") ) ";
                }

                if (!string.IsNullOrEmpty(filter.ServiceType))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.PotencialApplicantID IN ( SELECT PotApplID FROM PMIS_APPL.PotApplServiceTypes WHERE ServiceTypeID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.ServiceType) + ")) ";
                }

                if (filter.LastApperianceFrom.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.LastAppearance >= " + DBCommon.DateToDBCode(filter.LastApperianceFrom.Value) + " ";
                }

                if (filter.LastApperianceTo.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.LastAppearance < " + DBCommon.DateToDBCode(filter.LastApperianceTo.Value.AddDays(1)) + " ";
                }
                
                where = (where == "" ? "" : " AND ") + where;

                string SQL = @"SELECT COUNT(*) as Cnt
                               FROM PMIS_APPL.PotencialApplicants a
                               JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                               LEFT OUTER JOIN PMIS_ADM.Persons c ON b.PersonID = c.PersonID
                               JOIN PMIS_ADM.MilitaryDepartments d ON a.MilitaryDepartmentID = d.MilitaryDepartmentID 
                               WHERE a.MilitaryDepartmentID in (Select d.MilitaryDepartmentId from PMIS_ADM.MILITARYDEPARTMENTSPERUSER d where d.Userid = " + currentUser.UserId + @") 
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

        public static bool SavePotencialApplicant(PotencialApplicant potencialApplicant, User currentUser, Change changeEntry)
        {
           // Person person = potencialApplicant.Person;

            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";
                if (potencialApplicant.PotencialApplicantId == 0)
                {
                    SQL += @"INSERT INTO PMIS_APPL.PotencialApplicants (PersonID, MilitaryDepartmentID,
                                  Comments, LastAppearance, CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate)
                            VALUES (:PersonID, :MilitaryDepartmentID, :Comments, :LastAppearance, 
                                   :CreatedBy, :CreatedDate, :LastModifiedBy, :LastModifiedDate);

                            SELECT PMIS_APPL.PotApplicants_ID_SEQ.currval INTO :PotencialApplicantID FROM dual;

                            ";

                    //Create obect using log for INSERT records
                    changeEvent = new ChangeEvent("APPL_PotencialApplicants_Add", "", null, potencialApplicant.Person, currentUser);

                    //Fill object with data
                    changeEvent.AddDetail(new ChangeEventDetail("APPL_PotencialApplicants_Comments", "",
                                    potencialApplicant.Comments, currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("APPL_PotencialApplicants_LastApperiance", "",
                        CommonFunctions.FormatDate(potencialApplicant.LastAppearance.ToString()), currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("APPL_PotencialApplicants_ServiceType", "",
                        potencialApplicant.ServiceTypesString, currentUser));

                    changeEvent.AddDetail(new ChangeEventDetail("APPL_PotencialApplicants_MilitaryTrainingCourse", "",
                        potencialApplicant.MilitaryTrainingCoursesString, currentUser));

                }
                else
                {
                    SQL += @"UPDATE PMIS_APPL.PotencialApplicants SET
                               PersonID = :PersonID, 
                               MilitaryDepartmentID = :MilitaryDepartmentID,
                               Comments = :Comments,
                               LastAppearance = :LastAppearance,
                               LastModifiedBy = CASE WHEN :AnyActualChanges = 1 
                                                     THEN :LastModifiedBy
                                                     ELSE LastModifiedBy
                                                END, 
                               LastModifiedDate = CASE WHEN :AnyActualChanges = 1 
                                                       THEN :LastModifiedDate
                                                       ELSE LastModifiedDate
                                                  END

                            WHERE PotencialApplicantID = :PotencialApplicantID ;                       

                            ";

                    //Create obect using log for UPDATE records
                    changeEvent = new ChangeEvent("APPL_PotencialApplicants_Edit", "", null, potencialApplicant.Person, currentUser);

                    PotencialApplicant oldPotencialApplicant = GetPotencialApplicant(potencialApplicant.PotencialApplicantId, currentUser);

                    if (oldPotencialApplicant.Comments.Trim() !=
                           potencialApplicant.Comments.Trim())
                    {

                        changeEvent.AddDetail(new ChangeEventDetail("APPL_PotencialApplicants_Comments",
                                      oldPotencialApplicant.Comments,
                                      potencialApplicant.Comments, currentUser));
                    }

                    if (oldPotencialApplicant.LastAppearance !=
                        potencialApplicant.LastAppearance)
                    {

                        changeEvent.AddDetail(new ChangeEventDetail("APPL_PotencialApplicants_LastApperiance",
                            CommonFunctions.FormatDate(oldPotencialApplicant.LastAppearance.ToString()),
                            CommonFunctions.FormatDate(potencialApplicant.LastAppearance.ToString()), currentUser));

                    }

                    if (oldPotencialApplicant.ServiceTypesString.Trim() !=
                        potencialApplicant.ServiceTypesString.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("APPL_PotencialApplicants_ServiceType",
                            oldPotencialApplicant.ServiceTypesString, potencialApplicant.ServiceTypesString, currentUser));
                    }

                    if (oldPotencialApplicant.MilitaryTrainingCoursesString.Trim() !=
                        potencialApplicant.MilitaryTrainingCoursesString.Trim())
                    {
                        changeEvent.AddDetail(new ChangeEventDetail("APPL_PotencialApplicants_MilitaryTrainingCourse", 
                            oldPotencialApplicant.MilitaryTrainingCoursesString, potencialApplicant.MilitaryTrainingCoursesString, currentUser));
                    }
                }

                SQL += @"DELETE FROM PMIS_APPL.PotApplServiceTypes 
                         WHERE PotApplID = :PotencialApplicantID;
                        ";

                foreach (ServiceType serviceType in potencialApplicant.ServiceTypes)
                {
                    SQL += @"INSERT INTO PMIS_APPL.PotApplServiceTypes (PotApplID, ServiceTypeID)
                             VALUES (:PotencialApplicantID, " + serviceType.ServiceTypeID.ToString() + @");
                            ";
                }

                SQL += @"DELETE FROM PMIS_APPL.PotApplMilTrainingCourses 
                         WHERE PotApplID = :PotencialApplicantID;
                        ";

                foreach (MilitaryTrainingCourse militaryTrainingCourse in potencialApplicant.MilitaryTrainingCourses)
                {
                    SQL += @"INSERT INTO PMIS_APPL.PotApplMilTrainingCourses (PotApplID, MilTrainingCourseID)
                             VALUES (:PotencialApplicantID, " + militaryTrainingCourse.MilitaryTrainingCourseID.ToString() + @");
                            ";
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramApplicantID = new OracleParameter();
                paramApplicantID.ParameterName = "PotencialApplicantID";
                paramApplicantID.OracleType = OracleType.Number;

                if (potencialApplicant.PotencialApplicantId != 0)
                {
                    paramApplicantID.Direction = ParameterDirection.Input;
                    paramApplicantID.Value = potencialApplicant.PotencialApplicantId;
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
                param.Value = potencialApplicant.PersonId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryDepartmentID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = potencialApplicant.MilitaryDepartmentId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Comments";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.VarChar;
                if (!String.IsNullOrEmpty(potencialApplicant.Comments))
                {
                    param.Value = potencialApplicant.Comments;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "LastAppearance";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.DateTime;
                if (potencialApplicant.LastAppearance.HasValue)
                {
                    param.Value = potencialApplicant.LastAppearance;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);


                if (potencialApplicant.PotencialApplicantId == 0)
                {
                    BaseDbObjectUtil.SetCreatedParams(cmd, currentUser);
                }
                else
                {
                    BaseDbObjectUtil.SetAnyActualChanges(cmd, changeEvent);
                }

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();

                if (potencialApplicant.PotencialApplicantId == 0)
                {
                    potencialApplicant.PotencialApplicantId = DBCommon.GetInt(paramApplicantID.Value);

                }

                result = true;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                if(changeEvent.ChangeEventDetails.Count>0)
                changeEntry.AddEvent(changeEvent);
            }

            return result;
        }

        public static bool DeletePotencialApplicant(PotencialApplicant potencialApplicant, User currentUser, Change changeEntry)
        {
            Person person = potencialApplicant.Person;
            string SQL = "";
            bool isDeleted = false;

            //Create Old object 
            PotencialApplicant oldPtencialApplicant = GetPotencialApplicant(potencialApplicant.PotencialApplicantId, currentUser);

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();
            try
            {
                SQL = @"BEGIN
                            DELETE FROM PMIS_APPL.PotApplServiceTypes WHERE PotApplID = :PotencialApplicantID;
                            DELETE FROM PMIS_APPL.PotApplMilTrainingCourses WHERE PotApplID = :PotencialApplicantID;

                            DELETE FROM PMIS_APPL.PotencialApplicants WHERE PotencialApplicantID = :PotencialApplicantID;
                        END;";

                SQL = DBCommon.FixNewLines(SQL);
                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "PotencialApplicantID";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.Number;
                param.Value = potencialApplicant.PotencialApplicantId;
                cmd.Parameters.Add(param);

                isDeleted = cmd.ExecuteNonQuery() == 1;
            }
            finally
            {
                conn.Close();
            }
            if (isDeleted)
            {
                ChangeEvent changeEvent = new ChangeEvent("APPL_PotencialApplicants_Delete", "", null, person, currentUser);

                if (!String.IsNullOrEmpty(oldPtencialApplicant.Comments))
                {
                    changeEvent.AddDetail(new ChangeEventDetail("APPL_PotencialApplicants_Comments",
                                 oldPtencialApplicant.Comments, "", currentUser));
                }

                if (oldPtencialApplicant.LastAppearance.HasValue)
                {
                    changeEvent.AddDetail(new ChangeEventDetail("APPL_PotencialApplicants_LastApperiance",
                       CommonFunctions.FormatDate(oldPtencialApplicant.LastAppearance.ToString()), "", currentUser));
                }

                if (!String.IsNullOrEmpty(oldPtencialApplicant.ServiceTypesString))
                {
                    changeEvent.AddDetail(new ChangeEventDetail("APPL_PotencialApplicants_ServiceType",
                       oldPtencialApplicant.ServiceTypesString, "", currentUser));
                }

                if (!String.IsNullOrEmpty(oldPtencialApplicant.MilitaryTrainingCoursesString))
                {
                    changeEvent.AddDetail(new ChangeEventDetail("APPL_PotencialApplicants_MilitaryTrainingCourse",
                       oldPtencialApplicant.MilitaryTrainingCoursesString, "", currentUser));
                }
                changeEntry.AddEvent(changeEvent);
            }
            return isDeleted;
        }

    }

    public class ServiceType : BaseDbObject
    {
        private int serviceTypeID;
        private string serviceTypeName;

        public int ServiceTypeID
        {
            get
            {
                return serviceTypeID;
            }
            set
            {
                serviceTypeID = value;
            }
        }

        public string ServiceTypeName
        {
            get
            {
                return serviceTypeName;
            }
            set
            {
                serviceTypeName = value;
            }
        }

        public ServiceType(User user)
            : base(user)
        {
        }
    }

    //This class provides some methods for working with ServiceType objects
    public static class ServiceTypeUtil
    {
        //This method extracts a new object with type of ServiceType from a particular data reader
        //It is defined as a separate method to be reused easier
        public static ServiceType ExtractServiceTypeFromDataReader(OracleDataReader dr, User currentUser)
        {
            int? serviceTypeID = null;

            if (DBCommon.IsInt(dr["ServiceTypeID"]))
                serviceTypeID = DBCommon.GetInt(dr["ServiceTypeID"]);

            string serviceTypeName = dr["ServiceTypeName"].ToString();

            ServiceType serviceType = new ServiceType(currentUser);

            if (serviceTypeID.HasValue)
            {
                serviceType.ServiceTypeID = serviceTypeID.Value;
                serviceType.ServiceTypeName = serviceTypeName;
            }

            return serviceType;
        }
        
        public static List<ServiceType> GetAllServiceTypes(User currentUser)
        {
            List<ServiceType> listServiceTypes = new List<ServiceType>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.ServiceTypeID AS ServiceTypeID, a.ServiceTypeName AS ServiceTypeName
                               FROM PMIS_APPL.ServiceTypes a
                               WHERE a.Active = 1
                               ORDER BY a.ServiceTypeName";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["ServiceTypeID"]))
                    {
                        ServiceType serviceType = ExtractServiceTypeFromDataReader(dr, currentUser);
                        listServiceTypes.Add(serviceType);
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listServiceTypes;
        }

        public static List<ServiceType> GetServiceTypeNamesByServiceTypeIDs(string listServiceTypeID, User currentUser)
        {
            List<ServiceType> listServiceType = new List<ServiceType>();

            if (listServiceTypeID == string.Empty) return listServiceType;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.ServiceTypeID, a.ServiceTypeName
                              FROM PMIS_APPL.ServiceTypes a
                              WHERE a.ServiceTypeID IN (" + listServiceTypeID + @")
                              ORDER BY a.ServiceTypeName";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["ServiceTypeID"]))
                    {
                        ServiceType serviceType = new ServiceType(currentUser);
                        serviceType = ExtractServiceTypeFromDataReader(dr, currentUser);
                        listServiceType.Add(serviceType);
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }
            return listServiceType;
        }
              
        //Return a list of all ServiceType records of specific  potential applicant 
        public static List<ServiceType> GetAssignedServiceTypes(int potencialApplicantID, User currentUser)
        {
            List<ServiceType> listServiceTypes = new List<ServiceType>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.ServiceTypeID AS ServiceTypeID, a.ServiceTypeName AS ServiceTypeName
                               FROM PMIS_APPL.ServiceTypes a
                               INNER JOIN PMIS_APPL.PotApplServiceTypes b ON b.ServiceTypeID = a.ServiceTypeID
                               WHERE b.PotApplID = :PotencialApplicantID AND 
                                     a.Active = 1
                               ORDER BY a.ServiceTypeName";
                                
                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PotencialApplicantID", OracleType.Number).Value = potencialApplicantID;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["ServiceTypeID"]))
                    {
                        ServiceType serviceType = ExtractServiceTypeFromDataReader(dr, currentUser);
                        listServiceTypes.Add(serviceType);
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listServiceTypes;
        }
    }

    public class MilitaryTrainingCourse : BaseDbObject
    {
        private int militaryTrainingCourseID;
        private string militaryTrainingCourseName;

        public int MilitaryTrainingCourseID
        {
            get
            {
                return militaryTrainingCourseID;
            }
            set
            {
                militaryTrainingCourseID = value;
            }
        }

        public string MilitaryTrainingCourseName
        {
            get
            {
                return militaryTrainingCourseName;
            }
            set
            {
                militaryTrainingCourseName = value;
            }
        }

        public MilitaryTrainingCourse(User user)
            : base(user)
        {
        }
    }

    //This class provides some methods for working with MilitaryTrainingCourse objects
    public static class MilitaryTrainingCourseUtil
    {
        //This method extracts a new object with type of MilitaryTrainingCourse from a particular data reader
        //It is defined as a separate method to be reused easier
        public static MilitaryTrainingCourse ExtractMilitaryTrainingCourseFromDataReader(OracleDataReader dr, User currentUser)
        {
            int? militaryTrainingCourseID = null;

            if (DBCommon.IsInt(dr["MilitaryTrainingCourseID"]))
                militaryTrainingCourseID = DBCommon.GetInt(dr["MilitaryTrainingCourseID"]);

            string militaryTrainingCourseName = dr["MilitaryTrainingCourseName"].ToString();

            MilitaryTrainingCourse militaryTrainingCourse = new MilitaryTrainingCourse(currentUser);

            if (militaryTrainingCourseID.HasValue)
            {
                militaryTrainingCourse.MilitaryTrainingCourseID = militaryTrainingCourseID.Value;
                militaryTrainingCourse.MilitaryTrainingCourseName = militaryTrainingCourseName;
            }

            return militaryTrainingCourse;
        }

        public static List<MilitaryTrainingCourse> GetAllMilitaryTrainingCourses(User currentUser)
        {
            List<MilitaryTrainingCourse> listMilitaryTrainingCourses = new List<MilitaryTrainingCourse>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.MilTrainingCourseID AS MilitaryTrainingCourseID, a.MilTrainingCourseName AS MilitaryTrainingCourseName
                               FROM PMIS_APPL.MilTrainingCourses a
                               WHERE a.Active = 1
                               ORDER BY a.MilTrainingCourseName ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["MilitaryTrainingCourseID"]))
                    {
                        MilitaryTrainingCourse militaryTrainingCourse = ExtractMilitaryTrainingCourseFromDataReader(dr, currentUser);
                        listMilitaryTrainingCourses.Add(militaryTrainingCourse);
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listMilitaryTrainingCourses;
        }

        //Return a list of all MilitaryTrainingCourse records of specific  potential applicant 
        public static List<MilitaryTrainingCourse> GetAssignedMilitaryTrainingCourses(int potencialApplicantID, User currentUser)
        {                  
            List<MilitaryTrainingCourse> listMilitaryTrainingCourses = new List<MilitaryTrainingCourse>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.MilTrainingCourseID AS MilitaryTrainingCourseID, a.MilTrainingCourseName AS MilitaryTrainingCourseName
                               FROM PMIS_APPL.MilTrainingCourses a
                               INNER JOIN PMIS_APPL.PotApplMilTrainingCourses b ON b.MilTrainingCourseID = a.MilTrainingCourseID
                               WHERE b.PotApplID = :PotencialApplicantID AND
                                     a.Active = 1
                               ORDER BY a.MilTrainingCourseName";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PotencialApplicantID", OracleType.Number).Value = potencialApplicantID;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["MilitaryTrainingCourseID"]))
                    {
                        MilitaryTrainingCourse serviceType = ExtractMilitaryTrainingCourseFromDataReader(dr, currentUser);
                        listMilitaryTrainingCourses.Add(serviceType);
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listMilitaryTrainingCourses;
        }
    }
}