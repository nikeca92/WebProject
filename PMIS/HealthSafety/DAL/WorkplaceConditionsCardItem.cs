using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using PMIS.Common;

namespace PMIS.HealthSafety.Common
{
    public class WorkplaceConditionsCardItem
    {
        private int workplaceConditionsCardItemId;        
        private int indicatorTypeId;        
        private string indicatorTypeName;        
        private int? indicatorId;
        private string indicatorName;        
        private decimal? value;        
        private decimal? rate;        
        private decimal? assessment;

        public int WorkplaceConditionsCardItemId
        {
            get { return workplaceConditionsCardItemId; }
            set { workplaceConditionsCardItemId = value; }
        }

        public int IndicatorTypeId
        {
            get { return indicatorTypeId; }
            set { indicatorTypeId = value; }
        }

        public string IndicatorTypeName
        {
            get { return indicatorTypeName; }
            set { indicatorTypeName = value; }
        }

        public int? IndicatorId
        {
            get { return indicatorId; }
            set { indicatorId = value; }
        }

        public string IndicatorName
        {
            get { return indicatorName; }
            set { indicatorName = value; }
        }

        public decimal? Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public decimal? Rate
        {
            get { return rate; }
            set { rate = value; }
        }

        public decimal? Assessment
        {
            get { return assessment; }
            set { assessment = value; }
        }

        public string Caption
        {
            get
            {
                if (IndicatorId.HasValue)
                    return IndicatorName;
                else
                    return IndicatorTypeName;
            }
        }

        public bool CanDelete
        {
            get { return true; }
        }

        public WorkplaceConditionsCardItem()
        {
        }  
    }

    public static class WorkplaceConditionsCardItemUtil
    {
        public static WorkplaceConditionsCardItem GetWorkplaceConditionsCardItem(int workplaceConditionsCardItemId, User currentUser)
        {
            WorkplaceConditionsCardItem cardItem = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.IndicatorTypeID as IndicatorTypeID, a.IndicatorTypeName as IndicatorTypeName,
                                      a.IndicatorID as IndicatorID, a.IndicatorName as IndicatorName,
                                      a.Value as Value, a.Rate as Rate, a.Assessment as Assessment
                               FROM PMIS_HS.WorkplaceConditionsCardItems a                       
                               WHERE a.WorkplaceConditionsCardItemID = :WorkplaceConditionsCardItemID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("WorkplaceConditionsCardItemID", OracleType.Number).Value = workplaceConditionsCardItemId;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    cardItem = new WorkplaceConditionsCardItem();
                    cardItem.WorkplaceConditionsCardItemId = workplaceConditionsCardItemId;
                    cardItem.IndicatorTypeId = DBCommon.GetInt(dr["IndicatorTypeID"]);
                    cardItem.IndicatorId = (DBCommon.IsInt(dr["IndicatorID"]) ? (int?)DBCommon.GetInt(dr["IndicatorID"]) : null);
                    cardItem.IndicatorTypeName = dr["IndicatorTypeName"].ToString();
                    cardItem.IndicatorName = dr["IndicatorName"].ToString();
                    cardItem.Value = (dr["Value"] is decimal ? (decimal?)dr["Value"] : null);
                    cardItem.Rate = (dr["Rate"] is decimal ? (decimal?)dr["Rate"] : null);
                    cardItem.Assessment = (dr["Assessment"] is decimal ? (decimal?)dr["Assessment"] : null);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return cardItem;
        }

        public static List<WorkplaceConditionsCardItem> GetAllWorkplaceConditionsCardItemsByCard(int workplaceConditionsCardId, User currentUser)
        {
            List<WorkplaceConditionsCardItem> cardItems = new List<WorkplaceConditionsCardItem>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.WorkplaceConditionsCardItemID as WorkplaceConditionsCardItemID, a.IndicatorTypeID as IndicatorTypeID, 
                                      a.IndicatorID as IndicatorID, a.IndicatorTypeName as IndicatorTypeName, a.IndicatorName as IndicatorName,
                                      a.Value as Value, a.Rate as Rate, a.Assessment as Assessment
                               FROM PMIS_HS.WorkplaceConditionsCardItems a                       
                               WHERE a.WorkplaceConditionsCardID = :WorkplaceConditionsCardID AND a.IndicatorID IS NULL";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("WorkplaceConditionsCardID", OracleType.Number).Value = workplaceConditionsCardId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["WorkplaceConditionsCardItemID"]))
                    {
                        WorkplaceConditionsCardItem cardItem = new WorkplaceConditionsCardItem();
                        cardItem.WorkplaceConditionsCardItemId = DBCommon.GetInt(dr["WorkplaceConditionsCardItemID"]);
                        cardItem.IndicatorTypeId = DBCommon.GetInt(dr["IndicatorTypeID"]);
                        cardItem.IndicatorId = (DBCommon.IsInt(dr["IndicatorID"]) ? (int?)DBCommon.GetInt(dr["IndicatorID"]) : null);
                        cardItem.IndicatorTypeName = dr["IndicatorTypeName"].ToString();
                        cardItem.IndicatorName = dr["IndicatorName"].ToString();
                        cardItem.Value = (dr["Value"] is decimal ? (decimal?)dr["Value"] : null);
                        cardItem.Rate = (dr["Rate"] is decimal ? (decimal?)dr["Rate"] : null);
                        cardItem.Assessment = (dr["Assessment"] is decimal ? (decimal?)dr["Assessment"] : null);
                        cardItems.Add(cardItem);
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return cardItems;
        }

        public static List<WorkplaceConditionsCardItem> GetAllWorkplaceConditionsCardItemsByIndicatorType(int workplaceConditionsCardId, int indicatorTypeId, User currentUser)
        {
            List<WorkplaceConditionsCardItem> cardItems = new List<WorkplaceConditionsCardItem>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.WorkplaceConditionsCardItemID as WorkplaceConditionsCardItemID, a.IndicatorTypeID as IndicatorTypeID, 
                                      a.IndicatorID as IndicatorID, a.IndicatorTypeName as IndicatorTypeName, a.IndicatorName as IndicatorName,
                                      a.Value as Value, a.Rate as Rate, a.Assessment as Assessment
                               FROM PMIS_HS.WorkplaceConditionsCardItems a                       
                               WHERE a.WorkplaceConditionsCardID = :WorkplaceConditionsCardID AND a.IndicatorTypeID = :IndicatorTypeID AND a.IndicatorID IS NOT NULL";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("WorkplaceConditionsCardID", OracleType.Number).Value = workplaceConditionsCardId;
                cmd.Parameters.Add("IndicatorTypeID", OracleType.Number).Value = indicatorTypeId;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (DBCommon.IsInt(dr["WorkplaceConditionsCardItemID"]))
                    {
                        WorkplaceConditionsCardItem cardItem = new WorkplaceConditionsCardItem();
                        cardItem.WorkplaceConditionsCardItemId = DBCommon.GetInt(dr["WorkplaceConditionsCardItemID"]);
                        cardItem.IndicatorTypeId = DBCommon.GetInt(dr["IndicatorTypeID"]);
                        cardItem.IndicatorId = (DBCommon.IsInt(dr["IndicatorID"]) ? (int?)DBCommon.GetInt(dr["IndicatorID"]) : null);
                        cardItem.IndicatorTypeName = dr["IndicatorTypeName"].ToString();
                        cardItem.IndicatorName = dr["IndicatorName"].ToString();
                        cardItem.Value = (dr["Value"] is decimal ? (decimal?)dr["Value"] : null);
                        cardItem.Rate = (dr["Rate"] is decimal ? (decimal?)dr["Rate"] : null);
                        cardItem.Assessment = (dr["Assessment"] is decimal ? (decimal?)dr["Assessment"] : null);
                        cardItems.Add(cardItem);
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return cardItems;
        }

        public static List<WorkplaceConditionsCardItem> GetDefaultWorkplaceConditionsCardItems(User currentUser)
        {
            List<WorkplaceConditionsCardItem> cardItems = new List<WorkplaceConditionsCardItem>();

            List<WCondIndicatorType> indicatorTypes = WCondIndicatorTypeUtil.GetAllIndicatorTypes(currentUser);

            foreach (WCondIndicatorType indicatorType in indicatorTypes)
            {
                WorkplaceConditionsCardItem cardItem = new WorkplaceConditionsCardItem();
                cardItem.IndicatorTypeId = indicatorType.IndicatorTypeId;
                cardItem.IndicatorTypeName = indicatorType.IndicatorTypeName;
                cardItem.IndicatorId = null;
                cardItem.IndicatorName = null;
                cardItem.Value = null;
                cardItem.Rate = null;
                cardItem.Assessment = null;

                cardItems.Add(cardItem);
            }

            return cardItems;
        }

        public static bool SaveWorkplaceConditionsCardItem(int workplaceConditionsCardId, WorkplaceConditionsCardItem item, User currentUser, Change changeEntry)
        {
            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            WorkplaceConditionsCard card = WorkplaceConditionsCardUtil.GetWorkplaceConditionsCard(workplaceConditionsCardId, currentUser);

            string logDescription = "";
            logDescription += "Номер на карта: " + card.CardNumber;
            logDescription += "<br/>Елемент: " + item.IndicatorTypeName;
            logDescription += "<br/>Показател: " + (string.IsNullOrEmpty(item.IndicatorName) ? "" : item.IndicatorName);

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"BEGIN
                        
                       ";
                if (item.WorkplaceConditionsCardItemId == 0)
                {
                    SQL += @"INSERT INTO PMIS_HS.WorkplaceConditionsCardItems (WorkplaceConditionsCardID, IndicatorTypeID, IndicatorID, IndicatorTypeName, 
                                                                               IndicatorName, Value, Rate, Assessment)
                            VALUES (:WorkplaceConditionsCardID, :IndicatorTypeID, :IndicatorID, :IndicatorTypeName, 
                                    :IndicatorName, :Value, :Rate, :Assessment);

                            SELECT PMIS_HS.WConditionsCardItems_ID_SEQ.currval INTO :WorkplaceConditionsCardItemID FROM dual;

                            ";

                    changeEvent = new ChangeEvent("HS_WCondCard_AddItem", logDescription, card.MilitaryUnit, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail("HS_WCondCardItem_IndicatorTypeName", "", item.IndicatorTypeName, currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_WCondCardItem_IndicatorName", "", (string.IsNullOrEmpty(item.IndicatorName) ? "" : item.IndicatorName) , currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_WCondCardItem_Value", "", (item.Value.HasValue ? CommonFunctions.FormatDecimal(item.Value.Value) : ""), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_WCondCardItem_Rate", "", (item.Rate.HasValue ? CommonFunctions.FormatDecimal(item.Rate.Value) : ""), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail("HS_WCondCardItem_Assessment", "", (item.Assessment.HasValue ? CommonFunctions.FormatDecimal(item.Assessment.Value) : ""), currentUser));
                }
                else
                {
                    SQL += @"UPDATE PMIS_HS.WorkplaceConditionsCardItems SET
                               WorkplaceConditionsCardID = :WorkplaceConditionsCardID,
                               IndicatorTypeID = :IndicatorTypeID,
                               IndicatorID = :IndicatorID,
                               IndicatorTypeName = :IndicatorTypeName,
                               IndicatorName = :IndicatorName,
                               Value = :Value,
                               Rate = :Rate,
                               Assessment = :Assessment
                            WHERE WorkplaceConditionsCardItemID = :WorkplaceConditionsCardItemID ;                            

                            ";

                    changeEvent = new ChangeEvent("HS_WCondCard_EditItem", logDescription, card.MilitaryUnit, null, currentUser);

                    WorkplaceConditionsCardItem oldItem = WorkplaceConditionsCardItemUtil.GetWorkplaceConditionsCardItem(item.WorkplaceConditionsCardItemId, currentUser);

                    if (oldItem.IndicatorTypeName.Trim() != item.IndicatorTypeName.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_WCondCardItem_IndicatorTypeName", oldItem.IndicatorTypeName, item.IndicatorTypeName, currentUser));

                    if (oldItem.IndicatorName.Trim() != item.IndicatorName.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail("HS_WCondCardItem_IndicatorName", oldItem.IndicatorName, item.IndicatorName, currentUser));

                    if (oldItem.Value != item.Value)
                        changeEvent.AddDetail(new ChangeEventDetail("HS_WCondCardItem_Value", (oldItem.Value.HasValue ? CommonFunctions.FormatDecimal(oldItem.Value.Value) : ""), (item.Value.HasValue ? CommonFunctions.FormatDecimal(item.Value.Value) : ""), currentUser));

                    if (oldItem.Rate != item.Rate)
                        changeEvent.AddDetail(new ChangeEventDetail("HS_WCondCardItem_Rate", (oldItem.Rate.HasValue ? CommonFunctions.FormatDecimal(oldItem.Rate.Value) : ""), (item.Rate.HasValue ? CommonFunctions.FormatDecimal(item.Rate.Value) : ""), currentUser));

                    if (oldItem.Assessment != item.Assessment)
                        changeEvent.AddDetail(new ChangeEventDetail("HS_WCondCardItem_Assessment", (oldItem.Assessment.HasValue ? CommonFunctions.FormatDecimal(oldItem.Assessment.Value) : ""), (item.Assessment.HasValue ? CommonFunctions.FormatDecimal(item.Assessment.Value) : ""), currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramWorkplaceConditionsCardItemID = new OracleParameter();
                paramWorkplaceConditionsCardItemID.ParameterName = "WorkplaceConditionsCardItemID";
                paramWorkplaceConditionsCardItemID.OracleType = OracleType.Number;

                if (item.WorkplaceConditionsCardItemId != 0)
                {
                    paramWorkplaceConditionsCardItemID.Direction = ParameterDirection.Input;
                    paramWorkplaceConditionsCardItemID.Value = item.WorkplaceConditionsCardItemId;
                }
                else
                {
                    paramWorkplaceConditionsCardItemID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramWorkplaceConditionsCardItemID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "WorkplaceConditionsCardID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = workplaceConditionsCardId;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "IndicatorTypeID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (item.IndicatorTypeId != -1)
                    param.Value = item.IndicatorTypeId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "IndicatorID";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (item.IndicatorId != null && item.IndicatorId != -1)
                    param.Value = item.IndicatorId;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "IndicatorTypeName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(item.IndicatorTypeName))
                    param.Value = item.IndicatorTypeName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "IndicatorName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                if (!string.IsNullOrEmpty(item.IndicatorName))
                    param.Value = item.IndicatorName;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);              

                param = new OracleParameter();
                param.ParameterName = "Value";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (item.Value.HasValue)
                    param.Value = item.Value.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Rate";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (item.Rate.HasValue)
                    param.Value = item.Rate.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "Assessment";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                if (item.Assessment.HasValue)
                    param.Value = item.Assessment.Value;
                else
                    param.Value = DBNull.Value;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (item.WorkplaceConditionsCardItemId == 0)
                {
                    item.WorkplaceConditionsCardItemId = DBCommon.GetInt(paramWorkplaceConditionsCardItemID.Value);
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
                {
                    changeEntry.AddEvent(changeEvent);
                    WorkplaceConditionsCardUtil.SetWorkplaceConditionsCardLastModified(workplaceConditionsCardId, currentUser);
                }
            }

            return result;
        }

        public static bool DeleteWorkplaceConditionsCardItem(int workplaceConditionsCardId, int cardItemId, User currentUser, Change changeEntry)
        {
            bool result = false;

            WorkplaceConditionsCard card = WorkplaceConditionsCardUtil.GetWorkplaceConditionsCard(workplaceConditionsCardId, currentUser);

            string logDescription = "";
            logDescription += "Номер на карта: " + card.CardNumber;

            WorkplaceConditionsCardItem oldItem = WorkplaceConditionsCardItemUtil.GetWorkplaceConditionsCardItem(cardItemId, currentUser);

            ChangeEvent changeEvent = new ChangeEvent("HS_WCondCard_DeleteItem", logDescription, card.MilitaryUnit, null, currentUser);

            changeEvent.AddDetail(new ChangeEventDetail("HS_WCondCardItem_IndicatorTypeName", oldItem.IndicatorTypeName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_WCondCardItem_IndicatorName", oldItem.IndicatorName, "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_WCondCardItem_Value", (oldItem.Value.HasValue ? CommonFunctions.FormatDecimal(oldItem.Value.Value) : ""), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_WCondCardItem_Rate", (oldItem.Rate.HasValue ? CommonFunctions.FormatDecimal(oldItem.Rate.Value) : ""), "", currentUser));
            changeEvent.AddDetail(new ChangeEventDetail("HS_WCondCardItem_Assessment", (oldItem.Assessment.HasValue ? CommonFunctions.FormatDecimal(oldItem.Assessment.Value) : ""), "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = "DELETE FROM PMIS_HS.WorkplaceConditionsCardItems WHERE WorkplaceConditionsCardItemID = :WorkplaceConditionsCardItemID";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("WorkplaceConditionsCardItemID", OracleType.Number).Value = cardItemId;

                result = cmd.ExecuteNonQuery() == 1;
            }
            finally
            {
                conn.Close();
            }

            if (result)
            {
                changeEntry.AddEvent(changeEvent);

                WorkplaceConditionsCardUtil.SetWorkplaceConditionsCardLastModified(workplaceConditionsCardId, currentUser);
            }

            return result;
        }
    }

}