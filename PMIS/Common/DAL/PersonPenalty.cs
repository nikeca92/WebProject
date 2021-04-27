using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;

namespace PMIS.Common
{
    public class PersonPenalty : BaseDbObject
    {
        public int PersonPenaltyId { get; set; }

        //1	Наказание NKZ_NKZKOD -> VS_OWNER.KLV_NKZ (1)
        public string PenaltyCode { get; set; }
        public Penalty penalty;
        public Penalty Penalty
        {
            get
            {
                if (penalty == null)
                {
                    penalty = PenaltyUtil.GetPenalty(PenaltyCode, CurrentUser);
                }
                return penalty;
            }
        }

        //2 Заповед (наложил) - NKZ_ZPVDN (7)   -  can be NULL
        public string VacAnnImposed { get; set; }

        //3 Дата - NKZ_KOGAN
        public DateTime DateImposed { get; set; }

        //iv.	Наложил наказанието - NKZ_NALNKOD -> VS_OWNER.KLV_SPZ
        public string MilitaryCommanderRankCodeImposed { get; set; }
        public MilitaryCommanderRank militaryCommanderRankImposed;
        public MilitaryCommanderRank MilitaryCommanderRankImposed
        {
            get
            {
                if (militaryCommanderRankImposed == null)
                {
                    militaryCommanderRankImposed = MilitaryCommanderRankUtil.GetMilitaryCommanderRank(MilitaryCommanderRankCodeImposed, CurrentUser);
                }
                return militaryCommanderRankImposed;
            }
        }

        //v.	Отменил наказанието - NKZ_NALOKOD -> VS_OWNER.KLV_SPZ - can be NULL
        public string MilitaryCommanderRankCodeCanceled { get; set; }
        public MilitaryCommanderRank militaryCommanderRankCanceled;
        public MilitaryCommanderRank MilitaryCommanderRankCanceled
        {
            get
            {
                if (militaryCommanderRankCanceled == null)
                {
                    militaryCommanderRankCanceled = MilitaryCommanderRankUtil.GetMilitaryCommanderRank(MilitaryCommanderRankCodeCanceled, CurrentUser);
                }
                return militaryCommanderRankCanceled;
            }
        }

        //vi.	Заповед (отменил)  - NKZ_ZPVDO  - can be NULL
        public string VacAnnCanceled { get; set; }

        //vii.	Дата - NKZ_KOGAO        - can be NULL
        public DateTime? DateCanceled { get; set; }


        private bool canDelete;
        public bool CanDelete
        {
            get
            {
                return true;
            }
        }

        public PersonPenalty(User user)
            : base(user)
        {
            this.VacAnnCanceled = "";
            this.VacAnnImposed = "";
            this.MilitaryCommanderRankCodeCanceled = "";
            this.MilitaryCommanderRankCodeImposed = "";
        }
    }

    public class PersonPenaltyUtil
    {

        private static PersonPenalty ExtractPersonPenaltyFromDR(OracleDataReader dr, User currentUser)
        {
            PersonPenalty personPenalty = new PersonPenalty(currentUser);

            personPenalty.PersonPenaltyId = DBCommon.GetInt(dr["PersonPenaltyId"]);

            personPenalty.PenaltyCode = dr["PenaltyCode"].ToString();
            personPenalty.VacAnnImposed = dr["VacAnnImposed"].ToString();

            personPenalty.DateImposed = (DateTime)(dr["DateImposed"]);
            personPenalty.MilitaryCommanderRankCodeImposed = dr["MilitaryCommRankCodeImposed"].ToString();

            personPenalty.MilitaryCommanderRankCodeCanceled = dr["MilitaryCommRankCodeCanceled"].ToString();
            personPenalty.VacAnnCanceled = dr["VacAnnCanceled"].ToString();

            personPenalty.DateCanceled = (dr["DateCanceled"] is DateTime ? (DateTime)dr["DateCanceled"] : (DateTime?)null);


            return personPenalty;
        }

        public static PersonPenalty GetPersonPenalty(int personPenaltyId, User currentUser)
        {
            PersonPenalty personPenalty = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT 
                                  a.NKZID as PersonPenaltyId,

                                  a.NKZ_NKZKOD as PenaltyCode,

                                  a.NKZ_ZPVDN as  VacAnnImposed,
                                  a.NKZ_KOGAN as DateImposed,
                                  a.NKZ_NALNKOD as MilitaryCommRankCodeImposed,

                                  a.NKZ_NALOKOD as  MilitaryCommRankCodeCanceled,
                                  a.NKZ_ZPVDO as VacAnnCanceled,
                                  a.NKZ_KOGAO as DateCanceled 

                              FROM VS_OWNER.VS_NKZ a 
                              WHERE a.NKZID = :PersonPenaltyId";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonPenaltyId", OracleType.Number).Value = personPenaltyId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    personPenalty = ExtractPersonPenaltyFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personPenalty;
        }

        //Use this Method accordin PK define in table to chek exist combination of these 2 parameters
        public static PersonPenalty GetPersonPenalty(string identityNumber, DateTime dateImposed, User currentUser)
        {
            PersonPenalty personPenalty = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT 
                                  a.NKZID as PersonPenaltyId,

                                  a.NKZ_NKZKOD as PenaltyCode,

                                  a.NKZ_ZPVDN as  VacAnnImposed,
                                  a.NKZ_KOGAN as DateImposed,
                                  a.NKZ_NALNKOD as MilitaryCommRankCodeImposed,

                                  a.NKZ_NALOKOD as  MilitaryCommRankCodeCanceled,
                                  a.NKZ_ZPVDO as VacAnnCanceled,
                                  a.NKZ_KOGAO as DateCanceled 

                              FROM VS_OWNER.VS_NKZ a 
                              WHERE a.NKZ_EGNLS = :IdentityNumber 
                                AND a.NKZ_KOGAN = :DateImposed";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("IdentityNumber", OracleType.VarChar).Value = identityNumber;
                cmd.Parameters.Add("DateImposed", OracleType.DateTime).Value = dateImposed;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    personPenalty = ExtractPersonPenaltyFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personPenalty;
        }

        public static List<PersonPenalty> GetAllPersonPenaltyByPersonID(int personId, User currentUser)
        {
            List<PersonPenalty> listPersonPenalty = new List<PersonPenalty>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT
                                  a.NKZID as PersonPenaltyId,

                                  a.NKZ_NKZKOD as PenaltyCode,

                                  a.NKZ_ZPVDN as  VacAnnImposed,
                                  a.NKZ_KOGAN as DateImposed,
                                  a.NKZ_NALNKOD as MilitaryCommRankCodeImposed,

                                  a.NKZ_NALOKOD as  MilitaryCommRankCodeCanceled,
                                  a.NKZ_ZPVDO as VacAnnCanceled,
                                  a.NKZ_KOGAO as DateCanceled 

                              FROM VS_OWNER.VS_NKZ a 
                               INNER JOIN VS_OWNER.VS_LS c ON a.NKZ_EGNLS = c.EGN
                               WHERE c.PersonID = :PersonID
                               ORDER BY a.NKZ_KOGAN";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Number).Value = personId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    listPersonPenalty.Add(ExtractPersonPenaltyFromDR(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listPersonPenalty;
        }

        public static bool SavePersonPenalty(PersonPenalty personPenalty, Person person, User currentUser, Change changeEntry)
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

                if (personPenalty.PersonPenaltyId == 0)
                {
                    SQL += @"INSERT INTO VS_OWNER.VS_NKZ (
                                    NKZ_EGNLS, 
                                    NKZ_NKZKOD,

                                    NKZ_ZPVDN,
                                    NKZ_KOGAN,
                                    NKZ_NALNKOD,

                                    NKZ_NALOKOD,
                                    NKZ_ZPVDO,
                                    NKZ_KOGAO  )

                            VALUES (
                                    :IdentityNumber, 
                                    :PenaltyCode,

                                     :VacAnnImposed,
                                     :DateImposed,
                                     :MilitaryCommRankCodeImposed,

                                     :MilitaryCommRankCodeCanceled,
                                     :VacAnnCanceled,
                                     :DateCanceled 
                                    );

                           SELECT VS_OWNER.VS_NKZ_NKZID_SEQ.currval INTO :PersonPenaltyId FROM dual;
                            
                            ";

                    changeEvent = new ChangeEvent("RES_Reservist_MilServ_AddPenalty", "", null, person, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Penalty_Title", "", personPenalty.Penalty.PenaltyName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Penalty_VacAnnImposed", "", personPenalty.VacAnnImposed, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Penalty_DateImposed", "", CommonFunctions.FormatDate(personPenalty.DateImposed), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Penalty_MilitaryCommanderRankImposed", "", personPenalty.MilitaryCommanderRankImposed.MilitaryCommanderRankName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Penalty_MilitaryCommanderRankCanceled", "", !string.IsNullOrEmpty(personPenalty.MilitaryCommanderRankCodeCanceled) ? personPenalty.MilitaryCommanderRankCanceled.MilitaryCommanderRankName : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Penalty_VacAnnCanceled", "", personPenalty.VacAnnCanceled, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Penalty_DateCanceled", "", personPenalty.DateCanceled.HasValue ? CommonFunctions.FormatDate(personPenalty.DateCanceled) : "", currentUser));

                }
                else
                {
                    SQL += @"UPDATE VS_OWNER.VS_NKZ SET

                                  NKZ_NKZKOD = :PenaltyCode,

                                  NKZ_ZPVDN  = :VacAnnImposed,
                                  NKZ_KOGAN  = :DateImposed,
                                  NKZ_NALNKOD = :MilitaryCommRankCodeImposed,

                                  NKZ_NALOKOD = :MilitaryCommRankCodeCanceled,
                                  NKZ_ZPVDO  =  :VacAnnCanceled,
                                  NKZ_KOGAO  =  :DateCanceled 

                                WHERE NKZID = :PersonPenaltyId ; ";


                    PersonPenalty oldPersonPenalty = GetPersonPenalty(personPenalty.PersonPenaltyId, currentUser);

                    string logDescription = "Наказание: " + oldPersonPenalty.Penalty.PenaltyName + "; " +
                                            "Дата: " + CommonFunctions.FormatDate(oldPersonPenalty.DateImposed);


                    changeEvent = new ChangeEvent("RES_Reservist_MilServ_EditPenalty", logDescription, null, person, currentUser);

                    if (oldPersonPenalty.PenaltyCode != personPenalty.PenaltyCode)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Penalty_Title", oldPersonPenalty.Penalty.PenaltyName, personPenalty.Penalty.PenaltyName, currentUser));
                    if (oldPersonPenalty.VacAnnImposed != personPenalty.VacAnnImposed)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Penalty_VacAnnImposed", oldPersonPenalty.VacAnnImposed, personPenalty.VacAnnImposed, currentUser));
                    if (oldPersonPenalty.DateImposed != personPenalty.DateImposed)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Penalty_DateImposed", CommonFunctions.FormatDate(oldPersonPenalty.DateImposed), CommonFunctions.FormatDate(personPenalty.DateImposed), currentUser));
                    if (oldPersonPenalty.MilitaryCommanderRankCodeImposed != personPenalty.MilitaryCommanderRankCodeImposed)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Penalty_MilitaryCommanderRankImposed", oldPersonPenalty.MilitaryCommanderRankImposed.MilitaryCommanderRankName, personPenalty.MilitaryCommanderRankImposed.MilitaryCommanderRankName, currentUser));
                    if (oldPersonPenalty.MilitaryCommanderRankCodeCanceled != personPenalty.MilitaryCommanderRankCodeCanceled)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Penalty_MilitaryCommanderRankCanceled", !string.IsNullOrEmpty(oldPersonPenalty.MilitaryCommanderRankCodeCanceled) ? oldPersonPenalty.MilitaryCommanderRankCanceled.MilitaryCommanderRankName : "", !string.IsNullOrEmpty(personPenalty.MilitaryCommanderRankCodeCanceled) ? personPenalty.MilitaryCommanderRankCanceled.MilitaryCommanderRankName : "", currentUser));
                    if (oldPersonPenalty.VacAnnCanceled != personPenalty.VacAnnCanceled)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Penalty_VacAnnCanceled", oldPersonPenalty.VacAnnCanceled, personPenalty.VacAnnCanceled, currentUser));
                    if (oldPersonPenalty.DateCanceled != personPenalty.DateCanceled)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Penalty_DateCanceled", oldPersonPenalty.DateCanceled.HasValue ? CommonFunctions.FormatDate(oldPersonPenalty.DateCanceled) : "", personPenalty.DateCanceled.HasValue ? CommonFunctions.FormatDate(personPenalty.DateCanceled) : "", currentUser));

                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramPenaltyId = new OracleParameter();
                paramPenaltyId.ParameterName = "PersonPenaltyId";
                paramPenaltyId.OracleType = OracleType.Number;

                if (personPenalty.PersonPenaltyId != 0)
                {
                    paramPenaltyId.Direction = ParameterDirection.Input;
                    paramPenaltyId.Value = personPenalty.PersonPenaltyId;
                }
                else
                {
                    paramPenaltyId.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramPenaltyId);

                OracleParameter param = null;

                if (personPenalty.PersonPenaltyId == 0)
                {
                    //Insert
                    param = new OracleParameter();
                    param.ParameterName = "IdentityNumber";
                    param.OracleType = OracleType.VarChar;
                    param.Direction = ParameterDirection.Input;
                    param.Value = person.IdentNumber;
                    cmd.Parameters.Add(param);
                }
                else
                {
                    //Update
                    param = new OracleParameter();
                    param.ParameterName = "PersonPenaltyId";
                    param.OracleType = OracleType.Number;
                    param.Direction = ParameterDirection.Input;
                    param.Value = personPenalty.PersonPenaltyId;
                    cmd.Parameters.Add(param);
                }

                param = new OracleParameter();
                param.ParameterName = "PenaltyCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personPenalty.PenaltyCode;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "VacAnnImposed";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(personPenalty.VacAnnImposed))
                {
                    param.Value = personPenalty.VacAnnImposed;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "DateImposed";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                param.Value = personPenalty.DateImposed;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryCommRankCodeImposed";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personPenalty.MilitaryCommanderRankCodeImposed;
                cmd.Parameters.Add(param);


                param = new OracleParameter();
                param.ParameterName = "MilitaryCommRankCodeCanceled";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(personPenalty.MilitaryCommanderRankCodeCanceled))
                {
                    param.Value = personPenalty.MilitaryCommanderRankCodeCanceled;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);


                param = new OracleParameter();
                param.ParameterName = "VacAnnCanceled";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(personPenalty.VacAnnCanceled))
                {
                    param.Value = personPenalty.VacAnnCanceled;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);


                param = new OracleParameter();
                param.ParameterName = "DateCanceled";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (personPenalty.DateCanceled.HasValue)
                {
                    param.Value = personPenalty.DateCanceled.Value;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);



                cmd.ExecuteNonQuery();

                if (personPenalty.PersonPenaltyId == 0)
                    personPenalty.PersonPenaltyId = DBCommon.GetInt(paramPenaltyId.Value);

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


        public static bool DeletePersonPenalty(int personPenaltyId, Person person, User currentUser, Change changeEntry)
        {
            bool result = false;

            PersonPenalty oldPersonPenalty = GetPersonPenalty(personPenaltyId, currentUser);

            ChangeEvent changeEvent = new ChangeEvent("RES_Reservist_MilServ_DeletePenalty", "", null, person, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Penalty_Title", oldPersonPenalty.Penalty.PenaltyName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Penalty_VacAnnImposed", oldPersonPenalty.VacAnnImposed, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Penalty_DateImposed", CommonFunctions.FormatDate(oldPersonPenalty.DateImposed), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Penalty_MilitaryCommanderRankImposed", oldPersonPenalty.MilitaryCommanderRankImposed.MilitaryCommanderRankName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Penalty_MilitaryCommanderRankCanceled", !string.IsNullOrEmpty(oldPersonPenalty.MilitaryCommanderRankCodeCanceled) ? oldPersonPenalty.MilitaryCommanderRankCanceled.MilitaryCommanderRankName : "", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Penalty_VacAnnCanceled", oldPersonPenalty.VacAnnCanceled, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_Penalty_DateCanceled", oldPersonPenalty.DateCanceled.HasValue ? CommonFunctions.FormatDate(oldPersonPenalty.DateCanceled) : "", "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" BEGIN
                                  DELETE  FROM VS_OWNER.VS_NKZ WHERE NKZID = :PersonPenaltyId;
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonPenaltyId", OracleType.Number).Value = personPenaltyId;

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
