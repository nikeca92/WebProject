using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class ReportListTechnicsFromCommand : RESPage
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
                return "RES_REPORTS_REPORTLISTTECHNICSFROMCOMMAND";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if (GetUIItemAccessLevel("RES_REPORTS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_REPORTS_REPORTLISTTECHNICSFROMCOMMAND") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            //Setup any calendar control on the screen
            SetupDatePickers();

            //Hilight the current page in the menu bar
            HighlightMenuItems("Reports", "ReportListTechnicsFromCommand");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            //Setup some basic styles on the screen
            SetupStyle();

            //When the page is loaded for the first time
            if (!IsPostBack)
            {
                //Populate any drop-downs and list-boxes
                PopulateLists();

                //The default order is by module name
                if (string.IsNullOrEmpty(hdnSortBy.Value))
                    hdnSortBy.Value = "1";
            }

            //Collect the filter information to be able to pull the number of rows for this specific filter
            ReportListTechnicsFromCommandFilter filter = CollectFilterData();

            int allRows = ReportListTechnicsFromCommandBlockUtil.GetReportListTechnicsFromCommandBlockCount(filter, CurrentUser);
            //Get the number of rows and calculate the number of pages in the grid
            maxPage = pageLength == 0 ? 1 : allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);
            
            //When the page is loaded for the first time
            if (!IsPostBack)
            {
                //Simulate clicking the Refresh button to load the grid initially
                btnRefresh_Click(btnRefresh, new EventArgs());
            }

            lblMessage.Text = "";
            lblGridMessage.Text = "";
        }

        //Populate all listboxes on the screen
        private void PopulateLists()
        {
            PopulateMilitaryDepartment();
            PopulateMilitaryCommand();
        }

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

        private void PopulateMilitaryCommand()
        {
            ddMilitaryCommand.Items.Clear();
            ddMilitaryCommand.Items.Add(ListItems.GetOptionChooseOne());

            string militaryDepratment = "";

            if(ddMilitaryDepartment.SelectedValue != ListItems.GetOptionAll().Value)
                militaryDepratment = ddMilitaryDepartment.SelectedValue;

            List<MilitaryCommand> militaryCommands = MilitaryCommandUtil.GetMilitaryCommandsByMilitaryDepartmentForTechnics(CurrentUser, militaryDepratment);

            foreach (MilitaryCommand militaryCommand in militaryCommands)
            {
                ListItem li = new ListItem();
                li.Text = militaryCommand.DisplayTextForSelection;
                li.Value = militaryCommand.MilitaryCommandId.ToString();

                ddMilitaryCommand.Items.Add(li);
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


            //Collect the filters, the order control and the paging control data from the page
            int orderBy;
            if (!int.TryParse(hdnSortBy.Value, out orderBy))
                orderBy = 0;

            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            //Collect the filter information to be able to pull the number of rows for this specific filter
            ReportListTechnicsFromCommandFilter filter = CollectFilterData();

            //Get the list of records according to the specified filters, order and paging
            List<ReportListTechnicsFromCommandBlock> reportBlocks = ReportListTechnicsFromCommandBlockUtil.GetReportListTechnicsFromCommandBlockList(filter, CurrentUser);

            //No data found
            if (reportBlocks.Count == 0)
            {
                html = "<span>Няма намерени резултати</span>";

                if(ddMilitaryCommand.SelectedValue == ListItems.GetOptionChooseOne().Value)
                    html = "<span>Не е избрана команда</span>";

                this.btnPrintReportListTechnicsFromCommand.Visible = false;
                this.btnExport.Visible = false;
                this.pnlPaging.Visible = false;
            }
            //If there is data then generate dynamically the HTML for the data grid
            else 
            {
                this.btnPrintReportListTechnicsFromCommand.Visible = true;
                this.btnExport.Visible = true;
                this.pnlPaging.Visible = true;

                string headerStyle = "vertical-align: bottom; cursor: pointer;";
                int orderCol = (orderBy > 100 ? orderBy - 100 : orderBy);
                string img = orderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
                string[] arrOrderCol = { "", "", "", "", "", "" };
                arrOrderCol[orderCol - 1] = @"<div style='Position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

                //Setup the header of the grid
                html = @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px; vertical-align: bottom;' >№</th>
                               <th style='width: 20px;' ></th>
                               <th style='width: 110px; " + headerStyle + @"' onclick='SortTableBy(1);'>Подкоманда / Заявка №" + arrOrderCol[0] + @"</th>
                               <th style='width: 130px; " + headerStyle + @"' onclick='SortTableBy(2);'>Вид техника" + arrOrderCol[1] + @"</th>
                               <th style='width: 140px; " + headerStyle + @"' onclick='SortTableBy(6);'>Нормативна категория" + arrOrderCol[5] + @"</th>
                               <th style='width: 110px; " + headerStyle + @"' onclick='SortTableBy(3);'>Начин на явяване" + arrOrderCol[2] + @"</th>
                               <th style='width: 120px; " + headerStyle + @"' onclick='SortTableBy(4);'>Рег./Инв. номер" + arrOrderCol[3] + @"</th>
                               <th style='width: 230px; " + headerStyle + @"' onclick='SortTableBy(5);'>Собственик" + arrOrderCol[4] + @"</th>
                               <th style='width: 240px; " + headerStyle + @"' onclick=''>Адрес</th>
                            </tr>
                         </thead>";

                int counter = 1;

                //Iterate through all items and add them into the grid
                foreach (ReportListTechnicsFromCommandBlock reportBlock in reportBlocks)
                {
                    string cellStyle = "vertical-align: top;";

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='" + cellStyle + @"'>" + ((pageIdx - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'><img src='../Images/pawn_view.png' alt='Преглед' title='Преглед' class='GridActionIcon' onclick='PreviewTechnics(" + reportBlock.TechnicsId.ToString() + @");' /></td>
                                 <td style='" + cellStyle + @"'>" + reportBlock.MilitarySubCommand + @"</td>
                                 <td style='" + cellStyle + @"'>" + reportBlock.TechnicsType + @"</td>
                                 <td style='" + cellStyle + @"'>" + reportBlock.NormativeTechnics + @"</td>
                                 <td style='" + cellStyle + @"'>" + reportBlock.Readiness + @"</td>
                                 <td style='" + cellStyle + @"'>" + reportBlock.RegInvNumber + @"</td>
                                 <td style='" + cellStyle + @"'>" + reportBlock.Owner + @"</td>
                                 <td style='" + cellStyle + @"'>" + reportBlock.OwnerAddress + @"</td>
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
            
        }

        //Collect the filet information from the page
        private ReportListTechnicsFromCommandFilter CollectFilterData()
        {
            ReportListTechnicsFromCommandFilter filter = new ReportListTechnicsFromCommandFilter();

            string militaryDepartments = "";
            if (ddMilitaryDepartment.SelectedValue != ListItems.GetOptionAll().Value)
            {
                militaryDepartments = ddMilitaryDepartment.SelectedValue;
            }
            this.hdnMilitaryDepartmentId.Value = militaryDepartments;

            string militaryCommands = "";
            militaryCommands = ddMilitaryCommand.SelectedValue;
            this.hdnMilitaryCommandId.Value = ddMilitaryCommand.SelectedValue;

            int orderBy;
            if (!int.TryParse(hdnSortBy.Value, out orderBy))
                orderBy = 0;

            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            filter.MilitaryDepartments = militaryDepartments;
            filter.MilitaryCommands = militaryCommands;
            filter.OrderBy = orderBy;
            filter.PageIdx = pageIdx;
            filter.RowsPerPage = pageLength;

            return filter;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ddMilitaryDepartment.SelectedValue = ListItems.GetOptionAll().Value;
            ddMilitaryDepartment_Changed(ddMilitaryDepartment, new EventArgs());

            this.hdnMilitaryDepartmentId.Value = "";
            this.hdnMilitaryCommandId.Value = ddMilitaryCommand.SelectedValue;

            btnRefresh_Click(btnRefresh, new EventArgs());
        }

        protected void ddMilitaryDepartment_Changed(object sender, EventArgs e)
        {
            PopulateMilitaryCommand();
        }
    }
}
