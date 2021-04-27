using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Applicants.Common
{
    //It represents a single cadet school subject item from CadetSchoolSubjects table
    public class CadetSchoolSubject : BaseDbObject
    {
        private int cadetSchoolSubjectId;
        private int cadetId;
        private int militSchoolSpecId;
        private bool isRanked;

        private Cadet cadet;
        private MilitarySchoolSpecialization militSchoolSpec;

        public int CadetSchoolSubjectId
        {
            get { return cadetSchoolSubjectId; }
            set { cadetSchoolSubjectId = value; }
        }

        public int CadetId
        {
            get { return cadetId; }
            set { cadetId = value; }
        }

        public int MilitSchoolSpecId
        {
            get { return militSchoolSpecId; }
            set { militSchoolSpecId = value; }
        }

        public bool IsRanked
        {
            get { return isRanked; }
            set { isRanked = value; }
        }

        public Cadet Cadet
        {
            get
            {
                if (cadet == null)
                {
                    cadet = CadetUtil.GetCadet(cadetId, base.CurrentUser);
                }
                return cadet;

            }
            set { cadet = value; }
        }

        public MilitarySchoolSpecialization MilitarySchoolSpecialization
        {
            get
            {
                if (militSchoolSpec == null)
                {
                    militSchoolSpec = MilitarySchoolSpecializationUtil.GetMilitarySchoolSpecialization(militSchoolSpecId, base.CurrentUser);
                }
                return militSchoolSpec;

            }
            set { militSchoolSpec = value; }
        }

        public CadetSchoolSubject(User user) :base(user)
        {

        }
    }

    public class CadetSchoolSubjectFilter
    {
        int militarySchoolId;
        int schoolYear;
        int specializationId;
        string identityNumber;

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

        public string IdentityNumber
        {
            get { return identityNumber; }
            set { identityNumber = value; }
        }
    }

    public static class CadetSchoolSubjectUtil
    {
        //This method creates and returns a CadetSchoolSubject object. It extracts the data from a DataReader.
        public static CadetSchoolSubject ExtractCadetSchoolSubjectFromDataReader(OracleDataReader dr, User currentUser)
        {
            CadetSchoolSubject cadetSchoolSubject = new CadetSchoolSubject(currentUser);

            if (DBCommon.IsInt(dr["CadetSchoolSubjectID"]))
                cadetSchoolSubject.CadetSchoolSubjectId = DBCommon.GetInt(dr["CadetSchoolSubjectID"]);

            if (DBCommon.IsInt(dr["CadetID"]))
                cadetSchoolSubject.CadetId = DBCommon.GetInt(dr["CadetID"]);

            if (DBCommon.IsInt(dr["MilitSchoolSpecID"]))
                cadetSchoolSubject.MilitSchoolSpecId = DBCommon.GetInt(dr["MilitSchoolSpecID"]);

            cadetSchoolSubject.IsRanked = (dr["IsRanked"].ToString() == "1" ? true : false);

            return cadetSchoolSubject;
        }

        public static CadetSchoolSubject GetCadetSchoolSubject(int cadetSchoolSubjectId, User currentUser)
        {
            CadetSchoolSubject cadetSchoolSubject = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.CadetSchoolSubjectID, a.CadetID, 
                                    a.MilitSchoolSpecID, a.IsRanked
                                  FROM PMIS_APPL.CadetSchoolSubjects a
                                  WHERE a.CadetSchoolSubjectID = :CadetSchoolSubjectID ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("CadetSchoolSubjectID", OracleType.Number).Value = cadetSchoolSubjectId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    cadetSchoolSubject = ExtractCadetSchoolSubjectFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return cadetSchoolSubject;
        }

        public static List<CadetSchoolSubject> GetAllCadetSchoolSubjectsByCadetID(int cadetId, User currentUser)
        {
            List<CadetSchoolSubject> cadetSchoolSubjects = new List<CadetSchoolSubject>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.CadetSchoolSubjectID, a.CadetID, 
                                    a.MilitSchoolSpecID, a.IsRanked
                                FROM PMIS_APPL.CadetSchoolSubjects a
                                WHERE a.CadetID = :CadetID ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("CadetID", OracleType.Number).Value = cadetId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    CadetSchoolSubject applicantDocument = ExtractCadetSchoolSubjectFromDataReader(dr, currentUser);
                    cadetSchoolSubjects.Add(applicantDocument);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }
            return cadetSchoolSubjects;
        }

        public static List<CadetSchoolSubject> GetAllCadetSchoolSubjectsByPersonID(int personId, int year, User currentUser)
        {
            List<CadetSchoolSubject> cadetSchoolSubjects = new List<CadetSchoolSubject>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.CadetSchoolSubjectID, a.CadetID, 
                                    a.MilitSchoolSpecID, a.IsRanked
                                FROM PMIS_APPL.CadetSchoolSubjects a
                                INNER JOIN PMIS_APPL.Cadets b ON a.CadetID = b.CadetID
                                INNER JOIN PMIS_APPL.MilitarySchoolSpecializations c ON a.MilitSchoolSpecID = c.MilitSchoolSpecID
                                WHERE b.PersonID = :PersonID AND c.Year = :Year";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Number).Value = personId;
                cmd.Parameters.Add("Year", OracleType.Number).Value = year;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    CadetSchoolSubject applicantDocument = ExtractCadetSchoolSubjectFromDataReader(dr, currentUser);
                    cadetSchoolSubjects.Add(applicantDocument);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }
            return cadetSchoolSubjects;
        }

        //Save a particular object into the DB
        public static bool SaveCadetSchoolSubject(CadetSchoolSubject cadetSchoolSubject, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            string logDescription = "";
            logDescription += "Име: " + cadetSchoolSubject.Cadet.Person.FullName;
            logDescription += "<br />ЕГН: " + cadetSchoolSubject.Cadet.Person.IdentNumber;
            logDescription += "<br />Военно окръжие: " + cadetSchoolSubject.Cadet.MilitaryDepartment.MilitaryDepartmentName;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";
                if (cadetSchoolSubject.CadetSchoolSubjectId == 0)
                {
                    SQL += @"INSERT INTO PMIS_APPL.CadetSchoolSubjects (CadetID, MilitSchoolSpecID, IsRanked)
                            VALUES (:CadetID, :MilitSchoolSpecID, :IsRanked);

                            SELECT PMIS_APPL.CadetSchoolSubjects_ID_SEQ.currval INTO :CadetSchoolSubjectID FROM dual;

                            ";

                    changeEvent = new ChangeEvent("APPL_Cadets_AddSpecialization", logDescription, null, cadetSchoolSubject.Cadet.Person, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("APPL_Cadets_Specialization", "", cadetSchoolSubject.MilitarySchoolSpecialization.Specialization.SpecializationName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("APPL_Cadets_Specialization_IsRanked", "", (cadetSchoolSubject.IsRanked ? "1" : "0"), currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_APPL.CadetSchoolSubjects SET
                               CadetID = :CadetID, 
                               MilitSchoolSpecID = :MilitSchoolSpecID,                                
                               IsRanked = :IsRanked

                            WHERE CadetSchoolSubjectID = :CadetSchoolSubjectID ;                       

                            ";

                    CadetSchoolSubject oldCadetSchoolSubject = CadetSchoolSubjectUtil.GetCadetSchoolSubject(cadetSchoolSubject.CadetSchoolSubjectId, currentUser);

                    logDescription += "<br />Специализация: " + cadetSchoolSubject.MilitarySchoolSpecialization.Specialization.SpecializationName;

                    if (cadetSchoolSubject.IsRanked)
                    {
                        changeEvent = new ChangeEvent("APPL_Cadets_AddRanking", logDescription, null, cadetSchoolSubject.Cadet.Person, currentUser);    
                    }
                    else
                    {
                        changeEvent = new ChangeEvent("APPL_Cadets_DeleteRanking", logDescription, null, cadetSchoolSubject.Cadet.Person, currentUser);
                    }
                    
                    if (oldCadetSchoolSubject.IsRanked != cadetSchoolSubject.IsRanked)
                        changeEvent.AddDetail(new ChangeEventDetail("APPL_Cadets_Specialization_IsRanked", (oldCadetSchoolSubject.IsRanked ? "1" : "0"), (cadetSchoolSubject.IsRanked ? "1" : "0"), currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramCadetSchoolSubjectID = new OracleParameter();
                paramCadetSchoolSubjectID.ParameterName = "CadetSchoolSubjectID";
                paramCadetSchoolSubjectID.OracleType = OracleType.Number;

                if (cadetSchoolSubject.CadetSchoolSubjectId != 0)
                {
                    paramCadetSchoolSubjectID.Direction = ParameterDirection.Input;
                    paramCadetSchoolSubjectID.Value = cadetSchoolSubject.CadetSchoolSubjectId;
                }
                else
                {
                    paramCadetSchoolSubjectID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramCadetSchoolSubjectID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "CadetID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = cadetSchoolSubject.CadetId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitSchoolSpecID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = cadetSchoolSubject.MilitSchoolSpecId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "IsRanked";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = cadetSchoolSubject.IsRanked;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (cadetSchoolSubject.CadetSchoolSubjectId == 0)
                    cadetSchoolSubject.CadetSchoolSubjectId = DBCommon.GetInt(paramCadetSchoolSubjectID.Value);

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
                    CadetUtil.SetCadetModified(cadetSchoolSubject.CadetId, currentUser);
                }
            }

            return result;
        }

        public static bool DeleteCadetSchoolSubject(int cadetSchoolSubjectId, User currentUser, Change changeEntry)
        {
            bool result = false;

            CadetSchoolSubject oldCadetSchoolSubject = GetCadetSchoolSubject(cadetSchoolSubjectId, currentUser);

            string logDescription = "";
            logDescription += "Име: " + oldCadetSchoolSubject.Cadet.Person.FullName;
            logDescription += "<br />ЕГН: " + oldCadetSchoolSubject.Cadet.Person.IdentNumber;
            logDescription += "<br />Военно окръжие: " + oldCadetSchoolSubject.Cadet.MilitaryDepartment.MilitaryDepartmentName;

            ChangeEvent changeEvent = new ChangeEvent("APPL_Cadets_DeleteSpecialization", logDescription, null, oldCadetSchoolSubject.Cadet.Person, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("APPL_Cadets_Specialization", oldCadetSchoolSubject.MilitarySchoolSpecialization.Specialization.SpecializationName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("APPL_Cadets_Specialization_IsRanked", (oldCadetSchoolSubject.IsRanked ? "1" : "0"), "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"DELETE FROM PMIS_APPL.CadetSchoolSubjects WHERE CadetSchoolSubjectID = :CadetSchoolSubjectID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("CadetSchoolSubjectID", OracleType.Number).Value = cadetSchoolSubjectId;

                result = cmd.ExecuteNonQuery() == 1;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                changeEntry.AddEvent(changeEvent);
                CadetUtil.SetCadetModified(oldCadetSchoolSubject.CadetId, currentUser);
            }

            return result;
        }

        public static CadetSchoolSubject GetCadetSchoolSubjectByFilter(CadetSchoolSubjectFilter cadetSchoolSubjectFilter, User currentUser)
        {
            CadetSchoolSubject cadetSchoolSubject = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.CadetSchoolSubjectID, a.CadetID, a.MilitSchoolSpecID, a.IsRanked
                    FROM PMIS_APPL.CadetSchoolSubjects a
                    INNER JOIN PMIS_APPL.Cadets b ON a.CadetID = b.CadetID
                    INNER JOIN VS_OWNER.VS_LS c ON b.PersonID = c.PersonID 
                    LEFT OUTER JOIN PMIS_ADM.Persons d ON c.PersonID = d.PersonID
                    INNER JOIN PMIS_APPL.MilitarySchoolSpecializations e ON a.MilitSchoolSpecID = e.MilitSchoolSpecID
                    INNER JOIN VS_OWNER.KLV_VVU f ON e.MilitarySchoolID = f.VVUID
                    INNER JOIN PMIS_APPL.Specializations j ON e.SpecializationID = j.SpecializationID
                    WHERE f.VVUID = " + cadetSchoolSubjectFilter.MilitarySchoolId + " AND e.Year = " + cadetSchoolSubjectFilter.SchoolYear + @"
                    AND j.SpecializationID = " + cadetSchoolSubjectFilter.SpecializationId + " AND c.EGN = '" + cadetSchoolSubjectFilter.IdentityNumber + "'";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    cadetSchoolSubject = ExtractCadetSchoolSubjectFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return cadetSchoolSubject;
        }

        //Get a count of Cadet School Subjects for current Military School Specialization
        public static int CountAllByMilitarySchoolSpecialization(int militSchoolSpecId, User currentUser)
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
                               FROM PMIS_APPL.CadetSchoolSubjects a
                               WHERE MilitSchoolSpecId=:MilitSchoolSpecId";

                //Fill parameter
                param = new OracleParameter();
                param.ParameterName = "MilitSchoolSpecId";
                param.Direction = ParameterDirection.Input;
                param.OracleType = OracleType.Int32;
                param.Value = militSchoolSpecId;
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
    }
}
