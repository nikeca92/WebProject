using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using PMIS.Common;
using PMIS.Reserve.Common;
using System.Text;

namespace PMIS.Reserve.ContentPages
{
    public partial class FulfilTechnicsMilCommand : RESPage
    {
        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "RES_EQUIPTECHREQUESTS_FULFIL";
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
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteVehicle")
            {
                JSDeleteVehicle();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteTrailer")
            {
                JSDeleteTrailer();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteMobileLiftingEquip")
            {
                JSDeleteMobileLiftingEquip();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteTractor")
            {
                JSDeleteTractor();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteEngEquip")
            {
                JSDeleteEngEquip();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteRailwayEquip")
            {
                JSDeleteRailwayEquip();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteAviationEquip")
            {
                JSDeleteAviationEquip();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteVessel")
            {
                JSDeleteVessel();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteFuelContainer")
            {
                JSDeleteFuelContainer();
                return;
            }

            HighlightMenuItems("Equipment", "FulfilTechnicsMilCommand");

            //Load the page for the first time: Load the data on the screen
            if (!IsPostBack)
            {
                PopulateMilitaryDepartment();

                if (!string.IsNullOrEmpty(Request.Params["MilitaryCommandID"]) &&
                    !string.IsNullOrEmpty(Request.Params["MilitaryDepartmentId"]))
                {
                    ddMilitaryDepartment.SelectedValue = Request.Params["MilitaryDepartmentId"];
                    ddMilitaryDepartment_Changed(ddMilitaryDepartment, new EventArgs());

                    TechnicsRequestCommand techReqCommand = TechnicsRequestCommandUtil.GetTechnicsRequestCommand(CurrentUser, int.Parse(Request.Params["MilitaryCommandID"]));
                    ddUniqueCommands.SelectedValue = techReqCommand.MilitaryCommand.MilitaryCommandId.ToString();
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
            string header = "Комплектоване на команда с техника";

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
            ddUniqueCommands.DataSource = MilitaryCommandUtil.GetMilitaryCommandsByMilitaryDepartmentForTechnics(CurrentUser, ddMilitaryDepartment.SelectedValue);
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
                ddMilitaryCommand.DataSource = TechnicsRequestCommandUtil.GetAllTechnicsRequestCommandsForMilCommandAndMilDept(CurrentUser, int.Parse(ddUniqueCommands.SelectedValue), int.Parse(ddMilitaryDepartment.SelectedValue));
                ddMilitaryCommand.DataTextField = "DisplayText2";
                ddMilitaryCommand.DataValueField = "TechnicsRequestCommandId";
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

            screenHidden = GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS") == UIAccessLevel.Hidden ||
                           GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL") == UIAccessLevel.Hidden;

            screenDisabled = GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS") == UIAccessLevel.Disabled ||
                             GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL") == UIAccessLevel.Disabled;

            if (screenHidden)
                RedirectAccessDenied();

            bool isRequestHidden = GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL_REQUEST") == UIAccessLevel.Hidden;

            UIAccessLevel l;

            bool isMilCommandHidden = GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL_MILCOMMAND") == UIAccessLevel.Hidden;

            l = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL_MILCOMMAND_TIME");
            if (l == UIAccessLevel.Hidden || isMilCommandHidden)
            {
                this.pageHiddenControls.Add(lblTime);
                this.pageHiddenControls.Add(txtTime);
            }

            l = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL_MILCOMMAND_READINESS");
            if (l == UIAccessLevel.Hidden || isMilCommandHidden)
            {
                this.pageHiddenControls.Add(lblReadiness);
                this.pageHiddenControls.Add(txtReadiness);
            }

            l = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL_MILCOMMAND_LOCATION");
            if (l == UIAccessLevel.Hidden || isMilCommandHidden)
            {
                this.pageHiddenControls.Add(lblDeliveryLocation);
                this.pageHiddenControls.Add(txtDeliveryLocation);
            }

            l = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL_MILCOMMAND_PUNKT");
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
                TechnicsRequestCommandPunkt punkt = TechnicsRequestCommandPunktUtil.GetTechnicsRequestCommandPunkt(requestCommantId, int.Parse(ddMilitaryDepartment.SelectedValue), CurrentUser);

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

                TechnicsRequestCommand command = TechnicsRequestCommandUtil.GetTechnicsRequestCommand(CurrentUser, requestCommantId);

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

                hfEquipmentTechnicsRequestID.Value = command.EquipmentTechnicsRequestId.ToString();

                List<TechnicsRequestCommandPositionBlockForFulfilment> requestCommandPositions = TechnicsRequestCommandPositionUtil.GetTechnicsRequestCommandPositionsForFulfilment(CurrentUser, command.TechnicsRequestCommandId, int.Parse(ddMilitaryDepartment.SelectedValue));

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

        private string GenerateTable(List<TechnicsRequestCommandPositionBlockForFulfilment> requestCommandPositions)
        {
            StringBuilder sb = new StringBuilder();

            bool IsTableHidden = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL_MILCOMMAND_POSITIONS") == UIAccessLevel.Hidden;
            bool IsTechnicsTypeHidden = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL_MILCOMMAND_POSITIONS_TECHNICSTYPE") == UIAccessLevel.Hidden;
            bool IsNormativeTechnicsHidden = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL_MILCOMMAND_POSITIONS_NORMATIVETECHNICS") == UIAccessLevel.Hidden;
            bool IsTCommentHidden = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL_MILCOMMAND_POSITIONS_TCOMMENT") == UIAccessLevel.Hidden;            
            bool IsTechnicsCntHidden = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL_MILCOMMAND_POSITIONS_TECHNICSCNT") == UIAccessLevel.Hidden;
            bool IsFulfilledHidden = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL_MILCOMMAND_POSITIONS_FULFIL") == UIAccessLevel.Hidden;

            bool IsScreenEnabled = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS") == UIAccessLevel.Enabled && 
                                   this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL") == UIAccessLevel.Enabled;
            bool IsTableEnabled = IsScreenEnabled && this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL_MILCOMMAND_POSITIONS") == UIAccessLevel.Enabled;
            bool IsFulfilledEnabled = IsTableEnabled && !IsFulfilledHidden && 
                                      (this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL_MILCOMMAND_POSITIONS_FULFIL") == UIAccessLevel.Enabled) &&
                                      (this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL_MILCOMMAND_POSITIONS_FULFIL_SEARCH") == UIAccessLevel.Enabled);

            if (IsTableHidden)
                return "";

            int tableWidth = 20;

            sb.Append(@"<div style=""width:100%; height: 30px; display: inline-box; text-align: right; position:relative;"">
                            <div style='position: absolute; right:100px;'>
                                <div id=""btnTechFulfilmentRemoval"" style=""display: inline;"" 
                                     onclick=""techFulfilmentRemoval.showDialog('techFulfilmentRemoval_0', function(){document.getElementById(btnRefresh).click()}, document.getElementById(ddMilitaryCommand).value, document.getElementById(ddMilitaryDepartmentID).value);"" 
                                     class=""Button"">
                                        <i></i>
                                        <div id=""btnTechFulfilmentRemovalText"" style=""width: 170px;"">
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
            if (!IsTechnicsTypeHidden)
            {
                sb.Append("<th style=\"min-width: 120px; width: 120px;\">Вид техника</th>");
                tableWidth += 120;
            }

            if (!IsNormativeTechnicsHidden)
            {
                sb.Append("<th style=\"min-width: 220px; width: 220px;\">Нормативна категория</th>");
                tableWidth += 220;
            }

            if (!IsTCommentHidden)
            {
                sb.Append("<th style=\"min-width: 95px;\">Коментар</th>");
                tableWidth += 95;
            }

            if (!IsTechnicsCntHidden)
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

            int totalTechnicsCount = 0;
            int totalFulfiled = 0;
            int totalFulfiledReserve = 0;

            foreach (TechnicsRequestCommandPositionBlockForFulfilment requestCommandPosition in requestCommandPositions)
            {
                counter++;

                string pageForFulfilment = "SearchTechnics_" + requestCommandPosition.TechnicsTypeKey;            

                string fulfilHTML = "";

                if (IsFulfilledEnabled)
                    fulfilHTML = @"<img src='../Images/pawn_add.png' alt='Добавяне на основно попълнение' title='Добавяне на основно попълнение' class='GridActionIcon' onclick=""AddFulfilment('" + pageForFulfilment + "'," + requestCommandPosition.TechnicsRequestCommandPositionId + @");"" />";

                string fulfilReserveHTML = "";

                if (IsFulfilledEnabled)
                    fulfilReserveHTML = @"<img src='../Images/pawn_preferences.png' alt='Добавяне на допълващ резерв' title='Добавяне на допълващ резерв' class='GridActionIcon' onclick=""AddFulfilmentReserve('" + pageForFulfilment + "'," + requestCommandPosition.TechnicsRequestCommandPositionId + @");"" />";

                string viewFulfilmentHTML = "";

                if (!IsFulfilledHidden)
                    viewFulfilmentHTML = "<img src='../Images/pawn_view.png' alt='Преглед' title='Преглед' class='GridActionIcon' onclick='ShowViewFulfilmentLightBox(" + requestCommandPosition.TechnicsRequestCommandPositionId + @");' />";

                sb.Append("<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + "'>");
                sb.Append("<td>" + counter + "</td>");
                if (!IsTechnicsTypeHidden)
                    sb.Append("<td>" + requestCommandPosition.TechnicsType + "</td>");
                if (!IsNormativeTechnicsHidden)
                    sb.Append("<td>" + (requestCommandPosition.NormativeTechnics != null ? requestCommandPosition.NormativeTechnics.CodeAndText : "") + "</td>");
                if (!IsTCommentHidden)
                    sb.Append("<td>" + requestCommandPosition.TechnicsComment + "</td>");              
                if (!IsTechnicsCntHidden)
                    sb.Append("<td>" + requestCommandPosition.Count + "</td>");
                if (!IsFulfilledHidden)
                    sb.Append("<td>" + requestCommandPosition.Fulfiled + " (" + requestCommandPosition.FulfiledReserve + ")" + "</td>");

                if (IsFulfilledEnabled || !IsFulfilledHidden)
                    sb.Append("<td>" + fulfilHTML + fulfilReserveHTML + viewFulfilmentHTML + "</td>");
               
                sb.Append("</tr>");

                totalTechnicsCount += requestCommandPosition.Count;
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

                if (totalTechnicsCount > 0)
                {
                    decimal totalFulfiledPerc = (decimal)totalFulfiled / (decimal)totalTechnicsCount * (decimal)100;
                    decimal totalFulfiledReservePerc = (decimal)totalFulfiledReserve / (decimal)totalTechnicsCount * (decimal)100;

                    totalFulfiledPercStr = " (" + totalFulfiledPerc.ToString("0.0") + "%)";
                    totalFulfiledReservePercStr = " (" + totalFulfiledReservePerc.ToString("0.0") + "%)";
                }

                sb.Append(@"
<div style='text-align: right; margin: 0 auto; width: " + tableWidth.ToString() + @"px; padding: 10px;'>
   <span class='InputField' style='text-style: italic;'>Общо заявени:</span>
   <span class='ReadOnlyValue' style='width: 100px; display: inline-block; text-align: left;'>" + totalTechnicsCount.ToString() + @"</span> <br />
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

            TechnicsRequestCommandPunkt punkt = new TechnicsRequestCommandPunkt(CurrentUser);
            punkt.TechnicsRequestCommandID = militaryCommandID;
            punkt.MilitaryDepartmentID = militaryDepartmentID;
            punkt.CityID = cityID;
            punkt.Place = place;

            try
            {
                Change change = new Change(CurrentUser, "RES_EquipmentTechnicsRequests");

                TechnicsRequestCommandPunktUtil.SaveTechnicsRequestCommandPunkt(punkt, CurrentUser, change);

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
            int technicsRequestCommandPositionID = int.Parse(Request.Form["TechnicsRequestCommandPositionID"]);

            TechnicsRequestCommandPosition position = TechnicsRequestCommandPositionUtil.GetTechnicsRequestCommandPosition(CurrentUser, technicsRequestCommandPositionID);

            string response = "";

            if (position != null && position.TechnicsType != null)
            {
                switch (position.TechnicsType.TypeKey)
                {
                    case "VEHICLES":
                        response += GetViewFulfilmentVehiclesLightBoxHtml(technicsRequestCommandPositionID);
                        break;
                    case "TRAILERS":
                        response += GetViewFulfilmentTrailersLightBoxHtml(technicsRequestCommandPositionID);
                        break;
                    case "TRACTORS":
                        response += GetViewFulfilmentTractorsLightBoxHtml(technicsRequestCommandPositionID);
                        break;
                    case "ENG_EQUIP":
                        response += GetViewFulfilmentEngEquipLightBoxHtml(technicsRequestCommandPositionID);
                        break;
                    case "MOB_LIFT_EQUIP":
                        response += GetViewFulfilmentMobileLiftingEquipLightBoxHtml(technicsRequestCommandPositionID);
                        break;
                    case "RAILWAY_EQUIP":
                        response += GetViewFulfilmentRailwayEquipLightBoxHtml(technicsRequestCommandPositionID);
                        break;
                    case "AVIATION_EQUIP":
                        response += GetViewFulfilmentAviationEquipLightBoxHtml(technicsRequestCommandPositionID);
                        break;
                    case "VESSELS":
                        response += GetViewFulfilmentVesselsLightBoxHtml(technicsRequestCommandPositionID);
                        break;
                    case "FUEL_CONTAINERS":
                        response += GetViewFulfilmentFuelContainerLightBoxHtml(technicsRequestCommandPositionID);
                        break;
                }
                
            }

            AJAX a = new AJAX(AJAXTools.EncodeForXML(response), this.Response);
            a.Write();
            Response.End();
        }

        private void JSDeleteVehicle()
        {
            string stat = "";
            string response = "";

            int fulfilTechnicsRequestID = int.Parse(Request.Form["FulfilTechnicsRequestID"]);
            int militaryDepartmentID = int.Parse(Request.Form["MilitaryDepartmentID"]);                      

            try
            {
                Change change = new Change(CurrentUser, "RES_Technics_VEHICLES");

                MilitaryDepartment militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment(militaryDepartmentID, CurrentUser);

                FillTechnicsRequest request = FillTechnicsRequestUtil.GetFillTechnicsRequest(fulfilTechnicsRequestID, CurrentUser);

                TechnicsRequestCommandPosition position = TechnicsRequestCommandPositionUtil.GetTechnicsRequestCommandPosition(CurrentUser, request.TechnicsRequestCommandPositionID);

                Vehicle vehicle = VehicleUtil.GetVehicleByTechnicsId(request.TechnicsID, CurrentUser);

                string logDescription = "";
                logDescription += "Заявка №: " + position.TechnicsRequestsCommand.EquipmentTechnicsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(position.TechnicsRequestsCommand.EquipmentTechnicsRequest.RequestDate) +
                                  "; Команда: " + position.TechnicsRequestsCommand.MilitaryCommand.DisplayTextForSelection +
                                  "; ВО: " + militaryDepartment.MilitaryDepartmentName +
                                  "; Вид резерв: " + request.TechnicReadiness +
                                  "; Вид техника: " + position.TechnicsType.TypeName +
                                  "; Нормативна категория (код): " + (position.NormativeTechnics != null ? position.NormativeTechnics.NormativeCode : "") +
                                  "; Коментар: " + position.Comment +
                                  "; Рег. №: " + vehicle.RegNumber;

                ChangeEvent changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteVehicle", logDescription, position.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, CurrentUser);

                FillTechnicsRequestUtil.DeleteRequestCommandTechnic(fulfilTechnicsRequestID, CurrentUser, changeEvent);

                //Change the current Military Reporting Status of the chosen technics
                TechnicsMilRepStatusUtil.SetMilRepStatusTo_FREE(request.TechnicsID, CurrentUser, change);

                //Clear the current technics appointment
                TechnicsAppointmentUtil.ClearTheCurrentTechnicsAppointmentByTechnics(request.TechnicsID, CurrentUser, change);

                change.AddEvent(changeEvent);

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

        private void JSDeleteTrailer()
        {
            string stat = "";
            string response = "";

            int fulfilTechnicsRequestID = int.Parse(Request.Form["FulfilTechnicsRequestID"]);
            int militaryDepartmentID = int.Parse(Request.Form["MilitaryDepartmentID"]);

            try
            {
                Change change = new Change(CurrentUser, "RES_Technics_TRAILERS");

                MilitaryDepartment militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment(militaryDepartmentID, CurrentUser);

                FillTechnicsRequest request = FillTechnicsRequestUtil.GetFillTechnicsRequest(fulfilTechnicsRequestID, CurrentUser);

                TechnicsRequestCommandPosition position = TechnicsRequestCommandPositionUtil.GetTechnicsRequestCommandPosition(CurrentUser, request.TechnicsRequestCommandPositionID);

                Trailer trailer = TrailerUtil.GetTrailerByTechnicsId(request.TechnicsID, CurrentUser);

                string logDescription = "";
                logDescription += "Заявка №: " + position.TechnicsRequestsCommand.EquipmentTechnicsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(position.TechnicsRequestsCommand.EquipmentTechnicsRequest.RequestDate) +
                                  "; Команда: " + position.TechnicsRequestsCommand.MilitaryCommand.DisplayTextForSelection +
                                  "; ВО: " + militaryDepartment.MilitaryDepartmentName +
                                  "; Вид резерв: " + request.TechnicReadiness +
                                  "; Вид техника: " + position.TechnicsType.TypeName +
                                  "; Нормативна категория (код): " + (position.NormativeTechnics != null ? position.NormativeTechnics.NormativeCode : "") +
                                  "; Коментар: " + position.Comment +
                                  "; Рег. №: " + trailer.RegNumber;

                ChangeEvent changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteTrailer", logDescription, position.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, CurrentUser);

                FillTechnicsRequestUtil.DeleteRequestCommandTechnic(fulfilTechnicsRequestID, CurrentUser, changeEvent);

                //Change the current Military Reporting Status of the chosen technics
                TechnicsMilRepStatusUtil.SetMilRepStatusTo_FREE(request.TechnicsID, CurrentUser, change);

                //Clear the current technics appointment
                TechnicsAppointmentUtil.ClearTheCurrentTechnicsAppointmentByTechnics(request.TechnicsID, CurrentUser, change);

                change.AddEvent(changeEvent);

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

        private void JSDeleteMobileLiftingEquip()
        {
            string stat = "";
            string response = "";

            int fulfilTechnicsRequestID = int.Parse(Request.Form["FulfilTechnicsRequestID"]);
            int militaryDepartmentID = int.Parse(Request.Form["MilitaryDepartmentID"]);

            try
            {
                Change change = new Change(CurrentUser, "RES_Technics_MOB_LIFT_EQUIP");

                MilitaryDepartment militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment(militaryDepartmentID, CurrentUser);

                FillTechnicsRequest request = FillTechnicsRequestUtil.GetFillTechnicsRequest(fulfilTechnicsRequestID, CurrentUser);

                TechnicsRequestCommandPosition position = TechnicsRequestCommandPositionUtil.GetTechnicsRequestCommandPosition(CurrentUser, request.TechnicsRequestCommandPositionID);

                MobileLiftingEquip mobileLiftingEquip = MobileLiftingEquipUtil.GetMobileLiftingEquipByTechnicsId(request.TechnicsID, CurrentUser);

                string logDescription = "";
                logDescription += "Заявка №: " + position.TechnicsRequestsCommand.EquipmentTechnicsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(position.TechnicsRequestsCommand.EquipmentTechnicsRequest.RequestDate) +
                                  "; Команда: " + position.TechnicsRequestsCommand.MilitaryCommand.DisplayTextForSelection +
                                  "; ВО: " + militaryDepartment.MilitaryDepartmentName +
                                  "; Вид резерв: " + request.TechnicReadiness +
                                  "; Вид техника: " + position.TechnicsType.TypeName +
                                  "; Нормативна категория (код): " + (position.NormativeTechnics != null ? position.NormativeTechnics.NormativeCode : "") +
                                  "; Коментар: " + position.Comment +
                                  "; Рег. №: " + mobileLiftingEquip.RegNumber;

                ChangeEvent changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteMobLiftEquip", logDescription, position.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, CurrentUser);

                FillTechnicsRequestUtil.DeleteRequestCommandTechnic(fulfilTechnicsRequestID, CurrentUser, changeEvent);

                //Change the current Military Reporting Status of the chosen technics
                TechnicsMilRepStatusUtil.SetMilRepStatusTo_FREE(request.TechnicsID, CurrentUser, change);

                //Clear the current technics appointment
                TechnicsAppointmentUtil.ClearTheCurrentTechnicsAppointmentByTechnics(request.TechnicsID, CurrentUser, change);

                change.AddEvent(changeEvent);

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

        private void JSDeleteTractor()
        {
            string stat = "";
            string response = "";

            int fulfilTechnicsRequestID = int.Parse(Request.Form["FulfilTechnicsRequestID"]);
            int militaryDepartmentID = int.Parse(Request.Form["MilitaryDepartmentID"]);

            try
            {
                Change change = new Change(CurrentUser, "RES_Technics_TRACTORS");

                MilitaryDepartment militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment(militaryDepartmentID, CurrentUser);

                FillTechnicsRequest request = FillTechnicsRequestUtil.GetFillTechnicsRequest(fulfilTechnicsRequestID, CurrentUser);

                TechnicsRequestCommandPosition position = TechnicsRequestCommandPositionUtil.GetTechnicsRequestCommandPosition(CurrentUser, request.TechnicsRequestCommandPositionID);

                Tractor tractor = TractorUtil.GetTractorByTechnicsId(request.TechnicsID, CurrentUser);

                string logDescription = "";
                logDescription += "Заявка №: " + position.TechnicsRequestsCommand.EquipmentTechnicsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(position.TechnicsRequestsCommand.EquipmentTechnicsRequest.RequestDate) +
                                  "; Команда: " + position.TechnicsRequestsCommand.MilitaryCommand.DisplayTextForSelection +
                                  "; ВО: " + militaryDepartment.MilitaryDepartmentName +
                                  "; Вид резерв: " + request.TechnicReadiness +
                                  "; Вид техника: " + position.TechnicsType.TypeName +
                                  "; Нормативна категория (код): " + (position.NormativeTechnics != null ? position.NormativeTechnics.NormativeCode : "") +
                                  "; Коментар: " + position.Comment +
                                  "; Рег. №: " + tractor.RegNumber;

                ChangeEvent changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteTractor", logDescription, position.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, CurrentUser);

                FillTechnicsRequestUtil.DeleteRequestCommandTechnic(fulfilTechnicsRequestID, CurrentUser, changeEvent);

                //Change the current Military Reporting Status of the chosen technics
                TechnicsMilRepStatusUtil.SetMilRepStatusTo_FREE(request.TechnicsID, CurrentUser, change);

                //Clear the current technics appointment
                TechnicsAppointmentUtil.ClearTheCurrentTechnicsAppointmentByTechnics(request.TechnicsID, CurrentUser, change);

                change.AddEvent(changeEvent);

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

        private void JSDeleteEngEquip()
        {
            string stat = "";
            string response = "";

            int fulfilTechnicsRequestID = int.Parse(Request.Form["FulfilTechnicsRequestID"]);
            int militaryDepartmentID = int.Parse(Request.Form["MilitaryDepartmentID"]);

            try
            {
                Change change = new Change(CurrentUser, "RES_Technics_ENG_EQUIP");

                MilitaryDepartment militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment(militaryDepartmentID, CurrentUser);

                FillTechnicsRequest request = FillTechnicsRequestUtil.GetFillTechnicsRequest(fulfilTechnicsRequestID, CurrentUser);

                TechnicsRequestCommandPosition position = TechnicsRequestCommandPositionUtil.GetTechnicsRequestCommandPosition(CurrentUser, request.TechnicsRequestCommandPositionID);

                EngEquip engEquip = EngEquipUtil.GetEngEquipByTechnicsId(request.TechnicsID, CurrentUser);

                string logDescription = "";
                logDescription += "Заявка №: " + position.TechnicsRequestsCommand.EquipmentTechnicsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(position.TechnicsRequestsCommand.EquipmentTechnicsRequest.RequestDate) +
                                  "; Команда: " + position.TechnicsRequestsCommand.MilitaryCommand.DisplayTextForSelection +
                                  "; ВО: " + militaryDepartment.MilitaryDepartmentName +
                                  "; Вид резерв: " + request.TechnicReadiness +
                                  "; Вид техника: " + position.TechnicsType.TypeName +
                                  "; Нормативна категория (код): " + (position.NormativeTechnics != null ? position.NormativeTechnics.NormativeCode : "") +
                                  "; Коментар: " + position.Comment +
                                  "; Рег. №: " + engEquip.RegNumber;

                ChangeEvent changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteEngEquip", logDescription, position.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, CurrentUser);

                FillTechnicsRequestUtil.DeleteRequestCommandTechnic(fulfilTechnicsRequestID, CurrentUser, changeEvent);

                //Change the current Military Reporting Status of the chosen technics
                TechnicsMilRepStatusUtil.SetMilRepStatusTo_FREE(request.TechnicsID, CurrentUser, change);

                //Clear the current technics appointment
                TechnicsAppointmentUtil.ClearTheCurrentTechnicsAppointmentByTechnics(request.TechnicsID, CurrentUser, change);

                change.AddEvent(changeEvent);

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

        private void JSDeleteRailwayEquip()
        {
            string stat = "";
            string response = "";

            int fulfilTechnicsRequestID = int.Parse(Request.Form["FulfilTechnicsRequestID"]);
            int militaryDepartmentID = int.Parse(Request.Form["MilitaryDepartmentID"]);

            try
            {
                Change change = new Change(CurrentUser, "RES_Technics_RAILWAY_EQUIP");

                MilitaryDepartment militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment(militaryDepartmentID, CurrentUser);

                FillTechnicsRequest request = FillTechnicsRequestUtil.GetFillTechnicsRequest(fulfilTechnicsRequestID, CurrentUser);

                TechnicsRequestCommandPosition position = TechnicsRequestCommandPositionUtil.GetTechnicsRequestCommandPosition(CurrentUser, request.TechnicsRequestCommandPositionID);

                RailwayEquip railwayEquip = RailwayEquipUtil.GetRailwayEquipByTechnicsId(request.TechnicsID, CurrentUser);

                string logDescription = "";
                logDescription += "Заявка №: " + position.TechnicsRequestsCommand.EquipmentTechnicsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(position.TechnicsRequestsCommand.EquipmentTechnicsRequest.RequestDate) +
                                  "; Команда: " + position.TechnicsRequestsCommand.MilitaryCommand.DisplayTextForSelection +
                                  "; ВО: " + militaryDepartment.MilitaryDepartmentName +
                                  "; Вид резерв: " + request.TechnicReadiness +
                                  "; Вид техника: " + position.TechnicsType.TypeName +
                                  "; Нормативна категория (код): " + (position.NormativeTechnics != null ? position.NormativeTechnics.NormativeCode : "") +
                                  "; Коментар: " + position.Comment +
                                  "; Инв. №: " + railwayEquip.InventoryNumber;

                ChangeEvent changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteRailwayEquip", logDescription, position.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, CurrentUser);

                FillTechnicsRequestUtil.DeleteRequestCommandTechnic(fulfilTechnicsRequestID, CurrentUser, changeEvent);

                //Change the current Military Reporting Status of the chosen technics
                TechnicsMilRepStatusUtil.SetMilRepStatusTo_FREE(request.TechnicsID, CurrentUser, change);

                //Clear the current technics appointment
                TechnicsAppointmentUtil.ClearTheCurrentTechnicsAppointmentByTechnics(request.TechnicsID, CurrentUser, change);

                change.AddEvent(changeEvent);

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

        private void JSDeleteAviationEquip()
        {
            string stat = "";
            string response = "";

            int fulfilTechnicsRequestID = int.Parse(Request.Form["FulfilTechnicsRequestID"]);
            int militaryDepartmentID = int.Parse(Request.Form["MilitaryDepartmentID"]);

            try
            {
                Change change = new Change(CurrentUser, "RES_Technics_AVIATION_EQUIP");

                MilitaryDepartment militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment(militaryDepartmentID, CurrentUser);

                FillTechnicsRequest request = FillTechnicsRequestUtil.GetFillTechnicsRequest(fulfilTechnicsRequestID, CurrentUser);

                TechnicsRequestCommandPosition position = TechnicsRequestCommandPositionUtil.GetTechnicsRequestCommandPosition(CurrentUser, request.TechnicsRequestCommandPositionID);

                AviationEquip aviationEquip = AviationEquipUtil.GetAviationEquipByTechnicsId(request.TechnicsID, CurrentUser);

                string logDescription = "";
                logDescription += "Заявка №: " + position.TechnicsRequestsCommand.EquipmentTechnicsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(position.TechnicsRequestsCommand.EquipmentTechnicsRequest.RequestDate) +
                                  "; Команда: " + position.TechnicsRequestsCommand.MilitaryCommand.DisplayTextForSelection +
                                  "; ВО: " + militaryDepartment.MilitaryDepartmentName +
                                  "; Вид резерв: " + request.TechnicReadiness +
                                  "; Вид техника: " + position.TechnicsType.TypeName +
                                  "; Нормативна категория (код): " + (position.NormativeTechnics != null ? position.NormativeTechnics.NormativeCode : "") +
                                  "; Коментар: " + position.Comment +
                                  "; Инв. № (въздухопл. средство): " + aviationEquip.AirInvNumber;

                ChangeEvent changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteAviationEquip", logDescription, position.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, CurrentUser);

                FillTechnicsRequestUtil.DeleteRequestCommandTechnic(fulfilTechnicsRequestID, CurrentUser, changeEvent);

                //Change the current Military Reporting Status of the chosen technics
                TechnicsMilRepStatusUtil.SetMilRepStatusTo_FREE(request.TechnicsID, CurrentUser, change);

                //Clear the current technics appointment
                TechnicsAppointmentUtil.ClearTheCurrentTechnicsAppointmentByTechnics(request.TechnicsID, CurrentUser, change);

                change.AddEvent(changeEvent);

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

        private void JSDeleteVessel()
        {
            string stat = "";
            string response = "";

            int fulfilTechnicsRequestID = int.Parse(Request.Form["FulfilTechnicsRequestID"]);
            int militaryDepartmentID = int.Parse(Request.Form["MilitaryDepartmentID"]);

            try
            {
                Change change = new Change(CurrentUser, "RES_Technics_VESSELS");

                MilitaryDepartment militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment(militaryDepartmentID, CurrentUser);

                FillTechnicsRequest request = FillTechnicsRequestUtil.GetFillTechnicsRequest(fulfilTechnicsRequestID, CurrentUser);

                TechnicsRequestCommandPosition position = TechnicsRequestCommandPositionUtil.GetTechnicsRequestCommandPosition(CurrentUser, request.TechnicsRequestCommandPositionID);

                Vessel vessel = VesselUtil.GetVesselByTechnicsId(request.TechnicsID, CurrentUser);

                string logDescription = "";
                logDescription += "Заявка №: " + position.TechnicsRequestsCommand.EquipmentTechnicsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(position.TechnicsRequestsCommand.EquipmentTechnicsRequest.RequestDate) +
                                  "; Команда: " + position.TechnicsRequestsCommand.MilitaryCommand.DisplayTextForSelection +
                                  "; ВО: " + militaryDepartment.MilitaryDepartmentName +
                                  "; Вид резерв: " + request.TechnicReadiness +
                                  "; Вид техника: " + position.TechnicsType.TypeName +
                                  "; Нормативна категория (код): " + (position.NormativeTechnics != null ? position.NormativeTechnics.NormativeCode : "") +
                                  "; Коментар: " + position.Comment +
                                  "; Име: " + vessel.VesselName +
                                  "; Инвент. №: " + vessel.InventoryNumber;

                ChangeEvent changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteVessel", logDescription, position.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, CurrentUser);

                FillTechnicsRequestUtil.DeleteRequestCommandTechnic(fulfilTechnicsRequestID, CurrentUser, changeEvent);

                //Change the current Military Reporting Status of the chosen technics
                TechnicsMilRepStatusUtil.SetMilRepStatusTo_FREE(request.TechnicsID, CurrentUser, change);

                //Clear the current technics appointment
                TechnicsAppointmentUtil.ClearTheCurrentTechnicsAppointmentByTechnics(request.TechnicsID, CurrentUser, change);

                change.AddEvent(changeEvent);

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

        private void JSDeleteFuelContainer()
        {
            string stat = "";
            string response = "";

            int fulfilTechnicsRequestID = int.Parse(Request.Form["FulfilTechnicsRequestID"]);
            int militaryDepartmentID = int.Parse(Request.Form["MilitaryDepartmentID"]);

            try
            {
                Change change = new Change(CurrentUser, "RES_Technics_FUEL_CONTAINERS");

                MilitaryDepartment militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment(militaryDepartmentID, CurrentUser);

                FillTechnicsRequest request = FillTechnicsRequestUtil.GetFillTechnicsRequest(fulfilTechnicsRequestID, CurrentUser);

                TechnicsRequestCommandPosition position = TechnicsRequestCommandPositionUtil.GetTechnicsRequestCommandPosition(CurrentUser, request.TechnicsRequestCommandPositionID);

                FuelContainer fuelContainer = FuelContainerUtil.GetFuelContainerByTechnicsId(request.TechnicsID, CurrentUser);

                string logDescription = "";
                logDescription += "Заявка №: " + position.TechnicsRequestsCommand.EquipmentTechnicsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(position.TechnicsRequestsCommand.EquipmentTechnicsRequest.RequestDate) +
                                  "; Команда: " + position.TechnicsRequestsCommand.MilitaryCommand.DisplayTextForSelection +
                                  "; ВО: " + militaryDepartment.MilitaryDepartmentName +
                                  "; Вид резерв: " + request.TechnicReadiness +
                                  "; Вид техника: " + position.TechnicsType.TypeName +
                                  "; Нормативна категория (код): " + (position.NormativeTechnics != null ? position.NormativeTechnics.NormativeCode : "") +
                                  "; Коментар: " + position.Comment +
                                  "; Инв. №: " + fuelContainer.InventoryNumber;

                ChangeEvent changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteFuelContainer", logDescription, position.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, CurrentUser);

                FillTechnicsRequestUtil.DeleteRequestCommandTechnic(fulfilTechnicsRequestID, CurrentUser, changeEvent);

                //Change the current Military Reporting Status of the chosen technics
                TechnicsMilRepStatusUtil.SetMilRepStatusTo_FREE(request.TechnicsID, CurrentUser, change);

                //Clear the current technics appointment
                TechnicsAppointmentUtil.ClearTheCurrentTechnicsAppointmentByTechnics(request.TechnicsID, CurrentUser, change);

                change.AddEvent(changeEvent);

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


        // Generate contents for view fulfilment vehicles light box
        private string GetViewFulfilmentVehiclesLightBoxHtml(int techRequestCommandPositionID)
        {
            TechnicsType technicsType = TechnicsTypeUtil.GetTechnicsType("VEHICLES", CurrentUser);
            bool IsEditable = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL_MILCOMMAND_POSITIONS_FULFIL") == UIAccessLevel.Enabled;            

            string html = "";

            string htmlNoResults = "";

            List<VehicleFulfilmentBlock> vehicles = new List<VehicleFulfilmentBlock>();           
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

            allRows = VehicleUtil.GetAllVehicleFulfilmentBlocksCount(techRequestCommandPositionID, militaryDepartmentID, CurrentUser);
            maxPage = allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            vehicles = VehicleUtil.GetAllVehicleFulfilmentBlocks(techRequestCommandPositionID, militaryDepartmentID, orderBy, pageIndex, pageLength, CurrentUser);            

            // No data found
            if (vehicles.Count == 0)
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
                        <input type='hidden' id='hdnRequestCommandPositionID' value='" + techRequestCommandPositionID + @"' />

                        <span class='HeaderText'>" + technicsType.TypeName + @"</span><br /><br /><br />

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
            string[] arrOrderCol = { "", "", "", "", "", "", "" };

            arrOrderCol[orderCol - 1] = @"<div style='position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

            //Setup the header of the grid
            html += @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='width: 20px;" + headerStyle + @"'></th>
                               <th style='width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Рег. №" + arrOrderCol[0] + @"</th>
                               <th style='width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(5);'>Нормативна категория" + arrOrderCol[4] + @"</th>
                               <th style='width: 170px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>Марка/модел" + arrOrderCol[1] + @"</th>
                               <th style='width: 140px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>Вид" + arrOrderCol[2] + @"</th>
                               <th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(4);'>Тип резерв" + arrOrderCol[3] + @"</th>
                               <th style='width: 80px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(6);'>Връчено МН" + arrOrderCol[5] + @"</th>" +
               (IsEditable ? @"<th style='width: 30px;" + headerStyle + @"'>&nbsp;</th>" : "" ) + @"
                            </tr></thead>";

            int counter = 1;

            //Iterate through all items and add them into the grid
            foreach (VehicleFulfilmentBlock block in vehicles)
            {
                Vehicle vehicle = block.Vehicle;
                string deleteHTML = "";

                if (IsEditable)
                    deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на този запис' class='GridActionIcon' style='float: right; margin-right: 0px;' onclick='DeleteVehicle(" + block.FulfilTechnicsRequestID.ToString() + "," + militaryDepartmentID.ToString() + ");' />";

                string cellStyle = "vertical-align: top;";

                html += @"<tr  class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"' >
                                 <td style='" + cellStyle + @"'>" + ((pageIndex - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'><img src='../Images/pawn_view.png' alt='Преглед' title='Преглед' class='GridActionIcon' onclick='PreviewTechnics(" + vehicle.TechnicsId.ToString() + @");' /></td>
                                 <td style='" + cellStyle + @"'>" + vehicle.RegNumber + @"</td>
                                 <td style='" + cellStyle + @"' title='" + (vehicle.Technics != null ? vehicle.Technics.NormativeTechnics.NormativeName : "") + @"'>" + (vehicle.Technics != null ? vehicle.Technics.NormativeTechnics.NormativeCode : "") + @"</td>";

                                 //<td style='" + cellStyle + @"'>" + (vehicle.VehicleMake != null ? vehicle.VehicleMake.VehicleMakeName : "") + "/" + (vehicle.VehicleModel != null ? vehicle.VehicleModel.VehicleModelName : "") + @"</td>

                html += @"
                                 <td style='" + cellStyle + @"'>" + vehicle.VehicleMakeName + "/" + vehicle.VehicleModelName + @"</td>
                                 <td style='" + cellStyle + @"'>" + (vehicle.VehicleKind != null ? vehicle.VehicleKind.TableValue : "") + @"</td>
                                 <td style='" + cellStyle + @"'>" + block.TechnicReadiness + @"</td>
                                 <td style='" + cellStyle + @" text-align: center;'>" + (block.AppointmentIsDelivered ? ListItems.GetOptionYes().Text : ListItems.GetOptionNo().Text) + @"</td>" +
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

        // Generate contents for view fulfilment trailers light box
        private string GetViewFulfilmentTrailersLightBoxHtml(int techRequestCommandPositionID)
        {
            TechnicsType technicsType = TechnicsTypeUtil.GetTechnicsType("TRAILERS", CurrentUser);
            bool IsEditable = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL_MILCOMMAND_POSITIONS_FULFIL") == UIAccessLevel.Enabled;

            string html = "";

            string htmlNoResults = "";

            List<TrailerFulfilmentBlock> trailers = new List<TrailerFulfilmentBlock>();
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

            allRows = TrailerUtil.GetAllTrailerFulfilmentBlocksCount(techRequestCommandPositionID, militaryDepartmentID, CurrentUser);
            maxPage = allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            trailers = TrailerUtil.GetAllTrailerFulfilmentBlocks(techRequestCommandPositionID, militaryDepartmentID, orderBy, pageIndex, pageLength, CurrentUser);

            // No data found
            if (trailers.Count == 0)
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
                        <input type='hidden' id='hdnRequestCommandPositionID' value='" + techRequestCommandPositionID + @"' />

                        <span class='HeaderText'>" + technicsType.TypeName + @"</span><br /><br /><br />

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
                               <th style='width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Рег. №" + arrOrderCol[0] + @"</th>
                               <th style='width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(5);'>Нормативна категория" + arrOrderCol[4] + @"</th>
                               <th style='width: 170px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>Вид" + arrOrderCol[1] + @"</th>
                               <th style='width: 140px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>Тип" + arrOrderCol[2] + @"</th>
                               <th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(4);'>Тип резерв" + arrOrderCol[3] + @"</th>
                               <th style='width: 80px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(6);'>Връчено МН" + arrOrderCol[5] + @"</th>" +
               (IsEditable ? @"<th style='width: 30px;" + headerStyle + @"'>&nbsp;</th>" : "") + @"
                            </tr></thead>";

            int counter = 1;

            //Iterate through all items and add them into the grid
            foreach (TrailerFulfilmentBlock block in trailers)
            {
                Trailer trailer = block.Trailer;
                string deleteHTML = "";

                if (IsEditable)
                    deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на този запис' class='GridActionIcon' style='float: right; margin-right: 0px;' onclick='DeleteTrailer(" + block.FulfilTechnicsRequestID.ToString() + "," + militaryDepartmentID.ToString() + ");' />";

                string cellStyle = "vertical-align: top;";

                html += @"<tr  class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"' >
                                 <td style='" + cellStyle + @"'>" + ((pageIndex - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'><img src='../Images/pawn_view.png' alt='Преглед' title='Преглед' class='GridActionIcon' onclick='PreviewTechnics(" + trailer.TechnicsId.ToString() + @");' /></td>
                                 <td style='" + cellStyle + @"'>" + trailer.RegNumber + @"</td>
                                 <td style='" + cellStyle + @"' title='" + (trailer.Technics != null ? trailer.Technics.NormativeTechnics.NormativeName : "") + @"'>" + (trailer.Technics != null ? trailer.Technics.NormativeTechnics.NormativeCode : "") + @"</td>
                                 <td style='" + cellStyle + @"'>" + (trailer.TrailerKind != null ? trailer.TrailerKind.TableValue : "") + @"</td>
                                 <td style='" + cellStyle + @"'>" + (trailer.TrailerType != null ? trailer.TrailerType.TableValue : "") + @"</td>
                                 <td style='" + cellStyle + @"'>" + block.TechnicReadiness + @"</td>
                                 <td style='" + cellStyle + @" text-align: center;'>" + (block.AppointmentIsDelivered ? ListItems.GetOptionYes().Text : ListItems.GetOptionNo().Text) + @"</td>" +
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

        // Generate contents for view fulfilment tractors light box
        private string GetViewFulfilmentTractorsLightBoxHtml(int techRequestCommandPositionID)
        {
            TechnicsType technicsType = TechnicsTypeUtil.GetTechnicsType("TRACTORS", CurrentUser);
            bool IsEditable = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL_MILCOMMAND_POSITIONS_FULFIL") == UIAccessLevel.Enabled;

            string html = "";

            string htmlNoResults = "";

            List<TractorFulfilmentBlock> tractors = new List<TractorFulfilmentBlock>();
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

            allRows = TractorUtil.GetAllTractorFulfilmentBlocksCount(techRequestCommandPositionID, militaryDepartmentID, CurrentUser);
            maxPage = allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            tractors = TractorUtil.GetAllTractorFulfilmentBlocks(techRequestCommandPositionID, militaryDepartmentID, orderBy, pageIndex, pageLength, CurrentUser);

            // No data found
            if (tractors.Count == 0)
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
                        <input type='hidden' id='hdnRequestCommandPositionID' value='" + techRequestCommandPositionID + @"' />

                        <span class='HeaderText'>" + technicsType.TypeName + @"</span><br /><br /><br />

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
                               <th style='width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Рег. №" + arrOrderCol[0] + @"</th>
                               <th style='width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(5);'>Нормативна категория" + arrOrderCol[4] + @"</th>
                               <th style='width: 170px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>Марка/модел" + arrOrderCol[1] + @"</th>
                               <th style='width: 140px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>Вид" + arrOrderCol[2] + @"</th>
                               <th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(4);'>Тип резерв" + arrOrderCol[3] + @"</th>
                               <th style='width: 80px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(6);'>Връчено МН" + arrOrderCol[5] + @"</th>" +
               (IsEditable ? @"<th style='width: 30px;" + headerStyle + @"'>&nbsp;</th>" : "") + @"
                            </tr></thead>";

            int counter = 1;

            //Iterate through all items and add them into the grid
            foreach (TractorFulfilmentBlock block in tractors)
            {
                Tractor tractor = block.Tractor;
                string deleteHTML = "";

                if (IsEditable)
                    deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на този запис' class='GridActionIcon' style='float: right; margin-right: 0px;' onclick='DeleteTractor(" + block.FulfilTechnicsRequestID.ToString() + "," + militaryDepartmentID.ToString() + ");' />";

                string cellStyle = "vertical-align: top;";

                html += @"<tr  class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"' >
                                 <td style='" + cellStyle + @"'>" + ((pageIndex - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'><img src='../Images/pawn_view.png' alt='Преглед' title='Преглед' class='GridActionIcon' onclick='PreviewTechnics(" + tractor.TechnicsId.ToString() + @");' /></td>
                                 <td style='" + cellStyle + @"'>" + tractor.RegNumber + @"</td>
                                 <td style='" + cellStyle + @"' title='" + (tractor.Technics != null ? tractor.Technics.NormativeTechnics.NormativeName : "") + @"'>" + (tractor.Technics != null ? tractor.Technics.NormativeTechnics.NormativeCode : "") + @"</td>";

                                 //<td style='" + cellStyle + @"'>" + (tractor.TractorMake != null ? tractor.TractorMake.TractorMakeName : "") + "/" + (tractor.TractorModel != null ? tractor.TractorModel.TractorModelName : "") + @"</td>

                html += @"
                                 <td style='" + cellStyle + @"'>" + tractor.TractorMakeName + "/" + tractor.TractorModelName + @"</td>
                                 <td style='" + cellStyle + @"'>" + (tractor.TractorKind != null ? tractor.TractorKind.TableValue : "") + @"</td>
                                 <td style='" + cellStyle + @"'>" + block.TechnicReadiness + @"</td>
                                 <td style='" + cellStyle + @" text-align: center;'>" + (block.AppointmentIsDelivered ? ListItems.GetOptionYes().Text : ListItems.GetOptionNo().Text) + @"</td>" +
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

        // Generate contents for view fulfilment mobile lifting equip light box
        private string GetViewFulfilmentMobileLiftingEquipLightBoxHtml(int techRequestCommandPositionID)
        {
            TechnicsType technicsType = TechnicsTypeUtil.GetTechnicsType("MOB_LIFT_EQUIP", CurrentUser);
            bool IsEditable = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL_MILCOMMAND_POSITIONS_FULFIL") == UIAccessLevel.Enabled;

            string html = "";

            string htmlNoResults = "";

            List<MobileLiftingEquipFulfilmentBlock> mobileLiftingEquips = new List<MobileLiftingEquipFulfilmentBlock>();
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

            allRows = MobileLiftingEquipUtil.GetAllMobileLiftingEquipFulfilmentBlocksCount(techRequestCommandPositionID, militaryDepartmentID, CurrentUser);
            maxPage = allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            mobileLiftingEquips = MobileLiftingEquipUtil.GetAllMobileLiftingEquipFulfilmentBlocks(techRequestCommandPositionID, militaryDepartmentID, orderBy, pageIndex, pageLength, CurrentUser);

            // No data found
            if (mobileLiftingEquips.Count == 0)
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
                        <input type='hidden' id='hdnRequestCommandPositionID' value='" + techRequestCommandPositionID + @"' />

                        <span class='HeaderText'>" + technicsType.TypeName + @"</span><br /><br /><br />

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

                        <table id='tblMobileLiftingEquips' class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
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
                               <th style='width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Рег. №" + arrOrderCol[0] + @"</th>
                               <th style='width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(5);'>Нормативна категория" + arrOrderCol[4] + @"</th>
                               <th style='width: 140px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>Вид" + arrOrderCol[1] + @"</th>
                               <th style='width: 140px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>Тип" + arrOrderCol[2] + @"</th>
                               <th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(4);'>Тип резерв" + arrOrderCol[3] + @"</th>
                               <th style='width: 80px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(6);'>Връчено МН" + arrOrderCol[5] + @"</th>" +
               (IsEditable ? @"<th style='width: 30px;" + headerStyle + @"'>&nbsp;</th>" : "") + @"
                            </tr></thead>";

            int counter = 1;

            //Iterate through all items and add them into the grid
            foreach (MobileLiftingEquipFulfilmentBlock block in mobileLiftingEquips)
            {
                MobileLiftingEquip mobileLiftingEquip = block.MobileLiftingEquip; 
                string deleteHTML = "";

                if (IsEditable)
                    deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на този запис' class='GridActionIcon' style='float: right; margin-right: 0px;' onclick='DeleteMobileLiftingEquip(" + block.FulfilTechnicsRequestID.ToString() + "," + militaryDepartmentID.ToString() + ");' />";

                string cellStyle = "vertical-align: top;";

                html += @"<tr  class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"' >
                                 <td style='" + cellStyle + @"'>" + ((pageIndex - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'><img src='../Images/pawn_view.png' alt='Преглед' title='Преглед' class='GridActionIcon' onclick='PreviewTechnics(" + mobileLiftingEquip.TechnicsId.ToString() + @");' /></td>
                                 <td style='" + cellStyle + @"'>" + mobileLiftingEquip.RegNumber + @"</td>
                                 <td style='" + cellStyle + @"' title='" + (mobileLiftingEquip.Technics != null ? mobileLiftingEquip.Technics.NormativeTechnics.NormativeName : "") + @"'>" + (mobileLiftingEquip.Technics != null ? mobileLiftingEquip.Technics.NormativeTechnics.NormativeCode : "") + @"</td>
                                 <td style='" + cellStyle + @"'>" + (mobileLiftingEquip.MobileLiftingEquipKind != null ? mobileLiftingEquip.MobileLiftingEquipKind.TableValue : "") + @"</td>
                                 <td style='" + cellStyle + @"'>" + (mobileLiftingEquip.MobileLiftingEquipType != null ? mobileLiftingEquip.MobileLiftingEquipType.TableValue : "") + @"</td>
                                 <td style='" + cellStyle + @"'>" + block.TechnicReadiness + @"</td>
                                 <td style='" + cellStyle + @" text-align: center;'>" + (block.AppointmentIsDelivered ? ListItems.GetOptionYes().Text : ListItems.GetOptionNo().Text) + @"</td>" +
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

        // Generate contents for view fulfilment vessels light box
        private string GetViewFulfilmentVesselsLightBoxHtml(int techRequestCommandPositionID)
        {
            TechnicsType technicsType = TechnicsTypeUtil.GetTechnicsType("VESSELS", CurrentUser);
            bool IsEditable = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL_MILCOMMAND_POSITIONS_FULFIL") == UIAccessLevel.Enabled;

            string html = "";

            string htmlNoResults = "";

            List<VesselFulfilmentBlock> vessels = new List<VesselFulfilmentBlock>();
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

            allRows = VesselUtil.GetAllVesselFulfilmentBlocksCount(techRequestCommandPositionID, militaryDepartmentID, CurrentUser);
            maxPage = allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            vessels = VesselUtil.GetAllVesselFulfilmentBlocks(techRequestCommandPositionID, militaryDepartmentID, orderBy, pageIndex, pageLength, CurrentUser);

            // No data found
            if (vessels.Count == 0)
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
                        <input type='hidden' id='hdnRequestCommandPositionID' value='" + techRequestCommandPositionID + @"' />

                        <span class='HeaderText'>" + technicsType.TypeName + @"</span><br /><br /><br />

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

                        <table id='tblVessels' class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            
                         </thead>";

            //Set Table Results
            string headerStyle = "vertical-align: bottom;";
            int orderCol = (orderBy > 100 ? orderBy - 100 : orderBy);
            string img = orderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
            string[] arrOrderCol = { "", "", "", "", "", "", "" };

            arrOrderCol[orderCol - 1] = @"<div style='position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

            //Setup the header of the grid
            html += @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='width: 20px;" + headerStyle + @"'></th>
                               <th style='width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Име" + arrOrderCol[0] + @"</th>
                               <th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>Инвентарен номер" + arrOrderCol[1] + @"</th>
                               <th style='width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(6);'>Нормативна категория" + arrOrderCol[5] + @"</th> 
                               <th style='width: 140px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>Вид" + arrOrderCol[2] + @"</th>
                               <th style='width: 140px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(4);'>Тип" + arrOrderCol[3] + @"</th>
                               <th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(5);'>Тип резерв" + arrOrderCol[4] + @"</th>
                               <th style='width: 80px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(7);'>Връчено МН" + arrOrderCol[6] + @"</th>" +
               (IsEditable ? @"<th style='width: 30px;" + headerStyle + @"'>&nbsp;</th>" : "") + @"
                            </tr></thead>";

            int counter = 1;

            //Iterate through all items and add them into the grid
            foreach (VesselFulfilmentBlock block in vessels)
            {
                Vessel vessel = block.Vessel;
                string deleteHTML = "";

                if (IsEditable)
                    deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на този запис' class='GridActionIcon' style='float: right; margin-right: 0px;' onclick='DeleteVessel(" + block.FulfilTechnicsRequestID.ToString() + "," + militaryDepartmentID.ToString() + ");' />";

                string cellStyle = "vertical-align: top;";

                html += @"<tr  class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"' >
                                 <td style='" + cellStyle + @"'>" + ((pageIndex - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'><img src='../Images/pawn_view.png' alt='Преглед' title='Преглед' class='GridActionIcon' onclick='PreviewTechnics(" + vessel.TechnicsId.ToString() + @");' /></td>
                                 <td style='" + cellStyle + @"'>" + vessel.VesselName + @"</td>
                                 <td style='" + cellStyle + @"'>" + vessel.InventoryNumber + @"</td>
                                 <td style='" + cellStyle + @"' title='" + (vessel.Technics != null ? vessel.Technics.NormativeTechnics.NormativeName : "") + @"'>" + (vessel.Technics != null ? vessel.Technics.NormativeTechnics.NormativeCode : "") + @"</td>
                                 <td style='" + cellStyle + @"'>" + (vessel.VesselKind != null ? vessel.VesselKind.TableValue : "") + @"</td>
                                 <td style='" + cellStyle + @"'>" + (vessel.VesselType != null ? vessel.VesselType.TableValue : "") + @"</td>
                                 <td style='" + cellStyle + @"'>" + block.TechnicReadiness + @"</td>
                                 <td style='" + cellStyle + @" text-align: center;'>" + (block.AppointmentIsDelivered ? ListItems.GetOptionYes().Text : ListItems.GetOptionNo().Text) + @"</td>" +
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


        // Generate contents for view fulfilment eng equipment light box
        private string GetViewFulfilmentEngEquipLightBoxHtml(int techRequestCommandPositionID)
        {
            TechnicsType technicsType = TechnicsTypeUtil.GetTechnicsType("ENG_EQUIP", CurrentUser);
            bool IsEditable = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL_MILCOMMAND_POSITIONS_FULFIL") == UIAccessLevel.Enabled;

            string html = "";

            string htmlNoResults = "";

            List<EngEquipFulfilmentBlock> engEquips = new List<EngEquipFulfilmentBlock>();
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

            allRows = EngEquipUtil.GetAllEngEquipFulfilmentBlocksCount(techRequestCommandPositionID, militaryDepartmentID, CurrentUser);
            maxPage = allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            engEquips = EngEquipUtil.GetAllEngEquipFulfilmentBlocks(techRequestCommandPositionID, militaryDepartmentID, orderBy, pageIndex, pageLength, CurrentUser);

            // No data found
            if (engEquips.Count == 0)
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
                        <input type='hidden' id='hdnRequestCommandPositionID' value='" + techRequestCommandPositionID + @"' />

                        <span class='HeaderText'>" + technicsType.TypeName + @"</span><br /><br /><br />

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
                               <th style='width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Рег. №" + arrOrderCol[0] + @"</th>
                               <th style='width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(5);'>Нормативна категория" + arrOrderCol[4] + @"</th>
                               <th style='width: 170px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>Марка/модел" + arrOrderCol[1] + @"</th>
                               <th style='width: 140px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>Вид" + arrOrderCol[2] + @"</th>
                               <th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(4);'>Тип резерв" + arrOrderCol[3] + @"</th>
                               <th style='width: 80px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(6);'>Връчено МН" + arrOrderCol[5] + @"</th>" +
               (IsEditable ? @"<th style='width: 30px;" + headerStyle + @"'>&nbsp;</th>" : "") + @"
                            </tr></thead>";

            int counter = 1;

            //Iterate through all items and add them into the grid
            foreach (EngEquipFulfilmentBlock block in engEquips)
            {
                EngEquip engEquip = block.EngEquip;
                string deleteHTML = "";

                if (IsEditable)
                    deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на този запис' class='GridActionIcon' style='float: right; margin-right: 0px;' onclick='DeleteEngEquip(" + block.FulfilTechnicsRequestID.ToString() + "," + militaryDepartmentID.ToString() + ");' />";

                string cellStyle = "vertical-align: top;";

                html += @"<tr  class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"' >
                                 <td style='" + cellStyle + @"'>" + ((pageIndex - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'><img src='../Images/pawn_view.png' alt='Преглед' title='Преглед' class='GridActionIcon' onclick='PreviewTechnics(" + engEquip.TechnicsId.ToString() + @");' /></td>
                                 <td style='" + cellStyle + @"'>" + engEquip.RegNumber + @"</td>
                                 <td style='" + cellStyle + @"' title='" + (engEquip.Technics != null ? engEquip.Technics.NormativeTechnics.NormativeName : "") + @"'>" + (engEquip.Technics != null ? engEquip.Technics.NormativeTechnics.NormativeCode : "") + @"</td>";

                                 //<td style='" + cellStyle + @"'>" + (engEquip.EngEquipBaseMake != null ? engEquip.EngEquipBaseMake.EngEquipBaseMakeName : "") + "/" + (engEquip.EngEquipBaseModel != null ? engEquip.EngEquipBaseModel.EngEquipBaseModelName : "") + @"</td>

                html += @"
                                 <td style='" + cellStyle + @"'>" + engEquip.EngEquipBaseMakeName + "/" + engEquip.EngEquipBaseModelName + @"</td>
                                 <td style='" + cellStyle + @"'>" + (engEquip.EngEquipKind != null ? engEquip.EngEquipKind.TableValue : "") + @"</td>
                                 <td style='" + cellStyle + @"'>" + block.TechnicReadiness + @"</td>
                                 <td style='" + cellStyle + @" text-align: center;'>" + (block.AppointmentIsDelivered ? ListItems.GetOptionYes().Text : ListItems.GetOptionNo().Text) + @"</td>" +
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

        // Generate contents for view fulfilment railway equips light box
        private string GetViewFulfilmentRailwayEquipLightBoxHtml(int techRequestCommandPositionID)
        {
            TechnicsType technicsType = TechnicsTypeUtil.GetTechnicsType("RAILWAY_EQUIP", CurrentUser);
            bool IsEditable = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL_MILCOMMAND_POSITIONS_FULFIL") == UIAccessLevel.Enabled;

            string html = "";

            string htmlNoResults = "";

            List<RailwayEquipFulfilmentBlock> railwayEquips = new List<RailwayEquipFulfilmentBlock>();
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

            allRows = RailwayEquipUtil.GetAllRailwayEquipFulfilmentBlocksCount(techRequestCommandPositionID, militaryDepartmentID, CurrentUser);
            maxPage = allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            railwayEquips = RailwayEquipUtil.GetAllRailwayEquipFulfilmentBlocks(techRequestCommandPositionID, militaryDepartmentID, orderBy, pageIndex, pageLength, CurrentUser);

            // No data found
            if (railwayEquips.Count == 0)
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
                        <input type='hidden' id='hdnRequestCommandPositionID' value='" + techRequestCommandPositionID + @"' />

                        <span class='HeaderText'>" + technicsType.TypeName + @"</span><br /><br /><br />

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
            string[] arrOrderCol = { "", "", "", "", "", "", "" };

            arrOrderCol[orderCol - 1] = @"<div style='position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

            //Setup the header of the grid
            html += @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='width: 20px;" + headerStyle + @"'></th>
                               <th style='width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Инв. №" + arrOrderCol[0] + @"</th>
                               <th style='width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(6);'>Нормативна категория" + arrOrderCol[5] + @"</th>
                               <th style='width: 170px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>Вид" + arrOrderCol[1] + @"</th>
                               <th style='width: 140px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>Тип" + arrOrderCol[2] + @"</th>
                               <th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(4);'>Тип резерв" + arrOrderCol[3] + @"</th>
                               <th style='width: 60px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(5);'>Брой" + arrOrderCol[4] + @"</th>
                               <th style='width: 80px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(7);'>Връчено МН" + arrOrderCol[6] + @"</th>" +
               (IsEditable ? @"<th style='width: 30px;" + headerStyle + @"'>&nbsp;</th>" : "") + @"
                            </tr></thead>";

            int counter = 1;

            //Iterate through all items and add them into the grid
            foreach (RailwayEquipFulfilmentBlock block in railwayEquips)
            {
                RailwayEquip railwayEquip = block.RailwayEquip;
                string deleteHTML = "";

                if (IsEditable)
                    deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на този запис' class='GridActionIcon' style='float: right; margin-right: 0px;' onclick='DeleteRailwayEquip(" + block.FulfilTechnicsRequestID.ToString() + "," + militaryDepartmentID.ToString() + ");' />";

                string cellStyle = "vertical-align: top;";

                html += @"<tr  class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"' >
                                 <td style='" + cellStyle + @"'>" + ((pageIndex - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'><img src='../Images/pawn_view.png' alt='Преглед' title='Преглед' class='GridActionIcon' onclick='PreviewTechnics(" + railwayEquip.TechnicsId.ToString() + @");' /></td>
                                 <td style='" + cellStyle + @"'>" + railwayEquip.InventoryNumber + @"</td>
                                 <td style='" + cellStyle + @"' title='" + (railwayEquip.Technics != null ? railwayEquip.Technics.NormativeTechnics.NormativeName : "") + @"'>" + (railwayEquip.Technics != null ? railwayEquip.Technics.NormativeTechnics.NormativeCode : "") + @"</td>
                                 <td style='" + cellStyle + @"'>" + (railwayEquip.RailwayEquipKind != null ? railwayEquip.RailwayEquipKind.TableValue : "") + @"</td>
                                 <td style='" + cellStyle + @"'>" + (railwayEquip.RailwayEquipType != null ? railwayEquip.RailwayEquipType.TableValue : "") + @"</td>
                                 <td style='" + cellStyle + @"'>" + block.TechnicReadiness + @"</td>
                                 <td style='" + cellStyle + @"'>" + block.ItemsCount.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: center;'>" + (block.AppointmentIsDelivered ? ListItems.GetOptionYes().Text : ListItems.GetOptionNo().Text) + @"</td>" +
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

        // Generate contents for view fulfilment aviation equips light box
        private string GetViewFulfilmentAviationEquipLightBoxHtml(int techRequestCommandPositionID)
        {
            TechnicsType technicsType = TechnicsTypeUtil.GetTechnicsType("AVIATION_EQUIP", CurrentUser);
            bool IsEditable = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL_MILCOMMAND_POSITIONS_FULFIL") == UIAccessLevel.Enabled;

            string html = "";

            string htmlNoResults = "";

            List<AviationEquipFulfilmentBlock> aviationEquips = new List<AviationEquipFulfilmentBlock>();
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

            allRows = AviationEquipUtil.GetAllAviationEquipFulfilmentBlocksCount(techRequestCommandPositionID, militaryDepartmentID, CurrentUser);
            maxPage = allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            aviationEquips = AviationEquipUtil.GetAllAviationEquipFulfilmentBlocks(techRequestCommandPositionID, militaryDepartmentID, orderBy, pageIndex, pageLength, CurrentUser);

            // No data found
            if (aviationEquips.Count == 0)
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
                        <input type='hidden' id='hdnRequestCommandPositionID' value='" + techRequestCommandPositionID + @"' />

                        <span class='HeaderText'>" + technicsType.TypeName + @"</span><br /><br /><br />

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
            string[] arrOrderCol = { "", "", "", "", "", "", "" };

            arrOrderCol[orderCol - 1] = @"<div style='position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

            //Setup the header of the grid
            html += @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='width: 20px;" + headerStyle + @"'></th>
                               <th style='width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Инв. №" + arrOrderCol[0] + @"</th>
                               <th style='width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(6);'>Нормативна категория" + arrOrderCol[5] + @"</th>
                               <th style='width: 170px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>Вид" + arrOrderCol[1] + @"</th>
                               <th style='width: 140px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>Тип" + arrOrderCol[2] + @"</th>
                               <th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(4);'>Модел" + arrOrderCol[3] + @"</th>
                               <th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(5);'>Тип резерв" + arrOrderCol[4] + @"</th>
                               <th style='width: 80px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(7);'>Връчено МН" + arrOrderCol[6] + @"</th>" +
               (IsEditable ? @"<th style='width: 30px;" + headerStyle + @"'>&nbsp;</th>" : "") + @"
                            </tr></thead>";

            int counter = 1;

            //Iterate through all items and add them into the grid
            foreach (AviationEquipFulfilmentBlock block in aviationEquips)
            {
                AviationEquip aviationEquip = block.AviationEquip;
                string deleteHTML = "";

                if (IsEditable)
                    deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на този запис' class='GridActionIcon' style='float: right; margin-right: 0px;' onclick='DeleteAviationEquip(" + block.FulfilTechnicsRequestID.ToString() + "," + militaryDepartmentID.ToString() + ");' />";

                string cellStyle = "vertical-align: top;";

                html += @"<tr  class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"' >
                                 <td style='" + cellStyle + @"'>" + ((pageIndex - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'><img src='../Images/pawn_view.png' alt='Преглед' title='Преглед' class='GridActionIcon' onclick='PreviewTechnics(" + aviationEquip.TechnicsId.ToString() + @");' /></td>
                                 <td style='" + cellStyle + @"'>" + aviationEquip.AirInvNumber + @"</td>
                                 <td style='" + cellStyle + @"' title='" + (aviationEquip.Technics != null ? aviationEquip.Technics.NormativeTechnics.NormativeName : "") + @"'>" + (aviationEquip.Technics != null ? aviationEquip.Technics.NormativeTechnics.NormativeCode : "") + @"</td>
                                 <td style='" + cellStyle + @"'>" + (aviationEquip.AviationAirKind != null ? aviationEquip.AviationAirKind.TableValue : "") + @"</td>
                                 <td style='" + cellStyle + @"'>" + (aviationEquip.AviationAirType != null ? aviationEquip.AviationAirType.TableValue : "") + @"</td>";

                                 //<td style='" + cellStyle + @"'>" + (aviationEquip.AviationAirModel != null ? aviationEquip.AviationAirModel.TableValue : "") + @"</td>
                                 
                html += @"
                                 <td style='" + cellStyle + @"'>" + aviationEquip.AviationAirModelName + @"</td>
                                 <td style='" + cellStyle + @"'>" + block.TechnicReadiness + @"</td>
                                 <td style='" + cellStyle + @" text-align: center;'>" + (block.AppointmentIsDelivered ? ListItems.GetOptionYes().Text : ListItems.GetOptionNo().Text) + @"</td>" +
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

        // Generate contents for view fulfilment fuel containers light box
        private string GetViewFulfilmentFuelContainerLightBoxHtml(int techRequestCommandPositionID)
        {
            TechnicsType technicsType = TechnicsTypeUtil.GetTechnicsType("FUEL_CONTAINERS", CurrentUser);
            bool IsEditable = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL_MILCOMMAND_POSITIONS_FULFIL") == UIAccessLevel.Enabled;

            string html = "";

            string htmlNoResults = "";

            List<FuelContainerFulfilmentBlock> fuelContainers = new List<FuelContainerFulfilmentBlock>();
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

            allRows = FuelContainerUtil.GetAllFuelContainerFulfilmentBlocksCount(techRequestCommandPositionID, militaryDepartmentID, CurrentUser);
            maxPage = allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            fuelContainers = FuelContainerUtil.GetAllFuelContainerFulfilmentBlocks(techRequestCommandPositionID, militaryDepartmentID, orderBy, pageIndex, pageLength, CurrentUser);

            // No data found
            if (fuelContainers.Count == 0)
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
                        <input type='hidden' id='hdnRequestCommandPositionID' value='" + techRequestCommandPositionID + @"' />

                        <span class='HeaderText'>" + technicsType.TypeName + @"</span><br /><br /><br />

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
            string[] arrOrderCol = { "", "", "", "", "", "", "" };

            arrOrderCol[orderCol - 1] = @"<div style='position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

            //Setup the header of the grid
            html += @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='width: 20px;" + headerStyle + @"'></th>
                               <th style='width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Инв. №" + arrOrderCol[0] + @"</th>
                               <th style='width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(6);'>Нормативна категория" + arrOrderCol[5] + @"</th>
                               <th style='width: 170px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>Вид" + arrOrderCol[1] + @"</th>
                               <th style='width: 140px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>Тип" + arrOrderCol[2] + @"</th>
                               <th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(4);'>Тип резерв" + arrOrderCol[3] + @"</th>
                               <th style='width: 60px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(5);'>Брой" + arrOrderCol[4] + @"</th>
                               <th style='width: 80px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(7);'>Връчено МН" + arrOrderCol[6] + @"</th>" +
               (IsEditable ? @"<th style='width: 30px;" + headerStyle + @"'>&nbsp;</th>" : "") + @"
                            </tr></thead>";

            int counter = 1;

            //Iterate through all items and add them into the grid
            foreach (FuelContainerFulfilmentBlock block in fuelContainers)
            {
                FuelContainer fuelContainer = block.FuelContainer;
                string deleteHTML = "";

                if (IsEditable)
                    deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на този запис' class='GridActionIcon' style='float: right; margin-right: 0px;' onclick='DeleteFuelContainer(" + block.FulfilTechnicsRequestID.ToString() + "," + militaryDepartmentID.ToString() + ");' />";

                string cellStyle = "vertical-align: top;";

                html += @"<tr  class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"' >
                                 <td style='" + cellStyle + @"'>" + ((pageIndex - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'><img src='../Images/pawn_view.png' alt='Преглед' title='Преглед' class='GridActionIcon' onclick='PreviewTechnics(" + fuelContainer.TechnicsId.ToString() + @");' /></td>
                                 <td style='" + cellStyle + @"'>" + fuelContainer.InventoryNumber + @"</td>
                                 <td style='" + cellStyle + @"' title='" + (fuelContainer.Technics != null ? fuelContainer.Technics.NormativeTechnics.NormativeName : "") + @"'>" + (fuelContainer.Technics != null ? fuelContainer.Technics.NormativeTechnics.NormativeCode : "") + @"</td>
                                 <td style='" + cellStyle + @"'>" + (fuelContainer.FuelContainerKind != null ? fuelContainer.FuelContainerKind.TableValue : "") + @"</td>
                                 <td style='" + cellStyle + @"'>" + (fuelContainer.FuelContainerType != null ? fuelContainer.FuelContainerType.TableValue : "") + @"</td>
                                 <td style='" + cellStyle + @"'>" + block.TechnicReadiness + @"</td>
                                 <td style='" + cellStyle + @"'>" + block.ItemsCount.ToString() + @"</td>
                                 <td style='" + cellStyle + @" text-align: center;'>" + (block.AppointmentIsDelivered ? ListItems.GetOptionYes().Text : ListItems.GetOptionNo().Text) + @"</td>" +
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