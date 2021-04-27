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
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class Logout : RESPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Clear the session variables
            Session["ManageReservists_Filter"] = null;

            //Sing out the currently logged user by using the Forms Authentication SignOut() method
            FormsAuthentication.SignOut();
            Response.Redirect("~/");
        }
    }
}
