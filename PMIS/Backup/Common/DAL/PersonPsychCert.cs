using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;

namespace PMIS.Common
{
    //Represents single person psych cert
    public class PersonPsychCert : BaseDbObject
    {
        private int psychCertID;
        private int personID;
        private Person person;
        private DateTime? psychCertDate;
        private string protNum;
        private int? conclusionID;
        private MilitaryMedicalConclusion conclusion;
        private DateTime? expirationDate;

        public int PsychCertID
        {
            get { return psychCertID; }
            set { psychCertID = value; }
        }

        public int PersonID
        {
            get { return personID; }
            set { personID = value; }
        }

        public Person Person
        {
            get
            {
                if (person == null)
                    person = PersonUtil.GetPerson(personID, CurrentUser);

                return person;
            }
            set { person = value; }
        }

        public DateTime? PsychCertDate
        {
            get { return psychCertDate; }
            set { psychCertDate = value; }
        }

        public string ProtNum
        {
            get { return protNum; }
            set { protNum = value; }
        }

        public int? ConclusionID
        {
            get { return conclusionID; }
            set { conclusionID = value; }
        }

        public MilitaryMedicalConclusion Conclusion
        {
            get
            {
                if (conclusion == null && conclusionID.HasValue)
                    conclusion = MilitaryMedicalConclusionUtil.GetMilitaryMedicalConclusion(conclusionID.Value, CurrentUser);

                return conclusion;
            }
            set { conclusion = value; }
        }

        public DateTime? ExpirationDate
        {
            get { return expirationDate; }
            set { expirationDate = value; }
        }

        public bool CanDelete
        {
            get
            {
                return true;
            }
        }

        public PersonPsychCert(User user)
            : base(user)
        {

        }
    }

    public class PersonPsychCertUtil
    {
        private static PersonPsychCert ExtractPersonPsychCertFromDR(OracleDataReader dr, User currentUser)
        {
            PersonPsychCert personPsychCert = new PersonPsychCert(currentUser);

            personPsychCert.PsychCertID = DBCommon.GetInt(dr["PsychCertID"]);
            personPsychCert.PersonID = DBCommon.GetInt(dr["PersonID"]);
            personPsychCert.PsychCertDate = (dr["PsychCertDate"] is DateTime) ? (DateTime)dr["PsychCertDate"] : (DateTime?)null;
            personPsychCert.ProtNum = dr["ProtNum"].ToString();
            personPsychCert.ConclusionID = (DBCommon.IsInt(dr["ConclusionID"]) ? DBCommon.GetInt(dr["ConclusionID"]) : (int?)null);
            personPsychCert.ExpirationDate = (dr["ExpirationDate"] is DateTime) ? (DateTime)dr["ExpirationDate"] : (DateTime?)null;

            return personPsychCert;
        }

        public static PersonPsychCert GetPersonPsychCert(int psychCertID, User currentUser)
        {
            PersonPsychCert personPsychCert = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.PsychCertID,
							          a.PersonID,
							          a.PsychCertDate,
							          a.ProtNum,
	                                  a.ConclusionID,
							          a.ExpirationDate
                               FROM PMIS_ADM.PsychCert a
                               WHERE a.PsychCertID = :PsychCertID
                            ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PsychCertID", OracleType.Number).Value = psychCertID;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["PsychCertID"]))
                        personPsychCert = ExtractPersonPsychCertFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personPsychCert;
        }

        public static List<PersonPsychCert> GetAllPersonPsychCerts(int personID, User currentUser)
        {
            List<PersonPsychCert> personPsychCerts = new List<PersonPsychCert>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.PsychCertID,
							          a.PersonID,
							          a.PsychCertDate,
							          a.ProtNum,
	                                  a.ConclusionID,
							          a.ExpirationDate
                               FROM PMIS_ADM.PsychCert a
                               WHERE a.PersonID = :PersonID 
                               ORDER BY a.PsychCertDate DESC, PsychCertID DESC
                            ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.VarChar).Value = personID;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["PsychCertID"]))
                        personPsychCerts.Add(ExtractPersonPsychCertFromDR(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personPsychCerts;
        }

        public static bool SavePersonPsychCert(PersonPsychCert personPsychCert, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;
            
            string logDescription = "";
            logDescription += "Име: " + personPsychCert.Person.FullName;
            logDescription += "<br />ЕГН: " + personPsychCert.Person.IdentNumber;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                if (personPsychCert.PsychCertID == 0)
                {
                    SQL += @"INSERT INTO PMIS_ADM.PsychCert (PersonID, PsychCertDate, ProtNum, ConclusionID, ExpirationDate)
                             VALUES (:PersonID, :PsychCertDate, :ProtNum, :ConclusionID, :ExpirationDate);

                             SELECT PMIS_ADM.PsychCert_ID_SEQ.currval INTO :PsychCertID FROM dual;

                            ";

                    changeEvent = new ChangeEvent("RES_Reservist_AddPsychCert", logDescription, null, personPsychCert.Person, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_PsychCertDate", "", CommonFunctions.FormatDate(personPsychCert.PsychCertDate), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_PsychCertProtNum", "", personPsychCert.ProtNum, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_PsychCertConclusion", "", personPsychCert.Conclusion != null ? personPsychCert.Conclusion.MilitaryMedicalConclusionName : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_PsychCertExpirationDate", "", CommonFunctions.FormatDate(personPsychCert.ExpirationDate), currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_ADM.PsychCert SET
                               PsychCertDate = :PsychCertDate,
							   ProtNum = :ProtNum,
	                           ConclusionID = :ConclusionID,
							   ExpirationDate = :ExpirationDate
                            WHERE PsychCertID = :PsychCertID;                       

                            ";

                    PersonPsychCert oldPersonPsychCert = GetPersonPsychCert(personPsychCert.PsychCertID, currentUser);

                    logDescription += "<br />Комисия от дата: " + CommonFunctions.FormatDate(personPsychCert.PsychCertDate);

                    changeEvent = new ChangeEvent("RES_Reservist_EditPsychCert", logDescription, null, personPsychCert.Person, currentUser);

                    if (oldPersonPsychCert.PsychCertDate != personPsychCert.PsychCertDate)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_PsychCertDate", CommonFunctions.FormatDate(oldPersonPsychCert.PsychCertDate), CommonFunctions.FormatDate(personPsychCert.PsychCertDate), currentUser));

                    if (oldPersonPsychCert.ProtNum.Trim() != personPsychCert.ProtNum.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_PsychCertProtNum", oldPersonPsychCert.ProtNum, personPsychCert.ProtNum, currentUser));

                    if (oldPersonPsychCert.ConclusionID != personPsychCert.ConclusionID)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_PsychCertConclusion", oldPersonPsychCert.Conclusion != null ? oldPersonPsychCert.Conclusion.MilitaryMedicalConclusionName : "", personPsychCert.Conclusion != null ? personPsychCert.Conclusion.MilitaryMedicalConclusionName : "", currentUser));

                    if (oldPersonPsychCert.ExpirationDate != personPsychCert.ExpirationDate)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_PsychCertExpirationDate", CommonFunctions.FormatDate(oldPersonPsychCert.ExpirationDate), CommonFunctions.FormatDate(personPsychCert.ExpirationDate), currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramPsychCertID = new OracleParameter();
                paramPsychCertID.ParameterName = "PsychCertID";
                paramPsychCertID.OracleType = OracleType.Number;

                if (personPsychCert.PsychCertID != 0)
                {
                    paramPsychCertID.Direction = ParameterDirection.Input;
                    paramPsychCertID.Value = personPsychCert.PsychCertID;
                }
                else
                {
                    paramPsychCertID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramPsychCertID);

                OracleParameter param = null;

                if (personPsychCert.PsychCertID == 0)
                {
                    param = new OracleParameter();
                    param.ParameterName = "PersonID";
                    param.OracleType = OracleType.Number;
                    param.Direction = ParameterDirection.Input;
                    param.Value = personPsychCert.PersonID;
                    cmd.Parameters.Add(param);
                }

                param = new OracleParameter();
                param.ParameterName = "PsychCertDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (personPsychCert.PsychCertDate.HasValue)
                    param.Value = personPsychCert.PsychCertDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ProtNum";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(personPsychCert.ProtNum))
                    param.Value = personPsychCert.ProtNum;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ConclusionID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (personPsychCert.ConclusionID.HasValue)
                    param.Value = personPsychCert.ConclusionID.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ExpirationDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (personPsychCert.ExpirationDate.HasValue)
                    param.Value = personPsychCert.ExpirationDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);


                cmd.ExecuteNonQuery();

                if (personPsychCert.PsychCertID == 0)
                    personPsychCert.PsychCertID = DBCommon.GetInt(paramPsychCertID.Value);

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
                    PersonUtil.SetPersonModified(personPsychCert.Person.PersonId, currentUser);
                }
            }

            return result;
        }

        public static bool DeletePersonPsychCert(int psychCertID, User currentUser, Change changeEntry)
        {
            bool result = false;

            PersonPsychCert oldPersonPsychCert = GetPersonPsychCert(psychCertID, currentUser);

            string logDescription = "";
            logDescription += "Име: " + oldPersonPsychCert.Person.FullName;
            logDescription += "<br />ЕГН: " + oldPersonPsychCert.Person.IdentNumber;

            ChangeEvent changeEvent = new ChangeEvent("RES_Reservist_DeletePsychCert", logDescription, null, oldPersonPsychCert.Person, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_PsychCertDate", CommonFunctions.FormatDate(oldPersonPsychCert.PsychCertDate), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_PsychCertProtNum", oldPersonPsychCert.ProtNum, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_PsychCertConclusion", oldPersonPsychCert.Conclusion != null ? oldPersonPsychCert.Conclusion.MilitaryMedicalConclusionName : "", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_PsychCertExpirationDate", CommonFunctions.FormatDate(oldPersonPsychCert.ExpirationDate), "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" BEGIN
                                  DELETE FROM PMIS_ADM.PsychCert WHERE PsychCertID = :PsychCertID;
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PsychCertID", OracleType.Number).Value = psychCertID;

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
                PersonUtil.SetPersonModified(oldPersonPsychCert.Person.PersonId, currentUser);
            }

            return result;
        }
    }
}
