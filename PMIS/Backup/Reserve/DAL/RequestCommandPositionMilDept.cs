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
    //This class represents a command position dispatched to particular military department for fulfillment
    public class RequestCommandPositionMilDept : BaseDbObject
    {
        private int reqCommandPositionMilDeptID;        
        private int requestCommandPositionID;
        private int militaryDepartmentID;
        private MilitaryDepartment militaryDepartment;               
        private int? reservistsCount;

        public int ReqCommandPositionMilDeptID
        {
            get { return reqCommandPositionMilDeptID; }
            set { reqCommandPositionMilDeptID = value; }
        }

        public int RequestCommandPositionID
        {
            get { return requestCommandPositionID; }
            set { requestCommandPositionID = value; }
        }

        public int MilitaryDepartmentID
        {
            get 
            { return militaryDepartmentID; }
            set { militaryDepartmentID = value; }
        }

        public MilitaryDepartment MilitaryDepartment
        {
            get 
            {
                if (militaryDepartment == null)
                    militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment(MilitaryDepartmentID, CurrentUser);

                return militaryDepartment; 
            }
            set { militaryDepartment = value; }
        }

        public int? ReservistsCount
        {
            get { return reservistsCount; }
            set { reservistsCount = value; }
        }

        public RequestCommandPositionMilDept(User user)
            : base(user)
        {

        }
    }

    //Some methods for working with RequestCommandPositionMilDept objects
    public static class RequestCommandPositionMilDeptUtil
    {
        //This method creates and returns a RequestCommandPositionMilDept object. It extracts the data from a DataReader.
        //This is done in this way to have a signle place of creation of this object, however, pass the particular DataReader
        //object from various queries for better performance instead of executing a new call to the DB here to get the details for a specific ReservistsCount, for example.
        public static RequestCommandPositionMilDept ExtractRequestCommandPositionMilDeptFromDataReader(OracleDataReader dr, User currentUser)
        {
            RequestCommandPositionMilDept requestCommandPositionMilDept = new RequestCommandPositionMilDept(currentUser);

            requestCommandPositionMilDept.ReqCommandPositionMilDeptID = DBCommon.GetInt(dr["ReqCommandPositionMilDeptID"]);
            requestCommandPositionMilDept.RequestCommandPositionID = DBCommon.GetInt(dr["RequestCommandPositionID"]);
            requestCommandPositionMilDept.MilitaryDepartmentID = DBCommon.GetInt(dr["MilitaryDepartmentID"]);
            requestCommandPositionMilDept.ReservistsCount = DBCommon.IsInt(dr["MilDeptReservistCount"]) ? (int?)DBCommon.GetInt(dr["MilDeptReservistCount"]) : null;

            requestCommandPositionMilDept.MilitaryDepartment = MilitaryDepartmentUtil.ExtractMilitaryDepartmentFromDataReader(dr, currentUser); //avoid lazy initialization

            return requestCommandPositionMilDept;
        }

        //Get a specific RequestCommandPositionMilDept record
        public static RequestCommandPositionMilDept GetRequestCommandPositionMilDept(int reqCommandPositionMilDeptID, User currentUser)
        {
            RequestCommandPositionMilDept requestCommandPositionMilDept = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.ReqCommandPositionMilDeptID,
                                      a.RequestCommandPositionID,
                                      a.MilitaryDepartmentID,
                                      a.ReservistsCount as MilDeptReservistCount,
                                      b.MilitaryDepartmentName
                               FROM PMIS_RES.RequestCommandPositionsMilDept a
                               INNER JOIN PMIS_ADM.MilitaryDepartments b ON a.MilitaryDepartmentID = b.MilitaryDepartmentID
                               WHERE a.ReqCommandPositionMilDeptID = :ReqCommandPositionMilDeptID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("ReqCommandPositionMilDeptID", OracleType.Number).Value = reqCommandPositionMilDeptID;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    requestCommandPositionMilDept = ExtractRequestCommandPositionMilDeptFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return requestCommandPositionMilDept;
        }

        //Get a list of all RequestCommandPositionMilDept records for a particular request command position
        public static List<RequestCommandPositionMilDept> GetAllRequestCommandPositionMilDeptsByCommandPosition(int requestCommandPositionID, User currentUser)
        {
            List<RequestCommandPositionMilDept> result = new List<RequestCommandPositionMilDept>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.ReqCommandPositionMilDeptID,
                                      a.RequestCommandPositionID,
                                      a.MilitaryDepartmentID,
                                      a.ReservistsCount as MilDeptReservistCount,
                                      b.MilitaryDepartmentName
                               FROM PMIS_RES.RequestCommandPositionsMilDept a
                               INNER JOIN PMIS_ADM.MilitaryDepartments b ON a.MilitaryDepartmentID = b.MilitaryDepartmentID
                               WHERE a.RequestCommandPositionID = :RequestCommandPositionID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RequestCommandPositionID", OracleType.Number).Value = requestCommandPositionID;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {                                       
                    result.Add(ExtractRequestCommandPositionMilDeptFromDataReader(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public static bool AddMilitaryDepartmentToRequestCommandPositions(int equipmentReservistsRequestID, int requestCommandID, int militaryDepartmentID, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            EquipmentReservistsRequest equipmentReservistsRequest = EquipmentReservistsRequestUtil.GetEquipmentReservistsRequest(equipmentReservistsRequestID, currentUser);

            RequestCommand requestCommand = RequestCommandUtil.GetRequestsCommand(currentUser, requestCommandID);

            string logDescription = "";
            logDescription += "Заявка №: " + equipmentReservistsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(equipmentReservistsRequest.RequestDate) +
                              "; Команда: " + requestCommand.MilitaryCommand.DisplayTextForSelection;

            ChangeEvent changeEvent = new ChangeEvent("RES_EquipResRequests_AddMilDept", logDescription, equipmentReservistsRequest.MilitaryUnit, null, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_MilitaryDepartment", "", MilitaryDepartmentUtil.GetMilitaryDepartment(militaryDepartmentID, currentUser).MilitaryDepartmentName, currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"INSERT INTO PMIS_RES.RequestCommandPositionsMilDept (RequestCommandPositionID, MilitaryDepartmentID, ReservistsCount)
                         (SELECT a.RequestCommandPositionID, :MilitaryDepartmentID, CASE (SELECT COUNT(*) FROM PMIS_RES.RequestCommandPositionsMilDept x WHERE x.RequestCommandPositionID = a.RequestCommandPositionID) WHEN 0 THEN a.ReservistsCount ELSE NULL END
                          FROM PMIS_RES.RequestCommandPositions a
                          WHERE a.RequestsCommandID = :RequestsCommandID)
                       ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RequestsCommandID", OracleType.Number).Value = requestCommandID;
                cmd.Parameters.Add("MilitaryDepartmentID", OracleType.Number).Value = militaryDepartmentID;

                cmd.ExecuteNonQuery();

                result = true;
            }
            finally
            {
                conn.Close();
            }

            if (result)
                changeEntry.AddEvent(changeEvent);

            return result;
        }

        public static bool SaveMilitaryDepartmentsToRequestCommandPositions(int equipmentReservistsRequestID, List<RequestCommandPosition> oldPositions, List<RequestCommandPosition> newPositions, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            EquipmentReservistsRequest equipmentReservistsRequest = EquipmentReservistsRequestUtil.GetEquipmentReservistsRequest(equipmentReservistsRequestID, currentUser);

            string logDescription = "";            

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            var positions = (from n in newPositions
                             join o in oldPositions on n.RequestCommandPositionId equals o.RequestCommandPositionId select new { o.RequestCommandPositionId, o.RequestsCommand, newMilDepts = n.PositionMilitaryDepartments, oldMilDepts = o.PositionMilitaryDepartments  });

            try
            {
                SQL = @"BEGIN
                        
                       ";

                foreach (var position in positions)
                {
                    var milDepts = (from n in position.newMilDepts
                                    join o in position.oldMilDepts on n.ReqCommandPositionMilDeptID equals o.ReqCommandPositionMilDeptID
                                    select new { o.ReqCommandPositionMilDeptID, o.MilitaryDepartment, newReservistsCount = n.ReservistsCount, oldReservistsCount = o.ReservistsCount });

                    foreach (var milDept in milDepts)
                    {
                        logDescription += "Заявка №: " + equipmentReservistsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(equipmentReservistsRequest.RequestDate) +
                              "; Команда: " + position.RequestsCommand.MilitaryCommand.DisplayTextForSelection +
                              "; Военно окръжие: " + milDept.MilitaryDepartment.MilitaryDepartmentName;

                        ChangeEvent changeEvent = new ChangeEvent("RES_EquipResRequests_EditMilDept", logDescription, equipmentReservistsRequest.MilitaryUnit, null, currentUser);

                        changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_MilitaryDepartment_ReservistsCount", milDept.oldReservistsCount.HasValue ? milDept.oldReservistsCount.Value.ToString() : "", milDept.newReservistsCount.HasValue ? milDept.newReservistsCount.Value.ToString() : "", currentUser));

                        SQL += @"UPDATE PMIS_RES.RequestCommandPositionsMilDept SET
                                    ReservistsCount = " + (milDept.newReservistsCount.HasValue ? milDept.newReservistsCount.Value.ToString() : "NULL") + @"
                                 WHERE ReqCommandPositionMilDeptID = " + milDept.ReqCommandPositionMilDeptID + @" ;
                               ";

                        changeEntry.AddEvent(changeEvent);
                    }
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);                

                cmd.ExecuteNonQuery();

                result = true;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public static bool DeleteMilitaryDepartmentFromRequestCommandPositions(int equipmentReservistsRequestID, int requestCommandID, int militaryDepartmentID, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            EquipmentReservistsRequest equipmentReservistsRequest = EquipmentReservistsRequestUtil.GetEquipmentReservistsRequest(equipmentReservistsRequestID, currentUser);

            RequestCommand requestCommand = RequestCommandUtil.GetRequestsCommand(currentUser, requestCommandID);

            string logDescription = "";
            logDescription += "Заявка №: " + equipmentReservistsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(equipmentReservistsRequest.RequestDate) +
                              "; Команда: " + requestCommand.MilitaryCommand.DisplayTextForSelection;

            ChangeEvent changeEvent = new ChangeEvent("RES_EquipResRequests_DeleteMilDept", logDescription, equipmentReservistsRequest.MilitaryUnit, null, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_MilitaryDepartment", MilitaryDepartmentUtil.GetMilitaryDepartment(militaryDepartmentID, currentUser).MilitaryDepartmentName, "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @" DELETE FROM PMIS_RES.RequestCommandPositionsMilDept
                         WHERE MilitaryDepartmentID = :MilitaryDepartmentID AND 
                               RequestCommandPositionID IN (SELECT a.RequestCommandPositionID
                                                            FROM PMIS_RES.RequestCommandPositions a
                                                            WHERE a.RequestsCommandID = :RequestsCommandID)
                       ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RequestsCommandID", OracleType.Number).Value = requestCommandID;
                cmd.Parameters.Add("MilitaryDepartmentID", OracleType.Number).Value = militaryDepartmentID;

                cmd.ExecuteNonQuery();

                result = true;
            }
            finally
            {
                conn.Close();
            }

            if (result)
                changeEntry.AddEvent(changeEvent);

            return result;
        }
    }
}