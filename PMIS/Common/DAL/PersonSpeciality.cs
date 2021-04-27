using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;

namespace PMIS.Common
{
    public class PersonSpeciality : BaseDbObject
    {
        public int PersonSpecialityID { get; set; }
        public int PersonID { get; set; }
        public int ProfessionID { get; set; }
        public int? SpecialityID { get; set; }

        private Profession profession;
        public Profession Profession
        {
            get
            {
                if (profession == null)
                {
                    profession = ProfessionUtil.GetProfession(ProfessionID, CurrentUser);
                }
                return profession;
            }
        }

        private Speciality speciality;
        public Speciality Speciality
        {
            get
            {
                if (speciality == null && SpecialityID.HasValue)
                {
                    speciality = SpecialityUtil.GetSpeciality(SpecialityID.Value, CurrentUser);
                }
                return speciality;
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

        public PersonSpeciality(User user) : base(user) { }
    }

    public class PersonSpecialityUtil 
    {
        public static List<PersonSpeciality> GetAllPersonSpecialities(int pPersonID, User pCurrentUser)
        {
            List<PersonSpeciality> listPersonSpecialities = new List<PersonSpeciality>();

            OracleConnection conn = new OracleConnection(pCurrentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT
                                  a.PersonSpecialityID,
                                  a.ProfessionID,
                                  a.SpecialityID 
                              FROM PMIS_ADM.PersonSpecialities a 
                              LEFT OUTER JOIN PMIS_ADM.Professions b ON a.ProfessionID = b.ProfessionID
                              LEFT OUTER JOIN PMIS_ADM.Specialities c ON a.SpecialityID = c.SpecialityID
                              WHERE a.PersonID = :PersonID
                              ORDER BY b.ProfessionName, c.SpecialityName ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Number).Value = pPersonID;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    listPersonSpecialities.Add(ExtractPersonSpecialityFromDR(dr, pCurrentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listPersonSpecialities;
        }

        public static PersonSpeciality GetPersonSpeciality(int pPersonSpecialityID, User pCurrentUser)
        {
            PersonSpeciality personSpeciality = null;

            OracleConnection conn = new OracleConnection(pCurrentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT 
                                  a.PersonSpecialityID,
                                  a.ProfessionID,
                                  a.SpecialityID 
                              FROM PMIS_ADM.PersonSpecialities a 
                              WHERE a.PersonSpecialityID = :PersonSpecialityID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonSpecialityID", OracleType.Number).Value = pPersonSpecialityID;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    personSpeciality = ExtractPersonSpecialityFromDR(dr, pCurrentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personSpeciality;
        }

        public static PersonSpeciality GetPersonSpeciality(int pPersonID, int pProfessionID, int? pSpecialityID, User pCurrentUser)
        {
            PersonSpeciality personSpeciality = null;

            OracleConnection conn = new OracleConnection(pCurrentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT 
                                  a.PersonSpecialityID,
                                  a.ProfessionID,
                                  a.SpecialityID 
                              FROM PMIS_ADM.PersonSpecialities a 
                              WHERE a.PersonID = :PersonID AND
                                    a.ProfessionID = :ProfessionID AND
                                    NVL(a.SpecialityID, 0) = NVL(:SpecialityID, 0)";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Number).Value = pPersonID;
                cmd.Parameters.Add("ProfessionID", OracleType.Number).Value = pProfessionID;

                OracleParameter param = new OracleParameter();
                param.ParameterName = "SpecialityID";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (pSpecialityID.HasValue)
                    param.Value = pSpecialityID.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);


                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    personSpeciality = ExtractPersonSpecialityFromDR(dr, pCurrentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personSpeciality;
        }
       

        public static bool SavePersonSpeciality(PersonSpeciality pPersonSpeciality, Person pPeron, User pCurrentUser, Change pChangeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

           
            OracleConnection conn = new OracleConnection(pCurrentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                if (pPersonSpeciality.PersonSpecialityID == 0)
                {
                    SQL += @"INSERT INTO PMIS_ADM.PersonSpecialities (
                                    PersonID, 
                                    ProfessionID,
                                    SpecialityID )

                            VALUES (:PersonID, 
                                    :ProfessionID,
                                    :SpecialityID
                                    );

                           SELECT PMIS_ADM.PersonSpec_ID_SEQ.currval INTO :PersonSpecialityID FROM dual;
                            
                            ";

                    string logDescription = "ЕГН: " + pPeron.IdentNumber;
                    changeEvent = new ChangeEvent("RES_Reservist_EduWork_AddSpeciality", logDescription, null, pPeron, pCurrentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_Speciality_Profession", "", pPersonSpeciality.Profession.ProfessionName, pCurrentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_Speciality_Speciality", "", (pPersonSpeciality.Speciality != null ? pPersonSpeciality.Speciality.SpecialityName : ""), pCurrentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_ADM.PersonSpecialities SET
                                  ProfessionID = :ProfessionID,
                                  SpecialityID  = :SpecialityID
                              WHERE PersonSpecialityID = :PersonSpecialityID ; ";

                    PersonSpeciality oldPersonSpeciality = GetPersonSpeciality(pPersonSpeciality.PersonSpecialityID, pCurrentUser);

                    string logDescription = "ЕГН: " + pPeron.IdentNumber;
                    changeEvent = new ChangeEvent("RES_Reservist_EduWork_EditSpeciality", logDescription, null, pPeron, pCurrentUser);

                    if (oldPersonSpeciality.ProfessionID != pPersonSpeciality.ProfessionID)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_Speciality_Profession", oldPersonSpeciality.Profession.ProfessionName, pPersonSpeciality.Profession.ProfessionName, pCurrentUser));

                    if (!Common.CommonFunctions.IsEqualInt(oldPersonSpeciality.SpecialityID, pPersonSpeciality.SpecialityID))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_Speciality_Speciality", (oldPersonSpeciality.Speciality != null ? oldPersonSpeciality.Speciality.SpecialityName : ""),
                                                                                                                                  (pPersonSpeciality.Speciality != null ? pPersonSpeciality.Speciality.SpecialityName : ""), pCurrentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramPersonSpecialityID = new OracleParameter();
                paramPersonSpecialityID.ParameterName = "PersonSpecialityID";
                paramPersonSpecialityID.OracleType = OracleType.Number;

                if (pPersonSpeciality.PersonSpecialityID != 0)
                {
                    paramPersonSpecialityID.Direction = ParameterDirection.Input;
                    paramPersonSpecialityID.Value = pPersonSpeciality.PersonSpecialityID;
                }
                else
                {
                    paramPersonSpecialityID.Direction = ParameterDirection.Output;
                }
                cmd.Parameters.Add(paramPersonSpecialityID);

                OracleParameter param = null;

                if (pPersonSpeciality.PersonSpecialityID == 0)
                {
                    //Insert
                    param = new OracleParameter();
                    param.ParameterName = "PersonID";
                    param.OracleType = OracleType.VarChar;
                    param.Direction = ParameterDirection.Input;
                    param.Value = pPeron.PersonId;
                    cmd.Parameters.Add(param);
                }

                param = new OracleParameter();
                param.ParameterName = "ProfessionID";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = pPersonSpeciality.ProfessionID;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "SpecialityID";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (pPersonSpeciality.SpecialityID.HasValue)
                {
                    param.Value = pPersonSpeciality.SpecialityID.Value;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (pPersonSpeciality.PersonSpecialityID == 0)
                    pPersonSpeciality.PersonSpecialityID = DBCommon.GetInt(paramPersonSpecialityID.Value);

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
                    pChangeEntry.AddEvent(changeEvent);
                    PersonUtil.SetPersonModified(pPeron.PersonId, pCurrentUser);
                }
            }

            return result;
        }

        public static bool DeletePersonSpeciality(int pPersonSpecialityID, Person pPerson, User pCurrentUser, Change pChangeEntry)
        {
            bool result = false;

            PersonSpeciality oldPersonSpeciality = GetPersonSpeciality(pPersonSpecialityID, pCurrentUser);

            string logDescription = "ЕГН: " + pPerson.IdentNumber;
            ChangeEvent changeEvent = new ChangeEvent("RES_Reservist_EduWork_DeleteSpeciality", logDescription, null, pPerson, pCurrentUser);

            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_Speciality_Profession", oldPersonSpeciality.Profession.ProfessionName, "", pCurrentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_Speciality_Speciality", (oldPersonSpeciality.Speciality != null ? oldPersonSpeciality.Speciality.SpecialityName : ""), "", pCurrentUser));

            OracleConnection conn = new OracleConnection(pCurrentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" BEGIN
                                  DELETE FROM PMIS_ADM.PersonSpecialities WHERE PersonSpecialityID = :PersonSpecialityID;
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonSpecialityID", OracleType.Number).Value = pPersonSpecialityID;

                result = cmd.ExecuteNonQuery() == 1;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                pChangeEntry.AddEvent(changeEvent);
                PersonUtil.SetPersonModified(pPerson.PersonId, pCurrentUser);
            }

            return result;
        }

        private static PersonSpeciality ExtractPersonSpecialityFromDR(OracleDataReader pDR, User pCurrentUser)
        {
            PersonSpeciality personSpeciality = new PersonSpeciality(pCurrentUser);

            personSpeciality.PersonSpecialityID = DBCommon.GetInt(pDR["PersonSpecialityID"]);
            personSpeciality.ProfessionID = DBCommon.GetInt(pDR["ProfessionID"]);
            if (DBCommon.IsInt(pDR["SpecialityID"]))
                personSpeciality.SpecialityID = DBCommon.GetInt(pDR["SpecialityID"]);

            return personSpeciality;
        }
    }
}
