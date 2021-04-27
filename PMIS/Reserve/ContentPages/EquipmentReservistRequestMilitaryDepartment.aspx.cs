using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using PMIS.Common;
using PMIS.Reserve.Common;
using System.Text;
using System.Linq;

namespace PMIS.Reserve.ContentPages
{
    public partial class EquipmentReservistRequestMilitaryDepartment : RESPage
    {       
        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "RES_EQUIPRESREQUESTS_MILDEPT";
            }
        }

        //This property represents the ID of the EquipmentReservistsRequest object that is loaded on the screen        
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
            if (GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_MILDEPT") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSPopulateMilitaryDepartments")
            {
                JSPopulateMilitaryDepartments();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSAddMilitaryDepartment")
            {
                JSAddMilitaryDepartment();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteMilitaryDepartment")
            {
                JSDeleteMilitaryDepartment();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveReservistsCount")
            {
                JSSaveReservistsCount();
                return;
            }

            //Hilight the current page in the menu bar
            HighlightMenuItems("Equipment");            

            SetupPageUI();
            
            //When the page is loaded for the first time
            if (!IsPostBack)
            {
                // disable save button, untill Request Command selected
                DisableButton(btnSave);

                LoadData();

                if (ddMilitaryCommand.Items.Count > 1)
                {
                    ddMilitaryCommand.SelectedIndex = 1;
                    ddMilitaryCommand_Changed(null, null);
                }              
            }

            lblGridMessage.Text = "";

            //Set the message if there is a need (e.g. save data)
            if (hdnRefreshReason.Value != "")
            {
                if (hdnRefreshReason.Value == "SAVED")
                {
                    lblGridMessage.Text = "Данните са записани успешно";
                    lblGridMessage.CssClass = "SuccessText";
                }

                hdnRefreshReason.Value = "";
            }
        }

        private void LoadData()
        {
            EquipmentReservistsRequest equipmentReservistsRequest = EquipmentReservistsRequestUtil.GetEquipmentReservistsRequest(EquipmentReservistsRequestId, CurrentUser);

            txtRequestNumber.Text = equipmentReservistsRequest.RequestNumber;
            txtRequestDate.Text = CommonFunctions.FormatDate(equipmentReservistsRequest.RequestDate);
            txtEquipWithResRequestsStatus.Text = equipmentReservistsRequest.EquipWithResRequestsStatus != null ? equipmentReservistsRequest.EquipWithResRequestsStatus.StatusName : "";
            txtMilitaryUnit.Text = equipmentReservistsRequest.MilitaryUnit != null ? equipmentReservistsRequest.MilitaryUnit.DisplayTextForSelection : "";
            txtAdministration.Text = equipmentReservistsRequest.Administration != null ? equipmentReservistsRequest.Administration.AdministrationName : "";

            ddMilitaryCommand.DataSource = equipmentReservistsRequest.RequestCommands;
            ddMilitaryCommand.DataTextField = "DisplayText";
            ddMilitaryCommand.DataValueField = "RequestCommandId";
            ddMilitaryCommand.DataBind();
            ddMilitaryCommand.Items.Insert(0, ListItems.GetOptionChooseOne());
        }

        // Setup user interface elements according to rights of the user's role
        private void SetupPageUI()
        {
            if (this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_MILDEPT") != UIAccessLevel.Enabled || this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_MILDEPT_MILDEPT") != UIAccessLevel.Enabled)
            {
                pageDisabledControls.Add(btnSave);
            }

            // Equipment Reservist Request data section
            if (this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_MILDEPT_REQUEST") == UIAccessLevel.Hidden ||
                this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_MILDEPT_REQUEST_REQUESTNUMBER") == UIAccessLevel.Hidden)
            {
                pageHiddenControls.Add(lblRequestNumber);
                pageHiddenControls.Add(txtRequestNumber);
            }

            if (this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_MILDEPT_REQUEST") == UIAccessLevel.Hidden ||
                this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_MILDEPT_REQUEST_REQUESTDATE") == UIAccessLevel.Hidden)
            {
                pageHiddenControls.Add(lblRequestDate);
                pageHiddenControls.Add(txtRequestDate);
            }

            if (this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_MILDEPT_REQUEST") == UIAccessLevel.Hidden ||
                this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_MILDEPT_REQUEST_REQUESTSTATUS") == UIAccessLevel.Hidden)
            {
                pageHiddenControls.Add(lblEquipWithResRequestsStatus);
                pageHiddenControls.Add(txtEquipWithResRequestsStatus);
            }

            if (this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_MILDEPT_REQUEST") == UIAccessLevel.Hidden ||
                this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_MILDEPT_REQUEST_MILITARYUNIT") == UIAccessLevel.Hidden)
            {
                pageHiddenControls.Add(lblMilitaryUnit);
                pageHiddenControls.Add(txtMilitaryUnit);
            }

            if (this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_MILDEPT_REQUEST") == UIAccessLevel.Hidden ||
                this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_MILDEPT_REQUEST_ADMINISTRATION") == UIAccessLevel.Hidden)
            {
                pageHiddenControls.Add(lblAdministration);
                pageHiddenControls.Add(txtAdministration);
            }

            // Request Command data section
            if (this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_MILDEPT_COMMAND") == UIAccessLevel.Hidden ||
                this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_MILDEPT_COMMAND_TIME") == UIAccessLevel.Hidden)
            {
                pageHiddenControls.Add(lblTime);
                pageHiddenControls.Add(txtTime);
            }

            if (this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_MILDEPT_COMMAND") == UIAccessLevel.Hidden ||
                this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_MILDEPT_COMMAND_READINESS") == UIAccessLevel.Hidden)
            {
                pageHiddenControls.Add(lblReadiness);
                pageHiddenControls.Add(txtReadiness);
            }

            if (this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_MILDEPT_COMMAND") == UIAccessLevel.Hidden ||
                this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_MILDEPT_COMMAND_DELIVERYLOCATION") == UIAccessLevel.Hidden)
            {
                pageHiddenControls.Add(lblDeliveryLocation);
                pageHiddenControls.Add(txtDeliveryLocation);
            }
        }        
        
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ContentPages/ManageEquipmentReservistsRequests.aspx");
        }

        protected void ddMilitaryCommand_Changed(object sender, EventArgs e)
        {
            if (ddMilitaryCommand.SelectedValue != "-1")
            {
                EnableButton(btnSave);

                int requestCommantId = int.Parse(ddMilitaryCommand.SelectedValue);

                RequestCommand command = RequestCommandUtil.GetRequestsCommand(CurrentUser, requestCommantId);

                if (command.DeliveryCity != null)
                {
                    txtDeliveryLocation.Text = command.DeliveryCity.Region.RegionName;
                    txtDeliveryLocation.Text += ", " + command.DeliveryCity.Municipality.MunicipalityName;
                    txtDeliveryLocation.Text += ", " + command.DeliveryCity.CityName;
                    txtDeliveryLocation.Text += ", " + command.DeliveryPlace;

                    if (command.AppointmentTime.HasValue)
                        txtTime.Text = command.AppointmentTime.ToString() + " часа";
                    else
                        txtTime.Text = "";

                    if (command.MilitaryReadinessId.HasValue)
                        txtReadiness.Text = command.MilitaryReadiness.MilReadinessName;
                    else
                        txtReadiness.Text = "";
                }

                List<RequestCommandPosition> requestCommandPositions = RequestCommandPositionUtil.GetRequestCommandPositionsWithMilDepts(CurrentUser, command.RequestCommandId);

                pnlDataGrid.InnerHtml = GenerateTable(requestCommandPositions);
            }
            else
            {
                // clean old values
                txtDeliveryLocation.Text = "";
                txtTime.Text = "";
                txtReadiness.Text = "";
                pnlDataGrid.InnerHtml = "";

                // disable save button
                DisableButton(btnSave);
            }
        }

        // Generates html table with request command positions crossed to military departments
        private string GenerateTable(List<RequestCommandPosition> requestCommandPositions)
        {
            StringBuilder sb = new StringBuilder();

            // list of associated military departments - for generating columns of the table
            List<MilitaryDepartment> militaryDepartments = new List<MilitaryDepartment>();

            // randomly choosen RequestCommandPosition - the first one with more than 1 PositionMilitaryDepartments (or null)
            RequestCommandPosition randomRequestCommandPosition = (from r in requestCommandPositions where r.PositionMilitaryDepartments.Count > 0 select r).FirstOrDefault();
            
            // if there is at least one RequestCommandPosition with PositionMilitaryDepartments, then use them to fulfill list (in normal case all RequestCommandPosition objects have the same count of PositionMilitaryDepartments)
            if (randomRequestCommandPosition != null)
                militaryDepartments = (from m in randomRequestCommandPosition.PositionMilitaryDepartments select m.MilitaryDepartment).ToList();

            bool IsPositionHidden = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_MILDEPT_POSITION_POSITION") == UIAccessLevel.Hidden;
            bool IsMilRepSpecHidden = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_MILDEPT_POSITION_MILREPSPEC") == UIAccessLevel.Hidden;
            bool IsRankHidden = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_MILDEPT_POSITION_RANK") == UIAccessLevel.Hidden;
            bool IsReservistsHidden = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_MILDEPT_POSITION_RESERVISTS") == UIAccessLevel.Hidden;
            bool IsReservistsCountHidden = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_MILDEPT_MILDEPT_EDIT") == UIAccessLevel.Hidden;

            bool isScreenEnabled = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_MILDEPT") == UIAccessLevel.Enabled;
            bool isTableEnabled = isScreenEnabled && this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_MILDEPT_MILDEPT") == UIAccessLevel.Enabled;
            bool IsReservistsCountEnabled = isTableEnabled && (this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_MILDEPT_MILDEPT_EDIT") == UIAccessLevel.Enabled);

            string reservistsCountDisplayStyle = IsReservistsCountHidden ? "display : none" : "";            

            sb.Append("<center>");
            sb.Append("<table id='positionsTable' name='positionsTable' class='CommonHeaderTable' style='text-align: left;' border='1px'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style=\"min-width: 30px;\"></th>");
            if (!IsPositionHidden)
                sb.Append("<th style=\"min-width: 150px;\">Длъжност</th>");
            if (!IsMilRepSpecHidden)
                sb.Append("<th style=\"min-width: 65px;\">ВОС</th>");
            if (!IsRankHidden)
                sb.Append("<th style=\"min-width: 65px;\">Звание</th>");
            if (!IsReservistsHidden)
                sb.Append("<th style=\"min-width: 65px;\">Запасни</th>");

            foreach (MilitaryDepartment militaryDepartment in militaryDepartments)
            {
                string deleteHTML = "";
                if (isTableEnabled && this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_MILDEPT_MILDEPT_DELETE") == UIAccessLevel.Enabled)
                    deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на това военно окръжие' class='GridActionIcon' style='float: right; margin-right: 0px;' onclick='DeleteMilitaryDepartment(" + militaryDepartment.MilitaryDepartmentId.ToString() + ",\"" + militaryDepartment.MilitaryDepartmentName + "\");' />";
                sb.Append("<th style=\"width: 80px; " + reservistsCountDisplayStyle + " \">" + militaryDepartment.MilitaryDepartmentName + deleteHTML + "</th>");
            }
            
            sb.Append("</tr>");
            sb.Append("</thead>");
            int counter = 0;

            if (requestCommandPositions.Count > 0)
            {
                sb.Append("<tbody>");
            }

            foreach (RequestCommandPosition requestCommandPosition in requestCommandPositions)
            {               
                counter++;                               

                sb.Append("<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + "'>");
                sb.Append("<td>" + counter + "</td>");
                if (!IsPositionHidden)
                    sb.Append("<td>" + requestCommandPosition.Position + "</td>");
                if (!IsMilRepSpecHidden)
                    sb.Append("<td>" + requestCommandPosition.MilRepSpecHTML + "</td>");
                if (!IsRankHidden)
                    sb.Append("<td>" + requestCommandPosition.MilRankHTML + "</td>");
                if (!IsReservistsHidden)
                    sb.Append("<td>" + requestCommandPosition.ReservistsCount + "</td>");

                int milDeptsCounter = 0;
                foreach (RequestCommandPositionMilDept milDept in requestCommandPosition.PositionMilitaryDepartments)
                {
                    milDeptsCounter++;

                    string reservistsCountHTML = "";
                    if (IsReservistsCountEnabled)
                        reservistsCountHTML = "<input type='text' id='reservistsCount" + counter + "_" + milDeptsCounter + "' style='width: 50px;' value='" + (milDept.ReservistsCount.HasValue ? milDept.ReservistsCount.Value.ToString() : "") + "'/>";
                    else
                    {
                        reservistsCountHTML = "<input type='text' id='reservistsCount" + counter + "_" + milDeptsCounter + "' style='width: 50px; display: none;' value='" + (milDept.ReservistsCount.HasValue ? milDept.ReservistsCount.Value.ToString() : "") + "'/>";
                        reservistsCountHTML += milDept.ReservistsCount.HasValue ? milDept.ReservistsCount.Value.ToString() : "";
                    }

                    sb.Append("<td style='" + reservistsCountDisplayStyle + "' align='center'>" + reservistsCountHTML + "</td>");
                    sb.Append("<input type='hidden' id='militaryDepartmentID" + counter + "_" + milDeptsCounter + "' value='" + milDept.MilitaryDepartmentID + "' />");
                    sb.Append("<input type='hidden' id='reqCommandPositionMilDeptID" + counter + "_" + milDeptsCounter + "' value='" + milDept.ReqCommandPositionMilDeptID + "' />");                    
                }

                sb.Append("<input type='hidden' id='requestCommandPositionID" + counter + "' value='" + requestCommandPosition.RequestCommandPositionId + "' />");
                sb.Append("<input type='hidden' id='reservistsCount" + counter + "' value='" + requestCommandPosition.ReservistsCount + "' />");

                sb.Append("</tr>");
            }

            if (requestCommandPositions.Count > 0)
            {
                sb.Append("</tbody>");
            }

            sb.Append("<input type='hidden' id='positionsCounter' value='" + counter + "'/>");
            sb.Append("<input type='hidden' id='militaryDepartmentsCounter' value='" + militaryDepartments.Count + "'/>");
            sb.Append("</table>");
            sb.Append("</center>");

            if (requestCommandPositions.Count > 0)
            {
                if (isTableEnabled && this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_MILDEPT_MILDEPT_ADD") == UIAccessLevel.Enabled)
                    sb.Append(@"<div id=""btnShowAddMilitaryDepartmentLightBox"" style=""display: inline-block; padding-top: 20px;"" onclick=""ShowAddMilitaryDepartmentLightBox();"" class=""Button"">
                                    <i></i>
                                        <div id=""btnShowAddMilitaryDepartmentLightBoxText"" style=""width: 110px;"">Добавяне на ВО</div>
                                    <b></b>
                                </div>");
                sb.Append("<input type='hidden' id='RequestCommandID' value='" + requestCommandPositions.First().RequestsCommandId + "'/>");
            }

            return sb.ToString();
        }

        //Populate MilitaryDepartments (ajax call)
        private void JSPopulateMilitaryDepartments()
        {
            int requestCommandID = int.Parse(Request.Form["RequestCommandID"]);

            string stat = "";
            string response = "<md>" +
                                 "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                                 "<name>" + ListItems.GetOptionChooseOne().Text + "</name>" +
                              "</md>";

            try
            {
                List<MilitaryDepartment> militaryDepartments = MilitaryDepartmentUtil.GetAllMilitaryDepartmentsForRequestCommandPosition(requestCommandID, CurrentUser);

                foreach (MilitaryDepartment militaryDepartment in militaryDepartments)
                {
                    response += "<md>" +
                                   "<id>" + militaryDepartment.MilitaryDepartmentId.ToString() + "</id>" +
                                   "<name>" + militaryDepartment.MilitaryDepartmentName + "</name>" +
                                "</md>";
                }

                stat = AJAXTools.OK;
            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }


            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        // Add MilitaryDepartments by ajax call
        private void JSAddMilitaryDepartment()
        {
            string response = "";

            int equipmentReservistsRequestID = int.Parse(Request.Form["EquipmentReservistsRequestID"]);
            int requestCommandID = int.Parse(Request.Form["RequestCommandID"]);
            int militaryDepartmentID = int.Parse(Request.Form["MilitaryDepartmentID"]);            

            Change change = new Change(CurrentUser, "RES_EquipmentReservistsRequests");

            bool result = RequestCommandPositionMilDeptUtil.AddMilitaryDepartmentToRequestCommandPositions(equipmentReservistsRequestID, requestCommandID, militaryDepartmentID, CurrentUser, change);

            change.WriteLog();

            response = result ? AJAXTools.OK : AJAXTools.ERROR;

            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();
        }

        // Delete MilitaryDepartments by ajax call
        private void JSDeleteMilitaryDepartment()
        {
            string response = "";

            int equipmentReservistsRequestID = int.Parse(Request.Form["EquipmentReservistsRequestID"]);
            int requestCommandID = int.Parse(Request.Form["RequestCommandID"]);
            int militaryDepartmentID = int.Parse(Request.Form["MilitaryDepartmentID"]);

            Change change = new Change(CurrentUser, "RES_EquipmentReservistsRequests");

            //Get all reservists added to that command via that military department
            List<FillReservistsRequest> fillReservistRequests = FillReservistsRequestUtil.GetAllFillReservistsRequestByCommandAndMilDept(requestCommandID, militaryDepartmentID, CurrentUser);
            List<int> deletedReservists = new List<int>();

            //Remove all Reservists from that Command that have been added via that Military Department
            foreach (FillReservistsRequest fillReservistRequest in fillReservistRequests)
            {
                FillReservistsRequestUtil.DeleteRequestCommandReservist(fillReservistRequest.FillReservistsRequestID, fillReservistRequest.MilitaryDepartmentID, CurrentUser, change);

                //Just in case: clear the status and the appointment for each reservist only once (note each reservist should be added only once to a particular command)
                if (!deletedReservists.Contains(fillReservistRequest.ReservistID))
                {
                    //Change the current Military Reporting Status of each reservist
                    ReservistMilRepStatusUtil.SetMilRepStatusTo_FREE(fillReservistRequest.ReservistID, CurrentUser, change);

                    //Clear the current Mobilization Appointment for each Reservist
                    ReservistAppointmentUtil.ClearTheCurrentReservistAppointmentByReservist(fillReservistRequest.ReservistID, CurrentUser, change);

                    deletedReservists.Add(fillReservistRequest.ReservistID);
                }
            }

            bool result = RequestCommandPositionMilDeptUtil.DeleteMilitaryDepartmentFromRequestCommandPositions(equipmentReservistsRequestID, requestCommandID, militaryDepartmentID, CurrentUser, change);

            change.WriteLog();

            response = result ? AJAXTools.OK : AJAXTools.ERROR;

            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();
        }

        // saves all position by ajax request, if needed
        private void JSSaveReservistsCount()
        {
            string response = "";

            bool result = true;

            int equipmentReservistsRequestID = int.Parse(Request.Form["EquipmentReservistsRequestID"]);
            int positionsCount = int.Parse(Request.Params["PositionsCount"]);
            int militaryDepartmentsCount = int.Parse(Request.Params["MilitaryDepartmentsCount"]);
            int requestCommandID = int.Parse(Request.Params["RequestCommandID"]);

            List<RequestCommandPosition> oldPositions = RequestCommandPositionUtil.GetRequestCommandPositionsWithMilDepts(CurrentUser, requestCommandID);

            List<RequestCommandPosition> newPositions = new List<RequestCommandPosition>();

            for (int i = 1; i <= positionsCount; i++)
            {
                int requestCommandPositionID = int.Parse(Request.Params["RequestCommandPositionID" + i.ToString()]);

                RequestCommandPosition position = new RequestCommandPosition(CurrentUser);
                position.RequestCommandPositionId = requestCommandPositionID;
                position.RequestsCommandId = requestCommandID;
                position.PositionMilitaryDepartments = new List<RequestCommandPositionMilDept>();

                for (int j = 1; j <= militaryDepartmentsCount; j++)
                {
                    int reqCommandPositionMilDeptID = int.Parse(Request.Params["ReqCommandPositionMilDeptID" + i.ToString() + "_" + j.ToString()]);
                    int militaryDepartmentID = int.Parse(Request.Params["MilitaryDepartmentID" + i.ToString() + "_" + j.ToString()]);
                    int? reservistsCount = !string.IsNullOrEmpty(Request.Params["ReservistsCount" + i.ToString() + "_" + j.ToString()]) ? (int?)int.Parse(Request.Params["ReservistsCount" + i.ToString() + "_" + j.ToString()]) : null;

                    RequestCommandPositionMilDept milDept = new RequestCommandPositionMilDept(CurrentUser);
                    milDept.ReqCommandPositionMilDeptID = reqCommandPositionMilDeptID;
                    milDept.RequestCommandPositionID = requestCommandPositionID;
                    milDept.MilitaryDepartmentID = militaryDepartmentID;
                    milDept.ReservistsCount = reservistsCount;

                    position.PositionMilitaryDepartments.Add(milDept);
                }

                newPositions.Add(position);
            }

            List<RequestCommandPosition> newPositionsToSave = (from n in newPositions
                                                               join o in oldPositions on n.RequestCommandPositionId equals o.RequestCommandPositionId
                                                               where ((from m1 in n.PositionMilitaryDepartments join m2 in o.PositionMilitaryDepartments on m1.ReqCommandPositionMilDeptID equals m2.ReqCommandPositionMilDeptID where (m1.ReservistsCount != m2.ReservistsCount) select m1).Count() > 0)
                                                               select n).ToList();

            List<RequestCommandPosition> oldPositionsToSave = (from o in oldPositions
                                                               join n in newPositionsToSave on o.RequestCommandPositionId equals n.RequestCommandPositionId
                                                               select o).ToList();

            if (newPositionsToSave.Count > 0)
            {
                Change change = new Change(CurrentUser, "RES_EquipmentReservistsRequests");

                result = RequestCommandPositionMilDeptUtil.SaveMilitaryDepartmentsToRequestCommandPositions(equipmentReservistsRequestID, oldPositionsToSave, newPositionsToSave, CurrentUser, change);

                change.WriteLog();
            }

            response = result ? AJAXTools.OK : AJAXTools.ERROR;

            AJAX a = new AJAX(AJAXTools.EncodeForXML(response), this.Response);
            a.Write();
            Response.End();
        }
    }
}
