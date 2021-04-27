using System;
using System.Linq;
using System.Text;

using PMIS.Common;
using PMIS.Applicants.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace PMIS.Applicants.PrintContentPages
{
    public partial class PrintAllVacancyAnnounces : APPLPage
    {
        const string All = "Всички";

        string orderNumber = null;
        DateTime? orderDateFrom = null;
        DateTime? orderDateTo = null;
        int? vacAnnStatusId = null;
        DateTime? endDateFrom = null;
        DateTime? endDateTo = null;

        int sortBy = 1; // 1 - Default

        public void SetPrintTitleLinesWidth()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnPrintTitleLinesWidth");

            hf.Value = "720";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.SetPrintTitleLinesWidth();
            }

            // Check visibility right for the print screen
            if (this.GetUIItemAccessLevel("APPL_VACANN") != UIAccessLevel.Hidden)
            {
                if (!String.IsNullOrEmpty(Request.Params["OrderNum"]))
                {
                    orderNumber = Request.Params["OrderNum"].ToString();
                }

                if (!String.IsNullOrEmpty(Request.Params["OrderDateFrom"]))
                {
                    orderDateFrom = CommonFunctions.ParseDate(Request.Params["OrderDateFrom"]);
                }

                if (!String.IsNullOrEmpty(Request.Params["OrderDateTo"]))
               {
                    orderDateTo = CommonFunctions.ParseDate(Request.Params["OrderDateTo"]);
                }

                if (!String.IsNullOrEmpty(Request.Params["VacAnnStatus"]) && Request.Params["VacAnnStatus"] != "-1")
                {
                    vacAnnStatusId = int.Parse(Request.Params["VacAnnStatus"]);
                }

                if (!String.IsNullOrEmpty(Request.Params["EndDateFrom"]))
                {
                    endDateFrom = CommonFunctions.ParseDate(Request.Params["EndDateFrom"]);
                }

                if (!String.IsNullOrEmpty(Request.Params["EndDateTo"]))
                {
                    endDateTo = CommonFunctions.ParseDate(Request.Params["EndDateTo"]);
                }

                if (!String.IsNullOrEmpty(Request.Params["SortBy"]))
                {
                    int.TryParse(Request.Params["SortBy"], out sortBy);
                }

                this.divResults.InnerHtml = GeneratePageContent(false);
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
                sb.Append("<td rowspan=\"2\">" + this.GenerateAllVacancyAnnouncesHtml() + "</td>");
                sb.Append("<td style=\"vertical-align: top;\">" + CommonFunctions.GenerateButtons(true, 160, true) + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr><td style=\"vertical-align: bottom;\">" + CommonFunctions.GenerateButtons(false, 0, true) + "</td></tr>");
                sb.Append("</table>");

                contentPage = sb.ToString();
            }
            else
            {
                contentPage = this.GenerateAllVacancyAnnouncesForExport();
            }

            return contentPage;
        }

        // Generate display data as html into the page
        private string GenerateAllVacancyAnnouncesHtml()
        {
            VacancyAnnouncesFilter filter = new VacancyAnnouncesFilter()
            {
                OrderNum = orderNumber,
                OrderDateFrom = orderDateFrom,
                OrderDateTo = orderDateTo,
                VacancyAnnounceStatuses = vacAnnStatusId.HasValue ? vacAnnStatusId.ToString() : "",
                EndDateFrom = endDateFrom,
                EndDateTo = endDateTo,
                OrderBy = sortBy,
                PageIdx = 0
            };

            //Get the list of vacancy announces according to the specified filters and order
            List<VacancyAnnounce> vacancyAnnounces = VacancyAnnounceUtil.GetAllVacancyAnnounces(filter, 0, CurrentUser);

            VacancyAnnounceStatus vacancyAnnounceStatus = null;
            if (vacAnnStatusId != null)
            {
                vacancyAnnounceStatus = VacancyAnnounceStatusUtil.GetVacancyAnnounceStatus((int)vacAnnStatusId, CurrentUser);
            }

            string html = @"<table style='padding: 5px;'>
                             <tr>
                                <td align='right' style='width: 100px;'>
                                    <span class='Label'>Заповед №:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 195px;'>
                                   <span class='ValueLabel'>" + (!String.IsNullOrEmpty(orderNumber) ? orderNumber.ToString() : "") + @"</span>
                                </td>
                                <td align='right' style='width: 200px;'>
                                    <span class='Label'>Дата заповед от:&nbsp;</span>
                                    <span class='ValueLabel'>" + CommonFunctions.FormatDate(orderDateFrom) + @"</span>
                                </td>
                                <td align='left' style='width: 195px;'>
                                    <span class='Label'>до:&nbsp;</span>
                                    <span class='ValueLabel'>" + CommonFunctions.FormatDate(orderDateTo) + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' style='width: 100px;'>
                                    <span class='Label'>Статус:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 195px;'>
                                   <span class='ValueLabel'>" + (vacancyAnnounceStatus != null ? vacancyAnnounceStatus.VacAnnStatusName : All) + @"</span>
                                </td>
                                <td align='right' style='width: 200px;'>
                                    <span class='Label'>Крайна дата от:&nbsp;</span>
                                    <span class='ValueLabel'>" + CommonFunctions.FormatDate(endDateFrom) + @"</span>
                                </td>
                                <td align='left' style='width: 195px;'>
                                    <span class='Label'>до:&nbsp;</span>
                                    <span class='ValueLabel'>" + CommonFunctions.FormatDate(endDateTo) + @"</span>
                                </td>
                             </tr>";

            if (vacancyAnnounces.Count() > 0)
            {
                html += @"
                    <tr><td colspan='4' align='center'>
                    <table id='applicantsTable' name='applicantsTable' class='CommonHeaderTable'>
                        <thead>
                            <tr>
                                <th style='width: 30px; border-left: 1px solid #000000;'>№</th>
                                <th style='width: 150px;'>Заповед №</th>
                                <th style='width: 80px;'>От дата</th>
                                <th style='width: 150px;'>Крайна дата (за подаване на документи)</th>
                                <th style='width: 70px;'>Брой длъжности</th>
                                <th style='width: 200px; border-right: 1px solid #000000;'>Статус</th>
                            </tr>
                        </thead><tbody>";
            }

            int counter = 1;

            foreach (VacancyAnnounce vacancyAnnounce in vacancyAnnounces)
            {
                html += @"<tr>
                            <td align='center'>" + counter + @"</td>
                            <td align='left'>" + vacancyAnnounce.OrderNum + @"</td>
                            <td align='left'>" + CommonFunctions.FormatDate(vacancyAnnounce.OrderDate) + @"</td>
                            <td align='left'>" + CommonFunctions.FormatDate(vacancyAnnounce.EndDate) + @"</td>
                            <td align='left'>" + vacancyAnnounce.MaxPositions.ToString() + @"</td>
                            <td align='left'>" + vacancyAnnounce.VacancyAnnounceStatus.VacAnnStatusName + @"</td>
                          </tr>";
                counter++;
            }

            if (vacancyAnnounces.Count() > 0)
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
            Response.AppendHeader("content-disposition", "attachment; filename=VacancyAnnounces.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        // Generate html as excel response result
        private string GenerateAllVacancyAnnouncesForExport()
        {
            VacancyAnnouncesFilter filter = new VacancyAnnouncesFilter()
            {
                OrderNum = orderNumber,
                OrderDateFrom = orderDateFrom,
                OrderDateTo = orderDateTo,
                VacancyAnnounceStatuses = vacAnnStatusId.HasValue ? vacAnnStatusId.ToString() : "",
                EndDateFrom = endDateFrom,
                EndDateTo = endDateTo,
                OrderBy = sortBy,
                PageIdx = 0
            };

            //Get the list of vacancy announces according to the specified filters and order
            List<VacancyAnnounce> vacancyAnnounces = VacancyAnnounceUtil.GetAllVacancyAnnounces(filter, 0, CurrentUser);

            VacancyAnnounceStatus vacancyAnnounceStatus = null;
            if (vacAnnStatusId != null)
            {
                vacancyAnnounceStatus = VacancyAnnounceStatusUtil.GetVacancyAnnounceStatus((int)vacAnnStatusId, CurrentUser);
            }

            string html = @"<html xmlns='http://www.w3.org/1999/xhtml' xml:lang='en'>
                            <head>
                                <meta http-equiv='content-type' content='application/xhtml+xml; charset=UTF-8' />
                            </head>
                            <body>
                                <table>
                                    <tr><td align='center' colspan='6' style='font-weight: bold; font-size: 1.6em;'>АСУ на човешките ресурси</td></tr>
                                    <tr><td align='center' colspan='6' style='font-weight: bold; font-size: 2em;'>Кандидати</td></tr>
                                    <tr><td colspan='6'>&nbsp;</td></tr>
                                    <tr><td align='center' colspan='6' style='font-weight: bold; font-size: 1.3em;'>Списък на обявените конкурси</td></tr>
                                    <tr><td colspan='6'>&nbsp;</td></tr>
                                    <tr>
                                        <td align='center' colspan='6'>
                                            <span style='font-weight: normal;'>Заповед №:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + (!String.IsNullOrEmpty(orderNumber) ? orderNumber.ToString() : "") + @"</span>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <span style='font-weight: normal;'>Дата заповед от:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + CommonFunctions.FormatDate(orderDateFrom) + @"</span>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <span style='font-weight: normal;'>до:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + CommonFunctions.FormatDate(orderDateTo) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='center' colspan='6'>
                                            <span style='font-weight: normal;'>Статус:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + (vacancyAnnounceStatus != null ? vacancyAnnounceStatus.VacAnnStatusName : All) + @"</span>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <span style='font-weight: normal;'>Крайна дата от:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + CommonFunctions.FormatDate(endDateFrom) + @"</span>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <span style='font-weight: normal;'>до:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + CommonFunctions.FormatDate(endDateTo) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style='width: 30px;'>&nbsp;</td>
                                        <td style='width: 150px;'>&nbsp;</td>
                                        <td style='width: 100px;'>&nbsp;</td>
                                        <td style='width: 150px;'>&nbsp;</td>
                                        <td style='width: 70px;'>&nbsp;</td>
                                        <td style='width: 200px;'>&nbsp;</td>
                                    </tr>
                                </table>";


            if (vacancyAnnounces.Count() > 0)
            {
                html += @"
                    <table>
                        <thead>
                            <tr>
                                <th style='width: 30px; border-bottom: 1px solid white; background-color: black; color: #FFFFFF;'>№</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Заповед №</th>
                                <th style='width: 100px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>От дата</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Крайна дата (за подаване на документи)</th>
                                <th style='width: 70px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Брой длъжности</th>
                                <th style='width: 200px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Статус</th>
                            </tr>
                        </thead><tbody>";

                int counter = 1;

                foreach (VacancyAnnounce vacancyAnnounce in vacancyAnnounces)
                {
                    html += @"
                            <tr>
                                <td align='center' style='border: 1px solid black;'>" + counter + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + vacancyAnnounce.OrderNum + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + CommonFunctions.FormatDate(vacancyAnnounce.OrderDate) + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + CommonFunctions.FormatDate(vacancyAnnounce.EndDate) + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + vacancyAnnounce.MaxPositions.ToString() + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + vacancyAnnounce.VacancyAnnounceStatus.VacAnnStatusName + @"</td>
                              </tr>";
                    counter++;
                }

                html += "</tbody></table>";
            }

            html += "</body></html>";

            return html;
        }
    }
}
