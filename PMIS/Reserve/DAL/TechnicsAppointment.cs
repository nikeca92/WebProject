using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    //This class represents a particular Technics Appointment
    public class TechnicsAppointment : BaseDbObject
    {
        private int technicsAppointmentId;
        private int technicsId;
        private bool isCurrent;
        private string reqOrderNumber;
        private int? equipmentTechnicsRequestId;
        private DateTime? receiptAppointmentDate;
        private string militaryCommandName;
        private string militaryCommandSuffix;
        private int? militaryCommandId;
        private int? technicsReadinessId;     
        private string comment;
        private decimal? appointmentTime;
        private string appointmentPlace;
        private int? fillTechnicsRequestId;

        public int TechnicsAppointmentId
        {
            get
            {
                return technicsAppointmentId;
            }
            set
            {
                technicsAppointmentId = value;
            }
        }

        public int TechnicsId
        {
            get
            {
                return technicsId;
            }
            set
            {
                technicsId = value;
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

        public int? EquipmentTechnicsRequestId
        {
            get
            {
                return equipmentTechnicsRequestId;
            }
            set
            {
                equipmentTechnicsRequestId = value;
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

        public int? TechnicsReadinessId
        {
            get
            {
                return technicsReadinessId;
            }
            set
            {
                technicsReadinessId = value;
            }
        }

        public string Comment
        {
            get
            {
                return comment;
            }
            set
            {
                comment = value;
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

        public int? FillTechnicsRequestId
        {
            get
            {
                return fillTechnicsRequestId;
            }
            set
            {
                fillTechnicsRequestId = value;
            }
        }


        public TechnicsAppointment(User user)
            : base(user)
        {

        }
    }

    public static class TechnicsAppointmentUtil
    {
        //This method creates and returns a TechnicsAppointment object. It extracts the data from a DataReader.
        //This is done in this way to have a signle place of creation of this object, however, pass the particular DataReader
        //object from various queries for better performance instead of executing a new call to the DB
        public static TechnicsAppointment ExtractTechnicsAppointment(OracleDataReader dr, User currentUser)
        {
            TechnicsAppointment technicsAppointment = new TechnicsAppointment(currentUser);

            technicsAppointment.TechnicsAppointmentId = DBCommon.GetInt(dr["TechnicsAppointmentID"]);
            technicsAppointment.TechnicsId = DBCommon.GetInt(dr["TechnicsID"]);
            technicsAppointment.IsCurrent = (DBCommon.IsInt(dr["IsCurrent"]) && DBCommon.GetInt(dr["IsCurrent"]) == 1);
            technicsAppointment.ReqOrderNumber = dr["ReqOrderNumber"].ToString();
            technicsAppointment.EquipmentTechnicsRequestId = (DBCommon.IsInt(dr["EquipmentTechnicsRequestID"]) ? DBCommon.GetInt(dr["EquipmentTechnicsRequestID"]) : (int?)null);
            technicsAppointment.ReceiptAppointmentDate = (dr["ReceiptAppointmentDate"] is DateTime) ? (DateTime)dr["ReceiptAppointmentDate"] : (DateTime?)null;
            technicsAppointment.MilitaryCommandName = dr["MilitaryCommandName"].ToString();
            technicsAppointment.MilitaryCommandSuffix = dr["MilitaryCommandSuffix"].ToString();
            technicsAppointment.MilitaryCommandId = (DBCommon.IsInt(dr["MilitaryCommandID"]) ? DBCommon.GetInt(dr["MilitaryCommandID"]) : (int?)null);
            technicsAppointment.TechnicsReadinessId = (DBCommon.IsInt(dr["TechnicsReadinessID"]) ? DBCommon.GetInt(dr["TechnicsReadinessID"]) : (int?)null);
            technicsAppointment.Comment = dr["TComment"].ToString();
            technicsAppointment.AppointmentTime = (DBCommon.IsDecimal(dr["AppointmentTime"]) ? DBCommon.GetDecimal(dr["AppointmentTime"]) : (decimal?)null);
            technicsAppointment.AppointmentPlace = dr["AppointmentPlace"].ToString();
            technicsAppointment.FillTechnicsRequestId = (DBCommon.IsInt(dr["FulfillTechnicsRequestID"]) ? DBCommon.GetInt(dr["FulfillTechnicsRequestID"]) : (int?)null);

            BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, technicsAppointment);

            return technicsAppointment;
        }

        //Get a particular object by its ID
        public static TechnicsAppointment GetTechnicsAppointment(int technicsAppointmentId, User currentUser)
        {
            TechnicsAppointment technicsAppointment = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TechnicsAppointmentID,
                                      a.TechnicsID,
                                      a.IsCurrent,
                                        
                                      a.ReqOrderNumber,
                                      a.EquipmentTechnicsRequestID, 
                                      a.ReceiptAppointmentDate,
                                      a.MilitaryCommandName, 
                                      a.MilitaryCommandSuffix, 
                                      a.MilitaryCommandID,
                                      a.TechnicsReadinessID,     
                                      a.TComment,
                                      a.AppointmentTime,
                                      a.AppointmentPlace,
                                      a.FulfillTechnicsRequestID,

                                      a.CreatedBy,
                                      a.CreatedDate,
                                      a.LastModifiedBy,
                                      a.LastModifiedDate
                               FROM PMIS_RES.TechnicsAppointments a
                               WHERE a.TechnicsAppointmentID = :TechnicsAppointmentID ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsAppointmentID", OracleType.Number).Value = technicsAppointmentId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    technicsAppointment = ExtractTechnicsAppointment(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return technicsAppointment;
        }

        //Get the current appointment by TechnicsID
        public static TechnicsAppointment GetCurrentTechnicsAppointmentByTechnicsId(int technicsId, User currentUser)
        {
            TechnicsAppointment technicsAppointment = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TechnicsAppointmentID,
                                      a.TechnicsID,
                                      a.IsCurrent,
                                        
                                      a.ReqOrderNumber,
                                      a.EquipmentTechnicsRequestID, 
                                      a.ReceiptAppointmentDate,
                                      a.MilitaryCommandName, 
                                      a.MilitaryCommandSuffix, 
                                      a.MilitaryCommandID,
                                      a.TechnicsReadinessID,     
                                      a.TComment,
                                      a.AppointmentTime,
                                      a.AppointmentPlace,
                                      a.FulfillTechnicsRequestID,

                                      a.CreatedBy,
                                      a.CreatedDate,
                                      a.LastModifiedBy,
                                      a.LastModifiedDate
                               FROM PMIS_RES.TechnicsAppointments a
                               WHERE a.TechnicsID = :TechnicsID AND a.IsCurrent = 1 ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsID", OracleType.Number).Value = technicsId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    technicsAppointment = ExtractTechnicsAppointment(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return technicsAppointment;
        }

        //Get all appointments (history) by Technics
        public static List<TechnicsAppointment> GetAllTechnicsAppointmentsByTechnicsId(int technicsId, User currentUser)
        {
            List<TechnicsAppointment> technicsAppointments = new List<TechnicsAppointment>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TechnicsAppointmentID,
                                      a.TechnicsID,
                                      a.IsCurrent,
                                        
                                      a.ReqOrderNumber,
                                      a.EquipmentTechnicsRequestID, 
                                      a.ReceiptAppointmentDate,
                                      a.MilitaryCommandName, 
                                      a.MilitaryCommandSuffix, 
                                      a.MilitaryCommandID,
                                      a.TechnicsReadinessID,     
                                      a.TComment,
                                      a.AppointmentTime,
                                      a.AppointmentPlace,
                                      a.FulfillTechnicsRequestID,

                                      a.CreatedBy,
                                      a.CreatedDate,
                                      a.LastModifiedBy,
                                      a.LastModifiedDate
                               FROM PMIS_RES.TechnicsAppointments a
                               WHERE a.TechnicsID = :TechnicsID 
                               ORDER BY a.TechnicsAppointmentID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsID", OracleType.Number).Value = technicsId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    technicsAppointments.Add(ExtractTechnicsAppointment(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return technicsAppointments;
        }

        //Get all appointments (history) by Technics with pagination
        public static List<TechnicsAppointment> GetAllTechnicsAppointmentsByTechnicsId(int technicsId, int orderBy, int pageIdx, int rowsPerPage, User currentUser)
        {
            List<TechnicsAppointment> technicsAppointments = new List<TechnicsAppointment>();

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
                                               SELECT a.TechnicsAppointmentID,
                                                      a.TechnicsID,
                                                      a.IsCurrent,
                                                        
                                                      a.ReqOrderNumber,
                                                      a.EquipmentTechnicsRequestID, 
                                                      a.ReceiptAppointmentDate,
                                                      a.MilitaryCommandName, 
                                                      a.MilitaryCommandSuffix, 
                                                      a.MilitaryCommandID,
                                                      a.TechnicsReadinessID,     
                                                      a.TComment,
                                                      a.AppointmentTime,
                                                      a.AppointmentPlace,
                                                      a.FulfillTechnicsRequestID,

                                                      a.CreatedBy,
                                                      a.CreatedDate,
                                                      a.LastModifiedBy,
                                                      a.LastModifiedDate,
                                                      RANK() OVER (ORDER BY " + orderBySQL + @", a.TechnicsAppointmentID) as RowNumber
                                               FROM PMIS_RES.TechnicsAppointments a
                                               WHERE a.TechnicsID = :TechnicsID 
                                               ORDER BY " + orderBySQL + @", a.TechnicsAppointmentID
                                            ) tmp
                                            " + pageWhere;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsID", OracleType.Number).Value = technicsId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    technicsAppointments.Add(ExtractTechnicsAppointment(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return technicsAppointments;
        }

        //Get all appointments (history) count by Technics for pagination
        public static int GetAllTechnicsAppointmentsByTechnicsIdCount(int technicsId, User currentUser)
        {
            int technicsAppointments = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {             
                string SQL = @"SELECT COUNT(*) as Cnt
                               FROM PMIS_RES.TechnicsAppointments a
                               WHERE a.TechnicsID = :TechnicsID                                                                                            
                              ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsID", OracleType.Number).Value = technicsId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        technicsAppointments = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return technicsAppointments;
        }

        //Add a particular Technics Appointment into the DB
        public static bool AddTechnicsAppointment(TechnicsAppointment technicsAppointment, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL += @"BEGIN
                            UPDATE PMIS_RES.TechnicsAppointments SET
                               IsCurrent = 0
                            WHERE TechnicsID = :TechnicsID;

                            INSERT INTO PMIS_RES.TechnicsAppointments (
                                TechnicsID,
                                IsCurrent,
                                
                                ReqOrderNumber,
                                EquipmentTechnicsRequestID, 
                                ReceiptAppointmentDate,
                                MilitaryCommandName, 
                                MilitaryCommandSuffix, 
                                MilitaryCommandID,
                                TechnicsReadinessID,     
                                TComment,
                                AppointmentTime,
                                AppointmentPlace,
                                FulfillTechnicsRequestID,

                                CreatedBy,
                                CreatedDate,
                                LastModifiedBy,
                                LastModifiedDate
                            )
                            VALUES (
                                :TechnicsID,
                                :IsCurrent,
                                
                                :ReqOrderNumber,
                                :EquipmentTechnicsRequestID, 
                                :ReceiptAppointmentDate,
                                :MilitaryCommandName, 
                                :MilitaryCommandSuffix, 
                                :MilitaryCommandID,
                                :TechnicsReadinessID,     
                                :TComment,
                                :AppointmentTime,
                                :AppointmentPlace,
                                :FulfillTechnicsRequestID,

                                :CreatedBy,
                                :CreatedDate,
                                :LastModifiedBy,
                                :LastModifiedDate                              
                            );

                            SELECT PMIS_RES.TechnicsAppointments_ID_SEQ.currval INTO :TechnicsAppointmentID FROM dual;
                         END;
                        ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramTechnicsAppointmentID = new OracleParameter();
                paramTechnicsAppointmentID.ParameterName = "TechnicsAppointmentID";
                paramTechnicsAppointmentID.OracleType = OracleType.Number;
                paramTechnicsAppointmentID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(paramTechnicsAppointmentID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "TechnicsID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = technicsAppointment.TechnicsId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "IsCurrent";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = technicsAppointment.IsCurrent ? 1 : 0;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ReqOrderNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(technicsAppointment.ReqOrderNumber))
                    param.Value = technicsAppointment.ReqOrderNumber;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "EquipmentTechnicsRequestID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (technicsAppointment.EquipmentTechnicsRequestId.HasValue)
                    param.Value = technicsAppointment.EquipmentTechnicsRequestId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ReceiptAppointmentDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (technicsAppointment.ReceiptAppointmentDate.HasValue)
                    param.Value = technicsAppointment.ReceiptAppointmentDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryCommandName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(technicsAppointment.MilitaryCommandName))
                    param.Value = technicsAppointment.MilitaryCommandName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryCommandSuffix";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(technicsAppointment.MilitaryCommandSuffix))
                    param.Value = technicsAppointment.MilitaryCommandSuffix;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryCommandID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (technicsAppointment.MilitaryCommandId.HasValue)
                    param.Value = technicsAppointment.MilitaryCommandId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "TechnicsReadinessID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (technicsAppointment.TechnicsReadinessId.HasValue)
                    param.Value = technicsAppointment.TechnicsReadinessId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);         

                param = new OracleParameter();
                param.ParameterName = "TComment";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(technicsAppointment.Comment))
                    param.Value = technicsAppointment.Comment;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AppointmentTime";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (technicsAppointment.AppointmentTime.HasValue)
                    param.Value = technicsAppointment.AppointmentTime.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AppointmentPlace";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(technicsAppointment.AppointmentPlace))
                    param.Value = technicsAppointment.AppointmentPlace;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "FulfillTechnicsRequestID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (technicsAppointment.FillTechnicsRequestId.HasValue)
                    param.Value = technicsAppointment.FillTechnicsRequestId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                BaseDbObjectUtil.SetCreatedParams(cmd, currentUser);
                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();

                technicsAppointment.TechnicsAppointmentId = DBCommon.GetInt(paramTechnicsAppointmentID.Value);

                //Changes log                
                changeEvent = new ChangeEvent("RES_Technics_AddTechAppointment", TechnicsUtil.GetTechnicsLogDescription(technicsAppointment.TechnicsId, currentUser), null, null, currentUser);

                changeEvent.AddDetail(new ChangeEventDetail("RES_TechAppointment_ReqOrderNumber", "", technicsAppointment.ReqOrderNumber, currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("RES_TechAppointment_ReceiptAppointmentDate", "", CommonFunctions.FormatDate(technicsAppointment.ReceiptAppointmentDate), currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("RES_TechAppointment_MilitaryCommand", "", technicsAppointment.MilitaryCommandName + (String.IsNullOrEmpty(technicsAppointment.MilitaryCommandSuffix) ? "" : " ") + technicsAppointment.MilitaryCommandSuffix, currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("RES_TechAppointment_TechnicsReadiness", "", ReadinessUtil.ReadinessName(technicsAppointment.TechnicsReadinessId.Value), currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("RES_TechAppointment_Comment", "", technicsAppointment.Comment, currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("RES_TechAppointment_AppointmentTime", "", technicsAppointment.AppointmentTime.ToString(), currentUser));

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
                    TechnicsUtil.SetTechnicsModified(technicsAppointment.TechnicsId, currentUser);
                }
            }

            return result;
        }
                
        public static void RefreshTechnicsAppointment(List<FillTechnicsRequest> technicsRequests, User currentUser, Change changeEntry)
        {
            foreach (FillTechnicsRequest technicsRequest in technicsRequests)
            {
                TechnicsAppointment technicAppointment = TechnicsAppointmentUtil.GetCurrentTechnicsAppointmentByTechnicsId(technicsRequest.TechnicsID, currentUser);
                TechnicsRequestCommandPosition requestCommandPosition = TechnicsRequestCommandPositionUtil.GetTechnicsRequestCommandPosition(currentUser, technicsRequest.TechnicsRequestCommandPositionID);

                technicAppointment.TechnicsId = technicsRequest.TechnicsID;
                technicAppointment.IsCurrent = true;
                technicAppointment.ReqOrderNumber = requestCommandPosition.TechnicsRequestsCommand.EquipmentTechnicsRequest.RequestNumber;
                technicAppointment.EquipmentTechnicsRequestId = requestCommandPosition.TechnicsRequestsCommand.EquipmentTechnicsRequestId;
                technicAppointment.MilitaryCommandName = requestCommandPosition.TechnicsRequestsCommand.MilitaryCommand.DisplayTextForSelection;
                technicAppointment.MilitaryCommandSuffix = requestCommandPosition.TechnicsRequestsCommand.MilitaryCommandSuffix;
                technicAppointment.MilitaryCommandId = requestCommandPosition.TechnicsRequestsCommand.MilitaryCommand.MilitaryCommandId;
                technicAppointment.TechnicsReadinessId = technicsRequest.TechnicReadinessID;
                technicAppointment.Comment = requestCommandPosition.Comment;
                technicAppointment.AppointmentTime = requestCommandPosition.TechnicsRequestsCommand.AppointmentTime;
                technicAppointment.FillTechnicsRequestId = technicsRequest.FillTechnicsRequestID;

                TechnicsAppointmentUtil.UpdateTechnicAppointment(technicAppointment, currentUser, changeEntry);
            }
        }

        //Update a particular Reservist Appointment from the DB
        public static bool UpdateTechnicAppointment(TechnicsAppointment technicAppointment, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL += @"BEGIN

                            UPDATE PMIS_RES.TECHNICSAPPOINTMENTS
                            SET TechnicsID = :TechnicsID,
					            IsCurrent = :IsCurrent,
            					ReqOrderNumber = :ReqOrderNumber,
					            EquipmentTechnicsRequestID = :EquipmentTechnicsRequestID, 
					            MilitaryCommandName = :MilitaryCommandName, 
					            MilitaryCommandSuffix = :MilitaryCommandSuffix, 
					            MilitaryCommandID = :MilitaryCommandID,
					            TechnicsReadinessID = :TechnicsReadinessID,     
					            TComment = :TComment,
					            AppointmentTime = :AppointmentTime,
					            FulfillTechnicsRequestID = :FulfillTechnicsRequestID,
					            CreatedBy = :CreatedBy,
					            CreatedDate = :CreatedDate,
					            LastModifiedBy = :LastModifiedBy,
					            LastModifiedDate = :LastModifiedDate 
                            WHERE TECHNICSID = :TechnicsID AND
                                  IsCurrent = 1 ;

                         END;
                        ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "TechnicsID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = technicAppointment.TechnicsId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "IsCurrent";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = technicAppointment.IsCurrent ? 1 : 0;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ReqOrderNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(technicAppointment.ReqOrderNumber))
                    param.Value = technicAppointment.ReqOrderNumber;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "EquipmentTechnicsRequestID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (technicAppointment.EquipmentTechnicsRequestId.HasValue)
                    param.Value = technicAppointment.EquipmentTechnicsRequestId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);
                
                param = new OracleParameter();
                param.ParameterName = "MilitaryCommandName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(technicAppointment.MilitaryCommandName))
                    param.Value = technicAppointment.MilitaryCommandName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryCommandSuffix";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(technicAppointment.MilitaryCommandSuffix))
                    param.Value = technicAppointment.MilitaryCommandSuffix;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryCommandID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (technicAppointment.MilitaryCommandId.HasValue)
                    param.Value = technicAppointment.MilitaryCommandId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "TechnicsReadinessID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (technicAppointment.TechnicsReadinessId.HasValue)
                    param.Value = technicAppointment.TechnicsReadinessId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "TComment";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(technicAppointment.Comment))
                    param.Value = technicAppointment.Comment;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AppointmentTime";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (technicAppointment.AppointmentTime.HasValue)
                    param.Value = technicAppointment.AppointmentTime.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);
                                
                param = new OracleParameter();
                param.ParameterName = "FulfillTechnicsRequestID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (technicAppointment.FillTechnicsRequestId.HasValue)
                    param.Value = technicAppointment.FillTechnicsRequestId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);
                
                param = new OracleParameter();
                param.ParameterName = "CreatedBy";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = technicAppointment.CreatedByUserId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "CreatedDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                param.Value = technicAppointment.CreatedDate;
                cmd.Parameters.Add(param);

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                //Changes log

                changeEvent = new ChangeEvent("RES_Technics_UpdateTechAppointment", TechnicsUtil.GetTechnicsLogDescription(technicAppointment.TechnicsId, currentUser), null, null, currentUser);

                TechnicsAppointment oldTechnicAppointment = TechnicsAppointmentUtil.GetCurrentTechnicsAppointmentByTechnicsId(technicAppointment.TechnicsId, currentUser);

                if (oldTechnicAppointment.ReqOrderNumber.Trim() != technicAppointment.ReqOrderNumber.Trim())
                    changeEvent.AddDetail(new ChangeEventDetail("RES_TechAppointment_ReqOrderNumber", oldTechnicAppointment.ReqOrderNumber, oldTechnicAppointment.ReqOrderNumber, currentUser));
                
                if(oldTechnicAppointment.MilitaryCommandName != technicAppointment.MilitaryCommandName ||
                   oldTechnicAppointment.MilitaryCommandSuffix != technicAppointment.MilitaryCommandSuffix)
                   changeEvent.AddDetail(new ChangeEventDetail("RES_TechAppointment_MilitaryCommand", oldTechnicAppointment.MilitaryCommandName + (String.IsNullOrEmpty(oldTechnicAppointment.MilitaryCommandSuffix) ? "" : " ") + oldTechnicAppointment.MilitaryCommandSuffix, technicAppointment.MilitaryCommandName + (String.IsNullOrEmpty(technicAppointment.MilitaryCommandSuffix) ? "" : " ") + technicAppointment.MilitaryCommandSuffix, currentUser));

                if(oldTechnicAppointment.Comment != technicAppointment.Comment)
                    changeEvent.AddDetail(new ChangeEventDetail("RES_TechAppointment_Comment", oldTechnicAppointment.Comment, technicAppointment.Comment, currentUser));

                if(oldTechnicAppointment.AppointmentTime != technicAppointment.AppointmentTime)
                    changeEvent.AddDetail(new ChangeEventDetail("RES_TechAppointment_AppointmentTime", oldTechnicAppointment.AppointmentTime.ToString(), technicAppointment.AppointmentTime.ToString(), currentUser));

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
                
        //Clear the current Technics appointment, if any
        public static bool ClearTheCurrentTechnicsAppointmentByTechnics(int technicsId, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent = null;
            TechnicsAppointment currentAppointment = GetCurrentTechnicsAppointmentByTechnicsId(technicsId, currentUser);

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL += @"BEGIN
                            UPDATE PMIS_RES.TechnicsAppointments SET
                               IsCurrent = 0
                            WHERE TechnicsID = :TechnicsID;
                         END;
                        ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "TechnicsID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = technicsId;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                //Log into Audit Trail only if there are updated rows
                if (currentAppointment != null)
                {
                    //Changes log
                    changeEvent = new ChangeEvent("RES_Technics_ClearTechAppointment", TechnicsUtil.GetTechnicsLogDescription(technicsId, currentUser), null, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_TechAppointment_ReqOrderNumber", currentAppointment.ReqOrderNumber, "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_TechAppointment_ReceiptAppointmentDate", CommonFunctions.FormatDate(currentAppointment.ReceiptAppointmentDate), "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_TechAppointment_MilitaryCommand", currentAppointment.MilitaryCommandName + (String.IsNullOrEmpty(currentAppointment.MilitaryCommandSuffix) ? "" : " ") + currentAppointment.MilitaryCommandSuffix, "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_TechAppointment_TechnicsReadiness", ReadinessUtil.ReadinessName(currentAppointment.TechnicsReadinessId.Value), "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_TechAppointment_Comment", currentAppointment.Comment, "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_TechAppointment_AppointmentTime", currentAppointment.AppointmentTime.ToString(), "", currentUser));
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
                    TechnicsUtil.SetTechnicsModified(technicsId, currentUser);
                }
            }

            return result;
        }
    }
}