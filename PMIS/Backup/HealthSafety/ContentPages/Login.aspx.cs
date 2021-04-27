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
using PMIS.HealthSafety.Common;

namespace PMIS.HealthSafety.ContentPages
{
    public partial class Login : HSPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentUser.UserId != 0)
                Response.Redirect("~/ContentPages/Home.aspx");

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
            int userId = PMIS.Common.UserUtil.GetValidUserLogin(username, password, ModuleKey);

            //If the username and the password are valid then redirect the user from the Login page to the specific screen
            //where the user tried to go
            if (userId != 0)
            {
                User user = UserUtil.GetUser(userId, ModuleKey);
                Module module = ModuleUtil.GetModule(user, ModuleKey);

                LoginLog loginLog = LoginLogUtil.CreateInitialLoginLog(user, module);

                FormsAuthentication.RedirectFromLoginPage(loginLog.LoginLogID.ToString(), false);

                LoginLogUtil.UpdateLoginLog(user, module, Request, Session, loginLog);
            }
            else
            {
                InvalidLogin("Невалидни потребителско име / парола за достъп");
            }
        }

        private void InvalidLogin(string message)
        {
            Module module = ModuleUtil.GetModule(ModuleKey);
            FailedLoginResult result = FailedLoginUtil.WriteFailedLogin(txtUsername.Text, module, Request, Session);

            if (result == FailedLoginResult.BlockedUser)
                message = "Потребителят е блокиран";

            lblMessage.Text = message;
        }
    }
}
