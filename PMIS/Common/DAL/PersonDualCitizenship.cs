using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;

namespace PMIS.Common
{
    //Represents single person dual citizenship
    public class PersonDualCitizenship : BaseDbObject
    {
        private int dualCitizenshipId;
        private string countryId;
        
        private Country country;

        public int DualCitizenshipId
        {
            get
            {
                return dualCitizenshipId;
            }
            set
            {
                dualCitizenshipId = value;
            }
        }

        public string CountryId
        {
            get
            {
                return countryId;
            }
            set
            {
                countryId = value;
            }
        }

        public Country Country
        {
            get
            {
                //Lazy initialization
                if(country == null && !String.IsNullOrEmpty(countryId))
                    country = CountryUtil.GetCountry(countryId, CurrentUser);

                return country;
            }
            set
            {
                country = value;
            }
        }

        

        public bool CanDelete
        {
            get
            {
                return true;
            }
        }

        public PersonDualCitizenship(User user)
            : base(user)
        {

        }
    }

    public class PersonDualCitizenshipUtil
    {
        private static PersonDualCitizenship ExtractPersonDualCitizenshipFromDR(OracleDataReader dr, User currentUser)
        {
            PersonDualCitizenship personDualCitizenship = new PersonDualCitizenship(currentUser);

            personDualCitizenship.DualCitizenshipId = DBCommon.GetInt(dr["DualCitizenshipID"]);
            personDualCitizenship.CountryId = dr["CountryID"].ToString();

            return personDualCitizenship;
        }

        public static PersonDualCitizenship GetPersonDualCitizenship(string identNumber, string countryId, User currentUser)
        {
            PersonDualCitizenship personDualCitizenship = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.DVGRID as DualCitizenshipID, 
                                      a.DVGR_DJJKOD as CountryID
                               FROM VS_OWNER.VS_DVGR a
                               WHERE a.DVGR_EGNLS = :IdentNumber AND
                                     a.DVGR_DJJKOD = :CountryID ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("IdentNumber", OracleType.VarChar).Value = identNumber;
                cmd.Parameters.Add("CountryID", OracleType.VarChar).Value = countryId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    personDualCitizenship = ExtractPersonDualCitizenshipFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personDualCitizenship;
        }

        public static PersonDualCitizenship GetPersonDualCitizenship(int dualCitizenshipId, User currentUser)
        {
            PersonDualCitizenship personDualCitizenship = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.DVGRID as DualCitizenshipID, 
                                      a.DVGR_DJJKOD as CountryID
                               FROM VS_OWNER.VS_DVGR a
                               WHERE a.DVGRID = :DualCitizenshipId";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("DualCitizenshipId", OracleType.Number).Value = dualCitizenshipId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    personDualCitizenship = ExtractPersonDualCitizenshipFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personDualCitizenship;
        }

        public static List<PersonDualCitizenship> GetAllPersonDualCitizenshipByPersonID(int personId, User currentUser)
        {
            List<PersonDualCitizenship> personDualCitizenships = new List<PersonDualCitizenship>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.DVGRID as DualCitizenshipID, 
                                      a.DVGR_DJJKOD as CountryID
                               FROM VS_OWNER.VS_DVGR a
                               INNER JOIN VS_OWNER.VS_LS b ON a.DVGR_EGNLS = b.EGN
                               WHERE b.PersonID = :PersonID
                               ORDER BY a.DVGRID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Number).Value = personId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    personDualCitizenships.Add(ExtractPersonDualCitizenshipFromDR(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personDualCitizenships;
        }

        public static bool SavePersonDualCitizenship(PersonDualCitizenship personDualCitizenship, Person person, User currentUser, Change changeEntry)
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

                if (personDualCitizenship.DualCitizenshipId == 0)
                {
                    SQL += @"INSERT INTO VS_OWNER.VS_DVGR (DVGR_EGNLS, DVGR_DJJKOD)
                            VALUES (:IdentNumber, :CountryID);

                            SELECT VS_OWNER.VS_DVGR_DVGRID_SEQ.currval INTO :DualCitizenshipID FROM dual;

                            ";

                    changeEvent = new ChangeEvent("RES_Reservist_AddDualCitizenship", "", null, person, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_DualCitizenship_Country", "", personDualCitizenship.Country.CountryName, currentUser));
                }
                else
                {
                    SQL += @"UPDATE VS_OWNER.VS_DVGR SET
                               DVGR_DJJKOD = :CountryID
                            WHERE DVGRID = :DualCitizenshipID ;                       

                            ";

                    PersonDualCitizenship oldPersonDualCitizenship = GetPersonDualCitizenship(personDualCitizenship.DualCitizenshipId, currentUser);

                    string logDescription = "";

                    changeEvent = new ChangeEvent("RES_Reservist_EditDualCitizenship", logDescription, null, person, currentUser);

                    if (oldPersonDualCitizenship.CountryId.Trim() != personDualCitizenship.CountryId.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_DualCitizenship_Country", oldPersonDualCitizenship.Country.CountryName, personDualCitizenship.Country.CountryName, currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramDualCitizenshipID = new OracleParameter();
                paramDualCitizenshipID.ParameterName = "DualCitizenshipID";
                paramDualCitizenshipID.OracleType = OracleType.Number;

                if (personDualCitizenship.DualCitizenshipId != 0)
                {
                    paramDualCitizenshipID.Direction = ParameterDirection.Input;
                    paramDualCitizenshipID.Value = personDualCitizenship.DualCitizenshipId;
                }
                else
                {
                    paramDualCitizenshipID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramDualCitizenshipID);

                OracleParameter param = null;

                if (personDualCitizenship.DualCitizenshipId == 0)
                {
                    param = new OracleParameter();
                    param.ParameterName = "IdentNumber";
                    param.OracleType = OracleType.Number;
                    param.Direction = ParameterDirection.Input;
                    param.Value = person.IdentNumber;
                    cmd.Parameters.Add(param);
                }

                param = new OracleParameter();
                param.ParameterName = "CountryID";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personDualCitizenship.CountryId;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (personDualCitizenship.DualCitizenshipId == 0)
                    personDualCitizenship.DualCitizenshipId = DBCommon.GetInt(paramDualCitizenshipID.Value);

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

        public static bool DeletePersonDualCitizenship(int dualCitizenshipId, Person person, User currentUser, Change changeEntry)
        {
            bool result = false;

            PersonDualCitizenship oldPersonDualCitizenship = GetPersonDualCitizenship(dualCitizenshipId, currentUser);

            ChangeEvent changeEvent = new ChangeEvent("RES_Reservist_DeleteDualCitizenship", "", null, person, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("RES_DualCitizenship_Country", oldPersonDualCitizenship.Country.CountryName, "", currentUser));
            
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" BEGIN
                                  DELETE FROM VS_OWNER.VS_DVGR WHERE DVGRID = :DualCitizenshipID;
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("DualCitizenshipID", OracleType.Number).Value = dualCitizenshipId;

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
