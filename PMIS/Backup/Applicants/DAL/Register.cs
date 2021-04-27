using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMIS.Common;
using PMIS.Applicants.Common;
using System.Data.OracleClient;
using System.Data;

namespace PMIS.Applicants.Common
{
    public class Register : BaseDbObject
    {
        private int applicantId;
        private Applicant applicant;
        private int vacancyAnnounceId;
        private VacancyAnnounce vacancyAnnounce;
        private int responsibleMilitaryUnitId;
        private MilitaryUnit responsibleMilitaryUnit;
        private int registerNumber;
        private DateTime? documentDate;
        private string pageCount;
        private string notes;

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

        public int VacancyAnnounceId
        {
            get { return vacancyAnnounceId; }
            set { vacancyAnnounceId = value; }
        }

        public VacancyAnnounce VacancyAnnounce
        {
            get { return vacancyAnnounce; }
            set { vacancyAnnounce = value; }
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

        public int RegisterNumber
        {
            get { return registerNumber; }
            set { registerNumber = value; }
        }

        public DateTime? DocumentDate
        {
            get { return documentDate; }
            set { documentDate = value; }
        }

        public string PageCount
        {
            get { return pageCount; }
            set { pageCount = value; }
        }

        public string Notes
        {
            get { return notes; }
            set { notes = value; }
        }

        public Register(User user)
            : base(user)
        {
        }
    }

    public class RegisterYear : IDropDownItem
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

    public static class RegisterUtil
    {
        public static Register GetRegister(int applicantId, int vacancyAnnounceId, int responsibleMilitaryUnitId, User currentUser)
        {
            Register register = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.ApplicantID, a.VacancyAnnounceID, a.ResponsibleMilitaryUnitID, 
                                      a.RegisterNumber, a.DocumentDate, a.PageCount, a.Notes 
                               FROM PMIS_APPL.Register a
                               WHERE a.ApplicantID = :ApplicantID AND a.VacancyAnnounceID = :VacancyAnnounceID AND a.ResponsibleMilitaryUnitID = :ResponsibleMilitaryUnitID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ApplicantID", OracleType.Number).Value = applicantId;
                cmd.Parameters.Add("VacancyAnnounceID", OracleType.Number).Value = vacancyAnnounceId;
                cmd.Parameters.Add("ResponsibleMilitaryUnitID", OracleType.Number).Value = responsibleMilitaryUnitId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    register = new Register(currentUser);

                    if (DBCommon.IsInt(dr["ApplicantID"]))
                        register.ApplicantId = DBCommon.GetInt(dr["ApplicantID"]);

                    if (DBCommon.IsInt(dr["VacancyAnnounceID"]))
                        register.VacancyAnnounceId = DBCommon.GetInt(dr["VacancyAnnounceID"]);

                    if (DBCommon.IsInt(dr["ResponsibleMilitaryUnitID"]))
                        register.ResponsibleMilitaryUnitId = DBCommon.GetInt(dr["ResponsibleMilitaryUnitID"]);

                    if (DBCommon.IsInt(dr["RegisterNumber"]))
                        register.RegisterNumber = DBCommon.GetInt(dr["RegisterNumber"]);

                    if (dr["DocumentDate"] is DateTime)
                        register.DocumentDate = (DateTime)dr["DocumentDate"];

                    register.PageCount = dr["PageCount"].ToString();
                    register.Notes = dr["Notes"].ToString();

                    register.Applicant = ApplicantUtil.GetApplicant(register.ApplicantId, currentUser);
                    register.VacancyAnnounce = VacancyAnnounceUtil.GetVacancyAnnounce(register.VacancyAnnounceId, currentUser);
                    register.ResponsibleMilitaryUnit = MilitaryUnitUtil.GetMilitaryUnit(register.ResponsibleMilitaryUnitId, currentUser);

                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return register;
        }

        public static List<RegisterYear> GetAllRegisterYears(User currentUser)
        {
            var registerYears = new List<RegisterYear>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT DISTINCT TO_CHAR(a.DocumentDate, 'YYYY') AS Year
                               FROM PMIS_APPL.Register a
                               WHERE a.DocumentDate IS NOT NULL
                               ORDER BY Year DESC";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    RegisterYear registerYear = new RegisterYear()
                    {
                        Year = DBCommon.GetInt(dr["Year"]),
                        YearValue = dr["Year"].ToString()
                    };

                    registerYears.Add(registerYear);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return registerYears;
        }

        public static bool AddNewRegister(Register register, User currentUser, Change changeEntry)
        {
            bool result = false;

            ChangeEvent changeEvent;

            string logDescription = "";
            logDescription += "Военно окръжие: " + register.Applicant.MilitaryDepartment.MilitaryDepartmentName;
            logDescription += "<br />Заповед №: " + register.VacancyAnnounce.OrderNum;
            logDescription += "<br />ВПН/Структура отговорна за конкурса: " + register.ResponsibleMilitaryUnit.DisplayTextForSelection;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"INSERT INTO PMIS_APPL.Register
                                (ApplicantID, VacancyAnnounceID, ResponsibleMilitaryUnitID, RegisterNumber)
                                VALUES (:ApplicantID, :VacancyAnnounceID, :ResponsibleMilitaryUnitID, :RegisterNumber)";

                changeEvent = GetChangeEvent("APPL_Applicants_AddRegister", logDescription, null, register, currentUser);
                //changeEvent = new ChangeEvent("APPL_Applicants_AddRegister", logDescription, null, register.Applicant.Person, currentUser);

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ApplicantId", OracleType.Number).Value = register.ApplicantId;
                cmd.Parameters.Add("VacancyAnnounceID", OracleType.Number).Value = register.VacancyAnnounceId;
                cmd.Parameters.Add("ResponsibleMilitaryUnitID", OracleType.Number).Value = register.ResponsibleMilitaryUnitId;
                cmd.Parameters.Add("RegisterNumber", OracleType.Number).Value = register.RegisterNumber;

                cmd.ExecuteNonQuery();

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

        public static bool UpdateRegister(Register register, User currentUser, Change changeEntry)
        {
            bool result = false;

            ChangeEvent changeEvent;

            string logDescription = "";
            logDescription += "Военно окръжие: " + register.Applicant.MilitaryDepartment.MilitaryDepartmentName;
            logDescription += "<br />Заповед №: " + register.VacancyAnnounce.OrderNum;
            logDescription += "<br />ВПН/Структура отговорна за конкурса: " + register.ResponsibleMilitaryUnit.DisplayTextForSelection;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"UPDATE PMIS_APPL.Register
                                SET DocumentDate = :DocumentDate, 
                                    PageCount = :PageCount,
                                    Notes = :Notes
                                WHERE ApplicantId = :ApplicantId AND VacancyAnnounceID = :VacancyAnnounceID AND ResponsibleMilitaryUnitID = :ResponsibleMilitaryUnitID";

                Register oldRegister = GetRegister(register.Applicant.ApplicantId, register.VacancyAnnounce.VacancyAnnounceId, register.ResponsibleMilitaryUnit.MilitaryUnitId, currentUser);
                changeEvent = GetChangeEvent("APPL_Applicants_EditRegister", logDescription, oldRegister, register, currentUser);
                
                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ApplicantId", OracleType.Number).Value = register.ApplicantId;
                cmd.Parameters.Add("VacancyAnnounceID", OracleType.Number).Value = register.VacancyAnnounceId;
                cmd.Parameters.Add("ResponsibleMilitaryUnitID", OracleType.Number).Value = register.ResponsibleMilitaryUnitId;

                OracleParameter param = new OracleParameter();
                param.ParameterName = "DocumentDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (register.DocumentDate is DateTime)
                    param.Value = register.DocumentDate;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PageCount";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(register.PageCount))
                    param.Value = register.PageCount;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Notes";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(register.Notes))
                    param.Value = register.Notes;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);



                cmd.ExecuteNonQuery();

                result = true;
            }
            finally
            {
                conn.Close();
            }

            if (result && changeEvent.ChangeEventDetails.Count > 0)
            {
                changeEntry.AddEvent(changeEvent);
            }

            return result;
        }

        public static ChangeEvent GetChangeEvent(string changeEventType, string logDescription, Register oldRegister, Register register, User currentUser)
        {
            ChangeEvent changeEvent = changeEvent = new ChangeEvent(changeEventType, logDescription, null, register.Applicant.Person, currentUser);

            if (oldRegister == null)
            {
                changeEvent.AddDetail(new ChangeEventDetail("APPL_Applicants_Register_RegNumber", "",
                register.RegisterNumber.ToString(), currentUser));
            }
            else
            {
                if (oldRegister.DocumentDate != register.DocumentDate)
                {
                    var oldRegisterDate = oldRegister.DocumentDate.HasValue ? oldRegister.DocumentDate.Value.ToString("dd.MM.yyyy") : "";
                    var newRegisterDate = register.DocumentDate.HasValue ? register.DocumentDate.Value.ToString("dd.MM.yyyy") : "";

                    changeEvent.AddDetail(new ChangeEventDetail("APPL_Applicants_Register_Date",
                                 oldRegisterDate,
                                 newRegisterDate, currentUser));
                }

                if (oldRegister.PageCount != register.PageCount)
                    changeEvent.AddDetail(new ChangeEventDetail("APPL_Applicants_Register_PageCount",
                                 oldRegister.PageCount,
                                 register.PageCount, currentUser));

                if (oldRegister.Notes != register.Notes)
                    changeEvent.AddDetail(new ChangeEventDetail("APPL_Applicants_Register_Notes",
                                 oldRegister.Notes,
                                 register.Notes, currentUser));
            }

            return changeEvent;
        }
    }
}
