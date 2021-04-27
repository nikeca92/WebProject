using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class DischargeReasonSelectorHandlers : RESPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSearchDischargeReasons")
            {
                JSSearchDischargeReasons();
            }
        }

        private void JSSearchDischargeReasons()
        {
            string searchType = Request.Params["SearchType"] != null ? Request.Params["SearchType"] : "";
            string searchText = Request.Params["SearchText"] != null ? Request.Params["SearchText"] : "";
            string response = DischargeReasonUtil.DischargeReasonSelector_SearchDischargeReasons(searchType, searchText, 50, CurrentUser);

            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();
            return;
            
        }
    }
}
