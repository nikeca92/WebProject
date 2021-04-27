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
    public class ReportCadetBlock : BaseDbObject
    {
        private string cadetName = "";

        public string CadetName
        {
            get { return cadetName; }
            set { cadetName = value; }
        }

        private string identityNumber = "";

        public string IdentityNumber
        {
            get { return identityNumber; }
            set { identityNumber = value; }
        }

        private string militarySchoolName = "";

        public string MilitarySchoolName
        {
            get { return militarySchoolName; }
            set { militarySchoolName = value; }
        }

        private string militarySchoolYear = "";

        public string MilitarySchoolYear
        {
            get { return militarySchoolYear; }
            set { militarySchoolYear = value; }
        }


        private string subject = "";

        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        private string specialization = "";
        public string Specialization
        {
            get { return specialization; }
            set { specialization = value; }
        }

        private string status = "";

        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        public ReportCadetBlock(User user)
            : base(user)
        {
        }

    }

    public class ReportCadetBlockFilter
    {
        private int militarySchoolId;

        public int MilitarySchoolId
        {
            get { return militarySchoolId; }
            set { militarySchoolId = value; }
        }

        private int militarySchoolYear;

        public int MilitarySchoolYear
        {
            get { return militarySchoolYear; }
            set { militarySchoolYear = value; }
        }
        private int subjectId;

        public int SubjectId
        {
            get { return subjectId; }
            set { subjectId = value; }
        }


        private int specializationId;

        public int SpecializationId
        {
            get { return specializationId; }
            set { specializationId = value; }
        }
        private int statusId;

        public int StatusId
        {
            get { return statusId; }
            set { statusId = value; }
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

    public class ReportCadetUtil
    {
        //This method creates and returns a ApplicantDocumentStatus object. It extracts the data from a DataReader.
        public static ReportCadetBlock ExtractReportCadetBlockFromDataReader(OracleDataReader dr, User currentUser)
        {
            ReportCadetBlock reportCadetBlock = new ReportCadetBlock(currentUser);


            reportCadetBlock.CadetName = dr["CadetName"].ToString();
            reportCadetBlock.IdentityNumber = dr["IdentityNumber"].ToString();
            reportCadetBlock.MilitarySchoolName = dr["MilitarySchoolName"].ToString();
            reportCadetBlock.MilitarySchoolYear = dr["MilitarySchoolYear"].ToString();
            reportCadetBlock.Subject = dr["Subject"].ToString();
            reportCadetBlock.Specialization = dr["Specialization"].ToString();
            reportCadetBlock.Status = (dr["Status"].ToString() == "1" ? "Приет" : "Неприет");

            return reportCadetBlock;
        }

        public static List<MilitarySchoolYear> GetAllMilitaryYears(User currentUser)
        {
            List<MilitarySchoolYear> years = new List<MilitarySchoolYear>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @" SELECT a.Year
                                FROM PMIS_APPL.MilitarySchoolSpecializations a
                                UNION
                                SELECT to_number(to_char(sysdate, 'YYYY')) from dual";

                OracleCommand cmd = new OracleCommand(SQL, conn);

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

        public static List<MilitarySchoolSubject> GetAllMilitarySchoolSubject(ReportCadetBlockFilter filter, User currentUser)
        {
            MilitarySchoolSubject militarySchoolSubject;
            List<MilitarySchoolSubject> listMilitarySchool = new List<MilitarySchoolSubject>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                string where = "";
                OracleCommand cmd = new OracleCommand();

                if (filter.MilitarySchoolId > 0)
                {
                    where += " a.MilitarySchoolID=:MilitarySchoolID ";
                    cmd.Parameters.Add("MilitarySchoolID", OracleType.Number).Value = filter.MilitarySchoolId;
                }
                if (filter.MilitarySchoolYear > 0)
                {
                    where += (where == "" ? "" : " AND ") + "a.Year=:Year";
                    cmd.Parameters.Add("Year", OracleType.Number).Value = filter.MilitarySchoolYear;
                }

                //format where
                where = (where == "" ? "" : " WHERE ") + where;

                conn.Open();
                string SQL = @"  SELECT 
                                     c.vspid as SubjectId, c.vsp_ime as SubjectName                                      
                                     FROM PMIS_APPL.MilitarySchoolSpecializations a
                                     INNER JOIN PMIS_APPL.Specializations b ON a.SpecializationID = b.SpecializationID
                                     INNER JOIN VS_OWNER.KLV_VSP c ON b.MilitarySchoolSubjectID = c.VSPID "
                                      + where +
                                    @" GROUP BY c.vspid, c.vsp_ime ORDER BY  c.VSP_IME ";

                SQL = DBCommon.FixNewLines(SQL);

                cmd.CommandText = SQL;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    militarySchoolSubject = new MilitarySchoolSubject();
                    militarySchoolSubject.MilitarySchoolSubjectId = DBCommon.GetInt(dr["SubjectId"]);
                    militarySchoolSubject.MilitarySchoolSubjectName = dr["SubjectName"].ToString();

                    listMilitarySchool.Add(militarySchoolSubject);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listMilitarySchool;
        }

        public static List<Specialization> GetAllMilitarySchoolSpecializations(ReportCadetBlockFilter filter, User currentUser)
        {
            Specialization militarySchoolSpecialization;

            List<Specialization> listMilitarySchoolSpecialization = new List<Specialization>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                string where = "";
                OracleCommand cmd = new OracleCommand();

                if (filter.MilitarySchoolId > 0)
                {
                    where += " a.MilitarySchoolID=:MilitarySchoolID ";
                    cmd.Parameters.Add("MilitarySchoolID", OracleType.Number).Value = filter.MilitarySchoolId;
                }

                if (filter.MilitarySchoolYear > 0)
                {
                    where += (where == "" ? "" : " AND ") + "a.Year=:Year";
                    cmd.Parameters.Add("Year", OracleType.Number).Value = filter.MilitarySchoolYear;
                }

                if (filter.SubjectId > 0)
                {
                    where += (where == "" ? "" : " AND ") + @" b.specializationid IN( SELECT 
                                        b.SpecializationID                                     
                                     FROM PMIS_APPL.MilitarySchoolSpecializations a
                                     INNER JOIN PMIS_APPL.Specializations b ON a.SpecializationID = b.SpecializationID
                                     INNER JOIN VS_OWNER.KLV_VSP c ON b.MilitarySchoolSubjectID = c.VSPID  
                                     WHERE c.VSPID=:SubjectId )";
                    cmd.Parameters.Add("SubjectId", OracleType.Number).Value = filter.SubjectId;
                }

                //format where
                where = (where == "" ? "" : " WHERE ") + where;

                conn.Open();
                string SQL = @"  SELECT 
                                     b.SpecializationID as SpecializationID,  b.SpecializationName as SpecializationName                         
                                     FROM PMIS_APPL.MilitarySchoolSpecializations a
                                     LEFT OUTER JOIN PMIS_APPL.Specializations b ON a.SpecializationID = b.SpecializationID " +
                                       where +
                                     @" GROUP BY b.SpecializationID, b.SpecializationName ORDER BY b.SpecializationName ";


                SQL = DBCommon.FixNewLines(SQL);

                cmd.CommandText = SQL;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    militarySchoolSpecialization = new Specialization();

                    militarySchoolSpecialization.SpecializationId = DBCommon.GetInt(dr["SpecializationID"]);
                    militarySchoolSpecialization.SpecializationName = dr["SpecializationName"].ToString();

                    listMilitarySchoolSpecialization.Add(militarySchoolSpecialization);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listMilitarySchoolSpecialization;
        }

        public static List<ReportCadetBlock> GetAllReportCadetBlock(ReportCadetBlockFilter filter, int rowsPerPage, User currentUser)
        {
            ReportCadetBlock reportCadetBlock;
            List<ReportCadetBlock> listReportCadetBlock = new List<ReportCadetBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                OracleCommand cmd = new OracleCommand();

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("APPL_REPORTS_REPORT_CADET", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " k.CreatedBy = " + currentUser.UserId.ToString();
                }

                //add if need custom filter values 
                if (filter.MilitarySchoolId > 0)
                {
                    where += (where == "" ? "" : " AND ") +
                             " f.vvuid = :MilitarySchoolId ";
                    cmd.Parameters.Add("MilitarySchoolId", OracleType.Number).Value = filter.MilitarySchoolId;
                }

                if (filter.MilitarySchoolYear > 0)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.year = :MilitarySchoolYear ";
                    cmd.Parameters.Add("MilitarySchoolYear", OracleType.Number).Value = filter.MilitarySchoolYear;

                }

                if (filter.SubjectId > 0)
                {
                    where += (where == "" ? "" : " AND ") +
                             " c.vspid = :SubjectId ";
                    cmd.Parameters.Add("SubjectId", OracleType.Number).Value = filter.SubjectId;

                }

                if (filter.SpecializationId > 0)
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.SpecializationId = :SpecializationId ";
                    cmd.Parameters.Add("SpecializationId", OracleType.Number).Value = filter.SpecializationId;

                }

                // StatusId = -1 for all 
                // StatusId =1 for accepted 
                // StatusId =1 for UnAccepted 
                if (filter.StatusId >= 0)
                {
                    where += (where == "" ? "" : " AND ") +
                             " d.IsRanked =:StatusId";
                    cmd.Parameters.Add("StatusId", OracleType.Number).Value = filter.StatusId;

                }

                where = (where == "" ? "" : " WHERE ") + where;

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
                        orderBySQL = "m.IME " + orderByDir + ", m.FAM";
                        break;
                    case 2:
                        orderBySQL = "m.EGN";
                        break;
                    case 3:
                        orderBySQL = "f.vvu_ime";
                        break;
                    case 4:
                        orderBySQL = "a.year";
                        break;
                    case 5:
                        orderBySQL = "c.vsp_ime";
                        break;
                    case 6:
                        orderBySQL = "b.specializationname";
                        break;
                    case 7:
                        orderBySQL = "d.IsRanked";
                        break;
                    default:
                        orderBySQL = "m.IME " + orderByDir + ", m.FAM";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                          SELECT m.IME || ' ' || m.FAM as CadetName,                     
                                        m.EGN as IdentityNumber,                                     
                                        f.vvu_ime as MilitarySchoolName,                            
                                        a.year as MilitarySchoolYear,                             
                                        c.vsp_ime as Subject,
                                        c.VSPID as SubjectCode,           
                                        b.SpecializationName as Specialization,                 
                                        d.IsRanked as Status,
                                        RANK() OVER (ORDER BY " + orderBySQL + @", k.PersonID) as RowNumber
                                      FROM PMIS_APPL.MilitarySchoolSpecializations a
                                      INNER JOIN PMIS_APPL.Specializations b ON a.SpecializationID = b.SpecializationID
                                      INNER JOIN VS_OWNER.KLV_VSP c ON b.MilitarySchoolSubjectID = c.VSPID
                                      INNER JOIN PMIS_APPL.CadetSchoolSubjects d ON d.militschoolspecid = a.militschoolspecid 
                                      INNER JOIN PMIS_APPL.Cadets k on k.CadetId=d.CadetId
                                      INNER JOIN VS_OWNER.VS_LS m ON k.PersonID = m.PersonID
                                      INNER JOIN VS_OWNER.KLV_VVU f ON f.VVUID = a.MilitarySchoolID
                                          " + where + @"     
                                                                     
                                       ) tmp
                                       " + pageWhere;

                SQL = DBCommon.FixNewLines(SQL);

                cmd.CommandText = SQL;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    reportCadetBlock = ExtractReportCadetBlockFromDataReader(dr, currentUser);

                    listReportCadetBlock.Add(reportCadetBlock);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listReportCadetBlock;
        }

        public static int GetAllReportCadetCount(ReportCadetBlockFilter filter, User currentUser)
        {
            int cadetCount = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                OracleCommand cmd = new OracleCommand();

                //Restric the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("APPL_REPORTS_REPORT_CADET", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " k.CreatedBy = " + currentUser.UserId.ToString();
                }

                //add if need custom filter values 
                if (filter.MilitarySchoolId > 0)
                {
                    where += (where == "" ? "" : " AND ") +
                             " f.vvuid = :MilitarySchoolId ";
                    cmd.Parameters.Add("MilitarySchoolId", OracleType.Number).Value = filter.MilitarySchoolId;
                }

                if (filter.MilitarySchoolYear > 0)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.year = :MilitarySchoolYear ";
                    cmd.Parameters.Add("MilitarySchoolYear", OracleType.Number).Value = filter.MilitarySchoolYear;

                }

                if (filter.SubjectId > 0)
                {
                    where += (where == "" ? "" : " AND ") +
                             " c.vspid = :SubjectId ";
                    cmd.Parameters.Add("SubjectId", OracleType.Number).Value = filter.SubjectId;

                }

                if (filter.SpecializationId > 0)
                {
                    where += (where == "" ? "" : " AND ") +
                             " b.SpecializationId = :SpecializationId ";
                    cmd.Parameters.Add("SpecializationId", OracleType.Number).Value = filter.SpecializationId;

                }

                // StatusId = -1 for all 
                // StatusId =1 for accepted 
                // StatusId =1 for UnAccepted 
                if (filter.StatusId >= 0)
                {
                    where += (where == "" ? "" : " AND ") +
                             " d.IsRanked =:StatusId";
                    cmd.Parameters.Add("StatusId", OracleType.Number).Value = filter.StatusId;

                }

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @"SELECT COUNT(*) as Cnt
                                      FROM PMIS_APPL.MilitarySchoolSpecializations a
                                      INNER JOIN PMIS_APPL.Specializations b ON a.SpecializationID = b.SpecializationID
                                      INNER JOIN VS_OWNER.KLV_VSP c ON b.MilitarySchoolSubjectID = c.VSPID
                                      INNER JOIN PMIS_APPL.CadetSchoolSubjects d ON d.militschoolspecid = a.militschoolspecid 
                                      INNER JOIN PMIS_APPL.Cadets k on k.CadetId=d.CadetId
                                      INNER JOIN VS_OWNER.VS_LS m ON k.PersonID = m.PersonID
                                      INNER JOIN VS_OWNER.KLV_VVU f ON f.VVUID = a.MilitarySchoolID
                                    " + where + @"
                               ";

                SQL = DBCommon.FixNewLines(SQL);

                cmd.CommandText = SQL;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        cadetCount = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return cadetCount;
        }


    }
}
