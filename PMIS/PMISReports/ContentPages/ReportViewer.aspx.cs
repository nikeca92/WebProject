using System;
using PMIS.PMISReports.Common;
using PMIS.Common;

namespace IzendaAdHocStarterKit
{
    public partial class ReportViewer : REPPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.GetUIItemAccessLevel("REP_LISTREPORTS") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }
        }
    }
}
