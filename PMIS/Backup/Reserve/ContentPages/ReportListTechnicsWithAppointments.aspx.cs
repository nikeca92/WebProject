using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class ReportListTechnicsWithAppointments : RESPage
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
                return "RES_REPORTS_LISTTECHWITHAPPOINTMENTS";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if (GetUIItemAccessLevel("RES_REPORTS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            //Setup any calendar control on the screen
            SetupDatePickers();

            //Hilight the current page in the menu bar
            HighlightMenuItems("Reports", "ReportListTechnicsWithAppointments");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            //Setup some basic styles on the screen
            SetupStyle();

            //Collect the filter information to be able to pull the number of rows for this specific filter
            ReportListTechnicsWithAppointmentsFilter filter = CollectFilterData();

            int allRows = ReportListTechnicsWithAppointmentsBlockUtil.GetReportListTechnicsWithAppointmentsBlockCount(filter, CurrentUser);
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
                //btnRefresh_Click(btnRefresh, new EventArgs());
                this.btnPrintReportListTechnicsWithAppointments.Visible = false;
                this.btnExport.Visible = false;
                this.divNavigation.Visible = false;
                this.pnlSearchHint.Visible = true;
            }

            lblMessage.Text = "";
            lblGridMessage.Text = "";
        }

        //Populate all listboxes on the screen
        private void PopulateLists()
        {
            PopulateAdministrations();
            PopulateEquipWithTechRequestsStatuses();
            PopulateMilitaryDepartment();
        }

        //Populate the Administrations drop-down
        private void PopulateAdministrations()
        {
            ddAdministration.Items.Clear();
            ddAdministration.Items.Add(ListItems.GetOptionAll());

            List<Administration> administrations = AdministrationUtil.GetAllAdministrations(CurrentUser);

            foreach (Administration administration in administrations)
            {
                ListItem li = new ListItem();
                li.Text = administration.AdministrationName;
                li.Value = administration.AdministrationId.ToString();

                ddAdministration.Items.Add(li);
            }
        }

        //Populate the EquipWithTechRequestsStatuses drop-down
        private void PopulateEquipWithTechRequestsStatuses()
        {
            ddEquipWithTechRequestsStatus.Items.Clear();
            ddEquipWithTechRequestsStatus.Items.Add(ListItems.GetOptionAll());

            List<EquipWithTechRequestsStatus> statuses = EquipWithTechRequestsStatusUtil.GetAllEquipWithTechRequestsStatuses(CurrentUser);

            foreach (EquipWithTechRequestsStatus status in statuses)
            {
                ListItem li = new ListItem();
                li.Text = status.StatusName;
                li.Value = status.EquipWithTechRequestsStatusId.ToString();

                ddEquipWithTechRequestsStatus.Items.Add(li);
            }
        }

        //Populate the MilitaryUnits drop-down
        private void PopulateMilitaryDepartment()
        {
            ddMilitaryDepartment.Items.Clear();
            ddMilitaryDepartment.Items.Add(ListItems.GetOptionAll());

            List<MilitaryDepartment> militaryDepartments = MilitaryDepartmentUtil.GetAllMilitaryDepartmentsByEquipmentTechRequestsPerUser(CurrentUser, CurrentUser);

            foreach (MilitaryDepartment militaryDepartment in militaryDepartments)
            {
                ListItem li = new ListItem();
                li.Text = militaryDepartment.MilitaryDepartmentName;
                li.Value = militaryDepartment.MilitaryDepartmentId.ToString();

                ddMilitaryDepartment.Items.Add(li);
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
        private void RefreshReportItems()
        {
            string html = "";
            this.pnlSearchHint.Visible = false;


            //Collect the filters, the order control and the paging control data from the page
            int orderBy;
            if (!int.TryParse(hdnSortBy.Value, out orderBy))
                orderBy = 0;

            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            //Collect the filter information to be able to pull the number of rows for this specific filter
            ReportListTechnicsWithAppointmentsFilter filter = CollectFilterData();

            //Get the list of records according to the specified filters, order and paging
            List<ReportListTechnicsWithAppointmentsBlock> reportBlocks = ReportListTechnicsWithAppointmentsBlockUtil.GetReportListTechnicsWithAppointmentsBlockList(filter, pageLength, CurrentUser);

            //No data found
            if (reportBlocks.Count == 0)
            {
                html = "<span>Няма намерени резултати</span>";
                this.btnPrintReportListTechnicsWithAppointments.Visible = false;
                this.btnExport.Visible = false;
                this.divNavigation.Visible = false;
            }
            //If there is data then generate dynamically the HTML for the data grid
            else 
            {
                this.btnPrintReportListTechnicsWithAppointments.Visible = true;
                this.btnExport.Visible = true;
                this.divNavigation.Visible = false;

                bool isReqNumberHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_REQUEST_REQNUMBER") == UIAccessLevel.Hidden;
                bool isReqDateHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_REQUEST_REQDATE") == UIAccessLevel.Hidden;
                bool isMilUnitHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_REQUEST_MILUNIT") == UIAccessLevel.Hidden;
                bool isAdministrationHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_REQUEST_ADMINISTRATION") == UIAccessLevel.Hidden;
                bool isReqStatusHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_REQUEST_REQSTATUS") == UIAccessLevel.Hidden;

                string headerStyle = "vertical-align: bottom;";
                int orderCol = (orderBy > 100 ? orderBy - 100 : orderBy);
                string img = orderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
                string[] arrOrderCol = {"", "", "", "", ""};
                arrOrderCol[orderCol - 1] = @"<div style='Position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

                //Setup the header of the grid
                html = @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>" +
       (!isReqNumberHidden ? @"<th style='width: 110px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Заявка №" + arrOrderCol[0] + @"</th>" : "") +
         (!isReqDateHidden ? @"<th style='width: 110px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>От дата" + arrOrderCol[1] + @"</th>" : "") +
         (!isMilUnitHidden ? @"<th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>" + CommonFunctions.GetLabelText("MilitaryUnit") + arrOrderCol[2] + @"</th>" : "") +
  (!isAdministrationHidden ? @"<th style='width: 250px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(4);'>Министерство/Ведомство" + arrOrderCol[3] + @"</th>" : "") +
       (!isReqStatusHidden ? @"<th style='width: 200px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(5);'>Статус на заявката" + arrOrderCol[4] + @"</th>" : "") + @"
                               <th style='width: 30px;" + headerStyle + @"'>&nbsp;</th>
                            </tr>
                         </thead>";

                int counter = 1;

                //Iterate through all items and add them into the grid
                foreach (ReportListTechnicsWithAppointmentsBlock reportBlock in reportBlocks)
                {
                    string cellStyle = "vertical-align: top;";

                    string detailsHTML = "";

                    detailsHTML = "<img src='../Images/note_view.png' alt='Детайлна справка' title='Детайлна справка' class='GridActionIcon' style='width: 20px; height: 20px;' onclick='ShowDetails(" + reportBlock.EquipmentTechnicsRequestId.ToString() + ");' />";

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='" + cellStyle + @"'>" + ((pageIdx - 1) * pageLength + counter).ToString() + @"</td>" +
         (!isReqNumberHidden ? @"<td style='" + cellStyle + @"'>" + reportBlock.RequestNumber + @"</td>" : "") +
           (!isReqDateHidden ? @"<td style='" + cellStyle + @"'>" + reportBlock.RequestDate + @"</td>" : "") +
           (!isMilUnitHidden ? @"<td style='" + cellStyle + @"'>" + reportBlock.MilitaryUnit + @"</td>" : "") +
    (!isAdministrationHidden ? @"<td style='" + cellStyle + @"'>" + reportBlock.Administration + @"</td>" : "") +
         (!isReqStatusHidden ? @"<td style='" + cellStyle + @"'>" + reportBlock.RequestStatus + @"</td>" : "") + @"
                                 <td style='" + cellStyle + @"'>" + detailsHTML + @"</td>
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
                hdnRefreshReason.Value = "";
            }
        }

        //Refresh the data grid
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = "1";
                RefreshReportItems();
            }
        }

        //Go to the first page and refresh the grid
        protected void btnFirst_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = "1";
                RefreshReportItems();
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

                    RefreshReportItems();
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

                    RefreshReportItems();
                }
            }
        }

        //Go to the last page and refresh the grid
        protected void btnLast_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = maxPage.ToString();
                RefreshReportItems();
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
                    RefreshReportItems();
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

        //Setup any date picker controls on the page by setting the CSS of the target inputs
        //Note that the date picker CSS is common
        //This makes the calendar controls to appear on the page next to each input
        private void SetupDatePickers()
        {
            txtRequestDateFrom.CssClass = CommonFunctions.DatePickerCSS();
            txtRequestDateTo.CssClass = CommonFunctions.DatePickerCSS();
        }

        //Collect the filet information from the page
        private ReportListTechnicsWithAppointmentsFilter CollectFilterData()
        {
            ReportListTechnicsWithAppointmentsFilter filter = new ReportListTechnicsWithAppointmentsFilter();


            DateTime? requestDateFrom = null;

            if (CommonFunctions.TryParseDate(txtRequestDateFrom.Text))
            {
                requestDateFrom = CommonFunctions.ParseDate(txtRequestDateFrom.Text);
                this.hdnRequestDateFrom.Value = txtRequestDateFrom.Text;
            }

            DateTime? requestDateTo = null;

            if (CommonFunctions.TryParseDate(txtRequestDateTo.Text))
            {
                requestDateTo = CommonFunctions.ParseDate(txtRequestDateTo.Text);
                this.hdnRequestDateTo.Value = txtRequestDateTo.Text;
            }

            string militaryUnits = "";

            if (msMilitaryUnit.SelectedValue != ListItems.GetOptionAll().Value)
            {
                militaryUnits = msMilitaryUnit.SelectedValue;
                this.hdnMilitaryUnitId.Value = msMilitaryUnit.SelectedValue;
            }

            string administrations = "";

            if (ddAdministration.SelectedValue != ListItems.GetOptionAll().Value)
            {
                administrations = ddAdministration.SelectedValue;
                this.hdnAdministrationId.Value = ddAdministration.SelectedValue;
            }

            string equipWithTechRequestsStatuses = "";

            if (ddEquipWithTechRequestsStatus.SelectedValue != ListItems.GetOptionAll().Value)
            {
                equipWithTechRequestsStatuses = ddEquipWithTechRequestsStatus.SelectedValue;
                this.hdnEquipWithTechRequestsStatusId.Value = ddEquipWithTechRequestsStatus.SelectedValue;
            }

            string militaryDepartments = "";

            if (ddMilitaryDepartment.SelectedValue != ListItems.GetOptionAll().Value)
            {
                militaryDepartments = ddMilitaryDepartment.SelectedValue;
                this.hdnMilitaryDepartmentId.Value = ddMilitaryDepartment.SelectedValue;
            }

            int orderBy;
            if (!int.TryParse(hdnSortBy.Value, out orderBy))
                orderBy = 0;

            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            this.hdnRequestNumber.Value = this.txtRequestNumber.Text;

            filter.RequestNum = txtRequestNumber.Text;
            filter.RequestDateFrom = requestDateFrom;
            filter.RequestDateTo = requestDateTo;
            filter.MilitaryUnits = militaryUnits;
            filter.Administrations = administrations;
            filter.EquipWithTechRequestsStatuses = equipWithTechRequestsStatuses;
            filter.MilitaryDepartments = militaryDepartments;
            filter.OrderBy = orderBy;
            filter.PageIdx = pageIdx;

            return filter;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtRequestNumber.Text = "";
            txtRequestDateFrom.Text = "";
            txtRequestDateTo.Text = "";
            msMilitaryUnit.SelectedText = "";
            msMilitaryUnit.SelectedValue = ListItems.GetOptionAll().Value;
            ddAdministration.SelectedValue = ListItems.GetOptionAll().Value;
            ddEquipWithTechRequestsStatus.SelectedValue = ListItems.GetOptionAll().Value;
            ddMilitaryDepartment.SelectedValue = ListItems.GetOptionAll().Value;
        }
    }
}
