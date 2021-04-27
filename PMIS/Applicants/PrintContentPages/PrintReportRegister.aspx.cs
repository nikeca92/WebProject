using System;
using System.Linq;
using System.Text;

using PMIS.Common;
using PMIS.Applicants.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace PMIS.Applicants.PrintContentPages
{
    public partial class PrintReportRegister : APPLPage
    {
        int militaryDepartmentID = 0;
        string year = "";

        UIAccessLevel l;

        int visibleColumnsCount = 7;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check visibility right for the print screen
            if (this.GetUIItemAccessLevel("APPL_REPORTS_REGISTER") != UIAccessLevel.Hidden)
            {
                if (!String.IsNullOrEmpty(Request.Params["Year"]))
                {
                    year = Request.Params["Year"];
                }

                if (!String.IsNullOrEmpty(Request.Params["MilitaryDepartmentID"])
                    && Request.Params["MilitaryDepartmentID"] != ListItems.GetOptionChooseOne().Value)
                {
                    int mDepartmentID = 0;
                    int.TryParse(Request.Params["MilitaryDepartmentID"], out mDepartmentID);
                    if (mDepartmentID > 0)
                    {
                        militaryDepartmentID = mDepartmentID;
                    }
                }

                this.GenerateExcel();
            }
        }

        protected void GenerateExcel()
        {
            string result = this.GenerateContentForExport();
            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment; filename=ReportRegister.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        // Generate html as excel response result
        private string GenerateContentForExport()
        {
            ReportRegisterFilter filter = new ReportRegisterFilter()
            {
                MilitaryDepartmentId = militaryDepartmentID,
                Year = year
            };

            //Get the list of records according to the specified filters and order
            List<ReportRegisterBlock> listBlocks = ReportRegisterUtil.GetReportRegisterBlock(filter, 0, CurrentUser);

            MilitaryDepartment militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment(militaryDepartmentID, CurrentUser);

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
                                    <tr><td align='center' colspan='" + visibleColumnsCount + @"' style='font-weight: bold; font-size: 1.3em;'>Регистър</td></tr>
                                    <tr><td colspan='" + visibleColumnsCount + @"'>&nbsp;</td></tr>
                                    <tr>
                                        <td colspan='" + visibleColumnsCount + @"' align='center'>
                                            <span style='font-weight: normal;'>Военно окръжие:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + (militaryDepartment != null ? militaryDepartment.MilitaryDepartmentName : "") + @"</span>
                                        </td>                         
                                    </tr>
                                    <tr>
                                        <td colspan='" + visibleColumnsCount + @"' align='center'>
                                            <span style='font-weight: normal;'>Година:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + year + @"</span>
                                        </td>      
                                    </tr>
                                    <tr>
                                        <td style='width: 30px;'>&nbsp;</td>
                                    </tr>";


            if (listBlocks.Count() > 0)
            {
                html += @"
                        <thead>
                            <tr>
                               <th class='TableHeaderCell' style='width: 50px;'>Номер</th>
                               <th class='TableHeaderCell' style='width: 120px;'>Дата на постъпване на документа</th>
                               <th class='TableHeaderCell' style='width: 150px;'>От къде е постъпил документа</th>
                               <th class='TableHeaderCell' style='width: 300px;'>Кратко съдържание</th>
                               <th class='TableHeaderCell' style='width: 80px;'>Брой на листата</th>
                               <th class='TableHeaderCell' style='width: 200px;'>Номер на писмото с което се изпраща документа</th>
                               <th class='TableHeaderCell' style='width: 300px;'>Забележка</th>
                            </tr>
                        </thead>
                    <tbody>";

                var groupedBlocks = listBlocks
                    .GroupBy(x => new
                    {
                        x.RegisterNumber,
                        x.DocumentDate,
                        x.ApplicantFullName,
                        x.OrderNumber,
                        x.OrderDateString,
                        x.PageCount,
                        x.Notes
                    })
                    .Select(x => new
                    {
                        RegisterNumber = x.Key.RegisterNumber,
                        DocumentDate = x.Key.DocumentDate,
                        ApplicantFullName = x.Key.ApplicantFullName,
                        OrderNumber = x.Key.OrderNumber,
                        OrderDateString = x.Key.OrderDateString,
                        Positions = x.Select(y => new
                                     {
                                         PositionName = y.PositionName,
                                         VPN = y.VPN,
                                         SEQ = y.SEQ
                                     }).OrderBy(y => y.SEQ).ToList(),
                        PageCount = x.Key.PageCount,
                        Notes = x.Key.Notes
                    })
                    .OrderBy(x => x.RegisterNumber)
                    .ToList();

                foreach (var block in groupedBlocks)
                {
                    string cellStyleText = @"vertical-align: top; text-align: left;";
                    string cellStyleDate = @"vertical-align: top; text-align: left; mso-number-format: ""dd.mm.yyyyг.""";

                    string descriptionHTML = "Кандидат за ВС по МЗ №" + block.OrderNumber.ToString() + "/" + block.OrderDateString;

                    int rowspan = 1;

                    foreach (var position in block.Positions)
                    {
                        descriptionHTML += @"<br style=""mso-data-placement:same-cell;"" />
                                             &nbsp; -в.ф. " + position.VPN + " - " + position.PositionName;

                        rowspan++;
                    }

                    string notesHTML = CommonFunctions.HtmlEncoding(block.Notes).Replace(Environment.NewLine, @"<br style=""mso-data-placement:same-cell;"" />");

                    html += @"
                            <tr>
                                <td class='TableDataCell' style='" + cellStyleText + @"'>" + block.RegisterNumber.ToString() + @"</td>
                                <td class='TableDataCell' style='" + cellStyleDate + @"'>" + block.DocumentDate + @"</td>
                                <td class='TableDataCell' style='" + cellStyleText + @"'>" + block.ApplicantFullName + @"</td>
                                <td class='TableDataCell' style='" + cellStyleText + @"'>" + descriptionHTML + @"</td>
                                <td class='TableDataCell' style='" + cellStyleText + @"'>" + block.PageCount.ToString() + @"</td>
                                <td class='TableDataCell' style='" + cellStyleText + @"'></td>
                                <td class='TableDataCell' style='" + cellStyleText + @"'>" + notesHTML + @"</td>
                            </tr>";
                }

                html += "</tbody></table>";
            }

            html += "</body></html>";

            return html;
        }
    }
}
