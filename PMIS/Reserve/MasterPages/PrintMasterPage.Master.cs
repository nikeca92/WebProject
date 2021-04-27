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

namespace PMIS.Reserve.MasterPages
{
    public partial class PrintMasterPage : System.Web.UI.MasterPage
    {
        public string PrintTitleLinesWidth
        {
            get
            {
                return this.hdnPrintTitleLinesWidth.Value;
            }
        }

        public string HeadersLeft
        {
            get
            {
                if (this.hdnHeadersLeft.Value == "")
                {
                    return "35";
                }

                return this.hdnHeadersLeft.Value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
