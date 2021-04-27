using System;
using System.Collections.Generic;
using System.Text;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class AddEditTechnics_MOB_LIFT_EQUIP : RESPage
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
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRefreshMobileLiftingEquipKindList")
            {
                JSRefreshMobileLiftingEquipKindList();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRefreshMobileLiftingEquipTypeList")
            {
                JSRefreshMobileLiftingEquipTypeList();
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
                GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            int technicsId = 0;
            if (!String.IsNullOrEmpty(Request.Form["TechnicsId"]))
                technicsId = int.Parse(Request.Form["TechnicsId"]);

            string stat = "";
            string response = "";

            try
            {
                MobileLiftingEquip mobileLiftingEquip = MobileLiftingEquipUtil.GetMobileLiftingEquipByTechnicsId(technicsId, CurrentUser);
                TechnicsMilRepStatus currentMilRepStatus = TechnicsMilRepStatusUtil.GetTechnicsMilRepCurrentStatusByTechnicsId(mobileLiftingEquip.TechnicsId, CurrentUser);
                string currMilRepStatusName = (currentMilRepStatus != null ? currentMilRepStatus.TechMilitaryReportStatus.TechMilitaryReportStatusName : TechMilitaryReportStatusUtil.GetLabelWhenLackOfStatus());
            
                stat = AJAXTools.OK;

                response = @"
                    <mobileLiftingEquip>
                         <technicsId>" + AJAXTools.EncodeForXML(mobileLiftingEquip.TechnicsId.ToString()) + @"</technicsId>
                         <mobileLiftingEquipId>" + AJAXTools.EncodeForXML(mobileLiftingEquip.MobileLiftingEquipId.ToString()) + @"</mobileLiftingEquipId>
                         <regNumber>" + AJAXTools.EncodeForXML(mobileLiftingEquip.RegNumber) + @"</regNumber>
                         <inventoryNumber>" + AJAXTools.EncodeForXML(mobileLiftingEquip.InventoryNumber) + @"</inventoryNumber>
                         <technicsCategoryId>" + AJAXTools.EncodeForXML(mobileLiftingEquip.Technics.TechnicsCategoryId.HasValue ? mobileLiftingEquip.Technics.TechnicsCategoryId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</technicsCategoryId>
                         <lastModified>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDateTime(mobileLiftingEquip.Technics.LastModifiedDate)) + @"</lastModified>
                         <resMilRepStatus>" + AJAXTools.EncodeForXML(currMilRepStatusName) + @"</resMilRepStatus>
                         <mobileLiftingEquipKindId>" + AJAXTools.EncodeForXML(mobileLiftingEquip.MobileLiftingEquipKindId.HasValue ? mobileLiftingEquip.MobileLiftingEquipKindId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</mobileLiftingEquipKindId>
                         <mobileLiftingEquipTypeId>" + AJAXTools.EncodeForXML(mobileLiftingEquip.MobileLiftingEquipTypeId.HasValue ? mobileLiftingEquip.MobileLiftingEquipTypeId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</mobileLiftingEquipTypeId>
                         <loadingCapacity>" + AJAXTools.EncodeForXML(mobileLiftingEquip.LoadingCapacity.HasValue ? mobileLiftingEquip.LoadingCapacity.ToString() : "") + @"</loadingCapacity>                         
                         <residenceCityId>" + AJAXTools.EncodeForXML(mobileLiftingEquip.Technics.ResidenceCityId != null ? mobileLiftingEquip.Technics.ResidenceCityId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</residenceCityId>
                         <residencePostCode>" + AJAXTools.EncodeForXML(mobileLiftingEquip.Technics.ResidencePostCode) + @"</residencePostCode>
                         <residenceRegionId>" + AJAXTools.EncodeForXML(mobileLiftingEquip.Technics.ResidenceCityId != null ? mobileLiftingEquip.Technics.ResidenceCity.RegionId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</residenceRegionId>
                         <residenceMunicipalityId>" + AJAXTools.EncodeForXML(mobileLiftingEquip.Technics.ResidenceCityId != null ? mobileLiftingEquip.Technics.ResidenceCity.MunicipalityId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</residenceMunicipalityId>
                         <residenceDistrictId>" + AJAXTools.EncodeForXML(mobileLiftingEquip.Technics.ResidenceDistrictId != null ? mobileLiftingEquip.Technics.ResidenceDistrict.DistrictId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</residenceDistrictId>
                         <residenceAddress>" + AJAXTools.EncodeForXML(mobileLiftingEquip.Technics.ResidenceAddress) + @"</residenceAddress>
                         <currMilDepartment>" + AJAXTools.EncodeForXML(mobileLiftingEquip.Technics.CurrTechMilRepStatus != null && mobileLiftingEquip.Technics.CurrTechMilRepStatus.SourceMilDepartment != null ? mobileLiftingEquip.Technics.CurrTechMilRepStatus.SourceMilDepartment.MilitaryDepartmentName : TechMilitaryReportStatusUtil.GetLabelWhenLackOfStatus()) + @"</currMilDepartment>
                         <normativeTechnicsId>" + AJAXTools.EncodeForXML(mobileLiftingEquip.Technics.NormativeTechnicsId != null ? mobileLiftingEquip.Technics.NormativeTechnics.NormativeTechnicsId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</normativeTechnicsId>
                         <normativeCode>" + AJAXTools.EncodeForXML(mobileLiftingEquip.Technics.NormativeTechnicsId != null ? mobileLiftingEquip.Technics.NormativeTechnics.NormativeCode : "") + @"</normativeCode>
                    </mobileLiftingEquip>";               

                if (mobileLiftingEquip.Technics.ResidenceCityId != null)
                {
                    List<Municipality> municipalities = MunicipalityUtil.GetMunicipalities(mobileLiftingEquip.Technics.ResidenceCity.RegionId, CurrentUser);
                    List<City> cities = CityUtil.GetCities(mobileLiftingEquip.Technics.ResidenceCity.MunicipalityId, CurrentUser);
                    List<District> districts = mobileLiftingEquip.Technics.ResidenceCity.Districts;

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
                GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            int? technicsId = null;
            if (!String.IsNullOrEmpty(Request.Params["TechnicsId"]))
            {
                technicsId = int.Parse(Request.Params["TechnicsId"]);
            }

            int? mobileLiftingEquipId = null;
            if (!String.IsNullOrEmpty(Request.Params["MobileLiftingEquipId"]))
            {
                mobileLiftingEquipId = int.Parse(Request.Params["MobileLiftingEquipId"]);
            }

            string regNumber = Request.Params["RegNumber"];
            string inventoryNumber = Request.Params["InventoryNumber"];
            
            int? technicsCategoryId = null;
            if (!String.IsNullOrEmpty(Request.Params["TechnicsCategoryId"]) &&
                Request.Params["TechnicsCategoryId"] != ListItems.GetOptionChooseOne().Value)
            {
                technicsCategoryId = int.Parse(Request.Params["TechnicsCategoryId"]);
            }          

            int? mobileLiftingEquipKindId = null;
            if (!String.IsNullOrEmpty(Request.Params["MobileLiftingEquipKindId"]) &&
                Request.Params["MobileLiftingEquipKindId"] != ListItems.GetOptionChooseOne().Value)
            {
                mobileLiftingEquipKindId = int.Parse(Request.Params["MobileLiftingEquipKindId"]);
            }

            int? mobileLiftingEquipTypeId = null;
            if (!String.IsNullOrEmpty(Request.Params["MobileLiftingEquipTypeId"]) &&
                Request.Params["MobileLiftingEquipTypeId"] != ListItems.GetOptionChooseOne().Value)
            {
                mobileLiftingEquipTypeId = int.Parse(Request.Params["MobileLiftingEquipTypeId"]);
            }

            decimal? loadingCapacity = null;
            if (!String.IsNullOrEmpty(Request.Params["LoadingCapacity"]))
            {
                loadingCapacity = decimal.Parse(Request.Params["LoadingCapacity"]);
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

            MobileLiftingEquip mobileLiftingEquip = new MobileLiftingEquip(CurrentUser);

            mobileLiftingEquip.MobileLiftingEquipId = mobileLiftingEquipId.HasValue ? mobileLiftingEquipId.Value : 0;
            mobileLiftingEquip.TechnicsId = technicsId.HasValue ? technicsId.Value : 0;
            mobileLiftingEquip.RegNumber = regNumber;
            mobileLiftingEquip.InventoryNumber = inventoryNumber;           
            mobileLiftingEquip.MobileLiftingEquipKindId = mobileLiftingEquipKindId;
            mobileLiftingEquip.MobileLiftingEquipTypeId = mobileLiftingEquipTypeId;
            mobileLiftingEquip.LoadingCapacity = loadingCapacity;
            

            mobileLiftingEquip.Technics = new Technics(CurrentUser);
            mobileLiftingEquip.Technics.TechnicsId = mobileLiftingEquip.TechnicsId;
            mobileLiftingEquip.Technics.TechnicsType = TechnicsTypeUtil.GetTechnicsType("MOB_LIFT_EQUIP", CurrentUser);
            mobileLiftingEquip.Technics.TechnicsCategoryId = technicsCategoryId.HasValue ? technicsCategoryId.Value : (int?)null;
            mobileLiftingEquip.Technics.ItemsCount = 1;
            mobileLiftingEquip.Technics.ResidencePostCode = residencePostCode;
            mobileLiftingEquip.Technics.ResidenceCityId = residenceCityId;
            mobileLiftingEquip.Technics.ResidenceDistrictId = residenceDistrictId;
            mobileLiftingEquip.Technics.ResidenceAddress = residenceAddress;
            mobileLiftingEquip.Technics.NormativeTechnicsId = normativeTechnicsId;


            string stat = "";
            string response = "";

            try
            {
                //Track the changes into the Audit Trail 
                Change change = new Change(CurrentUser, "RES_Technics_MOB_LIFT_EQUIP");

                MobileLiftingEquipUtil.SaveMobileLiftingEquip(mobileLiftingEquip, CurrentUser, change);

                //Write into the Audit Trail
                change.WriteLog();

                stat = AJAXTools.OK;
                response = @"<response>OK</response>
                             <technicsId>" + AJAXTools.EncodeForXML(mobileLiftingEquip.TechnicsId.ToString()) + @"</technicsId>
                             <mobileLiftingEquipId>" + AJAXTools.EncodeForXML(mobileLiftingEquip.MobileLiftingEquipId.ToString()) + @"</mobileLiftingEquipId>";
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
                GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string regNumber = Request.Params["RegNumber"];

            string stat = "";
            string response = "";

            try
            {
                int technicsId = 0;

                MobileLiftingEquip mobileLiftingEquip = MobileLiftingEquipUtil.GetMobileLiftingEquipByRegNumber(regNumber, CurrentUser);

                if (mobileLiftingEquip != null)
                {
                    technicsId = mobileLiftingEquip.TechnicsId;
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

        //Refresh the list MobileLiftingEquipKind (ajax call)
        private void JSRefreshMobileLiftingEquipKindList()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<mobileLiftingEquipKind>";

                response += "<k>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</k>";

                List<GTableItem> mobileLiftingEquipKinds = GTableItemUtil.GetAllGTableItemsByTableName("MobileLiftingEquipKind", ModuleKey, 1, 0, 0, CurrentUser);

                foreach (GTableItem mobileLiftingEquipKind in mobileLiftingEquipKinds)
                {
                    response += "<k>" +
                                "<id>" + mobileLiftingEquipKind.TableKey.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(mobileLiftingEquipKind.TableValue.ToString()) + "</name>" +
                                "</k>";
                }

                response += "</mobileLiftingEquipKind>";

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

        //Refresh the list MobileLiftingEquipType (ajax call)
        private void JSRefreshMobileLiftingEquipTypeList()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<mobileLiftingEquipType>";

                response += "<r>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</r>";

                List<GTableItem> mobileLiftingEquipTypes = GTableItemUtil.GetAllGTableItemsByTableName("MobileLiftingEquipType", ModuleKey, 1, 0, 0, CurrentUser);

                foreach (GTableItem mobileLiftingEquipType in mobileLiftingEquipTypes)
                {
                    response += "<r>" +
                                "<id>" + mobileLiftingEquipType.TableKey.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(mobileLiftingEquipType.TableValue.ToString()) + "</name>" +
                                "</r>";
                }

                response += "</mobileLiftingEquipType>";

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
                GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            int mobileLiftingEquipId = int.Parse(Request.Params["MobileLiftingEquipId"]);
            string newRegNumber = Request.Params["NewRegNumber"];
            
            string stat = "";
            string response = "";

            try
            {
                MobileLiftingEquip existingMobileLiftingEquip = MobileLiftingEquipUtil.GetMobileLiftingEquipByRegNumber(newRegNumber, CurrentUser);

                if (existingMobileLiftingEquip == null)
                {
                    //Track the changes into the Audit Trail 
                    Change change = new Change(CurrentUser, "RES_Technics_MOB_LIFT_EQUIP");

                    MobileLiftingEquipUtil.ChangeRegNumber(mobileLiftingEquipId, newRegNumber, CurrentUser, change);

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
                GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP") == UIAccessLevel.Hidden)
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

            List<MobileLiftingEquipRegNumber> mobileLiftingEquipRegNumbers = new List<MobileLiftingEquipRegNumber>();
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

            int mobileLiftingEquipId = 0;
            int.TryParse((Request.Params["MobileLiftingEquipId"]).ToString(), out mobileLiftingEquipId);

            allRows = MobileLiftingEquipUtil.GetAllMobileLiftingEquipRegNumbersCount(mobileLiftingEquipId, currentUser);
            maxPage = allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            mobileLiftingEquipRegNumbers = MobileLiftingEquipUtil.GetAllMobileLiftingEquipRegNumbers(mobileLiftingEquipId, orderBy, pageIndex, pageLength, currentUser);

            // No data found
            if (mobileLiftingEquipRegNumbers.Count == 0)
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

                        <table id='tblMobileLiftingEquipRegNumberHistory' class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
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
            foreach (MobileLiftingEquipRegNumber mobileLiftingEquipRegNumber in mobileLiftingEquipRegNumbers)
            {

                string cellStyle = "vertical-align: top;";

                html += @"<tr  class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"' >
                                 <td style='" + cellStyle + @"'>" + ((pageIndex - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + mobileLiftingEquipRegNumber.RegNumber + @"</td>
                              </tr>";

                counter++;
            }
            html += "</table>";

            html += "<input type='hidden' id='hdnMobileLiftingEquipRegNumberHistoryCounter' value='" + counter + "' />";


            html += @"<div style='height: 10px;'>
                </div>

                 <div style='text-align: center'>
                   <span id='lblMobileLiftingEquipRegNumberHistoryMessage'>" + htmlNoResults + @"</span><br/>
                </div>
";

            html += @"  </div>                        
                        <div id='btnCloseMobileLiftingEquipRegNumberHistoryTable' runat='server' class='Button' onclick=""HideRegNumberHistoryLightBox();""><i></i><div style='width:70px; padding-left:5px;'>Затвори</div><b></b></div>
                      </center>";



            return html;
        }
    }


    public static class AddEditTechnics_MOB_LIFT_EQUIP_PageUtil
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
                                 page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP_ADD") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP_ADD_BASICINFO") == UIAccessLevel.Disabled;

                screenHidden = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP_ADD") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP_ADD_BASICINFO") == UIAccessLevel.Hidden;

                basicInfoDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP_ADD") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP_ADD_BASICINFO") == UIAccessLevel.Disabled;

                basicInfoHidden = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP_ADD") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP_ADD_BASICINFO") == UIAccessLevel.Hidden;


                UIAccessLevel l;

                l = page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP_ADD_BASICINFO_REGNUMBER");

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
                                 page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_VEHICLES_EDIT") == UIAccessLevel.Disabled || isPreview;


                screenHidden = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP_EDIT") == UIAccessLevel.Hidden;

                basicInfoDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP_EDIT") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP_EDIT_BASICINFO") == UIAccessLevel.Disabled;

                basicInfoHidden = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP_EDIT") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP_EDIT_BASICINFO") == UIAccessLevel.Hidden;


                UIAccessLevel l;


                l = page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP_EDIT_BASICINFO_REGNUMBER");

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

            List<GTableItem> mobileLiftingEquipKinds = GTableItemUtil.GetAllGTableItemsByTableName("MobileLiftingEquipKind", page.ModuleKey, 1, 0, 0, page.CurrentUser);
            List<IDropDownItem> ddiMobileLiftingEquipKinds = new List<IDropDownItem>();
            foreach (GTableItem mobileLiftingEquipKind in mobileLiftingEquipKinds)
            {
                ddiMobileLiftingEquipKinds.Add(mobileLiftingEquipKind);
            }

            string mobileLiftingEquipKindsHTML = ListItems.GetDropDownHtml(ddiMobileLiftingEquipKinds, null, "ddMobileLiftingEquipKind", true, null, "", "style='width: 330px;'", true);
            string editMobileLiftingEquipKindsHTML = @"<img id=""imgMaintMobileLiftingEquipKind"" alt=""Редактиране на списъка"" title=""Редактиране на списъка"" style=""cursor: pointer;"" src=""../Images/list_edit.png"" onclick=""ShowGTable('MobileLiftingEquipKind', 1, 1, RefreshMobileLiftingEquipKindList);"" />";

            List<GTableItem> mobileLiftingEquipTypes = GTableItemUtil.GetAllGTableItemsByTableName("MobileLiftingEquipType", page.ModuleKey, 1, 0, 0, page.CurrentUser);
            List<IDropDownItem> ddiMobileLiftingEquipType = new List<IDropDownItem>();
            foreach (GTableItem mobileLiftingEquipType in mobileLiftingEquipTypes)
            {
                ddiMobileLiftingEquipType.Add(mobileLiftingEquipType);
            }

            string mobileLiftingEquipTypesHTML = ListItems.GetDropDownHtml(ddiMobileLiftingEquipType, null, "ddMobileLiftingEquipType", true, null, "", "style='width: 330px;'", true);
            string editMobileLiftingEquipTypeHTML = @"<img id=""imgMaintMobileLiftingEquipType"" alt=""Редактиране на списъка"" title=""Редактиране на списъка"" style=""cursor: pointer;"" src=""../Images/list_edit.png"" onclick=""ShowGTable('MobileLiftingEquipType', 1, 1, RefreshMobileLiftingEquipTypeList);"" />";


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
            <input type=""text"" id=""txtInventoryNumber"" class=""InputField"" onblur=""InventoryNumberBlur();"" style=""width: 100px;"" maxlength=""50"" />
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
            <span id=""lblMobileLiftingEquipKind"" class=""InputLabel"">Вид:</span>
         </td>
         <td style=""text-align: left;"">
            " + mobileLiftingEquipKindsHTML + editMobileLiftingEquipKindsHTML + @"
         </td>
         <td style=""text-align: right;"">
            <span id=""lblMobileLiftingEquipType"" class=""InputLabel"">Тип:</span>
         </td>
         <td style=""text-align: left;"">
            " + mobileLiftingEquipTypesHTML + editMobileLiftingEquipTypeHTML + @"
         </td>
      </tr>
         <td style=""text-align: left;"" colspan=""2"">
            <span id=""lblLoadingCapacity"" class=""InputLabel"">Товароподемност:</span>
            <input type=""text"" id=""txtLoadingCapacity"" class=""InputField"" style=""width: 100px;"" maxlength=""50"" />
         </td>
      <tr>
      </tr>
   </table>
</fieldset>

<div style=""height: 10px;""></div>

" + AddEditTechnics_BasicInfo_PageUtil.GetBasicInfoResidenceContent(page) + @"

<div style=""height: 10px;""></div>

<input type=""hidden"" id=""hdnMobileLiftingEquipId"" />

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
                                 page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP_ADD") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP_ADD_BASICINFO") == UIAccessLevel.Disabled;

                basicInfoDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP_ADD") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP_ADD_BASICINFO") == UIAccessLevel.Disabled;

                UIAccessLevel l;

                l = page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP_ADD_BASICINFO_INVENTORYNUMBER");

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

                l = page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP_ADD_BASICINFO_TECHNICSCATEGORY");

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


                l = page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP_ADD_BASICINFO_MOB_LIFT_EQUIPKIND");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblMobileLiftingEquipKind");
                    disabledClientControls.Add("ddMobileLiftingEquipKind");
                    hiddenClientControls.Add("imgMaintMobileLiftingEquipKind");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblMobileLiftingEquipKind");
                    hiddenClientControls.Add("ddMobileLiftingEquipKind");
                    hiddenClientControls.Add("imgMaintMobileLiftingEquipKind");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP_ADD_BASICINFO_MOB_LIFT_EQUIPTYPE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblMobileLiftingEquipType");
                    disabledClientControls.Add("ddMobileLiftingEquipType");
                    hiddenClientControls.Add("imgMaintMobileLiftingEquipType");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblMobileLiftingEquipType");
                    hiddenClientControls.Add("ddMobileLiftingEquipType");
                    hiddenClientControls.Add("imgMaintMobileLiftingEquipType");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP_ADD_BASICINFO_LOADINGCAPACITY");

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

                AddEditTechnics_BasicInfo_PageUtil.GetBasicInfoResidenceUIItems(page, true,
                    ref disabledClientControls, ref hiddenClientControls);
            }
            else // edit mode of page
            {
                screenDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP_EDIT") == UIAccessLevel.Disabled || isPreview;

                basicInfoDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP_EDIT") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP_EDIT_BASICINFO") == UIAccessLevel.Disabled;

                
                UIAccessLevel l;

                l = page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP_EDIT_BASICINFO_INVENTORYNUMBER");

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

                l = page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP_EDIT_BASICINFO_TECHNICSCATEGORY");

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

                l = page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP_EDIT_BASICINFO_MOB_LIFT_EQUIPKIND");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblMobileLiftingEquipKind");
                    disabledClientControls.Add("ddMobileLiftingEquipKind");
                    hiddenClientControls.Add("imgMaintMobileLiftingEquipKind");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblMobileLiftingEquipKind");
                    hiddenClientControls.Add("ddMobileLiftingEquipKind");
                    hiddenClientControls.Add("imgMaintMobileLiftingEquipKind");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP_EDIT_BASICINFO_MOB_LIFT_EQUIPTYPE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblMobileLiftingEquipType");
                    disabledClientControls.Add("ddMobileLiftingEquipType");
                    hiddenClientControls.Add("imgMaintMobileLiftingEquipType");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblMobileLiftingEquipType");
                    hiddenClientControls.Add("ddMobileLiftingEquipType");
                    hiddenClientControls.Add("imgMaintMobileLiftingEquipType");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_MOB_LIFT_EQUIP_EDIT_BASICINFO_LOADINGCAPACITY");

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

                AddEditTechnics_BasicInfo_PageUtil.GetBasicInfoResidenceUIItems(page, false,
                    ref disabledClientControls, ref hiddenClientControls);
            }

            if (page.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled ||
                page.GetUIItemAccessLevel("RES_LISTMAINT_MOBILELIFTINGEQUIPKIND") != UIAccessLevel.Enabled)
            {
                hiddenClientControls.Add("imgMaintMobileLiftingEquipKind");
            }

            if (page.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled ||
                page.GetUIItemAccessLevel("RES_LISTMAINT_MOBILELIFTINGEQUIPTYPE") != UIAccessLevel.Enabled)
            {
                hiddenClientControls.Add("imgMaintMobileLiftingEquipType");
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
