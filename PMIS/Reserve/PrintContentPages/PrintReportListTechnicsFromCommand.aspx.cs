using System;
using System.Linq;
using System.Text;

using PMIS.Common;
using PMIS.Reserve.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace PMIS.Reserve.PrintContentPages
{
    public partial class PrintReportListTechnicsFromCommand : RESPage
    {
        const string All = "Всички";

        string militaryDepartmentId = "";
        string militaryCommandId = "";

        int sortBy = 1; // 1 - Default

        public void SetPrintTitleLinesWidth()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnPrintTitleLinesWidth");

            hf.Value = "915";
        }

        public void SetHeadersLeft()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnHeadersLeft");

            hf.Value = "60";
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
                this.GetUIItemAccessLevel("RES_REPORTS_REPORTLISTTECHNICSFROMCOMMAND") != UIAccessLevel.Hidden)
            {
                if (!String.IsNullOrEmpty(Request.Params["MilitaryDepartmentId"]))
                {
                    militaryDepartmentId = Request.Params["MilitaryDepartmentId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["MilitaryCommandId"]))
                {
                    militaryCommandId = Request.Params["MilitaryCommandId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["SortBy"]))
                {
                    int.TryParse(Request.Params["SortBy"], out sortBy);
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
                sb.Append("<td style=\"vertical-align: top;\">" + CommonFunctions.GenerateButtons(true, 180, true) + "</td>");
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
            ReportListTechnicsFromCommandFilter filter = new ReportListTechnicsFromCommandFilter()
            {
                MilitaryDepartments = militaryDepartmentId,
                MilitaryCommands = militaryCommandId,
                OrderBy = sortBy,
                PageIdx = 0,
                RowsPerPage = 0
            };

            //Get the list of records according to the specified filters and order
            List<ReportListTechnicsFromCommandBlock> reportListTechnicsFromCommandBlocks = ReportListTechnicsFromCommandBlockUtil.GetReportListTechnicsFromCommandBlockList(filter, CurrentUser);

            MilitaryDepartment militaryDepartment = null;
            int militDepId = 0;
            if (int.TryParse(militaryDepartmentId, out militDepId))
            {
                militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment(militDepId, CurrentUser);
            }

            MilitaryCommand militaryCommand = null;
            int militCommandId = 0;
            if (int.TryParse(militaryCommandId, out militCommandId))
            {
                militaryCommand = MilitaryCommandUtil.GetMilitaryCommand(militCommandId, CurrentUser);
            }

            StringBuilder html = new StringBuilder();
            html.Append(@"<table style='padding: 5px;'>
                             <tr>
                                <td align='right' style='width: 230px;'>
                                    <span class='Label'>ВО:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 245px;'>
                                    <span class='ValueLabel'>" + (militaryDepartment != null ? militaryDepartment.MilitaryDepartmentName : All) + @"</span>
                                </td>
                                <td align='right' style='width: 130px;'>
                                    <span class='Label'>Команда:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 275px;'>
                                    <span class='ValueLabel'>" + (militaryCommand != null ? militaryCommand.DisplayTextForSelection : All) + @"</span>
                                </td>
                             </tr>");

            if (reportListTechnicsFromCommandBlocks.Count() > 0)
            {
                html.Append(@"
                    <tr><td colspan='4' align='center'>
                    <table id='applicantsTable' name='applicantsTable' class='CommonHeaderTable'>
                        <thead>
                            <tr>
                                <th style='width: 15px; border-left: 1px solid #000000;'>№</th>
                                <th style='width: 80px;'>Подкоманда / Заявка №</th>
                                <th style='width: 130px;'>Вид техника</th>
                                <th style='width: 150px;'>Нормативна категория</th>
                                <th style='width: 110px;'>Начин на явяване</th>
                                <th style='width: 120px;'>Рег./Инв. номер</th>
                                <th style='width: 120px;'>Собственик</th>
                                <th style='width: 120px; border-right: 1px solid #000000;'>Адрес</th>
                            </tr>
                        </thead><tbody>");
            }

            int counter = 1;

            foreach (ReportListTechnicsFromCommandBlock reportBlock in reportListTechnicsFromCommandBlocks)
            {
                html.Append(@"<tr>
                            <td align='center'>" + counter + @"</td>
                            <td style='text-align: left;'>" + reportBlock.MilitarySubCommand + @"</td>
                            <td style='text-align: left;'>" + reportBlock.TechnicsType + @"</td>
                            <td style='text-align: left;'>" + reportBlock.NormativeTechnics + @"</td>
                            <td style='text-align: left;'>" + reportBlock.Readiness + @"</td>
                            <td style='text-align: left;'>" + reportBlock.RegInvNumber + @"</td>
                            <td style='text-align: left;'>" + reportBlock.Owner + @"</td>
                            <td style='text-align: left;'>" + reportBlock.OwnerAddress + @"</td>
                          </tr>");
                counter++;
            }

            if (reportListTechnicsFromCommandBlocks.Count() > 0)
            {
                html.Append("</tbody></table></td></tr>");
            }

            html.Append("</table>");

            return html.ToString();
        }

        // Handles export to excel button click
        protected void btnGenerateExcel_Click(object sender, EventArgs e)
        {
            string result = this.GeneratePageContent(true);
            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment; filename=ReportListTechnicsFromCommand.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        // Generate html as excel response result
        private string GenerateAllRecordsForExport()
        {
            int visibleColumnsCount = 8;

            ReportListTechnicsFromCommandFilter filter = new ReportListTechnicsFromCommandFilter()
            {
                MilitaryDepartments = militaryDepartmentId,
                MilitaryCommands = militaryCommandId,
                OrderBy = sortBy,
                PageIdx = 0,
                RowsPerPage = 0
            };

            //Get the list of records according to the specified filters and order
            List<ReportListTechnicsFromCommandBlock> reportListTechnicsFromCommandBlocks = ReportListTechnicsFromCommandBlockUtil.GetReportListTechnicsFromCommandBlockList(filter, CurrentUser);

            MilitaryDepartment militaryDepartment = null;
            int militDepId = 0;
            if (int.TryParse(militaryDepartmentId, out militDepId))
            {
                militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment(militDepId, CurrentUser);
            }

            MilitaryCommand militaryCommand = null;
            int militCommandId = 0;
            if (int.TryParse(militaryCommandId, out militCommandId))
            {
                militaryCommand = MilitaryCommandUtil.GetMilitaryCommand(militCommandId, CurrentUser);
            }

            StringBuilder html = new StringBuilder();
            html.Append(@"<html xmlns='http://www.w3.org/1999/xhtml' xml:lang='en'>
                            <head>
                                <meta http-equiv='content-type' content='application/xhtml+xml; charset=UTF-8' />
                            </head>
                            <body>
                                <table>
                                    <tr><td align='center' colspan='" + visibleColumnsCount + @"' style='font-weight: bold; font-size: 1.6em;'>АСУ на човешките ресурси</td></tr>
                                    <tr><td align='center' colspan='" + visibleColumnsCount + @"' style='font-weight: bold; font-size: 2em;'>Резервисти</td></tr>
                                    <tr><td colspan='" + visibleColumnsCount + @"'>&nbsp;</td></tr>
                                    <tr><td align='center' colspan='" + visibleColumnsCount + @"' style='font-weight: bold; font-size: 1.3em;'>Списък на техниката с МН от команда</td></tr>
                                    <tr><td colspan='" + visibleColumnsCount + @"'>&nbsp;</td></tr>
                                    <tr>
                                        <td align='center' colspan='" + visibleColumnsCount + @"'>
                                            <span style='font-weight: normal;'>ВО:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + (militaryDepartment != null ? militaryDepartment.MilitaryDepartmentName : All) + @"</span>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <span style='font-weight: normal;'>Команда:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + (militaryCommand != null ? militaryCommand.DisplayTextForSelection : All) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan='" + visibleColumnsCount + @"'>&nbsp;</td>
                                    </tr>
                                </table>");


            if (reportListTechnicsFromCommandBlocks.Count() > 0)
            {
                html.Append(@"
                    <table>
                        <thead>
                            <tr>
                                <th style='width: 30px; border-bottom: 1px solid white; background-color: black; color: #FFFFFF;'>№</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Подкоманда / Заявка №</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Вид техника</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Нормативна категория</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Начин на явяване</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Рег./Инв. номер</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Собственик</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Адрес</th>
                            </tr>
                        </thead><tbody>");

                int counter = 1;

                foreach (ReportListTechnicsFromCommandBlock reportBlock in reportListTechnicsFromCommandBlocks)
                {
                    html.Append(@"
                            <tr>
                                <td align='center' style='border: 1px solid black;'>" + counter + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + reportBlock.MilitarySubCommand + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + reportBlock.TechnicsType + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + reportBlock.NormativeTechnics + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + reportBlock.Readiness + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + reportBlock.RegInvNumber + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + reportBlock.Owner + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + reportBlock.OwnerAddress + @"</td>
                            </tr>");
                    counter++;
                }

                html.Append("</tbody></table>");
            }

            html.Append("</body></html>");

            return html.ToString();
        }
    }
}
