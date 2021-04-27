using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class ManageEquipmentReservistsRequestsFulfilment : RESPage
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
                return "RES_EQUIPRESREQUESTS_FULFIL";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if (GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            //Setup any calendar control on the screen
            SetupDatePickers();           

            //Hilight the current page in the menu bar
            HighlightMenuItems("Equipment", "ManageEquipmentReservistsRequestsFulfilment");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            //Setup some basic styles on the screen
            SetupStyle();

            //When the page is loaded for the first time
            if (!IsPostBack)
            {
                //Populate any drop-downs and list-boxes
                PopulateLists();
            }

            //Collect the filter information to be able to pull the number of rows for this specific filter
            EquipmentReservistsRequestsFilter filter = CollectFilterData();

            int allRows = EquipmentReservistsRequestUtil.GetAllEquipmentReservistsRequestForFulfilmentCount(filter, CurrentUser);
            //Get the number of rows and calculate the number of pages in the grid
            maxPage = pageLength == 0 ? 1 : allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);
            
            //When the page is loaded for the first time
            if (!IsPostBack)
            {
                //The default order is by module name
                if (string.IsNullOrEmpty(hdnSortBy.Value))
                    hdnSortBy.Value = "1";

                //Simulate clicking the Refresh button to load the grid initially
                //btnRefresh_Click(btnRefresh, new EventArgs());
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
            PopulateEquipWithResRequestsStatuses();
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

        //Populate the EquipWithResRequestsStatuses drop-down
        private void PopulateEquipWithResRequestsStatuses()
        {
            ddEquipWithResRequestsStatus.Items.Clear();
            ddEquipWithResRequestsStatus.Items.Add(ListItems.GetOptionAll());

            List<EquipWithResRequestsStatus> statuses = EquipWithResRequestsStatusUtil.GetAllEquipWithResRequestsStatuses(CurrentUser);

            foreach (EquipWithResRequestsStatus status in statuses)
            {
                ListItem li = new ListItem();
                li.Text = status.StatusName;
                li.Value = status.EquipWithResRequestsStatusId.ToString();

                ddEquipWithResRequestsStatus.Items.Add(li);
            }
        }        

        //Populate the MilitaryUnits drop-down
        private void PopulateMilitaryDepartment()
        {
            ddMilitaryDepartment.Items.Clear();

            List<MilitaryDepartment> militaryDepartments = MilitaryDepartmentUtil.GetAllMilitaryDepartments(CurrentUser);

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
        private void RefreshEquipmentReservistsRequests()
        {
            string html = "";
            this.pnlSearchHint.Visible = false;

            bool IsRequestNumberHidden = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL_REQUEST_REQUESTNUMBER") == UIAccessLevel.Hidden;
            bool IsRequestDateHidden = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL_REQUEST_REQUESTDATE") == UIAccessLevel.Hidden;
            bool IsMilitaryUnitHidden = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL_REQUEST_MILITARYUNIT") == UIAccessLevel.Hidden;
            bool IsAdministrationHidden = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL_REQUEST_ADMINISTRATION") == UIAccessLevel.Hidden;
            bool IsStatusHidden = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL_REQUEST_STATUS") == UIAccessLevel.Hidden;

            //Collect the filters, the order control and the paging control data from the page
            int orderBy;
            if (!int.TryParse(hdnSortBy.Value, out orderBy))
                orderBy = 0;

            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            //Collect the filter information to be able to pull the number of rows for this specific filter
            EquipmentReservistsRequestsFilter filter = CollectFilterData();

            //Get the list of records according to the specified filters, order and paging
            List<EquipmentReservistsRequest> equipmentReservistsRequests = EquipmentReservistsRequestUtil.GetAllEquipmentReservistsRequestForFulfilment(filter, pageLength, CurrentUser);

            //No data found
            if (equipmentReservistsRequests.Count == 0)
            {
                html = "<span>Няма намерени резултати</span>";
                this.divNavigation.Visible = false;
            }
            //If there is data then generate dynamically the HTML for the data grid
            else 
            {
                this.divNavigation.Visible = true;

                string headerStyle = "vertical-align: bottom;";
                int orderCol = (orderBy > 100 ? orderBy - 100 : orderBy);
                string img = orderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
                string[] arrOrderCol = {"", "", "", "", "", ""};
                arrOrderCol[orderCol - 1] = @"<div style='Position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

                //Setup the header of the grid
                html = @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th> " +
   (!IsRequestNumberHidden ? @"<th style='width: 110px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Заявка №" + arrOrderCol[0] + @"</th>" : "") +
     (!IsRequestDateHidden ? @"<th style='width: 110px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>От дата" + arrOrderCol[1] + @"</th>" : "") +
    (!IsMilitaryUnitHidden ? @"<th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>" + CommonFunctions.GetLabelText("MilitaryUnit") + arrOrderCol[2] + @"</th>" : "") +
  (!IsAdministrationHidden ? @"<th style='width: 250px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(4);'>Министерство/Ведомство" + arrOrderCol[3] + @"</th>" : "") +
          (!IsStatusHidden ? @"<th style='width: 200px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(5);'>Статус на заявката" + arrOrderCol[4] + @"</th>" : "") +
                             @"<th style='width: 110px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(6);'>Изпълн." + arrOrderCol[5] + @"</th>" +
                               @"<th style='width: 30px;" + headerStyle + @"'>&nbsp;</th>
                            </tr>
                         </thead>";

                int counter = 1;

                //Iterate through all items and add them into the grid
                foreach (EquipmentReservistsRequest equipmentReservistsRequest in equipmentReservistsRequests)
                {
                    string cellStyle = "vertical-align: top;";                  

                    string fulfilmentHTML = "";

                    if (GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL") != UIAccessLevel.Hidden)
                        fulfilmentHTML = "<img src='../Images/list_edit.png' alt='Изпълнение на заявка' title='Изпълнение на заявка' class='GridActionIcon' onclick='FulfilEquipmentReservists(" + equipmentReservistsRequest.EquipmentReservistsRequestId.ToString() + ", " + ddMilitaryDepartment.SelectedValue + ");' />";

                    string fulFil = "";
                    decimal fulFilPerc = 0;
                    decimal fulFilResPerc = 0;

                    if (equipmentReservistsRequest.ReservistsCount > 0)
                    {
                        fulFilPerc = (decimal)equipmentReservistsRequest.FulfilCount / (decimal)equipmentReservistsRequest.ReservistsCount * (decimal)100;
                        fulFilResPerc = (decimal)equipmentReservistsRequest.FulfilResCount / (decimal)equipmentReservistsRequest.ReservistsCount * (decimal)100;
                    }

                    fulFil = "<span title='" + ReadinessUtil.ReadinessName(1) + "' style='cursor: default;'>" + fulFilPerc.ToString("0.0") + "%</span> <span title='" + ReadinessUtil.ReadinessName(1) + "' style='cursor: default;'>(" + fulFilResPerc.ToString("0.0") + "%)</span>";

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='" + cellStyle + @"'>" + ((pageIdx - 1) * pageLength + counter).ToString() + @"</td> " +
     (!IsRequestNumberHidden ? @"<td style='" + cellStyle + @"'>" + equipmentReservistsRequest.RequestNumber + @"</td>" : "") +
       (!IsRequestDateHidden ? @"<td style='" + cellStyle + @"'>" + CommonFunctions.FormatDate(equipmentReservistsRequest.RequestDate) + @"</td>" : "") +
      (!IsMilitaryUnitHidden ? @"<td style='" + cellStyle + @"'>" + (equipmentReservistsRequest.MilitaryUnit == null ? "" : equipmentReservistsRequest.MilitaryUnit.DisplayTextForSelection) + @"</td>" : "") +
    (!IsAdministrationHidden ? @"<td style='" + cellStyle + @"'>" + (equipmentReservistsRequest.Administration == null ? "" : equipmentReservistsRequest.Administration.AdministrationName) + @"</td>" : "") +
            (!IsStatusHidden ? @"<td style='" + cellStyle + @"'>" + (equipmentReservistsRequest.EquipWithResRequestsStatus == null ? "" : equipmentReservistsRequest.EquipWithResRequestsStatus.StatusName) + @"</td>" : "") +
                               @"<td style='" + cellStyle + @"'>" + fulFil + @"</td>" +
                                 @"<td style='" + cellStyle + @"' align='center'>" + fulfilmentHTML + @"</td>
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
        }

        //Refresh the data grid
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = "1";
                RefreshEquipmentReservistsRequests();
            }
        }

        //Go to the first page and refresh the grid
        protected void btnFirst_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = "1";
                RefreshEquipmentReservistsRequests();
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

                    RefreshEquipmentReservistsRequests();
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

                    RefreshEquipmentReservistsRequests();
                }
            }
        }

        //Go to the last page and refresh the grid
        protected void btnLast_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = maxPage.ToString();
                RefreshEquipmentReservistsRequests();
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
                    RefreshEquipmentReservistsRequests();
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
        private EquipmentReservistsRequestsFilter CollectFilterData()
        {
            EquipmentReservistsRequestsFilter filter = new EquipmentReservistsRequestsFilter();


            DateTime? requestDateFrom = null;

            if (CommonFunctions.TryParseDate(txtRequestDateFrom.Text))
                requestDateFrom = CommonFunctions.ParseDate(txtRequestDateFrom.Text);

            DateTime? requestDateTo = null;

            if (CommonFunctions.TryParseDate(txtRequestDateTo.Text))
                requestDateTo = CommonFunctions.ParseDate(txtRequestDateTo.Text);

            string militaryUnits = "";

            if (msMilitaryUnit.SelectedValue != ListItems.GetOptionAll().Value)
                militaryUnits = msMilitaryUnit.SelectedValue;

            string administrations = "";

            if (ddAdministration.SelectedValue != ListItems.GetOptionAll().Value)
                administrations = ddAdministration.SelectedValue;

            string equipWithResRequestsStatuses = "";

            if (ddEquipWithResRequestsStatus.SelectedValue != ListItems.GetOptionAll().Value)
                equipWithResRequestsStatuses = ddEquipWithResRequestsStatus.SelectedValue;            

            string militaryDepartments = "";

            if (ddMilitaryDepartment.SelectedValue != ListItems.GetOptionAll().Value)
                militaryDepartments = ddMilitaryDepartment.SelectedValue;

            int orderBy;
            if (!int.TryParse(hdnSortBy.Value, out orderBy))
                orderBy = 0;

            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            filter.RequestNum = txtRequestNumber.Text;
            filter.RequestDateFrom = requestDateFrom;
            filter.RequestDateTo = requestDateTo;
            filter.CommandNum = txtCommandNum.Text;
            filter.MilitaryUnits = militaryUnits;
            filter.Administrations = administrations;
            filter.EquipWithResRequestsStatuses = equipWithResRequestsStatuses;
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
            txtCommandNum.Text = "";
            msMilitaryUnit.SelectedText = "";
            msMilitaryUnit.SelectedValue = ListItems.GetOptionAll().Value;
            ddAdministration.SelectedValue = ListItems.GetOptionAll().Value;
            ddEquipWithResRequestsStatus.SelectedValue = ListItems.GetOptionAll().Value;
            if (ddMilitaryDepartment.Items.Count > 0)
                ddMilitaryDepartment.SelectedIndex = 0;
            //btnRefresh_Click(sender, e);
        }
    }
}
