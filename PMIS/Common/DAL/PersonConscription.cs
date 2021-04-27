using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;

namespace PMIS.Common
{
    public class PersonConscription : BaseDbObject
    {
        private int personConscriptionId;
        private int personId;
        private string militaryUnit;
        private string position;
        private DateTime dateFrom;
        private DateTime dateTo;

        public int PersonConscriptionId
        {
            get
            {
                return personConscriptionId;
            }
            set
            {
                personConscriptionId = value;
            }
        }

        public int PersonId
        {
            get
            {
                return personId;
            }
            set
            {
                personId = value;
            }
        }

        public string MilitaryUnit
        {
            get
            {
                return militaryUnit;
            }
            set
            {
                militaryUnit = value;
            }
        }

        public string Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        public DateTime DateFrom
        {
            get
            {
                return dateFrom;
            }
            set
            {
                dateFrom = value;
            }
        }

        public DateTime DateTo
        {
            get
            {
                return dateTo;
            }
            set
            {
                dateTo = value;
            }
        }       

        public bool CanDelete
        {
            get
            {
                return true;
            }
        }

        public PersonConscription(User user)
            : base(user)
        {

        }
    }
    public class PersonConscriptionUtil
    {

        private static PersonConscription ExtractPersonConscriptionFromDR(OracleDataReader dr, User currentUser)
        {
            PersonConscription personConscription = new PersonConscription(currentUser);

            personConscription.PersonConscriptionId = DBCommon.GetInt(dr["PersonConscriptionID"]);
            personConscription.PersonId = DBCommon.GetInt(dr["PersonID"]);
            personConscription.MilitaryUnit = dr["MilitaryUnit"].ToString();
            personConscription.Position = dr["Position"].ToString();
            personConscription.DateFrom = (DateTime)(dr["DateFrom"]);
            personConscription.DateTo = (DateTime)(dr["DateTo"]);

            return personConscription;
        }

        public static PersonConscription GetPersonConscription(int personConscriptionId, User currentUser)
        {
            PersonConscription PersonConscription = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT 
                                  a.PersonConscriptionID,
                                  a.PersonID,
                                  a.MilitaryUnit,
                                  a.Position,
                                  a.DateFrom,
                                  a.DateTo
                              FROM PMIS_RES.PersonConscription a 
                              WHERE a.PersonConscriptionID = :PersonConscriptionID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonConscriptionID", OracleType.Number).Value = personConscriptionId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    PersonConscription = ExtractPersonConscriptionFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return PersonConscription;
        }

        public static PersonConscription GetPersonConscription(int personId, DateTime dateFrom, User currentUser)
        {
            PersonConscription PersonConscription = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT 
                                  a.PersonConscriptionID,
                                  a.PersonID,
                                  a.MilitaryUnit,
                                  a.Position,
                                  a.DateFrom,
                                  a.DateTo
                              FROM PMIS_RES.PersonConscription a 
                              WHERE a.PersonID = :PersonID 
                                AND a.DateFrom = :DateFrom";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Number).Value = personId;
                cmd.Parameters.Add("DateFrom", OracleType.DateTime).Value = dateFrom;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    PersonConscription = ExtractPersonConscriptionFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return PersonConscription;
        }

        public static List<PersonConscription> GetAllPersonConscriptionByPersonID(int personId, User currentUser)
        {
            List<PersonConscription> listPersonConscription = new List<PersonConscription>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT 
                                  a.PersonConscriptionID,
                                  a.PersonID,
                                  a.MilitaryUnit,
                                  a.Position,
                                  a.DateFrom,
                                  a.DateTo
                              FROM PMIS_RES.PersonConscription a
                               WHERE a.PersonID = :PersonID
                               ORDER BY a.DateFrom, a.DateTo";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Number).Value = personId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    listPersonConscription.Add(ExtractPersonConscriptionFromDR(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listPersonConscription;
        }

        public static bool SavePersonConscription(PersonConscription personConscription, Person person, User currentUser, Change changeEntry)
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

                if (personConscription.PersonConscriptionId == 0)
                {
                    SQL += @"INSERT INTO PMIS_RES.PersonConscription (
                                    PersonID,
                                    MilitaryUnit,
                                    Position,
                                    DateFrom,
                                    DateTo  )

                            VALUES (
                                    :PersonID, 
                                    :MilitaryUnit,
                                    :Position,
                                    :DateFrom,
                                    :DateTo
                                    );

                           SELECT PMIS_RES.PersonConscription_ID_SEQ.currval INTO :PersonConscriptionId FROM dual;
                            
                            ";

                    changeEvent = new ChangeEvent("RES_Reservist_MilServ_AddConscription", "", null, person, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Conscription_MilitaryUnit", "", personConscription.MilitaryUnit, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Conscription_Position", "", personConscription.Position, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Conscription_DateFrom", "", CommonFunctions.FormatDate(personConscription.DateFrom), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Conscription_DateTo", "", CommonFunctions.FormatDate(personConscription.DateTo), currentUser));

                }
                else
                {
                    SQL += @"UPDATE PMIS_RES.PersonConscription SET

                                    MilitaryUnit = :MilitaryUnit,
                                    Position    = :Position,
                                    DateFrom   = :DateFrom,
                                    DateTo   = :DateTo

                              WHERE PersonConscriptionID = :PersonConscriptionId ; ";


                    PersonConscription oldPersonConscription = GetPersonConscription(personConscription.PersonConscriptionId, currentUser);

                    string logDescription = "Наборна служба - от дата " + CommonFunctions.FormatDate(oldPersonConscription.DateFrom) + " " +
                                            "до дата " + CommonFunctions.FormatDate(oldPersonConscription.DateTo);

                    changeEvent = new ChangeEvent("RES_Reservist_MilServ_EditConscription", logDescription, null, person, currentUser);

                    if (oldPersonConscription.MilitaryUnit != personConscription.MilitaryUnit)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Conscription_MilitaryUnit", oldPersonConscription.MilitaryUnit, personConscription.MilitaryUnit, currentUser));
                    if (oldPersonConscription.Position != personConscription.Position)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Conscription_Position", oldPersonConscription.Position, personConscription.Position, currentUser));
                    if (oldPersonConscription.DateFrom != personConscription.DateFrom)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Conscription_DateFrom", CommonFunctions.FormatDate(oldPersonConscription.DateFrom), CommonFunctions.FormatDate(personConscription.DateFrom), currentUser));
                    if (oldPersonConscription.DateTo != personConscription.DateTo)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Conscription_DateTo", CommonFunctions.FormatDate(oldPersonConscription.DateTo), CommonFunctions.FormatDate(personConscription.DateTo), currentUser));

                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramPersonConscriptionId = new OracleParameter();
                paramPersonConscriptionId.ParameterName = "PersonConscriptionId";
                paramPersonConscriptionId.OracleType = OracleType.Number;

                if (personConscription.PersonConscriptionId != 0)
                {
                    paramPersonConscriptionId.Direction = ParameterDirection.Input;
                    paramPersonConscriptionId.Value = personConscription.PersonConscriptionId;
                }
                else
                {
                    paramPersonConscriptionId.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramPersonConscriptionId);

                OracleParameter param = null;

                if (personConscription.PersonConscriptionId == 0)
                {
                    //Insert
                    param = new OracleParameter();
                    param.ParameterName = "PersonID";
                    param.OracleType = OracleType.Number;
                    param.Direction = ParameterDirection.Input;
                    param.Value = person.PersonId;
                    cmd.Parameters.Add(param);
                }

                param = new OracleParameter();
                param.ParameterName = "MilitaryUnit";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personConscription.MilitaryUnit;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Position";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personConscription.Position;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "DateFrom";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                param.Value = personConscription.DateFrom;
                cmd.Parameters.Add(param);


                param = new OracleParameter();
                param.ParameterName = "DateTo";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                param.Value = personConscription.DateTo;
                cmd.Parameters.Add(param);



                cmd.ExecuteNonQuery();

                if (personConscription.PersonConscriptionId == 0)
                    personConscription.PersonConscriptionId = DBCommon.GetInt(paramPersonConscriptionId.Value);

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


        public static bool DeletePersonConscription(int personConscriptionId, Person person, User currentUser, Change changeEntry)
        {
            bool result = false;

            PersonConscription oldPersonConscription = GetPersonConscription(personConscriptionId, currentUser);

            ChangeEvent changeEvent = new ChangeEvent("RES_Reservist_MilServ_DeleteConscription", "", null, person, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Conscription_MilitaryUnit", oldPersonConscription.MilitaryUnit, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Conscription_Position", oldPersonConscription.Position, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Conscription_DateFrom", CommonFunctions.FormatDate(oldPersonConscription.DateFrom), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Conscription_DateTo", CommonFunctions.FormatDate(oldPersonConscription.DateTo), "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" BEGIN
                                  DELETE FROM PMIS_RES.PersonConscription WHERE PersonConscriptionID = :PersonConscriptionId;
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonConscriptionId", OracleType.Number).Value = personConscriptionId;

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
