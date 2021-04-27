using System;
using System.Collections.Generic;

using PMIS.Common;
using PMIS.HealthSafety.Common;

namespace HealthSafety.ContentPages
{
    public partial class ManageUnsafeWorkingConditionsNotices : HSPage
    {
        //Get the config setting that says how many rows per page should be dispayed in the grid
        private int pageLength = int.Parse(Config.GetWebSetting("RowsPerPage"));
        //Stores information about how many pages are in the grid
        private int maxPage;

        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "HS_UNSAFEWCONDNOTICE";
            }
        }
        private string MilitaryUnitLabel = CommonFunctions.GetLabelText("MilitaryUnit");

        protected void Page_Load(object sender, EventArgs e)
        {
            //Set Label Text
            SetLabelValueText();

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteUnsafeWConditionsNotice")
            {
                this.JSDeleteNotice();
                return;
            }

            //Hilight the current page in the menu bar
            HighlightMenuItems("Accidents", "Accidents_SearchUnsafeWorkingConditionsNotices");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            jsMilitaryUnitSelectorDiv.InnerHtml = @"<script src=""" + Page.ClientScript.GetWebResourceUrl(typeof(MilitaryUnitSelector.MilitaryUnitSelector), "MilitaryUnitSelector.MilitaryUnitSelector.js") + @""" type=""text/javascript""></script>";

            //Setup any calendar control on the screen
            this.SetupDatePickers();

            if (!IsPostBack)
            {
                //Pre-fill the date fields with the today's date
                this.txtDateFrom.Text = CommonFunctions.FormatDate(DateTime.Now);
                this.txtDateTo.Text = CommonFunctions.FormatDate(DateTime.Now);
            }

            //Collect the filter information to be able to pull the number of rows for this specific filter
            int? militaryUnitId = null;

            if (this.musMilitaryUnit.SelectedValue != "" && this.musMilitaryUnit.SelectedValue != ListItems.GetOptionAll().Value)
                militaryUnitId = Int32.Parse(this.musMilitaryUnit.SelectedValue);

            DateTime? dateFrom = null;

            if (CommonFunctions.TryParseDate(this.txtDateFrom.Text))
                dateFrom = CommonFunctions.ParseDate(this.txtDateFrom.Text);

            DateTime? dateTo = null;

            if (CommonFunctions.TryParseDate(this.txtDateTo.Text))
                dateTo = CommonFunctions.ParseDate(this.txtDateTo.Text);

            int allRows = UnsafeWorkingConditionsNoticeUtil.GetAllNoticesCount(this.txtNoticeNumber.Text, militaryUnitId, dateFrom, dateTo, CurrentUser);
            //Get the number of rows and calculate the number of pages in the grid
            maxPage = pageLength == 0 ? 1 : allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            //When the page is loaded for the first time
            if (!IsPostBack)
            {
                //Populate drop-down
                this.PopulateDropDownList();

                //The default order is by registration number
                if (string.IsNullOrEmpty(hdnSortBy.Value))
                    hdnSortBy.Value = "1";

                //Simulate clicking the Refresh button to load the grid initially
                this.btnRefresh_Click(btnRefresh, new EventArgs());
            }

            //setup AddNewNotice button
            this.SetUpAddNewNoticeButton();

            lblMessage.Text = "";
            lblGridMessage.Text = "";
        }

        //Populate the listbox in the filter
        private void PopulateDropDownList()
        {
            
        }

        //Setup any date picker controls on the page by setting the CSS of the target inputs
        //Note that the date picker CSS is common
        //This makes the calendar controls to appear on the page next to each input
        private void SetupDatePickers()
        {
            txtDateFrom.CssClass = CommonFunctions.DatePickerCSS();
            txtDateTo.CssClass = CommonFunctions.DatePickerCSS();
        }

        // Setup AddNewNotice button according to rights of user's role
        private void SetUpAddNewNoticeButton()
        {
            if (this.GetUIItemAccessLevel("HS_ADDUNSAFEWCONDNOTICE") == UIAccessLevel.Enabled && this.GetUIItemAccessLevel("HS_UNSAFEWCONDNOTICE") == UIAccessLevel.Enabled)
            {
                EnableButton(btnNew);
            }
            else
            {
                if (this.GetUIItemAccessLevel("HS_ADDUNSAFEWCONDNOTICE") == UIAccessLevel.Hidden)
                {
                    HideControl(btnNew);
                }
                else
                {
                    DisableButton(btnNew);
                }
            }
        }

        //Refresh the data grid
        private void RefreshNotices()
        {
            string html = "";


            //Collect the filters, the order control and the paging control data from the page
            int orderBy;
            if (!int.TryParse(hdnSortBy.Value, out orderBy))
                orderBy = 0;

            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            //Collect the filter information to be able to pull the number of rows for this specific filter
            int? militaryUnitId = null;

            if (this.musMilitaryUnit.SelectedValue != ListItems.GetOptionAll().Value)
            {
                militaryUnitId = Int32.Parse(this.musMilitaryUnit.SelectedValue);
                this.hfМilitaryUnitId.Value = this.musMilitaryUnit.SelectedValue;
            }
            else
            {
                this.hfМilitaryUnitId.Value = "";
            }

            DateTime? dateFrom = null;
            this.hfDateFrom.Value = "";

            if (CommonFunctions.TryParseDate(txtDateFrom.Text))
            {
                dateFrom = CommonFunctions.ParseDate(txtDateFrom.Text);
                this.hfDateFrom.Value = this.txtDateFrom.Text;
            }

            DateTime? dateTo = null;
            this.hfDateTo.Value = "";

            if (CommonFunctions.TryParseDate(txtDateTo.Text))
            {
                dateTo = CommonFunctions.ParseDate(txtDateTo.Text);
                this.hfDateTo.Value = this.txtDateTo.Text;
            }

            //Get the list of notices according to the specified filters, order and paging
            List<UnsafeWorkingConditionsNotice> notices = UnsafeWorkingConditionsNoticeUtil.GetAllUnsafeWorkingConditionsNotices(this.txtNoticeNumber.Text, militaryUnitId, dateFrom, dateTo, orderBy, pageIdx, pageLength, CurrentUser);

            //No data found
            if (notices.Count == 0)
            {
                html = "<span>Няма намерени резултати</span>";
                this.btnPrintAllUnsafeWCondNotices.Visible = false;
            }
            //If there is data then generate dynamically the HTML for the data grid
            else
            {
                this.btnPrintAllUnsafeWCondNotices.Visible = true;

                string headerStyle = "vertical-align: bottom;";
                int orderCol = (orderBy > 100 ? orderBy - 100 : orderBy);
                string img = orderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
                string[] arrOrderCol = { "", "", "" };
                arrOrderCol[orderCol - 1] = @"<div style='position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

                //Setup the header of the grid
                html = @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='vertical-align: middle; width: 20px;" + headerStyle + @"'>№</th>
                               <th style='vertical-align: middle; width: 160px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Сведение №" + arrOrderCol[0] + @"</th>
                               <th style='vertical-align: middle; width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>Дата на изготвяне" + arrOrderCol[1] + @"</th>
                               <th style='vertical-align: middle; width: 130px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>" + this.MilitaryUnitLabel + arrOrderCol[2] + @"</th>
                               <th style='width: 50px;" + headerStyle + @"'>&nbsp;</th>
                            </tr>
                         </thead>";

                int counter = 1;

                // Get the deleting right for notice
                bool isDeleteDisabled = (this.GetUIItemAccessLevel("HS_DELETEUNSAFEWCONDNOTICE") != UIAccessLevel.Enabled
                                        || this.GetUIItemAccessLevel("HS_UNSAFEWCONDNOTICE") != UIAccessLevel.Enabled);

                // Get the visible right for notice
                bool isEditHidden = (this.GetUIItemAccessLevel("HS_EDITUNSAFEWCONDNOTICE") == UIAccessLevel.Hidden) ||
                                      (this.GetUIItemAccessLevel("HS_UNSAFEWCONDNOTICE") == UIAccessLevel.Hidden);

                //Iterate through all items and add them into the grid
                foreach (UnsafeWorkingConditionsNotice notice in notices)
                {
                    string cellStyle = "vertical-align: top;";

                    string editHTML = "";
                    if (!isEditHidden)
                    {
                        editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='EditUnsafeWConditionsNotice(" + notice.UnsafeWConditionsNoticeId.ToString() + ");' />";
                    }

                    string deleteHTML = "";
                    if (!isDeleteDisabled)
                    {
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на това сведение' class='GridActionIcon' onclick='DeleteUnsafeWConditionsNotice(" + notice.UnsafeWConditionsNoticeId.ToString() + ");' />";
                    }

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td align='center' style='" + cellStyle + @"'>" + ((pageIdx - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + notice.NoticeNumber + @"</td>
                                 <td style='" + cellStyle + @"'>" + CommonFunctions.FormatDate(notice.NoticeDate) + @"</td>
                                 <td style='" + cellStyle + @"'>" + (notice.MilitaryUnit != null ? notice.MilitaryUnit.DisplayTextForSelection : "") + @"</td>
                                 <td align='center' style='" + cellStyle + @"'>" + editHTML + deleteHTML + @"</td>
                              </tr>";

                    counter++;
                }

                html += "</table>";
            }

            //Put the generated grid on the page
            this.pnlUnsafeWConditionsGrid.InnerHtml = html;

            //Refresh the paging button according to the current position
            this.SetImgBtns();
            this.lblPagination.Text = " | " + hdnPageIdx.Value + " от " + maxPage.ToString() + " | ";
            this.txtGotoPage.Text = "";

            //Set the message if there is a need (e.g. a deleted item)
            if (this.hdnRefreshReason.Value != "")
            {
                if (this.hdnRefreshReason.Value == "DELETED")
                {
                    this.lblGridMessage.Text = "Сведението беше изтрито успешно";
                    this.lblGridMessage.CssClass = "SuccessText";
                }

                this.hdnRefreshReason.Value = "";
            }
        }

        //Refresh the data grid
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            this.hdnPageIdx.Value = "1";
            this.RefreshNotices();
        }

        //Go to the first page and refresh the grid
        protected void btnFirst_Click(object sender, EventArgs e)
        {
            this.hdnPageIdx.Value = "1";
            this.RefreshNotices();
        }

        //Go to the previous page and refresh the grid
        protected void btnPrev_Click(object sender, EventArgs e)
        {
            int page = int.Parse(this.hdnPageIdx.Value);

            if (page > 1)
            {
                page--;
                this.hdnPageIdx.Value = page.ToString();

                this.RefreshNotices();
            }
        }

        //Go to the next page and refresh the grid
        protected void btnNext_Click(object sender, EventArgs e)
        {
            int page = int.Parse(this.hdnPageIdx.Value);

            if (page < maxPage)
            {
                page++;
                this.hdnPageIdx.Value = page.ToString();

                this.RefreshNotices();
            }
        }

        //Go to the last page and refresh the grid
        protected void btnLast_Click(object sender, EventArgs e)
        {
            this.hdnPageIdx.Value = this.maxPage.ToString();
            this.RefreshNotices();
        }

        //Go to a specific page (entered by the user) and refresh the grid
        protected void btnGoto_Click(object sender, EventArgs e)
        {
            int gotoPage;
            if (int.TryParse(this.txtGotoPage.Text, out gotoPage) && gotoPage > 0 && gotoPage <= maxPage)
            {
                this.hdnPageIdx.Value = gotoPage.ToString();
                this.RefreshNotices();
            }
        }

        //Refresh the paging image buttons
        private void SetImgBtns()
        {
            int page = int.Parse(hdnPageIdx.Value);

            this.btnFirst.Enabled = true;
            this.btnPrev.Enabled = true;
            this.btnLast.Enabled = true;
            this.btnNext.Enabled = true;
            this.btnFirst.ImageUrl = "../Images/ButtonFirst.png";
            this.btnPrev.ImageUrl = "../Images/ButtonPrev.png";
            this.btnLast.ImageUrl = "../Images/ButtonLast.png";
            this.btnNext.ImageUrl = "../Images/ButtonNext.png";

            if (page == 1)
            {
                this.btnFirst.Enabled = false;
                this.btnPrev.Enabled = false;
                this.btnFirst.ImageUrl = "../Images/ButtonFirstDisabled.png";
                this.btnPrev.ImageUrl = "../Images/ButtonPrevDisabled.png";
            }

            if (page == maxPage)
            {
                this.btnLast.Enabled = false;
                this.btnNext.Enabled = false;
                this.btnLast.ImageUrl = "../Images/ButtonLastDisabled.png";
                this.btnNext.ImageUrl = "../Images/ButtonNextDisabled.png";
            }
        }

        //Navigate back to the home screen
        protected void btnBack_Click(object sender, EventArgs e)
        {
            RedirectAccessDenied();
        }

        //Go to create a new record
        protected void btnNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ContentPages/AddEditUnsafeWorkingConditionsNotice.aspx");
        }

        //Delete risk notice (ajax call)
        private void JSDeleteNotice()
        {
            if (this.GetUIItemAccessLevel("HS_DELETEUNSAFEWCONDNOTICE") != UIAccessLevel.Enabled
                || this.GetUIItemAccessLevel("HS_UNSAFEWCONDNOTICE") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            int noticeId = int.Parse(Request.Form["UnsafeWConditionsNoticeID"]);

            string stat = "";
            string response = "";

            try
            {
                //Track the changes into the Audit Trail 
                Change change = new Change(CurrentUser, "HS_UnsafeWCondNotices");

                UnsafeWorkingConditionsNoticeUtil.DeleteUnsafeWorkingConditionsNotice(noticeId, CurrentUser, change);

                change.WriteLog();

                stat = AJAXTools.OK;
                response = "<response>OK</response>";
            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }

            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }
        private void SetLabelValueText()
        {
            this.lblMilitaryUnit.Text = this.MilitaryUnitLabel + ":";

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            musMilitaryUnit.SelectedText = "";
            musMilitaryUnit.SelectedValue = ListItems.GetOptionAll().Value;
            txtNoticeNumber.Text = "";
            txtDateFrom.Text = "";
            txtDateTo.Text = "";
        }
    }
}
