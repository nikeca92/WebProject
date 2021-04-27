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
    public partial class PrintAllDeclarationOfAccidents : HSPage
    {
        string declarNumber = null;
        string workerName = null;
        DateTime? dateFrom = null;
        DateTime? dateTo = null;
        int sortBy = 1; // 1 - Default

        DeclarationOfAccidentFilter declarationOfAccidentFilter;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check visibility right for the print screen
            if (this.GetUIItemAccessLevel("HS_DECLARATIONACC") != UIAccessLevel.Hidden)
            {
                if (!String.IsNullOrEmpty(Request.Params["DeclarNumber"]))
                {
                    declarNumber = Request.Params["DeclarNumber"];
                }

                if (!String.IsNullOrEmpty(Request.Params["WorkerName"]))
                {
                    workerName = Request.Params["WorkerName"];
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

                //create filter object
                declarationOfAccidentFilter = new DeclarationOfAccidentFilter();
                //initialize fields in object
                declarationOfAccidentFilter.DeclarationNumber = declarNumber;
                declarationOfAccidentFilter.WorkerFullName = workerName;
                declarationOfAccidentFilter.DeclarationDateFrom = dateFrom;
                declarationOfAccidentFilter.DeclarationDateTo = dateTo;
                declarationOfAccidentFilter.OrderBy = sortBy;
                declarationOfAccidentFilter.PageCount = 0;
                declarationOfAccidentFilter.PageIndex = 0;

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
                sb.Append("<td rowspan=\"2\">" + this.GenerateAllDeclarationsOfAccidentsHtml() + "</td>");
                sb.Append("<td style=\"vertical-align: top;\">" + CommonFunctions.GenerateButtons(true, 160, true) + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr><td style=\"vertical-align: bottom;\">" + CommonFunctions.GenerateButtons(false, 0, true) + "</td></tr>");
                sb.Append("</table>");

                contentPage = sb.ToString();
            }
            else
            {
                contentPage = this.GenerateAllDeclarationsOfAccidentForExport();
            }

            return contentPage;
        }

        // Generate display data as html into the page
        private string GenerateAllDeclarationsOfAccidentsHtml()
        {
            //Get the list of Investigation Protocols according to the specified filters, order and paging
            List<DeclarationOfAccident> declarationsOfAccidentList = DeclarationOfAccidentUtil.GetAllDeclarationOfAccident(declarationOfAccidentFilter, CurrentUser);

            string html = @"<table style='padding: 5px;'>
                             <tr>
                                <td align='right' style='width: 120px;'>
                                    <span class='Label'>№ Декларация:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 550px;'>
                                    <span class='ValueLabel'>" + (String.IsNullOrEmpty(declarNumber) ? "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" : (declarNumber + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;")) + @"</span>
                                    <span class='Label'>Име на пострадалия:&nbsp;</span>
                                    <span class='ValueLabel'>" + (String.IsNullOrEmpty(workerName) ? "" : workerName) + @"</span>
                                </td>
                             </tr>
                             <tr>                
                                <td align='left' colspan='2'>
                                    <span class='Label'>&nbsp;&nbsp;&nbsp;&nbsp;Дата на декларацията от:&nbsp;</span>
                                    <span class='ValueLabel'>" + (dateFrom != null ? CommonFunctions.FormatDate(dateFrom.ToString()) : "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + @"</span>
                                    <span class='Label'>&nbsp;&nbsp;&nbsp;до:&nbsp;</span>
                                   <span class='ValueLabel'>" + (dateTo != null ? CommonFunctions.FormatDate(dateTo.ToString()) : "") + @"</span>
                                </td>
                             </tr>";

            if (declarationsOfAccidentList.Count() > 0)
            {
                html += @"
                    <tr><td colspan='2' align='center'>
                    <table id='declarsTable' name='declarsTable' class='CommonHeaderTable'>
                        <thead>
                            <tr>
                                <th style='width: 30px; border-left: 1px solid #000000;'>№</th>
                                <th style='width: 276px;'>Декларация №</th>
                                <th style='width: 80px;'>Дата</th>
                                <th style='width: 280px; border-right: 1px solid #000000;'>Име на пострадал</th>
                            </tr>
                        </thead><tbody>";
            }

            int counter = 1;

            foreach (DeclarationOfAccident declarationOfAccident in declarationsOfAccidentList)
            {
                html += @"<tr>
                            <td align='center'>" + counter + @"</td>
                            <td align='left'>" + declarationOfAccident.DeclarationOfAccidentHeader.DeclarationNumber + @"</td>
                            <td align='left'>" + (declarationOfAccident.DeclarationOfAccidentHeader.DeclarationDate.HasValue ? CommonFunctions.FormatDate(declarationOfAccident.DeclarationOfAccidentHeader.DeclarationDate.Value) : "") + @"</td>
                            <td align='left'>" + declarationOfAccident.DeclarationOfAccidentWorker.WorkerFullName + @"</td>
                          </tr>";
                counter++;
            }

            if (declarationsOfAccidentList.Count() > 0)
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
            Response.AppendHeader("content-disposition", "attachment; filename=DeclarationOfAccidents.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        // Generate html as excel response result
        private string GenerateAllDeclarationsOfAccidentForExport()
        {
            //Get the list of Investigation Protocols according to the specified filters, order and paging
            List<DeclarationOfAccident> declarationsOfAccidentList = DeclarationOfAccidentUtil.GetAllDeclarationOfAccident(declarationOfAccidentFilter, CurrentUser);

            string html = @"<html xmlns='http://www.w3.org/1999/xhtml' xml:lang='en'>
                            <head>
                                <meta http-equiv='content-type' content='application/xhtml+xml; charset=UTF-8' />
                            </head>
                            <body>
                            <table>
                            <tr><td align='center' colspan='4' style='font-weight: bold; font-size: 1.6em;'>АСУ на човешките ресурси</td></tr>
                            <tr><td align='center' colspan='4' style='font-weight: bold; font-size: 2em;'>Безопасност на труда</td></tr>
                            <tr><td colspan='4'>&nbsp;</td></tr>
                            <tr><td align='center' colspan='4' style='font-weight: bold; font-size: 1.3em;'>Декларации за злополука</td></tr>
                             <tr>
                                <td align='left' colspan='4' style='vertical-align: top;'>
                                    <span style='font-weight: normal;'>№ Декларация:&nbsp;</span>
                                    <span style='font-weight: bold;'>" + (String.IsNullOrEmpty(declarNumber) ? "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" : (declarNumber + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;")) + @"</span>
                                    <span style='font-weight: normal;'>Име на пострадалия:&nbsp;</span>
                                    <span style='font-weight: bold;'>" + (String.IsNullOrEmpty(workerName) ? "&nbsp;" : workerName) + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='left' colspan='4' style='vertical-align: top;'>
                                    <span style='font-weight: normal;'>Дата на декларацията от:&nbsp;</span>
                                    <span style='font-weight: bold;'>" + (dateFrom != null ? CommonFunctions.FormatDate(dateFrom.ToString()) : "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + @"</span>
                                    <span style='font-weight: normal;'>&nbsp;&nbsp;&nbsp;до:&nbsp;</span>
                                    <span style='font-weight: bold;'>" + (dateTo != null ? CommonFunctions.FormatDate(dateTo.ToString()) : "&nbsp;") + @"</span>
                                 </td>
                             </tr>";

            if (declarationsOfAccidentList.Count() > 0)
            {
                html += @"
                    <tr>
                        <td align='center' style='width: 30px; border-bottom: 1px solid white; background-color: black; color: #FFFFFF;'>№</td>
                        <td align='center' style='width: 276px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Декларация №</td>
                        <td align='center' style='width: 80px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Дата</td>
                        <td align='center' style='width: 280px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Име на пострадал</td>
                    </tr>";
            }

            int counter = 1;

            foreach (DeclarationOfAccident declarationOfAccident in declarationsOfAccidentList)
            {
                html += @"<tr>
                            <td align='center' style='border: 1px solid black;'>" + counter + @"</td>
                            <td align='left' style='border: 1px solid black;'>" + declarationOfAccident.DeclarationOfAccidentHeader.DeclarationNumber + @"</td>
                            <td align='left' style='border: 1px solid black;'>" + (declarationOfAccident.DeclarationOfAccidentHeader.DeclarationDate.HasValue ? CommonFunctions.FormatDate(declarationOfAccident.DeclarationOfAccidentHeader.DeclarationDate.Value) : "&nbsp;") + @"</td>
                            <td align='left' style='border: 1px solid black;'>" + declarationOfAccident.DeclarationOfAccidentWorker.WorkerFullName + @"</td>
                          </tr>";
                counter++;
            }

            html += "</table>";

            html += "</body></html>";

            return html;
        }
    }
}
