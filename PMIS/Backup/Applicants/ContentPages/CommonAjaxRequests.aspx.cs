using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using PMIS.Common;
using PMIS.Applicants.Common;
using System.Text;

namespace PMIS.Applicants.ContentPages
{
    public partial class CommonAjaxRequests : APPLPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "IsValidIdentityNumber")
            {
                string identityNumber = "";

                if (Request.Params["identityNumber"] != null)
                {
                    identityNumber = Request.Params["identityNumber"].ToString();
                }

                IsValidIdentityNumber(identityNumber );
                return;
            }

        }

        void IsValidIdentityNumber(string  identityNumber)
        {
            string response = "";
            if (PersonUtil.IsValidIdentityNumber(identityNumber, CurrentUser))
            {
                response = "OK";
            }
            else
            {
                response = "NO";
            }

            AJAX a = new AJAX(AJAXTools.EncodeForXML(response), this.Response);
            a.Write();
            Response.End();
        }
    }
}
