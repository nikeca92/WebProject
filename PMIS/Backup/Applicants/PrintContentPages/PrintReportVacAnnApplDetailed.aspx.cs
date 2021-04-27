using System;
using System.Linq;
using System.Text;

using PMIS.Common;
using PMIS.Applicants.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace PMIS.Applicants.PrintContentPages
{
    public partial class PrintReportVacAnnApplDetailed : APPLPage
    {
        const string All = "Всички";
        private string MilitaryUnitLabel = CommonFunctions.GetLabelText("MilitaryUnit");

        int vacancyAnnounceId = 0;
        int? militaryDepartmentId = null;
        int? militaryUnitId = null;
        int? responsibleMilitaryUnitId = null;
        int sortBy = 1; // 1 - Default

        UIAccessLevel l;

        int visibleColumnsCount = 1;

        bool isApplNameVisible = false;
        bool isIdentNumbVisible = false;
        bool isPermAddressVisible = false;
        bool isMilitaryUnitVisible = false;
        bool isPositionVisible = false;
        bool isRespMilitUnitVisible = false;
        bool isMilitDepartmentVisible = false;
        bool isApplDocStatusVisible = false;
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

                if (!String.IsNullOrEmpty(Request.Params["MilitaryDepartmentID"])
                    && Request.Params["MilitaryDepartmentID"] != ListItems.GetOptionAll().Value)
                {
                    int mDepartmentId = 0;
                    int.TryParse(Request.Params["MilitaryDepartmentID"], out mDepartmentId);
                    if (mDepartmentId > 0)
                    {
                        militaryDepartmentId = mDepartmentId;
                    }
                }

                if (!String.IsNullOrEmpty(Request.Params["MilitaryUnitID"])
                    && Request.Params["MilitaryUnitID"] != ListItems.GetOptionAll().Value)
                {
                    int mUnitId = 0;
                    int.TryParse(Request.Params["MilitaryUnitID"], out mUnitId);
                    if (mUnitId > 0)
                    {
                        militaryUnitId = mUnitId;
                    }
                }

                if (!String.IsNullOrEmpty(Request.Params["ResponsibleMilitaryUnitID"])
                    && Request.Params["ResponsibleMilitaryUnitID"] != ListItems.GetOptionAll().Value)
                {
                    int rMilitaryUnitId = 0;
                    int.TryParse(Request.Params["ResponsibleMilitaryUnitID"], out rMilitaryUnitId);
                    if (rMilitaryUnitId > 0)
                    {
                        responsibleMilitaryUnitId = rMilitaryUnitId;
                    }
                }

                if (!String.IsNullOrEmpty(Request.Params["SortBy"]))
                {
                    int.TryParse(Request.Params["SortBy"], out sortBy);
                }

                if (vacancyAnnounceId > 0)
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
            l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_REPORT_DETAILED_APPLICANT");
            if (l != UIAccessLevel.Hidden)
            {
                isApplNameVisible = true;
                visibleColumnsCount++;
            }

            l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_REPORT_DETAILED_IDENTNUM");
            if (l != UIAccessLevel.Hidden)
            {
                isIdentNumbVisible = true;
                visibleColumnsCount++;
            }

            l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_REPORT_DETAILED_PERMADDRESS");
            if (l != UIAccessLevel.Hidden)
            {
                isPermAddressVisible = true;
                visibleColumnsCount++;
            }

            l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_REPORT_DETAILED_MILITARYUNIT");
            if (l != UIAccessLevel.Hidden)
            {
                isMilitaryUnitVisible = true;
                visibleColumnsCount++;
            }

            l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_REPORT_DETAILED_POSITION");
            if (l != UIAccessLevel.Hidden)
            {
                isPositionVisible = true;
                visibleColumnsCount++;
            }

            l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_REPORT_DETAILED_MILITARYUNIT_RESP");
            if (l != UIAccessLevel.Hidden)
            {
                isRespMilitUnitVisible = true;
                visibleColumnsCount++;
            }

            l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_REPORT_DETAILED_MILITARYDEPARTMENT");
            if (l != UIAccessLevel.Hidden)
            {
                isMilitDepartmentVisible = true;
                visibleColumnsCount++;
            }

            l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_REPORT_DETAILED_DOCSTATUS");
            if (l != UIAccessLevel.Hidden)
            {
                isApplDocStatusVisible = true;
                visibleColumnsCount++;
            }

            l = this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_REPORT_DETAILED_APPLSTATUS");
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
            ReportVacAnnApplDetailedBlockFilter filter = new ReportVacAnnApplDetailedBlockFilter()
            {
                VacancyAnnounceId = vacancyAnnounceId,
                MilitaryDepartmentId = militaryDepartmentId,
                MilitaryUnitId = militaryUnitId,
                ResponsibleMilitaryUnitId = responsibleMilitaryUnitId,
                OrderBy = sortBy,
                PageIndex = 0
            };

            //Get the list of records according to the specified filters and order
            List<ReportVacAnnApplDetailedBlock> listDetailedReports = ReportVacAnnApplDetailedUtil.GetListDetailedReportSearch(filter, 0, CurrentUser);

            VacancyAnnounce vacancyAnnounce = VacancyAnnounceUtil.GetVacancyAnnounce((int)vacancyAnnounceId, CurrentUser);

            MilitaryDepartment militaryDepartment = null;
            if (militaryDepartmentId != null)
            {
                militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment((int)militaryDepartmentId, CurrentUser);
            }

            MilitaryUnit militaryUnit = null;
            if (militaryUnitId != null)
            {
                militaryUnit = MilitaryUnitUtil.GetMilitaryUnit((int)militaryUnitId, CurrentUser);
            }

            MilitaryUnit responsibleMilitaryUnit = null;
            if (responsibleMilitaryUnitId != null)
            {
                responsibleMilitaryUnit = MilitaryUnitUtil.GetMilitaryUnit((int)responsibleMilitaryUnitId, CurrentUser);
            }

            string html = @"<table style='padding: 5px;'>
                             <tr>
                                <td align='right' style='width: 130px;'>
                                    <span class='Label'>Заповед №:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 170px;'>
                                   <span class='ValueLabel'>" + vacancyAnnounce.OrderNumOrderDate + @"</span>
                                </td>
                                <td align='right' style='width: 240px;'>
                                    <span class='Label'>Място на регистрация:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 170px;'>
                                    <span class='ValueLabel'>" + (militaryDepartment != null ? militaryDepartment.MilitaryDepartmentName : All) + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' style='width: 130px;'>
                                    <span class='Label'>ВПН/Структура:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 170px;'>
                                   <span class='ValueLabel'>" + (militaryUnit != null ? militaryUnit.DisplayTextForSelection : All) + @"</span>
                                </td>
                                <td align='right' style='width: 240px;'>
                                    <span class='Label'>ВПН/Структура отговорна за конкурса:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 170px;'>
                                    <span class='ValueLabel'>" + (responsibleMilitaryUnit != null ? responsibleMilitaryUnit.DisplayTextForSelection : All) + @"</span>
                                </td>
                             </tr>";

            if (listDetailedReports.Count() > 0)
            {
                html += @"<tr><td colspan='4' align='center'>
                            <table id='applicantsTable' name='applicantsTable' class='CommonHeaderTable'>
                                <thead>
                                    <tr>
                                        <th style='width: 20px; border-left: 1px solid #000000;'>№</th>";

                if (isApplNameVisible)
                {
                    html += "<th style='width: 70px;'>Кандидат</th>";   
                }

                if (isIdentNumbVisible)
                {
                    html += "<th style='width: 80px;'>ЕГН</th>";   
                }

                if (isPermAddressVisible)
                {
                    html += "<th style='width: 80px;'>Постоянен адрес</th>";
                }

                if (isMilitaryUnitVisible)
                {
                    html += "<th style='width: 110px;'>" + MilitaryUnitLabel + "</th>";   
                }

                if (isPositionVisible)
                {
                    html += "<th style='width: 70px;'>Длъжност</th>";   
                }

                if (isRespMilitUnitVisible)
                {
                    html += "<th style='width: 120px;'>" + MilitaryUnitLabel + " отговорнa за конкурса</th>";   
                }

                if (isMilitDepartmentVisible)
                {
                    html += "<th style='width: 90px;'>Място на регистрация</th>";   
                }

                if (isApplDocStatusVisible)
                {
                    html += "<th style='width: 100px;'>Статус на документите</th>";   
                }

                if (isApplStatusVisible)
                {
                    html += "<th style='width: 80px; border-right: 1px solid #000000;'>Статус на кандидата</th>";   
                }

                html += @"</tr>
                </thead><tbody>";
            }

            int counter = 1;

            foreach (ReportVacAnnApplDetailedBlock vacAnnApplDetailedReportBlock in listDetailedReports)
            {
                html += @"<tr>
                            <td align='center'>" + counter + @"</td>";

                if (isApplNameVisible)
                {
                    html += "<td align='left'>" + vacAnnApplDetailedReportBlock.ApplicantName + @"</td>";
                }

                if (isIdentNumbVisible)
                {
                    html += "<td align='left'>" + vacAnnApplDetailedReportBlock.IdentityNumber + @"</td>";
                }

                if (isPermAddressVisible)
                {
                    html += "<td align='left'>" + vacAnnApplDetailedReportBlock.PermAddress + @"</td>";
                }

                if (isMilitaryUnitVisible)
                {
                    html += "<td align='left'>" + vacAnnApplDetailedReportBlock.MilitaryUnit + @"</td>";
                }

                if (isPositionVisible)
                {
                    html += "<td align='left'>" + vacAnnApplDetailedReportBlock.Position + @"</td>";
                }

                if (isRespMilitUnitVisible)
                {
                    html += "<td align='left'>" + vacAnnApplDetailedReportBlock.MilitaryUnitResponsable + @"</td>";
                }

                if (isMilitDepartmentVisible)
                {
                    html += "<td align='left'>" + vacAnnApplDetailedReportBlock.MilitaryDepartment + @"</td>";
                }

                if (isApplDocStatusVisible)
                {
                    html += "<td align='left'>" + vacAnnApplDetailedReportBlock.ApplicantDocumentStatus + @"</td>";
                }

                if (isApplStatusVisible)
                {
                    html += "<td align='left'>" + vacAnnApplDetailedReportBlock.ApplicantStatus + @"</td>";
                }

                html += "</tr>";
                counter++;
            }

            if (listDetailedReports.Count() > 0)
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
            Response.AppendHeader("content-disposition", "attachment; filename=ReportVacAnnApplDetailed.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        // Generate html as excel response result
        private string GenerateContentForExport()
        {
            ReportVacAnnApplDetailedBlockFilter filter = new ReportVacAnnApplDetailedBlockFilter()
            {
                VacancyAnnounceId = vacancyAnnounceId,
                MilitaryDepartmentId = militaryDepartmentId,
                MilitaryUnitId = militaryUnitId,
                ResponsibleMilitaryUnitId = responsibleMilitaryUnitId,
                OrderBy = sortBy,
                PageIndex = 0
            };

            //Get the list of records according to the specified filters and order
            List<ReportVacAnnApplDetailedBlock> listDetailedReports = ReportVacAnnApplDetailedUtil.GetListDetailedReportSearch(filter, 0, CurrentUser);

            VacancyAnnounce vacancyAnnounce = VacancyAnnounceUtil.GetVacancyAnnounce((int)vacancyAnnounceId, CurrentUser);

            MilitaryDepartment militaryDepartment = null;
            if (militaryDepartmentId != null)
            {
                militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment((int)militaryDepartmentId, CurrentUser);
            }

            MilitaryUnit militaryUnit = null;
            if (militaryUnitId != null)
            {
                militaryUnit = MilitaryUnitUtil.GetMilitaryUnit((int)militaryUnitId, CurrentUser);
            }

            MilitaryUnit responsibleMilitaryUnit = null;
            if (responsibleMilitaryUnitId != null)
            {
                responsibleMilitaryUnit = MilitaryUnitUtil.GetMilitaryUnit((int)responsibleMilitaryUnitId, CurrentUser);
            }

            string html = @"<html xmlns='http://www.w3.org/1999/xhtml' xml:lang='en'>
                            <head>
                                <meta http-equiv='content-type' content='application/xhtml+xml; charset=UTF-8' />
                            </head>
                            <body>
                                <table>
                                    <tr><td align='center' colspan='" + visibleColumnsCount + @"' style='font-weight: bold; font-size: 1.6em;'>АСУ на човешките ресурси</td></tr>
                                    <tr><td align='center' colspan='" + visibleColumnsCount + @"' style='font-weight: bold; font-size: 2em;'>Кандидати</td></tr>
                                    <tr><td colspan='" + visibleColumnsCount + @"'>&nbsp;</td></tr>
                                    <tr><td align='center' colspan='" + visibleColumnsCount + @"' style='font-weight: bold; font-size: 1.3em;'>Детайлна справка за кандидатите по обявен конкурс</td></tr>
                                    <tr><td colspan='" + visibleColumnsCount + @"'>&nbsp;</td></tr>
                                    <tr>
                                        <td align='center' colspan='" + visibleColumnsCount + @"'>
                                            <span style='font-weight: normal;'>Заповед №:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + vacancyAnnounce.OrderNumOrderDate + @"</span>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <span style='font-weight: normal;'>Място на регистрация:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + (militaryDepartment != null ? militaryDepartment.MilitaryDepartmentName : All) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='center' colspan='" + visibleColumnsCount + @"'>
                                            <span style='font-weight: normal;'>ВПН/Структура:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + (militaryUnit != null ? militaryUnit.DisplayTextForSelection : All) + @"</span>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <span style='font-weight: normal;'>ВПН/Структура отговорна за конкурса:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + (responsibleMilitaryUnit != null ? responsibleMilitaryUnit.DisplayTextForSelection : All) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>";
            html += @"<td style='width: 30px;'>&nbsp;</td>";

            if (isApplNameVisible)
            {                
                html += @"<td style='width: 150px;'>&nbsp;</td>";
            }

            if (isIdentNumbVisible)
            {
                html += @"<td style='width: 100px;'>&nbsp;</td>";
            }

            if (isPermAddressVisible)
            {
                html += @"<td style='width: 100px;'>&nbsp;</td>";
            }

            if (isMilitaryUnitVisible)
            {                
                html += @"<td style='width: 200px;'>&nbsp;</td>";
            }

            if (isPositionVisible)
            {                
                html += @"<td style='width: 150px;'>&nbsp;</td>";
            }

            if (isRespMilitUnitVisible)
            {
                html += @"<td style='width: 250px;'>&nbsp;</td>";
            }

            if (isMilitDepartmentVisible)
            {
                html += @"<td style='width: 250px;'>&nbsp;</td>";
            }

            if (isApplDocStatusVisible)
            {                
                html += @"<td style='width: 150px;'>&nbsp;</td>";
            }

            if (isApplStatusVisible)
            {
                html += @"<td style='width: 150px;'>&nbsp;</td>";
            }
            
            html += @"</tr>
                    </table>";


            if (listDetailedReports.Count() > 0)
            {
                html += @"
                    <table>
                        <thead>
                            <tr>
                                <th style='width: 30px; border-bottom: 1px solid white; background-color: black; color: #FFFFFF;'>№</th>";

                if (isApplNameVisible)
                {                
                    html += @"<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Кандидат</th>";
                }

                if (isIdentNumbVisible)
                {
                    html += @"<th style='width: 100px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>ЕГН</th>";
                }

                if (isPermAddressVisible)
                {
                    html += @"<th style='width: 100px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Постоянен адрес</th>";
                }

                if (isMilitaryUnitVisible)
                {                
                    html += @"<th style='width: 200px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>" + MilitaryUnitLabel + @"</th>";
                }

                if (isPositionVisible)
                {                
                    html += @"<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Длъжност</th>";
                }

                if (isRespMilitUnitVisible)
                {
                    html += @"<th style='width: 250px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>" + MilitaryUnitLabel + @" отговорнa за конкурса</th>";
                }

                if (isMilitDepartmentVisible)
                {
                    html += @"<th style='width: 250px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Място на регистрация</th>";
                }

                if (isApplDocStatusVisible)
                {                
                    html += @"<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Статус на документите</th>";
                }

                if (isApplStatusVisible)
                {
                    html += @"<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Статус на кандидата</th>";
                }

                html += @"</tr>
                    </thead><tbody>";

                int counter = 1;

                foreach (ReportVacAnnApplDetailedBlock vacAnnApplDetailedReportBlock in listDetailedReports)
                {
                    html += @"
                            <tr>
                                <td align='center' style='border: 1px solid black;'>" + counter + @"</td>";

                    if (isApplNameVisible)
                    {
                        html += @"<td align='left' style='border: 1px solid black;'>" + vacAnnApplDetailedReportBlock.ApplicantName + @"</td>";
                    }

                    if (isIdentNumbVisible)
                    {
                        html += @"<td align='left' style='border: 1px solid black;'>" + vacAnnApplDetailedReportBlock.IdentityNumber + @"</td>";
                    }

                    if (isPermAddressVisible)
                    {
                        html += @"<td align='left' style='border: 1px solid black;'>" + vacAnnApplDetailedReportBlock.PermAddress + @"</td>";
                    }

                    if (isMilitaryUnitVisible)
                    {
                        html += @"<td align='left' style='border: 1px solid black;'>" + vacAnnApplDetailedReportBlock.MilitaryUnit + @"</td>";
                    }

                    if (isPositionVisible)
                    {
                        html += @"<td align='left' style='border: 1px solid black;'>" + vacAnnApplDetailedReportBlock.Position + @"</td>";
                    }

                    if (isRespMilitUnitVisible)
                    {
                        html += @"<td align='left' style='border: 1px solid black;'>" + vacAnnApplDetailedReportBlock.MilitaryUnitResponsable + @"</td>";
                    }

                    if (isMilitDepartmentVisible)
                    {
                        html += @"<td align='left' style='border: 1px solid black;'>" + vacAnnApplDetailedReportBlock.MilitaryDepartment + @"</td>";
                    }

                    if (isApplDocStatusVisible)
                    {
                        html += @"<td align='left' style='border: 1px solid black;'>" + vacAnnApplDetailedReportBlock.ApplicantDocumentStatus + @"</td>";
                    }

                    if (isApplStatusVisible)
                    {
                        html += @"<td align='left' style='border: 1px solid black;'>" + vacAnnApplDetailedReportBlock.ApplicantStatus + @"</td>";
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
