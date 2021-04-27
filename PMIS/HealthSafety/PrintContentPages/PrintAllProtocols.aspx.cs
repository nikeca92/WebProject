using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using PMIS.Common;
using PMIS.HealthSafety.Common;

namespace PMIS.HealthSafety.PrintContentPages
{
    public partial class PrintAllProtocols : HSPage
    {
        const string All = "Всички";

        string protocolTypeId = "";
        string protNumber = "";
        DateTime? dateFrom = null;
        DateTime? dateTo = null;
        int sortBy = 1; // 1 - Default

        private string MilitaryUnitLabel = CommonFunctions.GetLabelText("MilitaryUnit");

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check visibility right for the print screen
            if (this.GetUIItemAccessLevel("HS_PROTOCOLS") != UIAccessLevel.Hidden)
            {
                if (!String.IsNullOrEmpty(Request.Params["ProtocolTypeID"]) && Request.Params["ProtocolTypeID"] != "-1")
                {
                    protocolTypeId = Request.Params["ProtocolTypeID"];
                }

                if (!String.IsNullOrEmpty(Request.Params["ProtNumber"]))
                {
                    protNumber = Request.Params["ProtNumber"];
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
                sb.Append("<td rowspan=\"2\">" + this.GenerateAllProtocolsHtml() + "</td>");
                sb.Append("<td style=\"vertical-align: top;\">" + CommonFunctions.GenerateButtons(true, 160, true) + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr><td style=\"vertical-align: bottom;\">" + CommonFunctions.GenerateButtons(false, 0, true) + "</td></tr>");
                sb.Append("</table>");

                contentPage = sb.ToString();
            }
            else
            {
                contentPage = this.GenerateAllProtocolsForExport();
            }

            return contentPage;
        }

        // Generate display data as html into the page
        private string GenerateAllProtocolsHtml()
        {
            //Get the list of Protocols according to the specified filters, order and paging
            List<Protocol> protocols = ProtocolUtil.GetAllProtocols(protNumber, protocolTypeId, dateFrom, dateTo, sortBy, 0, 0, CurrentUser);

            ProtocolType protocolType = null;
            if (!String.IsNullOrEmpty(protocolTypeId))
            {
                protocolType = ProtocolTypeUtil.GetProtocolType(Int32.Parse(protocolTypeId), CurrentUser);
            }

            string html = @"<table style='padding: 5px;'>                            
                             <tr>
                                <td align='right' style='vertical-align: top; width: 130px;'>
                                    <span class='Label'>Измерване:&nbsp;</span>
                                </td>
                                <td align='left' style='vertical-align: top; width: 550px;'>
                                    <span class='ValueLabel'>" + (protocolType != null ? protocolType.ProtocolTypeName : All) + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' style='vertical-align: top; width: 130px;'>
                                    <span class='Label'>Протокол №:&nbsp;</span>
                                </td>
                                <td align='left' style='vertical-align: top; width: 550px;'>
                                   <span class='ValueLabel'>" + (String.IsNullOrEmpty(protNumber) ? "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" : protNumber) + @"</span>
                                   <span class='Label'>&nbsp;&nbsp;Дата от:&nbsp;</span>
                                   <span class='ValueLabel'>" + (dateFrom.HasValue ? CommonFunctions.FormatDate(dateFrom) : "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + @"</span>
                                   <span class='Label'>&nbsp;&nbsp;до:&nbsp;</span>
                                   <span class='ValueLabel'>" + (dateTo.HasValue ? CommonFunctions.FormatDate(dateTo) : "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + @"</span>
                                 </td>
                             </tr>";

            if (protocols.Count() > 0)
            {
                html += @"
                    <tr><td colspan='2' align='center'>
                    <table id='protocolsTable' name='protocolsTable' class='CommonHeaderTable'>
                        <thead>
                            <tr>
                                <th style='width: 20px; border-left: 1px solid #000000;'>№</th>
                                <th style='width: 130px;'>Протокол №</th>
                                <th style='width: 80px;'>Дата</th>
                                <th style='width: 240px;'>Измерване</th>
                                <th style='width: 200px; border-right: 1px solid #000000;'>" + this.MilitaryUnitLabel + @"</th>
                            </tr>
                        </thead><tbody>";
            }

            int counter = 1;

            foreach (Protocol protocol in protocols)
            {
                html += @"<tr>
                                 <td align='center'>" + counter + @"</td>
                                 <td align='center'>" + protocol.ProtocolNumber + @"</td>
                                 <td align='center'>" + (protocol.ProtocolDate.HasValue ? CommonFunctions.FormatDate(protocol.ProtocolDate.Value) : "") + @"</td>
                                 <td align='left'>" + (protocol.ProtocolType != null ? protocol.ProtocolType.ProtocolTypeName : "") + @"</td>
                                 <td align='left'>" + (protocol.MilitaryUnit != null ? protocol.MilitaryUnit.DisplayTextForSelection : "") + @"</td>
                          </tr>";
                counter++;
            }

            if (protocols.Count() > 0)
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
            Response.AppendHeader("content-disposition", "attachment; filename=Protocols.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        // Generate html as excel response result
        private string GenerateAllProtocolsForExport()
        {
            //Get the list of Protocols according to the specified filters, order and paging
            List<Protocol> protocols = ProtocolUtil.GetAllProtocols(protNumber, protocolTypeId, dateFrom, dateTo, sortBy, 0, 0, CurrentUser);

            ProtocolType protocolType = null;
            if (!String.IsNullOrEmpty(protocolTypeId))
            {
                protocolType = ProtocolTypeUtil.GetProtocolType(Int32.Parse(protocolTypeId), CurrentUser);
            }

            string html = @"<html xmlns='http://www.w3.org/1999/xhtml' xml:lang='en'>
                            <head>
                                <meta http-equiv='content-type' content='application/xhtml+xml; charset=UTF-8' />
                            </head>
                            <body>
                            <table>
                            <tr><td align='center' colspan='5' style='font-weight: bold; font-size: 1.6em;'>АСУ на човешките ресурси</td></tr>
                            <tr><td align='center' colspan='5' style='font-weight: bold; font-size: 2em;'>Безопасност на труда</td></tr>
                            <tr><td colspan='5'>&nbsp;</td></tr>
                            <tr><td align='center' colspan='5' style='font-weight: bold; font-size: 1.3em;'>Протоколи от извършени измервания</td></tr>
                             <tr>
                                <td align='right' colspan='2' style='vertical-align: top; width: 80px;'>
                                    <span style='font-weight: normal;'>Измерване:&nbsp;</span>
                                </td>
                                <td align='left' colspan='3' style='font-weight: bold; vertical-align: top;'>
                                    <span style='font-weight: bold;'>" + (protocolType != null ? protocolType.ProtocolTypeName : All) + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' colspan='2' style='vertical-align: top; width: 80px;'>
                                    <span style='font-weight: normal;'>Протокол №:&nbsp;</span>
                                </td>
                                <td align='left' colspan='3' style='vertical-align: top;'>
                                   <span style='font-weight: bold;'>" + (String.IsNullOrEmpty(protNumber) ? "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" : protNumber) + @"</span>
                                   <span style='font-weight: normal;'>&nbsp;&nbsp;Дата от:&nbsp;</span>
                                   <span style='font-weight: bold;'>" + CommonFunctions.FormatDate(dateFrom) + @"</span>
                                   <span style='font-weight: normal;'>&nbsp;&nbsp;до:&nbsp;</span>
                                   <span style='font-weight: bold;'>" + CommonFunctions.FormatDate(dateTo) + @"</span>
                                 </td>
                             </tr>";

            if (protocols.Count() > 0)
            {
                html += @"
                    <tr>
                        <td align='center' style='width: 20px; border-bottom: 1px solid white; background-color: black; color: #FFFFFF;'>№</td>
                        <td align='center' style='width: 130px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Протокол №</td>
                        <td align='center' style='width: 80px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Дата</td>
                        <td align='center' style='width: 240px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Измерване</td>
                        <td align='center' style='width: 200px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>" + this.MilitaryUnitLabel + @"</td>
                    </tr>";
            }

            int counter = 1;

            foreach (Protocol protocol in protocols)
            {
                html += @"<tr>
                                 <td align='center' style='border: 1px solid black;'>" + counter + @"</td>
                                 <td align='center' style='border: 1px solid black;'>" + protocol.ProtocolNumber + @"</td>
                                 <td align='center' style='border: 1px solid black;'>" + (protocol.ProtocolDate.HasValue ? CommonFunctions.FormatDate(protocol.ProtocolDate.Value) : "") + @"</td>
                                 <td align='left' style='border: 1px solid black;'>" + (protocol.ProtocolType != null ? protocol.ProtocolType.ProtocolTypeName : "") + @"</td>
                                 <td align='left' style='border: 1px solid black;'>" + (protocol.MilitaryUnit != null ? protocol.MilitaryUnit.DisplayTextForSelection : "") + @"</td>
                          </tr>";
                counter++;
            }

            html += "</table>";

            html += "</body></html>";

            return html;
        }
    }
}
