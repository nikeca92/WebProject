using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;

namespace PMIS.Common
{
    public class GTableItem : BaseDbObject, IDropDownItem
    {
        private int gTableId;
        private string tableName;
        private int tableKey;
        private int tableSeq;
        private string tableValue;

        public int GTableId
        {
            get { return gTableId; }
            set { gTableId = value; }
        }

        public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }

        public int TableKey
        {
            get { return tableKey; }
            set { tableKey = value; }
        }

        public int TableSeq
        {
            get { return tableSeq; }
            set { tableSeq = value; }
        }

        public string TableValue
        {
            get { return tableValue; }
            set { tableValue = value; }
        }

        public GTableItem(User user)
            : base(user)
        {

        }

        //IDropDownItem Members
        public string Text()
        {
            return tableValue;
        }

        public string Value()
        {
            return tableKey.ToString();
        }
    }

    public static class GTableItemUtil
    {
        private static string GetModulSchemeByKey(string moduleKey)
        {
            return "PMIS_" + moduleKey;
        }

        public static GTableItem GetTableItem(string tableName, int tableKey, string moduleKey, User currentUser)
        {
            string moduleScheme = GetModulSchemeByKey(moduleKey);

            GTableItem gTableItem = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.GTableID, a.TableSeq, a.TableValue
                               FROM " + moduleScheme + @".GTable a
                               WHERE a.TableName = :TableName AND a.TableKey = :TableKey";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TableName", OracleType.NVarChar).Value = tableName;
                cmd.Parameters.Add("TableKey", OracleType.Number).Value = tableKey;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    gTableItem = new GTableItem(currentUser);

                    gTableItem.GTableId = DBCommon.GetInt(dr["GTableID"]);
                    gTableItem.TableName = tableName;
                    gTableItem.TableKey = tableKey;
                    gTableItem.TableSeq = DBCommon.GetInt(dr["TableSeq"]);
                    gTableItem.TableValue = dr["TableValue"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return gTableItem;
        }

        public static int GetAllGTableItemsCountByTableName(string tableName, string moduleKey, User currentUser)
        {
            string moduleScheme = GetModulSchemeByKey(moduleKey);
            int gTableItemsCount = 0;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT COUNT(*) as Count
                               FROM " + moduleScheme + @".GTable a
                               WHERE a.TableName = :TableName
                               ";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TableName", OracleType.NVarChar).Value = tableName;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Count"]))
                        gTableItemsCount = DBCommon.GetInt(dr["Count"]);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return gTableItemsCount;
        }

        public static List<GTableItem> GetAllGTableItemsByTableName(string tableName, string moduleKey, int orderBy, int pageIdx, int rowsPerPage, User currentUser)
        {
            string moduleScheme = GetModulSchemeByKey(moduleKey);

            List<GTableItem> gTableItems = new List<GTableItem>();

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

                //2016-12-05: Ticket #149: The GTable classifiers used in Technics are causing issues because they appear 
                //in random order (the users haven't populated the TableSeq field correctly. We need to get all these lists appearing in alphabetical order.
                //Instead of changing the orderBy parameter of each function call, we decided to change the order here and make it always by TableValue.
                //If there is a further issue with this and there is a place where it must be ordered by TableSeq, then we will adjust the code and will use the orderBy
                //parameter again. It could be a good idea to use enum instead of integer values.
                switch (orderBy)
                {
                    case 1:                       
                    case 2:
                        orderBySQL = "a.TableValue";
                        break;
                    default:
                        orderBySQL = "a.TableValue";
                        break;
                }

                orderBySQL += " " + orderByDir + DBCommon.FixNullsOrder(orderByDir);

                string SQL = @"SELECT tmp.*  FROM (
                                  SELECT a.GTableID, a.TableName, a.TableKey, a.TableSeq, a.TableValue, 
                                         RANK() OVER (ORDER BY " + orderBySQL + @", a.GTableID) as RowNumber 
                                  FROM " + moduleScheme + @".GTable a         
                                  WHERE a.TableName = :TableName
                                  ORDER BY " + orderBySQL + @", a.TableKey                             
                               ) tmp
                               " + pageWhere;

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TableName", OracleType.NVarChar).Value = tableName;

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    GTableItem gTableItem = new GTableItem(currentUser);
                    gTableItem.GTableId = DBCommon.GetInt(dr["GTableID"]);
                    gTableItem.TableName = tableName;
                    gTableItem.TableKey = DBCommon.GetInt(dr["TableKey"]);
                    gTableItem.TableSeq = DBCommon.GetInt(dr["TableSeq"]);
                    gTableItem.TableValue = dr["TableValue"].ToString();

                    gTableItems.Add(gTableItem);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return gTableItems;
        }

        public static bool SaveGTableItem(GTableItem gTableItem, string moduleKey, Maintenance gTableMaintance, User currentUser, Change changeEntry)
        {
            string moduleScheme = GetModulSchemeByKey(moduleKey);

            bool result = false;

            string SQL = "";

            ChangeEvent changeEvent;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                SQL = @"DECLARE LastKey " + moduleScheme + @".GTable.TableKey%TYPE;
                        BEGIN
                        
                       ";

                MaintFieldSettings mfsTableSeq = MaintenanceUtil.GetMaintFieldSettings(currentUser, gTableMaintance, "TableSeq");
                MaintFieldSettings mfsTableValue = MaintenanceUtil.GetMaintFieldSettings(currentUser, gTableMaintance, "TableValue");

                if (gTableItem.GTableId == 0)
                {
                    SQL += @"SELECT TableKey INTO LastKey
                                FROM ( SELECT TableKey, RANK() OVER (ORDER BY TableKey DESC) key_rank
                                       FROM " + moduleScheme + @".GTable ) 
                              WHERE key_rank = 1;

                              INSERT INTO " + moduleScheme + @".GTable (TableName, TableKey, TableSeq, TableValue)
                              VALUES (:TableName, LastKey + 1, :TableSeq, :TableValue);

                              SELECT " + moduleScheme + @".GTable_ID_SEQ.currval INTO :GTableID FROM dual;
                            ";

                    changeEvent = new ChangeEvent(gTableMaintance.InsertChangeEventTypeKey, "", null, null, currentUser);

                    changeEvent.AddDetail(new ChangeEventDetail(mfsTableSeq.FieldKey, "", gTableItem.TableSeq.ToString(), currentUser));
                    changeEvent.AddDetail(new ChangeEventDetail(mfsTableValue.FieldKey, "", gTableItem.TableValue, currentUser));
                }
                else
                {
                    SQL += @"UPDATE " + moduleScheme + @".GTable SET
                                TableName = :TableName, 
                                TableKey = :TableKey, 
                                TableSeq = :TableSeq, 
                                TableValue = :TableValue
                               WHERE GTableID = :GTableID; 
                            ";

                    changeEvent = new ChangeEvent(gTableMaintance.UpdateChangeEventTypeKey, "", null, null, currentUser);

                    GTableItem oldGTableItem = GTableItemUtil.GetTableItem(gTableItem.TableName, gTableItem.TableKey, moduleKey, currentUser);

                    if (oldGTableItem.TableSeq != gTableItem.TableSeq)
                        changeEvent.AddDetail(new ChangeEventDetail(mfsTableSeq.FieldKey, oldGTableItem.TableSeq.ToString(), gTableItem.TableSeq.ToString(), currentUser));

                    if (oldGTableItem.TableValue.Trim() != gTableItem.TableValue.Trim())
                        changeEvent.AddDetail(new ChangeEventDetail(mfsTableValue.FieldKey, oldGTableItem.TableValue, gTableItem.TableValue, currentUser));
                }

                SQL += @"END;";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleParameter paramGTableID = new OracleParameter();
                paramGTableID.ParameterName = "GTableID";
                paramGTableID.OracleType = OracleType.Number;

                if (gTableItem.GTableId != 0)
                {
                    paramGTableID.Direction = ParameterDirection.Input;
                    paramGTableID.Value = gTableItem.GTableId;
                }
                else
                {
                    paramGTableID.Direction = ParameterDirection.Output;
                }

                cmd.Parameters.Add(paramGTableID);

                OracleParameter param = new OracleParameter();
                param.ParameterName = "TableName";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = gTableItem.TableName;
                cmd.Parameters.Add(param);

                if (gTableItem.GTableId > 0)
                {
                    param = new OracleParameter();
                    param.ParameterName = "TableKey";
                    param.OracleType = OracleType.Number;
                    param.Direction = ParameterDirection.Input;
                    param.Value = gTableItem.TableKey;
                    cmd.Parameters.Add(param);
                }

                param = new OracleParameter();
                param.ParameterName = "TableSeq";
                param.OracleType = OracleType.Number;
                param.Direction = ParameterDirection.Input;
                param.Value = gTableItem.TableSeq;
                cmd.Parameters.Add(param);

                param = new OracleParameter();
                param.ParameterName = "TableValue";
                param.OracleType = OracleType.VarChar;
                param.Direction = ParameterDirection.Input;
                param.Value = gTableItem.TableValue;
                cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();

                if (gTableItem.GTableId == 0)
                    gTableItem.GTableId = DBCommon.GetInt(paramGTableID.Value);

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

        public static bool DeleteGTableItem(string tableName, int tableKey, string moduleKey, Maintenance gTableMaintance, User currentUser, Change changeEntry)
        {
            string moduleScheme = GetModulSchemeByKey(moduleKey);

            bool result = false;

            GTableItem oldGTableItem = GTableItemUtil.GetTableItem(tableName, tableKey, moduleKey, currentUser);

            ChangeEvent changeEvent = new ChangeEvent(gTableMaintance.DeleteChangeEventTypeKey, "", null, null, currentUser);

            MaintFieldSettings mfsTableSeq = MaintenanceUtil.GetMaintFieldSettings(currentUser, gTableMaintance, "TableSeq");
            changeEvent.AddDetail(new ChangeEventDetail(mfsTableSeq.FieldKey, oldGTableItem.TableSeq.ToString(), "", currentUser));
            
            MaintFieldSettings mfsTableValue = MaintenanceUtil.GetMaintFieldSettings(currentUser, gTableMaintance, "TableValue");
            changeEvent.AddDetail(new ChangeEventDetail(mfsTableValue.FieldKey, oldGTableItem.TableValue, "", currentUser));

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"DELETE FROM " + moduleScheme + @".GTable WHERE TableName = :TableName AND TableKey = :TableKey";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("TableName", OracleType.NVarChar).Value = tableName;
                cmd.Parameters.Add("TableKey", OracleType.Number).Value = tableKey;

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

        public static string GetTableTitle(string tableName, User currentUser)
        {
            string tableTitle = "";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                string SQL = @"SELECT a.HeaderTitle
                               FROM PMIS_ADM.Maintenance a
                               WHERE a.GTableTableName = :GTableTableName";

                SQL = DBCommon.FixNewLines(SQL);

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("GTableTableName", OracleType.NVarChar).Value = tableName;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    tableTitle = dr["HeaderTitle"].ToString();
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return tableTitle;
        }
    }
}
