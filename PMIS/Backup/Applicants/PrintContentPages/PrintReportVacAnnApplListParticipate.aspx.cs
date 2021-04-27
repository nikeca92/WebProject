using System;
using System.Linq;
using System.Text;

using PMIS.Common;
using PMIS.Applicants.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace PMIS.Applicants.PrintContentPages
{
    public partial class PrintReportVacAnnApplListParticipate : APPLPage
    {
        const string All = "Всички";

        int vacancyAnnounceId = 0;
        int responsibleMilitaryUnitId = 0;
        string applicantStatustKey = null;
        int sortBy = 1; // 1 - Default

        UIAccessLevel l;

        int visibleColumnsCount = 1;

        bool isIdentNumbVisible = false;
        bool isApplNameVisible = false;
        bool isAddresVisible = false;
        bool isMilitDepartmentVisible = false;
        bool isApplStatusVisible = false;

        public void SetPrintTitleLinesWidth()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnPrintTitleLinesWidth");

            hf.Value = "790";
        }

        public void SetHeadersLeft()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnHeadersLeft");

            hf.Value = "100";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.SetPrintTitleLinesWidth();
                this.SetHeadersLeft();
            }

            // Check visibility right for the print screen
            if (this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_REPORT_DETAILED") != UIAccessLevel.Hidden)
            {
                if (!String.IsNullOrEmpty(Request.Params["VacancyAnnounceID"]))
                {
                    int.TryParse(Request.Params["VacancyAnnounceID"], out vacancyAnnounceId);
                }

                if (!String.IsNullOrEmpty(Request.Params["ResponsibleMilitaryUnitID"]))
                {
                    int.TryParse(Request.Params["ResponsibleMilitaryUnitID"], out responsibleMilitaryUnitId);
                }

                if (!String.IsNullOrEmpty(Request.Params["ApplicantStatusID"])
                    && Request.Params["ApplicantStatusID"] != ListItems.GetOptionAll().Value)
                {
                    applicantStatustKey = Request.Params["ApplicantStatusID"].ToString();
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
            l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_PARTICIPATE_IDENTNUM");
            if (l != UIAccessLevel.Hidden)
            {
                isIdentNumbVisible = true;
                visibleColumnsCount++;
            }

            l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_PARTICIPATE_APPLICANT");
            if (l != UIAccessLevel.Hidden)
            {
                isApplNameVisible = true;
                visibleColumnsCount++;
            }

            l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_PARTICIPATE_ADDRESS");
            if (l != UIAccessLevel.Hidden)
            {
                isAddresVisible = true;
                visibleColumnsCount++;
            }

            l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_PARTICIPATE_MILITARYDEPARTMENT");
            if (l != UIAccessLevel.Hidden)
            {
                isMilitDepartmentVisible = true;
                visibleColumnsCount++;
            }

            l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_PARTICIPATE_REMARK");
            if (l != UIAccessLevel.Hidden)
            {
                isApplStatusVisible = true;
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
            ReportVacAnnApplListParticipateBlockFilter filter = new ReportVacAnnApplListParticipateBlockFilter()
            {
                VacancyAnnounceId = vacancyAnnounceId,
                ResponsibleMilitaryUnitId = responsibleMilitaryUnitId,
                OrderBy = sortBy,
                PageIndex = 0,
                PageCount = 0
            };

            ApplicantExamStatus applicantExamStatus = null;

            switch (applicantStatustKey)
            {
                case "NOTRATED":
                    applicantExamStatus = ApplicantExamStatusUtil.GetApplicantExamStatusByKey("NOTRATED", CurrentUser);
                    break;
                case "DOCUMENTSREJECTED":
                    ApplicantPositionStatus appPositionStatus = ApplicantPositionStatusUtil.GetApplicantPositionStatusByKey("DOCUMENTSREJECTED", CurrentUser);
                    applicantExamStatus = new ApplicantExamStatus() 
                    { 
                        StatusId = appPositionStatus.StatusId,
                        StatusKey = appPositionStatus.StatusKey,
                        StatusName = appPositionStatus.StatusName
                    };
                    break;
            }

            filter.ApplicantExamStatus = applicantExamStatus;

            //Get the list of records according to the specified filters, order and paging
            List<ReportVacAnnApplListParticipateBlock> listVacAnnApplListParticipateBlock = ReportVacAnnApplListParticipateBlockUtil.GetListVacAnnApplListParticipateBlockSearch(filter, 0, CurrentUser);

            VacancyAnnounce vacancyAnnounce = VacancyAnnounceUtil.GetVacancyAnnounce((int)vacancyAnnounceId, CurrentUser);

            MilitaryUnit militaryUnit = MilitaryUnitUtil.GetMilitaryUnit((int)responsibleMilitaryUnitId, CurrentUser);

            string html = @"<table style='padding: 5px;'>
                             <tr>
                                <td align='right' style='width: 220px;'>
                                    <span class='Label'>Конкурс обявен със заповед №:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 570px;'>
                                <span class='ValueLabel'>" + vacancyAnnounce.OrderNumOrderDate + @"</span>&nbsp;
                                    <span class='Label'>Отговорно поделение:&nbsp;</span>
                                    <span class='ValueLabel'>" + militaryUnit.DisplayTextForSelection + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' style=''>
                                    <span class='Label'>Статус на кандидата:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                    <span class='ValueLabel'>" + (applicantExamStatus != null ? applicantExamStatus.StatusName : All) + @"</span>
                                </td>
                             </tr>";

            if (listVacAnnApplListParticipateBlock.Count() > 0)
            {
                html += @"
                    <tr><td colspan='4' align='center'>
                    <table id='applicantsTable' name='applicantsTable' class='CommonHeaderTable'>
                        <thead>
                            <tr>
                                <th style='width: 20px; border-left: 1px solid #000000;'>№</th>";

                if (isIdentNumbVisible)
                {
                    html += "<th style='width: 80px;'>ЕГН</th>";
                }

                if (isApplNameVisible)
                {
                    html += "<th style='width: 200px;'>Трите имена на кандидата</th>";
                }

                if (isAddresVisible)
                {
                    html += "<th style='width: 150px;'>Адрес</th>";
                }

                if (isMilitDepartmentVisible)
                {
                    html += "<th style='width: 150px;'>Място на регистрация</th>";
                }

                if (isApplStatusVisible)
                {
                    html += "<th style='width: 150px; border-right: 1px solid #000000;'>Статус</th>";
                }

                html += @"</tr>
                        </thead><tbody>";
            }

            int counter = 1;

            foreach (ReportVacAnnApplListParticipateBlock reportVacAnnApplListParticipateBlock in listVacAnnApplListParticipateBlock)
            {
                html += @"<tr>
                            <td align='center'>" + counter + @"</td>";

                if (isApplNameVisible)
                {
                    html += "<td align='left'>" + reportVacAnnApplListParticipateBlock.ApplicantName + @"</td>";
                }

                if (isIdentNumbVisible)
                {
                    html += "<td align='left'>" + reportVacAnnApplListParticipateBlock.IdentityNumber + @"</td>";
                }

                if (isAddresVisible)
                {
                    html += "<td align='left'>" + reportVacAnnApplListParticipateBlock.Address + @"</td>";
                }

                if (isMilitDepartmentVisible)
                {
                    html += "<td align='left'>" + reportVacAnnApplListParticipateBlock.MilitaryDepartment + @"</td>";
                }

                if (isApplStatusVisible)
                {
                    html += "<td align='left'>" + reportVacAnnApplListParticipateBlock.Remark + @"</td>";
                }
                
                html += "</tr>";
                counter++;
            }

            if (listVacAnnApplListParticipateBlock.Count() > 0)
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
            Response.AppendHeader("content-disposition", "attachment; filename=ReportVacAnnApplListParticipate.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        // Generate html as excel response result
        private string GenerateContentForExport()
        {
            ReportVacAnnApplListParticipateBlockFilter filter = new ReportVacAnnApplListParticipateBlockFilter()
            {
                VacancyAnnounceId = vacancyAnnounceId,
                ResponsibleMilitaryUnitId = responsibleMilitaryUnitId,
                OrderBy = sortBy,
                PageIndex = 0,
                PageCount = 0
            };

            ApplicantExamStatus applicantExamStatus = null;

            switch (applicantStatustKey)
            {
                case "NOTRATED":
                    applicantExamStatus = ApplicantExamStatusUtil.GetApplicantExamStatusByKey("NOTRATED", CurrentUser);
                    break;
                case "DOCUMENTSREJECTED":
                    ApplicantPositionStatus appPositionStatus = ApplicantPositionStatusUtil.GetApplicantPositionStatusByKey("DOCUMENTSREJECTED", CurrentUser);
                    applicantExamStatus = new ApplicantExamStatus()
                    {
                        StatusId = appPositionStatus.StatusId,
                        StatusKey = appPositionStatus.StatusKey,
                        StatusName = appPositionStatus.StatusName
                    };
                    break;
            }

            filter.ApplicantExamStatus = applicantExamStatus;

            //Get the list of records according to the specified filters, order and paging
            List<ReportVacAnnApplListParticipateBlock> listVacAnnApplListParticipateBlock = ReportVacAnnApplListParticipateBlockUtil.GetListVacAnnApplListParticipateBlockSearch(filter, 0, CurrentUser);

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
                                    <tr><td align='center' colspan='" + visibleColumnsCount + @"' style='font-weight: bold; font-size: 1.3em;'>Списък на кандидатите участвали в конкурс</td></tr>
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
                                        <td align='center' colspan='6'>
                                            <span style='font-weight: normal;'>Статус на кандидата:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + (applicantExamStatus != null ? applicantExamStatus.StatusName : All) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style='width: 30px;'>&nbsp;</td>";

            if (isIdentNumbVisible)
            {
                html += "<td style='width: 100px;'>&nbsp;</td>";
            }

            if (isApplNameVisible)
            {
                html += "<td style='width: 200px;'>&nbsp;</td>";
            }

            if (isAddresVisible)
            {
                html += "<td style='width: 200px;'>&nbsp;</td>";
            }

            if (isMilitDepartmentVisible)
            {
                html += "<td style='width: 200px;'>&nbsp;</td>";
            }

            if (isApplStatusVisible)
            {
                html += "<td style='width: 200px;'>&nbsp;</td>";
            }

            html += @"</tr>
                </table>";


            if (listVacAnnApplListParticipateBlock.Count() > 0)
            {
                html += @"
                    <table>
                        <thead>
                            <tr>
                                <th style='width: 30px; border-bottom: 1px solid white; background-color: black; color: #FFFFFF;'>№</th>";

                if (isIdentNumbVisible)
                {
                    html += "<th style='width: 100px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>ЕГН</th>";
                }

                if (isApplNameVisible)
                {
                    html += "<th style='width: 200px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Трите имена на кандидата</th>";
                }

                if (isAddresVisible)
                {
                    html += "<th style='width: 200px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Адрес</th>";
                }

                if (isMilitDepartmentVisible)
                {
                    html += "<th style='width: 200px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Място на регистрация</th>";
                }

                if (isApplStatusVisible)
                {
                    html += "<th style='width: 200px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Статус</th>";
                }
                
                html += @"</tr>
                    </thead><tbody>";

                int counter = 1;

                foreach (ReportVacAnnApplListParticipateBlock reportVacAnnApplListParticipateBlock in listVacAnnApplListParticipateBlock)
                {
                    html += @"
                            <tr>
                                <td align='center' style='border: 1px solid black;'>" + counter + @"</td>";

                    if (isIdentNumbVisible)
                    {
                        html += "<td align='left' style='border: 1px solid black;'>" + reportVacAnnApplListParticipateBlock.IdentityNumber + @"</td>";
                    }

                    if (isApplNameVisible)
                    {
                        html += "<td align='left' style='border: 1px solid black;'>" + reportVacAnnApplListParticipateBlock.ApplicantName + @"</td>";
                    }

                    if (isAddresVisible)
                    {
                        html += "<td align='left' style='border: 1px solid black;'>" + reportVacAnnApplListParticipateBlock.Address + @"</td>";
                    }

                    if (isMilitDepartmentVisible)
                    {
                        html += "<td align='left' style='border: 1px solid black;'>" + reportVacAnnApplListParticipateBlock.MilitaryDepartment + @"</td>";
                    }

                    if (isApplStatusVisible)
                    {
                        html += "<td align='left' style='border: 1px solid black;'>" + reportVacAnnApplListParticipateBlock.Remark + @"</td>";
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
