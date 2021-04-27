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
    public partial class ReportPostpone : RESPage
    {
        private string sessionResultsKey = "ReportPostponeResult";

        private ReportPostponeResult reportResult = null;
        private ReportPostponeResult ReportResult
        {
            get
            {
                if (reportResult == null)
                {
                    if (Session[sessionResultsKey] != null)
                    {
                        reportResult = (ReportPostponeResult)Session[sessionResultsKey];
                    }
                    else
                    {
                        ReportPostponeFilter filter = CollectFilterData();
                        reportResult = ReportPostponeUtil.GetReportPostpone(filter, CurrentUser);
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
                return "RES_POSTPONE_REPORT";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.GetUIItemAccessLevel("RES_POSTPONE_REPORT") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            //Hilight the current page in the menu bar
            HighlightMenuItems("Postpone", "ReportPostpone");

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
            ddAdministrations.Attributes.Add("onchange", "SetPickListsSelection();");
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
            PopulateAdministrations();
            PopulateRegions();
            PopulateCompanies();
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
            PopulateCompanies();
        }

        private void PopulatePostponeYears()
        {
            List<int> currYear = new List<int>();
            currYear.Add(DateTime.Now.Year);
            List<int> postponeResYears = PostponeResUtil.GetAllPostponeResYears(CurrentUser);
            List<int> fulfilResYears = PostponeResUtil.GetAllPostponeResFulfilYears(CurrentUser);
            List<int> postponeTechYears = PostponeTechUtil.GetAllPostponeTechYears(CurrentUser);
            List<int> fulfilTechYears = PostponeTechUtil.GetAllPostponeTechFulfilYears(CurrentUser);

            List<int> years = currYear.Union(postponeResYears).Union(fulfilResYears).
                                       Union(postponeTechYears).Union(fulfilTechYears).OrderByDescending(x => x).ToList();

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
            PopulateCompanies();
        }

        private void PopulateAdministrations()
        {
            ddAdministrations.Items.Clear();
            ddAdministrations.Items.Add(ListItems.GetOptionAll());

            List<Administration> administrations = AdministrationUtil.GetAllAdministrations(CurrentUser);

            foreach (Administration administration in administrations)
            {
                ListItem li = new ListItem();
                li.Text = administration.AdministrationName;
                li.Value = administration.AdministrationId.ToString();

                ddAdministrations.Items.Add(li);
            }
        }

        protected void ddAdministrations_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateCompanies();
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

            PopulateCompanies();
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

            PopulateCompanies();
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
            PopulateCompanies();
        }
        private void PopulateCompanies()
        {
            hdnCompanySelected.Value = "";
            hdnCompanySelectedText.Value = "";
            string result = "";

            ReportPostponeFilter filter = CollectFilterData();
            List<Company> companies = ReportPostponeUtil.GetCompaniesList(CurrentUser, filter);

            foreach (Company company in companies)
            {
                string pickListItem = "{value : '" + company.CompanyId.ToString() + "' , label : '" + company.CompanyName.Replace("'", "\\'") + "'}";
                result += (result == "" ? "" : ",") + pickListItem;
            }

            if (result != "")
                result = "[" + result + "]";

            hdnCompanyJson.Value = result;
        }

        //Setup some styling on the page
        private void SetupStyle()
        {
            lblMilitaryDepartment.Style.Add("vertical-align", "middle");
            lblAdministration.Style.Add("vertical-align", "middle");
        }

        //Refresh the data grid
        private void RefreshReport()
        {
            string html = "";

            //Get the list of postpones according to the specified filters
            List<ReportPostponeResBlock> reportResBlocks = ReportResult.ResBlock;
            List<ReportPostponeTechBlock> reportTechBlocks = ReportResult.TechBlock;

            //No data found
            if (reportResBlocks.Count == 0 && reportTechBlocks.Count == 0)
            {
                html = "<span>Няма намерени резултати</span>";
            }
            //If there is data then generate dynamically the HTML for the data grid
            else
            {
                //RES
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
                               <th rowspan='3'>Клас по НКПД</th>
                               <th colspan='10'>Запасни, заявени за безусловно и условно отсрочване</th>
                            </tr>
                            <tr>
                               <th colspan='2'>Общо</th>
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

                int resCounter = 1;

                int totalEmployeesCnt = 0;

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

                foreach (ReportPostponeResBlock reportResBlock in reportResBlocks)
                {
                    totalEmployeesCnt = reportResBlock.TotalEmployeesCnt;

                    int itemTotalAbsolutelyPostpone =
                        reportResBlock.OfficersAbsolutelyPostpone +
                        reportResBlock.OfCandAbsolutelyPostpone +
                        reportResBlock.SergeantsAbsolutelyPostpone +
                        reportResBlock.SoldiersAbsolutelyPostpone;

                    int itemTotalAbsolutelyFulfil =
                        reportResBlock.OfficersAbsolutelyFulfil +
                        reportResBlock.OfCandAbsolutelyFulfil +
                        reportResBlock.SergeantsAbsolutelyFulfil +
                        reportResBlock.SoldiersAbsolutelyFulfil;

                    int itemTotalConditionedPostpone =
                        reportResBlock.OfficersConditionedPostpone +
                        reportResBlock.OfCandConditionedPostpone +
                        reportResBlock.SergeantsConditionedPostpone +
                        reportResBlock.SoldiersConditionedPostpone;

                    int itemTotalConditionedFulfil =
                        reportResBlock.OfficersConditionedFulfil +
                        reportResBlock.OfCandConditionedFulfil +
                        reportResBlock.SergeantsConditionedFulfil +
                        reportResBlock.SoldiersConditionedFulfil;

                    totalTotalAbsolutelyPostpone += itemTotalAbsolutelyPostpone;
                    totalTotalAbsolutelyFulfil += itemTotalAbsolutelyFulfil;
                    totalTotalConditionedPostpone += itemTotalConditionedPostpone;
                    totalTotalConditionedFulfil += itemTotalConditionedFulfil;
                    totalOfficersAbsolutelyPostpone += reportResBlock.OfficersAbsolutelyPostpone;
                    totalOfficersAbsolutelyFulfil += reportResBlock.OfficersAbsolutelyFulfil;
                    totalOfficersConditionedPostpone += reportResBlock.OfficersConditionedPostpone;
                    totalOfficersConditionedFulfil += reportResBlock.OfficersConditionedFulfil;
                    totalOfCandAbsolutelyPostpone += reportResBlock.OfCandAbsolutelyPostpone;
                    totalOfCandAbsolutelyFulfil += reportResBlock.OfCandAbsolutelyFulfil;
                    totalOfCandConditionedPostpone += reportResBlock.OfCandConditionedPostpone;
                    totalOfCandConditionedFulfil += reportResBlock.OfCandConditionedFulfil;
                    totalSergeantsAbsolutelyPostpone += reportResBlock.SergeantsAbsolutelyPostpone;
                    totalSergeantsAbsolutelyFulfil += reportResBlock.SergeantsAbsolutelyFulfil;
                    totalSergeantsConditionedPostpone += reportResBlock.SergeantsConditionedPostpone;
                    totalSergeantsConditionedFulfil += reportResBlock.SergeantsConditionedFulfil;
                    totalSoldiersAbsolutelyPostpone += reportResBlock.SoldiersAbsolutelyPostpone;
                    totalSoldiersAbsolutelyFulfil += reportResBlock.SoldiersAbsolutelyFulfil;
                    totalSoldiersConditionedPostpone += reportResBlock.SoldiersConditionedPostpone;
                    totalSoldiersConditionedFulfil += reportResBlock.SoldiersConditionedFulfil;

                    html += @"<tr class='" + (resCounter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='text-align: center;'>" + resCounter.ToString() + @"</td>
                                 <td style='text-align: left; font-weight: bold; font-style: italic;'>" + reportResBlock.NKPDNickname + @":</td>
                                 <td colspan='10'></td>
                              </tr>
                              <tr class='" + (resCounter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td></td>
                                 <td style='text-align: left; font-style: italic;'>- безусловно отсрочени</td>
                                 <td style='text-align: center;'>" + itemTotalAbsolutelyPostpone.ToString() + @"</td>
                                 <td style='text-align: center;'>" + itemTotalAbsolutelyFulfil.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportResBlock.OfficersAbsolutelyPostpone.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportResBlock.OfficersAbsolutelyFulfil.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportResBlock.OfCandAbsolutelyPostpone.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportResBlock.OfCandAbsolutelyFulfil.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportResBlock.SergeantsAbsolutelyPostpone.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportResBlock.SergeantsAbsolutelyFulfil.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportResBlock.SoldiersAbsolutelyPostpone.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportResBlock.SoldiersAbsolutelyFulfil.ToString() + @"</td>
                              </tr>
                              <tr class='" + (resCounter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td></td>
                                 <td style='text-align: left; font-style: italic;'>- условно отсрочени</td>
                                 <td style='text-align: center;'>" + itemTotalConditionedPostpone.ToString() + @"</td>
                                 <td style='text-align: center;'>" + itemTotalConditionedFulfil.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportResBlock.OfficersConditionedPostpone.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportResBlock.OfficersConditionedFulfil.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportResBlock.OfCandConditionedPostpone.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportResBlock.OfCandConditionedFulfil.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportResBlock.SergeantsConditionedPostpone.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportResBlock.SergeantsConditionedFulfil.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportResBlock.SoldiersConditionedPostpone.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportResBlock.SoldiersConditionedFulfil.ToString() + @"</td>
                              </tr>";

                    resCounter++;
                }

                html += @"<tr class='" + (resCounter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='text-align: center;'></td>
                                 <td style='text-align: left; font-weight: bold;'>Всичко</td>
                                 <td colspan='10'></td>
                              </tr>
                              <tr class='" + (resCounter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
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
                              <tr class='" + (resCounter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
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

                html += "</table><br/>";

                //Put the summary stats above the results grid
                decimal totalTotalAbsolutelyPerc = 0;
                if (totalTotalAbsolutelyPostpone != 0)
                    totalTotalAbsolutelyPerc = totalTotalAbsolutelyFulfil * 100.0m / totalTotalAbsolutelyPostpone;

                decimal totalTotalConditionedPerc = 0;
                if (totalTotalConditionedPostpone != 0)
                    totalTotalConditionedPerc = totalTotalConditionedFulfil * 100.0m / totalTotalConditionedPostpone;

                //TECH
                //Setup the header of the grid
                html += @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <colgroup>
                            <col style='width: 40px;'>
                            <col style='width: 600px;'>
                            <col style='width: 80px;'>
                            <col style='width: 40px;'>
                            <col style='width: 40px;'>
                            <col style='width: 40px;'>
                            <col style='width: 40px;'>
                         </colgroup>
                         <thead>
                            <tr>
                               <th rowspan='2'>№ по ред</th>
                               <th rowspan='2'>Тип на техниката</th>
                               <th rowspan='2'>Брой техника, водеща се на военен отчет - всичко</th>
                               <th colspan='2'>Техника за безусловно отсрочване</th>
                               <th colspan='2'>Техника за условно отсрочване</th>
                            </tr>
                            <tr>
                               <th>предл.</th>
                               <th>изпълн.</th>
                               <th>предл.</th>
                               <th>изпълн.</th>
                            </tr> 
                         </thead>";

                int techCounter = 1;
                int technicsTypeCounter = 1;

                int totalMilitaryReportTotal = 0;
                int totalPostponeAbsolutely = 0;
                int totalFulfilAbsolutely = 0;
                int totalPostponeConditioned = 0;
                int totalFulfilConditioned = 0;

                int prevTechnicsTypeID = 0;

                foreach (ReportPostponeTechBlock reportTechBlock in reportTechBlocks)
                {
                    if (prevTechnicsTypeID != reportTechBlock.TechnicsTypeId)
                    {
                        html += @"<tr class='" + (techCounter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                     <td style='text-align: center; font-weight: bold;'>" + CommonFunctions.IntegerToRoman(technicsTypeCounter) + @"</td>
                                     <td style='text-align: left; font-weight: bold;'>" + reportTechBlock.TechnicsTypeName + @"</td>
                                     <td style='text-align: center; font-weight: bold;'>" + reportTechBlocks.Where(x => x.TechnicsTypeId == reportTechBlock.TechnicsTypeId).Sum(x => x.MilitaryReportTotal).ToString() + @"</td>
                                     <td style='text-align: center; font-weight: bold;'>" + reportTechBlocks.Where(x => x.TechnicsTypeId == reportTechBlock.TechnicsTypeId).Sum(x => x.PostponeAbsolutely).ToString() + @"</td>
                                     <td style='text-align: center; font-weight: bold;'>" + reportTechBlocks.Where(x => x.TechnicsTypeId == reportTechBlock.TechnicsTypeId).Sum(x => x.FulfilAbsolutely).ToString() + @"</td>
                                     <td style='text-align: center; font-weight: bold;'>" + reportTechBlocks.Where(x => x.TechnicsTypeId == reportTechBlock.TechnicsTypeId).Sum(x => x.PostponeConditioned).ToString() + @"</td>
                                     <td style='text-align: center; font-weight: bold;'>" + reportTechBlocks.Where(x => x.TechnicsTypeId == reportTechBlock.TechnicsTypeId).Sum(x => x.FulfilConditioned).ToString() + @"</td>
                                  </tr>";

                        technicsTypeCounter++;
                        techCounter = 1;
                        prevTechnicsTypeID = reportTechBlock.TechnicsTypeId;
                    }

                    totalMilitaryReportTotal += reportTechBlock.MilitaryReportTotal;
                    totalPostponeAbsolutely += reportTechBlock.PostponeAbsolutely;
                    totalFulfilAbsolutely += reportTechBlock.FulfilAbsolutely;
                    totalPostponeConditioned += reportTechBlock.PostponeConditioned;
                    totalFulfilConditioned += reportTechBlock.FulfilConditioned;

                    html += @"<tr class='" + (techCounter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='text-align: center;'>" + techCounter.ToString() + @"</td>
                                 <td style='text-align: left;'>" + reportTechBlock.TechnicsSubTypeName + @"</td>
                                 <td style='text-align: center;'>" + reportTechBlock.MilitaryReportTotal.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportTechBlock.PostponeAbsolutely.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportTechBlock.FulfilAbsolutely.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportTechBlock.PostponeConditioned.ToString() + @"</td>
                                 <td style='text-align: center;'>" + reportTechBlock.FulfilConditioned.ToString() + @"</td>
                              </tr>";

                    techCounter++;
                }

                html += @"<tr class='" + (techCounter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='text-align: center;'></td>
                                 <td style='text-align: left; font-weight: bold;'>Всичко</td>
                                 <td style='text-align: center; font-weight: bold;'>" + totalMilitaryReportTotal.ToString() + @"</td>
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
                               <td style='text-align: left;'>Обща численост на персонала " + totalEmployeesCnt.ToString() + @" души </td>
                               <td></td>
                               <td></td>
                               <td></td>
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
                                <Style ss:ID=""HT1res"">
                                    <Alignment ss:Horizontal=""Center"" ss:Vertical=""Bottom"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS"" ss:Size=""16"" ss:Bold=""1""/>
                                </Style>
                                <Style ss:ID=""HT2res"">
                                    <Alignment ss:Horizontal=""Center"" ss:Vertical=""Bottom"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS"" ss:Size=""20"" ss:Bold=""1""/>
                                </Style>
                                <Style ss:ID=""HT3res"">
                                    <Alignment ss:Horizontal=""Center"" ss:Vertical=""Bottom"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS"" ss:Size=""13"" ss:Bold=""1""/>
                                </Style>
                                <Style ss:ID=""HT4res"">
                                    <Alignment ss:Horizontal=""Left"" ss:Vertical=""Bottom"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS"" ss:Size=""13"" ss:Bold=""1""/>
                                </Style>
                                <Style ss:ID=""DTres"">
                                    <Alignment ss:Vertical=""Bottom"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS""/>
                                    <Borders>
                                       <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                    </Borders>
                                </Style>
                                <Style ss:ID=""DTCres"">
                                    <Alignment ss:Vertical=""Bottom"" ss:Horizontal=""Center"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS""/>
                                    <Borders>
                                       <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                    </Borders>
                                </Style>
                                <Style ss:ID=""DTBres"">
                                    <Alignment ss:Vertical=""Bottom"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS"" ss:Bold=""1"" />
                                    <Borders>
                                       <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                    </Borders>
                                </Style>
                                <Style ss:ID=""DTBIres"">
                                    <Alignment ss:Vertical=""Bottom"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS"" ss:Bold=""1"" ss:Italic=""1"" />
                                    <Borders>
                                       <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                    </Borders>
                                </Style>
                                <Style ss:ID=""DTIres"">
                                    <Alignment ss:Vertical=""Bottom"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS"" ss:Italic=""1"" />
                                    <Borders>
                                       <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                    </Borders>
                                </Style>
                                <Style ss:ID=""Nres"">
                                    <Alignment ss:Vertical=""Bottom"" ss:Horizontal=""Center"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS""/>
                                    <Borders>
                                       <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                    </Borders>
                                </Style>
                                <Style ss:ID=""NBres"">
                                    <Alignment ss:Vertical=""Bottom"" ss:Horizontal=""Center"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS"" ss:Bold=""1"" />
                                    <Borders>
                                       <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                    </Borders>
                                </Style>
                                <Style ss:ID=""Pres"">
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
                                <Style ss:ID=""FLres"">
                                    <Alignment ss:Vertical=""Bottom"" ss:Horizontal=""Left"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS""/>
                                </Style>
                                <Style ss:ID=""FVres"">
                                    <Alignment ss:Vertical=""Bottom"" ss:Horizontal=""Left"" ss:WrapText=""0""/>
                                    <Font ss:FontName=""Arial Unicode MS"" ss:Italic=""1""/>
                                </Style>
                                <Style ss:ID=""HT1tech"">
                                    <Alignment ss:Horizontal=""Center"" ss:Vertical=""Center"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS"" ss:Size=""16"" ss:Bold=""1""/>
                                </Style>
                                <Style ss:ID=""HT2tech"">
                                    <Alignment ss:Horizontal=""Center"" ss:Vertical=""Center"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS"" ss:Size=""20"" ss:Bold=""1""/>
                                </Style>
                                <Style ss:ID=""HT3tech"">
                                    <Alignment ss:Horizontal=""Center"" ss:Vertical=""Center"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS"" ss:Size=""13"" ss:Bold=""1""/>
                                </Style>
                                <Style ss:ID=""DTtech"">
                                    <Alignment ss:Vertical=""Center"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS""/>
                                    <Borders>
                                       <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                    </Borders>
                                </Style>
                                <Style ss:ID=""DTCtech"">
                                    <Alignment ss:Vertical=""Center"" ss:Horizontal=""Center"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS""/>
                                    <Borders>
                                       <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                    </Borders>
                                </Style>
                                <Style ss:ID=""DTBtech"">
                                    <Alignment ss:Vertical=""Center"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS"" ss:Bold=""1"" />
                                    <Borders>
                                       <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                    </Borders>
                                </Style>
                                <Style ss:ID=""DTBItech"">
                                    <Alignment ss:Vertical=""Center"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS"" ss:Bold=""1"" ss:Italic=""1"" />
                                    <Borders>
                                       <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                    </Borders>
                                </Style>
                                <Style ss:ID=""DTItech"">
                                    <Alignment ss:Vertical=""Center"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS"" ss:Italic=""1"" />
                                    <Borders>
                                       <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                    </Borders>
                                </Style>
                                <Style ss:ID=""Ntech"">
                                    <Alignment ss:Vertical=""Center"" ss:Horizontal=""Center"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS""/>
                                    <Borders>
                                       <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                    </Borders>
                                </Style>
                                <Style ss:ID=""NBtech"">
                                    <Alignment ss:Vertical=""Center"" ss:Horizontal=""Center"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS"" ss:Bold=""1"" />
                                    <Borders>
                                       <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                       <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1"" ss:Color=""#666666""/>
                                    </Borders>
                                </Style>
                                <Style ss:ID=""Ptech"">
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
                                <Style ss:ID=""FLtech"">
                                    <Alignment ss:Vertical=""Center"" ss:Horizontal=""Left"" ss:WrapText=""1""/>
                                    <Font ss:FontName=""Arial Unicode MS""/>
                                </Style>
                                <Style ss:ID=""FVtech"">
                                    <Alignment ss:Vertical=""Center"" ss:Horizontal=""Left"" ss:WrapText=""0""/>
                                    <Font ss:FontName=""Arial Unicode MS"" ss:Italic=""1""/>
                                </Style>
                            </Styles>
                            <Worksheet ss:Name=""Отсрочване на запасни и техника"">
                                <Table>
                                    <Column ss:Width=""30""/>
                                    <Column ss:Width=""130""/>
                                    <Column ss:Width=""42""/>
                                    <Column ss:Width=""42""/>
                                    <Column ss:Width=""42""/>
                                    <Column ss:Width=""42""/>
                                    <Column ss:Width=""42""/>
                                    <Column ss:Width=""54""/>
                                    <Column ss:Width=""42""/>
                                    <Column ss:Width=""42""/>
                                    <Column ss:Width=""42""/>
                                    <Column ss:Width=""42""/>
                                    <Row ss:AutoFitHeight=""0"" ss:Height=""20.25"">
                                        <Cell ss:MergeAcross=""11"" ss:StyleID=""HT1res""><Data ss:Type=""String"">АСУ на човешките ресурси</Data></Cell>
                                    </Row>
                                    <Row ss:AutoFitHeight=""0"" ss:Height=""26.25"">
                                        <Cell ss:MergeAcross=""11"" ss:StyleID=""HT2res""><Data ss:Type=""String"">Отчет на ресурсите от резерва</Data></Cell>
                                    </Row>
                                    <Row ss:AutoFitHeight=""0"" ss:Height=""16.5"">
                                        <Cell ss:MergeAcross=""11"" ss:StyleID=""HT3res""><Data ss:Type=""String"">Протокол за изпълнение на отсрочването</Data></Cell>
                                    </Row>
                                    <Row>
                                        <Cell/>
                                    </Row>
                                    <Row>
                                        <Cell ss:MergeAcross=""1"" ss:StyleID=""FLres""><Data ss:Type=""String"">Военно окръжие:</Data></Cell>
                                        <Cell ss:StyleID=""FVres""><Data ss:Type=""String"">" + ReportResult.Filter.MilitaryDepartmentDisplayText + @"</Data></Cell>
                                    </Row>
                                    <Row>
                                        <Cell ss:MergeAcross=""1"" ss:StyleID=""FLres""><Data ss:Type=""String"">Година:</Data></Cell>
                                        <Cell ss:StyleID=""FVres""><Data ss:Type=""Number"">" + ReportResult.Filter.PostponeYearDisplayText + @"</Data></Cell>
                                    </Row>
                                    <Row>
                                        <Cell ss:MergeAcross=""1"" ss:StyleID=""FLres""><Data ss:Type=""String"">Министерство:</Data></Cell>
                                        <Cell ss:StyleID=""FVres""><Data ss:Type=""String"">" + ReportResult.Filter.AdministrationDisplayText + @"</Data></Cell>
                                    </Row>
                                    <Row>
                                        <Cell ss:MergeAcross=""1"" ss:StyleID=""FLres""><Data ss:Type=""String"">Област:</Data></Cell>
                                        <Cell ss:StyleID=""FVres""><Data ss:Type=""String"">" + ReportResult.Filter.RegionDisplayText + @"</Data></Cell>
                                    </Row>
                                    <Row>
                                        <Cell ss:MergeAcross=""1"" ss:StyleID=""FLres""><Data ss:Type=""String"">Община:</Data></Cell>
                                        <Cell ss:StyleID=""FVres""><Data ss:Type=""String"">" + ReportResult.Filter.MunicipalityDisplayText + @"</Data></Cell>
                                    </Row>
                                    <Row>
                                        <Cell ss:MergeAcross=""1"" ss:StyleID=""FLres""><Data ss:Type=""String"">Населено място:</Data></Cell>
                                        <Cell ss:StyleID=""FVres""><Data ss:Type=""String"">" + ReportResult.Filter.CityDisplayText + @"</Data></Cell>
                                    </Row>
                                    <Row>
                                        <Cell ss:MergeAcross=""1"" ss:StyleID=""FLres""><Data ss:Type=""String"">Фирма:</Data></Cell>
                                        <Cell ss:StyleID=""FVres""><Data ss:Type=""String"">" + ReportResult.Filter.CompanyDisplayText + @"</Data></Cell>
                                    </Row>
                                    
                           ");

            //Get the list of postpones according to the specified filters
            List<ReportPostponeResBlock> reportResBlocks = ReportResult.ResBlock;
            List<ReportPostponeTechBlock> reportTechBlocks = ReportResult.TechBlock;

            //No data found
            if (reportResBlocks.Count == 0 && reportTechBlocks.Count == 0)
            {
                sb.Append(@"<Row><Cell/></Row>
                            <Row>
                                <Cell><Data ss:Type=""String"">Няма намерени резултати</Data></Cell>
                            </Row>");
            }
            else
            {
                int technicsTypesCount = reportTechBlocks.GroupBy(x => x.TechnicsTypeId).Count();

                sb.Append(@"<Row><Cell/></Row>
                            <Row>
                               <Cell ss:MergeAcross=""5"" ss:StyleID=""DTres""><Data ss:Type=""String""></Data></Cell>
                               <Cell ss:StyleID=""DTCres""><Data ss:Type=""String"">Заявено</Data></Cell>
                               <Cell ss:StyleID=""DTCres""><Data ss:Type=""String"">Изпълнено</Data></Cell>
                               <Cell ss:StyleID=""DTCres""><Data ss:Type=""String"">% изпълн.</Data></Cell> 
                            </Row>
                            <Row>
                               <Cell ss:MergeAcross=""5"" ss:StyleID=""DTres""><Data ss:Type=""String"">Обща численост на персонала " + reportResBlocks[0].TotalEmployeesCnt.ToString() + @" души</Data></Cell>
                               <Cell ss:StyleID=""DTres""/>
                               <Cell ss:StyleID=""DTres""/>
                               <Cell ss:StyleID=""DTres""/>
                            </Row>
                            <Row>
                               <Cell ss:MergeAcross=""5"" ss:StyleID=""DTres""><Data ss:Type=""String"">Общо запасни, предлагани за безусловно отсрочване</Data></Cell>
                               <Cell ss:StyleID=""Nres"" ss:Formula=""=R[" + (11 + 3 * reportResBlocks.Count) + @"]C[-4]""><Data ss:Type=""Number""></Data></Cell>
                               <Cell ss:StyleID=""Nres"" ss:Formula=""=R[" + (11 + 3 * reportResBlocks.Count) + @"]C[-4]""><Data ss:Type=""Number""></Data></Cell>
                               <Cell ss:StyleID=""Pres"" ss:Formula=""=IF(RC[-2]=0, 0, RC[-1]/RC[-2])""><Data ss:Type=""Number""></Data></Cell> 
                            </Row>
                            <Row>
                               <Cell ss:MergeAcross=""5"" ss:StyleID=""DTres""><Data ss:Type=""String"">Общо запасни, предлагани за условно отсрочване</Data></Cell>
                               <Cell ss:StyleID=""Nres"" ss:Formula=""=R[" + (11 + 3 * reportResBlocks.Count) + @"]C[-4]""><Data ss:Type=""Number""></Data></Cell>
                               <Cell ss:StyleID=""Nres"" ss:Formula=""=R[" + (11 + 3 * reportResBlocks.Count) + @"]C[-4]""><Data ss:Type=""Number""></Data></Cell>
                               <Cell ss:StyleID=""Pres"" ss:Formula=""=IF(RC[-2]=0, 0, RC[-1]/RC[-2])""><Data ss:Type=""Number""></Data></Cell> 
                            </Row>
                            <Row>
                               <Cell ss:MergeAcross=""5"" ss:StyleID=""DTtech""><Data ss:Type=""String"">Общ брой техника, предлагана за безусловно отсрочване</Data></Cell>
                               <Cell ss:StyleID=""Ntech"" ss:Formula=""=R[" + (43 + reportTechBlocks.Count + technicsTypesCount) + @"]C[2]""><Data ss:Type=""Number""></Data></Cell>
                               <Cell ss:StyleID=""Ntech"" ss:Formula=""=R[" + (43 + reportTechBlocks.Count + technicsTypesCount) + @"]C[2]""><Data ss:Type=""Number""></Data></Cell>
                               <Cell ss:StyleID=""Ptech"" ss:Formula=""=IF(RC[-2]=0, 0, RC[-1]/RC[-2])""><Data ss:Type=""Number""></Data></Cell> 
                            </Row>
                            <Row>
                               <Cell ss:MergeAcross=""5"" ss:StyleID=""DTtech""><Data ss:Type=""String"">Общ брой техника, предлагана за условно отсрочване</Data></Cell>
                               <Cell ss:StyleID=""Ntech"" ss:Formula=""=R[" + (42 + reportTechBlocks.Count + technicsTypesCount) + @"]C[4]""><Data ss:Type=""Number""></Data></Cell>
                               <Cell ss:StyleID=""Ntech"" ss:Formula=""=R[" + (42 + reportTechBlocks.Count + technicsTypesCount) + @"]C[4]""><Data ss:Type=""Number""></Data></Cell>
                               <Cell ss:StyleID=""Ptech"" ss:Formula=""=IF(RC[-2]=0, 0, RC[-1]/RC[-2])""><Data ss:Type=""Number""></Data></Cell> 
                            </Row>
                            <Row><Cell/></Row>
                            <Row>
                                <Cell ss:MergeAcross=""11"" ss:StyleID=""HT4res""><Data ss:Type=""String"">А.Изпълнение на заявките за безусловно и условно отсрочване на запасни</Data></Cell>                                
                            </Row>
                            <Row>
                               <Cell ss:MergeDown=""2"" ss:StyleID=""DTCres""><Data ss:Type=""String"">№ по ред</Data></Cell>
                               <Cell ss:MergeDown=""2"" ss:StyleID=""DTCres""><Data ss:Type=""String"">Клас по НКПД</Data></Cell>
                               <Cell ss:MergeAcross=""9"" ss:StyleID=""DTCres""><Data ss:Type=""String"">Запасни, заявени за безусловно и условно отсрочване</Data></Cell>
                            </Row>
                            <Row ss:AutoFitHeight=""0"" ss:Height=""29"">
                               <Cell ss:Index=""3"" ss:MergeAcross=""1"" ss:StyleID=""DTCres""><Data ss:Type=""String"">Общо</Data></Cell>
                               <Cell ss:MergeAcross=""1"" ss:StyleID=""DTCres""><Data ss:Type=""String"">Офицери</Data></Cell>
                               <Cell ss:MergeAcross=""1"" ss:StyleID=""DTCres""><Data ss:Type=""String"">Офицерски кандидати</Data></Cell>
                               <Cell ss:MergeAcross=""1"" ss:StyleID=""DTCres""><Data ss:Type=""String"">Сержанти / старшини</Data></Cell>
                               <Cell ss:MergeAcross=""1"" ss:StyleID=""DTCres""><Data ss:Type=""String"">Войници / матроси</Data></Cell>
                            </Row>
                            <Row>
                               <Cell ss:Index=""3"" ss:StyleID=""DTCres""><Data ss:Type=""String"">предл.</Data></Cell>
                               <Cell ss:StyleID=""DTCres""><Data ss:Type=""String"">изпълн.</Data></Cell>
                               <Cell ss:StyleID=""DTCres""><Data ss:Type=""String"">предл.</Data></Cell>
                               <Cell ss:StyleID=""DTCres""><Data ss:Type=""String"">изпълн.</Data></Cell>
                               <Cell ss:StyleID=""DTCres""><Data ss:Type=""String"">предл.</Data></Cell>
                               <Cell ss:StyleID=""DTCres""><Data ss:Type=""String"">изпълн.</Data></Cell>
                               <Cell ss:StyleID=""DTCres""><Data ss:Type=""String"">предл.</Data></Cell>
                               <Cell ss:StyleID=""DTCres""><Data ss:Type=""String"">изпълн.</Data></Cell>
                               <Cell ss:StyleID=""DTCres""><Data ss:Type=""String"">предл.</Data></Cell>
                               <Cell ss:StyleID=""DTCres""><Data ss:Type=""String"">изпълн.</Data></Cell>
                            </Row>
                            <Row>
                               <Cell ss:StyleID=""DTCres""><Data ss:Type=""Number"">1</Data></Cell>
                               <Cell ss:StyleID=""DTCres""><Data ss:Type=""Number"">2</Data></Cell>
                               <Cell ss:StyleID=""DTCres""><Data ss:Type=""Number"">3</Data></Cell>
                               <Cell ss:StyleID=""DTCres""><Data ss:Type=""Number"">4</Data></Cell>
                               <Cell ss:StyleID=""DTCres""><Data ss:Type=""Number"">5</Data></Cell>
                               <Cell ss:StyleID=""DTCres""><Data ss:Type=""Number"">6</Data></Cell>
                               <Cell ss:StyleID=""DTCres""><Data ss:Type=""Number"">7</Data></Cell>
                               <Cell ss:StyleID=""DTCres""><Data ss:Type=""Number"">8</Data></Cell>
                               <Cell ss:StyleID=""DTCres""><Data ss:Type=""Number"">9</Data></Cell>
                               <Cell ss:StyleID=""DTCres""><Data ss:Type=""Number"">10</Data></Cell>
                               <Cell ss:StyleID=""DTCres""><Data ss:Type=""Number"">11</Data></Cell>
                               <Cell ss:StyleID=""DTCres""><Data ss:Type=""Number"">12</Data></Cell>
                            </Row>");

                int resCounter = 1;
                int totalEmployeesCnt = 0;

                foreach (ReportPostponeResBlock reportResBlock in reportResBlocks)
                {
                    totalEmployeesCnt = reportResBlock.TotalEmployeesCnt;

                    sb.Append(@"<Row>
                                    <Cell ss:StyleID=""Nres""><Data ss:Type=""Number"">" + resCounter.ToString() + @"</Data></Cell>
                                    <Cell ss:StyleID=""DTBIres""><Data ss:Type=""String"">" + reportResBlock.NKPDNickname + @"</Data></Cell>
                                    <Cell ss:MergeAcross=""9"" ss:StyleID=""DTres"" />
                                </Row>
                                <Row>
                                    <Cell ss:StyleID=""DTres"" />
                                    <Cell ss:StyleID=""DTIres""><Data ss:Type=""String"">- безусловно отсрочени</Data></Cell>
                                    <Cell ss:StyleID=""Nres"" ss:Formula=""=RC[2]+RC[4]+RC[6]+RC[8]""><Data ss:Type=""Number""></Data></Cell>
                                    <Cell ss:StyleID=""Nres"" ss:Formula=""=RC[2]+RC[4]+RC[6]+RC[8]""><Data ss:Type=""Number""></Data></Cell>
                                    <Cell ss:StyleID=""Nres""><Data ss:Type=""Number"">" + reportResBlock.OfficersAbsolutelyPostpone.ToString() + @"</Data></Cell>
                                    <Cell ss:StyleID=""Nres""><Data ss:Type=""Number"">" + reportResBlock.OfficersAbsolutelyFulfil.ToString() + @"</Data></Cell>
                                    <Cell ss:StyleID=""Nres""><Data ss:Type=""Number"">" + reportResBlock.OfCandAbsolutelyPostpone.ToString() + @"</Data></Cell>
                                    <Cell ss:StyleID=""Nres""><Data ss:Type=""Number"">" + reportResBlock.OfCandAbsolutelyFulfil.ToString() + @"</Data></Cell>
                                    <Cell ss:StyleID=""Nres""><Data ss:Type=""Number"">" + reportResBlock.SergeantsAbsolutelyPostpone.ToString() + @"</Data></Cell>
                                    <Cell ss:StyleID=""Nres""><Data ss:Type=""Number"">" + reportResBlock.SergeantsAbsolutelyFulfil.ToString() + @"</Data></Cell>
                                    <Cell ss:StyleID=""Nres""><Data ss:Type=""Number"">" + reportResBlock.SoldiersAbsolutelyPostpone.ToString() + @"</Data></Cell>
                                    <Cell ss:StyleID=""Nres""><Data ss:Type=""Number"">" + reportResBlock.SoldiersAbsolutelyFulfil.ToString() + @"</Data></Cell>
                               </Row>
                               <Row>
                                    <Cell ss:StyleID=""DTres"" />
                                    <Cell ss:StyleID=""DTIres""><Data ss:Type=""String"">- условно отсрочени</Data></Cell>
                                    <Cell ss:StyleID=""Nres"" ss:Formula=""=RC[2]+RC[4]+RC[6]+RC[8]""><Data ss:Type=""Number""></Data></Cell>
                                    <Cell ss:StyleID=""Nres"" ss:Formula=""=RC[2]+RC[4]+RC[6]+RC[8]""><Data ss:Type=""Number""></Data></Cell>
                                    <Cell ss:StyleID=""Nres""><Data ss:Type=""Number"">" + reportResBlock.OfficersConditionedPostpone.ToString() + @"</Data></Cell>
                                    <Cell ss:StyleID=""Nres""><Data ss:Type=""Number"">" + reportResBlock.OfficersConditionedFulfil.ToString() + @"</Data></Cell>
                                    <Cell ss:StyleID=""Nres""><Data ss:Type=""Number"">" + reportResBlock.OfCandConditionedPostpone.ToString() + @"</Data></Cell>
                                    <Cell ss:StyleID=""Nres""><Data ss:Type=""Number"">" + reportResBlock.OfCandConditionedFulfil.ToString() + @"</Data></Cell>
                                    <Cell ss:StyleID=""Nres""><Data ss:Type=""Number"">" + reportResBlock.SergeantsConditionedPostpone.ToString() + @"</Data></Cell>
                                    <Cell ss:StyleID=""Nres""><Data ss:Type=""Number"">" + reportResBlock.SergeantsConditionedFulfil.ToString() + @"</Data></Cell>
                                    <Cell ss:StyleID=""Nres""><Data ss:Type=""Number"">" + reportResBlock.SoldiersConditionedPostpone.ToString() + @"</Data></Cell>
                                    <Cell ss:StyleID=""Nres""><Data ss:Type=""Number"">" + reportResBlock.SoldiersConditionedFulfil.ToString() + @"</Data></Cell>
                               </Row>");

                    resCounter++;
                }

                string totalRowAbsolutelyFormula = "";
                string totalRowConditionedFormula = "";

                for (int i = reportResBlocks.Count; i > 0; i--)
                {
                    totalRowAbsolutelyFormula += (String.IsNullOrEmpty(totalRowAbsolutelyFormula) ? "" : "+") +
                        "R[-" + ((int)(i * 3)).ToString() + "]C";

                    totalRowConditionedFormula += (String.IsNullOrEmpty(totalRowConditionedFormula) ? "" : "+") +
                        "R[-" + ((int)(i * 3)).ToString() + "]C";
                }

                sb.Append(@"<Row>
                                <Cell ss:StyleID=""DTres""><Data ss:Type=""String""></Data></Cell>
                                <Cell ss:StyleID=""DTBres""><Data ss:Type=""String"">Всичко</Data></Cell>
                                <Cell ss:MergeAcross=""9"" ss:StyleID=""DTres"" />
                            </Row>
                            <Row>
                                <Cell ss:StyleID=""DTres"" />
                                <Cell ss:StyleID=""DTBIres""><Data ss:Type=""String"">- безусловно отсрочени</Data></Cell>
                                <Cell ss:StyleID=""NBres"" ss:Formula=""=RC[2]+RC[4]+RC[6]+RC[8]""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NBres"" ss:Formula=""=RC[2]+RC[4]+RC[6]+RC[8]""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NBres"" ss:Formula=""" + totalRowAbsolutelyFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NBres"" ss:Formula=""" + totalRowAbsolutelyFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NBres"" ss:Formula=""" + totalRowAbsolutelyFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NBres"" ss:Formula=""" + totalRowAbsolutelyFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NBres"" ss:Formula=""" + totalRowAbsolutelyFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NBres"" ss:Formula=""" + totalRowAbsolutelyFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NBres"" ss:Formula=""" + totalRowAbsolutelyFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NBres"" ss:Formula=""" + totalRowAbsolutelyFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                           </Row>
                           <Row>
                                <Cell ss:StyleID=""DTres"" />
                                <Cell ss:StyleID=""DTBIres""><Data ss:Type=""String"">- условно отсрочени</Data></Cell>
                                <Cell ss:StyleID=""NBres"" ss:Formula=""=RC[2]+RC[4]+RC[6]+RC[8]""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NBres"" ss:Formula=""=RC[2]+RC[4]+RC[6]+RC[8]""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NBres"" ss:Formula=""" + totalRowConditionedFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NBres"" ss:Formula=""" + totalRowConditionedFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NBres"" ss:Formula=""" + totalRowConditionedFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NBres"" ss:Formula=""" + totalRowConditionedFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NBres"" ss:Formula=""" + totalRowConditionedFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NBres"" ss:Formula=""" + totalRowConditionedFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NBres"" ss:Formula=""" + totalRowConditionedFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                <Cell ss:StyleID=""NBres"" ss:Formula=""" + totalRowConditionedFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                           </Row>");
            }

            //TECH
            sb.Append(@"<Row><Cell/></Row>
                        <Row>
                            <Cell ss:MergeAcross=""11"" ss:StyleID=""HT4res""><Data ss:Type=""String"">Б.Изпълнение на заявките за безусловно и условно отсрочване на техника-запас</Data></Cell>                                
                        </Row>
                        <Row ss:AutoFitHeight=""0"" ss:Height=""75"">
                           <Cell ss:MergeDown=""1"" ss:StyleID=""DTCtech""><Data ss:Type=""String"">№ по ред</Data></Cell>
                           <Cell ss:MergeAcross=""5"" ss:MergeDown=""1"" ss:StyleID=""DTCtech""><Data ss:Type=""String"">Тип на техниката</Data></Cell>
                           <Cell ss:MergeDown=""1"" ss:StyleID=""DTCtech""><Data ss:Type=""String"">Брой техника, водеща се на военен отчет - всичко</Data></Cell>
                           <Cell ss:MergeAcross=""1"" ss:StyleID=""DTCtech""><Data ss:Type=""String"">Техника за безусловно отсрочване</Data></Cell>
                           <Cell ss:MergeAcross=""1"" ss:StyleID=""DTCtech""><Data ss:Type=""String"">Техника за условно отсрочване</Data></Cell>
                        </Row>
                        <Row>
                           <Cell ss:Index=""9"" ss:StyleID=""DTCtech""><Data ss:Type=""String"">предл.</Data></Cell>
                           <Cell ss:StyleID=""DTCtech""><Data ss:Type=""String"">изпълн.</Data></Cell>
                           <Cell ss:StyleID=""DTCtech""><Data ss:Type=""String"">предл.</Data></Cell>
                           <Cell ss:StyleID=""DTCtech""><Data ss:Type=""String"">изпълн.</Data></Cell>                               
                        </Row>
                        <Row>
                           <Cell ss:StyleID=""DTCtech""><Data ss:Type=""Number"">1</Data></Cell>
                           <Cell ss:MergeAcross=""5"" ss:StyleID=""DTCtech""><Data ss:Type=""Number"">2</Data></Cell>
                           <Cell ss:StyleID=""DTCtech""><Data ss:Type=""Number"">3</Data></Cell>
                           <Cell ss:StyleID=""DTCtech""><Data ss:Type=""Number"">4</Data></Cell>
                           <Cell ss:StyleID=""DTCtech""><Data ss:Type=""Number"">5</Data></Cell>
                           <Cell ss:StyleID=""DTCtech""><Data ss:Type=""Number"">6</Data></Cell>
                           <Cell ss:StyleID=""DTCtech""><Data ss:Type=""Number"">7</Data></Cell>
                        </Row>");

            int techCounter = 1;
            int technicsTypeCounter = 1;
            int prevTechnicsTypeID = 0;

            List<int> totalRowFormulaItems = new List<int>();

            foreach (ReportPostponeTechBlock reportTechBlock in reportTechBlocks)
            {
                if (prevTechnicsTypeID != reportTechBlock.TechnicsTypeId)
                {
                    int groupItemsCount = reportTechBlocks.Count(x => x.TechnicsTypeId == reportTechBlock.TechnicsTypeId);
                    string technicsTypeRowFormula = "=SUM(R[1]C:R[" + groupItemsCount + "]C)";
                    totalRowFormulaItems.Add(groupItemsCount);

                    sb.Append(@"<Row>
                                    <Cell ss:StyleID=""NBtech""><Data ss:Type=""String"">" + CommonFunctions.IntegerToRoman(technicsTypeCounter) + @"</Data></Cell>
                                    <Cell ss:MergeAcross=""5"" ss:StyleID=""DTBtech""><Data ss:Type=""String"">" + reportTechBlock.TechnicsTypeName + @"</Data></Cell>
                                    <Cell ss:StyleID=""NBtech"" ss:Formula=""" + technicsTypeRowFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                    <Cell ss:StyleID=""NBtech"" ss:Formula=""" + technicsTypeRowFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                    <Cell ss:StyleID=""NBtech"" ss:Formula=""" + technicsTypeRowFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                    <Cell ss:StyleID=""NBtech"" ss:Formula=""" + technicsTypeRowFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                    <Cell ss:StyleID=""NBtech"" ss:Formula=""" + technicsTypeRowFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                                </Row>");

                    technicsTypeCounter++;
                    techCounter = 1;
                    prevTechnicsTypeID = reportTechBlock.TechnicsTypeId;
                }

                sb.Append(@"<Row>
                                <Cell ss:StyleID=""Ntech""><Data ss:Type=""Number"">" + techCounter.ToString() + @"</Data></Cell>
                                <Cell ss:MergeAcross=""5"" ss:StyleID=""DTtech""><Data ss:Type=""String"">" + reportTechBlock.TechnicsSubTypeName + @"</Data></Cell>
                                <Cell ss:StyleID=""Ntech""><Data ss:Type=""Number"">" + reportTechBlock.MilitaryReportTotal.ToString() + @"</Data></Cell>
                                <Cell ss:StyleID=""Ntech""><Data ss:Type=""Number"">" + reportTechBlock.PostponeAbsolutely.ToString() + @"</Data></Cell>
                                <Cell ss:StyleID=""Ntech""><Data ss:Type=""Number"">" + reportTechBlock.FulfilAbsolutely.ToString() + @"</Data></Cell>
                                <Cell ss:StyleID=""Ntech""><Data ss:Type=""Number"">" + reportTechBlock.PostponeConditioned.ToString() + @"</Data></Cell>
                                <Cell ss:StyleID=""Ntech""><Data ss:Type=""Number"">" + reportTechBlock.FulfilConditioned.ToString() + @"</Data></Cell>
                           </Row>");

                techCounter++;
            }

            string totalRowFormula = "";
            for(int i = 0; i < totalRowFormulaItems.Count; i++)
            {
                int rowsOffsetFromTotal = totalRowFormulaItems[i] + 1;

                for (int j = i + 1; j < totalRowFormulaItems.Count; j++)
                {
                    rowsOffsetFromTotal += totalRowFormulaItems[j] + 1;
                }

                totalRowFormula += (String.IsNullOrEmpty(totalRowFormula) ? "" : "+") +
                    "R[-" + rowsOffsetFromTotal.ToString() + "]C";
            }

            sb.Append(@"<Row>
                            <Cell ss:StyleID=""DTtech""><Data ss:Type=""String""></Data></Cell>
                            <Cell ss:MergeAcross=""5"" ss:StyleID=""DTBtech""><Data ss:Type=""String"">Всичко</Data></Cell>
                            <Cell ss:StyleID=""NBtech"" ss:Formula=""" + totalRowFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                            <Cell ss:StyleID=""NBtech"" ss:Formula=""" + totalRowFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                            <Cell ss:StyleID=""NBtech"" ss:Formula=""" + totalRowFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                            <Cell ss:StyleID=""NBtech"" ss:Formula=""" + totalRowFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                            <Cell ss:StyleID=""NBtech"" ss:Formula=""" + totalRowFormula + @"""><Data ss:Type=""Number""></Data></Cell>
                       </Row>");

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
            ddAdministrations.SelectedValue = ListItems.GetOptionAll().Value;
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
            Response.AppendHeader("content-disposition", "attachment; filename=ReportPostpone.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        private ReportPostponeFilter CollectFilterData()
        {
            ReportPostponeFilter filter = new ReportPostponeFilter();

            string administration = "";
            if (ddAdministrations.SelectedValue != ListItems.GetOptionAll().Value)
            {
                administration = ddAdministrations.SelectedValue;
            }

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
            filter.AdministrationIds = administration;
            filter.Region = region;
            filter.Municipality = municiplaity;
            filter.City = city;
            filter.CompanyIds = hdnCompanySelected.Value;

            filter.MilitaryDepartmentDisplayText = hdnMilitaryDepartmentSelectedText.Value;
            filter.PostponeYearDisplayText = Server.HtmlEncode(ddPostponeYears.SelectedItem != null ? ddPostponeYears.SelectedItem.Text : ListItems.GetOptionAll().Text);
            filter.AdministrationDisplayText = Server.HtmlEncode(ddAdministrations.SelectedItem != null ? ddAdministrations.SelectedItem.Text : ListItems.GetOptionAll().Text);
            filter.RegionDisplayText = Server.HtmlEncode(ddRegion.SelectedItem != null ? ddRegion.SelectedItem.Text : ListItems.GetOptionAll().Text);
            filter.MunicipalityDisplayText = Server.HtmlEncode(ddMuniciplaity.SelectedItem != null ? ddMuniciplaity.SelectedItem.Text : ListItems.GetOptionAll().Text);
            filter.CityDisplayText = Server.HtmlEncode(ddCity.SelectedItem != null ? ddCity.SelectedItem.Text : ListItems.GetOptionAll().Text);
            filter.CompanyDisplayText = hdnCompanySelectedText.Value;

            return filter;
        }
    }
}
