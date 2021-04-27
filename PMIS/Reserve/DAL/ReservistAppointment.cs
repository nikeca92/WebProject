using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    //This class represents a particular Reservist Appointment
    public class ReservistAppointment : BaseDbObject
    {
        private int reservistAppointmentId;
        private int reservistId;
        private bool isCurrent;
        private string reqOrderNumber;
        private int? equipmentReservistsRequestId;
        private DateTime? receiptAppointmentDate;
        private string militaryCommandName;
        private string militaryCommandSuffix;
        private int? militaryCommandId;
        private int? reservistReadinessId;
        private string militaryRankName;
        private string militaryRankId;
        private string milReportingSpecialityName;
        private string milReportingSpecialityCode;
        private int? milReportSpecialityId;
        private string position;
        private decimal? appointmentTime;
        private string appointmentPlace;
        private int? fillReservistsRequestId;

        public int ReservistAppointmentId
        {
            get
            {
                return reservistAppointmentId;
            }
            set
            {
                reservistAppointmentId = value;
            }
        }

        public int ReservistId
        {
            get
            {
                return reservistId;
            }
            set
            {
                reservistId = value;
            }
        }

        public bool IsCurrent
        {
            get
            {
                return isCurrent;
            }
            set
            {
                isCurrent = value;
            }
        }

        public string ReqOrderNumber
        {
            get
            {
                return reqOrderNumber;
            }
            set
            {
                reqOrderNumber = value;
            }
        }

        public int? EquipmentReservistsRequestId
        {
            get
            {
                return equipmentReservistsRequestId;
            }
            set
            {
                equipmentReservistsRequestId = value;
            }
        }

        public DateTime? ReceiptAppointmentDate
        {
            get
            {
                return receiptAppointmentDate;
            }
            set
            {
                receiptAppointmentDate = value;
            }
        }

        public string MilitaryCommandName
        {
            get
            {
                return militaryCommandName;
            }
            set
            {
                militaryCommandName = value;
            }
        }

        public string MilitaryCommandSuffix
        {
            get
            {
                return militaryCommandSuffix;
            }
            set
            {
                militaryCommandSuffix = value;
            }
        }

        public int? MilitaryCommandId
        {
            get
            {
                return militaryCommandId;
            }
            set
            {
                militaryCommandId = value;
            }
        }

        public int? ReservistReadinessId
        {
            get
            {
                return reservistReadinessId;
            }
            set
            {
                reservistReadinessId = value;
            }
        }

        public string MilitaryRankName
        {
            get
            {
                return militaryRankName;
            }
            set
            {
                militaryRankName = value;
            }
        }

        public string MilitaryRankId
        {
            get
            {
                return militaryRankId;
            }
            set
            {
                militaryRankId = value;
            }
        }

        public string MilReportingSpecialityName
        {
            get
            {
                return milReportingSpecialityName;
            }
            set
            {
                milReportingSpecialityName = value;
            }
        }

        public string MilReportingSpecialityCode
        {
            get
            {
                return milReportingSpecialityCode;
            }
            set
            {
                milReportingSpecialityCode = value;
            }
        }

        public int? MilReportSpecialityId
        {
            get
            {
                return milReportSpecialityId;
            }
            set
            {
                milReportSpecialityId = value;
            }
        }

        public string Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        public decimal? AppointmentTime
        {
            get
            {
                return appointmentTime;
            }
            set
            {
                appointmentTime = value;
            }
        }

        public string AppointmentPlace
        {
            get
            {
                return appointmentPlace;
            }
            set
            {
                appointmentPlace = value;
            }
        }

        public int? FillReservistsRequestId
        {
            get
            {
                return fillReservistsRequestId;
            }
            set
            {
                fillReservistsRequestId = value;
            }
        }


        public ReservistAppointment(User user)
            : base(user)
        {

        }
    }

    public static class ReservistAppointmentUtil
    {
        //This method creates and returns a MilitaryReportStatus object. It extracts the data from a DataReader.
        //This is done in this way to have a signle place of creation of this object, however, pass the particular DataReader
        //object from various queries for better performance instead of executing a new call to the DB
        public static ReservistAppointment ExtractReservistAppointment(OracleDataReader dr, User currentUser)
        {
            ReservistAppointment reservistAppointment = new ReservistAppointment(currentUser);

            reservistAppointment.ReservistAppointmentId = DBCommon.GetInt(dr["ReservistAppointmentID"]);
            reservistAppointment.ReservistId = DBCommon.GetInt(dr["ReservistID"]);
            reservistAppointment.IsCurrent = (DBCommon.IsInt(dr["IsCurrent"]) && DBCommon.GetInt(dr["IsCurrent"]) == 1);
            reservistAppointment.ReqOrderNumber = dr["ReqOrderNumber"].ToString();
            reservistAppointment.EquipmentReservistsRequestId = (DBCommon.IsInt(dr["EquipmentReservistsRequestID"]) ? DBCommon.GetInt(dr["EquipmentReservistsRequestID"]) : (int?)null);
            reservistAppointment.ReceiptAppointmentDate = (dr["ReceiptAppointmentDate"] is DateTime) ? (DateTime)dr["ReceiptAppointmentDate"] : (DateTime?)null;
            reservistAppointment.MilitaryCommandName = dr["MilitaryCommandName"].ToString();
            reservistAppointment.MilitaryCommandSuffix = dr["MilitaryCommandSuffix"].ToString();
            reservistAppointment.MilitaryCommandId = (DBCommon.IsInt(dr["MilitaryCommandID"]) ? DBCommon.GetInt(dr["MilitaryCommandID"]) : (int?)null);
            reservistAppointment.ReservistReadinessId = (DBCommon.IsInt(dr["ReservistReadinessID"]) ? DBCommon.GetInt(dr["ReservistReadinessID"]) : (int?)null);
            reservistAppointment.MilitaryRankName = dr["MilitaryRankName"].ToString();
            reservistAppointment.MilitaryRankId = dr["MilitaryRankID"].ToString();
            reservistAppointment.MilReportingSpecialityName = dr["MilReportingSpecialityName"].ToString();
            reservistAppointment.MilReportingSpecialityCode = dr["MilReportingSpecialityCode"].ToString();
            reservistAppointment.MilReportSpecialityId = (DBCommon.IsInt(dr["MilReportSpecialityID"]) ? DBCommon.GetInt(dr["MilReportSpecialityID"]) : (int?)null);
            reservistAppointment.Position = dr["Position"].ToString();
            reservistAppointment.AppointmentTime = (DBCommon.IsDecimal(dr["AppointmentTime"]) ? DBCommon.GetDecimal(dr["AppointmentTime"]) : (decimal?)null);
            reservistAppointment.AppointmentPlace = dr["AppointmentPlace"].ToString();
            reservistAppointment.FillReservistsRequestId = (DBCommon.IsInt(dr["FillReservistsRequestID"]) ? DBCommon.GetInt(dr["FillReservistsRequestID"]) : (int?)null);

            BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, reservistAppointment);

            return reservistAppointment;
        }

        //Get a particular object by its ID
        public static ReservistAppointment GetReservistAppointment(int reservistAppointmentId, User currentUser)
        {
            ReservistAppointment reservistAppointment = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.ReservistAppointmentID,
                                      a.ReservistID,
                                      a.IsCurrent,
                                        
                                      a.ReqOrderNumber,
                                      a.EquipmentReservistsRequestID, 
                                      a.ReceiptAppointmentDate,
                                      a.MilitaryCommandName, 
                                      a.MilitaryCommandSuffix, 
                                      a.MilitaryCommandID,
                                      a.ReservistReadinessID,
                                      a.MilitaryRankName,
                                      a.MilitaryRankID,
                                      a.MilReportingSpecialityName,
                                      a.MilReportingSpecialityCode,
                                      a.MilReportSpecialityID,
                                      a.Position,
                                      a.AppointmentTime,
                                      a.AppointmentPlace,
                                      a.FillReservistsRequestID,

                                      a.CreatedBy,
                                      a.CreatedDate,
                                      a.LastModifiedBy,
                                      a.LastModifiedDate
                               FROM PMIS_RES.ReservistAppointments a
                               WHERE a.ReservistAppointmentID = :ReservistAppointmentID ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ReservistAppointmentID", OracleType.Number).Value = reservistAppointmentId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    reservistAppointment = ExtractReservistAppointment(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return reservistAppointment;
        }

        //Get the current appointment by ReservistID
        public static ReservistAppointment GetCurrentReservistAppointmentByReservistId(int reservistId, User currentUser)
        {
            ReservistAppointment reservistAppointment = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.ReservistAppointmentID,
                                      a.ReservistID,
                                      a.IsCurrent,
                                        
                                      a.ReqOrderNumber,
                                      a.EquipmentReservistsRequestID, 
                                      a.ReceiptAppointmentDate,
                                      a.MilitaryCommandName, 
                                      a.MilitaryCommandSuffix, 
                                      a.MilitaryCommandID,
                                      a.ReservistReadinessID,
                                      a.MilitaryRankName,
                                      a.MilitaryRankID,
                                      a.MilReportingSpecialityName,
                                      a.MilReportingSpecialityCode,
                                      a.MilReportSpecialityID,
                                      a.Position,
                                      a.AppointmentTime,
                                      a.AppointmentPlace,
                                      a.FillReservistsRequestID,

                                      a.CreatedBy,
                                      a.CreatedDate,
                                      a.LastModifiedBy,
                                      a.LastModifiedDate
                               FROM PMIS_RES.ReservistAppointments a
                               WHERE a.ReservistId = :ReservistId AND a.IsCurrent = 1 ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ReservistId", OracleType.Number).Value = reservistId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    reservistAppointment = ExtractReservistAppointment(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return reservistAppointment;
        }

        //Get all appointments (history) by Reservist
        public static List<ReservistAppointment> GetAllReservistAppointmentsByReservistId(int reservistId, User currentUser)
        {
            List<ReservistAppointment> reservistAppointments = new List<ReservistAppointment>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.ReservistAppointmentID,
                                      a.ReservistID,
                                      a.IsCurrent,
                                        
                                      a.ReqOrderNumber,
                                      a.EquipmentReservistsRequestID, 
                                      a.ReceiptAppointmentDate,
                                      a.MilitaryCommandName, 
                                      a.MilitaryCommandSuffix, 
                                      a.MilitaryCommandID,
                                      a.ReservistReadinessID,
                                      a.MilitaryRankName,
                                      a.MilitaryRankID,
                                      a.MilReportingSpecialityName,
                                      a.MilReportingSpecialityCode,
                                      a.MilReportSpecialityID,
                                      a.Position,
                                      a.AppointmentTime,
                                      a.AppointmentPlace,
                                      a.FillReservistsRequestID,

                                      a.CreatedBy,
                                      a.CreatedDate,
                                      a.LastModifiedBy,
                                      a.LastModifiedDate
                               FROM PMIS_RES.ReservistAppointments a
                               WHERE a.ReservistID = :ReservistID 
                               ORDER BY a.ReservistAppointmentID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ReservistID", OracleType.Number).Value = reservistId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    reservistAppointments.Add(ExtractReservistAppointment(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return reservistAppointments;
        }

        //Get all appointments (history) by Reservist with pagination
        public static List<ReservistAppointment> GetAllReservistAppointmentsByReservistId(int reservistId, int orderBy, int pageIdx, int rowsPerPage, User currentUser)
        {
            List<ReservistAppointment> reservistAppointments = new List<ReservistAppointment>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string pageWhere = "";

                if (pageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE tmp.RowNumber BETWEEN (" + pageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + pageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

                string orderBySQL = "a.ReceiptAppointmentDate";
                string orderByDir = "ASC";

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                               SELECT a.ReservistAppointmentID,
                                                      a.ReservistID,
                                                      a.IsCurrent,
                                                        
                                                      a.ReqOrderNumber,
                                                      a.EquipmentReservistsRequestID, 
                                                      a.ReceiptAppointmentDate,
                                                      a.MilitaryCommandName, 
                                                      a.MilitaryCommandSuffix, 
                                                      a.MilitaryCommandID,
                                                      a.ReservistReadinessID,
                                                      a.MilitaryRankName,
                                                      a.MilitaryRankID,
                                                      a.MilReportingSpecialityName,
                                                      a.MilReportingSpecialityCode,
                                                      a.MilReportSpecialityID,
                                                      a.Position,
                                                      a.AppointmentTime,
                                                      a.AppointmentPlace,
                                                      a.FillReservistsRequestID,

                                                      a.CreatedBy,
                                                      a.CreatedDate,
                                                      a.LastModifiedBy,
                                                      a.LastModifiedDate,
                                                      RANK() OVER (ORDER BY " + orderBySQL + @", a.ReservistAppointmentID) as RowNumber
                                               FROM PMIS_RES.ReservistAppointments a
                                               WHERE a.ReservistID = :ReservistID 
                                               ORDER BY " + orderBySQL + @", a.ReservistAppointmentID
                                            ) tmp
                                            " + pageWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ReservistID", OracleType.Number).Value = reservistId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    reservistAppointments.Add(ExtractReservistAppointment(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return reservistAppointments;
        }

        //Get all appointments (history) count by Reservist for pagination
        public static int GetAllReservistAppointmentsByReservistIdCount(int reservistId, User currentUser)
        {
            int reservistAppointments = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {             
                string SQL = @"SELECT COUNT(*) as Cnt
                               FROM PMIS_RES.ReservistAppointments a
                               WHERE a.ReservistID = :ReservistID                                                                                            
                              ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ReservistID", OracleType.Number).Value = reservistId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        reservistAppointments = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return reservistAppointments;
        }

        //Add a particular Reservist Appointment into the DB
        public static bool AddReservistAppointment(ReservistAppointment reservistAppointment, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL += @"BEGIN
                            UPDATE PMIS_RES.ReservistAppointments SET
                               IsCurrent = 0
                            WHERE ReservistID = :ReservistID;

                            INSERT INTO PMIS_RES.ReservistAppointments (
                                ReservistID,
                                IsCurrent,
                                
                                ReqOrderNumber,
                                EquipmentReservistsRequestID, 
                                ReceiptAppointmentDate,
                                MilitaryCommandName, 
                                MilitaryCommandSuffix, 
                                MilitaryCommandID,
                                ReservistReadinessID,
                                MilitaryRankName,
                                MilitaryRankID,
                                MilReportingSpecialityName,
                                MilReportingSpecialityCode,
                                MilReportSpecialityID,
                                Position,
                                AppointmentTime,
                                AppointmentPlace,
                                FillReservistsRequestID,
                                
                                CreatedBy,
                                CreatedDate,
                                LastModifiedBy,
                                LastModifiedDate
                            )
                            VALUES (
                                :ReservistID,
                                :IsCurrent,
                                
                                :ReqOrderNumber,
                                :EquipmentReservistsRequestID, 
                                :ReceiptAppointmentDate,
                                :MilitaryCommandName, 
                                :MilitaryCommandSuffix, 
                                :MilitaryCommandID,
                                :ReservistReadinessID,
                                :MilitaryRankName,
                                :MilitaryRankID,
                                :MilReportingSpecialityName,
                                :MilReportingSpecialityCode,
                                :MilReportSpecialityID,
                                :Position,
                                :AppointmentTime,
                                :AppointmentPlace,
                                :FillReservistsRequestID,
                                
                                :CreatedBy,
                                :CreatedDate,
                                :LastModifiedBy,
                                :LastModifiedDate
                            );

                            SELECT PMIS_RES.ReservistAppointments_ID_SEQ.currval INTO :ReservistAppointmentID FROM dual;
                         END;
                        ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramReservistAppointmentID = new OracleParameter();
                paramReservistAppointmentID.ParameterName = "ReservistAppointmentID";
                paramReservistAppointmentID.OracleType = OracleType.Number;
                paramReservistAppointmentID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(paramReservistAppointmentID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "ReservistID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = reservistAppointment.ReservistId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "IsCurrent";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = reservistAppointment.IsCurrent ? 1 : 0;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ReqOrderNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(reservistAppointment.ReqOrderNumber))
                    param.Value = reservistAppointment.ReqOrderNumber;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "EquipmentReservistsRequestID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (reservistAppointment.EquipmentReservistsRequestId.HasValue)
                    param.Value = reservistAppointment.EquipmentReservistsRequestId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ReceiptAppointmentDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (reservistAppointment.ReceiptAppointmentDate.HasValue)
                    param.Value = reservistAppointment.ReceiptAppointmentDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryCommandName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(reservistAppointment.MilitaryCommandName))
                    param.Value = reservistAppointment.MilitaryCommandName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryCommandSuffix";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(reservistAppointment.MilitaryCommandSuffix))
                    param.Value = reservistAppointment.MilitaryCommandSuffix;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryCommandID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (reservistAppointment.MilitaryCommandId.HasValue)
                    param.Value = reservistAppointment.MilitaryCommandId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ReservistReadinessID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (reservistAppointment.ReservistReadinessId.HasValue)
                    param.Value = reservistAppointment.ReservistReadinessId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryRankName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(reservistAppointment.MilitaryRankName))
                    param.Value = reservistAppointment.MilitaryRankName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryRankID";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(reservistAppointment.MilitaryRankId))
                    param.Value = reservistAppointment.MilitaryRankId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilReportingSpecialityName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(reservistAppointment.MilReportingSpecialityName))
                    param.Value = reservistAppointment.MilReportingSpecialityName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilReportingSpecialityCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(reservistAppointment.MilReportingSpecialityCode))
                    param.Value = reservistAppointment.MilReportingSpecialityCode;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilReportSpecialityID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (reservistAppointment.MilReportSpecialityId.HasValue)
                    param.Value = reservistAppointment.MilReportSpecialityId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Position";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(reservistAppointment.Position))
                    param.Value = reservistAppointment.Position;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AppointmentTime";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (reservistAppointment.AppointmentTime.HasValue)
                    param.Value = reservistAppointment.AppointmentTime.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AppointmentPlace";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(reservistAppointment.AppointmentPlace))
                    param.Value = reservistAppointment.AppointmentPlace;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "FillReservistsRequestID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (reservistAppointment.FillReservistsRequestId.HasValue)
                    param.Value = reservistAppointment.FillReservistsRequestId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                BaseDbObjectUtil.SetCreatedParams(cmd, currentUser);
                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();

                reservistAppointment.ReservistAppointmentId = DBCommon.GetInt(paramReservistAppointmentID.Value);

                //Changes log
                Reservist reservist = ReservistUtil.GetReservist(reservistAppointment.ReservistId, currentUser);
                changeEvent = new ChangeEvent("RES_Reservist_AddResAppointment", "", null, reservist.Person, currentUser);

                changeEvent.AddDetail(new ChangeEventDetail("RES_ResAppointment_ReqOrderNumber", "", reservistAppointment.ReqOrderNumber, currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("RES_ResAppointment_ReceiptAppointmentDate", "", CommonFunctions.FormatDate(reservistAppointment.ReceiptAppointmentDate), currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("RES_ResAppointment_MilitaryCommand", "", reservistAppointment.MilitaryCommandName + (String.IsNullOrEmpty(reservistAppointment.MilitaryCommandSuffix) ? "" : " ") + reservistAppointment.MilitaryCommandSuffix, currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("RES_ResAppointment_ReservistReadiness", "", ReadinessUtil.ReadinessName(reservistAppointment.ReservistReadinessId.Value), currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("RES_ResAppointment_MilitaryRank", "", reservistAppointment.MilitaryRankName, currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("RES_ResAppointment_MilReportingSpeciality", "", reservistAppointment.MilReportingSpecialityCode + " " + reservistAppointment.MilReportingSpecialityName, currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("RES_ResAppointment_Position", "", reservistAppointment.Position, currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("RES_ResAppointment_AppointmentTime", "", reservistAppointment.AppointmentTime.ToString(), currentUser));

                result = true;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                if (changeEvent != null && changeEvent.ChangeEventDetails.Count > 0)
                {
                    changeEntry.AddEvent(changeEvent);
                    ReservistUtil.SetReservistModified(reservistAppointment.ReservistId, currentUser);
                }
            }

            return result;
        }
       
        public static void RefreshReservistAppointment(List<FillReservistsRequest> reservistsRequests, User currentUser, Change changeEntry)
        {
            foreach (FillReservistsRequest reservistsRequest in reservistsRequests)
            {
                ReservistAppointment reservistAppointment = ReservistAppointmentUtil.GetCurrentReservistAppointmentByReservistId(reservistsRequest.ReservistID, currentUser);
                RequestCommandPosition requestCommandPosition = RequestCommandPositionUtil.GetRequestCommandPosition(currentUser, reservistsRequest.RequestCommandPositionID);
                MilitaryReportSpeciality fulfilMRS = reservistsRequest.MilitaryReportSpeciality;

                reservistAppointment.ReservistId = reservistsRequest.ReservistID;
                reservistAppointment.IsCurrent = true;
                reservistAppointment.ReqOrderNumber = requestCommandPosition.RequestsCommand.EquipmentReservistsRequest.RequestNumber;
                reservistAppointment.EquipmentReservistsRequestId = requestCommandPosition.RequestsCommand.EquipmentReservistsRequestId;
                //reservistAppointment.ReceiptAppointmentDate = DateTime.Now; //TODO? Should we change this?
                reservistAppointment.MilitaryCommandName = requestCommandPosition.RequestsCommand.MilitaryCommand.DisplayTextForSelection;
                reservistAppointment.MilitaryCommandSuffix = requestCommandPosition.RequestsCommand.MilitaryCommandSuffix;
                reservistAppointment.MilitaryCommandId = requestCommandPosition.RequestsCommand.MilitaryCommand.MilitaryCommandId;
                reservistAppointment.ReservistReadinessId = reservistsRequest.ReservistReadinessID;
                reservistAppointment.MilitaryRankName = (requestCommandPosition.MilitaryRanks != null && requestCommandPosition.MilitaryRanks.Count > 0 ? requestCommandPosition.MilitaryRanks[0].Rank.LongName : ""); //TODO Is this correct to get 1st?
                reservistAppointment.MilitaryRankId = (requestCommandPosition.MilitaryRanks != null && requestCommandPosition.MilitaryRanks.Count > 0 ? requestCommandPosition.MilitaryRanks[0].Rank.MilitaryRankId : "");
                reservistAppointment.MilReportingSpecialityName = (fulfilMRS != null ? fulfilMRS.MilReportingSpecialityName : "");
                reservistAppointment.MilReportingSpecialityCode = (fulfilMRS != null ? fulfilMRS.MilReportingSpecialityCode : "");
                reservistAppointment.MilReportSpecialityId = (fulfilMRS != null ? fulfilMRS.MilReportSpecialityId : (int?)null);
                reservistAppointment.Position = requestCommandPosition.Position;
                reservistAppointment.AppointmentTime = requestCommandPosition.RequestsCommand.AppointmentTime;
                //reservistAppointment.AppointmentPlace = ""; //TODO Currently we do not support this
                reservistAppointment.FillReservistsRequestId = reservistsRequest.FillReservistsRequestID;

                ReservistAppointmentUtil.UpdateReservistAppointment(reservistAppointment, currentUser, changeEntry);
            }
        }

        //Update a particular Reservist Appointment from the DB
        public static bool UpdateReservistAppointment(ReservistAppointment reservistAppointment, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {   
                SQL += @"BEGIN

                             UPDATE PMIS_RES.ReservistAppointments
                            SET ReservistID = :ReservistID,
                                IsCurrent = :IsCurrent,
                                ReqOrderNumber = :ReqOrderNumber,
                                EquipmentReservistsRequestID = :EquipmentReservistsRequestID, 
                                
                                MilitaryCommandName = :MilitaryCommandName, 
                                MilitaryCommandSuffix = :MilitaryCommandSuffix, 
                                MilitaryCommandID = :MilitaryCommandID,
                                ReservistReadinessID = :ReservistReadinessID,
                                MilitaryRankName = :MilitaryRankName,
                                MilitaryRankID = :MilitaryRankID,
                                MilReportingSpecialityName = :MilReportingSpecialityName,
                                MilReportingSpecialityCode = :MilReportingSpecialityCode,
                                MilReportSpecialityID = :MilReportSpecialityID,
                                Position = :Position,
                                AppointmentTime = :AppointmentTime,
                                
                                FillReservistsRequestID = :FillReservistsRequestID,
                                CreatedBy = :CreatedBy,
                                CreatedDate = :CreatedDate,
                                LastModifiedBy = :LastModifiedBy,
                                LastModifiedDate = :LastModifiedDate
                            WHERE ReservistID = :ReservistID AND
                                  IsCurrent = 1 ;

                         END;
                        ";
                
                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);
                                
                OracleParameter param = new OracleParameter();
                param.ParameterName = "ReservistID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = reservistAppointment.ReservistId;
                cmd.Parameters.Add(param);
               
                param = new OracleParameter();
                param.ParameterName = "IsCurrent";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = reservistAppointment.IsCurrent ? 1 : 0;
                cmd.Parameters.Add(param);
                
                param = new OracleParameter();
                param.ParameterName = "ReqOrderNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(reservistAppointment.ReqOrderNumber))
                    param.Value = reservistAppointment.ReqOrderNumber;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);
               
                param = new OracleParameter();
                param.ParameterName = "EquipmentReservistsRequestID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (reservistAppointment.EquipmentReservistsRequestId.HasValue)
                    param.Value = reservistAppointment.EquipmentReservistsRequestId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);
                
                param = new OracleParameter();
                param.ParameterName = "MilitaryCommandName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(reservistAppointment.MilitaryCommandName))
                    param.Value = reservistAppointment.MilitaryCommandName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryCommandSuffix";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(reservistAppointment.MilitaryCommandSuffix))
                    param.Value = reservistAppointment.MilitaryCommandSuffix;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryCommandID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (reservistAppointment.MilitaryCommandId.HasValue)
                    param.Value = reservistAppointment.MilitaryCommandId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ReservistReadinessID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (reservistAppointment.ReservistReadinessId.HasValue)
                    param.Value = reservistAppointment.ReservistReadinessId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryRankName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(reservistAppointment.MilitaryRankName))
                    param.Value = reservistAppointment.MilitaryRankName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryRankID";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(reservistAppointment.MilitaryRankId))
                    param.Value = reservistAppointment.MilitaryRankId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilReportingSpecialityName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(reservistAppointment.MilReportingSpecialityName))
                    param.Value = reservistAppointment.MilReportingSpecialityName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilReportingSpecialityCode";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(reservistAppointment.MilReportingSpecialityCode))
                    param.Value = reservistAppointment.MilReportingSpecialityCode;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilReportSpecialityID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (reservistAppointment.MilReportSpecialityId.HasValue)
                    param.Value = reservistAppointment.MilReportSpecialityId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Position";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(reservistAppointment.Position))
                    param.Value = reservistAppointment.Position;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AppointmentTime";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (reservistAppointment.AppointmentTime.HasValue)
                    param.Value = reservistAppointment.AppointmentTime.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);
                                
                param = new OracleParameter();
                param.ParameterName = "FillReservistsRequestID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (reservistAppointment.FillReservistsRequestId.HasValue)
                    param.Value = reservistAppointment.FillReservistsRequestId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "CreatedBy";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = reservistAppointment.CreatedByUserId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "CreatedDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                param.Value = reservistAppointment.CreatedDate;
                cmd.Parameters.Add(param);
                
                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);
                
                //Changes log
                Reservist reservist = ReservistUtil.GetReservist(reservistAppointment.ReservistId, currentUser);
                changeEvent = new ChangeEvent("RES_Reservist_UpdateResAppointment", "", null, reservist.Person, currentUser);

                ReservistAppointment oldReservistAppointment = ReservistAppointmentUtil.GetCurrentReservistAppointmentByReservistId(reservistAppointment.ReservistId, currentUser);

                if (oldReservistAppointment.ReqOrderNumber.Trim() != reservistAppointment.ReqOrderNumber.Trim())
                    changeEvent.AddDetail(new ChangeEventDetail("RES_ResAppointment_ReqOrderNumber", oldReservistAppointment.ReqOrderNumber, reservistAppointment.ReqOrderNumber, currentUser));
                              
                if (oldReservistAppointment.MilitaryCommandName.Trim() != reservistAppointment.MilitaryCommandName.Trim() ||
                    oldReservistAppointment.MilitaryCommandSuffix.Trim() != reservistAppointment.MilitaryCommandSuffix.Trim())
                    changeEvent.AddDetail(new ChangeEventDetail("RES_ResAppointment_MilitaryCommand", oldReservistAppointment.MilitaryCommandName + (String.IsNullOrEmpty(oldReservistAppointment.MilitaryCommandSuffix) ? "" : " ") + oldReservistAppointment.MilitaryCommandSuffix, reservistAppointment.MilitaryCommandName + (String.IsNullOrEmpty(reservistAppointment.MilitaryCommandSuffix) ? "" : " ") + reservistAppointment.MilitaryCommandSuffix, currentUser));
                
                if(oldReservistAppointment.MilitaryRankName != reservistAppointment.MilitaryRankName)
                    changeEvent.AddDetail(new ChangeEventDetail("RES_ResAppointment_MilitaryRank", oldReservistAppointment.MilitaryRankName, reservistAppointment.MilitaryRankName, currentUser));

                if (oldReservistAppointment.MilReportingSpecialityCode != reservistAppointment.MilReportingSpecialityCode ||
                    oldReservistAppointment.MilReportingSpecialityName != reservistAppointment.MilReportingSpecialityName)
                    changeEvent.AddDetail(new ChangeEventDetail("RES_ResAppointment_MilReportingSpeciality", oldReservistAppointment.MilReportingSpecialityCode + " " + oldReservistAppointment.MilReportingSpecialityName, reservistAppointment.MilReportingSpecialityCode + " " + reservistAppointment.MilReportingSpecialityName , currentUser));

                if (oldReservistAppointment.Position.Trim() != reservistAppointment.Position.Trim())
                    changeEvent.AddDetail(new ChangeEventDetail("RES_ResAppointment_Position", oldReservistAppointment.Position, reservistAppointment.Position, currentUser));

                if(oldReservistAppointment.AppointmentTime != reservistAppointment.AppointmentTime)
                    changeEvent.AddDetail(new ChangeEventDetail("RES_ResAppointment_AppointmentTime", oldReservistAppointment.AppointmentTime.ToString(), reservistAppointment.AppointmentTime.ToString(), currentUser));
                              
                cmd.ExecuteNonQuery();
                result = true;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                if (changeEvent != null && changeEvent.ChangeEventDetails.Count > 0)
                    changeEntry.AddEvent(changeEvent);
                
            }

            return result;
        }
       
        //Clear the current reservist appointment, if any
        public static bool ClearTheCurrentReservistAppointmentByReservist(int reservistId, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent = null;
            ReservistAppointment currentAppointment = GetCurrentReservistAppointmentByReservistId(reservistId, currentUser);

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL += @"BEGIN
                            UPDATE PMIS_RES.ReservistAppointments SET
                               IsCurrent = 0
                            WHERE ReservistID = :ReservistID;
                         END;
                        ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "ReservistID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = reservistId;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                //Log into Audit Trail only if there are updated rows
                if (currentAppointment != null)
                {
                    //Changes log
                    Reservist reservist = ReservistUtil.GetReservist(reservistId, currentUser);
                    changeEvent = new ChangeEvent("RES_Reservist_ClearResAppointment", "", null, reservist.Person, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_ResAppointment_ReqOrderNumber", currentAppointment.ReqOrderNumber, "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_ResAppointment_ReceiptAppointmentDate", CommonFunctions.FormatDate(currentAppointment.ReceiptAppointmentDate), "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_ResAppointment_MilitaryCommand", currentAppointment.MilitaryCommandName + (String.IsNullOrEmpty(currentAppointment.MilitaryCommandSuffix) ? "" : " ") + currentAppointment.MilitaryCommandSuffix, "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_ResAppointment_ReservistReadiness", ReadinessUtil.ReadinessName(currentAppointment.ReservistReadinessId.Value), "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_ResAppointment_MilitaryRank", currentAppointment.MilitaryRankName, "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_ResAppointment_MilReportingSpeciality", currentAppointment.MilReportingSpecialityCode + " " + currentAppointment.MilReportingSpecialityName, "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_ResAppointment_Position", currentAppointment.Position, "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_ResAppointment_AppointmentTime", currentAppointment.AppointmentTime.ToString(), "", currentUser));
                }

                result = true;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                if (changeEvent != null && changeEvent.ChangeEventDetails.Count > 0)
                {
                    changeEntry.AddEvent(changeEvent);
                    ReservistUtil.SetReservistModified(reservistId, currentUser);
                }
            }

            return result;
        }
    }
}