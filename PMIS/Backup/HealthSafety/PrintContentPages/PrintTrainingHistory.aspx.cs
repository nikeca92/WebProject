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
    public partial class PrintTrainingHistory : HSPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int personID = 0;

            if (int.TryParse(Request.Params["PersonID"], out personID))
            {
                Person person = PersonUtil.GetPerson(personID, CurrentUser);

                // Check visibility right for the print screen
                bool screenHidden = (this.GetUIItemAccessLevel("HS_TRAININGHISTORY") == UIAccessLevel.Hidden)
                                    || (this.GetUIItemAccessLevel("HS_EDITTRAINHIST") == UIAccessLevel.Hidden);

                if (person != null && !screenHidden)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table>");
                    sb.Append("<tr>");
                    sb.Append("<td width='700px' rowspan=\"2\">" + this.GenerateTrainingHistoryHtml(person) + "</td>");
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

        // Generates html content related to contextual person
        private string GenerateTrainingHistoryHtml(Person person)
        {
            string html = @"<table style='padding: 5px;'>
                                <tr style='min-height: 17px;'>
                                    <td align='right' style='width: 150px;'>"
                                    + "<span class='Label'>ЕГН:&nbsp;</span>" +
                                    @"</td>
                                    <td align='left' style='width: 195px;'>"
                                    + "<span class='ValueLabel'>" + person.IdentNumber + @"</span>" +
                                    @"</td>
                                    <td align='right' style='width: 140px;'>"
                                    + "<span class='Label'>Име:&nbsp;</span>" +
                                    @"</td>
                                    <td align='left' style='width: 195px;'>"
                                    + "<span class='ValueLabel'>" + person.FullName + @"</span>" +
                                    @"</td>
                                    <td align='right' style='width: 150px;'>"
                                    + "<span class='Label'>Звание:&nbsp;</span>" +
                                    @"</td>
                                    <td align='left' style='width: 195px;'>"
                                    + "<span class='ValueLabel'>" + (person.MilitaryRank != null ? person.MilitaryRank.LongName : "") + @"</span>" +
                                    @"</td>
                                </tr>";

            bool IsTrainingDateHidden = this.GetUIItemAccessLevel("HS_EDITTRAINHIST_TRAININGDATE") == UIAccessLevel.Hidden;
            bool IsTrainingYearHidden = this.GetUIItemAccessLevel("HS_EDITTRAINHIST_TRAININGYEAR") == UIAccessLevel.Hidden;
            bool IsTrainingDescHidden = this.GetUIItemAccessLevel("HS_EDITTRAINHIST_TRAININGDESC") == UIAccessLevel.Hidden;
            bool IsLegalRefHidden = this.GetUIItemAccessLevel("HS_EDITTRAINHIST_LEGALREF") == UIAccessLevel.Hidden;

            if (!(this.GetUIItemAccessLevel("HS_EDITTRAINHIST") == UIAccessLevel.Hidden
                || IsTrainingDateHidden && IsTrainingYearHidden && IsTrainingDescHidden && IsLegalRefHidden))
            {
                html += @"
                    <tr><td colspan='6' align='center'>
                    <table id='trainingsTable' name='trainingsTable' class='CommonHeaderTable'>
                        <thead>
                            <tr>
                                <th style='width: 30px; border-left: 1px solid #000000;'></th>                                
                                <th style='width: 70px;'>Дата</th>
                                <th style='width: 50px;'>Година</th>
                                <th style='width: 250px;'>Обучение</th>
                                <th style='width: 250px; border-right: 1px solid #000000;'>Наредба</th>
                            </tr>
                        </thead><tbody>";
                int counter = 1;

                List<Training> trainings = TrainingUtil.GetAllTrainingsByPerson(person.PersonId, CurrentUser);
                foreach (Training training in trainings)
                {
                    html += @"<tr>
                            <td align='left'>" + counter + @"</td>
                            <td align='left'>" + (IsTrainingDateHidden ? "&nbsp;" : CommonFunctions.FormatDate(training.TrainingDate)) + @"</td>
                            <td align='left'>" + (IsTrainingYearHidden ? "&nbsp;" : (training.TrainingYear.HasValue ? training.TrainingYear.Value.ToString() : "&nbsp;")) + @"</td>
                            <td align='left'>" + (IsTrainingDescHidden ? "&nbsp;" : (string.IsNullOrEmpty(training.TrainingDesc) ? "&nbsp;" : CommonFunctions.ReplaceNewLinesInString(training.TrainingDesc))) + @"</td>
                            <td align='left'>" + (IsLegalRefHidden ? "&nbsp;" : (string.IsNullOrEmpty(training.LegalRef) ? "&nbsp;" : CommonFunctions.ReplaceNewLinesInString(training.LegalRef))) + @"</td>
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
