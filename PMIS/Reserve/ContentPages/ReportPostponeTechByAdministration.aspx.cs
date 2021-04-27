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
    public partial class ReportPostponeTechByAdministration : RESPage
    {
        private string sessionResultsKey = "ReportPostponeTechByAdministrationResult";

        private ReportPostponeTechByAdministrationResult reportResult = null;
        private ReportPostponeTechByAdministrationResult ReportResult
        {
            get
            {
                if (reportResult == null)
                {
                    if (Session[sessionResultsKey] != null)
                    {
                        reportResult = (ReportPostponeTechByAdministrationResult)Session[sessionResultsKey];
                    }
                    else
                    {
                        ReportPostponeTechByAdministrationFilter filter = CollectFilterData();
                        reportResult = ReportPostponeTechByAdministrationUtil.GetReportPostponeTechByAdministration(filter, CurrentUser);
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
                return "RES_POSTPONE_REPORT_TECH_BY_ADMINISTRATION";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.GetUIItemAccessLevel("RES_POSTPONE_REPORT_TECH_BY_ADMINISTRATION") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }         
           
            //Hilight the current page in the menu bar
            HighlightMenuItems("Postpone", "ReportPostponeTechByAdministration");

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
            //Nothing here, but it is ready to do something (e.g. pre-fill some filters
        }

        private void PopulatePostponeYears()
        {
            List<int> currYear = new List<int>();
            currYear.Add(DateTime.Now.Year);
            List<int> postponeYears = PostponeTechUtil.GetAllPostponeTechYears(CurrentUser);
            List<int> fulfilYears = PostponeTechUtil.GetAllPostponeTechFulfilYears(CurrentUser);

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
            List<ReportPostponeTechByAdministrationBlock> reportBlocks = ReportResult.AllBlocks;

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
                            <col style='width: 600px;'>
                            <col style='width: 40px;'>
                            <col style='width: 40px;'>
                            <col style='width: 40px;'>
                            <col style='width: 40px;'>
                            <col style='width: 40px;'>
                            <col style='width: 40px;'>
                         </colgroup>
                         <thead>
                            <tr>
                               <th rowspan='2'>№ по ред</th>
                               <th rowspan='2'>Тип на техниката</th>
                               <th colspan='2'>Общо техника за отсрочване</th>
                               <th colspan='2'>Техника за безусловно отсрочване</th>
                               <th colspan='2'>Техника за условно отсрочване</th>
                            </tr>
                            <tr>
                               <th>предл.</th>
                               <th>изпълн.</th>
                               <th>предл.</th>
                               <th>изпълн.</th>
                               <th>предл.</th>
                               <th>изпълн.</th>
                            </tr> 
                         </thead>";

                int counter = 1;
                
                int prevAdministrationID = 0;
                string prevAdministrationName = "";

                foreach (ReportPostponeTechByAdministrationBlock reportBlock in reportBlocks)
                {
                    if (prevAdministrationID != reportBlock.AdministrationId)
                    {
                        if(prevAdministrationID > 0)
                        {
                            html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                         <td></td>
                                         <td style='text-align: left; font-weight: bold;'>Всичко " + prevAdministrationName + @"</td>
                                         <td style='text-align: center; font-weight: bold;'>" + reportBlocks.Where(x => x.AdministrationId == prevAdministrationID).Sum(x => x.PostponeAbsolutely + x.PostponeConditioned).ToString() + @"</td>
                                         <td style='text-align: center; font-weight: bold;'>" + reportBlocks.Where(x => x.AdministrationId == prevAdministrationID).Sum(x => x.FulfilAbsolutely + x.FulfilConditioned).ToString() + @"</td>
                                         <td style='text-align: center; font-weight: bold;'>" + reportBlocks.Where(x => x.AdministrationId == prevAdministrationID).Sum(x => x.PostponeAbsolutely).ToString() + @"</td>
                                         <td style='text-align: center; font-weight: bold;'>" + reportBlocks.Where(x => x.AdministrationId == prevAdministrationID).Sum(x => x.FulfilAbsolutely).ToString() + @"</td>
                                         <td style='text-align: center; font-weight: bold;'>" + reportBlocks.Where(x => x.AdministrationId == prevAdministrationID).Sum(x => x.PostponeConditioned).ToString() + @"</td>
                                         <td style='text-align: center; font-weight: bold;'>" + reportBlocks.Where(x => x.AdministrationId == prevAdministrationID).Sum(x => x.FulfilConditioned).ToString() + @"</td>
                                      </tr>";
                        }

                        html += @"<tr class='" + ((counter + 1) % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                     <td colspan='12' style='text-align: center; font-weight: bold; padding-top: 5px; padding-bottom: 5px;'>" + reportBlock.AdministrationName + @"</td>
                                  </tr>";

                        counter = 1;
                        prevAdministrationID = reportBlock.AdministrationId;
                        prevAdministrationName = reportBlock.AdministrationName;
                    }
                    
                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='text-align: center;'>" + CommonFunctions.IntegerToRoman(counter) + @"</td>
                                 <td style='text-align: left;'>" + reportBlock.TechnicsTypeName + @"</td>
                                 <td style='text-align: center;'>" + ((int)(reportBlock.PostponeAbsolutely + reportBlock.PostponeConditioned)).ToString() + @"</td>
                                 <td style='text-align: center;'>" + ((int)(reportBlock.FulfilAbsolutely + reportBlock.FulfilConditioned)).ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportBlock.PostponeAbsolutely.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportBlock.FulfilAbsolutely.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportBlock.PostponeConditioned.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportBlock.FulfilConditioned.ToString() + @"</td>
                              </tr>";

                    counter++;
                }

                if (prevAdministrationID != 0)
                {
                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td></td>
                                 <td style='text-align: left; font-weight: bold;'>Всичко " + prevAdministrationName + @"</td>
                                 <td style='text-align: center; font-weight: bold;'>" + reportBlocks.Where(x => x.AdministrationId == prevAdministrationID).Sum(x => x.PostponeAbsolutely + x.PostponeConditioned).ToString() + @"</td>
                                 <td style='text-align: center; font-weight: bold;'>" + reportBlocks.Where(x => x.AdministrationId == prevAdministrationID).Sum(x => x.FulfilAbsolutely + x.FulfilConditioned).ToString() + @"</td>
                                 <td style='text-align: center; font-weight: bold;'>" + reportBlocks.Where(x => x.AdministrationId == prevAdministrationID).Sum(x => x.PostponeAbsolutely).ToString() + @"</td>
                                 <td style='text-align: center; font-weight: bold;'>" + reportBlocks.Where(x => x.AdministrationId == prevAdministrationID).Sum(x => x.FulfilAbsolutely).ToString() + @"</td>
                                 <td style='text-align: center; font-weight: bold;'>" + reportBlocks.Where(x => x.AdministrationId == prevAdministrationID).Sum(x => x.PostponeConditioned).ToString() + @"</td>
                                 <td style='text-align: center; font-weight: bold;'>" + reportBlocks.Where(x => x.AdministrationId == prevAdministrationID).Sum(x => x.FulfilConditioned).ToString() + @"</td>
                              </tr>";
                }


                html += @"<tr class='" + ((counter + 1) % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                             <td colspan='12' style='text-align: center; font-weight: bold; padding-top: 5px; padding-bottom: 5px;'>Всичко</td>
                          </tr>";

                var technicsTypes = reportBlocks.GroupBy(x => new {x.TechnicsTypeId, x.TechnicsTypeName});
                counter = 1;

                foreach(var technicsType in technicsTypes)
                {
                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='text-align: center;'>" + CommonFunctions.IntegerToRoman(counter) + @"</td>
                                 <td style='text-align: left;'>" + technicsType.Key.TechnicsTypeName + @"</td>
                                 <td style='text-align: center;'>" + technicsType.Sum(x => x.PostponeConditioned + x.PostponeAbsolutely).ToString() + @"</td>
                                 <td style='text-align: center;'>" + technicsType.Sum(x => x.FulfilConditioned + x.FulfilAbsolutely).ToString() + @"</td>
                                 <td style='text-align: center;'>" + technicsType.Sum(x => x.PostponeAbsolutely).ToString() + @"</td>
                                 <td style='text-align: center;'>" + technicsType.Sum(x => x.FulfilAbsolutely).ToString() + @"</td>
                                 <td style='text-align: center;'>" + technicsType.Sum(x => x.PostponeConditioned).ToString() + @"</td>
                                 <td style='text-align: center;'>" + technicsType.Sum(x => x.FulfilConditioned).ToString() + @"</td>
                              </tr>";

                    counter++;
                }

                int totalPostponeAbsolutely = reportBlocks.Sum(x => x.PostponeAbsolutely);
                int totalFulfilAbsolutely = reportBlocks.Sum(x => x.FulfilAbsolutely);
                int totalPostponeConditioned = reportBlocks.Sum(x => x.PostponeConditioned);
                int totalFulfilConditioned = reportBlocks.Sum(x => x.FulfilConditioned);

                html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                             <td style='text-align: center;'></td>
                             <td style='text-align: left; font-weight: bold;'>Всичко</td>
                             <td style='text-align: center; font-weight: bold;'>" + ((int)(totalPostponeAbsolutely + totalPostponeConditioned)).ToString() + @"</td>
                             <td style='text-align: center; font-weight: bold;'>" + ((int)(totalFulfilAbsolutely + totalFulfilConditioned)).ToString() + @"</td>
                             <td style='text-align: center; font-weight: bold;'>" + totalPostponeAbsolutely.ToString() + @"</td>
                             <td style='text-align: center; font-weight: bold;'>" + totalFulfilAbsolutely.ToString() + @"</td>
                             <td style='text-align: center; font-weight: bold;'>" + totalPostponeConditioned.ToString() + @"</td>
                             <td style='text-align: center; font-weight: bold;'>" + totalFulfilConditioned.ToString() + @"</td>
                          </tr>";

                html += "</table>";

                //Put the summary stats above the results grid
                decimal totalAbsolutelyPerc = 0;
                if (totalPostponeAbsolutely != 0)
                    totalAbsolutelyPerc = totalFulfilAbsolutely * 100.0m / totalPostponeAbsolutely;

                decimal totalConditionedPerc = 0;
                if (totalPostponeConditioned != 0)
                    totalConditionedPerc = totalFulfilConditioned * 100.0m / totalPostponeConditioned;

                html = @"<table class='SummaryStatsTable'>
                            <tr>
                               <td></td>
                               <td>Заявено</td>
                               <td>Изпълнено</td>
                               <td>% изпълн.</td>
                            </tr>
                            <tr>
                               <td style='text-align: left;'>Общ брой техника, предлагана за безусловно отсрочване</td>
                               <td style='text-align: right;'>" + totalPostponeAbsolutely.ToString() + @" бр.</td>
                               <td style='text-align: right;'>" + totalFulfilAbsolutely.ToString() + @" бр.</td>
                               <td style='text-align: right;'>" + totalAbsolutelyPerc.ToString("0") + @" %</td>
                            </tr>
                            <tr>
                               <td style='text-align: left;'>Общ брой техника, предлагана за условно отсрочване</td>
                               <td style='text-align: right;'>" + totalPostponeConditioned.ToString() + @" бр.</td>
                               <td style='text-align: right;'>" + totalFulfilConditioned.ToString() + @" бр.</td>
                               <td style='text-align: right;'>" + totalConditionedPerc.ToString("0") + @" %</td>
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
                                    <Alignment ss:Horizontal=""Center"" ss:Vertical=""Center"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS"" ss:Size=""16"" ss:Bold=""1""/>
                                </Style>
                                <Style ss:ID=""HT2"">
                                    <Alignment ss:Horizontal=""Center"" ss:Vertical=""Center"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS"" ss:Size=""20"" ss:Bold=""1""/>
                                </Style>
                                <Style ss:ID=""HT3"">
                                    <Alignment ss:Horizontal=""Center"" ss:Vertical=""Center"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS"" ss:Size=""13"" ss:Bold=""1""/>
                                </Style>
                                <Style ss:ID=""DT"">
                                    <Alignment ss:Vertical=""Center"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS""/>
                                    <Borders>
                                       <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                    </Borders>
                                </Style>
                                <Style ss:ID=""DTC"">
                                    <Alignment ss:Vertical=""Center"" ss:Horizontal=""Center"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS""/>
                                    <Borders>
                                       <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                    </Borders>
                                </Style>
                                <Style ss:ID=""DTB"">
                                    <Alignment ss:Vertical=""Center"" ss:WrapText=""1""/>
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
                                    <Alignment ss:Vertical=""Center"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS"" ss:Bold=""1"" ss:Italic=""1"" />
                                    <Borders>
                                       <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                    </Borders>
                                </Style>
                                <Style ss:ID=""DTI"">
                                    <Alignment ss:Vertical=""Center"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS"" ss:Italic=""1"" />
                                    <Borders>
                                       <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                    </Borders>
                                </Style>
                                <Style ss:ID=""N"">
                                    <Alignment ss:Vertical=""Center"" ss:Horizontal=""Center"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS""/>
                                    <Borders>
                                       <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                    </Borders>
                                </Style>
                                <Style ss:ID=""NB"">
                                    <Alignment ss:Vertical=""Center"" ss:Horizontal=""Center"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS"" ss:Bold=""1"" />
                                    <Borders>
                                       <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                    </Borders>
                                </Style>
                                <Style ss:ID=""P"">
                                    <Alignment ss:Vertical=""Center"" ss:Horizontal=""Center"" ss:WrapText=""1""/>
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
                                    <Alignment ss:Vertical=""Center"" ss:Horizontal=""Left"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS""/>
                                </Style>
                                <Style ss:ID=""FV"">
                                    <Alignment ss:Vertical=""Center"" ss:Horizontal=""Left"" ss:WrapText=""0""/>
                                    <Font ss:FontName=""Arial Unicode MS"" ss:Italic=""1""/>
                                </Style>
                            </Styles>
                            <Worksheet ss:Name=""Отсрочване на техника"">
                                <Table>
                                    <Column ss:Width=""30""/>
                                    <Column ss:Width=""250""/>
                                    <Column ss:Width=""60""/>
                                    <Column ss:Width=""60""/>
                                    <Column ss:Width=""60""/>
                                    <Column ss:Width=""60""/>
                                    <Column ss:Width=""60""/>
                                    <Row ss:AutoFitHeight=""0"" ss:Height=""20.25"">
                                        <Cell ss:MergeAcross=""7"" ss:StyleID=""HT1""><Data ss:Type=""String"">АСУ на човешките ресурси</Data></Cell>
                                    </Row>
                                    <Row ss:AutoFitHeight=""0"" ss:Height=""26.25"">
                                        <Cell ss:MergeAcross=""7"" ss:StyleID=""HT2""><Data ss:Type=""String"">Отчет на ресурсите от резерва</Data></Cell>
                                    </Row>
                                    <Row ss:AutoFitHeight=""0"" ss:Height=""16.5"">
                                        <Cell ss:MergeAcross=""7"" ss:StyleID=""HT3""><Data ss:Type=""String"">Отчет за изпълнение отсрочването на техника</Data></Cell>
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
            List<ReportPostponeTechByAdministrationBlock> reportBlocks = ReportResult.AllBlocks;

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
                int administrationsCount = reportBlocks.GroupBy(x => x.AdministrationId).Count();
                int totalTechnicsTypesCount = reportBlocks.GroupBy(x => x.TechnicsTypeId).Count();

                sb.Append(@"<Row><Cell/></Row>
                            <Row>
                               <Cell ss:MergeAcross=""1"" ss:StyleID=""DT""><Data ss:Type=""String""></Data></Cell>
                               <Cell ss:StyleID=""DTC""><Data ss:Type=""String"">Заявено</Data></Cell>
                               <Cell ss:StyleID=""DTC""><Data ss:Type=""String"">Изпълнено</Data></Cell>
                               <Cell ss:StyleID=""DTC""><Data ss:Type=""String"">% изпълн.</Data></Cell> 
                            </Row>
                            <Row>
                               <Cell ss:MergeAcross=""1"" ss:StyleID=""DT""><Data ss:Type=""String"">Общо запасни, предлагани за безусловно отсрочване</Data></Cell>
                               <Cell ss:StyleID=""N"" ss:Formula=""=R[" + (5 + reportBlocks.Count + 2 * administrationsCount + totalTechnicsTypesCount + 2) + @"]C[2]""><Data ss:Type=""Number""></Data></Cell>
                               <Cell ss:StyleID=""N"" ss:Formula=""=R[" + (5 + reportBlocks.Count + 2 * administrationsCount + totalTechnicsTypesCount + 2) + @"]C[2]""><Data ss:Type=""Number""></Data></Cell>
                               <Cell ss:StyleID=""P"" ss:Formula=""=IF(RC[-2]=0, 0, RC[-1]/RC[-2])""><Data ss:Type=""Number""></Data></Cell> 
                            </Row>
                            <Row>
                               <Cell ss:MergeAcross=""1"" ss:StyleID=""DT""><Data ss:Type=""String"">Общо запасни, предлагани за условно отсрочване</Data></Cell>
                               <Cell ss:StyleID=""N"" ss:Formula=""=R[" + (4 + reportBlocks.Count + 2 * administrationsCount + totalTechnicsTypesCount + 2) + @"]C[4]""><Data ss:Type=""Number""></Data></Cell>
                               <Cell ss:StyleID=""N"" ss:Formula=""=R[" + (4 + reportBlocks.Count + 2 * administrationsCount + totalTechnicsTypesCount + 2) + @"]C[4]""><Data ss:Type=""Number""></Data></Cell>
                               <Cell ss:StyleID=""P"" ss:Formula=""=IF(RC[-2]=0, 0, RC[-1]/RC[-2])""><Data ss:Type=""Number""></Data></Cell> 
                            </Row>
                            <Row><Cell/></Row>
                            <Row ss:Height=""30"">
                               <Cell ss:MergeDown=""1"" ss:StyleID=""DTC""><Data ss:Type=""String"">№ по ред</Data></Cell>
                               <Cell ss:MergeDown=""1"" ss:StyleID=""DTC""><Data ss:Type=""String"">Тип на техниката</Data></Cell>
                               <Cell ss:MergeAcross=""1"" ss:StyleID=""DTC""><Data ss:Type=""String"">Общо техника за отсрочване</Data></Cell>
                               <Cell ss:MergeAcross=""1"" ss:StyleID=""DTC""><Data ss:Type=""String"">Техника за безусловно отсрочване</Data></Cell>
                               <Cell ss:MergeAcross=""1"" ss:StyleID=""DTC""><Data ss:Type=""String"">Техника за условно отсрочване</Data></Cell>
                            </Row>
                            <Row>
                               <Cell ss:Index=""3"" ss:StyleID=""DTC""><Data ss:Type=""String"">предл.</Data></Cell>
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
                            </Row>");

                int counter = 1;
                int prevAdministrationID = 0;
                string prevAdministrationName = "";

                //This collection is used to keep the list of row number where the same technics type appears on the report. It is used for the total section
                Dictionary<int, List<int>> technicsTypesRow = new Dictionary<int, List<int>>();
                foreach (var technicsType in reportBlocks.GroupBy(x => x.TechnicsTypeId))
                    technicsTypesRow.Add(technicsType.Key, new List<int>());

                int rowIdx = 0;

                foreach (ReportPostponeTechByAdministrationBlock reportBlock in reportBlocks)
                {
                    if (prevAdministrationID != reportBlock.AdministrationId)
                    {
                        if(prevAdministrationID > 0)
                        {
                            int itemsCountForPrevAdministration = reportBlocks.Count(x => x.AdministrationId == prevAdministrationID);
                            string administrationRowFormula = "=SUM(R[-1]C:R[-" + itemsCountForPrevAdministration + "]C)";

                            sb.Append(@"<Row>
                                            <Cell ss:StyleID=""NB""><Data ss:Type=""String""></Data></Cell>
                                            <Cell ss:StyleID=""DTB""><Data ss:Type=""String"">Вичко " + prevAdministrationName + @"</Data></Cell>
                                            <Cell ss:StyleID=""NB"" ss:Formula=""=RC[2]+RC[4]""><Data ss:Type=""Number""></Data></Cell>
                                            <Cell ss:StyleID=""NB"" ss:Formula=""=RC[2]+RC[4]""><Data ss:Type=""Number""></Data></Cell>
                                            <Cell ss:StyleID=""NB"" ss:Formula=""" + administrationRowFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                            <Cell ss:StyleID=""NB"" ss:Formula=""" + administrationRowFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                            <Cell ss:StyleID=""NB"" ss:Formula=""" + administrationRowFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                            <Cell ss:StyleID=""NB"" ss:Formula=""" + administrationRowFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                        </Row>");
                            rowIdx++;
                        }

                        sb.Append(@"<Row>
                                        <Cell ss:MergeAcross=""7"" ss:StyleID=""DTBC""><Data ss:Type=""String"">" + reportBlock.AdministrationName + @"</Data></Cell>
                                    </Row>");
                        rowIdx++;

                        counter = 1;
                        prevAdministrationID = reportBlock.AdministrationId;
                        prevAdministrationName = reportBlock.AdministrationName;
                    }

                    sb.Append(@"<Row>
                                    <Cell ss:StyleID=""N""><Data ss:Type=""String"">" + CommonFunctions.IntegerToRoman(counter) + @"</Data></Cell>
                                    <Cell ss:StyleID=""DT""><Data ss:Type=""String"">" + reportBlock.TechnicsTypeName + @"</Data></Cell>
                                    <Cell ss:StyleID=""N"" ss:Formula=""=RC[2]+RC[4]""><Data ss:Type=""Number""></Data></Cell>
                                    <Cell ss:StyleID=""N"" ss:Formula=""=RC[2]+RC[4]""><Data ss:Type=""Number""></Data></Cell>
                                    <Cell ss:StyleID=""N""><Data ss:Type=""Number"">" + reportBlock.PostponeAbsolutely.ToString() + @"</Data></Cell>
                                    <Cell ss:StyleID=""N""><Data ss:Type=""Number"">" + reportBlock.FulfilAbsolutely.ToString() + @"</Data></Cell>
                                    <Cell ss:StyleID=""N""><Data ss:Type=""Number"">" + reportBlock.PostponeConditioned.ToString() + @"</Data></Cell>
                                    <Cell ss:StyleID=""N""><Data ss:Type=""Number"">" + reportBlock.FulfilConditioned.ToString() + @"</Data></Cell>
                               </Row>");
                    rowIdx++;

                    technicsTypesRow[reportBlock.TechnicsTypeId].Add(rowIdx);

                    counter++;
                }

                if (prevAdministrationID != 0)
                {
                    int itemsCountForPrevAdministration = reportBlocks.Count(x => x.AdministrationId == prevAdministrationID);
                    string administrationRowFormula = "=SUM(R[-1]C:R[-" + itemsCountForPrevAdministration + "]C)";

                    sb.Append(@"<Row>
                                    <Cell ss:StyleID=""NB""><Data ss:Type=""String""></Data></Cell>
                                    <Cell ss:StyleID=""DTB""><Data ss:Type=""String"">Вичко " + prevAdministrationName + @"</Data></Cell>
                                    <Cell ss:StyleID=""NB"" ss:Formula=""=RC[2]+RC[4]""><Data ss:Type=""Number""></Data></Cell>
                                    <Cell ss:StyleID=""NB"" ss:Formula=""=RC[2]+RC[4]""><Data ss:Type=""Number""></Data></Cell>
                                    <Cell ss:StyleID=""NB"" ss:Formula=""" + administrationRowFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                    <Cell ss:StyleID=""NB"" ss:Formula=""" + administrationRowFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                    <Cell ss:StyleID=""NB"" ss:Formula=""" + administrationRowFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                    <Cell ss:StyleID=""NB"" ss:Formula=""" + administrationRowFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                </Row>");
                    rowIdx++;
                }

                sb.Append(@"<Row>
                                <Cell ss:MergeAcross=""7"" ss:StyleID=""DTBC""><Data ss:Type=""String"">Всичко</Data></Cell>
                            </Row>");
                rowIdx++;

                var technicsTypes = reportBlocks.GroupBy(x => new { x.TechnicsTypeId, x.TechnicsTypeName });
                counter = 1;

                foreach (var technicsType in technicsTypes)
                {
                    string totalTechnicsTypeFormula = "";
                    foreach (int row in technicsTypesRow[technicsType.Key.TechnicsTypeId])
                    {
                        totalTechnicsTypeFormula += (String.IsNullOrEmpty(totalTechnicsTypeFormula) ? "=" : "+") +
                            "R[-" + ((int)(rowIdx - row + 1)) + "]C";
                    }

                    sb.Append(@"<Row>
                                    <Cell ss:StyleID=""N""><Data ss:Type=""String"">" + CommonFunctions.IntegerToRoman(counter) + @"</Data></Cell>
                                    <Cell ss:StyleID=""DT""><Data ss:Type=""String"">" + technicsType.Key.TechnicsTypeName + @"</Data></Cell>
                                    <Cell ss:StyleID=""N"" ss:Formula=""=RC[2]+RC[4]""><Data ss:Type=""Number""></Data></Cell>
                                    <Cell ss:StyleID=""N"" ss:Formula=""=RC[2]+RC[4]""><Data ss:Type=""Number""></Data></Cell>
                                    <Cell ss:StyleID=""N"" ss:Formula=""" + totalTechnicsTypeFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                    <Cell ss:StyleID=""N"" ss:Formula=""" + totalTechnicsTypeFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                    <Cell ss:StyleID=""N"" ss:Formula=""" + totalTechnicsTypeFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                    <Cell ss:StyleID=""N"" ss:Formula=""" + totalTechnicsTypeFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                               </Row>");
                    rowIdx++;

                    counter++;
                }

                string totalRowFormula = "=SUM(R[-1]C:R[-" + technicsTypes.Count() + "]C)";

                sb.Append(@"<Row>
                                <Cell ss:StyleID=""NB""><Data ss:Type=""String""></Data></Cell>
                                <Cell ss:StyleID=""DTB""><Data ss:Type=""String"">Вичко</Data></Cell>
                                <Cell ss:StyleID=""NB"" ss:Formula=""=RC[2]+RC[4]""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NB"" ss:Formula=""=RC[2]+RC[4]""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NB"" ss:Formula=""" + totalRowFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NB"" ss:Formula=""" + totalRowFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NB"" ss:Formula=""" + totalRowFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NB"" ss:Formula=""" + totalRowFormula + @"""><Data ss:Type=""Number""></Data></Cell>
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
            Response.AppendHeader("content-disposition", "attachment; filename=ReportPostponeTechByAdm.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        private ReportPostponeTechByAdministrationFilter CollectFilterData()
        {
            ReportPostponeTechByAdministrationFilter filter = new ReportPostponeTechByAdministrationFilter();

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
