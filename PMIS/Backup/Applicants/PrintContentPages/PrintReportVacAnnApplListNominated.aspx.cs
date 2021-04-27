using System;
using System.Linq;
using System.Text;

using PMIS.Common;
using PMIS.Applicants.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace PMIS.Applicants.PrintContentPages
{
    public partial class PrintReportVacAnnApplListNominated : APPLPage
    {
        const string All = "Всички";

        int vacancyAnnounceId = 0;
        int responsibleMilitaryUnitId = 0;
        int sortBy = 1; // 1 - Default

        UIAccessLevel l;

        int visibleColumnsCount = 1;

        bool isApplNameVisible = false;
        bool isIdentNumbVisible = false;
        bool isMilitUnitVisible = false;
        bool isPositionVisible = false;
        bool isPositionCodeVisible = false;
        bool isPositionAccessLevelVisible = false;
        bool isTotalPointsVisible = false;
        bool isStatusVisible = false;

        private string MilitaryUnitLabel = CommonFunctions.GetLabelText("MilitaryUnit");

        public void SetPrintTitleLinesWidth()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnPrintTitleLinesWidth");

            hf.Value = "855";
        }

        public void SetHeadersLeft()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnHeadersLeft");

            hf.Value = "120";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.SetPrintTitleLinesWidth();
                this.SetHeadersLeft();
            }

            // Check visibility right for the print screen
            if (this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_NOMINATION") != UIAccessLevel.Hidden)
            {
                if (!String.IsNullOrEmpty(Request.Params["VacancyAnnounceID"]))
                {
                    int.TryParse(Request.Params["VacancyAnnounceID"], out vacancyAnnounceId);
                }

                if (!String.IsNullOrEmpty(Request.Params["ResponsibleMilitaryUnitID"]))
                {
                    int.TryParse(Request.Params["ResponsibleMilitaryUnitID"], out responsibleMilitaryUnitId);
                }

                if (!String.IsNullOrEmpty(Request.Params["SortBy"]))
                {
                    int.TryParse(Request.Params["SortBy"], out sortBy);
                }

                if (vacancyAnnounceId > 0 && responsibleMilitaryUnitId > 0)
                {
                    this.SetUIResultVisibility();

                    this.divResults.InnerHtml = GeneratePageContent(false);
                }
                else
                {
                    this.divResults.InnerHtml = "";
                }
            }
            else
            {
                this.divResults.InnerHtml = "";
            }
        }

        private void SetUIResultVisibility()
        {
            l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_NOMINATION_APPLICANT");
            if (l != UIAccessLevel.Hidden)
            {
                isApplNameVisible = true;
                visibleColumnsCount++;
            }

            l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_NOMINATION_IDENTNUM");
            if (l != UIAccessLevel.Hidden)
            {
                isIdentNumbVisible = true;
                visibleColumnsCount++;
            }

            l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_NOMINATION_MILITARYUNIT");
            if (l != UIAccessLevel.Hidden)
            {
                isMilitUnitVisible = true;
                visibleColumnsCount++;
            }

            l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_NOMINATION_POSITION");
            if (l != UIAccessLevel.Hidden)
            {
                isPositionVisible = true;
                visibleColumnsCount++;
            }

            l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_NOMINATION_POSITION_CODE");
            if (l != UIAccessLevel.Hidden)
            {
                isPositionCodeVisible = true;
                visibleColumnsCount++;
            }

            l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_NOMINATION_POSITION_ACCESSLEVEL");
            if (l != UIAccessLevel.Hidden)
            {
                isPositionAccessLevelVisible = true;
                visibleColumnsCount++;
            }

            l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_NOMINATION_POSITION_TOTALPOINTS");
            if (l != UIAccessLevel.Hidden)
            {
                isTotalPointsVisible = true;
                visibleColumnsCount++;
            }

            l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_NOMINATION_POSITION_STATUS");
            if (l != UIAccessLevel.Hidden)
            {
                isStatusVisible = true;
                visibleColumnsCount++;
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
                sb.Append("<td rowspan=\"2\" style='min-width: 855px;'>" + this.GenerateContentHtml() + "</td>");
                sb.Append("<td style=\"vertical-align: top;\">" + CommonFunctions.GenerateButtons(true, 160, true) + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr><td style=\"vertical-align: bottom;\">" + CommonFunctions.GenerateButtons(false, 0, true) + "</td></tr>");
                sb.Append("</table>");

                contentPage = sb.ToString();
            }
            else
            {
                contentPage = this.GenerateContentForExport();
            }

            return contentPage;
        }

        // Generate display data as html into the page
        private string GenerateContentHtml()
        {
            ReportVacAnnApplListNominatedBlockFilter filter = new ReportVacAnnApplListNominatedBlockFilter()
            {
                VacancyAnnounceId = vacancyAnnounceId,
                ResponsibleMilitaryUnitId = responsibleMilitaryUnitId,
                OrderBy = sortBy,
                PageIndex = 0,
                PageCount = 0
            };

            //Get the list of records according to the specified filters, order and paging
            List<ReportVacAnnApplListNominatedBlock> listVacAnnApplListNominatedBlock = ReportVacAnnApplListNominatedUtil.GetListDetailedReportSearch(filter, 0, CurrentUser);

            VacancyAnnounce vacancyAnnounce = VacancyAnnounceUtil.GetVacancyAnnounce((int)vacancyAnnounceId, CurrentUser);

            MilitaryUnit militaryUnit = MilitaryUnitUtil.GetMilitaryUnit((int)responsibleMilitaryUnitId, CurrentUser);

            string html = @"<table style='padding: 5px;'>
                             <tr>
                                <td align='right' style='width: 220px;'>
                                    <span class='Label'>Конкурс обявен със заповед №:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 620px;'>
                                <span class='ValueLabel'>" + vacancyAnnounce.OrderNumOrderDate + @"</span>&nbsp;
                                    <span class='Label'>Отговорно поделение:&nbsp;</span>
                                    <span class='ValueLabel'>" + militaryUnit.DisplayTextForSelection + @"</span>
                                </td>
                             </tr>";

            if (listVacAnnApplListNominatedBlock.Count() > 0)
            {
                html += @"
                    <tr><td colspan='4' align='center'>
                    <table id='applicantsTable' name='applicantsTable' class='CommonHeaderTable'>
                        <thead>
                            <tr>
                                <th style='width: 20px; border-left: 1px solid #000000;'>№</th>";
                
                if (isApplNameVisible)
                {
                    html += "<th style='width: 120px;'>Кандидат</th>";
                }

                if (isIdentNumbVisible)
                {
                    html += "<th style='width: 90px;'>ЕГН</th>";
                }

                if (isMilitUnitVisible)
                {
                    html += "<th style='width: 150px;'>" + MilitaryUnitLabel + @"</th>";
                }

                if (isPositionVisible)
                {
                    html += "<th style='width: 90px;'>Длъжност</th>";
                }

                if (isPositionCodeVisible)
                {
                    html += "<th style='width: 90px;'>Код на длъжноста</th>";
                }

                if (isPositionAccessLevelVisible)
                {
                    html += "<th style='width: 90px;'>Ниво на достъп до КИ</th>";
                }

                if (isTotalPointsVisible)
                {
                    html += "<th style='width: 70px;'>Общо точки</th>";
                }

                if (isStatusVisible)
                {
                    html += "<th style='width: 70px; border-right: 1px solid #000000;'>Статус на кандидата</th>";
                }
                
                html += @"</tr>
                    </thead><tbody>";
            }

            int counter = 1;

            foreach (ReportVacAnnApplListNominatedBlock reportVacAnnApplListNominatedBlock in listVacAnnApplListNominatedBlock)
            {
                html += @"<tr>
                            <td align='center'>" + counter + @"</td>";

                if (isApplNameVisible)
                {
                    html += "<td align='left'>" + reportVacAnnApplListNominatedBlock.ApplicantName + "</td>";                    
                }

                if (isIdentNumbVisible)
                {
                    html += "<td align='left'>" + reportVacAnnApplListNominatedBlock.IdentityNumber + "</td>";
                }

                if (isMilitUnitVisible)
                {
                    html += "<td align='left'>" + reportVacAnnApplListNominatedBlock.MilitaryUnit + "</td>";
                }

                if (isPositionVisible)
                {
                    html += "<td align='left'>" + reportVacAnnApplListNominatedBlock.Position + "</td>";
                }

                if (isPositionCodeVisible)
                {
                    html += "<td align='left'>" + reportVacAnnApplListNominatedBlock.PositionCode + "</td>";
                }

                if (isPositionAccessLevelVisible)
                {
                    html += "<td align='left'>" + reportVacAnnApplListNominatedBlock.AccessLevel + "</td>";
                }

                if (isTotalPointsVisible)
                {
                    html += "<td align='left'>" + reportVacAnnApplListNominatedBlock.TotalPoints + "</td>";
                }

                if (isStatusVisible)
                {
                    html += "<td align='left'>" + reportVacAnnApplListNominatedBlock.Status + "</td>";
                }

                html += "</tr>";
                counter++;
            }

            if (listVacAnnApplListNominatedBlock.Count() > 0)
            {
                html += "</tbody></table></td></tr>";
            }

            html += "</table>";

            return html;
        }

        // Handles export to excel button click
        protected void btnGenerateExcel_Click(object sender, EventArgs e)
        {
            string result = this.GeneratePageContent(true);
            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment; filename=ReportVacAnnListNominated.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        // Generate html as excel response result
        private string GenerateContentForExport()
        {
            ReportVacAnnApplListNominatedBlockFilter filter = new ReportVacAnnApplListNominatedBlockFilter()
            {
                VacancyAnnounceId = vacancyAnnounceId,
                ResponsibleMilitaryUnitId = responsibleMilitaryUnitId,
                OrderBy = sortBy,
                PageIndex = 0,
                PageCount = 0
            };

            //Get the list of records according to the specified filters, order and paging
            List<ReportVacAnnApplListNominatedBlock> listVacAnnApplListNominatedBlock = ReportVacAnnApplListNominatedUtil.GetListDetailedReportSearch(filter, 0, CurrentUser);

            VacancyAnnounce vacancyAnnounce = VacancyAnnounceUtil.GetVacancyAnnounce((int)vacancyAnnounceId, CurrentUser);

            MilitaryUnit militaryUnit = MilitaryUnitUtil.GetMilitaryUnit((int)responsibleMilitaryUnitId, CurrentUser);

            string html = @"<html xmlns='http://www.w3.org/1999/xhtml' xml:lang='en'>
                            <head>
                                <meta http-equiv='content-type' content='application/xhtml+xml; charset=UTF-8' />
                            </head>
                            <body>
                                <table>
                                    <tr><td align='center' colspan='9' style='font-weight: bold; font-size: 1.6em;'>АСУ на човешките ресурси</td></tr>
                                    <tr><td align='center' colspan='9' style='font-weight: bold; font-size: 2em;'>Кандидати</td></tr>
                                    <tr><td colspan='9'>&nbsp;</td></tr>
                                    <tr><td align='center' colspan='9' style='font-weight: bold; font-size: 1.3em;'>Списък на кандидатите определени за назначаване</td></tr>
                                    <tr><td colspan='9'>&nbsp;</td></tr>
                                    <tr>
                                        <td align='center' colspan='9'>
                                            <span style='font-weight: normal;'>Конкурс обявен със заповед №:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + vacancyAnnounce.OrderNumOrderDate + @"</span>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <span style='font-weight: normal;'>Отговорно поделение:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + militaryUnit.DisplayTextForSelection + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style='width: 30px;'>&nbsp;</td>";

            if (isApplNameVisible)
            {
                html += "<td style='width: 200px;'>&nbsp;</td>";
            }

            if (isIdentNumbVisible)
            {
                html += "<td style='width: 100px;'>&nbsp;</td>";
            }

            if (isMilitUnitVisible)
            {
                html += "<td style='width: 150px;'>&nbsp;</td>";
            }

            if (isPositionVisible)
            {
                html += "<td style='width: 100px;'>&nbsp;</td>";
            }

            if (isPositionCodeVisible)
            {
                html += "<td style='width: 100px;'>&nbsp;</td>";
            }

            if (isPositionAccessLevelVisible)
            {
                html += "<td style='width: 150px;'>&nbsp;</td>";
            }

            if (isTotalPointsVisible)
            {
                html += "<td style='width: 100px;'>&nbsp;</td>";
            }

            if (isStatusVisible)
            {
                html += "<td style='width: 150px;'>&nbsp;</td>";
            }

            html += @"</tr>
                </table>";


            if (listVacAnnApplListNominatedBlock.Count() > 0)
            {
                html += @"
                    <table>
                        <thead>
                            <tr>
                                <th style='width: 30px; border-bottom: 1px solid white; background-color: black; color: #FFFFFF;'>№</th>";

                if (isApplNameVisible)
                {
                    html += "<th style='width: 200px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Кандидат</th>";
                }

                if (isIdentNumbVisible)
                {
                    html += "<th style='width: 100px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>ЕГН</th>";
                }

                if (isMilitUnitVisible)
                {
                    html += "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>" + MilitaryUnitLabel + @"</th>";
                }

                if (isPositionVisible)
                {
                    html += "<th style='width: 100px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Длъжност</th>";
                }

                if (isPositionCodeVisible)
                {
                    html += "<th style='width: 100px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Код на длъжността</th>";
                }

                if (isPositionAccessLevelVisible)
                {
                    html += "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Ниво на достъп до КИ</th>";
                }

                if (isTotalPointsVisible)
                {
                    html += "<th style='width: 100px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Общо точки</th>";
                }

                if (isStatusVisible)
                {
                    html += "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Статус на кандидата</th>";
                }
                
                html += @"</tr>
                    </thead><tbody>";

                int counter = 1;

                foreach (ReportVacAnnApplListNominatedBlock reportVacAnnApplListNominatedBlock in listVacAnnApplListNominatedBlock)
                {
                    html += @"
                            <tr>
                                <td align='center' style='border: 1px solid black;'>" + counter + @"</td>";

                    if (isApplNameVisible)
                    {
                        html += "<td align='left' style='border: 1px solid black;'>" + reportVacAnnApplListNominatedBlock.ApplicantName + "</td>";
                    }

                    if (isIdentNumbVisible)
                    {
                        html += "<td align='left' style='border: 1px solid black;'>" + reportVacAnnApplListNominatedBlock.IdentityNumber + "</td>";
                    }

                    if (isMilitUnitVisible)
                    {
                        html += "<td align='left' style='border: 1px solid black;'>" + reportVacAnnApplListNominatedBlock.MilitaryUnit + "</td>";
                    }

                    if (isPositionVisible)
                    {
                        html += "<td align='left' style='border: 1px solid black;'>" + reportVacAnnApplListNominatedBlock.Position + "</td>";
                    }

                    if (isPositionCodeVisible)
                    {
                        html += "<td align='left' style='border: 1px solid black;'>" + reportVacAnnApplListNominatedBlock.PositionCode + "</td>";
                    }

                    if (isPositionAccessLevelVisible)
                    {
                        html += "<td align='left' style='border: 1px solid black;'>" + reportVacAnnApplListNominatedBlock.AccessLevel + "</td>";
                    }

                    if (isTotalPointsVisible)
                    {
                        html += "<td align='left' style='border: 1px solid black;'>" + reportVacAnnApplListNominatedBlock.TotalPoints + "</td>";
                    }

                    if (isStatusVisible)
                    {
                        html += "<td align='left' style='border: 1px solid black;'>" + reportVacAnnApplListNominatedBlock.Status + "</td>";
                    }

                     html += "</tr>";
                    counter++;
                }

                html += "</tbody></table>";
            }

            html += "</body></html>";

            return html;
        }
    }
}
