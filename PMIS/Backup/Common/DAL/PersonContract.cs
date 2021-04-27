using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;

namespace PMIS.Common
{
    public class PersonContract : BaseDbObject
    {
        public int PersonContractId { get; set; }

        //1 Вид документ - RV_LOW_VALUE  -> VS_OWNER.CG_REF_CODES AND RV_DOMAIN = VID_DOC(1)
        public string PersonContractDocumentTypeKey { get; set; }
        public DocumentType personContractDocumentType;
        public DocumentType PersonContractDocumentType
        {
            get
            {
                if (personContractDocumentType == null && !string.IsNullOrEmpty(PersonContractDocumentTypeKey))
                {
                    personContractDocumentType = DocumentTypeUtil.GetDocumentType(PersonContractDocumentTypeKey, CurrentUser);
                }
                return personContractDocumentType;
            }
        }

        //2 Номер - DOG_NOM   (10)
        public string PersonContractNumber { get; set; }

        //3 Дата - DOG_KOGA
        public DateTime PersonContractDateWhen { get; set; }

        //4 Изтича на - DOG_SROK  - can be NULL
        public DateTime? PersonContractDatePeriod { get; set; }

        private bool canDelete;
        public bool CanDelete
        {
            get
            {
                return true;
            }
        }

        public PersonContract(User user)
            : base(user)
        {
            PersonContractDocumentTypeKey = "";
            PersonContractNumber = "";
        }

        public DateTime? PersonMilitaryServiceTo { get; set; }

        public string PersonContractDurationKey { get; set; }
        public ContractDuration personContractDuration;
        public ContractDuration PersonContractDuration
        {
            get
            {
                if (personContractDuration == null && !string.IsNullOrEmpty(PersonContractDurationKey))
                {
                    personContractDuration = ContractDurationUtil.GetContractDuration(PersonContractDurationKey, CurrentUser);
                }
                return personContractDuration;
            }
        }
    }
    public class PersonContractUtil
    {

        private static PersonContract ExtractPersonContractFromDR(OracleDataReader dr, User currentUser)
        {
            PersonContract personContract = new PersonContract(currentUser);

            personContract.PersonContractId = DBCommon.GetInt(dr["PersonContractId"]);
            personContract.PersonContractDocumentTypeKey = dr["PersonDocumentTypeKey"].ToString();
            personContract.PersonContractNumber = dr["PersonContractNumber"].ToString();
            personContract.PersonContractDateWhen = (DateTime)(dr["PersonContractDateWhen"]);
            personContract.PersonContractDatePeriod = (dr["PersonContractDatePeriod"] is DateTime ? (DateTime)dr["PersonContractDatePeriod"] : (DateTime?)null);

            personContract.PersonContractDurationKey = dr["PersonContractDurationKey"].ToString();
            personContract.PersonMilitaryServiceTo = (dr["PersonMilitaryServiceTo"] is DateTime ? (DateTime)dr["PersonMilitaryServiceTo"] : (DateTime?)null);
            
            return personContract;
        }

        public static PersonContract GetPersonContract(int personContractId, User currentUser)
        {
            PersonContract PersonContract = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT 
                                  a.DOGID as PersonContractId,
                                  a.DOG_VDGKOD as PersonDocumentTypeKey,
                                  a.DOG_NOM as PersonContractNumber,
                                  a.DOG_KOGA as  PersonContractDateWhen,
                                  a.DOG_SROK as PersonContractDatePeriod,
                                  a.DOG_TDGKOD as PersonContractDurationKey,
                                  a.DOG_VSLDO as PersonMilitaryServiceTo
                              FROM VS_OWNER.VS_DOG a 
                              WHERE a.DOGID = :PersonContractId";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonContractId", OracleType.Number).Value = personContractId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    PersonContract = ExtractPersonContractFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return PersonContract;
        }

        //Use this Method accordin PK define in table to chek exist combination of these 2 parameters
        public static PersonContract GetPersonContract(string identityNumber, string personContractNumber, DateTime personContractDateWhen, User currentUser)
        {
            PersonContract PersonContract = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT 
                                  a.DOGID as PersonContractId,
                                  a.DOG_VDGKOD as PersonDocumentTypeKey,
                                  a.DOG_NOM as PersonContractNumber,
                                  a.DOG_KOGA as  PersonContractDateWhen,
                                  a.DOG_SROK as PersonContractDatePeriod,
                                  a.DOG_TDGKOD as PersonContractDurationKey,
                                  a.DOG_VSLDO as PersonMilitaryServiceTo
                               FROM VS_OWNER.VS_DOG a  
                               WHERE a.DOG_EGNLS = :IdentityNumber 
                               AND   a.DOG_NOM = :PersonContractNumber
                               AND   a.DOG_KOGA = :PersonContractDateWhen";
                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("IdentityNumber", OracleType.VarChar).Value = identityNumber;
                cmd.Parameters.Add("PersonContractNumber", OracleType.VarChar).Value = personContractNumber;
                cmd.Parameters.Add("PersonContractDateWhen", OracleType.DateTime).Value = personContractDateWhen;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    PersonContract = ExtractPersonContractFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return PersonContract;
        }

        public static List<PersonContract> GetAllPersonContractByPersonID(int personId, User currentUser)
        {
            List<PersonContract> listPersonContract = new List<PersonContract>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT
                                 a.DOGID as PersonContractId,
                                 a.DOG_VDGKOD as PersonDocumentTypeKey,
                                 a.DOG_NOM as PersonContractNumber,
                                 a.DOG_KOGA as  PersonContractDateWhen,
                                 a.DOG_SROK as PersonContractDatePeriod,
                                 a.DOG_TDGKOD as PersonContractDurationKey,
                                 a.DOG_VSLDO as PersonMilitaryServiceTo

                               FROM VS_OWNER.VS_DOG a  
                               INNER JOIN VS_OWNER.VS_LS c ON a.DOG_EGNLS = c.EGN
                               WHERE c.PersonID = :PersonID
                               ORDER BY a.DOG_KOGA, a.DOG_NOM";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Number).Value = personId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    listPersonContract.Add(ExtractPersonContractFromDR(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listPersonContract;
        }

        public static bool SavePersonContract(PersonContract personContract, Person person, User currentUser, Change changeEntry)
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

                if (personContract.PersonContractId == 0)
                {
                    SQL += @"INSERT INTO VS_OWNER.VS_DOG (
                                    DOG_EGNLS,
                                    DOG_VDGKOD,
                                    DOG_NOM,
                                    DOG_KOGA,
                                    DOG_SROK,
                                    DOG_TDGKOD,
                                    DOG_VSLDO)

                            VALUES (
                                    :IdentityNumber, 
                                    :PersonDocumentTypeKey,
                                    :PersonContractNumber,
                                    :PersonContractDateWhen,
                                    :PersonContractDatePeriod,
                                    :PersonContractDurationKey,
                                    :PersonMilitaryServiceTo
                                    );

                           SELECT VS_OWNER.VS_DOG_DOGID_SEQ.currval INTO :PersonContractId FROM dual;
                            
                            ";

                    changeEvent = new ChangeEvent("RES_Reservist_MilServ_AddConract", "", null, person, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Contract_DocumentType","", personContract.PersonContractDocumentType.DocumentTypeName,  currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Contract_Number", "",personContract.PersonContractNumber,  currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Contract_DateWhen", "", CommonFunctions.FormatDate(personContract.PersonContractDateWhen), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Contract_DatePeriod","", personContract.PersonContractDatePeriod.HasValue ? CommonFunctions.FormatDate(personContract.PersonContractDatePeriod) : "",  currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Contract_ContractDuration", "", (personContract.PersonContractDuration != null ? personContract.PersonContractDuration.ContractDurationName : ""), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Contract_MilitaryServiceTo", "", personContract.PersonMilitaryServiceTo.HasValue ? CommonFunctions.FormatDate(personContract.PersonMilitaryServiceTo) : "", currentUser));

                }
                else
                {
                    SQL += @"UPDATE VS_OWNER.VS_DOG SET

                                    DOG_VDGKOD = :PersonDocumentTypeKey,
                                    DOG_NOM    = :PersonContractNumber,
                                    DOG_KOGA   = :PersonContractDateWhen,
                                    DOG_SROK   = :PersonContractDatePeriod,
                                    DOG_TDGKOD = :PersonContractDurationKey,
                                    DOG_VSLDO  = :PersonMilitaryServiceTo

                              WHERE DOGID = :PersonContractId ; ";


                    PersonContract oldPersonContract = GetPersonContract(personContract.PersonContractId, currentUser);

                    string logDescription = "Документ: " + oldPersonContract.PersonContractDocumentType.DocumentTypeName + "; " +
                                            "Дата: " + CommonFunctions.FormatDate(oldPersonContract.PersonContractDateWhen);

                    changeEvent = new ChangeEvent("RES_Reservist_MilServ_EditConract", logDescription, null, person, currentUser);

                    if (oldPersonContract.PersonContractDocumentTypeKey != personContract.PersonContractDocumentTypeKey)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Contract_DocumentType", oldPersonContract.PersonContractDocumentType.DocumentTypeName, personContract.PersonContractDocumentType.DocumentTypeName, currentUser));
                    if (oldPersonContract.PersonContractNumber != personContract.PersonContractNumber)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Contract_Number", oldPersonContract.PersonContractNumber, personContract.PersonContractNumber, currentUser));
                    if (oldPersonContract.PersonContractDateWhen != personContract.PersonContractDateWhen)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Contract_DateWhen", CommonFunctions.FormatDate(oldPersonContract.PersonContractDateWhen), CommonFunctions.FormatDate(personContract.PersonContractDateWhen), currentUser));
                    if (oldPersonContract.PersonContractDatePeriod != personContract.PersonContractDatePeriod)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Contract_DatePeriod", oldPersonContract.PersonContractDatePeriod.HasValue ? CommonFunctions.FormatDate(oldPersonContract.PersonContractDatePeriod) : "", personContract.PersonContractDatePeriod.HasValue ? CommonFunctions.FormatDate(personContract.PersonContractDatePeriod) : "", currentUser));
                    if (oldPersonContract.PersonContractDurationKey != personContract.PersonContractDurationKey)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Contract_ContractDuration", oldPersonContract.PersonContractDuration != null ? oldPersonContract.PersonContractDuration.ContractDurationName : "", personContract.PersonContractDuration != null ? personContract.PersonContractDuration.ContractDurationName : "", currentUser));
                    if (oldPersonContract.PersonMilitaryServiceTo != personContract.PersonMilitaryServiceTo)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Contract_MilitaryServiceTo", oldPersonContract.PersonMilitaryServiceTo.HasValue ? CommonFunctions.FormatDate(oldPersonContract.PersonMilitaryServiceTo) : "", personContract.PersonMilitaryServiceTo.HasValue ? CommonFunctions.FormatDate(personContract.PersonMilitaryServiceTo) : "", currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramPenaltyId = new OracleParameter();
                paramPenaltyId.ParameterName = "PersonContractId";
                paramPenaltyId.OracleType = OracleType.Number;

                if (personContract.PersonContractId != 0)
                {
                    paramPenaltyId.Direction = ParameterDirection.Input;
                    paramPenaltyId.Value = personContract.PersonContractId;
                }
                else
                {
                    paramPenaltyId.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramPenaltyId);

                OracleParameter param = null;

                if (personContract.PersonContractId == 0)
                {
                    //Insert
                    param = new OracleParameter();
                    param.ParameterName = "IdentityNumber";
                    param.OracleType = OracleType.VarChar;
                    param.Direction = ParameterDirection.Input;
                    param.Value = person.IdentNumber;
                    cmd.Parameters.Add(param);
                }
                else
                {
                    //Update
                    param = new OracleParameter();
                    param.ParameterName = "PersonContractId";
                    param.OracleType = OracleType.Number;
                    param.Direction = ParameterDirection.Input;
                    param.Value = personContract.PersonContractId;
                    cmd.Parameters.Add(param);
                }

                param = new OracleParameter();
                param.ParameterName = "PersonDocumentTypeKey";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personContract.PersonContractDocumentTypeKey;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PersonContractNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personContract.PersonContractNumber;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PersonContractDateWhen";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                param.Value = personContract.PersonContractDateWhen;
                cmd.Parameters.Add(param);


                param = new OracleParameter();
                param.ParameterName = "PersonContractDatePeriod";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (personContract.PersonContractDatePeriod.HasValue)
                {
                    param.Value = personContract.PersonContractDatePeriod.Value;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PersonContractDurationKey";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = (personContract.PersonContractDuration != null ? personContract.PersonContractDurationKey : "");
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PersonMilitaryServiceTo";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (personContract.PersonMilitaryServiceTo.HasValue)
                    param.Value = personContract.PersonMilitaryServiceTo.Value;
                else
                    param.Value = DBNull.Value;
                
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (personContract.PersonContractId == 0)
                    personContract.PersonContractId = DBCommon.GetInt(paramPenaltyId.Value);

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


        public static bool DeletePersonContract(int personContractId, Person person, User currentUser, Change changeEntry)
        {
            bool result = false;

            PersonContract oldPersonContract = GetPersonContract(personContractId, currentUser);

            ChangeEvent changeEvent = new ChangeEvent("RES_Reservist_MilServ_DeleteConract", "", null, person, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Contract_DocumentType", oldPersonContract.PersonContractDocumentType.DocumentTypeName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Contract_Number", oldPersonContract.PersonContractNumber, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Contract_DateWhen", CommonFunctions.FormatDate(oldPersonContract.PersonContractDateWhen), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Contract_DatePeriod", oldPersonContract.PersonContractDatePeriod.HasValue ? CommonFunctions.FormatDate(oldPersonContract.PersonContractDatePeriod) : "", "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" BEGIN
                                  DELETE  FROM VS_OWNER.VS_DOG WHERE DOGID = :PersonContractId;
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonContractId", OracleType.Number).Value = personContractId;

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
