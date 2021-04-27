using System;
using System.Linq;
using System.Text;

using PMIS.Common;
using PMIS.Reserve.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Collections;

namespace PMIS.Reserve.PrintContentPages
{
    public partial class PrintReportA33 : RESPage
    {
        private string sessionResultsKey = "ReportA33Result";

        const string All = "Всички";

        string militaryDepartmentId = "";

        public void SetPrintTitleLinesWidth()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnPrintTitleLinesWidth");

            hf.Value = "1024";
        }

        public void SetHeadersLeft()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnHeadersLeft");

            hf.Value = "255";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.SetPrintTitleLinesWidth();
                this.SetHeadersLeft();
            }

            // Check visibility right for the print screen
            if (GetUIItemAccessLevel("RES_REPORTS") == UIAccessLevel.Hidden ||
                this.GetUIItemAccessLevel("RES_REPORTS_REPORTA33") != UIAccessLevel.Hidden)
            {
                if (!String.IsNullOrEmpty(Request.Params["MilitaryDepartmentId"]))
                {
                    militaryDepartmentId = Request.Params["MilitaryDepartmentId"];
                }

                if (!IsPostBack)
                {
                    if (Request.Params["Export"] != null && Request.Params["Export"].ToLower() == "true")
                    {
                        btnGenerateExcel_Click(this, new EventArgs());
                    }
                    else
                    {
                        this.divResults.InnerHtml = GeneratePageContent(false);
                    }
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
                sb.Append("<td rowspan=\"2\">" + this.GenerateAllRecordsHtml() + "</td>");
                sb.Append("<td style=\"vertical-align: top;\">" + CommonFunctions.GenerateButtons(true, 160, true) + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr><td style=\"vertical-align: bottom;\">" + CommonFunctions.GenerateButtons(false, 0, true) + "</td></tr>");
                sb.Append("</table>");

                contentPage = sb.ToString();
            }
            else
            {
                contentPage = this.GenerateAllRecordsForExport();
            }

            return contentPage;
        }

        // Generate display data as html into the page
        private string GenerateAllRecordsHtml()
        {
            ReportA33Filter filter = new ReportA33Filter()
            {
                MilitaryDepartmentIds = militaryDepartmentId,
                PageSize = -1,
                PageIdx = -1
            };

            ReportA33Result reportResult = null;

            if (Session[sessionResultsKey] != null)
                reportResult = (ReportA33Result)Session[sessionResultsKey];
            else
                reportResult = ReportA33Util.GetReportA33(filter, CurrentUser);


            string militaryDepartmentName = "";
            List<MilitaryDepartment> militaryDepartments = MilitaryDepartmentUtil.GetAllMilitaryDepartmentsByIDs(CurrentUser, militaryDepartmentId);
            foreach (MilitaryDepartment militaryDepartment in militaryDepartments)
            {
                militaryDepartmentName += (militaryDepartmentName == "" ? "" : ", ") + militaryDepartment.MilitaryDepartmentName;
            }

            if(militaryDepartmentName == "")
            {
                militaryDepartmentName = All;
            }

            StringBuilder html = new StringBuilder();
            html.Append(@"<table style='padding: 5px; padding-left: 0px; width: 1024px;'>
                             <tr>
                                <td align='center' style='padding-left: 100px;'>
                                    <span class='Label'>Военно окръжие:&nbsp;</span>
                                    <span class='ValueLabel'>" + militaryDepartmentName + @"</span>&nbsp;&nbsp;&nbsp;&nbsp;                                    
                                </td>
                             </tr>
                            ");

            if (reportResult.Rows.Count > 0)
            {
                string headerRow1 = "";
                string headerRow2 = "";
                string headerRow3 = "";

                int cellIndex = 0;
                foreach (string headerCell in reportResult.HeaderCells)
                {
                    cellIndex++;
                    if (cellIndex <= 3)
                    {
                        headerRow1 += "<th style='width: " + (cellIndex == 3 ? "180" : "30") + @"px;' rowspan='2'>" + headerCell + "</th>";
                    }
                    else
                    {
                        if (cellIndex == 4)
                        {
                            headerRow1 += "<th colspan='3'>ЛИЧЕН СЪСТАВ</th>";
                        }
                        else if (cellIndex == 6)
                        {
                            headerRow2 += "<th style='width: 60px;'>Всичко</th>";
                        }
                        else if (cellIndex == 7)
                        {
                            int colspan = reportResult.HeaderCells.Count() - 4;
                            headerRow1 += "<th colspan='" + colspan.ToString() + "'>ТЕХНИКА ОТ НАЦИОНАЛНОТО СТОПАНСТВО</th>";
                        }

                        string s = headerCell; //
                        if (s == "Инженерна техника")
                            s = "ИТ";
                        else if (s == "Подемно-транспортна техника")
                            s = "ППТТ";
                        else if (s == "Специализиран железопътен състав")
                            s = "СЖС";
                        else if (s == "Авиационна техника")
                            s = "АТ";

                        headerRow2 += "<th style='width: 60px;'>" + s + "</th>";

                        if (cellIndex == reportResult.HeaderCells.Count())
                        {
                            headerRow2 += "<th style='width: 60px;'>Всичко</th>";
                        }
                    }

                    headerRow3 += "<td>" + cellIndex + "</td>";
                }

                cellIndex++;
                headerRow3 += "<td>" + cellIndex + "</td>";
                cellIndex++;
                headerRow3 += "<td>" + cellIndex + "</td>";

                //Setup the header of the grid
                html.Append(@"<tr><td colspan='1' align='center'>
                         <table id='applicantsTable' name='applicantsTable' class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                            " + headerRow1.ToString() + @"
                            </tr> 
                            <tr>
                            " + headerRow2.ToString() + @"
                            </tr>
                         </thead>
                         <tbody>
                            <tr>
                            " + headerRow3.ToString() + @"
                            </tr>");
            }

            ArrayList blocks = reportResult.Rows;

            int counter = 0;
            string oldSection = "";

            foreach (string[] row in blocks)
            {
                counter++;

                if (counter == 2)
                {
                    html.Append("<tr><td colspan='" + ((int)(row.Count() + 2)).ToString() + "'>&nbsp;</td></tr>");
                }

                html.Append(@"<tr>");

                int count = 0;
                int totalHR = 0;
                int totalTech = 0;

                string section = "";

                foreach (string cell in row)
                {
                    string cellValue = cell;

                    count++;

                    if (count == 1)
                    {
                        section = cellValue;

                        if (section != oldSection)
                        {
                            oldSection = section;

                            string sectionTitle = "";

                            if (section == "Б")
                                sectionTitle = "Доставят се за:";
                            else if (section == "В")
                                sectionTitle = "Начин на доставяне:";

                            if (sectionTitle != "")
                            {
                                html.Append("<td>" + section + @"</td>" +
                                        "<td>&nbsp;</td>" +
                                        "<td>" + sectionTitle + @"</td>" +
                                        "<td colspan='" + ((int)(row.Count() - 1)).ToString() + "'>&nbsp;</td>" +
                                        "</tr><tr>");

                                cellValue = "";
                            }
                        }
                        else
                        {
                            cellValue = "";
                        }
                    }

                    if (count == 4 || count == 5)
                    {
                        totalHR += int.Parse(cellValue);
                    }
                    else if (count == 6)
                    {
                        html.Append("<td>" + totalHR.ToString() + "</td>");

                        totalHR = 0;
                    }

                    if (count >= 6 && count <= row.Count())
                    {
                        totalTech += int.Parse(cellValue);
                    }

                    html.Append("<td>" + cellValue + "</td>");

                    if (count == row.Count())
                    {
                        html.Append("<td>" + totalTech.ToString() + "</td>");

                        totalTech = 0;
                    }
                }

                html.Append("</tr>");
            }

            if (reportResult.Rows.Count > 0)
            {
                html.Append("</tbody></table></td></tr>");
            }

            html.Append("</table>");

            return html.ToString();
        }

        // Handles export to excel button click
        protected void btnGenerateExcel_Click(object sender, EventArgs e)
        {
            string result = this.GeneratePageContent(true);
            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment; filename=ReportA33.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        // Generate html as excel response result
        private string GenerateAllRecordsForExport()
        {

            ReportA33Filter filter = new ReportA33Filter()
            {
                MilitaryDepartmentIds = militaryDepartmentId,
                PageSize = -1,
                PageIdx = -1
            };

            ReportA33Result reportResult = null;

            if (Session[sessionResultsKey] != null)
                reportResult = (ReportA33Result)Session[sessionResultsKey];
            else
                reportResult = ReportA33Util.GetReportA33(filter, CurrentUser);


            string militaryDepartmentName = "";
            List<MilitaryDepartment> militaryDepartments = MilitaryDepartmentUtil.GetAllMilitaryDepartmentsByIDs(CurrentUser, militaryDepartmentId);
            foreach (MilitaryDepartment militaryDepartment in militaryDepartments)
            {
                militaryDepartmentName += (militaryDepartmentName == "" ? "" : ", ") + militaryDepartment.MilitaryDepartmentName;
            }

            if (militaryDepartmentName == "")
            {
                militaryDepartmentName = All;
            }

            StringBuilder html = new StringBuilder();
            html.Append(@"<html xmlns='http://www.w3.org/1999/xhtml' xml:lang='en'>
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
                                    <tr><td align='center' colspan='" + ((int)((reportResult.HeaderCells.Count() + 2))) + @"' style='font-weight: bold; font-size: 1.6em;'>АСУ на човешките ресурси</td></tr>
                                    <tr><td align='center' colspan='" + ((int)((reportResult.HeaderCells.Count() + 2))) + @"' style='font-weight: bold; font-size: 2em;'>Отчет на ресурсите от резерва</td></tr>
                                    <tr><td colspan='" + ((int)((reportResult.HeaderCells.Count() + 2))) + @"'>&nbsp;</td></tr>
                                    <tr><td align='center' colspan='" + ((int)((reportResult.HeaderCells.Count() + 2))) + @"' style='font-weight: bold; font-size: 1.3em;'>Анализ на ресурсите (A33)</td></tr>
                                    <tr><td colspan='" + ((int)((reportResult.HeaderCells.Count() + 2))) + @"'>&nbsp;</td></tr>
                                    <tr>
                                        <td align='center' colspan='" + ((int)((reportResult.HeaderCells.Count() + 2))) + @"'>
                                            <span style='font-weight: normal;'>Военно окръжие:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + militaryDepartmentName + @"</span>&nbsp;&nbsp;&nbsp;                                           
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan='15'>&nbsp;</td>
                                    </tr>
                                </table>");

            if (reportResult.Rows.Count > 0)
            {
                string headerRow1 = "";
                string headerRow2 = "";
                string headerRow3 = "";

                int cellIndex = 0;
                foreach (string headerCell in reportResult.HeaderCells)
                {
                    cellIndex++;
                    if (cellIndex <= 3)
                    {
                        headerRow1 += "<th class='TableHeaderCell' style='width: 180px;' rowspan='2'>" + headerCell + "</th>";
                    }
                    else
                    {
                        if (cellIndex == 4)
                        {
                            headerRow1 += "<th class='TableHeaderCell' colspan='3'>ЛИЧЕН СЪСТАВ</th>";
                        }
                        else if (cellIndex == 6)
                        {
                            headerRow2 += "<th class='TableHeaderCell' style='width: 60px;'>Всичко</th>";
                        }
                        else if (cellIndex == 7)
                        {
                            int colspan = reportResult.HeaderCells.Count() - 4;
                            headerRow1 += "<th class='TableHeaderCell' colspan='" + colspan.ToString() + "'>ТЕХНИКА ОТ НАЦИОНАЛНОТО СТОПАНСТВО</th>";
                        }

                        string s = headerCell; //
                        if (s == "Инженерна техника")
                            s = "ИТ";
                        else if (s == "Подемно-транспортна техника")
                            s = "ППТТ";
                        else if (s == "Специализиран железопътен състав")
                            s = "СЖС";
                        else if (s == "Авиационна техника")
                            s = "АТ";

                        headerRow2 += "<th class='TableHeaderCell' style='width: 60px;'>" + s + "</th>";

                        if (cellIndex == reportResult.HeaderCells.Count())
                        {
                            headerRow2 += "<th class='TableHeaderCell' style='width: 60px;'>Всичко</th>";
                        }
                    }

                    headerRow3 += "<td class='TableDataCell'>" + cellIndex + "</td>";
                }

                cellIndex++;
                headerRow3 += "<td class='TableDataCell'>" + cellIndex + "</td>";
                cellIndex++;
                headerRow3 += "<td class='TableDataCell'>" + cellIndex + "</td>";

                //Setup the header of the grid
                html.Append(@"<table>                        
                         <thead>
                            <tr>
                            " + headerRow1.ToString() + @"
                            </tr> 
                            <tr>
                            " + headerRow2.ToString() + @"
                            </tr>                             
                         </thead>
                         <tbody>
                            <tr>
                            " + headerRow3.ToString() + @"
                            </tr>");
            }

            ArrayList blocks = reportResult.Rows;

            int counter = 0;
            string oldSection = "";

            foreach (string[] row in blocks)
            {
                counter++;

                if (counter == 2)
                {
                    html.Append("<tr><td colspan='" + ((int)(row.Count() + 2)).ToString() + "'>&nbsp;</td></tr>");
                }

                html.Append(@"<tr>");

                int count = 0;
                int totalHR = 0;
                int totalTech = 0;

                string section = "";

                foreach (string cell in row)
                {
                    string cellValue = cell;

                    count++;

                    if (count == 1)
                    {
                        section = cellValue;

                        if (section != oldSection)
                        {
                            oldSection = section;

                            string sectionTitle = "";

                            if (section == "Б")
                                sectionTitle = "Доставят се за:";
                            else if (section == "В")
                                sectionTitle = "Начин на доставяне:";

                            if (sectionTitle != "")
                            {
                                html.Append("<td class='TableDataCell'>" + section + @"</td>" +
                                        "<td>&nbsp;</td>" +
                                        "<td class='TableDataCell'>" + sectionTitle + @"</td>" +
                                        "<td colspan='" + ((int)(row.Count() - 1)).ToString() + "'>&nbsp;</td>" +
                                        "</tr><tr>");

                                cellValue = "";
                            }
                        }
                        else
                        {
                            cellValue = "";
                        }
                    }

                    if (count == 4 || count == 5)
                    {
                        totalHR += int.Parse(cellValue);
                    }
                    else if (count == 6)
                    {
                        html.Append("<td class='TableDataCell'>" + totalHR.ToString() + "</td>");

                        totalHR = 0;
                    }

                    if (count >= 6 && count <= row.Count())
                    {
                        totalTech += int.Parse(cellValue);
                    }

                    html.Append("<td class='TableDataCell'>" + cellValue + "</td>");

                    if (count == row.Count())
                    {
                        html.Append("<td class='TableDataCell'>" + totalTech.ToString() + "</td>");

                        totalTech = 0;
                    }
                }

                html.Append("</tr>");
            }

            if (reportResult.Rows.Count > 0)
                html.Append("</tbody></table>");

            html.Append("</body></html>");

            return html.ToString();
        }
    }
}
