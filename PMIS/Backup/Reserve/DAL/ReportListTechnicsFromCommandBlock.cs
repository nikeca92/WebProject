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
    //This class represents all information about the filter, the order and the paging information on the screen
    public class ReportListTechnicsFromCommandFilter
    {
        string militaryDepartments;
        string militaryCommands;

        int orderBy;
        int pageIdx;
        int rowsPerPage;

        
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


        public string MilitaryCommands
        {
            get
            {
                return militaryCommands;
            }
            set
            {
                militaryCommands = value;
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

        public int RowsPerPage
        {
            get
            {
                return rowsPerPage;
            }
            set
            {
                rowsPerPage = value;
            }
        }
    }

    public class ReportListTechnicsFromCommandBlock : BaseDbObject
    {
        private int technicsId;
        private string militarySubCommand;
        private string technicsType;
        private string normativeTechnics;
        private string readiness;
        private string regInvNumber;
        private string owner;
        private string ownerAddress;

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

        public string MilitarySubCommand
        {
            get
            {
                return militarySubCommand;
            }
            set
            {
                militarySubCommand = value;
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

        public string OwnerAddress
        {
            get
            {
                return ownerAddress;
            }
            set
            {
                ownerAddress = value;
            }
        }

        public ReportListTechnicsFromCommandBlock (User user)
            : base(user)
        {
        }
    }

    public class ReportListTechnicsFromCommandBlockUtil
    {
        //This method get list of report items
        public static List<ReportListTechnicsFromCommandBlock> GetReportListTechnicsFromCommandBlockList(ReportListTechnicsFromCommandFilter filter, User currentUser)
        {
            ReportListTechnicsFromCommandBlock reportBlock;
            List<ReportListTechnicsFromCommandBlock> listReportBlock = new List<ReportListTechnicsFromCommandBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string whereClause = "";

                if (!String.IsNullOrEmpty(filter.MilitaryDepartments))
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " g.MilitaryDepartmentID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryDepartments) + ") ";
                }

                if (!String.IsNullOrEmpty(filter.MilitaryCommands))
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " e.MilitaryCommandID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryCommands) + ") ";
                }

                whereClause += (whereClause == "" ? "" : " AND ") +
                                    @" (/*Ticket #128*/
                                         (g.MilitaryDepartmentID IS NULL AND a.MilitaryUnitID IS NULL) OR 
                                         g.MilitaryDepartmentID IN (" + currentUser.MilitaryDepartmentIDs + @") OR
                                         a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @")
                                       ) ";

                whereClause = (whereClause == "" ? "" : " WHERE ") + whereClause;

                string pageWhere = "";

                if (filter.PageIdx > 0 && filter.RowsPerPage > 0)
                    pageWhere = " WHERE RowNumber BETWEEN (" + filter.PageIdx.ToString() + @" - 1) * " + filter.RowsPerPage.ToString() + @" + 1 AND " + filter.PageIdx.ToString() + @" * " + filter.RowsPerPage.ToString() + @" ";

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
                        orderBySQL = "e.MilitaryCommandSuffix || ' / ' || a.RequestNumber";
                        break;
                    case 2:
                        orderBySQL = "tt.TechnicsTypeName";
                        break;
                    case 3:
                        orderBySQL = "g.TechnicReadinessID";
                        break;
                    case 4:
                        orderBySQL = @"CASE WHEN tt.TechnicsTypeKey = 'VEHICLES' THEN veh.RegNumber
                                               WHEN tt.TechnicsTypeKey = 'TRAILERS' THEN trl.RegNumber
                                               WHEN tt.TechnicsTypeKey = 'TRACTORS' THEN trc.RegNumber
                                               WHEN tt.TechnicsTypeKey = 'ENG_EQUIP' THEN eng.RegNumber
                                               WHEN tt.TechnicsTypeKey = 'MOB_LIFT_EQUIP' THEN mob.RegNumber
                                               ELSE '' 
                                          END ||
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
                                          END";
                        break;
                    case 5:
                        orderBySQL = @"j.UnifiedIdentityCode || j.CompanyName ||
                                       obl.IME_OBL || CASE WHEN obl.IME_OBL IS NULL THEN '' ELSE ', ' END || obs.IME_OBS || CASE WHEN cities.Ime_Nma IS NULL THEN '' ELSE ', ' END || cities.Ime_Nma ||
                                       dis.DistrictName || j.Address || j.Phone";
                        break;
                    case 6:
                        orderBySQL = @"n.NormativeCode || n.NormativeName";
                        break;
                    default:
                        orderBySQL = "e.MilitaryCommandSuffix || ' / ' || a.RequestNumber";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                   SELECT i.TechnicsId,
                                          e.MilitaryCommandSuffix || ' / ' || a.RequestNumber as MilitarySubCommand,
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
                                          j.Phone as OwnershipPhone,
                                          DENSE_RANK() OVER (ORDER BY " + orderBySQL + @", 
                                                 CASE WHEN tt.TechnicsTypeKey = 'VEHICLES' THEN veh.RegNumber
                                                      WHEN tt.TechnicsTypeKey = 'TRAILERS' THEN trl.RegNumber
                                                      WHEN tt.TechnicsTypeKey = 'TRACTORS' THEN trc.RegNumber
                                                      WHEN tt.TechnicsTypeKey = 'ENG_EQUIP' THEN eng.RegNumber
                                                      WHEN tt.TechnicsTypeKey = 'MOB_LIFT_EQUIP' THEN mob.RegNumber
                                                      ELSE '' 
                                                 END,
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
                                                 END ) as RowNumber 
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

                                   " + whereClause + @"
                                   ORDER BY " + orderBySQL + @", 
                                             CASE WHEN tt.TechnicsTypeKey = 'VEHICLES' THEN veh.RegNumber
                                                  WHEN tt.TechnicsTypeKey = 'TRAILERS' THEN trl.RegNumber
                                                  WHEN tt.TechnicsTypeKey = 'TRACTORS' THEN trc.RegNumber
                                                  WHEN tt.TechnicsTypeKey = 'ENG_EQUIP' THEN eng.RegNumber
                                                  WHEN tt.TechnicsTypeKey = 'MOB_LIFT_EQUIP' THEN mob.RegNumber
                                                  ELSE '' 
                                             END,
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
                                             END 
                                ) tmp
                               " + pageWhere; ;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    reportBlock = new ReportListTechnicsFromCommandBlock(currentUser);

                    reportBlock.TechnicsId = DBCommon.GetInt(dr["TechnicsId"]);
                    reportBlock.MilitarySubCommand = dr["MilitarySubCommand"].ToString();
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

                    reportBlock.Owner = ownership;
                    
                    string owneradd = "";

                    owneradd += dr["OwnershipCity"].ToString();

                    if (!(dr["OwnershipDistrict"] is DBNull))
                        owneradd += ", " + dr["OwnershipDistrict"].ToString();

                    if (!(dr["OwnershipAddress"] is DBNull))
                        owneradd += "<br/>" + dr["OwnershipAddress"].ToString();

                    if (!(dr["OwnershipPhone"] is DBNull))
                        owneradd += "<br/>" + dr["OwnershipPhone"].ToString();

                    reportBlock.OwnerAddress = owneradd;

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

        public static int GetReportListTechnicsFromCommandBlockCount(ReportListTechnicsFromCommandFilter filter, User currentUser)
        {
            int cnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string whereClause = "";

                if (!String.IsNullOrEmpty(filter.MilitaryDepartments))
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " g.MilitaryDepartmentID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryDepartments) + ") ";
                }

                if (!String.IsNullOrEmpty(filter.MilitaryCommands))
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " e.MilitaryCommandID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryCommands) + ") ";
                }

                whereClause += (whereClause == "" ? "" : " AND ") +
                                    @" (/*Ticket #128*/
                                         (g.MilitaryDepartmentID IS NULL AND a.MilitaryUnitID IS NULL) OR 
                                         g.MilitaryDepartmentID IN (" + currentUser.MilitaryDepartmentIDs + @") OR
                                         a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @")
                                       ) ";

                whereClause = (whereClause == "" ? "" : " WHERE ") + whereClause;

                string SQL = @"SELECT COUNT(*) as Cnt
                               FROM PMIS_RES.EquipmentTechnicsRequests a
                               INNER JOIN PMIS_RES.TechnicsRequestCommands e ON a.EquipmentTechnicsRequestID = e.EquipmentTechnicsRequestID
                               INNER JOIN PMIS_RES.TechnicsRequestCmdPositions f ON e.TechRequestsCommandID = f.TechRequestsCommandID
                               INNER JOIN PMIS_RES.FulfilTechnicsRequest g ON f.TechnicsRequestCmdPositionID = g.TechnicsRequestCmdPositionID
                               " + whereClause + @"
                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    cnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return cnt;
        }
    }
}
