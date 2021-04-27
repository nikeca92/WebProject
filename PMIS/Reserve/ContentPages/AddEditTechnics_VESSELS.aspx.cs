using System;
using System.Collections.Generic;
using System.Text;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class AddEditTechnics_VESSELS : RESPage
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
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSCheckInventNumber")
            {
                JSCheckInventNumber();
                return;
            }            

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRefreshVesselKindList")
            {
                JSRefreshVesselKindList();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRefreshVesselTypeList")
            {
                JSRefreshVesselTypeList();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSChangeInventNumber")
            {
                JSChangeInventNumber();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadInventNumberHistory")
            {
                JSLoadInventNumberHistory();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadVesselCrew")
            {
                JSLoadVesselCrew();
                return;
            }      
        }     

        //Load Basic Info (ajax call)
        private void JSLoadBasicInfo()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_VESSELS") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            int technicsId = 0;
            if (!String.IsNullOrEmpty(Request.Form["TechnicsId"]))
                technicsId = int.Parse(Request.Form["TechnicsId"]);

            string stat = "";
            string response = "";

            try
            {
                Vessel vessel = VesselUtil.GetVesselByTechnicsId(technicsId, CurrentUser);
                TechnicsMilRepStatus currentMilRepStatus = TechnicsMilRepStatusUtil.GetTechnicsMilRepCurrentStatusByTechnicsId(vessel.TechnicsId, CurrentUser);
                string currMilRepStatusName = (currentMilRepStatus != null ? currentMilRepStatus.TechMilitaryReportStatus.TechMilitaryReportStatusName : TechMilitaryReportStatusUtil.GetLabelWhenLackOfStatus());
          
                stat = AJAXTools.OK;

                response = @"
                    <vessel>
                         <technicsId>" + AJAXTools.EncodeForXML(vessel.TechnicsId.ToString()) + @"</technicsId>
                         <vesselId>" + AJAXTools.EncodeForXML(vessel.VesselId.ToString()) + @"</vesselId>
                         <vesselName>" + AJAXTools.EncodeForXML(vessel.VesselName) + @"</vesselName>
                         <inventoryNumber>" + AJAXTools.EncodeForXML(vessel.InventoryNumber) + @"</inventoryNumber>
                         <technicsCategoryId>" + AJAXTools.EncodeForXML(vessel.Technics.TechnicsCategoryId.HasValue ? vessel.Technics.TechnicsCategoryId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</technicsCategoryId>
                         <vesselKindId>" + AJAXTools.EncodeForXML(vessel.VesselKindId.HasValue ? vessel.VesselKindId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</vesselKindId>
                         <vesselTypeId>" + AJAXTools.EncodeForXML(vessel.VesselTypeId.HasValue ? vessel.VesselTypeId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</vesselTypeId>
                         <lastModified>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDateTime(vessel.Technics.LastModifiedDate)) + @"</lastModified>
                         <resMilRepStatus>" + AJAXTools.EncodeForXML(currMilRepStatusName) + @"</resMilRepStatus>
                         <loadDisplacement>" + AJAXTools.EncodeForXML(vessel.LoadDisplacement.HasValue ? CommonFunctions.FormatDecimal(vessel.LoadDisplacement.Value) : "") + @"</loadDisplacement>
                         <lightDisplacement>" + AJAXTools.EncodeForXML(vessel.LightDisplacement.HasValue ? CommonFunctions.FormatDecimal(vessel.LightDisplacement.Value) : "") + @"</lightDisplacement>
                         <length>" + AJAXTools.EncodeForXML(vessel.Length.HasValue ? CommonFunctions.FormatDecimal(vessel.Length.Value) : "") + @"</length>
                         <width>" + AJAXTools.EncodeForXML(vessel.Width.HasValue ? CommonFunctions.FormatDecimal(vessel.Width.Value) : "") + @"</width>
                         <maxHeight>" + AJAXTools.EncodeForXML(vessel.MaxHeight.HasValue ? CommonFunctions.FormatDecimal(vessel.MaxHeight.Value) : "") + @"</maxHeight>
                         <maxWadeLoad>" + AJAXTools.EncodeForXML(vessel.MaxWadeLoad.HasValue ? CommonFunctions.FormatDecimal(vessel.MaxWadeLoad.Value) : "") + @"</maxWadeLoad>
                         <maxWadeLight>" + AJAXTools.EncodeForXML(vessel.MaxWadeLight.HasValue ? CommonFunctions.FormatDecimal(vessel.MaxWadeLight.Value) : "") + @"</maxWadeLight>
                         <officers>" + AJAXTools.EncodeForXML(vessel.Officers.HasValue ? vessel.Officers.ToString() : "") + @"</officers>
                         <sailors>" + AJAXTools.EncodeForXML(vessel.Sailors.HasValue ? vessel.Sailors.ToString() : "") + @"</sailors>
                         <enginePower>" + AJAXTools.EncodeForXML(vessel.EnginePower.HasValue ? CommonFunctions.FormatDecimal(vessel.EnginePower.Value) : "") + @"</enginePower>
                         <speedNodes>" + AJAXTools.EncodeForXML(vessel.SpeedNodes.HasValue ? CommonFunctions.FormatDecimal(vessel.SpeedNodes.Value) : "") + @"</speedNodes>
                         <stopDate>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(vessel.StopDate)) + @"</stopDate>
                         <stopReasons>" + AJAXTools.EncodeForXML(vessel.StopReasons) + @"</stopReasons>
                         <residenceCityId>" + AJAXTools.EncodeForXML(vessel.Technics.ResidenceCityId != null ? vessel.Technics.ResidenceCityId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</residenceCityId>
                         <residencePostCode>" + AJAXTools.EncodeForXML(vessel.Technics.ResidencePostCode) + @"</residencePostCode>
                         <residenceRegionId>" + AJAXTools.EncodeForXML(vessel.Technics.ResidenceCityId != null ? vessel.Technics.ResidenceCity.RegionId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</residenceRegionId>
                         <residenceMunicipalityId>" + AJAXTools.EncodeForXML(vessel.Technics.ResidenceCityId != null ? vessel.Technics.ResidenceCity.MunicipalityId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</residenceMunicipalityId>
                         <residenceDistrictId>" + AJAXTools.EncodeForXML(vessel.Technics.ResidenceDistrictId != null ? vessel.Technics.ResidenceDistrict.DistrictId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</residenceDistrictId>
                         <residenceAddress>" + AJAXTools.EncodeForXML(vessel.Technics.ResidenceAddress) + @"</residenceAddress>
                         <currMilDepartment>" + AJAXTools.EncodeForXML(vessel.Technics.CurrTechMilRepStatus != null && vessel.Technics.CurrTechMilRepStatus.SourceMilDepartment != null ? vessel.Technics.CurrTechMilRepStatus.SourceMilDepartment.MilitaryDepartmentName : TechMilitaryReportStatusUtil.GetLabelWhenLackOfStatus()) + @"</currMilDepartment>
                         <normativeTechnicsId>" + AJAXTools.EncodeForXML(vessel.Technics.NormativeTechnicsId != null ? vessel.Technics.NormativeTechnics.NormativeTechnicsId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</normativeTechnicsId>
                         <normativeCode>" + AJAXTools.EncodeForXML(vessel.Technics.NormativeTechnicsId != null ? vessel.Technics.NormativeTechnics.NormativeCode : "") + @"</normativeCode>
                    </vessel>";             

                if (vessel.Technics.ResidenceCityId != null)
                {
                    List<Municipality> municipalities = MunicipalityUtil.GetMunicipalities(vessel.Technics.ResidenceCity.RegionId, CurrentUser);
                    List<City> cities = CityUtil.GetCities(vessel.Technics.ResidenceCity.MunicipalityId, CurrentUser);
                    List<District> districts = vessel.Technics.ResidenceCity.Districts;

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
                GetUIItemAccessLevel("RES_TECHNICS_VESSELS") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            int? technicsId = null;
            if (!String.IsNullOrEmpty(Request.Params["TechnicsId"]))
            {
                technicsId = int.Parse(Request.Params["TechnicsId"]);
            }

            int? vesselId = null;
            if (!String.IsNullOrEmpty(Request.Params["VesselId"]))
            {
                vesselId = int.Parse(Request.Params["VesselId"]);
            }

            string vesselName = Request.Params["VesselName"];
            string inventoryNumber = Request.Params["InventoryNumber"];
            
            int? technicsCategoryId = null;
            if (!String.IsNullOrEmpty(Request.Params["TechnicsCategoryId"]) &&
                Request.Params["TechnicsCategoryId"] != ListItems.GetOptionChooseOne().Value)
            {
                technicsCategoryId = int.Parse(Request.Params["TechnicsCategoryId"]);
            }          

            int? vesselKindId = null;
            if (!String.IsNullOrEmpty(Request.Params["VesselKindId"]) &&
                Request.Params["VesselKindId"] != ListItems.GetOptionChooseOne().Value)
            {
                vesselKindId = int.Parse(Request.Params["VesselKindId"]);
            }

            int? vesselTypeId = null;
            if (!String.IsNullOrEmpty(Request.Params["VesselTypeId"]) &&
                Request.Params["VesselTypeId"] != ListItems.GetOptionChooseOne().Value)
            {
                vesselTypeId = int.Parse(Request.Params["VesselTypeId"]);
            }

            decimal? loadDisplacement = null;
            if (!String.IsNullOrEmpty(Request.Params["LoadDisplacement"]))
            {
                loadDisplacement = decimal.Parse(Request.Params["LoadDisplacement"]);
            }

            decimal? lightDisplacement = null;
            if (!String.IsNullOrEmpty(Request.Params["LightDisplacement"]))
            {
                lightDisplacement = decimal.Parse(Request.Params["LightDisplacement"]);
            }

            decimal? length = null;
            if (!String.IsNullOrEmpty(Request.Params["Length"]))
            {
                length = decimal.Parse(Request.Params["Length"]);
            }

            decimal? width = null;
            if (!String.IsNullOrEmpty(Request.Params["Width"]))
            {
                width = decimal.Parse(Request.Params["Width"]);
            }

            decimal? maxHeight = null;
            if (!String.IsNullOrEmpty(Request.Params["MaxHeight"]))
            {
                maxHeight = decimal.Parse(Request.Params["MaxHeight"]);
            }

            decimal? maxWadeLoad = null;
            if (!String.IsNullOrEmpty(Request.Params["MaxWadeLoad"]))
            {
                maxWadeLoad = decimal.Parse(Request.Params["MaxWadeLoad"]);
            }

            decimal? maxWadeLight = null;
            if (!String.IsNullOrEmpty(Request.Params["MaxWadeLight"]))
            {
                maxWadeLight = decimal.Parse(Request.Params["MaxWadeLight"]);
            }

            int? officers = null;
            if (!String.IsNullOrEmpty(Request.Params["Officers"]))
            {
                officers = int.Parse(Request.Params["Officers"]);
            }

            int? sailors = null;
            if (!String.IsNullOrEmpty(Request.Params["Sailors"]))
            {
                sailors = int.Parse(Request.Params["Sailors"]);
            }

            decimal? enginePower = null;
            if (!String.IsNullOrEmpty(Request.Params["EnginePower"]))
            {
                enginePower = decimal.Parse(Request.Params["EnginePower"]);
            }

            decimal? speedNodes = null;
            if (!String.IsNullOrEmpty(Request.Params["SpeedNodes"]))
            {
                speedNodes = decimal.Parse(Request.Params["SpeedNodes"]);
            }

            DateTime? stopDate = null;
            if (!String.IsNullOrEmpty(Request.Params["StopDate"]))
            {
                stopDate = CommonFunctions.ParseDate(Request.Params["StopDate"]);
            }

            string stopReasons = Request.Params["StopReasons"];

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

            Vessel vessel = new Vessel(CurrentUser);

            vessel.VesselId = vesselId.HasValue ? vesselId.Value : 0;
            vessel.TechnicsId = technicsId.HasValue ? technicsId.Value : 0;
            vessel.VesselName = vesselName;
            vessel.InventoryNumber = inventoryNumber;          
            vessel.VesselKindId = vesselKindId;
            vessel.VesselTypeId = vesselTypeId;
            vessel.LoadDisplacement = loadDisplacement;
            vessel.LightDisplacement = lightDisplacement;
            vessel.Length = length;
            vessel.Width = width;
            vessel.MaxHeight = maxHeight;
            vessel.MaxWadeLoad = maxWadeLoad;
            vessel.MaxWadeLight = maxWadeLight;
            vessel.Officers = officers;
            vessel.Sailors = sailors;
            vessel.EnginePower = enginePower;
            vessel.SpeedNodes = speedNodes;
            vessel.StopDate = stopDate;
            vessel.StopReasons = stopReasons;         

            vessel.Technics = new Technics(CurrentUser);
            vessel.Technics.TechnicsId = vessel.TechnicsId;
            vessel.Technics.TechnicsType = TechnicsTypeUtil.GetTechnicsType("VESSELS", CurrentUser);
            vessel.Technics.TechnicsCategoryId = technicsCategoryId.HasValue ? technicsCategoryId.Value : (int?)null;
            vessel.Technics.ItemsCount = 1;
            vessel.Technics.ResidencePostCode = residencePostCode;
            vessel.Technics.ResidenceCityId = residenceCityId;
            vessel.Technics.ResidenceDistrictId = residenceDistrictId;
            vessel.Technics.ResidenceAddress = residenceAddress;
            vessel.Technics.NormativeTechnicsId = normativeTechnicsId;

            string stat = "";
            string response = "";

            try
            {
                //Track the changes into the Audit Trail 
                Change change = new Change(CurrentUser, "RES_Technics_VESSELS");

                VesselUtil.SaveVessel(vessel, CurrentUser, change);

                //Write into the Audit Trail
                change.WriteLog();

                stat = AJAXTools.OK;
                response = @"<response>OK</response>
                             <technicsId>" + AJAXTools.EncodeForXML(vessel.TechnicsId.ToString()) + @"</technicsId>
                             <vesselId>" + AJAXTools.EncodeForXML(vessel.VesselId.ToString()) + @"</vesselId>";
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
        private void JSCheckInventNumber()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_VESSELS") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string inventNumber = Request.Params["InventNumber"];

            string stat = "";
            string response = "";

            try
            {
                int technicsId = 0;

                Vessel vessel = VesselUtil.GetVesselByInventoryNumber(inventNumber, CurrentUser);

                if (vessel != null)
                {
                    technicsId = vessel.TechnicsId;
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

        //Refresh the list VesselKind (ajax call)
        private void JSRefreshVesselKindList()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_VESSELS") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<vesselKind>";

                response += "<k>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</k>";

                List<GTableItem> vesselKinds = GTableItemUtil.GetAllGTableItemsByTableName("VesselKind", ModuleKey, 1, 0, 0, CurrentUser);

                foreach (GTableItem vesselKind in vesselKinds)
                {
                    response += "<k>" +
                                "<id>" + vesselKind.TableKey.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(vesselKind.TableValue.ToString()) + "</name>" +
                                "</k>";
                }

                response += "</vesselKind>";

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

        //Refresh the list VesselType (ajax call)
        private void JSRefreshVesselTypeList()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_VESSELS") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<vesselType>";

                response += "<r>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</r>";

                List<GTableItem> vesselTypes = GTableItemUtil.GetAllGTableItemsByTableName("VesselType", ModuleKey, 1, 0, 0, CurrentUser);

                foreach (GTableItem vesselType in vesselTypes)
                {
                    response += "<r>" +
                                "<id>" + vesselType.TableKey.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(vesselType.TableValue.ToString()) + "</name>" +
                                "</r>";
                }

                response += "</vesselType>";

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
        private void JSChangeInventNumber()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_VESSELS") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            int vesselId = int.Parse(Request.Params["VesselId"]);
            string newInventoryNumber = Request.Params["NewInventoryNumber"];
            
            string stat = "";
            string response = "";

            try
            {
                Vessel existingVessel = VesselUtil.GetVesselByInventoryNumber(newInventoryNumber, CurrentUser);

                if (existingVessel == null)
                {
                    //Track the changes into the Audit Trail 
                    Change change = new Change(CurrentUser, "RES_Technics_VESSELS");

                    VesselUtil.ChangeInventoryNumber(vesselId, newInventoryNumber, CurrentUser, change);

                    //Write into the Audit Trail
                    change.WriteLog();

                    stat = AJAXTools.OK;
                    response = @"<response>OK</response>";
                }
                else
                {
                    stat = AJAXTools.OK;
                    response = @"<response>Вече съществува запис с този инвентарен номер</response>";
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

        private void JSLoadInventNumberHistory()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_VESSELS") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<response>" + AJAXTools.EncodeForXML(GetInvNumberHistoryLightBox(CurrentUser)) + "</response>";

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

        public string GetInvNumberHistoryLightBox(User currentUser)
        {
            string html = "";

            string htmlNoResults = "";

            List<VesselInventoryNumber> vesselInventNumbers = new List<VesselInventoryNumber>();
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

            int vesselId = 0;
            int.TryParse((Request.Params["VesselId"]).ToString(), out vesselId);

            allRows = VesselUtil.GetAllVesselInventoryNumbersCount(vesselId, currentUser);
            maxPage = allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            vesselInventNumbers = VesselUtil.GetAllVesselInventoryNumbers(vesselId, orderBy, pageIndex, pageLength, currentUser);

            // No data found
            if (vesselInventNumbers.Count == 0)
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

                        <span class='HeaderText'>История на инвентарните номера</span><br /><br /><br />

                        <div style='text-align: center;'>
                           <div style='display: inline; position: relative; top: -10px;'>
                              <img id='btnTableFirst' " + btnFirst + @" alt='Първа страница' title='Първа страница' class='PaginationButton' onclick=""BtnInvNumberHistoryPagingClick('btnFirst');"" />
                              <img id='btnTablePrev' " + btnPrev + @" alt='Предишна страница' title='Предишна страница' class='PaginationButton' onclick=""BtnInvNumberHistoryPagingClick('btnPrev');"" />
                              <span id='lblTablePagination' class='PaginationLabel'>" + pageTablePagination + @"</span>
                              <img id='btnTableNext' " + btnNext + @" alt='Следваща страница' title='Следваща страница' class='PaginationButton' onclick=""BtnInvNumberHistoryPagingClick('btnNext');"" />
                              <img id='btnTableLast' " + btnLast + @" alt='Последна страница' title='Последна страница' class='PaginationButton' onclick=""BtnInvNumberHistoryPagingClick('btnLast');"" />
                              
                              <span style='padding: 0 30px'>&nbsp;</span>
                              <span style='text-align: right;'>Отиди на страница</span>
                              <input id='txtTableGotoPage' class='InputField' type='text' style='width: 30px;' value='' />
                              <img id='btnTableGoto' src='../Images/ButtonGoto.png' alt='Отиди на страница' title='Отиди на страница' class='PaginationButton' onclick=""BtnInvNumberHistoryPagingClick('btnPageGo');"" />
                           </div>
                        </div>

                        <table id='tblVesselInvNumberHistory' class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            
                         </thead>";

            //Set Table Results
            string headerStyle = "vertical-align: bottom;";

            //Setup the header of the grid
            html += @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='width: 120px; " + headerStyle + @"'>Инвентарен номер</th>
                           </tr></thead>";

            int counter = 1;

            //Iterate through all items and add them into the grid
            foreach (VesselInventoryNumber vesselInventNumber in vesselInventNumbers)
            {

                string cellStyle = "vertical-align: top;";

                html += @"<tr  class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"' >
                                 <td style='" + cellStyle + @"'>" + ((pageIndex - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + vesselInventNumber.InventoryNumber + @"</td>
                              </tr>";

                counter++;
            }
            html += "</table>";

            html += "<input type='hidden' id='hdnVesselInvNumberHistoryCounter' value='" + counter + "' />";


            html += @"<div style='height: 10px;'>
                </div>

                 <div style='text-align: center'>
                   <span id='lblVesselInvNumberHistoryMessage'>" + htmlNoResults + @"</span><br/>
                </div>
";

            html += @"  </div>                        
                        <div id='btnCloseVesselInvNumberHistoryTable' runat='server' class='Button' onclick=""HideInvNumberHistoryLightBox();""><i></i><div style='width:70px; padding-left:5px;'>Затвори</div><b></b></div>
                      </center>";



            return html;
        }

        //Load Vessel Crew (ajax call)
        private void JSLoadVesselCrew()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_VESSELS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_OWNER_VESSELCREW") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            int vesselCrewId = 0;
            if (!String.IsNullOrEmpty(Request.Form["VesselCrewId"]))
                vesselCrewId = int.Parse(Request.Form["VesselCrewId"]);

            string stat = "";
            string response = "";

            try
            {
                VesselCrew vesselCrew = VesselCrewUtil.GetVesselCrew(vesselCrewId, CurrentUser);

                stat = AJAXTools.OK;

                response = @"
                    <vesselcrew>
                         <vesselCrewCategoryId>" + AJAXTools.EncodeForXML(vesselCrew.VesselCrewCategoryID.HasValue ? vesselCrew.VesselCrewCategoryID.ToString() : ListItems.GetOptionChooseOne().Value) + @"</vesselCrewCategoryId>
                         <identNumber>" + AJAXTools.EncodeForXML(vesselCrew.IdentNumber) + @"</identNumber>
                         <militaryRankId>" + AJAXTools.EncodeForXML(!String.IsNullOrEmpty(vesselCrew.MilitaryRankID) ? vesselCrew.MilitaryRankID.ToString() : ListItems.GetOptionChooseOne().Value) + @"</militaryRankId>
                         <fullName>" + AJAXTools.EncodeForXML(vesselCrew.FullName) + @"</fullName>
                         <address>" + AJAXTools.EncodeForXML(vesselCrew.Address) + @"</address>
                         <hasAppointment>" + AJAXTools.EncodeForXML(vesselCrew.HasAppointment.HasValue ? (vesselCrew.HasAppointment.Value ? "1" : "0") : "0") + @"</hasAppointment>
                    </vesselcrew>";               
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


    public static class AddEditTechnics_VESSELS_PageUtil
    {
        public static string GetGeneralPanelInventaryNumberContent()
        {
            string html = "";
            html = @"<div id=""tdInvNumber"">
                     <span id=""lblInventoryNumber"" class=""InputLabel"" style=""vertical-align: top; position: relative; top: 4px;"">Инвентарен номер:</span>
                     <span id=""txtInventoryNumberCont""><input type=""text"" id=""txtInventoryNumber"" class=""RequiredInputField"" style=""width: 90px; display: none;"" maxlength=""20""
                          onfocus=""InvNumberFocus();"" onblur=""InvNumberBlur();"" /></span>
                    <span id=""lblInventoryNumberValueCont""><span id=""lblInventoryNumberValue"" class=""ReadOnlyValue"" style=""display: none; vertical-align: top; position: relative; top: 4px;""></span></span>
                    <span id=""imgEditInvNumberCont""><img id=""imgEditInvNumber"" alt=""Промяна на инвентарния номер"" title=""Промяна на инвентарния номер"" style=""cursor: pointer; display: none;"" src=""../Images/list_edit.png"" onclick=""ChangeInvNumber();"" /></span>
                    <span id=""imgHistoryInvNumberCont""><img id=""imgHistoryInvNumber"" alt=""История на инвентарните номера"" title=""История на инвентарните номера"" style=""cursor: pointer; width: 18px; height: 18px; display: none;"" src=""../Images/index_view.png"" onclick=""InvNumberHistory_Click();"" /></span>
                    </div>
                    <div id=""ChangeInvNumberLightBox"" class=""ChangeVesselInvNumberLightBox"" style=""display: none; text-align: center;"">
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
                                        <span class=""HeaderText"" style=""text-align: center;"">Промяна на инвентарен номер</span>
                                    </td>
                                </tr>
                                <tr style=""height: 15px"">
                                </tr>
                                <tr style=""min-height: 17px"">
                                    <td style=""text-align: right;"">
                                        <span id=""lblCurrInventoryNumber"" class=""InputLabel"">Текущ инв. номер:</span>
                                    </td>
                                    <td style=""text-align: left;"">
                                        <span id=""lblCurrInventoryNumberValue"" class=""ReadOnlyValue""></span>
                                    </td>
                                </tr>
                                <tr style=""min-height: 17px"">
                                    <td style=""text-align: right;"">
                                        <span id=""lblNewInventoryNumber"" class=""InputLabel"">Нов инв. номер:</span>
                                    </td>
                                    <td style=""text-align: left;"">
                                        <input type=""text"" id=""txtNewInventoryNumber"" onblur = ""NewInventoryNumberBlur();"" class=""RequiredInputField"" style=""width: 90px;"" maxlength=""20"" UnsavedCheckSkipMe=""true"" />
                                    </td>
                                </tr>
                                <tr style=""height: 35px"">
                                    <td colspan=""2"" style=""padding-top: 5px;"">
                                        <span id=""spanChangeInvNumberLightBoxMessage"" class=""ErrorText"" style=""display: none;"">
                                        </span>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan=""2"" style=""text-align: center;"">
                                        <table style=""margin: 0 auto;"">
                                            <tr>
                                                <td>
                                                    <div id=""btnSaveChangeInvNumberLightBox"" style=""display: inline;"" onclick=""SaveChangeInvNumberLightBox();""
                                                        class=""Button"">
                                                        <i></i>
                                                        <div id=""btnSaveChangeInvNumberLightBoxText"" style=""width: 70px;"">
                                                            Запис</div>
                                                        <b></b>
                                                    </div>
                                                    <div id=""btnCloseChangeInvNumberLightBox"" style=""display: inline;"" onclick=""HideChangeInvNumberLightBox();""
                                                        class=""Button"">
                                                        <i></i>
                                                        <div id=""btnCloseChangeInvNumberLightBox"" style=""width: 70px;"">
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

                    <div id=""divInvNumberHistoryLightBox"" style=""display: none;"" class=""lboxInvNumberHistory""></div>
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
                                 page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_ADD") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_ADD_BASICINFO") == UIAccessLevel.Disabled;

                screenHidden = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_ADD") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_ADD_BASICINFO") == UIAccessLevel.Hidden;

                basicInfoDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_ADD") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_ADD_BASICINFO") == UIAccessLevel.Disabled;

                basicInfoHidden = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_ADD") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_ADD_BASICINFO") == UIAccessLevel.Hidden;


                UIAccessLevel l;

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_ADD_BASICINFO_INVENTORYNUMBER");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblInventoryNumber");
                    disabledClientControls.Add("txtInventoryNumber");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblInventoryNumber");
                    hiddenClientControls.Add("txtInventoryNumberCont");
                    hiddenClientControls.Add("lblInventoryNumberValue");
                }
                
            }
            else // edit mode of page
            {
                screenDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT") == UIAccessLevel.Disabled || isPreview;


                screenHidden = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT") == UIAccessLevel.Hidden;

                basicInfoDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_BASICINFO") == UIAccessLevel.Disabled;

                basicInfoHidden = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_BASICINFO") == UIAccessLevel.Hidden;


                UIAccessLevel l;

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_BASICINFO_INVENTORYNUMBER");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblInventoryNumber");
                    disabledClientControls.Add("txtInventoryNumber");
                    hiddenClientControls.Add("imgEditInvNumberCont");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblInventoryNumber");
                    hiddenClientControls.Add("txtInventoryNumberCont");
                    hiddenClientControls.Add("lblInventoryNumberValueCont");
                    hiddenClientControls.Add("imgEditInvNumberCont");
                    hiddenClientControls.Add("imgHistoryInvNumberCont");
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

            List<GTableItem> vesselKinds = GTableItemUtil.GetAllGTableItemsByTableName("VesselKind", page.ModuleKey, 1, 0, 0, page.CurrentUser);
            List<IDropDownItem> ddiVesselKinds = new List<IDropDownItem>();
            foreach (GTableItem vesselKind in vesselKinds)
            {
                ddiVesselKinds.Add(vesselKind);
            }

            string vesselKindsHTML = ListItems.GetDropDownHtml(ddiVesselKinds, null, "ddVesselKind", true, null, "", "style='width: 280px;'", true);
            string editVesselKindsHTML = @"<img id=""imgMaintVesselKind"" alt=""Редактиране на списъка"" title=""Редактиране на списъка"" style=""cursor: pointer;"" src=""../Images/list_edit.png"" onclick=""ShowGTable('VesselKind', 1, 1, RefreshVesselKindList);"" />";

            List<GTableItem> vesselTypes = GTableItemUtil.GetAllGTableItemsByTableName("VesselType", page.ModuleKey, 1, 0, 0, page.CurrentUser);
            List<IDropDownItem> ddiVesselType = new List<IDropDownItem>();
            foreach (GTableItem vesselType in vesselTypes)
            {
                ddiVesselType.Add(vesselType);
            }

            string vesselTypesHTML = ListItems.GetDropDownHtml(ddiVesselType, null, "ddVesselType", true, null, "", "style='width: 280px;'", true);
            string editVesselTypeHTML = @"<img id=""imgMaintVesselType"" alt=""Редактиране на списъка"" title=""Редактиране на списъка"" style=""cursor: pointer;"" src=""../Images/list_edit.png"" onclick=""ShowGTable('VesselType', 1, 1, RefreshVesselTypeList);"" />";


            string html = @"
<div style=""height: 10px;""></div>

<fieldset style=""width: 880px; padding: 0px;"">
   <table class=""InputRegion"" style=""width: 880px; padding: 10px; padding-top: 0px; margin-top: 0px; padding-left: 0px;"">
      <tr style=""height: 3px;"">
      </tr>
      <tr>
         <td style=""text-align: right;"">
            <span id=""lblVesselKind"" class=""InputLabel"">Вид:</span>
         </td>
         <td style=""text-align: left;"" colspan=""3"">
            " + vesselKindsHTML + editVesselKindsHTML + @"
            &nbsp;&nbsp;
            <span id=""lblVesselType"" class=""InputLabel"">Тип:</span>
            " + vesselTypesHTML + editVesselTypeHTML + @"
         </td>         
      </tr>
      <tr>
         <td style=""text-align: right;"">
            <span id=""lblVesselName"" class=""InputLabel"">Име на плавателното средство:</span>
         </td>
         <td style=""text-align: left;"">
            <input type=""text"" id=""txtVesselName"" class=""InputField"" style=""width: 335px;"" maxlength=""250"" />
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

<table>
    <tr>
        <td>
            <fieldset style=""width: 210px; height: 120px; padding: 0px;"">
               <table class=""InputRegion"" style=""width: 210px;  height: 120px; padding: 10px; padding-top: 0px; margin-top: 0px;"">
                  <tr style=""height: 3px;"">
                    <td colspan=""2"" style=""text-align: left;"">
                        <span style=""color: #0B4489; font-weight: bold; font-size: 1.1em; width: 100%; position: relative; top: -5px;"">Водоизместване</span>
                    </td>
                  </tr>     
                  <tr>
                     <td style=""text-align: right;"">
                        <span id=""lblLoadDisplacement"" class=""InputLabel"">Пълен:</span>
                     </td>
                     <td style=""text-align: left;"">
                        <input type=""text"" id=""txtLoadDisplacement"" class=""InputField"" style=""width: 100px;"" maxlength=""50"" />
                     </td>   
                  </tr>
                  <tr>
                     <td style=""text-align: right;"">
                        <span id=""lblLightDisplacement"" class=""InputLabel"">Празен:</span>
                     </td>
                     <td style=""text-align: left;"">
                        <input type=""text"" id=""txtLightDisplacement"" class=""InputField"" style=""width: 100px;"" maxlength=""50"" />
                     </td>   
                  </tr>                
               </table>
            </fieldset>
        </td>
        <td>
            <fieldset style=""width: 210px; height: 120px; padding: 0px;"">
               <table class=""InputRegion"" style=""width: 210px; height: 120px; padding: 10px; padding-top: 0px; margin-top: 0px;"">
                  <tr style=""height: 3px;"">
                    <td colspan=""2"" style=""text-align: left;"">
                        <span style=""color: #0B4489; font-weight: bold; font-size: 1.1em; width: 100%; position: relative; top: -5px;"">Размери</span>
                    </td>
                  </tr>     
                  <tr>
                     <td style=""text-align: right;"">
                        <span id=""lblLength"" class=""InputLabel"">Дължина:</span>
                     </td>
                     <td style=""text-align: left;"">
                        <input type=""text"" id=""txtLength"" class=""InputField"" style=""width: 100px;"" maxlength=""50"" />
                     </td>   
                  </tr>
                  <tr>
                     <td style=""text-align: right;"">
                        <span id=""lblWidth"" class=""InputLabel"">Ширина:</span>
                     </td>
                     <td style=""text-align: left;"">
                        <input type=""text"" id=""txtWidth"" class=""InputField"" style=""width: 100px;"" maxlength=""50"" />
                     </td>   
                  </tr>
                  <tr>
                     <td style=""text-align: right;"">
                        <span id=""lblMaxHeight"" class=""InputLabel"">Макс. височина:</span>
                     </td>
                     <td style=""text-align: left; vertical-align: bottom;"">
                        <input type=""text"" id=""txtMaxHeight"" class=""InputField"" style=""width: 100px;"" maxlength=""50"" />
                     </td>   
                  </tr>
               </table>
            </fieldset>
        </td>
        <td>
            <fieldset style=""width: 210px; height: 120px; padding: 0px;"">
               <table class=""InputRegion"" style=""width: 210px; height: 120px; padding: 10px; padding-top: 0px; margin-top: 0px;"">
                  <tr style=""height: 3px;"">
                    <td colspan=""2"" style=""text-align: left;"">
                        <span style=""color: #0B4489; font-weight: bold; font-size: 1.1em; width: 100%; position: relative; top: -5px;"">Максимално газене</span>
                    </td>
                  </tr>     
                  <tr>
                     <td style=""text-align: right;"">
                        <span id=""lblMaxWadeLoad"" class=""InputLabel"">Пълен:</span>
                     </td>
                     <td style=""text-align: left;"">
                        <input type=""text"" id=""txtMaxWadeLoad"" class=""InputField"" style=""width: 100px;"" maxlength=""50"" />
                     </td>   
                  </tr>
                  <tr>
                     <td style=""text-align: right;"">
                        <span id=""lblMaxWadeLight"" class=""InputLabel"">Празен:</span>
                     </td>
                     <td style=""text-align: left;"">
                        <input type=""text"" id=""txtMaxWadeLight"" class=""InputField"" style=""width: 100px;"" maxlength=""50"" />
                     </td>   
                  </tr>
               </table>
            </fieldset>
        </td>
        <td>
            <fieldset style=""width: 210px; height: 120px; padding: 0px;"">
               <table class=""InputRegion"" style=""width: 210px; height: 120px; padding: 10px; padding-top: 0px; margin-top: 0px;"">
                  <tr style=""height: 3px;"">
                    <td colspan=""2"" style=""text-align: left;"">
                        <span style=""color: #0B4489; font-weight: bold; font-size: 1.1em; width: 100%; position: relative; top: -5px;"">Екипаж</span>
                    </td>
                  </tr>     
                  <tr>
                     <td style=""text-align: right;"">
                        <span id=""lblOfficers"" class=""InputLabel"">Офицери:</span>
                     </td>
                     <td style=""text-align: left;"">
                        <input type=""text"" id=""txtOfficers"" class=""InputField"" style=""width: 100px;"" maxlength=""50"" />
                     </td>   
                  </tr>
                  <tr>
                     <td style=""text-align: right;"">
                        <span id=""lblSailors"" class=""InputLabel"">Моряци:</span>
                     </td>
                     <td style=""text-align: left;"">
                        <input type=""text"" id=""txtSailors"" class=""InputField"" style=""width: 100px;"" maxlength=""50"" />
                     </td>   
                  </tr>
               </table>
            </fieldset>
        </td>
    </tr>
</table>

<div style=""height: 10px;""></div>

<fieldset style=""width: 880px; padding: 0px;"">
   <table class=""InputRegion"" style=""width: 880px; padding: 10px; padding-top: 0px; margin-top: 0px;"">
      <tr style=""height: 3px;"">
      </tr>
      <tr>
         <td style=""text-align: right;"">
            <span id=""lblEnginePower"" class=""InputLabel"">Мощност на двигателната установка:</span>
         </td>
         <td style=""text-align: left;"">
            <input type=""text"" id=""txtEnginePower"" class=""InputField"" style=""width: 100px;"" maxlength=""50"" />
         </td>
         <td style=""text-align: right;"">
            <span id=""lblSpeedNodes"" class=""InputLabel"">Скорост във възли:</span>
         </td>
         <td style=""text-align: left;"">
            <input type=""text"" id=""txtSpeedNodes"" class=""InputField"" style=""width: 100px;"" maxlength=""50"" />
         </td>        
      </tr>      
   </table>
</fieldset>

<div style=""height: 10px;""></div>

<fieldset style=""width: 880px; padding: 0px;"">
   <table class=""InputRegion"" style=""width: 880px; padding: 10px; padding-top: 0px; margin-top: 0px;"">
      <tr style=""height: 3px;"">
      </tr>     
      <tr>
         <td style=""text-align: right;"">
            <span id=""lblStopDate"" class=""InputLabel"">Спиране на плавателния съд:</span>
         </td>
         <td style=""text-align: left;"">
            <span id=""txtStopDateCont""><input type=""text"" id=""txtStopDate"" class='" + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" /></span>
         </td>
         <td style=""text-align: right;"">
            <span id=""lblStopReasons"" class=""InputLabel"">Причини:</span>
         </td>
         <td style=""text-align: left;"" colspan=""3"">
            <input type=""text"" id=""txtStopReasons"" class=""InputField"" style=""width: 335px;"" maxlength=""250"" />
         </td>
      </tr>
   </table>
</fieldset>

<div id=""residenceContentHideDiv"" style=""display:none;"">
" + AddEditTechnics_BasicInfo_PageUtil.GetBasicInfoResidenceContent(page) + @"
</div>

<div style=""height: 10px;""></div>

<input type=""hidden"" id=""hdnVesselId"" />

";
            return html;
        }

        public static string GetVesselCrewTable(AddEditTechnics page, User currentUser)
        {
            System.Web.HttpRequest Request = page.Request;

            bool isPreview = false;
            if (!String.IsNullOrEmpty(page.Request.Params["Preview"]))
            {
                isPreview = true;
            }

            string html = "";

            string htmlNoResults = "";

            List<VesselCrew> vesselCrew = new List<VesselCrew>();
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

            int technicsId = page.TechnicsId;

            Vessel vessel = VesselUtil.GetVesselByTechnicsId(technicsId, currentUser);

            int vesselId = vessel.VesselId;

            allRows = VesselCrewUtil.GetAllVesselCrewByVesselIDCount(vesselId, currentUser);
            maxPage = allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            vesselCrew = VesselCrewUtil.GetAllVesselCrewByVesselID(vesselId, orderBy, pageIndex, pageLength, currentUser);

            // No data found
            if (vesselCrew.Count == 0)
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
            html += @"
                      <div style=""height: 10px;""></div>
                      <fieldset id=""fsVesselCrew"" style=""width: 830px; padding: 0px;"">
                      <table class=""InputRegion"" style=""width: 830px; padding: 10px; padding-top: 0px; margin-top: 0px;"">
                          <tr style=""height: 3px;"">
                          </tr>
                          <tr>
                            <td>
                              <center>
                              <div style='min-height: 150px; margin-bottom: 10px;'>

                                <input type='hidden' id='hdnOrderBy' value='" + orderBy + @"' />
                                <input type='hidden' id='hdnPageIndex' value='" + pageIndex + @"' />
                                <input type='hidden' id='hdnPageMaxPage' value='" + maxPage + @"' />

                                <span class='HeaderText'>Екипаж</span><br /><br /><br />

                                <div style='text-align: center;'>
                                   <div style='display: inline; position: relative; top: -10px;'>
                                      <img id='btnTableFirst' " + btnFirst + @" alt='Първа страница' title='Първа страница' class='PaginationButton' onclick=""BtnVesselCrewTablePagingClick('btnFirst');"" />
                                      <img id='btnTablePrev' " + btnPrev + @" alt='Предишна страница' title='Предишна страница' class='PaginationButton' onclick=""BtnVesselCrewTablePagingClick('btnPrev');"" />
                                      <span id='lblTablePagination' class='PaginationLabel'>" + pageTablePagination + @"</span>
                                      <img id='btnTableNext' " + btnNext + @" alt='Следваща страница' title='Следваща страница' class='PaginationButton' onclick=""BtnVesselCrewTablePagingClick('btnNext');"" />
                                      <img id='btnTableLast' " + btnLast + @" alt='Последна страница' title='Последна страница' class='PaginationButton' onclick=""BtnVesselCrewTablePagingClick('btnLast');"" />
                                      
                                      <span style='padding: 0 30px'>&nbsp;</span>
                                      <span style='text-align: right;'>Отиди на страница</span>
                                      <input id='txtTableGotoPage' class='InputField' type='text' style='width: 30px;' value='' />
                                      <img id='btnTableGoto' src='../Images/ButtonGoto.png' alt='Отиди на страница' title='Отиди на страница' class='PaginationButton' onclick=""BtnVesselCrewTablePagingClick('btnPageGo');"" />
                                   </div>
                                </div>

                                <table id='tblVesselCrew' class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                                 <thead>
                                    
                                 </thead>";

            //Set Table Results
            string headerStyle = "vertical-align: bottom;";
            int orderCol = (orderBy > 100 ? orderBy - 100 : orderBy);
            string img = orderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
            string[] arrOrderCol = { "", "", "", "", "", "" };

            arrOrderCol[orderCol - 1] = @"<div style='position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

            string newHTML = "<img src='../Images/note_add.png' alt='Нов запис' title='Нов запис' class='btnNewTableRecordIcon' onclick='NewVesselCrew();' />";            
            
            bool IsCategoryHidden = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_OWNER_VESSELCREW_CATEGORY") == UIAccessLevel.Hidden;
            bool IsIdentNumberHidden = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_OWNER_VESSELCREW_IDENTNUMBER") == UIAccessLevel.Hidden;
            bool IsMilitaryRankHidden = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_OWNER_VESSELCREW_MILITARYRANK") == UIAccessLevel.Hidden;
            bool IsFullNameHidden = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_OWNER_VESSELCREW_FULLNAME") == UIAccessLevel.Hidden;
            bool IsHasAppointmentHidden = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_OWNER_VESSELCREW_HASAPPOINTMENT") == UIAccessLevel.Hidden;
            bool IsAddressHidden = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_OWNER_VESSELCREW_ADDRESS") == UIAccessLevel.Hidden;
            
            bool IsEditable = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Enabled &&
                              page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS") == UIAccessLevel.Enabled &&
                              page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT") == UIAccessLevel.Enabled &&
                              page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_OWNER") == UIAccessLevel.Enabled &&
                              page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_OWNER_VESSELCREW") == UIAccessLevel.Enabled && !isPreview;

            //Setup the header of the grid
            html += @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th> " +
                               (!IsCategoryHidden ? @"<th style='width: 120px; width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortVesselCrewTableBy(1);'>Категория" + arrOrderCol[0] + @"</th>" : "") +
                               (!IsIdentNumberHidden ? @"<th style='width: 120px; width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortVesselCrewTableBy(2);'>ЕГН" + arrOrderCol[1] + @"</th>" : "") +
                               (!IsMilitaryRankHidden ? @"<th style='width: 120px; width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortVesselCrewTableBy(3);'>Звание" + arrOrderCol[2] + @"</th>" : "") +
                               (!IsFullNameHidden ? @"<th style='width: 120px; width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortVesselCrewTableBy(4);'>Трите имена" + arrOrderCol[3] + @"</th>" : "") +
                               (!IsHasAppointmentHidden ? @"<th style='width: 50px; width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortVesselCrewTableBy(5);'>МН" + arrOrderCol[4] + @"</th>" : "") +
                               (!IsAddressHidden ? @"<th style='width: 120px; width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortVesselCrewTableBy(6);'>Адрес" + arrOrderCol[5] + @"</th>" : "") +
                               (IsEditable ? @"<th style='width: 50px; " + headerStyle + @"'><div class='btnNewTableRecord'>" + newHTML + @"</div></th>" :"") +
                           @"</tr></thead>";

            int counter = 1;

            //Iterate through all items and add them into the grid
            foreach (VesselCrew crew in vesselCrew)
            {

                string cellStyle = "vertical-align: top;";

                string deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване' class='GridActionIcon' onclick='DeleteVesselCrew(" + crew.VesselCrewID.ToString() + ");' />";                                

                string editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='EditVesselCrew(" + crew.VesselCrewID.ToString() + ");' />";
                
                html += @"<tr  class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"' >
                                 <td style='" + cellStyle + @"'>" + ((pageIndex - 1) * pageLength + counter).ToString() + @"</td>" +
                                 (!IsCategoryHidden ?  @"<td style='" + cellStyle + @"'>" + (crew.VesselCrewCategory != null ? crew.VesselCrewCategory.VesselCrewCategoryName : "") + @"</td>" : "") +
                                 (!IsIdentNumberHidden ? @"<td style='" + cellStyle + @"'>" + crew.IdentNumber + @"</td>" : "") +
                                 (!IsMilitaryRankHidden ? @"<td style='" + cellStyle + @"'>" + (crew.MilitaryRank != null ? crew.MilitaryRank.LongName : "") + @"</td>" : "") +
                                 (!IsFullNameHidden ? @"<td style='" + cellStyle + @"'>" + crew.FullName + @"</td>" : "") +
                                 (!IsHasAppointmentHidden ? @"<td style='" + cellStyle + @"'>" + (crew.HasAppointment != null ? (crew.HasAppointment.Value ? "Да" : "Не") : "") + @"</td>" : "") +
                                 (!IsAddressHidden ? @"<td style='" + cellStyle + @"'>" + crew.Address + @"</td>" : "") +
                                 (IsEditable ? @"<td style='text-align: left;' nowrap>" + editHTML + deleteHTML + @"</td>" : "") +
                            @"</tr>";

                counter++;
            }
            html += "</table>";

            html += "<input type='hidden' id='hdnVesselCrewCounter' value='" + counter + "' />";


            html += @"<div style='height: 10px;'>
                </div>

                 <div style='text-align: center'>
                   <span id='lblVesselCrewMessage'>" + htmlNoResults + @"</span><br/>
                </div>
";

            html += @"              </div>                                    
                                  </center>
                              </td>
                          </tr>
                      </table>
                      </fieldset>";



            return html;
        }

        public static string GetVesselCrewLightBox(AddEditTechnics page, User currentUser)
        {
            List<VesselCrewCategory> vesselCrewCategories = VesselCrewCategoryUtil.GetVesselCrewCategories(currentUser);
            List<IDropDownItem> ddiVesselCrewCategories = new List<IDropDownItem>();

            foreach (VesselCrewCategory vesselCrewCategory in vesselCrewCategories)
            {
                ddiVesselCrewCategories.Add(vesselCrewCategory);
            }

            // Generates html for drop down list
            string vesselCrewCategoriesHTML = ListItems.GetDropDownHtml(ddiVesselCrewCategories, null, "ddVesselCrewCategoryLightBox", true, null, "", "style='width: 180px;' UnsavedCheckSkipMe='true' ", true);

            List<MilitaryRank> militaryRanks = MilitaryRankUtil.GetAllMilitaryRanks(currentUser);
            List<IDropDownItem> ddiMilitaryRanks = new List<IDropDownItem>();

            foreach (MilitaryRank militaryRank in militaryRanks)
            {
                ddiMilitaryRanks.Add(militaryRank);
            }

            // Generates html for drop down list
            string militaryRanksHTML = ListItems.GetDropDownHtml(ddiMilitaryRanks, null, "ddMilitaryRankLightBox", true, null, "", "style='width: 180px;' UnsavedCheckSkipMe='true' ", true);



            string html = @"<div id=""lboxVesselCrew"" style=""display: none;"" class=""lboxVesselCrew"">
<center>
    <input type=""hidden"" id=""hdnVesselCrewID"" />
    <table width=""80%"" style=""text-align: center;"">
        <colgroup style=""width: 40%"">
        </colgroup>
        <colgroup style=""width: 60%"">
        </colgroup>
        <tr style=""height: 15px"">
        </tr>
        <tr>
            <td colspan=""2"" align=""center"">
                <span class=""HeaderText"" style=""text-align: center;"" id=""lblAddEditVesselCrewTitle""></span>
            </td>
        </tr>
        <tr style=""height: 15px"">
        </tr>  
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblVesselCrewCategoryLightBox"" class=""InputLabel"">Категория:</span>
            </td>
            <td style=""text-align: left;"">
                " + vesselCrewCategoriesHTML + @"
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right; width: 80px;"">
                <span id=""lblIdentNumberLightBox"" class=""InputLabel"">ЕГН:</span>
            </td>
            <td style=""text-align: left; width: 420px;"">
                <input id=""txtIdentNumberLightBox"" type=""text"" class=""InputField"" maxlength=""10"" UnsavedCheckSkipMe=""true"" />
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblMilitaryRankLightBox"" class=""InputLabel"">Звание:</span>
            </td>
            <td style=""text-align: left;"">
                " + militaryRanksHTML + @"
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right; width: 80px;"">
                <span id=""lblFullNameLightBox"" class=""InputLabel"">Име:</span>
            </td>
            <td style=""text-align: left; width: 420px;"">
                <input id=""txtFullNameLightBox"" type=""text"" class=""InputField"" maxlength=""250"" UnsavedCheckSkipMe=""true""/>
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right; width: 80px;"">
                <span id=""lblAddressLightBox"" class=""InputLabel"">Адрес:</span>
            </td>
            <td style=""text-align: left; width: 420px;"">
                <input id=""txtAddressLightBox"" type=""text"" class=""InputField"" maxlength=""250"" UnsavedCheckSkipMe=""true""/>
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td></td>
            <td style=""text-align: left;"">
                <input type=""checkbox"" id=""chkHasAppointmentLightBox"" />
                <span id=""lblHasAppointmentLightBox"" class=""InputLabel"">МН</span>
            </td>
        </tr>
        <tr style=""height: 23px; padding-top: 5px;"">
            <td colspan=""2"">
                <span id=""spanAddEditVesselCrewLightBox"" class=""ErrorText"" style=""display: none;"">
                </span>&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan=""2"" style=""text-align: center;"">
                <table style=""margin: 0 auto;"">
                    <tr>
                        <td>
                            <div id=""btnSaveAddEditVesselCrewLightBox"" style=""display: inline;"" onclick=""SaveAddEditVesselCrewLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnSaveAddEditVesselCrewLightBoxText"" style=""width: 70px;"">
                                    Запис</div>
                                <b></b>
                            </div>
                            <div id=""btnCloseAddEditVesselCrewLightBox"" style=""display: inline;"" onclick=""HideAddEditVesselCrewLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnCloseAddEditVesselCrewLightBoxText"" style=""width: 70px;"">
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
                                 page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_ADD") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_ADD_BASICINFO") == UIAccessLevel.Disabled;

                basicInfoDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_ADD") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_ADD_BASICINFO") == UIAccessLevel.Disabled;

                UIAccessLevel l;
                
                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_ADD_BASICINFO_VESSELNAME");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblVesselName");
                    disabledClientControls.Add("txtVesselName");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblVesselName");
                    hiddenClientControls.Add("txtVesselName");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_ADD_BASICINFO_TECHNICSCATEGORY");

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

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_ADD_BASICINFO_VESSELKIND");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblVesselKind");
                    disabledClientControls.Add("ddVesselKind");
                    hiddenClientControls.Add("imgMaintVesselKind");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblVesselKind");
                    hiddenClientControls.Add("ddVesselKind");
                    hiddenClientControls.Add("imgMaintVesselKind");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_ADD_BASICINFO_VESSELTYPE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblVesselType");
                    disabledClientControls.Add("ddVesselType");
                    hiddenClientControls.Add("imgMaintVesselType");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblVesselType");
                    hiddenClientControls.Add("ddVesselType");
                    hiddenClientControls.Add("imgMaintVesselType");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_ADD_BASICINFO_LOADDISPLACEMENT");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblLoadDisplacement");
                    disabledClientControls.Add("txtLoadDisplacement");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblLoadDisplacement");
                    hiddenClientControls.Add("txtLoadDisplacement");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_ADD_BASICINFO_LIGHTDISPLACEMENT");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblLightDisplacement");
                    disabledClientControls.Add("txtLightDisplacement");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblLightDisplacement");
                    hiddenClientControls.Add("txtLightDisplacement");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_ADD_BASICINFO_LENGTH");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblLength");
                    disabledClientControls.Add("txtLength");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblLength");
                    hiddenClientControls.Add("txtLength");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_ADD_BASICINFO_WIDTH");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblWidth");
                    disabledClientControls.Add("txtWidth");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWidth");
                    hiddenClientControls.Add("txtWidth");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_ADD_BASICINFO_MAXHEIGHT");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblMaxHeight");
                    disabledClientControls.Add("txtMaxHeight");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblMaxHeight");
                    hiddenClientControls.Add("txtMaxHeight");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_ADD_BASICINFO_MAXWADELOAD");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblMaxWadeLoad");
                    disabledClientControls.Add("txtMaxWadeLoad");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblMaxWadeLoad");
                    hiddenClientControls.Add("txtMaxWadeLoad");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_ADD_BASICINFO_MAXWADELIGHT");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblMaxWadeLight");
                    disabledClientControls.Add("txtMaxWadeLight");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblMaxWadeLight");
                    hiddenClientControls.Add("txtMaxWadeLight");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_ADD_BASICINFO_OFFICERS");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblOfficers");
                    disabledClientControls.Add("txtOfficers");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblOfficers");
                    hiddenClientControls.Add("txtOfficers");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_ADD_BASICINFO_SAILORS");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblSailors");
                    disabledClientControls.Add("txtSailors");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblSailors");
                    hiddenClientControls.Add("txtSailors");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_ADD_BASICINFO_ENGINEPOWER");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblEnginePower");
                    disabledClientControls.Add("txtEnginePower");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEnginePower");
                    hiddenClientControls.Add("txtEnginePower");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_ADD_BASICINFO_SPEEDNODES");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblSpeedNodes");
                    disabledClientControls.Add("txtSpeedNodes");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblSpeedNodes");
                    hiddenClientControls.Add("txtSpeedNodes");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_ADD_BASICINFO_STOPDATE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblStopDate");
                    disabledClientControls.Add("txtStopDate");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblStopDate");
                    hiddenClientControls.Add("txtStopDateCont");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_ADD_BASICINFO_STOPREASONS");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblStopReasons");
                    disabledClientControls.Add("txtStopReasons");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblStopReasons");
                    hiddenClientControls.Add("txtStopReasons");
                }

                AddEditTechnics_BasicInfo_PageUtil.GetBasicInfoResidenceUIItems(page, true,
                    ref disabledClientControls, ref hiddenClientControls);
            }
            else // edit mode of page
            {
                screenDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT") == UIAccessLevel.Disabled || isPreview;

                basicInfoDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_BASICINFO") == UIAccessLevel.Disabled;
                                
                UIAccessLevel l;
                
                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_BASICINFO_VESSELNAME");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblVesselName");
                    disabledClientControls.Add("txtVesselName");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblVesselName");
                    hiddenClientControls.Add("txtVesselName");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_BASICINFO_TECHNICSCATEGORY");

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

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_BASICINFO_VESSELKIND");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblVesselKind");
                    disabledClientControls.Add("ddVesselKind");
                    hiddenClientControls.Add("imgMaintVesselKind");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblVesselKind");
                    hiddenClientControls.Add("ddVesselKind");
                    hiddenClientControls.Add("imgMaintVesselKind");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_BASICINFO_VESSELTYPE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblVesselType");
                    disabledClientControls.Add("ddVesselType");
                    hiddenClientControls.Add("imgMaintVesselType");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblVesselType");
                    hiddenClientControls.Add("ddVesselType");
                    hiddenClientControls.Add("imgMaintVesselType");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_BASICINFO_LOEDITISPLACEMENT");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblLoadDisplacement");
                    disabledClientControls.Add("txtLoadDisplacement");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblLoadDisplacement");
                    hiddenClientControls.Add("txtLoadDisplacement");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_BASICINFO_LIGHTDISPLACEMENT");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblLightDisplacement");
                    disabledClientControls.Add("txtLightDisplacement");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblLightDisplacement");
                    hiddenClientControls.Add("txtLightDisplacement");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_BASICINFO_LENGTH");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblLength");
                    disabledClientControls.Add("txtLength");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblLength");
                    hiddenClientControls.Add("txtLength");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_BASICINFO_WIDTH");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblWidth");
                    disabledClientControls.Add("txtWidth");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWidth");
                    hiddenClientControls.Add("txtWidth");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_BASICINFO_MAXHEIGHT");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblMaxHeight");
                    disabledClientControls.Add("txtMaxHeight");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblMaxHeight");
                    hiddenClientControls.Add("txtMaxHeight");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_BASICINFO_MAXWADELOAD");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblMaxWadeLoad");
                    disabledClientControls.Add("txtMaxWadeLoad");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblMaxWadeLoad");
                    hiddenClientControls.Add("txtMaxWadeLoad");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_BASICINFO_MAXWADELIGHT");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblMaxWadeLight");
                    disabledClientControls.Add("txtMaxWadeLight");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblMaxWadeLight");
                    hiddenClientControls.Add("txtMaxWadeLight");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_BASICINFO_OFFICERS");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblOfficers");
                    disabledClientControls.Add("txtOfficers");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblOfficers");
                    hiddenClientControls.Add("txtOfficers");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_BASICINFO_SAILORS");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblSailors");
                    disabledClientControls.Add("txtSailors");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblSailors");
                    hiddenClientControls.Add("txtSailors");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_BASICINFO_ENGINEPOWER");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblEnginePower");
                    disabledClientControls.Add("txtEnginePower");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEnginePower");
                    hiddenClientControls.Add("txtEnginePower");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_BASICINFO_SPEEDNODES");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblSpeedNodes");
                    disabledClientControls.Add("txtSpeedNodes");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblSpeedNodes");
                    hiddenClientControls.Add("txtSpeedNodes");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_BASICINFO_STOPDATE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblStopDate");
                    disabledClientControls.Add("txtStopDate");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblStopDate");
                    hiddenClientControls.Add("txtStopDateCont");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_VESSELS_EDIT_BASICINFO_STOPREASONS");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblStopReasons");
                    disabledClientControls.Add("txtStopReasons");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblStopReasons");
                    hiddenClientControls.Add("txtStopReasons");
                }             

                AddEditTechnics_BasicInfo_PageUtil.GetBasicInfoResidenceUIItems(page, false,
                    ref disabledClientControls, ref hiddenClientControls);
            }

            if (page.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled ||
                page.GetUIItemAccessLevel("RES_LISTMAINT_VESSELKIND") != UIAccessLevel.Enabled)
            {
                hiddenClientControls.Add("imgMaintVesselKind");
            }

            if (page.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled ||
                page.GetUIItemAccessLevel("RES_LISTMAINT_VESSELTYPE") != UIAccessLevel.Enabled)
            {
                hiddenClientControls.Add("imgMaintVesselType");
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
