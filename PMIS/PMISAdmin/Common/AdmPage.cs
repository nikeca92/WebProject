using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using PMIS.Common;

namespace PMIS.PMISAdmin.Common
{
    //Each page from the PMISAdmin module should inherit this base class
    public class AdmPage : BasePage
    {
        //The current module is ADM (PMISAdmin)
        public override string ModuleKey
        {
            get
            {
                return ModuleUtil.ADM();
            }
        }

        //This functions is used to mark which items in the menu bar shuld be highlighted
        //This is used to show the users where exactly is he/she from the list of various system screens
        protected void HighlightMenuItems(params string[] listMenuItems)
        {
            for (int i = 0; i < listMenuItems.Length; i++)
            {
                ((PMIS.PMISAdmin.MasterPages.MasterPage)Master).menuItemsHighlighted.Add(listMenuItems[i]);
            }
        }
    }
}
