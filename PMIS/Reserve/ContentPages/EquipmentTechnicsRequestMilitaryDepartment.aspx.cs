using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using PMIS.Common;
using PMIS.Reserve.Common;
using System.Text;
using System.Linq;

namespace PMIS.Reserve.ContentPages
{
    public partial class EquipmentTechnicsRequestMilitaryDepartment : RESPage
    {       
        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "RES_EQUIPTECHREQUESTS_MILDEPT";
            }
        }

        //This property represents the ID of the EquipmentTechnicsRequest object that is loaded on the screen        
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
            if (GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_MILDEPT") == UIAccessLevel.Hidden)
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
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveCount")
            {
                JSSaveCount();
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
            EquipmentTechnicsRequest equipmentTechnicsRequest = EquipmentTechnicsRequestUtil.GetEquipmentTechnicsRequest(EquipmentTechnicsRequestId, CurrentUser);

            txtRequestNumber.Text = equipmentTechnicsRequest.RequestNumber;
            txtRequestDate.Text = CommonFunctions.FormatDate(equipmentTechnicsRequest.RequestDate);
            txtEquipWithTechRequestsStatus.Text = equipmentTechnicsRequest.EquipWithTechRequestsStatus != null ? equipmentTechnicsRequest.EquipWithTechRequestsStatus.StatusName : "";
            txtMilitaryUnit.Text = equipmentTechnicsRequest.MilitaryUnit != null ? equipmentTechnicsRequest.MilitaryUnit.DisplayTextForSelection : "";
            txtAdministration.Text = equipmentTechnicsRequest.Administration != null ? equipmentTechnicsRequest.Administration.AdministrationName : "";

            ddMilitaryCommand.DataSource = equipmentTechnicsRequest.TechnicsRequestCommands;
            ddMilitaryCommand.DataTextField = "DisplayText";
            ddMilitaryCommand.DataValueField = "TechnicsRequestCommandId";
            ddMilitaryCommand.DataBind();
            ddMilitaryCommand.Items.Insert(0, ListItems.GetOptionChooseOne());
        }

        // Setup user interface elements according to rights of the user's role
        private void SetupPageUI()
        {
            if (this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_MILDEPT") != UIAccessLevel.Enabled || this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_MILDEPT_MILDEPT") != UIAccessLevel.Enabled)
            {
                pageDisabledControls.Add(btnSave);
            }

            // Equipment Reservist Request data section
            if (this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_MILDEPT_REQUEST") == UIAccessLevel.Hidden ||
                this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_MILDEPT_REQUEST_REQUESTNUMBER") == UIAccessLevel.Hidden)
            {
                pageHiddenControls.Add(lblRequestNumber);
                pageHiddenControls.Add(txtRequestNumber);
            }

            if (this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_MILDEPT_REQUEST") == UIAccessLevel.Hidden ||
                this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_MILDEPT_REQUEST_REQUESTDATE") == UIAccessLevel.Hidden)
            {
                pageHiddenControls.Add(lblRequestDate);
                pageHiddenControls.Add(txtRequestDate);
            }

            if (this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_MILDEPT_REQUEST") == UIAccessLevel.Hidden ||
                this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_MILDEPT_REQUEST_REQUESTSTATUS") == UIAccessLevel.Hidden)
            {
                pageHiddenControls.Add(lblEquipWithTechRequestsStatus);
                pageHiddenControls.Add(txtEquipWithTechRequestsStatus);
            }

            if (this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_MILDEPT_REQUEST") == UIAccessLevel.Hidden ||
                this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_MILDEPT_REQUEST_MILITARYUNIT") == UIAccessLevel.Hidden)
            {
                pageHiddenControls.Add(lblMilitaryUnit);
                pageHiddenControls.Add(txtMilitaryUnit);
            }

            if (this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_MILDEPT_REQUEST") == UIAccessLevel.Hidden ||
                this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_MILDEPT_REQUEST_ADMINISTRATION") == UIAccessLevel.Hidden)
            {
                pageHiddenControls.Add(lblAdministration);
                pageHiddenControls.Add(txtAdministration);
            }

            // Request Command data section
            if (this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_MILDEPT_COMMAND") == UIAccessLevel.Hidden ||
                this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_MILDEPT_COMMAND_TIME") == UIAccessLevel.Hidden)
            {
                pageHiddenControls.Add(lblTime);
                pageHiddenControls.Add(txtTime);
            }

            if (this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_MILDEPT_COMMAND") == UIAccessLevel.Hidden ||
                this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_MILDEPT_COMMAND_READINESS") == UIAccessLevel.Hidden)
            {
                pageHiddenControls.Add(lblReadiness);
                pageHiddenControls.Add(txtReadiness);
            }

            if (this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_MILDEPT_COMMAND") == UIAccessLevel.Hidden ||
                this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_MILDEPT_COMMAND_DELIVERYLOCATION") == UIAccessLevel.Hidden)
            {
                pageHiddenControls.Add(lblDeliveryLocation);
                pageHiddenControls.Add(txtDeliveryLocation);
            }
        }        
        
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ContentPages/ManageEquipmentTechnicsRequests.aspx");
        }

        protected void ddMilitaryCommand_Changed(object sender, EventArgs e)
        {
            if (ddMilitaryCommand.SelectedValue != "-1")
            {
                EnableButton(btnSave);

                int technicsRequestCommantId = int.Parse(ddMilitaryCommand.SelectedValue);

                TechnicsRequestCommand command = TechnicsRequestCommandUtil.GetTechnicsRequestCommand(CurrentUser, technicsRequestCommantId);

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

                List<TechnicsRequestCommandPosition> technicsRequestCommandPositions = TechnicsRequestCommandPositionUtil.GetTechnicsRequestCommandPositionsWithMilDepts(CurrentUser, command.TechnicsRequestCommandId);

                pnlDataGrid.InnerHtml = GenerateTable(technicsRequestCommandPositions);
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

        // Generates html table with technics request command positions crossed to military departments
        private string GenerateTable(List<TechnicsRequestCommandPosition> technicsRequestCommandPositions)
        {
            StringBuilder sb = new StringBuilder();

            // list of associated military departments - for generating columns of the table
            List<MilitaryDepartment> militaryDepartments = new List<MilitaryDepartment>();

            // randomly choosen TechnicsRequestCommandPosition - the first one with more than 1 TechnicsPositionMilitaryDepartments (or null)
            TechnicsRequestCommandPosition randomTechnicsRequestCommandPosition = (from r in technicsRequestCommandPositions where r.TechnicsPositionMilitaryDepartments.Count > 0 select r).FirstOrDefault();
            
            // if there is at least one RequestCommandPosition with PositionMilitaryDepartments, then use them to fulfill list (in normal case all RequestCommandPosition objects have the same count of PositionMilitaryDepartments)
            if (randomTechnicsRequestCommandPosition != null)
                militaryDepartments = (from m in randomTechnicsRequestCommandPosition.TechnicsPositionMilitaryDepartments select m.MilitaryDepartment).ToList();

            bool IsTechnicsTypeHidden = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_MILDEPT_POSITION_TECHNICSTYPE") == UIAccessLevel.Hidden || this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_MILDEPT_POSITION") == UIAccessLevel.Hidden;
            bool IsNormativeTechnicsHidden = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_MILDEPT_POSITION_NORMATIVETECHNICS") == UIAccessLevel.Hidden || this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_MILDEPT_POSITION") == UIAccessLevel.Hidden;
            bool IsCommentHidden = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_MILDEPT_POSITION_COMMENT") == UIAccessLevel.Hidden || this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_MILDEPT_POSITION") == UIAccessLevel.Hidden;
            bool IsCountHidden = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_MILDEPT_POSITION_COUNT") == UIAccessLevel.Hidden || this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_MILDEPT_POSITION") == UIAccessLevel.Hidden;
            bool IsEditHidden = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_MILDEPT_MILDEPT_EDIT") == UIAccessLevel.Hidden;

            bool isScreenEnabled = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_MILDEPT") == UIAccessLevel.Enabled;
            bool isTableEnabled = isScreenEnabled && this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_MILDEPT_MILDEPT") == UIAccessLevel.Enabled;
            bool IsCountEnabled = isTableEnabled && (this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_MILDEPT_MILDEPT_EDIT") == UIAccessLevel.Enabled);

            string countDisplayStyle = IsEditHidden ? "display : none" : "";            

            sb.Append("<center>");
            sb.Append("<table id='positionsTable' name='positionsTable' class='CommonHeaderTable' style='text-align: left;' border='1px'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style=\"min-width: 30px;\"></th>");
            if (!IsTechnicsTypeHidden)
                sb.Append("<th style=\"min-width: 80px; width: 90px;\">Вид техника</th>");
            if (!IsNormativeTechnicsHidden)
                sb.Append("<th style=\"min-width: 110px; width: 160px;\">Нормативна категория</th>");
            if (!IsCommentHidden)
                sb.Append("<th style=\"min-width: 95px;\">Коментар</th>");
            if (!IsCountHidden)
                sb.Append("<th style=\"min-width: 65px;\">Брой, Водачи</th>");

            foreach (MilitaryDepartment militaryDepartment in militaryDepartments)
            {
                string deleteHTML = "";
                if (isTableEnabled && this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_MILDEPT_MILDEPT_DELETE") == UIAccessLevel.Enabled)
                    deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на това военно окръжие' class='GridActionIcon' style='float: right; margin-right: 0px;' onclick='DeleteMilitaryDepartment(" + militaryDepartment.MilitaryDepartmentId.ToString() + ",\"" + militaryDepartment.MilitaryDepartmentName + "\");' />";
                sb.Append("<th style=\"width: 130px; " + countDisplayStyle + " \">" + militaryDepartment.MilitaryDepartmentName + deleteHTML + "</th>");
            }
            
            sb.Append("</tr>");
            sb.Append("</thead>");
            int counter = 0;

            if (technicsRequestCommandPositions.Count > 0)
            {
                sb.Append("<tbody>");
            }

            foreach (TechnicsRequestCommandPosition technicsRequestCommandPosition in technicsRequestCommandPositions)
            {               
                counter++;                               

                sb.Append("<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + "'>");
                sb.Append("<td>" + counter + "</td>");
                if (!IsTechnicsTypeHidden)
                    sb.Append("<td>" + technicsRequestCommandPosition.TechnicsType.TypeName + "</td>");
                if (!IsNormativeTechnicsHidden)
                    sb.Append("<td>" + (technicsRequestCommandPosition.NormativeTechnics != null ? technicsRequestCommandPosition.NormativeTechnics.CodeAndText : "") + "</td>");
                if (!IsCommentHidden)
                    sb.Append("<td>" + technicsRequestCommandPosition.Comment + "</td>");
                if (!IsCountHidden)
                    sb.Append("<td>" + technicsRequestCommandPosition.Count + ", " + technicsRequestCommandPosition.DriversCount + "</td>");

                int milDeptsCounter = 0;
                foreach (TechnicsReqCmdPositionMilDept milDept in technicsRequestCommandPosition.TechnicsPositionMilitaryDepartments)
                {
                    milDeptsCounter++;

                    string countHTML = "";
                    if (IsCountEnabled)
                        countHTML = "<input type='text' title='Брой' id='count" + counter + "_" + milDeptsCounter + "' style='width: 50px;' value='" + (milDept.Count.HasValue ? milDept.Count.Value.ToString() : "") + "'/>&nbsp;" +
                                    "<input type='text' title='Водачи' id='driversCount" + counter + "_" + milDeptsCounter + "' style='width: 50px;' value='" + (milDept.DriversCount.HasValue ? milDept.DriversCount.Value.ToString() : "") + "'/>";
                    else
                    {
                        countHTML = "<input type='text' id='count" + counter + "_" + milDeptsCounter + "' style='width: 50px; display: none;' value='" + (milDept.Count.HasValue ? milDept.Count.Value.ToString() : "") + "'/>" +
                                    "<input type='text' id='driversCount" + counter + "_" + milDeptsCounter + "' style='width: 50px; display: none;' value='" + (milDept.DriversCount.HasValue ? milDept.DriversCount.Value.ToString() : "") + "'/>";
                        countHTML += milDept.Count.HasValue ? milDept.Count.Value.ToString() : "";
                        countHTML += ", ";
                        countHTML += milDept.DriversCount.HasValue ? milDept.DriversCount.Value.ToString() : "";
                    }

                    sb.Append("<td style='" + countDisplayStyle + "' align='center' nowrap='nowrap'>" + countHTML + "</td>");
                    sb.Append("<input type='hidden' id='militaryDepartmentID" + counter + "_" + milDeptsCounter + "' value='" + milDept.MilitaryDepartmentID + "' />");
                    sb.Append("<input type='hidden' id='techReqCommandPositionMilDeptID" + counter + "_" + milDeptsCounter + "' value='" + milDept.TechReqCommandPositionMilDeptID + "' />");
                }

                sb.Append("<input type='hidden' id='technicsRequestCommandPositionID" + counter + "' value='" + technicsRequestCommandPosition.TechnicsRequestCommandPositionId + "' />");
                sb.Append("<input type='hidden' id='count" + counter + "' value='" + technicsRequestCommandPosition.Count + "' />");
                sb.Append("<input type='hidden' id='driversCount" + counter + "' value='" + technicsRequestCommandPosition.DriversCount + "' />");

                sb.Append("</tr>");
            }

            if (technicsRequestCommandPositions.Count > 0)
            {
                sb.Append("</tbody>");
            }

            sb.Append("<input type='hidden' id='positionsCounter' value='" + counter + "'/>");
            sb.Append("<input type='hidden' id='militaryDepartmentsCounter' value='" + militaryDepartments.Count + "'/>");
            sb.Append("</table>");
            sb.Append("</center>");

            if (technicsRequestCommandPositions.Count > 0)
            {
                if (isTableEnabled && this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_MILDEPT_MILDEPT_ADD") == UIAccessLevel.Enabled)
                    sb.Append(@"<div id=""btnShowAddMilitaryDepartmentLightBox"" style=""display: inline-block; padding-top: 20px;"" onclick=""ShowAddMilitaryDepartmentLightBox();"" class=""Button"">
                                    <i></i>
                                        <div id=""btnShowAddMilitaryDepartmentLightBoxText"" style=""width: 110px;"">Добавяне на ВО</div>
                                    <b></b>
                                </div>");
                sb.Append("<input type='hidden' id='TechnicsRequestCommandID' value='" + technicsRequestCommandPositions.First().TechnicsRequestsCommandId + "'/>");
            }

            return sb.ToString();
        }

        //Populate MilitaryDepartments (ajax call)
        private void JSPopulateMilitaryDepartments()
        {
            int technicsRequestCommandID = int.Parse(Request.Form["TechnicsRequestCommandID"]);

            string stat = "";
            string response = "<md>" +
                                 "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                                 "<name>" + ListItems.GetOptionChooseOne().Text + "</name>" +
                              "</md>";

            try
            {
                List<MilitaryDepartment> militaryDepartments = MilitaryDepartmentUtil.GetAllMilitaryDepartmentsForTechnicsRequestCommandPosition(technicsRequestCommandID, CurrentUser);

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

            int equipmentTechnicsRequestID = int.Parse(Request.Form["EquipmentTechnicsRequestID"]);
            int technicsRequestCommandID = int.Parse(Request.Form["TechnicsRequestCommandID"]);
            int militaryDepartmentID = int.Parse(Request.Form["MilitaryDepartmentID"]);            

            Change change = new Change(CurrentUser, "RES_EquipmentTechnicsRequests");

            bool result = TechnicsReqCmdPositionMilDeptUtil.AddMilitaryDepartmentToTechRequestCommandPositions(equipmentTechnicsRequestID, technicsRequestCommandID, militaryDepartmentID, CurrentUser, change);

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

            int equipmentTechnicsRequestID = int.Parse(Request.Form["EquipmentTechnicsRequestID"]);
            int technicsRequestCommandID = int.Parse(Request.Form["TechnicsRequestCommandID"]);
            int militaryDepartmentID = int.Parse(Request.Form["MilitaryDepartmentID"]);

            Change change = new Change(CurrentUser, "RES_EquipmentTechnicsRequests");

            List<FillTechnicsRequest> fillTechnicsRequests = FillTechnicsRequestUtil.GetFillTechnicsRequestByTechReqCommandAndMilDept(technicsRequestCommandID, militaryDepartmentID, CurrentUser);
            List<int> deletedTechnics = new List<int>();

            //Remove the all Technics from that Military Command and Military Department
            foreach (FillTechnicsRequest fillTechnicsRequest in fillTechnicsRequests)
            {
                MilitaryDepartment militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment(fillTechnicsRequest.MilitaryDepartmentID, CurrentUser);
                TechnicsRequestCommandPosition position = TechnicsRequestCommandPositionUtil.GetTechnicsRequestCommandPosition(CurrentUser, fillTechnicsRequest.TechnicsRequestCommandPositionID);
                Technics technics = TechnicsUtil.GetTechnics(fillTechnicsRequest.TechnicsID, CurrentUser);

                string logDescription = "";

                logDescription += "Заявка №: " + position.TechnicsRequestsCommand.EquipmentTechnicsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(position.TechnicsRequestsCommand.EquipmentTechnicsRequest.RequestDate) +
                                  "; Команда: " + position.TechnicsRequestsCommand.MilitaryCommand.DisplayTextForSelection +
                                  "; ВО: " + militaryDepartment.MilitaryDepartmentName +
                                  "; Вид резерв: " + fillTechnicsRequest.TechnicReadiness +
                                  "; Вид техника: " + position.TechnicsType.TypeName +
                                  "; Нормативна категория от заявка (код): " + (position.NormativeTechnics != null ? position.NormativeTechnics.NormativeCode : "") +
                                  "; Нормативна категория на техника (код): " + (technics.NormativeTechnics != null ? technics.NormativeTechnics.NormativeCode : "") +
                                  "; Коментар: " + position.Comment;

                ChangeEvent changeEvent = null;

                switch (technics.TechnicsType.TypeKey)
                {
                    case "VEHICLES":
                        Vehicle vehicle = VehicleUtil.GetVehicleByTechnicsId(technics.TechnicsId, CurrentUser);
                        logDescription += "; Рег. №: " + vehicle.RegNumber;
                        changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteVehicle", logDescription, position.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, CurrentUser);

                        break;
                    case "TRAILERS":
                        Trailer trailer = TrailerUtil.GetTrailerByTechnicsId(technics.TechnicsId, CurrentUser);
                        logDescription += "; Рег. №: " + trailer.RegNumber;
                        changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteTrailer", logDescription, position.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, CurrentUser);

                        break;
                    case "TRACTORS":
                        Tractor tractor = TractorUtil.GetTractorByTechnicsId(technics.TechnicsId, CurrentUser);
                        logDescription += "; Рег. №: " + tractor.RegNumber;
                        changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteTractor", logDescription, position.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, CurrentUser);

                        break;
                    case "ENG_EQUIP":
                        EngEquip engEquip = EngEquipUtil.GetEngEquipByTechnicsId(technics.TechnicsId, CurrentUser);
                        logDescription += "; Рег. №: " + engEquip.RegNumber;
                        changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteEngEquip", logDescription, position.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, CurrentUser);

                        break;
                    case "MOB_LIFT_EQUIP":
                        MobileLiftingEquip mobileLiftingEquip = MobileLiftingEquipUtil.GetMobileLiftingEquipByTechnicsId(technics.TechnicsId, CurrentUser);
                        logDescription += "; Рег. №: " + mobileLiftingEquip.RegNumber;
                        changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteMobLiftEquip", logDescription, position.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, CurrentUser);

                        break;
                    case "RAILWAY_EQUIP":
                        RailwayEquip railwayEquip = RailwayEquipUtil.GetRailwayEquipByTechnicsId(technics.TechnicsId, CurrentUser);
                        logDescription += "; Инв. №: " + railwayEquip.InventoryNumber;
                        changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteRailwayEquip", logDescription, position.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, CurrentUser);

                        break;
                    case "AVIATION_EQUIP":
                        AviationEquip aviationEquip = AviationEquipUtil.GetAviationEquipByTechnicsId(technics.TechnicsId, CurrentUser);
                        logDescription += "; Инв. №: " + aviationEquip.AirInvNumber;
                        changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteAviationEquip", logDescription, position.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, CurrentUser);

                        break;
                    case "VESSELS":
                        Vessel vessel = VesselUtil.GetVesselByTechnicsId(technics.TechnicsId, CurrentUser);
                        logDescription += "; Име: " + vessel.VesselName +
                                          "; Инв. №: " + vessel.InventoryNumber;
                        changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteVessel", logDescription, position.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, CurrentUser);

                        break;
                    case "FUEL_CONTAINERS":
                        FuelContainer fuelContainer = FuelContainerUtil.GetFuelContainerByTechnicsId(technics.TechnicsId, CurrentUser);
                        logDescription += "; Инв. №: " + fuelContainer.InventoryNumber;
                        changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteFuelContainer", logDescription, position.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, CurrentUser);

                        break;
                }

                FillTechnicsRequestUtil.DeleteRequestCommandTechnic(fillTechnicsRequest.FillTechnicsRequestID, CurrentUser, changeEvent);

                change.AddEvent(changeEvent);

                if (!deletedTechnics.Contains(technics.TechnicsId))
                {
                    //Change the current Military Reporting Status of the chosen technics
                    TechnicsMilRepStatusUtil.SetMilRepStatusTo_FREE(technics.TechnicsId, CurrentUser, change);

                    //Clear the Mobilization Appointment
                    TechnicsAppointmentUtil.ClearTheCurrentTechnicsAppointmentByTechnics(technics.TechnicsId, CurrentUser, change);

                    deletedTechnics.Add(technics.TechnicsId);
                }
            }


            bool result = TechnicsReqCmdPositionMilDeptUtil.DeleteMilitaryDepartmentFromTechnicsRequestCommandPositions(equipmentTechnicsRequestID, technicsRequestCommandID, militaryDepartmentID, CurrentUser, change);

            change.WriteLog();

            response = result ? AJAXTools.OK : AJAXTools.ERROR;

            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();
        }

        // saves all position by ajax request, if needed
        private void JSSaveCount()
        {
            string response = "";

            bool result = true;

            int equipmentTechnicsRequestID = int.Parse(Request.Form["EquipmentTechnicsRequestID"]);
            int positionsCount = int.Parse(Request.Params["PositionsCount"]);
            int militaryDepartmentsCount = int.Parse(Request.Params["MilitaryDepartmentsCount"]);
            int technicsRequestCommandID = int.Parse(Request.Params["TechnicsRequestCommandID"]);

            List<TechnicsRequestCommandPosition> oldPositions = TechnicsRequestCommandPositionUtil.GetTechnicsRequestCommandPositionsWithMilDepts(CurrentUser, technicsRequestCommandID);

            List<TechnicsRequestCommandPosition> newPositions = new List<TechnicsRequestCommandPosition>();

            for (int i = 1; i <= positionsCount; i++)
            {
                int technicsRequestCommandPositionID = int.Parse(Request.Params["TechnicsRequestCommandPositionID" + i.ToString()]);

                TechnicsRequestCommandPosition position = new TechnicsRequestCommandPosition(CurrentUser);
                position.TechnicsRequestCommandPositionId = technicsRequestCommandPositionID;
                position.TechnicsRequestsCommandId = technicsRequestCommandID;
                position.TechnicsPositionMilitaryDepartments = new List<TechnicsReqCmdPositionMilDept>();

                for (int j = 1; j <= militaryDepartmentsCount; j++)
                {
                    int techReqCommandPositionMilDeptID = int.Parse(Request.Params["TechReqCommandPositionMilDeptID" + i.ToString() + "_" + j.ToString()]);
                    int militaryDepartmentID = int.Parse(Request.Params["MilitaryDepartmentID" + i.ToString() + "_" + j.ToString()]);
                    int? count = !string.IsNullOrEmpty(Request.Params["Count" + i.ToString() + "_" + j.ToString()]) ? (int?)int.Parse(Request.Params["Count" + i.ToString() + "_" + j.ToString()]) : null;
                    int? driversCount = !string.IsNullOrEmpty(Request.Params["DriversCount" + i.ToString() + "_" + j.ToString()]) ? (int?)int.Parse(Request.Params["DriversCount" + i.ToString() + "_" + j.ToString()]) : null;

                    TechnicsReqCmdPositionMilDept milDept = new TechnicsReqCmdPositionMilDept(CurrentUser);
                    milDept.TechReqCommandPositionMilDeptID = techReqCommandPositionMilDeptID;
                    milDept.TechRequestCommandPositionID = technicsRequestCommandPositionID;
                    milDept.MilitaryDepartmentID = militaryDepartmentID;
                    milDept.Count = count;
                    milDept.DriversCount = driversCount;

                    position.TechnicsPositionMilitaryDepartments.Add(milDept);
                }

                newPositions.Add(position);
            }

            List<TechnicsRequestCommandPosition> newPositionsToSave = (from n in newPositions
                                                                       join o in oldPositions on n.TechnicsRequestCommandPositionId equals o.TechnicsRequestCommandPositionId
                                                                       where ((from m1 in n.TechnicsPositionMilitaryDepartments join m2 in o.TechnicsPositionMilitaryDepartments on m1.TechReqCommandPositionMilDeptID equals m2.TechReqCommandPositionMilDeptID where ((m1.Count != m2.Count) || (m1.DriversCount != m2.DriversCount))select m1).Count() > 0)
                                                                       select n).ToList();

            List<TechnicsRequestCommandPosition> oldPositionsToSave = (from o in oldPositions
                                                                       join n in newPositionsToSave on o.TechnicsRequestCommandPositionId equals n.TechnicsRequestCommandPositionId
                                                                       select o).ToList();

            if (newPositionsToSave.Count > 0)
            {
                Change change = new Change(CurrentUser, "RES_EquipmentTechnicsRequests");

                result = TechnicsReqCmdPositionMilDeptUtil.SaveMilitaryDepartmentsToTechRequestCommandPositions(equipmentTechnicsRequestID, oldPositionsToSave, newPositionsToSave, CurrentUser, change);

                change.WriteLog();
            }

            response = result ? AJAXTools.OK : AJAXTools.ERROR;

            AJAX a = new AJAX(AJAXTools.EncodeForXML(response), this.Response);
            a.Write();
            Response.End();
        }
    }
}
