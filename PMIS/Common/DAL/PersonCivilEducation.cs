using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Text;

namespace PMIS.Common
{
    //Represents single person education
    public class PersonCivilEducation : BaseDbObject
    {
        private int civilEducationId;
        private string personEducationCode;
        private string personSchoolSubjectCode;
        private int graduateYear;
        private LearningMethod learningMethod;

        private PersonEducation personEducation;
        private PersonSchoolSubject personSchoolSubject;

        public int CivilEducationId
        {
            get
            {
                return civilEducationId;
            }
            set
            {
                civilEducationId = value;
            }
        }

        public string PersonEducationCode
        {
            get
            {
                return personEducationCode;
            }
            set
            {
                personEducationCode = value;
            }
        }

        public string PersonSchoolSubjectCode
        {
            get
            {
                return personSchoolSubjectCode;
            }
            set
            {
                personSchoolSubjectCode = value;
            }
        }

        public int GraduateYear
        {
            get
            {
                return graduateYear;
            }
            set
            {
                graduateYear = value;
            }
        }

        public LearningMethod LearningMethod
        {
            get
            {
                return learningMethod;
            }
            set
            {
                learningMethod = value;
            }
        }

        public PersonEducation PersonEducation
        {
            get
            {
                if (personEducation == null)
                    personEducation = PersonEducationUtil.GetPersonEducation(personEducationCode, CurrentUser);

                return personEducation;
            }
            set { personEducation = value; }
        }

        public PersonSchoolSubject PersonSchoolSubject
        {
            get
            {
                if (personSchoolSubject == null)
                    personSchoolSubject = PersonSchoolSubjectUtil.GetPersonSchoolSubject(personSchoolSubjectCode, CurrentUser);
                return personSchoolSubject;
            }
            set { personSchoolSubject = value; }
        }

        public bool CanDelete
        {
            get
            {
                return true;
            }
        }

        public PersonCivilEducation(User user)
            : base(user)
        {

        }
    }

    public class PersonCivilEducationUtil
    {
        private static PersonCivilEducation ExtractPersonCivilEducationFromDR(OracleDataReader dr, User currentUser)
        {
            PersonCivilEducation personCivilEducation = new PersonCivilEducation(currentUser);
            
            personCivilEducation.CivilEducationId = DBCommon.GetInt(dr["CivilEducationID"]);
            personCivilEducation.PersonEducationCode = dr["EducationCode"].ToString();
            personCivilEducation.PersonSchoolSubjectCode = dr["EducationSubjectCode"].ToString();
            personCivilEducation.GraduateYear = DBCommon.GetInt(dr["GraduateYear"]);
            personCivilEducation.LearningMethod = new LearningMethod();
            personCivilEducation.LearningMethod.LearningMethodKey = dr["LearningMethodKey"].ToString();
            personCivilEducation.LearningMethod.LearningMethodName = dr["LearningMethodName"].ToString();

            return personCivilEducation;
        }

        public static PersonCivilEducation GetPersonCivilEducation(string identNumber, string personEducationCode, int graduateYear, User currentUser)
        {
            PersonCivilEducation personCivilEducation = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.OBRGID as CivilEducationID, a.OBRG_KOD as EducationCode,
                                      a.OBRG_SPEKOD as EducationSubjectCode, a.OBRG_KOGA as GraduateYear,
                                      b.LearningMethodKey, b.LearningMethodName
                               FROM VS_OWNER.VS_OBRG a
                               INNER JOIN PMIS_ADM.LearningMethods b ON a.OBRG_VZOKOD = b.LearningMethodKey
                               WHERE a.OBRG_EGNLS = :IdentNumber AND
                                     a.OBRG_KOD = :PersonEducationCode AND
                                     a.OBRG_KOGA = :GraduateYear ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("IdentNumber", OracleType.VarChar).Value = identNumber;
                cmd.Parameters.Add("PersonEducationCode", OracleType.VarChar).Value = personEducationCode;
                cmd.Parameters.Add("GraduateYear", OracleType.Number).Value = graduateYear;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    personCivilEducation = ExtractPersonCivilEducationFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personCivilEducation;
        }

        public static PersonCivilEducation GetPersonCivilEducation(int civilEducationId, User currentUser)
        {
            PersonCivilEducation personCivilEducation = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.OBRGID as CivilEducationID, a.OBRG_KOD as EducationCode,
                                      a.OBRG_SPEKOD as EducationSubjectCode, a.OBRG_KOGA as GraduateYear,
                                      b.LearningMethodKey, b.LearningMethodName
                               FROM VS_OWNER.VS_OBRG a
                               INNER JOIN PMIS_ADM.LearningMethods b ON a.OBRG_VZOKOD = b.LearningMethodKey
                               WHERE a.OBRGID = :CivilEducationId";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("CivilEducationId", OracleType.Number).Value = civilEducationId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    personCivilEducation = ExtractPersonCivilEducationFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personCivilEducation;
        }

        public static List<PersonCivilEducation> GetAllPersonCivilEducationsByPersonID(int personId, User currentUser)
        {
            List<PersonCivilEducation> personCivilEducations = new List<PersonCivilEducation>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.OBRGID as CivilEducationID, a.OBRG_KOD as EducationCode,
                                      a.OBRG_SPEKOD as EducationSubjectCode, a.OBRG_KOGA as GraduateYear,
                                      b.LearningMethodKey, b.LearningMethodName
                               FROM VS_OWNER.VS_OBRG a
                               INNER JOIN PMIS_ADM.LearningMethods b ON a.OBRG_VZOKOD = b.LearningMethodKey
                               INNER JOIN VS_OWNER.VS_LS c ON a.OBRG_EGNLS = c.EGN
                               WHERE c.PersonID = :PersonID
                               ORDER BY a.OBRG_KOGA, a.OBRG_KOD";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Number).Value = personId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    personCivilEducations.Add(ExtractPersonCivilEducationFromDR(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personCivilEducations;
        }

        public static bool SavePersonCivilEducation(PersonCivilEducation personCivilEducation, Person person, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                if (personCivilEducation.CivilEducationId == 0)
                {
                    SQL += @"INSERT INTO VS_OWNER.VS_OBRG (OBRG_EGNLS, OBRG_KOD, OBRG_SPEKOD, OBRG_KOGA, OBRG_VZOKOD)
                            VALUES (:IdentNumber, :EducationCode, :EducationSubjectCode, :GraduateYear, :LearningMethodKey);

                            SELECT VS_OWNER.VS_OBRG_OBRGID_SEQ.currval INTO :CivilEducationID FROM dual;

                            ";

                    changeEvent = new ChangeEvent("RES_Reservist_AddCivilEducation", "", null, person, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_CivilEducation_Education", "", personCivilEducation.PersonEducation.PersonEducationName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_CivilEducation_EducationSubject", "", (personCivilEducation.PersonSchoolSubject != null ? personCivilEducation.PersonSchoolSubject.PersonSchoolSubjectName : ""), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_CivilEducation_GraduateYear", "", personCivilEducation.GraduateYear.ToString(), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_CivilEducation_LearningMethod", "", personCivilEducation.LearningMethod.LearningMethodName, currentUser));
                }
                else
                {
                    SQL += @"UPDATE VS_OWNER.VS_OBRG SET
                               OBRG_KOD = :EducationCode, 
                               OBRG_SPEKOD = :EducationSubjectCode, 
                               OBRG_KOGA = :GraduateYear, 
                               OBRG_VZOKOD = :LearningMethodKey
                            WHERE OBRGID = :CivilEducationID ;                       

                            ";

                    PersonCivilEducation oldPersonCivilEducation = GetPersonCivilEducation(personCivilEducation.CivilEducationId, currentUser);
                    
                    string logDescription = "Образователна степен: " + oldPersonCivilEducation.PersonEducation.PersonEducationName + "; " + 
                                            "Година на завършване: " + oldPersonCivilEducation.GraduateYear.ToString();

                    changeEvent = new ChangeEvent("RES_Reservist_EditCivilEducation", logDescription, null, person, currentUser);

                    if (oldPersonCivilEducation.PersonEducationCode.Trim() != personCivilEducation.PersonEducationCode.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_CivilEducation_Education", oldPersonCivilEducation.PersonEducation.PersonEducationName, personCivilEducation.PersonEducation.PersonEducationName, currentUser));

                    if (oldPersonCivilEducation.PersonSchoolSubjectCode.Trim() != personCivilEducation.PersonSchoolSubjectCode.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_CivilEducation_EducationSubject", (oldPersonCivilEducation.PersonSchoolSubject != null ? oldPersonCivilEducation.PersonSchoolSubject.PersonSchoolSubjectName : ""), (personCivilEducation.PersonSchoolSubject != null ? personCivilEducation.PersonSchoolSubject.PersonSchoolSubjectName : ""), currentUser));

                    if (oldPersonCivilEducation.GraduateYear != personCivilEducation.GraduateYear)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_CivilEducation_GraduateYear", oldPersonCivilEducation.GraduateYear.ToString(), personCivilEducation.GraduateYear.ToString(), currentUser));

                    if (oldPersonCivilEducation.LearningMethod.LearningMethodKey != personCivilEducation.LearningMethod.LearningMethodKey)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_CivilEducation_LearningMethod", oldPersonCivilEducation.LearningMethod.LearningMethodName, personCivilEducation.LearningMethod.LearningMethodName, currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramCivilEducationID = new OracleParameter();
                paramCivilEducationID.ParameterName = "CivilEducationID";
                paramCivilEducationID.OracleType = OracleType.Number;

                if (personCivilEducation.CivilEducationId != 0)
                {
                    paramCivilEducationID.Direction = ParameterDirection.Input;
                    paramCivilEducationID.Value = personCivilEducation.CivilEducationId;
                }
                else
                {
                    paramCivilEducationID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramCivilEducationID);

                OracleParameter param = null;

                if(personCivilEducation.CivilEducationId == 0)
                {
                    param = new OracleParameter();
                    param.ParameterName = "IdentNumber";
                    param.OracleType = OracleType.VarChar;
                    param.Direction = ParameterDirection.Input;
                    param.Value = person.IdentNumber;
                    cmd.Parameters.Add(param);
                }

                param = new OracleParameter();
                param.ParameterName = "EducationCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personCivilEducation.PersonEducationCode;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "EducationSubjectCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if(personCivilEducation.PersonSchoolSubject != null)
                    param.Value = personCivilEducation.PersonSchoolSubjectCode;
                else 
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "GraduateYear";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = personCivilEducation.GraduateYear;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "LearningMethodKey";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personCivilEducation.LearningMethod.LearningMethodKey;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (personCivilEducation.CivilEducationId == 0)
                    personCivilEducation.CivilEducationId = DBCommon.GetInt(paramCivilEducationID.Value);

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
                    PersonUtil.SetPersonModified(person.PersonId, currentUser);
                }
            }

            return result;
        }

        public static bool DeletePersonCivilEducation(int civilEducationId, Person person, User currentUser, Change changeEntry)
        {
            bool result = false;

            PersonCivilEducation oldPersonCivilEducation = GetPersonCivilEducation(civilEducationId, currentUser);

            ChangeEvent changeEvent = new ChangeEvent("RES_Reservist_DeleteCivilEducation", "", null, person, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("RES_CivilEducation_Education", oldPersonCivilEducation.PersonEducation.PersonEducationName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_CivilEducation_EducationSubject", (oldPersonCivilEducation.PersonSchoolSubject != null ? oldPersonCivilEducation.PersonSchoolSubject.PersonSchoolSubjectName : ""), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_CivilEducation_GraduateYear", oldPersonCivilEducation.GraduateYear.ToString(), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_CivilEducation_LearningMethod", oldPersonCivilEducation.LearningMethod.LearningMethodName, "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" BEGIN
                                  DELETE FROM VS_OWNER.VS_OBRG WHERE OBRGID = :CivilEducationID;
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("CivilEducationID", OracleType.Number).Value = civilEducationId;

                result = cmd.ExecuteNonQuery() == 1;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                changeEntry.AddEvent(changeEvent);
                PersonUtil.SetPersonModified(person.PersonId, currentUser);
            }

            return result;
        }

        public static string CivilEducationSelector_SearchCivilEducation(string searchType, string searchText, int maxRowNumbers, User currentUser)
        {
            StringBuilder sb = new StringBuilder();
            string SQL = "";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);

            conn.Open();

            sb.Append("<response>");

            try
            {
                SQL = @"SELECT * 
                        FROM (
                              SELECT a.SPE_KOD as SchoolSubjectCode, 
                                     a.SPE_IME as SchoolSubjectName,
                                     RANK() OVER (ORDER BY UPPER(a.SPE_IME), a.SPE_KOD) as RowNumber
                              FROM VS_OWNER.KLV_SPE a
                              WHERE UPPER(a.SPE_IME) LIKE UPPER(:searchText)
                              ORDER BY UPPER(a.SPE_IME)
                              ) tmp 
                        " + (maxRowNumbers > 0 ? " WHERE RowNumber <= " + maxRowNumbers.ToString() : "");

                OracleCommand cmd = new OracleCommand(SQL, conn);

                string searchTextLIKE = "";
                switch (searchType)
                {
                    case "starts_with":
                        searchTextLIKE = searchText.Trim() + "%";
                        break;
                    case "contains":
                        searchTextLIKE = "%" + searchText.Trim() + "%";
                        break;
                    case "ends_with":
                        searchTextLIKE = "%" + searchText.Trim();
                        break;
                }

                cmd.Parameters.Add("searchText", OracleType.VarChar).Value = searchTextLIKE;

                OracleDataReader dr = cmd.ExecuteReader();

                sb.Append("<result>");
                while (dr.Read())
                {
                    sb.Append("<personEducation>");
                    sb.Append("<schoolSubjectName>");
                    sb.Append(AJAXTools.EncodeForXML(!String.IsNullOrEmpty(dr["SchoolSubjectName"].ToString())? dr["SchoolSubjectName"].ToString(): ""));
                    sb.Append("</schoolSubjectName>");
                    sb.Append("<schoolSubjectCode>");
                    sb.Append(AJAXTools.EncodeForXML(!String.IsNullOrEmpty(dr["SchoolSubjectCode"].ToString())?dr["SchoolSubjectCode"].ToString():""));
                    sb.Append("</schoolSubjectCode>");
                    sb.Append("</personEducation>");
                }

                dr.Close();

                SQL = @"SELECT COUNT(*) as Cnt
                        FROM VS_OWNER.KLV_SPE a
                              WHERE UPPER(a.SPE_IME) LIKE UPPER(:searchText)
                        ";

                cmd = new OracleCommand(SQL, conn);
                cmd.Parameters.Add("searchText", OracleType.VarChar).Value = searchTextLIKE;

                dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    sb.Append("<totalRowsCount>");
                    sb.Append(AJAXTools.EncodeForXML(dr["Cnt"].ToString()));
                    sb.Append("</totalRowsCount>");
                }

                dr.Close();

                sb.Append("</result>");
            }
            finally
            {
                conn.Close();
            }

            sb.Append("</response>");

            return sb.ToString();
        }
    }
}
