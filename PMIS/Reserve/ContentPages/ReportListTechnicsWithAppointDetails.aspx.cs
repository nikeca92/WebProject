using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class ReportListTechnicsWithAppointDetails : RESPage
    {
        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "RES_REPORTS_LISTTECHWITHAPPOINTMENTS";
            }
        }

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

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if (GetUIItemAccessLevel("RES_REPORTS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            lblMilitaryUnit.Text = "Заявката е от " + CommonFunctions.GetLabelText("MilitaryUnit") + ":";

            LoadHeaderInfo();
            LoadReportDeatils();

            //Hilight the current page in the menu bar
            HighlightMenuItems("Reports", "ReportListTechnicsWithAppointments");

            SetupPageUI();

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);
        }

        //Navigate to the previous page
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ContentPages/ReportListTechnicsWithAppointments.aspx");
        }

        private void LoadHeaderInfo()
        {
            EquipmentTechnicsRequest request = EquipmentTechnicsRequestUtil.GetEquipmentTechnicsRequest(EquipmentTechnicsRequestId, CurrentUser);

            lblRequestNumberValue.Text = request.RequestNumber;
            lblRequestDateValue.Text = CommonFunctions.FormatDate(request.RequestDate);
            lblEquipWithTechRequestsStatusValue.Text = (request.EquipWithTechRequestsStatus != null ? request.EquipWithTechRequestsStatus.StatusName : "");
            lblMilitaryUnitValue.Text = (request.MilitaryUnit != null ? request.MilitaryUnit.DisplayTextForSelection : "");
            lblAdministrationValue.Text = (request.Administration != null ? request.Administration.AdministrationName : "");
        }

        private void LoadReportDeatils()
        {
            string html = "";

            //Get the list of records according to the specified filters, order and paging
            List<ReportListTechnicsWithAppointDetailsBlock> reportBlocks = ReportListTechnicsWithAppointmentsBlockUtil.GetReportListTechnicsWithAppointDetailsBlockList(EquipmentTechnicsRequestId, CurrentUser);

            //No data found
            if (reportBlocks.Count == 0)
            {
                html = "<span>Няма техника с МН към тази заявка</span>";
            }
            //If there is data then generate dynamically the HTML for the data grid
            else
            {
                bool isMilCommandHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_DETAILS_MILCOMMAND") == UIAccessLevel.Hidden;
                bool isTechnicsTypeHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_DETAILS_TECHNICSTYPE") == UIAccessLevel.Hidden;
                bool isNormativeTechnicsHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_DETAILS_NORMATIVETECHNICS") == UIAccessLevel.Hidden;
                bool isReadinessHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_DETAILS_READINESS") == UIAccessLevel.Hidden;
                bool isRegInvNumberHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_DETAILS_REGINVNUMBER") == UIAccessLevel.Hidden;
                bool isOwnerHidden = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_DETAILS_OWNER") == UIAccessLevel.Hidden;

                string headerStyle = "vertical-align: bottom;";

                //Setup the header of the grid
                html = @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='width: 20px;" + headerStyle + @"'></th>" +
       (!isMilCommandHidden ? "<th style='width: 140px; " + headerStyle + @"' >Команда</th>" : "") +
     (!isTechnicsTypeHidden ? "<th style='width: 120px; " + headerStyle + @"' >Вид техника</th>" : "") +
(!isNormativeTechnicsHidden ? "<th style='width: 130px; " + headerStyle + @"' >Нормативна категория</th>" : "") +
        (!isReadinessHidden ? "<th style='width: 110px; " + headerStyle + @"' >Начин на явяване</th>" : "") +
     (!isRegInvNumberHidden ? "<th style='width: 120px; " + headerStyle + @"' >Рег./Инв. номер</th>" : "") +
            (!isOwnerHidden ? "<th style='width: 230px; " + headerStyle + @"' >Собственик</th>" : "") + @"
                            </tr>
                         </thead>";

                int counter = 1;

                //Iterate through all items and add them into the grid
                foreach (ReportListTechnicsWithAppointDetailsBlock reportBlock in reportBlocks)
                {
                    string cellStyle = "vertical-align: top;";

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='" + cellStyle + @"'>" + counter.ToString() + @"</td>
                                 <td style='" + cellStyle + @"'><img src='../Images/pawn_view.png' alt='Преглед' title='Преглед' class='GridActionIcon' onclick='PreviewTechnics(" + reportBlock.TechnicsId.ToString() + @");' /></td>" +
         (!isMilCommandHidden ? "<td style='" + cellStyle + @"'>" + reportBlock.MilitaryCommand + @"</td>" : "") +
       (!isTechnicsTypeHidden ? "<td style='" + cellStyle + @"'>" + reportBlock.TechnicsType + @"</td>" : "") +
  (!isNormativeTechnicsHidden ? "<td style='" + cellStyle + @"'>" + reportBlock.NormativeTechnics + @"</td>" : "") +
          (!isReadinessHidden ? "<td style='" + cellStyle + @"'>" + reportBlock.Readiness + @"</td>" : "") +
       (!isRegInvNumberHidden ? "<td style='" + cellStyle + @"'>" + reportBlock.RegInvNumber + @"</td>" : "") +
              (!isOwnerHidden ? "<td style='" + cellStyle + @"'>" + reportBlock.Owner + @"</td>" : "") + @"
                              </tr>";

                    counter++;
                }

                html += "</table>";
            }

            //Put the generated grid on the page
            pnlItemsGrid.InnerHtml = html;
        }

        // Setup user interface elements according to rights of the user's role
        private void SetupPageUI()
        {
            UIAccessLevel l;

            l = GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_REQUEST_REQNUMBER");

            if (l == UIAccessLevel.Disabled)
            {
                pageDisabledControls.Add(lblRequestNumber);
                pageDisabledControls.Add(lblRequestNumberValue);
            }
            if (l == UIAccessLevel.Hidden)
            {
                pageHiddenControls.Add(lblRequestNumber);
                pageHiddenControls.Add(lblRequestNumberValue);
            }

            l = this.GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_REQUEST_REQDATE");
            if (l == UIAccessLevel.Disabled)
            {
                pageDisabledControls.Add(lblRequestDate);
                pageDisabledControls.Add(lblRequestDateValue);
            }
            if (l == UIAccessLevel.Hidden)
            {
                this.pageHiddenControls.Add(lblRequestDate);
                this.pageHiddenControls.Add(lblRequestDateValue);
            }

            l = this.GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_REQUEST_REQSTATUS");
            if (l == UIAccessLevel.Disabled)
            {
                pageDisabledControls.Add(lblEquipWithTechRequestsStatus);
                pageDisabledControls.Add(lblEquipWithTechRequestsStatusValue);
            }
            if (l == UIAccessLevel.Hidden)
            {
                this.pageHiddenControls.Add(lblEquipWithTechRequestsStatus);
                this.pageHiddenControls.Add(lblEquipWithTechRequestsStatusValue);
            }

            l = this.GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_REQUEST_MILUNIT");
            if (l == UIAccessLevel.Disabled)
            {
                pageDisabledControls.Add(lblMilitaryUnit);
                pageDisabledControls.Add(lblMilitaryUnitValue);
            }
            if (l == UIAccessLevel.Hidden)
            {
                this.pageHiddenControls.Add(lblMilitaryUnit);
                this.pageHiddenControls.Add(lblMilitaryUnitValue);
            }

            l = this.GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS_REQUEST_ADMINISTRATION");
            if (l == UIAccessLevel.Disabled)
            {
                pageDisabledControls.Add(lblAdministration);
                pageDisabledControls.Add(lblAdministrationValue);
            }
            if (l == UIAccessLevel.Hidden)
            {
                this.pageHiddenControls.Add(lblAdministration);
                this.pageHiddenControls.Add(lblAdministrationValue);
            }
        }
    }
}
