using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using PMIS.Common;
using PMIS.Applicants.Common;

namespace PMIS.Applicants.ContentPages
{
    public partial class ManageVacancyAnnounces : APPLPage
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
                return "APPL_VACANN";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if (GetUIItemAccessLevel("APPL_VACANN") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            SetBtnNew();

            //Setup any calendar control on the screen
            SetupDatePickers();

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteVacancyAnnounce")
            {
                JSDeleteVacancyAnnounce();
                return;
            }

            //Hilight the current page in the menu bar
            HighlightMenuItems("VacancyAnnounces", "VacancyAnnounces_Search");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            //Setup some basic styles on the screen
            SetupStyle();

            //Collect the filter information to be able to pull the number of rows for this specific filter
            VacancyAnnouncesFilter filter = CollectFilterData();

            int allRows = VacancyAnnounceUtil.GetAllVacancyAnnouncesCount(filter, CurrentUser);
            //Get the number of rows and calculate the number of pages in the grid
            maxPage = pageLength == 0 ? 1 : allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);
            
            //When the page is loaded for the first time
            if (!IsPostBack)
            {
                //Populate any drop-downs and list-boxes
                PopulateLists();

                //The default order is by module name
                if (string.IsNullOrEmpty(hdnSortBy.Value))
                    hdnSortBy.Value = "1";

                //Simulate clicking the Refresh button to load the grid initially
                btnRefresh_Click(btnRefresh, new EventArgs());
            }

            lblMessage.Text = "";
            lblGridMessage.Text = "";
        }

        //Populate all listboxes on the screen
        private void PopulateLists()
        {
            PopulateVacancyAnnounceStatuses();
        }

        //Populate the VacancyAnnounceStatuses drop-down
        private void PopulateVacancyAnnounceStatuses()
        {
            ddVacancyAnnounceStatuses.Items.Clear();
            ddVacancyAnnounceStatuses.Items.Add(ListItems.GetOptionAll());

            List<VacancyAnnounceStatus> statuses = VacancyAnnounceStatusUtil.GetAllVacancyAnnounceStatuses(CurrentUser);

            foreach (VacancyAnnounceStatus status in statuses)
            {
                ListItem li = new ListItem();
                li.Text = status.VacAnnStatusName;
                li.Value = status.VacancyAnnouncesStatusId.ToString();

                ddVacancyAnnounceStatuses.Items.Add(li);
            }
        }

        //Setup some styling on the page
        private void SetupStyle()
        {

        }

        //Validate some field on the screen
        private bool ValidateData()
        {
            bool isDataValid = true;
            string errMsg = "";

            if (!isDataValid)
            {
                lblMessage.CssClass = "ErrorText";
                lblMessage.Text = errMsg;
            }

            return isDataValid;
        }

        //Refresh the data grid
        private void RefreshVacancyAnnounces()
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
            VacancyAnnouncesFilter filter = CollectFilterData();

            //Get the list of records according to the specified filters, order and paging
            List<VacancyAnnounce> vacancyAnnounces = VacancyAnnounceUtil.GetAllVacancyAnnounces(filter, pageLength, CurrentUser);

            //No data found
            if (vacancyAnnounces.Count == 0)
            {
                html = "<span>Няма намерени резултати</span>";
                this.btnPrintAllVacancyAnnounces.Visible = false;
            }
            //If there is data then generate dynamically the HTML for the data grid
            else 
            {
                this.btnPrintAllVacancyAnnounces.Visible = true;

                string headerStyle = "vertical-align: bottom;";
                int orderCol = (orderBy > 100 ? orderBy - 100 : orderBy);
                string img = orderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
                string[] arrOrderCol = {"", "", "", "", ""};
                arrOrderCol[orderCol - 1] = @"<div style='position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

                //Setup the header of the grid
                html = @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='width: 180px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Заповед №" + arrOrderCol[0] + @"</th>
                               <th style='width: 110px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>От дата" + arrOrderCol[1] + @"</th>
                               <th style='width: 110px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>Крайна дата (за подаване на документи)" + arrOrderCol[2] + @"</th>
                               <th style='width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(4);'>Брой длъжности" + arrOrderCol[3] + @"</th>
                               <th style='width: 220px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(5);'>Статус" + arrOrderCol[4] + @"</th>
                               <th style='width: 60px;" + headerStyle + @"'>&nbsp;</th>
                            </tr>
                         </thead>";

                int counter = 1;

                //Iterate through all items and add them into the grid
                foreach (VacancyAnnounce vacancyAnnounce in vacancyAnnounces)
                {
                    string cellStyle = "vertical-align: top;";

                    string deleteHTML = "";

                    if (vacancyAnnounce.CanDelete)
                    {
                        if (GetUIItemAccessLevel("APPL_VACANN") == UIAccessLevel.Enabled &&
                            GetUIItemAccessLevel("APPL_VACANN_DELETEVACANN") == UIAccessLevel.Enabled
                            )
                            deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на този конкурс' class='GridActionIcon' onclick='DeleteVacancyAnnounce(" + vacancyAnnounce.VacancyAnnounceId.ToString() + ");' />";
                    }

                    string editHTML = "";

                    if (GetUIItemAccessLevel("APPL_VACANN_EDITVACANN") != UIAccessLevel.Hidden)
                        editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='EditVacancyAnnounce(" + vacancyAnnounce.VacancyAnnounceId.ToString() + ");' />";

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='" + cellStyle + @"'>" + ((pageIdx - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + vacancyAnnounce.OrderNum + @"</td>
                                 <td style='" + cellStyle + @"'>" + CommonFunctions.FormatDate(vacancyAnnounce.OrderDate) + @"</td>
                                 <td style='" + cellStyle + @"'>" + CommonFunctions.FormatDate(vacancyAnnounce.EndDate) + @"</td>
                                 <td style='" + cellStyle + @"'>" + vacancyAnnounce.MaxPositions.ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + vacancyAnnounce.VacancyAnnounceStatus.VacAnnStatusName + @"</td>
                                 <td style='" + cellStyle + @"'>" + editHTML + deleteHTML + @"</td>
                              </tr>";

                    counter++;
                }

                html += "</table>";
            }

            //Put the generated grid on the page
            pnlDataGrid.InnerHtml = html;

            //Refresh the paging button according to the current position
            SetImgBtns();
            lblPagination.Text = " | " + hdnPageIdx.Value + " от " + maxPage.ToString() + " | ";
            txtGotoPage.Text = "";

            //Set the message if there is a need (e.g. a deleted item)
            if (hdnRefreshReason.Value != "")
            {
                if (hdnRefreshReason.Value == "DELETED")
                {
                    lblGridMessage.Text = "Конкурсът беше изтрит успешно";
                    lblGridMessage.CssClass = "SuccessText";
                }

                hdnRefreshReason.Value = "";
            }
        }

        //Refresh the data grid
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = "1";
                RefreshVacancyAnnounces();
            }
        }

        //Go to the first page and refresh the grid
        protected void btnFirst_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = "1";
                RefreshVacancyAnnounces();
            }
        }

        //Go to the previous page and refresh the grid
        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                int page = int.Parse(hdnPageIdx.Value);

                if (page > 1)
                {
                    page--;
                    hdnPageIdx.Value = page.ToString();

                    RefreshVacancyAnnounces();
                }
            }
        }

        //Go to the next page and refresh the grid
        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                int page = int.Parse(hdnPageIdx.Value);

                if (page < maxPage)
                {
                    page++;
                    hdnPageIdx.Value = page.ToString();

                    RefreshVacancyAnnounces();
                }
            }
        }

        //Go to the last page and refresh the grid
        protected void btnLast_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = maxPage.ToString();
                RefreshVacancyAnnounces();
            }
        }


        //Go to a specific page (entered by the user) and refresh the grid
        protected void btnGoto_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                int gotoPage;
                if (int.TryParse(txtGotoPage.Text, out gotoPage) && gotoPage > 0 && gotoPage <= maxPage)
                {
                    hdnPageIdx.Value = gotoPage.ToString();
                    RefreshVacancyAnnounces();
                }
            }
        }

        //Refresh the paging image buttons
        private void SetImgBtns()
        {
            int page = int.Parse(hdnPageIdx.Value);

            btnFirst.Enabled = true;
            btnPrev.Enabled = true;
            btnLast.Enabled = true;
            btnNext.Enabled = true;
            btnFirst.ImageUrl = "../Images/ButtonFirst.png";
            btnPrev.ImageUrl = "../Images/ButtonPrev.png";
            btnLast.ImageUrl = "../Images/ButtonLast.png";
            btnNext.ImageUrl = "../Images/ButtonNext.png";

            if (page == 1)
            {
                btnFirst.Enabled = false;
                btnPrev.Enabled = false;
                btnFirst.ImageUrl = "../Images/ButtonFirstDisabled.png";
                btnPrev.ImageUrl = "../Images/ButtonPrevDisabled.png";
            }

            if (page == maxPage)
            {
                btnLast.Enabled = false;
                btnNext.Enabled = false;
                btnLast.ImageUrl = "../Images/ButtonLastDisabled.png";
                btnNext.ImageUrl = "../Images/ButtonNextDisabled.png";
            }
        }

        //Navigate back to the home screen
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ContentPages/Home.aspx");
        }

        //Go to create a new record
        protected void btnNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ContentPages/AddEditVacancyAnnounce.aspx");
        }

        //Delete a record (ajax call)
        private void JSDeleteVacancyAnnounce()
        {
            if (GetUIItemAccessLevel("APPL_VACANN") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("APPL_VACANN_DELETEVACANN") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            int VacancyAnnounceId = int.Parse(Request.Form["VacancyAnnounceId"]);

            string stat = "";
            string response = "";
            
            try
            {
                Change change = new Change(CurrentUser, "APPL_VacancyAnnounces");

                VacancyAnnounceUtil.DeleteVacancyAnnounce(VacancyAnnounceId, CurrentUser, change);

                change.WriteLog();

                stat = AJAXTools.OK;
                response = "<response>OK</response>";
            }
            catch(Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }


            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        private void SetBtnNew()
        {
            if (this.GetUIItemAccessLevel("APPL_VACANN_ADDVACANN") == UIAccessLevel.Enabled && this.GetUIItemAccessLevel("APPL_VACANN") == UIAccessLevel.Enabled)
            {
                EnableButton(btnNew);
            }
            else
            {
                if (this.GetUIItemAccessLevel("APPL_VACANN_ADDVACANN") == UIAccessLevel.Hidden)
                {
                    HideControl(btnNew);
                }
                else
                {
                    DisableButton(btnNew);
                }
            }
        }

        //Setup any date picker controls on the page by setting the CSS of the target inputs
        //Note that the date picker CSS is common
        //This makes the calendar controls to appear on the page next to each input
        private void SetupDatePickers()
        {
            txtOrderDateFrom.CssClass = CommonFunctions.DatePickerCSS();
            txtOrderDateTo.CssClass = CommonFunctions.DatePickerCSS();
            txtEndDateFrom.CssClass = CommonFunctions.DatePickerCSS();
            txtEndDateTo.CssClass = CommonFunctions.DatePickerCSS();
        }

        //Collect the filet information from the page
        private VacancyAnnouncesFilter CollectFilterData()
        {
            VacancyAnnouncesFilter filter = new VacancyAnnouncesFilter();

            string vacancyAnnounceStatuses = "";

            this.hdnOrderNum.Value = txtOrderNum.Text;

            if (ddVacancyAnnounceStatuses.SelectedValue != ListItems.GetOptionAll().Value)
            {
                vacancyAnnounceStatuses = ddVacancyAnnounceStatuses.SelectedValue;
                this.hdnVacAnnStatus.Value = vacancyAnnounceStatuses;
            }
            else
            {
                this.hdnVacAnnStatus.Value = "";
            }

            DateTime? orderDateFrom = null;

            if (CommonFunctions.TryParseDate(txtOrderDateFrom.Text))
            {
                orderDateFrom = CommonFunctions.ParseDate(txtOrderDateFrom.Text);
                this.hdnOrderDateFrom.Value = txtOrderDateFrom.Text;
            }
            else
            {
                this.hdnOrderDateFrom.Value = "";
            }

            DateTime? orderDateTo = null;

            if (CommonFunctions.TryParseDate(txtOrderDateTo.Text))
            {
                orderDateTo = CommonFunctions.ParseDate(txtOrderDateTo.Text);
                this.hdnOrderDateTo.Value = txtOrderDateTo.Text;
            }
            else
            {
                this.hdnOrderDateTo.Value = "";
            }

            DateTime? endDateFrom = null;

            if (CommonFunctions.TryParseDate(txtEndDateFrom.Text))
            {
                endDateFrom = CommonFunctions.ParseDate(txtEndDateFrom.Text);
                this.hdnEndDateFrom.Value = txtEndDateFrom.Text;
            }
            else
            {
                this.hdnEndDateFrom.Value = "";
            }

            DateTime? endDateTo = null;

            if (CommonFunctions.TryParseDate(txtEndDateTo.Text))
            {
                endDateTo = CommonFunctions.ParseDate(txtEndDateTo.Text);
                this.hdnEndDateTo.Value = txtEndDateTo.Text;
            }
            else
            {
                this.hdnEndDateTo.Value = "";
            }

            int orderBy;
            if (!int.TryParse(hdnSortBy.Value, out orderBy))
                orderBy = 0;

            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            filter.OrderNum = txtOrderNum.Text;
            filter.OrderDateFrom = orderDateFrom;
            filter.OrderDateTo = orderDateTo;
            filter.VacancyAnnounceStatuses = vacancyAnnounceStatuses;
            filter.EndDateFrom = endDateFrom;
            filter.EndDateTo = endDateTo;
            filter.OrderBy = orderBy;
            filter.PageIdx = pageIdx;

            return filter;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtOrderNum.Text = "";
            txtOrderDateFrom.Text = "";
            txtOrderDateTo.Text = "";
            ddVacancyAnnounceStatuses.SelectedValue = ListItems.GetOptionAll().Value;
            txtEndDateFrom.Text = "";
            txtEndDateTo.Text = "";            
        }
    }
}
