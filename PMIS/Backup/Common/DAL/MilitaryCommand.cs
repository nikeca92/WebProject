using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;

namespace PMIS.Common
{
    //This class represents the Military Command objects (war time structures)
    public class MilitaryCommand : BaseDbObject
    {
        private int militaryCommandId;
        private string shortName;
        private string commandNumber;
        private int? militaryUnitId;
        private MilitaryUnit militaryUnit;

        public int MilitaryCommandId
        {
            get { return militaryCommandId; }
            set { militaryCommandId = value; }
        }

        public string ShortName
        {
            get { return shortName; }
            set { shortName = value; }
        }

        public string CommandNumber
        {
            get { return commandNumber; }
            set { commandNumber = value; }
        }

        public int? MilitaryUnitId
        {
            get { return militaryUnitId; }
            set { militaryUnitId = value; }
        }

        public MilitaryUnit MilitaryUnit
        {
            get
            {
                //Lazy initialization
                if (militaryUnit == null && MilitaryUnitId.HasValue)
                    militaryUnit = MilitaryUnitUtil.GetMilitaryUnit(MilitaryUnitId.Value, CurrentUser);

                return militaryUnit;
            }
            set
            {
                militaryUnit = value;
            }
        }

        public string DisplayTextForSelection
        {
            get
            {
                var text = "";

                if (!String.IsNullOrEmpty(CommandNumber))
                    text += CommandNumber;

                if (!String.IsNullOrEmpty(ShortName))
                    text += (String.IsNullOrEmpty(text) ? "" : " ") + ShortName;

                return text;
            }
        }

        public string DisplayTextForSelectionInclVPN
        {
            get
            {
                var text = "";

                if (!String.IsNullOrEmpty(CommandNumber))
                    text += CommandNumber;

                if (!String.IsNullOrEmpty(ShortName))
                    text += (String.IsNullOrEmpty(text) ? "" : " ") + ShortName;

                if (MilitaryUnit != null &&
                    !String.IsNullOrEmpty(MilitaryUnit.VPN))
                    text = MilitaryUnit.VPN + " " + text;   

                return text;
            }
        }

        public MilitaryCommand(User user)
            : base(user)
        {
        }     
    }

    //Some utility methods that help working with Military Command objects
    public static class MilitaryCommandUtil
    {
        //Extract a particular MilitaryCommand object from a data reader
        public static MilitaryCommand ExtractMilitaryCommandFromDR(User currentUser, OracleDataReader dr)
        {
            MilitaryCommand militaryCommand = new MilitaryCommand(currentUser);

            militaryCommand.MilitaryCommandId = (DBCommon.IsInt(dr["MilitaryCommandID"]) ? DBCommon.GetInt(dr["MilitaryCommandID"]) : 0);
            militaryCommand.ShortName = dr["ShortName"].ToString();
            militaryCommand.CommandNumber = dr["CommandNumber"].ToString();
            militaryCommand.MilitaryUnitId = (DBCommon.IsInt(dr["MilitaryUnitID"]) ? DBCommon.GetInt(dr["MilitaryUnitID"]) : (int?)null);

            return militaryCommand;
        }

        //Get a particualr MilitaryCommand object from the DB by its ID
        public static MilitaryCommand GetMilitaryCommand(int militaryCommandId, User currentUser)
        {
            MilitaryCommand militaryCommand = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.KOD_VVR as MilitaryCommandID, 
                                      a.IMEES as ShortName,
                                      a.NK as CommandNumber,
                                      a.KOD_MIR2 as MilitaryUnitID
                               FROM UKAZ_OWNER.VVR a
                               INNER JOIN UKAZ_OWNER.STRV b ON a.KOD_VVR = b.KOD_VVR
                               WHERE a.KOD_VVR = :MilitaryCommandId";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitaryCommandId", OracleType.Number).Value = militaryCommandId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    militaryCommand = ExtractMilitaryCommandFromDR(currentUser, dr);              
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryCommand;
        }

        //Get a list of all MilitaryCommands that belongs to a particular MilitaryUnit
        public static List<MilitaryCommand> GetMilitaryCommandsByMilitaryUnit(User currentUser, int militaryUnitId)
        {
            List<MilitaryCommand> militaryCommands = new List<MilitaryCommand>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.KOD_VVR as MilitaryCommandID,
                                      a.IMEES as ShortName,
                                      a.NK as CommandNumber,
                                      a.KOD_MIR2 as MilitaryUnitID
                               FROM UKAZ_OWNER.VVR a
                               INNER JOIN UKAZ_OWNER.STRV b ON a.KOD_VVR = b.KOD_VVR
                               WHERE a.KOD_MIR2 = :MilitaryUnitId
                               ORDER BY a.IMEES";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitaryUnitId", OracleType.Number).Value = militaryUnitId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    militaryCommands.Add(ExtractMilitaryCommandFromDR(currentUser, dr));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryCommands;
        }

        //Get a list of all MilitaryCommands that belongs to a particular MilitaryUnit AND its child military units
        public static List<MilitaryCommand> GetMilitaryCommandsByMilitaryUnitAndChildren(User currentUser, int militaryUnitId)
        {
            List<MilitaryCommand> militaryCommands = new List<MilitaryCommand>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.KOD_VVR as MilitaryCommandID,
                                      a.IMEES as ShortName,
                                      a.NK as CommandNumber,
                                      a.KOD_MIR2 as MilitaryUnitID
                               FROM UKAZ_OWNER.VVR a
                               INNER JOIN UKAZ_OWNER.STRV b ON a.KOD_VVR = b.KOD_VVR
                               INNER JOIN UKAZ_OWNER.MIR c ON a.KOD_MIR2 = c.KOD_MIR
                               WHERE a.KOD_MIR2 IN (SELECT * FROM TABLE(PMIS_ADM.CommonFunctions.GetMilitaryUnitAndChildren(:MilitaryUnitId)))
                               ORDER BY c.VPN, a.NK";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitaryUnitId", OracleType.Number).Value = militaryUnitId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    militaryCommands.Add(ExtractMilitaryCommandFromDR(currentUser, dr));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryCommands;
        }

        //Get a list of all Military Commands that belongs to a particular Military Department and Military Readiness (Reservists)
        public static List<MilitaryCommand> GetMilitaryCommandsByMilitaryReadinessForReservists(User currentUser, string militaryDepartmentIds, string militaryReadinessID)
        {
            List<MilitaryCommand> militaryCommands = new List<MilitaryCommand>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT DISTINCT b.KOD_VVR as MilitaryCommandID,
                                               b.IMEES as ShortName,
                                               b.NK as CommandNumber,
                                               b.KOD_MIR2 as MilitaryUnitID
                               FROM PMIS_RES.RequestsCommands a
                               INNER JOIN UKAZ_OWNER.VVR b ON a.MilitaryCommandID = b.KOD_VVR
                               LEFT OUTER JOIN PMIS_RES.RequestCommandPositions c ON a.RequestsCommandID = c.RequestsCommandID                               
                               LEFT OUTER JOIN PMIS_RES.RequestCommandPositionsMilDept g ON g.RequestCommandPositionID = c.RequestCommandPositionID
                               WHERE " + (string.IsNullOrEmpty(militaryReadinessID) ? "" : " a.milreadinessid = " + CommonFunctions.AvoidSQLInjForListOfIDs(militaryReadinessID) + " AND") + @" 
                                     a.EquipmentReservistsRequestID IN (SELECT DISTINCT a.EquipmentReservistsRequestID
                                                                        FROM PMIS_RES.EquipmentReservistsRequests a
                                                                        LEFT OUTER JOIN PMIS_RES.RequestsCommands b ON a.EquipmentReservistsRequestID = b.EquipmentReservistsRequestID
                                                                        LEFT OUTER JOIN PMIS_RES.RequestCommandPositions c ON b.RequestsCommandID = c.RequestsCommandID
                                                                        LEFT OUTER JOIN PMIS_RES.RequestCommandPositionsMilDept d ON c.RequestCommandPositionID = d.RequestCommandPositionID
                                                                        WHERE  (/*Ticket #128*/
                                                                                (d.MilitaryDepartmentID IS NULL AND a.MilitaryUnitID IS NULL) OR 
                                                                                d.MilitaryDepartmentID IN (" + currentUser.MilitaryDepartmentIDs + @") OR
                                                                                a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @")
                                                                               ) 
                                                                       )
                                    " + (string.IsNullOrEmpty(militaryDepartmentIds) ? "" : " AND g.MilitaryDepartmentID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(militaryDepartmentIds) + ") ") + @"
                               ORDER BY b.NK, b.KOD_VVR";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    militaryCommands.Add(ExtractMilitaryCommandFromDR(currentUser, dr));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryCommands;
        }

        //Get a list of all Military Commands that belongs to a particular Military Department and Military Readiness (Technics)
        public static List<MilitaryCommand> GetMilitaryCommandsByMilitaryReadinessForTechnics(User currentUser, string militaryDepartmentIds, string militaryReadinessID)
        {
            List<MilitaryCommand> militaryCommands = new List<MilitaryCommand>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT DISTINCT b.KOD_VVR as MilitaryCommandID,
                                               b.IMEES as ShortName,
                                               b.NK as CommandNumber,
                                               b.KOD_MIR2 as MilitaryUnitID
                               FROM PMIS_RES.TechnicsRequestCommands a
                               INNER JOIN UKAZ_OWNER.VVR b ON a.MilitaryCommandID = b.KOD_VVR
                               LEFT OUTER JOIN PMIS_RES.TechnicsRequestCmdPositions c ON a.TechRequestsCommandID = c.TechRequestsCommandID                               
                               LEFT OUTER JOIN PMIS_RES.TechRequestCmdPositionsMilDept g ON g.TechnicsRequestCmdPositionID = c.TechnicsRequestCmdPositionID
                               WHERE " + (string.IsNullOrEmpty(militaryReadinessID) ? "" : " a.MilReadinessID = " + CommonFunctions.AvoidSQLInjForListOfIDs(militaryReadinessID) + " AND") + @" 
                                     a.EquipmentTechnicsRequestID IN (SELECT DISTINCT a.EquipmentTechnicsRequestID
                                                                      FROM PMIS_RES.EquipmentTechnicsRequests a
                                                                      LEFT OUTER JOIN PMIS_RES.TechnicsRequestCommands b ON a.EquipmentTechnicsRequestID = b.EquipmentTechnicsRequestID
                                                                      LEFT OUTER JOIN PMIS_RES.TechnicsRequestCmdPositions c ON b.TechRequestsCommandID = c.TechRequestsCommandID
                                                                      LEFT OUTER JOIN PMIS_RES.TechRequestCmdPositionsMilDept d ON c.TechnicsRequestCmdPositionID = d.TechnicsRequestCmdPositionID
                                                                      WHERE  (/*Ticket #128*/
                                                                               (d.MilitaryDepartmentID IS NULL AND a.MilitaryUnitID IS NULL) OR 
                                                                               d.MilitaryDepartmentID IN (" + currentUser.MilitaryDepartmentIDs + @") OR
                                                                               a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @")
                                                                             )
                                                                     )
                                     " + (string.IsNullOrEmpty(militaryDepartmentIds) ? "" : " AND g.MilitaryDepartmentID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(militaryDepartmentIds) + ") ") + @"
                               ORDER BY b.NK, b.KOD_VVR
                              ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    militaryCommands.Add(ExtractMilitaryCommandFromDR(currentUser, dr));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryCommands;
        }

        //Get a list of all Military Commands that belongs to a particular Military Department (Reservists)
        public static List<MilitaryCommand> GetMilitaryCommandsByMilitaryDepartmentForReservists(User currentUser, string militaryDepartmentIds)
        {
            List<MilitaryCommand> militaryCommands = new List<MilitaryCommand>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT DISTINCT b.KOD_VVR as MilitaryCommandID,
                                               b.IMEES as ShortName,
                                               b.NK as CommandNumber,
                                               b.KOD_MIR2 as MilitaryUnitID
                               FROM PMIS_RES.RequestsCommands a
                               INNER JOIN UKAZ_OWNER.VVR b ON a.MilitaryCommandID = b.KOD_VVR
                               LEFT OUTER JOIN PMIS_RES.RequestCommandPositions c ON a.RequestsCommandID = c.RequestsCommandID                               
                               LEFT OUTER JOIN PMIS_RES.RequestCommandPositionsMilDept g ON g.RequestCommandPositionID = c.RequestCommandPositionID
                               WHERE a.EquipmentReservistsRequestID IN (SELECT DISTINCT a.EquipmentReservistsRequestID
                                                                        FROM PMIS_RES.EquipmentReservistsRequests a
                                                                        LEFT OUTER JOIN PMIS_RES.RequestsCommands b ON a.EquipmentReservistsRequestID = b.EquipmentReservistsRequestID
                                                                        LEFT OUTER JOIN PMIS_RES.RequestCommandPositions c ON b.RequestsCommandID = c.RequestsCommandID
                                                                        LEFT OUTER JOIN PMIS_RES.RequestCommandPositionsMilDept d ON c.RequestCommandPositionID = d.RequestCommandPositionID
                                                                        WHERE  (/*Ticket #128*/
                                                                                (d.MilitaryDepartmentID IS NULL AND a.MilitaryUnitID IS NULL) OR 
                                                                                d.MilitaryDepartmentID IN (" + currentUser.MilitaryDepartmentIDs + @") OR
                                                                                a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @")
                                                                               ) 
                                                                       )
                                    " + (string.IsNullOrEmpty(militaryDepartmentIds) ? "" : " AND g.MilitaryDepartmentID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(militaryDepartmentIds) + ") ") + @"
                               ORDER BY b.NK, b.KOD_VVR";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    militaryCommands.Add(ExtractMilitaryCommandFromDR(currentUser, dr));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryCommands;
        }

        //Get a list of all Military Commands that belongs to a particular Military Department (Reservists)
        public static List<MilitaryCommand> GetMilitaryCommandsByMilitaryDepartmentForTechnics(User currentUser, string militaryDepartmentIds)
        {
            List<MilitaryCommand> militaryCommands = new List<MilitaryCommand>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT DISTINCT b.KOD_VVR as MilitaryCommandID,
                                               b.IMEES as ShortName,
                                               b.NK as CommandNumber,
                                               b.KOD_MIR2 as MilitaryUnitID
                               FROM PMIS_RES.TechnicsRequestCommands a
                               INNER JOIN UKAZ_OWNER.VVR b ON a.MilitaryCommandID = b.KOD_VVR
                               LEFT OUTER JOIN PMIS_RES.TechnicsRequestCmdPositions c ON a.TechRequestsCommandID = c.TechRequestsCommandID                               
                               LEFT OUTER JOIN PMIS_RES.TechRequestCmdPositionsMilDept g ON g.TechnicsRequestCmdPositionID = c.TechnicsRequestCmdPositionID
                               WHERE a.EquipmentTechnicsRequestID IN (SELECT DISTINCT a.EquipmentTechnicsRequestID
                                                                      FROM PMIS_RES.EquipmentTechnicsRequests a
                                                                      LEFT OUTER JOIN PMIS_RES.TechnicsRequestCommands b ON a.EquipmentTechnicsRequestID = b.EquipmentTechnicsRequestID
                                                                      LEFT OUTER JOIN PMIS_RES.TechnicsRequestCmdPositions c ON b.TechRequestsCommandID = c.TechRequestsCommandID
                                                                      LEFT OUTER JOIN PMIS_RES.TechRequestCmdPositionsMilDept d ON c.TechnicsRequestCmdPositionID = d.TechnicsRequestCmdPositionID
                                                                      WHERE  (/*Ticket #128*/
                                                                               (d.MilitaryDepartmentID IS NULL AND a.MilitaryUnitID IS NULL) OR 
                                                                               d.MilitaryDepartmentID IN (" + currentUser.MilitaryDepartmentIDs + @") OR
                                                                               a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @")
                                                                             )
                                                                     )
                                     " + (string.IsNullOrEmpty(militaryDepartmentIds) ? "" : " AND g.MilitaryDepartmentID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(militaryDepartmentIds) + ") ") + @"
                               ORDER BY b.NK, b.KOD_VVR";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    militaryCommands.Add(ExtractMilitaryCommandFromDR(currentUser, dr));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return militaryCommands;
        }

        public static List<MilitaryCommand> GetAllMilitaryCommandsByIDs(User currentUser, string militaryCommandIds)
        {
            List<MilitaryCommand> listMilitaryCommands = new List<MilitaryCommand>();

            if (String.IsNullOrEmpty(militaryCommandIds))
                militaryCommandIds = "-1";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            try
            {
                conn.Open();
                string SQL = @"SELECT a.KOD_VVR as MilitaryCommandID, 
                                      a.IMEES as ShortName,
                                      a.NK as CommandNumber,
                                      a.KOD_MIR2 as MilitaryUnitID
                               FROM UKAZ_OWNER.VVR a
                               INNER JOIN UKAZ_OWNER.STRV b ON a.KOD_VVR = b.KOD_VVR
                               WHERE a.KOD_VVR IN (" + militaryCommandIds + @")";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["MilitaryCommandID"]))
                    {
                        MilitaryCommand militaryCommand = ExtractMilitaryCommandFromDR(currentUser, dr);
                        listMilitaryCommands.Add(militaryCommand);
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listMilitaryCommands;
        }
    }
}
