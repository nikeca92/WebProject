using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMIS.Common
{
    public class GTablePage : BasePage
    {
        // Get all items by specified GTableName (ajax call)
        public void JSGetGTableItems(bool errorExists, bool isAfterAdd, bool isAfterDelete)
        {
            string response = "";
            string status = "";

            if (!errorExists)
            {
                if (!String.IsNullOrEmpty(Request.Params["GTableName"]))
                {
                    string tableName = Request.Params["GTableName"];

                    Maintenance gTableMaintance = MaintenanceUtil.GetMaintenance(CurrentUser, ModuleKey + "_" + tableName);

                    if (GetUIItemAccessLevel(gTableMaintance.UIKeyMaintenance) == UIAccessLevel.Hidden)
                        RedirectAjaxAccessDenied();

                    try
                    {
                        response += "<response>" + AJAXTools.EncodeForXML(this.GenerateGTableByTableName(tableName, isAfterAdd, isAfterDelete, gTableMaintance)) + "</response>";
                        status = AJAXTools.OK;
                    }
                    catch
                    {
                        status += AJAXTools.ERROR;
                    }
                }
            }
            else
            {
                status += AJAXTools.ERROR;
            }

            AJAX a = new AJAX(response, status, Response);
            a.Write();
            Response.End();
        }

        //Delete GTable item by specified GTableName and GTableKey (ajax call)
        public void JSDeleteGTableItem()
        {
            if (!String.IsNullOrEmpty(Request.Params["GTableName"]) && !String.IsNullOrEmpty(Request.Params["GTableKey"]))
            {
                bool errorExists = false;
                string tableName = Request.Params["GTableName"];
                int tableKey = 0;
                Int32.TryParse(Request.Params["GTableKey"], out tableKey);

                Maintenance gTableMaintance = MaintenanceUtil.GetMaintenance(CurrentUser, ModuleKey + "_" + tableName);

                if (this.GetUIItemAccessLevel(gTableMaintance.UIKeyDelete) != UIAccessLevel.Enabled 
                    || !gTableMaintance.CanDelete)
                    RedirectAjaxAccessDenied();

                try
                {
                    //Track the changes into the Audit Trail 
                    Change change = new Change(CurrentUser, gTableMaintance.ChangeTypeKey);

                    GTableItemUtil.DeleteGTableItem(tableName, tableKey, ModuleKey, gTableMaintance, CurrentUser, change);

                    change.WriteLog();
                }
                catch
                {
                    errorExists = true;
                }

                this.JSGetGTableItems(errorExists, false, true);
            }
        }

        public void JSSaveGTableItem()
        {
            if (!String.IsNullOrEmpty(Request.Params["GTableName"])
                && !String.IsNullOrEmpty(Request.Params["GTableKey"])
                && !String.IsNullOrEmpty(Request.Params["GTableSeq"])
                && !String.IsNullOrEmpty(Request.Params["GTableValue"]))
            {
                bool errorExists = false;
                string tableName = Request.Params["GTableName"];

                int tableKey = 0;
                Int32.TryParse(Request.Params["GTableKey"], out tableKey);

                int tableSeq = 0;
                Int32.TryParse(Request.Params["GTableSeq"], out tableSeq);

                string tableValue = Request.Params["GTableValue"];

                Maintenance gTableMaintance = MaintenanceUtil.GetMaintenance(CurrentUser, ModuleKey + "_" + tableName);

                bool isAfterAdd = false;

                GTableItem gti = GTableItemUtil.GetTableItem(tableName, tableKey, ModuleKey, CurrentUser);
                if (gti == null)
                {
                    if (this.GetUIItemAccessLevel(gTableMaintance.UIKeyAdd) != UIAccessLevel.Enabled
                        || !gTableMaintance.CanAdd)
                        RedirectAjaxAccessDenied();

                    gti = new GTableItem(CurrentUser);
                    isAfterAdd = true;
                }
                else
                {
                    if (this.GetUIItemAccessLevel(gTableMaintance.UIKeyEdit) != UIAccessLevel.Enabled)
                        RedirectAjaxAccessDenied();
                }

                gti.TableName = tableName;
                gti.TableSeq = tableSeq;
                gti.TableValue = tableValue;

                try
                {
                    //Track the changes into the Audit Trail 
                    Change change = new Change(CurrentUser, gTableMaintance.ChangeTypeKey);

                    GTableItemUtil.SaveGTableItem(gti, ModuleKey, gTableMaintance, CurrentUser, change);

                    change.WriteLog();
                }
                catch
                {
                    errorExists = true;
                }

                this.JSGetGTableItems(errorExists, isAfterAdd, false);
            }
            else
            {
                string response = "";
                string status = AJAXTools.ERROR;

                AJAX a = new AJAX(response, status, Response);
                a.Write();
                Response.End();
            }
        }

        //Generate the GTable data grid
        public string GenerateGTableByTableName(string tableName, bool isAfterAdd, bool isAfterDelete, Maintenance gTableMaintance)
        {
            string orderByStr = Request.Params["GTableOrderBy"];
            string pageIdxStr = Request.Params["GTablePageIdx"];

            MaintFieldSettings mfsTableSeq = MaintenanceUtil.GetMaintFieldSettings(CurrentUser, gTableMaintance, "TableSeq");
            MaintFieldSettings mfsTableValue = MaintenanceUtil.GetMaintFieldSettings(CurrentUser, gTableMaintance, "TableValue");

            // Get the config setting that says how many rows per page should be dispayed in the grid
            int pageLength = int.Parse(Config.GetWebSetting("RowsPerPage"));
            // Stores information about how many pages are in the grid
            int maxPage;

            int allRows = GTableItemUtil.GetAllGTableItemsCountByTableName(tableName, ModuleKey, CurrentUser);
            // Get the number of rows and calculate the number of pages in the grid
            maxPage = pageLength == 0 ? 1 : allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            string tableTitle = GTableItemUtil.GetTableTitle(tableName, CurrentUser);

            string html = "";

            // Collect order control and the paging control data from the page
            int orderBy = 1;
            if (!String.IsNullOrEmpty(orderByStr))
            {
                int.TryParse(orderByStr, out orderBy);
            }

            int pageIdx = 1;
            if (!String.IsNullOrEmpty(pageIdxStr))
            {
                int.TryParse(pageIdxStr, out pageIdx);

                if (isAfterAdd && pageIdx < maxPage)
                {
                    pageIdx++;
                }
                else if (isAfterDelete)
                {
                    if (pageIdx > maxPage)
                    {
                        pageIdx--;
                    }

                }
            }

            // Get the list of GTable items according to the specified order and paging
            List<GTableItem> gTableItems = GTableItemUtil.GetAllGTableItemsByTableName(tableName, ModuleKey, orderBy, pageIdx, pageLength, CurrentUser);

            // No data found
            if (gTableItems.Count == 0)
            {
                html = "<span>Няма намерени резултати</span>";
            }
            // If there is data then generate dynamically the HTML for the data grid
            else
            {
                string headerStyle = "vertical-align: bottom;";
                int orderCol = (orderBy > 100 ? orderBy - 100 : orderBy);
                string img = orderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
                string[] arrOrderCol = { "", "" };
                arrOrderCol[orderCol - 1] = @"<div style='position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

                // Refresh the paging image buttons
                string btnFirst = "src='../Images/ButtonFirst.png'";
                string btnPrev = "src='../Images/ButtonPrev.png'";
                string btnLast = "src='../Images/ButtonLast.png'";
                string btnNext = "src='../Images/ButtonNext.png'";

                if (pageIdx == 1)
                {
                    btnFirst = "src='../Images/ButtonFirstDisabled.png' disabled='true'";
                    btnPrev = "src='../Images/ButtonPrevDisabled.png' disabled='true'";
                }

                if (pageIdx == maxPage)
                {
                    btnLast = "src='../Images/ButtonLastDisabled.png' disabled='true'";
                    btnNext = "src='../Images/ButtonNextDisabled.png' disabled='true'";
                }

                // Set current page number
                string gTablePagination = " | " + pageIdx + " от " + maxPage + " | ";

                // Setup the header of the grid
                html = @"<div style='min-height: 150px; margin-bottom: 10px;'>
                        <input type='hidden' id='hdnGTableOrderBy' value='" + orderBy + @"' />
                        <input type='hidden' id='hdnGTablePageIdx' value='" + pageIdx + @"' />
                        <input type='hidden' id='hdnGTableMaxPage' value='" + maxPage + @"' />

                        <span class='HeaderText'>" + tableTitle + @"</span><br /><br /><br />

                        <div style='text-align: center;'>
                           <div style='display: inline; position: relative; top: -10px;'>
                              <img id='btnGTableFirst' " + btnFirst + @" alt='Първа страница' title='Първа страница' class='PaginationButton' onclick=""BtnGTableFirstClick('" + tableName + @"');"" />
                              <img id='btnGTablePrev' " + btnPrev + @" alt='Предишна страница' title='Предишна страница' class='PaginationButton' onclick=""BtnGTablePrevClick('" + tableName + @"');"" />
                              <span id='lblGTablePagination' class='PaginationLabel'>" + gTablePagination + @"</span>
                              <img id='btnGTableNext' " + btnNext + @" alt='Следваща страница' title='Следваща страница' class='PaginationButton' onclick=""BtnGTableNextClick('" + tableName + @"');"" />
                              <img id='btnGTableLast' " + btnLast + @" alt='Последна страница' title='Последна страница' class='PaginationButton' onclick=""BtnGTableLastClick('" + tableName + @"');"" />
                              
                              <span style='padding: 0 30px'>&nbsp;</span>
                              <span style='text-align: right;'>Отиди на страница</span>
                              <input id='txtGTableGotoPage' class='InputField' type='text' style='width: 30px;' value='' />
                              <img id='btnGTableGoto' src='../Images/ButtonGoto.png' alt='Отиди на страница' title='Отиди на страница' class='PaginationButton' onclick=""BtnGTableGotoClick('" + tableName + @"');"" />
                           </div>
                        </div>

                        <table id='" + tableName + @"' class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='vertical-align: middle; width: " + mfsTableSeq.WidthPixels + @"px; cursor: pointer;" + headerStyle + @"' onclick=""SortGTableBy('" + tableName + @"', 1);"">" + mfsTableSeq.FieldLabel + arrOrderCol[0] + @"</th>
                               <th style='vertical-align: middle; width: " + mfsTableValue.WidthPixels + @"px; cursor: pointer;" + headerStyle + @"' onclick=""SortGTableBy('" + tableName + @"', 2);"">" + mfsTableValue.FieldLabel + arrOrderCol[1] + @"</th>
                               <th style='width: 80px;" + headerStyle + @"'>&nbsp;</th>
                            </tr>
                         </thead>";

                int counter = 1;

                // Get the deleting right for GTable item
                bool isDeleteDisabled = (GetUIItemAccessLevel(gTableMaintance.UIKeyDelete) != UIAccessLevel.Enabled
                                        || GetUIItemAccessLevel(gTableMaintance.UIKeyMaintenance) != UIAccessLevel.Enabled
                                        || !gTableMaintance.CanDelete);

                // Get the visible right for GTable item
                bool isEditHidden = (GetUIItemAccessLevel(gTableMaintance.UIKeyEdit) != UIAccessLevel.Enabled)
                                    || GetUIItemAccessLevel(gTableMaintance.UIKeyMaintenance) != UIAccessLevel.Enabled;

                // Get the adding right for GTable item
                bool isAddHidden = (GetUIItemAccessLevel(gTableMaintance.UIKeyAdd) != UIAccessLevel.Enabled)
                                    || !gTableMaintance.CanAdd;

                //Iterate through all items and add them into the grid
                foreach (GTableItem item in gTableItems)
                {
                    string cellStyle = "vertical-align: top;";

                    string editHTML = "";
                    if (!isEditHidden)
                    {
                        editHTML = @"<img mode='edit' src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick=""EditGTableItem(this, '" + item.TableName + "', '" + item.TableKey + "', '" + counter + @"');"" /><img mode='save' src='../Images/save.png' alt='Запис' title='Запис' class='GridActionIcon' onclick=""SaveGTableItem('" + item.TableName + "', '" + item.TableKey + "', '" + counter + @"');"" style='display: none;' /><img mode='cancel' src='../Images/cancel.png' alt='Отказ' title='Отказ' class='GridActionIcon' onclick=""CancelGTableItem('" + item.TableName + @"');"" style='display: none; margin-left: 5px;' />";
                    }

                    string deleteHTML = "";
                    if (!isDeleteDisabled && !isEditHidden && !MaintenanceUtil.AreThereRelatedRecords(CurrentUser, item.TableKey.ToString(), gTableMaintance.MasterTable, gTableMaintance.MasterField) && allRows > 1)
                    {
                        deleteHTML = @"<img mode='delete' src='../Images/delete.png' alt='Изтриване' title='Изтриване' style='margin-left: 5px;' class='GridActionIcon' onclick=""DeleteGTableItem(this, '" + item.TableName + "', '" + item.TableKey + "', '" + item.TableValue + @"');"" />";
                    }

                    html += @"<tr style='min-height: 17px;' class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td align='center' style='width: " + mfsTableSeq.WidthPixels + @"px; " + cellStyle + @"'><input class='RequiredInputField' id='InputItemSeq_" + item.TableKey + "' style='width: 95%; display: none;' type='text' maxlength='100' value='" + item.TableSeq + @"'/><span class='InputLabel' id='InputSpanSeq_" + item.TableKey + @"'>" + item.TableSeq + @"</td>
                                 <td valign='middle' style='width: " + mfsTableValue.WidthPixels + @"px; " + cellStyle + @"'><input class='RequiredInputField' id='InputItemValue_" + item.TableKey + "' style='width: 95%; display: none;' type='text' maxlength='4000' value='" + item.TableValue + @"'/><span class='InputLabel' id='InputSpanValue_" + item.TableKey + @"'>" + item.TableValue + @"</td>
                                 <td align='center' style='width: 80px; " + cellStyle + @"'>" + editHTML + deleteHTML + @"</td>
                              </tr>";

                    counter++;
                }

                html += "</table><br />";

                if (!isAddHidden && !isEditHidden)
                {
                    html += @"<center>
                            <table id='NewGTableItemTable'>
                                <tr id='trNewGTableItemImg' style='min-height: 17px;'>
                                    <td align='left' colspan='3' style='width: " + (80 + mfsTableSeq.WidthPixels + mfsTableValue.WidthPixels) + @"px;'>
                                        <img id='imgAddNewGTableItem' mode='add' style='cursor: pointer; float: left;' src='../Images/add_new.png' alt='Добавяне на нов' title='Добавяне на нов' onclick=""AddNewGTableItem('" + tableName + @"');"" />
                                    </td>
                                </tr>
                                <tr id='trNewGTableItemLabel' style='min-height: 17px; display: none;'>
                                    <td align='left' colspan='3' style='width: " + (80 + mfsTableSeq.WidthPixels + mfsTableValue.WidthPixels) + @"px;'>
                                        <span>Нов</span>
                                    </td>
                                </tr>
                                <tr id='trNewGTableItemFields' style='min-height: 17px; display: none;'>
                                    <td style='width: " + (mfsTableSeq.WidthPixels - 20) + @"px;'>
                                        <input class='RequiredInputField' id='NewGTableItemSeq' style='width: 95%;' type='text' maxlength='100' />
                                    </td>
                                    <td style='width: " + (20 + mfsTableValue.WidthPixels) + @"px;'>
                                        <input class='RequiredInputField' id='NewGTableItemValue' style='width: 95%;' type='text' maxlength='4000' />
                                    </td>
                                    <td style='width: 80px;'>
                                        <img mode='save' src='../Images/save.png' alt='Запис' title='Запис' class='GridActionIcon' onclick=""SaveNewGTableItem('" + tableName + @"');"" />
                                        <img mode='cancel' src='../Images/cancel.png' alt='Отказ' title='Отказ' class='GridActionIcon' onclick='CancelNewGTableItem();' />
                                    </td>
                                </tr>
                            </table>
                        </center>";
                }

                html += @"</div>
                          <span id='gTableMessage' style='display: none'></span><br />
                          <div id='btnCloseGTable' runat='server' class='Button' onclick='HideGTableLightBox();'><i></i><div style='width:70px; padding-left:5px;'>Затвори</div><b></b></div>";
            }

            return html;
        }
    }
}
