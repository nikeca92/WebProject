using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using PMIS.Common;
using PMIS.Applicants.Common;

namespace PMIS.Applicants.ContentPages
{
    public partial class ReportVacAnnApplListNominated : APPLPage
    {
        //Get the config setting that says how many rows per page should be dispayed in the grid
        private int rowsPerPage = int.Parse(Config.GetWebSetting("RowsPerPage"));
        //Stores information about how many pages are in the grid
        private int maxPage;

        ReportVacAnnApplListNominatedBlockFilter filter;

        private string MilitaryUnitLabel = CommonFunctions.GetLabelText("MilitaryUnit");

        UIAccessLevel l;

        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "APPL_REPORTS_VACANNAPPL_LIST_NOMINATION";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            //Redirect if the access is denied
            if (GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_NOMINATION") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            //Hilight the current page in the menu bar
            HighlightMenuItems("Reports", "VacAnnApplNominatedReport");

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
                    hdnSortBy.Value = "3";

                //Hide Pagination Ites Div
                this.divPagingItems.Visible = false;
            }

            //Fill the filter information to be able to pull the number of rows for this specific filter
            filter = CollectFilterData();

            int allRows = ReportVacAnnApplListNominatedUtil.GetAllDetailedReportsCount(filter, CurrentUser);
            //Get the number of rows and calculate the number of pages in the grid
            maxPage = rowsPerPage == 0 ? 1 : allRows / rowsPerPage + (allRows != 0 && allRows % rowsPerPage == 0 ? 0 : 1);

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

            //Use method define in VacAnnApplListRankingBlockUtil class becouse is the same!

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

            ////Collect the filter information to be able to pull the number of rows for this specific filter
            filter = CollectFilterData();

            //int allRows = VacAnnApplListNominatedUtil.GetAllDetailedReportsCount(filter, CurrentUser);
            ////Get the number of rows and calculate the number of pages in the grid
            //maxPage = rowsPerPage == 0 ? 1 : allRows / rowsPerPage + (allRows != 0 && allRows % rowsPerPage == 0 ? 0 : 1);

            //Get the list of records according to the specified filters, order and paging
            List<ReportVacAnnApplListNominatedBlock> listDetailedReports = new List<ReportVacAnnApplListNominatedBlock>();

            listDetailedReports = ReportVacAnnApplListNominatedUtil.GetListDetailedReportSearch(filter, rowsPerPage, CurrentUser);

            //No data found
            if (listDetailedReports.Count == 0)
            {
                html = "<span>Няма намерени резултати</span>";
                this.btnPrintReportVacAnnApplListNominated.Visible = false;
                this.divPagingItems.Visible = false;
            }
            //If there is data then generate dynamically the HTML for the data grid
            else
            {
                this.btnPrintReportVacAnnApplListNominated.Visible = true;
                this.divPagingItems.Visible = true;

                int rowToShow = 0;
                bool[] rowsArray = new bool[8];
                //Fill array use in UI
                l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_NOMINATION_APPLICANT");
                if (l != UIAccessLevel.Hidden)
                {
                    rowsArray[0] = true;
                }

                l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_NOMINATION_IDENTNUM");
                if (l != UIAccessLevel.Hidden)
                {
                    rowsArray[1] = true;
                }

                l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_NOMINATION_MILITARYUNIT");
                if (l != UIAccessLevel.Hidden)
                {
                    rowsArray[2] = true;
                }

                l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_NOMINATION_POSITION");
                if (l != UIAccessLevel.Hidden)
                {
                    rowsArray[3] = true;
                }

                l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_NOMINATION_POSITION_CODE");
                if (l != UIAccessLevel.Hidden)
                {
                    rowsArray[4] = true;
                }

                l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_NOMINATION_POSITION_ACCESSLEVEL");
                if (l != UIAccessLevel.Hidden)
                {
                    rowsArray[5] = true;
                }

                l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_NOMINATION_POSITION_TOTALPOINTS");
                if (l != UIAccessLevel.Hidden)
                {
                    rowsArray[6] = true;
                }

                l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_NOMINATION_POSITION_STATUS");
                if (l != UIAccessLevel.Hidden)
                {
                    rowsArray[7] = true;
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
                        html += @"     <th style='width: 110px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>" + MilitaryUnitLabel + arrOrderCol[i] + @"</th>";
                        i++;
                    }

                    if (rowsArray[3] == true)
                    {
                        html += @"     <th style='width: 90px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(4);'>Длъжност" + arrOrderCol[i] + @"</th> ";
                        i++;
                    }

                    if (rowsArray[4] == true)
                    {
                        html += @"     <th style='width: 200px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(5);'>Код на длъжноста" + arrOrderCol[i] + @"</th>";
                        i++;
                    }

                    if (rowsArray[5] == true)
                    {
                        html += @"     <th style='width: 130px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(6);'>Ниво на достъп до КИ" + arrOrderCol[i] + @"</th>  ";
                        i++;
                    }

                    if (rowsArray[6] == true)
                    {
                        html += @"     <th style='width: 110px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(7);'>Общо точки" + arrOrderCol[i] + @"</th>";
                        i++;
                    }

                    if (rowsArray[7] == true)
                    {
                        html += @"     <th style='width: 90px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(8);'>Статус на кандидата" + arrOrderCol[i] + @"</th>  ";
                    }

                    html += @"     </tr>
                         </thead>";

                    int counter = 1;

                    //Iterate through all items and add them into the grid
                    foreach (ReportVacAnnApplListNominatedBlock vacAnnApplListNominatedBlock in listDetailedReports)
                    {
                        string cellStyle = "vertical-align: top;";

                        html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'> ";


                        html += @"           <td align='center' style='" + cellStyle + @"'>" + ((pageIdx - 1) * rowsPerPage + counter).ToString() + @"</td> ";

                        if (rowsArray[0])
                        {
                            html += @"           <td style='" + cellStyle + @"'>" + vacAnnApplListNominatedBlock.ApplicantName + @"</td> ";
                        }
                        if (rowsArray[1])
                        {
                            html += @"           <td style='" + cellStyle + @"'>" + vacAnnApplListNominatedBlock.IdentityNumber + @"</td>";
                        }
                        if (rowsArray[2])
                        {
                            html += @"           <td style='" + cellStyle + @"'>" + vacAnnApplListNominatedBlock.MilitaryUnit + @"</td>  ";
                        }
                        if (rowsArray[3])
                        {
                            html += @"           <td style='" + cellStyle + @"'>" + vacAnnApplListNominatedBlock.Position + @"</td>  ";
                        }
                        if (rowsArray[4])
                        {
                            html += @"           <td style='" + cellStyle + @"'>" + vacAnnApplListNominatedBlock.PositionCode + @"</td> ";
                        }
                        if (rowsArray[5])
                        {
                            html += @"           <td style='" + cellStyle + @"'>" + vacAnnApplListNominatedBlock.AccessLevel + @"</td> ";
                        }
                        if (rowsArray[6])
                        {
                            html += @"           <td style='" + cellStyle + @"'>" + vacAnnApplListNominatedBlock.TotalPoints + @"</td> ";
                        }
                        if (rowsArray[7])
                        {
                            html += @"           <td style='" + cellStyle + @"'>" + vacAnnApplListNominatedBlock.Status + @"</td> ";
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
        private ReportVacAnnApplListNominatedBlockFilter CollectFilterData()
        {
            filter = new ReportVacAnnApplListNominatedBlockFilter();
            int responsibleMilitaryUnitId = 0;

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

            //Get ResponsibleMilitaryUnitId
            if (this.hfResponsibleMilitaryUnitId.Value != "")
            {
                responsibleMilitaryUnitId = int.Parse(this.hfResponsibleMilitaryUnitId.Value);

                if (responsibleMilitaryUnitId > 0)
                {
                    filter.ResponsibleMilitaryUnitId = responsibleMilitaryUnitId;
                }
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
        }
    }
}
