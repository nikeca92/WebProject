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
    public partial class ResFulfilmentRemovalHandlers : RESPage
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

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRemoveReservists")
            {
                JSRemoveReservists();
            }
        }

        private void JSGetData()
        {
            int requestCommantID = int.Parse(Request.Form["RequestCommantID"]);
            int militaryDepartmentID = int.Parse(Request.Form["MilitaryDepartmentID"]);

            RequestCommand requestCommand = RequestCommandUtil.GetRequestsCommand(CurrentUser, requestCommantID);
            List<RequestCommandPositionBlockForFulfilment> requestCommandPositions = RequestCommandPositionUtil.GetRequestCommandPositionsForFulfilment(CurrentUser, requestCommand.RequestCommandId, militaryDepartmentID);
            
            StringBuilder response = new StringBuilder();
            
            response.Append("<Data>");

            response.Append("<RequestCommand>");

            response.Append("<RequestCommandID>");
            response.Append(requestCommand.RequestCommandId);
            response.Append("</RequestCommandID>");

            response.Append("<RequestCommandName>");
            response.Append(AJAXTools.EncodeForXML(requestCommand.MilitaryCommand.DisplayTextForSelection));
            response.Append("</RequestCommandName>");

            response.Append("<SubRequestCommandName>");
            response.Append(AJAXTools.EncodeForXML(requestCommand.DisplayText2));
            response.Append("</SubRequestCommandName>");

            response.Append("</RequestCommand>");

            
            response.Append("<RequestCommandPositions>");

            foreach (RequestCommandPositionBlockForFulfilment rcp in requestCommandPositions)
            {
                response.Append("<RequestCommandPosition>");

                response.Append("<RequestCommandPositionID>");
                response.Append(rcp.RequestCommandPositionId);
                response.Append("</RequestCommandPositionID>");

                response.Append("<Position>");
                response.Append(AJAXTools.EncodeForXML(rcp.Position));
                response.Append("</Position>");

                response.Append("<MilRepSpec>");
                response.Append(AJAXTools.EncodeForXML(rcp.MilRepSpecHTML));
                response.Append("</MilRepSpec>");

                response.Append("<MilitaryRank>");
                response.Append(AJAXTools.EncodeForXML(rcp.MilRankHTML));
                response.Append("</MilitaryRank>");

                response.Append("<ReservistsCount>");
                response.Append(rcp.ReservistsCount);
                response.Append("</ReservistsCount>");

                response.Append("<Fulfiled>");
                response.Append(rcp.Fulfiled + " (" + rcp.FulfiledReserve + ")");
                response.Append("</Fulfiled>");

                response.Append("</RequestCommandPosition>");
            }
                        
            response.Append("</RequestCommandPositions>");
            response.Append("</Data>");

            AJAX a = new AJAX(response.ToString(), Response);
            a.Write();
            Response.End();
            return;
        }

        private void JSRemoveReservists()
        {
            string stat = "";
            string response = "";
            
            try
            {
                Change change = new Change(CurrentUser, "RES_EquipmentReservistsRequests");
                List<string> IDs = Request.Form["RequestCommandPositionIDs"].Split(new string[]{","}, StringSplitOptions.RemoveEmptyEntries).ToList(); 
             
                foreach(string ID in IDs)
                {
                    int requestCommandPositionID = int.Parse(ID);

                    List<FillReservistsRequest> fillReservistRequests = FillReservistsRequestUtil.GetAllFillReservistsRequestByReqCommandPosition(requestCommandPositionID, CurrentUser);
                    List<int> deletedReservists = new List<int>();

                    //Remove all Reservists from that Position
                    foreach (FillReservistsRequest fillReservistRequest in fillReservistRequests)
                    {
                        FillReservistsRequestUtil.DeleteRequestCommandReservist(fillReservistRequest.FillReservistsRequestID, fillReservistRequest.MilitaryDepartmentID, CurrentUser, change);

                        //Just in case: clear the status and the appointment for each reservist only once (note each reservist should be added only once to a particular position)
                        if (!deletedReservists.Contains(fillReservistRequest.ReservistID))
                        {
                            //Change the current Military Reporting Status of each reservist
                            ReservistMilRepStatusUtil.SetMilRepStatusTo_FREE(fillReservistRequest.ReservistID, CurrentUser, change);

                            //Clear the current Mobilization Appointment for each Reservist
                            ReservistAppointmentUtil.ClearTheCurrentReservistAppointmentByReservist(fillReservistRequest.ReservistID, CurrentUser, change);

                            deletedReservists.Add(fillReservistRequest.ReservistID);
                        }                        
                    }
                }

                if(change.HasEvents)
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
