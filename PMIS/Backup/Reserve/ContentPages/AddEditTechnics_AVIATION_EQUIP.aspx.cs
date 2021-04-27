using System;
using System.Collections.Generic;
using System.Text;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class AddEditTechnics_AVIATION_EQUIP : RESPage
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
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSCheckAirInvNumber")
            {
                JSCheckAirInvNumber();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRefreshAviationAirKindList")
            {
                JSRefreshAviationAirKindList();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRefreshAviationAirTypeList")
            {
                JSRefreshAviationAirTypeList();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            //if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRefreshAviationAirModelList")
            //{
            //    JSRefreshAviationAirModelList();
            //    return;
            //}

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRefreshAviationOtherKindList")
            {
                JSRefreshAviationOtherKindList();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRefreshAviationOtherTypeList")
            {
                JSRefreshAviationOtherTypeList();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            //if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRepopulateAviationOtherBaseMachineModels")
            //{
            //    JSRepopulateAviationOtherBaseMachineModels();
            //    return;
            //}

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRefreshAviationOtherBaseMachineKindList")
            {
                JSRefreshAviationOtherBaseMachineKindList();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRefreshAviationOtherBaseMachineTypeList")
            {
                JSRefreshAviationOtherBaseMachineTypeList();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRefreshAviationOtherEquipmentKindList")
            {
                JSRefreshAviationOtherEquipmentKindList();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSChangeAirInvNumber")
            {
                JSChangeAirInvNumber();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadAirInvNumberHistory")
            {
                JSLoadAirInvNumberHistory();
                return;
            }
        }

        //Load Basic Info (ajax call)
        private void JSLoadBasicInfo()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            int technicsId = 0;
            if (!String.IsNullOrEmpty(Request.Form["TechnicsId"]))
                technicsId = int.Parse(Request.Form["TechnicsId"]);

            string stat = "";
            string response = "";

            try
            {
                AviationEquip aviationEquip = AviationEquipUtil.GetAviationEquipByTechnicsId(technicsId, CurrentUser);
                TechnicsMilRepStatus currentMilRepStatus = TechnicsMilRepStatusUtil.GetTechnicsMilRepCurrentStatusByTechnicsId(aviationEquip.TechnicsId, CurrentUser);
                string currMilRepStatusName = (currentMilRepStatus != null ? currentMilRepStatus.TechMilitaryReportStatus.TechMilitaryReportStatusName : TechMilitaryReportStatusUtil.GetLabelWhenLackOfStatus());
           
                stat = AJAXTools.OK;

                response = @"
                    <aviationEquip>
                         <technicsId>" + AJAXTools.EncodeForXML(aviationEquip.TechnicsId.ToString()) + @"</technicsId>
                         <aviationEquipId>" + AJAXTools.EncodeForXML(aviationEquip.AviationEquipId.ToString()) + @"</aviationEquipId>
                         <airInvNumber>" + AJAXTools.EncodeForXML(aviationEquip.AirInvNumber) + @"</airInvNumber>
                         <technicsCategoryId>" + AJAXTools.EncodeForXML(aviationEquip.Technics.TechnicsCategoryId.HasValue ? aviationEquip.Technics.TechnicsCategoryId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</technicsCategoryId>
                         <aviationAirKindId>" + AJAXTools.EncodeForXML(aviationEquip.AviationAirKindId.HasValue ? aviationEquip.AviationAirKindId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</aviationAirKindId>
                         <aviationAirTypeId>" + AJAXTools.EncodeForXML(aviationEquip.AviationAirTypeId.HasValue ? aviationEquip.AviationAirTypeId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</aviationAirTypeId>
                         <lastModified>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDateTime(aviationEquip.Technics.LastModifiedDate)) + @"</lastModified>
                         <resMilRepStatus>" + AJAXTools.EncodeForXML(currMilRepStatusName) + @"</resMilRepStatus>";
          
                         //<aviationAirModelId>" + AJAXTools.EncodeForXML(aviationEquip.AviationAirModelId.HasValue ? aviationEquip.AviationAirModelId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</aviationAirModelId>
                
                response += @"
                         <aviationAirModelName>" + AJAXTools.EncodeForXML(aviationEquip.AviationAirModelName) + @"</aviationAirModelName>
                         <airSeats>" + AJAXTools.EncodeForXML(aviationEquip.AirSeats.HasValue ? aviationEquip.AirSeats.ToString() : "") + @"</airSeats>
                         <airCarryingCapacity>" + AJAXTools.EncodeForXML(aviationEquip.AirCarryingCapacity.HasValue ? aviationEquip.AirCarryingCapacity.ToString() : "") + @"</airCarryingCapacity>
                         <airMaxDistance>" + AJAXTools.EncodeForXML(aviationEquip.AirMaxDistance.HasValue ? aviationEquip.AirMaxDistance.ToString() : "") + @"</airMaxDistance>
                         <airLastTechnicalReviewDate>" + AJAXTools.EncodeForXML(CommonFunctions.FormatDate(aviationEquip.AirLastTechnicalReviewDate)) + @"</airLastTechnicalReviewDate>
                         <otherInvNumber>" + AJAXTools.EncodeForXML(aviationEquip.OtherInvNumber) + @"</otherInvNumber>
                         <aviationOtherKindId>" + AJAXTools.EncodeForXML(aviationEquip.AviationOtherKindId.HasValue ? aviationEquip.AviationOtherKindId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</aviationOtherKindId>
                         <aviationOtherTypeId>" + AJAXTools.EncodeForXML(aviationEquip.AviationOtherTypeId.HasValue ? aviationEquip.AviationOtherTypeId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</aviationOtherTypeId>";

                         //<aviationOtherBaseMachineMakeId>" + AJAXTools.EncodeForXML(aviationEquip.AviationOtherBaseMachineMakeId.HasValue ? aviationEquip.AviationOtherBaseMachineMakeId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</aviationOtherBaseMachineMakeId>
                         //<aviationOtherBaseMachineModelId>" + AJAXTools.EncodeForXML(aviationEquip.AviationOtherBaseMachineModelId.HasValue ? aviationEquip.AviationOtherBaseMachineModelId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</aviationOtherBaseMachineModelId>

                response += @"
                         <aviationOtherBaseMachineMakeName>" + AJAXTools.EncodeForXML(aviationEquip.AviationOtherBaseMachineMakeName) + @"</aviationOtherBaseMachineMakeName>
                         <aviationOtherBaseMachineModelName>" + AJAXTools.EncodeForXML(aviationEquip.AviationOtherBaseMachineModelName) + @"</aviationOtherBaseMachineModelName>
                         <aviationOtherBaseMachineKindId>" + AJAXTools.EncodeForXML(aviationEquip.AviationOtherBaseMachineKindId.HasValue ? aviationEquip.AviationOtherBaseMachineKindId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</aviationOtherBaseMachineKindId>
                         <aviationOtherBaseMachineTypeId>" + AJAXTools.EncodeForXML(aviationEquip.AviationOtherBaseMachineTypeId.HasValue ? aviationEquip.AviationOtherBaseMachineTypeId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</aviationOtherBaseMachineTypeId>
                         <baseMachineMileageHoursSinceLastRepair>" + AJAXTools.EncodeForXML(aviationEquip.BaseMachineMileageHoursSinceLastRepair) + @"</baseMachineMileageHoursSinceLastRepair>
                         <aviationOtherEquipmentKindId>" + AJAXTools.EncodeForXML(aviationEquip.AviationOtherEquipmentKindId.HasValue ? aviationEquip.AviationOtherEquipmentKindId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</aviationOtherEquipmentKindId>
                         <equipMileageHourSinceLstRepair>" + AJAXTools.EncodeForXML(aviationEquip.EquipMileageHourSinceLstRepair) + @"</equipMileageHourSinceLstRepair>
                         <currMilDepartment>" + AJAXTools.EncodeForXML(aviationEquip.Technics.CurrTechMilRepStatus != null && aviationEquip.Technics.CurrTechMilRepStatus.SourceMilDepartment != null ? aviationEquip.Technics.CurrTechMilRepStatus.SourceMilDepartment.MilitaryDepartmentName : TechMilitaryReportStatusUtil.GetLabelWhenLackOfStatus()) + @"</currMilDepartment>
                         <normativeTechnicsId>" + AJAXTools.EncodeForXML(aviationEquip.Technics.NormativeTechnicsId != null ? aviationEquip.Technics.NormativeTechnics.NormativeTechnicsId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</normativeTechnicsId>
                         <normativeCode>" + AJAXTools.EncodeForXML(aviationEquip.Technics.NormativeTechnicsId != null ? aviationEquip.Technics.NormativeTechnics.NormativeCode : "") + @"</normativeCode>
                    </aviationEquip>";

                //response += "<avother_model>" +
                //            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                //            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                //            "</avother_model>";

                //if (aviationEquip.AviationOtherBaseMachineModelId.HasValue)
                //{
                //    foreach (AviationOtherBaseMachineModel aviationOtherBaseMachineModel in aviationEquip.AviationOtherBaseMachineMake.AviationOtherBaseMachineModels)
                //    {
                //        response += "<avother_model>" +
                //                    "<id>" + aviationOtherBaseMachineModel.AviationOtherBaseMachineModelId.ToString() + "</id>" +
                //                    "<name>" + AJAXTools.EncodeForXML(aviationOtherBaseMachineModel.AviationOtherBaseMachineModelName) + "</name>" +
                //                    "</avother_model>";
                //    }
                //}
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
                GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            int? technicsId = null;
            if (!String.IsNullOrEmpty(Request.Params["TechnicsId"]))
            {
                technicsId = int.Parse(Request.Params["TechnicsId"]);
            }

            int? aviationEquipId = null;
            if (!String.IsNullOrEmpty(Request.Params["AviationEquipId"]))
            {
                aviationEquipId = int.Parse(Request.Params["AviationEquipId"]);
            }

            string airInvNumber = Request.Params["AirInvNumber"];
            
            int? technicsCategoryId = null;
            if (!String.IsNullOrEmpty(Request.Params["TechnicsCategoryId"]) &&
                Request.Params["TechnicsCategoryId"] != ListItems.GetOptionChooseOne().Value)
            {
                technicsCategoryId = int.Parse(Request.Params["TechnicsCategoryId"]);
            }

            int? aviationAirKindId = null;
            if (!String.IsNullOrEmpty(Request.Params["AviationAirKindId"]) &&
                Request.Params["AviationAirKindId"] != ListItems.GetOptionChooseOne().Value)
            {
                aviationAirKindId = int.Parse(Request.Params["AviationAirKindId"]);
            }

            int? aviationAirTypeId = null;
            if (!String.IsNullOrEmpty(Request.Params["AviationAirTypeId"]) &&
                Request.Params["AviationAirTypeId"] != ListItems.GetOptionChooseOne().Value)
            {
                aviationAirTypeId = int.Parse(Request.Params["AviationAirTypeId"]);
            }

            //int? aviationAirModelId = null;
            //if (!String.IsNullOrEmpty(Request.Params["AviationAirModelId"]) &&
            //    Request.Params["AviationAirModelId"] != ListItems.GetOptionChooseOne().Value)
            //{
            //    aviationAirModelId = int.Parse(Request.Params["AviationAirModelId"]);
            //}

            string aviationAirModelName = Request.Params["AviationAirModelName"];

            int? airSeats = null;
            if (!String.IsNullOrEmpty(Request.Params["AirSeats"]) &&
                Request.Params["AirSeats"] != ListItems.GetOptionChooseOne().Value)
            {
                airSeats = int.Parse(Request.Params["AirSeats"]);
            }

            decimal? airCarryingCapacity = null;
            if (!String.IsNullOrEmpty(Request.Params["AirCarryingCapacity"]) &&
                Request.Params["AirCarryingCapacity"] != ListItems.GetOptionChooseOne().Value)
            {
                airCarryingCapacity = decimal.Parse(Request.Params["AirCarryingCapacity"]);
            }

            decimal? airMaxDistance = null;
            if (!String.IsNullOrEmpty(Request.Params["AirMaxDistance"]) &&
                Request.Params["AirMaxDistance"] != ListItems.GetOptionChooseOne().Value)
            {
                airMaxDistance = decimal.Parse(Request.Params["AirMaxDistance"]);
            }

            DateTime? airLastTechnicalReviewDate = null;
            if (!String.IsNullOrEmpty(Request.Params["AirLastTechnicalReviewDate"]))
            {
                airLastTechnicalReviewDate = CommonFunctions.ParseDate(Request.Params["AirLastTechnicalReviewDate"]);
            }

            string otherInvNumber = Request.Params["OtherInvNumber"];

            int? aviationOtherKindId = null;
            if (!String.IsNullOrEmpty(Request.Params["AviationOtherKindId"]) &&
                Request.Params["AviationOtherKindId"] != ListItems.GetOptionChooseOne().Value)
            {
                aviationOtherKindId = int.Parse(Request.Params["AviationOtherKindId"]);
            }

            int? aviationOtherTypeId = null;
            if (!String.IsNullOrEmpty(Request.Params["AviationOtherTypeId"]) &&
                Request.Params["AviationOtherTypeId"] != ListItems.GetOptionChooseOne().Value)
            {
                aviationOtherTypeId = int.Parse(Request.Params["AviationOtherTypeId"]);
            }

            //int? aviationOtherBaseMachineMakeId = null;
            //if (!String.IsNullOrEmpty(Request.Params["AviationOtherBaseMachineMakeId"]) &&
            //    Request.Params["AviationOtherBaseMachineMakeId"] != ListItems.GetOptionChooseOne().Value)
            //{
            //    aviationOtherBaseMachineMakeId = int.Parse(Request.Params["AviationOtherBaseMachineMakeId"]);
            //}

            //int? aviationOtherBaseMachineModelId = null;
            //if (!String.IsNullOrEmpty(Request.Params["AviationOtherBaseMachineModelId"]) &&
            //    Request.Params["AviationOtherBaseMachineModelId"] != ListItems.GetOptionChooseOne().Value)
            //{
            //    aviationOtherBaseMachineModelId = int.Parse(Request.Params["AviationOtherBaseMachineModelId"]);
            //}

            string aviationOtherBaseMachineMakeName = Request.Params["AviationOtherBaseMachineMakeName"];
            string aviationOtherBaseMachineModelName = Request.Params["AviationOtherBaseMachineModelName"];

            int? aviationOtherBaseMachineKindId = null;
            if (!String.IsNullOrEmpty(Request.Params["AviationOtherBaseMachineKindId"]) &&
                Request.Params["AviationOtherBaseMachineKindId"] != ListItems.GetOptionChooseOne().Value)
            {
                aviationOtherBaseMachineKindId = int.Parse(Request.Params["AviationOtherBaseMachineKindId"]);
            }

            int? aviationOtherBaseMachineTypeId = null;
            if (!String.IsNullOrEmpty(Request.Params["AviationOtherBaseMachineTypeId"]) &&
                Request.Params["AviationOtherBaseMachineTypeId"] != ListItems.GetOptionChooseOne().Value)
            {
                aviationOtherBaseMachineTypeId = int.Parse(Request.Params["AviationOtherBaseMachineTypeId"]);
            }

            string baseMachineMileageHoursSinceLastRepair = Request.Params["BaseMachineMileageHoursSinceLastRepair"];

            int? aviationOtherEquipmentKindId = null;
            if (!String.IsNullOrEmpty(Request.Params["AviationOtherEquipmentKindId"]) &&
                Request.Params["AviationOtherEquipmentKindId"] != ListItems.GetOptionChooseOne().Value)
            {
                aviationOtherEquipmentKindId = int.Parse(Request.Params["AviationOtherEquipmentKindId"]);
            }

            string equipMileageHourSinceLstRepair = Request.Params["EquipMileageHourSinceLstRepair"];

            int? normativeTechnicsId = null;
            if (!String.IsNullOrEmpty(Request.Form["NormativeTechnicsId"]) &&
                Request.Form["NormativeTechnicsId"] != ListItems.GetOptionChooseOne().Value)
            {
                normativeTechnicsId = int.Parse(Request.Form["NormativeTechnicsId"]);
            }

            AviationEquip aviationEquip = new AviationEquip(CurrentUser);

            aviationEquip.AviationEquipId = aviationEquipId.HasValue ? aviationEquipId.Value : 0;
            aviationEquip.TechnicsId = technicsId.HasValue ? technicsId.Value : 0;
            aviationEquip.AirInvNumber = airInvNumber;
            aviationEquip.AviationAirKindId = aviationAirKindId;
            aviationEquip.AviationAirTypeId = aviationAirTypeId;

            //aviationEquip.AviationAirModelId = aviationAirModelId;

            aviationEquip.AviationAirModelName = aviationAirModelName;

            aviationEquip.AirSeats = airSeats;
            aviationEquip.AirCarryingCapacity = airCarryingCapacity;
            aviationEquip.AirMaxDistance = airMaxDistance;
            aviationEquip.AirLastTechnicalReviewDate = airLastTechnicalReviewDate;
            aviationEquip.OtherInvNumber = otherInvNumber;
            aviationEquip.AviationOtherKindId = aviationOtherKindId;
            aviationEquip.AviationOtherTypeId = aviationOtherTypeId;

            //aviationEquip.AviationOtherBaseMachineMakeId = aviationOtherBaseMachineMakeId;
            //aviationEquip.AviationOtherBaseMachineModelId = aviationOtherBaseMachineModelId;

            aviationEquip.AviationOtherBaseMachineMakeName = aviationOtherBaseMachineMakeName;
            aviationEquip.AviationOtherBaseMachineModelName = aviationOtherBaseMachineModelName;
            
            aviationEquip.AviationOtherBaseMachineKindId = aviationOtherBaseMachineKindId;
            aviationEquip.AviationOtherBaseMachineTypeId = aviationOtherBaseMachineTypeId;
            aviationEquip.BaseMachineMileageHoursSinceLastRepair = baseMachineMileageHoursSinceLastRepair;
            aviationEquip.AviationOtherEquipmentKindId = aviationOtherEquipmentKindId;
            aviationEquip.EquipMileageHourSinceLstRepair = equipMileageHourSinceLstRepair;

            aviationEquip.Technics = new Technics(CurrentUser);
            aviationEquip.Technics.TechnicsId = aviationEquip.TechnicsId;
            aviationEquip.Technics.TechnicsType = TechnicsTypeUtil.GetTechnicsType("AVIATION_EQUIP", CurrentUser);
            aviationEquip.Technics.TechnicsCategoryId = technicsCategoryId.HasValue ? technicsCategoryId.Value : (int?)null;
            aviationEquip.Technics.ItemsCount = 1;
            aviationEquip.Technics.NormativeTechnicsId = normativeTechnicsId;

            string stat = "";
            string response = "";

            try
            {
                //Track the changes into the Audit Trail 
                Change change = new Change(CurrentUser, "RES_Technics_AVIATION_EQUIP");

                AviationEquipUtil.SaveAviationEquip(aviationEquip, CurrentUser, change);

                //Write into the Audit Trail
                change.WriteLog();

                stat = AJAXTools.OK;
                response = @"<response>OK</response>
                             <technicsId>" + AJAXTools.EncodeForXML(aviationEquip.TechnicsId.ToString()) + @"</technicsId>
                             <aviationEquipId>" + AJAXTools.EncodeForXML(aviationEquip.AviationEquipId.ToString()) + @"</aviationEquipId>";
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

        //Check air inv Number(ajax call)
        private void JSCheckAirInvNumber()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string airInvNumber = Request.Params["AirInvNumber"];

            string stat = "";
            string response = "";

            try
            {
                int technicsId = 0;

                AviationEquip aviationEquip = AviationEquipUtil.GetAviationEquipByAirInvNumber(airInvNumber, CurrentUser);

                if (aviationEquip != null)
                {
                    technicsId = aviationEquip.TechnicsId;
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

        //Get the aviationEquip otherbase models for a particular aviationEquip otherbase make (ajax call)
        //private void JSRepopulateAviationOtherBaseMachineModels()
        //{
        //    if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
        //        GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP") == UIAccessLevel.Hidden)
        //        RedirectAjaxAccessDenied();

        //    string stat = "";
        //    string response = "";

        //    try
        //    {
        //        int aviationOtherBaseMachineMakeId = 0;

        //        if (!String.IsNullOrEmpty(Request.Form["AviationOtherBaseMachineMakeId"]))
        //            aviationOtherBaseMachineMakeId = int.Parse(Request.Form["AviationOtherBaseMachineMakeId"]);

        //        response = "<aviationOtherBaseMachineModels>";

        //        response += "<m>" +
        //                     "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
        //                     "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
        //                     "</m>";

        //        List<AviationOtherBaseMachineModel> models = AviationOtherBaseMachineModelUtil.GetAllAviationOtherBaseMachineModels(aviationOtherBaseMachineMakeId, CurrentUser);

        //        foreach (AviationOtherBaseMachineModel model in models)
        //        {
        //            response += "<m>" +
        //                        "<id>" + model.AviationOtherBaseMachineModelId.ToString() + "</id>" +
        //                        "<name>" + AJAXTools.EncodeForXML(model.AviationOtherBaseMachineModelName) + "</name>" +
        //                        "</m>";
        //        }

        //        response += "</aviationOtherBaseMachineModels>";

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

        //Refresh the list AviationAirKind (ajax call)
        private void JSRefreshAviationAirKindList()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<aviationAirKind>";

                response += "<i>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</i>";

                List<GTableItem> aviationAirKinds = GTableItemUtil.GetAllGTableItemsByTableName("AviationAirKind", ModuleKey, 1, 0, 0, CurrentUser);

                foreach (GTableItem aviationAirKind in aviationAirKinds)
                {
                    response += "<i>" +
                                "<id>" + aviationAirKind.TableKey.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(aviationAirKind.TableValue.ToString()) + "</name>" +
                                "</i>";
                }

                response += "</aviationAirKind>";

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

        //Refresh the list AviationAirType (ajax call)
        private void JSRefreshAviationAirTypeList()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<aviationAirType>";

                response += "<i>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</i>";

                List<GTableItem> aviationAirTypes = GTableItemUtil.GetAllGTableItemsByTableName("AviationAirType", ModuleKey, 1, 0, 0, CurrentUser);

                foreach (GTableItem aviationAirType in aviationAirTypes)
                {
                    response += "<i>" +
                                "<id>" + aviationAirType.TableKey.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(aviationAirType.TableValue.ToString()) + "</name>" +
                                "</i>";
                }

                response += "</aviationAirType>";

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

        //Refresh the list AviationAirModel (ajax call)
        //private void JSRefreshAviationAirModelList()
        //{
        //    if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
        //        GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP") == UIAccessLevel.Hidden)
        //        RedirectAjaxAccessDenied();

        //    string stat = "";
        //    string response = "";

        //    try
        //    {
        //        response = "<aviationAirModel>";

        //        response += "<i>" +
        //                    "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
        //                    "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
        //                    "</i>";

        //        List<GTableItem> aviationAirModels = GTableItemUtil.GetAllGTableItemsByTableName("AviationAirModel", ModuleKey, 1, 0, 0, CurrentUser);

        //        foreach (GTableItem aviationAirModel in aviationAirModels)
        //        {
        //            response += "<i>" +
        //                        "<id>" + aviationAirModel.TableKey.ToString() + "</id>" +
        //                        "<name>" + AJAXTools.EncodeForXML(aviationAirModel.TableValue.ToString()) + "</name>" +
        //                        "</i>";
        //        }

        //        response += "</aviationAirModel>";

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

        //Refresh the list AviationOtherKind (ajax call)
        private void JSRefreshAviationOtherKindList()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<aviationOtherKind>";

                response += "<i>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</i>";

                List<GTableItem> aviationOtherKinds = GTableItemUtil.GetAllGTableItemsByTableName("AviationOtherKind", ModuleKey, 1, 0, 0, CurrentUser);

                foreach (GTableItem aviationOtherKind in aviationOtherKinds)
                {
                    response += "<i>" +
                                "<id>" + aviationOtherKind.TableKey.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(aviationOtherKind.TableValue.ToString()) + "</name>" +
                                "</i>";
                }

                response += "</aviationOtherKind>";

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

        //Refresh the list AviationOtherType (ajax call)
        private void JSRefreshAviationOtherTypeList()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<aviationOtherType>";

                response += "<i>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</i>";

                List<GTableItem> aviationOtherTypes = GTableItemUtil.GetAllGTableItemsByTableName("AviationOtherType", ModuleKey, 1, 0, 0, CurrentUser);

                foreach (GTableItem aviationOtherType in aviationOtherTypes)
                {
                    response += "<i>" +
                                "<id>" + aviationOtherType.TableKey.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(aviationOtherType.TableValue.ToString()) + "</name>" +
                                "</i>";
                }

                response += "</aviationOtherType>";

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


        //Refresh the list AviationOtherBaseMachineKind (ajax call)
        private void JSRefreshAviationOtherBaseMachineKindList()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<aviationOtherBaseMachineKind>";

                response += "<i>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</i>";

                List<GTableItem> aviationOtherBaseMachineKinds = GTableItemUtil.GetAllGTableItemsByTableName("AviationOtherBaseMachineKind", ModuleKey, 1, 0, 0, CurrentUser);

                foreach (GTableItem aviationOtherBaseMachineKind in aviationOtherBaseMachineKinds)
                {
                    response += "<i>" +
                                "<id>" + aviationOtherBaseMachineKind.TableKey.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(aviationOtherBaseMachineKind.TableValue.ToString()) + "</name>" +
                                "</i>";
                }

                response += "</aviationOtherBaseMachineKind>";

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

        //Refresh the list AviationOtherBaseMachineType (ajax call)
        private void JSRefreshAviationOtherBaseMachineTypeList()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<aviationOtherBaseMachineType>";

                response += "<i>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</i>";

                List<GTableItem> aviationOtherBaseMachineTypes = GTableItemUtil.GetAllGTableItemsByTableName("AviationOtherBaseMachineType", ModuleKey, 1, 0, 0, CurrentUser);

                foreach (GTableItem aviationOtherBaseMachineType in aviationOtherBaseMachineTypes)
                {
                    response += "<i>" +
                                "<id>" + aviationOtherBaseMachineType.TableKey.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(aviationOtherBaseMachineType.TableValue.ToString()) + "</name>" +
                                "</i>";
                }

                response += "</aviationOtherBaseMachineType>";

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

        //Refresh the list AviationOtherEquipmentKind (ajax call)
        private void JSRefreshAviationOtherEquipmentKindList()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<aviationOtherEquipmentKind>";

                response += "<i>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</i>";

                List<GTableItem> aviationOtherEquipmentKinds = GTableItemUtil.GetAllGTableItemsByTableName("AviationOtherEquipmentKind", ModuleKey, 1, 0, 0, CurrentUser);

                foreach (GTableItem aviationOtherEquipmentKind in aviationOtherEquipmentKinds)
                {
                    response += "<i>" +
                                "<id>" + aviationOtherEquipmentKind.TableKey.ToString() + "</id>" +
                                "<name>" + AJAXTools.EncodeForXML(aviationOtherEquipmentKind.TableValue.ToString()) + "</name>" +
                                "</i>";
                }

                response += "</aviationOtherEquipmentKind>";

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



        //Change the air inv number (ajax call)
        private void JSChangeAirInvNumber()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            int aviationEquipId = int.Parse(Request.Params["AviationEquipId"]);
            string newAirInvNumber = Request.Params["NewAirInvNumber"];
            
            string stat = "";
            string response = "";

            try
            {
                AviationEquip existingAviationEquip = AviationEquipUtil.GetAviationEquipByAirInvNumber(newAirInvNumber, CurrentUser);

                if (existingAviationEquip == null)
                {
                    //Track the changes into the Audit Trail 
                    Change change = new Change(CurrentUser, "RES_Technics_AVIATION_EQUIP");

                    AviationEquipUtil.ChangeAirInvNumber(aviationEquipId, newAirInvNumber, CurrentUser, change);

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

        private void JSLoadAirInvNumberHistory()
        {
            if (GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<response>" + AJAXTools.EncodeForXML(GetAirInvNumberHistoryLightBox(CurrentUser)) + "</response>";

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

        public string GetAirInvNumberHistoryLightBox(User currentUser)
        {
            string html = "";

            string htmlNoResults = "";

            List<AviationEquipAirInvNumber> aviationEquipAirInvNumbers = new List<AviationEquipAirInvNumber>();
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

            int aviationEquipId = 0;
            int.TryParse((Request.Params["AviationEquipId"]).ToString(), out aviationEquipId);

            allRows = AviationEquipUtil.GetAllAviationEquipAirInvNumbersCount(aviationEquipId, currentUser);
            maxPage = allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            aviationEquipAirInvNumbers = AviationEquipUtil.GetAllAviationEquipAirInvNumbers(aviationEquipId, orderBy, pageIndex, pageLength, currentUser);

            // No data found
            if (aviationEquipAirInvNumbers.Count == 0)
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
                              <img id='btnTableFirst' " + btnFirst + @" alt='Първа страница' title='Първа страница' class='PaginationButton' onclick=""BtnAirInvNumberHistoryPagingClick('btnFirst');"" />
                              <img id='btnTablePrev' " + btnPrev + @" alt='Предишна страница' title='Предишна страница' class='PaginationButton' onclick=""BtnAirInvNumberHistoryPagingClick('btnPrev');"" />
                              <span id='lblTablePagination' class='PaginationLabel'>" + pageTablePagination + @"</span>
                              <img id='btnTableNext' " + btnNext + @" alt='Следваща страница' title='Следваща страница' class='PaginationButton' onclick=""BtnAirInvNumberHistoryPagingClick('btnNext');"" />
                              <img id='btnTableLast' " + btnLast + @" alt='Последна страница' title='Последна страница' class='PaginationButton' onclick=""BtnAirInvNumberHistoryPagingClick('btnLast');"" />
                              
                              <span style='padding: 0 30px'>&nbsp;</span>
                              <span style='text-align: right;'>Отиди на страница</span>
                              <input id='txtTableGotoPage' class='InputField' type='text' style='width: 30px;' value='' />
                              <img id='btnTableGoto' src='../Images/ButtonGoto.png' alt='Отиди на страница' title='Отиди на страница' class='PaginationButton' onclick=""BtnAirInvNumberHistoryPagingClick('btnPageGo');"" />
                           </div>
                        </div>

                        <table id='tblAviationEquipAirInvNumberHistory' class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
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
            foreach (AviationEquipAirInvNumber aviationEquipAirInvNumber in aviationEquipAirInvNumbers)
            {

                string cellStyle = "vertical-align: top;";

                html += @"<tr  class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"' >
                                 <td style='" + cellStyle + @"'>" + ((pageIndex - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + aviationEquipAirInvNumber.AirInvNumber + @"</td>
                              </tr>";

                counter++;
            }
            html += "</table>";

            html += "<input type='hidden' id='hdnAviationEquipAirInvNumberHistoryCounter' value='" + counter + "' />";


            html += @"<div style='height: 10px;'>
                </div>

                 <div style='text-align: center'>
                   <span id='lblAviationEquipAirInvNumberHistoryMessage'>" + htmlNoResults + @"</span><br/>
                </div>
";

            html += @"  </div>                        
                        <div id='btnCloseAviationEquipAirInvNumberHistoryTable' runat='server' class='Button' onclick=""HideAirInvNumberHistoryLightBox();""><i></i><div style='width:70px; padding-left:5px;'>Затвори</div><b></b></div>
                      </center>";



            return html;
        }
    }


    public static class AddEditTechnics_AVIATION_EQUIP_PageUtil
    {
        public static string GetGeneralPanelInventaryNumberContent()
        {
            string html = "";
            html = @"<div id=""tdAirInvNumber"">
                    <span id=""lblAirInvNuber"" class=""InputLabel"" style=""vertical-align: top; position: relative; top: 4px;"">Инвентарен номер:</span>
                    <span id=""txtAirInvNumberCont""><input type=""text"" id=""txtAirInvNumber"" class=""RequiredInputField"" style=""width: 90px; display: none;"" maxlength=""20""
                                      onfocus=""AirInvNumberFocus();"" onblur=""AirInvNumberBlur();"" /></span>
                    <span id=""lblAirInvNumberValueCont""><span id=""lblAirInvNumberValue"" class=""ReadOnlyValue"" style=""display: none; vertical-align: top; position: relative; top: 4px;""></span></span>
                    <span id=""imgEditAirInvNumberCont""><img id=""imgEditAirInvNumber"" alt=""Промяна на регистрационния номер"" title=""Промяна на регистрационния номер"" style=""cursor: pointer; display: none;"" src=""../Images/list_edit.png"" onclick=""ChangeAirInvNumber();"" /></span>
                    <span id=""imgHistoryAirInvNumberCont""><img id=""imgHistoryAirInvNumber"" alt=""История на регистрационните номера"" title=""История на регистрационните номера"" style=""cursor: pointer; width: 18px; height: 18px; display: none;"" src=""../Images/index_view.png"" onclick=""AirInvNumberHistory_Click();"" /></span>
                    </div>
                    <div id=""ChangeAirInvNumberLightBox"" class=""ChangeVehicleRegNumberLightBox"" style=""display: none; text-align: center;"">
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
                                        <span id=""lblCurrAirInvNumber"" class=""InputLabel"">Текущ инв. номер:</span>
                                    </td>
                                    <td style=""text-align: left;"">
                                        <span id=""lblCurrAirInvNumberValue"" class=""ReadOnlyValue""></span>
                                    </td>
                                </tr>
                                <tr style=""min-height: 17px"">
                                    <td style=""text-align: right;"">
                                        <span id=""lblNewAirInvNumber"" class=""InputLabel"">Нов инв. номер:</span>
                                    </td>
                                    <td style=""text-align: left;"">
                                        <input type=""text"" id=""txtNewAirInvNumber"" onblur=""NewAirInvNumberBlur();"" class=""RequiredInputField"" style=""width: 90px;"" maxlength=""20"" UnsavedCheckSkipMe=""true"" />
                                    </td>
                                </tr>                      
                                <tr style=""height: 35px"">
                                    <td colspan=""2"" style=""padding-top: 5px;"">
                                        <span id=""spanChangeAirInvNumberLightBoxMessage"" class=""ErrorText"" style=""display: none;"">
                                        </span>&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan=""2"" style=""text-align: center;"">
                                        <table style=""margin: 0 auto;"">
                                            <tr>
                                                <td>
                                                    <div id=""btnSaveChangeAirInvNumberLightBox"" style=""display: inline;"" onclick=""SaveChangeAirInvNumberLightBox();""
                                                        class=""Button"">
                                                        <i></i>
                                                        <div id=""btnChangeAirInvNumberLightBoxText"" style=""width: 70px;"">
                                                            Запис</div>
                                                        <b></b>
                                                    </div>
                                                    <div id=""btnCloseChangeAirInvNumberLightBox"" style=""display: inline;"" onclick=""HideChangeAirInvNumberLightBox();""
                                                        class=""Button"">
                                                        <i></i>
                                                        <div id=""btnCloseChangeAirInvNumberLightBox"" style=""width: 70px;"">
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

                    <div id=""divAirInvNumberHistoryLightBox"" style=""display: none;"" class=""lboxRegNumberHistory""></div>
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
                                 page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_ADD") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_ADD_BASICINFO") == UIAccessLevel.Disabled;

                screenHidden = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_ADD") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_ADD_BASICINFO") == UIAccessLevel.Hidden;

                basicInfoDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_ADD") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_ADD_BASICINFO") == UIAccessLevel.Disabled;

                basicInfoHidden = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_ADD") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_ADD_BASICINFO") == UIAccessLevel.Hidden;


                UIAccessLevel l;

                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_ADD_BASICINFO_AIRINVNUMBER");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblAirInvNuber");
                    disabledClientControls.Add("txtAirInvNumber");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAirInvNuber");
                    hiddenClientControls.Add("txtAirInvNumberCont");
                    hiddenClientControls.Add("lblAirInvNumberValue");
                }


            }
            else // edit mode of page
            {
                screenDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_EDIT") == UIAccessLevel.Disabled || isPreview;


                screenHidden = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP") == UIAccessLevel.Hidden ||
                               page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_EDIT") == UIAccessLevel.Hidden;

                basicInfoDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_EDIT") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_EDIT_BASICINFO") == UIAccessLevel.Disabled;

                basicInfoHidden = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_EDIT") == UIAccessLevel.Hidden ||
                                  page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_EDIT_BASICINFO") == UIAccessLevel.Hidden;


                UIAccessLevel l;

                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_EDIT_BASICINFO_AIRINVNUMBER");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblAirInvNuber");
                    disabledClientControls.Add("txtAirInvNumber");
                    hiddenClientControls.Add("imgEditAirInvNumberCont");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAirInvNuber");
                    hiddenClientControls.Add("txtAirInvNumberCont");
                    hiddenClientControls.Add("lblAirInvNumberValueCont");
                    hiddenClientControls.Add("imgEditAirInvNumberCont");
                    hiddenClientControls.Add("imgHistoryAirInvNumberCont");
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


            List<GTableItem> aviationAirKinds = GTableItemUtil.GetAllGTableItemsByTableName("AviationAirKind", page.ModuleKey, 1, 0, 0, page.CurrentUser);
            List<IDropDownItem> ddiAviationAirKinds = new List<IDropDownItem>();
            foreach (GTableItem aviationAirKind in aviationAirKinds)
            {
                ddiAviationAirKinds.Add(aviationAirKind);
            }

            string aviationAirKindsHTML = ListItems.GetDropDownHtml(ddiAviationAirKinds, null, "ddAviationAirKind", true, null, "", "style='width: 280px;'", true);
            string editAviationAirKindsHTML = @"<img id=""imgMaintAviationAirKind"" alt=""Редактиране на списъка"" title=""Редактиране на списъка"" style=""cursor: pointer;"" src=""../Images/list_edit.png"" onclick=""ShowGTable('AviationAirKind', 1, 1, RefreshAviationAirKindList);"" />";

            List<GTableItem> aviationAirTypes = GTableItemUtil.GetAllGTableItemsByTableName("AviationAirType", page.ModuleKey, 1, 0, 0, page.CurrentUser);
            List<IDropDownItem> ddiAviationAirType = new List<IDropDownItem>();
            foreach (GTableItem aviationAirType in aviationAirTypes)
            {
                ddiAviationAirType.Add(aviationAirType);
            }

            string aviationAirTypesHTML = ListItems.GetDropDownHtml(ddiAviationAirType, null, "ddAviationAirType", true, null, "", "style='width: 280px;'", true);
            string editAviationAirTypeHTML = @"<img id=""imgMaintAviationAirType"" alt=""Редактиране на списъка"" title=""Редактиране на списъка"" style=""cursor: pointer;"" src=""../Images/list_edit.png"" onclick=""ShowGTable('AviationAirType', 1, 1, RefreshAviationAirTypeList);"" />";

            //List<GTableItem> aviationAirModels = GTableItemUtil.GetAllGTableItemsByTableName("AviationAirModel", page.ModuleKey, 1, 0, 0, page.CurrentUser);
            //List<IDropDownItem> ddiAviationAirModel = new List<IDropDownItem>();
            //foreach (GTableItem aviationAirModel in aviationAirModels)
            //{
            //    ddiAviationAirModel.Add(aviationAirModel);
            //}

            //string aviationAirModelsHTML = ListItems.GetDropDownHtml(ddiAviationAirModel, null, "ddAviationAirModel", true, null, "", "style='width: 280px;'", true);
            //string editAviationAirModelHTML = @"<img id=""imgMaintAviationAirModel"" alt=""Редактиране на списъка"" title=""Редактиране на списъка"" style=""cursor: pointer;"" src=""../Images/list_edit.png"" onclick=""ShowGTable('AviationAirModel', 1, 1, RefreshAviationAirModelList);"" />";

            List<GTableItem> aviationOtherKinds = GTableItemUtil.GetAllGTableItemsByTableName("AviationOtherKind", page.ModuleKey, 1, 0, 0, page.CurrentUser);
            List<IDropDownItem> ddiAviationOtherKinds = new List<IDropDownItem>();
            foreach (GTableItem aviationOtherKind in aviationOtherKinds)
            {
                ddiAviationOtherKinds.Add(aviationOtherKind);
            }

            string aviationOtherKindsHTML = ListItems.GetDropDownHtml(ddiAviationOtherKinds, null, "ddAviationOtherKind", true, null, "", "style='width: 320px;'", true);
            string editAviationOtherKindsHTML = @"<img id=""imgMaintAviationOtherKind"" alt=""Редактиране на списъка"" title=""Редактиране на списъка"" style=""cursor: pointer;"" src=""../Images/list_edit.png"" onclick=""ShowGTable('AviationOtherKind', 1, 1, RefreshAviationOtherKindList);"" />";

            List<GTableItem> aviationOtherTypes = GTableItemUtil.GetAllGTableItemsByTableName("AviationOtherType", page.ModuleKey, 1, 0, 0, page.CurrentUser);
            List<IDropDownItem> ddiAviationOtherType = new List<IDropDownItem>();
            foreach (GTableItem aviationOtherType in aviationOtherTypes)
            {
                ddiAviationOtherType.Add(aviationOtherType);
            }

            string aviationOtherTypesHTML = ListItems.GetDropDownHtml(ddiAviationOtherType, null, "ddAviationOtherType", true, null, "", "style='width: 320px;'", true);
            string editAviationOtherTypeHTML = @"<img id=""imgMaintAviationOtherType"" alt=""Редактиране на списъка"" title=""Редактиране на списъка"" style=""cursor: pointer;"" src=""../Images/list_edit.png"" onclick=""ShowGTable('AviationOtherType', 1, 1, RefreshAviationOtherTypeList);"" />";

            //List<IDropDownItem> ddiAviationOtherBaseMachineMakes = new List<IDropDownItem>();
            //List<AviationOtherBaseMachineMake> aviationOtherBaseMachineMakes = AviationOtherBaseMachineMakeUtil.GetAllAviationOtherBaseMachineMakes(page.CurrentUser);

            //foreach (AviationOtherBaseMachineMake aviationOtherBaseMachineMake in aviationOtherBaseMachineMakes)
            //{
            //    ddiAviationOtherBaseMachineMakes.Add(aviationOtherBaseMachineMake);
            //}

            //string aviationOtherBaseMachineMakesHTML = ListItems.GetDropDownHtml(ddiAviationOtherBaseMachineMakes, null, "ddAviationOtherBaseMachineMake", true, null, "RepopulateAviationOtherBaseMachineModels(this.value);", "style='width: 320px;'", true);

            //List<IDropDownItem> ddiAviationOtherBaseMachineModels = new List<IDropDownItem>();
            //DropDownItem blankItem = new DropDownItem();
            //blankItem.Txt = ListItems.GetOptionChooseOne().Text;
            //blankItem.Val = ListItems.GetOptionChooseOne().Value;
            //ddiAviationOtherBaseMachineModels.Add(blankItem);

            //string aviationOtherBaseMachineModelsHTML = ListItems.GetDropDownHtml(ddiAviationOtherBaseMachineModels, null, "ddAviationOtherBaseMachineModel", false, null, "", "style='width: 320px;'", true);


            List<GTableItem> aviationOtherBaseMachineKinds = GTableItemUtil.GetAllGTableItemsByTableName("AviationOtherBaseMachineKind", page.ModuleKey, 1, 0, 0, page.CurrentUser);
            List<IDropDownItem> ddiAviationOtherBaseMachineKinds = new List<IDropDownItem>();
            foreach (GTableItem aviationOtherBaseMachineKind in aviationOtherBaseMachineKinds)
            {
                ddiAviationOtherBaseMachineKinds.Add(aviationOtherBaseMachineKind);
            }

            string aviationOtherBaseMachineKindsHTML = ListItems.GetDropDownHtml(ddiAviationOtherBaseMachineKinds, null, "ddAviationOtherBaseMachineKind", true, null, "", "style='width: 320px;'", true);
            string editAviationOtherBaseMachineKindsHTML = @"<img id=""imgMaintAviationOtherBaseMachineKind"" alt=""Редактиране на списъка"" title=""Редактиране на списъка"" style=""cursor: pointer;"" src=""../Images/list_edit.png"" onclick=""ShowGTable('AviationOtherBaseMachineKind', 1, 1, RefreshAviationOtherBaseMachineKindList);"" />";

            List<GTableItem> aviationOtherBaseMachineTypes = GTableItemUtil.GetAllGTableItemsByTableName("AviationOtherBaseMachineType", page.ModuleKey, 1, 0, 0, page.CurrentUser);
            List<IDropDownItem> ddiAviationOtherBaseMachineType = new List<IDropDownItem>();
            foreach (GTableItem aviationOtherBaseMachineType in aviationOtherBaseMachineTypes)
            {
                ddiAviationOtherBaseMachineType.Add(aviationOtherBaseMachineType);
            }

            string aviationOtherBaseMachineTypesHTML = ListItems.GetDropDownHtml(ddiAviationOtherBaseMachineType, null, "ddAviationOtherBaseMachineType", true, null, "", "style='width: 320px;'", true);
            string editAviationOtherBaseMachineTypeHTML = @"<img id=""imgMaintAviationOtherBaseMachineType"" alt=""Редактиране на списъка"" title=""Редактиране на списъка"" style=""cursor: pointer;"" src=""../Images/list_edit.png"" onclick=""ShowGTable('AviationOtherBaseMachineType', 1, 1, RefreshAviationOtherBaseMachineTypeList);"" />";

            List<GTableItem> aviationOtherEquipmentKinds = GTableItemUtil.GetAllGTableItemsByTableName("AviationOtherEquipmentKind", page.ModuleKey, 1, 0, 0, page.CurrentUser);
            List<IDropDownItem> ddiAviationOtherEquipmentKind = new List<IDropDownItem>();
            foreach (GTableItem aviationOtherEquipmentKind in aviationOtherEquipmentKinds)
            {
                ddiAviationOtherEquipmentKind.Add(aviationOtherEquipmentKind);
            }

            string aviationOtherEquipmentKindsHTML = ListItems.GetDropDownHtml(ddiAviationOtherEquipmentKind, null, "ddAviationOtherEquipmentKind", true, null, "", "style='width: 320px;'", true);
            string editAviationOtherEquipmentKindHTML = @"<img id=""imgMaintAviationOtherEquipmentKind"" alt=""Редактиране на списъка"" title=""Редактиране на списъка"" style=""cursor: pointer;"" src=""../Images/list_edit.png"" onclick=""ShowGTable('AviationOtherEquipmentKind', 1, 1, RefreshAviationOtherEquipmentKindList);"" />";


            string html = @"
<div style=""height: 10px;""></div>

<fieldset style=""width: 830px; padding: 0px;"">
   <table class=""InputRegion"" style=""width: 830px; padding: 10px; padding-top: 0px; margin-top: 0px; padding-left: 0px;"">
      <tr style=""height: 3px;"">
      </tr>
      <tr>
         <td style=""text-align: left;"" colspan=""2"">
            <span style=""color: #0B4489; font-weight: bold; font-size: 1.1em; width: 100%; Position: relative; top: -5px;"">Въздухоплавателни средства</span>
         </td>
      </tr>
      <tr>
         
         <td style=""text-align: right; width: 100px;"">
            <span id=""lblTechnicsCategory"" class=""InputLabel"">Категория:</span>
         </td>
         <td style=""text-align: left; width: 350px;"">
            " + techncisCategoriesHTML + @"
         </td>
      </tr>
      <tr>
         <td style=""text-align: right;"">
            <span id=""lblAviationAirKind"" class=""InputLabel"">Вид:</span>
         </td>
         <td style=""text-align: left;"">
            " + aviationAirKindsHTML + editAviationAirKindsHTML + @"
         </td>
         <td style=""text-align: right;"">
            <span id=""lblAviationAirType"" class=""InputLabel"">Тип:</span>
         </td>
         <td style=""text-align: left;"">
            " + aviationAirTypesHTML + editAviationAirTypeHTML + @"
         </td>
      </tr>
      <tr>
         <td style=""text-align: right;"">
            <span id=""lblAviationAirModel"" class=""InputLabel"">Модел:</span>
         </td>
         <td style=""text-align: left;"">
            <input type=""text"" id=""txtAviationAirModelName"" class=""InputField"" style=""width: 100px;"" maxlength=""300"" />
         </td>
         <td style=""text-align: right;"">
            <span id=""lblAirSeats"" class=""InputLabel"">Брой места:</span>
         </td>
         <td style=""text-align: left;"">
            <input type=""text"" id=""txtAirSeats"" class=""InputField"" style=""width: 40px;"" maxlength=""50"" />
            &nbsp;
            <span id=""lblAirCarryingCapacity"" class=""InputLabel"">Товароносимост (т):</span>
            <input type=""text"" id=""txtAirCarryingCapacity"" class=""InputField"" style=""width: 80px;"" maxlength=""50"" />
         </td>
      </tr>
      <tr>
         <td style=""text-align: left;"" colspan=""3"">
            <span id=""lblAirMaxDistance"" class=""InputLabel"">Максимален полет(км):</span>
            <input type=""text"" id=""txtAirMaxDistance"" class=""InputField"" style=""width: 100px;"" maxlength=""50"" />
            <span id=""lblAirLastTechnicalReviewDate"" class=""InputLabel"" style=""float: right; position: relative; top: 3px;"">Дата на последния преглед:</span>
         </td>
         <td style=""text-align: left;"">
            <span id=""txtAirLastTechnicalReviewDateCont""><input type=""text"" id=""txtAirLastTechnicalReviewDate"" class='" + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" /></span>
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
            <span style=""color: #0B4489; font-weight: bold; font-size: 1.1em; width: 100%; Position: relative; top: -5px;"">Друга специализирана техника</span>
         </td>
      </tr>
      <tr>
         <td style=""text-align: left;"" colspan=""2"">
            <span id=""lblOtherInvNumber"" class=""InputLabel"">Инвентарен номер:</span>
            <input type=""text"" id=""txtOtherInvNumber"" onblur=""OtherInvNumberBlur();"" class=""InputField"" style=""width: 100px;"" maxlength=""50"" />
         </td>
      </tr>
      <tr>
         <td style=""text-align: right; width: 50px;"">
            <span id=""lblAviationOtherKind"" class=""InputLabel"">Вид:</span>
         </td>
         <td style=""text-align: left; width: 330px;"" nowrap>
            " + aviationOtherKindsHTML + editAviationOtherKindsHTML + @"
         </td>
         <td style=""text-align: right; width: 50px;"">
            <span id=""lblAviationOtherType"" class=""InputLabel"">Тип:</span>
         </td>
         <td style=""text-align: left;"" nowrap>
            " + aviationOtherTypesHTML + editAviationOtherTypeHTML + @"
         </td>
      </tr>
      <tr>
         <td class=""InputLabel"" colspan=""2"" style=""font-weight: bold; text-decoration: underline; padding-top: 15px; text-align: left;"">Базова машина</td>
      </tr>
      </tr>
         <td colspan=""4"">
            <table>
               <tr>
                  <td style=""text-align: right;"">
                     <span id=""lblAviationOtherBaseMachineMake"" class=""InputLabel"">Марка:</span>
                  </td>
                  <td style=""text-align: left;"">
                     <input type=""text"" id=""txtAviationOtherBaseMachineMakeName"" class=""InputField"" style=""width: 314px;"" maxlength=""300"" />
                  </td>
                  <td style=""text-align: right; width: 60px;"">
                     <span id=""lblAviationOtherBaseMachineModel"" class=""InputLabel"">Модел:</span>
                  </td>
                  <td style=""text-align: left;"">
                     <input type=""text"" id=""txtAviationOtherBaseMachineModelName"" class=""InputField"" style=""width: 314px;"" maxlength=""300"" />
                  </td>
               </tr>
               <tr>
                  <td style=""text-align: right;"">
                     <span id=""lblAviationOtherBaseMachineKind"" class=""InputLabel"">Вид:</span>
                  </td>
                  <td style=""text-align: left;"">
                     " + aviationOtherBaseMachineKindsHTML + editAviationOtherBaseMachineKindsHTML + @"
                  </td>
                  <td style=""text-align: right;"">
                     <span id=""lblAviationOtherBaseMachineType"" class=""InputLabel"">Тип:</span>
                  </td>
                  <td style=""text-align: left;"">
                     " + aviationOtherBaseMachineTypesHTML + editAviationOtherBaseMachineTypeHTML + @"
                  </td>
               </tr>
               <tr>
                  <td style=""text-align: right;"" colspan=""3"">
                     <span id=""lblBaseMachineMileageHoursSinceLastRepair"" class=""InputLabel"">Километри/часове от последния ремонт:</span>
                  </td>
                  <td style=""text-align: left;"">
                     <input type=""text"" id=""txtBaseMachineMileageHoursSinceLastRepair"" class=""InputField"" style=""width: 100px;"" maxlength=""50"" />
                  </td>
               </tr>
            </table>
         </td>
      </tr>
      <tr>
         <td colspan=""2"" class=""InputLabel"" style=""font-weight: bold; text-decoration: underline; padding-top: 15px; text-align: left;"">Оборудване</td>
      </tr>
      </tr>
         <td colspan=""4"">
            <table>
               <tr>
                  <td style=""text-align: right; width: 46px;"">
                     <span id=""lblAviationOtherEquipmentKind"" class=""InputLabel"">Вид:</span>
                  </td>
                  <td style=""text-align: left; width: 325px;"" nowrap>
                     " + aviationOtherEquipmentKindsHTML + editAviationOtherEquipmentKindHTML + @"
                  </td>
                  <td style=""text-align: right; width: 300px;"">
                     <span id=""lblEquipMileageHourSinceLstRepair"" class=""InputLabel"">Километри/часове от последния ремонт:</span>
                  </td>
                  <td style=""text-align: left; width: 90px;"">
                     <input type=""text"" id=""txtEquipMileageHourSinceLstRepair"" class=""InputField"" style=""width: 100px;"" maxlength=""50"" />
                  </td>
               </tr>
            </table>
         </td>
      </tr>
   </table>
</fieldset>

<div style=""height: 10px;""></div>

<input type=""hidden"" id=""hdnAviationEquipId"" />
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
                                 page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_ADD") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_ADD_BASICINFO") == UIAccessLevel.Disabled;

                basicInfoDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_ADD") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_ADD_BASICINFO") == UIAccessLevel.Disabled;
                                
                UIAccessLevel l;

                
                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_ADD_BASICINFO_TECHNICSCATEGORY");

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

                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_ADD_BASICINFO_AVIATIONAIRKIND");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblAviationAirKind");
                    disabledClientControls.Add("ddAviationAirKind");
                    hiddenClientControls.Add("imgMaintAviationAirKind");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAviationAirKind");
                    hiddenClientControls.Add("ddAviationAirKind");
                    hiddenClientControls.Add("imgMaintAviationAirKind");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_ADD_BASICINFO_AVIATIONAIRTYPE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblAviationAirType");
                    disabledClientControls.Add("ddAviationAirType");
                    hiddenClientControls.Add("imgMaintAviationAirType");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAviationAirType");
                    hiddenClientControls.Add("ddAviationAirType");
                    hiddenClientControls.Add("imgMaintAviationAirType");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_ADD_BASICINFO_AVIATIONAIRMODEL");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblAviationAirModel");
                    disabledClientControls.Add("txtAviationAirModelName");
                    //hiddenClientControls.Add("imgMaintAviationAirModel");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAviationAirModel");
                    hiddenClientControls.Add("txtAviationAirModelName");
                    //hiddenClientControls.Add("imgMaintAviationAirModel");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_ADD_BASICINFO_AIRSEATS");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblAirSeats");
                    disabledClientControls.Add("txtAirSeats");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAirSeats");
                    hiddenClientControls.Add("txtAirSeats");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_ADD_BASICINFO_AIRCARRYINGCAPACITY");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblAirCarryingCapacity");
                    disabledClientControls.Add("txtAirCarryingCapacity");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAirCarryingCapacity");
                    hiddenClientControls.Add("txtAirCarryingCapacity");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_ADD_BASICINFO_AIRMAXDISTANCE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblAirMaxDistance");
                    disabledClientControls.Add("txtAirMaxDistance");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAirMaxDistance");
                    hiddenClientControls.Add("txtAirMaxDistance");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_ADD_BASICINFO_AIRLASTTECHNICALREVIEWDATE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblAirLastTechnicalReviewDate");
                    disabledClientControls.Add("txtAirLastTechnicalReviewDate");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAirLastTechnicalReviewDate");
                    hiddenClientControls.Add("txtAirLastTechnicalReviewDateCont");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_ADD_BASICINFO_OTHERINVNUMBER");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblOtherInvNumber");
                    disabledClientControls.Add("txtOtherInvNumber");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblOtherInvNumber");
                    hiddenClientControls.Add("txtOtherInvNumber");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_ADD_BASICINFO_AVIATIONOTHERKIND");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblAviationOtherKind");
                    disabledClientControls.Add("ddAviationOtherKind");
                    hiddenClientControls.Add("imgMaintAviationOtherKind");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAviationOtherKind");
                    hiddenClientControls.Add("ddAviationOtherKind");
                    hiddenClientControls.Add("imgMaintAviationOtherKind");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_ADD_BASICINFO_AVIATIONOTHERTYPE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblAviationOtherType");
                    disabledClientControls.Add("ddAviationOtherType");
                    hiddenClientControls.Add("imgMaintAviationOtherType");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAviationOtherType");
                    hiddenClientControls.Add("ddAviationOtherType");
                    hiddenClientControls.Add("imgMaintAviationOtherType");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_ADD_BASICINFO_AVIATIONOTHERBASEMACHINEMAKE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblAviationOtherBaseMachineMake");
                    disabledClientControls.Add("txtAviationOtherBaseMachineMakeName");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAviationOtherBaseMachineMake");
                    hiddenClientControls.Add("txtAviationOtherBaseMachineMakeName");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_ADD_BASICINFO_AVIATIONOTHERBASEMACHINEMODEL");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblAviationOtherBaseMachineModel");
                    disabledClientControls.Add("txtAviationOtherBaseMachineModelName");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAviationOtherBaseMachineModel");
                    hiddenClientControls.Add("txtAviationOtherBaseMachineModelName");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_ADD_BASICINFO_AVIATIONOTHERBASEMACHINEKIND");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblAviationOtherBaseMachineKind");
                    disabledClientControls.Add("ddAviationOtherBaseMachineKind");
                    hiddenClientControls.Add("imgMaintAviationOtherBaseMachineKind");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAviationOtherBaseMachineKind");
                    hiddenClientControls.Add("ddAviationOtherBaseMachineKind");
                    hiddenClientControls.Add("imgMaintAviationOtherBaseMachineKind");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_ADD_BASICINFO_AVIATIONOTHERBASEMACHINETYPE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblAviationOtherBaseMachineType");
                    disabledClientControls.Add("ddAviationOtherBaseMachineType");
                    hiddenClientControls.Add("imgMaintAviationOtherBaseMachineType");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAviationOtherBaseMachineType");
                    hiddenClientControls.Add("ddAviationOtherBaseMachineType");
                    hiddenClientControls.Add("imgMaintAviationOtherBaseMachineType");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_ADD_BASICINFO_BASEMACHINEMILEAGEHOURSSINCELASTREPAIR");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblBaseMachineMileageHoursSinceLastRepair");
                    disabledClientControls.Add("txtBaseMachineMileageHoursSinceLastRepair");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblBaseMachineMileageHoursSinceLastRepair");
                    hiddenClientControls.Add("txtBaseMachineMileageHoursSinceLastRepair");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_ADD_BASICINFO_AVIATIONOTHEREQUIPMENTKIND");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblAviationOtherEquipmentKind");
                    disabledClientControls.Add("ddAviationOtherEquipmentKind");
                    hiddenClientControls.Add("imgMaintAviationOtherEquipmentKind");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAviationOtherEquipmentKind");
                    hiddenClientControls.Add("ddAviationOtherEquipmentKind");
                    hiddenClientControls.Add("imgMaintAviationOtherEquipmentKind");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_ADD_BASICINFO_EQUIPMILEAGEHOURSINCELSTREPAIR");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblEquipMileageHourSinceLstRepair");
                    disabledClientControls.Add("txtEquipMileageHourSinceLstRepair");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEquipMileageHourSinceLstRepair");
                    hiddenClientControls.Add("txtEquipMileageHourSinceLstRepair");
                }
            }
            else // edit mode of page
            {
                screenDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP") == UIAccessLevel.Disabled ||
                                 page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_EDIT") == UIAccessLevel.Disabled || isPreview;

                basicInfoDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_EDIT") == UIAccessLevel.Disabled ||
                                    page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_EDIT_BASICINFO") == UIAccessLevel.Disabled;

                
                UIAccessLevel l;

                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_EDIT_BASICINFO_TECHNICSCATEGORY");

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

                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_EDIT_BASICINFO_AVIATIONAIRKIND");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblAviationAirKind");
                    disabledClientControls.Add("ddAviationAirKind");
                    hiddenClientControls.Add("imgMaintAviationAirKind");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAviationAirKind");
                    hiddenClientControls.Add("ddAviationAirKind");
                    hiddenClientControls.Add("imgMaintAviationAirKind");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_EDIT_BASICINFO_AVIATIONAIRTYPE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblAviationAirType");
                    disabledClientControls.Add("ddAviationAirType");
                    hiddenClientControls.Add("imgMaintAviationAirType");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAviationAirType");
                    hiddenClientControls.Add("ddAviationAirType");
                    hiddenClientControls.Add("imgMaintAviationAirType");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_EDIT_BASICINFO_AVIATIONAIRMODEL");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblAviationAirModel");
                    disabledClientControls.Add("txtAviationAirModelName");
                    //hiddenClientControls.Add("imgMaintAviationAirModel");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAviationAirModel");
                    hiddenClientControls.Add("txtAviationAirModelName");
                    //hiddenClientControls.Add("imgMaintAviationAirModel");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_EDIT_BASICINFO_AIRSEATS");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblAirSeats");
                    disabledClientControls.Add("txtAirSeats");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAirSeats");
                    hiddenClientControls.Add("txtAirSeats");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_EDIT_BASICINFO_AIRCARRYINGCAPACITY");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblAirCarryingCapacity");
                    disabledClientControls.Add("txtAirCarryingCapacity");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAirCarryingCapacity");
                    hiddenClientControls.Add("txtAirCarryingCapacity");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_EDIT_BASICINFO_AIRMAXDISTANCE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblAirMaxDistance");
                    disabledClientControls.Add("txtAirMaxDistance");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAirMaxDistance");
                    hiddenClientControls.Add("txtAirMaxDistance");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_EDIT_BASICINFO_AIRLASTTECHNICALREVIEWDATE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblAirLastTechnicalReviewDate");
                    disabledClientControls.Add("txtAirLastTechnicalReviewDate");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAirLastTechnicalReviewDate");
                    hiddenClientControls.Add("txtAirLastTechnicalReviewDateCont");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_EDIT_BASICINFO_OTHERINVNUMBER");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblOtherInvNumber");
                    disabledClientControls.Add("txtOtherInvNumber");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblOtherInvNumber");
                    hiddenClientControls.Add("txtOtherInvNumber");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_EDIT_BASICINFO_AVIATIONOTHERKIND");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblAviationOtherKind");
                    disabledClientControls.Add("ddAviationOtherKind");
                    hiddenClientControls.Add("imgMaintAviationOtherKind");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAviationOtherKind");
                    hiddenClientControls.Add("ddAviationOtherKind");
                    hiddenClientControls.Add("imgMaintAviationOtherKind");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_EDIT_BASICINFO_AVIATIONOTHERTYPE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblAviationOtherType");
                    disabledClientControls.Add("ddAviationOtherType");
                    hiddenClientControls.Add("imgMaintAviationOtherType");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAviationOtherType");
                    hiddenClientControls.Add("ddAviationOtherType");
                    hiddenClientControls.Add("imgMaintAviationOtherType");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_EDIT_BASICINFO_AVIATIONOTHERBASEMACHINEMAKE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblAviationOtherBaseMachineMake");
                    disabledClientControls.Add("txtAviationOtherBaseMachineMakeName");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAviationOtherBaseMachineMake");
                    hiddenClientControls.Add("txtAviationOtherBaseMachineMakeName");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_EDIT_BASICINFO_AVIATIONOTHERBASEMACHINEMODEL");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblAviationOtherBaseMachineModel");
                    disabledClientControls.Add("txtAviationOtherBaseMachineModelName");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAviationOtherBaseMachineModel");
                    hiddenClientControls.Add("txtAviationOtherBaseMachineModelName");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_EDIT_BASICINFO_AVIATIONOTHERBASEMACHINEKIND");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblAviationOtherBaseMachineKind");
                    disabledClientControls.Add("ddAviationOtherBaseMachineKind");
                    hiddenClientControls.Add("imgMaintAviationOtherBaseMachineKind");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAviationOtherBaseMachineKind");
                    hiddenClientControls.Add("ddAviationOtherBaseMachineKind");
                    hiddenClientControls.Add("imgMaintAviationOtherBaseMachineKind");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_EDIT_BASICINFO_AVIATIONOTHERBASEMACHINETYPE");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblAviationOtherBaseMachineType");
                    disabledClientControls.Add("ddAviationOtherBaseMachineType");
                    hiddenClientControls.Add("imgMaintAviationOtherBaseMachineType");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAviationOtherBaseMachineType");
                    hiddenClientControls.Add("ddAviationOtherBaseMachineType");
                    hiddenClientControls.Add("imgMaintAviationOtherBaseMachineType");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_EDIT_BASICINFO_BASEMACHINEMILEAGEHOURSSINCELASTREPAIR");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblBaseMachineMileageHoursSinceLastRepair");
                    disabledClientControls.Add("txtBaseMachineMileageHoursSinceLastRepair");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblBaseMachineMileageHoursSinceLastRepair");
                    hiddenClientControls.Add("txtBaseMachineMileageHoursSinceLastRepair");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_EDIT_BASICINFO_AVIATIONOTHEREQUIPMENTKIND");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblAviationOtherEquipmentKind");
                    disabledClientControls.Add("ddAviationOtherEquipmentKind");
                    hiddenClientControls.Add("imgMaintAviationOtherEquipmentKind");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAviationOtherEquipmentKind");
                    hiddenClientControls.Add("ddAviationOtherEquipmentKind");
                    hiddenClientControls.Add("imgMaintAviationOtherEquipmentKind");
                }

                l = page.GetUIItemAccessLevel("RES_TECHNICS_AVIATION_EQUIP_EDIT_BASICINFO_EQUIPMILEAGEHOURSINCELSTREPAIR");

                if (l == UIAccessLevel.Disabled || screenDisabled || basicInfoDisabled)
                {
                    disabledClientControls.Add("lblEquipMileageHourSinceLstRepair");
                    disabledClientControls.Add("txtEquipMileageHourSinceLstRepair");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEquipMileageHourSinceLstRepair");
                    hiddenClientControls.Add("txtEquipMileageHourSinceLstRepair");
                }
            }

            if (page.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled ||
                page.GetUIItemAccessLevel("RES_LISTMAINT_AVIATIONAIRKIND") != UIAccessLevel.Enabled)
            {
                hiddenClientControls.Add("imgMaintAviationAirKind");
            }

            if (page.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled ||
                page.GetUIItemAccessLevel("RES_LISTMAINT_AVIATIONAIRTYPE") != UIAccessLevel.Enabled)
            {
                hiddenClientControls.Add("imgMaintAviationAirType");
            }

            if (page.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled ||
                page.GetUIItemAccessLevel("RES_LISTMAINT_AVIATIONAIRMODEL") != UIAccessLevel.Enabled)
            {
                hiddenClientControls.Add("imgMaintAviationAirModel");
            }

            if (page.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled ||
                page.GetUIItemAccessLevel("RES_LISTMAINT_AVIATIONOTHERKIND") != UIAccessLevel.Enabled)
            {
                hiddenClientControls.Add("imgMaintAviationOtherKind");
            }

            if (page.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled ||
                page.GetUIItemAccessLevel("RES_LISTMAINT_AVIATIONOTHERTYPE") != UIAccessLevel.Enabled)
            {
                hiddenClientControls.Add("imgMaintAviationOtherType");
            }

            if (page.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled ||
                page.GetUIItemAccessLevel("RES_LISTMAINT_AVIATIONOTHERBASEMACHINEKIND") != UIAccessLevel.Enabled)
            {
                hiddenClientControls.Add("imgMaintAviationOtherBaseMachineKind");
            }

            if (page.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled ||
                page.GetUIItemAccessLevel("RES_LISTMAINT_AVIATIONOTHERBASEMACHINETYPE") != UIAccessLevel.Enabled)
            {
                hiddenClientControls.Add("imgMaintAviationOtherBaseMachineType");
            }

            if (page.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled ||
                page.GetUIItemAccessLevel("RES_LISTMAINT_AVIATIONOTHEREQUIPMENTKIND") != UIAccessLevel.Enabled)
            {
                hiddenClientControls.Add("imgMaintAviationOtherEquipmentKind");
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
