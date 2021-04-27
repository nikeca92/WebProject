using System;
using PMIS.Common;
using PMIS.PMISReports.Common;

namespace IzendaAdHocStarterKit
{
    public partial class ReportList : REPPage
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
            if (this.GetUIItemAccessLevel("REP_LISTREPORTS") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            //Highlight the current page in the menu bar
            HighlightMenuItems("Reports", "Reports_List");
		}

		override protected void OnInit(EventArgs e)
		{
			InitializeComponent();
			base.OnInit(e);
		}
		
		private void InitializeComponent()
		{
		}
	}
}
