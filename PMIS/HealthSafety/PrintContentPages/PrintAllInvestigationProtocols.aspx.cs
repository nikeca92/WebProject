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
    public partial class PrintAllInvestigationProtocols : HSPage
    {
        string investProtNumber = null;
        string injuredName = null;
        DateTime? protocolDateFrom = null;
        DateTime? protocolDateTo = null;
        DateTime? accidentDateFrom = null;
        DateTime? accidentDateTo = null;
        int sortBy = 1; // 1 - Default

        InvestigationProtocolFilter investigationProtocolFilter;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check visibility right for the print screen
            if (this.GetUIItemAccessLevel("HS_INVPROTOCOLS") != UIAccessLevel.Hidden)
            {
                if (!String.IsNullOrEmpty(Request.Params["InvestProtocolNumber"]))
                {
                    investProtNumber = Request.Params["InvestProtocolNumber"];
                }

                if (!String.IsNullOrEmpty(Request.Params["InjuredName"]))
                {
                    injuredName = Request.Params["InjuredName"];
                }

                if (!String.IsNullOrEmpty(Request.Params["ProtocolDateFrom"]) && CommonFunctions.TryParseDate(Request.Params["ProtocolDateFrom"]))
                {
                    protocolDateFrom = CommonFunctions.ParseDate(Request.Params["ProtocolDateFrom"]);
                }

                if (!String.IsNullOrEmpty(Request.Params["DateTo"]) && CommonFunctions.TryParseDate(Request.Params["DateTo"]))
                {
                    protocolDateTo = CommonFunctions.ParseDate(Request.Params["DateTo"]);
                }

                if (!String.IsNullOrEmpty(Request.Params["AccidentDateFrom"]) && CommonFunctions.TryParseDate(Request.Params["AccidentDateFrom"]))
                {
                    accidentDateFrom = CommonFunctions.ParseDate(Request.Params["AccidentDateFrom"]);
                }

                if (!String.IsNullOrEmpty(Request.Params["AccidentDateTo"]) && CommonFunctions.TryParseDate(Request.Params["AccidentDateTo"]))
                {
                    accidentDateTo = CommonFunctions.ParseDate(Request.Params["AccidentDateTo"]);
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
            investigationProtocolFilter = new InvestigationProtocolFilter()
            {
                InvestigaitonProtocolNumber = investProtNumber,
                WorkerFullName = injuredName,
                InvProtDateFrom = protocolDateFrom,
                InvProtDateTo = protocolDateTo,
                AccDateTimeFrom = accidentDateFrom,
                AccDateTimeTo = accidentDateTo,
                OrderBy = sortBy,
                PageCount = 0,
                PageIndex = 0
            };

            string contentPage = "";

            if (!isExport)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<table>");
                sb.Append("<tr>");
                sb.Append("<td rowspan=\"2\">" + this.GenerateAllInvestigationProtocolsHtml() + "</td>");
                sb.Append("<td style=\"vertical-align: top;\">" + CommonFunctions.GenerateButtons(true, 160, true) + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr><td style=\"vertical-align: bottom;\">" + CommonFunctions.GenerateButtons(false, 0, true) + "</td></tr>");
                sb.Append("</table>");

                contentPage = sb.ToString();
            }
            else
            {
                contentPage = this.GenerateAllInvestigationProtocolsForExport();
            }

            return contentPage;
        }

        // Generate display data as html into the page
        private string GenerateAllInvestigationProtocolsHtml()
        {
            //Get the list of Investigation Protocols according to the specified filters, order and paging
            List<InvestigationProtocol> investigationProtocols = InvestigationProtocolUtil.GetAllInvestigationProtocols(investigationProtocolFilter, CurrentUser);

            string html = @"<table style='padding: 5px;'>
                             <tr>
                                <td align='right' style='width: 165px;'>
                                    <span class='Label'>№ Протокол:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 100px;'>
                                    <span class='ValueLabel'>" + (!String.IsNullOrEmpty(investProtNumber) ? (investProtNumber.ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") : "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + @"</span>
                                </td>
                                <td align='left' style='width: 400px;'>
                                    <span class='Label'>Име на пострадалия:&nbsp;</span>
                                    <span class='ValueLabel'>" + (!String.IsNullOrEmpty(injuredName) ? injuredName.ToString() : "") + @"</span>
                                </td>
                             </tr>
                             <tr>                
                                <td align='right' style='width: 165px;'>
                                    <span class='Label'>Дата на протокола от:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 100px;'>
                                    <span class='ValueLabel'>" + (protocolDateFrom.HasValue ? CommonFunctions.FormatDate(protocolDateFrom) : "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + @"</span>
                                </td>
                                <td align='left' style='width: 400px;'>
                                    <span class='Label'>до:&nbsp;</span>
                                    <span class='ValueLabel'>" + (protocolDateTo.HasValue ? CommonFunctions.FormatDate(protocolDateTo) : "&nbsp;") + @"</span>
                                </td>
                             </tr>
                             <tr>                
                                <td align='right' style='width: 165px;'>
                                    <span class='Label'>Дата на злополуката от:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 100px;'>
                                    <span class='ValueLabel'>" + (accidentDateFrom.HasValue ? CommonFunctions.FormatDate(accidentDateFrom) : "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + @"</span>
                                </td>
                                <td align='left' style='width: 400px;'>
                                    <span class='Label'>до:&nbsp;</span>
                                    <span class='ValueLabel'>" + (accidentDateTo.HasValue ? CommonFunctions.FormatDate(accidentDateTo) : "&nbsp;") + @"</span>
                                </td>
                             </tr>";

            if (investigationProtocols.Count() > 0)
            {
                html += @"
                    <tr><td align='center' colspan='3'>
                    <table id='investProtocolsTable' name='investProtocolsTable' class='CommonHeaderTable'>
                        <thead>
                            <tr>
                                <th style='width: 30px; border-left: 1px solid #000000;'>№</th>
                                <th style='width: 135px;'>Протокол №</th>
                                <th style='width: 100px;'>Дата на протокола</th>
                                <th style='width: 300px;'>Име на пострадалия</th>
                                <th style='width: 100px; border-right: 1px solid #000000;'>Дата на злополуката</th>
                            </tr>
                        </thead><tbody>";
            }

            int counter = 1;

            foreach (InvestigationProtocol investProtocol in investigationProtocols)
            {
                html += @"<tr>
                            <td align='center'>" + counter + @"</td>
                            <td align='left'>" + investProtocol.InvestigaitonProtocolNumber.ToString() + @"</td>
                            <td align='left'>" + CommonFunctions.FormatDate(investProtocol.InvProtDate) + @"</td>
                            <td align='left'>" + ((investProtocol.DeclarationOfAccident != null && investProtocol.DeclarationOfAccident.DeclarationOfAccidentWorker != null) ? investProtocol.DeclarationOfAccident.DeclarationOfAccidentWorker.WorkerFullName.ToString() : "") + @"</td>
                            <td align='left'>" + ((investProtocol.DeclarationOfAccident != null && investProtocol.DeclarationOfAccident.DeclarationOfAccidentAcc != null) ? (investProtocol.DeclarationOfAccident.DeclarationOfAccidentAcc.AccDateTime.HasValue ? CommonFunctions.FormatDate(investProtocol.DeclarationOfAccident.DeclarationOfAccidentAcc.AccDateTime) : "") : "") + @"</td>
                          </tr>";
                counter++;
            }

            if (investigationProtocols.Count() > 0)
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
            Response.AppendHeader("content-disposition", "attachment; filename=InvestigationProtocols.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        // Generate html as excel response result
        private string GenerateAllInvestigationProtocolsForExport()
        {
            //Get the list of Investigation Protocols according to the specified filters, order and paging
            List<InvestigationProtocol> investigationProtocols = InvestigationProtocolUtil.GetAllInvestigationProtocols(investigationProtocolFilter, CurrentUser);

            string html = @"<html xmlns='http://www.w3.org/1999/xhtml' xml:lang='en'>
                            <head>
                                <meta http-equiv='content-type' content='application/xhtml+xml; charset=UTF-8' />
                            </head>
                            <body>
                                <table>
                                    <tr><td align='center' colspan='5' style='font-weight: bold; font-size: 1.6em;'>АСУ на човешките ресурси</td></tr>
                                    <tr><td align='center' colspan='5' style='font-weight: bold; font-size: 2em;'>Безопасност на труда</td></tr>
                                    <tr><td colspan='5'>&nbsp;</td></tr>
                                    <tr><td align='center' colspan='5' style='font-weight: bold; font-size: 1.3em;'>Протоколи за резултатите от злополука</td></tr>
                                    <tr>
                                        <td align='right' colspan='2'>
                                            <span style='font-weight: normal;'>№ Протокол:&nbsp;</span>
                                        </td>
                                        <td align='left' style='width: 100px;'>
                                            <span style='font-weight: bold;'>" + (!String.IsNullOrEmpty(investProtNumber) ? (investProtNumber.ToString() + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") : "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + @"</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: normal;'>Име на пострадалия:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + (!String.IsNullOrEmpty(injuredName) ? injuredName.ToString() : "") + @"</span>
                                        </td>
                                    </tr>
                                    <tr>                
                                        <td align='right' colspan='2'>
                                            <span style='font-weight: normal;'>Дата на протокола от:&nbsp;</span>
                                        </td>
                                        <td align='left' style='width: 100px;'>
                                            <span style='font-weight: bold;'>" + (protocolDateFrom.HasValue ? CommonFunctions.FormatDate(protocolDateFrom) : "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + @"</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: normal;'>до:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + (protocolDateTo.HasValue ? CommonFunctions.FormatDate(protocolDateTo) : "&nbsp;") + @"</span>
                                        </td>
                                    </tr>
                                    <tr>                
                                        <td align='right' colspan='2'>
                                            <span style='font-weight: normal;'>Дата на злополуката от:&nbsp;</span>
                                        </td>
                                        <td align='left' style='width: 100px;'>
                                            <span style='font-weight: bold;'>" + (accidentDateFrom.HasValue ? CommonFunctions.FormatDate(accidentDateFrom) : "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + @"</span>
                                        </td>
                                        <td align='left' colspan='2'>
                                            <span style='font-weight: normal;'>до:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + (accidentDateTo.HasValue ? CommonFunctions.FormatDate(accidentDateTo) : "&nbsp;") + @"</span>
                                        </td>
                                    </tr>
                                </table>";


            if (investigationProtocols.Count() > 0)
            {
                html += @"
                    <table>
                        <thead>
                            <tr>
                                <th style='width: 30px; border-bottom: 1px solid white; background-color: black; color: #FFFFFF;'>№</th>
                                <th style='width: 135px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Протокол №</th>
                                <th style='width: 100px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Дата на протокола</th>
                                <th style='width: 300px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Име на пострадалия</th>
                                <th style='width: 100px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Дата на злополуката</th>
                            </tr>
                        </thead><tbody>";

                int counter = 1;

                foreach (InvestigationProtocol investProtocol in investigationProtocols)
                {
                    html += @"<tr>
                                <td align='center' style='border: 1px solid black;'>" + counter + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + investProtocol.InvestigaitonProtocolNumber.ToString() + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + CommonFunctions.FormatDate(investProtocol.InvProtDate) + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + ((investProtocol.DeclarationOfAccident != null && investProtocol.DeclarationOfAccident.DeclarationOfAccidentWorker != null) ? investProtocol.DeclarationOfAccident.DeclarationOfAccidentWorker.WorkerFullName.ToString() : "") + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + ((investProtocol.DeclarationOfAccident != null && investProtocol.DeclarationOfAccident.DeclarationOfAccidentAcc != null) ? (investProtocol.DeclarationOfAccident.DeclarationOfAccidentAcc.AccDateTime.HasValue ? CommonFunctions.FormatDate(investProtocol.DeclarationOfAccident.DeclarationOfAccidentAcc.AccDateTime) : "") : "") + @"</td>
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
