using System;
using System.Collections.Generic;
using System.Text;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class AddEditTechnics_TRACTORS : RESPage
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
            //if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRepopulateTractorModels")
            //{
            //    JSRepopulateTractorModels();
            //    return;
            //}

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRefreshTractorKindList")
            {
                JSRefreshTractorKindList();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRefreshTractorTypeList")
            {
                JSRefreshTractorTypeList();
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
                GetUIItemAccessLevel("RES_TECHNICS_TRACTORS") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            int technicsId = 0;
            if (!String.IsNullOrEmpty(Request.Form["TechnicsId"]))
                technicsId = int.Parse(Request.Form["TechnicsId"]);

            string stat = "";
            string response = "";

            try
            {
                Tractor tractor = TractorUtil.GetTractorByTechnicsId(technicsId, CurrentUser);
                TechnicsMilRepStatus currentMilRepStatus = TechnicsMilRepStatusUtil.GetTechnicsMilRepCurrentStatusByTechnicsId(tractor.TechnicsId, CurrentUser);
                string currMilRepStatusName = (currentMilRepStatus != null ? currentMilRepStatus.TechMilitaryReportStatus.TechMilitaryReportStatusName : TechMilitaryReportStatusUtil.GetLabelWhenLackOfStatus());
           
                stat = AJAXTools.OK;

                response = @"
                    <tractor>
                         <technicsId>" + AJAXTools.EncodeForXML(tractor.TechnicsId.ToString()) + @"</technicsId>
                         <tractorId>" + AJAXTools.EncodeForXML(tractor.TractorId.ToString()) + @"</tractorId>
                         <regNumber>" + AJAXTools.EncodeForXML(tractor.RegNumber) + @"</regNumber>
                         <inventoryNumber>" + AJAXTools.EncodeForXML(tractor.InventoryNumber) + @"</inventoryNumber>
                         <technicsCategoryId>" + AJAXTools.EncodeForXML(tractor.Technics.TechnicsCategoryId.HasValue ? tractor.Technics.TechnicsCategoryId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</technicsCategoryId>
                         
                         <lastModified>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDateTime(tractor.Technics.LastModifiedDate)) + @"</lastModified>
                         <resMilRepStatus>" + AJAXTools.EncodeForXML(currMilRepStatusName) + @"</resMilRepStatus>";
            
                         //<tractorMakeId>" + AJAXTools.EncodeForXML(tractor.TractorMakeId.HasValue ? tractor.TractorMakeId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</tractorMakeId>
                         //<tractorModelId>" + AJAXTools.EncodeForXML(tractor.TractorModelId.HasValue ? tractor.TractorModelId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</tractorModelId>

                response += @"         
                         <tractorMakeName>" + AJAXTools.EncodeForXML(tractor.TractorMakeName) + @"</tractorMakeName>
                         <tractorModelName>" + AJAXTools.EncodeForXML(tractor.TractorModelName) + @"</tractorModelName>
                         <tractorKindId>" + AJAXTools.EncodeForXML(tractor.TractorKindId.HasValue ? tractor.TractorKindId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</tractorKindId>
                         <tractorTypeId>" + AJAXTools.EncodeForXML(tractor.TractorTypeId.HasValue ? tractor.TractorTypeId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</tractorTypeId>
                         <power>" + AJAXTools.EncodeForXML(tractor.Power.HasValue ? tractor.Power.ToString() : "") + @"</power>
                         <firstRegistrationDate>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(tractor.FirstRegistrationDate)) + @"</firstRegistrationDate>
                         <lastAnnualTechnicalReviewDate>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(tractor.LastAnnualTechnicalReviewDate)) + @"</lastAnnualTechnicalReviewDate>
                         <mileage>" + AJAXTools.EncodeForXML(tractor.Mileage.HasValue ? tractor.Mileage.ToString() : "") + @"</mileage>
                         <residenceCityId>" + AJAXTools.EncodeForXML(tractor.Technics.ResidenceCityId != null ? tractor.Technics.ResidenceCityId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</residenceCityId>
                         <residencePostCode>" + AJAXTools.EncodeForXML(tractor.Technics.ResidencePostCode) + @"</residencePostCode>
                         <residenceRegionId>" + AJAXTools.EncodeForXML(tractor.Technics.ResidenceCityId != null ? tractor.Technics.ResidenceCity.RegionId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</residenceRegionId>
                         <residenceMunicipalityId>" + AJAXTools.EncodeForXML(tractor.Technics.ResidenceCityId != null ? tractor.Technics.ResidenceCity.MunicipalityId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</residenceMunicipalityId>
                         <residenceDistrictId>" + AJAXTools.EncodeForXML(tractor.Technics.ResidenceDistrictId != null ? tractor.Technics.ResidenceDistrict.DistrictId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</residenceDistrictId>
                         <residenceAddress>" + AJAXTools.EncodeForXML(tractor.Technics.ResidenceAddress) + @"</residenceAddress>
                         <currMilDepartment>" + AJAXTools.EncodeForXML(tractor.Technics.CurrTechMilRepStatus != null && tractor.Technics.CurrTechMilRepStatus.SourceMilDepartment != null ? tractor.Technics.CurrTechMilRepStatus.SourceMilDepartment.MilitaryDepartmentName : TechMilitaryReportStatusUtil.GetLabelWhenLackOfStatus()) + @"</currMilDepartment>
                         <normativeTechnicsId>" + AJAXTools.EncodeForXML(tractor.Technics.NormativeTechnicsId != null ? tractor.Technics.NormativeTechnics.NormativeTechnicsId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</normativeTechnicsId>
                         <normativeCode>" + AJAXTools.EncodeForXML(tractor.Technics.NormativeTechnicsId != null ? tractor.Technics.NormativeTechnics.NormativeCode : "") + @"</normativeCode>
                    </tractor>";

                //response += "<vmodel>" +
                //            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                //            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                //            "</vmodel>";

                //if (tractor.TractorMakeId.HasValue)
                //{
                //    foreach (TractorModel tractorModel in tractor.TractorMake.TractorModels)
                //    {
                //        response += "<vmodel>" +
                //                    "<id>" + tractorModel.TractorModelId.ToString() + "</id>" +
                //                    "<name>" + AJAXTools.EncodeForXML(tractorModel.TractorModelName) + "</name>" +
                //                    "</vmodel>";
                //    }
                //}

                if (tractor.Technics.ResidenceCityId != null)
                {
                    List<Municipality> municipalities = MunicipalityUtil.GetMunicipalities(tractor.Technics.ResidenceCity.RegionId, CurrentUser);
                    List<City> cities = CityUtil.GetCities(tractor.Technics.ResidenceCity.MunicipalityId, CurrentUser);
                    List<District> districts = tractor.Technics.ResidenceCity.Districts;

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
                GetUIItemAccessLevel("RES_TECHNICS_TRACTORS") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            int? technicsId = null;
            if (!String.IsNullOrEmpty(Request.Params["TechnicsId"]))
            {
                technicsId = int.Parse(Request.Params["TechnicsId"]);
            }

            int? tractorId = null;
            if (!String.IsNullOrEmpty(Request.Params["TractorId"]))
            {
                tractorId = int.Parse(Request.Params["TractorId"]);
            }

            string regNumber = Request.Params["RegNumber"];
            string inventoryNumber = Request.Params["InventoryNumber"];
            
            int? technicsCategoryId = null;
            if (!String.IsNullOrEmpty(Request.Params["TechnicsCategoryId"]) &&
                Request.Params["TechnicsCategoryId"] != ListItems.GetOptionChooseOne().Value)
            {
                technicsCategoryId = int.Parse(Request.Params["TechnicsCategoryId"]);
            }

            //int? tractorMakeId = null;
            //if (!String.IsNullOrEmpty(Request.Params["TractorMakeId"]) &&
            //    Request.Params["TractorMakeId"] != ListItems.GetOptionChooseOne().Value)
            //{
            //    tractorMakeId = int.Parse(Request.Params["TractorMakeId"]);
            //}

            //int? tractorModelId = null;
            //if (!String.IsNullOrEmpty(Request.Params["TractorModelId"]) &&
            //    Request.Params["TractorModelId"] != ListItems.GetOptionChooseOne().Value)
            //{
            //    tractorModelId = int.Parse(Request.Params["TractorModelId"]);
            //}

            string tractorMakeName = Request.Params["TractorMakeName"];
            string tractorModelName = Request.Params["TractorModelName"];

            int? tractorKindId = null;
            if (!String.IsNullOrEmpty(Request.Params["TractorKindId"]) &&
                Request.Params["TractorKindId"] != ListItems.GetOptionChooseOne().Value)
            {
                tractorKindId = int.Parse(Request.Params["TractorKindId"]);
            }

            int? tractorTypeId = null;
            if (!String.IsNullOrEmpty(Request.Params["TractorTypeId"]) &&
                Request.Params["TractorTypeId"] != ListItems.GetOptionChooseOne().Value)
            {
                tractorTypeId = int.Parse(Request.Params["TractorTypeId"]);
            }

            decimal? power = null;
            if (!String.IsNullOrEmpty(Request.Params["Power"]))
            {
                power = decimal.Parse(Request.Params["Power"]);
            }

            DateTime? firstRegistrationDate = null;
            if (!String.IsNullOrEmpty(Request.Params["FirstRegistrationDate"]))
            {
                firstRegistrationDate = CommonFunctions.ParseDate(Request.Params["FirstRegistrationDate"]);
            }

            DateTime? lastAnnualTechnicalReviewDate = null;
            if (!String.IsNullOrEmpty(Request.Params["LastAnnualTechnicalReviewDate"]))
            {
                lastAnnualTechnicalReviewDate = CommonFunctions.ParseDate(Request.Params["LastAnnualTechnicalReviewDate"]);
            }

            decimal? mileage = null;
            if (!String.IsNullOrEmpty(Request.Params["Mileage"]))
            {
                mileage = decimal.Parse(Request.Params["Mileage"]);
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

            Tractor tractor = new Tractor(CurrentUser);

            tractor.TractorId = tractorId.HasValue ? tractorId.Value : 0;
            tractor.TechnicsId = technicsId.HasValue ? technicsId.Value : 0;
            tractor.RegNumber = regNumber;
            tractor.InventoryNumber = inventoryNumber;

            //tractor.TractorMakeId = tractorMakeId;
            //tractor.TractorModelId = tractorModelId;

            tractor.TractorMakeName = tractorMakeName;
            tractor.TractorModelName = tractorModelName;
            
            tractor.TractorKindId = tractorKindId;
            tractor.TractorTypeId = tractorTypeId;
            tractor.Power = power;
            tractor.FirstRegistrationDate = firstRegistrationDate;
            tractor.LastAnnualTechnicalReviewDate = lastAnnualTechnicalReviewDate;
            tractor.Mileage = mileage;

            tractor.Technics = new Technics(CurrentUser);
            tractor.Technics.TechnicsId = tractor.TechnicsId;
            tractor.Technics.TechnicsType = TechnicsTypeUtil.GetTechnicsType("TRACTORS", CurrentUser);
            tractor.Technics.TechnicsCategoryId = technicsCategoryId.HasValue ? technicsCategoryId.Value : (int?)null;
            tractor.Technics.ItemsCount = 1;
            tractor.Technics.ResidencePostCode = residencePostCode;
            tractor.Technics.ResidenceCityId = residenceCityId;
            tractor.Technics.ResidenceDistrictId = residenceDistrictId;
            tractor.Technics.ResidenceAddress = residenceAddress;
            tractor.Technics.NormativeTechnicsId = normativeTechnicsId;


            string stat = "";
            string response = "";

            try
            {
                //Track the changes into the Audit Trail 
                Change change = new Change(CurrentUser, "RES_Technics_TRACTORS");

                TractorUtil.SaveTractor(tractor, CurrentUser, change);

                //Write into the Audit Trail
                change.WriteLog();

                stat = AJAXTools.OK;
                response = @"<response>OK</response>
                             <technicsId>" + AJAXTools.EncodeForXML(tractor.TechnicsId.ToString()) + @"</technicsId>
                             <tractorId>" + AJAXTools.EncodeForXML(tractor.TractorId.ToString()) + @"</tractorId>";
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
                GetUIItemAccessLevel("RES_TECHNICS_TRACTORS") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string regNumber = Request.Params["RegNumber"];

            string stat = "";
            string response = "";

            try
            {
                int technicsId = 0;

                Tractor tractor = TractorUtil.GetTractorByRegNumber(regNumber, CurrentUser);

                if (tractor != null)
                {
                    technicsId = tractor.TechnicsId;
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

        //Get the tractor models for a particular tractor make (ajax call)
        //private void JSRepopulateTractorModels()
        //{
        //    if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
        //        GetUIItemAccessLevel("RES_TECHNICS_TRACTORS") == UIAccessLevel.Hidden)
        //        RedirectAjaxAccessDenied();

        //    string stat = "";
        //    string response = "";

        //    try
        //    {
        //        int tractorMakeId = 0;

        //        if (!String.IsNullOrEmpty(Request.Form["TractorMakeId"]))
        //            tractorMakeId = int.Parse(Request.Form["TractorMakeId"]);

        //        response = "<tractorModels>";

        //        response += "<m>" +
        //                     "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
        //                     "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
        //                     "</m>";

        //        List<TractorModel> models = TractorModelUtil.GetAllTractorModels(tractorMakeId, CurrentUser);

        //        foreach (TractorModel model in models)
        //        {
        //            response += "<m>" +
        //                        "<id>" + model.TractorModelId.ToString() + "</id>" +
        //                        "<name>" + AJAXTools.EncodeForXML(model.TractorModelName) + "</name>" +
        //                        "</m>";
        //        }

        //        response += "</tractorModels>";

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

        //Refresh the list TractorKind (ajax call)
        private void JSRefreshTractorKindList()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_TRACTORS") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<tractorKind>";

                response += "<k>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</k>";

                List<GTableItem> tractorKinds = GTableItemUtil.GetAllGTableItemsByTableName("TractorKind", ModuleKey, 1, 0, 0, CurrentUser);

                foreach (GTableItem tractorKind in tractorKinds)
                {
                    response += "<k>" +
                                "<id>" + tractorKind.TableKey.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(tractorKind.TableValue.ToString()) + "</name>" +
                                "</k>";
                }

                response += "</tractorKind>";

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

        //Refresh the list TractorType (ajax call)
        private void JSRefreshTractorTypeList()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_TRACTORS") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<tractorType>";

                response += "<r>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</r>";

                List<GTableItem> tractorTypes = GTableItemUtil.GetAllGTableItemsByTableName("TractorType", ModuleKey, 1, 0, 0, CurrentUser);

                foreach (GTableItem tractorType in tractorTypes)
                {
                    response += "<r>" +
                                "<id>" + tractorType.TableKey.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(tractorType.TableValue.ToString()) + "</name>" +
                                "</r>";
                }

                response += "</tractorType>";

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
                GetUIItemAccessLevel("RES_TECHNICS_TRACTORS") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            int tractorId = int.Parse(Request.Params["TractorId"]);
            string newRegNumber = Request.Params["NewRegNumber"];
            
            string stat = "";
            string response = "";

            try
            {
                Tractor existingTractor = TractorUtil.GetTractorByRegNumber(newRegNumber, CurrentUser);

                if (existingTractor == null)
                {
                    //Track the changes into the Audit Trail 
                    Change change = new Change(CurrentUser, "RES_Technics_TRACTORS");

                    TractorUtil.ChangeRegNumber(tractorId, newRegNumber, CurrentUser, change);

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
                GetUIItemAccessLevel("RES_TECHNICS_TRACTORS") == UIAccessLevel.Hidden)
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

            List<TractorRegNumber> tractorRegNumbers = new List<TractorRegNumber>();
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

            int tractorId = 0;
            int.TryParse((Request.Params["TractorId"]).ToString(), out tractorId);

            allRows = TractorUtil.GetAllTractorRegNumbersCount(tractorId, currentUser);
            maxPage = allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            tractorRegNumbers = TractorUtil.GetAllTractorRegNumbers(tractorId, orderBy, pageIndex, pageLength, currentUser);

            // No data found
            if (tractorRegNumbers.Count == 0)
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

                        <table id='tblTractorRegNumberHistory' class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
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
            foreach (TractorRegNumber tractorRegNumber in tractorRegNumbers)
            {

                string cellStyle = "vertical-align: top;";

                html += @"<tr  class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"' >
                                 <td style='" + cellStyle + @"'>" + ((pageIndex - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + tractorRegNumber.RegNumber + @"</td>
                              </tr>";

                counter++;
            }
            html += "</table>";

            html += "<input type='hidden' id='hdnTractorRegNumberHistoryCounter' value='" + counter + "' />";


            html += @"<div style='height: 10px;'>
                </div>

                 <div style='text-align: center'>
                   <span id='lblTractorRegNumberHistoryMessage'>" + htmlNoResults + @"</span><br/>
                </div>
";

            html += @"  </div>                        
                        <div id='btnCloseTractorRegNumberHistoryTable' runat='server' class='Button' onclick=""HideRegNumberHistoryLightBox();""><i></i><div style='width:70px; padding-left:5px;'>Затвори</div><b></b></div>
                      </center>";



            return html;
        }
    }


    public static class AddEditTechnics_TRACTORS_PageUtil
    {
        public static string GetBasicInfoTabContent(AddEditTechnics page)
        {
            List<IDropDownItem> ddiTechnicsCategories = new List<IDropDownItem>();
            List<TechnicsCategory> technicsCategories = TechnicsCategoryUtil.GetAllTechnicsCategories(page.CurrentUser);

            foreach (TechnicsCategory technicsCategory in technicsCategories)
            {
                ddiTechnicsCategories.Add(technicsCategory);
            }

            string techncisCategoriesHTML = ListItems.GetDropDownHtml(ddiTechnicsCategories, null, "ddTechnicsCategory", true, null, "", "style='width: 170px;'", true);

            //List<IDropDownItem> ddiTractorMakes = new List<IDropDownItem>();
            //List<TractorMake> tractorMakes = TractorMakeUtil.GetAllTractorMakes(page.CurrentUser);

            //foreach (TractorMake tractorMake in tractorMakes)
            //{
            //    ddiTractorMakes.Add(tractorMake);
            //}            

            //string tractorMakesHTML = ListItems.GetDropDownHtml(ddiTractorMakes, null, "ddTractorMake", true, null, "RepopulateTractorModels(this.value);", "style='width: 300px;'", true);

            //List<IDropDownItem> ddiTractorModels = new List<IDropDownItem>();
            //DropDownItem blankItem = new DropDownItem();
            //blankItem.Txt = ListItems.GetOptionChooseOne().Text;
            //blankItem.Val = ListItems.GetOptionChooseOne().Value;
            //ddiTractorModels.Add(blankItem);

            //string tractorModelsHTML = ListItems.GetDropDownHtml(ddiTractorModels, null, "ddTractorModel", false, null, "", "style='width: 300px;'", true);


            List<GTableItem> tractorKinds = GTableItemUtil.GetAllGTableItemsByTableName("TractorKind", page.ModuleKey, 1, 0, 0, page.CurrentUser);
            List<IDropDownItem> ddiTractorKinds = new List<IDropDownItem>();
            foreach (GTableItem tractorKind in tractorKinds)
            {
                ddiTractorKinds.Add(tractorKind);
            }

            string tractorKindsHTML = ListItems.GetDropDownHtml(ddiTractorKinds, null, "ddTractorKind", true, null, "", "style='width: 300px;'", true);
            string editTractorKindsHTML = @"<img id=""imgMaintTractorKind"" alt=""Редактиране на списъка"" title=""Редактиране на списъка"" style=""cursor: pointer;"" src=""../Images/list_edit.png"" onclick=""ShowGTable('TractorKind', 1, 1, RefreshTractorKindList);"" />";

            List<GTableItem> tractorTypes = GTableItemUtil.GetAllGTableItemsByTableName("TractorType", page.ModuleKey, 1, 0, 0, page.CurrentUser);
            List<IDropDownItem> ddiTractorType = new List<IDropDownItem>();
            foreach (GTableItem tractorType in tractorTypes)
            {
                ddiTractorType.Add(tractorType);
            }

            string tractorTypesHTML = ListItems.GetDropDownHtml(ddiTractorType, null, "ddTractorType", true, null, "", "style='width: 300px;'", true);
            string editTractorTypeHTML = @"<img id=""imgMaintTractorType"" alt=""Редактиране на списъка"" title=""Редактиране на списъка"" style=""cursor: pointer;"" src=""../Images/list_edit.png"" onclick=""ShowGTable('TractorType', 1, 1, RefreshTractorTypeList);"" />";


            string html = @"
<div style=""height: 10px;""></div>

<fieldset style=""width: 830px; padding: 0px;"">
   <table class=""InputRegion"" style=""width: 830px; padding: 10px; padding-top: 0px; margin-top: 0px; padding-left: 0px;"">
      <tr style=""height: 3px;"">
      </tr>
      <tr>
         
         <td style=""text-align: right; width: 140px;"">
            <span id=""lblInventoryNuber"" class=""InputLabel"">Инвентарен номер:</span>
         </td>
         <td style=""text-align: left; width: 110px;"">
            <input type=""text"" id=""txtInventoryNumber"" onblur=""InventoryNumberBlur();"" class=""InputField"" style=""width: 100px;"" maxlength=""50"" />
         </td>
         <td style=""text-align: right; width: 90px;"">
            <span id=""lblTechnicsCategory"" class=""InputLabel"">Категория:</span>
         </td>
         <td style=""text-align: left; width: 170px;"">
            " + techncisCategoriesHTML + @"
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
         <td style=""text-align: right;"">
            <span id=""lblTractorMake"" class=""InputLabel"">Марка:</span>
         </td>
         <td style=""text-align: left;"">
            <input type=""text"" id=""txtTractorMakeName"" class=""InputField"" style=""width: 294px;"" maxlength=""300"" />
         </td>
         <td style=""text-align: right;"">
            <span id=""lblTractorModel"" class=""InputLabel"">Модел:</span>
         </td>
         <td style=""text-align: left;"">
            <input type=""text"" id=""txtTractorModelName"" class=""InputField"" style=""width: 294px;"" maxlength=""300"" />
         </td>
      </tr>
      <tr>
         <td style=""text-align: right;"">
            <span id=""lblTractorKind"" class=""InputLabel"">Вид:</span>
         </td>
         <td style=""text-align: left;"">
            " + tractorKindsHTML + editTractorKindsHTML + @"
         </td>
         <td style=""text-align: right;"">
            <span id=""lblTractorType"" class=""InputLabel"">Тип:</span>
         </td>
         <td style=""text-align: left;"">
            " + tractorTypesHTML + editTractorTypeHTML + @"
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
         <td style=""text-align: right; width: 160px;"">
            <span id=""lblPower"" class=""InputLabel"">Мощност:</span>
         </td>
         <td style=""text-align: left; width: 170px;"">
            <input type=""text"" id=""txtPower"" class=""InputField"" style=""width: 100px;"" maxlength=""50"" />
         </td>
         <td style=""text-align: right; width: 230px;"">
            <span id=""lblFirstRegistrationDate"" class=""InputLabel"">Дата на първата регистрация:</span>
         </td>
         <td style=""text-align: left; width: 250px;"">
            <span id=""txtFirstRegistrationDateCont""><input type=""text"" id=""txtFirstRegistrationDate"" class='" + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" /></span>
         </td>
      </tr>
      <tr>
         <td style=""text-align: right;"" colspan=""3"">
            <span id=""lblLastAnnualTechnicalReviewDate"" class=""InputLabel"">Дата на последен ГТП:</span>
         </td>
         <td style=""text-align: left;"">
            <span id=""txtLastAnnualTechnicalReviewDateCont""><input type=""text"" id=""txtLastAnnualTechnicalReviewDate"" class='" + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" /></span>
         </td>
      </tr>
      <tr>
         <td style=""text-align: right;"" colspan=""3"">
            <span id=""lblMileage"" class=""InputLabel"">Изминати километри:</span>
         </td>
         <td style=""text-align: left;"">
            <input type=""text"" id=""txtMileage"" class=""InputField"" style=""width: 100px;"" maxlength=""50"" />
         </td>
      </tr>
   </table>
</fieldset>

" + AddEditTechnics_BasicInfo_PageUtil.GetBasicInfoResidenceContent(page) + @"

<div style=""height: 10px;""></div>

<input type=""hidden"" id=""hdnTractorId"" />

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
                                 page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_ADD") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_ADD_BASICINFO") == UIAccessLevel.Disabled;

                basicInfoDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_ADD") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_ADD_BASICINFO") == UIAccessLevel.Disabled;

               
                UIAccessLevel l;

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_ADD_BASICINFO_INVENTORYNUMBER");

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

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_ADD_BASICINFO_TECHNICSCATEGORY");

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

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_ADD_BASICINFO_TRACTORMAKE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblTractorMake");
                    disabledClientControls.Add("txtTractorMakeName");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblTractorMake");
                    hiddenClientControls.Add("txtTractorMakeName");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_ADD_BASICINFO_TRACTORMODEL");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblTractorModel");
                    disabledClientControls.Add("txtTractorModelName");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblTractorModel");
                    hiddenClientControls.Add("txtTractorModelName");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_ADD_BASICINFO_TRACTORKIND");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblTractorKind");
                    disabledClientControls.Add("ddTractorKind");
                    hiddenClientControls.Add("imgMaintTractorKind");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblTractorKind");
                    hiddenClientControls.Add("ddTractorKind");
                    hiddenClientControls.Add("imgMaintTractorKind");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_ADD_BASICINFO_TRACTORTYPE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblTractorType");
                    disabledClientControls.Add("ddTractorType");
                    hiddenClientControls.Add("imgMaintTractorType");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblTractorType");
                    hiddenClientControls.Add("ddTractorType");
                    hiddenClientControls.Add("imgMaintTractorType");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_ADD_BASICINFO_POWER");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblPower");
                    disabledClientControls.Add("txtPower");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblPower");
                    hiddenClientControls.Add("txtPower");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_ADD_BASICINFO_FIRSTREGDATE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblFirstRegistrationDate");
                    disabledClientControls.Add("txtFirstRegistrationDate");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblFirstRegistrationDate");
                    hiddenClientControls.Add("txtFirstRegistrationDateCont");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_ADD_BASICINFO_LASTANNTECHREVIEWDATE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblLastAnnualTechnicalReviewDate");
                    disabledClientControls.Add("txtLastAnnualTechnicalReviewDate");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblLastAnnualTechnicalReviewDate");
                    hiddenClientControls.Add("txtLastAnnualTechnicalReviewDateCont");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_ADD_BASICINFO_MILEAGE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblMileage");
                    disabledClientControls.Add("txtMileage");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblMileage");
                    hiddenClientControls.Add("txtMileage");
                }

                AddEditTechnics_BasicInfo_PageUtil.GetBasicInfoResidenceUIItems(page, true,
                    ref disabledClientControls, ref hiddenClientControls);
            }
            else // edit mode of page
            {
                screenDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_EDIT") == UIAccessLevel.Disabled || isPreview;

                basicInfoDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_EDIT") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_EDIT_BASICINFO") == UIAccessLevel.Disabled;

               
                UIAccessLevel l;
                
                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_EDIT_BASICINFO_INVENTORYNUMBER");

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

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_EDIT_BASICINFO_TECHNICSCATEGORY");

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

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_EDIT_BASICINFO_TRACTORMAKE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblTractorMake");
                    disabledClientControls.Add("txtTractorMakeName");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblTractorMake");
                    hiddenClientControls.Add("txtTractorMakeName");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_EDIT_BASICINFO_TRACTORMODEL");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblTractorModel");
                    disabledClientControls.Add("txtTractorModelName");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblTractorModel");
                    hiddenClientControls.Add("txtTractorModelName");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_EDIT_BASICINFO_TRACTORKIND");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblTractorKind");
                    disabledClientControls.Add("ddTractorKind");
                    hiddenClientControls.Add("imgMaintTractorKind");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblTractorKind");
                    hiddenClientControls.Add("ddTractorKind");
                    hiddenClientControls.Add("imgMaintTractorKind");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_EDIT_BASICINFO_TRACTORTYPE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblTractorType");
                    disabledClientControls.Add("ddTractorType");
                    hiddenClientControls.Add("imgMaintTractorType");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblTractorType");
                    hiddenClientControls.Add("ddTractorType");
                    hiddenClientControls.Add("imgMaintTractorType");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_EDIT_BASICINFO_POWER");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblPower");
                    disabledClientControls.Add("txtPower");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblPower");
                    hiddenClientControls.Add("txtPower");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_EDIT_BASICINFO_FIRSTREGDATE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblFirstRegistrationDate");
                    disabledClientControls.Add("txtFirstRegistrationDate");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblFirstRegistrationDate");
                    hiddenClientControls.Add("txtFirstRegistrationDateCont");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_EDIT_BASICINFO_LASTANNTECHREVIEWDATE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblLastAnnualTechnicalReviewDate");
                    disabledClientControls.Add("txtLastAnnualTechnicalReviewDate");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblLastAnnualTechnicalReviewDate");
                    hiddenClientControls.Add("txtLastAnnualTechnicalReviewDateCont");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_EDIT_BASICINFO_MILEAGE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblMileage");
                    disabledClientControls.Add("txtMileage");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblMileage");
                    hiddenClientControls.Add("txtMileage");
                }

                AddEditTechnics_BasicInfo_PageUtil.GetBasicInfoResidenceUIItems(page, false,
                    ref disabledClientControls, ref hiddenClientControls);
            }

            if (page.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled ||
                page.GetUIItemAccessLevel("RES_LISTMAINT_TRACTORKIND") != UIAccessLevel.Enabled)
            {
                hiddenClientControls.Add("imgMaintTractorKind");
            }

            if (page.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled ||
                page.GetUIItemAccessLevel("RES_LISTMAINT_TRACTORTYPE") != UIAccessLevel.Enabled)
            {
                hiddenClientControls.Add("imgMaintTractorType");
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
                                            <input type=""text"" id=""txtNewRegNumber"" onblur=""NewRegNumberBlur();"" class=""RequiredInputField"" style=""width: 90px;"" maxlength=""20"" UnsavedCheckSkipMe=""true"" />
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
                                 page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_ADD") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_ADD_BASICINFO") == UIAccessLevel.Disabled;

                screenHidden = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_ADD") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_ADD_BASICINFO") == UIAccessLevel.Hidden;

                basicInfoDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_ADD") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_ADD_BASICINFO") == UIAccessLevel.Disabled;

                basicInfoHidden = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_ADD") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_ADD_BASICINFO") == UIAccessLevel.Hidden;


                UIAccessLevel l;

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_ADD_BASICINFO_REGNUMBER");

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
                                 page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_EDIT") == UIAccessLevel.Disabled || isPreview;


                screenHidden = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_EDIT") == UIAccessLevel.Hidden;

                basicInfoDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_EDIT") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_EDIT_BASICINFO") == UIAccessLevel.Disabled;

                basicInfoHidden = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_EDIT") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_EDIT_BASICINFO") == UIAccessLevel.Hidden;


                UIAccessLevel l;

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRACTORS_EDIT_BASICINFO_REGNUMBER");

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
    }
}
