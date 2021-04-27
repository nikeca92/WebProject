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
    public class ReportAnalyzeResFulfilmentFilter
    {
        public int MilitaryUnitId { get; set; }
        public string MilitaryCommandIds { get; set; }
        public string MilitaryCategoryKey { get; set; }
    }

    public class ReportAnalyzeResFulfilmentByCategoryBlock
    {
        public string MilitaryCommand { get; set; }
        public string MilitaryUnit { get; set; }
        public int FulfiledOfficers { get; set; }
        public int FulfiledOffCand { get; set; }
        public int FulfiledSergeants { get; set; }
        public int FulfiledSoldiers { get; set; }
        public int FulfiledTotal { get; set; }
    }

    public class ReportAnalyzeResFulfilmentByMRSBlock
    {
        public string MilitaryCommand { get; set; }
        public int CountByStaff { get; set; }
        public int FulfiledByMRS { get; set; }
        public decimal FulfiledByMRSPerc { get; set; }
        public int ChangesCnt { get; set; }
        public decimal ChangesPerc { get; set; }
    }

    public class ReportAnalyzeResFulfilmentByCommandBlock
    {
        public string MilitaryCommand { get; set; }
        public string MilitaryUnit { get; set; }
        public int CountByStaff { get; set; }
        public int FulfiledByMilitaryDepartment { get; set; }
        public decimal FulfiledPerc { get; set; }
    }

    public class ReportAnalyzeResFulfilmentByAgeBlock
    {
        public string MilitaryCommand { get; set; }
        public int FulfiledAgeUnder35 { get; set; }
        public int FulfiledAgeUnder45 { get; set; }
        public int FulfiledAgeAbove45 { get; set; }
    }

    public class ReportAnalyzeResFulfilmentByEdu_Officers_Block
    {
        public string MilitaryCommand { get; set; }
        public int FulfiledVA { get; set; }
        public int FulfiledVU { get; set; }
        public int FulfiledSZHO { get; set; }
        public int FulfiledNoMilEdu { get; set; }
        public int FulfiledTotal { get; set; }
    }

    public class ReportAnalyzeResFulfilmentByEdu_OffCand_Block
    {
        public string MilitaryCommand { get; set; }
        public int FulfiledVU { get; set; }
        public int FulfiledCollege { get; set; }
        public int FulfiledNoMilEdu { get; set; }
        public int FulfiledTotal { get; set; }
    }

    public class ReportAnalyzeResFulfilmentByEdu_Sergeants_Block
    {
        public string MilitaryCommand { get; set; }
        public int FulfiledVU { get; set; }
        public int FulfiledCollege { get; set; }
        public int FulfiledNoMilEdu { get; set; }
        public int FulfiledTotal { get; set; }
    }

    public class ReportAnalyzeResFulfilmentByEdu_Soldiers_Block
    {
        public string MilitaryCommand { get; set; }
        public int FulfiledVisshe { get; set; }
        public int FulfiledSredno { get; set; }
        public int FulfiledOsnovno { get; set; }
        public int FulfiledNoEdu { get; set; }
        public int FulfiledTotal { get; set; }
    }

    public class ReportAnalyzeResFulfilmentResult
    {
        public ReportAnalyzeResFulfilmentFilter Filter { get; set; }
        public List<ReportAnalyzeResFulfilmentByCategoryBlock> ByCategoryResult { get; set; }
        public List<ReportAnalyzeResFulfilmentByMRSBlock> ByMRSResult { get; set; }
        public List<ReportAnalyzeResFulfilmentByCommandBlock> ByCommandResult { get; set; }
        public List<ReportAnalyzeResFulfilmentByAgeBlock> ByAgeResult { get; set; }
        public List<ReportAnalyzeResFulfilmentByEdu_Officers_Block> ByEdu_Officers_Result { get; set; }
        public List<ReportAnalyzeResFulfilmentByEdu_OffCand_Block> ByEdu_OffCand_Result { get; set; }
        public List<ReportAnalyzeResFulfilmentByEdu_Sergeants_Block> ByEdu_Sergeants_Result { get; set; }
        public List<ReportAnalyzeResFulfilmentByEdu_Soldiers_Block> ByEdu_Soldiers_Result { get; set; }
    }

    public static class ReportAnalyzeResFulfilmentUtil
    {
        public static ReportAnalyzeResFulfilmentResult GetReportAnalyzeResFulfilment(ReportAnalyzeResFulfilmentFilter filter, User currentUser)
        {
            ReportAnalyzeResFulfilmentResult reportResult = new ReportAnalyzeResFulfilmentResult();

            reportResult.ByCategoryResult = ReportAnalyzeResFulfilmentUtil.GetByCategoryResult(filter, currentUser);
            reportResult.ByMRSResult = ReportAnalyzeResFulfilmentUtil.GetByMRSResult(filter, currentUser);
            reportResult.ByCommandResult = ReportAnalyzeResFulfilmentUtil.GetByCommandResult(filter, currentUser);
            reportResult.ByAgeResult = ReportAnalyzeResFulfilmentUtil.GetByAgeResult(filter, currentUser);
            
            if (filter.MilitaryCategoryKey == "KEY_OFFICER")
                reportResult.ByEdu_Officers_Result = ReportAnalyzeResFulfilmentUtil.GetByEdu_Officers_Result(filter, currentUser);
            else if (filter.MilitaryCategoryKey == "KEY_OFFICER_CANDIDATE")
                reportResult.ByEdu_OffCand_Result = ReportAnalyzeResFulfilmentUtil.GetByEdu_OffCand_Result(filter, currentUser);
            else if (filter.MilitaryCategoryKey == "KEY_SERGEANTS")
                reportResult.ByEdu_Sergeants_Result = ReportAnalyzeResFulfilmentUtil.GetByEdu_Sergeants_Result(filter, currentUser);
            else if (filter.MilitaryCategoryKey == "KEY_SOLDIERS")
                reportResult.ByEdu_Soldiers_Result = ReportAnalyzeResFulfilmentUtil.GetByEdu_Soldiers_Result(filter, currentUser);

            return reportResult;
        }

        public static List<ReportAnalyzeResFulfilmentByCategoryBlock> GetByCategoryResult(ReportAnalyzeResFulfilmentFilter filter, User currentUser)
        {
            List<ReportAnalyzeResFulfilmentByCategoryBlock> reportResult = new List<ReportAnalyzeResFulfilmentByCategoryBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string whereClause = "";
                string whereClause2 = "";

                if (!string.IsNullOrEmpty(filter.MilitaryCommandIds))
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                             @" a.KOD_VVR IN ( " + filter.MilitaryCommandIds + ") ";

                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                             @" a.MilitaryCommandID IN ( " + filter.MilitaryCommandIds + ") ";
                }
                else
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                             @" a.KOD_VVR IN (-1) ";

                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                             @" a.MilitaryCommandID IN (-1) ";
                }
                
                whereClause = (whereClause == "" ? "" : " WHERE ") + whereClause;
                whereClause2 = (whereClause2 == "" ? "" : " WHERE ") + whereClause2;

                string SQL = @"SELECT a.NK as MilitaryCommand,
                                      a.IMEED as MilitaryUnit,
                                      NVL(r.FulfiledOfficers, 0) as FulfiledOfficers,
                                      NVL(r.FulfiledOffCand, 0) as FulfiledOffCand,
                                      NVL(r.FulfiledSergeants, 0) as FulfiledSergeants,
                                      NVL(r.FulfiledSoldiers, 0) as FulfiledSoldiers
                               FROM UKAZ_OWNER.VVR a
                               INNER JOIN UKAZ_OWNER.STRV b ON a.KOD_VVR = b.KOD_VVR
                               LEFT OUTER JOIN (
                                  SELECT a.MilitaryCommandID,
                                         SUM(CASE WHEN g.MilitaryRankCategory = 2 THEN 1 ELSE 0 END) As FulfiledOfficers,
                                         SUM(CASE WHEN g.MilitaryRankCategory = 1 AND g.MilitaryRankSubCategory = 1 THEN 1 ELSE 0 END) As FulfiledSergeants,
                                         SUM(CASE WHEN g.MilitaryRankCategory = 1 AND g.MilitaryRankSubCategory = 2 THEN 1 ELSE 0 END) As FulfiledSoldiers,
                                         SUM(CASE WHEN g.MilitaryRankCategory = 1 AND g.MilitaryRankSubCategory = 3 THEN 1 ELSE 0 END) As FulfiledOffCand
                                  FROM PMIS_RES.RequestsCommands a
                                  INNER JOIN PMIS_RES.RequestCommandPositions b ON b.RequestsCommandID = a.RequestsCommandID
                                  INNER JOIN PMIS_RES.FillReservistsRequest c ON c.RequestCommandPositionId = b.RequestCommandPositionId AND c.ReservistReadinessID = 1 AND NVL(c.AppointmentIsDelivered, 0) = 1
                                  INNER JOIN PMIS_RES.Reservists d ON d.ReservistId = c.ReservistId
                                  INNER JOIN VS_OWNER.VS_LS e ON e.PersonId = d.PersonId                               
                                  INNER JOIN VS_OWNER.KLV_ZVA f ON f.ZVA_KOD = e.KOD_ZVA                               
                                  INNER JOIN PMIS_ADM.MilitaryRankCategories g ON g.ZVA_KAT_KOD = f.ZVA_KAT_KOD
                                  " + whereClause2 + @"
                                  GROUP BY a.MilitaryCommandID
                               ) r ON a.KOD_VVR = r.MilitaryCommandID
                               " + whereClause + @"
                               ORDER BY 1
                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ReportAnalyzeResFulfilmentByCategoryBlock block = new ReportAnalyzeResFulfilmentByCategoryBlock();

                    block.MilitaryCommand = dr["MilitaryCommand"].ToString();
                    block.MilitaryUnit = dr["MilitaryUnit"].ToString();

                    if (dr["FulfiledOfficers"] is decimal)
                        block.FulfiledOfficers = int.Parse(dr["FulfiledOfficers"].ToString());

                    if (dr["FulfiledOffCand"] is decimal)
                        block.FulfiledOffCand = int.Parse(dr["FulfiledOffCand"].ToString());

                    if (dr["FulfiledSergeants"] is decimal)
                        block.FulfiledSergeants = int.Parse(dr["FulfiledSergeants"].ToString());

                    if (dr["FulfiledSoldiers"] is decimal)
                        block.FulfiledSoldiers = int.Parse(dr["FulfiledSoldiers"].ToString());

                    block.FulfiledTotal = block.FulfiledOfficers + block.FulfiledOffCand + block.FulfiledSergeants + block.FulfiledSoldiers;

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

        public static List<ReportAnalyzeResFulfilmentByMRSBlock> GetByMRSResult(ReportAnalyzeResFulfilmentFilter filter, User currentUser)
        {
            List<ReportAnalyzeResFulfilmentByMRSBlock> reportResult = new List<ReportAnalyzeResFulfilmentByMRSBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string whereClause = "";
                string whereClause2 = "";

                if (!string.IsNullOrEmpty(filter.MilitaryCommandIds))
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                             @" a.KOD_VVR IN ( " + filter.MilitaryCommandIds + ") ";

                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                             @" a.MilitaryCommandID IN ( " + filter.MilitaryCommandIds + ") ";
                }
                else
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                             @" a.KOD_VVR IN (-1) ";

                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                             @" a.MilitaryCommandID IN (-1) ";
                }

                if (filter.MilitaryCategoryKey == "KEY_OFFICER")
                {
                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                             @" g.MilitaryRankCategory = 2 ";
                }
                else if (filter.MilitaryCategoryKey == "KEY_OFFICER_CANDIDATE")
                {
                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                             @" g.MilitaryRankCategory = 1 AND g.MilitaryRankSubCategory = 3 ";
                }
                else if (filter.MilitaryCategoryKey == "KEY_SERGEANTS")
                {
                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                             @" g.MilitaryRankCategory = 1 AND g.MilitaryRankSubCategory = 1 ";
                }
                else if (filter.MilitaryCategoryKey == "KEY_SOLDIERS")
                {
                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                             @" g.MilitaryRankCategory = 1 AND g.MilitaryRankSubCategory = 2 ";
                }

                whereClause = (whereClause == "" ? "" : " WHERE ") + whereClause;
                whereClause2 = (whereClause2 == "" ? "" : " WHERE ") + whereClause2;

                string SQL = @"SELECT a.NK as MilitaryCommand,
                                      NVL(z.CountByStaff, 0) as CountByStaff,
                                      NVL(r.FulfiledByMRS, 0) as FulfiledByMRS,
                                      NVL(r.ChangesCnt, 0) as ChangesCnt
                               FROM UKAZ_OWNER.VVR a
                               INNER JOIN UKAZ_OWNER.STRV b ON a.KOD_VVR = b.KOD_VVR
                               LEFT OUTER JOIN (
                                  SELECT a.MilitaryCommandID,
                                         SUM(CASE WHEN c.MilReportSpecialityID = b1.MilReportSpecialityID THEN 1 ELSE 0 END) As FulfiledByMRS,
                                         SUM(CASE WHEN c.MilReportSpecialityID <> b1.MilReportSpecialityID THEN 1 ELSE 0 END) As ChangesCnt
                                  FROM PMIS_RES.RequestsCommands a
                                  INNER JOIN PMIS_RES.RequestCommandPositions b ON b.RequestsCommandID = a.RequestsCommandID
                                  LEFT OUTER JOIN PMIS_RES.CommandPositionMRSpecialities b1 ON b.RequestCommandPositionID = b1.RequestCommandPositionID AND b1.IsPrimary = 1
                                  INNER JOIN PMIS_RES.FillReservistsRequest c ON c.RequestCommandPositionId = b.RequestCommandPositionId AND c.ReservistReadinessID = 1 AND NVL(c.AppointmentIsDelivered, 0) = 1
                                  INNER JOIN PMIS_RES.Reservists d ON d.ReservistId = c.ReservistId
                                  INNER JOIN VS_OWNER.VS_LS e ON e.PersonId = d.PersonId                               
                                  INNER JOIN VS_OWNER.KLV_ZVA f ON f.ZVA_KOD = e.KOD_ZVA                               
                                  INNER JOIN PMIS_ADM.MilitaryRankCategories g ON g.ZVA_KAT_KOD = f.ZVA_KAT_KOD
                                  " + whereClause2 + @"
                                  GROUP BY a.MilitaryCommandID
                               ) r ON a.KOD_VVR = r.MilitaryCommandID
                               LEFT OUTER JOIN (
                                  SELECT a.MilitaryCommandID,
                                         SUM(b.ReservistsCount) As CountByStaff
                                  FROM PMIS_RES.RequestsCommands a
                                  INNER JOIN PMIS_RES.RequestCommandPositions b ON b.RequestsCommandID = a.RequestsCommandID
                                  INNER JOIN PMIS_RES.CommandPositionMilRanks mr ON b.RequestCommandPositionID = mr.RequestCommandPositionID AND mr.IsPrimary = 1
                                  INNER JOIN VS_OWNER.KLV_ZVA f ON f.ZVA_KOD = mr.MilitaryRankID
                                  INNER JOIN PMIS_ADM.MilitaryRankCategories g ON g.ZVA_KAT_KOD = f.ZVA_KAT_KOD
                                  " + whereClause2 + @"
                                  GROUP BY a.MilitaryCommandID
                               ) z ON a.KOD_VVR = z.MilitaryCommandID
                               " + whereClause + @"
                               ORDER BY 1
                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ReportAnalyzeResFulfilmentByMRSBlock block = new ReportAnalyzeResFulfilmentByMRSBlock();

                    block.MilitaryCommand = dr["MilitaryCommand"].ToString();

                    if (dr["CountByStaff"] is decimal)
                        block.CountByStaff = int.Parse(dr["CountByStaff"].ToString());

                    if (dr["FulfiledByMRS"] is decimal)
                        block.FulfiledByMRS = int.Parse(dr["FulfiledByMRS"].ToString());

                    if (block.CountByStaff != 0)
                        block.FulfiledByMRSPerc = block.FulfiledByMRS * 100.0m / block.CountByStaff;

                    if (dr["ChangesCnt"] is decimal)
                        block.ChangesCnt = int.Parse(dr["ChangesCnt"].ToString());

                    if (block.CountByStaff != 0)
                        block.ChangesPerc = block.ChangesCnt * 100.0m / block.CountByStaff;

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

        public static List<ReportAnalyzeResFulfilmentByCommandBlock> GetByCommandResult(ReportAnalyzeResFulfilmentFilter filter, User currentUser)
        {
            List<ReportAnalyzeResFulfilmentByCommandBlock> reportResult = new List<ReportAnalyzeResFulfilmentByCommandBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string whereClause = "";
                string whereClause2 = "";

                if (!string.IsNullOrEmpty(filter.MilitaryCommandIds))
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                             @" a.KOD_VVR IN ( " + filter.MilitaryCommandIds + ") ";

                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                             @" a.MilitaryCommandID IN ( " + filter.MilitaryCommandIds + ") ";
                }
                else
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                             @" a.KOD_VVR IN (-1) ";

                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                             @" a.MilitaryCommandID IN (-1) ";
                }

                if (filter.MilitaryCategoryKey == "KEY_OFFICER")
                {
                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                             @" g.MilitaryRankCategory = 2 ";
                }
                else if (filter.MilitaryCategoryKey == "KEY_OFFICER_CANDIDATE")
                {
                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                             @" g.MilitaryRankCategory = 1 AND g.MilitaryRankSubCategory = 3 ";
                }
                else if (filter.MilitaryCategoryKey == "KEY_SERGEANTS")
                {
                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                             @" g.MilitaryRankCategory = 1 AND g.MilitaryRankSubCategory = 1 ";
                }
                else if (filter.MilitaryCategoryKey == "KEY_SOLDIERS")
                {
                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                             @" g.MilitaryRankCategory = 1 AND g.MilitaryRankSubCategory = 2 ";
                }

                whereClause = (whereClause == "" ? "" : " WHERE ") + whereClause;
                whereClause2 = (whereClause2 == "" ? "" : " WHERE ") + whereClause2;

                string SQL = @"SELECT a.NK as MilitaryCommand,
                                      a.IMEED as MilitaryUnit,
                                      NVL(z.CountByStaff, 0) as CountByStaff,
                                      NVL(r.FulfiledByMilitaryDepartment, 0) as FulfiledByMilitaryDepartment
                               FROM UKAZ_OWNER.VVR a
                               INNER JOIN UKAZ_OWNER.STRV b ON a.KOD_VVR = b.KOD_VVR
                               LEFT OUTER JOIN (
                                  SELECT a.MilitaryCommandID,
                                         COUNT(*) As FulfiledByMilitaryDepartment
                                  FROM PMIS_RES.RequestsCommands a
                                  INNER JOIN PMIS_RES.RequestCommandPositions b ON b.RequestsCommandID = a.RequestsCommandID
                                  INNER JOIN PMIS_RES.FillReservistsRequest c ON c.RequestCommandPositionId = b.RequestCommandPositionId AND c.ReservistReadinessID = 1 AND NVL(c.AppointmentIsDelivered, 0) = 1
                                  INNER JOIN PMIS_RES.Reservists d ON d.ReservistId = c.ReservistId
                                  INNER JOIN VS_OWNER.VS_LS e ON e.PersonId = d.PersonId                               
                                  INNER JOIN VS_OWNER.KLV_ZVA f ON f.ZVA_KOD = e.KOD_ZVA                               
                                  INNER JOIN PMIS_ADM.MilitaryRankCategories g ON g.ZVA_KAT_KOD = f.ZVA_KAT_KOD
                                  " + whereClause2 + @"
                                  GROUP BY a.MilitaryCommandID
                               ) r ON a.KOD_VVR = r.MilitaryCommandID
                               LEFT OUTER JOIN (
                                  SELECT a.MilitaryCommandID,
                                         SUM(b.ReservistsCount) As CountByStaff
                                  FROM PMIS_RES.RequestsCommands a
                                  INNER JOIN PMIS_RES.RequestCommandPositions b ON b.RequestsCommandID = a.RequestsCommandID
                                  INNER JOIN PMIS_RES.CommandPositionMilRanks mr ON b.RequestCommandPositionID = mr.RequestCommandPositionID AND mr.IsPrimary = 1
                                  INNER JOIN VS_OWNER.KLV_ZVA f ON f.ZVA_KOD = mr.MilitaryRankID
                                  INNER JOIN PMIS_ADM.MilitaryRankCategories g ON g.ZVA_KAT_KOD = f.ZVA_KAT_KOD
                                  " + whereClause2 + @"
                                  GROUP BY a.MilitaryCommandID
                               ) z ON a.KOD_VVR = z.MilitaryCommandID
                               " + whereClause + @"
                               ORDER BY 1
                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ReportAnalyzeResFulfilmentByCommandBlock block = new ReportAnalyzeResFulfilmentByCommandBlock();

                    block.MilitaryCommand = dr["MilitaryCommand"].ToString();
                    block.MilitaryUnit = dr["MilitaryUnit"].ToString();

                    if (dr["CountByStaff"] is decimal)
                        block.CountByStaff = int.Parse(dr["CountByStaff"].ToString());

                    if (dr["FulfiledByMilitaryDepartment"] is decimal)
                        block.FulfiledByMilitaryDepartment = int.Parse(dr["FulfiledByMilitaryDepartment"].ToString());

                    if (block.CountByStaff != 0)
                        block.FulfiledPerc = block.FulfiledByMilitaryDepartment * 100.0m / block.CountByStaff;

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

        public static List<ReportAnalyzeResFulfilmentByAgeBlock> GetByAgeResult(ReportAnalyzeResFulfilmentFilter filter, User currentUser)
        {
            List<ReportAnalyzeResFulfilmentByAgeBlock> reportResult = new List<ReportAnalyzeResFulfilmentByAgeBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string whereClause = "";
                string whereClause2 = "";

                if (!string.IsNullOrEmpty(filter.MilitaryCommandIds))
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                             @" a.KOD_VVR IN ( " + filter.MilitaryCommandIds + ") ";

                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                             @" a.MilitaryCommandID IN ( " + filter.MilitaryCommandIds + ") ";
                }
                else
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                             @" a.KOD_VVR IN (-1) ";

                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                             @" a.MilitaryCommandID IN (-1) ";
                }

                if (filter.MilitaryCategoryKey == "KEY_OFFICER")
                {
                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                             @" g.MilitaryRankCategory = 2 ";
                }
                else if (filter.MilitaryCategoryKey == "KEY_OFFICER_CANDIDATE")
                {
                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                             @" g.MilitaryRankCategory = 1 AND g.MilitaryRankSubCategory = 3 ";
                }
                else if (filter.MilitaryCategoryKey == "KEY_SERGEANTS")
                {
                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                             @" g.MilitaryRankCategory = 1 AND g.MilitaryRankSubCategory = 1 ";
                }
                else if (filter.MilitaryCategoryKey == "KEY_SOLDIERS")
                {
                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                             @" g.MilitaryRankCategory = 1 AND g.MilitaryRankSubCategory = 2 ";
                }

                whereClause = (whereClause == "" ? "" : " WHERE ") + whereClause;
                whereClause2 = (whereClause2 == "" ? "" : " WHERE ") + whereClause2;

                string SQL = @"SELECT a.NK as MilitaryCommand,
                                      NVL(r.AgeUnder35, 0) as FulfiledAgeUnder35,
                                      NVL(r.AgeUnder45, 0) as FulfiledAgeUnder45,
                                      NVL(r.AgeAbove45, 0) as FulfiledAgeAbove45
                               FROM UKAZ_OWNER.VVR a
                               INNER JOIN UKAZ_OWNER.STRV b ON a.KOD_VVR = b.KOD_VVR
                               LEFT OUTER JOIN (
                                  SELECT a.MilitaryCommandID,
                                         SUM(CASE WHEN z.Age <= 35 THEN 1 ELSE 0 END) as AgeUnder35,
		                                 SUM(CASE WHEN z.Age > 35  AND z.Age <= 45 THEN 1 ELSE 0 END) as AgeUnder45,
                                         SUM(CASE WHEN z.Age > 45 THEN 1 ELSE 0 END) as AgeAbove45
                                  FROM PMIS_RES.RequestsCommands a
                                  INNER JOIN PMIS_RES.RequestCommandPositions b ON b.RequestsCommandID = a.RequestsCommandID
                                  INNER JOIN PMIS_RES.FillReservistsRequest c ON c.RequestCommandPositionId = b.RequestCommandPositionId AND c.ReservistReadinessID = 1 AND NVL(c.AppointmentIsDelivered, 0) = 1
                                  INNER JOIN PMIS_RES.Reservists d ON d.ReservistId = c.ReservistId
                                  INNER JOIN VS_OWNER.VS_LS e ON e.PersonId = d.PersonId                               
                                  INNER JOIN VS_OWNER.KLV_ZVA f ON f.ZVA_KOD = e.KOD_ZVA                               
                                  INNER JOIN PMIS_ADM.MilitaryRankCategories g ON g.ZVA_KAT_KOD = f.ZVA_KAT_KOD
                                  INNER JOIN (SELECT EGN as EGN, PMIS_ADM.COMMONFUNCTIONS.GetAgeFromEGN(EGN) as Age FROM VS_OWNER.VS_LS) z ON z.EGN = e.EGN
                                  " + whereClause2 + @"
                                  GROUP BY a.MilitaryCommandID
                               ) r ON a.KOD_VVR = r.MilitaryCommandID
                               " + whereClause + @"
                               ORDER BY 1
                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ReportAnalyzeResFulfilmentByAgeBlock block = new ReportAnalyzeResFulfilmentByAgeBlock();

                    block.MilitaryCommand = dr["MilitaryCommand"].ToString();

                    if (dr["FulfiledAgeUnder35"] is decimal)
                        block.FulfiledAgeUnder35 = int.Parse(dr["FulfiledAgeUnder35"].ToString());

                    if (dr["FulfiledAgeUnder45"] is decimal)
                        block.FulfiledAgeUnder45 = int.Parse(dr["FulfiledAgeUnder45"].ToString());

                    if (dr["FulfiledAgeAbove45"] is decimal)
                        block.FulfiledAgeAbove45 = int.Parse(dr["FulfiledAgeAbove45"].ToString());

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

        public static List<ReportAnalyzeResFulfilmentByEdu_Officers_Block> GetByEdu_Officers_Result(ReportAnalyzeResFulfilmentFilter filter, User currentUser)
        {
            List<ReportAnalyzeResFulfilmentByEdu_Officers_Block> reportResult = new List<ReportAnalyzeResFulfilmentByEdu_Officers_Block>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string whereClause = "";
                string whereClause2 = "";

                if (!string.IsNullOrEmpty(filter.MilitaryCommandIds))
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                             @" a.KOD_VVR IN ( " + filter.MilitaryCommandIds + ") ";

                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                             @" a.MilitaryCommandID IN ( " + filter.MilitaryCommandIds + ") ";
                }
                else
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                             @" a.KOD_VVR IN (-1) ";

                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                             @" a.MilitaryCommandID IN (-1) ";
                }

                //"KEY_OFFICER"
                whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                         @" g.MilitaryRankCategory = 2 ";
                
                whereClause = (whereClause == "" ? "" : " WHERE ") + whereClause;
                whereClause2 = (whereClause2 == "" ? "" : " WHERE ") + whereClause2;

                string SQL = @"SELECT a.NK as MilitaryCommand,
                                      NVL(r.FulfiledVA, 0) as FulfiledVA,
                                      NVL(r.FulfiledVU, 0) as FulfiledVU,
                                      NVL(r.FulfiledSZHO, 0) as FulfiledSZHO,
                                      NVL(r.FulfiledNoMilEdu, 0) as FulfiledNoMilEdu,
                                      NVL(r.FulfiledTotal, 0) as FulfiledTotal
                               FROM UKAZ_OWNER.VVR a
                               INNER JOIN UKAZ_OWNER.STRV b ON a.KOD_VVR = b.KOD_VVR
                               LEFT OUTER JOIN (
                                  SELECT a.MilitaryCommandID,
                                         SUM(CASE WHEN e.OBR_VA = 'Y' THEN 1 ELSE 0 END) as FulfiledVA,
                                         SUM(CASE WHEN e.OBR_VU = 'Y' THEN 1 ELSE 0 END) as FulfiledVU,
                                         SUM(CASE WHEN e.OBR_SHZO = 'Y' THEN 1 ELSE 0 END) as FulfiledSZHO,
                                         SUM(CASE WHEN NVL(e.OBR_VA, 'N') = 'N' AND NVL(e.OBR_VU, 'N') = 'N' AND NVL(e.OBR_SHZO, 'N') = 'N' THEN 1 ELSE 0 END) as FulfiledNoMilEdu,
                                         COUNT(*) as FulfiledTotal
                                  FROM PMIS_RES.RequestsCommands a
                                  INNER JOIN PMIS_RES.RequestCommandPositions b ON b.RequestsCommandID = a.RequestsCommandID
                                  INNER JOIN PMIS_RES.FillReservistsRequest c ON c.RequestCommandPositionId = b.RequestCommandPositionId AND c.ReservistReadinessID = 1 AND NVL(c.AppointmentIsDelivered, 0) = 1
                                  INNER JOIN PMIS_RES.Reservists d ON d.ReservistId = c.ReservistId
                                  INNER JOIN VS_OWNER.VS_LS e ON e.PersonId = d.PersonId                               
                                  INNER JOIN VS_OWNER.KLV_ZVA f ON f.ZVA_KOD = e.KOD_ZVA                               
                                  INNER JOIN PMIS_ADM.MilitaryRankCategories g ON g.ZVA_KAT_KOD = f.ZVA_KAT_KOD
                                  " + whereClause2 + @"
                                  GROUP BY a.MilitaryCommandID
                               ) r ON a.KOD_VVR = r.MilitaryCommandID
                               " + whereClause + @"
                               ORDER BY 1
                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ReportAnalyzeResFulfilmentByEdu_Officers_Block block = new ReportAnalyzeResFulfilmentByEdu_Officers_Block();

                    block.MilitaryCommand = dr["MilitaryCommand"].ToString();

                    if (dr["FulfiledVA"] is decimal)
                        block.FulfiledVA = int.Parse(dr["FulfiledVA"].ToString());

                    if (dr["FulfiledVU"] is decimal)
                        block.FulfiledVU = int.Parse(dr["FulfiledVU"].ToString());

                    if (dr["FulfiledSZHO"] is decimal)
                        block.FulfiledSZHO = int.Parse(dr["FulfiledSZHO"].ToString());

                    if (dr["FulfiledNoMilEdu"] is decimal)
                        block.FulfiledNoMilEdu = int.Parse(dr["FulfiledNoMilEdu"].ToString());

                    if (dr["FulfiledTotal"] is decimal)
                        block.FulfiledTotal = int.Parse(dr["FulfiledTotal"].ToString());

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

        public static List<ReportAnalyzeResFulfilmentByEdu_OffCand_Block> GetByEdu_OffCand_Result(ReportAnalyzeResFulfilmentFilter filter, User currentUser)
        {
            List<ReportAnalyzeResFulfilmentByEdu_OffCand_Block> reportResult = new List<ReportAnalyzeResFulfilmentByEdu_OffCand_Block>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string whereClause = "";
                string whereClause2 = "";

                if (!string.IsNullOrEmpty(filter.MilitaryCommandIds))
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                             @" a.KOD_VVR IN ( " + filter.MilitaryCommandIds + ") ";

                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                             @" a.MilitaryCommandID IN ( " + filter.MilitaryCommandIds + ") ";
                }
                else
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                             @" a.KOD_VVR IN (-1) ";

                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                             @" a.MilitaryCommandID IN (-1) ";
                }

                //"KEY_OFFICER_CANDIDATE"
                whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                         @" g.MilitaryRankCategory = 1 AND g.MilitaryRankSubCategory = 3 ";

                whereClause = (whereClause == "" ? "" : " WHERE ") + whereClause;
                whereClause2 = (whereClause2 == "" ? "" : " WHERE ") + whereClause2;

                string SQL = @"SELECT a.NK as MilitaryCommand,
                                      NVL(r.FulfiledVU, 0) as FulfiledVU,
                                      NVL(r.FulfiledCollege, 0) as FulfiledCollege,
                                      NVL(r.FulfiledNoMilEdu, 0) as FulfiledNoMilEdu,
                                      NVL(r.FulfiledTotal, 0) as FulfiledTotal
                               FROM UKAZ_OWNER.VVR a
                               INNER JOIN UKAZ_OWNER.STRV b ON a.KOD_VVR = b.KOD_VVR
                               LEFT OUTER JOIN (
                                  SELECT a.MilitaryCommandID,
                                         SUM(CASE WHEN e.OBR_VU = 'Y' THEN 1 ELSE 0 END) as FulfiledVU,
                                         SUM(CASE WHEN y.OBRV_EGNLS IS NOT NULL THEN 1 ELSE 0 END) as FulfiledCollege,
                                         SUM(CASE WHEN NVL(e.OBR_VU, 'N') = 'N' AND y.OBRV_EGNLS IS NULL THEN 1 ELSE 0 END) as FulfiledNoMilEdu,
                                         COUNT(*) as FulfiledTotal
                                  FROM PMIS_RES.RequestsCommands a
                                  INNER JOIN PMIS_RES.RequestCommandPositions b ON b.RequestsCommandID = a.RequestsCommandID
                                  INNER JOIN PMIS_RES.FillReservistsRequest c ON c.RequestCommandPositionId = b.RequestCommandPositionId AND c.ReservistReadinessID = 1 AND NVL(c.AppointmentIsDelivered, 0) = 1
                                  INNER JOIN PMIS_RES.Reservists d ON d.ReservistId = c.ReservistId
                                  INNER JOIN VS_OWNER.VS_LS e ON e.PersonId = d.PersonId                               
                                  INNER JOIN VS_OWNER.KLV_ZVA f ON f.ZVA_KOD = e.KOD_ZVA                               
                                  INNER JOIN PMIS_ADM.MilitaryRankCategories g ON g.ZVA_KAT_KOD = f.ZVA_KAT_KOD
                                  LEFT OUTER JOIN (SELECT OBRV_EGNLS FROM VS_OWNER.VS_OBRV WHERE OBRV_VVUKOD = '" + MilitarySchoolUtil.CollegeDBKey + @"' GROUP BY OBRV_EGNLS) y ON e.EGN = y.OBRV_EGNLS
                                  " + whereClause2 + @"
                                  GROUP BY a.MilitaryCommandID
                               ) r ON a.KOD_VVR = r.MilitaryCommandID
                               " + whereClause + @"
                               ORDER BY 1
                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ReportAnalyzeResFulfilmentByEdu_OffCand_Block block = new ReportAnalyzeResFulfilmentByEdu_OffCand_Block();

                    block.MilitaryCommand = dr["MilitaryCommand"].ToString();

                    if (dr["FulfiledVU"] is decimal)
                        block.FulfiledVU = int.Parse(dr["FulfiledVU"].ToString());

                    if (dr["FulfiledCollege"] is decimal)
                        block.FulfiledCollege = int.Parse(dr["FulfiledCollege"].ToString());

                    if (dr["FulfiledNoMilEdu"] is decimal)
                        block.FulfiledNoMilEdu = int.Parse(dr["FulfiledNoMilEdu"].ToString());

                    if (dr["FulfiledTotal"] is decimal)
                        block.FulfiledTotal = int.Parse(dr["FulfiledTotal"].ToString());

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

        public static List<ReportAnalyzeResFulfilmentByEdu_Sergeants_Block> GetByEdu_Sergeants_Result(ReportAnalyzeResFulfilmentFilter filter, User currentUser)
        {
            List<ReportAnalyzeResFulfilmentByEdu_Sergeants_Block> reportResult = new List<ReportAnalyzeResFulfilmentByEdu_Sergeants_Block>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string whereClause = "";
                string whereClause2 = "";

                if (!string.IsNullOrEmpty(filter.MilitaryCommandIds))
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                             @" a.KOD_VVR IN ( " + filter.MilitaryCommandIds + ") ";

                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                             @" a.MilitaryCommandID IN ( " + filter.MilitaryCommandIds + ") ";
                }
                else
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                             @" a.KOD_VVR IN (-1) ";

                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                             @" a.MilitaryCommandID IN (-1) ";
                }

                //"KEY_SERGEANTS"
                whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                         @" g.MilitaryRankCategory = 1 AND g.MilitaryRankSubCategory = 1 ";

                whereClause = (whereClause == "" ? "" : " WHERE ") + whereClause;
                whereClause2 = (whereClause2 == "" ? "" : " WHERE ") + whereClause2;

                string SQL = @"SELECT a.NK as MilitaryCommand,
                                      NVL(r.FulfiledVU, 0) as FulfiledVU,
                                      NVL(r.FulfiledCollege, 0) as FulfiledCollege,
                                      NVL(r.FulfiledNoMilEdu, 0) as FulfiledNoMilEdu,
                                      NVL(r.FulfiledTotal, 0) as FulfiledTotal
                               FROM UKAZ_OWNER.VVR a
                               INNER JOIN UKAZ_OWNER.STRV b ON a.KOD_VVR = b.KOD_VVR
                               LEFT OUTER JOIN (
                                  SELECT a.MilitaryCommandID,
                                         SUM(CASE WHEN e.OBR_VU = 'Y' THEN 1 ELSE 0 END) as FulfiledVU,
                                         SUM(CASE WHEN y.OBRV_EGNLS IS NOT NULL THEN 1 ELSE 0 END) as FulfiledCollege,
                                         SUM(CASE WHEN NVL(e.OBR_VU, 'N') = 'N' AND y.OBRV_EGNLS IS NULL THEN 1 ELSE 0 END) as FulfiledNoMilEdu,
                                         COUNT(*) as FulfiledTotal
                                  FROM PMIS_RES.RequestsCommands a
                                  INNER JOIN PMIS_RES.RequestCommandPositions b ON b.RequestsCommandID = a.RequestsCommandID
                                  INNER JOIN PMIS_RES.FillReservistsRequest c ON c.RequestCommandPositionId = b.RequestCommandPositionId AND c.ReservistReadinessID = 1 AND NVL(c.AppointmentIsDelivered, 0) = 1
                                  INNER JOIN PMIS_RES.Reservists d ON d.ReservistId = c.ReservistId
                                  INNER JOIN VS_OWNER.VS_LS e ON e.PersonId = d.PersonId                               
                                  INNER JOIN VS_OWNER.KLV_ZVA f ON f.ZVA_KOD = e.KOD_ZVA                               
                                  INNER JOIN PMIS_ADM.MilitaryRankCategories g ON g.ZVA_KAT_KOD = f.ZVA_KAT_KOD
                                  LEFT OUTER JOIN (SELECT OBRV_EGNLS FROM VS_OWNER.VS_OBRV WHERE OBRV_VVUKOD = '" + MilitarySchoolUtil.CollegeDBKey + @"' GROUP BY OBRV_EGNLS) y ON e.EGN = y.OBRV_EGNLS
                                  " + whereClause2 + @"
                                  GROUP BY a.MilitaryCommandID
                               ) r ON a.KOD_VVR = r.MilitaryCommandID
                               " + whereClause + @"
                               ORDER BY 1
                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ReportAnalyzeResFulfilmentByEdu_Sergeants_Block block = new ReportAnalyzeResFulfilmentByEdu_Sergeants_Block();

                    block.MilitaryCommand = dr["MilitaryCommand"].ToString();

                    if (dr["FulfiledVU"] is decimal)
                        block.FulfiledVU = int.Parse(dr["FulfiledVU"].ToString());

                    if (dr["FulfiledCollege"] is decimal)
                        block.FulfiledCollege = int.Parse(dr["FulfiledCollege"].ToString());

                    if (dr["FulfiledNoMilEdu"] is decimal)
                        block.FulfiledNoMilEdu = int.Parse(dr["FulfiledNoMilEdu"].ToString());

                    block.FulfiledTotal = block.FulfiledVU + block.FulfiledCollege + block.FulfiledNoMilEdu;

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

        public static List<ReportAnalyzeResFulfilmentByEdu_Soldiers_Block> GetByEdu_Soldiers_Result(ReportAnalyzeResFulfilmentFilter filter, User currentUser)
        {
            List<ReportAnalyzeResFulfilmentByEdu_Soldiers_Block> reportResult = new List<ReportAnalyzeResFulfilmentByEdu_Soldiers_Block>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string whereClause = "";
                string whereClause2 = "";

                if (!string.IsNullOrEmpty(filter.MilitaryCommandIds))
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                             @" a.KOD_VVR IN ( " + filter.MilitaryCommandIds + ") ";

                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                             @" a.MilitaryCommandID IN ( " + filter.MilitaryCommandIds + ") ";
                }
                else
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                             @" a.KOD_VVR IN (-1) ";

                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                             @" a.MilitaryCommandID IN (-1) ";
                }

                //"KEY_SOLDIERS"
                whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                         @" g.MilitaryRankCategory = 1 AND g.MilitaryRankSubCategory = 2 ";

                whereClause = (whereClause == "" ? "" : " WHERE ") + whereClause;
                whereClause2 = (whereClause2 == "" ? "" : " WHERE ") + whereClause2;

                string SQL = @"SELECT a.NK as MilitaryCommand,
                                      NVL(r.FulfiledVisshe, 0) as FulfiledVisshe,
                                      NVL(r.FulfiledSredno, 0) as FulfiledSredno,
                                      NVL(r.FulfiledOsnovno, 0) as FulfiledOsnovno,
                                      NVL(r.FulfiledNoEdu, 0) as FulfiledNoEdu,
                                      NVL(r.FulfiledTotal, 0) as FulfiledTotal
                               FROM UKAZ_OWNER.VVR a
                               INNER JOIN UKAZ_OWNER.STRV b ON a.KOD_VVR = b.KOD_VVR
                               LEFT OUTER JOIN (
                                  SELECT a.MilitaryCommandID,
                                         SUM(CASE WHEN e.OBR_VISHE = 'Y' THEN 1 ELSE 0 END) as FulfiledVisshe,
                                         SUM(CASE WHEN e.OBR_SR = 'Y' AND NVL(e.OBR_VISHE, '_') <> 'Y' THEN 1 ELSE 0 END) as FulfiledSredno,
                                         SUM(CASE WHEN z.OBRG_EGNLS IS NOT NULL AND NVL(e.OBR_SR, '_') <> 'Y' AND NVL(e.OBR_VISHE, '_') <> 'Y' THEN 1 ELSE 0 END) as FulfiledOsnovno,
                                         SUM(CASE WHEN NOT (e.OBR_VISHE = 'Y') AND
                                                       NOT (e.OBR_SR = 'Y' AND NVL(e.OBR_VISHE, '_') <> 'Y') AND
                                                       NOT (z.OBRG_EGNLS IS NOT NULL AND NVL(e.OBR_SR, '_') <> 'Y' AND NVL(e.OBR_VISHE, '_') <> 'Y') 
                                                  THEN 1 ELSE 0 END) as FulfiledNoEdu,
                                         COUNT(*) as FulfiledTotal
                                  FROM PMIS_RES.RequestsCommands a
                                  INNER JOIN PMIS_RES.RequestCommandPositions b ON b.RequestsCommandID = a.RequestsCommandID
                                  INNER JOIN PMIS_RES.FillReservistsRequest c ON c.RequestCommandPositionId = b.RequestCommandPositionId AND c.ReservistReadinessID = 1 AND NVL(c.AppointmentIsDelivered, 0) = 1
                                  INNER JOIN PMIS_RES.Reservists d ON d.ReservistId = c.ReservistId
                                  INNER JOIN VS_OWNER.VS_LS e ON e.PersonId = d.PersonId                               
                                  INNER JOIN VS_OWNER.KLV_ZVA f ON f.ZVA_KOD = e.KOD_ZVA                               
                                  INNER JOIN PMIS_ADM.MilitaryRankCategories g ON g.ZVA_KAT_KOD = f.ZVA_KAT_KOD
                                  LEFT OUTER JOIN (SELECT OBRG_EGNLS 
                                                   FROM VS_OWNER.VS_OBRG 
                                                   WHERE OBRG_KOD = '" + PersonEducationUtil.OsnovnoDBKey + @"' OR 
                                                         OBRG_KOD = '" + PersonEducationUtil.Klas10DBKey + @"'
                                                   GROUP BY OBRG_EGNLS) z ON e.EGN = z.OBRG_EGNLS
                                  " + whereClause2 + @"
                                  GROUP BY a.MilitaryCommandID
                               ) r ON a.KOD_VVR = r.MilitaryCommandID
                               " + whereClause + @"
                               ORDER BY 1
                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ReportAnalyzeResFulfilmentByEdu_Soldiers_Block block = new ReportAnalyzeResFulfilmentByEdu_Soldiers_Block();

                    block.MilitaryCommand = dr["MilitaryCommand"].ToString();

                    if (dr["FulfiledVisshe"] is decimal)
                        block.FulfiledVisshe = int.Parse(dr["FulfiledVisshe"].ToString());

                    if (dr["FulfiledSredno"] is decimal)
                        block.FulfiledSredno = int.Parse(dr["FulfiledSredno"].ToString());

                    if (dr["FulfiledOsnovno"] is decimal)
                        block.FulfiledOsnovno = int.Parse(dr["FulfiledOsnovno"].ToString());

                    if (dr["FulfiledNoEdu"] is decimal)
                        block.FulfiledNoEdu = int.Parse(dr["FulfiledNoEdu"].ToString());

                    if (dr["FulfiledTotal"] is decimal)
                        block.FulfiledTotal = int.Parse(dr["FulfiledTotal"].ToString());

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