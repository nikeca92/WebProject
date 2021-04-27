using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using System.Linq;

using PMIS.Common;

namespace PMIS.Reserve.Common
{
    //This class represents all information about the filter, the order and the paging information on the screen
    public class ReportSV1Filter
    {
        public string MilitaryDepartmentIds { get; set; }
        public string MilitaryForceSortIds { get; set; }
        public string MilitaryReportSpecialityIds { get; set; }

        public string Region { get; set; }
        public string Municipality { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string PostCode { get; set; }
        public string Address { get; set; }

        public int PageIdx { get; set; }
        public int PageSize { get; set; }
    }

    public class ReportSV1Block
    {
        public int MilitaryStructureID { get; set; }
        public string MilitaryStructureName { get; set; }
        public int MilitaryStructureOrder { get; set; }
        
        public int MilitaryForceTypeID { get; set; }
        public string MilitaryForceTypeName { get; set; }
        public int MilitaryForceTypeOrder { get; set; }

        public int MilitaryForceSortID { get; set; }
        public string MilitaryForceSortName { get; set; }
        public int MilitaryForceSortOrder { get; set; }

        public int MilRepSpecialityID { get; set; }
        public string MilRepSpecialityCode { get; set; }

        public int MilRepStatusID { get; set; }
        public string MilRepStatusName { get; set; }
        public int MilRepStatusOrder { get; set; }
        public string MilRepStatusKey { get; set; }
        public int RowType { get; set; }
        public int ClassA_Of_Cnt { get; set; }
        public int ClassA_OfCand_Cnt { get; set; }
        public int ClassA_Ser_Cnt { get; set; }
        public int ClassA_Sol_Cnt { get; set; }
        public int ClassB_Of_Cnt { get; set; }
        public int ClassB_OfCand_Cnt { get; set; }
        public int ClassB_Ser_Cnt { get; set; }
        public int ClassB_Sol_Cnt { get; set; }
        public int ClassC_Of_Cnt { get; set; }
        public int ClassC_OfCand_Cnt { get; set; }
        public int ClassC_Ser_Cnt { get; set; }
        public int ClassC_Sol_Cnt { get; set; }
        public int Total_Of_Cnt { get; set; }
        public int Total_OfCand_Cnt { get; set; }
        public int Total_Ser_Cnt { get; set; }
        public int Total_Sol_Cnt { get; set; }
        public int TotalCnt { get; set; }
        public int Rank { get; set; } 
    }

    public class ReportSV1Result
    {
        public List<ReportSV1Block> AllBlocks { get; set; }
        public ReportSV1Filter Filter { get; set; }

        public int RankCount
        {
            get
            {
                return AllBlocks.GroupBy(x => x.Rank).Count();
            }
        }

        public int MaxPage
        {
            get
            {
                return Filter.PageSize == 0 ? 1 : RankCount / Filter.PageSize + (RankCount != 0 && RankCount % Filter.PageSize == 0 ? 0 : 1);
            }
        }

        public List<ReportSV1Block> PagedBlocks
        {
            get
            {
                return AllBlocks.Where(x => x.Rank > (Filter.PageIdx - 1) * Filter.PageSize && x.Rank < (Filter.PageIdx * Filter.PageSize) + 1).ToList();
            }
        }
    }

    public static class ReportSV1Util
    {
        public static ReportSV1Result GetReportSV1(ReportSV1Filter filter, User currentUser)
        {
            ReportSV1Result reportResult = new ReportSV1Result();
            List<ReportSV1Block> reportBlocks = new List<ReportSV1Block>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string wherePersons = "";

                if (!String.IsNullOrEmpty(filter.MilitaryDepartmentIds))
                {
                    wherePersons += (wherePersons == "" ? "" : " AND ") +
                                    " c.SourceMilDepartmentID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryDepartmentIds) + ") ";
                }

                wherePersons += (wherePersons == "" ? "" : " AND ") +
                         @" (c.SourceMilDepartmentID IS NULL OR c.SourceMilDepartmentID IN ( " + currentUser.MilitaryDepartmentIDs + ")) ";
                                               
                if (!string.IsNullOrEmpty(filter.Region))
                {
                    wherePersons += (wherePersons == "" ? "" : " AND ") +
                             @" d.KOD_NMA_MJ IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBL IN ( " + filter.Region + ") ) ";
                }

                if (!string.IsNullOrEmpty(filter.Municipality))
                {
                    wherePersons += (wherePersons == "" ? "" : " AND ") +
                             @" d.KOD_NMA_MJ IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBS IN ( " + filter.Municipality + ") ) ";
                }

                if (!string.IsNullOrEmpty(filter.City))
                {
                    wherePersons += (wherePersons == "" ? "" : " AND ") +
                             @" d.KOD_NMA_MJ IN ( " + filter.City + ") ";
                }

                if (!string.IsNullOrEmpty(filter.District))
                {
                    wherePersons += (wherePersons == "" ? "" : " AND ") +
                             @" d.PermAddrDistrictID IN ( " + filter.District + ") ";
                }

                if (!string.IsNullOrEmpty(filter.Address))
                {
                    wherePersons += (wherePersons == "" ? "" : " AND ") +
                             " UPPER(d.ADRES) LIKE UPPER('%" + filter.Address.Replace("'", "''") + "%') ";
                }

                if (!string.IsNullOrEmpty(filter.PostCode))
                {
                    wherePersons += (wherePersons == "" ? "" : " AND ") +
                             @" d.PermSecondPostCode LIKE '%" + filter.PostCode.Replace("'", "''") + "%' ";
                }
                
                wherePersons = (wherePersons == "" ? "" : " WHERE ") + wherePersons;



                string whereCommands = "";

                if (!String.IsNullOrEmpty(filter.MilitaryForceSortIds))
                {
                    whereCommands += (whereCommands == "" ? "" : " AND ") +
                                    " a.RODID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryForceSortIds) + ") ";
                }
                else
                {
                    whereCommands += (whereCommands == "" ? "" : " AND ") +
                                    " (a.Active = 1 AND a1.IsActive = 1 AND a2.IsActive = 1) ";
                }

                whereCommands = (" WHERE " + whereCommands);

                if (!String.IsNullOrEmpty(filter.MilitaryReportSpecialityIds))
                {
                    whereCommands += " AND " + CommonFunctions.GetOracleSQLINClause("b.MilReportSpecialityID", filter.MilitaryReportSpecialityIds);
                }


                string whereTechnicsRequests = "";

                if (!String.IsNullOrEmpty(filter.MilitaryDepartmentIds))
                {
                    whereTechnicsRequests += (whereTechnicsRequests == "" ? "" : " AND ") +
                                    " d.MilitaryDepartmentID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryDepartmentIds) + ") ";
                }

                whereTechnicsRequests += (whereTechnicsRequests == "" ? "" : " AND ") +
                         @" (d.MilitaryDepartmentID IS NULL OR d.MilitaryDepartmentID IN ( " + currentUser.MilitaryDepartmentIDs + ")) ";


                whereTechnicsRequests = (whereTechnicsRequests == "" ? "" : " WHERE ") + whereTechnicsRequests;

                string SQL = @"
SELECT
     a1.MilitaryStructureID,
     a1.MilitaryStructureName,
     NVL(a1.Seq, 9999) as MilitaryStructureOrder,

     a2.MilitaryForceTypeID,
     a2.MilitaryForceTypeName,
     NVL(a2.Seq, 9999) as MilitaryForceTypeOrder,
 
     a.RODID as MilitaryForceSortID,
     a.ROD_IME as MilitaryForceSortName,
     NVL(a.ROD_SEQ, 9999) as MilitaryForceSortOrder,

	 b.MilReportSpecialityID, b.MilReportingSpecialityCode,

	 c.MilitaryReportStatusID, c.MilitaryReportStatusName, c.MilitaryReportStatusOrder,
     c.MilitaryReportStatusKey,
	 SUM(CASE WHEN d.Age > 0 AND d.Age <= 35 AND d.Category = 'OFFICER' THEN 1 ELSE 0 END) as ClassA_Of_Cnt,
     SUM(CASE WHEN d.Age > 0 AND d.Age <= 35 AND d.Category = 'OFFICER_CANDIDATE' THEN 1 ELSE 0 END) as ClassA_OfCand_Cnt,
	 SUM(CASE WHEN d.Age > 0 AND d.Age <= 35 AND d.Category = 'SERGEANT' THEN 1 ELSE 0 END) as ClassA_Ser_Cnt,
	 SUM(CASE WHEN d.Age > 0 AND d.Age <= 35 AND d.Category = 'SOLDIER' THEN 1 ELSE 0 END) as ClassA_Sol_Cnt,
	 SUM(CASE WHEN d.Age > 35 AND d.Age <= 45 AND d.Category = 'OFFICER' THEN 1 ELSE 0 END) as ClassB_Of_Cnt,
     SUM(CASE WHEN d.Age > 35 AND d.Age <= 45 AND d.Category = 'OFFICER_CANDIDATE' THEN 1 ELSE 0 END) as ClassB_OfCand_Cnt, 
	 SUM(CASE WHEN d.Age > 35 AND d.Age <= 45 AND d.Category = 'SERGEANT' THEN 1 ELSE 0 END) as ClassB_Ser_Cnt,
	 SUM(CASE WHEN d.Age > 35 AND d.Age <= 45 AND d.Category = 'SOLDIER' THEN 1 ELSE 0 END) as ClassB_Sol_Cnt,
	 SUM(CASE WHEN d.Age > 45 AND d.Category = 'OFFICER' THEN 1 ELSE 0 END) as ClassC_Of_Cnt,
     SUM(CASE WHEN d.Age > 45 AND d.Category = 'OFFICER_CANDIDATE' THEN 1 ELSE 0 END) as ClassC_OfCand_Cnt,
	 SUM(CASE WHEN d.Age > 45 AND d.Category = 'SERGEANT' THEN 1 ELSE 0 END) as ClassC_Ser_Cnt,
	 SUM(CASE WHEN d.Age > 45 AND d.Category = 'SOLDIER' THEN 1 ELSE 0 END) as ClassC_Sol_Cnt,
	 SUM(CASE WHEN d.Category = 'OFFICER' THEN 1 ELSE 0 END) as Total_Of_Cnt,
     SUM(CASE WHEN d.Category = 'OFFICER_CANDIDATE' THEN 1 ELSE 0 END) as Total_OfCand_Cnt,
	 SUM(CASE WHEN d.Category = 'SERGEANT' THEN 1 ELSE 0 END) as Total_Ser_Cnt,
	 SUM(CASE WHEN d.Category = 'SOLDIER' THEN CASE WHEN c.MilitaryReportStatusID = -1 THEN NVL(ReservistID, 0) ELSE 1 END ELSE 0 END) as Total_Sol_Cnt,
	 SUM(CASE WHEN d.Category IS NOT NULL THEN CASE WHEN c.MilitaryReportStatusID = -1 THEN NVL(ReservistID, 0) ELSE 1 END ELSE 0 END) as TotalCnt
FROM PMIS_ADM.MilitaryStructures a1
INNER JOIN PMIS_ADM.MilitaryForceTypes a2 ON a2.MilitaryStructureID = a1.MilitaryStructureID
INNER JOIN VS_OWNER.KLV_ROD a ON a.MilitaryForceTypeID = a2.MilitaryForceTypeID
INNER JOIN PMIS_ADM.MilitaryReportSpecialities b ON b.MilitaryForceSortID = a.RODID
CROSS JOIN 
 (SELECT a.MilitaryReportStatusID, 
         CASE a.MilitaryReportStatusKey
              WHEN 'COMPULSORY_RESERVE_MOB_APPOINTMENT' THEN 'С мобилизационно назначение - основно попълнение'
              WHEN 'POSTPONED' THEN 'Отсрочени'
              WHEN 'TEMPORARY_REMOVED' THEN 'Вр. отписани'
              WHEN 'FREE' THEN 'Свободни'
              ELSE a.MilitaryReportStatusName
         END as MilitaryReportStatusName,
         CASE a.MilitaryReportStatusKey
              WHEN 'VOLUNTARY_RESERVE' THEN 10
              WHEN 'COMPULSORY_RESERVE_MOB_APPOINTMENT' THEN 20
              WHEN 'POSTPONED' THEN 30
              WHEN 'TEMPORARY_REMOVED' THEN 40
              WHEN 'FREE' THEN 50              
              ELSE 0
         END as MilitaryReportStatusOrder,
         a.MilitaryReportStatusKey
  FROM PMIS_RES.MilitaryReportStatuses a
  WHERE a.MilitaryReportstatusKey NOT IN ('REMOVED', 'MILITARY_EMPLOYED', 'DISCHARGED', 'AUXILIARY', 'MILITARY_REPORT_PERSONS')
  
  UNION 
  
  SELECT -1 as MilitaryReportStatusID, 'получили МН с техника-запас' as MilitaryReportStatusName,
         51 as MilitaryReportStatusOrder,
         '-1' as MilitaryReportStatusKey
  FROM dual

  UNION 
  
  SELECT -2 as MilitaryReportStatusID, 'С мобилизационно назначение - резерв' as MilitaryReportStatusName,
         21 as MilitaryReportStatusOrder,
         '-2' as MilitaryReportStatusKey
  FROM dual
 ) c
LEFT OUTER JOIN 
(
	SELECT a.ReservistID, b.MilReportSpecialityID,
           CASE WHEN c1.MilitaryReportStatusKey = 'COMPULSORY_RESERVE_MOB_APPOINTMENT' AND 
                     g.ReservistReadinessID = 2
                THEN -2
                ELSE c.MilitaryReportStatusID
           END as MilitaryReportStatusID,
		   CASE WHEN f.MilitaryRankCategory = 2
				THEN 'OFFICER'
				ELSE CASE WHEN f.MilitaryRankCategory = 1
						  THEN CASE WHEN f.MilitaryRankSubCategory = 1
                                    THEN 'SERGEANT'
                                    ELSE CASE WHEN f.MilitaryRankSubCategory = 2
                                              THEN 'SOLDIER'
                                              ELSE CASE WHEN f.MilitaryRankSubCategory = 3
                                                        THEN 'OFFICER_CANDIDATE'
                                                        ELSE ''
                                                   END
                                         END
							   END
						  ELSE ''
					 END
		   END as Category,
		   PMIS_ADM.COMMONFUNCTIONS.GetAgeFromEGN(d.EGN) as Age
	FROM PMIS_RES.Reservists a
	INNER JOIN PMIS_ADM.PersonMilRepSpec b ON a.PersonID = b.PersonID AND b.IsPrimary = 1
	INNER JOIN PMIS_RES.ReservistMilRepStatuses c ON a.ReservistID = c.ReservistID AND c.IsCurrent = 1
    INNER JOIN PMIS_RES.MilitaryReportStatuses c1 ON c.MilitaryReportStatusID = c1.MilitaryReportStatusID 
                                                  AND c1.MilitaryReportStatusKey <> 'MILITARY_REPORT_PERSONS' 
	INNER JOIN VS_OWNER.VS_LS d ON a.PersonID = d.PersonID
    INNER JOIN PMIS_ADM.Persons h ON h.PersonID = d.PersonID
	INNER JOIN VS_OWNER.KLV_ZVA e ON d.KOD_ZVA = e.ZVA_KOD
	INNER JOIN PMIS_ADM.MilitaryRankCategories f ON e.ZVA_KAT_KOD = f.ZVA_KAT_KOD
    LEFT OUTER JOIN PMIS_RES.FillReservistsRequest g ON g.ReservistID = a.ReservistID
    " + wherePersons + @"

    UNION ALL

    SELECT d.DriversCount as ReservistID, 
           (SELECT MilReportSpecialityID  FROM (SELECT * FROM PMIS_ADM.MilitaryReportSpecialities WHERE MilReportingSpecialityCode = '5233' ORDER BY MilReportSpecialityID  ) WHERE ROWNUM = 1) as MilReportSpecialityID,
           -1 as MilitaryReportStatusID,
           'SOLDIER' as Category,
           0 as Age
    FROM PMIS_RES.EquipmentTechnicsRequests a
    INNER JOIN PMIS_RES.TechnicsRequestCommands b ON a.EquipmentTechnicsRequestID = b.EquipmentTechnicsRequestID
    INNER JOIN PMIS_RES.TechnicsRequestCmdPositions c ON b.TechRequestsCommandID = c.TechRequestsCommandID
    INNER JOIN PMIS_RES.TechRequestCmdPositionsMilDept d ON c.TechnicsRequestCmdPositionID = d.TechnicsRequestCmdPositionID
    " + whereTechnicsRequests + @"
) d ON b.MilReportSpecialityID = d.MilReportSpecialityID AND c.MilitaryReportStatusID = d.MilitaryReportStatusID
" + whereCommands + @"
GROUP BY a1.Seq, a1.MilitaryStructureName, a1.MilitaryStructureID,
         a2.Seq, a2.MilitaryForceTypeName, a2.MilitaryForceTypeID,
         a.ROD_SEQ, a.ROD_IME, a.RODID,
	     b.MilReportSpecialityID, b.MilReportingSpecialityCode,
	     c.MilitaryReportStatusID, c.MilitaryReportStatusName, c.MilitaryReportStatusOrder,
         c.MilitaryReportStatusKey
";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ReportSV1Block reportBlock = new ReportSV1Block();

                    reportBlock.MilitaryStructureID = DBCommon.GetInt(dr["MilitaryStructureID"]);
                    reportBlock.MilitaryStructureName = dr["MilitaryStructureName"].ToString();
                    reportBlock.MilitaryStructureOrder = DBCommon.GetInt(dr["MilitaryStructureOrder"]);

                    reportBlock.MilitaryForceTypeID = DBCommon.GetInt(dr["MilitaryForceTypeID"]);
                    reportBlock.MilitaryForceTypeName = dr["MilitaryForceTypeName"].ToString();
                    reportBlock.MilitaryForceTypeOrder = DBCommon.GetInt(dr["MilitaryForceTypeOrder"]);

                    reportBlock.MilitaryForceSortID = DBCommon.GetInt(dr["MilitaryForceSortID"]);
                    reportBlock.MilitaryForceSortName = dr["MilitaryForceSortName"].ToString();
                    reportBlock.MilitaryForceSortOrder = DBCommon.GetInt(dr["MilitaryForceSortOrder"]);

                    reportBlock.MilRepSpecialityID = DBCommon.GetInt(dr["MilReportSpecialityID"]);
                    reportBlock.MilRepSpecialityCode = dr["MilReportingSpecialityCode"].ToString();

                    reportBlock.MilRepStatusID = DBCommon.GetInt(dr["MilitaryReportStatusID"]);
                    reportBlock.MilRepStatusName = dr["MilitaryReportStatusName"].ToString();
                    reportBlock.MilRepStatusOrder = DBCommon.GetInt(dr["MilitaryReportStatusOrder"]);
                    reportBlock.MilRepStatusKey = dr["MilitaryReportStatusKey"].ToString();
                    reportBlock.RowType = 1;
                    reportBlock.ClassA_Of_Cnt = DBCommon.GetInt(dr["ClassA_Of_Cnt"]);
                    reportBlock.ClassA_OfCand_Cnt = DBCommon.GetInt(dr["ClassA_OfCand_Cnt"]);
                    reportBlock.ClassA_Ser_Cnt = DBCommon.GetInt(dr["ClassA_Ser_Cnt"]);
                    reportBlock.ClassA_Sol_Cnt = DBCommon.GetInt(dr["ClassA_Sol_Cnt"]);
                    reportBlock.ClassB_Of_Cnt = DBCommon.GetInt(dr["ClassB_Of_Cnt"]);
                    reportBlock.ClassB_OfCand_Cnt = DBCommon.GetInt(dr["ClassB_OfCand_Cnt"]);
                    reportBlock.ClassB_Ser_Cnt = DBCommon.GetInt(dr["ClassB_Ser_Cnt"]);
                    reportBlock.ClassB_Sol_Cnt = DBCommon.GetInt(dr["ClassB_Sol_Cnt"]);
                    reportBlock.ClassC_Of_Cnt = DBCommon.GetInt(dr["ClassC_Of_Cnt"]);
                    reportBlock.ClassC_OfCand_Cnt = DBCommon.GetInt(dr["ClassC_OfCand_Cnt"]);
                    reportBlock.ClassC_Ser_Cnt = DBCommon.GetInt(dr["ClassC_Ser_Cnt"]);
                    reportBlock.ClassC_Sol_Cnt = DBCommon.GetInt(dr["ClassC_Sol_Cnt"]);
                    reportBlock.Total_Of_Cnt = DBCommon.GetInt(dr["Total_Of_Cnt"]);
                    reportBlock.Total_OfCand_Cnt = DBCommon.GetInt(dr["Total_OfCand_Cnt"]);
                    reportBlock.Total_Ser_Cnt = DBCommon.GetInt(dr["Total_Ser_Cnt"]);
                    reportBlock.Total_Sol_Cnt = DBCommon.GetInt(dr["Total_Sol_Cnt"]);
                    reportBlock.TotalCnt = DBCommon.GetInt(dr["TotalCnt"]);

                    reportBlocks.Add(reportBlock);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            var milRepSpecialityTotals_CheckForEmpty = (from a in reportBlocks
                                                        group a by a.MilRepSpecialityID into g
                                                        select new ReportSV1Block
                                                          {
                                                              MilitaryStructureID = g.Max(x => x.MilitaryStructureID),
                                                              MilitaryStructureName = g.Max(x => x.MilitaryStructureName),
                                                              MilitaryStructureOrder = g.Max(x => x.MilitaryStructureOrder),
                                                              MilitaryForceTypeID = g.Max(x => x.MilitaryForceTypeID),
                                                              MilitaryForceTypeName = g.Max(x => x.MilitaryForceTypeName),
                                                              MilitaryForceTypeOrder = g.Max(x => x.MilitaryForceTypeOrder),
                                                              MilitaryForceSortID = g.Max(x => x.MilitaryForceSortID),
                                                              MilitaryForceSortName = g.Max(x => x.MilitaryForceSortName),
                                                              MilitaryForceSortOrder = g.Max(x => x.MilitaryForceSortOrder),
                                                              MilRepSpecialityID = g.Key,
                                                              MilRepSpecialityCode = g.Max(x => x.MilRepSpecialityCode),
                                                              MilRepStatusID = 0,
                                                              MilRepStatusName = "",
                                                              MilRepStatusOrder = 0,
                                                              MilRepStatusKey = "",
                                                              RowType = 2,
                                                              ClassA_Of_Cnt = g.Sum(x => x.ClassA_Of_Cnt),
                                                              ClassA_OfCand_Cnt = g.Sum(x => x.ClassA_OfCand_Cnt),
                                                              ClassA_Ser_Cnt = g.Sum(x => x.ClassA_Ser_Cnt),
                                                              ClassA_Sol_Cnt = g.Sum(x => x.ClassA_Sol_Cnt),
                                                              ClassB_Of_Cnt = g.Sum(x => x.ClassB_Of_Cnt),
                                                              ClassB_OfCand_Cnt = g.Sum(x => x.ClassB_OfCand_Cnt),
                                                              ClassB_Ser_Cnt = g.Sum(x => x.ClassB_Ser_Cnt),
                                                              ClassB_Sol_Cnt = g.Sum(x => x.ClassB_Sol_Cnt),
                                                              ClassC_Of_Cnt = g.Sum(x => x.ClassC_Of_Cnt),
                                                              ClassC_OfCand_Cnt = g.Sum(x => x.ClassC_OfCand_Cnt),
                                                              ClassC_Ser_Cnt = g.Sum(x => x.ClassC_Ser_Cnt),
                                                              ClassC_Sol_Cnt = g.Sum(x => x.ClassC_Sol_Cnt),
                                                              Total_Of_Cnt = g.Sum(x => x.Total_Of_Cnt),
                                                              Total_OfCand_Cnt = g.Sum(x => x.Total_OfCand_Cnt),
                                                              Total_Ser_Cnt = g.Sum(x => x.Total_Ser_Cnt),
                                                              Total_Sol_Cnt = g.Sum(x => x.Total_Sol_Cnt),
                                                              TotalCnt = g.Sum(x => x.TotalCnt)
                                                          }).ToList();

            var emptyMilRepSpecialities = (from s in milRepSpecialityTotals_CheckForEmpty
                                           where s.TotalCnt == 0
                                           select s.MilRepSpecialityID).ToList();

            reportBlocks.RemoveAll(x => emptyMilRepSpecialities.Contains(x.MilRepSpecialityID));

            var milRepSpecialityTotals = (from a in reportBlocks
                                          where a.MilRepStatusID != -1
                                          group a by a.MilRepSpecialityID into g
                                          select new ReportSV1Block
                                          {
                                              MilitaryStructureID = g.Max(x => x.MilitaryStructureID),
                                              MilitaryStructureName = g.Max(x => x.MilitaryStructureName),
                                              MilitaryStructureOrder = g.Max(x => x.MilitaryStructureOrder),
                                              MilitaryForceTypeID = g.Max(x => x.MilitaryForceTypeID),
                                              MilitaryForceTypeName = g.Max(x => x.MilitaryForceTypeName),
                                              MilitaryForceTypeOrder = g.Max(x => x.MilitaryForceTypeOrder),
                                              MilitaryForceSortID = g.Max(x => x.MilitaryForceSortID),
                                              MilitaryForceSortName = g.Max(x => x.MilitaryForceSortName),
                                              MilitaryForceSortOrder = g.Max(x => x.MilitaryForceSortOrder),
                                              MilRepSpecialityID = g.Key,
                                              MilRepSpecialityCode = g.Max(x => x.MilRepSpecialityCode),
                                              MilRepStatusID = 0,
                                              MilRepStatusName = "",
                                              MilRepStatusOrder = 0,
                                              MilRepStatusKey = "",
                                              RowType = 2,
                                              ClassA_Of_Cnt = g.Sum(x => x.ClassA_Of_Cnt),
                                              ClassA_OfCand_Cnt = g.Sum(x => x.ClassA_OfCand_Cnt),
                                              ClassA_Ser_Cnt = g.Sum(x => x.ClassA_Ser_Cnt),
                                              ClassA_Sol_Cnt = g.Sum(x => x.ClassA_Sol_Cnt),
                                              ClassB_Of_Cnt = g.Sum(x => x.ClassB_Of_Cnt),
                                              ClassB_OfCand_Cnt = g.Sum(x => x.ClassB_OfCand_Cnt),
                                              ClassB_Ser_Cnt = g.Sum(x => x.ClassB_Ser_Cnt),
                                              ClassB_Sol_Cnt = g.Sum(x => x.ClassB_Sol_Cnt),
                                              ClassC_Of_Cnt = g.Sum(x => x.ClassC_Of_Cnt),
                                              ClassC_OfCand_Cnt = g.Sum(x => x.ClassC_OfCand_Cnt),
                                              ClassC_Ser_Cnt = g.Sum(x => x.ClassC_Ser_Cnt),
                                              ClassC_Sol_Cnt = g.Sum(x => x.ClassC_Sol_Cnt),
                                              Total_Of_Cnt = g.Sum(x => x.Total_Of_Cnt),
                                              Total_OfCand_Cnt = g.Sum(x => x.Total_OfCand_Cnt),
                                              Total_Ser_Cnt = g.Sum(x => x.Total_Ser_Cnt),
                                              Total_Sol_Cnt = g.Sum(x => x.Total_Sol_Cnt),
                                              TotalCnt = g.Sum(x => x.TotalCnt)
                                          }).ToList();

            var milForceSortDetails = (from a in reportBlocks
                                       group a by new { a.MilitaryForceSortID, a.MilRepStatusID } into g
                                       select new ReportSV1Block
                                         {
                                             MilitaryStructureID = g.Max(x => x.MilitaryStructureID),
                                             MilitaryStructureName = g.Max(x => x.MilitaryStructureName),
                                             MilitaryStructureOrder = g.Max(x => x.MilitaryStructureOrder),
                                             MilitaryForceTypeID = g.Max(x => x.MilitaryForceTypeID),
                                             MilitaryForceTypeName = g.Max(x => x.MilitaryForceTypeName),
                                             MilitaryForceTypeOrder = g.Max(x => x.MilitaryForceTypeOrder),
                                             MilitaryForceSortID = g.Key.MilitaryForceSortID,
                                             MilitaryForceSortName = g.Max(x => x.MilitaryForceSortName),
                                             MilitaryForceSortOrder = g.Max(x => x.MilitaryForceSortOrder),
                                             MilRepSpecialityID = 0,
                                             MilRepSpecialityCode = "",
                                             MilRepStatusID = g.Key.MilRepStatusID,
                                             MilRepStatusName = g.Max(x => x.MilRepStatusName),
                                             MilRepStatusOrder = g.Max(x => x.MilRepStatusOrder),
                                             MilRepStatusKey = g.Max(x => x.MilRepStatusKey),
                                             RowType = 1,
                                             ClassA_Of_Cnt = g.Sum(x => x.ClassA_Of_Cnt),
                                             ClassA_OfCand_Cnt = g.Sum(x => x.ClassA_OfCand_Cnt),
                                             ClassA_Ser_Cnt = g.Sum(x => x.ClassA_Ser_Cnt),
                                             ClassA_Sol_Cnt = g.Sum(x => x.ClassA_Sol_Cnt),
                                             ClassB_Of_Cnt = g.Sum(x => x.ClassB_Of_Cnt),
                                             ClassB_OfCand_Cnt = g.Sum(x => x.ClassB_OfCand_Cnt),
                                             ClassB_Ser_Cnt = g.Sum(x => x.ClassB_Ser_Cnt),
                                             ClassB_Sol_Cnt = g.Sum(x => x.ClassB_Sol_Cnt),
                                             ClassC_Of_Cnt = g.Sum(x => x.ClassC_Of_Cnt),
                                             ClassC_OfCand_Cnt = g.Sum(x => x.ClassC_OfCand_Cnt),
                                             ClassC_Ser_Cnt = g.Sum(x => x.ClassC_Ser_Cnt),
                                             ClassC_Sol_Cnt = g.Sum(x => x.ClassC_Sol_Cnt),
                                             Total_Of_Cnt = g.Sum(x => x.Total_Of_Cnt),
                                             Total_OfCand_Cnt = g.Sum(x => x.Total_OfCand_Cnt),
                                             Total_Ser_Cnt = g.Sum(x => x.Total_Ser_Cnt),
                                             Total_Sol_Cnt = g.Sum(x => x.Total_Sol_Cnt),
                                             TotalCnt = g.Sum(x => x.TotalCnt)
                                         }).ToList();

            var milForceSortTotals = (from a in reportBlocks
                                      where a.MilRepStatusID != -1
                                      group a by a.MilitaryForceSortID into g
                                      select new ReportSV1Block
                                      {
                                          MilitaryStructureID = g.Max(x => x.MilitaryStructureID),
                                          MilitaryStructureName = g.Max(x => x.MilitaryStructureName),
                                          MilitaryStructureOrder = g.Max(x => x.MilitaryStructureOrder),
                                          MilitaryForceTypeID = g.Max(x => x.MilitaryForceTypeID),
                                          MilitaryForceTypeName = g.Max(x => x.MilitaryForceTypeName),
                                          MilitaryForceTypeOrder = g.Max(x => x.MilitaryForceTypeOrder),
                                          MilitaryForceSortID = g.Key,
                                          MilitaryForceSortName = g.Max(x => x.MilitaryForceSortName),
                                          MilitaryForceSortOrder = g.Max(x => x.MilitaryForceSortOrder),
                                          MilRepSpecialityID = 0,
                                          MilRepSpecialityCode = "",
                                          MilRepStatusID = 0,
                                          MilRepStatusName = "",
                                          MilRepStatusOrder = 0,
                                          MilRepStatusKey = "",
                                          RowType = 2,
                                          ClassA_Of_Cnt = g.Sum(x => x.ClassA_Of_Cnt),
                                          ClassA_OfCand_Cnt = g.Sum(x => x.ClassA_OfCand_Cnt),
                                          ClassA_Ser_Cnt = g.Sum(x => x.ClassA_Ser_Cnt),
                                          ClassA_Sol_Cnt = g.Sum(x => x.ClassA_Sol_Cnt),
                                          ClassB_Of_Cnt = g.Sum(x => x.ClassB_Of_Cnt),
                                          ClassB_OfCand_Cnt = g.Sum(x => x.ClassB_OfCand_Cnt),
                                          ClassB_Ser_Cnt = g.Sum(x => x.ClassB_Ser_Cnt),
                                          ClassB_Sol_Cnt = g.Sum(x => x.ClassB_Sol_Cnt),
                                          ClassC_Of_Cnt = g.Sum(x => x.ClassC_Of_Cnt),
                                          ClassC_OfCand_Cnt = g.Sum(x => x.ClassC_OfCand_Cnt),
                                          ClassC_Ser_Cnt = g.Sum(x => x.ClassC_Ser_Cnt),
                                          ClassC_Sol_Cnt = g.Sum(x => x.ClassC_Sol_Cnt),
                                          Total_Of_Cnt = g.Sum(x => x.Total_Of_Cnt),
                                          Total_OfCand_Cnt = g.Sum(x => x.Total_OfCand_Cnt),
                                          Total_Ser_Cnt = g.Sum(x => x.Total_Ser_Cnt),
                                          Total_Sol_Cnt = g.Sum(x => x.Total_Sol_Cnt),
                                          TotalCnt = g.Sum(x => x.TotalCnt)
                                      }).ToList();

            var milForceTypeDetails = (from a in reportBlocks
                                       group a by new { a.MilitaryForceTypeID, a.MilRepStatusID } into g
                                       select new ReportSV1Block
                                       {
                                           MilitaryStructureID = g.Max(x => x.MilitaryStructureID),
                                           MilitaryStructureName = g.Max(x => x.MilitaryStructureName),
                                           MilitaryStructureOrder = g.Max(x => x.MilitaryStructureOrder),
                                           MilitaryForceTypeID = g.Key.MilitaryForceTypeID,
                                           MilitaryForceTypeName = g.Max(x => x.MilitaryForceTypeName),
                                           MilitaryForceTypeOrder = g.Max(x => x.MilitaryForceTypeOrder),
                                           MilitaryForceSortID = 0,
                                           MilitaryForceSortName = "",
                                           MilitaryForceSortOrder = 0,
                                           MilRepSpecialityID = 0,
                                           MilRepSpecialityCode = "",
                                           MilRepStatusID = g.Key.MilRepStatusID,
                                           MilRepStatusName = g.Max(x => x.MilRepStatusName),
                                           MilRepStatusOrder = g.Max(x => x.MilRepStatusOrder),
                                           MilRepStatusKey = g.Max(x => x.MilRepStatusKey),
                                           RowType = 1,
                                           ClassA_Of_Cnt = g.Sum(x => x.ClassA_Of_Cnt),
                                           ClassA_OfCand_Cnt = g.Sum(x => x.ClassA_OfCand_Cnt),
                                           ClassA_Ser_Cnt = g.Sum(x => x.ClassA_Ser_Cnt),
                                           ClassA_Sol_Cnt = g.Sum(x => x.ClassA_Sol_Cnt),
                                           ClassB_Of_Cnt = g.Sum(x => x.ClassB_Of_Cnt),
                                           ClassB_OfCand_Cnt = g.Sum(x => x.ClassB_OfCand_Cnt),
                                           ClassB_Ser_Cnt = g.Sum(x => x.ClassB_Ser_Cnt),
                                           ClassB_Sol_Cnt = g.Sum(x => x.ClassB_Sol_Cnt),
                                           ClassC_Of_Cnt = g.Sum(x => x.ClassC_Of_Cnt),
                                           ClassC_OfCand_Cnt = g.Sum(x => x.ClassC_OfCand_Cnt),
                                           ClassC_Ser_Cnt = g.Sum(x => x.ClassC_Ser_Cnt),
                                           ClassC_Sol_Cnt = g.Sum(x => x.ClassC_Sol_Cnt),
                                           Total_Of_Cnt = g.Sum(x => x.Total_Of_Cnt),
                                           Total_OfCand_Cnt = g.Sum(x => x.Total_OfCand_Cnt),
                                           Total_Ser_Cnt = g.Sum(x => x.Total_Ser_Cnt),
                                           Total_Sol_Cnt = g.Sum(x => x.Total_Sol_Cnt),
                                           TotalCnt = g.Sum(x => x.TotalCnt)
                                       }).ToList();

            var milForceTypeTotals = (from a in reportBlocks
                                      where a.MilRepStatusID != -1
                                      group a by a.MilitaryForceTypeID into g
                                      select new ReportSV1Block
                                      {
                                          MilitaryStructureID = g.Max(x => x.MilitaryStructureID),
                                          MilitaryStructureName = g.Max(x => x.MilitaryStructureName),
                                          MilitaryStructureOrder = g.Max(x => x.MilitaryStructureOrder),
                                          MilitaryForceTypeID = g.Key,
                                          MilitaryForceTypeName = g.Max(x => x.MilitaryForceTypeName),
                                          MilitaryForceTypeOrder = g.Max(x => x.MilitaryForceTypeOrder),
                                          MilitaryForceSortID = 0,
                                          MilitaryForceSortName = "",
                                          MilitaryForceSortOrder = 0,
                                          MilRepSpecialityID = 0,
                                          MilRepSpecialityCode = "",
                                          MilRepStatusID = 0,
                                          MilRepStatusName = "",
                                          MilRepStatusOrder = 0,
                                          MilRepStatusKey = "",
                                          RowType = 2,
                                          ClassA_Of_Cnt = g.Sum(x => x.ClassA_Of_Cnt),
                                          ClassA_OfCand_Cnt = g.Sum(x => x.ClassA_OfCand_Cnt),
                                          ClassA_Ser_Cnt = g.Sum(x => x.ClassA_Ser_Cnt),
                                          ClassA_Sol_Cnt = g.Sum(x => x.ClassA_Sol_Cnt),
                                          ClassB_Of_Cnt = g.Sum(x => x.ClassB_Of_Cnt),
                                          ClassB_OfCand_Cnt = g.Sum(x => x.ClassB_OfCand_Cnt),
                                          ClassB_Ser_Cnt = g.Sum(x => x.ClassB_Ser_Cnt),
                                          ClassB_Sol_Cnt = g.Sum(x => x.ClassB_Sol_Cnt),
                                          ClassC_Of_Cnt = g.Sum(x => x.ClassC_Of_Cnt),
                                          ClassC_OfCand_Cnt = g.Sum(x => x.ClassC_OfCand_Cnt),
                                          ClassC_Ser_Cnt = g.Sum(x => x.ClassC_Ser_Cnt),
                                          ClassC_Sol_Cnt = g.Sum(x => x.ClassC_Sol_Cnt),
                                          Total_Of_Cnt = g.Sum(x => x.Total_Of_Cnt),
                                          Total_OfCand_Cnt = g.Sum(x => x.Total_OfCand_Cnt),
                                          Total_Ser_Cnt = g.Sum(x => x.Total_Ser_Cnt),
                                          Total_Sol_Cnt = g.Sum(x => x.Total_Sol_Cnt),
                                          TotalCnt = g.Sum(x => x.TotalCnt)
                                      }).ToList();

            var milStructureDetails = (from a in reportBlocks
                                       group a by new { a.MilitaryStructureID, a.MilRepStatusID } into g
                                       select new ReportSV1Block
                                       {
                                           MilitaryStructureID = g.Key.MilitaryStructureID,
                                           MilitaryStructureName = g.Max(x => x.MilitaryStructureName),
                                           MilitaryStructureOrder = g.Max(x => x.MilitaryStructureOrder),
                                           MilitaryForceTypeID = 0,
                                           MilitaryForceTypeName = "",
                                           MilitaryForceTypeOrder = 0,
                                           MilitaryForceSortID = 0,
                                           MilitaryForceSortName = "",
                                           MilitaryForceSortOrder = 0,
                                           MilRepSpecialityID = 0,
                                           MilRepSpecialityCode = "",
                                           MilRepStatusID = g.Key.MilRepStatusID,
                                           MilRepStatusName = g.Max(x => x.MilRepStatusName),
                                           MilRepStatusOrder = g.Max(x => x.MilRepStatusOrder),
                                           MilRepStatusKey = g.Max(x => x.MilRepStatusKey),
                                           RowType = 1,
                                           ClassA_Of_Cnt = g.Sum(x => x.ClassA_Of_Cnt),
                                           ClassA_OfCand_Cnt = g.Sum(x => x.ClassA_OfCand_Cnt),
                                           ClassA_Ser_Cnt = g.Sum(x => x.ClassA_Ser_Cnt),
                                           ClassA_Sol_Cnt = g.Sum(x => x.ClassA_Sol_Cnt),
                                           ClassB_Of_Cnt = g.Sum(x => x.ClassB_Of_Cnt),
                                           ClassB_OfCand_Cnt = g.Sum(x => x.ClassB_OfCand_Cnt),
                                           ClassB_Ser_Cnt = g.Sum(x => x.ClassB_Ser_Cnt),
                                           ClassB_Sol_Cnt = g.Sum(x => x.ClassB_Sol_Cnt),
                                           ClassC_Of_Cnt = g.Sum(x => x.ClassC_Of_Cnt),
                                           ClassC_OfCand_Cnt = g.Sum(x => x.ClassC_OfCand_Cnt),
                                           ClassC_Ser_Cnt = g.Sum(x => x.ClassC_Ser_Cnt),
                                           ClassC_Sol_Cnt = g.Sum(x => x.ClassC_Sol_Cnt),
                                           Total_Of_Cnt = g.Sum(x => x.Total_Of_Cnt),
                                           Total_OfCand_Cnt = g.Sum(x => x.Total_OfCand_Cnt),
                                           Total_Ser_Cnt = g.Sum(x => x.Total_Ser_Cnt),
                                           Total_Sol_Cnt = g.Sum(x => x.Total_Sol_Cnt),
                                           TotalCnt = g.Sum(x => x.TotalCnt)
                                       }).ToList();

            var milStructureTotals = (from a in reportBlocks
                                      where a.MilRepStatusID != -1
                                      group a by a.MilitaryStructureID into g
                                      select new ReportSV1Block
                                      {
                                          MilitaryStructureID = g.Key,
                                          MilitaryStructureName = g.Max(x => x.MilitaryStructureName),
                                          MilitaryStructureOrder = g.Max(x => x.MilitaryStructureOrder),
                                          MilitaryForceTypeID = 0,
                                          MilitaryForceTypeName = "",
                                          MilitaryForceTypeOrder = 0,
                                          MilitaryForceSortID = 0,
                                          MilitaryForceSortName = "",
                                          MilitaryForceSortOrder = 0,
                                          MilRepSpecialityID = 0,
                                          MilRepSpecialityCode = "",
                                          MilRepStatusID = 0,
                                          MilRepStatusName = "",
                                          MilRepStatusOrder = 0,
                                          MilRepStatusKey = "",
                                          RowType = 2,
                                          ClassA_Of_Cnt = g.Sum(x => x.ClassA_Of_Cnt),
                                          ClassA_OfCand_Cnt = g.Sum(x => x.ClassA_OfCand_Cnt),
                                          ClassA_Ser_Cnt = g.Sum(x => x.ClassA_Ser_Cnt),
                                          ClassA_Sol_Cnt = g.Sum(x => x.ClassA_Sol_Cnt),
                                          ClassB_Of_Cnt = g.Sum(x => x.ClassB_Of_Cnt),
                                          ClassB_OfCand_Cnt = g.Sum(x => x.ClassB_OfCand_Cnt),
                                          ClassB_Ser_Cnt = g.Sum(x => x.ClassB_Ser_Cnt),
                                          ClassB_Sol_Cnt = g.Sum(x => x.ClassB_Sol_Cnt),
                                          ClassC_Of_Cnt = g.Sum(x => x.ClassC_Of_Cnt),
                                          ClassC_OfCand_Cnt = g.Sum(x => x.ClassC_OfCand_Cnt),
                                          ClassC_Ser_Cnt = g.Sum(x => x.ClassC_Ser_Cnt),
                                          ClassC_Sol_Cnt = g.Sum(x => x.ClassC_Sol_Cnt),
                                          Total_Of_Cnt = g.Sum(x => x.Total_Of_Cnt),
                                          Total_OfCand_Cnt = g.Sum(x => x.Total_OfCand_Cnt),
                                          Total_Ser_Cnt = g.Sum(x => x.Total_Ser_Cnt),
                                          Total_Sol_Cnt = g.Sum(x => x.Total_Sol_Cnt),
                                          TotalCnt = g.Sum(x => x.TotalCnt)
                                      }).ToList();

            var totalDetails = (from a in reportBlocks
                                group a by a.MilRepStatusID into g
                                select new ReportSV1Block
                                {
                                   MilitaryStructureID = -1,
                                   MilitaryStructureName = "",
                                   MilitaryStructureOrder = -1,
                                   MilitaryForceTypeID = -1,
                                   MilitaryForceTypeName = "",
                                   MilitaryForceTypeOrder = -1,
                                   MilitaryForceSortID = -1,
                                   MilitaryForceSortName = "",
                                   MilitaryForceSortOrder = -1,
                                   MilRepSpecialityID = -1,
                                   MilRepSpecialityCode = "",
                                   MilRepStatusID = g.Key,
                                   MilRepStatusName = g.Max(x => x.MilRepStatusName),
                                   MilRepStatusOrder = g.Max(x => x.MilRepStatusOrder),
                                   MilRepStatusKey = g.Max(x => x.MilRepStatusKey),
                                   RowType = 1,
                                   ClassA_Of_Cnt = g.Sum(x => x.ClassA_Of_Cnt),
                                   ClassA_OfCand_Cnt = g.Sum(x => x.ClassA_OfCand_Cnt),
                                   ClassA_Ser_Cnt = g.Sum(x => x.ClassA_Ser_Cnt),
                                   ClassA_Sol_Cnt = g.Sum(x => x.ClassA_Sol_Cnt),
                                   ClassB_Of_Cnt = g.Sum(x => x.ClassB_Of_Cnt),
                                   ClassB_OfCand_Cnt = g.Sum(x => x.ClassB_OfCand_Cnt),
                                   ClassB_Ser_Cnt = g.Sum(x => x.ClassB_Ser_Cnt),
                                   ClassB_Sol_Cnt = g.Sum(x => x.ClassB_Sol_Cnt),
                                   ClassC_Of_Cnt = g.Sum(x => x.ClassC_Of_Cnt),
                                   ClassC_OfCand_Cnt = g.Sum(x => x.ClassC_OfCand_Cnt),
                                   ClassC_Ser_Cnt = g.Sum(x => x.ClassC_Ser_Cnt),
                                   ClassC_Sol_Cnt = g.Sum(x => x.ClassC_Sol_Cnt),
                                   Total_Of_Cnt = g.Sum(x => x.Total_Of_Cnt),
                                   Total_OfCand_Cnt = g.Sum(x => x.Total_OfCand_Cnt),
                                   Total_Ser_Cnt = g.Sum(x => x.Total_Ser_Cnt),
                                   Total_Sol_Cnt = g.Sum(x => x.Total_Sol_Cnt),
                                   TotalCnt = g.Sum(x => x.TotalCnt)
                                }).ToList();

            var grandTotal = (from a in reportBlocks
                              where a.MilRepStatusID != -1
                              group a by 1 into g
                              select new ReportSV1Block
                                {
                                    MilitaryStructureID = -1,
                                    MilitaryStructureName = "",
                                    MilitaryStructureOrder = -1,
                                    MilitaryForceTypeID = -1,
                                    MilitaryForceTypeName = "",
                                    MilitaryForceTypeOrder = -1,
                                    MilitaryForceSortID = -1,
                                    MilitaryForceSortName = "",
                                    MilitaryForceSortOrder = -1,
                                    MilRepSpecialityID = -1,
                                    MilRepSpecialityCode = "",
                                    MilRepStatusID = 0,
                                    MilRepStatusName = "",
                                    MilRepStatusOrder = 0,
                                    MilRepStatusKey = "",
                                    RowType = 0,
                                    ClassA_Of_Cnt = g.Sum(x => x.ClassA_Of_Cnt),
                                    ClassA_OfCand_Cnt = g.Sum(x => x.ClassA_OfCand_Cnt),
                                    ClassA_Ser_Cnt = g.Sum(x => x.ClassA_Ser_Cnt),
                                    ClassA_Sol_Cnt = g.Sum(x => x.ClassA_Sol_Cnt),
                                    ClassB_Of_Cnt = g.Sum(x => x.ClassB_Of_Cnt),
                                    ClassB_OfCand_Cnt = g.Sum(x => x.ClassB_OfCand_Cnt),
                                    ClassB_Ser_Cnt = g.Sum(x => x.ClassB_Ser_Cnt),
                                    ClassB_Sol_Cnt = g.Sum(x => x.ClassB_Sol_Cnt),
                                    ClassC_Of_Cnt = g.Sum(x => x.ClassC_Of_Cnt),
                                    ClassC_OfCand_Cnt = g.Sum(x => x.ClassC_OfCand_Cnt),
                                    ClassC_Ser_Cnt = g.Sum(x => x.ClassC_Ser_Cnt),
                                    ClassC_Sol_Cnt = g.Sum(x => x.ClassC_Sol_Cnt),
                                    Total_Of_Cnt = g.Sum(x => x.Total_Of_Cnt),
                                    Total_OfCand_Cnt = g.Sum(x => x.Total_OfCand_Cnt),
                                    Total_Ser_Cnt = g.Sum(x => x.Total_Ser_Cnt),
                                    Total_Sol_Cnt = g.Sum(x => x.Total_Sol_Cnt),
                                    TotalCnt = g.Sum(x => x.TotalCnt)
                                }).ToList();


            reportBlocks.AddRange(milRepSpecialityTotals);
            reportBlocks.AddRange(milForceSortDetails);
            reportBlocks.AddRange(milForceSortTotals);
            reportBlocks.AddRange(milForceTypeDetails);
            reportBlocks.AddRange(milForceTypeTotals);
            reportBlocks.AddRange(milStructureDetails);
            reportBlocks.AddRange(milStructureTotals);
            reportBlocks.AddRange(totalDetails);
            reportBlocks.AddRange(grandTotal);


            var sortedResult = (from a in reportBlocks
                                orderby a.MilitaryStructureOrder, a.MilitaryStructureName, a.MilitaryStructureID,
                                        a.MilitaryForceTypeOrder, a.MilitaryForceTypeName, a.MilitaryForceTypeID,
                                        a.MilitaryForceSortOrder, a.MilitaryForceSortName, a.MilitaryForceSortID,
                                        a.MilRepSpecialityCode, a.MilRepSpecialityID,
                                        a.RowType,
                                        a.MilRepStatusOrder
                                select a);

            int prevMilRepSpecialityId = -2;
            int prevMilForceSortId = -2;
            int prevMilForceTypeId = -2;
            int prevMilStructureId = -2;
            int rank = 0;
            foreach (var a in sortedResult)
            {
                if (prevMilRepSpecialityId != a.MilRepSpecialityID ||
                    prevMilForceSortId != a.MilitaryForceSortID ||
                    prevMilForceTypeId != a.MilitaryForceTypeID ||
                    prevMilStructureId != a.MilitaryStructureID)
                {
                    prevMilRepSpecialityId = a.MilRepSpecialityID;
                    prevMilForceSortId = a.MilitaryForceSortID;
                    prevMilForceTypeId = a.MilitaryForceTypeID;
                    prevMilStructureId = a.MilitaryStructureID;
                    rank++;
                }

                a.Rank = rank;
            }

            int allCnt = sortedResult.GroupBy(x => x.Rank).Count();
            
            if ((filter.PageIdx - 1) * filter.PageSize > allCnt)
            {
                filter.PageIdx = 1;
            }

            reportResult.AllBlocks = sortedResult.ToList();
            reportResult.Filter = filter;

            return reportResult;
        }
    }
}