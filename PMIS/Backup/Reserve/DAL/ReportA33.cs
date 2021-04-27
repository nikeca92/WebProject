using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using System.Linq;
using System.Collections;
using System.IO;
using System.Text;

using PMIS.Common;

namespace PMIS.Reserve.Common
{
    //This class represents all information about the filter, the order and the paging information on the screen
    public class ReportA33Filter
    {
        public string MilitaryDepartmentIds { get; set; }       
        public int PageIdx { get; set; }
        public int PageSize { get; set; }
    }

    public class ReportA33Result
    {
        public ReportA33Filter Filter { get; set; }

        public int MaxPage
        {
            get
            {
                return Filter.PageSize == 0 ? 1 : Rows.Count / Filter.PageSize + 1;
            }
        }

        public string[] HeaderCells { get; set; }
        public ArrayList Rows { get; set; }
    }

    public static class ReportA33Util
    {
        public static ReportA33Result GetReportA33(ReportA33Filter filter, User currentUser)
        {
            ReportA33Result reportResult = new ReportA33Result();
            reportResult.Filter = filter;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string emptySuffixString = "~=~";

                string whereClause = "";

                if (!String.IsNullOrEmpty(filter.MilitaryDepartmentIds))
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " <TABLE>.SourceMilDepartmentID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryDepartmentIds) + ") ";
                }

                whereClause += (whereClause == "" ? "" : " AND ") +
                         @" (<TABLE>.SourceMilDepartmentID IS NULL OR <TABLE>.SourceMilDepartmentID IN ( " + currentUser.MilitaryDepartmentIDs + ")) ";

                whereClause = (whereClause == "" ? "" : " WHERE ") + whereClause;


                string whereClause2 = "";

                if (!String.IsNullOrEmpty(filter.MilitaryDepartmentIds))
                {
                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                                    " a.MilitaryDepartmentID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryDepartmentIds) + ") ";
                }

                whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                         @" (a.MilitaryDepartmentID IS NULL OR a.MilitaryDepartmentID IN ( " + currentUser.MilitaryDepartmentIDs + ")) ";

                whereClause2 = (whereClause2 == "" ? "" : " WHERE ") + whereClause2;

                string SQL = @"
SELECT StatusOrder, StatusID, StatusName, ColumnOrder, ColumnLabel, ColumnValue
FROM
(
    SELECT s.StatusID, s.StatusOrder, s.StatusName, 0 as ColumnOrder, ' ' as ColumnLabel, s.Section as ColumnValue
	FROM PMIS_RES.ViewA33 s
	
	UNION ALL

    SELECT s.StatusID, s.StatusOrder, s.StatusName, 1 as ColumnOrder, 'Ред' as ColumnLabel, TO_CHAR(s.StatusOrder) as ColumnValue
	FROM PMIS_RES.ViewA33 s
	
	UNION ALL

	SELECT s.StatusID, s.StatusOrder, s.StatusName, 2 as ColumnOrder, 'Състояние' as ColumnLabel, s.StatusName as ColumnValue
	FROM PMIS_RES.ViewA33 s

	UNION ALL

	SELECT s.StatusID, s.StatusOrder, s.StatusName, 2 + k.ColumnOrder as ColumnOrder, k.ColumnLabel as ColumnLabel, 
	       TO_CHAR(SUM(CASE WHEN r.MilitaryRankCategory = k.MilitaryRankCategory AND
		                         (
								   (s.StatusID = 1 AND r.MilitaryReportStatusKey <> 'DISCHARGED') OR
								   (s.StatusID = 2 AND r.MilitaryReportStatusKey = 'COMPULSORY_RESERVE_MOB_APPOINTMENT' AND
								    r.ReservistReadinessID = 1) OR
								   (r.MilitaryReportStatusKey = 'COMPULSORY_RESERVE_MOB_APPOINTMENT' AND
								    r.ReservistReadinessID = 1 AND
									s.MilReadinessID = r.MilReadinessID) OR
								   (s.StatusID = 3 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness) AND 
								    r.MilitaryReportStatusKey = 'COMPULSORY_RESERVE_MOB_APPOINTMENT' AND
								    r.ReservistReadinessID = 2) OR
								   (s.StatusID = 4 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness) AND 
								    r.MilitaryReportStatusKey = 'POSTPONED') OR
								   (s.StatusID = 5 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness) AND 
								    r.MilitaryReportStatusKey = 'TEMPORARY_REMOVED') OR	
								   (s.StatusID = 6 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness) AND 
								    r.MilitaryReportStatusKey = 'FREE') OR
								   (s.StatusID = 7 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness) AND 
								    r.MilitaryReportStatusKey = 'TEMPORARY_REMOVED' AND
									r.TemporaryRemoved_Reason = 'НГВС') OR
								   (s.StatusID = 8 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness) AND 
								    r.MilitaryReportStatusKey = 'COMPULSORY_RESERVE_MOB_APPOINTMENT' AND
								    r.ReservistReadinessID = 1 AND
									r.TechRequestsCommandId IS NOT NULL
									)  OR
								   (r.MilitaryReportStatusKey = 'COMPULSORY_RESERVE_MOB_APPOINTMENT' AND
								    r.ReservistReadinessID = 1 AND
                                    s.AdministrationID > 0 AND 
								    s.AdministrationID = r.AdministrationID) OR
								   (s.StatusID = 9 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness) + (SELECT COUNT(*) FROM PMIS_ADM.Administrations) AND 
								    NVL(r.PunktCnt, 0) = 0 AND
									r.MilitaryReportStatusKey = 'COMPULSORY_RESERVE_MOB_APPOINTMENT' AND
                                    r.ReservistReadinessID = 1) OR
								   (s.StatusID = 10 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness) + (SELECT COUNT(*) FROM PMIS_ADM.Administrations) AND 
								    NVL(r.PunktCnt, 0) > 0 AND
									r.MilitaryReportStatusKey = 'COMPULSORY_RESERVE_MOB_APPOINTMENT' AND
                                    r.ReservistReadinessID = 1)
								 )
		                    THEN 1 
							ELSE 0 
					   END)) as ColumnValue
	FROM PMIS_RES.ViewA33 s
	CROSS JOIN (SELECT 'OP' as ColumnLabel, 1 as ColumnOrder, 2 as MilitaryRankCategory FROM dual
				UNION ALL
				SELECT 'CBP' as ColumnLabel, 2 as ColumnOrder, 1 as MilitaryRankCategory FROM dual
			   ) k
    LEFT OUTER JOIN (
	    SELECT m.MilitaryReportStatusKey, f.ReservistReadinessID, c.MilReadinessID, i.MilitaryRankCategory, gt.TableValue as TemporaryRemoved_Reason,
		       req.AdministrationID, rpunkt.Cnt as PunktCnt,
               MAX(ct.TechRequestsCommandId) as TechRequestsCommandId /*If a person is a driver on more than one technics*/
		FROM PMIS_RES.RESERVISTS e
		LEFT OUTER JOIN VS_OWNER.VS_LS g ON g.PersonId = e.PersonId
		LEFT OUTER JOIN VS_OWNER.KLV_ZVA h ON h.ZVA_KOD = g.KOD_ZVA
		LEFT OUTER JOIN PMIS_ADM.MilitaryRankCategories i ON i.ZVA_KAT_KOD = h.ZVA_KAT_KOD
		LEFT OUTER JOIN PMIS_RES.ReservistMilRepStatuses r ON e.ReservistID = r.ReservistID AND r.IsCurrent = 1
		LEFT OUTER JOIN PMIS_RES.GTable gt ON gt.TableName = 'MilRepStat_ТemporaryRemovedReasons' AND gt.TableKey = r.TemporaryRemoved_ReasonID
		LEFT OUTER JOIN PMIS_RES.MilitaryReportStatuses m ON r.MilitaryReportStatusID = m.MilitaryReportStatusID
		LEFT OUTER JOIN PMIS_RES.FillReservistsRequest f ON e.ReservistID = f.ReservistID
		LEFT OUTER JOIN PMIS_RES.RequestCommandPositions p ON f.RequestCommandPositionID = p.RequestCommandPositionID
		LEFT OUTER JOIN PMIS_RES.RequestsCommands c ON p.RequestsCommandID = c.RequestsCommandID
		LEFT OUTER JOIN PMIS_RES.EquipmentReservistsRequests req ON c.EquipmentReservistsRequestID = req.EquipmentReservistsRequestID
		LEFT OUTER JOIN (SELECT COUNT(*) as Cnt, a.RequestCommandID
		                 FROM PMIS_RES.RequestCommandPunkt a
						 " + whereClause2 + (whereClause2 == "" ? " WHERE " : " AND ") + @" a.CityID IS NOT NULL OR a.Place IS NOT NULL
						 GROUP BY a.RequestCommandID) rpunkt ON rpunkt.RequestCommandID = c.RequestsCommandID
		LEFT OUTER JOIN PMIS_RES.Technics th ON th.DriverReservistID = e.ReservistID
		LEFT OUTER JOIN PMIS_RES.FulfilTechnicsRequest ft ON ft.TechnicsId = th.TechnicsId  
		LEFT OUTER JOIN PMIS_RES.TechnicsRequestCmdPositions pt ON pt.TechnicsRequestCmdPositionId = ft.TechnicsRequestCmdPositionId
		LEFT OUTER JOIN PMIS_RES.TechnicsRequestCommands ct ON ct.TechRequestsCommandId = pt.TechRequestsCommandId AND
		                                                       c.MilitaryCommandId = ct.MilitaryCommandId AND NVL(c.MilitaryCommandSuffix, :EmptySuffixString) = NVL(ct.MilitaryCommandSuffix, :EmptySuffixString) AND c.MilReadinessID = ct.MilReadinessID
        " + whereClause.Replace("<TABLE>", "r") + @"
        GROUP BY m.MilitaryReportStatusKey, f.ReservistReadinessID, c.MilReadinessID, i.MilitaryRankCategory, gt.TableValue,
		         req.AdministrationID, rpunkt.Cnt
		) r ON 1 = 1
	GROUP BY s.StatusID, s.StatusOrder, s.StatusName, k.ColumnOrder, k.ColumnLabel
				  
    UNION ALL

	SELECT s.StatusID, s.StatusOrder, s.StatusName, 5 + (DENSE_RANK() OVER (ORDER BY g.TableSeq)) as ColumnOrder, g.TableValue as ColumnLabel, 
	       TO_CHAR(SUM(CASE WHEN g.TableKey = v.VehicleKindID AND
		                         (
								   (s.StatusID = 1 AND v.TechMilitaryReportStatusKey <> 'EXEMPT') OR
								   (s.StatusID = 2 AND v.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND
								    v.TechnicReadinessID = 1) OR
								   (v.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND
								    v.TechnicReadinessID = 1 AND
									s.MilReadinessID = v.MilReadinessID) OR
								   (s.StatusID = 3 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness) AND 
								    v.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND
								    v.TechnicReadinessID = 2) OR
								   (s.StatusID = 4 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness) AND 
								    v.TechMilitaryReportStatusKey = 'EXEMPT') OR
								   (s.StatusID = 5 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness) AND 
								    1 = 2) OR	
								   (s.StatusID = 6 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness) AND 
								    v.TechMilitaryReportStatusKey = 'FREE') OR
								   (s.StatusID = 7 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness) AND 
								    1 = 2) OR
								   (s.StatusID = 8 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness) AND 
								    v.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND
								    v.TechnicReadinessID = 1 AND
									v.RequestsCommandID IS NOT NULL
									) OR
								   (v.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND
								    v.TechnicReadinessID = 1 AND
                                    s.AdministrationID > 0 AND 
								    s.AdministrationID = v.AdministrationID) OR
								   (s.StatusID = 9 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness) + (SELECT COUNT(*) FROM PMIS_ADM.Administrations) AND 
								    NVL(v.PunktCnt, 0) = 0 AND
									v.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND
                                    v.TechnicReadinessID = 1) OR
								   (s.StatusID = 10 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness) + (SELECT COUNT(*) FROM PMIS_ADM.Administrations) AND 
								    NVL(v.PunktCnt, 0) > 0 AND
									v.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND
                                    v.TechnicReadinessID = 1)
								 )
		                    THEN 1 
							ELSE 0 
					   END)) as ColumnValue
	FROM PMIS_RES.ViewA33 s
	LEFT OUTER JOIN PMIS_RES.GTABLE g ON g.TableName = 'VehicleKind'
	LEFT OUTER JOIN (
	    SELECT v.VehicleKindID, ts.TechMilitaryReportStatusKey, ft.TechnicReadinessID, ct.MilReadinessID,
		       c.RequestsCommandID, tr.AdministrationID, tpunkt.Cnt as PunktCnt
		FROM PMIS_RES.Vehicles v
		INNER JOIN PMIS_RES.Technics t ON v.TechnicsID = t.TechnicsID
		LEFT OUTER JOIN PMIS_RES.TechnicsMilRepStatus s ON t.TechnicsID = s.TechnicsID AND s.IsCurrent = 1
		LEFT OUTER JOIN PMIS_RES.TechMilitaryReportStatuses ts ON s.TechMilitaryReportStatusID = ts.TechMilitaryReportStatusID
		LEFT OUTER JOIN PMIS_RES.FulfilTechnicsRequest ft ON v.TechnicsID = ft.TechnicsID
		LEFT OUTER JOIN PMIS_RES.TechnicsRequestCmdPositions pt ON pt.TechnicsRequestCmdPositionId = ft.TechnicsRequestCmdPositionId
		LEFT OUTER JOIN PMIS_RES.TechnicsRequestCommands ct ON ct.TechRequestsCommandId = pt.TechRequestsCommandId
		LEFT OUTER JOIN PMIS_RES.EquipmentTechnicsRequests tr ON tr.EquipmentTechnicsRequestID = ct.EquipmentTechnicsRequestID
		LEFT OUTER JOIN (SELECT COUNT(*) as Cnt, a.TechRequestsCommandId
		                 FROM PMIS_RES.TechRequestCommandPunkt a
						 " + whereClause2 + (whereClause2 == "" ? " WHERE " : " AND ") + @" a.CityID IS NOT NULL OR a.Place IS NOT NULL
						 GROUP BY a.TechRequestsCommandId) tpunkt ON tpunkt.TechRequestsCommandId = ct.TechRequestsCommandId
		LEFT OUTER JOIN PMIS_RES.FillReservistsRequest f ON t.DriverReservistID = f.ReservistID
		LEFT OUTER JOIN PMIS_RES.RequestCommandPositions p ON f.RequestCommandPositionID = p.RequestCommandPositionID
		LEFT OUTER JOIN PMIS_RES.RequestsCommands c ON p.RequestsCommandID = c.RequestsCommandID AND
		                                               c.MilitaryCommandId = ct.MilitaryCommandId AND NVL(c.MilitaryCommandSuffix, :EmptySuffixString) = NVL(ct.MilitaryCommandSuffix, :EmptySuffixString) AND c.MilReadinessID = ct.MilReadinessID
        " + whereClause.Replace("<TABLE>", "s") + @"
	    ) v ON 1 = 1
	GROUP BY s.StatusID, s.StatusOrder, s.StatusName, g.TableSeq, g.TableValue
	
	UNION ALL

	SELECT s.StatusID, s.StatusOrder, s.StatusName, 5 + (SELECT COUNT(*) FROM PMIS_RES.GTABLE WHERE TableName = 'VehicleKind') + (DENSE_RANK() OVER (ORDER BY tt.Seq)) as ColumnOrder, tt.TechnicsTypeName as ColumnLabel, 
	       TO_CHAR(SUM(CASE WHEN tt.TechnicsTypeID = t.TechnicsTypeID AND
		                         (
								   (s.StatusID = 1 AND t.TechMilitaryReportStatusKey <> 'EXEMPT') OR
								   (s.StatusID = 2 AND t.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND
								    t.TechnicReadinessID = 1) OR
								   (t.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND
								    t.TechnicReadinessID = 1 AND
									s.MilReadinessID = t.MilReadinessID) OR
								   (s.StatusID = 3 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness) AND 
								    t.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND
								    t.TechnicReadinessID = 2) OR
								   (s.StatusID = 4 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness) AND 
								    t.TechMilitaryReportStatusKey = 'EXEMPT') OR
								   (s.StatusID = 5 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness) AND 
								    1 = 2) OR	
								   (s.StatusID = 6 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness) AND 
								    t.TechMilitaryReportStatusKey = 'FREE') OR
								   (s.StatusID = 7 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness) AND 
								    1 = 2) OR
								   (s.StatusID = 8 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness) AND 
								    t.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND
								    t.TechnicReadinessID = 1 AND
									t.RequestsCommandID IS NOT NULL
									)  OR
								   (t.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND
								    t.TechnicReadinessID = 1 AND
                                    s.AdministrationID > 0 AND 
								    s.AdministrationID = t.AdministrationID) OR
								   (s.StatusID = 9 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness) + (SELECT COUNT(*) FROM PMIS_ADM.Administrations) AND 
								    NVL(t.PunktCnt, 0) = 0 AND
									t.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND
                                    t.TechnicReadinessID = 1 ) OR
								   (s.StatusID = 10 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness) + (SELECT COUNT(*) FROM PMIS_ADM.Administrations) AND 
								    NVL(t.PunktCnt, 0) > 0 AND
									t.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND
                                    t.TechnicReadinessID = 1)
								 )
		                    THEN t.ItemsCount 
							ELSE 0 
					   END)) as ColumnValue
	FROM PMIS_RES.ViewA33 s
	CROSS JOIN PMIS_RES.TechnicsTypes tt
	LEFT OUTER JOIN (
	    SELECT t.TechnicsTypeID, ts.TechMilitaryReportStatusKey, ft.TechnicReadinessID, ct.MilReadinessID,
		       c.RequestsCommandID, tr.AdministrationID, tpunkt.Cnt as PunktCnt, t.ItemsCount
		FROM PMIS_RES.Technics t
		INNER JOIN PMIS_RES.TechnicsTypes tt ON t.TechnicsTypeID = tt.TechnicsTypeID
		LEFT OUTER JOIN PMIS_RES.TechnicsMilRepStatus s ON t.TechnicsID = s.TechnicsID AND s.IsCurrent = 1
		LEFT OUTER JOIN PMIS_RES.TechMilitaryReportStatuses ts ON s.TechMilitaryReportStatusID = ts.TechMilitaryReportStatusID
		LEFT OUTER JOIN PMIS_RES.FulfilTechnicsRequest ft ON t.TechnicsID = ft.TechnicsID
		LEFT OUTER JOIN PMIS_RES.TechnicsRequestCmdPositions pt ON pt.TechnicsRequestCmdPositionId = ft.TechnicsRequestCmdPositionId
		LEFT OUTER JOIN PMIS_RES.TechnicsRequestCommands ct ON ct.TechRequestsCommandId = pt.TechRequestsCommandId
		LEFT OUTER JOIN PMIS_RES.EquipmentTechnicsRequests tr ON tr.EquipmentTechnicsRequestID = ct.EquipmentTechnicsRequestID
		LEFT OUTER JOIN (SELECT COUNT(*) as Cnt, a.TechRequestsCommandId
		                 FROM PMIS_RES.TechRequestCommandPunkt a
						 " + whereClause2 + (whereClause2 == "" ? " WHERE " : " AND ") + @" a.CityID IS NOT NULL OR a.Place IS NOT NULL
						 GROUP BY a.TechRequestsCommandId) tpunkt ON tpunkt.TechRequestsCommandId = ct.TechRequestsCommandId
		LEFT OUTER JOIN PMIS_RES.FillReservistsRequest f ON t.DriverReservistID = f.ReservistID
		LEFT OUTER JOIN PMIS_RES.RequestCommandPositions p ON f.RequestCommandPositionID = p.RequestCommandPositionID
		LEFT OUTER JOIN PMIS_RES.RequestsCommands c ON p.RequestsCommandID = c.RequestsCommandID AND
		                                               c.MilitaryCommandId = ct.MilitaryCommandId AND NVL(c.MilitaryCommandSuffix, :EmptySuffixString) = NVL(ct.MilitaryCommandSuffix, :EmptySuffixString) AND c.MilReadinessID = ct.MilReadinessID
        " + whereClause.Replace("<TABLE>", "s") + (whereClause == "" ? " WHERE " : " AND ") + @" tt.TechnicsTypeKey <> 'VEHICLES'
	    ) t ON 1 = 1
    WHERE tt.TechnicsTypeKey <> 'VEHICLES' AND NVL(tt.Active, 0) = 1
	GROUP BY s.StatusID, s.StatusOrder, s.StatusName, tt.TechnicsTypeID, tt.Seq, tt.TechnicsTypeName
) s
ORDER BY StatusOrder, StatusID, ColumnOrder, ColumnLabel
";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("EmptySuffixString", OracleType.VarChar).Value = emptySuffixString;

                if (Config.GetWebSetting("ReportA33LogSQL").ToLower() == "true")
                {
                    StreamWriter sw = new StreamWriter(Config.GetWebSetting("ReportA33LogSQLFile"), false, Encoding.UTF8);
                    sw.WriteLine("EmptySuffixString = '" + emptySuffixString + "'");
                    sw.WriteLine("");
                    sw.WriteLine("---------------");
                    sw.WriteLine("");
                    sw.WriteLine(SQL);
                    sw.Close();
                }

                OracleDataReader dr = cmd.ExecuteReader();

                bool isFirstRow = true;
                ArrayList headerCellsList = new ArrayList();
                ArrayList rowsList = new ArrayList();

                ArrayList tmpRow = new ArrayList();
                int oldStatusID = -1;
                
                while (dr.Read())
                {
                    int statusId = int.Parse(dr["StatusID"].ToString());
                    string statusName = (string)dr["StatusName"];
                    int columnOrder = int.Parse(dr["ColumnOrder"].ToString());
                    string columnLabel = (string)dr["ColumnLabel"];
                    string columnValue = (string)dr["ColumnValue"];

                    if (oldStatusID != -1 && oldStatusID != statusId)
                    { 
                        //new row
                        isFirstRow = false;

                        rowsList.Add((string[])tmpRow.ToArray(typeof(string)));
                        tmpRow.Clear();
                    }

                    oldStatusID = statusId;                   

                    if (isFirstRow)
                    {
                        headerCellsList.Add(columnLabel);
                    }

                    tmpRow.Add(columnValue);
                }

                dr.Close();

                if (tmpRow.Count > 0)
                {
                    rowsList.Add((string[])tmpRow.ToArray(typeof(string)));
                }
                
                tmpRow.Clear();

                reportResult.HeaderCells = (string[])headerCellsList.ToArray(typeof(string));
                reportResult.Rows = rowsList;
            }
            finally
            {
                conn.Close();
            }
           
            return reportResult;
        }
    }
}