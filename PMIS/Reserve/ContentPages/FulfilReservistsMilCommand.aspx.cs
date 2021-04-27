using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using PMIS.Common;
using PMIS.Reserve.Common;
using System.Text;

namespace PMIS.Reserve.ContentPages
{
    public partial class FulfilReservistsMilCommand : RESPage
    {
        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "RES_EQUIPRESREQUESTS_FULFIL";
            }
        }
      
        protected void Page_Load(object sender, EventArgs e)
        {
            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSavePunkt")
            {
                JSSavePunkt();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetViewFulfilmentLightBox")
            {
                JSGetViewFulfilmentLightBox();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteReservist")
            {
                JSDeleteReservist();
                return;
            }

            HighlightMenuItems("Equipment", "FulfilReservistsMilCommand");

            //Load the page for the first time: Load the data on the screen
            if (!IsPostBack)
            {
                PopulateMilitaryDepartment();

                if (!string.IsNullOrEmpty(Request.Params["MilitaryCommandID"]) &&
                    !string.IsNullOrEmpty(Request.Params["MilitaryDepartmentId"]))
                {
                    ddMilitaryDepartment.SelectedValue = Request.Params["MilitaryDepartmentId"];
                    ddMilitaryDepartment_Changed(ddMilitaryDepartment, new EventArgs());

                    RequestCommand reqCommand = RequestCommandUtil.GetRequestsCommand(CurrentUser, int.Parse(Request.Params["MilitaryCommandID"]));
                    ddUniqueCommands.SelectedValue = reqCommand.MilitaryCommand.MilitaryCommandId.ToString();
                    ddUniqueCommands_Changed(ddUniqueCommands, new EventArgs());

                    ddMilitaryCommand.SelectedValue = Request.Params["MilitaryCommandID"];
                    ddMilitaryCommand_Changed(ddMilitaryCommand, new EventArgs());
                }
                else
                {
                    LoadData();
                }

                SetPageHeader();
            }            

            SetupPageUI();
        }        

        //Set the correct page header (i.e. either New or Edit)
        private void SetPageHeader()
        {
            string header = "Комплектоване на команда с резервисти";

            lblHeaderTitle.InnerHtml = header;
            Page.Title = header;
        }

        private void PopulateRegions()
        {
            ddRegion.DataSource = RegionUtil.GetRegions(CurrentUser);
            ddRegion.DataTextField = "RegionName";
            ddRegion.DataValueField = "RegionId";
            ddRegion.DataBind();
            ddRegion.Items.Insert(0, ListItems.GetOptionChooseOne());
        }

        private void PopulateMunicipalities(int regionID)
        {
            //http://stackoverflow.com/questions/1381147/selectedvalue-which-is-invalid-because-it-does-not-exist-in-the-list-of-items
            ddMuniciplaity.SelectedValue = null;

            ddMuniciplaity.DataSource = MunicipalityUtil.GetMunicipalities(regionID, CurrentUser); ;
            ddMuniciplaity.DataTextField = "MunicipalityName";
            ddMuniciplaity.DataValueField = "MunicipalityId";
            ddMuniciplaity.DataBind();
            ddMuniciplaity.Items.Insert(0, ListItems.GetOptionChooseOne());
        }

        private void PopulateCities(int municipalityID)
        {
            ddCity.SelectedValue = null;

            ddCity.DataSource = CityUtil.GetCities(municipalityID, CurrentUser);
            ddCity.DataTextField = "CityName";
            ddCity.DataValueField = "CityId";
            ddCity.DataBind();
            ddCity.Items.Insert(0, ListItems.GetOptionChooseOne());
        }

        //Populate the MilitaryUnits drop-down
        private void PopulateMilitaryDepartment()
        {
            ddMilitaryDepartment.Items.Clear();

            List<MilitaryDepartment> militaryDepartments = MilitaryDepartmentUtil.GetAllMilitaryDepartments(CurrentUser);

            foreach (MilitaryDepartment militaryDepartment in militaryDepartments)
            {
                ListItem li = new ListItem();
                li.Text = militaryDepartment.MilitaryDepartmentName;
                li.Value = militaryDepartment.MilitaryDepartmentId.ToString();

                ddMilitaryDepartment.Items.Add(li);
            }

            if (militaryDepartments.Count == 0)
                ddMilitaryDepartment.Items.Insert(0, "");
        }

        private void PopulateUniqueCommands()
        {
            ddUniqueCommands.DataSource = MilitaryCommandUtil.GetMilitaryCommandsByMilitaryDepartmentForReservists(CurrentUser, ddMilitaryDepartment.SelectedValue);
            ddUniqueCommands.DataTextField = "DisplayTextForSelection";
            ddUniqueCommands.DataValueField = "MilitaryCommandId";
            ddUniqueCommands.DataBind();
            ddUniqueCommands.Items.Insert(0, ListItems.GetOptionChooseOne());

            if (ddUniqueCommands.Items.Count > 1)
                ddUniqueCommands.Items[1].Selected = true;
        }

        private void PopulateMilitaryCommands()
        {
            if (ddUniqueCommands.SelectedValue != "" && ddMilitaryDepartment.SelectedValue != "")
            {
                ddMilitaryCommand.DataSource = RequestCommandUtil.GetAllRequestCommandForMilCommandAndMilDept(CurrentUser, int.Parse(ddUniqueCommands.SelectedValue), int.Parse(ddMilitaryDepartment.SelectedValue));
                ddMilitaryCommand.DataTextField = "DisplayText2";
                ddMilitaryCommand.DataValueField = "RequestCommandId";
                ddMilitaryCommand.DataBind();
            }

            ddMilitaryCommand.Items.Insert(0, ListItems.GetOptionChooseOne());

            if (ddMilitaryCommand.Items.Count > 1)
                ddMilitaryCommand.Items[1].Selected = true;
        }

        //Load the existing data
        private void LoadData()
        {
            ddMilitaryDepartment_Changed(ddMilitaryDepartment, new EventArgs());
            
            //disable "punkt"-panel
            DisableControl(ddRegion);
            DisableControl(ddMuniciplaity);
            DisableControl(ddCity);
            DisableControl(txtPlace);
            btnImgSave.Style.Add("display", "none");
        }                  

        //Navigate to the previous page
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/");
        }

        // Setup user interface elements according to rights of the user's role
        private void SetupPageUI()
        {
            //Setup the client controls
            List<string> disabledClientControls = new List<string>();
            List<string> hiddenClientControls = new List<string>();

            bool screenHidden = false;
            bool screenDisabled = false;

            screenHidden = GetUIItemAccessLevel("RES_EQUIPRESREQUESTS") == UIAccessLevel.Hidden ||
                           GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL") == UIAccessLevel.Hidden;

            screenDisabled = GetUIItemAccessLevel("RES_EQUIPRESREQUESTS") == UIAccessLevel.Disabled ||
                             GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL") == UIAccessLevel.Disabled;

            if (screenHidden)
                RedirectAccessDenied();

            bool isRequestHidden = GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL_REQUEST") == UIAccessLevel.Hidden;

            UIAccessLevel l;

            bool isMilCommandHidden = GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL_MILCOMMAND") == UIAccessLevel.Hidden;

            l = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL_MILCOMMAND_TIME");
            if (l == UIAccessLevel.Hidden || isMilCommandHidden)
            {
                this.pageHiddenControls.Add(lblTime);
                this.pageHiddenControls.Add(txtTime);
            }

            l = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL_MILCOMMAND_READINESS");
            if (l == UIAccessLevel.Hidden || isMilCommandHidden)
            {
                this.pageHiddenControls.Add(lblReadiness);
                this.pageHiddenControls.Add(txtReadiness);
            }

            l = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL_MILCOMMAND_LOCATION");
            if (l == UIAccessLevel.Hidden || isMilCommandHidden)
            {
                this.pageHiddenControls.Add(lblDeliveryLocation);
                this.pageHiddenControls.Add(txtDeliveryLocation);
            }

            l = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL_MILCOMMAND_PUNKT");
            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                this.pageDisabledControls.Add(ddRegion);
                this.pageDisabledControls.Add(ddMuniciplaity);
                this.pageDisabledControls.Add(ddCity);
                this.pageDisabledControls.Add(txtPlace);
                hiddenClientControls.Add(btnImgSave.ClientID);
            }
            if (l == UIAccessLevel.Hidden)
            {
                this.pageHiddenControls.Add(lblRegion);
                this.pageHiddenControls.Add(lblMuniciplaity);
                this.pageHiddenControls.Add(lblCity);
                this.pageHiddenControls.Add(ddRegion);
                this.pageHiddenControls.Add(ddMuniciplaity);
                this.pageHiddenControls.Add(ddCity);
                this.pageHiddenControls.Add(txtPlace);
                hiddenClientControls.Add(btnImgSave.ClientID);                
            }           

            SetDisabledClientControls(disabledClientControls.ToArray());
            SetHiddenClientControls(hiddenClientControls.ToArray());
        }

        protected void ddMilitaryDepartment_Changed(object sender, EventArgs e)
        {
            PopulateUniqueCommands();
            ddUniqueCommands_Changed(ddMilitaryDepartment, new EventArgs());
        }

        protected void ddUniqueCommands_Changed(object sender, EventArgs e)
        {
            PopulateMilitaryCommands();
            ddMilitaryCommand_Changed(ddUniqueCommands, new EventArgs());
        }

        protected void ddMilitaryCommand_Changed(object sender, EventArgs e)
        {
            // clear "punkt"-panel
            ddRegion.Items.Clear();
            ddMuniciplaity.Items.Clear();
            ddCity.Items.Clear();
            txtPlace.Text = "";

            if (ddMilitaryCommand.SelectedValue != "-1")
            {
                int requestCommantId = int.Parse(ddMilitaryCommand.SelectedValue);

                PopulateRegions();

                //enable "punkt"-panel
                EnableControl(ddRegion);
                EnableControl(ddMuniciplaity);
                EnableControl(ddCity);
                EnableControl(txtPlace);
                btnImgSave.Style.Add("display", ""); 
                
                // load "punkt"-panel
                RequestCommandPunkt punkt = RequestCommandPunktUtil.GetRequestCommandPunkt(requestCommantId, int.Parse(ddMilitaryDepartment.SelectedValue), CurrentUser);

                if (punkt != null)
                {
                    if (punkt.City != null)
                    {
                        ddRegion.SelectedValue = punkt.City.RegionId.ToString();
                        PopulateMunicipalities(punkt.City.RegionId);
                        ddMuniciplaity.SelectedValue = punkt.City.MunicipalityId.ToString();
                        PopulateCities(punkt.City.MunicipalityId);
                        ddCity.SelectedValue = punkt.CityID.ToString();
                    }
                    txtPlace.Text = punkt.Place;
                }

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

                hfEquipmentReservistsRequestID.Value = command.EquipmentReservistsRequestId.ToString();

                List<RequestCommandPositionBlockForFulfilment> requestCommandPositions = RequestCommandPositionUtil.GetRequestCommandPositionsForFulfilment(CurrentUser, command.RequestCommandId, int.Parse(ddMilitaryDepartment.SelectedValue));

                pnlDataGrid.InnerHtml = GenerateTable(requestCommandPositions);
            }
            else
            {
                // clean old values
                txtDeliveryLocation.Text = "";
                txtTime.Text = "";
                txtReadiness.Text = "";
                pnlDataGrid.InnerHtml = "";              

                // disable "punkt"-panel
                DisableControl(ddRegion);
                DisableControl(ddMuniciplaity);
                DisableControl(ddCity);
                DisableControl(txtPlace);
                btnImgSave.Style.Add("display", "none");
            }
        }

        private string GenerateTable(List<RequestCommandPositionBlockForFulfilment> requestCommandPositions)
        {
            StringBuilder sb = new StringBuilder();

            bool IsTableHidden = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL_MILCOMMAND_POSITIONS") == UIAccessLevel.Hidden;
            bool IsPositionHidden = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL_MILCOMMAND_POSITIONS_POSITION") == UIAccessLevel.Hidden;
            bool IsMilRepSpecHidden = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL_MILCOMMAND_POSITIONS_MILREPSPEC") == UIAccessLevel.Hidden;
            bool IsRankHidden = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL_MILCOMMAND_POSITIONS_MILRANK") == UIAccessLevel.Hidden;
            bool IsReservistsHidden = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL_MILCOMMAND_POSITIONS_RESERVISTSCNT") == UIAccessLevel.Hidden;
            bool IsFulfilledHidden = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL_MILCOMMAND_POSITIONS_FULFIL") == UIAccessLevel.Hidden;

            bool IsScreenEnabled = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS") == UIAccessLevel.Enabled && 
                                   this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL") == UIAccessLevel.Enabled;
            bool IsTableEnabled = IsScreenEnabled && this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL_MILCOMMAND_POSITIONS") == UIAccessLevel.Enabled;
            bool IsFulfilledEnabled = IsTableEnabled && !IsFulfilledHidden && 
                                      (this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL_MILCOMMAND_POSITIONS_FULFIL") == UIAccessLevel.Enabled) &&
                                      (this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL_MILCOMMAND_POSITIONS_FULFIL_SEARCH") == UIAccessLevel.Enabled);

            if (IsTableHidden)
                return "";

            int tableWidth = 20;

            sb.Append(@"<div style=""width:100%; height: 30px; display: inline-box; text-align: right; position:relative;"">
                            <div style='position: absolute; right:100px;'>
                                <div id=""btnResFulfilmentRemoval"" style=""display: inline;"" 
                                     onclick=""resFulfilmentRemoval.showDialog('resFulfilmentRemoval_0', function(){document.getElementById(btnRefresh).click()}, document.getElementById(ddMilitaryCommand).value, document.getElementById(ddMilitaryDepartmentID).value);"" 
                                     class=""Button"">
                                        <i></i>
                                        <div id=""btnResFulfilmentRemovalText"" style=""width: 170px;"">
                                            Прекратяване на МН
                                        </div>
                                        <b></b>
                                </div>
                            </div>
                      </div>");

            sb.Append("<center>");
                        
            sb.Append("<table id='positionsTable' name='positionsTable' class='CommonHeaderTable' style='text-align: left;' border='1px'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style=\"min-width: 30px;\"></th>");
            tableWidth += 30;
            if (!IsPositionHidden)
            {
                sb.Append("<th style=\"min-width: 150px;\">Длъжност</th>");
                tableWidth += 150;
            }

            if (!IsMilRepSpecHidden)
            {
                sb.Append("<th style=\"min-width: 165px;\">ВОС</th>");
                tableWidth += 165;
            }

            if (!IsRankHidden)
            {
                sb.Append("<th style=\"min-width: 85px;\">Звание</th>");
                tableWidth += 85;
            }

            if (!IsReservistsHidden)
            {
                sb.Append("<th style=\"min-width: 65px;\">Заявени</th>");
                tableWidth += 65;
            }

            if (!IsFulfilledHidden)
            {
                sb.Append("<th style=\"min-width: 65px;\">Изпълнени</th>");
                tableWidth += 65;
            }

            if (IsFulfilledEnabled || !IsFulfilledHidden)
            {
                sb.Append("<th style=\"min-width: 60px;\"></th>");
                tableWidth += 60;
            }

            sb.Append("</tr>");
            sb.Append("</thead>");
            int counter = 0;

            if (requestCommandPositions.Count > 0)
            {
                sb.Append("<tbody>");
            }

            int totalReservistsCount = 0;
            int totalFulfiled = 0;
            int totalFulfiledReserve = 0;

            foreach (RequestCommandPositionBlockForFulfilment requestCommandPosition in requestCommandPositions)
            {
                counter++;

                string fulfilHTML = "";

                if (IsFulfilledEnabled)
                    fulfilHTML = "<img src='../Images/user_add.png' alt='Добавяне на основно попълнение' title='Добавяне на основно попълнение' class='GridActionIcon' onclick='AddFulfilment(" + requestCommandPosition.RequestCommandPositionId + @");' />";

                string fulfilReserveHTML = "";

                if (IsFulfilledEnabled)
                    fulfilReserveHTML = "<img src='../Images/user_back.png' alt='Добавяне на допълващ резерв' title='Добавяне на допълващ резерв' class='GridActionIcon' onclick='AddFulfilmentReserve(" + requestCommandPosition.RequestCommandPositionId + @");' />";

                string viewFulfilmentHTML = "";

                if (!IsFulfilledHidden)
                    viewFulfilmentHTML = "<img src='../Images/user_view.png' alt='Преглед' title='Преглед' class='GridActionIcon' onclick='ShowViewFulfilmentLightBox(" + requestCommandPosition.RequestCommandPositionId + @");' />";

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
                if (!IsFulfilledHidden)
                    sb.Append("<td>" + requestCommandPosition.Fulfiled + " (" + requestCommandPosition.FulfiledReserve + ")" + "</td>");

                if (IsFulfilledEnabled || !IsFulfilledHidden)
                    sb.Append("<td>" + fulfilHTML + fulfilReserveHTML + viewFulfilmentHTML + "</td>");
               
                sb.Append("</tr>");

                totalReservistsCount += requestCommandPosition.ReservistsCount;
                totalFulfiled += requestCommandPosition.Fulfiled;
                totalFulfiledReserve += requestCommandPosition.FulfiledReserve;
            }

            if (requestCommandPositions.Count > 0)
            {
                sb.Append("</tbody>");
            }

            sb.Append("</table>");

            if (requestCommandPositions.Count > 0)
            {
                string totalFulfiledPercStr = "";
                string totalFulfiledReservePercStr = "";

                if (totalReservistsCount > 0)
                {
                    decimal totalFulfiledPerc = (decimal)totalFulfiled / (decimal)totalReservistsCount * (decimal)100;
                    decimal totalFulfiledReservePerc = (decimal)totalFulfiledReserve / (decimal)totalReservistsCount * (decimal)100;

                    totalFulfiledPercStr = " (" + totalFulfiledPerc.ToString("0.0") + "%)";
                    totalFulfiledReservePercStr = " (" + totalFulfiledReservePerc.ToString("0.0") + "%)";
                }

                sb.Append(@"
<div style='text-align: right; margin: 0 auto; width: " + tableWidth.ToString() + @"px; padding: 10px;'>
   <span class='InputField' style='text-style: italic;'>Общо заявени:</span>
   <span class='ReadOnlyValue' style='width: 100px; display: inline-block; text-align: left;'>" + totalReservistsCount.ToString() + @"</span> <br />
   <span class='InputField' style='text-style: italic;'>Общо '" + ReadinessUtil.ReadinessName(1) + @"':</span>
   <span class='ReadOnlyValue' style='width: 100px; display: inline-block; text-align: left;'>" + totalFulfiled.ToString() + totalFulfiledPercStr + @"</span> <br />
   <span class='InputField' style='text-style: italic;'>Общо '" + ReadinessUtil.ReadinessName(2) + @"':</span>
   <span class='ReadOnlyValue' style='width: 100px; display: inline-block; text-align: left;'>" + totalFulfiledReserve.ToString() + totalFulfiledReservePercStr + @"</span>
</div>");
            }

            sb.Append("</center>");           

            return sb.ToString();
        }

        protected void ddRegion_Changed(object sender, EventArgs e)
        {
            ddMuniciplaity.Items.Clear();
            ddCity.Items.Clear();

            if (ddRegion.SelectedValue != "-1")
            {
                int regionId = int.Parse(ddRegion.SelectedValue);

                PopulateMunicipalities(regionId);
            }           
        }

        protected void ddMuniciplaity_Changed(object sender, EventArgs e)
        {
            ddCity.Items.Clear();

            if (ddMuniciplaity.SelectedValue != "-1")
            {
                int municiplaityId = int.Parse(ddMuniciplaity.SelectedValue);

                PopulateCities(municiplaityId);
            }           
        }

        private void JSSavePunkt()
        {
            string stat = "";
            string response = "";

            int militaryCommandID = int.Parse(Request.Form["MilitaryCommandID"]);
            int militaryDepartmentID = int.Parse(Request.Form["MilitaryDepartmentID"]);
            int? cityID = !string.IsNullOrEmpty(Request.Form["CityID"]) && Request.Form["CityID"] != "-1" ? (int?)int.Parse(Request.Form["CityID"]) : null;
            string place = Request.Form["Place"];

            RequestCommandPunkt punkt = new RequestCommandPunkt(CurrentUser);
            punkt.RequestCommandID = militaryCommandID;
            punkt.MilitaryDepartmentID = militaryDepartmentID;
            punkt.CityID = cityID;
            punkt.Place = place;

            try
            {
                Change change = new Change(CurrentUser, "RES_EquipmentReservistsRequests");

                RequestCommandPunktUtil.SaveRequestCommandPunkt(punkt, CurrentUser, change);

                change.WriteLog();

                stat = AJAXTools.OK;
                response = "<status>OK</status>";
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

        private void JSGetViewFulfilmentLightBox()
        {
            int requestCommandPositionID = int.Parse(Request.Form["RequestCommandPositionID"]);

            string response = "";
            response += GetViewFulfilmentLightBoxHtml(requestCommandPositionID);

            AJAX a = new AJAX(AJAXTools.EncodeForXML(response), this.Response);
            a.Write();
            Response.End();
        }

        private void JSDeleteReservist()
        {
            string stat = "";
            string response = "";

            int fillReservistsRequestID = int.Parse(Request.Form["FillReservistsRequestID"]);
            int militaryDepartmentID = int.Parse(Request.Form["MilitaryDepartmentID"]);                      

            try
            {
                FillReservistsRequest fillReservistsRequest = FillReservistsRequestUtil.GetFillReservistsRequest(fillReservistsRequestID, CurrentUser);
                int reservistId = fillReservistsRequest.ReservistID;

                Change change = new Change(CurrentUser, "RES_EquipmentReservistsRequests");

                FillReservistsRequestUtil.DeleteRequestCommandReservist(fillReservistsRequestID, militaryDepartmentID, CurrentUser, change);

                //Change the current Military Reporting Status of the chosen reservist
                ReservistMilRepStatusUtil.SetMilRepStatusTo_FREE(reservistId, CurrentUser, change);

                //Clear the current reservist appointment
                ReservistAppointmentUtil.ClearTheCurrentReservistAppointmentByReservist(reservistId, CurrentUser, change);

                change.WriteLog();

                stat = AJAXTools.OK;
                response = "<status>OK</status>";
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

        // Generate contents for view fulfilment light box
        private string GetViewFulfilmentLightBoxHtml(int requestCommandPositionID)
        {
            bool IsEditable = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL_MILCOMMAND_POSITIONS_FULFIL") == UIAccessLevel.Enabled;            

            string html = "";

            string htmlNoResults = "";           

            List<FillReservistsRequest> fillReservistsRequests = new List<FillReservistsRequest>();           
            int pageIndex = 1; //Default
            int pageLength = int.Parse(Config.GetWebSetting("RowsPerPage"));
            int allRows = 0;
            int maxPage = 1;
            int orderBy = 1; //Default

            int militaryDepartmentID = int.Parse(Request.Params["MilitaryDepartmentID"]);

            if (Request.Params["PageIndex"] != null && Request.Params["OrderBy"] != null)
            {               
                pageIndex = int.Parse(Request.Params["PageIndex"]);
                orderBy = int.Parse(Request.Params["OrderBy"]);
            }

            allRows = FillReservistsRequestUtil.GetAllFillReservistsRequestByRequestCommandPositionCount(requestCommandPositionID, militaryDepartmentID, CurrentUser);
            maxPage = allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            fillReservistsRequests = FillReservistsRequestUtil.GetAllFillReservistsRequestByRequestCommandPosition(requestCommandPositionID, militaryDepartmentID, orderBy, pageIndex, pageLength, CurrentUser);            

            // No data found
            if (fillReservistsRequests.Count == 0)
            {
                htmlNoResults = "Няма намерени резултати";
            }
           
            //Set pagination section
            // Refresh the paging image buttons
            string btnFirst = "src='../Images/ButtonFirst.png'";
            string btnPrev = "src='../Images/ButtonPrev.png'";
            string btnNext = "src='../Images/ButtonNext.png'";
            string btnLast = "src='../Images/ButtonLast.png'";

            if (pageIndex == 1)
            {
                btnFirst = "src='../Images/ButtonFirstDisabled.png' disabled='true'";
                btnPrev = "src='../Images/ButtonPrevDisabled.png' disabled='true'";
            }

            if (pageIndex == maxPage)
            {
                btnLast = "src='../Images/ButtonLastDisabled.png' disabled='true'";
                btnNext = "src='../Images/ButtonNextDisabled.png' disabled='true'";
            }

            // Set current page number
            string pageTablePagination = " | " + pageIndex + " от " + maxPage + " | ";

            // Setup the header of the grid
            html += @"<div style='min-height: 150px; margin-bottom: 10px;'>

                        <input type='hidden' id='hdnOrderBy' value='" + orderBy + @"' />
                        <input type='hidden' id='hdnPageIndex' value='" + pageIndex + @"' />
                        <input type='hidden' id='hdnPageMaxPage' value='" + maxPage + @"' />
                        <input type='hidden' id='hdnRequestCommandPositionID' value='" + requestCommandPositionID + @"' />

                        <span class='HeaderText'>Резервисти</span><br /><br /><br />

                        <div style='text-align: center;'>
                           <div style='display: inline; position: relative; top: -10px;'>
                              <img id='btnTableFirst' " + btnFirst + @" alt='Първа страница' title='Първа страница' class='PaginationButton' onclick=""BtnPagingClick('btnFirst');"" />
                              <img id='btnTablePrev' " + btnPrev + @" alt='Предишна страница' title='Предишна страница' class='PaginationButton' onclick=""BtnPagingClick('btnPrev');"" />
                              <span id='lblTablePagination' class='PaginationLabel'>" + pageTablePagination + @"</span>
                              <img id='btnTableNext' " + btnNext + @" alt='Следваща страница' title='Следваща страница' class='PaginationButton' onclick=""BtnPagingClick('btnNext');"" />
                              <img id='btnTableLast' " + btnLast + @" alt='Последна страница' title='Последна страница' class='PaginationButton' onclick=""BtnPagingClick('btnLast');"" />
                              
                              <span style='padding: 0 30px'>&nbsp;</span>
                              <span style='text-align: right;'>Отиди на страница</span>
                              <input id='txtTableGotoPage' class='InputField' type='text' style='width: 30px;' value='' />
                              <img id='btnTableGoto' src='../Images/ButtonGoto.png' alt='Отиди на страница' title='Отиди на страница' class='PaginationButton' onclick=""BtnPagingClick('btnPageGo');"" />
                           </div>
                        </div>

                        <table id='tblReservists' class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            
                         </thead>";

            //Set Table Results
            string headerStyle = "vertical-align: bottom;";
            int orderCol = (orderBy > 100 ? orderBy - 100 : orderBy);
            string img = orderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
            string[] arrOrderCol = { "", "", "", "", "", "" };

            arrOrderCol[orderCol - 1] = @"<div style='position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

            //Setup the header of the grid
            html += @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='width: 20px;" + headerStyle + @"'></th>
                               <th style='width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>ЕГН" + arrOrderCol[0] + @"</th>
                               <th style='width: 210px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>Име" + arrOrderCol[1] + @"</th>
                               <th style='width: 270px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>Назначен на ВОС" + arrOrderCol[2] + @"</th>
                               <th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(4);'>Вид резерв" + arrOrderCol[3] + @"</th>
                               <th style='width: 80px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(5);'>Връчено МН" + arrOrderCol[4] + @"</th>" +
               (IsEditable ? @"<th style='width: 30px;" + headerStyle + @"'>&nbsp;</th>" : "" ) + @"
                            </tr></thead>";

            int counter = 1;

            //Iterate through all items and add them into the grid
            foreach (FillReservistsRequest fillReservistsRequest in fillReservistsRequests)
            {
                string deleteHTML = "";

                if (IsEditable)
                    deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на този резервист' class='GridActionIcon' style='float: right; margin-right: 0px;' onclick='DeleteReservist(" + fillReservistsRequest.FillReservistsRequestID.ToString() + "," + militaryDepartmentID.ToString() + ");' />";

                string cellStyle = "vertical-align: top;";

                string milReportingSpeciality = "";

                if (fillReservistsRequest.MilitaryReportSpeciality != null)
                    milReportingSpeciality = fillReservistsRequest.MilitaryReportSpeciality.CodeAndName;

                if (milReportingSpeciality.Length > 35)
                {
                    milReportingSpeciality = "<span title='" + milReportingSpeciality + "'>" + milReportingSpeciality.Substring(0, 35) + "...</span>";
                }

                html += @"<tr  class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"' >
                                 <td style='" + cellStyle + @"'>" + ((pageIndex - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'><img src='../Images/user.png' alt='Преглед' title='Преглед' class='GridActionIcon' onclick='PreviewReservist(" + fillReservistsRequest.Reservist.ReservistId.ToString() + @");' /></td>
                                 <td style='" + cellStyle + @"'>" + fillReservistsRequest.Reservist.Person.IdentNumber + @"</td>
                                 <td style='" + cellStyle + @"'>" + fillReservistsRequest.Reservist.Person.FullName + @"</td>
                                 <td style='" + cellStyle + @"'>" + milReportingSpeciality + @"</td>
                                 <td style='" + cellStyle + @"'>" + fillReservistsRequest.ReservistReadiness + @"</td>
                                 <td style='" + cellStyle + @" text-align: center;'>" + (fillReservistsRequest.AppointmentIsDelivered ? ListItems.GetOptionYes().Text : ListItems.GetOptionNo().Text) + @"</td>" +
                (IsEditable ? @"<td style='" + cellStyle + @"'>" + deleteHTML + @"</td>" : "") +
                               @"
                              </tr>";

                counter++;
            }
            html += "</table>";

            html += @"<div style='height: 10px;'>
                </div>

                 <div style='text-align: center'>
                   <span id='lblViewFulfilmentMessage'>" + htmlNoResults + @"</span><br/>
                </div>";

            html += @"  
                        </div>                        
                        <div id='btnCloseTable' runat='server' class='Button' onclick=""HideViewFulfilmentLightBox();""><i></i><div style='width:70px; padding-left:5px;'>Затвори</div><b></b></div>
                      </center>";

            return html;
        }
    }
}