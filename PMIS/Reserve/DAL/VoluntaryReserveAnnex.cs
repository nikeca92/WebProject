using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    //This class represents a particular record from the VoluntaryReserveAnnexes table
    public class VoluntaryReserveAnnex : BaseDbObject
    {
        private int voluntaryReserveAnnexId;
        private int reservistMilRepStatusId;
        private string annexNumber;
        private DateTime? annexDate;
        private int? annexDurationMonths;
        private DateTime? annexExpireDate;

        public int VoluntaryReserveAnnexId 
        {
            get
            {
                return voluntaryReserveAnnexId;
            }
            set
            {
                voluntaryReserveAnnexId = value;
            }
        }

        public int ReservistMilRepStatusId
        {
            get
            {
                return reservistMilRepStatusId;
            }
            set
            {
                reservistMilRepStatusId = value;
            }
        }

        public string AnnexNumber
        {
            get
            {
                return annexNumber;
            }
            set
            {
                annexNumber = value;
            }
        }

        public DateTime? AnnexDate
        {
            get
            {
                return annexDate;
            }
            set
            {
                annexDate = value;
            }
        }

        public int? AnnexDurationMonths
        {
            get
            {
                return annexDurationMonths;
            }
            set 
            {
                annexDurationMonths = value;
            }
        }

        public DateTime? AnnexExpireDate
        {
            get
            {
                return annexExpireDate;
            }
            set
            {
                annexExpireDate = value;
            }
        }

        public VoluntaryReserveAnnex(User user)
            : base(user)
        {

        }
    }

    public static class VoluntaryReserveAnnexUtil
    {
        //This method creates and returns a VoluntaryReserveAnnex object. It extracts the data from a DataReader.
        //This is done in this way to have a single place of creation of this object, however, pass the particular DataReader
        //object from various queries for better performance instead of executing a new call to the DB
        public static VoluntaryReserveAnnex ExtractVoluntaryReserveAnnex(OracleDataReader dr, User currentUser)
        {
            VoluntaryReserveAnnex voluntaryReserveAnnex = new VoluntaryReserveAnnex(currentUser);

            voluntaryReserveAnnex.VoluntaryReserveAnnexId = DBCommon.GetInt(dr["VoluntaryReserveAnnexID"]);
            voluntaryReserveAnnex.ReservistMilRepStatusId = DBCommon.GetInt(dr["ReservistMilRepStatusID"]);
            voluntaryReserveAnnex.AnnexNumber = dr["AnnexNumber"].ToString();
            voluntaryReserveAnnex.AnnexDate = (dr["AnnexDate"] is DateTime) ? (DateTime)dr["AnnexDate"] : (DateTime?)null;
            voluntaryReserveAnnex.AnnexDurationMonths = (DBCommon.IsInt(dr["AnnexDurationMonths"]) ? DBCommon.GetInt(dr["AnnexDurationMonths"]) : (int?)null);
            voluntaryReserveAnnex.AnnexExpireDate = (dr["AnnexExpireDate"] is DateTime) ? (DateTime)dr["AnnexExpireDate"] : (DateTime?)null;           

            return voluntaryReserveAnnex;
        }

        //Get a VoluntaryReserveAnnex object by voluntaryReserveAnnexID
        public static VoluntaryReserveAnnex GetVoluntaryReserveAnnexByVoluntaryReserveAnnexId(int voluntaryReserveAnnexID, User currentUser)
        {
            VoluntaryReserveAnnex voluntaryReserveAnnex = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.VoluntaryReserveAnnexID,
                                      a.ReservistMilRepStatusID,
                                      a.AnnexNumber,
                                      a.AnnexDate,
                                      a.AnnexDurationMonths,
                                      a.AnnexExpireDate
                               FROM PMIS_RES.VoluntaryReserveAnnexes a
                               WHERE a.VoluntaryReserveAnnexID = :VoluntaryReserveAnnexID 
                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VoluntaryReserveAnnexID", OracleType.Number).Value = voluntaryReserveAnnexID;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    voluntaryReserveAnnex = ExtractVoluntaryReserveAnnex(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return voluntaryReserveAnnex;
        }

        //Get all VoluntaryReserveAnnex objects as list by reservistMilRepStatusId
        public static List<VoluntaryReserveAnnex> GetVoluntaryReserveAnnexesByReservistMilRepStatusId(int reservistMilRepStatusId, User currentUser)
        {
            List<VoluntaryReserveAnnex> voluntaryReserveAnnexes = new List<VoluntaryReserveAnnex>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.VoluntaryReserveAnnexID,
                                      a.ReservistMilRepStatusID,
                                      a.AnnexNumber,
                                      a.AnnexDate,
                                      a.AnnexDurationMonths,
                                      a.AnnexExpireDate
                               FROM PMIS_RES.VoluntaryReserveAnnexes a
                               WHERE a.ReservistMilRepStatusID = :ReservistMilRepStatusID 
                               ORDER BY a.VoluntaryReserveAnnexID ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ReservistMilRepStatusID", OracleType.Number).Value = reservistMilRepStatusId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    voluntaryReserveAnnexes.Add(ExtractVoluntaryReserveAnnex(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return voluntaryReserveAnnexes;
        }

        public static bool SaveVoluntaryReserveAnnex(VoluntaryReserveAnnex voluntaryReserveAnnex, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            ReservistMilRepStatus reservistMilRepStatus = ReservistMilRepStatusUtil.GetReservistMilRepStatus(voluntaryReserveAnnex.ReservistMilRepStatusId, currentUser);
            Reservist reservist = ReservistUtil.GetReservist(reservistMilRepStatus.ReservistId, currentUser);
            Person person = reservist.Person;

            try
            {
                SQL = @"BEGIN
                                
                               ";
                if (voluntaryReserveAnnex.VoluntaryReserveAnnexId == 0)
                {
                    SQL += @"INSERT INTO PMIS_RES.VoluntaryReserveAnnexes
                                    (ReservistMilRepStatusID,
                                     AnnexNumber,     
                                     AnnexDate,
                                     AnnexDurationMonths,
                                     AnnexExpireDate)
                            VALUES  (:ReservistMilRepStatusID,
                                     :AnnexNumber,
                                     :AnnexDate,
                                     :AnnexDurationMonths,
                                     :AnnexExpireDate);
                                    
                            SELECT PMIS_RES.VoluntaryReserveAnnexes_ID_SEQ.currval INTO :VoluntaryReserveAnnexID FROM dual;
                                    
                            ";

                    changeEvent = new ChangeEvent("RES_Reservist_MilRepStatus_AddVoluntaryReserveAnnex", null, null, person, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_AnnexNumber", "", voluntaryReserveAnnex.AnnexNumber, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_AnnexDate", "", CommonFunctions.FormatDate(voluntaryReserveAnnex.AnnexDate), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_AnnexDurationMonths", "",voluntaryReserveAnnex.AnnexDurationMonths.HasValue ? voluntaryReserveAnnex.AnnexDurationMonths.Value.ToString() : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_AnnexExpireDate", "", CommonFunctions.FormatDate(voluntaryReserveAnnex.AnnexExpireDate), currentUser));                                   
                }
                else
                {
                    SQL += @"UPDATE PMIS_RES.VoluntaryReserveAnnexes SET
                                             ReservistMilRepStatusID = :ReservistMilRepStatusID,
                                             AnnexNumber = :AnnexNumber,
                                             AnnexDate = :AnnexDate,
                                             AnnexDurationMonths = :AnnexDurationMonths,
                                             AnnexExpireDate = :AnnexExpireDate
                                    WHERE VoluntaryReserveAnnexID = :VoluntaryReserveAnnexID ;                       
        
                                    ";

                    changeEvent = new ChangeEvent("RES_Reservist_MilRepStatus_EditVoluntaryReserveAnnex", null, null, person, currentUser);

                    VoluntaryReserveAnnex oldVoluntaryReserveAnnex = VoluntaryReserveAnnexUtil.GetVoluntaryReserveAnnexByVoluntaryReserveAnnexId(voluntaryReserveAnnex.VoluntaryReserveAnnexId, currentUser);

                    if (oldVoluntaryReserveAnnex.AnnexNumber.Trim() != voluntaryReserveAnnex.AnnexNumber.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_AnnexNumber", oldVoluntaryReserveAnnex.AnnexNumber, voluntaryReserveAnnex.AnnexNumber, currentUser));

                    if (oldVoluntaryReserveAnnex.AnnexDate != voluntaryReserveAnnex.AnnexDate)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_AnnexDate", CommonFunctions.FormatDate(oldVoluntaryReserveAnnex.AnnexDate), CommonFunctions.FormatDate(voluntaryReserveAnnex.AnnexDate), currentUser));

                    if (oldVoluntaryReserveAnnex.AnnexDurationMonths != voluntaryReserveAnnex.AnnexDurationMonths)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_AnnexDurationMonths", oldVoluntaryReserveAnnex.AnnexDurationMonths.HasValue ? oldVoluntaryReserveAnnex.AnnexDurationMonths.Value.ToString() : "", voluntaryReserveAnnex.AnnexDurationMonths.HasValue ? voluntaryReserveAnnex.AnnexDurationMonths.Value.ToString() : "", currentUser));

                    if (oldVoluntaryReserveAnnex.AnnexExpireDate != voluntaryReserveAnnex.AnnexExpireDate)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_AnnexExpireDate", CommonFunctions.FormatDate(oldVoluntaryReserveAnnex.AnnexExpireDate), CommonFunctions.FormatDate(voluntaryReserveAnnex.AnnexExpireDate), currentUser)); 
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramVoluntaryReserveAnnexID = new OracleParameter();
                paramVoluntaryReserveAnnexID.ParameterName = "VoluntaryReserveAnnexID";
                paramVoluntaryReserveAnnexID.OracleType = OracleType.Number;

                if (voluntaryReserveAnnex.VoluntaryReserveAnnexId != 0)
                {
                    paramVoluntaryReserveAnnexID.Direction = ParameterDirection.Input;
                    paramVoluntaryReserveAnnexID.Value = voluntaryReserveAnnex.VoluntaryReserveAnnexId;
                }
                else
                {
                    paramVoluntaryReserveAnnexID.Direction = ParameterDirection.Output;
                }
                cmd.Parameters.Add(paramVoluntaryReserveAnnexID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "ReservistMilRepStatusID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = voluntaryReserveAnnex.ReservistMilRepStatusId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AnnexNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(voluntaryReserveAnnex.AnnexNumber))
                    param.Value = voluntaryReserveAnnex.AnnexNumber;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AnnexDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (voluntaryReserveAnnex.AnnexDate.HasValue)
                    param.Value = voluntaryReserveAnnex.AnnexDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AnnexDurationMonths";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (voluntaryReserveAnnex.AnnexDurationMonths.HasValue)
                    param.Value = voluntaryReserveAnnex.AnnexDurationMonths.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AnnexExpireDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (voluntaryReserveAnnex.AnnexExpireDate.HasValue)
                    param.Value = voluntaryReserveAnnex.AnnexExpireDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (voluntaryReserveAnnex.VoluntaryReserveAnnexId == 0)
                    voluntaryReserveAnnex.VoluntaryReserveAnnexId = DBCommon.GetInt(paramVoluntaryReserveAnnexID.Value);

                result = true;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                if (changeEvent.ChangeEventDetails.Count > 0)
                    changeEntry.AddEvent(changeEvent);
            }

            return result;
        }

        public static void DeleteVoluntaryReserveAnnex(int voluntaryReserveAnnexId, User currentUser, Change changeEntry)
        {
            ChangeEvent changeEvent = null;

            VoluntaryReserveAnnex voluntaryReserveAnnex = VoluntaryReserveAnnexUtil.GetVoluntaryReserveAnnexByVoluntaryReserveAnnexId(voluntaryReserveAnnexId, currentUser);

            string logDescription = "";
            //logDescription += "Доп. сп. №: " + voluntaryReserveAnnex.AnnexNumber;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string SQL = "";

            try
            {

                SQL += @"BEGIN                            
                            DELETE FROM PMIS_RES.VoluntaryReserveAnnexes 
                            WHERE VoluntaryReserveAnnexID = :VoluntaryReserveAnnexID;
                         END;";

                changeEvent = new ChangeEvent("RES_Reservist_MilRepStatus_DeleteVoluntaryReserveAnnex", logDescription, null, null, currentUser);

                changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_AnnexNumber", voluntaryReserveAnnex.AnnexNumber, "", currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_AnnexDate", CommonFunctions.FormatDate(voluntaryReserveAnnex.AnnexDate), "", currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_AnnexDurationMonths", voluntaryReserveAnnex.AnnexDurationMonths.HasValue ? voluntaryReserveAnnex.AnnexDurationMonths.Value.ToString() : "", "", currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("RES_Reservist_MilRepStatus_AnnexExpireDate", CommonFunctions.FormatDate(voluntaryReserveAnnex.AnnexExpireDate), "", currentUser));                                   

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("VoluntaryReserveAnnexID", OracleType.Number).Value = voluntaryReserveAnnexId;

                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }

            if (changeEvent != null && changeEvent.ChangeEventDetails.Count > 0)
                changeEntry.AddEvent(changeEvent);
        }
    }
}