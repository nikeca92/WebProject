using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;

namespace PMIS.Common
{
    public class PersonMilitaryTrainingCource : BaseDbObject
    {
        public int MilitaryTrainingCourceId { get; set; }

        public string MilitaryTrainingCourceCode { get; set; }
        private MilitaryTrainingCource militaryTrainingCource;
        public MilitaryTrainingCource MilitaryTrainingCource
        {
            get
            {
                if (militaryTrainingCource == null)
                {
                    militaryTrainingCource = MilitaryTrainingCourceUtil.GetMilitaryTrainingCource(MilitaryTrainingCourceCode, CurrentUser);
                }
                return militaryTrainingCource;
            }
        }

        public int? MilitaryTrainingCourceDurationMonth { get; set; }
        public int? MilitaryTrainingCourceDurationDay { get; set; }
        public int? MilitaryTrainingCourceLevel { get; set; }

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

        public string MilitaryTrainingCourceVacAnn { get; set; }

        public DateTime? MilitaryTrainingCourceDateWhen { get; set; }

        public string MilitaryCommanderRankCode { get; set; }
        private MilitaryCommanderRank militaryCommanderRank;
        public MilitaryCommanderRank MilitaryCommanderRank
        {
            get
            {
                if (militaryCommanderRank == null)
                {
                    militaryCommanderRank = MilitaryCommanderRankUtil.GetMilitaryCommanderRank(MilitaryCommanderRankCode, CurrentUser);
                }
                return militaryCommanderRank;
            }
        }

        public DateTime? MilitaryTrainingCourceDateOfCource { get; set; }

        public string PersonLanguageCode { get; set; }
        private PersonLanguage personLanguage;
        public PersonLanguage PersonLanguage
        {
            get
            {
                if (personLanguage == null)
                {
                    personLanguage = PersonLanguageUtil.GetPersonLanguage(PersonLanguageCode, CurrentUser);
                }
                return personLanguage;
            }
        }

        public int MilitarySchoolId { get; set; }
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

        public string MilitaryTrainingCourceNameDescription { get; set; }

        private bool canDelete;
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

        public PersonMilitaryTrainingCource(User user)
            : base(user)
        {

        }
    }

    public class PersonMilitaryTrainingCourceUtil
    {
        private static PersonMilitaryTrainingCource ExtractPersonMilitaryTrainingCourceFromDR(OracleDataReader dr, User currentUser)
        {
            PersonMilitaryTrainingCource personMilitaryTrainingCource = new PersonMilitaryTrainingCource(currentUser);

            personMilitaryTrainingCource.MilitaryTrainingCourceId = DBCommon.GetInt(dr["MilitaryTrainingCourceId"]);

            personMilitaryTrainingCource.MilitaryTrainingCourceCode = dr["MilitaryTrainingCourceCode"].ToString();

            int DurationDay = DBCommon.GetInt(dr["MilitaryTrainingCourceDurDay"]);
            int DurationMonth = DBCommon.GetInt(dr["MilitaryTrainingCourceDurMonth"]);
            int CourceLevel = DBCommon.GetInt(dr["MilitaryTrainingCourceLevel"]);

            if (DurationDay != -1) personMilitaryTrainingCource.MilitaryTrainingCourceDurationDay = DurationDay;
            if (DurationMonth != -1) personMilitaryTrainingCource.MilitaryTrainingCourceDurationMonth = DurationMonth;
            if (CourceLevel != -1) personMilitaryTrainingCource.MilitaryTrainingCourceLevel = CourceLevel;

            personMilitaryTrainingCource.CountryCode = dr["CountryCode"].ToString();
            personMilitaryTrainingCource.MilitaryTrainingCourceVacAnn = dr["MilitaryTrainingCourceVacAnn"].ToString();

            if (dr["MilitaryTrainingCourceDateWhen"] is DateTime) personMilitaryTrainingCource.MilitaryTrainingCourceDateWhen = Convert.ToDateTime(dr["MilitaryTrainingCourceDateWhen"]);


            personMilitaryTrainingCource.MilitaryCommanderRankCode = dr["MilitaryCommanderRankCode"].ToString();

            if (dr["MilitaryCourceDateOfCource"] is DateTime) personMilitaryTrainingCource.MilitaryTrainingCourceDateOfCource = Convert.ToDateTime(dr["MilitaryCourceDateOfCource"]);

            personMilitaryTrainingCource.PersonLanguageCode = dr["PersonLanguageCode"].ToString();

            personMilitaryTrainingCource.MilitarySchoolId = DBCommon.GetInt(dr["MilitarySchoolId"]);
            personMilitaryTrainingCource.MilitaryTrainingCourceNameDescription = dr["MilitaryTrCourceNameDescr"].ToString();

            personMilitaryTrainingCource.VitoshaMilitaryReportSpecialityCode = dr["VitoshaMilRepSpecCode"].ToString();

            return personMilitaryTrainingCource;
        }

        public static PersonMilitaryTrainingCource GetPersonMilitaryTrainingCource(int militaryTrainingCourceId, User currentUser)
        {
            PersonMilitaryTrainingCource personMilitaryTrainingCource = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT 
                                a.KURSID as MilitaryTrainingCourceId,
                                a.KURS_KUSKOD as MilitaryTrainingCourceCode,
                                a.KURS_PROD_DNI as MilitaryTrainingCourceDurDay,
                                a.KURS_PROD as MilitaryTrainingCourceDurMonth,
                                a.KURS_NIVO as MilitaryTrainingCourceLevel,
                                a.kurs_djjkod as CountryCode,  
                                a.KURS_ZPVD as MilitaryTrainingCourceVacAnn,
                                a.KURS_KOGA as MilitaryTrainingCourceDateWhen,
                                a.KURS_SPZKOD as MilitaryCommanderRankCode,
                                a.KURS_DATA as MilitaryCourceDateOfCource,
                                a.KURS_EZKKOD as PersonLanguageCode,
                                m.VVUID as MilitarySchoolId,
                                a.KURS_IMEK as MilitaryTrCourceNameDescr,
                                a.KURS_VOS as VitoshaMilRepSpecCode,
                                a.KURS_VOS as VitoshaMilRepSpecCode
                                FROM VS_OWNER.VS_KURS a
                                LEFT OUTER JOIN VS_OWNER.KLV_VVU m on m.VVU_KOD = a.KURS_VVUKOD                       
                                WHERE a.KURSID = :MilitaryTrainingCourceId";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitaryTrainingCourceId", OracleType.Number).Value = militaryTrainingCourceId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    personMilitaryTrainingCource = ExtractPersonMilitaryTrainingCourceFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personMilitaryTrainingCource;
        }

        //Use this Method accordin PK define in table to chek exist combination of these 3 parameters
        public static PersonMilitaryTrainingCource GetPersonMilitaryTrainingCource(string identityNumber, string militaryTrainingCourceCode, DateTime militaryTrainingCourceDateWhen, User currentUser)
        {
            PersonMilitaryTrainingCource personMilitaryTrainingCource = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT 
                                 a.KURSID as MilitaryTrainingCourceId,
                                a.KURS_KUSKOD as MilitaryTrainingCourceCode,
                                a.KURS_PROD_DNI as MilitaryTrainingCourceDurDay,
                                a.KURS_PROD as MilitaryTrainingCourceDurMonth,
                                a.KURS_NIVO as MilitaryTrainingCourceLevel,
                                a.kurs_djjkod as CountryCode,  
                                a.KURS_ZPVD as MilitaryTrainingCourceVacAnn,
                                a.KURS_KOGA as MilitaryTrainingCourceDateWhen,
                                a.KURS_SPZKOD as MilitaryCommanderRankCode,
                                a.KURS_DATA as MilitaryCourceDateOfCource,
                                a.KURS_EZKKOD as PersonLanguageCode,
                                m.VVUID as MilitarySchoolId,
                                a.KURS_IMEK as MilitaryTrCourceNameDescr,
                                a.KURS_VOS as VitoshaMilRepSpecCode
                                FROM VS_OWNER.VS_KURS a
                                LEFT OUTER JOIN VS_OWNER.KLV_VVU m on m.VVU_KOD = a.KURS_VVUKOD   
                                WHERE a.KURS_EGNLS = :IdentityNumber
                                AND a.KURS_KUSKOD = :MilitaryTrainingCourceCode
                                AND a.KURS_KOGA = :MilitaryTrainingCourceDateWhen";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("IdentityNumber", OracleType.VarChar).Value = identityNumber;
                cmd.Parameters.Add("MilitaryTrainingCourceCode", OracleType.VarChar).Value = militaryTrainingCourceCode;
                cmd.Parameters.Add("MilitaryTrainingCourceDateWhen", OracleType.DateTime).Value = militaryTrainingCourceDateWhen;
                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    personMilitaryTrainingCource = ExtractPersonMilitaryTrainingCourceFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personMilitaryTrainingCource;
        }

        public static List<PersonMilitaryTrainingCource> GetAllPersonMilitaryTrainingCourceByPersonID(int personId, User currentUser)
        {
            List<PersonMilitaryTrainingCource> listPersonMilitaryTrainingCource = new List<PersonMilitaryTrainingCource>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT 
                                a.KURSID as MilitaryTrainingCourceId,
                                a.KURS_KUSKOD as MilitaryTrainingCourceCode,
                                a.KURS_PROD_DNI as MilitaryTrainingCourceDurDay,
                                a.KURS_PROD as MilitaryTrainingCourceDurMonth,
                                a.KURS_NIVO as MilitaryTrainingCourceLevel,
                                a.kurs_djjkod as CountryCode,  
                                a.KURS_ZPVD as MilitaryTrainingCourceVacAnn,
                                a.KURS_KOGA as MilitaryTrainingCourceDateWhen,
                                a.KURS_SPZKOD as MilitaryCommanderRankCode,
                                a.KURS_DATA as MilitaryCourceDateOfCource,
                                a.KURS_EZKKOD as PersonLanguageCode,
                                m.VVUID as MilitarySchoolId,
                                a.KURS_IMEK as MilitaryTrCourceNameDescr,
                                a.KURS_VOS as VitoshaMilRepSpecCode
                                FROM VS_OWNER.VS_KURS a
                                LEFT OUTER JOIN VS_OWNER.KLV_VVU m on m.VVU_KOD = a.KURS_VVUKOD   
                                INNER JOIN VS_OWNER.VS_LS c ON a.KURS_EGNLS = c.EGN
                                WHERE c.PersonID = :PersonID
                                ORDER BY a.KURS_KUSKOD, a.KURS_KOGA, a.KURS_EZKKOD, a.KURS_IMEK";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Number).Value = personId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    listPersonMilitaryTrainingCource.Add(ExtractPersonMilitaryTrainingCourceFromDR(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listPersonMilitaryTrainingCource;
        }

        public static bool SavePersonMilitaryEducationAcademy(PersonMilitaryTrainingCource personMilitaryTrainingCource, Person person, User currentUser, Change changeEntry)
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

                if (personMilitaryTrainingCource.MilitaryTrainingCourceId == 0)
                {
                    SQL += @"INSERT INTO VS_OWNER.VS_KURS (
                                KURS_EGNLS,                                
                                KURS_KUSKOD,
                                KURS_PROD_DNI,
                                KURS_PROD,
                                KURS_NIVO,
                                kurs_djjkod,  
                                KURS_ZPVD,
                                KURS_KOGA,
                                KURS_SPZKOD,
                                KURS_DATA,
                                KURS_EZKKOD,
                                KURS_VVUKOD,
                                KURS_IMEK,
                                KURS_VOS )
                            VALUES (
                                :IdentNumber,
                                :MilitaryTrainingCourceCode,
                                :MilitaryTrCourceDurDay,
                                :MilitaryTrainingCourceDurMonth,
                                :MilitaryTrainingCourceLevel,
                                :CountryCode,  
                                :MilitaryTrainingCourceVacAnn,
                                :MilitaryTrainingCourceDateWhen,
                                :MilitaryCommanderRankCode,
                                :MilitaryTrCourceDateOfCource,
                                :PersonLanguageCode,
                                :MilitarySchoolCode,
                                :MilitaryTrCourceNameDescr,
                                :VitoshaMilRepSpecCode);

                            SELECT VS_OWNER.VS_KURS_KURSID_SEQ.currval INTO :MilitaryTrainingCourceId FROM dual;
                            
                            ";

                    changeEvent = new ChangeEvent("RES_Reservist_EduWork_AddTrainingCource", "", null, person, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_TrainingCource", "", personMilitaryTrainingCource.MilitaryTrainingCource.MilitaryTrainingCourceName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_DurationMonth", "", personMilitaryTrainingCource.MilitaryTrainingCourceDurationMonth.HasValue ? personMilitaryTrainingCource.MilitaryTrainingCourceDurationMonth.Value.ToString() : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_DurationDay", "", personMilitaryTrainingCource.MilitaryTrainingCourceDurationDay.HasValue ? personMilitaryTrainingCource.MilitaryTrainingCourceDurationDay.Value.ToString() : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_Level", "", personMilitaryTrainingCource.MilitaryTrainingCourceLevel.HasValue ? personMilitaryTrainingCource.MilitaryTrainingCourceLevel.Value.ToString() : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_Country", "", personMilitaryTrainingCource.Country.CountryName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_VacAnn", "", string.IsNullOrEmpty(personMilitaryTrainingCource.MilitaryTrainingCourceVacAnn) ? personMilitaryTrainingCource.MilitaryTrainingCourceVacAnn : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_DateWhen", "", CommonFunctions.FormatDate(personMilitaryTrainingCource.MilitaryTrainingCourceDateWhen.Value), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_CommanderRank", "", personMilitaryTrainingCource.MilitaryCommanderRank != null ? personMilitaryTrainingCource.MilitaryCommanderRank.MilitaryCommanderRankName : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_DateOfCource", "", personMilitaryTrainingCource.MilitaryTrainingCourceDateOfCource != null ? CommonFunctions.FormatDate(personMilitaryTrainingCource.MilitaryTrainingCourceDateOfCource.Value) : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_Language", "", personMilitaryTrainingCource.PersonLanguage != null ? personMilitaryTrainingCource.PersonLanguage.PersonLanguageName : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_MilitarySchool", "", personMilitaryTrainingCource.MilitarySchool != null ? personMilitaryTrainingCource.MilitarySchool.MilitarySchoolName : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_Description", "",  personMilitaryTrainingCource.MilitaryTrainingCourceNameDescription, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_VitoshaMilitaryReportSpeciality", "", (personMilitaryTrainingCource.VitoshaMilitaryReportSpeciality != null ? personMilitaryTrainingCource.VitoshaMilitaryReportSpeciality.CodeAndName : ""), currentUser));
                }
                else
                {
                    SQL += @"UPDATE VS_OWNER.VS_KURS SET
                                KURS_KUSKOD   = :MilitaryTrainingCourceCode,
                                KURS_PROD_DNI = :MilitaryTrCourceDurDay,
                                KURS_PROD     = :MilitaryTrainingCourceDurMonth,
                                KURS_NIVO     = :MilitaryTrainingCourceLevel,
                                kurs_djjkod   = :CountryCode,  
                                KURS_ZPVD     = :MilitaryTrainingCourceVacAnn,
                                KURS_KOGA     = :MilitaryTrainingCourceDateWhen,
                                KURS_SPZKOD   = :MilitaryCommanderRankCode,
                                KURS_DATA     = :MilitaryTrCourceDateOfCource,
                                KURS_EZKKOD   = :PersonLanguageCode,
                                KURS_VVUKOD   = :MilitarySchoolCode,
                                KURS_IMEK     = :MilitaryTrCourceNameDescr,
                                KURS_VOS      = :VitoshaMilRepSpecCode

                              WHERE KURSID = :MilitaryTrainingCourceId ; ";



                    PersonMilitaryTrainingCource oldPersonMilitaryTrainingCource = GetPersonMilitaryTrainingCource(personMilitaryTrainingCource.MilitaryTrainingCourceId, currentUser);

                    string logDescription = "Курс: " + oldPersonMilitaryTrainingCource.MilitaryTrainingCource.MilitaryTrainingCourceName + "; " +
                                            "Дата на заповедта: " + CommonFunctions.FormatDate(oldPersonMilitaryTrainingCource.MilitaryTrainingCourceDateWhen);


                    changeEvent = new ChangeEvent("RES_Reservist_EduWork_EditMltTrainingCource", logDescription, null, person, currentUser);

                    if (oldPersonMilitaryTrainingCource.MilitaryTrainingCourceCode.Trim() != personMilitaryTrainingCource.MilitaryTrainingCourceCode.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_TrainingCource", oldPersonMilitaryTrainingCource.MilitaryTrainingCource.MilitaryTrainingCourceName, personMilitaryTrainingCource.MilitaryTrainingCource.MilitaryTrainingCourceName, currentUser));
                   
                    if (oldPersonMilitaryTrainingCource.MilitaryTrainingCourceDurationMonth != personMilitaryTrainingCource.MilitaryTrainingCourceDurationMonth)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_DurationMonth", oldPersonMilitaryTrainingCource.MilitaryTrainingCourceDurationMonth.HasValue ? oldPersonMilitaryTrainingCource.MilitaryTrainingCourceDurationMonth.Value.ToString() : "", personMilitaryTrainingCource.MilitaryTrainingCourceDurationMonth.HasValue ? personMilitaryTrainingCource.MilitaryTrainingCourceDurationMonth.Value.ToString() : "", currentUser));
                   
                    if (oldPersonMilitaryTrainingCource.MilitaryTrainingCourceDurationDay != personMilitaryTrainingCource.MilitaryTrainingCourceDurationDay)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_DurationDay", oldPersonMilitaryTrainingCource.MilitaryTrainingCourceDurationDay.HasValue ? oldPersonMilitaryTrainingCource.MilitaryTrainingCourceDurationDay.Value.ToString() : "", personMilitaryTrainingCource.MilitaryTrainingCourceDurationDay.HasValue ? personMilitaryTrainingCource.MilitaryTrainingCourceDurationDay.Value.ToString() : "", currentUser));
                   
                    if (oldPersonMilitaryTrainingCource.MilitaryTrainingCourceLevel != personMilitaryTrainingCource.MilitaryTrainingCourceLevel)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_Level", oldPersonMilitaryTrainingCource.MilitaryTrainingCourceLevel.HasValue ? oldPersonMilitaryTrainingCource.MilitaryTrainingCourceLevel.Value.ToString() : "", personMilitaryTrainingCource.MilitaryTrainingCourceLevel.HasValue ? personMilitaryTrainingCource.MilitaryTrainingCourceLevel.Value.ToString() : "", currentUser));
                   
                    if (oldPersonMilitaryTrainingCource.CountryCode != personMilitaryTrainingCource.CountryCode)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_Country", oldPersonMilitaryTrainingCource.Country.CountryName, personMilitaryTrainingCource.Country.CountryName, currentUser));
                   
                    if (oldPersonMilitaryTrainingCource.MilitaryTrainingCourceVacAnn != personMilitaryTrainingCource.MilitaryTrainingCourceVacAnn)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_VacAnn", oldPersonMilitaryTrainingCource.MilitaryTrainingCourceVacAnn, personMilitaryTrainingCource.MilitaryTrainingCourceVacAnn, currentUser));
                   
                    if (oldPersonMilitaryTrainingCource.MilitaryTrainingCourceDateWhen != personMilitaryTrainingCource.MilitaryTrainingCourceDateWhen)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_DateWhen", oldPersonMilitaryTrainingCource.MilitaryTrainingCourceDateWhen.HasValue ? CommonFunctions.FormatDate(oldPersonMilitaryTrainingCource.MilitaryTrainingCourceDateWhen.Value) : "", personMilitaryTrainingCource.MilitaryTrainingCourceDateWhen.HasValue ? CommonFunctions.FormatDate(personMilitaryTrainingCource.MilitaryTrainingCourceDateWhen.Value) : "", currentUser));
                  
                    if (oldPersonMilitaryTrainingCource.MilitaryCommanderRankCode.Trim() != personMilitaryTrainingCource.MilitaryCommanderRankCode.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_CommanderRank", oldPersonMilitaryTrainingCource.MilitaryCommanderRank != null ? oldPersonMilitaryTrainingCource.MilitaryCommanderRank.MilitaryCommanderRankName : "", personMilitaryTrainingCource.MilitaryCommanderRank != null ? personMilitaryTrainingCource.MilitaryCommanderRank.MilitaryCommanderRankName : "", currentUser));
                   
                    if (oldPersonMilitaryTrainingCource.MilitaryTrainingCourceDateOfCource != personMilitaryTrainingCource.MilitaryTrainingCourceDateOfCource)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_DateOfCource", oldPersonMilitaryTrainingCource.MilitaryTrainingCourceDateOfCource != null ? CommonFunctions.FormatDate(oldPersonMilitaryTrainingCource.MilitaryTrainingCourceDateOfCource.Value) : "", personMilitaryTrainingCource.MilitaryTrainingCourceDateOfCource != null ? CommonFunctions.FormatDate(personMilitaryTrainingCource.MilitaryTrainingCourceDateOfCource.Value) : "", currentUser));
                   
                    if (oldPersonMilitaryTrainingCource.PersonLanguageCode.Trim() != personMilitaryTrainingCource.PersonLanguageCode.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_Language", oldPersonMilitaryTrainingCource.PersonLanguage != null ? oldPersonMilitaryTrainingCource.PersonLanguage.PersonLanguageName : "", personMilitaryTrainingCource.PersonLanguage != null ? personMilitaryTrainingCource.PersonLanguage.PersonLanguageName : "", currentUser));
                   
                    if (oldPersonMilitaryTrainingCource.MilitarySchoolId != personMilitaryTrainingCource.MilitarySchoolId)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_MilitarySchool", oldPersonMilitaryTrainingCource.MilitarySchool != null ? oldPersonMilitaryTrainingCource.MilitarySchool.MilitarySchoolName : "", personMilitaryTrainingCource.MilitarySchool != null ? personMilitaryTrainingCource.MilitarySchool.MilitarySchoolName : "", currentUser));
                  
                    if (oldPersonMilitaryTrainingCource.MilitaryTrainingCourceNameDescription.Trim() != personMilitaryTrainingCource.MilitaryTrainingCourceNameDescription.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_Description", oldPersonMilitaryTrainingCource.MilitaryTrainingCourceNameDescription , personMilitaryTrainingCource.MilitaryTrainingCourceNameDescription, currentUser));

                    if (oldPersonMilitaryTrainingCource.VitoshaMilitaryReportSpecialityCode != personMilitaryTrainingCource.VitoshaMilitaryReportSpecialityCode)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_VitoshaMilitaryReportSpeciality", (oldPersonMilitaryTrainingCource.VitoshaMilitaryReportSpeciality != null ? oldPersonMilitaryTrainingCource.VitoshaMilitaryReportSpeciality.CodeAndName : ""), (personMilitaryTrainingCource.VitoshaMilitaryReportSpeciality != null ? personMilitaryTrainingCource.VitoshaMilitaryReportSpeciality.CodeAndName : ""), currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramMilitaryTrainingCourceId = new OracleParameter();
                paramMilitaryTrainingCourceId.ParameterName = "MilitaryTrainingCourceId";
                paramMilitaryTrainingCourceId.OracleType = OracleType.Number;

                if (personMilitaryTrainingCource.MilitaryTrainingCourceId > 0)
                {
                    //Update
                    paramMilitaryTrainingCourceId.Direction = ParameterDirection.Input;
                    paramMilitaryTrainingCourceId.Value = personMilitaryTrainingCource.MilitaryTrainingCourceId;
                }
                else
                {
                    paramMilitaryTrainingCourceId.Direction = ParameterDirection.Output;
                }
                cmd.Parameters.Add(paramMilitaryTrainingCourceId);


                OracleParameter param = null;
                //1.
                if (personMilitaryTrainingCource.MilitaryTrainingCourceId == 0)
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
                    param.ParameterName = "MilitaryTrainingCourceId";
                    param.OracleType = OracleType.Number;
                    param.Direction = ParameterDirection.Input;
                    param.Value = personMilitaryTrainingCource.MilitaryTrainingCourceId;
                    cmd.Parameters.Add(param);
                }
                //2.
                //Mandatory
                param = new OracleParameter();
                param.ParameterName = "MilitaryTrainingCourceCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personMilitaryTrainingCource.MilitaryTrainingCourceCode;
                cmd.Parameters.Add(param);

                //3
                param = new OracleParameter();
                param.ParameterName = "MilitaryTrainingCourceDurMonth";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (personMilitaryTrainingCource.MilitaryTrainingCourceDurationMonth.HasValue)
                {
                    param.Value = personMilitaryTrainingCource.MilitaryTrainingCourceDurationMonth.Value;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);

                //4
                param = new OracleParameter();
                param.ParameterName = "MilitaryTrCourceDurDay";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (personMilitaryTrainingCource.MilitaryTrainingCourceDurationDay.HasValue)
                {
                    param.Value = personMilitaryTrainingCource.MilitaryTrainingCourceDurationDay.Value;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);

                //5
                param = new OracleParameter();
                param.ParameterName = "MilitaryTrainingCourceLevel";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (personMilitaryTrainingCource.MilitaryTrainingCourceLevel.HasValue)
                {
                    param.Value = personMilitaryTrainingCource.MilitaryTrainingCourceLevel.Value;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);

                //6
                //Mandatory
                param = new OracleParameter();
                param.ParameterName = "CountryCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personMilitaryTrainingCource.CountryCode;
                cmd.Parameters.Add(param);

                //7
                param = new OracleParameter();
                param.ParameterName = "MilitaryTrainingCourceVacAnn";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(personMilitaryTrainingCource.MilitaryTrainingCourceVacAnn))
                {
                    param.Value = personMilitaryTrainingCource.MilitaryTrainingCourceVacAnn;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);

                //8
                //Mandatory
                param = new OracleParameter();
                param.ParameterName = "MilitaryTrainingCourceDateWhen";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                param.Value = personMilitaryTrainingCource.MilitaryTrainingCourceDateWhen;
                cmd.Parameters.Add(param);

                //9
                param = new OracleParameter();
                param.ParameterName = "MilitaryCommanderRankCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(personMilitaryTrainingCource.MilitaryCommanderRankCode))
                {
                    param.Value = personMilitaryTrainingCource.MilitaryCommanderRankCode;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);

                //10
                param = new OracleParameter();
                param.ParameterName = "MilitaryTrCourceDateOfCource";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (personMilitaryTrainingCource.MilitaryTrainingCourceDateOfCource.HasValue)
                {
                    param.Value = personMilitaryTrainingCource.MilitaryTrainingCourceDateOfCource;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);

                //11
                param = new OracleParameter();
                param.ParameterName = "PersonLanguageCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(personMilitaryTrainingCource.PersonLanguageCode))
                {
                    param.Value = personMilitaryTrainingCource.PersonLanguageCode;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);

                //12
                param = new OracleParameter();
                param.ParameterName = "MilitarySchoolCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (personMilitaryTrainingCource.MilitarySchoolId > 0)
                {
                    param.Value = personMilitaryTrainingCource.MilitarySchool.MilitarySchoolCode;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);

                //13
                param = new OracleParameter();
                param.ParameterName = "MilitaryTrCourceNameDescr";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(personMilitaryTrainingCource.MilitaryTrainingCourceNameDescription))
                {
                    param.Value = personMilitaryTrainingCource.MilitaryTrainingCourceNameDescription;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "VitoshaMilRepSpecCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personMilitaryTrainingCource.VitoshaMilitaryReportSpecialityCode;
                cmd.Parameters.Add(param);


                cmd.ExecuteNonQuery();

                if (personMilitaryTrainingCource.MilitaryTrainingCourceId == 0)
                    personMilitaryTrainingCource.MilitaryTrainingCourceId = DBCommon.GetInt(paramMilitaryTrainingCourceId.Value);

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

        public static bool DeletePersonMilitaryTrainingCource(int militaryTrainingCourceId, Person person, User currentUser, Change changeEntry)
        {
            bool result = false;

            PersonMilitaryTrainingCource oldPersonMilitaryTrainingCource = GetPersonMilitaryTrainingCource(militaryTrainingCourceId, currentUser);

            ChangeEvent changeEvent = new ChangeEvent("RES_Reservist_EduWork_DeleteTrainingCource", "", null, person, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_TrainingCource", oldPersonMilitaryTrainingCource.MilitaryTrainingCource.MilitaryTrainingCourceName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_DurationMonth", oldPersonMilitaryTrainingCource.MilitaryTrainingCourceDurationMonth.HasValue ? oldPersonMilitaryTrainingCource.MilitaryTrainingCourceDurationMonth.Value.ToString() : "", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_DurationDay", oldPersonMilitaryTrainingCource.MilitaryTrainingCourceDurationDay.HasValue ? oldPersonMilitaryTrainingCource.MilitaryTrainingCourceDurationDay.Value.ToString() : "", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_Level", oldPersonMilitaryTrainingCource.MilitaryTrainingCourceLevel.HasValue ? oldPersonMilitaryTrainingCource.MilitaryTrainingCourceLevel.Value.ToString() : "", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_Country", oldPersonMilitaryTrainingCource.Country.CountryName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_VacAnn", string.IsNullOrEmpty(oldPersonMilitaryTrainingCource.MilitaryTrainingCourceVacAnn) ? oldPersonMilitaryTrainingCource.MilitaryTrainingCourceVacAnn : "", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_DateWhen", CommonFunctions.FormatDate(oldPersonMilitaryTrainingCource.MilitaryTrainingCourceDateWhen.Value), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_CommanderRank", oldPersonMilitaryTrainingCource.MilitaryCommanderRank != null ? oldPersonMilitaryTrainingCource.MilitaryCommanderRank.MilitaryCommanderRankName : "", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_DateOfCource", oldPersonMilitaryTrainingCource.MilitaryTrainingCourceDateOfCource != null ? CommonFunctions.FormatDate(oldPersonMilitaryTrainingCource.MilitaryTrainingCourceDateOfCource.Value) : "", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_Language", oldPersonMilitaryTrainingCource.PersonLanguage != null ? oldPersonMilitaryTrainingCource.PersonLanguage.PersonLanguageName : "", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_MilitarySchool", oldPersonMilitaryTrainingCource.MilitarySchool != null ? oldPersonMilitaryTrainingCource.MilitarySchool.MilitarySchoolName : "", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_MltTrCource_Description", string.IsNullOrEmpty(oldPersonMilitaryTrainingCource.MilitaryTrainingCourceNameDescription) ? oldPersonMilitaryTrainingCource.MilitaryTrainingCourceNameDescription : "", "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" BEGIN
                                  DELETE FROM VS_OWNER.VS_KURS WHERE KURSID = :MilitaryTrainingCourceId;
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitaryTrainingCourceId", OracleType.Number).Value = militaryTrainingCourceId;

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
