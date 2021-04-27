using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;

namespace PMIS.Common
{
    public class PersonScientificTitle : BaseDbObject
    {
        public int PersonScientificTitleId { get; set; }

        public string ScientificTitleKey { get; set; }
        public ScientificTitle scientificTitle;
        public ScientificTitle ScientificTitle
        {
            get
            {
                if (scientificTitle == null)
                {
                    scientificTitle = ScientificTitleUtil.GetScientificTitle(ScientificTitleKey, CurrentUser);
                }
                return scientificTitle;
            }
        }

        public int? PersonScientificTitleYear { get; set; }
        public string PersonScientificTitleNumberProtocol { get; set; }
        public string PersonScientificTitleDesription { get; set; }

        private bool canDelete;
        public bool CanDelete
        {
            get
            {
                return true;
            }
        }

        public PersonScientificTitle(User user)
            : base(user)
        {

        }
    }
    public class PersonScientificTitleUtil
    {
        private static PersonScientificTitle ExtractPersonScientificTitleFromDR(OracleDataReader dr, User currentUser)
        {
            PersonScientificTitle personScientificTitle = new PersonScientificTitle(currentUser);

            personScientificTitle.PersonScientificTitleId = DBCommon.GetInt(dr["PersonScientificTitleId"]);

            personScientificTitle.ScientificTitleKey = dr["ScientificTitleKey"].ToString();
            personScientificTitle.PersonScientificTitleYear = DBCommon.GetInt(dr["PersonScientificTitleYear"]);
            personScientificTitle.PersonScientificTitleNumberProtocol = dr["PersonScientificTitleNumProt"].ToString();
            personScientificTitle.PersonScientificTitleDesription = dr["PersonScientificTitleDes"].ToString();

            return personScientificTitle;
        }

        public static PersonScientificTitle GetPersonScientificTitle(int personScientificTitleId, User currentUser)
        {
            PersonScientificTitle personScientificTitle = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT
                                  NZVID  AS PersonScientificTitleId,
                                  NZV_NZVKOD AS ScientificTitleKey,
                                  NZV_KOGA AS PersonScientificTitleYear,
                                  NZV_NPROT AS PersonScientificTitleNumProt,
                                  NZV_SPTEXT AS PersonScientificTitleDes
                                  FROM VS_OWNER.VS_NZV
                              WHERE NZVID = :PersonScientificTitleId";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonScientificTitleId", OracleType.Number).Value = personScientificTitleId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    personScientificTitle = ExtractPersonScientificTitleFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personScientificTitle;
        }

        //Use this Method accordin PK define in table to chek exist combination of these 3 parameters
        public static PersonScientificTitle GetPersonScientificTitle(string identityNumber, string personScientificTitleNumProt, int personScientificTitleYear, User currentUser)
        {
            PersonScientificTitle personScientificTitle = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT
                                  NZVID  AS PersonScientificTitleId,
                                  NZV_NZVKOD AS ScientificTitleKey,
                                  NZV_KOGA AS PersonScientificTitleYear,
                                  NZV_NPROT AS PersonScientificTitleNumProt,
                                  NZV_SPTEXT AS PersonScientificTitleDes
                                  FROM VS_OWNER.VS_NZV
                                WHERE NZV_EGNLS = :IdentityNumber
                                AND NZV_KOGA = :PersonScientificTitleYear
                                AND NZV_NPROT = :PersonScientificTitleNumProt";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("IdentityNumber", OracleType.VarChar).Value = identityNumber;
                cmd.Parameters.Add("PersonScientificTitleYear", OracleType.Number).Value = personScientificTitleYear;
                cmd.Parameters.Add("PersonScientificTitleNumProt", OracleType.VarChar).Value = personScientificTitleNumProt;
                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    personScientificTitle = ExtractPersonScientificTitleFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personScientificTitle;
        }

        public static List<PersonScientificTitle> GetAllPersonScientificTitleByPersonID(int personId, User currentUser)
        {
            List<PersonScientificTitle> listPersonScientificTitle = new List<PersonScientificTitle>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT
                                  a.NZVID  AS PersonScientificTitleId,
                                  a.NZV_NZVKOD AS ScientificTitleKey,
                                  a.NZV_KOGA AS PersonScientificTitleYear,
                                  a.NZV_NPROT AS PersonScientificTitleNumProt,
                                  a.NZV_SPTEXT AS PersonScientificTitleDes
                                  FROM VS_OWNER.VS_NZV a
                              INNER JOIN VS_OWNER.VS_LS c ON a.NZV_EGNLS = c.EGN
                              WHERE c.PersonID = :PersonID
                              ORDER BY a.NZV_NZVKOD, a.NZV_KOGA, a.NZV_NPROT";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Number).Value = personId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    listPersonScientificTitle.Add(ExtractPersonScientificTitleFromDR(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listPersonScientificTitle;
        }

        public static bool SavePersonScientificTitle(PersonScientificTitle personScientificTitle, Person person, User currentUser, Change changeEntry)
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
                if (personScientificTitle.PersonScientificTitleId == 0)
                {
                    SQL += @"INSERT INTO VS_OWNER.VS_NZV (NZVID, NZV_EGNLS,  NZV_NZVKOD, NZV_KOGA,  NZV_NPROT,  NZV_SPTEXT)
                            VALUES (:PersonScientificTitleId, :IdentNumber, :ScientificTitleKey, :PersonScientificTitleYear, :PersonScientificTitleNumProt, :PersonScientificTitleDes);

                            SELECT VS_OWNER.VS_NZV_NZVID_SEQ.currval INTO :PersonScientificTitleId FROM dual;
                            
                            ";

                    changeEvent = new ChangeEvent("RES_Reservist_EduWork_AddScientificTitle", "", null, person, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_ScnTtl_ScientificTitle", "", personScientificTitle.ScientificTitle.ScientificTitleName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_ScnTtl_Year", "", personScientificTitle.PersonScientificTitleYear.Value.ToString(), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_ScnTtl_NumberProtocol", "", personScientificTitle.PersonScientificTitleNumberProtocol, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_ScnTtl_Desription", "", personScientificTitle.PersonScientificTitleDesription, currentUser));
                }
                else
                {
                    SQL += @"UPDATE VS_OWNER.VS_NZV SET

                               NZV_NZVKOD  = :ScientificTitleKey,
                               NZV_KOGA    = :PersonScientificTitleYear,
                               NZV_NPROT   = :PersonScientificTitleNumProt,
                               NZV_SPTEXT  = :PersonScientificTitleDes

                             WHERE NZVID = :PersonScientificTitleId ; ";



                    PersonScientificTitle oldPersonScientificTitle = GetPersonScientificTitle(personScientificTitle.PersonScientificTitleId, currentUser);

                    string logDescription = "Номер на протокола: " + oldPersonScientificTitle.PersonScientificTitleNumberProtocol + "; " +
                                            "Година на завършване: " + oldPersonScientificTitle.PersonScientificTitleYear.Value.ToString();

                    changeEvent = new ChangeEvent("RES_Reservist_EduWork_EditMltScientificTitle", logDescription, null, person, currentUser);

                    if (oldPersonScientificTitle.ScientificTitleKey != personScientificTitle.ScientificTitleKey)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_ScnTtl_ScientificTitle", oldPersonScientificTitle.ScientificTitle != null ? oldPersonScientificTitle.ScientificTitle.ScientificTitleName : "", personScientificTitle.ScientificTitle != null ? personScientificTitle.ScientificTitle.ScientificTitleName : "", currentUser));

                    if (oldPersonScientificTitle.PersonScientificTitleYear != personScientificTitle.PersonScientificTitleYear)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_ScnTtl_Year", oldPersonScientificTitle.PersonScientificTitleYear.HasValue ? oldPersonScientificTitle.PersonScientificTitleYear.Value.ToString() : "", personScientificTitle.PersonScientificTitleYear.HasValue ? personScientificTitle.PersonScientificTitleYear.Value.ToString() : "", currentUser));

                    if (oldPersonScientificTitle.PersonScientificTitleNumberProtocol != personScientificTitle.PersonScientificTitleNumberProtocol)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_ScnTtl_NumberProtocol", oldPersonScientificTitle.PersonScientificTitleNumberProtocol, personScientificTitle.PersonScientificTitleNumberProtocol, currentUser));

                    if (oldPersonScientificTitle.PersonScientificTitleDesription != personScientificTitle.PersonScientificTitleDesription)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_ScnTtl_Desription", oldPersonScientificTitle.PersonScientificTitleDesription, personScientificTitle.PersonScientificTitleDesription, currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramPersonScientificTitleIdId = new OracleParameter();
                paramPersonScientificTitleIdId.ParameterName = "PersonScientificTitleId";
                paramPersonScientificTitleIdId.OracleType = OracleType.Number;

                if (personScientificTitle.PersonScientificTitleId != 0)
                {
                    paramPersonScientificTitleIdId.Direction = ParameterDirection.Input;
                    paramPersonScientificTitleIdId.Value = personScientificTitle.PersonScientificTitleId;
                }
                else
                {
                    paramPersonScientificTitleIdId.Direction = ParameterDirection.Output;
                }
                cmd.Parameters.Add(paramPersonScientificTitleIdId);


                OracleParameter param = null;

                if (personScientificTitle.PersonScientificTitleId == 0)
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
                    param.ParameterName = "PersonScientificTitleId";
                    param.OracleType = OracleType.VarChar;
                    param.Direction = ParameterDirection.Input;
                    param.Value = personScientificTitle.PersonScientificTitleId;
                    cmd.Parameters.Add(param);
                }

                param = new OracleParameter();
                param.ParameterName = "ScientificTitleKey";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personScientificTitle.ScientificTitleKey;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PersonScientificTitleYear";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = personScientificTitle.PersonScientificTitleYear;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PersonScientificTitleNumProt";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personScientificTitle.PersonScientificTitleNumberProtocol;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "PersonScientificTitleDes";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(personScientificTitle.PersonScientificTitleDesription))
                {
                    param.Value = personScientificTitle.PersonScientificTitleDesription;

                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);


                cmd.ExecuteNonQuery();

                if (personScientificTitle.PersonScientificTitleId == 0)
                    personScientificTitle.PersonScientificTitleId = DBCommon.GetInt(paramPersonScientificTitleIdId.Value);

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

        public static bool DeletePersonScientificTitle(int personScientificTitleId, Person person, User currentUser, Change changeEntry)
        {
            bool result = false;

            PersonScientificTitle oldPersonScientificTitle = GetPersonScientificTitle(personScientificTitleId, currentUser);

            ChangeEvent changeEvent = new ChangeEvent("RES_Reservist_EduWork_DeleteScientificTitle", "", null, person, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_ScnTtl_ScientificTitle", oldPersonScientificTitle.ScientificTitle.ScientificTitleName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_ScnTtl_Year", oldPersonScientificTitle.PersonScientificTitleYear.Value.ToString(), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_ScnTtl_NumberProtocol", oldPersonScientificTitle.PersonScientificTitleNumberProtocol, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_EduWork_ScnTtl_Desription", oldPersonScientificTitle.PersonScientificTitleDesription, "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" BEGIN
                                  DELETE FROM VS_OWNER.VS_NZV WHERE NZVID = :PersonScientificTitleId;
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonScientificTitleId", OracleType.Number).Value = personScientificTitleId;

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
