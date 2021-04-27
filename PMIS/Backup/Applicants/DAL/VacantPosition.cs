using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.Applicants.Common
{
    public class VacantPosition : BaseDbObject
    {  
        private int? militaryUnitID;       
        private MilitaryUnit militaryUnit;
        private string positionName;        
        private string positionCode;        
        private string clInformationAccLevelNATO;
        private string clInformationAccLevelBG;        
        private string clInformationAccLevelEU;        
        private int positionsCnt;
        
        public int? MilitaryUnitID
        {
            get { return militaryUnitID; }
            set { militaryUnitID = value; }
        }

        public MilitaryUnit MilitaryUnit
        {
            get 
            {
                if (militaryUnit == null && militaryUnitID.HasValue)
                    militaryUnit = MilitaryUnitUtil.GetMilitaryUnit(militaryUnitID.Value, CurrentUser);
                return militaryUnit; 
            }
            set { militaryUnit = value; }
        }

        public string PositionName
        {
            get { return positionName; }
            set { positionName = value; }
        }

        public string PositionCode
        {
            get { return positionCode; }
            set { positionCode = value; }
        }

        public string ClInformationAccLevelNATO
        {
            get { return clInformationAccLevelNATO; }
            set { clInformationAccLevelNATO = value; }
        }

        public string ClInformationAccLevelBG
        {
            get { return clInformationAccLevelBG; }
            set { clInformationAccLevelBG = value; }
        }

        public string ClInformationAccLevelEU
        {
            get { return clInformationAccLevelEU; }
            set { clInformationAccLevelEU = value; }
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
        //object from various queries for better performance instead of executing a new call to the DB here to get the details for a specific VacantPositionID, for example.
        public static VacantPosition ExtractVacantPositionFromDataReader(OracleDataReader dr, User currentUser)
        {
            VacantPosition vacantPosition = new VacantPosition(currentUser);

            vacantPosition.MilitaryUnitID = (DBCommon.IsInt(dr["MilitaryUnitID"]) ? (int?)DBCommon.GetInt(dr["MilitaryUnitID"]) : null);
            vacantPosition.PositionName = dr["PositionName"].ToString();
            vacantPosition.PositionCode = dr["PositionCode"].ToString();
            vacantPosition.ClInformationAccLevelNATO = dr["ClInformationAccLevelNATO"].ToString();
            vacantPosition.ClInformationAccLevelBG = dr["ClInformationAccLevelBG"].ToString();
            vacantPosition.ClInformationAccLevelEU = dr["ClInformationAccLevelEU"].ToString();
            vacantPosition.PositionsCnt = DBCommon.GetInt(dr["PositionsCnt"]);

            return vacantPosition;
        }

        public static VacantPosition GetVacantPosition(int militaryUnitID, string positionCode,
            string positionName, string ClInformationAccLevelNATO, string ClInformationAccLevelBG, string ClInformationAccLevelEU, 
            User currentUser)
        {
            VacantPosition vacantPosition = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" SELECT  a.MilitaryUnitID,
                                        a.MilitaryUnitName, 
                                        a.PositionName, 
                                        a.PositionCode, 
                                        a.ClInformationAccLevelNATO,
                                        a.ClInformationAccLevelBG,
                                        a.ClInformationAccLevelEU,
                                        COUNT(*) as PositionsCnt                                
                                FROM
                                    (SELECT b.MSTR_KOD_MIR as MilitaryUnitID,
                                            c.IMEES as MilitaryUnitName,
                                            a.MSST_TEXT_DL || ' в ' || vs_owner.vs_spr.GET_OES(a.MSST_MSTR_ID) as PositionName,
                                            CASE a.MSST_KATKOD
                                                WHEN '60' THEN TRIM(MSST_DLOKOD)
                                                WHEN '73' THEN TRIM(MSST_DLOKOD)
                                                WHEN '20' THEN TRIM(MSST_DLSKOD)
                                                WHEN '50' THEN TRIM(MSST_DLSKOD)
                                                WHEN '55' THEN TRIM(MSST_DLSKOD)
                                            END as PositionCode,
                                            d.RV_MEANING as ClInformationAccLevelNATO,
                                            e.RV_MEANING as ClInformationAccLevelBG,
                                            f.RV_MEANING as ClInformationAccLevelEU,
                                            a.MSST_IDKI_NATO as ClInformationAccLevelNATO_ID,
                                            a.MSST_IDKI as ClInformationAccLevelBG_ID,
                                            a.MSST_IDKI_EU as ClInformationAccLevelEU_ID
                                     FROM VS_OWNER.VS_MSST a
                                     LEFT OUTER JOIN VS_OWNER.VS_MSTR b ON a.MSST_MSTR_ID  = b.MSTR_ID
                                     LEFT OUTER JOIN UKAZ_OWNER.MIR c ON b.MSTR_KOD_MIR = c.KOD_MIR
                                     LEFT OUTER JOIN VS_OWNER.CG_REF_CODES d ON a.MSST_IDKI_NATO = d.RV_LOW_VALUE AND d.RV_DOMAIN = 'NIVO_KL_INF_NATO'
                                     LEFT OUTER JOIN VS_OWNER.CG_REF_CODES e ON a.MSST_IDKI = e.RV_LOW_VALUE AND e.RV_DOMAIN = 'NIVO_KL_INF'
                                     LEFT OUTER JOIN VS_OWNER.CG_REF_CODES f ON a.MSST_IDKI_EU = f.RV_LOW_VALUE AND f.RV_DOMAIN = 'NIVO_KL_INF_EU'
                                     WHERE a.MSST_EGN IS NULL AND
                                           a.MSST_KATKOD IN (20, 50, 55, 60, 73)
                                    ) a
                                    WHERE a.MilitaryUnitID = :MilitaryUnitID AND
                                          NVL(a.PositionCode, '-1') = NVL(:PositionCode, '-1') AND
                                          NVL(a.PositionName, '-1') = NVL(:PositionName, '-1') AND
                                          NVL(a.ClInformationAccLevelNATO_ID, '-1') = NVL(:ClInformationAccLevelNATO, '-1') AND
                                          NVL(a.ClInformationAccLevelBG_ID, '-1') = NVL(:ClInformationAccLevelBG, '-1') AND
                                          NVL(a.ClInformationAccLevelEU_ID, '-1') = NVL(:ClInformationAccLevelEU, '-1') ";
                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    SQL += @" AND (a.MilitaryUnitID IS NULL OR a.MilitaryUnitID IN (" + currentUser.MilitaryUnitIDs + @"))
                            ";

                SQL +=  @"
                          GROUP BY a.MilitaryUnitID,
                                   a.MilitaryUnitName, 
                                   a.PositionName, 
                                   a.PositionCode, 
                                   a.ClInformationAccLevelNATO,
                                   a.ClInformationAccLevelBG,
                                   a.ClInformationAccLevelEU
                         ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MilitaryUnitID", OracleType.Number).Value = militaryUnitID;
                cmd.Parameters.Add("PositionCode", OracleType.VarChar).Value = positionCode ;
                cmd.Parameters.Add("PositionName", OracleType.VarChar).Value = positionName;
                cmd.Parameters.Add("ClInformationAccLevelNATO", OracleType.VarChar).Value = ClInformationAccLevelNATO;
                cmd.Parameters.Add("ClInformationAccLevelBG", OracleType.VarChar).Value = ClInformationAccLevelBG;
                cmd.Parameters.Add("ClInformationAccLevelEU", OracleType.VarChar).Value = ClInformationAccLevelEU;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    vacantPosition = ExtractVacantPositionFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return vacantPosition;
        }

        public static List<VacantPosition> GetAllVacantPositions(string militaryUnitIDs, string positionName, int vacancyAnnounceTypeId , int orderBy, int pageIdx, int rowsPerPage, User currentUser)
        {
            List<VacantPosition> vacantPositions = new List<VacantPosition>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                if (!String.IsNullOrEmpty(positionName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(a.PositionName) LIKE UPPER('%" + positionName.Replace("'", "''") + @"%') ";
                }

                if (!String.IsNullOrEmpty(militaryUnitIDs))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MilitaryUnitID IN (" + militaryUnitIDs + @")";
                }                              

                where = (where == "" ? "" : " WHERE ") + where;

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    if (where == "")
                    {
                        where += @" WHERE (NVL(a.MilitaryUnitID, -1) IN (" + currentUser.MilitaryUnitIDs + @" UNION SELECT -1 FROM Dual))
                            ";
                    }
                    else
                    {
                        where += @" AND (NVL(a.MilitaryUnitID, -1) IN (" + currentUser.MilitaryUnitIDs + @" UNION SELECT -1 FROM Dual))
                            ";
                    }

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
                        orderBySQL = "a.MilitaryUnitName";
                        break;
                    case 2:
                        orderBySQL = "a.PositionName";
                        break;
                    case 3:
                        orderBySQL = "a.PositionCode";
                        break;
                    case 4:
                        orderBySQL = "a.PositionsCnt - NVL(pos.PositionsCnt, 0)";
                        break;      
                    default:
                        orderBySQL = "a.MilitaryUnitName";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @" SELECT * FROM (
                                SELECT a.MilitaryUnitID,
                                       a.MilitaryUnitName, 
                                       a.PositionName, 
                                       a.PositionCode, 
                                       a.ClInformationAccLevelNATO,
                                       a.ClInformationAccLevelBG,
                                       a.ClInformationAccLevelEU,
                                       a.PositionsCnt - NVL(pos.PositionsCnt, 0) as PositionsCnt,
                                       RANK() OVER (ORDER BY " + orderBySQL + @", a.MilitaryUnitID, a.PositionCode, a.PositionName, a.ClInformationAccLevelNATO, a.ClInformationAccLevelBG, a.ClInformationAccLevelEU) as RowNumber
                                FROM (
                                      SELECT a.MilitaryUnitID,
                                             a.MilitaryUnitName, 
                                             a.PositionName, 
                                             a.PositionCode, 
                                             a.ClInformationAccLevelNATO,
                                             a.ClInformationAccLevelBG,
                                             a.ClInformationAccLevelEU,
                                             a.ClInformationAccLevelNATO_NAME,
                                             a.ClInformationAccLevelBG_NAME,
                                             a.ClInformationAccLevelEU_NAME,
                                             COUNT(*) as PositionsCnt                                             
                                      FROM (
                                            SELECT b.MSTR_KOD_MIR as MilitaryUnitID,
                                                   c.VPN || ' ' || c.IMEES as MilitaryUnitName,
                                                   a.MSST_TEXT_DL || ' в ' || vs_owner.vs_spr.GET_OES(a.MSST_MSTR_ID) as PositionName,
                                                   CASE a.MSST_KATKOD
                                                        WHEN '60' THEN TRIM(MSST_DLOKOD)
                                                        WHEN '73' THEN TRIM(MSST_DLOKOD)
                                                        WHEN '20' THEN TRIM(MSST_DLSKOD)
                                                        WHEN '50' THEN TRIM(MSST_DLSKOD)
                                                        WHEN '55' THEN TRIM(MSST_DLSKOD)
                                                   END as PositionCode,
                                                   a.MSST_IDKI_NATO as ClInformationAccLevelNATO,
                                                   a.MSST_IDKI as ClInformationAccLevelBG,
                                                   a.MSST_IDKI_EU as ClInformationAccLevelEU,
                                                   d.RV_MEANING as ClInformationAccLevelNATO_NAME,
                                                   e.RV_MEANING as ClInformationAccLevelBG_NAME,
                                                   f.RV_MEANING as ClInformationAccLevelEU_NAME
                                            FROM VS_OWNER.VS_MSST a
                                            LEFT OUTER JOIN VS_OWNER.VS_MSTR b ON a.MSST_MSTR_ID  = b.MSTR_ID
                                            LEFT OUTER JOIN UKAZ_OWNER.MIR c ON b.MSTR_KOD_MIR = c.KOD_MIR
                                            LEFT OUTER JOIN VS_OWNER.CG_REF_CODES d ON a.MSST_IDKI_NATO = d.RV_LOW_VALUE AND d.RV_DOMAIN = 'NIVO_KL_INF_NATO'
                                            LEFT OUTER JOIN VS_OWNER.CG_REF_CODES e ON a.MSST_IDKI = e.RV_LOW_VALUE AND e.RV_DOMAIN = 'NIVO_KL_INF'
                                            LEFT OUTER JOIN VS_OWNER.CG_REF_CODES f ON a.MSST_IDKI_EU = f.RV_LOW_VALUE AND f.RV_DOMAIN = 'NIVO_KL_INF_EU'
                                            WHERE a.MSST_EGN IS NULL AND
                                                  a.MSST_KATKOD IN (20, 50, 55, 60, 73)
                                           ) a
                                           " + where + @"                                                   
                                           GROUP BY a.MilitaryUnitID,
                                                    a.MilitaryUnitName, 
                                                    a.PositionName, 
                                                    a.PositionCode, 
                                                    a.ClInformationAccLevelNATO,
                                                    a.ClInformationAccLevelBG,
                                                    a.ClInformationAccLevelEU,
                                                    a.ClInformationAccLevelNATO_NAME,
                                                    a.ClInformationAccLevelBG_NAME,
                                                    a.ClInformationAccLevelEU_NAME
                                     ) a
                                LEFT OUTER JOIN (
                                                  SELECT a.MilitaryUnitID, 
                                                         a.PositionCode, 
                                                         a.PositionName,
                                                         a.ClInformationAccLevelNATO,
                                                         a.ClInformationAccLevelBG,
                                                         a.ClInformationAccLevelEU,
                                                         SUM(a.PositionsCnt) as PositionsCnt
                                                  FROM PMIS_APPL.VacancyAnnouncePositions a
                                                  GROUP BY a.MilitaryUnitID, 
                                                           a.PositionCode,
                                                           a.PositionName,
                                                           a.ClInformationAccLevelNATO,
                                                           a.ClInformationAccLevelBG,
                                                           a.ClInformationAccLevelEU
                                                ) pos ON pos.MilitaryUnitID = a.MilitaryUnitID AND 
                                                         NVL(pos.PositionCode, '-1') = NVL(a.PositionCode, '-1') AND
                                                         NVL(pos.PositionName, '-1') = NVL(a.PositionName, '-1') AND
                                                         NVL(pos.ClInformationAccLevelNATO, '-1') = NVL(a.ClInformationAccLevelNATO_NAME, '-1') AND
                                                         NVL(pos.ClInformationAccLevelBG, '-1') = NVL(a.ClInformationAccLevelBG_NAME, '-1') AND
                                                         NVL(pos.ClInformationAccLevelEU, '-1') = NVL(a.ClInformationAccLevelEU_NAME, '-1')
                                WHERE a.PositionsCnt - NVL(pos.PositionsCnt, 0) > 0 AND
                                      PMIS_ADM.CommonFunctions.IsMilitaryUnitActual(a.MilitaryUnitID) = 1
                                ORDER BY " + orderBySQL + @", a.MilitaryUnitID, a.PositionCode
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

        public static int GetAllVacantPositionsCount(string militaryUnitIDs, string positionName, User currentUser)
        {
            int vacantPositionsCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string where = "";

                if (!String.IsNullOrEmpty(positionName))
                {
                    where += (where == "" ? "" : " AND ") +
                             " UPPER(a.PositionName) LIKE UPPER('%" + positionName.Replace("'", "''") + @"%') ";
                }

                if (!String.IsNullOrEmpty(militaryUnitIDs))
                {
                    where += (where == "" ? "" : " AND ") +
                             " a.MilitaryUnitID IN (" + militaryUnitIDs + @")";
                }

                where = (where == "" ? "" : " WHERE ") + where;

                if (!string.IsNullOrEmpty(currentUser.MilitaryUnitIDs))
                    if (where == "")
                    {
                        where += @" WHERE (NVL(a.MilitaryUnitID, -1) IN (" + currentUser.MilitaryUnitIDs + @" UNION SELECT -1 FROM Dual))
                            ";
                    }
                    else
                    {
                        where += @" AND (NVL(a.MilitaryUnitID, -1) IN (" + currentUser.MilitaryUnitIDs + @" UNION SELECT -1 FROM Dual))
                            ";
                    }

                string SQL = @" SELECT COUNT(*) as Cnt FROM (
                                SELECT a.MilitaryUnitID,
                                       a.MilitaryUnitName, 
                                       a.PositionName, 
                                       a.PositionCode, 
                                       a.ClInformationAccLevelNATO,
                                       a.ClInformationAccLevelBG,
                                       a.ClInformationAccLevelEU,
                                       a.PositionsCnt - NVL(pos.PositionsCnt, 0) as PositionsCnt
                                FROM (
                                      SELECT a.MilitaryUnitID,
                                             a.MilitaryUnitName, 
                                             a.PositionName, 
                                             a.PositionCode, 
                                             a.ClInformationAccLevelNATO,
                                             a.ClInformationAccLevelBG,
                                             a.ClInformationAccLevelEU,
                                             a.ClInformationAccLevelNATO_NAME,
                                             a.ClInformationAccLevelBG_NAME,
                                             a.ClInformationAccLevelEU_NAME,
                                             COUNT(*) as PositionsCnt                                             
                                      FROM (
                                            SELECT b.MSTR_KOD_MIR as MilitaryUnitID,
                                                   c.VPN || ' ' || c.IMEES as MilitaryUnitName,
                                                   a.MSST_TEXT_DL || ' в ' || vs_owner.vs_spr.GET_OES(a.MSST_MSTR_ID) as PositionName,
                                                   CASE a.MSST_KATKOD
                                                        WHEN '60' THEN TRIM(MSST_DLOKOD)
                                                        WHEN '73' THEN TRIM(MSST_DLOKOD)
                                                        WHEN '20' THEN TRIM(MSST_DLSKOD)
                                                        WHEN '50' THEN TRIM(MSST_DLSKOD)
                                                        WHEN '55' THEN TRIM(MSST_DLSKOD)
                                                   END as PositionCode,
                                                   a.MSST_IDKI_NATO as ClInformationAccLevelNATO,
                                                   a.MSST_IDKI as ClInformationAccLevelBG,
                                                   a.MSST_IDKI_EU as ClInformationAccLevelEU,
                                                   d.RV_MEANING as ClInformationAccLevelNATO_NAME,
                                                   e.RV_MEANING as ClInformationAccLevelBG_NAME,
                                                   f.RV_MEANING as ClInformationAccLevelEU_NAME
                                            FROM VS_OWNER.VS_MSST a
                                            LEFT OUTER JOIN VS_OWNER.VS_MSTR b ON a.MSST_MSTR_ID  = b.MSTR_ID
                                            LEFT OUTER JOIN UKAZ_OWNER.MIR c ON b.MSTR_KOD_MIR = c.KOD_MIR
                                            LEFT OUTER JOIN VS_OWNER.CG_REF_CODES d ON a.MSST_IDKI_NATO = d.RV_LOW_VALUE AND d.RV_DOMAIN = 'NIVO_KL_INF_NATO'
                                            LEFT OUTER JOIN VS_OWNER.CG_REF_CODES e ON a.MSST_IDKI = e.RV_LOW_VALUE AND e.RV_DOMAIN = 'NIVO_KL_INF'
                                            LEFT OUTER JOIN VS_OWNER.CG_REF_CODES f ON a.MSST_IDKI_EU = f.RV_LOW_VALUE AND f.RV_DOMAIN = 'NIVO_KL_INF_EU'
                                            WHERE a.MSST_EGN IS NULL AND
                                                  a.MSST_KATKOD IN (20, 50, 55, 60, 73)
                                           ) a
                                           " + where + @"                                                   
                                           GROUP BY a.MilitaryUnitID,
                                                    a.MilitaryUnitName, 
                                                    a.PositionName, 
                                                    a.PositionCode, 
                                                    a.ClInformationAccLevelNATO,
                                                    a.ClInformationAccLevelBG,
                                                    a.ClInformationAccLevelEU,
                                                    a.ClInformationAccLevelNATO_NAME,
                                                    a.ClInformationAccLevelBG_NAME,
                                                    a.ClInformationAccLevelEU_NAME
                                     ) a
                                LEFT OUTER JOIN (
                                                  SELECT a.MilitaryUnitID, 
                                                         a.PositionCode, 
                                                         a.PositionName,
                                                         a.ClInformationAccLevelNATO,
                                                         a.ClInformationAccLevelBG,
                                                         a.ClInformationAccLevelEU,
                                                         SUM(a.PositionsCnt) as PositionsCnt
                                                  FROM PMIS_APPL.VacancyAnnouncePositions a
                                                  GROUP BY a.MilitaryUnitID, 
                                                           a.PositionCode,
                                                           a.PositionName,
                                                           a.ClInformationAccLevelNATO,
                                                           a.ClInformationAccLevelBG,
                                                           a.ClInformationAccLevelEU
                                                ) pos ON pos.MilitaryUnitID = a.MilitaryUnitID AND 
                                                         NVL(pos.PositionCode, '-1') = NVL(a.PositionCode, '-1') AND
                                                         NVL(pos.PositionName, '-1') = NVL(a.PositionName, '-1') AND
                                                         NVL(pos.ClInformationAccLevelNATO, '-1') = NVL(a.ClInformationAccLevelNATO_NAME, '-1') AND
                                                         NVL(pos.ClInformationAccLevelBG, '-1') = NVL(a.ClInformationAccLevelBG_NAME, '-1') AND
                                                         NVL(pos.ClInformationAccLevelEU, '-1') = NVL(a.ClInformationAccLevelEU_NAME, '-1')
                                WHERE a.PositionsCnt - NVL(pos.PositionsCnt, 0) > 0 AND
                                      PMIS_ADM.CommonFunctions.IsMilitaryUnitActual(a.MilitaryUnitID) = 1
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