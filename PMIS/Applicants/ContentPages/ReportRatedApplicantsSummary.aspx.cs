using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using PMIS.Common;
using PMIS.Applicants.Common;

namespace PMIS.Applicants.ContentPages
{
    public partial class ReportRatedApplicantsSummary : APPLPage
    {
        public string MilitaryUnitLabel = CommonFunctions.GetLabelText("MilitaryUnit");

        //Get the config setting that says how many rows per page should be dispayed in the grid
        private int rowsPerPage = int.Parse(Config.GetWebSetting("RowsPerPage"));
        //Stores information about how many pages are in the grid
        private int maxPage;

        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "APPL_REPORTS_REPORT_RATED_APPLICANTS_SUMMARY";
            }
        }

        UIAccessLevel l;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if (GetUIItemAccessLevel("APPL_REPORTS_REPORT_RATED_APPLICANTS_SUMMARY") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            //Hilight the current page in the menu bar
            HighlightMenuItems("Reports", "ReportRatedApplicantsSummary");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            //When the page is loaded for the first time
            if (!IsPostBack)
            {
                //Populate any drop-downs and list-boxes
                PopulateLists();

                //Simulate clicking the Refresh button to load the grid initially
                // btnRefresh_Click(btnRefresh, new EventArgs());

                this.divPagingtems.Visible = false;
                this.btnPrintReportRatedApplicantsSummary.Visible = false;
            }
            else
            {
                //Collect the filter information to be able to pull the number of rows for this specific filter
                ReportRatedApplicantsSummaryFilter filter = CollectFilterData();

                int allRows = ReportRatedApplicantsSummaryUtil.GetAllReportRatedApplicantsSummaryCount(filter, CurrentUser);
                //Get the number of rows and calculate the number of pages in the grid
                maxPage = rowsPerPage == 0 ? 1 : allRows / rowsPerPage + (allRows != 0 && allRows % rowsPerPage == 0 ? 0 : 1);
            }

            lblMessage.Text = "";
            lblGridMessage.Text = "";

        }
        //Populate all listboxes on the screen
        private void PopulateLists()
        {
            this.PopulateVacancyAnnounces();
            this.PopulateMilitaryUnits();
            this.PopulateMilitaryDepartments();
            this.PopulatePositions();
            this.PopulateStatuses();
        }

        //Populate the VacancyAnnounces drop-down
        private void PopulateVacancyAnnounces()
        {
            this.ddlVacancyAnnounces.DataSource = VacancyAnnounceUtil.GetVacancyAnnouncesListItemsForReports("APPL_REPORTS_REPORT_RATED_APPLICANTS_SUMMARY", true, CurrentUser);
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

        private void PopulatePositions()
        {
            int vacancyAnnounceID = 0;
            if (ddlVacancyAnnounces.SelectedValue != ListItems.GetOptionChooseOne().Value)
                vacancyAnnounceID = int.Parse(ddlVacancyAnnounces.SelectedValue);

            this.ddlPosition.DataSource = VacancyAnnounceUtil.GetDistinctPositionsForVacancyAnnounceID(vacancyAnnounceID, CurrentUser);
            this.ddlPosition.DataTextField = "Text";
            this.ddlPosition.DataValueField = "Value";
            this.ddlPosition.DataBind();
            this.ddlPosition.Items.Insert(0, ListItems.GetOptionAll());
        }

        private void PopulateStatuses()
        {
            this.ddlStatus.DataSource = ApplicantPositionUtil.GetAllCombinedApplicantStatuses(CurrentUser);
            this.ddlStatus.DataTextField = "Text";
            this.ddlStatus.DataValueField = "Value";
            this.ddlStatus.DataBind();
            this.ddlStatus.Items.Insert(0, ListItems.GetOptionAll());
        }

        //Refresh the data grid
        private void RefreshReports()
        {
            string html = "";

            //Collect the filters and the paging control data from the page            
            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            //Collect the filter information to be able to pull the number of rows for this specific filter
            ReportRatedApplicantsSummaryFilter filter = CollectFilterData();

            //Get the list of records according to the specified filters and paging
            List<ReportRatedApplicantsSummaryBlock> listBlocks = ReportRatedApplicantsSummaryUtil.GetReportRatedApplicantsSummarySearch(filter, rowsPerPage, CurrentUser);

            //No data found
            if (listBlocks.Count == 0)
            {
                html = "<span>Няма намерени резултати</span>";
                this.btnPrintReportRatedApplicantsSummary.Visible = false;
                this.divPagingtems.Visible = false;
            }
            //If there is data then generate dynamically the HTML for the data grid
            else
            {
                this.divPagingtems.Visible = true;
                this.btnPrintReportRatedApplicantsSummary.Visible = true;

                string headerStyle = "vertical-align: bottom;";

                //Setup the header of the grid
                html = @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                     <thead>
                        <tr>
                           <th style='width: 180px;" + headerStyle + @"' rowspan='2'>Военно формирование</th>
                           <th style='width: 180px;" + headerStyle + @"' colspan='3'>до 35 години</th>
                           <th style='width: 180px;" + headerStyle + @"' colspan='3'>до 45 години</th>
                           <th style='width: 180px;" + headerStyle + @"' colspan='3'>над 45 години</th>
                           <th style='width: 180px;" + headerStyle + @"' colspan='3'>Всичко</th>
                           <th style='width: 180px;" + headerStyle + @"' colspan='3'>без военна подготовка</th>
                        </tr>
                        <tr>
                           <th style='width: 60px;" + headerStyle + @"'>М</th>
                           <th style='width: 60px;" + headerStyle + @"'>Ж</th>
                           <th style='width: 60px;" + headerStyle + @"'>Общо</th>
                           <th style='width: 60px;" + headerStyle + @"'>М</th>
                           <th style='width: 60px;" + headerStyle + @"'>Ж</th>
                           <th style='width: 60px;" + headerStyle + @"'>Общо</th>
                           <th style='width: 60px;" + headerStyle + @"'>М</th>
                           <th style='width: 60px;" + headerStyle + @"'>Ж</th>
                           <th style='width: 60px;" + headerStyle + @"'>Общо</th>
                           <th style='width: 60px;" + headerStyle + @"'>М</th>
                           <th style='width: 60px;" + headerStyle + @"'>Ж</th>
                           <th style='width: 60px;" + headerStyle + @"'>Общо</th>
                           <th style='width: 60px;" + headerStyle + @"'>М</th>
                           <th style='width: 60px;" + headerStyle + @"'>Ж</th>
                           <th style='width: 60px;" + headerStyle + @"'>Общо</th>
                        </tr>
                     </thead>";

                int counter = 1;

                //Iterate through all items and add them into the grid
                foreach (ReportRatedApplicantsSummaryBlock block in listBlocks)
                {
                    string cellStyleText = "vertical-align: top;";
                    string cellStyleNumber = "vertical-align: top; text-align: right;";

                    if (block.RowType == 3)
                        cellStyleText += " text-align: right;";

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='" + cellStyleText + @"'>" + block.DisplayText + @"</td>
                                 <td style='" + cellStyleNumber + @"'>" + block.ClassA_M_Cnt.ToString() + @"</td>
                                 <td style='" + cellStyleNumber + @"'>" + block.ClassA_F_Cnt.ToString() + @"</td>
                                 <td style='" + cellStyleNumber + @"'>" + block.ClassA_Total_Cnt.ToString() + @"</td>
                                 <td style='" + cellStyleNumber + @"'>" + block.ClassB_M_Cnt.ToString() + @"</td>
                                 <td style='" + cellStyleNumber + @"'>" + block.ClassB_F_Cnt.ToString() + @"</td>
                                 <td style='" + cellStyleNumber + @"'>" + block.ClassB_Total_Cnt.ToString() + @"</td>
                                 <td style='" + cellStyleNumber + @"'>" + block.ClassC_M_Cnt.ToString() + @"</td>
                                 <td style='" + cellStyleNumber + @"'>" + block.ClassC_F_Cnt.ToString() + @"</td>
                                 <td style='" + cellStyleNumber + @"'>" + block.ClassC_Total_Cnt.ToString() + @"</td>
                                 <td style='" + cellStyleNumber + @"'>" + block.Total_M_Cnt.ToString() + @"</td>
                                 <td style='" + cellStyleNumber + @"'>" + block.Total_F_Cnt.ToString() + @"</td>
                                 <td style='" + cellStyleNumber + @"'>" + block.Total_Total_Cnt.ToString() + @"</td>
                                 <td style='" + cellStyleNumber + @"'>" + block.WithoutMilitary_M_Cnt.ToString() + @"</td>
                                 <td style='" + cellStyleNumber + @"'>" + block.WithoutMilitary_F_Cnt.ToString() + @"</td>
                                 <td style='" + cellStyleNumber + @"'>" + block.WithoutMilitary_Total_Cnt.ToString() + @"</td>
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
        private ReportRatedApplicantsSummaryFilter CollectFilterData()
        {
            ReportRatedApplicantsSummaryFilter filter = new ReportRatedApplicantsSummaryFilter();

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

            if (this.ddlPosition.SelectedValue != ListItems.GetOptionChooseOne().Value
                && this.ddlPosition.SelectedValue != "")
            {
                filter.Position = this.ddlPosition.SelectedValue;
                this.hfPosition.Value = this.ddlPosition.SelectedValue;
            }
            else
            {
                this.hfPosition.Value = "";
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

            if (this.ddlStatus.SelectedValue != ListItems.GetOptionChooseOne().Value
                && this.ddlStatus.SelectedValue != "")
            {
                filter.Status = this.ddlStatus.SelectedValue;
                this.hfStatus.Value = this.ddlStatus.SelectedValue;
            }
            else
            {
                this.hfStatus.Value = "";
            }

            int pageIdx;
            if (!int.TryParse(this.hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            filter.PageIndex = pageIdx;

            return filter;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ddlVacancyAnnounces.SelectedValue = ListItems.GetOptionChooseOne().Value;
            ddlMilitaryUnit.SelectedValue = ListItems.GetOptionAll().Value;
            ddlPosition.SelectedValue = ListItems.GetOptionAll().Value;
            ddlMilitaryDepartments.SelectedValue = ListItems.GetOptionAll().Value;
            ddlStatus.SelectedValue = ListItems.GetOptionAll().Value;
        }

        protected void ddlVacancyAnnounces_Changed(object sender, EventArgs e)
        {
            PopulateMilitaryUnits();
            PopulatePositions();
        }
    }
}
