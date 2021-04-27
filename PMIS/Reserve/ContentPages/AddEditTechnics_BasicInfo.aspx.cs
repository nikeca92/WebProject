using System;
using System.Collections.Generic;
using System.Text;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class AddEditTechnics_BasicInfo : RESPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
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

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadNormativeTechnics")
            {
                JSLoadNormativeTechnics();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetTechnicsSubTypeInfo")
            {
                JSGetTechnicsSubTypeInfo();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetAddressInfoForCompany")
            {
                JSGetAddressInfoForCompany();
                return;
            }
        }

        //Populate the PostCode and District when changing the City (ajax call)
        private void JSRepopulatePostCodeAndDistrict()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden)
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
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden)
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
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden)
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

        //Get the Municipalities for a particular Region (ajax call)
        private void JSRepopulateMunicipality()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden)
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
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden)
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

        private void JSGetNormativeTechnicsCode()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

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
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                string normativeCode = Request.Form["NormativeCode"];
                string technicsTypeKey = Request.Form["TechnicsTypeKey"];

                int normativeTechnicsId = 0;

                NormativeTechnics normativeTechnics = NormativeTechnicsUtil.GetNormativeTechnicsObjByCode(CurrentUser, normativeCode, technicsTypeKey);

                if (normativeTechnics != null)
                    normativeTechnicsId = normativeTechnics.NormativeTechnicsId;

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

        private void JSGetTechnicsSubTypeInfo()
        {
            string response = "";
            string stat = "";
            string technicsSubTypeInfo = "";

            try
            {
                int technicsId = int.Parse(Request.Form["TechnicsId"]);
                Technics technics = TechnicsUtil.GetTechnics(technicsId, CurrentUser);

                string missingValue = "<липсва>";
                technicsSubTypeInfo = (technics.NormativeTechnics != null && technics.NormativeTechnics.TechnicsSubType != null ? technics.NormativeTechnics.TechnicsSubType.TechnicsSubTypeName : missingValue);

                stat = AJAXTools.OK;

                response = "<technicsSubTypeInfo>" + AJAXTools.EncodeForXML(technicsSubTypeInfo) + @"</technicsSubTypeInfo>";
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

        private void JSGetAddressInfoForCompany()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            int companyId = 0;
            if (!String.IsNullOrEmpty(Request.Form["CompanyId"]))
                companyId = int.Parse(Request.Form["CompanyId"]);

            string stat = "";
            string response = "";

            try
            {
                Company company = CompanyUtil.GetCompany(companyId, CurrentUser);
                
                stat = AJAXTools.OK;

                response = @"
                    <addressInfo>
                         <cityId>" + AJAXTools.EncodeForXML(company != null && company.CityId != null ? company.CityId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</cityId>
                         <postCode>" + AJAXTools.EncodeForXML(company != null ? company.PostCode : "") + @"</postCode>
                         <regionId>" + AJAXTools.EncodeForXML(company != null && company.CityId != null ? company.City.RegionId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</regionId>
                         <municipalityId>" + AJAXTools.EncodeForXML(company != null && company.CityId != null ? company.City.MunicipalityId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</municipalityId>
                         <districtId>" + AJAXTools.EncodeForXML(company != null && company.DistrictId != null ? company.District.DistrictId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</districtId>
                         <address>" + AJAXTools.EncodeForXML(company != null ? company.Address : "") + @"</address>
                    </addressInfo>";

                if (company != null && company.CityId != null)
                {
                    List<Municipality> municipalities = MunicipalityUtil.GetMunicipalities(company.City.RegionId, CurrentUser);
                    List<City> cities = CityUtil.GetCities(company.City.MunicipalityId, CurrentUser);
                    List<District> districts = company.City.Districts;

                    foreach (Municipality municipality in municipalities)
                    {
                        response += "<r_m>" +
                                    "<id>" + municipality.MunicipalityId.ToString() + "</id>" +
                                    "<name>" + AJAXTools.EncodeForXML(municipality.MunicipalityName) + "</name>" +
                                    "</r_m>";
                    }

                    foreach (City city in cities)
                    {
                        response += "<r_c>" +
                                    "<id>" + city.CityId.ToString() + "</id>" +
                                    "<name>" + AJAXTools.EncodeForXML(city.CityName) + "</name>" +
                                    "</r_c>";
                    }

                    response += "<r_d>" +
                                "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                                "</r_d>";

                    foreach (District district in districts)
                    {
                        response += "<r_d>" +
                                    "<id>" + district.DistrictId.ToString() + "</id>" +
                                    "<name>" + AJAXTools.EncodeForXML(district.DistrictName) + "</name>" +
                                    "</r_d>";
                    }
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

        private void JSLoadNormativeTechnics()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<normativeTechnics>";

                response += "<n>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</n>";

                string technicsTypeKey = Request.Form["TechnicsTypeKey"];
                string vehicleKindIDStr = Request.Form["VehicleKindID"];
                int? vehicleKindID = null;
                if (!String.IsNullOrEmpty(vehicleKindIDStr) && vehicleKindIDStr != ListItems.GetOptionChooseOne().Value)
                    vehicleKindID = int.Parse(vehicleKindIDStr);

                List<NormativeTechnics> normativeTechnics = NormativeTechnicsUtil.GetNormativeTechnicsByVehicleKind(CurrentUser, technicsTypeKey, vehicleKindID);

                foreach (NormativeTechnics normativeTechnicsRec in normativeTechnics)
                {
                    response += "<n>" +
                                "<id>" + normativeTechnicsRec.Value() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(normativeTechnicsRec.Text()) + "</name>" +
                                "</n>";
                }

                response += "</normativeTechnics>";

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


    public static class AddEditTechnics_BasicInfo_PageUtil
    {
        public static string GetBasicInfoTabContent(AddEditTechnics page)
        {
            string tabContent = "";

            switch (page.TechnicsTypeKey)
            {
                case "VEHICLES":
                    tabContent = AddEditTechnics_VEHICLES_PageUtil.GetBasicInfoTabContent(page);
                    break;
                case "TRAILERS":
                    tabContent = AddEditTechnics_TRAILERS_PageUtil.GetBasicInfoTabContent(page);
                    break;
                case "TRACTORS":
                    tabContent = AddEditTechnics_TRACTORS_PageUtil.GetBasicInfoTabContent(page);
                    break;
                case "ENG_EQUIP":
                    tabContent = AddEditTechnics_ENG_EQUIP_PageUtil.GetBasicInfoTabContent(page);
                    break;
                case "MOB_LIFT_EQUIP":
                    tabContent = AddEditTechnics_MOB_LIFT_EQUIP_PageUtil.GetBasicInfoTabContent(page);
                    break;
                case "RAILWAY_EQUIP":
                    tabContent = AddEditTechnics_RAILWAY_EQUIP_PageUtil.GetBasicInfoTabContent(page);
                    break;
                case "AVIATION_EQUIP":
                    tabContent = AddEditTechnics_AVIATION_EQUIP_PageUtil.GetBasicInfoTabContent(page);
                    break;
                case "VESSELS":
                    tabContent = AddEditTechnics_VESSELS_PageUtil.GetBasicInfoTabContent(page);
                    break;
                case "FUEL_CONTAINERS":
                    tabContent = AddEditTechnics_FUEL_CONTAINERS_PageUtil.GetBasicInfoTabContent(page);
                    break;
            }

            return tabContent;
        }

        public static string GetBasicInfoTabUIItems(AddEditTechnics page)
        {
            string tabUIItems = "";

            switch (page.TechnicsTypeKey)
            {
                case "VEHICLES":
                    tabUIItems = AddEditTechnics_VEHICLES_PageUtil.GetBasicInfoTabUIItems(page);
                    break;
                case "TRAILERS":
                    tabUIItems = AddEditTechnics_TRAILERS_PageUtil.GetBasicInfoTabUIItems(page);
                    break;
                case "TRACTORS":
                    tabUIItems = AddEditTechnics_TRACTORS_PageUtil.GetBasicInfoTabUIItems(page);
                    break;
                case "ENG_EQUIP":
                    tabUIItems = AddEditTechnics_ENG_EQUIP_PageUtil.GetBasicInfoTabUIItems(page);
                    break;
                case "MOB_LIFT_EQUIP":
                    tabUIItems = AddEditTechnics_MOB_LIFT_EQUIP_PageUtil.GetBasicInfoTabUIItems(page);
                    break;
                case "RAILWAY_EQUIP":
                    tabUIItems = AddEditTechnics_RAILWAY_EQUIP_PageUtil.GetBasicInfoTabUIItems(page);
                    break;
                case "AVIATION_EQUIP":
                    tabUIItems = AddEditTechnics_AVIATION_EQUIP_PageUtil.GetBasicInfoTabUIItems(page);
                    break;
                case "VESSELS":
                    tabUIItems = AddEditTechnics_VESSELS_PageUtil.GetBasicInfoTabUIItems(page);
                    break;
                case "FUEL_CONTAINERS":
                    tabUIItems = AddEditTechnics_FUEL_CONTAINERS_PageUtil.GetBasicInfoTabUIItems(page);
                    break;
            }

            return tabUIItems;
        }

        public static string GetBasicInfoResidenceContent(AddEditTechnics page)
        {
            List<IDropDownItem> ddiRegions = new List<IDropDownItem>();
            List<Region> regions = RegionUtil.GetRegions(page.CurrentUser);

            foreach (Region region in regions)
            {
                ddiRegions.Add(region);
            }

            string residenceRegionsHTML = ListItems.GetDropDownHtml(ddiRegions, null, "ddResidenceRegion", true, null, "ddResidenceRegion_Changed();", "style='width: 170px;'");
            string residenceMunicipalitiesHTML = ListItems.GetDropDownHtml(null, null, "ddResidenceMunicipality", true, null, "ddResidenceMunicipality_Changed();", "style='width: 170px;'");
            string residenceCityHTML = ListItems.GetDropDownHtml(null, null, "ddResidenceCity", true, null, "ddResidenceCity_Changed();", "style='width: 170px;'");
            string residenceDistrictHTML = ListItems.GetDropDownHtml(null, null, "ddResidenceDistrict", true, null, "ddResidenceDistrict_Changed();", "style='width: 170px;'");


            string html = @"
<div style=""height: 10px;""></div>

<fieldset style=""width: 830px; padding: 0px;"">
   <table class=""InputRegion"" style=""width: 830px; padding: 10px; padding-top: 0px; margin-top: 0px;"">
      <tr style=""height: 3px;"">
      </tr>    
      <tr>
         <td style=""text-align: left;"">
            <span style=""color: #0B4489; font-weight: bold; font-size: 1.1em; width: 100%; Position: relative; top: -5px;"">Mестодомуване</span>
            &nbsp;&nbsp;
            <span id=""btnImgCopyOwnerAddressCont""><img src=""../Images/copy.png"" id=""btnImgCopyOwnerAddress"" title=""Копиране от Адрес на собственика"" alt=""Копиране от Адрес на собственика"" style=""cursor: pointer;""
               onclick=""CopyOwnerAddressToResidence();"" /></span>
         </td>
      </tr>
      <tr>
         <td style=""text-align: left;"">
            <table style=""margin: 0 auto;"">
                <tr>
                    <td style=""text-align: right; width: 80px;"">
                        <span id=""lblResidenceRegion"" class=""InputLabel"">Област:</span>
                    </td>
                    <td style=""text-align: left; width: 170px;"">
                         " + residenceRegionsHTML + @"
                    </td>
                    <td style=""text-align: right; width: 80px;"">
                        <span id=""lblResidenceMunicipality"" class=""InputLabel"">Община:</span>
                    </td>
                    <td style=""text-align: left;width: 170px;"">
                         " + residenceMunicipalitiesHTML + @"
                    </td>                    
                    <td style=""text-align: right; width: 205px;"">
                        <span id=""lblResidenceCity"" class=""InputLabel"">Населено място:</span>
                    </td>
                    <td style=""text-align: left; width: 170px;"">
                        " + residenceCityHTML + @"
                    </td>                    
                </tr>
                <tr>
                    <td style=""text-align: right; vertical-align: top;"" rowspan=""2"">
                        <span id=""lblResidenceAddress"" class=""InputLabel"">Адрес:</span>
                    </td>
                    <td colspan=""3"" style=""text-align: left;"" rowspan=""2"">
                        <textarea id=""txtResidenceAddress"" cols=""3"" rows='3' class='InputField' style='width: 99%;'></textarea>
                    </td>
                    <td style=""text-align: right;"">
                        <span id=""lblResidenceDistrict"" class=""InputLabel"">Район:</span>
                    </td>
                    <td style=""text-align: left;"">
                         " + residenceDistrictHTML + @"
                    </td>
                </tr>
                <tr>                    
                    <td style=""text-align: right;"">
                        <span id=""lblResidencePostCode"" class=""InputLabel"">Пощенски код:</span>
                    </td>
                    <td style=""text-align: left;"">
                        <input id=""txtResidencePostCode"" onfocus=""txtResidencePostCode_Focus();"" onblur=""txtResidencePostCode_Blur();""
                            type=""text"" class=""InputField"" style=""width: 50px;"" />
                    </td>
                </tr>
            </table>
         </td>
      </tr>
   </table>
</fieldset>


<div style=""height: 10px;""></div>
";

            return html;
        }

        public static void GetBasicInfoResidenceUIItems(AddEditTechnics page, bool isAddMode,
                                                          ref List<string> disabledClientControls,
                                                          ref List<string> hiddenClientControls)
        {
            bool isPreview = false;
            if (!String.IsNullOrEmpty(page.Request.Params["Preview"]))
            {
                isPreview = true;
            }

            bool screenDisabled = false;
            bool basicInfoDisabled = false;

            if (isAddMode) // add mode of page
            {
                screenDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey) == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_ADD") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_ADD_BASICINFO") == UIAccessLevel.Disabled;

                basicInfoDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey) == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_ADD") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_ADD_BASICINFO") == UIAccessLevel.Disabled;

                UIAccessLevel l;

                l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_ADD_BASICINFO_RESIDENCECITY");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblResidencePostCode");
                    disabledClientControls.Add("txtResidencePostCode");
                    disabledClientControls.Add("lblResidenceCity");
                    disabledClientControls.Add("ddResidenceCity");
                    disabledClientControls.Add("lblResidenceRegion");
                    disabledClientControls.Add("ddResidenceRegion");
                    disabledClientControls.Add("lblResidenceMunicipality");
                    disabledClientControls.Add("ddResidenceMunicipality");
                    disabledClientControls.Add("lblResidenceDistrict");
                    disabledClientControls.Add("ddResidenceDistrict");
                    hiddenClientControls.Add("btnImgCopyOwnerAddressCont");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblResidencePostCode");
                    hiddenClientControls.Add("txtResidencePostCode");
                    hiddenClientControls.Add("lblResidenceCity");
                    hiddenClientControls.Add("ddResidenceCity");
                    hiddenClientControls.Add("lblResidenceRegion");
                    hiddenClientControls.Add("ddResidenceRegion");
                    hiddenClientControls.Add("lblResidenceMunicipality");
                    hiddenClientControls.Add("ddResidenceMunicipality");
                    hiddenClientControls.Add("lblResidenceDistrict");
                    hiddenClientControls.Add("ddResidenceDistrict");
                    hiddenClientControls.Add("btnImgCopyOwnerAddressCont");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_ADD_BASICINFO_RESIDENCEADDRESS");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblResidenceAddress");
                    disabledClientControls.Add("txtResidenceAddress");
                    hiddenClientControls.Add("btnImgCopyOwnerAddressCont");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblResidenceAddress");
                    hiddenClientControls.Add("txtResidenceAddress");
                    hiddenClientControls.Add("btnImgCopyOwnerAddressCont");
                }
            }
            else // edit mode of page
            {
                screenDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey) == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_BASICINFO") == UIAccessLevel.Disabled || isPreview;

                basicInfoDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey) == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_BASICINFO") == UIAccessLevel.Disabled;

                UIAccessLevel l;

                l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_BASICINFO_RESIDENCECITY");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblResidencePostCode");
                    disabledClientControls.Add("txtResidencePostCode");
                    disabledClientControls.Add("lblResidenceCity");
                    disabledClientControls.Add("ddResidenceCity");
                    disabledClientControls.Add("lblResidenceRegion");
                    disabledClientControls.Add("ddResidenceRegion");
                    disabledClientControls.Add("lblResidenceMunicipality");
                    disabledClientControls.Add("ddResidenceMunicipality");
                    disabledClientControls.Add("lblResidenceDistrict");
                    disabledClientControls.Add("ddResidenceDistrict");
                    hiddenClientControls.Add("btnImgCopyOwnerAddressCont");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblResidencePostCode");
                    hiddenClientControls.Add("txtResidencePostCode");
                    hiddenClientControls.Add("lblResidenceCity");
                    hiddenClientControls.Add("ddResidenceCity");
                    hiddenClientControls.Add("lblResidenceRegion");
                    hiddenClientControls.Add("ddResidenceRegion");
                    hiddenClientControls.Add("lblResidenceMunicipality");
                    hiddenClientControls.Add("ddResidenceMunicipality");
                    hiddenClientControls.Add("lblResidenceDistrict");
                    hiddenClientControls.Add("ddResidenceDistrict");
                    hiddenClientControls.Add("btnImgCopyOwnerAddressCont");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_BASICINFO_RESIDENCEADDRESS");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblResidenceAddress");
                    disabledClientControls.Add("txtResidenceAddress");
                    hiddenClientControls.Add("btnImgCopyOwnerAddressCont");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblResidenceAddress");
                    hiddenClientControls.Add("txtResidenceAddress");
                    hiddenClientControls.Add("btnImgCopyOwnerAddressCont");
                }
            }
        }
    }
}
