using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    //This class represents a particular technics command position
    public class TechnicsRequestCommandPosition : BaseDbObject
    {
        private int technicsRequestCommandPositionId;
        private int technicsRequestsCommandId;
        private TechnicsRequestCommand technicsRequestsCommand;
        private TechnicsType technicsType;
        private NormativeTechnics normativeTechnics;
        private int count;
        private int driversCount;
        private string comment;
        private List<TechnicsReqCmdPositionMilDept> technicsPositionMilitaryDepartments;
        private int fulfilCount;
        private int seq;

        public int TechnicsRequestCommandPositionId
        {
            get
            {
                return technicsRequestCommandPositionId;
            }
            set
            {
                technicsRequestCommandPositionId = value;
            }
        }

        public int TechnicsRequestsCommandId
        {
            get
            {
                return technicsRequestsCommandId;
            }
            set
            {
                technicsRequestsCommandId = value;
            }
        }

        public TechnicsRequestCommand TechnicsRequestsCommand
        {
            get
            {
                //Lazy initialization
                if (technicsRequestsCommand == null)
                    technicsRequestsCommand = TechnicsRequestCommandUtil.GetTechnicsRequestCommand(CurrentUser, TechnicsRequestsCommandId);

                return technicsRequestsCommand;
            }
            set
            {
                technicsRequestsCommand = value;
            }
        }

        public TechnicsType TechnicsType
        {
            get
            {
                return technicsType;
            }
            set
            {
                technicsType = value;
            }
        }

        public NormativeTechnics NormativeTechnics
        {
            get
            {
                return normativeTechnics;
            }
            set
            {
                normativeTechnics = value;
            }
        }

        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                count = value;
            }
        }

        public int DriversCount
        {
            get
            {
                return driversCount;
            }
            set
            {
                driversCount = value;
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

        public int FulfilCount
        {
            get
            {
                return fulfilCount;
            }

            set
            {
                fulfilCount = value;
            }
        }

        public List<TechnicsReqCmdPositionMilDept> TechnicsPositionMilitaryDepartments
        {
            get 
            {
                //Lazy initialization. Use it only when the list of positions military departments isn't already loaded
                //When loading the entire list of position we pull them too

                if (technicsPositionMilitaryDepartments == null)
                    technicsPositionMilitaryDepartments = TechnicsReqCmdPositionMilDeptUtil.GetAllTechnicsReqCmdPositionMilDeptByCommandPosition(TechnicsRequestCommandPositionId, CurrentUser);

                return technicsPositionMilitaryDepartments; 
            }
            set { technicsPositionMilitaryDepartments = value; }
        }

        public bool CanDelete
        {
            get { return true; }

        }

        public TechnicsRequestCommandPosition(User user)
            : base(user)
        {

        }

        public int Seq
        {
            get { return seq; }
            set { seq = value; }
        }
    }

    public class TechnicsRequestCommandPositionBlockForFulfilment : BaseDbObject
    {
        private int technicsRequestCommandPositionId;
        private int technicsRequestsCommandId;
        private TechnicsRequestCommand technicsRequestsCommand;
        private string technicsTypeKey;        
        private string technicsType;
        private NormativeTechnics normativeTechnics;
        private string technicsComment;
        private int count;
        private int fulfiled;        
        private int fulfiledReserve;

        public int TechnicsRequestCommandPositionId
        {
            get
            {
                return technicsRequestCommandPositionId;
            }
            set
            {
                technicsRequestCommandPositionId = value;
            }
        }

        public int TechnicsRequestsCommandId
        {
            get
            {
                return technicsRequestsCommandId;
            }
            set
            {
                technicsRequestsCommandId = value;
            }
        }

        public TechnicsRequestCommand TechnicsRequestCommand
        {
            get
            {
                //Lazy initialization
                if (technicsRequestsCommand == null)
                    technicsRequestsCommand = TechnicsRequestCommandUtil.GetTechnicsRequestCommand(CurrentUser, TechnicsRequestCommandPositionId);

                return technicsRequestsCommand;
            }
            set
            {
                technicsRequestsCommand = value;
            }
        }

        public string TechnicsTypeKey
        {
            get { return technicsTypeKey; }
            set { technicsTypeKey = value; }
        }

        public string TechnicsType
        {
            get
            {
                return technicsType;
            }
            set
            {
                technicsType = value;
            }
        }

        public NormativeTechnics NormativeTechnics
        {
            get { return normativeTechnics; }
            set { normativeTechnics = value; }
        }

        public string TechnicsComment
        {
            get
            {
                return technicsComment;
            }
            set
            {
                technicsComment = value;
            }
        }

        public int Count
        {
            get
            {
                return count;
            }
            set
            {
                count = value;
            }
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

        public TechnicsRequestCommandPositionBlockForFulfilment(User user)
            : base(user)
        {

        }
    }   

    //Some methods for working with TechnicsRequestCommandPosition objects
    public static class TechnicsRequestCommandPositionUtil
    {
        //This method creates and returns a TechnicsRequestCommandPosition object. It extracts the data from a DataReader.
        //This is done in this way to have a signle place of creation of this object, however, pass the particular DataReader
        //object from various queries for better performance instead of executing a new call to the DB here to get the details for a specific ID, for example.
        public static TechnicsRequestCommandPosition ExtractTechnicsRequestCommandPositionFromDataReader(OracleDataReader dr, User currentUser)
        {
            TechnicsRequestCommandPosition technicsRequestCommandPosition = new TechnicsRequestCommandPosition(currentUser);

            technicsRequestCommandPosition.TechnicsRequestCommandPositionId = DBCommon.GetInt(dr["TechnicsRequestCmdPositionID"]);
            technicsRequestCommandPosition.TechnicsRequestsCommandId = DBCommon.GetInt(dr["TechRequestsCommandID"]);
            technicsRequestCommandPosition.TechnicsType = TechnicsTypeUtil.ExtractTechnicsTypeFromDataReader(dr, currentUser);
            technicsRequestCommandPosition.NormativeTechnics = NormativeTechnicsUtil.ExtractNormativeTechnicsFromDataReader(dr, currentUser);
            technicsRequestCommandPosition.Seq = DBCommon.GetInt(dr["Seq"]);

            if (DBCommon.IsInt(dr["Count"]))
                technicsRequestCommandPosition.Count = DBCommon.GetInt(dr["Count"]);

            if (DBCommon.IsInt(dr["DriversCount"]))
                technicsRequestCommandPosition.DriversCount = DBCommon.GetInt(dr["DriversCount"]);

            technicsRequestCommandPosition.Comment = dr["TComment"].ToString();
            technicsRequestCommandPosition.FulfilCount = DBCommon.GetInt(dr["FulfilCount"]);

            return technicsRequestCommandPosition;
        }

        //Get a specific TechnicsRequestCommandPosition record
        public static TechnicsRequestCommandPosition GetTechnicsRequestCommandPosition(User currentUser, int technicsRequestCommandPositionId)
        {
            TechnicsRequestCommandPosition technicsRequestCommandPosition = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TechnicsRequestCmdPositionID,
                                      a.TechRequestsCommandID,
                                      a.TechnicsTypeID, b.TechnicsTypeKey, b.TechnicsTypeName, b.Active as TechnicsTypeActive,
                                      n.NormativeTechnicsID, n.NormativeCode, n.NormativeName, n.TechnicsSubTypeID,
                                      a.Count,
                                      a.DriversCount,
                                      a.TComment,
                                      a.Seq,
                                      NVL(fulfil.FulfilCount, 0) as FulfilCount
                               FROM PMIS_RES.TechnicsRequestCmdPositions a
                               INNER JOIN PMIS_RES.TechnicsTypes b ON a.TechnicsTypeID = b.TechnicsTypeID
                               LEFT OUTER JOIN PMIS_RES.NormativeTechnics n ON a.NormativeTechnicsID = n.NormativeTechnicsID
                               LEFT OUTER JOIN (SELECT COUNT(*) as FulfilCount,
                                                       TechnicsRequestCmdPositionID
                                                FROM PMIS_RES.FulfilTechnicsRequest
                                                GROUP BY TechnicsRequestCmdPositionID) fulfil ON a.TechnicsRequestCmdPositionID = fulfil.TechnicsRequestCmdPositionID
                               WHERE a.TechnicsRequestCmdPositionID = :TechnicsRequestCmdPositionID
                               ";
                               
                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsRequestCmdPositionID", OracleType.Number).Value = technicsRequestCommandPositionId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    technicsRequestCommandPosition = ExtractTechnicsRequestCommandPositionFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return technicsRequestCommandPosition;
        }

        //Get a list of all TechnicsRequestCommandPosition records for a particular request command
        public static List<TechnicsRequestCommandPosition> GetTechnicsRequestCommandPositions(User currentUser, int technicsRequestsCommandId)
        {
            List<TechnicsRequestCommandPosition> technicsRequestCommandPositions = new List<TechnicsRequestCommandPosition>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TechnicsRequestCmdPositionID,
                                      a.TechRequestsCommandID,
                                      a.TechnicsTypeID, b.TechnicsTypeKey, b.TechnicsTypeName, b.Active as TechnicsTypeActive,
                                      n.NormativeTechnicsID, n.NormativeCode, n.NormativeName, n.TechnicsSubTypeID,
                                      a.Count,
                                      a.DriversCount,
                                      a.TComment,
                                      a.Seq,
                                      NVL(fulfil.FulfilCount, 0) as FulfilCount
                               FROM PMIS_RES.TechnicsRequestCmdPositions a
                               INNER JOIN PMIS_RES.TechnicsTypes b ON a.TechnicsTypeID = b.TechnicsTypeID
                               LEFT OUTER JOIN PMIS_RES.NormativeTechnics n ON a.NormativeTechnicsID = n.NormativeTechnicsID
                               LEFT OUTER JOIN (SELECT COUNT(*) as FulfilCount,
                                                       TechnicsRequestCmdPositionID
                                                FROM PMIS_RES.FulfilTechnicsRequest
                                                GROUP BY TechnicsRequestCmdPositionID) fulfil ON a.TechnicsRequestCmdPositionID = fulfil.TechnicsRequestCmdPositionID
                               WHERE a.TechRequestsCommandID = :TechRequestsCommandID
                               ORDER BY a.Seq ASC, a.TechnicsRequestCmdPositionID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechRequestsCommandID", OracleType.Number).Value = technicsRequestsCommandId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    technicsRequestCommandPositions.Add(ExtractTechnicsRequestCommandPositionFromDataReader(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return technicsRequestCommandPositions;
        }

        //Get a list of all TechnicsRequestCommandPosition records for a particular request command, with prefilled position military departments list
        public static List<TechnicsRequestCommandPosition> GetTechnicsRequestCommandPositionsWithMilDepts(User currentUser, int technicsRequestsCommandId)
        {
            List<TechnicsRequestCommandPosition> technicsRequestCommandPositions = new List<TechnicsRequestCommandPosition>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TechnicsRequestCmdPositionID,
                                      a.TechRequestsCommandID,
                                      a.TechnicsTypeID, b.TechnicsTypeKey, b.TechnicsTypeName, b.Active as TechnicsTypeActive,
                                      n.NormativeTechnicsID, n.NormativeCode, n.NormativeName, n.TechnicsSubTypeID,
                                      a.Count,
                                      a.DriversCount,
                                      a.TComment,
                                      a.Seq,
                                      NVL(fulfil.FulfilCount, 0) as FulfilCount,

                                      c.TechReqCmdPositionsMilDeptID,
                                      c.MilitaryDepartmentID,
                                      c.Count as MilDeptCount,
                                      c.DriversCount as MilDeptDriversCount,
                                      d.MilitaryDepartmentName
                               FROM PMIS_RES.TechnicsRequestCmdPositions a
                               INNER JOIN PMIS_RES.TechnicsTypes b ON a.TechnicsTypeID = b.TechnicsTypeID
                               LEFT OUTER JOIN PMIS_RES.NormativeTechnics n ON a.NormativeTechnicsID = n.NormativeTechnicsID
                               LEFT OUTER JOIN PMIS_RES.TechRequestCmdPositionsMilDept c ON a.TechnicsRequestCmdPositionID = c.TechnicsRequestCmdPositionID
                               LEFT OUTER JOIN PMIS_ADM.MilitaryDepartments d ON c.MilitaryDepartmentID = d.MilitaryDepartmentID
                               LEFT OUTER JOIN (SELECT COUNT(*) as FulfilCount,
                                                       TechnicsRequestCmdPositionID
                                                FROM PMIS_RES.FulfilTechnicsRequest
                                                GROUP BY TechnicsRequestCmdPositionID) fulfil ON a.TechnicsRequestCmdPositionID = fulfil.TechnicsRequestCmdPositionID
                               WHERE a.TechRequestsCommandID = :TechRequestsCommandID
                               ORDER BY a.Seq ASC, a.TechnicsRequestCmdPositionID, c.TechReqCmdPositionsMilDeptID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechRequestsCommandID", OracleType.Number).Value = technicsRequestsCommandId;

                OracleDataReader dr = cmd.ExecuteReader();

                int prevTechnicsRequestCommandPositionId = 0;
                TechnicsRequestCommandPosition technicsRequestCommandPosition = null;

                while (dr.Read())
                {
                    int technicsRequestCommandPositionId = DBCommon.GetInt(dr["TechnicsRequestCmdPositionID"]);

                    if (prevTechnicsRequestCommandPositionId != technicsRequestCommandPositionId)
                    {
                        technicsRequestCommandPosition = ExtractTechnicsRequestCommandPositionFromDataReader(dr, currentUser);
                        technicsRequestCommandPosition.TechnicsPositionMilitaryDepartments = new List<TechnicsReqCmdPositionMilDept>();

                        technicsRequestCommandPositions.Add(technicsRequestCommandPosition);

                        prevTechnicsRequestCommandPositionId = technicsRequestCommandPositionId;
                    }

                    if (DBCommon.IsInt(dr["TechnicsRequestCmdPositionID"]) && DBCommon.IsInt(dr["TechReqCmdPositionsMilDeptID"]))
                    {
                        TechnicsReqCmdPositionMilDept technicsRequestCommandPositionMilDept = TechnicsReqCmdPositionMilDeptUtil.ExtractTechnicsReqCmdPositionMilDeptFromDataReader(dr, currentUser);

                        technicsRequestCommandPosition.TechnicsPositionMilitaryDepartments.Add(technicsRequestCommandPositionMilDept);
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return technicsRequestCommandPositions;
        }

        //Get a list of all TechnicsRequestCommandPositionBlockForFulfilment
        public static List<TechnicsRequestCommandPositionBlockForFulfilment> GetTechnicsRequestCommandPositionsForFulfilment(User currentUser, int techRequestsCommandId, int militaryDepartmentId)
        {
            List<TechnicsRequestCommandPositionBlockForFulfilment> technicsRequestCommandPositions = new List<TechnicsRequestCommandPositionBlockForFulfilment>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TechnicsRequestCmdPositionID,
                                      a.TechRequestsCommandID,
                                      a.TechnicsTypeID,
                                      a.TComment,      
                                      a.Seq,                                
                                      a.Count,
                                      a.DriversCount,
                                      NVL(fulfil.FulfilCount, 0) as FulfilCount,

                                      e.TechnicsTypeKey,
                                      e.TechnicsTypeName,
                                      e.Active as TechnicsTypeActive,

                                      n.NormativeTechnicsID, n.NormativeCode, n.NormativeName, n.TechnicsSubTypeID,

                                      b.TechReqCmdPositionsMilDeptID,
                                      b.MilitaryDepartmentID,
                                      b.Count as MilDeptTechnicsCount,
                                      NVL(c.Fulfiled, 0) as Fulfiled,
                                      NVL(d.FulfiledReserve, 0) as FulfiledReserve
                               FROM PMIS_RES.TechnicsRequestCmdPositions a                               
                               INNER JOIN PMIS_RES.TechRequestCmdPositionsMilDept b ON a.TechnicsRequestCmdPositionID = b.TechnicsRequestCmdPositionID   
                               INNER JOIN PMIS_RES.TechnicsTypes e ON a.TechnicsTypeID = e.TechnicsTypeID
                               LEFT OUTER JOIN PMIS_RES.NormativeTechnics n ON a.NormativeTechnicsID = n.NormativeTechnicsID
                               LEFT OUTER JOIN 
                                                (
                                                    SELECT z.TechnicsRequestCmdPositionID, z.MilitaryDepartmentID, SUM(a.ItemsCount) as Fulfiled
                                                    FROM PMIS_RES.FulfilTechnicsRequest z 
                                                    INNER JOIN PMIS_RES.Technics a ON z.TechnicsID = a.TechnicsID
                                                    WHERE z.TechnicReadinessID = 1
                                                    GROUP BY z.TechnicsRequestCmdPositionID, z.MilitaryDepartmentID
                                                ) c ON a.TechnicsRequestCmdPositionID = c.TechnicsRequestCmdPositionID AND b.MilitaryDepartmentID = c.MilitaryDepartmentID
                               LEFT OUTER JOIN
                                                (
                                                    SELECT z.TechnicsRequestCmdPositionID, z.MilitaryDepartmentID, SUM(a.ItemsCount) as FulfiledReserve
                                                    FROM PMIS_RES.FulfilTechnicsRequest z
                                                    INNER JOIN PMIS_RES.Technics a ON z.TechnicsID = a.TechnicsID
                                                    WHERE z.TechnicReadinessID = 2
                                                    GROUP BY z.TechnicsRequestCmdPositionID, z.MilitaryDepartmentID
                                                ) d ON a.TechnicsRequestCmdPositionID = d.TechnicsRequestCmdPositionID AND b.MilitaryDepartmentID = d.MilitaryDepartmentID
                               LEFT OUTER JOIN (SELECT COUNT(*) as FulfilCount,
                                                       TechnicsRequestCmdPositionID
                                                FROM PMIS_RES.FulfilTechnicsRequest
                                                GROUP BY TechnicsRequestCmdPositionID) fulfil ON a.TechnicsRequestCmdPositionID = fulfil.TechnicsRequestCmdPositionID
                               WHERE a.TechRequestsCommandID = :TechRequestsCommandID AND b.MilitaryDepartmentID = :MilitaryDepartmentID AND NVL(b.Count, 0) > 0
                               ORDER BY a.Seq ASC, a.TechnicsRequestCmdPositionID, b.TechReqCmdPositionsMilDeptID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechRequestsCommandID", OracleType.Number).Value = techRequestsCommandId;
                cmd.Parameters.Add("MilitaryDepartmentID", OracleType.Number).Value = militaryDepartmentId;

                OracleDataReader dr = cmd.ExecuteReader();                                

                while (dr.Read())
                {
                    TechnicsRequestCommandPosition requestCommandPosition = ExtractTechnicsRequestCommandPositionFromDataReader(dr, currentUser);

                    TechnicsRequestCommandPositionBlockForFulfilment block = new TechnicsRequestCommandPositionBlockForFulfilment(currentUser);
                    block.TechnicsRequestCommandPositionId = requestCommandPosition.TechnicsRequestCommandPositionId;
                    block.TechnicsRequestsCommandId = requestCommandPosition.TechnicsRequestsCommandId;
                    block.TechnicsTypeKey = requestCommandPosition.TechnicsType.TypeKey;
                    block.TechnicsType = requestCommandPosition.TechnicsType.TypeName;
                    block.NormativeTechnics = requestCommandPosition.NormativeTechnics;
                    block.TechnicsComment = requestCommandPosition.Comment;
                    block.Count = DBCommon.GetInt(dr["MilDeptTechnicsCount"]);                   
                    block.Fulfiled = DBCommon.GetInt(dr["Fulfiled"]);
                    block.FulfiledReserve = DBCommon.GetInt(dr["FulfiledReserve"]);

                    technicsRequestCommandPositions.Add(block);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return technicsRequestCommandPositions;
        }

        //Save a position for a particular technics request command
        public static bool SaveTechnicsRequestCommandPosition(TechnicsRequestCommandPosition technicsRequestCommandPosition, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent = null;

            string logDescription = "";
            logDescription += "Заявка №: " + technicsRequestCommandPosition.TechnicsRequestsCommand.EquipmentTechnicsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(technicsRequestCommandPosition.TechnicsRequestsCommand.EquipmentTechnicsRequest.RequestDate) +
                              "; Команда: " + technicsRequestCommandPosition.TechnicsRequestsCommand.MilitaryCommand.DisplayTextForSelection +
                              "; Вид техника: " + technicsRequestCommandPosition.TechnicsType.TypeName +
                              "; Нормативна категория (код): " + technicsRequestCommandPosition.NormativeTechnics.NormativeCode +
                              "; Коментар: " + technicsRequestCommandPosition.Comment;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                //New record
                if (technicsRequestCommandPosition.TechnicsRequestCommandPositionId == 0)
                {
                    SQL += @"INSERT INTO PMIS_RES.TechnicsRequestCmdPositions (TechRequestsCommandID, TechnicsTypeID, NormativeTechnicsID,
                                TComment, Count, DriversCount)
                             VALUES (:TechRequestsCommandID, :TechnicsTypeID, :NormativeTechnicsID,
                                :TComment, :Count, :DriversCount);

                            SELECT PMIS_RES.TechRequestCmdPositions_ID_SEQ.currval INTO :TechnicsRequestCmdPositionID FROM dual;

                            INSERT INTO PMIS_RES.TechRequestCmdPositionsMilDept (TechnicsRequestCmdPositionID, MilitaryDepartmentID, Count, DriversCount)
                            SELECT :TechnicsRequestCmdPositionID, a.MilitaryDepartmentID, NULL, NULL
                            FROM PMIS_RES.TechRequestCmdPositionsMilDept a
                            WHERE a.TechnicsRequestCmdPositionID IN (SELECT TechnicsRequestCmdPositionID 
                                                                     FROM PMIS_RES.TechnicsRequestCmdPositions
                                                                     WHERE TechRequestsCommandID = :TechRequestsCommandID)
                            GROUP BY a.MilitaryDepartmentID;

                            ";

                    changeEvent = new ChangeEvent("RES_EquipTechRequests_Command_AddPosition", logDescription, technicsRequestCommandPosition.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_Command_Position_TechnicsType", "", technicsRequestCommandPosition.TechnicsType.TypeName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_Command_Position_NormativeTechnics", "", technicsRequestCommandPosition.NormativeTechnics.CodeAndText, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_Command_Position_Comment", "", technicsRequestCommandPosition.Comment, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_Command_Position_Count", "", technicsRequestCommandPosition.Count.ToString(), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_Command_Position_DriversCount", "", technicsRequestCommandPosition.DriversCount.ToString(), currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_RES.TechnicsRequestCmdPositions SET
                               TechRequestsCommandID = :TechRequestsCommandID,
                               TechnicsTypeID = :TechnicsTypeID,
                               NormativeTechnicsID = :NormativeTechnicsID,
                               TComment = :TComment,
                               Count = :Count,
                               DriversCount = :DriversCount
                             WHERE TechnicsRequestCmdPositionID = :TechnicsRequestCmdPositionID;

                            ";

                    changeEvent = new ChangeEvent("RES_EquipTechRequests_Command_EditPosition", logDescription, technicsRequestCommandPosition.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, currentUser);

                    TechnicsRequestCommandPosition oldTechnicsRequestCommandPosition = TechnicsRequestCommandPositionUtil.GetTechnicsRequestCommandPosition(currentUser, technicsRequestCommandPosition.TechnicsRequestCommandPositionId);

                    if (oldTechnicsRequestCommandPosition.TechnicsType.TechnicsTypeId != technicsRequestCommandPosition.TechnicsType.TechnicsTypeId)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_Command_Position_TechnicsType", oldTechnicsRequestCommandPosition.TechnicsType.TypeName, technicsRequestCommandPosition.TechnicsType.TypeName, currentUser));

                    if (oldTechnicsRequestCommandPosition.NormativeTechnics == null || oldTechnicsRequestCommandPosition.NormativeTechnics.NormativeTechnicsId != technicsRequestCommandPosition.NormativeTechnics.NormativeTechnicsId)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_Command_Position_NormativeTechnics", (oldTechnicsRequestCommandPosition.NormativeTechnics == null ? "" : oldTechnicsRequestCommandPosition.NormativeTechnics.CodeAndText), technicsRequestCommandPosition.NormativeTechnics.CodeAndText, currentUser));

                    if (oldTechnicsRequestCommandPosition.Comment != technicsRequestCommandPosition.Comment)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_Command_Position_Comment", oldTechnicsRequestCommandPosition.Comment, technicsRequestCommandPosition.Comment, currentUser));

                    if (oldTechnicsRequestCommandPosition.Count != technicsRequestCommandPosition.Count)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_Command_Position_Count", oldTechnicsRequestCommandPosition.Count.ToString(), technicsRequestCommandPosition.Count.ToString(), currentUser));

                    if (oldTechnicsRequestCommandPosition.DriversCount != technicsRequestCommandPosition.DriversCount)
                        changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_Command_Position_DriversCount", oldTechnicsRequestCommandPosition.DriversCount.ToString(), technicsRequestCommandPosition.DriversCount.ToString(), currentUser));
                    
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramTechnicsRequestCommandPositionID = new OracleParameter();
                paramTechnicsRequestCommandPositionID.ParameterName = "TechnicsRequestCmdPositionID";
                paramTechnicsRequestCommandPositionID.OracleType = OracleType.Number;

                if (technicsRequestCommandPosition.TechnicsRequestCommandPositionId != 0)
                {
                    paramTechnicsRequestCommandPositionID.Direction = ParameterDirection.Input;
                    paramTechnicsRequestCommandPositionID.Value = technicsRequestCommandPosition.TechnicsRequestCommandPositionId;
                }
                else
                {
                    paramTechnicsRequestCommandPositionID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramTechnicsRequestCommandPositionID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "TechRequestsCommandID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = technicsRequestCommandPosition.TechnicsRequestsCommandId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "TechnicsTypeID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = technicsRequestCommandPosition.TechnicsType.TechnicsTypeId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "NormativeTechnicsID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = technicsRequestCommandPosition.NormativeTechnics.NormativeTechnicsId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "TComment";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!String.IsNullOrEmpty(technicsRequestCommandPosition.Comment))
                    param.Value = technicsRequestCommandPosition.Comment;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Count";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = technicsRequestCommandPosition.Count;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "DriversCount";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = technicsRequestCommandPosition.DriversCount;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (technicsRequestCommandPosition.TechnicsRequestCommandPositionId == 0)
                    technicsRequestCommandPosition.TechnicsRequestCommandPositionId = DBCommon.GetInt(paramTechnicsRequestCommandPositionID.Value);

                if (changeEvent != null && changeEvent.ChangeEventDetails.Count > 0)
                    EquipmentTechnicsRequestUtil.SetEquipmentTechnicsRequestModified(technicsRequestCommandPosition.TechnicsRequestsCommand.EquipmentTechnicsRequestId, currentUser);

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
        public static void DeleteTechnicsRequestCommandPosition(User currentUser, int technicsRequestCommandPositionId, Change changeEntry)
        {
            ChangeEvent changeEvent = null;

            TechnicsRequestCommandPosition technicsRequestCommandPosition = GetTechnicsRequestCommandPosition(currentUser, technicsRequestCommandPositionId);

            string logDescription = "";
            logDescription += "Заявка №: " + technicsRequestCommandPosition.TechnicsRequestsCommand.EquipmentTechnicsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(technicsRequestCommandPosition.TechnicsRequestsCommand.EquipmentTechnicsRequest.RequestDate) +
                              "; Команда: " + technicsRequestCommandPosition.TechnicsRequestsCommand.MilitaryCommand.DisplayTextForSelection +
                              "; Вид техника: " + technicsRequestCommandPosition.TechnicsType.TypeName +
                              "; Нормативна категория (код): " + technicsRequestCommandPosition.NormativeTechnics.NormativeCode +
                              "; Коментар: " + technicsRequestCommandPosition.Comment;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string SQL = "";

            try
            {

                SQL += @"BEGIN
                            DELETE FROM PMIS_RES.TechRequestCmdPositionsMilDept
                            WHERE TechnicsRequestCmdPositionID = :TechnicsRequestCmdPositionID;
 
                            DELETE FROM PMIS_RES.TechnicsRequestCmdPositions 
                            WHERE TechnicsRequestCmdPositionID = :TechnicsRequestCmdPositionID;
                         END;";

                changeEvent = new ChangeEvent("RES_EquipTechRequests_Command_DeletePosition", logDescription, technicsRequestCommandPosition.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, currentUser);

                changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_Command_Position_TechnicsType", technicsRequestCommandPosition.TechnicsType.TypeName, "", currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_Command_Position_Comment", technicsRequestCommandPosition.Comment, "", currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_Command_Position_Count", technicsRequestCommandPosition.Count.ToString(), "", currentUser));
                changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_Command_Position_DriversCount", technicsRequestCommandPosition.DriversCount.ToString(), "", currentUser));

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsRequestCmdPositionID", OracleType.Number).Value = technicsRequestCommandPositionId;

                cmd.ExecuteNonQuery();

                EquipmentTechnicsRequestUtil.SetEquipmentTechnicsRequestModified(technicsRequestCommandPosition.TechnicsRequestsCommand.EquipmentTechnicsRequestId, currentUser);
            }
            finally
            {
                conn.Close();
            }

            if (changeEvent != null && changeEvent.ChangeEventDetails.Count > 0)
                changeEntry.AddEvent(changeEvent);
        }

        public static bool SwapTechnicsRequestCommandPositionsOrder(User pCurrentUser, Change pChangeEntry, TechnicsRequestCommandPosition pTechnicsRequestCommandPosition_1, TechnicsRequestCommandPosition pTechnicsRequestCommandPosition_2)
        {
            bool result = false;
            string SQL = "";
            SQL += @"BEGIN
                     UPDATE PMIS_RES.TechnicsRequestCmdPositions t
                     SET Seq = (SELECT Seq 
                                FROM PMIS_RES.TechnicsRequestCmdPositions t2 
                                WHERE t2.TechnicsRequestCmdPositionID = :TechnicsPositionID_2)
                     WHERE t.TechRequestsCommandID = :TechnicsRequestsCommandID AND
                           t.TechnicsRequestCmdPositionID = :TechnicsPositionID_1;
                     
                     UPDATE PMIS_RES.TechnicsRequestCmdPositions t2
                     SET Seq = :TmpSeq
                     WHERE t2.TechRequestsCommandID = :TechnicsRequestsCommandID AND
                           t2.TechnicsRequestCmdPositionID = :TechnicsPositionID_2;
                    END;";

            ChangeEvent changeEvent = null;
            string logDescription = "";
            logDescription += "Заявка №: " + pTechnicsRequestCommandPosition_1.TechnicsRequestsCommand.EquipmentTechnicsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(pTechnicsRequestCommandPosition_1.TechnicsRequestsCommand.EquipmentTechnicsRequest.RequestDate) +
                              "; Команда: " + pTechnicsRequestCommandPosition_1.TechnicsRequestsCommand.MilitaryCommand.DisplayTextForSelection +
                              "; Вид техника: " + pTechnicsRequestCommandPosition_1.TechnicsType.TypeName +
                              "; Нормативна категория (код): " + pTechnicsRequestCommandPosition_1.NormativeTechnics.NormativeCode;

            OracleConnection conn = new OracleConnection(pCurrentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "TechnicsRequestsCommandID";
               
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = pTechnicsRequestCommandPosition_1.TechnicsRequestsCommand.TechnicsRequestCommandId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "TechnicsPositionID_1";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = pTechnicsRequestCommandPosition_1.TechnicsRequestCommandPositionId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "TechnicsPositionID_2";
                
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = pTechnicsRequestCommandPosition_2.TechnicsRequestCommandPositionId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "TmpSeq";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.InputOutput;
                param.Value = pTechnicsRequestCommandPosition_1.Seq;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();
                changeEvent = new ChangeEvent("RES_EquipTechRequests_Command_MovePosition", logDescription, pTechnicsRequestCommandPosition_1.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, pCurrentUser);

                changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_Command_Position_Seq", pTechnicsRequestCommandPosition_1.Seq.ToString(), pTechnicsRequestCommandPosition_2.Seq.ToString(), pCurrentUser));

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

        public static bool RearrangeTechnicsRequestCommandPositions(int pTechnicsRequestCommandId, User pCurrentUser)
        {
            bool result = false;
            string SQL = "";
            SQL += @"UPDATE PMIS_RES.TechnicsRequestCmdPositions t
                     SET Seq = (
                          SELECT Rank
                          FROM (
                                 SELECT TechnicsRequestCmdPositionID, TechRequestsCommandID, Seq,
                                        RANK() OVER(PARTITION BY TechRequestsCommandID ORDER BY Seq, TechnicsRequestCmdPositionID ASC NULLS LAST) Rank
                                 FROM PMIS_RES.TechnicsRequestCmdPositions
                          )  tmp
                          WHERE tmp.TechnicsRequestCmdPositionID = t.TechnicsRequestCmdPositionID
                                
                    )
                    WHERE t.TechRequestsCommandID = :TechnicsRequestsCommandID
                    ";

            OracleConnection conn = new OracleConnection(pCurrentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "TechnicsRequestsCommandID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = pTechnicsRequestCommandId;
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
    }
}