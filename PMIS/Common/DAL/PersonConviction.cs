using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;

namespace PMIS.Common
{
    //Represents single person conviction
    public class PersonConviction : BaseDbObject
    {
        private int convictionId;
        private string convictionCode;
        private string convictionReasonCode;
        private DateTime dateFrom;
        private DateTime? dateTo;

        private Conviction conviction;
        private ConvictionReason convictionReason;

        public int ConvictionId
        {
            get
            {
                return convictionId;
            }
            set
            {
                convictionId = value;
            }
        }

        public string ConvictionCode
        {
            get
            {
                return convictionCode;
            }
            set
            {
                convictionCode = value;
            }
        }

        public string ConvictionReasonCode
        {
            get
            {
                return convictionReasonCode;
            }
            set
            {
                convictionReasonCode = value;
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

        public DateTime? DateTo
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

        public Conviction Conviction
        {
            get
            {
                //Lazy initialization
                if(conviction == null && !String.IsNullOrEmpty(convictionCode))
                    conviction = ConvictionUtil.GetConviction(convictionCode, CurrentUser);

                return conviction;
            }
            set
            {
                conviction = value;
            }
        }

        public ConvictionReason ConvictionReason
        {
            get
            {
                //Lazy initialization
                if(convictionReason == null && !String.IsNullOrEmpty(convictionReasonCode))
                    convictionReason = ConvictionReasonUtil.GetConvictionReason(convictionReasonCode, CurrentUser);

                return convictionReason;
            }
            set
            {
                convictionReason = value;
            }
        }

        

        public bool CanDelete
        {
            get
            {
                return true;
            }
        }

        public PersonConviction(User user)
            : base(user)
        {

        }
    }

    public class PersonConvictionUtil
    {
        private static PersonConviction ExtractPersonConvictionFromDR(OracleDataReader dr, User currentUser)
        {
            PersonConviction personConviction = new PersonConviction(currentUser);

            personConviction.ConvictionId = DBCommon.GetInt(dr["ConvictionID"]);
            personConviction.ConvictionCode = dr["ConvictionCode"].ToString();
            personConviction.ConvictionReasonCode = dr["ConvictionReasonCode"].ToString();
            personConviction.DateFrom = (DateTime)dr["DateFrom"];
            personConviction.DateTo = (dr["DateTo"] is DateTime ? (DateTime)dr["DateTo"] : (DateTime?)null);

            return personConviction;
        }

        public static PersonConviction GetPersonConviction(string identNumber, DateTime dateFrom, User currentUser)
        {
            PersonConviction personConviction = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.SDMID as ConvictionID, 
                                      a.SDM_KOD as ConvictionCode,
                                      a.SDM_PSDKOD as ConvictionReasonCode, 
                                      a.SDM_OT as DateFrom,
                                      a.SDM_DO as DateTo
                               FROM VS_OWNER.VS_SDM a
                               WHERE a.SDM_EGNLS = :IdentNumber AND
                                     a.SDM_OT = :DateFrom ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("IdentNumber", OracleType.VarChar).Value = identNumber;
                cmd.Parameters.Add("DateFrom", OracleType.DateTime).Value = dateFrom;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    personConviction = ExtractPersonConvictionFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personConviction;
        }

        public static PersonConviction GetPersonConviction(int convictionId, User currentUser)
        {
            PersonConviction personConviction = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.SDMID as ConvictionID, 
                                      a.SDM_KOD as ConvictionCode,
                                      a.SDM_PSDKOD as ConvictionReasonCode, 
                                      a.SDM_OT as DateFrom,
                                      a.SDM_DO as DateTo
                               FROM VS_OWNER.VS_SDM a
                               WHERE a.SDMID = :ConvictionId";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ConvictionId", OracleType.Number).Value = convictionId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    personConviction = ExtractPersonConvictionFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personConviction;
        }

        public static List<PersonConviction> GetAllPersonConvictionsByPersonID(int personId, User currentUser)
        {
            List<PersonConviction> personPersonConvictions = new List<PersonConviction>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.SDMID as ConvictionID, 
                                      a.SDM_KOD as ConvictionCode,
                                      a.SDM_PSDKOD as ConvictionReasonCode, 
                                      a.SDM_OT as DateFrom,
                                      a.SDM_DO as DateTo
                               FROM VS_OWNER.VS_SDM a
                               INNER JOIN VS_OWNER.VS_LS b ON a.SDM_EGNLS = b.EGN
                               WHERE b.PersonID = :PersonID
                               ORDER BY a.SDM_OT";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Number).Value = personId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    personPersonConvictions.Add(ExtractPersonConvictionFromDR(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personPersonConvictions;
        }

        public static bool SavePersonConviction(PersonConviction personConviction, Person person, User currentUser, Change changeEntry)
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

                if (personConviction.ConvictionId == 0)
                {
                    SQL += @"INSERT INTO VS_OWNER.VS_SDM (SDM_EGNLS, SDM_KOD, SDM_PSDKOD, SDM_ND, SDM_OT, SDM_DO)
                            VALUES (:IdentNumber, :ConvictionCode, :ConvictionReasonCode, :DocNumber, :DateFrom, :DateTo);

                            SELECT VS_OWNER.VS_SDM_SDMID_SEQ.currval INTO :ConvictionID FROM dual;

                            ";

                    changeEvent = new ChangeEvent("RES_Reservist_AddConviction", "", null, person, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Conviction_Conviction", "", personConviction.Conviction.ConvictionName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Conviction_ConvictionReason", "", (personConviction.ConvictionReason != null ? personConviction.ConvictionReason.ConvictionReasonName : ""), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Conviction_DateFrom", "", CommonFunctions.FormatDate(personConviction.DateFrom), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Conviction_DateTo", "", CommonFunctions.FormatDate(personConviction.DateTo), currentUser));
                }
                else
                {
                    SQL += @"UPDATE VS_OWNER.VS_SDM SET
                               SDM_KOD = :ConvictionCode, 
                               SDM_PSDKOD = :ConvictionReasonCode, 
                               SDM_OT = :DateFrom, 
                               SDM_DO = :DateTo
                            WHERE SDMID = :ConvictionID ;                       

                            ";

                    PersonConviction oldPersonConviction = GetPersonConviction(personConviction.ConvictionId, currentUser);

                    string logDescription = "От дата: " + CommonFunctions.FormatDate(oldPersonConviction.DateFrom) + "; ";

                    changeEvent = new ChangeEvent("RES_Reservist_EditConviction", logDescription, null, person, currentUser);

                    if (oldPersonConviction.ConvictionCode.Trim() != personConviction.ConvictionCode.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Conviction_Conviction", oldPersonConviction.Conviction.ConvictionName, personConviction.Conviction.ConvictionName, currentUser));

                    if (oldPersonConviction.ConvictionReasonCode.Trim() != personConviction.ConvictionReasonCode.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Conviction_ConvictionReason", (oldPersonConviction.ConvictionReason != null ? oldPersonConviction.ConvictionReason.ConvictionReasonName : ""), (personConviction.ConvictionReason != null ? personConviction.ConvictionReason.ConvictionReasonName : ""), currentUser));

                    if (!CommonFunctions.IsEqual(oldPersonConviction.DateFrom, personConviction.DateFrom))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Conviction_DateFrom", CommonFunctions.FormatDate(oldPersonConviction.DateFrom), CommonFunctions.FormatDate(personConviction.DateFrom), currentUser));

                    if (!CommonFunctions.IsEqual(oldPersonConviction.DateTo, personConviction.DateTo))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Conviction_DateTo", CommonFunctions.FormatDate(oldPersonConviction.DateTo), CommonFunctions.FormatDate(personConviction.DateTo), currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramConvictionID = new OracleParameter();
                paramConvictionID.ParameterName = "ConvictionID";
                paramConvictionID.OracleType = OracleType.Number;

                if (personConviction.ConvictionId != 0)
                {
                    paramConvictionID.Direction = ParameterDirection.Input;
                    paramConvictionID.Value = personConviction.ConvictionId;
                }
                else
                {
                    paramConvictionID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramConvictionID);

                OracleParameter param = null;

                if (personConviction.ConvictionId == 0)
                {
                    param = new OracleParameter();
                    param.ParameterName = "IdentNumber";
                    param.OracleType = OracleType.Number;
                    param.Direction = ParameterDirection.Input;
                    param.Value = person.IdentNumber;
                    cmd.Parameters.Add(param);

                    param = new OracleParameter();
                    param.ParameterName = "DocNumber";
                    param.OracleType = OracleType.VarChar;
                    param.Direction = ParameterDirection.Input;
                    param.Value = CommonFunctions.FormatDate(personConviction.DateFrom);
                    cmd.Parameters.Add(param);
                }

                param = new OracleParameter();
                param.ParameterName = "ConvictionCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personConviction.ConvictionCode;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ConvictionReasonCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if(personConviction.ConvictionReason != null)
                    param.Value = personConviction.ConvictionReasonCode;
                else 
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "DateFrom";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                param.Value = personConviction.DateFrom;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "DateTo";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (personConviction.DateTo.HasValue)
                    param.Value = personConviction.DateTo.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (personConviction.ConvictionId == 0)
                    personConviction.ConvictionId = DBCommon.GetInt(paramConvictionID.Value);

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

        public static bool DeletePersonConviction(int convictionId, Person person, User currentUser, Change changeEntry)
        {
            bool result = false;

            PersonConviction oldPersonConviction = GetPersonConviction(convictionId, currentUser);

            ChangeEvent changeEvent = new ChangeEvent("RES_Reservist_DeleteConviction", "", null, person, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("RES_Conviction_Conviction", oldPersonConviction.Conviction.ConvictionName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Conviction_ConvictionReason", (oldPersonConviction.ConvictionReason != null ? oldPersonConviction.ConvictionReason.ConvictionReasonName : ""), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Conviction_DateFrom", CommonFunctions.FormatDate(oldPersonConviction.DateFrom), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Conviction_DateTo", CommonFunctions.FormatDate(oldPersonConviction.DateTo), "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" BEGIN
                                  DELETE FROM VS_OWNER.VS_SDM WHERE SDMID = :ConvictionID;
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ConvictionID", OracleType.Number).Value = convictionId;

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
