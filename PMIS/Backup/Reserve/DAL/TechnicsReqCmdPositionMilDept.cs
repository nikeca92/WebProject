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
    //This class represents a technics command position dispatched to particular military department for fulfillment
    public class TechnicsReqCmdPositionMilDept : BaseDbObject
    {
        private int techReqCommandPositionMilDeptID;        
        private int techRequestCommandPositionID;
        private int militaryDepartmentID;
        private MilitaryDepartment militaryDepartment;               
        private int? count;
        private int? driversCount;

        public int TechReqCommandPositionMilDeptID
        {
            get { return techReqCommandPositionMilDeptID; }
            set { techReqCommandPositionMilDeptID = value; }
        }

        public int TechRequestCommandPositionID
        {
            get { return techRequestCommandPositionID; }
            set { techRequestCommandPositionID = value; }
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

        public int? Count
        {
            get { return count; }
            set { count = value; }
        }

        public int? DriversCount
        {
            get { return driversCount; }
            set { driversCount = value; }
        }

        public TechnicsReqCmdPositionMilDept(User user)
            : base(user)
        {

        }
    }

    //Some methods for working with TechnicsReqCmdPositionMilDept objects
    public static class TechnicsReqCmdPositionMilDeptUtil
    {
        //This method creates and returns a TechnicsReqCmdPositionMilDept object. It extracts the data from a DataReader.
        //This is done in this way to have a signle place of creation of this object, however, pass the particular DataReader
        //object from various queries for better performance instead of executing a new call to the DB here to get the details for a specific object, for example.
        public static TechnicsReqCmdPositionMilDept ExtractTechnicsReqCmdPositionMilDeptFromDataReader(OracleDataReader dr, User currentUser)
        {
            TechnicsReqCmdPositionMilDept technicsReqCmdPositionMilDept = new TechnicsReqCmdPositionMilDept(currentUser);

            technicsReqCmdPositionMilDept.TechReqCommandPositionMilDeptID = DBCommon.GetInt(dr["TechReqCmdPositionsMilDeptID"]);
            technicsReqCmdPositionMilDept.TechRequestCommandPositionID = DBCommon.GetInt(dr["TechnicsRequestCmdPositionID"]);
            technicsReqCmdPositionMilDept.MilitaryDepartmentID = DBCommon.GetInt(dr["MilitaryDepartmentID"]);
            technicsReqCmdPositionMilDept.Count = DBCommon.IsInt(dr["MilDeptCount"]) ? (int?)DBCommon.GetInt(dr["MilDeptCount"]) : null;
            technicsReqCmdPositionMilDept.DriversCount = DBCommon.IsInt(dr["MilDeptDriversCount"]) ? (int?)DBCommon.GetInt(dr["MilDeptDriversCount"]) : null;

            technicsReqCmdPositionMilDept.MilitaryDepartment = MilitaryDepartmentUtil.ExtractMilitaryDepartmentFromDataReader(dr, currentUser); //avoid lazy initialization

            return technicsReqCmdPositionMilDept;
        }

        //Get a specific TechnicsReqCmdPositionMilDept record
        public static TechnicsReqCmdPositionMilDept GetTechnicsReqCmdPositionMilDept(int techReqCmdPositionsMilDeptID, User currentUser)
        {
            TechnicsReqCmdPositionMilDept technicsReqCmdPositionMilDept = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TechReqCmdPositionsMilDeptID,
                                      a.TechnicsRequestCmdPositionID,
                                      a.MilitaryDepartmentID,
                                      a.Count as MilDeptCount,
                                      a.DriversCount as MilDeptDriversCount,
                                      b.MilitaryDepartmentName
                               FROM PMIS_RES.TechRequestCmdPositionsMilDept a
                               INNER JOIN PMIS_ADM.MilitaryDepartments b ON a.MilitaryDepartmentID = b.MilitaryDepartmentID
                               WHERE a.TechReqCmdPositionsMilDeptID = :TechReqCmdPositionsMilDeptID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechReqCmdPositionsMilDeptID", OracleType.Number).Value = techReqCmdPositionsMilDeptID;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    technicsReqCmdPositionMilDept = ExtractTechnicsReqCmdPositionMilDeptFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return technicsReqCmdPositionMilDept;
        }

        //Get a list of all TechnicsReqCmdPositionMilDept records for a particular request command position
        public static List<TechnicsReqCmdPositionMilDept> GetAllTechnicsReqCmdPositionMilDeptByCommandPosition(int technicsRequestCmdPositionId, User currentUser)
        {
            List<TechnicsReqCmdPositionMilDept> result = new List<TechnicsReqCmdPositionMilDept>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.TechReqCmdPositionsMilDeptID,
                                      a.TechnicsRequestCmdPositionID,
                                      a.MilitaryDepartmentID,
                                      a.Count as MilDeptCount,
                                      b.MilitaryDepartmentName
                               FROM PMIS_RES.TechRequestCmdPositionsMilDept a
                               INNER JOIN PMIS_ADM.MilitaryDepartments b ON a.MilitaryDepartmentID = b.MilitaryDepartmentID
                               WHERE a.TechnicsRequestCmdPositionID = :TechnicsRequestCmdPositionID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsRequestCmdPositionID", OracleType.Number).Value = technicsRequestCmdPositionId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {                                       
                    result.Add(ExtractTechnicsReqCmdPositionMilDeptFromDataReader(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public static bool AddMilitaryDepartmentToTechRequestCommandPositions(int equipmentTechnicsRequestId, int techRequestCommandId, int militaryDepartmentID, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            EquipmentTechnicsRequest equipmentTechnicsRequest = EquipmentTechnicsRequestUtil.GetEquipmentTechnicsRequest(equipmentTechnicsRequestId, currentUser);

            TechnicsRequestCommand technicsRequestCommand = TechnicsRequestCommandUtil.GetTechnicsRequestCommand(currentUser, techRequestCommandId);

            string logDescription = "";
            logDescription += "Заявка №: " + equipmentTechnicsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(equipmentTechnicsRequest.RequestDate) +
                              "; Команда: " + technicsRequestCommand.MilitaryCommand.DisplayTextForSelection;

            ChangeEvent changeEvent = new ChangeEvent("RES_EquipTechRequests_AddMilDept", logDescription, equipmentTechnicsRequest.MilitaryUnit, null, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_MilitaryDepartment", "", MilitaryDepartmentUtil.GetMilitaryDepartment(militaryDepartmentID, currentUser).MilitaryDepartmentName, currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @" INSERT INTO PMIS_RES.TechRequestCmdPositionsMilDept (TechnicsRequestCmdPositionID, MilitaryDepartmentID, 
                                                                              Count, 
                                                                              DriversCount)
                         (SELECT a.TechnicsRequestCmdPositionID, :MilitaryDepartmentID, 
                                 CASE (SELECT COUNT(*) FROM PMIS_RES.TechRequestCmdPositionsMilDept x WHERE x.TechnicsRequestCmdPositionID = a.TechnicsRequestCmdPositionID) WHEN 0 THEN a.Count ELSE NULL END,
                                 CASE (SELECT COUNT(*) FROM PMIS_RES.TechRequestCmdPositionsMilDept x WHERE x.TechnicsRequestCmdPositionID = a.TechnicsRequestCmdPositionID) WHEN 0 THEN a.DriversCount ELSE NULL END
                          FROM PMIS_RES.TechnicsRequestCmdPositions a
                          WHERE a.TechRequestsCommandID = :TechRequestsCommandID)";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechRequestsCommandID", OracleType.Number).Value = techRequestCommandId;
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

        public static bool SaveMilitaryDepartmentsToTechRequestCommandPositions(int equipmentTechnicsRequestId, List<TechnicsRequestCommandPosition> oldPositions, List<TechnicsRequestCommandPosition> newPositions, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            EquipmentTechnicsRequest equipmentTechnicsRequest = EquipmentTechnicsRequestUtil.GetEquipmentTechnicsRequest(equipmentTechnicsRequestId, currentUser);

            string logDescription = "";            

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            var positions = (from n in newPositions
                             join o in oldPositions on n.TechnicsRequestCommandPositionId equals o.TechnicsRequestCommandPositionId select new { o.TechnicsRequestCommandPositionId, o.TechnicsRequestsCommand, newMilDepts = n.TechnicsPositionMilitaryDepartments, oldMilDepts = o.TechnicsPositionMilitaryDepartments  });

            try
            {
                SQL = @"BEGIN
                        
                       ";

                foreach (var position in positions)
                {
                    var milDepts = (from n in position.newMilDepts
                                    join o in position.oldMilDepts on n.TechReqCommandPositionMilDeptID equals o.TechReqCommandPositionMilDeptID
                                    select new { o.TechReqCommandPositionMilDeptID, o.MilitaryDepartment, newCount = n.Count, oldCount = o.Count,
                                                 newDriversCount = n.DriversCount,
                                                 oldDriversCount = o.DriversCount
                                               });

                    foreach (var milDept in milDepts)
                    {
                        TechnicsRequestCommandPosition techRequestCommandPosition = TechnicsRequestCommandPositionUtil.GetTechnicsRequestCommandPosition(currentUser, position.TechnicsRequestCommandPositionId);

                        logDescription += "Заявка №: " + equipmentTechnicsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(equipmentTechnicsRequest.RequestDate) +
                              "; Команда: " + position.TechnicsRequestsCommand.MilitaryCommand.DisplayTextForSelection +
                              "; Вид техника: " + techRequestCommandPosition.TechnicsType.TypeName +
                              "; Нормативна категория (код): " + (techRequestCommandPosition.NormativeTechnics != null ? techRequestCommandPosition.NormativeTechnics.NormativeCode : "") +
                              "; Военно окръжие: " + milDept.MilitaryDepartment.MilitaryDepartmentName;

                        ChangeEvent changeEvent = new ChangeEvent("RES_EquipTechRequests_EditMilDept", logDescription, equipmentTechnicsRequest.MilitaryUnit, null, currentUser);

                        if (!CommonFunctions.IsEqualInt(milDept.oldCount, milDept.newCount))
                            changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_MilitaryDepartment_Count", milDept.oldCount.HasValue ? milDept.oldCount.Value.ToString() : "", milDept.newCount.HasValue ? milDept.newCount.Value.ToString() : "", currentUser));

                        if (!CommonFunctions.IsEqualInt(milDept.oldDriversCount, milDept.newDriversCount))
                            changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_MilitaryDepartment_DriversCount", milDept.oldDriversCount.HasValue ? milDept.oldDriversCount.Value.ToString() : "", milDept.newDriversCount.HasValue ? milDept.newDriversCount.Value.ToString() : "", currentUser));

                        SQL += @"UPDATE PMIS_RES.TechRequestCmdPositionsMilDept SET
                                    Count        = " + (milDept.newCount.HasValue ? milDept.newCount.Value.ToString() : "NULL") + @",
                                    DriversCount = " + (milDept.newDriversCount.HasValue ? milDept.newDriversCount.Value.ToString() : "NULL") + @"
                                 WHERE TechReqCmdPositionsMilDeptID = " + milDept.TechReqCommandPositionMilDeptID + @" ;
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

        public static bool DeleteMilitaryDepartmentFromTechnicsRequestCommandPositions(int equipmentTechnicsRequestId, int technicsRequestCommandId, int militaryDepartmentId, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            EquipmentTechnicsRequest equipmentTechnicsRequest = EquipmentTechnicsRequestUtil.GetEquipmentTechnicsRequest(equipmentTechnicsRequestId, currentUser);

            TechnicsRequestCommand technicsRequestCommand = TechnicsRequestCommandUtil.GetTechnicsRequestCommand(currentUser, technicsRequestCommandId);

            string logDescription = "";
            logDescription += "Заявка №: " + equipmentTechnicsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(equipmentTechnicsRequest.RequestDate) +
                              "; Команда: " + technicsRequestCommand.MilitaryCommand.DisplayTextForSelection;

            ChangeEvent changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteMilDept", logDescription, equipmentTechnicsRequest.MilitaryUnit, null, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_MilitaryDepartment", MilitaryDepartmentUtil.GetMilitaryDepartment(militaryDepartmentId, currentUser).MilitaryDepartmentName, "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @" DELETE FROM PMIS_RES.TechRequestCmdPositionsMilDept
                         WHERE MilitaryDepartmentID = :MilitaryDepartmentID AND 
                               TechnicsRequestCmdPositionID IN (SELECT a.TechnicsRequestCmdPositionID
                                                                FROM PMIS_RES.TechnicsRequestCmdPositions a
                                                                WHERE a.TechRequestsCommandID = :TechRequestsCommandID)
                       ";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechRequestsCommandID", OracleType.Number).Value = technicsRequestCommandId;
                cmd.Parameters.Add("MilitaryDepartmentID", OracleType.Number).Value = militaryDepartmentId;

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