using System;
using System.Text;

using PMIS.Common;
using PMIS.HealthSafety.Common;

namespace PMIS.HealthSafety.PrintContentPages
{
    public partial class PrintProtocol : HSPage
    {
        private string MilitaryUnitLabel = CommonFunctions.GetLabelText("MilitaryUnit");

        protected void Page_Load(object sender, EventArgs e)
        {
            int protocolID = 0;

            if (int.TryParse(Request.Params["ProtocolID"], out protocolID))
            {
                Protocol protocol = ProtocolUtil.GetProtocol(protocolID, CurrentUser);

                // Check visibility right for the print screen
                bool screenHidden = (this.GetUIItemAccessLevel("HS_EDITPROT") == UIAccessLevel.Hidden)
                        || (this.GetUIItemAccessLevel("HS_PROTOCOLS") == UIAccessLevel.Hidden);

                if (protocol != null && !screenHidden)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table>");
                    sb.Append("<tr>");
                    sb.Append("<td rowspan=\"2\">" + this.GenerateProtocolHtml(protocol) + "</td>");
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

        // Generates html content related to contextual protocol
        private string GenerateProtocolHtml(Protocol protocol)
        {
            bool isProtNumHidden = this.GetUIItemAccessLevel("HS_EDITPROT_PROTNUM") == UIAccessLevel.Hidden;
            bool isProtDateHidden = this.GetUIItemAccessLevel("HS_EDITPROT_PROTDATE") == UIAccessLevel.Hidden;
            bool isProtTypeHidden = this.GetUIItemAccessLevel("HS_EDITPROT_PROTTYPE") == UIAccessLevel.Hidden;
            bool isProtMilUnitHidden = this.GetUIItemAccessLevel("HS_EDITPROT_PROTMILUNIT") == UIAccessLevel.Hidden;
            bool isProtAddressHidden = this.GetUIItemAccessLevel("HS_EDITPROT_PROTADDRESS") == UIAccessLevel.Hidden;
            bool isProtObjectHidden = this.GetUIItemAccessLevel("HS_EDITPROT_PROTOBJECT") == UIAccessLevel.Hidden;
            bool isProtRequestHidden = this.GetUIItemAccessLevel("HS_EDITPROT_PROTREQUESTING") == UIAccessLevel.Hidden;
            bool isProtUsedEquipHidden = this.GetUIItemAccessLevel("HS_EDITPROT_PROTUSEDEQUIP") == UIAccessLevel.Hidden;
            bool isProtMeasureDateHidden = this.GetUIItemAccessLevel("HS_EDITPROT_PROTMEASURDATE") == UIAccessLevel.Hidden;
            bool isProtNormDocHidden = this.GetUIItemAccessLevel("HS_EDITPROT_PROTNORMDOC") == UIAccessLevel.Hidden;
            bool isProtPeopPresentHidden = this.GetUIItemAccessLevel("HS_EDITPROT_PROTPEOPLEPRESENT") == UIAccessLevel.Hidden;
            bool isProtMeasureMethodHidden = this.GetUIItemAccessLevel("HS_EDITPROT_PROTMEASURMETHOD") == UIAccessLevel.Hidden;

            string html = @"<table style='padding: 5px;'>
                                <tr style='min-height: 17px;'>
                                    <td align='right' style='width: 150px;'>"
                                    + (isProtNumHidden ? "&nbsp;" : "<span class='Label'>Протокол №:&nbsp;</span>") +
                                    @"</td>
                                    <td align='left' style='width: 195px;'>"
                                    + (isProtNumHidden ? "&nbsp;" : "<span class='ValueLabel'>" + protocol.ProtocolNumber.ToString() + @"</span>") +
                                    @"</td>
                                    <td align='right' style='width: 140px;'>"
                                    + (isProtDateHidden ? "&nbsp;" : "<span class='Label'>от дата:&nbsp;</span>") +
                                    @"</td>
                                    <td align='left' style='width: 195px;'>"
                                    + (isProtDateHidden ? "&nbsp;" : "<span class='ValueLabel'>" + CommonFunctions.FormatDate(protocol.ProtocolDate) + @"</span>") +
                                    @"</td>
                                </tr>
                                <tr style='min-height: 17px;'>
                                    <td align='right' style='width: 150px;'>"
                                    + (isProtTypeHidden ? "&nbsp;" : "<span class='Label'>от измерване:&nbsp;</span>") +
                                    @"</td>
                                    <td align='left' colspan='3' style='width: 530px;'>"
                                    + (isProtTypeHidden ? "&nbsp;" : "<span class='ValueLabel'>" + (protocol.ProtocolType != null ? protocol.ProtocolType.ProtocolTypeName.ToString() : "") + @"</span>") +
                                    @"</td>
                                </tr>
                                <tr style='min-height: 17px;'>
                                    <td align='right' style='width: 150px;'>"
                                    + (isProtMilUnitHidden ? "&nbsp;" : "<span class='Label'>" + this.MilitaryUnitLabel + @":&nbsp;</span>") +
                                    @"</td>
                                    <td align='left' style='width: 195px;'>"
                                    + (isProtMilUnitHidden ? "&nbsp;" : "<span class='ValueLabel'>" + (protocol.MilitaryUnit != null ? protocol.MilitaryUnit.DisplayTextForSelection : "") + @"</span>") +
                                    @"</td>
                                    <td align='right' style='width: 140px; vertical-align: top;' rowspan='2'>"
                                    + (isProtAddressHidden ? "&nbsp;" : "<span class='Label'>Адрес:&nbsp;</span>") +
                                    @"</td>
                                    <td align='left' style='width: 195px; vertical-align: top;' rowspan='2'>"
                                    + (isProtAddressHidden ? "&nbsp;" : "<span class='ValueLabel'>" + CommonFunctions.ReplaceNewLinesInString(protocol.Address.ToString()) + @"</span>") +
                                    @"</td>
                                </tr>
                                <tr style='min-height: 17px;'>
                                    <td align='right' style='width: 150px;'>"
                                    + (isProtObjectHidden ? "&nbsp;" : "<span class='Label'>Обект:&nbsp;</span>") +
                                    @"</td>
                                    <td align='left' style='width: 195px;'>"
                                    + (isProtObjectHidden ? "&nbsp;" : "<span class='ValueLabel'>" + protocol.Obekt.ToString() + @"</span>") +
                                    @"</td>
                                </tr>
                                <tr style='min-height: 17px;'>
                                    <td align='right' style='width: 150px;'>"
                                    + (isProtRequestHidden ? "&nbsp;" : "<span class='Label'>Заявител:&nbsp;</span>") +
                                    @"</td>
                                    <td align='left' style='width: 195px;'>"
                                    + (isProtRequestHidden ? "&nbsp;" : "<span class='ValueLabel'>" + protocol.Requesting.ToString() + @"</span>") +
                                    @"</td>
                                    <td align='right' style='width: 140px; vertical-align: top;' rowspan='2'>"
                                    + (isProtUsedEquipHidden ? "&nbsp;" : "<span class='Label'>Използвана апаратура:&nbsp;</span>") +
                                    @"</td>
                                    <td align='left' style='width: 195px; vertical-align: top;' rowspan='2'>"
                                    + (isProtUsedEquipHidden ? "&nbsp;" : "<span class='ValueLabel'>" + CommonFunctions.ReplaceNewLinesInString(protocol.UsedEquipments.ToString()) + @"</span>") +
                                    @"</td>
                                </tr>
                                <tr style='min-height: 17px;'>
                                    <td align='right' style='width: 150px;'>"
                                        + (isProtMeasureDateHidden ? "&nbsp;" : "<span class='Label'>Дата:&nbsp;</span>") +
                                    @"</td>
                                    <td align='left' style='width: 195px;'>"
                                        + (isProtMeasureDateHidden ? "&nbsp;" : "<span class='ValueLabel'>" + CommonFunctions.FormatDate(protocol.MeasurementDate) + @"</span>") +
                                    @"</td>
                                </tr>
                                <tr style='min-height: 17px;'>
                                    <td align='right' style='width: 150px;'>"
                                        + (isProtNormDocHidden ? "&nbsp;" : "<span class='Label'>Нормативен документ:&nbsp;</span>") +
                                    @"</td>
                                    <td align='left' style='width: 195px;'>"
                                        + (isProtNormDocHidden ? "&nbsp;" : "<span class='ValueLabel'>" + protocol.NormativeDocument.ToString() + @"</span>") +
                                    @"</td>
                                    <td align='right' style='width: 140px; vertical-align: top;' rowspan='2'>"
                                        + (isProtPeopPresentHidden ? "&nbsp;" : "<span class='Label'>Присъствали на измерванията:&nbsp;</span>") +
                                    @"</td>
                                    <td align='left' style='width: 195px; vertical-align: top;' rowspan='2'>"
                                        + (isProtPeopPresentHidden ? "&nbsp;" : "<span class='ValueLabel'>" + CommonFunctions.ReplaceNewLinesInString(protocol.PeoplePresent.ToString()) + @"</span>") +
                                    @"</td>
                                </tr>
                                <tr style='min-height: 17px;'>
                                    <td align='right' style='width: 150px;'>"
                                        + (isProtPeopPresentHidden ? "&nbsp;" : "<span class='Label'>Метод на измерване:&nbsp;</span>") +
                                    @"</td>
                                    <td align='left' style='width: 195px;'>"
                                        + (isProtPeopPresentHidden ? "&nbsp;" : "<span class='ValueLabel'>" + protocol.MeasurementMethod.ToString() + @"</span>") +
                                    @"</td>
                                </tr>";

            bool isPIWorkPlaceHidden = this.GetUIItemAccessLevel("HS_EDITPROT_PROTITEMS_WORKPLACE") == UIAccessLevel.Hidden;
            bool isPIWorkPeopleHidden = this.GetUIItemAccessLevel("HS_EDITPROT_PROTITEMS_WORKPEOPLE") == UIAccessLevel.Hidden;
            bool isPIMeasureHidden = this.GetUIItemAccessLevel("HS_EDITPROT_PROTITEMS_MEAS") == UIAccessLevel.Hidden;
            bool isPIMeasuredHidden = this.GetUIItemAccessLevel("HS_EDITPROT_PROTITEMS_MEASURED") == UIAccessLevel.Hidden;
            bool isPIOtherHidden = this.GetUIItemAccessLevel("HS_EDITPROT_PROTITEMS_OTHER") == UIAccessLevel.Hidden;

            if (!(this.GetUIItemAccessLevel("HS_EDITPROT_PROTITEMS") == UIAccessLevel.Hidden
                || isPIWorkPlaceHidden && isPIWorkPeopleHidden && isPIMeasureHidden && isPIMeasuredHidden && isPIOtherHidden))
            {
                html += @"
                    <tr><td colspan='4' align='center'>
                    <table id='protocolItemsTable' name='protocolItemsTable' class='CommonHeaderTable'>
                        <thead>
                            <tr>
                                <th style='width: 100px; border-left: 1px solid #000000;'>Място на измерване</th>
                                <th style='width: 65px;'>Брой хора</th>
                                <th style='width: 150px;'>Измервана величина</th>
                                <th style='width: 75px;'>Измерена стойност</th>
                                <th style='width: 75px;'>Гранична стойност</th>
                                <th style='width: 75px;'>Разлика</th>
                                <th style='width: 120px; border-right: 1px solid #000000;'>Допълнителна информация</th>
                            </tr>
                        </thead><tbody>";
                int counter = 1;

                foreach (ProtocolItem pItem in protocol.ProtocolItems)
                {
                    html += @"<tr>
                            <td align='left'>" + (isPIWorkPlaceHidden ? "&nbsp;" : pItem.WorkingPlace.WorkingPlaceName.ToString()) + @"</td>
                            <td align='center'>" + (isPIWorkPeopleHidden ? "&nbsp;" : pItem.WorkingPeople.ToString()) + @"</td>
                            <td align='left'>" + (isPIMeasureHidden ? "&nbsp;" : (pItem.Measure != null ? pItem.Measure.MeasureName : "&nbsp;")) + @"</td>
                            <td align='center'>" + (isPIMeasuredHidden ? "&nbsp;" : CommonFunctions.FormatDecimal(pItem.Measured)) + @"</td>
                            <td align='center'>" + (isPIMeasuredHidden ? "&nbsp;" : CommonFunctions.FormatDecimal(pItem.Threshold)) + @"</td>
                            <td align='center'>" + (isPIMeasuredHidden ? "&nbsp;" : CommonFunctions.FormatDecimal(Math.Abs(pItem.Threshold - pItem.Measured))) + @"</td>
                            <td align='left'>" + (isPIOtherHidden ? "&nbsp;" : CommonFunctions.ReplaceNewLinesInString(pItem.Other.ToString())) + @"</td>
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
