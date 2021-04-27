using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using PMIS.Common;
using PMIS.Applicants.Common;
using System.Text;
using System.IO;
using System.Web.UI;
using System.Linq;

namespace PMIS.Applicants.ContentPages
{
    public partial class EditCadet : APPLPage
    {
        string redirectBack = "";

        public override string PageUIKey
        {
            get
            {
                return "APPL_CADETS_EDITCADET";
            }
        }

        //Get-Set Id for person (0 - if new)
        private int PersonId
        {
            get
            {
                int personId = 0;
                //gets personId either from the hidden field on the page or from page url
                if (String.IsNullOrEmpty(this.hdnPersonId.Value))
                {
                    if (Request.Params["PersonId"] != null)
                        Int32.TryParse(Request.Params["PersonId"].ToString(), out personId);

                    //sets person ID in hidden field on the page in order to be accessible in javascript
                    this.hdnPersonId.Value = personId.ToString();
                }
                else
                {
                    Int32.TryParse(this.hdnPersonId.Value, out personId);
                }

                return personId;
            }
            set { this.hdnPersonId.Value = value.ToString(); }
        }

        //Gets Id for loaded person (0 - if new)
        private int HdnPersonId
        {
            get
            {
                int hdnPersonId = 0;
                //gets personId from page url
                if (Request.Params["HdnPersonId"] != null)
                    Int32.TryParse(Request.Params["HdnPersonId"].ToString(), out hdnPersonId);

                return hdnPersonId;
            }
        }

        private bool canCurrentUserAccessThisMilDepartment;

        private int HdnCadetId
        {
            get
            {
                int hdnCadetId = 0;
                //gets applicantId from page url
                if (Request.Params["HdnCadetId"] != null)
                    Int32.TryParse(Request.Params["HdnCadetId"].ToString(), out hdnCadetId);

                return hdnCadetId;
            }
        }

        private bool CanCurrentUserAccessThisMilDepartment
        {
            get
            {
                string milDepID = "";

                if (HdnCadetId > 0 || hdnCadetId.Value != "")
                {
                    int cadetID = HdnCadetId;

                    if (cadetID == 0)
                    {
                        cadetID = int.Parse(hdnCadetId.Value);
                    }

                    Cadet cadet = CadetUtil.GetCadet(cadetID, CurrentUser);
                    milDepID = cadet.MilitaryDepartmentId.ToString();
                }
                else
                {
                    milDepID = Request.Params["MilitaryDepartmentId"];
                }

                string[] currentUserMilDepartmentIDs = CurrentUser.MilitaryDepartmentIDs_ListOfValues.Split(',');

                canCurrentUserAccessThisMilDepartment = currentUserMilDepartmentIDs.Any(c => c == milDepID);

                return canCurrentUserAccessThisMilDepartment;
            }
        }

        private bool pageDisabled;
        private bool PageDisabled
        {
            get
            {
                return pageDisabled;
            }

            set
            {
                pageDisabled = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if (GetUIItemAccessLevel("APPL_CADETS_EDITCADET") == UIAccessLevel.Hidden
                || GetUIItemAccessLevel("APPL_CADETS") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadPersonDetails")
            {
                JSLoadPersonDetails();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadTab")
            {
                if ((Request.Params["SelectedTabId"] != null) && (Request.Params["HdnPersonId"] != null))
                {
                    string selectesTabId = Request.Params["SelectedTabId"];
                    JSDisplayTab(selectesTabId);
                }
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetCadetEducationLightBoxContent")
            {
                this.GenerateCadetEducationLightBoxContent();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveCadetEducation")
            {
                this.JSSaveCadetEducation();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteCadetEducation")
            {
                if ((Request.Params["SelectedTabId"] != null) && (Request.Params["HdnPersonId"] != null)
                    && (Request.Params["CivilEducationId"] != null))
                {
                    this.JSDeleteCadetEducation();
                    return;
                }
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetCadetLanguageLightBoxContent")
            {
                this.GenerateCadetLanguageLightBoxContent();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveCadetLanguage")
            {
                this.JSSaveCadetLanguage();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteCadetLanguage")
            {
                if ((Request.Params["SelectedTabId"] != null) && (Request.Params["HdnPersonId"] != null)
                    && (Request.Params["ForeignLanguageId"] != null))
                {
                    this.JSDeleteCadetLanguage();
                    return;
                }
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetMilitarySchoolSpecializationsLightBoxContent")
            {
                this.GenerateMilitarySchoolSpecializationsLightBoxContent();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSAddMilitarySchoolSpecializationToPerson")
            {
                this.JSAddMilitarySchoolSpecializationToPerson();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteMilitarySchoolSpecialization")
            {
                this.JSDeleteMilitarySchoolSpecialization();
                return;
            }

            jsItemSelectorDiv.InnerHtml = @"<script src=""" + Page.ClientScript.GetWebResourceUrl(typeof(ItemSelector.ItemSelector), "ItemSelector.ItemSelector.js") + @""" type=""text/javascript""></script>";

            //Hilight the correct item in the menu
            HighlightMenuItems("Cadets", "Cadets_Add");

            //Hide the navigation buttons
            HideNavigationControls(btnBack);

            int personId = 0;
            int militaryDepartmentId = 0;

            if (!String.IsNullOrEmpty(Request.Params["PersonId"])
                && int.TryParse(Request.Params["PersonId"], out personId))
            {
                this.hdnPersonId.Value = personId.ToString();
            }

            string militaryDepartmentName = "";

            if (!String.IsNullOrEmpty(Request.Params["MilitaryDepartmentId"]))
            {
                hdnMilitaryDepartmentId.Value = Request.Params["MilitaryDepartmentId"];
                militaryDepartmentId = int.Parse(Request.Params["MilitaryDepartmentId"]);

                militaryDepartmentName = MilitaryDepartmentUtil.GetMilitaryDepartment(militaryDepartmentId, CurrentUser).MilitaryDepartmentName;
            }

            if (!String.IsNullOrEmpty(Request.Params["CadetId"]))
            {
                int cadetId = 0;
                Int32.TryParse(Request.Params["CadetId"].ToString(), out cadetId);

                Cadet cadet = CadetUtil.GetCadet(cadetId, CurrentUser);
                if (cadet != null)
                {
                    this.hdnCadetId.Value = cadet.CadetId.ToString();
                    this.hdnPersonId.Value = cadet.PersonId.ToString();
                    personId = cadet.PersonId;
                    this.hdnMilitaryDepartmentId.Value = cadet.MilitaryDepartmentId.ToString();

                    militaryDepartmentName = cadet.MilitaryDepartment.MilitaryDepartmentName;
                }
            }
            else
            {
                if (CadetUtil.IsAlreadyRegistered(personId, militaryDepartmentId, CurrentUser))
                {
                    Cadet cadet = CadetUtil.GetCadet(personId, militaryDepartmentId, CurrentUser);
                    if (cadet != null)
                    {
                        this.hdnCadetId.Value = cadet.CadetId.ToString();

                        militaryDepartmentName = cadet.MilitaryDepartment.MilitaryDepartmentName;
                    }
                }
            }

            spanMilitaryDepartmentName.InnerText = militaryDepartmentName;

            if (Request.Params["PageFrom"] != null && Request.Params["PageFrom"] == "1")
            {
                //Request is come from ManageCadets.aspx
                redirectBack = "~/ContentPages/ManageCadets.aspx";
            }
            else
            {
                if (Request.Params["PageFrom"] != null && Request.Params["PageFrom"] == "2")
                {
                    //Request is come from AddCadet_SelectPerson.aspx
                    redirectBack = "~/ContentPages/AddCadet_SelectPerson.aspx";
                }
                else
                {
                    //Set visibilty of editButton False
                    divEdit.Style["display"] = "none";
                    //Request is come from AddCadet_PersonDetails.aspx
                    redirectBack = "~/ContentPages/AddCadet_PersonDetails.aspx?IdentNumber=" + hdnIdentNumber.Value + "&MilitaryDepartmentId=" + hdnMilitaryDepartmentId.Value + "&PageFrom=2";
                }
            }

            // if important parameters are omitted in page url (manually, for example), then redirect instead of crash
            Person person = PersonUtil.GetPerson(personId, CurrentUser);
            if (person == null || person.PresCityId == null)
            {
                Response.Redirect("~/ContentPages/AddCadet_SelectPerson.aspx");
            }

            if (Config.GetWebSetting("KOD_KZV_Check_Cadet").ToLower() == "true" &&
                CommonFunctions.IsKeyInList(person.PersonTypeCode, Config.GetWebSetting("KOD_KZV_Restricted_Cadet")) || !CanCurrentUserAccessThisMilDepartment)
            {
                PageDisabled = true;
            }

            SetupPageUI();
        }

        // Setup user interface elements according to rights of the user's role
        private void SetupPageUI()
        {
            if (PageDisabled)
                pageHiddenControls.Add(btnEdit);

            UIAccessLevel l;

            List<string> disabledClientControls = new List<string>();
            List<string> hiddenClientControls = new List<string>();

            if (!this.IsRegistred()) //Mode Add
            {
                bool screenHidden = (this.GetUIItemAccessLevel("APPL_CADETS_ADDCADET") == UIAccessLevel.Hidden) ||
                                                                  (this.GetUIItemAccessLevel("APPL_CADETS") == UIAccessLevel.Hidden);

                bool screenDisabled = (this.GetUIItemAccessLevel("APPL_CADETS_ADDCADET") == UIAccessLevel.Disabled) ||
                                      (this.GetUIItemAccessLevel("APPL_CADETS") == UIAccessLevel.Disabled);

                if (screenHidden)
                    RedirectAccessDenied();

                //Hide Client Controls

                l = this.GetUIItemAccessLevel("APPL_CADETS_ADD_PERSONDETAILS");

                if (l != UIAccessLevel.Enabled || screenHidden || screenDisabled)
                {
                    hiddenClientControls.Add(divEdit.ClientID);
                }

                if (l == UIAccessLevel.Hidden || screenHidden)
                {
                    hiddenClientControls.Add("lblFirstName");
                    hiddenClientControls.Add("lblFirstNameValue");
                    hiddenClientControls.Add("lblLastNameValue");

                    hiddenClientControls.Add("lblIdentNumber");
                    hiddenClientControls.Add("lblIdentNumberValue");

                    hiddenClientControls.Add("lblGender");
                    hiddenClientControls.Add("lblGenderValue");

                    hiddenClientControls.Add("lblLastModified");
                    hiddenClientControls.Add("lblLastModifiedValue");

                    hiddenClientControls.Add("lblPermAddresTitle");

                    hiddenClientControls.Add("lblPermPostCode");
                    hiddenClientControls.Add("txtPermPostCode");

                    hiddenClientControls.Add("lblPermCity");
                    hiddenClientControls.Add("txtPermCity");

                    hiddenClientControls.Add("lblPermDistrict");
                    hiddenClientControls.Add("txtPermDistrict");

                    hiddenClientControls.Add("lblPermMunicipality");
                    hiddenClientControls.Add("txtPermMunicipality");

                    hiddenClientControls.Add("lblPermRegion");
                    hiddenClientControls.Add("txtPermRegion");

                    hiddenClientControls.Add("lblPermAddress");
                    hiddenClientControls.Add("txtPermAddress");

                    hiddenClientControls.Add("lblPresAddressTitle");

                    hiddenClientControls.Add("lblPresPostCode");
                    hiddenClientControls.Add("txtPresPostCode");

                    hiddenClientControls.Add("lblPresCity");
                    hiddenClientControls.Add("txtPresCity");

                    hiddenClientControls.Add("lblPresDistrict");
                    hiddenClientControls.Add("txtPresDistrict");

                    hiddenClientControls.Add("lblPresMunicipality");
                    hiddenClientControls.Add("txtPresMunicipility");

                    hiddenClientControls.Add("lblPresRegion");
                    hiddenClientControls.Add("txtPresRegion");

                    hiddenClientControls.Add("lblPresAddress");
                    hiddenClientControls.Add("txtPresAddress");

                    hiddenClientControls.Add("lblContactAddressTitle");

                    hiddenClientControls.Add("lblContactPostCode");
                    hiddenClientControls.Add("txtContactPostCode");

                    hiddenClientControls.Add("lblContactCity");
                    hiddenClientControls.Add("txtContactCity");

                    hiddenClientControls.Add("lblContactDistrict");
                    hiddenClientControls.Add("txtContactDistrict");

                    hiddenClientControls.Add("lblContactMunicipality");
                    hiddenClientControls.Add("txtContactMunicipality");

                    hiddenClientControls.Add("lblContactRegion");
                    hiddenClientControls.Add("txtContactRegion");

                    hiddenClientControls.Add("lblContactAddress");
                    hiddenClientControls.Add("txtContactAddress");

                    hiddenClientControls.Add("lblIDCardNumber");
                    hiddenClientControls.Add("txtIDCardNumber");

                    hiddenClientControls.Add("lblIDCardIssuedBy");
                    hiddenClientControls.Add("txtIDCardIssuedBy");

                    hiddenClientControls.Add("lblIDCardIssueDate");
                    hiddenClientControls.Add("txtIDCardIssueDate");

                    hiddenClientControls.Add("lblHomePhone");
                    hiddenClientControls.Add("txtHomePhone");

                    hiddenClientControls.Add("lblMobilePhone");
                    hiddenClientControls.Add("txtMobilePhone");

                    hiddenClientControls.Add("lblEmail");
                    hiddenClientControls.Add("txtEmail");

                    hiddenClientControls.Add("lblDrvLicCategories");
                    hiddenClientControls.Add("txtDrvLicCategories");

                    hiddenClientControls.Add("lblServeInMilitary");
                    hiddenClientControls.Add("lblServeInMilitaryValue");
                }
                else
                {
                    if (this.GetUIItemAccessLevel("APPL_CADETS_ADDCADET_FIRSTNAME") == UIAccessLevel.Hidden
                        && this.GetUIItemAccessLevel("APPL_CADETS_ADDCADET_LASTNAME") == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblFirstName");
                    }

                    l = this.GetUIItemAccessLevel("APPL_CADETS_ADDCADET_FIRSTNAME");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblFirstNameValue");
                    }

                    l = this.GetUIItemAccessLevel("APPL_CADETS_ADDCADET_LASTNAME");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblLastNameValue");
                    }

                    l = this.GetUIItemAccessLevel("APPL_CADETS_ADDCADET_EGN");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblIdentNumber");
                        hiddenClientControls.Add("lblIdentNumberValue");
                    }

                    l = this.GetUIItemAccessLevel("APPL_CADETS_ADDCADET_GENDER");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblGender");
                        hiddenClientControls.Add("lblGenderValue");
                    }


                    l = this.GetUIItemAccessLevel("APPL_CADETS_ADDCADET_PERMADDRESS_CITY");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblPermAddresTitle");

                        hiddenClientControls.Add("lblPermPostCode");
                        hiddenClientControls.Add("txtPermPostCode");

                        hiddenClientControls.Add("lblPermDistrict");
                        hiddenClientControls.Add("txtPermDistrict");

                        hiddenClientControls.Add("lblPermCity");
                        hiddenClientControls.Add("txtPermCity");

                        hiddenClientControls.Add("lblPermMunicipality");
                        hiddenClientControls.Add("txtPermMunicipality");

                        hiddenClientControls.Add("lblPermRegion");
                        hiddenClientControls.Add("txtPermRegion");
                    }

                    l = this.GetUIItemAccessLevel("APPL_CADETS_ADDCADET_PERMADDRESS_ADDRESS");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblPermAddress");
                        hiddenClientControls.Add("txtPermAddress");
                    }

                    l = this.GetUIItemAccessLevel("APPL_CADETS_ADDCADET_PRESADDRESS_CITY");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblPresAddressTitle");

                        hiddenClientControls.Add("lblPresPostCode");
                        hiddenClientControls.Add("txtPresPostCode");

                        hiddenClientControls.Add("lblPresDistrict");
                        hiddenClientControls.Add("txtPresDistrict");

                        hiddenClientControls.Add("lblPresCity");
                        hiddenClientControls.Add("txtPresCity");

                        hiddenClientControls.Add("lblPresMunicipality");
                        hiddenClientControls.Add("txtPresMunicipility");

                        hiddenClientControls.Add("lblPresRegion");
                        hiddenClientControls.Add("txtPresRegion");
                    }

                    l = this.GetUIItemAccessLevel("APPL_CADETS_ADDCADET_PRESADDRESS_ADDRESS");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblPresAddress");
                        hiddenClientControls.Add("txtPresAddress");
                    }

                    l = this.GetUIItemAccessLevel("APPL_CADETS_ADDCADET_CONTACTADDRESS_CITY");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblContactAddressTitle");

                        hiddenClientControls.Add("lblContactPostCode");
                        hiddenClientControls.Add("txtContactPostCode");

                        hiddenClientControls.Add("lblContactDistrict");
                        hiddenClientControls.Add("txtContactDistrict");

                        hiddenClientControls.Add("lblContactCity");
                        hiddenClientControls.Add("txtContactCity");

                        hiddenClientControls.Add("lblContactMunicipality");
                        hiddenClientControls.Add("txtContactMunicipality");

                        hiddenClientControls.Add("lblContactRegion");
                        hiddenClientControls.Add("txtContactRegion");
                    }

                    l = this.GetUIItemAccessLevel("APPL_CADETS_ADDCADET_CONTACTADDRESS_ADDRESS");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblContactAddress");
                        hiddenClientControls.Add("txtContactAddress");
                    }

                    l = this.GetUIItemAccessLevel("APPL_CADETS_ADDCADET_IDCARDNUMBER");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblIDCardNumber");
                        hiddenClientControls.Add("txtIDCardNumber");
                    }

                    l = this.GetUIItemAccessLevel("APPL_CADETS_ADDCADET_IDCARDISSUEDBY");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblIDCardIssuedBy");
                        hiddenClientControls.Add("txtIDCardIssuedBy");
                    }

                    l = this.GetUIItemAccessLevel("APPL_CADETS_ADDCADET_IDCARDISSUEDATE");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblIDCardIssueDate");
                        hiddenClientControls.Add("txtIDCardIssueDate");
                    }

                    l = this.GetUIItemAccessLevel("APPL_CADETS_ADDCADET_HOMEPHONE");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblHomePhone");
                        hiddenClientControls.Add("txtHomePhone");
                    }

                    l = this.GetUIItemAccessLevel("APPL_CADETS_ADDCADET_MOBILEPHONE");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblMobilePhone");
                        hiddenClientControls.Add("txtMobilePhone");
                    }

                    l = this.GetUIItemAccessLevel("APPL_CADETS_ADDCADET_EMAIL");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblEmail");
                        hiddenClientControls.Add("txtEmail");
                    }

                    l = this.GetUIItemAccessLevel("APPL_CADETS_ADDCADET_DRIVINGLICENCE");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblDrvLicCategories");
                        hiddenClientControls.Add("txtDrvLicCategories");
                    }

                    l = this.GetUIItemAccessLevel("APPL_CADETS_ADDCADET_SERVEINMILITARY");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblServeInMilitary");
                        hiddenClientControls.Add("lblServeInMilitaryValue");
                    }
                }
            }
            else // edit mode of page
            {
                bool screenHidden = (this.GetUIItemAccessLevel("APPL_CADETS_EDITCADET") == UIAccessLevel.Hidden) ||
                                                                  (this.GetUIItemAccessLevel("APPL_CADETS") == UIAccessLevel.Hidden);

                bool screenDisabled = (this.GetUIItemAccessLevel("APPL_CADETS_EDITCADET") == UIAccessLevel.Disabled) ||
                                      (this.GetUIItemAccessLevel("APPL_CADETS") == UIAccessLevel.Disabled);

                if (screenHidden)
                    RedirectAccessDenied();

                //Hide Client Controls

                l = this.GetUIItemAccessLevel("APPL_CADETS_EDIT_PERSONDETAILS");

                if (l != UIAccessLevel.Enabled || screenHidden || screenDisabled)
                {
                    hiddenClientControls.Add(divEdit.ClientID);
                }

                if (l == UIAccessLevel.Hidden || screenHidden)
                {
                    hiddenClientControls.Add("lblFirstName");
                    hiddenClientControls.Add("lblFirstNameValue");
                    hiddenClientControls.Add("lblLastNameValue");

                    hiddenClientControls.Add("lblIdentNumber");
                    hiddenClientControls.Add("lblIdentNumberValue");

                    hiddenClientControls.Add("lblGender");
                    hiddenClientControls.Add("lblGenderValue");

                    hiddenClientControls.Add("lblLastModified");
                    hiddenClientControls.Add("lblLastModifiedValue");

                    hiddenClientControls.Add("lblPermAddresTitle");

                    hiddenClientControls.Add("lblPermPostCode");
                    hiddenClientControls.Add("txtPermPostCode");

                    hiddenClientControls.Add("lblPermDistrict");
                    hiddenClientControls.Add("txtPermDistrict");

                    hiddenClientControls.Add("lblPermCity");
                    hiddenClientControls.Add("txtPermCity");

                    hiddenClientControls.Add("lblPermMunicipality");
                    hiddenClientControls.Add("txtPermMunicipality");

                    hiddenClientControls.Add("lblPermRegion");
                    hiddenClientControls.Add("txtPermRegion");

                    hiddenClientControls.Add("lblPermAddress");
                    hiddenClientControls.Add("txtPermAddress");

                    hiddenClientControls.Add("lblPresAddressTitle");

                    hiddenClientControls.Add("lblPresPostCode");
                    hiddenClientControls.Add("txtPresPostCode");

                    hiddenClientControls.Add("lblPresDistrict");
                    hiddenClientControls.Add("txtPresDistrict");

                    hiddenClientControls.Add("lblPresCity");
                    hiddenClientControls.Add("txtPresCity");

                    hiddenClientControls.Add("lblPresMunicipality");
                    hiddenClientControls.Add("txtPresMunicipility");

                    hiddenClientControls.Add("lblPresRegion");
                    hiddenClientControls.Add("txtPresRegion");

                    hiddenClientControls.Add("lblPresAddress");
                    hiddenClientControls.Add("txtPresAddress");

                    hiddenClientControls.Add("lblContactAddressTitle");

                    hiddenClientControls.Add("lblContactPostCode");
                    hiddenClientControls.Add("txtPresPostCode");

                    hiddenClientControls.Add("lblPresDistrict");
                    hiddenClientControls.Add("txtContactDistrict");

                    hiddenClientControls.Add("lblContactCity");
                    hiddenClientControls.Add("txtContactCity");

                    hiddenClientControls.Add("lblContactMunicipality");
                    hiddenClientControls.Add("txtContactMunicipality");

                    hiddenClientControls.Add("lblContactRegion");
                    hiddenClientControls.Add("txtContactRegion");

                    hiddenClientControls.Add("lblContactAddress");
                    hiddenClientControls.Add("txtContactAddress");

                    hiddenClientControls.Add("lblIDCardNumber");
                    hiddenClientControls.Add("txtIDCardNumber");

                    hiddenClientControls.Add("lblIDCardIssuedBy");
                    hiddenClientControls.Add("txtIDCardIssuedBy");

                    hiddenClientControls.Add("lblIDCardIssueDate");
                    hiddenClientControls.Add("txtIDCardIssueDate");

                    hiddenClientControls.Add("lblHomePhone");
                    hiddenClientControls.Add("txtHomePhone");

                    hiddenClientControls.Add("lblMobilePhone");
                    hiddenClientControls.Add("txtMobilePhone");

                    hiddenClientControls.Add("lblEmail");
                    hiddenClientControls.Add("txtEmail");

                    hiddenClientControls.Add("lblDrvLicCategories");
                    hiddenClientControls.Add("txtDrvLicCategories");

                    hiddenClientControls.Add("lblServeInMilitary");
                    hiddenClientControls.Add("lblServeInMilitaryValue");
                }
                else
                {
                    if (this.GetUIItemAccessLevel("APPL_CADETS_EDITCADET_FIRSTNAME") == UIAccessLevel.Hidden
                        && this.GetUIItemAccessLevel("APPL_CADETS_EDITCADET_LASTNAME") == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblFirstName");
                    }

                    l = this.GetUIItemAccessLevel("APPL_CADETS_EDITCADET_FIRSTNAME");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblFirstNameValue");
                    }

                    l = this.GetUIItemAccessLevel("APPL_CADETS_EDITCADET_LASTNAME");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblLastNameValue");
                    }

                    l = this.GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EGN");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblIdentNumber");
                        hiddenClientControls.Add("lblIdentNumberValue");
                    }

                    l = this.GetUIItemAccessLevel("APPL_CADETS_EDITCADET_GENDER");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblGender");
                        hiddenClientControls.Add("lblGenderValue");
                    }


                    l = this.GetUIItemAccessLevel("APPL_CADETS_EDITCADET_PERMADDRESS_CITY");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblPermAddresTitle");

                        hiddenClientControls.Add("lblPermPostCode");
                        hiddenClientControls.Add("txtPermPostCode");

                        hiddenClientControls.Add("lblPermDistrict");
                        hiddenClientControls.Add("txtPermDistrict");

                        hiddenClientControls.Add("lblPermCity");
                        hiddenClientControls.Add("txtPermCity");

                        hiddenClientControls.Add("lblPermMunicipality");
                        hiddenClientControls.Add("txtPermMunicipality");

                        hiddenClientControls.Add("lblPermRegion");
                        hiddenClientControls.Add("txtPermRegion");
                    }

                    l = this.GetUIItemAccessLevel("APPL_CADETS_EDITCADET_PERMADDRESS_ADDRESS");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblPermAddress");
                        hiddenClientControls.Add("txtPermAddress");
                    }

                    l = this.GetUIItemAccessLevel("APPL_CADETS_EDITCADET_PRESADDRESS_CITY");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblPresAddressTitle");

                        hiddenClientControls.Add("lblPresPostCode");
                        hiddenClientControls.Add("txtPresPostCode");

                        hiddenClientControls.Add("lblPresDistrict");
                        hiddenClientControls.Add("txtPresDistrict");

                        hiddenClientControls.Add("lblPresCity");
                        hiddenClientControls.Add("txtPresCity");

                        hiddenClientControls.Add("lblPresMunicipality");
                        hiddenClientControls.Add("txtPresMunicipility");

                        hiddenClientControls.Add("lblPresRegion");
                        hiddenClientControls.Add("txtPresRegion");
                    }

                    l = this.GetUIItemAccessLevel("APPL_CADETS_EDITCADET_PRESADDRESS_ADDRESS");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblPresAddress");
                        hiddenClientControls.Add("txtPresAddress");
                    }

                    l = this.GetUIItemAccessLevel("APPL_CADETS_EDITCADET_CONTACTADDRESS_CITY");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblContactAddressTitle");

                        hiddenClientControls.Add("lblContactPostCode");
                        hiddenClientControls.Add("txtContactPostCode");

                        hiddenClientControls.Add("lblContactDistrict");
                        hiddenClientControls.Add("txtContactDistrict");

                        hiddenClientControls.Add("lblContactCity");
                        hiddenClientControls.Add("txtContactCity");

                        hiddenClientControls.Add("lblContactMunicipality");
                        hiddenClientControls.Add("txtContactMunicipality");

                        hiddenClientControls.Add("lblContactRegion");
                        hiddenClientControls.Add("txtContactRegion");
                    }

                    l = this.GetUIItemAccessLevel("APPL_CADETS_EDITCADET_CONTACTADDRESS_ADDRESS");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblContactAddress");
                        hiddenClientControls.Add("txtContactAddress");
                    }

                    l = this.GetUIItemAccessLevel("APPL_CADETS_EDITCADET_IDCARDNUMBER");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblIDCardNumber");
                        hiddenClientControls.Add("txtIDCardNumber");
                    }

                    l = this.GetUIItemAccessLevel("APPL_CADETS_EDITCADET_IDCARDISSUEDBY");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblIDCardIssuedBy");
                        hiddenClientControls.Add("txtIDCardIssuedBy");
                    }

                    l = this.GetUIItemAccessLevel("APPL_CADETS_EDITCADET_IDCARDISSUEDATE");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblIDCardIssueDate");
                        hiddenClientControls.Add("txtIDCardIssueDate");
                    }

                    l = this.GetUIItemAccessLevel("APPL_CADETS_EDITCADET_HOMEPHONE");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblHomePhone");
                        hiddenClientControls.Add("txtHomePhone");
                    }

                    l = this.GetUIItemAccessLevel("APPL_CADETS_EDITCADET_MOBILEPHONE");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblMobilePhone");
                        hiddenClientControls.Add("txtMobilePhone");
                    }

                    l = this.GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EMAIL");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblEmail");
                        hiddenClientControls.Add("txtEmail");
                    }

                    l = this.GetUIItemAccessLevel("APPL_CADETS_EDITCADET_DRIVINGLICENCE");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblDrvLicCategories");
                        hiddenClientControls.Add("txtDrvLicCategories");
                    }

                    l = this.GetUIItemAccessLevel("APPL_CADETS_EDITCADET_SERVEINMILITARY");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblServeInMilitary");
                        hiddenClientControls.Add("lblServeInMilitaryValue");
                    }
                }
            }

            SetDisabledClientControls(disabledClientControls.ToArray());
            SetHiddenClientControls(hiddenClientControls.ToArray());
        }

        //Navigate back to the home screen
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(redirectBack);
        }

        //Navigate back to the home screen
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            string redirect;
            Person person = PersonUtil.GetPerson(this.PersonId, CurrentUser);

            redirect = "~/ContentPages/AddCadet_PersonDetails.aspx?IdentNumber=" + person.IdentNumber + "&MilitaryDepartmentId=" + hdnMilitaryDepartmentId.Value + "&PageFrom=2";
            Response.Redirect(redirect);
        }

        //Load Person details (ajax call)
        private void JSLoadPersonDetails()
        {
            if (GetUIItemAccessLevel("APPL_CADETS_EDITCADET") == UIAccessLevel.Hidden
                && GetUIItemAccessLevel("APPL_CADETS") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            int personId = int.Parse(Request.Form["PersonId"]);

            string stat = "";
            string response = "";

            try
            {
                Person person = PersonUtil.GetPerson(personId, CurrentUser);
                PersonStatus personStatus = PersonUtil.GetPersonStatusByPerson(person, CurrentUser); 
             
                List<string> listRegionMunicipalityPostCode = CityUtil.GetRegionMunicipalityPostCodeByCityId(person.PermCityId, CurrentUser);

                string permRegion = "";
                string permMunicipality = "";
                string permCity = "";
                string permPostCode = "";
                if (listRegionMunicipalityPostCode.Count > 0)
                {
                    permRegion = listRegionMunicipalityPostCode[0];
                    permMunicipality = listRegionMunicipalityPostCode[1];
                    permCity = listRegionMunicipalityPostCode[2];
                    permPostCode = listRegionMunicipalityPostCode[3];
                }

                listRegionMunicipalityPostCode = CityUtil.GetRegionMunicipalityPostCodeByCityId(person.PresCityId, CurrentUser);

                string presRegion = "";
                string presMunicipality = "";
                string presCity = "";
                string presPostCode = "";
                if (listRegionMunicipalityPostCode.Count > 0)
                {
                    presRegion = listRegionMunicipalityPostCode[0];
                    presMunicipality = listRegionMunicipalityPostCode[1];
                    presCity = listRegionMunicipalityPostCode[2];
                    presPostCode = listRegionMunicipalityPostCode[3];
                }

                listRegionMunicipalityPostCode = CityUtil.GetRegionMunicipalityPostCodeByCityId(person.ContactAddress.CityId, CurrentUser);

                string contactRegion = "";
                string contactMunicipality = "";
                string contactCity = "";
                string contactPostCode = "";
                if (listRegionMunicipalityPostCode.Count > 0)
                {
                    contactRegion = listRegionMunicipalityPostCode[0];
                    contactMunicipality = listRegionMunicipalityPostCode[1];
                    contactCity = listRegionMunicipalityPostCode[2];
                    contactPostCode = listRegionMunicipalityPostCode[3];
                }

                string drivingCategory = "";
                if (person.DrivingLicenseCategories.Count > 0)
                {
                    for (int i = 0; i <= person.DrivingLicenseCategories.Count - 1; i++)
                    {
                        if (i < person.DrivingLicenseCategories.Count - 1)
                        {
                            drivingCategory += person.DrivingLicenseCategories[i].DrivingLicenseCategoryName + ", ";
                        }
                        else
                        {
                            drivingCategory += person.DrivingLicenseCategories[i].DrivingLicenseCategoryName;
                        }
                    }
                }

                string serveInMilitaryUnitName = "";

                if (person.MilitaryUnit != null)
                {
                    serveInMilitaryUnitName = person.MilitaryUnit.DisplayTextForSelection;
                }

                stat = AJAXTools.OK;
                response = @"
                    <person>
                         <personId>" + AJAXTools.EncodeForXML(person.PersonId.ToString()) + @"</personId>
                         <identNumber>" + AJAXTools.EncodeForXML(person.IdentNumber) + @"</identNumber>
                         <firstName>" + AJAXTools.EncodeForXML(person.FirstName) + @"</firstName>
                         <lastName>" + AJAXTools.EncodeForXML(person.LastName) + @"</lastName>
                         <genderName>" + AJAXTools.EncodeForXML(person.Gender == null ? "" : person.Gender.GenderName) + @"</genderName>
                         <lastModified>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDateTime(person.LastModifiedDate)) + @"</lastModified>
                         <age>" + AJAXTools.EncodeForXML(CommonFunctions.GetAgeFromEGNbyDate(person.IdentNumber, DateTime.Now, CurrentUser).ToString()) + @"</age>
                         <ageMonthsPart>" + AJAXTools.EncodeForXML(CommonFunctions.GetAgeMonthsPartFromEGNbyDate(person.IdentNumber, DateTime.Now, CurrentUser).ToString()) + @"</ageMonthsPart>
            
                         <permPostCode>" + AJAXTools.EncodeForXML(permPostCode) + @"</permPostCode>
                         <permSecondPostCode>" + AJAXTools.EncodeForXML(String.IsNullOrEmpty(person.PermSecondPostCode) ? "" : person.PermSecondPostCode) + @"</permSecondPostCode>
                         <permCity>" + AJAXTools.EncodeForXML(permCity) + @"</permCity>
                         <permRegion>" + AJAXTools.EncodeForXML(permRegion) + @"</permRegion>
                         <permDistrict>" + AJAXTools.EncodeForXML(person.PermDistrict != null ? person.PermDistrict.DistrictName : "") + @"</permDistrict>
                         <permAddress>" + AJAXTools.EncodeForXML(person.PermAddress) + @"</permAddress>
                         <permMunicipality>" + AJAXTools.EncodeForXML(permMunicipality) + @"</permMunicipality>

                         <presPostCode>" + AJAXTools.EncodeForXML(presPostCode) + @"</presPostCode>
                         <presSecondPostCode>" + AJAXTools.EncodeForXML(String.IsNullOrEmpty(person.PresSecondPostCode) ? "" : person.PresSecondPostCode) + @"</presSecondPostCode>
                         <presCity>" + AJAXTools.EncodeForXML(presCity) + @"</presCity>
                         <presRegion>" + AJAXTools.EncodeForXML(presRegion) + @"</presRegion>
                         <presDistrict>" + AJAXTools.EncodeForXML(person.PresDistrict != null ? person.PresDistrict.DistrictName : "") + @"</presDistrict>
                         <presAddress>" + AJAXTools.EncodeForXML(person.PresAddress) + @"</presAddress>
                         <presMunicipility>" + AJAXTools.EncodeForXML(presMunicipality) + @"</presMunicipility>

                         <contactPostCode>" + AJAXTools.EncodeForXML(contactPostCode) + @"</contactPostCode>
                         <contactSecondPostCode>" + AJAXTools.EncodeForXML(String.IsNullOrEmpty(person.ContactAddress.PostCode) ? "" : person.ContactAddress.PostCode) + @"</contactSecondPostCode>
                         <contactCity>" + AJAXTools.EncodeForXML(contactCity) + @"</contactCity>
                         <contactRegion>" + AJAXTools.EncodeForXML(contactRegion) + @"</contactRegion>
                         <contactDistrict>" + AJAXTools.EncodeForXML(person.ContactAddress.District != null ? person.ContactAddress.District.DistrictName : "") + @"</contactDistrict>
                         <contactAddress>" + AJAXTools.EncodeForXML(person.ContactAddress.AddressText) + @"</contactAddress>
                         <contactMunicipality>" + AJAXTools.EncodeForXML(contactMunicipality) + @"</contactMunicipality>

                         <IDCardNumber>" + AJAXTools.EncodeForXML(person.IDCardNumber) + @"</IDCardNumber>
                         <IDCardIssuedBy>" + AJAXTools.EncodeForXML(person.IDCardIssuedBy) + @"</IDCardIssuedBy>
                         <IDCardIssueDate>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(person.IDCardIssueDate)) + @"</IDCardIssueDate>
                         <homePhone>" + AJAXTools.EncodeForXML(person.HomePhone == null ? "" : person.HomePhone.ToString()) + @"</homePhone>
                         <mobilePhone>" + AJAXTools.EncodeForXML(person.MobilePhone == null ? "" : person.MobilePhone.ToString()) + @"</mobilePhone>
                         <email>" + AJAXTools.EncodeForXML(person.Email) + @"</email>
                         <drvLicCategories>" + AJAXTools.EncodeForXML(drivingCategory) + @"</drvLicCategories>
                         <militaryUnitName>" + serveInMilitaryUnitName + @"</militaryUnitName>
                         
                         <medCertHTML>" + AJAXTools.EncodeForXML(GetMedCertTable(person.PersonId)) + @"</medCertHTML>
                         <psychCertHTML>" + AJAXTools.EncodeForXML(GetPsychCertTable(person.PersonId)) + @"</psychCertHTML>";

                response += "<PersonStatus>";
                response += "<PersonStatus_Status>" + AJAXTools.EncodeForXML(personStatus.Status) + "</PersonStatus_Status>";
                response += "<PersonStatus_Details>";
                foreach (var d in personStatus.Details)
                {
                    response += "<PersonStatus_Detail>";
                    response += "<PersonStatus_Detail_Key>" + AJAXTools.EncodeForXML(d.Key) + "</PersonStatus_Detail_Key>";
                    response += "<PersonStatus_Detail_Value>" + AJAXTools.EncodeForXML(d.Value) + "</PersonStatus_Detail_Value>";
                    response += "</PersonStatus_Detail>";
                }
                response += "</PersonStatus_Details>";
                response += "</PersonStatus>";
                response += "</person>";
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

        //Load a particular tab's content
        private void JSDisplayTab(string selectedTabId)
        {
            string html = "";

            //Select case to load data
            switch (selectedTabId)
            {
                case "btnTabSubjects":
                    {
                        if (GetUIItemAccessLevel("APPL_CADETS_EDITCADET_SPECS") != UIAccessLevel.Hidden &&
                            GetUIItemAccessLevel("APPL_CADETS_EDITCADET") != UIAccessLevel.Hidden &&
                            GetUIItemAccessLevel("APPL_CADETS") != UIAccessLevel.Hidden)
                        {
                            html = GenerateTabSubjectsContent(String.Empty, false);
                        }
                    }
                    break;
                case "btnTabEducation":
                    html = this.GenerateTabEducationContent();
                    break;
                default:
                    break;
            }

            string response = "<TabHTML>" + AJAXTools.EncodeForXML(html) + "</TabHTML>";
            string stat = AJAXTools.OK;

            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        //Generate the content of subjects tab
        private string GenerateTabSubjectsContent(string message, bool existError)
        {
            bool pageDisabled = false;

            Person person = PersonUtil.GetPerson(this.HdnPersonId, CurrentUser);
 
            if (Config.GetWebSetting("KOD_KZV_Check_Cadet").ToLower() == "true" &&
                CommonFunctions.IsKeyInList(person.PersonTypeCode, Config.GetWebSetting("KOD_KZV_Restricted_Cadet")) || !CanCurrentUserAccessThisMilDepartment)
            {
                pageDisabled = true;
            }

            bool isReadOnly = false;
            if (GetUIItemAccessLevel("APPL_CADETS_EDITCADET_SPECS") == UIAccessLevel.Disabled ||
                GetUIItemAccessLevel("APPL_CADETS_EDITCADET") == UIAccessLevel.Disabled ||
                GetUIItemAccessLevel("APPL_CADETS") == UIAccessLevel.Disabled ||
                pageDisabled)
            {
                isReadOnly = true;
            }

            DateTime dateTimeNow = DateTime.Now;

            List<MilitarySchoolYear> militarySchoolYears = MilitarySchoolSpecializationUtil.GetAllYearsByPersonID(this.HdnPersonId, CurrentUser);

            MilitarySchoolYear currentYear = new MilitarySchoolYear() { Year = dateTimeNow.Year, YearValue = dateTimeNow.Year.ToString() };
            if (!militarySchoolYears.Exists(msy => msy.Value() == currentYear.Value()))
            {
                militarySchoolYears.Add(currentYear);
            }

            MilitarySchoolYear nextYear = new MilitarySchoolYear() { Year = dateTimeNow.Year + 1, YearValue = (dateTimeNow.Year + 1).ToString() };
            if (!militarySchoolYears.Exists(msy => msy.Value() == nextYear.Value()))
            {
                militarySchoolYears.Add(nextYear);
            }

            MilitarySchoolYear selectedYearObj = null;
            int selectedYearPar = 0;
            if (!String.IsNullOrEmpty(Request.Params["Year"])
                && Int32.TryParse(Request.Params["Year"].ToString(), out selectedYearPar)
                && selectedYearPar != 0)
            {
                selectedYearObj = new MilitarySchoolYear() { Year = selectedYearPar, YearValue = selectedYearPar.ToString() };
            }
            else
            {
                selectedYearObj = new MilitarySchoolYear() { Year = dateTimeNow.Year, YearValue = dateTimeNow.Year.ToString() };
            }

            List<IDropDownItem> ddiMilitarySchoolYears = new List<IDropDownItem>();
            foreach (MilitarySchoolYear militarySchoolYear in militarySchoolYears)
            {
                ddiMilitarySchoolYears.Add(militarySchoolYear);
            }

            IDropDownItem selectedYearItem = null;

            if (selectedYearObj != null)
            {
                selectedYearItem = (ddiMilitarySchoolYears.Count > 0 ? ddiMilitarySchoolYears.Find(va => va.Value() == selectedYearObj.Value()) : null);
            }
            else
            {
                selectedYearItem = (ddiMilitarySchoolYears.Count > 0 ? ddiMilitarySchoolYears[0] : null);
            }

            // Generates html for years drop down list
            string militarySchoolYearsHTML = ListItems.GetDropDownHtml(ddiMilitarySchoolYears, null, "ddlMilitarySchoolYears", false, selectedYearItem, "ddlMilitarySchoolYearChange(this)", "style='min-width: 100px;'");

            if (selectedYearItem != null && selectedYearPar == 0)
            {
                int.TryParse(selectedYearItem.Value(), out selectedYearPar);
            }

            if (selectedYearPar < DateTime.Now.Year)
            {
                isReadOnly = true;
            }

            List<CadetSchoolSubject> cadetSchoolSubjects = CadetSchoolSubjectUtil.GetAllCadetSchoolSubjectsByPersonID(this.HdnPersonId, selectedYearPar, CurrentUser);

            StringBuilder sb = new StringBuilder();

            //Generate Cadet Military School Subjects table
            sb.Append("<table style='width: 900px;'>");
            sb.Append("<tr><td align='center' style='width: 400px'><span class='InputLabel'>Учебна година: </span>" + militarySchoolYearsHTML + "</td>");

            if (isReadOnly)
            {
                sb.Append("<td style='width: 500px'>&nbsp;</td></tr>");
            }
            else
            {
                sb.Append("<td align='left' style='width: 500px'><div id='btnAddSubjects' class='Button' onclick='ShowMilitarySchoolSpecializationsLightBox(1, 1)'><i></i><div style='width:200px; padding-left:5px;'>Добавяне на специалност(и)</div><b></b></div></td></tr>");
            }

            sb.Append("<tr><td colspan='2' align='center'>");

            sb.Append("<table><tr><td>");
            sb.Append("<table id='cadetSchoolSubjectsTable' name='cadetSchoolSubjectsTable' class='CommonHeaderTable' style='text-align: left;' border='1px'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style=\"width: 30px;\">№</th>");
            sb.Append("<th style=\"width: 200px;\">Военно училище</th>");
            sb.Append("<th style=\"width: 300px;\">Специалност</th>");
            sb.Append("<th style=\"width: 300px;\">Специализация</th>");
            sb.Append("<th style=\"width: 30px;\"></th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            int counter = 1;

            if (cadetSchoolSubjects.Count > 0)
            {
                sb.Append("<tbody>");
            }

            foreach (CadetSchoolSubject cadetSchoolSubject in cadetSchoolSubjects)
            {
                string deleteHTML = "";

                if (this.GetUIItemAccessLevel("APPL_CADETS_EDITCADET_SPECS") == UIAccessLevel.Enabled &&
                    this.GetUIItemAccessLevel("APPL_CADETS_EDITCADET") == UIAccessLevel.Enabled &&
                    this.GetUIItemAccessLevel("APPL_CADETS") == UIAccessLevel.Enabled &&
                    !isReadOnly)
                {
                    deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на тази специализация' class='GridActionIcon' onclick='DeleteMilitarySchoolSpecialization(" + cadetSchoolSubject.CadetSchoolSubjectId.ToString() + ");' />";
                }

                sb.Append("<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + "'>");
                sb.Append("<td>" + counter + "</td>");
                sb.Append("<td>" + MilitarySchoolUtil.GetMilitarySchool(cadetSchoolSubject.MilitarySchoolSpecialization.MilitarySchoolId, CurrentUser).MilitarySchoolName + "</td>");
                sb.Append("<td>" + cadetSchoolSubject.MilitarySchoolSpecialization.Specialization.MilitarySchoolSubject.MilitarySchoolSubjectName + "</td>");
                sb.Append("<td>" + cadetSchoolSubject.MilitarySchoolSpecialization.Specialization.SpecializationName + "</td>");
                sb.Append("<td align='center'>" + deleteHTML + "</td>");

                sb.Append("</tr>");
                counter++;
            }

            if (cadetSchoolSubjects.Count > 0)
            {
                sb.Append("</tbody>");
            }

            sb.Append("</table>");

            sb.Append("</td></tr>");

            string messageClass = "";
            if (existError)
            {
                messageClass = "ErrorText";
            }
            else
            {
                messageClass = "SuccessText";
            }

            sb.Append("<tr><td colspan='2' align='center'><span id='spanSubjectMessage' class='" + messageClass + "'>" + message + "</span></td></tr>");

            sb.Append("</table>");

            return sb.ToString();
        }

        //Generate html content for military school specializations light box
        public void GenerateMilitarySchoolSpecializationsLightBoxContent()
        {
            if (GetUIItemAccessLevel("APPL_CADETS") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("APPL_CADETS_EDITCADET") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("APPL_CADETS_EDITCADET_SPECS") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int year = 0;
                if (!String.IsNullOrEmpty(Request.Params["Year"]))
                    int.TryParse(Request.Params["Year"], out year);

                if (this.HdnPersonId == 0 || year == 0)
                {
                    throw new Exception();
                }

                string orderByStr = Request.Params["SpecTableOrderBy"];
                string pageIdxStr = Request.Params["SpecTablePageIdx"];

                // Get the config setting that says how many rows per page should be dispayed in the grid
                int rowsPerPage = int.Parse(Config.GetWebSetting("RowsPerPage"));
                // Stores information about how many pages are in the grid
                int maxPage;

                List<MilitarySchool> militarySchools = MilitarySchoolUtil.GetAllMilitarySchools(CurrentUser, true);

                if (militarySchools.Count == 0)
                {
                    throw new Exception();
                }

                int militarySchoolId = 0;
                if (!String.IsNullOrEmpty(Request.Params["MilitarySchoolId"]))
                    int.TryParse(Request.Params["MilitarySchoolId"], out militarySchoolId);

                MilitarySchool militarySchoolPar = MilitarySchoolUtil.GetMilitarySchool(militarySchoolId, CurrentUser);

                // Generates html for military schools drop down list
                List<IDropDownItem> ddiMilitarySchools = new List<IDropDownItem>();
                foreach (MilitarySchool militarySchool in militarySchools)
                {
                    ddiMilitarySchools.Add(militarySchool);
                }

                IDropDownItem selectedMilitarySchool = null;

                if (militarySchoolPar != null)
                {
                    selectedMilitarySchool = (ddiMilitarySchools.Count > 0 ? ddiMilitarySchools.Find(pe => pe.Value() == militarySchoolPar.Value()) : null);
                }
                else
                {
                    selectedMilitarySchool = (ddiMilitarySchools.Count > 0 ? ddiMilitarySchools[0] : null);
                }

                string militarySchoolsHTML = ListItems.GetDropDownHtml(ddiMilitarySchools, null, "ddlMilitarySchools", false, selectedMilitarySchool, null, "style='min-width: 200px;'");

                MilitarySchoolSpecializationFilter filter = new MilitarySchoolSpecializationFilter()
                {
                    MilitarySchoolId = (militarySchoolId > 0 ? militarySchoolId : militarySchools[0].MilitarySchoolId),
                    Year = year,
                    RowsPerPage = rowsPerPage
                };

                int allRows = MilitarySchoolSpecializationUtil.GetAllMilitarySchoolSpecializationsByFilterCount(filter, this.HdnPersonId, CurrentUser);
                // Get the number of rows and calculate the number of pages in the grid
                maxPage = rowsPerPage == 0 ? 1 : allRows / rowsPerPage + (allRows != 0 && allRows % rowsPerPage == 0 ? 0 : 1);

                string html = "";

                // Collect order control and the paging control data from the page
                int orderBy = 1;
                if (!String.IsNullOrEmpty(orderByStr))
                {
                    int.TryParse(orderByStr, out orderBy);
                }

                filter.OrderBy = orderBy;

                int pageIdx = 1;
                if (!String.IsNullOrEmpty(pageIdxStr))
                {
                    int.TryParse(pageIdxStr, out pageIdx);
                }

                filter.PageIndex = pageIdx;

                // Get the list of specialization items according to the specified order and paging
                List<MilitarySchoolSpecialization> militSchoolSpecs = MilitarySchoolSpecializationUtil.GetAllMilitarySchoolSpecializationsByFilter(filter, this.HdnPersonId, CurrentUser);


                // If there is data then generate dynamically the HTML for the data grid
                string headerStyle = "vertical-align: bottom;";
                int orderCol = (orderBy > 100 ? orderBy - 100 : orderBy);
                string img = orderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
                string[] arrOrderCol = { "", "" };
                arrOrderCol[orderCol - 1] = @"<div style='position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

                // Refresh the paging image buttons
                string btnFirst = "src='../Images/ButtonFirst.png'";
                string btnPrev = "src='../Images/ButtonPrev.png'";
                string btnLast = "src='../Images/ButtonLast.png'";
                string btnNext = "src='../Images/ButtonNext.png'";

                if (pageIdx == 1)
                {
                    btnFirst = "src='../Images/ButtonFirstDisabled.png' disabled='true'";
                    btnPrev = "src='../Images/ButtonPrevDisabled.png' disabled='true'";
                }

                if (pageIdx == maxPage)
                {
                    btnLast = "src='../Images/ButtonLastDisabled.png' disabled='true'";
                    btnNext = "src='../Images/ButtonNextDisabled.png' disabled='true'";
                }

                // Set current page number
                string specializationsTablePagination = " | " + pageIdx + " от " + maxPage + " | ";

                // Setup the header of the grid
                html = @"<div style='min-height: 150px; margin-bottom: 10px;'>
                    <input type='hidden' id='hdnMilitarySchoolSpecializationsTableOrderBy' value='" + orderBy + @"' />
                    <input type='hidden' id='hdnMilitarySchoolSpecializationsTablePageIdx' value='" + pageIdx + @"' />
                    <input type='hidden' id='hdnMilitarySchoolSpecializationsTableMaxPage' value='" + maxPage + @"' />

                    <span class='HeaderText'>Специалности и специализации</span><br /><br /><br />

                    <table width='100%'>
                            <tr>
                                <td align='right' style='width: 200px;'>
                                    <span id='lblMilitarySchool' class='InputLabel'>Военно училище:</span>
                                </td>
                                <td align='left' style='min-width: 220px;'>
                                    <span id='spMilitarySchool'>" + militarySchoolsHTML + @"</span>
                                </td>
                                <td>
                                    <div id='btnSearchMilitarySchoolSpecializations' runat='server' class='Button' onclick='GetMilitarySchoolSpecializationItems(1, 1);'><i></i><div style='width:70px; padding-left:5px;'>Покажи</div><b></b></div>
                                </td>
                            </tr>
                        </table>

                    <div style='text-align: center;'>
                       <div style='display: inline; position: relative; top: -10px;'>
                          <img id='btnMilitarySchoolSpecializationsTableFirst' " + btnFirst + @" alt='Първа страница' title='Първа страница' class='PaginationButton' onclick=""BtnMilitarySchoolSpecializationsTableFirstClick();"" />
                          <img id='btnMilitarySchoolSpecializationsTablePrev' " + btnPrev + @" alt='Предишна страница' title='Предишна страница' class='PaginationButton' onclick=""BtnMilitarySchoolSpecializationsTablePrevClick();"" />
                          <span id='lblMilitarySchoolSpecializationsTablePagination' class='PaginationLabel'>" + specializationsTablePagination + @"</span>
                          <img id='btnMilitarySchoolSpecializationsTableNext' " + btnLast + @" alt='Следваща страница' title='Следваща страница' class='PaginationButton' onclick=""BtnMilitarySchoolSpecializationsTableNextClick();"" />
                          <img id='btnMilitarySchoolSpecializationsTableLast' " + btnNext + @" alt='Последна страница' title='Последна страница' class='PaginationButton' onclick=""BtnMilitarySchoolSpecializationsTableLastClick();"" />
                          
                          <span style='padding: 0 30px'>&nbsp;</span>
                          <span style='text-align: right;'>Отиди на страница</span>
                          <input id='txtMilitarySchoolSpecializationsTableGotoPage' class='InputField' type='text' style='width: 30px;' value='' />
                          <img id='btnMilitarySchoolSpecializationsTableGoto' src='../Images/ButtonGoto.png' alt='Отиди на страница' title='Отиди на страница' class='PaginationButton' onclick=""BtnMilitarySchoolSpecializationsTableGotoClick();"" />
                       </div>
                    </div>";

                // No data found
                if (militSchoolSpecs.Count == 0)
                {
                    html += "<div style='width: 730px; padding-top: 20px; padding-bottom: 20px;'>Няма намерени резултати</div>";
                }
                else
                {
                    html += @"<table id='MilitarySchoolSpecializationsTable' class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                     <thead>
                        <tr>
                           <th style='width: 20px;" + headerStyle + @"'>№</th>
                           <th style='vertical-align: middle; width: 350px; cursor: pointer;" + headerStyle + @"' onclick='SortMilitarySchoolSpecializationsTableBy(1);'>Специалност" + arrOrderCol[0] + @"</th>
                           <th style='vertical-align: middle; width: 350px; cursor: pointer;" + headerStyle + @"' onclick='SortMilitarySchoolSpecializationsTableBy(2);'>Специализация" + arrOrderCol[1] + @"</th>
                        </tr>
                     </thead>";

                    int counter = 1;

                    //Iterate through all items and add them into the grid
                    foreach (MilitarySchoolSpecialization militSchoolSpec in militSchoolSpecs)
                    {
                        string cellStyle = "vertical-align: top;";

                        html += @"<tr style='min-height: 17px; cursor: pointer;' class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"' onclick='AddMilitarySchoolSpecializationToPerson(" + militSchoolSpec.MilitarySchoolSpecializationId.ToString() + @");'>
                             <td align='center' style='" + cellStyle + @"'>" + ((pageIdx - 1) * rowsPerPage + counter).ToString() + @"</td>
                             <td style='" + cellStyle + @"'>" + militSchoolSpec.Specialization.MilitarySchoolSubject.MilitarySchoolSubjectName + @"</td>
                             <td style='" + cellStyle + @"'>" + militSchoolSpec.Specialization.SpecializationName + @"</td>
                          </tr>";

                        counter++;
                    }

                    html += "</table><br />";
                }

                html += @"</div>
                      <span id='spMilitarySchoolSpecializationsLightBoxMessage' style='display: none'></span><br />
                      <div id='btnCloseMilitarySchoolSpecializationsLightBox' runat='server' class='Button' onclick='HideMilitarySchoolSpecializationsLightBox();'><i></i><div style='width:70px; padding-left:5px;'>Затвори</div><b></b></div>";


                stat = AJAXTools.OK;
                response = AJAXTools.EncodeForXML(html);
            }
            catch
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.ERROR;
            }

            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        //Save an cadet military school specialization record (ajax call)
        private void JSAddMilitarySchoolSpecializationToPerson()
        {
            if (GetUIItemAccessLevel("APPL_CADETS") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("APPL_CADETS_EDITCADET") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("APPL_CADETS_EDITCADET_SPECS") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int militarySchoolSpecializationId = 0;
                int.TryParse(Request.Params["MilitarySchoolSpecializationId"], out militarySchoolSpecializationId);

                int militaryDepartmentId = 0;
                int.TryParse(Request.Params["MilitaryDepartmentId"], out militaryDepartmentId);

                if (this.HdnPersonId == 0 || militarySchoolSpecializationId == 0
                    || militaryDepartmentId == 0)
                {
                    throw new Exception();
                }

                Change change = new Change(CurrentUser, "APPL_Cadets");

                Cadet cadet = CadetUtil.GetCadet(this.HdnPersonId, militaryDepartmentId, CurrentUser);

                if (cadet == null)
                {
                    cadet = new Cadet(CurrentUser) 
                    { 
                        PersonId = this.HdnPersonId,
                        MilitaryDepartmentId = militaryDepartmentId
                    };

                    CadetUtil.SaveCadet(cadet, CurrentUser, change);
                }
                
                string message = "";

                if (cadet.CadetId != 0)
                {
                    message = "Специализацията е добавена успешно";
                }
                else
                {
                    message = "Специализацията не е добавена успешно";
                }

                if (cadet.CadetId != 0)
                {
                    CadetSchoolSubject cadetSchoolSubject = new CadetSchoolSubject(CurrentUser) 
                    { 
                        CadetId = cadet.CadetId,
                        MilitSchoolSpecId = militarySchoolSpecializationId,
                        IsRanked = false
                    };

                    CadetSchoolSubjectUtil.SaveCadetSchoolSubject(cadetSchoolSubject, CurrentUser, change);
                }

                change.WriteLog();

                stat = AJAXTools.OK;
                response = AJAXTools.EncodeForXML(this.GenerateTabSubjectsContent(message, false));
            }
            catch
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.ERROR;
            }

            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        //Delete an cadet military school specialization record (ajax call)
        private void JSDeleteMilitarySchoolSpecialization()
        {
            if (GetUIItemAccessLevel("APPL_CADETS") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("APPL_CADETS_EDITCADET") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("APPL_CADETS_EDITCADET_SPECS") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            int cadetSchoolSubjectId = 0;
            int.TryParse(Request.Params["CadetSchoolSubjectId"], out cadetSchoolSubjectId);

            if (this.HdnPersonId == 0 || cadetSchoolSubjectId == 0)
            {
                throw new Exception();
            }

            CadetSchoolSubject cadetSchoolSubject = CadetSchoolSubjectUtil.GetCadetSchoolSubject(cadetSchoolSubjectId, CurrentUser);

            string status = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "APPL_Cadets");

                if (!CadetSchoolSubjectUtil.DeleteCadetSchoolSubject(cadetSchoolSubject.CadetSchoolSubjectId, CurrentUser, change))
                {
                    throw new Exception();
                }

                change.WriteLog();

                status = AJAXTools.OK;
                response = AJAXTools.EncodeForXML(this.GenerateTabSubjectsContent("Специализацията е изтрита успешно", false));
            }
            catch (Exception ex)
            {
                status = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }


            AJAX a = new AJAX(response, status, Response);
            a.Write();
            Response.End();
        }

        //Generate the content of education tab
        private string GenerateTabEducationContent()
        {
            if (this.HdnPersonId > 0)
            {
                string html = "";

                if (GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU") != UIAccessLevel.Hidden &&
                    GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU_EDU") != UIAccessLevel.Hidden &&
                    GetUIItemAccessLevel("APPL_CADETS_EDITCADET") != UIAccessLevel.Hidden &&
                    GetUIItemAccessLevel("APPL_CADETS") != UIAccessLevel.Hidden)
                {
                    html += "<div id='divCadetEduTable'>";
                    html += this.GenerateCadetEducationsTable(String.Empty, false);
                    html += "</div>";
                    html += "<br />";
                }

                if (GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU") != UIAccessLevel.Hidden &&
                    GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU_LANG") != UIAccessLevel.Hidden &&
                    GetUIItemAccessLevel("APPL_CADETS_EDITCADET") != UIAccessLevel.Hidden &&
                    GetUIItemAccessLevel("APPL_CADETS") != UIAccessLevel.Hidden)
                {
                    html += "<div id='divCadetlLangTable'>";
                    html += this.GenerateCadetLanguagesTable(String.Empty, false);
                    html += "</div>";
                }

                return html;
            }
            else
            {
                return "";
            }
        }

        //Generate cadet education table
        private string GenerateCadetEducationsTable(string message, bool existError)
        {
            bool pageDisabled = false;

            bool IsEducationHidden = this.GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU_EDU_EDU") == UIAccessLevel.Hidden;
            bool IsSubjectHidden = this.GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU_EDU_SUBJ") == UIAccessLevel.Hidden;
            bool IsYearHidden = this.GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU_EDU_YEAR") == UIAccessLevel.Hidden;
            bool IsLearningMethodHidden = this.GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU_EDU_LEARNINGMETHOD") == UIAccessLevel.Hidden;

            if (IsEducationHidden &&
                IsSubjectHidden &&
                IsYearHidden &&
                IsLearningMethodHidden
                )
            {
                return "";
            }

            Person person = PersonUtil.GetPerson(this.HdnPersonId, CurrentUser);

            if (Config.GetWebSetting("KOD_KZV_Check_Cadet").ToLower() == "true" &&
                CommonFunctions.IsKeyInList(person.PersonTypeCode, Config.GetWebSetting("KOD_KZV_Restricted_Cadet")) || !CanCurrentUserAccessThisMilDepartment)
            {
                pageDisabled = true;
            }

            //Generate Education table
            string html = "";
            List<PersonCivilEducation> civilEducations = PersonCivilEducationUtil.GetAllPersonCivilEducationsByPersonID(this.HdnPersonId, CurrentUser);

            html += "<table>";
            html += "<tr><td align='left'><span class='SmallHeaderText'>Образование</span></td></tr>";
            html += "<tr><td align='center'>";

            //No data found
            if (civilEducations.Count == 0)
            {
                html += "<span>Няма въведена информация</span>";
            }
            //If there is data then generate dynamically the HTML for the data grid
            else
            {
                string headerStyle = "vertical-align: bottom;";

                //Setup the header of the grid
                html += @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                                <th style='width: 20px;" + headerStyle + @"'>№</th>
     " + (!IsEducationHidden ? @"<th style='width: 200px;" + headerStyle + @"'>Образователна степен</th>" : "") + @"
       " + (!IsSubjectHidden ? @"<th style='width: 320px;" + headerStyle + @"'>Специалност</th>" : "") + @"
          " + (!IsYearHidden ? @"<th style='width: 80px;" + headerStyle + @"'>Година на завършване</th>" : "") + @"
" + (!IsLearningMethodHidden ? @"<th style='width: 120px;" + headerStyle + @"'>Начин на обучение</th>" : "") + @"
                                 <th style='width: 60px;" + headerStyle + @"'>&nbsp;</th>
                            </tr>
                         </thead>";

                int counter = 1;

                //Iterate through all items and add them into the grid
                foreach (PersonCivilEducation civilEducation in civilEducations)
                {
                    string cellStyle = "vertical-align: top;";

                    string deleteHTML = "";
                    string editHTML = "";

                    if (GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU") == UIAccessLevel.Enabled &&
                        GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU_EDU") == UIAccessLevel.Enabled &&
                        GetUIItemAccessLevel("APPL_CADETS_EDITCADET") == UIAccessLevel.Enabled &&
                        GetUIItemAccessLevel("APPL_CADETS") == UIAccessLevel.Enabled &&
                        !pageDisabled)
                    {
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на образование' class='GridActionIcon' onclick='DeleteCadetEducation(" + civilEducation.CivilEducationId.ToString() + ");' />";
                        editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='ShowCadetEducationLightBox(" + civilEducation.CivilEducationId.ToString() + ");' />";
                    }

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td align='center' style='" + cellStyle + @"'>" + counter.ToString() + @"</td>
     " + (!IsEducationHidden ? @"<td style='" + cellStyle + @"'>" + (civilEducation.PersonEducation != null ? civilEducation.PersonEducation.PersonEducationName : "") + @"</td>" : "") + @"
       " + (!IsSubjectHidden ? @"<td style='" + cellStyle + @"'>" + (civilEducation.PersonSchoolSubject != null ? civilEducation.PersonSchoolSubject.PersonSchoolSubjectName.ToString() : "") + @"</td>" : "") + @"
          " + (!IsYearHidden ? @"<td style='" + cellStyle + @"'>" + civilEducation.GraduateYear.ToString() + @"</td>" : "") + @"
" + (!IsLearningMethodHidden ? @"<td style='" + cellStyle + @"'>" + (civilEducation.LearningMethod != null ? civilEducation.LearningMethod.LearningMethodName.ToString() : "") + @"</td>" : "") + @"
                                 <td align='center' valign='middle' style='" + cellStyle + @"'>" + editHTML + deleteHTML + @"</td>
                              </tr>";

                    counter++;
                }

                html += "</table><br />";

                string messageClass = "";
                if (existError)
                {
                    messageClass = "ErrorText";
                }
                else
                {
                    messageClass = "SuccessText";
                }

                html += "<span id='spanEducationMessage' class='" + messageClass + "'>" + message + "</span>";
            }
            html += "</td></tr>";

            if (GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU") == UIAccessLevel.Enabled &&
                GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU_EDU") == UIAccessLevel.Enabled &&
                GetUIItemAccessLevel("APPL_CADETS_EDITCADET") == UIAccessLevel.Enabled &&
                GetUIItemAccessLevel("APPL_CADETS") == UIAccessLevel.Enabled &&
                !pageDisabled)
            {
                html += "<tr><td>";
                html += "<div id='btnAddNewCadetEducation' style='display: inline;' onclick='ShowCadetEducationLightBox(0);' class='Button'><i></i><div id='btnAddNewCadetEducationText' style='width:110px;'>Добавяне на ново</div><b></b></div><br />";
                html += "</td></tr>";
            }

            html += "</table>";

            return html;
        }

        //Save an cadet education record (ajax call)
        private void JSSaveCadetEducation()
        {
            if (GetUIItemAccessLevel("APPL_CADETS") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("APPL_CADETS_EDITCADET") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int hdnPersonId = 0;
                int.TryParse(Request.Params["HdnPersonId"], out hdnPersonId);

                int civilEducationId = 0;
                int.TryParse(Request.Params["CivilEducationId"], out civilEducationId);
                
                Change change = new Change(CurrentUser, "APPL_Cadets");

                string message = "";

                if (hdnPersonId != 0 && civilEducationId != 0)
                {
                    message = "Образованието е редактирано успешно";
                }
                else if (hdnPersonId != 0 && civilEducationId == 0)
                {
                    message = "Образованието е добавено успешно";
                }
                else
                {
                    throw new Exception();
                }

                string personEducationCode = Request.Params["PersonEducationCode"];
                string personSchoolSubjectCode = Request.Params["PersonSchoolSubjectCode"];
                int graduateYear = int.Parse(Request.Params["GraduateYear"]);
                string learningMethodKey = Request.Params["LearningMethodKey"];

                Person person = PersonUtil.GetPerson(hdnPersonId, CurrentUser);

                PersonCivilEducation existingPersonCivilEducation = PersonCivilEducationUtil.GetPersonCivilEducation(person.IdentNumber, personEducationCode, graduateYear, CurrentUser);

                if (existingPersonCivilEducation != null &&
                    existingPersonCivilEducation.CivilEducationId != civilEducationId)
                {
                    message = "Избраната образователна степен вече е въведена за избраната година на завършване";

                    stat = AJAXTools.OK;
                    response = AJAXTools.EncodeForXML(this.GenerateCadetEducationsTable(message, true));
                }
                else
                {
                    PersonCivilEducation personCivilEducation = new PersonCivilEducation(CurrentUser);

                    personCivilEducation.CivilEducationId = civilEducationId;
                    personCivilEducation.PersonEducationCode = personEducationCode;
                    personCivilEducation.PersonSchoolSubjectCode = personSchoolSubjectCode;
                    personCivilEducation.GraduateYear = graduateYear;
                    personCivilEducation.LearningMethod = LearningMethodUtil.GetLearningMethod(CurrentUser, learningMethodKey);

                    PersonCivilEducationUtil.SavePersonCivilEducation(personCivilEducation, person, CurrentUser, change);
                    change.WriteLog();

                    stat = AJAXTools.OK;
                    response = AJAXTools.EncodeForXML(this.GenerateCadetEducationsTable(message, false));
                }
            }
            catch
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.ERROR;
            }

            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        //Delete an cadet education record (ajax call)
        private void JSDeleteCadetEducation()
        {
            if (GetUIItemAccessLevel("APPL_CADETS") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("APPL_CADETS_EDITCADET") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            string status = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "APPL_Cadets");

                int hdnPersonId = int.Parse(Request.Params["HdnPersonId"]);
                int civilEducationId = int.Parse(Request.Params["CivilEducationId"]);

                Person person = PersonUtil.GetPerson(hdnPersonId, CurrentUser);

                PersonCivilEducationUtil.DeletePersonCivilEducation(civilEducationId, person, CurrentUser, change);

                change.WriteLog();

                status = AJAXTools.OK;
                response = AJAXTools.EncodeForXML(this.GenerateCadetEducationsTable("Образованието е изтрито успешно", false));
            }
            catch (Exception ex)
            {
                status = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }

            AJAX a = new AJAX(response, status, Response);
            a.Write();
            Response.End();
        }

        //Get the UIItems info for the CivilEducation table
        public string GetCivilEducationUIItems()
        {
            string UIItemsXML = "";

            //Setup the "manually add positions" light-box
            List<string> disabledClientControls = new List<string>();
            List<string> hiddenClientControls = new List<string>();

            UIAccessLevel l;

            l = GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU_EDU_EDU");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonEducation");
                disabledClientControls.Add("ddCivilEduPersonEducation");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonEducation");
                hiddenClientControls.Add("ddCivilEduPersonEducation");
            }

            l = GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU_EDU_SUBJ");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblCivilEduPersonSchoolSubject");
                disabledClientControls.Add("txtSubject");
                hiddenClientControls.Add("btnSelectCivilSubject");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblCivilEduPersonSchoolSubject");
                hiddenClientControls.Add("txtSubject");
                hiddenClientControls.Add("btnSelectCivilSubject");
            }

            l = GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU_EDU_YEAR");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblCivilEduGraduateYear");
                disabledClientControls.Add("txtCivilEduGraduateYear");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblCivilEduGraduateYear");
                hiddenClientControls.Add("txtCivilEduGraduateYear");
            }

            l = GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU_EDU_LEARNINGMETHOD");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblCivilEduLearningMethod");
                disabledClientControls.Add("ddCivilEduLearningMethod");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblCivilEduLearningMethod");
                hiddenClientControls.Add("ddCivilEduLearningMethod");
            }


            string disabledClientControlsIds = "";
            string hiddenClientControlsIds = "";

            foreach (string disabledClientControl in disabledClientControls)
            {
                disabledClientControlsIds += (String.IsNullOrEmpty(disabledClientControlsIds) ? "" : ",") +
                    disabledClientControl;
            }

            foreach (string hiddenClientControl in hiddenClientControls)
            {
                hiddenClientControlsIds += (String.IsNullOrEmpty(hiddenClientControlsIds) ? "" : ",") +
                    hiddenClientControl;
            }

            UIItemsXML = "<disabledClientControls>" + AJAXTools.EncodeForXML(disabledClientControlsIds) + "</disabledClientControls>" +
                         "<hiddenClientControls>" + AJAXTools.EncodeForXML(hiddenClientControlsIds) + "</hiddenClientControls>";

            return UIItemsXML;
        }

        //Generate html content for cadet education light box
        private void GenerateCadetEducationLightBoxContent()
        {
            int civilEducationId = 0;
            int.TryParse(Request.Params["CivilEducationId"], out civilEducationId);
            PersonCivilEducation civilEducation = PersonCivilEducationUtil.GetPersonCivilEducation(civilEducationId, CurrentUser);

            // Generates html for person educations drop down list
            List<PersonEducation> personEducations = PersonEducationUtil.GetAllPersonEducations(CurrentUser);
            List<IDropDownItem> ddiPersonEducations = new List<IDropDownItem>();
            foreach (PersonEducation personEducation in personEducations)
            {
                ddiPersonEducations.Add(personEducation);
            }

            IDropDownItem selectedPersonEducation = null;
            if (civilEducation != null)
            {
                selectedPersonEducation = (ddiPersonEducations.Count > 0 ? ddiPersonEducations.Find(pe => pe.Value() == civilEducation.PersonEducation.Value()) : null);
            }

            string personEducationsHTML = ListItems.GetDropDownHtml(ddiPersonEducations, null, "ddCivilEduPersonEducation", true, selectedPersonEducation, "", "style='width: 150px;' UnsavedCheckSkipMe='true' class='RequiredInputField' ");

            // Generates html for drop down list
            List<LearningMethod> listLearningMethod = LearningMethodUtil.GetLearningMethods(CurrentUser);
            List<IDropDownItem> ddiLearningMethod = new List<IDropDownItem>();

            foreach (LearningMethod learningMethod in listLearningMethod)
            {
                ddiLearningMethod.Add(learningMethod);
            }

            IDropDownItem selectedPersonLearningMethod = null;
            if (civilEducation != null)
            {
                selectedPersonLearningMethod = (ddiLearningMethod.Count > 0 ? ddiLearningMethod.Find(plm => plm.Value() == civilEducation.LearningMethod.LearningMethodKey) : null);
            }

            string learningMethodHTML = ListItems.GetDropDownHtml(ddiLearningMethod, null, "ddCivilEduLearningMethod", true, selectedPersonLearningMethod, "", "style='width: 120px;' UnsavedCheckSkipMe='true' class='RequiredInputField' ");

            string subjectNameHTML = "";
            if (civilEducation != null && civilEducation.PersonSchoolSubject != null)
            {
                subjectNameHTML = civilEducation.PersonSchoolSubject.PersonSchoolSubjectName;
            }

            string graduateYearHTML = "";
            if (civilEducation != null)
            {
                graduateYearHTML = civilEducation.GraduateYear.ToString();
            }

            string html = "";
            html += @"
                    <input type='hidden' id='hdnCivilEducationID' name='hdnCivilEducationID' value='" + civilEducationId + @"' />
                    <center>
                        <table style='text-align:center;'>
                        <tr style='height: 15px'></tr>
                        <tr>
                            <td colspan='2' align='center'>
                                <span id='lblCivilEducationBoxTitle' class='SmallHeaderText'>" + (civilEducationId != 0 ? "Редактиране" : "Добавяне") + @" на образование</span>
                            </td>
                        </tr>
                        <tr style='height: 17px'></tr>
                        <tr style='min-height: 17px;'>
                            <td align='right' style='width: 100px;'>
                                <span id='lblPersonEducation' class='InputLabel'>Образователна степен:</span>
                            </td>
                            <td align='left' style='min-width: 220px;'>
                                <span id='spPersonEducations'>" + personEducationsHTML + @"</span>
                            </td>
                        </tr>
                        <tr style=""min-height: 17px"">
                            <td style=""text-align: right;"">
                                <span id=""lblCivilEduPersonSchoolSubject"" class=""InputLabel"">Специалност:</span>
                            </td>
                            <td style=""text-align: left;"">
                                <input type=""hidden"" id=""hdnSchoolSubjectCode"" value=""" + (civilEducation != null ? civilEducation.PersonSchoolSubjectCode : "") + @""" />
                                <input type=""hidden"" id=""hdnSchoolSubjectName"" />
                                <table>
                                    <tr>
                                        <td style=""text-align: bottom;"">
                                            <div id=""txtSubject"" class=""ReadOnlyValue"" style=""background-color:#FFFFCC;width: 300px;min-height:15px;"">" + subjectNameHTML + @"<div>
                                        </td>
                                        <td style=""vertical-align: top;"">
                                            <input id=""btnSelectCivilSubject""
                                                   onclick='civilEducationSelector.showDialog(""civilEducationSelectorForPerson"", CivilEducationSelector_OnSelectedCivilEducation);' 
                                                   type=""button"" value=""Търсене"" class=""OpenCompanySelectorButton"" style=""margin-top:0px"" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style=""min-height: 17px"">
                            <td style=""text-align: right;"">
                                <span id=""lblCivilEduGraduateYear"" class=""InputLabel"">Година на завършване:</span>
                            </td>
                            <td style=""text-align: left;"">
                                <input type=""text"" id=""txtCivilEduGraduateYear"" value=""" + graduateYearHTML + @""" maxlength=""4"" class=""RequiredInputField"" style=""width: 50px;"" />
                            </td>
                        </tr> 
                        <tr style=""min-height: 17px"">
                            <td style=""text-align: right;"">
                                <span id=""lblCivilEduLearningMethod"" class=""InputLabel"">Начин на обучение:</span>
                            </td>
                            <td style=""text-align: left;"">
                                " + learningMethodHTML + @"
                            </td>
                        </tr>        
                        <tr style='height: 30px'>
                            <td colspan='2'> 
                                <span id='spanEducationLightBoxMessage' class='ErrorText' style='display: none;'></span>
                           </td>
                        </tr>
                        <tr>
                            <td colspan='2' style='text-align: center;'>
                                <table style='margin: 0 auto;'>
                                   <tr>
                                      <td>
                                         <div id='btnSaveCadetEducation' style='display: inline;' onclick='SaveCadetEducation();' class='Button'><i></i><div id='btnSaveCadetEducationText' style='width:70px;'>Запис</div><b></b></div>
                                         <div id='btnCloseCadetEducationBox' style='display: inline;' onclick='HideCadetEducationLightBox();' class='Button'><i></i><div id='btnCloseCadetEducationBoxText' style='width:70px;'>Затвори</div><b></b></div>
                                      </td>
                                   </tr>
                                </table>                    
                            </td>
                        </tr>
                   </table>
                    </center>";

            string lightBoxHTML = html;
            string UIItems = GetCivilEducationUIItems();

            string status = AJAXTools.OK;

            string response = @"<lightBoxHTML>" + AJAXTools.EncodeForXML(lightBoxHTML) + @"</lightBoxHTML>";

            if (!String.IsNullOrEmpty(UIItems))
            {
                response += "<UIItems>" + UIItems + "</UIItems>";
            }

            AJAX a = new AJAX(response, status, Response);
            a.Write();
            Response.End();
        }

        //Generate cadet languages table
        private string GenerateCadetLanguagesTable(string message, bool existError)
        {
            bool pageDisabled = false;

            bool IsLanguageCodeHidden = this.GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU_LANG_LANG") == UIAccessLevel.Hidden;
            bool IsLanguageLevelOfKnowledgeKeyHidden = this.GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU_LANG_LVLKNG") == UIAccessLevel.Hidden;
            bool IsLanguageFormOfKnowledgeKeyHidden = this.GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU_LANG_LVLFORM") == UIAccessLevel.Hidden;
            bool IsLanguageStanAgHidden = this.GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU_LANG_STANAG") == UIAccessLevel.Hidden;
            bool IsLanguageDiplomaKeyHidden = this.GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU_LANG_DPLM") == UIAccessLevel.Hidden;
            bool IsLanguageVacAnnHidden = this.GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU_LANG_DOCNUM") == UIAccessLevel.Hidden;
            bool IsLanguageDateWhenHidden = this.GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU_LANG_DOCDATE") == UIAccessLevel.Hidden;

            if (IsLanguageCodeHidden &&
                IsLanguageLevelOfKnowledgeKeyHidden &&
                IsLanguageFormOfKnowledgeKeyHidden &&
                IsLanguageStanAgHidden &&
                IsLanguageDiplomaKeyHidden &&
                IsLanguageVacAnnHidden &&
                IsLanguageDateWhenHidden)
            {
                return "";
            }

            Person person = PersonUtil.GetPerson(this.HdnPersonId, CurrentUser);

            if (Config.GetWebSetting("KOD_KZV_Check_Cadet").ToLower() == "true" &&
                CommonFunctions.IsKeyInList(person.PersonTypeCode, Config.GetWebSetting("KOD_KZV_Restricted_Cadet")) || !CanCurrentUserAccessThisMilDepartment)
            {
                pageDisabled = true;
            }

            //Generate Language table
            string html = "";
            List<PersonLangEduForeignLanguage> listPersonLangEduForeignLanguage = PersonLangEduForeignLanguageUtil.GetAllPersonLangEduForeignLanguageByPersonID(person.PersonId, CurrentUser);

            html += "<table>";
            html += "<tr><td align='left'><span class='SmallHeaderText'>Езикова подготовка</span></td></tr>";
            html += "<tr><td align='center'>";

            //No data found
            if (listPersonLangEduForeignLanguage.Count == 0)
            {
                html += "<span>Няма въведена информация</span>";
            }
            //If there is data then generate dynamically the HTML for the data grid
            else
            {
                //Setup the header of the grid
                html += @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px; vertical-align: bottom;'>№</th>
  " + (!IsLanguageCodeHidden ? @"<th style='width: 100px; vertical-align: bottom;'>Език</th>" : "") + @"
  " + (!IsLanguageLevelOfKnowledgeKeyHidden ? @"<th style='width: 160px; vertical-align: bottom;'>Степен на владеене</th>" : "") + @"                    
  " + (!IsLanguageFormOfKnowledgeKeyHidden ? @"<th style='width: 160px; vertical-align: bottom;'>Форма на владеене</th>" : "") + @"
  " + (!IsLanguageStanAgHidden ? @"<th style='width: 70px; vertical-align: bottom;'>STANAG</br>СГЧП</th>" : "") + @"
  " + (!IsLanguageDiplomaKeyHidden ? @"<th style='width: 130px; vertical-align: bottom;'>Диплом</th>" : "") + @"
  " + (!IsLanguageVacAnnHidden ? @"<th style='width: 80px; vertical-align: bottom;'>Номер на документа</th>" : "") + @"                    
  " + (!IsLanguageDateWhenHidden ? @"<th style='width: 80px; vertical-align: bottom;'>Дата на документа</th>" : "") + @"
                                <th style='width: 60px;'>&nbsp;</th>
                            </tr>
                         </thead>";

                int counter = 1;

                //Iterate through all items and add them into the grid
                foreach (PersonLangEduForeignLanguage personLangEduForeignLanguage in listPersonLangEduForeignLanguage)
                {
                    string deleteHTML = "";
                    string editHTML = "";

                    if (GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU") == UIAccessLevel.Enabled &&
                        GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU_LANG") == UIAccessLevel.Enabled &&
                        GetUIItemAccessLevel("APPL_CADETS_EDITCADET") == UIAccessLevel.Enabled &&
                        GetUIItemAccessLevel("APPL_CADETS") == UIAccessLevel.Enabled &&
                        !pageDisabled)
                    {
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на език' class='GridActionIcon' onclick='DeleteCadetLanguage(" + personLangEduForeignLanguage.PersonLangEduForeignLanguageId.ToString() + ");' />";
                        editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='ShowCadetLanguageLightBox(" + personLangEduForeignLanguage.PersonLangEduForeignLanguageId.ToString() + ");' />";
                    }

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='text-align: center;'>" + counter.ToString() + @"</td>
        " + (!IsLanguageCodeHidden ? @"<td style='text-align: left;'>" + (personLangEduForeignLanguage.PersonLanguage != null ? personLangEduForeignLanguage.PersonLanguage.PersonLanguageName.ToString() : "") + @"</td>" : "") + @"
        " + (!IsLanguageLevelOfKnowledgeKeyHidden ? @"<td style='text-align: left;'>" + (personLangEduForeignLanguage.LanguageLevelOfKnowledge != null ? personLangEduForeignLanguage.LanguageLevelOfKnowledge.LanguageLevelOfKnowledgeName.ToString() : "") + @"</td>" : "") + @"                    
        " + (!IsLanguageFormOfKnowledgeKeyHidden ? @"<td style='text-align: left;'>" + (personLangEduForeignLanguage.LanguageFormOfKnowledge != null ? personLangEduForeignLanguage.LanguageFormOfKnowledge.LanguageFormOfKnowledgeName.ToString() : "") + @"</td>" : "") + @"                    
        " + (!IsLanguageStanAgHidden ? @"<td style='text-align: left;'>" + personLangEduForeignLanguage.LanguageStanAg.ToString() + @"</td>" : "") + @"
        " + (!IsLanguageDiplomaKeyHidden ? @"<td style='text-align: left;'>" + (personLangEduForeignLanguage.LanguageDiplomaKey != null ? personLangEduForeignLanguage.LanguageDiploma.LanguageDiplomaName.ToString() : "") + @"</td>" : "") + @"                    
        " + (!IsLanguageVacAnnHidden ? @"<td style='text-align: left;'>" + personLangEduForeignLanguage.LanguageVacAnn.ToString() + @"</td>" : "") + @"
        " + (!IsLanguageDateWhenHidden ? @"<td style='text-align: left;'>" + (personLangEduForeignLanguage.LanguageDateWhen != null ? CommonFunctions.FormatDate(personLangEduForeignLanguage.LanguageDateWhen) : "") + @"</td>" : "") + @"
                                 <td style='text-align: left;' nowrap>" + editHTML + deleteHTML + @"</td>
                              </tr>";

                    counter++;
                }

                html += "</table><br />";

                string messageClass = "";
                if (existError)
                {
                    messageClass = "ErrorText";
                }
                else
                {
                    messageClass = "SuccessText";
                }

                html += "<span id='spanLanguageMessage' class='" + messageClass + "'>" + message + "</span>";
            }

            html += "</td></tr>";

            if (GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU") == UIAccessLevel.Enabled &&
                GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU_LANG") == UIAccessLevel.Enabled &&
                GetUIItemAccessLevel("APPL_CADETS_EDITCADET") == UIAccessLevel.Enabled &&
                GetUIItemAccessLevel("APPL_CADETS") == UIAccessLevel.Enabled &&
                !pageDisabled)
            {
                html += "<tr><td>";
                html += "<div id='btnAddNewCadetLanguage' style='display: inline;' onclick='ShowCadetLanguageLightBox(0);' class='Button'><i></i><div id='btnAddNewCadetLanguageText' style='width:110px;'>Добавяне на нова</div><b></b></div><br />";
                html += "</td></tr>";
            }

            html += "</table>";

            return html;
        }

        //Save an cadet language record (ajax call)
        private void JSSaveCadetLanguage()
        {
            if (GetUIItemAccessLevel("APPL_CADETS") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("APPL_CADETS_EDITCADET") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU_LANG") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int hdnPersonId = 0;
                int.TryParse(Request.Params["HdnPersonId"], out hdnPersonId);

                int personLangEduForeignLanguageId = 0;
                int.TryParse(Request.Params["ForeignLanguageId"], out personLangEduForeignLanguageId);

                Change change = new Change(CurrentUser, "APPL_Cadets");

                string message = "";

                if (hdnPersonId != 0 && personLangEduForeignLanguageId != 0)
                {
                    message = "Езиковата подготовка е редактирана успешно";
                }
                else if (hdnPersonId != 0 && personLangEduForeignLanguageId == 0)
                {
                    message = "Езиковата подготовка е добавена успешно";
                }
                else
                {
                    throw new Exception();
                }

                string languageCode = Request.Params["LanguageCode"];
                string languageLevelOfKnowledgeKey = Request.Params["LanguageLevelOfKnowledgeKey"];
                string languageFormOfKnowledgeKey = Request.Params["LanguageFormOfKnowledgeKey"];
                string languageStanAg = Request.Params["LanguageStanAg"];
                string languageDiplomaKey = Request.Params["LanguageDiplomaKey"];
                string languageVacAnn = Request.Params["LanguageVacAnn"];

                Person person = PersonUtil.GetPerson(hdnPersonId, CurrentUser);

                PersonLangEduForeignLanguage existingPersonLangEduForeignLanguage = PersonLangEduForeignLanguageUtil.GetPersonLangEduForeignLanguage(person.IdentNumber, languageCode, CurrentUser);

                if (existingPersonLangEduForeignLanguage != null &&
                    existingPersonLangEduForeignLanguage.PersonLangEduForeignLanguageId != personLangEduForeignLanguageId)
                {
                    message = "Избраният език вече е въведен";

                    stat = AJAXTools.OK;
                    response = AJAXTools.EncodeForXML(this.GenerateCadetLanguagesTable(message, true));
                }
                else
                {
                    PersonLangEduForeignLanguage personLangEduForeignLanguage = new PersonLangEduForeignLanguage(CurrentUser);

                    personLangEduForeignLanguage.PersonLangEduForeignLanguageId = personLangEduForeignLanguageId;
                    personLangEduForeignLanguage.LanguageCode = languageCode;
                    personLangEduForeignLanguage.LanguageLevelOfKnowledgeKey = languageLevelOfKnowledgeKey;
                    personLangEduForeignLanguage.LanguageFormOfKnowledgeKey = languageFormOfKnowledgeKey;
                    personLangEduForeignLanguage.LanguageStanAg = languageStanAg;
                    personLangEduForeignLanguage.LanguageDiplomaKey = languageDiplomaKey;
                    personLangEduForeignLanguage.LanguageVacAnn = languageVacAnn;

                    if (!string.IsNullOrEmpty(Request.Params["LanguageDateWhen"]))
                    {
                        personLangEduForeignLanguage.LanguageDateWhen = CommonFunctions.ParseDate(Request.Params["LanguageDateWhen"]);
                    }

                    PersonLangEduForeignLanguageUtil.SavePersonMilitaryEducationAcademy(personLangEduForeignLanguage, person, CurrentUser, change);

                    change.WriteLog();

                    stat = AJAXTools.OK;
                    response = AJAXTools.EncodeForXML(this.GenerateCadetLanguagesTable(message, false));
                }
            }
            catch
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.ERROR;
            }

            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        //Delete an cadet language record (ajax call)
        private void JSDeleteCadetLanguage()
        {
            if (GetUIItemAccessLevel("APPL_CADETS") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("APPL_CADETS_EDITCADET") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU_LANG") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "APPL_Cadets");

                int hdnPersonId = int.Parse(Request.Params["HdnPersonId"]);
                int personLangEduForeignLanguageId = int.Parse(Request.Params["ForeignLanguageId"]);

                Person person = PersonUtil.GetPerson(hdnPersonId, CurrentUser);

                PersonLangEduForeignLanguageUtil.DeletePersonMilitaryTrainingCource(personLangEduForeignLanguageId, person, CurrentUser, change);

                change.WriteLog();

                stat = AJAXTools.OK;
                response = AJAXTools.EncodeForXML(this.GenerateCadetLanguagesTable("Езиковата подготовка е изтрита успешно", false));
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

        //Get the UIItems info for the ForeignLanguage table
        public string GetForeignLanguageUIItems()
        {
            string UIItemsXML = "";

            //Setup the "manually add positions" light-box
            List<string> disabledClientControls = new List<string>();
            List<string> hiddenClientControls = new List<string>();

            UIAccessLevel l;

            l = GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU_LANG_LANG");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonLangEduForeignLanguagePersonLanguage");
                disabledClientControls.Add("ddPersonLangEduForeignLanguagePersonLanguage");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonLangEduForeignLanguagePersonLanguage");
                hiddenClientControls.Add("ddPersonLangEduForeignLanguagePersonLanguage");
            }

            l = GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU_LANG_LVLKNG");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonLangEduForeignLanguageLanguageLevel");
                disabledClientControls.Add("ddPersonLangEduForeignLanguageLanguageLevel");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonLangEduForeignLanguageLanguageLevel");
                hiddenClientControls.Add("ddPersonLangEduForeignLanguageLanguageLevel");
            }

            l = GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU_LANG_LVLFORM");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonLangEduForeignLanguageLanguageForm");
                disabledClientControls.Add("ddPersonLangEduForeignLanguageLanguageForm");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonLangEduForeignLanguageLanguageForm");
                hiddenClientControls.Add("ddPersonLangEduForeignLanguageLanguageForm");
            }

            l = GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU_LANG_DPLM");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonLangEduForeignLanguageLanguageDiploma");
                disabledClientControls.Add("ddPersonLangEduForeignLanguageLanguageDiploma");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonLangEduForeignLanguageLanguageDiploma");
                hiddenClientControls.Add("ddPersonLangEduForeignLanguageLanguageDiploma");
            }

            l = GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU_LANG_DOCNUM");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonLangEduForeignLanguageLanguageVacAnn");
                disabledClientControls.Add("txtPersonLangEduForeignLanguageLanguageVacAnn");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonLangEduForeignLanguageLanguageVacAnn");
                hiddenClientControls.Add("txtPersonLangEduForeignLanguageLanguageVacAnn");
            }

            l = GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU_LANG_DOCDATE");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPersonLangEduForeignLanguageLanguageDateWhen");
                disabledClientControls.Add("txtPersonLangEduForeignLanguageLanguageDateWhen");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPersonLangEduForeignLanguageLanguageDateWhen");
                hiddenClientControls.Add("txtPersonLangEduForeignLanguageLanguageDateWhenCont");
            }

            string disabledClientControlsIds = "";
            string hiddenClientControlsIds = "";

            foreach (string disabledClientControl in disabledClientControls)
            {
                disabledClientControlsIds += (String.IsNullOrEmpty(disabledClientControlsIds) ? "" : ",") +
                    disabledClientControl;
            }

            foreach (string hiddenClientControl in hiddenClientControls)
            {
                hiddenClientControlsIds += (String.IsNullOrEmpty(hiddenClientControlsIds) ? "" : ",") +
                    hiddenClientControl;
            }

            UIItemsXML = "<disabledClientControls>" + AJAXTools.EncodeForXML(disabledClientControlsIds) + "</disabledClientControls>" +
                         "<hiddenClientControls>" + AJAXTools.EncodeForXML(hiddenClientControlsIds) + "</hiddenClientControls>";

            return UIItemsXML;
        }

        //Generate html content for cadet language light box
        private void GenerateCadetLanguageLightBoxContent()
        {
            int personLangEduForeignLanguageId = 0;
            int.TryParse(Request.Params["ForeignLanguageId"], out personLangEduForeignLanguageId);

            PersonLangEduForeignLanguage personLangEduForeignLanguage = PersonLangEduForeignLanguageUtil.GetPersonLangEduForeignLanguage(personLangEduForeignLanguageId, CurrentUser);

            // Generates html for person languages drop down list
            List<PersonLanguage> personLanguages = PersonLanguageUtil.GetAllPersonLanguages(CurrentUser);
            List<IDropDownItem> ddiPersonLanguages = new List<IDropDownItem>();
            foreach (PersonLanguage personLanguage in personLanguages)
            {
                ddiPersonLanguages.Add(personLanguage);
            }

            IDropDownItem selectedPersonLanguage = null;

            if (personLangEduForeignLanguage != null)
            {
                selectedPersonLanguage = (ddiPersonLanguages.Count > 0 ? ddiPersonLanguages.Find(pl => pl.Value() == personLangEduForeignLanguage.PersonLanguage.Value()) : null);
            }

            // Generates html for drop down list
            string personLanguagesHTML = ListItems.GetDropDownHtml(ddiPersonLanguages, null, "ddPersonLangEduForeignLanguagePersonLanguage", true, selectedPersonLanguage, "", "style='width: 150px;' UnsavedCheckSkipMe='true' class='RequiredInputField' ");

            // Generates html for drop down list
            List<LanguageLevelOfKnowledge> listLanguageLevelOfKnowledges = LanguageLevelOfKnowledgeUtil.GetAllLanguageLevelOfKnowledges(CurrentUser);
            List<IDropDownItem> ddiLanguageLevelOfKnowledge = new List<IDropDownItem>();

            foreach (LanguageLevelOfKnowledge languageLevelOfKnowledge in listLanguageLevelOfKnowledges)
            {
                ddiLanguageLevelOfKnowledge.Add(languageLevelOfKnowledge);
            }

            IDropDownItem selectedLanguageLevelOfKnowledge = null;

            if (personLangEduForeignLanguage != null)
            {
                selectedLanguageLevelOfKnowledge = (ddiLanguageLevelOfKnowledge.Count > 0 ? ddiLanguageLevelOfKnowledge.Find(llok => llok.Value() == personLangEduForeignLanguage.LanguageLevelOfKnowledgeKey) : null);
            }

            // Generates html for drop down list
            string languageLevelOfKnowledgeHTML = ListItems.GetDropDownHtml(ddiLanguageLevelOfKnowledge, null, "ddPersonLangEduForeignLanguageLanguageLevel", true, selectedLanguageLevelOfKnowledge, "", "style='width: 150px;' UnsavedCheckSkipMe='true' class='RequiredInputField' ");

            // Generates html for drop down list
            List<LanguageFormOfKnowledge> listLanguageFormOfKnowledges = LanguageFormOfKnowledgeUtil.GetAllLanguageFormOfKnowledges(CurrentUser);
            List<IDropDownItem> ddiLanguageFormOfKnowledge = new List<IDropDownItem>();

            foreach (LanguageFormOfKnowledge LanguageFormOfKnowledge in listLanguageFormOfKnowledges)
            {
                ddiLanguageFormOfKnowledge.Add(LanguageFormOfKnowledge);
            }

            IDropDownItem selectedLanguageFormOfKnowledge = null;

            if (personLangEduForeignLanguage != null)
            {
                selectedLanguageFormOfKnowledge = (ddiLanguageFormOfKnowledge.Count > 0 ? ddiLanguageFormOfKnowledge.Find(lfok => lfok.Value() == personLangEduForeignLanguage.LanguageFormOfKnowledgeKey) : null);
            }

            // Generates html for drop down list
            string languageFormOfKnowledgeHTML = ListItems.GetDropDownHtml(ddiLanguageFormOfKnowledge, null, "ddPersonLangEduForeignLanguageLanguageForm", true, selectedLanguageFormOfKnowledge, "", "style='width: 150px;' UnsavedCheckSkipMe='true' class='RequiredInputField' ");

            // Generates html for drop down list
            List<LanguageDiploma> listLanguageDiplomas = LanguageDiplomaUtil.GetAllLanguageDiplomas(CurrentUser);
            List<IDropDownItem> ddiLanguageDiploma = new List<IDropDownItem>();

            foreach (LanguageDiploma LanguageDiploma in listLanguageDiplomas)
            {
                ddiLanguageDiploma.Add(LanguageDiploma);
            }

            IDropDownItem selectedLanguageDiploma = null;

            if (personLangEduForeignLanguage != null)
            {
                selectedLanguageDiploma = (ddiLanguageDiploma.Count > 0 ? ddiLanguageDiploma.Find(ld => ld.Value() == personLangEduForeignLanguage.LanguageDiplomaKey) : null);
            }

            // Generates html for drop down list
            string languageDiplomaHTML = ListItems.GetDropDownHtml(ddiLanguageDiploma, null, "ddPersonLangEduForeignLanguageLanguageDiploma", true, selectedLanguageDiploma, "", "style='width: 150px;' UnsavedCheckSkipMe='true' class='RequiredInputField'");

            string languageVacAnnHTML = "";
            if (personLangEduForeignLanguage != null)
            {
                languageVacAnnHTML = personLangEduForeignLanguage.LanguageVacAnn;
            }

            string languageDateWhenHTML = "";
            if (personLangEduForeignLanguage != null)
            {
                languageDateWhenHTML = CommonFunctions.FormatDate(personLangEduForeignLanguage.LanguageDateWhen);
            }

            string html = "<input type='hidden' id='hdnForeignLanguageID' name='hdnForeignLanguageID' value='" + personLangEduForeignLanguageId + @"' />";

            if (personLanguages.Count == 0 && personLangEduForeignLanguageId == 0)
            {
                html += @"<center>
                            <div style='padding-top: 50px; padding-bottom: 50px;'>
                                <span class='InputLabel'>Няма намерени езици</span>
                            </div>
                            <table>
                               <tr>
                                  <td>
                                     <div id='btnCloseCadetLanguageBox' style='display: inline;' onclick='HideCadetLanguageLightBox();' class='Button'><i></i><div id='btnCloseCadetLanguageBoxText' style='width:70px;'>Затвори</div><b></b></div>
                                  </td>
                               </tr>
                            </table>
                            </center>";
            }
            else
            {
                html += @"
                    <center>
                        <table style='text-align:center;'>
                        <tr style='height: 15px'></tr>
                        <tr>
                            <td colspan='2' align='center'>
                                <span id='lblForeignLanguageBoxTitle' class='SmallHeaderText'>" + (personLangEduForeignLanguageId != 0 ? "Редактиране" : "Добавяне") + @" на език</span>
                            </td>
                        </tr>
                        <tr style='height: 17px'></tr>
                        <tr style=""min-height: 17px"">
                            <td style=""text-align: right;"">
                                <span id=""lblPersonLangEduForeignLanguagePersonLanguage"" class=""InputLabel"">Език:</span>
                            </td>
                            <td style=""text-align: left;"">
                                " + personLanguagesHTML + @"
                            </td>
                        </tr>
                        <tr style=""min-height: 17px"">
                            <td style=""text-align: right;"">
                                <span id=""lblPersonLangEduForeignLanguageLanguageLevel"" class=""InputLabel"">Степен на владеене:</span>
                            </td>
                            <td style=""text-align: left;"">
                                " + languageLevelOfKnowledgeHTML + @"
                            </td>
                        </tr>
                        <tr style=""min-height: 17px"">
                            <td style=""text-align: right;"">
                                <span id=""lblPersonLangEduForeignLanguageLanguageForm"" class=""InputLabel"">Форма на владеене:</span>
                            </td>
                            <td style=""text-align: left;"">
                                " + languageFormOfKnowledgeHTML + @"
                            </td>
                        </tr>
                        <tr style=""min-height: 17px"">
                            <td style=""text-align: right;"">
                                <span id=""lblPersonLangEduForeignLanguageLanguageDiploma"" class=""InputLabel"">Диплом:</span>
                            </td>
                            <td style=""text-align: left;"">
                                " + languageDiplomaHTML + @"
                            </td>
                        </tr>
                        <tr style=""min-height: 17px"">
                            <td style=""text-align: right;"">
                                <span id=""lblPersonLangEduForeignLanguageLanguageVacAnn"" class=""InputLabel"">Номер на документа:</span>
                            </td>
                            <td style=""text-align: left;"">
                                <input type=""text"" id=""txtPersonLangEduForeignLanguageLanguageVacAnn"" value='" + languageVacAnnHTML + @"' UnsavedCheckSkipMe='true' maxlength=""7"" class=""InputField"" style=""width: 70px;"" />
                            </td>
                        </tr>
                        <tr style=""min-height: 17px"">
                            <td style=""text-align: right;"">
                                <span id=""lblPersonLangEduForeignLanguageLanguageDateWhen"" class=""InputLabel"">Дата на документа:</span>
                            </td>
                            <td style=""text-align: left;"">
                                <span id=""txtPersonLangEduForeignLanguageLanguageDateWhenCont""><input type=""text"" id=""txtPersonLangEduForeignLanguageLanguageDateWhen"" value='" + languageDateWhenHTML + @"' UnsavedCheckSkipMe='true' class='" + CommonFunctions.DatePickerCSS() + @"' maxlength=""10"" style=""width: 70px;"" inLightBox=""true"" /></span>
                            </td>
                        </tr>             
                        <tr style='height: 30px'>
                            <td colspan='2'> 
                                <span id='spanLanguageLightBoxMessage' class='ErrorText' style='display: none;'></span>
                           </td>
                        </tr>
                        <tr>
                            <td colspan='2' style='text-align: center;'>
                                <table style='margin: 0 auto;'>
                                   <tr>
                                      <td>
                                         <div id='btnSaveCadetLanguage' style='display: inline;' onclick='SaveCadetLanguage();' class='Button'><i></i><div id='btnSaveCadetLanguageText' style='width:70px;'>Запис</div><b></b></div>
                                         <div id='btnCloseCadetLanguageBox' style='display: inline;' onclick='HideCadetLanguageLightBox();' class='Button'><i></i><div id='btnCloseCadetLanguageBoxText' style='width:70px;'>Затвори</div><b></b></div>
                                      </td>
                                   </tr>
                                </table>                    
                            </td>
                        </tr>
                   </table>
                    </center>";
            }

            string lightBoxHTML = html;
            string UIItems = GetForeignLanguageUIItems();

            string status = AJAXTools.OK;

            string response = @"<lightBoxHTML>" + AJAXTools.EncodeForXML(lightBoxHTML) + @"</lightBoxHTML>";

            if (!String.IsNullOrEmpty(UIItems))
            {
                response += "<UIItems>" + UIItems + "</UIItems>";
            }

            AJAX a = new AJAX(response, status, Response);
            a.Write();
            Response.End();
        }

        public bool IsSubjectsVisible()
        {
            if (IsRegistred())
            {
                return this.GetUIItemAccessLevel("APPL_CADETS_EDITCADET_SPECS") != UIAccessLevel.Hidden;   
            }
            else
            {
                return this.GetUIItemAccessLevel("APPL_CADETS_ADDCADET_SPECS") != UIAccessLevel.Hidden;
            }
        }

        public bool IsEducationVisible()
        {
            if (IsRegistred())
            {
                return this.GetUIItemAccessLevel("APPL_CADETS_EDITCADET_EDU") != UIAccessLevel.Hidden;
            }
            else
            {
                return this.GetUIItemAccessLevel("APPL_CADETS_ADDCADET_EDU") != UIAccessLevel.Hidden;
            }
        }

        //Chek is Person is already registred as cadet
        private bool IsRegistred()
        {
            int personId = 0;
            if (!String.IsNullOrEmpty(Request.Params["PersonId"])
                && int.TryParse(Request.Params["PersonId"], out personId))
            {
                this.hdnPersonId.Value = personId.ToString();
            }

            int militaryDepartmentId = 0;
            if (!String.IsNullOrEmpty(Request.Params["MilitaryDepartmentId"]))
            {
                militaryDepartmentId = int.Parse(Request.Params["MilitaryDepartmentId"]);
            }

            if (personId == 0 || militaryDepartmentId == 0)
            {
                return false;
            }
            else
            {
                Person person = PersonUtil.GetPerson(personId, CurrentUser);

                if (person == null)
                {
                    //We have a brand new Person and Cadet
                    return false;
                }

                if (CadetUtil.IsAlreadyRegistered(person.PersonId, militaryDepartmentId, CurrentUser))
                {
                    //This person is already registred as cadet for this MilitaryDepartament
                    return true;
                }
                else
                {
                    //This person is not registred yet as cadet for this MilitaryDepartament
                    return false;
                }
            }
        }

        public string GetMedCertTable(int personID)
        {
            StringBuilder tableHTML = new StringBuilder();
            var personMedCerts = PersonMedCertUtil.GetAllPersonMedCerts(personID, CurrentUser);

            if (personMedCerts.Count == 0)
            {
                tableHTML.Append("<span>Няма въведени данни</span>");
            }
            else
            {
                tableHTML.Append(@"<table class='CommonHeaderTable' style='text-align: left;'>
                              <thead>
                                <tr>
                                   <th style='width: 20px; vertical-align: bottom;'>№</th>
                                   <th style='width: 80px; vertical-align: bottom;'>Комисия от дата</th>
                                   <th style='width: 180px; vertical-align: bottom;'>Протокол</th>
                                   <th style='width: 80px; vertical-align: bottom;'>Решение</th>
                                   <th style='width: 240px; vertical-align: bottom;'>Медицинска рубрика</th>
                                   <th style='width: 80px; vertical-align: bottom;'>Дата на валидност</th>
                                </tr>
                              </thead>");

                int counter = 0;

                foreach (var personMedCert in personMedCerts)
                {
                    counter++;

                    tableHTML.Append(@"<tr style='vertical-align: middle; height:20px; " + (counter == 1 ? "font-weight: bold;" : "") + @"' class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                     <td style='text-align: center;'>" + counter.ToString() + @"</td>
                                     <td style='text-align: left;'>" + CommonFunctions.FormatDate(personMedCert.MedCertDate) + @"</td>
                                     <td style='text-align: left;'>" + personMedCert.ProtNum + @"</td>
                                     <td style='text-align: left;'>" + (personMedCert.Conclusion != null ? personMedCert.Conclusion.MilitaryMedicalConclusionName.ToString() : "") + @"</td>
                                     <td style='text-align: left;'>" + (personMedCert.MedRubric != null ? personMedCert.MedRubric.MedicalRubricTitle.ToString() : "") + @"</td>
                                     <td style='text-align: left;'>" + CommonFunctions.FormatDate(personMedCert.ExpirationDate) + @"</td>
                                   </tr>");
                }

                tableHTML.Append("</table>");
            }

            return tableHTML.ToString();
        }

        public string GetPsychCertTable(int personID)
        {
            StringBuilder tableHTML = new StringBuilder();
            var personPsychCerts = PersonPsychCertUtil.GetAllPersonPsychCerts(personID, CurrentUser);

            if (personPsychCerts.Count == 0)
            {
                tableHTML.Append("<span>Няма въведени данни</span>");
            }
            else
            {
                tableHTML.Append(@"<table class='CommonHeaderTable' style='text-align: left;'>
                              <thead>
                                <tr>
                                   <th style='width: 20px; vertical-align: bottom;'>№</th>
                                   <th style='width: 80px; vertical-align: bottom;'>Комисия от дата</th>
                                   <th style='width: 180px; vertical-align: bottom;'>Протокол</th>
                                   <th style='width: 80px; vertical-align: bottom;'>Решение</th>
                                   <th style='width: 80px; vertical-align: bottom;'>Дата на валидност</th>
                                </tr>
                              </thead>");

                int counter = 0;

                foreach (var personPsychCert in personPsychCerts)
                {
                    counter++;

                    tableHTML.Append(@"<tr style='vertical-align: middle; height:20px; " + (counter == 1 ? "font-weight: bold;" : "") + @"' class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                         <td style='text-align: center;'>" + counter.ToString() + @"</td>
                                         <td style='text-align: left;'>" + CommonFunctions.FormatDate(personPsychCert.PsychCertDate) + @"</td>
                                         <td style='text-align: left;'>" + personPsychCert.ProtNum + @"</td>
                                         <td style='text-align: left;'>" + (personPsychCert.Conclusion != null ? personPsychCert.Conclusion.MilitaryMedicalConclusionName.ToString() : "") + @"</td>
                                         <td style='text-align: left;'>" + CommonFunctions.FormatDate(personPsychCert.ExpirationDate) + @"</td>
                                       </tr>");
                }

                tableHTML.Append("</table>");
            }

            return tableHTML.ToString();
        }
    }
}
