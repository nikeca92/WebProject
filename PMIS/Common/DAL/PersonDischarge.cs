using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;

namespace PMIS.Common
{
    public class PersonDischarge : BaseDbObject
    {
        public int PersonDischargeId { get; set; }
        public int PersonId { get; set; }

        //Година - SNOK_GDNA
        public int Year { get; set; }

        //Причина за освобождаване от ВС - SNOK_SNOKOD -> VS_OWNER.KLV_SNO.SNO_KOD
        public string DischargeReasonCode { get; set; }
        private DischargeReason dischargeReason;
        public DischargeReason DischargeReason
        {
            get
            {
                if (dischargeReason == null && !string.IsNullOrEmpty(DischargeReasonCode))
                {
                    dischargeReason = DischargeReasonUtil.GetDischargeReason(DischargeReasonCode, CurrentUser);
                }
                return dischargeReason;
            }
        }

        //Къде отива при снемане от отчет - SNOK_PSOKOD -> VS_OWNER.KLV_PSO.PSO_KOD
        public string DischargeDestinationCode { get; set; }
        private DischargeDestination dischargeDestination;
        public DischargeDestination DischargeDestination
        {
            get
            {
                if (dischargeDestination == null && !string.IsNullOrEmpty(DischargeDestinationCode))
                {
                    dischargeDestination = DischargeDestinationUtil.GetDischargeDestination(DischargeDestinationCode, CurrentUser);
                }
                return dischargeDestination;
            }
        }

        //Заповед - SNOK_ZPVD
        public string Order { get; set; }

        //Дата - SNOK_KOGA
        public DateTime? OrderDate { get; set; }

        //Считано от - SNOK_OT_KOGA
        public DateTime? OrderEffectiveDate { get; set; }

        //Подписал заповедта - SNOK_SPZKOD -> VS_OWNER.KLV_SPZ.SPZ_KOD
        public string MilitaryCommanderRankCode { get; set; }
        public MilitaryCommanderRank militaryCommanderRank;
        public MilitaryCommanderRank MilitaryCommanderRank
        {
            get
            {
                if (militaryCommanderRank == null)
                {
                    militaryCommanderRank = MilitaryCommanderRankUtil.GetMilitaryCommanderRank(MilitaryCommanderRankCode, CurrentUser);
                }
                return militaryCommanderRank;
            }
        }

        public bool CanDelete
        {
            get
            {
                return true;
            }
        }

        public PersonDischarge(User user)
            : base(user)
        {

        }
    }
    public class PersonDischargeUtil
    {

        private static PersonDischarge ExtractPersonDischargeFromDR(OracleDataReader dr, User currentUser)
        {
            PersonDischarge personDischarge = new PersonDischarge(currentUser);

            personDischarge.PersonDischargeId = DBCommon.GetInt(dr["PersonDischargeID"]);
            personDischarge.PersonId = DBCommon.GetInt(dr["PersonID"]);

            personDischarge.Year = DBCommon.GetInt(dr["Year"]);
            personDischarge.DischargeReasonCode = dr["DischargeReasonCode"].ToString();
            personDischarge.DischargeDestinationCode = dr["DischargeDestinationCode"].ToString();
            personDischarge.Order = dr["Order"].ToString();
            personDischarge.OrderDate = (dr["OrderDate"] is DateTime ? (DateTime)dr["OrderDate"] : (DateTime?)null);
            personDischarge.OrderEffectiveDate = (dr["OrderEffectiveDate"] is DateTime ? (DateTime)dr["OrderEffectiveDate"] : (DateTime?)null);
            personDischarge.MilitaryCommanderRankCode = dr["MilitaryCommanderRankCode"].ToString();

            return personDischarge;
        }

        public static PersonDischarge GetPersonDischarge(int personDischargeId, User currentUser)
        {
            PersonDischarge PersonDischarge = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT 
                                  a.SNOKVSID as PersonDischargeID,
                                  b.PersonID,
                                  a.SNOK_GDNA as Year,
                                  a.SNOK_SNOKOD as DischargeReasonCode,
                                  a.SNOK_PSOKOD as DischargeDestinationCode,
                                  a.SNOK_ZPVD as ""Order"",
                                  a.SNOK_KOGA as OrderDate,
                                  a.SNOK_OT_KOGA as OrderEffectiveDate,
                                  a.SNOK_SPZKOD as MilitaryCommanderRankCode
                              FROM VS_OWNER.VS_SNOKVS a
                              INNER JOIN VS_OWNER.VS_LS b ON a.SNOK_EGNLS = b.EGN
                              WHERE a.SNOKVSID = :PersonDischargeID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonDischargeID", OracleType.Number).Value = personDischargeId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    PersonDischarge = ExtractPersonDischargeFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return PersonDischarge;
        }

        public static PersonDischarge GetPersonDischarge(int personId, DateTime OrderDate, string DischargeReasonCode, User currentUser)
        {
            PersonDischarge PersonDischarge = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT 
                                  a.SNOKVSID as PersonDischargeID,
                                  b.PersonID,
                                  a.SNOK_GDNA as Year,
                                  a.SNOK_SNOKOD as DischargeReasonCode,
                                  a.SNOK_PSOKOD as DischargeDestinationCode,
                                  a.SNOK_ZPVD as ""Order"",
                                  a.SNOK_KOGA as OrderDate,
                                  a.SNOK_OT_KOGA as OrderEffectiveDate,
                                  a.SNOK_SPZKOD as MilitaryCommanderRankCode
                               FROM VS_OWNER.VS_SNOKVS a
                               INNER JOIN VS_OWNER.VS_LS b ON a.SNOK_EGNLS = b.EGN
                               WHERE b.PersonID = :PersonID AND
                                     a.SNOK_KOGA = :OrderDate AND
                                     a.SNOK_SNOKOD = :DischargeReasonCode";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Number).Value = personId;
                cmd.Parameters.Add("OrderDate", OracleType.DateTime).Value = OrderDate;
                cmd.Parameters.Add("DischargeReasonCode", OracleType.VarChar).Value = DischargeReasonCode;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    PersonDischarge = ExtractPersonDischargeFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return PersonDischarge;
        }

        public static List<PersonDischarge> GetAllPersonDischargeByPersonID(int personId, User currentUser)
        {
            List<PersonDischarge> listPersonDischarge = new List<PersonDischarge>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();

                string SQL = @"SELECT 
                                  a.SNOKVSID as PersonDischargeID,
                                  b.PersonID,
                                  a.SNOK_GDNA as Year,
                                  a.SNOK_SNOKOD as DischargeReasonCode,
                                  a.SNOK_PSOKOD as DischargeDestinationCode,
                                  a.SNOK_ZPVD as ""Order"",
                                  a.SNOK_KOGA as OrderDate,
                                  a.SNOK_OT_KOGA as OrderEffectiveDate,
                                  a.SNOK_SPZKOD as MilitaryCommanderRankCode
                               FROM VS_OWNER.VS_SNOKVS a
                               INNER JOIN VS_OWNER.VS_LS b ON a.SNOK_EGNLS = b.EGN
                               WHERE b.PersonID = :PersonID
                               ORDER BY a.SNOK_KOGA, a.SNOK_GDNA, a.SNOK_ZPVD";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Number).Value = personId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    listPersonDischarge.Add(ExtractPersonDischargeFromDR(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listPersonDischarge;
        }

        public static bool SavePersonDischarge(PersonDischarge personDischarge, Person person, User currentUser, Change changeEntry)
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

                if (personDischarge.PersonDischargeId == 0)
                {
                    SQL += @"INSERT INTO VS_OWNER.VS_SNOKVS (
                                    SNOK_EGNLS,
                                    SNOK_GDNA,
                                    SNOK_SNOKOD,
                                    SNOK_PSOKOD,
                                    SNOK_ZPVD,
                                    SNOK_KOGA,
                                    SNOK_OT_KOGA,
                                    SNOK_SPZKOD)

                            VALUES (
                                    :IdentNumber,
                                    :Year,
                                    :DischargeReasonCode,
                                    :DischargeDestinationCode,
                                    :Order,
                                    :OrderDate,
                                    :OrderEffectiveDate,
                                    :MilitaryCommanderRankCode
                                   );

                           SELECT VS_OWNER.VS_SNOKVS_SNOKVSID_SEQ.currval INTO :PersonDischargeId FROM dual;
                            
                            ";


                    changeEvent = new ChangeEvent("RES_Reservist_MilServ_AddDischarge", "", null, person, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Discharge_Year", "", personDischarge.Year.ToString(), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Discharge_DischargeReason", "", (personDischarge.DischargeReason != null ? personDischarge.DischargeReason.DischargeReasonName : ""), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Discharge_DischargeDestination", "", (personDischarge.DischargeDestination != null ? personDischarge.DischargeDestination.DischargeDestinationName : ""), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Discharge_Order", "", personDischarge.Order, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Discharge_OrderDate", "", CommonFunctions.FormatDate(personDischarge.OrderDate), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Discharge_OrderEffectiveDate", "", CommonFunctions.FormatDate(personDischarge.OrderEffectiveDate), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Discharge_MilitaryCommanderRank", "", (personDischarge.MilitaryCommanderRank != null ? personDischarge.MilitaryCommanderRank.MilitaryCommanderRankName : ""), currentUser));
                }
                else
                {
                    SQL += @"UPDATE VS_OWNER.VS_SNOKVS SET
                                SNOK_GDNA = :Year,
                                SNOK_SNOKOD = :DischargeReasonCode,
                                SNOK_PSOKOD = :DischargeDestinationCode,
                                SNOK_ZPVD = :Order,
                                SNOK_KOGA = :OrderDate,
                                SNOK_OT_KOGA = :OrderEffectiveDate,
                                SNOK_SPZKOD = :MilitaryCommanderRankCode
                             WHERE SNOKVSID = :PersonDischargeId ; ";


                    PersonDischarge oldPersonDischarge = GetPersonDischarge(personDischarge.PersonDischargeId, currentUser);

                    string logDescription = "Отчисляване от дата " + CommonFunctions.FormatDate(oldPersonDischarge.OrderDate) + " " +
                        @"по причина """ + (oldPersonDischarge.DischargeReason != null ? oldPersonDischarge.DischargeReason.DischargeReasonName : "") + @"""";

                    changeEvent = new ChangeEvent("RES_Reservist_MilServ_EditDischarge", logDescription, null, person, currentUser);

                    if (oldPersonDischarge.Year != personDischarge.Year)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Discharge_Year", oldPersonDischarge.Year.ToString(), personDischarge.Year.ToString(), currentUser));
                    if (oldPersonDischarge.DischargeReasonCode != personDischarge.DischargeReasonCode)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Discharge_DischargeReason", (oldPersonDischarge.DischargeReason != null ? oldPersonDischarge.DischargeReason.DischargeReasonName : ""), (personDischarge.DischargeReason != null ? personDischarge.DischargeReason.DischargeReasonName : ""), currentUser));
                    if (oldPersonDischarge.DischargeDestinationCode != personDischarge.DischargeDestinationCode)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Discharge_DischargeDestination", (oldPersonDischarge.DischargeDestination != null ? oldPersonDischarge.DischargeDestination.DischargeDestinationName : ""), (personDischarge.DischargeDestination != null ? personDischarge.DischargeDestination.DischargeDestinationName : ""), currentUser));
                    if (oldPersonDischarge.Order != personDischarge.Order)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Discharge_Order", oldPersonDischarge.Order, personDischarge.Order, currentUser));
                    if (oldPersonDischarge.OrderDate != personDischarge.OrderDate)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Discharge_OrderDate", CommonFunctions.FormatDate(oldPersonDischarge.OrderDate), CommonFunctions.FormatDate(personDischarge.OrderDate), currentUser));
                    if (oldPersonDischarge.OrderEffectiveDate != personDischarge.OrderEffectiveDate)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Discharge_OrderEffectiveDate", CommonFunctions.FormatDate(oldPersonDischarge.OrderEffectiveDate), CommonFunctions.FormatDate(personDischarge.OrderEffectiveDate), currentUser));
                    if (oldPersonDischarge.MilitaryCommanderRankCode != personDischarge.MilitaryCommanderRankCode)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Discharge_MilitaryCommanderRank", (oldPersonDischarge.MilitaryCommanderRank != null ? oldPersonDischarge.MilitaryCommanderRank.MilitaryCommanderRankName : ""), (personDischarge.MilitaryCommanderRank != null ? personDischarge.MilitaryCommanderRank.MilitaryCommanderRankName : ""), currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramPersonDischargeId = new OracleParameter();
                paramPersonDischargeId.ParameterName = "PersonDischargeId";
                paramPersonDischargeId.OracleType = OracleType.Number;

                if (personDischarge.PersonDischargeId != 0)
                {
                    paramPersonDischargeId.Direction = ParameterDirection.Input;
                    paramPersonDischargeId.Value = personDischarge.PersonDischargeId;
                }
                else
                {
                    paramPersonDischargeId.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramPersonDischargeId);

                OracleParameter param = null;

                if (personDischarge.PersonDischargeId == 0)
                {
                    //Insert
                    param = new OracleParameter();
                    param.ParameterName = "IdentNumber";
                    param.OracleType = OracleType.VarChar;
                    param.Direction = ParameterDirection.Input;
                    param.Value = person.IdentNumber;
                    cmd.Parameters.Add(param);
                }

                param = new OracleParameter();
                param.ParameterName = "Year";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = personDischarge.Year;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "DischargeReasonCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(personDischarge.DischargeReasonCode))
                {
                    param.Value = personDischarge.DischargeReasonCode;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "DischargeDestinationCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(personDischarge.DischargeDestinationCode))
                {
                    param.Value = personDischarge.DischargeDestinationCode;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Order";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(personDischarge.Order))
                {
                    param.Value = personDischarge.Order;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "OrderDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (personDischarge.OrderDate.HasValue)
                {
                    param.Value = personDischarge.OrderDate.Value;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "OrderEffectiveDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (personDischarge.OrderEffectiveDate.HasValue)
                {
                    param.Value = personDischarge.OrderEffectiveDate.Value;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryCommanderRankCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(personDischarge.MilitaryCommanderRankCode))
                {
                    param.Value = personDischarge.MilitaryCommanderRankCode;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);


                cmd.ExecuteNonQuery();

                if (personDischarge.PersonDischargeId == 0)
                    personDischarge.PersonDischargeId = DBCommon.GetInt(paramPersonDischargeId.Value);

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


        public static bool DeletePersonDischarge(int personDischargeId, Person person, User currentUser, Change changeEntry)
        {
            bool result = false;

            PersonDischarge oldPersonDischarge = GetPersonDischarge(personDischargeId, currentUser);

            ChangeEvent changeEvent = new ChangeEvent("RES_Reservist_MilServ_DeleteDischarge", "", null, person, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Discharge_Year", oldPersonDischarge.Year.ToString(), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Discharge_DischargeReason", (oldPersonDischarge.DischargeReason != null ? oldPersonDischarge.DischargeReason.DischargeReasonName : ""), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Discharge_DischargeDestination", (oldPersonDischarge.DischargeDestination != null ? oldPersonDischarge.DischargeDestination.DischargeDestinationName : ""), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Discharge_Order", oldPersonDischarge.Order, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Discharge_OrderDate", CommonFunctions.FormatDate(oldPersonDischarge.OrderDate), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Discharge_OrderEffectiveDate", CommonFunctions.FormatDate(oldPersonDischarge.OrderEffectiveDate), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Discharge_MilitaryCommanderRank", (oldPersonDischarge.MilitaryCommanderRank != null ? oldPersonDischarge.MilitaryCommanderRank.MilitaryCommanderRankName : ""), "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" BEGIN
                                  DELETE FROM VS_OWNER.VS_SNOKVS WHERE SNOKVSID = :PersonDischargeId;
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonDischargeId", OracleType.Number).Value = personDischargeId;

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
