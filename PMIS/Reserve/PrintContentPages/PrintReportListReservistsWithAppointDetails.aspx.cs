using System;
using System.Linq;
using System.Text;

using PMIS.Common;
using PMIS.Reserve.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace PMIS.Reserve.PrintContentPages
{
    public partial class PrintReportListReservistsWithAppointDetails : RESPage
    {
        //This property represents the ID of the EquipmentReservistsRequest object that is loaded on the screen
        //If this is a new object then the ID is 0
        //It is stored in a hidden field on the page
        private int EquipmentReservistsRequestId
        {
            get
            {
                int equipmentReservistsRequestId = 0;
                if (String.IsNullOrEmpty(this.hfEquipmentReservistsRequestID.Value)
                    || this.hfEquipmentReservistsRequestID.Value == "0")
                {
                    if (Request.Params["EquipmentReservistsRequestId"] != null)
                        int.TryParse(Request.Params["EquipmentReservistsRequestId"].ToString(), out equipmentReservistsRequestId);

                    this.hfEquipmentReservistsRequestID.Value = equipmentReservistsRequestId.ToString();
                }
                else
                {
                    Int32.TryParse(this.hfEquipmentReservistsRequestID.Value, out equipmentReservistsRequestId);
                }

                return equipmentReservistsRequestId;
            }

            set
            {
                this.hfEquipmentReservistsRequestID.Value = value.ToString();
            }
        }

        public void SetPrintTitleLinesWidth()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnPrintTitleLinesWidth");

            hf.Value = "765";
        }

        public void SetHeadersLeft()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnHeadersLeft");

            hf.Value = "60";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.SetPrintTitleLinesWidth();
                this.SetHeadersLeft();
            }

            // Check visibility right for the print screen
            if (GetUIItemAccessLevel("RES_REPORTS") != UIAccessLevel.Hidden &&
                this.GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS") != UIAccessLevel.Hidden)
            {
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
                sb.Append("<td style=\"vertical-align: top;\">" + CommonFunctions.GenerateButtons(true, 155, true) + "</td>");
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
            bool isMilCommandHidden = GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_DETAILS_MILCOMMAND") == UIAccessLevel.Hidden;
            bool isPositionHidden = GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_DETAILS_POSITION") == UIAccessLevel.Hidden;
            bool isMilRepSpecialityHidden = GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_DETAILS_MILREPSPECIALITY") == UIAccessLevel.Hidden;
            bool isMilRankHidden = GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_DETAILS_MILRANK") == UIAccessLevel.Hidden;
            bool isReadinessHidden = GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_DETAILS_READINESS") == UIAccessLevel.Hidden;
            bool isIdentityNumberHidden = GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_DETAILS_IDENTITYNUMBER") == UIAccessLevel.Hidden;
            bool isFullNameHidden = GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_DETAILS_FULLNAME") == UIAccessLevel.Hidden;
            bool isPermCityNameHidden = GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_DETAILS_PERMCITYNAME") == UIAccessLevel.Hidden;

            bool isRequestNumberHidden = GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_REQUEST_REQNUMBER") == UIAccessLevel.Hidden;
            bool isRequestDateHidden = GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_REQUEST_REQDATE") == UIAccessLevel.Hidden;
            bool isRequestStatusHidden = GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_REQUEST_REQSTATUS") == UIAccessLevel.Hidden;
            bool isMilitaryUnitHidden = GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_REQUEST_MILUNIT") == UIAccessLevel.Hidden;
            bool isAdministrationHidden = GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_REQUEST_ADMINISTRATION") == UIAccessLevel.Hidden;
            

            //Get the list of records according to the specified filters, order and paging
            List<ReportListReservistsWithAppointDetailsBlock> reportBlocks = ReportListReservistsWithAppointmentsBlockUtil.GetReportListReservistsWithAppointDetailsBlockList(EquipmentReservistsRequestId, CurrentUser);

            EquipmentReservistsRequest request = EquipmentReservistsRequestUtil.GetEquipmentReservistsRequest(EquipmentReservistsRequestId, CurrentUser);

            string requestNumberLabel = "Заявка №:";
            string requestDateLabel = "от дата:&nbsp;";
            string equipWithResRequestsStatusLabel = "Статус на заявката:";
            string militaryUnitLabel = "Заявката е от " + CommonFunctions.GetLabelText("MilitaryUnit") + ":";
            string administrationLabel = "От кое министерство/ведомство:";

            string requestNumber = request.RequestNumber;
            string requestDate = CommonFunctions.FormatDate(request.RequestDate);
            string equipWithResRequestsStatus = (request.EquipWithResRequestsStatus != null ? request.EquipWithResRequestsStatus.StatusName : "");
            string militaryUnit = (request.MilitaryUnit != null ? request.MilitaryUnit.DisplayTextForSelection : "");
            string administration = (request.Administration != null ? request.Administration.AdministrationName : "");

            if (isRequestNumberHidden)
            {
                requestNumber = "";
                requestNumberLabel = "";
            }

            if (isRequestDateHidden)
            {
                requestDate = "";
                requestDateLabel = "";
            }

            if (isRequestStatusHidden)
            {
                equipWithResRequestsStatus = "";
                equipWithResRequestsStatusLabel = "";
            }

            if (isMilitaryUnitHidden)
            {
                militaryUnit = "";
                militaryUnitLabel = "";
            }

            if (isAdministrationHidden)
            {
                administration = "";
                administrationLabel = "";
            }

            StringBuilder html = new StringBuilder();
            html.Append(@"<table style='padding: 5px;'>
                             <tr>
                                <td align='right' style='width: 125px;'>
                                    <span class='Label'>" + requestNumberLabel + @"</span>
                                </td>
                                <td align='left' style='width: 185px;'>
                                   <span class='ValueLabel'>" + requestNumber + @"</span>&nbsp;&nbsp;
                                   <span class='Label'>" + requestDateLabel + @"</span>
                                   <span class='ValueLabel'>" + requestDate + @"</span>&nbsp;
                                </td>
                                <td align='right' style='width: 150px;'>
                                   <span class='Label'>" + equipWithResRequestsStatusLabel + @"</span>
                                </td>
                                <td align='left' style='width: 245px;'>
                                   <span class='ValueLabel'>" + equipWithResRequestsStatus + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right'>
                                    <span class='Label'>" + militaryUnitLabel + @"</span>
                                </td>
                                <td align='left' style='vertical-align: bottom;'>
                                    <span class='ValueLabel'>" + militaryUnit + @"</span>
                                </td>
                                <td align='right'>
                                    <span class='Label'>" + administrationLabel + @"</span>
                                </td>
                                <td align='left' style='vertical-align: bottom;'>
                                    <span class='ValueLabel'>" + administration + @"</span>
                                </td>
                             </tr>");

            if (reportBlocks.Count() > 0)
            {
                html.Append(@"
                    <tr><td colspan='4' align='center'>
                    <table id='applicantsTable' name='applicantsTable' class='CommonHeaderTable'>
                        <thead>
                            <tr>
                                <th style='width: 15px; border-left: 1px solid #000000;'>№</th>" +
        (!isMilCommandHidden ? "<th style='width: 80px;'>Команда</th>" : "") +
          (!isPositionHidden ? "<th style='width: 110px;'>Длъжност</th>" : "") +
  (!isMilRepSpecialityHidden ? "<th style='width: 100px;'>ВОС</th>" : "") +
           (!isMilRankHidden ? "<th style='width: 70px;'>Звание</th>" : "") +
         (!isReadinessHidden ? "<th style='width: 80px;'>Начин на явяване</th>" : "") +
    (!isIdentityNumberHidden ? "<th style='width: 80px;'>ЕГН</th>" : "") +
          (!isFullNameHidden ? "<th style='width: 110px; border-right: 1px solid #000000;'>Име</th>" : "") +
      (!isPermCityNameHidden ? "<th style='width: 80px; border-right: 1px solid #000000;'>Населено място</th>" : "") + @"
                            </tr>
                        </thead><tbody>");
            }

            int counter = 1;

            foreach (ReportListReservistsWithAppointDetailsBlock reportBlock in reportBlocks)
            {
                html.Append(@"<tr>
                            <td align='center'>" + counter + @"</td>" +
         (!isMilCommandHidden ? "<td style='text-align: left;'>" + reportBlock.MilitaryCommand + @"</td>" : "") +
           (!isPositionHidden ? "<td style='text-align: left;'>" + reportBlock.Position + @"</td>" : "") +
   (!isMilRepSpecialityHidden ? "<td style='text-align: left;'>" + reportBlock.MilitaryReportingSpecialty + @"</td>" : "") +
            (!isMilRankHidden ? "<td style='text-align: left;'>" + reportBlock.MilitaryRank + @"</td>" : "") +
          (!isReadinessHidden ? "<td style='text-align: left;'>" + reportBlock.Readiness + @"</td>" : "") +
     (!isIdentityNumberHidden ? "<td style='text-align: left;'>" + reportBlock.IdentityNumber + @"</td>" : "") +
           (!isFullNameHidden ? "<td style='text-align: left;'>" + reportBlock.FullName + @"</td>" : "") +
       (!isPermCityNameHidden ? "<td style='text-align: left;'>" + reportBlock.PermPlaceName + @"</td>" : "") + @"
                          </tr>");

                counter++;
            }

            if (reportBlocks.Count() > 0)
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
            Response.AppendHeader("content-disposition", "attachment; filename=PrintReportListReservistsWithAppointDetails.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        // Generate html as excel response result
        private string GenerateAllRecordsForExport()
        {
            bool isMilCommandHidden = GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_DETAILS_MILCOMMAND") == UIAccessLevel.Hidden;
            bool isPositionHidden = GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_DETAILS_POSITION") == UIAccessLevel.Hidden;
            bool isMilRepSpecialityHidden = GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_DETAILS_MILREPSPECIALITY") == UIAccessLevel.Hidden;
            bool isMilRankHidden = GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_DETAILS_MILRANK") == UIAccessLevel.Hidden;
            bool isReadinessHidden = GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_DETAILS_READINESS") == UIAccessLevel.Hidden;
            bool isIdentityNumberHidden = GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_DETAILS_IDENTITYNUMBER") == UIAccessLevel.Hidden;
            bool isFullNameHidden = GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_DETAILS_FULLNAME") == UIAccessLevel.Hidden;
            bool isPermCityNameHidden = GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_DETAILS_PERMCITYNAME") == UIAccessLevel.Hidden;

            bool isRequestNumberHidden = GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_REQUEST_REQNUMBER") == UIAccessLevel.Hidden;
            bool isRequestDateHidden = GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_REQUEST_REQDATE") == UIAccessLevel.Hidden;
            bool isRequestStatusHidden = GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_REQUEST_REQSTATUS") == UIAccessLevel.Hidden;
            bool isMilitaryUnitHidden = GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_REQUEST_MILUNIT") == UIAccessLevel.Hidden;
            bool isAdministrationHidden = GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_REQUEST_ADMINISTRATION") == UIAccessLevel.Hidden;

            int visibleColumnsCount = 1;
            visibleColumnsCount += isMilCommandHidden ? 0 : 1;
            visibleColumnsCount += isPositionHidden ? 0 : 1;
            visibleColumnsCount += isMilRepSpecialityHidden ? 0 : 1;
            visibleColumnsCount += isMilRankHidden ? 0 : 1;
            visibleColumnsCount += isReadinessHidden ? 0 : 1;
            visibleColumnsCount += isIdentityNumberHidden ? 0 : 1;
            visibleColumnsCount += isFullNameHidden ? 0 : 1;
            visibleColumnsCount += isPermCityNameHidden ? 0 : 1;

            //Get the list of records according to the specified filters, order and paging
            List<ReportListReservistsWithAppointDetailsBlock> reportBlocks = ReportListReservistsWithAppointmentsBlockUtil.GetReportListReservistsWithAppointDetailsBlockList(EquipmentReservistsRequestId, CurrentUser);

            EquipmentReservistsRequest request = EquipmentReservistsRequestUtil.GetEquipmentReservistsRequest(EquipmentReservistsRequestId, CurrentUser);

            string requestNumberLabel = "Заявка №:";
            string requestDateLabel = "от дата:&nbsp;";
            string equipWithResRequestsStatusLabel = "Статус на заявката:";
            string militaryUnitLabel = "Заявката е от " + CommonFunctions.GetLabelText("MilitaryUnit") + ":";
            string administrationLabel = "От кое министерство/ведомство:";

            string requestNumber = request.RequestNumber;
            string requestDate = CommonFunctions.FormatDate(request.RequestDate);
            string equipWithResRequestsStatus = (request.EquipWithResRequestsStatus != null ? request.EquipWithResRequestsStatus.StatusName : "");
            string militaryUnit = (request.MilitaryUnit != null ? request.MilitaryUnit.DisplayTextForSelection : "");
            string administration = (request.Administration != null ? request.Administration.AdministrationName : "");

            if (isRequestNumberHidden)
            {
                requestNumber = "";
                requestNumberLabel = "";
            }

            if (isRequestDateHidden)
            {
                requestDate = "";
                requestDateLabel = "";
            }

            if (isRequestStatusHidden)
            {
                equipWithResRequestsStatus = "";
                equipWithResRequestsStatusLabel = "";
            }

            if (isMilitaryUnitHidden)
            {
                militaryUnit = "";
                militaryUnitLabel = "";
            }

            if (isAdministrationHidden)
            {
                administration = "";
                administrationLabel = "";
            }

            StringBuilder html = new StringBuilder();
            html.Append(@"<html xmlns='http://www.w3.org/1999/xhtml' xml:lang='en'>
                            <head>
                                <meta http-equiv='content-type' content='application/xhtml+xml; charset=UTF-8' />
                            </head>
                            <body>
                                <table>
                                    <tr><td align='center' colspan='" + visibleColumnsCount + @"' style='font-weight: bold; font-size: 1.6em;'>АСУ на човешките ресурси</td></tr>
                                    <tr><td align='center' colspan='" + visibleColumnsCount + @"' style='font-weight: bold; font-size: 2em;'>Резервисти</td></tr>
                                    <tr><td colspan='" + visibleColumnsCount + @"'>&nbsp;</td></tr>
                                    <tr><td align='center' colspan='" + visibleColumnsCount + @"' style='font-weight: bold; font-size: 1.3em;'>Списък на хората с МН по определена заявка</td></tr>
                                    <tr><td colspan='" + visibleColumnsCount + @"'>&nbsp;</td></tr>
                                    <tr>
                                        <td align='center' colspan='" + visibleColumnsCount + @"'>
                                            <span style='font-weight: normal;'>" + requestNumberLabel + @"</span>
                                            <span style='font-weight: bold;'>" + requestNumber + @"</span>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <span style='font-weight: normal;'>" + requestDateLabel + @"</span>
                                            <span style='font-weight: bold;'>" + requestDate + @"</span>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <span style='font-weight: normal;'>" + equipWithResRequestsStatusLabel + @"</span>
                                            <span style='font-weight: bold;'>" + equipWithResRequestsStatus + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='center' colspan='" + visibleColumnsCount + @"'>
                                            <span style='font-weight: normal;'>" + militaryUnitLabel + @"</span>
                                            <span style='font-weight: bold;'>" + militaryUnit + @"</span>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <span style='font-weight: normal;'>" + administrationLabel + @"</span>
                                            <span style='font-weight: bold;'>" + administration + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan='" + visibleColumnsCount + @"'>&nbsp;</td>
                                    </tr>
                                </table>");


            if (reportBlocks.Count() > 0)
            {
                html.Append(@"
                    <table>
                        <thead>
                            <tr>
                                <th style='width: 30px; border-bottom: 1px solid white; background-color: black; color: #FFFFFF;'>№</th>" +
         (!isMilCommandHidden? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Команда</th>" : "") +
          (!isPositionHidden ? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Длъжност</th>" : "") +
  (!isMilRepSpecialityHidden ? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>ВОС</th>" : "") +
           (!isMilRankHidden ? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Звание</th>" : "") +
         (!isReadinessHidden ? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Начин на явяване</th>" : "") +
    (!isIdentityNumberHidden ? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>ЕГН</th>" : "") +
          (!isFullNameHidden ? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Име</th>" : "") +
      (!isPermCityNameHidden ? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Населено място</th>" : "") +
                            @"</tr>
                        </thead><tbody>");

                int counter = 1;

                foreach (ReportListReservistsWithAppointDetailsBlock reportBlock in reportBlocks)
                {
                    html.Append(@"
                            <tr>
                                <td align='center' style='border: 1px solid black;'>" + counter + @"</td>" +
        (!isMilCommandHidden ? "<td align='left' style='border: 1px solid black;'>" + reportBlock.MilitaryCommand + @"</td>" : "") +
          (!isPositionHidden ? "<td align='left' style='border: 1px solid black;'>" + reportBlock.Position + @"</td>" : "") +
  (!isMilRepSpecialityHidden ? "<td align='left' style='border: 1px solid black;'>" + reportBlock.MilitaryReportingSpecialty + @"</td>" : "") +
           (!isMilRankHidden ? "<td align='left' style='border: 1px solid black;'>" + reportBlock.MilitaryRank + @"</td>" : "") +
         (!isReadinessHidden ? "<td align='left' style='border: 1px solid black;'>" + reportBlock.Readiness + @"</td>" : "") +
    (!isIdentityNumberHidden ? "<td align='left' style='border: 1px solid black;'>" + reportBlock.IdentityNumber + @"</td>" : "") +
          (!isFullNameHidden ? "<td align='left' style='border: 1px solid black;'>" + reportBlock.FullName + @"</td>" : "") +
      (!isPermCityNameHidden ? "<td align='left' style='border: 1px solid black;'>" + reportBlock.PermPlaceName + @"</td>" : "") + @"
                             </tr>");
                    counter++;
                }

                html.Append("</tbody></table>");
            }

            html.Append("</body></html>");

            return html.ToString();
        }
    }
}
