using System;
using System.Linq;
using System.Text;

using PMIS.Common;
using PMIS.Applicants.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace PMIS.Applicants.PrintContentPages
{
    public partial class PrintReportCadets : APPLPage
    {
        const string All = "Всички";

        int? militarySchoolId = null;
        int? militarySchoolYear = null;
        int? militarySubjectId = null;
        int? militarySpecId = null;
        int? statusId = null;

        int sortBy = 1; // 1 - Default

        UIAccessLevel l;

        int visibleColumnsCount = 1;

        bool isCadetNameVisible = false;
        bool isIdentNumbVisible = false;
        bool isMilitSchoolVisible = false;
        bool isMilitSchoolYearVisible = false;
        bool isSubjectVisible = false;
        bool isSpecializationVisible = false;
        bool isStatusVisible = false;

        public void SetPrintTitleLinesWidth()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnPrintTitleLinesWidth");

            hf.Value = "810";
        }

        public void SetHeadersLeft()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnHeadersLeft");

            hf.Value = "110";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.SetPrintTitleLinesWidth();
                this.SetHeadersLeft();
            }

            // Check visibility right for the print screen
            if (this.GetUIItemAccessLevel("APPL_REPORTS_REPORT_CADET") != UIAccessLevel.Hidden)
            {
                if (!String.IsNullOrEmpty(Request.Params["MilitarySchoolID"])
                    && Request.Params["MilitarySchoolID"] != ListItems.GetOptionChooseOne().Value)
                {
                    int mSchoolId = 0;
                    int.TryParse(Request.Params["MilitarySchoolID"], out mSchoolId);
                    if (mSchoolId > 0)
                    {
                        militarySchoolId = mSchoolId;
                    }
                }

                if (!String.IsNullOrEmpty(Request.Params["MilitarySchoolYear"])
                    && Request.Params["MilitarySchoolYear"] != ListItems.GetOptionChooseOne().Value)
                {
                    int mSchoolYear = 0;
                    int.TryParse(Request.Params["MilitarySchoolYear"], out mSchoolYear);
                    if (mSchoolYear > 0)
                    {
                        militarySchoolYear = mSchoolYear;
                    }
                }

                if (!String.IsNullOrEmpty(Request.Params["MilitarySubjectID"])
                    && Request.Params["MilitarySubjectID"] != ListItems.GetOptionChooseOne().Value)
                {
                    int mSubjectId = 0;
                    int.TryParse(Request.Params["MilitarySubjectID"], out mSubjectId);
                    if (mSubjectId > 0)
                    {
                        militarySubjectId = mSubjectId;
                    }
                }

                if (!String.IsNullOrEmpty(Request.Params["MilitarySpecID"])
                    && Request.Params["MilitarySpecID"] != ListItems.GetOptionChooseOne().Value)
                {
                    int mSpecId = 0;
                    int.TryParse(Request.Params["MilitarySpecID"], out mSpecId);
                    if (mSpecId > 0)
                    {
                        militarySpecId = mSpecId;
                    }
                }

                if (!String.IsNullOrEmpty(Request.Params["StatusID"])
                    && Request.Params["StatusID"] != ListItems.GetOptionChooseOne().Value)
                {
                    int mStatusId = 0;
                    int.TryParse(Request.Params["StatusID"], out mStatusId);
                    if (mStatusId >= 0)
                    {
                        statusId = mStatusId;
                    }
                }

                if (!String.IsNullOrEmpty(Request.Params["SortBy"]))
                {
                    int.TryParse(Request.Params["SortBy"], out sortBy);
                }

                this.SetUIResultVisibility();

                this.divResults.InnerHtml = GeneratePageContent(false);
            }
            else
            {
                this.divResults.InnerHtml = "";
            }
        }

        private void SetUIResultVisibility()
        {
            l = this.GetUIItemAccessLevel("APPL_REPORTS_REPORT_CADET_CADETNAME");
            if (l != UIAccessLevel.Hidden)
            {
                isCadetNameVisible = true;
                visibleColumnsCount++;
            }

            l = this.GetUIItemAccessLevel("APPL_REPORTS_REPORT_CADET_IDENTNUM");
            if (l != UIAccessLevel.Hidden)
            {
                isIdentNumbVisible = true;
                visibleColumnsCount++;
            }

            l = this.GetUIItemAccessLevel("APPL_REPORTS_REPORT_CADET_MILITARYSCOOLNAME");
            if (l != UIAccessLevel.Hidden)
            {
                isMilitSchoolVisible = true;
                visibleColumnsCount++;
            }

            l = this.GetUIItemAccessLevel("APPL_REPORTS_REPORT_CADET_MILITARYSCOOLYEAR");
            if (l != UIAccessLevel.Hidden)
            {
                isMilitSchoolYearVisible = true;
                visibleColumnsCount++;
            }

            l = this.GetUIItemAccessLevel("APPL_REPORTS_REPORT_CADET_SUBJECT");
            if (l != UIAccessLevel.Hidden)
            {
                isSubjectVisible = true;
                visibleColumnsCount++;
            }

            l = this.GetUIItemAccessLevel("APPL_REPORTS_REPORT_CADET_SPECIALISATION");
            if (l != UIAccessLevel.Hidden)
            {
                isSpecializationVisible = true;
                visibleColumnsCount++;
            }

            l = this.GetUIItemAccessLevel("APPL_REPORTS_REPORT_CADET_STATUS");
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
                sb.Append("<td rowspan=\"2\" style='min-width: 810px;'>" + this.GenerateContentHtml() + "</td>");
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
            ReportCadetBlockFilter filter = new ReportCadetBlockFilter()
            {
                MilitarySchoolId = (militarySchoolId != null ? (int)militarySchoolId : 0),
                MilitarySchoolYear = (militarySchoolYear != null ? (int)militarySchoolYear : 0),
                SubjectId = (militarySubjectId != null ? (int)militarySubjectId : 0),
                SpecializationId = (militarySpecId != null ? (int)militarySpecId : 0),
                StatusId = (statusId != null ? (int)statusId : -1),
                OrderBy = sortBy,
                PageIndex = 0,
                PageCount = 0
            };

            //Get the list of records according to the specified filters, order and paging
            List<ReportCadetBlock> listReportCadets = ReportCadetUtil.GetAllReportCadetBlock(filter, 0, CurrentUser);

            MilitarySchool militarySchool = null;
            if (militarySchoolId != null)
            {
                militarySchool = MilitarySchoolUtil.GetMilitarySchool((int)militarySchoolId, CurrentUser);
            }

            MilitarySchoolSubject militarySchoolSubject = null;
            if (militarySubjectId != null)
            {
                militarySchoolSubject = MilitarySchoolSubjectUtil.GetMilitarySchoolSubject((int)militarySubjectId, CurrentUser);
            }

            Specialization militarySpecialization = null;
            if (militarySpecId != null)
            {
                militarySpecialization = SpecializationUtil.GetSpecialization((int)militarySpecId, CurrentUser);
            }

            string html = @"<table style='padding: 5px;'>
                             <tr>
                                <td align='right' style='width: 250px;'>
                                    <span class='Label'>Военно училище:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 150px;'>
                                   <span class='ValueLabel'>" + (militarySchool != null ? militarySchool.MilitarySchoolName : All) + @"</span>
                                </td>
                                <td align='right' style='width: 200px;'>
                                    <span class='Label'>Учебна година:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 200px;'>
                                    <span class='ValueLabel'>" + (militarySchoolYear != null ? militarySchoolYear.ToString() : All) + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' style=''>
                                    <span class='Label'>Специалност:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                   <span class='ValueLabel'>" + (militarySchoolSubject != null ? militarySchoolSubject.MilitarySchoolSubjectName : All) + @"</span>
                                </td>
                                <td align='right' style=''>
                                    <span class='Label'>Специализация:&nbsp;</span>
                                </td>
                                <td align='left' style=''>
                                    <span class='ValueLabel'>" + (militarySpecialization != null ? militarySpecialization.SpecializationName : All) + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' style=''>
                                    <span class='Label'>Статус:&nbsp;</span>
                                </td>
                                <td align='left' colspan='3'>
                                   <span class='ValueLabel'>" + (statusId != null ? (statusId == 0 ? "Неприети" : "Приети") : All) + @"</span>
                                </td>
                             </tr>";

            if (listReportCadets.Count() > 0)
            {
                html += @"
                    <tr><td colspan='4' align='center'>
                    <table id='applicantsTable' name='applicantsTable' class='CommonHeaderTable'>
                        <thead>
                            <tr>
                                <th style='width: 30px; border-left: 1px solid #000000;'>№</th>";

                if (isCadetNameVisible)
                {
                    html += "<th style='width: 100px;'>Курсант</th>";
                }

                if (isIdentNumbVisible)
                {
                    html += "<th style='width: 80px;'>ЕГН</th>";
                }

                if (isMilitSchoolVisible)
                {
                    html += "<th style='width: 160px;'>Военно училище</th>";
                }

                if (isMilitSchoolYearVisible)
                {
                    html += "<th style='width: 60px;'>Учебна година</th>";
                }

                if (isSubjectVisible)
                {
                    html += "<th style='width: 130px;'>Специалност</th>";
                }

                if (isSpecializationVisible)
                {
                    html += "<th style='width: 140px;'>Специализация</th>";
                }

                if (isStatusVisible)
                {
                    html += "<th style='width: 60px; border-right: 1px solid #000000;'>Статус</th>";
                }

                html += @"</tr>
                    </thead><tbody>";
            }

            int counter = 1;

            foreach (ReportCadetBlock reportCadetBlock in listReportCadets)
            {
                html += @"<tr>
                            <td align='center'>" + counter + @"</td>";

                if (isCadetNameVisible)
                {
                    html += "<td align='left'>" + reportCadetBlock.CadetName + "</td>";
                }

                if (isIdentNumbVisible)
                {
                    html += "<td align='left'>" + reportCadetBlock.IdentityNumber + "</td>";
                }

                if (isMilitSchoolVisible)
                {
                    html += "<td align='left'>" + reportCadetBlock.MilitarySchoolName + "</td>";
                }

                if (isMilitSchoolYearVisible)
                {
                    html += "<td align='left'>" + reportCadetBlock.MilitarySchoolYear + "</td>";                    
                }

                if (isSubjectVisible)
                {
                    html += "<td align='left'>" + reportCadetBlock.Subject + "</td>";
                }

                if (isSpecializationVisible)
                {
                    html += "<td align='left'>" + reportCadetBlock.Specialization + "</td>";
                }

                if (isStatusVisible)
                {
                    html += "<td align='left'>" + reportCadetBlock.Status + "</td>";
                }

                html += "</tr>";
                counter++;
            }

            if (listReportCadets.Count() > 0)
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
            Response.AppendHeader("content-disposition", "attachment; filename=ReportCadets.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        // Generate html as excel response result
        private string GenerateContentForExport()
        {
            ReportCadetBlockFilter filter = new ReportCadetBlockFilter()
            {
                MilitarySchoolId = (militarySchoolId != null ? (int)militarySchoolId : 0),
                MilitarySchoolYear = (militarySchoolYear != null ? (int)militarySchoolYear : 0),
                SubjectId = (militarySubjectId != null ? (int)militarySubjectId : 0),
                SpecializationId = (militarySpecId != null ? (int)militarySpecId : 0),
                OrderBy = sortBy,
                PageIndex = 0,
                PageCount = 0
            };

            //Get the list of records according to the specified filters, order and paging
            List<ReportCadetBlock> listReportCadets = ReportCadetUtil.GetAllReportCadetBlock(filter, 0, CurrentUser);

            MilitarySchool militarySchool = null;
            if (militarySchoolId != null)
            {
                militarySchool = MilitarySchoolUtil.GetMilitarySchool((int)militarySchoolId, CurrentUser);
            }

            MilitarySchoolSubject militarySchoolSubject = null;
            if (militarySubjectId != null)
            {
                militarySchoolSubject = MilitarySchoolSubjectUtil.GetMilitarySchoolSubject((int)militarySubjectId, CurrentUser);
            }

            Specialization militarySpecialization = null;
            if (militarySpecId != null)
            {
                militarySpecialization = SpecializationUtil.GetSpecialization((int)militarySpecId, CurrentUser);
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
                                    <tr><td align='center' colspan='" + visibleColumnsCount + @"' style='font-weight: bold; font-size: 1.3em;'>Списък на курсантите</td></tr>
                                    <tr><td colspan='" + visibleColumnsCount + @"'>&nbsp;</td></tr>
                                    <tr>
                                        <td align='center' colspan='" + visibleColumnsCount + @"'>
                                            <span style='font-weight: normal;'>Военно училище:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + (militarySchool != null ? militarySchool.MilitarySchoolName : All) + @"</span>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <span style='font-weight: normal;'>Учебна година:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + (militarySchoolYear != null ? militarySchoolYear.ToString() : All) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='center' colspan='" + visibleColumnsCount + @"'>
                                            <span style='font-weight: normal;'>Специалност:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + (militarySchoolSubject != null ? militarySchoolSubject.MilitarySchoolSubjectName : All) + @"</span>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <span style='font-weight: normal;'>Специализация:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + (militarySpecialization != null ? militarySpecialization.SpecializationName : All) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='center' colspan='" + visibleColumnsCount + @"'>
                                            <span style='font-weight: normal;'>Статус:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + (statusId != null ? (statusId == 0 ? "Неприети" : "Приети") : All) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style='width: 30px;'>&nbsp;</td>";

            if (isCadetNameVisible)
            {
                html += "<td style='width: 200px;'>&nbsp;</td>";
            }

            if (isIdentNumbVisible)
            {
                html += "<td style='width: 100px;'>&nbsp;</td>";
            }

            if (isMilitSchoolVisible)
            {
                html += "<td style='width: 200px;'>&nbsp;</td>";
            }

            if (isMilitSchoolYearVisible)
            {
                html += "<td style='width: 150px;'>&nbsp;</td>";
            }

            if (isSubjectVisible)
            {
                html += "<td style='width: 300px;'>&nbsp;</td>";
            }

            if (isSpecializationVisible)
            {
                html += "<td style='width: 300px;'>&nbsp;</td>";
            }

            if (isStatusVisible)
            {
                html += "<td style='width: 100px;'>&nbsp;</td>";
            }

            html += @"</tr>
                </table>";


            if (listReportCadets.Count() > 0)
            {
                html += @"
                    <table>
                        <thead>
                            <tr>
                                <th style='width: 30px; border-bottom: 1px solid white; background-color: black; color: #FFFFFF;'>№</th>";
                
                if (isCadetNameVisible)
                {
                    html += "<th style='width: 200px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Курсант</th>";
                }

                if (isIdentNumbVisible)
                {
                    html += "<th style='width: 100px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>ЕГН</th>";
                }

                if (isMilitSchoolVisible)
                {
                    html += "<th style='width: 200px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Военно училище</th>";
                }

                if (isMilitSchoolYearVisible)
                {
                    html += "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Учебна година</th>";
                }

                if (isSubjectVisible)
                {
                    html += "<th style='width: 300px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Специалност отговорна за конкурса</th>";
                }

                if (isSpecializationVisible)
                {
                    html += "<th style='width: 300px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Специализация</th>";
                }

                if (isStatusVisible)
                {
                    html += "<th style='width: 100px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Статус</th>";
                }
                
                html += @"</tr>
                        </thead><tbody>";

                int counter = 1;

                foreach (ReportCadetBlock reportCadetBlock in listReportCadets)
                {
                    html += @"
                            <tr>
                                <td align='center' style='border: 1px solid black;'>" + counter + "</td>";

                    if (isCadetNameVisible)
                    {
                        html += "<td align='left' style='border: 1px solid black;'>" + reportCadetBlock.CadetName + "</td>";
                    }

                    if (isIdentNumbVisible)
                    {
                        html += "<td align='left' style='border: 1px solid black;'>" + reportCadetBlock.IdentityNumber + "</td>";
                    }

                    if (isMilitSchoolVisible)
                    {
                        html += "<td align='left' style='border: 1px solid black;'>" + reportCadetBlock.MilitarySchoolName + "</td>";
                    }

                    if (isMilitSchoolYearVisible)
                    {
                        html += "<td align='left' style='border: 1px solid black;'>" + reportCadetBlock.MilitarySchoolYear + "</td>";
                    }

                    if (isSubjectVisible)
                    {
                        html += "<td align='left' style='border: 1px solid black;'>" + reportCadetBlock.Subject + "</td>";
                    }

                    if (isSpecializationVisible)
                    {
                        html += "<td align='left' style='border: 1px solid black;'>" + reportCadetBlock.Specialization + "</td>";
                    }

                    if (isStatusVisible)
                    {
                        html += "<td align='left' style='border: 1px solid black;'>" + reportCadetBlock.Status + "</td>";
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
