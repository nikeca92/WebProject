using System;
using PMIS.PMISReports.Common;
using PMIS.Common;

namespace IzendaAdHocStarterKit
{
    public partial class Settings : REPPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.GetUIItemAccessLevel("REP_SETTINGS") != UIAccessLevel.Enabled)
            {
                RedirectAccessDenied();
            }

            //Highlight the current page in the menu bar
            HighlightMenuItems("Settings");
        }
    }
}
