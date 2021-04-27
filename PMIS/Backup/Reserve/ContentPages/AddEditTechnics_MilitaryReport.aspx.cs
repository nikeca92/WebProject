using System;
using System.Collections.Generic;
using System.Linq;

using PMIS.Common;
using PMIS.Reserve.Common;
using System.Web;
using System.Text;
using System.IO;
using System.Web.UI;

namespace PMIS.Reserve.ContentPages
{
    public partial class AddEditTechnics_MilitaryReport : RESPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadNewMilRepStatuses")
            {
                JSLoadNewMilRepStatuses();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadTechnicsMilRepStatus")
            {
                JSLoadTechnicsMilRepStatus();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveMilRepStatus")
            {
                JSSaveMilRepStatus();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadMilRepStatusHistory")
            {
                JSLoadMilRepStatusHistory();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadTechnicsAppointmentHistory")
            {
                JSLoadTechnicsAppointmentHistory();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveMilitaryReportTab")
            {
                JSSaveMilitaryReportTab();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSPrintMK")
            {
                JSPrintMK();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSPrintPZ")
            {
                JSPrintPZ();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSPrintOK")
            {
                JSPrintOK();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSPrintTO")
            {
                JSPrintTO();
                return;
            }
        }

        //Get the available new statuses for a particular Technics (ajax call)
        private void JSLoadNewMilRepStatuses()
        {
            int technicsId = int.Parse(Request.Form["TechnicsId"]);

            string technicsTypeKey = TechnicsUtil.GetTechnics(technicsId, CurrentUser).TechnicsType.TypeKey;

            if (GetUIItemAccessLevel("RES_TECHNICS_" + technicsTypeKey + "_EDIT_MILREP_MILREPSTATUS_MILREPSTATUS") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                
                TechnicsMilRepStatus currentTechMilRepStatus = TechnicsMilRepStatusUtil.GetTechnicsMilRepCurrentStatusByTechnicsId(technicsId, CurrentUser);

                List<TechMilitaryReportStatus> allStatuses = TechMilitaryReportStatusUtil.GetAllTechMilitaryReportStatuses(CurrentUser);

                //If there is a current status then filter the statuses
                if (currentTechMilRepStatus != null)
                {
                    string currTechMilRepStatus = currentTechMilRepStatus.TechMilitaryReportStatus.TechMilitaryReportStatusKey;

                    //Remove the current status from the list
                    allStatuses.RemoveAll(x => x.TechMilitaryReportStatusKey == currTechMilRepStatus);
                }

                allStatuses.RemoveAll(x => x.TechMilitaryReportStatusKey == "MOBILE_APPOINTMENT");

                response = "<statuses>";

                response += "<s>" +
                            "<id>" + ListItems.GetOptionChooseOne().Value + "</id>" +
                            "<key>" + "" + "</key>" +
                            "<name>" + AJAXTools.EncodeForXML(ListItems.GetOptionChooseOne().Text) + "</name>" +
                            "</s>";

                foreach (TechMilitaryReportStatus status in allStatuses)
                {
                    response += "<s>" +
                                "<id>" + status.TechMilitaryReportStatusId.ToString() + "</id>" +
                                "<key>" + status.TechMilitaryReportStatusKey + "</key>" +
                                "<name>" + AJAXTools.EncodeForXML(status.TechMilitaryReportStatusName) + "</name>" +
                                "</s>";
                }

                response += "<mildeptid>" + (currentTechMilRepStatus != null ? currentTechMilRepStatus.SourceMilDepartmentId.ToString() : "-1") + "</mildeptid>";

                response += "</statuses>";

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

        private void JSLoadTechnicsMilRepStatus()
        {
            string stat = "";
            string response = "";

            try
            {
                int technicsMilRepStatusId = int.Parse(Request.Form["TechnicsMilRepStatusId"]);

                TechnicsMilRepStatus technicsMilRepStatus = TechnicsMilRepStatusUtil.GetTechnicsMilRepStatus(technicsMilRepStatusId, CurrentUser);

                response = "<response>";

                response += "<technicsMilRepStatus>";
                response += "<MilitaryReportStatusID>" + technicsMilRepStatus.TechMilitaryReportStatusId + "</MilitaryReportStatusID>";
                response += "<MilitaryReportStatusKey>" + technicsMilRepStatus.TechMilitaryReportStatus.TechMilitaryReportStatusKey + "</MilitaryReportStatusKey>";
                response += "<EnrolDate>" + CommonFunctions.FormatDate(technicsMilRepStatus.EnrolDate) + "</EnrolDate>";
                response += "<DischargeDate>" + CommonFunctions.FormatDate(technicsMilRepStatus.DischargeDate) + "</DischargeDate>";
                response += "<SourceMilDepartmentID>" + technicsMilRepStatus.SourceMilDepartmentId + "</SourceMilDepartmentID>";
                response += "<ContractContractNumber>" + AJAXTools.EncodeForXML(technicsMilRepStatus.Contract_ContractNumber) + "</ContractContractNumber>";
                response += "<ContractMilitaryUnitID>" + technicsMilRepStatus.Contract_MilitaryUnitID + "</ContractMilitaryUnitID>";
                response += "<ContractMilitaryUnitName>" + AJAXTools.EncodeForXML(technicsMilRepStatus.Contract_MilitaryUnit != null ? technicsMilRepStatus.Contract_MilitaryUnit.DisplayTextForSelection : "") + "</ContractMilitaryUnitName>";
                response += "<ContractContractFromDate>" + CommonFunctions.FormatDate(technicsMilRepStatus.Contract_ContractFromDate) + "</ContractContractFromDate>";
                response += "<ContractContractToDate>" + CommonFunctions.FormatDate(technicsMilRepStatus.Contract_ContractToDate) + "</ContractContractToDate>"; response += "<VoluntaryContractNumber>" + AJAXTools.EncodeForXML(technicsMilRepStatus.Voluntary_ContractNumber) + "</VoluntaryContractNumber>";
                response += "<VoluntaryContractDate>" + CommonFunctions.FormatDate(technicsMilRepStatus.Voluntary_ContractDate) + "</VoluntaryContractDate>";
                response += "<VoluntaryDurationMonths>" + technicsMilRepStatus.Voluntary_DurationMonths + "</VoluntaryDurationMonths>";
                response += "<VoluntaryContractToDate>" + CommonFunctions.FormatDate(technicsMilRepStatus.Voluntary_ContractToDate) + "</VoluntaryContractToDate>";
                response += "<VoluntaryFulfilPlaceID>" + (technicsMilRepStatus.Voluntary_FulfilPlaceID != null ? technicsMilRepStatus.Voluntary_FulfilPlaceID.Value.ToString() : "-1") + "</VoluntaryFulfilPlaceID>";
                response += "<VoluntaryFulfilPlaceText>" + (technicsMilRepStatus.Voluntary_FulfilPlaceID != null ? technicsMilRepStatus.Voluntary_FulfilPlace.DisplayTextForSelection : "") + "</VoluntaryFulfilPlaceText>";
                response += "<RemovedDate>" + CommonFunctions.FormatDate(technicsMilRepStatus.Removed_Date) + "</RemovedDate>";
                response += "<RemovedReasonID>" + technicsMilRepStatus.Removed_ReasonId + "</RemovedReasonID>";
                response += "<TemporaryRemovedReasonID>" + technicsMilRepStatus.TemporaryRemoved_ReasonId + "</TemporaryRemovedReasonID>";
                response += "<TemporaryRemovedDate>" + CommonFunctions.FormatDate(technicsMilRepStatus.TemporaryRemoved_Date) + "</TemporaryRemovedDate>";
                response += "<TemporaryRemovedDuration>" + technicsMilRepStatus.TemporaryRemoved_Duration + "</TemporaryRemovedDuration>";
                response += "<TechnicsPostponeTypeID>" + technicsMilRepStatus.TechnicsPostpone_TypeId + "</TechnicsPostponeTypeID>";
                response += "<TechnicsPostponeYear>" + technicsMilRepStatus.TechnicsPostpone_Year + "</TechnicsPostponeYear>";
                response += "</technicsMilRepStatus>";           

                response += "</response>";

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

        // Save Military Report Status by ajax request
        private void JSSaveMilRepStatus()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int technicsID = int.Parse(Request.Form["TechnicsID"]);
                int technicsMilRepStatusID = int.Parse(Request.Form["TechnicsMilRepStatusID"]);
                string militaryReportStatusKey = Request.Form["MilitaryReportStatusKey"];

                Technics technics = TechnicsUtil.GetTechnics(technicsID, CurrentUser);

                TechnicsMilRepStatus technicsMilRepStatus = new TechnicsMilRepStatus(CurrentUser);

                technicsMilRepStatus.TechnicsMilRepStatusId = technicsMilRepStatusID;
                technicsMilRepStatus.TechnicsId = technicsID;
                technicsMilRepStatus.IsCurrent = true;

                // if there is change in PostponeYear, when the status is POSTPONED, then automatically save new status
                if (militaryReportStatusKey == "POSTPONED")
                {
                    TechnicsMilRepStatus oldTechnicsMilRepStatus = TechnicsMilRepStatusUtil.GetTechnicsMilRepCurrentStatusByTechnicsId(technicsID, CurrentUser);

                    int? newPostponeYear = (!String.IsNullOrEmpty(Request.Form["PostponeYear"]) ? (int?)int.Parse(Request.Form["PostponeYear"]) : null);

                    if (oldTechnicsMilRepStatus != null &&
                        oldTechnicsMilRepStatus.TechMilitaryReportStatus.TechMilitaryReportStatusKey == "POSTPONED" &&
                        oldTechnicsMilRepStatus.TechnicsPostpone_Year != newPostponeYear)
                        oldTechnicsMilRepStatus.TechnicsMilRepStatusId = 0;
                }

                if (!String.IsNullOrEmpty(Request.Form["MilitaryReportStatusID"]) && Request.Form["MilitaryReportStatusID"] != ListItems.GetOptionChooseOne().Value)
                    technicsMilRepStatus.TechMilitaryReportStatusId = int.Parse(Request.Form["MilitaryReportStatusID"]);

                technicsMilRepStatus.EnrolDate = (!String.IsNullOrEmpty(Request.Form["EnrolDate"]) ? CommonFunctions.ParseDate(Request.Form["EnrolDate"]) : (DateTime?)null);
                technicsMilRepStatus.DischargeDate = (!String.IsNullOrEmpty(Request.Form["DischargeDate"]) ? CommonFunctions.ParseDate(Request.Form["DischargeDate"]) : (DateTime?)null);

                if (!String.IsNullOrEmpty(Request.Form["SourceMilDepartmentID"]) && Request.Form["SourceMilDepartmentID"] != ListItems.GetOptionChooseOne().Value)
                    technicsMilRepStatus.SourceMilDepartmentId = int.Parse(Request.Form["SourceMilDepartmentID"]);

                switch (militaryReportStatusKey)
                {
                    case "CONTRACT":
                        technicsMilRepStatus.Contract_ContractNumber = Request.Form["ContractContractNumber"];
                        technicsMilRepStatus.Contract_ContractFromDate = (!String.IsNullOrEmpty(Request.Form["ContractContractFromDate"]) ? CommonFunctions.ParseDate(Request.Form["ContractContractFromDate"]) : (DateTime?)null);
                        technicsMilRepStatus.Contract_ContractToDate = (!String.IsNullOrEmpty(Request.Form["ContractContractToDate"]) ? CommonFunctions.ParseDate(Request.Form["ContractContractToDate"]) : (DateTime?)null);
                        if (!String.IsNullOrEmpty(Request.Form["ContractMilitaryUnitID"]) && Request.Form["ContractMilitaryUnitID"] != ListItems.GetOptionChooseOne().Value)
                            technicsMilRepStatus.Contract_MilitaryUnitID = int.Parse(Request.Form["ContractMilitaryUnitID"]);
                        break;
                    case "VOLUNTARY_RESERVE":
                        technicsMilRepStatus.Voluntary_ContractNumber = Request.Form["VoluntaryContractNumber"];
                        technicsMilRepStatus.Voluntary_ContractDate = (!String.IsNullOrEmpty(Request.Form["VoluntaryContractDate"]) ? CommonFunctions.ParseDate(Request.Form["VoluntaryContractDate"]) : (DateTime?)null);
                        technicsMilRepStatus.Voluntary_DurationMonths = (!String.IsNullOrEmpty(Request.Form["VoluntaryDurationMonths"]) ? (int?)int.Parse(Request.Form["VoluntaryDurationMonths"]) : null);
                        technicsMilRepStatus.Voluntary_ContractToDate = (!String.IsNullOrEmpty(Request.Form["VoluntaryContractToDate"]) ? CommonFunctions.ParseDate(Request.Form["VoluntaryContractToDate"]) : (DateTime?)null);
                        
                        if (int.Parse(Request.Form["VoluntaryFulfilPlaceID"]) != -1)
                            technicsMilRepStatus.Voluntary_FulfilPlaceID = int.Parse(Request.Form["VoluntaryFulfilPlaceID"]);

                        break;
                    case "REMOVED":
                        technicsMilRepStatus.Removed_Date = (!String.IsNullOrEmpty(Request.Form["RemovedDate"]) ? CommonFunctions.ParseDate(Request.Form["RemovedDate"]) : (DateTime?)null);
                        if (!String.IsNullOrEmpty(Request.Form["RemovedReasonID"]) && Request.Form["RemovedReasonID"] != ListItems.GetOptionChooseOne().Value)
                            technicsMilRepStatus.Removed_ReasonId = int.Parse(Request.Form["RemovedReasonID"]);
                        break;                    
                    case "TEMPORARY_REMOVED":
                        if (!String.IsNullOrEmpty(Request.Form["TemporaryRemovedReasonID"]) && Request.Form["TemporaryRemovedReasonID"] != ListItems.GetOptionChooseOne().Value)
                            technicsMilRepStatus.TemporaryRemoved_ReasonId = int.Parse(Request.Form["TemporaryRemovedReasonID"]);
                        technicsMilRepStatus.TemporaryRemoved_Date = (!String.IsNullOrEmpty(Request.Form["TemporaryRemovedDate"]) ? CommonFunctions.ParseDate(Request.Form["TemporaryRemovedDate"]) : (DateTime?)null);
                        if (!String.IsNullOrEmpty(Request.Form["TemporaryRemovedDuration"]) && Request.Form["TemporaryRemovedDuration"] != ListItems.GetOptionChooseOne().Value)
                            technicsMilRepStatus.TemporaryRemoved_Duration = int.Parse(Request.Form["TemporaryRemovedDuration"]);
                        break;
                    case "POSTPONED":
                        if (!String.IsNullOrEmpty(Request.Form["TechnicsPostponeTypeID"]) && Request.Form["TechnicsPostponeTypeID"] != ListItems.GetOptionChooseOne().Value)
                            technicsMilRepStatus.TechnicsPostpone_TypeId = int.Parse(Request.Form["TechnicsPostponeTypeID"]);
                        technicsMilRepStatus.TechnicsPostpone_Year = (!String.IsNullOrEmpty(Request.Form["TechnicsPostponeYear"]) ? (int?)int.Parse(Request.Form["TechnicsPostponeYear"]) : null);                        
                        break;
                }

                //Track the changes into the Audit Trail 
                Change change = new Change(CurrentUser, "RES_Technics_" + technics.TechnicsType.TypeKey);

                bool modifiedMilitaryDepartment = false;

                // check for changes in SourceMilitaryDepartment and clear technics Punkt field
                if (technicsMilRepStatusID != 0)
                {
                    TechnicsMilRepStatus oldTechnicsMilRepStatus = TechnicsMilRepStatusUtil.GetTechnicsMilRepStatus(technicsMilRepStatusID, CurrentUser);

                    if (oldTechnicsMilRepStatus.SourceMilDepartmentId != technicsMilRepStatus.SourceMilDepartmentId)
                    {
                        modifiedMilitaryDepartment = true;

                        technics.PunktID = null;

                        string logDescription = TechnicsUtil.GetTechnicsLogDescription(technicsID, CurrentUser);

                        ChangeEvent changeEvent = new ChangeEvent("RES_Technics_EditMilRep", logDescription, null, null, CurrentUser);

                        TechnicsUtil.SaveTechnics_WhenEditingMilitaryReportTab(technics, CurrentUser, changeEvent);
                    }
                }

                //When changing the current Military Report Status of particular technics then
                //remove that technics from any Equipment Requests and also clear the current Mobilization Appointment, if any
                if (modifiedMilitaryDepartment ||
                    technicsMilRepStatusID == 0)
                {
                    List<FillTechnicsRequest> fillTechnicsRequests = FillTechnicsRequestUtil.GetFillTechnicsRequestByTechnicsId(technicsID, CurrentUser);

                    //Remove the Technics from each Equipment Request
                    foreach (FillTechnicsRequest fillTechnicsRequest in fillTechnicsRequests)
                    {
                        MilitaryDepartment militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment(fillTechnicsRequest.MilitaryDepartmentID, CurrentUser);

                        TechnicsRequestCommandPosition position = TechnicsRequestCommandPositionUtil.GetTechnicsRequestCommandPosition(CurrentUser, fillTechnicsRequest.TechnicsRequestCommandPositionID);

                        string logDescription = "";

                        logDescription += "Заявка №: " + position.TechnicsRequestsCommand.EquipmentTechnicsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(position.TechnicsRequestsCommand.EquipmentTechnicsRequest.RequestDate) +
                                                  "; Команда: " + position.TechnicsRequestsCommand.MilitaryCommand.DisplayTextForSelection +
                                                  "; ВО: " + militaryDepartment.MilitaryDepartmentName +
                                                  "; Вид резерв: " + fillTechnicsRequest.TechnicReadiness +
                                                  "; Вид техника: " + position.TechnicsType.TypeName +
                                                  "; Коментар: " + position.Comment;

                        ChangeEvent changeEvent = null;

                        switch (technics.TechnicsType.TypeKey)
                        {
                            case "VEHICLES":
                                Vehicle vehicle = VehicleUtil.GetVehicleByTechnicsId(technicsID, CurrentUser);
                                logDescription += "; Рег. №: " + vehicle.RegNumber;
                                changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteVehicle", logDescription, position.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, CurrentUser);

                                break;
                            case "TRAILERS":
                                Trailer trailer = TrailerUtil.GetTrailerByTechnicsId(technicsID, CurrentUser);
                                logDescription += "; Рег. №: " + trailer.RegNumber;
                                changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteTrailer", logDescription, position.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, CurrentUser);

                                break;
                            case "TRACTORS":
                                Tractor tractor = TractorUtil.GetTractorByTechnicsId(technicsID, CurrentUser);
                                logDescription += "; Рег. №: " + tractor.RegNumber;
                                changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteTractor", logDescription, position.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, CurrentUser);

                                break;
                            case "ENG_EQUIP":
                                EngEquip engEquip = EngEquipUtil.GetEngEquipByTechnicsId(technicsID, CurrentUser);
                                logDescription += "; Рег. №: " + engEquip.RegNumber;
                                changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteEngEquip", logDescription, position.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, CurrentUser);

                                break;
                            case "MOB_LIFT_EQUIP":
                                MobileLiftingEquip mobileLiftingEquip = MobileLiftingEquipUtil.GetMobileLiftingEquipByTechnicsId(technicsID, CurrentUser);
                                logDescription += "; Рег. №: " + mobileLiftingEquip.RegNumber;
                                changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteMobLiftEquip", logDescription, position.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, CurrentUser);

                                break;
                            case "RAILWAY_EQUIP":
                                RailwayEquip railwayEquip = RailwayEquipUtil.GetRailwayEquipByTechnicsId(technicsID, CurrentUser);
                                logDescription += "; Инв. №: " + railwayEquip.InventoryNumber;
                                changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteRailwayEquip", logDescription, position.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, CurrentUser);

                                break;
                            case "AVIATION_EQUIP":
                                AviationEquip aviationEquip = AviationEquipUtil.GetAviationEquipByTechnicsId(technicsID, CurrentUser);
                                logDescription += "; Инв. №: " + aviationEquip.AirInvNumber;
                                changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteAviationEquip", logDescription, position.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, CurrentUser);

                                break;
                            case "VESSELS":
                                Vessel vessel = VesselUtil.GetVesselByTechnicsId(technicsID, CurrentUser);
                                logDescription += "; Име: " + vessel.VesselName +
                                                  "; Инв. №: " + vessel.InventoryNumber;
                                changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteVessel", logDescription, position.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, CurrentUser);

                                break;
                            case "FUEL_CONTAINERS":
                                FuelContainer fuelContainer = FuelContainerUtil.GetFuelContainerByTechnicsId(technicsID, CurrentUser);
                                logDescription += "; Инв. №: " + fuelContainer.InventoryNumber;
                                changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteFuelContainer", logDescription, position.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, CurrentUser);

                                break;
                        }

                        FillTechnicsRequestUtil.DeleteRequestCommandTechnic(fillTechnicsRequest.FillTechnicsRequestID, CurrentUser, changeEvent);

                        change.AddEvent(changeEvent);
                    }

                    //Clear the Mobilization Appointment
                    TechnicsAppointmentUtil.ClearTheCurrentTechnicsAppointmentByTechnics(technicsID, CurrentUser, change);
                }

                //Save the TechnicsMilRepStatus
                TechnicsMilRepStatusUtil.SaveTechnicsMilRepStatus(technicsMilRepStatus, CurrentUser, change);

                //Write into the Audit Trail
                change.WriteLog();

                stat = AJAXTools.OK;
                response = @"<result><response>OK</response>
                            <TechnicsMilRepStatusId>" + technicsMilRepStatus.TechnicsMilRepStatusId + @"</TechnicsMilRepStatusId>
                            <GroupManagementSection>" + AJAXTools.EncodeForXML(AddEditTechnics_MilitaryReport_PageUtil.GetGroupManagementSection(technicsID, CurrentUser)) + "</GroupManagementSection>" + 
                           "<TechnicsAppointmentSection>" + AJAXTools.EncodeForXML(AddEditTechnics_MilitaryReport_PageUtil.GetTechnicsAppointmentSection(technicsID, CurrentUser, this)) + "</TechnicsAppointmentSection>" + 
                          @"</result>" ;
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

        private void JSLoadMilRepStatusHistory()
        {
            int technicsId = 0;
            int.TryParse((Request.Params["TechnicsId"]).ToString(), out technicsId);

            string technicsTypeKey = TechnicsUtil.GetTechnics(technicsId, CurrentUser).TechnicsType.TypeKey;

            if (GetUIItemAccessLevel("RES_TECHNICS_" + technicsTypeKey + "_EDIT_MILREP_MILREPSTATUS_HISTORY") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                response = "<response>" + AJAXTools.EncodeForXML(AddEditTechnics_MilitaryReport_PageUtil.GetMilRepStatusHistoryLightBox(CurrentUser, Request)) + "</response>";

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

        private void JSLoadTechnicsAppointmentHistory()
        {
            int technicsId = 0;
            int.TryParse((Request.Params["TechnicsId"]).ToString(), out technicsId);

            if (GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsUtil.GetTechnics(technicsId, CurrentUser).TechnicsType.TypeKey + "_EDIT_MILREP_APPOINTMENT_HISTORY") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {

                response = "<response>" + AJAXTools.EncodeForXML(AddEditTechnics_MilitaryReport_PageUtil.GetTechnicsAppointmentHistoryLightBox(CurrentUser, Request, this)) + "</response>";

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

        //Save Military Report Tab (ajax call)
        private void JSSaveMilitaryReportTab()
        {
            string stat = "";
            string response = "";

            try
            {
                int technicsId = int.Parse(Request.Form["TechnicsId"]);
                string groupManagementSection = Request.Form["GroupManagementSection"];
                string section = Request.Form["Section"];
                string deliverer = Request.Form["Deliverer"];
                int? punktId = null;
                if (!String.IsNullOrEmpty(Request.Form["PunktId"]) && Request.Form["PunktId"] != ListItems.GetOptionChooseOne().Value)
                    punktId = int.Parse(Request.Form["PunktId"]);
                int? appointmentIsDelivered = !string.IsNullOrEmpty(Request.Form["AppointmentIsDelivered"]) ? (int?)int.Parse(Request.Form["AppointmentIsDelivered"]) : null;
                
                Technics technics = TechnicsUtil.GetTechnics(technicsId, CurrentUser);

                Change change = new Change(CurrentUser, "RES_Technics_" + technics.TechnicsType.TypeKey);

                string logDescription = TechnicsUtil.GetTechnicsLogDescription(technicsId, CurrentUser);

                ChangeEvent changeEvent = new ChangeEvent("RES_Technics_EditMilRep", logDescription, null, null, CurrentUser);

                technics.GroupManagementSection = groupManagementSection;
                technics.Section = section;
                technics.Deliverer = deliverer;
                technics.PunktID = punktId;

                TechnicsUtil.SaveTechnics_WhenEditingMilitaryReportTab(technics, CurrentUser, changeEvent);

                if (appointmentIsDelivered.HasValue)
                    TechnicsUtil.SaveTechnics_SetAppointmentIsDelivered(technics.TechnicsId, appointmentIsDelivered.Value, CurrentUser, changeEvent);

                if (changeEvent.ChangeEventDetails.Count > 0)
                    change.AddEvent(changeEvent);

                change.WriteLog();

                stat = AJAXTools.OK;
                response = "<status>OK</status>";
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

        private void JSPrintMK()
        {
            int technicsId = int.Parse(Request.Params["TechnicsId"]);
            string result = AddEditTechnics_MilitaryReport_PageUtil.PrintMK(technicsId, CurrentUser);

            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment;filename=MK.doc");
            Response.ContentType = "application/vnd.ms-word";

            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            Response.Write(result);
            Response.End();
        }

        private void JSPrintPZ()
        {
            int technicsId = int.Parse(Request.Params["TechnicsId"]);
            string result = AddEditTechnics_MilitaryReport_PageUtil.PrintPZ(technicsId, CurrentUser);

            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment;filename=PZ.doc");
            Response.ContentType = "application/vnd.ms-word";

            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            Response.Write(result);
            Response.End();
        }

        private void JSPrintOK()
        {
            int technicsId = int.Parse(Request.Params["TechnicsId"]);
            string result = AddEditTechnics_MilitaryReport_PageUtil.PrintOK(technicsId, CurrentUser);

            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment;filename=OK.doc");
            Response.ContentType = "application/vnd.ms-word";

            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            Response.Write(result);
            Response.End();
        }

        private void JSPrintTO()
        {
            int technicsId = int.Parse(Request.Params["TechnicsId"]);
            string result = AddEditTechnics_MilitaryReport_PageUtil.PrintTO(technicsId, CurrentUser);

            Response.Clear();
            Response.AppendHeader("content-disposition", "attachment;filename=TO.doc");
            Response.ContentType = "application/vnd.ms-word";

            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());

            Response.Write(result);
            Response.End();
        }

    }

    public static class AddEditTechnics_MilitaryReport_PageUtil
    {
        public static string GetTabContent(int technicsId, string moduleKey, User currentUser, AddEditTechnics page)
        {
            string technicsMilitaryReportStatusSection = GetTechnicsMilitaryReportStatusSection(technicsId, currentUser, page);
            string addEditMilRepStatusLightBox = GetAddEditMilRepStatusLightBox(technicsId, currentUser, moduleKey, page);
            string milRepStatusHistoryLightBox = @"<div id=""divMilRepStatusHistoryLightBox"" style=""display: none;"" class=""lboxMilRepStatusHistory""></div>";
            string technicsAppointmentSection = GetTechnicsAppointmentSection(technicsId, currentUser, page);
            string technicsAppointmentHistoryLightBox = @"<div id=""divTechnicsAppointmentHistoryLightBox"" style=""display: none;"" class=""lboxTechnicsAppointmentHistory""></div>";
            string groupManagementSection = @"<div id=""divGroupManagementSection"" >" + GetGroupManagementSection(technicsId, currentUser) + "</div>";

            string html = @"
<div style=""height: 10px;""></div>
" + technicsMilitaryReportStatusSection + @"
" + addEditMilRepStatusLightBox + @"
" + milRepStatusHistoryLightBox + @"
<div style=""height: 10px;""></div>
<div id=""divTechnicsAppointmentSection"">" + technicsAppointmentSection + @"</div>
" + technicsAppointmentHistoryLightBox + @"
<div style=""height: 10px;""></div>
" + groupManagementSection + @"

<div style=""height: 10px;""></div>
";

            return html;
        }

        public static string GetTechnicsMilitaryReportStatusSection(int technicsId, User currentUser, AddEditTechnics page)
        {
            Technics technics = TechnicsUtil.GetTechnics(technicsId, currentUser);
            TechnicsMilRepStatus currentMilRepStatus = TechnicsMilRepStatusUtil.GetTechnicsMilRepCurrentStatusByTechnicsId(technicsId, currentUser);
            TechnicsMilRepStatus firstMilRepStatus = TechnicsMilRepStatusUtil.GetTechnicsMilRepFirstStatusByTechnicsId(technicsId, currentUser);

            string contractDisplay = "none";
            string voluntaryDisplay = "none";
            string removedDisplay = "none";
            string temporaryRemovedDisplay = "none";
            string postponedDisplay = "none";

            string militaryReportStatusKey = currentMilRepStatus != null ? currentMilRepStatus.TechMilitaryReportStatus.TechMilitaryReportStatusKey : "";
            switch (militaryReportStatusKey)
            {
                case "CONTRACT":
                    contractDisplay = "";
                    break;
                case "VOLUNTARY_RESERVE":
                    voluntaryDisplay = "";
                    break;
                case "REMOVED":
                    removedDisplay = "";
                    break;              
                case "TEMPORARY_REMOVED":
                    temporaryRemovedDisplay = "";
                    break;
                case "POSTPONED":
                    postponedDisplay = "";
                    break;
            }

            string firstEnrolDate = (firstMilRepStatus != null ? CommonFunctions.FormatDate(firstMilRepStatus.EnrolDate) : "");
            string currEnrolDate = (currentMilRepStatus != null ? CommonFunctions.FormatDate(currentMilRepStatus.EnrolDate) : "");
            string dischargeDate = currentMilRepStatus != null ? CommonFunctions.FormatDate(currentMilRepStatus.DischargeDate) : "";
            string currMilRepStatusName = (currentMilRepStatus != null ? currentMilRepStatus.TechMilitaryReportStatus.TechMilitaryReportStatusName : TechMilitaryReportStatusUtil.GetLabelWhenLackOfStatus());
            string currSourceMilDepartmentName = (currentMilRepStatus != null && currentMilRepStatus.SourceMilDepartment != null ? currentMilRepStatus.SourceMilDepartment.MilitaryDepartmentName : "");
            string contractContractNumber = currentMilRepStatus != null ? currentMilRepStatus.Contract_ContractNumber : "";
            string contractContractFromDate = currentMilRepStatus != null ? CommonFunctions.FormatDate(currentMilRepStatus.Contract_ContractFromDate) : "";
            string contractMilitaryUnit = currentMilRepStatus != null && currentMilRepStatus.Contract_MilitaryUnit != null ? currentMilRepStatus.Contract_MilitaryUnit.DisplayTextForSelection : "";
            string contractContractToDate = currentMilRepStatus != null ? CommonFunctions.FormatDate(currentMilRepStatus.Contract_ContractToDate) : "";            
            string voluntaryContractNumber = currentMilRepStatus != null ? currentMilRepStatus.Voluntary_ContractNumber : "";
            string voluntaryContractDate = currentMilRepStatus != null ? CommonFunctions.FormatDate(currentMilRepStatus.Voluntary_ContractDate) : "";
            string voluntaryDurationMonths = currentMilRepStatus != null ? currentMilRepStatus.Voluntary_DurationMonths.ToString() : "";
            string voluntaryContractToDate = currentMilRepStatus != null ? CommonFunctions.FormatDate(currentMilRepStatus.Voluntary_ContractToDate) : "";
            string voluntaryFulfilPlace = (currentMilRepStatus != null && currentMilRepStatus.Voluntary_FulfilPlace != null ? currentMilRepStatus.Voluntary_FulfilPlace.DisplayTextForSelection : "");
            
            string removedDate = currentMilRepStatus != null ? CommonFunctions.FormatDate(currentMilRepStatus.Removed_Date) : "";
            string removedReason = currentMilRepStatus != null && currentMilRepStatus.Removed_Reason != null ? currentMilRepStatus.Removed_Reason.Text() : "";
            string temporaryRemovedReason = currentMilRepStatus != null && currentMilRepStatus.TemporaryRemoved_Reason != null ? currentMilRepStatus.TemporaryRemoved_Reason.Text() : "";
            string temporaryRemovedDate = currentMilRepStatus != null ? CommonFunctions.FormatDate(currentMilRepStatus.TemporaryRemoved_Date) : "";
            string temporaryRemovedDuration = currentMilRepStatus != null ? currentMilRepStatus.TemporaryRemoved_Duration.ToString() : "";
            string technicsPostponeType = currentMilRepStatus != null && currentMilRepStatus.TechnicsPostpone_Type != null ? currentMilRepStatus.TechnicsPostpone_Type.Text() : "";
            string technicsPostponeYear = currentMilRepStatus != null ? currentMilRepStatus.TechnicsPostpone_Year.ToString() : "";

            string missingValue = "<липсва>";
            string technicsPostponeOwner = missingValue;
            if (technics.OwnershipCompany != null)
            {
                technicsPostponeOwner = (technics.OwnershipCompany != null ? technics.OwnershipCompany.UnifiedIdentityCode + " " + technics.OwnershipCompany.CompanyName : "");
            }
            
            string technicsPostponeTechnicsSubTypeName = (technics.NormativeTechnics != null && technics.NormativeTechnics.TechnicsSubType != null ? technics.NormativeTechnics.TechnicsSubType.TechnicsSubTypeName : missingValue);

            string btnAddNewStatusHTML = "";
            string btnEditCurrStatusHTML = "";
            string btnHistoryStatusesHTML = "";

            string btnPrintTOHTML = "";

            btnAddNewStatusHTML = @"<img id=""btnAddNewResMilRepStatus"" src='../Images/index_new.png' alt='Смяна на състоянието по отчета' title='Смяна на състоянието по отчета' class='GridActionIcon' style='width: 22px; height: 22px;' onclick='btnAddNewTechMilRepStatus_Click();'  />";

            btnEditCurrStatusHTML = @"<img id=""btnEditCurrResMilRepStatus"" src='../Images/index_preferences.png' alt='Редактиране детайлите на текущото състояние по отчета' title='Редактиране детайлите на текущото състояние по отчета' class='GridActionIcon' style='width: 22px; height: 22px;display: " + (currentMilRepStatus != null ? "" : "none") + ";' onclick='btnEditCurrStatusHTML_Click();'  />";

            btnHistoryStatusesHTML = @"<img id=""btnHistoryStatuses"" src='../Images/index_view.png' alt='История' title='История' class='GridActionIcon' style='width: 22px; height: 22px;' onclick='btnHistoryStatuses_Click();'  />";

            if (currentMilRepStatus != null &&
                page.GetUIItemAccessLevel("RES_PRINT") != UIAccessLevel.Hidden &&
                page.GetUIItemAccessLevel("RES_PRINT_POSTPONE_TECHNICS") != UIAccessLevel.Hidden)
            {
                btnPrintTOHTML = @"<tr>
                                    <td colspan=""5"" style=""text-align: center; position: relative; "">
                                        <div style=""position: relative; left: 591px; top: 0px; width: 250px; padding-top: 7px;"">                                  
                                            <div id=""btnPrintTO"" style=""display: inline;margin-top:10px;align=center"" onclick=""PrintTO();"" class=""Button"" olddisabled="""">
                                                <i></i><div id=""btnPrintTOText""  style=""width: 195px; height: 24px;"">Печат на талон за отсрочване</div><b></b>
                                            </div>
                                        </div>                                    
                                    </td>
                                </tr> ";
            }

            string html = @"
<fieldset style=""width: 830px; padding: 0px;"">
   <input type=""hidden"" id=""technicsMilRepStatusId"" value=""" + (currentMilRepStatus != null ? currentMilRepStatus.TechnicsMilRepStatusId.ToString() : "0") + @""" />
   <table class=""InputRegion"" style=""width: 830px; padding: 10px; padding-left: 0px; padding-top: 0px; margin-top: 0px;"">
      <tr style=""height: 3px;"">
      </tr>
      <tr>
         <td style=""text-align: left;"">
            <table>
                <tr>
                    <td style=""text-align: right; width: 200px;"">
                        <span id=""lblFirstEnrolDate"" class=""InputLabel"">Първоначално зачисляване:</span>
                    </td>
                    <td style=""text-align: left; width: 120px;"">
                        <span id=""lblFirstEnrolDateValue"" class=""ReadOnlyValue"">" + firstEnrolDate + @"</span>
                    </td>
                    <td style=""text-align: right; width: 160px;"">
                        &nbsp;
                    </td>
                    <td style=""text-align: left; width: 200px;"">
                        &nbsp;
                    </td>
                    <td style=""width: 150px;"">
                       <div style=""text-align: right; position: relative; top: -5px; left: 5px;"">
                          " + btnAddNewStatusHTML + btnEditCurrStatusHTML + btnHistoryStatusesHTML + @"
                       </div>
                    </td>
                </tr>
                <tr>
                    <td style=""text-align: right;"">
                        <span id=""lblCurrEnrolDate"" class=""InputLabel"">Дата на промяна:</span>
                    </td>
                    <td style=""text-align: left;"">
                        <span id=""lblCurrEnrolDateValue"" class=""ReadOnlyValue"">" + currEnrolDate + @"</span>
                    </td>                    
                    <td style=""text-align: right;"">
                        <span id=""lblDischargeDate"" class=""InputLabel"">Дата на отчисляване:</span>
                    </td>
                    <td style=""text-align: left;"">
                        <span id=""txtDischargeDate"" class=""ReadOnlyValue"" style=""width: 80px;"">" + dischargeDate + @"</span>
                    </td>
                </tr>
                <tr>
                    <td style=""text-align: right; vertical-align: top;"">
                        <span id=""lblCurrMilitaryReportStatus"" class=""InputLabel"">Състояние по отчета:</span>
                    </td>
                    <td style=""text-align: left; vertical-align: top;"">
                        <span id=""lblCurrMilitaryReportStatusValue"" class=""ReadOnlyValue"">" + currMilRepStatusName + @"</span>
                    </td>
                    <td style=""text-align: right; vertical-align: top;"">
                        <span id=""lblCurrSourceMilDepartmentName"" class=""InputLabel"">Военно окръжие:</span>
                    </td>
                    <td style=""text-align: left; vertical-align: top;"">
                        <span id=""lblCurrSourceMilDepartmentNameValue"" class=""ReadOnlyValue"">" + currSourceMilDepartmentName + @"</span>
                    </td>
                </tr>
                <tr>
                    <td colspan=""4"" style=""text-align: center;"">
                        <div id=""divSectionContract"" style=""display: " + contractDisplay + @"; width: 100%;"">
                            <table style=""text-align: center; width: 100%;"">
                                <tr style=""min-height: 17px"">
                                    <td style=""text-align: right;"">
                                        <span id=""lblContractContractNumber"" class=""InputLabel"">Договор №:</span>
                                    </td>
                                    <td style=""text-align: left;"">
                                        <span id=""txtContractContractNumber"" class=""ReadOnlyValue"" style=""width: 120px;"">" + contractContractNumber + @"</span>
                                    </td>
                                    <td style=""text-align: right;"">
                                        <span id=""lblContractMilitaryUnit"" class=""InputLabel"">" + CommonFunctions.GetLabelText("MilitaryUnit") + @":</span>
                                    </td>
                                    <td style=""text-align: left;"">
                                        <span id=""txtContractMilitaryUnit"" class=""ReadOnlyValue"">" + contractMilitaryUnit + @"</span>                               
                                    </td>                             
                                </tr>
                                <tr style=""min-height: 17px"">
                                    <td style=""text-align: right;"">
                                        <span id=""lblContractContractFromDate"" class=""InputLabel"">Дата:</span>
                                    </td>
                                    <td style=""text-align: left;"">
                                        <span id=""txtContractContractFromDate"" class=""ReadOnlyValue"" style=""width: 80px;"">" + contractContractFromDate + @"</span>
                                    </td>
                                    <td style=""text-align: right;"">
                                        <span id=""lblContractContractToDate"" class=""InputLabel"">До дата:</span>
                                    </td>
                                    <td style=""text-align: left;"">
                                        <span id=""txtContractContractToDate"" class=""ReadOnlyValue"" style=""width: 80px;"">" + contractContractToDate + @"</span>
                                    </td>                                   
                                </tr>                                
                            </table>
                        </div>
                        <div id=""divSectionVoluntary"" style=""display: " + voluntaryDisplay + @"; width: 100%;"">
                            <table style=""text-align: center; width: 100%;"">
                                <tr style=""min-height: 17px"">
                                    <td style=""text-align: right;"">
                                        <span id=""lblVoluntaryContractNumber"" class=""InputLabel"">Договор №:</span>
                                    </td>
                                    <td style=""text-align: left;"">
                                        <span id=""txtVoluntaryContractNumber"" class=""ReadOnlyValue"" style=""width: 120px;"">" + voluntaryContractNumber + @"</span>
                                    </td>
                                    <td style=""text-align: right;"">
                                        <span id=""lblVoluntaryContractDate"" class=""InputLabel"">от дата:</span>
                                    </td>
                                    <td style=""text-align: left;"">
                                        <span id=""txtVoluntaryContractDate"" class=""ReadOnlyValue"" style=""width: 80px;"">" + voluntaryContractDate + @"</span>
                                    </td>
                                    <td style=""text-align: right;"">
                                        <span id=""lblVoluntaryDurationMonths"" class=""InputLabel"">Срок:</span>
                                    </td>
                                    <td style=""text-align: left;"">
                                        <span id=""txtVoluntaryDurationMonths"" class=""ReadOnlyValue"">" + voluntaryDurationMonths + @"</span>
                                    </td>
                                </tr>
                                <tr style=""min-height: 17px"">
                                    <td style=""text-align: right;"">
                                        <span id=""lblVoluntaryFulfilPlace"" class=""InputLabel"">Място на изпълнение:</span>
                                    </td>
                                    <td style=""text-align: left;"">
                                        <span id=""txtVoluntaryFulfilPlace"" class=""ReadOnlyValue"" style=""width: 120px;"">" + voluntaryFulfilPlace + @"</span>
                                    </td>
                                    <td style=""text-align: right;"">
                                        <span id=""lblVoluntaryContractToDate"" class=""InputLabel"">до дата:</span>
                                    </td>
                                    <td style=""text-align: left;"">
                                        <span id=""txtVoluntaryContractToDate"" class=""ReadOnlyValue"" style=""width: 80px;"">" + voluntaryContractToDate + @"</span>
                                    </td>                                   
                                </tr>                               
                            </table>
                        </div>
                        <div id=""divSectionRemoved"" style=""display: " + removedDisplay + @"; width: 100%;"">
                            <table style=""text-align: center; width: 100%;"">
                                <tr style=""min-height: 17px"">
                                    <td style=""text-align: right;"">
                                        <span id=""lblRemovedDate"" class=""InputLabel"">Изключен от отчет:</span>
                                    </td>
                                    <td style=""text-align: left;"">
                                        <span id=""txtRemovedDate"" class=""ReadOnlyValue"" style=""width: 80px;"">" + removedDate + @"</span>
                                    </td>
                                    <td style=""text-align: right;"">
                                        <span id=""lblRemovedReason"" class=""InputLabel"">Причина за изключване:</span>
                                    </td>
                                    <td style=""text-align: left;""> 
                                        <span id=""txtRemovedReason"" class=""ReadOnlyValue"">" + removedReason + @"</span>
                                    </td>
                                </tr>
                            </table>
                        </div>                        
                        <div id=""divSectionTemporaryRemoved"" style=""display: " + temporaryRemovedDisplay + @"; width: 100%;"">
                            <table style=""text-align: center; width: 100%;"">
                                <tr style=""min-height: 17px"">
                                    <td style=""text-align: right;"">
                                        <span id=""lblTemporaryRemovedReason"" class=""InputLabel"">Причина за временно снемане:</span>
                                    </td>
                                    <td style=""text-align: left;""> 
                                        <span id=""txtTemporaryRemovedReason"" class=""ReadOnlyValue"">" + temporaryRemovedReason + @"</span>
                                    </td>
                                    <td style=""text-align: right;"">
                                        <span id=""lblTemporaryRemovedDate"" class=""InputLabel"">Начална дата:</span>
                                    </td>
                                    <td style=""text-align: left;"">
                                        <span id=""txtTemporaryRemovedDate"" class=""ReadOnlyValue"" style=""width: 80px;"">" + temporaryRemovedDate + @"</span>
                                    </td>
                                    <td style=""text-align: right;"">
                                        <span id=""lblTemporaryRemovedDuration"" class=""InputLabel"">Продължителност:</span>
                                    </td>
                                    <td style=""text-align: left;"">
                                        <span id=""txtTemporaryRemovedDuration"" class=""ReadOnlyValue"" style=""width: 40px"">" + temporaryRemovedDuration + @"</span>    
                                        <span class=""InputLabel"">месеца</span>                           
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div id=""divSectionPostpone"" style=""display: " + postponedDisplay + @"; width: 100%; padding-top: 15px;"">
                            <table style=""text-align: center; width: 100%;"">
                                <tr style=""min-height: 17px"">
                                    <td style=""text-align: right; vertical-align: top;"">
                                        <span id=""lblPostponeType"" class=""InputLabel"">Вид отсрочване:</span>
                                    </td>
                                    <td style=""text-align: left; vertical-align: top;""> 
                                        <span id=""txtPostponeType"" class=""ReadOnlyValue"">" + technicsPostponeType + @"</span>
                                    </td>
                                    <td style=""text-align: right; vertical-align: top;"">
                                        <span id=""lblPostponeOwner"" class=""InputLabel"">Собственик:</span>
                                    </td>
                                    <td style=""text-align: left; vertical-align: top;"">
                                        <span id=""lblPostponeOwnerValue"" class=""ReadOnlyValue"">" + technicsPostponeOwner + @"</span>
                                    </td>                                    
                                </tr>
                                <tr style=""min-height: 17px"">
                                    <td style=""text-align: right; vertical-align: top;"">
                                        <span id=""lblPostponeYear"" class=""InputLabel"">За коя година:</span>
                                    </td>
                                    <td style=""text-align: left; vertical-align: top;"">
                                        <span id=""txtPostponeYear"" class=""ReadOnlyValue"" style=""width: 40px"">" + technicsPostponeYear + @"</span>
                                    </td> 
                                    <td style=""text-align: right; vertical-align: top;"">
                                        <span id=""lblPostponeTechnicsSubTypeName"" class=""InputLabel"">Тип на техниката:</span>
                                    </td>
                                    <td style=""text-align: left; vertical-align: top;"">
                                        <span id=""lblPostponeTechnicsSubTypeNameValue"" class=""ReadOnlyValue"">" + technicsPostponeTechnicsSubTypeName + @"</span>
                                    </td>                                     
                                </tr>
                                " + btnPrintTOHTML + @"
                                
                            </table>
                        </div>                        
                    </td>
                </tr>
                " + (page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_PRINT_OK") != UIAccessLevel.Hidden ?
               @"
                <tr>
                    <td colspan=""5"" style=""text-align: center; position: relative; "">
                        <div style=""position: absolute; top: 0px; right: -36px; width: 250px;"">                                  
                            <div id=""btnPrintOK"" style=""display: inline;margin-top:10px;align=center"" onclick=""PrintOK();"" class=""Button"" olddisabled="""">
                                <i></i><div id=""btnPrintOKText""  style=""width: 195px;"">Печат на отчетен картон</div><b></b>
                            </div>
                        </div>                                    
                    </td>
                </tr> 
                " : "") + @"  
                <tr>        
                    <td colspan=""4"" style=""text-align: center;"">
                        <span id=""spanMilRepStatusSectionMsg"" class=""ErrorText"" style=""display: none;""></span>&nbsp;
                    </td>        
                </tr>
            </table>
         </td>
      </tr>
   </table>
</fieldset>
";

            return html;
        }

        //Render the AddEditMilRepStatus light-box
        public static string GetAddEditMilRepStatusLightBox(int technicsId, User currentUser, string moduleKey, AddEditTechnics page)
        {
            Technics technics = TechnicsUtil.GetTechnics(technicsId, currentUser);

            string ddCurrSourceMilitaryDepartmentHtml = "";           

            List<MilitaryDepartment> milDepts = MilitaryDepartmentUtil.GetAllMilitaryDepartments(currentUser);

            List<IDropDownItem> milDeptsDropDownItems = new List<IDropDownItem>();
            foreach (MilitaryDepartment milDept in milDepts)
                milDeptsDropDownItems.Add(milDept as IDropDownItem);

            ddCurrSourceMilitaryDepartmentHtml = ListItems.GetDropDownHtml(milDeptsDropDownItems, null, "ddCurrSourceMilDepartmentNameLightBox", true, null, null, @"UnsavedCheckSkipMe=""true"" class=""RequiredInputField"" style=""width: auto;""");

            string ddRemovedReasonsHtml = "";

            List<GTableItem> removedReasons = GTableItemUtil.GetAllGTableItemsByTableName("TechMilRepStat_RemovedReasons", moduleKey, 1, 0, 0, currentUser);

            List<IDropDownItem> removedReasonsDropDownItems = new List<IDropDownItem>();
            foreach (GTableItem removedReason in removedReasons)
                removedReasonsDropDownItems.Add(removedReason as IDropDownItem);

            ddRemovedReasonsHtml = ListItems.GetDropDownHtml(removedReasonsDropDownItems, null, "ddRemovedReasonLightBox", true, null, null, @"UnsavedCheckSkipMe=""true""");

            string ddTemporaryRemovedReasonsHtml = "";

            List<GTableItem> temporaryRemovedReasons = GTableItemUtil.GetAllGTableItemsByTableName("TechMilRepStat_ТemporaryRemovedReasons", moduleKey, 1, 0, 0, currentUser);

            List<IDropDownItem> temporaryRemovedReasonsDropDownItems = new List<IDropDownItem>();
            foreach (GTableItem temporaryRemovedReason in temporaryRemovedReasons)
                temporaryRemovedReasonsDropDownItems.Add(temporaryRemovedReason as IDropDownItem);

            ddTemporaryRemovedReasonsHtml = ListItems.GetDropDownHtml(temporaryRemovedReasonsDropDownItems, null, "ddTemporaryRemovedReasonsLightBox", true, null, null, @"UnsavedCheckSkipMe=""true""");
            
            string ddTechnicsPostponeTypeHtml = "";

            List<TechnicsPostponeType> technicsPostponeTypes = TechnicsPostponeTypeUtil.GetAllTechnicsPostponeTypes(currentUser);

            List<IDropDownItem> technicsPostponeTypesDropDownItems = new List<IDropDownItem>();
            foreach (TechnicsPostponeType technicsPostponeType in technicsPostponeTypes)
                technicsPostponeTypesDropDownItems.Add(technicsPostponeType as IDropDownItem);

            ddTechnicsPostponeTypeHtml = ListItems.GetDropDownHtml(technicsPostponeTypesDropDownItems, null, "ddPostponeTypeLightBox", true, null, null, @"UnsavedCheckSkipMe=""true"" class=""RequiredInputField"" style=""width: auto;""");

            string muContractMilitaryUnitHtml = "";

            MilitaryUnitSelector.MilitaryUnitSelector milUnitSelector = new MilitaryUnitSelector.MilitaryUnitSelector();
            milUnitSelector.Page = page;
            milUnitSelector.DataSourceWebPage = "DataSource.aspx";
            milUnitSelector.DataSourceKey = "MilitaryUnit";
            milUnitSelector.ResultMaxCount = 1000;
            milUnitSelector.Style.Add("width", "180px");
            milUnitSelector.DivMainCss = "isDivMainClass";
            milUnitSelector.DivListCss = "isDivListClass";
            milUnitSelector.DivFullListCss = "isDivFullListClass";
            milUnitSelector.DivFullListTitle = CommonFunctions.GetLabelText("MilitaryUnit");
            milUnitSelector.ID = "muContractMilitaryUnitLightBox";

            StringWriter sw = new StringWriter();
            HtmlTextWriter tw = new HtmlTextWriter(sw);

            milUnitSelector.RenderControl(tw);

            muContractMilitaryUnitHtml = sw.ToString();

            string missingValue = "<липсва>";
            string technicsPostponeOwner = missingValue;
            if (technics.OwnershipCompany != null)
            {
                technicsPostponeOwner = (technics.OwnershipCompany != null ? technics.OwnershipCompany.UnifiedIdentityCode + " " + technics.OwnershipCompany.CompanyName : "");
            }
            
            string technicsPostponeTechnicsSubTypeName = (technics.NormativeTechnics != null && technics.NormativeTechnics.TechnicsSubType != null ? technics.NormativeTechnics.TechnicsSubType.TechnicsSubTypeName : missingValue);


            //Generate MilitaryUnitSelector
            MilitaryUnitSelector.MilitaryUnitSelector voluntaryFulfilPlace = new MilitaryUnitSelector.MilitaryUnitSelector();
            voluntaryFulfilPlace.Page = page;
            voluntaryFulfilPlace.DataSourceWebPage = "DataSource.aspx";
            voluntaryFulfilPlace.DataSourceKey = "MilitaryUnit";
            voluntaryFulfilPlace.ResultMaxCount = 1000;
            voluntaryFulfilPlace.Style.Add("width", "100px");
            voluntaryFulfilPlace.DivMainCss = "isDivMainClass";
            voluntaryFulfilPlace.DivListCss = "isDivListClass";
            voluntaryFulfilPlace.DivFullListCss = "isDivFullListClass";
            voluntaryFulfilPlace.DivFullListTitle = CommonFunctions.GetLabelText("MilitaryUnit");
            voluntaryFulfilPlace.IncludeOnlyActual = false;
            voluntaryFulfilPlace.ID = "itmsVoluntaryFulfilPlace";

            if (page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_MILREPSTATUS_EDITMILREPSTATUS_VOLUNTARYFULFILPLACE") == UIAccessLevel.Disabled)
                voluntaryFulfilPlace.Enabled = false;
           
            StringWriter sw2 = new StringWriter();
            HtmlTextWriter tw2 = new HtmlTextWriter(sw2);
            voluntaryFulfilPlace.RenderControl(tw2);


            string html = @"
<div id=""lboxAddEditMilRepStatus"" style=""display: none;"" class=""lboxAddEditMilRepStatus"">
<center>
    <input type=""hidden"" id=""hdnTechnicsMilRepStatusID"" />
    <table width=""100%"" style=""text-align: center;"">
        <tr style=""height: 15px"">
        </tr>
        <tr>
            <td colspan=""4"" align=""center"">
                <span class=""HeaderText"" style=""text-align: center;"" id=""lblAddEditMilRepStatusTitle""></span>
            </td>
        </tr>
        <tr style=""height: 15px"">
        </tr>  
        <tr style=""min-height: 17px"">
            <td style=""text-align: right; width: 200px;"">
                <span id=""lblCurrentMilRepStatusLightBox"" class=""InputLabel"">Текущо състояние:</span>
            </td>
            <td style=""text-align: left; width: 230px;"">
                <span id=""lblCurrentMilRepStatusValueLightBox"" class=""ReadOnlyValue""></span>
            </td>
            <td style=""text-align: right; width: 130px;"">
                <span id=""lblNewMilRepStatus"" class=""InputLabel"">Ново състояние:</span>
            </td>
            <td style=""text-align: left; width: 230px;"">
                <select id=""ddNewMilRepStatus"" onchange=""NewMilRepStatusChanged();"" UnsavedCheckSkipMe=""true"" class=""RequiredInputField"" ></select>
            </td>
        </tr>
        <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblCurrEnrolDateLightBox"" class=""InputLabel"">Дата на промяна:</span>
            </td>
            <td style=""text-align: left;"">
                <span id=""spanCurrEnrolDateLightBox"">
                    <input id=""txtCurrEnrolDateLightBox"" class=""RequiredInputField " + CommonFunctions.DatePickerCSS() + @""" UnsavedCheckSkipMe=""true""></input>
                </span>
            </td>
            <td style=""text-align: right;"">
                <span id=""lblDischargeDateLightBox"" class=""InputLabel"">Снет от отчет:</span>
            </td>
            <td style=""text-align: left;"">
                <span id=""spanDischargeDateLightBox"">
                    <input id=""txtDischargeDateLightBox"" class=""" + CommonFunctions.DatePickerCSS() + @""" UnsavedCheckSkipMe=""true""></input>
                </span>
            </td>
        </tr>
        <tr style=""min-height: 17px"">            
            <td style=""text-align: right;"">
                <span id=""lblCurrSourceMilDepartmentNameLightBox"" class=""InputLabel"">Военно окръжие:</span>
            </td>
            <td style=""text-align: left;""> "
                + ddCurrSourceMilitaryDepartmentHtml + @"
            </td>
        </tr>
        <tr>
            <td colspan=""4"" style=""text-align: center;"">
                <div id=""divContract"" style=""display: none; width: 100%;"">
                    <table style=""text-align: center; width: 100%;"">
                        <tr style=""min-height: 17px"">
                            <td style=""text-align: right; width: 200px;"">
                                <span id=""lblContractContractNumberLightBox"" class=""InputLabel"">Договор №:</span>
                            </td>
                            <td style=""text-align: left; width: 240px;"">
                                <input id=""txtContractContractNumberLightBox"" class=""InputField"" UnsavedCheckSkipMe=""true""></input>
                            </td>                           
                            <td style=""text-align: right; width: 130px;"">
                                <span id=""lblContractMilitaryUnitLightBox"" class=""InputLabel"">" + CommonFunctions.GetLabelText("MilitaryUnit") + @":</span>
                            </td>
                            <td style=""text-align: left; width: 230px;""> "
                                 + muContractMilitaryUnitHtml + @"
                            </td>
                        </tr>
                        <tr style=""min-height: 17px"">
                            <td style=""text-align: right;"">
                                <span id=""lblContractContractFromDateLightBox"" class=""InputLabel"">Дата:</span>
                            </td>
                            <td style=""text-align: left;"">
                                <span id=""spanContractContractFromDateLightBox"">
                                    <input id=""txtContractContractFromDateLightBox"" class=""" + CommonFunctions.DatePickerCSS() + @""" style=""width: 80px;"" UnsavedCheckSkipMe=""true""></input>
                                </span>
                            </td>
                            <td style=""text-align: right;"">
                                <span id=""lblContractContractToDateLightBox"" class=""InputLabel"">До дата:</span>
                            </td>
                            <td style=""text-align: left;"">
                                <span id=""spanContractContractToDateLightBox"">
                                    <input id=""txtContractContractToDateLightBox"" class=""" + CommonFunctions.DatePickerCSS() + @""" style=""width: 80px;"" UnsavedCheckSkipMe=""true""></input>
                                </span>
                            </td>                         
                        </tr>                   
                    </table>
                </div>
                <div id=""divVoluntary"" style=""display: none; width: 100%;"">
                    <table style=""text-align: center; width: 100%;"">
                        <tr style=""min-height: 17px"">
                            <td style=""text-align: right;"">
                                <span id=""lblVoluntaryContractNumberLightBox"" class=""InputLabel"">Договор №:</span>
                            </td>
                            <td style=""text-align: left;"">
                                <input id=""txtVoluntaryContractNumberLightBox"" class=""InputField"" style=""width: 120px;"" UnsavedCheckSkipMe=""true""></input>
                            </td>
                            <td style=""text-align: right;"">
                                <span id=""lblVoluntaryContractDateLightBox"" class=""InputLabel"">от дата:</span>
                            </td>
                            <td style=""text-align: left;"">
                                <span id=""spanVoluntaryContractDateLightBox"">
                                    <input id=""txtVoluntaryContractDateLightBox"" class=""" + CommonFunctions.DatePickerCSS() + @""" style=""width: 80px;"" UnsavedCheckSkipMe=""true"" inLightBox=""true""></input>
                                </span>
                            </td>
                            <td style=""text-align: right;"">
                                <span id=""lblVoluntaryDurationMonthsLightBox"" class=""InputLabel"">Срок:</span>
                            </td>
                            <td style=""text-align: left;"">
                                <input id=""txtVoluntaryDurationMonthsLightBox"" class=""InputField"" UnsavedCheckSkipMe=""true""></input>
                            </td>                           
                        </tr>
                        <tr style=""min-height: 17px"">
                           
                            <td style=""text-align: right;"">
                                <span id=""lblVoluntaryFulfilPlaceLightBox"" class=""InputLabel"">Място на изпълнение:</span>
                            </td>
                            <td style=""text-align: left;"">
                                <span id=""spanVoluntaryFulfilPlaceLightBox"">" + sw2.ToString() + @"</span>                                
                            </td>
                            <td style=""text-align: right;"">
                                <span id=""lblVoluntaryContractToDateLightBox"" class=""InputLabel"">до дата:</span>
                            </td>
                            <td style=""text-align: left;"">
                                <span id=""spanVoluntaryContractToDateLightBox"">
                                    <input id=""txtVoluntaryContractToDateLightBox"" class=""" + CommonFunctions.DatePickerCSS() + @""" style=""width: 80px;"" UnsavedCheckSkipMe=""true"" inLightBox=""true""></input>
                                </span>
                            </td>                            
                        </tr>                       
                    </table>
                </div>
                <div id=""divRemoved"" style=""display: none; width: 100%;"">
                    <table style=""text-align: center; width: 100%;"">
                        <tr style=""min-height: 17px"">
                            <td style=""text-align: right;"">                                
                                <span id=""lblRemovedDateLightBox"" class=""InputLabel"">Изключен от отчет:</span>                                
                            </td>
                            <td style=""text-align: left;"">
                                <span id=""spanRemovedDateLightBox"">
                                    <input id=""txtRemovedDateLightBox"" class=""" + CommonFunctions.DatePickerCSS() + @""" style=""width: 80px;"" UnsavedCheckSkipMe=""true"" inLightBox=""true""></input>
                                </span>
                            </td>
                            <td style=""text-align: right;"">
                                <span id=""lblRemovedReasonLightBox"" class=""InputLabel"">Причина за изключване:</span>
                            </td>
                            <td style=""text-align: left;""> "
                                + ddRemovedReasonsHtml +
                            @"
                            </td>
                        </tr>
                    </table>
                </div>             
                <div id=""divTemporaryRemoved"" style=""display: none; width: 100%;"">
                    <table style=""text-align: center; width: 100%;"">
                        <tr style=""min-height: 17px"">
                            <td style=""text-align: right;"">
                                <span id=""lblTemporaryRemovedReasonLightBox"" class=""InputLabel"">Причина за временно снемане:</span>
                            </td>
                            <td style=""text-align: left;""> "
                                + ddTemporaryRemovedReasonsHtml +
                            @"
                            </td>
                            <td style=""text-align: right;"">
                                <span id=""lblTemporaryRemovedDateLightBox"" class=""InputLabel"">Начална дата:</span>
                            </td>
                            <td style=""text-align: left;"">
                                <span id=""spanTemporaryRemovedDateLightBox"">
                                    <input id=""txtTemporaryRemovedDateLightBox"" class=""" + CommonFunctions.DatePickerCSS() + @""" style=""width: 80px;"" UnsavedCheckSkipMe=""true"" inLightBox=""true""></input>
                                <span>
                            </td>
                            <td style=""text-align: right;"">
                                <span id=""lblTemporaryRemovedDurationLightBox"" class=""InputLabel"">Продължителност:</span>
                            </td>
                            <td style=""text-align: left;"">
                                <input id=""txtTemporaryRemovedDurationLightBox"" class=""InputField"" style=""width: 40px"" UnsavedCheckSkipMe=""true""></input>    
                                <span class=""InputLabel"">месеца</span>                           
                            </td>
                        </tr>
                    </table>
                </div>
                <div id=""divPostpone"" style=""display: none; width: 100%;"">
                    <table style=""text-align: center; width: 100%;"">
                        <tr style=""min-height: 17px"">
                            <td style=""text-align: right;"">
                                <span id=""lblPostponeTypeLightBox"" class=""InputLabel"">Вид отсрочване:</span>
                            </td>
                            <td style=""text-align: left;""> "
                                + ddTechnicsPostponeTypeHtml +
                            @"
                            </td>
                            <td style=""text-align: right;"">
                                <span id=""lblPostponeOwnerLightBox"" class=""InputLabel"">Собственик:</span>
                            </td>
                            <td style=""text-align: left;"">
                                <span id=""lblPostponeOwnerValueLightBox"" class=""ReadOnlyValue"">" + technicsPostponeOwner + @"</span>
                            </td>   
                        </tr>
                        <tr style=""min-height: 17px"">
                            <td style=""text-align: right;"">
                                <span id=""lblPostponeYearLightBox"" class=""InputLabel"">За коя година:</span>
                            </td>
                            <td style=""text-align: left;"">
                                <input id=""txtPostponeYearLightBox"" class=""RequiredInputField"" style=""width: 40px"" UnsavedCheckSkipMe=""true""></input>
                            </td>
                            <td style=""text-align: right; vertical-align: top;"">
                                <span id=""lblPostponeTechnicsSubTypeNameLightBox"" class=""InputLabel"">Тип на техниката:</span>
                            </td>
                            <td style=""text-align: left; vertical-align: top;"">
                                <span id=""lblPostponeTechnicsSubTypeNameValueLightBox"" class=""ReadOnlyValue"">" + technicsPostponeTechnicsSubTypeName + @"</span>
                            </td>                           
                        </tr>                        
                    </table>
                </div>
            </td>                    
        </tr>
        <tr style=""height: 46px; padding-top: 5px;"">
            <td colspan=""4"">
                <span id=""spanAddEditMilRepStatusLightBox"" class=""ErrorText"" style=""display: none;"">
                </span>&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan=""4"" style=""text-align: center;"">
                <table style=""margin: 0 auto;"">
                    <tr>
                        <td>
                            <div id=""btnSaveAddEditMilRepStatusLightBox"" style=""display: inline;"" onclick=""SaveAddEditMilRepStatusLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnSaveAddEditMilRepStatusLightBoxText"" style=""width: 70px;"">
                                    Запис</div>
                                <b></b>
                            </div>
                            <div id=""btnCloseAddEditCivilEducationLightBox"" style=""display: inline;"" onclick=""HideAddEditMilRepStatusLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnCloseAddEditCivilEducationLightBoxText"" style=""width: 70px;"">
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
";

            return html;
        }

        public static string GetMilRepStatusHistoryLightBox(User currentUser, HttpRequest Request)
        {
            string html = "";

            string htmlNoResults = "";

            List<TechnicsMilRepStatus> technicsMilRepStatuses = new List<TechnicsMilRepStatus>();
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

            int technicsId = 0;
            int.TryParse((Request.Params["TechnicsId"]).ToString(), out technicsId);

            allRows = TechnicsMilRepStatusUtil.GetAllTechnicsMilRepStatusByTechnicsIdCount(technicsId, currentUser);
            maxPage = allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            technicsMilRepStatuses = TechnicsMilRepStatusUtil.GetAllTechnicsMilRepStatusByTechnicsId(technicsId, orderBy, pageIndex, pageLength, currentUser);

            // No data found
            if (technicsMilRepStatuses.Count == 0)
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

                        <span class='HeaderText'>История на отчета</span><br /><br /><br />

                        <div style='text-align: center;'>
                           <div style='display: inline; position: relative; top: -10px;'>
                              <img id='btnTableFirst' " + btnFirst + @" alt='Първа страница' title='Първа страница' class='PaginationButton' onclick=""BtnMilRepStatusHistoryPagingClick('btnFirst');"" />
                              <img id='btnTablePrev' " + btnPrev + @" alt='Предишна страница' title='Предишна страница' class='PaginationButton' onclick=""BtnMilRepStatusHistoryPagingClick('btnPrev');"" />
                              <span id='lblTablePagination' class='PaginationLabel'>" + pageTablePagination + @"</span>
                              <img id='btnTableNext' " + btnNext + @" alt='Следваща страница' title='Следваща страница' class='PaginationButton' onclick=""BtnMilRepStatusHistoryPagingClick('btnNext');"" />
                              <img id='btnTableLast' " + btnLast + @" alt='Последна страница' title='Последна страница' class='PaginationButton' onclick=""BtnMilRepStatusHistoryPagingClick('btnLast');"" />
                              
                              <span style='padding: 0 30px'>&nbsp;</span>
                              <span style='text-align: right;'>Отиди на страница</span>
                              <input id='txtTableGotoPage' class='InputField' type='text' style='width: 30px;' value='' />
                              <img id='btnTableGoto' src='../Images/ButtonGoto.png' alt='Отиди на страница' title='Отиди на страница' class='PaginationButton' onclick=""BtnMilRepStatusHistoryPagingClick('btnPageGo');"" />
                           </div>
                        </div>

                        <table id='tblTechnicsMilRepStatusHistory' class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            
                         </thead>";

            //Set Table Results
            string headerStyle = "vertical-align: bottom;";

            //Setup the header of the grid
            html += @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='width: 80px; " + headerStyle + @"'>Дата</th>
                               <th style='width: 180px; " + headerStyle + @"'>Статус</th>
                               <th style='width: 180px; " + headerStyle + @"'>ВО</th>
                               <th style='width: 80px; " + headerStyle + @"'>Отчислен</th>                              
                           </tr></thead>";

            int counter = 1;

            //Iterate through all items and add them into the grid
            foreach (TechnicsMilRepStatus technicsMilRepStatus in technicsMilRepStatuses)
            {

                string cellStyle = "vertical-align: top;";

                html += @"<tr  class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"' >
                                 <td style='" + cellStyle + @"'>" + ((pageIndex - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + CommonFunctions.FormatDate(technicsMilRepStatus.EnrolDate) + @"</td>
                                 <td style='" + cellStyle + @"'>" + (technicsMilRepStatus.TechMilitaryReportStatus != null ? technicsMilRepStatus.TechMilitaryReportStatus.TechMilitaryReportStatusName : "") + @"</td>
                                 <td style='" + cellStyle + @"'>" + (technicsMilRepStatus.SourceMilDepartment != null ? technicsMilRepStatus.SourceMilDepartment.MilitaryDepartmentName : "") + @"</td>
                                 <td style='" + cellStyle + @"'>" + CommonFunctions.FormatDate(technicsMilRepStatus.DischargeDate) + @"</td>                                 
                              </tr>";

                counter++;
            }
            html += "</table>";

            html += "<input type='hidden' id='hdnTechnicsMilRepStatusHistoryCounter' value='" + counter + "' />";


            html += @"<div style='height: 10px;'>
                </div>

                 <div style='text-align: center'>
                   <span id='lblTechnicsMilRepStatusHistoryMessage'>" + htmlNoResults + @"</span><br/>
                </div>
";

            html += @"  </div>                        
                        <div id='btnCloseTechnicsMilRepStatusHistoryTable' runat='server' class='Button' onclick=""HideTechnicsMilRepStatusHistoryLightBox();""><i></i><div style='width:70px; padding-left:5px;'>Затвори</div><b></b></div>
                      </center>";



            return html;
        }

        public static string GetTechnicsAppointmentSection(int technicsId, User currentUser, RESPage page)
        {
            TechnicsAppointment technicsAppointment = TechnicsAppointmentUtil.GetCurrentTechnicsAppointmentByTechnicsId(technicsId, currentUser);
            FillTechnicsRequest request = FillTechnicsRequestUtil.GetFillTechnicsRequestByTechnicsId(technicsId, currentUser).FirstOrDefault();

            string reqOrderNumber = technicsAppointment != null ? technicsAppointment.ReqOrderNumber : "";
            string receiptAppointmentDate = technicsAppointment != null ? CommonFunctions.FormatDate(technicsAppointment.ReceiptAppointmentDate) : "";
            string militaryCommandName = technicsAppointment != null ? technicsAppointment.MilitaryCommandName : "";
            string militaryCommandSuffix = technicsAppointment != null ? technicsAppointment.MilitaryCommandSuffix : "";
            string technicsReadiness = technicsAppointment != null && technicsAppointment.TechnicsReadinessId.HasValue ? ReadinessUtil.ReadinessName(technicsAppointment.TechnicsReadinessId.Value) : "";
            string comment = technicsAppointment != null ? technicsAppointment.Comment : "";
            string appointmentTime = technicsAppointment != null && technicsAppointment.AppointmentTime.HasValue ? technicsAppointment.AppointmentTime.Value.ToString() : "";
            string appointmentPlace = technicsAppointment != null ? technicsAppointment.AppointmentPlace : "";
            string appointmentIsDelivered = request != null && request.AppointmentIsDelivered ? @"checked=""checked""" : "";

            string btnHistoryAppointmentsHTML = "";
            btnHistoryAppointmentsHTML = @"<img id=""btnHistoryAppointments"" src='../Images/index_view.png' alt='История' title='История' class='GridActionIcon' style='width: 22px; height: 22px;' onclick='btnHistoryAppointments_Click();'  />";

            string btnPrintMKHTML = "";
            string btnPrintPZHTML = "";

            if (technicsAppointment != null &&
                page.GetUIItemAccessLevel("RES_PRINT") != UIAccessLevel.Hidden &&
                page.GetUIItemAccessLevel("RES_PRINT_TECHNICS") != UIAccessLevel.Hidden)
            {
                if(page.GetUIItemAccessLevel("RES_PRINT_TECHNICS_MK") != UIAccessLevel.Hidden)
                    btnPrintMKHTML = @"<div id=""btnPrintMK"" style=""display: inline;"" onclick=""PrintMK();""class=""Button"">
                                           <i></i>
                                           <div id=""btnPrintMKText"" style=""width: 90px;"">Печат на МК</div>
                                           <b></b>
                                       </div>";

                if (page.GetUIItemAccessLevel("RES_PRINT_TECHNICS_PZ") != UIAccessLevel.Hidden)
                    btnPrintPZHTML = @"<div id=""btnPrintPZ"" style=""display: inline;"" onclick=""PrintPZ();""class=""Button"">
                                           <i></i>
                                           <div id=""btnPrintPZText"" style=""width: 90px;"">Печат на ПЗ</div>
                                           <b></b>
                                       </div>";
            }

            string html = @"
<fieldset style=""width: 830px; padding: 0px;"">
   <input type=""hidden"" id=""technicsAppointmentId"" value=""" + (technicsAppointment != null ? technicsAppointment.TechnicsAppointmentId.ToString() : "0") + @""" />
   <table class=""InputRegion"" style=""width: 830px; padding: 10px; padding-left: 0px; padding-top: 0px; margin-top: 0px;"">
      <tr style=""height: 3px;"">
      </tr>
      <tr>
         <td style=""text-align: left;"">
            <table>
                <tr>
                    <td style=""text-align: right; width: 150px;"">
                        <span id=""lblReqOrderNumber"" class=""InputLabel"">Номер на заявка:</span>
                    </td>
                    <td style=""text-align: left; width: 120px;"">
                        <span id=""lblReqOrderNumberValue"" class=""ReadOnlyValue"">" + reqOrderNumber + @"</span>
                    </td>
                    <td style=""text-align: right; width: 260px;"">
                        <span id=""lblReceiptAppointmentDate"" class=""InputLabel"">Дата на получаване на назначение:</span>
                    </td>
                    <td style=""text-align: left; width: 150px;"">
                        <span id=""lblReceiptAppointmentDateValue"" class=""ReadOnlyValue"">" + receiptAppointmentDate + @"</span>
                    </td>
                    <td style=""width: 150px;"">
                       <div style=""text-align: right; position: relative; top: -5px; left: 5px;"">
                          " + btnHistoryAppointmentsHTML + @"
                       </div>
                    </td>
                </tr>
                <tr>
                    <td style=""text-align: right; vertical-align: top;"">
                        <span id=""lblMilitaryCommandName"" class=""InputLabel"">Команда:</span>
                    </td>
                    <td style=""text-align: left; vertical-align: top;"">
                        <span id=""lblMilitaryCommandNameValue"" class=""ReadOnlyValue"">" + militaryCommandName + @"</span>
                    </td>
                    <td style=""text-align: right; vertical-align: top;"">
                        <span id=""lblTechnicsReadiness"" class=""InputLabel"">Начин на явяване:</span>
                    </td>
                    <td style=""text-align: left; vertical-align: top;"">
                        <span id=""lblTechnicsReadinessValue"" class=""ReadOnlyValue"">" + technicsReadiness + @"</span>
                    </td>
                </tr> 
                <tr>
                    <td style=""text-align: right; vertical-align: top;"">
                        <span id=""lblMilitaryCommandSuffix"" class=""InputLabel"">Буква:</span>
                    </td>
                    <td style=""text-align: left; vertical-align: top;"">
                        <span id=""lblMilitaryCommandSuffixValue"" class=""ReadOnlyValue"">" + militaryCommandSuffix + @"</span>
                    </td>
                </tr>
                <tr>         
                    <td style=""text-align: right;"">
                        <span id=""lblAppointmentComment"" class=""InputLabel"">Коментар:</span>
                    </td>
                    <td colspan=""3"" style=""text-align: left;"">
                        <span id=""lblAppointmentCommentValue"" class=""ReadOnlyValue"">" + comment + @"</span>
                    </td>        
                </tr>           
                <tr>
                    <td style=""text-align: right;"">
                        <span id=""lblAppointmentTime"" class=""InputLabel"">Време за явяване:</span>
                    </td>
                    <td style=""text-align: left;"">
                        <span id=""lblAppointmentTimeValue"" class=""ReadOnlyValue"">" + appointmentTime + @"</span>
                    </td>
                    <td style=""text-align: right; display: none;"">
                        <span id=""lblAppointmentPlace"" class=""InputLabel"">Място на явяване:</span>
                    </td>
                    <td style=""text-align: left; display: none;"">
                        <span id=""lblAppointmentPlaceValue"" class=""ReadOnlyValue"">" + appointmentPlace + @"</span>
                    </td>
                </tr>" + (request != null ?
                @"<tr>
                    <td style=""text-align: right; vertical-align: top;"">
                        <input type=""checkbox"" id=""cbAppointmentIsDelivered"" class=""InputLabel"" " + appointmentIsDelivered + @"/>                        
                    </td>
                    <td colspan=""3"" style=""text-align: left;"">
                        <span id=""lblAppointmentIsDelivered"" class=""ReadOnlyValue"">Връчено МН</span>
                    </td>
                </tr> " : "") + @"
                <tr>        
                    <td colspan=""4"" style=""text-align: center;"">
                        <span id=""spanTechnicsAppointmentSectionMsg"" class=""ErrorText"" style=""display: none;""></span>&nbsp;
                    </td>
                </tr>
                <tr>        
                    <td colspan=""5"" style=""text-align: center; position: relative; height: 15px;"">
                        <div style=""position: absolute; top: 0px; right: -30px; width: 250px;"">
                            " + btnPrintMKHTML + @"
                            " + btnPrintPZHTML + @"
                        </div>
                    </td>
                </tr>
            </table>
         </td>
      </tr>
   </table>
</fieldset>
";

            return html;
        }

        public static string GetTechnicsAppointmentHistoryLightBox(User currentUser, HttpRequest Request, AddEditTechnics_MilitaryReport page)
        {
            int technicsId = 0;
            int.TryParse((Request.Params["TechnicsId"]).ToString(), out technicsId);

            string html = "";

            string htmlNoResults = "";

            string technicsTypeKey = TechnicsUtil.GetTechnics(technicsId, currentUser).TechnicsType.TypeKey;

            bool IsReqOrderNumberHidden = page.GetUIItemAccessLevel("RES_TECHNICS_" + technicsTypeKey + "_EDIT_MILREP_APPOINTMENT_REQORDERNUMBER") == UIAccessLevel.Hidden;
            bool IsReceiptAppointmentDateHidden = page.GetUIItemAccessLevel("RES_TECHNICS_" + technicsTypeKey + "_EDIT_MILREP_APPOINTMENT_RECEIPTAPPOINTMENTDATE") == UIAccessLevel.Hidden;
            bool IsMilitaryCommandNameHidden = page.GetUIItemAccessLevel("RES_TECHNICS_" + technicsTypeKey + "_EDIT_MILREP_APPOINTMENT_MILITARYCOMMAND") == UIAccessLevel.Hidden;
            bool IsMilitaryCommandSuffixHidden = page.GetUIItemAccessLevel("RES_TECHNICS_" + technicsTypeKey + "_EDIT_MILREP_APPOINTMENT_MILITARYCOMMANDSUFFIX") == UIAccessLevel.Hidden;
            bool IsTechnicsReadinessHidden = page.GetUIItemAccessLevel("RES_TECHNICS_" + technicsTypeKey + "_EDIT_MILREP_APPOINTMENT_READINESS") == UIAccessLevel.Hidden;
            bool IsCommentHidden = page.GetUIItemAccessLevel("RES_TECHNICS_" + technicsTypeKey + "_EDIT_MILREP_APPOINTMENT_COMMENT") == UIAccessLevel.Hidden;
            bool IsAppointmentTimeHidden = page.GetUIItemAccessLevel("RES_TECHNICS_" + technicsTypeKey + "_EDIT_MILREP_APPOINTMENT_APPOINTMENTTIME") == UIAccessLevel.Hidden;
            bool IsAppointmentPlaceHidden = page.GetUIItemAccessLevel("RES_TECHNICS_" + technicsTypeKey + "_EDIT_MILREP_APPOINTMENT_APPOINTMENTPLACE") == UIAccessLevel.Hidden;

            List<TechnicsAppointment> technicsAppointments = new List<TechnicsAppointment>();
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

            

            allRows = TechnicsAppointmentUtil.GetAllTechnicsAppointmentsByTechnicsIdCount(technicsId, currentUser);
            maxPage = allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            technicsAppointments = TechnicsAppointmentUtil.GetAllTechnicsAppointmentsByTechnicsId(technicsId, orderBy, pageIndex, pageLength, currentUser);

            // No data found
            if (technicsAppointments.Count == 0)
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

                        <span class='HeaderText'>История на мобилизационните назначения</span><br /><br /><br />

                        <div style='text-align: center;'>
                           <div style='display: inline; position: relative; top: -10px;'>
                              <img id='btnTableFirst' " + btnFirst + @" alt='Първа страница' title='Първа страница' class='PaginationButton' onclick=""BtnTechnicsAppointmentHistoryPagingClick('btnFirst');"" />
                              <img id='btnTablePrev' " + btnPrev + @" alt='Предишна страница' title='Предишна страница' class='PaginationButton' onclick=""BtnTechnicsAppointmentHistoryPagingClick('btnPrev');"" />
                              <span id='lblTablePagination' class='PaginationLabel'>" + pageTablePagination + @"</span>
                              <img id='btnTableNext' " + btnNext + @" alt='Следваща страница' title='Следваща страница' class='PaginationButton' onclick=""BtnTechnicsAppointmentHistoryPagingClick('btnNext');"" />
                              <img id='btnTableLast' " + btnLast + @" alt='Последна страница' title='Последна страница' class='PaginationButton' onclick=""BtnTechnicsAppointmentHistoryPagingClick('btnLast');"" />
                              
                              <span style='padding: 0 30px'>&nbsp;</span>
                              <span style='text-align: right;'>Отиди на страница</span>
                              <input id='txtTableGotoPage' class='InputField' type='text' style='width: 30px;' value='' />
                              <img id='btnTableGoto' src='../Images/ButtonGoto.png' alt='Отиди на страница' title='Отиди на страница' class='PaginationButton' onclick=""BtnTechnicsAppointmentHistoryPagingClick('btnPageGo');"" />
                           </div>
                        </div>

                        <table id='tblTechnicsAppointmentHistory' class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            
                         </thead>";

            //Set Table Results
            string headerStyle = "vertical-align: bottom;";

            //Setup the header of the grid
            html += @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>" +
  (!IsReqOrderNumberHidden ? @"<th style='width: 80px; " + headerStyle + @"'>Номер на заявка</th>" : "") +
  (!IsReceiptAppointmentDateHidden ? @"<th style='width: 130px; " + headerStyle + @"'>Дата на получаване на назначение</th>" : "") +
  (!IsMilitaryCommandNameHidden ? @"<th style='width: 180px; " + headerStyle + @"'>Команда</th>" : "") +
  (!IsMilitaryCommandSuffixHidden ? @"<th style='width: 50px; " + headerStyle + @"'>Буква</th>" : "") +   
  (!IsTechnicsReadinessHidden ? @"<th style='width: 180px; " + headerStyle + @"'>Начин на явяване</th>" : "") +  
  (!IsCommentHidden ? @"<th style='width: 180px; " + headerStyle + @"'>Коментар</th>" : "") +
  (!IsAppointmentTimeHidden ? @"<th style='width: 80px; " + headerStyle + @"'>Време за явяване</th>" : "") +
  (!IsAppointmentPlaceHidden ? @"<th style='width: 180px; display: none; " + headerStyle + @"'>Място на явяване</th>" : "") +
                           @"</tr></thead>";

            int counter = 1;

            //Iterate through all items and add them into the grid
            foreach (TechnicsAppointment technicsAppointment in technicsAppointments)
            {
                string cellStyle = "vertical-align: top;";

                html += @"<tr  class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"' >
                                 <td style='" + cellStyle + @"'>" + ((pageIndex - 1) * pageLength + counter).ToString() + @"</td>" +
  (!IsReqOrderNumberHidden ? @"<td style='" + cellStyle + @"'>" + technicsAppointment.ReqOrderNumber + @"</td>" : "") +
  (!IsReceiptAppointmentDateHidden ? @"<td style='" + cellStyle + @"'>" + CommonFunctions.FormatDate(technicsAppointment.ReceiptAppointmentDate) + @"</td>" : "") +
  (!IsMilitaryCommandNameHidden ? @"<td style='" + cellStyle + @"'>" + technicsAppointment.MilitaryCommandName + @"</td>" : "") +
  (!IsMilitaryCommandSuffixHidden ? @"<td style='" + cellStyle + @"'>" + technicsAppointment.MilitaryCommandSuffix + @"</td>" : "") +
  (!IsTechnicsReadinessHidden ? @"<td style='" + cellStyle + @"'>" + (technicsAppointment.TechnicsReadinessId.HasValue ? ReadinessUtil.ReadinessName(technicsAppointment.TechnicsReadinessId.Value) : "") + @"</td>" : "") +
  (!IsCommentHidden ? @"<td style='" + cellStyle + @"'>" + technicsAppointment.Comment + @"</td>" : "") +
  (!IsAppointmentTimeHidden ? @"<td style='" + cellStyle + @"'>" + technicsAppointment.AppointmentTime + @"</td>" : "") +
  (!IsAppointmentPlaceHidden ? @"<td style='display: none; " + cellStyle + @"'>" + technicsAppointment.AppointmentPlace + @"</td>" : "") +
                              @"</tr>";

                counter++;
            }
            html += "</table>";

            html += "<input type='hidden' id='hdnTechnicsAppointmentHistoryCounter' value='" + counter + "' />";


            html += @"<div style='height: 10px;'>
                </div>

                 <div style='text-align: center'>
                   <span id='lblTechnicsAppointmentHistoryMessage'>" + htmlNoResults + @"</span><br/>
                </div>
";

            html += @"  </div>                        
                        <div id='btnCloseTechnicsAppointmentHistoryTable' runat='server' class='Button' onclick=""HideTechnicsAppointmentHistoryLightBox();""><i></i><div style='width:70px; padding-left:5px;'>Затвори</div><b></b></div>
                      </center>";



            return html;
        }

        public static string GetGroupManagementSection(int technicsId, User currentUser)
        {
            string ddGMSPunktHtml = "";

            Technics technics = TechnicsUtil.GetTechnics(technicsId, currentUser);

            TechnicsMilRepStatus currentMilRepStatus = TechnicsMilRepStatusUtil.GetTechnicsMilRepCurrentStatusByTechnicsId(technicsId, currentUser);

            List<TechnicsRequestCommandPunkt> requestCommandPunkts = new List<TechnicsRequestCommandPunkt>();

            if (currentMilRepStatus != null && currentMilRepStatus.SourceMilDepartmentId.HasValue)
                requestCommandPunkts = TechnicsRequestCommandPunktUtil.GetAllTechnicsRequestCommandPunktByMilDeptID(currentMilRepStatus.SourceMilDepartmentId.Value, currentUser);

            List<IDropDownItem> requestCommandPunktsDropDownItems = new List<IDropDownItem>();
            foreach (TechnicsRequestCommandPunkt requestCommandPunkt in requestCommandPunkts)
                requestCommandPunktsDropDownItems.Add(requestCommandPunkt as IDropDownItem);

            ddGMSPunktHtml = ListItems.GetDropDownHtml(requestCommandPunktsDropDownItems, null, "ddGMSPunkt", true, technics.Punkt, null, @"style=""width: 180px;""");

            string groupManagementSection = technics != null ? technics.GroupManagementSection : "";
            string section = technics != null ? technics.Section : "";
            string deliverer = technics != null ? technics.Deliverer : "";

            string html = @"
<fieldset style=""width: 830px; padding: 0px;"">   
   <table class=""InputRegion"" style=""width: 830px; padding: 10px; padding-left: 0px; padding-top: 0px; margin-top: 0px;"">
      <tr style=""height: 10px;"">
      </tr>
      <tr>
           <td style=""text-align: right; width: 150px;"">
                <span id=""lblGMSGroupManagementSection"" class=""InputLabel"">ГРС:</span>
           </td>
           <td style=""text-align: left; width: 250px;"">
                <input type=""text"" id=""txtGMSGroupManagementSection"" class='InputField' value='" + groupManagementSection + @"' />
           </td>
           <td style=""text-align: right;"">
                <span id=""lblGMSSection"" class=""InputLabel"">Секция:</span>
           </td>
           <td style=""text-align: left;"">
                <input type=""text"" id=""txtGMSSection"" class='InputField' value='" + section + @"' />
           </td>
      </tr>
      <tr>
           <td style=""text-align: right;"">
                <span id=""lblGMSDeliverer"" class=""InputLabel"">Връчител:</span>
           </td>
           <td style=""text-align: left;"">
                <input type=""text"" id=""txtGMSDeliverer"" class='InputField' value='" + deliverer + @"' />
           </td>
           <td style=""text-align: right;"">
                <span id=""lblGMSPunkt"" class=""InputLabel"">ПИ/КПП:</span>
           </td>
            <td style=""text-align: left;""> "
              + ddGMSPunktHtml +
             @"</td>
       </tr>                 
                <tr>        
                    <td colspan=""4"" style=""text-align: center;"">
                        <span id=""spanGMSMsg"" class=""ErrorText"" style=""display: none;""></span>&nbsp;
                    </td>        
                </tr>
            </table>
         </td>
      </tr>
   </table>
</fieldset>
";

            return html;
        }

        public static string GetTabUIItems(AddEditTechnics page)
        {
            string UIItemsXML = "";

            bool isPreview = false;
            if (!String.IsNullOrEmpty(page.Request.Params["Preview"]))
            {
                isPreview = true;
            }

            bool screenDisabled = false;

            List<string> disabledClientControls = new List<string>();
            List<string> hiddenClientControls = new List<string>();

            screenDisabled = page.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Disabled ||
                             page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey) == UIAccessLevel.Disabled ||
                             page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT") == UIAccessLevel.Disabled ||
                             page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP") == UIAccessLevel.Disabled || isPreview;                    

            UIAccessLevel l;
                   

            // section Military Report Status

            bool milReportStatusSectionDisabled = screenDisabled || page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP_MILREPSTATUS") == UIAccessLevel.Disabled;

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_MILREPSTATUS_MILREPSTATUS");

            if (l != UIAccessLevel.Enabled || milReportStatusSectionDisabled)
            {
                hiddenClientControls.Add("btnAddNewResMilRepStatus");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_MILREPSTATUS_HISTORY");

            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("btnHistoryStatuses");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_MILREPSTATUS_EDITMILREPSTATUS");

            if (l != UIAccessLevel.Enabled || milReportStatusSectionDisabled)
            {
                hiddenClientControls.Add("btnEditCurrResMilRepStatus");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_MILREPSTATUS_EDITMILREPSTATUS_ENROLDATE");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblCurrEnrolDateLightBox");
                disabledClientControls.Add("txtCurrEnrolDateLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblFirstEnrolDate");
                hiddenClientControls.Add("lblFirstEnrolDateValue");
                hiddenClientControls.Add("lblCurrEnrolDate");
                hiddenClientControls.Add("lblCurrEnrolDateValue");
                hiddenClientControls.Add("lblCurrEnrolDateLightBox");
                hiddenClientControls.Add("spanCurrEnrolDateLightBox");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_MILREPSTATUS_EDITMILREPSTATUS_DISCHARGEDATE");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblDischargeDateLightBox");
                disabledClientControls.Add("txtDischargeDateLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblDischargeDateLightBox");
                hiddenClientControls.Add("spanDischargeDateLightBox");
                hiddenClientControls.Add("lblDischargeDate");
                hiddenClientControls.Add("txtDischargeDate");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_MILREPSTATUS_EDITMILREPSTATUS_SOURCEMILDEPT");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblCurrSourceMilDepartmentNameLightBox");
                disabledClientControls.Add("ddCurrSourceMilDepartmentNameLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblCurrSourceMilDepartmentNameLightBox");
                hiddenClientControls.Add("ddCurrSourceMilDepartmentNameLightBox");
                hiddenClientControls.Add("lblCurrSourceMilDepartmentName");
                hiddenClientControls.Add("lblCurrSourceMilDepartmentNameValue");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_MILREPSTATUS_EDITMILREPSTATUS_CONTRACTCONTRACTNUMBER");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblContractContractNumberLightBox");
                disabledClientControls.Add("txtContractContractNumberLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblContractContractNumberLightBox");
                hiddenClientControls.Add("txtContractContractNumberLightBox");
                hiddenClientControls.Add("lblContractContractNumber");
                hiddenClientControls.Add("txtContractContractNumber");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_MILREPSTATUS_EDITMILREPSTATUS_CONTRACTCONTRACTFROMDATE");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblContractContractFromDateLightBox");
                disabledClientControls.Add("txtContractContractFromDateLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblContractContractFromDateLightBox");
                hiddenClientControls.Add("spanContractContractFromDateLightBox");
                hiddenClientControls.Add("lblContractContractFromDate");
                hiddenClientControls.Add("txtContractContractFromDate");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_MILREPSTATUS_EDITMILREPSTATUS_CONTRACTCONTRACTTODATE");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblContractContractToDateLightBox");
                disabledClientControls.Add("txtContractContractToDateLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblContractContractToDateLightBox");
                hiddenClientControls.Add("spanContractContractToDateLightBox");
                hiddenClientControls.Add("lblContractContractToDate");
                hiddenClientControls.Add("txtContractContractToDate");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_MILREPSTATUS_EDITMILREPSTATUS_CONTRACTMILITARYUNIT");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblContractMilitaryUnitLightBox");
                disabledClientControls.Add("muContractMilitaryUnitLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblContractMilitaryUnitLightBox");
                hiddenClientControls.Add("muContractMilitaryUnitLightBox");
                hiddenClientControls.Add("lblContractMilitaryUnit");
                hiddenClientControls.Add("txtContractMilitaryUnit");
            }      

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_MILREPSTATUS_EDITMILREPSTATUS_VOLUNTARYCONTRACTNUMBER");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblVoluntaryContractNumberLightBox");
                disabledClientControls.Add("txtVoluntaryContractNumberLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblVoluntaryContractNumberLightBox");
                hiddenClientControls.Add("txtVoluntaryContractNumberLightBox");
                hiddenClientControls.Add("lblVoluntaryContractNumber");
                hiddenClientControls.Add("txtVoluntaryContractNumber");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_MILREPSTATUS_EDITMILREPSTATUS_VOLUNTARYCONTRACTDATE");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblVoluntaryContractDateLightBox");
                disabledClientControls.Add("txtVoluntaryContractDateLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblVoluntaryContractDateLightBox");
                hiddenClientControls.Add("spanVoluntaryContractDateLightBox");
                hiddenClientControls.Add("lblVoluntaryContractDate");
                hiddenClientControls.Add("txtVoluntaryContractDate");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_MILREPSTATUS_EDITMILREPSTATUS_VOLUNTARYDURATIONMONTHS");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblVoluntaryDurationMonthsLightBox");
                disabledClientControls.Add("txtVoluntaryDurationMonthsLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblVoluntaryDurationMonthsLightBox");
                hiddenClientControls.Add("txtVoluntaryDurationMonthsLightBox");
                hiddenClientControls.Add("lblVoluntaryDurationMonths");
                hiddenClientControls.Add("txtVoluntaryDurationMonths");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_MILREPSTATUS_EDITMILREPSTATUS_VOLUNTARYCONTRACTTODATE");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblVoluntaryContractToDateLightBox");
                disabledClientControls.Add("txtVoluntaryContractToDateLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblVoluntaryContractToDateLightBox");
                hiddenClientControls.Add("spanVoluntaryContractToDateLightBox");
                hiddenClientControls.Add("lblVoluntaryContractToDate");
                hiddenClientControls.Add("txtVoluntaryContractToDate");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_MILREPSTATUS_EDITMILREPSTATUS_VOLUNTARYFULFILPLACE");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblVoluntaryFulfilPlaceLightBox");                
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblVoluntaryFulfilPlaceLightBox");
                hiddenClientControls.Add("spanVoluntaryFulfilPlaceLightBox");
                hiddenClientControls.Add("lblVoluntaryFulfilPlace");
                hiddenClientControls.Add("txtVoluntaryFulfilPlace");
            }           

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_MILREPSTATUS_EDITMILREPSTATUS_REMOVEDDATE");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblRemovedDateLightBox");
                disabledClientControls.Add("txtRemovedDateLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblRemovedDateLightBox");
                hiddenClientControls.Add("spanRemovedDateLightBox");
                hiddenClientControls.Add("lblRemovedDate");
                hiddenClientControls.Add("txtRemovedDate");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_MILREPSTATUS_EDITMILREPSTATUS_REMOVEDREASON");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblRemovedReasonLightBox");
                disabledClientControls.Add("ddRemovedReasonLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblRemovedReasonLightBox");
                hiddenClientControls.Add("ddRemovedReasonLightBox");
                hiddenClientControls.Add("lblRemovedReason");
                hiddenClientControls.Add("txtRemovedReason");
            }            

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_MILREPSTATUS_EDITMILREPSTATUS_TEMPREMOVEDREASON");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblTemporaryRemovedReasonLightBox");
                disabledClientControls.Add("ddTemporaryRemovedReasonsLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblTemporaryRemovedReasonLightBox");
                hiddenClientControls.Add("ddTemporaryRemovedReasonsLightBox");
                hiddenClientControls.Add("lblTemporaryRemovedReason");
                hiddenClientControls.Add("txtTemporaryRemovedReason");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_MILREPSTATUS_EDITMILREPSTATUS_TEMPREMOVEDDATE");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblTemporaryRemovedDateLightBox");
                disabledClientControls.Add("txtTemporaryRemovedDateLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblTemporaryRemovedDateLightBox");
                hiddenClientControls.Add("spanTemporaryRemovedDateLightBox");
                hiddenClientControls.Add("lblTemporaryRemovedDate");
                hiddenClientControls.Add("txtTemporaryRemovedDate");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_MILREPSTATUS_EDITMILREPSTATUS_TEMPREMOVEDDURATION");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblTemporaryRemovedDurationLightBox");
                disabledClientControls.Add("txtTemporaryRemovedDurationLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblTemporaryRemovedDurationLightBox");
                hiddenClientControls.Add("txtTemporaryRemovedDurationLightBox");
                hiddenClientControls.Add("lblTemporaryRemovedDuration");
                hiddenClientControls.Add("txtTemporaryRemovedDuration");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_MILREPSTATUS_EDITMILREPSTATUS_POSTPONETYPE");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblPostponeTypeLightBox");
                disabledClientControls.Add("ddPostponeTypeLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPostponeTypeLightBox");
                hiddenClientControls.Add("ddPostponeTypeLightBox");
                hiddenClientControls.Add("lblPostponeType");
                hiddenClientControls.Add("txtPostponeType");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_MILREPSTATUS_EDITMILREPSTATUS_POSTPONEYEAR");

            if (l == UIAccessLevel.Disabled || milReportStatusSectionDisabled)
            {
                disabledClientControls.Add("lblPostponeYearLightBox");
                disabledClientControls.Add("txtPostponeYearLightBox");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblPostponeYearLightBox");
                hiddenClientControls.Add("txtPostponeYearLightBox");
                hiddenClientControls.Add("lblPostponeYear");
                hiddenClientControls.Add("txtPostponeYear");
            }

            // section technics appointment

            bool reservistAppointmentSectionDisabled = screenDisabled || page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_APPOINTMENT") == UIAccessLevel.Disabled;

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_APPOINTMENT_HISTORY");

            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("btnHistoryAppointments");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_APPOINTMENT_REQORDERNUMBER");

            if (l == UIAccessLevel.Disabled || reservistAppointmentSectionDisabled)
            {
                disabledClientControls.Add("lblReqOrderNumber");
                disabledClientControls.Add("lblReqOrderNumberValue");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblReqOrderNumber");
                hiddenClientControls.Add("lblReqOrderNumberValue");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_APPOINTMENT_RECEIPTAPPOINTMENTDATE");

            if (l == UIAccessLevel.Disabled || reservistAppointmentSectionDisabled)
            {
                disabledClientControls.Add("lblReceiptAppointmentDate");
                disabledClientControls.Add("lblReceiptAppointmentDateValue");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblReceiptAppointmentDate");
                hiddenClientControls.Add("lblReceiptAppointmentDateValue");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_APPOINTMENT_MILITARYCOMMAND");

            if (l == UIAccessLevel.Disabled || reservistAppointmentSectionDisabled)
            {
                disabledClientControls.Add("lblMilitaryCommandName");
                disabledClientControls.Add("lblMilitaryCommandNameValue");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilitaryCommandName");
                hiddenClientControls.Add("lblMilitaryCommandNameValue");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_APPOINTMENT_MILITARYCOMMANDSUFFIX");

            if (l == UIAccessLevel.Disabled || reservistAppointmentSectionDisabled)
            {
                disabledClientControls.Add("lblMilitaryCommandSuffix");
                disabledClientControls.Add("lblMilitaryCommandSuffixValue");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblMilitaryCommandSuffix");
                hiddenClientControls.Add("lblMilitaryCommandSuffixValue");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_APPOINTMENT_READINESS");

            if (l == UIAccessLevel.Disabled || reservistAppointmentSectionDisabled)
            {
                disabledClientControls.Add("lblTechnicsReadiness");
                disabledClientControls.Add("lblTechnicsReadinessValue");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblTechnicsReadiness");
                hiddenClientControls.Add("lblTechnicsReadinessValue");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_APPOINTMENT_COMMENT");

            if (l == UIAccessLevel.Disabled || reservistAppointmentSectionDisabled)
            {
                disabledClientControls.Add("lblAppointmentComment");
                disabledClientControls.Add("lblAppointmentCommentValue");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblAppointmentComment");
                hiddenClientControls.Add("lblAppointmentCommentValue");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_APPOINTMENT_APPOINTMENTTIME");

            if (l == UIAccessLevel.Disabled || reservistAppointmentSectionDisabled)
            {
                disabledClientControls.Add("lblAppointmentTime");
                disabledClientControls.Add("lblAppointmentTimeValue");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblAppointmentTime");
                hiddenClientControls.Add("lblAppointmentTimeValue");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_APPOINTMENT_APPOINTMENTPLACE");

            if (l == UIAccessLevel.Disabled || reservistAppointmentSectionDisabled)
            {
                disabledClientControls.Add("lblAppointmentPlace");
                disabledClientControls.Add("lblAppointmentPlaceValue");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblAppointmentPlace");
                hiddenClientControls.Add("lblAppointmentPlaceValue");
            }


            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_APPOINTMENT_APPOINTMENTISDELIVERED");

            if (l == UIAccessLevel.Disabled || reservistAppointmentSectionDisabled)
            {
                disabledClientControls.Add("lblAppointmentIsDelivered");
                disabledClientControls.Add("cbAppointmentIsDelivered");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblAppointmentIsDelivered");
                hiddenClientControls.Add("cbAppointmentIsDelivered");
            }


            //section GroupManagementSection
            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_GROUPMANAGEMENTSECTION");

            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                disabledClientControls.Add("lblGMSGroupManagementSection");
                disabledClientControls.Add("txtGMSGroupManagementSection");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblGMSGroupManagementSection");
                hiddenClientControls.Add("txtGMSGroupManagementSection");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_SECTION");

            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                disabledClientControls.Add("lblGMSSection");
                disabledClientControls.Add("txtGMSSection");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblGMSSection");
                hiddenClientControls.Add("txtGMSSection");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_DELIVERER");

            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                disabledClientControls.Add("lblGMSDeliverer");
                disabledClientControls.Add("txtGMSDeliverer");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblGMSDeliverer");
                hiddenClientControls.Add("txtGMSDeliverer");
            }

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_MILREP_PUNKT");

            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                disabledClientControls.Add("lblGMSPunkt");
                disabledClientControls.Add("ddGMSPunkt");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblGMSPunkt");
                hiddenClientControls.Add("ddGMSPunkt");
            }          


            string disabledClientControlsIds = "";
            string hiddenClientControlsIds = "";

            foreach (string disabledClientControl in disabledClientControls)
            {
                disabledClientControlsIds += (String.IsNullOrEmpty(disabledClientControlsIds) ? "" : ",") +
                    disabledClientControl;
            }

            foreach (string hiddenClientControl in hiddenClientControls)
            {
                hiddenClientControlsIds += (String.IsNullOrEmpty(hiddenClientControlsIds) ? "" : ",") +
                    hiddenClientControl;
            }

            UIItemsXML = "<disabledClientControls>" + AJAXTools.EncodeForXML(disabledClientControlsIds) + "</disabledClientControls>" +
                         "<hiddenClientControls>" + AJAXTools.EncodeForXML(hiddenClientControlsIds) + "</hiddenClientControls>";

            return UIItemsXML;
        }

        public static string PrintMK(int technicsId, User currentUser)
        {
            List<int> technicsIDs = new List<int>();
            technicsIDs.Add(technicsId);

            string result = GeneratePrintTechnicsUtil.PrintMK(technicsIDs, currentUser);
            return result;
        }

        public static string PrintPZ(int technicsId, User currentUser)
        {
            List<int> technicsIDs = new List<int>();
            technicsIDs.Add(technicsId);

            string result = GeneratePrintTechnicsUtil.PrintPZ(technicsIDs, currentUser);
            return result;
        }

        public static string PrintOK(int technicsId, User currentUser)
        {
            List<int> technicsIDs = new List<int>();
            technicsIDs.Add(technicsId);

            string result = GeneratePrintTechnicsUtil.PrintOK(technicsIDs, currentUser);
            return result;
        }

        public static string PrintTO(int technicsId, User currentUser)
        {
            List<int> technicsIDs = new List<int>();
            technicsIDs.Add(technicsId);

            string result = GeneratePrintTechnicsUtil.PrintTO(technicsIDs, currentUser);
            return result;
        }
    }
}
