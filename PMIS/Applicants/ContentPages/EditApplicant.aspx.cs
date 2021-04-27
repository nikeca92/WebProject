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
    public partial class EditApplicant : APPLPage
    {
        string redirectBack = "";

        private bool canCurrentUserAccessThisMilDepartment;

        public override string PageUIKey
        {
            get
            {
                return "APPL_APPL_EDITAPPL";
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

        //Gets Id for applicant related to loaded person (0 - if new)
        private int HdnApplicantId
        {
            get
            {
                int hdnApplicantId = 0;
                //gets applicantId from page url
                if (Request.Params["HdnApplicantId"] != null)
                    Int32.TryParse(Request.Params["HdnApplicantId"].ToString(), out hdnApplicantId);

                return hdnApplicantId;
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

        private bool CanCurrentUserAccessThisMilDepartment
        {
            get
            {
                string milDepID = "";

                if (HdnApplicantId > 0 || this.hdnApplicantId.Value != "")
                {
                    int applID = HdnApplicantId;

                    if (applID == 0)
                    {
                        applID = int.Parse(this.hdnApplicantId.Value);
                    }

                    Applicant appl = ApplicantUtil.GetApplicant(applID, CurrentUser);
                    milDepID = appl.MilitaryDepartmentId.ToString();
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

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if (GetUIItemAccessLevel("APPL_APPL_EDITAPPL") == UIAccessLevel.Hidden
                || GetUIItemAccessLevel("APPL_APPL") == UIAccessLevel.Hidden)
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
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetApplicantEducationLightBoxContent")
            {
                this.GenerateApplicantEducationLightBoxContent();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveApplicantEducation")
            {
                this.JSSaveApplicantEducation();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteApplicantEducation")
            {
                if ((Request.Params["SelectedTabId"] != null) && (Request.Params["HdnPersonId"] != null)
                    && (Request.Params["CivilEducationId"] != null))
                {
                    this.JSDeleteApplicantEducation();
                    return;
                }
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetApplicantLanguageLightBoxContent")
            {
                this.GenerateApplicantLanguageLightBoxContent();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveApplicantLanguage")
            {
                this.JSSaveApplicantLanguage();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteApplicantLanguage")
            {
                if ((Request.Params["SelectedTabId"] != null) && (Request.Params["HdnPersonId"] != null)
                    && (Request.Params["ForeignLanguageId"] != null))
                {
                    this.JSDeleteApplicantLanguage();
                    return;
                }
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetAddApplicantPositionLightBox")
            {
                JSGetAddApplicantPositionLightBox();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSAddApplicantPosition")
            {
                JSAddApplicantPosition();
                return;
            }

            //Process the ajax request for properties of applicant position(for displaying in the light box)
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetApplicantPosition")
            {
                JSGetApplicantPosition();
                return;
            }


            //Process the ajax request for saving applicant position(for displaying in the light box)
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveApplicantPosition")
            {
                JSSaveApplicantPosition();
                return;
            }

            //Process ajax request for deleting of applicant position
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteApplicantPosition")
            {
                JSDeleteApplicantPosition();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetApplicantDocuments")
            {
                this.JSGetApplicantDocuments();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveApplicantDocuments")
            {
                this.JSSaveApplicantDocuments();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSCheckApplicantPositionDocuemnts")
            {
                this.JSCheckApplicantPositionDocuemnts();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetApplicantDocumentStatusLightBoxContent")
            {
                this.JSGetApplicantDocumentStatusLightBoxContent();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSPrintDocuments")
            {
                JSPrintDocuments();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSPrintLetter")
            {
                JSPrintLetter();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSPrintApplication")
            {
                JSPrintApplication();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRegister")
            {
                JSRegister();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveRegisterData")
            {
                JSSaveRegisterData();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSMoveApplicantPosition")
            {
                JSMoveApplicantPosition();
                return;
            }

            jsMilitaryUnitSelectorDiv.InnerHtml = @"<script src=""" + Page.ClientScript.GetWebResourceUrl(typeof(MilitaryUnitSelector.MilitaryUnitSelector), "MilitaryUnitSelector.MilitaryUnitSelector.js") + @""" type=""text/javascript""></script>";
            jsItemSelectorDiv.InnerHtml = @"<script src=""" + Page.ClientScript.GetWebResourceUrl(typeof(ItemSelector.ItemSelector), "ItemSelector.ItemSelector.js") + @""" type=""text/javascript""></script>";

            //Hilight the correct item in the menu
            HighlightMenuItems("Applicants", "Applicants_Add");

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

            if (!String.IsNullOrEmpty(Request.Params["ApplicantId"]))
            {
                int applicantId = 0;
                Int32.TryParse(Request.Params["ApplicantId"].ToString(), out applicantId);

                Applicant applicant = ApplicantUtil.GetApplicant(applicantId, CurrentUser);
                if (applicant != null)
                {
                    this.hdnApplicantId.Value = applicant.ApplicantId.ToString();
                    this.hdnPersonId.Value = applicant.PersonId.ToString();
                    personId = applicant.PersonId;
                    this.hdnMilitaryDepartmentId.Value = applicant.MilitaryDepartmentId.ToString();

                    militaryDepartmentName = applicant.MilitaryDepartment.MilitaryDepartmentName;
                }
            }
            else
            {
                if (ApplicantUtil.IsAlreadyRegistered(personId, militaryDepartmentId, CurrentUser))
                {
                    Applicant applicant = ApplicantUtil.GetApplicant(personId, militaryDepartmentId, CurrentUser);
                    if (applicant != null)
                    {
                        this.hdnApplicantId.Value = applicant.ApplicantId.ToString();

                        militaryDepartmentName = applicant.MilitaryDepartment.MilitaryDepartmentName;
                    }
                }
            }

            spanMilitaryDepartmentName.InnerText = militaryDepartmentName;

            if (Request.Params["PageFrom"] != null && Request.Params["PageFrom"] == "1")
            {
                //Request is come from ManageApplicants.aspx
                redirectBack = "~/ContentPages/ManageApplicants.aspx";
            }
            else
            {
                if (Request.Params["PageFrom"] != null && Request.Params["PageFrom"] == "2")
                {
                    //Request is come from AddApplicant_SelectPerson.aspx
                    redirectBack = "~/ContentPages/AddApplicant_SelectPerson.aspx";
                }
                else
                {
                    //Set visibilty of editButton False
                    divEdit.Style["display"] = "none";
                    //Request is come from AddApplicant_PersonDetails.aspx
                    redirectBack = "~/ContentPages/AddApplicant_PersonDetails.aspx?IdentNumber=" + hdnIdentNumber.Value + "&MilitaryDepartmentId=" + hdnMilitaryDepartmentId.Value + "&PageFrom=2";
                }
            }

            // if important parameters are omitted in page url (manually, for example), then redirect instead of crash
            Person person = PersonUtil.GetPerson(personId, CurrentUser);
            if (person == null || person.PresCityId == null)
            {
                Response.Redirect("~/ContentPages/AddApplicant_SelectPerson.aspx");
            }

            if (Config.GetWebSetting("KOD_KZV_Check_Applicant").ToLower() == "true" &&
                CommonFunctions.IsKeyInList(person.PersonTypeCode, Config.GetWebSetting("KOD_KZV_Restricted_Applicant")) ||
                !CanCurrentUserAccessThisMilDepartment)
            {
                PageDisabled = true;
            }

            SetupPageUI();

            if (!IsPostBack)
            {
                LoadDropDowns(); //fills dropdowns on the page with values
            }
        }

        // Setup user interface elements according to rights of the user's role
        private void SetupPageUI()
        {
            List<string> disabledClientControls = new List<string>();
            List<string> hiddenClientControls = new List<string>();

            if (PageDisabled)
            {
                pageHiddenControls.Add(btnEdit);
                hiddenClientControls.Add("tblPrintDocuments");
            }

            UIAccessLevel l;

            l = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_POSITIONS_ORDERNUM");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblOrderNumLB");
                disabledClientControls.Add("txtOrderNumLB");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblOrderNumLB");
                hiddenClientControls.Add("txtOrderNumLB");
            }

            l = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_POSITIONS_RESPMILITARYUNIT");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblResponsibleMilitaryUnitLB");
                disabledClientControls.Add("txtResponsibleMilitaryUnitLB");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblResponsibleMilitaryUnitLB");
                hiddenClientControls.Add("txtResponsibleMilitaryUnitLB");
            }

            l = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_POSITIONS_POSITIONNAME");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblPositionNameLB");
                disabledClientControls.Add("txtPositionNameLB");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPositionNameLB");
                hiddenClientControls.Add("txtPositionNameLB");
            }

            l = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_POSITIONS_DOCSTATUS");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblDocStatus");
                pageDisabledControls.Add(ddDocStatus);
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblDocStatus");
                pageHiddenControls.Add(ddDocStatus);
            }

            l = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_PRINT_LETTERS");
            if (l == UIAccessLevel.Hidden || l == UIAccessLevel.Disabled)
            {
                hiddenClientControls.Add("ddLetter");
                hiddenClientControls.Add("btnPrintLetter");
            }

            l = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_PRINT_DOCUMENTS");
            if (l == UIAccessLevel.Hidden || l == UIAccessLevel.Disabled)
            {
                hiddenClientControls.Add("btnPrintDocuments");
            }

            SetDisabledClientControls(disabledClientControls.ToArray());
            SetHiddenClientControls(hiddenClientControls.ToArray());
        }

        // Populate all dropdowns on the screen
        private void LoadDropDowns()
        {

            this.ddDocStatus.DataSource = ApplicantPositionDocumentStatusUtil.GetAllApplicantPositionDocumentStatus(CurrentUser);
            this.ddDocStatus.DataTextField = "StatusName";
            this.ddDocStatus.DataValueField = "StatusId";
            this.ddDocStatus.DataBind();
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

            redirect = "~/ContentPages/AddApplicant_PersonDetails.aspx?IdentNumber=" + person.IdentNumber + "&MilitaryDepartmentId=" + hdnMilitaryDepartmentId.Value + "&PageFrom=2";
            Response.Redirect(redirect);
        }

        //Load Person details (ajax call)
        private void JSLoadPersonDetails()
        {
            //if (GetUIItemAccessLevel("APPL_APPL_EDITAPPL") != UIAccessLevel.Hidden 
            //    && GetUIItemAccessLevel("APPL_APPL") != UIAccessLevel.Hidden)
            //RedirectAjaxAccessDenied();

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

                string hasMilitaryService = "";

                if (person.HasMilitaryService != null)
                {
                    if (person.HasMilitaryService.Value == 1)
                    {
                        hasMilitaryService = "Да";
                    }
                    else
                    {
                        hasMilitaryService = "Не";
                    }
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
                         <wentToMilitary>" + AJAXTools.EncodeForXML(hasMilitaryService) + @"</wentToMilitary>
                         <MilitaryTraining>" + AJAXTools.EncodeForXML(person.MilitaryTrainingString) + @"</MilitaryTraining>
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
                case "btnTabPositions":
                    html = GenerateTabPositionsContent();
                    break;
                case "btnTabEducation":
                    html = this.GenerateTabEducationContent();
                    break;
                case "btnTabDocuments":
                    html = this.GenerateTabDocumentsContent();
                    break;
                case "btnTabHistory":
                    html = GenerateTabHistoryContent();
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

        //Generate the content of education tab
        private string GenerateTabEducationContent()
        {
            if (this.HdnPersonId > 0)
            {
                string html = "";

                if (GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU") != UIAccessLevel.Hidden &&
                    GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU_EDU") != UIAccessLevel.Hidden &&
                    GetUIItemAccessLevel("APPL_APPL_EDITAPPL") != UIAccessLevel.Hidden &&
                    GetUIItemAccessLevel("APPL_APPL") != UIAccessLevel.Hidden)
                {
                    html += "<div id='divApplEduTable'>";
                    html += this.GenerateApplicantEducationsTable(String.Empty, false);
                    html += "</div>";
                    html += "<br />";
                }

                if (GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU") != UIAccessLevel.Hidden &&
                    GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU_LANG") != UIAccessLevel.Hidden &&
                    GetUIItemAccessLevel("APPL_APPL_EDITAPPL") != UIAccessLevel.Hidden &&
                    GetUIItemAccessLevel("APPL_APPL") != UIAccessLevel.Hidden)
                {
                    html += "<div id='divApplLangTable'>";
                    html += this.GenerateApplicantLanguagesTable(String.Empty, false);
                    html += "</div>";
                }

                return html;
            }
            else
            {
                return "";
            }
        }

        //Generate applicant education table
        private string GenerateApplicantEducationsTable(string message, bool existError)
        {
            bool pageDisabled = false;

            bool IsEducationHidden = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU_EDU_EDU") == UIAccessLevel.Hidden;
            bool IsSubjectHidden = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU_EDU_SUBJ") == UIAccessLevel.Hidden;
            bool IsYearHidden = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU_EDU_YEAR") == UIAccessLevel.Hidden;
            bool IsLearningMethodHidden = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU_EDU_LEARNINGMETHOD") == UIAccessLevel.Hidden;

            if (IsEducationHidden &&
                IsSubjectHidden &&
                IsYearHidden &&
                IsLearningMethodHidden
                )
            {
                return "";
            }

            Person person = PersonUtil.GetPerson(this.HdnPersonId, CurrentUser);

            if (Config.GetWebSetting("KOD_KZV_Check_Applicant").ToLower() == "true" &&
                CommonFunctions.IsKeyInList(person.PersonTypeCode, Config.GetWebSetting("KOD_KZV_Restricted_Applicant")) ||
                !CanCurrentUserAccessThisMilDepartment)
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

                    if (GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU") == UIAccessLevel.Enabled &&
                        GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU_EDU") == UIAccessLevel.Enabled &&
                        GetUIItemAccessLevel("APPL_APPL_EDITAPPL") == UIAccessLevel.Enabled &&
                        GetUIItemAccessLevel("APPL_APPL") == UIAccessLevel.Enabled &&
                        !pageDisabled)
                    {
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на образование' class='GridActionIcon' onclick='DeleteApplicantEducation(" + civilEducation.CivilEducationId.ToString() + ");' />";
                        editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='ShowApplicantEducationLightBox(" + civilEducation.CivilEducationId.ToString() + ");' />";
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

            if (GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU") == UIAccessLevel.Enabled &&
                GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU_EDU") == UIAccessLevel.Enabled &&
                GetUIItemAccessLevel("APPL_APPL_EDITAPPL") == UIAccessLevel.Enabled &&
                GetUIItemAccessLevel("APPL_APPL") == UIAccessLevel.Enabled &&
                !pageDisabled)
            {
                html += "<tr><td>";
                html += "<div id='btnAddNewApplicantEducation' style='display: inline;' onclick='ShowApplicantEducationLightBox(0);' class='Button'><i></i><div id='btnAddNewApplicantEducationText' style='width:110px;'>Добавяне на ново</div><b></b></div><br />";
                html += "</td></tr>";
            }

            html += "</table>";

            return html;
        }

        //Save an applicant education record (ajax call)
        private void JSSaveApplicantEducation()
        {
            if (GetUIItemAccessLevel("APPL_APPL") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int hdnPersonId = 0;
                int.TryParse(Request.Params["HdnPersonId"], out hdnPersonId);

                int civilEducationId = 0;
                int.TryParse(Request.Params["CivilEducationId"], out civilEducationId);

                Change change = new Change(CurrentUser, "APPL_Applicants");

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
                    response = AJAXTools.EncodeForXML(this.GenerateApplicantEducationsTable(message, true));
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
                    response = AJAXTools.EncodeForXML(this.GenerateApplicantEducationsTable(message, false));
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

        //Delete an applicant education record (ajax call)
        private void JSDeleteApplicantEducation()
        {
            if (GetUIItemAccessLevel("APPL_APPL") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            string status = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "APPL_Applicants");

                int hdnPersonId = int.Parse(Request.Params["HdnPersonId"]);
                int civilEducationId = int.Parse(Request.Params["CivilEducationId"]);

                Person person = PersonUtil.GetPerson(hdnPersonId, CurrentUser);

                PersonCivilEducationUtil.DeletePersonCivilEducation(civilEducationId, person, CurrentUser, change);

                change.WriteLog();

                status = AJAXTools.OK;
                response = AJAXTools.EncodeForXML(this.GenerateApplicantEducationsTable("Образованието е изтрито успешно", false));
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

            l = GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU_EDU_EDU");
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

            l = GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU_EDU_SUBJ");
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

            l = GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU_EDU_YEAR");
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

            l = GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU_EDU_LEARNINGMETHOD");
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

        //Generate html content for applicant education light box
        private void GenerateApplicantEducationLightBoxContent()
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
                                         <div id='btnSaveApplicantEducation' style='display: inline;' onclick='SaveApplicantEducation();' class='Button'><i></i><div id='btnSaveApplicantEducationText' style='width:70px;'>Запис</div><b></b></div>
                                         <div id='btnCloseApplicantEducationBox' style='display: inline;' onclick='HideApplicantEducationLightBox();' class='Button'><i></i><div id='btnCloseApplicantEducationBoxText' style='width:70px;'>Затвори</div><b></b></div>
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

        //Generate applicant languages table
        private string GenerateApplicantLanguagesTable(string message, bool existError)
        {
            bool pageDisabled = false;

            bool IsLanguageCodeHidden = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU_LANG_LANG") == UIAccessLevel.Hidden;
            bool IsLanguageLevelOfKnowledgeKeyHidden = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU_LANG_LVLKNG") == UIAccessLevel.Hidden;
            bool IsLanguageFormOfKnowledgeKeyHidden = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU_LANG_LVLFORM") == UIAccessLevel.Hidden;
            bool IsLanguageStanAgHidden = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU_LANG_STANAG") == UIAccessLevel.Hidden;
            bool IsLanguageDiplomaKeyHidden = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU_LANG_DPLM") == UIAccessLevel.Hidden;
            bool IsLanguageVacAnnHidden = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU_LANG_DOCNUM") == UIAccessLevel.Hidden;
            bool IsLanguageDateWhenHidden = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU_LANG_DOCDATE") == UIAccessLevel.Hidden;

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

            if (Config.GetWebSetting("KOD_KZV_Check_Applicant").ToLower() == "true" &&
                CommonFunctions.IsKeyInList(person.PersonTypeCode, Config.GetWebSetting("KOD_KZV_Restricted_Applicant")) ||
                !CanCurrentUserAccessThisMilDepartment)
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

                    if (GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU") == UIAccessLevel.Enabled &&
                        GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU_LANG") == UIAccessLevel.Enabled &&
                        GetUIItemAccessLevel("APPL_APPL_EDITAPPL") == UIAccessLevel.Enabled &&
                        GetUIItemAccessLevel("APPL_APPL") == UIAccessLevel.Enabled &&
                        !pageDisabled)
                    {
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на език' class='GridActionIcon' onclick='DeleteApplicantLanguage(" + personLangEduForeignLanguage.PersonLangEduForeignLanguageId.ToString() + ");' />";
                        editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='ShowApplicantLanguageLightBox(" + personLangEduForeignLanguage.PersonLangEduForeignLanguageId.ToString() + ");' />";
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

            if (GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU") == UIAccessLevel.Enabled &&
                GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU_LANG") == UIAccessLevel.Enabled &&
                GetUIItemAccessLevel("APPL_APPL_EDITAPPL") == UIAccessLevel.Enabled &&
                GetUIItemAccessLevel("APPL_APPL") == UIAccessLevel.Enabled &&
                !pageDisabled)
            {
                html += "<tr><td>";
                html += "<div id='btnAddNewApplicantLanguage' style='display: inline;' onclick='ShowApplicantLanguageLightBox(0);' class='Button'><i></i><div id='btnAddNewApplicantLanguageText' style='width:110px;'>Добавяне на нова</div><b></b></div><br />";
                html += "</td></tr>";
            }

            html += "</table>";

            return html;
        }

        //Save an applicant language record (ajax call)
        private void JSSaveApplicantLanguage()
        {
            if (GetUIItemAccessLevel("APPL_APPL") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU_LANG") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int hdnPersonId = 0;
                int.TryParse(Request.Params["HdnPersonId"], out hdnPersonId);

                int personLangEduForeignLanguageId = 0;
                int.TryParse(Request.Params["ForeignLanguageId"], out personLangEduForeignLanguageId);

                Change change = new Change(CurrentUser, "APPL_Applicants");

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
                    response = AJAXTools.EncodeForXML(this.GenerateApplicantLanguagesTable(message, true));
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
                    response = AJAXTools.EncodeForXML(this.GenerateApplicantLanguagesTable(message, false));
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

        //Delete an applicant language record (ajax call)
        private void JSDeleteApplicantLanguage()
        {
            if (GetUIItemAccessLevel("APPL_APPL") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU_LANG") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "APPL_Applicants");

                int hdnPersonId = int.Parse(Request.Params["HdnPersonId"]);
                int personLangEduForeignLanguageId = int.Parse(Request.Params["ForeignLanguageId"]);

                Person person = PersonUtil.GetPerson(hdnPersonId, CurrentUser);

                PersonLangEduForeignLanguageUtil.DeletePersonMilitaryTrainingCource(personLangEduForeignLanguageId, person, CurrentUser, change);

                change.WriteLog();

                stat = AJAXTools.OK;
                response = AJAXTools.EncodeForXML(this.GenerateApplicantLanguagesTable("Езиковата подготовка е изтрита успешно", false));
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

            l = GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU_LANG_LANG");
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


            l = GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU_LANG_LVLKNG");
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

            l = GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU_LANG_LVLFORM");
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

            l = GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU_LANG_DPLM");
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

            l = GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU_LANG_DOCNUM");
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

            l = GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU_LANG_DOCDATE");
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

        //Generate html content for applicant language light box
        private void GenerateApplicantLanguageLightBoxContent()
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
                                     <div id='btnCloseApplicantLanguageBox' style='display: inline;' onclick='HideApplicantLanguageLightBox();' class='Button'><i></i><div id='btnCloseApplicantLanguageBoxText' style='width:70px;'>Затвори</div><b></b></div>
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
                                         <div id='btnSaveApplicantLanguage' style='display: inline;' onclick='SaveApplicantLanguage();' class='Button'><i></i><div id='btnSaveApplicantLanguageText' style='width:70px;'>Запис</div><b></b></div>
                                         <div id='btnCloseApplicantLanguageBox' style='display: inline;' onclick='HideApplicantLanguageLightBox();' class='Button'><i></i><div id='btnCloseApplicantLanguageBoxText' style='width:70px;'>Затвори</div><b></b></div>
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

        private string GenerateTabPositionsContent()
        {
            bool IsOrderNumHidden = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_POSITIONS_ORDERNUM") == UIAccessLevel.Hidden;
            bool IsSubmitDocsDepartmentHidden = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_POSITIONS_SUBMITDOCSDEPARTMENT") == UIAccessLevel.Hidden;
            bool IsMilitaryUnitHidden = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_POSITIONS_MILITARYUNIT") == UIAccessLevel.Hidden;
            bool IsPositionNameHidden = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_POSITIONS_POSITIONNAME") == UIAccessLevel.Hidden;
            bool IsRespMilitaryUnitHidden = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_POSITIONS_RESPMILITARYUNIT") == UIAccessLevel.Hidden;
            bool IsDocStatusHidden = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_POSITIONS_DOCSTATUS") == UIAccessLevel.Hidden;
            bool IsStatusHidden = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_POSITIONS_STATUS") == UIAccessLevel.Hidden;
            bool IsRegisterApplicationHiddent = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_POSITIONS_REGISTER_APPLICATION") == UIAccessLevel.Hidden;
            bool IsRegisterApplicationDisabled = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_POSITIONS_REGISTER_APPLICATION") == UIAccessLevel.Disabled;

            int personId = 0;

            if (!string.IsNullOrEmpty(Request.Params["HdnPersonId"]))
                personId = int.Parse(Request.Params["HdnPersonId"]);

            int applicantId = 0;

            if (!string.IsNullOrEmpty(Request.Params["ApplicantId"]))
                applicantId = int.Parse(Request.Params["ApplicantId"]);
            else if (!string.IsNullOrEmpty(Request.Params["HdnApplicantId"]))
                applicantId = int.Parse(Request.Params["HdnApplicantId"]);

            bool pageDisabled = false;

            Person person = PersonUtil.GetPerson(personId, CurrentUser);

            if (Config.GetWebSetting("KOD_KZV_Check_Applicant").ToLower() == "true" &&
                CommonFunctions.IsKeyInList(person.PersonTypeCode, Config.GetWebSetting("KOD_KZV_Restricted_Applicant")) ||
                !CanCurrentUserAccessThisMilDepartment)
            {
                pageDisabled = true;
            }

            bool positionsTableEditPermission = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_POSITIONS") == UIAccessLevel.Enabled &&
                                                this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL") == UIAccessLevel.Enabled &&
                                                this.GetUIItemAccessLevel("APPL_APPL") == UIAccessLevel.Enabled &&
                                                !pageDisabled;


            List<ApplicantPosition> positionsAll = ApplicantPositionUtil.GetAllApplicantPositionByPersonID(personId, false, CurrentUser);
            var groupedPositions = positionsAll
                .GroupBy(x => new
                {
                    x.VacancyAnnouncePosition.VacancyAnnounceID,
                    x.VacancyAnnouncePosition.ResponsibleMilitaryUnitID,
                    DisplayTextForSelection = (x.VacancyAnnouncePosition.ResponsibleMilitaryUnit != null ? x.VacancyAnnouncePosition.ResponsibleMilitaryUnit.DisplayTextForSelection : "")
                })
                .Select(x => new
                {
                    VacancyAnnounceID = x.Key.VacancyAnnounceID,
                    ResponsibleMilitaryUnitID = x.Key.ResponsibleMilitaryUnitID,
                    ResponsibleMilitaryUnitName = x.Key.DisplayTextForSelection,
                    Positions = x.OrderBy(y => y.Seq).ToList()
                })
                .OrderBy(x => x.VacancyAnnounceID)
                .ThenBy(x => x.ResponsibleMilitaryUnitID)
                .ToList();

            StringBuilder sb = new StringBuilder();
            int tableCount = 0;

            sb.Append("<table style='width: 100%;'>");

            foreach (var milUnitWithPositions in groupedPositions)
            {
                tableCount++;

                List<ApplicantPosition> positions = milUnitWithPositions.Positions.OrderByDescending(x => x.ApplicantId == applicantId).ThenBy(x => x.Seq).ToList();
                VacancyAnnounce vacancyAnnounce = VacancyAnnounceUtil.GetVacancyAnnounce(milUnitWithPositions.VacancyAnnounceID, CurrentUser);
                Register register = null;

                if (milUnitWithPositions.ResponsibleMilitaryUnitID.HasValue)
                    register = RegisterUtil.GetRegister(applicantId, milUnitWithPositions.VacancyAnnounceID, milUnitWithPositions.ResponsibleMilitaryUnitID.Value, CurrentUser);

                sb.Append("<tr><td>");

                sb.Append(@"<table id=""tblPositions-" + tableCount + @""" style=""width: 98%; border-collapse: collapse; background: #E8E8E8; border: solid 1px #CCCCCC;"">
                                    <tr>
                                        <td>");

                sb.Append(@"<table>
                                    <tr>
                                        <td>");

                if (!IsOrderNumHidden)
                    sb.Append(@"<span class=""InputLabel"" style= ""margin-left: 6px;"">Заповед №: </span>
                                            <span class=""ReadOnlyValue"">" + vacancyAnnounce.OrderNum + @"</span>");

                if (!IsRespMilitaryUnitHidden)
                    sb.Append(@"<span class=""InputLabel"" style=""margin-left: 15px;"">ВПН/Структура отговорна за конкурса: </span>
                                            <span class=""ReadOnlyValue"">" + milUnitWithPositions.ResponsibleMilitaryUnitName + @"</span>");

                sb.Append(@"</td></tr>");

                if (!pageDisabled)
                {
                    if (!IsRegisterApplicationHiddent)
                    {
                        string printButtonLabel = "Регистриране и печат";
                        int buttonWidth = 130;

                        if (register != null || !milUnitWithPositions.ResponsibleMilitaryUnitID.HasValue)
                        {
                            printButtonLabel = "Печат";
                            buttonWidth = 45;
                        }

                        sb.Append(@"<tr>
		                            <td>
                                        <table id=""tblRegister-" + tableCount + @""" style=""margin-top: 5px;"">
                                            <tr>");

                        if (!IsRegisterApplicationDisabled)
                            sb.Append(@"<td style=""vertical-align: top;"">
		                                            <select id=""ddApplication-" + tableCount + @""" onchange=""ChangeStyle('btnPrintApplication-" + tableCount + @"');"">
		                                                <option value=""-1"" selected disabled>Моля изберете</option>		                            
                                                        <option value=""" + GeneratePrintApplicantUtil.ApplicationValue.VS + @""">Заявление ВС</option>
                                                        <option value=""" + GeneratePrintApplicantUtil.ApplicationValue.DR4B + @""">Заявление ДР чл.4б</option>
                                                        <option value=""" + GeneratePrintApplicantUtil.ApplicationValue.DR6 + @""">Заявление ДР чл.6</option>
                                                        <option value=""" + GeneratePrintApplicantUtil.ApplicationValue.NVP + @""">Заявление НВП</option>
                                                        <option value=""" + GeneratePrintApplicantUtil.ApplicationValue.SVP + @""">Заявление СВП</option>
                                                    </select>
		                                        </td>
                                                 <td style=""text-align: center; vertical-align: top;""> 
                                                    <div id=""btnPrintApplication-" + tableCount + @""" class=""DisabledButton"" data-reg=""" + (register != null ? "1" : "0") + @""" style=""margin-top: 0px;"" onclick=""PrintApplication(" + tableCount + @");""><i></i><div style=""width:" + buttonWidth + @"px; padding-left:5px; height: 24px;"">" + printButtonLabel + @"</div><b></b></div>                                                               
                                                 </td>");

                        sb.Append(@"<td>
                                        <table id=""tblReg-" + tableCount + @""" style=""display: " + (register != null ? "" : "none") + @""">
                                            <tr>
                                                <td style=""vertical-align: top;"">
                                                    <span class=""InputLabel"" style=""margin-left: " + (!IsRegisterApplicationDisabled ? "10" : "0") + @"px;"">Рег. №: </span>
                                                    <span id=""spnRegNum-" + tableCount + @""" class=""ReadOnlyValue"">" + (register != null ? register.RegisterNumber.ToString() : "") + @"</span>
                                                </td>
                                                <td  style=""vertical-align: top;"">
                                                    <span class=""InputLabel"" style=""margin-left: 10px;"">Дата: </span>
                                                    <span id=""spanRegDate-" + tableCount + @""">
                                                        <input type=""text"" id=""txtRegDate-" + tableCount + @""" value=""" + (register != null ? (register.DocumentDate.HasValue ? register.DocumentDate.Value.ToString("dd.MM.yyyy") : "") : "") + @""" class='" + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" " + (IsRegisterApplicationDisabled ? "disabled" : "") + @" />
                                                    </span>
                                                </td>
                                                
                                                <td rowspan=""2"" style=""vertical-align: top;"">
                                                    <span class=""InputLabel"" style=""margin-left: 10px; vertical-align: top;"">Бележки: </span>
                                                    <textarea id=""txtaRegNotes-" + tableCount + @""" rows=""3"" class=""InputField"" style=""width: 230px;"" " + (IsRegisterApplicationDisabled ? "disabled" : "") + @">" + (register != null ? register.Notes : "") + @"</textarea></td>
                                                    "
                                                    + (IsRegisterApplicationDisabled ? "" : @"<td style=""vertical-align: top;"" rowspan=""2"">
                                                    <div id=""btnSave-" + tableCount + @""" class=""Button"" style=""margin-top: 0px; margin-left: 10px;"" onclick=""SaveRegisterData(" + tableCount + @")""><i></i><div style=""width: 45px; padding-left:5px; height: 24px;"">Запис</div><b></b></div><br/>
                                                    <span id=""lblMessage-" + tableCount + @""" class=""SuccessText""></span>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td  style=""vertical-align: top;"">
                                                    <span class=""InputLabel"" style=""margin-left: 10px;"">Брой листа: </span>
                                                    <input type=""text"" id=""txtRegPageCount-" + tableCount + @""" value=""" + (register != null ? register.PageCount : "") + @""" class=""InputField"" style=""width: 35px;"" " + (IsRegisterApplicationDisabled ? "disabled" : "") + @">
                                                </td>
                                            </tr>
                                        </table>
                                    </td>"));

                        sb.Append(@"</tr>");



                        if (register != null)
                            sb.Append(@"<tr>
                                            <td colspan=""6"">
                                                <div class=""ErrorText"" id=""lblErrorMsg-" + tableCount + @""" style=""margin-left: 10px;""></div>
                                            </td>
                                        </tr>");

                        sb.Append(@"</table>
                                    </td>

                                  </tr>
                                  <tr>
                                     <td><input type=""hidden"" id=""hdnVacancyAnnounceID-" + tableCount + @""" value=""" + milUnitWithPositions.VacancyAnnounceID.ToString() + @""">
                                         <input type=""hidden"" id=""hdnResponsibleMilitaryUnitID-" + tableCount + @""" value=""" + milUnitWithPositions.ResponsibleMilitaryUnitID.ToString() + @""">
                                     </td>
                                  </tr>");
                    }
                }

                sb.Append(@"</table></td></tr>");

                int idx = tableCount;

                sb.Append(@"<tr><td id='tdApplicantPosisionsTable-" + idx + @"'>");

                string applicantPositionsTable = GenerateApplicantPositionsTable(applicantId, positions, idx);
                
                sb.Append(applicantPositionsTable);
                sb.Append(@"</td></tr>");
                sb.Append(@"<tr><td style=""height: 10px;""></td></tr>");
                sb.Append(@"</table>");

                sb.Append("</td></tr>");
                sb.Append(@"<tr><td style=""height: 20px;""></td></tr>");
            }

            if (positionsTableEditPermission)
            {
                sb.Append("<tr><td><table><tr><td></td></tr>");
                sb.Append("<tr><td align='left'>");
                sb.Append("<div id='btnAddApplicantPosition' class='Button' onclick='ShowAddApplicantPositionLightBox()'><i></i><div style='width:145px; padding-left:5px;'>Добавяне на длъжност</div><b></b></div>");
                sb.Append("</td></tr></table></td></tr>");
            }

            sb.Append("</table>");

            return sb.ToString();
        }

        private string GenerateTabHistoryContent()
        {
            bool IsOrderNumHidden = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_POSITIONS_ORDERNUM") == UIAccessLevel.Hidden;
            bool IsMilitaryUnitHidden = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_POSITIONS_MILITARYUNIT") == UIAccessLevel.Hidden;
            bool IsPositionNameHidden = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_POSITIONS_POSITIONNAME") == UIAccessLevel.Hidden;
            bool IsRespMilitaryUnitHidden = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_POSITIONS_RESPMILITARYUNIT") == UIAccessLevel.Hidden;
            bool IsStatusHidden = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_POSITIONS_STATUS") == UIAccessLevel.Hidden;

            int personId = 0;

            if (!string.IsNullOrEmpty(Request.Params["HdnPersonId"]))
                personId = int.Parse(Request.Params["HdnPersonId"]);

            List<ApplicantPosition> positions = ApplicantPositionUtil.GetAllApplicantPositionByPersonID(personId, true, CurrentUser);

            StringBuilder sb = new StringBuilder();
            sb.Append("<table id='positionsHistoryTable' name='positionsHistoryTable' class='CommonHeaderTable' style='text-align: left;' border='1px'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style=\"min-width: 30px;\"></th>");
            if (!IsOrderNumHidden)
                sb.Append("<th style=\"min-width: 80px;\">Заповед №</th>");
            if (!IsMilitaryUnitHidden)
                sb.Append("<th style=\"min-width: 130px;\">" + CommonFunctions.GetLabelText("MilitaryUnit") + "</th>");
            if (!IsPositionNameHidden)
                sb.Append("<th style=\"min-width: 80px;\">Длъжност</th>");
            if (!IsRespMilitaryUnitHidden)
                sb.Append("<th style=\"min-width: 130px;\">" + CommonFunctions.GetLabelText("MilitaryUnit") + " отговорна за конкурса</th>");
            if (!IsStatusHidden)
                sb.Append("<th style=\"width: 160px;\">Статус</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            int counter = 1;

            if (positions.Count > 0)
            {
                sb.Append("<tbody>");
            }

            foreach (ApplicantPosition position in positions)
            {
                VacancyAnnounce vacancyAnnounce = VacancyAnnounceUtil.GetVacancyAnnounce(position.VacancyAnnouncePosition.VacancyAnnounceID, CurrentUser);

                sb.Append("<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + "'>");
                sb.Append("<td>" + counter + "</td>");
                if (!IsOrderNumHidden)
                    sb.Append("<td>" + vacancyAnnounce.OrderNum + "</td>");
                if (!IsMilitaryUnitHidden)
                    sb.Append("<td>" + (position.VacancyAnnouncePosition.MilitaryUnit != null ? position.VacancyAnnouncePosition.MilitaryUnit.DisplayTextForSelection : "") + "</td>");
                if (!IsPositionNameHidden)
                    sb.Append("<td>" + position.VacancyAnnouncePosition.PositionName + "</td>");
                if (!IsRespMilitaryUnitHidden)
                    sb.Append("<td>" + (position.VacancyAnnouncePosition.ResponsibleMilitaryUnit != null ? position.VacancyAnnouncePosition.ResponsibleMilitaryUnit.DisplayTextForSelection : "") + "</td>");
                if (!IsStatusHidden)
                    sb.Append("<td>" + (position.ApplicantStatus != null ? position.ApplicantStatus.StatusName : "") + "</td>");

                sb.Append("</tr>");
                counter++;
            }

            if (positions.Count > 0)
            {
                sb.Append("</tbody>");
            }

            sb.Append("</table>");
            sb.Append("<div style='height: 10px;'></div>");

            return sb.ToString();
        }

        // Return contents for add vacancy position light box by ajax request
        private void JSGetAddApplicantPositionLightBox()
        {
            string response = "";
            response += GetAddApplicantPositionLightBoxHtml();

            AJAX a = new AJAX(AJAXTools.EncodeForXML(response), this.Response);
            a.Write();
            Response.End();
        }

        // Generate contents for add vacancy position light box
        private string GetAddApplicantPositionLightBoxHtml()
        {
            bool IsOrderNumHidden = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_POSITIONS_ORDERNUM") == UIAccessLevel.Hidden;
            bool IsRespMilitaryUnitHidden = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_POSITIONS_RESPMILITARYUNIT") == UIAccessLevel.Hidden;
            bool IsMilitaryUnitHidden = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_POSITIONS_MILITARYUNIT") == UIAccessLevel.Hidden;
            bool IsPositionNameHidden = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_POSITIONS_POSITIONNAME") == UIAccessLevel.Hidden;

            string html = "";

            string htmlNoResults = "";

            MilitaryUnitSelector.MilitaryUnitSelector respMilUnitSelector = new MilitaryUnitSelector.MilitaryUnitSelector();
            respMilUnitSelector.Page = this;
            respMilUnitSelector.DataSourceWebPage = "DataSource.aspx";
            respMilUnitSelector.DataSourceKey = "MilitaryUnit";
            respMilUnitSelector.ResultMaxCount = 1000;
            respMilUnitSelector.Style.Add("width", "86%");
            respMilUnitSelector.DivMainCss = "isDivMainClass";
            respMilUnitSelector.DivListCss = "isDivListClass";
            respMilUnitSelector.DivFullListCss = "isDivFullListClass";
            respMilUnitSelector.DivFullListTitle = CommonFunctions.GetLabelText("MilitaryUnit") + " отговорна за конкурса";
            respMilUnitSelector.ID = "RespMilUnitSelector";

            MilitaryUnitSelector.MilitaryUnitSelector milUnitSelector = new MilitaryUnitSelector.MilitaryUnitSelector();
            milUnitSelector.Page = this;
            milUnitSelector.DataSourceWebPage = "DataSource.aspx";
            milUnitSelector.DataSourceKey = "MilitaryUnit";
            milUnitSelector.ResultMaxCount = 1000;
            milUnitSelector.Style.Add("width", "86%");
            milUnitSelector.DivMainCss = "isDivMainClass";
            milUnitSelector.DivListCss = "isDivListClass";
            milUnitSelector.DivFullListCss = "isDivFullListClass";
            milUnitSelector.DivFullListTitle = CommonFunctions.GetLabelText("MilitaryUnit");
            milUnitSelector.ID = "MilUnitSelector";

            List<VacancyAnnouncePosition> availablePositions = new List<VacancyAnnouncePosition>();
            string respMilitaryUnitIds = "";
            string militaryUnitIds = "";
            int applicantID = 0;
            string orderNum = "";
            string positionName = "";
            int pageIndex = 1; //Default
            int pageLength = int.Parse(Config.GetWebSetting("RowsPerPage"));
            int allRows = 0;
            int maxPage = 1;
            int orderBy = 1; //Default

            applicantID = (!string.IsNullOrEmpty(Request.Params["ApplicantID"]) ? int.Parse(Request.Params["ApplicantID"]) : 0);
            if (Request.Params["RespMilitaryUnitID"] != null && Request.Params["MilitaryUnitID"] != null && Request.Params["PositionName"] != null && Request.Params["PageIndex"] != null && Request.Params["OrderBy"] != null && Request.Params["MaxPage"] != null)
            {
                if (Request.Params["RespMilitaryUnitID"] != ListItems.GetOptionAll().Value)
                    respMilitaryUnitIds = Request.Params["RespMilitaryUnitID"];

                if (!string.IsNullOrEmpty(respMilitaryUnitIds))
                {
                    MilitaryUnit unit = MilitaryUnitUtil.GetMilitaryUnit(int.Parse(respMilitaryUnitIds), CurrentUser);
                    if (unit != null)
                    {
                        respMilUnitSelector.SelectedValue = respMilitaryUnitIds;
                        respMilUnitSelector.SelectedText = unit.DisplayTextForSelection;
                    }
                }

                if (Request.Params["MilitaryUnitID"] != ListItems.GetOptionAll().Value)
                    militaryUnitIds = Request.Params["MilitaryUnitID"];

                if (!string.IsNullOrEmpty(militaryUnitIds))
                {
                    MilitaryUnit unit = MilitaryUnitUtil.GetMilitaryUnit(int.Parse(militaryUnitIds), CurrentUser);
                    if (unit != null)
                    {
                        milUnitSelector.SelectedValue = militaryUnitIds;
                        milUnitSelector.SelectedText = unit.DisplayTextForSelection;
                    }
                }

                positionName = (Request.Params["PositionName"] != null ? Request.Params["PositionName"] : "");
                orderNum = (Request.Params["OrderNum"] != null ? Request.Params["OrderNum"] : "");

                pageIndex = int.Parse(Request.Params["PageIndex"]);
                orderBy = int.Parse(Request.Params["OrderBy"]);
            }

            allRows = VacancyAnnouncePositionUtil.GetAllVacancyAnnouncePositionsForApplicantCount(applicantID, respMilitaryUnitIds, militaryUnitIds, positionName, orderNum, CurrentUser);
            maxPage = allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            availablePositions = VacancyAnnouncePositionUtil.GetAllVacancyAnnouncePositionsForApplicant(applicantID, respMilitaryUnitIds, militaryUnitIds, positionName, orderNum, orderBy, pageIndex, pageLength, CurrentUser);

            StringWriter sw = new StringWriter();
            HtmlTextWriter tw = new HtmlTextWriter(sw);

            StringWriter sw2 = new StringWriter();
            HtmlTextWriter tw2 = new HtmlTextWriter(sw2);

            respMilUnitSelector.RenderControl(tw);
            milUnitSelector.RenderControl(tw2);

            // No data found
            if (availablePositions.Count == 0)
            {
                htmlNoResults = "Няма въведена информация";
            }

            //Set filter section
            html += @"<center>
                        <table width='100%' style='border-collapse: collapse; vertical-align: middle; color: #0B449D;'>
                        <tr style='height: 30px'>
                            <td align='right'>
                                <span class='InputLabel' style='padding-left: 10px'>Заповед №:</span>                            
                             </td>    
                            <td align='left'>
                                <input type='text' id='txtOrderNum' class='InputField' value='" + orderNum + @"'></input>
                            </td>    
                            <td align='right'>
                                <span class='InputLabel' style='padding-left: 10px'>" + CommonFunctions.GetLabelText("MilitaryUnit") + " отговорна за конкурса" + @":</span>                                                                                                
                            </td>
                            <td align='left'>
                                " + sw.ToString() + @"
                            </td>                   
                            <td align='right'>
                                <span class='InputLabel' style='padding-left: 10px'>" + CommonFunctions.GetLabelText("MilitaryUnit") + @":</span>                                                                                                
                            </td>
                            <td align='left'>
                                " + sw2.ToString() + @"
                            </td>
                            <td align='right'>
                                <span class='InputLabel' style='padding-left: 10px'>Длъжност:</span>
                            </td>
                            <td align='left'>
                                <input type='text' id='txtPositionName' class='InputField' value='" + positionName + @"'></input>
                            </td>
                        </tr>                        
                        <tr style='height: 40px'>
                            <td colspan='8' align='center'>
                                <div id='btnSearch' class='Button' onclick='FilterAddApplicantPositionLightBox()'><i></i><div style='width:70px; padding-left:5px;'>Покажи</div><b></b></div></td>
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
            html += @"<div style='min-height: 150px; margin-bottom: 10px;'>

                        <input type='hidden' id='hdnAddApplicantPositionOrderBy' value='" + orderBy + @"' />
                        <input type='hidden' id='hdnAddApplicantPositionPageIndex' value='" + pageIndex + @"' />
                        <input type='hidden' id='hdnPageMaxPage' value='" + maxPage + @"' />

                        <span class='HeaderText'>Избор на желани длъжности</span><br /><br /><br />

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

                        ";

            //Set Table Results
            string headerStyle = "vertical-align: bottom;";
            int orderCol = (orderBy > 100 ? orderBy - 100 : orderBy);
            string img = orderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
            string[] arrOrderCol = { "", "", "", "", "", "", "" };

            arrOrderCol[orderCol - 1] = @"<div style='position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

            //Setup the header of the grid
            html += @"<div id='tblVacantPositionsScrollWrapper' style='overflow-y: auto;'>
                      <table class='CommonHeaderTable' style='margin: 0 auto; text-align: left; width: 99%;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>" +
    (!IsOrderNumHidden ? @"<th style='width: 110px; cursor: pointer;" + headerStyle + @"' onclick='SortAddApplicantPositionTableBy(1);'>Заповед №" + arrOrderCol[0] + @"</th>" : "") +
    (!IsRespMilitaryUnitHidden ? @"<th style='width: 130px; cursor: pointer;" + headerStyle + @"' onclick='SortAddApplicantPositionTableBy(2);'>" + CommonFunctions.GetLabelText("MilitaryUnit") + " отговорна за конкурса" + arrOrderCol[1] + @"</th>" : "") +
    (!IsMilitaryUnitHidden ? @"<th style='width: 130px; cursor: pointer;" + headerStyle + @"' onclick='SortAddApplicantPositionTableBy(3);'>" + CommonFunctions.GetLabelText("MilitaryUnit") + arrOrderCol[2] + @"</th>" : "") +
    (!IsPositionNameHidden ? @"<th style='width: auto; cursor: pointer;" + headerStyle + @"' onclick='SortAddApplicantPositionTableBy(4);'>Длъжност" + arrOrderCol[3] + @"</th>" : "") +
    (!IsPositionNameHidden ? @"<th style='width: 120px; cursor: pointer;" + headerStyle + @"' onclick='SortAddApplicantPositionTableBy(5);'>Задължителни изисквания" + arrOrderCol[4] + @"</th>" : "") +
    (!IsPositionNameHidden ? @"<th style='width: 120px; cursor: pointer;" + headerStyle + @"' onclick='SortAddApplicantPositionTableBy(6);'>Допълнителни изисквания" + arrOrderCol[5] + @"</th>" : "") +
    (!IsPositionNameHidden ? @"<th style='width: 120px; cursor: pointer;" + headerStyle + @"' onclick='SortAddApplicantPositionTableBy(7);'>Специфични изисквания" + arrOrderCol[6] + @"</th>" : "") +
                        @" </tr></thead>";

            int counter = 1;

            //Iterate through all items and add them into the grid
            foreach (VacancyAnnouncePosition vacantPosition in availablePositions)
            {

                string cellStyle = "vertical-align: top;";

                string mandatoryRequirementsCursor = vacantPosition.MandatoryRequirements.Length > 30 ? " cursor: pointer;" : " cursor: default;";
                string additionalRequirementsCursor = vacantPosition.AdditionalRequirements.Length > 30 ? " cursor: pointer;" : " cursor: default;";
                string specificRequirementsCursor = vacantPosition.SpecificRequirements.Length > 30 ? " cursor: pointer;" : " cursor: default;";

                string mandatoryRequirementsFull = CommonFunctions.ReplaceNewLinesInString(vacantPosition.MandatoryRequirements.Replace("'", "\'").Replace(@"""", "&quot;"));
                string additionalRequirementsFull = CommonFunctions.ReplaceNewLinesInString(vacantPosition.AdditionalRequirements.Replace("'", "\'").Replace(@"""", "&quot;"));
                string specificRequirementsFull = CommonFunctions.ReplaceNewLinesInString(vacantPosition.SpecificRequirements.Replace("'", "\'").Replace(@"""", "&quot;"));

                string mandatoryRequirementsOnClick = vacantPosition.MandatoryRequirements.Length > 30 ? "ShowInfoPopup('Задължителни изисквания', '" + mandatoryRequirementsFull + "', true, false, event);" : "";
                string additionalRequirementsOnClick = vacantPosition.AdditionalRequirements.Length > 30 ? "ShowInfoPopup('Допълнителни изисквания', '" + additionalRequirementsFull + "', true, false, event);" : "";
                string specificRequirementsOnClick = vacantPosition.SpecificRequirements.Length > 30 ? "ShowInfoPopup('Специфични изисквания', '" + specificRequirementsFull + "', true, false, event);" : "";

                string mandatoryRequirements = vacantPosition.MandatoryRequirements.Length > 30 ? vacantPosition.MandatoryRequirements.Substring(0, 30) + "..." : vacantPosition.MandatoryRequirements;
                string additionalRequirements = vacantPosition.AdditionalRequirements.Length > 30 ? vacantPosition.AdditionalRequirements.Substring(0, 30) + "..." : vacantPosition.AdditionalRequirements;
                string specificRequirements = vacantPosition.SpecificRequirements.Length > 30 ? vacantPosition.SpecificRequirements.Substring(0, 30) + "..." : vacantPosition.SpecificRequirements;

                html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"' style='cursor: pointer;' title='Избор' >
                                 <td style='" + cellStyle + @"' onclick='SelectPosition(" + vacantPosition.VacancyAnnouncePositionID + @");'>" + ((pageIndex - 1) * pageLength + counter).ToString() + @"</td>" +
      (!IsOrderNumHidden ? @"<td style='" + cellStyle + @"' onclick='SelectPosition(" + vacantPosition.VacancyAnnouncePositionID + @");'><span id='positionCode" + counter + @"'>" + VacancyAnnounceUtil.GetVacancyAnnounce(vacantPosition.VacancyAnnounceID, CurrentUser).OrderNum + @"</span></td>" : "") +
      (!IsRespMilitaryUnitHidden ? @"<td style='" + cellStyle + @"' onclick='SelectPosition(" + vacantPosition.VacancyAnnouncePositionID + @");'>" + (vacantPosition.ResponsibleMilitaryUnit != null ? vacantPosition.ResponsibleMilitaryUnit.DisplayTextForSelection : "") + @"</td>" : "") +
      (!IsMilitaryUnitHidden ? @"<td style='" + cellStyle + @"' onclick='SelectPosition(" + vacantPosition.VacancyAnnouncePositionID + @");'>" + (vacantPosition.MilitaryUnit != null ? vacantPosition.MilitaryUnit.DisplayTextForSelection : "") + @"</td>" : "") +
      (!IsPositionNameHidden ? @"<td style='" + cellStyle + @"' onclick='SelectPosition(" + vacantPosition.VacancyAnnouncePositionID + @");'>" + vacantPosition.PositionName + @"</td>" : "") +
      (!IsPositionNameHidden ? @"<td style='" + cellStyle + mandatoryRequirementsCursor + @"' title='Задължителни изисквания' onclick=""" + mandatoryRequirementsOnClick + @""">" + mandatoryRequirements + @"</td>" : "") +
      (!IsPositionNameHidden ? @"<td style='" + cellStyle + additionalRequirementsCursor + @"' title='Допълнителни изисквания' onclick=""" + additionalRequirementsOnClick + @""">" + additionalRequirements + @"</td>" : "") +
      (!IsPositionNameHidden ? @"<td style='" + cellStyle + specificRequirementsCursor + @"' title='Специфични изисквания' onclick=""" + specificRequirementsOnClick + @""">" + specificRequirements + @"</td>" : "") +
                      @"</tr>";

                counter++;
            }
            html += @"</table>
                      </div>";


            html += @"<div style='height: 10px;'>
                </div>

                 <div style='text-align: center'>
                   <span id='lblAddApplicantPositionMessage'>" + htmlNoResults + @"</span><br/>
                </div>
";

            html += @"  </div>                        
                        <div id='btnCloseTable' runat='server' class='Button' onclick=""HideAddApplicantPositionLightBox();""><i></i><div style='width:70px; padding-left:5px;'>Затвори</div><b></b></div>
                      </center>";



            return html;
        }

        private void JSAddApplicantPosition()
        {
            int personID = 0;
            int applicantID = 0;
            int militaryDepartmentID = 0;
            int vacancyAnnouncePositionID = int.Parse(Request.Params["VacancyAnnouncePositionID"]);

            if (!string.IsNullOrEmpty(Request.Params["HdnPersonId"]))
            {
                personID = int.Parse(Request.Params["HdnPersonId"]);
            }

            if (!string.IsNullOrEmpty(Request.Params["ApplicantId"]))
            {
                applicantID = int.Parse(Request.Params["ApplicantId"]);
            }

            if (!string.IsNullOrEmpty(Request.Params["HdnMilitaryDepartmentId"]))
            {
                militaryDepartmentID = int.Parse(Request.Params["HdnMilitaryDepartmentId"]);
            }

            Change change = new Change(CurrentUser, "APPL_Applicants");

            Applicant applicant;
            if (applicantID == 0)
            {
                applicant = new Applicant(CurrentUser);
                applicant.MilitaryDepartmentId = militaryDepartmentID;
                applicant.PersonId = personID;
                ApplicantUtil.SaveApplicant(applicant, CurrentUser, change);
                applicantID = applicant.ApplicantId;
            }

            ApplicantPosition applicantPosition = new ApplicantPosition(CurrentUser);
            applicantPosition.VacancyAnnouncePositionId = vacancyAnnouncePositionID;
            applicantPosition.ApplicantDocsStatusId = ApplicantPositionDocumentStatusUtil.GetApplicantPositionDocumentStatusByKey("MAKEUP", CurrentUser).StatusId;
            ApplicantPositionUtil.SaveApplicantPosition(applicantID, applicantPosition, CurrentUser, change);

            int vacancyAnnounceId = applicantPosition.VacancyAnnouncePosition.VacancyAnnounceID;
            int responsibleMilUnitId = applicantPosition.VacancyAnnouncePosition.ResponsibleMilitaryUnitID.HasValue ? applicantPosition.VacancyAnnouncePosition.ResponsibleMilitaryUnitID.Value : 0;
            ApplicantPositionUtil.RearrangeApplicantPositions(applicantID, vacancyAnnounceId, responsibleMilUnitId, CurrentUser);

            change.WriteLog();

            string response = "<response><ApplicantID>" + applicantID + "</ApplicantID></response>";

            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();
        }

        // Get ajax-requested data for Applicant Position(by ApplicantPositionID)
        private void JSGetApplicantPosition()
        {
            int applicantPositionID = int.Parse(Request.Form["ApplicantPositionID"]);

            ApplicantPosition p = ApplicantPositionUtil.GetApplicantPosition(applicantPositionID, CurrentUser);

            string response = @"<response>                                   
                                    <OrderNum>" + AJAXTools.EncodeForXML(VacancyAnnounceUtil.GetVacancyAnnounce(p.VacancyAnnouncePosition.VacancyAnnounceID, CurrentUser).OrderNum) + @"</OrderNum>
                                    <ResponsibleMilitaryUnit>" + AJAXTools.EncodeForXML(p.VacancyAnnouncePosition.ResponsibleMilitaryUnit != null ? p.VacancyAnnouncePosition.ResponsibleMilitaryUnit.DisplayTextForSelection : "") + @"</ResponsibleMilitaryUnit>
                                    <PositionName>" + AJAXTools.EncodeForXML(p.VacancyAnnouncePosition.PositionName) + @"</PositionName>
                                    <DocStatusID>" + (p.ApplicantDocsStatusId.HasValue ? p.ApplicantDocsStatusId.Value.ToString() : "1") + @"</DocStatusID>
                                </response>";

            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();
        }

        // Check if all documents for an applicant position are sent (from ajax request)
        private void JSCheckApplicantPositionDocuemnts()
        {
            string resultMsg = "OK";

            int applicantPositionID = int.Parse(Request.Form["ApplicantPositionID"]);
            int docStatusID = int.Parse(Request.Form["DocStatusID"]);

            ApplicantPosition p = ApplicantPositionUtil.GetApplicantPosition(applicantPositionID, CurrentUser);
            ApplicantPositionDocumentStatus statusSent = ApplicantPositionDocumentStatusUtil.GetApplicantPositionDocumentStatusByKey("SENT", CurrentUser);

            // if document status is changed to "SENT", then set application position status to "DOCUMENTSAPPLIED"
            if (docStatusID == statusSent.StatusId)
            {
                List<ApplicantDocument> applicantDocuments = ApplicantDocumentUtil.GetApplicantDocumentsForVacancyAnnounce(p.ApplicantId, p.VacancyAnnouncePosition.VacancyAnnounceID, CurrentUser);

                bool anyNonSentDocuments = false;

                foreach (ApplicantDocument applicantDocument in applicantDocuments)
                {
                    if (applicantDocument.ApplicantDocumentStatus == null ||
                       applicantDocument.ApplicantDocumentStatus.StatusKey == "UNKNOWN" ||
                       applicantDocument.ApplicantDocumentStatus.StatusKey == "MISSING")
                    {
                        anyNonSentDocuments = true;
                        break;
                    }
                }

                if (anyNonSentDocuments)
                {
                    resultMsg = "За да може статусът на документите да се смени на '" + statusSent.StatusName + "' трябва всички документи по МЗ за тази позиция да са получени";
                }
            }

            string response = @"<response>" + AJAXTools.EncodeForXML(resultMsg) + "</response>";
            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();
        }

        // Save(update) applicant position from ajax request
        private void JSSaveApplicantPosition()
        {
            string resultMsg = "";

            int applicantID = int.Parse(Request.Form["ApplicantID"]);
            int applicantPositionID = int.Parse(Request.Form["ApplicantPositionID"]);
            int docStatusID = int.Parse(Request.Form["DocStatusID"]);

            ApplicantPosition p = ApplicantPositionUtil.GetApplicantPosition(applicantPositionID, CurrentUser);

            p.ApplicantDocsStatusId = docStatusID;

            // if document status is changed to "SENT", then set application position status to "DOCUMENTSAPPLIED"
            if (docStatusID == ApplicantPositionDocumentStatusUtil.GetApplicantPositionDocumentStatusByKey("SENT", CurrentUser).StatusId)
                p.ApplicantStatusId = ApplicantPositionStatusUtil.GetApplicantPositionStatusByKey("DOCUMENTSAPPLIED", CurrentUser).StatusId;
            else
                p.ApplicantStatusId = null;

            //Track the changes into the Audit Trail 
            Change change = new Change(CurrentUser, "APPL_Applicants");

            if (ApplicantPositionUtil.SaveApplicantPosition(applicantID, p, CurrentUser, change))
                resultMsg = GenerateTabPositionsContent();
            else
                resultMsg = AJAXTools.ERROR;

            change.WriteLog();

            string response = @"<response>" + AJAXTools.EncodeForXML(resultMsg) + "</response>";
            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();
        }

        // Deletes vacancy announce position by ajax request
        private void JSDeleteApplicantPosition()
        {

            if (this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_POSITIONS") != UIAccessLevel.Enabled ||
                this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL") != UIAccessLevel.Enabled ||
                this.GetUIItemAccessLevel("APPL_APPL") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            int applicantID = int.Parse(Request.Form["ApplicantID"]);
            int applicantPositionID = int.Parse(Request.Form["ApplicantPositionID"]);

            string stat = "";
            string response = "";

            ApplicantPosition applicantPosition = ApplicantPositionUtil.GetApplicantPosition(applicantPositionID, CurrentUser);
            int vacancyAnnounceId = applicantPosition.VacancyAnnouncePosition.VacancyAnnounceID;
            int responsibleMilUnitId = applicantPosition.VacancyAnnouncePosition.ResponsibleMilitaryUnitID.HasValue ? applicantPosition.VacancyAnnouncePosition.ResponsibleMilitaryUnitID.Value : 0;
                
            try
            {
                Change change = new Change(CurrentUser, "APPL_Applicants");

                if (!ApplicantPositionUtil.DeleteApplicantPosition(applicantID, applicantPositionID, CurrentUser, change))
                    throw new Exception("Database operation failed!");

                ApplicantPositionUtil.RearrangeApplicantPositions(applicantID, vacancyAnnounceId, responsibleMilUnitId, CurrentUser);

                change.WriteLog();

                stat = AJAXTools.OK;
                response = "<response>" + AJAXTools.EncodeForXML(GenerateTabPositionsContent()) + "</response>";
            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = "<response>" + AJAXTools.EncodeForXML(ex.Message) + "</response>";
            }


            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        //Generate the content of applicant documents tab
        private string GenerateTabDocumentsContent()
        {
            if (this.HdnPersonId > 0)
            {
                string html = "";

                if (GetUIItemAccessLevel("APPL_APPL_EDITAPPL_DOCUMENTS") != UIAccessLevel.Hidden &&
                    GetUIItemAccessLevel("APPL_APPL_EDITAPPL") != UIAccessLevel.Hidden &&
                    GetUIItemAccessLevel("APPL_APPL") != UIAccessLevel.Hidden)
                {
                    html += "<div id='divApplicantDocuments'>";
                    html += this.GenerateApplicantDocumentsHTML(String.Empty, false);
                    html += "</div>";
                }

                return html;
            }
            else
            {
                return "";
            }
        }

        //Gets applicant documents (ajax call)
        private void JSGetApplicantDocuments()
        {
            string status = "";
            string response = "";

            try
            {
                status = AJAXTools.OK;
                response = AJAXTools.EncodeForXML(this.GenerateApplicantDocumentsHTML(String.Empty, false));
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

        //Saves all applicant documents by vacancy announce (ajax call)
        private void JSSaveApplicantDocuments()
        {
            if (GetUIItemAccessLevel("APPL_APPL") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("APPL_APPL_EDITAPPL_DOCUMENTS") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            string status = "";
            string response = "";

            try
            {
                int vacancyAnnounceId = 0;
                int.TryParse(Request.Params["VacancyAnnounceId"], out vacancyAnnounceId);

                if (this.HdnApplicantId == 0 || vacancyAnnounceId == 0)
                {
                    throw new Exception();
                }

                if (!String.IsNullOrEmpty(Request.Params["IdsList"]))
                {
                    Change change = new Change(CurrentUser, "APPL_Applicants");

                    string[] pairIdsArray = Request.Params["IdsList"].Split(';');
                    foreach (string pairIds in pairIdsArray)
                    {
                        string[] idsArray = pairIds.Split(',');
                        int documentId = int.Parse(idsArray[0]);
                        int applDocumentStatusId = int.Parse(idsArray[1]);
                        ApplicantDocument applicantDocument = ApplicantDocumentUtil.GetApplicantDocument(this.HdnApplicantId, vacancyAnnounceId, documentId, CurrentUser);
                        if (applicantDocument == null)
                        {
                            applicantDocument = new ApplicantDocument(CurrentUser);
                        }

                        if (applicantDocument.ApplicantDocumentId == null)
                        {
                            applicantDocument.ApplicantDocumentId = 0;
                        }

                        applicantDocument.Document = new Document(CurrentUser) { DocumentId = documentId };
                        applicantDocument.ApplicantDocumentStatus = new ApplicantDocumentStatus() { StatusId = applDocumentStatusId };

                        ApplicantDocumentUtil.SaveApplicantDocument(applicantDocument, this.HdnApplicantId, vacancyAnnounceId, CurrentUser, change);
                    }

                    change.WriteLog();
                }

                status = AJAXTools.OK;
                response = AJAXTools.EncodeForXML(this.GenerateApplicantDocumentsHTML("Статусът на документите е обновен", false));
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

        //Generate applicant documents tab HTML content
        private string GenerateApplicantDocumentsHTML(string message, bool existError)
        {
            bool pageDisabled = false;

            Person person = PersonUtil.GetPerson(this.HdnPersonId, CurrentUser);

            if (Config.GetWebSetting("KOD_KZV_Check_Applicant").ToLower() == "true" &&
                CommonFunctions.IsKeyInList(person.PersonTypeCode, Config.GetWebSetting("KOD_KZV_Restricted_Applicant")) ||
                !CanCurrentUserAccessThisMilDepartment)
            {
                pageDisabled = true;
            }

            bool isReadOnly = true;
            if (GetUIItemAccessLevel("APPL_APPL_EDITAPPL_DOCUMENTS") == UIAccessLevel.Enabled &&
                GetUIItemAccessLevel("APPL_APPL_EDITAPPL") == UIAccessLevel.Enabled &&
                GetUIItemAccessLevel("APPL_APPL") == UIAccessLevel.Enabled &&
                !pageDisabled)
            {
                isReadOnly = false;
            }

            VacancyAnnounce currentVacancyAnnounce = null;
            int selectedVacancyAnnounceId = 0;
            if (Request.Params["VacancyAnnounceId"] != null
                && Int32.TryParse(Request.Params["VacancyAnnounceId"].ToString(), out selectedVacancyAnnounceId))
            {
                currentVacancyAnnounce = VacancyAnnounceUtil.GetVacancyAnnounce(selectedVacancyAnnounceId, CurrentUser);
            }

            //Generate Applicant Documents table
            string html = "";

            // Generates html for vacancy announces drop down list
            List<VacancyAnnounce> vacancyAnnounces = VacancyAnnounceUtil.GetAllVacancyAnnouncesByApplicantID(this.HdnApplicantId, CurrentUser);
            List<IDropDownItem> ddiVacancyAnnounces = new List<IDropDownItem>();
            foreach (VacancyAnnounce vacancyAnnounce in vacancyAnnounces)
            {
                ddiVacancyAnnounces.Add(vacancyAnnounce);
            }

            IDropDownItem selectedVacancyAnnounce = null;

            if (currentVacancyAnnounce != null)
            {
                selectedVacancyAnnounce = (ddiVacancyAnnounces.Count > 0 ? ddiVacancyAnnounces.Find(va => va.Value() == currentVacancyAnnounce.Value()) : null);
            }
            else
            {
                selectedVacancyAnnounce = (ddiVacancyAnnounces.Count > 0 ? ddiVacancyAnnounces[0] : null);
            }

            string vacancyAnnouncesHTML = ListItems.GetDropDownHtml(ddiVacancyAnnounces, null, "ddlVacancyAnnounces", false, selectedVacancyAnnounce, "ddlVacancyAnnounceChange(this)", "style='min-width: 200px;'");

            if (selectedVacancyAnnounce != null && selectedVacancyAnnounceId == 0)
            {
                int.TryParse(selectedVacancyAnnounce.Value(), out selectedVacancyAnnounceId);
            }

            List<ApplicantDocument> applicantDocuments = ApplicantDocumentUtil.GetApplicantDocumentsForVacancyAnnounce(this.HdnApplicantId, selectedVacancyAnnounceId, CurrentUser);
            List<ApplicantDocumentStatus> applicantDocumentStatuses = ApplicantDocumentStatusUtil.GetAllApplicantDocumentStatus(CurrentUser);

            html += "<table style='min-width: 700px;'>";
            html += "<tr><td align='left'><span class='InputLabel'>Заповед №: </span>" + vacancyAnnouncesHTML + "</td></tr>";
            html += "<tr><td align='center'>";

            //No data found
            if (applicantDocuments.Count == 0)
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
                            <tr>" +
       (isReadOnly ? "" : @"<th style='width: 20px;" + headerStyle + @"'><input type=""checkbox"" id=""cbDocAll"" onclick=""SelectAllDocCB();"" title=""Селектирай всички""/></th>") +
                             @"<th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='width: 500px;" + headerStyle + @"'>Документ</th>
                               <th style='" + headerStyle + @"'>Статус" + (!isReadOnly && applicantDocuments.Count > 0 ? @"<img src='../Images/document_config_16.png' alt='Статус' title='Смяна на статуса на избраните документи' class='GridActionIcon' style='float: right; vertical-align: top;' onclick='ShowApplicantDocumentStatusLightBox();' />" : "") + @"</th>
                            </tr>
                         </thead>";

                int counter = 0;

                //Iterate through all items and add them into the grid
                foreach (ApplicantDocument applDocument in applicantDocuments)
                {
                    counter++;
                    string cellStyle = "vertical-align: top;";

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>" +
            (isReadOnly ? "" : @"<td align='center' style='" + cellStyle + @"'><input type=""checkbox"" id=""cbDoc" + counter.ToString() + @""" value=""" + applDocument.Document.DocumentId + @""" /></td>") +
                               @"<td align='center' style='" + cellStyle + @"'>" + counter.ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>
                                    <span>" + applDocument.Document.DocumentName + @"</span>
                                    <input type='hidden' id='hdnDocId" + counter.ToString() + "' value='" + applDocument.Document.DocumentId + @"' />
                                 </td>
                                 <td style='" + cellStyle + @"'>" + GenerateApplicantDocumentsStatusDDL(applDocument.ApplicantDocumentStatus, applicantDocumentStatuses, counter, isReadOnly) + @"</td>
                              </tr>";
                }

                html += "</table><br />";

                html += "<input type='hidden' id='hdnApplicantDocumentsCount' value='" + counter.ToString() + @"' />";

                string messageClass = "";
                if (existError)
                {
                    messageClass = "ErrorText";
                }
                else
                {
                    messageClass = "SuccessText";
                }

                html += "<span class='" + messageClass + "'>" + message + "</span>";
            }
            html += "</td></tr>";

            if (!isReadOnly && applicantDocuments.Count > 0)
            {
                html += "<tr><td>";
                html += "<div id='btnSaveApplicantDocuments' style='display: inline;' onclick='SaveApplicantDocuments();' class='Button'><i></i><div id='btnSaveApplicantDocumentsText' style='width:90px;'>Запис</div><b></b></div><br />";
                html += "</td></tr>";
            }

            html += "</table>";

            return html;
        }

        private string GenerateApplicantDocumentsStatusDDL(ApplicantDocumentStatus currentApplicantDocumentStatus, List<ApplicantDocumentStatus> applDocumentStatuses, int index, bool isReadOnly)
        {
            // Generates html for vacancy announce document statuses drop down list
            List<IDropDownItem> ddiApplicantDocumentStatuses = new List<IDropDownItem>();
            foreach (ApplicantDocumentStatus applicantDocumentStatus in applDocumentStatuses)
            {
                ddiApplicantDocumentStatuses.Add(applicantDocumentStatus);
            }

            IDropDownItem selectedApplicantDocumentStatus = null;

            if (currentApplicantDocumentStatus != null)
            {
                selectedApplicantDocumentStatus = (ddiApplicantDocumentStatuses.Count > 0 ? ddiApplicantDocumentStatuses.Find(ads => ads.Value() == currentApplicantDocumentStatus.StatusId.ToString()) : null);
            }
            else
            {
                selectedApplicantDocumentStatus = (ddiApplicantDocumentStatuses.Count > 0 ? ddiApplicantDocumentStatuses[0] : null);
            }

            return ListItems.GetDropDownHtml(ddiApplicantDocumentStatuses, null, "ddlApplicantDocumentStatuses" + index.ToString(), false, selectedApplicantDocumentStatus, null, (isReadOnly ? "disabled='disabled' style='min-width: 50px;'" : "style='min-width: 50px;'"));
        }

        //Generate html content for applicant document status light box
        private void JSGetApplicantDocumentStatusLightBoxContent()
        {
            // Generates html for applicant document statuses drop down list
            List<ApplicantDocumentStatus> applicantDocumentStatuses = ApplicantDocumentStatusUtil.GetAllApplicantDocumentStatus(CurrentUser);
            List<IDropDownItem> ddlApplicantDocumentStatuses = new List<IDropDownItem>();
            foreach (ApplicantDocumentStatus applicantDocumentStatus in applicantDocumentStatuses)
            {
                ddlApplicantDocumentStatuses.Add(applicantDocumentStatus);
            }

            string applicantDocumentStatusesHTML = ListItems.GetDropDownHtml(ddlApplicantDocumentStatuses, null, "ddlChangeApplicantDocumentStatuses", false, null, null, "style='min-width: 200px;'");

            string html = "";
            html += @"                    
                    <center>
                        <table style='text-align:center;'>
                        <tr style='height: 15px'></tr>
                        <tr>
                            <td colspan='2' align='center'>
                                <span id='lblApplicantDocumentStatusTitle' class='SmallHeaderText'>Смяна на статуса на документите</span>
                            </td>
                        </tr>
                        <tr style='height: 17px'></tr>
                        <tr style='min-height: 17px;'>
                            <td align='right' style='width: 100px;'>
                                <span id='lblChangeApplicantDocumentStatus' class='InputLabel'>Статус:</span>
                            </td>
                            <td align='left' style='min-width: 220px;'>
                                <span id='spChangeApplicantDocumentStatus'>" + applicantDocumentStatusesHTML + @"</span>
                            </td>
                        </tr>                     
                        <tr style='height: 20px'></tr>
                        <tr>
                            <td colspan='2' style='text-align: center;'>
                                <table style='margin: 0 auto;'>
                                   <tr>
                                      <td>
                                         <div id='btnChangeApplicantDocumentStatuses' style='display: inline;' onclick='ChangeApplicantDocumentStatuses();' class='Button'><i></i><div id='btnChangeApplicantDocumentStatusesText' style='width:70px;'>Избор</div><b></b></div>
                                         <div id='btnCloseApplicantDocumentStatusLightBox' style='display: inline;' onclick='HideApplicantDocumentStatusLightBox();' class='Button'><i></i><div id='btnApplicantDocumentStatusLightBoxText' style='width:70px;'>Затвори</div><b></b></div>
                                      </td>
                                   </tr>
                                </table>                    
                            </td>
                        </tr>
                   </table>
                   </center>";

            string status = AJAXTools.OK;
            string response = AJAXTools.EncodeForXML(html);

            AJAX a = new AJAX(response, status, Response);
            a.Write();
            Response.End();
        }

        private void JSPrintDocuments()
        {
            if (this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_PRINT_DOCUMENTS") == UIAccessLevel.Disabled ||
                this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL") != UIAccessLevel.Enabled ||
                this.GetUIItemAccessLevel("APPL_APPL") != UIAccessLevel.Enabled ||
                pageDisabled)
                RedirectAjaxAccessDenied();

            int personId = int.Parse(Request.Params["PersonId"]);
            string result = EditApplicant_PageUtil.PrintDocuments(personId, CurrentUser);

            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment;filename=Documents.doc");
            Response.ContentType = "application/vnd.ms-word";

            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            Response.Write(result);
            Response.End();
        }

        private void JSPrintLetter()
        {
            if (this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_PRINT_LETTERS") != UIAccessLevel.Enabled ||
                this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL") != UIAccessLevel.Enabled ||
                this.GetUIItemAccessLevel("APPL_APPL") != UIAccessLevel.Enabled ||
                pageDisabled)
                RedirectAjaxAccessDenied();

            int applicantId = int.Parse(Request.Params["ApplicantId"]);
            string letterValue = Request.Params["Letter"];
            string result = EditApplicant_PageUtil.PrintLetter(applicantId, letterValue, CurrentUser);

            string fileName = "";

            switch (letterValue)
            {
                case "LPZP": fileName = "Pismo LPZP nov obr.";
                    break;
                case "CVMKVarna": fileName = "Pismo CVMK Varna";
                    break;
                case "CVMKPlovdiv": fileName = "Pismo CVMK Plovdiv";
                    break;
                case "CVMKSofia": fileName = "Pismo CVMK Sofia";
                    break;
                default: fileName = "Pismo";
                    break;
            }

            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment;filename=" + fileName + ".doc");
            Response.ContentType = "application/vnd.ms-word";

            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            Response.Write(result);
            Response.End();
        }

        private void JSPrintApplication()
        {
            if (this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_POSITIONS_REGISTER_APPLICATION") != UIAccessLevel.Enabled ||
                this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_POSITIONS") != UIAccessLevel.Enabled ||
                this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL") != UIAccessLevel.Enabled ||
                this.GetUIItemAccessLevel("APPL_APPL") != UIAccessLevel.Enabled ||
                pageDisabled)
                RedirectAjaxAccessDenied();

            int applicantId = int.Parse(Request.Params["ApplicantId"]);
            string appValue = Request.Params["App"];
            int vacancyAnnounceId = int.Parse(Request.Params["VacAnnId"]);

            int responsibleMilitaryUnitId = 0;

            if (!String.IsNullOrEmpty(Request.Params["ResMilUnId"]))
                responsibleMilitaryUnitId = int.Parse(Request.Params["ResMilUnId"]);

            string result = EditApplicant_PageUtil.PrintApplication(applicantId, appValue, vacancyAnnounceId, responsibleMilitaryUnitId, CurrentUser);

            string fileName = "";

            if (appValue == GeneratePrintApplicantUtil.ApplicationValue.VS.ToString())
                fileName = "Zaiavlenie VS";
            else if (appValue == GeneratePrintApplicantUtil.ApplicationValue.DR4B.ToString())
                fileName = "Zaiavlenie DR chlen 4B";
            else if (appValue == GeneratePrintApplicantUtil.ApplicationValue.DR6.ToString())
                fileName = "Zaiavlenie DR chlen 6";
            else if (appValue == GeneratePrintApplicantUtil.ApplicationValue.NVP.ToString())
                fileName = "Zaiavlenie NVP";
            else if (appValue == GeneratePrintApplicantUtil.ApplicationValue.SVP.ToString())
                fileName = "Zaiavlenie SVP";
            else
                fileName = "Zaiavlenie";

            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment;filename=" + fileName + ".doc");
            Response.ContentType = "application/vnd.ms-word";

            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            Response.Write(result);
            Response.End();
        }

        public void JSRegister()
        {
            int applicantId = int.Parse(Request.Params["ApplicantId"]);
            int vacancyAnnounceId = int.Parse(Request.Params["VacAnnId"]);

            int responsibleMilitaryUnitId = 0;

            if (!String.IsNullOrEmpty(Request.Params["ResMilUnId"]))
                responsibleMilitaryUnitId = int.Parse(Request.Params["ResMilUnId"]);

            string tblCount = Request.Form["TblCount"];

            string stat = "";
            string response = "";

            try
            {
                Register register = RegisterApplication(applicantId, vacancyAnnounceId, responsibleMilitaryUnitId);

                stat = AJAXTools.OK;
                response = @"<response>OK</response>
                             <registerNumber>" + AJAXTools.EncodeForXML(register.RegisterNumber.ToString()) + @"</registerNumber>
                             <tblCount>" + AJAXTools.EncodeForXML(tblCount) + @"</tblCount>
                             ";
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

        public void JSSaveRegisterData()
        {
            int applicantId = int.Parse(Request.Params["ApplicantId"]);
            int vacancyAnnounceId = int.Parse(Request.Params["VacancyAnnounceId"]);

            int responsibleMilitaryUnitId = 0;

            if (!String.IsNullOrEmpty(Request.Params["ResponsibleMilitaryUnitId"]))
                responsibleMilitaryUnitId = int.Parse(Request.Params["ResponsibleMilitaryUnitId"]);

            string documentDate = Request.Form["DocumentDate"].ToString();

            string pageCount = Request.Form["PageCount"];
            string notes = Request.Form["Notes"];
            string tblCount = Request.Form["TblCount"];

            Register register = RegisterUtil.GetRegister(applicantId, vacancyAnnounceId, responsibleMilitaryUnitId, CurrentUser);
            register.DocumentDate = (!String.IsNullOrEmpty(documentDate) ? CommonFunctions.ParseDate(documentDate) : (DateTime?)null); ;
            register.PageCount = pageCount;
            register.Notes = notes;

            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "APPL_Applicants");
                RegisterUtil.UpdateRegister(register, CurrentUser, change);
                change.WriteLog();

                stat = AJAXTools.OK;
                response = @"<response>OK</response>
                             <docmentDate>" + AJAXTools.EncodeForXML(register.DocumentDate.HasValue ? register.DocumentDate.Value.ToString("d.M.yyyy") : "") + @"</docmentDate>
                             <pageCount>" + AJAXTools.EncodeForXML(!String.IsNullOrEmpty(register.PageCount) ? register.PageCount.ToString() : "") + @"</pageCount>
                             <notes>" + AJAXTools.EncodeForXML(!String.IsNullOrEmpty(register.Notes) ? register.Notes.ToString() : "") + @"</notes>
                             <tblCount>" + AJAXTools.EncodeForXML(tblCount) + @"</tblCount>
                             ";
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


        private Register RegisterApplication(int applicantId, int vacancyAnnounceId, int responsibleMilitaryUnitId)
        {
            Applicant applicant = ApplicantUtil.GetApplicant(applicantId, CurrentUser);
            int militaryDepartmentId = applicant.MilitaryDepartmentId;
            VacancyAnnounce vacancyAnnounce = VacancyAnnounceUtil.GetVacancyAnnounce(vacancyAnnounceId, CurrentUser);
            MilitaryUnit responsibleMilitaryUnit = MilitaryUnitUtil.GetMilitaryUnit(responsibleMilitaryUnitId, CurrentUser);

            Register register = RegisterUtil.GetRegister(applicantId, vacancyAnnounceId, responsibleMilitaryUnitId, CurrentUser);

            if (register == null)
            {
                int currentYear = DateTime.Now.Year;
                RegisterNumbers registerNumbers = RegisterNumbersUtil.GetRegisterNumbers(militaryDepartmentId, currentYear, CurrentUser);
                int regNumber = 0;

                if (registerNumbers == null)
                {
                    RegisterNumbersUtil.AddNewRegisterNumbers(militaryDepartmentId, currentYear, CurrentUser);
                    registerNumbers = RegisterNumbersUtil.GetRegisterNumbers(militaryDepartmentId, currentYear, CurrentUser);
                    regNumber = registerNumbers.LastNumber;
                }
                else
                {
                    regNumber = registerNumbers.LastNumber + 1;
                }

                Change change = new Change(CurrentUser, "APPL_Applicants");

                Register newRegister = new Register(CurrentUser);
                newRegister.ApplicantId = applicantId;
                newRegister.VacancyAnnounceId = vacancyAnnounceId;
                newRegister.ResponsibleMilitaryUnitId = responsibleMilitaryUnitId;
                newRegister.Applicant = applicant;
                newRegister.VacancyAnnounce = vacancyAnnounce;
                newRegister.ResponsibleMilitaryUnit = responsibleMilitaryUnit;
                newRegister.RegisterNumber = regNumber;

                RegisterUtil.AddNewRegister(newRegister, CurrentUser, change);
                change.WriteLog();

                if (registerNumbers.LastNumber != regNumber)
                    RegisterNumbersUtil.UpdateRegisterNumbers(militaryDepartmentId, currentYear, regNumber, CurrentUser);

                register = RegisterUtil.GetRegister(applicantId, vacancyAnnounceId, responsibleMilitaryUnitId, CurrentUser);
            }

            return register;
        }

        public bool IsPositionsVisible()
        {
            return this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_POSITIONS") != UIAccessLevel.Hidden;
        }

        public bool IsEducationVisible()
        {
            return this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU") != UIAccessLevel.Hidden;
        }

        public bool IsDocumentsVisible()
        {
            return this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_DOCUMENTS") != UIAccessLevel.Hidden;
        }

        public bool IsHistoryVisible()
        {
            return this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_HISTORY") != UIAccessLevel.Hidden;
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

        private string GenerateApplicantPositionsTable(int applicantId, List<ApplicantPosition> positions, int idx)
        {
            StringBuilder sb = new StringBuilder();

            bool IsOrderNumHidden = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_POSITIONS_ORDERNUM") == UIAccessLevel.Hidden;
            bool IsSubmitDocsDepartmentHidden = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_POSITIONS_SUBMITDOCSDEPARTMENT") == UIAccessLevel.Hidden;
            bool IsMilitaryUnitHidden = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_POSITIONS_MILITARYUNIT") == UIAccessLevel.Hidden;
            bool IsPositionNameHidden = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_POSITIONS_POSITIONNAME") == UIAccessLevel.Hidden;
            bool IsRespMilitaryUnitHidden = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_POSITIONS_RESPMILITARYUNIT") == UIAccessLevel.Hidden;
            bool IsDocStatusHidden = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_POSITIONS_DOCSTATUS") == UIAccessLevel.Hidden;
            bool IsStatusHidden = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_POSITIONS_STATUS") == UIAccessLevel.Hidden;
            bool IsRegisterApplicationHiddent = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_POSITIONS_REGISTER_APPLICATION") == UIAccessLevel.Hidden;
            bool IsRegisterApplicationDisabled = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_POSITIONS_REGISTER_APPLICATION") == UIAccessLevel.Disabled;
            bool pageDisabled = false;

            Applicant applicant = ApplicantUtil.GetApplicant(applicantId, CurrentUser);
            Person person = applicant.Person;

            string milDepID = applicant.MilitaryDepartmentId.ToString();
            string[] currentUserMilDepartmentIDs = CurrentUser.MilitaryDepartmentIDs_ListOfValues.Split(',');
            canCurrentUserAccessThisMilDepartment = currentUserMilDepartmentIDs.Any(c => c == milDepID);

            if (Config.GetWebSetting("KOD_KZV_Check_Applicant").ToLower() == "true" &&
                            CommonFunctions.IsKeyInList(person.PersonTypeCode, Config.GetWebSetting("KOD_KZV_Restricted_Applicant")) ||
                            !canCurrentUserAccessThisMilDepartment)
            {
                pageDisabled = true;
            }

            bool positionsTableEditPermission = this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL_POSITIONS") == UIAccessLevel.Enabled &&
                                                this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL") == UIAccessLevel.Enabled &&
                                                this.GetUIItemAccessLevel("APPL_APPL") == UIAccessLevel.Enabled &&
                                                !pageDisabled;

            sb.Append("<table name='applicantPositionsTable' class='CommonHeaderTable' style='text-align: left; width: 100%; background: #FFFFFF;' border='1px'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style='width: 15px;vertical-align: bottom; border-right:0px;'>&nbsp;</th>");
            sb.Append("<th style=\"min-width: 15px;\">№</th>");
            if (!IsSubmitDocsDepartmentHidden)
                sb.Append("<th style=\"width: 80px;\">Подадени документи в</th>");
            if (!IsMilitaryUnitHidden)
                sb.Append("<th style=\"min-width: 140px;\">" + CommonFunctions.GetLabelText("MilitaryUnit") + "</th>");
            if (!IsPositionNameHidden)
                sb.Append("<th colspan=\"2\" style=\"min-width: 230px;\">Длъжност</th>");
            if (!IsDocStatusHidden)
                sb.Append("<th style=\"width: 100px;\">Статус на документите</th>");
            if (!IsStatusHidden)
                sb.Append("<th style=\"width: 90px;\">Статус</th>");
            sb.Append("<th style=\"width: 50px;\"></th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            int counter = 1;

            if (positions.Count > 0)
            {
                sb.Append("<tbody>");
            }

            foreach (ApplicantPosition position in positions)
            {
                bool otherMilitaryDepartment = false;

                //If the position is for other ApplicantID (i.e. other department) then do not allow editing it
                if (position.ApplicantId != applicantId)
                {
                    otherMilitaryDepartment = true;
                }

                string moveUpHTML = "";
                string moveDownHTML = "";

                if (positionsTableEditPermission && !otherMilitaryDepartment)
                {
                    if (counter != 1)
                    {
                        moveUpHTML += "<img src ='../Images/move_up.gif' alt='Преместване нагоре' title='Преместване нагоре' class='GridActionIcon' onclick='MoveApplicantPosition(" + idx + ", " + position.ApplicantPositionId.ToString() + ", " + positions[counter - 2].ApplicantPositionId.ToString() + @");' />";
                    }

                    if (counter != positions.Where(x => x.ApplicantId == applicantId).ToList().Count)
                    {
                        moveDownHTML += "<img src='../Images/move_down.gif' alt='Преместване надолу' title='Преместване надолу' class='GridActionIcon' onclick='MoveApplicantPosition(" + idx + ", " + position.ApplicantPositionId.ToString() + ", " + positions[counter].ApplicantPositionId.ToString() + @");' />";
                    }
                }

                string moveHTML = moveUpHTML;
                moveHTML += (!string.IsNullOrEmpty(moveHTML) ? "<br>" : "");
                moveHTML += moveDownHTML;

                sb.Append("<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + "'>");
                sb.Append("<td style='text-align: left; vertical-align: middle; border-right: 0px;'>" + moveHTML + @"</td>");
                sb.Append("<td>" + counter + "</td>");
                if (!IsSubmitDocsDepartmentHidden)
                    sb.Append("<td>" + (position.SubmitDocsDepartment != null ? position.SubmitDocsDepartment.MilitaryDepartmentName : "") + "</td>");
                if (!IsMilitaryUnitHidden)
                    sb.Append("<td>" + (position.VacancyAnnouncePosition.MilitaryUnit != null ? position.VacancyAnnouncePosition.MilitaryUnit.DisplayTextForSelection : "") + "</td>");
                if (!IsPositionNameHidden)
                {
                    string mandatoryRequirementsImg = position.VacancyAnnouncePosition.MandatoryRequirements.Length > 0 ? "../Images/Z_active.png" : "../Images/Z_inactive.png";
                    string additionalRequirementsImg = position.VacancyAnnouncePosition.AdditionalRequirements.Length > 0 ? "../Images/D_active.png" : "../Images/D_inactive.png";
                    string specificRequirementsImg = position.VacancyAnnouncePosition.SpecificRequirements.Length > 0 ? "../Images/C_active.png" : "../Images/C_inactive.png";

                    string mandatoryRequirementsCursor = (position.VacancyAnnouncePosition.MandatoryRequirements.Length > 0 ? " cursor: pointer;" : " cursor: default;");
                    string additionalRequirementsCursor = (position.VacancyAnnouncePosition.AdditionalRequirements.Length > 0 ? " cursor: pointer;" : " cursor: default;");
                    string specificRequirementsCursor = (position.VacancyAnnouncePosition.SpecificRequirements.Length > 0 ? " cursor: pointer;" : " cursor: default;");

                    string mandatoryRequirementsFull = CommonFunctions.ReplaceNewLinesInString(position.VacancyAnnouncePosition.MandatoryRequirements.Replace("'", "\'").Replace(@"""", "&quot;"));
                    string additionalRequirementsFull = CommonFunctions.ReplaceNewLinesInString(position.VacancyAnnouncePosition.AdditionalRequirements.Replace("'", "\'").Replace(@"""", "&quot;"));
                    string specificRequirementsFull = CommonFunctions.ReplaceNewLinesInString(position.VacancyAnnouncePosition.SpecificRequirements.Replace("'", "\'").Replace(@"""", "&quot;"));

                    string mandatoryRequirementsOnClick = position.VacancyAnnouncePosition.MandatoryRequirements.Length > 0 ? "ShowInfoPopup('Задължителни изисквания', '" + mandatoryRequirementsFull + "', false, true, event);" : "";
                    string additionalRequirementsOnClick = position.VacancyAnnouncePosition.AdditionalRequirements.Length > 0 ? "ShowInfoPopup('Допълнителни изисквания', '" + additionalRequirementsFull + "', false, true, event);" : "";
                    string specificRequirementsOnClick = position.VacancyAnnouncePosition.SpecificRequirements.Length > 0 ? "ShowInfoPopup('Специфични изисквания', '" + specificRequirementsFull + "', false, true, event);" : "";

                    string mandatoryRequirementTooltip = position.VacancyAnnouncePosition.MandatoryRequirements.Length > 0 ? "Задължителни изисквания" : "Няма задължителни изисквания";
                    string additionalRequirementsTooltip = position.VacancyAnnouncePosition.AdditionalRequirements.Length > 0 ? "Допълнителни изисквания" : "Няма допълнителни изисквания";
                    string specificRequirementsTooltip = position.VacancyAnnouncePosition.SpecificRequirements.Length > 0 ? "Специфични изисквания" : "Няма специфични изисквания";

                    sb.Append("<td style='border-right: none;'>" + position.VacancyAnnouncePosition.PositionName + "</td>");
                    sb.Append("<td style='border-left: none; text-align: right;'>");
                    sb.Append("<img border='0' src='" + mandatoryRequirementsImg + "' alt='Задължителни изисквания' title='" + mandatoryRequirementTooltip + "' onclick=\"" + mandatoryRequirementsOnClick + "\" style='width: 14px; height: 14px; margin-bottom: 3px;" + mandatoryRequirementsCursor + "' />");
                    sb.Append("<img border='0' src='" + additionalRequirementsImg + "' alt='Допълнителни изисквания' title='" + additionalRequirementsTooltip + "' onclick=\"" + additionalRequirementsOnClick + "\" style='width: 14px; height: 14px; margin-bottom: 3px;" + additionalRequirementsCursor + "' />");
                    sb.Append("<img border='0' src='" + specificRequirementsImg + "' alt='Специфични изисквания' title='" + specificRequirementsTooltip + "' onclick=\"" + specificRequirementsOnClick + "\" style='width: 14px; height: 14px;" + specificRequirementsCursor + "' />");
                    sb.Append("</td>");
                }

                if (!IsDocStatusHidden)
                    sb.Append("<td>" + (position.ApplicantDocsStatus != null ? position.ApplicantDocsStatus.StatusName : "") + "</td>");
                if (!IsStatusHidden)
                    sb.Append("<td>" + (position.ApplicantStatus != null ? position.CombinedApplicantStatus : "") + "</td>");

                // add edit and delete icons(buttons), which calls javascript functionality for necessary actions
                if (positionsTableEditPermission && !otherMilitaryDepartment)
                {
                    if (position.ApplicantDocsStatus != null)
                    {
                        if (position.ApplicantDocsStatus.StatusKey != "SENT")
                        {
                            sb.Append(@"<td><img border='0' src='../Images/edit.png' alt='Редактиране' title='Редактиране' onclick='javascript:ShowApplicantPositionLightBox(" + position.ApplicantPositionId + @");' style='cursor: pointer;' />&nbsp;<img border='0' src='../Images/delete.png' alt='Изтриване' title='Изтриване' onclick='javascript:DeleteApplicantPosition(" + position.ApplicantPositionId + @");' style='cursor: pointer;' /></td>");
                        }
                        else
                        {
                            if (position.ApplicantStatus == null ||
                               (position.ApplicantStatus.StatusKey == "DOCUMENTSAPPLIED" && position.Rating == null))
                            {
                                sb.Append(@"<td><img border='0' src='../Images/edit.png' alt='Редактиране' title='Редактиране' onclick='javascript:ShowApplicantPositionLightBox(" + position.ApplicantPositionId + @");' style='cursor: pointer;' />&nbsp;</td>");
                            }
                            else
                            {
                                sb.Append(@"<td>&nbsp;</td>");
                            }
                        }
                    }
                    else
                    {
                        sb.Append(@"<td><img border='0' src='../Images/edit.png' alt='Редактиране' title='Редактиране' onclick='javascript:ShowApplicantPositionLightBox(" + position.ApplicantPositionId + @");' style='cursor: pointer;' />&nbsp;<img border='0' src='../Images/delete.png' alt='Изтриване' title='Изтриване' onclick='javascript:DeleteApplicantPosition(" + position.ApplicantPositionId + @");' style='cursor: pointer;' /></td>");
                    }
                }
                else
                {
                    sb.Append(@"<td>&nbsp;</td>");
                }

                sb.Append("</tr>");
                counter++;
            }

            if (positions.Count > 0)
            {
                sb.Append("</tbody>");
            }

            sb.Append("</table>");

            return sb.ToString();
        }

        //Change the order of the Applicant Positions
        private void JSMoveApplicantPosition()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "APPL_Applicants");

                int applicantPositionID_1 = int.Parse(Request.Form["ApplicantPositionID_1"]);
                int applicantPositionID_2 = int.Parse(Request.Form["ApplicantPositionID_2"]);

                ApplicantPosition applicantPos1 = ApplicantPositionUtil.GetApplicantPosition(applicantPositionID_1, CurrentUser);
                ApplicantPosition applicantPos2 = ApplicantPositionUtil.GetApplicantPosition(applicantPositionID_2, CurrentUser);
                ApplicantPositionUtil.SwapApplicantPositionsOrder(CurrentUser, change, applicantPos1, applicantPos2);

                change.WriteLog();

                int idx = int.Parse(Request.Form["Idx"]);

                Applicant applicant = ApplicantUtil.GetApplicant(applicantPos1.ApplicantId, CurrentUser);
                int vacancyAnnounceId = applicantPos1.VacancyAnnouncePosition.VacancyAnnounceID;
                int? resMilitaryUnitId = applicantPos1.VacancyAnnouncePosition.ResponsibleMilitaryUnitID;

                List<ApplicantPosition> positionsAll = ApplicantPositionUtil.GetAllApplicantPositionByPersonID(applicant.PersonId, false, CurrentUser);
                List<ApplicantPosition> selectedPositions = positionsAll
                    .Where(x=> x.VacancyAnnouncePosition.VacancyAnnounceID == vacancyAnnounceId && x.VacancyAnnouncePosition.ResponsibleMilitaryUnitID == resMilitaryUnitId)
                    .OrderByDescending(x => x.ApplicantId == applicant.ApplicantId).ThenBy(x => x.Seq).ToList();

                string refreshedPositionsTable = GenerateApplicantPositionsTable(applicant.ApplicantId, selectedPositions, idx);

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
    }

    public static class EditApplicant_PageUtil
    {
        public static string PrintDocuments(int personId, User currentUser)
        {
            List<int> personIDs = new List<int>();
            personIDs.Add(personId);

            string result = GeneratePrintApplicantUtil.PrintDocuments(personIDs, currentUser);
            return result;
        }

        public static string PrintLetter(int applicantId, string letter, User currentUser)
        {
            List<int> applicantIDs = new List<int>();
            applicantIDs.Add(applicantId);

            string result = GeneratePrintApplicantUtil.PrintLetter(applicantIDs, letter, currentUser);
            return result;
        }

        public static string PrintApplication(int applicantId, string appValue, int vacancyAnnounceId, int responsibleMilitaryUnitId, User currentUser)
        {
            List<int> applicantIDs = new List<int>();
            applicantIDs.Add(applicantId);

            string result = GeneratePrintApplicantUtil.PrintApplication(applicantIDs, appValue, vacancyAnnounceId, responsibleMilitaryUnitId, currentUser);
            return result;
        }
    }
}
