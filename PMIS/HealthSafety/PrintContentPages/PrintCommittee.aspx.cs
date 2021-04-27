using System;
using System.Text;

using PMIS.Common;
using PMIS.HealthSafety.Common;

namespace PMIS.HealthSafety.PrintContentPages
{
    public partial class PrintCommittee : HSPage
    {
        private string MilitaryUnitLabel = CommonFunctions.GetLabelText("MilitaryUnit");

        protected void Page_Load(object sender, EventArgs e)
        {
            int committeeID = 0;

            if (int.TryParse(Request.Params["CommitteeID"], out committeeID))
            {
                Committee committee = CommitteeUtil.GetCommittee(committeeID, CurrentUser);

                // Check visibility right for the print screen
                bool screenHidden = (this.GetUIItemAccessLevel("HS_EDITCOMMITTEE") == UIAccessLevel.Hidden)
                        || (this.GetUIItemAccessLevel("HS_COMMITTEE") == UIAccessLevel.Hidden);

                if (committee != null && !screenHidden)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table>");
                    sb.Append("<tr>");
                    sb.Append("<td rowspan=\"2\">" + this.GenerateCommitteeHtml(committee) + "</td>");
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

        // Generates html content related to contextual committee
        private string GenerateCommitteeHtml(Committee committee)
        {
            bool isCommitteeTypeHidden = this.GetUIItemAccessLevel("HS_EDITCOMMITTEE_TYPE") == UIAccessLevel.Hidden;
            bool isMilForceTypeHidden = this.GetUIItemAccessLevel("HS_EDITCOMMITTEE_MILFORCETYPE") == UIAccessLevel.Hidden;
            bool isMilUnitHidden = this.GetUIItemAccessLevel("HS_EDITCOMMITTEE_MILUNIT") == UIAccessLevel.Hidden;

            string html = @"<table style='padding: 5px;'>
                                <tr style='min-height: 17px;'>
                                    <td align='right' style='width: 150px;'>"
                                    + (isCommitteeTypeHidden ? "&nbsp;" : "<span class='Label'>Тип:&nbsp;</span>") +
                                    @"</td>
                                    <td align='left' colspan='3' style='width: 255px;'>"
                                    + (isCommitteeTypeHidden ? "&nbsp;" : "<span class='ValueLabel'>" + (committee.CommitteeType != null ? committee.CommitteeType.TableValue : "&nbsp;") + @"</span>") +
                                    @"</td>                                  
                                </tr>
                                <tr style='min-height: 17px;'>
                                    <td align='right' style='width: 150px;'>"
                                    + (isMilForceTypeHidden ? "&nbsp;" : "<span class='Label'>Вид ВС:&nbsp;</span>") +
                                    @"</td>
                                    <td align='left' style='width: 150px;'>"
                                    + (isMilForceTypeHidden ? "&nbsp;" : "<span class='ValueLabel'>" + (committee.MilitaryForceType != null ? committee.MilitaryForceType.MilitaryForceTypeName : "&nbsp;") + @"</span>") +
                                    @"</td>
                                    <td align='right' style='width: 150px;'>"
                                    + (isMilUnitHidden ? "&nbsp;" : "<span class='Label'>" + this.MilitaryUnitLabel + @":&nbsp;</span>") +
                                    @"</td>
                                    <td align='left' style='width: 150px;'>"
                                    + (isMilUnitHidden ? "&nbsp;" : "<span class='ValueLabel'>" + (committee.MilitaryUnit != null ? committee.MilitaryUnit.DisplayTextForSelection : "&nbsp;") + @"</span>") + 
                                    @"</td>
                                </tr>";

            if (this.GetUIItemAccessLevel("HS_EDITCOMMITTEE_MEMBERS") != UIAccessLevel.Hidden)
            {
                bool isTrainingYearHidden = this.GetUIItemAccessLevel("HS_EDITTRAINHIST_TRAININGYEAR") == UIAccessLevel.Hidden;
                bool isLegalRefHidden = this.GetUIItemAccessLevel("HS_EDITTRAINHIST_LEGALREF") == UIAccessLevel.Hidden;

                html += @"
                    <tr><td colspan='4' align='center'>
                    <table id='committeeMembersTable' name='committeeMembersTable' class='CommonHeaderTable'>
                        <thead>
                            <tr>
                                <th style='width: 30px; border-left: 1px solid #000000;'></th>
                                <th style='width: 100px;'>ЕГН</th>
                                <th style='width: 100px;'>Звание</th>
                                <th style='width: 150px;'>Име</th>
                                <th style='width: 80px;'>Година на обучение</th>
                                <th style='width: 200px; border-right: 1px solid #000000;'>Наредба</th>
                            </tr>
                        </thead><tbody>";
                int counter = 1;

                foreach (CommitteeMember member in committee.CommitteeMembers)
                {
                    if (member.Person != null)
                    {
                        Training training = TrainingUtil.GetLastTrainingByPerson(member.PersonId.Value, CurrentUser);
                        Person person = member.Person;

                        html += @"<tr>
                            <td align='left'>" + counter.ToString() + @"</td>
                            <td align='left'>" + (string.IsNullOrEmpty(person.IdentNumber) ? "&nbsp;" : person.IdentNumber) + @"</td>
                            <td align='left'>" + (person.MilitaryRank != null ? person.MilitaryRank.LongName : "&nbsp;") + @"</td>
                            <td align='left'>" + (string.IsNullOrEmpty(person.FullName) ? "&nbsp;" : person.FullName) + @"</td>
                            <td align='left'>" + (training == null || isTrainingYearHidden ? "&nbsp;" : (training.TrainingYear.HasValue ? training.TrainingYear.Value.ToString() : "&nbsp;")) + @"</td>                            
                            <td align='left'>" + (training == null || isLegalRefHidden ? "&nbsp;" : (string.IsNullOrEmpty(training.LegalRef) ? "&nbsp;" : CommonFunctions.ReplaceNewLinesInString(training.LegalRef))) + @"</td>
                          </tr>";
                        counter++;
                    }
                }

                html += @"</tbody></table>
                    </td></tr>";
            }

            html += "</table>";

            return html;
        }
    }
}
