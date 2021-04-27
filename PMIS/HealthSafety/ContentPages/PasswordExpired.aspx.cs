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
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Globalization;

namespace PMIS.HealthSafety.ContentPages
{
    /*THIS PAGE IS USED IN ALL MODULES. SO, IF THERE ARE ANY CHANGE IN THIS PAGE THEN PROBABLY THE SAME CHANGES SHULD BE
      APPLIED IN THE OTHER MODULES (PROJECTS) TOO*/
    public partial class PasswordExpired : HSPage
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
            this.lblMessage.Text = "";

            string passwordReq = PasswordPolicyUtil.StrongPasswordRequirements(CurrentUser);

            if (passwordReq != "")
            {
                imgPasswordReq.Attributes.Add("title", passwordReq);
                imgPasswordReq.Visible = true;
            }
            else
            {
                imgPasswordReq.Visible = false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                UserUtil.ChangePassword(txtPassword.Text, CurrentUser.UserId, CurrentUser);
                Response.Redirect("~");
            }
        }

        private bool ValidateData()
        {
            bool isDataValid = true;
            string errMsg = "";
            List<string> errRightsFields = new List<string>();

            if (txtPassword.Text == "")
            {
                isDataValid = false;

                if (pageDisabledControls.Contains(txtPassword) || pageHiddenControls.Contains(txtPassword))
                    errRightsFields.Add("Парола");
                else
                    errMsg += "Паролата е празна<br/>";
            }
            else if (txtPassword.Text != txtPassword2.Text)
            {
                isDataValid = false;
                errMsg += "Паролата не е повторена правилно<br/>";
            }
            else if (!PasswordPolicyUtil.IsStrongPassword(txtPassword.Text, CurrentUser))
            {
                isDataValid = false;
                errMsg += "Паролата не отговаря на изискванията<br/>";
            }
            else
            {
                PasswordPolicy passwordPolicy = PasswordPolicyUtil.GetPasswordPolicy(CurrentUser);

                if (passwordPolicy.CannotReusePrevPasswords.HasValue && passwordPolicy.CannotReusePrevPasswords.Value > 0 &&
                    OldPasswordUtil.IsInLastNPasswords(txtPassword.Text, CurrentUser.UserId, passwordPolicy.CannotReusePrevPasswords.Value, CurrentUser))
                {
                    isDataValid = false;
                    errMsg += "Паролата вече е била използвана<br/>";
                }
            }

            if (errRightsFields.Count > 0)
            {
                errMsg = "<i>" + CommonFunctions.GetErrorMessageNoRights(errRightsFields.ToArray()) + "</i><br />" + errMsg;
            }

            if (!isDataValid)
            {
                this.lblMessage.CssClass = "ErrorText";
                this.lblMessage.Text = errMsg;
            }

            return isDataValid;
        }
    }
}
