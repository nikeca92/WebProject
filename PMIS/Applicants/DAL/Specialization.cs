using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using PMIS.Common;

namespace PMIS.Applicants.Common
{
    public class Specialization
    {
        private int specializationId;
        private string specializationName;
        private MilitarySchoolSubject militarySchoolSubject;

        public int SpecializationId
        {
            get { return specializationId; }
            set { specializationId = value; }
        }

        public string SpecializationName
        {
            get { return specializationName; }
            set { specializationName = value; }
        }

        public MilitarySchoolSubject MilitarySchoolSubject
        {
            get { return militarySchoolSubject; }
            set { militarySchoolSubject = value; }
        }
    }

    //This class represents all information about the filter, the order and the paging information on the screen
    public class SpecializationFilter
    {
        int militarySchoolId;
        int year;
        string subjectName;
        string specializationName;
        int orderBy;
        int pageIdx;

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

        public string SubjectName
        {
            get { return subjectName; }
            set { subjectName = value; }
        }

        public string SpecializationName
        {
            get { return specializationName; }
            set { specializationName = value; }
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
    }

    public static class SpecializationUtil
    {
        public static Specialization GetSpecialization(int specializationId, User currentUser)
        {
            Specialization specialization = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.SpecializationName, a.MilitarySchoolSubjectID, b.VSP_IME as MilitarySchoolSubjectName
                               FROM PMIS_APPL.Specializations a
                               INNER JOIN VS_OWNER.KLV_VSP b ON a.MilitarySchoolSubjectID = b.VSPID
                               WHERE a.SpecializationID = :SpecializationID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("SpecializationID", OracleType.Number).Value = specializationId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    specialization = new Specialization();
                    specialization.SpecializationId = specializationId;
                    specialization.SpecializationName = dr["SpecializationName"].ToString();
                    specialization.MilitarySchoolSubject = new MilitarySchoolSubject() 
                    {
                        MilitarySchoolSubjectId = DBCommon.GetInt(dr["MilitarySchoolSubjectID"]),
                        MilitarySchoolSubjectName = dr["MilitarySchoolSubjectName"].ToString()
                    };
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return specialization;
        }

        public static List<Specialization> GetAllSpecializations(User currentUser)
        {
            List<Specialization> specializations = new List<Specialization>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.SpecializationID, a.SpecializationName, a.MilitarySchoolSubjectID, b.VSP_IME as MilitarySchoolSubjectName
                               FROM PMIS_APPL.Specializations a
                               INNER JOIN VS_OWNER.KLV_VSP b ON a.MilitarySchoolSubjectID = b.VSPID
                               ORDER BY a.SpecializationID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Specialization specialization = new Specialization();
                    specialization.SpecializationId = DBCommon.GetInt(dr["SpecializationID"]);
                    specialization.SpecializationName = dr["SpecializationName"].ToString();
                    specialization.MilitarySchoolSubject = new MilitarySchoolSubject()
                    {
                        MilitarySchoolSubjectId = DBCommon.GetInt(dr["MilitarySchoolSubjectID"]),
                        MilitarySchoolSubjectName = dr["MilitarySchoolSubjectName"].ToString()
                    };

                    specializations.Add(specialization);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return specializations;
        }

        public static List<Specialization> GetAllUnusedSpecsByMilitarySchoolID(SpecializationFilter filter, int rowsPerPage, User currentUser)
        {
            List<Specialization> specializations = new List<Specialization>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();

                string pageWhere = "";

                if (filter.PageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE RowNumber BETWEEN (" + filter.PageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + filter.PageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

                string where = "";

                if (!String.IsNullOrEmpty(filter.SubjectName))
                {
                    where += " AND UPPER(b.VSP_IME) LIKE '%" + filter.SubjectName.Replace("'", "''").ToUpper() + @"%' ";
                }

                if (!String.IsNullOrEmpty(filter.SpecializationName))
                {
                    where += " AND UPPER(a.SpecializationName) LIKE '%" + filter.SpecializationName.Replace("'", "''").ToUpper() + @"%' ";
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
                        orderBySQL = "b.VSP_IME";
                        break;
                    case 2:
                        orderBySQL = "a.SpecializationName";
                        break;
                    default:
                        orderBySQL = "b.VSP_IME";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                    SELECT a.SpecializationID, a.SpecializationName, 
                                        a.MilitarySchoolSubjectID, b.VSP_IME as MilitarySchoolSubjectName,
                                        RANK() OVER (ORDER BY " + orderBySQL + @", a.SpecializationID) as RowNumber 
                                    FROM PMIS_APPL.Specializations a
                                    INNER JOIN VS_OWNER.KLV_VSP b ON a.MilitarySchoolSubjectID = b.VSPID
                                    WHERE a.SpecializationID NOT IN (SELECT a.SpecializationID
                                        FROM PMIS_APPL.Specializations a
                                        INNER JOIN PMIS_APPL.MilitarySchoolSpecializations b ON a.SpecializationID = b.SpecializationID
                                        WHERE b.MilitarySchoolID = :MilitarySchoolID AND b.Year = :Year)"
                                        + where + 
                                    @"ORDER BY " + orderBySQL + @", a.SpecializationID
                                ) tmp
                                " + pageWhere;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitarySchoolID", OracleType.Number).Value = filter.MilitarySchoolId;
                cmd.Parameters.Add("Year", OracleType.Number).Value = filter.Year;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Specialization specialization = new Specialization();
                    specialization.SpecializationId = DBCommon.GetInt(dr["SpecializationID"]);
                    specialization.SpecializationName = dr["SpecializationName"].ToString();
                    specialization.MilitarySchoolSubject = new MilitarySchoolSubject()
                    {
                        MilitarySchoolSubjectId = DBCommon.GetInt(dr["MilitarySchoolSubjectID"]),
                        MilitarySchoolSubjectName = dr["MilitarySchoolSubjectName"].ToString()
                    };

                    specializations.Add(specialization);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return specializations;
        }

        public static int GetAllUnusedSpecsByMilitarySchoolIDCount(SpecializationFilter filter, User currentUser)
        {
            int specializationsCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();

                string where = "";

                if (!String.IsNullOrEmpty(filter.SubjectName))
                {
                    where += " UPPER(a.MilitarySchoolSubjectID) LIKE '%" + filter.SubjectName.Replace("'", "''").ToUpper() + @"%' ";
                }

                if (!String.IsNullOrEmpty(filter.SpecializationName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(a.SpecializationName) LIKE '%" + filter.SpecializationName.Replace("'", "''").ToUpper() + @"%' ";
                }

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @"SELECT COUNT(*) as Cnt
                                FROM PMIS_APPL.Specializations a
                                INNER JOIN VS_OWNER.KLV_VSP b ON a.MilitarySchoolSubjectID = b.VSPID
                                "
                                + where +
                                @"
                                   AND a.SpecializationID NOT IN (SELECT a.SpecializationID
                                    FROM PMIS_APPL.Specializations a
                                    INNER JOIN PMIS_APPL.MilitarySchoolSpecializations b ON a.SpecializationID = b.SpecializationID
                                    WHERE b.MilitarySchoolID = :MilitarySchoolID AND b.Year = :Year)";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitarySchoolID", OracleType.Number).Value = filter.MilitarySchoolId;
                cmd.Parameters.Add("Year", OracleType.Number).Value = filter.Year;

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

        public static List<Specialization> GetAllSpecsByMilitarySchoolSubjectID(int militarySchoolId, int year, int subjectId, User currentUser)
        {
            List<Specialization> specializations = new List<Specialization>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();

                string SQL = @"SELECT a.SpecializationID, a.SpecializationName, 
                                  a.MilitarySchoolSubjectID, b.VSP_IME as MilitarySchoolSubjectName
                              FROM PMIS_APPL.Specializations a
                              INNER JOIN VS_OWNER.KLV_VSP b ON a.MilitarySchoolSubjectID = b.VSPID
                              WHERE a.SpecializationID IN (SELECT a.SpecializationID
                                  FROM PMIS_APPL.Specializations a
                                  INNER JOIN PMIS_APPL.MilitarySchoolSpecializations b ON a.SpecializationID = b.SpecializationID
                                  WHERE a.MilitarySchoolSubjectID = :MilitarySchoolSubjectID AND b.MilitarySchoolID = :MilitarySchoolID AND b.Year = :Year)";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitarySchoolID", OracleType.Number).Value = militarySchoolId;
                cmd.Parameters.Add("Year", OracleType.Number).Value = year;
                cmd.Parameters.Add("MilitarySchoolSubjectID", OracleType.Number).Value = subjectId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    Specialization specialization = new Specialization();
                    specialization.SpecializationId = DBCommon.GetInt(dr["SpecializationID"]);
                    specialization.SpecializationName = dr["SpecializationName"].ToString();
                    specialization.MilitarySchoolSubject = new MilitarySchoolSubject()
                    {
                        MilitarySchoolSubjectId = DBCommon.GetInt(dr["MilitarySchoolSubjectID"]),
                        MilitarySchoolSubjectName = dr["MilitarySchoolSubjectName"].ToString()
                    };

                    specializations.Add(specialization);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return specializations;
        }
    }
}
