using System;
using System.Collections.Generic;
using System.Text;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class AddEditTechnics_TRAILERS : RESPage
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
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRefreshTrailerKindList")
            {
                JSRefreshTrailerKindList();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRefreshTrailerTypeList")
            {
                JSRefreshTrailerTypeList();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRefreshTrailerBodyKindList")
            {
                JSRefreshTrailerBodyKindList();
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
                GetUIItemAccessLevel("RES_TECHNICS_TRAILERS") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            int technicsId = 0;
            if (!String.IsNullOrEmpty(Request.Form["TechnicsId"]))
                technicsId = int.Parse(Request.Form["TechnicsId"]);

            string stat = "";
            string response = "";

            try
            {
                Trailer trailer = TrailerUtil.GetTrailerByTechnicsId(technicsId, CurrentUser);
                TechnicsMilRepStatus currentMilRepStatus = TechnicsMilRepStatusUtil.GetTechnicsMilRepCurrentStatusByTechnicsId(trailer.TechnicsId, CurrentUser);
                string currMilRepStatusName = (currentMilRepStatus != null ? currentMilRepStatus.TechMilitaryReportStatus.TechMilitaryReportStatusName : TechMilitaryReportStatusUtil.GetLabelWhenLackOfStatus());
           
                stat = AJAXTools.OK;

                response = @"
                    <trailer>
                         <technicsId>" + AJAXTools.EncodeForXML(trailer.TechnicsId.ToString()) + @"</technicsId>
                         <trailerId>" + AJAXTools.EncodeForXML(trailer.TrailerId.ToString()) + @"</trailerId>
                         <regNumber>" + AJAXTools.EncodeForXML(trailer.RegNumber) + @"</regNumber>
                         <inventoryNumber>" + AJAXTools.EncodeForXML(trailer.InventoryNumber) + @"</inventoryNumber>
                         <lastModified>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDateTime(trailer.Technics.LastModifiedDate)) + @"</lastModified>
                         <resMilRepStatus>" + AJAXTools.EncodeForXML(currMilRepStatusName) + @"</resMilRepStatus>
                         <technicsCategoryId>" + AJAXTools.EncodeForXML(trailer.Technics.TechnicsCategoryId.HasValue ? trailer.Technics.TechnicsCategoryId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</technicsCategoryId>
                         <trailerKindId>" + AJAXTools.EncodeForXML(trailer.TrailerKindId.HasValue ? trailer.TrailerKindId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</trailerKindId>
                         <trailerTypeId>" + AJAXTools.EncodeForXML(trailer.TrailerTypeId.HasValue ? trailer.TrailerTypeId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</trailerTypeId>
                         <trailerBodyKindId>" + AJAXTools.EncodeForXML(trailer.TrailerBodyKindId.HasValue ? trailer.TrailerBodyKindId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</trailerBodyKindId>
                         <carryingCapacity>" + AJAXTools.EncodeForXML(trailer.CarryingCapacity.HasValue ? trailer.CarryingCapacity.ToString() : "") + @"</carryingCapacity>
                         <firstRegistrationDate>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(trailer.FirstRegistrationDate)) + @"</firstRegistrationDate>
                         <lastAnnualTechnicalReviewDate>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(trailer.LastAnnualTechnicalReviewDate)) + @"</lastAnnualTechnicalReviewDate>
                         <mileage>" + AJAXTools.EncodeForXML(trailer.Mileage.HasValue ? trailer.Mileage.ToString() : "") + @"</mileage>
                         <residenceCityId>" + AJAXTools.EncodeForXML(trailer.Technics.ResidenceCityId != null ? trailer.Technics.ResidenceCityId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</residenceCityId>
                         <residencePostCode>" + AJAXTools.EncodeForXML(trailer.Technics.ResidencePostCode) + @"</residencePostCode>
                         <residenceRegionId>" + AJAXTools.EncodeForXML(trailer.Technics.ResidenceCityId != null ? trailer.Technics.ResidenceCity.RegionId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</residenceRegionId>
                         <residenceMunicipalityId>" + AJAXTools.EncodeForXML(trailer.Technics.ResidenceCityId != null ? trailer.Technics.ResidenceCity.MunicipalityId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</residenceMunicipalityId>
                         <residenceDistrictId>" + AJAXTools.EncodeForXML(trailer.Technics.ResidenceDistrictId != null ? trailer.Technics.ResidenceDistrict.DistrictId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</residenceDistrictId>
                         <residenceAddress>" + AJAXTools.EncodeForXML(trailer.Technics.ResidenceAddress) + @"</residenceAddress>
                         <currMilDepartment>" + AJAXTools.EncodeForXML(trailer.Technics.CurrTechMilRepStatus != null && trailer.Technics.CurrTechMilRepStatus.SourceMilDepartment != null ? trailer.Technics.CurrTechMilRepStatus.SourceMilDepartment.MilitaryDepartmentName : TechMilitaryReportStatusUtil.GetLabelWhenLackOfStatus()) + @"</currMilDepartment>
                         <normativeTechnicsId>" + AJAXTools.EncodeForXML(trailer.Technics.NormativeTechnicsId != null ? trailer.Technics.NormativeTechnics.NormativeTechnicsId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</normativeTechnicsId>
                         <normativeCode>" + AJAXTools.EncodeForXML(trailer.Technics.NormativeTechnicsId != null ? trailer.Technics.NormativeTechnics.NormativeCode : "") + @"</normativeCode>
                    </trailer>";

                if (trailer.Technics.ResidenceCityId != null)
                {
                    List<Municipality> municipalities = MunicipalityUtil.GetMunicipalities(trailer.Technics.ResidenceCity.RegionId, CurrentUser);
                    List<City> cities = CityUtil.GetCities(trailer.Technics.ResidenceCity.MunicipalityId, CurrentUser);
                    List<District> districts = trailer.Technics.ResidenceCity.Districts;

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
                GetUIItemAccessLevel("RES_TECHNICS_TRAILERS") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            int? technicsId = null;
            if (!String.IsNullOrEmpty(Request.Params["TechnicsId"]))
            {
                technicsId = int.Parse(Request.Params["TechnicsId"]);
            }

            int? trailerId = null;
            if (!String.IsNullOrEmpty(Request.Params["TrailerId"]))
            {
                trailerId = int.Parse(Request.Params["TrailerId"]);
            }

            string regNumber = Request.Params["RegNumber"];
            string inventoryNumber = Request.Params["InventoryNumber"];
            
            int? technicsCategoryId = null;
            if (!String.IsNullOrEmpty(Request.Params["TechnicsCategoryId"]) &&
                Request.Params["TechnicsCategoryId"] != ListItems.GetOptionChooseOne().Value)
            {
                technicsCategoryId = int.Parse(Request.Params["TechnicsCategoryId"]);
            }

            int? trailerKindId = null;
            if (!String.IsNullOrEmpty(Request.Params["TrailerKindId"]) &&
                Request.Params["TrailerKindId"] != ListItems.GetOptionChooseOne().Value)
            {
                trailerKindId = int.Parse(Request.Params["TrailerKindId"]);
            }

            int? trailerTypeId = null;
            if (!String.IsNullOrEmpty(Request.Params["TrailerTypeId"]) &&
                Request.Params["TrailerTypeId"] != ListItems.GetOptionChooseOne().Value)
            {
                trailerTypeId = int.Parse(Request.Params["TrailerTypeId"]);
            }

            int? trailerBodyKindId = null;
            if (!String.IsNullOrEmpty(Request.Params["TrailerBodyKindId"]) &&
                Request.Params["TrailerBodyKindId"] != ListItems.GetOptionChooseOne().Value)
            {
                trailerBodyKindId = int.Parse(Request.Params["TrailerBodyKindId"]);
            }

            decimal? carryingCapacity = null;
            if (!String.IsNullOrEmpty(Request.Params["CarryingCapacity"]))
            {
                carryingCapacity = decimal.Parse(Request.Params["CarryingCapacity"]);
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

            Trailer trailer = new Trailer(CurrentUser);

            trailer.TrailerId = trailerId.HasValue ? trailerId.Value : 0;
            trailer.TechnicsId = technicsId.HasValue ? technicsId.Value : 0;
            trailer.RegNumber = regNumber;
            trailer.InventoryNumber = inventoryNumber;
            trailer.TrailerKindId = trailerKindId;
            trailer.TrailerTypeId = trailerTypeId;
            trailer.TrailerBodyKindId = trailerBodyKindId;
            trailer.CarryingCapacity = carryingCapacity;
            trailer.FirstRegistrationDate = firstRegistrationDate;
            trailer.LastAnnualTechnicalReviewDate = lastAnnualTechnicalReviewDate;
            trailer.Mileage = mileage;

            trailer.Technics = new Technics(CurrentUser);
            trailer.Technics.TechnicsId = trailer.TechnicsId;
            trailer.Technics.TechnicsType = TechnicsTypeUtil.GetTechnicsType("TRAILERS", CurrentUser);
            trailer.Technics.TechnicsCategoryId = technicsCategoryId.HasValue ? technicsCategoryId.Value : (int?)null;
            trailer.Technics.ItemsCount = 1;
            trailer.Technics.ResidencePostCode = residencePostCode;
            trailer.Technics.ResidenceCityId = residenceCityId;
            trailer.Technics.ResidenceDistrictId = residenceDistrictId;
            trailer.Technics.ResidenceAddress = residenceAddress;
            trailer.Technics.NormativeTechnicsId = normativeTechnicsId;


            string stat = "";
            string response = "";

            try
            {
                //Track the changes into the Audit Trail 
                Change change = new Change(CurrentUser, "RES_Technics_TRAILERS");

                TrailerUtil.SaveTrailer(trailer, CurrentUser, change);

                //Write into the Audit Trail
                change.WriteLog();

                stat = AJAXTools.OK;
                response = @"<response>OK</response>
                             <technicsId>" + AJAXTools.EncodeForXML(trailer.TechnicsId.ToString()) + @"</technicsId>
                             <trailerId>" + AJAXTools.EncodeForXML(trailer.TrailerId.ToString()) + @"</trailerId>";
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
                GetUIItemAccessLevel("RES_TECHNICS_TRAILERS") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string regNumber = Request.Params["RegNumber"];

            string stat = "";
            string response = "";

            try
            {
                int technicsId = 0;

                Trailer trailer = TrailerUtil.GetTrailerByRegNumber(regNumber, CurrentUser);

                if (trailer != null)
                {
                    technicsId = trailer.TechnicsId;
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

        //Refresh the list TrailerKind (ajax call)
        private void JSRefreshTrailerKindList()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_TRAILERS") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<trailerKind>";

                response += "<k>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</k>";

                List<GTableItem> trailerKinds = GTableItemUtil.GetAllGTableItemsByTableName("TrailerKind", ModuleKey, 1, 0, 0, CurrentUser);

                foreach (GTableItem trailerKind in trailerKinds)
                {
                    response += "<k>" +
                                "<id>" + trailerKind.TableKey.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(trailerKind.TableValue.ToString()) + "</name>" +
                                "</k>";
                }

                response += "</trailerKind>";

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

        //Refresh the list TrailerType (ajax call)
        private void JSRefreshTrailerTypeList()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_TRAILERS") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<trailerType>";

                response += "<t>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</t>";

                List<GTableItem> trailerTypes = GTableItemUtil.GetAllGTableItemsByTableName("TrailerType", ModuleKey, 1, 0, 0, CurrentUser);

                foreach (GTableItem trailerType in trailerTypes)
                {
                    response += "<t>" +
                                "<id>" + trailerType.TableKey.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(trailerType.TableValue.ToString()) + "</name>" +
                                "</t>";
                }

                response += "</trailerType>";

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

        //Refresh the list TrailerBodyKind (ajax call)
        private void JSRefreshTrailerBodyKindList()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_TRAILERS") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<trailerBodyKind>";

                response += "<k>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</k>";

                List<GTableItem> trailerBodyKinds = GTableItemUtil.GetAllGTableItemsByTableName("TrailerBodyKind", ModuleKey, 1, 0, 0, CurrentUser);

                foreach (GTableItem trailerBodyKind in trailerBodyKinds)
                {
                    response += "<k>" +
                                "<id>" + trailerBodyKind.TableKey.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(trailerBodyKind.TableValue.ToString()) + "</name>" +
                                "</k>";
                }

                response += "</trailerBodyKind>";

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
                GetUIItemAccessLevel("RES_TECHNICS_TRAILERS") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            int trailerId = int.Parse(Request.Params["TrailerId"]);
            string newRegNumber = Request.Params["NewRegNumber"];
            
            string stat = "";
            string response = "";

            try
            {
                Trailer existingTrailer = TrailerUtil.GetTrailerByRegNumber(newRegNumber, CurrentUser);

                if (existingTrailer == null)
                {
                    //Track the changes into the Audit Trail 
                    Change change = new Change(CurrentUser, "RES_Technics_TRAILERS");

                    TrailerUtil.ChangeRegNumber(trailerId, newRegNumber, CurrentUser, change);

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
                GetUIItemAccessLevel("RES_TECHNICS_TRAILERS") == UIAccessLevel.Hidden)
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

            List<TrailerRegNumber> trailerRegNumbers = new List<TrailerRegNumber>();
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

            int trailerId = 0;
            int.TryParse((Request.Params["TrailerId"]).ToString(), out trailerId);

            allRows = TrailerUtil.GetAllTrailerRegNumbersCount(trailerId, currentUser);
            maxPage = allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            trailerRegNumbers = TrailerUtil.GetAllTrailerRegNumbers(trailerId, orderBy, pageIndex, pageLength, currentUser);

            // No data found
            if (trailerRegNumbers.Count == 0)
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

                        <table id='tblTrailerRegNumberHistory' class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
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
            foreach (TrailerRegNumber trailerRegNumber in trailerRegNumbers)
            {

                string cellStyle = "vertical-align: top;";

                html += @"<tr  class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"' >
                                 <td style='" + cellStyle + @"'>" + ((pageIndex - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + trailerRegNumber.RegNumber + @"</td>
                              </tr>";

                counter++;
            }
            html += "</table>";

            html += "<input type='hidden' id='hdnTrailerRegNumberHistoryCounter' value='" + counter + "' />";


            html += @"<div style='height: 10px;'>
                </div>

                 <div style='text-align: center'>
                   <span id='lblTrailerRegNumberHistoryMessage'>" + htmlNoResults + @"</span><br/>
                </div>
";

            html += @"  </div>                        
                        <div id='btnCloseTrailerRegNumberHistoryTable' runat='server' class='Button' onclick=""HideRegNumberHistoryLightBox();""><i></i><div style='width:70px; padding-left:5px;'>Затвори</div><b></b></div>
                      </center>";



            return html;
        }
    }


    public static class AddEditTechnics_TRAILERS_PageUtil
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
                    <div id=""ChangeRegNumberLightBox"" class=""ChangeTrailerRegNumberLightBox"" style=""display: none; text-align: center;"">
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
                                 page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_ADD") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_ADD_BASICINFO") == UIAccessLevel.Disabled;

                screenHidden = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_ADD") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_ADD_BASICINFO") == UIAccessLevel.Hidden;

                basicInfoDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_ADD") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_ADD_BASICINFO") == UIAccessLevel.Disabled;

                basicInfoHidden = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_ADD") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_ADD_BASICINFO") == UIAccessLevel.Hidden;


                UIAccessLevel l;

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_ADD_BASICINFO_REGNUMBER");

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
                                  page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS") == UIAccessLevel.Disabled ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_EDIT") == UIAccessLevel.Disabled || isPreview;


                screenHidden = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_EDIT") == UIAccessLevel.Hidden;

                basicInfoDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_EDIT") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_EDIT_BASICINFO") == UIAccessLevel.Disabled;

                basicInfoHidden = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_EDIT") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_EDIT_BASICINFO") == UIAccessLevel.Hidden;

                UIAccessLevel l;

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_EDIT_BASICINFO_REGNUMBER");

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


            List<GTableItem> trailerKinds = GTableItemUtil.GetAllGTableItemsByTableName("TrailerKind", page.ModuleKey, 1, 0, 0, page.CurrentUser);
            List<IDropDownItem> ddiTrailerKinds = new List<IDropDownItem>();
            foreach (GTableItem trailerKind in trailerKinds)
            {
                ddiTrailerKinds.Add(trailerKind);
            }

            string trailerKindsHTML = ListItems.GetDropDownHtml(ddiTrailerKinds, null, "ddTrailerKind", true, null, "", "style='width: 300px;'", true);
            string editTrailerKindsHTML = @"<img id=""imgMaintTrailerKind"" alt=""Редактиране на списъка"" title=""Редактиране на списъка"" style=""cursor: pointer;"" src=""../Images/list_edit.png"" onclick=""ShowGTable('TrailerKind', 1, 1, RefreshTrailerKindList);"" />";


            List<GTableItem> trailerTypes = GTableItemUtil.GetAllGTableItemsByTableName("TrailerType", page.ModuleKey, 1, 0, 0, page.CurrentUser);
            List<IDropDownItem> ddiTrailerTypes = new List<IDropDownItem>();
            foreach (GTableItem trailerType in trailerTypes)
            {
                ddiTrailerTypes.Add(trailerType);
            }

            string trailerTypesHTML = ListItems.GetDropDownHtml(ddiTrailerTypes, null, "ddTrailerType", true, null, "", "style='width: 300px;'", true);
            string editTrailerTypesHTML = @"<img id=""imgMaintTrailerType"" alt=""Редактиране на списъка"" title=""Редактиране на списъка"" style=""cursor: pointer;"" src=""../Images/list_edit.png"" onclick=""ShowGTable('TrailerType', 1, 1, RefreshTrailerTypeList);"" />";


            List<GTableItem> trailerBodyKinds = GTableItemUtil.GetAllGTableItemsByTableName("TrailerBodyKind", page.ModuleKey, 1, 0, 0, page.CurrentUser);
            List<IDropDownItem> ddiTrailerBodyKinds = new List<IDropDownItem>();
            foreach (GTableItem trailerBodyKind in trailerBodyKinds)
            {
                ddiTrailerBodyKinds.Add(trailerBodyKind);
            }

            string trailerBodyKindsHTML = ListItems.GetDropDownHtml(ddiTrailerBodyKinds, null, "ddTrailerBodyKind", true, null, "", "style='width: 300px;'", true);
            string editTrailerBodyKindsHTML = @"<img id=""imgMaintTrailerBodyKind"" alt=""Редактиране на списъка"" title=""Редактиране на списъка"" style=""cursor: pointer;"" src=""../Images/list_edit.png"" onclick=""ShowGTable('TrailerBodyKind', 1, 1, RefreshTrailerBodyKindList);"" />";


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
            <span id=""lblTrailerKind"" class=""InputLabel"">Вид:</span>
         </td>
         <td style=""text-align: left;"">
            " + trailerKindsHTML + editTrailerKindsHTML + @"
         </td>
         <td style=""text-align: right;"">
            <span id=""lblTrailerType"" class=""InputLabel"">Тип:</span>
         </td>
         <td style=""text-align: left;"">
            " + trailerTypesHTML + editTrailerTypesHTML + @"
         </td>
      </tr>
      <tr>
         <td style=""text-align: right;"">
            <span id=""lblTrailerBodyKind"" class=""InputLabel"">Вид каросерия:</span>
         </td>
         <td style=""text-align: left;"">
            " + trailerBodyKindsHTML + editTrailerBodyKindsHTML + @"
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
            &nbsp;
         </td>
         <td style=""text-align: left;"">
            &nbsp;
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
            &nbsp;
         </td>
         <td style=""text-align: left;"">
            &nbsp;
         </td>
         <td style=""text-align: right;"">
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

<input type=""hidden"" id=""hdnTrailerId"" />";
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
                                 page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_ADD") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_ADD_BASICINFO") == UIAccessLevel.Disabled;

                basicInfoDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_ADD") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_ADD_BASICINFO") == UIAccessLevel.Disabled;

                if (screenDisabled)
                {
                    hiddenClientControls.Add("btnBasicOtherInfo");
                }

                if (basicInfoDisabled)
                {
                    hiddenClientControls.Add("btnBasicOtherInfo");
                }

                UIAccessLevel l;

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_ADD_BASICINFO_INVENTORYNUMBER");

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

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_ADD_BASICINFO_TECHNICSCATEGORY");

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

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_ADD_BASICINFO_TRAILERKIND");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblTrailerKind");
                    disabledClientControls.Add("ddTrailerKind");
                    hiddenClientControls.Add("imgMaintTrailerKind");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblTrailerKind");
                    hiddenClientControls.Add("ddTrailerKind");
                    hiddenClientControls.Add("imgMaintTrailerKind");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_ADD_BASICINFO_TRAILERTYPE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblTrailerType");
                    disabledClientControls.Add("ddTrailerType");
                    hiddenClientControls.Add("imgMaintTrailerType");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblTrailerType");
                    hiddenClientControls.Add("ddTrailerType");
                    hiddenClientControls.Add("imgMaintTrailerType");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_ADD_BASICINFO_TRAILERBODYKIND");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblTrailerBodyKind");
                    disabledClientControls.Add("ddTrailerBodyKind");
                    hiddenClientControls.Add("imgMaintTrailerBodyKind");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblTrailerBodyKind");
                    hiddenClientControls.Add("ddTrailerBodyKind");
                    hiddenClientControls.Add("imgMaintTrailerBodyKind");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_ADD_BASICINFO_CARRYINGCAPACITY");

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

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_ADD_BASICINFO_FIRSTREGDATE");

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

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_ADD_BASICINFO_LASTANNTECHREVIEWDATE");

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

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_ADD_BASICINFO_MILEAGE");

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
                                 page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_EDIT") == UIAccessLevel.Disabled || isPreview;

                basicInfoDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_EDIT") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_EDIT_BASICINFO") == UIAccessLevel.Disabled;

                if (screenDisabled)
                {
                    hiddenClientControls.Add("btnBasicOtherInfo");
                }

                if (basicInfoDisabled)
                {
                    hiddenClientControls.Add("btnBasicOtherInfo");
                }

                UIAccessLevel l;
                
                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_EDIT_BASICINFO_INVENTORYNUMBER");

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

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_EDIT_BASICINFO_TECHNICSCATEGORY");

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

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_EDIT_BASICINFO_TRAILERKIND");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblTrailerKind");
                    disabledClientControls.Add("ddTrailerKind");
                    hiddenClientControls.Add("imgMaintTrailerKind");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblTrailerKind");
                    hiddenClientControls.Add("ddTrailerKind");
                    hiddenClientControls.Add("imgMaintTrailerKind");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_EDIT_BASICINFO_TRAILERTYPE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblTrailerType");
                    disabledClientControls.Add("ddTrailerType");
                    hiddenClientControls.Add("imgMaintTrailerType");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblTrailerType");
                    hiddenClientControls.Add("ddTrailerType");
                    hiddenClientControls.Add("imgMaintTrailerType");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_EDIT_BASICINFO_TRAILERBODYKIND");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblTrailerBodyKind");
                    disabledClientControls.Add("ddTrailerBodyKind");
                    hiddenClientControls.Add("imgMaintTrailerBodyKind");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblTrailerBodyKind");
                    hiddenClientControls.Add("ddTrailerBodyKind");
                    hiddenClientControls.Add("imgMaintTrailerBodyKind");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_EDIT_BASICINFO_CARRYINGCAPACITY");

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

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_EDIT_BASICINFO_FIRSTREGDATE");

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

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_EDIT_BASICINFO_LASTANNTECHREVIEWDATE");

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

                l = page.GetUIItemAccessLevel("RES_TECHNICS_TRAILERS_EDIT_BASICINFO_MILEAGE");

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
                page.GetUIItemAccessLevel("RES_LISTMAINT_TRAILERKIND") != UIAccessLevel.Enabled)
            {
                hiddenClientControls.Add("imgMaintTrailerKind");
            }

            if (page.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled ||
                page.GetUIItemAccessLevel("RES_LISTMAINT_TRAILERTYPE") != UIAccessLevel.Enabled)
            {
                hiddenClientControls.Add("imgMaintTrailerType");
            }

            if (page.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled ||
                page.GetUIItemAccessLevel("RES_LISTMAINT_TRAILERBODYKIND") != UIAccessLevel.Enabled)
            {
                hiddenClientControls.Add("imgMaintTrailerBodyKind");
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
