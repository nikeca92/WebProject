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
    public partial class ReportAnalyzeResFulfilment : RESPage
    {
        //Get the config setting that says how many rows per page should be dispayed in the grid
        private string sessionResultsKey = "ReportAnalyzeResFulfilment";

        private ReportAnalyzeResFulfilmentResult reportResult = null;
        private ReportAnalyzeResFulfilmentResult ReportResult
        {
            get
            {
                if (reportResult == null)
                {
                    if (Session[sessionResultsKey] != null)
                    {
                        reportResult = (ReportAnalyzeResFulfilmentResult)Session[sessionResultsKey];
                    }
                    else
                    {
                        ReportAnalyzeResFulfilmentFilter filter = CollectFilterData();
                        reportResult = ReportAnalyzeResFulfilmentUtil.GetReportAnalyzeResFulfilment(filter, CurrentUser);
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
                return "RES_REPORTS_REPORTANALYZERESFULFILMENT";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.GetUIItemAccessLevel("RES_REPORTS_REPORTANALYZERESFULFILMENT") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }         
           
            //Hilight the current page in the menu bar
            HighlightMenuItems("Reports", "ReportAnalyzeResFulfilment");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            //Setup some basic styles on the screen
            SetupStyle();

            //When the page is loaded for the first time
            if (!IsPostBack)
            {
                Session[sessionResultsKey] = null;

                lblMilitaryUnit.Text = CommonFunctions.GetLabelText("MilitaryUnit") + ":";

                //Populate any drop-downs and list-boxes
                PopulateLists();
            
                //Do not 'Simulate clicking the Refresh button to load the grid initially' to prevent slow loading
                //btnRefresh_Click(btnRefresh, new EventArgs());
            }

            lblMessage.Text = "";
            lblGridMessage.Text = "";

            btnRefresh.Attributes.Add("onclick", "SetPickListsSelection();");
        }             

        //Populate all listboxes on the screen
        private void PopulateLists()
        {
            PopulateMilitaryCategories();
        }

        private void PopulateMilitaryCategories()
        {
            ddMilitaryCategory.Items.Clear();

            ddMilitaryCategory.DataSource = MilitaryCategoryForReportsUtil.GetAllMilitaryCategoriesForReports();
            ddMilitaryCategory.DataTextField = "CategoryName";
            ddMilitaryCategory.DataValueField = "CategoryKey";
            ddMilitaryCategory.DataBind();
        }
        
        //Setup some styling on the page
        private void SetupStyle()
        {
        
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
            if (this.GetUIItemAccessLevel("RES_REPORTS_REPORTANALYZERESFULFILMENT") == UIAccessLevel.Hidden)
                return;

            string percFormat = "0.0";

            string html = "";

            bool anyData = false;

            if (ReportResult.ByCategoryResult != null && ReportResult.ByCategoryResult.Count > 0)
            {
                anyData = true;

                string cellStyle = "vertical-align: top;";

                html += @"<div style='height: 20px;'>&nbsp;</div>
                         <div class='SmallHeaderText' style='text-align: left; width: 100%;'>Разпределението на личния състав от запаса по команди е както следва:</div>
                         <table class='CommonHeaderTable' style='text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 40px;'>№ по ред</th> 
                               <th style='width: 80px;'>№ на команда</th>
                               <th style='width: 270px;'>Военно формирование</th>
                               <th style='width: 80px;'>Офицери</th>
                               <th style='width: 80px;'>Офицерски кандидати</th>
                               <th style='width: 80px;'>Сержанти<br/>/старшини/</th>
                               <th style='width: 80px;'>Войници<br/>/матроси/</th>
                               <th style='width: 80px;'>ВСИЧКО</th>
                            </tr>
                         </thead>";

                int counter = 0;
                foreach (ReportAnalyzeResFulfilmentByCategoryBlock block in ReportResult.ByCategoryResult)
                {
                    counter++;

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>                  
                                 <td style='" + cellStyle + @" text-align: left;'>" + counter.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: left;'>" + block.MilitaryCommand + @"</td>
                                 <td style='" + cellStyle + @" text-align: left;'>" + block.MilitaryUnit + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledOfficers.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledOffCand.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledSergeants.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledSoldiers.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledTotal.ToString() + @"</td>
                              </tr>";
                }

                html += "</table>";
            }

            if (ReportResult.ByMRSResult != null && ReportResult.ByMRSResult.Count > 0)
            {
                anyData = true;

                string cellStyle = "vertical-align: top;";

                html += @"<div style='height: 20px;'>&nbsp;</div>
                         <div class='SmallHeaderText' style='text-align: left; width: 100%;'>Получили мобилизационно назначение по военноотчетни специалности:</div>
                         <table class='CommonHeaderTable' style='text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 40px;'>№ по ред</th>
                               <th style='width: 80px;'>№ на команда</th>
                               <th style='width: 80px;'>По щат</th>
                               <th style='width: 80px;'>По основна ВОС</th>
                               <th style='width: 80px;'>%</th>
                               <th style='width: 80px;'>Заменени</th>
                               <th style='width: 80px;'>%</th>
                            </tr>
                         </thead>";

                int counter = 0;
                foreach (ReportAnalyzeResFulfilmentByMRSBlock block in ReportResult.ByMRSResult)
                {
                    counter++;

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>                  
                                 <td style='" + cellStyle + @" text-align: left;'>" + counter.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: left;'>" + block.MilitaryCommand + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.CountByStaff.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledByMRS.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledByMRSPerc.ToString(percFormat) + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.ChangesCnt.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.ChangesPerc.ToString(percFormat) + @"</td>
                              </tr>";
                }

                html += "</table>";
            }

            if (ReportResult.ByCommandResult != null && ReportResult.ByCommandResult.Count > 0)
            {
                anyData = true;

                string cellStyle = "vertical-align: top;";

                html += @"<div style='height: 20px;'>&nbsp;</div>
                         <div class='SmallHeaderText' style='text-align: left; width: 100%;'>Разпределение на личният състав по команди:</div>
                         <table class='CommonHeaderTable' style='text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 40px;'>№ по ред</th>
                               <th style='width: 80px;'>№ на команда</th>
                               <th style='width: 270px;'>Военно формирование</th>
                               <th style='width: 80px;'>По щат</th>
                               <th style='width: 80px;'>Подадени от ВО</th>
                               <th style='width: 80px;'>% на комплектоване</th>
                            </tr>
                         </thead>";

                int counter = 0;
                foreach (ReportAnalyzeResFulfilmentByCommandBlock block in ReportResult.ByCommandResult)
                {
                    counter++;

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>                  
                                 <td style='" + cellStyle + @" text-align: left;'>" + counter.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: left;'>" + block.MilitaryCommand + @"</td>
                                 <td style='" + cellStyle + @" text-align: left;'>" + block.MilitaryUnit + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.CountByStaff.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledByMilitaryDepartment.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledPerc.ToString(percFormat) + @"</td>
                              </tr>";
                }

                html += "</table>";
            }

            if (ReportResult.ByAgeResult != null && ReportResult.ByAgeResult.Count > 0)
            {
                anyData = true;

                string cellStyle = "vertical-align: top;";

                html += @"<div style='height: 20px;'>&nbsp;</div>
                         <div class='SmallHeaderText' style='text-align: left; width: 100%;'>Комплектоването на длъжностите по възраст:</div>
                         <table class='CommonHeaderTable' style='text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 40px;' rowspan='2'>№ по ред</th>
                               <th style='width: 80px;' rowspan='2'>№ на команда</th>
                               <th style='width: 240px;' colspan='3'>Възраст</th>
                            </tr>
                            <tr>
                               <th style='width: 80px;'>До 35 г.</th>
                               <th style='width: 80px;'>До 45 г.</th>
                               <th style='width: 80px;'>Над 45 г.</th>
                            </tr>
                         </thead>";

                int counter = 0;
                foreach (ReportAnalyzeResFulfilmentByAgeBlock block in ReportResult.ByAgeResult)
                {
                    counter++;

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>                  
                                 <td style='" + cellStyle + @" text-align: left;'>" + counter.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: left;'>" + block.MilitaryCommand + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledAgeUnder35.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledAgeUnder45.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledAgeAbove45.ToString() + @"</td>
                              </tr>";
                }

                html += "</table>";
            }

            if (ReportResult.ByEdu_Officers_Result != null && ReportResult.ByEdu_Officers_Result.Count > 0)
            {
                anyData = true;

                string cellStyle = "vertical-align: top;";

                html += @"<div style='height: 20px;'>&nbsp;</div>
                         <div class='SmallHeaderText' style='text-align: left; width: 100%;'>По притежавано образование комплектуването е:</div>
                         <table class='CommonHeaderTable' style='text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 40px;' rowspan='2'>№ по ред</th>
                               <th style='width: 80px;' rowspan='2'>№ на команда</th>
                               <th style='width: 320px;' colspan='4'>Военно образование</th>
                               <th style='width: 80px;' rowspan='2'>ВСИЧКО</th>
                            </tr>
                            <tr>
                               <th style='width: 80px;'>ВА (ГЩФ)</th>
                               <th style='width: 80px;'>Военно училище</th>
                               <th style='width: 80px;'>Школи</th>
                               <th style='width: 80px;'>Без военно образование</th>
                            </tr>
                         </thead>";

                int counter = 0;
                foreach (ReportAnalyzeResFulfilmentByEdu_Officers_Block block in ReportResult.ByEdu_Officers_Result)
                {
                    counter++;

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>                  
                                 <td style='" + cellStyle + @" text-align: left;'>" + counter.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: left;'>" + block.MilitaryCommand + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledVA.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledVU.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledSZHO.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledNoMilEdu.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledTotal.ToString() + @"</td>
                              </tr>";
                }

                html += "</table>";
            }

            if (ReportResult.ByEdu_OffCand_Result != null && ReportResult.ByEdu_OffCand_Result.Count > 0)
            {
                anyData = true;

                string cellStyle = "vertical-align: top;";

                html += @"<div style='height: 20px;'>&nbsp;</div>
                         <div class='SmallHeaderText' style='text-align: left; width: 100%;'>По притежавано образование комплектуването е:</div>
                         <table class='CommonHeaderTable' style='text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 40px;' rowspan='2'>№ по ред</th>
                               <th style='width: 80px;' rowspan='2'>№ на команда</th>
                               <th style='width: 320px;' colspan='4'>Военно образование</th>
                               <th style='width: 80px;' rowspan='2'>ВСИЧКО</th>
                            </tr>
                            <tr>
                               <th style='width: 80px;'>Военно училище</th>
                               <th style='width: 80px;'>Колеж</th>
                               <th style='width: 80px;'>........</th>
                               <th style='width: 80px;'>Без военно образование</th>
                            </tr>
                         </thead>";

                int counter = 0;
                foreach (ReportAnalyzeResFulfilmentByEdu_OffCand_Block block in ReportResult.ByEdu_OffCand_Result)
                {
                    counter++;

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>                  
                                 <td style='" + cellStyle + @" text-align: left;'>" + counter.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: left;'>" + block.MilitaryCommand + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledVU.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledCollege.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'></td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledNoMilEdu.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledTotal.ToString() + @"</td>
                              </tr>";
                }

                html += "</table>";
            }

            if (ReportResult.ByEdu_Sergeants_Result != null && ReportResult.ByEdu_Sergeants_Result.Count > 0)
            {
                anyData = true;

                string cellStyle = "vertical-align: top;";

                html += @"<div style='height: 20px;'>&nbsp;</div>
                         <div class='SmallHeaderText' style='text-align: left; width: 100%;'>По притежавано образование комплектуването е:</div>
                         <table class='CommonHeaderTable' style='text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 40px;' rowspan='2'>№ по ред</th>
                               <th style='width: 80px;' rowspan='2'>№ на команда</th>
                               <th style='width: 320px;' colspan='4'>Военно образование</th>
                               <th style='width: 80px;' rowspan='2'>ВСИЧКО</th>
                            </tr>
                            <tr>
                               <th style='width: 80px;'>Военно училище</th>
                               <th style='width: 80px;'>Колеж</th>
                               <th style='width: 80px;'>........</th>
                               <th style='width: 80px;'>Без военно образование</th>
                            </tr>
                         </thead>";

                int counter = 0;
                foreach (ReportAnalyzeResFulfilmentByEdu_Sergeants_Block block in ReportResult.ByEdu_Sergeants_Result)
                {
                    counter++;

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>                  
                                 <td style='" + cellStyle + @" text-align: left;'>" + counter.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: left;'>" + block.MilitaryCommand + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledVU.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledCollege.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'></td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledNoMilEdu.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledTotal.ToString() + @"</td>
                              </tr>";
                }

                html += "</table>";
            }

            if (ReportResult.ByEdu_Soldiers_Result != null && ReportResult.ByEdu_Soldiers_Result.Count > 0)
            {
                anyData = true;

                string cellStyle = "vertical-align: top;";

                html += @"<div style='height: 20px;'>&nbsp;</div>
                         <div class='SmallHeaderText' style='text-align: left; width: 100%;'>По притежавано образование комплектуването е:</div>
                         <table class='CommonHeaderTable' style='text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 40px;' rowspan='2'>№ по ред</th>
                               <th style='width: 80px;' rowspan='2'>№ на команда</th>
                               <th style='width: 320px;' colspan='4'>Притежавано образование</th>
                               <th style='width: 80px;' rowspan='2'>ВСИЧКО</th>
                            </tr>
                            <tr>
                               <th style='width: 80px;'>Висше</th>
                               <th style='width: 80px;'>Средно</th>
                               <th style='width: 80px;'>Основно</th>
                               <th style='width: 80px;'>Без образование</th>
                            </tr>
                         </thead>";

                int counter = 0;
                foreach (ReportAnalyzeResFulfilmentByEdu_Soldiers_Block block in ReportResult.ByEdu_Soldiers_Result)
                {
                    counter++;

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>                  
                                 <td style='" + cellStyle + @" text-align: left;'>" + counter.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: left;'>" + block.MilitaryCommand + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledVisshe.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledSredno.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledOsnovno.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledNoEdu.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledTotal.ToString() + @"</td>
                              </tr>";
                }

                html += "</table>";
            }

            if (!anyData)
            {
                html += "<div style='text-align: center;'>Няма намерени данни</div>";
            }

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
            Session[sessionResultsKey] = null;
            reportResult = null;
        }

        private ReportAnalyzeResFulfilmentFilter CollectFilterData()
        {
            ReportAnalyzeResFulfilmentFilter filter = new ReportAnalyzeResFulfilmentFilter();

            filter.MilitaryUnitId = String.IsNullOrEmpty(msMilitaryUnit.SelectedValue) ? 0 : int.Parse(msMilitaryUnit.SelectedValue);
            filter.MilitaryCommandIds = hdnMilitaryCommandSelected.Value;
            filter.MilitaryCategoryKey = ddMilitaryCategory.SelectedValue;

            return filter;
        }

        protected void hdnBtnReloadMilitaryCommands_Click(object sender, EventArgs e)
        {
            PopulateMilitaryCommands();
        }

        private void PopulateMilitaryCommands()
        {
            hdnMilitaryCommandSelected.Value = "";
            int militaryUnitId = Int32.Parse(msMilitaryUnit.SelectedValue);

            string result = "";

            List<MilitaryCommand> militaryCommands = MilitaryCommandUtil.GetMilitaryCommandsByMilitaryUnitAndChildren(CurrentUser, militaryUnitId);

            foreach (MilitaryCommand militaryCommand in militaryCommands)
            {
                string pickListItem = "{value : '" + militaryCommand.MilitaryCommandId.ToString() + "' , label : '" + militaryCommand.DisplayTextForSelection.Replace("'", "\\'") + "'}";
                result += (result == "" ? "" : ",") + pickListItem;
            }

            if (result != "")
                result = "[" + result + "]";

            hdnMilitaryCommandJson.Value = result;
        }
    }
}
