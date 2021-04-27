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
    public class ReportNormativeTechnicsFilter
    {
        public string MilitaryDepartmentIds { get; set; }
        public bool IsOwnershipAddress { get; set; }
        public string Region { get; set; }
        public string Municipality { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string PostCode { get; set; }
        public string Address { get; set; }
        public int PageIdx { get; set; }
        public int PageSize { get; set; }
    }

    public class ReportNormativeTechnicsResult
    {
        public ReportNormativeTechnicsFilter Filter { get; set; }

        public int MaxPage
        {
            get
            {
                return Filter.PageSize == 0 ? 1 : Rows.Count / Filter.PageSize + 1;
            }
        }

        public List<ReportTableHeaderCell> Header1Cells { get; set; }
        public List<ReportTableHeaderCell> Header2Cells { get; set; }
        public List<ReportTableHeaderCell> Header3Cells { get; set; }
        public ArrayList Rows { get; set; }

        public ArrayList PagedRows
        {
            get
            {
                ArrayList pagedRows = new ArrayList();

                for(int i = (Filter.PageIdx - 1) * Filter.PageSize + 1; i < (Filter.PageIdx * Filter.PageSize) + 1 && i < Rows.Count; i++)
                    pagedRows.Add(Rows[i - 1]);

                return pagedRows;
            }
        }
    }

    public static class ReportNormativeTechnicsUtil
    {
        public static ReportNormativeTechnicsResult GetReportNormativeTechnics(ReportNormativeTechnicsFilter filter, User currentUser)
        {
            ReportNormativeTechnicsResult reportResult = new ReportNormativeTechnicsResult();
            reportResult.Filter = filter;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string whereClause = "";
                
                if (!String.IsNullOrEmpty(filter.MilitaryDepartmentIds))
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " d.SourceMilDepartmentID IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryDepartmentIds) + ") ";
                }

                if (filter.IsOwnershipAddress)
                {

                    if (!string.IsNullOrEmpty(filter.Region))
                    {
                        whereClause += (whereClause == "" ? "" : " AND ") +
                                 @" com.CityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBL IN ( " + filter.Region + ") ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.Municipality))
                    {
                        whereClause += (whereClause == "" ? "" : " AND ") +
                                 @" com.CityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBS IN ( " + filter.Municipality + ") ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.City))
                    {
                        whereClause += (whereClause == "" ? "" : " AND ") +
                                 @" com.CityID IN ( " + filter.City + ") ";
                    }

                    if (!string.IsNullOrEmpty(filter.District))
                    {
                        whereClause += (whereClause == "" ? "" : " AND ") +
                                 @" com.DistrictID IN ( " + filter.District + ") ";
                    }

                    if (!string.IsNullOrEmpty(filter.Address))
                    {
                        whereClause += (whereClause == "" ? "" : " AND ") +
                                 " UPPER(com.Address) LIKE UPPER('%" + filter.Address.Replace("'", "''") + "%') ";
                    }

                    if (!string.IsNullOrEmpty(filter.PostCode))
                    {
                        whereClause += (whereClause == "" ? "" : " AND ") +
                                 @" com.PostCode LIKE '%" + filter.PostCode.Replace("'", "''") + "%' ";
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(filter.Region))
                    {
                        whereClause += (whereClause == "" ? "" : " AND ") +
                                 @" c.ResidenceCityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBL IN ( " + filter.Region + ") ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.Municipality))
                    {
                        whereClause += (whereClause == "" ? "" : " AND ") +
                                 @" c.ResidenceCityID IN ( SELECT KOD_NMA FROM UKAZ_OWNER.KL_NMA WHERE KOD_OBS IN ( " + filter.Municipality + ") ) ";
                    }

                    if (!string.IsNullOrEmpty(filter.City))
                    {
                        whereClause += (whereClause == "" ? "" : " AND ") +
                                 @" c.ResidenceCityID IN ( " + filter.City + ") ";
                    }

                    if (!string.IsNullOrEmpty(filter.District))
                    {
                        whereClause += (whereClause == "" ? "" : " AND ") +
                                 @" c.ResidenceDistrictID IN ( " + filter.District + ") ";
                    }

                    if (!string.IsNullOrEmpty(filter.Address))
                    {
                        whereClause += (whereClause == "" ? "" : " AND ") +
                                 " UPPER(c.ResidenceAddress) LIKE UPPER('%" + filter.Address.Replace("'", "''") + "%') ";
                    }

                    if (!string.IsNullOrEmpty(filter.PostCode))
                    {
                        whereClause += (whereClause == "" ? "" : " AND ") +
                                 @" c.ResidencePostCode LIKE '%" + filter.PostCode.Replace("'", "''") + "%' ";
                    }
                }

                whereClause += (whereClause == "" ? "" : " AND ") +
                         @" (d.SourceMilDepartmentID IS NULL OR d.SourceMilDepartmentID IN ( " + currentUser.MilitaryDepartmentIDs + ")) ";

                //whereClause = (whereClause == "" ? "" : " WHERE ") + whereClause;

                string SQL = @"
SELECT a.OrderCode, a.NormativeCode, a.NormativeName, a.NormativeLevel, 
       'Код' as ColumnHeader1,
       1 as ColumnHeader1ColSpan,
       3 as ColumnHeader1RowSpan,
       '' as ColumnHeader2,
       0 as ColumnHeader2ColSpan,
       0 as ColumnHeader2RowSpan,
       '' as ColumnLabel,
       1 as ColumnOrder,
       a.NormativeCode as ColumnValue
FROM PMIS_RES.ViewReportNormativeTechnics a

UNION ALL

SELECT a.OrderCode, a.NormativeCode, a.NormativeName, a.NormativeLevel, 
       'Вид и тип МПС' as ColumnHeader1,
       1 as ColumnHeader1ColSpan,
       3 as ColumnHeader1RowSpan,
       '' as ColumnHeader2,
       0 as ColumnHeader2ColSpan,
       0 as ColumnHeader2RowSpan,
       '' as ColumnLabel, 
       2 as ColumnOrder,
       a.NormativeName as ColumnValue
FROM PMIS_RES.ViewReportNormativeTechnics a

UNION ALL

SELECT a.OrderCode, a.NormativeCode, a.NormativeName, a.NormativeLevel, 
       'Техника-резерв' as ColumnHeader1,
       1 as ColumnHeader1ColSpan,
       3 as ColumnHeader1RowSpan,
       '' as ColumnHeader2,
       0 as ColumnHeader2ColSpan,
       0 as ColumnHeader2RowSpan,
       '' as ColumnLabel, 
       3 as ColumnOrder,
       TO_CHAR(SUM(CASE WHEN c.TechnicsID IS NOT NULL
                        THEN c.ItemsCount
                        ELSE 0
                   END
                   )) as ColumnValue
FROM PMIS_RES.ViewReportNormativeTechnics a
LEFT OUTER JOIN PMIS_RES.NormativeTechnics b ON (a.NormativeTechnicsID <> 0 AND a.NormativeTechnicsID = b.NormativeTechnicsID) OR
                                                (a.NormativeTechnicsID = 0 AND
                                                 (NVL(a.NormativeLevel1, '_') = NVL(b.NormativeLevel1, '_') OR a.normativelevel < 1) AND
                                                 (NVL(a.NormativeLevel2, '_') = NVL(b.NormativeLevel2, '_') OR a.NormativeLevel < 2) AND
                                                 (NVL(a.NormativeLevel3, '_') = NVL(b.NormativeLevel3, '_') OR a.NormativeLevel < 3) AND
                                                 (NVL(a.NormativeLevel4, '_') = NVL(b.NormativeLevel4, '_') OR a.NormativeLevel < 4) AND
                                                 (NVL(a.NormativeLevel5, '_') = NVL(b.NormativeLevel5, '_') OR a.NormativeLevel < 5)
                                                )
LEFT OUTER JOIN 
  (SELECT c.TechnicsID, c.NormativeTechnicsID, c.ItemsCount
   FROM PMIS_RES.Technics c 
   INNER JOIN PMIS_RES.TechnicsMilRepStatus d ON c.TechnicsID = d.TechnicsID AND d.IsCurrent = 1
   INNER JOIN PMIS_RES.TechMilitaryReportStatuses e ON d.TechMilitaryReportStatusID = e.TechMilitaryReportStatusID
   LEFT OUTER JOIN PMIS_ADM.Companies com ON c.OwnershipCompanyID = com.CompanyID
   WHERE e.TechMilitaryReportStatusKey = 'VOLUNTARY_RESERVE' AND " + whereClause + @"
  ) c ON b.NormativeTechnicsID = c.NormativeTechnicsID
GROUP BY a.OrderCode, a.NormativeCode, a.NormativeName, a.NormativeLevel

UNION ALL

SELECT a.OrderCode, a.NormativeCode, a.NormativeName, a.NormativeLevel, 
       'Tехника-запас' as ColumnHeader1,
       6 as ColumnHeader1ColSpan,
       1 as ColumnHeader1RowSpan,
       'Водят се на отчет' as ColumnHeader2,
       1 as ColumnHeader2ColSpan,
       2 as ColumnHeader2RowSpan,
       '' as ColumnLabel, 
       4 as ColumnOrder,
       TO_CHAR(SUM(CASE WHEN c.TechnicsID IS NOT NULL
                        THEN c.ItemsCount
                        ELSE 0
                   END
                   )) as ColumnValue
FROM PMIS_RES.ViewReportNormativeTechnics a
LEFT OUTER JOIN PMIS_RES.NormativeTechnics b ON (a.NormativeTechnicsID <> 0 AND a.NormativeTechnicsID = b.NormativeTechnicsID) OR
                                                (a.NormativeTechnicsID = 0 AND
                                                 (NVL(a.NormativeLevel1, '_') = NVL(b.NormativeLevel1, '_') OR a.normativelevel < 1) AND
                                                 (NVL(a.NormativeLevel2, '_') = NVL(b.NormativeLevel2, '_') OR a.NormativeLevel < 2) AND
                                                 (NVL(a.NormativeLevel3, '_') = NVL(b.NormativeLevel3, '_') OR a.NormativeLevel < 3) AND
                                                 (NVL(a.NormativeLevel4, '_') = NVL(b.NormativeLevel4, '_') OR a.NormativeLevel < 4) AND
                                                 (NVL(a.NormativeLevel5, '_') = NVL(b.NormativeLevel5, '_') OR a.NormativeLevel < 5)
                                                )
LEFT OUTER JOIN 
  (SELECT c.TechnicsID, c.NormativeTechnicsID, c.ItemsCount
   FROM PMIS_RES.Technics c 
   INNER JOIN PMIS_RES.TechnicsMilRepStatus d ON c.TechnicsID = d.TechnicsID AND d.IsCurrent = 1
   INNER JOIN PMIS_RES.TechMilitaryReportStatuses e ON d.TechMilitaryReportStatusID = e.TechMilitaryReportStatusID
   LEFT OUTER JOIN PMIS_ADM.Companies com ON c.OwnershipCompanyID = com.CompanyID
   WHERE e.TechMilitaryReportStatusKey <> 'VOLUNTARY_RESERVE' AND e.TechMilitaryReportStatusKey <> 'REMOVED' AND " + whereClause + @"
  ) c ON b.NormativeTechnicsID = c.NormativeTechnicsID
GROUP BY a.OrderCode, a.NormativeCode, a.NormativeName, a.NormativeLevel

UNION ALL

SELECT a.OrderCode, a.NormativeCode, a.NormativeName, a.NormativeLevel, 
       '' as ColumnHeader1,
       0 as ColumnHeader1ColSpan,
       0 as ColumnHeader1RowSpan,
       'Получили МН' as ColumnHeader2,
       1 as ColumnHeader2ColSpan,
       2 as ColumnHeader2RowSpan,
       '' as ColumnLabel, 
       5 as ColumnOrder,
       TO_CHAR(SUM(CASE WHEN c.TechnicsID IS NOT NULL
                        THEN c.ItemsCount
                        ELSE 0
                   END
                   )) as ColumnValue
FROM PMIS_RES.ViewReportNormativeTechnics a
LEFT OUTER JOIN PMIS_RES.NormativeTechnics b ON (a.NormativeTechnicsID <> 0 AND a.NormativeTechnicsID = b.NormativeTechnicsID) OR
                                                (a.NormativeTechnicsID = 0 AND
                                                 (NVL(a.NormativeLevel1, '_') = NVL(b.NormativeLevel1, '_') OR a.normativelevel < 1) AND
                                                 (NVL(a.NormativeLevel2, '_') = NVL(b.NormativeLevel2, '_') OR a.NormativeLevel < 2) AND
                                                 (NVL(a.NormativeLevel3, '_') = NVL(b.NormativeLevel3, '_') OR a.NormativeLevel < 3) AND
                                                 (NVL(a.NormativeLevel4, '_') = NVL(b.NormativeLevel4, '_') OR a.NormativeLevel < 4) AND
                                                 (NVL(a.NormativeLevel5, '_') = NVL(b.NormativeLevel5, '_') OR a.NormativeLevel < 5)
                                                )
LEFT OUTER JOIN 
  (SELECT c.TechnicsID, c.NormativeTechnicsID, c.ItemsCount
   FROM PMIS_RES.Technics c 
   INNER JOIN PMIS_RES.TechnicsMilRepStatus d ON c.TechnicsID = d.TechnicsID AND d.IsCurrent = 1
   INNER JOIN PMIS_RES.TechMilitaryReportStatuses e ON d.TechMilitaryReportStatusID = e.TechMilitaryReportStatusID
   INNER JOIN PMIS_RES.FulfilTechnicsRequest f ON c.TechnicsID = f.TechnicsID
   LEFT OUTER JOIN PMIS_ADM.Companies com ON c.OwnershipCompanyID = com.CompanyID
   WHERE e.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND f.TechnicReadinessID = 1 AND " + whereClause + @"
  ) c ON b.NormativeTechnicsID = c.NormativeTechnicsID
GROUP BY a.OrderCode, a.NormativeCode, a.NormativeName, a.NormativeLevel

UNION ALL

SELECT a.OrderCode, a.NormativeCode, a.NormativeName, a.NormativeLevel, 
       '' as ColumnHeader1,
       0 as ColumnHeader1ColSpan,
       0 as ColumnHeader1RowSpan,
       'Резерв с МН /ед.т./' as ColumnHeader2,
       1 as ColumnHeader2ColSpan,
       2 as ColumnHeader2RowSpan,
       '' as ColumnLabel, 
       6 as ColumnOrder,
       TO_CHAR(SUM(CASE WHEN c.TechnicsID IS NOT NULL
                        THEN c.ItemsCount
                        ELSE 0
                   END
                   )) as ColumnValue
FROM PMIS_RES.ViewReportNormativeTechnics a
LEFT OUTER JOIN PMIS_RES.NormativeTechnics b ON (a.NormativeTechnicsID <> 0 AND a.NormativeTechnicsID = b.NormativeTechnicsID) OR
                                                (a.NormativeTechnicsID = 0 AND
                                                 (NVL(a.NormativeLevel1, '_') = NVL(b.NormativeLevel1, '_') OR a.normativelevel < 1) AND
                                                 (NVL(a.NormativeLevel2, '_') = NVL(b.NormativeLevel2, '_') OR a.NormativeLevel < 2) AND
                                                 (NVL(a.NormativeLevel3, '_') = NVL(b.NormativeLevel3, '_') OR a.NormativeLevel < 3) AND
                                                 (NVL(a.NormativeLevel4, '_') = NVL(b.NormativeLevel4, '_') OR a.NormativeLevel < 4) AND
                                                 (NVL(a.NormativeLevel5, '_') = NVL(b.NormativeLevel5, '_') OR a.NormativeLevel < 5)
                                                )
LEFT OUTER JOIN 
  (SELECT c.TechnicsID, c.NormativeTechnicsID, c.ItemsCount
   FROM PMIS_RES.Technics c 
   INNER JOIN PMIS_RES.TechnicsMilRepStatus d ON c.TechnicsID = d.TechnicsID AND d.IsCurrent = 1
   INNER JOIN PMIS_RES.TechMilitaryReportStatuses e ON d.TechMilitaryReportStatusID = e.TechMilitaryReportStatusID
   INNER JOIN PMIS_RES.FulfilTechnicsRequest f ON c.TechnicsID = f.TechnicsID
   LEFT OUTER JOIN PMIS_ADM.Companies com ON c.OwnershipCompanyID = com.CompanyID
   WHERE e.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND f.TechnicReadinessID = 2 AND " + whereClause + @"
  ) c ON b.NormativeTechnicsID = c.NormativeTechnicsID
GROUP BY a.OrderCode, a.NormativeCode, a.NormativeName, a.NormativeLevel

UNION ALL

SELECT a.OrderCode, a.NormativeCode, a.NormativeName, a.NormativeLevel, 
       '' as ColumnHeader1,
       0 as ColumnHeader1ColSpan,
       0 as ColumnHeader1RowSpan,
       'Свободни без МН' as ColumnHeader2,
       1 as ColumnHeader2ColSpan,
       2 as ColumnHeader2RowSpan,
       '' as ColumnLabel, 
       7 as ColumnOrder,
       TO_CHAR(SUM(CASE WHEN c.TechnicsID IS NOT NULL
                        THEN c.ItemsCount
                        ELSE 0
                   END
                   )) as ColumnValue
FROM PMIS_RES.ViewReportNormativeTechnics a
LEFT OUTER JOIN PMIS_RES.NormativeTechnics b ON (a.NormativeTechnicsID <> 0 AND a.NormativeTechnicsID = b.NormativeTechnicsID) OR
                                                (a.NormativeTechnicsID = 0 AND
                                                 (NVL(a.NormativeLevel1, '_') = NVL(b.NormativeLevel1, '_') OR a.normativelevel < 1) AND
                                                 (NVL(a.NormativeLevel2, '_') = NVL(b.NormativeLevel2, '_') OR a.NormativeLevel < 2) AND
                                                 (NVL(a.NormativeLevel3, '_') = NVL(b.NormativeLevel3, '_') OR a.NormativeLevel < 3) AND
                                                 (NVL(a.NormativeLevel4, '_') = NVL(b.NormativeLevel4, '_') OR a.NormativeLevel < 4) AND
                                                 (NVL(a.NormativeLevel5, '_') = NVL(b.NormativeLevel5, '_') OR a.NormativeLevel < 5)
                                                )
LEFT OUTER JOIN 
  (SELECT c.TechnicsID, c.NormativeTechnicsID, c.ItemsCount
   FROM PMIS_RES.Technics c 
   INNER JOIN PMIS_RES.TechnicsMilRepStatus d ON c.TechnicsID = d.TechnicsID AND d.IsCurrent = 1
   INNER JOIN PMIS_RES.TechMilitaryReportStatuses e ON d.TechMilitaryReportStatusID = e.TechMilitaryReportStatusID
   LEFT OUTER JOIN PMIS_ADM.Companies com ON c.OwnershipCompanyID = com.CompanyID
   WHERE e.TechMilitaryReportStatusKey = 'FREE' AND " + whereClause + @"
  ) c ON b.NormativeTechnicsID = c.NormativeTechnicsID
GROUP BY a.OrderCode, a.NormativeCode, a.NormativeName, a.NormativeLevel

UNION ALL

SELECT a.OrderCode, a.NormativeCode, a.NormativeName, a.NormativeLevel, 
       '' as ColumnHeader1,
       0 as ColumnHeader1ColSpan,
       0 as ColumnHeader1RowSpan,
       'Отсрочени от повикване' as ColumnHeader2,
       1 as ColumnHeader2ColSpan,
       2 as ColumnHeader2RowSpan,
       '' as ColumnLabel, 
       8 as ColumnOrder,
       TO_CHAR(SUM(CASE WHEN c.TechnicsID IS NOT NULL
                        THEN c.ItemsCount
                        ELSE 0
                   END
                   )) as ColumnValue
FROM PMIS_RES.ViewReportNormativeTechnics a
LEFT OUTER JOIN PMIS_RES.NormativeTechnics b ON (a.NormativeTechnicsID <> 0 AND a.NormativeTechnicsID = b.NormativeTechnicsID) OR
                                                (a.NormativeTechnicsID = 0 AND
                                                 (NVL(a.NormativeLevel1, '_') = NVL(b.NormativeLevel1, '_') OR a.normativelevel < 1) AND
                                                 (NVL(a.NormativeLevel2, '_') = NVL(b.NormativeLevel2, '_') OR a.NormativeLevel < 2) AND
                                                 (NVL(a.NormativeLevel3, '_') = NVL(b.NormativeLevel3, '_') OR a.NormativeLevel < 3) AND
                                                 (NVL(a.NormativeLevel4, '_') = NVL(b.NormativeLevel4, '_') OR a.NormativeLevel < 4) AND
                                                 (NVL(a.NormativeLevel5, '_') = NVL(b.NormativeLevel5, '_') OR a.NormativeLevel < 5)
                                                )
LEFT OUTER JOIN 
  (SELECT c.TechnicsID, c.NormativeTechnicsID, c.ItemsCount
   FROM PMIS_RES.Technics c 
   INNER JOIN PMIS_RES.TechnicsMilRepStatus d ON c.TechnicsID = d.TechnicsID AND d.IsCurrent = 1
   INNER JOIN PMIS_RES.TechMilitaryReportStatuses e ON d.TechMilitaryReportStatusID = e.TechMilitaryReportStatusID
   LEFT OUTER JOIN PMIS_ADM.Companies com ON c.OwnershipCompanyID = com.CompanyID
   WHERE e.TechMilitaryReportStatusKey = 'POSTPONED' AND " + whereClause + @"
  ) c ON b.NormativeTechnicsID = c.NormativeTechnicsID
GROUP BY a.OrderCode, a.NormativeCode, a.NormativeName, a.NormativeLevel

UNION ALL

SELECT a.OrderCode, a.NormativeCode, a.NormativeName, a.NormativeLevel, 
       '' as ColumnHeader1,
       0 as ColumnHeader1ColSpan,
       0 as ColumnHeader1RowSpan,
       'Временно отписани' as ColumnHeader2,
       1 as ColumnHeader2ColSpan,
       2 as ColumnHeader2RowSpan,
       '' as ColumnLabel, 
       9 as ColumnOrder,
       TO_CHAR(SUM(CASE WHEN c.TechnicsID IS NOT NULL
                        THEN c.ItemsCount
                        ELSE 0
                   END
                   )) as ColumnValue
FROM PMIS_RES.ViewReportNormativeTechnics a
LEFT OUTER JOIN PMIS_RES.NormativeTechnics b ON (a.NormativeTechnicsID <> 0 AND a.NormativeTechnicsID = b.NormativeTechnicsID) OR
                                                (a.NormativeTechnicsID = 0 AND
                                                 (NVL(a.NormativeLevel1, '_') = NVL(b.NormativeLevel1, '_') OR a.normativelevel < 1) AND
                                                 (NVL(a.NormativeLevel2, '_') = NVL(b.NormativeLevel2, '_') OR a.NormativeLevel < 2) AND
                                                 (NVL(a.NormativeLevel3, '_') = NVL(b.NormativeLevel3, '_') OR a.NormativeLevel < 3) AND
                                                 (NVL(a.NormativeLevel4, '_') = NVL(b.NormativeLevel4, '_') OR a.NormativeLevel < 4) AND
                                                 (NVL(a.NormativeLevel5, '_') = NVL(b.NormativeLevel5, '_') OR a.NormativeLevel < 5)
                                                )
LEFT OUTER JOIN 
  (SELECT c.TechnicsID, c.NormativeTechnicsID, c.ItemsCount
   FROM PMIS_RES.Technics c 
   INNER JOIN PMIS_RES.TechnicsMilRepStatus d ON c.TechnicsID = d.TechnicsID AND d.IsCurrent = 1
   INNER JOIN PMIS_RES.TechMilitaryReportStatuses e ON d.TechMilitaryReportStatusID = e.TechMilitaryReportStatusID
   LEFT OUTER JOIN PMIS_ADM.Companies com ON c.OwnershipCompanyID = com.CompanyID
   WHERE e.TechMilitaryReportStatusKey = 'TEMPORARY_REMOVED' AND " + whereClause + @"
  ) c ON b.NormativeTechnicsID = c.NormativeTechnicsID
GROUP BY a.OrderCode, a.NormativeCode, a.NormativeName, a.NormativeLevel

UNION ALL

SELECT a.OrderCode, a.NormativeCode, a.NormativeName, a.NormativeLevel, 
       'Всичко техника' as ColumnHeader1,
       1 as ColumnHeader1ColSpan,
       3 as ColumnHeader1RowSpan,
       '' as ColumnHeader2,
       0 as ColumnHeader2ColSpan,
       0 as ColumnHeader2RowSpan,
       '' as ColumnLabel, 
       10 as ColumnOrder,
       TO_CHAR(SUM(CASE WHEN c.TechnicsID IS NOT NULL
                        THEN c.ItemsCount
                        ELSE 0
                   END
                   )) as ColumnValue
FROM PMIS_RES.ViewReportNormativeTechnics a
LEFT OUTER JOIN PMIS_RES.NormativeTechnics b ON (a.NormativeTechnicsID <> 0 AND a.NormativeTechnicsID = b.NormativeTechnicsID) OR
                                                (a.NormativeTechnicsID = 0 AND
                                                 (NVL(a.NormativeLevel1, '_') = NVL(b.NormativeLevel1, '_') OR a.normativelevel < 1) AND
                                                 (NVL(a.NormativeLevel2, '_') = NVL(b.NormativeLevel2, '_') OR a.NormativeLevel < 2) AND
                                                 (NVL(a.NormativeLevel3, '_') = NVL(b.NormativeLevel3, '_') OR a.NormativeLevel < 3) AND
                                                 (NVL(a.NormativeLevel4, '_') = NVL(b.NormativeLevel4, '_') OR a.NormativeLevel < 4) AND
                                                 (NVL(a.NormativeLevel5, '_') = NVL(b.NormativeLevel5, '_') OR a.NormativeLevel < 5)
                                                )
LEFT OUTER JOIN 
  (SELECT c.TechnicsID, c.NormativeTechnicsID, c.ItemsCount
   FROM PMIS_RES.Technics c 
   INNER JOIN PMIS_RES.TechnicsMilRepStatus d ON c.TechnicsID = d.TechnicsID AND d.IsCurrent = 1
   INNER JOIN PMIS_RES.TechMilitaryReportStatuses e ON d.TechMilitaryReportStatusID = e.TechMilitaryReportStatusID
   LEFT OUTER JOIN PMIS_ADM.Companies com ON c.OwnershipCompanyID = com.CompanyID
   WHERE e.TechMilitaryReportStatusKey <> 'REMOVED' AND " + whereClause + @"
  ) c ON b.NormativeTechnicsID = c.NormativeTechnicsID
GROUP BY a.OrderCode, a.NormativeCode, a.NormativeName, a.NormativeLevel

UNION ALL

SELECT a.OrderCode, a.NormativeCode, a.NormativeName, a.NormativeLevel, 
       'Собственост' as ColumnHeader1,
       (SELECT COUNT(*) FROM PMIS_ADM.OwnershipTypes) * 2 as ColumnHeader1ColSpan,
       1 as ColumnHeader1RowSpan,
       ot.OwnershipTypeName as ColumnHeader2,
       2 as ColumnHeader2ColSpan,
       1 as ColumnHeader2RowSpan,
       st.SubTypeLabel as ColumnLabel,
       10 +
       ((DENSE_RANK() OVER (ORDER BY CASE WHEN ot.OwnershipTypeKey LIKE '%COMPANY%' THEN 1 ELSE 2 END, ot.OwnershipTypeName)) - 1) * 2 + st.SubTypeID as ColumnOrder,
       TO_CHAR(SUM(CASE WHEN st.SubTypeID = 2 AND e.TechnicsID IS NOT NULL AND d1.TechMilitaryReportStatusKey = 'MOBILE_APPOINTMENT' AND e.TechnicReadinessID = 1
                        THEN c.ItemsCount
                        ELSE CASE WHEN st.SubTypeID = 1 AND c.TechnicsID IS NOT NULL
                                  THEN c.ItemsCount
                                  ELSE 0
                             END
                   END
                   )) as ColumnValue
FROM PMIS_RES.ViewReportNormativeTechnics a
CROSS JOIN PMIS_ADM.OwnershipTypes ot
CROSS JOIN (SELECT 1 as SubTypeID,
                   'Всичко' as SubTypeLabel
            FROM Dual

            UNION ALL

            SELECT 2 as SubTypeID,
                   'С МН' as SubTypeLabel
            FROM Dual
           ) st
LEFT OUTER JOIN PMIS_RES.NormativeTechnics b ON (a.NormativeTechnicsID <> 0 AND a.NormativeTechnicsID = b.NormativeTechnicsID) OR
                                                (a.NormativeTechnicsID = 0 AND
                                                 (NVL(a.NormativeLevel1, '_') = NVL(b.NormativeLevel1, '_') OR a.normativelevel < 1) AND
                                                 (NVL(a.NormativeLevel2, '_') = NVL(b.NormativeLevel2, '_') OR a.NormativeLevel < 2) AND
                                                 (NVL(a.NormativeLevel3, '_') = NVL(b.NormativeLevel3, '_') OR a.NormativeLevel < 3) AND
                                                 (NVL(a.NormativeLevel4, '_') = NVL(b.NormativeLevel4, '_') OR a.NormativeLevel < 4) AND
                                                 (NVL(a.NormativeLevel5, '_') = NVL(b.NormativeLevel5, '_') OR a.NormativeLevel < 5)
                                                )
LEFT OUTER JOIN 
  (SELECT c.TechnicsID, c.NormativeTechnicsID, com.OwnershipTypeID,
          d.TechMilitaryReportStatusID, c.ItemsCount
   FROM PMIS_RES.Technics c 
   LEFT OUTER JOIN PMIS_ADM.Companies com ON c.OwnershipCompanyID = com.CompanyID
   LEFT OUTER JOIN PMIS_RES.TechnicsMilRepStatus d ON c.TechnicsID = d.TechnicsID AND d.IsCurrent = 1
   WHERE 1=1 AND " + whereClause + @"
  ) c ON b.NormativeTechnicsID = c.NormativeTechnicsID AND c.OwnershipTypeID = ot.OwnershipTypeID
LEFT OUTER JOIN PMIS_RES.TechMilitaryReportStatuses d1 ON c.TechMilitaryReportStatusID = d1.TechMilitaryReportStatusID
LEFT OUTER JOIN PMIS_RES.FulfilTechnicsRequest e ON c.TechnicsID = e.TechnicsID
GROUP BY a.OrderCode, a.NormativeCode, a.NormativeName, a.NormativeLevel,
         ot.OwnershipTypeID, ot.OwnershipTypeName, ot.OwnershipTypeKey, st.SubTypeID, st.SubTypeLabel

ORDER BY OrderCode, NormativeCode NULLS FIRST, NormativeLevel,
         ColumnOrder
";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                bool isFirstRow = true;
                List<ReportTableHeaderCell> header1CellsList = new List<ReportTableHeaderCell>();
                List<ReportTableHeaderCell> header2CellsList = new List<ReportTableHeaderCell>();
                List<ReportTableHeaderCell> header3CellsList = new List<ReportTableHeaderCell>();

                ArrayList rowsList = new ArrayList();

                ArrayList tmpRow = new ArrayList();
                string oldNormativeName = "";

                int columnIndex = 0;

                int header1ColSpan = 0;
                int header2ColSpan = 0;

                while (dr.Read())
                {
                    columnIndex++;

                    string normativeCode = dr["NormativeCode"].ToString();
                    string normativeName = dr["NormativeName"].ToString();
                    string columnLabel = dr["ColumnLabel"].ToString();
                    string columnValue = dr["ColumnValue"].ToString();
                    string columnHeader1 = dr["ColumnHeader1"].ToString();
                    int columnHeader1ColSpan = DBCommon.GetInt(dr["ColumnHeader1ColSpan"]);
                    int columnHeader1RowSpan = DBCommon.GetInt(dr["ColumnHeader1RowSpan"]);
                    string columnHeader2 = dr["ColumnHeader2"].ToString();
                    int columnHeader2ColSpan = DBCommon.GetInt(dr["ColumnHeader2ColSpan"]);
                    int columnHeader2RowSpan = DBCommon.GetInt(dr["ColumnHeader2RowSpan"]);

                    if (oldNormativeName != "" && oldNormativeName != normativeName)
                    { 
                        //new row
                        isFirstRow = false;
                        columnIndex = 1;

                        rowsList.Add((string[])tmpRow.ToArray(typeof(string)));
                        tmpRow.Clear();
                    }

                    oldNormativeName = normativeName;                   

                    if (isFirstRow)
                    {
                        if (columnHeader1RowSpan != 0)
                        {
                            if (columnHeader1ColSpan > 1 && header1ColSpan > 0 && header1ColSpan < columnHeader1ColSpan)
                            {
                                header1ColSpan++;
                            }
                            else
                            {
                                if (columnHeader1ColSpan > 1)
                                {
                                    header1ColSpan = 1;
                                }
                                else
                                {
                                    header1ColSpan = 0;
                                }

                                ReportTableHeaderCell header1 = new ReportTableHeaderCell();
                                header1.Label = columnHeader1;
                                header1.ColSpan = columnHeader1ColSpan;
                                header1.RowSpan = columnHeader1RowSpan;

                                header1CellsList.Add(header1);
                            }
                        }

                        if (columnHeader2RowSpan != 0)
                        {
                            if (columnHeader2ColSpan > 1 && header2ColSpan > 0 && header2ColSpan < columnHeader2ColSpan)
                            {
                                header2ColSpan++;
                            }
                            else
                            {
                                if (columnHeader2ColSpan > 1)
                                {
                                    header2ColSpan = 1;
                                }
                                else
                                {
                                    header2ColSpan = 0;
                                }

                                ReportTableHeaderCell header2 = new ReportTableHeaderCell();
                                header2.Label = columnHeader2;
                                header2.ColSpan = columnHeader2ColSpan;
                                header2.RowSpan = columnHeader2RowSpan;

                                header2CellsList.Add(header2);
                            }
                        }

                        if (columnLabel != "")
                        {
                            ReportTableHeaderCell header3 = new ReportTableHeaderCell();
                            header3.Label = columnLabel;
                            header3.ColSpan = 1;
                            header3.RowSpan = 1;

                            header3CellsList.Add(header3);
                        }
                    }

                    tmpRow.Add(columnValue);
                }

                dr.Close();

                if (tmpRow.Count > 0)
                {
                    rowsList.Add((string[])tmpRow.ToArray(typeof(string)));
                }
                
                tmpRow.Clear();

                reportResult.Header1Cells = header1CellsList;
                reportResult.Header2Cells = header2CellsList;
                reportResult.Header3Cells = header3CellsList;
                
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