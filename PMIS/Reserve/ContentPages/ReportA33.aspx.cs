using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Drawing;

using PMIS.Common;
using PMIS.Reserve.Common;
using System.Collections;

namespace PMIS.Reserve.ContentPages
{
    public partial class ReportA33 : RESPage
    {
        //Get the config setting that says how many rows per page should be dispayed in the grid
        private int pageLength = 0;
        private string sessionResultsKey = "ReportA33Result";

        private ReportA33Result reportResult = null;
        private ReportA33Result ReportResult
        {
            get
            {
                if (reportResult == null)
                {
                    if (Session[sessionResultsKey] != null)
                    {
                        DateTime pullSessionStart = BenchmarkLog.WriteStart("\tНачало на извличане на справката от сесията", CurrentUser, Request);
                        reportResult = (ReportA33Result)Session[sessionResultsKey];
                        BenchmarkLog.WriteEnd("\tКрай на извличане на справката от сесията", CurrentUser, Request, pullSessionStart);
                    }
                    else
                    {
                        DateTime pullFilterStart = BenchmarkLog.WriteStart("\tНачало на извличане на филтъра", CurrentUser, Request);
                        ReportA33Filter filter = CollectFilterData();
                        BenchmarkLog.WriteEnd("\tКрай на извличане на филтъра", CurrentUser, Request, pullFilterStart);

                        DateTime pullReportStart = BenchmarkLog.WriteStart("\tНачало на извличане на данните", CurrentUser, Request);
                        reportResult = ReportA33Util.GetReportA33(filter, CurrentUser);
                        BenchmarkLog.WriteEnd("\tКрай на извличане на данните", CurrentUser, Request, pullReportStart);

                        Session[sessionResultsKey] = reportResult;
                    }
                }

                return reportResult;
            }
        }

        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "RES_REPORTS_REPORTA33";
            }
        }

        private DateTime? postBackStart = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.GetUIItemAccessLevel("RES_REPORTS_REPORTA33") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            DateTime? pageStart = null;
            if (!IsPostBack)
                pageStart = BenchmarkLog.WriteStart("Отваряне на екран 'Анализ на ресурсите (A33)'", CurrentUser, Request);

            if (IsPostBack)
                postBackStart = BenchmarkLog.WriteStart("PostBack в екран 'Анализ на ресурсите (A33)'", CurrentUser, Request);
           
            //Hilight the current page in the menu bar
            HighlightMenuItems("Reports", "ReportA33");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            //Setup some basic styles on the screen
            SetupStyle();

            //When the page is loaded for the first time
            if (!IsPostBack)
            {
                Session[sessionResultsKey] = null;

                //Populate any drop-downs and list-boxes
                PopulateLists();
            
                //Do not 'Simulate clicking the Refresh button to load the grid initially' to prevent slow loading
                //btnRefresh_Click(btnRefresh, new EventArgs());
            }

            lblMessage.Text = "";
            lblGridMessage.Text = "";

            btnRefresh.Attributes.Add("onclick", "SetPickListsSelection();");

            if (pageStart.HasValue)
                BenchmarkLog.WriteEnd("Край на зареждане на екран 'Анализ на ресурсите (A33)'", CurrentUser, Request, pageStart.Value);
        }             

        //Populate all listboxes on the screen
        private void PopulateLists()
        {
            PopulateMilitaryDepartments();            
        }


        private void PopulateMilitaryDepartments()
        {
            string result = "";

            List<MilitaryDepartment> militaryDepartments = MilitaryDepartmentUtil.GetAllMilitaryDepartments(CurrentUser);

            foreach (MilitaryDepartment militaryDepartment in militaryDepartments)
            {
                string pickListItem = "{value : '" + militaryDepartment.MilitaryDepartmentId.ToString() + "' , label : '" + militaryDepartment.MilitaryDepartmentName.Replace("'", "\\'") + "'}";
                result += (result == "" ? "" : ",") + pickListItem;
            }

            if (result != "")
                result = "[" + result + "]";

            hdnMilitaryDepartmentJson.Value = result;
        }        

        //Setup some styling on the page
        private void SetupStyle()
        {
            lblMilitaryDepartment.Style.Add("vertical-align", "middle");            
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
        private void RefreshReport()
        {
            if (this.GetUIItemAccessLevel("RES_REPORTS_REPORTA33") == UIAccessLevel.Hidden)
                return;

            DateTime renderStart = BenchmarkLog.WriteStart("\tНачало на генериране на изхода", CurrentUser, Request);

            pnlPaging.Visible = true;

            string html = "";

            //Get the list of postpones according to the specified filters, order and paging
            ArrayList reportBlocks = ReportResult.Rows;

            //No data found
            if (reportBlocks.Count == 0)
            {
                html = "<span>Няма намерени резултати</span>";
                this.btnPrintReport.Visible = false;
            }
            //If there is data then generate dynamically the HTML for the data grid
            else
            {
                this.btnPrintReport.Visible = true;                

                string headerRow1 = "";
                string headerRow2 = "";
                string headerRow3 = "";

                int cellIndex = 0;
                foreach (string headerCell in ReportResult.HeaderCells)
                {
                    cellIndex++;
                    if (cellIndex <= 3)
                    {
                        headerRow1 += "<th style='width: " + (cellIndex == 3 ? "180" : "30") + @"px;' rowspan='2'>" + headerCell + "</th>";
                    }
                    else
                    {
                        if (cellIndex == 4)
                        {
                            headerRow1 += "<th colspan='3'>ЛИЧЕН СЪСТАВ</th>";
                        }
                        else if (cellIndex == 6)
                        {
                            headerRow2 += "<th style='width: 60px;'>Всичко</th>";
                        }
                        else if (cellIndex == 7)
                        {
                            int colspan = ReportResult.HeaderCells.Count() - 4;
                            headerRow1 += "<th colspan='" + colspan.ToString() + "'>ТЕХНИКА ОТ НАЦИОНАЛНОТО СТОПАНСТВО</th>";
                        }

                        string s = headerCell; //
                        if (s == "Инженерна техника")
                            s = "ИТ";
                        else if (s == "Подемно-транспортна техника")
                            s = "ППТТ";
                        else if (s == "Специализиран железопътен състав")
                            s = "СЖС";
                        else if (s == "Авиационна техника")
                            s = "АТ";
                        
                        headerRow2 += "<th style='width: 60px;'>" + s + "</th>";

                        if (cellIndex == ReportResult.HeaderCells.Count())
                        {
                            headerRow2 += "<th style='width: 60px;'>Всичко</th>";
                        }
                    }

                    headerRow3 += "<td>" + cellIndex + "</td>";
                }

                cellIndex++;
                headerRow3 += "<td>" + cellIndex + "</td>";
                cellIndex++;
                headerRow3 += "<td>" + cellIndex + "</td>";

                //Setup the header of the grid
                html = @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                            " + headerRow1.ToString() + @"
                            </tr> 
                            <tr>
                            " + headerRow2.ToString() + @"
                            </tr>                            
                         </thead>
                            <tr>
                            " + headerRow3.ToString() + @"
                            </tr>";

                //No paging on this report
                pnlPaging.Visible = false;
                ArrayList blocks = ReportResult.Rows;

                int counter = 0;
                string oldSection = "";

                foreach (string[] row in blocks)
                {
                    counter++;

                    if (counter == 2)
                    {
                        html += "<tr><td colspan='" + ((int)(row.Count() + 2)).ToString() + "'>&nbsp;</td></tr>";
                    }

                    html += @"<tr>
                             ";

                    int count = 0;
                    int totalHR = 0;
                    int totalTech = 0;

                    string section = "";

                    foreach (string cell in row)
                    {
                        string cellValue = cell;

                        count++;

                        if (count == 1)
                        {
                            section = cellValue;

                            if (section != oldSection)
                            {
                                oldSection = section;

                                string sectionTitle = "";

                                if (section == "Б")
                                    sectionTitle = "Доставят се за:";
                                else if (section == "В")
                                    sectionTitle = "Начин на доставяне:";

                                if (sectionTitle != "")
                                {
                                    html += "<td>" + section + @"</td>" +
                                            "<td>&nbsp;</td>" +
                                            "<td>" + sectionTitle + @"</td>" +
                                            "<td colspan='" + ((int)(row.Count() - 1)).ToString() + "'>&nbsp;</td>" +
                                            "</tr><tr>";

                                    cellValue = "";
                                }
                            }
                            else
                            {
                                cellValue = "";
                            }
                        }

                        if (count == 4 || count == 5)
                        {
                            totalHR += int.Parse(cellValue);
                        }
                        else if (count == 6)
                        {
                            html += "<td>" + totalHR.ToString() + "</td>";

                            totalHR = 0;
                        }

                        if (count >= 6 && count <= row.Count())
                        {
                            totalTech += int.Parse(cellValue);
                        }

                        html += "<td>" + cellValue + "</td>";

                        if (count == row.Count())
                        {
                            html += "<td>" + totalTech.ToString() + "</td>";

                            totalTech = 0;
                        }
                    }

                    html += "</tr>";
                }

                html += "</table>";                
            }

            //Refresh the paging button according to the current position
            SetImgBtns();
            lblPagination.Text = " | " + hdnPageIdx.Value + " от " + ReportResult.MaxPage.ToString() + " | ";
            txtGotoPage.Text = "";
            
            this.pnlReportGrid.InnerHtml = html;

            BenchmarkLog.WriteEnd("\tКрай на генериране на изхода", CurrentUser, Request, renderStart);
        }

        //Refresh the data grid
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            pnlSearchHint.Visible = false;

            Session[sessionResultsKey] = null;
            reportResult = null;

            if (ValidateData())
            {
                hdnPageIdx.Value = "1";
                ReportResult.Filter.PageIdx = 1;
                RefreshReport();
            }

            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Анализ на ресурсите (A33)'", CurrentUser, Request, postBackStart.Value);
        }

        //Go to the first page and refresh the grid
        protected void btnFirst_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = "1";
                ReportResult.Filter.PageIdx = 1;
                RefreshReport();
            }

            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Анализ на ресурсите (A33)'", CurrentUser, Request, postBackStart.Value);
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
                    ReportResult.Filter.PageIdx = page;

                    RefreshReport();
                }
            }

            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Анализ на ресурсите (A33)'", CurrentUser, Request, postBackStart.Value);
        }

        //Go to the next page and refresh the grid
        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                int page = int.Parse(hdnPageIdx.Value);

                if (page < ReportResult.MaxPage)
                {
                    page++;
                    hdnPageIdx.Value = page.ToString();
                    ReportResult.Filter.PageIdx = page;

                    RefreshReport();
                }
            }

            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Анализ на ресурсите (A33)'", CurrentUser, Request, postBackStart.Value);
        }

        //Go to the last page and refresh the grid
        protected void btnLast_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = ReportResult.MaxPage.ToString();
                ReportResult.Filter.PageIdx = ReportResult.MaxPage;
                RefreshReport();
            }

            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Анализ на ресурсите (A33)'", CurrentUser, Request, postBackStart.Value);
        }

        //Go to a specific page (entered by the user) and refresh the grid
        protected void btnGoto_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                int gotoPage;
                if (int.TryParse(txtGotoPage.Text, out gotoPage) && gotoPage > 0 && gotoPage <= ReportResult.MaxPage)
                {
                    hdnPageIdx.Value = gotoPage.ToString();
                    ReportResult.Filter.PageIdx = gotoPage;
                    RefreshReport();
                }
            }

            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Анализ на ресурсите (A33)'", CurrentUser, Request, postBackStart.Value);
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

            if (page == ReportResult.MaxPage)
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
            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Анализ на ресурсите (A33)'", CurrentUser, Request, postBackStart.Value);

            Response.Redirect("~/ContentPages/Home.aspx");
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            hdnMilitaryDepartmentSelected.Value = "";            
            
            pnlPaging.Visible = false;
            pnlReportGrid.InnerHtml = "";
            pnlSearchHint.Visible = true;

            Session[sessionResultsKey] = null;
            reportResult = null;

            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Анализ на ресурсите (A33)'", CurrentUser, Request, postBackStart.Value);
        }

        private ReportA33Filter CollectFilterData()
        {
            ReportA33Filter filter = new ReportA33Filter();

            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 1;

            hdnPageIdx.Value = pageIdx.ToString();

            filter.PageIdx = pageIdx;
            filter.PageSize = pageLength;

            filter.MilitaryDepartmentIds = hdnMilitaryDepartmentSelected.Value;
            hdnMilitaryDepartmentId.Value = hdnMilitaryDepartmentSelected.Value;

            return filter;
        }
    }
}
