using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PMIS.Reserve.Common;
using PMIS.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class CivilEducationSelectorHandlers : RESPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSearchCivilEducation")
            {
                JSSearchCivilEducation();
            }
        }

        private void JSSearchCivilEducation()
        {
            string searchType = Request.Params["SearchType"] != null ? Request.Params["SearchType"] : "";
            string searchText = Request.Params["SearchText"] != null ? Request.Params["SearchText"] : "";
            string response = PersonCivilEducationUtil.CivilEducationSelector_SearchCivilEducation(searchType, searchText, 50, CurrentUser);

            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();
            return;

        }
    }
}
