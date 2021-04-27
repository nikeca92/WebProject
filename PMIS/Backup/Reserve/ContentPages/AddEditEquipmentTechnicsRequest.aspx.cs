using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class AddEditEquipmentTechnicsRequest : RESPage
    {
        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "RES_EQUIPTECHREQUESTS";
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
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadRequestCommandPosition")
            {
                JSLoadRequestCommandPosition();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSMoveTechnicsRequestCommandPosition")
            {
                JSMoveTechnicsRequestCommandPosition();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteRequestCommandPosition")
            {
                JSDeleteRequestCommandPosition();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRepopulateNormativeTechnics")
            {
                JSRepopulateNormativeTechnics();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetNormativeTechnicsCode")
            {
                JSGetNormativeTechnicsCode();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetNormativeTechnicsId")
            {
                JSGetNormativeTechnicsId();
                return;
            }


            jsMilitaryUnitSelectorDiv.InnerHtml = @"<script src=""" + Page.ClientScript.GetWebResourceUrl(typeof(MilitaryUnitSelector.MilitaryUnitSelector), "MilitaryUnitSelector.MilitaryUnitSelector.js") + @""" type=""text/javascript""></script>";

            //Highlight the correct item in the menu
            if (EquipmentTechnicsRequestId == 0)
                HighlightMenuItems("Equipment", "AddEditEquipmentTechnicsRequest");
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

                ddTechnicsType.Attributes.Add("onchange", "RepopulateNormativeTechnics(this, null);");

                lblMilitaryUnit.Text = "Заявката е от " + CommonFunctions.GetLabelText("MilitaryUnit") + ":";
            }

            SetupDatePickers();
            SetupPageUI();
        }

        //Set the correct page header (i.e. either New or Edit)
        private void SetPageHeader()
        {
            string header = (EquipmentTechnicsRequestId > 0 ? "Редактиране на заявка за окомплектоване с техника от резерва" : "Въвеждане на заявка за окомплектоване с техника от резерва");

            lblHeaderTitle.InnerHtml = header;
            Page.Title = header;
        }

        //Load any drop-down on the screen
        private void LoadDropDowns()
        {
            PopulateEquipWithTechRequestsStatuses();
            PopulateAdministrations();
            PopulateTechnicsType();
        }

        //Populate the statuses drop-down
        private void PopulateEquipWithTechRequestsStatuses()
        {
            ddEquipWithTechRequestsStatus.Items.Clear();
            ddEquipWithTechRequestsStatus.Items.Add(ListItems.GetOptionChooseOne());

            List<EquipWithTechRequestsStatus> equipWithTechRequestsStatuses = EquipWithTechRequestsStatusUtil.GetAllEquipWithTechRequestsStatuses(CurrentUser);

            foreach (EquipWithTechRequestsStatus equipWithTechRequestsStatus in equipWithTechRequestsStatuses)
            {
                ListItem li = new ListItem();
                li.Text = equipWithTechRequestsStatus.StatusName;
                li.Value = equipWithTechRequestsStatus.EquipWithTechRequestsStatusId.ToString();

                ddEquipWithTechRequestsStatus.Items.Add(li);
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

        //Populate the technics type drop-down
        private void PopulateTechnicsType()
        {
            ddTechnicsType.Items.Clear();
            ddTechnicsType.Items.Add(ListItems.GetOptionChooseOne());

            List<TechnicsType> technicsTypes = TechnicsTypeUtil.GetAllTechnicsTypes(CurrentUser);

            foreach (TechnicsType technicsType in technicsTypes)
            {
                ListItem li = new ListItem();
                li.Text = technicsType.TypeName;
                li.Value = technicsType.TechnicsTypeId.ToString();

                ddTechnicsType.Items.Add(li);
            }
        }

        //Load the existing data in Edit mode
        private void LoadData()
        {
            //If Edit mode
            if (EquipmentTechnicsRequestId > 0)
            {
                EquipmentTechnicsRequest equipmentTechnicsRequest = EquipmentTechnicsRequestUtil.GetEquipmentTechnicsRequest(EquipmentTechnicsRequestId, CurrentUser);

                txtRequestNumber.Text = equipmentTechnicsRequest.RequestNumber;
                txtRequestDate.Text = CommonFunctions.FormatDate(equipmentTechnicsRequest.RequestDate);

                if (equipmentTechnicsRequest.EquipWithTechRequestsStatus != null)
                    ddEquipWithTechRequestsStatus.SelectedValue = equipmentTechnicsRequest.EquipWithTechRequestsStatus.EquipWithTechRequestsStatusId.ToString();

                if (equipmentTechnicsRequest.MilitaryUnit != null)
                {
                    msMilitaryUnit.SelectedValue = equipmentTechnicsRequest.MilitaryUnit.MilitaryUnitId.ToString();
                    msMilitaryUnit.SelectedText = equipmentTechnicsRequest.MilitaryUnit.DisplayTextForSelection;
                }

                if (equipmentTechnicsRequest.Administration != null)
                    ddAdministration.SelectedValue = equipmentTechnicsRequest.Administration.AdministrationId.ToString();

                //Display the Add Request Command button
                btnAddRequestCommandCont.Style.Add("display", "");

                List<string> disabledClientControls = new List<string>();
                List<string> hiddenClientControls = new List<string>();

                //Load the request commands
                StringBuilder requestCommandsHTML = new StringBuilder();

                if (GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS") != UIAccessLevel.Hidden)
                {
                    for (int i = 0; i < equipmentTechnicsRequest.TechnicsRequestCommands.Count; i++)
                    {
                        requestCommandsHTML.Append(RenderTechnicsRequestCommand(equipmentTechnicsRequest.TechnicsRequestCommands[i], i + 1,
                                                                                disabledClientControls, hiddenClientControls));
                    }
                }

                //Setup page UIItems
                if (GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS") != UIAccessLevel.Enabled ||
                    GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT") != UIAccessLevel.Enabled ||
                    GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS") != UIAccessLevel.Enabled ||
                    GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS_ADD") != UIAccessLevel.Enabled)
                {
                    hiddenClientControls.Add("btnAddRequestCommand");
                }

                SetDisabledClientControls(disabledClientControls.ToArray());
                SetHiddenClientControls(hiddenClientControls.ToArray());

                hdnRequestCommandsCount.Value = equipmentTechnicsRequest.TechnicsRequestCommands.Count.ToString();
                hdnVisibleRequestCommandsCount.Value = equipmentTechnicsRequest.TechnicsRequestCommands.Count.ToString();

                divRequestCommands.InnerHtml = requestCommandsHTML.ToString();
            }
            else
            {
                List<string> disabledClientControls = new List<string>();
                List<string> hiddenClientControls = new List<string>();

                //Setup page UIItems
                if (GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS") != UIAccessLevel.Enabled ||
                    GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT") != UIAccessLevel.Enabled ||
                    GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS") != UIAccessLevel.Enabled ||
                    GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS_ADD") != UIAccessLevel.Enabled)
                {
                    hiddenClientControls.Add("btnAddRequestCommand");
                }

                SetDisabledClientControls(disabledClientControls.ToArray());
                SetHiddenClientControls(hiddenClientControls.ToArray());
            }
        }

        //Collect the information from the page form
        //Store the data in a object in the memory and use that object when peforming DB operations
        private EquipmentTechnicsRequest CollectData()
        {
            EquipmentTechnicsRequest equipmentTechnicsRequest = new EquipmentTechnicsRequest(CurrentUser);

            equipmentTechnicsRequest.EquipmentTechnicsRequestId = EquipmentTechnicsRequestId;
            equipmentTechnicsRequest.RequestNumber = txtRequestNumber.Text;
            if (txtRequestDate.Text != "")
                equipmentTechnicsRequest.RequestDate = CommonFunctions.ParseDate(txtRequestDate.Text);

            if (ddEquipWithTechRequestsStatus.SelectedValue != ListItems.GetOptionChooseOne().Value)
            {
                int equipWithTechRequestsStatusId = int.Parse(ddEquipWithTechRequestsStatus.SelectedValue);
                equipmentTechnicsRequest.EquipWithTechRequestsStatus = EquipWithTechRequestsStatusUtil.GetEquipWithTechRequestsStatus(equipWithTechRequestsStatusId, CurrentUser);
            }

            if (msMilitaryUnit.SelectedValue != ListItems.GetOptionChooseOne().Value)
            {
                int militaryUnitId = int.Parse(msMilitaryUnit.SelectedValue);
                equipmentTechnicsRequest.MilitaryUnit = MilitaryUnitUtil.GetMilitaryUnit(militaryUnitId, CurrentUser);
            }

            if (ddAdministration.SelectedValue != ListItems.GetOptionChooseOne().Value)
            {
                int administrationId = int.Parse(ddAdministration.SelectedValue);
                equipmentTechnicsRequest.Administration = AdministrationUtil.GetAdministration(administrationId, CurrentUser);
            }

            return equipmentTechnicsRequest;
        }

        //Save the data
        private void SaveData()
        {
            //First collect the data from the page form
            EquipmentTechnicsRequest equipmentTechnicsRequest = CollectData();

            //Track the changes into the Audit Trail 
            Change change = new Change(CurrentUser, "RES_EquipmentTechnicsRequests");

            //If a successfull save operation then indicate this on the screen; Otherwise alert a warning
            if (EquipmentTechnicsRequestUtil.SaveEquipmentTechnicsRequest(equipmentTechnicsRequest, CurrentUser, change))
            {
                if (EquipmentTechnicsRequestId == 0)
                {
                    SetLocationHash("AddEditEquipmentTechnicsRequest.aspx?EquipmentTechnicsRequestId=" + equipmentTechnicsRequest.EquipmentTechnicsRequestId.ToString());
                }

                EquipmentTechnicsRequestId = equipmentTechnicsRequest.EquipmentTechnicsRequestId;

                this.lblMessage.CssClass = "SuccessText";
                this.lblMessage.Text = "Записът е успешен";

                SetPageHeader();
                hdnSavedChangesContainer.Value = "tblRequestHeaderSection";
                SetupPageUI();

                if (change.HasEvents)
                {
                    List<FillTechnicsRequest> requestTechnicsByEquipment = FillTechnicsRequestUtil.GetFillTechnicsRequestByEquipmentTechnicsRequest(equipmentTechnicsRequest.EquipmentTechnicsRequestId, CurrentUser);
                    TechnicsAppointmentUtil.RefreshTechnicsAppointment(requestTechnicsByEquipment, CurrentUser, change);
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
                Response.Redirect("~/ContentPages/ManageEquipmentTechnicsRequests.aspx");
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

            if (EquipmentTechnicsRequestId == 0) // add mode of page
            {
                screenHidden = GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS") != UIAccessLevel.Enabled ||
                               GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_ADD") != UIAccessLevel.Enabled;

                screenDisabled = GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS") == UIAccessLevel.Disabled ||
                                 GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_ADD") == UIAccessLevel.Disabled;

                if (screenHidden)
                    RedirectAccessDenied();

                if (screenDisabled)
                {
                    pageDisabledControls.Add(btnSave);
                }

                UIAccessLevel l;

                l = GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_ADD_REQUESTNUM");

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

                l = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_ADD_REQUESTDATE");
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

                l = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_ADD_STATUS");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblEquipWithTechRequestsStatus);
                    pageDisabledControls.Add(ddEquipWithTechRequestsStatus);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblEquipWithTechRequestsStatus);
                    this.pageHiddenControls.Add(ddEquipWithTechRequestsStatus);
                }

                l = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_ADD_MILITARYUNIT");
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

                l = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_ADD_ADMINISTRATION");
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
                screenHidden = GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS") == UIAccessLevel.Hidden ||
                               GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT") == UIAccessLevel.Hidden;

                screenDisabled = GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS") == UIAccessLevel.Disabled ||
                                 GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT") == UIAccessLevel.Disabled;

                if (screenHidden)
                    RedirectAccessDenied();

                if (screenDisabled)
                {
                    pageDisabledControls.Add(btnSave);
                }

                UIAccessLevel l;

                l = GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_REQUESTNUM");

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

                l = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_REQUESTDATE");
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

                l = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_STATUS");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblEquipWithTechRequestsStatus);
                    pageDisabledControls.Add(ddEquipWithTechRequestsStatus);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblEquipWithTechRequestsStatus);
                    this.pageHiddenControls.Add(ddEquipWithTechRequestsStatus);
                }

                l = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_MILITARYUNIT");
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

                l = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_ADMINISTRATION");
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

            l_lb = GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS_TECHNICSTYPE");
            if (l_lb == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblTechnicsType");
                disabledClientControls.Add(ddTechnicsType.ClientID);
            }
            else if (l_lb == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblTechnicsType");
                hiddenClientControls.Add(ddTechnicsType.ClientID);
            }

            l_lb = GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS_NORMATIVETECHNICS");
            if (l_lb == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblNormativeTechnics");
                disabledClientControls.Add("txtNormativeCode");
                disabledClientControls.Add("ddNormativeTechnics");
            }
            else if (l_lb == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblNormativeTechnics");
                hiddenClientControls.Add("txtNormativeCode");
                hiddenClientControls.Add("ddNormativeTechnics");
            }

            l_lb = GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS_COMMENT");
            if (l_lb == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblComment");
                disabledClientControls.Add("txtComment");
            }
            else if (l_lb == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblComment");
                hiddenClientControls.Add("txtComment");
            }

            l_lb = GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS_COUNT");
            if (l_lb == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblCount");
                disabledClientControls.Add("txtCount");
            }
            else if (l_lb == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblCount");
                hiddenClientControls.Add("txtCount");
            }

            l_lb = GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS_DRIVERSCOUNT");
            if (l_lb == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblDriversCount");
                disabledClientControls.Add("txtDriversCount");
            }
            else if (l_lb == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblDriversCount");
                hiddenClientControls.Add("txtDriversCount");
            }

            if (GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS_TECHNICSTYPE") != UIAccessLevel.Enabled &&
                GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS_NORMATIVETECHNICS") != UIAccessLevel.Enabled &&
                GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS_COMMENT") != UIAccessLevel.Enabled &&
                GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS_COUNT") != UIAccessLevel.Enabled &&
                GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS_DRIVERSCOUNT") != UIAccessLevel.Enabled)
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
                Change change = new Change(CurrentUser, "RES_EquipmentTechnicsRequests");

                int EquipmentTechnicsRequestId = int.Parse(Request.Form["EquipmentTechnicsRequestId"]);
                int militaryCommandId = int.Parse(Request.Form["MilitaryCommandId"]);
                int commandsCount = int.Parse(Request.Form["RequestCommandsCount"]);

                TechnicsRequestCommand technicsRequestCommand = TechnicsRequestCommandUtil.AddTechnicsRequestCommand(CurrentUser, 
                    EquipmentTechnicsRequestId, militaryCommandId, change);

                change.WriteLog();

                List<string> disabledClientControls = new List<string>();
                List<string> hiddenClientControls = new List<string>();

                string resultHTML = RenderTechnicsRequestCommand(technicsRequestCommand, commandsCount + 1, disabledClientControls, hiddenClientControls);

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
                Change change = new Change(CurrentUser, "RES_EquipmentTechnicsRequests");

                int technicsRequestCommandId = int.Parse(Request.Form["TechnicsRequestCommandId"]);


                List<FillTechnicsRequest> fillTechnicsRequests = FillTechnicsRequestUtil.GetFillTechnicsRequestByTechRequesCommand(technicsRequestCommandId, CurrentUser);
                
                /*
                List<int> deletedTechnics = new List<int>();

                //Remove all Technics from that Military Command
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
                }*/

                string msg = "";
                if (fillTechnicsRequests.Count == 0)
                {
                    TechnicsRequestCommandUtil.DeleteTechnicsRequestCommand(CurrentUser, technicsRequestCommandId, change);
                    change.WriteLog();
                }
                else
                {
                    msg = "Командата не може да бъде изтрита, поради наличието на техника назначена от следните ВО:<br/>";
                    msg += "<div style='max-height: 400px;overflow-y:auto;margin-top:10px;'><ul style='margin-top:-5px; padding-top:3px;'>";

                    foreach (var md in fillTechnicsRequests.GroupBy(x => x.MilitaryDepartment.MilitaryDepartmentName))
                    {
                        msg += "<li>" + md.Key + "</li>";
                    }

                    msg += "</ul></div>";
                }

                stat = AJAXTools.OK;
                response = "<status>OK</status><msg>" + AJAXTools.EncodeForXML(msg) + "</msg>";
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
                    response  += "<m>" + 
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
                Change change = new Change(CurrentUser, "RES_EquipmentTechnicsRequests");

                int technicsRequestCommandId = int.Parse(Request.Form["TechnicsRequestCommandId"]);
                int militaryCommandId = int.Parse(Request.Form["MilitaryCommandId"]);
                string militaryCommandSuffix = Request.Form["MilitaryCommandSuffix"];
                int deliveryCityId = int.Parse(Request.Form["DeliveryCityId"]);
                string deliveryPlace = Request.Form["DeliveryPlace"];
                int EquipmentTechnicsRequestId = int.Parse(Request.Form["EquipmentTechnicsRequestId"]);
                string appointmentTimeStr = Request.Form["AppointmentTime"];
                string milReadinessIdStr = Request.Form["MilReadinessId"];
                bool doAppointmentUpdate = Request.Form["DoAppointmentUpdate"] == "1" ? true : false;

                TechnicsRequestCommand technicsRequestCommand = new TechnicsRequestCommand(CurrentUser);
                technicsRequestCommand.TechnicsRequestCommandId = technicsRequestCommandId;
                technicsRequestCommand.MilitaryCommand = MilitaryCommandUtil.GetMilitaryCommand(militaryCommandId, CurrentUser);
                technicsRequestCommand.EquipmentTechnicsRequestId = EquipmentTechnicsRequestId;
                technicsRequestCommand.MilitaryCommandSuffix = militaryCommandSuffix;

                if (!String.IsNullOrEmpty(appointmentTimeStr))
                {
                    technicsRequestCommand.AppointmentTime = decimal.Parse(appointmentTimeStr);
                }

                if (!String.IsNullOrEmpty(milReadinessIdStr) && milReadinessIdStr != ListItems.GetOptionChooseOne().Value)
                {
                    technicsRequestCommand.MilitaryReadinessId = int.Parse(milReadinessIdStr);
                }

                if (deliveryCityId != int.Parse(ListItems.GetOptionChooseOne().Value))
                {
                    technicsRequestCommand.DeliveryCity = CityUtil.GetCity(deliveryCityId, CurrentUser);
                }

                technicsRequestCommand.DeliveryPlace = deliveryPlace;

                TechnicsRequestCommandUtil.SaveTechnicsRequestCommand(CurrentUser, technicsRequestCommand, change);

                if (doAppointmentUpdate && change.HasEvents)
                {
                    List<FillTechnicsRequest> requestTechnicsByCommand = FillTechnicsRequestUtil.GetFillTechnicsRequestByTechRequesCommand(technicsRequestCommand.TechnicsRequestCommandId, CurrentUser);
                    TechnicsAppointmentUtil.RefreshTechnicsAppointment(requestTechnicsByCommand, CurrentUser, change);
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
                Change change = new Change(CurrentUser, "RES_EquipmentTechnicsRequests");

                int technicsRequestCommandPositionId = int.Parse(Request.Form["TechnicsRequestCommandPositionId"]);
                int technicsRequestCommandId = int.Parse(Request.Form["TechnicsRequestCommandId"]);
                int technicsTypeId = int.Parse(Request.Form["TechnicsTypeId"]);
                int normativeTechnicsId = int.Parse(Request.Form["NormativeTechnicsId"]);
                string comment = Request.Form["Comment"];
                int count = int.Parse(Request.Form["Count"]);
                int driversCount = int.Parse(Request.Form["DriversCount"]);

                TechnicsRequestCommandPosition technicsRequestCommandPosition = new TechnicsRequestCommandPosition(CurrentUser);

                technicsRequestCommandPosition.TechnicsRequestCommandPositionId = technicsRequestCommandPositionId;
                technicsRequestCommandPosition.TechnicsRequestsCommandId = technicsRequestCommandId;
                technicsRequestCommandPosition.TechnicsType = TechnicsTypeUtil.GetTechnicsType(technicsTypeId, CurrentUser);
                technicsRequestCommandPosition.NormativeTechnics = NormativeTechnicsUtil.GetNormativeTechnicsObj(CurrentUser, normativeTechnicsId);
                technicsRequestCommandPosition.Comment = comment;
                technicsRequestCommandPosition.Count = count;
                technicsRequestCommandPosition.DriversCount = driversCount;

                bool add = technicsRequestCommandPosition.TechnicsRequestCommandPositionId == 0;

                TechnicsRequestCommandPositionUtil.SaveTechnicsRequestCommandPosition(technicsRequestCommandPosition, CurrentUser, change);
                
                if(add)
                    TechnicsRequestCommandPositionUtil.RearrangeTechnicsRequestCommandPositions(technicsRequestCommandId, CurrentUser);


                if (change.HasEvents)
                {
                    List<FillTechnicsRequest> requestTechnicsByCommandPosition = FillTechnicsRequestUtil.GetFillTechnicsRequestByTechReqCommandPosition(technicsRequestCommandPosition.TechnicsRequestCommandPositionId, CurrentUser);
                    TechnicsAppointmentUtil.RefreshTechnicsAppointment(requestTechnicsByCommandPosition, CurrentUser, change);
                }

                change.WriteLog();

                int idx = int.Parse(Request.Form["Idx"]);
                string refreshedPositionsTable = RenderTechnicsRequestCommandPositions(technicsRequestCommandPosition.TechnicsRequestsCommand, idx);

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
                int technicsRequestCommandPositionId = int.Parse(Request.Form["TechnicsRequestCommandPositionId"]);

                TechnicsRequestCommandPosition technicsRequestCommandPosition = TechnicsRequestCommandPositionUtil.GetTechnicsRequestCommandPosition(CurrentUser, technicsRequestCommandPositionId);

                stat = AJAXTools.OK;
                response = @"<technicsRequestCommandPosition>
                                <technicsRequestCommandPositionID>" + technicsRequestCommandPosition.TechnicsRequestCommandPositionId.ToString() + @"</technicsRequestCommandPositionID>
                                <technicsRequestsCommandID>" + technicsRequestCommandPosition.TechnicsRequestsCommandId.ToString() + @"</technicsRequestsCommandID>
                                <technicsTypeId>" + technicsRequestCommandPosition.TechnicsType.TechnicsTypeId.ToString() + @"</technicsTypeId>
                                <normativeTechnicsId>" + (technicsRequestCommandPosition.NormativeTechnics != null ? technicsRequestCommandPosition.NormativeTechnics.NormativeTechnicsId.ToString() : "") + @"</normativeTechnicsId>
                                <normativeCode>" + (technicsRequestCommandPosition.NormativeTechnics != null ? AJAXTools.EncodeForXML(technicsRequestCommandPosition.NormativeTechnics.NormativeCode) : "") + @"</normativeCode>
                                <comment>" + AJAXTools.EncodeForXML(technicsRequestCommandPosition.Comment) + @"</comment>
                                <count>" + technicsRequestCommandPosition.Count.ToString() + @"</count>
                                <driversCount>" + technicsRequestCommandPosition.DriversCount.ToString() + @"</driversCount>
                                <fulfilCount>" + technicsRequestCommandPosition.FulfilCount.ToString() + @"</fulfilCount>
                             </technicsRequestCommandPosition>";
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
        private void JSMoveTechnicsRequestCommandPosition()
        {
            string stat = "";
            string response = "";

            try
            {                                           
                Change change = new Change(CurrentUser, "RES_EquipmentTechnicsRequests");

                int technicsRequestCommandID = int.Parse(Request.Form["TechnicsRequestCommandID"]);
                int technicsRequestCommandPositionID_1 = int.Parse(Request.Form["TechnicsRequestCommandPositionID_1"]);
                int technicsRequestCommandPositionID_2 = int.Parse(Request.Form["TechnicsRequestCommandPositionID_2"]);


                TechnicsRequestCommandPosition technicsCommandPos1 = TechnicsRequestCommandPositionUtil.GetTechnicsRequestCommandPosition(CurrentUser, technicsRequestCommandPositionID_1);
                TechnicsRequestCommandPosition technicsCommandPos2 = TechnicsRequestCommandPositionUtil.GetTechnicsRequestCommandPosition(CurrentUser, technicsRequestCommandPositionID_2);
                TechnicsRequestCommandPositionUtil.SwapTechnicsRequestCommandPositionsOrder(CurrentUser, change, technicsCommandPos1, technicsCommandPos2);

                change.WriteLog();

                int idx = int.Parse(Request.Form["Idx"]);
                TechnicsRequestCommand command = TechnicsRequestCommandUtil.GetTechnicsRequestCommand(CurrentUser, technicsRequestCommandID);
                string refreshedPositionsTable = RenderTechnicsRequestCommandPositions(command, idx);

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

        //Delete a particular command position (ajax call)
        private void JSDeleteRequestCommandPosition()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_EquipmentTechnicsRequests");
                int technicsRequestCommandPositionId = int.Parse(Request.Form["TechnicsRequestCommandPositionId"]);
                int technicsRequestsCommandId = int.Parse(Request.Form["TechnicsRequestCommandId"]);


                List<FillTechnicsRequest> fillTechnicsRequests = FillTechnicsRequestUtil.GetFillTechnicsRequestByTechReqCommandPosition(technicsRequestCommandPositionId, CurrentUser);
                /*
                List<int> deletedTechnics = new List<int>();

                //Remove the all Technics from that Military Command Position
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
                    TechnicsRequestCommandPositionUtil.DeleteTechnicsRequestCommandPosition(CurrentUser, technicsRequestCommandPositionId, change);
                    TechnicsRequestCommandPositionUtil.RearrangeTechnicsRequestCommandPositions(technicsRequestsCommandId, CurrentUser);
                    change.WriteLog();
                }
                else
                {
                    msg = "Видът техника не може да бъде изтрит, поради наличието на техника назначена от следните ВО:<br/>";
                    msg += "<div style='max-height: 400px;overflow-y:auto;margin-top:10px;'><ul style='margin-top:-5px; padding-top:3px;'>";

                    foreach (var md in fillTechnicsRequests.GroupBy(x => x.MilitaryDepartment.MilitaryDepartmentName))
                    {
                        msg += "<li>" + md.Key + "</li>";
                    }

                    msg += "</ul></div>";
                }    
                
                int idx = int.Parse(Request.Form["Idx"]);
                TechnicsRequestCommand technicsReqestCommand = TechnicsRequestCommandUtil.GetTechnicsRequestCommand(CurrentUser, technicsRequestsCommandId);
                string refreshedPositionsTable = RenderTechnicsRequestCommandPositions(technicsReqestCommand, idx);

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

        //Populate the normative technics drop-down when changing the technics type (ajax call)
        private void JSRepopulateNormativeTechnics()
        {
            string stat = "";
            string response = "";

            try
            {
                int technicsTypeId = 0;

                if (!String.IsNullOrEmpty(Request.Form["TechnicsTypeId"]))
                    technicsTypeId = int.Parse(Request.Form["TechnicsTypeId"]);

                response = "<normativetechnics>";

                response += "<n>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</n>";

                if (technicsTypeId > 0)
                {
                    TechnicsType technicsType = TechnicsTypeUtil.GetTechnicsType(technicsTypeId, CurrentUser);
                    List<NormativeTechnics> normativeTechnics = NormativeTechnicsUtil.GetNormativeTechnics(CurrentUser, technicsType.TypeKey);

                    foreach (NormativeTechnics normativeTech in normativeTechnics)
                    {
                        response += "<n>" +
                                    "<id>" + normativeTech.NormativeTechnicsId + "</id>" +
                                    "<name>" + AJAXTools.EncodeForXML(normativeTech.NormativeName) + "</name>" +
                                    "</n>";
                    }
                }

                response += "</normativetechnics>";

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

        private void JSGetNormativeTechnicsCode()
        {
            string stat = "";
            string response = "";

            try
            {
                int normativeTechnicsId = 0;

                if (!String.IsNullOrEmpty(Request.Form["NormativeTechnicsId"]))
                    normativeTechnicsId = int.Parse(Request.Form["NormativeTechnicsId"]);

                string normativeCode = "";

                NormativeTechnics normativeTechnics = NormativeTechnicsUtil.GetNormativeTechnicsObj(CurrentUser, normativeTechnicsId);

                if (normativeTechnics != null)
                    normativeCode = normativeTechnics.NormativeCode;

                stat = AJAXTools.OK;
                response = "<normativeCode>" + AJAXTools.EncodeForXML(normativeCode) + "</normativeCode>";
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

        private void JSGetNormativeTechnicsId()
        {
            string stat = "";
            string response = "";

            try
            {
                string normativeCode = Request.Form["NormativeCode"];

                int technicsTypeId = 0;

                if (!String.IsNullOrEmpty(Request.Form["TechnicsTypeId"]))
                    technicsTypeId = int.Parse(Request.Form["TechnicsTypeId"]);

                int normativeTechnicsId = 0;

                if (technicsTypeId > 0)
                {
                    TechnicsType technicsType = TechnicsTypeUtil.GetTechnicsType(technicsTypeId, CurrentUser);
                    NormativeTechnics normativeTechnics = NormativeTechnicsUtil.GetNormativeTechnicsObjByCode(CurrentUser, normativeCode, technicsType.TypeKey);

                    if (normativeTechnics != null)
                        normativeTechnicsId = normativeTechnics.NormativeTechnicsId;
                }

                stat = AJAXTools.OK;
                response = "<normativeTechnicsId>" + AJAXTools.EncodeForXML(normativeTechnicsId.ToString()) + "</normativeTechnicsId>";
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

        private string RenderTechnicsRequestCommand(TechnicsRequestCommand technicsRequestCommand, int idx, 
                                                    List<string> disabledClientControls,
                                                    List<string> hiddenClientControls)
        {
            string idxStr = idx.ToString();

            string htmlDelete = "";

            if (technicsRequestCommand.CanDelete &&
               GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS") == UIAccessLevel.Enabled &&
               GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT") == UIAccessLevel.Enabled &&
               GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS") == UIAccessLevel.Enabled &&
               GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS_DELETE") == UIAccessLevel.Enabled)
                htmlDelete = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на тази команда' class='GridActionIcon' onclick='DeleteRequestCommand(" + technicsRequestCommand.TechnicsRequestCommandId.ToString() + ", " + idxStr + @");'  />";

            string htmlSave = "";

            if (GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS") == UIAccessLevel.Enabled &&
                GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT") == UIAccessLevel.Enabled &&
                GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS") == UIAccessLevel.Enabled &&
                GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS_EDIT") == UIAccessLevel.Enabled)
                htmlSave = "<img id='imgBtnSaveReqCmd" + idx + "' src='../Images/save.png' alt='Запис' title='Запис' class='GridActionIcon' style='Position: relative; top: -1px;' data-technicsRequestCommandId='" + technicsRequestCommand.TechnicsRequestCommandId.ToString() + "' data-idx='" + idxStr + @"' onclick='CallSaveRequestCommand(this, 1);'  />&nbsp;";

            //Load Delivery City information
            City deliveryCity = technicsRequestCommand.DeliveryCity;
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
                    technicsRequestCommand.MilitaryReadinessId.HasValue &&
                    technicsRequestCommand.MilitaryReadinessId.Value == milReadiness.MilReadinessId)
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
                                                <span id='lblMilitaryCommandValue" + idxStr + @"' class='ReadOnlyValue'>" + CommonFunctions.HtmlEncoding(technicsRequestCommand.MilitaryCommand.DisplayTextForSelection) + @"</span>
                                                <input type='text' id='txtMilitaryCommandSuffix" + idxStr + @"' class='InputField' style='width: 30px;' value=""" + CommonFunctions.HtmlEncoding(technicsRequestCommand.MilitaryCommandSuffix) + @""" maxlength=""20"" />

                                                <span style='padding: 30px;'>&nbsp;</span>

                                                <span class='InputLabel' id='lblAppointmentTime" + idxStr + @"' >Време за явяване:</span>
                                                <input type='text' id='txtAppointmentTime" + idxStr + @"' value=""" + CommonFunctions.HtmlEncoding(technicsRequestCommand.AppointmentTime.HasValue ? technicsRequestCommand.AppointmentTime.ToString() : "") + @""" class='InputField' style='width: 40px;' />
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
                                                                    <input type='text' id='txtDeliveryPlace" + idxStr + @"' class='InputField' style='width: 240px;' value=""" + CommonFunctions.HtmlEncoding(technicsRequestCommand.DeliveryPlace) + @""" maxlength=""500"" />
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
                                                         <img id=""btnAddPositionManually" + idxStr + @""" src='../Images/data_new.png' alt='Въвеждане на нова позиция' title='Въвеждане на нова позиция' class='GridActionIcon' style='width: 18px; height: 18px;' onclick='AddPositionManually(" + technicsRequestCommand.TechnicsRequestCommandId.ToString() + ", " + idxStr + @");'  />
                                                      </td>
                                                   </tr>
                                                   <tr>
                                                      <td id='tdRequestCommandPositionsCont" + idxStr + @"'>
                                                         " + RenderTechnicsRequestCommandPositions(technicsRequestCommand, idx) + @"
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

                                <input type='hidden' id='hdnMilitaryCommandName" + idxStr + @"' value=""" + CommonFunctions.HtmlEncoding(technicsRequestCommand.MilitaryCommand.DisplayTextForSelection) + @""" />
                                <input type='hidden' id='hdnMilitaryCommandId" + idxStr + @"' value=""" + technicsRequestCommand.MilitaryCommand.MilitaryCommandId.ToString() + @""" />
                            </div>";

            //Setup UIItems logic
            bool commandsDisabled = GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS") == UIAccessLevel.Disabled ||
                                    GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT") == UIAccessLevel.Disabled ||
                                    GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS") == UIAccessLevel.Disabled ||
                                    GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS_EDIT") == UIAccessLevel.Disabled;

            UIAccessLevel l;

            l = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS_EDIT_SUFFIX");
            if (l == UIAccessLevel.Disabled || commandsDisabled)
            {
                disabledClientControls.Add("txtMilitaryCommandSuffix" + idxStr);
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("txtMilitaryCommandSuffix" + idxStr);
            }

            l = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS_EDIT_APTTIME");
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

            l = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS_EDIT_MILREADINESS");
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

            l = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS_EDIT_DELIVERYCITY");
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

            l = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS_EDIT_DELIVERYPLACE");
            if (l == UIAccessLevel.Disabled || commandsDisabled)
            {
                disabledClientControls.Add("txtDeliveryPlace" + idxStr);
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("txtDeliveryPlace" + idxStr);
            }

            if (this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS_EDIT_DELIVERYCITY") == UIAccessLevel.Hidden &&
                this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS_EDIT_DELIVERYPLACE") == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("tdDeliveryPlace" + idxStr);
            }

            if (commandsDisabled ||
                this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS") != UIAccessLevel.Enabled)
            {
                hiddenClientControls.Add("btnAddPositionManually" + idxStr);
            }

            return html;
        }

        private string RenderTechnicsRequestCommandPositions(TechnicsRequestCommand technicsRequestCommand, int idx)
        {
            bool IsPositionsHidden = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS") == UIAccessLevel.Hidden;

            if (IsPositionsHidden)
                return "";

            bool IsTechnicsTypeHidden = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS_TECHNICSTYPE") == UIAccessLevel.Hidden;
            bool IsNormativeTechnicsHidden = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS_NORMATIVETECHNICS") == UIAccessLevel.Hidden;
            bool IsCommentHidden = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS_COMMENT") == UIAccessLevel.Hidden;            
            bool IsCountHidden = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS_COUNT") == UIAccessLevel.Hidden;
            bool IsDriversCountHidden = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS_DRIVERSCOUNT") == UIAccessLevel.Hidden;

            StringBuilder html = new StringBuilder();

            html.Append(@"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                             <thead>
                                <tr>
                                    <th style='width: 15px;vertical-align: bottom; border-right:0px;'>&nbsp;</th>                                           
                                    <th style='width: 20px; vertical-align: bottom;'>№</th>
     " + (!IsTechnicsTypeHidden ? @"<th style='width: 160px; vertical-align: bottom;'>Вид техника</th>" : "") + @"
" + (!IsNormativeTechnicsHidden ? @"<th style='width: 290px; vertical-align: bottom;'>Нормативна категория</th>" : "") + @"
          " + (!IsCommentHidden ? @"<th style='width: 130px; vertical-align: bottom;'>Коментар</th>" : "") + @"                    
            " + (!IsCountHidden ? @"<th style='width: 60px; vertical-align: bottom;'>Брой</th>" : "") + @"
     " + (!IsDriversCountHidden ? @"<th style='width: 60px; vertical-align: bottom;'>Водачи</th>" : "") + @"
                                    <th style='width: 50px;vertical-align: bottom;'>&nbsp;</th>
                                </tr>
                             </thead>");

            int counter = 0;

            foreach (TechnicsRequestCommandPosition position in technicsRequestCommand.TechnicsRequestCommandPositions)
            {
                counter++;

                string deleteHTML = "";

                if (position.CanDelete)
                {
                    if (GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS") == UIAccessLevel.Enabled &&
                        GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT") == UIAccessLevel.Enabled &&
                        GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS") == UIAccessLevel.Enabled &&
                        GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS") == UIAccessLevel.Enabled
                        )
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на тази позиция' class='GridActionIcon' onclick='DeleteRequestCommandPosition(" + position.TechnicsRequestsCommandId.ToString() + ", " + position.TechnicsRequestCommandPositionId.ToString() + ", " + idx.ToString() + @");' />";
                }

                string editHTML = "";

                if (GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS") == UIAccessLevel.Enabled &&
                    GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT") == UIAccessLevel.Enabled &&
                    GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS") == UIAccessLevel.Enabled &&
                    GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS") == UIAccessLevel.Enabled)
                    editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране на позицията' class='GridActionIcon' onclick='EditRequestCommandPosition(" + position.TechnicsRequestsCommandId.ToString() + ", " + position.TechnicsRequestCommandPositionId.ToString() + ", " + idx.ToString() + @");' />";


                string moveUpHTML = "";
                string moveDownHTML = "";
                if (GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS") == UIAccessLevel.Enabled &&
                   GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT") == UIAccessLevel.Enabled &&
                   GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS") == UIAccessLevel.Enabled &&
                   GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_EDIT_COMMANDS_EDIT_POSITIONS") == UIAccessLevel.Enabled)
                {
                    if (counter != 1)
                    {
                        moveUpHTML += "<img src='../Images/move_up.gif' alt='Преместване нагоре' title='Преместване нагоре' class='GridActionIcon' onclick='MoveTechnicsRequestCommandPosition(" + idx + ", " + position.TechnicsRequestsCommandId.ToString() + ", " + position.TechnicsRequestCommandPositionId.ToString() + ", " + technicsRequestCommand.TechnicsRequestCommandPositions[counter - 2].TechnicsRequestCommandPositionId.ToString() + @");' />";
                    }

                    if (counter != technicsRequestCommand.TechnicsRequestCommandPositions.Count)
                    {
                        moveDownHTML += "<img src='../Images/move_down.gif' alt='Преместване надолу' title='Преместване надолу' class='GridActionIcon' onclick='MoveTechnicsRequestCommandPosition(" + idx + ", " + position.TechnicsRequestsCommandId.ToString() + ", " + position.TechnicsRequestCommandPositionId.ToString() + ", " + technicsRequestCommand.TechnicsRequestCommandPositions[counter].TechnicsRequestCommandPositionId.ToString() + @");' />";
                    }                
                }

                string moveHTML = moveUpHTML;
                moveHTML += (!string.IsNullOrEmpty(moveHTML) ? "<br>" : "");
                moveHTML += moveDownHTML;

                html.Append(@"<tr style='vertical-align: middle; height:20px;' class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                    <td style='text-align: left; vertical-align: top;border-right:0px;'>" + moveHTML + @"</td>
                                    <td style='text-align: center; vertical-align: top;'>" + counter.ToString() + @"</td>
     " + (!IsTechnicsTypeHidden ? @"<td style='text-align: left; vertical-align: top;'>" + position.TechnicsType.TypeName + @"</td>" : "") + @"
" + (!IsNormativeTechnicsHidden ? @"<td style='text-align: left; vertical-align: top;'>" + (position.NormativeTechnics != null ? position.NormativeTechnics.CodeAndText : "") + @"</td>" : "") + @"
          " + (!IsCommentHidden ? @"<td style='text-align: left; vertical-align: top;'>" + CommonFunctions.ReplaceNewLinesInString(position.Comment) + @"</td>" : "") + @"
            " + (!IsCountHidden ? @"<td style='text-align: left; vertical-align: top;'>" + position.Count.ToString() + @"</td>" : "") + @"
     " + (!IsDriversCountHidden ? @"<td style='text-align: left; vertical-align: top;'>" + position.DriversCount.ToString() + @"</td>" : "") + @"
                                    <td style='text-align: left; vertical-align: top;'>" + editHTML + deleteHTML + @"</td>
                              </tr>
                             ");
            }

            html.Append("</table>");

            return html.ToString();
        }
    }
}
