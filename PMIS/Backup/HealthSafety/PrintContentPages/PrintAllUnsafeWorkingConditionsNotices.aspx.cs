using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Text;

using PMIS.Common;
using PMIS.HealthSafety.Common;
using System.Collections.Generic;

namespace PMIS.HealthSafety.PrintContentPages
{
    public partial class PrintAllUnsafeWorkingConditionsNotices : HSPage
    {
        int? militaryUnitId = null;
        string noticeNumber = null;
        DateTime? dateFrom = null;
        DateTime? dateTo = null;
        int sortBy = 1; // 1 - Default

        private string MilitaryUnitLabel = CommonFunctions.GetLabelText("MilitaryUnit");

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check visibility right for the print screen
            if (this.GetUIItemAccessLevel("HS_UNSAFEWCONDNOTICE") != UIAccessLevel.Hidden)
            {
                if (!String.IsNullOrEmpty(Request.Params["MilitaryUnitID"]) && Request.Params["MilitaryUnitID"] != "-1")
                {
                    int mUnitId = 0;
                    int.TryParse(Request.Params["MilitaryUnitID"], out mUnitId);
                    if (mUnitId > 0)
                    {
                        militaryUnitId = mUnitId;
                    }
                }

                if (!String.IsNullOrEmpty(Request.Params["NoticeNumber"]))
                {
                    noticeNumber = Request.Params["NoticeNumber"];
                }

                if (!String.IsNullOrEmpty(Request.Params["DateFrom"]) && CommonFunctions.TryParseDate(Request.Params["DateFrom"]))
                {
                    dateFrom = CommonFunctions.ParseDate(Request.Params["DateFrom"]);
                }

                if (!String.IsNullOrEmpty(Request.Params["DateTo"]) && CommonFunctions.TryParseDate(Request.Params["DateTo"]))
                {
                    dateTo = CommonFunctions.ParseDate(Request.Params["DateTo"]);
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
                sb.Append("<td rowspan=\"2\">" + this.GenerateAllUnsafeWCondNoticesHtml() + "</td>");
                sb.Append("<td style=\"vertical-align: top;\">" + CommonFunctions.GenerateButtons(true, 160, true) + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr><td style=\"vertical-align: bottom;\">" + CommonFunctions.GenerateButtons(false, 0, true) + "</td></tr>");
                sb.Append("</table>");

                contentPage = sb.ToString();
            }
            else
            {
                contentPage = this.GenerateAllUnsafeWCondNoticesForExport();
            }

            return contentPage;
        }

        // Generate display data as html into the page
        private string GenerateAllUnsafeWCondNoticesHtml()
        {
            //Get the list of notices according to the specified filters, order and paging
            List<UnsafeWorkingConditionsNotice> unsafeWConditionsNotices = UnsafeWorkingConditionsNoticeUtil.GetAllUnsafeWorkingConditionsNotices(noticeNumber, militaryUnitId, dateFrom, dateTo, sortBy, 0, 0, CurrentUser);

            MilitaryUnit militaryUnit = null;
            if (militaryUnitId != null)
            {
                militaryUnit = MilitaryUnitUtil.GetMilitaryUnit((int)militaryUnitId, CurrentUser);
            }

            string html = @"<table style='padding: 5px;'>
                             <tr>
                                <td align='right' style='width: 140px;'>
                                    <span class='Label'>" + this.MilitaryUnitLabel + @":&nbsp;</span>
                                </td>
                                <td align='left' style='width: 195px;'>
                                   <span class='ValueLabel'>" + (militaryUnit != null ? militaryUnit.DisplayTextForSelection : "") + @"</span>
                                </td>
                                <td align='right' style='width: 140px;'>
                                    <span class='Label'>Сведение №:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 195px;'>
                                    <span class='ValueLabel'>" + (noticeNumber != null ? noticeNumber : "") + @"</span>
                                </td>
                             </tr>
                             <tr>                
                                <td align='right' style='width: 140px;'>
                                    <span class='Label'>Дата от:&nbsp;</span>
                                </td>
                                <td align='left' colspan='3'>
                                    <span class='ValueLabel'>" + (dateFrom != null ? CommonFunctions.FormatDate(dateFrom.ToString()) : "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + @"</span>
                                    <span class='Label'>&nbsp;&nbsp;&nbsp;до:&nbsp;</span>
                                   <span class='ValueLabel'>" + (dateTo != null ? CommonFunctions.FormatDate(dateTo.ToString()) : "") + @"</span>
                                </td>
                             </tr>";

            if (unsafeWConditionsNotices.Count() > 0)
            {
                html += @"
                    <tr><td colspan='4' align='center'>
                    <table id='noticesTable' name='noticesTable' class='CommonHeaderTable'>
                        <thead>
                            <tr>
                                <th style='width: 30px; border-left: 1px solid #000000;'>№</th>
                                <th style='width: 476px;'>Сведение №</th>
                                <th style='width: 80px;'>Дата на изготвяне</th>
                                <th style='width: 80px; border-right: 1px solid #000000;'>" + this.MilitaryUnitLabel + @"</th>
                            </tr>
                        </thead><tbody>";
            }

            int counter = 1;

            foreach (UnsafeWorkingConditionsNotice unsafeWConditionsNotice in unsafeWConditionsNotices)
            {
                html += @"<tr>
                            <td align='center'>" + counter + @"</td>
                            <td align='left'>" + unsafeWConditionsNotice.NoticeNumber + @"</td>
                            <td>" + CommonFunctions.FormatDate(unsafeWConditionsNotice.NoticeDate) + @"</td>
                            <td>" + (unsafeWConditionsNotice.MilitaryUnit != null ? unsafeWConditionsNotice.MilitaryUnit.DisplayTextForSelection : "") + @"</td>
                          </tr>";
                counter++;
            }

            if (unsafeWConditionsNotices.Count() > 0)
            {
                html += "</tbody></table>";
            }

            html += "</table>";

            return html;
        }

        // Handles export to excel button click
        protected void btnGenerateExcel_Click(object sender, EventArgs e)
        {
            string result = this.GeneratePageContent(true);
            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment; filename=UnsafeWorkingConditionsNotices.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        // Generate html as excel response result
        private string GenerateAllUnsafeWCondNoticesForExport()
        {
            //Get the list of notices according to the specified filters, order and paging
            List<UnsafeWorkingConditionsNotice> noticeWCondNotices = UnsafeWorkingConditionsNoticeUtil.GetAllUnsafeWorkingConditionsNotices(noticeNumber, militaryUnitId, dateFrom, dateTo, sortBy, 0, 0, CurrentUser);

            MilitaryUnit militaryUnit = null;
            if (militaryUnitId != null)
            {
                militaryUnit = MilitaryUnitUtil.GetMilitaryUnit((int)militaryUnitId, CurrentUser);
            }

            string html = @"<html xmlns='http://www.w3.org/1999/xhtml' xml:lang='en'>
                            <head>
                                <meta http-equiv='content-type' content='application/xhtml+xml; charset=UTF-8' />
                            </head>
                            <body>
                                <table>
                                    <tr><td align='center' colspan='4' style='font-weight: bold; font-size: 1.6em;'>АСУ на човешките ресурси</td></tr>
                                    <tr><td align='center' colspan='4' style='font-weight: bold; font-size: 2em;'>Безопасност на труда</td></tr>
                                    <tr><td colspan='4'>&nbsp;</td></tr>
                                    <tr><td align='center' colspan='4' style='font-weight: bold; font-size: 1.3em;'>Сведения за заболявания и наранявания свързани с работата</td></tr>
                                    <tr>
                                        <td align='right' style='width: 140px;'>
                                            <span style='font-weight: normal;'>" + this.MilitaryUnitLabel + @":&nbsp;</span>
                                        </td>
                                        <td align='left' style='width: 195px;'>
                                           <span style='font-weight: bold;'>" + (militaryUnit != null ? militaryUnit.DisplayTextForSelection : "") + @"</span>
                                        </td>
                                        <td align='right' style='width: 140px;'>
                                            <span style='font-weight: normal;'>Сведение №:&nbsp;</span>
                                        </td>
                                        <td align='left' style='width: 195px;'>
                                            <span style='font-weight: bold;'>" + (noticeNumber != null ? noticeNumber : "") + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' style='width: 140px;'>
                                            <span style='font-weight: normal;'>Дата от:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='3'>
                                            <span style='font-weight: bold;'>" + (dateFrom != null ? CommonFunctions.FormatDate(dateFrom.ToString()) : "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + @"</span>
                                            <span style='font-weight: normal;'>&nbsp;&nbsp;&nbsp;до:&nbsp;</span>
                                           <span style='font-weight: bold;'>" + (dateTo != null ? CommonFunctions.FormatDate(dateTo.ToString()) : "") + @"</span>
                                        </td>
                                    </tr>
                                </table>";


            if (noticeWCondNotices.Count() > 0)
            {
                html += @"
                    <table>
                        <thead>
                            <tr>
                                <th style='width: 140px; border-bottom: 1px solid white; background-color: black; color: #FFFFFF;'>№</th>
                                <th style='width: 195px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Сведение №</th>
                                <th style='width: 140px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Дата на изготвяне</th>
                                <th style='width: 195px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>" + this.MilitaryUnitLabel + @"</th>
                            </tr>
                        </thead><tbody>";

                int counter = 1;

                foreach (UnsafeWorkingConditionsNotice unsafeWConditionsNotice in noticeWCondNotices)
                {
                    html += @"<tr>
                                <td align='center' style='border: 1px solid black;'>" + counter + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + unsafeWConditionsNotice.NoticeNumber + @"</td>
                                <td style='border: 1px solid black;'>" + CommonFunctions.FormatDate(unsafeWConditionsNotice.NoticeDate) + @"</td>
                                <td style='border: 1px solid black;'>" + (unsafeWConditionsNotice.MilitaryUnit != null ? unsafeWConditionsNotice.MilitaryUnit.DisplayTextForSelection : "") + @"</td>
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
