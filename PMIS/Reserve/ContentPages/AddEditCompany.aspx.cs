using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class AddEditCompany : RESPage
    {
        public string UnifiedIdentityCodeLabelText = CommonFunctions.GetLabelText("UnifiedIdentityCode");

        string redirectBack = "";

        public override string PageUIKey
        {
            get
            {
                return "RES_LISTMAINT_COMPANIES";
            }
        }
        
        public int CompanyId
        {
            get
            {
                int companyId = 0;
                //gets reservistid either from the hidden field on the page or from page url
                if (String.IsNullOrEmpty(this.hdnCompanyId.Value) || this.hdnCompanyId.Value == "0")
                {
                    if (Request.Params["HdnCompanyId"] != null)
                        Int32.TryParse(Request.Params["HdnCompanyId"].ToString(), out companyId);

                    //sets reservist ID in hidden field on the page in order to be accessible in javascript
                    this.hdnCompanyId.Value = companyId.ToString();
                }
                else
                {
                    Int32.TryParse(this.hdnCompanyId.Value, out companyId);
                }

                return companyId;
            }
            set { this.hdnCompanyId.Value = value.ToString(); }
        }

        //This is a flag field that says if the screen is opened from the Home screen
        //This is used to navigate the user back to the home screen when using the Back button
        private int FromHome
        {
            get
            {
                int fh = 0;
                if (String.IsNullOrEmpty(this.hdnFromHome.Value)
                    || this.hdnFromHome.Value == "0")
                {
                    if (Request.Params["fh"] != null)
                        int.TryParse(Request.Params["fh"].ToString(), out fh);

                    this.hdnFromHome.Value = hdnFromHome.ToString();
                }
                else
                {
                    Int32.TryParse(this.hdnFromHome.Value, out fh);
                }

                return fh;
            }

            set
            {
                this.hdnFromHome.Value = value.ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {            
            //Set the Request parameters
            if (!String.IsNullOrEmpty(Request.Params["CompanyId"]))
                this.hdnCompanyId.Value = Request.Params["CompanyId"];


            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadCompany")
            {
                JSLoadCompany();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveCompany")
            {
                JSSaveCompany();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSCheckIsBulstatAndCompanyName")
            {
                JSCheckIsBulstatAndCompanyName();
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
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRepopulateRegionMunicipalityCityDistrictByCityId")
            {
                JSRepopulateRegionMunicipalityCityDistrictByCityId();
                return;
            }

            SetupPageUI(); //setup user interface elements according to rights of the user's role

            //Hilight the correct item in the menu
            HighlightMenuItems("Lists", "Lists_RES_Companies");

            //Hide the navigation buttons
            //HideNavigationControls(btnBack);

            //Populate the drop-downs
            PopulateDropdowns();

            //if (Request.Params["fh"] == "1")
            //    redirectBack = "~/ContentPages/Home.aspx";
            //else
            redirectBack = "~/ContentPages/ManageCompanies.aspx";

            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.Params["CompanyId"]) && CompanyId == 0)
                    this.CompanyId = int.Parse(Request.Params["CompanyId"]);

                string header = (this.CompanyId > 0 ? "Редактиране на фирма" : "Добавяне на фирма");
                lblHeaderTitle.InnerHtml = header;
                this.Title = header;
            }         
        }

        //Populate the drop-downs
        private void PopulateDropdowns()
        {
            this.PopulateRegions();
            this.PopulateOwnershipTypes();
            this.PopulateAdministrations();
        }

        private void PopulateRegions()
        {
            List<Region> listRegion = RegionUtil.GetRegions(CurrentUser);
            List<IDropDownItem> ddiRegions = new List<IDropDownItem>();
            foreach (Region region in listRegion)
            {
                ddiRegions.Add(region);
            }

            IDropDownItem selectedRegion = (ddiRegions.Count > 0 ? ddiRegions[0] : null);

            // Generates html for permanent regions drop down list
            string permRegionsHTML = ListItems.GetDropDownHtml(ddiRegions, null, "ddlRegion", true, null, "ddRegion_Changed()", "style='width: 170px;'");
            this.divRegion.InnerHtml = permRegionsHTML;


            // Generates html for permanent municipalities drop down list
            this.divMunicipality.InnerHtml = ListItems.GetDropDownHtml(null, null, "ddlMunicipality", true, null, "ddMunicipality_Changed()", "style='width: 170px;'");            

            // Generates html for permanent cities drop down list
            this.divCity.InnerHtml = ListItems.GetDropDownHtml(null, null, "ddlCity", true, null, "ddCity_Changed()", "style='width: 170px;' class='InputField' ");

            // Generates html for permanent districts drop down list
            this.divDistrict.InnerHtml = ListItems.GetDropDownHtml(null, null, "ddlDistrict", true, null, "ddDistrict_Changed()", "style='width: 170px;'");            
        }

        //Populate the OwnershipTypes drop-down
        private void PopulateOwnershipTypes()
        {
            this.ddOwnershipType.DataSource = OwnershipTypeUtil.GetAllOwnershipTypes(CurrentUser);
            this.ddOwnershipType.DataTextField = "OwnershipTypeName";
            this.ddOwnershipType.DataValueField = "OwnershipTypeId";
            this.ddOwnershipType.DataBind();
        }

        //Populate the OwnershipTypes drop-down
        private void PopulateAdministrations()
        {
            this.ddAdministration.DataSource = AdministrationUtil.GetAllAdministrations(CurrentUser);
            this.ddAdministration.DataTextField = "AdministrationName";
            this.ddAdministration.DataValueField = "AdministrationId";
            this.ddAdministration.DataBind();
            this.ddAdministration.Items.Insert(0, ListItems.GetOptionChooseOne());
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(redirectBack);
        }

        //Load Military Report Person details (ajax call)
        private void JSLoadCompany()
        {
            if (GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES") == UIAccessLevel.Hidden
                || GetUIItemAccessLevel("RES_LISTMAINT") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                Company company = CompanyUtil.GetCompany(this.CompanyId, CurrentUser);

                stat = AJAXTools.OK;

                //Existing Military Report Person: Load him details
                if (company != null)
                {
                    response = @"
                        <company>
                            <companyId>" + AJAXTools.EncodeForXML(company.CompanyId.ToString()) + @"</companyId>                            
                            <bulstat>" + AJAXTools.EncodeForXML(company.UnifiedIdentityCode) + @"</bulstat>
                            <companyName>" + AJAXTools.EncodeForXML(company.CompanyName) + @"</companyName>
                            <ownershipTypeId>" + AJAXTools.EncodeForXML(company.OwnershipTypeId.ToString()) + @"</ownershipTypeId>
                            <administrationId>" + AJAXTools.EncodeForXML(company.AdministrationId.HasValue ? company.AdministrationId.Value.ToString() : ListItems.GetOptionChooseOne().Value) + @"</administrationId>
                            <phone>" + AJAXTools.EncodeForXML(company.Phone) + @"</phone>                            
                            <cityId>" + AJAXTools.EncodeForXML(company.CityId != null ? company.CityId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</cityId>
                            <postCode>" + AJAXTools.EncodeForXML((!String.IsNullOrEmpty(company.PostCode) ? company.PostCode : (company.CityId != null ? company.City.PostCode.ToString() : ""))) + @"</postCode>                            
                            <regionId>" + AJAXTools.EncodeForXML(company.CityId != null ? company.City.RegionId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</regionId>
                            <municipalityId>" + AJAXTools.EncodeForXML(company.CityId != null ? company.City.MunicipalityId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</municipalityId>
                            <address>" + AJAXTools.EncodeForXML(company.Address) + @"</address>
                            <districtId>" + AJAXTools.EncodeForXML(company.DistrictId != null ? company.DistrictId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</districtId>
                        </company>";
                }
                else //New company
                {
                    response = @"
                        <company>
                            <companyId>0</companyId>                            
                            <bulstat></bulstat>
                            <companyName></companyName>
                            <ownershipTypeId></ownershipTypeId>
                            <administrationId>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Value) + @"</administrationId>
                            <phone></phone> 
                            <cityId>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Value) + @"</cityId>
                            <postCode></postCode>                           
                            <regionId>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Value) + @"</regionId>
                            <municipalityId>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Value) + @"</municipalityId>
                            <address></address>
                            <districtId>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Value) + @"</districtId>                            
                        </company>";
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

        //Save Military Report Person details (ajax call)
        private void JSSaveCompany()
        {
            if (GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES_EDIT") != UIAccessLevel.Enabled
                || GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES") != UIAccessLevel.Enabled
                || GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            int companyId = int.Parse(Request.Form["CompanyId"]);

            string bulstat = Request.Form["Bulstat"];
            string companyName = Request.Form["CompanyName"];
            string phone = Request.Form["Phone"];

            int ownershipTypeId = int.Parse(Request.Form["OwnershipTypeId"]);
            int? administrationId = Request.Form["AdministrationId"].ToString() != ListItems.GetOptionChooseOne().Value ?  (int?)int.Parse(Request.Form["AdministrationId"]) : null;

            int? cityId = null;
            if (!String.IsNullOrEmpty(Request.Form["CityID"]) &&
                Request.Form["CityID"] != ListItems.GetOptionChooseOne().Value)
            {
                cityId = int.Parse(Request.Form["CityID"]);
            }

            string postCode = Request.Form["SecondPostCode"];
            string address = Request.Form["Address"];

            int? districtId = null;
            if (!String.IsNullOrEmpty(Request.Form["DistrictID"]) &&
                Request.Form["DistrictID"] != ListItems.GetOptionChooseOne().Value)
            {
                districtId = int.Parse(Request.Form["DistrictID"]);
            }   

            Company company = new Company(CurrentUser);

            company.CompanyId = companyId;
            company.OwnershipTypeId = ownershipTypeId;
            company.AdministrationId = administrationId;
            company.UnifiedIdentityCode = bulstat;
            company.CompanyName = companyName;
            company.Phone = phone;

            company.CityId = cityId;
            company.Address = address;
            company.DistrictId = districtId;
            company.PostCode = postCode;           

            string stat = "";
            string response = "";

            try
            {
                //Track the changes into the Audit Trail 
                Change change = new Change(CurrentUser, "RES_MilitaryReportPersons");


                //Save the changes in table Person
                if (company.CompanyId == 0)
                {
                    CompanyUtil.SaveCompany(company, CurrentUser, change);
                }
                else
                {
                    CompanyUtil.SaveCompany(company, CurrentUser, change);
                }

                change.WriteLog();

                stat = AJAXTools.OK;
                response = @"<response>OK</response>
                             <companyId>" + AJAXTools.EncodeForXML(company.CompanyId.ToString()) + @"</companyId>";
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

        private void JSCheckIsBulstatAndCompanyName()
        {
            if (GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES") == UIAccessLevel.Hidden
                || GetUIItemAccessLevel("RES_LISTMAINT") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string bulstat = Request.Params["Bulstat"];
            string companyName = Request.Params["CompanyName"];

            string stat = "";
            string response = "";

            try
            {
                int isValidBulstat = 1;
                int isAlreadyOccupiedCompanyName = 0;

                /*След като класификатора на фирмите ще съдържа и списък на хора (физически лица собственици на фирми), не е правилно да проверява за уникалност на името. Нормално е да има няколко човека с едно и също име.
                Company company = CompanyUtil.GetCompanyByCompanyName(companyName, CurrentUser);

                if (company != null)
                {
                    if (this.CompanyId != company.CompanyId)
                    {
                        isAlreadyOccupiedCompanyName = 1;   
                    }
                }
                */

                if (!String.IsNullOrEmpty(bulstat) && !(CompanyUtil.IsValidUnifiedIdentityNumber(bulstat, CurrentUser) || PersonUtil.IsValidIdentityNumber(bulstat, CurrentUser)))
                {
                    isValidBulstat = 0;
                }
               

                stat = AJAXTools.OK;

                response = @"
                    <isValidBulstat>" + isValidBulstat.ToString() + @"</isValidBulstat>
                    <isAlreadyOccupiedCompanyName>" + isAlreadyOccupiedCompanyName.ToString() + @"</isAlreadyOccupiedCompanyName>
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

        //Get the Municipalities for a particular Region (ajax call)
        private void JSRepopulateMunicipality()
        {
            if (GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES") == UIAccessLevel.Hidden
                || GetUIItemAccessLevel("RES_LISTMAINT") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

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
            if (GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES") == UIAccessLevel.Hidden
                || GetUIItemAccessLevel("RES_LISTMAINT") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

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

        //Populate the Region, the Municipalityand the City when changing the PostCode (ajax call)
        private void JSRepopulateRegionMunicipalityCity()
        {
            if (GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES") == UIAccessLevel.Hidden
                || GetUIItemAccessLevel("RES_LISTMAINT") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

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
            if (GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES") == UIAccessLevel.Hidden
                || GetUIItemAccessLevel("RES_LISTMAINT") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

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
            if (GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES") == UIAccessLevel.Hidden
                || GetUIItemAccessLevel("RES_LISTMAINT") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

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
            if (GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES") == UIAccessLevel.Hidden
                || GetUIItemAccessLevel("RES_LISTMAINT") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

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

                /*
                if (foundDistrict == null)
                {
                    Company company = CompanyUtil.GetCompany(this.CompanyId, CurrentUser);
                    if (company != null)
                    {
                        foundDistrict = company.District;                     
                    }
                }*/

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

                    response += "<districtId>0</districtId>" +
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

        //Populate the Region, the Municipalityand, the City and the District by CityID (ajax call)
        private void JSRepopulateRegionMunicipalityCityDistrictByCityId()
        {
            if (GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES") == UIAccessLevel.Hidden
                || GetUIItemAccessLevel("RES_LISTMAINT") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int cityId = 0;

                if (!String.IsNullOrEmpty(Request.Form["CityId"]))
                {
                    try
                    {
                        cityId = int.Parse(Request.Form["CityId"]);
                    }
                    catch
                    {
                        cityId = 0;
                    }
                }


                City foundCity = null;
                foundCity = CityUtil.GetCity(cityId, CurrentUser);

                if (foundCity != null)
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

                    response += "<districtId>0</districtId>" +
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

        // Setup user interface elements according to rights of the user's role
        private void SetupPageUI()
        {
            List<string> disabledClientControls = new List<string>();
            List<string> hiddenClientControls = new List<string>();

            if (CompanyId == 0) // add mode of page
            {
                bool screenHidden = (this.GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES_ADD") != UIAccessLevel.Enabled) ||
                                      (this.GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES") != UIAccessLevel.Enabled) ||
                                      (this.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled);

                bool screenDisabled = (this.GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES_ADD") == UIAccessLevel.Disabled) ||
                                      (this.GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES") == UIAccessLevel.Disabled) ||
                                      (this.GetUIItemAccessLevel("RES_LISTMAINT") == UIAccessLevel.Disabled);

                if (screenHidden)
                    RedirectAccessDenied();

                if (screenDisabled)
                {
                    this.pageDisabledControls.Add(btnSave);                                        
                }
              
                UIAccessLevel l;

                l = this.GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES_ADD_BULSTAT");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblBulstat");
                    disabledClientControls.Add("txtBulstat");
                    
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblBulstat");
                    hiddenClientControls.Add("txtBulstat");
                }

                l = this.GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES_ADD_COMPANYNAME");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblCompanyName");
                    disabledClientControls.Add("txtCompanyName");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblCompanyName");
                    hiddenClientControls.Add("txtCompanyName");
                }

                l = this.GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES_ADD_OWNERSHIPTYPE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblOwnershipType");
                    disabledClientControls.Add(ddOwnershipType.ClientID);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblOwnershipType");
                    hiddenClientControls.Add(ddOwnershipType.ClientID);
                }

                l = this.GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES_ADD_ADMINISTRATION");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblAdministration");
                    disabledClientControls.Add(ddAdministration.ClientID);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAdministration");
                    hiddenClientControls.Add(ddAdministration.ClientID);
                }

                l = this.GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES_ADD_PHONE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblPhone");
                    disabledClientControls.Add("txtPhone");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblPhone");
                    hiddenClientControls.Add("txtPhone");
                }

                l = this.GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES_ADD_CITY");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblRegion");
                    disabledClientControls.Add(divRegion.ClientID);
                    disabledClientControls.Add("lblMunicipality");
                    disabledClientControls.Add(divMunicipality.ClientID);
                    disabledClientControls.Add("lblCity");
                    disabledClientControls.Add(divCity.ClientID);
                    disabledClientControls.Add("lblDistrict");
                    disabledClientControls.Add(divDistrict.ClientID);
                    disabledClientControls.Add("lblPostCode");
                    disabledClientControls.Add("txtPostCode");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblRegion");
                    hiddenClientControls.Add(divRegion.ClientID);
                    hiddenClientControls.Add("lblMunicipality");
                    hiddenClientControls.Add(divMunicipality.ClientID);
                    hiddenClientControls.Add("lblCity");
                    hiddenClientControls.Add(divCity.ClientID);
                    hiddenClientControls.Add("lblDistrict");
                    hiddenClientControls.Add(divDistrict.ClientID);
                    hiddenClientControls.Add("lblPostCode");
                    hiddenClientControls.Add("txtPostCode");
                }

                l = this.GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES_ADD_ADDRESS");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblAddress");
                    disabledClientControls.Add("txtaAddress");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAddress");
                    hiddenClientControls.Add("txtaAddress");
                }
            }
            else // edit mode of page
            {

                bool screenHidden = (this.GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES_EDIT") != UIAccessLevel.Enabled) ||
                      (this.GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES") != UIAccessLevel.Enabled) ||
                      (this.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled);

                bool screenDisabled = (this.GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES_EDIT") == UIAccessLevel.Disabled) ||
                                      (this.GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES") == UIAccessLevel.Disabled) ||
                                      (this.GetUIItemAccessLevel("RES_LISTMAINT") == UIAccessLevel.Disabled);

                if (screenHidden)
                    RedirectAccessDenied();

                if (screenDisabled)
                {
                    this.pageDisabledControls.Add(btnSave);
                }

                UIAccessLevel l;

                l = this.GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES_EDIT_BULSTAT");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblBulstat");
                    disabledClientControls.Add("txtBulstat");

                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblBulstat");
                    hiddenClientControls.Add("txtBulstat");
                }

                l = this.GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES_EDIT_COMPANYNAME");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblCompanyName");
                    disabledClientControls.Add("txtCompanyName");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblCompanyName");
                    hiddenClientControls.Add("txtCompanyName");
                }

                l = this.GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES_EDIT_OWNERSHIPTYPE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblOwnershipType");
                    disabledClientControls.Add(ddOwnershipType.ClientID);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblOwnershipType");
                    hiddenClientControls.Add(ddOwnershipType.ClientID);
                }

                l = this.GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES_EDIT_ADMINISTRATION");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblAdministration");
                    disabledClientControls.Add(ddAdministration.ClientID);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAdministration");
                    hiddenClientControls.Add(ddAdministration.ClientID);
                }

                l = this.GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES_EDIT_PHONE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblPhone");
                    disabledClientControls.Add("txtPhone");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblPhone");
                    hiddenClientControls.Add("txtPhone");
                }

                l = this.GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES_EDIT_CITY");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblRegion");
                    disabledClientControls.Add(divRegion.ClientID);
                    disabledClientControls.Add("lblMunicipality");
                    disabledClientControls.Add(divMunicipality.ClientID);
                    disabledClientControls.Add("lblCity");
                    disabledClientControls.Add(divCity.ClientID);
                    disabledClientControls.Add("lblDistrict");
                    disabledClientControls.Add(divDistrict.ClientID);
                    disabledClientControls.Add("lblPostCode");
                    disabledClientControls.Add("txtPostCode");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblRegion");
                    hiddenClientControls.Add(divRegion.ClientID);
                    hiddenClientControls.Add("lblMunicipality");
                    hiddenClientControls.Add(divMunicipality.ClientID);
                    hiddenClientControls.Add("lblCity");
                    hiddenClientControls.Add(divCity.ClientID);
                    hiddenClientControls.Add("lblDistrict");
                    hiddenClientControls.Add(divDistrict.ClientID);
                    hiddenClientControls.Add("lblPostCode");
                    hiddenClientControls.Add("txtPostCode");
                }

                l = this.GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES_EDIT_ADDRESS");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblAddress");
                    disabledClientControls.Add("txtaAddress");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAddress");
                    hiddenClientControls.Add("txtaAddress");
                }                          
            }

            SetDisabledClientControls(disabledClientControls.ToArray());
            SetHiddenClientControls(hiddenClientControls.ToArray());
        }
    }
}
