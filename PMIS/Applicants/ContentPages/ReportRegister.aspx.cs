using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;

using PMIS.Common;
using PMIS.Applicants.Common;

namespace PMIS.Applicants.ContentPages
{
    public partial class ReportRegister : APPLPage
    {
        //Get the config setting that says how many rows per page should be dispayed in the grid
        private int rowsPerPage = int.Parse(Config.GetWebSetting("RowsPerPage"));
        //Stores information about how many pages are in the grid
        private int maxPage;

        ReportRegisterFilter filter;

        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "APPL_REPORTS_REGISTER";
            }
        }

        UIAccessLevel l;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if (GetUIItemAccessLevel("APPL_REPORTS_REGISTER") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            //Hilight the current page in the menu bar
            HighlightMenuItems("Reports", "ReportRegister");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            //Fill the filter information to be able to pull the number of rows for this specific filter
            filter = CollectFilterData();

            //Count Rows depends of filter data
            int allRows = ReportRegisterUtil.GetReportRegisterCount(filter, CurrentUser);

            //Get the number of rows and calculate the number of pages in the grid
            maxPage = rowsPerPage == 0 ? 1 : allRows / rowsPerPage + (allRows != 0 && allRows % rowsPerPage == 0 ? 0 : 1);

            //When the page is loaded for the first time
            if (!IsPostBack)
            {
                //Populate any drop-downs and list-boxes
                PopulateLists();
                this.btnExcel.Visible = false;
                this.divPagingItems.Visible = false;
            }

            lblMessage.Text = "";
            lblGridMessage.Text = "";
        }

        //Populate all listboxes on the screen
        private void PopulateLists()
        {
            this.PopulateMilitaryDepartments();
            this.PopulateYears();
        }

        //Populate the MilitaryDepartments drop-down
        private void PopulateMilitaryDepartments()
        {
            this.ddlMilitaryDepartments.DataSource = MilitaryDepartmentUtil.GetAllMilitaryDepartments(CurrentUser);
            this.ddlMilitaryDepartments.DataTextField = "MilitaryDepartmentName";
            this.ddlMilitaryDepartments.DataValueField = "MilitaryDepartmentId";
            this.ddlMilitaryDepartments.DataBind();
            this.ddlMilitaryDepartments.Items.Insert(0, ListItems.GetOptionChooseOne());
        }

        //Populate the Years drop-down
        private void PopulateYears()
        {
            this.ddlYears.DataSource = RegisterUtil.GetAllRegisterYears(CurrentUser);
            this.ddlYears.DataTextField = "yearValue";
            this.ddlYears.DataValueField = "year";
            this.ddlYears.DataBind();
            this.ddlYears.Items.Insert(0, ListItems.GetOptionChooseOne());
        }

        //Validate some field on the screen
        private bool ValidateData()
        {
            bool isDataValid = true;
            return isDataValid;
        }

        //Refresh the data grid
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = "1";
                RefreshReports();
            }
        }

        //Go to the first page and refresh the grid
        protected void btnFirst_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = "1";
                RefreshReports();
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

                    RefreshReports();
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

                    RefreshReports();
                }
            }
        }

        //Go to the last page and refresh the grid
        protected void btnLast_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = maxPage.ToString();
                RefreshReports();
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
                    RefreshReports();
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
            hdnPageIdx.Value = "1";
            Response.Redirect("~/ContentPages/Home.aspx");
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ddlMilitaryDepartments.SelectedValue = ListItems.GetOptionChooseOne().Value;
            ddlYears.SelectedValue = ListItems.GetOptionChooseOne().Value;
        }

        //Refresh the data grid
        private void RefreshReports()
        {
            string html = "";

            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            //Collect the filter information to be able to pull the number of rows for this specific filter
            filter = CollectFilterData();

            //Get the list of records according to the specified filters and paging
            List<ReportRegisterBlock> listBlocks = ReportRegisterUtil.GetReportRegisterBlock(filter, rowsPerPage, CurrentUser);

            //No data found
            if (listBlocks.Count == 0)
            {
                this.btnExcel.Visible = false;
                this.divPagingItems.Visible = false;
                html = "<span>Няма намерени резултати</span>";
            }
            //If there is data then generate dynamically the HTML for the data grid
            else
            {
                this.btnExcel.Visible = true;
                this.divPagingItems.Visible = true;
                string headerStyle = "vertical-align: bottom;";

                //Setup the header of the grid
                html = @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                     <thead>
                        <tr>
                           <th style='width: 40px;" + headerStyle + @"'>Номер</th>
                           <th style='width: 100px;" + headerStyle + @"'>Дата на постъпване на документа</th>
                           <th style='width: 180px;" + headerStyle + @"'>От къде е постъпил документа</th>
                           <th style='width: 240px;" + headerStyle + @"'>Кратко съдържание</th>
                           <th style='width: 80px;" + headerStyle + @"'>Брой на листата</th>
                           <th style='width: 150px;" + headerStyle + @"'>Номер на писмото с което се изпраща документа</th>
                           <th style='width: 200px;" + headerStyle + @"'>Забележка</th>
                        </tr>
                     </thead>";

                int counter = 1;

                var groupedBlocks = listBlocks
                    .GroupBy(x => new
                    {
                        x.RegisterNumber,
                        x.DocumentDateString,
                        x.ApplicantFullName,
                        x.OrderNumber,
                        x.OrderDateString,
                        x.PageCount,
                        x.Notes
                    })
                    .Select(x => new
                    {
                        RegisterNumber = x.Key.RegisterNumber,
                        DocumentDateString = x.Key.DocumentDateString,
                        ApplicantFullName = x.Key.ApplicantFullName,
                        OrderNumber = x.Key.OrderNumber,
                        OrderDateString = x.Key.OrderDateString,
                        Positions = x.Select(y => new 
                                     {
                                         PositionName = y.PositionName,
                                         VPN = y.VPN,
                                         SEQ = y.SEQ
                                     }).OrderBy(y => y.SEQ).ToList(),
                        PageCount = x.Key.PageCount,
                        Notes = x.Key.Notes
                    })
                    .OrderBy(x => x.RegisterNumber)
                    .ToList();

                //Iterate through all items and add them into the grid
                foreach (var block in groupedBlocks)
                {
                    string cellStyleText = "vertical-align: top; text-align: left;";

                    string descriptionHTML = "Кандидат за ВС по МЗ №" + block.OrderNumber.ToString() + "/" + block.OrderDateString;

                    foreach (var position in block.Positions)
                    {
                        descriptionHTML += @"<br/>
                                             &nbsp; -в.ф. " + position.VPN + " - " + position.PositionName;
                    }

                    string notesHTML = CommonFunctions.HtmlEncoding(block.Notes).Replace(Environment.NewLine, "<br />");

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='" + cellStyleText + @"'>" + block.RegisterNumber.ToString() + @"</td>
                                 <td style='" + cellStyleText + @"'>" + block.DocumentDateString + @"</td>
                                 <td style='" + cellStyleText + @"'>" + block.ApplicantFullName + @"</td>
                                 <td style='" + cellStyleText + @"'>" + descriptionHTML + @"</td>
                                 <td style='" + cellStyleText + @"'>" + block.PageCount + @"</td>
                                 <td></td>
                                 <td style='" + cellStyleText + @"'>" + notesHTML + @"</td>
                              </tr>";

                    counter++;
                }

                html += "</table>";
            }

            //Put the generated grid on the page
            pnlReportsGrid.InnerHtml = html;

            //Refresh the paging button according to the current position
            SetImgBtns();
            lblPagination.Text = " | " + hdnPageIdx.Value + " от " + maxPage.ToString() + " | ";
            txtGotoPage.Text = "";
        }

        //Collect the filter information from the page
        private ReportRegisterFilter CollectFilterData()
        {
            int pageIdx;
            if (!int.TryParse(this.hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            ReportRegisterFilter filter = new ReportRegisterFilter();

            if (this.ddlMilitaryDepartments.SelectedValue != ListItems.GetOptionChooseOne().Value
                && this.ddlMilitaryDepartments.SelectedValue != "")
            {
                filter.MilitaryDepartmentId = int.Parse(this.ddlMilitaryDepartments.SelectedValue);
                this.hfMilitaryDepartmentId.Value = this.ddlMilitaryDepartments.SelectedValue;
            }
            else
            {
                this.hfMilitaryDepartmentId.Value = "";
            }

            if (this.ddlYears.SelectedValue != ListItems.GetOptionChooseOne().Value
                && this.ddlYears.SelectedValue != "")
            {
                filter.Year = this.ddlYears.SelectedValue;
                this.hfYear.Value = this.ddlYears.SelectedValue;
            }
            else
            {
                this.hfYear.Value = "";
            }

            filter.PageIndex = pageIdx;

            return filter;
        }
    }
}
