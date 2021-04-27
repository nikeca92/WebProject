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
    public class ReportStaffPositionsListFilter
    {
        public int MilitaryUnitId { get; set; }
        public int MilitaryCommandId { get; set; }
        public string SubMilitaryCommandSuffix { get; set; }
        public int PageIdx { get; set; }
        public int PageSize { get; set; }
    }

    public class ReportStaffPositionsListBlock
    {
        public int RowIndex { get; set; }
        public string PositionName { get; set; }
        public string MilitaryRankName { get; set; }
        public string StaffMRS { get; set; }
        public string PositionCode { get; set; }
        public string PersonFullNameAndRank { get; set; }
        public string PersonMRS { get; set; }
        public string PersonIdentityNumber { get; set; }
        public string PersonPermAddress { get; set; }
        public string PersonEducation { get; set; }
        public string MilitaryDepartment { get; set; }
        public string StaffWeapon { get; set; }
        public DateTime? AppointmentDate { get; set; }
    }

    public class ReportStaffPositionsListResult
    {
        public ReportStaffPositionsListFilter Filter { get; set; }

        public int MaxPage
        {
            get
            {
                return Filter.PageSize == 0 ? 1 : Result.Count / Filter.PageSize + 1;
            }
        }

        public List<ReportStaffPositionsListBlock> Result { get; set; }

        public List<ReportStaffPositionsListBlock> PagedResult
        {
            get
            {
                List<ReportStaffPositionsListBlock> pagedResult = new List<ReportStaffPositionsListBlock>();

                for (int i = (Filter.PageIdx - 1) * Filter.PageSize + 1; i < (Filter.PageIdx * Filter.PageSize) + 1 && i <= Result.Count; i++)
                {
                    pagedResult.Add(Result[i - 1]);
                }

                return pagedResult;
            }
        }
    }

    public static class ReportStaffPositionsListUtil
    {
        public static ReportStaffPositionsListResult GetReportStaffPositionsList(ReportStaffPositionsListFilter filter, User currentUser)
        {
            ReportStaffPositionsListResult reportResult = new ReportStaffPositionsListResult();
            reportResult.Filter = filter;

            reportResult.Result = ReportStaffPositionsListUtil.GetResult(filter, currentUser);

            return reportResult;
        }

        public static List<ReportStaffPositionsListBlock> GetResult(ReportStaffPositionsListFilter filter, User currentUser)
        {
            List<ReportStaffPositionsListBlock> reportResult = new List<ReportStaffPositionsListBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string whereClause = "";
                
                whereClause += (whereClause == "" ? "" : " AND ") +
                         @" a.MilitaryCommandID = " + filter.MilitaryCommandId.ToString() + " ";

                if (!String.IsNullOrEmpty(filter.SubMilitaryCommandSuffix))
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " a.MilitaryCommandSuffix = '" + filter.SubMilitaryCommandSuffix + "' ";
                }
                
                whereClause = (whereClause == "" ? "" : " WHERE ") + whereClause;

                string SQL = @"
                               SELECT b.Position as PositionName,
                                      b1.ZVA_IMEES as MilitaryRankName,
                                      b3.MilReportingSpecialityCode || ' ' || b3.MilReportingSpecialityName as StaffMRS,
                                      '' as PositionCode,
                                      LOWER(f.ZVA_IMEES) || ' ' || e.IME || ' ' || e.FAM as PersonFullNameAndRank,
                                      e2.MilReportingSpecialityCode || ' ' || e2.MilReportingSpecialityName as PersonMRS,
                                      e.EGN as PersonIdentityNumber,
                                      CASE WHEN p3.IME_OBL IS NOT NULL 
                                           THEN 'обл. ' || p3.IME_OBL 
                                           ELSE ''
                                      END || 
                                      CASE WHEN p4.IME_OBS IS NOT NULL 
                                           THEN ', общ. ' || p4.IME_OBS
                                           ELSE ''
                                      END || 
                                      CASE WHEN p.IME_NMA IS NOT NULL 
                                           THEN ', ' || p2.IME_S || ' ' || p.IME_NMA
                                           ELSE ''
                                      END ||
                                      CASE WHEN e.Adres IS NOT NULL 
                                           THEN ', ' || e.Adres
                                           ELSE ''
                                      END ||
                                      CASE WHEN NVL(NVL(e.PermSecondPostCode, p.PK), '0') <> '0'
                                           THEN ', пк ' || NVL(e.PermSecondPostCode, p.PK)
                                           ELSE ''
                                      END as PersonPermAddress,
                                      CASE WHEN g.MilitaryRankCategory = 2 /*KEY_OFFICER*/
                                           THEN CASE WHEN e.OBR_VA = 'Y' 
                                                     THEN 'ВА (ГЩФ)' 
                                                     ELSE CASE WHEN e.OBR_VU = 'Y' 
                                                               THEN 'Военно училище'
                                                               ELSE CASE WHEN e.OBR_SHZO = 'Y' 
                                                                         THEN 'Школи'
                                                                         ELSE 'Без военно образование' 
                                                                    END
                                                          END
                                                END
                                           ELSE CASE WHEN g.MilitaryRankCategory = 1 AND g.MilitaryRankSubCategory = 3 /*KEY_OFFICER_CANDIDATE*/ OR
                                                          g.MilitaryRankCategory = 1 AND g.MilitaryRankSubCategory = 1 /*KEY_SERGEANTS*/
                                                     THEN CASE WHEN e.OBR_VU = 'Y' 
                                                               THEN 'Военно училище'
                                                               ELSE CASE WHEN y.OBRV_EGNLS IS NOT NULL
                                                                         THEN 'Колеж'
                                                                         ELSE 'Без военно образование' 
                                                                    END
                                                          END
                                                     ELSE CASE WHEN g.MilitaryRankCategory = 1 AND g.MilitaryRankSubCategory = 2 /*KEY_SOLDIERS*/
                                                               THEN CASE WHEN e.OBR_VISHE = 'Y' 
                                                                         THEN 'Висше' 
                                                                         ELSE CASE WHEN e.OBR_SR = 'Y'
                                                                                   THEN 'Средно' 
                                                                                   ELSE CASE WHEN z.OBRG_EGNLS IS NOT NULL
                                                                                             THEN 'Основно' 
                                                                                             ELSE 'Без образование'
                                                                                        END
                                                                              END
                                                                    END
                                                               ELSE ''
                                                          END
                                                END
                                      END as PersonEducation,
                                      md.MilitaryDepartmentName as MilitaryDepartment,
                                      '' as StaffWeapon,
                                      '' as AppointmentDate
                               FROM PMIS_RES.RequestsCommands a
                               INNER JOIN PMIS_RES.RequestCommandPositions b ON b.RequestsCommandID = a.RequestsCommandID
                               LEFT OUTER JOIN PMIS_RES.CommandPositionMilRanks mr ON b.RequestCommandPositionID = mr.RequestCommandPositionID AND mr.IsPrimary = 1
                               LEFT OUTER JOIN VS_OWNER.KLV_ZVA b1 ON b1.ZVA_KOD = mr.MilitaryRankID
                               LEFT OUTER JOIN PMIS_RES.CommandPositionMRSpecialities b2 ON b2.RequestCommandPositionID = b.RequestCommandPositionID AND b2.IsPrimary = 1
                               LEFT OUTER JOIN PMIS_ADM.MilitaryReportSpecialities b3 ON b3.MilReportSpecialityID = b2.MilReportSpecialityID
                               LEFT OUTER JOIN PMIS_RES.FillReservistsRequest c ON c.RequestCommandPositionId = b.RequestCommandPositionId AND c.ReservistReadinessID = 1 AND NVL(c.AppointmentIsDelivered, 0) = 1
                               LEFT OUTER JOIN PMIS_RES.Reservists d ON d.ReservistId = c.ReservistId
                               LEFT OUTER JOIN VS_OWNER.VS_LS e ON e.PersonId = d.PersonId
                               LEFT OUTER JOIN PMIS_ADM.Persons pe ON pe.PersonId = e.PersonId
                               LEFT OUTER JOIN VS_OWNER.KLV_ZVA f ON f.ZVA_KOD = e.KOD_ZVA
                               LEFT OUTER JOIN PMIS_ADM.MilitaryRankCategories g ON g.ZVA_KAT_KOD = f.ZVA_KAT_KOD
                               LEFT OUTER JOIN PMIS_ADM.PersonMilRepSpec e1 ON e1.PersonID = e.PersonID AND e1.IsPrimary = 1
                               LEFT OUTER JOIN PMIS_ADM.MilitaryReportSpecialities e2 ON e2.MilReportSpecialityID = e1.MilReportSpecialityID
                               LEFT OUTER JOIN UKAZ_OWNER.KL_NMA p on e.KOD_NMA_MJ = p.KOD_NMA
                               LEFT OUTER JOIN UKAZ_OWNER.KL_VNM p2 ON p2.KOD_VNM = p.KOD_VNM
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBL p3 ON p3.KOD_OBL = p.KOD_OBL
                               LEFT OUTER JOIN UKAZ_OWNER.KL_OBS p4 ON p4.KOD_OBS = p.KOD_OBS
                               LEFT OUTER JOIN PMIS_RES.ReservistMilRepStatuses r ON d.ReservistID = r.ReservistID AND r.IsCurrent = 1
                               LEFT OUTER JOIN PMIS_ADM.MilitaryDepartments md ON md.MilitaryDepartmentID = r.SourceMilDepartmentID
                               LEFT OUTER JOIN (SELECT OBRV_EGNLS FROM VS_OWNER.VS_OBRV WHERE OBRV_VVUKOD = '" + MilitarySchoolUtil.CollegeDBKey + @"' GROUP BY OBRV_EGNLS) y ON e.EGN = y.OBRV_EGNLS
                               LEFT OUTER JOIN (SELECT OBRG_EGNLS 
                                                   FROM VS_OWNER.VS_OBRG 
                                                   WHERE OBRG_KOD = '" + PersonEducationUtil.OsnovnoDBKey + @"' OR 
                                                         OBRG_KOD = '" + PersonEducationUtil.Klas10DBKey + @"'
                                                   GROUP BY OBRG_EGNLS) z ON e.EGN = z.OBRG_EGNLS
                               " + whereClause + @"
                               ORDER BY 1, 2, 3, 5, 7
                              ";

                OracleCommand cmd = new OracleCommand(SQL, conn);
                
                OracleDataReader dr = cmd.ExecuteReader();

                int rowIdx = 0;

                while (dr.Read())
                {
                    rowIdx++;

                    ReportStaffPositionsListBlock block = new ReportStaffPositionsListBlock();

                    block.RowIndex = rowIdx;
                    block.PositionName = dr["PositionName"].ToString();
                    block.MilitaryRankName = dr["MilitaryRankName"].ToString();
                    block.StaffMRS = dr["StaffMRS"].ToString();
                    block.PositionCode = dr["PositionCode"].ToString();
                    block.PersonFullNameAndRank = dr["PersonFullNameAndRank"].ToString();
                    block.PersonMRS = dr["PersonMRS"].ToString();
                    block.PersonIdentityNumber = dr["PersonIdentityNumber"].ToString();
                    block.PersonPermAddress = dr["PersonPermAddress"].ToString();
                    block.PersonEducation = dr["PersonEducation"].ToString();
                    block.MilitaryDepartment = dr["MilitaryDepartment"].ToString();
                    block.StaffWeapon = dr["StaffWeapon"].ToString();
                    block.AppointmentDate = (dr["AppointmentDate"] is DateTime) ? (DateTime)dr["AppointmentDate"] : (DateTime?)null;

                    reportResult.Add(block);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return reportResult;
        }
    }
}