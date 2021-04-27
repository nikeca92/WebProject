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
    public class ReportAnalyzeTechCommandFilter
    {
        public string MilitaryDepartmentIds { get; set; }
        public int MilitaryReadinessID { get; set; }
        public int MilitaryCommandId { get; set; }
        public string MilitaryCommandSuffix { get; set; }
        public string ReportType { get; set; }        
    }

    public class ReportOverallAnalyzeTechCommandResultBlock
    {
        public string NormativeName { get; set; }
        public int RequestedPos { get; set; }
        public int FilledPos { get; set; }
        public int ReplacedPos { get; set; }
        public int ReservePos { get; set; }
    }

    public class ReportMilRepSpecAnalyzeTechCommandBlock
    {
        public string MilRepSpecName { get; set; }
        public int RequestOfficers { get; set; }
        public int RequestSergeants { get; set; }
        public int RequestSoldiers { get; set; }
        public int RequestOffCand { get; set; }
        public int FulfiledOfficers { get; set; }
        public int FulfiledSergeants { get; set; }
        public int FulfiledSoldiers { get; set; }
        public int FulfiledOffCand { get; set; }
        public int ChangesOfficers { get; set; }
        public int ChangesSergeants { get; set; }
        public int ChangesSoldiers { get; set; }
        public int ChangesOffCand { get; set; }
        public int ReserveOfficers { get; set; }
        public int ReserveSergeants { get; set; }
        public int ReserveSoldiers { get; set; }
        public int ReserveOffCand { get; set; }
    }

    public class ReportPositionMRSAnalyzeTechMRSBlock
    {
        public string MilitaryReportSpecilityName { get; set; }
        public int Cnt { get; set; }
    }

    public class ReportPositionMRSAnalyzeTechPositionBlock
    {
        public string Position { get; set; }
        public List<ReportPositionMRSAnalyzeTechMRSBlock> MRS { get; set; }
    }


    public class ReportPositionDeliveryAnalyzeTechBlock
    {
        public int TechnicsTypeID { get; set; }
        public int? VehicleKindID { get; set; }
        public string MuniciplaityName { get; set; }
        public string DistrictName { get; set; }
        public int Fulfiled { get; set; }
        public int Reserve { get; set; }
    }

    public class ReportAnalyzeTechCommandResult
    {
        public ReportAnalyzeTechCommandFilter Filter { get; set; }
        public List<ReportOverallAnalyzeTechCommandResultBlock> OverallResult { get; set; }
        public List<ReportPositionDeliveryAnalyzeTechBlock> PositionDeliveryResult { get; set; }
    }    

    public static class ReportAnalyzeTechCommandUtil
    {
        public static ReportAnalyzeTechCommandResult GetReportAnalyzeTechCommand(ReportAnalyzeTechCommandFilter filter, User currentUser)
        {
            ReportAnalyzeTechCommandResult reportResult = new ReportAnalyzeTechCommandResult();

            reportResult.OverallResult = ReportAnalyzeTechCommandUtil.GetReportOverallAnalyzeTechCommand(filter, currentUser);
            reportResult.PositionDeliveryResult = ReportAnalyzeTechCommandUtil.GetReportPositionDeliveryAnalyzeTech(filter, currentUser);
            return reportResult;
        }

        public static List<ReportOverallAnalyzeTechCommandResultBlock> GetReportOverallAnalyzeTechCommand(ReportAnalyzeTechCommandFilter filter, User currentUser)
        {
            List<ReportOverallAnalyzeTechCommandResultBlock> reportResult = new List<ReportOverallAnalyzeTechCommandResultBlock>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string whereClause = "";
                string whereClause2 = "";

                if (!String.IsNullOrEmpty(filter.MilitaryDepartmentIds))
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " d.MilitaryDepartmentId IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryDepartmentIds) + ") ";

                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                                    " x.MilitaryDepartmentId IN (" + CommonFunctions.AvoidSQLInjForListOfIDs(filter.MilitaryDepartmentIds) + ") ";
                }

                whereClause += (whereClause == "" ? "" : " AND ") +
                        @" (d.MilitaryDepartmentId IS NULL OR d.MilitaryDepartmentId IN ( " + currentUser.MilitaryDepartmentIDs + ")) ";

                whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                         @" (x.MilitaryDepartmentId IS NULL OR x.MilitaryDepartmentId IN ( " + currentUser.MilitaryDepartmentIDs + ")) ";

                if (filter.MilitaryReadinessID > -1)
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " b.MilReadinessID = " + filter.MilitaryReadinessID.ToString();

                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                                    " b.MilReadinessID = " + filter.MilitaryReadinessID.ToString();
                }

                if (filter.MilitaryCommandId > -1)
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " b.MilitaryCommandId = " + filter.MilitaryCommandId.ToString();

                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                                    " b.MilitaryCommandId = " + filter.MilitaryCommandId.ToString();
                }

                if (!String.IsNullOrEmpty(filter.MilitaryCommandSuffix) && filter.MilitaryCommandSuffix != "-1")
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " b.MilitaryCommandSuffix = '" + filter.MilitaryCommandSuffix + "' ";

                    whereClause2 += (whereClause2 == "" ? "" : " AND ") +
                                    " b.MilitaryCommandSuffix = '" + filter.MilitaryCommandSuffix + "' ";
                }

                whereClause = (whereClause == "" ? "" : " WHERE ") + whereClause;

                whereClause2 = (whereClause2 == "" ? "" : " WHERE ") + whereClause2;

                string SQL = @"
                                SELECT nt.NormativeTechnicsID, nt.NormativeCode, nt.NormativeName,
                                       t1.RequestedPos, t2.FilledPos, t2.ReplacedPos, t2.ReservePos
                                FROM PMIS_RES.NormativeTechnics nt INNER JOIN
                                (
                                    SELECT c.NormativeTechnicsID,
                                            SUM(NVL(x.Count,0))  As RequestedPos
                                    FROM UKAZ_OWNER.VVR a
                                    INNER JOIN PMIS_RES.TECHNICSREQUESTCOMMANDS b ON b.MilitaryCommandId = a.KOD_VVR
                                    INNER JOIN PMIS_RES.TECHNICSREQUESTCMDPOSITIONS c ON c.TechRequestsCommandID = b.TechRequestsCommandID                               
                                    LEFT OUTER JOIN PMIS_RES.TechRequestCmdPositionsMilDept x ON x.TechnicsRequestCmdPositionID = c.TechnicsRequestCmdPositionID
                                   " + whereClause2 + @"
                                    GROUP BY c.normativetechnicsid
                                )  t1 ON t1.NormativeTechnicsID = nt.NormativeTechnicsID
                                LEFT OUTER JOIN 
                                (
                                    SELECT   c.NormativeTechnicsID,
                                            SUM(CASE WHEN d.TechnicReadinessID = 1 THEN 1 ELSE 0 END) As FilledPos,
                                            SUM(CASE WHEN d.TechnicReadinessID = 1 AND f.NormativeTechnicsID <> c.NormativeTechnicsID THEN 1 ELSE 0 END) As ReplacedPos,
                                            SUM(CASE WHEN d.TechnicReadinessID = 2 THEN 1 ELSE 0 END) As ReservePos
                                    FROM UKAZ_OWNER.VVR a
                                    INNER JOIN PMIS_RES.TechnicsRequestCommands b ON b.MilitaryCommandId = a.KOD_VVR
                                    INNER JOIN PMIS_RES.TechnicsRequestCmdPositions c ON c.TechRequestsCommandID = b.TechRequestsCommandID
                                    INNER JOIN PMIS_RES.FulfilTechnicsRequest d ON d.TechnicsRequestCmdPositionID = c.TechnicsRequestCmdPositionID AND NVL(d.AppointmentIsDelivered, 0) = 1
                                    INNER JOIN PMIS_RES.Technics f ON f.TechnicsId = d.TechnicsId
                                   " + whereClause + @"
                                    GROUP BY c.normativetechnicsid
                                ) t2 ON t2.NormativeTechnicsID = nt.NormativeTechnicsID 
                                ORDER BY 1

                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    ReportOverallAnalyzeTechCommandResultBlock block = new ReportOverallAnalyzeTechCommandResultBlock();
                    block.NormativeName = dr["NormativeName"].ToString();

                    if (dr["RequestedPos"] is decimal)
                        block.RequestedPos = int.Parse(dr["RequestedPos"].ToString());

                    if (dr["FilledPos"] is decimal)
                        block.FilledPos = int.Parse(dr["FilledPos"].ToString());

                    if (dr["ReplacedPos"] is decimal)
                        block.ReplacedPos = int.Parse(dr["ReplacedPos"].ToString());

                    if (dr["ReservePos"] is decimal)
                        block.ReservePos = int.Parse(dr["ReservePos"].ToString());

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

        public static List<ReportPositionDeliveryAnalyzeTechBlock> GetReportPositionDeliveryAnalyzeTech(ReportAnalyzeTechCommandFilter filter, User currentUser)
        {
            List<ReportPositionDeliveryAnalyzeTechBlock> reportResult = new List<ReportPositionDeliveryAnalyzeTechBlock>();

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

                if (filter.MilitaryReadinessID > -1)
                {
                    whereClause += (whereClause == "" ? "" : " AND ") +
                                    " b.MilReadinessID = " + filter.MilitaryReadinessID.ToString();
                }

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

                whereClause = (whereClause == "" ? "" : " WHERE ") + whereClause;

                string SQL = @"                               
                                SELECT
                                    f.TechnicsTypeID,
                                    v.VehicleKindID,
	                                p.IME_OBS as MunicipalityName,
                                    dis.DistrictName,
                                    SUM(CASE WHEN d.TechnicReadinessID = 1 THEN 1 ELSE 0 END) As Fulfiled,
                                    SUM(CASE WHEN d.TechnicReadinessID = 2 THEN 1 ELSE 0 END) As Reserve
                                FROM UKAZ_OWNER.VVR a
                                INNER JOIN PMIS_RES.TechnicsRequestCommands b ON b.MilitaryCommandId = a.KOD_VVR
                                INNER JOIN PMIS_RES.TechnicsRequestCmdPositions c ON c.TechRequestsCommandID = b.TechRequestsCommandID
                                INNER JOIN PMIS_RES.FulfilTechnicsRequest d ON d.TechnicsRequestCmdPositionId = c.TechnicsRequestCmdPositionId AND NVL(d.AppointmentIsDelivered, 0) = 1
                                INNER JOIN PMIS_RES.Technics f ON f.TechnicsID = d.TechnicsID
                                INNER JOIN PMIS_ADM.Companies g ON f.OwnershipCompanyID = g.CompanyID
                                INNER JOIN UKAZ_OWNER.KL_NMA O ON O.KOD_NMA = g.CityID
                                INNER JOIN UKAZ_OWNER.KL_OBS p ON p.KOD_OBS = o.KOD_OBS
                                LEFT OUTER JOIN UKAZ_OWNER.Districts dis ON dis.DistrictID = g.DistrictID
                                LEFT OUTER JOIN PMIS_RES.Vehicles v ON v.TechnicsID = f.TechnicsID
                                " + whereClause + @"
                                GROUP BY f.TechnicsTypeID, v.VehicleKindID, p.IME_OBS, dis.DistrictName
                                ORDER BY 1
                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();
               

                while (dr.Read())
                {
                    ReportPositionDeliveryAnalyzeTechBlock block = new ReportPositionDeliveryAnalyzeTechBlock();

                    block.TechnicsTypeID = DBCommon.GetInt(dr["TechnicsTypeID"]);
                    if (DBCommon.IsInt(dr["VehicleKindID"]))
                        block.VehicleKindID = DBCommon.GetInt(dr["VehicleKindID"]);
                    block.MuniciplaityName = dr["MunicipalityName"].ToString();
                    block.DistrictName = dr["DistrictName"].ToString();
                    block.Fulfiled = int.Parse(dr["Fulfiled"].ToString());
                    block.Reserve = int.Parse(dr["Reserve"].ToString());

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