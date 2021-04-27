using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;

using PMIS.Common;
using PMIS.Applicants.Common;


namespace PMIS.Applicants.ContentPages
{
    public partial class AddPotencialApplicant_PersonDetails : APPLPage
    {
        private bool canCurrentUserAccessThisMilDepartment;

        UIAccessLevel l;

        //Use this for disable/hide UI client controls
        List<string> disabledClientControls = new List<string>();
        List<string> hiddenClientControls = new List<string>();

        string responseBack = "";

        public override string PageUIKey
        {
            get
            {
                return "APPL_POTENCIALAPPL";
            }
        }

        //Get-Set Id for person (0 - if new)
        private int PersonId
        {
            get
            {
                int personId = 0;
                //gets personId either from the hidden field on the page or from page url
                if (String.IsNullOrEmpty(this.hdnPersonID.Value))
                {
                    if (Request.Params["PersonId"] != null)
                        Int32.TryParse(Request.Params["PersonId"].ToString(), out personId);

                    if (personId == 0)
                    {
                        personId = this.GetPersonId();
                    }

                    //sets person ID in hidden field on the page in order to be accessible in javascript
                    this.hdnPersonID.Value = personId.ToString();
                }
                else
                {
                    Int32.TryParse(this.hdnPersonID.Value, out personId);
                }

                return personId;
            }
            set { this.hdnPersonID.Value = value.ToString(); }
        }

        private int HdnPotencialApplicantId
        {
            get
            {
                int hdnPotencialApplicantId = 0;
                //gets applicantId from page url
                if (Request.Params["PotencialApplicantId"] != null)
                    Int32.TryParse(Request.Params["PotencialApplicantId"].ToString(), out hdnPotencialApplicantId);

                return hdnPotencialApplicantId;
            }
        }

        private bool CanCurrentUserAccessThisMilDepartment
        {
            get
            {
                string milDepID = "";

                if (HdnPotencialApplicantId > 0)
                {
                    PotencialApplicant potencialApplicant = PotencialApplicantUtil.GetPotencialApplicant(HdnPotencialApplicantId, CurrentUser);
                    milDepID = potencialApplicant.MilitaryDepartmentId.ToString();
                }
                else
                {
                    milDepID = this.hdnMilitaryDepartmentID.Value;
                }

                string[] currentUserMilDepartmentIDs = CurrentUser.MilitaryDepartmentIDs_ListOfValues.Split(',');

                canCurrentUserAccessThisMilDepartment = currentUserMilDepartmentIDs.Any(c => c == milDepID);

                return canCurrentUserAccessThisMilDepartment;
            }
        }

        private bool PageDisabled
        {
            get
            {
                bool pageDisabled = false;

                Person person = null;

                if (!String.IsNullOrEmpty(hdnPotencialApplicantId.Value))
                {
                    PotencialApplicant potentialApplicant = PotencialApplicantUtil.GetPotencialApplicant(int.Parse(hdnPotencialApplicantId.Value), CurrentUser);
                    person = potentialApplicant.Person;
                }
                else
                {
                    person = PersonUtil.GetPersonByIdentNumber(hdnIdentNumber.Value, CurrentUser);
                }


                if (Config.GetWebSetting("KOD_KZV_Check_PotentialApplicant").ToLower() == "true" &&
                    CommonFunctions.IsKeyInList(person.PersonTypeCode, Config.GetWebSetting("KOD_KZV_Restricted_PotentialApplicant")) || !CanCurrentUserAccessThisMilDepartment)
                {
                    pageDisabled = true;
                }

                return pageDisabled;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if (GetUIItemAccessLevel("APPL_POTENCIALAPPL") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            //Set the Request parameters
            if (!String.IsNullOrEmpty(Request.Params["PotencialApplicantId"]))
                hdnPotencialApplicantId.Value = Request.Params["PotencialApplicantId"];

            if (!String.IsNullOrEmpty(Request.Params["IdentNumber"]))
                hdnIdentNumber.Value = Request.Params["IdentNumber"];

            if (!String.IsNullOrEmpty(Request.Params["MilitaryDepartmentId"]))
                hdnMilitaryDepartmentID.Value = Request.Params["MilitaryDepartmentId"];

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadPersonDetails")
            {
                JSLoadPersonDetails();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSavePersonDetails")
            {
                JSSavePersonDetails();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRepopulateMunicipality")
            {
                JSRepopulateMunicipality();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRepopulateCity")
            {
                JSRepopulateCity();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRepopulatePostCode")
            {
                JSRepopulatePostCode();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRepopulateRegionMunicipalityCity")
            {
                JSRepopulateRegionMunicipalityCity();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRepopulatePostCodeAndDistrict")
            {
                JSRepopulatePostCodeAndDistrict();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRepopulateDistrictPostCode")
            {
                JSRepopulateDistrictPostCode();
                return;
            }


            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRepopulateRegionMunicipalityCityDistrict")
            {
                JSRepopulateRegionMunicipalityCityDistrict();
                return;
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
                if ((Request.Params["SelectedTabId"] != null) && (Request.Params["PersonId"] != null)
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
                if ((Request.Params["SelectedTabId"] != null) && (Request.Params["PersonId"] != null)
                    && (Request.Params["ForeignLanguageId"] != null))
                {
                    this.JSDeleteApplicantLanguage();
                    return;
                }
            }

            jsItemSelectorDiv.InnerHtml = @"<script src=""" + Page.ClientScript.GetWebResourceUrl(typeof(ItemSelector.ItemSelector), "ItemSelector.ItemSelector.js") + @""" type=""text/javascript""></script>";

            //Hilight the correct item in the menu
            HighlightMenuItems("Applicants", "PotencialApplicants_Add");

            //Hide the navigation buttons
            HideNavigationControls(btnBack);

            //Populate the drop-downs
            PopulateDropdowns();

            //For save logic
            LnkForceNoChangesCheck(btnSave);

            if (Request.Params["PageFrom"] != null && (Request.Params["PageFrom"] == "1"))
            {
                responseBack = "~/ContentPages/ManagePotencialApplicants.aspx";
            }
            else
            {
                responseBack = "~/ContentPages/AddPotencialApplicant_SelectPerson.aspx";
            }

            this.SetSetupPageUIServerControls(); //setup user interface elements according to rights of the user's role

            this.txtLastApperiance.CssClass = CommonFunctions.DatePickerCSS();

            SetMilitaryDepartmentName();
        }

        private void SetMilitaryDepartmentName()
        {
            string militaryDepartmentName = "";

            if (hdnMilitaryDepartmentID.Value != "")
            {
                try
                {
                    militaryDepartmentName = MilitaryDepartmentUtil.GetMilitaryDepartment(int.Parse(hdnMilitaryDepartmentID.Value), CurrentUser).MilitaryDepartmentName;
                }
                catch
                { }
            }
            else
            {
                if (hdnPotencialApplicantId.Value != "")
                {
                    militaryDepartmentName = PotencialApplicantUtil.GetPotencialApplicant(int.Parse(hdnPotencialApplicantId.Value), CurrentUser).MilitaryDepartment.MilitaryDepartmentName;
                }
            }

            spanMilitaryDepartmentName.InnerText = militaryDepartmentName;
        }

        //Populate the drop-downs
        private void PopulateDropdowns()
        {
            PopulateGender();
            PopulateDrivingLicenseCategories();

            this.PopulateRegions();
        }

        //Populate the Gender drop-down
        private void PopulateGender()
        {
            List<Gender> genders = GenderUtil.GetGenders(CurrentUser);

            List<IDropDownItem> ddItems = new List<IDropDownItem>();
            foreach (Gender gender in genders)
            {
                ddItems.Add(gender);
            }

            string gendersHTML = ListItems.GetDropDownHtml(ddItems, "ddGender", true);
            pnlGenderContainer.InnerHtml = gendersHTML;
        }

        //Populate the DrivingLicenseCategories pick list
        private void PopulateDrivingLicenseCategories()
        {
            string result = "";

            List<DrivingLicenseCategory> categories = DrivingLicenseCategoryUtil.GetAllDrivingLicenseCategories(CurrentUser);

            foreach (DrivingLicenseCategory category in categories)
            {
                string pickListItem = "{value : '" + category.DrivingLicenseCategoryId.ToString() + "' , label : '" + category.DrivingLicenseCategoryName.Replace("'", "\\'") + "'}";
                result += (result == "" ? "" : ",") + pickListItem;
            }

            if (result != "")
                result = "[" + result + "]";

            hdnDrvLicCategories.Value = result;
        }

        private void PopulateRegions()
        {
            List<Region> listRegion = RegionUtil.GetRegions(CurrentUser);
            List<IDropDownItem> ddiRegions = new List<IDropDownItem>();
            foreach (Region region in listRegion)
            {
                ddiRegions.Add(region);
            }

            // Generates html for permanent regions drop down list
            string permRegionsHTML = ListItems.GetDropDownHtml(ddiRegions, null, "ddPermRegion", true, null, "ddPermRegion_Changed();", "style='width: 170px;'");
            this.divPermRegion.InnerHtml = permRegionsHTML;

            // Generates html for present regions drop down list
            string presRegionsHTML = ListItems.GetDropDownHtml(ddiRegions, null, "ddPresRegion", true, null, "ddPresRegion_Changed();", "style='width: 170px;'");
            this.divPresRegion.InnerHtml = presRegionsHTML;

            // Generates html for contact regions drop down list
            string contactRegionsHTML = ListItems.GetDropDownHtml(ddiRegions, null, "ddContactRegion", true, null, "ddContactRegion_Changed();", "style='width: 170px;'");
            this.divContactRegion.InnerHtml = contactRegionsHTML;

            // Generates html for permanent municipalities drop down list
            this.divPermMunicipality.InnerHtml = ListItems.GetDropDownHtml(null, null, "ddPermMunicipality", true, null, "ddPermMunicipality_Changed();", "style='width: 170px;'");

            // Generates html for present municipalities drop down list
            this.divPresMunicipility.InnerHtml = ListItems.GetDropDownHtml(null, null, "ddPresMunicipality", true, null, "ddPresMunicipality_Changed();", "style='width: 170px;'");

            // Generates html for contact municipalities drop down list
            this.divContactMunicipality.InnerHtml = ListItems.GetDropDownHtml(null, null, "ddContactMunicipality", true, null, "ddContactMunicipality_Changed();", "style='width: 170px;'");

            // Generates html for permanent cities drop down list
            this.divPermCity.InnerHtml = ListItems.GetDropDownHtml(null, null, "ddPermCity", true, null, "ddPermCity_Changed();", "style='width: 170px;' class='RequiredInputField' ");

            // Generates html for present cities drop down list
            this.divPresCity.InnerHtml = ListItems.GetDropDownHtml(null, null, "ddPresCity", true, null, "ddPresCity_Changed();", "style='width: 170px;' class='RequiredInputField' ");

            // Generates html for contact cities drop down list
            this.divContactCity.InnerHtml = ListItems.GetDropDownHtml(null, null, "ddContactCity", true, null, "ddContactCity_Changed();", "style='width: 170px;'");

            // Generates html for permanent districts drop down list
            this.divPermDistrict.InnerHtml = ListItems.GetDropDownHtml(null, null, "ddPermDistrict", true, null, "ddPermDistrict_Changed();", "style='width: 170px;'");

            // Generates html for present districts drop down list
            this.divPresDistrict.InnerHtml = ListItems.GetDropDownHtml(null, null, "ddPresDistrict", true, null, "ddPresDistrict_Changed();", "style='width: 170px;'");

            // Generates html for contact districts drop down list
            this.divContactDistrict.InnerHtml = ListItems.GetDropDownHtml(null, null, "ddContactDistrict", true, null, "ddContactDistrict_Changed();", "style='width: 170px;'");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(responseBack);
        }

        //Load Person details (ajax call)
        private void JSLoadPersonDetails()
        {
            bool isRegistred = IsRegistred();
            this.SetupPageUIClientControls(isRegistred);

            //string disabledControlsList = "<disabledControlsList>" + SetListDisabledControls() + "</disabledControlsList>";
            //string hiddenControlsList = "<hiddenControlsList>" + SetListHiddenControls() + "</hiddenControlsList>";

            PotencialApplicant potencialApplicant = new PotencialApplicant(CurrentUser);
            string identNumber = "";
            Person person;
            PersonStatus personStatus = null;

            string stat = "";
            string response = "";
            stat = AJAXTools.OK;
            string isRgistred = "NO";

            string comment = "";
            string lastApperianceDate = "";

            string divEducation = "";
            string divLanguage = "";

            List<ServiceType> availableServiceTypes = new List<ServiceType>();
            List<ServiceType> assignedServiceTypes = new List<ServiceType>();

            List<MilitaryTrainingCourse> availableMilitaryTrainingCources = new List<MilitaryTrainingCourse>();
            List<MilitaryTrainingCourse> assignedMilitaryTrainingCources = new List<MilitaryTrainingCourse>();

            try
            {
                if (!String.IsNullOrEmpty(Request.Params["PotencialApplicantId"]))
                {
                    int potencialApplicantId = 0;
                    Int32.TryParse(Request.Params["PotencialApplicantId"].ToString(), out potencialApplicantId);

                    potencialApplicant = PotencialApplicantUtil.GetPotencialApplicant(potencialApplicantId, CurrentUser);
                    if (potencialApplicant != null)
                    {
                        this.hdnPersonID.Value = potencialApplicant.PersonId.ToString();
                        this.hdnMilitaryDepartmentID.Value = potencialApplicant.MilitaryDepartmentId.ToString();
                        isRgistred = "OK";

                        comment = potencialApplicant.Comments;
                        if (potencialApplicant.LastAppearance.HasValue)
                        {
                            lastApperianceDate = CommonFunctions.FormatDate(potencialApplicant.LastAppearance.Value.ToString());
                        }
                    }
                    person = PersonUtil.GetPerson(potencialApplicant.PersonId, CurrentUser);
                    personStatus = PersonUtil.GetPersonStatusByPerson(person, CurrentUser);
                }
                else
                {
                    identNumber = Request.Form["IdentNumber"];
                    person = PersonUtil.GetPersonByIdentNumber(identNumber, CurrentUser);

                    int militaryDepartmentID = 0;
                    int.TryParse(this.hdnMilitaryDepartmentID.Value, out militaryDepartmentID);

                    if (person != null)
                    {   //We have brand New person
                        //Chek if this person is alredy registred as potencial applicant
                        potencialApplicant = PotencialApplicantUtil.GetPotencialApplicant(person.PersonId, militaryDepartmentID, CurrentUser);

                        if (potencialApplicant != null)
                        {
                            this.hdnPotencialApplicantId.Value = potencialApplicant.PotencialApplicantId.ToString();
                            isRgistred = "OK";

                            comment = potencialApplicant.Comments;
                            if (potencialApplicant.LastAppearance.HasValue)
                            {
                                lastApperianceDate = CommonFunctions.FormatDate(potencialApplicant.LastAppearance.Value);
                            }
                        }
                        personStatus = PersonUtil.GetPersonStatusByPerson(person, CurrentUser);
                    }
                }

                int potencialApplicantID = 0;
                if (potencialApplicant != null)
                    potencialApplicantID = potencialApplicant.PotencialApplicantId;

                if (potencialApplicant == null)
                {
                    availableServiceTypes = ServiceTypeUtil.GetAllServiceTypes(CurrentUser);
                    availableMilitaryTrainingCources = MilitaryTrainingCourseUtil.GetAllMilitaryTrainingCourses(CurrentUser);
                }
                else
                {
                    assignedServiceTypes = ServiceTypeUtil.GetAssignedServiceTypes(potencialApplicant.PotencialApplicantId, CurrentUser);
                    assignedMilitaryTrainingCources = MilitaryTrainingCourseUtil.GetAssignedMilitaryTrainingCourses(potencialApplicant.PotencialApplicantId, CurrentUser);

                    List<ServiceType> allServiceTypes = ServiceTypeUtil.GetAllServiceTypes(CurrentUser);
                    availableServiceTypes = (from st in allServiceTypes
                                             where !assignedServiceTypes.Exists(x => x.ServiceTypeID == st.ServiceTypeID)
                                             select st).ToList();

                    List<MilitaryTrainingCourse> allMilitaryTrainingCources = MilitaryTrainingCourseUtil.GetAllMilitaryTrainingCourses(CurrentUser);
                    availableMilitaryTrainingCources = (from mtc in allMilitaryTrainingCources
                                                        where !assignedMilitaryTrainingCources.Exists(x => x.MilitaryTrainingCourseID == mtc.MilitaryTrainingCourseID)
                                                        select mtc).ToList();
                }

                //Existing Person: Load him details
                if (person != null)
                {
                    this.PersonId = person.PersonId;

                    string drivingLicenseCategories = "";

                    foreach (DrivingLicenseCategory category in person.DrivingLicenseCategories)
                    {
                        drivingLicenseCategories += (drivingLicenseCategories == "" ? "" : ",") +
                            category.DrivingLicenseCategoryId.ToString();
                    }

                    response = @"
                        <person>
                            <personId>" + AJAXTools.EncodeForXML(person.PersonId.ToString()) + @"</personId>
                            <firstName>" + AJAXTools.EncodeForXML(person.FirstName) + @"</firstName>
                            <lastName>" + AJAXTools.EncodeForXML(person.LastName) + @"</lastName>
                            <identNumber>" + AJAXTools.EncodeForXML(person.IdentNumber) + @"</identNumber>
                            <genderId>" + AJAXTools.EncodeForXML(person.Gender == null ? ListItems.GetOptionChooseOne().Value : person.Gender.GenderId.ToString()) + @"</genderId>
                            <lastModified>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDateTime(person.LastModifiedDate)) + @"</lastModified>
                            <age>" + AJAXTools.EncodeForXML(CommonFunctions.GetAgeFromEGNbyDate(person.IdentNumber, DateTime.Now, CurrentUser).ToString()) + @"</age>
                            <ageMonthsPart>" + AJAXTools.EncodeForXML(CommonFunctions.GetAgeMonthsPartFromEGNbyDate(person.IdentNumber, DateTime.Now, CurrentUser).ToString()) + @"</ageMonthsPart>                                 
                            <drivingLicenseCategories>" + AJAXTools.EncodeForXML(drivingLicenseCategories) + @"</drivingLicenseCategories>
                            <permCityId>" + AJAXTools.EncodeForXML(person.PermCityId != null ? person.PermCityId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</permCityId>
                            <permPostCode>" + AJAXTools.EncodeForXML(person.PermCityId != null ? (person.PermDistrictId != null && person.PermDistrict.PostCode != "" ? person.PermDistrict.PostCode : person.PermCity.PostCode.ToString()) : "") + @"</permPostCode>
                            <permSecondPostCode>" + AJAXTools.EncodeForXML(String.IsNullOrEmpty(person.PermSecondPostCode) ? "" : person.PermSecondPostCode) + @"</permSecondPostCode>
                            <permRegionId>" + AJAXTools.EncodeForXML(person.PermCityId != null ? person.PermCity.RegionId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</permRegionId>
                            <permMunicipalityId>" + AJAXTools.EncodeForXML(person.PermCityId != null ? person.PermCity.MunicipalityId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</permMunicipalityId>
                            <permDistrictId>" + AJAXTools.EncodeForXML(person.PermDistrictId != null ? person.PermDistrict.DistrictId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</permDistrictId>
                            <permAddress>" + AJAXTools.EncodeForXML(person.PermAddress) + @"</permAddress>
                            <presCityId>" + AJAXTools.EncodeForXML(person.PresCityId != null ? person.PresCityId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</presCityId>
                            <presPostCode>" + AJAXTools.EncodeForXML(person.PresCityId != null ? (person.PresDistrictId != null && person.PresDistrict.PostCode != "" ? person.PresDistrict.PostCode : person.PresCity.PostCode.ToString()) : "") + @"</presPostCode>
                            <presSecondPostCode>" + AJAXTools.EncodeForXML(String.IsNullOrEmpty(person.PresSecondPostCode) ? "" : person.PresSecondPostCode) + @"</presSecondPostCode>
                            <presRegionId>" + AJAXTools.EncodeForXML(person.PresCityId != null ? person.PresCity.RegionId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</presRegionId>
                            <presMunicipalityId>" + AJAXTools.EncodeForXML(person.PresCityId != null ? person.PresCity.MunicipalityId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</presMunicipalityId>                            
                            <presDistrictId>" + AJAXTools.EncodeForXML(person.PresDistrictId != null ? person.PresDistrict.DistrictId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</presDistrictId>
                            <presAddress>" + AJAXTools.EncodeForXML(person.PresAddress) + @"</presAddress>
                            <contactCityId>" + AJAXTools.EncodeForXML(person.ContactAddress.CityId != null ? person.ContactAddress.CityId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</contactCityId>
                            <contactPostCode>" + AJAXTools.EncodeForXML(person.ContactAddress.CityId != null ? (person.ContactAddress.DistrictId != null && person.ContactAddress.District.PostCode != "" ? person.ContactAddress.District.PostCode : person.ContactAddress.City.PostCode.ToString()) : "") + @"</contactPostCode>
                            <contactSecondPostCode>" + AJAXTools.EncodeForXML(String.IsNullOrEmpty(person.ContactAddress.PostCode) ? "" : person.ContactAddress.PostCode) + @"</contactSecondPostCode>
                            <contactRegionId>" + AJAXTools.EncodeForXML(person.ContactAddress.CityId != null ? person.ContactAddress.City.RegionId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</contactRegionId>
                            <contactMunicipalityId>" + AJAXTools.EncodeForXML(person.ContactAddress.CityId != null ? person.ContactAddress.City.MunicipalityId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</contactMunicipalityId>                            
                            <contactDistrictId>" + AJAXTools.EncodeForXML(person.ContactAddress.DistrictId != null ? person.ContactAddress.District.DistrictId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</contactDistrictId>
                            <contactAddress>" + AJAXTools.EncodeForXML(person.ContactAddress.AddressText) + @"</contactAddress>
                            <IDCardNumber>" + AJAXTools.EncodeForXML(person.IDCardNumber) + @"</IDCardNumber>
                            <IDCardIssuedBy>" + AJAXTools.EncodeForXML(person.IDCardIssuedBy) + @"</IDCardIssuedBy>
                            <IDCardIssueDate>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(person.IDCardIssueDate)) + @"</IDCardIssueDate>
                            <homePhone>" + AJAXTools.EncodeForXML(person.HomePhone != null ? person.HomePhone.ToString() : "") + @"</homePhone>
                            <mobilePhone>" + AJAXTools.EncodeForXML(person.MobilePhone) + @"</mobilePhone>
                            <email>" + AJAXTools.EncodeForXML(person.Email) + @"</email>
                            <hasMilitarySrv>" + AJAXTools.EncodeForXML(person.HasMilitaryService != null ? person.HasMilitaryService.ToString() : "") + @"</hasMilitarySrv>
                            <militaryTraining>" + AJAXTools.EncodeForXML(person.MilitaryTraining != null ? person.MilitaryTraining.ToString() : "") + @"</militaryTraining>                            
                            <isRegistred>" + AJAXTools.EncodeForXML(isRgistred) + @"</isRegistred>
                            <comment>" + AJAXTools.EncodeForXML(comment) + @"</comment>
                            <lastApperianceDate>" + AJAXTools.EncodeForXML(lastApperianceDate) + @"</lastApperianceDate>
                            <birthCountryId>" + AJAXTools.EncodeForXML(person.BirthCountry != null ? person.BirthCountry.CountryId : "") + @"</birthCountryId>
                            <birthCityId>" + AJAXTools.EncodeForXML(person.BirthCityId != null ? person.BirthCityId.ToString() : "") + @"</birthCityId>
                            <birthCityIfAbroad>" + AJAXTools.EncodeForXML(person.BirthCityIfAbroad != null ? person.BirthCityIfAbroad : "") + @"</birthCityIfAbroad>
                        ";
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

                    if (person.PermCityId != null)
                    {
                        List<Municipality> municipalities = MunicipalityUtil.GetMunicipalities(person.PermCity.RegionId, CurrentUser);
                        List<City> cities = CityUtil.GetCities(person.PermCity.MunicipalityId, CurrentUser);
                        List<District> districts = person.PermCity.Districts;

                        foreach (Municipality municipality in municipalities)
                        {
                            response += "<p_m>" +
                                        "<id>" + municipality.MunicipalityId.ToString() + "</id>" +
                                        "<name>" + AJAXTools.EncodeForXML(municipality.MunicipalityName) + "</name>" +
                                        "</p_m>";
                        }

                        foreach (City city in cities)
                        {
                            response += "<p_c>" +
                                        "<id>" + city.CityId.ToString() + "</id>" +
                                        "<name>" + AJAXTools.EncodeForXML(city.CityName) + "</name>" +
                                        "</p_c>";
                        }

                        response += "<p_d>" +
                                    "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                                    "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                                    "</p_d>";

                        foreach (District district in districts)
                        {
                            response += "<p_d>" +
                                        "<id>" + district.DistrictId.ToString() + "</id>" +
                                        "<name>" + AJAXTools.EncodeForXML(district.DistrictName) + "</name>" +
                                        "</p_d>";
                        }
                    }

                    if (person.PresCityId != null)
                    {
                        List<Municipality> municipalities = MunicipalityUtil.GetMunicipalities(person.PresCity.RegionId, CurrentUser);
                        List<City> cities = CityUtil.GetCities(person.PresCity.MunicipalityId, CurrentUser);
                        List<District> districts = person.PresCity.Districts;

                        foreach (Municipality municipality in municipalities)
                        {
                            response += "<c_m>" +
                                        "<id>" + municipality.MunicipalityId.ToString() + "</id>" +
                                        "<name>" + AJAXTools.EncodeForXML(municipality.MunicipalityName) + "</name>" +
                                        "</c_m>";
                        }

                        foreach (City city in cities)
                        {
                            response += "<c_c>" +
                                        "<id>" + city.CityId.ToString() + "</id>" +
                                        "<name>" + AJAXTools.EncodeForXML(city.CityName) + "</name>" +
                                        "</c_c>";
                        }

                        response += "<c_d>" +
                                    "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                                    "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                                    "</c_d>";

                        foreach (District district in districts)
                        {
                            response += "<c_d>" +
                                        "<id>" + district.DistrictId.ToString() + "</id>" +
                                        "<name>" + AJAXTools.EncodeForXML(district.DistrictName) + "</name>" +
                                        "</c_d>";
                        }
                    }

                    if (person.ContactAddress.CityId != null)
                    {
                        List<Municipality> municipalities = MunicipalityUtil.GetMunicipalities(person.ContactAddress.City.RegionId, CurrentUser);
                        List<City> cities = CityUtil.GetCities(person.ContactAddress.City.MunicipalityId, CurrentUser);
                        List<District> districts = person.ContactAddress.City.Districts;

                        foreach (Municipality municipality in municipalities)
                        {
                            response += "<con_m>" +
                                        "<id>" + municipality.MunicipalityId.ToString() + "</id>" +
                                        "<name>" + AJAXTools.EncodeForXML(municipality.MunicipalityName) + "</name>" +
                                        "</con_m>";
                        }

                        foreach (City city in cities)
                        {
                            response += "<con_c>" +
                                        "<id>" + city.CityId.ToString() + "</id>" +
                                        "<name>" + AJAXTools.EncodeForXML(city.CityName) + "</name>" +
                                        "</con_c>";
                        }

                        response += "<con_d>" +
                                    "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                                    "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                                    "</con_d>";

                        foreach (District district in districts)
                        {
                            response += "<con_d>" +
                                        "<id>" + district.DistrictId.ToString() + "</id>" +
                                        "<name>" + AJAXTools.EncodeForXML(district.DistrictName) + "</name>" +
                                        "</con_d>";
                        }
                    }
                }
                else //New person
                {
                    personStatus = new PersonStatus();

                    response = @"
                        <person>
                            <personId>0</personId>
                            <firstName></firstName>
                            <lastName></lastName>
                            <identNumber>" + AJAXTools.EncodeForXML(identNumber) + @"</identNumber>
                            <genderId>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Value) + @"</genderId>
                            <lastModified></lastModified>
                            <age>" + AJAXTools.EncodeForXML(CommonFunctions.GetAgeFromEGNbyDate(identNumber, DateTime.Now, CurrentUser).ToString()) + @"</age>
                            <ageMonthsPart>" + AJAXTools.EncodeForXML(CommonFunctions.GetAgeMonthsPartFromEGNbyDate(identNumber, DateTime.Now, CurrentUser).ToString()) + @"</ageMonthsPart>     
                            <drivingLicenseCategories></drivingLicenseCategories>
                            <permCityId>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Value) + @"</permCityId>
                            <permPostCode></permPostCode>
                            <permSecondPostCode></permSecondPostCode>
                            <permRegionId>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Value) + @"</permRegionId>
                            <permMunicipalityId>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Value) + @"</permMunicipalityId>
                            <permDistrictId>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Value) + @"</permDistrictId>
                            <permAddress></permAddress>
                            <presCityId>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Value) + @"</presCityId>
                            <presPostCode></presPostCode>
                            <presSecondPostCode></presSecondPostCode>
                            <presRegionId>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Value) + @"</presRegionId>
                            <presMunicipalityId>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Value) + @"</presMunicipalityId>     
                            <presDistrictId>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Value) + @"</presDistrictId>                       
                            <presAddress></presAddress>
                            <contactCityId>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Value) + @"</contactCityId>
                            <contactPostCode></contactPostCode>
                            <contactSecondPostCode></contactSecondPostCode>
                            <contactRegionId>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Value) + @"</contactRegionId>
                            <contactMunicipalityId>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Value) + @"</contactMunicipalityId>
                            <contactDistrictId>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Value) + @"</contactDistrictId>
                            <contactAddress></contactAddress>
                            <IDCardNumber></IDCardNumber>
                            <IDCardIssuedBy></IDCardIssuedBy>
                            <IDCardIssueDate></IDCardIssueDate>
                            <homePhone></homePhone>
                            <mobilePhone></mobilePhone>
                            <email></email>
                            <hasMilitarySrv></hasMilitarySrv> 
                            <militaryTraining></militaryTraining>
                            <isRegistred>" + AJAXTools.EncodeForXML(isRgistred) + @"</isRegistred>
                            <comment>" + AJAXTools.EncodeForXML(comment) + @"</comment>
                            <lastApperianceDate>" + AJAXTools.EncodeForXML(lastApperianceDate) + @"</lastApperianceDate>
                            <birthCountryId></birthCountryId>
                            <birthCityId></birthCityId>
                            <birthCityIfAbroad></birthCityIfAbroad>
                        ";
                    response += "<PersonStatus>";
                    response += "<PersonStatus_Status>" + AJAXTools.EncodeForXML(personStatus.Status) + "</PersonStatus_Status>";
                    response += "<PersonStatus_Details></PersonStatus_Details>";
                    response += "</PersonStatus>";
                }
            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }

            if (IsRegistred())
            {
                divEducation = GenerateApplicantEducationsTable(string.Empty, false);
                divLanguage = GenerateApplicantLanguagesTable(string.Empty, false);
            }

            divEducation = AJAXTools.EncodeForXML(divEducation);

            divLanguage = AJAXTools.EncodeForXML(divLanguage);

            response += "<availableServiceTypes>";
            foreach (ServiceType sp in availableServiceTypes)
            {
                response += @"<el>";
                response += "<id>" + sp.ServiceTypeID + "</id>";
                response += "<name>" + AJAXTools.EncodeForXML(sp.ServiceTypeName) + "</name>";
                response += "</el>";
            }
            response += "</availableServiceTypes>";

            response += "<assignedServiceTypes>";
            foreach (ServiceType sp in assignedServiceTypes)
            {
                response += "<el>";
                response += "<id>" + sp.ServiceTypeID + "</id>";
                response += "<name>" + AJAXTools.EncodeForXML(sp.ServiceTypeName) + "</name>";
                response += "</el>";
            }
            response += "</assignedServiceTypes>";

            response += "<availableMilitaryTrainingCources>";
            foreach (MilitaryTrainingCourse mtc in availableMilitaryTrainingCources)
            {
                response += "<el>";
                response += "<id>" + mtc.MilitaryTrainingCourseID + "</id>";
                response += "<name>" + AJAXTools.EncodeForXML(mtc.MilitaryTrainingCourseName) + "</name>";
                response += "</el>";
            }
            response += "</availableMilitaryTrainingCources>";

            response += "<assignedMilitaryTrainingCources>";
            foreach (MilitaryTrainingCourse mtc in assignedMilitaryTrainingCources)
            {
                response += "<el>";
                response += "<id>" + mtc.MilitaryTrainingCourseID + "</id>";
                response += "<name>" + AJAXTools.EncodeForXML(mtc.MilitaryTrainingCourseName) + "</name>";
                response += "</el>";
            }
            response += "</assignedMilitaryTrainingCources>";
            response += "</person>";

            response += "<HtmlEducation>" + divEducation + "</HtmlEducation>";

            response += "<HtmlLanguage>" + divLanguage + "</HtmlLanguage>";

            response += "<listDisabledControls>" + SetListDisabledControls() + @"</listDisabledControls>
                         <listHiddenControls>" + SetListHiddenControls() + @"</listHiddenControls>>
                     ";

            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        //Save Person details (ajax call)
        private void JSSavePersonDetails()
        {
            string firstName = Request.Form["FirstName"];
            string lastName = Request.Form["LastName"];
            string identNumber = Request.Form["IdentNumber"];

            int? personId = null;

            if (!String.IsNullOrEmpty(Request.Form["PersonID"]))
            {
                personId = int.Parse(Request.Form["PersonID"]);
            }

            int? genderId = null;
            if (!String.IsNullOrEmpty(Request.Form["GenderId"]) &&
                Request.Form["GenderId"] != ListItems.GetOptionChooseOne().Value)
            {
                genderId = int.Parse(Request.Form["GenderId"]);
            }

            string drivingLicenseCategories = Request.Form["DrivingLicenseCategories"];

            int? permCityId = null;
            if (!String.IsNullOrEmpty(Request.Form["PermCityID"]) &&
                Request.Form["PermCityID"] != ListItems.GetOptionChooseOne().Value)
            {
                permCityId = int.Parse(Request.Form["PermCityID"]);
            }

            int? permDistrictId = null;
            if (!String.IsNullOrEmpty(Request.Form["PermDistrictID"]) &&
                Request.Form["PermDistrictID"] != ListItems.GetOptionChooseOne().Value)
            {
                permDistrictId = int.Parse(Request.Form["PermDistrictID"]);
            }

            string permSecondPostCode = Request.Form["PermSecondPostCode"];
            string permAddress = Request.Form["PermAddress"];

            int? presCityId = null;
            if (!String.IsNullOrEmpty(Request.Form["PresCityID"]) &&
                Request.Form["PresCityID"] != ListItems.GetOptionChooseOne().Value)
            {
                presCityId = int.Parse(Request.Form["PresCityID"]);
            }

            int? presDistrictId = null;
            if (!String.IsNullOrEmpty(Request.Form["PresDistrictID"]) &&
                Request.Form["PresDistrictID"] != ListItems.GetOptionChooseOne().Value)
            {
                presDistrictId = int.Parse(Request.Form["PresDistrictID"]);
            }

            string presSecondPostCode = Request.Form["PresSecondPostCode"];
            string presAddress = Request.Form["PresAddress"];

            int? contactCityId = null;
            if (!String.IsNullOrEmpty(Request.Form["ContactCityID"]) &&
                Request.Form["ContactCityID"] != ListItems.GetOptionChooseOne().Value)
            {
                contactCityId = int.Parse(Request.Form["ContactCityID"]);
            }

            int? contactDistrictId = null;
            if (!String.IsNullOrEmpty(Request.Form["ContactDistrictID"]) &&
                Request.Form["ContactDistrictID"] != ListItems.GetOptionChooseOne().Value)
            {
                contactDistrictId = int.Parse(Request.Form["ContactDistrictID"]);
            }

            string contactSecondPostCode = Request.Form["ContactSecondPostCode"];
            string contactAddress = Request.Form["ContactAddress"];

            string IDCardNumber = Request.Form["IDCardNumber"];
            string IDCardIssuedBy = Request.Form["IDCardIssuedBy"];
            string IDCardIssueDate = Request.Form["IDCardIssueDate"];

            long? homePhone = null;
            if (!String.IsNullOrEmpty(Request.Form["HomePhone"]))
            {
                homePhone = long.Parse(Request.Form["HomePhone"]);
            }

            string mobilePhone = Request.Form["MobilePhone"];
            string email = Request.Form["Email"];

            int? hasMilitarySrv = null;
            if (!String.IsNullOrEmpty(Request.Form["HasMilitarySrv"]) &&
                Request.Form["HasMilitarySrv"] != ListItems.GetOptionChooseOne().Value)
            {
                hasMilitarySrv = int.Parse(Request.Form["HasMilitarySrv"]);
            }

            int? militaryTraining = null;
            if (!String.IsNullOrEmpty(Request.Form["MilitaryTraining"]) &&
                Request.Form["MilitaryTraining"] != ListItems.GetOptionChooseOne().Value)
            {
                militaryTraining = int.Parse(Request.Form["MilitaryTraining"]);
            }

            string birthCountryId = null;
            if (!String.IsNullOrEmpty(Request.Form["BirthCountryId"]))
            {
                birthCountryId = Request.Form["BirthCountryId"];
            }

            int? birthCityId = null;
            if (!String.IsNullOrEmpty(Request.Form["BirthCityId"]))
            {
                birthCityId = int.Parse(Request.Form["BirthCityId"]);
            }

            string birthCityIfAbroad = Request.Form["BirthCityIfAbroad"];


            Person person = new Person(CurrentUser);

            person.PersonId = personId.HasValue ? personId.Value : 0;
            person.IdentNumber = identNumber;
            person.FirstName = firstName;
            person.LastName = lastName;
            person.Gender = genderId.HasValue ? GenderUtil.GetGender(CurrentUser, genderId.Value) : null;
            person.DrivingLicenseCategories = new List<DrivingLicenseCategory>();

            if (!String.IsNullOrEmpty(drivingLicenseCategories))
            {
                List<DrivingLicenseCategory> listDrivingCategories = DrivingLicenseCategoryUtil.GetDrivingLicenseCategoryByCategoryId(drivingLicenseCategories, CurrentUser);
                foreach (DrivingLicenseCategory drivingLicenseCategory in listDrivingCategories)
                {
                    person.DrivingLicenseCategories.Add(drivingLicenseCategory);
                }
            }

            person.PermCityId = permCityId;
            person.PermDistrictId = permDistrictId;
            person.PermSecondPostCode = permSecondPostCode;
            person.PermAddress = permAddress;
            person.PresCityId = presCityId;
            person.PresDistrictId = presDistrictId;
            person.PresSecondPostCode = presSecondPostCode;
            person.PresAddress = presAddress;
            person.ContactAddress = new Address(CurrentUser)
            {
                AddressId = AddressUtil.GetAddressIdByPersonIdAndAddressType(person.PersonId, "ADR_CONTACT", CurrentUser),
                CityId = contactCityId,
                DistrictId = contactDistrictId,
                PostCode = contactSecondPostCode,
                AddressText = contactAddress
            };
            person.IDCardNumber = IDCardNumber;
            person.IDCardIssuedBy = IDCardIssuedBy;
            person.IDCardIssueDate = (!String.IsNullOrEmpty(IDCardIssueDate) ? CommonFunctions.ParseDate(IDCardIssueDate) : (DateTime?)null);
            person.HomePhone = homePhone;
            person.MobilePhone = mobilePhone;
            person.Email = email;
            person.HasMilitaryService = hasMilitarySrv;
            person.MilitaryTraining = militaryTraining;
            person.BirthCountry = (!String.IsNullOrEmpty(birthCountryId) ? CountryUtil.GetCountry(birthCountryId, CurrentUser) : null);
            person.BirthCityId = birthCityId;
            person.BirthCityIfAbroad = birthCityIfAbroad;

            string stat = "";
            string response = "";

            try
            {
                //Track the changes into the Audit Trail 
                Change change = new Change(CurrentUser, "APPL_PotencialApplicants");

                PotencialApplicant potencialApplicant = new PotencialApplicant(CurrentUser);

                if (!string.IsNullOrEmpty(hdnPotencialApplicantId.Value))
                {
                    int potencialApplicantId = 0;
                    int.TryParse(hdnPotencialApplicantId.Value, out potencialApplicantId);

                    potencialApplicant = PotencialApplicantUtil.GetPotencialApplicant(potencialApplicantId, CurrentUser);
                }
                else
                {
                    //int potencialApplicantId = 0;
                    //int.TryParse(hdnPotencialApplicantId.Value, out potencialApplicantId);

                    int militaryDepartmentId = 0;
                    int.TryParse(hdnMilitaryDepartmentID.Value, out militaryDepartmentId);

                    //   potencialApplicant.PotencialApplicantId = potencialApplicantId;
                    potencialApplicant.MilitaryDepartmentId = militaryDepartmentId;
                    potencialApplicant.PersonId = person.PersonId;

                    //In case with refresh page F5 - we loss data in hdnPotencialApplicantId
                    if (PotencialApplicantUtil.IsAlreadyRegistered(person.PersonId, militaryDepartmentId, CurrentUser))
                    {
                        potencialApplicant = PotencialApplicantUtil.GetPotencialApplicant(person.PersonId, militaryDepartmentId, CurrentUser);
                    }
                }

                //Set properties in potencialApplicant object
                if (!string.IsNullOrEmpty(Request.Form["Comment"]))
                {
                    string Comment = Request.Form["Comment"];
                    potencialApplicant.Comments = Comment;
                }

                if (!string.IsNullOrEmpty(Request.Form["LastApperianceDate"]))
                {
                    DateTime LastApperianceDate = CommonFunctions.ParseDate(Request.Form["LastApperianceDate"]).Value;
                    potencialApplicant.LastAppearance = LastApperianceDate;
                }

                potencialApplicant.ServiceTypes = new List<ServiceType>();
                int serviceTypesCnt = int.Parse(Request.Form["STCnt"]);

                for (int i = 1; i <= serviceTypesCnt; i++)
                {
                    int serviceTypeID = int.Parse(Request.Form["STId_" + i.ToString()]);
                    string serviceTypeName = Request.Form["STDisplayText_" + i.ToString()];

                    ServiceType st = new ServiceType(CurrentUser);

                    st.ServiceTypeID = serviceTypeID;
                    st.ServiceTypeName = serviceTypeName;

                    potencialApplicant.ServiceTypes.Add(st);
                }

                potencialApplicant.MilitaryTrainingCourses = new List<MilitaryTrainingCourse>();
                int militaryTrainingCoursesCnt = int.Parse(Request.Form["MTCCnt"]);

                for (int i = 1; i <= militaryTrainingCoursesCnt; i++)
                {
                    int militaryTrainingCourseID = int.Parse(Request.Form["MTCId_" + i.ToString()]);
                    string militaryTrainingCourseName = Request.Form["MTCDisplayText_" + i.ToString()];

                    MilitaryTrainingCourse mtc = new MilitaryTrainingCourse(CurrentUser);

                    mtc.MilitaryTrainingCourseID = militaryTrainingCourseID;
                    mtc.MilitaryTrainingCourseName = militaryTrainingCourseName;

                    potencialApplicant.MilitaryTrainingCourses.Add(mtc);
                }

                //Save the changes in table Person
                if (person.PersonId == 0)
                {
                    PersonUtil.SavePerson_WhenAddingNewPotentialApplicant(person, "ADM_PersonDetails_Add", CurrentUser, change);
                    //Set new PersonId
                    potencialApplicant.PersonId = person.PersonId;
                }
                else
                {
                    PersonUtil.SavePerson_WhenAddingNewPotentialApplicant(person, "ADM_PersonDetails_Edit", CurrentUser, change);
                }

                //Save the changes in table PotencialApplicant
                PotencialApplicantUtil.SavePotencialApplicant(potencialApplicant, CurrentUser, change);

                //After succes Save we set new ListDisabled/Hidden Controls

                this.SetupPageUIClientControls(true);

                //Use this for page refresh(F5)
                SetLocationHash("AddPotencialApplicant_PersonDetails.aspx?PotencialApplicantId=" + potencialApplicant.PotencialApplicantId.ToString());


                //Now chek for server control  LASTAPPERIANCE
                string lastApperiance = "";
                if (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_LASTAPPERIANCE") == UIAccessLevel.Hidden
                    || this.GetUIItemAccessLevel("APPL_POTENCIALAPPL") == UIAccessLevel.Hidden)
                {
                    lastApperiance = "Hidden";
                }

                if (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_LASTAPPERIANCE") == UIAccessLevel.Disabled
                 || this.GetUIItemAccessLevel("APPL_POTENCIALAPPL") == UIAccessLevel.Disabled)
                {
                    lastApperiance = "Disabled";
                }

                //Write into the Audit Trail
                change.WriteLog();

                stat = AJAXTools.OK;
                response = @"<response>OK</response>
                             <personId>" + AJAXTools.EncodeForXML(person.PersonId.ToString()) + @"</personId>
                             <potencialApplicantId>" + potencialApplicant.PotencialApplicantId.ToString() + "</potencialApplicantId>";

                response += "<isRegistred>OK</isRegistred>";


                response += "<listDisabledControls>" + SetListDisabledControls() + @"</listDisabledControls>
                             <listHiddenControls>" + SetListHiddenControls() + @"</listHiddenControls>
                             <lastApperiance>" + lastApperiance + @"</lastApperiance>>
                     ";

                //Refresh Education and Language UI if need

                string divEducation = "";
                string divLanguage = "";

                if (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU") != UIAccessLevel.Hidden
                   || this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU_EDU") != UIAccessLevel.Hidden)
                {
                    divEducation = GenerateApplicantEducationsTable(string.Empty, false);
                    divEducation = AJAXTools.EncodeForXML(divEducation);
                    response += "<HtmlEducation>" + divEducation + "</HtmlEducation>";
                }
                else
                {
                    response += "<HtmlEducation>" + divEducation + "</HtmlEducation>";
                }

                if (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU") != UIAccessLevel.Hidden
                   || this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU_LANG") != UIAccessLevel.Hidden)
                {
                    divLanguage = GenerateApplicantLanguagesTable(string.Empty, false);
                    divLanguage = AJAXTools.EncodeForXML(divLanguage);
                    response += "<HtmlLanguage>" + divLanguage + "</HtmlLanguage>";
                }
                else
                {
                    response += "<HtmlLanguage>" + divLanguage + "</HtmlLanguage>";
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

        //Fill Drop Down List for Municipality, City and PostCode for current RegionId
        private void JSGetListMunicipality(int regionId)
        {
            string response = ""; //Hold XML for Municipality
            string responceCity = ""; //Hold XML for City and PostCode
            List<Municipality> listMunicipality = new List<Municipality>();

            listMunicipality = MunicipalityUtil.GetMunicipalities(regionId, CurrentUser);
            for (int i = 0; i <= listMunicipality.Count - 1; i++)
            {
                response += "<municipality>";
                response += "<municipalityId>";
                response += listMunicipality[i].MunicipalityId;
                response += "</municipalityId>";
                response += "<municipalyName>";
                response += listMunicipality[i].MunicipalityName;
                response += "</municipalyName>";
                response += "</municipality>";
            }

            response = response + responceCity;
            AJAX a = new AJAX(response, this.Response);
            a.Write();
            Response.End();
        }

        //Create XML for City and PostCode and Return Responce
        private void JSGetListCity(int municipalityId)
        {
            string response = "";
            string responsePostCode = "";
            List<City> listCity = new List<City>();

            listCity = CityUtil.GetCities(municipalityId, CurrentUser);
            for (int i = 0; i <= listCity.Count - 1; i++)
            {
                if (i == 0)
                {
                    responsePostCode = "<postCode>" + listCity[i].PostCode + "</postCode>";
                }
                response += "<city>";
                response += "<cityId>";
                response += listCity[i].CityId;
                response += "</cityId>";
                response += "<cityName>";
                response += listCity[i].CityName;
                response += "</cityName>";
                response += "</city>";
            }

            response += responsePostCode;
            AJAX a = new AJAX(response, this.Response);
            a.Write();
            Response.End();
        }

        //Create XML for PostCode and Return Responce
        private void JSGetPostCodeByCity(int cityId)
        {
            string response = "";
            int? postCode = null;

            City city = CityUtil.GetCity(cityId, CurrentUser);
            if (city != null)
            {
                postCode = city.PostCode;
            }
            response = "<postCode>" + postCode.ToString() + "</postCode>";

            //Bind District List
            List<District> listDistrict = new List<District>();

            listDistrict = DistrictUtil.GetDistricts(cityId, CurrentUser);
            for (int i = 0; i <= listDistrict.Count - 1; i++)
            {
                response += "<district>";
                response += "<districtId>";
                response += listDistrict[i].DistrictId;
                response += "</districtId>";
                response += "<districtName>";
                response += listDistrict[i].DistrictName;
                response += "</districtName>";
                response += "</district>";
            }

            AJAX a = new AJAX(response, this.Response);
            a.Write();
            Response.End();
        }

        //Create XML for PostCode and Return Responce
        private void JSGetPostCodeByDistrict(int districtId, int cityId)
        {
            string response = "";
            string postCode = "";

            District district = DistrictUtil.GetDistrict(districtId, CurrentUser);
            if (district != null)
            {
                if (district.PostCode != "")
                {
                    postCode = district.PostCode;
                }
            }

            if (postCode == "")
            {
                City city = CityUtil.GetCity(cityId, CurrentUser);
                postCode = city.PostCode.ToString();
            }

            response = "<postCode>" + postCode.ToString() + "</postCode>";

            AJAX a = new AJAX(response, this.Response);
            a.Write();
            Response.End();
        }

        //Create XML for using PostCode and Return Responce
        private void JSGetCityForPostCode(int postCode, int cityID)
        {
            City city = null;

            if (cityID > 0)
            {
                city = CityUtil.GetCity(cityID, CurrentUser);
            }
            else
            {
                city = CityUtil.GetCityByPostCode(postCode, CurrentUser);
            }
            string response = "";
            if (city != null)
            {
                response += "<statusResult>OK</statusResult>";
                response += "<selectedRegionId>" + city.RegionId + "</selectedRegionId>";
                response += "<selectedMunicipalityID>" + city.MunicipalityId + "</selectedMunicipalityID>";
                response += "<selectedCityId>" + city.CityId + "</selectedCityId>";
                response += "<selectedDistrictId>" + (ListItems.GetOptionChooseOne().Value) + "</selectedDistrictId>";

                //Bind Municipality List
                List<Municipality> listMunicipality = new List<Municipality>();

                listMunicipality = MunicipalityUtil.GetMunicipalities(city.RegionId, CurrentUser);
                for (int i = 0; i <= listMunicipality.Count - 1; i++)
                {
                    response += "<municipality>";
                    response += "<municipalityId>";
                    response += listMunicipality[i].MunicipalityId;
                    response += "</municipalityId>";
                    response += "<municipalyName>";
                    response += listMunicipality[i].MunicipalityName;
                    response += "</municipalyName>";
                    response += "</municipality>";
                }

                //Bind City List
                List<City> listCity = new List<City>();

                listCity = CityUtil.GetCities(city.MunicipalityId, CurrentUser);
                for (int i = 0; i <= listCity.Count - 1; i++)
                {
                    response += "<city>";
                    response += "<cityId>";
                    response += listCity[i].CityId;
                    response += "</cityId>";
                    response += "<cityName>";
                    response += listCity[i].CityName;
                    response += "</cityName>";
                    response += "</city>";
                }

                //Bind District List
                List<District> listDistrict = new List<District>();

                listDistrict = DistrictUtil.GetDistricts(city.CityId, CurrentUser);
                for (int i = 0; i <= listDistrict.Count - 1; i++)
                {
                    response += "<district>";
                    response += "<districtId>";
                    response += listDistrict[i].DistrictId;
                    response += "</districtId>";
                    response += "<districtName>";
                    response += listDistrict[i].DistrictName;
                    response += "</districtName>";
                    response += "</district>";
                }
            }
            else
            {
                response += "<statusResult>NO</statusResult>";
            }

            AJAX a = new AJAX(response, this.Response);
            a.Write();
            Response.End();
        }

        //Generate applicant education table
        private string GenerateApplicantEducationsTable(string message, bool existError)
        {
            bool IsEducationHidden;
            bool IsSubjectHidden;
            bool IsYearHidden;
            bool IsLearningMethodHidden;

            if (IsRegistred())
            {
                IsEducationHidden = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU_EDU_EDU") == UIAccessLevel.Hidden;
                IsSubjectHidden = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU_EDU_SUBJ") == UIAccessLevel.Hidden;
                IsYearHidden = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU_EDU_YEAR") == UIAccessLevel.Hidden;
                IsLearningMethodHidden = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU_EDU_LEARNINGMETHOD") == UIAccessLevel.Hidden;
            }
            else
            {
                IsEducationHidden = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_EDU_EDU_EDU") == UIAccessLevel.Hidden;
                IsSubjectHidden = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_EDU_EDU_SUBJ") == UIAccessLevel.Hidden;
                IsYearHidden = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_EDU_EDU_YEAR") == UIAccessLevel.Hidden;
                IsLearningMethodHidden = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_EDU_EDU_LEARNINGMETHOD") == UIAccessLevel.Hidden;
            }

            if (IsEducationHidden &&
                IsSubjectHidden &&
                IsYearHidden &&
                IsLearningMethodHidden
                )
            {
                return "";
            }

            //Generate Education table
            string html = "";
            List<PersonCivilEducation> civilEducations = PersonCivilEducationUtil.GetAllPersonCivilEducationsByPersonID(this.PersonId, CurrentUser);

            html += "<div id='divApplEduTable'>";
            html += "<table style='width: 100%;'>";

            html += "<tr><td align='left'><span class='SmallHeaderText'>Образование</span></td></tr>";

            html += "<tr><td align='center'>";

            //No data found
            if (civilEducations.Count == 0)
            {
                html += "<span>Няма въведена информация</span>";
                html += "</td></tr>";
                html += "<tr><td>";
                bool screenDisabled;

                if (IsRegistred())
                {
                    screenDisabled = ((this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU") == UIAccessLevel.Disabled) ||
                    (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU_EDU") == UIAccessLevel.Disabled) ||
                    (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL") == UIAccessLevel.Disabled));
                }
                else
                {
                    screenDisabled = ((this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_EDU") == UIAccessLevel.Disabled) ||
                   (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_EDU_EDU") == UIAccessLevel.Disabled) ||
                   (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL") == UIAccessLevel.Disabled));
                }

                if (!screenDisabled && this.PersonId > 0)
                {
                    html += "<div id='btnAddNewApplicantEducation' style='display: inline;' onclick='ShowApplicantEducationLightBox(0);' class='Button'><i></i><div id='btnAddNewApplicantEducationText' style='width:90px;'>Добави ново</div><b></b></div><br />";
                }

                html += "</td></tr>";
                html += "</table></div>";
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

                string POTAPPPL = "APPL_POTENCIALAPPL_ADD_POTENCIALAPPL";
                string EDU = "APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_EDU";
                string EDUEDU = "APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_EDU_EDU";

                if (IsRegistred())
                {
                    POTAPPPL = "APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL";
                    EDU = "APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU";
                    EDUEDU = "APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU_EDU";
                }

                bool HtmlEdit = (GetUIItemAccessLevel(EDU) == UIAccessLevel.Enabled &&
                        GetUIItemAccessLevel(EDUEDU) == UIAccessLevel.Enabled &&
                        GetUIItemAccessLevel(POTAPPPL) == UIAccessLevel.Enabled &&
                        CanCurrentUserAccessThisMilDepartment);

                //Iterate through all items and add them into the grid
                foreach (PersonCivilEducation civilEducation in civilEducations)
                {
                    string cellStyle = "vertical-align: top;";

                    string deleteHTML = "";
                    string editHTML = "";

                    if (HtmlEdit)
                    {
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на това образование' class='GridActionIcon' onclick='DeleteApplicantEducation(" + civilEducation.CivilEducationId.ToString() + ");' />";
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

                html += "<span id='lblEducationMessage' class='" + messageClass + "'>" + message + "</span>";

                html += "</td></tr>";

                bool screenDisabled = false;
                if (IsRegistred())
                {
                    screenDisabled = (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU") == UIAccessLevel.Disabled) ||
                    (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU_EDU") == UIAccessLevel.Disabled) ||
                    (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL") == UIAccessLevel.Disabled);
                }
                else
                {
                    screenDisabled = (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_EDU") == UIAccessLevel.Disabled) ||
                   (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_EDU_EDU") == UIAccessLevel.Disabled) ||
                   (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL") == UIAccessLevel.Disabled);
                }

                if (!screenDisabled && this.PersonId > 0)
                {
                    html += "<tr><td>";
                    html += "<div id='btnAddNewApplicantEducation' style='display: inline;' onclick='ShowApplicantEducationLightBox(0);' class='Button'><i></i><div id='btnAddNewApplicantEducationText' style='width:90px;'>Добави ново</div><b></b></div><br />";
                    html += "</td></tr>";
                }

                html += "</td></tr>";
                html += "</table></div>";
            }

            return html;
        }

        //Generate applicant languages table
        private string GenerateApplicantLanguagesTable(string message, bool existError)
        {
            bool IsLanguageCodeHidden;
            bool IsLanguageLevelOfKnowledgeKeyHidden;
            bool IsLanguageFormOfKnowledgeKeyHidden;
            bool IsLanguageStanAgHidden;
            bool IsLanguageDiplomaKeyHidden;
            bool IsLanguageVacAnnHidden;
            bool IsLanguageDateWhenHidden;

            if (IsRegistred())
            {
                IsLanguageCodeHidden = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU_LANG_LANG") == UIAccessLevel.Hidden;
                IsLanguageLevelOfKnowledgeKeyHidden = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU_LANG_LVLKNG") == UIAccessLevel.Hidden;
                IsLanguageFormOfKnowledgeKeyHidden = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU_LANG_LVLFORM") == UIAccessLevel.Hidden;
                IsLanguageStanAgHidden = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU_LANG_STANAG") == UIAccessLevel.Hidden;
                IsLanguageDiplomaKeyHidden = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU_LANG_DPLM") == UIAccessLevel.Hidden;
                IsLanguageVacAnnHidden = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU_LANG_DOCNUM") == UIAccessLevel.Hidden;
                IsLanguageDateWhenHidden = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU_LANG_DOCDATE") == UIAccessLevel.Hidden;
            }
            else
            {
                IsLanguageCodeHidden = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_EDU_LANG_LANG") == UIAccessLevel.Hidden;
                IsLanguageLevelOfKnowledgeKeyHidden = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_EDU_LANG_LVLKNG") == UIAccessLevel.Hidden;
                IsLanguageFormOfKnowledgeKeyHidden = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_EDU_LANG_LVLFORM") == UIAccessLevel.Hidden;
                IsLanguageStanAgHidden = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_EDU_LANG_STANAG") == UIAccessLevel.Hidden;
                IsLanguageDiplomaKeyHidden = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_EDU_LANG_DPLM") == UIAccessLevel.Hidden;
                IsLanguageVacAnnHidden = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_EDU_LANG_DOCNUM") == UIAccessLevel.Hidden;
                IsLanguageDateWhenHidden = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_EDU_LANG_DOCDATE") == UIAccessLevel.Hidden;
            }

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

            //Generate Language table
            string html = "";
            List<PersonLangEduForeignLanguage> listPersonLangEduForeignLanguage = PersonLangEduForeignLanguageUtil.GetAllPersonLangEduForeignLanguageByPersonID(this.PersonId, CurrentUser);

            html += "<div id='divApplLangTable'>";
            html += "<table style='width: 100%;'>";
            html += "<tr><td align='left'><span class='SmallHeaderText'>Езикова подготовка</span></td></tr>";

            html += "<tr><td align='center'>";

            //No data found
            if (listPersonLangEduForeignLanguage.Count == 0)
            {
                html += "<span>Няма въведена информация</span>";

                html += "</td></tr>";

                html += "<tr><td>";

                bool screenDisabled = false;

                if (IsRegistred())
                {
                    screenDisabled = (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU") == UIAccessLevel.Disabled) ||
                      (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU_LANG") == UIAccessLevel.Disabled) ||
                      (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL") == UIAccessLevel.Disabled);
                }
                else
                {
                    screenDisabled = (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_EDU") == UIAccessLevel.Disabled) ||
                        (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_EDU_LANG") == UIAccessLevel.Disabled) ||
                        (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL") == UIAccessLevel.Disabled);
                }

                if (!screenDisabled && this.PersonId > 0)
                {
                    html += "<div id='btnAddNewApplicantLanguage' style='display: inline;' onclick='ShowApplicantLanguageLightBox(0);' class='Button'><i></i><div id='btnAddNewApplicantLanguageText' style='width:90px;'>Добави нова</div><b></b></div><br />";
                }


                html += "</td></tr>";

                html += "</table></div>";

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

                string POTAPPPL = "APPL_POTENCIALAPPL_ADD_POTENCIALAPPL";
                string EDU = "APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_EDU";
                string LANG = "APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_EDU_LANG";

                if (IsRegistred())
                {
                    POTAPPPL = "APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL";
                    EDU = "APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU";
                    LANG = "APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU_LANG";
                }

                bool HtmlEdit = ((GetUIItemAccessLevel(EDU) == UIAccessLevel.Enabled &&
                         GetUIItemAccessLevel(LANG) == UIAccessLevel.Enabled &&
                         GetUIItemAccessLevel(POTAPPPL) == UIAccessLevel.Enabled) && CanCurrentUserAccessThisMilDepartment);

                //Iterate through all items and add them into the grid
                foreach (PersonLangEduForeignLanguage personLangEduForeignLanguage in listPersonLangEduForeignLanguage)
                {
                    string deleteHTML = "";
                    string editHTML = "";

                    if (HtmlEdit)
                    {
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на тази езикова подготовка' class='GridActionIcon' onclick='DeleteApplicantLanguage(" + personLangEduForeignLanguage.PersonLangEduForeignLanguageId.ToString() + ");' />";
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

                html += "<span id='lblLanguageMessage' class='" + messageClass + "'>" + message + "</span>";

                html += "</td></tr>";

                bool screenDisabled = false;

                if (IsRegistred())
                {
                    screenDisabled = (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU") == UIAccessLevel.Disabled) ||
                      (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU_LANG") == UIAccessLevel.Disabled) ||
                      (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL") == UIAccessLevel.Disabled);
                }
                else
                {
                    screenDisabled = (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_EDU") == UIAccessLevel.Disabled) ||
                        (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_EDU_LANG") == UIAccessLevel.Disabled) ||
                        (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL") == UIAccessLevel.Disabled);
                }

                if (!screenDisabled && this.PersonId > 0)
                {
                    html += "<tr><td>";
                    html += "<div id='btnAddNewApplicantLanguage' style='display: inline;' onclick='ShowApplicantLanguageLightBox(0);' class='Button'><i></i><div id='btnAddNewApplicantLanguageText' style='width:90px;'>Добави нова</div><b></b></div><br />";
                    html += "</td></tr>";
                }

                html += "</td></tr>";
                html += "</table></div>";
            }

            return html;
        }

        //Generate html content for applicant education light box
        private void GenerateApplicantEducationLightBoxContent()
        {
            bool isRegistred = false;
            if (Request.Params["isRegistred"] != null)
            {
                if (Request.Params["isRegistred"] == "OK")
                {
                    isRegistred = true;
                }
            }

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
            string UIItems = GetCivilEducationUIItems(isRegistred);

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

        //Save an applicant education record (ajax call)
        private void JSSaveApplicantEducation()
        {
            if (GetUIItemAccessLevel("APPL_POTENCIALAPPL") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";
            int hdnPersonId = this.PersonId;

            try
            {
                //int.TryParse(Request.Params["PersonId"], out hdnPersonId);

                int civilEducationId = 0;
                int.TryParse(Request.Params["CivilEducationId"], out civilEducationId);

                Change change = new Change(CurrentUser, "APPL_PotencialApplicants");

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
            if (GetUIItemAccessLevel("APPL_POTENCIALAPPL") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("APPL_APPL_EDITAPPL_EDU") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            string status = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "APPL_PotencialApplicants");

                int hdnPersonId = int.Parse(Request.Params["PersonId"]);
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
        public string GetCivilEducationUIItems(bool isRegistred)
        {
            string UIItemsXML = "";

            //Setup the "manually add positions" light-box
            List<string> disabledClientControls = new List<string>();
            List<string> hiddenClientControls = new List<string>();

            UIAccessLevel l;

            if (!isRegistred)
            {
                l = GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_EDU_EDU_EDU");
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

                l = GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_EDU_EDU_SUBJ");
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

                l = GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_EDU_EDU_YEAR");
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

                l = GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_EDU_EDU_LEARNINGMETHOD");
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
            }
            else
            {
                l = GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU_EDU_EDU");
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

                l = GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU_EDU_SUBJ");
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

                l = GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU_EDU_YEAR");
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

                l = GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU_EDU_LEARNINGMETHOD");
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

        //Get the UIItems info for the ForeignLanguage table
        public string GetForeignLanguageUIItems(bool isRegistred)
        {
            string UIItemsXML = "";

            //Setup the "manually add positions" light-box
            List<string> disabledClientControls = new List<string>();
            List<string> hiddenClientControls = new List<string>();

            UIAccessLevel l;

            if (!isRegistred)
            {
                l = GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_EDU_LANG_LANG");
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


                l = GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_EDU_LANG_LVLKNG");
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

                l = GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_EDU_LANG_LVLFORM");
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

                l = GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_EDU_LANG_DPLM");
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

                l = GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_EDU_LANG_DOCNUM");
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

                l = GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_EDU_LANG_DOCDATE");
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
            }
            else
            {
                l = GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU_LANG_LANG");
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

                l = GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU_LANG_LVLKNG");
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

                l = GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU_LANG_LVLFORM");
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

                l = GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU_LANG_DPLM");
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

                l = GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU_LANG_DOCNUM");
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

                l = GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU_LANG_DOCDATE");
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
        private void GenerateApplicantLanguageLightBoxContent()
        {
            bool isRegistred = false;
            if (Request.Params["isRegistred"] != null)
            {
                if (Request.Params["isRegistred"] == "OK")
                {
                    isRegistred = true;
                }
            }

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
            string UIItems = GetForeignLanguageUIItems(isRegistred);

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
                int hdnPersonId = this.PersonId;
                //int.TryParse(Request.Params["PersonId"], out hdnPersonId);

                int personLangEduForeignLanguageId = 0;
                int.TryParse(Request.Params["ForeignLanguageId"], out personLangEduForeignLanguageId);

                Change change = new Change(CurrentUser, "APPL_PotencialApplicants");

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
                Change change = new Change(CurrentUser, "APPL_PotencialApplicants");

                int hdnPersonId = int.Parse(Request.Params["PersonId"]);
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

        // Setup user interface elements according to rights of the user's role

        private void SetSetupPageUIServerControls()
        {
            bool isRegistred = IsRegistred();

            //if (!String.IsNullOrEmpty(Request.Params["PotencialApplicantId"]))
            //{
            //    isRegistred = true;
            //}
            //else
            //{
            //    string identNumber = Request.Params["IdentNumber"];
            //    Person person = PersonUtil.GetPersonByIdentNumber(identNumber, CurrentUser);

            //    int militaryDepartmentID = 0;
            //    int.TryParse(this.hdnMilitaryDepartmentID.Value, out militaryDepartmentID);

            //    PotencialApplicant potencialApplicant = PotencialApplicantUtil.GetPotencialApplicant(person.PersonId, militaryDepartmentID, CurrentUser);
            //    if (potencialApplicant != null)
            //    {
            //        isRegistred = true;
            //    }
            //}
            if (!isRegistred) //Mode Insert
            {
                bool screenHidden = (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL") != UIAccessLevel.Enabled) ||
                                      (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL") != UIAccessLevel.Enabled);

                bool screenDisabled = (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL") == UIAccessLevel.Disabled) ||
                                      (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL") == UIAccessLevel.Disabled);

                if (screenHidden)
                    RedirectAccessDenied();

                if (screenDisabled)
                {
                    this.pageDisabledControls.Add(btnSave);
                }

                //Enable/Disable Server Controls

                l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_LASTAPPERIANCE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(txtLastApperiance);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(txtLastApperiance);
                }
            }
            else
            {
                //Enable/Disable Server Controls

                bool screenHidden = (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL") == UIAccessLevel.Hidden) ||
                                      (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL") == UIAccessLevel.Hidden);

                bool screenDisabled = (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL") == UIAccessLevel.Disabled) ||
                                      (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL") == UIAccessLevel.Disabled);

                if (screenHidden)
                    RedirectAccessDenied();

                if (screenDisabled)
                {
                    this.pageDisabledControls.Add(btnSave);
                }

                //Enable/Disable Server Controls

                l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_LASTAPPERIANCE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(txtLastApperiance);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(txtLastApperiance);
                }
            }

        }

        private void SetupPageUIClientControls(bool isRegistred)
        {

            if (!isRegistred) //Mode Add
            {
                bool screenHidden = (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL") != UIAccessLevel.Enabled) ||
                                      (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL") != UIAccessLevel.Enabled);

                bool screenDisabled = (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL") == UIAccessLevel.Disabled) ||
                                      (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL") == UIAccessLevel.Disabled);

                if (screenHidden)
                    RedirectAccessDenied();

                //Enable/Disable Client Controls

                disabledClientControls = new List<string>();
                hiddenClientControls = new List<string>();

                if (screenDisabled)
                {
                    hiddenClientControls.Add(btnSave.ClientID);
                    hiddenClientControls.Add("btnAddNewApplicantEducation");
                    hiddenClientControls.Add("btnAddNewApplicantLanguage");
                }

                //Chack for whole page person details


                l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_PERSONDETAILS");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("txtFirstName");
                    disabledClientControls.Add("lblFirstName");

                    disabledClientControls.Add("txtLastName");
                    disabledClientControls.Add("lblLastName");

                    disabledClientControls.Add("ddGender");
                    disabledClientControls.Add("lblGender");


                    disabledClientControls.Add("txtPermPostCode");
                    disabledClientControls.Add("ddPermRegion");
                    disabledClientControls.Add("ddPermMunicipality");
                    disabledClientControls.Add("ddPermCity");
                    disabledClientControls.Add("ddPermDistrict");

                    disabledClientControls.Add("lblPermPostCode");
                    disabledClientControls.Add("lblPermRegion");
                    disabledClientControls.Add("lblPermMunicipality");
                    disabledClientControls.Add("lblPermCity");
                    disabledClientControls.Add("lblPermDistrict");

                    disabledClientControls.Add("txtaPermAddress");
                    disabledClientControls.Add("lblPermAddress");
                    hiddenClientControls.Add("btnImgCopyAddressPerm");

                    disabledClientControls.Add("txtPresPostCode");
                    disabledClientControls.Add("ddPresRegion");
                    disabledClientControls.Add("ddPresMunicipality");
                    disabledClientControls.Add("ddPresCity");
                    disabledClientControls.Add("ddPresDistrict");

                    disabledClientControls.Add("lblPresPostCode");
                    disabledClientControls.Add("lblPresRegion");
                    disabledClientControls.Add("lblPresMunicipality");
                    disabledClientControls.Add("lblPresCity");
                    disabledClientControls.Add("lblPresDistrict");

                    disabledClientControls.Add("txtaPresAddress");
                    disabledClientControls.Add("lblPresAddress");
                    hiddenClientControls.Add("btnImgCopyAddressPres");

                    disabledClientControls.Add("txtContactPostCode");
                    disabledClientControls.Add("ddContactRegion");
                    disabledClientControls.Add("ddContactMunicipality");
                    disabledClientControls.Add("ddContactCity");
                    disabledClientControls.Add("ddContactDistrict");

                    disabledClientControls.Add("lblContactPostCode");
                    disabledClientControls.Add("lblContactRegion");
                    disabledClientControls.Add("lblContactMunicipality");
                    disabledClientControls.Add("lblContactCity");
                    disabledClientControls.Add("lblContactDistrict");

                    disabledClientControls.Add("txtaContactAddress");
                    disabledClientControls.Add("lblContactAddress");
                    hiddenClientControls.Add("btnImgCopyAddressContact");

                    disabledClientControls.Add("hasMilitarySrv1");
                    disabledClientControls.Add("hasMilitarySrv2");

                    disabledClientControls.Add("lblWentToMilitary");
                    disabledClientControls.Add("lblWentToMilitaryYes");
                    disabledClientControls.Add("lblWentToMilitaryNo");

                    disabledClientControls.Add("militaryTraining1");
                    disabledClientControls.Add("militaryTraining2");

                    disabledClientControls.Add("lblMilitaryTraining");
                    disabledClientControls.Add("lblMilitaryTraining1");
                    disabledClientControls.Add("lblMilitaryTraining2");

                    disabledClientControls.Add("lblIDCardNumber");
                    disabledClientControls.Add("txtIDCardNumber");

                    disabledClientControls.Add("lblIDCardIssuedBy");
                    disabledClientControls.Add("txtIDCardIssuedBy");

                    disabledClientControls.Add("lblIDCardIssueDate");
                    disabledClientControls.Add("txtIDCardIssueDate");

                    disabledClientControls.Add("txtHomePhone");
                    disabledClientControls.Add("lblHomePhone");

                    disabledClientControls.Add("txtMobilePhone");
                    disabledClientControls.Add("lblMobilePhone");

                    disabledClientControls.Add("txtEmail");
                    disabledClientControls.Add("lblEmail");

                    disabledClientControls.Add("pickListDrvLicCategories_txtSelected");
                    disabledClientControls.Add("lblDrvLicCategories");
                    //not disable but hide image button
                    hiddenClientControls.Add("pickListDrvLicCategories_imgDropDown");

                    disabledClientControls.Add("hasMilitarySrv1");
                    disabledClientControls.Add("hasMilitarySrv2");

                    disabledClientControls.Add("lblWentToMilitary");
                    disabledClientControls.Add("lblWentToMilitaryYes");
                    disabledClientControls.Add("lblWentToMilitaryNo");
                }

                if (l == UIAccessLevel.Hidden || screenHidden)
                {
                    hiddenClientControls.Add("txtFirstName");
                    hiddenClientControls.Add("lblFirstName");

                    hiddenClientControls.Add("txtLastName");
                    hiddenClientControls.Add("lblLastName");

                    hiddenClientControls.Add("ddGender");
                    hiddenClientControls.Add("lblGender");


                    hiddenClientControls.Add("txtPermPostCode");
                    hiddenClientControls.Add("ddPermRegion");
                    hiddenClientControls.Add("ddPermMunicipality");
                    hiddenClientControls.Add("ddPermCity");
                    hiddenClientControls.Add("ddPermDistrict");

                    hiddenClientControls.Add("lblPermPostCode");
                    hiddenClientControls.Add("lblPermRegion");
                    hiddenClientControls.Add("lblPermMunicipality");
                    hiddenClientControls.Add("lblPermDistrict");

                    hiddenClientControls.Add("txtaPermAddress");
                    hiddenClientControls.Add("lblPermAddress");
                    hiddenClientControls.Add("btnImgCopyAddressPerm");

                    hiddenClientControls.Add("txtPresPostCode");
                    hiddenClientControls.Add("ddPresRegion");
                    hiddenClientControls.Add("ddPresMunicipality");
                    hiddenClientControls.Add("ddPresCity");
                    hiddenClientControls.Add("ddPresDistrict");

                    hiddenClientControls.Add("lblPresPostCode");
                    hiddenClientControls.Add("lblPresRegion");
                    hiddenClientControls.Add("lblPresMunicipality");
                    hiddenClientControls.Add("lblPresDistrict");

                    hiddenClientControls.Add("txtaPresAddress");
                    hiddenClientControls.Add("lblPresAddress");
                    hiddenClientControls.Add("btnImgCopyAddressPres");

                    hiddenClientControls.Add("txtContactPostCode");
                    hiddenClientControls.Add("ddContactRegion");
                    hiddenClientControls.Add("ddContactMunicipality");
                    hiddenClientControls.Add("ddContactCity");
                    hiddenClientControls.Add("ddContactDistrict");

                    hiddenClientControls.Add("lblContactPostCode");
                    hiddenClientControls.Add("lblContactRegion");
                    hiddenClientControls.Add("lblContactMunicipality");
                    hiddenClientControls.Add("lblContactCity");
                    hiddenClientControls.Add("lblContactDistrict");

                    hiddenClientControls.Add("txtaContactAddress");
                    hiddenClientControls.Add("lblContactAddress");
                    hiddenClientControls.Add("btnImgCopyAddressContact");

                    hiddenClientControls.Add("hasMilitarySrv1");
                    hiddenClientControls.Add("hasMilitarySrv2");

                    hiddenClientControls.Add("lblWentToMilitary");
                    hiddenClientControls.Add("lblWentToMilitaryYes");
                    hiddenClientControls.Add("lblWentToMilitaryNo");

                    hiddenClientControls.Add("militaryTraining1");
                    hiddenClientControls.Add("militaryTraining2");

                    hiddenClientControls.Add("lblMilitaryTraining");
                    hiddenClientControls.Add("lblMilitaryTraining1");
                    hiddenClientControls.Add("lblMilitaryTraining2");

                    hiddenClientControls.Add("lblIDCardNumber");
                    hiddenClientControls.Add("txtIDCardNumber");

                    hiddenClientControls.Add("lblIDCardIssuedBy");
                    hiddenClientControls.Add("txtIDCardIssuedBy");

                    hiddenClientControls.Add("lblIDCardIssueDate");
                    hiddenClientControls.Add("spanIDCardIssueDate");

                    hiddenClientControls.Add("txtHomePhone");
                    hiddenClientControls.Add("lblHomePhone");

                    hiddenClientControls.Add("txtMobilePhone");
                    hiddenClientControls.Add("lblMobilePhone");

                    hiddenClientControls.Add("txtEmail");
                    hiddenClientControls.Add("lblEmail");

                    hiddenClientControls.Add("pickListDrvLicCategories_txtSelected");
                    hiddenClientControls.Add("lblDrvLicCategories");
                    //not disable but hide image button
                    hiddenClientControls.Add("pickListDrvLicCategories_imgDropDown");

                    hiddenClientControls.Add("hasMilitarySrv1");
                    hiddenClientControls.Add("hasMilitarySrv2");

                    hiddenClientControls.Add("lblWentToMilitary");
                    hiddenClientControls.Add("lblWentToMilitaryYes");
                    hiddenClientControls.Add("lblWentToMilitaryNo");
                }

                else
                {


                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_FIRSTNAME");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("txtFirstName");
                        disabledClientControls.Add("lblFirstName");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("txtFirstName");
                        hiddenClientControls.Add("lblFirstName");
                    }

                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_LASTNAME");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("txtLastName");
                        disabledClientControls.Add("lblLastName");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("txtLastName");
                        hiddenClientControls.Add("lblLastName");
                    }


                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_GENDER");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("ddGender");
                        disabledClientControls.Add("lblGender");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("ddGender");
                        hiddenClientControls.Add("lblGender");
                    }

                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_PERMADDRESS_CITY");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("txtPermPostCode");
                        disabledClientControls.Add("ddPermRegion");
                        disabledClientControls.Add("ddPermMunicipality");
                        disabledClientControls.Add("ddPermCity");
                        disabledClientControls.Add("ddPermDistrict");

                        disabledClientControls.Add("lblPermPostCode");
                        disabledClientControls.Add("lblPermRegion");
                        disabledClientControls.Add("lblPermMunicipality");
                        disabledClientControls.Add("lblPermDistrict");
                        hiddenClientControls.Add("btnImgCopyAddressPerm");
                    }

                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("txtPermPostCode");
                        hiddenClientControls.Add("ddPermRegion");
                        hiddenClientControls.Add("ddPermMunicipality");
                        hiddenClientControls.Add("ddPermCity");
                        hiddenClientControls.Add("ddPermDistrict");

                        hiddenClientControls.Add("lblPermPostCode");
                        hiddenClientControls.Add("lblPermRegion");
                        hiddenClientControls.Add("lblPermMunicipality");
                        hiddenClientControls.Add("lblPermCity");
                        hiddenClientControls.Add("lblPermDistrict");
                        hiddenClientControls.Add("btnImgCopyAddressPerm");
                    }

                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_PERMADDRESS_ADDRESS");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("txtaPermAddress");
                        disabledClientControls.Add("lblPermAddress");
                        hiddenClientControls.Add("btnImgCopyAddressPerm");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("txtaPermAddress");
                        hiddenClientControls.Add("lblPermAddress");
                        hiddenClientControls.Add("btnImgCopyAddressPerm");
                    }


                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_PRESADDRESS_CITY");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("txtPresPostCode");
                        disabledClientControls.Add("ddPresRegion");
                        disabledClientControls.Add("ddPresMunicipality");
                        disabledClientControls.Add("ddPresCity");
                        disabledClientControls.Add("ddPresDistrict");

                        disabledClientControls.Add("lblPresPostCode");
                        disabledClientControls.Add("lblPresRegion");
                        disabledClientControls.Add("lblPresMunicipality");
                        disabledClientControls.Add("lblPresCity");
                        disabledClientControls.Add("lblPresDistrict");
                        hiddenClientControls.Add("btnImgCopyAddressPres");
                    }

                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("txtPresPostCode");
                        hiddenClientControls.Add("ddPresRegion");
                        hiddenClientControls.Add("ddPresMunicipality");
                        hiddenClientControls.Add("ddPresCity");
                        hiddenClientControls.Add("ddPresDistrict");

                        hiddenClientControls.Add("lblPresPostCode");
                        hiddenClientControls.Add("lblPresRegion");
                        hiddenClientControls.Add("lblPresMunicipality");
                        hiddenClientControls.Add("lblPresDistrict");
                        hiddenClientControls.Add("btnImgCopyAddressPres");
                    }

                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_PRESADDRESS_ADDRESS");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("txtaPresAddress");
                        disabledClientControls.Add("lblPresAddress");
                        hiddenClientControls.Add("btnImgCopyAddressPres");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("txtaPresAddress");
                        hiddenClientControls.Add("lblPresAddress");
                        hiddenClientControls.Add("btnImgCopyAddressPres");
                    }

                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_CONTACTADDRESS_CITY");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("txtContactPostCode");
                        disabledClientControls.Add("ddContactRegion");
                        disabledClientControls.Add("ddContactMunicipality");
                        disabledClientControls.Add("ddContactCity");
                        disabledClientControls.Add("ddContactDistrict");

                        disabledClientControls.Add("lblContactPostCode");
                        disabledClientControls.Add("lblContactRegion");
                        disabledClientControls.Add("lblContactMunicipality");
                        disabledClientControls.Add("lblContactCity");
                        disabledClientControls.Add("lblContactDistrict");
                        hiddenClientControls.Add("btnImgCopyAddressContact");
                    }

                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("txtContactPostCode");
                        hiddenClientControls.Add("ddContactRegion");
                        hiddenClientControls.Add("ddContactMunicipality");
                        hiddenClientControls.Add("ddContactCity");
                        hiddenClientControls.Add("ddContactDistrict");

                        hiddenClientControls.Add("lblContactPostCode");
                        hiddenClientControls.Add("lblContactRegion");
                        hiddenClientControls.Add("lblContactMunicipality");
                        hiddenClientControls.Add("lblContactCity");
                        hiddenClientControls.Add("lblContactDistrict");
                        hiddenClientControls.Add("btnImgCopyAddressContact");
                    }

                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_CONTACTADDRESS_ADDRESS");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("txtaContactAddress");
                        disabledClientControls.Add("lblContactAddress");
                        hiddenClientControls.Add("btnImgCopyAddressContact");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("txtaContactAddress");
                        hiddenClientControls.Add("lblContactAddress");
                        hiddenClientControls.Add("btnImgCopyAddressContact");
                    }

                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_IDCARDNUMBER");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("lblIDCardNumber");
                        disabledClientControls.Add("txtIDCardNumber");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblIDCardNumber");
                        hiddenClientControls.Add("txtIDCardNumber");
                    }

                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_IDCARDISSUEDBY");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("lblIDCardIssuedBy");
                        disabledClientControls.Add("txtIDCardIssuedBy");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblIDCardIssuedBy");
                        hiddenClientControls.Add("txtIDCardIssuedBy");
                    }

                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_IDCARDISSUEDATE");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("lblIDCardIssueDate");
                        disabledClientControls.Add("txtIDCardIssueDate");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblIDCardIssueDate");
                        hiddenClientControls.Add("spanIDCardIssueDate");
                    }

                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_HOMEPHONE");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("txtHomePhone");
                        disabledClientControls.Add("lblHomePhone");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("txtHomePhone");
                        hiddenClientControls.Add("lblHomePhone");
                    }

                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_MOBILEPHONE");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("txtMobilePhone");
                        disabledClientControls.Add("lblMobilePhone");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("txtMobilePhone");
                        hiddenClientControls.Add("lblMobilePhone");
                    }

                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_EMAIL");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("txtEmail");
                        disabledClientControls.Add("lblEmail");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("txtEmail");
                        hiddenClientControls.Add("lblEmail");
                    }

                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_DRIVINGLICENCE");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("pickListDrvLicCategories_txtSelected");
                        disabledClientControls.Add("lblDrvLicCategories");
                        //not disable but hide image button
                        hiddenClientControls.Add("pickListDrvLicCategories_imgDropDown");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        //In this case we hide div element
                        hiddenClientControls.Add("tdPickListDrvLicCategories");
                        hiddenClientControls.Add("lblDrvLicCategories");
                    }


                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_HASMILITARYSERVICE");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("hasMilitarySrv1");
                        disabledClientControls.Add("hasMilitarySrv2");

                        disabledClientControls.Add("lblWentToMilitary");
                        disabledClientControls.Add("lblWentToMilitaryYes");
                        disabledClientControls.Add("lblWentToMilitaryNo");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("hasMilitarySrv1");
                        hiddenClientControls.Add("hasMilitarySrv2");

                        hiddenClientControls.Add("lblWentToMilitary");
                        hiddenClientControls.Add("lblWentToMilitaryYes");
                        hiddenClientControls.Add("lblWentToMilitaryNo");
                    }

                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_MILITARYTRAINING");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("militaryTraining1");
                        disabledClientControls.Add("militaryTraining2");

                        disabledClientControls.Add("lblMilitaryTraining");
                        disabledClientControls.Add("lblMilitaryTraining1");
                        disabledClientControls.Add("lblMilitaryTraining2");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("militaryTraining1");
                        hiddenClientControls.Add("militaryTraining2");

                        hiddenClientControls.Add("lblMilitaryTraining");
                        hiddenClientControls.Add("lblMilitaryTraining1");
                        hiddenClientControls.Add("lblMilitaryTraining2");
                    }
                }

                l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_COMMENTS");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("txtaComment");
                    disabledClientControls.Add("lblComment");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("txtaComment");
                    hiddenClientControls.Add("lblComment");
                }

                l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_SERVICETYPE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblServiceType");
                    disabledClientControls.Add("lblSelectedServiceType");
                    disabledClientControls.Add("ddServiceTypes_AvailableOptions");
                    disabledClientControls.Add("ddServiceTypes_AssignedOptions");
                    hiddenClientControls.Add("btnSelectServiceTypes");
                    hiddenClientControls.Add("btnRemoveServiceTypes");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblServiceType");
                    hiddenClientControls.Add("lblSelectedServiceType");
                    hiddenClientControls.Add("ddServiceTypes_AvailableOptions");
                    hiddenClientControls.Add("ddServiceTypes_AssignedOptions");
                    hiddenClientControls.Add("btnSelectServiceTypes");
                    hiddenClientControls.Add("btnRemoveServiceTypes");
                }

                l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_MilitaryTrainingCourse");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblMilitaryTrainingCourse");
                    disabledClientControls.Add("lblSelectedMilitaryTrainingCourse");
                    disabledClientControls.Add("ddMTC_AvailableOptions");
                    disabledClientControls.Add("ddMTC_AssignedOptions");
                    hiddenClientControls.Add("btnSelectMTC");
                    hiddenClientControls.Add("btnRemoveMTC");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblMilitaryTrainingCourse");
                    hiddenClientControls.Add("lblSelectedMilitaryTrainingCourse");
                    hiddenClientControls.Add("ddMTC_AvailableOptions");
                    hiddenClientControls.Add("ddMTC_AssignedOptions");
                    hiddenClientControls.Add("btnSelectMTC");
                    hiddenClientControls.Add("btnRemoveMTC");
                }

                l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_LASTAPPERIANCE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblLastApperiance");
                    disabledClientControls.Add(txtLastApperiance.ClientID);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("divLastApperiance");
                }

                l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_EDU");
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("divEducation");
                    hiddenClientControls.Add("divLanguage");
                }
                else
                {
                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_EDU_EDU");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("divEducation");
                    }

                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL_EDU_LANG");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("divLanguage");
                    }
                }
            }
            else // edit mode of page
            {
                bool screenHidden = (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL") == UIAccessLevel.Hidden) ||
                                      (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL") == UIAccessLevel.Hidden);

                bool screenDisabled = (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL") == UIAccessLevel.Disabled) ||
                                      (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL") == UIAccessLevel.Disabled);

                if (screenHidden)
                    RedirectAccessDenied();

                if (PageDisabled)
                    screenDisabled = true;

                //Enable/Disable Client Controls

                disabledClientControls = new List<string>();
                hiddenClientControls = new List<string>();

                if (screenDisabled)
                {
                    hiddenClientControls.Add(btnSave.ClientID);
                    hiddenClientControls.Add("btnAddNewApplicantEducation");
                    hiddenClientControls.Add("btnAddNewApplicantLanguage");
                }

                l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_PERSONDETAILS");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("txtFirstName");
                    disabledClientControls.Add("lblFirstName");

                    disabledClientControls.Add("txtLastName");
                    disabledClientControls.Add("lblLastName");

                    disabledClientControls.Add("ddGender");
                    disabledClientControls.Add("lblGender");


                    disabledClientControls.Add("txtPermPostCode");
                    disabledClientControls.Add("ddPermRegion");
                    disabledClientControls.Add("ddPermMunicipality");
                    disabledClientControls.Add("ddPermCity");
                    disabledClientControls.Add("ddPermDistrict");

                    disabledClientControls.Add("lblPermPostCode");
                    disabledClientControls.Add("lblPermRegion");
                    disabledClientControls.Add("lblPermMunicipality");
                    disabledClientControls.Add("lblPermCity");
                    disabledClientControls.Add("lblPermDistrict");

                    disabledClientControls.Add("txtaPermAddress");
                    disabledClientControls.Add("lblPermAddress");
                    hiddenClientControls.Add("btnImgCopyAddressPerm");

                    disabledClientControls.Add("txtPresPostCode");
                    disabledClientControls.Add("ddPresRegion");
                    disabledClientControls.Add("ddPresMunicipality");
                    disabledClientControls.Add("ddPresCity");
                    disabledClientControls.Add("ddPresDistrict");

                    disabledClientControls.Add("lblPresPostCode");
                    disabledClientControls.Add("lblPresRegion");
                    disabledClientControls.Add("lblPresMunicipality");
                    disabledClientControls.Add("lblPresCity");
                    disabledClientControls.Add("lblPresDistrict");

                    disabledClientControls.Add("txtaPresAddress");
                    disabledClientControls.Add("lblPresAddress");
                    hiddenClientControls.Add("btnImgCopyAddressPres");

                    disabledClientControls.Add("txtContactPostCode");
                    disabledClientControls.Add("ddContactRegion");
                    disabledClientControls.Add("ddContactMunicipality");
                    disabledClientControls.Add("ddContactCity");
                    disabledClientControls.Add("ddContactDistrict");

                    disabledClientControls.Add("lblContactPostCode");
                    disabledClientControls.Add("lblContactRegion");
                    disabledClientControls.Add("lblContactMunicipality");
                    disabledClientControls.Add("lblContactCity");
                    disabledClientControls.Add("lblContactDistrict");

                    disabledClientControls.Add("txtaContactAddress");
                    disabledClientControls.Add("lblContactAddress");
                    hiddenClientControls.Add("btnImgCopyAddressContact");

                    //disabledClientControls.Add("hasMilitarySrv1");
                    //disabledClientControls.Add("hasMilitarySrv2");

                    //disabledClientControls.Add("lblWentToMilitary");
                    //disabledClientControls.Add("lblWentToMilitaryYes");
                    //disabledClientControls.Add("lblWentToMilitaryNo");

                    disabledClientControls.Add("lblIDCardNumber");
                    disabledClientControls.Add("txtIDCardNumber");

                    disabledClientControls.Add("lblIDCardIssuedBy");
                    disabledClientControls.Add("txtIDCardIssuedBy");

                    disabledClientControls.Add("lblIDCardIssueDate");
                    disabledClientControls.Add("txtIDCardIssueDate");

                    disabledClientControls.Add("txtHomePhone");
                    disabledClientControls.Add("lblHomePhone");

                    disabledClientControls.Add("txtMobilePhone");
                    disabledClientControls.Add("lblMobilePhone");

                    disabledClientControls.Add("txtEmail");
                    disabledClientControls.Add("lblEmail");

                    disabledClientControls.Add("pickListDrvLicCategories_txtSelected");
                    disabledClientControls.Add("lblDrvLicCategories");
                    //not disable but hide image button
                    hiddenClientControls.Add("pickListDrvLicCategories_imgDropDown");

                    disabledClientControls.Add("hasMilitarySrv1");
                    disabledClientControls.Add("hasMilitarySrv2");

                    disabledClientControls.Add("lblWentToMilitary");
                    disabledClientControls.Add("lblWentToMilitaryYes");
                    disabledClientControls.Add("lblWentToMilitaryNo");

                    disabledClientControls.Add("militaryTraining1");
                    disabledClientControls.Add("militaryTraining2");

                    disabledClientControls.Add("lblMilitaryTraining");
                    disabledClientControls.Add("lblMilitaryTraining1");
                    disabledClientControls.Add("lblMilitaryTraining2");

                }
                if (l == UIAccessLevel.Hidden || screenHidden)
                {
                    hiddenClientControls.Add("txtFirstName");
                    hiddenClientControls.Add("lblFirstName");

                    hiddenClientControls.Add("txtLastName");
                    hiddenClientControls.Add("lblLastName");

                    hiddenClientControls.Add("ddGender");
                    hiddenClientControls.Add("lblGender");


                    hiddenClientControls.Add("txtPermPostCode");
                    hiddenClientControls.Add("ddPermRegion");
                    hiddenClientControls.Add("ddPermMunicipality");
                    hiddenClientControls.Add("ddPermCity");
                    hiddenClientControls.Add("ddPermDistrict");

                    hiddenClientControls.Add("lblPermPostCode");
                    hiddenClientControls.Add("lblPermRegion");
                    hiddenClientControls.Add("lblPermMunicipality");
                    hiddenClientControls.Add("lblPermCity");
                    hiddenClientControls.Add("lblPermDistrict");

                    hiddenClientControls.Add("txtaPermAddress");
                    hiddenClientControls.Add("lblPermAddress");
                    hiddenClientControls.Add("btnImgCopyAddressPerm");

                    hiddenClientControls.Add("txtPresPostCode");
                    hiddenClientControls.Add("ddPresRegion");
                    hiddenClientControls.Add("ddPresMunicipality");
                    hiddenClientControls.Add("ddPresCity");
                    hiddenClientControls.Add("ddPresDistrict");

                    hiddenClientControls.Add("lblPresPostCode");
                    hiddenClientControls.Add("lblPresRegion");
                    hiddenClientControls.Add("lblPresMunicipality");
                    hiddenClientControls.Add("lblPresCity");
                    hiddenClientControls.Add("lblPresDistrict");

                    hiddenClientControls.Add("txtaPresAddress");
                    hiddenClientControls.Add("lblPresAddress");
                    hiddenClientControls.Add("btnImgCopyAddressPres");

                    hiddenClientControls.Add("txtContactPostCode");
                    hiddenClientControls.Add("ddContactRegion");
                    hiddenClientControls.Add("ddContactMunicipality");
                    hiddenClientControls.Add("ddContactCity");
                    hiddenClientControls.Add("ddContactDistrict");

                    hiddenClientControls.Add("lblContactPostCode");
                    hiddenClientControls.Add("lblContactRegion");
                    hiddenClientControls.Add("lblContactMunicipality");
                    hiddenClientControls.Add("lblContactCity");
                    hiddenClientControls.Add("lblContactDistrict");

                    hiddenClientControls.Add("txtaContactAddress");
                    hiddenClientControls.Add("lblContactAddress");
                    hiddenClientControls.Add("btnImgCopyAddressContact");

                    //hiddenClientControls.Add("hasMilitarySrv1");
                    //hiddenClientControls.Add("hasMilitarySrv2");

                    //hiddenClientControls.Add("lblWentToMilitary");
                    //hiddenClientControls.Add("lblWentToMilitaryYes");
                    //hiddenClientControls.Add("lblWentToMilitaryNo");

                    hiddenClientControls.Add("lblIDCardNumber");
                    hiddenClientControls.Add("txtIDCardNumber");

                    hiddenClientControls.Add("lblIDCardIssuedBy");
                    hiddenClientControls.Add("txtIDCardIssuedBy");

                    hiddenClientControls.Add("lblIDCardIssueDate");
                    hiddenClientControls.Add("spanIDCardIssueDate");

                    hiddenClientControls.Add("txtHomePhone");
                    hiddenClientControls.Add("lblHomePhone");

                    hiddenClientControls.Add("txtMobilePhone");
                    hiddenClientControls.Add("lblMobilePhone");

                    hiddenClientControls.Add("txtEmail");
                    hiddenClientControls.Add("lblEmail");

                    hiddenClientControls.Add("pickListDrvLicCategories_txtSelected");
                    hiddenClientControls.Add("lblDrvLicCategories");
                    //not disable but hide image button
                    hiddenClientControls.Add("pickListDrvLicCategories_imgDropDown");

                    hiddenClientControls.Add("hasMilitarySrv1");
                    hiddenClientControls.Add("hasMilitarySrv2");

                    hiddenClientControls.Add("lblWentToMilitary");
                    hiddenClientControls.Add("lblWentToMilitaryYes");
                    hiddenClientControls.Add("lblWentToMilitaryNo");

                    hiddenClientControls.Add("militaryTraining1");
                    hiddenClientControls.Add("militaryTraining2");

                    hiddenClientControls.Add("lblMilitaryTraining");
                    hiddenClientControls.Add("lblMilitaryTraining1");
                    hiddenClientControls.Add("lblMilitaryTraining2");
                }
                else
                {

                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_FIRSTNAME");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("txtFirstName");
                        disabledClientControls.Add("lblFirstName");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("txtFirstName");
                        hiddenClientControls.Add("lblFirstName");
                    }

                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_LASTNAME");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("txtLastName");
                        disabledClientControls.Add("lblLastName");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("txtLastName");
                        hiddenClientControls.Add("lblLastName");
                    }


                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_GENDER");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("ddGender");
                        disabledClientControls.Add("lblGender");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("ddGender");
                        hiddenClientControls.Add("lblGender");
                    }


                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_PERMADDRESS_CITY");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("txtPermPostCode");
                        disabledClientControls.Add("ddPermRegion");
                        disabledClientControls.Add("ddPermMunicipality");
                        disabledClientControls.Add("ddPermCity");
                        disabledClientControls.Add("ddPermDistrict");

                        disabledClientControls.Add("lblPermPostCode");
                        disabledClientControls.Add("lblPermRegion");
                        disabledClientControls.Add("lblPermMunicipality");
                        disabledClientControls.Add("lblPermDistrict");
                        hiddenClientControls.Add("btnImgCopyAddressPerm");
                    }

                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("txtPermPostCode");
                        hiddenClientControls.Add("ddPermRegion");
                        hiddenClientControls.Add("ddPermMunicipality");
                        hiddenClientControls.Add("ddPermCity");
                        hiddenClientControls.Add("ddPermDistrict");

                        hiddenClientControls.Add("lblPermPostCode");
                        hiddenClientControls.Add("lblPermRegion");
                        hiddenClientControls.Add("lblPermMunicipality");
                        hiddenClientControls.Add("lblPermCity");
                        hiddenClientControls.Add("lblPermDistrict");
                        hiddenClientControls.Add("btnImgCopyAddressPerm");
                    }

                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_PERMADDRESS_ADDRESS");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("txtaPermAddress");
                        disabledClientControls.Add("lblPermAddress");
                        hiddenClientControls.Add("btnImgCopyAddressPerm");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("txtaPermAddress");
                        hiddenClientControls.Add("lblPermAddress");
                        hiddenClientControls.Add("btnImgCopyAddressPerm");
                    }


                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_PRESADDRESS_CITY");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("txtPresPostCode");
                        disabledClientControls.Add("ddPresRegion");
                        disabledClientControls.Add("ddPresMunicipality");
                        disabledClientControls.Add("ddPresCity");
                        disabledClientControls.Add("ddPresDistrict");

                        disabledClientControls.Add("lblPresPostCode");
                        disabledClientControls.Add("lblPresRegion");
                        disabledClientControls.Add("lblPresMunicipality");
                        disabledClientControls.Add("lblPresCity");
                        disabledClientControls.Add("lblPresDistrict");
                        hiddenClientControls.Add("btnImgCopyAddressPres");
                    }

                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("txtPresPostCode");
                        hiddenClientControls.Add("ddPresRegion");
                        hiddenClientControls.Add("ddPresMunicipality");
                        hiddenClientControls.Add("ddPresCity");
                        hiddenClientControls.Add("ddPresDistrict");

                        hiddenClientControls.Add("lblPresPostCode");
                        hiddenClientControls.Add("lblPresRegion");
                        hiddenClientControls.Add("lblPresMunicipality");
                        hiddenClientControls.Add("lblPresCity");
                        hiddenClientControls.Add("lblPresDistrict");
                        hiddenClientControls.Add("btnImgCopyAddressPres");
                    }

                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_PRESADDRESS_ADDRESS");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("txtaPresAddress");
                        disabledClientControls.Add("lblPresAddress");
                        hiddenClientControls.Add("btnImgCopyAddressPres");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("txtaPresAddress");
                        hiddenClientControls.Add("lblPresAddress");
                        hiddenClientControls.Add("btnImgCopyAddressPres");
                    }

                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_CONTACTADDRESS_CITY");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("txtContactPostCode");
                        disabledClientControls.Add("ddContactRegion");
                        disabledClientControls.Add("ddContactMunicipality");
                        disabledClientControls.Add("ddContactCity");
                        disabledClientControls.Add("ddContactDistrict");

                        disabledClientControls.Add("lblContactPostCode");
                        disabledClientControls.Add("lblContactRegion");
                        disabledClientControls.Add("lblContactMunicipality");
                        disabledClientControls.Add("lblContactCity");
                        disabledClientControls.Add("lblContactDistrict");
                        hiddenClientControls.Add("btnImgCopyAddressContact");
                    }

                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("txtContactPostCode");
                        hiddenClientControls.Add("ddContactRegion");
                        hiddenClientControls.Add("ddContactMunicipality");
                        hiddenClientControls.Add("ddContactCity");
                        hiddenClientControls.Add("ddContactDistrict");

                        hiddenClientControls.Add("lblContactPostCode");
                        hiddenClientControls.Add("lblContactRegion");
                        hiddenClientControls.Add("lblContactMunicipality");
                        hiddenClientControls.Add("lblContactCity");
                        hiddenClientControls.Add("lblContactDistrict");
                        hiddenClientControls.Add("btnImgCopyAddressContact");
                    }

                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_CONTACTADDRESS_ADDRESS");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("txtaContactAddress");
                        disabledClientControls.Add("lblContactAddress");
                        hiddenClientControls.Add("btnImgCopyAddressContact");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("txtaContactAddress");
                        hiddenClientControls.Add("lblContactAddress");
                        hiddenClientControls.Add("btnImgCopyAddressContact");
                    }

                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_IDCARDNUMBER");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("lblIDCardNumber");
                        disabledClientControls.Add("txtIDCardNumber");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblIDCardNumber");
                        hiddenClientControls.Add("txtIDCardNumber");
                    }

                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_IDCARDISSUEDBY");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("lblIDCardIssuedBy");
                        disabledClientControls.Add("txtIDCardIssuedBy");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblIDCardIssuedBy");
                        hiddenClientControls.Add("txtIDCardIssuedBy");
                    }

                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_IDCARDISSUEDATE");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("lblIDCardIssueDate");
                        disabledClientControls.Add("txtIDCardIssueDate");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblIDCardIssueDate");
                        hiddenClientControls.Add("spanIDCardIssueDate");
                    }

                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_HOMEPHONE");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("txtHomePhone");
                        disabledClientControls.Add("lblHomePhone");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("txtHomePhone");
                        hiddenClientControls.Add("lblHomePhone");
                    }

                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_MOBILEPHONE");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("txtMobilePhone");
                        disabledClientControls.Add("lblMobilePhone");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("txtMobilePhone");
                        hiddenClientControls.Add("lblMobilePhone");
                    }

                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EMAIL");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("txtEmail");
                        disabledClientControls.Add("lblEmail");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("txtEmail");
                        hiddenClientControls.Add("lblEmail");
                    }

                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_DRIVINGLICENCE");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("pickListDrvLicCategories_txtSelected");
                        disabledClientControls.Add("lblDrvLicCategories");
                        //not disable but hide image button
                        hiddenClientControls.Add("pickListDrvLicCategories_imgDropDown");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        //In this case we hide div element
                        hiddenClientControls.Add("tdPickListDrvLicCategories");
                        hiddenClientControls.Add("lblDrvLicCategories");
                    }


                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_HASMILITARYSERVICE");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("hasMilitarySrv1");
                        disabledClientControls.Add("hasMilitarySrv2");

                        disabledClientControls.Add("lblWentToMilitary");
                        disabledClientControls.Add("lblWentToMilitaryYes");
                        disabledClientControls.Add("lblWentToMilitaryNo");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("hasMilitarySrv1");
                        hiddenClientControls.Add("hasMilitarySrv2");

                        hiddenClientControls.Add("lblWentToMilitary");
                        hiddenClientControls.Add("lblWentToMilitaryYes");
                        hiddenClientControls.Add("lblWentToMilitaryNo");
                    }

                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_MILITARYTRAINING");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("militaryTraining1");
                        disabledClientControls.Add("militaryTraining2");

                        disabledClientControls.Add("lblMilitaryTraining");
                        disabledClientControls.Add("lblMilitaryTraining1");
                        disabledClientControls.Add("lblMilitaryTraining2");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("militaryTraining1");
                        hiddenClientControls.Add("militaryTraining2");

                        hiddenClientControls.Add("lblMilitaryTraining");
                        hiddenClientControls.Add("lblMilitaryTraining1");
                        hiddenClientControls.Add("lblMilitaryTraining2");
                    }

                }

                l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_COMMENTS");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("txtaComment");
                    disabledClientControls.Add("lblComment");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("txtaComment");
                    hiddenClientControls.Add("lblComment");
                }

                l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_SERVICETYPE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblServiceType");
                    disabledClientControls.Add("lblSelectedServiceType");
                    disabledClientControls.Add("ddServiceTypes_AvailableOptions");
                    disabledClientControls.Add("ddServiceTypes_AssignedOptions");
                    hiddenClientControls.Add("btnSelectServiceTypes");
                    hiddenClientControls.Add("btnRemoveServiceTypes");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblServiceType");
                    hiddenClientControls.Add("lblSelectedServiceType");
                    hiddenClientControls.Add("ddServiceTypes_AvailableOptions");
                    hiddenClientControls.Add("ddServiceTypes_AssignedOptions");
                    hiddenClientControls.Add("btnSelectServiceTypes");
                    hiddenClientControls.Add("btnRemoveServiceTypes");
                }

                l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_MilitaryTrainingCourse");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblMilitaryTrainingCourse");
                    disabledClientControls.Add("lblSelectedMilitaryTrainingCourse");
                    disabledClientControls.Add("ddMTC_AvailableOptions");
                    disabledClientControls.Add("ddMTC_AssignedOptions");
                    hiddenClientControls.Add("btnSelectMTC");
                    hiddenClientControls.Add("btnRemoveMTC");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblMilitaryTrainingCourse");
                    hiddenClientControls.Add("lblSelectedMilitaryTrainingCourse");
                    hiddenClientControls.Add("ddMTC_AvailableOptions");
                    hiddenClientControls.Add("ddMTC_AssignedOptions");
                    hiddenClientControls.Add("btnSelectMTC");
                    hiddenClientControls.Add("btnRemoveMTC");
                }

                l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_LASTAPPERIANCE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblLastApperiance");
                    disabledClientControls.Add(txtLastApperiance.ClientID);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("divLastApperiance");
                }

                l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU");
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("divEducation");
                    hiddenClientControls.Add("divLanguage");
                }
                else
                {
                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU_EDU");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("divEducation");
                    }

                    l = this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL_EDU_LANG");
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("divLanguage");
                    }
                }
            }
        }

        private bool IsRegistred()
        {
            bool isAlreadyRegistred = false;
            if (!String.IsNullOrEmpty(Request.Params["PotencialApplicantId"]))
            {
                isAlreadyRegistred = true;
            }
            else
            {
                string identNumber = "";
                if (!String.IsNullOrEmpty(Request.Params["IdentNumber"]))
                {
                    identNumber = Request.Params["IdentNumber"];
                }

                Person person = PersonUtil.GetPersonByIdentNumber(identNumber, CurrentUser);

                if (person == null)
                {
                    int personId = 0;
                    if (!String.IsNullOrEmpty(Request.Params["PersonId"]))
                    {
                        int.TryParse(Request.Params["PersonId"], out personId);
                    }

                    person = PersonUtil.GetPerson(personId, CurrentUser);
                }

                if (person == null)
                {
                    //We have a brand new Person and Potencial Applicant
                    isAlreadyRegistred = false;
                }
                else
                {
                    int militaryDepartmentID = 0;
                    int.TryParse(Request.Params["MilitaryDepartmentId"], out militaryDepartmentID);

                    //PotencialApplicant potencialApplicant = PotencialApplicantUtil.GetPotencialApplicant(person.PersonId, militaryDepartmentID, CurrentUser);
                    if (PotencialApplicantUtil.IsAlreadyRegistered(person.PersonId, militaryDepartmentID, CurrentUser))
                    {
                        //This person is already registred for this MilitaryService
                        isAlreadyRegistred = true;
                    }
                }
            }

            return isAlreadyRegistred;
        }

        private int GetPersonId()
        {
            int personId = 0;
            if (!String.IsNullOrEmpty(Request.Params["PotencialApplicantId"]))
            {
                int pApplicantId = int.Parse(Request.Params["PotencialApplicantId"]);

                PotencialApplicant pApplicant = PotencialApplicantUtil.GetPotencialApplicant(pApplicantId, CurrentUser);
                if (pApplicant != null)
                {
                    personId = pApplicant.PersonId;
                }
            }
            else
            {
                string identNumber = "";
                if (!String.IsNullOrEmpty(Request.Params["IdentNumber"]))
                {
                    identNumber = Request.Params["IdentNumber"];

                    Person person = PersonUtil.GetPersonByIdentNumber(identNumber, CurrentUser);

                    if (person != null)
                    {
                        personId = person.PersonId;
                    }
                }
            }

            return personId;
        }

        //Create list with disabledClientControls
        private string SetListDisabledControls()
        {
            string result = "";

            foreach (string s in disabledClientControls)
            {
                result += "," + s;
            }
            return result;

        }

        //Create list with hiddenClientControls
        private string SetListHiddenControls()
        {
            string result = "";

            foreach (string s in hiddenClientControls)
            {
                result += "," + s;
            }
            return result;
        }

        //Get the Municipalities for a particular Region (ajax call)
        private void JSRepopulateMunicipality()
        {
            string stat = "";
            string response = "";

            try
            {
                int regionId = 0;

                if (!String.IsNullOrEmpty(Request.Form["RegionId"]))
                    regionId = int.Parse(Request.Form["RegionId"]);

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

        //Populate the Cities when changing the Municipality (ajax call)
        private void JSRepopulateCity()
        {
            string stat = "";
            string response = "";

            try
            {
                int municipalityId = 0;

                if (!String.IsNullOrEmpty(Request.Form["MunicipalityId"]))
                    municipalityId = int.Parse(Request.Form["MunicipalityId"]);

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

        //Populate the PostCode when changing the City (ajax call)
        private void JSRepopulatePostCode()
        {
            string stat = "";
            string response = "";

            try
            {
                int cityId = 0;

                if (!String.IsNullOrEmpty(Request.Form["CityId"]))
                    cityId = int.Parse(Request.Form["CityId"]);

                string postCode = "";

                if (cityId != 0 && cityId != int.Parse(ListItems.GetOptionChooseOne().Value))
                {
                    City city = CityUtil.GetCity(cityId, CurrentUser);
                    postCode = city.PostCode.ToString();
                }

                stat = AJAXTools.OK;
                response = "<postCode>" + AJAXTools.EncodeForXML(postCode) + "</postCode>";
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

        //Populate the Region, the Municipalityand the City when changing the PostCode (ajax call)
        private void JSRepopulateRegionMunicipalityCity()
        {
            string stat = "";
            string response = "";

            try
            {
                int postCode = 0;

                if (!String.IsNullOrEmpty(Request.Form["PostCode"]))
                {
                    try
                    {
                        postCode = int.Parse(Request.Form["PostCode"]);
                    }
                    catch
                    {
                        postCode = 0;
                    }
                }

                //District district = DistrictUtil.GetDistrictByPostCode(postCode.ToString(), CurrentUser);
                City foundCity = null;
                foundCity = CityUtil.GetCityByPostCode(postCode, CurrentUser);

                if (postCode > 0 && foundCity != null)
                {
                    List<Municipality> municipalities = MunicipalityUtil.GetMunicipalities(foundCity.RegionId, CurrentUser);
                    List<City> cities = CityUtil.GetCities(foundCity.MunicipalityId, CurrentUser);

                    foreach (Municipality municipality in municipalities)
                    {
                        response += "<m>" +
                                    "<id>" + municipality.MunicipalityId.ToString() + "</id>" +
                                    "<name>" + AJAXTools.EncodeForXML(municipality.MunicipalityName) + "</name>" +
                                    "</m>";
                    }

                    foreach (City city in cities)
                    {
                        response += "<c>" +
                                    "<id>" + city.CityId.ToString() + "</id>" +
                                    "<name>" + AJAXTools.EncodeForXML(city.CityName) + "</name>" +
                                    "</c>";
                    }

                    response += "<cityId>" + foundCity.CityId.ToString() + @"</cityId>" +
                                "<municipalityId>" + foundCity.MunicipalityId.ToString() + @"</municipalityId>" +
                                "<regionId>" + foundCity.RegionId.ToString() + @"</regionId>";
                }
                else
                {
                    response = "<cityId>0</cityId>";
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

        //Populate the PostCode and District when changing the City (ajax call)
        private void JSRepopulatePostCodeAndDistrict()
        {
            string stat = "";
            string response = "";

            try
            {
                int cityId = 0;

                if (!String.IsNullOrEmpty(Request.Form["CityId"]))
                    cityId = int.Parse(Request.Form["CityId"]);

                string cityPostCode = "";
                string districts = "<districts>" +
                                   "<d>" +
                                   "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                                   "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                                   "</d>";

                if (cityId != 0 && cityId != int.Parse(ListItems.GetOptionChooseOne().Value))
                {
                    City city = CityUtil.GetCity(cityId, CurrentUser);
                    cityPostCode = city.PostCode.ToString();

                    foreach (District district in city.Districts)
                    {
                        districts += "<d>" +
                                     "<id>" + district.DistrictId.ToString() + "</id>" +
                                     "<name>" + AJAXTools.EncodeForXML(district.DistrictName) + "</name>" +
                                     "</d>";
                    }
                }

                districts += "</districts>";

                stat = AJAXTools.OK;
                response = "<cityPostCode>" + AJAXTools.EncodeForXML(cityPostCode) + "</cityPostCode>" +
                           districts;
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

        //Populate the PostCode when changing the District (ajax call)
        private void JSRepopulateDistrictPostCode()
        {
            string stat = "";
            string response = "";

            try
            {
                int districtId = 0;

                if (!String.IsNullOrEmpty(Request.Form["DistrictId"]))
                    districtId = int.Parse(Request.Form["DistrictId"]);

                string postCode = "";

                if (districtId != 0 && districtId != int.Parse(ListItems.GetOptionChooseOne().Value))
                {
                    District district = DistrictUtil.GetDistrict(districtId, CurrentUser);
                    postCode = district.PostCode;

                    if (postCode == "")
                    {
                        int cityId = 0;
                        if (!String.IsNullOrEmpty(Request.Form["CityId"]))
                            cityId = int.Parse(Request.Form["CityId"]);

                        if (cityId != 0 && cityId != int.Parse(ListItems.GetOptionChooseOne().Value))
                        {
                            City city = CityUtil.GetCity(cityId, CurrentUser);
                            postCode = city.PostCode.ToString();
                        }
                    }
                }

                stat = AJAXTools.OK;
                response = "<districtPostCode>" + AJAXTools.EncodeForXML(postCode) + "</districtPostCode>";
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

        //Populate the Region, the Municipalityand, the City and the District when changing the PostCode (ajax call)
        private void JSRepopulateRegionMunicipalityCityDistrict()
        {
            string stat = "";
            string response = "";

            try
            {
                int postCode = 0;

                if (!String.IsNullOrEmpty(Request.Form["PostCode"]))
                {
                    try
                    {
                        postCode = int.Parse(Request.Form["PostCode"]);
                    }
                    catch
                    {
                        postCode = 0;
                    }
                }

                City foundCity = null;

                foundCity = CityUtil.GetCityByPostCode(postCode, CurrentUser);

                if (postCode > 0 && foundCity != null)
                {
                    List<Municipality> municipalities = MunicipalityUtil.GetMunicipalities(foundCity.RegionId, CurrentUser);
                    List<City> cities = CityUtil.GetCities(foundCity.MunicipalityId, CurrentUser);
                    List<District> districts = foundCity.Districts;

                    foreach (Municipality municipality in municipalities)
                    {
                        response += "<m>" +
                                    "<id>" + municipality.MunicipalityId.ToString() + "</id>" +
                                    "<name>" + AJAXTools.EncodeForXML(municipality.MunicipalityName) + "</name>" +
                                    "</m>";
                    }

                    foreach (City city in cities)
                    {
                        response += "<c>" +
                                    "<id>" + city.CityId.ToString() + "</id>" +
                                    "<name>" + AJAXTools.EncodeForXML(city.CityName) + "</name>" +
                                    "</c>";
                    }

                    response += "<d>" +
                                "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                                "</d>";

                    foreach (District district in districts)
                    {
                        response += "<d>" +
                                    "<id>" + district.DistrictId.ToString() + "</id>" +
                                    "<name>" + AJAXTools.EncodeForXML(district.DistrictName) + "</name>" +
                                    "</d>";
                    }

                    response += "<districtId></districtId>" +
                                "<cityId>" + foundCity.CityId.ToString() + @"</cityId>" +
                                "<municipalityId>" + foundCity.MunicipalityId.ToString() + @"</municipalityId>" +
                                "<regionId>" + foundCity.RegionId.ToString() + @"</regionId>";
                }
                else
                {
                    response = "<cityId>0</cityId>";
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
    }
}

