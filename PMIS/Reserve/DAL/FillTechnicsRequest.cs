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
    //This class represents a FillTechnicsRequest record
    public class FillTechnicsRequest : BaseDbObject
    {
        private int fillTechnicsRequestID;
        private int technicsRequestCommandPositionID;
        private int militaryDepartmentID;        
        private MilitaryDepartment militaryDepartment;        
        private int technicsID;
        //private Technic reservist;        
        private int technicReadinessID;
        private bool appointmentIsDelivered;

        public int FillTechnicsRequestID
        {
            get { return fillTechnicsRequestID; }
            set { fillTechnicsRequestID = value; }
        }

        public int TechnicsRequestCommandPositionID
        {
            get { return technicsRequestCommandPositionID; }
            set { technicsRequestCommandPositionID = value; }
        }

        public int MilitaryDepartmentID
        {
            get { return militaryDepartmentID; }
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

        public int TechnicsID
        {
            get { return technicsID; }
            set { technicsID = value; }
        }       

        public int TechnicReadinessID
        {
            get { return technicReadinessID; }
            set { technicReadinessID = value; }
        }

        public string TechnicReadiness
        {
            get
            {
                return ReadinessUtil.ReadinessName(TechnicReadinessID);
            }
        }

        public bool AppointmentIsDelivered
        {
            get { return appointmentIsDelivered; }
            set { appointmentIsDelivered = value; }
        }

        public FillTechnicsRequest(User user)
            : base(user)
        {

        }
    }

    //Some methods for working with FillTechnicsRequest objects
    public static class FillTechnicsRequestUtil
    {
        //This method creates and returns a FillTechnicsRequest object. It extracts the data from a DataReader.
        //This is done in this way to have a signle place of creation of this object, however, pass the particular DataReader
        //object from various queries for better performance instead of executing a new call to the DB
        public static FillTechnicsRequest ExtractFillTechnicsRequest(OracleDataReader dr, User currentUser)
        {
            FillTechnicsRequest fillTechnicsRequest = new FillTechnicsRequest(currentUser);

            fillTechnicsRequest.FillTechnicsRequestID = DBCommon.GetInt(dr["FulfilTechnicsRequestID"]);
            fillTechnicsRequest.TechnicsRequestCommandPositionID = DBCommon.GetInt(dr["TechnicsRequestCmdPositionID"]);
            fillTechnicsRequest.MilitaryDepartmentID = DBCommon.GetInt(dr["MilitaryDepartmentID"]);
            fillTechnicsRequest.TechnicsID = DBCommon.GetInt(dr["TechnicsID"]);
            fillTechnicsRequest.TechnicReadinessID = DBCommon.GetInt(dr["TechnicReadinessID"]);
            fillTechnicsRequest.AppointmentIsDelivered = DBCommon.GetInt(dr["AppointmentIsDelivered"]) == 1;

            return fillTechnicsRequest;
        }

        //Get a specific FillTechnicsRequest record
        public static FillTechnicsRequest GetFillTechnicsRequest(int fillTechnicsRequestID, User currentUser)
        {
            FillTechnicsRequest fillTechnicsRequest = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.FulfilTechnicsRequestID,
                                      a.TechnicsRequestCmdPositionID,
                                      a.MilitaryDepartmentID,
                                      a.TechnicsID,
                                      a.TechnicReadinessID,
                                      a.AppointmentIsDelivered
                               FROM PMIS_RES.FulfilTechnicsRequest a                               
                               WHERE a.FulfilTechnicsRequestID = :FulfilTechnicsRequestID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("FulfilTechnicsRequestID", OracleType.Number).Value = fillTechnicsRequestID;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    fillTechnicsRequest = ExtractFillTechnicsRequest(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return fillTechnicsRequest;
        }

        //Get all FillTechnicsRequest records for a particular TechnicsID
        public static List<FillTechnicsRequest> GetFillTechnicsRequestByTechnicsId(int technicsId, User currentUser)
        {
            List<FillTechnicsRequest> fillTechnicsRequests = new List<FillTechnicsRequest>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.FulfilTechnicsRequestID,
                                      a.TechnicsRequestCmdPositionID,
                                      a.MilitaryDepartmentID,
                                      a.TechnicsID,
                                      a.TechnicReadinessID,
                                      a.AppointmentIsDelivered
                               FROM PMIS_RES.FulfilTechnicsRequest a                               
                               WHERE a.TechnicsID = :TechnicsID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsID", OracleType.Number).Value = technicsId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    fillTechnicsRequests.Add(ExtractFillTechnicsRequest(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return fillTechnicsRequests;
        }

        //Save a position for a particular FillTechnicsRequest
        public static bool SaveRequestCommandTechnic(FillTechnicsRequest fillTechnicsRequest, User currentUser, ChangeEvent changeEvent)
        {
            bool result = false;

            string SQL = "";
           
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();            

            try
            {
                SQL = @"BEGIN
                        
                       ";

                SQL += @"INSERT INTO PMIS_RES.FulfilTechnicsRequest (TechnicsRequestCmdPositionID, MilitaryDepartmentID, TechnicsID, TechnicReadinessID)
                            VALUES (:TechnicsRequestCmdPositionID, :MilitaryDepartmentID, :TechnicsID, :TechnicReadinessID);

                            SELECT PMIS_RES.FulfilTechnicsRequest_ID_SEQ.currval INTO :FulfilTechnicsRequestID FROM dual;

                            ";

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramFulfilTechnicsRequestID = new OracleParameter();
                paramFulfilTechnicsRequestID.ParameterName = "FulfilTechnicsRequestID";
                paramFulfilTechnicsRequestID.OracleType = OracleType.Number;
                paramFulfilTechnicsRequestID.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(paramFulfilTechnicsRequestID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "TechnicsRequestCmdPositionID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = fillTechnicsRequest.TechnicsRequestCommandPositionID;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryDepartmentID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = fillTechnicsRequest.MilitaryDepartmentID;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "TechnicsID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = fillTechnicsRequest.TechnicsID;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "TechnicReadinessID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = fillTechnicsRequest.TechnicReadinessID;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                fillTechnicsRequest.FillTechnicsRequestID = DBCommon.GetInt(paramFulfilTechnicsRequestID.Value);

                result = true;
            }
            finally
            {
                conn.Close();
            }           

            return result;
        }

        //Delete a particualar FillTechnicsRequest
        public static void DeleteRequestCommandTechnic(int fulfilTechnicsRequestID, User currentUser, ChangeEvent changeEvent)
        {    
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string SQL = "";

            try
            {
                SQL += @"DELETE FROM PMIS_RES.FulfilTechnicsRequest
                         WHERE FulfilTechnicsRequestID = :FulfilTechnicsRequestID";                

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("FulfilTechnicsRequestID", OracleType.Number).Value = fulfilTechnicsRequestID;

                cmd.ExecuteNonQuery();                
            }
            finally
            {
                conn.Close();
            }  
        }

        //Get all FillTechnicsRequest records for a particular technics request command and military departments
        public static List<FillTechnicsRequest> GetFillTechnicsRequestByTechReqCommandAndMilDept(int technicsRequestCommandId, int militaryDepartmentId, User currentUser)
        {
            List<FillTechnicsRequest> fillTechnicsRequests = new List<FillTechnicsRequest>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.FulfilTechnicsRequestID,
                                      a.TechnicsRequestCmdPositionID,
                                      a.MilitaryDepartmentID,
                                      a.TechnicsID,
                                      a.TechnicReadinessID,
                                      a.AppointmentIsDelivered
                               FROM PMIS_RES.FulfilTechnicsRequest a                               
                               WHERE a.MilitaryDepartmentID = :MilitaryDepartmentID AND
                                     a.TechnicsRequestCmdPositionID IN (SELECT TechnicsRequestCmdPositionID
                                                                        FROM PMIS_RES.TechnicsRequestCmdPositions
                                                                        WHERE TechRequestsCommandID = :TechRequestsCommandID)";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitaryDepartmentID", OracleType.Number).Value = militaryDepartmentId;
                cmd.Parameters.Add("TechRequestsCommandID", OracleType.Number).Value = technicsRequestCommandId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    fillTechnicsRequests.Add(ExtractFillTechnicsRequest(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return fillTechnicsRequests;
        }


        //Get all FillTechnicsRequest records for a particular technics request command position
        public static List<FillTechnicsRequest> GetFillTechnicsRequestByTechReqCommandPosition(int technicsRequestCommandPositionId, User currentUser)
        {
            List<FillTechnicsRequest> fillTechnicsRequests = new List<FillTechnicsRequest>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.FulfilTechnicsRequestID,
                                      a.TechnicsRequestCmdPositionID,
                                      a.MilitaryDepartmentID,
                                      a.TechnicsID,
                                      a.TechnicReadinessID,
                                      a.AppointmentIsDelivered
                               FROM PMIS_RES.FulfilTechnicsRequest a                               
                               WHERE a.TechnicsRequestCmdPositionID = :TechnicsRequestCmdPositionID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechnicsRequestCmdPositionID", OracleType.Number).Value = technicsRequestCommandPositionId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    fillTechnicsRequests.Add(ExtractFillTechnicsRequest(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return fillTechnicsRequests;
        }

        //Get all FillTechnicsRequest records for a particular technics request command
        public static List<FillTechnicsRequest> GetFillTechnicsRequestByTechRequesCommand(int technicsRequestCommandId, User currentUser)
        {
            List<FillTechnicsRequest> fillTechnicsRequests = new List<FillTechnicsRequest>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.FulfilTechnicsRequestID,
                                      a.TechnicsRequestCmdPositionID,
                                      a.MilitaryDepartmentID,
                                      a.TechnicsID,
                                      a.TechnicReadinessID,
                                      a.AppointmentIsDelivered
                               FROM PMIS_RES.FulfilTechnicsRequest a                               
                               WHERE a.TechnicsRequestCmdPositionID IN (SELECT TechnicsRequestCmdPositionID
                                                                        FROM PMIS_RES.TechnicsRequestCmdPositions
                                                                        WHERE TechRequestsCommandID = :TechRequestsCommandID)";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechRequestsCommandID", OracleType.Number).Value = technicsRequestCommandId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    fillTechnicsRequests.Add(ExtractFillTechnicsRequest(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return fillTechnicsRequests;
        }


        //Get all FillTechnicsRequest records for a particular equipment technics request 
        public static List<FillTechnicsRequest> GetFillTechnicsRequestByEquipmentTechnicsRequest(int equipmentTechnicsRequestId, User currentUser)
        {
            List<FillTechnicsRequest> fillTechnicsRequests = new List<FillTechnicsRequest>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.FulfilTechnicsRequestID,
                                      a.TechnicsRequestCmdPositionID,
                                      a.MilitaryDepartmentID,
                                      a.TechnicsID,
                                      a.TechnicReadinessID,
                                      a.AppointmentIsDelivered
                               FROM PMIS_RES.FulfilTechnicsRequest a                               
                               WHERE a.TechnicsRequestCmdPositionID IN (SELECT TechnicsRequestCmdPositionID
                                                                        FROM PMIS_RES.TechnicsRequestCmdPositions
                                                                        WHERE TechRequestsCommandID IN (SELECT TechRequestsCommandID
                                                                                                        FROM PMIS_RES.TechnicsRequestCommands
                                                                                                        WHERE EquipmentTechnicsRequestID = :EquipmentTechnicsRequestID)
                                                                       )";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("EquipmentTechnicsRequestID", OracleType.Number).Value = equipmentTechnicsRequestId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    fillTechnicsRequests.Add(ExtractFillTechnicsRequest(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return fillTechnicsRequests;
        }
    }
}