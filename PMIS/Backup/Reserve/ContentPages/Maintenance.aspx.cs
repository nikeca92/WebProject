using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    /*THIS PAGE IS USED IN ALL MODULES. SO, IF THERE ARE ANY CHANGE IN THIS PAGE THEN PROBABLY THE SAME CHANGES SHULD BE
      APPLIED IN THE OTHER MODULES (PROJECTS) TOO*/
    public partial class MaintenancePage : RESPage
    {
        //Get the config setting that says how many rows per page should be dispayed in the grid
        private int pageLength = int.Parse(Config.GetWebSetting("RowsPerPage"));
        //Stores information about how many pages are in the grid
        private int maxPage;

        private int PageIdx
        {
            get
            {
                int pageIdx = 1;

                if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                    pageIdx = 1;

                return pageIdx;
            }

            set
            {
                hdnPageIdx.Value = value.ToString();
            }
        }

        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "HS_LISTMAINT";
            }
        }

        private Maintenance maint = null;

        //This key is used to have an unique session variable name on each page.
        //It is used to prevent mixing session data when the user opens the same screen in two windows/tabs, for example
        private string TableGridSessionName
        {
            get
            {
                return "TableGrid" + UniqueWindowID.Value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {            
            //Set the unique GUID that is used to have an unique session variable name
            if (UniqueWindowID.Value == "")
                UniqueWindowID.Value = Guid.NewGuid().ToString();

            //Get the particula MaintKey. It is used to find the Maintenance record that would be loaded
            string maintKey = Request.Params["MaintKey"];

            if (maintKey == null)
                maintKey = "";

            //Get the infomration about the current Maintenance record
            maint = MaintenanceUtil.GetMaintenance(CurrentUser, maintKey);

            if (maint != null && maint.MaintId > 0)
            {
                int allRows = MaintenanceUtil.GetMaintRecordsCnt(CurrentUser, maint);
                //Get the number of rows and calculate the number of pages in the grid
                maxPage = pageLength == 0 ? 1 : allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

                if (GetUIItemAccessLevel(ModuleKey + "_LISTMAINT") == UIAccessLevel.Hidden ||
                    GetUIItemAccessLevel(maint.UIKeyMaintenance) == UIAccessLevel.Hidden ||
                    CurrentUser.Role.ModuleByRole.ModuleId != maint.Module.ModuleId)
                {
                    RedirectAccessDenied();
                }

                if (GetUIItemAccessLevel(ModuleKey + "_LISTMAINT") == UIAccessLevel.Disabled ||
                    GetUIItemAccessLevel(maint.UIKeyMaintenance) == UIAccessLevel.Disabled ||
                    (GetUIItemAccessLevel(maint.UIKeyEdit) == UIAccessLevel.Disabled &&
                     GetUIItemAccessLevel(maint.UIKeyAdd) == UIAccessLevel.Disabled &&
                     GetUIItemAccessLevel(maint.UIKeyDelete) == UIAccessLevel.Disabled))
                {
                    pageDisabledControls.Add(btnSave);
                }

                //Highlight the correct menu item. It is specific for each maintenance record.
                HighlightMenuItems("Lists", maint.MenuItemKey);

                //Set up the correct page title. It is pulled from the Maintenance record
                lblHeader.Text = maint.HeaderTitle;
                Page.Title = maint.HeaderTitle;

                //Load the data grid
                TableGrid();
                TableGrid_ReassignImgButtonsClick(Page);
            }
            else
            {
                NotCorrect("Error");
            }

            LnkForceNoChangesCheck(btnSave);
            //Hide the navigation buttons
            HideNavigationControls(btnCancel);

            lblStatus.Text = "";
        }

        //This method load the table grid on the page - either from the Session object or create a new data table
        protected void TableGrid()
        {
            if (!IsPostBack && Session[TableGridSessionName] != null)
                Session[TableGridSessionName] = null;

            Table tbl = new Table();

            //If loading the page for the first time then create a new table and store it in the session
            if (!IsPostBack)
            {
                tblMaintTable.Style.Add("margin", "0 auto");
                tbl = tblMaintTable;
                TableGrid_AddTableGridHeader(ref tbl);
                TableGrid_LoadTableGrid(ref tbl);
                TableGrid_AddRowButton(ref tbl);
                Session[TableGridSessionName] = tbl;
            }
            else //If this is a post back then get the table object from the session
            {
                tbl = (Table)Session[TableGridSessionName];
            }

            int idx = MainDiv.Controls.IndexOf(tblMaintTable);

            MainDiv.Controls.RemoveAt(idx);
            MainDiv.Controls.AddAt(idx, tbl);
        }

        protected void NotCorrect(string message)
        {
            lblMessage.Text = message;
            MainDiv.Visible = false;
            MessageDiv.Visible = true;
        }

        //Load the table grid header
        private void TableGrid_AddTableGridHeader(ref Table tbl)
        {
            TableRow tr = new TableRow();
            tr.VerticalAlign = VerticalAlign.Bottom;
            tr.CssClass = "TableHeaderRow";

            string[] arrFields = maint.FldList.Split(',');

            //Iterate through all fields from the Maintenance record
            for (int i = 0; i < arrFields.Length; i++)
            {
                TableCell tc = new TableCell();
                tc.CssClass = "TableHeaderCell";

                string tableColumn = arrFields[i].Trim().Substring(0, (arrFields[i].Trim().IndexOf('[') > 0 ? arrFields[i].Trim().IndexOf('[') : arrFields[i].Trim().Length));

                //For each column get specific settings (the are pulled from the DB)
                MaintFieldSettings setings = MaintenanceUtil.GetMaintFieldSettings(CurrentUser, maint, tableColumn);

                if (setings != null)
                {
                    //The column header is stored in the FieldLabel property
                    tc.Text = setings.FieldLabel;

                    //If there is provided an exact width in pixes then set it.
                    if (setings.WidthPixels > 0)
                    {
                        tc.Style.Add("width", setings.WidthPixels.ToString() + "px");
                    }
                }

                //If the column is set to be hidden (e.g. the ID) then hide the cell
                if (arrFields[i].Trim().ToUpper().EndsWith("[HIDDEN]"))
                    tc.Visible = false;

                tr.Cells.Add(tc);
            }

            //This is header cell of the image buttons column
            TableHeaderCell tcBtns = new TableHeaderCell();
            tcBtns.CssClass = "TableHeaderCell";
            tcBtns.Style.Add("width", "60px");
            tcBtns.Text = "&nbsp;";
            tr.Cells.Add(tcBtns);

            tbl.Rows.Add(tr);
        }

        //Load the data grid
        private void TableGrid_LoadTableGrid(ref Table tbl)
        {
            //The entire data for the grid is loaded in this generic list
            //To outer list is the list of rows. The inner list stored the list of fields for each specific row
            List<List<MaintField>> maintData = MaintenanceUtil.GetMaintData(CurrentUser, maint, false, PageIdx, pageLength);

            //Map settings to each field by index to avoid calling the GetMaintFieldSettings for each field for each row
            List<MaintFieldSettings> settings = new List<MaintFieldSettings>();

            foreach (MaintField field in maintData[0])
            {
                MaintFieldSettings maintFieldSettings = MaintenanceUtil.GetMaintFieldSettings(CurrentUser, maint, field.FieldName);
                settings.Add(maintFieldSettings);
            }

            int rowIdx = -1;

            //Iterate the rows
            foreach (List<MaintField> dataRow in maintData)
            {
                rowIdx++;
                TableRow tr = new TableRow();

                string KeyFieldValue = "";

                int idx = -1;

                //In each row iterate through the columns
                foreach (MaintField maintField in dataRow)
                {
                    //Get UIITems access rules
                    idx++;

                    bool uiItemsDisabled = false;
                    bool uiItemsHidden = false;

                    if (settings[idx] != null)
                    {
                        if (GetUIItemAccessLevel(ModuleKey + "_LISTMAINT") == UIAccessLevel.Disabled ||
                            GetUIItemAccessLevel(maint.UIKeyMaintenance) == UIAccessLevel.Disabled ||
                            GetUIItemAccessLevel(maint.UIKeyEdit) == UIAccessLevel.Disabled ||
                            GetUIItemAccessLevel(settings[idx].UIItemEditKey) == UIAccessLevel.Disabled)
                        {
                            uiItemsDisabled = true;
                        }

                        if (GetUIItemAccessLevel(ModuleKey + "_LISTMAINT") == UIAccessLevel.Hidden ||
                            GetUIItemAccessLevel(maint.UIKeyMaintenance) == UIAccessLevel.Hidden ||
                            GetUIItemAccessLevel(maint.UIKeyEdit) == UIAccessLevel.Hidden ||
                            GetUIItemAccessLevel(settings[idx].UIItemEditKey) == UIAccessLevel.Hidden)
                        {
                            uiItemsHidden = true;
                        }
                    }

                    //Create a new TableCell that would streo the specific field
                    TableCell tc = new TableCell();
                    tc.CssClass = "";

                    //Get teh FieldKey column
                    if (maintField.IsKeyField)
                        KeyFieldValue = maintField.FieldValue;

                    //If it is set to be ReadOnly then put a label instead of an input
                    if (maintField.IsReadOnly)
                    {
                        Label lblLabel = new Label();
                        lblLabel.Text = maintField.FieldValue;
                        lblLabel.CssClass = "ReadOnlyValue";
                        lblLabel.ID = maintField.FieldName + "_" + rowIdx.ToString();

                        if (uiItemsHidden)
                            HideControl(lblLabel);

                        tc.Controls.Add(lblLabel);
                    }
                    else if (maintField.Options.Count > 0) //If there are any Options then render a drop-down instead of a textbox
                    {
                        DropDownList ddList = new DropDownList();
                        ddList.CssClass = "InputField";
                        ddList.ID = maintField.FieldName + "_" + rowIdx.ToString();

                        //Add the drop-down options which are stored in the MaintField object (the Options property)
                        foreach (MaintFieldOption option in maintField.Options)
                        {
                            ddList.Items.Add(new ListItem(option.Label, option.Value));
                        }

                        ddList.SelectedValue = maintField.FieldValue;

                        //Disable the input if the user has no rights to edit it
                        if (uiItemsDisabled)
                            DisableControl(ddList);

                        if (uiItemsHidden)
                            HideControl(ddList);

                        tc.Controls.Add(ddList);
                    }
                    else if (settings[idx] != null && settings[idx].ValidateDataType == "bool") //If it is a bool field; then render a checkbox
                    {
                        CheckBox chk = new CheckBox();
                        chk.CssClass = "InputField";
                        chk.ID = maintField.FieldName + "_" + rowIdx.ToString();
                        chk.Checked = maintField.FieldValue == "1";

                        //Disable the input if the user has no rights to edit it
                        if (uiItemsDisabled)
                            DisableControl(chk);

                        if (uiItemsHidden)
                            HideControl(chk);

                        tc.Controls.Add(chk);
                    }
                    else //In all other cases redner a text-box
                    {
                        TextBox txtBox = new TextBox();
                        txtBox.Text = maintField.FieldValue;
                        txtBox.CssClass = (settings[idx] != null && settings[idx].IsMandatory ? "RequiredInputField" : "InputField");
                        txtBox.ID = maintField.FieldName + "_" + rowIdx.ToString();
                        txtBox.Style.Add("width", "95%");

                        if (maintField.ColumnSize.HasValue)
                            txtBox.MaxLength = maintField.ColumnSize.Value;

                        //Disable the input if the user has no rights to edit it
                        if (uiItemsDisabled)
                            DisableControl(txtBox);

                        if (uiItemsHidden)
                            HideControl(txtBox);

                        tc.Controls.Add(txtBox);
                    }

                    //If the column is Hidden then add a hidden fied that indicates this
                    if (maintField.IsHidden)
                    {
                        tc.Visible = false;

                        HiddenField hdnHiddenCell = new HiddenField();
                        hdnHiddenCell.Value = "true";
                        hdnHiddenCell.ID = maintField.FieldName + "HiddenCell_" + rowIdx.ToString();
                        tc.Controls.Add(hdnHiddenCell);
                    }

                    //Also store the old value of the input as a hidden field. 
                    //Later this would be used to compare the new value when generating the Audti Trail records
                    HiddenField hdnOldValue = new HiddenField();
                    hdnOldValue.Value = maintField.FieldValue;
                    hdnOldValue.ID = maintField.FieldName + "OldValue_" + rowIdx.ToString();
                    tc.Controls.Add(hdnOldValue);

                    tr.Cells.Add(tc);
                }

                //If there is an ability to Delete or Add then add the last cell that would store the buttons
                if (maint.CanDelete || maint.CanAdd)
                {
                    TableCell tcb = new TableCell();
                    tcb.Style.Add("text-align", "left");
                    tcb.Style.Add("padding-left", "8px");
                    tr.Cells.Add(tcb);
                }

                //If the Delete ability is allowes and ther aren't any related trecord then add the Delete button
                if (maint.CanDelete && !MaintenanceUtil.AreThereRelatedRecords(CurrentUser, KeyFieldValue, maint.MasterTable, maint.MasterField))
                {
                    if (GetUIItemAccessLevel(ModuleKey + "_LISTMAINT") == UIAccessLevel.Enabled &&
                        GetUIItemAccessLevel(maint.UIKeyMaintenance) == UIAccessLevel.Enabled &&
                        GetUIItemAccessLevel(maint.UIKeyDelete) == UIAccessLevel.Enabled)
                    {
                        for (int i = tr.Cells.Count - 1; i >= 0; i++)
                        {
                            if (tr.Cells[i].Visible)
                            {
                                ImageButton delBtn = new ImageButton();

                                delBtn.ID = "btnDelete_" + rowIdx.ToString();
                                delBtn.ImageUrl = "~/Images/delete.png";
                                delBtn.ToolTip = "Изтриване";
                                //This is used to display the Buttons when the page is loaded entirely to prevent any issues if the user click on a button, however, the JS function isn't loaded yet
                                delBtn.Attributes.Add("showonload", "true");
                                delBtn.Attributes.Add("style", "display: none;");

                                tr.Cells[i].Controls.Add(delBtn);

                                break;
                            }
                        }
                    }
                }

                tbl.Rows.Add(tr);
            }

            SetImgBtns();
        }

        //This function is used to add the "New" button on the last row in the grid
        //Also this function is now used to set the visibility of the Delete buttons
        private void TableGrid_AddRowButton(ref Table tbl)
        {
            //Set all delete buttons to be visible by default
            for (int i = 1; i < tbl.Rows.Count; i++)
            {
                Control tmp = tbl.Rows[i].Cells[tbl.Rows[i].Cells.Count - 1].FindControl("btnAddRow");

                if (tmp != null)
                    tbl.Rows[i].Cells[tbl.Rows[i].Cells.Count - 1].Controls.Remove(tmp);

                foreach (Control ctrl in tbl.Rows[i].Cells[tbl.Rows[i].Cells.Count - 1].Controls)
                {
                    if (ctrl is ImageButton && ctrl.ID.StartsWith("btnDelete_"))
                    {
                        ImageButton delBtn = (ImageButton)ctrl;

                        delBtn.Attributes.Add("showonload", "true");
                        delBtn.Attributes.Add("style", "display: none;");
                    }
                }
            }

            //If there could be added new rows then add the Add button
            if (maint.CanAdd)
            {
                if (GetUIItemAccessLevel(ModuleKey + "_LISTMAINT") == UIAccessLevel.Enabled &&
                    GetUIItemAccessLevel(maint.UIKeyMaintenance) == UIAccessLevel.Enabled &&
                    GetUIItemAccessLevel(maint.UIKeyAdd) == UIAccessLevel.Enabled)
                {
                    ImageButton img = new ImageButton();
                    
                    img.ID = "btnAddRow";
                    img.ImageUrl = "~/Images/addrow.gif";
                    img.ToolTip = "Нов ред";
                    img.Attributes.Add("showonload", "true");
                    img.Attributes.Add("style", "display: none;");

                    for (int i = tbl.Rows.Count - 1; i >= 1; i--)
                        if (tbl.Rows[i].Visible)
                        {
                            tbl.Rows[i].Cells[tbl.Rows[i].Cells.Count - 1].Controls.Add(img);
                            TableGrid_ReassignImgButtonsClick(tbl.Rows[i].Cells[tbl.Rows[i].Cells.Count - 1]);
                            break;
                        }
                }
            }

            //If there is only one visible row then hide the delete button
            int visibleRowsCnt = 0;
            int visibleRowIdx = 0;
            for (int i = tbl.Rows.Count - 1; i >= 1; i--)
                if (tbl.Rows[i].Visible)
                {
                    visibleRowsCnt++;
                    visibleRowIdx = i;
                }

            if (visibleRowsCnt == 1 && PageIdx == 1)
            {
                foreach (Control ctrl in tbl.Rows[visibleRowIdx].Cells[tbl.Rows[visibleRowIdx].Cells.Count - 1].Controls)
                {
                    if (ctrl is ImageButton && ctrl.ID.StartsWith("btnDelete_"))
                    {
                        ImageButton delBtn = (ImageButton)ctrl;

                        delBtn.Attributes.Add("showonload", "false");
                        delBtn.Attributes.Add("style", "display: none;");
                    }
                }
            }
        }

        //This is used to reassign the ImgButtons Click events
        //This should be done once the grid is loaded in the page, otherwise the Click events don't fire
        private void TableGrid_ReassignImgButtonsClick(System.Web.UI.Control currentControl)
        {
            foreach (System.Web.UI.Control tmp in currentControl.Controls)
            {
                if (tmp.ID != null)
                {
                    if (tmp.ID.StartsWith("btnDelete"))
                    {
                        ImageButton btn = (ImageButton)tmp;
                        btn.Click += new ImageClickEventHandler(TableGrid_lkDelete_Click);

                        if (String.IsNullOrEmpty(btn.Attributes["deletenewrow"]) ||
                            btn.Attributes["deletenewrow"] != "true")
                            btn.Attributes.Add("onclick", "ForceAsk();");
                        else
                            btn.Attributes.Add("onclick", "UndoForceAsk();");
                    }

                    if (tmp.ID == "btnAddRow")
                    {
                        ImageButton btn = (ImageButton)tmp;
                        btn.Click += new ImageClickEventHandler(TableGrid_lkAdd_Click);
                        btn.Attributes.Add("onclick", "ForceAsk();");
                    }
                }

                TableGrid_ReassignImgButtonsClick(tmp);
            }
        }

        //When clicking the Add New button then get the grid, add a new row and store the data grid back into the session
        private void TableGrid_lkAdd_Click(object sender, EventArgs e)
        {
            Table tbl = (Table)Session[TableGridSessionName];
            TableGrid_AddNewRow(ref tbl);
            TableGrid_AddRowButton(ref tbl);
            Session[TableGridSessionName] = tbl;
        }

        //Add a new row in the grid
        private void TableGrid_AddNewRow(ref Table tbl)
        {
            //Get the data for the grid, however, only the first row
            //Basically, this is used to get the structure of the column (like in the regular grid load function)
            List<List<MaintField>> maintData = MaintenanceUtil.GetMaintData(CurrentUser, maint, true, 0, 0);

            //Map settings to each field by index to avoid calling the GetMaintFieldSettings for each field for each row
            List<MaintFieldSettings> settings = new List<MaintFieldSettings>();

            foreach (MaintField field in maintData[0])
            {
                MaintFieldSettings maintFieldSettings = MaintenanceUtil.GetMaintFieldSettings(CurrentUser, maint, field.FieldName);
                settings.Add(maintFieldSettings);
            }

            int rowIdx = tbl.Rows.Count - 1;

            //Everything is the same as the normal grid load, however, the row is only one and the values should be blank
            foreach (List<MaintField> dataRow in maintData)
            {
                TableRow tr = new TableRow();

                int idx = -1;

                foreach (MaintField maintField in dataRow)
                {
                    //Get UIITems access rules
                    idx++;

                    bool uiItemsDisabled = false;
                    bool uiItemsHidden = false;

                    if (settings[idx] != null)
                    {
                        if (GetUIItemAccessLevel(ModuleKey + "_LISTMAINT") == UIAccessLevel.Disabled ||
                            GetUIItemAccessLevel(maint.UIKeyMaintenance) == UIAccessLevel.Disabled ||
                            GetUIItemAccessLevel(maint.UIKeyAdd) == UIAccessLevel.Disabled ||
                            GetUIItemAccessLevel(settings[idx].UIItemAddKey) == UIAccessLevel.Disabled)
                        {
                            uiItemsDisabled = true;
                        }

                        if (GetUIItemAccessLevel(ModuleKey + "_LISTMAINT") == UIAccessLevel.Hidden ||
                            GetUIItemAccessLevel(maint.UIKeyMaintenance) == UIAccessLevel.Hidden ||
                            GetUIItemAccessLevel(maint.UIKeyAdd) == UIAccessLevel.Hidden ||
                            GetUIItemAccessLevel(settings[idx].UIItemAddKey) == UIAccessLevel.Hidden)
                        {
                            uiItemsHidden = true;
                        }
                    }

                    TableCell tc = new TableCell();
                    tc.CssClass = "";

                    if (maintField.IsReadOnly)
                    {
                        Label lblLabel = new Label();
                        lblLabel.Text = "";
                        lblLabel.CssClass = "ReadOnlyValue";
                        lblLabel.ID = maintField.FieldName + "_" + rowIdx.ToString();

                        if (uiItemsHidden)
                            HideControl(lblLabel);

                        tc.Controls.Add(lblLabel);
                    }
                    else if (maintField.Options.Count > 0)
                    {
                        DropDownList ddList = new DropDownList();
                        ddList.CssClass = "InputField";
                        ddList.ID = maintField.FieldName + "_" + rowIdx.ToString();

                        foreach (MaintFieldOption option in maintField.Options)
                        {
                            ddList.Items.Add(new ListItem(option.Label, option.Value));
                        }

                        //Disable the input if the user has no rights to edit it
                        if (uiItemsDisabled)
                            DisableControl(ddList);

                        if (uiItemsHidden)
                            HideControl(ddList);

                        tc.Controls.Add(ddList);
                    }
                    else if (settings[idx] != null && settings[idx].ValidateDataType == "bool") //If it is a bool field; then render a checkbox
                    {
                        CheckBox chk = new CheckBox();
                        chk.CssClass = "InputField";
                        chk.ID = maintField.FieldName + "_" + rowIdx.ToString();
                        chk.Checked = false;

                        //Disable the input if the user has no rights to edit it
                        if (uiItemsDisabled)
                            DisableControl(chk);

                        if (uiItemsHidden)
                            HideControl(chk);

                        tc.Controls.Add(chk);
                    }
                    else
                    {
                        TextBox txtBox = new TextBox();
                        txtBox.Text = ""; //Note that when adding a New row the value is empty
                        txtBox.CssClass = (settings[idx] != null && settings[idx].IsMandatory ? "RequiredInputField" : "InputField");
                        txtBox.ID = maintField.FieldName + "_" + rowIdx.ToString();
                        txtBox.Style.Add("width", "95%");

                        if (maintField.ColumnSize.HasValue)
                            txtBox.MaxLength = maintField.ColumnSize.Value;

                        //Disable the input if the user has no rights to edit it
                        if (uiItemsDisabled)
                            DisableControl(txtBox);

                        if (uiItemsHidden)
                            HideControl(txtBox);

                        tc.Controls.Add(txtBox);
                    }

                    if (maintField.IsHidden)
                    {
                        tc.Visible = false;

                        HiddenField hdnHiddenCell = new HiddenField();
                        hdnHiddenCell.Value = "true";
                        hdnHiddenCell.ID = maintField.FieldName + "HiddenCell_" + rowIdx.ToString();
                        tc.Controls.Add(hdnHiddenCell);
                    }

                    HiddenField hdnOldValue = new HiddenField();
                    hdnOldValue.Value = "";
                    hdnOldValue.ID = maintField.FieldName + "OldValue_" + rowIdx.ToString();
                    tc.Controls.Add(hdnOldValue);

                    tr.Cells.Add(tc);
                }

                if (maint.CanDelete || maint.CanAdd)
                {
                    TableCell tcb = new TableCell();
                    tcb.Style.Add("text-align", "left");
                    tcb.Style.Add("padding-left", "8px");
                    tr.Cells.Add(tcb);
                }

                //The users should be always able to delet the new rows that haven't been saved yet
                for (int i = tr.Cells.Count - 1; i >= 0; i++)
                {
                    if (tr.Cells[i].Visible)
                    {
                        ImageButton delBtn = new ImageButton();

                        delBtn.ID = "btnDelete_" + rowIdx.ToString();
                        delBtn.ImageUrl = "~/Images/delete.png";
                        delBtn.ToolTip = "Изтриване";
                        delBtn.Attributes.Add("showonload", "true");
                        delBtn.Attributes.Add("style", "display: none;");
                        delBtn.Attributes.Add("deletenewrow", "true");

                        tr.Cells[i].Controls.Add(delBtn);

                        break;
                    }
                }

                tbl.Rows.Add(tr);
            }
        }

        //Save the data grid
        private string TableGrid_Save(ref Table tbl)
        {
            //Store the data from the from in a list of "row" object
            List<MaintRowForSave> maintData = new List<MaintRowForSave>();

            string[] arrFields = maint.FldList.Split(',');

            for (int i = 0; i < arrFields.Length; i++)
                arrFields[i] = arrFields[i].Replace("~", ",");

            for (int j = 1; j < tbl.Rows.Count; j++)
            {
                MaintRowForSave dataRow = new MaintRowForSave();

                bool NewRow = false;
                string KeyFieldValue = "";

                //If the key field is empty (missing ID) then it is a new row
                foreach (TableCell tc in tbl.Rows[j].Cells)
                    foreach (Control c in tc.Controls)
                        if (c.ID.StartsWith(maint.KeyFld + "_") && ((TextBox)c).Text == "")
                            NewRow = true;

                //Get the key field value (the ID of the record)
                foreach (TableCell tc in tbl.Rows[j].Cells)
                    foreach (Control c in tc.Controls)
                        if (c.ID.StartsWith(maint.KeyFld + "_") && c is TextBox)
                            KeyFieldValue = ((TextBox)c).Text;

                dataRow.KeyFieldValue = KeyFieldValue;
                dataRow.IsNewRow = NewRow;

                //Deleted row
                if (!tbl.Rows[j].Visible)
                {
                    //If this is a "deleted" new row then ignore it
                    if (NewRow)
                        break;

                    //Mark the particular record to be deleted
                    dataRow.IsDeleted = true;

                    //Keep the fields for the log (only the OldValue)
                    for (int i = 0; i < arrFields.Length; i++)
                    {
                        string field = arrFields[i].Trim().Substring(0, (arrFields[i].Trim().IndexOf('[') > 0 ? arrFields[i].Trim().IndexOf('[') : arrFields[i].Trim().Length));
                        string oldValue = "";
                        bool Hidden = false;
                        bool ReadOnly = false;

                        foreach (TableCell tc in tbl.Rows[j].Cells)
                            foreach (Control c in tc.Controls)
                                if (c.ID.StartsWith(field + "_") && c is Label)
                                {
                                    ReadOnly = true;
                                }
                                else if (c.ID.StartsWith(field + "OldValue_") && c is HiddenField)
                                {
                                    oldValue = ((HiddenField)c).Value;
                                }
                                else if (c.ID.StartsWith(field + "HiddenCell_") && c is HiddenField)
                                {
                                    Hidden = ((HiddenField)c).Value == "true";
                                }

                        if (!Hidden && !ReadOnly)
                        {
                            MaintField maintField = new MaintField();

                            maintField.FieldName = field;
                            maintField.OldValue = oldValue;

                            if (arrFields[i].Trim().ToUpper().Contains("[SELECT ")) //Check if there is provided a SELECT query that should populate a drop-down
                            {
                                maintField.Options = MaintenanceUtil.GetFieldOptions(CurrentUser, arrFields[i].Trim().Substring(arrFields[i].Trim().IndexOf("[") + 1, arrFields[i].Trim().LastIndexOf("]") - arrFields[i].Trim().IndexOf("[") - 1));
                            }

                            dataRow.MaintFields.Add(maintField);
                        }
                    }
                }
                else
                {
                    //Update an existing record
                    if (!NewRow)
                    {
                        //Iterate through the columns and get the value of each input
                        //Also, collect information about if the field is Hidden or ReadOnly, etc.
                        for (int i = 0; i < arrFields.Length; i++)
                        {
                            string field = arrFields[i].Trim().Substring(0, (arrFields[i].Trim().IndexOf('[') > 0 ? arrFields[i].Trim().IndexOf('[') : arrFields[i].Trim().Length));
                            string value = "";
                            string oldValue = "";
                            bool Hidden = false;
                            bool ReadOnly = false;

                            foreach (TableCell tc in tbl.Rows[j].Cells)
                                foreach (Control c in tc.Controls)
                                    if (c.ID.StartsWith(field + "_") && c is TextBox)
                                    {
                                        value = ((TextBox)c).Text;
                                    }
                                    else if (c.ID.StartsWith(field + "_") && c is Label)
                                    {
                                        value = ((Label)c).Text;
                                        ReadOnly = true;
                                    }
                                    else if (c.ID.StartsWith(field + "_") && c is DropDownList)
                                    {
                                        value = ((DropDownList)c).SelectedValue;
                                    }
                                    else if (c.ID.StartsWith(field + "_") && c is CheckBox)
                                    {
                                        value = ((CheckBox)c).Checked ? "1" : "0";
                                    }
                                    else if (c.ID.StartsWith(field + "OldValue_") && c is HiddenField)
                                    {
                                        oldValue = ((HiddenField)c).Value;
                                    }
                                    else if (c.ID.StartsWith(field + "HiddenCell_") && c is HiddenField)
                                    {
                                        Hidden = ((HiddenField)c).Value == "true";
                                    }


                            if (!Hidden && !ReadOnly)
                            {
                                MaintField maintField = new MaintField();

                                maintField.FieldValue = value;
                                maintField.FieldName = field;
                                maintField.OldValue = oldValue;

                                if (arrFields[i].Trim().ToUpper().Contains("[SELECT ")) //Check if there is provided a SELECT query that should populate a drop-down
                                {
                                    maintField.Options = MaintenanceUtil.GetFieldOptions(CurrentUser, arrFields[i].Trim().Substring(arrFields[i].Trim().IndexOf("[") + 1, arrFields[i].Trim().LastIndexOf("]") - arrFields[i].Trim().IndexOf("[") - 1));
                                }

                                dataRow.MaintFields.Add(maintField);
                            }
                        }
                    }
                    else //new record
                    {
                        //For the new records collect the fields and the values in a similar way
                        for (int i = 0; i < arrFields.Length; i++)
                        {
                            string field = arrFields[i].Trim().Substring(0, (arrFields[i].Trim().IndexOf('[') > 0 ? arrFields[i].Trim().IndexOf('[') : arrFields[i].Trim().Length));
                            string value = "";

                            foreach (TableCell tc in tbl.Rows[j].Cells)
                                foreach (Control c in tc.Controls)
                                    if (c.ID.StartsWith(field + "_") && c is TextBox)
                                        value = ((TextBox)c).Text;
                                    else if (c.ID.StartsWith(field + "_") && c is Label)
                                        value = ((Label)c).Text;
                                    else if (c.ID.StartsWith(field + "_") && c is DropDownList)
                                        value = ((DropDownList)c).SelectedValue;
                                    else if (c.ID.StartsWith(field + "_") && c is CheckBox)
                                        value = ((CheckBox)c).Checked ? "1" : "0";

                            if (field.Trim().ToUpper() != maint.KeyFld.Trim().ToUpper())
                            {
                                MaintField maintField = new MaintField();

                                maintField.FieldName = field;
                                maintField.FieldValue = value;

                                if (arrFields[i].Trim().ToUpper().Contains("[SELECT ")) //Check if there is provided a SELECT query that should populate a drop-down
                                {
                                    maintField.Options = MaintenanceUtil.GetFieldOptions(CurrentUser, arrFields[i].Trim().Substring(arrFields[i].Trim().IndexOf("[") + 1, arrFields[i].Trim().LastIndexOf("]") - arrFields[i].Trim().IndexOf("[") - 1));
                                }

                                dataRow.MaintFields.Add(maintField);
                            }
                        }
                    }
                }

                maintData.Add(dataRow);
            }

            //First validate the collected data
            string result = ValidateData(maintData);

            //If the grid data is valid then save it
            if (result == "OK")
            {
                //If there is provided a ChangeTypeKey for this Maintenance record then create a Change object and pass it to the Save method
                Change change = null;
                if (!String.IsNullOrEmpty(maint.ChangeTypeKey))
                    change = new Change(CurrentUser, maint.ChangeTypeKey);

                //Save the data
                MaintenanceUtil.SaveMaintData(CurrentUser, maint, maintData, change);

                //Write the changes into the Audit Trail
                if (change != null)
                    change.WriteLog();
            }

            return result;
        }

        private string ValidateData(List<MaintRowForSave> maintData)
        {
            string result = "";

            //Use this list of values when checking the unique columns
            List<List<string>> enteredValues = new List<List<string>>();

            //Map settings to each field by index to avoid calling the GetMaintFieldSettings for each field for each row
            List<MaintFieldSettings> settings = new List<MaintFieldSettings>();

            foreach (MaintField field in maintData[0].MaintFields)
            {
                MaintFieldSettings maintFieldSettings = MaintenanceUtil.GetMaintFieldSettings(CurrentUser, maint, field.FieldName);
                settings.Add(maintFieldSettings);

                enteredValues.Add(new List<string>());
            }

            //Iterate though the rows
            foreach (MaintRowForSave dataRow in maintData)
            {
                //Validate the row only if it isn't deleted
                if (!dataRow.IsDeleted)
                {
                    int idx = 0;

                    //Iterate the fields on the particular row
                    foreach (MaintField field in dataRow.MaintFields)
                    {
                        //Check if the field is Mandatory
                        if (settings[idx].IsMandatory)
                        {
                            if (String.IsNullOrEmpty(field.FieldValue))
                            {
                                //Check if the fields is empty, however, the user doesn't have enough rights to enter a value in it
                                if (GetUIItemAccessLevel(dataRow.IsNewRow ? settings[idx].UIItemAddKey : settings[idx].UIItemEditKey) != UIAccessLevel.Enabled)
                                    result = CommonFunctions.GetErrorMessageNoRights(new string[] { settings[idx].FieldLabel });
                                else
                                    result = CommonFunctions.GetErrorMessageMandatory(settings[idx].FieldLabel);

                                break;
                            }
                        }
                        
                        //Check if there is provided a specific data type (e.g. number, integer)
                        //and if there is a specific type then validate the entered value
                        if (!String.IsNullOrEmpty(settings[idx].ValidateDataType) && 
                            settings[idx].ValidateDataType.ToUpper() != "string".ToUpper())
                        {
                            string validateDataType = settings[idx].ValidateDataType.ToUpper();

                            switch (validateDataType)
                            {
                                case "NUMBER":
                                    {
                                        bool isValid = true;

                                        try
                                        {
                                            decimal.Parse(field.FieldValue);
                                        }
                                        catch
                                        {
                                            isValid = false;
                                        }

                                        if (!isValid && !String.IsNullOrEmpty(field.FieldValue))
                                            result = "В полето \"" + settings[idx].FieldLabel + "\" има невалидно число";

                                        break;
                                    }
                                case "INTEGER":
                                    {
                                        bool isValid = true;

                                        try
                                        {
                                            int.Parse(field.FieldValue);
                                        }
                                        catch
                                        {
                                            isValid = false;
                                        }

                                        if (!isValid && !String.IsNullOrEmpty(field.FieldValue))
                                            result = "В полето \"" + settings[idx].FieldLabel + "\" има невалидно цяло число";

                                        break;
                                    }
                            }

                            if (!String.IsNullOrEmpty(result))
                                break;
                        }

                        //If the column is set to be Unique then check the value on the current row if it isn't an already existing value in a previous row
                        if (settings[idx].IsUnique)
                        {
                            bool found = false;

                            foreach (string enteredValue in enteredValues[idx])
                            {
                                if (enteredValue == field.FieldValue.ToUpper())
                                {
                                    found = true;
                                    break;
                                }
                            }

                            if (found)
                            {
                                result = "Полето \"" + settings[idx].FieldLabel + "\" може да съдържа само уникални сойности";
                                break;
                            }
                        }

                        enteredValues[idx].Add(field.FieldValue.ToUpper());

                        idx++;
                    }

                    if (!String.IsNullOrEmpty(result))
                        break;
                }
            }

            //If there isn't an error message set the status to be OK
            if (String.IsNullOrEmpty(result))
                result = "OK";

            return result;
        }

        //Save click
        protected void TableGrid_Save_Click(object sender, EventArgs e)
        {
            //Get the table from the session
            Table tbl = (Table)Session[TableGridSessionName];
            string result = TableGrid_Save(ref tbl);

            //If everything is OK and the data is saved then reload the grid and put is back in the session
            if (result == "OK")
            {
                //Re-calculate the allRows number because after the save it could be different
                int allRows = MaintenanceUtil.GetMaintRecordsCnt(CurrentUser, maint);
                //Get the number of rows and calculate the number of pages in the grid
                maxPage = pageLength == 0 ? 1 : allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

                if (PageIdx > maxPage)
                    PageIdx = maxPage;

                tbl.Rows.Clear();
                TableGrid_AddTableGridHeader(ref tbl);
                TableGrid_LoadTableGrid(ref tbl);
                TableGrid_AddRowButton(ref tbl);
                Session[TableGridSessionName] = tbl;

                lblStatus.Text = "Записът е успешен";
                lblStatus.CssClass = "SuccessText";
                hdnSavedChanges.Value = "True";
            }
            else //If there is a problem (the Validation has failed) then show the error message
            {
                lblStatus.Text = result;
                lblStatus.CssClass = "ErrorText";
            }
        }

        //The Delete click
        protected void TableGrid_lkDelete_Click(object sender, EventArgs e)
        {
            //Get the table from the sessions
            Table tbl = (Table)Session[TableGridSessionName];
            ImageButton btnSender = (ImageButton)sender;
            string Row = btnSender.ID.Substring(btnSender.ID.IndexOf('_') + 1);
            int RowNumber = int.Parse(Row);
            //Set the row to be Invisible which means "Deleted"
            tbl.Rows[RowNumber + 1].Visible = false;
            TableGrid_AddRowButton(ref tbl);
            //Store the table grid back into the sessin object
            Session[TableGridSessionName] = tbl;
        }

        //When clicking Cancel then redirect to the Home screen
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ContentPages/Home.aspx");
        }


        //Go to the first page and refresh the grid
        protected void btnFirst_Click(object sender, EventArgs e)
        {
            PageIdx = 1;
            ReloadGrid();
        }

        //Go to the previous page and refresh the grid
        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (PageIdx > 1)
            {
                PageIdx = PageIdx - 1;
                ReloadGrid();
            }
        }

        //Go to the next page and refresh the grid
        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (PageIdx < maxPage)
            {
                PageIdx = PageIdx + 1;
                ReloadGrid();
            }
        }

        //Go to the last page and refresh the grid
        protected void btnLast_Click(object sender, EventArgs e)
        {
            PageIdx = maxPage;
            ReloadGrid();
        }


        //Go to a specific page (entered by the user) and refresh the grid
        protected void btnGoto_Click(object sender, EventArgs e)
        {
            int gotoPage;
            if (int.TryParse(txtGotoPage.Text, out gotoPage) && gotoPage > 0 && gotoPage <= maxPage)
            {
                PageIdx = gotoPage;
                ReloadGrid();
            }
        }

        //Refresh the paging image buttons
        private void SetImgBtns()
        {
            btnFirst.Enabled = true;
            btnPrev.Enabled = true;
            btnLast.Enabled = true;
            btnNext.Enabled = true;
            btnFirst.ImageUrl = "../Images/ButtonFirst.png";
            btnPrev.ImageUrl = "../Images/ButtonPrev.png";
            btnLast.ImageUrl = "../Images/ButtonLast.png";
            btnNext.ImageUrl = "../Images/ButtonNext.png";

            if (PageIdx == 1)
            {
                btnFirst.Enabled = false;
                btnPrev.Enabled = false;
                btnFirst.ImageUrl = "../Images/ButtonFirstDisabled.png";
                btnPrev.ImageUrl = "../Images/ButtonPrevDisabled.png";
            }

            if (PageIdx == maxPage)
            {
                btnLast.Enabled = false;
                btnNext.Enabled = false;
                btnLast.ImageUrl = "../Images/ButtonLastDisabled.png";
                btnNext.ImageUrl = "../Images/ButtonNextDisabled.png";
            }

            lblPagination.Text = " | " + PageIdx.ToString() + " от " + maxPage.ToString() + " | ";
            txtGotoPage.Text = "";
        }

        private void ReloadGrid()
        {
            //Get the table from the session
            Table tbl = (Table)Session[TableGridSessionName];
            tbl.Rows.Clear();
            TableGrid_AddTableGridHeader(ref tbl);
            TableGrid_LoadTableGrid(ref tbl);
            TableGrid_AddRowButton(ref tbl);
            Session[TableGridSessionName] = tbl;

            hdnSavedChanges.Value = "True";
        }
    }
}
