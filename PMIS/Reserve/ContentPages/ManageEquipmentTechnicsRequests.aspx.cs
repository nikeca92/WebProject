using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;
using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class ManageEquipmentTechnicsRequests : RESPage
    {
        //Get the config setting that says how many rows per page should be dispayed in the grid
        private int pageLength = int.Parse(Config.GetWebSetting("RowsPerPage"));
        //Stores information about how many pages are in the grid
        private int maxPage;

        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "RES_EQUIPTECHREQUESTS";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if (GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            SetBtnNew();

            //Setup any calendar control on the screen
            SetupDatePickers();

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteEquipmentTechnicsRequest")
            {
                JSDeleteEquipmentTechnicsRequest();
                return;
            }

            //Hilight the current page in the menu bar
            HighlightMenuItems("Equipment", "ManageEquipmentTechnicsRequests");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            //Setup some basic styles on the screen
            SetupStyle();

            //Collect the filter information to be able to pull the number of rows for this specific filter
            EquipmentTechnicsRequestsFilter filter = CollectFilterData();

            int allRows = EquipmentTechnicsRequestUtil.GetAllEquipmentTechnicsRequestCount(filter, CurrentUser);
            //Get the number of rows and calculate the number of pages in the grid
            maxPage = pageLength == 0 ? 1 : allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);
            
            //When the page is loaded for the first time
            if (!IsPostBack)
            {
                //Populate any drop-downs and list-boxes
                PopulateLists();

                //The default order is by module name
                if (string.IsNullOrEmpty(hdnSortBy.Value))
                    hdnSortBy.Value = "1";

                //Simulate clicking the Refresh button to load the grid initially
                //btnRefresh_Click(btnRefresh, new EventArgs());
                this.btnPrintAllEquipmentTechnicsRequests.Visible = false;
                this.divNavigation.Visible = false;
                this.pnlSearchHint.Visible = true;
            }

            lblMessage.Text = "";
            lblGridMessage.Text = "";
        }

        //Populate all listboxes on the screen
        private void PopulateLists()
        {            
            PopulateAdministrations();
            PopulateEquipWithTechRequestsStatuses();
            PopulateMilitaryDepartment();
        }        

        //Populate the Administrations drop-down
        private void PopulateAdministrations()
        {
            ddAdministration.Items.Clear();
            ddAdministration.Items.Add(ListItems.GetOptionAll());

            List<Administration> administrations = AdministrationUtil.GetAllAdministrations(CurrentUser);

            foreach (Administration administration in administrations)
            {
                ListItem li = new ListItem();
                li.Text = administration.AdministrationName;
                li.Value = administration.AdministrationId.ToString();

                ddAdministration.Items.Add(li);
            }
        }

        //Populate the EquipWithTechRequestsStatuses drop-down
        private void PopulateEquipWithTechRequestsStatuses()
        {
            ddEquipWithTechRequestsStatus.Items.Clear();
            ddEquipWithTechRequestsStatus.Items.Add(ListItems.GetOptionAll());

            List<EquipWithTechRequestsStatus> statuses = EquipWithTechRequestsStatusUtil.GetAllEquipWithTechRequestsStatuses(CurrentUser);

            foreach (EquipWithTechRequestsStatus status in statuses)
            {
                ListItem li = new ListItem();
                li.Text = status.StatusName;
                li.Value = status.EquipWithTechRequestsStatusId.ToString();

                ddEquipWithTechRequestsStatus.Items.Add(li);
            }
        }

        //Populate the MilitaryUnits drop-down
        private void PopulateMilitaryDepartment()
        {
            ddMilitaryDepartment.Items.Clear();
            ddMilitaryDepartment.Items.Add(ListItems.GetOptionAll());

            List<MilitaryDepartment> militaryDepartments = MilitaryDepartmentUtil.GetAllMilitaryDepartmentsByEquipmentTechRequestsPerUser(CurrentUser, CurrentUser);

            foreach (MilitaryDepartment militaryDepartment in militaryDepartments)
            {
                ListItem li = new ListItem();
                li.Text = militaryDepartment.MilitaryDepartmentName;
                li.Value = militaryDepartment.MilitaryDepartmentId.ToString();

                ddMilitaryDepartment.Items.Add(li);
            }
        }

        //Setup some styling on the page
        private void SetupStyle()
        {

        }

        //Validate some field on the screen
        private bool ValidateData()
        {
            bool isDataValid = true;
            string errMsg = "";

            if (!isDataValid)
            {
                lblMessage.CssClass = "ErrorText";
                lblMessage.Text = errMsg;
            }

            return isDataValid;
        }

        //Refresh the data grid
        private void RefreshEquipmentTechnicsRequests()
        {
            string html = "";
            this.pnlSearchHint.Visible = false;


            //Collect the filters, the order control and the paging control data from the page
            int orderBy;
            if (!int.TryParse(hdnSortBy.Value, out orderBy))
                orderBy = 0;

            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            //Collect the filter information to be able to pull the number of rows for this specific filter
            EquipmentTechnicsRequestsFilter filter = CollectFilterData();

            //Get the list of records according to the specified filters, order and paging
            List<EquipmentTechnicsRequest> equipmentTechnicsRequests = EquipmentTechnicsRequestUtil.GetAllEquipmentTechnicsRequest(filter, pageLength, CurrentUser);

            //No data found
            if (equipmentTechnicsRequests.Count == 0)
            {
                html = "<span>Няма намерени резултати</span>";
                this.btnPrintAllEquipmentTechnicsRequests.Visible = false;
                this.divNavigation.Visible = false;
            }
            //If there is data then generate dynamically the HTML for the data grid
            else 
            {
                this.btnPrintAllEquipmentTechnicsRequests.Visible = true;
                this.divNavigation.Visible = true;

                string headerStyle = "vertical-align: bottom;";
                int orderCol = (orderBy > 100 ? orderBy - 100 : orderBy);
                string img = orderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
                string[] arrOrderCol = {"", "", "", "", ""};
                arrOrderCol[orderCol - 1] = @"<div style='Position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

                //Setup the header of the grid
                html = @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='width: 110px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Заявка №" + arrOrderCol[0] + @"</th>
                               <th style='width: 110px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>От дата" + arrOrderCol[1] + @"</th>
                               <th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>" + CommonFunctions.GetLabelText("MilitaryUnit") + arrOrderCol[2] + @"</th>
                               <th style='width: 250px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(4);'>Министерство/Ведомство" + arrOrderCol[3] + @"</th>
                               <th style='width: 200px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(5);'>Статус на заявката" + arrOrderCol[4] + @"</th>
                               <th style='width: 60px;" + headerStyle + @"'>&nbsp;</th>
                            </tr>
                         </thead>";

                int counter = 1;

                //Iterate through all items and add them into the grid
                foreach (EquipmentTechnicsRequest equipmentTechnicsRequest in equipmentTechnicsRequests)
                {
                    string cellStyle = "vertical-align: top;";

                    string deleteHTML = "";

                    if (equipmentTechnicsRequest.CanDelete)
                    {
                        if (GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS") == UIAccessLevel.Enabled &&
                            GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_DELETE") == UIAccessLevel.Enabled
                            )
                            deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на тази заявка' class='GridActionIcon' onclick='DeleteEquipmentTechnicsRequest(" + equipmentTechnicsRequest.EquipmentTechnicsRequestId.ToString() + ");' />";
                    }

                    string milDeptHTML = "";

                    if (GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_MILDEPT") != UIAccessLevel.Hidden)
                        milDeptHTML = "<img src='../Images/list_edit.png' alt='Определяне на ВО' title='Определяне на ВО' class='GridActionIcon' onclick='EquipmentTechnicsRequestMilDept(" + equipmentTechnicsRequest.EquipmentTechnicsRequestId.ToString() + ");' />";

                    string editHTML = "";

                    if (GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT") != UIAccessLevel.Hidden)
                        editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='EditEquipmentTechnicsRequest(" + equipmentTechnicsRequest.EquipmentTechnicsRequestId.ToString() + ");' />";

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='" + cellStyle + @"'>" + ((pageIdx - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + equipmentTechnicsRequest.RequestNumber + @"</td>
                                 <td style='" + cellStyle + @"'>" + CommonFunctions.FormatDate(equipmentTechnicsRequest.RequestDate) + @"</td>
                                 <td style='" + cellStyle + @"'>" + (equipmentTechnicsRequest.MilitaryUnit == null ? "" : equipmentTechnicsRequest.MilitaryUnit.DisplayTextForSelection) + @"</td>
                                 <td style='" + cellStyle + @"'>" + (equipmentTechnicsRequest.Administration == null ? "" : equipmentTechnicsRequest.Administration.AdministrationName) + @"</td>
                                 <td style='" + cellStyle + @"'>" + (equipmentTechnicsRequest.EquipWithTechRequestsStatus == null ? "" : equipmentTechnicsRequest.EquipWithTechRequestsStatus.StatusName) + @"</td>
                                 <td style='" + cellStyle + @"'>" + editHTML + milDeptHTML + deleteHTML + @"</td>
                              </tr>";

                    counter++;
                }

                html += "</table>";
            }

            //Put the generated grid on the page
            pnlDataGrid.InnerHtml = html;

            //Refresh the paging button according to the current position
            SetImgBtns();
            lblPagination.Text = " | " + hdnPageIdx.Value + " от " + maxPage.ToString() + " | ";
            txtGotoPage.Text = "";

            //Set the message if there is a need (e.g. a deleted item)
            if (hdnRefreshReason.Value != "")
            {
                if (hdnRefreshReason.Value == "DELETED")
                {
                    lblGridMessage.Text = "Заявката беше изтрита успешно";
                    lblGridMessage.CssClass = "SuccessText";
                }

                hdnRefreshReason.Value = "";
            }
        }

        //Refresh the data grid
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = "1";
                RefreshEquipmentTechnicsRequests();
            }
        }

        //Go to the first page and refresh the grid
        protected void btnFirst_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = "1";
                RefreshEquipmentTechnicsRequests();
            }
        }

        //Go to the previous page and refresh the grid
        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                int page = int.Parse(hdnPageIdx.Value);

                if (page > 1)
                {
                    page--;
                    hdnPageIdx.Value = page.ToString();

                    RefreshEquipmentTechnicsRequests();
                }
            }
        }

        //Go to the next page and refresh the grid
        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                int page = int.Parse(hdnPageIdx.Value);

                if (page < maxPage)
                {
                    page++;
                    hdnPageIdx.Value = page.ToString();

                    RefreshEquipmentTechnicsRequests();
                }
            }
        }

        //Go to the last page and refresh the grid
        protected void btnLast_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = maxPage.ToString();
                RefreshEquipmentTechnicsRequests();
            }
        }


        //Go to a specific page (entered by the user) and refresh the grid
        protected void btnGoto_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                int gotoPage;
                if (int.TryParse(txtGotoPage.Text, out gotoPage) && gotoPage > 0 && gotoPage <= maxPage)
                {
                    hdnPageIdx.Value = gotoPage.ToString();
                    RefreshEquipmentTechnicsRequests();
                }
            }
        }

        //Refresh the paging image buttons
        private void SetImgBtns()
        {
            int page = int.Parse(hdnPageIdx.Value);

            btnFirst.Enabled = true;
            btnPrev.Enabled = true;
            btnLast.Enabled = true;
            btnNext.Enabled = true;
            btnFirst.ImageUrl = "../Images/ButtonFirst.png";
            btnPrev.ImageUrl = "../Images/ButtonPrev.png";
            btnLast.ImageUrl = "../Images/ButtonLast.png";
            btnNext.ImageUrl = "../Images/ButtonNext.png";

            if (page == 1)
            {
                btnFirst.Enabled = false;
                btnPrev.Enabled = false;
                btnFirst.ImageUrl = "../Images/ButtonFirstDisabled.png";
                btnPrev.ImageUrl = "../Images/ButtonPrevDisabled.png";
            }

            if (page == maxPage)
            {
                btnLast.Enabled = false;
                btnNext.Enabled = false;
                btnLast.ImageUrl = "../Images/ButtonLastDisabled.png";
                btnNext.ImageUrl = "../Images/ButtonNextDisabled.png";
            }
        }

        //Navigate back to the home screen
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ContentPages/Home.aspx");
        }

        //Go to create a new record
        protected void btnNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ContentPages/AddEditEquipmentTechnicsRequest.aspx");
        }

        //Delete a record (ajax call)
        private void JSDeleteEquipmentTechnicsRequest()
        {
            if (GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_DELETE") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            int equipmentTechnicsRequestId = int.Parse(Request.Form["EquipmentTechnicsRequestId"]);

            string stat = "";
            string response = "";
            
            try
            {
                Change change = new Change(CurrentUser, "RES_EquipmentTechnicsRequests");


                List<FillTechnicsRequest> fillTechnicsRequests = FillTechnicsRequestUtil.GetFillTechnicsRequestByEquipmentTechnicsRequest(equipmentTechnicsRequestId, CurrentUser);
                /*
                List<int> deletedTechnics = new List<int>();

                //Remove all Technics from that Equipment Technics Request
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
                */

                string msg = "";
                if (fillTechnicsRequests.Count == 0)
                {
                    EquipmentTechnicsRequestUtil.DeleteEquipmentTechnicsRequest(equipmentTechnicsRequestId, CurrentUser, change);
                    change.WriteLog();

                }
                else
                {
                    msg = "Заявката не може да бъде изтрита, поради наличието на техника назначена от следните ВО:<br/>";
                    msg += "<div style='max-height: 400px;overflow-y:auto;margin-top:10px;'><ul style='margin-top:-5px; padding-top:3px;'>";

                    foreach (var md in fillTechnicsRequests.GroupBy(x => x.MilitaryDepartment.MilitaryDepartmentName))
                    {
                        msg += "<li>" + md.Key + "</li>";
                    }

                    msg += "</ul></div>";
                }
                
                stat = AJAXTools.OK;
                response = "<xml><response>OK</response><msg>" + AJAXTools.EncodeForXML(msg) + "</msg></xml>";
            }
            catch(Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }


            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }
 
        private void SetBtnNew()
        {        
            if (this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_ADD") == UIAccessLevel.Enabled
                && this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS") == UIAccessLevel.Enabled)
            {
                EnableButton(btnNew);
            }
            else
            {
                if (this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_ADD") == UIAccessLevel.Hidden)
                {
                    HideControl(btnNew);
                }
                else
                {
                    DisableButton(btnNew);
                }
            }
        }

        //Setup any date picker controls on the page by setting the CSS of the target inputs
        //Note that the date picker CSS is common
        //This makes the calendar controls to appear on the page next to each input
        private void SetupDatePickers()
        {
            txtRequestDateFrom.CssClass = CommonFunctions.DatePickerCSS();
            txtRequestDateTo.CssClass = CommonFunctions.DatePickerCSS();
        }

        //Collect the filet information from the page
        private EquipmentTechnicsRequestsFilter CollectFilterData()
        {
            EquipmentTechnicsRequestsFilter filter = new EquipmentTechnicsRequestsFilter();

            this.hdnRequestNumber.Value = this.txtRequestNumber.Text;

            DateTime? requestDateFrom = null;

            if (CommonFunctions.TryParseDate(txtRequestDateFrom.Text))
            {
                requestDateFrom = CommonFunctions.ParseDate(txtRequestDateFrom.Text);
                this.hdnRequestDateFrom.Value = this.txtRequestDateFrom.Text;
            }
            else
            {
                this.hdnRequestDateFrom.Value = "";
            }

            DateTime? requestDateTo = null;

            if (CommonFunctions.TryParseDate(txtRequestDateTo.Text))
            {
                requestDateTo = CommonFunctions.ParseDate(txtRequestDateTo.Text);
                this.hdnRequestDateTo.Value = this.txtRequestDateTo.Text;
            }
            else
            {
                this.hdnRequestDateTo.Value = "";
            }

            this.hdnCommandNum.Value = this.txtCommandNum.Text;

            string militaryUnits = "";

            if (msMilitaryUnit.SelectedValue != ListItems.GetOptionAll().Value)
            {
                militaryUnits = msMilitaryUnit.SelectedValue;
                this.hdnMilitaryUnitId.Value = msMilitaryUnit.SelectedValue;
            }
            else
            {
                this.hdnMilitaryUnitId.Value = "";
            }

            string administrations = "";

            if (ddAdministration.SelectedValue != ListItems.GetOptionAll().Value)
            {
                administrations = ddAdministration.SelectedValue;
                this.hdnAdministrationId.Value = this.ddAdministration.SelectedValue;
            }
            else
            {
                this.hdnAdministrationId.Value = "";
            }

            string equipWithTechRequestsStatuses = "";

            if (ddEquipWithTechRequestsStatus.SelectedValue != ListItems.GetOptionAll().Value)
            {
                equipWithTechRequestsStatuses = ddEquipWithTechRequestsStatus.SelectedValue;
                this.hdnEquipWithTechRequestsStatusId.Value = this.ddEquipWithTechRequestsStatus.SelectedValue;
            }
            else
            {
                this.hdnEquipWithTechRequestsStatusId.Value = "";
            }

            string militaryDepartments = "";

            if (ddMilitaryDepartment.SelectedValue != ListItems.GetOptionAll().Value)
            {
                militaryDepartments = ddMilitaryDepartment.SelectedValue;
                this.hdnMilitaryDepartmentId.Value = this.ddMilitaryDepartment.SelectedValue;
            }
            else
            {
                this.hdnMilitaryDepartmentId.Value = "";
            }

            int orderBy;
            if (!int.TryParse(hdnSortBy.Value, out orderBy))
                orderBy = 0;

            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            filter.RequestNum = txtRequestNumber.Text;
            filter.RequestDateFrom = requestDateFrom;
            filter.RequestDateTo = requestDateTo;
            filter.CommandNum = txtCommandNum.Text;
            filter.MilitaryUnits = militaryUnits;
            filter.Administrations = administrations;
            filter.EquipWithTechRequestsStatuses = equipWithTechRequestsStatuses;
            filter.MilitaryDepartments = militaryDepartments;
            filter.OrderBy = orderBy;
            filter.PageIdx = pageIdx;

            return filter;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtRequestNumber.Text = "";
            txtRequestDateFrom.Text = "";
            txtRequestDateTo.Text = "";
            txtCommandNum.Text = "";
            msMilitaryUnit.SelectedText = "";
            msMilitaryUnit.SelectedValue = ListItems.GetOptionAll().Value;
            ddAdministration.SelectedValue = ListItems.GetOptionAll().Value;
            ddEquipWithTechRequestsStatus.SelectedValue = ListItems.GetOptionAll().Value;
            ddMilitaryDepartment.SelectedValue = ListItems.GetOptionAll().Value;
            //btnRefresh_Click(sender, e);
        }
    }
}
