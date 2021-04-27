using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;

namespace PMIS.Common
{
    //Represents single person med cert
    public class PersonMedCert : BaseDbObject
    {
        private int medCertID;
        private int personID;
        private Person person;
        private DateTime? medCertDate;
        private string protNum;
        private int? conclusionID;
        private MilitaryMedicalConclusion conclusion;
        private int? medRubricID;
        private MedicalRubric medRubric; 
        private DateTime? expirationDate;

        public int MedCertID
        {
            get { return medCertID; }
            set { medCertID = value; }
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

        public DateTime? MedCertDate
        {
            get { return medCertDate; }
            set { medCertDate = value; }
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

        public int? MedRubricID
        {
            get { return medRubricID; }
            set { medRubricID = value; }
        }

        public MedicalRubric MedRubric
        {
            get
            {
                if (medRubric == null && medRubricID.HasValue)
                    medRubric = MedicalRubricUtil.GetMedicalRubric(medRubricID.Value, CurrentUser);

                return medRubric;
            }
            set { medRubric = value; }
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

        public PersonMedCert(User user)
            : base(user)
        {

        }
    }

    public class PersonMedCertUtil
    {
        private static PersonMedCert ExtractPersonMedCertFromDR(OracleDataReader dr, User currentUser)
        {
            PersonMedCert personMedCert = new PersonMedCert(currentUser);

            personMedCert.MedCertID = DBCommon.GetInt(dr["MedCertID"]);
            personMedCert.PersonID = DBCommon.GetInt(dr["PersonID"]);
            personMedCert.MedCertDate = (dr["MedCertDate"] is DateTime) ? (DateTime)dr["MedCertDate"] : (DateTime?)null;
            personMedCert.ProtNum = dr["ProtNum"].ToString();
            personMedCert.ConclusionID = (DBCommon.IsInt(dr["ConclusionID"]) ? DBCommon.GetInt(dr["ConclusionID"]) : (int?)null);
            personMedCert.MedRubricID = (DBCommon.IsInt(dr["MedRubricID"]) ? DBCommon.GetInt(dr["MedRubricID"]) : (int?)null);
            personMedCert.ExpirationDate = (dr["ExpirationDate"] is DateTime) ? (DateTime)dr["ExpirationDate"] : (DateTime?)null;
            
            return personMedCert;
        }

        public static PersonMedCert GetPersonMedCert(int medCertID, User currentUser)
        {
            PersonMedCert personMedCert = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.MedCertID,
							          a.PersonID,
							          a.MedCertDate,
							          a.ProtNum,
	                                  a.ConclusionID,
							          a.MedRubricID,
							          a.ExpirationDate
                               FROM PMIS_ADM.MedCert a
                               WHERE a.MedCertID = :MedCertID
                            ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MedCertID", OracleType.Number).Value = medCertID;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["MedCertID"]))
                        personMedCert = ExtractPersonMedCertFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personMedCert;
        }

        public static List<PersonMedCert> GetAllPersonMedCerts(int personID, User currentUser)
        {
            List<PersonMedCert> personMedCerts = new List<PersonMedCert>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.MedCertID,
							          a.PersonID,
							          a.MedCertDate,
							          a.ProtNum,
	                                  a.ConclusionID,
							          a.MedRubricID,
							          a.ExpirationDate
                               FROM PMIS_ADM.MedCert a
                               WHERE a.PersonID = :PersonID 
                               ORDER BY a.MedCertDate DESC, a.MedCertID DESC
                            ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.VarChar).Value = personID;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["MedCertID"]))
                        personMedCerts.Add(ExtractPersonMedCertFromDR(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personMedCerts;
        }

        public static bool SavePersonMedCert(PersonMedCert personMedCert, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;
            
            string logDescription = "";
            logDescription += "Име: " + personMedCert.Person.FullName;
            logDescription += "<br />ЕГН: " + personMedCert.Person.IdentNumber;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                if (personMedCert.MedCertID == 0)
                {
                    SQL += @"INSERT INTO PMIS_ADM.MedCert (PersonID, MedCertDate, ProtNum, ConclusionID, MedRubricID, ExpirationDate)
                             VALUES (:PersonID, :MedCertDate, :ProtNum, :ConclusionID, :MedRubricID, :ExpirationDate);

                             SELECT PMIS_ADM.MedCert_ID_SEQ.currval INTO :MedCertID FROM dual;

                            ";

                    changeEvent = new ChangeEvent("RES_Reservist_AddMedCert", logDescription, null, personMedCert.Person, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_MedCertDate", "", CommonFunctions.FormatDate(personMedCert.MedCertDate), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_MedCertProtNum", "", personMedCert.ProtNum, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_MedCertConclusion", "", personMedCert.Conclusion != null ? personMedCert.Conclusion.MilitaryMedicalConclusionName : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_MedCertMedRubric", "", personMedCert.MedRubric != null ? personMedCert.MedRubric.MedicalRubricTitle : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_MedCertExpirationDate", "", CommonFunctions.FormatDate(personMedCert.ExpirationDate), currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_ADM.MedCert SET
                               MedCertDate = :MedCertDate,
							   ProtNum = :ProtNum,
	                           ConclusionID = :ConclusionID,
							   MedRubricID = :MedRubricID,
							   ExpirationDate = :ExpirationDate
                            WHERE MedCertID = :MedCertID ;                       

                            ";

                    PersonMedCert oldPersonMedCert = GetPersonMedCert(personMedCert.MedCertID, currentUser);

                    logDescription += "<br />Комисия от дата: " + CommonFunctions.FormatDate(personMedCert.MedCertDate);

                    changeEvent = new ChangeEvent("RES_Reservist_EditMedCert", logDescription, null, personMedCert.Person, currentUser);

                    if (oldPersonMedCert.MedCertDate != personMedCert.MedCertDate)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_MedCertDate", CommonFunctions.FormatDate(oldPersonMedCert.MedCertDate), CommonFunctions.FormatDate(personMedCert.MedCertDate), currentUser));

                    if (oldPersonMedCert.ProtNum.Trim() != personMedCert.ProtNum.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_MedCertProtNum", oldPersonMedCert.ProtNum, personMedCert.ProtNum, currentUser));

                    if (oldPersonMedCert.ConclusionID != personMedCert.ConclusionID)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_MedCertConclusion", oldPersonMedCert.Conclusion != null ? oldPersonMedCert.Conclusion.MilitaryMedicalConclusionName : "", personMedCert.Conclusion != null ? personMedCert.Conclusion.MilitaryMedicalConclusionName : "", currentUser));

                    if (oldPersonMedCert.MedRubricID != personMedCert.MedRubricID)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_MedCertMedRubric", oldPersonMedCert.MedRubric != null ? oldPersonMedCert.MedRubric.MedicalRubricTitle : "", personMedCert.MedRubric != null ? personMedCert.MedRubric.MedicalRubricTitle : "", currentUser));

                    if (oldPersonMedCert.ExpirationDate != personMedCert.ExpirationDate)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_MedCertExpirationDate", CommonFunctions.FormatDate(oldPersonMedCert.ExpirationDate), CommonFunctions.FormatDate(personMedCert.ExpirationDate), currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramMedCertID = new OracleParameter();
                paramMedCertID.ParameterName = "MedCertID";
                paramMedCertID.OracleType = OracleType.Number;

                if (personMedCert.MedCertID != 0)
                {
                    paramMedCertID.Direction = ParameterDirection.Input;
                    paramMedCertID.Value = personMedCert.MedCertID;
                }
                else
                {
                    paramMedCertID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramMedCertID);

                OracleParameter param = null;

                if (personMedCert.MedCertID == 0)
                {
                    param = new OracleParameter();
                    param.ParameterName = "PersonID";
                    param.OracleType = OracleType.Number;
                    param.Direction = ParameterDirection.Input;
                    param.Value = personMedCert.PersonID;
                    cmd.Parameters.Add(param);
                }

                param = new OracleParameter();
                param.ParameterName = "MedCertDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (personMedCert.MedCertDate.HasValue)
                    param.Value = personMedCert.MedCertDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ProtNum";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(personMedCert.ProtNum))
                    param.Value = personMedCert.ProtNum;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ConclusionID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (personMedCert.ConclusionID.HasValue)
                    param.Value = personMedCert.ConclusionID.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);


                param = new OracleParameter();
                param.ParameterName = "MedRubricID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (personMedCert.MedRubricID.HasValue)
                    param.Value = personMedCert.MedRubricID.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ExpirationDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (personMedCert.ExpirationDate.HasValue)
                    param.Value = personMedCert.ExpirationDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);


                cmd.ExecuteNonQuery();

                if (personMedCert.MedCertID == 0)
                    personMedCert.MedCertID = DBCommon.GetInt(paramMedCertID.Value);

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
                    PersonUtil.SetPersonModified(personMedCert.Person.PersonId, currentUser);
                }
            }

            return result;
        }

        public static bool DeletePersonMedCert(int medCertID, User currentUser, Change changeEntry)
        {
            bool result = false;

            PersonMedCert oldPersonMedCert = GetPersonMedCert(medCertID, currentUser);

            string logDescription = "";
            logDescription += "Име: " + oldPersonMedCert.Person.FullName;
            logDescription += "<br />ЕГН: " + oldPersonMedCert.Person.IdentNumber;

            ChangeEvent changeEvent = new ChangeEvent("RES_Reservist_DeleteMedCert", logDescription, null, oldPersonMedCert.Person, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_MedCertDate", CommonFunctions.FormatDate(oldPersonMedCert.MedCertDate), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_MedCertProtNum", oldPersonMedCert.ProtNum, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_MedCertConclusion", oldPersonMedCert.Conclusion != null ? oldPersonMedCert.Conclusion.MilitaryMedicalConclusionName : "", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_MedCertMedRubric", oldPersonMedCert.MedRubric != null ? oldPersonMedCert.MedRubric.MedicalRubricTitle : "", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_MedCertExpirationDate", CommonFunctions.FormatDate(oldPersonMedCert.ExpirationDate), "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" BEGIN
                                  DELETE FROM PMIS_ADM.MedCert WHERE MedCertID = :MedCertID;
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MedCertID", OracleType.Number).Value = medCertID;

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
                PersonUtil.SetPersonModified(oldPersonMedCert.Person.PersonId, currentUser);
            }

            return result;
        }
    }
}
