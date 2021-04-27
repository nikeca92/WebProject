using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;

namespace PMIS.Common
{
    public class PersonLangEduForeignLanguage : BaseDbObject
    {
        public int PersonLangEduForeignLanguageId { get; set; }

        public string LanguageCode { get; set; }
        public PersonLanguage personLanguage;
        public PersonLanguage PersonLanguage
        {
            get
            {
                if (personLanguage == null)
                {
                    personLanguage = PersonLanguageUtil.GetPersonLanguage(LanguageCode, CurrentUser);
                }
                return personLanguage;
            }
        }

        public string LanguageLevelOfKnowledgeKey { get; set; }
        public LanguageLevelOfKnowledge languageLevelOfKnowledge;
        public LanguageLevelOfKnowledge LanguageLevelOfKnowledge
        {
            get
            {
                if (languageLevelOfKnowledge == null)
                {
                    languageLevelOfKnowledge = LanguageLevelOfKnowledgeUtil.GetLanguageLevelOfKnowledge(LanguageLevelOfKnowledgeKey, CurrentUser);
                }
                return languageLevelOfKnowledge;
            }
        }

        public string LanguageFormOfKnowledgeKey { get; set; }
        public LanguageFormOfKnowledge languageFormOfKnowledge;
        public LanguageFormOfKnowledge LanguageFormOfKnowledge
        {
            get
            {
                if (languageFormOfKnowledge == null)
                {
                    languageFormOfKnowledge = LanguageFormOfKnowledgeUtil.GetLanguageFormOfKnowledge(LanguageFormOfKnowledgeKey, CurrentUser);
                }
                return languageFormOfKnowledge;
            }
        }

        public string LanguageStanAg { get; set; }

        public string LanguageDiplomaKey { get; set; }
        public LanguageDiploma languageDiploma;
        public LanguageDiploma LanguageDiploma
        {
            get
            {
                if (languageDiploma == null)
                {
                    languageDiploma = LanguageDiplomaUtil.GetLanguageDiploma(LanguageDiplomaKey, CurrentUser);
                }
                return languageDiploma;
            }
        }

        public string LanguageVacAnn { get; set; }

        public DateTime? LanguageDateWhen { get; set; }

        private bool canDelete;
        public bool CanDelete
        {
            get
            {
                return true;
            }
        }

        public PersonLangEduForeignLanguage(User user)
            : base(user)
        {

        }

    }
    public class PersonLangEduForeignLanguageUtil
    {
        private static PersonLangEduForeignLanguage ExtractPersonLangEduForeignLanguageFromDR(OracleDataReader dr, User currentUser)
        {
            PersonLangEduForeignLanguage personLangEduForeignLanguage = new PersonLangEduForeignLanguage(currentUser);

            personLangEduForeignLanguage.PersonLangEduForeignLanguageId = DBCommon.GetInt(dr["PersonLangEduForeignLanguageId"]);

            personLangEduForeignLanguage.LanguageCode = dr["LanguageCode"].ToString();

            personLangEduForeignLanguage.LanguageLevelOfKnowledgeKey = dr["LanguageLevelOfKnowledgeKey"].ToString();

            personLangEduForeignLanguage.LanguageFormOfKnowledgeKey = dr["LanguageFormOfKnowledgeKey"].ToString();

            personLangEduForeignLanguage.LanguageDiplomaKey = dr["LanguageDiplomaKey"].ToString();

            personLangEduForeignLanguage.LanguageStanAg = dr["LanguageStanAg"].ToString();

            personLangEduForeignLanguage.LanguageVacAnn = dr["LanguageVacAnn"].ToString();

            if (dr["LanguageDateWhen"] is DateTime) personLangEduForeignLanguage.LanguageDateWhen = Convert.ToDateTime(dr["LanguageDateWhen"]);

            return personLangEduForeignLanguage;
        }

        public static PersonLangEduForeignLanguage GetPersonLangEduForeignLanguage(int personLangEduForeignLanguageId, User currentUser)
        {
            PersonLangEduForeignLanguage personLangEduForeignLanguage = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT 
                                a.EZIKID as PersonLangEduForeignLanguageId,
                                a.EZIK_EZKKOD as LanguageCode,
                                a.EZIK_SVEKOD as LanguageLevelOfKnowledgeKey,
                                a.EZIK_FVLKOD as LanguageFormOfKnowledgeKey,
                                a.EZIK_STANAG as LanguageStanAg,
                                a.EZIK_DEZKOD as LanguageDiplomaKey,
                                a.EZIK_ZPVD as LanguageVacAnn,  
                                a.EZIK_KOGA as LanguageDateWhen
                                FROM VS_OWNER.VS_EZIK a
                                WHERE a.EZIKID = :PersonLangEduForeignLanguageId";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonLangEduForeignLanguageId", OracleType.Number).Value = personLangEduForeignLanguageId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    personLangEduForeignLanguage = ExtractPersonLangEduForeignLanguageFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personLangEduForeignLanguage;
        }

        //Use this Method accordin PK define in table to chek exist combination of these 2 parameters
        public static PersonLangEduForeignLanguage GetPersonLangEduForeignLanguage(string identityNumber, string languageCode, User currentUser)
        {
            PersonLangEduForeignLanguage personLangEduForeignLanguage = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT 
                                a.EZIKID as PersonLangEduForeignLanguageId,
                                a.EZIK_EZKKOD as LanguageCode,
                                a.EZIK_SVEKOD as LanguageLevelOfKnowledgeKey,
                                a.EZIK_FVLKOD as LanguageFormOfKnowledgeKey,
                                a.EZIK_STANAG as LanguageStanAg,
                                a.EZIK_DEZKOD as LanguageDiplomaKey,
                                a.EZIK_ZPVD as LanguageVacAnn,  
                                a.EZIK_KOGA as LanguageDateWhen
                                FROM VS_OWNER.VS_EZIK a  
                                WHERE a.EZIK_EGNLS = :IdentityNumber
                                AND a.EZIK_EZKKOD = :LanguageCode";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("IdentityNumber", OracleType.VarChar).Value = identityNumber;
                cmd.Parameters.Add("LanguageCode", OracleType.VarChar).Value = languageCode;
                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    personLangEduForeignLanguage = ExtractPersonLangEduForeignLanguageFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personLangEduForeignLanguage;
        }

        public static List<PersonLangEduForeignLanguage> GetAllPersonLangEduForeignLanguageByPersonID(int personId, User currentUser)
        {
            List<PersonLangEduForeignLanguage> listPersonLangEduForeignLanguage = new List<PersonLangEduForeignLanguage>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT 
                                a.EZIKID as PersonLangEduForeignLanguageId,
                                a.EZIK_EZKKOD as LanguageCode,
                                a.EZIK_SVEKOD as LanguageLevelOfKnowledgeKey,
                                a.EZIK_FVLKOD as LanguageFormOfKnowledgeKey,
                                a.EZIK_STANAG as LanguageStanAg,
                                a.EZIK_DEZKOD as LanguageDiplomaKey,
                                a.EZIK_ZPVD as LanguageVacAnn,  
                                a.EZIK_KOGA as LanguageDateWhen
                                FROM VS_OWNER.VS_EZIK a
                              INNER JOIN VS_OWNER.VS_LS c ON a.EZIK_EGNLS = c.EGN
                              WHERE c.PersonID = :PersonID
                              ORDER BY a.EZIK_EZKKOD, a.EZIK_SVEKOD, a.EZIK_FVLKOD, a.EZIK_DEZKOD";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Number).Value = personId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    listPersonLangEduForeignLanguage.Add(ExtractPersonLangEduForeignLanguageFromDR(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listPersonLangEduForeignLanguage;
        }

        public static bool SavePersonMilitaryEducationAcademy(PersonLangEduForeignLanguage personLangEduForeignLanguage, Person person, User currentUser, Change changeEntry)
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

                if (personLangEduForeignLanguage.PersonLangEduForeignLanguageId == 0)
                {
                    SQL += @"INSERT INTO VS_OWNER.VS_EZIK (
                                EZIK_EGNLS,
                                EZIK_EZKKOD,
                                EZIK_SVEKOD,
                                EZIK_FVLKOD,
                                EZIK_STANAG,
                                EZIK_DEZKOD,
                                EZIK_ZPVD,  
                                EZIK_KOGA)
                            VALUES (
                                :IdentNumber,
                                :LanguageCode,
                                :LanguageLevelOfKnowledgeKey,
                                :LanguageFormOfKnowledgeKey,
                                :LanguageStanAg,
                                :LanguageDiplomaKey,
                                :LanguageVacAnn,  
                                :LanguageDateWhen );

                            SELECT VS_OWNER.VS_EZIK_EZIKID_SEQ.currval INTO :PersonLangEduForeignLanguageId FROM dual;
                            
                            ";

                    changeEvent = new ChangeEvent("RES_Reservist_EduWork_AddLanguage", "", null, person, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_Language_Language", "", personLangEduForeignLanguage.PersonLanguage.PersonLanguageName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_Language_LevelOfKnowledge", "", personLangEduForeignLanguage.LanguageLevelOfKnowledge.LanguageLevelOfKnowledgeName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_Language_FormOfKnowledge", "", personLangEduForeignLanguage.LanguageFormOfKnowledge.LanguageFormOfKnowledgeName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_Language_Diploma", "", personLangEduForeignLanguage.LanguageDiploma.LanguageDiplomaName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_Language_VacAnn", "", string.IsNullOrEmpty(personLangEduForeignLanguage.LanguageVacAnn) ? personLangEduForeignLanguage.LanguageVacAnn : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_Language_DateWhen", "", personLangEduForeignLanguage.LanguageDateWhen.HasValue ? CommonFunctions.FormatDate( personLangEduForeignLanguage.LanguageDateWhen.Value.ToString()) : "", currentUser));

                }
                else
                {
                    SQL += @"UPDATE VS_OWNER.VS_EZIK SET

                                EZIK_EZKKOD = :LanguageCode,
                                EZIK_SVEKOD = :LanguageLevelOfKnowledgeKey,
                                EZIK_FVLKOD = :LanguageFormOfKnowledgeKey,
                                EZIK_STANAG = :LanguageStanAg,
                                EZIK_DEZKOD = :LanguageDiplomaKey,
                                EZIK_ZPVD   = :LanguageVacAnn,  
                                EZIK_KOGA   = :LanguageDateWhen

                              WHERE EZIKID = :PersonLangEduForeignLanguageId ; ";



                    PersonLangEduForeignLanguage oldPersonLangEduForeignLanguage = GetPersonLangEduForeignLanguage(personLangEduForeignLanguage.PersonLangEduForeignLanguageId, currentUser);

                    //usuali logdescription is unique combination define in primary key
                    string logDescription = "Език: " + oldPersonLangEduForeignLanguage.PersonLanguage.PersonLanguageName;

                    changeEvent = new ChangeEvent("RES_Reservist_EduWork_EditLanguage", logDescription, null, person, currentUser);

                    if (oldPersonLangEduForeignLanguage.LanguageCode.Trim() != personLangEduForeignLanguage.LanguageCode.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_Language_Language", oldPersonLangEduForeignLanguage.PersonLanguage.PersonLanguageName, personLangEduForeignLanguage.PersonLanguage.PersonLanguageName, currentUser));

                    if (oldPersonLangEduForeignLanguage.LanguageLevelOfKnowledgeKey.Trim() != personLangEduForeignLanguage.LanguageLevelOfKnowledgeKey.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_Language_LevelOfKnowledge", oldPersonLangEduForeignLanguage.LanguageLevelOfKnowledge.LanguageLevelOfKnowledgeName, personLangEduForeignLanguage.LanguageLevelOfKnowledge.LanguageLevelOfKnowledgeName, currentUser));

                    if (oldPersonLangEduForeignLanguage.LanguageFormOfKnowledgeKey.Trim() != personLangEduForeignLanguage.LanguageFormOfKnowledgeKey.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_Language_FormOfKnowledge", oldPersonLangEduForeignLanguage.LanguageFormOfKnowledge.LanguageFormOfKnowledgeName, personLangEduForeignLanguage.LanguageFormOfKnowledge.LanguageFormOfKnowledgeName, currentUser));

                    if (oldPersonLangEduForeignLanguage.LanguageDiplomaKey.Trim() != personLangEduForeignLanguage.LanguageDiplomaKey.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_Language_Diploma", oldPersonLangEduForeignLanguage.LanguageDiploma.LanguageDiplomaName, personLangEduForeignLanguage.LanguageDiploma.LanguageDiplomaName, currentUser));

                    if (oldPersonLangEduForeignLanguage.LanguageVacAnn.Trim() != personLangEduForeignLanguage.LanguageVacAnn.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_Language_VacAnn", oldPersonLangEduForeignLanguage.LanguageVacAnn, personLangEduForeignLanguage.LanguageVacAnn, currentUser));

                    string oldLanguageDateWhen = CommonFunctions.FormatDate(oldPersonLangEduForeignLanguage.LanguageDateWhen);
                    string newLanguageDateWhen = CommonFunctions.FormatDate(personLangEduForeignLanguage.LanguageDateWhen);
                    if (oldLanguageDateWhen != newLanguageDateWhen)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_Language_DateWhen", oldLanguageDateWhen, newLanguageDateWhen, currentUser));

                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramPersonLangEduForeignLanguageId = new OracleParameter();
                paramPersonLangEduForeignLanguageId.ParameterName = "PersonLangEduForeignLanguageId";
                paramPersonLangEduForeignLanguageId.OracleType = OracleType.Number;

                if (personLangEduForeignLanguage.PersonLangEduForeignLanguageId > 0)
                {
                    //Update
                    paramPersonLangEduForeignLanguageId.Direction = ParameterDirection.Input;
                    paramPersonLangEduForeignLanguageId.Value = personLangEduForeignLanguage.PersonLangEduForeignLanguageId;
                }
                else
                {
                    paramPersonLangEduForeignLanguageId.Direction = ParameterDirection.Output;
                }
                cmd.Parameters.Add(paramPersonLangEduForeignLanguageId);


                OracleParameter param = null;
                //1.
                if (personLangEduForeignLanguage.PersonLangEduForeignLanguageId == 0)
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
                    param.ParameterName = "PersonLangEduForeignLanguageId";
                    param.OracleType = OracleType.Number;
                    param.Direction = ParameterDirection.Input;
                    param.Value = personLangEduForeignLanguage.PersonLangEduForeignLanguageId;
                    cmd.Parameters.Add(param);
                }


                //2.
                //Mandatory
                param = new OracleParameter();
                param.ParameterName = "LanguageCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personLangEduForeignLanguage.LanguageCode;
                cmd.Parameters.Add(param);

                //3
                param = new OracleParameter();
                param.ParameterName = "LanguageLevelOfKnowledgeKey";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personLangEduForeignLanguage.LanguageLevelOfKnowledgeKey;
                cmd.Parameters.Add(param);


                //4
                param = new OracleParameter();
                param.ParameterName = "LanguageFormOfKnowledgeKey";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personLangEduForeignLanguage.LanguageFormOfKnowledgeKey;
                cmd.Parameters.Add(param);


                //5
                param = new OracleParameter();
                param.ParameterName = "LanguageStanAg";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(personLangEduForeignLanguage.LanguageStanAg))
                {
                    param.Value = personLangEduForeignLanguage.LanguageStanAg;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);

                //6
                param = new OracleParameter();
                param.ParameterName = "LanguageDiplomaKey";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personLangEduForeignLanguage.LanguageDiplomaKey;
                cmd.Parameters.Add(param);


                //7
                param = new OracleParameter();
                param.ParameterName = "LanguageVacAnn";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(personLangEduForeignLanguage.LanguageVacAnn))
                {
                    param.Value = personLangEduForeignLanguage.LanguageVacAnn;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);


                //8
                param = new OracleParameter();
                param.ParameterName = "LanguageDateWhen";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (personLangEduForeignLanguage.LanguageDateWhen.HasValue)
                {
                    param.Value = personLangEduForeignLanguage.LanguageDateWhen;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);


                cmd.ExecuteNonQuery();

                if (personLangEduForeignLanguage.PersonLangEduForeignLanguageId == 0)
                    personLangEduForeignLanguage.PersonLangEduForeignLanguageId = DBCommon.GetInt(paramPersonLangEduForeignLanguageId.Value);

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

        public static bool DeletePersonMilitaryTrainingCource(int personLangEduForeignLanguageId, Person person, User currentUser, Change changeEntry)
        {
            bool result = false;
            ChangeEvent changeEvent;

            PersonLangEduForeignLanguage oldPersonLangEduForeignLanguage = GetPersonLangEduForeignLanguage(personLangEduForeignLanguageId, currentUser);

            changeEvent = new ChangeEvent("RES_Reservist_EduWork_DeleteLanguage", "", null, person, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_Language_Language", oldPersonLangEduForeignLanguage.PersonLanguage.PersonLanguageName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_Language_LevelOfKnowledge", oldPersonLangEduForeignLanguage.LanguageLevelOfKnowledge.LanguageLevelOfKnowledgeName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_Language_FormOfKnowledge", oldPersonLangEduForeignLanguage.LanguageFormOfKnowledge.LanguageFormOfKnowledgeName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_Language_Diploma", oldPersonLangEduForeignLanguage.LanguageDiploma.LanguageDiplomaName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_Language_VacAnn", string.IsNullOrEmpty(oldPersonLangEduForeignLanguage.LanguageVacAnn) ? oldPersonLangEduForeignLanguage.LanguageVacAnn : "", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_Language_DateWhen", oldPersonLangEduForeignLanguage.LanguageDateWhen.HasValue ? CommonFunctions.FormatDate(oldPersonLangEduForeignLanguage.LanguageDateWhen.Value.ToString()) : "", "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" BEGIN
                                  DELETE FROM VS_OWNER.VS_EZIK WHERE EZIKID = :PersonLangEduForeignLanguageId;
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonLangEduForeignLanguageId", OracleType.Number).Value = personLangEduForeignLanguageId;

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
