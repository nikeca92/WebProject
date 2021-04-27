using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;

namespace PMIS.Common
{
    public class PersonRewardIncentiv : BaseDbObject
    {

        public int PersonRewardIncentivId { get; set; }

        //NGR_NGRKOD (1)
        public string RewardIncentivCode { get; set; }
        public RewardIncentiv rewardIncentiv;
        public RewardIncentiv RewardIncentiv
        {
            get
            {
                if (rewardIncentiv == null)
                {
                    rewardIncentiv = RewardIncentivUtil.GetRewardIncentiv(RewardIncentivCode, CurrentUser);
                }
                return rewardIncentiv;
            }
        }

        //NGR_NOMER (1)
        public int RewardIncentivNumber { get; set; }

        //NGR_ZPVD (7)  - nulable YES
        public string VacAnn { get; set; }

        //NGR_KOGA
        public DateTime DateWhen { get; set; }

        //NGR_NALKOD  - nulable YES
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


        private bool canDelete;
        public bool CanDelete
        {
            get
            {
                return true;
            }
        }

        public PersonRewardIncentiv(User user)
            : base(user)
        {
            this.RewardIncentivCode = "";
            this.MilitaryCommanderRankCode = "";
            this.VacAnn = "";
        }
    }

    public class PersonRewardIncentivUtil
    {

        private static PersonRewardIncentiv ExtractPersonRewardIncentivFromDR(OracleDataReader dr, User currentUser)
        {
            PersonRewardIncentiv personRewardIncentiv = new PersonRewardIncentiv(currentUser);

            personRewardIncentiv.PersonRewardIncentivId = DBCommon.GetInt(dr["PersonRewardIncentivId"]);
            personRewardIncentiv.RewardIncentivCode = dr["RewardIncentivCode"].ToString();
            personRewardIncentiv.RewardIncentivNumber = DBCommon.GetInt(dr["RewardIncentivNumber"]);
            personRewardIncentiv.VacAnn = dr["VacAnn"].ToString();
            personRewardIncentiv.DateWhen = (DateTime)(dr["DateWhen"]);
            personRewardIncentiv.MilitaryCommanderRankCode = dr["MilitaryCommanderRankCode"].ToString();

            return personRewardIncentiv;
        }

        public static PersonRewardIncentiv GetPersonRewardIncentiv(int personRewardIncentivId, User currentUser)
        {
            PersonRewardIncentiv personRewardIncentiv = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT 
                                  a.NGRID as PersonRewardIncentivId,
                                  a.NGR_NGRKOD as RewardIncentivCode,
                                  a.NGR_NOMER as  RewardIncentivNumber,
                                  a.NGR_ZPVD as VacAnn,
                                  a.NGR_KOGA as DateWhen,
                                  a.NGR_NALKOD as MilitaryCommanderRankCode 
                              FROM VS_OWNER.VS_NGR a 
                              WHERE a.NGRID = :PersonRewardIncentivId";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonRewardIncentivId", OracleType.Number).Value = personRewardIncentivId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    personRewardIncentiv = ExtractPersonRewardIncentivFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personRewardIncentiv;
        }

        //Use this Method accordin PK define in table to chek exist combination of these 3 parameters
        public static PersonRewardIncentiv GetPersonRewardIncentiv(string identityNumber, int rewardIncentivNumber, DateTime dateWhen, User currentUser)
        {
            PersonRewardIncentiv personRewardIncentiv = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT 
                                  a.NGRID as PersonRewardIncentivId,
                                  a.NGR_NGRKOD as RewardIncentivCode,
                                  a.NGR_NOMER as  RewardIncentivNumber,
                                  a.NGR_ZPVD as VacAnn,
                                  a.NGR_KOGA as DateWhen,
                                  a.NGR_NALKOD as MilitaryCommanderRankCode 
                              FROM VS_OWNER.VS_NGR a 
                              WHERE a.NGR_EGNLS = :IdentityNumber 
                                AND a.NGR_NOMER = :RewardIncentivNumber 
                                AND a.NGR_KOGA = :DateWhen";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("IdentityNumber", OracleType.VarChar).Value = identityNumber;
                cmd.Parameters.Add("RewardIncentivNumber", OracleType.Number).Value = rewardIncentivNumber;
                cmd.Parameters.Add("DateWhen", OracleType.DateTime).Value = dateWhen;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    personRewardIncentiv = ExtractPersonRewardIncentivFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personRewardIncentiv;
        }

        public static List<PersonRewardIncentiv> GetAllPersonRewardIncentivByPersonID(int personId, User currentUser)
        {
            List<PersonRewardIncentiv> listPersonRewardIncentiv = new List<PersonRewardIncentiv>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT
                                  a.NGRID as PersonRewardIncentivId,
                                  a.NGR_NGRKOD as RewardIncentivCode,
                                  a.NGR_NOMER as  RewardIncentivNumber,
                                  a.NGR_ZPVD as VacAnn,
                                  a.NGR_KOGA as DateWhen,
                                  a.NGR_NALKOD as MilitaryCommanderRankCode 
                               FROM VS_OWNER.VS_NGR a 
                               INNER JOIN VS_OWNER.VS_LS c ON a.NGR_EGNLS = c.EGN
                               WHERE c.PersonID = :PersonID
                               ORDER BY a.NGR_KOGA, a.NGR_NOMER";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Number).Value = personId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    listPersonRewardIncentiv.Add(ExtractPersonRewardIncentivFromDR(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listPersonRewardIncentiv;
        }

        public static bool SavePersonRewardIncentiv(PersonRewardIncentiv personRewardIncentiv, Person person, User currentUser, Change changeEntry)
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

                if (personRewardIncentiv.PersonRewardIncentivId == 0)
                {
                    SQL += @"INSERT INTO VS_OWNER.VS_NGR (
                                    NGR_EGNLS, 
                                    NGR_NGRKOD,
                                    NGR_NOMER, 
                                    NGR_ZPVD,
                                    NGR_KOGA,
                                    NGR_NALKOD )

                            VALUES (:IdentNumber, 
                                    :RewardIncentivCode,
                                    :RewardIncentivNumber,
                                    :VacAnn,
                                    :DateWhen,
                                    :MilitaryCommanderRankCode 
                                    );

                           SELECT VS_OWNER.VS_NGR_NGRID_SEQ.currval INTO :PersonRewardIncentivId FROM dual;
                            
                            ";

                    changeEvent = new ChangeEvent("RES_Reservist_MilServ_AddRewardIncentiv", "", null, person, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_RewardIncentiv_Title", "", personRewardIncentiv.RewardIncentiv.RewardIncentivName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_RewardIncentiv_Number", "", personRewardIncentiv.RewardIncentivNumber.ToString(), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_RewardIncentiv_VacAnn", "", personRewardIncentiv.VacAnn, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_RewardIncentiv_DateWhen", "", CommonFunctions.FormatDate(personRewardIncentiv.DateWhen), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_RewardIncentiv_MilitaryCommanderRank", "", !string.IsNullOrEmpty(personRewardIncentiv.MilitaryCommanderRankCode) ? personRewardIncentiv.MilitaryCommanderRank.MilitaryCommanderRankName : "", currentUser));

                }
                else
                {
                    SQL += @"UPDATE VS_OWNER.VS_NGR SET

                                  NGR_NGRKOD = :RewardIncentivCode,
                                  NGR_NOMER  = :RewardIncentivNumber,
                                  NGR_ZPVD   = :VacAnn,
                                  NGR_KOGA   = :DateWhen,
                                  NGR_NALKOD = :MilitaryCommanderRankCode 

                              WHERE NGRID = :PersonRewardIncentivId ; ";


                    PersonRewardIncentiv oldPersonRewardIncentiv = GetPersonRewardIncentiv(personRewardIncentiv.PersonRewardIncentivId, currentUser);

                    string logDescription = "Награда: " + oldPersonRewardIncentiv.RewardIncentiv.RewardIncentivName + "; " +
                                            "Дата: " + CommonFunctions.FormatDate(oldPersonRewardIncentiv.DateWhen);


                    changeEvent = new ChangeEvent("RES_Reservist_MilServ_EditRewardIncentiv", logDescription, null, person, currentUser);


                    if (oldPersonRewardIncentiv.RewardIncentivCode != personRewardIncentiv.RewardIncentivCode)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_RewardIncentiv_Title", oldPersonRewardIncentiv.RewardIncentiv.RewardIncentivName, personRewardIncentiv.RewardIncentiv.RewardIncentivName, currentUser));
                    if (oldPersonRewardIncentiv.RewardIncentivNumber != personRewardIncentiv.RewardIncentivNumber)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_RewardIncentiv_Number", oldPersonRewardIncentiv.RewardIncentivNumber.ToString(), personRewardIncentiv.RewardIncentivNumber.ToString(), currentUser));
                    if (oldPersonRewardIncentiv.VacAnn != personRewardIncentiv.VacAnn)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_RewardIncentiv_VacAnn", oldPersonRewardIncentiv.VacAnn, personRewardIncentiv.VacAnn, currentUser));
                    if (oldPersonRewardIncentiv.DateWhen != personRewardIncentiv.DateWhen)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_RewardIncentiv_DateWhen", CommonFunctions.FormatDate(oldPersonRewardIncentiv.DateWhen), CommonFunctions.FormatDate(personRewardIncentiv.DateWhen), currentUser));
                    if (oldPersonRewardIncentiv.MilitaryCommanderRankCode != personRewardIncentiv.MilitaryCommanderRankCode)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_RewardIncentiv_MilitaryCommanderRank", !string.IsNullOrEmpty(oldPersonRewardIncentiv.MilitaryCommanderRankCode) ? oldPersonRewardIncentiv.MilitaryCommanderRank.MilitaryCommanderRankName : "", !string.IsNullOrEmpty(personRewardIncentiv.MilitaryCommanderRankCode) ? personRewardIncentiv.MilitaryCommanderRank.MilitaryCommanderRankName : "", currentUser));


                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramRewardIncentivId = new OracleParameter();
                paramRewardIncentivId.ParameterName = "PersonRewardIncentivId";
                paramRewardIncentivId.OracleType = OracleType.Number;

                if (personRewardIncentiv.PersonRewardIncentivId != 0)
                {
                    paramRewardIncentivId.Direction = ParameterDirection.Input;
                    paramRewardIncentivId.Value = personRewardIncentiv.PersonRewardIncentivId;
                }
                else
                {
                    paramRewardIncentivId.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramRewardIncentivId);

                OracleParameter param = null;

                if (personRewardIncentiv.PersonRewardIncentivId == 0)
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
                    param.ParameterName = "PersonRewardIncentivId";
                    param.OracleType = OracleType.Number;
                    param.Direction = ParameterDirection.Input;
                    param.Value = personRewardIncentiv.PersonRewardIncentivId;
                    cmd.Parameters.Add(param);
                }

                param = new OracleParameter();
                param.ParameterName = "RewardIncentivCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personRewardIncentiv.RewardIncentivCode;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "RewardIncentivNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personRewardIncentiv.RewardIncentivNumber;
                cmd.Parameters.Add(param);


                param = new OracleParameter();
                param.ParameterName = "VacAnn";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(personRewardIncentiv.VacAnn))
                {
                    param.Value = personRewardIncentiv.VacAnn;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);


                param = new OracleParameter();
                param.ParameterName = "DateWhen";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                param.Value = personRewardIncentiv.DateWhen;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryCommanderRankCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(personRewardIncentiv.MilitaryCommanderRankCode))
                {
                    param.Value = personRewardIncentiv.MilitaryCommanderRankCode;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (personRewardIncentiv.PersonRewardIncentivId == 0)
                    personRewardIncentiv.PersonRewardIncentivId = DBCommon.GetInt(paramRewardIncentivId.Value);

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


        public static bool DeletePersonRewardIncentiv(int personRewardIncentivId, Person person, User currentUser, Change changeEntry)
        {
            bool result = false;

            PersonRewardIncentiv oldPersonRewardIncentiv = GetPersonRewardIncentiv(personRewardIncentivId, currentUser);

            ChangeEvent changeEvent = new ChangeEvent("RES_Reservist_MilServ_DeleteRewardIncentiv", "", null, person, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_RewardIncentiv_Title", oldPersonRewardIncentiv.RewardIncentiv.RewardIncentivName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_RewardIncentiv_Number", oldPersonRewardIncentiv.RewardIncentivNumber.ToString(), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_RewardIncentiv_VacAnn", oldPersonRewardIncentiv.VacAnn, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_RewardIncentiv_DateWhen", CommonFunctions.FormatDate(oldPersonRewardIncentiv.DateWhen), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_RewardIncentiv_MilitaryCommanderRank", !string.IsNullOrEmpty(oldPersonRewardIncentiv.MilitaryCommanderRankCode) ? oldPersonRewardIncentiv.MilitaryCommanderRank.MilitaryCommanderRankName : "", "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" BEGIN
                                  DELETE  FROM VS_OWNER.VS_NGR WHERE NGRID = :PersonRewardIncentivId;
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonRewardIncentivId", OracleType.Number).Value = personRewardIncentivId;

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
