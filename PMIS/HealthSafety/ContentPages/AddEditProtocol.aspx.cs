using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using PMIS.Common;
using PMIS.HealthSafety.Common;

namespace PMIS.HealthSafety.ContentPages
{
    public partial class AddEditProtocol : HSPage
    {
        private bool tableEditPermission = true;
        private bool showTable = true;

        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "HS_PROTOCOLS";
            }
        }

        //Getter/Setter of the ID of the displayed protocol(0 - if new)
        private int ProtocolId
        {
            get
            {
                int protocolId = 0;
                //gets ProtocolID either from the hidden field on the page or from page url
                if (String.IsNullOrEmpty(this.hfProtocolID.Value)
                    || this.hfProtocolID.Value == "0")
                {
                    if (Request.Params["ProtocolId"] != null)
                        int.TryParse(Request.Params["ProtocolId"].ToString(), out protocolId);

                    //sets protocol ID in hidden field on the page in order to be accessible in javascript
                    this.hfProtocolID.Value = protocolId.ToString();
                }
                else
                {
                    Int32.TryParse(this.hfProtocolID.Value, out protocolId);
                }

                return protocolId;
            }
            set { this.hfProtocolID.Value = value.ToString(); }
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

                    this.hfFromHome.Value = fh.ToString();
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

        private string MilitaryUnitLabel = CommonFunctions.GetLabelText("MilitaryUnit");

        protected void Page_Load(object sender, EventArgs e)
        {
            //Set Label Text
            SetLabelValueText();

            //Highlight the current page in the menu bar
            if (ProtocolId == 0)
                HighlightMenuItems("Protocols_Add", "Protocols");
            else
                HighlightMenuItems("Protocols");

            //Hide the navigation buttons
            HideNavigationControls(btnBack);

            // Prevent showing "Save changes" dialog box
            LnkForceNoChangesCheck(btnSave);
            LnkForceNoChangesCheck(btnNewProtocolItem);

            //Set custom max length for text areas
            CommonFunctions.SetTextAreaEvents(txtAddress, 2000);
            CommonFunctions.SetTextAreaEvents(txtUsedEquipments, 2000);
            CommonFunctions.SetTextAreaEvents(txtPeoplePresent, 2000);

            //Process the ajax request for saving protocol items
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveProtocolItem")
            {
                JSSaveProtocolItem();
                return;
            }

            //Process the ajax request for the corresponding threshold value for the light box(on change Measure drop down)
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetThreshold")
            {
                JSGetThreshold();
                return;
            }

            //Process the ajax request for properties of protocol item(for displaying in the light box)
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetProtocolItem")
            {
                JSGetProtocolItem();
                return;
            }

            //Process ajax request for deleting of protocol item
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteProtocolItem")
            {
                JSDeleteProtocolItem();
                return;
            }

            jsMilitaryUnitSelectorDiv.InnerHtml = @"<script src=""" + Page.ClientScript.GetWebResourceUrl(typeof(MilitaryUnitSelector.MilitaryUnitSelector), "MilitaryUnitSelector.MilitaryUnitSelector.js") + @""" type=""text/javascript""></script>";

            SetupPageUI(); //setup user interface elements according to rights of the user's role
            SetBtnNewProtocolItem(); //enable or disable button for adding new protocol items, according to mode of work(add or edir protocol)
            SetupDatePickers(); //Setup any calendar control on the screen

            if (!IsPostBack)
            {
                //Pre-fill the date fields with the today's date
                txtProtocolDate.Text = CommonFunctions.FormatDate(DateTime.Now);
                txtMeasurementDate.Text = CommonFunctions.FormatDate(DateTime.Now);

                SetPageName(); // sets page titles according to mode of work(add or edit protocol)
                LoadDropDowns(); //fills dropdowns on the page with values
                LoadData(); // fills the controls on the page, displaying properties of the protocol
                LoadProtocolItemsTable(); // loads html table in the page with protocol items to this protocol
                this.SetBtnPrintProtocol(); // Set visibility of the print button
            }                        

            lblMessage.Text = ""; // clean message of protocol operations
            SetProtocolItemMessage(); //display message from ajax operations on protocol items, if exist            
        }       

        // Set page titles according to mode of work(add or edit protocol)
        private void SetPageName()
        {
            if (this.ProtocolId > 0)
            {
                lblHeaderTitle.InnerHtml = "Редактиране на протокол от извършвани измервания";                
            }
            else
            {
                lblHeaderTitle.InnerHtml = "Добавяне на протокол от извършвани измервания";               
            }

            Page.Title = lblHeaderTitle.InnerHtml;
        }

        // Set visibility of print button
        private void SetBtnPrintProtocol()
        {
            // if the protocol is new and not saved yet, it is not allowed to print it
            if (this.ProtocolId == 0)
            {
                this.btnPrintProtocol.Visible = false;
            }
            else
            {
                this.btnPrintProtocol.Visible = true;
            }
        }

        // Enable or disable button for adding new protocol items, according to mode of work(add or edir protocol)
        private void SetBtnNewProtocolItem()
        {
            // if the protocol is new and not saved yet, it is not allowed to add new items to protocol
            if (ProtocolId == 0)
            {
                DisableButton(btnNewProtocolItem);                
            }
            else
            {
                EnableButton(btnNewProtocolItem);                
            }
        }

        // Setup any date picker controls on the page by setting the CSS of the target inputs
        // Note that the date picker CSS is common
        // This makes the calendar controls to appear on the page next to each input
        private void SetupDatePickers()
        {
            txtProtocolDate.CssClass = "RequiredInputField " + CommonFunctions.DatePickerCSS();
            txtMeasurementDate.CssClass = CommonFunctions.DatePickerCSS();
        }

        // Setup user interface elements according to rights of the user's role
        private void SetupPageUI()
        {
            if (ProtocolId == 0) // add mode of page
            {
                bool screenHidden = (this.GetUIItemAccessLevel("HS_ADDPROT") != UIAccessLevel.Enabled) ||
                                      (this.GetUIItemAccessLevel("HS_PROTOCOLS") != UIAccessLevel.Enabled);

                bool screenDisabled = (this.GetUIItemAccessLevel("HS_ADDPROT") == UIAccessLevel.Disabled) ||
                                      (this.GetUIItemAccessLevel("HS_PROTOCOLS") == UIAccessLevel.Disabled);

                if (screenHidden)
                    RedirectAccessDenied();

                if (screenDisabled)
                {
                    this.pageDisabledControls.Add(btnSave);
                    this.pageDisabledControls.Add(btnNewProtocolItem);
                    tableEditPermission = false;
                }

                UIAccessLevel l;
                
                l = this.GetUIItemAccessLevel("HS_ADDPROT_PROTNUM");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblProtocolNum);
                    this.pageDisabledControls.Add(txtProtocolNum);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblProtocolNum);
                    this.pageHiddenControls.Add(txtProtocolNum);                    
                }

                l = this.GetUIItemAccessLevel("HS_ADDPROT_PROTDATE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblProtocolDate);
                    this.pageDisabledControls.Add(txtProtocolDate);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblProtocolDate);
                    this.pageHiddenControls.Add(txtProtocolDate);
                }
                
                l = this.GetUIItemAccessLevel("HS_ADDPROT_PROTTYPE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblProtocolType);
                    this.pageDisabledControls.Add(ddProtocolType);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblProtocolType);
                    this.pageHiddenControls.Add(ddProtocolType);
                }

                l = this.GetUIItemAccessLevel("HS_ADDPROT_PROTMILUNIT");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblMilitaryUnit);
                    this.pageDisabledControls.Add(musMilitaryUnit);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblMilitaryUnit);
                    this.pageHiddenControls.Add(musMilitaryUnit);
                }

                l = this.GetUIItemAccessLevel("HS_ADDPROT_PROTADDRESS");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblAddress);
                    this.pageDisabledControls.Add(txtAddress);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblAddress);
                    this.pageHiddenControls.Add(txtAddress);
                }

                l = this.GetUIItemAccessLevel("HS_ADDPROT_PROTOBJECT");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblObject);
                    this.pageDisabledControls.Add(txtObject);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblObject);
                    this.pageHiddenControls.Add(txtObject);
                }

                l = this.GetUIItemAccessLevel("HS_ADDPROT_PROTREQUESTING");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblRequesting);
                    this.pageDisabledControls.Add(txtRequesting);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblRequesting);
                    this.pageHiddenControls.Add(txtRequesting);
                }

                l = this.GetUIItemAccessLevel("HS_ADDPROT_PROTUSEDEQUIP");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblUsedEquipments);
                    this.pageDisabledControls.Add(txtUsedEquipments);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblUsedEquipments);
                    this.pageHiddenControls.Add(txtUsedEquipments);
                }

                l = this.GetUIItemAccessLevel("HS_ADDPROT_PROTMEASURDATE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblMeasurementDate);
                    this.pageDisabledControls.Add(txtMeasurementDate);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblMeasurementDate);
                    this.pageHiddenControls.Add(txtMeasurementDate);
                }

                l = this.GetUIItemAccessLevel("HS_ADDPROT_PROTNORMDOC");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblNormativeDocument);
                    this.pageDisabledControls.Add(txtNormativeDocument);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblNormativeDocument);
                    this.pageHiddenControls.Add(txtNormativeDocument);
                }

                l = this.GetUIItemAccessLevel("HS_ADDPROT_PROTPEOPLEPRESENT");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblPeoplePresent);
                    this.pageDisabledControls.Add(txtPeoplePresent);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblPeoplePresent);
                    this.pageHiddenControls.Add(txtPeoplePresent);
                }

                l = this.GetUIItemAccessLevel("HS_ADDPROT_PROTMEASURMETHOD");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblMeasurementMethod);
                    this.pageDisabledControls.Add(txtMeasurementMethod);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblMeasurementMethod);
                    this.pageHiddenControls.Add(txtMeasurementMethod);
                }

                l = this.GetUIItemAccessLevel("HS_ADDPROT_PROTITEMS");
                if (l == UIAccessLevel.Disabled || screenDisabled
                        || (this.GetUIItemAccessLevel("HS_ADDPROT_PROTITEMS_WORKPLACE") == UIAccessLevel.Disabled
                        && this.GetUIItemAccessLevel("HS_ADDPROT_PROTITEMS_WORKPEOPLE") == UIAccessLevel.Disabled
                        && this.GetUIItemAccessLevel("HS_ADDPROT_PROTITEMS_MEAS") == UIAccessLevel.Disabled
                        && this.GetUIItemAccessLevel("HS_ADDPROT_PROTITEMS_MEASURED") == UIAccessLevel.Disabled
                        && this.GetUIItemAccessLevel("HS_ADDPROT_PROTITEMS_OTHER") == UIAccessLevel.Disabled
                        || this.GetUIItemAccessLevel("HS_ADDPROT_PROTITEMS_WORKPLACE") == UIAccessLevel.Hidden
                        && this.GetUIItemAccessLevel("HS_ADDPROT_PROTITEMS_WORKPEOPLE") == UIAccessLevel.Hidden
                        && this.GetUIItemAccessLevel("HS_ADDPROT_PROTITEMS_MEAS") == UIAccessLevel.Hidden
                        && this.GetUIItemAccessLevel("HS_ADDPROT_PROTITEMS_MEASURED") == UIAccessLevel.Hidden
                        && this.GetUIItemAccessLevel("HS_ADDPROT_PROTITEMS_OTHER") == UIAccessLevel.Hidden))
                {                    
                    this.pageDisabledControls.Add(btnNewProtocolItem);
                    tableEditPermission = false;
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(btnNewProtocolItem);
                    showTable = false;
                }

                List<string> disabledClientControls = new List<string>();
                List<string> hiddenClientControls = new List<string>();

                l = this.GetUIItemAccessLevel("HS_ADDPROT_PROTITEMS_WORKPLACE");
                if (l == UIAccessLevel.Disabled)
                {
                    disabledClientControls.Add("lblWorkingPlace");
                    disabledClientControls.Add("txtWorkingPlace");                   
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWorkingPlace");
                    hiddenClientControls.Add("txtWorkingPlace");
                }

                l = this.GetUIItemAccessLevel("HS_ADDPROT_PROTITEMS_WORKPEOPLE");
                if (l == UIAccessLevel.Disabled)
                {
                    disabledClientControls.Add("lblWorkingPeople");
                    disabledClientControls.Add("txtWorkingPeople");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWorkingPeople");
                    hiddenClientControls.Add("txtWorkingPeople");
                }

                l = this.GetUIItemAccessLevel("HS_ADDPROT_PROTITEMS_MEAS");
                if (l == UIAccessLevel.Disabled)
                {
                    disabledClientControls.Add("lblMeasure");
                    disabledClientControls.Add(ddMeasure.ClientID);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblMeasure");
                    hiddenClientControls.Add(ddMeasure.ClientID);
                }

                l = this.GetUIItemAccessLevel("HS_ADDPROT_PROTITEMS_MEASURED");
                if (l == UIAccessLevel.Disabled)
                {
                    disabledClientControls.Add("lblMeasured");
                    disabledClientControls.Add("txtMeasured");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblMeasured");
                    hiddenClientControls.Add("txtMeasured");
                }

                l = this.GetUIItemAccessLevel("HS_ADDPROT_PROTITEMS_OTHER");
                if (l == UIAccessLevel.Disabled)
                {
                    disabledClientControls.Add("lblOther");
                    disabledClientControls.Add("txtOther");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblOther");
                    hiddenClientControls.Add("txtOther");
                }             

                SetDisabledClientControls(disabledClientControls.ToArray());
                SetHiddenClientControls(hiddenClientControls.ToArray());
            }
            else // edit mode of page
            {
                bool screenHidden = (this.GetUIItemAccessLevel("HS_EDITPROT") == UIAccessLevel.Hidden) ||
                                      (this.GetUIItemAccessLevel("HS_PROTOCOLS") == UIAccessLevel.Hidden);

                bool screenDisabled = (this.GetUIItemAccessLevel("HS_EDITPROT") == UIAccessLevel.Disabled) ||
                                      (this.GetUIItemAccessLevel("HS_PROTOCOLS") == UIAccessLevel.Disabled);

                if (screenHidden)
                    RedirectAccessDenied();

                if (screenDisabled)
                {
                    this.pageDisabledControls.Add(btnSave);
                    this.pageDisabledControls.Add(btnNewProtocolItem);
                    tableEditPermission = false;
                }

                UIAccessLevel l;

                l = this.GetUIItemAccessLevel("HS_EDITPROT_PROTNUM");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblProtocolNum);
                    this.pageDisabledControls.Add(txtProtocolNum);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblProtocolNum);
                    this.pageHiddenControls.Add(txtProtocolNum);
                }               

                l = this.GetUIItemAccessLevel("HS_EDITPROT_PROTDATE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblProtocolDate);
                    this.pageDisabledControls.Add(txtProtocolDate);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblProtocolDate);
                    this.pageHiddenControls.Add(txtProtocolDate);
                }

                l = this.GetUIItemAccessLevel("HS_EDITPROT_PROTTYPE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblProtocolType);
                    this.pageDisabledControls.Add(ddProtocolType);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblProtocolType);
                    this.pageHiddenControls.Add(ddProtocolType);
                }

                l = this.GetUIItemAccessLevel("HS_EDITPROT_PROTMILUNIT");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblMilitaryUnit);
                    this.pageDisabledControls.Add(musMilitaryUnit);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblMilitaryUnit);
                    this.pageHiddenControls.Add(musMilitaryUnit);
                }

                l = this.GetUIItemAccessLevel("HS_EDITPROT_PROTADDRESS");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblAddress);
                    this.pageDisabledControls.Add(txtAddress);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblAddress);
                    this.pageHiddenControls.Add(txtAddress);
                }

                l = this.GetUIItemAccessLevel("HS_EDITPROT_PROTOBJECT");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblObject);
                    this.pageDisabledControls.Add(txtObject);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblObject);
                    this.pageHiddenControls.Add(txtObject);
                }

                l = this.GetUIItemAccessLevel("HS_EDITPROT_PROTREQUESTING");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblRequesting);
                    this.pageDisabledControls.Add(txtRequesting);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblRequesting);
                    this.pageHiddenControls.Add(txtRequesting);
                }

                l = this.GetUIItemAccessLevel("HS_EDITPROT_PROTUSEDEQUIP");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblUsedEquipments);
                    this.pageDisabledControls.Add(txtUsedEquipments);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblUsedEquipments);
                    this.pageHiddenControls.Add(txtUsedEquipments);
                }

                l = this.GetUIItemAccessLevel("HS_EDITPROT_PROTMEASURDATE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblMeasurementDate);
                    this.pageDisabledControls.Add(txtMeasurementDate);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblMeasurementDate);
                    this.pageHiddenControls.Add(txtMeasurementDate);
                }

                l = this.GetUIItemAccessLevel("HS_EDITPROT_PROTNORMDOC");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblNormativeDocument);
                    this.pageDisabledControls.Add(txtNormativeDocument);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblNormativeDocument);
                    this.pageHiddenControls.Add(txtNormativeDocument);
                }

                l = this.GetUIItemAccessLevel("HS_EDITPROT_PROTPEOPLEPRESENT");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblPeoplePresent);
                    this.pageDisabledControls.Add(txtPeoplePresent);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblPeoplePresent);
                    this.pageHiddenControls.Add(txtPeoplePresent);
                }

                l = this.GetUIItemAccessLevel("HS_EDITPROT_PROTMEASURMETHOD");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblMeasurementMethod);
                    this.pageDisabledControls.Add(txtMeasurementMethod);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblMeasurementMethod);
                    this.pageHiddenControls.Add(txtMeasurementMethod);
                }

                l = this.GetUIItemAccessLevel("HS_EDITPROT_PROTITEMS");
                if (l == UIAccessLevel.Disabled || screenDisabled
                        || (this.GetUIItemAccessLevel("HS_EDITPROT_PROTITEMS_WORKPLACE") == UIAccessLevel.Disabled
                        && this.GetUIItemAccessLevel("HS_EDITPROT_PROTITEMS_WORKPEOPLE") == UIAccessLevel.Disabled
                        && this.GetUIItemAccessLevel("HS_EDITPROT_PROTITEMS_MEAS") == UIAccessLevel.Disabled
                        && this.GetUIItemAccessLevel("HS_EDITPROT_PROTITEMS_MEASURED") == UIAccessLevel.Disabled
                        && this.GetUIItemAccessLevel("HS_EDITPROT_PROTITEMS_OTHER") == UIAccessLevel.Disabled
                        || this.GetUIItemAccessLevel("HS_EDITPROT_PROTITEMS_WORKPLACE") == UIAccessLevel.Hidden
                        && this.GetUIItemAccessLevel("HS_EDITPROT_PROTITEMS_WORKPEOPLE") == UIAccessLevel.Hidden
                        && this.GetUIItemAccessLevel("HS_EDITPROT_PROTITEMS_MEAS") == UIAccessLevel.Hidden
                        && this.GetUIItemAccessLevel("HS_EDITPROT_PROTITEMS_MEASURED") == UIAccessLevel.Hidden
                        && this.GetUIItemAccessLevel("HS_EDITPROT_PROTITEMS_OTHER") == UIAccessLevel.Hidden))
                {
                    this.pageDisabledControls.Add(btnNewProtocolItem);
                    tableEditPermission = false;
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(btnNewProtocolItem);
                    showTable = false;
                }

                List<string> disabledClientControls = new List<string>();
                List<string> hiddenClientControls = new List<string>();

                l = this.GetUIItemAccessLevel("HS_EDITPROT_PROTITEMS_WORKPLACE");
                if (l == UIAccessLevel.Disabled)
                {
                    disabledClientControls.Add("lblWorkingPlace");
                    this.pageDisabledControls.Add(isWorkingPlace);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWorkingPlace");
                    this.pageHiddenControls.Add(isWorkingPlace);
                }

                l = this.GetUIItemAccessLevel("HS_EDITPROT_PROTITEMS_WORKPEOPLE");
                if (l == UIAccessLevel.Disabled)
                {
                    disabledClientControls.Add("lblWorkingPeople");
                    disabledClientControls.Add("txtWorkingPeople");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWorkingPeople");
                    hiddenClientControls.Add("txtWorkingPeople");
                }

                l = this.GetUIItemAccessLevel("HS_EDITPROT_PROTITEMS_MEAS");
                if (l == UIAccessLevel.Disabled)
                {
                    disabledClientControls.Add("lblMeasure");
                    disabledClientControls.Add(ddMeasure.ClientID);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblMeasure");
                    hiddenClientControls.Add(ddMeasure.ClientID);
                }

                l = this.GetUIItemAccessLevel("HS_EDITPROT_PROTITEMS_MEASURED");
                if (l == UIAccessLevel.Disabled)
                {
                    disabledClientControls.Add("lblMeasured");
                    disabledClientControls.Add("txtMeasured");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblMeasured");
                    hiddenClientControls.Add("txtMeasured");
                }

                l = this.GetUIItemAccessLevel("HS_EDITPROT_PROTITEMS_OTHER");
                if (l == UIAccessLevel.Disabled)
                {
                    disabledClientControls.Add("lblOther");
                    disabledClientControls.Add("txtOther");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblOther");
                    hiddenClientControls.Add("txtOther");
                }

                SetDisabledClientControls(disabledClientControls.ToArray());
                SetHiddenClientControls(hiddenClientControls.ToArray());
            }
            
        }

        // Display message from ajax operations on protocol items, if exist
        private void SetProtocolItemMessage()
        {
            if (hfMsg.Value == "FailProtocolItemSave")
            {
                lblProtocolItemMessage.Text = "Неуспешен запис на измерване!";
                lblProtocolItemMessage.CssClass = "ErrorText";
            }
            else if (hfMsg.Value == "SuccessProtocolItemSave")
            {
                lblProtocolItemMessage.Text = "Успешен запис на измерване!";
                lblProtocolItemMessage.CssClass = "SuccessText";
            }
            else if (hfMsg.Value == "FailProtocolItemDelete")
            {
                lblProtocolItemMessage.Text = "Неуспешно изтриване на измерване!";
                lblProtocolItemMessage.CssClass = "ErrorText";
            }
            else if (hfMsg.Value == "SuccessProtocolItemDelete")
            {
                lblProtocolItemMessage.Text = "Успешно изтриване на измерване!";
                lblProtocolItemMessage.CssClass = "SuccessText";
            }
            else
            {
                lblProtocolItemMessage.Text = "";
            }
          
            hfMsg.Value = ""; //clean message form ajax operations
        }

        // Populate all dropdowns on the screen
        private void LoadDropDowns()
        {
            this.ddProtocolType.DataSource = ProtocolTypeUtil.GetAllProtocolTypes(CurrentUser);
            this.ddProtocolType.DataTextField = "ProtocolTypeName";
            this.ddProtocolType.DataValueField = "ProtocolTypeId";
            this.ddProtocolType.DataBind();
            this.ddProtocolType.Items.Insert(0, ListItems.GetOptionChooseOne());

            this.ddMeasure.DataSource = MeasureUtil.GetAllMeasures(CurrentUser);
            this.ddMeasure.DataTextField = "MeasureName";
            this.ddMeasure.DataValueField = "MeasureId";
            this.ddMeasure.DataBind();
        }

        // Populate protocol's properites on the page
        private void LoadData()
        {
            if (ProtocolId > 0)
            {
                Protocol protocol = ProtocolUtil.GetProtocol(ProtocolId, CurrentUser);

                if (protocol != null)
                {
                    txtProtocolNum.Text = protocol.ProtocolNumber;
                    if (protocol.ProtocolDate.HasValue)
                        txtProtocolDate.Text = CommonFunctions.FormatDate(protocol.ProtocolDate.ToString());
                    ddProtocolType.SelectedValue = protocol.ProtocolTypeId.ToString();
                    musMilitaryUnit.SelectedValue = protocol.MilitaryUnitId.ToString();
                    hdnMilitaryUnitID.Value = protocol.MilitaryUnitId.ToString();
                    musMilitaryUnit.SelectedText = (protocol.MilitaryUnit == null ? "" : protocol.MilitaryUnit.DisplayTextForSelection);
                    txtObject.Text = protocol.Obekt;
                    txtRequesting.Text = protocol.Requesting;
                    if (protocol.MeasurementDate.HasValue)
                        txtMeasurementDate.Text = CommonFunctions.FormatDate(protocol.MeasurementDate.ToString());
                    txtNormativeDocument.Text = protocol.NormativeDocument;
                    txtMeasurementMethod.Text = protocol.MeasurementMethod;
                    txtAddress.Text = protocol.Address;
                    txtUsedEquipments.Text = protocol.UsedEquipments;
                    txtPeoplePresent.Text = protocol.PeoplePresent;
                }
            }            
        }

        // Gathers protocol's properties from the page controls
        private Protocol CollectData()
        {
            Protocol protocol = new Protocol(CurrentUser);
            protocol.ProtocolId = ProtocolId;
            protocol.ProtocolNumber = txtProtocolNum.Text.Trim();
            if (CommonFunctions.TryParseDate(txtProtocolDate.Text.Trim()))
                protocol.ProtocolDate = CommonFunctions.ParseDate(txtProtocolDate.Text.Trim());
            else
                protocol.ProtocolDate = null;
            protocol.ProtocolTypeId = int.Parse(ddProtocolType.SelectedValue);
            protocol.MilitaryUnitId = int.Parse(musMilitaryUnit.SelectedValue);
            protocol.Obekt = txtObject.Text.Trim();
            protocol.Requesting = txtRequesting.Text.Trim();
            if (CommonFunctions.TryParseDate(txtMeasurementDate.Text.Trim()))
                protocol.MeasurementDate = CommonFunctions.ParseDate(txtMeasurementDate.Text.Trim());
            else
                protocol.MeasurementDate = null;
            protocol.NormativeDocument = txtNormativeDocument.Text.Trim();
            protocol.MeasurementMethod = txtMeasurementMethod.Text.Trim();
            protocol.Address = txtAddress.Text.Trim();
            protocol.UsedEquipments = txtUsedEquipments.Text.Trim();
            protocol.PeoplePresent = txtPeoplePresent.Text.Trim();

            return protocol; // return loaded with values protocol-object
        }

        // Loads html table in the page with protocol items to this protocol
        private void LoadProtocolItemsTable()
        {
            this.divProtocolItems.InnerHtml = "";
            if (showTable)
            {
                List<ProtocolItem> protocolItems = ProtocolItemUtil.GetProtocolItemsByProtocol(ProtocolId, CurrentUser); // gets list of all protocol items related to this protocol
                this.divProtocolItems.InnerHtml = this.GenerateProtocolItemsTable(protocolItems); //generate and display html with protocol items table on the page

                if (protocolItems.Count > 0)
                    musMilitaryUnit.Enabled = false;
                else
                    musMilitaryUnit.Enabled = true;
            }
        }

        // Generates html table from list of protocol items
        private string GenerateProtocolItemsTable(List<ProtocolItem> protocolItems)
        {
            bool IsWorkplaceHidden = this.GetUIItemAccessLevel("HS_EDITPROT_PROTITEMS_WORKPLACE") == UIAccessLevel.Hidden;
            bool IsWorkPeopleHidden = this.GetUIItemAccessLevel("HS_EDITPROT_PROTITEMS_WORKPEOPLE") == UIAccessLevel.Hidden;
            bool IsMeasureHidden = this.GetUIItemAccessLevel("HS_EDITPROT_PROTITEMS_MEAS") == UIAccessLevel.Hidden;
            bool IsMeasuredHidden = this.GetUIItemAccessLevel("HS_EDITPROT_PROTITEMS_MEASURED") == UIAccessLevel.Hidden;
            bool IsOtherHidden = this.GetUIItemAccessLevel("HS_EDITPROT_PROTITEMS_OTHER") == UIAccessLevel.Hidden;

            StringBuilder sb = new StringBuilder();
            sb.Append("<table id='protocolItemsTable' name='protocolItemsTable' class='CommonHeaderTable' style='text-align: left;' border='1px'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style=\"min-width: 30px;\"></th>");
            if (!IsWorkplaceHidden)
                sb.Append("<th style=\"min-width: 150px;\">Място на измерване</th>");
            if (!IsWorkPeopleHidden)
                sb.Append("<th style=\"min-width: 65px;\">Брой хора</th>");
            if (!IsMeasureHidden)
                sb.Append("<th style=\"min-width: 150px;\">Измервана величина</th>");
            if (!IsMeasuredHidden)
                sb.Append("<th style=\"width: 50px;\">Измерена стойност</th>");
            if (!IsMeasureHidden)
                sb.Append("<th style=\"width: 50px;\">Гранична стойност</th>");
            if (!(IsMeasuredHidden || IsMeasureHidden))
                sb.Append("<th style=\"width: 50px;\">Разлика</th>");
            if (!IsOtherHidden)
                sb.Append("<th style=\"min-width: 150px;\">Допълнителна информация</th>");
            if (tableEditPermission)
                sb.Append("<th style=\"min-width: 20px;\"></th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            int counter = 1;

            if (protocolItems.Count > 0)
            {
                sb.Append("<tbody>");
            }

            foreach (ProtocolItem item in protocolItems)
            {
                sb.Append("<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + "'>");
                sb.Append("<td>" + counter + "</td>");
                if (!IsWorkplaceHidden)
                    sb.Append("<td>" + item.WorkingPlace.WorkingPlaceName + "</td>");
                if (!IsWorkPeopleHidden)
                    sb.Append("<td>" + item.WorkingPeople.ToString() + "</td>");
                if (!IsMeasureHidden)
                    sb.Append("<td>" + item.Measure.MeasureName + "</td>");
                if (!IsMeasuredHidden)
                    sb.Append("<td>" + CommonFunctions.FormatDecimal(item.Measured) + "</td>");
                if (!IsMeasureHidden)
                    sb.Append("<td>" + CommonFunctions.FormatDecimal(item.Threshold) + "</td>");
                if (!(IsMeasuredHidden || IsMeasureHidden))
                    sb.Append("<td>" + CommonFunctions.FormatDecimal(Math.Abs(item.Threshold - item.Measured)) + "</td>");
                if (!IsOtherHidden)
                    sb.Append("<td>" + CommonFunctions.ReplaceNewLinesInString(item.Other) + "</td>");

                // add edit and delete icons(buttons), which calls javascript functionality for necessary actions
                if (tableEditPermission)
                    sb.Append(@"<td><img border='0' src='../Images/edit.png' alt='покажи' title='покажи' onclick='javascript:ShowProtocolItemLightBox(" + item.ProtocolItemID + @");' style='cursor: pointer;' />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<img border='0' src='../Images/delete.png' alt='Изтриване' title='Изтриване' onclick='javascript:DeleteProtocolItem(" + item.ProtocolItemID + @");' style='cursor: pointer;' /></td>");

                sb.Append("</tr>");
                counter++;
            }

            if (protocolItems.Count > 0)
            {
                sb.Append("</tbody>");
            }

            sb.Append("</table>");            

            return sb.ToString();
        }

        // Check for protocol properties validation and display appropriate error messages on the page, if need
        private bool ValidateData()
        {
            bool isDataValid = true;
            string errMsg = "";
            List<string> errRightsFields = new List<string>();

            if (txtProtocolNum.Text.Trim() == "")
            {
                isDataValid = false;

                if (pageDisabledControls.Contains(txtProtocolNum) || pageHiddenControls.Contains(txtProtocolNum))
                    errRightsFields.Add("Номер на протокол");
                else
                    errMsg += CommonFunctions.GetErrorMessageMandatory("Номер на протокол") + "<br/>";
            }

            if (txtProtocolDate.Text.Trim() == "")
            {
                isDataValid = false;
                if (pageDisabledControls.Contains(txtProtocolDate) || pageHiddenControls.Contains(txtProtocolDate))
                    errRightsFields.Add("Дата на протокол");
                else
                    errMsg += CommonFunctions.GetErrorMessageMandatory("Дата на протокол") + "<br/>";
            }

            if (txtProtocolDate.Text.Trim() != "" && !CommonFunctions.TryParseDate(txtProtocolDate.Text))
            {
                isDataValid = false;
                errMsg += CommonFunctions.GetErrorMessageDate("Дата на протокол") + "<br/>";
            }

            if (txtMeasurementDate.Text != "" && !CommonFunctions.TryParseDate(txtMeasurementDate.Text))
            {
                isDataValid = false;
                errMsg += CommonFunctions.GetErrorMessageDate("Дата на измерване") + "<br/>";
            }

            if (musMilitaryUnit.SelectedValue == "" || 
                musMilitaryUnit.SelectedValue == ListItems.GetOptionChooseOne().Value)
            {
                isDataValid = false;
                if (pageDisabledControls.Contains(musMilitaryUnit) || pageHiddenControls.Contains(musMilitaryUnit))
                    errRightsFields.Add(MilitaryUnitLabel);
                else
                    errMsg += CommonFunctions.GetErrorMessageMandatory(MilitaryUnitLabel) + "<br/>";
            }

            if (errRightsFields.Count > 0)
            {
                errMsg = "<i>" + CommonFunctions.GetErrorMessageNoRights(errRightsFields.ToArray()) + "</i><br />" + errMsg;
            }

            if (!isDataValid)
            {
                this.lblMessage.CssClass = "ErrorText";
                this.lblMessage.Text = errMsg;
            }
            
            return isDataValid;
        }

        // Saves protocol
        private void SaveData()
        {
            Protocol protocol = CollectData(); // gathers protocol properties from page controls

            //Track the changes into the Audit Trail 
            Change change = new Change(CurrentUser, "HS_Protocols");

            if (ProtocolUtil.SaveProtocol(protocol, CurrentUser, change))
            {
                if (ProtocolId == 0)
                {
                    SetLocationHash("AddEditProtocol.aspx?ProtocolId=" + protocol.ProtocolId.ToString());
                }

                ProtocolId = protocol.ProtocolId;

                this.lblMessage.CssClass = "SuccessText";
                this.lblMessage.Text = "Успешен запис на протокол!";

                hdnSavedChanges.Value = "True";
                SetupPageUI();

                this.SetBtnPrintProtocol();

                hdnMilitaryUnitID.Value = protocol.MilitaryUnitId.ToString();
            }
            else
            {
                this.lblMessage.CssClass = "ErrorText";
                this.lblMessage.Text = "Неуспешен запис на протокол!";
            }

            change.WriteLog();
        }

        // Saves protocol, but only if data valid
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                SaveData();
                SetBtnNewProtocolItem();
                this.SetPageName(); // sets page titles according to mode of work(add or edit protocol)
            }
        }

        // Check for protocol item properties validation and display appropriate error messages on the page, if need
        private bool ValidateProtocolItemFields(string workingPlace, string workingPeople, string measured, out string errorMsg)
        {                                                                                              
            bool isDataValid = true;
            errorMsg = "";

            if (string.IsNullOrEmpty(workingPlace))
            {
                isDataValid = false;
                errorMsg += CommonFunctions.GetErrorMessageMandatory("Място на измерване") + "<br />";
            }

            if (!string.IsNullOrEmpty(workingPeople))
            {
                int fake;
                if (!int.TryParse(workingPeople, out fake))
                {
                    isDataValid = false;
                    errorMsg += CommonFunctions.GetErrorMessageNumber("Брой хора") + "<br />";
                }
            }

            if (!string.IsNullOrEmpty(measured) && !CommonFunctions.IsValidDecimal(measured))
            {
                isDataValid = false;
                errorMsg += CommonFunctions.GetErrorMessageNumber("Измерена стойност") + "<br />";
            }
            
            return isDataValid;
        }

        // Saves protocol item from ajax request
        private void JSSaveProtocolItem()
        {
            string resultMsg = "";

            if (ValidateProtocolItemFields(Request.Form["WorkingPlace"], Request.Form["WorkingPeople"], Request.Form["Measured"], out resultMsg))
            {
                int protocolID = int.Parse(Request.Form["ProtocolID"]);
                int protocolItemID = int.Parse(Request.Form["ProtocolItemID"]);
                int militaryUnitId = int.Parse(Request.Form["MilitaryUnitID"]);
                int workingPlaceId = int.Parse(Request.Form["WorkingPlaceID"]);
                string workingPlaceName = Request.Form["WorkingPlace"];
                int workingPeople = !string.IsNullOrEmpty(Request.Form["WorkingPeople"]) ? int.Parse(Request.Form["WorkingPeople"]) : 0;
                int measureID = int.Parse(Request.Form["MeasureID"]);
                decimal measured = !string.IsNullOrEmpty(Request.Form["Measured"]) ? CommonFunctions.ParseDecimal(Request.Form["Measured"]) : 0;
                decimal threshold = !string.IsNullOrEmpty(Request.Form["Threshold"]) ? CommonFunctions.ParseDecimal(Request.Form["Threshold"]) : 0;
                string other = Request.Form["Other"];

                //Track the changes into the Audit Trail 
                Change change = new Change(CurrentUser, "HS_Protocols");

                WorkingPlace workingPlace = null;

                if (workingPlaceId == int.Parse(ListItems.GetOptionChooseOne().Value))
                {
                    WorkingPlace existingWorkingPlace = WorkingPlaceUtil.GetWorkingPlaceByName(militaryUnitId, workingPlaceName, CurrentUser);

                    //Just in case: Prevent adding an already existing item
                    if (existingWorkingPlace == null)
                    {
                        workingPlace = new WorkingPlace(CurrentUser);
                        workingPlace.WorkingPlaceName = workingPlaceName;
                        workingPlace.MilitaryUnitId = militaryUnitId;

                        WorkingPlaceUtil.AddWorkingPlace(workingPlace, CurrentUser, change);
                    }
                    else
                    {
                        workingPlace = existingWorkingPlace;
                    }
                }
                else
                {
                    workingPlace = WorkingPlaceUtil.GetWorkingPlace(workingPlaceId, CurrentUser);
                }

                ProtocolItem protocolItem = new ProtocolItem(CurrentUser);
                protocolItem.ProtocolItemID = protocolItemID;
                protocolItem.WorkingPlace = workingPlace;
                protocolItem.WorkingPeople = workingPeople;
                protocolItem.MeasureID = measureID;
                protocolItem.Measured = measured;
                protocolItem.Threshold = threshold;
                protocolItem.Other = other;

                resultMsg = ProtocolItemUtil.SaveProtocolItem(protocolID, protocolItem, CurrentUser, change) ? AJAXTools.OK : AJAXTools.ERROR;

                change.WriteLog();
            }           

            string response = @"<response>" + AJAXTools.EncodeForXML(resultMsg) + "</response>";
            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();
        }

        // Get the corresponding threshold value for given measure ID(by ajax request)
        private void JSGetThreshold()
        {
            int measureID = int.Parse(Request.Form["MeasureID"]);

            Measure measure = MeasureUtil.GetMeasure(measureID, CurrentUser);

            string res;
            if (measure != null)
                res = measure.Treshold.ToString();
            else
                res = AJAXTools.ERROR;

            string response = @"<response>" + AJAXTools.EncodeForXML(res) + "</response>";
            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();
        }

        // Get ajax-requested data for protocol item(by ProtocolItemID)
        private void JSGetProtocolItem()
        {
            int protocolItemID = int.Parse(Request.Form["ProtocolItemID"]);

            ProtocolItem p = ProtocolItemUtil.GetProtocolItem(protocolItemID, CurrentUser);

            string response = @"<response>                                   
                                    <WorkingPlaceId>" + p.WorkingPlace.WorkingPlaceId + @"</WorkingPlaceId>
                                    <WorkingPlace>" + AJAXTools.EncodeForXML(p.WorkingPlace.WorkingPlaceName) + @"</WorkingPlace>
                                    <WorkingPeople>" + p.WorkingPeople.ToString() + @"</WorkingPeople>
                                    <MeasureID>" + p.MeasureID.ToString() + @"</MeasureID>
                                    <Measured>" + CommonFunctions.FormatDecimal(p.Measured) + @"</Measured>
                                    <Threshold>" + CommonFunctions.FormatDecimal(p.Threshold) + @"</Threshold>
                                    <Other>" + AJAXTools.EncodeForXML(p.Other) + @"</Other>
                                </response>";

            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();
        }

        // Deletes protocol item by ajax request
        private void JSDeleteProtocolItem()
        {
            if (ProtocolId == 0)
            {
                if (this.GetUIItemAccessLevel("HS_ADDPROT_PROTITEMS") != UIAccessLevel.Enabled || this.GetUIItemAccessLevel("HS_PROTOCOLS") != UIAccessLevel.Enabled)
                    RedirectAjaxAccessDenied();
            }
            else
            {
                if (this.GetUIItemAccessLevel("HS_EDITPROT_PROTITEMS") != UIAccessLevel.Enabled || this.GetUIItemAccessLevel("HS_PROTOCOLS") != UIAccessLevel.Enabled)
                    RedirectAjaxAccessDenied();
            }

            int protocolItemId = int.Parse(Request.Form["ProtocolItemID"]);

            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "HS_Protocols");

                if (!ProtocolItemUtil.DeleteProtocolItem(ProtocolId, protocolItemId, CurrentUser, change))
                    throw new Exception("Database operation failed!");

                change.WriteLog();

                stat = AJAXTools.OK;
                response = "<response>OK</response>";
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

        // hidden button for forced refresh of protocol items table
        protected void btnHdnRefreshProtocolItems_Click(object sender, EventArgs e)
        {
            LoadProtocolItemsTable();
        }

        //Navigate back to the ManageProtocols screen
        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (FromHome != 1)
                Response.Redirect("~/ContentPages/ManageProtocols.aspx");
            else
                Response.Redirect("~/ContentPages/Home.aspx");
        }
        //Set label text
        private void SetLabelValueText()
        {
            this.lblMilitaryUnit.Text = this.MilitaryUnitLabel + ":";
            this.lblMilitaryUnitSelectionLightBox.InnerHtml = "Изберете " + this.MilitaryUnitLabel;
            this.lblMilitaryUnitSelectionLightBox2.InnerHtml = "Изборът на " + this.MilitaryUnitLabel + " трябва да бъде записан";
        }
    }
}
