using System;
using System.Linq;
using System.Text;

using PMIS.Common;
using PMIS.Reserve.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace PMIS.Reserve.PrintContentPages
{
    public partial class PrintReportAnalyzeCommand : RESPage
    {
        private string sessionResultsKey = "ReportAnalyzeCommand";

        const string All = "Всички";

        string militaryDepartmentId = "";
        string militaryCommandId = "";
        string militaryReadinessID = "";
        string militaryCommandSuffix = "";
        string reportType = "";

        public void SetPrintTitleLinesWidth()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnPrintTitleLinesWidth");

            hf.Value = "1024";
        }

        public void SetHeadersLeft()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnHeadersLeft");

            hf.Value = "255";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.SetPrintTitleLinesWidth();
                this.SetHeadersLeft();
            }

            // Check visibility right for the print screen
            if (GetUIItemAccessLevel("RES_REPORTS") == UIAccessLevel.Hidden ||
                this.GetUIItemAccessLevel("RES_REPORTS_REPORTANALYZECOMMAND") != UIAccessLevel.Hidden)
            {
                if (!String.IsNullOrEmpty(Request.Params["MilitaryDepartmentId"]))
                {
                    militaryDepartmentId = Request.Params["MilitaryDepartmentId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["MilitaryCommandId"]))
                {
                    militaryCommandId = Request.Params["MilitaryCommandId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["MilitaryReadinessID"]))
                {
                    militaryReadinessID = Request.Params["MilitaryReadinessID"];
                }

                if (!String.IsNullOrEmpty(Request.Params["MilitaryCommandSuffix"]))
                {
                    militaryCommandSuffix = Request.Params["MilitaryCommandSuffix"];
                }

                if (!String.IsNullOrEmpty(Request.Params["ReportType"]))
                {
                    reportType = Request.Params["ReportType"];
                }

                if (!IsPostBack)
                {
                    if (Request.Params["Export"] != null && Request.Params["Export"].ToLower() == "true")
                    {
                        btnGenerateExcel_Click(this, new EventArgs());
                    }
                    else
                    {
                        this.divResults.InnerHtml = GeneratePageContent(false);
                    }
                }
            }
            else
            {
                this.divResults.InnerHtml = "";
            }
        }

        // Generate the page content's html
        private string GeneratePageContent(bool isExport)
        {
            string contentPage = "";

            if (!isExport)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<table>");
                sb.Append("<tr>");
                sb.Append("<td rowspan=\"2\">" + this.GenerateAllRecordsHtml() + "</td>");
                sb.Append("<td style=\"vertical-align: top;\">" + CommonFunctions.GenerateButtons(true, 160, true) + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr><td style=\"vertical-align: bottom;\">" + CommonFunctions.GenerateButtons(false, 0, true) + "</td></tr>");
                sb.Append("</table>");

                contentPage = sb.ToString();
            }
            else
            {
                contentPage = this.GenerateAllRecordsForExport();
            }

            return contentPage;
        }

        // Generate display data as html into the page
        private string GenerateAllRecordsHtml()
        {
            ReportAnalyzeCommandFilter filter = new ReportAnalyzeCommandFilter()
            {
                MilitaryDepartmentIds = militaryDepartmentId,
                MilitaryCommandId = int.Parse(militaryCommandId),
                MilitaryReadinessID = int.Parse(militaryReadinessID),
                MilitaryCommandSuffix = militaryCommandSuffix,
                ReportType = reportType
            };

            ReportAnalyzeCommandResult reportResult = null;

            if (Session[sessionResultsKey] != null)
                reportResult = (ReportAnalyzeCommandResult)Session[sessionResultsKey];
            else
                reportResult = ReportAnalyzeCommandUtil.GetReportAnalyzeCommand(filter, CurrentUser);


            string militaryDepartmentName = "";
            List<MilitaryDepartment> militaryDepartments = MilitaryDepartmentUtil.GetAllMilitaryDepartmentsByIDs(CurrentUser, militaryDepartmentId);
            foreach (MilitaryDepartment militaryDepartment in militaryDepartments)
            {
                militaryDepartmentName += (militaryDepartmentName == "" ? "" : ", ") + militaryDepartment.MilitaryDepartmentName;
            }

            if(militaryDepartmentName == "")
            {
                militaryDepartmentName = All;
            }

            string militaryReadinessName = All;
            MilitaryReadiness militaryReadinesses = MilitaryReadinessUtil.GetMilitaryReadiness(filter.MilitaryReadinessID, CurrentUser);

            if (militaryReadinesses != null)
                militaryReadinessName = militaryReadinesses.MilReadinessName;                                   

            string militaryCommandName = All;
            MilitaryCommand militaryCommand = MilitaryCommandUtil.GetMilitaryCommand(filter.MilitaryCommandId, CurrentUser);
            if (militaryCommand != null)
                militaryCommandName = militaryCommand.DisplayTextForSelection;

            string militarySubCommand = All;
            if (militaryCommand != null && !string.IsNullOrEmpty(militaryCommandSuffix) && militaryCommandSuffix != "-1")
                militarySubCommand = militaryCommand.CommandNumber + " " + militaryCommandSuffix + " " + militaryCommand.ShortName;

            StringBuilder html = new StringBuilder();

            html.Append(@"<table style='padding: 5px; width: 1024px;'>
                             <tr>
                                <td align='center'>
                                    <span class='Label'>Военно окръжие:&nbsp;</span>
                                    <span class='ValueLabel'>" + militaryDepartmentName + @"</span>                                  
                                </td>
                             </tr>
                             <tr>
                                <td align='center'>
                                    <span class='Label'>Готовност:&nbsp;</span>
                                    <span class='ValueLabel'>" + militaryReadinessName + @"</span>                                  
                                </td>
                             </tr>
                             <tr>
                                <td align='center'>                                   
                                    <span class='Label'>Команда:&nbsp;</span>
                                    <span class='ValueLabel'>" + militaryCommandName + @"</span>                                    
                                </td>
                             </tr>
                             <tr>
                                <td align='center'>                                  
                                    <span class='Label'>Подкоманда:&nbsp;</span>
                                    <span class='ValueLabel'>" + militarySubCommand + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td>&nbsp;</td>
                             </tr>
                            ");

            if (reportResult.OverallResult != null)
            {
                string cellStyle = "vertical-align: top;";

                html.Append(@"<tr><td align='center'>
                         <div class='SmallHeaderText' style='text-align: center; width: 100%;'>ОБЩ БРОЙ</div>
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
                         </thead>");

                html.Append(@"<tr class='DataTableOddRow'>                                
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
                              </tr>");

                html.Append(@"</table>
                          </td></tr>");
            }

            if (reportResult.MilRepSpecResult != null && reportResult.MilRepSpecResult.Count > 0)
            {
                string cellStyle = "vertical-align: top;";

                html.Append(@"<tr><td align='center'>
                         <div style='height: 20px;'>&nbsp;</div>
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
                         </thead>");

                int counter = 0;
                foreach (ReportMilRepSpecAnalyzeCommandBlock block in reportResult.MilRepSpecResult)
                {
                    counter++;

                    html.Append(@"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>                  
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
                              </tr>");
                }

                html.Append(@"</table>
                          </td></tr>");
            }

            if (reportResult.PositionMRSResult != null && reportResult.PositionMRSResult.Count > 0)
            {
                int maxColumns = 0;

                foreach (ReportPositionMRSAnalyzePositionBlock block in reportResult.PositionMRSResult)
                {
                    if (maxColumns < block.MRS.Count)
                        maxColumns = block.MRS.Count;
                }

                string cellStyle = "vertical-align: top;";

                html.Append(@"<tr><td align='center'>
                         <div style='height: 20px;'>&nbsp;</div>
                         <div class='SmallHeaderText' style='text-align: center; width: 100%;'>Анализ длъжности по ВОС</div>
                         <table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                                <th style='width: 240px;'>Длъжност</th>
                        ");

                for (int i = 0; i < maxColumns; i++)
                {
                    html.Append(@" 
                                <th style='width: 120px;'>ВОС</th>
                                <th style='width: 30px;'>Бр</th>
                             ");
                }

                html.Append(@"
                            <th style='width: 30px;'>Всичко</th>
                            </tr>
                         </thead>");

                int counter = 0;
                foreach (ReportPositionMRSAnalyzePositionBlock block in reportResult.PositionMRSResult)
                {
                    counter++;

                    html.Append(@"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>");

                    html.Append(@"<td style='" + cellStyle + @" text-align: left;'>" + block.Position + @"</td>");

                    int totalCell = 0;
                    foreach (ReportPositionMRSAnalyzeMRSBlock cell in block.MRS)
                    {
                        html.Append(@"<td style='" + cellStyle + @" text-align: left;'>" + cell.MilitaryReportSpecilityName + @"</td>");
                        html.Append(@"<td style='" + cellStyle + @" text-align: left;'>" + cell.Cnt.ToString() + @"</td>");

                        totalCell += cell.Cnt;
                    }

                    for (int i = 0; i < maxColumns - block.MRS.Count; i++)
                    {
                        html.Append(@"<td>&nbsp;</td>");
                        html.Append(@"<td>&nbsp;</td>");
                    }

                    html.Append(@"<td style='" + cellStyle + @" text-align: left;'>" + totalCell.ToString() + @"</td>");

                    html.Append("</tr>");
                }

                html.Append(@"</table>
                          </td></tr>");
            }

            if (reportResult.PositionDeliveryResult != null && reportResult.PositionDeliveryResult.Count > 0)
            {
                string cellStyle = "vertical-align: top;";

                html.Append(@"<tr><td align='center'>
                         <div style='height: 20px;'>&nbsp;</div>
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
                         </thead>");

                int counter = 0;
                foreach (ReportPositionDeliveryAnalyzeBlock block in reportResult.PositionDeliveryResult)
                {
                    counter++;

                    html.Append(@"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>                  
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
                              </tr>");
                }

                html.Append(@"</table>
                          </td></tr>");
            }

            if (reportResult.PositionFulfilResult != null && reportResult.PositionFulfilResult.Count > 0)
            {
                string cellStyle = "vertical-align: top;";

                html.Append(@"<tr><td align='center'>
                         <div style='height: 20px;'>&nbsp;</div>
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
                         </thead>");

                int counter = 0;
                foreach (ReportPositionFulfilAnalyzeBlock block in reportResult.PositionFulfilResult)
                {
                    counter++;

                    html.Append(@"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>                  
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
                              </tr>");
                }

                html.Append(@"</table>
                          </td></tr>");
            }

            if (reportResult.MilRepSpecAndPositionResult != null && reportResult.MilRepSpecAndPositionResult.Count > 0)
            {
                string cellStyle = "vertical-align: top;";

                html.Append(@"<tr><td align='center'>
                         <div style='height: 20px;'>&nbsp;</div>
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
                         </thead>");

                string prevPosition = "";
                int counter = 0;
                foreach (ReportMilRepSpecAndPositionAnalyzeCommandBlock block in reportResult.MilRepSpecAndPositionResult)
                {
                    int rowsPerPosition = reportResult.MilRepSpecAndPositionResult.Where(x => x.Position == block.Position).Count();

                    counter++;

                    html.Append(@"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'> 
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
                              </tr>");

                    prevPosition = block.Position;
                }

                html.Append(@"</table>
                          </td></tr>");
            }

            if (reportResult.AgeMRSResult != null && reportResult.AgeMRSResult.Count > 0)
            {
                int maxColumns = 0;
                if (maxColumns < reportResult.AgeMRSResult.Count)
                {
                    maxColumns = reportResult.AgeMRSResult.Count;
                }
                string cellStyle = "vertical-align: top;";

                html.Append(@"<tr><td align='center'>
                         <div style='height: 20px;'>&nbsp;</div>
                         <div class='SmallHeaderText' style='text-align: center; width: 100%;'>Анализ на основното попълниение по ВОС и възраст</div>
                         <table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                                <th></th>
                                <th style='width: 240px;' colspan='3'>Възраст</th>
                            </tr>
                            <th style='width: 250px;'>ВОС</th>
                        ");

                for (int i = 0; i < maxColumns; i++)
                {
                    html.Append(@" <th style='width: 30px;'>" + reportResult.AgeMRSResult[i].Age + @"</th>");
                }

                html.Append(@"</thead>");

                int counter = 0;
                int indexMRS = 0;
                int totalFirstCol = 0;
                int totalSecondCol = 0;
                int totalThirdCol = 0;
                foreach (ReportAgeMRSAnalyzeMRSBlock block in reportResult.AgeMRSResult[indexMRS].MRS)
                {

                    counter++;

                    html.Append(@"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>");

                    html.Append(@"<td style='" + cellStyle + @" text-align: left; '>" + block.MilitaryReportSpecilityName + @"</td>");


                    int countCell = 0;
                    foreach (ReportAgeMRSAnalyzeBlock cell in reportResult.AgeMRSResult)
                    {
                        html.Append(@"<td style='" + cellStyle + @" text-align: right;'>" + cell.MRS[indexMRS].Cnt + @"</td>");

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

                    html.Append(@"</tr>");
                    indexMRS++;
                }
                html.Append(@"<tfoot>
                            <tr>
                              <th style='text-align: left'>Всичко</th>
                              <td style='text-align: right'>" + totalFirstCol + @"</td>
                              <td style='text-align: right'>" + totalSecondCol + @"</td>
                              <td style='text-align: right'>" + totalThirdCol + @"</td>
                            </tr>
                          </tfoot>");
                html.Append(@"</table>");
            }

            return html.ToString();
        }

        // Handles export to excel button click
        protected void btnGenerateExcel_Click(object sender, EventArgs e)
        {
            string result = this.GeneratePageContent(true);
            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment; filename=ReportAnalyzeCommand.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        // Generate html as excel response result
        private string GenerateAllRecordsForExport()
        {
            ReportAnalyzeCommandFilter filter = new ReportAnalyzeCommandFilter()
            {
                MilitaryDepartmentIds = militaryDepartmentId,
                MilitaryCommandId = int.Parse(militaryCommandId),
                MilitaryReadinessID = int.Parse(militaryReadinessID),
                MilitaryCommandSuffix = militaryCommandSuffix,
                ReportType = reportType
            };

            ReportAnalyzeCommandResult reportResult = null;

            if (Session[sessionResultsKey] != null)
                reportResult = (ReportAnalyzeCommandResult)Session[sessionResultsKey];
            else
                reportResult = ReportAnalyzeCommandUtil.GetReportAnalyzeCommand(filter, CurrentUser);


            string militaryDepartmentName = "";
            List<MilitaryDepartment> militaryDepartments = MilitaryDepartmentUtil.GetAllMilitaryDepartmentsByIDs(CurrentUser, militaryDepartmentId);
            foreach (MilitaryDepartment militaryDepartment in militaryDepartments)
            {
                militaryDepartmentName += (militaryDepartmentName == "" ? "" : ", ") + militaryDepartment.MilitaryDepartmentName;
            }

            if (militaryDepartmentName == "")
            {
                militaryDepartmentName = All;
            }

            string militaryReadinessName = All;
            MilitaryReadiness militaryReadinesses = MilitaryReadinessUtil.GetMilitaryReadiness(filter.MilitaryReadinessID, CurrentUser);

            if (militaryReadinesses != null)
                militaryReadinessName = militaryReadinesses.MilReadinessName;

            string militaryCommandName = All;
            MilitaryCommand militaryCommand = MilitaryCommandUtil.GetMilitaryCommand(filter.MilitaryCommandId, CurrentUser);
            if (militaryCommand != null)
                militaryCommandName = militaryCommand.DisplayTextForSelection;

            string militarySubCommand = All;
            if (militaryCommand != null && !string.IsNullOrEmpty(militaryCommandSuffix) && militaryCommandSuffix != "-1")
                militarySubCommand = militaryCommand.CommandNumber + " " + militaryCommandSuffix + " " + militaryCommand.ShortName;

            StringBuilder html = new StringBuilder();
            html.Append(@"<?xml version=""1.0""?> 

						   <Workbook xmlns=""urn:schemas-microsoft-com:office:spreadsheet""
									 xmlns:o=""urn:schemas-microsoft-com:office:office""
									 xmlns:x=""urn:schemas-microsoft-com:office:excel""
									 xmlns:ss=""urn:schemas-microsoft-com:office:spreadsheet""
									 xmlns:html=""http://www.w3.org/TR/REC-html40"">

							<Styles>
                                <Style ss:ID=""N"" ss:Name=""Normal"">
                                    <Alignment ss:Horizontal=""Right"" ss:Vertical=""Bottom""/>
                                    <Borders/>
                                    <Font ss:FontName=""Arial""/>
                                    <Interior/>
                                    <NumberFormat/>                            
                                </Style>

                                 <Style ss:ID=""H"" ss:Parent=""N"">
                                   <Alignment ss:Horizontal=""Center"" ss:Vertical=""Top"" ss:ReadingOrder=""LeftToRight"" ss:WrapText=""1""/>
                                   <Font ss:FontName=""Arial"" ss:Size=""16"" ss:Bold=""1""/>                         
                                 </Style>
                                 <Style ss:ID=""H2"" ss:Parent=""N"">
                                   <Alignment ss:Horizontal=""Center"" ss:Vertical=""Top"" ss:ReadingOrder=""LeftToRight"" ss:WrapText=""1""/>
                                   <Font ss:FontName=""Arial"" ss:Size=""20"" ss:Bold=""1""/>                         
                                 </Style>
                                <Style ss:ID=""H3"" ss:Parent=""N"">
                                   <Alignment ss:Horizontal=""Center"" ss:Vertical=""Top"" ss:ReadingOrder=""LeftToRight"" ss:WrapText=""1""/>
                                   <Font ss:FontName=""Arial"" />                                   
                                 </Style>
                                 <Style ss:ID=""H4"" ss:Parent=""N"">
                                   <Alignment ss:Horizontal=""Center"" ss:Vertical=""Top"" ss:ReadingOrder=""LeftToRight"" ss:WrapText=""1""/>
                                   <Font ss:FontName=""Arial"" ss:Size=""13"" ss:Bold=""1""/>                         
                                 </Style>
                                 <Style ss:ID=""H5"" ss:Parent=""N"">
                                   <Alignment ss:Horizontal=""Center"" ss:Vertical=""Top"" ss:ReadingOrder=""LeftToRight"" ss:WrapText=""1""/>
                                   <Font ss:FontName=""Arial"" ss:Size=""13"" ss:Bold=""1""/>                         
                                 </Style>
                                 <Style ss:ID=""H6"" ss:Parent=""N"">
                                   <Alignment ss:Horizontal=""Center"" ss:Vertical=""Top"" ss:ReadingOrder=""LeftToRight"" ss:WrapText=""1""/>
                                   <Font ss:FontName=""Arial"" ss:Size=""10""/>                         
                                 </Style>
                                <Style ss:ID=""H7"" ss:Parent=""N"">
                                    <Alignment ss:Horizontal=""Center"" ss:Vertical=""Bottom"" ss:ReadingOrder=""LeftToRight"" ss:WrapText=""1""/>
                                    <Borders>
                                        <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1""/>
                                        <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1""/>
                                        <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1""/>
                                        <Border ss:Position=""Top"" ss:LineStyle=""Continuous"" ss:Weight=""1""/>
                                    </Borders>
                                    <Font ss:FontName=""Arial"" ss:Size=""10"" ss:Bold=""1""/>                                    
                                </Style>                               
                                 
                                 
                                  <Style ss:ID=""C1"" ss:Parent=""N"">
                                    <Alignment ss:Horizontal=""Right"" ss:Vertical=""Top"" ss:ReadingOrder=""LeftToRight"" ss:WrapText=""1""/>
                                    <Borders>
                                        <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1""/>
                                        <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1""/>
                                        <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1""/>
                                    </Borders>
                                    <Font ss:FontName=""Arial"" ss:Size=""10""/>
                                  </Style>
                                  <Style ss:ID=""C2"" ss:Parent=""N"">
                                    <Alignment ss:Horizontal=""Right"" ss:Vertical=""Top"" ss:ReadingOrder=""LeftToRight"" ss:WrapText=""1""/>
                                    <Borders>
                                        <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1""/>
                                        <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1""/>
                                        <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1""/>
                                    </Borders>
                                    <Font ss:FontName=""Arial"" ss:Size=""10""/>
                                  </Style>
                                  <Style ss:ID=""C2_B"" ss:Parent=""N"">
                                    <Alignment ss:Horizontal=""Left"" ss:Vertical=""Top"" ss:ReadingOrder=""LeftToRight"" ss:WrapText=""1""/>
                                    <Borders>
                                        <Border ss:Position=""Bottom"" ss:LineStyle=""Continuous"" ss:Weight=""1""/>
                                        <Border ss:Position=""Left"" ss:LineStyle=""Continuous"" ss:Weight=""1""/>
                                        <Border ss:Position=""Right"" ss:LineStyle=""Continuous"" ss:Weight=""1""/>
                                    </Borders>
                                    <Font ss:FontName=""Arial"" ss:Size=""10"" ss:Bold=""1"" />
                                  </Style>
                           
                           </Styles>
");

            if (reportResult.OverallResult != null)
            {
                html.Append(@"
                    <Worksheet ss:Name=""ОБЩ БРОЙ"">
                    <Table ss:StyleID=""N"">
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Row ss:Height=""20.25"">                        
                        <Cell ss:MergeAcross=""11"" ss:StyleID=""H""><Data ss:Type=""String"">АСУ на човешките ресурси</Data></Cell>
                    </Row>
                    <Row ss:Height=""25"">                        
                        <Cell ss:MergeAcross=""11"" ss:StyleID=""H2""><Data ss:Type=""String"">Отчет на ресурсите от резерва</Data></Cell>
                    </Row>
                    <Row ss:Height=""12.75"">
                        <Cell ss:MergeAcross=""11"" ss:StyleID=""H3""/>                       
                    </Row>
                    <Row ss:Height=""20.25"">                        
                        <Cell ss:MergeAcross=""11"" ss:StyleID=""H4""><Data ss:Type=""String"">Сведение - анализ за комплектуването на команда</Data></Cell>
                    </Row>                   
                    <Row ss:Height=""13.5"">
                        <Cell ss:MergeAcross=""11"" ss:StyleID=""H3""/>                       
                    </Row>
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""11"" ss:StyleID=""H6"" ><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Военно окръжие: <B>" + militaryDepartmentName + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""11"" ss:StyleID=""H6""><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Готовност: <B>" + militaryReadinessName + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""11"" ss:StyleID=""H6""><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Команда: <B>" + militaryCommandName + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""11"" ss:StyleID=""H6""><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Подкоманда: <B>" + militarySubCommand + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">
                        <Cell ss:MergeAcross=""11"" ss:StyleID=""H3""/>
                    </Row>
                    <Row ss:Height=""20.25"">                        
                        <Cell ss:MergeAcross=""11"" ss:StyleID=""H5""><Data ss:Type=""String"">Общ брой</Data></Cell>
                    </Row>
                    <Row ss:Height=""25.5"">
                        <Cell ss:MergeAcross=""3"" ss:StyleID=""H7""><Data ss:Type=""String"">По заявка</Data></Cell>
                        <Cell ss:MergeAcross=""3"" ss:StyleID=""H7""><Data ss:Type=""String"">Фактическо изпълнение</Data></Cell>
                        <Cell ss:MergeAcross=""3"" ss:StyleID=""H7""><Data ss:Type=""String"">Резерв</Data></Cell>                             
                    </Row>   
                    <Row ss:Height=""25.5"">
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Офицери</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Оф.к-ти</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Сержанти</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Войници</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Офицери</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Оф.к-ти</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Сержанти</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Войници</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Офицери</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Оф.к-ти</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Сержанти</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Войници</Data></Cell>                  
                    </Row>                  ");

                html.Append(@"
                        <Row ss:Height=""13.5"">
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + reportResult.OverallResult.RequestOfficers.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + reportResult.OverallResult.RequestOffCand.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + reportResult.OverallResult.RequestSergeants.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + reportResult.OverallResult.RequestSoldiers.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + reportResult.OverallResult.FulfiledOfficers.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + reportResult.OverallResult.FulfiledOffCand.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + reportResult.OverallResult.FulfiledSergeants.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + reportResult.OverallResult.FulfiledSoldiers.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + reportResult.OverallResult.ReserveOfficers.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + reportResult.OverallResult.ReserveOffCand.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + reportResult.OverallResult.ReserveSergeants.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + reportResult.OverallResult.ReserveSoldiers.ToString() + @"</Data></Cell>                  
                    </Row>");

                html.Append(@"</Table>
                        <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">
                            <DoNotDisplayGridlines/>
                        </WorksheetOptions></Worksheet>");
            }

            if (reportResult.MilRepSpecResult != null && reportResult.MilRepSpecResult.Count > 0)
            {
                html.Append(@"
                    <Worksheet ss:Name=""Анализ по ВОС"">
                    <Table ss:StyleID=""N"">
                    <Column ss:StyleID=""N"" ss:Width=""180"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Row ss:Height=""20.25"">                        
                        <Cell ss:MergeAcross=""16"" ss:StyleID=""H""><Data ss:Type=""String"">АСУ на човешките ресурси</Data></Cell>
                    </Row>
                    <Row ss:Height=""25"">                        
                        <Cell ss:MergeAcross=""16"" ss:StyleID=""H2""><Data ss:Type=""String"">Отчет на ресурсите от резерва</Data></Cell>
                    </Row>
                    <Row ss:Height=""12.75"">
                        <Cell ss:MergeAcross=""16"" ss:StyleID=""H3""/>                       
                    </Row>
                    <Row ss:Height=""20.25"">                        
                        <Cell ss:MergeAcross=""16"" ss:StyleID=""H4""><Data ss:Type=""String"">Сведение - анализ за комплектуването на команда</Data></Cell>
                    </Row>                   
                    <Row ss:Height=""13.5"">
                        <Cell ss:MergeAcross=""16"" ss:StyleID=""H3""/>                       
                    </Row>
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""16"" ss:StyleID=""H6"" ><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Военно окръжие: <B>" + militaryDepartmentName + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""16"" ss:StyleID=""H6""><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Готовност: <B>" + militaryReadinessName + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""16"" ss:StyleID=""H6""><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Команда: <B>" + militaryCommandName + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""16"" ss:StyleID=""H6""><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Подкоманда: <B>" + militarySubCommand + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">
                        <Cell ss:MergeAcross=""16"" ss:StyleID=""H3""/>
                    </Row>
                    <Row ss:Height=""20.25"">                        
                        <Cell ss:MergeAcross=""16"" ss:StyleID=""H5""><Data ss:Type=""String"">Анализ по ВОС</Data></Cell>
                    </Row>
                    <Row ss:Height=""25.5"">
                        <Cell ss:MergeDown=""2"" ss:StyleID=""H7""><Data ss:Type=""String"">ВОС</Data></Cell>
                        <Cell ss:MergeDown=""1"" ss:MergeAcross=""3"" ss:StyleID=""H7""><Data ss:Type=""String"">По заявка</Data></Cell>
                        <Cell ss:MergeAcross=""7"" ss:StyleID=""H7""><Data ss:Type=""String"">Фактическо изпълнение</Data></Cell>
                        <Cell ss:MergeDown=""1"" ss:MergeAcross=""3"" ss:StyleID=""H7""><Data ss:Type=""String"">Резерв</Data></Cell>                             
                    </Row>   
                    <Row ss:Height=""25.5"">
                        <Cell ss:Index=""6"" ss:MergeAcross=""3"" ss:StyleID=""H7""><Data ss:Type=""String""></Data></Cell>
                        <Cell ss:MergeAcross=""3"" ss:StyleID=""H7""><Data ss:Type=""String"">От тях по заменки</Data></Cell>
                    </Row> 
                    <Row ss:Height=""25.5"">
                        <Cell ss:Index=""2"" ss:StyleID=""H7""><Data ss:Type=""String"">Офицери</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Оф.к-ти</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Сержанти</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Войници</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Офицери</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Оф.к-ти</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Сержанти</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Войници</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Офицери</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Оф.к-ти</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Сержанти</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Войници</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Офицери</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Оф.к-ти</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Сержанти</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Войници</Data></Cell>                  
                    </Row>                  ");

                foreach (ReportMilRepSpecAnalyzeCommandBlock block in reportResult.MilRepSpecResult)
                {
                    html.Append(@"
                        <Row ss:AutoFitHeight=""1"">
                        <Cell ss:StyleID=""C2""><Data ss:Type=""String"">" + block.MilRepSpecName + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.RequestOfficers.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.RequestOffCand.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.RequestSergeants.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.RequestSoldiers.ToString() + @"</Data></Cell>                     
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledOfficers.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledOffCand.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledSergeants.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledSoldiers.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.ChangesOfficers.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.ChangesOffCand.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.ChangesSergeants.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.ChangesSoldiers.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.ReserveOfficers.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.ReserveOffCand.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.ReserveSergeants.ToString() + @"</Data></Cell> 
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.ReserveSoldiers.ToString() + @"</Data></Cell>                 
                    </Row>");
                }

                html.Append(@"</Table>
                        <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">
                            <DoNotDisplayGridlines/>
                        </WorksheetOptions></Worksheet>");
            }

            if (reportResult.PositionMRSResult != null && reportResult.PositionMRSResult.Count > 0)
            {
                int maxColumns = 0;
                int allExcelColumns = 0;

                foreach (ReportPositionMRSAnalyzePositionBlock block in reportResult.PositionMRSResult)
                {
                    if (maxColumns < block.MRS.Count)
                        maxColumns = block.MRS.Count;
                }

                allExcelColumns = 2 + 2 * maxColumns;

                html.Append(@"
                    <Worksheet ss:Name=""Анализ длъжности по ВОС"">
                    <Table ss:StyleID=""N"">
                    <Column ss:StyleID=""N"" ss:Width=""180"" />
                 ");

                for (int i = 0; i < maxColumns; i++)
                {
                    html.Append(@" 
                                <Column ss:StyleID=""N"" ss:Width=""120"" />
                                <Column ss:StyleID=""N"" ss:Width=""60"" />
                             ");
                }

                html.Append(@"
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Row ss:Height=""20.25"">                        
                        <Cell ss:MergeAcross=""" + allExcelColumns + @""" ss:StyleID=""H""><Data ss:Type=""String"">АСУ на човешките ресурси</Data></Cell>
                    </Row>
                    <Row ss:Height=""25"">                        
                        <Cell ss:MergeAcross=""" + allExcelColumns + @""" ss:StyleID=""H2""><Data ss:Type=""String"">Отчет на ресурсите от резерва</Data></Cell>
                    </Row>
                    <Row ss:Height=""12.75"">
                        <Cell ss:MergeAcross=""" + allExcelColumns + @""" ss:StyleID=""H3""/>                       
                    </Row>
                    <Row ss:Height=""20.25"">                        
                        <Cell ss:MergeAcross=""" + allExcelColumns + @""" ss:StyleID=""H4""><Data ss:Type=""String"">Сведение - анализ за комплектуването на команда</Data></Cell>
                    </Row>                   
                    <Row ss:Height=""13.5"">
                        <Cell ss:MergeAcross=""" + allExcelColumns + @""" ss:StyleID=""H3""/>                       
                    </Row>
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""" + allExcelColumns + @""" ss:StyleID=""H6"" ><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Военно окръжие: <B>" + militaryDepartmentName + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""" + allExcelColumns + @""" ss:StyleID=""H6""><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Готовност: <B>" + militaryReadinessName + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""" + allExcelColumns + @""" ss:StyleID=""H6""><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Команда: <B>" + militaryCommandName + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""" + allExcelColumns + @""" ss:StyleID=""H6""><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Подкоманда: <B>" + militarySubCommand + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">
                        <Cell ss:MergeAcross=""" + allExcelColumns + @""" ss:StyleID=""H3""/>
                    </Row>
                    <Row ss:Height=""20.25"">                        
                        <Cell ss:MergeAcross=""" + allExcelColumns + @""" ss:StyleID=""H5""><Data ss:Type=""String"">Анализ длъжности по ВОС</Data></Cell>
                    </Row>                   
                    <Row ss:Height=""25.5"">
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Длъжност</Data></Cell>
                    ");

                for (int i = 0; i < maxColumns; i++)
                {
                    html.Append(@" 
                                <Cell ss:StyleID=""H7""><Data ss:Type=""String"">ВОС</Data></Cell>
                                <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Бр</Data></Cell>
                             ");
                }


                html.Append(@"<Cell ss:StyleID=""H7""><Data ss:Type=""String"">Всичко</Data></Cell>                        
                    </Row>                  ");

                foreach (ReportPositionMRSAnalyzePositionBlock block in reportResult.PositionMRSResult)
                {
                    html.Append(@"
                        <Row ss:AutoFitHeight=""1"">
                        <Cell ss:StyleID=""C2""><Data ss:Type=""String"">" + block.Position + @"</Data></Cell>
                        ");

                    int totalCell = 0;
                    foreach (ReportPositionMRSAnalyzeMRSBlock cell in block.MRS)
                    {
                        html.Append(@"<Cell ss:StyleID=""C2""><Data ss:Type=""String"">" + cell.MilitaryReportSpecilityName + @"</Data></Cell>");
                        html.Append(@"<Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + cell.Cnt.ToString() + @"</Data></Cell>");

                        totalCell += cell.Cnt;
                    }

                    for (int i = 0; i < maxColumns - block.MRS.Count; i++)
                    {
                        html.Append(@"<Cell ss:StyleID=""C1""><Data ss:Type=""Number""></Data></Cell>");
                        html.Append(@"<Cell ss:StyleID=""C1""><Data ss:Type=""Number""></Data></Cell>");
                    }

                    html.Append(@"<Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + totalCell.ToString() + @"</Data></Cell>");

                    html.Append("</Row>");                   
                }

                html.Append(@"</Table>
                        <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">
                            <DoNotDisplayGridlines/>
                        </WorksheetOptions></Worksheet>");
            }

            if (reportResult.PositionDeliveryResult != null && reportResult.PositionDeliveryResult.Count > 0)
            {
                html.Append(@"
                    <Worksheet ss:Name=""Анализ по доставяне"">
                    <Table ss:StyleID=""N"">
                    <Column ss:StyleID=""N"" ss:Width=""180"" />
                    <Column ss:StyleID=""N"" ss:Width=""180"" />
                    <Column ss:StyleID=""N"" ss:Width=""70"" ss:Span=""7""/>
                    
                    <Row ss:Height=""20.25"">                        
                        <Cell ss:MergeAcross=""9"" ss:StyleID=""H""><Data ss:Type=""String"">АСУ на човешките ресурси</Data></Cell>
                    </Row>
                    <Row ss:Height=""25"">                        
                        <Cell ss:MergeAcross=""9"" ss:StyleID=""H2""><Data ss:Type=""String"">Отчет на ресурсите от резерва</Data></Cell>
                    </Row>
                    <Row ss:Height=""12.75"">
                        <Cell ss:MergeAcross=""9"" ss:StyleID=""H3""/>                       
                    </Row>
                    <Row ss:Height=""20.25"">                        
                        <Cell ss:MergeAcross=""9"" ss:StyleID=""H4""><Data ss:Type=""String"">Сведение - анализ за комплектуването на команда</Data></Cell>
                    </Row>                   
                    <Row ss:Height=""13.5"">
                        <Cell ss:MergeAcross=""9"" ss:StyleID=""H3""/>                       
                    </Row>
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""9"" ss:StyleID=""H6"" ><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Военно окръжие: <B>" + militaryDepartmentName + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""9"" ss:StyleID=""H6""><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Готовност: <B>" + militaryReadinessName + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""9"" ss:StyleID=""H6""><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Команда: <B>" + militaryCommandName + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""9"" ss:StyleID=""H6""><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Подкоманда: <B>" + militarySubCommand + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">
                        <Cell ss:MergeAcross=""9"" ss:StyleID=""H3""/>
                    </Row>
                    <Row ss:Height=""20.25"">                        
                        <Cell ss:MergeAcross=""9"" ss:StyleID=""H5""><Data ss:Type=""String"">Анализ по доставяне</Data></Cell>
                    </Row>                
                    <Row ss:Height=""25.5"">                      
                        <Cell ss:StyleID=""H7"" ss:MergeDown=""1""><Data ss:Type=""String"">Община</Data></Cell>
                        <Cell ss:StyleID=""H7"" ss:MergeDown=""1""><Data ss:Type=""String"">Район</Data></Cell>
                        <Cell ss:MergeAcross=""3"" ss:StyleID=""H7""><Data ss:Type=""String"">Основно попълнение</Data></Cell>
                        <Cell ss:MergeAcross=""3"" ss:StyleID=""H7""><Data ss:Type=""String"">Резерв</Data></Cell>                  
                    </Row>   
                    <Row ss:Height=""25.5"">                      
                       
                        <Cell ss:Index=""3"" ss:StyleID=""H7""><Data ss:Type=""String"">Офицери</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Оф.к-ти</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Сержанти</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Войници</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Офицери</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Оф.к-ти</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Сержанти</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Войници</Data></Cell>  
                    </Row>   

");

                foreach (ReportPositionDeliveryAnalyzeBlock block in reportResult.PositionDeliveryResult)
                {
                    html.Append(@"
                        <Row ss:AutoFitHeight=""1"">
                            <Cell ss:StyleID=""C2""><Data ss:Type=""String"">" + block.MuniciplaityName + @"</Data></Cell>
                            <Cell ss:StyleID=""C2""><Data ss:Type=""String"">" + block.DistrictName + @"</Data></Cell> 
                            <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledOfficers.ToString() + @"</Data></Cell>
                            <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledOffCand.ToString() + @"</Data></Cell>
                            <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledSergeants.ToString() + @"</Data></Cell>
                            <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledSoldiers.ToString() + @"</Data></Cell>
                            <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.ReserveOfficers.ToString() + @"</Data></Cell>
                            <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.ReserveOffCand.ToString() + @"</Data></Cell>
                            <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.ReserveSergeants.ToString() + @"</Data></Cell>
                            <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.ReserveSoldiers.ToString() + @"</Data></Cell>
                        </Row>");
                }

                html.Append(@"</Table>
                        <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">
                            <DoNotDisplayGridlines/>
                        </WorksheetOptions></Worksheet>");
            }

            if (reportResult.PositionFulfilResult != null && reportResult.PositionFulfilResult.Count > 0)
            {
                html.Append(@"
                    <Worksheet ss:Name=""Анализ по длъжности"">
                    <Table ss:StyleID=""N"">
                    <Column ss:StyleID=""N"" ss:Width=""120"" />
                    <Column ss:StyleID=""N"" ss:Width=""80"" />
                    <Column ss:StyleID=""N"" ss:Width=""80"" />                 
                    <Row ss:Height=""20.25"">                        
                        <Cell ss:MergeAcross=""12"" ss:StyleID=""H""><Data ss:Type=""String"">АСУ на човешките ресурси</Data></Cell>
                    </Row>
                    <Row ss:Height=""25"">                        
                        <Cell ss:MergeAcross=""12"" ss:StyleID=""H2""><Data ss:Type=""String"">Отчет на ресурсите от резерва</Data></Cell>
                    </Row>
                    <Row ss:Height=""12.75"">
                        <Cell ss:MergeAcross=""12"" ss:StyleID=""H3""/>                       
                    </Row>
                    <Row ss:Height=""20.25"">                        
                        <Cell ss:MergeAcross=""12"" ss:StyleID=""H4""><Data ss:Type=""String"">Сведение - анализ за комплектуването на команда</Data></Cell>
                    </Row>                   
                    <Row ss:Height=""13.5"">
                        <Cell ss:MergeAcross=""12"" ss:StyleID=""H3""/>                       
                    </Row>
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""12"" ss:StyleID=""H6"" ><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Военно окръжие: <B>" + militaryDepartmentName + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""12"" ss:StyleID=""H6""><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Готовност: <B>" + militaryReadinessName + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""12"" ss:StyleID=""H6""><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Команда: <B>" + militaryCommandName + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""12"" ss:StyleID=""H6""><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Подкоманда: <B>" + militarySubCommand + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">
                        <Cell ss:MergeAcross=""12"" ss:StyleID=""H3""/>
                    </Row>
                    <Row ss:Height=""20.25"">                        
                        <Cell ss:MergeAcross=""12"" ss:StyleID=""H5""><Data ss:Type=""String"">Анализ по длъжности на основно попълнение</Data></Cell>
                    </Row>                
                    <Row ss:Height=""25.5"">
                        <Cell ss:MergeDown=""2"" ss:StyleID=""H7""><Data ss:Type=""String"">Длъжност</Data></Cell>
                        <Cell ss:MergeAcross=""6"" ss:StyleID=""H7""><Data ss:Type=""String"">Образование</Data></Cell>
                        <Cell ss:MergeAcross=""2"" ss:StyleID=""H7""><Data ss:Type=""String"">Възраст</Data></Cell>
                        <Cell ss:MergeDown=""2"" ss:StyleID=""H7""><Data ss:Type=""String"">С военна подготовка</Data></Cell>
                        <Cell ss:MergeDown=""2"" ss:StyleID=""H7""><Data ss:Type=""String"">Нуждаят се от курс</Data></Cell>
                    </Row>               
                    <Row ss:Height=""25.5"">
                        <Cell ss:Index=""2"" ss:MergeAcross=""2"" ss:StyleID=""H7""><Data ss:Type=""String"">общо</Data></Cell>
                        <Cell ss:MergeAcross=""3"" ss:StyleID=""H7""><Data ss:Type=""String"">военно</Data></Cell>
                        <Cell ss:MergeDown=""1"" ss:StyleID=""H7""><Data ss:Type=""String"">до 35 г.</Data></Cell>
                        <Cell ss:MergeDown=""1"" ss:StyleID=""H7""><Data ss:Type=""String"">до 45 г.</Data></Cell>
                        <Cell ss:MergeDown=""1"" ss:StyleID=""H7""><Data ss:Type=""String"">над 45 г.</Data></Cell>
                    </Row>               
                    <Row ss:Height=""25.5"">
                        <Cell ss:Index=""2"" ss:StyleID=""H7""><Data ss:Type=""String"">висше</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">полувсише</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">средно</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">ВА</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">ВУ</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">ШЗО</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">без военно обр.</Data></Cell>
                    </Row>                    ");

                foreach (ReportPositionFulfilAnalyzeBlock block in reportResult.PositionFulfilResult)
                {
                    html.Append(@"
                        <Row ss:AutoFitHeight=""1"">
                        <Cell ss:StyleID=""C2""><Data ss:Type=""String"">" + block.Position + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.VisheEducation.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.PoluvisheEducation.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.SrednoEducation.ToString() + @"</Data></Cell>                     
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.VAEducation.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.VUEducation.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.SZHOEducation.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.NoMilEducation.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.AgeUnder35.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.AgeUnder45.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.AgeAbove45.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.MilitaryTraining.ToString() + @"</Data></Cell> 
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.NeedCourse.ToString() + @"</Data></Cell>                 
                    </Row>");
                }

                html.Append(@"</Table>
                        <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">
                            <DoNotDisplayGridlines/>
                        </WorksheetOptions></Worksheet>");
            }

            if (reportResult.MilRepSpecAndPositionResult != null && reportResult.MilRepSpecAndPositionResult.Count > 0)
            {
                html.Append(@"
                    <Worksheet ss:Name=""Анализ по ВОС и длъжност"">
                    <Table ss:StyleID=""N"">
                    <Column ss:StyleID=""N"" ss:Width=""180"" />
                    <Column ss:StyleID=""N"" ss:Width=""180"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Row ss:Height=""20.25"">                        
                        <Cell ss:MergeAcross=""17"" ss:StyleID=""H""><Data ss:Type=""String"">АСУ на човешките ресурси</Data></Cell>
                    </Row>
                    <Row ss:Height=""25"">                        
                        <Cell ss:MergeAcross=""17"" ss:StyleID=""H2""><Data ss:Type=""String"">Отчет на ресурсите от резерва</Data></Cell>
                    </Row>
                    <Row ss:Height=""12.75"">
                        <Cell ss:MergeAcross=""17"" ss:StyleID=""H3""/>                       
                    </Row>
                    <Row ss:Height=""20.25"">                        
                        <Cell ss:MergeAcross=""17"" ss:StyleID=""H4""><Data ss:Type=""String"">Сведение - анализ за комплектуването на команда</Data></Cell>
                    </Row>                   
                    <Row ss:Height=""13.5"">
                        <Cell ss:MergeAcross=""17"" ss:StyleID=""H3""/>                       
                    </Row>
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""17"" ss:StyleID=""H6"" ><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Военно окръжие: <B>" + militaryDepartmentName + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""17"" ss:StyleID=""H6""><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Готовност: <B>" + militaryReadinessName + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""17"" ss:StyleID=""H6""><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Команда: <B>" + militaryCommandName + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""17"" ss:StyleID=""H6""><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Подкоманда: <B>" + militarySubCommand + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">
                        <Cell ss:MergeAcross=""17"" ss:StyleID=""H3""/>
                    </Row>
                    <Row ss:Height=""20.25"">                        
                        <Cell ss:MergeAcross=""17"" ss:StyleID=""H5""><Data ss:Type=""String"">Анализ по ВОС и длъжност</Data></Cell>
                    </Row>
                    <Row ss:Height=""25.5"">
                        <Cell ss:MergeDown=""2"" ss:StyleID=""H7""><Data ss:Type=""String"">Длъжност</Data></Cell>
                        <Cell ss:MergeDown=""2"" ss:StyleID=""H7""><Data ss:Type=""String"">ВОС</Data></Cell>
                        <Cell ss:MergeDown=""1"" ss:MergeAcross=""3"" ss:StyleID=""H7""><Data ss:Type=""String"">Заявка на формированието</Data></Cell>
                        <Cell ss:MergeAcross=""7"" ss:StyleID=""H7""><Data ss:Type=""String"">Фактическо изпълнение</Data></Cell>
                        <Cell ss:MergeDown=""1"" ss:MergeAcross=""3"" ss:StyleID=""H7""><Data ss:Type=""String"">Заявка на формированието</Data></Cell>                             
                    </Row>   
                    <Row ss:Height=""25.5"">
                        <Cell ss:Index=""7"" ss:MergeAcross=""3"" ss:StyleID=""H7""><Data ss:Type=""String""></Data></Cell>
                        <Cell ss:MergeAcross=""3"" ss:StyleID=""H7""><Data ss:Type=""String"">От тях по заменка</Data></Cell>
                    </Row> 
                    <Row ss:Height=""25.5"">
                        <Cell ss:Index=""3"" ss:StyleID=""H7""><Data ss:Type=""String"">Офицери</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Оф. к-ти</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Сержанти</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Войници</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Офицери</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Оф. к-ти</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Сержанти</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Войници</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Офицери</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Оф. к-ти</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Сержанти</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Войници</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Офицери</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Оф. к-ти</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Сержанти</Data></Cell>
                        <Cell ss:StyleID=""H7""><Data ss:Type=""String"">Войници</Data></Cell>                  
                    </Row>                  ");

                string prevPosition = "";
                foreach (ReportMilRepSpecAndPositionAnalyzeCommandBlock block in reportResult.MilRepSpecAndPositionResult)
                {
                    int rowsPerPosition = reportResult.MilRepSpecAndPositionResult.Where(x => x.Position == block.Position).Count();

                    html.Append(@"
                    <Row ss:AutoFitHeight=""1"">
                        " + (prevPosition != block.Position ?
                      @"<Cell ss:StyleID=""C2"" " + (rowsPerPosition > 1 ? @"ss:MergeDown=""" + ((int)(rowsPerPosition - 1)).ToString() + @"""" : "") + @"><Data ss:Type=""String"">" + block.Position + @"</Data></Cell>" : "") + @"
                        <Cell ss:StyleID=""" + (block.IsPrimary ? "C2_B" : "C2") + @""" " + (rowsPerPosition > 1 ? @"ss:Index=""2""" : "") + @"><Data ss:Type=""String"">" + block.MilRepSpecName + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.RequestOfficers.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.RequestOffCand.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.RequestSergeants.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.RequestSoldiers.ToString() + @"</Data></Cell>                     
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledOfficers.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledOffCand.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledSergeants.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.FulfiledSoldiers.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.ChangesOfficers.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.ChangesOffCand.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.ChangesSergeants.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.ChangesSoldiers.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.ReserveOfficers.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.ReserveOffCand.ToString() + @"</Data></Cell>
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.ReserveSergeants.ToString() + @"</Data></Cell> 
                        <Cell ss:StyleID=""C1""><Data ss:Type=""Number"">" + block.ReserveSoldiers.ToString() + @"</Data></Cell>                 
                    </Row>");

                    prevPosition = block.Position;
                }

                html.Append(@"</Table>
                        <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">
                            <DoNotDisplayGridlines/>
                        </WorksheetOptions></Worksheet>");
            }

            if (reportResult.AgeMRSResult != null && reportResult.AgeMRSResult.Count > 0)
            {
                int maxColumns = 0;

                if (maxColumns < reportResult.AgeMRSResult.Count)
                {
                    maxColumns = reportResult.AgeMRSResult.Count;
                }

                html.Append(@"
                    <Worksheet ss:Name=""Анализ по ВОС и възраст"">
                    <Table ss:StyleID=""N"">
                    <Column ss:StyleID=""N"" ss:Width=""300"" />");

                for (int i = 0; i < maxColumns; i++)
                {
                    html.Append(@"<Column ss:StyleID=""N"" ss:Width=""60"" />");
                }

                html.Append(@"
                    <Column ss:StyleID=""N"" ss:Width=""60"" />
                    <Row ss:Height=""20.25"">                        
                        <Cell ss:MergeAcross=""" + maxColumns + @""" ss:StyleID=""H""><Data ss:Type=""String"">АСУ на човешките ресурси</Data></Cell>
                    </Row>
                    <Row ss:Height=""25"">                        
                        <Cell ss:MergeAcross=""" + maxColumns + @""" ss:StyleID=""H2""><Data ss:Type=""String"">Отчет на ресурсите от резерва</Data></Cell>
                    </Row>
                    <Row ss:Height=""12.75"">
                        <Cell ss:MergeAcross=""" + maxColumns + @""" ss:StyleID=""H3""/>                       
                    </Row>
                    <Row ss:Height=""20.25"">                        
                        <Cell ss:MergeAcross=""" + maxColumns + @""" ss:StyleID=""H4""><Data ss:Type=""String"">Сведение - анализ за комплектуването на команда</Data></Cell>
                    </Row>                   
                    <Row ss:Height=""13.5"">
                        <Cell ss:MergeAcross=""" + maxColumns + @""" ss:StyleID=""H3""/>                       
                    </Row>
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""" + maxColumns + @""" ss:StyleID=""H6"" ><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Военно окръжие: <B>" + militaryDepartmentName + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""" + maxColumns + @""" ss:StyleID=""H6""><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Готовност: <B>" + militaryReadinessName + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""" + maxColumns + @""" ss:StyleID=""H6""><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Команда: <B>" + militaryCommandName + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">                        
                        <Cell ss:MergeAcross=""" + maxColumns + @""" ss:StyleID=""H6""><ss:Data ss:Type=""String"" xmlns=""http://www.w3.org/TR/REC-html40"">Подкоманда: <B>" + militarySubCommand + @"</B></Data></Cell>
                    </Row> 
                    <Row ss:Height=""13.5"">
                        <Cell ss:MergeAcross=""" + maxColumns + @""" ss:StyleID=""H3""/>
                    </Row>
                    <Row ss:Height=""20.25"">                        
                        <Cell ss:MergeAcross=""" + maxColumns + @""" ss:StyleID=""H5""><Data ss:Type=""String"">Анализ на основното попълниение по ВОС и възраст</Data></Cell>
                    </Row>                   
                    <Row>
						<Cell />
                        <Cell ss:StyleID=""H7"" ss:MergeAcross = ""2"">
                            <Data ss:Type=""String"">Възраст</Data>
                        </Cell>
                    </Row>
					<Row>
					    <Cell ss:StyleID=""H7"">
                            <Data ss:Type=""String"">ВОС</Data>
                        </Cell>");

                for (int i = 0; i < maxColumns; i++)
                {
                    html.Append(@" 
                                <Cell ss:StyleID=""H7""><Data ss:Type=""String"">" + reportResult.AgeMRSResult[i].Age + @"</Data></Cell>
                             ");
                }


                html.Append(@"</Row>");

                int indexMRS = 0;
                int totalFirstCol = 0;
                int totalSecondCol = 0;
                int totalThirdCol = 0;
                foreach (ReportAgeMRSAnalyzeMRSBlock block in reportResult.AgeMRSResult[indexMRS].MRS)
                {
                    html.Append(@"
                        <Row ss:AutoFitHeight=""1"">
                        <Cell ss:StyleID=""C2_B""><Data ss:Type=""String"">" + block.MilitaryReportSpecilityName + @"</Data></Cell>
                        ");

                    int countCell = 0;
                    foreach (ReportAgeMRSAnalyzeBlock cell in reportResult.AgeMRSResult)
                    {
                        html.Append(@"<Cell ss:StyleID=""C2""><Data ss:Type=""Number"">" + cell.MRS[indexMRS].Cnt + @"</Data></Cell>");

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

                    html.Append("</Row>");

                    indexMRS++;
                }
                html.Append(@"<Row>
                                <Cell ss:StyleID=""C2_B"">
                                    <Data ss:Type=""String"">Всичко</Data>
                                </Cell>
                                <Cell ss:StyleID=""C2"">
                                    <Data ss:Type=""Number"">" + totalFirstCol + @"</Data>
                                </Cell>
                                <Cell ss:StyleID=""C2"">
                                    <Data ss:Type=""Number"">" + totalSecondCol + @"</Data>
                                </Cell>
                                <Cell ss:StyleID=""C2"">
                                    <Data ss:Type=""Number"">" + totalThirdCol + @"</Data>
                                </Cell>
                              </Row>");
                html.Append(@"</Table>
                        <WorksheetOptions xmlns=""urn:schemas-microsoft-com:office:excel"">
                            <DoNotDisplayGridlines/>
                        </WorksheetOptions></Worksheet>");
            }

            html.Append("</Workbook>");

            return html.ToString();
        }
    }
}
