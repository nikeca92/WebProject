using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using PMIS.Common;
using PMIS.Applicants.Common;

namespace PMIS.Applicants.ContentPages
{
    public partial class ReportVacAnnApplDetailed : APPLPage
    {

        //Get the config setting that says how many rows per page should be dispayed in the grid
        private int rowsPerPage = int.Parse(Config.GetWebSetting("RowsPerPage"));
        //Stores information about how many pages are in the grid
        private int maxPage;

        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "APPL_REPORTS_VACANNAPPL_REPORT_DETAILED";
            }
        }
        private string MilitaryUnitLabel = CommonFunctions.GetLabelText("MilitaryUnit");

        UIAccessLevel l;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if (GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_REPORT_DETAILED") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            //Hilight the current page in the menu bar
            HighlightMenuItems("Reports", "VacAnnApplDetailedReport");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            //Collect the filter information to be able to pull the number of rows for this specific filter
            ReportVacAnnApplDetailedBlockFilter filter = CollectFilterData();

            int allRows = ReportVacAnnApplDetailedUtil.GetAllDetailedReportsCount(filter, CurrentUser);
            //Get the number of rows and calculate the number of pages in the grid
            maxPage = rowsPerPage == 0 ? 1 : allRows / rowsPerPage + (allRows != 0 && allRows % rowsPerPage == 0 ? 0 : 1);

            //When the page is loaded for the first time
            if (!IsPostBack)
            {
                //Populate any drop-downs and list-boxes
                PopulateLists();

                //The default order is by module name
                if (string.IsNullOrEmpty(hdnSortBy.Value))
                    hdnSortBy.Value = "1";

                //Simulate clicking the Refresh button to load the grid initially
                // btnRefresh_Click(btnRefresh, new EventArgs());

                this.divPagingtems.Visible = false;
                this.btnPrintReportVacAnnApplDetailed.Visible = false;
            }

            lblMessage.Text = "";
            lblGridMessage.Text = "";

        }
        //Populate all listboxes on the screen
        private void PopulateLists()
        {
            this.PopulateVacancyAnnounces();
            this.PopulateMilitaryDepartments();
            this.PopulateMilitaryUnits();
            this.PopulateResponsibleMilitaryUnits();
        }

        //Populate the VacancyAnnounces drop-down
        private void PopulateVacancyAnnounces()
        {
            this.ddlVacancyAnnounces.DataSource = VacancyAnnounceUtil.GetVacancyAnnouncesListItemsForReports("APPL_REPORTS_VACANNAPPL_REPORT_DETAILED", true, CurrentUser);
            this.ddlVacancyAnnounces.DataTextField = "Text";
            this.ddlVacancyAnnounces.DataValueField = "Value";
            this.ddlVacancyAnnounces.DataBind();
            this.ddlVacancyAnnounces.Items.Insert(0, ListItems.GetOptionChooseOne());
        }

        //Populate the MilitaryDepartments drop-down
        private void PopulateMilitaryDepartments()
        {
            this.ddlMilitaryDepartments.DataSource = MilitaryDepartmentUtil.GetAllMilitaryDepartments(CurrentUser);
            this.ddlMilitaryDepartments.DataTextField = "MilitaryDepartmentName";
            this.ddlMilitaryDepartments.DataValueField = "MilitaryDepartmentId";
            this.ddlMilitaryDepartments.DataBind();
            this.ddlMilitaryDepartments.Items.Insert(0, ListItems.GetOptionAll());
        }

        private void PopulateMilitaryUnits()
        {
            int vacancyAnnounceID = 0;
            if (ddlVacancyAnnounces.SelectedValue != ListItems.GetOptionChooseOne().Value)
                vacancyAnnounceID = int.Parse(ddlVacancyAnnounces.SelectedValue);

            this.ddlMilitaryUnit.DataSource = VacancyAnnounceUtil.GetDistinctMilitaryUnitsForVacancyAnnounceID(vacancyAnnounceID, CurrentUser);
            this.ddlMilitaryUnit.DataTextField = "Text";
            this.ddlMilitaryUnit.DataValueField = "Value";
            this.ddlMilitaryUnit.DataBind();
            this.ddlMilitaryUnit.Items.Insert(0, ListItems.GetOptionAll());
        }

        private void PopulateResponsibleMilitaryUnits()
        {
            int vacancyAnnounceID = 0;
            if (ddlVacancyAnnounces.SelectedValue != ListItems.GetOptionChooseOne().Value)
                vacancyAnnounceID = int.Parse(ddlVacancyAnnounces.SelectedValue);

            this.ddlResponsibleMilitaryUnit.DataSource = VacancyAnnounceUtil.GetDistinctRespMilitaryUnitsForVacancyAnnounceID(vacancyAnnounceID, CurrentUser);
            this.ddlResponsibleMilitaryUnit.DataTextField = "Text";
            this.ddlResponsibleMilitaryUnit.DataValueField = "Value";
            this.ddlResponsibleMilitaryUnit.DataBind();
            this.ddlResponsibleMilitaryUnit.Items.Insert(0, ListItems.GetOptionAll());
        }

        //Refresh the data grid
        private void RefreshReports()
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
            ReportVacAnnApplDetailedBlockFilter filter = CollectFilterData();

            //Get the list of records according to the specified filters, order and paging
            List<ReportVacAnnApplDetailedBlock> listDetailedReports = new List<ReportVacAnnApplDetailedBlock>();

            listDetailedReports = ReportVacAnnApplDetailedUtil.GetListDetailedReportSearch(filter, rowsPerPage, CurrentUser);

            //No data found
            if (listDetailedReports.Count == 0)
            {
                html = "<span>Няма намерени резултати</span>";
                this.btnPrintReportVacAnnApplDetailed.Visible = false;
                this.divPagingtems.Visible = false;
            }
            //If there is data then generate dynamically the HTML for the data grid
            else
            {
                this.divPagingtems.Visible = true;
                this.btnPrintReportVacAnnApplDetailed.Visible = true;

                int rowToShow = 0;
                bool[] rowsArray = new bool[9];
                //Fill array use in UI
                l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_REPORT_DETAILED_APPLICANT");
                if (l != UIAccessLevel.Hidden)
                {
                    rowsArray[0] = true;
                }

                l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_REPORT_DETAILED_IDENTNUM");
                if (l != UIAccessLevel.Hidden)
                {
                    rowsArray[1] = true;
                }

                l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_REPORT_DETAILED_PERMADDRESS");
                if (l != UIAccessLevel.Hidden)
                {
                    rowsArray[2] = true;
                }

                l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_REPORT_DETAILED_MILITARYUNIT");
                if (l != UIAccessLevel.Hidden)
                {
                    rowsArray[3] = true;
                }

                l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_REPORT_DETAILED_POSITION");
                if (l != UIAccessLevel.Hidden)
                {
                    rowsArray[4] = true;
                }

                l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_REPORT_DETAILED_MILITARYUNIT_RESP");
                if (l != UIAccessLevel.Hidden)
                {
                    rowsArray[5] = true;
                }

                l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_REPORT_DETAILED_MILITARYDEPARTMENT");
                if (l != UIAccessLevel.Hidden)
                {
                    rowsArray[6] = true;
                }

                l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_REPORT_DETAILED_DOCSTATUS");
                if (l != UIAccessLevel.Hidden)
                {
                    rowsArray[7] = true;
                }

                l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_REPORT_DETAILED_APPLSTATUS");
                if (l != UIAccessLevel.Hidden)
                {
                    rowsArray[8] = true;
                }

                foreach (bool b in rowsArray)
                {
                    if (b == true)
                    {
                        rowToShow++;
                    }
                }

                if (rowToShow > 0)
                {


                    string[] arrOrderCol = new string[rowToShow];

                    for (int k = 0; k <= rowToShow - 1; k++)
                    {
                        arrOrderCol[k] = "";
                    }

                    int i = 0;

                    string headerStyle = "vertical-align: bottom;";
                    int orderCol = (orderBy > 100 ? orderBy - 100 : orderBy);
                    string img = orderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
                    // string[] arrOrderCol = { "", "", "", "", "", "", "", "" };
                    arrOrderCol[orderCol - 1] = @"<div style='position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

                    //Setup the header of the grid
                    html = @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>";

                    if (rowsArray[0] == true)
                    {
                        html += @"     <th style='width: 200px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Кандидат" + arrOrderCol[i] + @"</th>";
                        i++;
                    }

                    if (rowsArray[1] == true)
                    {
                        html += @"     <th style='width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>ЕГН" + arrOrderCol[i] + @"</th>";
                        i++;
                    }

                    if (rowsArray[2] == true)
                    {
                        html += @"     <th style='width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>Постоянен адрес" + arrOrderCol[i] + @"</th>";
                        i++;
                    }

                    if (rowsArray[3] == true)
                    {
                        html += @"     <th style='width: 110px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(4);'>" + MilitaryUnitLabel + arrOrderCol[i] + @"</th>";
                        i++;
                    }

                    if (rowsArray[4] == true)
                    {
                        html += @"     <th style='width: 90px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(5);'>Длъжност" + arrOrderCol[i] + @"</th> ";
                        i++;
                    }

                    if (rowsArray[5] == true)
                    {
                        html += @"     <th style='width: 200px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(6);'>" + MilitaryUnitLabel + " отговорнa за конкурса" + arrOrderCol[i] + @"</th>";
                        i++;
                    }

                    if (rowsArray[6] == true)
                    {
                        html += @"     <th style='width: 130px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(7);'>Място на регистрация" + arrOrderCol[i] + @"</th>  ";
                        i++;
                    }

                    if (rowsArray[7] == true)
                    {
                        html += @"     <th style='width: 110px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(8);'>Статус на документите" + arrOrderCol[i] + @"</th>";
                        i++;
                    }

                    if (rowsArray[8] == true)
                    {
                        html += @"     <th style='width: 90px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(9);'>Статус на кандидата" + arrOrderCol[i] + @"</th>  ";
                    }

                    html += @"     </tr>
                         </thead>";

                    int counter = 1;

                    //Iterate through all items and add them into the grid
                    foreach (ReportVacAnnApplDetailedBlock vacAnnApplDetailedReportBlock in listDetailedReports)
                    {
                        string cellStyle = "vertical-align: top;";

                        html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'> ";


                        html += @"           <td align='center' style='" + cellStyle + @"'>" + ((pageIdx - 1) * rowsPerPage + counter).ToString() + @"</td> ";

                        if (rowsArray[0])
                        {
                            html += @"           <td style='" + cellStyle + @"'>" + vacAnnApplDetailedReportBlock.ApplicantName + @"</td> ";
                        }
                        if (rowsArray[1])
                        {
                            html += @"           <td style='" + cellStyle + @"'>" + vacAnnApplDetailedReportBlock.IdentityNumber + @"</td>";
                        }
                        if (rowsArray[2])
                        {
                            html += @"           <td style='" + cellStyle + @"'>" + vacAnnApplDetailedReportBlock.PermAddress + @"</td>";
                        }
                        if (rowsArray[3])
                        {
                            html += @"           <td style='" + cellStyle + @"'>" + vacAnnApplDetailedReportBlock.MilitaryUnit + @"</td>  ";
                        }
                        if (rowsArray[4])
                        {
                            html += @"           <td style='" + cellStyle + @"'>" + vacAnnApplDetailedReportBlock.Position + @"</td>  ";
                        }
                        if (rowsArray[5])
                        {
                            html += @"           <td style='" + cellStyle + @"'>" + vacAnnApplDetailedReportBlock.MilitaryUnitResponsable + @"</td> ";
                        }
                        if (rowsArray[6])
                        {
                            html += @"           <td style='" + cellStyle + @"'>" + vacAnnApplDetailedReportBlock.MilitaryDepartment + @"</td> ";
                        }
                        if (rowsArray[7])
                        {
                            html += @"           <td style='" + cellStyle + @"'>" + vacAnnApplDetailedReportBlock.ApplicantDocumentStatus + @"</td> ";
                        }
                        if (rowsArray[8])
                        {
                            html += @"           <td style='" + cellStyle + @"'>" + vacAnnApplDetailedReportBlock.ApplicantStatus + @"</td> ";
                        }
                        html += @"   </tr>";

                        counter++;
                    }

                    html += "</table>";
                }

            }

            //Put the generated grid on the page
            pnlReportsGrid.InnerHtml = html;

            //Refresh the paging button according to the current position
            SetImgBtns();
            lblPagination.Text = " | " + hdnPageIdx.Value + " от " + maxPage.ToString() + " | ";
            txtGotoPage.Text = "";

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
            Response.Redirect("~/ContentPages/Home.aspx");
        }

        //Collect the filet information from the page
        private ReportVacAnnApplDetailedBlockFilter CollectFilterData()
        {
            ReportVacAnnApplDetailedBlockFilter filter = new ReportVacAnnApplDetailedBlockFilter();

            if (this.ddlVacancyAnnounces.SelectedValue != ListItems.GetOptionChooseOne().Value
                && this.ddlVacancyAnnounces.SelectedValue != "")
            {
                filter.VacancyAnnounceId = int.Parse(this.ddlVacancyAnnounces.SelectedValue);
                this.hfVacancyAnnounceId.Value = this.ddlVacancyAnnounces.SelectedValue;
            }
            else
            {
                this.hfVacancyAnnounceId.Value = "";
            }

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

            if (this.ddlMilitaryUnit.SelectedValue != ListItems.GetOptionChooseOne().Value
                && this.ddlMilitaryUnit.SelectedValue != "")
            {
                filter.MilitaryUnitId = int.Parse(this.ddlMilitaryUnit.SelectedValue);
                this.hfMilitaryUnitId.Value = this.ddlMilitaryUnit.SelectedValue;
            }
            else
            {
                this.hfMilitaryUnitId.Value = "";
            }

            if (this.ddlResponsibleMilitaryUnit.SelectedValue != ListItems.GetOptionChooseOne().Value
                && this.ddlResponsibleMilitaryUnit.SelectedValue != "")
            {
                filter.ResponsibleMilitaryUnitId = int.Parse(this.ddlResponsibleMilitaryUnit.SelectedValue);
                this.hfResponsibleMilitaryUnitId.Value = this.ddlResponsibleMilitaryUnit.SelectedValue;
            }
            else
            {
                this.hfResponsibleMilitaryUnitId.Value = "";
            }

            int orderBy;
            if (!int.TryParse(this.hdnSortBy.Value, out orderBy))
                orderBy = 0;

            int pageIdx;
            if (!int.TryParse(this.hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            filter.OrderBy = orderBy;
            filter.PageIndex = pageIdx;

            return filter;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ddlVacancyAnnounces.SelectedValue = ListItems.GetOptionAll().Value;
            ddlMilitaryDepartments.SelectedValue = ListItems.GetOptionAll().Value;
            ddlMilitaryUnit.SelectedValue = ListItems.GetOptionAll().Value;
            ddlResponsibleMilitaryUnit.SelectedValue = ListItems.GetOptionAll().Value;          
        }

        protected void ddlVacancyAnnounces_Changed(object sender, EventArgs e)
        {
            PopulateMilitaryUnits();
            PopulateResponsibleMilitaryUnits();
        }
    }
}
