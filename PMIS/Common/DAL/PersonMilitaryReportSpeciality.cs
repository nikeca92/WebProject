using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;

namespace PMIS.Common
{
    //Represents single person military report speciality
    public class PersonMilitaryReportSpeciality : BaseDbObject
    {
        private int personMilRepSpecID;
        private int personID;
        private Person person;
        private int militaryReportSpecialityID;        
        private MilitaryReportSpeciality militaryReportSpeciality;
        private bool isPrimary;

        public int PersonMilRepSpecID
        {
            get { return personMilRepSpecID; }
            set { personMilRepSpecID = value; }
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

        public int MilitaryReportSpecialityID
        {
            get { return militaryReportSpecialityID; }
            set { militaryReportSpecialityID = value; }
        }

        public MilitaryReportSpeciality MilitaryReportSpeciality
        {
            get 
            {
                if (militaryReportSpeciality == null)
                    militaryReportSpeciality = MilitaryReportSpecialityUtil.GetMilitaryReportSpeciality(militaryReportSpecialityID, CurrentUser);

                return militaryReportSpeciality; 
            }
            set { militaryReportSpeciality = value; }
        }

        public bool IsPrimary
        {
            get { return isPrimary; }
            set { isPrimary = value; }
        }

        public bool CanDelete
        {
            get
            {
                return true;
            }
        }

        public PersonMilitaryReportSpeciality(User user)
            : base(user)
        {

        }
    }

    public class PersonMilitaryReportSpecialityUtil
    {
        private static PersonMilitaryReportSpeciality ExtractPersonMilitaryReportSpecialityFromDR(OracleDataReader dr, User currentUser)
        {
            PersonMilitaryReportSpeciality personMilitaryReportSpeciality = new PersonMilitaryReportSpeciality(currentUser);

            personMilitaryReportSpeciality.PersonMilRepSpecID = DBCommon.GetInt(dr["PersonMilRepSpecID"]);
            personMilitaryReportSpeciality.PersonID = DBCommon.GetInt(dr["PersonID"]);
            personMilitaryReportSpeciality.MilitaryReportSpecialityID = DBCommon.GetInt(dr["MilReportSpecialityID"]);
            personMilitaryReportSpeciality.MilitaryReportSpeciality = MilitaryReportSpecialityUtil.ExtractMilitaryReportSpecialityFromDR(currentUser, dr);
            personMilitaryReportSpeciality.IsPrimary = (DBCommon.IsInt(dr["IsPrimary"]) && DBCommon.GetInt(dr["IsPrimary"]) == 1);

            return personMilitaryReportSpeciality;
        }

        public static PersonMilitaryReportSpeciality GetPersonMilitaryReportSpeciality(int personMilRepSpecID, User currentUser)
        {
            PersonMilitaryReportSpeciality personMilitaryReportSpeciality = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.PersonMilRepSpecID,
                                      a.PersonID,
                                      b.MilReportSpecialityID,
                                      b.Type,
                                      b.MilReportingSpecialityName,
                                      b.MilReportingSpecialityCode, 
                                      b.Active as MilReportingSpecialityActive,
                                      b.MilitaryForceSortID,
                                      a.IsPrimary
                               FROM PMIS_ADM.PersonMilRepSpec a
                               INNER JOIN PMIS_ADM.MilitaryReportSpecialities b ON a.MilReportSpecialityID = b.MilReportSpecialityID
                               WHERE a.PersonMilRepSpecID = :PersonMilRepSpecID
                            ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonMilRepSpecID", OracleType.Number).Value = personMilRepSpecID;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["PersonMilRepSpecID"]))
                        personMilitaryReportSpeciality = ExtractPersonMilitaryReportSpecialityFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personMilitaryReportSpeciality;
        }       
               
        public static PersonMilitaryReportSpeciality GetPersonMilitaryReportSpeciality(int personID, int militaryReportSpecialityID, User currentUser)
        {
            PersonMilitaryReportSpeciality personMilitaryReportSpeciality = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.PersonMilRepSpecID,
                                      a.PersonID,
                                      b.MilReportSpecialityID,
                                      b.Type,
                                      b.MilReportingSpecialityName,
                                      b.MilReportingSpecialityCode,
                                      b.Active as MilReportingSpecialityActive,
                                      b.MilitaryForceSortID,
                                      a.IsPrimary
                               FROM PMIS_ADM.PersonMilRepSpec a
                               INNER JOIN PMIS_ADM.MilitaryReportSpecialities b ON a.MilReportSpecialityID = b.MilReportSpecialityID
                               WHERE a.PersonID = :PersonID AND a.MilReportSpecialityID = :MilReportSpecialityID
                            ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.VarChar).Value = personID;
                cmd.Parameters.Add("MilReportSpecialityID", OracleType.VarChar).Value = militaryReportSpecialityID;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["PersonMilRepSpecID"]))
                        personMilitaryReportSpeciality = ExtractPersonMilitaryReportSpecialityFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personMilitaryReportSpeciality;
        }       

        public static List<PersonMilitaryReportSpeciality> GetAllPersonMilitaryReportSpecialities(int personID, User currentUser)
        {
            List<PersonMilitaryReportSpeciality> personMilitaryReportSpecialities = new List<PersonMilitaryReportSpeciality>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.PersonMilRepSpecID,
                                      a.PersonID,
                                      b.MilReportSpecialityID,
                                      b.Type,
                                      b.MilReportingSpecialityName,
                                      b.MilReportingSpecialityCode,
                                      b.Active as MilReportingSpecialityActive,
                                      b.MilitaryForceSortID,
                                      a.IsPrimary
                               FROM PMIS_ADM.PersonMilRepSpec a
                               INNER JOIN PMIS_ADM.MilitaryReportSpecialities b ON a.MilReportSpecialityID = b.MilReportSpecialityID
                               WHERE a.PersonID = :PersonID 
                            ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.VarChar).Value = personID;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["PersonMilRepSpecID"]))
                        personMilitaryReportSpecialities.Add(ExtractPersonMilitaryReportSpecialityFromDR(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personMilitaryReportSpecialities;
        }

        public static bool SavePersonMilitaryReportSpeciality(PersonMilitaryReportSpeciality personMilitaryReportSpeciality, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;
            
            string logDescription = "";
            logDescription += "Име: " + personMilitaryReportSpeciality.Person.FullName;
            logDescription += "<br />ЕГН: " + personMilitaryReportSpeciality.Person.IdentNumber;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                if (personMilitaryReportSpeciality.IsPrimary)
                {
                    SQL += @"UPDATE PMIS_ADM.PersonMilRepSpec SET
                                IsPrimary = 0
                             WHERE PersonID = :PersonID;
                            ";
                }

                if (personMilitaryReportSpeciality.PersonMilRepSpecID == 0)
                {
                    SQL += @"INSERT INTO PMIS_ADM.PersonMilRepSpec (PersonID, MilReportSpecialityID, IsPrimary)
                            VALUES (:PersonID, :MilReportSpecialityID, :IsPrimary);

                            SELECT PMIS_ADM.PersonMilRepSpec_ID_SEQ.currval INTO :PersonMilRepSpecID FROM dual;

                            ";

                    changeEvent = new ChangeEvent("RES_Reservist_AddMilRepSpec", logDescription, null, personMilitaryReportSpeciality.Person, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepSpec_MilRepSpec", "", personMilitaryReportSpeciality.MilitaryReportSpeciality.CodeAndName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepSpec_IsPrimary", "", (personMilitaryReportSpeciality.IsPrimary ? "1" : "0"), currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_ADM.PersonMilRepSpec SET
                               MilReportSpecialityID = :MilReportSpecialityID,
                               IsPrimary = :IsPrimary
                            WHERE PersonMilRepSpecID = :PersonMilRepSpecID ;                       

                            ";

                    PersonMilitaryReportSpeciality oldPersonMilitaryReportSpeciality = GetPersonMilitaryReportSpeciality(personMilitaryReportSpeciality.PersonMilRepSpecID, currentUser);

                    if (oldPersonMilitaryReportSpeciality.MilitaryReportSpeciality.CodeAndName.Trim() == personMilitaryReportSpeciality.MilitaryReportSpeciality.CodeAndName.Trim() &&
                        oldPersonMilitaryReportSpeciality.IsPrimary != personMilitaryReportSpeciality.IsPrimary)
                    {
                        logDescription += "<br />ВОС: " + personMilitaryReportSpeciality.MilitaryReportSpeciality.CodeAndName;
                    }

                    changeEvent = new ChangeEvent("RES_Reservist_EditMilRepSpec", logDescription, null, personMilitaryReportSpeciality.Person, currentUser);

                    if (oldPersonMilitaryReportSpeciality.MilitaryReportSpeciality.CodeAndName.Trim() != personMilitaryReportSpeciality.MilitaryReportSpeciality.CodeAndName.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepSpec_MilRepSpec", oldPersonMilitaryReportSpeciality.MilitaryReportSpeciality.CodeAndName, personMilitaryReportSpeciality.MilitaryReportSpeciality.CodeAndName, currentUser));

                    if (oldPersonMilitaryReportSpeciality.IsPrimary != personMilitaryReportSpeciality.IsPrimary)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepSpec_IsPrimary", (oldPersonMilitaryReportSpeciality.IsPrimary ? "1" : "0"), (personMilitaryReportSpeciality.IsPrimary ? "1" : "0"), currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramPersonMilRepSpecID = new OracleParameter();
                paramPersonMilRepSpecID.ParameterName = "PersonMilRepSpecID";
                paramPersonMilRepSpecID.OracleType = OracleType.Number;

                if (personMilitaryReportSpeciality.PersonMilRepSpecID != 0)
                {
                    paramPersonMilRepSpecID.Direction = ParameterDirection.Input;
                    paramPersonMilRepSpecID.Value = personMilitaryReportSpeciality.PersonMilRepSpecID;
                }
                else
                {
                    paramPersonMilRepSpecID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramPersonMilRepSpecID);

                OracleParameter param = null;

                if (personMilitaryReportSpeciality.PersonMilRepSpecID == 0 ||
                    personMilitaryReportSpeciality.IsPrimary)
                {
                    param = new OracleParameter();
                    param.ParameterName = "PersonID";
                    param.OracleType = OracleType.Number;
                    param.Direction = ParameterDirection.Input;
                    param.Value = personMilitaryReportSpeciality.PersonID;
                    cmd.Parameters.Add(param);
                }

                param = new OracleParameter();
                param.ParameterName = "MilReportSpecialityID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = personMilitaryReportSpeciality.MilitaryReportSpecialityID;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "IsPrimary";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = (personMilitaryReportSpeciality.IsPrimary ? 1 : 0);
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (personMilitaryReportSpeciality.PersonMilRepSpecID == 0)
                    personMilitaryReportSpeciality.PersonMilRepSpecID = DBCommon.GetInt(paramPersonMilRepSpecID.Value);

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
                    PersonUtil.SetPersonModified(personMilitaryReportSpeciality.Person.PersonId, currentUser);
                }
            }

            return result;
        }

        public static bool DeletePersonMilitaryReportSpeciality(int personMilRepSpecID, User currentUser, Change changeEntry)
        {
            bool result = false;

            PersonMilitaryReportSpeciality oldPersonMilitaryReportSpeciality = GetPersonMilitaryReportSpeciality(personMilRepSpecID, currentUser);

            string logDescription = "";
            logDescription += "Име: " + oldPersonMilitaryReportSpeciality.Person.FullName;
            logDescription += "<br />ЕГН: " + oldPersonMilitaryReportSpeciality.Person.IdentNumber;

            ChangeEvent changeEvent = new ChangeEvent("RES_Reservist_DeleteMilRepSpec", logDescription, null, oldPersonMilitaryReportSpeciality.Person, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepSpec_MilRepSpec", oldPersonMilitaryReportSpeciality.MilitaryReportSpeciality.CodeAndName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepSpec_IsPrimary", (oldPersonMilitaryReportSpeciality.IsPrimary ? "1" : "0"), "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" BEGIN
                                  DELETE FROM PMIS_ADM.PersonMilRepSpec WHERE PersonMilRepSpecID = :PersonMilRepSpecID;
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonMilRepSpecID", OracleType.Number).Value = personMilRepSpecID;

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
                PersonUtil.SetPersonModified(oldPersonMilitaryReportSpeciality.Person.PersonId, currentUser);
            }

            return result;
        }
    }
}
