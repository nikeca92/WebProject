using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
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
    public partial class ReportA33v2 : RESPage
    {
        //Get the config setting that says how many rows per page should be dispayed in the grid
        private int pageLength = 0;
        private string sessionResultsKey = "ReportA33v2Result";

        private ReportA33v2Result reportResult = null;
        private ReportA33v2Result ReportResult
        {
            get
            {
                if (reportResult == null)
                {
                    if (Session[sessionResultsKey] != null)
                    {
                        DateTime pullSessionStart = BenchmarkLog.WriteStart("\tНачало на извличане на справката от сесията", CurrentUser, Request);
                        reportResult = (ReportA33v2Result)Session[sessionResultsKey];
                        BenchmarkLog.WriteEnd("\tКрай на извличане на справката от сесията", CurrentUser, Request, pullSessionStart);
                    }
                    else
                    {
                        DateTime pullFilterStart = BenchmarkLog.WriteStart("\tНачало на извличане на филтъра", CurrentUser, Request);
                        ReportA33v2Filter filter = CollectFilterData();
                        BenchmarkLog.WriteEnd("\tКрай на извличане на филтъра", CurrentUser, Request, pullFilterStart);

                        DateTime pullReportStart = BenchmarkLog.WriteStart("\tНачало на извличане на данните", CurrentUser, Request);
                        reportResult = ReportA33v2Util.GetReportA33v2(filter, CurrentUser);
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
                return "RES_REPORTS_REPORTA33v2";
            }
        }

        private DateTime? postBackStart = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.GetUIItemAccessLevel("RES_REPORTS_REPORTA33v2") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            DateTime? pageStart = null;
            if (!IsPostBack)
                pageStart = BenchmarkLog.WriteStart("Отваряне на екран 'Сведение-анализ за състоянието на ресурсите от резерва'", CurrentUser, Request);

            if (IsPostBack)
                postBackStart = BenchmarkLog.WriteStart("PostBack в екран 'Сведение-анализ за състоянието на ресурсите от резерва'", CurrentUser, Request);
           
            //Hilight the current page in the menu bar
            HighlightMenuItems("Reports", "ReportA33v2");

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
           
            ddRegion.Attributes.Add("onchange", "SetPickListsSelection();");
            ddMuniciplaity.Attributes.Add("onchange", "SetPickListsSelection();");
            ddCity.Attributes.Add("onchange", "SetPickListsSelection();");  
           
            if (pageStart.HasValue)
                BenchmarkLog.WriteEnd("Край на зареждане на екран 'Сведение-анализ за състоянието на ресурсите от резерва'", CurrentUser, Request, pageStart.Value);
        }             

        //Populate all listboxes on the screen
        private void PopulateLists()
        {
            PopulateMilitaryDepartments();        
            PopulateRegions();
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

        //Populate ddRegion
        private void PopulateRegions()
        {
            ddRegion.DataSource = RegionUtil.GetRegions(CurrentUser);
            ddRegion.DataTextField = "RegionName";
            ddRegion.DataValueField = "RegionId";
            ddRegion.DataBind();
            ddRegion.Items.Insert(0, ListItems.GetOptionAll());
        }

        protected void ddRegion_Changed(object sender, EventArgs e)
        {
            ddMuniciplaity.Items.Clear();
            ddCity.Items.Clear();
            ddDistrict.Items.Clear();

            if (ddRegion.SelectedValue != "-1")
            {
                int regionId = int.Parse(ddRegion.SelectedValue);

                PopulateMunicipalities(regionId);
            }
        }

        //Populate ddMuniciplaity
        private void PopulateMunicipalities(int regionID)
        {
            ddMuniciplaity.DataSource = MunicipalityUtil.GetMunicipalities(regionID, CurrentUser); ;
            ddMuniciplaity.DataTextField = "MunicipalityName";
            ddMuniciplaity.DataValueField = "MunicipalityId";
            ddMuniciplaity.DataBind();
            ddMuniciplaity.Items.Insert(0, ListItems.GetOptionAll());
        }

        protected void ddMuniciplaity_Changed(object sender, EventArgs e)
        {
            ddCity.Items.Clear();
            ddDistrict.Items.Clear();

            if (ddMuniciplaity.SelectedValue != "-1")
            {
                int municiplaityId = int.Parse(ddMuniciplaity.SelectedValue);

                PopulateCities(municiplaityId);
            }
        }

        //Populate ddCity
        private void PopulateCities(int municipalityID)
        {
            ddCity.DataSource = CityUtil.GetCities(municipalityID, CurrentUser);
            ddCity.DataTextField = "CityName";
            ddCity.DataValueField = "CityId";
            ddCity.DataBind();
            ddCity.Items.Insert(0, ListItems.GetOptionAll());

            // Initialize ddDistrict with blank value
            ddDistrict.Items.Insert(0, ListItems.GetOptionChooseOne());
        }

        //Populate ddDistrict
        protected void ddCity_Changed(object sender, EventArgs e)
        {
            ddDistrict.Items.Clear();

            if (ddCity.SelectedValue != "-1")
            {
                int cityId = int.Parse(ddCity.SelectedValue);

                ddDistrict.DataSource = DistrictUtil.GetDistricts(cityId, CurrentUser);
                ddDistrict.DataTextField = "DistrictName";
                ddDistrict.DataValueField = "DistrictId";
                ddDistrict.DataBind();

                if (ddDistrict.Items.Count > 0)
                    ddDistrict.Items.Insert(0, ListItems.GetOptionAll());
                else
                    ddDistrict.Items.Insert(0, ListItems.GetOptionChooseOne());
            }
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
            if (this.GetUIItemAccessLevel("RES_REPORTS_REPORTA33v2") == UIAccessLevel.Hidden)
                return;

            DateTime renderStart = BenchmarkLog.WriteStart("\tНачало на генериране на изхода", CurrentUser, Request);

            pnlPaging.Visible = true;

            StringBuilder html = new StringBuilder();

            //Get the list of postpones according to the specified filters, order and paging
            List<ReportA33v2Row> reportBlocks = ReportResult.Rows;

            //No data found
            if (reportBlocks.Count == 0)
            {
                html.Append("<span>Няма намерени резултати</span>");
                this.btnPrintReport.Visible = false;
            }
            //If there is data then generate dynamically the HTML for the data grid
            else
            {
                this.btnPrintReport.Visible = true;

                TechnicsType vehiclesTechnicsType = TechnicsTypeUtil.GetTechnicsType("VEHICLES", CurrentUser);
                int vehicleKindsCnt = GTableItemUtil.GetAllGTableItemsCountByTableName("VehicleKind", ModuleKey, CurrentUser);

                string headerRow1 = "";
                string headerRow2 = "";
                string headerRow3 = "";

                int cellIndex = 0;
                foreach (string headerCell in ReportResult.HeaderCells)
                {
                    cellIndex++;
                    if (cellIndex <= 2)
                    {
                        headerRow1 += "<th style='width: " + (cellIndex == 2 ? "260" : "50") + @"px;' rowspan='3'>" + headerCell + "</th>";
                    }
                    else
                    {
                        if (cellIndex == 3)
                        {
                            headerRow1 += "<th colspan='5'>ЛИЧЕН СЪСТАВ</th>";
                        }
                        else if (cellIndex == 7)
                        {
                            headerRow2 += "<th style='width: 60px;' rowspan='2'>Всичко</th>";
                        }
                        else if (cellIndex == 8)
                        {
                            int colspan = ReportResult.HeaderCells.Count() - 5;
                            headerRow1 += "<th colspan='" + colspan.ToString() + "'>ТЕХНИКА</th>";
                        }


                        string headerCellNoWrap = "";
                        string tooltip = "";
                        if (cellIndex >= 3 && cellIndex <= 6)
                        {
                            headerCellNoWrap = "nowrap='nowrap'";

                            switch (cellIndex)
                            {
                                case 3:
                                    tooltip = "Офицери";
                                    break;
                                case 4:
                                    tooltip = "Офицерски кандидати";
                                    break;
                                case 5:
                                    tooltip = "Сержанти ";
                                    break;
                                case 6:
                                    tooltip = "Войници";
                                    break;

                            }
                        }

                        if (cellIndex > 6 && cellIndex <= 6 + vehicleKindsCnt)
                        {
                            if (cellIndex == 7)
                                headerRow2 += "<th colspan='" + vehicleKindsCnt.ToString() + "'>" + vehiclesTechnicsType.TypeName + "</th>";

                            headerRow3 += "<th style='width: 60px;' " + headerCellNoWrap + ">" + headerCell + "</th>";
                        }
                        else
                        {
                            headerRow2 += "<th style='width: 60px;' " + headerCellNoWrap + " rowspan='2' title='" + tooltip + "'>" + headerCell + "</th>";
                        }                        

                        if (cellIndex == ReportResult.HeaderCells.Count())
                        {
                            headerRow2 += "<th style='width: 60px;' rowspan='2'>Всичко</th>";
                        }
                    }
                }

                //Setup the header of the grid
                html.Append(@"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                            " + headerRow1.ToString() + @"
                            </tr> 
                            <tr>
                            " + headerRow2.ToString() + @"
                            </tr>
                            <tr>
                            " + headerRow3.ToString() + @"
                            </tr>
                         </thead>
                         <tbody>
                            ");

                //No paging on this report
                pnlPaging.Visible = false;
                List<ReportA33v2Row> blocks = ReportResult.Rows;

                foreach (ReportA33v2Row row in blocks)
                {
                    html.Append(@"<tr>");

                    int count = 0;
                    int totalHR = 0;
                    int totalTech = 0;

                    bool emptyRowNumber = String.IsNullOrEmpty(row.ColumnValues[0]);

                    foreach (string cell in row.ColumnValues)
                    {
                        string cellValue = cell;

                        count++;

                        if (!row.IsEmptyRow)
                        {
                            if (count == 3 || count == 4 || count == 5 || count == 6)
                            {
                                totalHR += int.Parse(cellValue);
                            }
                            else if (count == 7)
                            {
                                html.Append("<td>" + totalHR.ToString() + "</td>");

                                totalHR = 0;
                            }

                            if (count >= 7 && count <= row.ColumnValues.Count())
                            {
                                totalTech += int.Parse(cellValue);
                            }
                        }

                        if (count == 1 && emptyRowNumber)
                        {
                        }
                        else if (count == 2 && emptyRowNumber)
                        {
                            html.Append("<td colspan='2'>" + cellValue + "</td>");
                        }
                        else if (!row.IsEmptyRow || count <= 2)
                        {
                            string style = "";
                            if (count == 1 && (cellValue.Split('.').Length - 1) <= 2)
                                style = "font-weight: bold;";

                            html.Append("<td style='" + style + "'>" + cellValue + "</td>");

                            if (count == row.ColumnValues.Count())
                            {
                                html.Append("<td>" + totalTech.ToString() + "</td>");
                                totalTech = 0;
                            }
                        }
                        else if (row.IsEmptyRow && count == 3)
                        {
                            html.Append("<td colspan='" + ((int)row.ColumnValues.Count() - 2 + 2).ToString() + "'></td>");
                        }
                    }

                    html.Append("</tr>");
                }

                html.Append("</tbody></table>");                
            }

            //Refresh the paging button according to the current position
            SetImgBtns();
            lblPagination.Text = " | " + hdnPageIdx.Value + " от " + ReportResult.MaxPage.ToString() + " | ";
            txtGotoPage.Text = "";
            
            this.pnlReportGrid.InnerHtml = html.ToString();

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
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Сведение-анализ за състоянието на ресурсите от резерва'", CurrentUser, Request, postBackStart.Value);
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
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Сведение-анализ за състоянието на ресурсите от резерва'", CurrentUser, Request, postBackStart.Value);
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
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Сведение-анализ за състоянието на ресурсите от резерва'", CurrentUser, Request, postBackStart.Value);
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
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Сведение-анализ за състоянието на ресурсите от резерва'", CurrentUser, Request, postBackStart.Value);
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
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Сведение-анализ за състоянието на ресурсите от резерва'", CurrentUser, Request, postBackStart.Value);
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
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Сведение-анализ за състоянието на ресурсите от резерва'", CurrentUser, Request, postBackStart.Value);
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
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Сведение-анализ за състоянието на ресурсите от резерва'", CurrentUser, Request, postBackStart.Value);

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
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Сведение-анализ за състоянието на ресурсите от резерва'", CurrentUser, Request, postBackStart.Value);
        }

        private ReportA33v2Filter CollectFilterData()
        {
            ReportA33v2Filter filter = new ReportA33v2Filter();

            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 1;

            hdnPageIdx.Value = pageIdx.ToString();

            string region = "";

            if (ddRegion.SelectedValue != ListItems.GetOptionAll().Value)
            {
                region = ddRegion.SelectedValue;
                this.hdnRegionId.Value = this.ddRegion.SelectedValue;
            }
            else
            {
                this.hdnRegionId.Value = "";
            }

            string municiplaity = "";

            if (ddMuniciplaity.SelectedValue != ListItems.GetOptionAll().Value)
            {
                municiplaity = ddMuniciplaity.SelectedValue;
                this.hdnMunicipalityId.Value = this.ddMuniciplaity.SelectedValue;
            }
            else
            {
                this.hdnMunicipalityId.Value = "";
            }

            string city = "";

            if (ddCity.SelectedValue != ListItems.GetOptionAll().Value)
            {
                city = ddCity.SelectedValue;
                this.hdnCityId.Value = this.ddCity.SelectedValue;
            }
            else
            {
                this.hdnCityId.Value = "";
            }

            string district = "";

            if (ddDistrict.SelectedValue != ListItems.GetOptionAll().Value)
            {
                district = ddDistrict.SelectedValue;
                this.hdnDistrictId.Value = this.ddDistrict.SelectedValue;
            }
            else
            {
                this.hdnDistrictId.Value = "";
            }

            this.hdnPostCode.Value = txtPostCode.Text;
            this.hdnAddress.Value = txtAddress.Text;
           
            filter.PageIdx = pageIdx;
            filter.PageSize = pageLength;

            filter.MilitaryDepartmentIds = hdnMilitaryDepartmentSelected.Value;
            hdnMilitaryDepartmentId.Value = hdnMilitaryDepartmentSelected.Value;

            filter.Region = region;
            filter.Municipality = municiplaity;
            filter.City = city;
            filter.District = district;
            filter.Address = txtAddress.Text;
            filter.PostCode = txtPostCode.Text;
            
            return filter;
        }
    }
}
