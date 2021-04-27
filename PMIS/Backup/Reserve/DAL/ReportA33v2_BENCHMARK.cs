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
    public class ReportA33v2Result_BENCHMARK
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
        public List<ReportA33v2Row_BENCHMARK> Rows { get; set; }
    }

    public class ReportA33v2Row_BENCHMARK
    {
        public int ReservistID { get; set; }
        public string MilitaryReportStatusKey { get; set; }
        public int ReservistReadinessID { get; set; }
        public bool AppointmentIsDelivered { get; set; }
        public int MilReadinessID { get; set; }
        public int MilitaryRankCategory { get; set; }
        public int MilitaryRankSubCategory { get; set; }
        public string TemporaryRemoved_Reason { get; set; }
        public int AdministrationGroup { get; set; }
        public int PunktCnt { get; set; }
        public bool IsMinistryOfDefence { get; set; }
    }

    public static class ReportA33v2Util_BENCHMARK
    {
        public static ReportA33v2Result_BENCHMARK GetReportA33v2(ReportA33v2Filter filter, User currentUser)
        {
            ReportA33v2Result_BENCHMARK reportResult = new ReportA33v2Result_BENCHMARK();
            reportResult.Filter = filter;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                //string emptySuffixString = "~=~";

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

	    SELECT COUNT(*) as ReservistID, m.MilitaryReportStatusKey, f.ReservistReadinessID, f.AppointmentIsDelivered, c.MilReadinessID, i.MilitaryRankCategory, i.MilitaryRankSubCategory, gt.TableValue as TemporaryRemoved_Reason,
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
";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                List<ReportA33v2Row_BENCHMARK> rowsList = new List<ReportA33v2Row_BENCHMARK>();

                while (dr.Read())
                {
                    var row = new ReportA33v2Row_BENCHMARK();

                    row.ReservistID = DBCommon.GetInt(dr["ReservistID"]);
                    row.MilitaryReportStatusKey = dr["MilitaryReportStatusKey"].ToString();
                    row.ReservistReadinessID = DBCommon.GetInt(dr["ReservistReadinessID"]);
                    row.AppointmentIsDelivered = DBCommon.GetInt(dr["AppointmentIsDelivered"]) == 1;
                    row.MilReadinessID = DBCommon.GetInt(dr["MilReadinessID"]);
                    row.MilitaryRankCategory = DBCommon.GetInt(dr["MilitaryRankCategory"]);
                    row.MilitaryRankSubCategory = DBCommon.GetInt(dr["MilitaryRankSubCategory"]);
                    row.TemporaryRemoved_Reason = dr["TemporaryRemoved_Reason"].ToString();
                    row.AdministrationGroup = DBCommon.GetInt(dr["AdministrationGroup"]);
                    row.PunktCnt = DBCommon.GetInt(dr["PunktCnt"]);
                    row.IsMinistryOfDefence = DBCommon.GetInt(dr["IsMinistryOfDefence"]) == 1;

                    rowsList.Add(row);
                }

                dr.Close();

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