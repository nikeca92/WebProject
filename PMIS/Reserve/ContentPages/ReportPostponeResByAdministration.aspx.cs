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
using System.Text;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class ReportPostponeResByAdministration : RESPage
    {
        private string sessionResultsKey = "ReportPostponeResByAdministrationResult";

        private ReportPostponeResByAdministrationResult reportResult = null;
        private ReportPostponeResByAdministrationResult ReportResult
        {
            get
            {
                if (reportResult == null)
                {
                    if (Session[sessionResultsKey] != null)
                    {
                        reportResult = (ReportPostponeResByAdministrationResult)Session[sessionResultsKey];
                    }
                    else
                    {
                        ReportPostponeResByAdministrationFilter filter = CollectFilterData();
                        reportResult = ReportPostponeResByAdministrationUtil.GetReportPostponeResByAdministration(filter, CurrentUser);
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
                return "RES_POSTPONE_REPORT_RES_BY_ADMINISTRATION";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.GetUIItemAccessLevel("RES_POSTPONE_REPORT_RES_BY_ADMINISTRATION") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }         
           
            //Hilight the current page in the menu bar
            HighlightMenuItems("Postpone", "ReportPostponeResByAdministration");

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
            
            btnRefresh.Attributes.Add("onclick", "SetPickListsSelection(); if(!IsFilterDataValid()) return false;");
            btnExport.Attributes.Add("onclick", "SetPickListsSelection();");
            ddPostponeYears.Attributes.Add("onchange", "SetPickListsSelection();");
            ddRegion.Attributes.Add("onchange", "SetPickListsSelection();");
            ddMuniciplaity.Attributes.Add("onchange", "SetPickListsSelection();");
            ddCity.Attributes.Add("onchange", "SetPickListsSelection();");

            btnPickListMilitaryDepartmentsChanged.Style.Add("display", "none");
        }             

        //Populate all listboxes on the screen
        private void PopulateLists()
        {
            PopulateMilitaryDepartments();
            PopulatePostponeYears();
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

        protected void btnPickListMilitaryDepartmentsChanged_Clicked(object sender, EventArgs e)
        {
            //Nothing here, but it is ready to do something (e.g. pre-fill some filters)
        }

        private void PopulatePostponeYears()
        {
            List<int> currYear = new List<int>();
            currYear.Add(DateTime.Now.Year);
            List<int> postponeYears = PostponeResUtil.GetAllPostponeResYears(CurrentUser);
            List<int> fulfilYears = PostponeResUtil.GetAllPostponeResFulfilYears(CurrentUser);

            List<int> years = currYear.Union(postponeYears).Union(fulfilYears).OrderByDescending(x => x).ToList();

            ddPostponeYears.Items.Clear();

            foreach (int year in years)
            {
                ListItem li = new ListItem();
                li.Text = year.ToString();
                li.Value = year.ToString();

                if (year == currYear[0])
                    li.Selected = true;

                ddPostponeYears.Items.Add(li);
            }
        }

        protected void ddPostponeYears_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Nothing here, but it is ready to do something (e.g. pre-fill some filters
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
        }

        protected void ddCity_Changed(object sender, EventArgs e)
        {
            //Nothing here, but it is ready to do something (e.g. pre-fill some filters
        }

       
        //Setup some styling on the page
        private void SetupStyle()
        {
            lblMilitaryDepartment.Style.Add("vertical-align", "middle");
        }

        //Refresh the data grid
        private void RefreshReport()
        {
            string html = "";

            //Get the list of postpones according to the specified filters
            List<ReportPostponeResByAdministrationBlock> reportBlocks = ReportResult.AllBlocks;

            //No data found
            if (reportBlocks.Count == 0)
            {
                html = "<span>Няма намерени резултати</span>";
            }
            //If there is data then generate dynamically the HTML for the data grid
            else
            {   
                //Setup the header of the grid
                html = @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <colgroup>
                            <col style='width: 40px;'>
                            <col style='width: 200px;'>
                            <col style='width: 60px;'>
                            <col style='width: 60px;'>
                            <col style='width: 60px;'>
                            <col style='width: 60px;'>
                            <col style='width: 60px;'>
                            <col style='width: 60px;'>
                            <col style='width: 60px;'>
                            <col style='width: 60px;'>
                            <col style='width: 60px;'>
                            <col style='width: 60px;'>
                         </colgroup>
                         <thead>
                            <tr>
                               <th rowspan='3'>№ по ред</th>
                               <th rowspan='3'>Вид отсрочване</th>
                               <th colspan='10'>Запасни, заявени за безусловно и условно отсрочване</th>
                            </tr>
                            <tr>
                               <th colspan='2'>Общо запасни</th>
                               <th colspan='2'>Офицери</th>
                               <th colspan='2'>Офицерски кандидати</th>
                               <th colspan='2'>Сержанти / старшини</th>
                               <th colspan='2'>Войници / матроси</th>
                            </tr>
                            <tr>
                               <th>предл.</th>
                               <th>изпълн.</th>
                               <th>предл.</th>
                               <th>изпълн.</th>
                               <th>предл.</th>
                               <th>изпълн.</th>
                               <th>предл.</th>
                               <th>изпълн.</th>
                               <th>предл.</th>
                               <th>изпълн.</th>
                            </tr> 
                         </thead>";

                int counter = 1;

                int totalTotalAbsolutelyPostpone = 0;
                int totalTotalAbsolutelyFulfil = 0;
                int totalTotalConditionedPostpone = 0;
                int totalTotalConditionedFulfil = 0;
                int totalOfficersAbsolutelyPostpone = 0;
                int totalOfficersAbsolutelyFulfil = 0;
                int totalOfficersConditionedPostpone = 0;
                int totalOfficersConditionedFulfil = 0;
                int totalOfCandAbsolutelyPostpone = 0;
                int totalOfCandAbsolutelyFulfil = 0;
                int totalOfCandConditionedPostpone = 0;
                int totalOfCandConditionedFulfil = 0;
                int totalSergeantsAbsolutelyPostpone = 0;
                int totalSergeantsAbsolutelyFulfil = 0;
                int totalSergeantsConditionedPostpone = 0;
                int totalSergeantsConditionedFulfil = 0;
                int totalSoldiersAbsolutelyPostpone = 0;
                int totalSoldiersAbsolutelyFulfil = 0;
                int totalSoldiersConditionedPostpone = 0;
                int totalSoldiersConditionedFulfil = 0;

                foreach (ReportPostponeResByAdministrationBlock reportBlock in reportBlocks)
                {
                    int itemTotalAbsolutelyPostpone =
                        reportBlock.OfficersAbsolutelyPostpone +
                        reportBlock.OfCandAbsolutelyPostpone +
                        reportBlock.SergeantsAbsolutelyPostpone +
                        reportBlock.SoldiersAbsolutelyPostpone;

                    int itemTotalAbsolutelyFulfil =
                        reportBlock.OfficersAbsolutelyFulfil +
                        reportBlock.OfCandAbsolutelyFulfil +
                        reportBlock.SergeantsAbsolutelyFulfil +
                        reportBlock.SoldiersAbsolutelyFulfil;

                    int itemTotalConditionedPostpone =
                        reportBlock.OfficersConditionedPostpone +
                        reportBlock.OfCandConditionedPostpone +
                        reportBlock.SergeantsConditionedPostpone +
                        reportBlock.SoldiersConditionedPostpone;

                    int itemTotalConditionedFulfil =
                        reportBlock.OfficersConditionedFulfil +
                        reportBlock.OfCandConditionedFulfil +
                        reportBlock.SergeantsConditionedFulfil +
                        reportBlock.SoldiersConditionedFulfil;

                    totalTotalAbsolutelyPostpone += itemTotalAbsolutelyPostpone;
                    totalTotalAbsolutelyFulfil += itemTotalAbsolutelyFulfil;
                    totalTotalConditionedPostpone += itemTotalConditionedPostpone;
                    totalTotalConditionedFulfil += itemTotalConditionedFulfil;
                    totalOfficersAbsolutelyPostpone += reportBlock.OfficersAbsolutelyPostpone;
                    totalOfficersAbsolutelyFulfil += reportBlock.OfficersAbsolutelyFulfil;
                    totalOfficersConditionedPostpone += reportBlock.OfficersConditionedPostpone;
                    totalOfficersConditionedFulfil += reportBlock.OfficersConditionedFulfil;
                    totalOfCandAbsolutelyPostpone += reportBlock.OfCandAbsolutelyPostpone;
                    totalOfCandAbsolutelyFulfil += reportBlock.OfCandAbsolutelyFulfil;
                    totalOfCandConditionedPostpone += reportBlock.OfCandConditionedPostpone;
                    totalOfCandConditionedFulfil += reportBlock.OfCandConditionedFulfil;
                    totalSergeantsAbsolutelyPostpone += reportBlock.SergeantsAbsolutelyPostpone;
                    totalSergeantsAbsolutelyFulfil += reportBlock.SergeantsAbsolutelyFulfil;
                    totalSergeantsConditionedPostpone += reportBlock.SergeantsConditionedPostpone;
                    totalSergeantsConditionedFulfil += reportBlock.SergeantsConditionedFulfil;
                    totalSoldiersAbsolutelyPostpone += reportBlock.SoldiersAbsolutelyPostpone;
                    totalSoldiersAbsolutelyFulfil += reportBlock.SoldiersAbsolutelyFulfil;
                    totalSoldiersConditionedPostpone += reportBlock.SoldiersConditionedPostpone;
                    totalSoldiersConditionedFulfil += reportBlock.SoldiersConditionedFulfil;
                    
                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td colspan='12' style='text-align: center; font-weight: bold; padding-top: 5px; padding-bottom: 5px;'>" + reportBlock.AdministrationName + @"</td>
                              </tr>
                              <tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td></td>
                                 <td style='text-align: left; font-style: italic;'>- безусловно отсрочени</td>
                                 <td style='text-align: center;'>" + itemTotalAbsolutelyPostpone.ToString() + @"</td>
                                 <td style='text-align: center;'>" + itemTotalAbsolutelyFulfil.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportBlock.OfficersAbsolutelyPostpone.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportBlock.OfficersAbsolutelyFulfil.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportBlock.OfCandAbsolutelyPostpone.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportBlock.OfCandAbsolutelyFulfil.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportBlock.SergeantsAbsolutelyPostpone.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportBlock.SergeantsAbsolutelyFulfil.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportBlock.SoldiersAbsolutelyPostpone.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportBlock.SoldiersAbsolutelyFulfil.ToString() + @"</td>
                              </tr>
                              <tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td></td>
                                 <td style='text-align: left; font-style: italic;'>- условно отсрочени</td>
                                 <td style='text-align: center;'>" + itemTotalConditionedPostpone.ToString() + @"</td>
                                 <td style='text-align: center;'>" + itemTotalConditionedFulfil.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportBlock.OfficersConditionedPostpone.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportBlock.OfficersConditionedFulfil.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportBlock.OfCandConditionedPostpone.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportBlock.OfCandConditionedFulfil.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportBlock.SergeantsConditionedPostpone.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportBlock.SergeantsConditionedFulfil.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportBlock.SoldiersConditionedPostpone.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportBlock.SoldiersConditionedFulfil.ToString() + @"</td>
                              </tr>";

                    counter++;
                }

                html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td colspan='12' style='text-align: center; font-weight: bold; padding-top: 5px; padding-bottom: 5px;'>Всичко</td>
                              </tr>
                              <tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td></td>
                                 <td style='text-align: left; font-style: italic; font-weight: bold;'>- безусловно отсрочени</td>
                                 <td style='text-align: center; font-weight: bold;'>" + totalTotalAbsolutelyPostpone.ToString() + @"</td>
                                 <td style='text-align: center; font-weight: bold;'>" + totalTotalAbsolutelyFulfil.ToString() + @"</td>
                                 <td style='text-align: center; font-weight: bold;'>" + totalOfficersAbsolutelyPostpone.ToString() + @"</td>
                                 <td style='text-align: center; font-weight: bold;'>" + totalOfficersAbsolutelyFulfil.ToString() + @"</td>
                                 <td style='text-align: center; font-weight: bold;'>" + totalOfCandAbsolutelyPostpone.ToString() + @"</td>
                                 <td style='text-align: center; font-weight: bold;'>" + totalOfCandAbsolutelyFulfil.ToString() + @"</td>
                                 <td style='text-align: center; font-weight: bold;'>" + totalSergeantsAbsolutelyPostpone.ToString() + @"</td>
                                 <td style='text-align: center; font-weight: bold;'>" + totalSergeantsAbsolutelyFulfil.ToString() + @"</td>
                                 <td style='text-align: center; font-weight: bold;'>" + totalSoldiersAbsolutelyPostpone.ToString() + @"</td>
                                 <td style='text-align: center; font-weight: bold;'>" + totalSoldiersAbsolutelyFulfil.ToString() + @"</td>
                              </tr>
                              <tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td></td>
                                 <td style='text-align: left; font-style: italic; font-weight: bold;'>- условно отсрочени</td>
                                 <td style='text-align: center; font-weight: bold;'>" + totalTotalConditionedPostpone.ToString() + @"</td>
                                 <td style='text-align: center; font-weight: bold;'>" + totalTotalConditionedFulfil.ToString() + @"</td>
                                 <td style='text-align: center; font-weight: bold;'>" + totalOfficersConditionedPostpone.ToString() + @"</td>
                                 <td style='text-align: center; font-weight: bold;'>" + totalOfficersConditionedFulfil.ToString() + @"</td>
                                 <td style='text-align: center; font-weight: bold;'>" + totalOfCandConditionedPostpone.ToString() + @"</td>
                                 <td style='text-align: center; font-weight: bold;'>" + totalOfCandConditionedFulfil.ToString() + @"</td>
                                 <td style='text-align: center; font-weight: bold;'>" + totalSergeantsConditionedPostpone.ToString() + @"</td>
                                 <td style='text-align: center; font-weight: bold;'>" + totalSergeantsConditionedFulfil.ToString() + @"</td>
                                 <td style='text-align: center; font-weight: bold;'>" + totalSoldiersConditionedPostpone.ToString() + @"</td>
                                 <td style='text-align: center; font-weight: bold;'>" + totalSoldiersConditionedFulfil.ToString() + @"</td>
                              </tr>";

                html += "</table>";

                //Put the summary stats above the results grid
                decimal totalTotalAbsolutelyPerc = 0;
                if (totalTotalAbsolutelyPostpone != 0)
                    totalTotalAbsolutelyPerc = totalTotalAbsolutelyFulfil * 100.0m / totalTotalAbsolutelyPostpone;

                decimal totalTotalConditionedPerc = 0;
                if (totalTotalConditionedPostpone != 0)
                    totalTotalConditionedPerc = totalTotalConditionedFulfil * 100.0m / totalTotalConditionedPostpone;

                html = @"<table class='SummaryStatsTable'>
                            <tr>
                               <td></td>
                               <td>Заявено</td>
                               <td>Изпълнено</td>
                               <td>% изпълн.</td>
                            </tr>
                            <tr>
                               <td style='text-align: left;'>Общо запасни, предлагани за безусловно отсрочване</td>
                               <td style='text-align: right;'>" + totalTotalAbsolutelyPostpone.ToString() + @" души</td>
                               <td style='text-align: right;'>" + totalTotalAbsolutelyFulfil.ToString() + @" души</td>
                               <td style='text-align: right;'>" + totalTotalAbsolutelyPerc.ToString("0") + @" %</td>
                            </tr>
                            <tr>
                               <td style='text-align: left;'>Общо запасни, предлагани за условно отсрочване</td>
                               <td style='text-align: right;'>" + totalTotalConditionedPostpone.ToString() + @" души</td>
                               <td style='text-align: right;'>" + totalTotalConditionedFulfil.ToString() + @" души</td>
                               <td style='text-align: right;'>" + totalTotalConditionedPerc.ToString("0") + @" %</td>
                            </tr>
                         </table>
                         <div style='height: 15px;'></div>" +
                         html;
            }

            this.pnlReportGrid.InnerHtml = html;
        }

        private string GenerateExcelOutput()
        {
            StringBuilder sb = new StringBuilder();
            
            sb.Append(@"<?xml version=""1.0""?>
                        <?mso-application progid=""Excel.Sheet""?>
                        <Workbook xmlns=""urn:schemas-microsoft-com:office:spreadsheet""
                                  xmlns:o=""urn:schemas-microsoft-com:office:office""
                                  xmlns:x=""urn:schemas-microsoft-com:office:excel""
                                  xmlns:ss=""urn:schemas-microsoft-com:office:spreadsheet""
                                  xmlns:html=""http://www.w3.org/TR/REC-html40"">                       
                            <ExcelWorkbook xmlns=""urn:schemas-microsoft-com:office:excel"">
                            </ExcelWorkbook>
                            <Styles>
                                <Style ss:ID=""HT1"">
                                    <Alignment ss:Horizontal=""Center"" ss:Vertical=""Bottom"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS"" ss:Size=""16"" ss:Bold=""1""/>
                                </Style>
                                <Style ss:ID=""HT2"">
                                    <Alignment ss:Horizontal=""Center"" ss:Vertical=""Bottom"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS"" ss:Size=""20"" ss:Bold=""1""/>
                                </Style>
                                <Style ss:ID=""HT3"">
                                    <Alignment ss:Horizontal=""Center"" ss:Vertical=""Bottom"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS"" ss:Size=""13"" ss:Bold=""1""/>
                                </Style>
                                <Style ss:ID=""DT"">
                                    <Alignment ss:Vertical=""Bottom"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS""/>
                                    <Borders>
                                       <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                    </Borders>
                                </Style>
                                <Style ss:ID=""DTC"">
                                    <Alignment ss:Vertical=""Bottom"" ss:Horizontal=""Center"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS""/>
                                    <Borders>
                                       <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                    </Borders>
                                </Style>
                                <Style ss:ID=""DTB"">
                                    <Alignment ss:Vertical=""Bottom"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS"" ss:Bold=""1"" />
                                    <Borders>
                                       <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                    </Borders>
                                </Style>
                                <Style ss:ID=""DTBC"">
                                    <Alignment ss:Horizontal=""Center"" ss:Vertical=""Bottom"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS"" ss:Bold=""1"" />
                                    <Borders>
                                       <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                    </Borders>
                                </Style>
                                <Style ss:ID=""DTBI"">
                                    <Alignment ss:Vertical=""Bottom"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS"" ss:Bold=""1"" ss:Italic=""1"" />
                                    <Borders>
                                       <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                    </Borders>
                                </Style>
                                <Style ss:ID=""DTI"">
                                    <Alignment ss:Vertical=""Bottom"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS"" ss:Italic=""1"" />
                                    <Borders>
                                       <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                    </Borders>
                                </Style>
                                <Style ss:ID=""N"">
                                    <Alignment ss:Vertical=""Bottom"" ss:Horizontal=""Center"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS""/>
                                    <Borders>
                                       <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                    </Borders>
                                </Style>
                                <Style ss:ID=""NB"">
                                    <Alignment ss:Vertical=""Bottom"" ss:Horizontal=""Center"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS"" ss:Bold=""1"" />
                                    <Borders>
                                       <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                    </Borders>
                                </Style>
                                <Style ss:ID=""P"">
                                    <Alignment ss:Vertical=""Bottom"" ss:Horizontal=""Center"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS""/>
                                    <NumberFormat ss:Format=""0%""/>
                                    <Borders>
                                       <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                    </Borders>
                                </Style>
                                <Style ss:ID=""FL"">
                                    <Alignment ss:Vertical=""Bottom"" ss:Horizontal=""Left"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS""/>
                                </Style>
                                <Style ss:ID=""FV"">
                                    <Alignment ss:Vertical=""Bottom"" ss:Horizontal=""Left"" ss:WrapText=""0""/>
                                    <Font ss:FontName=""Arial Unicode MS"" ss:Italic=""1""/>
                                </Style>
                            </Styles>
                            <Worksheet ss:Name=""Отсрочване на запасни - отчет"">
                                <Table>
                                    <Column ss:Width=""30""/>
                                    <Column ss:Width=""150""/>
                                    <Column ss:Width=""60""/>
                                    <Column ss:Width=""60""/>
                                    <Column ss:Width=""60""/>
                                    <Column ss:Width=""60""/>
                                    <Column ss:Width=""60""/>
                                    <Column ss:Width=""60""/>
                                    <Column ss:Width=""60""/>
                                    <Column ss:Width=""60""/>
                                    <Column ss:Width=""60""/>
                                    <Column ss:Width=""60""/>
                                    <Row ss:AutoFitHeight=""0"" ss:Height=""20.25"">
                                        <Cell ss:MergeAcross=""11"" ss:StyleID=""HT1""><Data ss:Type=""String"">АСУ на човешките ресурси</Data></Cell>
                                    </Row>
                                    <Row ss:AutoFitHeight=""0"" ss:Height=""26.25"">
                                        <Cell ss:MergeAcross=""11"" ss:StyleID=""HT2""><Data ss:Type=""String"">Отчет на ресурсите от резерва</Data></Cell>
                                    </Row>
                                    <Row ss:AutoFitHeight=""0"" ss:Height=""16.5"">
                                        <Cell ss:MergeAcross=""11"" ss:StyleID=""HT3""><Data ss:Type=""String"">Отчет за изпълнение отсрочването на запасни</Data></Cell>
                                    </Row>
                                    <Row>
                                        <Cell/>
                                    </Row>
                                    <Row>
                                        <Cell ss:MergeAcross=""1"" ss:StyleID=""FL""><Data ss:Type=""String"">Военно окръжие:</Data></Cell>
                                        <Cell ss:StyleID=""FV""><Data ss:Type=""String"">" + ReportResult.Filter.MilitaryDepartmentDisplayText + @"</Data></Cell>
                                    </Row>
                                    <Row>
                                        <Cell ss:MergeAcross=""1"" ss:StyleID=""FL""><Data ss:Type=""String"">Година:</Data></Cell>
                                        <Cell ss:StyleID=""FV""><Data ss:Type=""Number"">" + ReportResult.Filter.PostponeYearDisplayText + @"</Data></Cell>
                                    </Row>
                                    <Row>
                                        <Cell ss:MergeAcross=""1"" ss:StyleID=""FL""><Data ss:Type=""String"">Област:</Data></Cell>
                                        <Cell ss:StyleID=""FV""><Data ss:Type=""String"">" + ReportResult.Filter.RegionDisplayText + @"</Data></Cell>
                                    </Row>
                                    <Row>
                                        <Cell ss:MergeAcross=""1"" ss:StyleID=""FL""><Data ss:Type=""String"">Община:</Data></Cell>
                                        <Cell ss:StyleID=""FV""><Data ss:Type=""String"">" + ReportResult.Filter.MunicipalityDisplayText + @"</Data></Cell>
                                    </Row>
                                    <Row>
                                        <Cell ss:MergeAcross=""1"" ss:StyleID=""FL""><Data ss:Type=""String"">Населено място:</Data></Cell>
                                        <Cell ss:StyleID=""FV""><Data ss:Type=""String"">" + ReportResult.Filter.CityDisplayText + @"</Data></Cell>
                                    </Row>
                                    
                           ");

            //Get the list of postpones according to the specified filters
            List<ReportPostponeResByAdministrationBlock> reportBlocks = ReportResult.AllBlocks;

            //No data found
            if (reportBlocks.Count == 0)
            {
                sb.Append(@"<Row><Cell/></Row>
                            <Row>
                                <Cell><Data ss:Type=""String"">Няма намерени резултати</Data></Cell>
                            </Row>");
            }
            else
            {
                sb.Append(@"<Row><Cell/></Row>
                            <Row>
                               <Cell ss:MergeAcross=""3"" ss:StyleID=""DT""><Data ss:Type=""String""></Data></Cell>
                               <Cell ss:StyleID=""DTC""><Data ss:Type=""String"">Заявено</Data></Cell>
                               <Cell ss:StyleID=""DTC""><Data ss:Type=""String"">Изпълнено</Data></Cell>
                               <Cell ss:StyleID=""DTC""><Data ss:Type=""String"">% изпълн.</Data></Cell> 
                            </Row>
                            <Row>
                               <Cell ss:MergeAcross=""3"" ss:StyleID=""DT""><Data ss:Type=""String"">Общо запасни, предлагани за безусловно отсрочване</Data></Cell>
                               <Cell ss:StyleID=""N"" ss:Formula=""=R[" + (8 + 3 * reportBlocks.Count) + @"]C[-2]""><Data ss:Type=""Number""></Data></Cell>
                               <Cell ss:StyleID=""N"" ss:Formula=""=R[" + (8 + 3 * reportBlocks.Count) + @"]C[-2]""><Data ss:Type=""Number""></Data></Cell>
                               <Cell ss:StyleID=""P"" ss:Formula=""=IF(RC[-2]=0, 0, RC[-1]/RC[-2])""><Data ss:Type=""Number""></Data></Cell> 
                            </Row>
                            <Row>
                               <Cell ss:MergeAcross=""3"" ss:StyleID=""DT""><Data ss:Type=""String"">Общо запасни, предлагани за условно отсрочване</Data></Cell>
                               <Cell ss:StyleID=""N"" ss:Formula=""=R[" + (8 + 3 * reportBlocks.Count) + @"]C[-2]""><Data ss:Type=""Number""></Data></Cell>
                               <Cell ss:StyleID=""N"" ss:Formula=""=R[" + (8 + 3 * reportBlocks.Count) + @"]C[-2]""><Data ss:Type=""Number""></Data></Cell>
                               <Cell ss:StyleID=""P"" ss:Formula=""=IF(RC[-2]=0, 0, RC[-1]/RC[-2])""><Data ss:Type=""Number""></Data></Cell> 
                            </Row>
                            <Row><Cell/></Row>
                            <Row>
                               <Cell ss:MergeDown=""2"" ss:StyleID=""DTC""><Data ss:Type=""String"">№ по ред</Data></Cell>
                               <Cell ss:MergeDown=""2"" ss:StyleID=""DTC""><Data ss:Type=""String"">Вид отсрочване</Data></Cell>
                               <Cell ss:MergeAcross=""9"" ss:StyleID=""DTC""><Data ss:Type=""String"">Запасни, заявени за безусловно и условно отсрочване</Data></Cell>
                            </Row>
                            <Row>
                               <Cell ss:Index=""3"" ss:MergeAcross=""1"" ss:StyleID=""DTC""><Data ss:Type=""String"">Общо запасни</Data></Cell>
                               <Cell ss:MergeAcross=""1"" ss:StyleID=""DTC""><Data ss:Type=""String"">Офицери</Data></Cell>
                               <Cell ss:MergeAcross=""1"" ss:StyleID=""DTC""><Data ss:Type=""String"">Офицерски кандидати</Data></Cell>
                               <Cell ss:MergeAcross=""1"" ss:StyleID=""DTC""><Data ss:Type=""String"">Сержанти / старшини</Data></Cell>
                               <Cell ss:MergeAcross=""1"" ss:StyleID=""DTC""><Data ss:Type=""String"">Войници / матроси</Data></Cell>
                            </Row>
                            <Row>
                               <Cell ss:Index=""3"" ss:StyleID=""DTC""><Data ss:Type=""String"">предл.</Data></Cell>
                               <Cell ss:StyleID=""DTC""><Data ss:Type=""String"">изпълн.</Data></Cell>
                               <Cell ss:StyleID=""DTC""><Data ss:Type=""String"">предл.</Data></Cell>
                               <Cell ss:StyleID=""DTC""><Data ss:Type=""String"">изпълн.</Data></Cell>
                               <Cell ss:StyleID=""DTC""><Data ss:Type=""String"">предл.</Data></Cell>
                               <Cell ss:StyleID=""DTC""><Data ss:Type=""String"">изпълн.</Data></Cell>
                               <Cell ss:StyleID=""DTC""><Data ss:Type=""String"">предл.</Data></Cell>
                               <Cell ss:StyleID=""DTC""><Data ss:Type=""String"">изпълн.</Data></Cell>
                               <Cell ss:StyleID=""DTC""><Data ss:Type=""String"">предл.</Data></Cell>
                               <Cell ss:StyleID=""DTC""><Data ss:Type=""String"">изпълн.</Data></Cell>
                            </Row>
                            <Row>
                               <Cell ss:StyleID=""DTC""><Data ss:Type=""Number"">1</Data></Cell>
                               <Cell ss:StyleID=""DTC""><Data ss:Type=""Number"">2</Data></Cell>
                               <Cell ss:StyleID=""DTC""><Data ss:Type=""Number"">3</Data></Cell>
                               <Cell ss:StyleID=""DTC""><Data ss:Type=""Number"">4</Data></Cell>
                               <Cell ss:StyleID=""DTC""><Data ss:Type=""Number"">5</Data></Cell>
                               <Cell ss:StyleID=""DTC""><Data ss:Type=""Number"">6</Data></Cell>
                               <Cell ss:StyleID=""DTC""><Data ss:Type=""Number"">7</Data></Cell>
                               <Cell ss:StyleID=""DTC""><Data ss:Type=""Number"">8</Data></Cell>
                               <Cell ss:StyleID=""DTC""><Data ss:Type=""Number"">9</Data></Cell>
                               <Cell ss:StyleID=""DTC""><Data ss:Type=""Number"">10</Data></Cell>
                               <Cell ss:StyleID=""DTC""><Data ss:Type=""Number"">11</Data></Cell>
                               <Cell ss:StyleID=""DTC""><Data ss:Type=""Number"">12</Data></Cell>
                            </Row>");

                int counter = 1;                

                foreach (ReportPostponeResByAdministrationBlock reportBlock in reportBlocks)
                {
                    sb.Append(@"<Row>
                                    <Cell ss:MergeAcross=""11"" ss:StyleID=""DTBC""><Data ss:Type=""String"">" + reportBlock.AdministrationName + @"</Data></Cell>
                                </Row>
                                <Row>
                                    <Cell ss:StyleID=""DT"" />
                                    <Cell ss:StyleID=""DTI""><Data ss:Type=""String"">- безусловно отсрочени</Data></Cell>
                                    <Cell ss:StyleID=""N"" ss:Formula=""=RC[2]+RC[4]+RC[6]+RC[8]""><Data ss:Type=""Number""></Data></Cell>
                                    <Cell ss:StyleID=""N"" ss:Formula=""=RC[2]+RC[4]+RC[6]+RC[8]""><Data ss:Type=""Number""></Data></Cell>
                                    <Cell ss:StyleID=""N""><Data ss:Type=""Number"">" + reportBlock.OfficersAbsolutelyPostpone.ToString() + @"</Data></Cell>
                                    <Cell ss:StyleID=""N""><Data ss:Type=""Number"">" + reportBlock.OfficersAbsolutelyFulfil.ToString() + @"</Data></Cell>
                                    <Cell ss:StyleID=""N""><Data ss:Type=""Number"">" + reportBlock.OfCandAbsolutelyPostpone.ToString() + @"</Data></Cell>
                                    <Cell ss:StyleID=""N""><Data ss:Type=""Number"">" + reportBlock.OfCandAbsolutelyFulfil.ToString() + @"</Data></Cell>
                                    <Cell ss:StyleID=""N""><Data ss:Type=""Number"">" + reportBlock.SergeantsAbsolutelyPostpone.ToString() + @"</Data></Cell>
                                    <Cell ss:StyleID=""N""><Data ss:Type=""Number"">" + reportBlock.SergeantsAbsolutelyFulfil.ToString() + @"</Data></Cell>
                                    <Cell ss:StyleID=""N""><Data ss:Type=""Number"">" + reportBlock.SoldiersAbsolutelyPostpone.ToString() + @"</Data></Cell>
                                    <Cell ss:StyleID=""N""><Data ss:Type=""Number"">" + reportBlock.SoldiersAbsolutelyFulfil.ToString() + @"</Data></Cell>
                               </Row>
                               <Row>
                                    <Cell ss:StyleID=""DT"" />
                                    <Cell ss:StyleID=""DTI""><Data ss:Type=""String"">- условно отсрочени</Data></Cell>
                                    <Cell ss:StyleID=""N"" ss:Formula=""=RC[2]+RC[4]+RC[6]+RC[8]""><Data ss:Type=""Number""></Data></Cell>
                                    <Cell ss:StyleID=""N"" ss:Formula=""=RC[2]+RC[4]+RC[6]+RC[8]""><Data ss:Type=""Number""></Data></Cell>
                                    <Cell ss:StyleID=""N""><Data ss:Type=""Number"">" + reportBlock.OfficersConditionedPostpone.ToString() + @"</Data></Cell>
                                    <Cell ss:StyleID=""N""><Data ss:Type=""Number"">" + reportBlock.OfficersConditionedFulfil.ToString() + @"</Data></Cell>
                                    <Cell ss:StyleID=""N""><Data ss:Type=""Number"">" + reportBlock.OfCandConditionedPostpone.ToString() + @"</Data></Cell>
                                    <Cell ss:StyleID=""N""><Data ss:Type=""Number"">" + reportBlock.OfCandConditionedFulfil.ToString() + @"</Data></Cell>
                                    <Cell ss:StyleID=""N""><Data ss:Type=""Number"">" + reportBlock.SergeantsConditionedPostpone.ToString() + @"</Data></Cell>
                                    <Cell ss:StyleID=""N""><Data ss:Type=""Number"">" + reportBlock.SergeantsConditionedFulfil.ToString() + @"</Data></Cell>
                                    <Cell ss:StyleID=""N""><Data ss:Type=""Number"">" + reportBlock.SoldiersConditionedPostpone.ToString() + @"</Data></Cell>
                                    <Cell ss:StyleID=""N""><Data ss:Type=""Number"">" + reportBlock.SoldiersConditionedFulfil.ToString() + @"</Data></Cell>
                               </Row>");

                    counter++;
                }

                string totalRowAbsolutelyFormula = "";
                string totalRowConditionedFormula = "";

                for (int i = reportBlocks.Count; i > 0; i--)
                {
                    totalRowAbsolutelyFormula += (String.IsNullOrEmpty(totalRowAbsolutelyFormula) ? "" : "+") +
                        "R[-" + ((int)(i * 3)).ToString() + "]C";

                    totalRowConditionedFormula += (String.IsNullOrEmpty(totalRowConditionedFormula) ? "" : "+") +
                        "R[-" + ((int)(i * 3)).ToString() + "]C";
                }

                sb.Append(@"<Row>
                                <Cell ss:MergeAcross=""11"" ss:StyleID=""DTBC""><Data ss:Type=""String"">Всичко</Data></Cell>
                            </Row>
                            <Row>
                                <Cell ss:StyleID=""DT"" />
                                <Cell ss:StyleID=""DTBI""><Data ss:Type=""String"">- безусловно отсрочени</Data></Cell>
                                <Cell ss:StyleID=""NB"" ss:Formula=""=RC[2]+RC[4]+RC[6]+RC[8]""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NB"" ss:Formula=""=RC[2]+RC[4]+RC[6]+RC[8]""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NB"" ss:Formula=""" + totalRowAbsolutelyFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NB"" ss:Formula=""" + totalRowAbsolutelyFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NB"" ss:Formula=""" + totalRowAbsolutelyFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NB"" ss:Formula=""" + totalRowAbsolutelyFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NB"" ss:Formula=""" + totalRowAbsolutelyFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NB"" ss:Formula=""" + totalRowAbsolutelyFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NB"" ss:Formula=""" + totalRowAbsolutelyFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NB"" ss:Formula=""" + totalRowAbsolutelyFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                           </Row>
                           <Row>
                                <Cell ss:StyleID=""DT"" />
                                <Cell ss:StyleID=""DTBI""><Data ss:Type=""String"">- условно отсрочени</Data></Cell>
                                <Cell ss:StyleID=""NB"" ss:Formula=""=RC[2]+RC[4]+RC[6]+RC[8]""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NB"" ss:Formula=""=RC[2]+RC[4]+RC[6]+RC[8]""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NB"" ss:Formula=""" + totalRowConditionedFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NB"" ss:Formula=""" + totalRowConditionedFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NB"" ss:Formula=""" + totalRowConditionedFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NB"" ss:Formula=""" + totalRowConditionedFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NB"" ss:Formula=""" + totalRowConditionedFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NB"" ss:Formula=""" + totalRowConditionedFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NB"" ss:Formula=""" + totalRowConditionedFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NB"" ss:Formula=""" + totalRowConditionedFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                           </Row>");
            }


            sb.Append(@"        </Table>
                                <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">
                                    <DoNotDisplayGridlines/>
                                </WorksheetOptions>
                            </Worksheet>
                        </Workbook>");
            return sb.ToString();
        }

        //Refresh the data grid
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            pnlSearchHint.Visible = false;

            Session[sessionResultsKey] = null;
            reportResult = null;

            if (true)
            {
                RefreshReport();
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
            hdnMilitaryDepartmentSelectedText.Value = "";
            ddPostponeYears.SelectedValue = DateTime.Now.Year.ToString();
            ddRegion.SelectedValue = ListItems.GetOptionAll().Value;

            ddRegion_Changed(sender, e);

            pnlReportGrid.InnerHtml = "";
            pnlSearchHint.Visible = true;

            Session[sessionResultsKey] = null;
            reportResult = null;
        }

        protected void btnExcel_Click(object sender, EventArgs e)
        {
            string result = GenerateExcelOutput();
            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment; filename=ReportPostponeResByAdm.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        private ReportPostponeResByAdministrationFilter CollectFilterData()
        {
            ReportPostponeResByAdministrationFilter filter = new ReportPostponeResByAdministrationFilter();

            string region = "";
            if (ddRegion.SelectedValue != ListItems.GetOptionAll().Value)
            {
                region = ddRegion.SelectedValue;
            }

            string municiplaity = "";
            if (ddMuniciplaity.SelectedValue != ListItems.GetOptionAll().Value)
            {
                municiplaity = ddMuniciplaity.SelectedValue;
            }

            string city = "";
            if (ddCity.SelectedValue != ListItems.GetOptionAll().Value)
            {
                city = ddCity.SelectedValue;
            }

            filter.MilitaryDepartmentIds = hdnMilitaryDepartmentSelected.Value;
            filter.PostponeYear = ddPostponeYears.SelectedValue;
            filter.Region = region;
            filter.Municipality = municiplaity;
            filter.City = city;

            filter.MilitaryDepartmentDisplayText = hdnMilitaryDepartmentSelectedText.Value;
            filter.PostponeYearDisplayText = Server.HtmlEncode(ddPostponeYears.SelectedItem != null ? ddPostponeYears.SelectedItem.Text : ListItems.GetOptionAll().Text);
            filter.RegionDisplayText = Server.HtmlEncode(ddRegion.SelectedItem != null ? ddRegion.SelectedItem.Text : ListItems.GetOptionAll().Text);
            filter.MunicipalityDisplayText = Server.HtmlEncode(ddMuniciplaity.SelectedItem != null ? ddMuniciplaity.SelectedItem.Text : ListItems.GetOptionAll().Text);
            filter.CityDisplayText = Server.HtmlEncode(ddCity.SelectedItem != null ? ddCity.SelectedItem.Text : ListItems.GetOptionAll().Text);


            return filter;
        }
    }
}
