using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using System.Linq;

using PMIS.Common;
using System.Collections;

namespace PMIS.Reserve.Common
{
    //This class represents all information about the filter, the order and the paging information on the screen
    public class PrintTechnicsFilter
    {
        public string MilitaryDepartmentIds { get; set; }
        public int MilitaryCommandId { get; set; }
        public string MilitaryCommandSuffix { get; set; }
        public string RegNumber { get; set; }
        public int Readiness { get; set; }

        public int OrderBy { get; set; }
        public int PageIdx { get; set; }
    }

    public class PrintTechnicsResultBlock
    {
        public int TechnicsId { get; set; }
        public string TechnicsTypeName { get; set; }
        public string NormativeTechnics { get; set; }
        public string RegNumber { get; set; }
    }

    public class PrintTechnicsResult
    {
        private int rowsPerPage;
        private User currentUser;
        private PrintTechnicsFilter filter;

        public PrintTechnicsResult(int rowsPerPage, User currentUser)
        {
            this.rowsPerPage = rowsPerPage;
            this.currentUser = currentUser;
        }

        private int? allRecordsCount = null;
        public int AllRecordsCount 
        {
            get
            {
                if (!allRecordsCount.HasValue)
                {
                    allRecordsCount = PrintTechnicsUtil.GetPrintTechnicsResultsCount(filter, currentUser);
                }

                return allRecordsCount.Value;
            }
        }

        private List<PrintTechnicsResultBlock> overallResult = null;
        public List<PrintTechnicsResultBlock> OverallResult
        {
            get
            {
                if (overallResult == null)
                {
                    overallResult = PrintTechnicsUtil.GetPrintTechnicsResults(filter, rowsPerPage, currentUser);
                }

                return overallResult;
            }
        }

        public PrintTechnicsFilter Filter
        {
            get
            {
                return filter;
            }

            set
            {
                filter = value;
                allRecordsCount = null;
            }
        }
    }

    public static class PrintTechnicsUtil
    {
        public static List<PrintTechnicsResultBlock> GetPrintTechnicsResults(PrintTechnicsFilter filter, int rowsPerPage, User currentUser)
        {
            List<PrintTechnicsResultBlock> result = new List<PrintTechnicsResultBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string whereClause = "";

                if (!String.IsNullOrEmpty(filter.MilitaryDepartmentIds))
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " d.MilitaryDepartmentId IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryDepartmentIds) + ") ";
                }

                whereClause += (whereClause == "" ? "" : " AND ") +
                        @" (d.MilitaryDepartmentId IS NULL OR d.MilitaryDepartmentId IN ( " + currentUser.MilitaryDepartmentIDs + ")) ";


                if (filter.MilitaryCommandId > -1)
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " b.MilitaryCommandId = " + filter.MilitaryCommandId.ToString();

                }

                if (!String.IsNullOrEmpty(filter.MilitaryCommandSuffix) && filter.MilitaryCommandSuffix != "-1")
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " b.MilitaryCommandSuffix = '" + filter.MilitaryCommandSuffix + "' ";
                }

                if (!string.IsNullOrEmpty(filter.RegNumber))
                {
                    filter.RegNumber = filter.RegNumber.Trim();

                    if (filter.RegNumber.Length > 0)
                    {
                        whereClause += (whereClause == "" ? "(" : " AND (");

                        string[] regNumbers = filter.RegNumber.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                        for (int i = 0; i < regNumbers.Length; i++)
                        {
                            if (i < regNumbers.Length - 1)
                            {
                                whereClause += @" CASE g.TechnicsTypeKey
                                                    WHEN 'VEHICLES' THEN Upper(f1.RegNumber)
                                                    WHEN 'TRAILERS' THEN Upper(f2.RegNumber)
                                                    WHEN 'TRACTORS' THEN Upper(f3.RegNumber)
                                                    WHEN 'ENG_EQUIP' THEN Upper(f4.RegNumber)
                                                    WHEN 'MOB_LIFT_EQUIP' THEN Upper(f5.RegNumber)
                                                    WHEN 'RAILWAY_EQUIP' THEN Upper(f6.InventoryNumber)
                                                    WHEN 'AVIATION_EQUIP' THEN Upper(f7.AirInvNumber)
                                                    WHEN 'VESSELS' THEN Upper(f8.InventoryNumber)
                                                    WHEN 'FUEL_CONTAINERS' THEN Upper(f9.InventoryNumber)
                                                    ELSE ''
                                                END LIKE '%" + regNumbers[i].Replace("'", "''").ToUpper() + "%' OR ";
                            }
                            else
                            {
                                whereClause += @" CASE g.TechnicsTypeKey
                                                  WHEN 'VEHICLES' THEN Upper(f1.RegNumber)
                                                  WHEN 'TRAILERS' THEN Upper(f2.RegNumber)
                                                  WHEN 'TRACTORS' THEN Upper(f3.RegNumber)
                                                  WHEN 'ENG_EQUIP' THEN Upper(f4.RegNumber)
                                                  WHEN 'MOB_LIFT_EQUIP' THEN Upper(f5.RegNumber)
                                                  WHEN 'RAILWAY_EQUIP' THEN Upper(f6.InventoryNumber)
                                                  WHEN 'AVIATION_EQUIP' THEN Upper(f7.AirInvNumber)
                                                  WHEN 'VESSELS' THEN Upper(f8.InventoryNumber)
                                                  WHEN 'FUEL_CONTAINERS' THEN Upper(f9.InventoryNumber)
                                                  ELSE ''
                                              END LIKE '%" + regNumbers[i].Replace("'", "''").ToUpper() + "%') ";
                            }
                        }
                    }
                }

                if (filter.Readiness > -1)
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                         " d.TECHNICREADINESSID = " + filter.Readiness + " ";

                }

                whereClause = (whereClause == "" ? "" : " WHERE ") + whereClause;

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
                        orderBySQL = "g.TechnicsTypeName";
                        break;
                    case 2:
                        orderBySQL = "h.NormativeCode";
                        break;
                    case 3:
                        orderBySQL = @"CASE g.TechnicsTypeKey
                                            WHEN 'VEHICLES' THEN f1.RegNumber
                                            WHEN 'TRAILERS' THEN f2.RegNumber
                                            WHEN 'TRACTORS' THEN f3.RegNumber
                                            WHEN 'ENG_EQUIP' THEN f4.RegNumber
                                            WHEN 'MOB_LIFT_EQUIP' THEN f5.RegNumber
                                            WHEN 'RAILWAY_EQUIP' THEN f6.InventoryNumber
                                            WHEN 'AVIATION_EQUIP' THEN f7.AirInvNumber
                                            WHEN 'VESSELS' THEN f8.InventoryNumber
                                            WHEN 'FUEL_CONTAINERS' THEN f9.InventoryNumber
                                            ELSE ''
                                       END";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"
                               SELECT tmp.*
                               FROM 
                                  (
                                    SELECT f.TechnicsId,
                                           g.TechnicsTypeName,
                                           h.NormativeCode || ' ' || h.NormativeName as NormativeTechnics,
                                           CASE g.TechnicsTypeKey
                                                WHEN 'VEHICLES' THEN f1.RegNumber
                                                WHEN 'TRAILERS' THEN f2.RegNumber
                                                WHEN 'TRACTORS' THEN f3.RegNumber
                                                WHEN 'ENG_EQUIP' THEN f4.RegNumber
                                                WHEN 'MOB_LIFT_EQUIP' THEN f5.RegNumber
                                                WHEN 'RAILWAY_EQUIP' THEN f6.InventoryNumber
                                                WHEN 'AVIATION_EQUIP' THEN f7.AirInvNumber
                                                WHEN 'VESSELS' THEN f8.InventoryNumber
                                                WHEN 'FUEL_CONTAINERS' THEN f9.InventoryNumber
                                                ELSE ''
                                           END as RegNumber,
                                           RANK() OVER (ORDER BY " + orderBySQL + @", f.TechnicsId) as RowNumber 
                                    FROM UKAZ_OWNER.VVR a
                                    INNER JOIN PMIS_RES.TechnicsRequestCommands b ON b.MilitaryCommandId = a.KOD_VVR
                                    INNER JOIN PMIS_RES.TechnicsRequestCmdPositions c ON c.TechRequestsCommandID = b.TechRequestsCommandID
                                    INNER JOIN PMIS_RES.FulfilTechnicsRequest d ON d.TechnicsRequestCmdPositionID = c.TechnicsRequestCmdPositionID 
                                    INNER JOIN PMIS_RES.Technics f ON f.TechnicsId = d.TechnicsId
                                    INNER JOIN PMIS_RES.TechnicsTypes g ON f.TechnicsTypeID = g.TechnicsTypeID
                                    LEFT OUTER JOIN PMIS_RES.NormativeTechnics h ON f.NormativeTechnicsID = h.NormativeTechnicsID
                                    LEFT OUTER JOIN PMIS_RES.Vehicles f1 ON f.TechnicsID = f1.TechnicsID
                                    LEFT OUTER JOIN PMIS_RES.Trailers f2 ON f.TechnicsID = f2.TechnicsID
                                    LEFT OUTER JOIN PMIS_RES.Tractors f3 ON f.TechnicsID = f3.TechnicsID
                                    LEFT OUTER JOIN PMIS_RES.EngEquipment f4 ON f.TechnicsID = f4.TechnicsID
                                    LEFT OUTER JOIN PMIS_RES.MobileLiftingEquip f5 ON f.TechnicsID = f5.TechnicsID
                                    LEFT OUTER JOIN PMIS_RES.RailWayEquips f6 ON f.TechnicsID = f6.TechnicsID
                                    LEFT OUTER JOIN PMIS_RES.AviationEquipment f7 ON f.TechnicsID = f7.TechnicsID
                                    LEFT OUTER JOIN PMIS_RES.Vessels f8 ON f.TechnicsID = f8.TechnicsID
                                    LEFT OUTER JOIN PMIS_RES.FuelContainers f9 ON f.TechnicsID = f9.TechnicsID
                                    " + whereClause + @"
                                  ) tmp
                                " + pageWhere + @"
                                ORDER BY RowNumber
                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    PrintTechnicsResultBlock block = new PrintTechnicsResultBlock();
                    block.TechnicsId = DBCommon.GetInt(dr["TechnicsId"]);
                    block.TechnicsTypeName = dr["TechnicsTypeName"].ToString();
                    block.NormativeTechnics = dr["NormativeTechnics"].ToString();
                    block.RegNumber = dr["RegNumber"].ToString();

                    result.Add(block);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public static int GetPrintTechnicsResultsCount(PrintTechnicsFilter filter, User currentUser)
        {
            int cnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string whereClause = "";

                if (!String.IsNullOrEmpty(filter.MilitaryDepartmentIds))
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " d.MilitaryDepartmentId IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryDepartmentIds) + ") ";
                }

                whereClause += (whereClause == "" ? "" : " AND ") +
                        @" (d.MilitaryDepartmentId IS NULL OR d.MilitaryDepartmentId IN ( " + currentUser.MilitaryDepartmentIDs + ")) ";


                if (filter.MilitaryCommandId > -1)
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " b.MilitaryCommandId = " + filter.MilitaryCommandId.ToString();

                }

                if (!String.IsNullOrEmpty(filter.MilitaryCommandSuffix) && filter.MilitaryCommandSuffix != "-1")
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " b.MilitaryCommandSuffix = '" + filter.MilitaryCommandSuffix + "' ";
                }

                if (!string.IsNullOrEmpty(filter.RegNumber))
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                 @" CASE g.TechnicsTypeKey
                                        WHEN 'VEHICLES' THEN Upper(f1.RegNumber)
                                        WHEN 'TRAILERS' THEN Upper(f2.RegNumber)
                                        WHEN 'TRACTORS' THEN Upper(f3.RegNumber)
                                        WHEN 'ENG_EQUIP' THEN Upper(f4.RegNumber)
                                        WHEN 'MOB_LIFT_EQUIP' THEN Upper(f5.RegNumber)
                                        WHEN 'RAILWAY_EQUIP' THEN Upper(f6.InventoryNumber)
                                        WHEN 'AVIATION_EQUIP' THEN Upper(f7.AirInvNumber)
                                        WHEN 'VESSELS' THEN Upper(f8.InventoryNumber)
                                        WHEN 'FUEL_CONTAINERS' THEN Upper(f9.InventoryNumber)
                                        ELSE ''
                                    END LIKE '%" + filter.RegNumber.Replace("'", "''").ToUpper() + "%' ";
                }

                if (filter.Readiness > -1)
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                         " d.TECHNICREADINESSID = " + filter.Readiness + " ";

                }

                whereClause = (whereClause == "" ? "" : " WHERE ") + whereClause;

                string SQL = @"
                               SELECT COUNT(*) as Cnt
                               FROM UKAZ_OWNER.VVR a
                               INNER JOIN PMIS_RES.TechnicsRequestCommands b ON b.MilitaryCommandId = a.KOD_VVR
                               INNER JOIN PMIS_RES.TechnicsRequestCmdPositions c ON c.TechRequestsCommandID = b.TechRequestsCommandID
                               INNER JOIN PMIS_RES.FulfilTechnicsRequest d ON d.TechnicsRequestCmdPositionID = c.TechnicsRequestCmdPositionID 
                               INNER JOIN PMIS_RES.Technics f ON f.TechnicsId = d.TechnicsId
                               INNER JOIN PMIS_RES.TechnicsTypes g ON f.TechnicsTypeID = g.TechnicsTypeID
                               LEFT OUTER JOIN PMIS_RES.NormativeTechnics h ON f.NormativeTechnicsID = h.NormativeTechnicsID
                               LEFT OUTER JOIN PMIS_RES.Vehicles f1 ON f.TechnicsID = f1.TechnicsID
                               LEFT OUTER JOIN PMIS_RES.Trailers f2 ON f.TechnicsID = f2.TechnicsID
                               LEFT OUTER JOIN PMIS_RES.Tractors f3 ON f.TechnicsID = f3.TechnicsID
                               LEFT OUTER JOIN PMIS_RES.EngEquipment f4 ON f.TechnicsID = f4.TechnicsID
                               LEFT OUTER JOIN PMIS_RES.MobileLiftingEquip f5 ON f.TechnicsID = f5.TechnicsID
                               LEFT OUTER JOIN PMIS_RES.RailWayEquips f6 ON f.TechnicsID = f6.TechnicsID
                               LEFT OUTER JOIN PMIS_RES.AviationEquipment f7 ON f.TechnicsID = f7.TechnicsID
                               LEFT OUTER JOIN PMIS_RES.Vessels f8 ON f.TechnicsID = f8.TechnicsID
                               LEFT OUTER JOIN PMIS_RES.FuelContainers f9 ON f.TechnicsID = f9.TechnicsID
                              " + whereClause;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
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