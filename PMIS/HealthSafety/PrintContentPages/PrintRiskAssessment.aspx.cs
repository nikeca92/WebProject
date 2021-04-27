using System;
using System.Text;

using PMIS.Common;
using PMIS.HealthSafety.Common;

namespace PMIS.HealthSafety.PrintContentPages
{
    public partial class PrintRiskAssessment : HSPage
    {
        private string MilitaryUnitLabel = CommonFunctions.GetLabelText("MilitaryUnit");

        protected void Page_Load(object sender, EventArgs e)
        {
            int riskAssessmentID = 0;

            if (int.TryParse(Request.Params["RiskAssessmentID"], out riskAssessmentID))
            {
                RiskAssessment riskAssessment = RiskAssessmentUtil.GetRiskAssessment(riskAssessmentID, CurrentUser);
                
                // Check visibility right for the print screen
                bool screenHidden = (this.GetUIItemAccessLevel("HS_EDITRISKASSESS") == UIAccessLevel.Hidden)
                        || (this.GetUIItemAccessLevel("HS_RISKASSESSMENTS") == UIAccessLevel.Hidden);

                if (riskAssessment != null && !screenHidden)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table>");
                    sb.Append("<tr>");
                    sb.Append("<td rowspan=\"2\">" + this.GenerateRiskAssessmentHtml(riskAssessment) + "</td>");
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

        // Generates html content related to contextual risk assessment
        private string GenerateRiskAssessmentHtml(RiskAssessment riskAssessment)
        {
            bool isPrepDateHidden = this.GetUIItemAccessLevel("HS_EDITRISKASSESS_PREPDATE") == UIAccessLevel.Hidden;
            bool isRegNumHidden = this.GetUIItemAccessLevel("HS_EDITRISKASSESS_REGNUM") == UIAccessLevel.Hidden;
            bool isMilForceTypeHidden = this.GetUIItemAccessLevel("HS_EDITRISKASSESS_MILTFORCETYPE") == UIAccessLevel.Hidden;
            bool isMilUnitHidden = this.GetUIItemAccessLevel("HS_EDITRISKASSESS_MILUNIT") == UIAccessLevel.Hidden;
            bool isCommHidden = this.GetUIItemAccessLevel("HS_EDITRISKASSESS_COMMENT") == UIAccessLevel.Hidden;

            string html = @"
                        <table style='padding: 5px;'>
                            <tr style='min-height: 17px;'>
                                <td align='right' style='width: 140px;'>"
                                + (isPrepDateHidden ? "&nbsp;" : "<span class='Label'>Дата на изготвяне:&nbsp;</span>") +  
                                @"</td>
                                <td align='left' style='width: 195px;'>"
                                + (isPrepDateHidden ? "&nbsp;" : "<span class='ValueLabel'>" + CommonFunctions.FormatDate(riskAssessment.PreparationDate) + @"</span>") +
                                @"</td>
                                <td align='right' style='width: 140px;'>"
                                + (isRegNumHidden ? "&nbsp;" : "<span class='Label'>Регистрационен №:&nbsp;</span>") +
                                @"</td>
                                <td align='left' style='width: 195px;'>"
                                + (isRegNumHidden ? "&nbsp;" : "<span class='ValueLabel'>" + riskAssessment.RegNumber + @"</span>") +    
                                @"</td>
                            </tr>
                            <tr style='min-height: 17px;'>
                                <td align='right' style='width: 140px;'>"
                                + (isMilForceTypeHidden ? "&nbsp;" : "<span class='Label'>Вид ВС:&nbsp;</span>") +    
                                @"</td>
                                <td align='left' style='width: 195px;'>"
                                + (isMilForceTypeHidden ? "&nbsp;" : "<span class='ValueLabel'>" + (riskAssessment.MilitaryForceType != null ? riskAssessment.MilitaryForceType.MilitaryForceTypeName : "") + @"</span>") +    
                                @"</td>
                                <td align='right' style='width: 140px;'>"
                                + (isMilUnitHidden ? "&nbsp;" : "<span class='Label'>" + this.MilitaryUnitLabel +  @":&nbsp;</span>") +    
                                @"</td>
                                <td align='left' style='width: 195px;'>"
                                + (isMilUnitHidden ? "&nbsp;" : "<span class='ValueLabel'>" + (riskAssessment.MilitaryUnit != null ? riskAssessment.MilitaryUnit.DisplayTextForSelection : "") + @"</span>") +
                                @"</td>
                            </tr>
                            <tr>
                                <td align='center' colspan='4'>"
                                + (isCommHidden ? "&nbsp;" : @"<table style='width: 100%;'>
                                                                <tr>
                                                                    <td align='left'>
                                                                        <span class='Label'>Коментари:</span>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align='left'>
                                                                        <span class='ValueLabel'>" + CommonFunctions.ReplaceNewLinesInString(riskAssessment.Comments) + @"</span>
                                                                    </td>
                                                                </tr>
                                                            </table>") +    
                                @"</td>
                            </tr>";

            bool isRecommHidden = this.GetUIItemAccessLevel("HS_EDITRISKASSESS_RECOMM_RECOMM") == UIAccessLevel.Hidden;
            bool isDueDateHidden = this.GetUIItemAccessLevel("HS_EDITRISKASSESS_RECOMM_DUEDATE") == UIAccessLevel.Hidden;
            bool isExeDateHidden = this.GetUIItemAccessLevel("HS_EDITRISKASSESS_RECOMM_EXEDATE") == UIAccessLevel.Hidden;

            if (!(this.GetUIItemAccessLevel("HS_EDITRISKASSESS_RECOMM") == UIAccessLevel.Hidden
                || isRecommHidden && isDueDateHidden && isExeDateHidden))
            {
                html += @"
                    <tr><td colspan='4' align='center'>
                    <table id='recommendationsTable' name='recommendationsTable' class='CommonHeaderTable'>
                        <thead>
                            <tr>
                                <th style='width: 30px; border-left: 1px solid #000000;'></th>
                                <th style='width: 476px;'>Препоръки за отстраняването</th>
                                <th style='width: 80px;'>Срок</th>
                                <th style='width: 80px; border-right: 1px solid #000000;'>Дата на изпълнение</th>
                            </tr>
                        </thead><tbody>";
                int counter = 1;

                foreach (RiskRemoveRecommendation recommendation in riskAssessment.RiskRemoveRecommendationItems)
                {
                    html += @"<tr>
                            <td align='center'>" + counter + @"</td>
                            <td align='left'>" + (isRecommHidden ? "&nbsp;" : CommonFunctions.ReplaceNewLinesInString(recommendation.Recommendations)) + @"</td>
                            <td>" + (isDueDateHidden ? "&nbsp;" : CommonFunctions.FormatDate(recommendation.DueDate)) + @"</td>
                            <td>" + (isExeDateHidden ? "&nbsp;" : CommonFunctions.FormatDate(recommendation.ExecutionDate)) + @"</td>
                          </tr>";
                    counter++;
                }

                html += @"</tbody></table>
                    </td></tr>";   
            }

            html += "</table>";

            return html;
        }
    }
}
