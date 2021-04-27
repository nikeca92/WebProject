using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using PMIS.Common;
using PMIS.HealthSafety.Common;

namespace PMIS.HealthSafety.PrintContentPages
{
    public partial class PrintAllWorkplaceConditionsCards : HSPage
    {
        string militaryUnitId = "";
        string cardNumber = null;
        string jobType = null;
        int sortBy = 1; // 1 - Default

        private string MilitaryUnitLabel = CommonFunctions.GetLabelText("MilitaryUnit");

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check visibility right for the print screen
            if (this.GetUIItemAccessLevel("HS_WCONDCARDS") != UIAccessLevel.Hidden)
            {
                if (!String.IsNullOrEmpty(Request.Params["MilitaryUnitID"]) && Request.Params["MilitaryUnitID"] != "-1")
                {
                    militaryUnitId = Request.Params["MilitaryUnitID"];
                }

                if (!String.IsNullOrEmpty(Request.Params["CardNumber"]))
                {
                    cardNumber = Request.Params["CardNumber"];
                }

                if (!String.IsNullOrEmpty(Request.Params["JobType"]))
                {
                    jobType = Request.Params["JobType"];
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
                sb.Append("<td rowspan=\"2\">" + this.GenerateAllWorkplaceCondCardsHtml() + "</td>");
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
        private string GenerateAllWorkplaceCondCardsHtml()
        {
            //Get the list of Roles according to the specified filters, order
            List<WorkplaceConditionsCard> workplaceCondCards = WorkplaceConditionsCardUtil.GetAllWorkplaceConditionsCards(militaryUnitId, cardNumber, jobType, sortBy, 0, 0, CurrentUser);

            MilitaryUnit militaryUnit = null;
            if (!String.IsNullOrEmpty(militaryUnitId))
            {
                militaryUnit = MilitaryUnitUtil.GetMilitaryUnit(Int32.Parse(militaryUnitId), CurrentUser);
            }

            string html = @"<table style='padding: 5px;'>
                             <tr>
                                <td align='right' style='width: 210px;'>
                                    <span class='Label'>" + this.MilitaryUnitLabel + @":&nbsp;</span>
                                </td>
                                <td align='left' style='width: 195px;'>
                                   <span class='ValueLabel'>" + (militaryUnit != null ? militaryUnit.DisplayTextForSelection : "") + @"</span>
                                </td>
                                <td align='right' style='width: 140px;'>
                                    <span class='Label'>Номер на карта:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 125px;'>
                                    <span class='ValueLabel'>" + (cardNumber != null ? cardNumber : "") + @"</span>
                                </td>
                             </tr>
                             <tr>                
                                <td align='right' style='width: 140px;'>
                                    <span class='Label'>Работно място (вид работа):&nbsp;</span>
                                </td>
                                <td align='left' colspan='3'>
                                   <span class='ValueLabel'>" + (jobType != null ? jobType : "") + @"</span>
                                </td>
                             </tr>";

            if (workplaceCondCards.Count() > 0)
            {
                html += @"
                    <tr><td colspan='4' align='center'>
                    <table id='workplaceCondCardsTable' name='workplaceCondCardsTable' class='CommonHeaderTable'>
                        <thead>
                            <tr>
                                <th style='width: 30px; border-left: 1px solid #000000;'>№</th>
                                <th style='width: 130px;'>Номер на карта</th>
                                <th style='width: 260px;'>" + this.MilitaryUnitLabel + @"</th>
                                <th style='width: 250px; border-right: 1px solid #000000;'>Работно място</th>
                            </tr>
                        </thead><tbody>";
            }

            int counter = 1;

            foreach (WorkplaceConditionsCard workplaceCondCard in workplaceCondCards)
            {
                html += @"<tr>
                            <td align='center'>" + counter + @"</td>
                            <td align='left'>" + workplaceCondCard.CardNumber.ToString() + @"</td>
                            <td align='left'>" + (workplaceCondCard.MilitaryUnit != null ? workplaceCondCard.MilitaryUnit.DisplayTextForSelection : "&nbsp;") + @"</td>
                            <td align='left'>" + workplaceCondCard.JobType.ToString() + @"</td>
                          </tr>";
                counter++;
            }

            if (workplaceCondCards.Count() > 0)
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
            Response.AppendHeader("content-disposition", "attachment; filename=WorkplaceConditionsCards.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        // Generate html as excel response result
        private string GenerateAllRiskAssessmentsForExport()
        {
            //Get the list of Roles according to the specified filters, order
            List<WorkplaceConditionsCard> workplaceCondCards = WorkplaceConditionsCardUtil.GetAllWorkplaceConditionsCards(militaryUnitId, cardNumber, jobType, sortBy, 0, 0, CurrentUser);

            MilitaryUnit militaryUnit = null;
            if (!String.IsNullOrEmpty(militaryUnitId))
            {
                militaryUnit = MilitaryUnitUtil.GetMilitaryUnit(Int32.Parse(militaryUnitId), CurrentUser);
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
                                    <tr><td align='center' colspan='4' style='font-weight: bold; font-size: 1.3em;'>Карти за комплексно оценяване на рисковете за живота и здравето</td></tr>
                                    <tr>
                                        <td align='left' colspan='4'>
                                           <span style='font-weight: normal;'>" + this.MilitaryUnitLabel + @":&nbsp;</span>
                                           <span style='font-weight: bold;'>" + (militaryUnit != null ? (militaryUnit.DisplayTextForSelection + "&nbsp;&nbsp;&nbsp;") : "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + @"</span>
                                           <span style='font-weight: normal;'>Номер на карта:&nbsp;</span>
                                           <span style='font-weight: bold;'>" + (cardNumber != null ? cardNumber : "") + @"</span>
                                        </td>
                                     </tr>
                                     <tr>                
                                        <td align='left' colspan='4'>
                                            <span style='font-weight: normal;'>Работно място (вид работа):&nbsp;</span>
                                           <span style='font-weight: bold;'>" + (jobType != null ? jobType : "") + @"</span>
                                        </td>
                                     </tr>
                                </table>";


            if (workplaceCondCards.Count() > 0)
            {
                html += @"
                    <table>
                        <thead>
                            <tr>
                                <th style='width: 30px; border-bottom: 1px solid white; background-color: black; color: #FFFFFF;'>№</th>
                                <th style='width: 130px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Номер на карта</th>
                                <th style='width: 260px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>" + this.MilitaryUnitLabel + @"</th>
                                <th style='width: 250px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Работно място</th>
                            </tr>
                        </thead><tbody>";

                int counter = 1;

                foreach (WorkplaceConditionsCard workplaceCondCard in workplaceCondCards)
                {
                    html += @"<tr>
                                <td align='center' style='border: 1px solid black;'>" + counter + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + workplaceCondCard.CardNumber.ToString() + @"</td>
                                <td style='border: 1px solid black;'>" + (workplaceCondCard.MilitaryUnit != null ? workplaceCondCard.MilitaryUnit.DisplayTextForSelection : "&nbsp;") + @"</td>
                                <td style='border: 1px solid black;'>" + workplaceCondCard.JobType.ToString() + @"</td>
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
