using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;
using System.Web.UI.WebControls;

namespace PMIS.Reserve.Common
{
    public class ReportListReservistsWithAppointmentsBlock : BaseDbObject
    {
        private int equipmentReservistsRequestId;
        private string requestNumber;
        private string requestDate;
        private string militaryUnit;
        private string administration;
        private string requestStatus;       

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

        public string RequestDate
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

        public string MilitaryUnit
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

        public string Administration
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

        public string RequestStatus
        {
            get
            {
                return requestStatus;
            }
            set
            {
                requestStatus = value;
            }
        }

        public ReportListReservistsWithAppointmentsBlock(User user)
            : base(user)
        {
        }
    }

    //This class represents all information about the filter, the order and the paging information on the screen
    public class ReportListReservistsWithAppointmentsFilter
    {
        string requestNum;
        DateTime? requestDateFrom;
        DateTime? requestDateTo;
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

    public class ReportListReservistsWithAppointDetailsBlock : BaseDbObject
    {
        private int reservistId;
        private string militaryCommand;
        private string position;
        private string militaryReportingSpecialty;
        private string militaryRank;
        private string readiness;
        private string identityNumber;
        private string fullName;
        private string permPlaceName;

        public int ReservistId
        {
            get
            {
                return reservistId;
            }
            set
            {
                reservistId = value;
            }
        }

        public string MilitaryCommand
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

        public string MilitaryReportingSpecialty
        {
            get
            {
                return militaryReportingSpecialty;
            }
            set
            {
                militaryReportingSpecialty = value;
            }
        }

        public string MilitaryRank
        {
            get
            {
                return militaryRank;
            }
            set
            {
                militaryRank = value;
            }
        }

        public string Readiness
        {
            get
            {
                return readiness;
            }
            set
            {
                readiness = value;
            }
        }

        public string IdentityNumber
        {
            get
            {
                return identityNumber;
            }
            set
            {
                identityNumber = value;
            }
        }

        public string FullName
        {
            get
            {
                return fullName;
            }
            set
            {
                fullName = value;
            }
        }

        public string PermPlaceName
        {
            get
            {
                return permPlaceName;
            }
            set
            {
                permPlaceName = value;
            }
        }

        public ReportListReservistsWithAppointDetailsBlock (User user)
            : base(user)
        {
        }
    }

    public class ReportListReservistsWithAppointmentsBlockUtil
    {

        //This method creates and returns a ReportListReservistsWithAppointmentsBlock object. It extracts the data from a DataReader.
        public static ReportListReservistsWithAppointmentsBlock ExtractReportListReservistsWithAppointmentsBlockFromDataReader(OracleDataReader dr, User currentUser)
        {
            ReportListReservistsWithAppointmentsBlock reportListReservistsWithAppointmentsBlock = new ReportListReservistsWithAppointmentsBlock(currentUser);
        
            reportListReservistsWithAppointmentsBlock.EquipmentReservistsRequestId = DBCommon.GetInt(dr["EquipmentReservistsRequestID"]);
            reportListReservistsWithAppointmentsBlock.RequestNumber = dr["RequestNumber"].ToString();
            reportListReservistsWithAppointmentsBlock.RequestDate =  CommonFunctions.FormatDate((DateTime)dr["RequestDate"]);
            reportListReservistsWithAppointmentsBlock.MilitaryUnit = dr["MilitaryUnit"].ToString();
            reportListReservistsWithAppointmentsBlock.Administration = dr["administration"].ToString();
            reportListReservistsWithAppointmentsBlock.RequestStatus = dr["RequestStatus"].ToString();            

            return reportListReservistsWithAppointmentsBlock;
        }

        //This method get list of report items
        public static List<ReportListReservistsWithAppointmentsBlock> GetReportListReservistsWithAppointmentsBlockList(ReportListReservistsWithAppointmentsFilter filter, int rowsPerPage, User currentUser)
        {
            ReportListReservistsWithAppointmentsBlock reportBlock;
            List<ReportListReservistsWithAppointmentsBlock> listReportBlock = new List<ReportListReservistsWithAppointmentsBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restrict the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_REPORTS_LISTRESWITHAPPOINTMENTS", currentUser, false, currentUser.Role.RoleId, null)[0];
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


                if (!String.IsNullOrEmpty(filter.RequestNum))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.RequestNumber LIKE '%" + filter.RequestNum.Replace("'", "''") + @"%' ";
                }

                if (filter.RequestDateFrom.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.RequestDate >= " + DBCommon.DateToDBCode(filter.RequestDateFrom.Value) + " ";
                }

                if (filter.RequestDateTo.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.RequestDate < " + DBCommon.DateToDBCode(filter.RequestDateTo.Value.AddDays(1)) + " ";
                }

                if (!String.IsNullOrEmpty(filter.MilitaryUnits))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MilitaryUnitID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryUnits) + ") ";
                }

                if (!String.IsNullOrEmpty(filter.Administrations))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.AdministrationID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.Administrations) + ") ";
                }

                if (!String.IsNullOrEmpty(filter.EquipWithResRequestsStatuses))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.EquipWithResRequestsStatusID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.EquipWithResRequestsStatuses) + ") ";
                }

                if (!string.IsNullOrEmpty(filter.MilitaryDepartments))
                {
                    where += (where == "" ? "" : " AND ") +
                             " g.MilitaryDepartmentID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryDepartments) + ") ";

                }

                where = (where == "" ? "" : " WHERE ") + where;

                string pageWhere = "";

                if (filter.PageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE RowNumber BETWEEN (" + filter.PageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + filter.PageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

                string orderBySQL = "";
                string orderByDir = "ASC";

                if (filter.OrderBy > 100)
                {
                    filter.OrderBy -= 100;
                    orderByDir = "DESC";
                }

                switch (filter.OrderBy)
                {
                    case 1:
                        orderBySQL = "a.RequestNumber";
                        break;
                    case 2:
                        orderBySQL = "a.RequestDate";
                        break;
                    case 3:
                        orderBySQL = "c.IMEES";
                        break;
                    case 4:
                        orderBySQL = "d.AdministrationName";
                        break;
                    case 5:
                        orderBySQL = "b.StatusName";
                        break;
                    default:
                        orderBySQL = "a.RequestNumber";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                  SELECT DISTINCT a.EquipmentReservistsRequestID, 
                                         a.RequestNumber, 
                                         a.RequestDate,
                                         c.VPN || ' ' || c.IMEES as MilitaryUnit,
                                         d.AdministrationName as Administration,
                                         b.StatusName as RequestStatus,
                                         DENSE_RANK() OVER (ORDER BY " + orderBySQL + @", a.EquipmentReservistsRequestID) as RowNumber 
                                  FROM PMIS_RES.EquipmentReservistsRequests a
                                  LEFT OUTER JOIN PMIS_RES.RequestsCommands e ON a.EquipmentReservistsRequestID = e.EquipmentReservistsRequestID
                                  LEFT OUTER JOIN PMIS_RES.RequestCommandPositions f ON e.RequestsCommandID = f.RequestsCommandID
                                  LEFT OUTER JOIN PMIS_RES.RequestCommandPositionsMilDept g ON f.RequestCommandPositionID = g.RequestCommandPositionID

                                  LEFT OUTER JOIN PMIS_RES.EquipWithResRequestsStatuses b ON a.EquipWithResRequestsStatusID = b.EquipWithResRequestsStatusID
                                  LEFT OUTER JOIN UKAZ_OWNER.MIR c ON a.MilitaryUnitID = c.KOD_MIR
                                  LEFT OUTER JOIN PMIS_ADM.Administrations d ON a.AdministrationID = d.AdministrationID
                                  " + where + @"    
                                  ORDER BY " + orderBySQL + @", EquipmentReservistsRequestID                             
                               ) tmp
                               " + pageWhere;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    reportBlock = ExtractReportListReservistsWithAppointmentsBlockFromDataReader(dr, currentUser);
                    listReportBlock.Add(reportBlock);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listReportBlock;
        }

        //This method get Count of report items
        public static int GetReportListReservistsWithAppointmentsBlockCount(ReportListReservistsWithAppointmentsFilter filter, User currentUser)
        {
            int blocksCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restrict the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_REPORTS_LISTRESWITHAPPOINTMENTS", currentUser, false, currentUser.Role.RoleId, null)[0];
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


                if (!String.IsNullOrEmpty(filter.RequestNum))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.RequestNumber LIKE '%" + filter.RequestNum.Replace("'", "''") + @"%' ";
                }

                if (filter.RequestDateFrom.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.RequestDate >= " + DBCommon.DateToDBCode(filter.RequestDateFrom.Value) + " ";
                }

                if (filter.RequestDateTo.HasValue)
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.RequestDate < " + DBCommon.DateToDBCode(filter.RequestDateTo.Value.AddDays(1)) + " ";
                }

                if (!String.IsNullOrEmpty(filter.MilitaryUnits))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MilitaryUnitID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryUnits) + ") ";
                }

                if (!String.IsNullOrEmpty(filter.Administrations))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.AdministrationID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.Administrations) + ") ";
                }

                if (!String.IsNullOrEmpty(filter.EquipWithResRequestsStatuses))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.EquipWithResRequestsStatusID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.EquipWithResRequestsStatuses) + ") ";
                }

                if (!string.IsNullOrEmpty(filter.MilitaryDepartments))
                    where += (where == "" ? "" : " AND ") +
                             " d.MilitaryDepartmentID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryDepartments) + ") ";


                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @"SELECT COUNT(*) as Cnt
                               FROM PMIS_RES.EquipmentReservistsRequests a
                               WHERE a.EquipmentReservistsRequestID IN (SELECT a.EquipmentReservistsRequestID
                                                                        FROM PMIS_RES.EquipmentReservistsRequests a
                                                                        LEFT OUTER JOIN PMIS_RES.RequestsCommands b ON a.EquipmentReservistsRequestID = b.EquipmentReservistsRequestID
                                                                        LEFT OUTER JOIN PMIS_RES.RequestCommandPositions c ON b.RequestsCommandID = c.RequestsCommandID
                                                                        LEFT OUTER JOIN PMIS_RES.RequestCommandPositionsMilDept d ON c.RequestCommandPositionID = d.RequestCommandPositionID
                                                                        " + where + @"
                                                                        )                               
                               ";


                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        blocksCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return blocksCnt;
        }

        //This method get list of report items
        public static List<ReportListReservistsWithAppointDetailsBlock> GetReportListReservistsWithAppointDetailsBlockList(int equipmentReservistsRequestID, User currentUser)
        {
            ReportListReservistsWithAppointDetailsBlock reportBlock;
            List<ReportListReservistsWithAppointDetailsBlock> listReportBlock = new List<ReportListReservistsWithAppointDetailsBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT r.ReservistId,
                                      h.NK || ' ' || h.IMEES || ' ' || e.MilitaryCommandSuffix as MilitaryCommand,
                                      f.Position, 
                                      PMIS_RES.RESFunctions.GetMRSPerReqCmdPositionHTML(f.RequestCommandPositionID, 20) as MilitaryReportingSpecialty,
                                      z.ZVA_IMEES as MilitaryRank,
                                      g.ReservistReadinessID,
                                      v.EGN as IdentityNumber,
                                      v.IME || ' ' || v.FAM as FullName,
                                      obl.IME_OBL || CASE WHEN obl.IME_OBL IS NULL THEN '' ELSE ', ' END || obs.IME_OBS || CASE WHEN cities.Ime_Nma IS NULL THEN '' ELSE ', ' END || cities.Ime_Nma as PermPlaceName
                               FROM PMIS_RES.EquipmentReservistsRequests a
                               INNER JOIN PMIS_RES.RequestsCommands e ON a.EquipmentReservistsRequestID = e.EquipmentReservistsRequestID
                               INNER JOIN PMIS_RES.RequestCommandPositions f ON e.RequestsCommandID = f.RequestsCommandID
                               INNER JOIN PMIS_RES.FillReservistsRequest g ON f.RequestCommandPositionID = g.RequestCommandPositionID
                               LEFT OUTER JOIN UKAZ_OWNER.VVR h ON e.MilitaryCommandID = h.KOD_VVR 
                               INNER JOIN PMIS_RES.Reservists r ON g.ReservistID = r.ReservistID
                               INNER JOIN VS_OWNER.VS_LS v ON r.PersonID = v.PersonID
                               LEFT OUTER JOIN PMIS_RES.CommandPositionMilRanks mr ON f.RequestCommandPositionID = mr.RequestCommandPositionID AND mr.IsPrimary = 1
                               LEFT OUTER JOIN VS_OWNER.KLV_ZVA z ON mr.MilitaryRankID = z.ZVA_KOD
                               LEFT OUTER JOIN UKAZ_OWNER.KL_NMA cities ON v.KOD_NMA_MJ = cities.Kod_Nma
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBS obs ON cities.KOD_OBS = obs.KOD_OBS
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBL obl ON cities.KOD_OBL = obl.KOD_OBL             
                               WHERE a.EquipmentReservistsRequestID = :EquipmentReservistsRequestID
                               ORDER BY MilitaryCommand, Position, ReservistReadinessID, IdentityNumber, FullName";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("EquipmentReservistsRequestID", OracleType.Number).Value = equipmentReservistsRequestID;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    reportBlock = new ReportListReservistsWithAppointDetailsBlock(currentUser);

                    reportBlock.ReservistId = DBCommon.GetInt(dr["ReservistId"]);
                    reportBlock.MilitaryCommand = dr["MilitaryCommand"].ToString();
                    reportBlock.Position = dr["Position"].ToString();
                    reportBlock.MilitaryReportingSpecialty = dr["MilitaryReportingSpecialty"].ToString();
                    reportBlock.MilitaryRank = dr["MilitaryRank"].ToString();
                    reportBlock.Readiness = ReadinessUtil.ReadinessName(DBCommon.GetInt(dr["ReservistReadinessID"]));
                    reportBlock.IdentityNumber = dr["IdentityNumber"].ToString();
                    reportBlock.FullName = dr["FullName"].ToString();
                    reportBlock.PermPlaceName = dr["PermPlaceName"].ToString();

                    listReportBlock.Add(reportBlock);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return listReportBlock;
        }
    }
}
