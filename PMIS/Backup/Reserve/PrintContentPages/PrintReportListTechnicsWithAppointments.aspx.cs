using System;
using System.Linq;
using System.Text;

using PMIS.Common;
using PMIS.Reserve.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace PMIS.Reserve.PrintContentPages
{
    public partial class PrintReportListTechnicsWithAppointments : RESPage
    {
        const string All = "Всички";

        string requestNumber = "";
        string requestDateFrom = "";
        string requestDateTo = "";
        string militaryUnitId = "";
        string administrationId = "";
        string equipWithTechRequestsStatusId = "";
        string militaryDepartmentId = "";

        int sortBy = 1; // 1 - Default

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
            if (GetUIItemAccessLevel("RES_REPORTS") == UIAccessLevel.Hidden ||
                this.GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS") != UIAccessLevel.Hidden)
            {
                if (!String.IsNullOrEmpty(Request.Params["RequestNumber"]))
                {
                    requestNumber = Request.Params["RequestNumber"];
                }

                if (!String.IsNullOrEmpty(Request.Params["RequestDateFrom"]))
                {
                    requestDateFrom = Request.Params["RequestDateFrom"];
                }

                if (!String.IsNullOrEmpty(Request.Params["RequestDateTo"]))
                {
                    requestDateTo = Request.Params["RequestDateTo"];
                }

                if (!String.IsNullOrEmpty(Request.Params["MilitaryUnitId"]))
                {
                    militaryUnitId = Request.Params["MilitaryUnitId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["AdministrationId"]))
                {
                    administrationId = Request.Params["AdministrationId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["EquipWithTechRequestsStatusId"]))
                {
                    equipWithTechRequestsStatusId = Request.Params["EquipWithTechRequestsStatusId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["MilitaryDepartmentId"]))
                {
                    militaryDepartmentId = Request.Params["MilitaryDepartmentId"];
                }

                if (!String.IsNullOrEmpty(Request.Params["SortBy"]))
                {
                    int.TryParse(Request.Params["SortBy"], out sortBy);
                }

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
                sb.Append("<td style=\"vertical-align: top;\">" + CommonFunctions.GenerateButtons(true, 180, true) + "</td>");
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
            bool isReqNumberHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_REQUEST_REQNUMBER") == UIAccessLevel.Hidden;
            bool isReqDateHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_REQUEST_REQDATE") == UIAccessLevel.Hidden;
            bool isMilUnitHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_REQUEST_MILUNIT") == UIAccessLevel.Hidden;
            bool isAdministrationHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_REQUEST_ADMINISTRATION") == UIAccessLevel.Hidden;
            bool isReqStatusHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_REQUEST_REQSTATUS") == UIAccessLevel.Hidden;

            ReportListTechnicsWithAppointmentsFilter filter = new ReportListTechnicsWithAppointmentsFilter()
            {
                RequestNum = requestNumber,
                RequestDateFrom = (CommonFunctions.TryParseDate(requestDateFrom) ? CommonFunctions.ParseDate(requestDateFrom) : (DateTime?)null),
                RequestDateTo = (CommonFunctions.TryParseDate(requestDateTo) ? CommonFunctions.ParseDate(requestDateTo) : (DateTime?)null),
                MilitaryUnits = militaryUnitId,
                Administrations = administrationId,
                EquipWithTechRequestsStatuses = equipWithTechRequestsStatusId,
                MilitaryDepartments = militaryDepartmentId,
                OrderBy = sortBy,
                PageIdx = 0
            };

            //Get the list of records according to the specified filters and order
            List<ReportListTechnicsWithAppointmentsBlock> reportListTechnicsWithAppointmentsBlocks = ReportListTechnicsWithAppointmentsBlockUtil.GetReportListTechnicsWithAppointmentsBlockList(filter, 0, CurrentUser);

            MilitaryUnit militaryUnit = null;
            int milUnitId = 0;
            if (int.TryParse(militaryUnitId, out milUnitId))
            {
                militaryUnit = MilitaryUnitUtil.GetMilitaryUnit(milUnitId, CurrentUser);   
            }

            Administration administration = null;
            int administrId = 0;
            if (int.TryParse(administrationId, out administrId))
            {
                administration = AdministrationUtil.GetAdministration(administrId, CurrentUser);
            }

            EquipWithTechRequestsStatus equipWithTechRequestsStatus = null;
            int equipWithTechReqStId = 0;
            if (int.TryParse(equipWithTechRequestsStatusId, out equipWithTechReqStId))
            {
                equipWithTechRequestsStatus = EquipWithTechRequestsStatusUtil.GetEquipWithTechRequestsStatus(equipWithTechReqStId, CurrentUser);
            }

            MilitaryDepartment militaryDepartment = null;
            int militDepId = 0;
            if (int.TryParse(militaryDepartmentId, out militDepId))
            {
                militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment(militDepId, CurrentUser);
            }

            StringBuilder html = new StringBuilder();
            html.Append(@"<table style='padding: 5px;'>
                             <tr>
                                <td align='right' style='width: 135px;'>
                                    <span class='Label'>Заявка №:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 185px;'>
                                   <span class='ValueLabel'>" + requestNumber + @"</span>&nbsp;&nbsp;
                                </td>
                                <td align='right' style='width: 215px;'>
                                    <span class='Label'>В периода от дата:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 215px;'>
                                    <span class='ValueLabel'>" + requestDateFrom + @"</span>&nbsp;
                                    <span class='Label'>до дата:&nbsp;</span>
                                    <span class='ValueLabel'>" + requestDateTo + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' >
                                    <span class='Label'>Заявката е от " + CommonFunctions.GetLabelText("MilitaryUnit") + @":&nbsp;</span>
                                </td>
                                <td align='left'>
                                    <span class='ValueLabel'>" + (militaryUnit != null ? militaryUnit.DisplayTextForSelection : All) + @"</span>
                                </td>
                                <td align='right' >
                                    <span class='Label'>От кое министерство/ведомство:&nbsp;</span>
                                </td>
                                <td align='left' >
                                    <span class='ValueLabel'>" + (administration != null ? administration.AdministrationName : All) + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' >
                                    <span class='Label'>Статус на заявката:&nbsp;</span>
                                </td>
                                <td align='left' >
                                    <span class='ValueLabel'>" + (equipWithTechRequestsStatus != null ? equipWithTechRequestsStatus.StatusName : All) + @"</span>
                                </td>
                                <td align='right' >
                                    <span class='Label'>Заявката се изпълнява от ВО:&nbsp;</span>
                                </td>
                                <td align='left'>
                                    <span class='ValueLabel'>" + (militaryDepartment != null ? militaryDepartment.MilitaryDepartmentName : All) + @"</span>
                                </td>
                             </tr>");

            if (reportListTechnicsWithAppointmentsBlocks.Count() > 0)
            {
                html.Append(@"
                    <tr><td colspan='4' align='center'>
                    <table id='applicantsTable' name='applicantsTable' class='CommonHeaderTable'>
                        <thead>
                            <tr>
                                <th style='width: 15px; border-left: 1px solid #000000;'>№</th>" +
            (!isReqNumberHidden ? "<th style='width: 100px;'>Заявка №</th>" : "") +
            (!isReqDateHidden ? "<th style='width: 80px;'>От дата</th>" : "") +
            (!isMilUnitHidden ? "<th style='width: 150px;'>" + CommonFunctions.GetLabelText("MilitaryUnit") + "</th>" : "") +
            (!isAdministrationHidden ? "<th style='width: 170px;'>Министерство/Ведомство</th>" : "") +
            (!isReqStatusHidden ? "<th style='width: 170px; border-right: 1px solid #000000;'>Статус на заявката</th>" : "") + @"
                            </tr>
                        </thead><tbody>");
            }

            int counter = 1;

            foreach (ReportListTechnicsWithAppointmentsBlock reportListTechnicsWithAppointmentsBlock in reportListTechnicsWithAppointmentsBlocks)
            {
                html.Append(@"<tr>
                            <td align='center'>" + counter + @"</td>" +
            (!isReqNumberHidden ? "<td align='left'>" + reportListTechnicsWithAppointmentsBlock.RequestNumber + @"</td>" : "") +
            (!isReqDateHidden ? "<td align='left'>" + reportListTechnicsWithAppointmentsBlock.RequestDate + @"</td>" : "") +
            (!isMilUnitHidden ? "<td align='left'>" + reportListTechnicsWithAppointmentsBlock.MilitaryUnit + @"</td>" : "") +
            (!isAdministrationHidden ? "<td align='left'>" + reportListTechnicsWithAppointmentsBlock.Administration + @"</td>" : "") +
            (!isReqStatusHidden ? "<td align='left'>" + reportListTechnicsWithAppointmentsBlock.RequestStatus + @"</td>" : "") + @"
                          </tr>");
                counter++;
            }

            if (reportListTechnicsWithAppointmentsBlocks.Count() > 0)
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
            Response.AppendHeader("content-disposition", "attachment; filename=ReportListTechnicsWithAppointments.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        // Generate html as excel response result
        private string GenerateAllRecordsForExport()
        {
            bool isReqNumberHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_REQUEST_REQNUMBER") == UIAccessLevel.Hidden;
            bool isReqDateHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_REQUEST_REQDATE") == UIAccessLevel.Hidden;
            bool isMilUnitHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_REQUEST_MILUNIT") == UIAccessLevel.Hidden;
            bool isAdministrationHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_REQUEST_ADMINISTRATION") == UIAccessLevel.Hidden;
            bool isReqStatusHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_REQUEST_REQSTATUS") == UIAccessLevel.Hidden;

            int visibleColumnsCount = 1;
            if (!isReqNumberHidden)
                visibleColumnsCount++;
            if (!isReqDateHidden)
                visibleColumnsCount++;
            if (!isMilUnitHidden)
                visibleColumnsCount++;
            if (!isAdministrationHidden)
                visibleColumnsCount++;
            if (!isReqStatusHidden)
                visibleColumnsCount++;

            ReportListTechnicsWithAppointmentsFilter filter = new ReportListTechnicsWithAppointmentsFilter()
            {
                RequestNum = requestNumber,
                RequestDateFrom = (CommonFunctions.TryParseDate(requestDateFrom) ? CommonFunctions.ParseDate(requestDateFrom) : (DateTime?)null),
                RequestDateTo = (CommonFunctions.TryParseDate(requestDateTo) ? CommonFunctions.ParseDate(requestDateTo) : (DateTime?)null),
                MilitaryUnits = militaryUnitId,
                Administrations = administrationId,
                EquipWithTechRequestsStatuses = equipWithTechRequestsStatusId,
                MilitaryDepartments = militaryDepartmentId,
                OrderBy = sortBy,
                PageIdx = 0
            };

            //Get the list of records according to the specified filters and order
            List<ReportListTechnicsWithAppointmentsBlock> reportListTechnicsWithAppointmentsBlocks = ReportListTechnicsWithAppointmentsBlockUtil.GetReportListTechnicsWithAppointmentsBlockList(filter, 0, CurrentUser);

            MilitaryUnit militaryUnit = null;
            int milUnitId = 0;
            if (int.TryParse(militaryUnitId, out milUnitId))
            {
                militaryUnit = MilitaryUnitUtil.GetMilitaryUnit(milUnitId, CurrentUser);
            }

            Administration administration = null;
            int administrId = 0;
            if (int.TryParse(administrationId, out administrId))
            {
                administration = AdministrationUtil.GetAdministration(administrId, CurrentUser);
            }

            EquipWithTechRequestsStatus equipWithTechRequestsStatus = null;
            int equipWithTechReqStId = 0;
            if (int.TryParse(equipWithTechRequestsStatusId, out equipWithTechReqStId))
            {
                equipWithTechRequestsStatus = EquipWithTechRequestsStatusUtil.GetEquipWithTechRequestsStatus(equipWithTechReqStId, CurrentUser);
            }

            MilitaryDepartment militaryDepartment = null;
            int militDepId = 0;
            if (int.TryParse(militaryDepartmentId, out militDepId))
            {
                militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment(militDepId, CurrentUser);
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
                                    <tr><td align='center' colspan='" + visibleColumnsCount + @"' style='font-weight: bold; font-size: 1.3em;'>Списък на хората с МН по определена заявка - избор на заявка</td></tr>
                                    <tr><td colspan='" + visibleColumnsCount + @"'>&nbsp;</td></tr>
                                    <tr>
                                        <td align='center' colspan='" + visibleColumnsCount + @"'>
                                            <span style='font-weight: normal;'>Заявка №:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + requestNumber + @"</span>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <span style='font-weight: normal;'>В периода от дата:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + CommonFunctions.FormatDate(requestDateFrom) + @"</span>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <span style='font-weight: normal;'>до дата:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + CommonFunctions.FormatDate(requestDateTo) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='center' colspan='" + visibleColumnsCount + @"'>
                                            <span style='font-weight: normal;'>Заявката е от " + CommonFunctions.GetLabelText("MilitaryUnit") + @":&nbsp;</span>
                                            <span style='font-weight: bold;'>" + (militaryUnit != null ? militaryUnit.DisplayTextForSelection : All) + @"</span>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <span style='font-weight: normal;'>От кое министерство/ведомство:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + (administration != null ? administration.AdministrationName : All) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='center' colspan='" + visibleColumnsCount + @"'>
                                            <span style='font-weight: normal;'>Статус на заявката:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + (equipWithTechRequestsStatus != null ? equipWithTechRequestsStatus.StatusName : All) + @"</span>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <span style='font-weight: normal;'>Заявката се изпълнява от ВО:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + (militaryDepartment != null ? militaryDepartment.MilitaryDepartmentName : All) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan='" + visibleColumnsCount + @"'>&nbsp;</td>
                                    </tr>
                                </table>");


            if (reportListTechnicsWithAppointmentsBlocks.Count() > 0)
            {
                html.Append(@"
                    <table>
                        <thead>
                            <tr>
                                <th style='width: 30px; border-bottom: 1px solid white; background-color: black; color: #FFFFFF;'>№</th>" +
                                (!isReqNumberHidden ? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Заявка №</th>" : "") +
                                (!isReqDateHidden ? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>От дата</th>" : "") +
                                (!isMilUnitHidden ? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>" + CommonFunctions.GetLabelText("MilitaryUnit") + "</th>" : "") +
                                (!isAdministrationHidden ? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Министерство/Ведомство</th>" : "") +
                                (!isReqStatusHidden ? "<th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Статус на заявката</th>" : "") +
                            @"</tr>
                        </thead><tbody>");

                int counter = 1;

                foreach (ReportListTechnicsWithAppointmentsBlock reportListTechnicsWithAppointmentsBlock in reportListTechnicsWithAppointmentsBlocks)
                {
                    html.Append(@"
                            <tr>
                                <td align='center' style='border: 1px solid black;'>" + counter + @"</td>" +
                                (!isReqNumberHidden ? ("<td align='left' style='border: 1px solid black;'>" + reportListTechnicsWithAppointmentsBlock.RequestNumber + @"</td>") : "") +
                                (!isReqDateHidden ? ("<td align='left' style='border: 1px solid black;'>" + reportListTechnicsWithAppointmentsBlock.RequestDate + @"</td>") : "") +
                                (!isMilUnitHidden ? ("<td align='left' style='border: 1px solid black;'>" + reportListTechnicsWithAppointmentsBlock.MilitaryUnit + @"</td>") : "") +
                                (!isAdministrationHidden ? ("<td align='left' style='border: 1px solid black;'>" + reportListTechnicsWithAppointmentsBlock.Administration + @"</td>") : "") +
                                (!isReqStatusHidden ? ("<td align='left' style='border: 1px solid black;'>" + reportListTechnicsWithAppointmentsBlock.RequestStatus + @"</td>") : "") + @"
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
