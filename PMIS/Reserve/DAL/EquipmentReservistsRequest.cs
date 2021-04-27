using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    public class EquipmentReservistsRequest : BaseDbObject
    {
        private int equipmentReservistsRequestId;
        private string requestNumber;
        private DateTime? requestDate;
        private EquipWithResRequestsStatus equipWithResRequestsStatus;
        private MilitaryUnit militaryUnit;
        private Administration administration;
        private List<RequestCommand> requestCommands = null;
        private int reservistsCount = 0;
        private int fulfilCount = 0;
        private int fulfilResCount = 0;

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

        public EquipWithResRequestsStatus EquipWithResRequestsStatus
        {
            get
            {
                return equipWithResRequestsStatus;
            }
            set
            {
                equipWithResRequestsStatus = value;
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

        public List<RequestCommand> RequestCommands
        {
            get
            {
                //Lazy initialization
                if (requestCommands == null)
                {
                    requestCommands = RequestCommandUtil.GetRequestCommandsForRequest(CurrentUser, EquipmentReservistsRequestId);
                }

                return requestCommands;
            }
            set
            {
                requestCommands = value;
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

        public EquipmentReservistsRequest(User user)
            : base(user)
        {

        }
    }

    //This class represents all information about the filter, the order and the paging information on the screen
    public class EquipmentReservistsRequestsFilter
    {
        string requestNum;
        DateTime? requestDateFrom;
        DateTime? requestDateTo;
        string commandNum;
        string militaryUnits;
        string administrations;
        string equipWithResRequestsStatuses;
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

        public string EquipWithResRequestsStatuses
        {
            get
            {
                return equipWithResRequestsStatuses;
            }
            set
            {
                equipWithResRequestsStatuses = value;
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

    public static class EquipmentReservistsRequestUtil
    {
        //This method creates and returns a EquipmentReservistsRequest object. It extracts the data from a DataReader.
        //This is done in this way to have a signle place of creation of this object, however, pass the particular DataReader
        //object from various queries for better performance instead of executing a new call to the DB here to get the details for a specific EquipWithResRequestsStatusID, for example.
        public static EquipmentReservistsRequest ExtractEquipmentReservistsRequestFromDataReader(OracleDataReader dr, User currentUser)
        {
            int? equipmentReservistsRequestId = null;

            if (DBCommon.IsInt(dr["EquipmentReservistsRequestID"]))
                equipmentReservistsRequestId = DBCommon.GetInt(dr["EquipmentReservistsRequestID"]);

            string requestNumber = dr["RequestNumber"].ToString();
            DateTime? requestDate = null;
            if (dr["RequestDate"] is DateTime)
                requestDate = (DateTime)dr["RequestDate"];

            EquipmentReservistsRequest equipmentReservistsRequest = null;

            if (equipmentReservistsRequestId.HasValue)
            {
                equipmentReservistsRequest = new EquipmentReservistsRequest(currentUser);
                equipmentReservistsRequest.EquipmentReservistsRequestId = equipmentReservistsRequestId.Value;
                equipmentReservistsRequest.RequestNumber = requestNumber;
                equipmentReservistsRequest.RequestDate = requestDate;
                equipmentReservistsRequest.ReservistsCount = DBCommon.GetInt(dr["ReservistsCount"]);
                equipmentReservistsRequest.FulfilCount = DBCommon.GetInt(dr["FulfilCount"]);
                equipmentReservistsRequest.FulfilResCount = DBCommon.GetInt(dr["FulfilResCount"]);

                BaseDbObjectUtil.ExtractCreatedAndLastModified(dr, equipmentReservistsRequest);
            }

            return equipmentReservistsRequest;
        }

        //Get a particular object by its ID
        public static EquipmentReservistsRequest GetEquipmentReservistsRequest(int equipmentReservistsRequestId, User currentUser)
        {
            EquipmentReservistsRequest equipmentReservistsRequest = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restrict the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_EQUIPRESREQUESTS", currentUser, false, currentUser.Role.RoleId, null)[0];
                if (uiItem.AccessOnlyOwnData)
                {
                    where = " AND a.CreatedBy = " + currentUser.UserId.ToString();
                }

                string SQL = @"SELECT a.EquipmentReservistsRequestID, a.RequestNumber, a.RequestDate,
                                      a.CreatedBy, a.CreatedDate, a.LastModifiedBy, a.LastModifiedDate,
                                      a.EquipWithResRequestsStatusID, b.StatusKey, b.StatusName,
                                      a.MilitaryUnitID, NULL as ParentID, c.KOD_NMA as CityID, c.IMEES as ShortName, c.IMEED as LongName, c.VPN as VPN,
                                      a.AdministrationID, d.AdministrationName,
                                      PMIS_RES.RESFunctions.GetEquipResReq_ResCount(a.EquipmentReservistsRequestID, :FiltMilDept) as ReservistsCount,
                                      PMIS_RES.RESFunctions.GetEquipResReq_FulfilCount(a.EquipmentReservistsRequestID, :FiltMilDept) as FulFilCount,
                                      PMIS_RES.RESFunctions.GetEquipResReq_FulfilResCount(a.EquipmentReservistsRequestID, :FiltMilDept) as FulfilResCount
                               FROM PMIS_RES.EquipmentReservistsRequests a
                               LEFT OUTER JOIN PMIS_RES.EquipWithResRequestsStatuses b ON a.EquipWithResRequestsStatusID = b.EquipWithResRequestsStatusID
                               LEFT OUTER JOIN UKAZ_OWNER.MIR c ON a.MilitaryUnitID = c.KOD_MIR
                               LEFT OUTER JOIN PMIS_ADM.Administrations d ON a.AdministrationID = d.AdministrationID
                               WHERE a.EquipmentReservistsRequestID = :EquipmentReservistsRequestID " + where;

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("EquipmentReservistsRequestID", OracleType.Number).Value = equipmentReservistsRequestId;
                cmd.Parameters.Add("FiltMilDept", OracleType.VarChar).Value = currentUser.MilitaryDepartmentIDs;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    equipmentReservistsRequest = EquipmentReservistsRequestUtil.ExtractEquipmentReservistsRequestFromDataReader(dr, currentUser);

                    if(DBCommon.IsInt(dr["EquipWithResRequestsStatusID"]))
                       equipmentReservistsRequest.EquipWithResRequestsStatus = EquipWithResRequestsStatusUtil.ExtractEquipWithResRequestsStatusFromDataReader(dr, currentUser);

                    if (DBCommon.IsInt(dr["MilitaryUnitID"]))
                        equipmentReservistsRequest.MilitaryUnit = MilitaryUnitUtil.ExtractMilitaryUnitFromDR(currentUser, dr);

                    if (DBCommon.IsInt(dr["AdministrationID"]))
                        equipmentReservistsRequest.Administration = AdministrationUtil.ExtractAdministrationFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return equipmentReservistsRequest;
        }

        //Get a list of all EquipmentReservistsRequest records
        public static List<EquipmentReservistsRequest> GetAllEquipmentReservistsRequest(EquipmentReservistsRequestsFilter filt, int rowsPerPage, User currentUser)
        {
            List<EquipmentReservistsRequest> equipmentReservistsRequests = new List<EquipmentReservistsRequest>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                bool join_RequestsCommands = false;
                bool join_RequestCommandPositions = false;
                bool join_RequestCommandPositionsMilDept = false;
                bool join_EquipWithResRequestsStatuses = false;
                bool join_MIR = false;
                bool join_Administrations = false;
                bool join_VVR = false;

                string where = "";

                //Restrict the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_EQUIPRESREQUESTS", currentUser, false, currentUser.Role.RoleId, null)[0];
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

                if (!String.IsNullOrEmpty(filt.EquipWithResRequestsStatuses))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.EquipWithResRequestsStatusID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filt.EquipWithResRequestsStatuses) + ") ";
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

                join_RequestCommandPositionsMilDept = true;

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
                        join_EquipWithResRequestsStatuses = true;
                        break;
                    default:
                        orderBySQL = "a.RequestNumber";
                        break;
                }

                if (join_RequestCommandPositionsMilDept)
                {
                    join_RequestCommandPositions = true;
                }

                if (join_RequestCommandPositions || join_VVR)
                {
                    join_RequestsCommands = true;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT tmp.RowNumber,
                                      a.EquipmentReservistsRequestID, a.RequestNumber, a.RequestDate,
                                      a.CreatedBy, a.CreatedDate, a.LastModifiedBy, a.LastModifiedDate,
                                      a.EquipWithResRequestsStatusID, b.StatusKey, b.StatusName,
                                      a.MilitaryUnitID, NULL as ParentID, c.KOD_NMA as CityID, c.IMEES as ShortName, c.IMEED as LongName, c.VPN as VPN,
                                      a.AdministrationID, d.AdministrationName,
                                      PMIS_RES.RESFunctions.GetEquipResReq_ResCount(a.EquipmentReservistsRequestID, :FiltMilDept) as ReservistsCount,
                                      PMIS_RES.RESFunctions.GetEquipResReq_FulfilCount(a.EquipmentReservistsRequestID, :FiltMilDept) as FulFilCount,
                                      PMIS_RES.RESFunctions.GetEquipResReq_FulfilResCount(a.EquipmentReservistsRequestID, :FiltMilDept) as FulfilResCount
                                FROM (      SELECT DISTINCT a.EquipmentReservistsRequestID,
                                                   DENSE_RANK() OVER (ORDER BY " + orderBySQL + @", a.EquipmentReservistsRequestID) as RowNumber 
                                            FROM PMIS_RES.EquipmentReservistsRequests a
              " + (join_RequestsCommands ? "LEFT OUTER JOIN PMIS_RES.RequestsCommands e ON a.EquipmentReservistsRequestID = e.EquipmentReservistsRequestID" : "") + @"
       " + (join_RequestCommandPositions ? "LEFT OUTER JOIN PMIS_RES.RequestCommandPositions f ON e.RequestsCommandID = f.RequestsCommandID" : "") + @"
" + (join_RequestCommandPositionsMilDept ? "LEFT OUTER JOIN PMIS_RES.RequestCommandPositionsMilDept g ON f.RequestCommandPositionID = g.RequestCommandPositionID" : "") + @"

  " + (join_EquipWithResRequestsStatuses ? "LEFT OUTER JOIN PMIS_RES.EquipWithResRequestsStatuses b ON a.EquipWithResRequestsStatusID = b.EquipWithResRequestsStatusID" : "") + @"
                           " + (join_MIR ? "LEFT OUTER JOIN UKAZ_OWNER.MIR c ON a.MilitaryUnitID = c.KOD_MIR" : "") + @"
               " + (join_Administrations ? "LEFT OUTER JOIN PMIS_ADM.Administrations d ON a.AdministrationID = d.AdministrationID" : "") + @"

                           " + (join_VVR ? "LEFT OUTER JOIN UKAZ_OWNER.VVR h ON e.MilitaryCommandID = h.KOD_VVR" : "") + @"
                                           " + where + @"      
                                      ) tmp
                                INNER JOIN PMIS_RES.EquipmentReservistsRequests a ON tmp.EquipmentReservistsRequestID = a.EquipmentReservistsRequestID

                                LEFT OUTER JOIN PMIS_RES.EquipWithResRequestsStatuses b ON a.EquipWithResRequestsStatusID = b.EquipWithResRequestsStatusID
                                LEFT OUTER JOIN UKAZ_OWNER.MIR c ON a.MilitaryUnitID = c.KOD_MIR
                                LEFT OUTER JOIN PMIS_ADM.Administrations d ON a.AdministrationID = d.AdministrationID
                                " + pageWhere + @"
                                ORDER BY tmp.RowNumber";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("FiltMilDept", OracleType.VarChar).Value = filtMilDept;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    EquipmentReservistsRequest equipmentReservistsRequest = EquipmentReservistsRequestUtil.ExtractEquipmentReservistsRequestFromDataReader(dr, currentUser);

                    if (DBCommon.IsInt(dr["EquipWithResRequestsStatusID"]))
                        equipmentReservistsRequest.EquipWithResRequestsStatus = EquipWithResRequestsStatusUtil.ExtractEquipWithResRequestsStatusFromDataReader(dr, currentUser);

                    if (DBCommon.IsInt(dr["MilitaryUnitID"]))
                        equipmentReservistsRequest.MilitaryUnit = MilitaryUnitUtil.ExtractMilitaryUnitFromDR(currentUser, dr);

                    if (DBCommon.IsInt(dr["AdministrationID"]))
                        equipmentReservistsRequest.Administration = AdministrationUtil.ExtractAdministrationFromDataReader(dr, currentUser);

                    equipmentReservistsRequests.Add(equipmentReservistsRequest);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return equipmentReservistsRequests;
        }

        //Get a list of all EquipmentReservistsRequest records for fulfilment (filtered to military departments of the user)
        public static List<EquipmentReservistsRequest> GetAllEquipmentReservistsRequestForFulfilment(EquipmentReservistsRequestsFilter filt, int rowsPerPage, User currentUser)
        {
            List<EquipmentReservistsRequest> equipmentReservistsRequests = new List<EquipmentReservistsRequest>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                bool join_RequestsCommands = false;
                bool join_RequestCommandPositions = false;
                bool join_RequestCommandPositionsMilDept = false;
                bool join_EquipWithResRequestsStatuses = false;
                bool join_MIR = false;
                bool join_Administrations = false;
                bool join_VVR = false;

                string where = "";

                //Restrict the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_EQUIPRESREQUESTS", currentUser, false, currentUser.Role.RoleId, null)[0];
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

                if (!String.IsNullOrEmpty(filt.EquipWithResRequestsStatuses))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.EquipWithResRequestsStatusID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filt.EquipWithResRequestsStatuses) + ") ";
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
                             " NVL(g.ReservistsCount, 0) > 0";

                join_RequestCommandPositionsMilDept = true;

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
                        join_EquipWithResRequestsStatuses = true;
                        break;
                    case 6:
                        orderBySQL = @" CASE WHEN PMIS_RES.RESFunctions.GetEquipResReq_ResCount(a.EquipmentReservistsRequestID, :FiltMilDept) = 0 
                                             THEN 0 
                                             ELSE PMIS_RES.RESFunctions.GetEquipResReq_FulfilCount(a.EquipmentReservistsRequestID, :FiltMilDept) / 
                                                  PMIS_RES.RESFunctions.GetEquipResReq_ResCount(a.EquipmentReservistsRequestID, :FiltMilDept)
                                        END ";
                        break;
                    default:
                        orderBySQL = "a.RequestNumber";
                        break;
                }

                if (join_RequestCommandPositionsMilDept)
                {
                    join_RequestCommandPositions = true;
                }

                if (join_RequestCommandPositions || join_VVR)
                {
                    join_RequestsCommands = true;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT tmp.*,
                                      a.EquipmentReservistsRequestID, a.RequestNumber, a.RequestDate,
                                      a.CreatedBy, a.CreatedDate, a.LastModifiedBy, a.LastModifiedDate,
                                      a.EquipWithResRequestsStatusID, b.StatusKey, b.StatusName,
                                      a.MilitaryUnitID, NULL as ParentID, c.KOD_NMA as CityID, c.IMEES as ShortName, c.IMEED as LongName, c.VPN as VPN,
                                      a.AdministrationID, d.AdministrationName,
                                      PMIS_RES.RESFunctions.GetEquipResReq_ResCount(a.EquipmentReservistsRequestID, :FiltMilDept) as ReservistsCount,
                                      PMIS_RES.RESFunctions.GetEquipResReq_FulfilCount(a.EquipmentReservistsRequestID, :FiltMilDept) as FulFilCount,
                                      PMIS_RES.RESFunctions.GetEquipResReq_FulfilResCount(a.EquipmentReservistsRequestID, :FiltMilDept) as FulfilResCount
                               FROM (       SELECT DISTINCT a.EquipmentReservistsRequestID,
                                                   DENSE_RANK() OVER (ORDER BY " + orderBySQL + @", a.EquipmentReservistsRequestID) as RowNumber 
                                            FROM PMIS_RES.EquipmentReservistsRequests a
              " + (join_RequestsCommands ? "INNER JOIN PMIS_RES.RequestsCommands e ON a.EquipmentReservistsRequestID = e.EquipmentReservistsRequestID" : "") + @"
       " + (join_RequestCommandPositions ? "INNER JOIN PMIS_RES.RequestCommandPositions f ON e.RequestsCommandID = f.RequestsCommandID" : "") + @"
" + (join_RequestCommandPositionsMilDept ? "INNER JOIN PMIS_RES.RequestCommandPositionsMilDept g ON f.RequestCommandPositionID = g.RequestCommandPositionID" : "") + @"

  " + (join_EquipWithResRequestsStatuses ? "LEFT OUTER JOIN PMIS_RES.EquipWithResRequestsStatuses b ON a.EquipWithResRequestsStatusID = b.EquipWithResRequestsStatusID" : "") + @"
                           " + (join_MIR ? "LEFT OUTER JOIN UKAZ_OWNER.MIR c ON a.MilitaryUnitID = c.KOD_MIR" : "") + @"
               " + (join_Administrations ? "LEFT OUTER JOIN PMIS_ADM.Administrations d ON a.AdministrationID = d.AdministrationID" : "") + @"

                           " + (join_VVR ? "LEFT OUTER JOIN UKAZ_OWNER.VVR h ON e.MilitaryCommandID = h.KOD_VVR" : "") + @"
                                           " + where + @"    
                                    ) tmp
                               INNER JOIN PMIS_RES.EquipmentReservistsRequests a ON tmp.EquipmentReservistsRequestID = a.EquipmentReservistsRequestID

                               LEFT OUTER JOIN PMIS_RES.EquipWithResRequestsStatuses b ON a.EquipWithResRequestsStatusID = b.EquipWithResRequestsStatusID
                               LEFT OUTER JOIN UKAZ_OWNER.MIR c ON a.MilitaryUnitID = c.KOD_MIR
                               LEFT OUTER JOIN PMIS_ADM.Administrations d ON a.AdministrationID = d.AdministrationID
                               " + pageWhere + @"
                               ORDER BY tmp.RowNumber";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("FiltMilDept", OracleType.VarChar).Value = filtMilDept;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    EquipmentReservistsRequest equipmentReservistsRequest = EquipmentReservistsRequestUtil.ExtractEquipmentReservistsRequestFromDataReader(dr, currentUser);

                    if (DBCommon.IsInt(dr["EquipWithResRequestsStatusID"]))
                        equipmentReservistsRequest.EquipWithResRequestsStatus = EquipWithResRequestsStatusUtil.ExtractEquipWithResRequestsStatusFromDataReader(dr, currentUser);

                    if (DBCommon.IsInt(dr["MilitaryUnitID"]))
                        equipmentReservistsRequest.MilitaryUnit = MilitaryUnitUtil.ExtractMilitaryUnitFromDR(currentUser, dr);

                    if (DBCommon.IsInt(dr["AdministrationID"]))
                        equipmentReservistsRequest.Administration = AdministrationUtil.ExtractAdministrationFromDataReader(dr, currentUser);

                    equipmentReservistsRequests.Add(equipmentReservistsRequest);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return equipmentReservistsRequests;
        }

        public static int GetAllEquipmentReservistsRequestCount(EquipmentReservistsRequestsFilter filt, User currentUser)
        {
            int equipmentReservistsRequestsCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                bool join_RequestsCommands = false;
                bool join_RequestCommandPositions = false;
                bool join_RequestCommandPositionsMilDept = false;
                bool join_VVR = false;

                string where = "";

                //Restrict the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_EQUIPRESREQUESTS", currentUser, false, currentUser.Role.RoleId, null)[0];
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

                if (!String.IsNullOrEmpty(filt.EquipWithResRequestsStatuses))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.EquipWithResRequestsStatusID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filt.EquipWithResRequestsStatuses) + ") ";
                }

                if (!string.IsNullOrEmpty(filt.MilitaryDepartments))
                    where += (where == "" ? "" : " AND ") +
                             " d.MilitaryDepartmentID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filt.MilitaryDepartments) + ") ";

                join_RequestCommandPositionsMilDept = true;

                if (join_RequestCommandPositionsMilDept)
                {
                    join_RequestCommandPositions = true;
                }

                if (join_RequestCommandPositions || join_VVR)
                {
                    join_RequestsCommands = true;
                }

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @"             SELECT COUNT(DISTINCT a.EquipmentReservistsRequestID) as Cnt
                                            FROM PMIS_RES.EquipmentReservistsRequests a
              " + (join_RequestsCommands ? "LEFT OUTER JOIN PMIS_RES.RequestsCommands b ON a.EquipmentReservistsRequestID = b.EquipmentReservistsRequestID" : "") + @"
       " + (join_RequestCommandPositions ? "LEFT OUTER JOIN PMIS_RES.RequestCommandPositions c ON b.RequestsCommandID = c.RequestsCommandID" : "") + @"
" + (join_RequestCommandPositionsMilDept ? "LEFT OUTER JOIN PMIS_RES.RequestCommandPositionsMilDept d ON c.RequestCommandPositionID = d.RequestCommandPositionID" : "") + @"
                           " + (join_VVR ? "LEFT OUTER JOIN UKAZ_OWNER.VVR h ON b.MilitaryCommandID = h.KOD_VVR" : "") + @"
                                           " + where + @"
                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        equipmentReservistsRequestsCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return equipmentReservistsRequestsCnt;
        }

        public static int GetAllEquipmentReservistsRequestForFulfilmentCount(EquipmentReservistsRequestsFilter filt, User currentUser)
        {
            int equipmentReservistsRequestsCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                bool join_RequestsCommands = false;
                bool join_RequestCommandPositions = false;
                bool join_RequestCommandPositionsMilDept = false;
                bool join_VVR = false;

                string where = "";

                //Restrict the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_EQUIPRESREQUESTS", currentUser, false, currentUser.Role.RoleId, null)[0];
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

                if (!String.IsNullOrEmpty(filt.EquipWithResRequestsStatuses))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.EquipWithResRequestsStatusID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filt.EquipWithResRequestsStatuses) + ") ";
                }

                if (!string.IsNullOrEmpty(filt.MilitaryDepartments))
                    where += (where == "" ? "" : " AND ") +
                             " d.MilitaryDepartmentID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filt.MilitaryDepartments) + ") ";
                else
                    where += (where == "" ? "" : " AND ") +
                             " d.MilitaryDepartmentID IN (" + currentUser.MilitaryDepartmentIDs + ") ";

                where += (where == "" ? "" : " AND ") +
                             " NVL(d.ReservistsCount, 0) > 0";

                join_RequestCommandPositionsMilDept = true;

                if (join_RequestCommandPositionsMilDept)
                {
                    join_RequestCommandPositions = true;
                }

                if (join_RequestCommandPositions || join_VVR)
                {
                    join_RequestsCommands = true;
                }

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @"             SELECT COUNT(DISTINCT a.EquipmentReservistsRequestID) as Cnt
                                            FROM PMIS_RES.EquipmentReservistsRequests a
              " + (join_RequestsCommands ? "INNER JOIN PMIS_RES.RequestsCommands b ON a.EquipmentReservistsRequestID = b.EquipmentReservistsRequestID" : "") + @"
       " + (join_RequestCommandPositions ? "INNER JOIN PMIS_RES.RequestCommandPositions c ON b.RequestsCommandID = c.RequestsCommandID" : "") + @"
" + (join_RequestCommandPositionsMilDept ? "INNER JOIN PMIS_RES.RequestCommandPositionsMilDept d ON c.RequestCommandPositionID = d.RequestCommandPositionID" : "") + @"
                           " + (join_VVR ? "LEFT OUTER JOIN UKAZ_OWNER.VVR h ON b.MilitaryCommandID = h.KOD_VVR" : "") + @"
                               " + where + @"
                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        equipmentReservistsRequestsCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return equipmentReservistsRequestsCnt;
        }

        //Save a particular object into the DB
        public static bool SaveEquipmentReservistsRequest(EquipmentReservistsRequest equipmentReservistsRequest, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            string logDescription = "";
            logDescription += "Заявка №: " + equipmentReservistsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(equipmentReservistsRequest.RequestDate);

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";

                //New record
                if (equipmentReservistsRequest.EquipmentReservistsRequestId == 0)
                {
                    SQL += @"INSERT INTO PMIS_RES.EquipmentReservistsRequests (RequestNumber, RequestDate, 
                                EquipWithResRequestsStatusID, MilitaryUnitID, AdministrationID, 
                                CreatedBy, CreatedDate, LastModifiedBy, LastModifiedDate)
                            VALUES (:RequestNumber, :RequestDate, 
                                :EquipWithResRequestsStatusID, :MilitaryUnitID, :AdministrationID, 
                                :CreatedBy, :CreatedDate, :LastModifiedBy, :LastModifiedDate);

                            SELECT PMIS_RES.EquipResRequests_ID_SEQ.currval INTO :EquipmentReservistsRequestID FROM dual;

                            ";

                    changeEvent = new ChangeEvent("RES_EquipResRequests_Add", logDescription, equipmentReservistsRequest.MilitaryUnit, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_RequestNum", "", equipmentReservistsRequest.RequestNumber, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_RequestDate", "", CommonFunctions.FormatDate(equipmentReservistsRequest.RequestDate), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_Status", "", equipmentReservistsRequest.EquipWithResRequestsStatus != null && equipmentReservistsRequest.EquipWithResRequestsStatus.StatusName != null ? equipmentReservistsRequest.EquipWithResRequestsStatus.StatusName : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_MilitaryUnit", "", equipmentReservistsRequest.MilitaryUnit != null && equipmentReservistsRequest.MilitaryUnit.DisplayTextForSelection != null ? equipmentReservistsRequest.MilitaryUnit.DisplayTextForSelection : "", currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_Administration", "", equipmentReservistsRequest.Administration != null && equipmentReservistsRequest.Administration.AdministrationName != null ? equipmentReservistsRequest.Administration.AdministrationName : "", currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_RES.EquipmentReservistsRequests SET
                               RequestNumber = :RequestNumber,
                               RequestDate = :RequestDate, 
                               EquipWithResRequestsStatusID = :EquipWithResRequestsStatusID, 
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

                             WHERE EquipmentReservistsRequestID = :EquipmentReservistsRequestID ;                       

                            ";

                    changeEvent = new ChangeEvent("RES_EquipResRequests_Edit", logDescription, null, null, currentUser);

                    EquipmentReservistsRequest oldEquipmentReservistsRequest = EquipmentReservistsRequestUtil.GetEquipmentReservistsRequest(equipmentReservistsRequest.EquipmentReservistsRequestId, currentUser);

                    if (oldEquipmentReservistsRequest.RequestNumber.Trim() != equipmentReservistsRequest.RequestNumber.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_RequestNum", oldEquipmentReservistsRequest.RequestNumber, equipmentReservistsRequest.RequestNumber, currentUser));

                    if (!CommonFunctions.IsEqual(oldEquipmentReservistsRequest.RequestDate, equipmentReservistsRequest.RequestDate))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_RequestDate", CommonFunctions.FormatDate(oldEquipmentReservistsRequest.RequestDate), CommonFunctions.FormatDate(equipmentReservistsRequest.RequestDate), currentUser));

                    if ((oldEquipmentReservistsRequest.EquipWithResRequestsStatus != null && oldEquipmentReservistsRequest.EquipWithResRequestsStatus.StatusName != null ? oldEquipmentReservistsRequest.EquipWithResRequestsStatus.StatusName : "") !=
                        (equipmentReservistsRequest.EquipWithResRequestsStatus != null && equipmentReservistsRequest.EquipWithResRequestsStatus.StatusName != null ? equipmentReservistsRequest.EquipWithResRequestsStatus.StatusName : ""))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_Status", 
                            oldEquipmentReservistsRequest.EquipWithResRequestsStatus != null && oldEquipmentReservistsRequest.EquipWithResRequestsStatus.StatusName != null ? oldEquipmentReservistsRequest.EquipWithResRequestsStatus.StatusName : "",
                            equipmentReservistsRequest.EquipWithResRequestsStatus != null && equipmentReservistsRequest.EquipWithResRequestsStatus.StatusName != null ? equipmentReservistsRequest.EquipWithResRequestsStatus.StatusName : "", 
                            currentUser));

                    if ((oldEquipmentReservistsRequest.MilitaryUnit != null && oldEquipmentReservistsRequest.MilitaryUnit.DisplayTextForSelection != null ? oldEquipmentReservistsRequest.MilitaryUnit.DisplayTextForSelection : "") !=
                        (equipmentReservistsRequest.MilitaryUnit != null && equipmentReservistsRequest.MilitaryUnit.DisplayTextForSelection != null ? equipmentReservistsRequest.MilitaryUnit.DisplayTextForSelection : ""))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_MilitaryUnit",
                            oldEquipmentReservistsRequest.MilitaryUnit != null && oldEquipmentReservistsRequest.MilitaryUnit.DisplayTextForSelection != null ? oldEquipmentReservistsRequest.MilitaryUnit.DisplayTextForSelection : "",
                            equipmentReservistsRequest.MilitaryUnit != null && equipmentReservistsRequest.MilitaryUnit.DisplayTextForSelection != null ? equipmentReservistsRequest.MilitaryUnit.DisplayTextForSelection : "",
                            currentUser));

                    if ((oldEquipmentReservistsRequest.Administration != null && oldEquipmentReservistsRequest.Administration.AdministrationName != null ? oldEquipmentReservistsRequest.Administration.AdministrationName : "") !=
                        (equipmentReservistsRequest.Administration != null && equipmentReservistsRequest.Administration.AdministrationName != null ? equipmentReservistsRequest.Administration.AdministrationName : ""))
                        changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_Administration",
                            oldEquipmentReservistsRequest.Administration != null && oldEquipmentReservistsRequest.Administration.AdministrationName != null ? oldEquipmentReservistsRequest.Administration.AdministrationName : "",
                            equipmentReservistsRequest.Administration != null && equipmentReservistsRequest.Administration.AdministrationName != null ? equipmentReservistsRequest.Administration.AdministrationName : "",
                            currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramEquipmentReservistsRequestID = new OracleParameter();
                paramEquipmentReservistsRequestID.ParameterName = "EquipmentReservistsRequestID";
                paramEquipmentReservistsRequestID.OracleType = OracleType.Number;

                if (equipmentReservistsRequest.EquipmentReservistsRequestId != 0)
                {
                    paramEquipmentReservistsRequestID.Direction = ParameterDirection.Input;
                    paramEquipmentReservistsRequestID.Value = equipmentReservistsRequest.EquipmentReservistsRequestId;
                }
                else
                {
                    paramEquipmentReservistsRequestID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramEquipmentReservistsRequestID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "RequestNumber";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(equipmentReservistsRequest.RequestNumber))
                    param.Value = equipmentReservistsRequest.RequestNumber;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "RequestDate";
                param.OracleType = OracleType.DateTime;
                param.Direction = ParameterDirection.Input;
                if (equipmentReservistsRequest.RequestDate.HasValue)
                    param.Value = equipmentReservistsRequest.RequestDate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "EquipWithResRequestsStatusID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (equipmentReservistsRequest.EquipWithResRequestsStatus != null)
                    param.Value = equipmentReservistsRequest.EquipWithResRequestsStatus.EquipWithResRequestsStatusId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "MilitaryUnitID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (equipmentReservistsRequest.MilitaryUnit != null)
                    param.Value = equipmentReservistsRequest.MilitaryUnit.MilitaryUnitId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "AdministrationID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (equipmentReservistsRequest.Administration != null)
                    param.Value = equipmentReservistsRequest.Administration.AdministrationId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                if (equipmentReservistsRequest.EquipmentReservistsRequestId == 0)
                {
                    BaseDbObjectUtil.SetCreatedParams(cmd, currentUser);
                }
                else
                {
                    BaseDbObjectUtil.SetAnyActualChanges(cmd, changeEvent);
                }

                BaseDbObjectUtil.SetLastModifiedParams(cmd, currentUser);

                cmd.ExecuteNonQuery();

                if (equipmentReservistsRequest.EquipmentReservistsRequestId == 0)
                    equipmentReservistsRequest.EquipmentReservistsRequestId = DBCommon.GetInt(paramEquipmentReservistsRequestID.Value);

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
        public static bool DeleteEquipmentReservistsRequest(int equipmentReservistsRequestId, User currentUser, Change changeEntry)
        {
            bool result = false;

            EquipmentReservistsRequest oldEquipmentReservistsRequest = GetEquipmentReservistsRequest(equipmentReservistsRequestId, currentUser);

            string logDescription = "";
            logDescription += "Заявка №: " + oldEquipmentReservistsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(oldEquipmentReservistsRequest.RequestDate);

            ChangeEvent changeEvent = new ChangeEvent("RES_EquipResRequests_Delete", logDescription, null, null, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_RequestNum", oldEquipmentReservistsRequest.RequestNumber, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_RequestDate", CommonFunctions.FormatDate(oldEquipmentReservistsRequest.RequestDate), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_Status", oldEquipmentReservistsRequest.EquipWithResRequestsStatus != null && oldEquipmentReservistsRequest.EquipWithResRequestsStatus.StatusName != null ? oldEquipmentReservistsRequest.EquipWithResRequestsStatus.StatusName : "", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_MilitaryUnit", oldEquipmentReservistsRequest.MilitaryUnit != null && oldEquipmentReservistsRequest.MilitaryUnit.DisplayTextForSelection != null ? oldEquipmentReservistsRequest.MilitaryUnit.DisplayTextForSelection : "", "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("RES_EquipResReq_Administration", oldEquipmentReservistsRequest.Administration != null && oldEquipmentReservistsRequest.Administration.AdministrationName != null ? oldEquipmentReservistsRequest.Administration.AdministrationName : "", "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" BEGIN
                                   DELETE FROM PMIS_RES.RequestCommandPositionsMilDept
                                   WHERE RequestCommandPositionID IN (SELECT RequestCommandPositionID 
                                                                      FROM PMIS_RES.RequestCommandPositions
                                                                      WHERE RequestsCommandID IN (SELECT RequestsCommandID
                                                                                                  FROM PMIS_RES.RequestsCommands
                                                                                                  WHERE EquipmentReservistsRequestID = :EquipmentReservistsRequestID
                                                                                                 )
                                                                     );

                                   DELETE FROM PMIS_RES.CommandPositionMRSpecialities
                                   WHERE RequestCommandPositionID IN (SELECT RequestCommandPositionID 
                                                                      FROM PMIS_RES.RequestCommandPositions
                                                                      WHERE RequestsCommandID IN (SELECT RequestsCommandID
                                                                                                  FROM PMIS_RES.RequestsCommands
                                                                                                  WHERE EquipmentReservistsRequestID = :EquipmentReservistsRequestID
                                                                                                 )
                                                                     );

                                   UPDATE PMIS_RES.Reservists SET
                                      PunktID = NULL
                                   WHERE PunktID IN (SELECT RequestCommandPunktID
                                                     FROM PMIS_RES.RequestCommandPunkt
                                                     WHERE RequestCommandID IN (SELECT RequestsCommandID 
                                                                                FROM PMIS_RES.RequestsCommands
                                                                                WHERE EquipmentReservistsRequestID = :EquipmentReservistsRequestID
                                                                                )
                                                     );

                                   DELETE FROM PMIS_RES.RequestCommandPunkt
                                   WHERE RequestCommandID IN (SELECT RequestsCommandID 
                                                              FROM PMIS_RES.RequestsCommands
                                                              WHERE EquipmentReservistsRequestID = :EquipmentReservistsRequestID
                                                             );

                                   DELETE FROM PMIS_RES.RequestCommandPositions
                                   WHERE RequestCommandPositionID IN (SELECT RequestCommandPositionID 
                                                                      FROM PMIS_RES.RequestCommandPositions
                                                                      WHERE RequestsCommandID IN (SELECT RequestsCommandID
                                                                                                  FROM PMIS_RES.RequestsCommands
                                                                                                  WHERE EquipmentReservistsRequestID = :EquipmentReservistsRequestID
                                                                                                 )
                                                                     );

                                   DELETE FROM PMIS_RES.RequestsCommands
                                   WHERE EquipmentReservistsRequestID = :EquipmentReservistsRequestID;

                                   DELETE FROM PMIS_RES.EquipmentReservistsRequests 
                                   WHERE EquipmentReservistsRequestID = :EquipmentReservistsRequestID;
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("EquipmentReservistsRequestID", OracleType.Number).Value = equipmentReservistsRequestId;

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
        public static void SetEquipmentReservistsRequestModified(int equipmentReservistsRequestId, User currentUser)
        {
            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"UPDATE PMIS_RES.EquipmentReservistsRequests SET
                                  LastModifiedBy = :LastModifiedBy,
                                  LastModifiedDate = :LastModifiedDate
                               WHERE EquipmentReservistsRequestID = :EquipmentReservistsRequestID";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("EquipmentReservistsRequestID", OracleType.Number).Value = equipmentReservistsRequestId;

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