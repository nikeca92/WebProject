using System;
using System.Linq;
using System.Text;

using PMIS.Common;
using PMIS.Reserve.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace PMIS.Reserve.PrintContentPages
{
    public partial class PrintAllEquipmentTechnicsRequests : RESPage
    {
        const string All = "Всички";

        string requestNumber = "";
        DateTime? requestDateFrom = null;
        DateTime? requestDateTo = null;
        string commandNum = "";
        string militaryUnitId = "";
        string administrationId = "";
        string equipWithTechRequestsStatusId = "";
        string militaryDepartmentId = "";

        int sortBy = 1; // 1 - Default

        public void SetPrintTitleLinesWidth()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnPrintTitleLinesWidth");

            hf.Value = "785";
        }

        public void SetHeadersLeft()
        {
            HiddenField hf = (HiddenField)Master.FindControl("hdnHeadersLeft");

            hf.Value = "80";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.SetPrintTitleLinesWidth();
                this.SetHeadersLeft();
            }

            // Check visibility right for the print screen
            if (this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS") != UIAccessLevel.Hidden)
            {
                if (!String.IsNullOrEmpty(Request.Params["RequestNumber"]))
                {
                    requestNumber = Request.Params["RequestNumber"];
                }

                if (!String.IsNullOrEmpty(Request.Params["RequestDateFrom"])
                    && CommonFunctions.TryParseDate(Request.Params["RequestDateFrom"]))
                {
                    requestDateFrom = CommonFunctions.ParseDate(Request.Params["RequestDateFrom"]);
                }

                if (!String.IsNullOrEmpty(Request.Params["RequestDateTo"])
                    && CommonFunctions.TryParseDate(Request.Params["RequestDateTo"]))
                {
                    requestDateTo = CommonFunctions.ParseDate(Request.Params["RequestDateTo"]);
                }

                if (!String.IsNullOrEmpty(Request.Params["CommandNum"]))
                {
                    commandNum = Request.Params["CommandNum"];
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

                this.divResults.InnerHtml = GeneratePageContent(false);
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
                sb.Append("<td style=\"vertical-align: top;\">" + CommonFunctions.GenerateButtons(true, 160, true) + "</td>");
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
            EquipmentTechnicsRequestsFilter filter = new EquipmentTechnicsRequestsFilter()
            {
                RequestNum = requestNumber,
                RequestDateFrom = requestDateFrom,
                RequestDateTo = requestDateTo,
                CommandNum = commandNum,
                MilitaryUnits = militaryUnitId,
                Administrations = administrationId,
                EquipWithTechRequestsStatuses = equipWithTechRequestsStatusId,
                MilitaryDepartments = militaryDepartmentId,
                OrderBy = sortBy,
                PageIdx = 0
            };

            //Get the list of records according to the specified filters and order
            List<EquipmentTechnicsRequest> equipmentTechnicsRequests = EquipmentTechnicsRequestUtil.GetAllEquipmentTechnicsRequest(filter, 0, CurrentUser);

            MilitaryUnit militaryUnit = null;
            if (!String.IsNullOrEmpty(militaryUnitId))
            {
                militaryUnit = MilitaryUnitUtil.GetMilitaryUnit(int.Parse(militaryUnitId), CurrentUser);
            }

            Administration administration = null;
            if (!String.IsNullOrEmpty(administrationId))
            {
                administration = AdministrationUtil.GetAdministration(int.Parse(administrationId), CurrentUser);
            }

            EquipWithResRequestsStatus equipWithResRequestsStatus = null;
            if (!String.IsNullOrEmpty(equipWithTechRequestsStatusId))
            {
                equipWithResRequestsStatus = EquipWithResRequestsStatusUtil.GetEquipWithResRequestsStatus(int.Parse(equipWithTechRequestsStatusId), CurrentUser);
            }

            MilitaryDepartment militaryDepartment = null;
            if (!String.IsNullOrEmpty(militaryDepartmentId))
            {
                militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment(int.Parse(militaryDepartmentId), CurrentUser);
            }

            string html = @"<table style='padding: 5px;'>
                             <tr>
                                <td align='right' style='width: 140px;'>
                                    <span class='Label'>Заявка №:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 165px;'>
                                   <span class='ValueLabel'>" + requestNumber + @"</span>
                                </td>
                                <td align='right' style='width: 230px;'>
                                    <span class='Label'>В периода от дата:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 215px;'>
                                    <span class='ValueLabel'>" + CommonFunctions.FormatDate(requestDateFrom) + @"</span>
                                    <span class='Label'>до дата:&nbsp;</span>
                                    <span class='ValueLabel'>" + CommonFunctions.FormatDate(requestDateTo) + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' style='width: 140px;'>
                                    <span class='Label'>Команда №:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 165px;'>
                                   <span class='ValueLabel'>" + commandNum + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' style='width: 140px;'>
                                    <span class='Label'>Заявката е от " + CommonFunctions.GetLabelText("MilitaryUnit") + @":&nbsp;</span>
                                </td>
                                <td align='left' style='width: 165px;'>
                                   <span class='ValueLabel'>" + (militaryUnit != null ? militaryUnit.DisplayTextForSelection : All) + @"</span>
                                </td>
                                <td align='right' style='width: 230px;'>
                                    <span class='Label'>От кое министерство/ведомство:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 215px;'>
                                    <span class='ValueLabel'>" + (administration != null ? administration.AdministrationName : All) + @"</span>
                                </td>
                             </tr>
                             <tr>
                                <td align='right' style='width: 140px;'>
                                    <span class='Label'>Статус на заявката:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 165px;'>
                                   <span class='ValueLabel'>" + (equipWithResRequestsStatus != null ? equipWithResRequestsStatus.StatusName : All) + @"</span>
                                </td>
                                <td align='right' style='width: 230px;'>
                                    <span class='Label'>Заявката се изпълнява от ВО:&nbsp;</span>
                                </td>
                                <td align='left' style='width: 215px;'>
                                    <span class='ValueLabel'>" + (militaryDepartment != null ? militaryDepartment.MilitaryDepartmentName : All) + @"</span>
                                </td>
                             </tr>";

            if (equipmentTechnicsRequests.Count() > 0)
            {
                html += @"
                    <tr><td colspan='4' align='center'>
                    <table id='applicantsTable' name='applicantsTable' class='CommonHeaderTable'>
                        <thead>
                            <tr>
                                <th style='width: 30px; border-left: 1px solid #000000;'>№</th>
                                <th style='width: 150px;'>Заявка №</th>
                                <th style='width: 90px;'>От дата</th>
                                <th style='width: 150px;'>" + CommonFunctions.GetLabelText("MilitaryUnit") + @"</th>
                                <th style='width: 120px;'>Министерство/Ведомство</th>
                                <th style='width: 150px; border-right: 1px solid #000000;'>Статус на заявката</th>
                            </tr>
                        </thead><tbody>";
            }

            int counter = 1;

            foreach (EquipmentTechnicsRequest equipmentTechnicsRequest in equipmentTechnicsRequests)
            {
                html += @"<tr>
                            <td align='center'>" + counter + @"</td>
                            <td align='left'>" + equipmentTechnicsRequest.RequestNumber + @"</td>
                            <td align='left'>" + CommonFunctions.FormatDate(equipmentTechnicsRequest.RequestDate) + @"</td>
                            <td align='left'>" + (equipmentTechnicsRequest.MilitaryUnit == null ? "" : equipmentTechnicsRequest.MilitaryUnit.DisplayTextForSelection) + @"</td>
                            <td align='left'>" + (equipmentTechnicsRequest.Administration == null ? "" : equipmentTechnicsRequest.Administration.AdministrationName) + @"</td>
                            <td align='left'>" + (equipmentTechnicsRequest.EquipWithTechRequestsStatus == null ? "" : equipmentTechnicsRequest.EquipWithTechRequestsStatus.StatusName) + @"</td>
                          </tr>";
                counter++;
            }

            if (equipmentTechnicsRequests.Count() > 0)
            {
                html += "</tbody></table></td></tr>";
            }

            html += "</table>";

            return html;
        }

        // Handles export to excel button click
        protected void btnGenerateExcel_Click(object sender, EventArgs e)
        {
            string result = this.GeneratePageContent(true);
            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment; filename=EquipmentTechRequests.xls");
            Response.ContentType = "application/vnd.ms-excel";
            Response.Write(result);
            Response.End();
        }

        // Generate html as excel response result
        private string GenerateAllRecordsForExport()
        {
            EquipmentTechnicsRequestsFilter filter = new EquipmentTechnicsRequestsFilter()
            {
                RequestNum = requestNumber,
                RequestDateFrom = requestDateFrom,
                RequestDateTo = requestDateTo,
                CommandNum = commandNum,
                MilitaryUnits = militaryUnitId,
                Administrations = administrationId,
                EquipWithTechRequestsStatuses = equipWithTechRequestsStatusId,
                MilitaryDepartments = militaryDepartmentId,
                OrderBy = sortBy,
                PageIdx = 0
            };

            //Get the list of records according to the specified filters and order
            List<EquipmentTechnicsRequest> equipmentTechnicsRequests = EquipmentTechnicsRequestUtil.GetAllEquipmentTechnicsRequest(filter, 0, CurrentUser);

            MilitaryUnit militaryUnit = null;
            if (!String.IsNullOrEmpty(militaryUnitId))
            {
                militaryUnit = MilitaryUnitUtil.GetMilitaryUnit(int.Parse(militaryUnitId), CurrentUser);
            }

            Administration administration = null;
            if (!String.IsNullOrEmpty(administrationId))
            {
                administration = AdministrationUtil.GetAdministration(int.Parse(administrationId), CurrentUser);
            }

            EquipWithResRequestsStatus equipWithResRequestsStatus = null;
            if (!String.IsNullOrEmpty(equipWithTechRequestsStatusId))
            {
                equipWithResRequestsStatus = EquipWithResRequestsStatusUtil.GetEquipWithResRequestsStatus(int.Parse(equipWithTechRequestsStatusId), CurrentUser);
            }

            MilitaryDepartment militaryDepartment = null;
            if (!String.IsNullOrEmpty(militaryDepartmentId))
            {
                militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment(int.Parse(militaryDepartmentId), CurrentUser);
            }

            string html = @"<html xmlns='http://www.w3.org/1999/xhtml' xml:lang='en'>
                            <head>
                                <meta http-equiv='content-type' content='application/xhtml+xml; charset=UTF-8' />
                            </head>
                            <body>
                                <table>
                                    <tr><td align='center' colspan='6' style='font-weight: bold; font-size: 1.6em;'>АСУ на човешките ресурси</td></tr>
                                    <tr><td align='center' colspan='6' style='font-weight: bold; font-size: 2em;'>Резервисти</td></tr>
                                    <tr><td colspan='6'>&nbsp;</td></tr>
                                    <tr><td align='center' colspan='6' style='font-weight: bold; font-size: 1.3em;'>Списък на въведени заявки за окомплектоване с техника от резерва</td></tr>
                                    <tr><td colspan='6'>&nbsp;</td></tr>
                                    <tr>
                                        <td align='right' colspan='3'>
                                            <span style='font-weight: normal;'>Заявка №:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='3'>
                                            <span style='font-weight: bold;'>" + requestNumber + @"</span>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <span style='font-weight: normal;'>В периода от дата:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + CommonFunctions.FormatDate(requestDateFrom) + @"</span>
                                            &nbsp;&nbsp;
                                            <span style='font-weight: normal;'>до дата:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + CommonFunctions.FormatDate(requestDateTo) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='3'>
                                            <span style='font-weight: normal;'>Команда №:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='3'>
                                            <span style='font-weight: bold;'>" + commandNum + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='3'>
                                            <span style='font-weight: normal;'>Заявката е от " + CommonFunctions.GetLabelText("MilitaryUnit") + @":&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='3'>
                                            <span style='font-weight: bold;'>" + (militaryUnit != null ? militaryUnit.DisplayTextForSelection : All) + @"</span>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <span style='font-weight: normal;'>От кое министерство/ведомство:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + (administration != null ? administration.AdministrationName : All) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='right' colspan='3'>
                                            <span style='font-weight: normal;'>Заявка №:&nbsp;</span>
                                        </td>
                                        <td align='left' colspan='3'>
                                            <span style='font-weight: bold;'>" + (equipWithResRequestsStatus != null ? equipWithResRequestsStatus.StatusName : All) + @"</span>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <span style='font-weight: normal;'>В периода от дата:&nbsp;</span>
                                            <span style='font-weight: bold;'>" + (militaryDepartment != null ? militaryDepartment.MilitaryDepartmentName : All) + @"</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style='width: 30px;'>&nbsp;</td>
                                        <td style='width: 150px;'>&nbsp;</td>
                                        <td style='width: 100px;'>&nbsp;</td>
                                        <td style='width: 150px;'>&nbsp;</td>
                                        <td style='width: 150px;'>&nbsp;</td>
                                        <td style='width: 150px;'>&nbsp;</td>
                                    </tr>
                                </table>";


            if (equipmentTechnicsRequests.Count() > 0)
            {
                html += @"
                    <table>
                        <thead>
                            <tr>
                                <th style='width: 30px; border-bottom: 1px solid white; background-color: black; color: #FFFFFF;'>№</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Заявка №</th>
                                <th style='width: 100px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>От дата</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>" + CommonFunctions.GetLabelText("MilitaryUnit") + @"</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Министерство/Ведомство</th>
                                <th style='width: 150px; border-bottom: 1px solid white; border-left: 1px solid white; background-color: black; color: #FFFFFF;'>Статус на заявката</th>
                            </tr>
                        </thead><tbody>";

                int counter = 1;

                foreach (EquipmentTechnicsRequest equipmentTechnicsRequest in equipmentTechnicsRequests)
                {
                    html += @"
                            <tr>
                                <td align='center' style='border: 1px solid black;'>" + counter + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + equipmentTechnicsRequest.RequestNumber + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + CommonFunctions.FormatDate(equipmentTechnicsRequest.RequestDate) + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + (equipmentTechnicsRequest.MilitaryUnit == null ? "" : equipmentTechnicsRequest.MilitaryUnit.DisplayTextForSelection) + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + (equipmentTechnicsRequest.Administration == null ? "" : equipmentTechnicsRequest.Administration.AdministrationName) + @"</td>
                                <td align='left' style='border: 1px solid black;'>" + (equipmentTechnicsRequest.EquipWithTechRequestsStatus == null ? "" : equipmentTechnicsRequest.EquipWithTechRequestsStatus.StatusName) + @"</td>
                              </tr>";
                    counter++;
                }

                html += "</tbody></table>";
            }

            html += "</body></html>";

            return html;
        }
    }
}
