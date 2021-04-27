using System;
using System.Collections.Generic;
using System.Text;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class AddEditTechnics_FUEL_CONTAINERS : RESPage
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
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSCheckInvNumber")
            {
                JSCheckInvNumber();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRefreshFuelContainerKindList")
            {
                JSRefreshFuelContainerKindList();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRefreshFuelContainerTypeList")
            {
                JSRefreshFuelContainerTypeList();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSChangeInvNumber")
            {
                JSChangeInvNumber();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadInvNumberHistory")
            {
                JSLoadInvNumberHistory();
                return;
            }
        }

        //Load Basic Info (ajax call)
        private void JSLoadBasicInfo()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            int technicsId = 0;
            if (!String.IsNullOrEmpty(Request.Form["TechnicsId"]))
                technicsId = int.Parse(Request.Form["TechnicsId"]);

            string stat = "";
            string response = "";

            try
            {
                FuelContainer fuelContainer = FuelContainerUtil.GetFuelContainerByTechnicsId(technicsId, CurrentUser);
                TechnicsMilRepStatus currentMilRepStatus = TechnicsMilRepStatusUtil.GetTechnicsMilRepCurrentStatusByTechnicsId(fuelContainer.TechnicsId, CurrentUser);
                string currMilRepStatusName = (currentMilRepStatus != null ? currentMilRepStatus.TechMilitaryReportStatus.TechMilitaryReportStatusName : TechMilitaryReportStatusUtil.GetLabelWhenLackOfStatus());
               
                stat = AJAXTools.OK;

                response = @"
                    <fuelContainer>
                         <technicsId>" + AJAXTools.EncodeForXML(fuelContainer.TechnicsId.ToString()) + @"</technicsId>
                         <fuelContainerId>" + AJAXTools.EncodeForXML(fuelContainer.FuelContainerId.ToString()) + @"</fuelContainerId>
                         <inventoryNumber>" + AJAXTools.EncodeForXML(fuelContainer.InventoryNumber) + @"</inventoryNumber>
                         <technicsCategoryId>" + AJAXTools.EncodeForXML(fuelContainer.Technics.TechnicsCategoryId.HasValue ? fuelContainer.Technics.TechnicsCategoryId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</technicsCategoryId>
                         <fuelContainerKindId>" + AJAXTools.EncodeForXML(fuelContainer.FuelContainerKindId.HasValue ? fuelContainer.FuelContainerKindId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</fuelContainerKindId>
                         <fuelContainerTypeId>" + AJAXTools.EncodeForXML(fuelContainer.FuelContainerTypeId.HasValue ? fuelContainer.FuelContainerTypeId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</fuelContainerTypeId>
                         <fuelContainerCount>" + AJAXTools.EncodeForXML(fuelContainer.Technics.ItemsCount.ToString()) + @"</fuelContainerCount>
                         <lastModified>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDateTime(fuelContainer.Technics.LastModifiedDate)) + @"</lastModified>
                         <resMilRepStatus>" + AJAXTools.EncodeForXML(currMilRepStatusName) + @"</resMilRepStatus>
                         <residenceCityId>" + AJAXTools.EncodeForXML(fuelContainer.Technics.ResidenceCityId != null ? fuelContainer.Technics.ResidenceCityId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</residenceCityId>
                         <residencePostCode>" + AJAXTools.EncodeForXML(fuelContainer.Technics.ResidencePostCode) + @"</residencePostCode>
                         <residenceRegionId>" + AJAXTools.EncodeForXML(fuelContainer.Technics.ResidenceCityId != null ? fuelContainer.Technics.ResidenceCity.RegionId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</residenceRegionId>
                         <residenceMunicipalityId>" + AJAXTools.EncodeForXML(fuelContainer.Technics.ResidenceCityId != null ? fuelContainer.Technics.ResidenceCity.MunicipalityId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</residenceMunicipalityId>
                         <residenceDistrictId>" + AJAXTools.EncodeForXML(fuelContainer.Technics.ResidenceDistrictId != null ? fuelContainer.Technics.ResidenceDistrict.DistrictId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</residenceDistrictId>
                         <residenceAddress>" + AJAXTools.EncodeForXML(fuelContainer.Technics.ResidenceAddress) + @"</residenceAddress>
                         <currMilDepartment>" + AJAXTools.EncodeForXML(fuelContainer.Technics.CurrTechMilRepStatus != null && fuelContainer.Technics.CurrTechMilRepStatus.SourceMilDepartment != null ? fuelContainer.Technics.CurrTechMilRepStatus.SourceMilDepartment.MilitaryDepartmentName : TechMilitaryReportStatusUtil.GetLabelWhenLackOfStatus()) + @"</currMilDepartment>
                         <normativeTechnicsId>" + AJAXTools.EncodeForXML(fuelContainer.Technics.NormativeTechnicsId != null ? fuelContainer.Technics.NormativeTechnics.NormativeTechnicsId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</normativeTechnicsId>
                         <normativeCode>" + AJAXTools.EncodeForXML(fuelContainer.Technics.NormativeTechnicsId != null ? fuelContainer.Technics.NormativeTechnics.NormativeCode : "") + @"</normativeCode>
                    </fuelContainer>";

                if (fuelContainer.Technics.ResidenceCityId != null)
                {
                    List<Municipality> municipalities = MunicipalityUtil.GetMunicipalities(fuelContainer.Technics.ResidenceCity.RegionId, CurrentUser);
                    List<City> cities = CityUtil.GetCities(fuelContainer.Technics.ResidenceCity.MunicipalityId, CurrentUser);
                    List<District> districts = fuelContainer.Technics.ResidenceCity.Districts;

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
                GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            int? technicsId = null;
            if (!String.IsNullOrEmpty(Request.Params["TechnicsId"]))
            {
                technicsId = int.Parse(Request.Params["TechnicsId"]);
            }

            int? fuelContainerId = null;
            if (!String.IsNullOrEmpty(Request.Params["FuelContainerId"]))
            {
                fuelContainerId = int.Parse(Request.Params["FuelContainerId"]);
            }

            string inventoryNumber = Request.Params["InventoryNumber"];
            
            int? technicsCategoryId = null;
            if (!String.IsNullOrEmpty(Request.Params["TechnicsCategoryId"]) &&
                Request.Params["TechnicsCategoryId"] != ListItems.GetOptionChooseOne().Value)
            {
                technicsCategoryId = int.Parse(Request.Params["TechnicsCategoryId"]);
            }

            int? fuelContainerKindId = null;
            if (!String.IsNullOrEmpty(Request.Params["FuelContainerKindId"]) &&
                Request.Params["FuelContainerKindId"] != ListItems.GetOptionChooseOne().Value)
            {
                fuelContainerKindId = int.Parse(Request.Params["FuelContainerKindId"]);
            }

            int? fuelContainerTypeId = null;
            if (!String.IsNullOrEmpty(Request.Params["FuelContainerTypeId"]) &&
                Request.Params["FuelContainerTypeId"] != ListItems.GetOptionChooseOne().Value)
            {
                fuelContainerTypeId = int.Parse(Request.Params["FuelContainerTypeId"]);
            }

            int fuelContainerCount = 1;
            if (!String.IsNullOrEmpty(Request.Params["FuelContainerCount"]))
            {
                fuelContainerCount = int.Parse(Request.Params["FuelContainerCount"]);
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

            FuelContainer fuelContainer = new FuelContainer(CurrentUser);

            fuelContainer.FuelContainerId = fuelContainerId.HasValue ? fuelContainerId.Value : 0;
            fuelContainer.TechnicsId = technicsId.HasValue ? technicsId.Value : 0;
            fuelContainer.InventoryNumber = inventoryNumber;
            fuelContainer.FuelContainerKindId = fuelContainerKindId;
            fuelContainer.FuelContainerTypeId = fuelContainerTypeId;

            fuelContainer.Technics = new Technics(CurrentUser);
            fuelContainer.Technics.TechnicsId = fuelContainer.TechnicsId;
            fuelContainer.Technics.TechnicsType = TechnicsTypeUtil.GetTechnicsType("FUEL_CONTAINERS", CurrentUser);
            fuelContainer.Technics.TechnicsCategoryId = technicsCategoryId.HasValue ? technicsCategoryId.Value : (int?)null;
            fuelContainer.Technics.ItemsCount = fuelContainerCount;
            fuelContainer.Technics.ResidencePostCode = residencePostCode;
            fuelContainer.Technics.ResidenceCityId = residenceCityId;
            fuelContainer.Technics.ResidenceDistrictId = residenceDistrictId;
            fuelContainer.Technics.ResidenceAddress = residenceAddress;
            fuelContainer.Technics.NormativeTechnicsId = normativeTechnicsId;


            string stat = "";
            string response = "";

            try
            {
                //Track the changes into the Audit Trail 
                Change change = new Change(CurrentUser, "RES_Technics_FUEL_CONTAINERS");

                FuelContainerUtil.SaveFuelContainer(fuelContainer, CurrentUser, change);

                //Write into the Audit Trail
                change.WriteLog();

                stat = AJAXTools.OK;
                response = @"<response>OK</response>
                             <technicsId>" + AJAXTools.EncodeForXML(fuelContainer.TechnicsId.ToString()) + @"</technicsId>
                             <fuelContainerId>" + AJAXTools.EncodeForXML(fuelContainer.FuelContainerId.ToString()) + @"</fuelContainerId>";
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

        //Check inventory Number(ajax call)
        private void JSCheckInvNumber()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string invNumber = Request.Params["InventoryNumber"];

            string stat = "";
            string response = "";

            try
            {
                int technicsId = 0;

                FuelContainer fuelContainer = FuelContainerUtil.GetFuelContainerByInvNumber(invNumber, CurrentUser);

                if (fuelContainer != null)
                {
                    technicsId = fuelContainer.TechnicsId;
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

        //Refresh the list FuelContainerKind (ajax call)
        private void JSRefreshFuelContainerKindList()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<fuelContainerKind>";

                response += "<k>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</k>";

                List<GTableItem> fuelContainerKinds = GTableItemUtil.GetAllGTableItemsByTableName("FuelContainerKind", ModuleKey, 1, 0, 0, CurrentUser);

                foreach (GTableItem fuelContainerKind in fuelContainerKinds)
                {
                    response += "<k>" +
                                "<id>" + fuelContainerKind.TableKey.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(fuelContainerKind.TableValue.ToString()) + "</name>" +
                                "</k>";
                }

                response += "</fuelContainerKind>";

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

        //Refresh the list FuelContainerType (ajax call)
        private void JSRefreshFuelContainerTypeList()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<fuelContainerType>";

                response += "<t>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</t>";

                List<GTableItem> fuelContainerTypes = GTableItemUtil.GetAllGTableItemsByTableName("FuelContainerType", ModuleKey, 1, 0, 0, CurrentUser);

                foreach (GTableItem fuelContainerType in fuelContainerTypes)
                {
                    response += "<t>" +
                                "<id>" + fuelContainerType.TableKey.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(fuelContainerType.TableValue.ToString()) + "</name>" +
                                "</t>";
                }

                response += "</fuelContainerType>";

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

        //Change the inventory number (ajax call)
        private void JSChangeInvNumber()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            int fuelContainerId = int.Parse(Request.Params["FuelContainerId"]);
            string newInvNumber = Request.Params["NewInventoryNumber"];
            
            string stat = "";
            string response = "";

            try
            {
                FuelContainer existingFuelContainer = FuelContainerUtil.GetFuelContainerByInvNumber(newInvNumber, CurrentUser);

                if (existingFuelContainer == null)
                {
                    //Track the changes into the Audit Trail 
                    Change change = new Change(CurrentUser, "RES_Technics_FUEL_CONTAINERS");

                    FuelContainerUtil.ChangeInventoryNumber(fuelContainerId, newInvNumber, CurrentUser, change);

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

        private void JSLoadInvNumberHistory()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS") == UIAccessLevel.Hidden)
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

            List<FuelContainerInvNumber> fuelContainerInvNumbers = new List<FuelContainerInvNumber>();
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

            int fuelContainerId = 0;
            int.TryParse((Request.Params["FuelContainerId"]).ToString(), out fuelContainerId);

            allRows = FuelContainerUtil.GetAllFuelContainerInvNumbersCount(fuelContainerId, currentUser);
            maxPage = allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            fuelContainerInvNumbers = FuelContainerUtil.GetAllFuelContainerInvNumbers(fuelContainerId, orderBy, pageIndex, pageLength, currentUser);

            // No data found
            if (fuelContainerInvNumbers.Count == 0)
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

                        <table id='tblFuelContainerInvNumberHistory' class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
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
            foreach (FuelContainerInvNumber fuelContainerInvNumber in fuelContainerInvNumbers)
            {

                string cellStyle = "vertical-align: top;";

                html += @"<tr  class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"' >
                                 <td style='" + cellStyle + @"'>" + ((pageIndex - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + fuelContainerInvNumber.InventoryNumber + @"</td>
                              </tr>";

                counter++;
            }
            html += "</table>";

            html += "<input type='hidden' id='hdnFuelContainerInvNumberHistoryCounter' value='" + counter + "' />";


            html += @"<div style='height: 10px;'>
                </div>

                 <div style='text-align: center'>
                   <span id='lblFuelContainerInvNumberHistoryMessage'>" + htmlNoResults + @"</span><br/>
                </div>
";

            html += @"  </div>                        
                        <div id='btnCloseFuelContainerInvNumberHistoryTable' runat='server' class='Button' onclick=""HideInvNumberHistoryLightBox();""><i></i><div style='width:70px; padding-left:5px;'>Затвори</div><b></b></div>
                      </center>";



            return html;
        }
    }


    public static class AddEditTechnics_FUEL_CONTAINERS_PageUtil
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
                    <div id=""ChangeInvNumberLightBox"" class=""ChangeFuelContainerInvNumberLightBox"" style=""display: none; text-align: center;"">
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
                                        <input type=""text"" id=""txtNewInventoryNumber"" onblur=""NewInventoryNumberBlur();"" class=""RequiredInputField"" style=""width: 90px;"" maxlength=""20"" UnsavedCheckSkipMe=""true"" />
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
                                                        <div id=""btnChangeInvNumberLightBoxText"" style=""width: 70px;"">
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
                                 page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS_ADD") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS_ADD_BASICINFO") == UIAccessLevel.Disabled;

                screenHidden = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS_ADD") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS_ADD_BASICINFO") == UIAccessLevel.Hidden;

                basicInfoDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS_ADD") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS_ADD_BASICINFO") == UIAccessLevel.Disabled;

                basicInfoHidden = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS_ADD") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS_ADD_BASICINFO") == UIAccessLevel.Hidden;


                UIAccessLevel l;

                l = page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS_ADD_BASICINFO_INVENTORYNUMBER");

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
                                 page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS_EDIT") == UIAccessLevel.Disabled || isPreview;


                screenHidden = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS_EDIT") == UIAccessLevel.Hidden;

                basicInfoDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS_EDIT") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS_EDIT_BASICINFO") == UIAccessLevel.Disabled;

                basicInfoHidden = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS_EDIT") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS_EDIT_BASICINFO") == UIAccessLevel.Hidden;


                UIAccessLevel l;

                l = page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS_EDIT_BASICINFO_INVENTORYNUMBER");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblInventoryNumber");
                    disabledClientControls.Add("txtInventoryNumber");
                    hiddenClientControls.Add("imgEditInvNumberCont");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblInventoryNumber");
                    hiddenClientControls.Add("txtInventoryNumber");

                    hiddenClientControls.Add("lblInventoryNuber");
                    hiddenClientControls.Add("txtInventoryNumberCont");
                    hiddenClientControls.Add("lblRegNumberValueCont");
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


            List<GTableItem> fuelContainerKinds = GTableItemUtil.GetAllGTableItemsByTableName("FuelContainerKind", page.ModuleKey, 1, 0, 0, page.CurrentUser);
            List<IDropDownItem> ddiFuelContainerKinds = new List<IDropDownItem>();
            foreach (GTableItem fuelContainerKind in fuelContainerKinds)
            {
                ddiFuelContainerKinds.Add(fuelContainerKind);
            }

            string fuelContainerKindsHTML = ListItems.GetDropDownHtml(ddiFuelContainerKinds, null, "ddFuelContainerKind", true, null, "", "style='width: 270px;'", true);
            string editFuelContainerKindsHTML = @"<img id=""imgMaintFuelContainerKind"" alt=""Редактиране на списъка"" title=""Редактиране на списъка"" style=""cursor: pointer;"" src=""../Images/list_edit.png"" onclick=""ShowGTable('FuelContainerKind', 1, 1, RefreshFuelContainerKindList);"" />";


            List<GTableItem> fuelContainerTypes = GTableItemUtil.GetAllGTableItemsByTableName("FuelContainerType", page.ModuleKey, 1, 0, 0, page.CurrentUser);
            List<IDropDownItem> ddiFuelContainerTypes = new List<IDropDownItem>();
            foreach (GTableItem fuelContainerType in fuelContainerTypes)
            {
                ddiFuelContainerTypes.Add(fuelContainerType);
            }

            string fuelContainerTypesHTML = ListItems.GetDropDownHtml(ddiFuelContainerTypes, null, "ddFuelContainerType", true, null, "", "style='width: 270px;'", true);
            string editFuelContainerTypesHTML = @"<img id=""imgMaintFuelContainerType"" alt=""Редактиране на списъка"" title=""Редактиране на списъка"" style=""cursor: pointer;"" src=""../Images/list_edit.png"" onclick=""ShowGTable('FuelContainerType', 1, 1, RefreshFuelContainerTypeList);"" />";


            string html = @"
<div style=""height: 10px;""></div>

<fieldset style=""width: 830px; padding: 0px;"">
   <table class=""InputRegion"" style=""width: 830px; padding: 10px; padding-top: 0px; margin-top: 0px; padding-left: 0px;"">
      <tr style=""height: 3px;"">
      </tr>
      <tr>
         <td style=""text-align: right; width: 140px;"">
            &nbsp;
         </td>
         <td style=""text-align: left; width: 110px;"">
            &nbsp;
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
            <span id=""lblFuelContainerKind"" class=""InputLabel"">Вид:</span>
         </td>
         <td style=""text-align: left;"">
            " + fuelContainerKindsHTML + editFuelContainerKindsHTML + @"
         </td>
         <td style=""text-align: right;"">
            <span id=""lblFuelContainerType"" class=""InputLabel"">Тип:</span>
         </td>
         <td style=""text-align: left;"">
            " + fuelContainerTypesHTML + editFuelContainerTypesHTML + @"
         </td>
         <td style=""text-align: right;"">
            <span id=""lblFuelContainerCount"" class=""InputLabel"">Брой:</span>
         </td>
         <td style=""text-align: left;"">
            <input type=""text"" id=""txtFuelContainerCount"" class=""RequiredInputField"" style=""width: 90px;"" maxlength=""20"" />
         </td>
      </tr>
   </table>
</fieldset>

<div style=""height: 10px;""></div>

" + AddEditTechnics_BasicInfo_PageUtil.GetBasicInfoResidenceContent(page) + @"

<div style=""height: 10px;""></div>

<input type=""hidden"" id=""hdnFuelContainerId"" />

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
                                 page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS_ADD") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS_ADD_BASICINFO") == UIAccessLevel.Disabled;

                basicInfoDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS_ADD") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS_ADD_BASICINFO") == UIAccessLevel.Disabled;
                               
                UIAccessLevel l;

                l = page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS_ADD_BASICINFO_TECHNICSCATEGORY");

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

                l = page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS_ADD_BASICINFO_FUELCONTAINERKIND");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblFuelContainerKind");
                    disabledClientControls.Add("ddFuelContainerKind");
                    hiddenClientControls.Add("imgMaintFuelContainerKind");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblFuelContainerKind");
                    hiddenClientControls.Add("ddFuelContainerKind");
                    hiddenClientControls.Add("imgMaintFuelContainerKind");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS_ADD_BASICINFO_FUELCONTAINERTYPE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblFuelContainerType");
                    disabledClientControls.Add("ddFuelContainerType");
                    hiddenClientControls.Add("imgMaintFuelContainerType");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblFuelContainerType");
                    hiddenClientControls.Add("ddFuelContainerType");
                    hiddenClientControls.Add("imgMaintFuelContainerType");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS_ADD_BASICINFO_FUELCONTAINERCOUNT");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblFuelContainerCount");
                    disabledClientControls.Add("txtFuelContainerCount");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblFuelContainerCount");
                    hiddenClientControls.Add("txtFuelContainerCount");
                }

                AddEditTechnics_BasicInfo_PageUtil.GetBasicInfoResidenceUIItems(page, true,
                    ref disabledClientControls, ref hiddenClientControls);
             }
            else // edit mode of page
            {
                screenDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS_EDIT") == UIAccessLevel.Disabled || isPreview;

                basicInfoDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS_EDIT") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS_EDIT_BASICINFO") == UIAccessLevel.Disabled;
                               
                UIAccessLevel l;
                
                l = page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS_EDIT_BASICINFO_TECHNICSCATEGORY");

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

                l = page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS_EDIT_BASICINFO_FUELCONTAINERKIND");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblFuelContainerKind");
                    disabledClientControls.Add("ddFuelContainerKind");
                    hiddenClientControls.Add("imgMaintFuelContainerKind");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblFuelContainerKind");
                    hiddenClientControls.Add("ddFuelContainerKind");
                    hiddenClientControls.Add("imgMaintFuelContainerKind");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS_EDIT_BASICINFO_FUELCONTAINERTYPE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblFuelContainerType");
                    disabledClientControls.Add("ddFuelContainerType");
                    hiddenClientControls.Add("imgMaintFuelContainerType");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblFuelContainerType");
                    hiddenClientControls.Add("ddFuelContainerType");
                    hiddenClientControls.Add("imgMaintFuelContainerType");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_FUEL_CONTAINERS_EDIT_BASICINFO_FUELCONTAINERCOUNT");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblFuelContainerCount");
                    disabledClientControls.Add("txtFuelContainerCount");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblFuelContainerCount");
                    hiddenClientControls.Add("txtFuelContainerCount");
                }

                AddEditTechnics_BasicInfo_PageUtil.GetBasicInfoResidenceUIItems(page, false,
                    ref disabledClientControls, ref hiddenClientControls);
            }

            if (page.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled ||
                page.GetUIItemAccessLevel("RES_LISTMAINT_FUELCONTAINERKIND") != UIAccessLevel.Enabled)
            {
                hiddenClientControls.Add("imgMaintFuelContainerKind");
            }

            if (page.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled ||
                page.GetUIItemAccessLevel("RES_LISTMAINT_FUELCONTAINERTYPE") != UIAccessLevel.Enabled)
            {
                hiddenClientControls.Add("imgMaintFuelContainerType");
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
