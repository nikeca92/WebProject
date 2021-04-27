using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;

namespace PMIS.Common
{
    public class PersonMilitaryRank : BaseDbObject
    {
        private Person person;
        public int PersonId { get; set; }
        
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

        public string VacAnn { get; set; }

        public DateTime? DateArchive { get; set; }
        public DateTime? DateWhen { get; set; }

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
        /*
        private bool canDelete;
        public bool CanDelete
        {
            get
            {
                return true;
            }
        }
        */
        public PersonMilitaryRank(User user)
            : base(user)
        {
            this.VacAnn = "";
            this.MilitaryCommanderRankCode = "";
        }

        private bool isDR;
        public bool IsDR 
        {
            get{
                return isDR;
            }

            set {
                isDR = value;
            }
        }
    }

    public static class PersonMilitaryRankUtil
    {
        private static PersonMilitaryRank ExtractPersonArchiveTitleFromDR(OracleDataReader dr, User currentUser)
        {
            PersonMilitaryRank personMilitaryRank = new PersonMilitaryRank(currentUser);

            personMilitaryRank.PersonId = DBCommon.GetInt(dr["PersonID"]);
            personMilitaryRank.MilitaryRankId = dr["MilitaryRankId"].ToString();
            personMilitaryRank.VacAnn = dr["VacAnn"].ToString();
            
            if (dr["DateArchive"] is DateTime)
                personMilitaryRank.DateArchive = Convert.ToDateTime(dr["DateArchive"]);
            else
                personMilitaryRank.DateArchive = null;

            if (dr["DateWhen"] is DateTime)
                personMilitaryRank.DateWhen = Convert.ToDateTime(dr["DateWhen"]);
            else
                personMilitaryRank.DateWhen = null;

            personMilitaryRank.MilitaryCommanderRankCode = dr["MilitaryCommanderRankCode"].ToString();

            personMilitaryRank.IsDR = (dr["DR"].ToString() == "Y" ? true : false);

            return personMilitaryRank;
        }

        public static PersonMilitaryRank GetPersonMilitaryRank(int personID, User currentUser)
        {
            PersonMilitaryRank personMilitaryRank = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT 
                                  a.PERSONID as PersonID,
                                  a.KOD_ZVA as MilitaryRankId,
                                  a.Z_Z as VacAnn,
                                  a.Z_ZIZD as DateArchive,
                                  a.Z_ZKOGA as DateWhen,
                                  a.KOD_SPZZ as MilitaryCommanderRankCode,
                                  a.DR
                              FROM VS_OWNER.VS_LS a 
                              WHERE a.PERSONID = :PersonID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("PersonID", OracleType.Number).Value = personID;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    personMilitaryRank = ExtractPersonArchiveTitleFromDR(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return personMilitaryRank;
        }

        public static bool SavePersonMilitaryRank(PersonMilitaryRank personMilitaryRank, Person person, User currentUser, Change changeEntry)
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

                SQL += @"UPDATE VS_OWNER.VS_LS SET
                           KOD_ZVA = :MilitaryRankId,
                           Z_Z = :VacAnn,
                           Z_ZIZD = :DateArchive,
                           Z_ZKOGA = :DateWhen,
                           KOD_SPZZ = :MilitaryCommanderRankCode,
                           DR = :DR
                         WHERE PERSONID = :PersonID;";

                PersonMilitaryRank oldPersonMilitaryRank = GetPersonMilitaryRank(personMilitaryRank.PersonId, currentUser);

                string logDescription = "";
                if(oldPersonMilitaryRank != null &&
                   oldPersonMilitaryRank.MilitaryRank != null)
                    logDescription = "Военно звание: " + oldPersonMilitaryRank.MilitaryRank.LongName + "; " +
                                     "Дата: " + CommonFunctions.FormatDate(oldPersonMilitaryRank.DateWhen);
                else
                    logDescription = "Военно звание: " + personMilitaryRank.MilitaryRank.LongName + "; " +
                                     "Дата: " + CommonFunctions.FormatDate(personMilitaryRank.DateWhen);
                
                changeEvent = new ChangeEvent("RES_Reservist_MilServ_EditMilitaryRan", logDescription, null, person, currentUser);
                

                if (oldPersonMilitaryRank.MilitaryRankId != personMilitaryRank.MilitaryRankId)
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_MilitaryRan_Title", (oldPersonMilitaryRank.MilitaryRank != null ? oldPersonMilitaryRank.MilitaryRank.LongName : ""), personMilitaryRank.MilitaryRank.LongName, currentUser));
                if (oldPersonMilitaryRank.VacAnn != personMilitaryRank.VacAnn)
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_MilitaryRan_VacAnn", oldPersonMilitaryRank.VacAnn, personMilitaryRank.VacAnn, currentUser));
                if (oldPersonMilitaryRank.DateArchive != personMilitaryRank.DateArchive)
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_MilitaryRan_DateArchive", CommonFunctions.FormatDate(oldPersonMilitaryRank.DateWhen), CommonFunctions.FormatDate(personMilitaryRank.DateWhen), currentUser));
                if (oldPersonMilitaryRank.DateWhen != personMilitaryRank.DateWhen)
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_MilitaryRan_DateWhen", CommonFunctions.FormatDate(oldPersonMilitaryRank.DateArchive), CommonFunctions.FormatDate(personMilitaryRank.DateArchive), currentUser));
                if (oldPersonMilitaryRank.MilitaryCommanderRankCode != personMilitaryRank.MilitaryCommanderRankCode)
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_MilitaryRan_MilitaryCommanderRank", !string.IsNullOrEmpty(oldPersonMilitaryRank.MilitaryCommanderRankCode) ? oldPersonMilitaryRank.MilitaryCommanderRank.MilitaryCommanderRankName : "", !string.IsNullOrEmpty(personMilitaryRank.MilitaryCommanderRankCode) ? personMilitaryRank.MilitaryCommanderRank.MilitaryCommanderRankName : "", currentUser));
                if (oldPersonMilitaryRank.IsDR != personMilitaryRank.IsDR)
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilServ_MilitaryRan_DR", (oldPersonMilitaryRank.IsDR ? "1" : "0"), (personMilitaryRank.IsDR ? "1" : "0"), currentUser));

                SQL += @" END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);
                
                OracleParameter param = null;

                param = new OracleParameter();
                param.ParameterName = "PersonID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = personMilitaryRank.PersonId;
                cmd.Parameters.Add(param);                

                param = new OracleParameter();
                param.ParameterName = "MilitaryRankId";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = personMilitaryRank.MilitaryRankId.ToString();
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "VacAnn";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(personMilitaryRank.VacAnn))
                {
                    param.Value = personMilitaryRank.VacAnn;
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
                param.Value = personMilitaryRank.DateArchive.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "DateWhen";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                param.Value = personMilitaryRank.DateWhen.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryCommanderRankCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(personMilitaryRank.MilitaryCommanderRankCode))
                {
                    param.Value = personMilitaryRank.MilitaryCommanderRankCode;
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
                param.Value = personMilitaryRank.IsDR ? "Y" : "N";
                cmd.Parameters.Add(param);


                cmd.ExecuteNonQuery();
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
    }
}
