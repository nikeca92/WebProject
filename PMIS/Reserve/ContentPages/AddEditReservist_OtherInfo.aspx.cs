using System;

using PMIS.Common;
using PMIS.Reserve.Common;
using System.Collections.Generic;

namespace PMIS.Reserve.ContentPages
{
    public partial class AddEditReservist_OtherInfo : RESPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveOtherInfo")
            {
                this.JSSaveOtherInfo();
                return;
            }
            
        }

        private void JSSaveOtherInfo()
        {
            if (GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_OTHERDATA") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int reservistId = 0;
                if (!String.IsNullOrEmpty(Request.Params["ReservistId"]))
                {
                    int.TryParse(Request.Params["ReservistId"], out reservistId);
                }

                string otherInfo = "";
                if (!String.IsNullOrEmpty(Request.Params["OtherInfo"]))
                {
                    otherInfo = Request.Params["OtherInfo"].ToString();
                }

                if (reservistId == 0)
                {
                    throw new Exception("Резервистът не е намерен");
                }
                else
                {
                    stat = AJAXTools.OK;

                    //Track the changes into the Audit Trail 
                    Change change = new Change(CurrentUser, "RES_Reservists");

                    Reservist reservist = ReservistUtil.GetReservist(reservistId, CurrentUser);

                    bool isEmpty = false;
                    if (String.IsNullOrEmpty(reservist.Person.OtherInfo))
                    {
                        isEmpty = true;
                    }

                    reservist.Person.OtherInfo = otherInfo;

                    bool isSaved = PersonUtil.SavePerson_WhenAddingEditingReservist(reservist.Person, "ADM_PersonDetails_Edit", CurrentUser, change);

                    if (isSaved)
                    {
                        if (isEmpty)
                        {
                            response = "<response>Информацията посочена в полето за други данни е добавена</response>";    
                        }
                        else
                        {
                            response = "<response>Информацията посочена в полето за други данни е обновена</response>";
                        }

                        change.WriteLog();
                    }
                    else
                    {
                        response = "<response>Информацията посочена в полето за други данни не е записана</response>";
                        stat = AJAXTools.ERROR;
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
    }

    public static class AddEditReservist_OtherInfo_PageUtil
    {
        public static string GetTabContent(int reservistId, User currentUser)
        {
            Reservist reservist = ReservistUtil.GetReservist(reservistId, currentUser);

            string html = @"
                <fieldset style=""width: 830px; padding: 0px;"">
                   <table class=""InputRegion"" style=""width: 830px; padding: 10px; padding-top: 0px; margin-top: 0px;"">
                      <tr style=""height: 3px;"">
                      </tr>
                      <tr>
                         <td style=""text-align: left;"">
                            <span id=""lblOtherInfo"" style=""color: #0B4489; font-weight: bold; font-size: 1.1em; width: 100%; Position: relative; top: -5px;"">Други данни</span>
                         </td>
                      </tr>
                      <tr>
                         <td style=""text-align: right;"">
                            <textarea id=""txtOtherInfo"" rows='25' class='InputField' style='width: 98%;'>" + (reservist.Person.OtherInfo != null ? reservist.Person.OtherInfo : "") + @"</textarea>
                         </td>
                      </tr>
                   </table>
                </fieldset>

                <div style=""height: 10px;""></div>

";

            return html;
        }

        public static string GetTabUIItems(AddEditReservist page)
        {
            bool isPreview = false;
            if (!String.IsNullOrEmpty(page.Request.Params["Preview"]))
            {
                isPreview = true;
            }

            string UIItemsXML = "";

            bool screenDisabled = false;
            bool otherInfoDisabled = false;

            List<string> disabledClientControls = new List<string>();
            List<string> hiddenClientControls = new List<string>();

            screenDisabled = page.GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Disabled ||
                                             page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Disabled || isPreview;

            otherInfoDisabled = page.GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Disabled ||
                                   page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Disabled ||
                                   page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_OTHERDATA") == UIAccessLevel.Disabled;

            if (screenDisabled)
            {
                hiddenClientControls.Add("btnSaveOtherInfo");
            }

            if (otherInfoDisabled)
            {
                hiddenClientControls.Add("btnSaveOtherInfo");
            }

            UIAccessLevel l;

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_OTHERDATA");

            l = page.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_OTHERDATA");

            if (l == UIAccessLevel.Disabled || screenDisabled || otherInfoDisabled)
            {
                disabledClientControls.Add("lblOtherInfo");
                disabledClientControls.Add("txtOtherInfo");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblOtherInfo");
                hiddenClientControls.Add("txtOtherInfo");
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
    }
}
