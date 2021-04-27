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
    public class ReportA33v2Filter
    {
        public string MilitaryDepartmentIds { get; set; }  
        
        public string Region { get; set; }
        public string Municipality { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string PostCode { get; set; }
        public string Address { get; set; }
        
        public int PageIdx { get; set; }
        public int PageSize { get; set; }
    }

    public class ReportA33v2Result
    {
        public ReportA33v2Filter Filter { get; set; }

        public int MaxPage
        {
            get
            {
                return Filter.PageSize == 0 ? 1 : Rows.Count / Filter.PageSize + 1;
            }
        }

        public List<string> HeaderCells { get; set; }
        public List<ReportA33v2Row> Rows { get; set; }
    }

    public class ReportA33v2Row
    {
        public bool IsEmptyRow { get; set; }
        public List<string> ColumnValues { get; set; }
    }

    public static class ReportA33v2Util
    {
        public static ReportA33v2Result GetReportA33v2(ReportA33v2Filter filter, User currentUser)
        {
            ReportA33v2Result reportResult = new ReportA33v2Result();
            reportResult.Filter = filter;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string emptySuffixString = "~=~";

                string whereClause_Common = "";

                if (!String.IsNullOrEmpty(filter.MilitaryDepartmentIds))
                {
                    whereClause_Common += (whereClause_Common == "" ? "" : " AND ") +
                                    " <TABLE>.SourceMilDepartmentID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryDepartmentIds) + ") ";
                }

                whereClause_Common += (whereClause_Common == "" ? "" : " AND ") +
                         @" (<TABLE>.SourceMilDepartmentID IS NULL OR <TABLE>.SourceMilDepartmentID IN ( " + currentUser.MilitaryDepartmentIDs + ")) ";
                
                string whereClause_Persons = whereClause_Common;
                if (!string.IsNullOrEmpty(filter.Region))
                {
                    whereClause_Persons += (whereClause_Persons == "" ? "" : " AND ") +
                                         @" g.KOD_NMA_MJ IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBL IN ( " + filter.Region + ") ) ";
                }

                if (!string.IsNullOrEmpty(filter.Municipality))
                {
                    whereClause_Persons += (whereClause_Persons == "" ? "" : " AND ") +
                                         @" g.KOD_NMA_MJ IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBS IN ( " + filter.Municipality + ") ) ";
                }

                if (!string.IsNullOrEmpty(filter.City))
                {
                    whereClause_Persons += (whereClause_Persons == "" ? "" : " AND ") +
                                         @" g.KOD_NMA_MJ IN ( " + filter.City + ") ";
                }

                if (!string.IsNullOrEmpty(filter.District))
                {
                    whereClause_Persons += (whereClause_Persons == "" ? "" : " AND ") +
                                         @" g.PermAddrDistrictID IN ( " + filter.District + ") ";
                }

                if (!string.IsNullOrEmpty(filter.Address))
                {
                    whereClause_Persons += (whereClause_Persons == "" ? "" : " AND ") +
                                         " UPPER(g.ADRES) LIKE UPPER('%" + filter.Address.Replace("'", "''") + "%') ";
                }

                if (!string.IsNullOrEmpty(filter.PostCode))
                {
                    whereClause_Persons += (whereClause_Persons == "" ? "" : " AND ") +
                                         @" g.PermSecondPostCode LIKE '%" + filter.PostCode.Replace("'", "''") + "%' ";
                }
                
                string whereClause_Technics = whereClause_Common;
                if (!string.IsNullOrEmpty(filter.Region))
                {
                    whereClause_Technics += (whereClause_Technics == "" ? "" : " AND ") +
                                         @" com.CityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBL IN ( " + filter.Region + ") ) ";
                }

                if (!string.IsNullOrEmpty(filter.Municipality))
                {
                    whereClause_Technics += (whereClause_Technics == "" ? "" : " AND ") +
                                         @" com.CityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBS IN ( " + filter.Municipality + ") ) ";
                }

                if (!string.IsNullOrEmpty(filter.City))
                {
                    whereClause_Technics += (whereClause_Technics == "" ? "" : " AND ") +
                                         @" com.CityID IN ( " + filter.City + ") ";
                }

                if (!string.IsNullOrEmpty(filter.District))
                {
                    whereClause_Technics += (whereClause_Technics == "" ? "" : " AND ") +
                                         @" com.DistrictID IN ( " + filter.District + ") ";
                }

                if (!string.IsNullOrEmpty(filter.Address))
                {
                    whereClause_Technics += (whereClause_Technics == "" ? "" : " AND ") +
                                         " UPPER(com.Address) LIKE UPPER('%" + filter.Address.Replace("'", "''") + "%') ";
                }

                if (!string.IsNullOrEmpty(filter.PostCode))
                {
                    whereClause_Technics += (whereClause_Technics == "" ? "" : " AND ") +
                                         @" com.PostCode LIKE '%" + filter.PostCode.Replace("'", "''") + "%' ";
                }

                whereClause_Persons = (whereClause_Persons == "" ? "" : " WHERE ") + whereClause_Persons;
                whereClause_Technics = (whereClause_Technics == "" ? "" : " WHERE ") + whereClause_Technics;
                
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
SELECT StatusOrder, StatusID, StatusName, ColumnOrder, ColumnLabel, ColumnValue, IsEmptyRow
FROM
(
    SELECT s.StatusID, s.StatusOrder, s.StatusName, 0 as ColumnOrder, 'Ред' as ColumnLabel, s.RowNumber as ColumnValue, s.IsEmptyRow as IsEmptyRow
	FROM PMIS_RES.ViewA33v2 s
	
	UNION ALL

    SELECT s.StatusID, s.StatusOrder, s.StatusName, 1 as ColumnOrder, 'Резерв на въоръжените сили' as ColumnLabel, s.StatusName as ColumnValue, s.IsEmptyRow as IsEmptyRow
	FROM PMIS_RES.ViewA33v2 s

    UNION ALL

	SELECT s.StatusID, s.StatusOrder, s.StatusName, 2 + k.ColumnOrder as ColumnOrder, k.ColumnLabel as ColumnLabel, 
	       TO_CHAR(SUM(CASE WHEN r.MilitaryRankCategory IN (k.MilitaryRankCategory) AND r.MilitaryRankSubCategory IN (k.MilitaryRankSubCategory) AND
		                         (
								   (s.StatusID = 1 AND 
                                      (
                                         r.MilitaryReportStatusKey = 'VOLUNTARY_RESERVE' OR
                                       
                                         (
                                          (r.MilitaryReportStatusKey = 'COMPULSORY_RESERVE_MOB_APPOINTMENT' AND NVL(r.AppointmentIsDelivered, 0) = 1 AND
								           r.ReservistReadinessID = 2) OR
                                          (r.MilitaryReportStatusKey = 'COMPULSORY_RESERVE_MOB_APPOINTMENT' AND NVL(r.AppointmentIsDelivered, 0) = 1 AND
								           r.ReservistReadinessID = 1) OR
                                          
                                          r.MilitaryReportStatusKey = 'POSTPONED' OR
                                          r.MilitaryReportStatusKey = 'TEMPORARY_REMOVED' OR
                                          r.MilitaryReportStatusKey = 'FREE'
                                         )
                                      )
                                   ) OR

								   (s.StatusID = 2 AND r.MilitaryReportStatusKey = 'VOLUNTARY_RESERVE') OR

								   (s.StatusID = 3 AND 
                                      (
                                         (r.MilitaryReportStatusKey = 'COMPULSORY_RESERVE_MOB_APPOINTMENT' AND NVL(r.AppointmentIsDelivered, 0) = 1 AND
								          r.ReservistReadinessID = 2) OR
                                         (r.MilitaryReportStatusKey = 'COMPULSORY_RESERVE_MOB_APPOINTMENT' AND NVL(r.AppointmentIsDelivered, 0) = 1 AND
								          r.ReservistReadinessID = 1) OR
                                          
                                         r.MilitaryReportStatusKey = 'POSTPONED' OR
                                         r.MilitaryReportStatusKey = 'TEMPORARY_REMOVED' OR
                                         r.MilitaryReportStatusKey = 'FREE'
                                      )
                                   ) OR
                                
								   (r.MilitaryReportStatusKey = 'COMPULSORY_RESERVE_MOB_APPOINTMENT' AND NVL(r.AppointmentIsDelivered, 0) = 1 AND
								    r.ReservistReadinessID = 1 AND
									s.MilReadinessID = r.MilReadinessID AND
                                    s.AdministrationGroup IS NULL AND
                                    r.IsMinistryOfDefence = NVL(s.IsMinistryOfDefence, 0)) OR
                                 
                                   (s.StatusID = 5 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness WHERE MilReadinessCmdType IS NOT NULL) AND 
								    r.MilitaryReportStatusKey = 'COMPULSORY_RESERVE_MOB_APPOINTMENT' AND NVL(r.AppointmentIsDelivered, 0) = 1 AND
								    r.ReservistReadinessID = 2 AND
                                    r.IsMinistryOfDefence = NVL(s.IsMinistryOfDefence, 0)) OR
								   
                                   (s.StatusID = 6 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness WHERE MilReadinessCmdType IS NOT NULL) AND 
								    NVL(r.PunktCnt, 0) = 0 AND
									r.MilitaryReportStatusKey = 'COMPULSORY_RESERVE_MOB_APPOINTMENT' AND NVL(r.AppointmentIsDelivered, 0) = 1 AND
                                    r.ReservistReadinessID = 1 AND
                                    r.IsMinistryOfDefence = NVL(s.IsMinistryOfDefence, 0)) OR

                                   (s.StatusID = 7 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness WHERE MilReadinessCmdType IS NOT NULL) AND 
								    NVL(r.PunktCnt, 0) > 0 AND
									r.MilitaryReportStatusKey = 'COMPULSORY_RESERVE_MOB_APPOINTMENT' AND NVL(r.AppointmentIsDelivered, 0) = 1 AND
                                    r.ReservistReadinessID = 1 AND
                                    r.IsMinistryOfDefence = NVL(s.IsMinistryOfDefence, 0)) OR

                                   (s.StatusID = 8 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness WHERE MilReadinessCmdType IS NOT NULL) AND 
								    r.MilitaryReportStatusKey = 'COMPULSORY_RESERVE_MOB_APPOINTMENT' AND NVL(r.AppointmentIsDelivered, 0) = 1 AND
								    r.ReservistReadinessID = 1 AND                                  
                                    r.IsMinistryOfDefence = NVL(s.IsMinistryOfDefence, 0)) OR

                                   (r.MilitaryReportStatusKey = 'COMPULSORY_RESERVE_MOB_APPOINTMENT' AND NVL(r.AppointmentIsDelivered, 0) = 1 AND
								    r.ReservistReadinessID = 1 AND
                                    s.AdministrationGroup IS NOT NULL AND
									s.AdministrationGroup = r.AdministrationGroup AND
                                    (s.MilReadinessID = r.MilReadinessID OR s.MilReadinessID = 0) AND
                                    r.IsMinistryOfDefence = NVL(s.IsMinistryOfDefence, 0))  OR
                                   
                                   (s.StatusID = 10 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness WHERE MilReadinessCmdType IS NOT NULL) +
                                    (SELECT COUNT(*)
                                     FROM (SELECT AdministrationGroup 
                                           FROM PMIS_ADM.Administrations 
                                           WHERE AdministrationGroup IS NOT NULL 
                                                 AND NVL(IsMinistryOfDefence, 0) = 0 
                                           GROUP BY AdministrationGroup) adm_cnt) +
                                    (SELECT COUNT(*)
                                     FROM (  
                                        SELECT a.administrationgroup, mr.milreadinesscmdtype
                                        FROM PMIS_ADM.Administrations a
                                        INNER JOIN PMIS_RES.MilReadiness mr ON mr.administrationid = a.administrationid OR mr.administrationid IS NULL
                                        WHERE administrationgroup IS NOT NULL
                                        GROUP BY a.administrationgroup, mr.milreadinesscmdtype
                                     HAVING MAX(reporta33breakdown)  > 0) adm_readiness_cnt ) AND 
								    r.MilitaryReportStatusKey = 'COMPULSORY_RESERVE_MOB_APPOINTMENT' AND NVL(r.AppointmentIsDelivered, 0) = 1 AND
								    r.ReservistReadinessID = 2 AND
                                    r.IsMinistryOfDefence = NVL(s.IsMinistryOfDefence, 0)) OR

                                   (s.StatusID = 11 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness WHERE MilReadinessCmdType IS NOT NULL) +
                                    (SELECT COUNT(*)
                                     FROM (SELECT AdministrationGroup 
                                           FROM PMIS_ADM.Administrations 
                                           WHERE AdministrationGroup IS NOT NULL 
                                                 AND NVL(IsMinistryOfDefence, 0) = 0 
                                           GROUP BY AdministrationGroup) adm_cnt) +
                                    (SELECT COUNT(*)
                                     FROM (  
                                        SELECT a.administrationgroup, mr.milreadinesscmdtype
                                        FROM PMIS_ADM.Administrations a
                                        INNER JOIN PMIS_RES.MilReadiness mr ON mr.administrationid = a.administrationid OR mr.administrationid IS NULL
                                        WHERE administrationgroup IS NOT NULL
                                        GROUP BY a.administrationgroup, mr.milreadinesscmdtype
                                     HAVING MAX(reporta33breakdown)  > 0) adm_readiness_cnt ) AND 
								    NVL(r.PunktCnt, 0) = 0 AND
									r.MilitaryReportStatusKey = 'COMPULSORY_RESERVE_MOB_APPOINTMENT' AND NVL(r.AppointmentIsDelivered, 0) = 1 AND
                                    r.ReservistReadinessID = 1 AND
                                    r.IsMinistryOfDefence = NVL(s.IsMinistryOfDefence, 0)) OR

                                   (s.StatusID = 12 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness WHERE MilReadinessCmdType IS NOT NULL) +
                                    (SELECT COUNT(*)
                                     FROM (SELECT AdministrationGroup 
                                           FROM PMIS_ADM.Administrations 
                                           WHERE AdministrationGroup IS NOT NULL 
                                                 AND NVL(IsMinistryOfDefence, 0) = 0 
                                           GROUP BY AdministrationGroup) adm_cnt) +
                                    (SELECT COUNT(*)
                                     FROM (  
                                        SELECT a.administrationgroup, mr.milreadinesscmdtype
                                        FROM PMIS_ADM.Administrations a
                                        INNER JOIN PMIS_RES.MilReadiness mr ON mr.administrationid = a.administrationid OR mr.administrationid IS NULL
                                        WHERE administrationgroup IS NOT NULL
                                        GROUP BY a.administrationgroup, mr.milreadinesscmdtype
                                     HAVING MAX(reporta33breakdown)  > 0) adm_readiness_cnt ) AND 
								    NVL(r.PunktCnt, 0) > 0 AND
									r.MilitaryReportStatusKey = 'COMPULSORY_RESERVE_MOB_APPOINTMENT' AND NVL(r.AppointmentIsDelivered, 0) = 1 AND
                                    r.ReservistReadinessID = 1 AND
                                    r.IsMinistryOfDefence = NVL(s.IsMinistryOfDefence, 0)) OR

                                   (s.StatusID = 13 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness WHERE MilReadinessCmdType IS NOT NULL) +
                                    (SELECT COUNT(*)
                                     FROM (SELECT AdministrationGroup 
                                           FROM PMIS_ADM.Administrations 
                                           WHERE AdministrationGroup IS NOT NULL 
                                                 AND NVL(IsMinistryOfDefence, 0) = 0 
                                           GROUP BY AdministrationGroup) adm_cnt) +
                                    (SELECT COUNT(*)
                                     FROM (  
                                        SELECT a.administrationgroup, mr.milreadinesscmdtype
                                        FROM PMIS_ADM.Administrations a
                                        INNER JOIN PMIS_RES.MilReadiness mr ON mr.administrationid = a.administrationid OR mr.administrationid IS NULL
                                        WHERE administrationgroup IS NOT NULL
                                        GROUP BY a.administrationgroup, mr.milreadinesscmdtype
                                     HAVING MAX(reporta33breakdown)  > 0) adm_readiness_cnt ) AND 
								    r.MilitaryReportStatusKey = 'COMPULSORY_RESERVE_MOB_APPOINTMENT' AND NVL(r.AppointmentIsDelivered, 0) = 1 AND
								    r.ReservistReadinessID = 1 AND
                                    r.IsMinistryOfDefence = NVL(s.IsMinistryOfDefence, 0)) OR

                                   (s.StatusID = 14 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness WHERE MilReadinessCmdType IS NOT NULL)  +
                                    (SELECT COUNT(*)
                                     FROM (SELECT AdministrationGroup 
                                           FROM PMIS_ADM.Administrations 
                                           WHERE AdministrationGroup IS NOT NULL 
                                                 AND NVL(IsMinistryOfDefence, 0) = 0 
                                           GROUP BY AdministrationGroup) adm_cnt) +
                                    (SELECT COUNT(*)
                                     FROM (  
                                        SELECT a.administrationgroup, mr.milreadinesscmdtype
                                        FROM PMIS_ADM.Administrations a
                                        INNER JOIN PMIS_RES.MilReadiness mr ON mr.administrationid = a.administrationid OR mr.administrationid IS NULL
                                        WHERE administrationgroup IS NOT NULL
                                        GROUP BY a.administrationgroup, mr.milreadinesscmdtype
                                     HAVING MAX(reporta33breakdown)  > 0) adm_readiness_cnt ) AND 
                                    r.MilitaryReportStatusKey = 'POSTPONED') OR

                                   (s.StatusID = 15 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness WHERE MilReadinessCmdType IS NOT NULL) +
                                    (SELECT COUNT(*)
                                     FROM (SELECT AdministrationGroup 
                                           FROM PMIS_ADM.Administrations 
                                           WHERE AdministrationGroup IS NOT NULL 
                                                 AND NVL(IsMinistryOfDefence, 0) = 0 
                                           GROUP BY AdministrationGroup) adm_cnt) +
                                    (SELECT COUNT(*)
                                     FROM (  
                                        SELECT a.administrationgroup, mr.milreadinesscmdtype
                                        FROM PMIS_ADM.Administrations a
                                        INNER JOIN PMIS_RES.MilReadiness mr ON mr.administrationid = a.administrationid OR mr.administrationid IS NULL
                                        WHERE administrationgroup IS NOT NULL
                                        GROUP BY a.administrationgroup, mr.milreadinesscmdtype
                                     HAVING MAX(reporta33breakdown)  > 0) adm_readiness_cnt ) AND 
                                    r.MilitaryReportStatusKey = 'TEMPORARY_REMOVED') OR

                                   (s.StatusID = 16 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness WHERE MilReadinessCmdType IS NOT NULL)  +
                                    (SELECT COUNT(*)
                                     FROM (SELECT AdministrationGroup 
                                           FROM PMIS_ADM.Administrations 
                                           WHERE AdministrationGroup IS NOT NULL 
                                                 AND NVL(IsMinistryOfDefence, 0) = 0 
                                           GROUP BY AdministrationGroup) adm_cnt) +
                                    (SELECT COUNT(*)
                                     FROM (  
                                        SELECT a.administrationgroup, mr.milreadinesscmdtype
                                        FROM PMIS_ADM.Administrations a
                                        INNER JOIN PMIS_RES.MilReadiness mr ON mr.administrationid = a.administrationid OR mr.administrationid IS NULL
                                        WHERE administrationgroup IS NOT NULL
                                        GROUP BY a.administrationgroup, mr.milreadinesscmdtype
                                     HAVING MAX(reporta33breakdown)  > 0) adm_readiness_cnt ) AND 
                                    r.MilitaryReportStatusKey = 'FREE')
								 )
		                    THEN r.ReservistsCnt
							ELSE 0 
					   END)) as ColumnValue,
           s.IsEmptyRow as IsEmptyRow
	FROM PMIS_RES.ViewA33v2 s
	CROSS JOIN (SELECT 'Оф.' as ColumnLabel, 1 as ColumnOrder, (2) as MilitaryRankCategory, (1) as MilitaryRankSubCategory FROM dual
				  UNION ALL
				  SELECT 'Оф. к-ти' as ColumnLabel, 2 as ColumnOrder, (1) as MilitaryRankCategory, (3) as MilitaryRankSubCategory FROM dual
                  UNION ALL
				  SELECT 'Серж.' as ColumnLabel, 3 as ColumnOrder, (1) as MilitaryRankCategory, (1) as MilitaryRankSubCategory FROM dual
                  UNION ALL
				  SELECT 'В-ци' as ColumnLabel, 4 as ColumnOrder, (1) as MilitaryRankCategory, (2) as MilitaryRankSubCategory FROM dual
			   ) k
    LEFT OUTER JOIN (
	    SELECT COUNT(*) as ReservistsCnt, m.MilitaryReportStatusKey, f.ReservistReadinessID, f.AppointmentIsDelivered, c.MilReadinessID, i.MilitaryRankCategory, i.MilitaryRankSubCategory, gt.TableValue as TemporaryRemoved_Reason,
		       adm.AdministrationGroup, rpunkt.Cnt as PunktCnt,
               NVL(adm.IsMinistryOfDefence, 0) as IsMinistryOfDefence
		FROM PMIS_RES.RESERVISTS e
		LEFT OUTER JOIN VS_OWNER.VS_LS g ON g.PersonId = e.PersonId
                    LEFT JOIN PMIS_ADM.Persons g2 ON g2.PersonID = g.PersonID
		LEFT OUTER JOIN VS_OWNER.KLV_ZVA h ON h.ZVA_KOD = g.KOD_ZVA
		LEFT OUTER JOIN PMIS_ADM.MilitaryRankCategories i ON i.ZVA_KAT_KOD = h.ZVA_KAT_KOD
		LEFT OUTER JOIN PMIS_RES.ReservistMilRepStatuses r ON e.ReservistID = r.ReservistID AND r.IsCurrent = 1
		LEFT OUTER JOIN PMIS_RES.GTable gt ON gt.TableName = 'MilRepStat_ТemporaryRemovedReasons' AND gt.TableKey = r.TemporaryRemoved_ReasonID
		LEFT OUTER JOIN PMIS_RES.MilitaryReportStatuses m ON r.MilitaryReportStatusID = m.MilitaryReportStatusID
		LEFT OUTER JOIN PMIS_RES.FillReservistsRequest f ON e.ReservistID = f.ReservistID
		LEFT OUTER JOIN PMIS_RES.RequestCommandPositions p ON f.RequestCommandPositionID = p.RequestCommandPositionID
		LEFT OUTER JOIN PMIS_RES.RequestsCommands c ON p.RequestsCommandID = c.RequestsCommandID
		LEFT OUTER JOIN PMIS_RES.EquipmentReservistsRequests req ON c.EquipmentReservistsRequestID = req.EquipmentReservistsRequestID
        LEFT OUTER JOIN PMIS_ADM.Administrations adm ON req.AdministrationID = adm.AdministrationID
		LEFT OUTER JOIN (SELECT COUNT(*) as Cnt, a.RequestCommandID
		                 FROM PMIS_RES.RequestCommandPunkt a
						 " + whereClause2 + (whereClause2 == "" ? " WHERE " : " AND ") + @" a.CityID IS NOT NULL OR a.Place IS NOT NULL
						 GROUP BY a.RequestCommandID) rpunkt ON rpunkt.RequestCommandID = c.RequestsCommandID
        " + whereClause_Persons.Replace("<TABLE>", "r") + @"
        GROUP BY m.MilitaryReportStatusKey, f.ReservistReadinessID, f.AppointmentIsDelivered, c.MilReadinessID, i.MilitaryRankCategory, i.MilitaryRankSubCategory, gt.TableValue,
		         adm.AdministrationGroup, rpunkt.Cnt, NVL(adm.IsMinistryOfDefence, 0)
		) r ON 1 = 1
	GROUP BY s.StatusID, s.StatusOrder, s.StatusName, k.ColumnOrder, k.ColumnLabel, s.IsEmptyRow

    UNION ALL

	SELECT s.StatusID, s.StatusOrder, s.StatusName, 7 + (DENSE_RANK() OVER (ORDER BY g.TableSeq)) as ColumnOrder, g.TableValue as ColumnLabel, 
	       TO_CHAR(SUM(CASE WHEN g.TableKey = v.VehicleKindID AND
		                         (
								   (s.StatusID = 1 AND 
                                      (
                                         v.TechMilitaryReportStatusKey = 'VOLUNTARY_RESERVE' OR

                                         (
                                          (v.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND NVL(v.AppointmentIsDelivered, 0) = 1 AND
								           v.TechnicReadinessID = 2) OR
                                          (v.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND NVL(v.AppointmentIsDelivered, 0) = 1 AND
								           v.TechnicReadinessID = 1) OR

                                          v.TechMilitaryReportStatusKey = 'POSTPONED' OR
                                          v.TechMilitaryReportStatusKey = 'TEMPORARY_REMOVED' OR
                                          v.TechMilitaryReportStatusKey = 'FREE'
                                         )
                                      )
                                   ) OR

                                   (s.StatusID = 2 AND v.TechMilitaryReportStatusKey = 'VOLUNTARY_RESERVE') OR

								   (s.StatusID = 3 AND 
                                      (
                                         (v.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND NVL(v.AppointmentIsDelivered, 0) = 1 AND
								          v.TechnicReadinessID = 2) OR
                                         (v.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND NVL(v.AppointmentIsDelivered, 0) = 1 AND
								          v.TechnicReadinessID = 1) OR

                                         v.TechMilitaryReportStatusKey = 'POSTPONED' OR
                                         v.TechMilitaryReportStatusKey = 'TEMPORARY_REMOVED' OR
                                         v.TechMilitaryReportStatusKey = 'FREE'
                                      )
                                   ) OR

								   (v.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND NVL(v.AppointmentIsDelivered, 0) = 1 AND
								    v.TechnicReadinessID = 1 AND
									s.MilReadinessID = v.MilReadinessID AND
                                    s.AdministrationGroup IS NULL AND
                                    v.IsMinistryOfDefence = NVL(s.IsMinistryOfDefence, 0)) OR	

                                   (s.StatusID = 5 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness WHERE MilReadinessCmdType IS NOT NULL) AND 
								    v.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND NVL(v.AppointmentIsDelivered, 0) = 1 AND
								    v.TechnicReadinessID = 2 AND
                                    v.IsMinistryOfDefence = NVL(s.IsMinistryOfDefence, 0)) OR
								   
                                   (s.StatusID = 6 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness WHERE MilReadinessCmdType IS NOT NULL) AND 
								    NVL(v.PunktCnt, 0) = 0 AND
									v.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND NVL(v.AppointmentIsDelivered, 0) = 1 AND
                                    v.TechnicReadinessID = 1 AND
                                    v.IsMinistryOfDefence = NVL(s.IsMinistryOfDefence, 0)) OR

                                   (s.StatusID = 7 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness WHERE MilReadinessCmdType IS NOT NULL) AND 
								    NVL(v.PunktCnt, 0) > 0 AND
									v.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND NVL(v.AppointmentIsDelivered, 0) = 1 AND
                                    v.TechnicReadinessID = 1 AND
                                    v.IsMinistryOfDefence = NVL(s.IsMinistryOfDefence, 0)) OR

                                   (s.StatusID = 8 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness WHERE MilReadinessCmdType IS NOT NULL) AND 
								    v.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND NVL(v.AppointmentIsDelivered, 0) = 1 AND
								    v.TechnicReadinessID = 1 AND
                                    v.IsMinistryOfDefence = NVL(s.IsMinistryOfDefence, 0)) OR
                                    
                                   (v.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND NVL(v.AppointmentIsDelivered, 0) = 1 AND
								    v.TechnicReadinessID = 1 AND
                                    s.AdministrationGroup IS NOT NULL AND
									s.AdministrationGroup = v.AdministrationGroup AND
                                    (s.MilReadinessID = v.MilReadinessID OR s.MilReadinessID = 0) AND
                                    v.IsMinistryOfDefence = NVL(s.IsMinistryOfDefence, 0)) OR
                                 
                                   (s.StatusID = 10 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness WHERE MilReadinessCmdType IS NOT NULL) + 
                                    (SELECT COUNT(*)
                                     FROM (SELECT AdministrationGroup 
                                           FROM PMIS_ADM.Administrations 
                                           WHERE AdministrationGroup IS NOT NULL 
                                                 AND NVL(IsMinistryOfDefence, 0) = 0 
                                           GROUP BY AdministrationGroup) adm_cnt) +
                                    (SELECT COUNT(*)
                                     FROM (  
                                        SELECT a.administrationgroup, mr.milreadinesscmdtype
                                        FROM PMIS_ADM.Administrations a
                                        INNER JOIN PMIS_RES.MilReadiness mr ON mr.administrationid = a.administrationid OR mr.administrationid IS NULL
                                        WHERE administrationgroup IS NOT NULL
                                        GROUP BY a.administrationgroup, mr.milreadinesscmdtype
                                     HAVING MAX(reporta33breakdown)  > 0) adm_readiness_cnt ) AND 
								    v.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND NVL(v.AppointmentIsDelivered, 0) = 1 AND
								    v.TechnicReadinessID = 2 AND
                                    v.IsMinistryOfDefence = NVL(s.IsMinistryOfDefence, 0)) OR

                                   (s.StatusID = 11 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness WHERE MilReadinessCmdType IS NOT NULL) + 
                                    (SELECT COUNT(*)
                                     FROM (SELECT AdministrationGroup 
                                           FROM PMIS_ADM.Administrations 
                                           WHERE AdministrationGroup IS NOT NULL 
                                                 AND NVL(IsMinistryOfDefence, 0) = 0 
                                           GROUP BY AdministrationGroup) adm_cnt) +
                                    (SELECT COUNT(*)
                                     FROM (  
                                        SELECT a.administrationgroup, mr.milreadinesscmdtype
                                        FROM PMIS_ADM.Administrations a
                                        INNER JOIN PMIS_RES.MilReadiness mr ON mr.administrationid = a.administrationid OR mr.administrationid IS NULL
                                        WHERE administrationgroup IS NOT NULL
                                        GROUP BY a.administrationgroup, mr.milreadinesscmdtype
                                     HAVING MAX(reporta33breakdown)  > 0) adm_readiness_cnt ) AND 
								    NVL(v.PunktCnt, 0) = 0 AND
									v.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND NVL(v.AppointmentIsDelivered, 0) = 1 AND
                                    v.TechnicReadinessID = 1 AND
                                    v.IsMinistryOfDefence = NVL(s.IsMinistryOfDefence, 0)) OR

                                   (s.StatusID = 12 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness WHERE MilReadinessCmdType IS NOT NULL) + 
                                    (SELECT COUNT(*)
                                     FROM (SELECT AdministrationGroup 
                                           FROM PMIS_ADM.Administrations 
                                           WHERE AdministrationGroup IS NOT NULL 
                                                 AND NVL(IsMinistryOfDefence, 0) = 0 
                                           GROUP BY AdministrationGroup) adm_cnt) +
                                    (SELECT COUNT(*)
                                     FROM (  
                                        SELECT a.administrationgroup, mr.milreadinesscmdtype
                                        FROM PMIS_ADM.Administrations a
                                        INNER JOIN PMIS_RES.MilReadiness mr ON mr.administrationid = a.administrationid OR mr.administrationid IS NULL
                                        WHERE administrationgroup IS NOT NULL
                                        GROUP BY a.administrationgroup, mr.milreadinesscmdtype
                                     HAVING MAX(reporta33breakdown)  > 0) adm_readiness_cnt ) AND 
								    NVL(v.PunktCnt, 0) > 0 AND
									v.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND NVL(v.AppointmentIsDelivered, 0) = 1 AND
                                    v.TechnicReadinessID = 1 AND
                                    v.IsMinistryOfDefence = NVL(s.IsMinistryOfDefence, 0)) OR

                                   (s.StatusID = 13 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness WHERE MilReadinessCmdType IS NOT NULL) + 
                                    (SELECT COUNT(*)
                                     FROM (SELECT AdministrationGroup 
                                           FROM PMIS_ADM.Administrations 
                                           WHERE AdministrationGroup IS NOT NULL 
                                                 AND NVL(IsMinistryOfDefence, 0) = 0 
                                           GROUP BY AdministrationGroup) adm_cnt) +
                                    (SELECT COUNT(*)
                                     FROM (  
                                        SELECT a.administrationgroup, mr.milreadinesscmdtype
                                        FROM PMIS_ADM.Administrations a
                                        INNER JOIN PMIS_RES.MilReadiness mr ON mr.administrationid = a.administrationid OR mr.administrationid IS NULL
                                        WHERE administrationgroup IS NOT NULL
                                        GROUP BY a.administrationgroup, mr.milreadinesscmdtype
                                     HAVING MAX(reporta33breakdown)  > 0) adm_readiness_cnt ) AND 
								    v.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND NVL(v.AppointmentIsDelivered, 0) = 1 AND
								    v.TechnicReadinessID = 1 AND
                                    v.IsMinistryOfDefence = NVL(s.IsMinistryOfDefence, 0)) OR

                                   (s.StatusID = 14 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness WHERE MilReadinessCmdType IS NOT NULL) + 
                                    (SELECT COUNT(*)
                                     FROM (SELECT AdministrationGroup 
                                           FROM PMIS_ADM.Administrations 
                                           WHERE AdministrationGroup IS NOT NULL 
                                                 AND NVL(IsMinistryOfDefence, 0) = 0 
                                           GROUP BY AdministrationGroup) adm_cnt) +
                                    (SELECT COUNT(*)
                                     FROM (  
                                        SELECT a.administrationgroup, mr.milreadinesscmdtype
                                        FROM PMIS_ADM.Administrations a
                                        INNER JOIN PMIS_RES.MilReadiness mr ON mr.administrationid = a.administrationid OR mr.administrationid IS NULL
                                        WHERE administrationgroup IS NOT NULL
                                        GROUP BY a.administrationgroup, mr.milreadinesscmdtype
                                     HAVING MAX(reporta33breakdown)  > 0) adm_readiness_cnt ) AND 
                                    v.TechMilitaryReportStatusKey = 'POSTPONED') OR

                                   (s.StatusID = 15 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness WHERE MilReadinessCmdType IS NOT NULL) + 
                                    (SELECT COUNT(*)
                                     FROM (SELECT AdministrationGroup 
                                           FROM PMIS_ADM.Administrations 
                                           WHERE AdministrationGroup IS NOT NULL 
                                                 AND NVL(IsMinistryOfDefence, 0) = 0 
                                           GROUP BY AdministrationGroup) adm_cnt) +
                                    (SELECT COUNT(*)
                                     FROM (  
                                        SELECT a.administrationgroup, mr.milreadinesscmdtype
                                        FROM PMIS_ADM.Administrations a
                                        INNER JOIN PMIS_RES.MilReadiness mr ON mr.administrationid = a.administrationid OR mr.administrationid IS NULL
                                        WHERE administrationgroup IS NOT NULL
                                        GROUP BY a.administrationgroup, mr.milreadinesscmdtype
                                     HAVING MAX(reporta33breakdown)  > 0) adm_readiness_cnt ) AND 
                                    v.TechMilitaryReportStatusKey = 'TEMPORARY_REMOVED') OR

                                   (s.StatusID = 16 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness WHERE MilReadinessCmdType IS NOT NULL) + 
                                    (SELECT COUNT(*)
                                     FROM (SELECT AdministrationGroup 
                                           FROM PMIS_ADM.Administrations 
                                           WHERE AdministrationGroup IS NOT NULL 
                                                 AND NVL(IsMinistryOfDefence, 0) = 0 
                                           GROUP BY AdministrationGroup) adm_cnt) +
                                    (SELECT COUNT(*)
                                     FROM (  
                                        SELECT a.administrationgroup, mr.milreadinesscmdtype
                                        FROM PMIS_ADM.Administrations a
                                        INNER JOIN PMIS_RES.MilReadiness mr ON mr.administrationid = a.administrationid OR mr.administrationid IS NULL
                                        WHERE administrationgroup IS NOT NULL
                                        GROUP BY a.administrationgroup, mr.milreadinesscmdtype
                                     HAVING MAX(reporta33breakdown)  > 0) adm_readiness_cnt ) AND 
                                    v.TechMilitaryReportStatusKey = 'FREE')
								 )
		                    THEN v.VehiclesCnt 
							ELSE 0 
					   END)) as ColumnValue,
           s.IsEmptyRow as IsEmptyRow
	FROM PMIS_RES.ViewA33v2 s
	LEFT OUTER JOIN PMIS_RES.GTABLE g ON g.TableName = 'VehicleKind'
	LEFT OUTER JOIN (
	    SELECT COUNT(*) as VehiclesCnt, v.VehicleKindID, ts.TechMilitaryReportStatusKey, ft.TechnicReadinessID, ft.AppointmentIsDelivered, ct.MilReadinessID,
		       adm.AdministrationGroup, tpunkt.Cnt as PunktCnt,
               NVL(adm.IsMinistryOfDefence, 0) as IsMinistryOfDefence
		FROM PMIS_RES.Vehicles v
		INNER JOIN PMIS_RES.Technics t ON v.TechnicsID = t.TechnicsID
		LEFT OUTER JOIN PMIS_RES.TechnicsMilRepStatus s ON t.TechnicsID = s.TechnicsID AND s.IsCurrent = 1
		LEFT OUTER JOIN PMIS_RES.TechMilitaryReportStatuses ts ON s.TechMilitaryReportStatusID = ts.TechMilitaryReportStatusID
		LEFT OUTER JOIN PMIS_RES.FulfilTechnicsRequest ft ON v.TechnicsID = ft.TechnicsID
		LEFT OUTER JOIN PMIS_RES.TechnicsRequestCmdPositions pt ON pt.TechnicsRequestCmdPositionId = ft.TechnicsRequestCmdPositionId
		LEFT OUTER JOIN PMIS_RES.TechnicsRequestCommands ct ON ct.TechRequestsCommandId = pt.TechRequestsCommandId
		LEFT OUTER JOIN PMIS_RES.EquipmentTechnicsRequests tr ON tr.EquipmentTechnicsRequestID = ct.EquipmentTechnicsRequestID
        LEFT OUTER JOIN PMIS_ADM.Administrations adm ON tr.AdministrationID = adm.AdministrationID
        LEFT OUTER JOIN PMIS_ADM.Companies com ON t.OwnershipCompanyID = com.CompanyID
		LEFT OUTER JOIN (SELECT COUNT(*) as Cnt, a.TechRequestsCommandId
		                 FROM PMIS_RES.TechRequestCommandPunkt a
						 " + whereClause2 + (whereClause2 == "" ? " WHERE " : " AND ") + @" a.CityID IS NOT NULL OR a.Place IS NOT NULL
						 GROUP BY a.TechRequestsCommandId) tpunkt ON tpunkt.TechRequestsCommandId = ct.TechRequestsCommandId
        " + whereClause_Technics.Replace("<TABLE>", "s") + @"
        GROUP BY v.VehicleKindID, ts.TechMilitaryReportStatusKey, ft.TechnicReadinessID, ft.AppointmentIsDelivered, ct.MilReadinessID,
		         adm.AdministrationGroup, tpunkt.Cnt,
                 NVL(adm.IsMinistryOfDefence, 0)
	    ) v ON 1 = 1
	GROUP BY s.StatusID, s.StatusOrder, s.StatusName, g.TableSeq, g.TableValue, s.IsEmptyRow

    UNION ALL

    SELECT s.StatusID, s.StatusOrder, s.StatusName, 7 + (SELECT COUNT(*) FROM PMIS_RES.GTABLE WHERE TableName = 'VehicleKind') + (DENSE_RANK() OVER (ORDER BY tt.Seq)) as ColumnOrder, tt.TechnicsTypeName as ColumnLabel, 
	       TO_CHAR(SUM(CASE WHEN tt.TechnicsTypeID = t.TechnicsTypeID AND
		                         (
								   (s.StatusID = 1 AND 
                                      (
                                         t.TechMilitaryReportStatusKey = 'VOLUNTARY_RESERVE' OR 

                                         (
                                          (t.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND NVL(t.AppointmentIsDelivered, 0) = 1 AND
								           t.TechnicReadinessID = 2) OR
                                          (t.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND NVL(t.AppointmentIsDelivered, 0) = 1 AND
								           t.TechnicReadinessID = 1) OR

                                          t.TechMilitaryReportStatusKey = 'POSTPONED' OR
                                          t.TechMilitaryReportStatusKey = 'TEMPORARY_REMOVED' OR
                                          t.TechMilitaryReportStatusKey = 'FREE'
                                         )
                                      )
                                   ) OR

                                   (s.StatusID = 2 AND 
                                    t.TechMilitaryReportStatusKey = 'VOLUNTARY_RESERVE') OR

								   (s.StatusID = 3 AND 
                                      (
                                         (t.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND NVL(t.AppointmentIsDelivered, 0) = 1 AND
								          t.TechnicReadinessID = 2) OR
                                         (t.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND NVL(t.AppointmentIsDelivered, 0) = 1 AND
								          t.TechnicReadinessID = 1) OR

                                         t.TechMilitaryReportStatusKey = 'POSTPONED' OR
                                         t.TechMilitaryReportStatusKey = 'TEMPORARY_REMOVED' OR
                                         t.TechMilitaryReportStatusKey = 'FREE'
                                      )
                                   ) OR

								   (t.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND NVL(t.AppointmentIsDelivered, 0) = 1 AND
								    t.TechnicReadinessID = 1 AND
									s.MilReadinessID = t.MilReadinessID AND
                                    s.AdministrationGroup IS NULL AND
                                    t.IsMinistryOfDefence = NVL(s.IsMinistryOfDefence, 0)) OR	

                                   (s.StatusID = 5 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness WHERE MilReadinessCmdType IS NOT NULL) AND 
								    t.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND NVL(t.AppointmentIsDelivered, 0) = 1 AND
								    t.TechnicReadinessID = 2 AND
                                    t.IsMinistryOfDefence = NVL(s.IsMinistryOfDefence, 0)) OR
								   
                                   (s.StatusID = 6 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness WHERE MilReadinessCmdType IS NOT NULL) AND 
								    NVL(t.PunktCnt, 0) = 0 AND
									t.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND NVL(t.AppointmentIsDelivered, 0) = 1 AND
                                    t.TechnicReadinessID = 1 AND
                                    t.IsMinistryOfDefence = NVL(s.IsMinistryOfDefence, 0)) OR

                                   (s.StatusID = 7 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness WHERE MilReadinessCmdType IS NOT NULL) AND 
								    NVL(t.PunktCnt, 0) > 0 AND
									t.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND NVL(t.AppointmentIsDelivered, 0) = 1 AND
                                    t.TechnicReadinessID = 1 AND
                                    t.IsMinistryOfDefence = NVL(s.IsMinistryOfDefence, 0)) OR

                                   (s.StatusID = 8 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness WHERE MilReadinessCmdType IS NOT NULL) AND 
								    t.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND NVL(t.AppointmentIsDelivered, 0) = 1 AND
								    t.TechnicReadinessID = 1 AND
                                    t.IsMinistryOfDefence = NVL(s.IsMinistryOfDefence, 0)) OR

                                   (t.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND NVL(t.AppointmentIsDelivered, 0) = 1 AND
								    t.TechnicReadinessID = 1 AND
                                    s.AdministrationGroup IS NOT NULL AND
									s.AdministrationGroup = t.AdministrationGroup AND
                                    (s.MilReadinessID = t.MilReadinessID OR s.MilReadinessID = 0) AND
                                    t.IsMinistryOfDefence = NVL(s.IsMinistryOfDefence, 0)) OR
                                   
                                   (s.StatusID = 10 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness WHERE MilReadinessCmdType IS NOT NULL) + 
                                    (SELECT COUNT(*)
                                     FROM (SELECT AdministrationGroup 
                                           FROM PMIS_ADM.Administrations 
                                           WHERE AdministrationGroup IS NOT NULL 
                                                 AND NVL(IsMinistryOfDefence, 0) = 0 
                                           GROUP BY AdministrationGroup) adm_cnt) +
                                    (SELECT COUNT(*)
                                     FROM (  
                                        SELECT a.administrationgroup, mr.milreadinesscmdtype
                                        FROM PMIS_ADM.Administrations a
                                        INNER JOIN PMIS_RES.MilReadiness mr ON mr.administrationid = a.administrationid OR mr.administrationid IS NULL
                                        WHERE administrationgroup IS NOT NULL
                                        GROUP BY a.administrationgroup, mr.milreadinesscmdtype
                                     HAVING MAX(reporta33breakdown)  > 0) adm_readiness_cnt ) AND 
								    t.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND NVL(t.AppointmentIsDelivered, 0) = 1 AND
								    t.TechnicReadinessID = 2 AND
                                    t.IsMinistryOfDefence = NVL(s.IsMinistryOfDefence, 0)) OR

                                   (s.StatusID = 11 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness WHERE MilReadinessCmdType IS NOT NULL) + 
                                    (SELECT COUNT(*)
                                     FROM (SELECT AdministrationGroup 
                                           FROM PMIS_ADM.Administrations 
                                           WHERE AdministrationGroup IS NOT NULL 
                                                 AND NVL(IsMinistryOfDefence, 0) = 0 
                                           GROUP BY AdministrationGroup) adm_cnt) +
                                    (SELECT COUNT(*)
                                     FROM (  
                                        SELECT a.administrationgroup, mr.milreadinesscmdtype
                                        FROM PMIS_ADM.Administrations a
                                        INNER JOIN PMIS_RES.MilReadiness mr ON mr.administrationid = a.administrationid OR mr.administrationid IS NULL
                                        WHERE administrationgroup IS NOT NULL
                                        GROUP BY a.administrationgroup, mr.milreadinesscmdtype
                                     HAVING MAX(reporta33breakdown)  > 0) adm_readiness_cnt ) AND 
								    NVL(t.PunktCnt, 0) = 0 AND
									t.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND NVL(t.AppointmentIsDelivered, 0) = 1 AND
                                    t.TechnicReadinessID = 1 AND
                                    t.IsMinistryOfDefence = NVL(s.IsMinistryOfDefence, 0)) OR

                                   (s.StatusID = 12 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness WHERE MilReadinessCmdType IS NOT NULL) + 
                                    (SELECT COUNT(*)
                                     FROM (SELECT AdministrationGroup 
                                           FROM PMIS_ADM.Administrations 
                                           WHERE AdministrationGroup IS NOT NULL 
                                                 AND NVL(IsMinistryOfDefence, 0) = 0 
                                           GROUP BY AdministrationGroup) adm_cnt) +
                                    (SELECT COUNT(*)
                                     FROM (  
                                        SELECT a.administrationgroup, mr.milreadinesscmdtype
                                        FROM PMIS_ADM.Administrations a
                                        INNER JOIN PMIS_RES.MilReadiness mr ON mr.administrationid = a.administrationid OR mr.administrationid IS NULL
                                        WHERE administrationgroup IS NOT NULL
                                        GROUP BY a.administrationgroup, mr.milreadinesscmdtype
                                     HAVING MAX(reporta33breakdown)  > 0) adm_readiness_cnt ) AND 
								    NVL(t.PunktCnt, 0) > 0 AND
									t.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND NVL(t.AppointmentIsDelivered, 0) = 1 AND
                                    t.TechnicReadinessID = 1 AND
                                    t.IsMinistryOfDefence = NVL(s.IsMinistryOfDefence, 0)) OR

                                   (s.StatusID = 13 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness WHERE MilReadinessCmdType IS NOT NULL) + 
                                    (SELECT COUNT(*)
                                     FROM (SELECT AdministrationGroup 
                                           FROM PMIS_ADM.Administrations 
                                           WHERE AdministrationGroup IS NOT NULL 
                                                 AND NVL(IsMinistryOfDefence, 0) = 0 
                                           GROUP BY AdministrationGroup) adm_cnt) +
                                    (SELECT COUNT(*)
                                     FROM (  
                                        SELECT a.administrationgroup, mr.milreadinesscmdtype
                                        FROM PMIS_ADM.Administrations a
                                        INNER JOIN PMIS_RES.MilReadiness mr ON mr.administrationid = a.administrationid OR mr.administrationid IS NULL
                                        WHERE administrationgroup IS NOT NULL
                                        GROUP BY a.administrationgroup, mr.milreadinesscmdtype
                                     HAVING MAX(reporta33breakdown)  > 0) adm_readiness_cnt ) AND 
								    t.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND NVL(t.AppointmentIsDelivered, 0) = 1 AND
								    t.TechnicReadinessID = 1 AND
                                    t.IsMinistryOfDefence = NVL(s.IsMinistryOfDefence, 0)) OR

                                   (s.StatusID = 14 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness WHERE MilReadinessCmdType IS NOT NULL) + 
                                    (SELECT COUNT(*)
                                     FROM (SELECT AdministrationGroup 
                                           FROM PMIS_ADM.Administrations 
                                           WHERE AdministrationGroup IS NOT NULL 
                                                 AND NVL(IsMinistryOfDefence, 0) = 0 
                                           GROUP BY AdministrationGroup) adm_cnt) +
                                    (SELECT COUNT(*)
                                     FROM (  
                                        SELECT a.administrationgroup, mr.milreadinesscmdtype
                                        FROM PMIS_ADM.Administrations a
                                        INNER JOIN PMIS_RES.MilReadiness mr ON mr.administrationid = a.administrationid OR mr.administrationid IS NULL
                                        WHERE administrationgroup IS NOT NULL
                                        GROUP BY a.administrationgroup, mr.milreadinesscmdtype
                                     HAVING MAX(reporta33breakdown)  > 0) adm_readiness_cnt ) AND 
                                    t.TechMilitaryReportStatusKey = 'POSTPONED') OR

                                   (s.StatusID = 15 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness WHERE MilReadinessCmdType IS NOT NULL) + 
                                    (SELECT COUNT(*)
                                     FROM (SELECT AdministrationGroup 
                                           FROM PMIS_ADM.Administrations 
                                           WHERE AdministrationGroup IS NOT NULL 
                                                 AND NVL(IsMinistryOfDefence, 0) = 0 
                                           GROUP BY AdministrationGroup) adm_cnt) +
                                    (SELECT COUNT(*)
                                     FROM (  
                                        SELECT a.administrationgroup, mr.milreadinesscmdtype
                                        FROM PMIS_ADM.Administrations a
                                        INNER JOIN PMIS_RES.MilReadiness mr ON mr.administrationid = a.administrationid OR mr.administrationid IS NULL
                                        WHERE administrationgroup IS NOT NULL
                                        GROUP BY a.administrationgroup, mr.milreadinesscmdtype
                                     HAVING MAX(reporta33breakdown)  > 0) adm_readiness_cnt ) AND 
                                    t.TechMilitaryReportStatusKey = 'TEMPORARY_REMOVED') OR

                                   (s.StatusID = 16 + (SELECT COUNT(*) FROM PMIS_RES.MilReadiness WHERE MilReadinessCmdType IS NOT NULL) + 
                                    (SELECT COUNT(*)
                                     FROM (SELECT AdministrationGroup 
                                           FROM PMIS_ADM.Administrations 
                                           WHERE AdministrationGroup IS NOT NULL 
                                                 AND NVL(IsMinistryOfDefence, 0) = 0 
                                           GROUP BY AdministrationGroup) adm_cnt) +
                                    (SELECT COUNT(*)
                                     FROM (  
                                        SELECT a.administrationgroup, mr.milreadinesscmdtype
                                        FROM PMIS_ADM.Administrations a
                                        INNER JOIN PMIS_RES.MilReadiness mr ON mr.administrationid = a.administrationid OR mr.administrationid IS NULL
                                        WHERE administrationgroup IS NOT NULL
                                        GROUP BY a.administrationgroup, mr.milreadinesscmdtype
                                     HAVING MAX(reporta33breakdown)  > 0) adm_readiness_cnt ) AND 
                                    t.TechMilitaryReportStatusKey = 'FREE')
								 )
		                    THEN t.ItemsCount 
							ELSE 0 
					   END)) as ColumnValue,
           s.IsEmptyRow
	FROM PMIS_RES.ViewA33v2 s
	CROSS JOIN PMIS_RES.TechnicsTypes tt
	LEFT OUTER JOIN (
	    SELECT t.TechnicsTypeID, ts.TechMilitaryReportStatusKey, ft.TechnicReadinessID, ft.AppointmentIsDelivered, ct.MilReadinessID,
		       adm.AdministrationGroup, tpunkt.Cnt as PunktCnt, SUM(t.ItemsCount) as ItemsCount,
               NVL(adm.IsMinistryOfDefence, 0) as IsMinistryOfDefence
		FROM PMIS_RES.Technics t
		INNER JOIN PMIS_RES.TechnicsTypes tt ON t.TechnicsTypeID = tt.TechnicsTypeID
		LEFT OUTER JOIN PMIS_RES.TechnicsMilRepStatus s ON t.TechnicsID = s.TechnicsID AND s.IsCurrent = 1
		LEFT OUTER JOIN PMIS_RES.TechMilitaryReportStatuses ts ON s.TechMilitaryReportStatusID = ts.TechMilitaryReportStatusID
		LEFT OUTER JOIN PMIS_RES.FulfilTechnicsRequest ft ON t.TechnicsID = ft.TechnicsID
		LEFT OUTER JOIN PMIS_RES.TechnicsRequestCmdPositions pt ON pt.TechnicsRequestCmdPositionId = ft.TechnicsRequestCmdPositionId
		LEFT OUTER JOIN PMIS_RES.TechnicsRequestCommands ct ON ct.TechRequestsCommandId = pt.TechRequestsCommandId
		LEFT OUTER JOIN PMIS_RES.EquipmentTechnicsRequests tr ON tr.EquipmentTechnicsRequestID = ct.EquipmentTechnicsRequestID
        LEFT OUTER JOIN PMIS_ADM.Administrations adm ON tr.AdministrationID = adm.AdministrationID
        LEFT OUTER JOIN PMIS_ADM.Companies com ON t.OwnershipCompanyID = com.CompanyID
		LEFT OUTER JOIN (SELECT COUNT(*) as Cnt, a.TechRequestsCommandId
		                 FROM PMIS_RES.TechRequestCommandPunkt a
						 " + whereClause2 + (whereClause2 == "" ? " WHERE " : " AND ") + @" a.CityID IS NOT NULL OR a.Place IS NOT NULL
						 GROUP BY a.TechRequestsCommandId) tpunkt ON tpunkt.TechRequestsCommandId = ct.TechRequestsCommandId
        " + whereClause_Technics.Replace("<TABLE>", "s") + (whereClause_Technics == "" ? " WHERE " : " AND ") + @" tt.TechnicsTypeKey <> 'VEHICLES'
        GROUP BY t.TechnicsTypeID, ts.TechMilitaryReportStatusKey, ft.TechnicReadinessID, ft.AppointmentIsDelivered, ct.MilReadinessID,
		         adm.AdministrationGroup, tpunkt.Cnt,
                 NVL(adm.IsMinistryOfDefence, 0)
	    ) t ON 1 = 1
    WHERE tt.TechnicsTypeKey <> 'VEHICLES' AND NVL(tt.Active, 0) = 1
	GROUP BY s.StatusID, s.StatusOrder, s.StatusName, tt.TechnicsTypeID, tt.Seq, tt.TechnicsTypeName, s.IsEmptyRow
) s
ORDER BY StatusOrder, StatusID, ColumnOrder, ColumnLabel
";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                //cmd.Parameters.Add("EmptySuffixString", OracleType.VarChar).Value = emptySuffixString;

                if (Config.GetWebSetting("ReportA33v2LogSQL").ToLower() == "true")
                {
                    StreamWriter sw = new StreamWriter(Config.GetWebSetting("ReportA33v2LogSQLFile"), false, Encoding.UTF8);
                    sw.WriteLine("EmptySuffixString = '" + emptySuffixString + "'");
                    sw.WriteLine("");
                    sw.WriteLine("---------------");
                    sw.WriteLine("");
                    sw.WriteLine(SQL);
                    sw.Close();
                }

                OracleDataReader dr = cmd.ExecuteReader();

                bool isFirstRow = true;
                List<string> headerCellsList = new List<string>();
                List<ReportA33v2Row> rowsList = new List<ReportA33v2Row>();

                List<string> tmpRowColumns = new List<string>();
                int oldStatusID = -1;

                bool isEmptyRow = false;
                
                while (dr.Read())
                {
                    int statusId = DBCommon.GetInt(dr["StatusID"]);
                    string statusName = (string)dr["StatusName"];
                    int columnOrder = DBCommon.GetInt(dr["ColumnOrder"]);
                    string columnLabel = (string)dr["ColumnLabel"];
                    string columnValue = dr["ColumnValue"].ToString();

                    if (oldStatusID != -1 && oldStatusID != statusId)
                    { 
                        //new row
                        isFirstRow = false;

                        rowsList.Add(new ReportA33v2Row() { IsEmptyRow = isEmptyRow, ColumnValues = tmpRowColumns.ToList<string>() });
                        tmpRowColumns.Clear();
                    }

                    isEmptyRow = (DBCommon.GetInt(dr["IsEmptyRow"]) == 1);

                    oldStatusID = statusId;                   

                    if (isFirstRow)
                    {
                        headerCellsList.Add(columnLabel);
                    }

                    tmpRowColumns.Add(columnValue);
                }

                dr.Close();

                if (tmpRowColumns.Count > 0)
                {
                    rowsList.Add(new ReportA33v2Row() { IsEmptyRow = isEmptyRow, ColumnValues = tmpRowColumns.ToList<string>() });
                }
                
                tmpRowColumns.Clear();

                reportResult.HeaderCells = headerCellsList;
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