using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;

namespace PMIS.Common
{
    //Represents single person position title
    public class PersonPositionTitle : BaseDbObject
    {
        private int personPositionTitleID;
        private int personID;
        private Person person;
        private int positionTitleID;        
        private PositionTitle positionTitle;
        private bool isPrimary;

        public int PersonPositionTitleID
        {
            get { return personPositionTitleID; }
            set { personPositionTitleID = value; }
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

        public int PositionTitleID
        {
            get { return positionTitleID; }
            set { positionTitleID = value; }
        }

        public PositionTitle PositionTitle
        {
            get 
            {
                if (positionTitle == null)
                    positionTitle = PositionTitleUtil.GetPositionTitle(PositionTitleID, CurrentUser);

                return positionTitle; 
            }
            set { positionTitle = value; }
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

        public PersonPositionTitle(User user)
            : base(user)
        {

        }
    }

    public class PersonPositionTitleUtil
    {
        private static PersonPositionTitle ExtractPersonPositionTitleFromDR(OracleDataReader dr, User currentUser)
        {
            PersonPositionTitle personPositionTitle = new PersonPositionTitle(currentUser);

            personPositionTitle.PersonPositionTitleID = DBCommon.GetInt(dr["PersonPositionTitleID"]);
            personPositionTitle.PersonID = DBCommon.GetInt(dr["PersonID"]);
            personPositionTitle.PositionTitleID = DBCommon.GetInt(dr["PositionTitleID"]);
            personPositionTitle.PositionTitle = PositionTitleUtil.ExtractPositionTitleFromDR(currentUser, dr);
            personPositionTitle.IsPrimary = (DBCommon.IsInt(dr["IsPrimary"]) && DBCommon.GetInt(dr["IsPrimary"]) == 1);

            return personPositionTitle;
        }

        public static PersonPositionTitle GetPersonPositionTitle(int personPositionTitleID, User currentUser)
        {
            PersonPositionTitle personPositionTitle = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.PersonPositionTitleID,
                                      a.PersonID,
                                      b.PositionTitleID,
                                      b.PositionTitle,
                                      b.IsActive as PositionTitleActive,
                                      a.IsPrimary
                               FROM PMIS_ADM.PersonPositionTitles a
                               INNER JOIN PMIS_ADM.PositionTitles b ON a.PositionTitleID = b.PositionTitleID
                               WHERE a.PersonPositionTitleID = :PersonPositionTitleID
                            ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonPositionTitleID", OracleType.Number).Value = personPositionTitleID;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["PersonPositionTitleID"]))
                        personPositionTitle = ExtractPersonPositionTitleFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personPositionTitle;
        }

        public static PersonPositionTitle GetPersonPositionTitle(int personID, int positionTitleID, User currentUser)
        {
            PersonPositionTitle personPositionTitle = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.PersonPositionTitleID,
                                      a.PersonID,
                                      b.PositionTitleID,
                                      b.PositionTitle,
                                      b.IsActive as PositionTitleActive,
                                      a.IsPrimary
                               FROM PMIS_ADM.PersonPositionTitles a
                               INNER JOIN PMIS_ADM.PositionTitles b ON a.PositionTitleID = b.PositionTitleID
                               WHERE a.PersonID = :PersonID AND a.PositionTitleID = :PositionTitleID
                            ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.VarChar).Value = personID;
                cmd.Parameters.Add("PositionTitleID", OracleType.VarChar).Value = positionTitleID;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["PersonPositionTitleID"]))
                        personPositionTitle = ExtractPersonPositionTitleFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personPositionTitle;
        }

        public static List<PersonPositionTitle> GetAllPersonPositionTitles(int personID, User currentUser)
        {
            List<PersonPositionTitle> personPositionTitles = new List<PersonPositionTitle>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.PersonPositionTitleID,
                                      a.PersonID,
                                      b.PositionTitleID,
                                      b.PositionTitle,
                                      b.IsActive as PositionTitleActive,
                                      a.IsPrimary
                               FROM PMIS_ADM.PersonPositionTitles a
                               INNER JOIN PMIS_ADM.PositionTitles b ON a.PositionTitleID = b.PositionTitleID
                               WHERE a.PersonID = :PersonID 
                               ORDER BY a.PersonPositionTitleID
                            ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.VarChar).Value = personID;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["PersonPositionTitleID"]))
                        personPositionTitles.Add(ExtractPersonPositionTitleFromDR(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personPositionTitles;
        }

        public static bool SavePersonPositionTitle(PersonPositionTitle personPositionTitle, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;
            
            string logDescription = "";
            logDescription += "Име: " + personPositionTitle.Person.FullName;
            logDescription += "<br />ЕГН: " + personPositionTitle.Person.IdentNumber;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                if (personPositionTitle.IsPrimary)
                {
                    SQL += @"UPDATE PMIS_ADM.PersonPositionTitles SET
                                IsPrimary = 0
                             WHERE PersonID = :PersonID;
                            ";
                }

                if (personPositionTitle.PersonPositionTitleID == 0)
                {
                    SQL += @"INSERT INTO PMIS_ADM.PersonPositionTitles (PersonID, PositionTitleID, IsPrimary)
                            VALUES (:PersonID, :PositionTitleID, :IsPrimary);

                            SELECT PMIS_ADM.PersonPositionTitles_ID_SEQ.currval INTO :PersonPositionTitleID FROM dual;

                            ";

                    changeEvent = new ChangeEvent("RES_Reservist_AddPositionTitle", logDescription, null, personPositionTitle.Person, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_PositionTitles_PositionTitle", "", personPositionTitle.PositionTitle.PositionTitleName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_PositionTitles_IsPrimary", "", (personPositionTitle.IsPrimary ? "1" : "0"), currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_ADM.PersonPositionTitles SET
                               PositionTitleID = :PositionTitleID,
                               IsPrimary = :IsPrimary
                            WHERE PersonPositionTitleID = :PersonPositionTitleID ;                       

                            ";

                    PersonPositionTitle oldPersonPositionTitle = GetPersonPositionTitle(personPositionTitle.PersonPositionTitleID, currentUser);

                    if (oldPersonPositionTitle.PositionTitle.PositionTitleName.Trim() == personPositionTitle.PositionTitle.PositionTitleName.Trim() &&
                        oldPersonPositionTitle.IsPrimary != personPositionTitle.IsPrimary)
                    {
                        logDescription += "<br />Длъжност: " + personPositionTitle.PositionTitle.PositionTitleName;
                    }

                    changeEvent = new ChangeEvent("RES_Reservist_EditPositionTitle", logDescription, null, personPositionTitle.Person, currentUser);

                    if (oldPersonPositionTitle.PositionTitle.PositionTitleName.Trim() != personPositionTitle.PositionTitle.PositionTitleName.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_PositionTitles_PositionTitle", oldPersonPositionTitle.PositionTitle.PositionTitleName, personPositionTitle.PositionTitle.PositionTitleName, currentUser));

                    if (oldPersonPositionTitle.IsPrimary != personPositionTitle.IsPrimary)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_PositionTitles_IsPrimary", (oldPersonPositionTitle.IsPrimary ? "1" : "0"), (personPositionTitle.IsPrimary ? "1" : "0"), currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramPersonPositionTitleID = new OracleParameter();
                paramPersonPositionTitleID.ParameterName = "PersonPositionTitleID";
                paramPersonPositionTitleID.OracleType = OracleType.Number;

                if (personPositionTitle.PersonPositionTitleID != 0)
                {
                    paramPersonPositionTitleID.Direction = ParameterDirection.Input;
                    paramPersonPositionTitleID.Value = personPositionTitle.PersonPositionTitleID;
                }
                else
                {
                    paramPersonPositionTitleID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramPersonPositionTitleID);

                OracleParameter param = null;

                if (personPositionTitle.PersonPositionTitleID == 0 ||
                    personPositionTitle.IsPrimary)
                {
                    param = new OracleParameter();
                    param.ParameterName = "PersonID";
                    param.OracleType = OracleType.Number;
                    param.Direction = ParameterDirection.Input;
                    param.Value = personPositionTitle.PersonID;
                    cmd.Parameters.Add(param);
                }

                param = new OracleParameter();
                param.ParameterName = "PositionTitleID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = personPositionTitle.PositionTitleID;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "IsPrimary";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = (personPositionTitle.IsPrimary ? 1 : 0);
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (personPositionTitle.PersonPositionTitleID == 0)
                    personPositionTitle.PersonPositionTitleID = DBCommon.GetInt(paramPersonPositionTitleID.Value);

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
                    PersonUtil.SetPersonModified(personPositionTitle.Person.PersonId, currentUser);
                }
            }

            return result;
        }

        public static bool DeletePersonPositionTitle(int personPositionTitleID, User currentUser, Change changeEntry)
        {
            bool result = false;

            PersonPositionTitle oldPersonPositionTitle = GetPersonPositionTitle(personPositionTitleID, currentUser);

            string logDescription = "";
            logDescription += "Име: " + oldPersonPositionTitle.Person.FullName;
            logDescription += "<br />ЕГН: " + oldPersonPositionTitle.Person.IdentNumber;

            ChangeEvent changeEvent = new ChangeEvent("RES_Reservist_DeletePositionTitle", logDescription, null, oldPersonPositionTitle.Person, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_PositionTitles_PositionTitle", oldPersonPositionTitle.PositionTitle.PositionTitleName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_PositionTitles_IsPrimary", (oldPersonPositionTitle.IsPrimary ? "1" : "0"), "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" BEGIN
                                  DELETE FROM PMIS_ADM.PersonPositionTitles WHERE PersonPositionTitleID = :PersonPositionTitleID;
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonPositionTitleID", OracleType.Number).Value = personPositionTitleID;

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
                PersonUtil.SetPersonModified(oldPersonPositionTitle.Person.PersonId, currentUser);
            }

            return result;
        }
    }
}
