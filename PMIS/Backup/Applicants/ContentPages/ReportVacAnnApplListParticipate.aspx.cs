using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using PMIS.Common;
using PMIS.Applicants.Common;

namespace PMIS.Applicants.ContentPages
{
    public partial class ReportVacAnnApplListParticipate : APPLPage
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
                return "APPL_REPORTS_VACANNAPPL_LIST_PARTICIPATE";
            }
        }
        private string MilitaryUnitLabel = CommonFunctions.GetLabelText("MilitaryUnit");

        UIAccessLevel l;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if (GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_PARTICIPATE") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            //Hilight the current page in the menu bar
            HighlightMenuItems("Reports", "VacAnnApplListParticipate");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetListMilitaryUnitsForVacAnn")
            {
                int vacancyAnnounceId = int.Parse(Request.Params["vacancyAnnounceId"]);
                JSGetListMilitaryUnits(vacancyAnnounceId);
            }

            //Collect the filter information to be able to pull the number of rows for this specific filter
            ReportVacAnnApplListParticipateBlockFilter filter = CollectFilterData();

            int allRows = ReportVacAnnApplListParticipateBlockUtil.GetAllVacAnnApplListParticipateBlockCount(filter, CurrentUser);
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

                string HTML = "<select id='ddMilitaryUnitsForVacAnn' style='width: 240px' onchange='ddMilitaryUnitsForVacAnnChange(this)'>";
                HTML += "<option value='-1'></option>";
                HTML += "</select>";
                this.divMilitaryUnitsForVacAnn.InnerHtml = HTML;

                //Simulate clicking the Refresh button to load the grid initially
                // btnRefresh_Click(btnRefresh, new EventArgs());
                this.divPagingItems.Visible = false;
            }

            lblMessage.Text = "";
            lblGridMessage.Text = "";

        }
        //Populate all listboxes on the screen
        private void PopulateLists()
        {
            this.PopulateVacancyAnnounces();
            this.PopulateApplicantPostionStatus();
        }

        //Populate the VacancyAnnounces drop-down
        private void PopulateVacancyAnnounces()
        {
            this.ddlVacancyAnnounces.DataSource = VacancyAnnounceUtil.GetVacancyAnnouncesListItemsForReports("APPL_REPORTS_VACANNAPPL_LIST_PARTICIPATE", true, CurrentUser);
            this.ddlVacancyAnnounces.DataTextField = "Text";
            this.ddlVacancyAnnounces.DataValueField = "Value";
            this.ddlVacancyAnnounces.DataBind();
            this.ddlVacancyAnnounces.Items.Insert(0, ListItems.GetOptionChooseOne());
        }

        private void JSGetListMilitaryUnits(int vacancyAnnounceId)
        {

            string response = " <select id='ddMilitaryUnitsForVacAnn' style='width: 240px' onchange='ddMilitaryUnitsForVacAnnChange(this)'>";
            // response += "<option value='-1'></option>";
            List<MilitaryUnitForVacAnn> listMilitaryUnitsForVacAnn = ReportVacAnnApplListParticipateBlockUtil.GetListMilitaryUnitsForVacAnn(vacancyAnnounceId, CurrentUser);

            for (int i = 0; i <= listMilitaryUnitsForVacAnn.Count - 1; i++)
            {
                string selected = "";
                if (i == 0)
                {
                    selected = "selected";
                }
                response += "<option " + selected + @" value='" + listMilitaryUnitsForVacAnn[i].MilitaryUnitId + @"'>";
                response += listMilitaryUnitsForVacAnn[i].MilitaryUnitName;
                response += "</option>";
            }

            response += "</select>";

            AJAX a = new AJAX(CommonFunctions.HtmlEncoding(response), this.Response);
            a.Write();
            Response.End();
        }

        //Populate the CustomStatusFilter drop-down
        private void PopulateApplicantPostionStatus()
        {
            //Use special custom method for binding this dropdown list
            List<ApplicantExamStatus> listApplicantExamStatus = ReportVacAnnApplListParticipateBlockUtil.GetCustomListStatuses(CurrentUser);

            this.ddlApplStatus.DataSource = listApplicantExamStatus;

            this.ddlApplStatus.DataTextField = "StatusName";
            this.ddlApplStatus.DataValueField = "StatusKey";
            this.ddlApplStatus.DataBind();


            this.ddlApplStatus.Items.Insert(0, ListItems.GetOptionAll());


        }


        private string GetHtmlDiv()
        {
            int vacancyAnnounceId = 0;

            vacancyAnnounceId = int.Parse(this.hfVacancyAnnounceId.Value);

            string HTML = "<select id='ddMilitaryUnitsForVacAnn' style='width: 240px' onchange='ddMilitaryUnitsForVacAnnChange(this)'>";
            //  HTML += "<option value='-1'></option>";
            List<MilitaryUnitForVacAnn> listMilitaryUnitsForVacAnn = ReportVacAnnApplListParticipateBlockUtil.GetListMilitaryUnitsForVacAnn(vacancyAnnounceId, CurrentUser);

            for (int i = 0; i <= listMilitaryUnitsForVacAnn.Count - 1; i++)
            {
                string selected = "";
                if (listMilitaryUnitsForVacAnn[i].MilitaryUnitId.ToString() == this.hfResponsibleMilitaryUnitId.Value)
                {
                    selected = "selected";
                }
                HTML += "<option " + selected + @" value='" + listMilitaryUnitsForVacAnn[i].MilitaryUnitId + @"'>";
                HTML += listMilitaryUnitsForVacAnn[i].MilitaryUnitName;
                HTML += "</option>";
            }

            HTML += "</select>";

            return HTML;
        }

        //Refresh the data grid
        private void RefreshReports()
        {
            string html = "";

            //refresh div divMilitaryUnitsForVacAnn
            this.divMilitaryUnitsForVacAnn.InnerHtml = GetHtmlDiv();

            //Collect the filters, the order control and the paging control data from the page
            int orderBy;
            if (!int.TryParse(hdnSortBy.Value, out orderBy))
                orderBy = 0;

            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            //Collect the filter information to be able to pull the number of rows for this specific filter
            ReportVacAnnApplListParticipateBlockFilter filter = CollectFilterData();

            //Get the list of records according to the specified filters, order and paging
            List<ReportVacAnnApplListParticipateBlock> listVacAnnApplListParticipateBlock = new List<ReportVacAnnApplListParticipateBlock>();

            listVacAnnApplListParticipateBlock = ReportVacAnnApplListParticipateBlockUtil.GetListVacAnnApplListParticipateBlockSearch(filter, rowsPerPage, CurrentUser);

            //No data found
            if (listVacAnnApplListParticipateBlock.Count == 0)
            {
                html = "<span>Няма намерени резултати</span>";
                this.btnPrintReportVacAnnApplListParticipate.Visible = false;
                this.divPagingItems.Visible = false;
            }
            //If there is data then generate dynamically the HTML for the data grid
            else
            {
                this.btnPrintReportVacAnnApplListParticipate.Visible = true;
                this.divPagingItems.Visible = true;

                int rowToShow = 0;
                bool[] rowsArray = new bool[5];
                //Fill array use in UI
                l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_PARTICIPATE_IDENTNUM");
                if (l != UIAccessLevel.Hidden)
                {
                    rowsArray[0] = true;
                }

                l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_PARTICIPATE_APPLICANT");
                if (l != UIAccessLevel.Hidden)
                {
                    rowsArray[1] = true;
                }

                l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_PARTICIPATE_ADDRESS");
                if (l != UIAccessLevel.Hidden)
                {
                    rowsArray[2] = true;
                }

                l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_PARTICIPATE_MILITARYDEPARTMENT");
                if (l != UIAccessLevel.Hidden)
                {
                    rowsArray[3] = true;
                }

                l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_PARTICIPATE_REMARK");
                if (l != UIAccessLevel.Hidden)
                {
                    rowsArray[4] = true;
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
                        html += @"     <th style='width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>ЕГН" + arrOrderCol[i] + @"</th>";
                        i++;
                    }

                    if (rowsArray[1] == true)
                    {
                        html += @"     <th style='width: 200px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>Трите имена на кандидата" + arrOrderCol[i] + @"</th>";
                        i++;
                    }

                    if (rowsArray[2] == true)
                    {
                        html += @"     <th style='width: 200px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>Адрес" + arrOrderCol[i] + @"</th>";
                        i++;
                    }

                    if (rowsArray[3] == true)
                    {
                        html += @"     <th style='width: 200px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(4);'>Място на регистрация" + arrOrderCol[i] + @"</th> ";
                        i++;
                    }

                    if (rowsArray[4] == true)
                    {
                        html += @"     <th style='width: 200px; " + headerStyle + @"'>Статус" + arrOrderCol[i] + @"</th>";
                        i++;
                    }

                    html += @"     </tr>
                         </thead>";

                    int counter = 1;

                    //Iterate through all items and add them into the grid
                    foreach (ReportVacAnnApplListParticipateBlock vacAnnApplListParticipateBlock in listVacAnnApplListParticipateBlock)
                    {
                        string cellStyle = "vertical-align: top;";

                        html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'> ";


                        html += @"           <td align='center' style='" + cellStyle + @"'>" + ((pageIdx - 1) * rowsPerPage + counter).ToString() + @"</td> ";

                        if (rowsArray[0])
                        {
                            html += @"           <td style='" + cellStyle + @"'>" + vacAnnApplListParticipateBlock.IdentityNumber + @"</td> ";
                        }
                        if (rowsArray[1])
                        {
                            html += @"           <td style='" + cellStyle + @"'>" + vacAnnApplListParticipateBlock.ApplicantName + @"</td>";
                        }
                        if (rowsArray[2])
                        {
                            html += @"           <td style='" + cellStyle + @"'>" + vacAnnApplListParticipateBlock.Address + @"</td>  ";
                        }
                        if (rowsArray[3])
                        {
                            html += @"           <td style='" + cellStyle + @"'>" + vacAnnApplListParticipateBlock.MilitaryDepartment + @"</td>  ";
                        }
                        if (rowsArray[4])
                        {
                            html += @"           <td style='" + cellStyle + @"'>" + vacAnnApplListParticipateBlock.Remark + @"</td> ";
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
        private ReportVacAnnApplListParticipateBlockFilter CollectFilterData()
        {
            ReportVacAnnApplListParticipateBlockFilter filter = new ReportVacAnnApplListParticipateBlockFilter();

            int vacancyAnnounceId = 0;
            int responsibleMilitaryUnitId = 0;

            //Get VacancyAnnounceId
            if (this.hfVacancyAnnounceId.Value != "")
            {
                vacancyAnnounceId = int.Parse(this.hfVacancyAnnounceId.Value);

                if (vacancyAnnounceId > 0)
                {
                    filter.VacancyAnnounceId = vacancyAnnounceId;
                }
            }

            //Get ResponsibleMilitaryUnitId
            if (this.hfResponsibleMilitaryUnitId.Value != "")
            {
                responsibleMilitaryUnitId = int.Parse(this.hfResponsibleMilitaryUnitId.Value);

                if (responsibleMilitaryUnitId > 0)
                {
                    filter.ResponsibleMilitaryUnitId = responsibleMilitaryUnitId;
                }
            }

            //Status
            if (this.ddlApplStatus.SelectedValue != ListItems.GetOptionChooseOne().Value
                && this.ddlApplStatus.SelectedValue != "")
            {
                List<ApplicantExamStatus> listApplicantExamStatus = ReportVacAnnApplListParticipateBlockUtil.GetCustomListStatuses(CurrentUser);

                ApplicantExamStatus applicantExamStatus = new ApplicantExamStatus();

                foreach (ApplicantExamStatus listItem in listApplicantExamStatus)
                {
                    if (listItem.StatusKey == ddlApplStatus.SelectedItem.Value)
                    {
                        applicantExamStatus.StatusId = int.Parse(listItem.StatusId.ToString());
                        continue;
                    }
                }

                applicantExamStatus.StatusKey = this.ddlApplStatus.SelectedItem.Value;
                applicantExamStatus.StatusName = this.ddlApplStatus.SelectedItem.Text;

                filter.ApplicantExamStatus = applicantExamStatus;

                this.hfApplicantStatustId.Value = this.ddlApplStatus.SelectedValue;
            }
            else
            {
                this.hfApplicantStatustId.Value = "";
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
            ddlApplStatus.SelectedValue = ListItems.GetOptionAll().Value;
        }
    }
}
