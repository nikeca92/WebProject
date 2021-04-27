using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;

namespace PMIS.Common
{
    //This class represents the Maintenance object and all its settings
    //Each Maintenance object is used to present a particular Maintenance option in the system
    //Each Maintenance option is an ability to maintaing (edit) a particular data table in the system through a generic screen
    public class Maintenance : BaseDbObject
    {
        private int maintId;
        private string maintKey;
        private string headerTitle;
        private string dataTable;
        private string gTableTableName;
        private string fldList;
        private string updtList;
        private string keyFld;
        private bool canDelete;
        private bool canAdd;
        private string masterTable;
        private string masterField;
        private string changeTypeKey;
        private string insertChangeEventTypeKey;
        private string updateChangeEventTypeKey;
        private string deleteChangeEventTypeKey;
        private string menuItemKey;
        private string uiKeyMaintenance;
        private string uiKeyDelete;
        private string uiKeyEdit;
        private string uiKeyAdd;
        private int moduleId;
        private Module module;

        //The unique ID of the Maintenance record
        public int MaintId
        {
            get
            {
                return maintId;
            }
            set
            {
                maintId = value;
            }
        }

        //The "key" of the Maintenance record.
        //This key is used the open a particular Maint screen
        public string MaintKey
        {
            get
            {
                return maintKey;
            }
            set
            {
                maintKey = value;
            }
        }

        //This is the header text of the particular Maintenance screen
        //It is used for both the title of the page and the header label on the screen
        public string HeaderTitle
        {
            get
            {
                return headerTitle;
            }
            set
            {
                headerTitle = value;
            }
        }

        //This is the name of the DB data table which would be maintained
        public string DataTable
        {
            get
            {
                return dataTable;
            }
            set
            {
                dataTable = value;
            }
        }

        //If the particular data table is a stored in the GTable DB table then set the name of the GTable table to this property
        public string GTableTableName
        {
            get
            {
                return gTableTableName;
            }
            set
            {
                gTableTableName = value;
            }
        }

        //This is a flag if the particular Maintenance record is for editing a "GTable" table
        public bool IsGTable
        {
            get
            {
                return !String.IsNullOrEmpty(GTableTableName);
            }
        }


        //Ths is the list of fields
        public string FldList
        {
            get
            {
                return fldList;
            }
            set
            {
                fldList = value;
            }
        }

        //This is the list of fields that would be updated
        public string UpdtList
        {
            get
            {
                return updtList;
            }
            set
            {
                updtList = value;
            }
        }

        //Thisi is the "key field". Usually, this is the ID (the PrimaryKey column) of the DB record
        public string KeyFld
        {
            get
            {
                return keyFld;
            }
            set
            {
                keyFld = value;
            }
        }

        //This is a flag if there would be an ability to delete records
        public bool CanDelete
        {
            get
            {
                return canDelete;
            }
            set
            {
                canDelete = value;
            }
        }

        //This is a flag if there would be an ability to add new records
        public bool CanAdd
        {
            get
            {
                return canAdd;
            }
            set
            {
                canAdd = value;
            }
        }

        //This is the name of the Table (or tables) where there could be any related record
        //It is used to check if the Delete button would be displayed on a particular row
        public string MasterTable
        {
            get
            {
                return masterTable;
            }
            set
            {
                masterTable = value;
            }
        }

        //This is the name of the Field (it should containt the table name too as a prefix) where could be any related records
        //These are any FK columns
        public string MasterField
        {
            get
            {
                return masterField;
            }
            set
            {
                masterField = value;
            }
        }

        //This is the key for the ChangeTypeKey table
        //It is used whene the changes on the screen should be logged in the AuditTrail
        public string ChangeTypeKey
        {
            get
            {
                return changeTypeKey;
            }
            set
            {
                changeTypeKey = value;
            }
        }

        //This is the ChangeEventTypeKey for the Insert event
        public string InsertChangeEventTypeKey
        {
            get
            {
                return insertChangeEventTypeKey;
            }
            set
            {
                insertChangeEventTypeKey = value;
            }
        }

        //This is the ChangeEventTypeKey for the Update event
        public string UpdateChangeEventTypeKey
        {
            get
            {
                return updateChangeEventTypeKey;
            }
            set
            {
                updateChangeEventTypeKey = value;
            }
        }

        //This is the ChangeEventTypeKey for the Delete event
        public string DeleteChangeEventTypeKey
        {
            get
            {
                return deleteChangeEventTypeKey;
            }
            set
            {
                deleteChangeEventTypeKey = value;
            }
        }

        //This is the key from the menu for the particular maintenance record
        //It is used to hilight the correct item from the menu
        public string MenuItemKey
        {
            get
            {
                return menuItemKey;
            }
            set
            {
                menuItemKey = value;
            }
        }

        //This is the UIItems key for the entire Maintenance record
        public string UIKeyMaintenance
        {
            get
            {
                return uiKeyMaintenance;
            }

            set
            {
                uiKeyMaintenance = value;
            }
        }

        //This is the UIItems key for the "Delete" functionality
        public string UIKeyDelete
        {
            get
            {
                return uiKeyDelete;
            }

            set
            {
                uiKeyDelete = value;
            }
        }

        //This is the UIItems key for the "Edit" functionality
        public string UIKeyEdit
        {
            get
            {
                return uiKeyEdit;
            }

            set
            {
                uiKeyEdit = value;
            }
        }

        //This is the UIItems key for the "Add" functionality
        public string UIKeyAdd
        {
            get
            {
                return uiKeyAdd;
            }

            set
            {
                uiKeyAdd = value;
            }
        }

        //The ID of the related Module
        public int ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }

        //This is the related Module
        public Module Module
        {
            get
            {
                if (module == null)
                {
                    module = ModuleUtil.GetModule(CurrentUser, moduleId);
                }
                return module;
            }
            set
            {
                module = value;
            }
        }

        public Maintenance(User currentUser)
            : base(currentUser)
        {
            
        }
    }

    //This class represent a particular option in a drop-down when the colum is a "drop-down" column
    public class MaintFieldOption
    {
        private string val;
        private string lbl;

        public string Value
        {
            get
            {
                return val;
            }

            set
            {
                val = value;
            }
        }

        public string Label
        {
            get
            {
                return lbl;
            }

            set
            {
                lbl = value;
            }
        }
    }

    //This object represents a particular Field in the grid
    public class MaintField
    {
        private string fieldName;
        private string fieldValue;
        private int? columnSize;
        private bool isKeyField;
        private bool isReadOnly;
        private bool isHidden;
        private List<MaintFieldOption> options = new List<MaintFieldOption>();
        private string oldValue;

        //This is the name of the field (i.e. the DB Column name)
        public string FieldName
        {
            get
            {
                return fieldName;
            }
            set
            {
                fieldName = value;
            }
        }

        //This is the value of the particular field
        public string FieldValue
        {
            get
            {
                return fieldValue;
            }
            set
            {
                fieldValue = value;
            }
        }

        //This is the width of the column
        //It is used to set the MaxLength property to prevent entering very log values
        public int? ColumnSize
        {
            get
            {
                return columnSize;
            }
            set
            {
                columnSize = value;
            }
        }

        //This is a flag if this is the key column (i.e. the ID)
        public bool IsKeyField
        {
            get
            {
                return isKeyField;
            }

            set
            {
                isKeyField = value;
            }
        }

        //This is a flag if the field should be read-only (put a label instead of an input)
        public bool IsReadOnly
        {
            get
            {
                return isReadOnly;
            }
            set
            {
                isReadOnly = value;
            }
        }

        //This is a flag that says if the column should be hidden
        public bool IsHidden
        {
            get
            {
                return isHidden;
            }
            set
            {
                isHidden = value;
            }
        }

        //This is a list of options if the field represents a drop-down
        public List<MaintFieldOption> Options
        {
            get
            {
                return options;
            }
            set
            {
                options = value;
            }
        }

        //This stores the OldValue of the input. It is used when generating the Audti Trail records
        public string OldValue
        {
            get
            {
                return oldValue;
            }
            set
            {
                oldValue = value;
            }
        }

        //Use these properties when working with the audit trail records. They return the text value of the field.
        //The text value could be different than the actual value only for drop-down fields (containing Options)
        public string FieldTextValue
        {
            get
            {
                string textValue = FieldValue;

                //If there are any Options then it is a drop-down instead of a textbox
                if (Options.Count > 0)
                {
                    foreach (MaintFieldOption option in Options)
                        if (option.Value == FieldValue)
                        {
                            textValue = option.Label;
                            break;
                        }
                }

                return textValue;
            }
        }

        public string OldTextValue
        {
            get
            {
                string oldTextValue = OldValue;

                //If there are any Options then it is a drop-down instead of a textbox
                if (Options.Count > 0)
                {
                    foreach (MaintFieldOption option in Options)
                        if (option.Value == OldValue)
                        {
                            oldTextValue = option.Label;
                            break;
                        }
                }

                return oldTextValue;
            }
        }

        public MaintField()
        {
        }
    }

    //This represents an object which is actually a row from the grid that is going to be saved
    public class MaintRowForSave
    {
        private string keyFieldValue;
        private bool isDeleted;
        private bool isNewRow;
        List<MaintField> maintFields = new List<MaintField>();

        //This is the value of the key field (the ID)
        public string KeyFieldValue
        {
            get
            {
                return keyFieldValue;
            }
            set
            {
                keyFieldValue = value;
            }
        }

        //This is a flag if the particular row is Deleted
        public bool IsDeleted
        {
            get
            {
                return isDeleted;
            }
            set
            {
                isDeleted = value;
            }
        }

        //This is flag if the particular row is a newly inserted one
        public bool IsNewRow
        {
            get
            {
                return isNewRow;
            }
            set
            {
                isNewRow = value;
            }
        }

        //This is a list of all field on the row
        public List<MaintField> MaintFields
        {
            get
            {
                return maintFields;
            }
            set
            {
                maintFields = value;
            }
        }

        public MaintRowForSave()
        {
        }
    }

    //This class represents the settings for each specific MaintField
    public class MaintFieldSettings
    {
        private string fieldLabel;
        private int widthPixels;
        private string fieldKey;
        private string uiItemEditKey;
        private string uiItemAddKey;
        private bool isMandatory;
        private string validateDataType;
        private bool isUnique;

        //This is the label of the field. It is usually used as a header text in its column in the data grid
        public string FieldLabel
        {
            get
            {
                return fieldLabel;
            }
            set
            {
                fieldLabel = value;
            }
        }

        //This specifies the width of the column in pixels
        public int WidthPixels
        {
            get
            {
                return widthPixels;
            }
            set
            {
                widthPixels = value;
            }
        }

        //This is the field's "key". It is used to map the field in the list of Fields that are logged in the Audit Trail
        public string FieldKey
        {
            get
            {
                return fieldKey;
            }
            set
            {
                fieldKey = value;
            }
        }

        //This is the UIITem key of the current field. It is used when applying the UIItems rules in Edit mode.
        public string UIItemEditKey
        {
            get
            {
                return uiItemEditKey;
            }
            set
            {
                uiItemEditKey = value;
            }
        }

        //This is the UIITem key of the current field. It is used when applying the UIItems rules in Add mode.
        public string UIItemAddKey
        {
            get
            {
                return uiItemAddKey;
            }
            set
            {
                uiItemAddKey = value;
            }
        }

        //This is a flag if the field is mandatory
        public bool IsMandatory
        {
            get
            {
                return isMandatory;
            }
            set
            {
                isMandatory = value;
            }
        }

        //This is the validation data type for this field (e.g. number, integer)
        public string ValidateDataType
        {
            get
            {
                return validateDataType;
            }
            set
            {
                validateDataType = value;
            }
        }

        //This is a flag if the values in the column should be unique
        public bool IsUnique
        {
            get
            {
                return isUnique;
            }
            set
            {
                isUnique = value;
            }
        }

        public MaintFieldSettings()
        {
        }
    }

    //The MaintenanceUtil static class has some method that help when working with Maintenance objects
    public static class MaintenanceUtil
    {
        //This method creates a Maintenance object by exstracting the data from a data reader
        public static Maintenance ExtractMaintenanceFromDataReader(OracleDataReader dr, User currentUser)
        {
            int? maintId = null;
            
            if(DBCommon.IsInt(dr["MaintID"]))
                maintId = DBCommon.GetInt(dr["MaintID"]);

            string maintKey = dr["MaintKey"].ToString();
            string headerTitle = dr["HeaderTitle"].ToString();
            string dataTable = dr["DataTable"].ToString();
            string gTableTableName = dr["GTableTableName"].ToString();
            string fldList = dr["FldList"].ToString();
            string updtList = dr["UpdtList"].ToString();
            string keyFld = dr["KeyFld"].ToString();
            bool canDelete = DBCommon.IsInt(dr["CanDelete"]) && DBCommon.GetInt(dr["CanDelete"]) == 1;
            bool canAdd = DBCommon.IsInt(dr["CanAdd"]) && DBCommon.GetInt(dr["CanAdd"]) == 1;
            string masterTable = dr["MasterTable"].ToString();
            string masterField = dr["MasterField"].ToString();
            string changeTypeKey = dr["ChangeTypeKey"].ToString();
            string insertChangeEventTypeKey = dr["InsertChangeEventTypeKey"].ToString();
            string updateChangeEventTypeKey = dr["UpdateChangeEventTypeKey"].ToString();
            string deleteChangeEventTypeKey = dr["DeleteChangeEventTypeKey"].ToString();
            string menuItemKey = dr["MenuItemKey"].ToString();
            string uiKeyMaintenance = dr["UIKeyMaintenance"].ToString();
            string uiKeyDelete = dr["UIKeyDelete"].ToString();
            string uiKeyEdit = dr["UIKeyEdit"].ToString();
            string uiKeyAdd = dr["UIKeyAdd"].ToString();
            int moduleId = DBCommon.GetInt(dr["ModuleID"]);

            Maintenance maint = new Maintenance(currentUser);

            if (maintId.HasValue)
            {
                maint.MaintId = maintId.Value;
                maint.MaintKey = maintKey;
                maint.HeaderTitle = headerTitle;
                maint.DataTable = dataTable;
                maint.GTableTableName = gTableTableName;
                maint.FldList = fldList;
                maint.UpdtList = updtList;
                maint.KeyFld = keyFld;
                maint.CanDelete = canDelete;
                maint.CanAdd = canAdd;
                maint.MasterTable = masterTable;
                maint.MasterField = masterField;
                maint.ChangeTypeKey = changeTypeKey;
                maint.InsertChangeEventTypeKey = insertChangeEventTypeKey;
                maint.UpdateChangeEventTypeKey = updateChangeEventTypeKey;
                maint.DeleteChangeEventTypeKey = deleteChangeEventTypeKey;
                maint.MenuItemKey = menuItemKey;
                maint.UIKeyMaintenance = uiKeyMaintenance;
                maint.UIKeyDelete = uiKeyDelete;
                maint.UIKeyEdit = uiKeyEdit;
                maint.UIKeyAdd = uiKeyAdd;
                maint.ModuleId = moduleId;
            }

            return maint;
        }

        //Get a specific Maintenance object
        public static Maintenance GetMaintenance(User currentUser, string maintKey)
        {
            Maintenance maintenance = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string SQL = "";

            try
            {
                SQL = @"SELECT a.MaintID, a.MaintKey, a.HeaderTitle, a.DataTable, a.GTableTableName,
                               a.FldList, a.UpdtList, a.KeyFld, a.CanDelete, a.CanAdd, a.MasterTable, a.MasterField,
                               a.ChangeTypeKey, a.InsertChangeEventTypeKey, a.UpdateChangeEventTypeKey, a.DeleteChangeEventTypeKey,
                               a.MenuItemKey,
                               a.UIKeyMaintenance, a.UIKeyDelete, a.UIKeyEdit, a.UIKeyAdd, a.ModuleID
                        FROM PMIS_ADM.Maintenance a
                        WHERE a.MaintKey = :MaintKey";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.Parameters.Add("MaintKey", OracleType.VarChar).Value = maintKey;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    maintenance = ExtractMaintenanceFromDataReader(dr, currentUser);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return maintenance;
        }

        //Get the MaintFieldSettings object for a specific column
        public static MaintFieldSettings GetMaintFieldSettings(User currentUser, Maintenance maint, string tableColumn)
        {
            MaintFieldSettings settings = null;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string SQL = "";

            try
            {
                SQL = @"SELECT a.FieldLabel, a.WidthPixels, a.FieldKey,
                               a.UIItemEditKey, a.UIItemAddKey, a.IsMandatory, a.ValidateDataType, a.IsUnique
                        FROM PMIS_ADM.MaintFields a
                        WHERE a.TableName = :TableName AND a.TableColumn = :TableColumn";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                string tableName = maint.DataTable;

                if (maint.IsGTable)
                    tableName += "." + maint.GTableTableName;

                cmd.Parameters.Add("TableName", OracleType.VarChar).Value = tableName;
                cmd.Parameters.Add("TableColumn", OracleType.VarChar).Value = tableColumn;

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    string fieldLabel = dr["FieldLabel"].ToString();
                    
                    int widthPixels = 0;

                    if (DBCommon.IsInt(dr["WidthPixels"]))
                        widthPixels = DBCommon.GetInt(dr["WidthPixels"]);

                    string fieldKey = dr["FieldKey"].ToString();
                    string uiItemEditKey = dr["UIItemEditKey"].ToString();
                    string uiItemAddKey = dr["UIItemAddKey"].ToString();
                    bool isMandatory = DBCommon.IsInt(dr["IsMandatory"]) && DBCommon.GetInt(dr["IsMandatory"]) == 1;
                    string validateDataType = dr["ValidateDataType"].ToString();
                    bool isUnique = DBCommon.IsInt(dr["IsUnique"]) && DBCommon.GetInt(dr["IsUnique"]) == 1;

                    settings = new MaintFieldSettings();
                    settings.FieldLabel = fieldLabel;
                    settings.WidthPixels = widthPixels;
                    settings.FieldKey = fieldKey;
                    settings.UIItemEditKey = uiItemEditKey;
                    settings.UIItemAddKey = uiItemAddKey;
                    settings.IsMandatory = isMandatory;
                    settings.ValidateDataType = validateDataType;
                    settings.IsUnique = isUnique;
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return settings;
        }

        //Get a list ot Options (by passing the SQL) for drop-down columns
        public static List<MaintFieldOption> GetFieldOptions(User currentUser, string SQL)
        {
            List<MaintFieldOption> options = new List<MaintFieldOption>();

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    MaintFieldOption option = new MaintFieldOption();
                    option.Value = dr["ID"].ToString();
                    option.Label = dr["Text"].ToString();

                    options.Add(option);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return options;
        }

        //Get the data for a particular Maintenance grid
        public static List<List<MaintField>> GetMaintData(User currentUser, Maintenance maint, bool onlyOneRow, int pageIdx, int rowsPerPage)
        {
            List<List<MaintField>> maintData = new List<List<MaintField>>();

            string SQL = "";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                //Get the list of fields
                string[] arrFields = maint.FldList.Split(',');

                for (int i = 0; i < arrFields.Length; i++)
                {
                    arrFields[i] = arrFields[i].Replace("~", ",");

                    //Add each field to the SELECT clause
                    SQL += ((SQL == "") ? "" : ",") +
                        "\"" + arrFields[i].Trim().Substring(0, (arrFields[i].Trim().IndexOf('[') > 0 ? arrFields[i].Trim().IndexOf('[') : arrFields[i].Trim().Length)).ToUpper() + "\"";
                }

                string pageWhere = "";

                if (pageIdx > 0 && rowsPerPage > 0)
                    pageWhere = " RowNumber BETWEEN (" + pageIdx.ToString() + @" - 1) * " + rowsPerPage.ToString() + @" + 1 AND " + pageIdx.ToString() + @" * " + rowsPerPage.ToString() + @" ";

                //Construct the SELECT query. Note that it is slightly different when using GTable
                if (!maint.IsGTable)
                    SQL = "SELECT * FROM (SELECT " + SQL + ",  RANK() OVER (ORDER BY " + maint.KeyFld + ") as RowNumber FROM " + maint.DataTable + ") TMP " + (pageWhere == "" ? "" : " WHERE ") + pageWhere;
                else
                    SQL = "SELECT * FROM (SELECT " + SQL + ",  RANK() OVER (ORDER BY TableSeq) as RowNumber FROM " + maint.DataTable + " WHERE TableName = '" + maint.GTableTableName.Replace("'", "''") + "') TMP " + (pageWhere == "" ? "" : " WHERE ") + pageWhere;

                //Loading only the first row. It is used when adding new records in the grid
                if (onlyOneRow)
                {
                    SQL += " WHERE RowNumber = 1 ";
                }

                if (maint.IsGTable)
                {
                    SQL += " ORDER BY TableSeq ";
                }

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                DataTable schema = dr.GetSchemaTable();

                while (dr.Read())
                {
                    //Construct each row in the data list
                    List<MaintField> fields = new List<MaintField>();

                    //Iterate through the columns
                    for (int i = 0; i < arrFields.Length; i++)
                    {
                        string field = arrFields[i].Trim().Substring(0, (arrFields[i].Trim().IndexOf('[') > 0 ? arrFields[i].Trim().IndexOf('[') : arrFields[i].Trim().Length));
                        string value = "";

                        if (dr[field] is decimal)
                            value = CommonFunctions.FormatDecimal((decimal)dr[field]);
                        else if (dr[field] is DateTime)
                            value = CommonFunctions.FormatDate((DateTime)dr[field]);
                        else
                            value = dr[field].ToString().Trim();

                        bool isKeyField = false;

                        if (field.ToUpper().Trim() == maint.KeyFld.ToUpper().Trim())
                            isKeyField = true;

                        //Get the main information about each field (e.g. the name, the value, etc.)
                        MaintField maintField = new MaintField();
                        maintField.FieldName = field;
                        maintField.FieldValue = value;
                        maintField.IsKeyField = isKeyField;
                        
                        //Check if it is a read-only column
                        if (arrFields[i].Trim().ToUpper().EndsWith("[READONLY]"))
                        {
                            maintField.IsReadOnly = true;
                        }
                        else if (arrFields[i].Trim().ToUpper().Contains("[SELECT ")) //Check if there is provided a SELECT query that should populate a drop-down
                        {
                            maintField.Options = GetFieldOptions(currentUser, arrFields[i].Trim().Substring(arrFields[i].Trim().IndexOf("[") + 1, arrFields[i].Trim().LastIndexOf("]") - arrFields[i].Trim().IndexOf("[") - 1));
                        }
                        else //Otherwise it is a regular text-box input. In this case get the width of the column
                        {
                            if (dr[field] is string)
                                foreach (DataRow row in schema.Rows)
                                {
                                    if (row["ColumnName"].ToString().ToUpper().Trim() == field.ToUpper().Trim())
                                    {
                                        if ((int)row["ColumnSize"] > 0)
                                            maintField.ColumnSize = (int)row["ColumnSize"];
                                    }
                                }
                        }

                        //Finally check if the column should be Hidden
                        if (arrFields[i].Trim().ToUpper().EndsWith("[HIDDEN]"))
                            maintField.IsHidden = true;

                        fields.Add(maintField);
                    }

                    maintData.Add(fields);
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return maintData;
        }

        //Get the number of records for a particular Maintenance grid (used for paging)
        public static int GetMaintRecordsCnt(User currentUser, Maintenance maint)
        {
            int cnt = 0;

            string SQL = "";

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                //Construct the SELECT query. Note that it is slightly different when using GTable
                if (!maint.IsGTable)
                    SQL = "SELECT COUNT(*) as Cnt FROM " + maint.DataTable;
                else
                    SQL = "SELECT COUNT(*) as Cnt FROM " + maint.DataTable + " WHERE TableName = '" + maint.GTableTableName.Replace("'", "''") + "'";

                OracleCommand cmd = new OracleCommand(SQL, conn);

                OracleDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    if (DBCommon.IsInt(dr["Cnt"]))
                    {
                        cnt = DBCommon.GetInt(dr["Cnt"]);
                    }
                }

                dr.Close();
            }
            finally
            {
                conn.Close();
            }

            return cnt;
        }

        //Check if there are any related record (by a FK constraint) by using the MasterTable and MasterField settings
        //It is used to find out if the record could be deleted
        public static bool AreThereRelatedRecords(User currentUser, string KeyFieldValue, string MasterTable, string MasterField)
        {
            if (KeyFieldValue.Trim() == "" ||
                MasterTable.Trim() == "" ||
                MasterField.Trim() == "")
                return false;

            bool anyRelatedRecords = false;

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            string SQL = "";

            try
            {
                string[] MasterTables = MasterTable.Split(',');
                string[] MasterFields = MasterField.Split(',');

                for (int i = 0; i < MasterTables.Length; i++)
                {
                    for (int j = 0; j < MasterFields.Length; j++)
                    {
                        if (MasterFields[j].StartsWith(MasterTables[i]))
                        {
                            //Construct a SELECT query that should check if there are any related records
                            SQL = "SELECT NULL FROM " + MasterTables[i] + " " +
                                  "WHERE " + MasterFields[j] + " = '" + KeyFieldValue.Replace("'", "''") + "' ";

                            OracleCommand cmd = new OracleCommand(SQL, conn);

                            OracleDataReader dr = cmd.ExecuteReader();

                            if (dr.Read())
                            {
                                dr.Close();

                                anyRelatedRecords = true;
                                break;
                            }

                            dr.Close();
                        }
                    }

                    if(anyRelatedRecords)
                        break;
                }
            }
            finally
            {
                conn.Close();
            }

            return anyRelatedRecords;
        }

        //Save the Maintenance data grid
        public static bool SaveMaintData(User currentUser, Maintenance maint, List<MaintRowForSave> maintData, Change changeEntry)
        {
            if (maintData.Count == 0)
                return true;

            //Map settings to each field by index
            //Get the settings only once to avoid calling the GetMaintFieldSettings for each field on each row
            List<MaintFieldSettings> settings = new List<MaintFieldSettings>();

            foreach (MaintField field in maintData[0].MaintFields)
            {            
                MaintFieldSettings maintFieldSettings = MaintenanceUtil.GetMaintFieldSettings(currentUser, maint, field.FieldName);
                settings.Add(maintFieldSettings);
            }

            bool isSaved = false;

            string SQL = "BEGIN ";

            foreach (MaintRowForSave dataRow in maintData)
            {
                //If the row is deleted
                if (dataRow.IsDeleted)
                {
                    if (!maint.IsGTable)
                        SQL += "DELETE FROM " + maint.DataTable + " WHERE " + maint.KeyFld + " = '" + dataRow.KeyFieldValue.Replace("'", "''") + "'; ";
                    else
                        SQL += "DELETE FROM " + maint.DataTable + " WHERE TableName = '" + maint.GTableTableName.Replace("'", "''") + "' AND " + maint.KeyFld + " = '" + dataRow.KeyFieldValue.Replace("'", "''") + "'; ";

                    //If there is a Delete ChangeEventTypeKey then log the deleted row into the Audit Trail
                    //Log all fields on the row
                    if (changeEntry != null && !String.IsNullOrEmpty(maint.DeleteChangeEventTypeKey))
                    {
                        ChangeEvent changeEvent = new ChangeEvent(maint.DeleteChangeEventTypeKey, "", null, null, currentUser);

                        int idx = 0;
                        foreach (MaintField field in dataRow.MaintFields)
                        {
                            changeEvent.AddDetail(new ChangeEventDetail(settings[idx].FieldKey, field.OldTextValue, "", currentUser));
                            idx++;
                        }

                        changeEntry.AddEvent(changeEvent);
                    }
                }
                else
                {
                    //If it is an existing row then construct an UPDATE query
                    if (!dataRow.IsNewRow)
                    {
                        SQL += "UPDATE " + maint.DataTable + " SET ";

                        string updateList = "";

                        ChangeEvent changeEvent = null;
                        if (changeEntry != null && !String.IsNullOrEmpty(maint.UpdateChangeEventTypeKey))
                            changeEvent = new ChangeEvent(maint.UpdateChangeEventTypeKey, "", null, null, currentUser);

                        int idx = 0;

                        //Iterate through all fields on the row and check if there are any changes to be logged
                        foreach (MaintField field in dataRow.MaintFields)
                        {
                            updateList += ((updateList == "") ? "" : ", ") +
                                           " " + field.FieldName + " = '" + field.FieldValue.Replace("'", "''") + "' ";

                            if (changeEvent != null && field.FieldValue != field.OldValue)
                                changeEvent.AddDetail(new ChangeEventDetail(settings[idx].FieldKey, field.OldTextValue, field.FieldTextValue, currentUser));

                            idx++;
                        }

                        if (changeEvent != null && changeEvent.ChangeEventDetails.Count > 0)
                            changeEntry.AddEvent(changeEvent);

                        if (!maint.IsGTable)
                            SQL += updateList + " WHERE " + maint.KeyFld + " = '" + dataRow.KeyFieldValue.Replace("'", "''") + "'; ";
                        else
                            SQL += updateList + " WHERE TableName = '" + maint.GTableTableName.Replace("'", "''") + "' AND " + maint.KeyFld + " = '" + dataRow.KeyFieldValue.Replace("'", "''") + "'; ";
                    }
                    else
                    {
                        //If it is a new record then construct an INSERT clause
                        SQL += "INSERT INTO " + maint.DataTable + " ( ";

                        string fieldsList = "";

                        if (maint.IsGTable)
                            fieldsList = "TableName";
                        
                        //Write the changes to the Audti Trail history log
                        ChangeEvent changeEvent = null;
                        if (changeEntry != null && !String.IsNullOrEmpty(maint.InsertChangeEventTypeKey))
                            changeEvent = new ChangeEvent(maint.InsertChangeEventTypeKey, "", null, null, currentUser);

                        int idx = 0;
                        foreach (MaintField field in dataRow.MaintFields)
                        {
                            fieldsList += ((fieldsList == "") ? "" : ", ") +
                                           " " + field.FieldName + " ";

                            if (changeEvent != null)
                                changeEvent.AddDetail(new ChangeEventDetail(settings[idx].FieldKey, "", field.FieldTextValue, currentUser));

                            idx++;
                        }

                        if (changeEvent != null)
                            changeEntry.AddEvent(changeEvent);

                        SQL += fieldsList + ") VALUES (";

                        string valuesList = "";

                        if(maint.IsGTable)
                            valuesList = "'" + maint.GTableTableName.Replace("'", "''") + "'";

                        foreach (MaintField field in dataRow.MaintFields)
                        {
                            valuesList += ((valuesList == "") ? "" : ", ") +
                                           " '" + field.FieldValue.Replace("'", "''") + "' ";
                        }

                        SQL += valuesList + "); ";
                    }
                }
            }

            SQL += " END; ";

            SQL = DBCommon.FixNewLines(SQL);

            OracleConnection conn = new OracleConnection(currentUser.ConnectionString);
            conn.Open();

            try
            {
                OracleCommand cmd = new OracleCommand(SQL, conn);

                cmd.ExecuteNonQuery();

                isSaved = true;
            }
            finally
            {
                conn.Close();
            }

            return isSaved;
        }
    }
}