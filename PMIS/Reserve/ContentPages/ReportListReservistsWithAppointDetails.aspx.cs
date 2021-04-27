using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class ReportListReservistsWithAppointDetails : RESPage
    {
        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "RES_REPORTS_LISTRESWITHAPPOINTMENTS";
            }
        }

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

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if (GetUIItemAccessLevel("RES_REPORTS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            lblMilitaryUnit.Text = "Заявката е от " + CommonFunctions.GetLabelText("MilitaryUnit") + ":";

            LoadHeaderInfo();
            LoadReportDeatils();

            //Hilight the current page in the menu bar
            HighlightMenuItems("Reports", "ReportListReservistsWithAppointments");

            SetupPageUI();

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);
        }

        //Navigate to the previous page
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ContentPages/ReportListReservistsWithAppointments.aspx");
        }

        private void LoadHeaderInfo()
        {
            EquipmentReservistsRequest request = EquipmentReservistsRequestUtil.GetEquipmentReservistsRequest(EquipmentReservistsRequestId, CurrentUser);

            lblRequestNumberValue.Text = request.RequestNumber;
            lblRequestDateValue.Text = CommonFunctions.FormatDate(request.RequestDate);
            lblEquipWithResRequestsStatusValue.Text = (request.EquipWithResRequestsStatus != null ? request.EquipWithResRequestsStatus.StatusName : "");
            lblMilitaryUnitValue.Text = (request.MilitaryUnit != null ? request.MilitaryUnit.DisplayTextForSelection : "");
            lblAdministrationValue.Text = (request.Administration != null ? request.Administration.AdministrationName : "");
        }

        private void LoadReportDeatils()
        {
            string html = "";

            //Get the list of records according to the specified filters, order and paging
            List<ReportListReservistsWithAppointDetailsBlock> reportBlocks = ReportListReservistsWithAppointmentsBlockUtil.GetReportListReservistsWithAppointDetailsBlockList(EquipmentReservistsRequestId, CurrentUser);

            //No data found
            if (reportBlocks.Count == 0)
            {
                html = "<span>Няма хора с МН към тази заявка</span>";
            }
            //If there is data then generate dynamically the HTML for the data grid
            else
            {
                bool isMilCommandHidden = GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_DETAILS_MILCOMMAND") == UIAccessLevel.Hidden;
                bool isPositionHidden = GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_DETAILS_POSITION") == UIAccessLevel.Hidden;
                bool isMilRepSpecialityHidden = GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_DETAILS_MILREPSPECIALITY") == UIAccessLevel.Hidden;
                bool isMilRankHidden = GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_DETAILS_MILRANK") == UIAccessLevel.Hidden;
                bool isReadinessHidden = GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_DETAILS_READINESS") == UIAccessLevel.Hidden;
                bool isIdentityNumberHidden = GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_DETAILS_IDENTITYNUMBER") == UIAccessLevel.Hidden;
                bool isFullNameHidden = GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_DETAILS_FULLNAME") == UIAccessLevel.Hidden;
                bool isPermCityNameHidden = GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_DETAILS_PERMCITYNAME") == UIAccessLevel.Hidden;

                string headerStyle = "vertical-align: bottom;";

                //Setup the header of the grid
                html = @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='width: 20px;" + headerStyle + @"'></th>" +
       (!isMilCommandHidden ? "<th style='width: 110px; " + headerStyle + @"' >Команда</th>" : "") +
         (!isPositionHidden ? "<th style='width: 140px; " + headerStyle + @"' >Длъжност</th>" : "") +
 (!isMilRepSpecialityHidden ? "<th style='width: 170px; " + headerStyle + @"' >ВОС</th>" : "") +
          (!isMilRankHidden ? "<th style='width: 80px; " + headerStyle + @"' >Звание</th>" : "") +
        (!isReadinessHidden ? "<th style='width: 110px; " + headerStyle + @"' >Начин на явяване</th>" : "") +
   (!isIdentityNumberHidden ? "<th style='width: 90px; " + headerStyle + @"' >ЕГН</th>" : "") +
         (!isFullNameHidden ? "<th style='width: 180px; " + headerStyle + @"' >Име</th>" : "") +
     (!isPermCityNameHidden ? "<th style='width: 120px; " + headerStyle + @"' >Населено място</th>" : "") + @"
                            </tr>
                         </thead>";

                int counter = 1;

                //Iterate through all items and add them into the grid
                foreach (ReportListReservistsWithAppointDetailsBlock reportBlock in reportBlocks)
                {
                    string cellStyle = "vertical-align: top;";

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='" + cellStyle + @"'>" + counter.ToString() + @"</td>
                                 <td style='" + cellStyle + @"'><img src='../Images/user.png' alt='Преглед' title='Преглед' class='GridActionIcon' onclick='PreviewReservist(" + reportBlock.ReservistId.ToString() + @");' /></td>" +
         (!isMilCommandHidden ? "<td style='" + cellStyle + @"'>" + reportBlock.MilitaryCommand + @"</td>" : "") +
           (!isPositionHidden ? "<td style='" + cellStyle + @"'>" + reportBlock.Position + @"</td>" : "") +
   (!isMilRepSpecialityHidden ? "<td style='" + cellStyle + @"'>" + reportBlock.MilitaryReportingSpecialty + @"</td>" : "") +
            (!isMilRankHidden ? "<td style='" + cellStyle + @"'>" + reportBlock.MilitaryRank + @"</td>" : "") +
          (!isReadinessHidden ? "<td style='" + cellStyle + @"'>" + reportBlock.Readiness + @"</td>" : "") +
     (!isIdentityNumberHidden ? "<td style='" + cellStyle + @"'>" + reportBlock.IdentityNumber + @"</td>" : "") +
           (!isFullNameHidden ? "<td style='" + cellStyle + @"'>" + reportBlock.FullName + @"</td>" : "") +
       (!isPermCityNameHidden ? "<td style='" + cellStyle + @"'>" + reportBlock.PermPlaceName + @"</td>" : "") + @"
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

            l = GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_REQUEST_REQNUMBER");

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

            l = this.GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_REQUEST_REQDATE");
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

            l = this.GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_REQUEST_REQSTATUS");
            if (l == UIAccessLevel.Disabled)
            {
                pageDisabledControls.Add(lblEquipWithResRequestsStatus);
                pageDisabledControls.Add(lblEquipWithResRequestsStatusValue);
            }
            if (l == UIAccessLevel.Hidden)
            {
                this.pageHiddenControls.Add(lblEquipWithResRequestsStatus);
                this.pageHiddenControls.Add(lblEquipWithResRequestsStatusValue);
            }

            l = this.GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_REQUEST_MILUNIT");
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

            l = this.GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS_REQUEST_ADMINISTRATION");
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
