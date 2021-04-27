using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using System.Linq;
using PMIS.Common;

namespace PMIS.Applicants.Common
{    
    public class ApplicantExamMark : BaseDbObject
    {
        private int applicantExamMarkId;        
        private int applicantId;        
        private Applicant applicant;        
        private int responsibleMilitaryUnitId;        
        private MilitaryUnit responsibleMilitaryUnit;        
        private int vacancyAnnounceExamId;        
        private Exam exam;        
        private int? mark;        
        private int? points;

        public int ApplicantExamMarkId
        {
            get { return applicantExamMarkId; }
            set { applicantExamMarkId = value; }
        }

        public int ApplicantId
        {
            get { return applicantId; }
            set { applicantId = value; }
        }

        public Applicant Applicant
        {
            get { return applicant; }
            set { applicant = value; }
        }

        public int ResponsibleMilitaryUnitId
        {
            get { return responsibleMilitaryUnitId; }
            set { responsibleMilitaryUnitId = value; }
        }

        public MilitaryUnit ResponsibleMilitaryUnit
        {
            get { return responsibleMilitaryUnit; }
            set { responsibleMilitaryUnit = value; }
        }

        public int VacancyAnnounceExamId
        {
            get { return vacancyAnnounceExamId; }
            set { vacancyAnnounceExamId = value; }
        }

        public Exam Exam
        {
            get { return exam; }
            set { exam = value; }
        }

        public int? Mark
        {
            get { return mark; }
            set { mark = value; }
        }

        public int? Points
        {
            get { return points; }
            set { points = value; }
        }

        public ApplicantExamMark(User user)
            : base(user)
        {

        }
    }

    public class ApplicantExamMarkBlock
    {
        private int? applicantExamMarkId;
        private int vacancyAnnounceExamId;        
        private string examName;        
        private int? mark;        
        private int? points;

        public int? ApplicantExamMarkId
        {
            get { return applicantExamMarkId; }
            set { applicantExamMarkId = value; }
        }

        public int VacancyAnnounceExamId
        {
            get { return vacancyAnnounceExamId; }
            set { vacancyAnnounceExamId = value; }
        }

        public string ExamName
        {
            get { return examName; }
            set { examName = value; }
        }

        public int? Mark
        {
            get { return mark; }
            set { mark = value; }
        }

        public int? Points
        {
            get { return points; }
            set { points = value; }
        }
    }

    public class ApplicantExamsBlock : BaseDbObject
    {
        private int applicantId;
        private string applicantName;
        private string applicantIdentNumber;
        private string militaryDepartmentName;  
        private int? applicantExamStatus;
        private int personID;
        private int responsibleMilitaryUnitID;
        private int vacancyAnnounceID;        
        private List<ApplicantExamMarkBlock> marks;
        private bool hasRelatedApplPositionsWithHigherStatus;

        public int ApplicantId
        {
            get { return applicantId; }
            set { applicantId = value; }
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

        public int? ApplicantExamStatus
        {
            get { return applicantExamStatus; }
            set { applicantExamStatus = value; }
        }

        public int PersonID
        {
            get { return personID; }
            set { personID = value; }
        }

        public int ResponsibleMilitaryUnitID
        {
            get { return responsibleMilitaryUnitID; }
            set { responsibleMilitaryUnitID = value; }
        }

        public int VacancyAnnounceID
        {
            get { return vacancyAnnounceID; }
            set { vacancyAnnounceID = value; }
        }

        public List<ApplicantExamMarkBlock> Marks
        {
            get { return marks; }
            set { marks = value; }
        }

        public bool HasRelatedApplPositionsWithHigherStatus
        {
            get
            {
                return hasRelatedApplPositionsWithHigherStatus;
            }
            set
            {
                hasRelatedApplPositionsWithHigherStatus = value;
            }
        }

        public ApplicantExamsBlock(User user)
            : base(user)
        {

        }
    }

    public static class ApplicantExamMarkUtil
    {
        //This method fillings up an ApplicantExamMark object. It extracts the data from a DataReader.
        private static ApplicantExamMark ExtractApplicantExamMarkFromDataReader(OracleDataReader dr, User currentUser)
        {
            ApplicantExamMark applicantExamMark = new ApplicantExamMark(currentUser);

            applicantExamMark.ApplicantExamMarkId = DBCommon.GetInt(dr["ApplicantExamMarkID"]);
            applicantExamMark.ApplicantId = DBCommon.GetInt(dr["ApplicantID"]);
            applicantExamMark.ResponsibleMilitaryUnitId = DBCommon.GetInt(dr["ResponsibleMilitaryUnitID"]);
            applicantExamMark.VacancyAnnounceExamId = DBCommon.GetInt(dr["VacancyAnnounceExamID"]);
            applicantExamMark.Mark = (DBCommon.IsInt(dr["Mark"]) ? (int?)DBCommon.GetInt(dr["Mark"]) : null);
            applicantExamMark.Points = (DBCommon.IsInt(dr["Points"]) ? (int?)DBCommon.GetInt(dr["Points"]) : null);
           
            return applicantExamMark;
        }

        public static ApplicantExamMark GetApplicantExamMark(int applicantExamMarkId, User currentUser)
        {
            ApplicantExamMark applicantExamMark = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                string SQL = @"SELECT a.ApplicantExamMarkID, a.ApplicantID, a.ResponsibleMilitaryUnitID, a.VacancyAnnounceExamID, a.Mark, a.Points
                               FROM PMIS_APPL.ApplicantExamMarks a
                               WHERE a.ApplicantExamMarkID = :ApplicantExamMarkID " + where;

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    SQL += @" AND (a.ResponsibleMilitaryUnitID IS NULL OR a.ResponsibleMilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @"))
                            ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ApplicantExamMarkID", OracleType.Number).Value = applicantExamMarkId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    applicantExamMark = ExtractApplicantExamMarkFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return applicantExamMark;
        }

        public static List<ApplicantExamsBlock> GetAllApplicantExamsBlock(int vacancyAnnounceId, int responsibleMilitaryUnit, User currentUser)
        {
            List<ApplicantExamsBlock> applicantExamsBlocks = new List<ApplicantExamsBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    where += @" AND (c.ResponsibleMilitaryUnitID IS NULL OR c.ResponsibleMilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @"))
                            ";

                where += " AND a.ApplicantStatusID IS NOT NULL AND a.ApplicantStatusID NOT IN (SELECT STATUSID FROM PMIS_APPL.APPLICANTPOSITIONSTATUS WHERE STATUSKEY IN ('DOCUMENTSAPPLIED', 'DOCUMENTSREJECTED'))";    

                string SQL = @"SELECT NVL(f.ApplicantExamMarkID, -1) as ApplicantExamMarkID, 
                                      b.ApplicantID,
                                      b.PersonID,
                                      h.IME as FirstName, 
                                      h.FAM as LastName, 
                                      h.EGN as IdentNumber,      
                                      dep.MilitaryDepartmentName,
                                      d.VacancyAnnounceExamID,
                                      examNames.ExamName,
                                      f.Mark,
                                      f.Points,
                                      g.Status as ApplicantExamStatus,
                                      SUM(CASE WHEN i.StatusKey IN ('APPOINTED', 'RESERVE', 'NOMINATED', 'CANCELED')
                                            THEN 1
                                            ELSE 0
                                          END) as HasRelApplPosWithHigherStatus
                               FROM PMIS_APPL.ApplicantPositions a
                               INNER JOIN PMIS_APPL.Applicants b on a.ApplicantID = b.ApplicantID
                               INNER JOIN PMIS_ADM.MilitaryDepartments dep ON b.MilitaryDepartmentID = dep.MilitaryDepartmentID
                               INNER JOIN PMIS_APPL.VacancyAnnouncePositions c on a.VacancyAnnouncePositionID = c.VacancyAnnouncePositionID 
                               INNER JOIN PMIS_APPL.VacancyAnnounceExams d on d.VacancyAnnounceID = c.VacancyAnnounceID
                               INNER JOIN PMIS_APPL.Exams examNames on examNames.ExamID = d.ApplicantExamID
                               LEFT OUTER JOIN PMIS_APPL.ApplicantExamMarks f ON a.ApplicantID = f.ApplicantID AND c.ResponsibleMilitaryUnitID = f.ResponsibleMilitaryUnitID AND d.VacancyAnnounceExamID = f.VacancyAnnounceExamID
                               LEFT OUTER JOIN PMIS_APPL.ApplicantExamStatus g ON b.ApplicantID = g.ApplicantID AND c.ResponsibleMilitaryUnitID = g.ResponsibleMilitaryUnitID AND c.VacancyAnnounceID = g.VacancyAnnounceID
                               INNER JOIN VS_OWNER.VS_LS h ON b.PersonID = h.PersonID
                               INNER JOIN PMIS_APPL.ApplicantPositionStatus i ON a.ApplicantStatusID = i.StatusID
                               WHERE d.VacancyAnnounceID = :VacancyAnnounceID AND c.ResponsibleMilitaryUnitID = :ResponsibleMilitaryUnitID " + where + @"
                               GROUP BY f.ApplicantExamMarkID, 
                                        b.ApplicantID,
                                        b.PersonID,
                                        h.IME, 
                                        h.FAM, 
                                        h.EGN,      
                                        dep.MilitaryDepartmentName,
                                        d.VacancyAnnounceExamID,
                                        examNames.ExamName,
                                        f.Mark,
                                        f.Points,
                                        g.Status
                               ORDER BY b.ApplicantID, examNames.ExamName, d.VacancyAnnounceExamID";
                
                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VacancyAnnounceID", OracleType.Number).Value = vacancyAnnounceId;
                cmd.Parameters.Add("ResponsibleMilitaryUnitID", OracleType.Number).Value = responsibleMilitaryUnit;

                OracleDataReader dr = cmd.ExecuteReader();

                ApplicantExamsBlock lastBlock = null;

                while (dr.Read())
                {
                    if (lastBlock != null && lastBlock.ApplicantId != DBCommon.GetInt(dr["ApplicantID"]))
                    {
                        applicantExamsBlocks.Add(lastBlock);
                        lastBlock = null;
                    }

                    if (lastBlock == null)
                    {
                        lastBlock = new ApplicantExamsBlock(currentUser);
                        lastBlock.ApplicantId = DBCommon.GetInt(dr["ApplicantID"]);
                        lastBlock.PersonID = DBCommon.GetInt(dr["PersonID"]);
                        lastBlock.ResponsibleMilitaryUnitID = responsibleMilitaryUnit;
                        lastBlock.VacancyAnnounceID = vacancyAnnounceId;
                        lastBlock.ApplicantName = dr["FirstName"].ToString() + " " + dr["LastName"].ToString();
                        lastBlock.ApplicantIdentNumber = dr["IdentNumber"].ToString();
                        lastBlock.MilitaryDepartmentName = dr["MilitaryDepartmentName"].ToString();
                        lastBlock.ApplicantExamStatus = DBCommon.IsInt(dr["ApplicantExamStatus"]) ? (int?)DBCommon.GetInt(dr["ApplicantExamStatus"]) : null;
                        lastBlock.Marks = new List<ApplicantExamMarkBlock>();
                        lastBlock.HasRelatedApplPositionsWithHigherStatus = DBCommon.GetInt(dr["HasRelApplPosWithHigherStatus"]) == 1;
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
                    applicantExamsBlocks.Add(lastBlock);

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return applicantExamsBlocks;
        }

        public static bool SaveApplicantExamsBlocks(List<ApplicantExamsBlock> oldExams, List<ApplicantExamsBlock> newExams, string orderNumDate, string militaryUnitName, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            var exams = (from n in newExams
                         join o in oldExams on n.ApplicantId equals o.ApplicantId
                         select new { n.ApplicantId, o.ApplicantName, o.ApplicantIdentNumber, o.MilitaryDepartmentName, o.PersonID, o.ResponsibleMilitaryUnitID, o.VacancyAnnounceID, newExamStatusID = n.ApplicantExamStatus, oldExamStatusID = o.ApplicantExamStatus, newMarks = n.Marks, oldMarks = o.Marks });

            List<ApplicantExamStatus> statuses = ApplicantExamStatusUtil.GetAllApplicantExamStatuses(currentUser);
            List<ApplicantPositionStatus> positionStatuses = ApplicantPositionStatusUtil.GetAllApplicantPositionStatus(currentUser);
            ApplicantPositionStatus allowedStatus = (from s in positionStatuses where s.StatusKey == "PARTICIPATIONALLOWED" select s).FirstOrDefault();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                foreach (var exam in exams)
                {
                    Person person = new Person(currentUser);
                    person.PersonId = exam.PersonID;

                    MilitaryUnit responsibleMilitaryUnit = new MilitaryUnit(currentUser);
                    responsibleMilitaryUnit.MilitaryUnitId = exam.ResponsibleMilitaryUnitID;

                    if (exam.oldExamStatusID != exam.newExamStatusID)
                    {
                        string logDescription = "";
                        logDescription += "Кандидат: " + exam.ApplicantName;
                        logDescription += "<br />ЕГН: " + exam.ApplicantIdentNumber;
                        logDescription += "<br />Военно окръжие: " + exam.MilitaryDepartmentName;
                        logDescription += "<br />Заповед №: " + orderNumDate;
                        logDescription += "<br />" + CommonFunctions.GetLabelText("MilitaryUnit") + " отговорна за конкурса: " + militaryUnitName;

                        string oldStatusName = (from s in statuses where s.StatusId == exam.oldExamStatusID select s.StatusName).FirstOrDefault();
                        string newStatusName = (from s in statuses where s.StatusId == exam.newExamStatusID select s.StatusName).FirstOrDefault();

                        if(String.IsNullOrEmpty(oldStatusName))
                            oldStatusName = allowedStatus.StatusName;

                        if(String.IsNullOrEmpty(newStatusName))
                            newStatusName = allowedStatus.StatusName;

                        ChangeEvent changeEvent = new ChangeEvent("APPL_Applicants_ExamStatus", logDescription, responsibleMilitaryUnit, person, currentUser);

                        changeEvent.AddDetail(new ChangeEventDetail("APPL_Applicants_ExamStatus", oldStatusName, newStatusName, currentUser));

                        SQL += @" MERGE INTO PMIS_APPL.ApplicantExamStatus USING dual ON (ApplicantID=" + exam.ApplicantId.ToString() + @" AND ResponsibleMilitaryUnitID=" + exam.ResponsibleMilitaryUnitID.ToString() + @" AND VacancyAnnounceID=" + exam.VacancyAnnounceID.ToString() + @" )
                                  WHEN MATCHED THEN UPDATE SET Status = " + (exam.newExamStatusID.HasValue ? exam.newExamStatusID.Value.ToString() : "NULL") + @"
                                  WHEN NOT MATCHED THEN INSERT (ApplicantID, ResponsibleMilitaryUnitID, VacancyAnnounceID, Status) VALUES ( " + exam.ApplicantId.ToString() + @", " + exam.ResponsibleMilitaryUnitID.ToString() + @", " + exam.VacancyAnnounceID.ToString() + @", " + (exam.newExamStatusID.HasValue ? exam.newExamStatusID.Value.ToString() : "NULL") + @" ) ;
                                ";

                        changeEntry.AddEvent(changeEvent);
                    }

                    var marks = (from n in exam.newMarks
                                 join o in exam.oldMarks on n.VacancyAnnounceExamId equals o.VacancyAnnounceExamId
                                 select new { n.ApplicantExamMarkId, o.ExamName, o.VacancyAnnounceExamId, newMark = n.Mark, newPoints = n.Points, oldMark = o.Mark, oldPoints = o.Points });

                    foreach (var mark in marks)
                    {
                        string logDescription = "";
                        logDescription += "Кандидат: " + exam.ApplicantName;
                        logDescription += "<br />ЕГН: " + exam.ApplicantIdentNumber;
                        logDescription += "<br />Военно окръжие: " + exam.MilitaryDepartmentName;
                        logDescription += "<br />Заповед №: " + orderNumDate;
                        logDescription += "<br />" + CommonFunctions.GetLabelText("MilitaryUnit") + " отговорна за конкурса: " + militaryUnitName;
                        logDescription += "<br />Изпит: " + mark.ExamName;

                        if (mark.ApplicantExamMarkId == 0)
                        {
                            ChangeEvent changeEvent = new ChangeEvent("APPL_Applicants_AddExamMark", logDescription, responsibleMilitaryUnit, person, currentUser);
                            changeEvent.AddDetail(new ChangeEventDetail("APPL_Applicants_ExamMark", "" , mark.newMark.HasValue ? mark.newMark.Value.ToString() : "" , currentUser));
                            changeEvent.AddDetail(new ChangeEventDetail("APPL_Applicants_ExamPoints", "", mark.newPoints.HasValue ? mark.newPoints.Value.ToString() : "", currentUser));
                            changeEntry.AddEvent(changeEvent);
                        }
                        else
                        {
                            ChangeEvent changeEvent = new ChangeEvent("APPL_Applicants_EditExamMark", logDescription, responsibleMilitaryUnit, person, currentUser);

                            if (mark.newMark != mark.oldMark)
                            {
                                changeEvent.AddDetail(new ChangeEventDetail("APPL_Applicants_ExamMark", mark.oldMark.HasValue ? mark.oldMark.Value.ToString() : "", mark.newMark.HasValue ? mark.newMark.Value.ToString() : "", currentUser));
                            }

                            if (mark.newPoints != mark.oldPoints)
                            {
                                changeEvent.AddDetail(new ChangeEventDetail("APPL_Applicants_ExamMark", mark.oldPoints.HasValue ? mark.oldPoints.Value.ToString() : "", mark.newPoints.HasValue ? mark.newPoints.Value.ToString() : "", currentUser));
                            }

                            if (changeEvent.ChangeEventDetails.Count > 0)
                                changeEntry.AddEvent(changeEvent);
                        }

                        SQL += @" MERGE INTO PMIS_APPL.ApplicantExamMarks USING dual ON (ApplicantExamMarkID=" + mark.ApplicantExamMarkId.ToString() + @" )
                                  WHEN MATCHED THEN UPDATE SET Mark = " + (mark.newMark.HasValue ? mark.newMark.Value.ToString() : "NULL") + @", Points = " + (mark.newPoints.HasValue ? mark.newPoints.Value.ToString() : "NULL") + @" 
                                  WHEN NOT MATCHED THEN INSERT (ApplicantID,ResponsibleMilitaryUnitID, VacancyAnnounceExamID, Mark, Points) VALUES ( " + exam.ApplicantId.ToString() + @", " + exam.ResponsibleMilitaryUnitID.ToString() + @", " + mark.VacancyAnnounceExamId.ToString() + @", " + (mark.newMark.HasValue ? mark.newMark.Value.ToString() : "NULL") + @", " + (mark.newPoints.HasValue ? mark.newPoints.Value.ToString() : "NULL") + @" ) ;
                                ";                        
                    }                  
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
                ApplicantUtil.SetApplicantsModified((from p in oldExams select p.ApplicantId).ToList(), currentUser);

            return result;
        }
    }

    

}