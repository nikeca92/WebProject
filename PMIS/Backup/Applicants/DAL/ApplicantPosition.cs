using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using System.Linq;
using System.Web.UI.WebControls;

using PMIS.Common;

namespace PMIS.Applicants.Common
{
    //Represents single applicant position
    public class ApplicantPosition : BaseDbObject
    {
        private int applicantPositionId;
        private int applicantId;
        private int? seq;
        private int? vacancyAnnouncePositionId;
        private VacancyAnnouncePosition vacancyAnnouncePosition;        
        private int? submitDocsDepartmentId;
        private MilitaryDepartment submitDocsDepartment;        
        private int? applicantDocsStatusId;
        private ApplicantPositionDocumentStatus applicantDocsStatus;        
        private int? applicantStatusId;
        private ApplicantPositionStatus applicantStatus;
        private string combinedApplicantStatus;
        private string clInformationAccLevelBG;
        private decimal? rating;

        public int ApplicantPositionId
        {
            get { return applicantPositionId; }
            set { applicantPositionId = value; }
        }

        public int ApplicantId
        {
            get { return applicantId; }
            set { applicantId = value; }
        }

        public int? Seq
        {
            get { return seq; }
            set { seq = value; }
        }

        public int? VacancyAnnouncePositionId
        {
            get { return vacancyAnnouncePositionId; }
            set { vacancyAnnouncePositionId = value; }
        }

        public VacancyAnnouncePosition VacancyAnnouncePosition
        {
            get 
            {
                if (vacancyAnnouncePosition == null && vacancyAnnouncePositionId.HasValue)
                    vacancyAnnouncePosition = VacancyAnnouncePositionUtil.GetVacancyAnnouncePosition(vacancyAnnouncePositionId.Value, CurrentUser);
 
                return vacancyAnnouncePosition; 
            }
            set { vacancyAnnouncePosition = value; }
        }

        public int? SubmitDocsDepartmentId
        {
            get { return submitDocsDepartmentId; }
            set { submitDocsDepartmentId = value; }
        }

        public MilitaryDepartment SubmitDocsDepartment
        {
            get 
            {
                if (submitDocsDepartment == null && submitDocsDepartmentId.HasValue)
                    submitDocsDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment(submitDocsDepartmentId.Value, CurrentUser);

                return submitDocsDepartment; 
            }
            set { submitDocsDepartment = value; }
        }

        public int? ApplicantDocsStatusId
        {
            get { return applicantDocsStatusId; }
            set { applicantDocsStatusId = value; }
        }

        public ApplicantPositionDocumentStatus ApplicantDocsStatus
        {
            get 
            {
                if (applicantDocsStatus == null && applicantDocsStatusId.HasValue)
                    applicantDocsStatus = ApplicantPositionDocumentStatusUtil.GetApplicantPositionDocumentStatus(applicantDocsStatusId.Value, CurrentUser);

                return applicantDocsStatus; 
            }
            set { applicantDocsStatus = value; }
        }

        public int? ApplicantStatusId
        {
            get { return applicantStatusId; }
            set { applicantStatusId = value; }
        }

        public ApplicantPositionStatus ApplicantStatus
        {
            get 
            {
                if (applicantStatus == null && applicantStatusId.HasValue)
                    applicantStatus = ApplicantPositionStatusUtil.GetApplicantPositionStatus(applicantStatusId.Value, CurrentUser);

                return applicantStatus; 
            }
            set { applicantStatus = value; }
        }

        public string CombinedApplicantStatus
        {
            get
            {
                return combinedApplicantStatus;
            }

            set
            {
                combinedApplicantStatus = value;
            }
        }

        public string ClInformationAccLevelBG
        {
            get { return clInformationAccLevelBG; }
            set { clInformationAccLevelBG = value; }
        }

        public decimal? Rating
        {
            get { return rating; }
            set { rating = value; }
        }

        public ApplicantPosition(User user)
            : base(user)
        {

        }
    }

    public class AllowancePositionBlock
    {
        private int applicantPositionID;
        private int applicantID;        
        private string applicantName;        
        private string applicantIdentNumber;        
        private string militaryDepartmentName;        
        private decimal? rating;        
        private int statusID;
        private int personID;        
        private int militaryUnitID;
        private int responsibleMilitaryUnitID;
        private string combinedApplicantStatusKey;
        private string combinedApplicantStatusName;
        
        public int ApplicantPositionID
        {
            get { return applicantPositionID; }
            set { applicantPositionID = value; }
        }

        public int ApplicantID
        {
            get { return applicantID; }
            set { applicantID = value; }
        }
        
        public string ApplicantName
        {
            get { return applicantName; }
            set { applicantName = value; }
        }
        
        public string ApplicantIdentNumber
        {
            get { return applicantIdentNumber; }
            set { applicantIdentNumber = value; }
        }
        
        public string MilitaryDepartmentName
        {
            get { return militaryDepartmentName; }
            set { militaryDepartmentName = value; }
        }
        
        public decimal? Rating
        {
            get { return rating; }
            set { rating = value; }
        }
        
        public int StatusID
        {
            get { return statusID; }
            set { statusID = value; }
        }

        public int PersonID
        {
            get { return personID; }
            set { personID = value; }
        }

        public int MilitaryUnitID
        {
            get { return militaryUnitID; }
            set { militaryUnitID = value; }
        }

        public int ResponsibleMilitaryUnitID
        {
            get { return responsibleMilitaryUnitID; }
            set { responsibleMilitaryUnitID = value; }
        }

        public string CombinedApplicantStatusKey
        {
            get
            {
                return combinedApplicantStatusKey;
            }
            set
            {
                combinedApplicantStatusKey = value;
            }
        }

        public string CombinedApplicantStatusName
        {
            get
            {
                return combinedApplicantStatusName;
            }
            set
            {
                combinedApplicantStatusName = value;
            }
        }
    }

    public class RankPositionBlock
    {
        private int applicantPositionID;
        private int vacancyAnnouncePositionID;        
        private int applicantID;
        private string applicantName;
        private string applicantIdentNumber;      
        private string militaryDepartmentName; 
        private decimal? rating;
        private int statusID;
        private int personID;
        private int militaryUnitID;
        private int responsibleMilitaryUnitID;
        private List<ApplicantExamMarkBlock> marks;

        public int ApplicantPositionID
        {
            get { return applicantPositionID; }
            set { applicantPositionID = value; }
        }

        public int VacancyAnnouncePositionID
        {
            get { return vacancyAnnouncePositionID; }
            set { vacancyAnnouncePositionID = value; }
        }

        public int ApplicantID
        {
            get { return applicantID; }
            set { applicantID = value; }
        }

        public string ApplicantName
        {
            get { return applicantName; }
            set { applicantName = value; }
        }

        public string ApplicantIdentNumber
        {
            get { return applicantIdentNumber; }
            set { applicantIdentNumber = value; }
        }

        public string MilitaryDepartmentName
        {
            get { return militaryDepartmentName; }
            set { militaryDepartmentName = value; }
        }

        public decimal? Rating
        {
            get { return rating; }
            set { rating = value; }
        }

        public int StatusID
        {
            get { return statusID; }
            set { statusID = value; }
        }

        public int PersonID
        {
            get { return personID; }
            set { personID = value; }
        }

        public int MilitaryUnitID
        {
            get { return militaryUnitID; }
            set { militaryUnitID = value; }
        }

        public int ResponsibleMilitaryUnitID
        {
            get { return responsibleMilitaryUnitID; }
            set { responsibleMilitaryUnitID = value; }
        }

        public List<ApplicantExamMarkBlock> Marks
        {
            get { return marks; }
            set { marks = value; }
        }
    }

    public class NominationPositionBlock
    {
        private int applicantPositionID;
        private int vacancyAnnouncePositionID;
        private int applicantID;
        private string applicantName;
        private string applicantIdentNumber;
        private string clInformationAccLevelBG;
        private string militaryDepartmentName;
        private decimal? rating;
        private int statusID;
        private int personID;
        private int militaryUnitID;
        private int responsibleMilitaryUnitID;
        private List<ApplicantExamMarkBlock> marks;

        public int ApplicantPositionID
        {
            get { return applicantPositionID; }
            set { applicantPositionID = value; }
        }

        public int VacancyAnnouncePositionID
        {
            get { return vacancyAnnouncePositionID; }
            set { vacancyAnnouncePositionID = value; }
        }

        public int ApplicantID
        {
            get { return applicantID; }
            set { applicantID = value; }
        }

        public string ApplicantName
        {
            get { return applicantName; }
            set { applicantName = value; }
        }

        public string ApplicantIdentNumber
        {
            get { return applicantIdentNumber; }
            set { applicantIdentNumber = value; }
        }

        public string ClInformationAccLevelBG
        {
            get { return clInformationAccLevelBG; }
            set { clInformationAccLevelBG = value; }
        }

        public string MilitaryDepartmentName
        {
            get { return militaryDepartmentName; }
            set { militaryDepartmentName = value; }
        }

        public decimal? Rating
        {
            get { return rating; }
            set { rating = value; }
        }

        public int StatusID
        {
            get { return statusID; }
            set { statusID = value; }
        }

        public int PersonID
        {
            get { return personID; }
            set { personID = value; }
        }

        public int MilitaryUnitID
        {
            get { return militaryUnitID; }
            set { militaryUnitID = value; }
        }

        public int ResponsibleMilitaryUnitID
        {
            get { return responsibleMilitaryUnitID; }
            set { responsibleMilitaryUnitID = value; }
        }

        public List<ApplicantExamMarkBlock> Marks
        {
            get { return marks; }
            set { marks = value; }
        }
    }

    public static class ApplicantPositionUtil 
    {
        //This method fillings up an ApplicantPosition object. It extracts the data from a DataReader.
        private static ApplicantPosition ExtractApplicantPositionFromDataReader(OracleDataReader dr, User currentUser)
        {
            ApplicantPosition applicantPosition = new ApplicantPosition(currentUser);

            applicantPosition.ApplicantPositionId = DBCommon.GetInt(dr["ApplicantPositionID"]);
            applicantPosition.ApplicantId = DBCommon.GetInt(dr["ApplicantID"]);
            applicantPosition.Seq = (DBCommon.IsInt(dr["Seq"]) ? (int?)DBCommon.GetInt(dr["Seq"]) : null);
            applicantPosition.VacancyAnnouncePositionId = (DBCommon.IsInt(dr["VacancyAnnouncePositionID"]) ? (int?)DBCommon.GetInt(dr["VacancyAnnouncePositionID"]) : null);
            applicantPosition.SubmitDocsDepartmentId = (DBCommon.IsInt(dr["SubmitDocsDepartmentID"]) ? (int?)DBCommon.GetInt(dr["SubmitDocsDepartmentID"]) : null);
            applicantPosition.ApplicantDocsStatusId = (DBCommon.IsInt(dr["ApplicantDocumentsStatusID"]) ? (int?)DBCommon.GetInt(dr["ApplicantDocumentsStatusID"]) : null);
            applicantPosition.ApplicantStatusId = (DBCommon.IsInt(dr["ApplicantStatusID"]) ? (int?)DBCommon.GetInt(dr["ApplicantStatusID"]) : null);
            applicantPosition.ClInformationAccLevelBG = dr["ClInformationAccLevelBG"].ToString();
            applicantPosition.Rating = (DBCommon.IsDecimal(dr["Rating"]) ? (decimal?)DBCommon.GetDecimal(dr["Rating"]) : null);
            applicantPosition.CombinedApplicantStatus = dr["CombinedApplicantStatus"].ToString();

            return applicantPosition;
        }

        public static ApplicantPosition GetApplicantPosition(int applicantPositionId, User currentUser)
        {
            ApplicantPosition applicantPosition = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";                

                string SQL = @"SELECT a.ApplicantPositionID, a.ApplicantID, a.Seq, a.VacancyAnnouncePositionID, c.MilitaryDepartmentID as SubmitDocsDepartmentID, 
                                      a.ApplicantDocumentsStatusID, a.ApplicantStatusID, a.ClInformationAccLevelBG, a.Rating, 
                                      PMIS_APPL.APPL_Functions.GetCombinedApplicantStatus(a.ApplicantPositionID) as CombinedApplicantStatus
                                  FROM PMIS_APPL.ApplicantPositions a
                                  INNER JOIN PMIS_APPL.VacancyAnnouncePositions b ON a.VacancyAnnouncePositionID = b.VacancyAnnouncePositionID
                                  INNER JOIN PMIS_APPL.Applicants c ON a.ApplicantID = c.ApplicantID
                                  WHERE a.ApplicantPositionID = :ApplicantPositionID " + where;

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    SQL += @" AND (b.MilitaryUnitID IS NULL OR b.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @"))
                            ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ApplicantPositionID", OracleType.Number).Value = applicantPositionId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    applicantPosition = ExtractApplicantPositionFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return applicantPosition;
        }

        public static List<ApplicantPosition> GetAllApplicantPositionByPersonID(int personId, bool? onlyHistorical, User currentUser)
        {
            List<ApplicantPosition> applicantPositions = new List<ApplicantPosition>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                if (onlyHistorical.HasValue)
                {
                    if (onlyHistorical.Value)
                    {
                        where += " AND (a.ApplicantStatusID IN (SELECT StatusID FROM PMIS_APPL.ApplicantPositionStatus WHERE StatusKey IN ('DOCUMENTSREJECTED', 'NOTRATED', 'NOMINATED', 'CANCELED'))";
                        where += " OR f.vacannstatuskey ='FINISHED')";
                    }
                    else
                    {
                        where += " AND (a.ApplicantStatusID IS NULL OR a.ApplicantStatusID IN (SELECT StatusID FROM PMIS_APPL.ApplicantPositionStatus WHERE StatusKey IN ('DOCUMENTSAPPLIED', 'PARTICIPATIONALLOWED', 'RATED', 'APPOINTED', 'RESERVE')))";
                        where += " AND f.vacannstatuskey <>'FINISHED'";
                    }
                }

                string SQL = @"SELECT a.ApplicantPositionID,
                                      a.ApplicantID, 
                                      a.Seq, 
                                      a.VacancyAnnouncePositionID, 
                                      c.MilitaryDepartmentID as SubmitDocsDepartmentID, 
                                      a.ApplicantDocumentsStatusID, 
                                      a.ApplicantStatusID, 
                                      a.ClInformationAccLevelBG, 
                                      a.Rating,
                                      PMIS_APPL.APPL_Functions.GetCombinedApplicantStatus(a.ApplicantPositionID) as CombinedApplicantStatus
                               FROM PMIS_APPL.ApplicantPositions a
                               INNER JOIN PMIS_APPL.VacancyAnnouncePositions b ON a.VacancyAnnouncePositionID = b.VacancyAnnouncePositionID
                               INNER JOIN PMIS_APPL.Applicants c ON a.ApplicantID = c.ApplicantID
                               INNER JOIN PMIS_APPL.VacancyAnnounces d on d.VacancyAnnounceID = b.vacancyannounceid
                               INNER JOIN PMIS_APPL.VacancyAnnouncesStatuses f on f.vacancyannouncesstatusid = d.VacAnnStatusID
                               WHERE c.PersonID = :PersonID" + where;

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    SQL += @" AND (b.MilitaryUnitID IS NULL OR b.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @"))
                            ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Number).Value = personId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ApplicantPosition applicantPosition = ExtractApplicantPositionFromDataReader(dr, currentUser);
                    applicantPositions.Add(applicantPosition);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return applicantPositions;
        }

        public static List<AllowancePositionBlock> GetAllApplicantPositionForAllowance(int vacancyAnnounceId, int responsibleMilitaryUnitId, int militaryUnitId, int vacancyAnnouncePositionId, User currentUser)
        {
            List<AllowancePositionBlock> applicantPositions = new List<AllowancePositionBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                where += " AND a.ApplicantStatusID IS NOT NULL";                

                string SQL = @"SELECT a.ApplicantPositionID, d.MilitaryDepartmentName, a.ApplicantStatusID, a.Rating,
                                      e.IME as FirstName, e.FAM as LastName, e.EGN as IdentNumber, c.ApplicantID, c.PersonID, b.MilitaryUnitID,
                                      PMIS_APPL.APPL_Functions.GetCombinedApplicantStatusKey(a.ApplicantPositionID) as CombinedApplicantStatusKey,
                                      PMIS_APPL.APPL_Functions.GetCombinedApplicantStatus(a.ApplicantPositionID) as CombinedApplicantStatusName
                                  FROM PMIS_APPL.ApplicantPositions a
                                  INNER JOIN PMIS_APPL.VacancyAnnouncePositions b ON a.VacancyAnnouncePositionID = b.VacancyAnnouncePositionID
                                  INNER JOIN PMIS_APPL.Applicants c ON a.ApplicantID = c.ApplicantID
                                  INNER JOIN PMIS_ADM.MilitaryDepartments d ON c.MilitaryDepartmentID = d.MilitaryDepartmentID
                                  INNER JOIN VS_OWNER.VS_LS e ON c.PersonID = e.PersonID
                                  WHERE b.VacancyAnnounceID = :VacancyAnnounceID AND 
                                        b.ResponsibleMilitaryUnitID = :ResponsibleMilitaryUnitID AND 
                                        b.MilitaryUnitID = :MilitaryUnitID AND 
                                        b.VacancyAnnouncePositionID = :VacancyAnnouncePositionID" + where;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VacancyAnnounceID", OracleType.Number).Value = vacancyAnnounceId;
                cmd.Parameters.Add("ResponsibleMilitaryUnitID", OracleType.Number).Value = responsibleMilitaryUnitId;
                cmd.Parameters.Add("MilitaryUnitID", OracleType.Number).Value = militaryUnitId;
                cmd.Parameters.Add("VacancyAnnouncePositionID", OracleType.Number).Value = vacancyAnnouncePositionId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    AllowancePositionBlock block = new AllowancePositionBlock();

                    block.ApplicantPositionID = DBCommon.GetInt(dr["ApplicantPositionID"]);
                    block.ApplicantID = DBCommon.GetInt(dr["ApplicantID"]);
                    block.ApplicantName = dr["FirstName"].ToString() + " " + dr["LastName"].ToString();
                    block.ApplicantIdentNumber = dr["IdentNumber"].ToString();
                    block.MilitaryDepartmentName = dr["MilitaryDepartmentName"].ToString();
                    block.StatusID = DBCommon.GetInt(dr["ApplicantStatusID"]);
                    block.Rating = DBCommon.IsDecimal(dr["Rating"]) ? (decimal?)DBCommon.GetDecimal(dr["Rating"]) : null;
                    block.PersonID = DBCommon.GetInt(dr["PersonID"]);
                    block.MilitaryUnitID = DBCommon.GetInt(dr["MilitaryUnitID"]);
                    block.ResponsibleMilitaryUnitID = responsibleMilitaryUnitId;
                    block.CombinedApplicantStatusKey = dr["CombinedApplicantStatusKey"].ToString();
                    block.CombinedApplicantStatusName = dr["CombinedApplicantStatusName"].ToString();

                    applicantPositions.Add(block);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return applicantPositions;
        }

        public static List<RankPositionBlock> GetAllApplicantPositionForRanking(int vacancyAnnounceId, int responsibleMilitaryUnitId, int militaryUnitId, int vacancyAnnouncePositionId, User currentUser)
        {
            List<RankPositionBlock> applicantPositions = new List<RankPositionBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                where += " AND a.ApplicantStatusID IS NOT NULL AND a.ApplicantStatusID NOT IN (SELECT StatusID FROM PMIS_APPL.ApplicantPositionStatus WHERE StatusKey IN ('DOCUMENTSAPPLIED', 'DOCUMENTSREJECTED'))";

                where += " AND f.Status IN (SELECT ApplicantExamStatusID FROM PMIS_APPL.ApplicantExamStatuses WHERE ApplicantExamStatusKey = 'RATED')";

                string SQL = @"SELECT g.ApplicantExamMarkID,
                                      a.ApplicantPositionID,
                                      b.VacancyAnnouncePositionID,
                                      c.ApplicantID,
                                      d.MilitaryDepartmentName,
                                      e.IME as FirstName,
                                      e.FAM as LastName,
                                      e.EGN as IdentNumber,
                                      a.ApplicantStatusID, 
                                      a.Rating,
                                      c.PersonID, 
                                      b.MilitaryUnitID,
                                      h.VacancyAnnounceExamID,
                                      i.ExamName,
                                      g.Mark,
                                      g.Points
                                  FROM PMIS_APPL.ApplicantPositions a
                                  INNER JOIN PMIS_APPL.VacancyAnnouncePositions b ON a.VacancyAnnouncePositionID = b.VacancyAnnouncePositionID
                                  INNER JOIN PMIS_APPL.Applicants c ON a.ApplicantID = c.ApplicantID
                                  INNER JOIN PMIS_ADM.MilitaryDepartments d ON c.MilitaryDepartmentID = d.MilitaryDepartmentID
                                  INNER JOIN VS_OWNER.VS_LS e ON c.PersonID = e.PersonID
                                  INNER JOIN PMIS_APPL.ApplicantExamStatus f ON c.ApplicantID = f.ApplicantID AND b.ResponsibleMilitaryUnitID = f.ResponsibleMilitaryUnitID AND f.VacancyAnnounceID = b.VacancyAnnounceID
                                  INNER JOIN PMIS_APPL.ApplicantExamMarks g ON c.ApplicantID = g.ApplicantID AND b.ResponsibleMilitaryUnitID = g.ResponsibleMilitaryUnitID
                                  INNER JOIN PMIS_APPL.VacancyAnnounceExams h ON g.VacancyAnnounceExamID = h.VacancyAnnounceExamID AND h.VacancyAnnounceID = b.VacancyAnnounceID
                                  INNER JOIN PMIS_APPL.Exams i ON h.ApplicantExamID = i.ExamID
                                  WHERE b.VacancyAnnounceID = :VacancyAnnounceID AND 
                                        b.ResponsibleMilitaryUnitID = :ResponsibleMilitaryUnitID AND 
                                        b.MilitaryUnitID = :MilitaryUnitID AND 
                                        b.VacancyAnnouncePositionID = :VacancyAnnouncePositionID" + where + @"
                                  GROUP BY g.ApplicantExamMarkID,
                                           a.ApplicantPositionID,
                                           b.VacancyAnnouncePositionID,
                                           c.ApplicantID,
                                           d.MilitaryDepartmentName,
                                           e.IME,
                                           e.FAM,
                                           e.EGN,
                                           a.ApplicantStatusID, 
                                           a.Rating,
                                           c.PersonID, 
                                           b.MilitaryUnitID,
                                           h.VacancyAnnounceExamID,
                                           i.ExamName,
                                           g.Mark,
                                           g.Points
                                  ORDER BY a.ApplicantPositionID, h.VacancyAnnounceExamID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VacancyAnnounceID", OracleType.Number).Value = vacancyAnnounceId;
                cmd.Parameters.Add("ResponsibleMilitaryUnitID", OracleType.Number).Value = responsibleMilitaryUnitId;
                cmd.Parameters.Add("MilitaryUnitID", OracleType.Number).Value = militaryUnitId;
                cmd.Parameters.Add("VacancyAnnouncePositionID", OracleType.VarChar).Value = vacancyAnnouncePositionId;

                OracleDataReader dr = cmd.ExecuteReader();

                RankPositionBlock lastBlock = null;

                while (dr.Read())
                {
                    if (lastBlock != null && lastBlock.ApplicantPositionID != DBCommon.GetInt(dr["ApplicantPositionID"]))
                    {
                        applicantPositions.Add(lastBlock);
                        lastBlock = null;
                    }

                    if (lastBlock == null)
                    {
                        lastBlock = new RankPositionBlock();
                        lastBlock.ApplicantPositionID = DBCommon.GetInt(dr["ApplicantPositionID"]);
                        lastBlock.VacancyAnnouncePositionID = DBCommon.GetInt(dr["VacancyAnnouncePositionID"]);
                        lastBlock.ApplicantID = DBCommon.GetInt(dr["ApplicantID"]);
                        lastBlock.ApplicantName = dr["FirstName"].ToString() + " " + dr["LastName"].ToString();
                        lastBlock.ApplicantIdentNumber = dr["IdentNumber"].ToString();
                        lastBlock.MilitaryDepartmentName = dr["MilitaryDepartmentName"].ToString();
                        lastBlock.StatusID = DBCommon.GetInt(dr["ApplicantStatusID"]);
                        lastBlock.Rating = DBCommon.IsDecimal(dr["Rating"]) ? (decimal?)DBCommon.GetDecimal(dr["Rating"]) : null;
                        lastBlock.PersonID = DBCommon.GetInt(dr["PersonID"]);
                        lastBlock.MilitaryUnitID = DBCommon.GetInt(dr["MilitaryUnitID"]);
                        lastBlock.ResponsibleMilitaryUnitID = responsibleMilitaryUnitId;
                        lastBlock.Marks = new List<ApplicantExamMarkBlock>();
                    }

                    ApplicantExamMarkBlock mark = new ApplicantExamMarkBlock();
                    mark.ApplicantExamMarkId = DBCommon.GetInt(dr["ApplicantExamMarkID"]) != -1 ? (int?)DBCommon.GetInt(dr["ApplicantExamMarkID"]) : null;
                    mark.VacancyAnnounceExamId = DBCommon.GetInt(dr["VacancyAnnounceExamID"]);
                    mark.ExamName = dr["ExamName"].ToString();
                    mark.Mark = DBCommon.IsInt(dr["Mark"]) ? (int?)DBCommon.GetInt(dr["Mark"]) : null;
                    mark.Points = DBCommon.IsInt(dr["Points"]) ? (int?)DBCommon.GetInt(dr["Points"]) : null;
                    lastBlock.Marks.Add(mark);
                }

                if (lastBlock != null)
                    applicantPositions.Add(lastBlock);

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return applicantPositions;
        }

        public static List<NominationPositionBlock> GetAllApplicantPositionForNominating(int vacancyAnnounceId, int responsibleMilitaryUnitId, int militaryUnitId, int vacancyAnnouncePositionId, User currentUser)
        {
            List<NominationPositionBlock> applicantPositions = new List<NominationPositionBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                where += " AND a.ApplicantStatusID IS NOT NULL AND a.ApplicantStatusID IN (SELECT StatusID FROM PMIS_APPL.ApplicantPositionStatus WHERE StatusKey IN ('APPOINTED', 'RESERVE', 'NOMINATED', 'CANCELED'))";

                where += " AND f.Status IN (SELECT ApplicantExamStatusID FROM PMIS_APPL.ApplicantExamStatuses WHERE ApplicantExamStatusKey = 'RATED')";

                string SQL = @"SELECT g.ApplicantExamMarkID,
                                      a.ApplicantPositionID,
                                      b.VacancyAnnouncePositionID,
                                      c.ApplicantID,
                                      d.MilitaryDepartmentName,
                                      e.IME as FirstName,
                                      e.FAM as LastName,
                                      e.EGN as IdentNumber,
                                      a.ClInformationAccLevelBG,
                                      a.ApplicantStatusID, 
                                      a.Rating,
                                      c.PersonID, 
                                      b.MilitaryUnitID,
                                      h.VacancyAnnounceExamID,
                                      i.ExamName,
                                      g.Mark,
                                      g.Points
                                  FROM PMIS_APPL.ApplicantPositions a
                                  INNER JOIN PMIS_APPL.VacancyAnnouncePositions b ON a.VacancyAnnouncePositionID = b.VacancyAnnouncePositionID
                                  INNER JOIN PMIS_APPL.Applicants c ON a.ApplicantID = c.ApplicantID
                                  INNER JOIN PMIS_ADM.MilitaryDepartments d ON c.MilitaryDepartmentID = d.MilitaryDepartmentID
                                  INNER JOIN VS_OWNER.VS_LS e ON c.PersonID = e.PersonID
                                  INNER JOIN PMIS_APPL.ApplicantExamStatus f ON c.ApplicantID = f.ApplicantID AND b.ResponsibleMilitaryUnitID = f.ResponsibleMilitaryUnitID AND f.VacancyAnnounceID = b.VacancyAnnounceID
                                  INNER JOIN PMIS_APPL.ApplicantExamMarks g ON c.ApplicantID = g.ApplicantID AND b.ResponsibleMilitaryUnitID = g.ResponsibleMilitaryUnitID
                                  INNER JOIN PMIS_APPL.VacancyAnnounceExams h ON g.VacancyAnnounceExamID = h.VacancyAnnounceExamID AND h.VacancyAnnounceID = b.VacancyAnnounceID
                                  INNER JOIN PMIS_APPL.Exams i ON h.ApplicantExamID = i.ExamID
                                  WHERE b.VacancyAnnounceID = :VacancyAnnounceID AND 
                                        b.ResponsibleMilitaryUnitID = :ResponsibleMilitaryUnitID AND 
                                        b.MilitaryUnitID = :MilitaryUnitID AND 
                                        b.VacancyAnnouncePositionID = :VacancyAnnouncePositionID" + where + @"
                                  GROUP BY g.ApplicantExamMarkID,
                                           a.ApplicantPositionID,
                                           b.VacancyAnnouncePositionID,
                                           c.ApplicantID,
                                           d.MilitaryDepartmentName,
                                           e.IME,
                                           e.FAM,
                                           e.EGN,
                                           a.ClInformationAccLevelBG,
                                           a.ApplicantStatusID, 
                                           a.Rating,
                                           c.PersonID, 
                                           b.MilitaryUnitID,
                                           h.VacancyAnnounceExamID,
                                           i.ExamName,
                                           g.Mark,
                                           g.Points
                                  ORDER BY a.ApplicantPositionID, h.VacancyAnnounceExamID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VacancyAnnounceID", OracleType.Number).Value = vacancyAnnounceId;
                cmd.Parameters.Add("ResponsibleMilitaryUnitID", OracleType.Number).Value = responsibleMilitaryUnitId;
                cmd.Parameters.Add("MilitaryUnitID", OracleType.Number).Value = militaryUnitId;
                cmd.Parameters.Add("VacancyAnnouncePositionID", OracleType.VarChar).Value = vacancyAnnouncePositionId;

                OracleDataReader dr = cmd.ExecuteReader();

                NominationPositionBlock lastBlock = null;

                while (dr.Read())
                {
                    if (lastBlock != null && lastBlock.ApplicantPositionID != DBCommon.GetInt(dr["ApplicantPositionID"]))
                    {
                        applicantPositions.Add(lastBlock);
                        lastBlock = null;
                    }

                    if (lastBlock == null)
                    {
                        lastBlock = new NominationPositionBlock();
                        lastBlock.ApplicantPositionID = DBCommon.GetInt(dr["ApplicantPositionID"]);
                        lastBlock.VacancyAnnouncePositionID = DBCommon.GetInt(dr["VacancyAnnouncePositionID"]);
                        lastBlock.ApplicantID = DBCommon.GetInt(dr["ApplicantID"]);
                        lastBlock.ApplicantName = dr["FirstName"].ToString() + " " + dr["LastName"].ToString();
                        lastBlock.ApplicantIdentNumber = dr["IdentNumber"].ToString();
                        lastBlock.ClInformationAccLevelBG = dr["ClInformationAccLevelBG"].ToString();
                        lastBlock.MilitaryDepartmentName = dr["MilitaryDepartmentName"].ToString();
                        lastBlock.StatusID = DBCommon.GetInt(dr["ApplicantStatusID"]);
                        lastBlock.Rating = DBCommon.IsDecimal(dr["Rating"]) ? (decimal?)DBCommon.GetDecimal(dr["Rating"]) : null;
                        lastBlock.PersonID = DBCommon.GetInt(dr["PersonID"]);
                        lastBlock.MilitaryUnitID = DBCommon.GetInt(dr["MilitaryUnitID"]);
                        lastBlock.ResponsibleMilitaryUnitID = responsibleMilitaryUnitId;
                        lastBlock.Marks = new List<ApplicantExamMarkBlock>();
                    }

                    ApplicantExamMarkBlock mark = new ApplicantExamMarkBlock();
                    mark.ApplicantExamMarkId = DBCommon.GetInt(dr["ApplicantExamMarkID"]) != -1 ? (int?)DBCommon.GetInt(dr["ApplicantExamMarkID"]) : null;
                    mark.VacancyAnnounceExamId = DBCommon.GetInt(dr["VacancyAnnounceExamID"]);
                    mark.ExamName = dr["ExamName"].ToString();
                    mark.Mark = DBCommon.IsInt(dr["Mark"]) ? (int?)DBCommon.GetInt(dr["Mark"]) : null;
                    mark.Points = DBCommon.IsInt(dr["Points"]) ? (int?)DBCommon.GetInt(dr["Points"]) : null;
                    lastBlock.Marks.Add(mark);
                }

                if (lastBlock != null)
                    applicantPositions.Add(lastBlock);

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return applicantPositions;
        }

        public static bool SaveAllowancePositionBlocks(List<AllowancePositionBlock> oldPositions, List<AllowancePositionBlock> newPositions, string orderNumDate, string militaryUnitName, string positionCode, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            var positions = (from n in newPositions
                             join o in oldPositions on n.ApplicantPositionID equals o.ApplicantPositionID
                             select new { n.ApplicantPositionID, o.ApplicantID, o.ApplicantName, o.ApplicantIdentNumber, o.MilitaryDepartmentName, o.PersonID, o.ResponsibleMilitaryUnitID, newRating = n.Rating, newStatusID = n.StatusID, oldRating = o.Rating, oldStatusID = o.StatusID });

            List<ApplicantPositionStatus> statuses = ApplicantPositionStatusUtil.GetAllApplicantPositionStatus(currentUser);

            try
            {
                SQL = @"BEGIN
                        
                       ";

                foreach (var block in positions)
                {
                    Person person = new Person(currentUser);
                    person.PersonId = block.PersonID;

                    MilitaryUnit responsibleMilitaryUnit = new MilitaryUnit(currentUser);
                    responsibleMilitaryUnit.MilitaryUnitId = block.ResponsibleMilitaryUnitID;

                    string logDescription = "";
                    logDescription += "Кандидат: " + block.ApplicantName;
                    logDescription += "<br />ЕГН: " + block.ApplicantIdentNumber;
                    logDescription += "<br />Военно окръжие: " + block.MilitaryDepartmentName;
                    logDescription += "<br />Заповед №: " + orderNumDate;
                    logDescription += "<br />" + CommonFunctions.GetLabelText("MilitaryUnit") + " отговорна за конкурса: " + militaryUnitName;
                    logDescription += "<br />Код на длъжността: " + positionCode;

                    ChangeEvent changeEvent = new ChangeEvent("APPL_Applicants_Allowance", logDescription, responsibleMilitaryUnit, person, currentUser);

                    if (block.oldStatusID != block.newStatusID)
                        changeEvent.AddDetail(new ChangeEventDetail("APPL_ApplicantsPosition_Status", (from s in statuses where s.StatusId == block.oldStatusID select s.StatusName).First(), (from s in statuses where s.StatusId == block.newStatusID select s.StatusName).First(), currentUser));

                    if (block.oldRating != block.newRating)
                        changeEvent.AddDetail(new ChangeEventDetail("APPL_ApplicantsPosition_Rating", block.oldRating.HasValue ? block.oldRating.Value.ToString() : "", block.newRating.HasValue ? block.newRating.Value.ToString() : "", currentUser));

                    SQL += @" UPDATE PMIS_APPL.ApplicantPositions SET
                                Rating = " + (block.newRating.HasValue ? block.newRating.Value.ToString() : "NULL") + @",
                                ApplicantStatusID = " + block.newStatusID.ToString() + @"
                              WHERE ApplicantPositionID = " + block.ApplicantPositionID + @" ;

                            ";

                    changeEntry.AddEvent(changeEvent);
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.ExecuteNonQuery();


                result = true;
            }
            finally
            {
                conn.Close();
            }

            if (result)
                ApplicantUtil.SetApplicantsModified((from p in oldPositions select p.ApplicantID).ToList(), currentUser);


            return result;
        }

        public static bool SaveRankPositionBlocks(List<RankPositionBlock> oldPositions, List<RankPositionBlock> newPositions, string orderNumDate, string militaryUnitName, string positionCode, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            var positions = (from n in newPositions
                             join o in oldPositions on n.ApplicantPositionID equals o.ApplicantPositionID
                             select new { n.ApplicantPositionID, o.ApplicantID, o.ApplicantName, o.ApplicantIdentNumber, o.MilitaryDepartmentName, o.PersonID, o.ResponsibleMilitaryUnitID, newStatusID = n.StatusID, oldStatusID = o.StatusID });

            List<ApplicantPositionStatus> statuses = ApplicantPositionStatusUtil.GetAllApplicantPositionStatus(currentUser);
            List<ApplicantExamStatus> examStatuses = ApplicantExamStatusUtil.GetAllApplicantExamStatuses(currentUser);
            ApplicantExamStatus ratedStatus = (from s in examStatuses where s.StatusKey == "RATED" select s).FirstOrDefault();
            ApplicantPositionStatus allowedStatus = (from s in statuses where s.StatusKey == "PARTICIPATIONALLOWED" select s).FirstOrDefault();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                foreach (var block in positions)
                {
                    Person person = new Person(currentUser);
                    person.PersonId = block.PersonID;

                    MilitaryUnit responsibleMilitaryUnit = new MilitaryUnit(currentUser);
                    responsibleMilitaryUnit.MilitaryUnitId = block.ResponsibleMilitaryUnitID;

                    string logDescription = "";
                    logDescription += "Кандидат: " + block.ApplicantName;
                    logDescription += "<br />ЕГН: " + block.ApplicantIdentNumber;
                    logDescription += "<br />Военно окръжие: " + block.MilitaryDepartmentName;
                    logDescription += "<br />Заповед №: " + orderNumDate;
                    logDescription += "<br />" + CommonFunctions.GetLabelText("MilitaryUnit") + " отговорна за конкурса: " + militaryUnitName;
                    logDescription += "<br />Код на длъжността: " + positionCode;

                    ChangeEvent changeEvent = new ChangeEvent("APPL_Applicants_Ranking", logDescription, responsibleMilitaryUnit, person, currentUser);

                    if (block.oldStatusID != block.newStatusID)
                    {
                        string oldStatusName = (from s in statuses where s.StatusId == block.oldStatusID select s.StatusName).First();
                        string newStatusName = (from s in statuses where s.StatusId == block.newStatusID select s.StatusName).First();

                        if (block.oldStatusID == allowedStatus.StatusId)
                            oldStatusName = ratedStatus.StatusName;

                        if (block.newStatusID == allowedStatus.StatusId)
                            newStatusName = ratedStatus.StatusName;

                        changeEvent.AddDetail(new ChangeEventDetail("APPL_ApplicantsPosition_Status", oldStatusName, newStatusName, currentUser));
                    }

                    SQL += @" UPDATE PMIS_APPL.ApplicantPositions SET                                
                                ApplicantStatusID = " + block.newStatusID.ToString() + @"
                              WHERE ApplicantPositionID = " + block.ApplicantPositionID + @" ;

                            ";

                    changeEntry.AddEvent(changeEvent);
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.ExecuteNonQuery();


                result = true;
            }
            finally
            {
                conn.Close();
            }

            if (result)
                ApplicantUtil.SetApplicantsModified((from p in oldPositions select p.ApplicantID).ToList(), currentUser);


            return result;
        }

        public static bool SaveNominationPositionBlocks(List<NominationPositionBlock> oldPositions, List<NominationPositionBlock> newPositions, string orderNumDate, string militaryUnitName, string positionCode, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            var positions = (from n in newPositions
                             join o in oldPositions on n.ApplicantPositionID equals o.ApplicantPositionID
                             select new { n.ApplicantPositionID, o.ApplicantID, o.ApplicantName, o.ApplicantIdentNumber, o.MilitaryDepartmentName, o.PersonID, o.ResponsibleMilitaryUnitID, newClInformationAccLevelBG = n.ClInformationAccLevelBG, oldClInformationAccLevelBG = o.ClInformationAccLevelBG, newStatusID = n.StatusID, oldStatusID = o.StatusID });

            List<ClInformation> clInfoList = ClInformationUtil.GetAllClInformationBG(currentUser);

            List<ApplicantPositionStatus> statuses = ApplicantPositionStatusUtil.GetAllApplicantPositionStatus(currentUser);

            try
            {
                SQL = @"BEGIN
                        
                       ";

                foreach (var block in positions)
                {
                    Person person = new Person(currentUser);
                    person.PersonId = block.PersonID;

                    MilitaryUnit responsibleMilitaryUnit = new MilitaryUnit(currentUser);
                    responsibleMilitaryUnit.MilitaryUnitId = block.ResponsibleMilitaryUnitID;

                    string logDescription = "";
                    logDescription += "Кандидат: " + block.ApplicantName;
                    logDescription += "<br />ЕГН: " + block.ApplicantIdentNumber;
                    logDescription += "<br />Военно окръжие: " + block.MilitaryDepartmentName;
                    logDescription += "<br />Заповед №: " + orderNumDate;
                    logDescription += "<br />" + CommonFunctions.GetLabelText("MilitaryUnit") + " отговорна за конкурса: " + militaryUnitName;
                    logDescription += "<br />Код на длъжността: " + positionCode;

                    ChangeEvent changeEvent = new ChangeEvent("APPL_Applicants_Nominating", logDescription, responsibleMilitaryUnit, person, currentUser);

                    if (block.oldClInformationAccLevelBG != block.newClInformationAccLevelBG)
                        changeEvent.AddDetail(new ChangeEventDetail("APPL_Applicants_ClInformationAccLevelBG", (from s in clInfoList where s.ClInfoKey == block.oldClInformationAccLevelBG select s.ClInfoName).FirstOrDefault(), (from s in clInfoList where s.ClInfoKey == block.newClInformationAccLevelBG select s.ClInfoName).FirstOrDefault(), currentUser));

                    if (block.oldStatusID != block.newStatusID)
                        changeEvent.AddDetail(new ChangeEventDetail("APPL_ApplicantsPosition_Status", (from s in statuses where s.StatusId == block.oldStatusID select s.StatusName).First(), (from s in statuses where s.StatusId == block.newStatusID select s.StatusName).First(), currentUser));

                    SQL += @" UPDATE PMIS_APPL.ApplicantPositions SET
                                ClInformationAccLevelBG = '" + block.newClInformationAccLevelBG + @"',
                                ApplicantStatusID = " + block.newStatusID.ToString() + @"
                              WHERE ApplicantPositionID = " + block.ApplicantPositionID + @" ;

                            ";

                    changeEntry.AddEvent(changeEvent);
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.ExecuteNonQuery();


                result = true;
            }
            finally
            {
                conn.Close();
            }

            if (result)
                ApplicantUtil.SetApplicantsModified((from p in oldPositions select p.ApplicantID).ToList(), currentUser);


            return result;
        }

        public static bool SaveApplicantPosition(int applicantId, ApplicantPosition applicantPosition, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            Applicant applicant = ApplicantUtil.GetApplicant(applicantId, currentUser);
            VacancyAnnounce vacancyAnnounce = VacancyAnnounceUtil.GetVacancyAnnounce(applicantPosition.VacancyAnnouncePosition.VacancyAnnounceID, currentUser);

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                string logDescription = "";
                logDescription += "Кандидат: " + applicant.Person.FullName;
                logDescription += "<br />ЕГН: " + applicant.Person.IdentNumber;
                logDescription += "<br />Военно окръжие: " + applicant.MilitaryDepartment.MilitaryDepartmentName;
                logDescription += "<br />Заповед №: " + vacancyAnnounce.OrderNum + " / Дата:" + CommonFunctions.FormatDate(vacancyAnnounce.OrderDate);
                logDescription += "<br />" + CommonFunctions.GetLabelText("MilitaryUnit") + ": " + applicantPosition.VacancyAnnouncePosition.MilitaryUnit.DisplayTextForSelection;
                logDescription += "<br />Код на длъжността: " + applicantPosition.VacancyAnnouncePosition.PositionCode;

                if (applicantPosition.ApplicantPositionId == 0)
                {
                    SQL += @"INSERT INTO PMIS_APPL.ApplicantPositions 
                                            (ApplicantID, 
                                             Seq, 
                                             VacancyAnnouncePositionID,                                             
                                             ApplicantDocumentsStatusID, 
                                             ApplicantStatusID, 
                                             ClInformationAccLevelBG,
                                             Rating)
                            VALUES          (:ApplicantID, 
                                             :Seq, 
                                             :VacancyAnnouncePositionID,                                            
                                             :ApplicantDocumentsStatusID, 
                                             :ApplicantStatusID, 
                                             :ClInformationAccLevelBG,
                                             :Rating);

                            SELECT PMIS_APPL.ApplicantPositions_ID_SEQ.currval INTO :ApplicantPositionID FROM dual;

                            ";

                    changeEvent = new ChangeEvent("APPL_Applicants_AddPosition", logDescription, applicantPosition.VacancyAnnouncePosition.MilitaryUnit, applicant.Person, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("APPL_ApplicantsPosition_DocStatus", "", applicantPosition.ApplicantDocsStatus.StatusName, currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_APPL.ApplicantPositions SET
                               ApplicantID = :ApplicantID,
                               Seq = :Seq,
                               VacancyAnnouncePositionID = :VacancyAnnouncePositionID,                               
                               ApplicantDocumentsStatusID = :ApplicantDocumentsStatusID,
                               ApplicantStatusID = :ApplicantStatusID,
                               ClInformationAccLevelBG = :ClInformationAccLevelBG,
                               Rating = :Rating
                            WHERE ApplicantPositionID = :ApplicantPositionID ;                            

                            ";

                    changeEvent = new ChangeEvent("APPL_Applicants_EditPosition", logDescription, applicantPosition.VacancyAnnouncePosition.MilitaryUnit, applicant.Person, currentUser);

                    ApplicantPosition oldApplicantPosition = ApplicantPositionUtil.GetApplicantPosition(applicantPosition.ApplicantPositionId, currentUser);

                    if (oldApplicantPosition.ApplicantDocsStatus.StatusName != applicantPosition.ApplicantDocsStatus.StatusName)
                        changeEvent.AddDetail(new ChangeEventDetail("APPL_ApplicantsPosition_DocStatus", oldApplicantPosition.ApplicantDocsStatus.StatusName, applicantPosition.ApplicantDocsStatus.StatusName, currentUser));                    
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramApplicantPositionID = new OracleParameter();
                paramApplicantPositionID.ParameterName = "ApplicantPositionID";
                paramApplicantPositionID.OracleType = OracleType.Number;

                if (applicantPosition.ApplicantPositionId != 0)
                {
                    paramApplicantPositionID.Direction = ParameterDirection.Input;
                    paramApplicantPositionID.Value = applicantPosition.ApplicantPositionId;
                }
                else
                {
                    paramApplicantPositionID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramApplicantPositionID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "ApplicantID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = applicantId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Seq";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (applicantPosition.Seq.HasValue)
                    param.Value = applicantPosition.Seq.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "VacancyAnnouncePositionID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (applicantPosition.VacancyAnnouncePositionId.HasValue)
                    param.Value = applicantPosition.VacancyAnnouncePositionId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);               

                param = new OracleParameter();
                param.ParameterName = "ApplicantDocumentsStatusID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (applicantPosition.ApplicantDocsStatusId.HasValue)
                    param.Value = applicantPosition.ApplicantDocsStatusId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ApplicantStatusID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (applicantPosition.ApplicantStatusId.HasValue)
                    param.Value = applicantPosition.ApplicantStatusId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ClInformationAccLevelBG";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(applicantPosition.ClInformationAccLevelBG))
                    param.Value = applicantPosition.ClInformationAccLevelBG;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Rating";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (applicantPosition.Rating.HasValue)
                    param.Value = applicantPosition.Rating.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);              

                cmd.ExecuteNonQuery();

                if (applicantPosition.ApplicantPositionId == 0)
                {
                    applicantPosition.ApplicantPositionId = DBCommon.GetInt(paramApplicantPositionID.Value);
                }

                result = true;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                if (changeEvent.ChangeEventDetails.Count > 0)
                {
                    changeEntry.AddEvent(changeEvent);
                    ApplicantUtil.SetApplicantModified(applicantId, currentUser);
                }
            }

            return result;
        }

        public static bool DeleteApplicantPosition(int applicantId, int applicantPositionId, User currentUser, Change changeEntry)
        {
            bool result = false;

            Applicant applicant = ApplicantUtil.GetApplicant(applicantId, currentUser);
            ApplicantPosition oldApplicantPosition = ApplicantPositionUtil.GetApplicantPosition(applicantPositionId, currentUser);
            VacancyAnnounce vacancyAnnounce = VacancyAnnounceUtil.GetVacancyAnnounce(oldApplicantPosition.VacancyAnnouncePosition.VacancyAnnounceID, currentUser);

            string logDescription = "";
            logDescription += "Кандидат: " + applicant.Person.FullName;
            logDescription += "<br />ЕГН: " + applicant.Person.IdentNumber;
            logDescription += "<br />Военно окръжие: " + applicant.MilitaryDepartment.MilitaryDepartmentName;
            logDescription += "<br />Заповед №: " + vacancyAnnounce.OrderNum + " / Дата:" + CommonFunctions.FormatDate(vacancyAnnounce.OrderDate);
            logDescription += "<br />" + CommonFunctions.GetLabelText("MilitaryUnit") + ": " + oldApplicantPosition.VacancyAnnouncePosition.MilitaryUnit.DisplayTextForSelection;
            logDescription += "<br />Код на длъжността: " + oldApplicantPosition.VacancyAnnouncePosition.PositionCode;

            ChangeEvent changeEvent = new ChangeEvent("APPL_Applicants_DeletePosition", logDescription, oldApplicantPosition.VacancyAnnouncePosition.MilitaryUnit, applicant.Person, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("APPL_ApplicantsPosition_DocStatus", oldApplicantPosition.ApplicantDocsStatus.StatusName, "", currentUser));            

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = "DELETE FROM PMIS_APPL.ApplicantPositions WHERE ApplicantPositionID = :ApplicantPositionID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ApplicantPositionID", OracleType.Number).Value = applicantPositionId;

                result = cmd.ExecuteNonQuery() == 1;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                changeEntry.AddEvent(changeEvent);
                ApplicantUtil.SetApplicantModified(applicantId, currentUser);
            }

            return result;
        }

        //Get a count of Investigation Protocols for current declaration
        public static int CountAllApplicantPositionForVacancyAnnounce(int vacancyAnnounceId, User currentUser)
        {
            int count = 0;
            //Create connection object
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            //Create command object
            OracleCommand cmd = new OracleCommand();
            //Create paramaeter object
            OracleParameter param = new OracleParameter();

            conn.Open();

            try
            {
                string SQL = @"SELECT count(*)
                               FROM PMIS_APPL.ApplicantPositions a
                               INNER JOIN PMIS_APPL.VacancyAnnouncePositions b ON a.VacancyAnnouncePositionID = b.VacancyAnnouncePositionID
                               INNER JOIN PMIS_APPL.Applicants c ON a.ApplicantID = c.ApplicantID 
                               WHERE b.VacancyAnnounceId=:VacancyAnnounceId";

                //Fill parameter from filter
                param = new OracleParameter();
                param.ParameterName = "VacancyAnnounceId";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.Int32;
                param.Value = vacancyAnnounceId;
                cmd.Parameters.Add(param);

                SQL = DBCommon.FixNewLines(SQL);

                //Set connection and comand text to command object
                cmd.Connection = conn;
                cmd.CommandText = SQL;

                //Execute command and get number of row
                count = Convert.ToInt32(cmd.ExecuteScalar());
            }
            finally
            {
                conn.Close();
            }

            return count;
        }

        public static List<ListItem> GetAllCombinedApplicantStatuses(User currentUser)
        {
            List<ListItem> statuses = new List<ListItem>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT StatusName, StatusKey 
                               FROM (
                                  SELECT a.StatusName, a.StatusKey
                                  FROM PMIS_APPL.ApplicantPositionStatus a

                                  UNION ALL

                                  SELECT a.ApplicantExamStatusName as StatusName, a.ApplicantExamStatusKey as StatusKey
                                  FROM PMIS_APPL.ApplicantExamStatuses a
                               ) tmp

                               ORDER BY CASE StatusKey
                                             WHEN 'DOCUMENTSAPPLIED' THEN 1
                                             WHEN 'PARTICIPATIONALLOWED' THEN 2
                                             WHEN 'DOCUMENTSREJECTED' THEN 3
                                             WHEN 'RATED' THEN 4
                                             WHEN 'NOTRATED' THEN 5
                                             WHEN 'APPOINTED' THEN 6
                                             WHEN 'RESERVE' THEN 7
                                             WHEN 'NOMINATED' THEN 8
                                             WHEN 'CANCELED' THEN 9
                                        END
                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ListItem li = new ListItem();
                    li.Text = dr["StatusName"].ToString();
                    li.Value = dr["StatusKey"].ToString();
                    statuses.Add(li);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return statuses;
        }

        public static bool SwapApplicantPositionsOrder(User currentUser, Change changeEntry, ApplicantPosition applicantPosition_1, ApplicantPosition applicantPosition_2)
        {
            bool result = false;

            string SQL = "";

            SQL += @"BEGIN
                     UPDATE PMIS_APPL.ApplicantPositions t
                     SET Seq = (SELECT Seq 
                                FROM PMIS_APPL.ApplicantPositions t2 
                                WHERE t2.ApplicantPositionID = :ApplicantPositionID_2)
                     WHERE t.ApplicantID = :ApplicantID AND t.ApplicantPositionID = :ApplicantPositionID_1;
                     
                     UPDATE PMIS_APPL.ApplicantPositions t2
                     SET Seq = :TmpSeq
                     WHERE t2.ApplicantID = :ApplicantID AND t2.ApplicantPositionID = :ApplicantPositionID_2;
                    END;";

            ChangeEvent changeEvent = null;
            string logDescription = "";
            logDescription += "Заповед №: " + applicantPosition_1.VacancyAnnouncePosition.VacancyAnnounceID  +
                              "; ВПН/Структура отговорна за конкурса: " + applicantPosition_1.VacancyAnnouncePosition.ResponsibleMilitaryUnit.DisplayTextForSelection +
                              "; Длъжност: " + applicantPosition_1.VacancyAnnouncePosition.PositionName;

            Applicant applicant = ApplicantUtil.GetApplicant(applicantPosition_1.ApplicantId, currentUser);

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "ApplicantID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = applicantPosition_1.ApplicantId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ApplicantPositionID_1";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = applicantPosition_1.ApplicantPositionId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ApplicantPositionID_2";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = applicantPosition_2.ApplicantPositionId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "TmpSeq";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.InputOutput;
                param.Value = applicantPosition_1.Seq;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();
                changeEvent = new ChangeEvent("APPL_Applicants_MovePosition", logDescription, applicantPosition_1.VacancyAnnouncePosition.MilitaryUnit, applicant.Person, currentUser);

                changeEvent.AddDetail(new ChangeEventDetail("APPL_ApplicantsPosition_Seq", applicantPosition_1.Seq.ToString(), applicantPosition_2.Seq.ToString(), currentUser));

                result = true;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                if (changeEvent != null && changeEvent.ChangeEventDetails.Count > 0)
                    changeEntry.AddEvent(changeEvent);
            }

            return result;
        }

        public static bool RearrangeApplicantPositions(int applicantId, int vacancyAnnounceId, int responsibleMilitaryUnitId, User currentUser)
        {
            bool result = false;
            string SQL = "";

            SQL += @"UPDATE PMIS_APPL.ApplicantPositions t
                    SET Seq = (
                          SELECT Rank
                          FROM (
                                SELECT a.ApplicantPositionID, a.ApplicantID, b.VacancyAnnounceID, b.ResponsibleMilitaryUnitID, a.Seq,
                                       RANK() OVER(PARTITION BY a.ApplicantID, b.VacancyAnnounceID, b.ResponsibleMilitaryUnitID ORDER BY a.Seq, a.ApplicantPositionID ASC NULLS LAST) Rank 
                                FROM PMIS_APPL.ApplicantPositions a
                                INNER JOIN PMIS_APPL.VacancyAnnouncePositions b ON a.VacancyAnnouncePositionID = b.VacancyAnnouncePositionID)  tmp
                          WHERE tmp.ApplicantPositionID = t.ApplicantPositionID)
                    WHERE t.ApplicantID = :ApplicantID
                    AND t.ApplicantPositionID IN (SELECT ApplicantPositionID FROM PMIS_APPL.ApplicantPositions a1
                    INNER JOIN PMIS_APPL.VacancyAnnouncePositions b1 ON a1.VacancyAnnouncePositionID = b1.VacancyAnnouncePositionID
                    WHERE a1.ApplicantID = :ApplicantID AND b1.VacancyAnnounceID = :VacancyAnnounceID AND b1.ResponsibleMilitaryUnitID = :ResponsibleMilitaryUnitID)
                    ";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "ApplicantID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = applicantId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "VacancyAnnounceID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = vacancyAnnounceId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ResponsibleMilitaryUnitID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = responsibleMilitaryUnitId;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                result = true;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }
    }
}
