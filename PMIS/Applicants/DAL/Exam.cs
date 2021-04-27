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
    public class Exam : BaseDbObject
    {
        private int examId;
        public int ExamId
        {
            get { return examId; }
            set { examId = value; }
        }

        private string examName;
        public string ExamName
        {
            get { return examName; }
            set { examName = value; }
        }

        public Exam(User user)
            : base(user)
        {

        }

    }

    public class ExamUtil
    {
        public static Exam GetExam(int examId, User currentUser)
        {
            Exam exam = null;
            if (examId == 0)
            {
                return exam;
            }
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.ExamName as ExamName FROM PMIS_APPL.EXAMS a
                               WHERE a.examId = :examId";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("examId", OracleType.Number).Value = examId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    exam = new Exam(currentUser);
                    exam.ExamId = examId;
                    exam.ExamName = dr["ExamName"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return exam;
        }

        public static List<Exam> GetAllExams(User currentUser)
        {
            Exam exam;
            List<Exam> lstExam = new List<Exam>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @" SELECT  a.ExamId as ExamId, a.ExamName as ExamName FROM PMIS_APPL.EXAMS a                             
                                order by a.ExamName";

                OracleCommand cmd = new OracleCommand(SQL, conn);
                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    exam = new Exam(currentUser);
                    exam.ExamId = DBCommon.GetInt(dr["ExamId"]);
                    exam.ExamName = dr["ExamName"].ToString();
                    lstExam.Add(exam);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }
            return lstExam;
        }

        public static List<Exam> GetExamsForVacancyAnnounce(int vacancyAnnounceId, User currentUser)
        {
            Exam exam;
            List<Exam> lstExam = new List<Exam>();

            if (vacancyAnnounceId == 0)
            {
                return lstExam;
            }
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @" SELECT  a.ExamId as ExamId, a.ExamName as ExamName FROM PMIS_APPL.EXAMS a                             
                                WHERE a.ExamId in (SELECT APPLICANTEXAMID FROM PMIS_APPL.VACANCYANNOUNCEEXAMS 
                                WHERE VACANCYANNOUNCEID = :vacancyAnnounceId)
                                order by a.ExamName";

                OracleCommand cmd = new OracleCommand(SQL, conn);
                OracleParameter param = new OracleParameter();

                param = new OracleParameter();
                param.ParameterName = "vacancyAnnounceId";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.Int32;
                param.Value = vacancyAnnounceId;
                cmd.Parameters.Add(param);
                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    exam = new Exam(currentUser);
                    exam.ExamId = DBCommon.GetInt(dr["ExamId"]);
                    exam.ExamName = dr["ExamName"].ToString();
                    lstExam.Add(exam);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }
            return lstExam;
        }

        //Perform DELETE and return true/false
        public static bool DeleteExam(int examId, User currentUser, Change changeEntry)
        {
            string SQL = "";
            bool isDeleted = false;

            //Create Old Exam obect using GetInvestigationProtocol method
            Exam oldExam = GetExam(examId, currentUser);

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();
            try
            {
                SQL = @"DELETE FROM PMIS_APPL.EXAMS WHERE examId = :examId";
                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "examId";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.Number;
                param.Value = examId;
                cmd.Parameters.Add(param);

                isDeleted = cmd.ExecuteNonQuery() == 1;
            }
            finally
            {
                conn.Close();
            }
            return isDeleted;
        }

        //Perform DELETE from Table VACANCYANNOUNCEEXAMS and return true/false
        public static bool DeleteExamForVacancyAnnounce(VacancyAnnounce vacancyAnnounce, Exam exam, User currentUser, Change changeEntry)
        {
            string SQL = "";
            bool isDeleted = false;

            ChangeEvent changeEvent;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();
            try
            {
                SQL = @"DELETE FROM PMIS_APPL.VACANCYANNOUNCEEXAMS WHERE VACANCYANNOUNCEID = :vacancyAnnounceId
                                                                   AND APPLICANTEXAMID = :applicantExamId";
                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "vacancyAnnounceId";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.Number;
                param.Value = vacancyAnnounce.VacancyAnnounceId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "applicantExamId";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.Number;
                param.Value = exam.ExamId;
                cmd.Parameters.Add(param);

                isDeleted = cmd.ExecuteNonQuery() == 1;
            }
            finally
            {
                conn.Close();
            }
            if (isDeleted)
            {
                string logDescription = "";
                logDescription += "Заповед №: " + vacancyAnnounce.OrderNum + " / Дата:" + CommonFunctions.FormatDate(vacancyAnnounce.OrderDate);

                //Create obect using log for Delete records
                changeEvent = new ChangeEvent("APPL_VacAnn_DeleteExam", logDescription, null , null, currentUser);
                //Fill object with data
                changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnExam_Name", exam.ExamName, "", currentUser));
                //Add Event 
                changeEntry.AddEvent(changeEvent);
            }
            return isDeleted;
        }

        //Perform ADD value in Table VACANCYANNOUNCEEXAMS and return true/false
        public static bool AddExamForVacancyAnnounce(VacancyAnnounce vacancyAnnounce, Exam exam, User currentUser, Change changeEntry)
        {
            string SQL = "";
            bool isAdded = false;

            ChangeEvent changeEvent;

            if (vacancyAnnounce.VacancyAnnounceId == 0) return isAdded;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"INSERT INTO PMIS_APPL.VACANCYANNOUNCEEXAMS (
                                VACANCYANNOUNCEID, 
                                APPLICANTEXAMID)
                        VALUES (
                                :vacancyAnnounceId, 
                                :examId)";
                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "vacancyAnnounceId";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.Number;
                param.Value = vacancyAnnounce.VacancyAnnounceId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "examId";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.Number;
                param.Value = exam.ExamId;
                cmd.Parameters.Add(param);

                isAdded = cmd.ExecuteNonQuery() == 1;
            }
            finally
            {
                conn.Close();
            }

            if (isAdded)
            {
                //Create obect using log for Add records
                changeEvent = new ChangeEvent("APPL_VacAnn_AddExam", "Заповед №: " + vacancyAnnounce.OrderNum + " / Дата:" + CommonFunctions.FormatDate(vacancyAnnounce.OrderDate), null, null, currentUser);
                //Fill object with data
                changeEvent.AddDetail(new ChangeEventDetail("APPL_VacAnnExam_Name", "", exam.ExamName, currentUser));
                //Add Event 
                changeEntry.AddEvent(changeEvent);
            }

            return isAdded;
        }
    }

}