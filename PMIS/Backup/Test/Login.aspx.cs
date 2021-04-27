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

namespace PMIS.Test
{
    public partial class Login : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            string onkeydown = "return FormKeyDown(event, '" + btnLogin.ClientID + "');";

            txtUsername.Attributes.Add("onkeydown", onkeydown);
            txtPassword.Attributes.Add("onkeydown", onkeydown);

            lblMessage.Text = "";
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = "";
            string password = "";

            username = txtUsername.Text.Trim();
            password = txtPassword.Text.Trim();

            //This returns information about if the provided credentials are valid for the HS module
            int userId = PMIS.Common.UserUtil.GetValidUserLogin(username, password, ModuleUtil.HS());

            //If the username and the password are valid then redirect the user from the Login page to the specific screen
            //where the user tried to go
            if (userId != 0)
            {
                FormsAuthentication.RedirectFromLoginPage(userId.ToString(), false);
            }
            else
            {
                InvalidLogin("Невалидни потребителско име / парола за достъп");
            }
        }

        private void InvalidLogin(string message)
        {
            lblMessage.Text = message;
        }
    }
}
