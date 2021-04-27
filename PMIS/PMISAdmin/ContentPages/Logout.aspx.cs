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
using PMIS.PMISAdmin.Common;

namespace PMIS.PMISAdmin.ContentPages
{
    public partial class Logout : AdmPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Sing out the currently logged user by using the Forms Authentication SignOut() method
            FormsAuthentication.SignOut();
            Response.Redirect("~/");
        }
    }
}
