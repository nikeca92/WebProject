using System;
using System.Collections.Generic;
using System.Linq;

using PMIS.Common;
using PMIS.Reserve.Common;
using System.Web;
using System.Text;

namespace PMIS.Reserve.ContentPages
{
    public partial class AddEditTechnics_OtherInfo : RESPage
    {
        private string technicsTypeKey = null;
        public string TechnicsTypeKey
        {
            get
            {
                if (technicsTypeKey == null)
                {
                    int technicsId = int.Parse(Request.Params["TechnicsId"]);
                    Technics technics = TechnicsUtil.GetTechnics(technicsId, CurrentUser);

                    technicsTypeKey = technics.TechnicsType.TypeKey;
                }

                return technicsTypeKey;
            }
        }

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
            if (GetUIItemAccessLevel("RES_TECHNICS") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey) != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("RES_TECHNICS_" + TechnicsTypeKey + "_EDIT") != UIAccessLevel.Enabled )
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int technicsId = 0;
                if (!String.IsNullOrEmpty(Request.Params["TechnicsId"]))
                {
                    int.TryParse(Request.Params["TechnicsId"], out technicsId);
                }

                string technicsTypeKey = "";
                if (!String.IsNullOrEmpty(Request.Params["TechnicsTypeKey"]))
                {
                    technicsTypeKey = Request.Params["TechnicsTypeKey"].ToString();
                }

                string otherInfo = "";
                if (!String.IsNullOrEmpty(Request.Params["OtherInfo"]))
                {
                    otherInfo = Request.Params["OtherInfo"].ToString();
                }

                if (technicsId == 0)
                {
                    throw new Exception("Техниката не е намерена");
                }
                else
                {
                    TechnicsType technicsType = TechnicsTypeUtil.GetTechnicsType(technicsTypeKey, CurrentUser);

                    //Track the changes into the Audit Trail 
                    Change change = new Change(CurrentUser, "RES_Technics_" + technicsType.TypeKey);

                    TechnicsUtil.SaveTechnics_OtherInfo(technicsId, otherInfo, CurrentUser, change);

                    change.WriteLog();

                    stat = AJAXTools.OK;
                    response = "<response>Информацията посочена в полето за други данни е обновена</response>";
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

    public static class AddEditTechnics_OtherInfo_PageUtil
    {
        public static string GetTabContent(AddEditTechnics page)
        {
            Technics technics = TechnicsUtil.GetTechnics(page.TechnicsId, page.CurrentUser);

            string html = @"
<div style=""height: 10px;""></div>

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
            <textarea id=""txtOtherInfo"" rows='25' class='InputField' style='width: 98%;'>" + technics.OtherInfo + @"</textarea>
         </td>
      </tr>
   </table>
</fieldset>

<div style=""height: 10px;""></div>

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
                             page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT") == UIAccessLevel.Disabled || isPreview;                    

            UIAccessLevel l;

            //section PersonAdmClAccessAndMilRepSpec

            l = page.GetUIItemAccessLevel("RES_TECHNICS_" + page.TechnicsTypeKey + "_EDIT_OTHERINFO");

            if (l == UIAccessLevel.Disabled || screenDisabled)
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
