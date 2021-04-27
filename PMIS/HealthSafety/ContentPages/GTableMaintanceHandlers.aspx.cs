﻿using System;
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

using PMIS.Common;
using PMIS.HealthSafety.Common;

namespace HealthSafety.ContentPages
{
    public partial class GTableMaintanceHandlers : GTablePage
    {
        //The current module is HS (HealthSafety)
        public override string ModuleKey
        {
            get { return ModuleUtil.HS(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["AjaxMethod"] == null)
            {
                RedirectAccessDenied();
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetGTableItems")
            {
                JSGetGTableItems(false, false, false);
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteGTableItem")
            {
                JSDeleteGTableItem();
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveGTableItem")
            {
                JSSaveGTableItem();
            }
        }
    }
}
