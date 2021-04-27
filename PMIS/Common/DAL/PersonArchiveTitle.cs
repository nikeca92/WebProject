using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;

namespace PMIS.Common
{
    public class PersonArchiveTitle : BaseDbObject
    {
        public int PersonArchiveTitleId { get; set; }

        public string MilitaryRankId { get; set; }
        public MilitaryRank militaryRank;
        public MilitaryRank MilitaryRank
        {
            get
            {
                if (militaryRank == null)
                {
                    militaryRank = MilitaryRankUtil.GetMilitaryRank(MilitaryRankId, CurrentUser);
                }
                return militaryRank;
            }
        }

        //Cann be null in DB
        public string VacAnn { get; set; }

        public DateTime DateArchive { get; set; }
        public DateTime DateWhen { get; set; }

        //Cann be null  in DB
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

        public PersonArchiveTitle(User user)
            : base(user)
        {
            this.VacAnn = "";
            this.MilitaryCommanderRankCode = "";
        }

        private bool isDR;

        public bool IsDR
        {
            get { return isDR; }
            set { isDR = value; }
        }

    }
    public class PersonArchiveTitleUtil
    {

        private static PersonArchiveTitle ExtractPersonArchiveTitleFromDR(OracleDataReader dr, User currentUser)
        {
            PersonArchiveTitle personArchiveTitle = new PersonArchiveTitle(currentUser);

            personArchiveTitle.PersonArchiveTitleId = DBCommon.GetInt(dr["PersonArchiveTitleId"]);
            personArchiveTitle.MilitaryRankId = dr["MilitaryRankId"].ToString();
            personArchiveTitle.VacAnn = dr["VacAnn"].ToString();
            personArchiveTitle.DateArchive = Convert.ToDateTime(dr["DateArchive"]);
            personArchiveTitle.DateWhen = Convert.ToDateTime(dr["DateWhen"]);
            personArchiveTitle.MilitaryCommanderRankCode = dr["MilitaryCommanderRankCode"].ToString();
            personArchiveTitle.IsDR = (dr["DR"].ToString() == "Y" ? true : false);
            return personArchiveTitle;
        }

        public static PersonArchiveTitle GetPersonArchiveTitle(int personArchiveTitleId, User currentUser)
        {
            PersonArchiveTitle personArchiveTitle = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT 
                                  a.ARZVAID as PersonArchiveTitleId,
                                  a.ARZVA_ZVAKOD as MilitaryRankId,
                                  a.ARZVA_ZPVD as VacAnn,
                                  a.ARZVA_ZIZD as DateArchive,
                                  a.ARZVA_ZKOGA as DateWhen,
                                  a.ARZVA_SPZKOD as MilitaryCommanderRankCode,
                                  a.ARZVA_DR as DR
                              FROM VS_OWNER.VS_AR_ZVA a 
                              WHERE a.ARZVAID = :PersonArchiveTitleId";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonArchiveTitleId", OracleType.Number).Value = personArchiveTitleId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    personArchiveTitle = ExtractPersonArchiveTitleFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personArchiveTitle;
        }

        //Use this Method accordin PK define in table to chek exist combination of these 3 parameters
        public static PersonArchiveTitle GetPersonArchiveTitle(string identityNumber, string militaryRankId, DateTime dateWhen, User currentUser)
        {
            PersonArchiveTitle personArchiveTitle = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT 
                                  a.ARZVAID as PersonArchiveTitleId,
                                  a.ARZVA_ZVAKOD as MilitaryRankId,
                                  a.ARZVA_ZPVD as VacAnn,
                                  a.ARZVA_ZIZD as DateArchive,
                                  a.ARZVA_ZKOGA as DateWhen,
                                  a.ARZVA_SPZKOD as MilitaryCommanderRankCode,
                                  a.ARZVA_DR as DR 
                              FROM VS_OWNER.VS_AR_ZVA a 
                              WHERE a.ARZVA_EGNLS = :IdentityNumber 
                                AND a.ARZVA_ZVAKOD = :MilitaryRankId 
                                AND a.ARZVA_ZKOGA = :DateWhen";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("IdentityNumber", OracleType.VarChar).Value = identityNumber;
                cmd.Parameters.Add("MilitaryRankId", OracleType.VarChar).Value = militaryRankId;
                cmd.Parameters.Add("DateWhen", OracleType.DateTime).Value = dateWhen;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    personArchiveTitle = ExtractPersonArchiveTitleFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personArchiveTitle;
        }

        public static List<PersonArchiveTitle> GetAllPersonArchiveTitleByPersonID(int personId, User currentUser)
        {
            List<PersonArchiveTitle> listPersonArchiveTitle = new List<PersonArchiveTitle>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT
                                  a.ARZVAID as PersonArchiveTitleId,
                                  a.ARZVA_ZVAKOD as MilitaryRankId,
                                  a.ARZVA_ZPVD as VacAnn,
                                  a.ARZVA_ZIZD as DateArchive,
                                  a.ARZVA_ZKOGA as DateWhen,
                                  a.ARZVA_SPZKOD as MilitaryCommanderRankCode,
                                  a.ARZVA_DR as DR
                               FROM VS_OWNER.VS_AR_ZVA a 
                               INNER JOIN VS_OWNER.VS_LS c ON a.ARZVA_EGNLS = c.EGN
                               WHERE c.PersonID = :PersonID
                               ORDER BY a.ARZVA_ZVAKOD, a.ARZVA_ZKOGA, a.ARZVA_SPZKOD";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Number).Value = personId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    listPersonArchiveTitle.Add(ExtractPersonArchiveTitleFromDR(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listPersonArchiveTitle;
        }

        public static bool SavePersonArchiveTitle(PersonArchiveTitle personArchiveTitle, Person person, User currentUser, Change changeEntry)
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

                if (personArchiveTitle.PersonArchiveTitleId == 0)
                {
                    SQL += @"INSERT INTO VS_OWNER.VS_AR_ZVA (
                                    ARZVA_EGNLS, 
                                    ARZVA_ZVAKOD,
                                    ARZVA_ZPVD, 
                                    ARZVA_ZIZD,
                                    ARZVA_ZKOGA,
                                    ARZVA_SPZKOD,
                                    ARZVA_DR)

                            VALUES (:IdentNumber, 
                                    :MilitaryRankId,
                                    :VacAnn,
                                    :DateArchive,
                                    :DateWhen,
                                    :MilitaryCommanderRankCode,
                                    :DR
                                    );

                           SELECT VS_OWNER.VS_AR_ZVA_ARZVAID_SEQ.currval INTO :PersonArchiveTitleId FROM dual;
                            
                            ";

                    changeEvent = new ChangeEvent("RES_Reservist_MilServ_AddArchiveTitle", "", null, person, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_ArchiveTitle_Title", "", personArchiveTitle.MilitaryRank.LongName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_ArchiveTitle_VacAnn", "", personArchiveTitle.VacAnn, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_ArchiveTitle_DateArchive", "", CommonFunctions.FormatDate(personArchiveTitle.DateWhen), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_ArchiveTitle_DateWhen", "", CommonFunctions.FormatDate(personArchiveTitle.DateArchive), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_ArchiveTitle_MilitaryCommanderRank", "", !string.IsNullOrEmpty(personArchiveTitle.MilitaryCommanderRankCode) ? personArchiveTitle.MilitaryCommanderRank.MilitaryCommanderRankName : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_ArchiveTitle_DR", "", (personArchiveTitle.IsDR ? "1" : "0"), currentUser));

                }
                else
                {
                    SQL += @"UPDATE VS_OWNER.VS_AR_ZVA SET

                                  ARZVA_ZVAKOD = :MilitaryRankId,
                                  ARZVA_ZPVD   = :VacAnn,
                                  ARZVA_ZIZD   = :DateArchive,
                                  ARZVA_ZKOGA  = :DateWhen,
                                  ARZVA_SPZKOD = :MilitaryCommanderRankCode,
                                  ARZVA_DR     = :DR

                              WHERE ARZVAID = :PersonArchiveTitleId ; ";


                    PersonArchiveTitle oldPersonArchiveTitle = GetPersonArchiveTitle(personArchiveTitle.PersonArchiveTitleId, currentUser);

                    string logDescription = "Военно Звание: " + oldPersonArchiveTitle.MilitaryRank.LongName + "; " +
                                            "Дата: " + CommonFunctions.FormatDate(oldPersonArchiveTitle.DateWhen);


                    changeEvent = new ChangeEvent("RES_Reservist_MilServ_EditArchiveTitle", logDescription, null, person, currentUser);


                    if (oldPersonArchiveTitle.MilitaryRankId != personArchiveTitle.MilitaryRankId)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_ArchiveTitle_Title", oldPersonArchiveTitle.MilitaryRank.LongName, personArchiveTitle.MilitaryRank.LongName, currentUser));
                    if (oldPersonArchiveTitle.VacAnn != personArchiveTitle.VacAnn)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_ArchiveTitle_VacAnn", oldPersonArchiveTitle.VacAnn, personArchiveTitle.VacAnn, currentUser));
                    if (oldPersonArchiveTitle.DateArchive != personArchiveTitle.DateArchive)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_ArchiveTitle_DateArchive", CommonFunctions.FormatDate(oldPersonArchiveTitle.DateWhen), CommonFunctions.FormatDate(personArchiveTitle.DateWhen), currentUser));
                    if (oldPersonArchiveTitle.DateWhen != personArchiveTitle.DateWhen)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_ArchiveTitle_DateWhen", CommonFunctions.FormatDate(oldPersonArchiveTitle.DateArchive), CommonFunctions.FormatDate(personArchiveTitle.DateArchive), currentUser));
                    if (oldPersonArchiveTitle.MilitaryCommanderRankCode != personArchiveTitle.MilitaryCommanderRankCode)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_ArchiveTitle_MilitaryCommanderRank", !string.IsNullOrEmpty(oldPersonArchiveTitle.MilitaryCommanderRankCode) ? oldPersonArchiveTitle.MilitaryCommanderRank.MilitaryCommanderRankName : "", !string.IsNullOrEmpty(personArchiveTitle.MilitaryCommanderRankCode) ? personArchiveTitle.MilitaryCommanderRank.MilitaryCommanderRankName : "", currentUser));
                    if (oldPersonArchiveTitle.IsDR != personArchiveTitle.IsDR)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_ArchiveTitle_DR", (oldPersonArchiveTitle.IsDR ? "1" : "0"), (personArchiveTitle.IsDR ? "1" : "0"), currentUser));

                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramArchiveTitleId = new OracleParameter();
                paramArchiveTitleId.ParameterName = "PersonArchiveTitleId";
                paramArchiveTitleId.OracleType = OracleType.Number;

                if (personArchiveTitle.PersonArchiveTitleId != 0)
                {
                    paramArchiveTitleId.Direction = ParameterDirection.Input;
                    paramArchiveTitleId.Value = personArchiveTitle.PersonArchiveTitleId;
                }
                else
                {
                    paramArchiveTitleId.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramArchiveTitleId);

                OracleParameter param = null;

                if (personArchiveTitle.PersonArchiveTitleId == 0)
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
                    param.ParameterName = "PersonArchiveTitleId";
                    param.OracleType = OracleType.Number;
                    param.Direction = ParameterDirection.Input;
                    param.Value = personArchiveTitle.PersonArchiveTitleId;
                    cmd.Parameters.Add(param);
                }

                param = new OracleParameter();
                param.ParameterName = "MilitaryRankId";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personArchiveTitle.MilitaryRankId.ToString();
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "VacAnn";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(personArchiveTitle.VacAnn))
                {
                    param.Value = personArchiveTitle.VacAnn;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "DateArchive";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                param.Value = personArchiveTitle.DateArchive;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "DateWhen";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                param.Value = personArchiveTitle.DateWhen;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryCommanderRankCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(personArchiveTitle.MilitaryCommanderRankCode))
                {
                    param.Value = personArchiveTitle.MilitaryCommanderRankCode;
                }
                else
                {
                    param.Value = DBNull.Value;
                }
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "DR";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = (personArchiveTitle.IsDR ? "Y" : "N");
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (personArchiveTitle.PersonArchiveTitleId == 0)
                    personArchiveTitle.PersonArchiveTitleId = DBCommon.GetInt(paramArchiveTitleId.Value);

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


        public static bool DeletePersonArchiveTitle(int personArchiveTitleId, Person person, User currentUser, Change changeEntry)
        {
            bool result = false;

            PersonArchiveTitle oldPersonArchiveTitle = GetPersonArchiveTitle(personArchiveTitleId, currentUser);

            ChangeEvent changeEvent = new ChangeEvent("RES_Reservist_MilServ_DeleteArchiveTitle", "", null, person, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_ArchiveTitle_Title", oldPersonArchiveTitle.MilitaryRank.LongName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_ArchiveTitle_VacAnn", !string.IsNullOrEmpty(oldPersonArchiveTitle.VacAnn) ? oldPersonArchiveTitle.VacAnn : "", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_ArchiveTitle_DateArchive", CommonFunctions.FormatDate(oldPersonArchiveTitle.DateWhen), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_ArchiveTitle_DateWhen", CommonFunctions.FormatDate(oldPersonArchiveTitle.DateArchive), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_ArchiveTitle_MilitaryCommanderRank", !string.IsNullOrEmpty(oldPersonArchiveTitle.MilitaryCommanderRankCode) ? oldPersonArchiveTitle.MilitaryCommanderRank.MilitaryCommanderRankName : "", "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" BEGIN
                                  DELETE  FROM VS_OWNER.VS_AR_ZVA WHERE ARZVAID = :PersonArchiveTitleId;
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonArchiveTitleId", OracleType.Number).Value = personArchiveTitleId;

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
