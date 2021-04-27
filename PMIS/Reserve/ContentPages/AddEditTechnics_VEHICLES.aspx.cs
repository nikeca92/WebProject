using System;
using System.Collections.Generic;
using System.Text;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class AddEditTechnics_VEHICLES : RESPage
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
            //if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRepopulateVehicleModels")
            //{
            //    JSRepopulateVehicleModels();
            //    return;
            //}

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRefreshVehicleKindList")
            {
                JSRefreshVehicleKindList();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRefreshVehicleRoadabilityList")
            {
                JSRefreshVehicleRoadabilityList();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRefreshVehicleEngineTypeList")
            {
                JSRefreshVehicleEngineTypeList();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRefreshVehicleBodyTypeList")
            {
                JSRefreshVehicleBodyTypeList();
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
                GetUIItemAccessLevel("RES_TECHNICS_VEHICLES") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            int technicsId = 0;
            if (!String.IsNullOrEmpty(Request.Form["TechnicsId"]))
                technicsId = int.Parse(Request.Form["TechnicsId"]);

            string stat = "";
            string response = "";

            try
            {
                Vehicle vehicle = VehicleUtil.GetVehicleByTechnicsId(technicsId, CurrentUser);
                TechnicsMilRepStatus currentMilRepStatus = TechnicsMilRepStatusUtil.GetTechnicsMilRepCurrentStatusByTechnicsId(vehicle.TechnicsId, CurrentUser);
                string currMilRepStatusName = (currentMilRepStatus != null ? currentMilRepStatus.TechMilitaryReportStatus.TechMilitaryReportStatusName : TechMilitaryReportStatusUtil.GetLabelWhenLackOfStatus());
            

                stat = AJAXTools.OK;

                response = @"
                    <vehicle>
                         <technicsId>" + AJAXTools.EncodeForXML(vehicle.TechnicsId.ToString()) + @"</technicsId>
                         <vehicleId>" + AJAXTools.EncodeForXML(vehicle.VehicleId.ToString()) + @"</vehicleId>
                         <regNumber>" + AJAXTools.EncodeForXML(vehicle.RegNumber) + @"</regNumber>
                         <inventoryNumber>" + AJAXTools.EncodeForXML(vehicle.InventoryNumber) + @"</inventoryNumber>
                         <technicsCategoryId>" + AJAXTools.EncodeForXML(vehicle.Technics.TechnicsCategoryId.HasValue ? vehicle.Technics.TechnicsCategoryId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</technicsCategoryId>
                         <lastModified>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDateTime(vehicle.Technics.LastModifiedDate)) + @"</lastModified>
                         <resMilRepStatus>" + AJAXTools.EncodeForXML(currMilRepStatusName) + @"</resMilRepStatus>";
                         //<vehicleMakeId>" + AJAXTools.EncodeForXML(vehicle.VehicleMakeId.HasValue ? vehicle.VehicleMakeId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</vehicleMakeId>
                         //<vehicleModelId>" + AJAXTools.EncodeForXML(vehicle.VehicleModelId.HasValue ? vehicle.VehicleModelId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</vehicleModelId>

                response += @"
                         <vehicleMakeName>" + AJAXTools.EncodeForXML(vehicle.VehicleMakeName) + @"</vehicleMakeName>
                         <vehicleModelName>" + AJAXTools.EncodeForXML(vehicle.VehicleModelName) + @"</vehicleModelName>
                         <vehicleKindId>" + AJAXTools.EncodeForXML(vehicle.VehicleKindId.HasValue ? vehicle.VehicleKindId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</vehicleKindId>
                         <vehicleRoadabilityId>" + AJAXTools.EncodeForXML(vehicle.VehicleRoadabilityId.HasValue ? vehicle.VehicleRoadabilityId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</vehicleRoadabilityId>
                         <carryingCapacity>" + AJAXTools.EncodeForXML(vehicle.CarryingCapacity.HasValue ? vehicle.CarryingCapacity.ToString() : "") + @"</carryingCapacity>
                         <loadingCapacity>" + AJAXTools.EncodeForXML(vehicle.LoadingCapacity.HasValue ? vehicle.LoadingCapacity.ToString() : "") + @"</loadingCapacity>
                         <firstRegistrationDate>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(vehicle.FirstRegistrationDate)) + @"</firstRegistrationDate>
                         <lastAnnualTechnicalReviewDate>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(vehicle.LastAnnualTechnicalReviewDate)) + @"</lastAnnualTechnicalReviewDate>
                         <seats>" + AJAXTools.EncodeForXML(vehicle.Seats) + @"</seats>
                         <mileage>" + AJAXTools.EncodeForXML(vehicle.Mileage.HasValue ? vehicle.Mileage.ToString() : "") + @"</mileage>
                         <vehicleEngineTypeId>" + AJAXTools.EncodeForXML(vehicle.VehicleEngineTypeId.HasValue ? vehicle.VehicleEngineTypeId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</vehicleEngineTypeId>
                         <vehicleBodyTypeId>" + AJAXTools.EncodeForXML(vehicle.VehicleBodyTypeId.HasValue ? vehicle.VehicleBodyTypeId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</vehicleBodyTypeId>
                         <residenceCityId>" + AJAXTools.EncodeForXML(vehicle.Technics.ResidenceCityId != null ? vehicle.Technics.ResidenceCityId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</residenceCityId>
                         <residencePostCode>" + AJAXTools.EncodeForXML(vehicle.Technics.ResidencePostCode) + @"</residencePostCode>
                         <residenceRegionId>" + AJAXTools.EncodeForXML(vehicle.Technics.ResidenceCityId != null ? vehicle.Technics.ResidenceCity.RegionId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</residenceRegionId>
                         <residenceMunicipalityId>" + AJAXTools.EncodeForXML(vehicle.Technics.ResidenceCityId != null ? vehicle.Technics.ResidenceCity.MunicipalityId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</residenceMunicipalityId>
                         <residenceDistrictId>" + AJAXTools.EncodeForXML(vehicle.Technics.ResidenceDistrictId != null ? vehicle.Technics.ResidenceDistrict.DistrictId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</residenceDistrictId>
                         <residenceAddress>" + AJAXTools.EncodeForXML(vehicle.Technics.ResidenceAddress) + @"</residenceAddress>
                         <currMilDepartment>" + AJAXTools.EncodeForXML(vehicle.Technics.CurrTechMilRepStatus != null && vehicle.Technics.CurrTechMilRepStatus.SourceMilDepartment != null ? vehicle.Technics.CurrTechMilRepStatus.SourceMilDepartment.MilitaryDepartmentName : TechMilitaryReportStatusUtil.GetLabelWhenLackOfStatus()) + @"</currMilDepartment>
                         <normativeTechnicsId>" + AJAXTools.EncodeForXML(vehicle.Technics.NormativeTechnicsId != null ? vehicle.Technics.NormativeTechnics.NormativeTechnicsId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</normativeTechnicsId>
                         <normativeCode>" + AJAXTools.EncodeForXML(vehicle.Technics.NormativeTechnicsId != null ? vehicle.Technics.NormativeTechnics.NormativeCode : "") + @"</normativeCode>
                    </vehicle>";

                //Pre-populate the Normative Technics drop-down
                response += "<n>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</n>";
                
                int? vehicleKindId = null;
                if (vehicle.TechnicsId > 0)
                    vehicleKindId = vehicle.VehicleKindId;

                List<NormativeTechnics> normativeTechnics = NormativeTechnicsUtil.GetNormativeTechnicsByVehicleKind(CurrentUser, "VEHICLES", vehicleKindId);

                foreach (NormativeTechnics normativeTechnicsRec in normativeTechnics)
                {
                    response += "<n>" +
                                "<id>" + normativeTechnicsRec.Value() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(normativeTechnicsRec.Text()) + "</name>" +
                                "</n>";
                }

                //response += "<vmodel>" +
                //            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                //            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                //            "</vmodel>";

                //if (vehicle.VehicleMakeId.HasValue)
                //{
                //    foreach (VehicleModel vehicleModel in vehicle.VehicleMake.VehicleModels)
                //    {
                //        response += "<vmodel>" +
                //                    "<id>" + vehicleModel.VehicleModelId.ToString() + "</id>" +
                //                    "<name>" + AJAXTools.EncodeForXML(vehicleModel.VehicleModelName) + "</name>" +
                //                    "</vmodel>";
                //    }
                //}

                if (vehicle.Technics.ResidenceCityId != null)
                {
                    List<Municipality> municipalities = MunicipalityUtil.GetMunicipalities(vehicle.Technics.ResidenceCity.RegionId, CurrentUser);
                    List<City> cities = CityUtil.GetCities(vehicle.Technics.ResidenceCity.MunicipalityId, CurrentUser);
                    List<District> districts = vehicle.Technics.ResidenceCity.Districts;

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
                GetUIItemAccessLevel("RES_TECHNICS_VEHICLES") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            int? technicsId = null;
            if (!String.IsNullOrEmpty(Request.Params["TechnicsId"]))
            {
                technicsId = int.Parse(Request.Params["TechnicsId"]);
            }

            int? vehicleId = null;
            if (!String.IsNullOrEmpty(Request.Params["VehicleId"]))
            {
                vehicleId = int.Parse(Request.Params["VehicleId"]);
            }

            string regNumber = Request.Params["RegNumber"];
            string inventoryNumber = Request.Params["InventoryNumber"];
            
            int? technicsCategoryId = null;
            if (!String.IsNullOrEmpty(Request.Params["TechnicsCategoryId"]) &&
                Request.Params["TechnicsCategoryId"] != ListItems.GetOptionChooseOne().Value)
            {
                technicsCategoryId = int.Parse(Request.Params["TechnicsCategoryId"]);
            }

            //int? vehicleMakeId = null;
            //if (!String.IsNullOrEmpty(Request.Params["VehicleMakeId"]) &&
            //    Request.Params["VehicleMakeId"] != ListItems.GetOptionChooseOne().Value)
            //{
            //    vehicleMakeId = int.Parse(Request.Params["VehicleMakeId"]);
            //}

            //int? vehicleModelId = null;
            //if (!String.IsNullOrEmpty(Request.Params["VehicleModelId"]) &&
            //    Request.Params["VehicleModelId"] != ListItems.GetOptionChooseOne().Value)
            //{
            //    vehicleModelId = int.Parse(Request.Params["VehicleModelId"]);
            //}

            string vehicleMakeName = Request.Params["VehicleMakeName"];
            string vehicleModelName = Request.Params["VehicleModelName"];

            int? vehicleKindId = null;
            if (!String.IsNullOrEmpty(Request.Params["VehicleKindId"]) &&
                Request.Params["VehicleKindId"] != ListItems.GetOptionChooseOne().Value)
            {
                vehicleKindId = int.Parse(Request.Params["VehicleKindId"]);
            }

            int? vehicleRoadabilityId = null;
            if (!String.IsNullOrEmpty(Request.Params["VehicleRoadabilityId"]) &&
                Request.Params["VehicleRoadabilityId"] != ListItems.GetOptionChooseOne().Value)
            {
                vehicleRoadabilityId = int.Parse(Request.Params["VehicleRoadabilityId"]);
            }

            decimal? carryingCapacity = null;
            if (!String.IsNullOrEmpty(Request.Params["CarryingCapacity"]))
            {
                carryingCapacity = decimal.Parse(Request.Params["CarryingCapacity"]);
            }

            decimal? loadingCapacity = null;
            if (!String.IsNullOrEmpty(Request.Params["LoadingCapacity"]))
            {
                loadingCapacity = decimal.Parse(Request.Params["LoadingCapacity"]);
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

            string seats = Request.Params["Seats"];

            decimal? mileage = null;
            if (!String.IsNullOrEmpty(Request.Params["Mileage"]))
            {
                mileage = decimal.Parse(Request.Params["Mileage"]);
            }

            int? vehicleEngineTypeId = null;
            if (!String.IsNullOrEmpty(Request.Params["VehicleEngineTypeId"]) &&
                Request.Params["VehicleEngineTypeId"] != ListItems.GetOptionChooseOne().Value)
            {
                vehicleEngineTypeId = int.Parse(Request.Params["VehicleEngineTypeId"]);
            }

            int? vehicleBodyTypeId = null;
            if (!String.IsNullOrEmpty(Request.Params["VehicleBodyTypeId"]) &&
                Request.Params["VehicleBodyTypeId"] != ListItems.GetOptionChooseOne().Value)
            {
                vehicleBodyTypeId = int.Parse(Request.Params["VehicleBodyTypeId"]);
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

            Vehicle vehicle = new Vehicle(CurrentUser);

            vehicle.VehicleId = vehicleId.HasValue ? vehicleId.Value : 0;
            vehicle.TechnicsId = technicsId.HasValue ? technicsId.Value : 0;
            vehicle.RegNumber = regNumber;
            vehicle.InventoryNumber = inventoryNumber;
            
            //vehicle.VehicleMakeId = vehicleMakeId;
            //vehicle.VehicleModelId = vehicleModelId;

            vehicle.VehicleMakeName = vehicleMakeName;
            vehicle.VehicleModelName = vehicleModelName;
            
            vehicle.VehicleKindId = vehicleKindId;
            vehicle.VehicleRoadabilityId = vehicleRoadabilityId;
            vehicle.CarryingCapacity = carryingCapacity;
            vehicle.LoadingCapacity = loadingCapacity;
            vehicle.FirstRegistrationDate = firstRegistrationDate;
            vehicle.LastAnnualTechnicalReviewDate = lastAnnualTechnicalReviewDate;
            vehicle.Seats = seats;
            vehicle.Mileage = mileage;
            vehicle.VehicleEngineTypeId = vehicleEngineTypeId;
            vehicle.VehicleBodyTypeId = vehicleBodyTypeId;

            vehicle.Technics = new Technics(CurrentUser);
            vehicle.Technics.TechnicsId = vehicle.TechnicsId;
            vehicle.Technics.TechnicsType = TechnicsTypeUtil.GetTechnicsType("VEHICLES", CurrentUser);
            vehicle.Technics.TechnicsCategoryId = technicsCategoryId.HasValue ? technicsCategoryId.Value : (int?)null;
            vehicle.Technics.ItemsCount = 1;
            vehicle.Technics.ResidencePostCode = residencePostCode;
            vehicle.Technics.ResidenceCityId = residenceCityId;
            vehicle.Technics.ResidenceDistrictId = residenceDistrictId;
            vehicle.Technics.ResidenceAddress = residenceAddress;
            vehicle.Technics.NormativeTechnicsId = normativeTechnicsId;

            string stat = "";
            string response = "";

            try
            {
                //Track the changes into the Audit Trail 
                Change change = new Change(CurrentUser, "RES_Technics_VEHICLES");

                VehicleUtil.SaveVehicle(vehicle, CurrentUser, change);

                //Write into the Audit Trail
                change.WriteLog();

                stat = AJAXTools.OK;
                response = @"<response>OK</response>
                             <technicsId>" + AJAXTools.EncodeForXML(vehicle.TechnicsId.ToString()) + @"</technicsId>
                             <vehicleId>" + AJAXTools.EncodeForXML(vehicle.VehicleId.ToString()) + @"</vehicleId>";
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
                GetUIItemAccessLevel("RES_TECHNICS_VEHICLES") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string regNumber = Request.Params["RegNumber"];

            string stat = "";
            string response = "";

            try
            {
                int technicsId = 0;

                Vehicle vehicle = VehicleUtil.GetVehicleByRegNumber(regNumber, CurrentUser);

                if (vehicle != null)
                {
                    technicsId = vehicle.TechnicsId;
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

        //Get the vehicle models for a particular vehicle make (ajax call)
        //private void JSRepopulateVehicleModels()
        //{
        //    if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
        //        GetUIItemAccessLevel("RES_TECHNICS_VEHICLES") == UIAccessLevel.Hidden)
        //        RedirectAjaxAccessDenied();

        //    string stat = "";
        //    string response = "";

        //    try
        //    {
        //        int vehicleMakeId = 0;

        //        if (!String.IsNullOrEmpty(Request.Form["VehicleMakeId"]))
        //            vehicleMakeId = int.Parse(Request.Form["VehicleMakeId"]);

        //        response = "<vehicleModels>";

        //        response += "<m>" +
        //                     "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
        //                     "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
        //                     "</m>";

        //        List<VehicleModel> models = VehicleModelUtil.GetAllVehicleModels(vehicleMakeId, CurrentUser);

        //        foreach (VehicleModel model in models)
        //        {
        //            response += "<m>" +
        //                        "<id>" + model.VehicleModelId.ToString() + "</id>" +
        //                        "<name>" + AJAXTools.EncodeForXML(model.VehicleModelName) + "</name>" +
        //                        "</m>";
        //        }

        //        response += "</vehicleModels>";

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

        //Refresh the list VehicleKind (ajax call)
        private void JSRefreshVehicleKindList()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_VEHICLES") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<vehicleKind>";

                response += "<k>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</k>";

                List<GTableItem> vehicleKinds = GTableItemUtil.GetAllGTableItemsByTableName("VehicleKind", ModuleKey, 1, 0, 0, CurrentUser);

                foreach (GTableItem vehicleKind in vehicleKinds)
                {
                    response += "<k>" +
                                "<id>" + vehicleKind.TableKey.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(vehicleKind.TableValue.ToString()) + "</name>" +
                                "</k>";
                }

                response += "</vehicleKind>";

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

        //Refresh the list VehicleRoadability (ajax call)
        private void JSRefreshVehicleRoadabilityList()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_VEHICLES") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<vehicleRoadability>";

                response += "<r>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</r>";

                List<GTableItem> vehicleRoadabilities = GTableItemUtil.GetAllGTableItemsByTableName("VehicleRoadability", ModuleKey, 1, 0, 0, CurrentUser);

                foreach (GTableItem vehicleRoadability in vehicleRoadabilities)
                {
                    response += "<r>" +
                                "<id>" + vehicleRoadability.TableKey.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(vehicleRoadability.TableValue.ToString()) + "</name>" +
                                "</r>";
                }

                response += "</vehicleRoadability>";

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

        //Refresh the list EngineType (ajax call)
        private void JSRefreshVehicleEngineTypeList()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_VEHICLES") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<vehicleEngineType>";

                response += "<et>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</et>";

                List<GTableItem> vehicleEngineTypes = GTableItemUtil.GetAllGTableItemsByTableName("VehicleEngineType", ModuleKey, 1, 0, 0, CurrentUser);

                foreach (GTableItem vehicleEngineType in vehicleEngineTypes)
                {
                    response += "<et>" +
                                "<id>" + vehicleEngineType.TableKey.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(vehicleEngineType.TableValue.ToString()) + "</name>" +
                                "</et>";
                }

                response += "</vehicleEngineType>";

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

        //Refresh the list BodyType (ajax call)
        private void JSRefreshVehicleBodyTypeList()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_VEHICLES") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<vehicleBodyType>";

                response += "<bt>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</bt>";

                List<GTableItem> vehicleBodyTypes = GTableItemUtil.GetAllGTableItemsByTableName("VehicleBodyType", ModuleKey, 1, 0, 0, CurrentUser);

                foreach (GTableItem vehicleBodyType in vehicleBodyTypes)
                {
                    response += "<bt>" +
                                "<id>" + vehicleBodyType.TableKey.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(vehicleBodyType.TableValue.ToString()) + "</name>" +
                                "</bt>";
                }

                response += "</vehicleBodyType>";

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
                GetUIItemAccessLevel("RES_TECHNICS_VEHICLES") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            int vehicleId = int.Parse(Request.Params["VehicleId"]);
            string newRegNumber = Request.Params["NewRegNumber"];
            
            string stat = "";
            string response = "";

            try
            {
                Vehicle existingVehicle = VehicleUtil.GetVehicleByRegNumber(newRegNumber, CurrentUser);

                if (existingVehicle == null)
                {
                    //Track the changes into the Audit Trail 
                    Change change = new Change(CurrentUser, "RES_Technics_VEHICLES");

                    VehicleUtil.ChangeRegNumber(vehicleId, newRegNumber, CurrentUser, change);

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
                GetUIItemAccessLevel("RES_TECHNICS_VEHICLES") == UIAccessLevel.Hidden)
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

            List<VehicleRegNumber> vehicleRegNumbers = new List<VehicleRegNumber>();
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

            int vehicleId = 0;
            int.TryParse((Request.Params["VehicleId"]).ToString(), out vehicleId);

            allRows = VehicleUtil.GetAllVehicleRegNumbersCount(vehicleId, currentUser);
            maxPage = allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            vehicleRegNumbers = VehicleUtil.GetAllVehicleRegNumbers(vehicleId, orderBy, pageIndex, pageLength, currentUser);

            // No data found
            if (vehicleRegNumbers.Count == 0)
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

                        <table id='tblVehicleRegNumberHistory' class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
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
            foreach (VehicleRegNumber vehicleRegNumber in vehicleRegNumbers)
            {

                string cellStyle = "vertical-align: top;";

                html += @"<tr  class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"' >
                                 <td style='" + cellStyle + @"'>" + ((pageIndex - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + vehicleRegNumber.RegNumber + @"</td>
                              </tr>";

                counter++;
            }
            html += "</table>";

            html += "<input type='hidden' id='hdnVehicleRegNumberHistoryCounter' value='" + counter + "' />";


            html += @"<div style='height: 10px;'>
                </div>

                 <div style='text-align: center'>
                   <span id='lblVehicleRegNumberHistoryMessage'>" + htmlNoResults + @"</span><br/>
                </div>
";

            html += @"  </div>                        
                        <div id='btnCloseVehicleRegNumberHistoryTable' runat='server' class='Button' onclick=""HideRegNumberHistoryLightBox();""><i></i><div style='width:70px; padding-left:5px;'>Затвори</div><b></b></div>
                      </center>";



            return html;
        }
    }


    public static class AddEditTechnics_VEHICLES_PageUtil
    {
        public static string GetGeneralPanelRegNumberContent()
        {
            string html = "";
            html = @"  <div id=""tdRegNumber"">
                       <span id=""lblRegNuber"" class=""InputLabel"" style=""vertical-align: top; position: relative; top: 4px;"">Регистрационен номер:</span>
                       <span id=""txtRegNumberCont""><input type=""text"" id=""txtRegNumber"" class=""RequiredInputField"" style=""width: 90px;  display: none; "" maxlength=""20""
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
                                 page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_ADD") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_ADD_BASICINFO") == UIAccessLevel.Disabled;

                screenHidden = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_ADD") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_ADD_BASICINFO") == UIAccessLevel.Hidden;

                basicInfoDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_ADD") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_ADD_BASICINFO") == UIAccessLevel.Disabled;

                basicInfoHidden = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_ADD") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_ADD_BASICINFO") == UIAccessLevel.Hidden;


                UIAccessLevel l;

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_ADD_BASICINFO_REGNUMBER");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblRegNuber");
                    disabledClientControls.Add("txtRegNumber");
                }
                if (l == UIAccessLevel.Hidden || screenHidden || basicInfoHidden)
                {
                    hiddenClientControls.Add("lblRegNuber");
                    hiddenClientControls.Add("txtRegNumberCont");
                    hiddenClientControls.Add("lblRegNumberValue");
                }
               
            }
            else // edit mode of page
            {
                screenDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_EDIT") == UIAccessLevel.Disabled || isPreview;


                screenHidden = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_EDIT") == UIAccessLevel.Hidden;

                basicInfoDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_EDIT") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_EDIT_BASICINFO") == UIAccessLevel.Disabled;

                basicInfoHidden = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_EDIT") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_EDIT_BASICINFO") == UIAccessLevel.Hidden;


                UIAccessLevel l;

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_EDIT_BASICINFO_REGNUMBER");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblRegNuber");
                    disabledClientControls.Add("txtRegNumber");
                    hiddenClientControls.Add("imgEditRegNumberCont");
                }
                if (l == UIAccessLevel.Hidden || screenHidden || basicInfoHidden)
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

            //List<IDropDownItem> ddiVehicleMakes = new List<IDropDownItem>();
            //List<VehicleMake> vehicleMakes = VehicleMakeUtil.GetAllVehicleMakes(page.CurrentUser);

            //foreach (VehicleMake vehicleMake in vehicleMakes)
            //{
            //    ddiVehicleMakes.Add(vehicleMake);
            //}            

            //string vehicleMakesHTML = ListItems.GetDropDownHtml(ddiVehicleMakes, null, "ddVehicleMake", true, null, "RepopulateVehicleModels(this.value);", "style='width: 310px;'", true);

            //List<IDropDownItem> ddiVehicleModels = new List<IDropDownItem>();
            //DropDownItem blankItem = new DropDownItem();
            //blankItem.Txt = ListItems.GetOptionChooseOne().Text;
            //blankItem.Val = ListItems.GetOptionChooseOne().Value;
            //ddiVehicleModels.Add(blankItem);

            //string vehicleModelsHTML = ListItems.GetDropDownHtml(ddiVehicleModels, null, "ddVehicleModel", false, null, "", "style='width: 280px;'", true);


            

            List<GTableItem> vehicleRoadabilities = GTableItemUtil.GetAllGTableItemsByTableName("VehicleRoadability", page.ModuleKey, 1, 0, 0, page.CurrentUser);
            List<IDropDownItem> ddiVehicleRoadability = new List<IDropDownItem>();
            foreach (GTableItem vehicleRoadability in vehicleRoadabilities)
            {
                ddiVehicleRoadability.Add(vehicleRoadability);
            }

            string vehicleRoadabilitiesHTML = ListItems.GetDropDownHtml(ddiVehicleRoadability, null, "ddVehicleRoadability", true, null, "", "style='width: 280px;'", true);
            string editVehicleRoadabilityHTML = @"<img id=""imgMaintVehicleRoadability"" alt=""Редактиране на списъка"" title=""Редактиране на списъка"" style=""cursor: pointer;"" src=""../Images/list_edit.png"" onclick=""ShowGTable('VehicleRoadability', 1, 1, RefreshVehicleRoadabilityList);"" />";


            List<GTableItem> vehicleEngineTypes = GTableItemUtil.GetAllGTableItemsByTableName("VehicleEngineType", page.ModuleKey, 1, 0, 0, page.CurrentUser);
            List<IDropDownItem> ddiVehicleEngineType = new List<IDropDownItem>();
            foreach (GTableItem vehicleEngineType in vehicleEngineTypes)
            {
                ddiVehicleEngineType.Add(vehicleEngineType);
            }

            string vehicleEngineTypeHTML = ListItems.GetDropDownHtml(ddiVehicleEngineType, null, "ddVehicleEngineType", true, null, "", "style='width: 130px;'", true);
            string editVehicleEngineTypeHTML = @"<img id=""imgMaintVehicleEngineType"" alt=""Редактиране на списъка"" title=""Редактиране на списъка"" style=""cursor: pointer;"" src=""../Images/list_edit.png"" onclick=""ShowGTable('VehicleEngineType', 1, 1, RefreshVehicleEngineTypeList);"" />";

            List<GTableItem> vehicleBodyTypes = GTableItemUtil.GetAllGTableItemsByTableName("VehicleBodyType", page.ModuleKey, 1, 0, 0, page.CurrentUser);
            List<IDropDownItem> ddiVehicleBodyType = new List<IDropDownItem>();
            foreach (GTableItem vehicleBodyType in vehicleBodyTypes)
            {
                ddiVehicleBodyType.Add(vehicleBodyType);
            }

            string vehicleBodyTypeHTML = ListItems.GetDropDownHtml(ddiVehicleBodyType, null, "ddVehicleBodyType", true, null, "", "style='width: 220px;'", true);
            string editVehicleBodyTypeHTML = @"<img id=""imgMaintVehicleBodyType"" alt=""Редактиране на списъка"" title=""Редактиране на списъка"" style=""cursor: pointer;"" src=""../Images/list_edit.png"" onclick=""ShowGTable('VehicleBodyType', 1, 1, RefreshVehicleBodyTypeList);"" />";

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
         <td style=""text-align: right; width: 60px;"">
            <span id=""lblVehicleMake"" class=""InputLabel"">Марка:</span>
         </td>
         <td style=""text-align: left; width: 330px;"">
            <input type=""text"" id=""txtVehicleMakeName"" class=""InputField"" style=""width: 304px;"" maxlength=""300"" />
         </td>
         <td style=""text-align: right; width: 90px;"">
            <span id=""lblVehicleModel"" class=""InputLabel"">Модел:</span>
         </td>
         <td style=""text-align: left; width: 310px;"">
            <input type=""text"" id=""txtVehicleModelName"" class=""InputField"" style=""width: 274px;"" maxlength=""300"" />
         </td>
      </tr>
      <tr>
         <td style=""text-align: right;"">
            <span id=""lblVehicleRoadability"" class=""InputLabel"">Проходимост:</span>
         </td>
         <td style=""text-align: left;"">
            " + vehicleRoadabilitiesHTML + editVehicleRoadabilityHTML + @"
         </td>
         <td style=""text-align: right;"">
         </td>
         <td style=""text-align: left;"">
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
            <span id=""lblCarryingCapacity"" class=""InputLabel"">Товароносимост (т):</span>
         </td>
         <td style=""text-align: left; width: 170px;"">
            <input type=""text"" id=""txtCarryingCapacity"" class=""InputField"" style=""width: 100px;"" maxlength=""50"" />
         </td>
         <td style=""text-align: right; width: 230px;"">
            <span id=""lblFirstRegistrationDate"" class=""InputLabel"">Дата на първата регистрация:</span>
         </td>
         <td style=""text-align: left; width: 250px;"">
            <span id=""txtFirstRegistrationDateCont""><input type=""text"" id=""txtFirstRegistrationDate"" class='" + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" /></span>
         </td>
      </tr>
      <tr>
         <td style=""text-align: right;"">
            <span id=""lblLoadingCapacity"" class=""InputLabel"">Товароподемност (т):</span>
         </td>
         <td style=""text-align: left;"">
            <input type=""text"" id=""txtLoadingCapacity"" class=""InputField"" style=""width: 100px;"" maxlength=""50"" />
         </td>
         <td style=""text-align: right;"">
            <span id=""lblLastAnnualTechnicalReviewDate"" class=""InputLabel"">Дата на последен ГТП:</span>
         </td>
         <td style=""text-align: left;"">
            <span id=""txtLastAnnualTechnicalReviewDateCont""><input type=""text"" id=""txtLastAnnualTechnicalReviewDate"" class='" + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" /></span>
         </td>
      </tr>
      <tr>
         <td style=""text-align: right;"">
            <span id=""lblSeats"" class=""InputLabel"">Брой места:</span>
         </td>
         <td style=""text-align: left;"">
            <input type=""text"" id=""txtSeats"" class=""InputField"" style=""width: 100px;"" maxlength=""50"" />
         </td>
         <td style=""text-align: right;"">
            <span id=""lblMileage"" class=""InputLabel"">Изминати километри:</span>
         </td>
         <td style=""text-align: left;"">
            <input type=""text"" id=""txtMileage"" class=""InputField"" style=""width: 100px;"" maxlength=""50"" />
         </td>
      </tr>
      <tr>
         <td style=""text-align: right;"">
            <span id=""lblVehicleEngineType"" class=""InputLabel"">Двигател:</span>
         </td>
         <td style=""text-align: left;"">
            " + vehicleEngineTypeHTML + editVehicleEngineTypeHTML +  @"
         </td>
         <td style=""text-align: right;"">
            <span id=""lblVehicleBodyType"" class=""InputLabel"">Каросерия:</span>
         </td>
         <td style=""text-align: left;"">
            " + vehicleBodyTypeHTML + editVehicleBodyTypeHTML + @"
         </td>
      </tr>
   </table>
</fieldset>

" + AddEditTechnics_BasicInfo_PageUtil.GetBasicInfoResidenceContent(page) + @"

<div style=""height: 10px;""></div>

<input type=""hidden"" id=""hdnVehicleId"" />";

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
                                 page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_ADD") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_ADD_BASICINFO") == UIAccessLevel.Disabled;

                basicInfoDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_ADD") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_ADD_BASICINFO") == UIAccessLevel.Disabled;

                
                UIAccessLevel l;
               
                l = page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_ADD_BASICINFO_INVENTORYNUMBER");

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

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_ADD_BASICINFO_TECHNICSCATEGORY");

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

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_ADD_BASICINFO_VEHICLEMAKE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblVehicleMake");
                    disabledClientControls.Add("txtVehicleMakeName");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblVehicleMake");
                    hiddenClientControls.Add("txtVehicleMakeName");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_ADD_BASICINFO_VEHICLEMODEL");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblVehicleModel");
                    disabledClientControls.Add("txtVehicleModelName");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblVehicleModel");
                    hiddenClientControls.Add("txtVehicleModelName");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_ADD_BASICINFO_VEHICLEKIND");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblVehicleKind");
                    disabledClientControls.Add("ddVehicleKind");
                    hiddenClientControls.Add("imgMaintVehicleKind");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblVehicleKind");
                    hiddenClientControls.Add("ddVehicleKind");
                    hiddenClientControls.Add("imgMaintVehicleKind");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_ADD_BASICINFO_VEHICLEROADABILITY");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblVehicleRoadability");
                    disabledClientControls.Add("ddVehicleRoadability");
                    hiddenClientControls.Add("imgMaintVehicleRoadability");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblVehicleRoadability");
                    hiddenClientControls.Add("ddVehicleRoadability");
                    hiddenClientControls.Add("imgMaintVehicleRoadability");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_ADD_BASICINFO_CARRYINGCAPACITY");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblCarryingCapacity");
                    disabledClientControls.Add("txtCarryingCapacity");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblCarryingCapacity");
                    hiddenClientControls.Add("txtCarryingCapacity");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_ADD_BASICINFO_FIRSTREGDATE");

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

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_ADD_BASICINFO_LOADINGCAPACITY");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblLoadingCapacity");
                    disabledClientControls.Add("txtLoadingCapacity");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblLoadingCapacity");
                    hiddenClientControls.Add("txtLoadingCapacity");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_ADD_BASICINFO_LASTANNTECHREVIEWDATE");

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

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_ADD_BASICINFO_SEATS");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblSeats");
                    disabledClientControls.Add("txtSeats");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblSeats");
                    hiddenClientControls.Add("txtSeats");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_ADD_BASICINFO_MILEAGE");

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

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_ADD_BASICINFO_VEHICLEENGINETYPE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblVehicleEngineType");
                    disabledClientControls.Add("ddVehicleEngineType");
                    hiddenClientControls.Add("imgMaintVehicleEngineType");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblVehicleEngineType");
                    hiddenClientControls.Add("ddVehicleEngineType");
                    hiddenClientControls.Add("imgMaintVehicleEngineType");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_ADD_BASICINFO_VEHICLEBODYTYPE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblVehicleBodyType");
                    disabledClientControls.Add("ddVehicleBodyType");
                    hiddenClientControls.Add("imgMaintVehicleBodyType");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblVehicleBodyType");
                    hiddenClientControls.Add("ddVehicleBodyType");
                    hiddenClientControls.Add("imgMaintVehicleBodyType");
                }

                AddEditTechnics_BasicInfo_PageUtil.GetBasicInfoResidenceUIItems(page, true,
                    ref disabledClientControls, ref hiddenClientControls);
            }
            else // edit mode of page
            {
                screenDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_EDIT") == UIAccessLevel.Disabled || isPreview;

                basicInfoDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_EDIT") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_EDIT_BASICINFO") == UIAccessLevel.Disabled;
                               
                UIAccessLevel l;
               
                l = page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_EDIT_BASICINFO_INVENTORYNUMBER");

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

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_EDIT_BASICINFO_TECHNICSCATEGORY");

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

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_EDIT_BASICINFO_VEHICLEMAKE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblVehicleMake");
                    disabledClientControls.Add("txtVehicleMakeName");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblVehicleMake");
                    hiddenClientControls.Add("txtVehicleMakeName");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_EDIT_BASICINFO_VEHICLEMODEL");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblVehicleModel");
                    disabledClientControls.Add("txtVehicleModelName");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblVehicleModel");
                    hiddenClientControls.Add("txtVehicleModelName");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_EDIT_BASICINFO_VEHICLEKIND");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblVehicleKind");
                    disabledClientControls.Add("ddVehicleKind");
                    hiddenClientControls.Add("imgMaintVehicleKind");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblVehicleKind");
                    hiddenClientControls.Add("ddVehicleKind");
                    hiddenClientControls.Add("imgMaintVehicleKind");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_EDIT_BASICINFO_VEHICLEROADABILITY");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblVehicleRoadability");
                    disabledClientControls.Add("ddVehicleRoadability");
                    hiddenClientControls.Add("imgMaintVehicleRoadability");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblVehicleRoadability");
                    hiddenClientControls.Add("ddVehicleRoadability");
                    hiddenClientControls.Add("imgMaintVehicleRoadability");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_EDIT_BASICINFO_CARRYINGCAPACITY");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblCarryingCapacity");
                    disabledClientControls.Add("txtCarryingCapacity");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblCarryingCapacity");
                    hiddenClientControls.Add("txtCarryingCapacity");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_EDIT_BASICINFO_FIRSTREGDATE");

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

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_EDIT_BASICINFO_LOADINGCAPACITY");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblLoadingCapacity");
                    disabledClientControls.Add("txtLoadingCapacity");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblLoadingCapacity");
                    hiddenClientControls.Add("txtLoadingCapacity");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_EDIT_BASICINFO_LASTANNTECHREVIEWDATE");

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

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_EDIT_BASICINFO_SEATS");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblSeats");
                    disabledClientControls.Add("txtSeats");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblSeats");
                    hiddenClientControls.Add("txtSeats");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_EDIT_BASICINFO_MILEAGE");

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

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_EDIT_BASICINFO_VEHICLEENGINETYPE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblVehicleEngineType");
                    disabledClientControls.Add("ddVehicleEngineType");
                    hiddenClientControls.Add("imgMaintVehicleEngineType");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblVehicleEngineType");
                    hiddenClientControls.Add("ddVehicleEngineType");
                    hiddenClientControls.Add("imgMaintVehicleEngineType");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_EDIT_BASICINFO_VEHICLEBODYTYPE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblVehicleBodyType");
                    disabledClientControls.Add("ddVehicleBodyType");
                    hiddenClientControls.Add("imgMaintVehicleBodyType");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblVehicleBodyType");
                    hiddenClientControls.Add("ddVehicleBodyType");
                    hiddenClientControls.Add("imgMaintVehicleBodyType");
                }

                AddEditTechnics_BasicInfo_PageUtil.GetBasicInfoResidenceUIItems(page, false,
                    ref disabledClientControls, ref hiddenClientControls);
            }

            if (page.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled ||
                page.GetUIItemAccessLevel("RES_LISTMAINT_VEHICLEKIND") != UIAccessLevel.Enabled)
            {
                hiddenClientControls.Add("imgMaintVehicleKind");
            }

            if (page.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled ||
                page.GetUIItemAccessLevel("RES_LISTMAINT_VEHICLEROADABILITY") != UIAccessLevel.Enabled)
            {
                hiddenClientControls.Add("imgMaintVehicleRoadability");
            }

            if (page.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled ||
                page.GetUIItemAccessLevel("RES_LISTMAINT_VEHICLEENGINETYPE") != UIAccessLevel.Enabled)
            {
                hiddenClientControls.Add("imgMaintVehicleEngineType");
            }

            if (page.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled ||
                page.GetUIItemAccessLevel("RES_LISTMAINT_VEHICLEBODYTYPE") != UIAccessLevel.Enabled)
            {
                hiddenClientControls.Add("imgMaintVehicleBodyType");
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
