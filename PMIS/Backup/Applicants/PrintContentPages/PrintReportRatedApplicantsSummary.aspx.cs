using System;
using System.Linq;
using System.Text;

using PMIS.Common;
using PMIS.Applicants.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace PMIS.Applicants.PrintContentPages
{
    public partial class PrintReportRatedApplicantsSummary : APPLPage
    {
        const string All = "Всички";
        private string MilitaryUnitLabel = CommonFunctions.GetLabelText("MilitaryUnit");

        int vacancyAnnounceId = 0;
        int? militaryUnitId = null;
        string position = null;
        int? militaryDepartmentId = null;
        string status = null;

        UIAccessLevel l;

        int visibleColumnsCount = 16;

        public void SetPrintTitleLinesWidth()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnPrintTitleLinesWidth");

            hf.Value = "1140";
        }

        public void SetHeadersLeft()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnHeadersLeft");

            hf.Value = "270";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.SetPrintTitleLinesWidth();
                this.SetHeadersLeft();
            }

            // Check visibility right for the print screen
            if (this.GetUIItemAccessLevel("APPL_REPORTS_REPORT_RATED_APPLICANTS_SUMMARY") != UIAccessLevel.Hidden)
            {
                if (!String.IsNullOrEmpty(Request.Params["VacancyAnnounceID"]))
                {
                    int.TryParse(Request.Params["VacancyAnnounceID"], out vacancyAnnounceId);
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

                position = Request.Params["Position"];

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

                status = Request.Params["Status"];

                if (vacancyAnnounceId > 0)
                {
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
            ReportRatedApplicantsSummaryFilter filter = new ReportRatedApplicantsSummaryFilter()
            {
                VacancyAnnounceId = vacancyAnnounceId,
                MilitaryUnitId = militaryUnitId,
                Position = position,
                MilitaryDepartmentId = militaryDepartmentId,
                Status = status,
                PageIndex = 0
            };

            //Get the list of records according to the specified filters and order
            List<ReportRatedApplicantsSummaryBlock> listBlocks = ReportRatedApplicantsSummaryUtil.GetReportRatedApplicantsSummarySearch(filter, 0, CurrentUser);

            VacancyAnnounce vacancyAnnounce = VacancyAnnounceUtil.GetVacancyAnnounce((int)filter.VacancyAnnounceId, CurrentUser);

            MilitaryUnit militaryUnit = null;
            if (filter.MilitaryUnitId != null)
            {
                militaryUnit = MilitaryUnitUtil.GetMilitaryUnit((int)militaryUnitId, CurrentUser);
            }

            MilitaryDepartment militaryDepartment = null;
            if (filter.MilitaryDepartmentId != null)
            {
                militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment((int)filter.MilitaryDepartmentId, CurrentUser);
            }

            ApplicantPositionStatus applicantPositionStatus = null;
            ApplicantExamStatus applicantExamStatus = null;
            string statusName = "";
            if (!String.IsNullOrEmpty(filter.Status))
            {
                applicantPositionStatus = ApplicantPositionStatusUtil.GetApplicantPositionStatusByKey(filter.Status, CurrentUser);
                applicantExamStatus = ApplicantExamStatusUtil.GetApplicantExamStatusByKey(filter.Status, CurrentUser);

                if (applicantPositionStatus != null)
                    statusName = applicantPositionStatus.StatusName;
                else if (applicantExamStatus != null)
                    statusName = applicantExamStatus.StatusName;
            }

            string html = @"<table style='padding: 5px;'>
                             <tr>
                                <td align='right' style='width: 360px;'>
                                    <span class='Label'>Заповед №:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 195px;'>
                                   <span class='ValueLabel'>" + vacancyAnnounce.OrderNumOrderDate + @"</span>
                                </td>
                                <td align='right' style='width: 160px;'>
                                    <span class='Label'>" + MilitaryUnitLabel + @":&nbsp;</span>
                                </td>
                                <td align='left' style='width: 395px;'>
                                    <span class='ValueLabel'>" + (militaryUnit != null ? militaryUnit.DisplayTextForSelection : All) + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right'>
                                    <span class='Label'>Длъжност:&nbsp;</span>
                                </td>
                                <td align='left'>
                                   <span class='ValueLabel'>" + (!String.IsNullOrEmpty(filter.Position) ? filter.Position : All) + @"</span>
                                </td>
                                <td align='right'>
                                    <span class='Label'>Място на регистрация:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 195px;'>
                                    <span class='ValueLabel'>" + (militaryDepartment != null ? militaryDepartment.MilitaryDepartmentName : All) + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right'>
                                    <span class='Label'>Състояние:&nbsp;</span>
                                </td>
                                <td align='left'>
                                   <span class='ValueLabel'>" + (!String.IsNullOrEmpty(statusName) ? statusName : All) + @"</span>
                                </td>
                             </tr>";

            if (listBlocks.Count() > 0)
            {
                html += @"<tr><td colspan='4' align='center'>
                            <table id='applicantsTable' name='applicantsTable' class='CommonHeaderTable'>
                                <thead>
                                    <tr>
                                        <th style='width: 180px; border-left: 1px solid #000000;' rowspan='2'>Военно формирование</th>
                                        <th style='width: 180px;' colspan='3'>до 35 години</th>
                                        <th style='width: 180px;' colspan='3'>до 45 години</th>
                                        <th style='width: 180px;' colspan='3'>над 45 години</th>
                                        <th style='width: 180px;' colspan='3'>Всичко</th>
                                        <th style='width: 180px;' colspan='3'>без военна подготовка</th>
                                    </tr>
                                    <tr>
                                       <th style='width: 60px;'>М</th>
                                       <th style='width: 60px;'>Ж</th>
                                       <th style='width: 60px;'>Общо</th>
                                       <th style='width: 60px;'>М</th>
                                       <th style='width: 60px;'>Ж</th>
                                       <th style='width: 60px;'>Общо</th>
                                       <th style='width: 60px;'>М</th>
                                       <th style='width: 60px;'>Ж</th>
                                       <th style='width: 60px;'>Общо</th>
                                       <th style='width: 60px;'>М</th>
                                       <th style='width: 60px;'>Ж</th>
                                       <th style='width: 60px;'>Общо</th>
                                       <th style='width: 60px;'>М</th>
                                       <th style='width: 60px;'>Ж</th>
                                       <th style='width: 60px;'>Общо</th>
                                    </tr>
                                </thead>
                                <tbody>";
            }

            int counter = 1;

            foreach (ReportRatedApplicantsSummaryBlock block in listBlocks)
            {
                string cellStyleText = "text-align: left;";
                string cellStyleNumber = "text-align: right;";

                if (block.RowType == 3)
                    cellStyleText = " text-align: right;";

                html += @"<tr>
                             <td style='" + cellStyleText + @"'>" + block.DisplayText + @"</td>
                             <td style='" + cellStyleNumber + @"'>" + block.ClassA_M_Cnt.ToString() + @"</td>
                             <td style='" + cellStyleNumber + @"'>" + block.ClassA_F_Cnt.ToString() + @"</td>
                             <td style='" + cellStyleNumber + @"'>" + block.ClassA_Total_Cnt.ToString() + @"</td>
                             <td style='" + cellStyleNumber + @"'>" + block.ClassB_M_Cnt.ToString() + @"</td>
                             <td style='" + cellStyleNumber + @"'>" + block.ClassB_F_Cnt.ToString() + @"</td>
                             <td style='" + cellStyleNumber + @"'>" + block.ClassB_Total_Cnt.ToString() + @"</td>
                             <td style='" + cellStyleNumber + @"'>" + block.ClassC_M_Cnt.ToString() + @"</td>
                             <td style='" + cellStyleNumber + @"'>" + block.ClassC_F_Cnt.ToString() + @"</td>
                             <td style='" + cellStyleNumber + @"'>" + block.ClassC_Total_Cnt.ToString() + @"</td>
                             <td style='" + cellStyleNumber + @"'>" + block.Total_M_Cnt.ToString() + @"</td>
                             <td style='" + cellStyleNumber + @"'>" + block.Total_F_Cnt.ToString() + @"</td>
                             <td style='" + cellStyleNumber + @"'>" + block.Total_Total_Cnt.ToString() + @"</td>
                             <td style='" + cellStyleNumber + @"'>" + block.WithoutMilitary_M_Cnt.ToString() + @"</td>
                             <td style='" + cellStyleNumber + @"'>" + block.WithoutMilitary_F_Cnt.ToString() + @"</td>
                             <td style='" + cellStyleNumber + @"'>" + block.WithoutMilitary_Total_Cnt.ToString() + @"</td>
                          </tr>";
                counter++;
            }

            if (listBlocks.Count() > 0)
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
            Response.AppendHeader("content-disposition", "attachment; filename=RatedApplicantsSummary.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        // Generate html as excel response result
        private string GenerateContentForExport()
        {
            ReportRatedApplicantsSummaryFilter filter = new ReportRatedApplicantsSummaryFilter()
            {
                VacancyAnnounceId = vacancyAnnounceId,
                MilitaryUnitId = militaryUnitId,
                Position = position,
                MilitaryDepartmentId = militaryDepartmentId,
                Status = status,
                PageIndex = 0
            };

            //Get the list of records according to the specified filters and order
            List<ReportRatedApplicantsSummaryBlock> listBlocks = ReportRatedApplicantsSummaryUtil.GetReportRatedApplicantsSummarySearch(filter, 0, CurrentUser);

            VacancyAnnounce vacancyAnnounce = VacancyAnnounceUtil.GetVacancyAnnounce((int)filter.VacancyAnnounceId, CurrentUser);

            MilitaryUnit militaryUnit = null;
            if (filter.MilitaryUnitId != null)
            {
                militaryUnit = MilitaryUnitUtil.GetMilitaryUnit((int)militaryUnitId, CurrentUser);
            }

            MilitaryDepartment militaryDepartment = null;
            if (filter.MilitaryDepartmentId != null)
            {
                militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment((int)filter.MilitaryDepartmentId, CurrentUser);
            }

            ApplicantPositionStatus applicantPositionStatus = null;
            ApplicantExamStatus applicantExamStatus = null;
            string statusName = "";
            if (!String.IsNullOrEmpty(filter.Status))
            {
                applicantPositionStatus = ApplicantPositionStatusUtil.GetApplicantPositionStatusByKey(filter.Status, CurrentUser);
                applicantExamStatus = ApplicantExamStatusUtil.GetApplicantExamStatusByKey(filter.Status, CurrentUser);

                if (applicantPositionStatus != null)
                    statusName = applicantPositionStatus.StatusName;
                else if (applicantExamStatus != null)
                    statusName = applicantExamStatus.StatusName;
            }

            string html = @"<html xmlns='http://www.w3.org/1999/xhtml' xml:lang='en' xmlns:x='urn:schemas-microsoft-com:office:excel'>
                            <head>
                                <meta http-equiv='content-type' content='application/xhtml+xml; charset=UTF-8' />
                                <style>
                                   .TableHeaderCell
                                   {
                                       border: solid thin #000000;
                                       vertical-align: bottom;
                                   }

                                   .TableDataCell
                                   {
                                       border: solid thin #000000;
                                   }
                                </style> 
                            </head>
                            <body>
                                <table>
                                    <tr><td align='center' colspan='" + visibleColumnsCount + @"' style='font-weight: bold; font-size: 1.6em;'>АСУ на човешките ресурси</td></tr>
                                    <tr><td align='center' colspan='" + visibleColumnsCount + @"' style='font-weight: bold; font-size: 2em;'>Кандидати</td></tr>
                                    <tr><td colspan='" + visibleColumnsCount + @"'>&nbsp;</td></tr>
                                    <tr><td align='center' colspan='" + visibleColumnsCount + @"' style='font-weight: bold; font-size: 1.3em;'>Сведение за класираните кандидати</td></tr>
                                    <tr><td colspan='" + visibleColumnsCount + @"'>&nbsp;</td></tr>
                                    <tr>
                                        <td align='right' colspan='3'>
                                            <span style='font-weight: normal;'>Заповед №:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='5'>
                                            <span style='font-weight: bold;'>" + vacancyAnnounce.OrderNumOrderDate + @"</span>
                                        </td>
                                        <td align='right' colspan='3'>
                                            <span style='font-weight: normal;'>" + MilitaryUnitLabel + @":&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='5'>
                                            <span style='font-weight: bold;'>" + (militaryUnit != null ? militaryUnit.DisplayTextForSelection : All) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='3'>
                                            <span style='font-weight: normal;'>Длъжност:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='5'>
                                            <span style='font-weight: bold;'>" + (!String.IsNullOrEmpty(filter.Position) ? filter.Position : All) + @"</span>
                                        </td>
                                        <td align='right' colspan='3'>
                                            <span style='font-weight: normal;'>Място на регистрация:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='5'>
                                            <span style='font-weight: bold;'>" + (militaryDepartment != null ? militaryDepartment.MilitaryDepartmentName : All) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='3'>
                                            <span style='font-weight: normal;'>Състояние:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='5'>
                                            <span style='font-weight: bold;'>" + (!String.IsNullOrEmpty(statusName) ? statusName : All) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style='width: 30px;'>&nbsp;</td>
                                    </tr>
                                </table>";


            if (listBlocks.Count() > 0)
            {
                html += @"
                    <table>
                        <thead>
                            <tr>
                                <th class='TableHeaderCell' rowspan='2'>Военно формирование</th>
                                <th class='TableHeaderCell' colspan='3'>до 35 години</th>
                                <th class='TableHeaderCell' colspan='3'>до 45 години</th>
                                <th class='TableHeaderCell' colspan='3'>над 45 години</th>
                                <th class='TableHeaderCell' colspan='3'>Всичко</th>
                                <th class='TableHeaderCell' colspan='3'>без военна подготовка</th>
                            </tr>
                            <tr>
                                <th class='TableHeaderCell' style='width: 60px;'>М</th>
                                <th class='TableHeaderCell' style='width: 60px;'>Ж</th>
                                <th class='TableHeaderCell' style='width: 60px;'>Общо</th>
                                <th class='TableHeaderCell' style='width: 60px;'>М</th>
                                <th class='TableHeaderCell' style='width: 60px;'>Ж</th>
                                <th class='TableHeaderCell' style='width: 60px;'>Общо</th>
                                <th class='TableHeaderCell' style='width: 60px;'>М</th>
                                <th class='TableHeaderCell' style='width: 60px;'>Ж</th>
                                <th class='TableHeaderCell' style='width: 60px;'>Общо</th>
                                <th class='TableHeaderCell' style='width: 60px;'>М</th>
                                <th class='TableHeaderCell' style='width: 60px;'>Ж</th>
                                <th class='TableHeaderCell' style='width: 60px;'>Общо</th>
                                <th class='TableHeaderCell' style='width: 60px;'>М</th>
                                <th class='TableHeaderCell' style='width: 60px;'>Ж</th>
                                <th class='TableHeaderCell' style='width: 60px;'>Общо</th>
                            </tr>
                        </thead>
                    <tbody>";


                foreach (ReportRatedApplicantsSummaryBlock block in listBlocks)
                {
                    string cellStyleText = "vertical-align: top;";
                    string cellStyleNumber = @"vertical-align: top; text-align: right; mso-number-format: ""0""";

                    if (block.RowType == 3)
                        cellStyleText += " text-align: right;";

                    html += @"
                            <tr>
                                <td class='TableDataCell' style='" + cellStyleText + @"' >" + block.DisplayText + @"</td>
                                <td class='TableDataCell' style='" + cellStyleNumber + @"' x:num >" + block.ClassA_M_Cnt.ToString() + @"</td>
                                <td class='TableDataCell' style='" + cellStyleNumber + @"' x:num >" + block.ClassA_F_Cnt.ToString() + @"</td>
                                <td class='TableDataCell' style='" + cellStyleNumber + @"' x:num >" + block.ClassA_Total_Cnt.ToString() + @"</td>
                                <td class='TableDataCell' style='" + cellStyleNumber + @"' x:num >" + block.ClassB_M_Cnt.ToString() + @"</td>
                                <td class='TableDataCell' style='" + cellStyleNumber + @"' x:num >" + block.ClassB_F_Cnt.ToString() + @"</td>
                                <td class='TableDataCell' style='" + cellStyleNumber + @"' x:num >" + block.ClassB_Total_Cnt.ToString() + @"</td>
                                <td class='TableDataCell' style='" + cellStyleNumber + @"' x:num >" + block.ClassC_M_Cnt.ToString() + @"</td>
                                <td class='TableDataCell' style='" + cellStyleNumber + @"' x:num >" + block.ClassC_F_Cnt.ToString() + @"</td>
                                <td class='TableDataCell' style='" + cellStyleNumber + @"' x:num >" + block.ClassC_Total_Cnt.ToString() + @"</td>
                                <td class='TableDataCell' style='" + cellStyleNumber + @"' x:num >" + block.Total_M_Cnt.ToString() + @"</td>
                                <td class='TableDataCell' style='" + cellStyleNumber + @"' x:num >" + block.Total_F_Cnt.ToString() + @"</td>
                                <td class='TableDataCell' style='" + cellStyleNumber + @"' x:num >" + block.Total_Total_Cnt.ToString() + @"</td>
                                <td class='TableDataCell' style='" + cellStyleNumber + @"' x:num >" + block.WithoutMilitary_M_Cnt.ToString() + @"</td>
                                <td class='TableDataCell' style='" + cellStyleNumber + @"' x:num >" + block.WithoutMilitary_F_Cnt.ToString() + @"</td>
                                <td class='TableDataCell' style='" + cellStyleNumber + @"' x:num >" + block.WithoutMilitary_Total_Cnt.ToString() + @"</td>
                            </tr>";
                }

                html += "</tbody></table>";
            }

            html += "</body></html>";

            return html;
        }
    }
}
