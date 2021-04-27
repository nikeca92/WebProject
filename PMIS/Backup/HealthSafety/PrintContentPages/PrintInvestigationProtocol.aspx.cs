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

namespace PMIS.HealthSafety.PrintContentPages
{
    public partial class PrintInvestigationProtocol : HSPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            btnGenerateWord.Style.Add("display", "none");

            int investProtocolID = 0;

            if (int.TryParse(Request.Params["InvestigationProtocolID"], out investProtocolID))
            {
                InvestigationProtocol investProtocol = InvestigationProtocolUtil.GetInvestigationProtocol(investProtocolID, CurrentUser);

                // Check visibility right for the print screen
                bool screenHidden = (this.GetUIItemAccessLevel("HS_EDITINVPROTOCOL") == UIAccessLevel.Hidden)
                        || (this.GetUIItemAccessLevel("HS_INVPROTOCOLS") == UIAccessLevel.Hidden);

                if (investProtocol != null && !screenHidden)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table>");
                    sb.Append("<tr>");
                    sb.Append("<td rowspan=\"2\">" + this.GenerateInvestigationProtocolHtml(investProtocol) + "</td>");
                    sb.Append("<td style=\"vertical-align: top;\">" + CommonFunctions.GenerateButtons(true, 160, false, true) + "</td>");
                    sb.Append("</tr>");
                    sb.Append("<tr><td style=\"vertical-align: bottom;\">" + CommonFunctions.GenerateButtons(false, 0, false, true) + "</td></tr>");
                    sb.Append("</table>");

                    this.divResults.InnerHtml = sb.ToString();
                }
                else
                {
                    this.divResults.InnerHtml = "";
                }
            }
        }

        // Generates html content related to contextual investigation protocol
        private string GenerateInvestigationProtocolHtml(InvestigationProtocol investProtocol)
        {
            bool isInvProtNumberHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_INVESTIGAITONPROTOCOLNUMBER") == UIAccessLevel.Hidden;
            bool isInvProtDateHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_INVPROTDATE") == UIAccessLevel.Hidden;
            bool isDeclarationHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_DECLARATIONID") == UIAccessLevel.Hidden;
            bool isDateFromHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_DATEFROM") == UIAccessLevel.Hidden;
            bool isDateToHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_DATETO") == UIAccessLevel.Hidden;
            bool isLegalReasHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_LEGALREASON") == UIAccessLevel.Hidden;
            bool isOrderNumHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_ORDERNUM") == UIAccessLevel.Hidden;
            bool isCommChairmanHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_COMMISSIONCHAIRMAN") == UIAccessLevel.Hidden;
            bool isCommMembersHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_COMMISSIONMEMBERS") == UIAccessLevel.Hidden;
            bool isInjuredHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_INJURED") == UIAccessLevel.Hidden;
            bool isAccidentPlaceHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_ACCIDENTDATEANDPLACE") == UIAccessLevel.Hidden;
            bool isWitnesHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_WITNESSES") == UIAccessLevel.Hidden;
            bool isJobGenDescHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_JOBGENERALDESC") == UIAccessLevel.Hidden;
            bool isSpecTaskActHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_SPECIFICTASKACTIVITY") == UIAccessLevel.Hidden;
            bool isDeviatNormActHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_DEVIATIONOFNORMALACTIVITY") == UIAccessLevel.Hidden;
            bool isInjuryDetHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_INJURYDETAILS") == UIAccessLevel.Hidden;
            bool isAnalysAccCausHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_ANALYSISOFACCIDENTCAUSES") == UIAccessLevel.Hidden;
            bool isLegalViolatHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_LEGALVIOLATIONS") == UIAccessLevel.Hidden;
            bool isItrudHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_ITRUDERS") == UIAccessLevel.Hidden;
            bool isActToAvoidHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_ACTIONSTOAVOID") == UIAccessLevel.Hidden;
            bool isEnclosurHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_ENCLOSURES") == UIAccessLevel.Hidden;

            string html = @"
                        <table style='padding: 5px;'>
                            <tr style='min-height: 17px;'>
                                <td align='right' style='width: 110px;'>"
                                + (isInvProtNumberHidden ? "&nbsp;" : "<span class='Label'>№&nbsp;</span>") +
                                @"</td>
                                <td align='left' style='width: 570px;'>"
                                + (isInvProtNumberHidden ? "&nbsp;" : "<span class='ValueLabel'>" + investProtocol.InvestigaitonProtocolNumber.ToString() + @"</span>")
                                + (isInvProtDateHidden ? "&nbsp;" : "<span class='Label'>&nbsp;&nbsp;&nbsp;&nbsp;oт дата&nbsp;</span><span class='ValueLabel'>" + (investProtocol.InvProtDate.HasValue ? CommonFunctions.FormatDate(investProtocol.InvProtDate) : "") + @"</span>") +
                                @"</td>
                            </tr>";

                            if (!isDeclarationHidden)
                            {
                                html += @"
                                        <tr style='min-height: 17px;'>
                                            <td align='center' colspan='2'>
                                                <span class='Label'>за резултатите от разследване на злополука</span>
                                            </td>
                                        </tr>
                                        <tr style='min-height: 17px;'>
                                            <td align='right' style='width: 110px;'>
                                                <span class='Label'>станала на:&nbsp;</span>
                                            </td>
                                            <td align='left' style='width: 570px;'>
                                                <span class='ValueLabel'>" + ((investProtocol.DeclarationOfAccident != null && investProtocol.DeclarationOfAccident.DeclarationOfAccidentAcc != null) ? (investProtocol.DeclarationOfAccident.DeclarationOfAccidentAcc.AccDateTime.HasValue ? CommonFunctions.FormatDate(investProtocol.DeclarationOfAccident.DeclarationOfAccidentAcc.AccDateTime) : "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") : "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + @"</span>
                                                <span class='Label'>&nbsp;&nbsp;с&nbsp;</span>
                                                <span class='ValueLabel'>" + ((investProtocol.DeclarationOfAccident != null && investProtocol.DeclarationOfAccident.DeclarationOfAccidentWorker != null) ? investProtocol.DeclarationOfAccident.DeclarationOfAccidentWorker.WorkerFullName.ToString() : "") + @"</span>
                                            </td>
                                        </tr>";
                           }

                        html += @"
                                <tr style='min-height: 17px;'>
                                    <td align='right' style='width: 110px;'>"
                                    + (isDateFromHidden ? "&nbsp;" : "<span class='Label'>От&nbsp;</span>") +
                                    @"</td>
                                    <td align='left' style='width: 570px;'>"
                                    + (isDateFromHidden ? "&nbsp;" : "<span class='ValueLabel'>" + (investProtocol.DateFrom.HasValue ? CommonFunctions.FormatDate(investProtocol.DateFrom) : "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + @"</span>")
                                    + (isDateToHidden ? "&nbsp;" : "<span class='Label'>&nbsp;&nbsp;&nbsp;&nbsp;до&nbsp;</span><span class='ValueLabel'>" + (investProtocol.DateTo.HasValue ? CommonFunctions.FormatDate(investProtocol.DateTo) : "") + @"</span>") +
                                    @"</td>
                                </tr>";

                        if (!isLegalReasHidden)
                        {
                            html += @"
                                    <tr style='min-height: 17px;'>
                                        <td align='right' style='width: 110px;'>
                                            <span class='Label'>на основание:&nbsp;</span> 
                                        </td>
                                        <td align='left' style='width: 570px;'>
                                            <span class='ValueLabel'>" + investProtocol.LegalReason.ToString() + @"</span>
                                        </td>
                                    </tr>";
                        }

                        if (!isOrderNumHidden)
                        {
                            html += @"
                                    <tr style='min-height: 17px;'>
                                        <td align='right' style='width: 110px;'>
                                            <span class='Label'>и заповед №&nbsp;</span>
                                        </td>
                                        <td align='left' style='width: 570px;'>
                                            <span class='ValueLabel'>" + (!String.IsNullOrEmpty(investProtocol.OrderNum) ? investProtocol.OrderNum : "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;") + @"</span>
                                            <span class='Label'>&nbsp;комисия в състав:</span>
                                        </td>
                                    </tr>";
                        }

                        if (!isCommChairmanHidden)
                        {
                            html += @"
                                    <tr style='min-height: 17px;'>
                                        <td align='right' style='width: 110px;'>
                                            <span class='Label'>Председател:&nbsp;</span> 
                                        </td>
                                        <td align='left' style='width: 570px;'>
                                            <span class='ValueLabel'>" + investProtocol.CommissionChairman.ToString() + @"</span>
                                        </td>
                                    </tr>";
                        }

                        if (!isCommMembersHidden)
                        {
                            html += @"
                                    <tr style='min-height: 17px;'>
                                        <td align='right' style='width: 110px;'>
                                            <span class='Label'>Членове:&nbsp;</span> 
                                        </td>
                                        <td align='left' style='width: 570px;'>
                                            <span class='ValueLabel'>" + investProtocol.CommissionMember1.ToString() + @"</span>
                                        </td>
                                    </tr>
                                    <tr style='min-height: 17px;'>
                                        <td align='right' style='width: 110px;'>&nbsp;</td>
                                        <td align='left' style='width: 570px;'>
                                            <span class='ValueLabel'>" + investProtocol.CommissionMember2.ToString() + @"</span>
                                        </td>
                                    </tr>";

                            if (!String.IsNullOrEmpty(investProtocol.CommissionMember3))
                            {
                                html += @"
                                    <tr style='min-height: 17px;'>
                                        <td align='right' style='width: 110px;'>&nbsp;</td>
                                        <td align='left' style='width: 570px;'>
                                            <span class='ValueLabel'>" + investProtocol.CommissionMember3.ToString() + @"</span>
                                        </td>
                                    </tr>";
                            }

                            if (!String.IsNullOrEmpty(investProtocol.CommissionMember4))
                            {
                                html += @"
                                    <tr style='min-height: 17px;'>
                                        <td align='right' style='width: 110px;'>&nbsp;</td>
                                        <td align='left' style='width: 570px;'>
                                            <span class='ValueLabel'>" + investProtocol.CommissionMember4.ToString() + @"</span>
                                        </td>
                                    </tr>";
                            }

                            if (!String.IsNullOrEmpty(investProtocol.CommissionMember5))
                            {
                                html += @"
                                    <tr style='min-height: 17px;'>
                                        <td align='right' style='width: 110px;'>&nbsp;</td>
                                        <td align='left' style='width: 570px;'>
                                            <span class='ValueLabel'>" + investProtocol.CommissionMember5.ToString() + @"</span>
                                        </td>
                                    </tr>";
                            }
                        }

                        html += @"
                                <tr style='min-height: 17px;'>
                                    <td align='left' colspan='2' style='width: 680px;'>
                                        <span class='Label' style='margin-left: 50px;'>извърши разследване и установи следното:</span>
                                    </td>
                                </tr>";

                        if (!isInjuredHidden)
                        {
                            html += @"
                                    <tr style='min-height: 17px;'>
                                        <td align='left' colspan='2' style='width: 680px;'>
                                            <span class='Label'>I. Пострадал:</span><br />
                                            <span class='ValueLabel'>" + investProtocol.Injured.ToString() + @"&nbsp;</span>
                                        </td>
                                    </tr>
                                    <tr style='height: 5px;'>
                                        <td colspan='2' style='width: 680px;'></td>
                                    </tr>";
                        }

                        if (!isAccidentPlaceHidden)
                        {
                            html += @"
                                    <tr style='min-height: 17px;'>
                                        <td align='left' colspan='2' style='width: 680px;'>
                                            <span class='Label'>II. Място и време на злополуката:</span><br />
                                            <span class='ValueLabel'>" + investProtocol.AccidentDateAndPlace.ToString() + @"&nbsp;</span>
                                        </td>
                                    </tr>
                                    <tr style='height: 5px;'>
                                        <td colspan='2' style='width: 680px;'></td>
                                    </tr>";
                        }

                        if (!isWitnesHidden)
                        {
                            html += @"
                                    <tr style='min-height: 17px;'>
                                        <td align='left' colspan='2' style='width: 680px;'>
                                            <span class='Label'>III. Свидетели на злополуката и оказали първа помощ:</span><br />
                                            <span class='ValueLabel'>" + investProtocol.Witnesses.ToString() + @"&nbsp;</span>
                                        </td>
                                    </tr>
                                    <tr style='height: 5px;'>
                                        <td colspan='2' style='width: 680px;'></td>
                                    </tr>";
                        }

                        if (!isJobGenDescHidden)
                        {
                            html += @"
                                    <tr style='min-height: 17px;'>
                                        <td align='left' colspan='2' style='width: 680px;'>
                                            <span class='Label'>IV. Обща характеристика на работата, извършвана от пострадалия преди злополуката:</span><br />
                                            <span class='ValueLabel'>" + investProtocol.JobGeneralDesc.ToString() + @"&nbsp;</span>
                                        </td>
                                    </tr>
                                    <tr style='height: 5px;'>
                                        <td colspan='2' style='width: 680px;'></td>
                                    </tr>";
                        }

                        if (!isSpecTaskActHidden)
                        {
                            html += @"
                                    <tr style='min-height: 17px;'>
                                        <td align='left' colspan='2' style='width: 680px;'>
                                            <span class='Label'>V. Специфично физическо действие, извършвано от пострадалия преди злополуката и свързания с това действие материален фактор (предмет, вещество и др.):</span><br />
                                            <span class='ValueLabel'>" + investProtocol.SpecificTaskActivity.ToString() + @"&nbsp;</span>
                                        </td>
                                    </tr>
                                    <tr style='height: 5px;'>
                                        <td colspan='2' style='width: 680px;'></td>
                                    </tr>";
                        }

                        if (!isDeviatNormActHidden)
                        {
                            html += @"
                                    <tr style='min-height: 17px;'>
                                        <td align='left' colspan='2' style='width: 680px;'>
                                            <span class='Label'>VI. Отклонение от нормалните действия и условия и материалния фактор, свързан с тези отклонения:</span><br />
                                            <span class='ValueLabel'>" + investProtocol.DeviationOfNormalActivity.ToString() + @"&nbsp;</span>
                                        </td>
                                    </tr>
                                    <tr style='height: 5px;'>
                                        <td colspan='2' style='width: 680px;'></td>
                                    </tr>";
                        }

                        if (!isInjuryDetHidden)
                        {
                            html += @"
                                    <tr style='min-height: 17px;'>
                                        <td align='left' colspan='2' style='width: 680px;'>
                                            <span class='Label'>VII. Начин на увреждане и материалния фактор, причинил увреждането:</span><br />
                                            <span class='ValueLabel'>" + investProtocol.InjuryDetails.ToString() + @"&nbsp;</span>
                                        </td>
                                    </tr>
                                    <tr style='height: 5px;'>
                                        <td colspan='2' style='width: 680px;'></td>
                                    </tr>";
                        }

                        if (!isAnalysAccCausHidden)
                        {
                            html += @"
                                    <tr style='min-height: 17px;'>
                                        <td align='left' colspan='2' style='width: 680px;'>
                                            <span class='Label'>VIII. Анализ на причините за възникване на злополуката :</span><br />
                                            <span class='ValueLabel'>" + investProtocol.AnalysisOfAccidentCauses.ToString() + @"&nbsp;</span>
                                        </td>
                                    </tr>
                                    <tr style='height: 5px;'>
                                        <td colspan='2' style='width: 680px;'></td>
                                    </tr>";
                        }

                        if (!isLegalViolatHidden)
                        {
                            html += @"
                                    <tr style='min-height: 17px;'>
                                        <td align='left' colspan='2' style='width: 680px;'>
                                            <span class='Label'>IX. Допуснати нарушения на нормативни актове:</span><br />
                                            <span class='ValueLabel'>" + investProtocol.LegalViolations.ToString() + @"&nbsp;</span>
                                        </td>
                                    </tr>
                                    <tr style='height: 5px;'>
                                        <td colspan='2' style='width: 680px;'></td>
                                    </tr>";
                        }

                        if (!isItrudHidden)
                        {
                            html += @"
                                    <tr style='min-height: 17px;'>
                                        <td align='left' colspan='2' style='width: 680px;'>
                                            <span class='Label'>X. Лица допуснали нарушенията или на които се предлага търсенето на отговорност:</span><br />
                                            <span class='ValueLabel'>" + investProtocol.Itruders.ToString() + @"&nbsp;</span>
                                        </td>
                                    </tr>
                                    <tr style='height: 5px;'>
                                        <td colspan='2' style='width: 680px;'></td>
                                    </tr>";
                        }

                        if (!isActToAvoidHidden)
                        {
                            html += @"
                                    <tr style='min-height: 17px;'>
                                        <td align='left' colspan='2' style='width: 680px;'>
                                            <span class='Label'>XI. Необходими мерки за недопускане на подобни злополуки:</span><br />
                                            <span class='ValueLabel'>" + investProtocol.ActionsToAvoid.ToString() + @"&nbsp;</span>
                                        </td>
                                    </tr>
                                    <tr style='height: 5px;'>
                                        <td colspan='2' style='width: 680px;'></td>
                                    </tr>";
                        }

                        if (!isEnclosurHidden)
                        {
                            html += @"
                                    <tr style='min-height: 17px;'>
                                        <td align='left' colspan='2' style='width: 680px;'>
                                            <span class='Label'>XII. Приложения:</span><br />
                                            <span class='ValueLabel'>" + investProtocol.Enclosures.ToString() + @"&nbsp;</span>
                                        </td>
                                    </tr>";
                        }

            html += "</table>";

            return html;
        }

        private string GenerateWordExport()
        {
            int investProtocolID = int.Parse(Request.Params["InvestigationProtocolID"]);
            InvestigationProtocol investProtocol = InvestigationProtocolUtil.GetInvestigationProtocol(investProtocolID, CurrentUser);

            bool isInvProtNumberHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_INVESTIGAITONPROTOCOLNUMBER") == UIAccessLevel.Hidden;
            bool isInvProtDateHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_INVPROTDATE") == UIAccessLevel.Hidden;
            bool isDeclarationHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_DECLARATIONID") == UIAccessLevel.Hidden;
            bool isDateFromHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_DATEFROM") == UIAccessLevel.Hidden;
            bool isDateToHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_DATETO") == UIAccessLevel.Hidden;
            bool isLegalReasHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_LEGALREASON") == UIAccessLevel.Hidden;
            bool isOrderNumHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_ORDERNUM") == UIAccessLevel.Hidden;
            bool isCommChairmanHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_COMMISSIONCHAIRMAN") == UIAccessLevel.Hidden;
            bool isCommMembersHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_COMMISSIONMEMBERS") == UIAccessLevel.Hidden;
            bool isInjuredHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_INJURED") == UIAccessLevel.Hidden;
            bool isAccidentPlaceHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_ACCIDENTDATEANDPLACE") == UIAccessLevel.Hidden;
            bool isWitnesHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_WITNESSES") == UIAccessLevel.Hidden;
            bool isJobGenDescHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_JOBGENERALDESC") == UIAccessLevel.Hidden;
            bool isSpecTaskActHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_SPECIFICTASKACTIVITY") == UIAccessLevel.Hidden;
            bool isDeviatNormActHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_DEVIATIONOFNORMALACTIVITY") == UIAccessLevel.Hidden;
            bool isInjuryDetHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_INJURYDETAILS") == UIAccessLevel.Hidden;
            bool isAnalysAccCausHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_ANALYSISOFACCIDENTCAUSES") == UIAccessLevel.Hidden;
            bool isLegalViolatHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_LEGALVIOLATIONS") == UIAccessLevel.Hidden;
            bool isItrudHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_ITRUDERS") == UIAccessLevel.Hidden;
            bool isActToAvoidHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_ACTIONSTOAVOID") == UIAccessLevel.Hidden;
            bool isEnclosurHidden = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_ENCLOSURES") == UIAccessLevel.Hidden;

            string investigaitonProtocolNumber = "";
            string invProtDate = "";
            string accDateTime = "";
            string declarationOfAccidentWorker = "";
            string dateFrom = "";
            string dateTo = "";
            string legalReason = "";
            string orderNum = "";
            string commissionChairman = "";
            string commissionMember1 = "";
            string commissionMember2 = "";
            string commissionMember3 = "";
            string commissionMember4 = "";
            string commissionMember5 = "";
            string signCommissionMember1 = "";
            string signCommissionMember2 = "";
            string signCommissionMember3 = "";
            string signCommissionMember4 = "";
            string signCommissionMember5 = "";
            string injured = "";
            string accidentPlace = "";
            string witnesses = "";
            string jobGeneralDesc = "";
            string specificTaskActivity = "";
            string deviationOfNormalActivity = "";
            string injuryDetails = "";
            string analysisOfAccidentCauses = "";
            string legalViolations = "";
            string itruders = "";
            string actionsToAvoid = "";
            string enclosures = "";

            if(!isInvProtNumberHidden)
                investigaitonProtocolNumber = investProtocol.InvestigaitonProtocolNumber.ToString();

            if(!isInvProtDateHidden)
                invProtDate = (investProtocol.InvProtDate.HasValue ? CommonFunctions.FormatDate(investProtocol.InvProtDate) + " г." : "");

            if (!isDeclarationHidden)
            {
                accDateTime = ((investProtocol.DeclarationOfAccident != null && investProtocol.DeclarationOfAccident.DeclarationOfAccidentAcc != null) ? (investProtocol.DeclarationOfAccident.DeclarationOfAccidentAcc.AccDateTime.HasValue ? CommonFunctions.FormatDate(investProtocol.DeclarationOfAccident.DeclarationOfAccidentAcc.AccDateTime) + " г." : "&nbsp;") : "&nbsp;");
                declarationOfAccidentWorker = ((investProtocol.DeclarationOfAccident != null && investProtocol.DeclarationOfAccident.DeclarationOfAccidentWorker != null) ? investProtocol.DeclarationOfAccident.DeclarationOfAccidentWorker.WorkerFullName.ToString() : "");
            }

            if (!isDateFromHidden)
                dateFrom = (investProtocol.DateFrom.HasValue ? CommonFunctions.FormatDate(investProtocol.DateFrom) + " г." : "&nbsp;");

            if (!isDateToHidden)
                dateTo = (investProtocol.DateTo.HasValue ? CommonFunctions.FormatDate(investProtocol.DateTo) + " г." : "");

            if (!isLegalReasHidden)
                legalReason = investProtocol.LegalReason.ToString();

            if (!isOrderNumHidden)
                orderNum = (!String.IsNullOrEmpty(investProtocol.OrderNum) ? investProtocol.OrderNum : "&nbsp;");

            if (!isCommChairmanHidden)
                commissionChairman = investProtocol.CommissionChairman.ToString();

            if (!isCommMembersHidden)
            {
                commissionMember1 = investProtocol.CommissionMember1.ToString();
                commissionMember2 = investProtocol.CommissionMember2.ToString();
                commissionMember3 = investProtocol.CommissionMember3.ToString();
                commissionMember4 = investProtocol.CommissionMember4.ToString();
                commissionMember5 = investProtocol.CommissionMember5.ToString();

                string signSpace = CommonFunctions.Replicate(".", 30);

                if (!String.IsNullOrEmpty(commissionMember1))
                {
                    signCommissionMember1 = "<b>1.</b>" + signSpace + "/" + commissionMember1 + "/";
                    commissionMember1 = "&#09;<b>1. </b>" + commissionMember1 + "<br />";
                }

                if (!String.IsNullOrEmpty(commissionMember2))
                {
                    signCommissionMember2 = "<b>2.</b>" + signSpace + "/" + commissionMember2 + "/";
                    commissionMember2 = "&#09;&#09;<b>2. </b>" + commissionMember2 + "<br />";
                }

                if (!String.IsNullOrEmpty(commissionMember3))
                {
                    signCommissionMember3 = "<b>3.</b>" + signSpace + "/" + commissionMember3 + "/";
                    commissionMember3 = "&#09;&#09;<b>3. </b>" + commissionMember3 + "<br />";
                }

                if (!String.IsNullOrEmpty(commissionMember4))
                {
                    signCommissionMember4 = "<b>4.</b>" + signSpace + "/" + commissionMember4 + "/";
                    commissionMember4 = "&#09;&#09;<b>4. </b>" + commissionMember4 + "<br />";
                }

                if (!String.IsNullOrEmpty(commissionMember5))
                {
                    signCommissionMember5 = "<b>5.</b>" + signSpace + "/" + commissionMember5 + "/";
                    commissionMember5 = "&#09;&#09;<b>5. </b>" + commissionMember5 + "<br />";
                }
            }

            if (!isInjuredHidden)
                injured = CommonFunctions.ReplaceNewLinesInString(investProtocol.Injured.ToString());

            if (!isAccidentPlaceHidden)
                accidentPlace = CommonFunctions.ReplaceNewLinesInString(investProtocol.AccidentDateAndPlace.ToString());

            if (!isWitnesHidden)
                witnesses = CommonFunctions.ReplaceNewLinesInString(investProtocol.Witnesses.ToString());

            if (!isJobGenDescHidden)
                jobGeneralDesc = CommonFunctions.ReplaceNewLinesInString(investProtocol.JobGeneralDesc.ToString());

            if (!isSpecTaskActHidden)
                specificTaskActivity = CommonFunctions.ReplaceNewLinesInString(investProtocol.SpecificTaskActivity.ToString());

            if (!isDeviatNormActHidden)
                deviationOfNormalActivity = CommonFunctions.ReplaceNewLinesInString(investProtocol.DeviationOfNormalActivity.ToString());

            if (!isInjuryDetHidden)
                injuryDetails = CommonFunctions.ReplaceNewLinesInString(investProtocol.InjuryDetails.ToString());

            if (!isAnalysAccCausHidden)
                analysisOfAccidentCauses = CommonFunctions.ReplaceNewLinesInString(investProtocol.AnalysisOfAccidentCauses.ToString());

            if (!isLegalViolatHidden)
                legalViolations = CommonFunctions.ReplaceNewLinesInString(investProtocol.LegalViolations.ToString());

            if (!isItrudHidden)
                itruders = CommonFunctions.ReplaceNewLinesInString(investProtocol.Itruders.ToString());

            if (!isActToAvoidHidden)
                actionsToAvoid = CommonFunctions.ReplaceNewLinesInString(investProtocol.ActionsToAvoid.ToString());

            if (!isEnclosurHidden)
                enclosures = CommonFunctions.ReplaceNewLinesInString(investProtocol.Enclosures.ToString());

            string html = @"<html xmlns:v=""urn:schemas-microsoft-com:vml""
xmlns:o=""urn:schemas-microsoft-com:office:office""
xmlns:w=""urn:schemas-microsoft-com:office:word"" 
charset=""UTF-8"" >
<head>
   <style>
     @page Section1
	 {   
         size: 595.3pt 841.9pt;
	     margin: 0.98in 0.64in 0.98in 0.98in;
     }

     div.Section1
	 {page:Section1;}

      body
      {
         font-family: ""Times New Roman"";
         font-size: 12pt;
      }

      p
      {
         margin-top: 0px;
         margin-bottom: 0px;
      }

      .Header1
      {
         font-size: 17pt;
         font-weight: bold;
         text-align: center;
         margin-top: 8pt;
         margin-bottom: 8pt;
         letter-spacing: 2pt;
      }

      .Header2
      {
         font-size: 13pt;
         font-weight: bold;
         text-align: center;
         margin-top: 8pt;
         margin-bottom: 8pt;
         letter-spacing: 1.2pt;
      }

      .Header3
      {
         font-size: 12pt;
         font-weight: bold;
         text-align: center;
         margin-bottom: 16pt;
      }

      .NormalText
      {
         text-indent: 40pt;
         line-height: 115%;
      }

      .SectionLabel
      {
         text-indent: 40pt;
         line-height: 115%;
         font-weight: bold;
         margin-top: 15pt;
      }

      .SignatureSection
      {
         text-indent: 170pt;
         line-height: 115%;
         margin-top: 10pt;
      }
   </style>

   <xml>
      <w:WordDocument>
         <w:View>Print</w:View>
         <w:Zoom>100</w:Zoom>
      </w:WordDocument>
   </xml>
</head>
<body lang=BG>
   <div class=""Section1"">
      <p class='Header1'>ПРОТОКОЛ</p>
      <p class='Header2'>ЗА РЕЗУЛТАТИТЕ ОТ РАЗСЛЕДВАНЕ НА ЗЛОПОЛУКАТА</p>
      <p class='Header3'>№ " + investigaitonProtocolNumber + @" от дата " + invProtDate + @"</p>
      <p class='NormalText'>станала на " + accDateTime + @" с <span>" + declarationOfAccidentWorker + @"</span></p>
      <p class='NormalText'>&nbsp;</p>
      <p class='NormalText'>От " + dateFrom + @" до " + dateTo + @" на основание " + legalReason + @"
         и заповед " + orderNum + @", комисия в състав:<br />
         Председател:&#09;" + commissionChairman + @"<br />
         Членове:" + commissionMember1 + @"
                 " + commissionMember2 + @"
                 " + commissionMember3 + @"
                 " + commissionMember4 + @"
                 " + commissionMember5 + @"
         <br />
         <b>извърши разследване и установи следното:</b>
      </p>
      <p class='SectionLabel' style='margin-top: 0px;'>I. Пострадал</p>
      <p class='NormalText'>" + injured + @"</p>
      <p class='SectionLabel'>II. Място и време на злополуката</p>
      <p class='NormalText'>" + accidentPlace + @"</p>
      <p class='SectionLabel'>III. Свидетели на злополуката и оказали първа помощ</p>
      <p class='NormalText'>" + witnesses + @"</p>
      <p class='SectionLabel'>IV. Обща характеристика на работата, извършвана от пострадалия преди злополуката</p>
      <p class='NormalText'>" + jobGeneralDesc + @"</p>
      <p class='SectionLabel'>V. Специфично физическо действие, извършвано от пострадалия преди злополуката и свързания с това действие материален фактор (предмет, вещество и др.)</p>
      <p class='NormalText'>" + specificTaskActivity + @"</p>
      <p class='SectionLabel'>VI. Отклонение от нормалните действия и условия и материалния фактор, свързан с тези отклонения</p>
      <p class='NormalText'>" + deviationOfNormalActivity + @"</p>
      <p class='SectionLabel'>VII. Начин на увреждане и материалния фактор, причинил увреждането</p>
      <p class='NormalText'>" + injuryDetails + @"</p>
      <p class='SectionLabel'>VIII. Анализ на причините за възникване на злополуката</p>
      <p class='NormalText'>" + analysisOfAccidentCauses + @"</p>
      <p class='SectionLabel'>IX. Допуснати нарушения на нормативни актове</p>
      <p class='NormalText'>" + legalViolations + @"</p>
      <p class='SectionLabel'>X. Лица допуснали нарушенията или на които се предлага търсенето на отговорност</p>
      <p class='NormalText'>" + itruders + @"</p>
      <p class='SectionLabel'>XI. Необходими мерки за недопускане на подобни злополуки</p>
      <p class='NormalText'>" + actionsToAvoid + @"</p>
      <p class='SectionLabel'>XII. Приложения</p>
      <p class='NormalText'>" + enclosures + @"</p>
      <p class='SignatureSection' style='margin-top: 30pt;'><b>Председател:</b>................../" + commissionChairman + @"/</p>
      <p class='SignatureSection'><b>Членове:</b></p>
      <p class='SignatureSection'>" + signCommissionMember1 + @"</p>
      <p class='SignatureSection'>" + signCommissionMember2 + @"</p>
      <p class='SignatureSection'>" + signCommissionMember3 + @"</p>
      <p class='SignatureSection'>" + signCommissionMember4 + @"</p>
      <p class='SignatureSection'>" + signCommissionMember5 + @"</p>
    </div>
</body>
</html>";
            return html;
        }

        protected void btnGenerateWord_Click(object sender, EventArgs e)
        {
            string result = GenerateWordExport();

            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment;filename=InvestigationProtocol.doc");
            Response.ContentType = "application/vnd.ms-word";

            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            Response.Write(result);
            Response.End();
        }
    }
}
