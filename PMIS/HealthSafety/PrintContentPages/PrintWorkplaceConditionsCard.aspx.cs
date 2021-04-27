using System;
using System.Text;
using System.Collections.Generic;

using PMIS.Common;
using PMIS.HealthSafety.Common;

namespace PMIS.HealthSafety.PrintContentPages
{
    public partial class PrintWorkplaceConditionsCard : HSPage
    {
        private string MilitaryUnitLabel = CommonFunctions.GetLabelText("MilitaryUnit");

        protected void Page_Load(object sender, EventArgs e)
        {
            int workplaceConditionsCardID = 0;

            if (int.TryParse(Request.Params["WorkplaceConditionsCardID"], out workplaceConditionsCardID))
            {
                WorkplaceConditionsCard workplaceConditionsCard = WorkplaceConditionsCardUtil.GetWorkplaceConditionsCard(workplaceConditionsCardID, CurrentUser);

                // Check visibility right for the print screen
                bool screenHidden = (this.GetUIItemAccessLevel("HS_EDITWCONDCARD") == UIAccessLevel.Hidden)
                        || (this.GetUIItemAccessLevel("HS_WCONDCARDS") == UIAccessLevel.Hidden);

                if (workplaceConditionsCard != null && !screenHidden)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table>");
                    sb.Append("<tr>");
                    sb.Append("<td rowspan=\"2\">" + this.GenerateWorkplaceConditionsCardHtml(workplaceConditionsCard) + "</td>");
                    sb.Append("<td style=\"vertical-align: top;\">" + CommonFunctions.GenerateButtons(true, 160, false) + "</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr><td style=\"vertical-align: bottom;\">" + CommonFunctions.GenerateButtons(false, 0, false) + "</td></tr>");
                    sb.Append("</table>");

                    this.divResults.InnerHtml = sb.ToString();
                }
                else
                {
                    this.divResults.InnerHtml = "";
                }
            }
        }

        // Generates html content related to contextual workplace conditions card
        private string GenerateWorkplaceConditionsCardHtml(WorkplaceConditionsCard workplaceConditionsCard)
        {
            bool isMilUnitHidden = this.GetUIItemAccessLevel("HS_EDITWCONDCARD_MILITARYUNIT") == UIAccessLevel.Hidden;
            bool isCardNumHidden = this.GetUIItemAccessLevel("HS_EDITWCONDCARD_CARDNUMBER") == UIAccessLevel.Hidden;
            bool isCityHidden = this.GetUIItemAccessLevel("HS_EDITWCONDCARD_CITY") == UIAccessLevel.Hidden;
            bool isJobTypeHidden = this.GetUIItemAccessLevel("HS_EDITWCONDCARD_JOBTYPE") == UIAccessLevel.Hidden;
            bool isWorkerCountHidden = this.GetUIItemAccessLevel("HS_EDITWCONDCARD_WORKERSCOUNT") == UIAccessLevel.Hidden;
            bool isComplAssessHidden = this.GetUIItemAccessLevel("HS_EDITWCONDCARD_COMPLEXASSESSMENT") == UIAccessLevel.Hidden;
            bool isComplAssessPointValHidden = this.GetUIItemAccessLevel("HS_EDITWCONDCARD_COMPLEXASSESSMENTPOINTVALUE") == UIAccessLevel.Hidden;
            bool isAddRewardHidden = this.GetUIItemAccessLevel("S_EDITWCONDCARD_ADDITIONALREWARD") == UIAccessLevel.Hidden;

            string html = @"
                        <table style='padding: 5px;'>
                            <tr style='min-height: 17px;'>
                                <td align='right' style='width: 200px;'>"
                                + (isMilUnitHidden ? "&nbsp;" : "<span class='Label'>" + this.MilitaryUnitLabel + @":&nbsp;</span>") +
                                @"</td>
                                <td align='left' style='width: 175px;'>"
                                + (isMilUnitHidden ? "&nbsp;" : "<span class='ValueLabel'>" + (workplaceConditionsCard.MilitaryUnit != null ? workplaceConditionsCard.MilitaryUnit.DisplayTextForSelection : "") + @"</span>") +
                                @"</td>
                                <td colspan='2'></td>
                            </tr>
                            <tr style='min-height: 17px;'>
                                <td align='right' style='width: 200px;'>"
                                + (isCardNumHidden ? "&nbsp;" : "<span class='Label'>Номер на карта:&nbsp;</span>") +
                                @"</td>
                                <td align='left' style='width: 175px;'>"
                                + (isCardNumHidden ? "&nbsp;" : "<span class='ValueLabel'>" + workplaceConditionsCard.CardNumber.ToString() + @"</span>") +
                                @"</td>
                                <td align='right' style='width: 170px;'>"
                                + (isCityHidden ? "&nbsp;" : "<span class='Label'>Населено място:&nbsp;</span>") +
                                @"</td>
                                <td align='left' style='width: 125px;'>"
                                + (isCityHidden ? "&nbsp;" : "<span class='ValueLabel'>" + (workplaceConditionsCard.MilitaryUnit.City != null ? workplaceConditionsCard.MilitaryUnit.City.CityName : "") + @"</span>") +
                                @"</td>
                            </tr>
                            <tr style='min-height: 17px;'>
                                <td align='right' style='width: 200px;'>"
                                + (isJobTypeHidden ? "&nbsp;" : "<span class='Label'>Работно място (вид работа):&nbsp;</span>") +
                                @"</td>
                                <td align='left' style='width: 175px;'>"
                                + (isJobTypeHidden ? "&nbsp;" : "<span class='ValueLabel'>" + workplaceConditionsCard.JobType + @"</span>") +
                                @"</td>
                                <td align='right' style='width: 170px;'>"
                                + (isWorkerCountHidden ? "&nbsp;" : "<span class='Label'>Брой на работещите:&nbsp;</span>") +
                                @"</td>
                                <td align='left' style='width: 125px;'>"
                                + (isWorkerCountHidden ? "&nbsp;" : "<span class='ValueLabel'>" + workplaceConditionsCard.WorkersCount ?? "0" + @"</span>") +
                                @"</td>
                            </tr>";

            bool isCIIndHidden = this.GetUIItemAccessLevel("HS_EDITWCONDCARD_CARDITEMS_INDICATOR") == UIAccessLevel.Hidden;
            bool isCIValueHidden = this.GetUIItemAccessLevel("HS_EDITWCONDCARD_CARDITEMS_VALUE") == UIAccessLevel.Hidden;
            bool isCIRateHidden = this.GetUIItemAccessLevel("HS_EDITWCONDCARD_CARDITEMS_RATE") == UIAccessLevel.Hidden;
            bool isCIAssessHidden = this.GetUIItemAccessLevel("HS_EDITWCONDCARD_CARDITEMS_ASSESSMENT") == UIAccessLevel.Hidden;

            if (!(this.GetUIItemAccessLevel("HS_EDITWCONDCARD_CARDITEMS") == UIAccessLevel.Hidden
                || isCIIndHidden && isCIValueHidden && isCIRateHidden && isCIAssessHidden))
            {
                html += @"
                    <tr><td colspan='4' align='center'>
                    <table id='cardItemsTable' name='cardItemsTable' class='CommonHeaderTable'>
                        <thead>
                            <tr>
                                <th style='width: 30px; border-left: 1px solid #000000;'></th>
                                <th style='width: 394px;'>Показатели на условията на труд</th>
                                <th style='width: 80px;'>Стойност на показателя</th>
                                <th style='width: 80px;'>Степен</th>
                                <th style='width: 80px; border-right: 1px solid #000000;'>Оценка</th>
                            </tr>
                        </thead><tbody>";
                int counter = 1;

                foreach (WorkplaceConditionsCardItem wCondCardItem in workplaceConditionsCard.Items)
                {
                    html += @"<tr>
                                <td align='center'>" + counter + @"</td>
                                <td align='left'>" + wCondCardItem.Caption + @"</td>
                                <td>" + (wCondCardItem.Value.HasValue ? CommonFunctions.FormatDecimal(wCondCardItem.Value.Value) : "&nbsp;") + @"</td>
                                <td>" + (wCondCardItem.Rate.HasValue ? CommonFunctions.FormatDecimal(wCondCardItem.Rate.Value) : "&nbsp;") + @"</td>
                                <td>" + (wCondCardItem.Assessment.HasValue ? CommonFunctions.FormatDecimal(wCondCardItem.Assessment.Value) : "&nbsp;") + @"</td>
                              </tr>";

                    List<WorkplaceConditionsCardItem> childCardItems = WorkplaceConditionsCardItemUtil.GetAllWorkplaceConditionsCardItemsByIndicatorType(workplaceConditionsCard.WorkplaceConditionsCardId, wCondCardItem.IndicatorTypeId, CurrentUser);

                    foreach (WorkplaceConditionsCardItem childCardItem in childCardItems)
                    {
                        html += @"<tr>
                                    <td>&nbsp;</td>
                                    <td align='left' style='font-style:italic;'>&nbsp;&nbsp;&nbsp;" + childCardItem.Caption + @"</td>
                                    <td>" + (childCardItem.Value.HasValue ? CommonFunctions.FormatDecimal(childCardItem.Value.Value) : "&nbsp;") + @"</td>
                                    <td>" + (childCardItem.Rate.HasValue ? CommonFunctions.FormatDecimal(childCardItem.Rate.Value) : "&nbsp;") + @"</td>
                                    <td>" + (childCardItem.Assessment.HasValue ? CommonFunctions.FormatDecimal(childCardItem.Assessment.Value) : "&nbsp;") + @"</td>
                                  </tr>";
                    }

                    counter++;
                }

                html += @"</tbody></table>
                    </td></tr>";
            }

            html += @"<tr style='min-height: 17px;'>
                        <td align='right' colspan='3'>"
                        + (isComplAssessHidden ? "&nbsp;" : "<span class='Label'>Комплексна оценка:&nbsp;</span>") +
                        @"</td>
                        <td align='left'>"
                        + (isComplAssessHidden ? "&nbsp;" : "<span class='ValueLabel'>" + workplaceConditionsCard.ComplexAssessment ?? "0" + @"</span>") +
                        @"</td>
                    </tr>
                    <tr style='min-height: 17px;'>
                        <td align='right' colspan='3'>"
                        + (isComplAssessPointValHidden ? "&nbsp;" : "<span class='Label'>Стойност на една точка от комплексната оценка:&nbsp;</span>") +
                        @"</td>
                        <td align='left'>"
                        + (isComplAssessPointValHidden ? "&nbsp;" : "<span class='ValueLabel'>" + workplaceConditionsCard.ComplexAssessmentPointValue ?? "0" + @"</span>") +
                        @"</td>
                    </tr>
                    <tr style='min-height: 17px;'>
                        <td align='right' colspan='3'>"
                        + (isAddRewardHidden ? "&nbsp;" : "<span class='Label'>Размер на допълнителното трудово възнаграждение:&nbsp;</span>") +
                        @"</td>
                        <td align='left'>"
                        + (isAddRewardHidden ? "&nbsp;" : "<span class='ValueLabel'>" + workplaceConditionsCard.AdditionalReward ?? "0" + @"</span>") +
                        @"<span class='Label'>&nbsp;&nbsp;лева</span></td>
                    </tr>";

            html += "</table>";

            return html;
        }
    }
}
