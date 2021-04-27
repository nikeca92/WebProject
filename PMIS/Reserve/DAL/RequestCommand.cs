using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using System.Linq;

using PMIS.Common;

namespace PMIS.Reserve.Common
{
    //This class represents a particular command added into a particular request (for equipment with reservists)
    public class RequestCommand : BaseDbObject
    {
        private int requestCommandId;
        private int equipmentReservistsRequestId;
        private EquipmentReservistsRequest equipmentReservistsRequest;
        private MilitaryCommand militaryCommand;
        private string militaryCommandSuffix;
        private City deliveryCity;
        private string deliveryPlace;
        private List<RequestCommandPosition> requestCommandPositions;
        private decimal? appointmentTime;
        private int? militaryReadinessId;
        private MilitaryReadiness militaryReadiness;

        public int RequestCommandId
        {
            get
            {
                return requestCommandId;
            }
            set
            {
                requestCommandId = value;
            }
        }

        public int EquipmentReservistsRequestId
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

        public EquipmentReservistsRequest EquipmentReservistsRequest
        {
            get
            {
                //Lazy initialization
                if (equipmentReservistsRequest == null)
                    equipmentReservistsRequest = EquipmentReservistsRequestUtil.GetEquipmentReservistsRequest(EquipmentReservistsRequestId, CurrentUser);

                return equipmentReservistsRequest;
            }
            set
            {
                equipmentReservistsRequest = value;
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

        public List<RequestCommandPosition> RequestCommandPositions
        {
            get
            {
                //Lazy initialization; It would be used only when there is a need of it
                //When we populate all request commands for a request then we load the positions too (without using this lazy initialization)
                if (requestCommandPositions == null)
                {
                    requestCommandPositions = RequestCommandPositionUtil.GetRequestCommandPositions(CurrentUser, RequestCommandId);
                }

                return requestCommandPositions;
            }
            set
            {
                requestCommandPositions = value;
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
                return (String.IsNullOrEmpty(MilitaryCommandSuffix) ? " " : MilitaryCommandSuffix) + " / " + EquipmentReservistsRequest.RequestNumber;
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

        public RequestCommand(User user)
            : base(user)
        {

        }
    }

    //Some methods for working with RequestCommand objects
    public static class RequestCommandUtil
    {
        //This method creates and returns a EquipmentReservistsRequest object. It extracts the data from a DataReader.
        //This is done in this way to have a signle place of creation of this object, however, pass the particular DataReader
        //object from various queries for better performance instead of executing a new call to the DB here to get the details for a specific EquipWithResRequestsStatusID, for example.
        public static RequestCommand ExtractRequestCommandFromDataReader(OracleDataReader dr, User currentUser)
        {
            RequestCommand requestCommand = new RequestCommand(currentUser);

            requestCommand.RequestCommandId = DBCommon.GetInt(dr["RequestsCommandID"]);
            requestCommand.EquipmentReservistsRequestId = DBCommon.GetInt(dr["EquipmentReservistsRequestID"]);
            requestCommand.MilitaryCommand = MilitaryCommandUtil.ExtractMilitaryCommandFromDR(currentUser, dr);
            requestCommand.MilitaryCommandSuffix = dr["MilitaryCommandSuffix"].ToString();

            if (DBCommon.IsInt(dr["DeliveryCityID"]))
                requestCommand.DeliveryCity = CityUtil.GetCity(DBCommon.GetInt(dr["DeliveryCityID"]), currentUser);

            requestCommand.DeliveryPlace = dr["DeliveryPlace"].ToString();

            if (DBCommon.IsDecimal(dr["AppointmentTime"]))
                requestCommand.AppointmentTime = DBCommon.GetDecimal(dr["AppointmentTime"]);

            if (DBCommon.IsInt(dr["MilReadinessID"]))
                requestCommand.MilitaryReadinessId = DBCommon.GetInt(dr["MilReadinessID"]);

            return requestCommand;
        }

        //Get a specific RequestCommand record
        public static RequestCommand GetRequestsCommand(User currentUser, int requestsCommandId)
        {
            RequestCommand requestCommand = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.RequestsCommandID,
                                      a.EquipmentReservistsRequestID,
                                      a.MilitaryCommandID, b.IMEES as ShortName, b.NK as CommandNumber,
                                      b.KOD_MIR2 as MilitaryUnitID,
                                      a.MilitaryCommandSuffix,
                                      a.DeliveryCityID,
                                      a.DeliveryPlace,
                                      a.AppointmentTime,
                                      a.MilReadinessID
                               FROM PMIS_RES.RequestsCommands a
                               INNER JOIN UKAZ_OWNER.VVR b ON a.MilitaryCommandID = b.KOD_VVR
                               WHERE a.RequestsCommandID = :RequestsCommandID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RequestsCommandID", OracleType.Number).Value = requestsCommandId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    requestCommand = ExtractRequestCommandFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return requestCommand;
        }

        //Get a list of all RequestCommand records for a particular request for equipment with reservists
        public static List<RequestCommand> GetRequestCommandsForRequest(User currentUser, int equipmentReservistsRequestId)
        {
            List<RequestCommand> requestCommands = new List<RequestCommand>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.RequestsCommandID,
                                      a.EquipmentReservistsRequestID,
                                      a.MilitaryCommandID, b.IMEES as ShortName, b.NK as CommandNumber,
                                      b.KOD_MIR2 as MilitaryUnitID,
                                      a.MilitaryCommandSuffix,
                                      a.DeliveryCityID,
                                      a.DeliveryPlace,
                                      a.AppointmentTime,
                                      a.MilReadinessID,

                                      c.RequestCommandPositionID,
                                      c.Position,
                                      c2.MilitaryRankID, d.ZVA_IMEES as MilRankShortName, d.ZVA_IME as MilRankLongName, kat.KAT_IME as MilCategoryName, c2.IsPrimary as IsPrimaryRank,
                                      c.ReservistsCount,
                                      c.PositionType,
                                      c.Seq, 
                                      e.IsPrimary,
                                      f.MilReportSpecialityID,
                                      f.Type,
                                      f.MilReportingSpecialityName,
                                      f.MilReportingSpecialityCode,
                                      f.Active as MilReportingSpecialityActive,
                                      f.MilitaryForceSortID
                               FROM PMIS_RES.RequestsCommands a
                               INNER JOIN UKAZ_OWNER.VVR b ON a.MilitaryCommandID = b.KOD_VVR
                               LEFT OUTER JOIN PMIS_RES.RequestCommandPositions c ON a.RequestsCommandID = c.RequestsCommandID
                               LEFT OUTER JOIN PMIS_RES.CommandPositionMilRanks c2 ON c.RequestCommandPositionID = c2.RequestCommandPositionID
                               LEFT OUTER JOIN VS_OWNER.KLV_ZVA d ON c2.MilitaryRankID = d.ZVA_KOD
                               LEFT OUTER JOIN VS_OWNER.KLV_KAT kat ON d.ZVA_KAT_KOD = kat.KAT_KOD
                               LEFT OUTER JOIN PMIS_RES.CommandPositionMRSpecialities e ON c.RequestCommandPositionID = e.RequestCommandPositionID
                               LEFT OUTER JOIN PMIS_ADM.MilitaryReportSpecialities f ON e.MilReportSpecialityID = f.MilReportSpecialityID
                               WHERE a.EquipmentReservistsRequestID = :EquipmentReservistsRequestID
                               ORDER BY a.RequestsCommandID, c.Seq, c.RequestCommandPositionID, 
                                        NVL(c2.IsPrimary, 0) DESC, d.ZVA_KOD, 
                                        NVL(e.IsPrimary, 0) DESC, f.MilReportingSpecialityCode";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("EquipmentReservistsRequestID", OracleType.Number).Value = equipmentReservistsRequestId;

                OracleDataReader dr = cmd.ExecuteReader();

                int prevRequestsCommandId = 0;
                int prevRequestCommandPositionId = 0;
                int prevMilitaryRankId = 0;
                RequestCommand requestCommand = null;
                RequestCommandPosition position = null;

                while (dr.Read())
                {
                    int requestsCommandId = DBCommon.GetInt(dr["RequestsCommandID"]);
                    int? requestCommandPositionId = null;
                    
                    if(DBCommon.IsInt(dr["RequestCommandPositionID"]))
                        requestCommandPositionId = DBCommon.GetInt(dr["RequestCommandPositionID"]);

                    int? militaryRankId = null;

                    if (DBCommon.IsInt(dr["MilitaryRankID"]))
                        militaryRankId = DBCommon.GetInt(dr["MilitaryRankID"]);

                    //The Request Command has changed - then add a new object
                    //It is changes when all positions of the command are passed
                    if (prevRequestsCommandId != requestsCommandId)
                    {
                        requestCommand = ExtractRequestCommandFromDataReader(dr, currentUser);
                        requestCommand.RequestCommandPositions = new List<RequestCommandPosition>();

                        requestCommands.Add(requestCommand);

                        prevRequestsCommandId = requestsCommandId;
                    }

                    if (requestCommandPositionId.HasValue)
                    {
                        if (prevRequestCommandPositionId != requestCommandPositionId)
                        {
                            position = RequestCommandPositionUtil.ExtractRequestCommandPositionFromDataReader(dr, currentUser);
                            position.MilitaryRanks = new List<CommandPositionMilitaryRank>();
                            position.MilitaryReportSpecialities = new List<CommandPositionMilitaryReportSpeciality>();

                            requestCommand.RequestCommandPositions.Add(position);

                            prevRequestCommandPositionId = requestCommandPositionId.Value;
                        }

                        if (militaryRankId.HasValue)
                        {
                            if (prevMilitaryRankId != militaryRankId)
                            {
                                MilitaryRank milRank = MilitaryRankUtil.ExtractMilitaryRankFromDR(currentUser, dr);
                                CommandPositionMilitaryRank positionRank = new CommandPositionMilitaryRank();
                                positionRank.Rank = milRank;
                                positionRank.IsPrimary = DBCommon.IsInt(dr["IsPrimaryRank"]) && DBCommon.GetInt(dr["IsPrimaryRank"]) == 1;
                                position.MilitaryRanks.Add(positionRank);
                                prevMilitaryRankId = militaryRankId.Value;
                            }
                        }
                        else
                        {
                            prevMilitaryRankId = 0;
                        }

                        if (DBCommon.IsInt(dr["RequestCommandPositionID"]) && DBCommon.IsInt(dr["MilReportSpecialityID"]) &&
                            !position.MilitaryReportSpecialities.Any(x => x.Speciality.MilReportSpecialityId == DBCommon.GetInt(dr["MilReportSpecialityID"]))
                           )
                        {
                            MilitaryReportSpeciality militaryReportSpeciality = MilitaryReportSpecialityUtil.ExtractMilitaryReportSpecialityFromDR(currentUser, dr);
                            CommandPositionMilitaryReportSpeciality positionSpeciality = new CommandPositionMilitaryReportSpeciality();
                            positionSpeciality.Speciality = militaryReportSpeciality;
                            positionSpeciality.IsPrimary = DBCommon.IsInt(dr["IsPrimary"]) && DBCommon.GetInt(dr["IsPrimary"]) == 1;
                            position.MilitaryReportSpecialities.Add(positionSpeciality);
                        }
                    }
                    else
                    {
                        prevRequestCommandPositionId = 0;
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return requestCommands;
        }

        //Get a list of all RequestCommand records for a particular request for equipment with reservists and for particular Military Department
        public static List<RequestCommand> GetRequestCommandsForRequestAndMilDept(User currentUser, int equipmentReservistsRequestId, int militaryDepartmentId)
        {
            List<RequestCommand> requestCommands = new List<RequestCommand>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.RequestsCommandID,
                                      a.EquipmentReservistsRequestID,
                                      a.MilitaryCommandID, b.IMEES as ShortName, b.NK as CommandNumber,
                                      b.KOD_MIR2 as MilitaryUnitID,
                                      a.MilitaryCommandSuffix,
                                      a.DeliveryCityID,
                                      a.DeliveryPlace,
                                      a.AppointmentTime,
                                      a.MilReadinessID,

                                      c.RequestCommandPositionID,
                                      c.Position,
                                      c2.MilitaryRankID, d.ZVA_IMEES as MilRankShortName, d.ZVA_IME as MilRankLongName, kat.KAT_IME as MilCategoryName, c2.IsPrimary as IsPrimaryRank,
                                      c.ReservistsCount,
                                      c.PositionType,
                                      c.Seq,
                                      e.IsPrimary,
                                      f.MilReportSpecialityID,
                                      f.Type,
                                      f.MilReportingSpecialityName,
                                      f.MilReportingSpecialityCode,
                                      f.Active as MilReportingSpecialityActive,
                                      f.MilitaryForceSortID
                               FROM PMIS_RES.RequestsCommands a
                               INNER JOIN UKAZ_OWNER.VVR b ON a.MilitaryCommandID = b.KOD_VVR
                               LEFT OUTER JOIN PMIS_RES.RequestCommandPositions c ON a.RequestsCommandID = c.RequestsCommandID
                               LEFT OUTER JOIN PMIS_RES.CommandPositionMilRanks c2 ON c.RequestCommandPositionID = c2.RequestCommandPositionID
                               LEFT OUTER JOIN VS_OWNER.KLV_ZVA d ON c2.MilitaryRankID = d.ZVA_KOD
                               LEFT OUTER JOIN VS_OWNER.KLV_KAT kat ON d.ZVA_KAT_KOD = kat.KAT_KOD
                               LEFT OUTER JOIN PMIS_RES.CommandPositionMRSpecialities e ON c.RequestCommandPositionID = e.RequestCommandPositionID
                               LEFT OUTER JOIN PMIS_ADM.MilitaryReportSpecialities f ON e.MilReportSpecialityID = f.MilReportSpecialityID
                               LEFT OUTER JOIN PMIS_RES.RequestCommandPositionsMilDept g ON g.RequestCommandPositionID = c.RequestCommandPositionID
                               WHERE a.EquipmentReservistsRequestID = :EquipmentReservistsRequestID AND g.MilitaryDepartmentID = :MilitaryDepartmentID
                               ORDER BY a.RequestsCommandID, c.RequestCommandPositionID,
                                        NVL(c2.IsPrimary, 0) DESC, d.ZVA_KOD, 
                                        NVL(e.IsPrimary, 0) DESC, f.MilReportingSpecialityCode";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("EquipmentReservistsRequestID", OracleType.Number).Value = equipmentReservistsRequestId;
                cmd.Parameters.Add("MilitaryDepartmentID", OracleType.Number).Value = militaryDepartmentId;

                OracleDataReader dr = cmd.ExecuteReader();

                int prevRequestsCommandId = 0;
                int prevRequestCommandPositionId = 0;
                int prevMilitaryRankId = 0;
                RequestCommand requestCommand = null;
                RequestCommandPosition position = null;

                while (dr.Read())
                {
                    int requestsCommandId = DBCommon.GetInt(dr["RequestsCommandID"]);
                    int? requestCommandPositionId = null;

                    if (DBCommon.IsInt(dr["RequestCommandPositionID"]))
                        requestCommandPositionId = DBCommon.GetInt(dr["RequestCommandPositionID"]);

                    int? militaryRankId = null;

                    if (DBCommon.IsInt(dr["MilitaryRankID"]))
                        militaryRankId = DBCommon.GetInt(dr["MilitaryRankID"]);

                    //The Request Command has changed - then add a new object
                    //It is changes when all positions of the command are passed
                    if (prevRequestsCommandId != requestsCommandId)
                    {
                        requestCommand = ExtractRequestCommandFromDataReader(dr, currentUser);
                        requestCommand.RequestCommandPositions = new List<RequestCommandPosition>();

                        requestCommands.Add(requestCommand);

                        prevRequestsCommandId = requestsCommandId;
                    }

                    if (requestCommandPositionId.HasValue)
                    {
                        if (prevRequestCommandPositionId != requestCommandPositionId)
                        {
                            position = RequestCommandPositionUtil.ExtractRequestCommandPositionFromDataReader(dr, currentUser);
                            position.MilitaryRanks = new List<CommandPositionMilitaryRank>();
                            position.MilitaryReportSpecialities = new List<CommandPositionMilitaryReportSpeciality>();

                            requestCommand.RequestCommandPositions.Add(position);

                            prevRequestCommandPositionId = requestCommandPositionId.Value;
                        }

                        if (militaryRankId.HasValue)
                        {
                            if (prevMilitaryRankId != militaryRankId)
                            {
                                MilitaryRank milRank = MilitaryRankUtil.ExtractMilitaryRankFromDR(currentUser, dr);
                                CommandPositionMilitaryRank positionRank = new CommandPositionMilitaryRank();
                                positionRank.Rank = milRank;
                                positionRank.IsPrimary = DBCommon.IsInt(dr["IsPrimaryRank"]) && DBCommon.GetInt(dr["IsPrimaryRank"]) == 1;
                                position.MilitaryRanks.Add(positionRank);
                                prevMilitaryRankId = militaryRankId.Value;
                            }
                        }
                        else
                        {
                            prevMilitaryRankId = 0;
                        }

                        if (DBCommon.IsInt(dr["RequestCommandPositionID"]) && DBCommon.IsInt(dr["MilReportSpecialityID"]) &&
                            !position.MilitaryReportSpecialities.Any(x => x.Speciality.MilReportSpecialityId == DBCommon.GetInt(dr["MilReportSpecialityID"]))
                           )
                        {
                            MilitaryReportSpeciality militaryReportSpeciality = MilitaryReportSpecialityUtil.ExtractMilitaryReportSpecialityFromDR(currentUser, dr);
                            CommandPositionMilitaryReportSpeciality positionSpeciality = new CommandPositionMilitaryReportSpeciality();
                            positionSpeciality.Speciality = militaryReportSpeciality;
                            positionSpeciality.IsPrimary = DBCommon.IsInt(dr["IsPrimary"]) && DBCommon.GetInt(dr["IsPrimary"]) == 1;
                            position.MilitaryReportSpecialities.Add(positionSpeciality);
                        }
                    }
                    else
                    {
                        prevRequestCommandPositionId = 0;
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return requestCommands;
        }

        //Get a list of all RequestCommand records for a particular request for equipment with reservists and for particular Military Department
        public static List<DropDownItem> GetRequestCommandsForMilCommandAndMilDeptAndMilReadiness(User currentUser, int militaryCommandId, string militaryDepartmentIds, string militaryReadinessID)
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
                               FROM PMIS_RES.RequestsCommands a
                               INNER JOIN UKAZ_OWNER.VVR b ON a.MilitaryCommandID = b.KOD_VVR
                               INNER JOIN PMIS_RES.RequestCommandPositions c ON a.RequestsCommandID = c.RequestsCommandID                                             
                               INNER JOIN PMIS_RES.RequestCommandPositionsMilDept g ON g.RequestCommandPositionID = c.RequestCommandPositionID
                               WHERE " + (string.IsNullOrEmpty(militaryReadinessID) ? "" : " a.milreadinessid = " + CommonFunctions.AvoidSQLInjForListOfIDs(militaryReadinessID) + " AND") + @"
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

        //Get a list of all RequestCommand records for a particular request for equipment with reservists and for particular Military Department
        public static List<DropDownItem> GetRequestCommandsForMilCommandAndMilDept(User currentUser, int militaryCommandId, string militaryDepartmentIds)
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
                               FROM PMIS_RES.RequestsCommands a
                               INNER JOIN UKAZ_OWNER.VVR b ON a.MilitaryCommandID = b.KOD_VVR
                               INNER JOIN PMIS_RES.RequestCommandPositions c ON a.RequestsCommandID = c.RequestsCommandID                                             
                               INNER JOIN PMIS_RES.RequestCommandPositionsMilDept g ON g.RequestCommandPositionID = c.RequestCommandPositionID
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

        public static List<DropDownItem> GetRequestCommandsForMilCommand(User currentUser, int militaryCommandId)
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
                               FROM PMIS_RES.RequestsCommands a
                               INNER JOIN UKAZ_OWNER.VVR b ON a.MilitaryCommandID = b.KOD_VVR
                               WHERE a.MilitaryCommandID = :MilitaryCommandID
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

        //Get a list of all RequestCommand records for a particular request for equipment with reservists and for particular Military Department
        public static List<RequestCommand> GetAllRequestCommandForMilCommandAndMilDept(User currentUser, int militaryCommandId, int militaryDepartmentId)
        {
            List<RequestCommand> requestCommands = new List<RequestCommand>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.RequestsCommandID,
                                      a.EquipmentReservistsRequestID,
                                      a.MilitaryCommandID, b.IMEES as ShortName, b.NK as CommandNumber,
                                      b.KOD_MIR2 as MilitaryUnitID,
                                      a.MilitaryCommandSuffix,
                                      a.DeliveryCityID,
                                      a.DeliveryPlace,
                                      a.AppointmentTime,
                                      a.MilReadinessID,

                                      c.RequestCommandPositionID,
                                      c.Position,
                                      c2.MilitaryRankID, d.ZVA_IMEES as MilRankShortName, d.ZVA_IME as MilRankLongName, kat.KAT_IME as MilCategoryName, c2.IsPrimary as IsPrimaryRank,
                                      c.ReservistsCount,
                                      c.PositionType,
                                      c.Seq,
                                      e.IsPrimary,
                                      f.MilReportSpecialityID,
                                      f.Type,
                                      f.MilReportingSpecialityName,
                                      f.MilReportingSpecialityCode,
                                      f.Active as MilReportingSpecialityActive,
                                      f.MilitaryForceSortID
                               FROM PMIS_RES.RequestsCommands a
                               INNER JOIN UKAZ_OWNER.VVR b ON a.MilitaryCommandID = b.KOD_VVR
                               LEFT OUTER JOIN PMIS_RES.RequestCommandPositions c ON a.RequestsCommandID = c.RequestsCommandID
                               LEFT OUTER JOIN PMIS_RES.CommandPositionMilRanks c2 ON c.RequestCommandPositionID = c2.RequestCommandPositionID
				               LEFT OUTER JOIN VS_OWNER.KLV_ZVA d ON c2.MilitaryRankID = d.ZVA_KOD
                               LEFT OUTER JOIN VS_OWNER.KLV_KAT kat ON d.ZVA_KAT_KOD = kat.KAT_KOD
                               LEFT OUTER JOIN PMIS_RES.CommandPositionMRSpecialities e ON c.RequestCommandPositionID = e.RequestCommandPositionID
                               LEFT OUTER JOIN PMIS_ADM.MilitaryReportSpecialities f ON e.MilReportSpecialityID = f.MilReportSpecialityID
                               LEFT OUTER JOIN PMIS_RES.RequestCommandPositionsMilDept g ON g.RequestCommandPositionID = c.RequestCommandPositionID
                               WHERE a.MilitaryCommandID = :MilitaryCommandID AND g.MilitaryDepartmentID = :MilitaryDepartmentID
                               ORDER BY a.MilitaryCommandSuffix NULLS FIRST, a.RequestsCommandID, c.RequestCommandPositionID, 
                                     NVL(c2.IsPrimary, 0) DESC, d.ZVA_KOD,
                                     NVL(e.IsPrimary, 0) DESC, f.MilReportingSpecialityCode";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitaryCommandID", OracleType.Number).Value = militaryCommandId;
                cmd.Parameters.Add("MilitaryDepartmentID", OracleType.Number).Value = militaryDepartmentId;

                OracleDataReader dr = cmd.ExecuteReader();

                int prevRequestsCommandId = 0;
                int prevRequestCommandPositionId = 0;
                int prevMilitaryRankId = 0;
                RequestCommand requestCommand = null;
                RequestCommandPosition position = null;

                while (dr.Read())
                {
                    int requestsCommandId = DBCommon.GetInt(dr["RequestsCommandID"]);
                    int? requestCommandPositionId = null;

                    if (DBCommon.IsInt(dr["RequestCommandPositionID"]))
                        requestCommandPositionId = DBCommon.GetInt(dr["RequestCommandPositionID"]);

                    int? militaryRankId = null;

                    if (DBCommon.IsInt(dr["MilitaryRankID"]))
                        militaryRankId = DBCommon.GetInt(dr["MilitaryRankID"]);

                    //The Request Command has changed - then add a new object
                    //It is changes when all positions of the command are passed
                    if (prevRequestsCommandId != requestsCommandId)
                    {
                        requestCommand = ExtractRequestCommandFromDataReader(dr, currentUser);
                        requestCommand.RequestCommandPositions = new List<RequestCommandPosition>();

                        requestCommands.Add(requestCommand);

                        prevRequestsCommandId = requestsCommandId;
                    }

                    if (requestCommandPositionId.HasValue)
                    {
                        if (prevRequestCommandPositionId != requestCommandPositionId)
                        {
                            position = RequestCommandPositionUtil.ExtractRequestCommandPositionFromDataReader(dr, currentUser);
                            position.MilitaryRanks = new List<CommandPositionMilitaryRank>();
                            position.MilitaryReportSpecialities = new List<CommandPositionMilitaryReportSpeciality>();

                            requestCommand.RequestCommandPositions.Add(position);

                            prevRequestCommandPositionId = requestCommandPositionId.Value;
                        }

                        if (militaryRankId.HasValue)
                        {
                            if (prevMilitaryRankId != militaryRankId)
                            {
                                MilitaryRank milRank = MilitaryRankUtil.ExtractMilitaryRankFromDR(currentUser, dr);
                                CommandPositionMilitaryRank positionRank = new CommandPositionMilitaryRank();
                                positionRank.Rank = milRank;
                                positionRank.IsPrimary = DBCommon.IsInt(dr["IsPrimaryRank"]) && DBCommon.GetInt(dr["IsPrimaryRank"]) == 1;
                                position.MilitaryRanks.Add(positionRank);
                                prevMilitaryRankId = militaryRankId.Value;
                            }
                        }
                        else
                        {
                            prevMilitaryRankId = 0;
                        }

                        if (DBCommon.IsInt(dr["RequestCommandPositionID"]) && DBCommon.IsInt(dr["MilReportSpecialityID"]) &&
                            !position.MilitaryReportSpecialities.Any(x => x.Speciality.MilReportSpecialityId == DBCommon.GetInt(dr["MilReportSpecialityID"]))
                           )
                        {
                            MilitaryReportSpeciality militaryReportSpeciality = MilitaryReportSpecialityUtil.ExtractMilitaryReportSpecialityFromDR(currentUser, dr);
                            CommandPositionMilitaryReportSpeciality positionSpeciality = new CommandPositionMilitaryReportSpeciality();
                            positionSpeciality.Speciality = militaryReportSpeciality;
                            positionSpeciality.IsPrimary = DBCommon.IsInt(dr["IsPrimary"]) && DBCommon.GetInt(dr["IsPrimary"]) == 1;
                            position.MilitaryReportSpecialities.Add(positionSpeciality);
                        }
                    }
                    else
                    {
                        prevRequestCommandPositionId = 0;
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return requestCommands;
        }

        //Add a new command to a particular request
        public static RequestCommand AddRequestCommand(User currentUser, int equipmentReservistsRequestId, int militaryCommandId,
                                                       Change changeEntry)
        {
            RequestCommand requestCommand = null;

            ChangeEvent changeEvent = null;

            EquipmentReservistsRequest equipmentReservistsRequest = EquipmentReservistsRequestUtil.GetEquipmentReservistsRequest(equipmentReservistsRequestId, currentUser);

            string logDescription = "";
            logDescription += "Заявка №: " + equipmentReservistsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(equipmentReservistsRequest.RequestDate);

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string SQL = "";

            try
            {
                
                SQL += @"BEGIN
                            INSERT INTO PMIS_RES.RequestsCommands (EquipmentReservistsRequestID,
                                MilitaryCommandID, MilitaryCommandSuffix, DeliveryCityID, DeliveryPlace, AppointmentTime, MilReadinessID)
                            VALUES (:EquipmentReservistsRequestID,
                                :MilitaryCommandID, NULL, NULL, NULL, NULL, NULL);

                            SELECT PMIS_RES.RequestsCommands_ID_SEQ.currval INTO :RequestsCommandID FROM dual;
                         END;";

                changeEvent = new ChangeEvent("RES_EquipResRequests_AddCommand", logDescription, equipmentReservistsRequest.MilitaryUnit, null, currentUser);

                MilitaryCommand militaryCommand = MilitaryCommandUtil.GetMilitaryCommand(militaryCommandId, currentUser);

                changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_Command_Name", "", militaryCommand.DisplayTextForSelection, currentUser));

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramRequestsCommandID = new OracleParameter();
                paramRequestsCommandID.ParameterName = "RequestsCommandID";
                paramRequestsCommandID.OracleType = OracleType.Number;
                paramRequestsCommandID.Direction = ParameterDirection.Output;

                cmd.Parameters.Add(paramRequestsCommandID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "EquipmentReservistsRequestID";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = equipmentReservistsRequestId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryCommandID";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = militaryCommandId;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                int requestsCommandId = DBCommon.GetInt(paramRequestsCommandID.Value);

                requestCommand = RequestCommandUtil.GetRequestsCommand(currentUser, requestsCommandId);

                EquipmentReservistsRequestUtil.SetEquipmentReservistsRequestModified(equipmentReservistsRequestId, currentUser);
            }
            finally
            {
                conn.Close();
            }

            if (changeEvent != null && changeEvent.ChangeEventDetails.Count > 0)
                changeEntry.AddEvent(changeEvent);

            return requestCommand;
        }


        //Delete a particualr command from a particular request
        public static void DeleteRequestCommand(User currentUser, int requestsCommandId, Change changeEntry)
        {
            ChangeEvent changeEvent = null;

            RequestCommand requestCommand = GetRequestsCommand(currentUser, requestsCommandId);

            string logDescription = "";
            logDescription += "Заявка №: " + requestCommand.EquipmentReservistsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(requestCommand.EquipmentReservistsRequest.RequestDate);

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string SQL = "";

            try
            {

                SQL += @"BEGIN
                            DELETE FROM PMIS_RES.RequestCommandPositionsMilDept
                            WHERE RequestCommandPositionID IN (SELECT RequestCommandPositionID
                                                               FROM PMIS_RES.RequestCommandPositions
                                                               WHERE RequestsCommandID = :RequestsCommandID);

                            DELETE FROM PMIS_RES.CommandPositionMRSpecialities
                            WHERE RequestCommandPositionID IN (SELECT RequestCommandPositionID
                                                               FROM PMIS_RES.RequestCommandPositions
                                                               WHERE RequestsCommandID = :RequestsCommandID);

                            UPDATE PMIS_RES.Reservists SET
                               PunktID = NULL
                            WHERE PunktID IN (SELECT RequestCommandPunktID
                                              FROM PMIS_RES.RequestCommandPunkt
                                              WHERE RequestCommandID = :RequestsCommandID
                                              );

                            DELETE FROM PMIS_RES.RequestCommandPunkt
                            WHERE RequestCommandID = :RequestsCommandID;

                            DELETE FROM PMIS_RES.RequestCommandPositions
                            WHERE RequestsCommandID = :RequestsCommandID;

                            DELETE FROM PMIS_RES.RequestsCommands 
                            WHERE RequestsCommandID = :RequestsCommandID;
                         END;";

                changeEvent = new ChangeEvent("RES_EquipResRequests_DeleteCommand", logDescription, requestCommand.EquipmentReservistsRequest.MilitaryUnit, null, currentUser);

                changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_Command_Name", requestCommand.MilitaryCommand.DisplayTextForSelection, "", currentUser));

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RequestsCommandID", OracleType.Number).Value = requestsCommandId;

                cmd.ExecuteNonQuery();

                EquipmentReservistsRequestUtil.SetEquipmentReservistsRequestModified(requestCommand.EquipmentReservistsRequestId, currentUser);
            }
            finally
            {
                conn.Close();
            }

            if (changeEvent != null && changeEvent.ChangeEventDetails.Count > 0)
                changeEntry.AddEvent(changeEvent);
        }

        //Save a particualr command from a particular request
        public static void SaveRequestCommand(User currentUser, RequestCommand requestsCommand, Change changeEntry)
        {
            ChangeEvent changeEvent = null;

            string logDescription = "";
            logDescription += "Заявка №: " + requestsCommand.EquipmentReservistsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(requestsCommand.EquipmentReservistsRequest.RequestDate) +
                              "; Команда: " + requestsCommand.MilitaryCommand.DisplayTextForSelection;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string SQL = "";

            try
            {

                SQL += @"BEGIN
                            UPDATE PMIS_RES.RequestsCommands SET
                               MilitaryCommandSuffix = :MilitaryCommandSuffix, 
                               DeliveryCityID = :DeliveryCityID, 
                               DeliveryPlace = :DeliveryPlace,
                               AppointmentTime = :AppointmentTime,
                               MilReadinessID = :MilReadinessID
                            WHERE RequestsCommandID = :RequestsCommandID;
                         END;";

                changeEvent = new ChangeEvent("RES_EquipResRequests_EditCommand", logDescription, requestsCommand.EquipmentReservistsRequest.MilitaryUnit, null, currentUser);

                RequestCommand oldRequestsCommand = RequestCommandUtil.GetRequestsCommand(currentUser, requestsCommand.RequestCommandId);

                if (oldRequestsCommand.MilitaryCommandSuffix.Trim() != requestsCommand.MilitaryCommandSuffix.Trim())
                    changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_Command_Suffix", oldRequestsCommand.MilitaryCommandSuffix, requestsCommand.MilitaryCommandSuffix, currentUser));

                if ((oldRequestsCommand.DeliveryCity != null && oldRequestsCommand.DeliveryCity.CityName != null ? oldRequestsCommand.DeliveryCity.CityName : "") !=
                   (requestsCommand.DeliveryCity != null && requestsCommand.DeliveryCity.CityName != null ? requestsCommand.DeliveryCity.CityName : ""))
                    changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_Command_DeliveryCity",
                        oldRequestsCommand.DeliveryCity != null && oldRequestsCommand.DeliveryCity.CityName != null ? oldRequestsCommand.DeliveryCity.Region.RegionName + ", " + oldRequestsCommand.DeliveryCity.Municipality.MunicipalityName + ", " + oldRequestsCommand.DeliveryCity.CityName : "",
                        requestsCommand.DeliveryCity != null && requestsCommand.DeliveryCity.CityName != null ? requestsCommand.DeliveryCity.Region.RegionName + ", " + requestsCommand.DeliveryCity.Municipality.MunicipalityName + ", " + requestsCommand.DeliveryCity.CityName : "",
                        currentUser));

                if (oldRequestsCommand.DeliveryPlace.Trim() != requestsCommand.DeliveryPlace.Trim())
                    changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_Command_DeliveryPlace", oldRequestsCommand.DeliveryPlace, requestsCommand.DeliveryPlace, currentUser));

                if (!CommonFunctions.IsEqualDecimal(oldRequestsCommand.AppointmentTime, requestsCommand.AppointmentTime))
                    changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_Command_AppointmentTime", (oldRequestsCommand.AppointmentTime.HasValue ? oldRequestsCommand.AppointmentTime.ToString() : ""), (requestsCommand.AppointmentTime.HasValue ? requestsCommand.AppointmentTime.ToString() : ""), currentUser));

                if (!CommonFunctions.IsEqualInt(oldRequestsCommand.MilitaryReadinessId, requestsCommand.MilitaryReadinessId))
                    changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_Command_MilReadiness", (oldRequestsCommand.MilitaryReadinessId.HasValue ? oldRequestsCommand.MilitaryReadiness.MilReadinessName : ""), (requestsCommand.MilitaryReadinessId.HasValue ? requestsCommand.MilitaryReadiness.MilReadinessName : ""), currentUser));
                
                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RequestsCommandID", OracleType.Number).Value = requestsCommand.RequestCommandId;

                OracleParameter param = new OracleParameter();
                param.ParameterName = "MilitaryCommandSuffix";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(requestsCommand.MilitaryCommandSuffix))
                    param.Value = requestsCommand.MilitaryCommandSuffix;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "DeliveryCityID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (requestsCommand.DeliveryCity != null)
                    param.Value = requestsCommand.DeliveryCity.CityId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "DeliveryPlace";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(requestsCommand.DeliveryPlace))
                    param.Value = requestsCommand.DeliveryPlace;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AppointmentTime";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (requestsCommand.AppointmentTime.HasValue)
                    param.Value = requestsCommand.AppointmentTime.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilReadinessID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (requestsCommand.MilitaryReadinessId.HasValue)
                    param.Value = requestsCommand.MilitaryReadinessId.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (changeEvent != null && changeEvent.ChangeEventDetails.Count > 0)
                    EquipmentReservistsRequestUtil.SetEquipmentReservistsRequestModified(requestsCommand.EquipmentReservistsRequestId, currentUser);
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