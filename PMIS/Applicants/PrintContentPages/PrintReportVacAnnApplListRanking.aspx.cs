using System;
using System.Linq;
using System.Text;

using PMIS.Common;
using PMIS.Applicants.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace PMIS.Applicants.PrintContentPages
{
    public partial class PrintReportVacAnnApplListRanking : APPLPage
    {
        const string All = "Всички";

        int vacancyAnnounceId = 0;
        int responsibleMilitaryUnitId = 0;
        int sortBy = 1; // 1 - Default

        UIAccessLevel l;

        int visibleColumnsCount = 1;

        bool isApplNameVisible = false;
        bool isIdentNumbVisible = false;        
        bool isPositionVisible = false;
        bool isMarkVisible = false;
        bool isPointsVisible = false;

        int addWidth = 0;

        public void SetPrintTitleLinesWidth()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnPrintTitleLinesWidth");

            hf.Value = (790 + (addWidth > 340 ? addWidth - 110 : addWidth)).ToString();
        }

        public void SetHeadersLeft()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnHeadersLeft");

            hf.Value = (100 + (addWidth != 0 ? addWidth / 2 : addWidth)).ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check visibility right for the print screen
            if (this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_RANKING") != UIAccessLevel.Hidden)
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

            this.SetPrintTitleLinesWidth();
            this.SetHeadersLeft();
        }

        private void SetUIResultVisibility()
        {
            l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_RANKING_APPLICANT");
            if (l != UIAccessLevel.Hidden)
            {
                isApplNameVisible = true;
                visibleColumnsCount++;
            }

            l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_RANKING_IDENTNUM");
            if (l != UIAccessLevel.Hidden)
            {
                isIdentNumbVisible = true;
                visibleColumnsCount++;
            }

            l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_RANKING_POSITION");
            if (l != UIAccessLevel.Hidden)
            {
                isPositionVisible = true;
                visibleColumnsCount++;
            }

            l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_RANKING_MARK");
            if (l != UIAccessLevel.Hidden)
            {
                isMarkVisible = true;
                visibleColumnsCount++;
            }

            l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_RANKING_POINTS");
            if (l != UIAccessLevel.Hidden)
            {
                isPointsVisible = true;
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
                sb.Append("<td rowspan=\"2\" style='min-width: 790px;'>" + this.GenerateContentHtml() + "</td>");
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
            ReportVacAnnApplListRankingBlockFilter filter = new ReportVacAnnApplListRankingBlockFilter()
            {
                VacancyAnnounceId = vacancyAnnounceId,
                ResponsibleMilitaryUnitId = responsibleMilitaryUnitId,
                OrderBy = sortBy,
                PageIndex = 0,
                PageCount = 0
            };

            //Get the list of records according to the specified filters, order and paging
            List<ReportVacAnnApplListRankingBlock> listVacAnnApplListRankingBlock = ReportVacAnnApplListRankingBlockUtil.GetListVacAnnApplListRankingBlockSearch(filter, 0, CurrentUser);
            
            List<Exam> exams = ExamUtil.GetExamsForVacancyAnnounce(vacancyAnnounceId, CurrentUser);

            VacancyAnnounce vacancyAnnounce = VacancyAnnounceUtil.GetVacancyAnnounce((int)vacancyAnnounceId, CurrentUser);

            MilitaryUnit militaryUnit = MilitaryUnitUtil.GetMilitaryUnit((int)responsibleMilitaryUnitId, CurrentUser);

            string html = @"<table style='padding: 5px;'>
                             <tr>
                                <td align='right' style='width: 220px;'>
                                    <span class='Label'>Конкурс обявен със заповед №:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 500px;'>
                                    <span class='ValueLabel'>" + vacancyAnnounce.OrderNumOrderDate + @"</span>&nbsp;
                                    <span class='Label'>Отговорно поделение:&nbsp;</span>
                                    <span class='ValueLabel'>" + militaryUnit.DisplayTextForSelection + @"</span>
                                </td>
                             </tr>";

            if (listVacAnnApplListRankingBlock.Count() > 0)
            {
                html += @"
                    <tr><td colspan='4' align='center'>
                    <table id='applicantsTable' name='applicantsTable' class='CommonHeaderTable' style='table-layout: fixed; width: #TABLE_WIDTH#'>
                        <thead>
                            <tr>
                                <th style='width: 20px; border-left: 1px solid #000000;'>№</th>";

                if (isApplNameVisible)
                {
                    html += "<th style='width: 200px;'>Име</th>";
                }

                if (isIdentNumbVisible)
                {
                    html += "<th style='width: 90px;'>ЕГН</th>";
                }

                if (isPositionVisible)
                {
                    html += "<th style='width: 100px;'>Длъжност</th>";
                }
                
                int examsCount = 0;
                foreach (Exam exam in exams)
                {
                    if (isMarkVisible)
                    {
                        html += "<th style='width: 140px;'>Оценка " + exam.ExamName + "</th>";
                        
                        if (examsCount >= 1)
                        {
                            addWidth += 140;
                        }
                    }

                    if (isPointsVisible)
                    {
                        html += "<th style='width: 140px; border-left: solid 2px #000000;'>Точки " + exam.ExamName + "</th>";

                        if (examsCount >= 1)
                        {
                            addWidth += 140;
                        }
                    }

                    examsCount++;
                }

                html += @"</tr>
                    </thead><tbody>";
            }

            int counter = 1;

            foreach (ReportVacAnnApplListRankingBlock reportVacAnnApplListRankingBlock in listVacAnnApplListRankingBlock)
            {
                html += @"<tr>
                            <td align='center'>" + counter + @"</td>";

                if (isApplNameVisible)
                {
                    html += "<td align='left'>" + reportVacAnnApplListRankingBlock.ApplicantName + @"</td>";
                }

                if (isIdentNumbVisible)
                {
                    html += "<td align='left'>" + reportVacAnnApplListRankingBlock.IdentityNumber + @"</td>";
                }

                if (isPositionVisible)
                {
                    html += "<td align='left'>" + reportVacAnnApplListRankingBlock.Position + @"</td>";
                }

                foreach (ApplicantExamMarkBlock mark in reportVacAnnApplListRankingBlock.Marks)
                {
                    if (isMarkVisible)
                    {
                        html += "<td align='center'>" + (mark.Mark.HasValue ? mark.Mark.Value.ToString() : "") + "</td>";
                    }

                    if (isPointsVisible)
                    {
                        html += "<td align='center'>" + (mark.Points.HasValue ? mark.Points.Value.ToString() : "") + "</td>";
                    }
                }
                
                html += "</tr>";
                counter++;
            }

            if (listVacAnnApplListRankingBlock.Count() > 0)
            {
                html += "</tbody></table></td></tr>";
            }

            html += "</table>";

            html = html.Replace("#TABLE_WIDTH#", (790 + (addWidth > 340 ? addWidth - 110 : addWidth)).ToString() + "px");

            return html;
        }

        // Handles export to excel button click
        protected void btnGenerateExcel_Click(object sender, EventArgs e)
        {
            string result = this.GeneratePageContent(true);
            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment; filename=ReportVacAnnApplListRanking.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        // Generate html as excel response result
        private string GenerateContentForExport()
        {
            ReportVacAnnApplListRankingBlockFilter filter = new ReportVacAnnApplListRankingBlockFilter()
            {
                VacancyAnnounceId = vacancyAnnounceId,
                ResponsibleMilitaryUnitId = responsibleMilitaryUnitId,
                OrderBy = sortBy,
                PageIndex = 0,
                PageCount = 0
            };

            //Get the list of records according to the specified filters, order and paging
            List<ReportVacAnnApplListRankingBlock> listVacAnnApplListRankingBlock = ReportVacAnnApplListRankingBlockUtil.GetListVacAnnApplListRankingBlockSearch(filter, 0, CurrentUser);

            List<Exam> exams = ExamUtil.GetExamsForVacancyAnnounce(vacancyAnnounceId, CurrentUser);

            int examsCount = 0;
            foreach (Exam exam in exams)
            {
                if (examsCount > 0)
                {
                    if (isMarkVisible)
                    {
                        visibleColumnsCount++;
                    }

                    if (isPointsVisible)
                    {
                        visibleColumnsCount++;
                    }   
                }
            }

            VacancyAnnounce vacancyAnnounce = VacancyAnnounceUtil.GetVacancyAnnounce((int)vacancyAnnounceId, CurrentUser);

            MilitaryUnit militaryUnit = MilitaryUnitUtil.GetMilitaryUnit((int)responsibleMilitaryUnitId, CurrentUser);

            string html = @"<html xmlns='http://www.w3.org/1999/xhtml' xml:lang='en'>
                            <head>
                                <meta http-equiv='content-type' content='application/xhtml+xml; charset=UTF-8' />
                            </head>
                            <body>
                                <table>
                                    <tr><td align='center' colspan='" + visibleColumnsCount + @"' style='font-weight: bold; font-size: 1.6em;'>АСУ на човешките ресурси</td></tr>
                                    <tr><td align='center' colspan='" + visibleColumnsCount + @"' style='font-weight: bold; font-size: 2em;'>Кандидати</td></tr>
                                    <tr><td colspan='" + visibleColumnsCount + @"'>&nbsp;</td></tr>
                                    <tr><td align='center' colspan='" + visibleColumnsCount + @"' style='font-weight: bold; font-size: 1.3em;'>Списък на кандидатите класирани в конкурс</td></tr>
                                    <tr><td colspan='" + visibleColumnsCount + @"'>&nbsp;</td></tr>
                                    <tr>
                                        <td align='center' colspan='" + visibleColumnsCount + @"'>
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

            if (isPositionVisible)
            {
                html += "<td style='width: 200px;'>&nbsp;</td>";
            }

            foreach (Exam exam in exams)
            {
                if (isMarkVisible)
                {
                    html += "<td style='width: 200px;'>&nbsp;</td>";
                }

                if (isPointsVisible)
                {
                    html += "<td style='width: 200px;'>&nbsp;</td>";
                }
            }
            
            html += @"</tr>
                </table>";


            if (listVacAnnApplListRankingBlock.Count() > 0)
            {
                html += @"
                    <table>
                        <thead>
                            <tr>
                                <th style='width: 30px; border-bottom: 1px solid white; background-color: black; color: #FFFFFF;'>№</th>";
            
                if (isApplNameVisible)
                {
                    html += "<th style='width: 200px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Име</th>";
                }
             
                if (isIdentNumbVisible)
                {
                    html += "<th style='width: 100px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>ЕГН</th>";
                }

                if (isPositionVisible)
                {
                    html += "<th style='width: 200px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Длъжност</th>";
                }

                foreach (Exam exam in exams)
                {
                    if (isMarkVisible)
                    {
                        html += "<th style='width: 200px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Оценка " + exam.ExamName + "</th>";
                    }

                    if (isPointsVisible)
                    {
                        html += "<th style='width: 200px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Точки " + exam.ExamName + "</th>";
                    }
                }
                
                html += @"</tr>
                    </thead><tbody>";

                int counter = 1;

                foreach (ReportVacAnnApplListRankingBlock reportVacAnnApplListRankingBlock in listVacAnnApplListRankingBlock)
                {
                    html += @"
                            <tr>
                                <td align='center' style='border: 1px solid black;'>" + counter + @"</td>";

                    if (isApplNameVisible)
                    {
                        html += "<td align='left' style='border: 1px solid black;'>" + reportVacAnnApplListRankingBlock.IdentityNumber + @"</td>";
                    }

                    if (isIdentNumbVisible)
                    {
                        html += "<td align='left' style='border: 1px solid black;'>" + reportVacAnnApplListRankingBlock.ApplicantName + @"</td>";
                    }

                    if (isPositionVisible)
                    {
                        html += "<td align='left' style='border: 1px solid black;'>" + reportVacAnnApplListRankingBlock.Position + @"</td>";
                    }

                    foreach (ApplicantExamMarkBlock mark in reportVacAnnApplListRankingBlock.Marks)
                    {
                        if (isMarkVisible)
                        {
                            html += "<td align='center' style='border: 1px solid black;'>" + (mark.Mark.HasValue ? mark.Mark.Value.ToString() : "") + "</td>";
                        }

                        if (isPointsVisible)
                        {
                            html += "<td align='center' style='border: 1px solid black;'>" + (mark.Points.HasValue ? mark.Points.Value.ToString() : "") + "</td>";
                        }
                    }

                    html += @"</tr>";
                    counter++;
                }

                html += "</tbody></table>";
            }

            html += "</body></html>";

            return html;
        }
    }
}
