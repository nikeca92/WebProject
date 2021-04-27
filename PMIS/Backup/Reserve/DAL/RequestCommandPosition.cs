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
    //This class represents a particular command position
    public class RequestCommandPosition : BaseDbObject
    {
        private int requestCommandPositionId;
        private int requestsCommandId;
        private RequestCommand requestsCommand;
        private string position;
        private string milRankHTML;
        private List<CommandPositionMilitaryRank> militaryRanks;
        private int reservistsCount;
        private int positionType;
        private string milRepSpecHTML;
        private List<CommandPositionMilitaryReportSpeciality> militaryReportSpecialities;
        private List<RequestCommandPositionMilDept> positionMilitaryDepartments;
        private int seq;


        public int RequestCommandPositionId
        {
            get
            {
                return requestCommandPositionId;
            }
            set
            {
                requestCommandPositionId = value;
            }
        }

        public int RequestsCommandId
        {
            get
            {
                return requestsCommandId;
            }
            set
            {
                requestsCommandId = value;
            }
        }

        public RequestCommand RequestsCommand
        {
            get
            {
                //Lazy initialization
                if (requestsCommand == null)
                    requestsCommand = RequestCommandUtil.GetRequestsCommand(CurrentUser, RequestsCommandId);

                return requestsCommand;
            }
            set
            {
                requestsCommand = value;
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

        public string MilRankHTML
        {
            get { return milRankHTML; }
            set { milRankHTML = value; }
        }

        public List<CommandPositionMilitaryRank> MilitaryRanks
        {
            get
            {
                //Lazy initialization. Use it only when the list of specialities isn't already loaded
                //When loading the entire list of position we pull the specialities too
                if (militaryRanks == null)
                    militaryRanks = RequestCommandPositionUtil.GetMilitaryRanksPerPosition(CurrentUser, RequestCommandPositionId);

                return militaryRanks;
            }
            set
            {
                militaryRanks = value;
            }
        }

        public int ReservistsCount
        {
            get
            {
                return reservistsCount;
            }
            set
            {
                reservistsCount = value;
            }
        }

        public int PositionType
        {
            get
            {
                return positionType;
            }
            set
            {
                positionType = value;
            }
        }

        public string MilRepSpecHTML
        {
            get { return milRepSpecHTML; }
            set { milRepSpecHTML = value; }
        }

        public List<CommandPositionMilitaryReportSpeciality> MilitaryReportSpecialities
        {
            get
            {
                //Lazy initialization. Use it only when the list of specialities isn't already loaded
                //When loading the entire list of position we pull the specialities too
                if (militaryReportSpecialities == null)
                    militaryReportSpecialities = RequestCommandPositionUtil.GetMilitaryReportingSpecialitiesPerPosition(CurrentUser, RequestCommandPositionId);

                return militaryReportSpecialities;
            }
            set
            {
                militaryReportSpecialities = value;
            }
        }

        public List<RequestCommandPositionMilDept> PositionMilitaryDepartments
        {
            get 
            {
                //Lazy initialization. Use it only when the list of positions military departments isn't already loaded
                //When loading the entire list of position we pull them too

                if (positionMilitaryDepartments == null)
                    positionMilitaryDepartments = RequestCommandPositionMilDeptUtil.GetAllRequestCommandPositionMilDeptsByCommandPosition(RequestCommandPositionId, CurrentUser);

                return positionMilitaryDepartments; 
            }
            set { positionMilitaryDepartments = value; }
        }

        public string MilitaryReportSpecialitiesString
        {
            get
            {
                string militaryReportSpecialitiesString = "";

                foreach (CommandPositionMilitaryReportSpeciality speciality in MilitaryReportSpecialities)
                {
                    militaryReportSpecialitiesString += (militaryReportSpecialitiesString == "" ? "" : ", ") +
                        speciality.Speciality.CodeAndName + (speciality.IsPrimary ? " (основна)" : "");
                }

                return militaryReportSpecialitiesString;
            }
        }

        public string MilitaryRanksString
        {
            get
            {
                string militaryRanksString = "";

                foreach (CommandPositionMilitaryRank rank in MilitaryRanks)
                {
                    militaryRanksString += (militaryRanksString == "" ? "" : ", ") +
                        rank.Rank.LongName + (rank.IsPrimary ? " (основно)" : "");
                }

                return militaryRanksString;
            }
        }
        

        public bool CanDelete
        {
            get { return true; }

        }

        public RequestCommandPosition(User user)
            : base(user)
        {

        }

        public int Seq 
        {
            get { return seq; }
            set { seq = value; }
        }
    }

    public class CommandPositionMilitaryReportSpeciality
    {
        public MilitaryReportSpeciality Speciality { get; set; }
        public bool IsPrimary { get; set; }
    }

    public class CommandPositionMilitaryRank
    {
        public MilitaryRank Rank { get; set; }
        public bool IsPrimary { get; set; }
    }


    public class RequestCommandPositionBlockForFulfilment : BaseDbObject
    {
        private int requestCommandPositionId;
        private int requestsCommandId;
        private RequestCommand requestsCommand;
        private string position;
        private string milRankHTML;
        private int reservistsCount;
        private int positionType;
        private string milRepSpecHTML;
        private int fulfiled;        
        private int fulfiledReserve;        

        public int RequestCommandPositionId
        {
            get
            {
                return requestCommandPositionId;
            }
            set
            {
                requestCommandPositionId = value;
            }
        }

        public int RequestsCommandId
        {
            get
            {
                return requestsCommandId;
            }
            set
            {
                requestsCommandId = value;
            }
        }

        public RequestCommand RequestsCommand
        {
            get
            {
                //Lazy initialization
                if (requestsCommand == null)
                    requestsCommand = RequestCommandUtil.GetRequestsCommand(CurrentUser, RequestsCommandId);

                return requestsCommand;
            }
            set
            {
                requestsCommand = value;
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

        public string MilRankHTML
        {
            get { return milRankHTML; }
            set { milRankHTML = value; }
        }

        public int ReservistsCount
        {
            get
            {
                return reservistsCount;
            }
            set
            {
                reservistsCount = value;
            }
        }

        public int PositionType
        {
            get
            {
                return positionType;
            }
            set
            {
                positionType = value;
            }
        }

        public string MilRepSpecHTML
        {
            get { return milRepSpecHTML; }
            set { milRepSpecHTML = value; }
        }

        public int Fulfiled
        {
            get { return fulfiled; }
            set { fulfiled = value; }
        }

        public int FulfiledReserve
        {
            get { return fulfiledReserve; }
            set { fulfiledReserve = value; }
        }

        public RequestCommandPositionBlockForFulfilment(User user)
            : base(user)
        {

        }
    }

    //Some methods for working with RequestCommandPosition objects
    public static class RequestCommandPositionUtil
    {
        //This method creates and returns a RequestCommandPosition object. It extracts the data from a DataReader.
        //This is done in this way to have a signle place of creation of this object, however, pass the particular DataReader
        //object from various queries for better performance instead of executing a new call to the DB here to get the details for a specific ID, for example.
        public static RequestCommandPosition ExtractRequestCommandPositionFromDataReader(OracleDataReader dr, User currentUser)
        {
            RequestCommandPosition requestCommandPosition = new RequestCommandPosition(currentUser);

            requestCommandPosition.RequestCommandPositionId = DBCommon.GetInt(dr["RequestCommandPositionID"]);
            requestCommandPosition.RequestsCommandId = DBCommon.GetInt(dr["RequestsCommandID"]);
            requestCommandPosition.Position = dr["Position"].ToString();
            requestCommandPosition.Seq = DBCommon.GetInt(dr["Seq"]);

            if (DBCommon.IsInt(dr["ReservistsCount"]))
                requestCommandPosition.ReservistsCount = DBCommon.GetInt(dr["ReservistsCount"]);

            requestCommandPosition.PositionType = DBCommon.GetInt(dr["PositionType"]);
            
            return requestCommandPosition;
        }

        //Get a specific RequestCommandPosition record
        public static RequestCommandPosition GetRequestCommandPosition(User currentUser, int requestCommandPositionId)
        {
            RequestCommandPosition requestCommandPosition = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.RequestCommandPositionID,
                                      a.RequestsCommandID,
                                      a.Position,
                                      a.ReservistsCount,
                                      a.PositionType,
                                      a.Seq
                               FROM PMIS_RES.RequestCommandPositions a
                               WHERE a.RequestCommandPositionID = :RequestCommandPositionID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RequestCommandPositionID", OracleType.Number).Value = requestCommandPositionId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    requestCommandPosition = ExtractRequestCommandPositionFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return requestCommandPosition;
        }

        public static bool SwapRequestCommandPositionsOrder(User pCurrentUser, Change pChangeEntry, RequestCommandPosition pRequestCommandPosition_1, RequestCommandPosition pRequestCommandPosition_2)
        {
            bool result = false;
            string SQL = "";
            SQL += @"BEGIN
                     UPDATE PMIS_RES.RequestCommandPositions t
                     SET Seq = (SELECT Seq 
                                FROM PMIS_RES.RequestCommandPositions t2 
                                WHERE t2.RequestCommandPositionID = :RequestsCommandPositionID_2)
                     WHERE t.RequestsCommandID = :RequestsCommandID AND
                           t.RequestCommandPositionID = :RequestsCommandPositionID_1;
                     
                     UPDATE PMIS_RES.RequestCommandPositions t2
                     SET Seq = :TmpSeq
                     WHERE t2.RequestsCommandID = :RequestsCommandID AND
                           t2.RequestCommandPositionID = :RequestsCommandPositionID_2;
                    END;";

            ChangeEvent changeEvent = null;
            string logDescription = "";
            logDescription += "Заявка №: " + pRequestCommandPosition_1.RequestsCommand.EquipmentReservistsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(pRequestCommandPosition_1.RequestsCommand.EquipmentReservistsRequest.RequestDate) +
                              "; Команда: " + pRequestCommandPosition_1.RequestsCommand.MilitaryCommand.DisplayTextForSelection +
                              "; Длъжност: " + pRequestCommandPosition_1.Position;

            OracleConnection conn = new OracleConnection(pCurrentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "RequestsCommandID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = pRequestCommandPosition_1.RequestsCommand.RequestCommandId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "RequestsCommandPositionID_1";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = pRequestCommandPosition_1.RequestCommandPositionId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "RequestsCommandPositionID_2";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = pRequestCommandPosition_2.RequestCommandPositionId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "TmpSeq";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.InputOutput;
                param.Value = pRequestCommandPosition_1.Seq;
                cmd.Parameters.Add(param);
                                
                cmd.ExecuteNonQuery();
                changeEvent = new ChangeEvent("RES_EquipResRequests_Command_MovePosition", logDescription, pRequestCommandPosition_1.RequestsCommand.EquipmentReservistsRequest.MilitaryUnit, null, pCurrentUser);

                changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_Command_Position_Seq", pRequestCommandPosition_1.Seq.ToString(), pRequestCommandPosition_2.Seq.ToString(), pCurrentUser));

                result = true;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                if (changeEvent != null && changeEvent.ChangeEventDetails.Count > 0)
                    pChangeEntry.AddEvent(changeEvent);
            }

            return result;
        }

        public static bool RearrangeRequestCommandPositions(int pRequestCommandId, User pCurrentUser)
        {
            bool result = false;
            string SQL = "";
            SQL += @"UPDATE PMIS_RES.RequestCommandPositions t
                     SET Seq = (
                          SELECT Rank
                          FROM (
                                 SELECT RequestCommandPositionID, requestscommandid, Seq,
                                        RANK() OVER(PARTITION BY requestscommandid ORDER BY Seq, RequestCommandPositionID ASC NULLS LAST) Rank
                                 FROM PMIS_RES.RequestCommandPositions
                          )  tmp
                          WHERE tmp.RequestCommandPositionID = t.RequestCommandPositionID
                                
                    )
                    WHERE t.RequestsCommandID = :RequestsCommandID
                    ";

            OracleConnection conn = new OracleConnection(pCurrentUser.ConnectionString);
            conn.Open();

            try
            {               
                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "RequestsCommandID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = pRequestCommandId;
                cmd.Parameters.Add(param);


                cmd.ExecuteNonQuery();
              
                result = true;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        //Get a list of all RequestCommandPosition records for a particular request command
        public static List<RequestCommandPosition> GetRequestCommandPositions(User currentUser, int requestsCommandId)
        {
            List<RequestCommandPosition> requestCommandPositions = new List<RequestCommandPosition>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.RequestCommandPositionID,
                                      a.RequestsCommandID,
                                      a.Position,
                                      a2.MilitaryRankID, b.ZVA_IMEES as MilRankShortName, b.ZVA_IME as MilRankLongName, e.KAT_IME as MilCategoryName, a2.IsPrimary as IsPrimaryRank,
                                      a.ReservistsCount,
                                      a.PositionType,
                                      a.Seq,
                                      c.IsPrimary,
                                      d.MilReportSpecialityID,
                                      d.Type,
                                      d.MilReportingSpecialityName,
                                      d.MilReportingSpecialityCode,
                                      d.Active as MilReportingSpecialityActive,
                                      d.MilitaryForceSortID
                                     
                               FROM PMIS_RES.RequestCommandPositions a
                               LEFT OUTER JOIN PMIS_RES.CommandPositionMilRanks a2 ON a.RequestCommandPositionID = a2.RequestCommandPositionID
				               LEFT OUTER JOIN VS_OWNER.KLV_ZVA b ON a2.MilitaryRankID = b.ZVA_KOD
                               LEFT OUTER JOIN PMIS_RES.CommandPositionMRSpecialities c ON a.RequestCommandPositionID = c.RequestCommandPositionID
                               LEFT OUTER JOIN PMIS_ADM.MilitaryReportSpecialities d ON c.MilReportSpecialityID = d.MilReportSpecialityID         
                               LEFT OUTER JOIN VS_OWNER.KLV_KAT e ON b.ZVA_KAT_KOD = e.KAT_KOD                      
                               WHERE a.RequestsCommandID = :RequestsCommandID
                               ORDER BY a.Seq ASC, a.RequestCommandPositionID,
                                        NVL(a2.IsPrimary, 0) DESC, b.ZVA_KOD, 
                                        NVL(c.IsPrimary, 0) DESC, d.MilReportingSpecialityCode";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RequestsCommandID", OracleType.Number).Value = requestsCommandId;

                OracleDataReader dr = cmd.ExecuteReader();

                int prevRequestCommandPositionId = 0;
                int prevMilitaryRankId = 0;
                RequestCommandPosition requestCommandPosition = null;

                while (dr.Read())
                {
                    int requestCommandPositionId = DBCommon.GetInt(dr["RequestCommandPositionID"]);

                    int? militaryRankId = null;

                    if (DBCommon.IsInt(dr["MilitaryRankID"]))
                        militaryRankId = DBCommon.GetInt(dr["MilitaryRankID"]);


                    if (prevRequestCommandPositionId != requestCommandPositionId)
                    {
                        requestCommandPosition = ExtractRequestCommandPositionFromDataReader(dr, currentUser);
                        requestCommandPosition.MilitaryRanks = new List<CommandPositionMilitaryRank>();
                        requestCommandPosition.MilitaryReportSpecialities = new List<CommandPositionMilitaryReportSpeciality>();

                        requestCommandPositions.Add(requestCommandPosition);

                        prevRequestCommandPositionId = requestCommandPositionId;
                    }

                    if (militaryRankId.HasValue)
                    {
                        if (prevMilitaryRankId != militaryRankId)
                        {
                            MilitaryRank milRank = MilitaryRankUtil.ExtractMilitaryRankFromDR(currentUser, dr);
                            CommandPositionMilitaryRank positionRank = new CommandPositionMilitaryRank();
                            positionRank.Rank = milRank;
                            positionRank.IsPrimary = DBCommon.IsInt(dr["IsPrimaryRank"]) && DBCommon.GetInt(dr["IsPrimaryRank"]) == 1;
                            requestCommandPosition.MilitaryRanks.Add(positionRank);
                            prevMilitaryRankId = militaryRankId.Value;
                        }
                    }
                    else
                    {
                        prevMilitaryRankId = 0;
                    }

                    if (DBCommon.IsInt(dr["RequestCommandPositionID"]) && DBCommon.IsInt(dr["MilReportSpecialityID"]) &&
                        !requestCommandPosition.MilitaryReportSpecialities.Any(x => x.Speciality.MilReportSpecialityId == DBCommon.GetInt(dr["MilReportSpecialityID"]))
                       )
                    {
                        MilitaryReportSpeciality militaryReportSpeciality = MilitaryReportSpecialityUtil.ExtractMilitaryReportSpecialityFromDR(currentUser, dr);
                        CommandPositionMilitaryReportSpeciality positionSpeciality = new CommandPositionMilitaryReportSpeciality();
                        positionSpeciality.Speciality = militaryReportSpeciality;
                        positionSpeciality.IsPrimary = DBCommon.IsInt(dr["IsPrimary"]) && DBCommon.GetInt(dr["IsPrimary"]) == 1;
                        requestCommandPosition.MilitaryReportSpecialities.Add(positionSpeciality);
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return requestCommandPositions;
        }

        //Get a list of all RequestCommandPosition records for a particular request command, with prefilled position military departments list
        public static List<RequestCommandPosition> GetRequestCommandPositionsWithMilDepts(User currentUser, int requestsCommandId)
        {
            List<RequestCommandPosition> requestCommandPositions = new List<RequestCommandPosition>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.RequestCommandPositionID,
                                      a.RequestsCommandID,
                                      a.Position,
                                      PMIS_RES.RESFunctions.GetRanksPerReqCmdPositionHTML(a.RequestCommandPositionID, 20) as Ranks_HTML,
                                      a.ReservistsCount,
                                      a.PositionType,
                                      a.Seq,
                                      PMIS_RES.RESFunctions.GetMRSPerReqCmdPositionHTML(a.RequestCommandPositionID, 20) as MRS_HTML,

                                      c.ReqCommandPositionMilDeptID,
                                      c.MilitaryDepartmentID,
                                      c.ReservistsCount as MilDeptReservistCount,
                                      d.MilitaryDepartmentName
                               FROM PMIS_RES.RequestCommandPositions a
                               LEFT OUTER JOIN PMIS_RES.RequestCommandPositionsMilDept c ON a.RequestCommandPositionID = c.RequestCommandPositionID
                               LEFT OUTER JOIN PMIS_ADM.MilitaryDepartments d ON c.MilitaryDepartmentID = d.MilitaryDepartmentID
                               WHERE a.RequestsCommandID = :RequestsCommandID
                               ORDER BY a.Seq ASC, a.RequestCommandPositionID, c.ReqCommandPositionMilDeptID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RequestsCommandID", OracleType.Number).Value = requestsCommandId;

                OracleDataReader dr = cmd.ExecuteReader();

                int prevRequestCommandPositionId = 0;
                RequestCommandPosition requestCommandPosition = null;

                while (dr.Read())
                {
                    int requestCommandPositionId = DBCommon.GetInt(dr["RequestCommandPositionID"]);

                    if (prevRequestCommandPositionId != requestCommandPositionId)
                    {
                        requestCommandPosition = ExtractRequestCommandPositionFromDataReader(dr, currentUser);
                        requestCommandPosition.MilRepSpecHTML = dr["MRS_HTML"].ToString();
                        requestCommandPosition.MilRankHTML = dr["Ranks_HTML"].ToString();
                        requestCommandPosition.PositionMilitaryDepartments = new List<RequestCommandPositionMilDept>();

                        requestCommandPositions.Add(requestCommandPosition);

                        prevRequestCommandPositionId = requestCommandPositionId;
                    }

                    if (DBCommon.IsInt(dr["RequestCommandPositionID"]) && DBCommon.IsInt(dr["ReqCommandPositionMilDeptID"]))
                    {
                        RequestCommandPositionMilDept requestCommandPositionMilDept = RequestCommandPositionMilDeptUtil.ExtractRequestCommandPositionMilDeptFromDataReader(dr, currentUser);

                        requestCommandPosition.PositionMilitaryDepartments.Add(requestCommandPositionMilDept);
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return requestCommandPositions;
        }

        //Get a list of all RequestCommandPosition records for a particular request command, with prefilled position military departments list
        public static List<RequestCommandPositionBlockForFulfilment> GetRequestCommandPositionsForFulfilment(User currentUser, int requestsCommandId, int militaryDepartmentId)
        {
            List<RequestCommandPositionBlockForFulfilment> requestCommandPositions = new List<RequestCommandPositionBlockForFulfilment>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.RequestCommandPositionID,
                                      a.RequestsCommandID,
                                      a.Position,
                                      PMIS_RES.RESFunctions.GetRanksPerReqCmdPositionHTML(a.RequestCommandPositionID, 20) as Ranks_HTML,
                                      a.ReservistsCount,
                                      a.PositionType,
                                      a.Seq,
                                      PMIS_RES.RESFunctions.GetMRSPerReqCmdPositionHTML(a.RequestCommandPositionID, 20) as MRS_HTML,

                                      c.ReqCommandPositionMilDeptID,
                                      c.MilitaryDepartmentID,
                                      c.ReservistsCount as MilDeptReservistCount,
                                      NVL(d.Fulfiled, 0) as Fulfiled,
                                      NVL(e.FulfiledReserve, 0) as FulfiledReserve
                               FROM PMIS_RES.RequestCommandPositions a
                               INNER JOIN PMIS_RES.RequestCommandPositionsMilDept c ON a.RequestCommandPositionID = c.RequestCommandPositionID   
                               LEFT OUTER JOIN 
                                                (
                                                    SELECT z.RequestCommandPositionID, z.MilitaryDepartmentID, COUNT(*) as Fulfiled
                                                    FROM PMIS_RES.FillReservistsRequest z 
                                                    WHERE z.ReservistReadinessID = 1
                                                    GROUP BY z.RequestCommandPositionID, z.MilitaryDepartmentID
                                                ) d ON a.RequestCommandPositionID = d.RequestCommandPositionID AND c.MilitaryDepartmentID = d.MilitaryDepartmentID
                               LEFT OUTER JOIN
                                                (
                                                    SELECT z.RequestCommandPositionID, z.MilitaryDepartmentID, COUNT(*) as FulfiledReserve
                                                    FROM PMIS_RES.FillReservistsRequest z
                                                    WHERE z.ReservistReadinessID = 2
                                                    GROUP BY z.RequestCommandPositionID, z.MilitaryDepartmentID
                                                ) e ON a.RequestCommandPositionID = e.RequestCommandPositionID AND c.MilitaryDepartmentID = e.MilitaryDepartmentID
                               WHERE a.RequestsCommandID = :RequestsCommandID AND c.MilitaryDepartmentID = :MilitaryDepartmentID AND NVL(c.ReservistsCount, 0) > 0
                               ORDER BY a.Seq, a.RequestCommandPositionID, c.ReqCommandPositionMilDeptID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RequestsCommandID", OracleType.Number).Value = requestsCommandId;
                cmd.Parameters.Add("MilitaryDepartmentID", OracleType.Number).Value = militaryDepartmentId;

                OracleDataReader dr = cmd.ExecuteReader();                                

                while (dr.Read())
                {
                    RequestCommandPosition requestCommandPosition = ExtractRequestCommandPositionFromDataReader(dr, currentUser);
                    
                    RequestCommandPositionBlockForFulfilment block = new RequestCommandPositionBlockForFulfilment(currentUser);
                    block.RequestCommandPositionId = requestCommandPosition.RequestCommandPositionId;
                    block.RequestsCommandId = requestCommandPosition.RequestsCommandId;
                    block.Position = requestCommandPosition.Position;
                    block.MilRankHTML = dr["Ranks_HTML"].ToString();
                    block.PositionType = requestCommandPosition.PositionType;

                    block.ReservistsCount = DBCommon.GetInt(dr["MilDeptReservistCount"]);
                    block.MilRepSpecHTML = dr["MRS_HTML"].ToString();
                    block.Fulfiled = DBCommon.GetInt(dr["Fulfiled"]);
                    block.FulfiledReserve = DBCommon.GetInt(dr["FulfiledReserve"]);

                    requestCommandPositions.Add(block);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return requestCommandPositions;
        }

        //Save a position for a particular request command
        public static bool SaveRequestCommandPosition(RequestCommandPosition requestCommandPosition, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent = null;

            string logDescription = "";
            logDescription += "Заявка №: " + requestCommandPosition.RequestsCommand.EquipmentReservistsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(requestCommandPosition.RequestsCommand.EquipmentReservistsRequest.RequestDate) +
                              "; Команда: " + requestCommandPosition.RequestsCommand.MilitaryCommand.DisplayTextForSelection +
                              "; Длъжност: " + requestCommandPosition.Position;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                //New record
                if (requestCommandPosition.RequestCommandPositionId == 0)
                {
                    SQL += @"INSERT INTO PMIS_RES.RequestCommandPositions (RequestsCommandID, Position,
                                ReservistsCount, PositionType)
                            VALUES (:RequestsCommandID, :Position,
                                :ReservistsCount, :PositionType);

                            SELECT PMIS_RES.RequestCommandPositions_ID_SEQ.currval INTO :RequestCommandPositionID FROM dual;

                            INSERT INTO PMIS_RES.RequestCommandPositionsMilDept (RequestCommandPositionID, MilitaryDepartmentID, ReservistsCount)
                            SELECT :RequestCommandPositionID, a.MilitaryDepartmentID, NULL
                            FROM PMIS_RES.RequestCommandPositionsMilDept a
                            WHERE a.RequestCommandPositionID IN (SELECT RequestCommandPositionID 
                                                                 FROM PMIS_RES.RequestCommandPositions
                                                                 WHERE RequestsCommandID = :RequestsCommandID)
                            GROUP BY a.MilitaryDepartmentID;

                            ";

                    changeEvent = new ChangeEvent("RES_EquipResRequests_Command_AddPosition", logDescription, requestCommandPosition.RequestsCommand.EquipmentReservistsRequest.MilitaryUnit, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_Command_Position_Position", "", requestCommandPosition.Position, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_Command_Position_MilRepSpecialities", "", requestCommandPosition.MilitaryReportSpecialitiesString, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_Command_Position_MilRank", "", requestCommandPosition.MilitaryRanksString, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_Command_Position_ReservistsCount", "", requestCommandPosition.ReservistsCount.ToString(), currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_RES.RequestCommandPositions SET
                               RequestsCommandID = :RequestsCommandID,
                               Position = :Position,
                               ReservistsCount = :ReservistsCount
                             WHERE RequestCommandPositionID = :RequestCommandPositionID;

                            ";

                    changeEvent = new ChangeEvent("RES_EquipResRequests_Command_EditPosition", logDescription, requestCommandPosition.RequestsCommand.EquipmentReservistsRequest.MilitaryUnit, null, currentUser);

                    RequestCommandPosition oldRequestCommandPosition = RequestCommandPositionUtil.GetRequestCommandPosition(currentUser, requestCommandPosition.RequestCommandPositionId);

                    if (oldRequestCommandPosition.Position.Trim() != requestCommandPosition.Position.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_Command_Position_Position", oldRequestCommandPosition.Position, requestCommandPosition.Position, currentUser));

                    if (oldRequestCommandPosition.MilitaryReportSpecialitiesString.Trim() != requestCommandPosition.MilitaryReportSpecialitiesString.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_Command_Position_MilRepSpecialities", oldRequestCommandPosition.MilitaryReportSpecialitiesString, requestCommandPosition.MilitaryReportSpecialitiesString, currentUser));

                    if (oldRequestCommandPosition.MilitaryRanksString.Trim() != requestCommandPosition.MilitaryRanksString.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_Command_Position_MilRank", oldRequestCommandPosition.MilitaryRanksString, requestCommandPosition.MilitaryRanksString, currentUser));

                    if (oldRequestCommandPosition.ReservistsCount != requestCommandPosition.ReservistsCount)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_Command_Position_ReservistsCount", oldRequestCommandPosition.ReservistsCount.ToString(), requestCommandPosition.ReservistsCount.ToString(), currentUser));
                    
                }

                SQL += @"DELETE FROM PMIS_RES.CommandPositionMRSpecialities 
                         WHERE RequestCommandPositionID = :RequestCommandPositionID;

                        ";

                foreach (CommandPositionMilitaryReportSpeciality speciality in requestCommandPosition.MilitaryReportSpecialities)
                {
                    SQL += @"INSERT INTO PMIS_RES.CommandPositionMRSpecialities (RequestCommandPositionID, MilReportSpecialityID, IsPrimary)
                             VALUES (:RequestCommandPositionID, " + speciality.Speciality.MilReportSpecialityId.ToString() + @", " + (speciality.IsPrimary ? "1" : "0") + @");
                            ";

                }

                SQL += @"DELETE FROM PMIS_RES.CommandPositionMilRanks 
                         WHERE RequestCommandPositionID = :RequestCommandPositionID;

                        ";

                foreach (CommandPositionMilitaryRank rank in requestCommandPosition.MilitaryRanks)
                {
                    SQL += @"INSERT INTO PMIS_RES.CommandPositionMilRanks (RequestCommandPositionID, MilitaryRankID, IsPrimary)
                             VALUES (:RequestCommandPositionID, " + rank.Rank.MilitaryRankId.ToString() + @", " + (rank.IsPrimary ? "1" : "0") + @");
                            ";

                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramRequestCommandPositionID = new OracleParameter();
                paramRequestCommandPositionID.ParameterName = "RequestCommandPositionID";
                paramRequestCommandPositionID.OracleType = OracleType.Number;

                if (requestCommandPosition.RequestCommandPositionId != 0)
                {
                    paramRequestCommandPositionID.Direction = ParameterDirection.Input;
                    paramRequestCommandPositionID.Value = requestCommandPosition.RequestCommandPositionId;
                }
                else
                {
                    paramRequestCommandPositionID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramRequestCommandPositionID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "RequestsCommandID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = requestCommandPosition.RequestsCommandId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Position";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(requestCommandPosition.Position))
                    param.Value = requestCommandPosition.Position;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "ReservistsCount";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = requestCommandPosition.ReservistsCount;
                cmd.Parameters.Add(param);

                if (requestCommandPosition.RequestCommandPositionId == 0)
                {
                    param = new OracleParameter();
                    param.ParameterName = "PositionType";
                    param.OracleType = OracleType.Number;
                    param.Direction = ParameterDirection.Input;
                    param.Value = requestCommandPosition.PositionType;
                    cmd.Parameters.Add(param);
                }

                cmd.ExecuteNonQuery();

                if (requestCommandPosition.RequestCommandPositionId == 0)
                    requestCommandPosition.RequestCommandPositionId = DBCommon.GetInt(paramRequestCommandPositionID.Value);

                if (changeEvent != null && changeEvent.ChangeEventDetails.Count > 0)
                    EquipmentReservistsRequestUtil.SetEquipmentReservistsRequestModified(requestCommandPosition.RequestsCommand.EquipmentReservistsRequestId, currentUser);

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

        //Delete a particualr position from a particular command
        public static void DeleteRequestCommandPosition(User currentUser, int requestCommandPositionId, Change changeEntry)
        {
            ChangeEvent changeEvent = null;

            RequestCommandPosition requestCommandPosition = GetRequestCommandPosition(currentUser, requestCommandPositionId);

            string logDescription = "";
            logDescription += "Заявка №: " + requestCommandPosition.RequestsCommand.EquipmentReservistsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(requestCommandPosition.RequestsCommand.EquipmentReservistsRequest.RequestDate) +
                              "; Команда: " + requestCommandPosition.RequestsCommand.MilitaryCommand.DisplayTextForSelection +
                              "; Длъжност: " + requestCommandPosition.Position;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string SQL = "";

            try
            {

                SQL += @"BEGIN
                            DELETE FROM PMIS_RES.RequestCommandPositionsMilDept
                            WHERE RequestCommandPositionID = :RequestCommandPositionID;

                            DELETE FROM PMIS_RES.CommandPositionMRSpecialities
                            WHERE RequestCommandPositionID = :RequestCommandPositionID;

                            DELETE FROM PMIS_RES.CommandPositionMilRanks
                            WHERE RequestCommandPositionID = :RequestCommandPositionID;

                            DELETE FROM PMIS_RES.RequestCommandPositions 
                            WHERE RequestCommandPositionID = :RequestCommandPositionID;
                         END;";

                changeEvent = new ChangeEvent("RES_EquipResRequests_Command_DeletePosition", logDescription, requestCommandPosition.RequestsCommand.EquipmentReservistsRequest.MilitaryUnit, null, currentUser);

                changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_Command_Position_Position", requestCommandPosition.Position, "", currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_Command_Position_MilRepSpecialities", requestCommandPosition.MilitaryReportSpecialitiesString, "", currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_Command_Position_MilRank", requestCommandPosition.MilitaryRanksString, "", currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_Command_Position_ReservistsCount", requestCommandPosition.ReservistsCount.ToString(), "", currentUser));

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RequestCommandPositionID", OracleType.Number).Value = requestCommandPositionId;

                cmd.ExecuteNonQuery();

                EquipmentReservistsRequestUtil.SetEquipmentReservistsRequestModified(requestCommandPosition.RequestsCommand.EquipmentReservistsRequestId, currentUser);
            }
            finally
            {
                conn.Close();
            }

            if (changeEvent != null && changeEvent.ChangeEventDetails.Count > 0)
                changeEntry.AddEvent(changeEvent);
        }

        //Get a list of all Military Reporting Specialities assigned to a particular position
        public static List<CommandPositionMilitaryReportSpeciality> GetMilitaryReportingSpecialitiesPerPosition(User currentUser, int requestCommandPositionId)
        {
            List<CommandPositionMilitaryReportSpeciality> specialities = new List<CommandPositionMilitaryReportSpeciality>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT b.MilReportSpecialityID,
                                      b.Type,
                                      b.MilReportingSpecialityName,
                                      b.MilReportingSpecialityCode,
                                      b.Active as MilReportingSpecialityActive,
                                      b.MilitaryForceSortID,
                                      a.IsPrimary
                               FROM PMIS_RES.CommandPositionMRSpecialities a
                               INNER JOIN PMIS_ADM.MilitaryReportSpecialities b ON a.MilReportSpecialityID = b.MilReportSpecialityID
                               WHERE a.RequestCommandPositionID = :RequestCommandPositionID
                               ORDER BY NVL(a.IsPrimary, 0) DESC, b.MilReportingSpecialityCode";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RequestCommandPositionID", OracleType.Number).Value = requestCommandPositionId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    CommandPositionMilitaryReportSpeciality speciality = new CommandPositionMilitaryReportSpeciality();
                    speciality.Speciality = MilitaryReportSpecialityUtil.ExtractMilitaryReportSpecialityFromDR(currentUser, dr);
                    speciality.IsPrimary = DBCommon.IsInt(dr["IsPrimary"]) && DBCommon.GetInt(dr["IsPrimary"]) == 1;

                    specialities.Add(speciality);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return specialities;
        }

        //Get a list of all Military Ranks assigned to a particular position
        public static List<CommandPositionMilitaryRank> GetMilitaryRanksPerPosition(User currentUser, int requestCommandPositionId)
        {
            List<CommandPositionMilitaryRank> ranks = new List<CommandPositionMilitaryRank>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.MilitaryRankID, b.ZVA_IMEES as MilRankShortName, b.ZVA_IME as MilRankLongName, c.KAT_IME as MilCategoryName,
                                      a.IsPrimary
                               FROM PMIS_RES.CommandPositionMilRanks a
                               INNER JOIN VS_OWNER.KLV_ZVA b ON a.MilitaryRankID = b.ZVA_KOD
                               LEFT OUTER JOIN VS_OWNER.KLV_KAT c ON b.ZVA_KAT_KOD = c.KAT_KOD
                               WHERE a.RequestCommandPositionID = :RequestCommandPositionID
                               ORDER BY NVL(a.IsPrimary, 0) DESC, b.ZVA_KOD";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RequestCommandPositionID", OracleType.Number).Value = requestCommandPositionId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    CommandPositionMilitaryRank rank = new CommandPositionMilitaryRank();
                    rank.Rank = MilitaryRankUtil.ExtractMilitaryRankFromDR(currentUser, dr);
                    rank.IsPrimary = DBCommon.IsInt(dr["IsPrimary"]) && DBCommon.GetInt(dr["IsPrimary"]) == 1;

                    ranks.Add(rank);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return ranks;
        }

        //Import a particular existing vacant position
        public static void ImportVacantPosition(User currentUser, int requestCommandId, int maxVSST_ID, int positionsCount, Change changeEntry)
        {
            ChangeEvent changeEvent = null;

            RequestCommand requestCommand = RequestCommandUtil.GetRequestsCommand(currentUser, requestCommandId);

            string logDescription = "";
            logDescription += "Заявка №: " + requestCommand.EquipmentReservistsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(requestCommand.EquipmentReservistsRequest.RequestDate) +
                              "; Команда: " + requestCommand.MilitaryCommand.DisplayTextForSelection;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string SQL = "";

            try
            {

                SQL += @"BEGIN
                            INSERT INTO PMIS_RES.RequestCommandPositions (RequestsCommandID, Position, ReservistsCount, PositionType)
                            SELECT :RequestsCommandID as RequestsCommandID, 
                                   a.VSST_TEXT_DL as Position,
                                   :PositionsCount,
                                   :PositionType
                            FROM VS_OWNER.VS_VSST a
                            WHERE a.VSST_ID = :MAX_VSST_ID;
                            
                            SELECT PMIS_RES.RequestCommandPositions_ID_SEQ.currval INTO :RequestCommandPositionID FROM dual;

                            INSERT INTO PMIS_RES.CommandPositionMRSpecialities (RequestCommandPositionID, 
                               MilReportSpecialityID)
                            SELECT :RequestCommandPositionID, b.MilReportSpecialityID
                            FROM VS_OWNER.VS_VSST_VOS a
                            INNER JOIN PMIS_ADM.MilitaryReportSpecialities b ON CASE WHEN a.VSVOS_VSOKOD IS NOT NULL
                                                                                     THEN a.VSVOS_VSOKOD
                                                                                     ELSE a.VSVOS_VSSKOD
                                                                                END = b.MilReportingSpecialityCode
                            WHERE a.VSVOS_VSST_ID = :MAX_VSST_ID;

                            INSERT INTO PMIS_RES.CommandPositionMilRanks (RequestCommandPositionID, MilitaryRankID, IsPrimary)
                            SELECT :RequestCommandPositionID, b.ZAP_ZVA_KOD as MilitaryRankID, MAX(a.IsPrimary) as IsPrimary
                            FROM (SELECT VSST_ID, VSST_ZAPKOD, 1 as IsPrimary
	                              FROM VS_OWNER.VS_VSST
					              UNION
					              SELECT VSZAP_VSST_ID as VSST_ID, VSZAP_ZAPKOD as VSST_ZAPKOD, 0 as IsPrimary
					              FROM VS_OWNER.VS_VSST_ZAP) a
                            INNER JOIN VS_OWNER.KLV_ZAP b ON a.VSST_ZAPKOD = b.ZAP_KOD
                            WHERE a.VSST_ID = :MAX_VSST_ID
                            /*2019-11-26: We are not sure if it is possible a particular VSST_ZAPKOD to persist in both VS_OWNER.VS_VSST (the primary record) and VS_OWNER.VS_VSST_ZAP (the additional ones)
                              We tested this situation by putting the same VSST_ZAPKOD in both tables and because of we treat the main one is IsPrimary=1, then it was trying to import the same Military Rank twice which violates the
                              the PK contraint in our table PMIS_RES.CommandPositionMilRanks. That is why are adding this GROUP BY clause and we pull the IsPrimary flag using the MAX() function.*/
                            GROUP BY b.ZAP_ZVA_KOD;

                            INSERT INTO PMIS_RES.RequestCommandPositionsMilDept (RequestCommandPositionID, MilitaryDepartmentID, ReservistsCount)
                            SELECT :RequestCommandPositionID, a.MilitaryDepartmentID, NULL
                            FROM PMIS_RES.RequestCommandPositionsMilDept a
                            WHERE a.RequestCommandPositionID IN (SELECT RequestCommandPositionID 
                                                                 FROM PMIS_RES.RequestCommandPositions
                                                                 WHERE RequestsCommandID = :RequestsCommandID)
                            GROUP BY a.MilitaryDepartmentID;
                         END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RequestsCommandID", OracleType.Number).Value = requestCommandId;
                cmd.Parameters.Add("MAX_VSST_ID", OracleType.Number).Value = maxVSST_ID;
                cmd.Parameters.Add("PositionsCount", OracleType.Number).Value = positionsCount;
                cmd.Parameters.Add("PositionType", OracleType.Number).Value = 2;

                cmd.Parameters.Add("RequestCommandPositionID", OracleType.Number).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                EquipmentReservistsRequestUtil.SetEquipmentReservistsRequestModified(requestCommand.EquipmentReservistsRequestId, currentUser);

                int requestCommandPositionId = DBCommon.GetInt(cmd.Parameters["RequestCommandPositionID"].Value);

                RequestCommandPosition requestCommandPosition = GetRequestCommandPosition(currentUser, requestCommandPositionId);

                changeEvent = new ChangeEvent("RES_EquipResRequests_Command_ImportPosition", logDescription, requestCommandPosition.RequestsCommand.EquipmentReservistsRequest.MilitaryUnit, null, currentUser);

                changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_Command_Position_Position", "", requestCommandPosition.Position, currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_Command_Position_MilRepSpecialities", "", requestCommandPosition.MilitaryReportSpecialitiesString, currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_Command_Position_MilRank", "", requestCommandPosition.MilitaryRanksString, currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_Command_Position_ReservistsCount", "", requestCommandPosition.ReservistsCount.ToString(), currentUser));
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