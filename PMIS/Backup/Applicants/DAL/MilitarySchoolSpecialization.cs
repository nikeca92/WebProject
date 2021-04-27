using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using PMIS.Common;
using System.Data;

namespace PMIS.Applicants.Common
{
    public class MilitarySchoolSpecialization : BaseDbObject
    {
        private int militarySchoolSpecializationId;
        private int militarySchoolId;
        private int year;
        private Specialization specialization;

        public int MilitarySchoolSpecializationId
        {
            get { return militarySchoolSpecializationId; }
            set { militarySchoolSpecializationId = value; }
        }

        public int MilitarySchoolId
        {
            get { return militarySchoolId; }
            set { militarySchoolId = value; }
        }

        public int Year
        {
            get { return year; }
            set { year = value; }
        }

        public Specialization Specialization
        {
            get { return specialization; }
            set { specialization = value; }
        }

        public bool CanDelete
        {
            get
            {
                return (CadetSchoolSubjectUtil.CountAllByMilitarySchoolSpecialization(militarySchoolSpecializationId, CurrentUser) == 0);
            }
        }

        public MilitarySchoolSpecialization(User user)
            : base(user)
        {

        }
    }

    //This class represents all information about the filter and the order information on the screen
    public class MilitarySchoolSpecializationFilter
    {
        private int militarySchoolId;
        private int year;
        private int orderBy;
        private int pageIndex;
        private int rowsPerPage;

        public int MilitarySchoolId
        {
            get { return militarySchoolId; }
            set { militarySchoolId = value; }
        }

        public int Year
        {
            get { return year; }
            set { year = value; }
        }

        public int OrderBy
        {
            get { return orderBy; }
            set { orderBy = value; }
        }

        public int PageIndex
        {
            get { return pageIndex; }
            set { pageIndex = value; }
        }

        public int RowsPerPage
        {
            get { return rowsPerPage; }
            set { rowsPerPage = value; }
        }
    }

    public class MilitarySchoolYear : IDropDownItem
    {
        private int year;
        private string yearValue;

        public int Year
        {
            get { return year; }
            set { year = value; }
        }

        public string YearValue
        {
            get { return yearValue; }
            set { yearValue = value; }
        }

        //IDropDownItem Members
        public string Text()
        {
            return YearValue;
        }

        public string Value()
        {
            return Year.ToString();
        }
    }

    public static class MilitarySchoolSpecializationUtil
    {
        public static MilitarySchoolSpecialization GetMilitarySchoolSpecialization(int militarySchoolSpecializationId, User currentUser)
        {
            MilitarySchoolSpecialization militarySchoolSpecialization = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.MilitarySchoolID, a.Year, b.SpecializationID, b.SpecializationName, c.VSP_IME as SubjectName
                                FROM PMIS_APPL.MilitarySchoolSpecializations a
                                INNER JOIN PMIS_APPL.Specializations b ON a.SpecializationID = b.SpecializationID
                                INNER JOIN VS_OWNER.KLV_VSP c ON b.MilitarySchoolSubjectID = c.VSPID
                                WHERE a.MilitSchoolSpecID = :MilitarySchoolSpecializationID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitarySchoolSpecializationID", OracleType.Number).Value = militarySchoolSpecializationId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    militarySchoolSpecialization = new MilitarySchoolSpecialization(currentUser);
                    militarySchoolSpecialization.MilitarySchoolSpecializationId = militarySchoolSpecializationId;
                    militarySchoolSpecialization.MilitarySchoolId = DBCommon.GetInt(dr["MilitarySchoolID"]);
                    militarySchoolSpecialization.Year = DBCommon.GetInt(dr["Year"]);

                    militarySchoolSpecialization.Specialization = new Specialization()
                    {
                        SpecializationId = DBCommon.GetInt(dr["SpecializationID"]),
                        SpecializationName = dr["SpecializationName"].ToString(),
                        MilitarySchoolSubject = new MilitarySchoolSubject() 
                        {
                            MilitarySchoolSubjectName = dr["SubjectName"].ToString()
                        }
                    };
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militarySchoolSpecialization;
        }

        public static List<MilitarySchoolSpecialization> GetAllMilitarySchoolSpecializations(User currentUser)
        {
            List<MilitarySchoolSpecialization> militarySchoolSpecializations = new List<MilitarySchoolSpecialization>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.MilitSchoolSpecID, a.MilitarySchoolID, a.Year, 
                                    b.SpecializationID, b.SpecializationName, c.VSP_IME as SubjectName
                                FROM PMIS_APPL.MilitarySchoolSpecializations a
                                INNER JOIN PMIS_APPL.Specializations b ON a.SpecializationID = b.SpecializationID
                                INNER JOIN VS_OWNER.KLV_VSP c ON b.MilitarySchoolSubjectID = c.VSPID
                               ORDER BY a.MilitSchoolSpecID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    MilitarySchoolSpecialization militarySchoolSpecialization = new MilitarySchoolSpecialization(currentUser);
                    militarySchoolSpecialization.MilitarySchoolSpecializationId = DBCommon.GetInt(dr["MilitSchoolSpecID"]);
                    militarySchoolSpecialization.MilitarySchoolId = DBCommon.GetInt(dr["MilitarySchoolID"]);
                    militarySchoolSpecialization.Year = DBCommon.GetInt(dr["Year"]);

                    militarySchoolSpecialization.Specialization = new Specialization()
                    {
                        SpecializationId = DBCommon.GetInt(dr["SpecializationID"]),
                        SpecializationName = dr["SpecializationName"].ToString(),
                        MilitarySchoolSubject = new MilitarySchoolSubject()
                        {
                            MilitarySchoolSubjectName = dr["SubjectName"].ToString()
                        }
                    };

                    militarySchoolSpecializations.Add(militarySchoolSpecialization);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militarySchoolSpecializations;
        }

        public static List<MilitarySchoolSpecialization> GetAllMilitarySchoolSpecializationsByFilter(MilitarySchoolSpecializationFilter filter, int? personId, User currentUser)
        {
            List<MilitarySchoolSpecialization> militarySchoolSpecializations = new List<MilitarySchoolSpecialization>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();

                string pageWhere = "";

                if (filter.PageIndex > 0 && filter.RowsPerPage > 0)
                    pageWhere = " WHERE RowNumber BETWEEN (" + filter.PageIndex.ToString() + @" - 1) * " + filter.RowsPerPage.ToString() + @" + 1 AND " + filter.PageIndex.ToString() + @" * " + filter.RowsPerPage.ToString() + @" ";

                string where = @"
                                WHERE a.MilitarySchoolID = " + filter.MilitarySchoolId.ToString() + " AND a.Year = " + filter.Year.ToString();

                if (personId != null && personId > 0)
                {
                    where += @"
                                AND a.MilitSchoolSpecID NOT IN (SELECT a.MilitSchoolSpecID
                                     FROM PMIS_APPL.MilitarySchoolSpecializations a
                                     INNER JOIN PMIS_APPL.Specializations b ON a.SpecializationID = b.SpecializationID
                                     INNER JOIN VS_OWNER.KLV_VSP c ON b.MilitarySchoolSubjectID = c.VSPID
                                     INNER JOIN PMIS_APPL.CadetSchoolSubjects d ON a.MilitSchoolSpecID = d.MilitSchoolSpecID
                                     INNER JOIN PMIS_APPL.Cadets e ON e.CadetID = d.CadetID"
                                        + where + 
                                    @" AND e.PersonID = " + personId + @")
                                ";
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
                        orderBySQL = "c.VSP_IME";
                        break;
                    case 2:
                        orderBySQL = "b.SpecializationName";
                        break;
                    default:
                        orderBySQL = "c.VSP_IME";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                    SELECT a.MilitSchoolSpecID, a.MilitarySchoolID, a.Year,
                                        b.SpecializationID, b.SpecializationName, c.VSP_IME as SubjectName,
                                        RANK() OVER (ORDER BY " + orderBySQL + @", a.MilitSchoolSpecID) as RowNumber 
                                     FROM PMIS_APPL.MilitarySchoolSpecializations a
                                     INNER JOIN PMIS_APPL.Specializations b ON a.SpecializationID = b.SpecializationID
                                     INNER JOIN VS_OWNER.KLV_VSP c ON b.MilitarySchoolSubjectID = c.VSPID"
                                    + where +
                                    @"
                                        ORDER BY " + orderBySQL + @", a.MilitSchoolSpecID
                                ) tmp
                                " + pageWhere;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    MilitarySchoolSpecialization militarySchoolSpecialization = new MilitarySchoolSpecialization(currentUser);
                    militarySchoolSpecialization.MilitarySchoolSpecializationId = DBCommon.GetInt(dr["MilitSchoolSpecID"]);
                    militarySchoolSpecialization.MilitarySchoolId = DBCommon.GetInt(dr["MilitarySchoolID"]);
                    militarySchoolSpecialization.Year = DBCommon.GetInt(dr["Year"]);

                    militarySchoolSpecialization.Specialization = new Specialization()
                    {
                        SpecializationId = DBCommon.GetInt(dr["SpecializationID"]),
                        SpecializationName = dr["SpecializationName"].ToString(),
                        MilitarySchoolSubject = new MilitarySchoolSubject()
                        {
                            MilitarySchoolSubjectName = dr["SubjectName"].ToString()
                        }
                    };

                    militarySchoolSpecializations.Add(militarySchoolSpecialization);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militarySchoolSpecializations;
        }

        public static int GetAllMilitarySchoolSpecializationsByFilterCount(MilitarySchoolSpecializationFilter filter, int? personId, User currentUser)
        {
            int specializationsCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();

                string where = @"
                                WHERE a.MilitarySchoolID = " + filter.MilitarySchoolId.ToString() + " AND a.Year = " + filter.Year.ToString();

                if (personId != null && personId > 0)
                {
                    where += @"
                                AND a.MilitSchoolSpecID NOT IN (SELECT a.MilitSchoolSpecID
                                     FROM PMIS_APPL.MilitarySchoolSpecializations a
                                     INNER JOIN PMIS_APPL.Specializations b ON a.SpecializationID = b.SpecializationID
                                     INNER JOIN VS_OWNER.KLV_VSP c ON b.MilitarySchoolSubjectID = c.VSPID
                                     INNER JOIN PMIS_APPL.CadetSchoolSubjects d ON a.MilitSchoolSpecID = d.MilitSchoolSpecID
                                     INNER JOIN PMIS_APPL.Cadets e ON e.CadetID = d.CadetID"
                                        + where +
                                    @" AND e.PersonID = " + personId + @")
                                ";
                }

                string SQL = @" SELECT COUNT(*) as Cnt
                                 FROM PMIS_APPL.MilitarySchoolSpecializations a
                                 INNER JOIN PMIS_APPL.Specializations b ON a.SpecializationID = b.SpecializationID
                                 INNER JOIN VS_OWNER.KLV_VSP c ON b.MilitarySchoolSubjectID = c.VSPID"
                                + where;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        specializationsCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return specializationsCnt;
        }

        public static List<int> GetAllYearsByMilitarySchoolID(int militarySchoolId, User currentUser)
        {
            List<int> years = new List<int>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT DISTINCT(a.Year)
                               FROM PMIS_APPL.MilitarySchoolSpecializations a
                               WHERE a.MilitarySchoolID = :MilitarySchoolID
                               ORDER BY a.Year";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitarySchoolID", OracleType.Number).Value = militarySchoolId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    int year = DBCommon.GetInt(dr["Year"]);

                    years.Add(year);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return years;
        }

        public static List<int> GetAllYearsForAllMilitarySchools(User currentUser)
        {
            List<int> years = new List<int>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT DISTINCT(a.Year)
                               FROM PMIS_APPL.MilitarySchoolSpecializations a
                               ORDER BY a.Year";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    int year = DBCommon.GetInt(dr["Year"]);

                    years.Add(year);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return years;
        }

        public static bool SaveMilitarySchoolSpecialization(MilitarySchoolSpecialization militarySchoolSpecialization, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            string logDescription = "";
            logDescription += "Военно училище: " + MilitarySchoolUtil.GetMilitarySchool(militarySchoolSpecialization.MilitarySchoolId, currentUser).MilitarySchoolName;

            Specialization specialization = SpecializationUtil.GetSpecialization(militarySchoolSpecialization.Specialization.SpecializationId, currentUser);

            logDescription += " / Специалност: " + specialization.MilitarySchoolSubject.MilitarySchoolSubjectName;
            logDescription += " / Специализация: " + specialization.SpecializationName;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN

                            INSERT INTO PMIS_APPL.MilitarySchoolSpecializations (MilitarySchoolID, Year, SpecializationID)
                            VALUES (:MilitarySchoolID, :Year, :SpecializationID);

                            SELECT PMIS_APPL.MilitSchoolSpecial_ID_SEQ.currval INTO :MilitSchoolSpecID FROM dual;
                        
                        END;";

                changeEvent = new ChangeEvent("APPL_MilitSchoolSpecializations_Add", logDescription, null, null, currentUser);

                changeEvent.AddDetail(new ChangeEventDetail("APPL_MilitSchoolSpecializations_SpecializationName", "", specialization.SpecializationName, currentUser));

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramMilitSchoolSpecID = new OracleParameter();
                paramMilitSchoolSpecID.ParameterName = "MilitSchoolSpecID";
                paramMilitSchoolSpecID.OracleType = OracleType.Number;

                if (militarySchoolSpecialization.MilitarySchoolSpecializationId != 0)
                {
                    paramMilitSchoolSpecID.Direction = ParameterDirection.Input;
                    paramMilitSchoolSpecID.Value = militarySchoolSpecialization.MilitarySchoolSpecializationId;
                }
                else
                {
                    paramMilitSchoolSpecID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramMilitSchoolSpecID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "MilitarySchoolID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = militarySchoolSpecialization.MilitarySchoolId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Year";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = militarySchoolSpecialization.Year;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "SpecializationID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = militarySchoolSpecialization.Specialization.SpecializationId;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (militarySchoolSpecialization.MilitarySchoolSpecializationId == 0)
                    militarySchoolSpecialization.MilitarySchoolSpecializationId = DBCommon.GetInt(paramMilitSchoolSpecID.Value);

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

        public static bool DeleteMilitarySchoolSpecialization(int militarySchoolSpecializationId, User currentUser, Change changeEntry)
        {

            bool result = false;

            MilitarySchoolSpecialization oldMilitarySchoolSpecialization = GetMilitarySchoolSpecialization(militarySchoolSpecializationId, currentUser);

            string logDescription = "";
            logDescription += "Военно училище: " + MilitarySchoolUtil.GetMilitarySchool(oldMilitarySchoolSpecialization.MilitarySchoolId, currentUser).MilitarySchoolName;

            Specialization specialization = SpecializationUtil.GetSpecialization(oldMilitarySchoolSpecialization.Specialization.SpecializationId, currentUser);

            logDescription += " / Специалност: " + specialization.MilitarySchoolSubject.MilitarySchoolSubjectName;
            logDescription += " / Специализация: " + specialization.SpecializationName;

            ChangeEvent changeEvent = new ChangeEvent("APPL_MilitSchoolSpecializations_Delete", logDescription, null, null, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("APPL_MilitSchoolSpecializations_SpecializationName", "", specialization.SpecializationName, currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = "DELETE FROM PMIS_APPL.MilitarySchoolSpecializations WHERE MilitSchoolSpecID = :MilitarySchoolSpecializationID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitarySchoolSpecializationID", OracleType.Number).Value = militarySchoolSpecializationId;

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

        public static List<MilitarySchoolYear> GetAllYearsByPersonID(int personId, User currentUser)
        {
            List<MilitarySchoolYear> years = new List<MilitarySchoolYear>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT DISTINCT(a.Year)
                               FROM PMIS_APPL.MilitarySchoolSpecializations a
                               INNER JOIN PMIS_APPL.CadetSchoolSubjects b ON a.MilitSchoolSpecID = b.MilitSchoolSpecID
                               INNER JOIN PMIS_APPL.Cadets c ON b.CadetID = c.CadetID
                               WHERE c.PersonID = :PersonID
                               ORDER BY a.Year";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Number).Value = personId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    MilitarySchoolYear militSchoolYear = new MilitarySchoolYear() 
                    {
                        Year = DBCommon.GetInt(dr["Year"]),
                        YearValue = dr["Year"].ToString()
                    };

                    years.Add(militSchoolYear);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return years;
        }


    }
}
