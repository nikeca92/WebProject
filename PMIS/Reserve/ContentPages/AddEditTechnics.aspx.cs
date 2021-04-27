using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class AddEditTechnics : RESPage
    {
        string redirectBack = "";

        public override string PageUIKey
        {
            get
            {
                return "RES_TECHNICS";
            }
        }

        //Get-Set Id for technics (0 - if new)
        public int TechnicsId
        {
            get
            {
                int technicsId = 0;
                //gets reservistid either from the hidden field on the page or from page url
                if (String.IsNullOrEmpty(this.hdnTechnicsId.Value) || this.hdnTechnicsId.Value == "0")
                {
                    if (Request.Params["TechnicsId"] != null)
                        Int32.TryParse(Request.Params["TechnicsId"].ToString(), out technicsId);

                    //sets technics ID in hidden field on the page in order to be accessible in javascript
                    this.hdnTechnicsId.Value = technicsId.ToString();
                }
                else
                {
                    Int32.TryParse(this.hdnTechnicsId.Value, out technicsId);
                }

                return technicsId;
            }
            set { this.hdnTechnicsId.Value = value.ToString(); }
        }

        //Get-Set Id for technics type kye
        public string TechnicsTypeKey
        {
            get
            {
                string technicsTypeKey = "";
                //gets reservistid either from the hidden field on the page or from page url
                if (String.IsNullOrEmpty(this.hdnTechnicsTypeKey.Value))
                {
                    if (!String.IsNullOrEmpty(Request.Params["TechnicsTypeKey"]))
                        technicsTypeKey = Request.Params["TechnicsTypeKey"];

                    //sets technics type key in hidden field on the page in order to be accessible in javascript
                    this.hdnTechnicsTypeKey.Value = technicsTypeKey;
                }
                else
                {
                    technicsTypeKey = this.hdnTechnicsTypeKey.Value;
                }

                return technicsTypeKey;
            }
            set { this.hdnTechnicsTypeKey.Value = value; }
        }

        public string TabOwnerTitle { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveVesselCrew")
            {
                JSSaveVesselCrew();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteVesselCrew")
            {
                JSDeleteVesselCrew();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadVesselCrewTable")
            {
                JSLoadVesselCrewTable();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadTab")
            {
                if ((Request.Params["SelectedTabId"] != null) && (Request.Params["TechnicsId"] != null))
                {
                    string selectesTabId = Request.Params["SelectedTabId"];
                    JSDisplayTab(selectesTabId);
                }
            }

            jsMilitaryUnitSelectorDiv.InnerHtml = @"<script src=""" + Page.ClientScript.GetWebResourceUrl(typeof(MilitaryUnitSelector.MilitaryUnitSelector), "MilitaryUnitSelector.MilitaryUnitSelector.js") + @""" type=""text/javascript""></script>";
            jsItemSelectorDiv.InnerHtml = @"<script src=""" + Page.ClientScript.GetWebResourceUrl(typeof(ItemSelector.ItemSelector), "ItemSelector.ItemSelector.js") + @""" type=""text/javascript""></script>";

            if (Request.Params["fh"] == "1")
                redirectBack = "~/ContentPages/Home.aspx";
            else
                redirectBack = "~/ContentPages/ManageTechnics_" + TechnicsTypeKey + @".aspx";

            //Hide the navigation buttons
            HideNavigationControls(btnBack);

            TechnicsType technicsType = null;

            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.Params["Preview"]))
                    this.hdnIsPreview.Value = "1";

                if (TechnicsId > 0)
                {
                    Technics technics = TechnicsUtil.GetTechnics(TechnicsId, CurrentUser);
                    TechnicsTypeKey = technics.TechnicsType.TypeKey;

                    if(!technics.CanAccessMilitaryDepartment(CurrentUser))
                        this.hdnIsPreview.Value = "1";
                }

                technicsType = TechnicsTypeUtil.GetTechnicsType(TechnicsTypeKey, CurrentUser);

                hdnTechnicsTypeName.Value = technicsType.TypeName;

                string header = (TechnicsId > 0 ? "Редактиране на запис от " : "Добавяне на нов запис в ") + technicsType.TypeName;
                lblHeaderTitle.InnerHtml = header;
                this.Title = header;

                LoadDropDowns(); //fills dropdowns on the page with values
                LoadGeneralPanelInfo();
            }

            if(technicsType == null)
                technicsType = TechnicsTypeUtil.GetTechnicsType(TechnicsTypeKey, CurrentUser);

            //Redirect if the access is denied
            if (GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey) == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            //Hilight the correct item in the menu
            HighlightMenuItems("Technics", "Technics_" + TechnicsTypeKey);

            if (TechnicsId <= 0)
            {
                HighlightMenuItems("Technics", "Technics_AddEdit_" + TechnicsTypeKey);
            }

            TabOwnerTitle = "Собственик/Водач";

            if (technicsType.TypeKey == "TRAILERS" || technicsType.TypeKey == "FUEL_CONTAINERS")
                TabOwnerTitle = "Собственик";

            if (technicsType.TypeKey == "VESSELS")
                TabOwnerTitle = "Собственик/Екипаж";

            SetupPageUI();
        }

        // Setup user interface elements according to rights of the user's role
        private void SetupPageUI()
        {
            //In this method it is implemented the UIItems logic only for the header part of the screen

            bool screenHidden = false;
            bool screenDisabled = false;

            bool basicInfoDisabled = false;
            bool basicInfoHidden = false;
            bool militaryReportDisabled = false;
            bool militaryReportHidden = false;
            bool ownerDisabled = false;
            bool ownerHidden = false;
            bool otherInfoDisabled = false;
            bool otherInfoHidden = false;

            //Setup the "manually add positions" light-box
            List<string> disabledClientControls = new List<string>();
            List<string> hiddenClientControls = new List<string>();

            if (TechnicsId == 0) // add mode of page
            {
                screenHidden = GetUIItemAccessLevel("RES_TECHNICS") != UIAccessLevel.Enabled ||
                               GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey) != UIAccessLevel.Enabled ||
                               GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey + "_ADD") != UIAccessLevel.Enabled;

                screenDisabled = GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                 GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey) == UIAccessLevel.Disabled ||
                                 GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey + "_ADD") == UIAccessLevel.Disabled ||
                                 this.hdnIsPreview.Value == "1";

                basicInfoDisabled = GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                    GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey) == UIAccessLevel.Disabled ||
                                    GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey + "_ADD") == UIAccessLevel.Disabled ||
                                    GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey + "_ADD_BASICINFO") == UIAccessLevel.Disabled;

                basicInfoHidden =   GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                                    GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey) == UIAccessLevel.Hidden ||
                                    GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey + "_ADD") == UIAccessLevel.Hidden ||
                                    GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey + "_ADD_BASICINFO") == UIAccessLevel.Hidden;
                
                if (basicInfoDisabled || basicInfoHidden)
                {
                    hiddenClientControls.Add("btnSaveAllTabs");
                }

                UIAccessLevel l;
                
                l = this.GetUIItemAccessLevel("RES_TECHNICS_" + this.TechnicsTypeKey + "_ADD_BASICINFO_NORMATIVETECHNICS");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblNormativeCode");
                    disabledClientControls.Add("txtNormativeCode");
                    disabledClientControls.Add("ddNormativeTechnics");
                }
                if (l == UIAccessLevel.Hidden || screenHidden || basicInfoHidden)
                {
                    hiddenClientControls.Add("lblNormativeCode");
                    hiddenClientControls.Add("txtNormativeCode");
                    hiddenClientControls.Add("ddNormativeTechnics");
                    hiddenClientControls.Add("pnlNormativeTechnics");
                }

                l = this.GetUIItemAccessLevel("RES_TECHNICS_" + this.TechnicsTypeKey + "_ADD_BASICINFO_MILREPSTATUS");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblGeneralTabCurrMilitaryReportStatus");
                    disabledClientControls.Add("lblGeneralTabCurrMilitaryReportStatusValue");
                }
                if (l == UIAccessLevel.Hidden || screenHidden || basicInfoHidden)
                {
                    hiddenClientControls.Add("lblGeneralTabCurrMilitaryReportStatus");
                    hiddenClientControls.Add("lblGeneralTabCurrMilitaryReportStatusValue");
                }

                l = this.GetUIItemAccessLevel("RES_TECHNICS_" + this.TechnicsTypeKey + "_ADD_BASICINFO_LASTMODIFIED");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblLastModified");
                    disabledClientControls.Add("lblLastModifiedValue");
                }
                if (l == UIAccessLevel.Hidden || screenHidden || basicInfoHidden)
                {
                    hiddenClientControls.Add("lblLastModified");
                    hiddenClientControls.Add("lblLastModifiedValue");
                }

                switch (this.TechnicsTypeKey)
                {
                    case "VEHICLES":
                        AddEditTechnics_VEHICLES_PageUtil.GetGeneralPanelUIItems(this, true, ref disabledClientControls, ref hiddenClientControls);
                        break;
                    case "TRAILERS":
                        AddEditTechnics_TRAILERS_PageUtil.GetGeneralPanelUIItems(this, true, ref disabledClientControls, ref hiddenClientControls);
                        break;
                    case "TRACTORS":
                        AddEditTechnics_TRACTORS_PageUtil.GetGeneralPanelUIItems(this, true, ref disabledClientControls, ref hiddenClientControls);
                        break;
                    case "ENG_EQUIP":
                        AddEditTechnics_ENG_EQUIP_PageUtil.GetGeneralPanelUIItems(this, true, ref disabledClientControls, ref hiddenClientControls);
                        break;
                    case "MOB_LIFT_EQUIP":
                        AddEditTechnics_MOB_LIFT_EQUIP_PageUtil.GetGeneralPanelUIItems(this, true, ref disabledClientControls, ref hiddenClientControls);
                        break;
                    case "RAILWAY_EQUIP":
                        AddEditTechnics_RAILWAY_EQUIP_PageUtil.GetGeneralPanelUIItems(this, true, ref disabledClientControls, ref hiddenClientControls);
                        break;
                    case "AVIATION_EQUIP":
                        AddEditTechnics_AVIATION_EQUIP_PageUtil.GetGeneralPanelUIItems(this, true, ref disabledClientControls, ref hiddenClientControls);
                        break;
                    case "VESSELS":
                        AddEditTechnics_VESSELS_PageUtil.GetGeneralPanelUIItems(this, true, ref disabledClientControls, ref hiddenClientControls);
                        break;
                    case "FUEL_CONTAINERS":
                        AddEditTechnics_FUEL_CONTAINERS_PageUtil.GetGeneralPanelUIItems(this, true, ref disabledClientControls, ref hiddenClientControls);
                        break;
                }
               
                if (screenHidden || screenDisabled)
                    RedirectAccessDenied();                
            }
            else // edit mode of page
            {
                screenHidden = GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                               GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey) == UIAccessLevel.Hidden ||
                               GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey + "_EDIT") == UIAccessLevel.Hidden;

                screenDisabled = GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                 GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey) == UIAccessLevel.Disabled ||
                                 GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey + "_EDIT") == UIAccessLevel.Disabled ||
                                 this.hdnIsPreview.Value == "1";

                basicInfoDisabled = GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                    GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey) == UIAccessLevel.Disabled ||
                                    GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey + "_EDIT") == UIAccessLevel.Disabled ||
                                    GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey + "_EDIT_BASICINFO") == UIAccessLevel.Disabled;

                basicInfoHidden = GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                                  GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey) == UIAccessLevel.Hidden ||
                                  GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey + "_EDIT") == UIAccessLevel.Hidden ||
                                  GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey + "_EDIT_BASICINFO") == UIAccessLevel.Hidden;

                militaryReportDisabled = GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                         GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey) == UIAccessLevel.Disabled ||
                                         GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey + "_EDIT") == UIAccessLevel.Disabled ||
                                         GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey + "_EDIT_MILREP") == UIAccessLevel.Disabled;

                militaryReportHidden = GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                                         GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey) == UIAccessLevel.Hidden ||
                                         GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey + "_EDIT") == UIAccessLevel.Hidden ||
                                         GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey + "_EDIT_MILREP") == UIAccessLevel.Hidden;

                ownerDisabled = GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey) == UIAccessLevel.Disabled ||
                                GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey + "_EDIT") == UIAccessLevel.Disabled ||
                                GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey + "_EDIT_OWNER") == UIAccessLevel.Disabled;

                ownerHidden = GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                              GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey) == UIAccessLevel.Hidden ||
                              GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey + "_EDIT") == UIAccessLevel.Hidden ||
                              GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey + "_EDIT_OWNER") == UIAccessLevel.Hidden;

                otherInfoDisabled = GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                    GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey) == UIAccessLevel.Disabled ||
                                    GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey + "_EDIT") == UIAccessLevel.Disabled ||
                                    GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey + "_EDIT_OTHERINFO") == UIAccessLevel.Disabled;

                otherInfoHidden = GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                                  GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey) == UIAccessLevel.Hidden ||
                                  GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey + "_EDIT") == UIAccessLevel.Hidden ||
                                  GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey + "_EDIT_OTHERINFO") == UIAccessLevel.Hidden;

                if ((basicInfoDisabled || basicInfoHidden) &&
                    (militaryReportDisabled || militaryReportHidden) &&
                    (ownerDisabled || ownerHidden) &&
                    (otherInfoDisabled || otherInfoHidden))
                {
                    hiddenClientControls.Add("btnSaveAllTabs");
                }
                
                UIAccessLevel l;

                l = this.GetUIItemAccessLevel("RES_TECHNICS_" + this.TechnicsTypeKey + "_EDIT_BASICINFO_NORMATIVETECHNICS");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblNormativeCode");
                    disabledClientControls.Add("txtNormativeCode");
                    disabledClientControls.Add("ddNormativeTechnics");
                }
                if (l == UIAccessLevel.Hidden || screenHidden || basicInfoHidden)
                {
                    hiddenClientControls.Add("lblNormativeCode");
                    hiddenClientControls.Add("txtNormativeCode");
                    hiddenClientControls.Add("ddNormativeTechnics");
                    hiddenClientControls.Add("pnlNormativeTechnics");
                }

                l = this.GetUIItemAccessLevel("RES_TECHNICS_" + this.TechnicsTypeKey + "_EDIT_BASICINFO_MILREPSTATUS");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblGeneralTabCurrMilitaryReportStatus");
                    disabledClientControls.Add("lblGeneralTabCurrMilitaryReportStatusValue");
                }
                if (l == UIAccessLevel.Hidden || screenHidden || basicInfoHidden)
                {
                    hiddenClientControls.Add("lblGeneralTabCurrMilitaryReportStatus");
                    hiddenClientControls.Add("lblGeneralTabCurrMilitaryReportStatusValue");
                }


                l = this.GetUIItemAccessLevel("RES_TECHNICS_" + this.TechnicsTypeKey + "_EDIT_BASICINFO_LASTMODIFIED");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblLastModified");
                    disabledClientControls.Add("lblLastModifiedValue");
                }
                if (l == UIAccessLevel.Hidden || screenHidden || basicInfoHidden)
                {
                    hiddenClientControls.Add("lblLastModified");
                    hiddenClientControls.Add("lblLastModifiedValue");
                }

                switch (this.TechnicsTypeKey)
                {
                    case "VEHICLES":
                        AddEditTechnics_VEHICLES_PageUtil.GetGeneralPanelUIItems(this, false, ref disabledClientControls, ref hiddenClientControls);
                        break;
                    case "TRAILERS":
                        AddEditTechnics_TRAILERS_PageUtil.GetGeneralPanelUIItems(this, false, ref disabledClientControls, ref hiddenClientControls);
                        break;
                    case "TRACTORS":
                        AddEditTechnics_TRACTORS_PageUtil.GetGeneralPanelUIItems(this, false, ref disabledClientControls, ref hiddenClientControls);
                        break;
                    case "ENG_EQUIP":
                        AddEditTechnics_ENG_EQUIP_PageUtil.GetGeneralPanelUIItems(this, false, ref disabledClientControls, ref hiddenClientControls);
                        break;
                    case "MOB_LIFT_EQUIP":
                        AddEditTechnics_MOB_LIFT_EQUIP_PageUtil.GetGeneralPanelUIItems(this, false, ref disabledClientControls, ref hiddenClientControls);
                        break;
                    case "RAILWAY_EQUIP":
                        AddEditTechnics_RAILWAY_EQUIP_PageUtil.GetGeneralPanelUIItems(this, false, ref disabledClientControls, ref hiddenClientControls);
                        break;
                    case "AVIATION_EQUIP":
                        AddEditTechnics_AVIATION_EQUIP_PageUtil.GetGeneralPanelUIItems(this, false, ref disabledClientControls, ref hiddenClientControls);
                        break;
                    case "VESSELS":
                        AddEditTechnics_VESSELS_PageUtil.GetGeneralPanelUIItems(this, false, ref disabledClientControls, ref hiddenClientControls);
                        break;
                    case "FUEL_CONTAINERS":
                        AddEditTechnics_FUEL_CONTAINERS_PageUtil.GetGeneralPanelUIItems(this, false, ref disabledClientControls, ref hiddenClientControls);
                        break;
                }
                
                if (screenHidden)
                    RedirectAccessDenied();   
            }

            SetDisabledClientControls(disabledClientControls.ToArray());
            SetHiddenClientControls(hiddenClientControls.ToArray());
        }

        // Populate all dropdowns on the screen
        private void LoadDropDowns()
        {
            
        }

        //Generate the general fields for all technick types
        private void LoadGeneralPanelInfo()
        {
            string html = "";

            //NormativeTechnics 
            string notmativeTechnicsFilterHTML = "";
            if (this.TechnicsTypeKey == "VEHICLES")
            {
                List<GTableItem> vehicleKinds = GTableItemUtil.GetAllGTableItemsByTableName("VehicleKind", ModuleKey, 1, 0, 0, CurrentUser);
                List<IDropDownItem> ddiVehicleKinds = new List<IDropDownItem>();
                foreach (GTableItem vehicleKind in vehicleKinds)
                {
                    ddiVehicleKinds.Add(vehicleKind);
                }

                string vehicleKindsHTML = ListItems.GetDropDownHtml(ddiVehicleKinds, null, "ddVehicleKind", true, null, "", "style='width: 140px;' onchange='ddVehicleKind_Changed();'", true);
                string editVehicleKindsHTML = @"<img id=""imgMaintVehicleKind"" alt=""Редактиране на списъка"" title=""Редактиране на списъка"" style=""cursor: pointer;"" src=""../Images/list_edit.png"" onclick=""ShowGTable('VehicleKind', 1, 1, RefreshVehicleKindList);"" />";

                notmativeTechnicsFilterHTML = @"
                    <div style=""margin-bottom: 5px;"">
                        <span id=""lblVehicleKind"" class=""InputLabel"">Вид:&nbsp;</span>" + vehicleKindsHTML + editVehicleKindsHTML + @"
                    </div>";
            }

            bool normativeTechnicsMandatory = Config.GetWebSetting("NormativeTechnicsMandatory").ToLower() != "false";

            List<IDropDownItemWithTooltip> ddiNormativeTechnics = new List<IDropDownItemWithTooltip>();
            List<NormativeTechnics> normativeTechnics = NormativeTechnicsUtil.GetNormativeTechnics(this.CurrentUser, this.TechnicsTypeKey);

            foreach (NormativeTechnics normativeTechnicsRec in normativeTechnics)
            {
                ddiNormativeTechnics.Add(normativeTechnicsRec);
            }

            string normativeTechnicsDD = ListItems.GetDropDownHtmlWithTooltip(ddiNormativeTechnics, null, "ddNormativeTechnics", true, null, "ddNormativeTechnics_Changed();", "style='width: 710px;' class='" + (normativeTechnicsMandatory ? "RequiredInputField" : "InputField") + "'");

            string normativeTechnicsHTML = @"
                   <div id=""pnlNormativeTechnics"" style=""padding-top: 3px;"">
                      " + notmativeTechnicsFilterHTML + @"
                      <span id=""lblNormativeCode"" class=""InputLabel"">Код:</span>
                      <input id=""txtNormativeCode"" onfocus=""txtNormativeCode_Focus();"" onblur=""txtNormativeCode_Blur();"" type=""text"" class=""" + (normativeTechnicsMandatory ? "RequiredInputField" : "InputField") + @""" style=""width: 50px;"" />
                      " + normativeTechnicsDD + @"
                   </div>";

            //Technics Key number
            string technicsKeyNumber = "";
            switch (this.TechnicsTypeKey)
            {
                case "VEHICLES":
                    technicsKeyNumber = AddEditTechnics_VEHICLES_PageUtil.GetGeneralPanelRegNumberContent();
                    break;
                case "TRAILERS":
                    technicsKeyNumber = AddEditTechnics_TRAILERS_PageUtil.GetGeneralPanelRegNumberContent();
                    break;
                case "TRACTORS":
                    technicsKeyNumber = AddEditTechnics_TRACTORS_PageUtil.GetGeneralPanelRegNumberContent();
                    break;
                case "ENG_EQUIP":
                    technicsKeyNumber = AddEditTechnics_ENG_EQUIP_PageUtil.GetGeneralPanelRegNumberContent();
                    break;
                case "MOB_LIFT_EQUIP":
                    technicsKeyNumber = AddEditTechnics_MOB_LIFT_EQUIP_PageUtil.GetGeneralPanelRegNumberContent();
                    break;
                case "RAILWAY_EQUIP":
                    technicsKeyNumber = AddEditTechnics_RAILWAY_EQUIP_PageUtil.GetGeneralPanelInventaryNumberContent();
                    break;
                case "AVIATION_EQUIP":
                    technicsKeyNumber = AddEditTechnics_AVIATION_EQUIP_PageUtil.GetGeneralPanelInventaryNumberContent();
                    break;
                case "VESSELS":
                    technicsKeyNumber = AddEditTechnics_VESSELS_PageUtil.GetGeneralPanelInventaryNumberContent();
                    break;
                case "FUEL_CONTAINERS":
                    technicsKeyNumber = AddEditTechnics_FUEL_CONTAINERS_PageUtil.GetGeneralPanelInventaryNumberContent();
                    break;
            }
                  
                        
            html = @"<div style='text-align: center; margin-top:20px;'>
                                <table>
                                    <tr>
                                        <td style='text-align: right;'>
                                            <div>
                                                <span id='lblCurrMilDepartment' class='InputLabel'>Военно окръжие:</span>
                                                <span id='lblCurrMilDepartmentValue' class='ReadOnlyValue'>" + PMIS.Reserve.Common.MilitaryReportStatusUtil.GetLabelWhenLackOfStatus() + @"</span>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                <td>
                                <fieldset style='width: 830px; padding: 0px;'>
                                    <table  id='tblBasicInfoHeader' class='InputRegion' style='width: 830px;  padding-top: 0px; margin-top: 0px; padding-left:3px;'>
                                        <tr>
                                            <td colspan='3' style='text-align: left;' >
                                                <span style=""color: #0B4489; font-weight: bold; font-size: 1.1em; width: 100%; Position: relative; bottom: -5px;"">Нормативна категория</span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan='3' style='text-align: left;'>" + 
                                             normativeTechnicsHTML + @"
                                            </td>
                                        </tr>
                                        <tr>
                                            <td  style='text-align: left; vertical-align: top; width: 340px;'>" + 
                                                technicsKeyNumber + @"
                                            </td>
                                            <td style=""text-align: right; vertical-align: top; width:330px;"">
                                                <span id=""lblGeneralTabCurrMilitaryReportStatus"" class=""InputLabel"" style=""vertical-align: top; position: relative; top: 4px;"">Текущо състояние по отчета:</span>
                                            </td>
                                            <td style=""text-align: left; vertical-align: top;"">
                                                <span id=""lblGeneralTabCurrMilitaryReportStatusValue"" class=""ReadOnlyValue"" style=""vertical-align: top; position: relative; top: 4px;"">" + PMIS.Reserve.Common.MilitaryReportStatusUtil.GetLabelWhenLackOfStatus() + @"</span>
                                            </td>
                                        </tr>
                                        <tr>
                                           <td></td>
                                           <td style=""text-align: right; vertical-align: top;"">
                                             <span id=""lblLastModified"" class=""InputLabel"">Последна сверка:</span>
                                           </td>
                                           <td style=""text-align: left; vertical-align: top; width:160px;"">
                                             <span id=""lblLastModifiedValue"" class=""ReadOnlyValue""></span>
                                           </td>
                                        </tr>
                                    </table>
                                </fieldset>
                                </td>
                                </tr>
                                </table>
                            </div>";

            pnlGeneralTechnicsInfo.InnerHtml = html;
        }
        
        //Navigate back to the home screen
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(redirectBack);
        }

        //Load a particular tab's content
        private void JSDisplayTab(string selectedTabId)
        {
            string html = "";
            string UIItems = "";

            int technicsId = 0;
            try
            {
                technicsId = int.Parse(Request.Params["TechnicsId"]);
            }
            catch
            {
                technicsId = 0;
            }

            //Select case to load data
            switch (selectedTabId)
            {
                case "btnTabBasicInfo":
                    html = AddEditTechnics_BasicInfo_PageUtil.GetBasicInfoTabContent(this);
                    UIItems = AddEditTechnics_BasicInfo_PageUtil.GetBasicInfoTabUIItems(this);
                    break;
                case "btnTabMilitaryReport":
                    html = AddEditTechnics_MilitaryReport_PageUtil.GetTabContent(technicsId, ModuleKey, CurrentUser, this);
                    UIItems = AddEditTechnics_MilitaryReport_PageUtil.GetTabUIItems(this);
                    break;
                case "btnTabOwner":
                    html = AddEditTechnics_Owner_PageUtil.GetTabContent(this, CurrentUser);
                    UIItems = AddEditTechnics_Owner_PageUtil.GetTabUIItems(this);
                    break;
                case "btnTabOtherInfo":
                    html = AddEditTechnics_OtherInfo_PageUtil.GetTabContent(this);
                    UIItems = AddEditTechnics_OtherInfo_PageUtil.GetTabUIItems(this);
                    break;
                default:
                    break;
            }

            string response = "<TabHTML>" + AJAXTools.EncodeForXML(html) + "</TabHTML>";

            if (!String.IsNullOrEmpty(UIItems))
            {
                response += "<UIItems>" + UIItems + "</UIItems>";
            }

            string stat = AJAXTools.OK;

            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        public bool IsBasicInfoVisible()
        {
            bool visible = false;

            if (TechnicsId == 0)
                visible = this.GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey + "_ADD_BASICINFO") != UIAccessLevel.Hidden;
            else
                visible = this.GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey + "_EDIT_BASICINFO") != UIAccessLevel.Hidden;

            return visible;
        }

        public bool IsMilitaryReportVisible()
        {
            return this.GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey + "_EDIT_MILREP") != UIAccessLevel.Hidden;
        }

        public bool IsOwnerVisible()
        {
            return this.GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey + "_EDIT_OWNER") != UIAccessLevel.Hidden;
        }

        public bool IsOtherInfoVisible()
        {
            return this.GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey + "_EDIT_OTHERINFO") != UIAccessLevel.Hidden;
        }

        private void JSSaveVesselCrew()
        {          
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_VESSELS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_OWNER_VESSELCREW") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            int vesselId = 0;
            if (!String.IsNullOrEmpty(Request.Params["VesselId"]))
            {
                vesselId = int.Parse(Request.Params["VesselId"]);
            }

            int vesselCrewId = 0;
            if (!String.IsNullOrEmpty(Request.Params["VesselCrewId"]))
            {
                vesselCrewId = int.Parse(Request.Params["VesselCrewId"]);
            }

            int? vesselCrewCategoryId = null;
            if (!String.IsNullOrEmpty(Request.Params["VesselCrewCategoryID"]) &&
                Request.Params["VesselCrewCategoryID"] != ListItems.GetOptionChooseOne().Value)
            {
                vesselCrewCategoryId = int.Parse(Request.Params["VesselCrewCategoryID"]);
            }

            string identNumber = Request.Params["IdentNumber"];

            string militaryRankId = "";
            if (!String.IsNullOrEmpty(Request.Params["MilitaryRankID"]) &&
                (Request.Params["MilitaryRankID"] != ListItems.GetOptionChooseOne().Value))
            {
                militaryRankId = Request.Params["MilitaryRankID"];
            }

            string fullName = Request.Params["FullName"];
            string address = Request.Params["Address"];

            bool? hasAppointment = null;
            if (!String.IsNullOrEmpty(Request.Params["HasAppointment"]))
            {
                hasAppointment = (bool?)(int.Parse(Request.Params["HasAppointment"]) == 1);
            }

            string stat = "";
            string response = "";

            try
            {
                if (identNumber.Trim() != "" && !PersonUtil.IsValidIdentityNumber(identNumber, CurrentUser))
                {
                    stat = AJAXTools.OK;
                    response = "<status>" + AJAXTools.EncodeForXML("Въведеното ЕГН е невалидно") + "</status>";
                }
                else
                {
                    VesselCrew vesselCrew = new VesselCrew(CurrentUser);
                    vesselCrew.VesselCrewID = vesselCrewId;
                    vesselCrew.VesselCrewCategoryID = vesselCrewCategoryId;
                    vesselCrew.IdentNumber = identNumber;
                    vesselCrew.MilitaryRankID = militaryRankId;
                    vesselCrew.FullName = fullName;
                    vesselCrew.Address = address;
                    vesselCrew.HasAppointment = hasAppointment;

                    //Track the changes into the Audit Trail 
                    Change change = new Change(CurrentUser, "RES_Technics_VESSELS");

                    VesselCrewUtil.SaveVesselCrew(vesselId, vesselCrew, CurrentUser, change);

                    //Write into the Audit Trail
                    change.WriteLog();


                    stat = AJAXTools.OK;
                    response = "<status>OK</status>" +
                               "<refreshedTable>" + AJAXTools.EncodeForXML(AddEditTechnics_VESSELS_PageUtil.GetVesselCrewTable(this, CurrentUser)) + "</refreshedTable>";
                }
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

        private void JSDeleteVesselCrew()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_VESSELS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_OWNER_VESSELCREW") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            int vesselId = 0;
            if (!String.IsNullOrEmpty(Request.Params["VesselId"]))
            {
                vesselId = int.Parse(Request.Params["VesselId"]);
            }

            int vesselCrewId = 0;
            if (!String.IsNullOrEmpty(Request.Params["VesselCrewId"]))
            {
                vesselCrewId = int.Parse(Request.Params["VesselCrewId"]);
            }          

            string stat = "";
            string response = "";

            try
            {
                //Track the changes into the Audit Trail 
                Change change = new Change(CurrentUser, "RES_Technics_VESSELS");

                VesselCrewUtil.DeleteVesselCrew(vesselId, vesselCrewId, CurrentUser, change);

                //Write into the Audit Trail
                change.WriteLog();


                stat = AJAXTools.OK;
                response = "<status>OK</status>" +
                           "<refreshedTable>" + AJAXTools.EncodeForXML(AddEditTechnics_VESSELS_PageUtil.GetVesselCrewTable(this, CurrentUser)) + "</refreshedTable>";

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

        private void JSLoadVesselCrewTable()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_VESSELS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_OWNER_VESSELCREW") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = AJAXTools.OK;
            string response = "<refreshedTable>" + AJAXTools.EncodeForXML(AddEditTechnics_VESSELS_PageUtil.GetVesselCrewTable(this, CurrentUser)) + "</refreshedTable>";

            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }
    }
}
