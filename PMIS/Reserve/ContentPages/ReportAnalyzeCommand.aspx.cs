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
    public partial class ReportAnalyzeCommand : RESPage
    {
        //Get the config setting that says how many rows per page should be dispayed in the grid
        private string sessionResultsKey = "ReportAnalyzeCommand";

        private ReportAnalyzeCommandResult reportResult = null;
        private ReportAnalyzeCommandResult ReportResult
        {
            get
            {
                if (reportResult == null)
                {
                    if (Session[sessionResultsKey] != null)
                    {
                        reportResult = (ReportAnalyzeCommandResult)Session[sessionResultsKey];
                    }
                    else
                    {
                        ReportAnalyzeCommandFilter filter = CollectFilterData();
                        reportResult = ReportAnalyzeCommandUtil.GetReportAnalyzeCommand(filter, CurrentUser);
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
                return "RES_REPORTS_REPORTANALYZECOMMAND";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.GetUIItemAccessLevel("RES_REPORTS_REPORTANALYZECOMMAND") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }         
           
            //Hilight the current page in the menu bar
            HighlightMenuItems("Reports", "ReportAnalyzeCommand");

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
        }             

        //Populate all listboxes on the screen
        private void PopulateLists()
        {
            PopulateMilitaryDepartments();
            PopulateMilitaryReadiness();
            PopulateMilitaryCommands();
            PopulateReportType();
        }

        private void PopulateMilitaryCommands()
        {
            ddMilitaryCommand.Items.Clear();

            string readinessID = ddMilitaryReadiness.SelectedValue != ListItems.GetOptionAll().Value ? ddMilitaryReadiness.SelectedValue : "";

            ddMilitaryCommand.DataSource = MilitaryCommandUtil.GetMilitaryCommandsByMilitaryReadinessForReservists(CurrentUser, hdnMilitaryDepartmentSelected.Value, readinessID);
            ddMilitaryCommand.DataTextField = "DisplayTextForSelection";
            ddMilitaryCommand.DataValueField = "MilitaryCommandId";
            ddMilitaryCommand.DataBind();
            ddMilitaryCommand.Items.Insert(0, ListItems.GetOptionAll());

            ddMilitaryCommand_Changed(Page, new EventArgs());
        }

        private void PopulateMilitaryReadiness()
        {
            ddMilitaryReadiness.Items.Clear();

            ddMilitaryReadiness.DataSource = MilitaryReadinessUtil.GetAllMilitaryReadiness(CurrentUser);
            ddMilitaryReadiness.DataTextField = "MilReadinessName";
            ddMilitaryReadiness.DataValueField = "MilReadinessId";
            ddMilitaryReadiness.DataBind();
            ddMilitaryReadiness.Items.Insert(0, ListItems.GetOptionAll());

            ddMilitaryReadiness.SelectedIndex = 0;            
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

        private void PopulateReportType()
        {
            string result = "";
            
            string pickListItem = "{value : 'MilRepSpecAnalyze' , label : 'Анализ по ВОС'}";
            result += (result == "" ? "" : ",") + pickListItem;

            pickListItem = "{value : 'MilRepSpecPositionAnalyze' , label : 'Анализ длъжности по ВОС'}";
            result += (result == "" ? "" : ",") + pickListItem;

            pickListItem = "{value : 'DevilveryAnalyze' , label : 'Анализ по доставяне'}";
            result += (result == "" ? "" : ",") + pickListItem;

            pickListItem = "{value : 'PositionFulfilAnalyze' , label : 'Анализ по длъжности на основно попълнение'}";
            result += (result == "" ? "" : ",") + pickListItem;

            pickListItem = "{value : 'MilRepSpecAndPositionAnalyze' , label : 'Анализ по ВОС и длъжност'}";
            result += (result == "" ? "" : ",") + pickListItem;

            pickListItem = "{value : 'AgeMilRepSpecFulfilAnalyze' , label : 'Анализ по ВОС и възраст'}";
            result += (result == "" ? "" : ",") + pickListItem;
            
            if (result != "")
                result = "[" + result + "]";

            hdnReportTypeJson.Value = result;
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
            if (this.GetUIItemAccessLevel("RES_REPORTS_REPORTANALYZECOMMAND") == UIAccessLevel.Hidden)
                return;

            string html = "";            

            if (ReportResult.OverallResult != null)
            {
                string cellStyle = "vertical-align: top;";

                html += @"<div class='SmallHeaderText' style='text-align: center; width: 100%;'>ОБЩ БРОЙ</div>
                         <table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>                               
                               <th style='width: 240px;' colspan='4'>По заявка</th>
                               <th style='width: 240px;' colspan='4'>Фактическо изпълнение</th>
                               <th style='width: 240px;' colspan='4'>Резерв</th>                                                              
                            </tr> 
                            <tr>
                               <th style='width: 80px;'>Офицери</th>
                               <th style='width: 80px;'>Оф.к-ти</th>
                               <th style='width: 80px;'>Сержанти</th>
                               <th style='width: 80px;'>Войници</th>
                               <th style='width: 80px;'>Офицери</th>
                               <th style='width: 80px;'>Оф.к-ти</th>
                               <th style='width: 80px;'>Сержанти</th>
                               <th style='width: 80px;'>Войници</th>
                               <th style='width: 80px;'>Офицери</th>
                               <th style='width: 80px;'>Оф.к-ти</th>
                               <th style='width: 80px;'>Сержанти</th>
                               <th style='width: 80px;'>Войници</th>
                            </tr>
                         </thead>";

                html += @"<tr class='DataTableOddRow'>                                
                                 <td style='" + cellStyle + @" text-align: right;'>" + reportResult.OverallResult.RequestOfficers.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + reportResult.OverallResult.RequestOffCand.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + reportResult.OverallResult.RequestSergeants.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + reportResult.OverallResult.RequestSoldiers.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + reportResult.OverallResult.FulfiledOfficers.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + reportResult.OverallResult.FulfiledOffCand.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + reportResult.OverallResult.FulfiledSergeants.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + reportResult.OverallResult.FulfiledSoldiers.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right; '>" + reportResult.OverallResult.ReserveOfficers.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right; '>" + reportResult.OverallResult.ReserveOffCand.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + reportResult.OverallResult.ReserveSergeants.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + reportResult.OverallResult.ReserveSoldiers.ToString() + @"</td>
                              </tr>";

                html += "</table>";
            }

            if (ReportResult.MilRepSpecResult != null && ReportResult.MilRepSpecResult.Count > 0)
            {
                string cellStyle = "vertical-align: top;";

                html += @"<div style='height: 20px;'>&nbsp;</div>
                         <div class='SmallHeaderText' style='text-align: center; width: 100%;'>Анализ по ВОС</div>
                         <table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>                        
                               <th style='width: 240px;' rowspan='3'>ВОС</th>       
                               <th style='width: 240px;' rowspan='2' colspan='4'>По заявка</th>
                               <th style='width: 480px;' colspan='8'>Фактическо изпълнение</th>
                               <th style='width: 240px;' rowspan='2' colspan='4'>Резерв</th>                                                              
                            </tr> 
                            <tr>
                                <th style='width: 240px;' colspan='4'></th>
                                <th style='width: 240px;' colspan='4'>От тях по заменки</th>
                            </tr>
                            <tr>
                               <th style='width: 80px;'>Офицери</th>
                               <th style='width: 80px;'>Оф.к-ти</th>
                               <th style='width: 80px;'>Сержанти</th>
                               <th style='width: 80px;'>Войници</th>
                               <th style='width: 80px;'>Офицери</th>
                               <th style='width: 80px;'>Оф.к-ти</th>
                               <th style='width: 80px;'>Сержанти</th>
                               <th style='width: 80px;'>Войници</th>
                               <th style='width: 80px;'>Офицери</th>
                               <th style='width: 80px;'>Оф.к-ти</th>
                               <th style='width: 80px;'>Сержанти</th>
                               <th style='width: 80px;'>Войници</th>
                               <th style='width: 80px;'>Офицери</th>
                               <th style='width: 80px;'>Оф.к-ти</th>
                               <th style='width: 80px;'>Сержанти</th>
                               <th style='width: 80px;'>Войници</th>
                            </tr>
                         </thead>";

                int counter = 0;
                foreach (ReportMilRepSpecAnalyzeCommandBlock block in ReportResult.MilRepSpecResult)
                {
                    counter++;

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>                  
                                <td style='" + cellStyle + @" text-align: left;'>" + block.MilRepSpecName + @"</td>              
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.RequestOfficers.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.RequestOffCand.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.RequestSergeants.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.RequestSoldiers.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledOfficers.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledOffCand.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledSergeants.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledSoldiers.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.ChangesOfficers.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.ChangesOffCand.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.ChangesSergeants.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.ChangesSoldiers.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.ReserveOfficers.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.ReserveOffCand.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.ReserveSergeants.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.ReserveSoldiers.ToString() + @"</td>
                              </tr>";
                }

                html += "</table>";
            }

            if (ReportResult.PositionMRSResult != null && ReportResult.PositionMRSResult.Count > 0)
            {
                int maxColumns = 0;

                foreach (ReportPositionMRSAnalyzePositionBlock block in ReportResult.PositionMRSResult)
                {
                    if (maxColumns < block.MRS.Count)
                        maxColumns = block.MRS.Count;
                }

                string cellStyle = "vertical-align: top;";

                html += @"<div style='height: 20px;'>&nbsp;</div>
                         <div class='SmallHeaderText' style='text-align: center; width: 100%;'>Анализ длъжности по ВОС</div>
                         <table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                                <th style='width: 240px;'>Длъжност</th>
                        ";

                for (int i = 0; i < maxColumns; i++)
                {
                    html += @" 
                                <th style='width: 120px;'>ВОС</th>
                                <th style='width: 30px;'>Бр</th>
                             ";
                }

                html += @"
                            <th style='width: 30px;'>Всичко</th>
                            </tr>
                         </thead>";

                int counter = 0;
                foreach (ReportPositionMRSAnalyzePositionBlock block in ReportResult.PositionMRSResult)
                {                    
                    counter++;

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>";

                    html += @"<td style='" + cellStyle + @" text-align: left;'>" + block.Position + @"</td>";

                    int totalCell = 0;
                    foreach (ReportPositionMRSAnalyzeMRSBlock cell in block.MRS)
                    {                       
                        html += @"<td style='" + cellStyle + @" text-align: left;'>" + cell.MilitaryReportSpecilityName + @"</td>";
                        html += @"<td style='" + cellStyle + @" text-align: left;'>" + cell.Cnt.ToString() + @"</td>";

                        totalCell += cell.Cnt;
                    }

                    for (int i = 0; i < maxColumns - block.MRS.Count; i++)
                    {
                        html += @"<td>&nbsp;</td>";
                        html += @"<td>&nbsp;</td>";
                    }

                    html += @"<td style='" + cellStyle + @" text-align: left;'>" + totalCell.ToString() + @"</td>";

                    html += "</tr>";
                }

                html += "</table>";
            }

            if (ReportResult.PositionDeliveryResult != null && ReportResult.PositionDeliveryResult.Count > 0)
            {
                string cellStyle = "vertical-align: top;";

                html += @"<div style='height: 20px;'>&nbsp;</div>
                         <div class='SmallHeaderText' style='text-align: center; width: 100%;'>Анализ по доставяне</div>
                         <table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>                        
                               <th rowspan='2' style='width: 240px;'>Община</th>
                               <th rowspan='2' style='width: 240px;'>Район</th>
                               <th colspan='4' style='width: 180px;'>Основно попълнение</th>
                               <th colspan='4' style='width: 180px;'>Резерв</th>                                                            
                            </tr>  
                            <tr>
                                <th>Офицери</th>
                                <th>Оф.к-ти</th>
                                <th>Сержанти</th>
                                <th>Войници</th>
                                <th>Офицери</th>
                                <th>Оф.к-ти</th>
                                <th>Сержанти</th>
                                <th>Войници</th>
                            </tr>                                                         
                         </thead>";

                int counter = 0;
                foreach (ReportPositionDeliveryAnalyzeBlock block in ReportResult.PositionDeliveryResult)
                {
                    counter++;

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>                  
                                <td style='" + cellStyle + @" text-align: left;'>" + block.MuniciplaityName + @"</td>
                                <td style='" + cellStyle + @" text-align: left;'>" + block.DistrictName + @"</td>
                                <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledOfficers.ToString() + @"</td>
                                <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledOffCand.ToString() + @"</td>
                                <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledSergeants.ToString() + @"</td>
                                <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledSoldiers.ToString() + @"</td>
                                <td style='" + cellStyle + @" text-align: right;'>" + block.ReserveOfficers.ToString() + @"</td>
                                <td style='" + cellStyle + @" text-align: right;'>" + block.ReserveOffCand.ToString() + @"</td>
                                <td style='" + cellStyle + @" text-align: right;'>" + block.ReserveSergeants.ToString() + @"</td>
                                <td style='" + cellStyle + @" text-align: right;'>" + block.ReserveSoldiers.ToString() + @"</td>
                              </tr>";
                }

                html += "</table>";
            }

            if (ReportResult.PositionFulfilResult != null && ReportResult.PositionFulfilResult.Count > 0)
            {
                string cellStyle = "vertical-align: top;";

                html += @"<div style='height: 20px;'>&nbsp;</div>
                         <div class='SmallHeaderText' style='text-align: center; width: 100%;'>Анализ по длъжности на основно попълнение</div>
                         <table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>                        
                               <th style='width: 240px;' rowspan='3'>Длъжност</th>       
                               <th style='width: 560px;' colspan='7'>Образование</th>
                               <th style='width: 240px;' colspan='3'>Възраст</th>
                               <th style='width: 80px;' rowspan='3'>С военна подготовка</th>
                               <th style='width: 80px;' rowspan='3'>Нуждаят се от курс</th>
                            </tr> 
                            <tr>
                                <th style='width: 240px;' colspan='3'>общо</th>
                                <th style='width: 310px;' colspan='4'>военно</th>
                                <th style='width: 80px;' rowspan='2'>до 35 г.</th>
                                <th style='width: 80px;' rowspan='2'>до 45 г.</th>
                                <th style='width: 80px;' rowspan='2'>над 45 г.</th>
                            </tr>
                            <tr>
                               <th style='width: 80px;'>висше</th>
                               <th style='width: 80px;'>полувсише</th>
                               <th style='width: 80px;'>средно</th>
                               <th style='width: 80px;'>ВА</th>
                               <th style='width: 80px;'>ВУ</th>
                               <th style='width: 80px;'>ШЗО</th>
                               <th style='width: 80px;'>без военно обр.</th>                               
                            </tr>
                         </thead>";

                int counter = 0;
                foreach (ReportPositionFulfilAnalyzeBlock block in ReportResult.PositionFulfilResult)
                {
                    counter++;

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>                  
                                <td style='" + cellStyle + @" text-align: left;'>" + block.Position + @"</td>              
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.VisheEducation.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.PoluvisheEducation.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.SrednoEducation.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.VAEducation.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.VUEducation.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.SZHOEducation.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.NoMilEducation.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.AgeUnder35.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.AgeUnder45.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.AgeAbove45.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.MilitaryTraining.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.NeedCourse.ToString() + @"</td>
                              </tr>";
                }

                html += "</table>";
            }

            if (ReportResult.MilRepSpecAndPositionResult != null && ReportResult.MilRepSpecAndPositionResult.Count > 0)
            {
                string cellStyle = "vertical-align: top;";

                html += @"<div style='height: 20px;'>&nbsp;</div>
                         <div class='SmallHeaderText' style='text-align: center; width: 100%;'>Анализ по ВОС и длъжност</div>
                         <table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>                        
                               <th style='width: 240px;' rowspan='3'>Длъжност</th>
                               <th style='width: 240px;' rowspan='3'>ВОС</th>
                               <th style='width: 240px;' rowspan='2' colspan='4'>Заявка на формированието</th>
                               <th style='width: 480px;' colspan='8'>Фактическо изпълнение</th>
                               <th style='width: 240px;' rowspan='2' colspan='4'>процент резерв с МН/бр./</th>                                                              
                            </tr> 
                            <tr>
                                <th style='width: 240px;' colspan='4'></th>
                                <th style='width: 240px;' colspan='4'>От тях по заменка</th>
                            </tr>
                            <tr>
                               <th style='width: 80px;'>Офицери</th>
                               <th style='width: 80px;'>Оф. к-ти</th>
                               <th style='width: 80px;'>Сержанти</th>
                               <th style='width: 80px;'>Войници</th>
                               <th style='width: 80px;'>Офицери</th>
                               <th style='width: 80px;'>Оф. к-ти</th>
                               <th style='width: 80px;'>Сержанти</th>
                               <th style='width: 80px;'>Войници</th>
                               <th style='width: 80px;'>Офицери</th>
                               <th style='width: 80px;'>Оф. к-ти</th>
                               <th style='width: 80px;'>Сержанти</th>
                               <th style='width: 80px;'>Войници</th>
                               <th style='width: 80px;'>Офицери</th>
                               <th style='width: 80px;'>Оф. к-ти</th>
                               <th style='width: 80px;'>Сержанти</th>
                               <th style='width: 80px;'>Войници</th>
                            </tr>
                         </thead>";

                string prevPosition = "";
                int counter = 0;
                foreach (ReportMilRepSpecAndPositionAnalyzeCommandBlock block in ReportResult.MilRepSpecAndPositionResult)
                {
                    int rowsPerPosition = ReportResult.MilRepSpecAndPositionResult.Where(x => x.Position == block.Position).Count();

                    counter++;

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>     
                                 " + (prevPosition != block.Position ? 
                               @"<td style='" + cellStyle + @" text-align: left;' rowspan=" + rowsPerPosition + " >" + block.Position + @"</td>" : "") + @"
                                 <td style='" + cellStyle + @" text-align: left; " + (block.IsPrimary ? "font-weight: bold;" : "") + "'>" + block.MilRepSpecName + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.RequestOfficers.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.RequestOffCand.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.RequestSergeants.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.RequestSoldiers.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledOfficers.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledOffCand.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledSergeants.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.FulfiledSoldiers.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.ChangesOfficers.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.ChangesOffCand.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.ChangesSergeants.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.ChangesSoldiers.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.ReserveOfficers.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.ReserveOffCand.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.ReserveSergeants.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: right;'>" + block.ReserveSoldiers.ToString() + @"</td>
                              </tr>";

                    prevPosition = block.Position;
                }

                html += "</table>";
            }

            if (ReportResult.AgeMRSResult != null && ReportResult.AgeMRSResult.Count > 0)
            {
                int maxColumns = 0;
                if (maxColumns < ReportResult.AgeMRSResult.Count)
                {
                    maxColumns = ReportResult.AgeMRSResult.Count;
                }

                string cellStyle = "vertical-align: top;";

                html += @"<div style='height: 20px;'>&nbsp;</div>
                         <div class='SmallHeaderText' style='text-align: center; width: 100%;'>Анализ на основното попълниение по ВОС и възраст</div>
                         <table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                                <th></th>
                                <th style='width: 240px;' colspan='3'>Възраст</th>
                            </tr>
                        ";
                html += @"<th style='width: 300px;'>ВОС</th>";
                for (int i = 0; i < maxColumns; i++)
                {
                    html += @" <th style='width: 30px;'>" + ReportResult.AgeMRSResult[i].Age + @"</th>";
                }

                html += @"</thead>";
                

                int counter = 0;
                int indexMRS = 0;
                int totalFirstCol = 0;
                int totalSecondCol = 0;
                int totalThirdCol = 0;
                foreach (ReportAgeMRSAnalyzeMRSBlock block in ReportResult.AgeMRSResult[indexMRS].MRS)
                {
                    
                    counter++;

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>";

                    html += @"<td style='" + cellStyle + @" text-align: left; '>" + block.MilitaryReportSpecilityName + @"</td>";


                    int countCell = 0;
                    foreach (ReportAgeMRSAnalyzeBlock cell in ReportResult.AgeMRSResult)
                    {
                        html += @"<td style='" + cellStyle + @" text-align: right;'>" + cell.MRS[indexMRS].Cnt + @"</td>";

                        switch (countCell)
                        {
                            case 0:
                                totalFirstCol += cell.MRS[indexMRS].Cnt;
                                break;
                            case 1:
                                totalSecondCol += cell.MRS[indexMRS].Cnt;
                                break;
                            case 2:
                                totalThirdCol += cell.MRS[indexMRS].Cnt;
                                break;
                        }
                        countCell++;
                    }

                    html += "</tr>";
                    indexMRS++;
                }
                html += @"<tfoot>
                            <tr>
                              <th style='text-align: left'>Всичко</th>
                              <td style='text-align: right'>" + totalFirstCol + @"</td>
                              <td style='text-align: right'>" + totalSecondCol + @"</td>
                              <td style='text-align: right'>" + totalThirdCol + @"</td>
                            </tr>
                          </tfoot>";
                html += "</table>";
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
            hdnMilitaryDepartmentSelected.Value = "";
            PopulateMilitaryReadiness();
            PopulateMilitaryCommands();
            hdnReportTypeSelected.Value = "";

            Session[sessionResultsKey] = null;
            reportResult = null;
        }

        private ReportAnalyzeCommandFilter CollectFilterData()
        {
            ReportAnalyzeCommandFilter filter = new ReportAnalyzeCommandFilter();

            filter.MilitaryDepartmentIds = hdnMilitaryDepartmentSelected.Value;
            hdnMilitaryDepartmentId.Value = hdnMilitaryDepartmentSelected.Value;

            filter.MilitaryReadinessID = int.Parse(ddMilitaryReadiness.SelectedValue);

            filter.MilitaryCommandId = int.Parse(ddMilitaryCommand.SelectedValue);
            hdnMilitaryCommandId.Value = ddMilitaryCommand.SelectedValue;

            filter.MilitaryCommandSuffix = ddSubMilitaryCommand.SelectedValue;
            hdnSubMilitaryCommandId.Value = ddSubMilitaryCommand.SelectedValue;

            filter.ReportType = hdnReportTypeSelected.Value;
            hdnReportType.Value = hdnReportTypeSelected.Value;

            return filter;
        }

        protected void ddMilitaryReadiness_Changed(object sender, EventArgs e)
        {
            string readinessID = ddMilitaryReadiness.SelectedValue != ListItems.GetOptionAll().Value ? ddMilitaryReadiness.SelectedValue : "";

            ddMilitaryCommand.DataSource = MilitaryCommandUtil.GetMilitaryCommandsByMilitaryReadinessForReservists(CurrentUser, hdnMilitaryDepartmentSelected.Value, readinessID);
            ddMilitaryCommand.DataTextField = "DisplayTextForSelection";
            ddMilitaryCommand.DataValueField = "MilitaryCommandId";
            ddMilitaryCommand.DataBind();
            ddMilitaryCommand.Items.Insert(0, ListItems.GetOptionAll());

            ddMilitaryCommand_Changed(Page, new EventArgs());
        }

        protected void ddMilitaryCommand_Changed(object sender, EventArgs e)
        {
            string readinessID = ddMilitaryReadiness.SelectedValue != ListItems.GetOptionAll().Value ? ddMilitaryReadiness.SelectedValue : "";

            ddSubMilitaryCommand.DataSource = RequestCommandUtil.GetRequestCommandsForMilCommandAndMilDeptAndMilReadiness(CurrentUser, int.Parse(ddMilitaryCommand.SelectedValue), hdnMilitaryDepartmentSelected.Value, readinessID);
            ddSubMilitaryCommand.DataTextField = "Txt";
            ddSubMilitaryCommand.DataValueField = "Val";
            ddSubMilitaryCommand.DataBind();
            ddSubMilitaryCommand.Items.Insert(0, ListItems.GetOptionAll());
        }

        protected void hdnBtnReloadMilitaryCommands_Click(object sender, EventArgs e)
        {
            PopulateMilitaryCommands();
        }
    }
}
