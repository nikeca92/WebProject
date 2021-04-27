using System;
using System.Text;

using PMIS.Common;
using PMIS.HealthSafety.Common;

namespace PMIS.HealthSafety.PrintContentPages
{
    public partial class PrintUnsafeWorkingConditionsNotice : HSPage
    {
        private string MilitaryUnitLabel = CommonFunctions.GetLabelText("MilitaryUnit");

        protected void Page_Load(object sender, EventArgs e)
        {
            int unsafeWConditionsNoticeID = 0;

            if (int.TryParse(Request.Params["UnsafeWConditionsNoticeID"], out unsafeWConditionsNoticeID))
            {
                UnsafeWorkingConditionsNotice unsafeWConditionsNotice = UnsafeWorkingConditionsNoticeUtil.GetUnsafeWorkingConditionsNotice(unsafeWConditionsNoticeID, CurrentUser);

                // Check visibility right for the print screen
                bool screenHidden = (this.GetUIItemAccessLevel("HS_EDITUNSAFEWCONDNOTICE") == UIAccessLevel.Hidden)
                        || (this.GetUIItemAccessLevel("HS_UNSAFEWCONDNOTICE") == UIAccessLevel.Hidden);

                if (unsafeWConditionsNotice != null && !screenHidden)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table>");
                    sb.Append("<tr>");
                    sb.Append("<td rowspan=\"2\">" + this.GenerateUnsafeWConditionsNoticeHtml(unsafeWConditionsNotice) + "</td>");
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

        // Generates html content related to contextual unsafe work conditions notice
        private string GenerateUnsafeWConditionsNoticeHtml(UnsafeWorkingConditionsNotice unsafeWConditionsNotice)
        {
            bool isNoticeNumHidden = this.GetUIItemAccessLevel("HS_EDITUNSAFEWCONDNOTICE_NOTICENUMBER") == UIAccessLevel.Hidden;
            bool isNoticeDateHidden = this.GetUIItemAccessLevel("HS_EDITUNSAFEWCONDNOTICE_NOTICEDATE") == UIAccessLevel.Hidden;
            bool isReportPersonHidden = this.GetUIItemAccessLevel("HS_EDITUNSAFEWCONDNOTICE_REPORTINGPERSON") == UIAccessLevel.Hidden;
            bool isMilUnitHidden = this.GetUIItemAccessLevel("HS_EDITUNSAFEWCONDNOTICE_MILUNIT") == UIAccessLevel.Hidden;
            bool isViolationPlaceHidden = this.GetUIItemAccessLevel("HS_EDITUNSAFEWCONDNOTICE_VIOLATIONPLACE") == UIAccessLevel.Hidden;
            bool isResponsPersonHidden = this.GetUIItemAccessLevel("HS_EDITUNSAFEWCONDNOTICE_RESPONSIBLEPERSON") == UIAccessLevel.Hidden;
            bool isDangerDegreeHidden = this.GetUIItemAccessLevel("HS_EDITUNSAFEWCONDNOTICE_DANGERDEGREE") == UIAccessLevel.Hidden;
            bool isDescOfUCondHidden = this.GetUIItemAccessLevel("HS_EDITUNSAFEWCONDNOTICE_DESCOFUNSAFECOND") == UIAccessLevel.Hidden;
            bool isListOfViolReqHidden = this.GetUIItemAccessLevel("HS_EDITUNSAFEWCONDNOTICE_LISTOFVIOLATEDREQUIR") == UIAccessLevel.Hidden;
            bool isRiskReducDueDateHidden = this.GetUIItemAccessLevel("HS_EDITUNSAFEWCONDNOTICE_RISKREDUCINGDUEDATE") == UIAccessLevel.Hidden;
            bool isTempProcHidden = this.GetUIItemAccessLevel("HS_EDITUNSAFEWCONDNOTICE_TEMPPROCEDURES") == UIAccessLevel.Hidden;
            bool isFinalProcHidden = this.GetUIItemAccessLevel("HS_EDITUNSAFEWCONDNOTICE_FINALPROCEDURES") == UIAccessLevel.Hidden;
            bool isTempProcEstResHidden = this.GetUIItemAccessLevel("HS_EDITUNSAFEWCONDNOTICE_TEMPPROCESTRESULT") == UIAccessLevel.Hidden;
            bool isFinalProcEstResHidden = this.GetUIItemAccessLevel("HS_EDITUNSAFEWCONDNOTICE_FINALPROCESTRESULT") == UIAccessLevel.Hidden;
            bool isAddInfContPersonHidden = this.GetUIItemAccessLevel("HS_EDITUNSAFEWCONDNOTICE_ADDINFOCONTACTPERSON") == UIAccessLevel.Hidden;
            bool isAddContInfoHidden = this.GetUIItemAccessLevel("HS_EDITUNSAFEWCONDNOTICE_ADDCONTACTINFO") == UIAccessLevel.Hidden;

            string html = @"<table style='padding: 5px; width: 700px;'>
                                <tr style='min-height: 17px;'>
                                    <td align='right' style='width: 150px; vertical-align: top;'>"
                                    + (isNoticeNumHidden ? "&nbsp;" : "<span class='Label'>Сведение №:&nbsp;</span>") +
                                    @"</td>
                                    <td align='left' style='width: 180px; vertical-align: top;'>"
                                    + (isNoticeDateHidden ? "&nbsp;" : "<span class='ValueLabel'>" + unsafeWConditionsNotice.NoticeNumber + @"</span>") +
                                    @"</td>
                                    <td stile='width: 20px;'>&nbsp;</td>
                                    <td align='right' style='width: 160px; vertical-align: top;'>"
                                    + (isNoticeDateHidden ? "" : "<span class='Label'>Дата:&nbsp;</span>") +
                                    @"</td>
                                    <td align='left' style='width: 170px; vertical-align: top;'>"
                                    + (isNoticeDateHidden ? "" : "<span class='ValueLabel'>" + CommonFunctions.FormatDate(unsafeWConditionsNotice.NoticeDate) + @"</span>") +
                                    @"</td>
                                </tr>
                                <tr style='min-height: 17px;'>
                                    <td align='right' style='width: 150px; vertical-align: top;'>"
                                    + (isReportPersonHidden ? "" : "<span class='Label'>Докладващо лице:&nbsp;</span>") +
                                    @"</td>
                                    <td align='left' style='width: 180px; vertical-align: top;'>"
                                    + (isReportPersonHidden ? "" : "<span class='ValueLabel'>" + unsafeWConditionsNotice.ReportingPersonName + @"</span>") +
                                    @"</td>
                                    <td stile='width: 20px;'>&nbsp;</td>
                                    <td align='right' style='width: 160px; vertical-align: top;'>"
                                    + (isMilUnitHidden ? "" : "<span class='Label'>" + this.MilitaryUnitLabel + @":&nbsp;</span>") +
                                    @"</td>
                                    <td align='left' style='width: 170px; vertical-align: top;'>"
                                    + (isMilUnitHidden ? "" : "<span class='ValueLabel'>" + (unsafeWConditionsNotice.MilitaryUnit != null ? unsafeWConditionsNotice.MilitaryUnit.DisplayTextForSelection : "") + @"</span>") +
                                    @"</td>
                                </tr>
                                <tr style='min-height: 17px;'>
                                    <td align='right' rowspan='2' style='width: 150px; vertical-align: top;'>"
                                    + (isViolationPlaceHidden ? "" : "<span class='Label'>Местонахождение на нарушението:&nbsp;</span>") +
                                    @"</td>
                                    <td align='left' rowspan='2' style='width: 180px; vertical-align: top;'>"
                                    + (isViolationPlaceHidden ? "" : "<span class='ValueLabel'>" + CommonFunctions.ReplaceNewLinesInString(unsafeWConditionsNotice.ViolationPlace) + @"</span>") +
                                    @"</td>
                                    <td stile='width: 20px;'>&nbsp;</td>
                                    <td align='right' style='width: 160px; vertical-align: top;'>"
                                    + (isResponsPersonHidden ? "" : "<span class='Label'>Отговорно длъжностно лице:&nbsp;</span>") +
                                    @"</td>
                                    <td align='left' style='width: 170px; vertical-align: top;'>"
                                    + (isResponsPersonHidden ? "" : "<span class='ValueLabel'>" + unsafeWConditionsNotice.ResponsiblePersonName + @"</span>") +
                                    @"</td>
                                </tr>
                                <tr style='min-height: 17px;'>
                                    <td stile='width: 20px;'>&nbsp;</td>
                                    <td align='right' style='width: 160px; vertical-align: top;'>"
                                    + (isDangerDegreeHidden ? "" : "<span class='Label'>Степен на опасност:&nbsp;</span>") +
                                    @"</td>
                                    <td align='left' style='width: 170px; vertical-align: top;'>"
                                    + (isDangerDegreeHidden ? "" : "<span class='ValueLabel'>" + (unsafeWConditionsNotice.DangerDegree != null ? unsafeWConditionsNotice.DangerDegree.TableValue : "") + @"</span>") +
                                    @"</td>
                                </tr>
                                <tr style='line-height: 5px;'><td colspan='5'></td></tr>
                                <tr style='background-color: Black; line-height: 1px;'><td colspan='5'></td></tr>
                                <tr style='line-height: 5px;'><td colspan='5'></td></tr>
                                <tr style='min-height: 17px;'>
                                    <td align='left' colspan='2' style='width: 330px; vertical-align: top;'>"
                                    + (isDescOfUCondHidden ? "" : "<span class='Label'>Описание на нездравословно или опасно условие, включително брой хора изложени или заплашени от такова условие/я:&nbsp;</span>") +
                                    @"</td>
                                    <td stile='width: 20px;'>&nbsp;</td>
                                    <td align='left' colspan='2' style='width: 330px; vertical-align: top;'>"
                                    + (isListOfViolReqHidden ? "" : "<span class='Label'>Лист, по номер и/или име, на всяко от изискванията за здравословни и безопасни условия на труд, които може да са нарушени, ако са известни:&nbsp;</span>") +
                                    @"</td>
                                </tr>
                                <tr style='min-height: 17px;'>
                                    <td align='left' colspan='2' style='width: 330px; vertical-align: top;'>"
                                    + (isDescOfUCondHidden ? "" : "<span class='ValueLabel'>" + CommonFunctions.ReplaceNewLinesInString(unsafeWConditionsNotice.DescOfUnsafeCondition) + @"</span>") +
                                    @"</td>
                                    <td stile='width: 20px;'>&nbsp;</td>
                                    <td align='left' colspan='2' style='width: 330px; vertical-align: top;'>"
                                    + (isListOfViolReqHidden ? "" : "<span class='ValueLabel'>" + CommonFunctions.ReplaceNewLinesInString(unsafeWConditionsNotice.ListOfViolatedRequirements) + @"</span>") +
                                    @"</td>
                                </tr>
                                <tr style='line-height: 5px;'><td colspan='5'></td></tr>
                                <tr style='background-color: Black; line-height: 1px;'><td colspan='5'></td></tr>
                                <tr style='line-height: 5px;'><td colspan='5'></td></tr>
                                <tr style='min-height: 17px;'>
                                    <td align='left' colspan='5' style='vertical-align: top;'>"
                                    + (isRiskReducDueDateHidden ? "" : "<span class='Label'>Препоръчаните процедури за намаляване на риска да приключат до:&nbsp;</span>") +
                                    @"&nbsp;"
                                    + (isRiskReducDueDateHidden ? "" : "<span class='ValueLabel'>" + CommonFunctions.FormatDate(unsafeWConditionsNotice.RiskReducingDueDate) + @"</span>") +
                                    @"</td>
                                </tr>
                                <tr style='min-height: 17px;'>
                                    <td align='left' colspan='2' style='width: 330px; vertical-align: top;'>"
                                    + (isTempProcHidden ? "" : "<span class='Label'>Временни:&nbsp;</span>") +
                                    @"</td>
                                    <td stile='width: 20px;'>&nbsp;</td>
                                    <td align='left' colspan='2' style='width: 330px; vertical-align: top;'>"
                                    + (isFinalProcHidden ? "" : "<span class='Label'>Окончателни:&nbsp;</span>") +
                                    @"</td>
                                </tr>
                                <tr style='min-height: 17px;'>
                                    <td align='left' colspan='2' style='width: 330px; vertical-align: top;'>"
                                    + (isTempProcHidden ? "" : "<span class='ValueLabel'>" + CommonFunctions.ReplaceNewLinesInString(unsafeWConditionsNotice.TempProcedures) + @"</span>") +
                                    @"</td>
                                    <td stile='width: 20px;'>&nbsp;</td>
                                    <td align='left' colspan='2' style='width: 330px; vertical-align: top;'>"
                                    + (isFinalProcHidden ? "" : "<span class='ValueLabel'>" + CommonFunctions.ReplaceNewLinesInString(unsafeWConditionsNotice.FinalProcedures) + @"</span>") +
                                    @"</td>
                                </tr>
                                <tr style='min-height: 17px;'>
                                    <td align='left' colspan='2' style='width: 330px; vertical-align: top;'>"
                                    + (isTempProcEstResHidden ? "" : "<span class='Label'>Прогнозна стойност:&nbsp;</span><span class='ValueLabel'>" + unsafeWConditionsNotice.TempProceduresEstResult + @"</span>") +
                                    @"</td>
                                    <td stile='width: 20px;'>&nbsp;</td>
                                    <td align='left' colspan='2' style='width: 330px; vertical-align: top;'>"
                                    + (isFinalProcEstResHidden ? "" : "<span class='Label'>Прогнозна стойност:&nbsp;</span><span class='ValueLabel'>" + unsafeWConditionsNotice.FinalProceduresEstResult + @"</span>") +
                                    @"</td>
                                </tr>
                                <tr style='line-height: 5px;'><td colspan='5'></td></tr>
                                <tr style='background-color: Black; line-height: 1px;'><td colspan='5'></td></tr>
                                <tr style='line-height: 5px;'><td colspan='5'></td></tr>
                                <tr style='min-height: 17px;'>
                                    <td align='center' colspan='5'>"
                                    + (isAddContInfoHidden && isAddInfContPersonHidden ? "" : "<span class='Label'>Допълнителна информация относно това нарушение може да бъде получена от:&nbsp;</span>") +
                                    @"</td>
                                </tr>
                                <tr style='min-height: 17px;'>
                                    <td align='center' colspan='5' style='vertical-align: top;'>
                                        <table>
                                            <tr>
                                                <td align='right' style='width: 200px; vertical-align: top;'>"
                                                + (isAddInfContPersonHidden ? "" : "<span class='Label'>Лице за връзка:&nbsp;</span>") +
                                                @"</td>
                                                <td align='left' style='width: 470px; vertical-align: top;'>"
                                                + (isAddInfContPersonHidden ? "" : "<span class='ValueLabel'>" + unsafeWConditionsNotice.AdditionalInfoContactPerson + @"</span>") +
                                                @"</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr style='min-height: 17px;'>
                                    <td align='center' colspan='5' style='vertical-align: top;'>
                                        <table>
                                            <tr>
                                                <td align='right' style='width: 200px; vertical-align: top;'>"
                                                + (isAddContInfoHidden ? "" : "<span class='Label'>Информация за връзка:&nbsp;</span>") +
                                                @"</td>
                                                <td align='left' style='width: 470px; vertical-align: top;'>"
                                                + (isAddContInfoHidden ? "" : "<span class='ValueLabel'>" + unsafeWConditionsNotice.AdditionalContactInfo + @"</span>") +
                                                @"</td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>";

            return html;
        }
    }
}
