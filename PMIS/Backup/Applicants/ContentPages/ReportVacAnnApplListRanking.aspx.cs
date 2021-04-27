using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Text;
using PMIS.Common;
using PMIS.Applicants.Common;
using System.Linq;

namespace PMIS.Applicants.ContentPages
{
    public partial class ReportVacAnnApplListRanking : APPLPage
    {

        //Get the config setting that says how many rows per page should be dispayed in the grid
        private int rowsPerPage = int.Parse(Config.GetWebSetting("RowsPerPage"));
        //Stores information about how many pages are in the grid
        private int maxPage;

        List<ReportVacAnnApplListRankingBlock> blocks;

        ReportVacAnnApplListRankingBlockFilter filter;

        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "APPL_REPORTS_VACANNAPPL_LIST_RANKING";
            }
        }
        private string MilitaryUnitLabel = CommonFunctions.GetLabelText("MilitaryUnit");

        UIAccessLevel l;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if (GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_RANKING") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            //Hilight the current page in the menu bar
            HighlightMenuItems("Reports", "VacAnnApplListRanking");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetListMilitaryUnitsForVacAnn")
            {
                int vacancyAnnounceId = int.Parse(Request.Params["vacancyAnnounceId"]);
                JSGetListMilitaryUnits(vacancyAnnounceId);
            }

            //When the page is loaded for the first time
            if (!IsPostBack)
            {
                //Populate any drop-downs and list-boxes
                PopulateVacancyAnnounces();

                //The default order is by module name
                if (string.IsNullOrEmpty(hdnSortBy.Value))
                    hdnSortBy.Value = "1";

                //Hide Div wit Paging Items
                this.divPagingItems.Visible = false;
            }

            lblMessage.Text = "";
            lblGridMessage.Text = "";

        }

        //Populate the VacancyAnnounces drop-down
        private void PopulateVacancyAnnounces()
        {
            this.ddlVacancyAnnounces.DataSource = VacancyAnnounceUtil.GetVacancyAnnouncesListItemsForReports("APPL_REPORTS_VACANNAPPL_LIST_PARTICIPATE", true, CurrentUser);
            this.ddlVacancyAnnounces.DataTextField = "Text";
            this.ddlVacancyAnnounces.DataValueField = "Value";
            this.ddlVacancyAnnounces.DataBind();
            this.ddlVacancyAnnounces.Items.Insert(0, ListItems.GetOptionChooseOne());

            //Bind Div
            string HTML = " <select id='ddMilitaryUnitsForVacAnn' style='width: 240px' onchange='ddMilitaryUnitsForVacAnnChange(this)'>";
            HTML += "<option value='-1'></option>";
            HTML += "</select>";

            divMilitaryUnitsForVacAnn.InnerHtml = HTML;

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


        private void RefreshReports()
        {

            int vacancyAnnounceId = filter.VacancyAnnounceId.Value;
            int responsibleMilitaryUnit = filter.ResponsibleMilitaryUnitId.Value;

            //refresh div divMilitaryUnitsForVacAnn
            this.divMilitaryUnitsForVacAnn.InnerHtml = GetHtmlDiv();

            StringBuilder sb = new StringBuilder();

            List<ApplicantExamStatus> allApplicantExamStatuses = ApplicantExamStatusUtil.GetAllApplicantExamStatuses(CurrentUser);

            List<Exam> exams = ExamUtil.GetExamsForVacancyAnnounce(vacancyAnnounceId, CurrentUser);

            //List<VacAnnApplListRankingBlock> blocks = VacAnnApplListRankingBlockUtil.GetListVacAnnApplListRankingBlockSearch(filter, rowsPerPage, CurrentUser);

            if (blocks.Count == 0)
            {
                sb.Append("<span>Няма намерени резултати</span>");
                this.btnPrintReportVacAnnApplListRanking.Visible = false;
                this.divPagingItems.Visible = false;
            }
            else
            {
                this.btnPrintReportVacAnnApplListRanking.Visible = true;
                this.divPagingItems.Visible = true;

                bool IsApplicantNameHidden = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_RANKING_APPLICANT") == UIAccessLevel.Hidden;
                bool IsApplicantIdentNumHidden = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_RANKING_IDENTNUM") == UIAccessLevel.Hidden;
                bool IsApplicantPositionNumHidden = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_RANKING_POSITION") == UIAccessLevel.Hidden;
                bool IsMarkHidden = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_RANKING_MARK") == UIAccessLevel.Hidden;
                bool IsPointsHidden = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_RANKING_POINTS") == UIAccessLevel.Hidden;

                if ((!IsPointsHidden) || (!IsMarkHidden) || (!IsApplicantIdentNumHidden) || (!IsApplicantNameHidden) || (!IsApplicantPositionNumHidden))
                {

                    int rowToShow = 0;
                    bool[] rowsArray = new bool[3];
                    //Fill array use in UI
                    if (!IsApplicantNameHidden)
                    {
                        rowsArray[0] = true;
                    }

                    if (!IsApplicantIdentNumHidden)
                    {
                        rowsArray[1] = true;
                    }

                    if (!IsApplicantPositionNumHidden)
                    {
                        rowsArray[2] = true;
                    }

                    foreach (bool b in rowsArray)
                    {
                        if (b == true)
                        {
                            rowToShow++;
                        }
                    }

                    string[] arrOrderCol = new string[rowToShow];

                    for (int k = 0; k <= rowToShow - 1; k++)
                    {
                        arrOrderCol[k] = "";
                    }

                    int m = 0;


                    //Collect the filters, the order control and the paging control data from the page
                    int orderBy;
                    if (!int.TryParse(hdnSortBy.Value, out orderBy))
                        orderBy = 0;

                    int pageIdx;
                    if (!int.TryParse(this.hdnPageIdx.Value, out pageIdx))
                        pageIdx = 1;

                    int count = blocks.Count;
                    int itemFrom = rowsPerPage * (pageIdx - 1);
                    int itemTo = rowsPerPage * pageIdx - 1;

                    sb.Append("<center>");
                    sb.Append("<table id='positionsTable' name='positionsTable' class='CommonHeaderTable' style='text-align: left;' border='1px'>");
                    sb.Append("<thead>");
                    sb.Append("<tr>");
                    sb.Append("<th style=\"min-width: 30px;\"></th>");

                    string headerStyle = "vertical-align: bottom;";
                    int orderCol = (orderBy > 100 ? orderBy - 100 : orderBy);
                    string img = orderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
                    // string[] arrOrderCol = { "", "", "", "", "", "", "", "" };
                    arrOrderCol[orderCol - 1] = @"<div style='position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";
                    string html = "";

                    if (rowsArray[0] == true)
                    {
                        html += @"     <th style='width: 200px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Име" + arrOrderCol[m] + @"</th>";
                        m++;
                        sb.Append(html);
                    }

                    if (rowsArray[1] == true)
                    {
                        html = @"     <th style='width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>ЕГН" + arrOrderCol[m] + @"</th>";
                        m++;
                        sb.Append(html);
                    }

                    if (rowsArray[2] == true)
                    {
                        html = @"     <th style='width: 110px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>Длъжност" + arrOrderCol[m] + @"</th>";
                        sb.Append(html);
                    }


                    foreach (Exam exam in exams)
                    {
                        if (!IsMarkHidden)
                            sb.Append("<th style=\"min-width: 50px; " + "\">Оценка " + exam.ExamName + "</th>");

                        if (!IsPointsHidden)
                            sb.Append("<th style=\"min-width: 50px; " + "\">Точки " + exam.ExamName + "</th>");
                    }

                    sb.Append("</tr>");
                    sb.Append("</thead>");

                    int counter = 0;
                    sb.Append("<tbody>");

                    for (int i = itemFrom; i <= itemTo; i++)
                    {
                        if (i == count) break;
                        counter++;

                        int num = counter + itemFrom;

                        sb.Append("<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + "'>");
                        sb.Append("<td>" + num + "</td>");



                        if (!IsApplicantNameHidden)
                            sb.Append("<td>" + blocks[i].ApplicantName + "</td>");

                        if (!IsApplicantPositionNumHidden)
                            sb.Append("<td>" + blocks[i].IdentityNumber + "</td>");

                        if (!IsApplicantPositionNumHidden)
                            sb.Append("<td>" + blocks[i].Position + "</td>");

                        if ((!IsPointsHidden) && (!IsMarkHidden))
                        {
                            foreach (ApplicantExamMarkBlock mark in blocks[i].Marks)
                            {
                                string markHTML = "";
                                if (!IsMarkHidden)
                                {
                                    markHTML = "<span style='width: 50px;'>" + (mark.Mark.HasValue ? mark.Mark.Value.ToString() : "") + "<span>";
                                    sb.Append("<td style='" + "' align='center'>" + markHTML + "</td>");
                                }

                                string pointsHTML = "";
                                if (!IsPointsHidden)
                                {
                                    pointsHTML = "<span  style='width: 50px'>" + (mark.Points.HasValue ? mark.Points.Value.ToString() : "") + "<span>";
                                    sb.Append("<td style='" + "' align='center'>" + pointsHTML + "</td>");
                                }
                            }
                        }
                        sb.Append("</tr>");
                    }

                    sb.Append("</tbody>");
                    sb.Append("</table>");
                    sb.Append("</center>");
                }
            }

            // Put the generated grid on the page
            pnlReportsGrid.InnerHtml = sb.ToString();

            //Refresh the paging button according to the current position
            SetImgBtns();
            lblPagination.Text = " | " + hdnPageIdx.Value + " от " + maxPage.ToString() + " | ";
            txtGotoPage.Text = "";
        }

        //Validate some field on the screen
        private bool ValidateData()
        {
            bool isDataValid = false;

            //Collect the filter information to be able to pull the number of rows for this specific filter
            CollectFilterData();

            if (filter.VacancyAnnounceId.HasValue && filter.ResponsibleMilitaryUnitId.HasValue)
            {
                //We have both criteria for search
                isDataValid = true;
            }
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
        private void CollectFilterData()
        {
            filter = new ReportVacAnnApplListRankingBlockFilter();

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

            //Get OrderBy
            int orderBy;
            if (!int.TryParse(this.hdnSortBy.Value, out orderBy))
                orderBy = 0;
            filter.OrderBy = orderBy;

            if (responsibleMilitaryUnitId > 0 && vacancyAnnounceId > 0)
            {
                blocks = ReportVacAnnApplListRankingBlockUtil.GetListVacAnnApplListRankingBlockSearch(filter, rowsPerPage, CurrentUser);

                int allRows = blocks.Count;
                //Get the number of rows and calculate the number of pages in the grid
                maxPage = rowsPerPage == 0 ? 1 : allRows / rowsPerPage + (allRows != 0 && allRows % rowsPerPage == 0 ? 0 : 1);

                int pageIdx;
                if (!int.TryParse(this.hdnPageIdx.Value, out pageIdx))
                    pageIdx = 0;


                filter.PageIndex = pageIdx;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ddlVacancyAnnounces.SelectedValue = ListItems.GetOptionAll().Value;
        }
    }
}
