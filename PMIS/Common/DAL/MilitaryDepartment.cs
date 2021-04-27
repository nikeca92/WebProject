using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OracleClient;
using PMIS.Common;

namespace PMIS.Common
{
    //This class represents the MilitaryDepartment object
    public class MilitaryDepartment : BaseDbObject, IDropDownItem
    {
        private int militaryDepartmentId;
        private string militaryDepartmentName;

        public int MilitaryDepartmentId
        {
            get
            {
                return militaryDepartmentId;
            }
            set
            {
                militaryDepartmentId = value;
            }
        }

        public string MilitaryDepartmentName
        {
            get
            {
                return militaryDepartmentName;
            }
            set
            {
                militaryDepartmentName = value;
            }
        }

        public MilitaryDepartment(User user) : base(user)
        {
        }        

        public string Text()
        {
            return MilitaryDepartmentName;
        }

        public string Value()
        {
            return MilitaryDepartmentId.ToString();
        }

    }

    //This class provides some methods for working with MilitaryDepartment objects
    public static class MilitaryDepartmentUtil
    {
        //This method extracts a new object with type of MilitaryDepartment from a particular data reader
        //It is defined as a separate method to be reused easier
        public static MilitaryDepartment ExtractMilitaryDepartmentFromDataReader(OracleDataReader dr, User currentUser)
        {
            int? militaryDepartmentId = null;

            if (DBCommon.IsInt(dr["MilitaryDepartmentID"]))
                militaryDepartmentId = DBCommon.GetInt(dr["MilitaryDepartmentID"]);

            string militaryDepartmentName = dr["MilitaryDepartmentName"].ToString();

            MilitaryDepartment militaryDepartment = new MilitaryDepartment(currentUser);

            if (militaryDepartmentId.HasValue)
            {
                militaryDepartment.MilitaryDepartmentId = militaryDepartmentId.Value;
                militaryDepartment.MilitaryDepartmentName = militaryDepartmentName;
            }

            return militaryDepartment;
        }

        //Return a single MilitaryDepartment object by its ID
        public static MilitaryDepartment GetMilitaryDepartment(int militaryDepartmentId, User currentUser)
        {
            MilitaryDepartment militaryDepartment = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.MilitaryDepartmentID, a.MilitaryDepartmentName
                               FROM PMIS_ADM.MilitaryDepartments a
                               WHERE a.MilitaryDepartmentID = :MilitaryDepartmentID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitaryDepartmentID", OracleType.Number).Value = militaryDepartmentId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    militaryDepartment = ExtractMilitaryDepartmentFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryDepartment;
        }

        //Return a list of all MilitaryDepartments assigned to the current user
        public static List<MilitaryDepartment> GetAllMilitaryDepartments(User currentUser)
        {
            return GetAllMilitaryDepartmentsPerUser(currentUser, currentUser);
        }

        //Return a list of all MilitaryDepartments assigned to a particular user user
        public static List<MilitaryDepartment> GetAllMilitaryDepartmentsPerUser(User currentUser, User user)
        {
            List<MilitaryDepartment> listMilitaryDepartments = new List<MilitaryDepartment>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.MilitaryDepartmentID, a.MilitaryDepartmentName
                                FROM PMIS_ADM.MilitaryDepartments a
                                WHERE a.MilitaryDepartmentID IN (" + user.MilitaryDepartmentIDs + @")
                                ORDER BY a.MilitaryDepartmentName";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["MilitaryDepartmentID"]))
                    {
                        MilitaryDepartment militaryDepartment = ExtractMilitaryDepartmentFromDataReader(dr, currentUser);
                        listMilitaryDepartments.Add(militaryDepartment);
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listMilitaryDepartments;
        }

        //Return a list of all MilitaryDepartments 
        public static List<MilitaryDepartment> GetAllMilitaryDepartmentsWithoutRestrictions(User currentUser)
        {
            List<MilitaryDepartment> listMilitaryDepartments = new List<MilitaryDepartment>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.MilitaryDepartmentID, a.MilitaryDepartmentName
                                FROM PMIS_ADM.MilitaryDepartments a
                                ORDER BY a.MilitaryDepartmentName";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["MilitaryDepartmentID"]))
                    {
                        MilitaryDepartment militaryDepartment = ExtractMilitaryDepartmentFromDataReader(dr, currentUser);
                        listMilitaryDepartments.Add(militaryDepartment);
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listMilitaryDepartments;
        }

        //Return a list of all MilitaryDepartments by their IDs
        public static List<MilitaryDepartment> GetAllMilitaryDepartmentsByIDs(User currentUser, string militaryDepartmentIds)
        {
            List<MilitaryDepartment> listMilitaryDepartments = new List<MilitaryDepartment>();

            if (String.IsNullOrEmpty(militaryDepartmentIds))
                militaryDepartmentIds = "-1";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.MilitaryDepartmentID, a.MilitaryDepartmentName
                                FROM PMIS_ADM.MilitaryDepartments a
                                WHERE a.MilitaryDepartmentID IN (" + militaryDepartmentIds + @")
                                ORDER BY a.MilitaryDepartmentName";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["MilitaryDepartmentID"]))
                    {
                        MilitaryDepartment militaryDepartment = ExtractMilitaryDepartmentFromDataReader(dr, currentUser);
                        listMilitaryDepartments.Add(militaryDepartment);
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listMilitaryDepartments;
        }

        //Return a list of all MilitaryDepartments assigned to the current user, which are not yet added to RequestCommandPosition
        public static List<MilitaryDepartment> GetAllMilitaryDepartmentsForRequestCommandPosition(int requestCommandID, User currentUser)
        {
            List<MilitaryDepartment> listMilitaryDepartments = new List<MilitaryDepartment>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.MilitaryDepartmentID, a.MilitaryDepartmentName
                                FROM PMIS_ADM.MilitaryDepartments a
                                WHERE a.MilitaryDepartmentID IN (" + currentUser.MilitaryDepartmentIDs + @") AND
                                      a.MilitaryDepartmentID NOT IN (SELECT b.MilitaryDepartmentID 
                                                                     FROM PMIS_RES.RequestCommandPositions a
                                                                     INNER JOIN PMIS_RES.RequestCommandPositionsMilDept b ON a.RequestCommandPositionID = b.RequestCommandPositionID
                                                                     WHERE a.RequestsCommandID = :RequestsCommandID)
                                ORDER BY a.MilitaryDepartmentName";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("RequestsCommandID", OracleType.Number).Value = requestCommandID;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["MilitaryDepartmentID"]))
                    {
                        MilitaryDepartment militaryDepartment = ExtractMilitaryDepartmentFromDataReader(dr, currentUser);
                        listMilitaryDepartments.Add(militaryDepartment);
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listMilitaryDepartments;
        }

        //Return a list of all MilitaryDepartments assigned to the current user, which are not yet added to TechnicsRequestCommandPosition
        public static List<MilitaryDepartment> GetAllMilitaryDepartmentsForTechnicsRequestCommandPosition(int technicsRequestCommandId, User currentUser)
        {
            List<MilitaryDepartment> listMilitaryDepartments = new List<MilitaryDepartment>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.MilitaryDepartmentID, a.MilitaryDepartmentName
                                FROM PMIS_ADM.MilitaryDepartments a
                                WHERE a.MilitaryDepartmentID IN (" + currentUser.MilitaryDepartmentIDs + @") AND
                                      a.MilitaryDepartmentID NOT IN (SELECT b.MilitaryDepartmentID 
                                                                     FROM PMIS_RES.TechnicsRequestCmdPositions a
                                                                     INNER JOIN PMIS_RES.TechRequestCmdPositionsMilDept b ON a.TechnicsRequestCmdPositionID = b.TechnicsRequestCmdPositionID
                                                                     WHERE a.TechRequestsCommandID = :TechRequestsCommandID)
                                ORDER BY a.MilitaryDepartmentName";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TechRequestsCommandID", OracleType.Number).Value = technicsRequestCommandId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["MilitaryDepartmentID"]))
                    {
                        MilitaryDepartment militaryDepartment = ExtractMilitaryDepartmentFromDataReader(dr, currentUser);
                        listMilitaryDepartments.Add(militaryDepartment);
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listMilitaryDepartments;
        }

        //Return a list of all MilitaryDepartments assigned to a particular user user
        public static List<MilitaryDepartment> GetAllMilitaryDepartmentsByEquipmentResRequestsPerUser(User currentUser, User user)
        {
            List<MilitaryDepartment> listMilitaryDepartments = new List<MilitaryDepartment>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.MilitaryDepartmentID, a.MilitaryDepartmentName
                               FROM PMIS_ADM.MilitaryDepartments a
                               WHERE a.MilitaryDepartmentID IN (
                                     " + user.MilitaryDepartmentIDs_ByEquipmentResRequests + @"
                               )
                               ORDER BY a.MilitaryDepartmentName";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["MilitaryDepartmentID"]))
                    {
                        MilitaryDepartment militaryDepartment = ExtractMilitaryDepartmentFromDataReader(dr, currentUser);
                        listMilitaryDepartments.Add(militaryDepartment);
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listMilitaryDepartments;
        }

        //Return a list of all MilitaryDepartments assigned to a particular user user
        public static List<MilitaryDepartment> GetAllMilitaryDepartmentsByEquipmentTechRequestsPerUser(User currentUser, User user)
        {
            List<MilitaryDepartment> listMilitaryDepartments = new List<MilitaryDepartment>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.MilitaryDepartmentID, a.MilitaryDepartmentName
                               FROM PMIS_ADM.MilitaryDepartments a
                               WHERE a.MilitaryDepartmentID IN (
                                     " + user.MilitaryDepartmentIDs_ByEquipmentTechRequests + @"
                               )
                               ORDER BY a.MilitaryDepartmentName";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["MilitaryDepartmentID"]))
                    {
                        MilitaryDepartment militaryDepartment = ExtractMilitaryDepartmentFromDataReader(dr, currentUser);
                        listMilitaryDepartments.Add(militaryDepartment);
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listMilitaryDepartments;
        }

        public static string GetAllMilitaryDepartmentIDsByEquipmentResRequestsPerUser(User user)
        {
            string sql = @"SELECT DISTINCT d.MilitaryDepartmentID
                           FROM PMIS_RES.EquipmentReservistsRequests a
                           LEFT OUTER JOIN PMIS_RES.RequestsCommands b ON a.EquipmentReservistsRequestID = b.EquipmentReservistsRequestID
                           LEFT OUTER JOIN PMIS_RES.RequestCommandPositions c ON b.RequestsCommandID = c.RequestsCommandID
                           LEFT OUTER JOIN PMIS_RES.RequestCommandPositionsMilDept d ON c.RequestCommandPositionID = d.RequestCommandPositionID
                           WHERE  (/*Ticket #128*/
                                   (d.MilitaryDepartmentID IS NULL AND a.MilitaryUnitID IS NULL) OR 
                                   d.MilitaryDepartmentID IN (" + user.MilitaryDepartmentIDs + @") OR
                                   a.MilitaryUnitID IN (" + user.MilitaryUnitIDs + @")
                                  ) ";
            return sql;
        }

        public static string GetAllMilitaryDepartmentIDsByEquipmentTechRequestsPerUser(User user)
        {
            string sql = @"SELECT DISTINCT d.MilitaryDepartmentID
                           FROM PMIS_RES.EquipmentTechnicsRequests a
                           LEFT OUTER JOIN PMIS_RES.TechnicsRequestCommands b ON a.EquipmentTechnicsRequestID = b.EquipmentTechnicsRequestID
                           LEFT OUTER JOIN PMIS_RES.TechnicsRequestCmdPositions c ON b.TechRequestsCommandID = c.TechRequestsCommandID
                           LEFT OUTER JOIN PMIS_RES.TechRequestCmdPositionsMilDept d ON c.TechnicsRequestCmdPositionID = d.TechnicsRequestCmdPositionID
                           WHERE  (/*Ticket #128*/
                                   (d.MilitaryDepartmentID IS NULL AND a.MilitaryUnitID IS NULL) OR 
                                   d.MilitaryDepartmentID IN (" + user.MilitaryDepartmentIDs + @") OR
                                   a.MilitaryUnitID IN (" + user.MilitaryUnitIDs + @")
                                  ) ";
            return sql;
        }

        public static string GetMilitaryDepartmentIDsPerUser(User user)
        {
            string sql = "SELECT MilitaryDepartmentID FROM PMIS_ADM.MilitaryDepartmentsPerUser WHERE UserID = " + user.UserId.ToString() + " ";
            return sql;
        }

        public static string GetMilitaryDepartmentIDsPerUser_ListOfValues(User user)
        {
            string sql = "0";

            List<MilitaryDepartment> militaryDepartments = MilitaryDepartmentUtil.GetAllMilitaryDepartmentsPerUser(user, user);

            if (militaryDepartments.Count > 0)
            {
                sql = "";

                foreach (MilitaryDepartment militaryDepartment in militaryDepartments)
                {
                    sql += (String.IsNullOrEmpty(sql) ? "" : ",") + militaryDepartment.MilitaryDepartmentId.ToString();
                }
            }

            return sql;
        }

        //Update the list of Military Departments per user
        public static bool UpdateMilitaryDepartmentsPerUser(User user, string newMilitaryDepartmentIds, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";
            
            //Changes log
            string logDescription = "Потребител: " + user.Username + "; Име: " + user.FullName;

            List<MilitaryDepartment> oldMilitaryDepartments = GetAllMilitaryDepartmentsPerUser(currentUser, user);
            List<MilitaryDepartment> newMilitaryDepartments = GetAllMilitaryDepartmentsByIDs(currentUser, newMilitaryDepartmentIds);
            
            var newMilDepartmentIds = (from nd in newMilitaryDepartments select nd.MilitaryDepartmentId);
            var oldMilDepartmentIds = (from od in oldMilitaryDepartments select od.MilitaryDepartmentId);

            var deletedMilitaryDepartments = (from d in oldMilitaryDepartments
                                              where !newMilDepartmentIds.Contains(d.MilitaryDepartmentId)
                                              select d);

            var newlyAddedMilitaryDepartments = (from d in newMilitaryDepartments
                                                 where !oldMilDepartmentIds.Contains(d.MilitaryDepartmentId)
                                                 select d);


            //Log all items that have been deleted if any
            foreach (MilitaryDepartment deletedMilitaryDepartment in deletedMilitaryDepartments)
            {
                ChangeEvent changeEvent = changeEvent = new ChangeEvent("ADM_MilStructureAccess_DelMilDepartment", logDescription, null, null, currentUser);
                changeEvent.AddDetail(new ChangeEventDetail("ADM_MilStructureAccess_MilDepartment", deletedMilitaryDepartment.MilitaryDepartmentName, "", currentUser));
                changeEntry.AddEvent(changeEvent);
            }

            //Log all items that have been newly added if any
            foreach (MilitaryDepartment newlyAddedMilitaryDepartment in newlyAddedMilitaryDepartments)
            {
                ChangeEvent changeEvent = changeEvent = new ChangeEvent("ADM_MilStructureAccess_AddMilDepartment", logDescription, null, null, currentUser);
                changeEvent.AddDetail(new ChangeEventDetail("ADM_MilStructureAccess_MilDepartment", "", newlyAddedMilitaryDepartment.MilitaryDepartmentName, currentUser));
                changeEntry.AddEvent(changeEvent);
            }

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                           DELETE FROM PMIS_ADM.MilitaryDepartmentsPerUser
                           WHERE UserID = :UserID;

                       ";


                if (!String.IsNullOrEmpty(newMilitaryDepartmentIds))
                {
                    SQL += @"INSERT INTO PMIS_ADM.MilitaryDepartmentsPerUser (UserID, MilitaryDepartmentID)
                             SELECT :UserID, a.MilitaryDepartmentID
                             FROM PMIS_ADM.MilitaryDepartments a
                             WHERE a.MilitaryDepartmentID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(newMilitaryDepartmentIds) + @");
                            ";
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("UserID", OracleType.Number).Value = user.UserId;

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
