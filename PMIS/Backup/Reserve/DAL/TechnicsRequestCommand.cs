using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    //This class represents a particular command added into a particular request (for equipment with reservists)
    public class TechnicsRequestCommand : BaseDbObject
    {
        private int technicsRequestCommandId;
        private int equipmentTechnicsRequestId;
        private EquipmentTechnicsRequest equipmentTechnicsRequest;
        private MilitaryCommand militaryCommand;
        private string militaryCommandSuffix;
        private City deliveryCity;
        private string deliveryPlace;
        private List<TechnicsRequestCommandPosition> technicsRequestCommandPositions;
        private decimal? appointmentTime;
        private int? militaryReadinessId;
        private MilitaryReadiness militaryReadiness;

        public int TechnicsRequestCommandId
        {
            get
            {
                return technicsRequestCommandId;
            }
            set
            {
                technicsRequestCommandId = value;
            }
        }

        public int EquipmentTechnicsRequestId
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

        public EquipmentTechnicsRequest EquipmentTechnicsRequest
        {
            get
            {
                //Lazy initialization
                if (equipmentTechnicsRequest == null)
                    equipmentTechnicsRequest = EquipmentTechnicsRequestUtil.GetEquipmentTechnicsRequest(EquipmentTechnicsRequestId, CurrentUser);

                return equipmentTechnicsRequest;
            }
            set
            {
                equipmentTechnicsRequest = value;
            }
        }

        public MilitaryCommand MilitaryCommand
        {
            get
            {
                return militaryCommand;
            }
            set
            {
                militaryCommand = value;
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

        public City DeliveryCity
        {
            get
            {
                return deliveryCity;
            }
            set
            {
                deliveryCity = value;
            }
        }

        public string DeliveryPlace
        {
            get
            {
                return deliveryPlace;
            }
            set
            {
                deliveryPlace = value;
            }
        }

        public List<TechnicsRequestCommandPosition> TechnicsRequestCommandPositions
        {
            get
            {
                //Lazy initialization; It would be used only when there is a need of it
                //When we populate all request commands for a request then we load the positions too (without using this lazy initialization)
                if (technicsRequestCommandPositions == null)
                {
                    technicsRequestCommandPositions = TechnicsRequestCommandPositionUtil.GetTechnicsRequestCommandPositions(CurrentUser, TechnicsRequestCommandId);
                }

                return technicsRequestCommandPositions;
            }
            set
            {
                technicsRequestCommandPositions = value;
            }
        }

        public string DisplayText
        {
            get
            {
                return MilitaryCommand != null ? MilitaryCommand.CommandNumber + " " + MilitaryCommandSuffix + " " + MilitaryCommand.ShortName : "";
            }
        }

        public string DisplayText2
        {
            get
            {
                return (String.IsNullOrEmpty(MilitaryCommandSuffix) ? " " : MilitaryCommandSuffix) + " / " + EquipmentTechnicsRequest.RequestNumber;
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

        public int? MilitaryReadinessId
        {
            get
            {
                return militaryReadinessId;
            }
            set
            {
                militaryReadinessId = value;
            }
        }

        public MilitaryReadiness MilitaryReadiness
        {
            get
            {
                if (militaryReadiness == null && militaryReadinessId.HasValue)
                    militaryReadiness = MilitaryReadinessUtil.GetMilitaryReadiness(militaryReadinessId.Value, CurrentUser);

                return militaryReadiness;
            }
            set
            {
                militaryReadiness = value;
            }
        }

        public bool CanDelete
        {
            get { return true; }

        }

        public TechnicsRequestCommand(User user)
            : base(user)
        {

        }
    }

    //Some methods for working with TechnicsRequestCommand objects
    public static class TechnicsRequestCommandUtil
    {
        //This method creates and returns a EquipmentReservistsRequest object. It extracts the data from a DataReader.
        //This is done in this way to have a signle place of creation of this object, however, pass the particular DataReader
        //object from various queries for better performance instead of executing a new call to the DB here to get the details for a specific id, for example.
        public static TechnicsRequestCommand ExtractTechnicsRequestCommandFromDataReader(OracleDataReader dr, User currentUser)
        {
            TechnicsRequestCommand technicsRequestCommand = new TechnicsRequestCommand(currentUser);

            technicsRequestCommand.TechnicsRequestCommandId = DBCommon.GetInt(dr["TechRequestsCommandID"]);
            technicsRequestCommand.EquipmentTechnicsRequestId = DBCommon.GetInt(dr["EquipmentTechnicsRequestID"]);
            technicsRequestCommand.MilitaryCommand = MilitaryCommandUtil.ExtractMilitaryCommandFromDR(currentUser, dr);
            technicsRequestCommand.MilitaryCommandSuffix = dr["MilitaryCommandSuffix"].ToString();

            if (DBCommon.IsInt(dr["DeliveryCityID"]))
                technicsRequestCommand.DeliveryCity = CityUtil.GetCity(DBCommon.GetInt(dr["DeliveryCityID"]), currentUser);

            technicsRequestCommand.DeliveryPlace = dr["DeliveryPlace"].ToString();

            if (DBCommon.IsDecimal(dr["AppointmentTime"]))
                technicsRequestCommand.AppointmentTime = DBCommon.GetDecimal(dr["AppointmentTime"]);

            if (DBCommon.IsInt(dr["MilReadinessID"]))
                technicsRequestCommand.MilitaryReadinessId = DBCommon.GetInt(dr["MilReadinessID"]);

            return technicsRequestCommand;
        }

        //Get a specific TechnicsRequestCommand record
        public static TechnicsRequestCommand GetTechnicsRequestCommand(User currentUser, int technicsRequestsCommandId)
        {
            TechnicsRequestCommand technicsRequestCommand = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TechRequestsCommandID,
                                      a.EquipmentTechnicsRequestID,
                                      a.MilitaryCommandID, b.IMEES as ShortName, b.NK as CommandNumber,
                                      b.KOD_MIR2 as MilitaryUnitID,
                                      a.MilitaryCommandSuffix,
                                      a.DeliveryCityID,
                                      a.DeliveryPlace,
                                      a.AppointmentTime,
                                      a.MilReadinessID
                               FROM PMIS_RES.TechnicsRequestCommands a
                               INNER JOIN UKAZ_OWNER.VVR b ON a.MilitaryCommandID = b.KOD_VVR
                               WHERE a.TechRequestsCommandID = :TechRequestsCommandID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechRequestsCommandID", OracleType.Number).Value = technicsRequestsCommandId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    technicsRequestCommand = ExtractTechnicsRequestCommandFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return technicsRequestCommand;
        }

        //Get a list of all TechnicsRequestCommand records for a particular request for equipment with techncis
        public static List<TechnicsRequestCommand> GetTechnicsRequestCommandsForRequest(User currentUser, int equipmentTechnicsRequestId)
        {
            List<TechnicsRequestCommand> technicsRequestCommands = new List<TechnicsRequestCommand>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TechRequestsCommandID,
                                      a.EquipmentTechnicsRequestID,
                                      a.MilitaryCommandID, b.IMEES as ShortName, b.NK as CommandNumber,
                                      b.KOD_MIR2 as MilitaryUnitID,
                                      a.MilitaryCommandSuffix,
                                      a.DeliveryCityID,
                                      a.DeliveryPlace,
                                      a.AppointmentTime,
                                      a.MilReadinessID,

                                      c.TechnicsRequestCmdPositionID,
                                      c.TechRequestsCommandID,
                                      c.TechnicsTypeID, d.TechnicsTypeKey, d.TechnicsTypeName, d.Active as TechnicsTypeActive,
                                      n.NormativeTechnicsID, n.NormativeCode, n.NormativeName, n.TechnicsSubTypeID,
                                      c.Count,
                                      c.DriversCount,
                                      c.TComment,
                                      c.Seq,
                                      NVL(fulfil.FulfilCount, 0) as FulfilCount
                               FROM PMIS_RES.TechnicsRequestCommands a
                               INNER JOIN UKAZ_OWNER.VVR b ON a.MilitaryCommandID = b.KOD_VVR
                               LEFT OUTER JOIN PMIS_RES.TechnicsRequestCmdPositions c ON a.TechRequestsCommandID = c.TechRequestsCommandID
                               LEFT OUTER JOIN PMIS_RES.TechnicsTypes d ON c.TechnicsTypeID = d.TechnicsTypeID
                               LEFT OUTER JOIN PMIS_RES.NormativeTechnics n ON c.NormativeTechnicsID = n.NormativeTechnicsID
                               LEFT OUTER JOIN (SELECT COUNT(*) as FulfilCount,
                                                       TechnicsRequestCmdPositionID
                                                FROM PMIS_RES.FulfilTechnicsRequest
                                                GROUP BY TechnicsRequestCmdPositionID) fulfil ON c.TechnicsRequestCmdPositionID = fulfil.TechnicsRequestCmdPositionID
                               WHERE a.EquipmentTechnicsRequestID = :EquipmentTechnicsRequestID
                               ORDER BY a.TechRequestsCommandID, c.Seq ASC, c.TechnicsRequestCmdPositionID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("EquipmentTechnicsRequestID", OracleType.Number).Value = equipmentTechnicsRequestId;

                OracleDataReader dr = cmd.ExecuteReader();

                int prevTechnicsRequestsCommandId = 0;

                TechnicsRequestCommand technicsRequestCommand = null;
                TechnicsRequestCommandPosition technicsPosition = null;

                while (dr.Read())
                {
                    int technicsRequestsCommandId = DBCommon.GetInt(dr["TechRequestsCommandID"]);

                    //The Request Command has changed - then add a new object
                    //It is changes when all positions of the command are passed
                    if (prevTechnicsRequestsCommandId != technicsRequestsCommandId)
                    {
                        technicsRequestCommand = ExtractTechnicsRequestCommandFromDataReader(dr, currentUser);
                        technicsRequestCommand.TechnicsRequestCommandPositions = new List<TechnicsRequestCommandPosition>();

                        technicsRequestCommands.Add(technicsRequestCommand);

                        prevTechnicsRequestsCommandId = technicsRequestsCommandId;
                    }

                    if (DBCommon.IsInt(dr["TechnicsRequestCmdPositionID"]))
                    {
                        technicsPosition = TechnicsRequestCommandPositionUtil.ExtractTechnicsRequestCommandPositionFromDataReader(dr, currentUser);

                        technicsRequestCommand.TechnicsRequestCommandPositions.Add(technicsPosition);
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return technicsRequestCommands;
        }

        //Get a list of all TechnicsRequestCommand records for a particular request for equipment with techncis
        public static List<TechnicsRequestCommand> GetTechnicsRequestCommandsForRequestAndMilDept(User currentUser, int equipmentTechnicsRequestId, int militaryDepartmentId)
        {
            List<TechnicsRequestCommand> technicsRequestCommands = new List<TechnicsRequestCommand>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TechRequestsCommandID,
                                      a.EquipmentTechnicsRequestID,
                                      a.MilitaryCommandID, b.IMEES as ShortName, b.NK as CommandNumber,
                                      b.KOD_MIR2 as MilitaryUnitID,
                                      a.MilitaryCommandSuffix,
                                      a.DeliveryCityID,
                                      a.DeliveryPlace,
                                      a.AppointmentTime,
                                      a.MilReadinessID,

                                      c.TechnicsRequestCmdPositionID,
                                      c.TechRequestsCommandID,
                                      c.TechnicsTypeID, d.TechnicsTypeKey, d.TechnicsTypeName, d.Active as TechnicsTypeActive,
                                      n.NormativeTechnicsID, n.NormativeCode, n.NormativeName, n.TechnicsSubTypeID,
                                      c.Count,
                                      c.DriversCount,
                                      c.TComment,
                                      c.Seq,
                                      NVL(fulfil.FulfilCount, 0) as FulfilCount
                               FROM PMIS_RES.TechnicsRequestCommands a
                               INNER JOIN UKAZ_OWNER.VVR b ON a.MilitaryCommandID = b.KOD_VVR
                               LEFT OUTER JOIN PMIS_RES.TechnicsRequestCmdPositions c ON a.TechRequestsCommandID = c.TechRequestsCommandID
                               LEFT OUTER JOIN PMIS_RES.TechnicsTypes d ON c.TechnicsTypeID = d.TechnicsTypeID
                               LEFT OUTER JOIN PMIS_RES.NormativeTechnics n ON c.NormativeTechnicsID = n.NormativeTechnicsID
                               LEFT OUTER JOIN (SELECT COUNT(*) as FulfilCount,
                                                       TechnicsRequestCmdPositionID
                                                FROM PMIS_RES.FulfilTechnicsRequest
                                                GROUP BY TechnicsRequestCmdPositionID) fulfil ON c.TechnicsRequestCmdPositionID = fulfil.TechnicsRequestCmdPositionID
                               LEFT OUTER JOIN PMIS_RES.TechRequestCmdPositionsMilDept g ON g.TechnicsRequestCmdPositionID = c.TechnicsRequestCmdPositionID
                               WHERE a.EquipmentTechnicsRequestID = :EquipmentTechnicsRequestID AND g.MilitaryDepartmentID = :MilitaryDepartmentID
                               ORDER BY a.TechRequestsCommandID, c.TechnicsRequestCmdPositionID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("EquipmentTechnicsRequestID", OracleType.Number).Value = equipmentTechnicsRequestId;
                cmd.Parameters.Add("MilitaryDepartmentID", OracleType.Number).Value = militaryDepartmentId;

                OracleDataReader dr = cmd.ExecuteReader();

                int prevTechnicsRequestsCommandId = 0;

                TechnicsRequestCommand technicsRequestCommand = null;
                TechnicsRequestCommandPosition technicsPosition = null;

                while (dr.Read())
                {
                    int technicsRequestsCommandId = DBCommon.GetInt(dr["TechRequestsCommandID"]);

                    //The Request Command has changed - then add a new object
                    //It is changes when all positions of the command are passed
                    if (prevTechnicsRequestsCommandId != technicsRequestsCommandId)
                    {
                        technicsRequestCommand = ExtractTechnicsRequestCommandFromDataReader(dr, currentUser);
                        technicsRequestCommand.TechnicsRequestCommandPositions = new List<TechnicsRequestCommandPosition>();

                        technicsRequestCommands.Add(technicsRequestCommand);

                        prevTechnicsRequestsCommandId = technicsRequestsCommandId;
                    }

                    if (DBCommon.IsInt(dr["TechnicsRequestCmdPositionID"]))
                    {
                        technicsPosition = TechnicsRequestCommandPositionUtil.ExtractTechnicsRequestCommandPositionFromDataReader(dr, currentUser);

                        technicsRequestCommand.TechnicsRequestCommandPositions.Add(technicsPosition);
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return technicsRequestCommands;
        }

        public static List<DropDownItem> GetTechnicsRequestCommandsForMilCommandAndMilDept(User currentUser, int militaryCommandId, string militaryDepartmentIds)
        {
            List<DropDownItem> militaryCommands = new List<DropDownItem>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT DISTINCT                                       
                                      a.MilitaryCommandID, b.IMEES as ShortName, b.NK as CommandNumber,
                                      b.KOD_MIR2 as MilitaryUnitID,
                                      a.MilitaryCommandSuffix                                                                  
                               FROM PMIS_RES.TechnicsRequestCommands a
                               INNER JOIN UKAZ_OWNER.VVR b ON a.MilitaryCommandID = b.KOD_VVR
                               INNER JOIN PMIS_RES.TechnicsRequestCmdPositions c ON a.TechRequestsCommandID = c.TechRequestsCommandID                                             
                               INNER JOIN PMIS_RES.TechRequestCmdPositionsMilDept g ON g.TechnicsRequestCmdPositionID = c.TechnicsRequestCmdPositionID
                               WHERE a.MilitaryCommandID = :MilitaryCommandID " + (string.IsNullOrEmpty(militaryDepartmentIds) ? "" : @" AND g.MilitaryDepartmentID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(militaryDepartmentIds) + @") ") + @"
                               ORDER BY b.NK";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitaryCommandID", OracleType.Number).Value = militaryCommandId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    DropDownItem ddi = new DropDownItem();
                    ddi.Txt = dr["CommandNumber"] + " " + dr["MilitaryCommandSuffix"] + " " + dr["ShortName"];
                    ddi.Val = dr["MilitaryCommandSuffix"].ToString();
                    militaryCommands.Add(ddi);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryCommands;
        }

        public static List<DropDownItem> GetTechnicsRequestCommandsForMilCommandAndMilDeptAndMilReadiness(User currentUser, int militaryCommandId, string militaryDepartmentIds, string militaryReadinessID)
        {
            List<DropDownItem> militaryCommands = new List<DropDownItem>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT DISTINCT                                       
                                      a.MilitaryCommandID, b.IMEES as ShortName, b.NK as CommandNumber,
                                      b.KOD_MIR2 as MilitaryUnitID,
                                      a.MilitaryCommandSuffix                                                                  
                               FROM PMIS_RES.TechnicsRequestCommands a
                               INNER JOIN UKAZ_OWNER.VVR b ON a.MilitaryCommandID = b.KOD_VVR
                               INNER JOIN PMIS_RES.TechnicsRequestCmdPositions c ON a.TechRequestsCommandID = c.TechRequestsCommandID                                             
                               INNER JOIN PMIS_RES.TechRequestCmdPositionsMilDept g ON g.TechnicsRequestCmdPositionID = c.TechnicsRequestCmdPositionID
                               WHERE " + (string.IsNullOrEmpty(militaryReadinessID) ? "" : " a.MilReadinessID = " + CommonFunctions.AvoidSQLInjForListOfIDs(militaryReadinessID) + " AND") + @"
                                     a.MilitaryCommandID = :MilitaryCommandID " + (string.IsNullOrEmpty(militaryDepartmentIds) ? "" : @" AND g.MilitaryDepartmentID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(militaryDepartmentIds) + @") ") + @"
                               ORDER BY b.NK";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitaryCommandID", OracleType.Number).Value = militaryCommandId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    DropDownItem ddi = new DropDownItem();
                    ddi.Txt = dr["CommandNumber"] + " " + dr["MilitaryCommandSuffix"] + " " + dr["ShortName"];
                    ddi.Val = dr["MilitaryCommandSuffix"].ToString();
                    militaryCommands.Add(ddi);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryCommands;
        }

        //Get a list of all TechnicsRequestCommand records for a particular request for equipment with technics and for particular Military Department
        public static List<TechnicsRequestCommand> GetAllTechnicsRequestCommandsForMilCommandAndMilDept(User currentUser, int militaryCommandId, int militaryDepartmentId)
        {
            List<TechnicsRequestCommand> technicsRequestCommands = new List<TechnicsRequestCommand>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TechRequestsCommandID,
                                      a.EquipmentTechnicsRequestID,
                                      a.MilitaryCommandID, b.IMEES as ShortName, b.NK as CommandNumber,
                                      b.KOD_MIR2 as MilitaryUnitID,
                                      a.MilitaryCommandSuffix,
                                      a.DeliveryCityID,
                                      a.DeliveryPlace,
                                      a.AppointmentTime,
                                      a.MilReadinessID,

                                      c.TechnicsRequestCmdPositionID,
                                      c.TechRequestsCommandID,
                                      c.TechnicsTypeID, d.TechnicsTypeKey, d.TechnicsTypeName, d.Active as TechnicsTypeActive,
                                      n.NormativeTechnicsID, n.NormativeCode, n.NormativeName, n.TechnicsSubTypeID,
                                      c.Count,
                                      c.DriversCount,
                                      c.TComment,
                                      c.Seq,
                                      NVL(fulfil.FulfilCount, 0) as FulfilCount
                               FROM PMIS_RES.TechnicsRequestCommands a
                               INNER JOIN UKAZ_OWNER.VVR b ON a.MilitaryCommandID = b.KOD_VVR
                               LEFT OUTER JOIN PMIS_RES.TechnicsRequestCmdPositions c ON a.TechRequestsCommandID = c.TechRequestsCommandID
                               LEFT OUTER JOIN PMIS_RES.TechnicsTypes d ON c.TechnicsTypeID = d.TechnicsTypeID
                               LEFT OUTER JOIN PMIS_RES.NormativeTechnics n ON c.NormativeTechnicsID = n.NormativeTechnicsID
                               LEFT OUTER JOIN (SELECT COUNT(*) as FulfilCount,
                                                       TechnicsRequestCmdPositionID
                                                FROM PMIS_RES.FulfilTechnicsRequest
                                                GROUP BY TechnicsRequestCmdPositionID) fulfil ON c.TechnicsRequestCmdPositionID = fulfil.TechnicsRequestCmdPositionID
                               LEFT OUTER JOIN PMIS_RES.TechRequestCmdPositionsMilDept g ON g.TechnicsRequestCmdPositionID = c.TechnicsRequestCmdPositionID
                               WHERE a.MilitaryCommandID = :MilitaryCommandID AND g.MilitaryDepartmentID = :MilitaryDepartmentID
                               ORDER BY a.TechRequestsCommandID, c.TechnicsRequestCmdPositionID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitaryCommandID", OracleType.Number).Value = militaryCommandId;
                cmd.Parameters.Add("MilitaryDepartmentID", OracleType.Number).Value = militaryDepartmentId;

                OracleDataReader dr = cmd.ExecuteReader();

                int prevTechnicsRequestsCommandId = 0;

                TechnicsRequestCommand technicsRequestCommand = null;
                TechnicsRequestCommandPosition technicsPosition = null;

                while (dr.Read())
                {
                    int technicsRequestsCommandId = DBCommon.GetInt(dr["TechRequestsCommandID"]);

                    //The Request Command has changed - then add a new object
                    //It is changes when all positions of the command are passed
                    if (prevTechnicsRequestsCommandId != technicsRequestsCommandId)
                    {
                        technicsRequestCommand = ExtractTechnicsRequestCommandFromDataReader(dr, currentUser);
                        technicsRequestCommand.TechnicsRequestCommandPositions = new List<TechnicsRequestCommandPosition>();

                        technicsRequestCommands.Add(technicsRequestCommand);

                        prevTechnicsRequestsCommandId = technicsRequestsCommandId;
                    }

                    if (DBCommon.IsInt(dr["TechnicsRequestCmdPositionID"]))
                    {
                        technicsPosition = TechnicsRequestCommandPositionUtil.ExtractTechnicsRequestCommandPositionFromDataReader(dr, currentUser);

                        technicsRequestCommand.TechnicsRequestCommandPositions.Add(technicsPosition);
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return technicsRequestCommands;
        }

        //Add a new command to a particular technics request
        public static TechnicsRequestCommand AddTechnicsRequestCommand(User currentUser, int equipmentTechnicsRequestId, int militaryCommandId,
                                                                       Change changeEntry)
        {
            TechnicsRequestCommand technicsRequestCommand = null;

            ChangeEvent changeEvent = null;

            EquipmentTechnicsRequest equipmentTechnicsRequest = EquipmentTechnicsRequestUtil.GetEquipmentTechnicsRequest(equipmentTechnicsRequestId, currentUser);

            string logDescription = "";
            logDescription += "Заявка №: " + equipmentTechnicsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(equipmentTechnicsRequest.RequestDate);

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string SQL = "";

            try
            {
                
                SQL += @"BEGIN
                            INSERT INTO PMIS_RES.TechnicsRequestCommands (EquipmentTechnicsRequestID,
                                MilitaryCommandID, MilitaryCommandSuffix, DeliveryCityID, DeliveryPlace, AppointmentTime, MilReadinessID)
                            VALUES (:EquipmentTechnicsRequestID,
                                :MilitaryCommandID, NULL, NULL, NULL, NULL, NULL);

                            SELECT PMIS_RES.TechnicsRequestCommands_ID_SEQ.currval INTO :TechRequestsCommandID FROM dual;
                         END;";

                changeEvent = new ChangeEvent("RES_EquipTechRequests_AddCommand", logDescription, equipmentTechnicsRequest.MilitaryUnit, null, currentUser);

                MilitaryCommand militaryCommand = MilitaryCommandUtil.GetMilitaryCommand(militaryCommandId, currentUser);

                changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_Command_Name", "", militaryCommand.DisplayTextForSelection, currentUser));

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramTechRequestsCommandID = new OracleParameter();
                paramTechRequestsCommandID.ParameterName = "TechRequestsCommandID";
                paramTechRequestsCommandID.OracleType = OracleType.Number;
                paramTechRequestsCommandID.Direction = ParameterDirection.Output;

                cmd.Parameters.Add(paramTechRequestsCommandID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "EquipmentTechnicsRequestID";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = equipmentTechnicsRequestId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryCommandID";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = militaryCommandId;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                int technicsRequestsCommandId = DBCommon.GetInt(paramTechRequestsCommandID.Value);

                technicsRequestCommand = TechnicsRequestCommandUtil.GetTechnicsRequestCommand(currentUser, technicsRequestsCommandId);

                EquipmentTechnicsRequestUtil.SetEquipmentTechnicsRequestModified(equipmentTechnicsRequestId, currentUser);
            }
            finally
            {
                conn.Close();
            }

            if (changeEvent != null && changeEvent.ChangeEventDetails.Count > 0)
                changeEntry.AddEvent(changeEvent);

            return technicsRequestCommand;
        }


        //Delete a particualr command from a particular technics request
        public static void DeleteTechnicsRequestCommand(User currentUser, int technicsRequestsCommandId, Change changeEntry)
        {
            ChangeEvent changeEvent = null;

            TechnicsRequestCommand technicsRequestCommand = GetTechnicsRequestCommand(currentUser, technicsRequestsCommandId);

            string logDescription = "";
            logDescription += "Заявка №: " + technicsRequestCommand.EquipmentTechnicsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(technicsRequestCommand.EquipmentTechnicsRequest.RequestDate);

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string SQL = "";

            try
            {

                SQL += @"BEGIN
                            DELETE FROM PMIS_RES.TechRequestCmdPositionsMilDept
                            WHERE TechnicsRequestCmdPositionID IN (SELECT TechnicsRequestCmdPositionID
                                                                   FROM PMIS_RES.TechnicsRequestCmdPositions 
                                                                   WHERE TechRequestsCommandID = :TechRequestsCommandID);

                            DELETE FROM PMIS_RES.TechnicsRequestCmdPositions
                            WHERE TechRequestsCommandID = :TechRequestsCommandID;

                            UPDATE PMIS_RES.Technics SET
                               PunktID = NULL
                            WHERE PunktID IN (SELECT TechRequestCommandPunktID
                                              FROM PMIS_RES.TechRequestCommandPunkt
                                              WHERE TechRequestsCommandID = :TechRequestsCommandID
                                              );

                            DELETE FROM PMIS_RES.TechRequestCommandPunkt
                            WHERE TechRequestsCommandID = :TechRequestsCommandID;

                            DELETE FROM PMIS_RES.TechnicsRequestCommands 
                            WHERE TechRequestsCommandID = :TechRequestsCommandID;
                         END;";

                changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteCommand", logDescription, technicsRequestCommand.EquipmentTechnicsRequest.MilitaryUnit, null, currentUser);

                changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_Command_Name", technicsRequestCommand.MilitaryCommand.DisplayTextForSelection, "", currentUser));

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechRequestsCommandID", OracleType.Number).Value = technicsRequestsCommandId;

                cmd.ExecuteNonQuery();

                EquipmentTechnicsRequestUtil.SetEquipmentTechnicsRequestModified(technicsRequestCommand.EquipmentTechnicsRequestId, currentUser);
            }
            finally
            {
                conn.Close();
            }

            if (changeEvent != null && changeEvent.ChangeEventDetails.Count > 0)
                changeEntry.AddEvent(changeEvent);
        }

        //Save a particualr command from a particular technics request
        public static void SaveTechnicsRequestCommand(User currentUser, TechnicsRequestCommand technicsRequestsCommand, Change changeEntry)
        {
            ChangeEvent changeEvent = null;

            string logDescription = "";
            logDescription += "Заявка №: " + technicsRequestsCommand.EquipmentTechnicsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(technicsRequestsCommand.EquipmentTechnicsRequest.RequestDate) +
                              "; Команда: " + technicsRequestsCommand.MilitaryCommand.DisplayTextForSelection;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string SQL = "";

            try
            {

                SQL += @"BEGIN
                            UPDATE PMIS_RES.TechnicsRequestCommands SET
                               MilitaryCommandSuffix = :MilitaryCommandSuffix, 
                               DeliveryCityID = :DeliveryCityID, 
                               DeliveryPlace = :DeliveryPlace,
                               AppointmentTime = :AppointmentTime,
                               MilReadinessID = :MilReadinessID
                            WHERE TechRequestsCommandID = :TechRequestsCommandID;
                         END;";

                changeEvent = new ChangeEvent("RES_EquipTechRequests_EditCommand", logDescription, technicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, currentUser);

                TechnicsRequestCommand oldTechnicsRequestsCommand = TechnicsRequestCommandUtil.GetTechnicsRequestCommand(currentUser, technicsRequestsCommand.TechnicsRequestCommandId);

                if (oldTechnicsRequestsCommand.MilitaryCommandSuffix.Trim() != technicsRequestsCommand.MilitaryCommandSuffix.Trim())
                    changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_Command_Suffix", oldTechnicsRequestsCommand.MilitaryCommandSuffix, technicsRequestsCommand.MilitaryCommandSuffix, currentUser));

                if ((oldTechnicsRequestsCommand.DeliveryCity != null && oldTechnicsRequestsCommand.DeliveryCity.CityName != null ? oldTechnicsRequestsCommand.DeliveryCity.CityName : "") !=
                   (technicsRequestsCommand.DeliveryCity != null && technicsRequestsCommand.DeliveryCity.CityName != null ? technicsRequestsCommand.DeliveryCity.CityName : ""))
                    changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_Command_DeliveryCity",
                        oldTechnicsRequestsCommand.DeliveryCity != null && oldTechnicsRequestsCommand.DeliveryCity.CityName != null ? oldTechnicsRequestsCommand.DeliveryCity.Region.RegionName + ", " + oldTechnicsRequestsCommand.DeliveryCity.Municipality.MunicipalityName + ", " + oldTechnicsRequestsCommand.DeliveryCity.CityName : "",
                        technicsRequestsCommand.DeliveryCity != null && technicsRequestsCommand.DeliveryCity.CityName != null ? technicsRequestsCommand.DeliveryCity.Region.RegionName + ", " + technicsRequestsCommand.DeliveryCity.Municipality.MunicipalityName + ", " + technicsRequestsCommand.DeliveryCity.CityName : "",
                        currentUser));

                if (oldTechnicsRequestsCommand.DeliveryPlace.Trim() != technicsRequestsCommand.DeliveryPlace.Trim())
                    changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_Command_DeliveryPlace", oldTechnicsRequestsCommand.DeliveryPlace, technicsRequestsCommand.DeliveryPlace, currentUser));

                if (!CommonFunctions.IsEqualDecimal(oldTechnicsRequestsCommand.AppointmentTime, technicsRequestsCommand.AppointmentTime))
                    changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_Command_AppointmentTime", (oldTechnicsRequestsCommand.AppointmentTime.HasValue ? oldTechnicsRequestsCommand.AppointmentTime.ToString() : ""), (technicsRequestsCommand.AppointmentTime.HasValue ? technicsRequestsCommand.AppointmentTime.ToString() : ""), currentUser));

                if (!CommonFunctions.IsEqualInt(oldTechnicsRequestsCommand.MilitaryReadinessId, technicsRequestsCommand.MilitaryReadinessId))
                    changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_Command_MilReadiness", (oldTechnicsRequestsCommand.MilitaryReadinessId.HasValue ? oldTechnicsRequestsCommand.MilitaryReadiness.MilReadinessName : ""), (technicsRequestsCommand.MilitaryReadinessId.HasValue ? technicsRequestsCommand.MilitaryReadiness.MilReadinessName : ""), currentUser));
                
                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechRequestsCommandID", OracleType.Number).Value = technicsRequestsCommand.TechnicsRequestCommandId;

                OracleParameter param = new OracleParameter();
                param.ParameterName = "MilitaryCommandSuffix";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(technicsRequestsCommand.MilitaryCommandSuffix))
                    param.Value = technicsRequestsCommand.MilitaryCommandSuffix;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "DeliveryCityID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (technicsRequestsCommand.DeliveryCity != null)
                    param.Value = technicsRequestsCommand.DeliveryCity.CityId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "DeliveryPlace";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(technicsRequestsCommand.DeliveryPlace))
                    param.Value = technicsRequestsCommand.DeliveryPlace;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AppointmentTime";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (technicsRequestsCommand.AppointmentTime.HasValue)
                    param.Value = technicsRequestsCommand.AppointmentTime.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilReadinessID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (technicsRequestsCommand.MilitaryReadinessId.HasValue)
                    param.Value = technicsRequestsCommand.MilitaryReadinessId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (changeEvent != null && changeEvent.ChangeEventDetails.Count > 0)
                    EquipmentTechnicsRequestUtil.SetEquipmentTechnicsRequestModified(technicsRequestsCommand.EquipmentTechnicsRequestId, currentUser);
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