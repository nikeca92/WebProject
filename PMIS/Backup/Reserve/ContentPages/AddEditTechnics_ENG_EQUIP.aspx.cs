using System;
using System.Collections.Generic;
using System.Text;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class AddEditTechnics_ENG_EQUIP : RESPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadBasicInfo")
            {
                JSLoadBasicInfo();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveBasicInfo")
            {
                JSSaveBasicInfo();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSCheckRegNumber")
            {
                JSCheckRegNumber();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRefreshEngEquipKindList")
            {
                JSRefreshEngEquipKindList();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRefreshEngEquipTypeList")
            {
                JSRefreshEngEquipTypeList();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            //if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRepopulateEngEquipBaseModels")
            //{
            //    JSRepopulateEngEquipBaseModels();
            //    return;
            //}

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRefreshEngEquipBaseKindList")
            {
                JSRefreshEngEquipBaseKindList();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRefreshEngEquipBaseTypeList")
            {
                JSRefreshEngEquipBaseTypeList();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRefreshEngEquipBaseEngineTypeList")
            {
                JSRefreshEngEquipBaseEngineTypeList();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRefreshEngEquipWorkingBodyKindList")
            {
                JSRefreshEngEquipWorkingBodyKindList();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRefreshEngEquipWorkBodyEngineTypeList")
            {
                JSRefreshEngEquipWorkBodyEngineTypeList();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSChangeRegNumber")
            {
                JSChangeRegNumber();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadRegNumberHistory")
            {
                JSLoadRegNumberHistory();
                return;
            }
        }

        //Load Basic Info (ajax call)
        private void JSLoadBasicInfo()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            int technicsId = 0;
            if (!String.IsNullOrEmpty(Request.Form["TechnicsId"]))
                technicsId = int.Parse(Request.Form["TechnicsId"]);

            string stat = "";
            string response = "";

            try
            {
                EngEquip engEquip = EngEquipUtil.GetEngEquipByTechnicsId(technicsId, CurrentUser);
                TechnicsMilRepStatus currentMilRepStatus = TechnicsMilRepStatusUtil.GetTechnicsMilRepCurrentStatusByTechnicsId(engEquip.TechnicsId, CurrentUser);
                string currMilRepStatusName = (currentMilRepStatus != null ? currentMilRepStatus.TechMilitaryReportStatus.TechMilitaryReportStatusName : TechMilitaryReportStatusUtil.GetLabelWhenLackOfStatus());
           
                stat = AJAXTools.OK;

                response = @"
                    <engEquip>
                         <technicsId>" + AJAXTools.EncodeForXML(engEquip.TechnicsId.ToString()) + @"</technicsId>
                         <engEquipId>" + AJAXTools.EncodeForXML(engEquip.EngEquipId.ToString()) + @"</engEquipId>
                         <regNumber>" + AJAXTools.EncodeForXML(engEquip.RegNumber) + @"</regNumber>
                         <inventoryNumber>" + AJAXTools.EncodeForXML(engEquip.InventoryNumber) + @"</inventoryNumber>
                         <technicsCategoryId>" + AJAXTools.EncodeForXML(engEquip.Technics.TechnicsCategoryId.HasValue ? engEquip.Technics.TechnicsCategoryId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</technicsCategoryId>
                         <engEquipKindId>" + AJAXTools.EncodeForXML(engEquip.EngEquipKindId.HasValue ? engEquip.EngEquipKindId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</engEquipKindId>
                         <engEquipTypeId>" + AJAXTools.EncodeForXML(engEquip.EngEquipTypeId.HasValue ? engEquip.EngEquipTypeId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</engEquipTypeId>
                         <lastModified>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDateTime(engEquip.Technics.LastModifiedDate)) + @"</lastModified>
                         <resMilRepStatus>" + AJAXTools.EncodeForXML(currMilRepStatusName) + @"</resMilRepStatus>";

                         //<engEquipBaseMakeId>" + AJAXTools.EncodeForXML(engEquip.EngEquipBaseMakeId.HasValue ? engEquip.EngEquipBaseMakeId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</engEquipBaseMakeId>
                         //<engEquipBaseModelId>" + AJAXTools.EncodeForXML(engEquip.EngEquipBaseModelId.HasValue ? engEquip.EngEquipBaseModelId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</engEquipBaseModelId>

                response += @"
                         <engEquipBaseMakeName>" + AJAXTools.EncodeForXML(engEquip.EngEquipBaseMakeName) + @"</engEquipBaseMakeName>
                         <engEquipBaseModelName>" + AJAXTools.EncodeForXML(engEquip.EngEquipBaseModelName) + @"</engEquipBaseModelName>
                         <engEquipBaseKindId>" + AJAXTools.EncodeForXML(engEquip.EngEquipBaseKindId.HasValue ? engEquip.EngEquipBaseKindId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</engEquipBaseKindId>
                         <engEquipBaseTypeId>" + AJAXTools.EncodeForXML(engEquip.EngEquipBaseTypeId.HasValue ? engEquip.EngEquipBaseTypeId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</engEquipBaseTypeId>
                         <engEquipBaseEngineTypeId>" + AJAXTools.EncodeForXML(engEquip.EngEquipBaseEngineTypeId.HasValue ? engEquip.EngEquipBaseEngineTypeId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</engEquipBaseEngineTypeId>
                         <baseFirstRegDate>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(engEquip.BaseFirstRegDate)) + @"</baseFirstRegDate>
                         <baseMileage>" + AJAXTools.EncodeForXML(engEquip.BaseMileage.HasValue ? engEquip.BaseMileage.ToString() : "") + @"</baseMileage>
                         <workingBodyPerformancePerHour>" + AJAXTools.EncodeForXML(engEquip.WorkingBodyPerformancePerHour.HasValue ? engEquip.WorkingBodyPerformancePerHour.ToString() : "") + @"</workingBodyPerformancePerHour>
                         <engEquipWorkingBodyKindId>" + AJAXTools.EncodeForXML(engEquip.EngEquipWorkingBodyKindId.HasValue ? engEquip.EngEquipWorkingBodyKindId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</engEquipWorkingBodyKindId>
                         <workingBodyFirstRegDate>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(engEquip.WorkingBodyFirstRegDate)) + @"</workingBodyFirstRegDate> 
                         <engEquipWorkBodyEngineTypeId>" + AJAXTools.EncodeForXML(engEquip.EngEquipWorkBodyEngineTypeId.HasValue ? engEquip.EngEquipWorkBodyEngineTypeId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</engEquipWorkBodyEngineTypeId>
                         <residenceCityId>" + AJAXTools.EncodeForXML(engEquip.Technics.ResidenceCityId != null ? engEquip.Technics.ResidenceCityId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</residenceCityId>
                         <residencePostCode>" + AJAXTools.EncodeForXML(engEquip.Technics.ResidencePostCode) + @"</residencePostCode>
                         <residenceRegionId>" + AJAXTools.EncodeForXML(engEquip.Technics.ResidenceCityId != null ? engEquip.Technics.ResidenceCity.RegionId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</residenceRegionId>
                         <residenceMunicipalityId>" + AJAXTools.EncodeForXML(engEquip.Technics.ResidenceCityId != null ? engEquip.Technics.ResidenceCity.MunicipalityId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</residenceMunicipalityId>
                         <residenceDistrictId>" + AJAXTools.EncodeForXML(engEquip.Technics.ResidenceDistrictId != null ? engEquip.Technics.ResidenceDistrict.DistrictId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</residenceDistrictId>
                         <residenceAddress>" + AJAXTools.EncodeForXML(engEquip.Technics.ResidenceAddress) + @"</residenceAddress>
                         <currMilDepartment>" + AJAXTools.EncodeForXML(engEquip.Technics.CurrTechMilRepStatus != null && engEquip.Technics.CurrTechMilRepStatus.SourceMilDepartment != null ? engEquip.Technics.CurrTechMilRepStatus.SourceMilDepartment.MilitaryDepartmentName : TechMilitaryReportStatusUtil.GetLabelWhenLackOfStatus()) + @"</currMilDepartment>
                         <normativeTechnicsId>" + AJAXTools.EncodeForXML(engEquip.Technics.NormativeTechnicsId != null ? engEquip.Technics.NormativeTechnics.NormativeTechnicsId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</normativeTechnicsId>
                         <normativeCode>" + AJAXTools.EncodeForXML(engEquip.Technics.NormativeTechnicsId != null ? engEquip.Technics.NormativeTechnics.NormativeCode : "") + @"</normativeCode>
                    </engEquip>";

                //response += "<eeb_model>" +
                //            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                //            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                //            "</eeb_model>";

                //if (engEquip.EngEquipBaseMakeId.HasValue)
                //{
                //    foreach (EngEquipBaseModel engEquipBaseModel in engEquip.EngEquipBaseMake.EngEquipBaseModels)
                //    {
                //        response += "<eeb_model>" +
                //                    "<id>" + engEquipBaseModel.EngEquipBaseModelId.ToString() + "</id>" +
                //                    "<name>" + AJAXTools.EncodeForXML(engEquipBaseModel.EngEquipBaseModelName) + "</name>" +
                //                    "</eeb_model>";
                //    }
                //}

                if (engEquip.Technics.ResidenceCityId != null)
                {
                    List<Municipality> municipalities = MunicipalityUtil.GetMunicipalities(engEquip.Technics.ResidenceCity.RegionId, CurrentUser);
                    List<City> cities = CityUtil.GetCities(engEquip.Technics.ResidenceCity.MunicipalityId, CurrentUser);
                    List<District> districts = engEquip.Technics.ResidenceCity.Districts;

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

        //Save Basic information (ajax call)
        private void JSSaveBasicInfo()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            int? technicsId = null;
            if (!String.IsNullOrEmpty(Request.Params["TechnicsId"]))
            {
                technicsId = int.Parse(Request.Params["TechnicsId"]);
            }

            int? engEquipId = null;
            if (!String.IsNullOrEmpty(Request.Params["EngEquipId"]))
            {
                engEquipId = int.Parse(Request.Params["EngEquipId"]);
            }

            string regNumber = Request.Params["RegNumber"];
            string inventoryNumber = Request.Params["InventoryNumber"];
            
            int? technicsCategoryId = null;
            if (!String.IsNullOrEmpty(Request.Params["TechnicsCategoryId"]) &&
                Request.Params["TechnicsCategoryId"] != ListItems.GetOptionChooseOne().Value)
            {
                technicsCategoryId = int.Parse(Request.Params["TechnicsCategoryId"]);
            }

            int? engEquipKindId = null;
            if (!String.IsNullOrEmpty(Request.Params["EngEquipKindId"]) &&
                Request.Params["EngEquipKindId"] != ListItems.GetOptionChooseOne().Value)
            {
                engEquipKindId = int.Parse(Request.Params["EngEquipKindId"]);
            }

            int? engEquipTypeId = null;
            if (!String.IsNullOrEmpty(Request.Params["EngEquipTypeId"]) &&
                Request.Params["EngEquipTypeId"] != ListItems.GetOptionChooseOne().Value)
            {
                engEquipTypeId = int.Parse(Request.Params["EngEquipTypeId"]);
            }

            //int? engEquipBaseMakeId = null;
            //if (!String.IsNullOrEmpty(Request.Params["EngEquipBaseMakeId"]) &&
            //    Request.Params["EngEquipBaseMakeId"] != ListItems.GetOptionChooseOne().Value)
            //{
            //    engEquipBaseMakeId = int.Parse(Request.Params["EngEquipBaseMakeId"]);
            //}

            //int? engEquipBaseModelId = null;
            //if (!String.IsNullOrEmpty(Request.Params["EngEquipBaseModelId"]) &&
            //    Request.Params["EngEquipBaseModelId"] != ListItems.GetOptionChooseOne().Value)
            //{
            //    engEquipBaseModelId = int.Parse(Request.Params["EngEquipBaseModelId"]);
            //}

            string engEquipBaseMakeName = Request.Params["EngEquipBaseMakeName"];
            string engEquipBaseModelName = Request.Params["EngEquipBaseModelName"];

            int? engEquipBaseKindId = null;
            if (!String.IsNullOrEmpty(Request.Params["EngEquipBaseKindId"]) &&
                Request.Params["EngEquipBaseKindId"] != ListItems.GetOptionChooseOne().Value)
            {
                engEquipBaseKindId = int.Parse(Request.Params["EngEquipBaseKindId"]);
            }

            int? engEquipBaseTypeId = null;
            if (!String.IsNullOrEmpty(Request.Params["EngEquipBaseTypeId"]) &&
                Request.Params["EngEquipBaseTypeId"] != ListItems.GetOptionChooseOne().Value)
            {
                engEquipBaseTypeId = int.Parse(Request.Params["EngEquipBaseTypeId"]);
            }

            int? engEquipBaseEngineTypeId = null;
            if (!String.IsNullOrEmpty(Request.Params["EngEquipBaseEngineTypeId"]) &&
                Request.Params["EngEquipBaseEngineTypeId"] != ListItems.GetOptionChooseOne().Value)
            {
                engEquipBaseEngineTypeId = int.Parse(Request.Params["EngEquipBaseEngineTypeId"]);
            }

            DateTime? baseFirstRegDate = null;
            if (!String.IsNullOrEmpty(Request.Params["BaseFirstRegDate"]))
            {
                baseFirstRegDate = CommonFunctions.ParseDate(Request.Params["BaseFirstRegDate"]);
            }

            decimal? baseMileage = null;
            if (!String.IsNullOrEmpty(Request.Params["BaseMileage"]))
            {
                baseMileage = decimal.Parse(Request.Params["BaseMileage"]);
            }

            decimal? workingBodyPerformancePerHour = null;
            if (!String.IsNullOrEmpty(Request.Params["WorkingBodyPerformancePerHour"]))
            {
                workingBodyPerformancePerHour = decimal.Parse(Request.Params["WorkingBodyPerformancePerHour"]);
            }

            int? engEquipWorkingBodyKindId = null;
            if (!String.IsNullOrEmpty(Request.Params["EngEquipWorkingBodyKindId"]) &&
                Request.Params["EngEquipWorkingBodyKindId"] != ListItems.GetOptionChooseOne().Value)
            {
                engEquipWorkingBodyKindId = int.Parse(Request.Params["EngEquipWorkingBodyKindId"]);
            }

            DateTime? workingBodyFirstRegDate = null;
            if (!String.IsNullOrEmpty(Request.Params["WorkingBodyFirstRegDate"]))
            {
                workingBodyFirstRegDate = CommonFunctions.ParseDate(Request.Params["WorkingBodyFirstRegDate"]);
            }

            int? engEquipWorkBodyEngineTypeId = null;
            if (!String.IsNullOrEmpty(Request.Params["EngEquipWorkBodyEngineTypeId"]) &&
                Request.Params["EngEquipWorkBodyEngineTypeId"] != ListItems.GetOptionChooseOne().Value)
            {
                engEquipWorkBodyEngineTypeId = int.Parse(Request.Params["EngEquipWorkBodyEngineTypeId"]);
            }

            string residencePostCode = Request.Form["ResidencePostCode"];

            int? residenceCityId = null;
            if (!String.IsNullOrEmpty(Request.Form["ResidenceCityID"]) &&
                Request.Form["ResidenceCityID"] != ListItems.GetOptionChooseOne().Value)
            {
                residenceCityId = int.Parse(Request.Form["ResidenceCityID"]);
            }

            int? residenceDistrictId = null;
            if (!String.IsNullOrEmpty(Request.Form["ResidenceDistrictID"]) &&
                Request.Form["ResidenceDistrictID"] != ListItems.GetOptionChooseOne().Value)
            {
                residenceDistrictId = int.Parse(Request.Form["ResidenceDistrictID"]);
            }

            string residenceAddress = Request.Form["ResidenceAddress"];

            int? normativeTechnicsId = null;
            if (!String.IsNullOrEmpty(Request.Form["NormativeTechnicsId"]) &&
                Request.Form["NormativeTechnicsId"] != ListItems.GetOptionChooseOne().Value)
            {
                normativeTechnicsId = int.Parse(Request.Form["NormativeTechnicsId"]);
            }

            EngEquip engEquip = new EngEquip(CurrentUser);

            engEquip.EngEquipId = engEquipId.HasValue ? engEquipId.Value : 0;
            engEquip.TechnicsId = technicsId.HasValue ? technicsId.Value : 0;
            engEquip.RegNumber = regNumber;
            engEquip.InventoryNumber = inventoryNumber;
            engEquip.EngEquipKindId = engEquipKindId;
            engEquip.EngEquipTypeId = engEquipTypeId;

            //engEquip.EngEquipBaseMakeId = engEquipBaseMakeId;
            //engEquip.EngEquipBaseModelId = engEquipBaseModelId;

            engEquip.EngEquipBaseMakeName = engEquipBaseMakeName;
            engEquip.EngEquipBaseModelName = engEquipBaseModelName;
            
            engEquip.EngEquipBaseKindId = engEquipBaseKindId;
            engEquip.EngEquipBaseTypeId = engEquipBaseTypeId;
            engEquip.EngEquipBaseEngineTypeId = engEquipBaseEngineTypeId;
            engEquip.BaseFirstRegDate = baseFirstRegDate;
            engEquip.BaseMileage = baseMileage;
            engEquip.WorkingBodyPerformancePerHour = workingBodyPerformancePerHour;
            engEquip.EngEquipWorkingBodyKindId = engEquipWorkingBodyKindId;
            engEquip.WorkingBodyFirstRegDate = workingBodyFirstRegDate;
            engEquip.EngEquipWorkBodyEngineTypeId = engEquipWorkBodyEngineTypeId;

            engEquip.Technics = new Technics(CurrentUser);
            engEquip.Technics.TechnicsId = engEquip.TechnicsId;
            engEquip.Technics.TechnicsType = TechnicsTypeUtil.GetTechnicsType("ENG_EQUIP", CurrentUser);
            engEquip.Technics.TechnicsCategoryId = technicsCategoryId.HasValue ? technicsCategoryId.Value : (int?)null;
            engEquip.Technics.ItemsCount = 1;
            engEquip.Technics.ResidencePostCode = residencePostCode;
            engEquip.Technics.ResidenceCityId = residenceCityId;
            engEquip.Technics.ResidenceDistrictId = residenceDistrictId;
            engEquip.Technics.ResidenceAddress = residenceAddress;
            engEquip.Technics.NormativeTechnicsId = normativeTechnicsId;

            string stat = "";
            string response = "";

            try
            {
                //Track the changes into the Audit Trail 
                Change change = new Change(CurrentUser, "RES_Technics_ENG_EQUIP");

                EngEquipUtil.SaveEngEquip(engEquip, CurrentUser, change);

                //Write into the Audit Trail
                change.WriteLog();

                stat = AJAXTools.OK;
                response = @"<response>OK</response>
                             <technicsId>" + AJAXTools.EncodeForXML(engEquip.TechnicsId.ToString()) + @"</technicsId>
                             <engEquipId>" + AJAXTools.EncodeForXML(engEquip.EngEquipId.ToString()) + @"</engEquipId>";
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

        //Check reg Number(ajax call)
        private void JSCheckRegNumber()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string regNumber = Request.Params["RegNumber"];

            string stat = "";
            string response = "";

            try
            {
                int technicsId = 0;

                EngEquip engEquip = EngEquipUtil.GetEngEquipByRegNumber(regNumber, CurrentUser);

                if (engEquip != null)
                {
                    technicsId = engEquip.TechnicsId;
                }

                stat = AJAXTools.OK;

                response = @"
                    <technicsId>" + technicsId.ToString() + @"</technicsId>
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

        //Get the engEquip Base models for a particular engEquip Base make (ajax call)
        //private void JSRepopulateEngEquipBaseModels()
        //{
        //    if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
        //        GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP") == UIAccessLevel.Hidden)
        //        RedirectAjaxAccessDenied();

        //    string stat = "";
        //    string response = "";

        //    try
        //    {
        //        int engEquipBaseMakeId = 0;

        //        if (!String.IsNullOrEmpty(Request.Form["EngEquipBaseMakeId"]))
        //            engEquipBaseMakeId = int.Parse(Request.Form["EngEquipBaseMakeId"]);

        //        response = "<engEquipModels>";

        //        response += "<m>" +
        //                     "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
        //                     "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
        //                     "</m>";

        //        List<EngEquipBaseModel> models = EngEquipBaseModelUtil.GetAllEngEquipBaseModels(engEquipBaseMakeId, CurrentUser);

        //        foreach (EngEquipBaseModel model in models)
        //        {
        //            response += "<m>" +
        //                        "<id>" + model.EngEquipBaseModelId.ToString() + "</id>" +
        //                        "<name>" + AJAXTools.EncodeForXML(model.EngEquipBaseModelName) + "</name>" +
        //                        "</m>";
        //        }

        //        response += "</engEquipModels>";

        //        stat = AJAXTools.OK;
        //    }
        //    catch (Exception ex)
        //    {
        //        stat = AJAXTools.ERROR;
        //        response = AJAXTools.EncodeForXML(ex.Message);
        //    }

        //    AJAX a = new AJAX(response, stat, Response);
        //    a.Write();
        //    Response.End();
        //}

        //Refresh the list EngEquipKind (ajax call)
        private void JSRefreshEngEquipKindList()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<engEquipKind>";

                response += "<i>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</i>";

                List<GTableItem> engEquipKinds = GTableItemUtil.GetAllGTableItemsByTableName("EngEquipKind", ModuleKey, 1, 0, 0, CurrentUser);

                foreach (GTableItem engEquipKind in engEquipKinds)
                {
                    response += "<i>" +
                                "<id>" + engEquipKind.TableKey.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(engEquipKind.TableValue.ToString()) + "</name>" +
                                "</i>";
                }

                response += "</engEquipKind>";

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

        //Refresh the list EngEquipType (ajax call)
        private void JSRefreshEngEquipTypeList()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<engEquipType>";

                response += "<i>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</i>";

                List<GTableItem> engEquipTypes = GTableItemUtil.GetAllGTableItemsByTableName("EngEquipType", ModuleKey, 1, 0, 0, CurrentUser);

                foreach (GTableItem engEquipType in engEquipTypes)
                {
                    response += "<i>" +
                                "<id>" + engEquipType.TableKey.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(engEquipType.TableValue.ToString()) + "</name>" +
                                "</i>";
                }

                response += "</engEquipType>";

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

        //Refresh the list EngEquipBaseKind (ajax call)
        private void JSRefreshEngEquipBaseKindList()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<engEquipBaseKind>";

                response += "<i>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</i>";

                List<GTableItem> engEquipBaseKinds = GTableItemUtil.GetAllGTableItemsByTableName("EngEquipBaseKind", ModuleKey, 1, 0, 0, CurrentUser);

                foreach (GTableItem engEquipBaseKind in engEquipBaseKinds)
                {
                    response += "<i>" +
                                "<id>" + engEquipBaseKind.TableKey.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(engEquipBaseKind.TableValue.ToString()) + "</name>" +
                                "</i>";
                }

                response += "</engEquipBaseKind>";

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

        //Refresh the list EngEquipBaseType (ajax call)
        private void JSRefreshEngEquipBaseTypeList()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<engEquipBaseType>";

                response += "<i>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</i>";

                List<GTableItem> engEquipBaseTypes = GTableItemUtil.GetAllGTableItemsByTableName("EngEquipBaseType", ModuleKey, 1, 0, 0, CurrentUser);

                foreach (GTableItem engEquipBaseType in engEquipBaseTypes)
                {
                    response += "<i>" +
                                "<id>" + engEquipBaseType.TableKey.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(engEquipBaseType.TableValue.ToString()) + "</name>" +
                                "</i>";
                }

                response += "</engEquipBaseType>";

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

        //Refresh the list EngEquipBaseEngineType (ajax call)
        private void JSRefreshEngEquipBaseEngineTypeList()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<engEquipBaseEngineType>";

                response += "<i>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</i>";

                List<GTableItem> engEquipBaseEngineTypes = GTableItemUtil.GetAllGTableItemsByTableName("EngEquipBaseEngineType", ModuleKey, 1, 0, 0, CurrentUser);

                foreach (GTableItem engEquipBaseEngineType in engEquipBaseEngineTypes)
                {
                    response += "<i>" +
                                "<id>" + engEquipBaseEngineType.TableKey.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(engEquipBaseEngineType.TableValue.ToString()) + "</name>" +
                                "</i>";
                }

                response += "</engEquipBaseEngineType>";

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

        //Refresh the list EngEquipWorkingBodyKind (ajax call)
        private void JSRefreshEngEquipWorkingBodyKindList()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<engEquipWorkingBodyKind>";

                response += "<i>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</i>";

                List<GTableItem> engEquipWorkingBodyKinds = GTableItemUtil.GetAllGTableItemsByTableName("EngEquipWorkingBodyKind", ModuleKey, 1, 0, 0, CurrentUser);

                foreach (GTableItem engEquipWorkingBodyKind in engEquipWorkingBodyKinds)
                {
                    response += "<i>" +
                                "<id>" + engEquipWorkingBodyKind.TableKey.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(engEquipWorkingBodyKind.TableValue.ToString()) + "</name>" +
                                "</i>";
                }

                response += "</engEquipWorkingBodyKind>";

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

        //Refresh the list EngEquipWorkBodyEngineType (ajax call)
        private void JSRefreshEngEquipWorkBodyEngineTypeList()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<engEquipWorkBodyEngineType>";

                response += "<i>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</i>";

                List<GTableItem> engEquipWorkBodyEngineTypes = GTableItemUtil.GetAllGTableItemsByTableName("EngEquipWorkBodyEngineType", ModuleKey, 1, 0, 0, CurrentUser);

                foreach (GTableItem engEquipWorkBodyEngineType in engEquipWorkBodyEngineTypes)
                {
                    response += "<i>" +
                                "<id>" + engEquipWorkBodyEngineType.TableKey.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(engEquipWorkBodyEngineType.TableValue.ToString()) + "</name>" +
                                "</i>";
                }

                response += "</engEquipWorkBodyEngineType>";

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


        //Change the reg number (ajax call)
        private void JSChangeRegNumber()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            int engEquipId = int.Parse(Request.Params["EngEquipId"]);
            string newRegNumber = Request.Params["NewRegNumber"];
            
            string stat = "";
            string response = "";

            try
            {
                EngEquip existingEngEquip = EngEquipUtil.GetEngEquipByRegNumber(newRegNumber, CurrentUser);

                if (existingEngEquip == null)
                {
                    //Track the changes into the Audit Trail 
                    Change change = new Change(CurrentUser, "RES_Technics_ENG_EQUIP");

                    EngEquipUtil.ChangeRegNumber(engEquipId, newRegNumber, CurrentUser, change);

                    //Write into the Audit Trail
                    change.WriteLog();

                    stat = AJAXTools.OK;
                    response = @"<response>OK</response>";
                }
                else
                {
                    stat = AJAXTools.OK;
                    response = @"<response>Вече съществува запис с този регистрационен номер</response>";
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

        private void JSLoadRegNumberHistory()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<response>" + AJAXTools.EncodeForXML(GetRegNumberHistoryLightBox(CurrentUser)) + "</response>";

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

        public string GetRegNumberHistoryLightBox(User currentUser)
        {
            string html = "";

            string htmlNoResults = "";

            List<EngEquipRegNumber> engEquipRegNumbers = new List<EngEquipRegNumber>();
            int pageIndex = 1; //Default
            int pageLength = int.Parse(Config.GetWebSetting("RowsPerPage"));
            int allRows = 0;
            int maxPage = 1;
            int orderBy = 1; //Default

            if (Request.Params["PageIndex"] != null && Request.Params["OrderBy"] != null)
            {
                pageIndex = int.Parse(Request.Params["PageIndex"]);
                orderBy = int.Parse(Request.Params["OrderBy"]);
            }

            int engEquipId = 0;
            int.TryParse((Request.Params["EngEquipId"]).ToString(), out engEquipId);

            allRows = EngEquipUtil.GetAllEngEquipRegNumbersCount(engEquipId, currentUser);
            maxPage = allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            engEquipRegNumbers = EngEquipUtil.GetAllEngEquipRegNumbers(engEquipId, orderBy, pageIndex, pageLength, currentUser);

            // No data found
            if (engEquipRegNumbers.Count == 0)
            {
                htmlNoResults = "Няма намерени резултати";
            }

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
            html += @"<center>
                      <div style='min-height: 150px; margin-bottom: 10px;'>

                        <input type='hidden' id='hdnOrderBy' value='" + orderBy + @"' />
                        <input type='hidden' id='hdnPageIndex' value='" + pageIndex + @"' />
                        <input type='hidden' id='hdnPageMaxPage' value='" + maxPage + @"' />

                        <span class='HeaderText'>История на регистрационните номера</span><br /><br /><br />

                        <div style='text-align: center;'>
                           <div style='display: inline; position: relative; top: -10px;'>
                              <img id='btnTableFirst' " + btnFirst + @" alt='Първа страница' title='Първа страница' class='PaginationButton' onclick=""BtnRegNumberHistoryPagingClick('btnFirst');"" />
                              <img id='btnTablePrev' " + btnPrev + @" alt='Предишна страница' title='Предишна страница' class='PaginationButton' onclick=""BtnRegNumberHistoryPagingClick('btnPrev');"" />
                              <span id='lblTablePagination' class='PaginationLabel'>" + pageTablePagination + @"</span>
                              <img id='btnTableNext' " + btnNext + @" alt='Следваща страница' title='Следваща страница' class='PaginationButton' onclick=""BtnRegNumberHistoryPagingClick('btnNext');"" />
                              <img id='btnTableLast' " + btnLast + @" alt='Последна страница' title='Последна страница' class='PaginationButton' onclick=""BtnRegNumberHistoryPagingClick('btnLast');"" />
                              
                              <span style='padding: 0 30px'>&nbsp;</span>
                              <span style='text-align: right;'>Отиди на страница</span>
                              <input id='txtTableGotoPage' class='InputField' type='text' style='width: 30px;' value='' />
                              <img id='btnTableGoto' src='../Images/ButtonGoto.png' alt='Отиди на страница' title='Отиди на страница' class='PaginationButton' onclick=""BtnRegNumberHistoryPagingClick('btnPageGo');"" />
                           </div>
                        </div>

                        <table id='tblEngEquipRegNumberHistory' class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            
                         </thead>";

            //Set Table Results
            string headerStyle = "vertical-align: bottom;";

            //Setup the header of the grid
            html += @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='width: 120px; " + headerStyle + @"'>Регистрационен номер</th>
                           </tr></thead>";

            int counter = 1;

            //Iterate through all items and add them into the grid
            foreach (EngEquipRegNumber engEquipRegNumber in engEquipRegNumbers)
            {

                string cellStyle = "vertical-align: top;";

                html += @"<tr  class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"' >
                                 <td style='" + cellStyle + @"'>" + ((pageIndex - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + engEquipRegNumber.RegNumber + @"</td>
                              </tr>";

                counter++;
            }
            html += "</table>";

            html += "<input type='hidden' id='hdnEngEquipRegNumberHistoryCounter' value='" + counter + "' />";


            html += @"<div style='height: 10px;'>
                </div>

                 <div style='text-align: center'>
                   <span id='lblEngEquipRegNumberHistoryMessage'>" + htmlNoResults + @"</span><br/>
                </div>
";

            html += @"  </div>                        
                        <div id='btnCloseEngEquipRegNumberHistoryTable' runat='server' class='Button' onclick=""HideRegNumberHistoryLightBox();""><i></i><div style='width:70px; padding-left:5px;'>Затвори</div><b></b></div>
                      </center>";



            return html;
        }
    }


    public static class AddEditTechnics_ENG_EQUIP_PageUtil
    {
        public static string GetGeneralPanelRegNumberContent()
        {
            string html = "";
            html = @"<div id=""tdRegNumber"">
                     <span id=""lblRegNuber"" class=""InputLabel"" style=""vertical-align: top; position: relative; top: 4px;"">Регистрационен номер:</span>
                     <span id=""txtRegNumberCont""><input type=""text"" id=""txtRegNumber"" class=""RequiredInputField"" style=""width: 90px; display: none;"" maxlength=""20""
                                      onfocus=""RegNumberFocus();"" onblur=""RegNumberBlur();"" /></span>
                      <span id=""lblRegNumberValueCont""><span id=""lblRegNumberValue"" class=""ReadOnlyValue"" style=""display: none; vertical-align: top; position: relative; top: 4px;""></span></span>
                    <span id=""imgEditRegNumberCont""><img id=""imgEditRegNumber"" alt=""Промяна на регистрационния номер"" title=""Промяна на регистрационния номер"" style=""cursor: pointer; display: none;"" src=""../Images/list_edit.png"" onclick=""ChangeRegNumber();"" /></span>
                    <span id=""imgHistoryRegNumberCont""><img id=""imgHistoryRegNumber"" alt=""История на регистрационните номера"" title=""История на регистрационните номера"" style=""cursor: pointer; width: 18px; height: 18px; display: none;"" src=""../Images/index_view.png"" onclick=""RegNumberHistory_Click();"" /></span>
                    </div>
                    <div id=""ChangeRegNumberLightBox"" class=""ChangeVehicleRegNumberLightBox"" style=""display: none; text-align: center;"">
                        <center>
                            <table width=""80%"" style=""text-align: center;"">
                                <colgroup style=""width: 40%"">
                                </colgroup>
                                <colgroup style=""width: 60%"">
                                </colgroup>
                                <tr style=""height: 15px"">
                                </tr>
                                <tr>
                                    <td colspan=""2"" align=""center"">
                                        <span class=""HeaderText"" style=""text-align: center;"">Промяна на регистрационен номер</span>
                                    </td>
                                </tr>
                                <tr style=""height: 15px"">
                                </tr>
                                <tr style=""min-height: 17px"">
                                    <td style=""text-align: right;"">
                                        <span id=""lblCurrRegNumber"" class=""InputLabel"">Текущ рег. номер:</span>
                                    </td>
                                    <td style=""text-align: left;"">
                                        <span id=""lblCurrRegNumberValue"" class=""ReadOnlyValue""></span>
                                    </td>
                                </tr>
                                <tr style=""min-height: 17px"">
                                    <td style=""text-align: right;"">
                                        <span id=""lblNewRegNumber"" class=""InputLabel"">Нов рег. номер:</span>
                                    </td>
                                    <td style=""text-align: left;"">
                                        <input type=""text"" id=""txtNewRegNumber"" class=""RequiredInputField"" onblur=""NewRegNumberBlur();"" style=""width: 90px;"" maxlength=""20"" UnsavedCheckSkipMe=""true"" />
                                    </td>
                                </tr>                      
                                <tr style=""height: 35px"">
                                    <td colspan=""2"" style=""padding-top: 5px;"">
                                        <span id=""spanChangeRegNumberLightBoxMessage"" class=""ErrorText"" style=""display: none;"">
                                        </span>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan=""2"" style=""text-align: center;"">
                                        <table style=""margin: 0 auto;"">
                                            <tr>
                                                <td>
                                                    <div id=""btnSaveChangeRegNumberLightBox"" style=""display: inline;"" onclick=""SaveChangeRegNumberLightBox();""
                                                        class=""Button"">
                                                        <i></i>
                                                        <div id=""btnChangeRegNumberLightBoxText"" style=""width: 70px;"">
                                                            Запис</div>
                                                        <b></b>
                                                    </div>
                                                    <div id=""btnCloseChangeRegNumberLightBox"" style=""display: inline;"" onclick=""HideChangeRegNumberLightBox();""
                                                        class=""Button"">
                                                        <i></i>
                                                        <div id=""btnCloseChangeRegNumberLightBox"" style=""width: 70px;"">
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
                    </div>

                    <div id=""divRegNumberHistoryLightBox"" style=""display: none;"" class=""lboxRegNumberHistory""></div>
                 
";

            return html;
        }

        public static void GetGeneralPanelUIItems(AddEditTechnics page, bool isAddMode,
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
            bool screenHidden = false;
            bool basicInfoHidden = false;


            if (isAddMode) // add mode of page
            {
                screenDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_ADD") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_ADD_BASICINFO") == UIAccessLevel.Disabled;

                screenHidden = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_ADD") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_ADD_BASICINFO") == UIAccessLevel.Hidden;

                basicInfoDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_ADD") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_ADD_BASICINFO") == UIAccessLevel.Disabled;

                basicInfoHidden = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_ADD") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_ADD_BASICINFO") == UIAccessLevel.Hidden;


                UIAccessLevel l;

                l = page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_ADD_BASICINFO_REGNUMBER");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblRegNuber");
                    disabledClientControls.Add("txtRegNumber");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblRegNuber");
                    hiddenClientControls.Add("txtRegNumberCont");
                    hiddenClientControls.Add("lblRegNumberValue");
                }

            }
            else // edit mode of page
            {
                screenDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_EDIT") == UIAccessLevel.Disabled || isPreview;


                screenHidden = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_EDIT") == UIAccessLevel.Hidden;

                basicInfoDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_EDIT") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_EDIT_BASICINFO") == UIAccessLevel.Disabled;

                basicInfoHidden = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_EDIT") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_EDIT_BASICINFO") == UIAccessLevel.Hidden;


                UIAccessLevel l;

                l = page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_EDIT_BASICINFO_REGNUMBER");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblRegNuber");
                    disabledClientControls.Add("txtRegNumber");
                    hiddenClientControls.Add("imgEditRegNumberCont");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblRegNuber");
                    hiddenClientControls.Add("txtRegNumberCont");
                    hiddenClientControls.Add("lblRegNumberValueCont");
                    hiddenClientControls.Add("imgEditRegNumberCont");
                    hiddenClientControls.Add("imgHistoryRegNumberCont");
                }
            }
        }

        public static string GetBasicInfoTabContent(AddEditTechnics page)
        {
            List<IDropDownItem> ddiTechnicsCategories = new List<IDropDownItem>();
            List<TechnicsCategory> technicsCategories = TechnicsCategoryUtil.GetAllTechnicsCategories(page.CurrentUser);

            foreach (TechnicsCategory technicsCategory in technicsCategories)
            {
                ddiTechnicsCategories.Add(technicsCategory);
            }

            string techncisCategoriesHTML = ListItems.GetDropDownHtml(ddiTechnicsCategories, null, "ddTechnicsCategory", true, null, "", "style='width: 170px;'", true);


            List<GTableItem> engEquipKinds = GTableItemUtil.GetAllGTableItemsByTableName("EngEquipKind", page.ModuleKey, 1, 0, 0, page.CurrentUser);
            List<IDropDownItem> ddiEngEquipKinds = new List<IDropDownItem>();
            foreach (GTableItem engEquipKind in engEquipKinds)
            {
                ddiEngEquipKinds.Add(engEquipKind);
            }

            string engEquipKindsHTML = ListItems.GetDropDownHtml(ddiEngEquipKinds, null, "ddEngEquipKind", true, null, "", "style='width: 320px;'", true);
            string editEngEquipKindsHTML = @"<img id=""imgMaintEngEquipKind"" alt=""Редактиране на списъка"" title=""Редактиране на списъка"" style=""cursor: pointer;"" src=""../Images/list_edit.png"" onclick=""ShowGTable('EngEquipKind', 1, 1, RefreshEngEquipKindList);"" />";

            List<GTableItem> engEquipTypes = GTableItemUtil.GetAllGTableItemsByTableName("EngEquipType", page.ModuleKey, 1, 0, 0, page.CurrentUser);
            List<IDropDownItem> ddiEngEquipType = new List<IDropDownItem>();
            foreach (GTableItem engEquipType in engEquipTypes)
            {
                ddiEngEquipType.Add(engEquipType);
            }

            string engEquipTypesHTML = ListItems.GetDropDownHtml(ddiEngEquipType, null, "ddEngEquipType", true, null, "", "style='width: 320px;'", true);
            string editEngEquipTypeHTML = @"<img id=""imgMaintEngEquipType"" alt=""Редактиране на списъка"" title=""Редактиране на списъка"" style=""cursor: pointer;"" src=""../Images/list_edit.png"" onclick=""ShowGTable('EngEquipType', 1, 1, RefreshEngEquipTypeList);"" />";


            //List<IDropDownItem> ddiEngEquipBaseMakes = new List<IDropDownItem>();
            //List<EngEquipBaseMake> engEquipBaseMakes = EngEquipBaseMakeUtil.GetAllEngEquipBaseMakes(page.CurrentUser);

            //foreach (EngEquipBaseMake engEquipBaseMake in engEquipBaseMakes)
            //{
            //    ddiEngEquipBaseMakes.Add(engEquipBaseMake);
            //}

            //string engEquipBaseMakesHTML = ListItems.GetDropDownHtml(ddiEngEquipBaseMakes, null, "ddEngEquipBaseMake", true, null, "RepopulateEngEquipBaseModels(this.value);", "style='width: 240px;'", true);

            //List<IDropDownItem> ddiEngEquipBaseModels = new List<IDropDownItem>();
            //DropDownItem blankItem = new DropDownItem();
            //blankItem.Txt = ListItems.GetOptionChooseOne().Text;
            //blankItem.Val = ListItems.GetOptionChooseOne().Value;
            //ddiEngEquipBaseModels.Add(blankItem);

            //string engEquipBaseModelsHTML = ListItems.GetDropDownHtml(ddiEngEquipBaseModels, null, "ddEngEquipBaseModel", false, null, "", "style='width: 240px;'", true);


            List<GTableItem> engEquipBaseKinds = GTableItemUtil.GetAllGTableItemsByTableName("EngEquipBaseKind", page.ModuleKey, 1, 0, 0, page.CurrentUser);
            List<IDropDownItem> ddiEngEquipBaseKinds = new List<IDropDownItem>();
            foreach (GTableItem engEquipBaseKind in engEquipBaseKinds)
            {
                ddiEngEquipBaseKinds.Add(engEquipBaseKind);
            }

            string engEquipBaseKindsHTML = ListItems.GetDropDownHtml(ddiEngEquipBaseKinds, null, "ddEngEquipBaseKind", true, null, "", "style='width: 240px;'", true);
            string editEngEquipBaseKindsHTML = @"<img id=""imgMaintEngEquipBaseKind"" alt=""Редактиране на списъка"" title=""Редактиране на списъка"" style=""cursor: pointer;"" src=""../Images/list_edit.png"" onclick=""ShowGTable('EngEquipBaseKind', 1, 1, RefreshEngEquipBaseKindList);"" />";

            List<GTableItem> engEquipBaseTypes = GTableItemUtil.GetAllGTableItemsByTableName("EngEquipBaseType", page.ModuleKey, 1, 0, 0, page.CurrentUser);
            List<IDropDownItem> ddiEngEquipBaseType = new List<IDropDownItem>();
            foreach (GTableItem engEquipBaseType in engEquipBaseTypes)
            {
                ddiEngEquipBaseType.Add(engEquipBaseType);
            }

            string engEquipBaseTypesHTML = ListItems.GetDropDownHtml(ddiEngEquipBaseType, null, "ddEngEquipBaseType", true, null, "", "style='width: 240px;'", true);
            string editEngEquipBaseTypeHTML = @"<img id=""imgMaintEngEquipBaseType"" alt=""Редактиране на списъка"" title=""Редактиране на списъка"" style=""cursor: pointer;"" src=""../Images/list_edit.png"" onclick=""ShowGTable('EngEquipBaseType', 1, 1, RefreshEngEquipBaseTypeList);"" />";


            List<GTableItem> engEquipBaseEngineTypes = GTableItemUtil.GetAllGTableItemsByTableName("EngEquipBaseEngineType", page.ModuleKey, 1, 0, 0, page.CurrentUser);
            List<IDropDownItem> ddiEngEquipBaseEngineType = new List<IDropDownItem>();
            foreach (GTableItem engEquipBaseEngineType in engEquipBaseEngineTypes)
            {
                ddiEngEquipBaseEngineType.Add(engEquipBaseEngineType);
            }

            string engEquipBaseEngineTypesHTML = ListItems.GetDropDownHtml(ddiEngEquipBaseEngineType, null, "ddEngEquipBaseEngineType", true, null, "", "style='width: 240px;'", true);
            string editEngEquipBaseEngineTypeHTML = @"<img id=""imgMaintEngEquipBaseEngineType"" alt=""Редактиране на списъка"" title=""Редактиране на списъка"" style=""cursor: pointer;"" src=""../Images/list_edit.png"" onclick=""ShowGTable('EngEquipBaseEngineType', 1, 1, RefreshEngEquipBaseEngineTypeList);"" />";


            List<GTableItem> engEquipWorkingBodyKinds = GTableItemUtil.GetAllGTableItemsByTableName("EngEquipWorkingBodyKind", page.ModuleKey, 1, 0, 0, page.CurrentUser);
            List<IDropDownItem> ddiEngEquipWorkingBodyKind = new List<IDropDownItem>();
            foreach (GTableItem engEquipWorkingBodyKind in engEquipWorkingBodyKinds)
            {
                ddiEngEquipWorkingBodyKind.Add(engEquipWorkingBodyKind);
            }

            string engEquipWorkingBodyKindsHTML = ListItems.GetDropDownHtml(ddiEngEquipWorkingBodyKind, null, "ddEngEquipWorkingBodyKind", true, null, "", "style='width: 310px;'", true);
            string editEngEquipWorkingBodyKindHTML = @"<img id=""imgMaintEngEquipWorkingBodyKind"" alt=""Редактиране на списъка"" title=""Редактиране на списъка"" style=""cursor: pointer;"" src=""../Images/list_edit.png"" onclick=""ShowGTable('EngEquipWorkingBodyKind', 1, 1, RefreshEngEquipWorkingBodyKindList);"" />";


            List<GTableItem> engEquipWorkBodyEngineTypes = GTableItemUtil.GetAllGTableItemsByTableName("EngEquipWorkBodyEngineType", page.ModuleKey, 1, 0, 0, page.CurrentUser);
            List<IDropDownItem> ddiEngEquipWorkBodyEngineType = new List<IDropDownItem>();
            foreach (GTableItem engEquipWorkBodyEngineType in engEquipWorkBodyEngineTypes)
            {
                ddiEngEquipWorkBodyEngineType.Add(engEquipWorkBodyEngineType);
            }

            string engEquipWorkBodyEngineTypesHTML = ListItems.GetDropDownHtml(ddiEngEquipWorkBodyEngineType, null, "ddEngEquipWorkBodyEngineType", true, null, "", "style='width: 310px;'", true);
            string editEngEquipWorkBodyEngineTypeHTML = @"<img id=""imgMaintEngEquipWorkBodyEngineType"" alt=""Редактиране на списъка"" title=""Редактиране на списъка"" style=""cursor: pointer;"" src=""../Images/list_edit.png"" onclick=""ShowGTable('EngEquipWorkBodyEngineType', 1, 1, RefreshEngEquipWorkBodyEngineTypeList);"" />";


            string html = @"
<div style=""height: 10px;""></div>

<fieldset style=""width: 830px; padding: 0px;"">
   <table class=""InputRegion"" style=""width: 830px; padding: 10px; padding-top: 0px; margin-top: 0px; padding-left: 0px;"">
      <tr style=""height: 3px;"">
      </tr>
      <tr>
         
         <td style=""text-align: right;"">
            <span id=""lblInventoryNuber"" class=""InputLabel"">Инв. номер:</span>
         </td>
         <td style=""text-align: left;"">
            <input type=""text"" id=""txtInventoryNumber"" onblur=""InventoryNumberBlur();"" class=""InputField"" style=""width: 100px;"" maxlength=""50"" />
         </td>
         <td style=""text-align: right;"" colspan=""3"">
            <span id=""lblTechnicsCategory"" class=""InputLabel"">Категория:</span>
         </td>
         <td style=""text-align: left;"">
            " + techncisCategoriesHTML + @"
         </td>
      </tr>
      <tr>
         <td style=""text-align: left; padding-left: 20px;"" colspan=""8"">
            <span id=""lblEngEquipKind"" class=""InputLabel"">Вид:</span>
            " + engEquipKindsHTML + editEngEquipKindsHTML + @"
         
            <span id=""lblEngEquipType"" class=""InputLabel"" style=""padding-left: 53px;"">Тип:</span>
            " + engEquipTypesHTML + editEngEquipTypeHTML + @"
         </td>
      </tr>
   </table>
</fieldset>

<div style=""height: 10px;""></div>

<fieldset style=""width: 830px; padding: 0px;"">
   <table class=""InputRegion"" style=""width: 830px; padding: 10px; padding-top: 0px; margin-top: 0px;"">
      <tr style=""height: 3px;"">
      </tr>
      <tr>
         <td style=""text-align: left;"" colspan=""2"">
            <span style=""color: #0B4489; font-weight: bold; font-size: 1.1em; width: 100%; Position: relative; top: -5px;"">Базова машина</span>
         </td>
      </tr>
      <tr>
         <td style=""text-align: right;"">
            <span id=""lblEngEquipBaseMake"" class=""InputLabel"">Марка:</span>
         </td>
         <td style=""text-align: left;"">
            <input type=""text"" id=""txtEngEquipBaseMakeName"" class=""InputField"" style=""width: 235px;"" maxlength=""300"" />
         </td>
         <td style=""text-align: right;"">
            <span id=""lblEngEquipBaseModel"" class=""InputLabel"">Модел:</span>
         </td>
         <td style=""text-align: left;"">
            <input type=""text"" id=""txtEngEquipBaseModelName"" class=""InputField"" style=""width: 235px;"" maxlength=""300"" />
         </td>
      </tr>
      </tr>
         <td style=""text-align: right;"">
            <span id=""lblEngEquipBaseType"" class=""InputLabel"">Тип:</span>
         </td>
         <td style=""text-align: left;"">
            " + engEquipBaseTypesHTML + editEngEquipBaseTypeHTML + @"
         </td>
         <td style=""text-align: right;"">
            <span id=""lblBaseFirstRegDate"" class=""InputLabel"">Дата на първата регистрация:</span>
         </td>
         <td style=""text-align: left;"">
            <span id=""txtBaseFirstRegDateCont""><input type=""text"" id=""txtBaseFirstRegDate"" class='" + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" /></span>
         </td>
      </tr>
      <tr>
         <td style=""text-align: right;"">
            <span id=""lblEngEquipBaseKind"" class=""InputLabel"">Вид:</span>
         </td>
         <td style=""text-align: left;"">
            " + engEquipBaseKindsHTML + editEngEquipBaseKindsHTML + @"
         </td>
         <td style=""text-align: right;"">
            <span id=""lblBaseMileage"" class=""InputLabel"">Изминати километри:</span>
         </td>
         <td style=""text-align: left;"">
            <input type=""text"" id=""txtBaseMileage"" class=""InputField"" style=""width: 100px;"" maxlength=""50"" />
         </td>
      <tr>
      </tr>
         <td style=""text-align: right;"" colspan=""3"">
            <span id=""lblEngEquipBaseEngineType"" class=""InputLabel"">Вид гориво:</span>
         </td>
         <td style=""text-align: left;"">
            " + engEquipBaseEngineTypesHTML + editEngEquipBaseEngineTypeHTML + @"
         </td>
      </tr>
   </table>
</fieldset>

<div style=""height: 10px;""></div>

<fieldset style=""width: 830px; padding: 0px;"">
   <table class=""InputRegion"" style=""width: 830px; padding: 10px; padding-top: 0px; margin-top: 0px;"">
      <tr style=""height: 3px;"">
      </tr>
      <tr>
         <td style=""text-align: left;"">
            <span style=""color: #0B4489; font-weight: bold; font-size: 1.1em; width: 100%; Position: relative; top: -5px;"">Работен орган</span>
         </td>
      </tr>
      <tr>
         <td style=""text-align: right; width: 160px;"">
            <span id=""lblWorkingBodyPerformancePerHour"" class=""InputLabel"">Производителност за час:</span>
         </td>
         <td style=""text-align: left; width: 155px;"">
            <input type=""text"" id=""txtWorkingBodyPerformancePerHour"" class=""InputField"" style=""width: 100px;"" maxlength=""50"" />
         </td>
         <td style=""text-align: right; width: 230px;"">
            <span id=""lblWorkingBodyFirstRegDate"" class=""InputLabel"">Дата на първата регистрация:</span>
         </td>
         <td style=""text-align: left; width: 200px;"">
            <span id=""txtWorkingBodyFirstRegDateCont""><input type=""text"" id=""txtWorkingBodyFirstRegDate"" class='" + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" /></span>
         </td>
      </tr>
      </tr>
         <td style=""text-align: left;"" colspan=""4"">
            <span id=""lblEngEquipWorkingBodyKind"" class=""InputLabel"">Вид:</span>
            " + engEquipWorkingBodyKindsHTML + editEngEquipWorkingBodyKindHTML + @"
         
            <span id=""lblEngEquipWorkBodyEngineType"" class=""InputLabel"" style=""padding-left: 20px;"">Вид гориво:</span>
            " + engEquipWorkBodyEngineTypesHTML + editEngEquipWorkBodyEngineTypeHTML + @"
         </td>
      </tr>
   </table>
</fieldset>

" + AddEditTechnics_BasicInfo_PageUtil.GetBasicInfoResidenceContent(page) + @"
<div style=""height: 10px;""></div>

<input type=""hidden"" id=""hdnEngEquipId"" />
";
            return html;
        }

        public static string GetBasicInfoTabUIItems(AddEditTechnics page)
        {
            string UIItemsXML = "";

            bool isPreview = false;
            if (!String.IsNullOrEmpty(page.Request.Params["Preview"]))
            {
                isPreview = true;
            }

            bool screenDisabled = false;
            bool basicInfoDisabled = false;

            //Setup the "manually add positions" light-box
            List<string> disabledClientControls = new List<string>();
            List<string> hiddenClientControls = new List<string>();

            if (page.TechnicsId == 0) // add mode of page
            {
                screenDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_ADD") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_ADD_BASICINFO") == UIAccessLevel.Disabled;

                basicInfoDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_ADD") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_ADD_BASICINFO") == UIAccessLevel.Disabled;

                UIAccessLevel l;
                
                l = page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_ADD_BASICINFO_INVENTORYNUMBER");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblInventoryNuber");
                    disabledClientControls.Add("txtInventoryNumber");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblInventoryNuber");
                    hiddenClientControls.Add("txtInventoryNumber");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_ADD_BASICINFO_TECHNICSCATEGORY");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblTechnicsCategory");
                    disabledClientControls.Add("ddTechnicsCategory");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblTechnicsCategory");
                    hiddenClientControls.Add("ddTechnicsCategory");
                }


                l = page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_ADD_BASICINFO_ENGEQUIPKIND");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblEngEquipKind");
                    disabledClientControls.Add("ddEngEquipKind");
                    hiddenClientControls.Add("imgMaintEngEquipKind");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEngEquipKind");
                    hiddenClientControls.Add("ddEngEquipKind");
                    hiddenClientControls.Add("imgMaintEngEquipKind");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_ADD_BASICINFO_ENGEQUIPTYPE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblEngEquipType");
                    disabledClientControls.Add("ddEngEquipType");
                    hiddenClientControls.Add("imgMaintEngEquipType");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEngEquipType");
                    hiddenClientControls.Add("ddEngEquipType");
                    hiddenClientControls.Add("imgMaintEngEquipType");
                }


                l = page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_ADD_BASICINFO_ENGEQUIPBASEMAKE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblEngEquipBaseMake");
                    disabledClientControls.Add("txtEngEquipBaseMakeName");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEngEquipBaseMake");
                    hiddenClientControls.Add("txtEngEquipBaseMakeName");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_ADD_BASICINFO_ENGEQUIPBASEMODEL");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblEngEquipBaseModel");
                    disabledClientControls.Add("txtEngEquipBaseModelName");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEngEquipBaseModel");
                    hiddenClientControls.Add("txtEngEquipBaseModelName");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_ADD_BASICINFO_ENGEQUIPBASEKIND");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblEngEquipBaseKind");
                    disabledClientControls.Add("ddEngEquipBaseKind");
                    hiddenClientControls.Add("imgMaintEngEquipBaseKind");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEngEquipBaseKind");
                    hiddenClientControls.Add("ddEngEquipBaseKind");
                    hiddenClientControls.Add("imgMaintEngEquipBaseKind");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_ADD_BASICINFO_ENGEQUIPBASETYPE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblEngEquipBaseType");
                    disabledClientControls.Add("ddEngEquipBaseType");
                    hiddenClientControls.Add("imgMaintEngEquipBaseType");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEngEquipBaseType");
                    hiddenClientControls.Add("ddEngEquipBaseType");
                    hiddenClientControls.Add("imgMaintEngEquipBaseType");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_ADD_BASICINFO_ENGEQUIPBASEENGINETYPE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblEngEquipBaseEngineType");
                    disabledClientControls.Add("ddEngEquipBaseEngineType");
                    hiddenClientControls.Add("imgMaintEngEquipBaseEngineType");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEngEquipBaseEngineType");
                    hiddenClientControls.Add("ddEngEquipBaseEngineType");
                    hiddenClientControls.Add("imgMaintEngEquipBaseEngineType");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_ADD_BASICINFO_BASEFIRSTREGDATE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblBaseFirstRegDate");
                    disabledClientControls.Add("txtBaseFirstRegDate");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblBaseFirstRegDate");
                    hiddenClientControls.Add("txtBaseFirstRegDateCont");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_ADD_BASICINFO_BASEMILEAGE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblBaseMileage");
                    disabledClientControls.Add("txtBaseMileage");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblBaseMileage");
                    hiddenClientControls.Add("txtBaseMileage");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_ADD_BASICINFO_WORKINGBODYPERFORMANCEPERHOUR");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblWorkingBodyPerformancePerHour");
                    disabledClientControls.Add("txtWorkingBodyPerformancePerHour");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWorkingBodyPerformancePerHour");
                    hiddenClientControls.Add("txtWorkingBodyPerformancePerHour");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_ADD_BASICINFO_ENGEQUIPWORKINGBODYKIND");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblEngEquipWorkingBodyKind");
                    disabledClientControls.Add("ddEngEquipWorkingBodyKind");
                    hiddenClientControls.Add("imgMaintEngEquipWorkingBodyKind");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEngEquipWorkingBodyKind");
                    hiddenClientControls.Add("ddEngEquipWorkingBodyKind");
                    hiddenClientControls.Add("imgMaintEngEquipWorkingBodyKind");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_ADD_BASICINFO_WORKINGBODYFIRSTREGDATE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblWorkingBodyFirstRegDate");
                    disabledClientControls.Add("txtWorkingBodyFirstRegDate");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWorkingBodyFirstRegDate");
                    hiddenClientControls.Add("txtWorkingBodyFirstRegDateCont");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_ADD_BASICINFO_ENGEQUIPWORKINGBODYENGINETYPE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblEngEquipWorkBodyEngineType");
                    disabledClientControls.Add("ddEngEquipWorkBodyEngineType");
                    hiddenClientControls.Add("imgMaintEngEquipWorkBodyEngineType");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEngEquipWorkBodyEngineType");
                    hiddenClientControls.Add("ddEngEquipWorkBodyEngineType");
                    hiddenClientControls.Add("imgMaintEngEquipWorkBodyEngineType");
                }

                AddEditTechnics_BasicInfo_PageUtil.GetBasicInfoResidenceUIItems(page, true,
                    ref disabledClientControls, ref hiddenClientControls);
            }
            else // edit mode of page
            {
                screenDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_EDIT") == UIAccessLevel.Disabled || isPreview;

                basicInfoDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_EDIT") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_EDIT_BASICINFO") == UIAccessLevel.Disabled;

                UIAccessLevel l;
                
                l = page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_EDIT_BASICINFO_INVENTORYNUMBER");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblInventoryNuber");
                    disabledClientControls.Add("txtInventoryNumber");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblInventoryNuber");
                    hiddenClientControls.Add("txtInventoryNumber");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_EDIT_BASICINFO_TECHNICSCATEGORY");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblTechnicsCategory");
                    disabledClientControls.Add("ddTechnicsCategory");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblTechnicsCategory");
                    hiddenClientControls.Add("ddTechnicsCategory");
                }


                l = page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_EDIT_BASICINFO_ENGEQUIPKIND");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblEngEquipKind");
                    disabledClientControls.Add("ddEngEquipKind");
                    hiddenClientControls.Add("imgMaintEngEquipKind");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEngEquipKind");
                    hiddenClientControls.Add("ddEngEquipKind");
                    hiddenClientControls.Add("imgMaintEngEquipKind");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_EDIT_BASICINFO_ENGEQUIPTYPE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblEngEquipType");
                    disabledClientControls.Add("ddEngEquipType");
                    hiddenClientControls.Add("imgMaintEngEquipType");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEngEquipType");
                    hiddenClientControls.Add("ddEngEquipType");
                    hiddenClientControls.Add("imgMaintEngEquipType");
                }


                l = page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_EDIT_BASICINFO_ENGEQUIPBASEMAKE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblEngEquipBaseMake");
                    disabledClientControls.Add("txtEngEquipBaseMakeName");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEngEquipBaseMake");
                    hiddenClientControls.Add("txtEngEquipBaseMakeName");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_EDIT_BASICINFO_ENGEQUIPBASEMODEL");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblEngEquipBaseModel");
                    disabledClientControls.Add("txtEngEquipBaseModelName");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEngEquipBaseModel");
                    hiddenClientControls.Add("txtEngEquipBaseModelName");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_EDIT_BASICINFO_ENGEQUIPBASEKIND");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblEngEquipBaseKind");
                    disabledClientControls.Add("ddEngEquipBaseKind");
                    hiddenClientControls.Add("imgMaintEngEquipBaseKind");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEngEquipBaseKind");
                    hiddenClientControls.Add("ddEngEquipBaseKind");
                    hiddenClientControls.Add("imgMaintEngEquipBaseKind");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_EDIT_BASICINFO_ENGEQUIPBASETYPE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblEngEquipBaseType");
                    disabledClientControls.Add("ddEngEquipBaseType");
                    hiddenClientControls.Add("imgMaintEngEquipBaseType");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEngEquipBaseType");
                    hiddenClientControls.Add("ddEngEquipBaseType");
                    hiddenClientControls.Add("imgMaintEngEquipBaseType");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_EDIT_BASICINFO_ENGEQUIPBASEENGINETYPE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblEngEquipBaseEngineType");
                    disabledClientControls.Add("ddEngEquipBaseEngineType");
                    hiddenClientControls.Add("imgMaintEngEquipBaseEngineType");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEngEquipBaseEngineType");
                    hiddenClientControls.Add("ddEngEquipBaseEngineType");
                    hiddenClientControls.Add("imgMaintEngEquipBaseEngineType");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_EDIT_BASICINFO_BASEFIRSTREGDATE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblBaseFirstRegDate");
                    disabledClientControls.Add("txtBaseFirstRegDate");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblBaseFirstRegDate");
                    hiddenClientControls.Add("txtBaseFirstRegDateCont");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_EDIT_BASICINFO_BASEMILEAGE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblBaseMileage");
                    disabledClientControls.Add("txtBaseMileage");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblBaseMileage");
                    hiddenClientControls.Add("txtBaseMileage");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_EDIT_BASICINFO_WORKINGBODYPERFORMANCEPERHOUR");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblWorkingBodyPerformancePerHour");
                    disabledClientControls.Add("txtWorkingBodyPerformancePerHour");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWorkingBodyPerformancePerHour");
                    hiddenClientControls.Add("txtWorkingBodyPerformancePerHour");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_EDIT_BASICINFO_ENGEQUIPWORKINGBODYKIND");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblEngEquipWorkingBodyKind");
                    disabledClientControls.Add("ddEngEquipWorkingBodyKind");
                    hiddenClientControls.Add("imgMaintEngEquipWorkingBodyKind");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEngEquipWorkingBodyKind");
                    hiddenClientControls.Add("ddEngEquipWorkingBodyKind");
                    hiddenClientControls.Add("imgMaintEngEquipWorkingBodyKind");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_EDIT_BASICINFO_WORKINGBODYFIRSTREGDATE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblWorkingBodyFirstRegDate");
                    disabledClientControls.Add("txtWorkingBodyFirstRegDate");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWorkingBodyFirstRegDate");
                    hiddenClientControls.Add("txtWorkingBodyFirstRegDateCont");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_ENG_EQUIP_EDIT_BASICINFO_ENGEQUIPWORKINGBODYENGINETYPE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblEngEquipWorkBodyEngineType");
                    disabledClientControls.Add("ddEngEquipWorkBodyEngineType");
                    hiddenClientControls.Add("imgMaintEngEquipWorkBodyEngineType");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEngEquipWorkBodyEngineType");
                    hiddenClientControls.Add("ddEngEquipWorkBodyEngineType");
                    hiddenClientControls.Add("imgMaintEngEquipWorkBodyEngineType");
                }

                AddEditTechnics_BasicInfo_PageUtil.GetBasicInfoResidenceUIItems(page, false,
                    ref disabledClientControls, ref hiddenClientControls);
           }

            if (page.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled ||
                page.GetUIItemAccessLevel("RES_LISTMAINT_ENGEQUIPKIND") != UIAccessLevel.Enabled)
            {
                hiddenClientControls.Add("imgMaintEngEquipKind");
            }

            if (page.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled ||
                page.GetUIItemAccessLevel("RES_LISTMAINT_ENGEQUIPTYPE") != UIAccessLevel.Enabled)
            {
                hiddenClientControls.Add("imgMaintEngEquipType");
            }

            if (page.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled ||
                page.GetUIItemAccessLevel("RES_LISTMAINT_ENGEQUIPBASEKIND") != UIAccessLevel.Enabled)
            {
                hiddenClientControls.Add("imgMaintEngEquipBaseKind");
            }

            if (page.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled ||
                page.GetUIItemAccessLevel("RES_LISTMAINT_ENGEQUIPBASETYPE") != UIAccessLevel.Enabled)
            {
                hiddenClientControls.Add("imgMaintEngEquipBaseType");
            }

            if (page.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled ||
                page.GetUIItemAccessLevel("RES_LISTMAINT_ENGEQUIPBASEENGINETYPE") != UIAccessLevel.Enabled)
            {
                hiddenClientControls.Add("imgMaintEngEquipBaseEngineType");
            }

            if (page.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled ||
                page.GetUIItemAccessLevel("RES_LISTMAINT_ENGEQUIPWORKINGBODYKIND") != UIAccessLevel.Enabled)
            {
                hiddenClientControls.Add("imgMaintEngEquipWorkingBodyKind");
            }

            if (page.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled ||
                page.GetUIItemAccessLevel("RES_LISTMAINT_ENGEQUIPWORKBODYENGINETYPE") != UIAccessLevel.Enabled)
            {
                hiddenClientControls.Add("imgMaintEngEquipWorkBodyEngineType");
            }

            string disabledClientControlsIds = "";
            string hiddenClientControlsIds = "";

            foreach(string disabledClientControl in disabledClientControls)
            {
                disabledClientControlsIds += (String.IsNullOrEmpty(disabledClientControlsIds) ? "" : ",") +
                    disabledClientControl;
            }

            foreach(string hiddenClientControl in hiddenClientControls)
            {
                hiddenClientControlsIds += (String.IsNullOrEmpty(hiddenClientControlsIds) ? "" : ",") +
                    hiddenClientControl;
            }

            UIItemsXML = "<disabledClientControls>" + AJAXTools.EncodeForXML(disabledClientControlsIds) + "</disabledClientControls>" +
                         "<hiddenClientControls>" + AJAXTools.EncodeForXML(hiddenClientControlsIds) + "</hiddenClientControls>";

            return UIItemsXML;
        }
    }
}
