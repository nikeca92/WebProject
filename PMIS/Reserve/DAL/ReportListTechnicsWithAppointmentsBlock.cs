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
    public class ReportListTechnicsWithAppointmentsBlock : BaseDbObject
    {
        private int equipmentTechnicsRequestId;
        private string requestNumber;
        private string requestDate;
        private string militaryUnit;
        private string administration;
        private string requestStatus;       

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

        public ReportListTechnicsWithAppointmentsBlock(User user)
            : base(user)
        {
        }
    }

    //This class represents all information about the filter, the order and the paging information on the screen
    public class ReportListTechnicsWithAppointmentsFilter
    {
        string requestNum;
        DateTime? requestDateFrom;
        DateTime? requestDateTo;
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

    public class ReportListTechnicsWithAppointDetailsBlock : BaseDbObject
    {
        private int technicsId;
        private string militaryCommand;
        private string technicsType;
        private string normativeTechnics;
        private string readiness;
        private string regInvNumber;
        private string owner;

        public int TechnicsId
        {
            get
            {
                return technicsId;
            }
            set
            {
                technicsId = value;
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

        public string NormativeTechnics
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

        public string RegInvNumber
        {
            get
            {
                return regInvNumber;
            }
            set
            {
                regInvNumber = value;
            }
        }

        public string Owner
        {
            get
            {
                return owner;
            }
            set
            {
                owner = value;
            }
        }

        public ReportListTechnicsWithAppointDetailsBlock(User user)
            : base(user)
        {
        }
    }

    public class ReportListTechnicsWithAppointmentsBlockUtil
    {

        //This method creates and returns a ReportListTechnicsWithAppointmentsBlock object. It extracts the data from a DataReader.
        public static ReportListTechnicsWithAppointmentsBlock ExtractReportListTechnicsWithAppointmentsBlockFromDataReader(OracleDataReader dr, User currentUser)
        {
            ReportListTechnicsWithAppointmentsBlock reportListTechnicsWithAppointmentsBlock = new ReportListTechnicsWithAppointmentsBlock(currentUser);

            reportListTechnicsWithAppointmentsBlock.EquipmentTechnicsRequestId = DBCommon.GetInt(dr["EquipmentTechnicsRequestID"]);
            reportListTechnicsWithAppointmentsBlock.RequestNumber = dr["RequestNumber"].ToString();
            reportListTechnicsWithAppointmentsBlock.RequestDate = CommonFunctions.FormatDate((DateTime)dr["RequestDate"]);
            reportListTechnicsWithAppointmentsBlock.MilitaryUnit = dr["MilitaryUnit"].ToString();
            reportListTechnicsWithAppointmentsBlock.Administration = dr["administration"].ToString();
            reportListTechnicsWithAppointmentsBlock.RequestStatus = dr["RequestStatus"].ToString();

            return reportListTechnicsWithAppointmentsBlock;
        }

        //This method get list of report items
        public static List<ReportListTechnicsWithAppointmentsBlock> GetReportListTechnicsWithAppointmentsBlockList(ReportListTechnicsWithAppointmentsFilter filter, int rowsPerPage, User currentUser)
        {
            ReportListTechnicsWithAppointmentsBlock reportBlock;
            List<ReportListTechnicsWithAppointmentsBlock> listReportBlock = new List<ReportListTechnicsWithAppointmentsBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restrict the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_REPORTS_LISTTECHWITHAPPOINTMENTS", currentUser, false, currentUser.Role.RoleId, null)[0];
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

                if (!String.IsNullOrEmpty(filter.EquipWithTechRequestsStatuses))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.EquipWithTechRequestsStatusID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.EquipWithTechRequestsStatuses) + ") ";
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
                                  SELECT DISTINCT a.EquipmentTechnicsRequestID, 
                                         a.RequestNumber, 
                                         a.RequestDate,
                                         c.VPN || ' ' || c.IMEES as MilitaryUnit,
                                         d.AdministrationName as Administration,
                                         b.StatusName as RequestStatus,
                                         DENSE_RANK() OVER (ORDER BY " + orderBySQL + @", a.EquipmentTechnicsRequestID) as RowNumber 
                                  FROM PMIS_RES.EquipmentTechnicsRequests a
                                  LEFT OUTER JOIN PMIS_RES.TechnicsRequestCommands e ON a.EquipmentTechnicsRequestID = e.EquipmentTechnicsRequestID
                                  LEFT OUTER JOIN PMIS_RES.TechnicsRequestCmdPositions f ON e.TechRequestsCommandID = f.TechRequestsCommandID
                                  LEFT OUTER JOIN PMIS_RES.TechRequestCmdPositionsMilDept g ON f.TechnicsRequestCmdPositionID = g.TechnicsRequestCmdPositionID
                                                                        

                                  LEFT OUTER JOIN PMIS_RES.EquipWithTechRequestsStatuses b ON a.EquipWithTechRequestsStatusID = b.EquipWithTechRequestsStatusID
                                  LEFT OUTER JOIN UKAZ_OWNER.MIR c ON a.MilitaryUnitID = c.KOD_MIR
                                  LEFT OUTER JOIN PMIS_ADM.Administrations d ON a.AdministrationID = d.AdministrationID
                                  " + where + @"    
                                  ORDER BY " + orderBySQL + @", EquipmentTechnicsRequestID                             
                               ) tmp
                               " + pageWhere;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    reportBlock = ExtractReportListTechnicsWithAppointmentsBlockFromDataReader(dr, currentUser);
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
        public static int GetReportListTechnicsWithAppointmentsBlockCount(ReportListTechnicsWithAppointmentsFilter filter, User currentUser)
        {
            int blocksCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                //Restrict the user to access only his own records if this is set for the particular role
                UIItem uiItem = UIItemUtil.GetUIItems("RES_REPORTS_LISTTECHWITHAPPOINTMENTS", currentUser, false, currentUser.Role.RoleId, null)[0];
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

                if (!String.IsNullOrEmpty(filter.EquipWithTechRequestsStatuses))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.EquipWithTechRequestsStatusID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.EquipWithTechRequestsStatuses) + ") ";
                }

                if (!string.IsNullOrEmpty(filter.MilitaryDepartments))
                    where += (where == "" ? "" : " AND ") +
                             " d.MilitaryDepartmentID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryDepartments) + ") ";


                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @"SELECT COUNT(*) as Cnt
                               FROM PMIS_RES.EquipmentTechnicsRequests a
                               WHERE a.EquipmentTechnicsRequestID IN (SELECT a.EquipmentTechnicsRequestID
                                                                        FROM PMIS_RES.EquipmentTechnicsRequests a
                                                                        LEFT OUTER JOIN PMIS_RES.TechnicsRequestCommands b ON a.EquipmentTechnicsRequestID = b.EquipmentTechnicsRequestID
                                                                        LEFT OUTER JOIN PMIS_RES.TechnicsRequestCmdPositions c ON b.TechRequestsCommandID = c.TechRequestsCommandID
                                                                        LEFT OUTER JOIN PMIS_RES.TechRequestCmdPositionsMilDept d ON c.TechnicsRequestCmdPositionID = d.TechnicsRequestCmdPositionID
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
        public static List<ReportListTechnicsWithAppointDetailsBlock> GetReportListTechnicsWithAppointDetailsBlockList(int equipmentTechnicsRequestID, User currentUser)
        {
            ReportListTechnicsWithAppointDetailsBlock reportBlock;
            List<ReportListTechnicsWithAppointDetailsBlock> listReportBlock = new List<ReportListTechnicsWithAppointDetailsBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT i.TechnicsId,
                                      h.NK || ' ' || h.IMEES || ' ' || e.MilitaryCommandSuffix as MilitaryCommand,
                                      tt.TechnicsTypeName, 
                                      n.NormativeCode, n.NormativeName,
                                      g.TechnicReadinessID,
                                      CASE WHEN tt.TechnicsTypeKey = 'VEHICLES' THEN veh.RegNumber
                                           WHEN tt.TechnicsTypeKey = 'TRAILERS' THEN trl.RegNumber
                                           WHEN tt.TechnicsTypeKey = 'TRACTORS' THEN trc.RegNumber
                                           WHEN tt.TechnicsTypeKey = 'ENG_EQUIP' THEN eng.RegNumber
                                           WHEN tt.TechnicsTypeKey = 'MOB_LIFT_EQUIP' THEN mob.RegNumber
                                           ELSE '' 
                                      END as RegNumber,
                                      CASE WHEN tt.TechnicsTypeKey = 'VEHICLES' THEN veh.InventoryNumber
                                           WHEN tt.TechnicsTypeKey = 'TRAILERS' THEN trl.InventoryNumber
                                           WHEN tt.TechnicsTypeKey = 'TRACTORS' THEN trc.InventoryNumber
                                           WHEN tt.TechnicsTypeKey = 'ENG_EQUIP' THEN eng.InventoryNumber
                                           WHEN tt.TechnicsTypeKey = 'MOB_LIFT_EQUIP' THEN mob.InventoryNumber
                                           WHEN tt.TechnicsTypeKey = 'RAILWAY_EQUIP' THEN rail.InventoryNumber
                                           WHEN tt.TechnicsTypeKey = 'AVIATION_EQUIP' THEN av.AirInvNumber
                                           WHEN tt.TechnicsTypeKey = 'VESSELS' THEN ves.InventoryNumber
                                           WHEN tt.TechnicsTypeKey = 'FUEL_CONTAINERS' THEN fu.InventoryNumber
                                           ELSE '' 
                                      END as InvNumber,
                                      j.CompanyName, j.UnifiedIdentityCode,
                                      obl.IME_OBL || CASE WHEN obl.IME_OBL IS NULL THEN '' ELSE ', ' END || obs.IME_OBS || CASE WHEN cities.Ime_Nma IS NULL THEN '' ELSE ', ' END || cities.Ime_Nma as OwnershipCity,
                                      dis.DistrictName as OwnershipDistrict,
                                      j.Address as OwnershipAddress,
                                      j.Phone as OwnershipPhone
                               FROM PMIS_RES.EquipmentTechnicsRequests a
                               INNER JOIN PMIS_RES.TechnicsRequestCommands e ON a.EquipmentTechnicsRequestID = e.EquipmentTechnicsRequestID
                               INNER JOIN PMIS_RES.TechnicsRequestCmdPositions f ON e.TechRequestsCommandID = f.TechRequestsCommandID
                               INNER JOIN PMIS_RES.FulfilTechnicsRequest g ON f.TechnicsRequestCmdPositionID = g.TechnicsRequestCmdPositionID
                               INNER JOIN PMIS_RES.Technics i ON g.TechnicsID = i.TechnicsID
                               INNER JOIN PMIS_RES.TechnicsTypes tt ON tt.TechnicsTypeID = i.TechnicsTypeID
                               LEFT OUTER JOIN PMIS_RES.NormativeTechnics n ON n.NormativeTechnicsID = i.NormativeTechnicsID
                               LEFT OUTER JOIN UKAZ_OWNER.VVR h ON e.MilitaryCommandID = h.KOD_VVR 
                               LEFT OUTER JOIN PMIS_RES.Vehicles veh ON veh.TechnicsID = i.TechnicsID
                               LEFT OUTER JOIN PMIS_RES.Trailers trl ON trl.TechnicsID = i.TechnicsID
                               LEFT OUTER JOIN PMIS_RES.Tractors trc ON trc.TechnicsID = i.TechnicsID
                               LEFT OUTER JOIN PMIS_RES.EngEquipment eng ON eng.TechnicsID = i.TechnicsID
                               LEFT OUTER JOIN PMIS_RES.MobileLiftingEquip mob ON mob.TechnicsID = i.TechnicsID
                               LEFT OUTER JOIN PMIS_RES.RailWayEquips rail ON rail.TechnicsID = i.TechnicsID
                               LEFT OUTER JOIN PMIS_RES.AviationEquipment av ON av.TechnicsID = i.TechnicsID
                               LEFT OUTER JOIN PMIS_RES.Vessels ves ON ves.TechnicsID = i.TechnicsID
                               LEFT OUTER JOIN PMIS_RES.FuelContainers fu ON fu.TechnicsID = i.TechnicsID
                               LEFT OUTER JOIN PMIS_ADM.Companies j ON i.OwnershipCompanyID = j.CompanyID
                               LEFT OUTER JOIN UKAZ_OWNER.KL_NMA cities ON j.CityID = cities.Kod_Nma
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBS obs ON cities.KOD_OBS = obs.KOD_OBS
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBL obl ON cities.KOD_OBL = obl.KOD_OBL             
                               LEFT OUTER JOIN UKAZ_OWNER.Districts dis ON j.DistrictID = dis.DistrictID

                               WHERE a.EquipmentTechnicsRequestID = :EquipmentTechnicsRequestID
                               ORDER BY MilitaryCommand, i.TechnicsTypeID, TechnicReadinessID, RegNumber, InvNumber, OwnershipCity";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("EquipmentTechnicsRequestID", OracleType.Number).Value = equipmentTechnicsRequestID;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    reportBlock = new ReportListTechnicsWithAppointDetailsBlock(currentUser);

                    reportBlock.TechnicsId = DBCommon.GetInt(dr["TechnicsId"]);
                    reportBlock.MilitaryCommand = dr["MilitaryCommand"].ToString();
                    reportBlock.TechnicsType = dr["TechnicsTypeName"].ToString();
                    reportBlock.NormativeTechnics = (dr["NormativeCode"].ToString() + " " + dr["NormativeName"].ToString()).Trim();
                    reportBlock.Readiness = ReadinessUtil.ReadinessName(DBCommon.GetInt(dr["TechnicReadinessID"]));

                    string regNumber = dr["RegNumber"].ToString();
                    string invNumber = dr["InvNumber"].ToString();

                    reportBlock.RegInvNumber = regNumber + (!String.IsNullOrEmpty(regNumber) && !String.IsNullOrEmpty(invNumber) ? "/" : "") + invNumber;

                    string ownership = "";

                    if (!(dr["CompanyName"] is DBNull))
                    {
                        ownership = dr["UnifiedIdentityCode"].ToString() + " " + dr["CompanyName"].ToString();
                    }
                    else
                    {
                        ownership = "";
                    }

                    ownership += "<br/>";

                    ownership += dr["OwnershipCity"].ToString();

                    if (!(dr["OwnershipDistrict"] is DBNull))
                        ownership += ", " + dr["OwnershipDistrict"].ToString();

                    if (!(dr["OwnershipAddress"] is DBNull))
                        ownership += "<br/>" + dr["OwnershipAddress"].ToString();

                    if (!(dr["OwnershipPhone"] is DBNull))
                        ownership += "<br/>" + dr["OwnershipPhone"].ToString();

                    reportBlock.Owner = ownership;

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
