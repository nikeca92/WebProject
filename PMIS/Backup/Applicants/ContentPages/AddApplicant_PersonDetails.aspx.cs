using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;
using System.Text;

using PMIS.Common;
using PMIS.Applicants.Common;

namespace PMIS.Applicants.ContentPages
{
    public partial class AddApplicant_PersonDetails : APPLPage
    {
        string responseGo = "";
        string responseBack = "";
        bool isRegistred;

        //Use this for disable/hide UI client controls
        List<string> disabledClientControls = new List<string>();
        List<string> hiddenClientControls = new List<string>();

        UIAccessLevel l;


        public override string PageUIKey
        {
            get
            {
                return "APPL_APPL";
            }
        }

        public string DatePickerCSS { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if (GetUIItemAccessLevel("APPL_APPL") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            //Set the Request parameters
            if (!String.IsNullOrEmpty(Request.Params["IdentNumber"]))
                hdnIdentNumber.Value = Request.Params["IdentNumber"];

            if (!String.IsNullOrEmpty(Request.Params["MilitaryDepartmentId"]))
            {
                hdnMilitaryDepartmentID.Value = Request.Params["MilitaryDepartmentId"];

                string militaryDepartmentName = MilitaryDepartmentUtil.GetMilitaryDepartment(int.Parse(hdnMilitaryDepartmentID.Value), CurrentUser).MilitaryDepartmentName;
                spanMilitaryDepartmentName.InnerText = militaryDepartmentName;
            }

            if (!String.IsNullOrEmpty(Request.Params["PersonId"]))
                hdnPersonID.Value = Request.Params["PersonId"];


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
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadMedCert")
            {
                JSLoadMedCert();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveMedCert")
            {
                JSSaveMedCert();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteMedCert")
            {
                JSDeleteMedCert();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadPsychCert")
            {
                JSLoadPsychCert();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSavePsychCert")
            {
                JSSavePsychCert();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeletePsychCert")
            {
                JSDeletePsychCert();
                return;
            }

            //Hilight the correct item in the menu
            HighlightMenuItems("Applicants", "Applicants_Add");

            //Hide the navigation buttons
            HideNavigationControls(btnBack);

            //Hide the helper button
            btnGo.Style.Add("display", "none");


            DatePickerCSS = CommonFunctions.DatePickerCSS();
            //Populate the drop-downs
            PopulateDropdowns();

            //Set Titles if need  comeFrom=EditAppl
            if (Request.Params["PageFrom"] != null)
            {
                lblHeaderSubTitle.InnerText = "Редактиране на лични данни";
                lblGo.InnerText = "Запис";
                lblBack.InnerText = "Отказ";
                responseGo = "~/ContentPages/EditApplicant.aspx?PersonId=" + hdnPersonID.Value + "&MilitaryDepartmentId=" + hdnMilitaryDepartmentID.Value + "&PageFrom=" + Request.Params["PageFrom"];
                //We have the same redirect. Difference is that btnGo was perfomde Ajax save
                responseBack = responseGo;
            }
            else
            {
                lblHeaderSubTitle.InnerText = "Въвеждане на лични данни";
                lblGo.InnerText = "Продължи";
                lblBack.InnerText = "Назад";
                responseGo = "~/ContentPages/EditApplicant.aspx?PersonId=" + hdnPersonID.Value + "&MilitaryDepartmentId=" + hdnMilitaryDepartmentID.Value;
                responseBack = "~/ContentPages/AddApplicant_SelectPerson.aspx";
            }
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

        protected void btnGo_Click(object sender, EventArgs e)
        {
            //Ajax was Saved Data and now just redirect to other page
            Response.Redirect(responseGo);
        }

        //Load Person details (ajax call)
        private void JSLoadPersonDetails()
        {
            //if (GetUIItemAccessLevel("APPL_APPL") != UIAccessLevel.Enabled)
            //    RedirectAjaxAccessDenied();

            isRegistred = IsReqistred();
            this.SetupPageUIClientControls(isRegistred);

            //string disabledControlsList = "<disabledControlsList>" + SetListDisabledControls() + "</disabledControlsList>";
            //string hiddenControlsList = "<hiddenControlsList>" + SetListHiddenControls() + "</hiddenControlsList>";

            string identNumber = Request.Form["IdentNumber"];

            string stat = "";
            string response = "";

            try
            {
                Person person = PersonUtil.GetPersonByIdentNumber(identNumber, CurrentUser);
                PersonStatus personStatus = null;

                stat = AJAXTools.OK;

                //Existing Person: Load him details
                if (person != null)
                {
                    personStatus = PersonUtil.GetPersonStatusByPerson(person, CurrentUser);

                    string drivingLicenseCategories = "";

                    foreach (DrivingLicenseCategory category in person.DrivingLicenseCategories)
                    {
                        drivingLicenseCategories += (drivingLicenseCategories == "" ? "" : ",") +
                            category.DrivingLicenseCategoryId.ToString();
                    }

                    string medCertTable = GetMedCertTable(person.PersonId, CurrentUser);
                    string medCertLightBox = GetMedCertLightBox(CurrentUser);

                    string psychCertTable = GetPsychCertTable(person.PersonId, CurrentUser);
                    string psychCertLightBox = GetPsychCertLightBox(CurrentUser);

                    response = @"
                        <person>
                            <personId>" + AJAXTools.EncodeForXML(person.PersonId.ToString()) + @"</personId>
                            <firstName>" + AJAXTools.EncodeForXML(person.FirstName) + @"</firstName>
                            <lastName>" + AJAXTools.EncodeForXML(person.LastName) + @"</lastName>
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
                            <birthCountryId>" + AJAXTools.EncodeForXML(person.BirthCountry != null ? person.BirthCountry.CountryId : "") + @"</birthCountryId>
                            <birthCityId>" + AJAXTools.EncodeForXML(person.BirthCityId != null ? person.BirthCityId.ToString() : "") + @"</birthCityId>
                            <birthCityIfAbroad>" + AJAXTools.EncodeForXML(person.BirthCityIfAbroad != null ? person.BirthCityIfAbroad : "") + @"</birthCityIfAbroad>";
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
                    response += @"</PersonStatus>
                                  <medCertTableHTML>" + AJAXTools.EncodeForXML(medCertTable) + @"</medCertTableHTML>
                                  <medCertLightBoxHTML>" + AJAXTools.EncodeForXML(medCertLightBox) + @"</medCertLightBoxHTML>
                                  <psychCertTableHTML>" + AJAXTools.EncodeForXML(psychCertTable) + @"</psychCertTableHTML>
                                  <psychCertLightBoxHTML>" + AJAXTools.EncodeForXML(psychCertLightBox) + @"</psychCertLightBoxHTML>";
                    response += "</person>";

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
                            <birthCountryId></birthCountryId>
                            <birthCityId></birthCityId>
                            <birthCityIfAbroad></birthCityIfAbroad>";

                    response += "<PersonStatus>";
                    response += "<PersonStatus_Status>" + AJAXTools.EncodeForXML(personStatus.Status) + "</PersonStatus_Status>";
                    response += "<PersonStatus_Details></PersonStatus_Details>";
                    response += "</PersonStatus>";
                    response += @"<medCertTableHTML>" + AJAXTools.EncodeForXML("<span>Моля, запишете личните данните на новия кандидат, за да може да въведете медицинско освидетелстване.</span>") + @"</medCertTableHTML>
                                  <medCertLightBoxHTML></medCertLightBoxHTML>
                                  <psychCertTableHTML>" + AJAXTools.EncodeForXML("<span>Моля, запишете личните данните на новия кандидат, за да може да въведете психологическа пригодност.</span>") + @"</psychCertTableHTML>
                                  <psychCertLightBoxHTML></psychCertLightBoxHTML>";
                    response += "</person>";
                }
            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }

            response += "<listDisabledControls>" + SetListDisabledControls() + @"</listDisabledControls>
                         <listHiddenControls>" + SetListHiddenControls() + @"</listHiddenControls>";


            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        //Save Person details (ajax call)
        private void JSSavePersonDetails()
        {
            //if (GetUIItemAccessLevel("APPL_Applicants") != UIAccessLevel.Enabled)
            //    RedirectAjaxAccessDenied();

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

            //if (!String.IsNullOrEmpty(drivingLicenseCategories))
            //{
            //    string[] categories = drivingLicenseCategories.Split(',');
            //    for (int i = 0; i < categories.Length; i++)
            //    {
            //        int drvLicCategoryId = int.Parse(categories[i]);
            //        DrivingLicenseCategory category = new DrivingLicenseCategory(CurrentUser);
            //        category.DrivingLicenseCategoryId = drvLicCategoryId;
            //        person.DrivingLicenseCategories.Add(category);
            //    }
            //}


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
                Change change = new Change(CurrentUser, "APPL_Applicants");


                //Save the changes in table Person
                if (person.PersonId == 0)
                {
                    PersonUtil.SavePerson_WhenAddingNewApplicant(person, "ADM_PersonDetails_Add", CurrentUser, change);
                }
                else
                {
                    PersonUtil.SavePerson_WhenAddingNewApplicant(person, "ADM_PersonDetails_Edit", CurrentUser, change);
                }

                //Write into the Audit Trail
                change.WriteLog();

                stat = AJAXTools.OK;
                response = @"<response>OK</response>
                             <personId>" + AJAXTools.EncodeForXML(person.PersonId.ToString()) + @"</personId>";
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

        //Manage UI logic
        private void SetupPageUIClientControls(bool isRegistred)
        {

            if (!isRegistred) //Mode Add
            {
                bool screenHidden = (this.GetUIItemAccessLevel("APPL_APPL_ADDAPPL") != UIAccessLevel.Enabled) ||
                                      (this.GetUIItemAccessLevel("APPL_APPL") != UIAccessLevel.Enabled);

                bool screenDisabled = (this.GetUIItemAccessLevel("APPL_APPL_ADDAPPL") == UIAccessLevel.Disabled) ||
                                      (this.GetUIItemAccessLevel("APPL_APPL") == UIAccessLevel.Disabled);

                if (screenHidden)
                    RedirectAccessDenied();

                if (screenDisabled)
                {
                    this.pageDisabledControls.Add(btnSave);
                }

                //Enable/Disable Client Controls

                disabledClientControls = new List<string>();
                hiddenClientControls = new List<string>();

                //Chack for whole page person details

                l = this.GetUIItemAccessLevel("APPL_APPL_ADD_PERSONDETAILS");
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
                    l = this.GetUIItemAccessLevel("APPL_APPL_ADD_APPL_FIRSTNAME");
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

                    l = this.GetUIItemAccessLevel("APPL_APPL_ADD_APPL_LASTNAME");
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


                    l = this.GetUIItemAccessLevel("APPL_APPL_ADD_APPL_GENDER");
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

                    l = this.GetUIItemAccessLevel("APPL_APPL_ADD_APPL_PERMADDRESS_CITY");
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
                        disabledClientControls.Add("lblPermCity");
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

                    l = this.GetUIItemAccessLevel("APPL_APPL_ADD_APPL_PERMADDRESS_ADDRESS");
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


                    l = this.GetUIItemAccessLevel("APPL_APPL_ADD_APPL_PRESADDRESS_CITY");
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

                    l = this.GetUIItemAccessLevel("APPL_APPL_ADD_APPL_PRESADDRESS_ADDRESS");
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

                    l = this.GetUIItemAccessLevel("APPL_APPL_ADD_APPL_CONTACTADDRESS_CITY");
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

                    l = this.GetUIItemAccessLevel("APPL_APPL_ADD_APPL_CONTACTADDRESS_ADDRESS");
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

                    l = this.GetUIItemAccessLevel("APPL_APPL_ADD_APPL_IDCARDNUMBER");
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

                    l = this.GetUIItemAccessLevel("APPL_APPL_ADD_APPL_IDCARDISSUEDBY");
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

                    l = this.GetUIItemAccessLevel("APPL_APPL_ADD_APPL_IDCARDISSUEDATE");
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

                    l = this.GetUIItemAccessLevel("APPL_APPL_ADD_APPL_HOMEPHONE");
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

                    l = this.GetUIItemAccessLevel("APPL_APPL_ADD_APPL_MOBILEPHONE");
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

                    l = this.GetUIItemAccessLevel("APPL_APPL_ADD_APPL_EMAIL");
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

                    l = this.GetUIItemAccessLevel("APPL_APPL_ADD_APPL_DRIVINGLICENCE");
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


                    l = this.GetUIItemAccessLevel("APPL_APPL_ADD_APPL_HASMILITARYSERVICE");
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

                    l = this.GetUIItemAccessLevel("APPL_APPL_ADD_APPL_MILITARYTRAINING");
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
            }
            else // edit mode of page
            {
                bool screenHidden = (this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL") == UIAccessLevel.Hidden) ||
                                      (this.GetUIItemAccessLevel("APPL_APPL") == UIAccessLevel.Hidden);

                bool screenDisabled = (this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL") == UIAccessLevel.Disabled) ||
                                      (this.GetUIItemAccessLevel("APPL_APPL") == UIAccessLevel.Disabled);

                if (screenHidden)
                    RedirectAccessDenied();

                if (screenDisabled)
                {
                    this.pageDisabledControls.Add(btnSave);
                }

                //Enable/Disable Client Controls

                disabledClientControls = new List<string>();
                hiddenClientControls = new List<string>();

                l = this.GetUIItemAccessLevel("APPL_APPL_EDIT_PERSONDETAILS");
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

                    disabledClientControls.Add("lblMedCertDate");
                    disabledClientControls.Add("txtMedCertDate");

                    disabledClientControls.Add("lblMedCertProtNum");
                    disabledClientControls.Add("txtMedCertProtNum");

                    disabledClientControls.Add("lblMedCertConclusion");
                    disabledClientControls.Add("ddMedCertConclusion");

                    disabledClientControls.Add("lblMedCertMedRubric");
                    disabledClientControls.Add("ddMedCertMedRubric");

                    disabledClientControls.Add("lblMedCertExpirationDate");
                    disabledClientControls.Add("txtMedCertExpirationDate");

                    disabledClientControls.Add("lblPsychCertDate");
                    disabledClientControls.Add("txtPsychCertDate");

                    disabledClientControls.Add("lblPsychCertProtNum");
                    disabledClientControls.Add("txtPsychCertProtNum");

                    disabledClientControls.Add("lblPsychCertConclusion");
                    disabledClientControls.Add("ddPsychCertConclusion");

                    disabledClientControls.Add("lblPsychCertExpirationDate");
                    disabledClientControls.Add("txtPsychCertExpirationDate");
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

                    hiddenClientControls.Add("divMedCertSection");
                    hiddenClientControls.Add("divPsychCertSection");
                }
                else
                {
                    l = this.GetUIItemAccessLevel("APPL_APPL_EDIT_APPL_FIRSTNAME");
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

                    l = this.GetUIItemAccessLevel("APPL_APPL_EDIT_APPL_LASTNAME");
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


                    l = this.GetUIItemAccessLevel("APPL_APPL_EDIT_APPL_GENDER");
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


                    l = this.GetUIItemAccessLevel("APPL_APPL_EDIT_APPL_PERMADDRESS_CITY");
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
                        disabledClientControls.Add("lblPermCity");
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

                    l = this.GetUIItemAccessLevel("APPL_APPL_EDIT_APPL_PERMADDRESS_ADDRESS");
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


                    l = this.GetUIItemAccessLevel("APPL_APPL_EDIT_APPL_PRESADDRESS_CITY");
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

                    l = this.GetUIItemAccessLevel("APPL_APPL_EDIT_APPL_PRESADDRESS_ADDRESS");
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

                    l = this.GetUIItemAccessLevel("APPL_APPL_EDIT_APPL_CONTACTADDRESS_CITY");
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

                    l = this.GetUIItemAccessLevel("APPL_APPL_EDIT_APPL_CONTACTADDRESS_ADDRESS");
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

                    l = this.GetUIItemAccessLevel("APPL_APPL_EDIT_APPL_IDCARDNUMBER");
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

                    l = this.GetUIItemAccessLevel("APPL_APPL_EDIT_APPL_IDCARDISSUEDBY");
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

                    l = this.GetUIItemAccessLevel("APPL_APPL_EDIT_APPL_IDCARDISSUEDATE");
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

                    l = this.GetUIItemAccessLevel("APPL_APPL_EDIT_APPL_HOMEPHONE");
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

                    l = this.GetUIItemAccessLevel("APPL_APPL_EDIT_APPL_MOBILEPHONE");
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

                    l = this.GetUIItemAccessLevel("APPL_APPL_EDIT_APPL_EMAIL");
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

                    l = this.GetUIItemAccessLevel("APPL_APPL_EDIT_APPL_DRIVINGLICENCE");
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


                    l = this.GetUIItemAccessLevel("APPL_APPL_EDIT_APPL_HASMILITARYSERVICE");
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

                    l = this.GetUIItemAccessLevel("APPL_APPL_EDIT_APPL_MILITARYTRAINING");
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

                    bool sectionDisabled = (this.GetUIItemAccessLevel("APPL_APPL_EDIT_PERSONDETAILS_MEDCERT") == UIAccessLevel.Disabled);
                    bool sectionHidden = (this.GetUIItemAccessLevel("APPL_APPL_EDIT_PERSONDETAILS_MEDCERT") == UIAccessLevel.Hidden);

                    if (sectionHidden)
                        hiddenClientControls.Add("divMedCertSection");

                    l = this.GetUIItemAccessLevel("APPL_APPL_EDIT_APPL_MEDCERT_DATE");
                    if (l == UIAccessLevel.Disabled || screenDisabled || sectionDisabled)
                    {
                        disabledClientControls.Add("lblMedCertDate");
                        disabledClientControls.Add("txtMedCertDate");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblMedCertDate");
                        hiddenClientControls.Add("spanMedCertDate");
                    }

                    l = this.GetUIItemAccessLevel("APPL_APPL_EDIT_APPL_MEDCERT_PROTNUM");
                    if (l == UIAccessLevel.Disabled || screenDisabled || sectionDisabled)
                    {
                        disabledClientControls.Add("lblMedCertProtNum");
                        disabledClientControls.Add("txtMedCertProtNum");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblMedCertProtNum");
                        hiddenClientControls.Add("txtMedCertProtNum");
                    }

                    l = this.GetUIItemAccessLevel("APPL_APPL_EDIT_APPL_MEDCERT_CONCLUSION");
                    if (l == UIAccessLevel.Disabled || screenDisabled || sectionDisabled)
                    {
                        disabledClientControls.Add("lblMedCertConclusion");
                        disabledClientControls.Add("ddMedCertConclusion");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblMedCertConclusion");
                        hiddenClientControls.Add("ddMedCertConclusion");
                    }

                    l = this.GetUIItemAccessLevel("APPL_APPL_EDIT_APPL_MEDCERT_MEDRUBRIC");
                    if (l == UIAccessLevel.Disabled || screenDisabled || sectionDisabled)
                    {
                        disabledClientControls.Add("lblMedCertMedRubric");
                        disabledClientControls.Add("ddMedCertMedRubric");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblMedCertMedRubric");
                        hiddenClientControls.Add("ddMedCertMedRubric");
                    }

                    l = this.GetUIItemAccessLevel("APPL_APPL_EDIT_APPL_MEDCERT_EXPIRATIONDATE");
                    if (l == UIAccessLevel.Disabled || screenDisabled || sectionDisabled)
                    {
                        disabledClientControls.Add("lblMedCertExpirationDate");
                        disabledClientControls.Add("txtMedCertExpirationDate");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblMedCertExpirationDate");
                        hiddenClientControls.Add("spanMedCertExpirationDate");
                    }

                    //section psychological certificate
                    sectionDisabled = (this.GetUIItemAccessLevel("APPL_APPL_EDIT_PERSONDETAILS_PSYCHCERT") == UIAccessLevel.Disabled);
                    sectionHidden = (this.GetUIItemAccessLevel("APPL_APPL_EDIT_PERSONDETAILS_PSYCHCERT") == UIAccessLevel.Hidden);

                    if (sectionHidden)
                        hiddenClientControls.Add("divPsychCertSection");

                    l = this.GetUIItemAccessLevel("APPL_APPL_EDIT_APPL_PSYCHCERT_DATE");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("lblPsychCertDate");
                        disabledClientControls.Add("txtPsychCertDate");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblPsychCertDate");
                        hiddenClientControls.Add("spanPsychCertDate");
                    }

                    l = this.GetUIItemAccessLevel("APPL_APPL_EDIT_APPL_PSYCHCERT_PROTNUM");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("lblPsychCertProtNum");
                        disabledClientControls.Add("txtPsychCertProtNum");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblPsychCertProtNum");
                        hiddenClientControls.Add("txtPsychCertProtNum");
                    }

                    l = this.GetUIItemAccessLevel("APPL_APPL_EDIT_APPL_PSYCHCERT_CONCLUSION");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("lblPsychCertConclusion");
                        disabledClientControls.Add("ddPsychCertConclusion");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblPsychCertConclusion");
                        hiddenClientControls.Add("ddPsychCertConclusion");
                    }

                    l = this.GetUIItemAccessLevel("APPL_APPL_EDIT_APPL_PSYCHCERT_EXPIRATIONDATE");
                    if (l == UIAccessLevel.Disabled || screenDisabled)
                    {
                        disabledClientControls.Add("lblPsychCertExpirationDate");
                        disabledClientControls.Add("txtPsychCertExpirationDate");
                    }
                    if (l == UIAccessLevel.Hidden)
                    {
                        hiddenClientControls.Add("lblPsychCertExpirationDate");
                        hiddenClientControls.Add("spanPsychCertExpirationDate");
                    }
                }
            }
        }

        //Chek is Person is already registred as apllicant
        private bool IsReqistred()
        {
            bool isRegistred = false;
            if (!String.IsNullOrEmpty(Request.Params["PersonId"]))
            {
                int personId = 0;
                int.TryParse(Request.Params["PersonId"], out personId);
                if (personId > 0)
                {
                    return true;
                }
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
                    //We have a brand new Person and Applicant
                    return false;
                }

                int militaryDepartmentID = 0;
                int.TryParse(this.hdnMilitaryDepartmentID.Value, out militaryDepartmentID);

                if (ApplicantUtil.IsAlreadyRegistered(person.PersonId, militaryDepartmentID, CurrentUser))
                {
                    //This person is already registred for this MilitaryService
                    return true;
                }
            }
            return isRegistred;
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

        public string GetMedCertTable(int personID, User currentUser)
        {
            bool IsMedCertDateHidden = this.GetUIItemAccessLevel("APPL_APPL_EDIT_APPL_MEDCERT_DATE") == UIAccessLevel.Hidden;
            bool IsProtNumHidden = this.GetUIItemAccessLevel("APPL_APPL_EDIT_APPL_MEDCERT_PROTNUM") == UIAccessLevel.Hidden;
            bool IsConclusionHidden = this.GetUIItemAccessLevel("APPL_APPL_EDIT_APPL_MEDCERT_CONCLUSION") == UIAccessLevel.Hidden;
            bool IsMedRubricHidden = this.GetUIItemAccessLevel("APPL_APPL_EDIT_APPL_MEDCERT_MEDRUBRIC") == UIAccessLevel.Hidden;
            bool IsExpirationDateHidden = this.GetUIItemAccessLevel("APPL_APPL_EDIT_APPL_MEDCERT_EXPIRATIONDATE") == UIAccessLevel.Hidden;

            string newHTML = "";

            if (this.GetUIItemAccessLevel("APPL_APPL") == UIAccessLevel.Enabled &&
                this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL") == UIAccessLevel.Enabled &&
                this.GetUIItemAccessLevel("APPL_APPL_EDIT_PERSONDETAILS") == UIAccessLevel.Enabled &&
                this.GetUIItemAccessLevel("APPL_APPL_EDIT_PERSONDETAILS_MEDCERT") == UIAccessLevel.Enabled
            )
            {
                newHTML = "<img src='../Images/note_add.png' alt='Нов запис' title='Нов запис' class='btnNewTableRecordIcon' onclick='NewMedCert();' />";
            }

            StringBuilder tableHTML = new StringBuilder();

            tableHTML.Append(@"<table class='CommonHeaderTable' style='text-align: left;'>
                              <thead>
                                <tr>
                                   <th style='width: 20px; vertical-align: bottom;'>№</th>
     " + (!IsMedCertDateHidden ? @"<th style='width: 80px; vertical-align: bottom;'>Комисия от дата</th>" : "") + @"
         " + (!IsProtNumHidden ? @"<th style='width: 180px; vertical-align: bottom;'>Протокол</th>" : "") + @"                    
      " + (!IsConclusionHidden ? @"<th style='width: 80px; vertical-align: bottom;'>Решение</th>" : "") + @"
       " + (!IsMedRubricHidden ? @"<th style='width: 240px; vertical-align: bottom;'>Медицинска рубрика</th>" : "") + @"
  " + (!IsExpirationDateHidden ? @"<th style='width: 80px; vertical-align: bottom;'>Дата на валидност</th>" : "") + @"
                                   <th style='width: 50px;vertical-align: top;'>
                                      <div class='btnNewTableRecord'>" + newHTML + @"</div>
                                   </th>
                                </tr>
                              </thead>");

            int counter = 0;

            var personMedCerts = PersonMedCertUtil.GetAllPersonMedCerts(personID, this.CurrentUser);

            foreach (var personMedCert in personMedCerts)
            {
                counter++;

                string deleteHTML = "";

                if (personMedCert.CanDelete)
                {
                    if (this.GetUIItemAccessLevel("APPL_APPL") == UIAccessLevel.Enabled &&
                        this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL") == UIAccessLevel.Enabled &&
                        this.GetUIItemAccessLevel("APPL_APPL_EDIT_PERSONDETAILS") == UIAccessLevel.Enabled &&
                        this.GetUIItemAccessLevel("APPL_APPL_EDIT_PERSONDETAILS_MEDCERT") == UIAccessLevel.Enabled
                        )
                    {
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване' class='GridActionIcon' onclick='DeleteMedCert(" + personMedCert.MedCertID.ToString() + ");' />";
                    }
                }

                string editHTML = "";

                if (this.GetUIItemAccessLevel("APPL_APPL") == UIAccessLevel.Enabled &&
                    this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL") == UIAccessLevel.Enabled &&
                    this.GetUIItemAccessLevel("APPL_APPL_EDIT_PERSONDETAILS") == UIAccessLevel.Enabled &&
                    this.GetUIItemAccessLevel("APPL_APPL_EDIT_PERSONDETAILS_MEDCERT") == UIAccessLevel.Enabled
                    )
                {
                    editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='EditMedCert(" + personMedCert.MedCertID.ToString() + ");' />";

                }

                tableHTML.Append(@"<tr style='vertical-align: middle; height:20px; " + (counter == 1 ? "font-weight: bold;" : "") + @"' class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                     <td style='text-align: center;'>" + counter.ToString() + @"</td>
       " + (!IsMedCertDateHidden ? @"<td style='text-align: left;'>" + CommonFunctions.FormatDate(personMedCert.MedCertDate) + @"</td>" : "") + @"
           " + (!IsProtNumHidden ? @"<td style='text-align: left;'>" + personMedCert.ProtNum + @"</td>" : "") + @"                    
        " + (!IsConclusionHidden ? @"<td style='text-align: left;'>" + (personMedCert.Conclusion != null ? personMedCert.Conclusion.MilitaryMedicalConclusionName.ToString() : "") + @"</td>" : "") + @"
         " + (!IsMedRubricHidden ? @"<td style='text-align: left;'>" + (personMedCert.MedRubric != null ? personMedCert.MedRubric.MedicalRubricTitle.ToString() : "") + @"</td>" : "") + @"
    " + (!IsExpirationDateHidden ? @"<td style='text-align: left;'>" + CommonFunctions.FormatDate(personMedCert.ExpirationDate) + @"</td>" : "") + @"
                                     <td style='text-align: left;' nowrap>" + editHTML + deleteHTML + @"</td>
                                  </tr>
                             ");
            }

            tableHTML.Append("</table>");

            return tableHTML.ToString();
        }

        //Render the Med Cert light-box
        public static string GetMedCertLightBox(User currentUser)
        {
            string ddConclusionHtml = "";

            List<MilitaryMedicalConclusion> militaryMedicalConclusions = MilitaryMedicalConclusionUtil.GetAllMilitaryMedicalConclusions(currentUser);

            List<IDropDownItem> militaryMedicalConclusionsDropDownItems = new List<IDropDownItem>();
            foreach (MilitaryMedicalConclusion militaryMedicalConclusion in militaryMedicalConclusions)
                militaryMedicalConclusionsDropDownItems.Add(militaryMedicalConclusion as IDropDownItem);

            ddConclusionHtml = ListItems.GetDropDownHtml(militaryMedicalConclusionsDropDownItems, null, "ddMedCertConclusion", true, null, null, @"style=""width: 120px;""");

            string ddMedRubricHTML = "";

            List<MedicalRubric> medicalRubrics = MedicalRubricUtil.GetAllMedicalRubrics(currentUser);

            List<IDropDownItem> medicalRubricsDropDownItems = new List<IDropDownItem>();

            foreach (MedicalRubric medicalRubric in medicalRubrics)
                medicalRubricsDropDownItems.Add(medicalRubric as IDropDownItem);

            ddMedRubricHTML = ListItems.GetDropDownHtml(medicalRubricsDropDownItems, null, "ddMedCertMedRubric", true, null, null, @"style=""width: 120px;""");


            string html = @"
<div id=""divMedCertLightBox"" style=""display: none;"" class=""lboxMedCert"">
<center>
    <input type=""hidden"" id=""hdnMedCertID"" />
    <table width=""90%"" style=""text-align: center;"">
        <colgroup style=""width: 40%"">
        </colgroup>
        <colgroup style=""width: 60%"">
        </colgroup>
        <tr style=""height: 15px"">
        </tr>
        <tr>
            <td colspan=""2"" align=""center"">
                <span class=""HeaderText"" style=""text-align: center;"" id=""lblAddEditMedCertTitle""></span>
            </td>
        </tr>
        <tr style=""height: 15px"">
        </tr>  
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMedCertDate"" class=""InputLabel"">Комисия от дата:</span>
            </td>
            <td style=""text-align: left;"">
                <span id=""spanMedCertDate"">
                    <input type=""text"" id=""txtMedCertDate"" class='" + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" inLightBox=""true"" />
                </span>
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMedCertProtNum"" class=""InputLabel"">Протокол:</span>
            </td>
            <td style=""text-align: left;"">
                <input type=""text"" id=""txtMedCertProtNum"" class='InputField' />
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMedCertConclusion"" class=""InputLabel"">Решение:</span>
            </td>
            <td style=""text-align: left;"">
                " + ddConclusionHtml + @"
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMedCertMedRubric"" class=""InputLabel"">Медицинска рубрика:</span>
            </td>
            <td style=""text-align: left;"">
                " + ddMedRubricHTML + @"
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMedCertExpirationDate"" class=""InputLabel"">Дата на валидност:</span>
            </td>
            <td style=""text-align: left;"">
                <span id=""spanMedCertExpirationDate"">
                    <input type=""text"" id=""txtMedCertExpirationDate"" class='" + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" inLightBox=""true"" />
                </span>
            </td>
        </tr>

        <tr style=""height: 46px; padding-top: 5px;"">
            <td colspan=""2"">
                <span id=""spanAddEditMedCertLightBoxMsg"" class=""ErrorText"" style=""display: none;"">
                </span>&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan=""2"" style=""text-align: center;"">
                <table style=""margin: 0 auto;"">
                    <tr>
                        <td>
                            <div id=""btnSaveAddEditMedCertLightBox"" style=""display: inline;"" onclick=""SaveAddEditMedCertLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnSaveAddEditMedCertLightBoxText"" style=""width: 70px;"">
                                    Запис</div>
                                <b></b>
                            </div>
                            <div id=""btnCloseAddEditMedCertLightBox"" style=""display: inline;"" onclick=""HideAddEditMedCertLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnCloseAddEditMedCertLightBoxText"" style=""width: 70px;"">
                                    Затвори</div>
                                <b></b>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</center>
</div>";

            return html;
        }

        private void JSLoadMedCert()
        {
            if (GetUIItemAccessLevel("APPL_APPL_EDIT_PERSONDETAILS_MEDCERT") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int medCertID = int.Parse(Request.Form["MedCertID"]);

                PersonMedCert personMedCert = PersonMedCertUtil.GetPersonMedCert(medCertID, CurrentUser);

                response = "<response>";

                response += "<medCertDate>" + CommonFunctions.FormatDate(personMedCert.MedCertDate) + "</medCertDate>";
                response += "<protNum>" + AJAXTools.EncodeForXML(personMedCert.ProtNum) + "</protNum>";
                response += "<conclusionID>" + AJAXTools.EncodeForXML(personMedCert.Conclusion != null ? personMedCert.Conclusion.MilitaryMedicalConclusionId.ToString() : ListItems.GetOptionChooseOne().Value) + "</conclusionID>";
                response += "<medRubricID>" + AJAXTools.EncodeForXML(personMedCert.MedRubric != null ? personMedCert.MedRubric.MedicalRubricID.ToString() : ListItems.GetOptionChooseOne().Value) + "</medRubricID>";
                response += "<medCertExpirationDate>" + CommonFunctions.FormatDate(personMedCert.ExpirationDate) + "</medCertExpirationDate>";

                response += "</response>";

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

        //Save a particular Med Certs (ajax call)
        private void JSSaveMedCert()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "APPL_Applicants");

                int personId = int.Parse(Request.Form["PersonId"]);

                PersonMedCert personMedCert = new PersonMedCert(CurrentUser);

                personMedCert.MedCertID = int.Parse(Request.Form["MedCertID"]); ;
                personMedCert.PersonID = personId;
                personMedCert.MedCertDate = (!String.IsNullOrEmpty(Request.Form["MedCertDate"]) ? CommonFunctions.ParseDate(Request.Form["MedCertDate"]) : (DateTime?)null);
                personMedCert.ProtNum = Request.Form["MedCertProtNum"];
                personMedCert.ConclusionID = (!String.IsNullOrEmpty(Request.Form["MedCertConclusionId"]) && Request.Form["MedCertConclusionId"] != ListItems.GetOptionChooseOne().Value ? int.Parse(Request.Form["MedCertConclusionId"]) : (int?)null);
                personMedCert.MedRubricID = (!String.IsNullOrEmpty(Request.Form["MedCertMedRubricID"]) && Request.Form["MedCertMedRubricID"] != ListItems.GetOptionChooseOne().Value ? int.Parse(Request.Form["MedCertMedRubricID"]) : (int?)null);
                personMedCert.ExpirationDate = (!String.IsNullOrEmpty(Request.Form["MedCertExpirationDate"]) ? CommonFunctions.ParseDate(Request.Form["MedCertExpirationDate"]) : (DateTime?)null);

                PersonMedCertUtil.SavePersonMedCert(personMedCert, CurrentUser, change);

                change.WriteLog();

                string refreshedMedCertTable = GetMedCertTable(personId, CurrentUser);

                stat = AJAXTools.OK;
                response = "<status>OK</status>" +
                           "<refreshedMedCertTable>" + AJAXTools.EncodeForXML(refreshedMedCertTable) + @"</refreshedMedCertTable>";
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

        //Delete a particular Med Cert Title (ajax call)
        private void JSDeleteMedCert()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "APPL_Applicants");

                int medCertID = int.Parse(Request.Params["MedCertID"]);
                int personId = int.Parse(Request.Params["PersonId"]);

                PersonMedCertUtil.DeletePersonMedCert(medCertID, CurrentUser, change);

                change.WriteLog();

                string refreshedMedCertTable = GetMedCertTable(personId, CurrentUser);

                stat = AJAXTools.OK;
                response = "<status>OK</status>" +
                           "<refreshedMedCertTable>" + AJAXTools.EncodeForXML(refreshedMedCertTable) + @"</refreshedMedCertTable>";
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

        public string GetPsychCertTable(int personID, User currentUser)
        {
            bool IsPsychCertDateHidden = this.GetUIItemAccessLevel("APPL_APPL_EDIT_APPL_PSYCHCERT_DATE") == UIAccessLevel.Hidden;
            bool IsProtNumHidden = this.GetUIItemAccessLevel("APPL_APPL_EDIT_APPL_PSYCHCERT_PROTNUM") == UIAccessLevel.Hidden;
            bool IsConclusionHidden = this.GetUIItemAccessLevel("APPL_APPL_EDIT_APPL_PSYCHCERT_CONCLUSION") == UIAccessLevel.Hidden;
            bool IsExpirationDateHidden = this.GetUIItemAccessLevel("APPL_APPL_EDIT_APPL_PSYCHCERT_EXPIRATIONDATE") == UIAccessLevel.Hidden;

            string newHTML = "";

            if (this.GetUIItemAccessLevel("APPL_APPL") == UIAccessLevel.Enabled &&
                this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL") == UIAccessLevel.Enabled &&
                this.GetUIItemAccessLevel("APPL_APPL_EDIT_PERSONDETAILS") == UIAccessLevel.Enabled &&
                this.GetUIItemAccessLevel("APPL_APPL_EDIT_PERSONDETAILS_PSYCHCERT") == UIAccessLevel.Enabled
            )
            {
                newHTML = "<img src='../Images/note_add.png' alt='Нов запис' title='Нов запис' class='btnNewTableRecordIcon' onclick='NewPsychCert();' />";
            }

            StringBuilder tableHTML = new StringBuilder();

            tableHTML.Append(@"<table class='CommonHeaderTable' style='text-align: left;'>
                              <thead>
                                <tr>
                                   <th style='width: 20px; vertical-align: bottom;'>№</th>
   " + (!IsPsychCertDateHidden ? @"<th style='width: 80px; vertical-align: bottom;'>Комисия от дата</th>" : "") + @"
         " + (!IsProtNumHidden ? @"<th style='width: 180px; vertical-align: bottom;'>Протокол</th>" : "") + @"                    
      " + (!IsConclusionHidden ? @"<th style='width: 80px; vertical-align: bottom;'>Решение</th>" : "") + @"
  " + (!IsExpirationDateHidden ? @"<th style='width: 80px; vertical-align: bottom;'>Дата на валидност</th>" : "") + @"
                                   <th style='width: 50px;vertical-align: top;'>
                                      <div class='btnNewTableRecord'>" + newHTML + @"</div>
                                   </th>
                                </tr>
                              </thead>");

            int counter = 0;

            var personPsychCerts = PersonPsychCertUtil.GetAllPersonPsychCerts(personID, this.CurrentUser);

            foreach (var personPsychCert in personPsychCerts)
            {
                counter++;

                string deleteHTML = "";

                if (personPsychCert.CanDelete)
                {
                    if (this.GetUIItemAccessLevel("APPL_APPL") == UIAccessLevel.Enabled &&
                        this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL") == UIAccessLevel.Enabled &&
                        this.GetUIItemAccessLevel("APPL_APPL_EDIT_PERSONDETAILS") == UIAccessLevel.Enabled &&
                        this.GetUIItemAccessLevel("APPL_APPL_EDIT_PERSONDETAILS_PSYCHCERT") == UIAccessLevel.Enabled
                        )
                    {
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване' class='GridActionIcon' onclick='DeletePsychCert(" + personPsychCert.PsychCertID.ToString() + ");' />";
                    }
                }

                string editHTML = "";

                if (this.GetUIItemAccessLevel("APPL_APPL") == UIAccessLevel.Enabled &&
                    this.GetUIItemAccessLevel("APPL_APPL_EDITAPPL") == UIAccessLevel.Enabled &&
                    this.GetUIItemAccessLevel("APPL_APPL_EDIT_PERSONDETAILS") == UIAccessLevel.Enabled &&
                    this.GetUIItemAccessLevel("APPL_APPL_EDIT_PERSONDETAILS_PSYCHCERT") == UIAccessLevel.Enabled
                    )
                {
                    editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='EditPsychCert(" + personPsychCert.PsychCertID.ToString() + ");' />";

                }

                tableHTML.Append(@"<tr style='vertical-align: middle; height:20px; " + (counter == 1 ? "font-weight: bold;" : "") + @"' class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                     <td style='text-align: center;'>" + counter.ToString() + @"</td>
     " + (!IsPsychCertDateHidden ? @"<td style='text-align: left;'>" + CommonFunctions.FormatDate(personPsychCert.PsychCertDate) + @"</td>" : "") + @"
           " + (!IsProtNumHidden ? @"<td style='text-align: left;'>" + personPsychCert.ProtNum + @"</td>" : "") + @"                    
        " + (!IsConclusionHidden ? @"<td style='text-align: left;'>" + (personPsychCert.Conclusion != null ? personPsychCert.Conclusion.MilitaryMedicalConclusionName.ToString() : "") + @"</td>" : "") + @"
    " + (!IsExpirationDateHidden ? @"<td style='text-align: left;'>" + CommonFunctions.FormatDate(personPsychCert.ExpirationDate) + @"</td>" : "") + @"
                                     <td style='text-align: left;' nowrap>" + editHTML + deleteHTML + @"</td>
                                  </tr>
                             ");
            }

            tableHTML.Append("</table>");

            return tableHTML.ToString();
        }

        //Render the Psych Cert light-box
        public static string GetPsychCertLightBox(User currentUser)
        {
            string ddConclusionHtml = "";

            List<MilitaryMedicalConclusion> militaryMedicalConclusions = MilitaryMedicalConclusionUtil.GetAllMilitaryMedicalConclusions(currentUser);

            List<IDropDownItem> militaryMedicalConclusionsDropDownItems = new List<IDropDownItem>();
            foreach (MilitaryMedicalConclusion militaryMedicalConclusion in militaryMedicalConclusions)
                militaryMedicalConclusionsDropDownItems.Add(militaryMedicalConclusion as IDropDownItem);

            ddConclusionHtml = ListItems.GetDropDownHtml(militaryMedicalConclusionsDropDownItems, null, "ddPsychCertConclusion", true, null, null, @"style=""width: 120px;""");

            string html = @"
<div id=""divPsychCertLightBox"" style=""display: none;"" class=""lboxPsychCert"">
<center>
    <input type=""hidden"" id=""hdnPsychCertID"" />
    <table width=""90%"" style=""text-align: center;"">
        <colgroup style=""width: 40%"">
        </colgroup>
        <colgroup style=""width: 60%"">
        </colgroup>
        <tr style=""height: 15px"">
        </tr>
        <tr>
            <td colspan=""2"" align=""center"">
                <span class=""HeaderText"" style=""text-align: center;"" id=""lblAddEditPsychCertTitle""></span>
            </td>
        </tr>
        <tr style=""height: 15px"">
        </tr>  
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPsychCertDate"" class=""InputLabel"">Комисия от дата:</span>
            </td>
            <td style=""text-align: left;"">
                <span id=""spanPsychCertDate"">
                    <input type=""text"" id=""txtPsychCertDate"" class='" + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" inLightBox=""true"" />
                </span>
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPsychCertProtNum"" class=""InputLabel"">Протокол:</span>
            </td>
            <td style=""text-align: left;"">
                <input type=""text"" id=""txtPsychCertProtNum"" class='InputField' />
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPsychCertConclusion"" class=""InputLabel"">Решение:</span>
            </td>
            <td style=""text-align: left;"">
                " + ddConclusionHtml + @"
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPsychCertExpirationDate"" class=""InputLabel"">Дата на валидност:</span>
            </td>
            <td style=""text-align: left;"">
                <span id=""spanPsychCertExpirationDate"">
                    <input type=""text"" id=""txtPsychCertExpirationDate"" class='" + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" inLightBox=""true"" />
                </span>
            </td>
        </tr>

        <tr style=""height: 46px; padding-top: 5px;"">
            <td colspan=""2"">
                <span id=""spanAddEditPsychCertLightBoxMsg"" class=""ErrorText"" style=""display: none;"">
                </span>&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan=""2"" style=""text-align: center;"">
                <table style=""margin: 0 auto;"">
                    <tr>
                        <td>
                            <div id=""btnSaveAddEditPsychCertLightBox"" style=""display: inline;"" onclick=""SaveAddEditPsychCertLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnSaveAddEditPsychCertLightBoxText"" style=""width: 70px;"">
                                    Запис</div>
                                <b></b>
                            </div>
                            <div id=""btnCloseAddEditPsychCertLightBox"" style=""display: inline;"" onclick=""HideAddEditPsychCertLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnCloseAddEditPsychCertLightBoxText"" style=""width: 70px;"">
                                    Затвори</div>
                                <b></b>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</center>
</div>";

            return html;
        }

        private void JSLoadPsychCert()
        {
            if (GetUIItemAccessLevel("APPL_APPL_EDIT_PERSONDETAILS_PSYCHCERT") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int psychCertID = int.Parse(Request.Form["PsychCertID"]);

                PersonPsychCert personPsychCert = PersonPsychCertUtil.GetPersonPsychCert(psychCertID, CurrentUser);

                response = "<response>";

                response += "<psychCertDate>" + CommonFunctions.FormatDate(personPsychCert.PsychCertDate) + "</psychCertDate>";
                response += "<protNum>" + AJAXTools.EncodeForXML(personPsychCert.ProtNum) + "</protNum>";
                response += "<conclusionID>" + AJAXTools.EncodeForXML(personPsychCert.Conclusion != null ? personPsychCert.Conclusion.MilitaryMedicalConclusionId.ToString() : ListItems.GetOptionChooseOne().Value) + "</conclusionID>";
                response += "<psychCertExpirationDate>" + CommonFunctions.FormatDate(personPsychCert.ExpirationDate) + "</psychCertExpirationDate>";

                response += "</response>";

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

        //Save a particular Psych Certs (ajax call)
        private void JSSavePsychCert()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "APPL_Applicants");

                int personId = int.Parse(Request.Form["PersonId"]);

                PersonPsychCert personPsychCert = new PersonPsychCert(CurrentUser);

                personPsychCert.PsychCertID = int.Parse(Request.Form["PsychCertID"]); ;
                personPsychCert.PersonID = personId;
                personPsychCert.PsychCertDate = (!String.IsNullOrEmpty(Request.Form["PsychCertDate"]) ? CommonFunctions.ParseDate(Request.Form["PsychCertDate"]) : (DateTime?)null);
                personPsychCert.ProtNum = Request.Form["PsychCertProtNum"];
                personPsychCert.ConclusionID = (!String.IsNullOrEmpty(Request.Form["PsychCertConclusionId"]) && Request.Form["PsychCertConclusionId"] != ListItems.GetOptionChooseOne().Value ? int.Parse(Request.Form["PsychCertConclusionId"]) : (int?)null);
                personPsychCert.ExpirationDate = (!String.IsNullOrEmpty(Request.Form["PsychCertExpirationDate"]) ? CommonFunctions.ParseDate(Request.Form["PsychCertExpirationDate"]) : (DateTime?)null);

                PersonPsychCertUtil.SavePersonPsychCert(personPsychCert, CurrentUser, change);

                change.WriteLog();

                string refreshedPsychCertTable = GetPsychCertTable(personId, CurrentUser);

                stat = AJAXTools.OK;
                response = "<status>OK</status>" +
                           "<refreshedPsychCertTable>" + AJAXTools.EncodeForXML(refreshedPsychCertTable) + @"</refreshedPsychCertTable>";
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

        //Delete a particular Psych Cert Title (ajax call)
        private void JSDeletePsychCert()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "APPL_Applicants");

                int psychCertID = int.Parse(Request.Params["PsychCertID"]);
                int personId = int.Parse(Request.Params["PersonId"]);

                PersonPsychCertUtil.DeletePersonPsychCert(psychCertID, CurrentUser, change);

                change.WriteLog();

                string refreshedPsychCertTable = GetPsychCertTable(personId, CurrentUser);

                stat = AJAXTools.OK;
                response = "<status>OK</status>" +
                           "<refreshedPsychCertTable>" + AJAXTools.EncodeForXML(refreshedPsychCertTable) + @"</refreshedPsychCertTable>";
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
