using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;

namespace PMIS.Common
{

    public class PersonMilitaryEducation : BaseDbObject
    {

        private int militaryEducationId;
        public int MilitaryEducationId
        {
            get { return militaryEducationId; }
            set { militaryEducationId = value; }
        }


        private int militarySchoolId;
        public int MilitarySchoolId
        {
            get { return militarySchoolId; }
            set { militarySchoolId = value; }
        }
        private MilitarySchool militarySchool;
        public MilitarySchool MilitarySchool
        {
            get
            {
                if (militarySchool == null)
                {
                    militarySchool = MilitarySchoolUtil.GetMilitarySchool(MilitarySchoolId, CurrentUser);
                }
                return militarySchool;
            }
        }


        private string militaryEducationTypeCode;
        public string MilitaryEducationTypeCode
        {
            get { return militaryEducationTypeCode; }
            set { militaryEducationTypeCode = value; }
        }
        private MilitaryEducationType militaryEducationType;
        public MilitaryEducationType MilitaryEducationType
        {
            get
            {
                if (militaryEducationType == null)
                {
                    militaryEducationType = MilitaryEducationTypeUtil.GetMilitaryEducationType(MilitaryEducationTypeCode, CurrentUser);
                }
                return militaryEducationType;
            }
        }



        private int militarySchoolSubjectId;
        public int MilitarySchoolSubjectId
        {
            get { return militarySchoolSubjectId; }
            set { militarySchoolSubjectId = value; }
        }
        private MilitarySchoolSubject militarySchoolSubject;
        public MilitarySchoolSubject MilitarySchoolSubject
        {
            get
            {
                if (militarySchoolSubject == null)
                {
                    militarySchoolSubject = MilitarySchoolSubjectUtil.GetMilitarySchoolSubject(MilitarySchoolSubjectId, CurrentUser);
                }
                return militarySchoolSubject;
            }
            set { militarySchoolSubject = value; }
        }

        private string militaryArmsCode;
        public string MilitaryArmsCode
        {
            get { return militaryArmsCode; }
            set { militaryArmsCode = value; }
        }
        private MilitaryArms militaryArms;
        public MilitaryArms MilitaryArms
        {
            get
            {
                if (militaryArms == null)
                {
                    militaryArms = MilitaryArmsUtil.GetMilitaryArms(MilitaryArmsCode, CurrentUser);
                }
                return militaryArms;
            }
        }


        private string countryCode;
        public string CountryCode
        {
            get { return countryCode; }
            set { countryCode = value; }
        }
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


        private int graduateYear;
        public int GraduateYear
        {
            get { return graduateYear; }
            set { graduateYear = value; }
        }



        private LearningMethod learningMethod;
        public LearningMethod LearningMethod
        {
            get { return learningMethod; }
            set { learningMethod = value; }
        }

        public bool CanDelete
        {
            get
            {
                return true;
            }
        }

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

        public PersonMilitaryEducation(User user)
            : base(user)
        {

        }
    }

    public class PersonMilitaryEducationUtil
    {
        private static PersonMilitaryEducation ExtractPersonMilitaryEducationFromDR(OracleDataReader dr, User currentUser)
        {
            PersonMilitaryEducation personMilitaryEducation = new PersonMilitaryEducation(currentUser);

            personMilitaryEducation.MilitaryEducationId = DBCommon.GetInt(dr["MilitaryEducationId"]);
            personMilitaryEducation.MilitarySchoolId = DBCommon.GetInt(dr["MilitarySchoolId"]);
            personMilitaryEducation.MilitaryEducationTypeCode = dr["MilitaryEducationTypeCode"].ToString();
            personMilitaryEducation.MilitarySchoolSubjectId = DBCommon.GetInt(dr["MilitarySchoolSubjectId"]);
            personMilitaryEducation.MilitaryArmsCode = dr["MilitaryArmsCode"].ToString();
            personMilitaryEducation.GraduateYear = DBCommon.GetInt(dr["GraduateYear"]);

            personMilitaryEducation.CountryCode = dr["CountryCode"].ToString();

            personMilitaryEducation.LearningMethod = new LearningMethod();
            personMilitaryEducation.LearningMethod.LearningMethodKey = dr["LearningMethodKey"].ToString();
            personMilitaryEducation.LearningMethod.LearningMethodName = dr["LearningMethodName"].ToString();

            personMilitaryEducation.VitoshaMilitaryReportSpecialityCode = dr["VitoshaMilRepSpecCode"].ToString();


            return personMilitaryEducation;
        }

        public static PersonMilitaryEducation GetPersonMilitaryEducation(int militaryEducationId, User currentUser)
        {
            PersonMilitaryEducation personMilitaryEducation = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT
                               a.OBRVID as MilitaryEducationId,
                               m.vvuid as MilitarySchoolId,
                               a.OBRV_VVOKOD as MilitaryEducationTypeCode,
                               k.vspid as MilitarySchoolSubjectId,
                               a.OBRV_RVUKOD as MilitaryArmsCode,
                               a.OBRV_KOGA as GraduateYear,
                               a.OBRV_DJJKOD as CountryCode,
                               b.LearningMethodKey as LearningMethodKey, 
                               b.LearningMethodName as LearningMethodName,
                               a.OBRV_VOS as VitoshaMilRepSpecCode
                               FROM VS_OWNER.VS_OBRV a
                               INNER JOIN VS_OWNER.KLV_VVU m on m.vvu_kod=a.OBRV_VVUKOD
                               INNER JOIN VS_OWNER.KLV_VSP k on k.vsp_kod= a.obrv_vspkod
                               INNER JOIN PMIS_ADM.LearningMethods b ON a.OBRV_VZOKOD = b.LearningMethodKey
                               INNER JOIN VS_OWNER.VS_LS c ON a.OBRV_EGNLS = c.EGN
                              WHERE a.OBRVID = :MilitaryEducationId";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitaryEducationId", OracleType.Number).Value = militaryEducationId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    personMilitaryEducation = ExtractPersonMilitaryEducationFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personMilitaryEducation;
        }

        //Use this Method accordin PK define in table to chek exist combination of these 3 parameters
        public static PersonMilitaryEducation GetPersonMilitaryEducation(string identityNumber, string militarySchoolCode, int graduateYear, User currentUser)
        {
            //a.OBRV_EGNLS, OBRV_KOGA, OBRV_VVUKOD - Unique combination

            PersonMilitaryEducation personMilitaryEducation = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT
                               a.OBRVID as MilitaryEducationId,
                               m.vvuid as MilitarySchoolId,
                               a.OBRV_VVOKOD as MilitaryEducationTypeCode,
                               k.vspid as MilitarySchoolSubjectId,
                               a.OBRV_RVUKOD as MilitaryArmsCode,
                               a.OBRV_KOGA as GraduateYear,
                               a.OBRV_DJJKOD as CountryCode,
                               b.LearningMethodKey as LearningMethodKey, 
                               b.LearningMethodName as LearningMethodName,
                               a.OBRV_VOS as VitoshaMilRepSpecCode
                               FROM VS_OWNER.VS_OBRV a
                               INNER JOIN VS_OWNER.KLV_VVU m on m.vvu_kod=a.OBRV_VVUKOD
                               INNER JOIN VS_OWNER.KLV_VSP k on k.vsp_kod= a.obrv_vspkod
                               INNER JOIN PMIS_ADM.LearningMethods b ON a.OBRV_VZOKOD = b.LearningMethodKey
                               INNER JOIN VS_OWNER.VS_LS c ON a.OBRV_EGNLS = c.EGN
                                WHERE c.EGN = :IdentityNumber
                                AND a.OBRV_VVUKOD = :MilitarySchoolCode
                                AND a.OBRV_KOGA = :GraduateYear";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("IdentityNumber", OracleType.VarChar).Value = identityNumber;
                cmd.Parameters.Add("MilitarySchoolCode", OracleType.VarChar).Value = militarySchoolCode;
                cmd.Parameters.Add("GraduateYear", OracleType.Number).Value = graduateYear;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    personMilitaryEducation = ExtractPersonMilitaryEducationFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personMilitaryEducation;
        }

        public static List<PersonMilitaryEducation> GetAllPersonMilitaryEducationsByPersonID(int personId, User currentUser)
        {
            List<PersonMilitaryEducation> listPersonMilitaryEducation = new List<PersonMilitaryEducation>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT
                               a.OBRVID as MilitaryEducationId,
                               m.vvuid as MilitarySchoolId,
                               a.OBRV_VVOKOD as MilitaryEducationTypeCode,
                               k.vspid as MilitarySchoolSubjectId,
                               a.OBRV_RVUKOD as MilitaryArmsCode,
                               a.OBRV_KOGA as GraduateYear,
                               a.OBRV_DJJKOD as CountryCode,
                               b.LearningMethodKey as LearningMethodKey, 
                               b.LearningMethodName as LearningMethodName,
                               a.OBRV_VOS as VitoshaMilRepSpecCode
                               FROM VS_OWNER.VS_OBRV a
                               INNER JOIN VS_OWNER.KLV_VVU m on m.vvu_kod=a.OBRV_VVUKOD
                               INNER JOIN VS_OWNER.KLV_VSP k on k.vsp_kod= a.obrv_vspkod
                               INNER JOIN PMIS_ADM.LearningMethods b ON a.OBRV_VZOKOD = b.LearningMethodKey
                               INNER JOIN VS_OWNER.VS_LS c ON a.OBRV_EGNLS = c.EGN
                               WHERE c.PersonID = :PersonID
                               ORDER BY a.OBRV_VVUKOD, a.OBRV_KOGA, a.OBRV_VVOKOD, a.OBRV_VSPKOD, a.OBRV_RVUKOD";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Number).Value = personId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    listPersonMilitaryEducation.Add(ExtractPersonMilitaryEducationFromDR(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listPersonMilitaryEducation;
        }

        public static bool SavePersonMilitaryEducation(PersonMilitaryEducation personMilitaryEducation, Person person, User currentUser, Change changeEntry)
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

                if (personMilitaryEducation.MilitaryEducationId == 0)
                {
                    SQL += @"INSERT INTO VS_OWNER.VS_OBRV (OBRV_EGNLS, OBRV_VVUKOD, OBRV_VVOKOD, OBRV_VSPKOD, OBRV_RVUKOD, OBRV_KOGA, OBRV_DJJKOD, OBRV_VZOKOD, OBRV_VOS)
                            VALUES (:IdentNumber, :MilitarySchoolCode, :MilitaryEducationTypeCode, :MilitarySchoolSubjectCode, :MilitaryArmsCode, :GraduateYear, :CountryId, :LearningMethodKey, :VitoshaMilRepSpecCode);

                           SELECT VS_OWNER.VS_OBRV_OBRVID_SEQ.currval INTO :MilitaryEducationId FROM dual;
                            
                            ";

                    changeEvent = new ChangeEvent("RES_Reservist_EduWork_AddMltEdu_VU", "", null, person, currentUser);


                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VU_MilitarySchool", "", personMilitaryEducation.MilitarySchool.MilitarySchoolName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VU_MilitaryEducationType", "", personMilitaryEducation.MilitaryEducationType.MilitaryEducationTypeName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VU_MilitarySchoolSubject", "", personMilitaryEducation.MilitarySchoolSubject.MilitarySchoolSubjectName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VU_MilitaryArms", "", personMilitaryEducation.MilitaryArms != null ? personMilitaryEducation.MilitaryArms.MilitaryArmsName : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VU_GraduateYear", "", personMilitaryEducation.GraduateYear.ToString(), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VU_Country", "", personMilitaryEducation.Country.CountryName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VU_LearningMethod", "", personMilitaryEducation.LearningMethod.LearningMethodName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VU_VitoshaMilitaryReportSpeciality", "", (personMilitaryEducation.VitoshaMilitaryReportSpeciality != null ? personMilitaryEducation.VitoshaMilitaryReportSpeciality.CodeAndName : "") , currentUser));
                }
                else
                {
                    SQL += @"UPDATE VS_OWNER.VS_OBRV SET

                               OBRV_VVUKOD = :MilitarySchoolCode,
                               OBRV_VVOKOD = :MilitaryEducationTypeCode,
                               OBRV_VSPKOD = :MilitarySchoolSubjectCode,
                               OBRV_RVUKOD = :MilitaryArmsCode,
                               OBRV_KOGA   = :GraduateYear,
                               OBRV_DJJKOD = :CountryId,
                               OBRV_VZOKOD = :LearningMethodKey,
                               OBRV_VOS    = :VitoshaMilRepSpecCode

                              WHERE OBRVID = :MilitaryEducationId ; ";


                    PersonMilitaryEducation oldPersonMilitaryEducation = GetPersonMilitaryEducation(personMilitaryEducation.MilitaryEducationId, currentUser);

                    string logDescription = "Военно Училище: " + oldPersonMilitaryEducation.MilitarySchool.MilitarySchoolName + "; " +
                                            "Година на завършване: " + oldPersonMilitaryEducation.GraduateYear.ToString();


                    changeEvent = new ChangeEvent("RES_Reservist_EduWork_EditMltEdu_VU", logDescription, null, person, currentUser);

                    if (oldPersonMilitaryEducation.MilitarySchoolId != personMilitaryEducation.MilitarySchoolId)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VU_MilitarySchool", oldPersonMilitaryEducation.MilitarySchool.MilitarySchoolName, personMilitaryEducation.MilitarySchool.MilitarySchoolName, currentUser));

                    if (oldPersonMilitaryEducation.MilitaryEducationTypeCode.Trim() != personMilitaryEducation.MilitaryEducationTypeCode.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VU_MilitaryEducationType", oldPersonMilitaryEducation.MilitaryEducationType.MilitaryEducationTypeName, personMilitaryEducation.MilitaryEducationType.MilitaryEducationTypeName, currentUser));

                    if (oldPersonMilitaryEducation.MilitarySchoolSubjectId != personMilitaryEducation.MilitarySchoolSubjectId)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VU_MilitarySchoolSubject", oldPersonMilitaryEducation.MilitarySchoolSubject.MilitarySchoolSubjectName, personMilitaryEducation.MilitarySchoolSubject.MilitarySchoolSubjectName, currentUser));

                    if (oldPersonMilitaryEducation.MilitaryArmsCode != personMilitaryEducation.MilitaryArmsCode)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VU_MilitaryArms", oldPersonMilitaryEducation.MilitaryArms != null ? oldPersonMilitaryEducation.MilitaryArms.MilitaryArmsName : "", personMilitaryEducation.MilitaryArms != null ? personMilitaryEducation.MilitaryArms.MilitaryArmsName : "", currentUser));

                    if (oldPersonMilitaryEducation.GraduateYear != personMilitaryEducation.GraduateYear)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VU_GraduateYear", oldPersonMilitaryEducation.GraduateYear.ToString(), personMilitaryEducation.GraduateYear.ToString(), currentUser));


                    if (oldPersonMilitaryEducation.CountryCode.Trim() != personMilitaryEducation.CountryCode.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VU_Country", oldPersonMilitaryEducation.Country.CountryName, personMilitaryEducation.Country.CountryName, currentUser));

                    if (oldPersonMilitaryEducation.LearningMethod.LearningMethodKey.Trim() != personMilitaryEducation.LearningMethod.LearningMethodKey.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VU_LearningMethod", oldPersonMilitaryEducation.LearningMethod.LearningMethodName, personMilitaryEducation.LearningMethod.LearningMethodName, currentUser));

                    if (oldPersonMilitaryEducation.VitoshaMilitaryReportSpecialityCode != personMilitaryEducation.VitoshaMilitaryReportSpecialityCode)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VU_VitoshaMilitaryReportSpeciality", (oldPersonMilitaryEducation.VitoshaMilitaryReportSpeciality != null ? oldPersonMilitaryEducation.VitoshaMilitaryReportSpeciality.CodeAndName : ""), (personMilitaryEducation.VitoshaMilitaryReportSpeciality != null ? personMilitaryEducation.VitoshaMilitaryReportSpeciality.CodeAndName : ""), currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramMilitaryEducationId = new OracleParameter();
                paramMilitaryEducationId.ParameterName = "MilitaryEducationId";
                paramMilitaryEducationId.OracleType = OracleType.Number;

                if (personMilitaryEducation.MilitaryEducationId != 0)
                {
                    paramMilitaryEducationId.Direction = ParameterDirection.Input;
                    paramMilitaryEducationId.Value = personMilitaryEducation.MilitaryEducationId;
                }
                else
                {
                    paramMilitaryEducationId.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramMilitaryEducationId);

                OracleParameter param = null;

                if (personMilitaryEducation.MilitaryEducationId == 0)
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
                    param.ParameterName = "MilitaryEducationId";
                    param.OracleType = OracleType.Number;
                    param.Direction = ParameterDirection.Input;
                    param.Value = personMilitaryEducation.MilitaryEducationId;
                    cmd.Parameters.Add(param);
                }

                param = new OracleParameter();
                param.ParameterName = "MilitarySchoolCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personMilitaryEducation.MilitarySchool.MilitarySchoolCode;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryEducationTypeCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personMilitaryEducation.MilitaryEducationTypeCode;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitarySchoolSubjectCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personMilitaryEducation.MilitarySchoolSubject.MilitarySchoolSubjectCode;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryArmsCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personMilitaryEducation.MilitaryArmsCode;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "GraduateYear";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = personMilitaryEducation.GraduateYear;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "CountryId";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personMilitaryEducation.CountryCode;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "LearningMethodKey";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personMilitaryEducation.LearningMethod.LearningMethodKey;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "VitoshaMilRepSpecCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personMilitaryEducation.VitoshaMilitaryReportSpecialityCode;
                cmd.Parameters.Add(param);


                cmd.ExecuteNonQuery();

                if (personMilitaryEducation.MilitaryEducationId == 0)
                    personMilitaryEducation.MilitaryEducationId = DBCommon.GetInt(paramMilitaryEducationId.Value);

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

        public static bool DeletePersonMilitaryEducation(int militaryEducationId, Person person, User currentUser, Change changeEntry)
        {
            bool result = false;

            PersonMilitaryEducation oldPersonMilitaryEducation = GetPersonMilitaryEducation(militaryEducationId, currentUser);

            ChangeEvent changeEvent = new ChangeEvent("RES_Reservist_EduWork_DeleteMltEdu_VU", "", null, person, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VU_MilitarySchool", oldPersonMilitaryEducation.MilitarySchool.MilitarySchoolName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VU_MilitaryEducationType", oldPersonMilitaryEducation.MilitaryEducationType.MilitaryEducationTypeName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VU_MilitarySchoolSubject", oldPersonMilitaryEducation.MilitarySchoolSubject.MilitarySchoolSubjectName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VU_MilitaryArms", oldPersonMilitaryEducation.MilitaryArms != null ? oldPersonMilitaryEducation.MilitaryArms.MilitaryArmsName : "", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VU_GraduateYear", oldPersonMilitaryEducation.GraduateYear.ToString(), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VU_Country", oldPersonMilitaryEducation.Country.CountryName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltEdu_VU_LearningMethod", oldPersonMilitaryEducation.LearningMethod.LearningMethodName, "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" BEGIN
                                  DELETE FROM VS_OWNER.VS_OBRV WHERE OBRVID = :MilitaryEducationId;
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitaryEducationId", OracleType.Number).Value = militaryEducationId;

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
