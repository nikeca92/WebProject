using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.HealthSafety.Common
{
    public class WCondIndicator : IDropDownItem
    {
        private int indicatorId;
        private string indicatorName;

        public int IndicatorId
        {
            get { return indicatorId; }
            set { indicatorId = value; }
        }

        public string IndicatorName
        {
            get { return indicatorName; }
            set { indicatorName = value; }
        }

        public bool CanDelete
        {
            get { return true; }
        }

        public WCondIndicator(int indicatorId, string indicatorName)
        {
            this.indicatorId = indicatorId;
            this.indicatorName = indicatorName;
        }

        public WCondIndicator()
        {
        }

        #region DropDownItem Members

        public string Text()
        {
            return IndicatorName;
        }

        public string Value()
        {
            return IndicatorId.ToString();
        }

        #endregion
    }

    public static class WCondIndicatorUtil
    {
        public static WCondIndicator GetIndicator(int indicatorId, User currentUser)
        {
            WCondIndicator indicator = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.IndicatorID as IndicatorID, a.IndicatorName as IndicatorName
                               FROM PMIS_HS.WConditionsIndicators a                       
                               WHERE a.IndicatorID = :IndicatorID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("IndicatorID", OracleType.Number).Value = indicatorId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    indicator = new WCondIndicator();
                    indicator.IndicatorId = indicatorId;
                    indicator.IndicatorName = dr["IndicatorName"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return indicator;
        }

        public static List<WCondIndicator> GetAllIndicatorsByType(int indicatorTypeId, User currentUser)
        {
            List<WCondIndicator> indicators = new List<WCondIndicator>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.IndicatorID as IndicatorID, a.IndicatorName as IndicatorName
                               FROM PMIS_HS.WConditionsIndicators a
                               WHERE a.IndicatorTypeID = : IndicatorTypeID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("IndicatorTypeID", OracleType.Number).Value = indicatorTypeId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["IndicatorID"]))
                        indicators.Add(new WCondIndicator(DBCommon.GetInt(dr["IndicatorID"]), dr["IndicatorName"].ToString()));                  
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return indicators;
        }

        public static int GetAllIndicatorsByTypeCount(int indicatorTypeId, User currentUser)
        {
            int indicatorsCnt = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT COUNT(*) as Cnt
                               FROM PMIS_HS.WConditionsIndicators a
                               WHERE a.IndicatorTypeID = : IndicatorTypeID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("IndicatorTypeID", OracleType.Number).Value = indicatorTypeId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                        indicatorsCnt = DBCommon.GetInt(dr["Cnt"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return indicatorsCnt;
        }

        public static List<WCondIndicator> GetAllIndicatorsByType(int indicatorTypeId, int orderBy, int pageIdx, int rowsPerPage, User currentUser)
        {
            List<WCondIndicator> indicators = new List<WCondIndicator>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {

                string pageWhere = "";

                if (pageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " WHERE RowNumber BETWEEN (" + pageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + pageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

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
                        orderBySQL = "a.IndicatorName";
                        break;                   
                    default:
                        orderBySQL = "a.IndicatorName";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT tmp.IndicatorID as IndicatorID, tmp.IndicatorName as IndicatorName, tmp.RowNumber as RowNumber  FROM (
                                  SELECT a.IndicatorID as IndicatorID, a.IndicatorName as IndicatorName,                                          
                                         RANK() OVER (ORDER BY " + orderBySQL + @", a.IndicatorID) as RowNumber 
                                  FROM PMIS_HS.WConditionsIndicators a                                                     
                                  WHERE a.IndicatorTypeID = :IndicatorTypeID    
                                  ORDER BY " + orderBySQL + @", IndicatorID                             
                               ) tmp
                               " + pageWhere;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("IndicatorTypeID", OracleType.Number).Value = indicatorTypeId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["IndicatorID"]))
                        indicators.Add(new WCondIndicator(DBCommon.GetInt(dr["IndicatorID"]), dr["IndicatorName"].ToString()));
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return indicators;
        }

        public static bool SaveIndicator(int indicatorTypeId, WCondIndicator indicator, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";
                if (indicator.IndicatorId == 0)
                {
                    SQL += @"INSERT INTO PMIS_HS.WConditionsIndicators (IndicatorTypeID, IndicatorName)
                            VALUES (:IndicatorTypeID, :IndicatorName);

                            SELECT PMIS_HS.WCondIndicators_ID_SEQ.currval INTO :IndicatorID FROM dual;

                            ";

                    changeEvent = new ChangeEvent("HS_Lists_Indicators_AddIndicator", WCondIndicatorTypeUtil.GetIndicatorType(indicatorTypeId,currentUser).IndicatorTypeName, null, null, currentUser);
                    
                    changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_Indicators_IndicatorName", "", indicator.IndicatorName, currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_HS.WConditionsIndicators SET
                               IndicatorTypeID = :IndicatorTypeID,
                               IndicatorName = :IndicatorName
                            WHERE IndicatorID = :IndicatorID ;                            

                            ";

                    changeEvent = new ChangeEvent("HS_Lists_Indicators_EditIndicator", WCondIndicatorTypeUtil.GetIndicatorType(indicatorTypeId,currentUser).IndicatorTypeName, null, null, currentUser);

                    WCondIndicator oldIndicator = WCondIndicatorUtil.GetIndicator(indicator.IndicatorId, currentUser);                    

                    if (oldIndicator.IndicatorName.Trim() != indicator.IndicatorName.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_Indicators_IndicatorName", oldIndicator.IndicatorName, indicator.IndicatorName, currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramIndicatorID = new OracleParameter();
                paramIndicatorID.ParameterName = "IndicatorID";
                paramIndicatorID.OracleType = OracleType.Number;

                if (indicator.IndicatorId != 0)
                {
                    paramIndicatorID.Direction = ParameterDirection.Input;
                    paramIndicatorID.Value = indicator.IndicatorId;
                }
                else
                {
                    paramIndicatorID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramIndicatorID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "IndicatorTypeID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = indicatorTypeId;                
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "IndicatorName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(indicator.IndicatorName))
                    param.Value = indicator.IndicatorName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (indicator.IndicatorId == 0)
                {
                    indicator.IndicatorId = DBCommon.GetInt(paramIndicatorID.Value);
                }

                result = true;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                if (changeEvent.ChangeEventDetails.Count > 0)
                    changeEntry.AddEvent(changeEvent);
            }

            return result;
        }

        public static bool DeleteIndicator(int indicatorTypeId, int indicatorId, User currentUser, Change changeEntry)
        {
            bool result = false;

            WCondIndicator oldIndicator = WCondIndicatorUtil.GetIndicator(indicatorId, currentUser);

            ChangeEvent changeEvent = new ChangeEvent("HS_Lists_Indicators_DeleteIndicator", WCondIndicatorTypeUtil.GetIndicatorType(indicatorTypeId,currentUser).IndicatorTypeName, null, null, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("HS_Lists_Indicators_IndicatorName", oldIndicator.IndicatorName, "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @" BEGIN                                                                
                                    DELETE FROM PMIS_HS.WConditionsIndicators WHERE IndicatorID = :IndicatorID;
                                END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("IndicatorID", OracleType.Number).Value = indicatorId;

                result = cmd.ExecuteNonQuery() == 1;
            }
            finally
            {
                conn.Close();
            }

            if (result)
                changeEntry.AddEvent(changeEvent);

            return result;
        }
    }

}