using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using PMIS.Common;
using PMIS.Applicants.Common;

namespace PMIS.Applicants.ContentPages
{
    public partial class VacAnnApplCadetReport : APPLPage
    {
        //Get the config setting that says how many rows per page should be dispayed in the grid
        private int rowsPerPage = int.Parse(Config.GetWebSetting("RowsPerPage"));
        //Stores information about how many pages are in the grid
        private int maxPage;

        ReportCadetBlockFilter filter;

        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "APPL_REPORTS_REPORT_CADET";
            }
        }
        //     private string MilitaryUnitLabel = CommonFunctions.GetLabelText("MilitaryUnit");

        UIAccessLevel l;


        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if (GetUIItemAccessLevel("APPL_REPORTS_REPORT_CADET") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            //Hilight the current page in the menu bar
            HighlightMenuItems("Reports", "VacAnnApplCadetReport");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetDataForSubjectAndSpeziality")
            {
                filter = CollectFilterData();

                JSGetBindDivSubjectAndSpezialization();
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetDataForSpezialion")
            {
                filter = CollectFilterData();

                JSGetBindDivSpezialization();
            }

            //Fill the filter information to be able to pull the number of rows for this specific filter
            filter = CollectFilterData();

            //Count Rows depends of filter data
            int allRows = ReportCadetUtil.GetAllReportCadetCount(filter, CurrentUser);

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
                btnRefresh_Click(btnRefresh, new EventArgs());
            }

            lblMessage.Text = "";
            lblGridMessage.Text = "";

        }

        //Populate all listboxes on the screen
        private void PopulateLists()
        {
            this.PopulateMilitarySchools();
            this.PopulateMilitarySchoolYears();
            this.divSubject.InnerHtml = BindDivMilitarySchoolSubjects();
            this.divSpecialization.InnerHtml = BindDivMilitarySchoolSpezialitis();
        }

        //Populate the VacancyAnnounces drop-down
        private void PopulateMilitarySchools()
        {

            this.ddlMilitarySchool.DataSource = MilitarySchoolUtil.GetAllMilitarySchools(CurrentUser, true);
            this.ddlMilitarySchool.DataTextField = "MilitarySchoolName";
            this.ddlMilitarySchool.DataValueField = "MilitarySchoolId";
            this.ddlMilitarySchool.DataBind();

            this.ddlMilitarySchool.Items.Insert(0, ListItems.GetOptionAll());

        }

        //Populate the MilitaryDepartments drop-down
        private void PopulateMilitarySchoolYears()
        {
            this.ddlMilitarySchoolYear.DataSource = ReportCadetUtil.GetAllMilitaryYears(CurrentUser);

            this.ddlMilitarySchoolYear.DataTextField = "yearValue";
            this.ddlMilitarySchoolYear.DataValueField = "year";
            this.ddlMilitarySchoolYear.DataBind();
            this.ddlMilitarySchoolYear.Items.Insert(0, ListItems.GetOptionAll());
        }

        //Create HTML for DropDownList Subjects
        private string BindDivMilitarySchoolSubjects()
        {

            string HTML = "<select id='ddMilitarySchoolSubjects' style='width: 300px' onchange='ddMilitarySchoolSubjectsChange(this)'>";
            HTML += "<option value='-1' title='Всички'><Всички></option>";

            List<MilitarySchoolSubject> listMilitarySchoolSubject = ReportCadetUtil.GetAllMilitarySchoolSubject(filter, CurrentUser);

            for (int i = 0; i <= listMilitarySchoolSubject.Count - 1; i++)
            {
                string selected = "";
                if (listMilitarySchoolSubject[i].MilitarySchoolSubjectId.ToString() == this.hfMilitarySubjectId.Value)
                {
                    selected = "selected";
                }
                HTML += "<option " + selected + @" value='" + listMilitarySchoolSubject[i].MilitarySchoolSubjectId +
                @"'" + "title='" + listMilitarySchoolSubject[i].MilitarySchoolSubjectName + @"'>";
                HTML += listMilitarySchoolSubject[i].MilitarySchoolSubjectName;
                HTML += "</option>";
            }

            HTML += "</select>";

            return HTML;


        }

        //Create HTML for DropDownList Spezialisations
        private string BindDivMilitarySchoolSpezialitis()
        {

            string HTML = "<select id='ddMilitarySchoolSpezialitis' style='width: 240px' onchange='ddMilitarySchoolSpezialitisChange(this)'>";

            HTML += "<option value='-1' title='Всички'><Всички></option>";

            List<Specialization> listMilitarySchoolSpezialitis = ReportCadetUtil.GetAllMilitarySchoolSpecializations(filter, CurrentUser);

            for (int i = 0; i <= listMilitarySchoolSpezialitis.Count - 1; i++)
            {
                string selected = "";
                if (listMilitarySchoolSpezialitis[i].SpecializationId.ToString() == this.hfMilitarySpecializationId.Value)
                {
                    selected = "selected";
                }
                HTML += "<option " + selected + @" value='" + listMilitarySchoolSpezialitis[i].SpecializationId +
                @"'" + "title='" + listMilitarySchoolSpezialitis[i].SpecializationName + @"'>";
                HTML += listMilitarySchoolSpezialitis[i].SpecializationName;
                HTML += "</option>";
            }

            HTML += "</select>";

            return HTML;

        }

        //Get HTML according custom parameters
        private void JSGetBindDivSubjectAndSpezialization()
        {
            string response = "";
            response += "<divSubjects>" + BindDivMilitarySchoolSubjects() + "</divSubjects>";
            response += "<divSpezialitis>" + BindDivMilitarySchoolSpezialitis() + "</divSpezialitis>";

            AJAX a = new AJAX(CommonFunctions.HtmlEncoding(response), this.Response);
            a.Write();
            Response.End();
        }

        private void JSGetBindDivSpezialization()
        {
            string response = "";
            response += BindDivMilitarySchoolSpezialitis();

            AJAX a = new AJAX(CommonFunctions.HtmlEncoding(response), this.Response);
            a.Write();
            Response.End();
        }

        private void RefreshReports()
        {
            //Bind again contex in both Div
            this.divSubject.InnerHtml = BindDivMilitarySchoolSubjects();
            this.divSpecialization.InnerHtml = BindDivMilitarySchoolSpezialitis();

            string html = "";

            //Collect the filters, the order control and the paging control data from the page
            int orderBy;
            if (!int.TryParse(hdnSortBy.Value, out orderBy))
                orderBy = 0;

            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            ////Collect the filter information to be able to pull the number of rows for this specific filter
            filter = CollectFilterData();

            //Get the list of records according to the specified filters, order and paging
            List<ReportCadetBlock> listReportCadets = new List<ReportCadetBlock>();

            listReportCadets = ReportCadetUtil.GetAllReportCadetBlock(filter, rowsPerPage, CurrentUser);

            //No data found
            if (listReportCadets.Count == 0)
            {
                html = "<span>Няма намерени резултати</span>";
                this.btnPrintAllCadets.Visible = false;
                this.divPagingItems.Visible = false;
            }
            //If there is data then generate dynamically the HTML for the data grid
            else
            {
                this.btnPrintAllCadets.Visible = true;
                this.divPagingItems.Visible = true;

                int rowToShow = 0;
                bool[] rowsArray = new bool[7];
                //Fill array use in UI
                l = this.GetUIItemAccessLevel("APPL_REPORTS_REPORT_CADET_CADETNAME");
                if (l != UIAccessLevel.Hidden)
                {
                    rowsArray[0] = true;
                }

                l = this.GetUIItemAccessLevel("APPL_REPORTS_REPORT_CADET_IDENTNUM");
                if (l != UIAccessLevel.Hidden)
                {
                    rowsArray[1] = true;
                }

                l = this.GetUIItemAccessLevel("APPL_REPORTS_REPORT_CADET_MILITARYSCOOLNAME");
                if (l != UIAccessLevel.Hidden)
                {
                    rowsArray[2] = true;
                }

                l = this.GetUIItemAccessLevel("APPL_REPORTS_REPORT_CADET_MILITARYSCOOLYEAR");
                if (l != UIAccessLevel.Hidden)
                {
                    rowsArray[3] = true;
                }

                l = this.GetUIItemAccessLevel("APPL_REPORTS_REPORT_CADET_SUBJECT");
                if (l != UIAccessLevel.Hidden)
                {
                    rowsArray[4] = true;
                }

                l = this.GetUIItemAccessLevel("APPL_REPORTS_REPORT_CADET_SPECIALISATION");
                if (l != UIAccessLevel.Hidden)
                {
                    rowsArray[5] = true;
                }

                l = this.GetUIItemAccessLevel("APPL_REPORTS_REPORT_CADET_STATUS");
                if (l != UIAccessLevel.Hidden)
                {
                    rowsArray[6] = true;
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
                        html += @"     <th style='width: 200px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Kурсант" + arrOrderCol[i] + @"</th>";
                        i++;
                    }

                    if (rowsArray[1] == true)
                    {
                        html += @"     <th style='width: 80px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>ЕГН" + arrOrderCol[i] + @"</th>";
                        i++;
                    }

                    if (rowsArray[2] == true)
                    {
                        html += @"     <th style='width: 160px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>Военно училище" + arrOrderCol[i] + @"</th>";
                        i++;
                    }

                    if (rowsArray[3] == true)
                    {
                        html += @"     <th style='width: 90px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(4);'>Учебна година" + arrOrderCol[i] + @"</th> ";
                        i++;
                    }

                    if (rowsArray[4] == true)
                    {
                        html += @"     <th style='width: 200px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(5);'>Специалност" + " отговорнa за конкурса" + arrOrderCol[i] + @"</th>";
                        i++;
                    }

                    if (rowsArray[5] == true)
                    {
                        html += @"     <th style='width: 160px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(6);'>Специализация" + arrOrderCol[i] + @"</th>  ";
                        i++;
                    }
                    if (rowsArray[6] == true)
                    {
                        html += @"     <th style='width: 70px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(7);'>Статус" + arrOrderCol[i] + @"</th>  ";
                        i++;
                    }

                    html += @"     </tr>
                         </thead>";

                    int counter = 1;

                    //Iterate through all items and add them into the grid
                    foreach (ReportCadetBlock reportCadetBlock in listReportCadets)
                    {
                        string cellStyle = "vertical-align: top;";

                        html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'> ";


                        html += @"           <td align='center' style='" + cellStyle + @"'>" + ((pageIdx - 1) * rowsPerPage + counter).ToString() + @"</td> ";

                        if (rowsArray[0])
                        {
                            html += @"           <td style='" + cellStyle + @"'>" + reportCadetBlock.CadetName + @"</td> ";
                        }
                        if (rowsArray[1])
                        {
                            html += @"           <td style='" + cellStyle + @"'>" + reportCadetBlock.IdentityNumber + @"</td>";
                        }
                        if (rowsArray[2])
                        {
                            html += @"           <td style='" + cellStyle + @"'>" + reportCadetBlock.MilitarySchoolName + @"</td>  ";
                        }
                        if (rowsArray[3])
                        {
                            html += @"           <td style='" + cellStyle + @"'>" + reportCadetBlock.MilitarySchoolYear + @"</td>  ";
                        }
                        if (rowsArray[4])
                        {
                            html += @"           <td style='" + cellStyle + @"'>" + reportCadetBlock.Subject + @"</td> ";
                        }
                        if (rowsArray[5])
                        {
                            html += @"           <td style='" + cellStyle + @"'>" + reportCadetBlock.Specialization + @"</td> ";
                        }
                        if (rowsArray[6])
                        {
                            html += @"           <td style='" + cellStyle + @"'>" + reportCadetBlock.Status + @"</td> ";
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

        // Collect the filet information from the page
        private ReportCadetBlockFilter CollectFilterData()
        {
            filter = new ReportCadetBlockFilter();

            int militarySchoolId = 0;
            if (!string.IsNullOrEmpty(Request.Params["militarySchoolId"]))
            {
                //We have AJAX request
                int.TryParse(Request.Params["militarySchoolId"], out militarySchoolId);
            }
            else
            {
                int.TryParse(this.hfMilitarySchoolId.Value, out militarySchoolId);
            }

            int militarySchoolYear = 0;
            if (!string.IsNullOrEmpty(Request.Params["militarySchoolYear"]))
            {
                //We have AJAX request
                int.TryParse(Request.Params["militarySchoolYear"], out militarySchoolYear);
            }
            else
            {
                int.TryParse(this.hfMilitarySchoolYear.Value, out militarySchoolYear);
            }


            int militarySubjectId = 0;
            if (!string.IsNullOrEmpty(Request.Params["militarySubjectId"]))
            {
                //We have AJAX request
                int.TryParse(Request.Params["militarySubjectId"], out militarySubjectId);
            }
            else
            {
                int.TryParse(this.hfMilitarySubjectId.Value, out militarySubjectId);
            }

            int militarySpecializationId;
            if (!int.TryParse(this.hfMilitarySpecializationId.Value, out militarySpecializationId))
                militarySpecializationId = 0;

            int statusId;
            // if (!int.TryParse(this.hfStatusId.Value, out statusId))
            if (!int.TryParse(ddlStatus.SelectedValue, out statusId))
                statusId = 0;

            int orderBy;
            if (!int.TryParse(this.hdnSortBy.Value, out orderBy))
                orderBy = 0;

            int pageIdx;
            if (!int.TryParse(this.hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            //Set Filter Values
            filter = new ReportCadetBlockFilter();

            filter.MilitarySchoolId = militarySchoolId;
            filter.MilitarySchoolYear = militarySchoolYear;
            filter.SubjectId = militarySubjectId;
            filter.SpecializationId = militarySpecializationId;
            filter.StatusId = statusId;

            filter.OrderBy = orderBy;
            filter.PageIndex = pageIdx;

            return filter;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ddlMilitarySchool.SelectedValue = ListItems.GetOptionAll().Value;
            ddlMilitarySchoolYear.SelectedValue = ListItems.GetOptionAll().Value;
            ddlStatus.SelectedValue = ListItems.GetOptionAll().Value;
        }
    }
}
