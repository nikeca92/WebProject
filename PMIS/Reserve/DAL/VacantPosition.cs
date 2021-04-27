using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Reserve.Common
{
    public class VacantPosition : BaseDbObject
    {  
        private string position;
        private string militaryReportSpecialitiesHTML;
        private string militaryRanksHTML;
        private int maxVSST_ID;
        private int positionsCnt;

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

        public string MilitaryReportSpecialitiesHTML
        {
            get
            {
                return militaryReportSpecialitiesHTML;
            }
            set
            {
                militaryReportSpecialitiesHTML = value;
            }
        }

        public string MilitaryRanksHTML
        {
            get
            {
                return militaryRanksHTML;
            }
            set
            {
                militaryRanksHTML = value;
            }
        }

        public int MaxVSST_ID
        {
            get
            {
                return maxVSST_ID;
            }
            set
            {
                maxVSST_ID = value;
            }
        }
        
        public int PositionsCnt
        {
            get { return positionsCnt; }
            set { positionsCnt = value; }
        }

        public VacantPosition(User user)
            :base(user)
        {

        }
    }

    public static class VacantPositionUtil
    {
        //This method creates and returns a VacantPosition object. It extracts the data from a DataReader.
        //This is done in this way to have a signle place of creation of this object, however, pass the particular DataReader
        //object from various queries for better performance instead of executing a new call to the DB here to get the details for a specific ID, for example.
        public static VacantPosition ExtractVacantPositionFromDataReader(OracleDataReader dr, User currentUser)
        {
            VacantPosition vacantPosition = new VacantPosition(currentUser);

            vacantPosition.Position = dr["Position"].ToString();
            vacantPosition.MilitaryReportSpecialitiesHTML = dr["MRS_HTML"].ToString();
            vacantPosition.MilitaryRanksHTML = dr["Rank_HTML"].ToString();
            vacantPosition.MaxVSST_ID = DBCommon.GetInt(dr["MAX_VSST_ID"]);
            vacantPosition.PositionsCnt = DBCommon.GetInt(dr["PositionsCnt"]);

            return vacantPosition;
        }

        public static List<VacantPosition> GetAllVacantPositions(string position, RequestCommand requestCommand, int orderBy, int pageIdx, int rowsPerPage, User currentUser)
        {
            List<VacantPosition> vacantPositions = new List<VacantPosition>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                if (!String.IsNullOrEmpty(position))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(a.Position) LIKE UPPER('%" + position.Replace("'", "''") + @"%') ";
                }

                where = (where == "" ? "" : " WHERE ") + where;

                string pageWhere = "";

                if (pageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE tmp.RowNumber BETWEEN (" + pageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + pageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

                string orderBySQL = "";
                string orderByDir = "ASC";

                if (orderBy > 100)
                {
                    orderBy -= 100;
                    orderByDir = "DESC";
                }

                switch (orderBy)
                {
                    case 1:
                        orderBySQL = "a.Position";
                        break;
                    case 2:
                        orderBySQL = "a.MRS_HTML";
                        break;
                    case 3:
                        orderBySQL = " a.Rank_HTML";
                        break;      
                    case 4:
                        orderBySQL = "a.PositionsCnt";
                        break;      
                    default:
                        orderBySQL = "a.PositionsCnt - NVL(pos.PositionsCnt, 0)";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @" SELECT * FROM 
                                (
                                    SELECT a.Position, 
                                           a.MRS_HTML,
                                           a.Rank_HTML,
                                           a.MAX_VSST_ID,
                                           a.PositionsCnt - NVL(pos.PositionsCnt, 0) as PositionsCnt,
                                           RANK() OVER (ORDER BY " + orderBySQL + @", a.Position) as RowNumber
                                    FROM (
                                          SELECT a.Position,
                                                 a.MRS_HTML,
                                                 a.MRSIds,
                                                 a.Rank_HTML,
                                                 a.RankIds,
                                                 MAX(a.VSST_ID) as MAX_VSST_ID,
                                                 COUNT(*) as PositionsCnt                                             
                                          FROM (
                                                SELECT a.VSST_TEXT_DL as Position,
                                                       PMIS_ADM.CommonFunctions.GetMRSPerVSSTRecordHTML(a.VSST_ID) as MRS_HTML,
                                                       PMIS_ADM.CommonFunctions.GetMRSIdsPerVSSTRecord(a.VSST_ID) as MRSIds,
                                                       PMIS_ADM.CommonFunctions.GetRanksPerVSSTRecordHTML(a.VSST_ID) as Rank_HTML,
                                                       PMIS_ADM.CommonFunctions.GetRankIdsPerVSSTRecord(a.VSST_ID) as RankIds,
                                                       a.VSST_ID
                                                FROM VS_OWNER.VS_VSST a
                                                LEFT OUTER JOIN VS_OWNER.VS_VSTR b ON a.VSST_VSTR_ID = b.VSTR_ID
                                                WHERE a.VSST_EGN IS NULL AND b.VSTR_KOD_VVR = " + requestCommand.MilitaryCommand.MilitaryCommandId.ToString() + @"
                                               ) a
                                               " + where + @"                                                   
                                               GROUP BY a.Position, a.MRS_HTML, a.MRSIds, a.Rank_HTML, a.RankIds
                                         ) a
                                    LEFT OUTER JOIN (
                                                      SELECT a.Position, 
                                                             PMIS_RES.RESFunctions.GetRankIdsPerReqCmdPosition(a.RequestCommandPositionID) as RankIds,
                                                             PMIS_RES.RESFunctions.GetMRSIdsPerReqCmdPosition(a.RequestCommandPositionID) as MRSIds,
                                                             SUM(a.ReservistsCount) as PositionsCnt
                                                      FROM PMIS_RES.RequestCommandPositions a
                                                      WHERE a.PositionType = 2
                                                      GROUP BY a.Position, PMIS_RES.RESFunctions.GetRankIdsPerReqCmdPosition(a.RequestCommandPositionID), PMIS_RES.RESFunctions.GetMRSIdsPerReqCmdPosition(a.RequestCommandPositionID)
                                                    ) pos ON pos.Position = a.Position AND NVL(pos.RankIds, 'x') = NVL(a.RankIds, 'x') AND NVL(pos.MRSIds, 'x') = NVL(a.MRSIds, 'x')
                                    WHERE a.PositionsCnt - NVL(pos.PositionsCnt, 0) > 0
                                    ORDER BY " + orderBySQL + @", a.Position
                               ) tmp 
                                " + pageWhere;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    vacantPositions.Add(ExtractVacantPositionFromDataReader(dr, currentUser));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vacantPositions;
        }

        public static int GetAllVacantPositionsCount(string position, RequestCommand requestCommand, User currentUser)
        {
            int vacantPositionsCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                if (!String.IsNullOrEmpty(position))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(a.Position) LIKE UPPER('%" + position.Replace("'", "''") + @"%') ";
                }

                where = (where == "" ? "" : " WHERE ") + where;

                string SQL = @" SELECT COUNT(*) as Cnt FROM 
                                (
                                    SELECT a.Position, 
                                           a.MRS_HTML,
                                           a.Rank_HTML,
                                           a.MAX_VSST_ID,
                                           a.PositionsCnt - NVL(pos.PositionsCnt, 0) as PositionsCnt
                                    FROM (
                                          SELECT a.Position,
                                                 a.MRS_HTML,
                                                 a.MRSIds,
                                                 a.Rank_HTML,
                                                 a.RankIds,
                                                 MAX(a.VSST_ID) as MAX_VSST_ID,
                                                 COUNT(*) as PositionsCnt                                             
                                          FROM (
                                                SELECT a.VSST_TEXT_DL as Position,
                                                       PMIS_ADM.CommonFunctions.GetMRSPerVSSTRecordHTML(a.VSST_ID) as MRS_HTML,
                                                       PMIS_ADM.CommonFunctions.GetMRSIdsPerVSSTRecord(a.VSST_ID) as MRSIds,
                                                       PMIS_ADM.CommonFunctions.GetRanksPerVSSTRecordHTML(a.VSST_ID) as Rank_HTML,
                                                       PMIS_ADM.CommonFunctions.GetRankIdsPerVSSTRecord(a.VSST_ID) as RankIds,
                                                       a.VSST_ID
                                                FROM VS_OWNER.VS_VSST a
                                                LEFT OUTER JOIN VS_OWNER.VS_VSTR b ON a.VSST_VSTR_ID = b.VSTR_ID
                                                WHERE a.VSST_EGN IS NULL AND b.VSTR_KOD_VVR = " + requestCommand.MilitaryCommand.MilitaryCommandId.ToString() + @"
                                               ) a
                                               " + where + @"                                                   
                                               GROUP BY a.Position, a.MRS_HTML, a.MRSIds, a.Rank_HTML, a.RankIds
                                         ) a
                                    LEFT OUTER JOIN (
                                                      SELECT a.Position, 
                                                             PMIS_RES.RESFunctions.GetRankIdsPerReqCmdPosition(a.RequestCommandPositionID) as RankIds,
                                                             PMIS_RES.RESFunctions.GetMRSIdsPerReqCmdPosition(a.RequestCommandPositionID) as MRSIds,
                                                             SUM(a.ReservistsCount) as PositionsCnt
                                                      FROM PMIS_RES.RequestCommandPositions a
                                                      WHERE a.PositionType = 2
                                                      GROUP BY a.Position, PMIS_RES.RESFunctions.GetRankIdsPerReqCmdPosition(a.RequestCommandPositionID), PMIS_RES.RESFunctions.GetMRSIdsPerReqCmdPosition(a.RequestCommandPositionID)
                                                    ) pos ON pos.Position = a.Position AND NVL(pos.RankIds, 'x') = NVL(a.RankIds, 'x') AND NVL(pos.MRSIds, 'x') = NVL(a.MRSIds, 'x')
                                    WHERE a.PositionsCnt - NVL(pos.PositionsCnt, 0) > 0
                               ) tmp 
                                ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    vacantPositionsCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vacantPositionsCnt;
        }
    }
}