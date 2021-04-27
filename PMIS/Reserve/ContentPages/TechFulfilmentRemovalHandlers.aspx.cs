using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Text;
using System.Collections.Generic;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class TechFulfilmentRemovalHandlers : RESPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["AjaxMethod"] == null)
            {
                RedirectAccessDenied();
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetData")
            {
                JSGetData();
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRemoveTechnics")
            {
                JSRemoveTechnics();
            }
        }

        private void JSGetData()
        {
            int requestCommantID = int.Parse(Request.Form["RequestCommantID"]);
            int militaryDepartmentID = int.Parse(Request.Form["MilitaryDepartmentID"]);

            TechnicsRequestCommand requestCommand = TechnicsRequestCommandUtil.GetTechnicsRequestCommand(CurrentUser, requestCommantID);
            
            List<TechnicsRequestCommandPositionBlockForFulfilment> requestCommandPositions = TechnicsRequestCommandPositionUtil.GetTechnicsRequestCommandPositionsForFulfilment(CurrentUser, requestCommand.TechnicsRequestCommandId, militaryDepartmentID);
            
            StringBuilder response = new StringBuilder();

            response.Append("<Data>");

            response.Append("<RequestCommand>");

            response.Append("<RequestCommandID>");
            response.Append(requestCommand.TechnicsRequestCommandId);
            response.Append("</RequestCommandID>");

            response.Append("<RequestCommandName>");
            response.Append(AJAXTools.EncodeForXML(requestCommand.MilitaryCommand.DisplayTextForSelection));
            response.Append("</RequestCommandName>");

            response.Append("<SubRequestCommandName>");
            response.Append(AJAXTools.EncodeForXML(requestCommand.DisplayText2));
            response.Append("</SubRequestCommandName>");

            response.Append("</RequestCommand>");


            response.Append("<TechnicsRequestCommandPositions>");

            foreach (TechnicsRequestCommandPositionBlockForFulfilment trcp in requestCommandPositions)
            {
                response.Append("<TechnicsRequestCommandPosition>");

                response.Append("<TechnicsRequestCommandPositionID>");
                response.Append(trcp.TechnicsRequestCommandPositionId);
                response.Append("</TechnicsRequestCommandPositionID>");

                response.Append("<TechnicsType>");
                response.Append(AJAXTools.EncodeForXML(trcp.TechnicsType));
                response.Append("</TechnicsType>");

                response.Append("<NormativeTechnics>");
                response.Append(AJAXTools.EncodeForXML((trcp.NormativeTechnics != null ? trcp.NormativeTechnics.CodeAndText : "")));
                response.Append("</NormativeTechnics>");

                response.Append("<TechnicsComment>");
                response.Append(AJAXTools.EncodeForXML(trcp.TechnicsComment));
                response.Append("</TechnicsComment>");

                response.Append("<TechnicsCount>");
                response.Append(trcp.Count);
                response.Append("</TechnicsCount>");

                response.Append("<Fulfiled>");
                response.Append(trcp.Fulfiled + " (" + trcp.FulfiledReserve + ")");
                response.Append("</Fulfiled>");

                response.Append("</TechnicsRequestCommandPosition>");
            }

            response.Append("</TechnicsRequestCommandPositions>");
            response.Append("</Data>");

            AJAX a = new AJAX(response.ToString(), Response);
            a.Write();
            Response.End();
            return;
        }

        private void JSRemoveTechnics()
        {
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_EquipmentTechnicsRequests");
                List<string> IDs = Request.Form["TechnicsRequestCommandPositionIDs"].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();

                foreach (string ID in IDs)
                {
                    int technicsRequestCommandPositionID = int.Parse(ID);
                    List<FillTechnicsRequest> fillTechnicsRequests = FillTechnicsRequestUtil.GetFillTechnicsRequestByTechReqCommandPosition(technicsRequestCommandPositionID, CurrentUser);
                    
                    List<int> deletedTechnics = new List<int>();

                    //Remove the all Technics from that Military Command Position
                    foreach (FillTechnicsRequest fillTechnicsRequest in fillTechnicsRequests)
                    {
                        MilitaryDepartment militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment(fillTechnicsRequest.MilitaryDepartmentID, CurrentUser);
                        TechnicsRequestCommandPosition position = TechnicsRequestCommandPositionUtil.GetTechnicsRequestCommandPosition(CurrentUser, fillTechnicsRequest.TechnicsRequestCommandPositionID);
                        Technics technics = TechnicsUtil.GetTechnics(fillTechnicsRequest.TechnicsID, CurrentUser);

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
                                Vehicle vehicle = VehicleUtil.GetVehicleByTechnicsId(technics.TechnicsId, CurrentUser);
                                logDescription += "; Рег. №: " + vehicle.RegNumber;
                                changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteVehicle", logDescription, position.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, CurrentUser);

                                break;
                            case "TRAILERS":
                                Trailer trailer = TrailerUtil.GetTrailerByTechnicsId(technics.TechnicsId, CurrentUser);
                                logDescription += "; Рег. №: " + trailer.RegNumber;
                                changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteTrailer", logDescription, position.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, CurrentUser);

                                break;
                            case "TRACTORS":
                                Tractor tractor = TractorUtil.GetTractorByTechnicsId(technics.TechnicsId, CurrentUser);
                                logDescription += "; Рег. №: " + tractor.RegNumber;
                                changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteTractor", logDescription, position.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, CurrentUser);

                                break;
                            case "ENG_EQUIP":
                                EngEquip engEquip = EngEquipUtil.GetEngEquipByTechnicsId(technics.TechnicsId, CurrentUser);
                                logDescription += "; Рег. №: " + engEquip.RegNumber;
                                changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteEngEquip", logDescription, position.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, CurrentUser);

                                break;
                            case "MOB_LIFT_EQUIP":
                                MobileLiftingEquip mobileLiftingEquip = MobileLiftingEquipUtil.GetMobileLiftingEquipByTechnicsId(technics.TechnicsId, CurrentUser);
                                logDescription += "; Рег. №: " + mobileLiftingEquip.RegNumber;
                                changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteMobLiftEquip", logDescription, position.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, CurrentUser);

                                break;
                            case "RAILWAY_EQUIP":
                                RailwayEquip railwayEquip = RailwayEquipUtil.GetRailwayEquipByTechnicsId(technics.TechnicsId, CurrentUser);
                                logDescription += "; Инв. №: " + railwayEquip.InventoryNumber;
                                changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteRailwayEquip", logDescription, position.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, CurrentUser);

                                break;
                            case "AVIATION_EQUIP":
                                AviationEquip aviationEquip = AviationEquipUtil.GetAviationEquipByTechnicsId(technics.TechnicsId, CurrentUser);
                                logDescription += "; Инв. №: " + aviationEquip.AirInvNumber;
                                changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteAviationEquip", logDescription, position.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, CurrentUser);

                                break;
                            case "VESSELS":
                                Vessel vessel = VesselUtil.GetVesselByTechnicsId(technics.TechnicsId, CurrentUser);
                                logDescription += "; Име: " + vessel.VesselName +
                                                  "; Инв. №: " + vessel.InventoryNumber;
                                changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteVessel", logDescription, position.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, CurrentUser);

                                break;
                            case "FUEL_CONTAINERS":
                                FuelContainer fuelContainer = FuelContainerUtil.GetFuelContainerByTechnicsId(technics.TechnicsId, CurrentUser);
                                logDescription += "; Инв. №: " + fuelContainer.InventoryNumber;
                                changeEvent = new ChangeEvent("RES_EquipTechRequests_DeleteFuelContainer", logDescription, position.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, CurrentUser);

                                break;
                        }

                        FillTechnicsRequestUtil.DeleteRequestCommandTechnic(fillTechnicsRequest.FillTechnicsRequestID, CurrentUser, changeEvent);

                        change.AddEvent(changeEvent);

                        if (!deletedTechnics.Contains(technics.TechnicsId))
                        {
                            //Change the current Military Reporting Status of the chosen technics
                            TechnicsMilRepStatusUtil.SetMilRepStatusTo_FREE(technics.TechnicsId, CurrentUser, change);

                            //Clear the Mobilization Appointment
                            TechnicsAppointmentUtil.ClearTheCurrentTechnicsAppointmentByTechnics(technics.TechnicsId, CurrentUser, change);

                            deletedTechnics.Add(technics.TechnicsId);
                        }
                    }
                }

                if (change.HasEvents)
                    change.WriteLog();

                stat = AJAXTools.OK;
                response = "OK";
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
}
