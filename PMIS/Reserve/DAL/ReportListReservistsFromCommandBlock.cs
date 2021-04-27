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
    public class ReportListReservistsFromCommandFilter
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

    public class ReportListReservistsFromCommandBlock : BaseDbObject
    {
        private int reservistId;
        private string militarySubCommand;
        private string position;
        private string militaryReportingSpecialty;
        private string militaryRank;
        private string readiness;
        private string identityNumber;
        private string fullName;
        private string permPlaceName;

        public int ReservistId
        {
            get
            {
                return reservistId;
            }
            set
            {
                reservistId = value;
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

        public string Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        public string MilitaryReportingSpecialty
        {
            get
            {
                return militaryReportingSpecialty;
            }
            set
            {
                militaryReportingSpecialty = value;
            }
        }

        public string MilitaryRank
        {
            get
            {
                return militaryRank;
            }
            set
            {
                militaryRank = value;
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

        public string IdentityNumber
        {
            get
            {
                return identityNumber;
            }
            set
            {
                identityNumber = value;
            }
        }

        public string FullName
        {
            get
            {
                return fullName;
            }
            set
            {
                fullName = value;
            }
        }

        public string PermPlaceName
        {
            get
            {
                return permPlaceName;
            }
            set
            {
                permPlaceName = value;
            }
        }

        public ReportListReservistsFromCommandBlock (User user)
            : base(user)
        {
        }
    }

    public class ReportListReservistsFromCommandBlockUtil
    {
        //This method get list of report items
        public static List<ReportListReservistsFromCommandBlock> GetReportListReservistsFromCommandBlockList(ReportListReservistsFromCommandFilter filter, User currentUser)
        {
            ReportListReservistsFromCommandBlock reportBlock;
            List<ReportListReservistsFromCommandBlock> listReportBlock = new List<ReportListReservistsFromCommandBlock>();

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
                        orderBySQL = "f.Position";
                        break;
                    case 3:
                        orderBySQL = "ms.MilReportingSpecialityCode || ' ' || ms.MilReportingSpecialityName";
                        break;
                    case 4:
                        orderBySQL = "z.ZVA_IMEES";
                        break;
                    case 5:
                        orderBySQL = "g.ReservistReadinessID";
                        break;
                    case 6:
                        orderBySQL = "v.EGN";
                        break;
                    case 7:
                        orderBySQL = "v.IME || ' ' || v.FAM";
                        break;
                    case 8:
                        orderBySQL = "obl.IME_OBL || CASE WHEN obl.IME_OBL IS NULL THEN '' ELSE ', ' END || obs.IME_OBS || CASE WHEN cities.Ime_Nma IS NULL THEN '' ELSE ', ' END || cities.Ime_Nma";
                        break;
                    default:
                        orderBySQL = "v.EGN";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT * FROM (
                                   SELECT r.ReservistId,
                                          e.MilitaryCommandSuffix || ' / ' || a.RequestNumber as MilitarySubCommand,
                                          f.Position, 
                                          ms.MilReportingSpecialityCode || ' ' || ms.MilReportingSpecialityName as MilitaryReportingSpecialty,
                                          z.ZVA_IMEES as MilitaryRank,
                                          g.ReservistReadinessID,
                                          v.EGN as IdentityNumber,
                                          v.IME || ' ' || v.FAM as FullName,
                                          obl.IME_OBL || CASE WHEN obl.IME_OBL IS NULL THEN '' ELSE ', ' END || obs.IME_OBS || CASE WHEN cities.Ime_Nma IS NULL THEN '' ELSE ', ' END || cities.Ime_Nma as PermPlaceName,
                                          DENSE_RANK() OVER (ORDER BY " + orderBySQL + @", v.EGN) as RowNumber 
                                   FROM PMIS_RES.EquipmentReservistsRequests a
                                   INNER JOIN PMIS_RES.RequestsCommands e ON a.EquipmentReservistsRequestID = e.EquipmentReservistsRequestID
                                   INNER JOIN PMIS_RES.RequestCommandPositions f ON e.RequestsCommandID = f.RequestsCommandID
                                   INNER JOIN PMIS_RES.FillReservistsRequest g ON f.RequestCommandPositionID = g.RequestCommandPositionID
                                   INNER JOIN UKAZ_OWNER.VVR h ON e.MilitaryCommandID = h.KOD_VVR 
                                   INNER JOIN PMIS_RES.Reservists r ON g.ReservistID = r.ReservistID
                                   INNER JOIN VS_OWNER.VS_LS v ON r.PersonID = v.PersonID
                                   LEFT OUTER JOIN PMIS_ADM.MilitaryReportSpecialities ms ON g.MilReportSpecialityID = ms.MilReportSpecialityID
                                   LEFT OUTER JOIN PMIS_RES.CommandPositionMilRanks mr ON f.RequestCommandPositionID = mr.RequestCommandPositionID AND mr.IsPrimary = 1
                                   LEFT OUTER JOIN VS_OWNER.KLV_ZVA z ON mr.MilitaryRankID = z.ZVA_KOD
                                   LEFT OUTER JOIN UKAZ_OWNER.KL_NMA cities ON v.KOD_NMA_MJ = cities.Kod_Nma
                                   LEFT OUTER JOIN UKAZ_OWNER.KL_OBS obs ON cities.KOD_OBS = obs.KOD_OBS
                                   LEFT OUTER JOIN UKAZ_OWNER.KL_OBL obl ON cities.KOD_OBL = obl.KOD_OBL             
                                   " + whereClause + @"
                                   ORDER BY " + orderBySQL + @", v.EGN
                                ) tmp
                               " + pageWhere; ;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    reportBlock = new ReportListReservistsFromCommandBlock(currentUser);

                    reportBlock.ReservistId = DBCommon.GetInt(dr["ReservistId"]);
                    reportBlock.MilitarySubCommand = dr["MilitarySubCommand"].ToString();
                    reportBlock.Position = dr["Position"].ToString();
                    reportBlock.MilitaryReportingSpecialty = dr["MilitaryReportingSpecialty"].ToString();
                    reportBlock.MilitaryRank = dr["MilitaryRank"].ToString();
                    reportBlock.Readiness = ReadinessUtil.ReadinessName(DBCommon.GetInt(dr["ReservistReadinessID"]));
                    reportBlock.IdentityNumber = dr["IdentityNumber"].ToString();
                    reportBlock.FullName = dr["FullName"].ToString();
                    reportBlock.PermPlaceName = dr["PermPlaceName"].ToString();

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

        public static int GetReportListReservistsFromCommandBlockCount(ReportListReservistsFromCommandFilter filter, User currentUser)
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
                               FROM PMIS_RES.EquipmentReservistsRequests a
                               INNER JOIN PMIS_RES.RequestsCommands e ON a.EquipmentReservistsRequestID = e.EquipmentReservistsRequestID
                               INNER JOIN PMIS_RES.RequestCommandPositions f ON e.RequestsCommandID = f.RequestsCommandID
                               INNER JOIN PMIS_RES.FillReservistsRequest g ON f.RequestCommandPositionID = g.RequestCommandPositionID
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
