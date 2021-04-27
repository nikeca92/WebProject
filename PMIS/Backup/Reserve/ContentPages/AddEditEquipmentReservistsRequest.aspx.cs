using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class AddEditEquipmentReservistsRequest : RESPage
    {
        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "RES_EQUIPRESREQUESTS";
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

        //This is a flag field that says if the screen is opened from the Home screen
        //This is used to navigate the user back to the home screen when using the Back button
        private int FromHome
        {
            get
            {
                int fh = 0;
                if (String.IsNullOrEmpty(this.hfFromHome.Value)
                    || this.hfFromHome.Value == "0")
                {
                    if (Request.Params["fh"] != null)
                        int.TryParse(Request.Params["fh"].ToString(), out fh);

                    this.hfFromHome.Value = hfFromHome.ToString();
                }
                else
                {
                    Int32.TryParse(this.hfFromHome.Value, out fh);
                }

                return fh;
            }

            set
            {
                this.hfFromHome.Value = value.ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSPopulateMilitaryCommands")
            {
                JSPopulateMilitaryCommands();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSAddRequestCommand")
            {
                JSAddRequestCommand();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteRequestCommand")
            {
                JSDeleteRequestCommand();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSPopulateDeliveryMunicipality")
            {
                JSPopulateDeliveryMunicipality();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSPopulateDeliveryCity")
            {
                JSPopulateDeliveryCity();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveRequestCommand")
            {
                JSSaveRequestCommand();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveRequestCommandPosition")
            {
                JSSaveRequestCommandPosition();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSMoveRequestCommandPosition")
            {
                JSMoveRequestCommandPosition();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadRequestCommandPosition")
            {
                JSLoadRequestCommandPosition();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteRequestCommandPosition")
            {
                JSDeleteRequestCommandPosition();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSPopulateMilitaryReportSpecialities")
            {
                JSPopulateMilitaryReportSpecialities();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetImportVacantPositionLightBox")
            {
                JSGetImportVacantPositionLightBox();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSImportVacantPositions")
            {
                JSImportVacantPositions();
                return;
            }

            jsMilitaryUnitSelectorDiv.InnerHtml = @"<script src=""" + Page.ClientScript.GetWebResourceUrl(typeof(MilitaryUnitSelector.MilitaryUnitSelector), "MilitaryUnitSelector.MilitaryUnitSelector.js") + @""" type=""text/javascript""></script>";

            //Highlight the correct item in the menu
            if (EquipmentReservistsRequestId == 0)
                HighlightMenuItems("Equipment", "AddEditEquipmentReservistsRequest");
            else
                HighlightMenuItems("Equipment");

            LnkForceNoChangesCheck(btnSave);

            //Hide the navigation buttons
            HideNavigationControls(btnBack, btnBack2);

            //Load the page for the first time: Load the data on the screen
            if (!IsPostBack)
            {
                LoadDropDowns();
                LoadData();
                SetPageHeader();

                ddMRSType.Attributes.Add("onchange", "RefreshMilitaryReportSpecialities(parseInt(this.value));");

                lblMilitaryUnit.Text = "Заявката е от " + CommonFunctions.GetLabelText("MilitaryUnit") + ":";
            }

            SetupDatePickers();
            SetupPageUI();
        }

        //Set the correct page header (i.e. either New or Edit)
        private void SetPageHeader()
        {
            string header = (EquipmentReservistsRequestId > 0 ? "Редактиране на заявка за окомплектоване с ресурс от резерва" : "Въвеждане на заявка за окомплектоване с ресурс от резерва");

            lblHeaderTitle.InnerHtml = header;
            Page.Title = header;
        }

        //Load any drop-down on the screen
        private void LoadDropDowns()
        {
            PopulateEquipWithResRequestsStatuses();
            PopulateAdministrations();
            PopulateMilitaryRanks();
            PopulateMilitaryReportSpeciality();
        }

        //Populate the statuses drop-down
        private void PopulateEquipWithResRequestsStatuses()
        {
            ddEquipWithResRequestsStatus.Items.Clear();
            ddEquipWithResRequestsStatus.Items.Add(ListItems.GetOptionChooseOne());

            List<EquipWithResRequestsStatus> equipWithResRequestsStatuses = EquipWithResRequestsStatusUtil.GetAllEquipWithResRequestsStatuses(CurrentUser);

            foreach (EquipWithResRequestsStatus equipWithResRequestsStatus in equipWithResRequestsStatuses)
            {
                ListItem li = new ListItem();
                li.Text = equipWithResRequestsStatus.StatusName;
                li.Value = equipWithResRequestsStatus.EquipWithResRequestsStatusId.ToString();

                ddEquipWithResRequestsStatus.Items.Add(li);
            }
        }

        //Populate the administrations drop-down
        private void PopulateAdministrations()
        {
            ddAdministration.Items.Clear();
            ddAdministration.Items.Add(ListItems.GetOptionChooseOne());

            List<Administration> administrations = AdministrationUtil.GetAllAdministrations(CurrentUser);

            foreach (Administration administration in administrations)
            {
                ListItem li = new ListItem();
                li.Text = administration.AdministrationName;
                li.Value = administration.AdministrationId.ToString();

                ddAdministration.Items.Add(li);
            }

            Administration ministryOfDefence = AdministrationUtil.GetMinistryOfDefence(CurrentUser);
            if (ministryOfDefence != null)
                ddAdministration.SelectedValue = ministryOfDefence.AdministrationId.ToString();
        }

        //Populate the military ranks (in a light-box) listbox
        private void PopulateMilitaryRanks()
        {
            ddAvailableRanks.Items.Clear();
            
            List<MilitaryRank> militaryRanks = MilitaryRankUtil.GetAllMilitaryRanks(CurrentUser);

            foreach (MilitaryRank militaryRank in militaryRanks)
            {
                ListItem li = new ListItem();
                li.Text = militaryRank.LongName;
                li.Value = militaryRank.MilitaryRankId.ToString();

                ddAvailableRanks.Items.Add(li);
            }
        }

        //Populate the military reporting specialty types (in a light-box) drop-down
        private void PopulateMilitaryReportSpeciality()
        {
            ddMRSType.Items.Clear();

            List<MilitaryReportSpecialityType> types = MilitaryReportSpecialityTypeUtil.GetAllMilitaryReportSpecialityTypes(CurrentUser);

            foreach (MilitaryReportSpecialityType type in types)
            {
                ListItem li = new ListItem();
                li.Text = type.TypeName;
                li.Value = type.Type.ToString();

                ddMRSType.Items.Add(li);
            }
        }

        //Load the existing data in Edit mode
        private void LoadData()
        {
            //If Edit mode
            if (EquipmentReservistsRequestId > 0)
            {
                EquipmentReservistsRequest equipmentReservistsRequest = EquipmentReservistsRequestUtil.GetEquipmentReservistsRequest(EquipmentReservistsRequestId, CurrentUser);

                txtRequestNumber.Text = equipmentReservistsRequest.RequestNumber;
                txtRequestDate.Text = CommonFunctions.FormatDate(equipmentReservistsRequest.RequestDate);

                if (equipmentReservistsRequest.EquipWithResRequestsStatus != null)
                    ddEquipWithResRequestsStatus.SelectedValue = equipmentReservistsRequest.EquipWithResRequestsStatus.EquipWithResRequestsStatusId.ToString();

                if (equipmentReservistsRequest.MilitaryUnit != null)
                {
                    msMilitaryUnit.SelectedValue = equipmentReservistsRequest.MilitaryUnit.MilitaryUnitId.ToString();
                    msMilitaryUnit.SelectedText = equipmentReservistsRequest.MilitaryUnit.DisplayTextForSelection;
                }

                if (equipmentReservistsRequest.Administration != null)
                    ddAdministration.SelectedValue = equipmentReservistsRequest.Administration.AdministrationId.ToString();

                //Display the Add Request Command button
                btnAddRequestCommandCont.Style.Add("display", "");

                List<string> disabledClientControls = new List<string>();
                List<string> hiddenClientControls = new List<string>();

                //Load the request commands
                StringBuilder requestCommandsHTML = new StringBuilder();

                if (GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS") != UIAccessLevel.Hidden)
                {
                    for (int i = 0; i < equipmentReservistsRequest.RequestCommands.Count; i++)
                    {
                        requestCommandsHTML.Append(RenderRequestCommand(equipmentReservistsRequest.RequestCommands[i], i + 1,
                                                                        disabledClientControls, hiddenClientControls));
                    }
                }

                //Setup page UIItems
                if (GetUIItemAccessLevel("RES_EQUIPRESREQUESTS") != UIAccessLevel.Enabled ||
                    GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT") != UIAccessLevel.Enabled ||
                    GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS") != UIAccessLevel.Enabled ||
                    GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS_ADD") != UIAccessLevel.Enabled)
                {
                    hiddenClientControls.Add("btnAddRequestCommand");
                }

                SetDisabledClientControls(disabledClientControls.ToArray());
                SetHiddenClientControls(hiddenClientControls.ToArray());

                hdnRequestCommandsCount.Value = equipmentReservistsRequest.RequestCommands.Count.ToString();
                hdnVisibleRequestCommandsCount.Value = equipmentReservistsRequest.RequestCommands.Count.ToString();

                divRequestCommands.InnerHtml = requestCommandsHTML.ToString();
            }
            else
            {
                //We need this becasue just right after the save of a new request we go into an Edit mode
                List<string> disabledClientControls = new List<string>();
                List<string> hiddenClientControls = new List<string>();

                //Setup page UIItems
                if (GetUIItemAccessLevel("RES_EQUIPRESREQUESTS") != UIAccessLevel.Enabled ||
                    GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT") != UIAccessLevel.Enabled ||
                    GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS") != UIAccessLevel.Enabled ||
                    GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS_ADD") != UIAccessLevel.Enabled)
                {
                    hiddenClientControls.Add("btnAddRequestCommand");
                }

                SetDisabledClientControls(disabledClientControls.ToArray());
                SetHiddenClientControls(hiddenClientControls.ToArray());
            }
        }

        //Collect the information from the page form
        //Store the data in a object in the memory and use that object when peforming DB operations
        private EquipmentReservistsRequest CollectData()
        {
            EquipmentReservistsRequest equipmentReservistsRequest = new EquipmentReservistsRequest(CurrentUser);

            equipmentReservistsRequest.EquipmentReservistsRequestId = EquipmentReservistsRequestId;
            equipmentReservistsRequest.RequestNumber = txtRequestNumber.Text;
            if (txtRequestDate.Text != "")
                equipmentReservistsRequest.RequestDate = CommonFunctions.ParseDate(txtRequestDate.Text);

            if (ddEquipWithResRequestsStatus.SelectedValue != ListItems.GetOptionChooseOne().Value)
            {
                int equipWithResRequestsStatusId = int.Parse(ddEquipWithResRequestsStatus.SelectedValue);
                equipmentReservistsRequest.EquipWithResRequestsStatus = EquipWithResRequestsStatusUtil.GetEquipWithResRequestsStatus(equipWithResRequestsStatusId, CurrentUser);
            }

            if (msMilitaryUnit.SelectedValue != ListItems.GetOptionChooseOne().Value)
            {
                int militaryUnitId = int.Parse(msMilitaryUnit.SelectedValue);
                equipmentReservistsRequest.MilitaryUnit = MilitaryUnitUtil.GetMilitaryUnit(militaryUnitId, CurrentUser);
            }

            if (ddAdministration.SelectedValue != ListItems.GetOptionChooseOne().Value)
            {
                int administrationId = int.Parse(ddAdministration.SelectedValue);
                equipmentReservistsRequest.Administration = AdministrationUtil.GetAdministration(administrationId, CurrentUser);
            }

            return equipmentReservistsRequest;
        }

        //Save the data
        private void SaveData()
        {
            //First collect the data from the page form
            EquipmentReservistsRequest equipmentReservistsRequest = CollectData();

            //Track the changes into the Audit Trail 
            Change change = new Change(CurrentUser, "RES_EquipmentReservistsRequests");

            //If a successfull save operation then indicate this on the screen; Otherwise alert a warning
            if (EquipmentReservistsRequestUtil.SaveEquipmentReservistsRequest(equipmentReservistsRequest, CurrentUser, change))
            {
                if (EquipmentReservistsRequestId == 0)
                {
                    SetLocationHash("AddEditEquipmentReservistsRequest.aspx?EquipmentReservistsRequestId=" + equipmentReservistsRequest.EquipmentReservistsRequestId.ToString());
                }

                EquipmentReservistsRequestId = equipmentReservistsRequest.EquipmentReservistsRequestId;

                this.lblMessage.CssClass = "SuccessText";
                this.lblMessage.Text = "Записът е успешен";

                SetPageHeader();
                hdnSavedChangesContainer.Value = "tblRequestHeaderSection";
                SetupPageUI();

                if (change.HasEvents)
                {
                    List<FillReservistsRequest> requestReservistsByEquipment = FillReservistsRequestUtil.GetAllFillReservistsRequestByEquipmentRequest(equipmentReservistsRequest.EquipmentReservistsRequestId, CurrentUser);
                    ReservistAppointmentUtil.RefreshReservistAppointment(requestReservistsByEquipment, CurrentUser, change);
                }
            }
            else
            {
                this.lblMessage.CssClass = "ErrorText";
                this.lblMessage.Text = "Записът не е успешен";
            }

            change.WriteLog();
        }

        //Save the form data (first chech if it is valid)
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        //Navigate to the previous page
        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (FromHome != 1)
                Response.Redirect("~/ContentPages/ManageEquipmentReservistsRequests.aspx");
            else
                Response.Redirect("~/ContentPages/Home.aspx");
        }

        // Setup any date picker controls on the page by setting the CSS of the target inputs
        // Note that the date picker CSS is common
        // This makes the calendar controls to appear on the page next to each input
        private void SetupDatePickers()
        {
            txtRequestDate.CssClass = "RequiredInputField " + CommonFunctions.DatePickerCSS();
        }

        // Setup user interface elements according to rights of the user's role
        private void SetupPageUI()
        {
            bool screenHidden = false;
            bool screenDisabled = false;

            if (EquipmentReservistsRequestId == 0) // add mode of page
            {
                screenHidden = GetUIItemAccessLevel("RES_EQUIPRESREQUESTS") != UIAccessLevel.Enabled ||
                               GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_ADD") != UIAccessLevel.Enabled;

                screenDisabled = GetUIItemAccessLevel("RES_EQUIPRESREQUESTS") == UIAccessLevel.Disabled ||
                                 GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_ADD") == UIAccessLevel.Disabled;

                if (screenHidden)
                    RedirectAccessDenied();

                if (screenDisabled)
                {
                    pageDisabledControls.Add(btnSave);
                }

                UIAccessLevel l;

                l = GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_ADD_REQUESTNUM");

                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblRequestNumber);
                    pageDisabledControls.Add(txtRequestNumber);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(lblRequestNumber);
                    pageHiddenControls.Add(txtRequestNumber);
                }

                l = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_ADD_REQUESTDATE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblRequestDate);
                    pageDisabledControls.Add(txtRequestDate);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblRequestDate);
                    this.pageHiddenControls.Add(txtRequestDate);
                }

                l = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_ADD_STATUS");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblEquipWithResRequestsStatus);
                    pageDisabledControls.Add(ddEquipWithResRequestsStatus);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblEquipWithResRequestsStatus);
                    this.pageHiddenControls.Add(ddEquipWithResRequestsStatus);
                }

                l = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_ADD_MILITARYUNIT");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblMilitaryUnit);
                    pageDisabledControls.Add(msMilitaryUnit);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblMilitaryUnit);
                    this.pageHiddenControls.Add(msMilitaryUnit);
                }

                l = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_ADD_ADMINISTRATION");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblAdministration);
                    pageDisabledControls.Add(ddAdministration);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblAdministration);
                    this.pageHiddenControls.Add(ddAdministration);
                }
            }
            else // edit mode of page
            {
                screenHidden = GetUIItemAccessLevel("RES_EQUIPRESREQUESTS") == UIAccessLevel.Hidden ||
                               GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT") == UIAccessLevel.Hidden;

                screenDisabled = GetUIItemAccessLevel("RES_EQUIPRESREQUESTS") == UIAccessLevel.Disabled ||
                                 GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT") == UIAccessLevel.Disabled;

                if (screenHidden)
                    RedirectAccessDenied();

                if (screenDisabled)
                {
                    pageDisabledControls.Add(btnSave);
                }

                UIAccessLevel l;

                l = GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_REQUESTNUM");

                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblRequestNumber);
                    pageDisabledControls.Add(txtRequestNumber);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(lblRequestNumber);
                    pageHiddenControls.Add(txtRequestNumber);
                }

                l = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_REQUESTDATE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblRequestDate);
                    pageDisabledControls.Add(txtRequestDate);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblRequestDate);
                    this.pageHiddenControls.Add(txtRequestDate);
                }

                l = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_STATUS");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblEquipWithResRequestsStatus);
                    pageDisabledControls.Add(ddEquipWithResRequestsStatus);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblEquipWithResRequestsStatus);
                    this.pageHiddenControls.Add(ddEquipWithResRequestsStatus);
                }

                l = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_MILITARYUNIT");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblMilitaryUnit);
                    pageDisabledControls.Add(msMilitaryUnit);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblMilitaryUnit);
                    this.pageHiddenControls.Add(msMilitaryUnit);
                }

                l = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_ADMINISTRATION");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblAdministration);
                    pageDisabledControls.Add(ddAdministration);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblAdministration);
                    this.pageHiddenControls.Add(ddAdministration);
                }
            }

            //Setup the "manually add positions" light-box
            List<string> disabledClientControls = new List<string>();
            List<string> hiddenClientControls = new List<string>();

            UIAccessLevel l_lb;

            l_lb = GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS_POSITION");
            if (l_lb == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPosition");
                disabledClientControls.Add("txtPosition");
            }
            else if (l_lb == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPosition");
                hiddenClientControls.Add("txtPosition");
            }

            l_lb = GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS_MILITARYREPORTSPECIALITY");
            if (l_lb == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblMilitaryReportSpecialityType");
                disabledClientControls.Add("lblMilitaryReportSpeciality");
                disabledClientControls.Add(ddMRSType.ClientID);
                disabledClientControls.Add("ddAvailableMRS");
                hiddenClientControls.Add("btnSelectMRS");
                hiddenClientControls.Add("btnRemoveMRS");
                disabledClientControls.Add("lblSelectedMRS");
                disabledClientControls.Add("ddSelectedMRS");
                disabledClientControls.Add("lblIsPrimaryMilRepSpec");
                disabledClientControls.Add("chkIsPrimaryMilRepSpec");
            }
            else if (l_lb == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilitaryReportSpecialityType");
                hiddenClientControls.Add("lblMilitaryReportSpeciality");
                hiddenClientControls.Add(ddMRSType.ClientID);
                hiddenClientControls.Add("ddAvailableMRS");
                hiddenClientControls.Add("btnSelectMRS");
                hiddenClientControls.Add("btnRemoveMRS");
                hiddenClientControls.Add("lblSelectedMRS");
                hiddenClientControls.Add("ddSelectedMRS");
                hiddenClientControls.Add("lblIsPrimaryMilRepSpec");
                hiddenClientControls.Add("chkIsPrimaryMilRepSpec");
            }

            l_lb = GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS_MILITARYRANK");
            if (l_lb == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblMilitaryRank");
                disabledClientControls.Add(ddAvailableRanks.ClientID);
                hiddenClientControls.Add("btnSelectRanks");
                hiddenClientControls.Add("btnRemoveRanks");
                disabledClientControls.Add("lblSelectedRanks");
                disabledClientControls.Add("ddSelectedRanks");
                disabledClientControls.Add("lblIsPrimaryRank");
                disabledClientControls.Add("chkIsPrimaryRank");
            }
            else if (l_lb == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilitaryRank");
                hiddenClientControls.Add(ddAvailableRanks.ClientID);
                hiddenClientControls.Add("btnSelectRanks");
                hiddenClientControls.Add("btnRemoveRanks");
                hiddenClientControls.Add("lblSelectedRanks");
                hiddenClientControls.Add("ddSelectedRanks");
                hiddenClientControls.Add("lblIsPrimaryRank");
                hiddenClientControls.Add("chkIsPrimaryRank");
            }

            l_lb = GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS_RESERVISTSCOUNT");
            if (l_lb == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblReservistsCount");
                disabledClientControls.Add("txtReservistsCount");
            }
            else if (l_lb == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblReservistsCount");
                hiddenClientControls.Add("txtReservistsCount");
            }

            if (GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS_POSITION") != UIAccessLevel.Enabled &&
                GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS_MILITARYREPORTSPECIALITY") != UIAccessLevel.Enabled &&
                GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS_MILITARYRANK") != UIAccessLevel.Enabled &&
                GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS_RESERVISTSCOUNT") != UIAccessLevel.Enabled)
            {
                hiddenClientControls.Add("btnSaveAddEditRequestCommandPositionLightBox");
            }

            if (screenDisabled)
            {
                hiddenClientControls.Add(btnImgSave.ClientID);
            }

            SetDisabledClientControls(disabledClientControls.ToArray());
            SetHiddenClientControls(hiddenClientControls.ToArray());
        }

        //PopulateMiliataryCommands (ajax call)
        private void JSPopulateMilitaryCommands()
        {
            int militaryUnitId = int.Parse(Request.Form["MiliatryUnitId"]);

            string stat = "";
            string response = "<mc>" +
                                 "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                                 "<name>" + ListItems.GetOptionChooseOne().Text + "</name>" +
                              "</mc>";

            try
            {
                List<MilitaryCommand> militaryCommands = MilitaryCommandUtil.GetMilitaryCommandsByMilitaryUnitAndChildren(CurrentUser, militaryUnitId);

                foreach (MilitaryCommand militaryCommand in militaryCommands)
                {
                    response += "<mc>" +
                                   "<id>" + militaryCommand.MilitaryCommandId.ToString() + "</id>" +
                                   "<name>" + militaryCommand.DisplayTextForSelectionInclVPN + "</name>" +
                                "</mc>";
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

        //Add a new command to the request (ajax call)
        private void JSAddRequestCommand()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_EquipmentReservistsRequests");

                int equipmentReservistsRequestId = int.Parse(Request.Form["EquipmentReservistsRequestId"]);
                int militaryCommandId = int.Parse(Request.Form["MilitaryCommandId"]);
                int commandsCount = int.Parse(Request.Form["RequestCommandsCount"]);

                RequestCommand requestCommand = RequestCommandUtil.AddRequestCommand(CurrentUser,
                                                   equipmentReservistsRequestId, militaryCommandId, change);

                change.WriteLog();

                List<string> disabledClientControls = new List<string>();
                List<string> hiddenClientControls = new List<string>();

                string resultHTML = RenderRequestCommand(requestCommand, commandsCount + 1, disabledClientControls, hiddenClientControls);

                stat = AJAXTools.OK;
                response = "<resultHTML>" + AJAXTools.EncodeForXML(resultHTML) + "</resultHTML>" +
                           "<disabledClientControls>" + AJAXTools.EncodeForXML(string.Join(",", disabledClientControls.ToArray())) + "</disabledClientControls>" +
                           "<hiddenClientControls>" + AJAXTools.EncodeForXML(string.Join(",", hiddenClientControls.ToArray())) + "</hiddenClientControls>";
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

        //Delete a particular command from the reqest (ajax call)
        private void JSDeleteRequestCommand()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_EquipmentReservistsRequests");

                int requestCommandId = int.Parse(Request.Form["RequestCommandId"]);

                //Get all reservists added to that Command
                List<FillReservistsRequest> fillReservistRequests = FillReservistsRequestUtil.GetAllFillReservistsRequestByRequestCommand(requestCommandId, CurrentUser);

                /*
                List<int> deletedReservists = new List<int>();

                //Remove all Reservists from that Command
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
                */

                string msg = "";
                if (fillReservistRequests.Count == 0)
                {
                    RequestCommandUtil.DeleteRequestCommand(CurrentUser, requestCommandId, change);
                    change.WriteLog();
                }
                else
                {
                    msg = "Командата не може да бъде изтрита, поради наличието на резервисти назначени от следните ВО:<br/>";
                    msg += "<div style='max-height: 400px;overflow-y:auto;margin-top:10px;'><ul style='margin-top:-5px; padding-top:3px;'>";

                    foreach (var md in fillReservistRequests.GroupBy(x => x.MilitaryDepartment.MilitaryDepartmentName))
                    {
                        msg += "<li>" + md.Key + "</li>";
                    }

                    msg += "</ul></div>";
                }

                stat = AJAXTools.OK;
                response = "<xml><status>OK</status><msg>" + AJAXTools.EncodeForXML(msg) + "</msg></xml>";
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

        //Populate the Delivery Municipality drop-down when changing the Region (ajax call)
        private void JSPopulateDeliveryMunicipality()
        {
            string stat = "";
            string response = "";

            try
            {
                int regionId = 0;

                if (!String.IsNullOrEmpty(Request.Form["DeliveryRegionId"]))
                    regionId = int.Parse(Request.Form["DeliveryRegionId"]);

                response = "<municipalities>";

                if (regionId == 0 || regionId == int.Parse(ListItems.GetOptionChooseOne().Value))
                    response += "<m>" +
                                 "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                                 "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                                 "</m>";

                List<Municipality> municipalities = MunicipalityUtil.GetMunicipalities(regionId, CurrentUser);

                foreach (Municipality municipality in municipalities)
                {
                    response += "<m>" +
                                "<id>" + municipality.MunicipalityId.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(municipality.MunicipalityName) + "</name>" +
                                "</m>";
                }

                response += "</municipalities>";

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

        //Populate the Delivery City drop-down when changing the Municipality (ajax call)
        private void JSPopulateDeliveryCity()
        {
            string stat = "";
            string response = "";

            try
            {
                int municipalityId = 0;

                if (!String.IsNullOrEmpty(Request.Form["DeliveryMunicipalityId"]))
                    municipalityId = int.Parse(Request.Form["DeliveryMunicipalityId"]);

                response = "<cities>";

                if (municipalityId == 0 || municipalityId == int.Parse(ListItems.GetOptionChooseOne().Value))
                    response += "<c>" +
                                "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                                "</c>";

                List<City> cities = CityUtil.GetCities(municipalityId, CurrentUser);

                foreach (City city in cities)
                {
                    response += "<c>" +
                                "<id>" + city.CityId.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(city.CityName) + "</name>" +
                                "</c>";
                }

                response += "</cities>";

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

        //Save a particular command from the reqest (ajax call)
        private void JSSaveRequestCommand()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_EquipmentReservistsRequests");

                int requestCommandId = int.Parse(Request.Form["RequestCommandId"]);
                int militaryCommandId = int.Parse(Request.Form["MilitaryCommandId"]);
                string militaryCommandSuffix = Request.Form["MilitaryCommandSuffix"];
                int deliveryCityId = int.Parse(Request.Form["DeliveryCityId"]);
                string deliveryPlace = Request.Form["DeliveryPlace"];
                int equipmentReservistsRequestId = int.Parse(Request.Form["EquipmentReservistsRequestId"]);
                string appointmentTimeStr = Request.Form["AppointmentTime"];
                string milReadinessIdStr = Request.Form["MilReadinessId"];
                bool doAppointmentUpdate = Request.Form["DoAppointmentUpdate"] == "1" ? true : false;

                RequestCommand requestCommand = new RequestCommand(CurrentUser);
                requestCommand.RequestCommandId = requestCommandId;
                requestCommand.MilitaryCommand = MilitaryCommandUtil.GetMilitaryCommand(militaryCommandId, CurrentUser);
                requestCommand.EquipmentReservistsRequestId = equipmentReservistsRequestId;
                requestCommand.MilitaryCommandSuffix = militaryCommandSuffix;

                if (!String.IsNullOrEmpty(appointmentTimeStr))
                {
                    requestCommand.AppointmentTime = decimal.Parse(appointmentTimeStr);
                }

                if (!String.IsNullOrEmpty(milReadinessIdStr) && milReadinessIdStr != ListItems.GetOptionChooseOne().Value)
                {
                    requestCommand.MilitaryReadinessId = int.Parse(milReadinessIdStr);
                }

                if (deliveryCityId != int.Parse(ListItems.GetOptionChooseOne().Value))
                {
                    requestCommand.DeliveryCity = CityUtil.GetCity(deliveryCityId, CurrentUser);
                }

                requestCommand.DeliveryPlace = deliveryPlace;

                RequestCommandUtil.SaveRequestCommand(CurrentUser, requestCommand, change);

                if (doAppointmentUpdate && change.HasEvents)
                {
                    List<FillReservistsRequest> requestReservistsByCommand = FillReservistsRequestUtil.GetAllFillReservistsRequestByRequestCommand(requestCommand.RequestCommandId, CurrentUser);
                    ReservistAppointmentUtil.RefreshReservistAppointment(requestReservistsByCommand, CurrentUser, change);
                }

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

        //Save a particular command position (ajax call)
        private void JSSaveRequestCommandPosition()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_EquipmentReservistsRequests");

                int requestCommandPositionId = int.Parse(Request.Form["RequestCommandPositionId"]);
                int requestCommandId = int.Parse(Request.Form["RequestCommandId"]);
                string position = Request.Form["Position"];

                int reservistsCount = int.Parse(Request.Form["ReservistsCount"]);


                RequestCommandPosition requestCommandPosition = new RequestCommandPosition(CurrentUser);

                requestCommandPosition.RequestCommandPositionId = requestCommandPositionId;
                requestCommandPosition.RequestsCommandId = requestCommandId;
                requestCommandPosition.Position = position;
                if (requestCommandPositionId == 0)
                {
                    requestCommandPosition.PositionType = 1;  //Manually added
                }

                requestCommandPosition.MilitaryRanks = new List<CommandPositionMilitaryRank>();
                int militaryRanksCnt = int.Parse(Request.Form["RanksCnt"]);

                for (int i = 1; i <= militaryRanksCnt; i++)
                {
                    string milRankId = Request.Form["RankId_" + i.ToString()];
                    string longName = Request.Form["RankDisplayText_" + i.ToString()];
                    bool isPrimary = int.Parse(Request.Form["IsPrimaryRank_" + i.ToString()]) == 1;

                    MilitaryRank rank = new MilitaryRank();

                    rank.MilitaryRankId = milRankId;
                    rank.LongName = longName;

                    CommandPositionMilitaryRank positionRank = new CommandPositionMilitaryRank();
                    positionRank.Rank = rank;
                    positionRank.IsPrimary = isPrimary;

                    requestCommandPosition.MilitaryRanks.Add(positionRank);
                }

                requestCommandPosition.ReservistsCount = reservistsCount;

                requestCommandPosition.MilitaryReportSpecialities = new List<CommandPositionMilitaryReportSpeciality>();
                int militaryReportSpecialitiesCnt = int.Parse(Request.Form["MRSCnt"]);

                for (int i = 1; i <= militaryReportSpecialitiesCnt; i++)
                {
                    int milReportSpecialityId = int.Parse(Request.Form["MRSId_" + i.ToString()]);
                    string codeAndName = Request.Form["MRSDisplayText_" + i.ToString()];
                    bool isPrimary = int.Parse(Request.Form["IsPrimary_" + i.ToString()]) == 1;

                    MilitaryReportSpeciality mrs = new MilitaryReportSpeciality(CurrentUser);

                    mrs.MilReportSpecialityId = milReportSpecialityId;
                    mrs.CodeAndName = codeAndName;

                    CommandPositionMilitaryReportSpeciality positionSpeciality = new CommandPositionMilitaryReportSpeciality();
                    positionSpeciality.Speciality = mrs;
                    positionSpeciality.IsPrimary = isPrimary;

                    requestCommandPosition.MilitaryReportSpecialities.Add(positionSpeciality);
                }

                bool add = requestCommandPosition.RequestCommandPositionId == 0;
                RequestCommandPositionUtil.SaveRequestCommandPosition(requestCommandPosition, CurrentUser, change);

                if (add)
                    RequestCommandPositionUtil.RearrangeRequestCommandPositions(requestCommandPosition.RequestsCommand.RequestCommandId, CurrentUser);

                if (change.HasEvents)
                {
                    List<FillReservistsRequest> requestReservistsByCommandPosition = FillReservistsRequestUtil.GetAllFillReservistsRequestByReqCommandPosition(requestCommandPosition.RequestCommandPositionId, CurrentUser);
                    ReservistAppointmentUtil.RefreshReservistAppointment(requestReservistsByCommandPosition, CurrentUser, change);
                }
                change.WriteLog();

                int idx = int.Parse(Request.Form["Idx"]);
                string refreshedPositionsTable = RenderRequestCommandPositions(requestCommandPosition.RequestsCommand, idx);

                stat = AJAXTools.OK;
                response = "<refreshedPositionsTable>" + AJAXTools.EncodeForXML(refreshedPositionsTable) + @"</refreshedPositionsTable>";
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

        //Change the order of the Request Command Position
        private void JSMoveRequestCommandPosition()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_EquipmentReservistsRequests");

                int requestCommandID = int.Parse(Request.Form["RequestCommandID"]);
                int requestCommandPositionID_1 = int.Parse(Request.Form["RequestCommandPositionID_1"]);
                int requestCommandPositionID_2 = int.Parse(Request.Form["RequestCommandPositionID_2"]);

                // RequestCommandPosition requestCommandPosition = ReservistAppointmentUtil.getr new RequestCommandPosition(CurrentUser);


                RequestCommandPosition commandPos1 = RequestCommandPositionUtil.GetRequestCommandPosition(CurrentUser, requestCommandPositionID_1);
                RequestCommandPosition commandPos2 = RequestCommandPositionUtil.GetRequestCommandPosition(CurrentUser, requestCommandPositionID_2);
                RequestCommandPositionUtil.SwapRequestCommandPositionsOrder(CurrentUser, change, commandPos1, commandPos2);

                change.WriteLog();

                int idx = int.Parse(Request.Form["Idx"]);
                RequestCommand command = RequestCommandUtil.GetRequestsCommand(CurrentUser, requestCommandID);
                string refreshedPositionsTable = RenderRequestCommandPositions(command, idx);

                stat = AJAXTools.OK;
                response = "<refreshedPositionsTable>" + AJAXTools.EncodeForXML(refreshedPositionsTable) + @"</refreshedPositionsTable>";
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

        //Load a particular command position (ajax call)
        private void JSLoadRequestCommandPosition()
        {
            string stat = "";
            string response = "";

            try
            {
                int requestCommandPositionId = int.Parse(Request.Form["RequestCommandPositionId"]);

                RequestCommandPosition requestCommandPosition = RequestCommandPositionUtil.GetRequestCommandPosition(CurrentUser, requestCommandPositionId);
                List<FillReservistsRequest> fillReservistRequests = FillReservistsRequestUtil.GetAllFillReservistsRequestByReqCommandPosition(requestCommandPosition.RequestCommandPositionId, CurrentUser);

                string militaryRanks = "";
                foreach (CommandPositionMilitaryRank rank in requestCommandPosition.MilitaryRanks)
                {
                    militaryRanks += "<rank>" +
                                     "<id>" + rank.Rank.MilitaryRankId.ToString() + "</id>" +
                                     "<displayName>" + AJAXTools.EncodeForXML(rank.Rank.LongName) + "</displayName>" +
                                     "<isPrimary>" + (rank.IsPrimary ? "1" : "0") + "</isPrimary>" +
                                     "</rank>";
                }

                string militaryReportSpecialities = "";
                foreach (CommandPositionMilitaryReportSpeciality speciality in requestCommandPosition.MilitaryReportSpecialities)
                {
                    militaryReportSpecialities += "<speciality>" +
                                                  "<id>" + speciality.Speciality.MilReportSpecialityId.ToString() + "</id>" +
                                                  "<displayName>" + AJAXTools.EncodeForXML(speciality.Speciality.CodeAndName) + "</displayName>" +
                                                  "<isPrimary>" + (speciality.IsPrimary ? "1" : "0") + "</isPrimary>" +
                                                  "</speciality>";
                }

                stat = AJAXTools.OK;
                response = @"<requestCommandPosition>
                                <requestCommandPositionID>" + requestCommandPosition.RequestCommandPositionId.ToString() + @"</requestCommandPositionID>
                                <requestsCommandID>" + requestCommandPosition.RequestsCommandId.ToString() + @"</requestsCommandID>
                                <position>" + AJAXTools.EncodeForXML(requestCommandPosition.Position) + @"</position>
                                <militaryRanks>" + militaryRanks + @"</militaryRanks>
                                <reservistsCount>" + requestCommandPosition.ReservistsCount.ToString() + @"</reservistsCount>
                                <positionType>" + requestCommandPosition.PositionType.ToString() + @"</positionType>
                                <milReportSpecialities>" + militaryReportSpecialities + @"</milReportSpecialities>
                                <FillReservistRequestsCnt>" + (fillReservistRequests != null ? fillReservistRequests.Count : 0) + @"</FillReservistRequestsCnt>
                                <IsMilitaryReportSpecialityEnabled>" + (this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS_MILITARYREPORTSPECIALITY") == UIAccessLevel.Enabled ? "1" : "0") + @"</IsMilitaryReportSpecialityEnabled> 
                             </requestCommandPosition>";
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

        //Delete a particular command position (ajax call)
        private void JSDeleteRequestCommandPosition()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_EquipmentReservistsRequests");
                int requestCommandPositionId = int.Parse(Request.Form["RequestCommandPositionId"]);
                int requestsCommandId = int.Parse(Request.Form["RequestCommandId"]);

                //Get all reservists added to that position
                List<FillReservistsRequest> fillReservistRequests = FillReservistsRequestUtil.GetAllFillReservistsRequestByReqCommandPosition(requestCommandPositionId, CurrentUser);
                /*List<int> deletedReservists = new List<int>();

                //Remove all Reservists from that Position
                foreach (FillReservistsRequest fillReservistRequest in fillReservistRequests)
                {
                    FillReservistsRequestUtil.DeleteRequestCommandReservist(fillReservistRequest.FillReservistsRequestID, fillReservistRequest.MilitaryDepartmentID, CurrentUser, change);

                    //Just in case: clear the status and the appointment for each reservist only once (note each reservist should be added only once to a particular position)
                    if (!deletedReservists.Contains(fillReservistRequest.ReservistID))
                    {
                        //Change the current Military Reporting Status of each reservist
                        ReservistMilRepStatusUtil.SetMilRepStatusTo_FREE(fillReservistRequest.ReservistID, CurrentUser, change);

                        //Clear the current Mobilization Appointment for each Reservist
                        ReservistAppointmentUtil.ClearTheCurrentReservistAppointmentByReservist(fillReservistRequest.ReservistID, CurrentUser, change);

                        deletedReservists.Add(fillReservistRequest.ReservistID);
                    }
                }
                */

                string msg = "";
                if (fillReservistRequests.Count == 0)
                {
                    RequestCommandPositionUtil.DeleteRequestCommandPosition(CurrentUser, requestCommandPositionId, change);
                    RequestCommandPositionUtil.RearrangeRequestCommandPositions(requestsCommandId, CurrentUser);
                    change.WriteLog();
                }
                else
                {
                    msg = "Длъжността не може да бъде изтрита, поради наличието на резервисти назначени от следните ВО:<br/>";
                    msg += "<div style='max-height: 400px;overflow-y:auto;margin-top:10px;'><ul style='margin-top:-5px; padding-top:3px;'>";

                    foreach (var md in fillReservistRequests.GroupBy(x => x.MilitaryDepartment.MilitaryDepartmentName))
                    {
                        msg += "<li>" + md.Key + "</li>";
                    }

                    msg += "</ul></div>";
                }


                int idx = int.Parse(Request.Form["Idx"]);
                RequestCommand reqestCommand = RequestCommandUtil.GetRequestsCommand(CurrentUser, requestsCommandId);
                string refreshedPositionsTable = RenderRequestCommandPositions(reqestCommand, idx);

                stat = AJAXTools.OK;
                response = "<msg>" + AJAXTools.EncodeForXML(msg) + "</msg><refreshedPositionsTable>" + AJAXTools.EncodeForXML(refreshedPositionsTable) + @"</refreshedPositionsTable>";
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

        //Populate the Military Report Specialities list-box (ajax call)
        private void JSPopulateMilitaryReportSpecialities()
        {
            string stat = "";
            string response = "";

            try
            {
                int type = 1;

                if (!String.IsNullOrEmpty(Request.Form["Type"]))
                    type = int.Parse(Request.Form["Type"]);

                response = "<mrs>";

                List<MilitaryReportSpeciality> mrs = MilitaryReportSpecialityUtil.GetMilitaryReportSpecialitiesByType(CurrentUser, type);

                foreach (MilitaryReportSpeciality s in mrs)
                {
                    response += "<s>" +
                                "<id>" + s.MilReportSpecialityId.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(s.CodeAndName) + "</name>" +
                                "</s>";
                }

                response += "</mrs>";

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

        // Return contents for import vacant position light box by ajax request
        private void JSGetImportVacantPositionLightBox()
        {
            string response = "";
            response += GetImportVacantPositionLightBoxHtml();

            AJAX a = new AJAX(AJAXTools.EncodeForXML(response), this.Response);
            a.Write();
            Response.End();
        }

        // Add vacant positions by ajax request
        private void JSImportVacantPositions()
        {
            string response = "";

            int requestCommandId = int.Parse(Request.Params["RequestsCommandId"]);
            int counter = int.Parse(Request.Params["Count"]);
            int importedCount = 0;

            for (int i = 1; i < counter; i++)
            {
                if (!string.IsNullOrEmpty(Request.Params["Positions" + i]))
                {
                    int maxVSST_ID = int.Parse(Request.Params["MaxVSST_ID" + i]);
                    int positionsCount = int.Parse(Request.Params["Positions" + i]);


                    //Track the changes into the Audit Trail 
                    Change change = new Change(CurrentUser, "RES_EquipmentReservistsRequests");

                    RequestCommandPositionUtil.ImportVacantPosition(CurrentUser, requestCommandId, maxVSST_ID, positionsCount, change);

                    change.WriteLog();

                    importedCount++;
                }
            }

            RequestCommandPositionUtil.RearrangeRequestCommandPositions(requestCommandId, CurrentUser);

            int idx = int.Parse(Request.Form["Idx"]);
            RequestCommand requestCommand = RequestCommandUtil.GetRequestsCommand(CurrentUser, requestCommandId);
            string refreshedPositionsTable = RenderRequestCommandPositions(requestCommand, idx);

            response = "<refreshedPositionsTable>" + AJAXTools.EncodeForXML(refreshedPositionsTable) + @"</refreshedPositionsTable>" +
                       "<importedCount>" + importedCount.ToString() + "</importedCount>";

            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();
        }

        private string RenderRequestCommand(RequestCommand requestCommand, int idx,
                                            List<string> disabledClientControls,
                                            List<string> hiddenClientControls)
        {
            string idxStr = idx.ToString();

            string htmlDelete = "";

            if (requestCommand.CanDelete &&
               GetUIItemAccessLevel("RES_EQUIPRESREQUESTS") == UIAccessLevel.Enabled &&
               GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT") == UIAccessLevel.Enabled &&
               GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS") == UIAccessLevel.Enabled &&
               GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS_DELETE") == UIAccessLevel.Enabled)
                htmlDelete = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на тази команда' class='GridActionIcon' onclick='DeleteRequestCommand(" + requestCommand.RequestCommandId.ToString() + ", " + idxStr + @");'  />";

            string htmlSave = "";

            if (GetUIItemAccessLevel("RES_EQUIPRESREQUESTS") == UIAccessLevel.Enabled &&
                GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT") == UIAccessLevel.Enabled &&
                GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS") == UIAccessLevel.Enabled &&
                GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS_EDIT") == UIAccessLevel.Enabled)
                htmlSave = "<img id='imgBtnSaveReqCmd" + idx + "' src='../Images/save.png' alt='Запис' title='Запис' class='GridActionIcon' style='Position: relative; top: -1px;' data-requestCommandId='" + requestCommand.RequestCommandId.ToString() + "' data-idx='" + idxStr + @"' onclick='CallSaveRequestCommand(this, 1);'  />&nbsp;";

            //Load Delivery City information
            City deliveryCity = requestCommand.DeliveryCity;
            Region deliveryRegion = null;
            Municipality deliveryMunicipality = null;

            if (deliveryCity != null)
            {
                deliveryRegion = deliveryCity.Region;
                deliveryMunicipality = deliveryCity.Municipality;
            }

            List<Region> listRegions = RegionUtil.GetRegions(CurrentUser);
            List<IDropDownItem> ddiRegions = new List<IDropDownItem>();

            int selectedIdx = -1;
            foreach (Region region in listRegions)
            {
                ddiRegions.Add(region);

                if (selectedIdx == -1 &&
                   deliveryRegion != null &&
                   region.RegionId == deliveryRegion.RegionId)
                {
                    selectedIdx = listRegions.IndexOf(region);
                }
            }

            IDropDownItem selectedRegion = (selectedIdx > -1 ? ddiRegions[selectedIdx] : null);

            // Generates html for drop down list
            string regionsHTML = ListItems.GetDropDownHtml(ddiRegions, null, "DeliveryRegion" + idxStr, true, selectedRegion, "DeliveryRegionChange(this, " + idxStr + @")", "style='width: 160px;'");


            List<Municipality> listMunicipalities = new List<Municipality>();

            if (deliveryRegion != null)
            {
                listMunicipalities = MunicipalityUtil.GetMunicipalities(deliveryRegion.RegionId, CurrentUser);
            }

            List<IDropDownItem> ddiMunicipalities = new List<IDropDownItem>();

            selectedIdx = -1;
            foreach (Municipality municipality in listMunicipalities)
            {
                ddiMunicipalities.Add(municipality);

                if (selectedIdx == -1 &&
                   deliveryMunicipality != null &&
                   municipality.MunicipalityId == deliveryMunicipality.MunicipalityId)
                {
                    selectedIdx = listMunicipalities.IndexOf(municipality);
                }
            }

            IDropDownItem selectedMunicipality = (selectedIdx > -1 ? ddiMunicipalities[selectedIdx] : null);

            // Generates html for drop down list
            string municipalitiesHTML = ListItems.GetDropDownHtml(ddiMunicipalities, null, "DeliveryMunicipality" + idxStr, ddiMunicipalities.Count == 0, selectedMunicipality, "DeliveryMunicipalityChange(this, " + idxStr + @")", "style='width: 160px;'");


            List<City> listCities = new List<City>();

            if (deliveryMunicipality != null)
            {
                listCities = CityUtil.GetCities(deliveryMunicipality.MunicipalityId, CurrentUser);
            }

            List<IDropDownItem> ddiCities = new List<IDropDownItem>();

            selectedIdx = -1;
            foreach (City city in listCities)
            {
                ddiCities.Add(city);

                if (selectedIdx == -1 &&
                   deliveryCity != null &&
                   city.CityId == deliveryCity.CityId)
                {
                    selectedIdx = listCities.IndexOf(city);
                }
            }

            IDropDownItem selectedCity = (selectedIdx > -1 ? ddiCities[selectedIdx] : null);

            // Generates html for drop down list
            string citiesHTML = ListItems.GetDropDownHtml(ddiCities, null, "DeliveryCity" + idxStr, ddiCities.Count == 0, selectedCity, "", "style='width: 160px;'");


            List<MilitaryReadiness> listMilReadiness = MilitaryReadinessUtil.GetAllMilitaryReadiness(CurrentUser);
            List<IDropDownItem> ddiMilReadiness = new List<IDropDownItem>();

            selectedIdx = -1;
            foreach (MilitaryReadiness milReadiness in listMilReadiness)
            {
                ddiMilReadiness.Add(milReadiness);

                if (selectedIdx == -1 &&
                    requestCommand.MilitaryReadinessId.HasValue &&
                    requestCommand.MilitaryReadinessId.Value == milReadiness.MilReadinessId)
                {
                    selectedIdx = listMilReadiness.IndexOf(milReadiness);
                }
            }

            IDropDownItem selectedMilReadiness = (selectedIdx > -1 ? ddiMilReadiness[selectedIdx] : null);

            // Generates html for drop down list
            string milReadinessHTML = ListItems.GetDropDownHtml(ddiMilReadiness, null, "ddMilReadiness" + idxStr, true, selectedMilReadiness, "", "style='width: 110px;'");


            string html = @"<div id=""divCommand" + idxStr + @""" style='padding-top: " + (idx == 1 ? "2" : "25") + @"px;'>
                                <center>
                                <fieldset style=""width: 880px; padding: 0px;"">
                                    <table class=""InputRegion"" style=""width: 880px; margin-top: 0px;"">
                                       <tr>
                                          <td style=""text-align: left; vertical-align: top; width: 800px;"">
                                             <div>
                                                <span id='lblMilitaryCommand" + idxStr + @"' class='InputLabel'>Команда:</span>
                                                <span id='lblMilitaryCommandValue" + idxStr + @"' class='ReadOnlyValue'>" + CommonFunctions.HtmlEncoding(requestCommand.MilitaryCommand.DisplayTextForSelection) + @"</span>
                                                <input type='text' id='txtMilitaryCommandSuffix" + idxStr + @"' class='InputField' style='width: 30px;' value=""" + CommonFunctions.HtmlEncoding(requestCommand.MilitaryCommandSuffix) + @""" maxlength=""20"" />

                                                <span style='padding: 30px;'>&nbsp;</span>

                                                <span class='InputLabel' id='lblAppointmentTime" + idxStr + @"' >Време за явяване:</span>
                                                <input type='text' id='txtAppointmentTime" + idxStr + @"' value=""" + CommonFunctions.HtmlEncoding(requestCommand.AppointmentTime.HasValue ? requestCommand.AppointmentTime.ToString() : "") + @""" class='InputField' style='width: 40px;' />
                                                <span class='InputLabel' id='lblAppointmentTimeMeasure" + idxStr + @"' >часа</span>

                                                <span style='padding: 30px;'>&nbsp;</span>

                                                <span class='InputLabel' id='lblMilReadiness" + idxStr + @"' >Готовност:</span>
                                                " + milReadinessHTML + @"
                                             </div>

                                             <div style='height: 2px;'></div>

                                             <div style='text-align: center;'>
                                                <table style='margin: 0 auto;'>
                                                   <tr>
                                                      <td id='tdDeliveryPlace" + idxStr + @"'>
                                                        <fieldset style=""width: 540px; padding: 10px;"">
                                                        <legend style=""color: #0B4489; font-weight: bold; font-size: 1.1em;"">Място за доставяне</legend>
                                                           <table>
                                                              <tr>
                                                                 <td style='text-align: right; width: 130px;'>
                                                                    <span class='InputLabel' id='lblDeliveryRegion" + idxStr + @"'>Област:</span>
                                                                 </td>
                                                                 <td style='text-align: left; width: 160px'>
                                                                    " + regionsHTML + @"
                                                                 </td>
                                                                 <td style='text-align: right; width: 100px;'>
                                                                    <span class='InputLabel' id='lblDeliveryMunicipality" + idxStr + @"'>Община:</span>
                                                                 </td>
                                                                 <td style='text-align: left; width: 160px'>
                                                                    " + municipalitiesHTML + @"
                                                                 </td>
                                                              </tr>
                                                              <tr>
                                                                 <td style='text-align: right;'>
                                                                    <span class='InputLabel' id='lblDeliveryCity" + idxStr + @"'>Населено място:</span>
                                                                 </td>
                                                                 <td style='text-align: left;'>
                                                                    " + citiesHTML + @"
                                                                 </td>
                                                                 <td colspan='2' style='text-align: right;'>
                                                                    <input type='text' id='txtDeliveryPlace" + idxStr + @"' class='InputField' style='width: 240px;' value=""" + CommonFunctions.HtmlEncoding(requestCommand.DeliveryPlace) + @""" maxlength=""500"" />
                                                                 </td>
                                                              </tr>
                                                           </table>
                                                        </fieldset>
                                                      </td>
                                                   </tr>
                                                </table>
                                             </div>

                                             <div style='height: 20px; text-align: center;'>
                                                <span id='lblMessage" + idxStr + @"'></span>
                                             </div>

                                             <div style='text-align: center;'>
                                                <table style='margin: 0 auto;'>
                                                   <tr>
                                                      <td style='text-align: right; Position: relative; top: 4px;'>
                                                         <img id=""btnAddPositionManually" + idxStr + @""" src='../Images/data_new.png' alt='Въвеждане на нова длъжност' title='Въвеждане на нова длъжност' class='GridActionIcon' style='width: 18px; height: 18px;' onclick='AddPositionManually(" + requestCommand.RequestCommandId.ToString() + ", " + idxStr + @");'  />
                                                         <img id=""btnImportPositions" + idxStr + @""" src='../Images/data_into.png' alt='Импорт на вакантни длъжности' title='Импорт на вакантни длъжности' class='GridActionIcon' style='width: 18px; height: 18px;' onclick='ImportVacantPositions(" + requestCommand.RequestCommandId.ToString() + ", " + idxStr + @");'  />
                                                      </td>
                                                   </tr>
                                                   <tr>
                                                      <td id='tdRequestCommandPositionsCont" + idxStr + @"'>
                                                         " + RenderRequestCommandPositions(requestCommand, idx) + @"
                                                      </td>
                                                   </tr>
                                                </table>
                                             </div>

                                             <div style='height: 20px; text-align: center;'>
                                                <span id='lblMessagePositions" + idxStr + @"'></span>
                                             </div>
                                          </td>
                                          <td style=""text-align: right; vertical-align: top; width: 80px;"">
                                             " + htmlSave + htmlDelete + @"
                                          </td>
                                       </tr>
                                    </table>
                                </fieldset>
                                </center>

                                <input type='hidden' id='hdnMilitaryCommandName" + idxStr + @"' value=""" + CommonFunctions.HtmlEncoding(requestCommand.MilitaryCommand.DisplayTextForSelection) + @""" />
                                <input type='hidden' id='hdnMilitaryCommandId" + idxStr + @"' value=""" + requestCommand.MilitaryCommand.MilitaryCommandId.ToString() + @""" />
                            </div>";

            //Setup UIItems logic
            bool commandsDisabled = GetUIItemAccessLevel("RES_EQUIPRESREQUESTS") == UIAccessLevel.Disabled ||
                                    GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT") == UIAccessLevel.Disabled ||
                                    GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS") == UIAccessLevel.Disabled ||
                                    GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS_EDIT") == UIAccessLevel.Disabled;

            UIAccessLevel l;

            l = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS_EDIT_SUFFIX");
            if (l == UIAccessLevel.Disabled || commandsDisabled)
            {
                disabledClientControls.Add("txtMilitaryCommandSuffix" + idxStr);
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("txtMilitaryCommandSuffix" + idxStr);
            }

            l = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS_EDIT_APTTIME");
            if (l == UIAccessLevel.Disabled || commandsDisabled)
            {
                disabledClientControls.Add("lblAppointmentTime" + idxStr);
                disabledClientControls.Add("txtAppointmentTime" + idxStr);
                disabledClientControls.Add("lblAppointmentTimeMeasure" + idxStr);
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblAppointmentTime" + idxStr);
                hiddenClientControls.Add("txtAppointmentTime" + idxStr);
                hiddenClientControls.Add("lblAppointmentTimeMeasure" + idxStr);
            }

            l = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS_EDIT_MILREADINESS");
            if (l == UIAccessLevel.Disabled || commandsDisabled)
            {
                disabledClientControls.Add("lblMilReadiness" + idxStr);
                disabledClientControls.Add("ddMilReadiness" + idxStr);
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilReadiness" + idxStr);
                hiddenClientControls.Add("ddMilReadiness" + idxStr);
            }

            l = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS_EDIT_DELIVERYCITY");
            if (l == UIAccessLevel.Disabled || commandsDisabled)
            {
                disabledClientControls.Add("lblDeliveryRegion" + idxStr);
                disabledClientControls.Add("DeliveryRegion" + idxStr);
                disabledClientControls.Add("lblDeliveryMunicipality" + idxStr);
                disabledClientControls.Add("DeliveryMunicipality" + idxStr);
                disabledClientControls.Add("lblDeliveryCity" + idxStr);
                disabledClientControls.Add("DeliveryCity" + idxStr);
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblDeliveryRegion" + idxStr);
                hiddenClientControls.Add("DeliveryRegion" + idxStr);
                hiddenClientControls.Add("lblDeliveryMunicipality" + idxStr);
                hiddenClientControls.Add("DeliveryMunicipality" + idxStr);
                hiddenClientControls.Add("lblDeliveryCity" + idxStr);
                hiddenClientControls.Add("DeliveryCity" + idxStr);
            }

            l = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS_EDIT_DELIVERYPLACE");
            if (l == UIAccessLevel.Disabled || commandsDisabled)
            {
                disabledClientControls.Add("txtDeliveryPlace" + idxStr);
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("txtDeliveryPlace" + idxStr);
            }

            if (this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS_EDIT_DELIVERYCITY") == UIAccessLevel.Hidden &&
                this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS_EDIT_DELIVERYPLACE") == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("tdDeliveryPlace" + idxStr);
            }

            if (commandsDisabled ||
                this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS") != UIAccessLevel.Enabled)
            {
                hiddenClientControls.Add("btnAddPositionManually" + idxStr);
                hiddenClientControls.Add("btnImportPositions" + idxStr);
            }

            return html;
        }

        private string RenderRequestCommandPositions(RequestCommand requestCommand, int idx)
        {
            bool IsPositionsHidden = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS") == UIAccessLevel.Hidden;

            if (IsPositionsHidden)
                return "";

            bool IsPositionHidden = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS_POSITION") == UIAccessLevel.Hidden;
            bool IsMilitaryReportSpecialityHidden = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS_MILITARYREPORTSPECIALITY") == UIAccessLevel.Hidden;
            bool IsMilitaryRankHidden = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS_MILITARYRANK") == UIAccessLevel.Hidden;
            bool IsReservistsCountHidden = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS_RESERVISTSCOUNT") == UIAccessLevel.Hidden;

            StringBuilder html = new StringBuilder();

            html.Append(@"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                             <thead>
                                <tr>
                                           <th style='width: 15px;vertical-align: bottom; border-right:0px;'>&nbsp;</th>
                                           
                                           <th style='width: 20px; vertical-align: bottom;'>№</th>
                " + (!IsPositionHidden ? @"<th style='width: 160px; vertical-align: bottom;'>Длъжност</th>" : "") + @"
" + (!IsMilitaryReportSpecialityHidden ? @"<th style='width: 330px; vertical-align: bottom;'>ВОС</th>" : "") + @"                    
            " + (!IsMilitaryRankHidden ? @"<th style='width: 90px; vertical-align: bottom;'>Звание</th>" : "") + @"
         " + (!IsReservistsCountHidden ? @"<th style='width: 90px; vertical-align: bottom;'>Запасни</th>" : "") + @"
                                           <th style='width: 50px;vertical-align: bottom;'>&nbsp;</th>
                                           
                                </tr>
                             </thead>");

            int counter = 0;

            foreach (RequestCommandPosition position in requestCommand.RequestCommandPositions)
            {
                counter++;

                string deleteHTML = "";

                if (position.CanDelete)
                {
                    if (GetUIItemAccessLevel("RES_EQUIPRESREQUESTS") == UIAccessLevel.Enabled &&
                        GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT") == UIAccessLevel.Enabled &&
                        GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS") == UIAccessLevel.Enabled &&
                        GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS") == UIAccessLevel.Enabled
                        )
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на тази длъжност' class='GridActionIcon' onclick='DeleteRequestCommandPosition(" + position.RequestsCommandId.ToString() + ", " + position.RequestCommandPositionId.ToString() + ", " + idx.ToString() + @");' />";
                }

                string editHTML = "";

                if (
                    GetUIItemAccessLevel("RES_EQUIPRESREQUESTS") == UIAccessLevel.Enabled &&
                    GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT") == UIAccessLevel.Enabled &&
                    GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS") == UIAccessLevel.Enabled &&
                    GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS") == UIAccessLevel.Enabled)
                    editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране на длъжността' class='GridActionIcon' onclick='EditRequestCommandPosition(" + position.RequestsCommandId.ToString() + ", " + position.RequestCommandPositionId.ToString() + ", " + idx.ToString() + @");' />";

                string moveUpHTML = "";
                string moveDownHTML = "";

                if (
                  GetUIItemAccessLevel("RES_EQUIPRESREQUESTS") == UIAccessLevel.Enabled &&
                  GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT") == UIAccessLevel.Enabled &&
                  GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS") == UIAccessLevel.Enabled &&
                  GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS") == UIAccessLevel.Enabled)
                {
                    if (counter != 1)
                    {
                        moveUpHTML += "<img src='../Images/move_up.gif' alt='Преместване нагоре' title='Преместване нагоре' class='GridActionIcon' onclick='MoveRequestCommandPosition(" + idx + ", " + position.RequestsCommandId.ToString() + ", " + position.RequestCommandPositionId.ToString() + ", " + requestCommand.RequestCommandPositions[counter - 2].RequestCommandPositionId.ToString() + @");' />";
                    }

                    if (counter != requestCommand.RequestCommandPositions.Count)
                    {
                        moveDownHTML += "<img src='../Images/move_down.gif' alt='Преместване надолу' title='Преместване надолу' class='GridActionIcon' onclick='MoveRequestCommandPosition(" + idx + ", " + position.RequestsCommandId.ToString() + ", " + position.RequestCommandPositionId.ToString() + ", " + requestCommand.RequestCommandPositions[counter].RequestCommandPositionId.ToString() + @");' />";
                    }
                }

                string moveHTML = moveUpHTML;
                moveHTML += (!string.IsNullOrEmpty(moveHTML) ? "<br>" : "");
                moveHTML += moveDownHTML;

                StringBuilder specialities = new StringBuilder();
                foreach (CommandPositionMilitaryReportSpeciality speciality in position.MilitaryReportSpecialities)
                {
                    string shortDisplayName = speciality.Speciality.CodeAndName.Length > 40 ? speciality.Speciality.CodeAndName.Substring(0, 40) + "..." : speciality.Speciality.CodeAndName;
                    specialities.Append("<div style='cursor: default;" + (speciality.IsPrimary ? "font-weight: bold;" : "") + "' title='" + speciality.Speciality.CodeAndName + "'>" + shortDisplayName + "</div>");
                }

                StringBuilder ranks = new StringBuilder();
                foreach (CommandPositionMilitaryRank rank in position.MilitaryRanks)
                {
                    string shortDisplayName = rank.Rank.LongName.Length > 40 ? rank.Rank.LongName.Substring(0, 40) + "..." : rank.Rank.LongName;
                    ranks.Append("<div style='cursor: default;" + (rank.IsPrimary ? "font-weight: bold;" : "") + "' title='" + rank.Rank.LongName + "'>" + shortDisplayName + "</div>");
                }

                html.Append(@"<tr style='vertical-align: middle; height:20px;' class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                           <td style='text-align: left; vertical-align: top;border-right:0px;'>" + moveHTML + @"</td>
                                           <td style='text-align: center; vertical-align: top;'>" + counter.ToString() + @"</td>
                " + (!IsPositionHidden ? @"<td style='text-align: left; vertical-align: top;'>" + position.Position + @"</td>" : "") + @"
" + (!IsMilitaryReportSpecialityHidden ? @"<td style='text-align: left; vertical-align: top;'>" + specialities.ToString() + @"</td>" : "") + @"                    
            " + (!IsMilitaryRankHidden ? @"<td style='text-align: left; vertical-align: top;'>" + ranks + @"</td>" : "") + @"
         " + (!IsReservistsCountHidden ? @"<td style='text-align: left; vertical-align: top;'>" + position.ReservistsCount.ToString() + @"</td>" : "") + @"
                                           <td style='text-align: left; vertical-align: top;'>" + editHTML + deleteHTML + @"</td>
                                          
                              </tr>
                             ");
            }

            html.Append("</table>");

            return html.ToString();
        }

        // Generate contents for add vacancy position light box
        private string GetImportVacantPositionLightBoxHtml()
        {
            bool IsPositionHidden = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS_POSITION") == UIAccessLevel.Hidden;
            bool IsMilitaryReportSpecialityHidden = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS_MILITARYREPORTSPECIALITY") == UIAccessLevel.Hidden;
            bool IsMilitaryRankHidden = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS_MILITARYRANK") == UIAccessLevel.Hidden;
            bool IsReservistsCountHidden = this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS_RESERVISTSCOUNT") == UIAccessLevel.Hidden;


            string html = "";

            string htmlNoResults = "";

            List<VacantPosition> availablePositions = new List<VacantPosition>();
            string position = "";
            int pageIndex = 1; //Default
            int pageLength = int.Parse(Config.GetWebSetting("RowsPerPage"));
            int allRows = 0;
            int maxPage = 1;
            int orderBy = 1; //Default

            if (Request.Form["Position"] != null && Request.Form["PageIndex"] != null && Request.Form["OrderBy"] != null && Request.Form["MaxPage"] != null)
            {
                position = (Request.Form["Position"] != null ? Request.Form["Position"] : "");

                pageIndex = int.Parse(Request.Form["PageIndex"]);
                orderBy = int.Parse(Request.Form["OrderBy"]);
            }

            int requestCommandId = int.Parse(Request.Form["RequestCommandId"]);
            RequestCommand requestCommand = RequestCommandUtil.GetRequestsCommand(CurrentUser, requestCommandId);

            allRows = VacantPositionUtil.GetAllVacantPositionsCount(position, requestCommand, CurrentUser);
            maxPage = allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            availablePositions = VacantPositionUtil.GetAllVacantPositions(position, requestCommand, orderBy, pageIndex, pageLength, CurrentUser);

            // No data found
            if (availablePositions.Count == 0)
            {
                htmlNoResults = "Няма намерени резултати";
            }

            //Set filter section
            html += @"<center>
                        <table width='100%' style='border-collapse: collapse; vertical-align: middle; color: #0B449D;'>
                        <tr>
                            <td align='right' style='width: 10%;'>
                                <span class='InputLabel' style='padding-left: 10px'>Команда:</span>
                            </td>
                            <td align='left' style='width: 10%;'>
                                <span class='ReadOnlyValue'>" + Request.Form["LightBoxMilitaryCommandName"] + " " + Request.Form["LightBoxMilitaryCommandSuffix"] + @"</span>
                            </td>
                            <td style='width: 80%; text-align: left; padding-left: 30px;'>
                            </td>
                        </tr>
                        <tr style='height: 30px'>
                            <td align='right' style='width: 10%;'>
                                <span class='InputLabel' style='padding-left: 10px'>Длъжност:</span>
                            </td>
                            <td align='left' style='width: 10%;'>
                                <input type='text' id='txtPositionFilt' UnsavedCheckSkipMe='true' class='InputField' value='" + CommonFunctions.HtmlEncoding(position) + @"'></input>
                            </td>
                            <td style='width: 80%; text-align: left; padding-left: 30px;'>
                                <div id='btnSearch' class='Button' onclick='FilterImportVacantPositionsLightBox()'><i></i><div style='width:70px; padding-left:5px;'>Покажи</div><b></b></div>
                            </td>
                        </tr>
                      </table>";
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
            html += @"<div style='min-height: 150px; margin-bottom: 10px; max-height: 400px; overflow-y: scroll;'>

                        <input type='hidden' id='hdnOrderBy' value='" + orderBy + @"' UnsavedCheckSkipMe='true' />
                        <input type='hidden' id='hdnPageIndex' value='" + pageIndex + @"' UnsavedCheckSkipMe='true' />
                        <input type='hidden' id='hdnPageMaxPage' value='" + maxPage + @"' UnsavedCheckSkipMe='true' />

                        <span class='HeaderText'>Избор на вакантни длъжности</span><br /><br /><br />

                        <div style='text-align: center;'>
                           <div style='display: inline; Position: relative; top: -10px;'>
                              <img id='btnTableFirst' " + btnFirst + @" alt='Първа страница' title='Първа страница' class='PaginationButton' onclick=""BtnPagingClick('btnFirst');"" />
                              <img id='btnTablePrev' " + btnPrev + @" alt='Предишна страница' title='Предишна страница' class='PaginationButton' onclick=""BtnPagingClick('btnPrev');"" />
                              <span id='lblTablePagination' class='PaginationLabel'>" + pageTablePagination + @"</span>
                              <img id='btnTableNext' " + btnNext + @" alt='Следваща страница' title='Следваща страница' class='PaginationButton' onclick=""BtnPagingClick('btnNext');"" />
                              <img id='btnTableLast' " + btnLast + @" alt='Последна страница' title='Последна страница' class='PaginationButton' onclick=""BtnPagingClick('btnLast');"" />
                              
                              <span style='padding: 0 30px'>&nbsp;</span>
                              <span style='text-align: right;'>Отиди на страница</span>
                              <input id='txtTableGotoPage' UnsavedCheckSkipMe='true' class='InputField' type='text' style='width: 30px;' value='' />
                              <img id='btnTableGoto' src='../Images/ButtonGoto.png' alt='Отиди на страница' title='Отиди на страница' class='PaginationButton' onclick=""BtnPagingClick('btnPageGo');"" />
                           </div>
                        </div>

                        <table id='tblVacantPositions' class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            
                         </thead>";

            //Set Table Results
            string headerStyle = "vertical-align: bottom;";
            int orderCol = (orderBy > 100 ? orderBy - 100 : orderBy);
            string img = orderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
            string[] arrOrderCol = { "", "", "", "" };

            arrOrderCol[orderCol - 1] = @"<div style='Position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

            //Setup the header of the grid
            html += @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                                       <th style='width: 20px;" + headerStyle + @"'>№</th>" +
                (!IsPositionHidden ? @"<th style='width: 130px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Длъжност" + arrOrderCol[0] + @"</th>" : "") +
(!IsMilitaryReportSpecialityHidden ? @"<th style='width: 180px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>ВОС" + arrOrderCol[1] + @"</th>" : "") +
            (!IsMilitaryRankHidden ? @"<th style='width: 80px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>Звание" + arrOrderCol[2] + @"</th>" : "") +
         (!IsReservistsCountHidden ? @"<th style='width: 50px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(4);'>Брой позиции" + arrOrderCol[3] + @"</th>" : "") +
                                     @"<th style='width: 50px;" + headerStyle + @"'>Избрани позиции</th>

 </tr></thead>";

            int counter = 1;

            //Iterate through all items and add them into the grid
            foreach (VacantPosition vacantPosition in availablePositions)
            {

                string cellStyle = "vertical-align: top;";

                html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"' >
                                       <td style='" + cellStyle + @"'>" + ((pageIndex - 1) * pageLength + counter).ToString() + @"</td>" +
                (!IsPositionHidden ? @"<td style='" + cellStyle + @"'>" + vacantPosition.Position + @"</td>" : "") +
(!IsMilitaryReportSpecialityHidden ? @"<td style='" + cellStyle + @"'>" + vacantPosition.MilitaryReportSpecialitiesHTML + @"</td>" : "") +
            (!IsMilitaryRankHidden ? @"<td style='" + cellStyle + @"'>" + vacantPosition.MilitaryRanksHTML + @"</td>" : "") +
         (!IsReservistsCountHidden ? @"<td style='" + cellStyle + @"'><span id='positionCnt" + counter + @"'>" + vacantPosition.PositionsCnt.ToString() + @"</span></td>" : "") +
                                     @"<td style='" + cellStyle + @"'>
                                          <input type='text' name='txtVacantPositions" + counter + @"' id='txtVacantPositions" + counter + @"' UnsavedCheckSkipMe='true' style='width: 50px;' />
                                          <input type='hidden' id='hdnMaxVSST_ID" + counter + @"' UnsavedCheckSkipMe='true' value='" + vacantPosition.MaxVSST_ID.ToString() + @"' />
                                       </td>
                         </tr>";

                counter++;
            }
            html += "</table>";

            html += "<input type='hidden' id='hdnVacantPositionsCounter' value='" + counter + "' UnsavedCheckSkipMe='true' />";

            html += @"<div style='height: 10px;'>
                </div>

                 <div style='text-align: center'>
                   <span id='lblImportPositionsMessage'>" + htmlNoResults + @"</span><br/>
                </div>
";

            html += @"  </div>
                        <div id='btnImportVacantPositions' runat='server' class='Button' onclick=""ImportSelectedVacantPositions();""><i></i><div style='width:70px; padding-left:5px;'>Избери</div><b></b></div>
                        <div id='btnCloseTable' runat='server' class='Button' onclick=""HideImportVacantPositionsLightBox();""><i></i><div style='width:70px; padding-left:5px;'>Затвори</div><b></b></div>
                      </center>";


            return html;
        }
    }
}
