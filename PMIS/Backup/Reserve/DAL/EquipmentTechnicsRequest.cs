using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    public class EquipmentTechnicsRequest : BaseDbObject
    {
        private int equipmentTechnicsRequestId;
        private string requestNumber;
        private DateTime? requestDate;
        private EquipWithTechRequestsStatus equipWithTechRequestsStatus;
        private MilitaryUnit militaryUnit;
        private Administration administration;
        private List<TechnicsRequestCommand> technicsRequestCommands = null;
        private int count = 0;
        private int fulfilCount = 0;
        private int fulfilResCount = 0;

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

        public string RequestNumber
        {
            get
            {
                return requestNumber;
            }
            set
            {
                requestNumber = value;
            }
        }

        public DateTime? RequestDate
        {
            get
            {
                return requestDate;
            }
            set
            {
                requestDate = value;
            }
        }

        public EquipWithTechRequestsStatus EquipWithTechRequestsStatus
        {
            get
            {
                return equipWithTechRequestsStatus;
            }
            set
            {
                equipWithTechRequestsStatus = value;
            }
        }

        public MilitaryUnit MilitaryUnit
        {
            get
            {
                return militaryUnit;
            }
            set
            {
                militaryUnit = value;
            }
        }

        public Administration Administration
        {
            get
            {
                return administration;
            }
            set
            {
                administration = value;
            }
        }

        public List<TechnicsRequestCommand> TechnicsRequestCommands
        {
            get
            {
                //Lazy initialization
                if (technicsRequestCommands == null)
                {
                    technicsRequestCommands = TechnicsRequestCommandUtil.GetTechnicsRequestCommandsForRequest(CurrentUser, EquipmentTechnicsRequestId);
                }

                return technicsRequestCommands;
            }
            set
            {
                technicsRequestCommands = value;
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

        public int FulfilResCount
        {
            get
            {
                return fulfilResCount;
            }
            set
            {
                fulfilResCount = value;
            }
        }

        public bool CanDelete
        {
            get { return true; }

        }

        public EquipmentTechnicsRequest(User user)
            : base(user)
        {

        }
    }

    //This class represents all information about the filter, the order and the paging information on the screen
    public class EquipmentTechnicsRequestsFilter
    {
        string requestNum;
        DateTime? requestDateFrom;
        DateTime? requestDateTo;
        string commandNum;
        string militaryUnits;
        string administrations;
        string equipWithTechRequestsStatuses;
        string militaryDepartments;
        
        int orderBy;
        int pageIdx;

        public string RequestNum
        {
            get
            {
                return requestNum;
            }
            set
            {
                requestNum = value;
            }
        }

        public DateTime? RequestDateFrom
        {
            get
            {
                return requestDateFrom;
            }
            set
            {
                requestDateFrom = value;
            }
        }

        public DateTime? RequestDateTo
        {
            get
            {
                return requestDateTo;
            }
            set
            {
                requestDateTo = value;
            }
        }

        public string CommandNum
        {
            get
            {
                return commandNum;
            }
            set
            {
                commandNum = value;
            }
        }

        public string MilitaryUnits
        {
            get
            {
                return militaryUnits;
            }
            set
            {
                militaryUnits = value;
            }
        }

        public string Administrations
        {
            get
            {
                return administrations;
            }
            set
            {
                administrations = value;
            }
        }

        public string EquipWithTechRequestsStatuses
        {
            get
            {
                return equipWithTechRequestsStatuses;
            }
            set
            {
                equipWithTechRequestsStatuses = value;
            }
        }

        public string MilitaryDepartments
        {
            get
            {
                return militaryDepartments;
            }
            set
            {
                militaryDepartments = value;
            }
        }

       

        public int OrderBy
        {
            get
            {
                return orderBy;
            }
            set
            {
                orderBy = value;
            }
        }

        public int PageIdx
        {
            get
            {
                return pageIdx;
            }
            set
            {
                pageIdx = value;
            }
        }
    }

    public static class EquipmentTechnicsRequestUtil
    {
        //This method creates and returns a EquipmentTechnicsRequest object. It extracts the data from a DataReader.
        //This is done in this way to have a signle place of creation of this object, however, pass the particular DataReader
        //object from various queries for better performance instead of executing a new call to the DB here to get the details for a specific EquipWithTechRequestsStatusID, for example.
        public static EquipmentTechnicsRequest ExtractEquipmentTechnicsRequestFromDataReader(OracleDataReader dr, User currentUser)
        {
            int? equipmentTechnicsRequestId = null;

            if (DBCommon.IsInt(dr["EquipmentTechnicsRequestID"]))
                equipmentTechnicsRequestId = DBCommon.GetInt(dr["EquipmentTechnicsRequestID"]);

            string requestNumber = dr["RequestNumber"].ToString();
            DateTime? requestDate = null;
            if (dr["RequestDate"] is DateTime)
                requestDate = (DateTime)dr["RequestDate"];

            EquipmentTechnicsRequest equipmentTechnicsRequest = null;

            if (equipmentTechnicsRequestId.HasValue)
            {
                equipmentTechnicsRequest = new EquipmentTechnicsRequest(currentUser);
                equipmentTechnicsRequest.EquipmentTechnicsRequestId = equipmentTechnicsRequestId.Value;
                equipmentTechnicsRequest.RequestNumber = requestNumber;
                equipmentTechnicsRequest.RequestDate = requestDate;
                equipmentTechnicsRequest.Count = DBCommon.GetInt(dr["Count"]);
                equipmentTechnicsRequest.FulfilCount = DBCommon.GetInt(dr["FulfilCount"]);
                equipmentTechnicsRequest.FulfilResCount = DBCommon.GetInt(dr["FulfilResCount"]);

                BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, equipmentTechnicsRequest);
            }

            return equipmentTechnicsRequest;
        }

        //Get a particular object by its ID
        public static EquipmentTechnicsRequest GetEquipmentTechnicsRequest(int equipmentTechnicsRequestId, User currentUser)
        {
            EquipmentTechnicsRequest equipmentTechnicsRequest = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restrict the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_EQUIPTECHREQUESTS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where = " AND a.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.EquipmentTechnicsRequestID, a.RequestNumber, a.RequestDate,
                                      a.CreatedBy, a.CreatedDate, a.LastModifiedBy, a.LastModifiedDate,
                                      a.EquipWithTechRequestsStatusID, b.StatusKey, b.StatusName,
                                      a.MilitaryUnitID, NULL as ParentID, c.KOD_NMA as CityID, c.IMEES as ShortName, c.IMEED as LongName, c.VPN as VPN,
                                      a.AdministrationID, d.AdministrationName,
                                      PMIS_RES.RESFunctions.GetEquipTechReq_Count(a.EquipmentTechnicsRequestID, :FiltMilDept) as Count,
                                      PMIS_RES.RESFunctions.GetEquipTechReq_FulfilCount(a.EquipmentTechnicsRequestID, :FiltMilDept) as FulFilCount,
                                      PMIS_RES.RESFunctions.GetEquipTechReq_FulfilResCount(a.EquipmentTechnicsRequestID, :FiltMilDept) as FulfilResCount
                               FROM PMIS_RES.EquipmentTechnicsRequests a
                               LEFT OUTER JOIN PMIS_RES.EquipWithTechRequestsStatuses b ON a.EquipWithTechRequestsStatusID = b.EquipWithTechRequestsStatusID
                               LEFT OUTER JOIN UKAZ_OWNER.MIR c ON a.MilitaryUnitID = c.KOD_MIR
                               LEFT OUTER JOIN PMIS_ADM.Administrations d ON a.AdministrationID = d.AdministrationID
                               WHERE a.EquipmentTechnicsRequestID = :EquipmentTechnicsRequestID " + where;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("EquipmentTechnicsRequestID", OracleType.Number).Value = equipmentTechnicsRequestId;
                cmd.Parameters.Add("FiltMilDept", OracleType.VarChar).Value = currentUser.MilitaryDepartmentIDs;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    equipmentTechnicsRequest = EquipmentTechnicsRequestUtil.ExtractEquipmentTechnicsRequestFromDataReader(dr, currentUser);

                    if(DBCommon.IsInt(dr["EquipWithTechRequestsStatusID"]))
                        equipmentTechnicsRequest.EquipWithTechRequestsStatus = EquipWithTechRequestsStatusUtil.ExtractEquipWithTechRequestsStatusFromDataReader(dr, currentUser);

                    if (DBCommon.IsInt(dr["MilitaryUnitID"]))
                        equipmentTechnicsRequest.MilitaryUnit = MilitaryUnitUtil.ExtractMilitaryUnitFromDR(currentUser, dr);

                    if (DBCommon.IsInt(dr["AdministrationID"]))
                        equipmentTechnicsRequest.Administration = AdministrationUtil.ExtractAdministrationFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return equipmentTechnicsRequest;
        }

        //Get a list of all EquipmentTechnicsRequest records
        public static List<EquipmentTechnicsRequest> GetAllEquipmentTechnicsRequest(EquipmentTechnicsRequestsFilter filt, int rowsPerPage, User currentUser)
        {
            List<EquipmentTechnicsRequest> equipmentTechnicsRequests = new List<EquipmentTechnicsRequest>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                bool join_TechnicsRequestCommands = false;
                bool join_TechnicsRequestCmdPositions = false;
                bool join_TechRequestCmdPositionsMilDept = false;
                bool join_EquipWithTechRequestsStatuses = false;
                bool join_MIR = false;
                bool join_Administrations = false;
                bool join_VVR = false;

                string where = "";

                //Restrict the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_EQUIPTECHREQUESTS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.CreatedBy = " + currentUser.UserId.ToString();
                }

                //Ticket #128: "С роля „Военни формирования“ да си виждат командите спрямо схемата за мобилизация"
                //Добавяме условие потребителят да има достъп до ВО-то ИЛИ до поделението. Преди това потребителите от военните формирования не виждаха заявките от тях, защото те нямат достъп до ВО
                where += (where == "" ? "" : " AND ") +
                    @" (
                        (g.MilitaryDepartmentID IS NULL AND a.MilitaryUnitID IS NULL) OR 
                        g.MilitaryDepartmentID IN (" + currentUser.MilitaryDepartmentIDs + @") OR
                        a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @")
                       ) ";

                if (!String.IsNullOrEmpty(filt.RequestNum))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.RequestNumber LIKE '%" + filt.RequestNum.Replace("'", "''") + @"%' ";
                }

                if (filt.RequestDateFrom.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.RequestDate >= " + DBCommon.DateToDBCode(filt.RequestDateFrom.Value) + " ";
                }

                if (filt.RequestDateTo.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.RequestDate < " + DBCommon.DateToDBCode(filt.RequestDateTo.Value.AddDays(1)) + " ";
                }
                
                if (!String.IsNullOrEmpty(filt.CommandNum))
                {
                    where += (where == "" ? "" : " AND ") +
                             " h.NK LIKE '%" + filt.CommandNum.Replace("'", "''") + @"%' ";

                    join_VVR = true;
                }

                if (!String.IsNullOrEmpty(filt.MilitaryUnits))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MilitaryUnitID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filt.MilitaryUnits) + ") ";
                }

                if (!String.IsNullOrEmpty(filt.Administrations))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.AdministrationID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filt.Administrations) + ") ";
                }

                if (!String.IsNullOrEmpty(filt.EquipWithTechRequestsStatuses))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.EquipWithTechRequestsStatusID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filt.EquipWithTechRequestsStatuses) + ") ";
                }

                string filtMilDept = "";

                if (!string.IsNullOrEmpty(filt.MilitaryDepartments))
                {
                    where += (where == "" ? "" : " AND ") +
                             " g.MilitaryDepartmentID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filt.MilitaryDepartments) + ") ";

                    filtMilDept = CommonFunctions.AvoidSQLInjForListOfIDs(filt.MilitaryDepartments);
                }
                else
                {
                    filtMilDept = currentUser.MilitaryDepartmentIDs;
                }

                join_TechRequestCmdPositionsMilDept = true;

                where = (where == "" ? "" : " WHERE ") + where;

                string pageWhere = "";

                if (filt.PageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE RowNumber BETWEEN (" + filt.PageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + filt.PageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

                string orderBySQL = "";
                string orderByDir = "ASC";

                if (filt.OrderBy > 100)
                {
                    filt.OrderBy -= 100;
                    orderByDir = "DESC";
                }

                switch (filt.OrderBy)
                {
                    case 1:
                        orderBySQL = "a.RequestNumber";
                        break;
                    case 2:
                        orderBySQL = "a.RequestDate";
                        break;
                    case 3:
                        orderBySQL = "c.IMEES";
                        join_MIR = true;
                        break;
                    case 4:
                        orderBySQL = "d.AdministrationName";
                        join_Administrations = true;
                        break;
                    case 5:
                        orderBySQL = "b.StatusName";
                        join_EquipWithTechRequestsStatuses = true;
                        break;
                    default:
                        orderBySQL = "a.RequestNumber";
                        break;
                }

                if (join_TechRequestCmdPositionsMilDept)
                {
                    join_TechnicsRequestCmdPositions = true;
                }

                if (join_TechnicsRequestCmdPositions || join_VVR)
                {
                    join_TechnicsRequestCommands = true;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT tmp.*,
                                      a.EquipmentTechnicsRequestID, a.RequestNumber, a.RequestDate,
                                      a.CreatedBy, a.CreatedDate, a.LastModifiedBy, a.LastModifiedDate,
                                      a.EquipWithTechRequestsStatusID, b.StatusKey, b.StatusName,
                                      a.MilitaryUnitID, NULL as ParentID, c.KOD_NMA as CityID, c.IMEES as ShortName, c.IMEED as LongName, c.VPN as VPN,
                                      a.AdministrationID, d.AdministrationName,
                                      PMIS_RES.RESFunctions.GetEquipTechReq_Count(a.EquipmentTechnicsRequestID, :FiltMilDept) as Count,
                                      PMIS_RES.RESFunctions.GetEquipTechReq_FulfilCount(a.EquipmentTechnicsRequestID, :FiltMilDept) as FulFilCount,
                                      PMIS_RES.RESFunctions.GetEquipTechReq_FulfilResCount(a.EquipmentTechnicsRequestID, :FiltMilDept) as FulfilResCount
                               FROM (       SELECT DISTINCT a.EquipmentTechnicsRequestID,
                                                   DENSE_RANK() OVER (ORDER BY " + orderBySQL + @", a.EquipmentTechnicsRequestID) as RowNumber 
                                            FROM PMIS_RES.EquipmentTechnicsRequests a
       " + (join_TechnicsRequestCommands ? "LEFT OUTER JOIN PMIS_RES.TechnicsRequestCommands e ON a.EquipmentTechnicsRequestID = e.EquipmentTechnicsRequestID" : "") + @"
   " + (join_TechnicsRequestCmdPositions ? "LEFT OUTER JOIN PMIS_RES.TechnicsRequestCmdPositions f ON e.TechRequestsCommandID = f.TechRequestsCommandID" : "") + @"
" + (join_TechRequestCmdPositionsMilDept ? "LEFT OUTER JOIN PMIS_RES.TechRequestCmdPositionsMilDept g ON f.TechnicsRequestCmdPositionID = g.TechnicsRequestCmdPositionID" : "") + @"

 " + (join_EquipWithTechRequestsStatuses ? "LEFT OUTER JOIN PMIS_RES.EquipWithTechRequestsStatuses b ON a.EquipWithTechRequestsStatusID = b.EquipWithTechRequestsStatusID" : "") + @"
                           " + (join_MIR ? "LEFT OUTER JOIN UKAZ_OWNER.MIR c ON a.MilitaryUnitID = c.KOD_MIR" : "") + @"
               " + (join_Administrations ? "LEFT OUTER JOIN PMIS_ADM.Administrations d ON a.AdministrationID = d.AdministrationID" : "") + @"

                           " + (join_VVR ? "LEFT OUTER JOIN UKAZ_OWNER.VVR h ON e.MilitaryCommandID = h.KOD_VVR" : "") + @"
                                           " + where + @"    
                                    ) tmp
                               INNER JOIN PMIS_RES.EquipmentTechnicsRequests a ON a.EquipmentTechnicsRequestID = tmp.EquipmentTechnicsRequestID
                                
                               LEFT OUTER JOIN PMIS_RES.EquipWithTechRequestsStatuses b ON a.EquipWithTechRequestsStatusID = b.EquipWithTechRequestsStatusID
                               LEFT OUTER JOIN UKAZ_OWNER.MIR c ON a.MilitaryUnitID = c.KOD_MIR
                               LEFT OUTER JOIN PMIS_ADM.Administrations d ON a.AdministrationID = d.AdministrationID
                               " + pageWhere + @"
                               ORDER BY tmp.RowNumber";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("FiltMilDept", OracleType.VarChar).Value = filtMilDept;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    EquipmentTechnicsRequest equipmentTechnicsRequest = EquipmentTechnicsRequestUtil.ExtractEquipmentTechnicsRequestFromDataReader(dr, currentUser);

                    if (DBCommon.IsInt(dr["EquipWithTechRequestsStatusID"]))
                        equipmentTechnicsRequest.EquipWithTechRequestsStatus = EquipWithTechRequestsStatusUtil.ExtractEquipWithTechRequestsStatusFromDataReader(dr, currentUser);

                    if (DBCommon.IsInt(dr["MilitaryUnitID"]))
                        equipmentTechnicsRequest.MilitaryUnit = MilitaryUnitUtil.ExtractMilitaryUnitFromDR(currentUser, dr);

                    if (DBCommon.IsInt(dr["AdministrationID"]))
                        equipmentTechnicsRequest.Administration = AdministrationUtil.ExtractAdministrationFromDataReader(dr, currentUser);

                    equipmentTechnicsRequests.Add(equipmentTechnicsRequest);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return equipmentTechnicsRequests;
        }

        //Get a list of all EquipmentTechnicsRequest records for fulfilment (filtered to military departments of the user)
        public static List<EquipmentTechnicsRequest> GetAllEquipmentTechnicsRequestForFulfilment(EquipmentTechnicsRequestsFilter filt, int rowsPerPage, User currentUser)
        {
            List<EquipmentTechnicsRequest> equipmentTechnicsRequests = new List<EquipmentTechnicsRequest>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                bool join_TechnicsRequestCommands = false;
                bool join_TechnicsRequestCmdPositions = false;
                bool join_TechRequestCmdPositionsMilDept = false;
                bool join_EquipWithTechRequestsStatuses = false;
                bool join_MIR = false;
                bool join_Administrations = false;
                bool join_VVR = false;

                string where = "";

                //Restrict the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_EQUIPTECHREQUESTS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.CreatedBy = " + currentUser.UserId.ToString();
                }

                // commented, because it has to be filtered only by military department
                //if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                //{
                //    where += (where == "" ? "" : " AND ") +
                //             @" (a.MilitaryUnitID IS NULL OR a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @"))
                //            ";
                //}


                if (!String.IsNullOrEmpty(filt.RequestNum))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.RequestNumber LIKE '%" + filt.RequestNum.Replace("'", "''") + @"%' ";
                }

                if (filt.RequestDateFrom.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.RequestDate >= " + DBCommon.DateToDBCode(filt.RequestDateFrom.Value) + " ";
                }

                if (filt.RequestDateTo.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.RequestDate < " + DBCommon.DateToDBCode(filt.RequestDateTo.Value.AddDays(1)) + " ";
                }

                if (!String.IsNullOrEmpty(filt.CommandNum))
                {
                    where += (where == "" ? "" : " AND ") +
                             " h.NK LIKE '%" + filt.CommandNum.Replace("'", "''") + @"%' ";

                    join_VVR = true;
                }

                if (!String.IsNullOrEmpty(filt.MilitaryUnits))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MilitaryUnitID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filt.MilitaryUnits) + ") ";
                }

                if (!String.IsNullOrEmpty(filt.Administrations))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.AdministrationID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filt.Administrations) + ") ";
                }

                if (!String.IsNullOrEmpty(filt.EquipWithTechRequestsStatuses))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.EquipWithTechRequestsStatusID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filt.EquipWithTechRequestsStatuses) + ") ";
                }

                string filtMilDept = "";
                if (!string.IsNullOrEmpty(filt.MilitaryDepartments))
                {
                    where += (where == "" ? "" : " AND ") +
                             " g.MilitaryDepartmentID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filt.MilitaryDepartments) + ") ";

                    filtMilDept = CommonFunctions.AvoidSQLInjForListOfIDs(filt.MilitaryDepartments);
                }
                else
                {
                    where += (where == "" ? "" : " AND ") +
                             " g.MilitaryDepartmentID IN (" + currentUser.MilitaryDepartmentIDs + ") ";

                    filtMilDept = currentUser.MilitaryDepartmentIDs;
                }

                where += (where == "" ? "" : " AND ") +
                             " NVL(g.Count, 0) > 0";

                join_TechRequestCmdPositionsMilDept = true;


                where = (where == "" ? "" : " WHERE ") + where;

                string pageWhere = "";

                if (filt.PageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE RowNumber BETWEEN (" + filt.PageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + filt.PageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

                string orderBySQL = "";
                string orderByDir = "ASC";

                if (filt.OrderBy > 100)
                {
                    filt.OrderBy -= 100;
                    orderByDir = "DESC";
                }

                switch (filt.OrderBy)
                {
                    case 1:
                        orderBySQL = "a.RequestNumber";
                        break;
                    case 2:
                        orderBySQL = "a.RequestDate";
                        break;
                    case 3:
                        orderBySQL = "c.IMEES";
                        join_MIR = true;
                        break;
                    case 4:
                        orderBySQL = "d.AdministrationName";
                        join_Administrations = true;
                        break;
                    case 5:
                        orderBySQL = "b.StatusName";
                        join_EquipWithTechRequestsStatuses = true;
                        break;
                    case 6:
                        orderBySQL = @" CASE WHEN PMIS_RES.RESFunctions.GetEquipTechReq_Count(a.EquipmentTechnicsRequestID, :FiltMilDept) = 0 
                                             THEN 0 
                                             ELSE PMIS_RES.RESFunctions.GetEquipTechReq_FulfilCount(a.EquipmentTechnicsRequestID, :FiltMilDept) / 
                                                  PMIS_RES.RESFunctions.GetEquipTechReq_Count(a.EquipmentTechnicsRequestID, :FiltMilDept)
                                        END ";
                        break;
                    default:
                        orderBySQL = "a.RequestNumber";
                        break;
                }

                if (join_TechRequestCmdPositionsMilDept)
                {
                    join_TechnicsRequestCmdPositions = true;
                }

                if (join_TechnicsRequestCmdPositions || join_VVR)
                {
                    join_TechnicsRequestCommands = true;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT tmp.*,
                                      a.EquipmentTechnicsRequestID, a.RequestNumber, a.RequestDate,
                                      a.CreatedBy, a.CreatedDate, a.LastModifiedBy, a.LastModifiedDate,
                                      a.EquipWithTechRequestsStatusID, b.StatusKey, b.StatusName,
                                      a.MilitaryUnitID, NULL as ParentID, c.KOD_NMA as CityID, c.IMEES as ShortName, c.IMEED as LongName, c.VPN as VPN,
                                      a.AdministrationID, d.AdministrationName,
                                      PMIS_RES.RESFunctions.GetEquipTechReq_Count(a.EquipmentTechnicsRequestID, :FiltMilDept) as Count,
                                      PMIS_RES.RESFunctions.GetEquipTechReq_FulfilCount(a.EquipmentTechnicsRequestID, :FiltMilDept) as FulFilCount,
                                      PMIS_RES.RESFunctions.GetEquipTechReq_FulfilResCount(a.EquipmentTechnicsRequestID, :FiltMilDept) as FulfilResCount
                               FROM (       SELECT DISTINCT a.EquipmentTechnicsRequestID, 
                                                   DENSE_RANK() OVER (ORDER BY " + orderBySQL + @", a.EquipmentTechnicsRequestID) as RowNumber 
                                            FROM PMIS_RES.EquipmentTechnicsRequests a
       " + (join_TechnicsRequestCommands ? "INNER JOIN PMIS_RES.TechnicsRequestCommands e ON a.EquipmentTechnicsRequestID = e.EquipmentTechnicsRequestID" : "") + @"
   " + (join_TechnicsRequestCmdPositions ? "INNER JOIN PMIS_RES.TechnicsRequestCmdPositions f ON e.TechRequestsCommandID = f.TechRequestsCommandID" : "") + @"
" + (join_TechRequestCmdPositionsMilDept ? "INNER JOIN PMIS_RES.TechRequestCmdPositionsMilDept g ON f.TechnicsRequestCmdPositionID = g.TechnicsRequestCmdPositionID" : "") + @"

 " + (join_EquipWithTechRequestsStatuses ? "LEFT OUTER JOIN PMIS_RES.EquipWithTechRequestsStatuses b ON a.EquipWithTechRequestsStatusID = b.EquipWithTechRequestsStatusID" : "") + @"
                           " + (join_MIR ? "LEFT OUTER JOIN UKAZ_OWNER.MIR c ON a.MilitaryUnitID = c.KOD_MIR" : "") + @"
               " + (join_Administrations ? "LEFT OUTER JOIN PMIS_ADM.Administrations d ON a.AdministrationID = d.AdministrationID" : "") + @"

                           " + (join_VVR ? "LEFT OUTER JOIN UKAZ_OWNER.VVR h ON e.MilitaryCommandID = h.KOD_VVR" : "") + @"
                                           " + where + @"    
                                    ) tmp
                               INNER JOIN PMIS_RES.EquipmentTechnicsRequests a ON a.EquipmentTechnicsRequestID = tmp.EquipmentTechnicsRequestID

                               LEFT OUTER JOIN PMIS_RES.EquipWithTechRequestsStatuses b ON a.EquipWithTechRequestsStatusID = b.EquipWithTechRequestsStatusID
                               LEFT OUTER JOIN UKAZ_OWNER.MIR c ON a.MilitaryUnitID = c.KOD_MIR
                               LEFT OUTER JOIN PMIS_ADM.Administrations d ON a.AdministrationID = d.AdministrationID
                               " + pageWhere + @"
                               ORDER BY tmp.RowNumber";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("FiltMilDept", OracleType.VarChar).Value = filtMilDept;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    EquipmentTechnicsRequest equipmentTechnicsRequest = EquipmentTechnicsRequestUtil.ExtractEquipmentTechnicsRequestFromDataReader(dr, currentUser);

                    if (DBCommon.IsInt(dr["EquipWithTechRequestsStatusID"]))
                        equipmentTechnicsRequest.EquipWithTechRequestsStatus = EquipWithTechRequestsStatusUtil.ExtractEquipWithTechRequestsStatusFromDataReader(dr, currentUser);

                    if (DBCommon.IsInt(dr["MilitaryUnitID"]))
                        equipmentTechnicsRequest.MilitaryUnit = MilitaryUnitUtil.ExtractMilitaryUnitFromDR(currentUser, dr);

                    if (DBCommon.IsInt(dr["AdministrationID"]))
                        equipmentTechnicsRequest.Administration = AdministrationUtil.ExtractAdministrationFromDataReader(dr, currentUser);

                    equipmentTechnicsRequests.Add(equipmentTechnicsRequest);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return equipmentTechnicsRequests;
        }

        public static int GetAllEquipmentTechnicsRequestCount(EquipmentTechnicsRequestsFilter filt, User currentUser)
        {
            int equipmentTechnicsRequestsCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                bool join_TechnicsRequestCommands = false;
                bool join_TechnicsRequestCmdPositions = false;
                bool join_TechRequestCmdPositionsMilDept = false;
                bool join_VVR = false;

                string where = "";

                //Restrict the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_EQUIPTECHREQUESTS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.CreatedBy = " + currentUser.UserId.ToString();
                }

                //Ticket #128: "С роля „Военни формирования“ да си виждат командите спрямо схемата за мобилизация"
                //Добавяме условие потребителят да има достъп до ВО-то ИЛИ до поделението. Преди това потребителите от военните формирования не виждаха заявките от тях, защото те нямат достъп до ВО
                where += (where == "" ? "" : " AND ") +
                    @" (
                        (d.MilitaryDepartmentID IS NULL AND a.MilitaryUnitID IS NULL) OR 
                        d.MilitaryDepartmentID IN (" + currentUser.MilitaryDepartmentIDs + @") OR
                        a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @")
                       ) ";

                if (!String.IsNullOrEmpty(filt.RequestNum))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.RequestNumber LIKE '%" + filt.RequestNum.Replace("'", "''") + @"%' ";
                }

                if (filt.RequestDateFrom.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.RequestDate >= " + DBCommon.DateToDBCode(filt.RequestDateFrom.Value) + " ";
                }

                if (filt.RequestDateTo.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.RequestDate < " + DBCommon.DateToDBCode(filt.RequestDateTo.Value.AddDays(1)) + " ";
                }

                if (!String.IsNullOrEmpty(filt.CommandNum))
                {
                    where += (where == "" ? "" : " AND ") +
                             " h.NK LIKE '%" + filt.CommandNum.Replace("'", "''") + @"%' ";

                    join_VVR = true;
                }

                if (!String.IsNullOrEmpty(filt.MilitaryUnits))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MilitaryUnitID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filt.MilitaryUnits) + ") ";
                }

                if (!String.IsNullOrEmpty(filt.Administrations))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.AdministrationID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filt.Administrations) + ") ";
                }

                if (!String.IsNullOrEmpty(filt.EquipWithTechRequestsStatuses))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.EquipWithTechRequestsStatusID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filt.EquipWithTechRequestsStatuses) + ") ";
                }

                if (!string.IsNullOrEmpty(filt.MilitaryDepartments))
                    where += (where == "" ? "" : " AND ") +
                             " d.MilitaryDepartmentID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filt.MilitaryDepartments) + ") ";

                join_TechRequestCmdPositionsMilDept = true;

                if (join_TechRequestCmdPositionsMilDept)
                {
                    join_TechnicsRequestCmdPositions = true;
                }

                if (join_TechnicsRequestCmdPositions || join_VVR)
                {
                    join_TechnicsRequestCommands = true;
                }

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @"             SELECT COUNT(DISTINCT a.EquipmentTechnicsRequestID) as Cnt
                                            FROM PMIS_RES.EquipmentTechnicsRequests a
       " + (join_TechnicsRequestCommands ? "LEFT OUTER JOIN PMIS_RES.TechnicsRequestCommands b ON a.EquipmentTechnicsRequestID = b.EquipmentTechnicsRequestID" : "") + @"
   " + (join_TechnicsRequestCmdPositions ? "LEFT OUTER JOIN PMIS_RES.TechnicsRequestCmdPositions c ON b.TechRequestsCommandID = c.TechRequestsCommandID" : "") + @"
" + (join_TechRequestCmdPositionsMilDept ? "LEFT OUTER JOIN PMIS_RES.TechRequestCmdPositionsMilDept d ON c.TechnicsRequestCmdPositionID = d.TechnicsRequestCmdPositionID" : "") + @"
                           " + (join_VVR ? "LEFT OUTER JOIN UKAZ_OWNER.VVR h ON b.MilitaryCommandID = h.KOD_VVR" : "") + @"
                                           " + where + @"
                              ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        equipmentTechnicsRequestsCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return equipmentTechnicsRequestsCnt;
        }

        public static int GetAllEquipmentTechnicsRequestForFulfilmentCount(EquipmentTechnicsRequestsFilter filt, User currentUser)
        {
            int equipmentTechnicsRequestsCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                bool join_TechnicsRequestCommands = false;
                bool join_TechnicsRequestCmdPositions = false;
                bool join_TechRequestCmdPositionsMilDept = false;
                bool join_VVR = false;

                string where = "";

                //Restrict the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_EQUIPTECHREQUESTS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.CreatedBy = " + currentUser.UserId.ToString();
                }

                // commented, because it has to be filtered only by military department
                //if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                //{
                //    where += (where == "" ? "" : " AND ") +
                //             @" (a.MilitaryUnitID IS NULL OR a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @"))
                //            ";
                //}


                if (!String.IsNullOrEmpty(filt.RequestNum))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.RequestNumber LIKE '%" + filt.RequestNum.Replace("'", "''") + @"%' ";
                }

                if (filt.RequestDateFrom.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.RequestDate >= " + DBCommon.DateToDBCode(filt.RequestDateFrom.Value) + " ";
                }

                if (filt.RequestDateTo.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.RequestDate < " + DBCommon.DateToDBCode(filt.RequestDateTo.Value.AddDays(1)) + " ";
                }

                if (!String.IsNullOrEmpty(filt.CommandNum))
                {
                    where += (where == "" ? "" : " AND ") +
                             " h.NK LIKE '%" + filt.CommandNum.Replace("'", "''") + @"%' ";

                    join_VVR = true;
                }

                if (!String.IsNullOrEmpty(filt.MilitaryUnits))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MilitaryUnitID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filt.MilitaryUnits) + ") ";
                }

                if (!String.IsNullOrEmpty(filt.Administrations))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.AdministrationID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filt.Administrations) + ") ";
                }

                if (!String.IsNullOrEmpty(filt.EquipWithTechRequestsStatuses))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.EquipWithTechRequestsStatusID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filt.EquipWithTechRequestsStatuses) + ") ";
                }

                if (!string.IsNullOrEmpty(filt.MilitaryDepartments))
                    where += (where == "" ? "" : " AND ") +
                             " d.MilitaryDepartmentID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filt.MilitaryDepartments) + ") ";
                else
                    where += (where == "" ? "" : " AND ") +
                             " d.MilitaryDepartmentID IN (" + currentUser.MilitaryDepartmentIDs + ") ";

                where += (where == "" ? "" : " AND ") +
                             " NVL(d.Count, 0) > 0";

                join_TechRequestCmdPositionsMilDept = true;

                if (join_TechRequestCmdPositionsMilDept)
                {
                    join_TechnicsRequestCmdPositions = true;
                }

                if (join_TechnicsRequestCmdPositions || join_VVR)
                {
                    join_TechnicsRequestCommands = true;
                }

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @"             SELECT COUNT(DISTINCT a.EquipmentTechnicsRequestID) as Cnt
                                            FROM PMIS_RES.EquipmentTechnicsRequests a
       " + (join_TechnicsRequestCommands ? "INNER JOIN PMIS_RES.TechnicsRequestCommands b ON a.EquipmentTechnicsRequestID = b.EquipmentTechnicsRequestID" : "") + @"
   " + (join_TechnicsRequestCmdPositions ? "INNER JOIN PMIS_RES.TechnicsRequestCmdPositions c ON b.TechRequestsCommandID = c.TechRequestsCommandID" : "") + @"
" + (join_TechRequestCmdPositionsMilDept ? "INNER JOIN PMIS_RES.TechRequestCmdPositionsMilDept d ON c.TechnicsRequestCmdPositionID = d.TechnicsRequestCmdPositionID" : "") + @"
                           " + (join_VVR ? "LEFT OUTER JOIN UKAZ_OWNER.VVR h ON b.MilitaryCommandID = h.KOD_VVR" : "") + @"
                                           " + where + @"
                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        equipmentTechnicsRequestsCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return equipmentTechnicsRequestsCnt;
        }

        //Save a particular object into the DB
        public static bool SaveEquipmentTechnicsRequest(EquipmentTechnicsRequest equipmentTechnicsRequest, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            string logDescription = "";
            logDescription += "Заявка №: " + equipmentTechnicsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(equipmentTechnicsRequest.RequestDate);

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                //New record
                if (equipmentTechnicsRequest.EquipmentTechnicsRequestId == 0)
                {
                    SQL += @"INSERT INTO PMIS_RES.EquipmentTechnicsRequests (RequestNumber, RequestDate, 
                                EquipWithTechRequestsStatusID, MilitaryUnitID, AdministrationID, 
                                CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate)
                            VALUES (:RequestNumber, :RequestDate, 
                                :EquipWithTechRequestsStatusID, :MilitaryUnitID, :AdministrationID, 
                                :CreatedBy, :CreatedDate, :LastModifiedBy, :LastModifiedDate);

                            SELECT PMIS_RES.EquipTechRequests_ID_SEQ.currval INTO :EquipmentTechnicsRequestID FROM dual;

                            ";

                    changeEvent = new ChangeEvent("RES_EquipTechRequests_Add", logDescription, equipmentTechnicsRequest.MilitaryUnit, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_RequestNum", "", equipmentTechnicsRequest.RequestNumber, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_RequestDate", "", CommonFunctions.FormatDate(equipmentTechnicsRequest.RequestDate), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_Status", "", equipmentTechnicsRequest.EquipWithTechRequestsStatus != null && equipmentTechnicsRequest.EquipWithTechRequestsStatus.StatusName != null ? equipmentTechnicsRequest.EquipWithTechRequestsStatus.StatusName : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_MilitaryUnit", "", equipmentTechnicsRequest.MilitaryUnit != null && equipmentTechnicsRequest.MilitaryUnit.DisplayTextForSelection != null ? equipmentTechnicsRequest.MilitaryUnit.DisplayTextForSelection : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_Administration", "", equipmentTechnicsRequest.Administration != null && equipmentTechnicsRequest.Administration.AdministrationName != null ? equipmentTechnicsRequest.Administration.AdministrationName : "", currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_RES.EquipmentTechnicsRequests SET
                               RequestNumber = :RequestNumber,
                               RequestDate = :RequestDate, 
                               EquipWithTechRequestsStatusID = :EquipWithTechRequestsStatusID, 
                               MilitaryUnitID = :MilitaryUnitID, 
                               AdministrationID = :AdministrationID, 
                               LastModifiedBy = CASE WHEN :AnyActualChanges = 1 
                                                     THEN :LastModifiedBy
                                                     ELSE LastModifiedBy
                                                END, 
                               LastModifiedDate = CASE WHEN :AnyActualChanges = 1 
                                                       THEN :LastModifiedDate
                                                       ELSE LastModifiedDate
                                                  END

                             WHERE EquipmentTechnicsRequestID = :EquipmentTechnicsRequestID ;                       

                            ";

                    changeEvent = new ChangeEvent("RES_EquipTechRequests_Edit", logDescription, null, null, currentUser);

                    EquipmentTechnicsRequest oldEquipmentTechnicsRequest = EquipmentTechnicsRequestUtil.GetEquipmentTechnicsRequest(equipmentTechnicsRequest.EquipmentTechnicsRequestId, currentUser);

                    if (oldEquipmentTechnicsRequest.RequestNumber.Trim() != equipmentTechnicsRequest.RequestNumber.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_RequestNum", oldEquipmentTechnicsRequest.RequestNumber, equipmentTechnicsRequest.RequestNumber, currentUser));

                    if (!CommonFunctions.IsEqual(oldEquipmentTechnicsRequest.RequestDate, equipmentTechnicsRequest.RequestDate))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_RequestDate", CommonFunctions.FormatDate(oldEquipmentTechnicsRequest.RequestDate), CommonFunctions.FormatDate(equipmentTechnicsRequest.RequestDate), currentUser));

                    if ((oldEquipmentTechnicsRequest.EquipWithTechRequestsStatus != null && oldEquipmentTechnicsRequest.EquipWithTechRequestsStatus.StatusName != null ? oldEquipmentTechnicsRequest.EquipWithTechRequestsStatus.StatusName : "") !=
                        (equipmentTechnicsRequest.EquipWithTechRequestsStatus != null && equipmentTechnicsRequest.EquipWithTechRequestsStatus.StatusName != null ? equipmentTechnicsRequest.EquipWithTechRequestsStatus.StatusName : ""))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_Status", 
                            oldEquipmentTechnicsRequest.EquipWithTechRequestsStatus != null && oldEquipmentTechnicsRequest.EquipWithTechRequestsStatus.StatusName != null ? oldEquipmentTechnicsRequest.EquipWithTechRequestsStatus.StatusName : "",
                            equipmentTechnicsRequest.EquipWithTechRequestsStatus != null && equipmentTechnicsRequest.EquipWithTechRequestsStatus.StatusName != null ? equipmentTechnicsRequest.EquipWithTechRequestsStatus.StatusName : "", 
                            currentUser));

                    if ((oldEquipmentTechnicsRequest.MilitaryUnit != null && oldEquipmentTechnicsRequest.MilitaryUnit.DisplayTextForSelection != null ? oldEquipmentTechnicsRequest.MilitaryUnit.DisplayTextForSelection : "") !=
                        (equipmentTechnicsRequest.MilitaryUnit != null && equipmentTechnicsRequest.MilitaryUnit.DisplayTextForSelection != null ? equipmentTechnicsRequest.MilitaryUnit.DisplayTextForSelection : ""))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_MilitaryUnit",
                            oldEquipmentTechnicsRequest.MilitaryUnit != null && oldEquipmentTechnicsRequest.MilitaryUnit.DisplayTextForSelection != null ? oldEquipmentTechnicsRequest.MilitaryUnit.DisplayTextForSelection : "",
                            equipmentTechnicsRequest.MilitaryUnit != null && equipmentTechnicsRequest.MilitaryUnit.DisplayTextForSelection != null ? equipmentTechnicsRequest.MilitaryUnit.DisplayTextForSelection : "",
                            currentUser));

                    if ((oldEquipmentTechnicsRequest.Administration != null && oldEquipmentTechnicsRequest.Administration.AdministrationName != null ? oldEquipmentTechnicsRequest.Administration.AdministrationName : "") !=
                        (equipmentTechnicsRequest.Administration != null && equipmentTechnicsRequest.Administration.AdministrationName != null ? equipmentTechnicsRequest.Administration.AdministrationName : ""))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_Administration",
                            oldEquipmentTechnicsRequest.Administration != null && oldEquipmentTechnicsRequest.Administration.AdministrationName != null ? oldEquipmentTechnicsRequest.Administration.AdministrationName : "",
                            equipmentTechnicsRequest.Administration != null && equipmentTechnicsRequest.Administration.AdministrationName != null ? equipmentTechnicsRequest.Administration.AdministrationName : "",
                            currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramEquipmentTechnicsRequestID = new OracleParameter();
                paramEquipmentTechnicsRequestID.ParameterName = "EquipmentTechnicsRequestID";
                paramEquipmentTechnicsRequestID.OracleType = OracleType.Number;

                if (equipmentTechnicsRequest.EquipmentTechnicsRequestId != 0)
                {
                    paramEquipmentTechnicsRequestID.Direction = ParameterDirection.Input;
                    paramEquipmentTechnicsRequestID.Value = equipmentTechnicsRequest.EquipmentTechnicsRequestId;
                }
                else
                {
                    paramEquipmentTechnicsRequestID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramEquipmentTechnicsRequestID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "RequestNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(equipmentTechnicsRequest.RequestNumber))
                    param.Value = equipmentTechnicsRequest.RequestNumber;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "RequestDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (equipmentTechnicsRequest.RequestDate.HasValue)
                    param.Value = equipmentTechnicsRequest.RequestDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "EquipWithTechRequestsStatusID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (equipmentTechnicsRequest.EquipWithTechRequestsStatus != null)
                    param.Value = equipmentTechnicsRequest.EquipWithTechRequestsStatus.EquipWithTechRequestsStatusId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryUnitID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (equipmentTechnicsRequest.MilitaryUnit != null)
                    param.Value = equipmentTechnicsRequest.MilitaryUnit.MilitaryUnitId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AdministrationID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (equipmentTechnicsRequest.Administration != null)
                    param.Value = equipmentTechnicsRequest.Administration.AdministrationId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                if (equipmentTechnicsRequest.EquipmentTechnicsRequestId == 0)
                {
                    BaseDbObjectUtil.SetCreatedParams(cmd, currentUser);
                }
                else
                {
                    BaseDbObjectUtil.SetAnyActualChanges(cmd, changeEvent);
                }

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();

                if (equipmentTechnicsRequest.EquipmentTechnicsRequestId == 0)
                    equipmentTechnicsRequest.EquipmentTechnicsRequestId = DBCommon.GetInt(paramEquipmentTechnicsRequestID.Value);

                result = true;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                if (changeEvent.ChangeEventDetails.Count > 0)
                    changeEntry.AddEvent(changeEvent);
            }

            return result;
        }

        //Delete a particular object from the DB
        public static bool DeleteEquipmentTechnicsRequest(int equipmentTechnicsRequestId, User currentUser, Change changeEntry)
        {
            bool result = false;

            EquipmentTechnicsRequest oldEquipmentTechnicsRequest = GetEquipmentTechnicsRequest(equipmentTechnicsRequestId, currentUser);

            string logDescription = "";
            logDescription += "Заявка №: " + oldEquipmentTechnicsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(oldEquipmentTechnicsRequest.RequestDate);

            ChangeEvent changeEvent = new ChangeEvent("RES_EquipTechRequests_Delete", logDescription, null, null, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_RequestNum", oldEquipmentTechnicsRequest.RequestNumber, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_RequestDate", CommonFunctions.FormatDate(oldEquipmentTechnicsRequest.RequestDate), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_Status", oldEquipmentTechnicsRequest.EquipWithTechRequestsStatus != null && oldEquipmentTechnicsRequest.EquipWithTechRequestsStatus.StatusName != null ? oldEquipmentTechnicsRequest.EquipWithTechRequestsStatus.StatusName : "", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_MilitaryUnit", oldEquipmentTechnicsRequest.MilitaryUnit != null && oldEquipmentTechnicsRequest.MilitaryUnit.DisplayTextForSelection != null ? oldEquipmentTechnicsRequest.MilitaryUnit.DisplayTextForSelection : "", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_EquipTechReq_Administration", oldEquipmentTechnicsRequest.Administration != null && oldEquipmentTechnicsRequest.Administration.AdministrationName != null ? oldEquipmentTechnicsRequest.Administration.AdministrationName : "", "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" BEGIN
                                   DELETE FROM PMIS_RES.TechRequestCmdPositionsMilDept
                                   WHERE TechnicsRequestCmdPositionID IN (SELECT TechnicsRequestCmdPositionID 
                                                                          FROM PMIS_RES.TechnicsRequestCmdPositions
                                                                          WHERE TechRequestsCommandID IN (SELECT TechRequestsCommandID
                                                                                                          FROM PMIS_RES.TechnicsRequestCommands
                                                                                                          WHERE EquipmentTechnicsRequestID = :EquipmentTechnicsRequestID
                                                                                                         )
                                                                         );

                                   DELETE FROM PMIS_RES.TechnicsRequestCmdPositions
                                   WHERE TechnicsRequestCmdPositionID IN (SELECT TechnicsRequestCmdPositionID 
                                                                          FROM PMIS_RES.TechnicsRequestCmdPositions
                                                                          WHERE TechRequestsCommandID IN (SELECT TechRequestsCommandID
                                                                                                          FROM PMIS_RES.TechnicsRequestCommands
                                                                                                          WHERE EquipmentTechnicsRequestID = :EquipmentTechnicsRequestID
                                                                                                         )
                                                                         );

                                   UPDATE PMIS_RES.Technics SET
                                      PunktID = NULL
                                   WHERE PunktID IN (SELECT TechRequestCommandPunktID
                                                     FROM PMIS_RES.TechRequestCommandPunkt
                                                     WHERE TechRequestsCommandID IN (SELECT TechRequestsCommandID
                                                                                     FROM PMIS_RES.TechnicsRequestCommands
                                                                                     WHERE EquipmentTechnicsRequestID = :EquipmentTechnicsRequestID)
                                                     );

                                   DELETE FROM PMIS_RES.TechRequestCommandPunkt
                                   WHERE TechRequestsCommandID IN (SELECT TechRequestsCommandID
                                                                   FROM PMIS_RES.TechnicsRequestCommands
                                                                   WHERE EquipmentTechnicsRequestID = :EquipmentTechnicsRequestID);

                                   DELETE FROM PMIS_RES.TechnicsRequestCommands
                                   WHERE EquipmentTechnicsRequestID = :EquipmentTechnicsRequestID;

                                   DELETE FROM PMIS_RES.EquipmentTechnicsRequests 
                                   WHERE EquipmentTechnicsRequestID = :EquipmentTechnicsRequestID;
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("EquipmentTechnicsRequestID", OracleType.Number).Value = equipmentTechnicsRequestId;

                result = cmd.ExecuteNonQuery() == 1;
            }
            finally
            {
                conn.Close();
            }

            if (result)
                changeEntry.AddEvent(changeEvent);

            return result;
        }

        //When change any child record then update the last modified of request (the parent object)
        public static void SetEquipmentTechnicsRequestModified(int equipmentTechnicsRequestId, User currentUser)
        {
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"UPDATE PMIS_RES.EquipmentTechnicsRequests SET
                                  LastModifiedBy = :LastModifiedBy,
                                  LastModifiedDate = :LastModifiedDate
                               WHERE EquipmentTechnicsRequestID = :EquipmentTechnicsRequestID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("EquipmentTechnicsRequestID", OracleType.Number).Value = equipmentTechnicsRequestId;

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();
            }
            finally
            {
                conn.Close();
            }
        }
    }
}