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

namespace PMIS.Reserve.ContentPages
{
    public partial class ReportSV1 : RESPage
    {
        //Get the config setting that says how many rows per page should be dispayed in the grid
        private int pageLength = 5;
        private string sessionResultsKey = "ReportSV1Result";

        private ReportSV1Result reportResult = null;
        private ReportSV1Result ReportResult
        {
            get
            {
                if (reportResult == null)
                {
                    if (Session[sessionResultsKey] != null)
                    {
                        reportResult = (ReportSV1Result)Session[sessionResultsKey];
                    }
                    else
                    {
                        ReportSV1Filter filter = CollectFilterData();
                        reportResult = ReportSV1Util.GetReportSV1(filter, CurrentUser);
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
                return "RES_REPORTS_REPORTSV1";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.GetUIItemAccessLevel("RES_REPORTS_REPORTSV1") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }         
           
            //Hilight the current page in the menu bar
            HighlightMenuItems("Reports", "ReportSV1");

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
        }             

        //Populate all listboxes on the screen
        private void PopulateLists()
        {
            PopulateMilitaryDepartments();
            PopulateMilitaryForceSorts();
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

        private void PopulateMilitaryForceSorts()
        {
            ddMilitaryForceSort.Items.Clear();
            ddMilitaryForceSort.Items.Add(ListItems.GetOptionAll());

            List<MilitaryForceSort> milForceSorts = MilitaryForceSortUtil.GetAllMilitaryForceSorts(CurrentUser);

            foreach (MilitaryForceSort sort in milForceSorts)
            {
                if (sort.Active)
                {
                    ListItem li = new ListItem();
                    li.Text = sort.MilitaryForceSortName;
                    li.Value = sort.MilitaryForceSortId.ToString();

                    ddMilitaryForceSort.Items.Add(li);
                }
            }

            PopulateMilitaryReportSpecialities();
        }

        private void PopulateMilitaryReportSpecialities()
        {
            hdnMilitaryReportSpecialitiesSelected.Value = "";
            int militaryForceSortId = Int32.Parse(ddMilitaryForceSort.SelectedValue);

            string result = "";

            List<MilitaryReportSpeciality> militaryReportSpecialities = MilitaryReportSpecialityUtil.GetMilitaryReportSpecialitiesByMilitaryForceSort(CurrentUser, militaryForceSortId, true, true);

            foreach (MilitaryReportSpeciality militaryReprotSpeciality in militaryReportSpecialities)
            {
                string pickListItem = "{value : '" + militaryReprotSpeciality.MilReportSpecialityId.ToString() + "' , label : '" + militaryReprotSpeciality.MilReportingSpecialityCode.Replace("'", "\\'") + " - " + militaryReprotSpeciality.MilReportingSpecialityName.Replace("'", "\\'") + "'}";
                result += (result == "" ? "" : ",") + pickListItem;
            }

            if (result != "")
                result = "[" + result + "]";

            hdnMilitaryReportSpecialitiesJson.Value = result;
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
            lblMilitaryForceSort.Style.Add("vertical-align", "middle");
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
            if (this.GetUIItemAccessLevel("RES_REPORTS_REPORTSV1") == UIAccessLevel.Hidden)
                return;

            pnlPaging.Visible = true;

            string html = "";

            //Get the list of postpones according to the specified filters, order and paging
            List<ReportSV1Block> reportBlocks = ReportResult.PagedBlocks;

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
                
                //Setup the header of the grid
                html = @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 70px;' rowspan='2'></th>
                               <th style='width: 80px;' rowspan='2'>Показатели</th>
                               <th style='width: 280px;' colspan='4'>До 35 год.</th>
                               <th style='width: 280px;' colspan='4'>До 45 год.</th>
                               <th style='width: 280px;' colspan='4'>Над 45 год.</th>
                               <th style='width: 280px;' colspan='4'>ОБЩО</th>
                               <th style='width: 80px;' rowspan='2'>Всичко</th>
                            </tr> 
                            <tr>
                               <th style='width: 70px;' title='Офицери' nowrap='nowrap'>Оф.</th>
                               <th style='width: 70px;' title='Офицерски кандидати' nowrap='nowrap'>Оф. к-ти</th>
                               <th style='width: 70px;' title='Сержанти' nowrap='nowrap'>Серж.</th>
                               <th style='width: 70px;' title='Войници' nowrap='nowrap'>В-ци</th>
                               <th style='width: 70px;' title='Офицери' nowrap='nowrap'>Оф.</th>
                               <th style='width: 70px;' title='Офицерски кандидати' nowrap='nowrap'>Оф. к-ти</th>
                               <th style='width: 70px;' title='Сержанти' nowrap='nowrap'>Серж.</th>
                               <th style='width: 70px;' title='Войници' nowrap='nowrap'>В-ци</th>
                               <th style='width: 70px;' title='Офицери' nowrap='nowrap'>Оф.</th>
                               <th style='width: 70px;' title='Офицерски кандидати' nowrap='nowrap'>Оф. к-ти</th>
                               <th style='width: 70px;' title='Сержанти' nowrap='nowrap'>Серж.</th>
                               <th style='width: 70px;' title='Войници' nowrap='nowrap'>В-ци</th>
                               <th style='width: 70px;' title='Офицери' nowrap='nowrap'>Оф.</th>
                               <th style='width: 70px;' title='Офицерски кандидати' nowrap='nowrap'>Оф. к-ти</th>
                               <th style='width: 70px;' title='Сержанти' nowrap='nowrap'>Серж.</th>
                               <th style='width: 70px;' title='Войници' nowrap='nowrap'>В-ци</th>
                            </tr>
                         </thead>";

                int counter = 1;
                int prevMilRepSpecialityId = -2;
                int prevMilForceSortId = -2;
                int prevMilForceTypeId = -2;
                int prevMilStructureId = -2;
                int milRepStatusCounter = -2;

                //Iterate through all items and add them into the grid
                foreach (ReportSV1Block reportBlock in reportBlocks)
                {
                    //Initialize the milRepStatusCounter variable
                    //It depends on if the Grand Total section is displayed on the page
                    //because its total line is before the military reporting specialities
                    if (milRepStatusCounter == -2)
                    {
                        if (reportBlock.RowType == 0)
                            milRepStatusCounter = -1;
                        else
                            milRepStatusCounter = 0;
                    }

                    string cellStyle = "vertical-align: top;";

                    milRepStatusCounter++;

                    //When any of the "sections" is changed then reset the milRepStatusCounter
                    if (prevMilRepSpecialityId != reportBlock.MilRepSpecialityID)
                    {
                        if (prevMilRepSpecialityId != -2)
                            milRepStatusCounter = 1;

                        prevMilRepSpecialityId = reportBlock.MilRepSpecialityID;
                    }

                    if (prevMilForceSortId != reportBlock.MilitaryForceSortID)
                    {
                        if (prevMilForceSortId != -2)
                            milRepStatusCounter = 1;

                        prevMilForceSortId = reportBlock.MilitaryForceSortID;
                    }

                    if (prevMilForceTypeId != reportBlock.MilitaryForceTypeID)
                    {
                        if (prevMilForceTypeId != -2)
                            milRepStatusCounter = 1;

                        prevMilForceTypeId = reportBlock.MilitaryForceTypeID;
                    }

                    if (prevMilStructureId != reportBlock.MilitaryStructureID)
                    {
                        if (prevMilStructureId != -2)
                            milRepStatusCounter = 1;

                        prevMilStructureId = reportBlock.MilitaryStructureID;
                    }

                    string milRepStatusCounterStr = milRepStatusCounter.ToString() + " ";

                    if (milRepStatusCounter > 6)
                        milRepStatusCounterStr = " - ";

                    string firstCols = "";

                    if (reportBlock.MilRepSpecialityID > 0 && reportBlock.RowType == 1)
                    {
                        firstCols = @"<td style='" + cellStyle + @"'>" + (milRepStatusCounter == 1 ? reportBlock.MilRepSpecialityCode : "") + @"</td>
                                      <td style='" + cellStyle + @"'>" + milRepStatusCounterStr + reportBlock.MilRepStatusName + @"</td>";
                    }
                    else if (reportBlock.MilRepSpecialityID > 0 && reportBlock.RowType == 2)
                    {
                        firstCols = @"<td style='" + cellStyle + @"' colspan='2'>Всичко за ВОС " + reportBlock.MilRepSpecialityCode + @"</td>";
                    }
                    else if (reportBlock.MilitaryForceSortID > 0 && reportBlock.RowType == 1)
                    {
                        if (milRepStatusCounter == 1)
                        {
                            html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                        <td style='" + cellStyle + @"' colspan='19'>" + reportBlock.MilitaryForceSortName + @"</td>
                                      </tr>";

                            counter++;
                        }

                        firstCols = @"<td style='" + cellStyle + @"'></td>
                                      <td style='" + cellStyle + @"'>" + milRepStatusCounterStr + reportBlock.MilRepStatusName + @"</td>";
                    }
                    else if (reportBlock.MilitaryForceSortID > 0 && reportBlock.RowType == 2)
                    {
                        firstCols = @"<td style='" + cellStyle + @"' colspan='2'>Всичко за рода войски</td>";
                    }
                    else if (reportBlock.MilitaryForceTypeID > 0 && reportBlock.RowType == 1)
                    {
                        if (milRepStatusCounter == 1)
                        {
                            html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                        <td style='" + cellStyle + @"' colspan='19'>" + reportBlock.MilitaryForceTypeName + @"</td>
                                      </tr>";

                            counter++;
                        }

                        firstCols = @"<td style='" + cellStyle + @"'></td>
                                      <td style='" + cellStyle + @"'>" + milRepStatusCounterStr + reportBlock.MilRepStatusName + @"</td>";
                    }
                    else if (reportBlock.MilitaryForceTypeID > 0 && reportBlock.RowType == 2)
                    {
                        firstCols = @"<td style='" + cellStyle + @"' colspan='2'>Всичко за вида войски</td>";
                    }
                    else if (reportBlock.MilitaryStructureID > 0 && reportBlock.RowType == 1)
                    {
                        if (milRepStatusCounter == 1)
                        {
                            html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                        <td style='" + cellStyle + @"' colspan='19'>" + reportBlock.MilitaryStructureName + @"</td>
                                      </tr>";

                            counter++;
                        }

                        firstCols = @"<td style='" + cellStyle + @"'></td>
                                      <td style='" + cellStyle + @"'>" + milRepStatusCounterStr + reportBlock.MilRepStatusName + @"</td>";
                    }
                    else if (reportBlock.MilitaryStructureID > 0 && reportBlock.RowType == 2)
                    {
                        firstCols = @"<td style='" + cellStyle + @"' colspan='2'>Всичко за структурата</td>";
                    }
                    else if (reportBlock.RowType == 1)
                    {
                        firstCols = @"<td style='" + cellStyle + @"'></td>
                                      <td style='" + cellStyle + @"'>" + milRepStatusCounterStr + reportBlock.MilRepStatusName + @"</td>";
                    }
                    else if (reportBlock.RowType == 0)
                    {
                        firstCols = @"<td style='" + cellStyle + @"' colspan='2'>Водят се на отчет</td>";
                    }


                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 " + firstCols + @"
                                 <td style='" + cellStyle + @" text-align: right;'>" + reportBlock.ClassA_Of_Cnt + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + reportBlock.ClassA_OfCand_Cnt + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + reportBlock.ClassA_Ser_Cnt + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + reportBlock.ClassA_Sol_Cnt + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + reportBlock.ClassB_Of_Cnt + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + reportBlock.ClassB_OfCand_Cnt + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + reportBlock.ClassB_Ser_Cnt + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + reportBlock.ClassB_Sol_Cnt + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + reportBlock.ClassC_Of_Cnt + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + reportBlock.ClassC_OfCand_Cnt + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + reportBlock.ClassC_Ser_Cnt + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + reportBlock.ClassC_Sol_Cnt + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + reportBlock.Total_Of_Cnt + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + reportBlock.Total_OfCand_Cnt + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + reportBlock.Total_Ser_Cnt + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + reportBlock.Total_Sol_Cnt + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + reportBlock.TotalCnt + @"</td>
                              </tr>";

                    counter++;
                }

                html += "</table>";                
            }

            //Refresh the paging button according to the current position
            SetImgBtns();
            lblPagination.Text = " | " + hdnPageIdx.Value + " от " + ReportResult.MaxPage.ToString() + " | ";
            txtGotoPage.Text = "";
            
            this.pnlReportGrid.InnerHtml = html;
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
            Response.Redirect("~/ContentPages/Home.aspx");
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            hdnMilitaryDepartmentSelected.Value = "";
            ddMilitaryForceSort.SelectedValue = ListItems.GetOptionAll().Value;

            hdnMilitaryReportSpecialitiesSelected.Value = "";
            PopulateMilitaryReportSpecialities();

            pnlPaging.Visible = false;
            pnlReportGrid.InnerHtml = "";
            pnlSearchHint.Visible = true;

            Session[sessionResultsKey] = null;
            reportResult = null;
        }

        private ReportSV1Filter CollectFilterData()
        {
            ReportSV1Filter filter = new ReportSV1Filter();

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

            if (ddMilitaryForceSort.SelectedValue != ListItems.GetOptionAll().Value)
            {
                filter.MilitaryForceSortIds = ddMilitaryForceSort.SelectedValue;
                hdnMilitaryForceSortId.Value = ddMilitaryForceSort.SelectedValue;
            }
            else
            {
                hdnMilitaryForceSortId.Value = "";
            }

            filter.MilitaryReportSpecialityIds = hdnMilitaryReportSpecialitiesSelected.Value;
            hdnMilitaryReportSpecialityId.Value = hdnMilitaryReportSpecialitiesSelected.Value;
                        
            filter.Region = region;
            filter.Municipality = municiplaity;
            filter.City = city;
            filter.District = district;
            filter.Address = txtAddress.Text;
            filter.PostCode = txtPostCode.Text;

            return filter;
        }

        protected void ddMilitaryForceSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateMilitaryReportSpecialities();
        }
    }
}
