using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Applicants.Common
{
    //Each cadet is identified as a particular Person and MilitaryDepartment combination
    public class Cadet : BaseDbObject
    {
        private int cadetId;
        private int personId;
        private Person person;
        private int militaryDepartmentId;
        private MilitaryDepartment militaryDepartment;
        private List<CadetSchoolSubject> cadetSchoolSubjects;

        public int CadetId
        {
            get { return cadetId; }
            set { cadetId = value; }
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

        public List<CadetSchoolSubject> CadetSchoolSubjects
        {
            get
            {
                if (cadetSchoolSubjects == null)
                    cadetSchoolSubjects = CadetSchoolSubjectUtil.GetAllCadetSchoolSubjectsByCadetID(cadetId, CurrentUser);

                return cadetSchoolSubjects;
            }
            set
            {
                cadetSchoolSubjects = value;
            }
        }

        public bool CanDelete
        {
            get
            {
                if (CadetSchoolSubjects.Count > 0)
                    return false;
                else
                    return true;
            }
        }

        public Cadet(User user) :base(user)
        {

        }
    }

    public static class CadetUtil
    {
        //This method creates and returns a Cadet object. It extracts the data from a DataReader.
        public static Cadet ExtractCadetFromDataReader(OracleDataReader dr, User currentUser)
        {
            Cadet cadet = new Cadet(currentUser);

            if (DBCommon.IsInt(dr["CadetID"]))
                cadet.CadetId = DBCommon.GetInt(dr["CadetID"]);

            if (DBCommon.IsInt(dr["PersonID"]))
                cadet.PersonId = DBCommon.GetInt(dr["PersonID"]);

            if (DBCommon.IsInt(dr["MilitaryDepartmentID"]))
                cadet.MilitaryDepartmentId = DBCommon.GetInt(dr["MilitaryDepartmentID"]);

            BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, cadet);

            return cadet;
        }

        public static Cadet GetCadet(int cadetId, User currentUser)
        {
            Cadet cadet = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string andWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("APPL_CADETS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    andWhere = " AND a.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.CadetID, b.PersonID, d.MilitaryDepartmentID,
                                    a.CreatedBy, a.CreatedDate, a.LastModifiedBy, a.LastModifiedDate
                                  FROM PMIS_APPL.Cadets a
                                  JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                                  LEFT OUTER JOIN PMIS_ADM.Persons c ON b.PersonID = c.PersonID
                                  JOIN PMIS_ADM.MilitaryDepartments d ON a.MilitaryDepartmentID = d.MilitaryDepartmentID
                                  WHERE a.CadetID = :CadetID " + andWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("CadetID", OracleType.Number).Value = cadetId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    cadet = ExtractCadetFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return cadet;
        }

        public static Cadet GetCadet(int personId, int militaryDepartmentId, User currentUser)
        {
            Cadet cadet = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string andWhere = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("APPL_CADETS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    andWhere = " AND a.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.CadetID, b.PersonID, d.MilitaryDepartmentID,
                                    a.CreatedBy, a.CreatedDate, a.LastModifiedBy, a.LastModifiedDate
                                  FROM PMIS_APPL.Cadets a
                                  JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID
                                  LEFT OUTER JOIN PMIS_ADM.Persons c ON b.PersonID = c.PersonID
                                  JOIN PMIS_ADM.MilitaryDepartments d ON a.MilitaryDepartmentID = d.MilitaryDepartmentID
                                  WHERE a.PersonID = :PersonID AND a.MilitaryDepartmentID = :MilitaryDepartmentID " + andWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Number).Value = personId;
                cmd.Parameters.Add("MilitaryDepartmentID", OracleType.Number).Value = militaryDepartmentId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    cadet = ExtractCadetFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return cadet;
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
                               FROM PMIS_APPL.Cadets a
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

        public static void SetCadetModified(int cadetId, User currentUser)
        {
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"UPDATE PMIS_APPL.Cadets SET
                                  LastModifiedBy = :LastModifiedBy,
                                  LastModifiedDate = :LastModifiedDate
                               WHERE CadetID = :CadetID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("CadetID", OracleType.Number).Value = cadetId;

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }

        //Save a particular object into the DB
        public static bool SaveCadet(Cadet cadet, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            string logDescription = "";
            logDescription += "Име: " + cadet.Person.FullName;
            logDescription += "<br />ЕГН: " + cadet.Person.IdentNumber;
            logDescription += "<br />Военно окръжие: " + cadet.MilitaryDepartment.MilitaryDepartmentName;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";
                if (cadet.CadetId == 0)
                {
                    SQL += @"INSERT INTO PMIS_APPL.Cadets (PersonID, MilitaryDepartmentID,
                                   CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate)
                            VALUES (:PersonID, :MilitaryDepartmentID,
                                   :CreatedBy, :CreatedDate, :LastModifiedBy, :LastModifiedDate);

                            SELECT PMIS_APPL.Cadets_ID_SEQ.currval INTO :CadetID FROM dual;

                            ";

                    changeEvent = new ChangeEvent("APPL_Cadets_Add", logDescription, null, cadet.Person, currentUser);
                }
                else
                {
                    SQL += @"UPDATE PMIS_APPL.Cadets SET
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

                            WHERE CadetID = :CadetID ;                       

                            ";

                    changeEvent = new ChangeEvent("APPL_Cadets_Edit", logDescription, null, cadet.Person, currentUser);
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramCadetID = new OracleParameter();
                paramCadetID.ParameterName = "CadetID";
                paramCadetID.OracleType = OracleType.Number;

                if (cadet.CadetId != 0)
                {
                    paramCadetID.Direction = ParameterDirection.Input;
                    paramCadetID.Value = cadet.CadetId;
                }
                else
                {
                    paramCadetID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramCadetID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "PersonID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = cadet.PersonId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryDepartmentID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = cadet.MilitaryDepartmentId;
                cmd.Parameters.Add(param);

                if (cadet.CadetId == 0)
                {
                    BaseDbObjectUtil.SetCreatedParams(cmd, currentUser);
                }
                else
                {
                    BaseDbObjectUtil.SetAnyActualChanges(cmd, changeEvent);
                }

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();

                if (cadet.CadetId == 0)
                    cadet.CadetId = DBCommon.GetInt(paramCadetID.Value);

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

        public static bool DeleteCadet(int cadetId, User currentUser, Change changeEntry)
        {

            bool result = false;

            Cadet cadet = GetCadet(cadetId, currentUser);

            string logDescription = "";
            logDescription += "Име: " + cadet.Person.FullName;
            logDescription += "<br />ЕГН: " + cadet.Person.IdentNumber;
            logDescription += "<br />Военно окръжие: " + cadet.MilitaryDepartment.MilitaryDepartmentName;

            ChangeEvent changeEvent = new ChangeEvent("APPL_Cadets_Delete", logDescription, null, cadet.Person, currentUser);

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" DECLARE Cnt NUMBER;
                                BEGIN
                                  SELECT COUNT(*)
                                  INTO Cnt
                                  FROM PMIS_APPL.CadetSchoolSubjects
                                  WHERE CadetID = :CadetID;
                                  
                                  IF Cnt = 0 then
                                    DELETE FROM PMIS_APPL.Cadets WHERE CadetID = :CadetID;
                                  END IF;
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("CadetID", OracleType.Number).Value = cadetId;

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
    public class CadetsSearchFilter
    {
        int? militarySchoolId;
        int? militaryDepartmentId;
        int? schoolYear;
        string identityNumber;
        int orderBy;
        int pageIdx;

        public int? MilitarySchoolId
        {
            get { return militarySchoolId; }
            set { militarySchoolId = value; }
        }

        public int? MilitaryDepartmentId
        {
            get { return militaryDepartmentId; }
            set { militaryDepartmentId = value; }
        }

        public int? SchoolYear
        {
            get { return schoolYear; }
            set { schoolYear = value; }
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

    //Each applicant search is identitified as a particular Person and MilitaryDepartment combination depend military school
    public class CadetSearch : BaseDbObject
    {
        private int cadetId;
        private int personId;
        private string personName;
        private string personIdentNumber;
        private int militarySchoolId;
        private string militarySchoolName;
        private int schoolYear;
        private int militaryDepartmentId;
        private string militaryDepartmentName;
        private DateTime lastModifiedDate;
        List<CadetSchoolSubject> cadetSchoolSubjects;

        public int CadetId
        {
            get { return cadetId; }
            set { cadetId = value; }
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

        public int MilitarySchoolId
        {
            get { return militarySchoolId; }
            set { militarySchoolId = value; }
        }

        public string MilitarySchoolName
        {
            get { return militarySchoolName; }
            set { militarySchoolName = value; }
        }

        public int SchoolYear
        {
            get { return schoolYear; }
            set { schoolYear = value; }
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

        public List<CadetSchoolSubject> CadetSchoolSubjects
        {
            get
            {
                if (cadetSchoolSubjects == null)
                    cadetSchoolSubjects = CadetSchoolSubjectUtil.GetAllCadetSchoolSubjectsByPersonID(personId, schoolYear, CurrentUser);

                return cadetSchoolSubjects;
            }
            set
            {
                cadetSchoolSubjects = value;
            }
        }

        public bool CanDelete
        {
            get
            {
                if (CadetSchoolSubjects.Count > 0)
                    return false;
                else
                    return true;
            }
        }

        public CadetSearch(User user)
            : base(user)
        {

        }
    }

    public static class CadetSearchUtil
    {
        public static List<CadetSearch> GetAllCadetsBySearch(CadetsSearchFilter filter, int rowsPerPage, User currentUser)
        {
            List<CadetSearch> cadetSearches = new List<CadetSearch>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("APPL_CADETS", currentUser, false, currentUser.Role.RoleId, null)[0];
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

                if (filter.MilitarySchoolId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " f.MilitarySchoolID = " + filter.MilitarySchoolId.Value + " ";
                }

                if (filter.MilitaryDepartmentId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " d.MilitaryDepartmentID = " + filter.MilitaryDepartmentId.Value + " ";
                }

                if (filter.SchoolYear.HasValue)
                {
                    where += (where == "" ? " " : " AND ");
                    where += @" f.Year = " + filter.SchoolYear.Value + " ";
                }

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
                        rankOrderBySQL = "b.IME " + orderByDir + ", b.FAM";
                        orderBySQL = "PersonName";
                        break;
                    case 2:
                        rankOrderBySQL = "b.EGN";
                        orderBySQL = "b.EGN";
                        break;
                    case 3:
                        rankOrderBySQL = "j.VVU_IME";
                        orderBySQL = "j.VVU_IME";
                        break;
                    case 4:
                        rankOrderBySQL = "f.Year";
                        orderBySQL = "f.Year";
                        break;
                    case 5:
                        rankOrderBySQL = "d.MilitaryDepartmentName";
                        orderBySQL = "d.MilitaryDepartmentName";
                        break;
                    case 6:
                        rankOrderBySQL = "a.LastModifiedDate";
                        orderBySQL = "a.LastModifiedDate";
                        break;
                    default:
                        rankOrderBySQL = "b.IME " + orderByDir + ", b.FAM";
                        orderBySQL = "PersonName";
                        break;
                }

                rankOrderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);
                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                SELECT a.CadetID, f.MilitarySchoolID, j.VVU_IME as MilitarySchoolName, 
                                    d.MilitaryDepartmentID, d.MilitaryDepartmentName,
                                    b.PersonID, b.IME || ' ' || b.FAM as PersonName, b.EGN as PersonIdentNumber, 
                                    f.Year, a.CreatedBy, a.CreatedDate, a.LastModifiedBy, a.LastModifiedDate,
                                    RANK() OVER (ORDER BY " + rankOrderBySQL + @", a.CadetID) as RowNumber
                                    FROM PMIS_APPL.Cadets a
                                    INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID 
                                    LEFT OUTER JOIN PMIS_ADM.Persons c ON b.PersonID = c.PersonID
                                    INNER JOIN PMIS_ADM.MilitaryDepartments d ON a.MilitaryDepartmentID = d.MilitaryDepartmentID  
                                    INNER JOIN PMIS_APPL.CadetSchoolSubjects e ON a.CadetID = e.CadetID
                                    INNER JOIN PMIS_APPL.MilitarySchoolSpecializations f ON e.MilitSchoolSpecID = f.MilitSchoolSpecID
                                    INNER JOIN VS_OWNER.KLV_VVU j ON f.MilitarySchoolID = j.VVUID
                                    " + where + @"
                                    ORDER BY " + orderBySQL + @", a.CadetID 
                                ) tmp
                                " + pageWhere;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    CadetSearch cadetSearch = new CadetSearch(currentUser);

                    cadetSearch.CadetId = DBCommon.GetInt(dr["CadetID"]);
                    cadetSearch.PersonId = DBCommon.GetInt(dr["PersonID"]);
                    cadetSearch.PersonName = dr["PersonName"].ToString();
                    cadetSearch.PersonIdentNumber = dr["PersonIdentNumber"].ToString();
                    cadetSearch.MilitarySchoolId = DBCommon.GetInt(dr["MilitarySchoolID"]);
                    cadetSearch.MilitarySchoolName = dr["MilitarySchoolName"].ToString();
                    cadetSearch.MilitaryDepartmentId = DBCommon.GetInt(dr["MilitaryDepartmentID"]);
                    cadetSearch.MilitaryDepartmentName = dr["MilitaryDepartmentName"].ToString();
                    cadetSearch.SchoolYear = DBCommon.GetInt(dr["Year"]);

                    if (dr["LastModifiedDate"] is DateTime)
                        cadetSearch.LastModifiedDate = (DateTime)dr["LastModifiedDate"];

                    BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, cadetSearch);

                    cadetSearches.Add(cadetSearch);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return cadetSearches;
        }

        public static int GetAllCadetsCount(CadetsSearchFilter filter, User currentUser)
        {
            int cadetsCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("APPL_CADETS", currentUser, false, currentUser.Role.RoleId, null)[0];
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

                if (filter.MilitarySchoolId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " f.MilitarySchoolID = " + filter.MilitarySchoolId.Value + " ";
                }

                if (filter.MilitaryDepartmentId.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " d.MilitaryDepartmentID = " + filter.MilitaryDepartmentId.Value + " ";
                }

                if (filter.SchoolYear.HasValue)
                {
                    where += (where == "" ? " " : " AND ");
                    where += @" f.Year = " + filter.SchoolYear.Value + " ";
                }

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @"SELECT COUNT(*) as Cnt
                                    FROM PMIS_APPL.Cadets a
                                    INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID 
                                    LEFT OUTER JOIN PMIS_ADM.Persons c ON b.PersonID = c.PersonID
                                    INNER JOIN PMIS_ADM.MilitaryDepartments d ON a.MilitaryDepartmentID = d.MilitaryDepartmentID  
                                    INNER JOIN PMIS_APPL.CadetSchoolSubjects e ON a.CadetID = e.CadetID
                                    INNER JOIN PMIS_APPL.MilitarySchoolSpecializations f ON e.MilitSchoolSpecID = f.MilitSchoolSpecID
                                    INNER JOIN VS_OWNER.KLV_VVU j ON f.MilitarySchoolID = j.VVUID
                               " + where + @"
                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        cadetsCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return cadetsCnt;
        }
    }

    public class CadetsRankingFilter
    {
        int militarySchoolId;
        int schoolYear;
        int specializationId;

        public int MilitarySchoolId
        {
            get { return militarySchoolId; }
            set { militarySchoolId = value; }
        }

        public int SchoolYear
        {
            get { return schoolYear; }
            set { schoolYear = value; }
        }

        public int SpecializationId
        {
            get { return specializationId; }
            set { specializationId = value; }
        }
    }

    public class CadetRankingSearch : BaseDbObject
    {
        private string personName;
        private string personIdentNumber;
        private CadetSchoolSubject cadetSchoolSubject;

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

        public CadetSchoolSubject CadetSchoolSubject
        {
            get { return cadetSchoolSubject; }
            set { cadetSchoolSubject = value; }
        }

        public CadetRankingSearch(User user)
            : base(user)
        {

        }
    }

    public static class CadetRankingSearchUtil
    {
        public static List<CadetRankingSearch> GetAllCadetsRankingSearch(CadetsRankingFilter filter, User currentUser)
        {
            List<CadetRankingSearch> cadetRankingSearches = new List<CadetRankingSearch>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                where = "WHERE f.VVUID = " + filter.MilitarySchoolId + " AND e.Year = " + filter.SchoolYear + @"
                            AND j.SpecializationID = " + filter.SpecializationId + " AND d.IsRanked = 1";

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("APPL_CADETS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += " AND a.CreatedBy = " + currentUser.UserId.ToString();
                }
                
                string SQL = @"SELECT a.CadetID, b.IME || ' ' || b.FAM as PersonName, b.EGN as PersonIdentNumber,
                                    d.CadetSchoolSubjectID, d.IsRanked, e.MilitSchoolSpecID 
                                  FROM PMIS_APPL.Cadets a
                                  INNER JOIN VS_OWNER.VS_LS b ON a.PersonID = b.PersonID 
                                  LEFT OUTER JOIN PMIS_ADM.Persons c ON b.PersonID = c.PersonID
                                  INNER JOIN PMIS_APPL.CadetSchoolSubjects d ON a.CadetID = d.CadetID
                                  INNER JOIN PMIS_APPL.MilitarySchoolSpecializations e ON d.MilitSchoolSpecID = e.MilitSchoolSpecID
                                  INNER JOIN VS_OWNER.KLV_VVU f ON e.MilitarySchoolID = f.VVUID
                                  INNER JOIN PMIS_APPL.Specializations j ON e.SpecializationID = j.SpecializationID
                                    " + where + @"
                                    ORDER BY PersonName, a.CadetID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    CadetRankingSearch cadetRankingSearch = new CadetRankingSearch(currentUser);

                    cadetRankingSearch.PersonName = dr["PersonName"].ToString();
                    cadetRankingSearch.PersonIdentNumber = dr["PersonIdentNumber"].ToString();
                    cadetRankingSearch.CadetSchoolSubject = new CadetSchoolSubject(currentUser) 
                    {
                        CadetSchoolSubjectId = DBCommon.GetInt(dr["CadetSchoolSubjectID"]),
                        CadetId = DBCommon.GetInt(dr["CadetID"]),
                        MilitSchoolSpecId = DBCommon.GetInt(dr["MilitSchoolSpecID"]),
                        IsRanked = (dr["IsRanked"].ToString() == "1" ? true : false)
                    };

                    cadetRankingSearches.Add(cadetRankingSearch);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return cadetRankingSearches;
        }
    }
}
