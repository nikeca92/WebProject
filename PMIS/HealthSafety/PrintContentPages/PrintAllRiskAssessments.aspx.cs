using System;
using System.Linq;
using System.Text;

using PMIS.Common;
using PMIS.HealthSafety.Common;
using System.Collections.Generic;

namespace PMIS.HealthSafety.PrintContentPages
{
    public partial class PrintAllRiskAssessments : HSPage
    {
        int? militaryUnitId = null;
        string regNumber = null;
        DateTime? dateFrom = null;
        DateTime? dateTo = null;
        int sortBy = 1; // 1 - Default

        private string MilitaryUnitLabel = CommonFunctions.GetLabelText("MilitaryUnit");

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check visibility right for the print screen
            if (this.GetUIItemAccessLevel("HS_RISKASSESSMENTS") != UIAccessLevel.Hidden)
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

                if (!String.IsNullOrEmpty(Request.Params["RegNumber"]))
                {
                    regNumber = Request.Params["RegNumber"];
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
                sb.Append("<td rowspan=\"2\">" + this.GenerateAllRiskAssessmentsHtml() + "</td>");
                sb.Append("<td style=\"vertical-align: top;\">" + CommonFunctions.GenerateButtons(true, 160, true) + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr><td style=\"vertical-align: bottom;\">" + CommonFunctions.GenerateButtons(false, 0, true) + "</td></tr>");
                sb.Append("</table>");

                contentPage = sb.ToString();
            }
            else
            {
                contentPage = this.GenerateAllRiskAssessmentsForExport();
            }

            return contentPage;
        }

        // Generate display data as html into the page
        private string GenerateAllRiskAssessmentsHtml()
        {
            //Get the list of risk assessments according to the specified filters and order
            List<RiskAssessment> riskAssessments = RiskAssessmentUtil.GetAllRiskAssessments(regNumber, militaryUnitId, dateFrom, dateTo, sortBy, 0, 0, CurrentUser);

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
                                    <span class='Label'>Регистрационен №:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 195px;'>
                                    <span class='ValueLabel'>" + (regNumber != null ? regNumber : "") + @"</span>
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

            if (riskAssessments.Count() > 0)
            {
                html += @"
                    <tr><td colspan='4' align='center'>
                    <table id='assessmentsTable' name='assessmentsTable' class='CommonHeaderTable'>
                        <thead>
                            <tr>
                                <th style='width: 30px; border-left: 1px solid #000000;'>№</th>
                                <th style='width: 276px;'>Регистрационен №</th>
                                <th style='width: 80px;'>Дата на изготвяне</th>
                                <th style='width: 280px; border-right: 1px solid #000000;'>" + this.MilitaryUnitLabel + @"</th>
                            </tr>
                        </thead><tbody>";
            }

            int counter = 1;

            foreach (RiskAssessment rAssessment in riskAssessments)
            {
                html += @"<tr>
                            <td align='center'>" + counter + @"</td>
                            <td align='left'>" + rAssessment.RegNumber + @"</td>
                            <td align='left'>" + CommonFunctions.FormatDate(rAssessment.PreparationDate) + @"</td>
                            <td align='left'>" + (rAssessment.MilitaryUnit != null ? rAssessment.MilitaryUnit.DisplayTextForSelection: "") + @"</td>
                          </tr>";
                counter++;
            }

            if (riskAssessments.Count() > 0)
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
            Response.AppendHeader("content-disposition", "attachment; filename=RiskAssessments.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        // Generate html as excel response result
        private string GenerateAllRiskAssessmentsForExport()
        {
            //Get the list of risk assessments according to the specified filters and order
            List<RiskAssessment> riskAssessments = RiskAssessmentUtil.GetAllRiskAssessments(regNumber, militaryUnitId, dateFrom, dateTo, sortBy, 0, 0, CurrentUser);

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
                                    <tr><td align='center' colspan='4' style='font-weight: bold; font-size: 1.3em;'>Оценки на риска</td></tr>
                                    <tr>
                                        <td align='right' style='width: 140px;'>
                                            <span style='font-weight: normal;'>" + this.MilitaryUnitLabel + @":&nbsp;</span>
                                        </td>
                                        <td align='left' style='width: 300px;'>
                                           <span style='font-weight: bold;'>" + (militaryUnit != null ? militaryUnit.DisplayTextForSelection: "&nbsp;") + @"</span>
                                        </td>
                                        <td align='right' style='width: 140px;'>
                                            <span style='font-weight: normal;'>Регистрационен №:&nbsp;</span>
                                        </td>
                                        <td align='left' style='width: 300px;'>
                                            <span style='font-weight: bold;'>" + (regNumber != null ? regNumber : "&nbsp;") + @"</span>
                                        </td>
                                    </tr>
                                    <tr>                
                                        <td align='right' style='width: 140px;'>
                                            <span style='font-weight: normal;'>Дата от:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='3'>
                                            <span style='font-weight: bold;'>" + (dateFrom != null ? CommonFunctions.FormatDate(dateFrom.ToString()) : "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + @"</span>
                                            <span style='font-weight: normal;'>&nbsp;&nbsp;&nbsp;до:&nbsp;</span>
                                           <span style='font-weight: bold;'>" + (dateTo != null ? CommonFunctions.FormatDate(dateTo.ToString()) : "&nbsp;") + @"</span>
                                        </td>
                                    </tr>
                                </table>";


            if (riskAssessments.Count() > 0)
            {
                html += @"
                    <table>
                        <thead>
                            <tr>
                                <th style='width: 140px; border-bottom: 1px solid white; background-color: black; color: #FFFFFF;'>№</th>
                                <th style='width: 300px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Регистрационен №</th>
                                <th style='width: 140px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Дата на изготвяне</th>
                                <th style='width: 300px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>" + this.MilitaryUnitLabel + @"</th>
                            </tr>
                        </thead><tbody>";

                int counter = 1;

                foreach (RiskAssessment rAssessment in riskAssessments)
                {
                    html += @"
                            <tr>
                                <td align='center' style='border: 1px solid black;'>" + counter + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + rAssessment.RegNumber + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + CommonFunctions.FormatDate(rAssessment.PreparationDate) + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + (rAssessment.MilitaryUnit != null ? rAssessment.MilitaryUnit.DisplayTextForSelection : "&nbsp;") + @"</td>
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
