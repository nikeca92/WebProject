using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using PMIS.Common;
using PMIS.PMISReports.Common;

namespace PMIS.PMISReports.ContentPages
{
    public partial class Home : REPPage
    {
        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            HighlightMenuItems("Home");
            this.SetupVisibility();
        }

        private void SetupVisibility()
        {
            if (this.GetUIItemAccessLevel("REP_ADDEDITREPORT") != UIAccessLevel.Enabled &&
                this.GetUIItemAccessLevel("REP_LISTREPORTS") == UIAccessLevel.Hidden)
            {
                this.divReports.Visible = false;
            }

            if (this.GetUIItemAccessLevel("REP_ADDEDITREPORT") != UIAccessLevel.Enabled)
            {
                this.divAddReport.Visible = false;                
            }

            if (this.GetUIItemAccessLevel("REP_LISTREPORTS") == UIAccessLevel.Hidden)
            {
                this.divReportsList.Visible = false;
            }

            if (this.GetUIItemAccessLevel("REP_SETTINGS") != UIAccessLevel.Enabled)
            {
                this.divSettings.Visible = false;
                this.divSettingsLink.Visible = false;
            }                      
        }
    }
}
