using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;

namespace PMIS.Common
{
    public class PersonMilitaryEducationAcademy : BaseDbObject
    {


        public int MilitaryEducationAcademyId { get; set; }


        public bool GstCode { get; set; }

        public string MilitaryAcademyCode { get; set; }
        private MilitaryAcademy militaryAcademy;
        public MilitaryAcademy MilitaryAcademy
        {
            get
            {
                if (militaryAcademy == null)
                {
                    militaryAcademy = MilitaryAcademyUtil.GetMilitaryAcademy(MilitaryAcademyCode, CurrentUser);
                }
                return militaryAcademy;
            }
        }

        public string MilitaryAcademySubjectCode { get; set; }
        private MilitarySubject militaryAcademySubject;
        public MilitarySubject MilitaryAcademySubject
        {
            get
            {
                if (militaryAcademySubject == null)
                {
                    militaryAcademySubject = MilitarySubjectUtil.GetMilitarySubject(MilitaryAcademySubjectCode, CurrentUser);
                }
                return militaryAcademySubject;
            }
        }

        public int DurationYear { get; set; }
        public int GraduateYear { get; set; }

        public string CountryCode { get; set; }
        private Country country;
        public Country Country
        {
            get
            {
                if (country == null)
                {
                    country = CountryUtil.GetCountry(CountryCode, CurrentUser);
                }
                return country;
            }
        }

        private bool canDelete;
        public bool CanDelete
        {
            get
            {
                return true;
            }
        }

        public LearningMethod LearningMethod { get; set; }

        private string mVitoshaMilitaryReportSpecialityCode;
        public string VitoshaMilitaryReportSpecialityCode
        {
            get { return mVitoshaMilitaryReportSpecialityCode; }
            set { mVitoshaMilitaryReportSpecialityCode = value; }
        }
        private VitoshaMilitaryReportSpeciality mVitoshaMilitaryReportSpeciality;
        public VitoshaMilitaryReportSpeciality VitoshaMilitaryReportSpeciality
        {
            get
            {
                if (mVitoshaMilitaryReportSpeciality == null)
                    mVitoshaMilitaryReportSpeciality = VitoshaMilitaryReportSpecialityUtil.GetVitoshaMilitaryReportSpeciality(mVitoshaMilitaryReportSpecialityCode, CurrentUser);

                return mVitoshaMilitaryReportSpeciality;
            }
            set { mVitoshaMilitaryReportSpeciality = value; }
        }

        public PersonMilitaryEducationAcademy(User user)
            : base(user)
        {
        }
    }

    public class PersonMilitaryEducationAcademyUtil
    {
        private static PersonMilitaryEducationAcademy ExtractPersonMilitaryEducationAcademyFromDR(OracleDataReader dr, User currentUser)
        {
            PersonMilitaryEducationAcademy personMilitaryEducationAcademy = new PersonMilitaryEducationAcademy(currentUser);

            personMilitaryEducationAcademy.MilitaryEducationAcademyId = DBCommon.GetInt(dr["MilitaryEducationAcademyId"]);

            personMilitaryEducationAcademy.GstCode = (dr["GstCode"].ToString() == "N" ? false : true);
            personMilitaryEducationAcademy.MilitaryAcademyCode = dr["MilitaryAcademyCode"].ToString();
            personMilitaryEducationAcademy.MilitaryAcademySubjectCode = dr["MilitaryAcademySubjectCode"].ToString();
            personMilitaryEducationAcademy.DurationYear = DBCommon.GetInt(dr["DurationYear"]);
            personMilitaryEducationAcademy.GraduateYear = DBCommon.GetInt(dr["GraduateYear"]);
            personMilitaryEducationAcademy.CountryCode = dr["CountryCode"].ToString();

            personMilitaryEducationAcademy.LearningMethod = new LearningMethod();
            personMilitaryEducationAcademy.LearningMethod.LearningMethodKey = dr["LearningMethodKey"].ToString();
            personMilitaryEducationAcademy.LearningMethod.LearningMethodName = dr["LearningMethodName"].ToString();

            personMilitaryEducationAcademy.VitoshaMilitaryReportSpecialityCode = dr["VitoshaMilRepSpecCode"].ToString();

            return personMilitaryEducationAcademy;
        }

        public static PersonMilitaryEducationAcademy GetPersonMilitaryEducation(int militaryEducationAcademyId, User currentUser)
        {
            PersonMilitaryEducationAcademy personMilitaryEducationAcademy = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT
                              a.OBRVAID AS MilitaryEducationAcademyId,
                              a.OBRVA_VVAKOD AS MilitaryAcademyCode,
                              a.OBRVA_GSTKOD AS GstCode,
                              a.OBRVA_SVAKOD AS MilitaryAcademySubjectCode,
                              a.OBRVA_PROD AS DurationYear,
                              a.OBRVA_KOGA AS GraduateYear,
                              a.OBRVA_DJJKOD AS CountryCode,
                              b.LearningMethodKey as LearningMethodKey, 
                              b.LearningMethodName as LearningMethodName,
                              a.OBRVA_VOS as VitoshaMilRepSpecCode
                              FROM VS_OWNER.VS_OBRVA a
                              INNER JOIN VS_OWNER.VS_LS c ON a.OBRVA_EGNLS = c.EGN
                              INNER JOIN PMIS_ADM.LearningMethods b ON a.OBRVA_VZOKOD = b.LearningMethodKey
                              WHERE a.OBRVAID = :MilitaryEducationAcademyId";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitaryEducationAcademyId", OracleType.Number).Value = militaryEducationAcademyId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    personMilitaryEducationAcademy = ExtractPersonMilitaryEducationAcademyFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personMilitaryEducationAcademy;
        }

        //Use this Method accordin PK define in table to chek exist combination of these 3 parameters
        public static PersonMilitaryEducationAcademy GetPersonMilitaryEducation(string identityNumber, string militaryAcademyCode, int graduateYear, User currentUser)
        {
            PersonMilitaryEducationAcademy personMilitaryEducationAcademy = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT
                              a.OBRVAID AS MilitaryEducationAcademyId,
                              a.OBRVA_VVAKOD AS MilitaryAcademyCode,
                              a.OBRVA_GSTKOD AS GstCode,
                              a.OBRVA_SVAKOD AS MilitaryAcademySubjectCode,
                              a.OBRVA_PROD AS DurationYear,
                              a.OBRVA_KOGA AS GraduateYear,
                              a.OBRVA_DJJKOD AS CountryCode,
                              b.LearningMethodKey as LearningMethodKey, 
                              b.LearningMethodName as LearningMethodName,
                              a.OBRVA_VOS as VitoshaMilRepSpecCode
                              FROM VS_OWNER.VS_OBRVA a
                              INNER JOIN VS_OWNER.VS_LS c ON a.OBRVA_EGNLS = c.EGN
                              INNER JOIN PMIS_ADM.LearningMethods b ON a.OBRVA_VZOKOD = b.LearningMethodKey
                                WHERE c.EGN = :IdentityNumber
                                AND a.OBRVA_VVAKOD = :MilitaryAcademyCode
                                AND a.OBRVA_KOGA = :GraduateYear";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("IdentityNumber", OracleType.VarChar).Value = identityNumber;
                cmd.Parameters.Add("MilitaryAcademyCode", OracleType.VarChar).Value = militaryAcademyCode;
                cmd.Parameters.Add("GraduateYear", OracleType.Number).Value = graduateYear;
                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    personMilitaryEducationAcademy = ExtractPersonMilitaryEducationAcademyFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personMilitaryEducationAcademy;
        }

        public static List<PersonMilitaryEducationAcademy> GetAllPersonMilitaryEducationAcademysByPersonID(int personId, User currentUser)
        {
            List<PersonMilitaryEducationAcademy> listPersonMilitaryEducationAcademy = new List<PersonMilitaryEducationAcademy>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT
                              a.OBRVAID AS MilitaryEducationAcademyId,
                              a.OBRVA_VVAKOD AS MilitaryAcademyCode,
                              a.OBRVA_GSTKOD AS GstCode,
                              a.OBRVA_SVAKOD AS MilitaryAcademySubjectCode,
                              a.OBRVA_PROD AS DurationYear,
                              a.OBRVA_KOGA AS GraduateYear,
                              a.OBRVA_DJJKOD AS CountryCode,
                              b.LearningMethodKey as LearningMethodKey, 
                              b.LearningMethodName as LearningMethodName,
                              a.OBRVA_VOS as VitoshaMilRepSpecCode
                              FROM VS_OWNER.VS_OBRVA a
                              INNER JOIN VS_OWNER.VS_LS c ON a.OBRVA_EGNLS = c.EGN
                              INNER JOIN PMIS_ADM.LearningMethods b ON a.OBRVA_VZOKOD = b.LearningMethodKey
                              WHERE c.PersonID = :PersonID
                              ORDER BY a.OBRVA_VVAKOD, a.OBRVA_KOGA, a.OBRVA_SVAKOD, a.OBRVA_GSTKOD";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Number).Value = personId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    listPersonMilitaryEducationAcademy.Add(ExtractPersonMilitaryEducationAcademyFromDR(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listPersonMilitaryEducationAcademy;
        }

        public static bool SavePersonMilitaryEducationAcademy(PersonMilitaryEducationAcademy personMilitaryEducationAcademy, Person person, User currentUser, Change changeEntry)
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

                if (personMilitaryEducationAcademy.MilitaryEducationAcademyId == 0)
                {
                    SQL += @"INSERT INTO VS_OWNER.VS_OBRVA (OBRVA_EGNLS, OBRVA_VVAKOD, OBRVA_GSTKOD, OBRVA_SVAKOD, OBRVA_PROD, OBRVA_KOGA, OBRVA_DJJKOD, OBRVA_VZOKOD, OBRVA_VOS)
                            VALUES (:IdentNumber, :MilitaryAcademyCode, :GstCode, :MilitaryAcademySubjectCode, :DurationYear, :GraduateYear, :CountryCode, :LearningMethodKey, :VitoshaMilRepSpecCode);

                            SELECT VS_OWNER.VS_OBRVA_OBRVAID_SEQ.currval INTO :MilitaryEducationAcademyId FROM dual;
                            
                            ";

                    changeEvent = new ChangeEvent("RES_Reservist_EduWork_AddMltEdu_VA", "", null, person, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VA_MilitaryAcademy", "", personMilitaryEducationAcademy.MilitaryAcademy.MilitaryAcademyName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VA_GstCode", "", personMilitaryEducationAcademy.GstCode ? "1" : "0", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VA_MilitarySubject", "", personMilitaryEducationAcademy.MilitaryAcademySubject != null ? personMilitaryEducationAcademy.MilitaryAcademySubject.MilitarySubjectName : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VA_DurationYear", "", personMilitaryEducationAcademy.DurationYear.ToString(), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VA_GraduateYear", "", personMilitaryEducationAcademy.GraduateYear.ToString(), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VA_Country", "", personMilitaryEducationAcademy.Country.CountryName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VA_LearningMethod", "", personMilitaryEducationAcademy.LearningMethod.LearningMethodName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VA_VitoshaMilitaryReportSpeciality", "", (personMilitaryEducationAcademy.VitoshaMilitaryReportSpeciality != null ? personMilitaryEducationAcademy.VitoshaMilitaryReportSpeciality.CodeAndName : ""), currentUser));
                }
                else
                {
                    SQL += @"UPDATE VS_OWNER.VS_OBRVA SET

                               OBRVA_VVAKOD = :MilitaryAcademyCode,
                               OBRVA_GSTKOD = :GstCode,
                               OBRVA_SVAKOD = :MilitaryAcademySubjectCode,
                               OBRVA_PROD   = :DurationYear,
                               OBRVA_KOGA   = :GraduateYear,
                               OBRVA_DJJKOD = :CountryCode,
                               OBRVA_VZOKOD = :LearningMethodKey,
                               OBRVA_VOS    = :VitoshaMilRepSpecCode

                             WHERE OBRVAID = :MilitaryEducationAcademyId ; ";



                    PersonMilitaryEducationAcademy oldPersonMilitaryEducationAcademy = GetPersonMilitaryEducation(personMilitaryEducationAcademy.MilitaryEducationAcademyId, currentUser);

                    string logDescription = "Военна академия: " + oldPersonMilitaryEducationAcademy.MilitaryAcademy.MilitaryAcademyName + "; " +
                                            "Година на завършване: " + oldPersonMilitaryEducationAcademy.GraduateYear.ToString();

                    changeEvent = new ChangeEvent("RES_Reservist_EduWork_EditMltEdu_VA", logDescription, null, person, currentUser);

                    if (oldPersonMilitaryEducationAcademy.MilitaryAcademyCode.Trim() != personMilitaryEducationAcademy.MilitaryAcademyCode.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VA_MilitaryAcademy", oldPersonMilitaryEducationAcademy.MilitaryAcademy.MilitaryAcademyName, personMilitaryEducationAcademy.MilitaryAcademy.MilitaryAcademyName, currentUser));

                    if (oldPersonMilitaryEducationAcademy.GstCode != personMilitaryEducationAcademy.GstCode)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VA_GstCode", oldPersonMilitaryEducationAcademy.GstCode ? "1" : "0", personMilitaryEducationAcademy.GstCode ? "1" : "0", currentUser));

                    if (oldPersonMilitaryEducationAcademy.MilitaryAcademySubjectCode != personMilitaryEducationAcademy.MilitaryAcademySubjectCode)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VA_MilitarySubject", oldPersonMilitaryEducationAcademy.MilitaryAcademySubject != null ? oldPersonMilitaryEducationAcademy.MilitaryAcademySubject.MilitarySubjectName : "", personMilitaryEducationAcademy.MilitaryAcademySubject != null ? personMilitaryEducationAcademy.MilitaryAcademySubject.MilitarySubjectName : "", currentUser));

                    if (oldPersonMilitaryEducationAcademy.DurationYear != personMilitaryEducationAcademy.DurationYear)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VA_DurationYear", oldPersonMilitaryEducationAcademy.DurationYear.ToString(), personMilitaryEducationAcademy.DurationYear.ToString(), currentUser));

                    if (oldPersonMilitaryEducationAcademy.GraduateYear != personMilitaryEducationAcademy.GraduateYear)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VA_GraduateYear", oldPersonMilitaryEducationAcademy.GraduateYear.ToString(), personMilitaryEducationAcademy.GraduateYear.ToString(), currentUser));

                    if (oldPersonMilitaryEducationAcademy.CountryCode.Trim() != personMilitaryEducationAcademy.CountryCode.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VA_Country", oldPersonMilitaryEducationAcademy.Country.CountryName, personMilitaryEducationAcademy.Country.CountryName, currentUser));

                    if (oldPersonMilitaryEducationAcademy.LearningMethod.LearningMethodKey.Trim() != personMilitaryEducationAcademy.LearningMethod.LearningMethodKey.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VA_LearningMethod", oldPersonMilitaryEducationAcademy.LearningMethod.LearningMethodName, personMilitaryEducationAcademy.LearningMethod.LearningMethodName, currentUser));

                    if (oldPersonMilitaryEducationAcademy.VitoshaMilitaryReportSpecialityCode != personMilitaryEducationAcademy.VitoshaMilitaryReportSpecialityCode)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VA_VitoshaMilitaryReportSpeciality", (oldPersonMilitaryEducationAcademy.VitoshaMilitaryReportSpeciality != null ? oldPersonMilitaryEducationAcademy.VitoshaMilitaryReportSpeciality.CodeAndName : ""), (personMilitaryEducationAcademy.VitoshaMilitaryReportSpeciality != null ? personMilitaryEducationAcademy.VitoshaMilitaryReportSpeciality.CodeAndName : ""), currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramMilitaryEducationAcademyId = new OracleParameter();
                paramMilitaryEducationAcademyId.ParameterName = "MilitaryEducationAcademyId";
                paramMilitaryEducationAcademyId.OracleType = OracleType.Number;

                if (personMilitaryEducationAcademy.MilitaryEducationAcademyId != 0)
                {
                    paramMilitaryEducationAcademyId.Direction = ParameterDirection.Input;
                    paramMilitaryEducationAcademyId.Value = personMilitaryEducationAcademy.MilitaryEducationAcademyId;
                }
                else
                {
                    paramMilitaryEducationAcademyId.Direction = ParameterDirection.Output;
                }
                cmd.Parameters.Add(paramMilitaryEducationAcademyId);


                OracleParameter param = null;

                if (personMilitaryEducationAcademy.MilitaryEducationAcademyId == 0)
                {
                    //Insert
                    param = new OracleParameter();
                    param.ParameterName = "IdentNumber";
                    param.OracleType = OracleType.VarChar;
                    param.Direction = ParameterDirection.Input;
                    param.Value = person.IdentNumber;
                    cmd.Parameters.Add(param);
                }
                else
                {
                    //Update
                    param = new OracleParameter();
                    param.ParameterName = "MilitaryEducationAcademyId";
                    param.OracleType = OracleType.Number;
                    param.Direction = ParameterDirection.Input;
                    param.Value = personMilitaryEducationAcademy.MilitaryEducationAcademyId;
                    cmd.Parameters.Add(param);
                }

                param = new OracleParameter();
                param.ParameterName = "MilitaryAcademyCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personMilitaryEducationAcademy.MilitaryAcademyCode;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "GstCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personMilitaryEducationAcademy.GstCode ? "Y" : "N";
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryAcademySubjectCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personMilitaryEducationAcademy.MilitaryAcademySubjectCode;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "DurationYear";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = personMilitaryEducationAcademy.DurationYear;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "GraduateYear";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = personMilitaryEducationAcademy.GraduateYear;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "CountryCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personMilitaryEducationAcademy.CountryCode;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "LearningMethodKey";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personMilitaryEducationAcademy.LearningMethod.LearningMethodKey;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "VitoshaMilRepSpecCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personMilitaryEducationAcademy.VitoshaMilitaryReportSpecialityCode;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (personMilitaryEducationAcademy.MilitaryEducationAcademyId == 0)
                    personMilitaryEducationAcademy.MilitaryEducationAcademyId = DBCommon.GetInt(paramMilitaryEducationAcademyId.Value);

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


        public static bool DeletePersonMilitaryEducationAcademy(int militaryEducationAcademyId, Person person, User currentUser, Change changeEntry)
        {
            bool result = false;

            PersonMilitaryEducationAcademy oldPersonMilitaryEducationAcademy = GetPersonMilitaryEducation(militaryEducationAcademyId, currentUser);

            ChangeEvent changeEvent = new ChangeEvent("RES_Reservist_EduWork_DeleteMltEdu_VA", "", null, person, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VA_MilitaryAcademy", oldPersonMilitaryEducationAcademy.MilitaryAcademy.MilitaryAcademyName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VA_GstCode", oldPersonMilitaryEducationAcademy.GstCode ? "1" : "0", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VA_MilitarySubject", oldPersonMilitaryEducationAcademy.MilitaryAcademySubject != null ? oldPersonMilitaryEducationAcademy.MilitaryAcademySubject.MilitarySubjectName : "", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VA_DurationYear", oldPersonMilitaryEducationAcademy.DurationYear.ToString(), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VA_GraduateYear", oldPersonMilitaryEducationAcademy.GraduateYear.ToString(), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VA_Country", oldPersonMilitaryEducationAcademy.Country.CountryName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VA_LearningMethod", oldPersonMilitaryEducationAcademy.LearningMethod.LearningMethodName, "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" BEGIN
                                  DELETE FROM VS_OWNER.VS_OBRVA WHERE OBRVAID = :MilitaryEducationAcademyId;
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitaryEducationAcademyId", OracleType.Number).Value = militaryEducationAcademyId;

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


    }
}
