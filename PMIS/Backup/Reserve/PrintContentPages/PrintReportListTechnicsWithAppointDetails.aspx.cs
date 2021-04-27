using System;
using System.Linq;
using System.Text;

using PMIS.Common;
using PMIS.Reserve.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace PMIS.Reserve.PrintContentPages
{
    public partial class PrintReportListTechnicsWithAppointDetails : RESPage
    {
        //This property represents the ID of the EquipmentTechnicsRequest object that is loaded on the screen
        //If this is a new object then the ID is 0
        //It is stored in a hidden field on the page
        private int EquipmentTechnicsRequestId
        {
            get
            {
                int equipmentTechnicsRequestId = 0;
                if (String.IsNullOrEmpty(this.hfEquipmentTechnicsRequestID.Value)
                    || this.hfEquipmentTechnicsRequestID.Value == "0")
                {
                    if (Request.Params["EquipmentTechnicsRequestId"] != null)
                        int.TryParse(Request.Params["EquipmentTechnicsRequestId"].ToString(), out equipmentTechnicsRequestId);

                    this.hfEquipmentTechnicsRequestID.Value = equipmentTechnicsRequestId.ToString();
                }
                else
                {
                    Int32.TryParse(this.hfEquipmentTechnicsRequestID.Value, out equipmentTechnicsRequestId);
                }

                return equipmentTechnicsRequestId;
            }

            set
            {
                this.hfEquipmentTechnicsRequestID.Value = value.ToString();
            }
        }

        public void SetPrintTitleLinesWidth()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnPrintTitleLinesWidth");

            hf.Value = "935";
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
                this.GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS") != UIAccessLevel.Hidden)
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
            bool isMilCommandHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_DETAILS_MILCOMMAND") == UIAccessLevel.Hidden;
            bool isTechnicsTypeHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_DETAILS_TECHNICSTYPE") == UIAccessLevel.Hidden;
            bool isNormativeTechnicsHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_DETAILS_NORMATIVETECHNICS") == UIAccessLevel.Hidden;
            bool isReadinessHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_DETAILS_READINESS") == UIAccessLevel.Hidden;
            bool isRegInvNumberHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_DETAILS_REGINVNUMBER") == UIAccessLevel.Hidden;
            bool isOwnerHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_DETAILS_OWNER") == UIAccessLevel.Hidden;

            bool isRequestNumberHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_REQUEST_REQNUMBER") == UIAccessLevel.Hidden;
            bool isRequestDateHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_REQUEST_REQDATE") == UIAccessLevel.Hidden;
            bool isRequestStatusHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_REQUEST_REQSTATUS") == UIAccessLevel.Hidden;
            bool isMilitaryUnitHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_REQUEST_MILUNIT") == UIAccessLevel.Hidden;
            bool isAdministrationHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_REQUEST_ADMINISTRATION") == UIAccessLevel.Hidden;
            

            //Get the list of records according to the specified filters, order and paging
            List<ReportListTechnicsWithAppointDetailsBlock> reportBlocks = ReportListTechnicsWithAppointmentsBlockUtil.GetReportListTechnicsWithAppointDetailsBlockList(EquipmentTechnicsRequestId, CurrentUser);

            EquipmentTechnicsRequest request = EquipmentTechnicsRequestUtil.GetEquipmentTechnicsRequest(EquipmentTechnicsRequestId, CurrentUser);

            string requestNumberLabel = "Заявка №:";
            string requestDateLabel = "от дата:&nbsp;";
            string equipWithTechRequestsStatusLabel = "Статус на заявката:";
            string militaryUnitLabel = "Заявката е от " + CommonFunctions.GetLabelText("MilitaryUnit") + ":";
            string administrationLabel = "От кое министерство/ведомство:";

            string requestNumber = request.RequestNumber;
            string requestDate = CommonFunctions.FormatDate(request.RequestDate);
            string equipWithTechRequestsStatus = (request.EquipWithTechRequestsStatus != null ? request.EquipWithTechRequestsStatus.StatusName : "");
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
                equipWithTechRequestsStatus = "";
                equipWithTechRequestsStatusLabel = "";
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
                                <td align='right' style='width: 175px;'>
                                    <span class='Label'>" + requestNumberLabel + @"</span>
                                </td>
                                <td align='left' style='width: 195px;'>
                                   <span class='ValueLabel'>" + requestNumber + @"</span>&nbsp;&nbsp;
                                   <span class='Label'>" + requestDateLabel + @"</span>
                                   <span class='ValueLabel'>" + requestDate + @"</span>&nbsp;
                                </td>
                                <td align='right' style='width: 220px;'>
                                   <span class='Label'>" + equipWithTechRequestsStatusLabel + @"</span>
                                </td>
                                <td align='left' style='width: 325px;'>
                                   <span class='ValueLabel'>" + equipWithTechRequestsStatus + @"</span>
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
        (!isMilCommandHidden ? "<th style='width: 140px;'>Команда</th>" : "") +
      (!isTechnicsTypeHidden ? "<th style='width: 120px;'>Вид техника</th>" : "") +
 (!isNormativeTechnicsHidden ? "<th style='width: 140px;'>Нормативна категория</th>" : "") +
         (!isReadinessHidden ? "<th style='width: 110px;'>Начин на явяване</th>" : "") +
      (!isRegInvNumberHidden ? "<th style='width: 120px;'>Рег./Инв. номер</th>" : "") +
             (!isOwnerHidden ? "<th style='width: 240px; border-right: 1px solid #000000;'>Собственик</th>" : "") + @"
                            </tr>
                        </thead><tbody>");
            }

            int counter = 1;

            foreach (ReportListTechnicsWithAppointDetailsBlock reportBlock in reportBlocks)
            {
                html.Append(@"<tr>
                               <td align='center'>" + counter + @"</td>" +
       (!isMilCommandHidden ? "<td style='text-align: left;'>" + reportBlock.MilitaryCommand + @"</td>" : "") +
     (!isTechnicsTypeHidden ? "<td style='text-align: left;'>" + reportBlock.TechnicsType + @"</td>" : "") +
(!isNormativeTechnicsHidden ? "<td style='text-align: left;'>" + reportBlock.NormativeTechnics + @"</td>" : "") +
        (!isReadinessHidden ? "<td style='text-align: left;'>" + reportBlock.Readiness + @"</td>" : "") +
     (!isRegInvNumberHidden ? "<td style='text-align: left;'>" + reportBlock.RegInvNumber + @"</td>" : "") +
            (!isOwnerHidden ? "<td style='text-align: left;'>" + reportBlock.Owner + @"</td>" : "") + @"
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
            Response.AppendHeader("content-disposition", "attachment; filename=PrintReportListTechnicsWithAppointDetails.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        // Generate html as excel response result
        private string GenerateAllRecordsForExport()
        {
            bool isMilCommandHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_DETAILS_MILCOMMAND") == UIAccessLevel.Hidden;
            bool isTechnicsTypeHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_DETAILS_TECHNICSTYPE") == UIAccessLevel.Hidden;
            bool isNormativeTechnicsHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_DETAILS_NORMATIVETECHNICS") == UIAccessLevel.Hidden;
            bool isReadinessHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_DETAILS_READINESS") == UIAccessLevel.Hidden;
            bool isRegInvNumberHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_DETAILS_REGINVNUMBER") == UIAccessLevel.Hidden;
            bool isOwnerHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_DETAILS_OWNER") == UIAccessLevel.Hidden;

            bool isRequestNumberHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_REQUEST_REQNUMBER") == UIAccessLevel.Hidden;
            bool isRequestDateHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_REQUEST_REQDATE") == UIAccessLevel.Hidden;
            bool isRequestStatusHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_REQUEST_REQSTATUS") == UIAccessLevel.Hidden;
            bool isMilitaryUnitHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_REQUEST_MILUNIT") == UIAccessLevel.Hidden;
            bool isAdministrationHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_REQUEST_ADMINISTRATION") == UIAccessLevel.Hidden;
            
            int visibleColumnsCount = 1;
            visibleColumnsCount += isMilCommandHidden ? 0 : 1;
            visibleColumnsCount += isTechnicsTypeHidden ? 0 : 1;
            visibleColumnsCount += isNormativeTechnicsHidden ? 0 : 1;
            visibleColumnsCount += isReadinessHidden ? 0 : 1;
            visibleColumnsCount += isRegInvNumberHidden ? 0 : 1;
            visibleColumnsCount += isOwnerHidden ? 0 : 1;

            //Get the list of records according to the specified filters, order and paging
            List<ReportListTechnicsWithAppointDetailsBlock> reportBlocks = ReportListTechnicsWithAppointmentsBlockUtil.GetReportListTechnicsWithAppointDetailsBlockList(EquipmentTechnicsRequestId, CurrentUser);

            EquipmentTechnicsRequest request = EquipmentTechnicsRequestUtil.GetEquipmentTechnicsRequest(EquipmentTechnicsRequestId, CurrentUser);

            string requestNumberLabel = "Заявка №:";
            string requestDateLabel = "от дата:&nbsp;";
            string equipWithTechRequestsStatusLabel = "Статус на заявката:";
            string militaryUnitLabel = "Заявката е от " + CommonFunctions.GetLabelText("MilitaryUnit") + ":";
            string administrationLabel = "От кое министерство/ведомство:";

            string requestNumber = request.RequestNumber;
            string requestDate = CommonFunctions.FormatDate(request.RequestDate);
            string equipWithTechRequestsStatus = (request.EquipWithTechRequestsStatus != null ? request.EquipWithTechRequestsStatus.StatusName : "");
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
                equipWithTechRequestsStatus = "";
                equipWithTechRequestsStatusLabel = "";
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
                                            <span style='font-weight: normal;'>" + equipWithTechRequestsStatusLabel + @"</span>
                                            <span style='font-weight: bold;'>" + equipWithTechRequestsStatus + @"</span>
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
        (!isMilCommandHidden ? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Команда</th>" : "") +
      (!isTechnicsTypeHidden ? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Вид техника</th>" : "") +
 (!isNormativeTechnicsHidden ? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Нормативна категория</th>" : "") +
         (!isReadinessHidden ? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Начин на явяване</th>" : "") +
      (!isRegInvNumberHidden ? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Рег./Инв. номер</th>" : "") +
             (!isOwnerHidden ? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Собственик</th>" : "") +
                          @"</tr>
                        </thead><tbody>");

                int counter = 1;

                foreach (ReportListTechnicsWithAppointDetailsBlock reportBlock in reportBlocks)
                {
                    html.Append(@"
                            <tr>
                                <td align='center' style='border: 1px solid black;'>" + counter + @"</td>" +
        (!isMilCommandHidden ? "<td align='left' style='border: 1px solid black;'>" + reportBlock.MilitaryCommand + @"</td>" : "") +
      (!isTechnicsTypeHidden ? "<td align='left' style='border: 1px solid black;'>" + reportBlock.TechnicsType + @"</td>" : "") +
 (!isNormativeTechnicsHidden ? "<td align='left' style='border: 1px solid black;'>" + reportBlock.NormativeTechnics + @"</td>" : "") +
         (!isReadinessHidden ? "<td align='left' style='border: 1px solid black;'>" + reportBlock.Readiness + @"</td>" : "") +
      (!isRegInvNumberHidden ? "<td align='left' style='border: 1px solid black;'>" + reportBlock.RegInvNumber + @"</td>" : "") +
             (!isOwnerHidden ? "<td align='left' style='border: 1px solid black;'>" + reportBlock.Owner + @"</td>" : "") + @"
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
