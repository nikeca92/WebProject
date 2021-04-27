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
using PMIS.PMISReports.Common;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Globalization;

namespace PMIS.PMISReports.ContentPages
{
    /*THIS PAGE IS USED IN ALL MODULES. SO, IF THERE ARE ANY CHANGE IN THIS PAGE THEN PROBABLY THE SAME CHANGES SHULD BE
      APPLIED IN THE OTHER MODULES (PROJECTS) TOO*/
    public partial class EditUserProfile : REPPage
    {
        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "REP_EDITPROFILE";
            }
        }

        //This property represents the ID of the User object that is loaded on the screen
        //If this is a new role then the ID is 0
        //It is stored in a hidden field on the page
        private int UserId
        {
            get
            {
                int userId = 0;
                if (String.IsNullOrEmpty(this.hfUserID.Value)
                    || this.hfUserID.Value == "0")
                {
                    this.hfUserID.Value = CurrentUser.UserId.ToString();
                    userId = CurrentUser.UserId;
                }
                else
                {
                    Int32.TryParse(this.hfUserID.Value, out userId);
                }

                return userId;
            }

            set
            {
                this.hfUserID.Value = value.ToString(); 
            }
        }

        //This property is used to find out if the Set Password section is expanded
        private bool IsPasswordExpanded
        {
            get
            {
                return hfIsPasswordExpanded.Value == "1";
            }

            set
            {
                hfIsPasswordExpanded.Value = value ? "1" : "0";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Highlight the current page in the menu bar
            HighlightMenuItems("UserProfile");

            //Load the page for the first time: Load the data on the screen
            if (!IsPostBack)
            {
                LoadData();
                SetPageHeader();

                //Set the passwords rows visibility
                SetPasswordsVisibility();

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

            SetupPageUI();
        }

        //Set the correct page header (i.e. either New or Edit)
        private void SetPageHeader()
        {
            string header = "Редактиране на профил";

            lblHeaderTitle.InnerHtml = header;
            Page.Title = header;
        }

        //Load the existing data in Edit mode
        private void LoadData()
        {
            User user = UserUtil.GetUser(CurrentUser, UserId);

            txtUsername.Text = user.Username;
            txtFirstName.Text = user.FirstName;
            txtMiddleName.Text = user.MiddleName;
            txtLastName.Text = user.LastName;
            txtEmail.Text = user.Email;
            txtPhone.Text = user.Phone;

            //Iterate through all assigned roles and set the approperiate Roles to the Module specific drop-downs
            foreach (UserRole role in user.AllRoles)
            {
                if (role.Module.ModuleKey == ModuleUtil.ADM())
                {
                    lblADM_Role.Text = role.RoleName;
                }

                if (role.Module.ModuleKey == ModuleUtil.HS())
                {
                    lblHS_Role.Text = role.RoleName;
                }

                if (role.Module.ModuleKey == ModuleUtil.APPL())
                {
                    lblAPPL_Role.Text = role.RoleName;
                }

                if (role.Module.ModuleKey == ModuleUtil.RES())
                {
                    lblRES_Role.Text = role.RoleName;
                }

                if (role.Module.ModuleKey == ModuleUtil.REP())
                {
                    lblREP_Role.Text = role.RoleName;
                }
            }
        }

        //Collect the information from the page form
        //Store the data in a object in the memory and use that object when peforming DB operations
        private User CollectData()
        {
            User user = new User(CurrentUser);

            user.UserId = UserId;
            user.Username = txtUsername.Text;
            user.Password = txtPassword.Text;
            user.FirstName = txtFirstName.Text;
            user.MiddleName = txtMiddleName.Text;
            user.LastName = txtLastName.Text;
            user.Email = txtEmail.Text;
            user.Phone = txtPhone.Text;

            return user;
        }

        //Validate the form data before doing any server actions
        private bool ValidateData()
        {
            bool isDataValid = true;
            string errMsg = "";
            List<string> errRightsFields = new List<string>();

            if (txtUsername.Text.Trim() == "")
            {
                isDataValid = false;

                if (pageDisabledControls.Contains(txtUsername) || pageHiddenControls.Contains(txtUsername))
                    errRightsFields.Add("Потребителско име");
                else
                    errMsg += CommonFunctions.GetErrorMessageMandatory("Потребителско име") + "<br/>";
            }
            else if (UserUtil.IsExistingUsername(CurrentUser, txtUsername.Text, UserId))
            {
                isDataValid = false;
                errMsg += "Потребителското име е заето<br/>";
            }

            //Validate the password only when the Set Password section is expanded
            if (IsPasswordExpanded)
            {
                if (!UserUtil.IsValidPassword(CurrentUser, txtOldPassword.Text, UserId))
                {
                    isDataValid = false;
                    errMsg += "Старата парола е невалидна<br/>";
                }
                else if (txtPassword.Text == "")
                {
                    isDataValid = false;
                    errMsg += CommonFunctions.GetErrorMessageMandatory("Нова парола") + "<br/>";
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
                        OldPasswordUtil.IsInLastNPasswords(txtPassword.Text, UserId, passwordPolicy.CannotReusePrevPasswords.Value, CurrentUser))
                    {
                        isDataValid = false;
                        errMsg += "Паролата вече е била използвана от този потребител<br/>";
                    }
                }
            }

            if (txtFirstName.Text.Trim() == "")
            {
                isDataValid = false;

                if (pageDisabledControls.Contains(txtFirstName) || pageHiddenControls.Contains(txtFirstName))
                    errRightsFields.Add("Име");
                else
                    errMsg += CommonFunctions.GetErrorMessageMandatory("Име") + "<br/>";
            }

            if (txtLastName.Text.Trim() == "")
            {
                isDataValid = false;

                if (pageDisabledControls.Contains(txtLastName) || pageHiddenControls.Contains(txtLastName))
                    errRightsFields.Add("Фамилия");
                else
                    errMsg += CommonFunctions.GetErrorMessageMandatory("Фамилия") + "<br/>";
            }

            if (!isDataValid)
            {
                this.lblMessage.CssClass = "ErrorText";
                this.lblMessage.Text = errMsg;
            }

            return isDataValid;
        }

        //Save the data
        private void SaveData()
        {
            //First collect the data from the page form
            User user = CollectData();

            //Track the changes into the Audit Trail 
            Change change = new Change(CurrentUser, ModuleKey + "_UserProfile");

            //If a successfull save operation then indicate this on the screen; Otherwise alert a warning
            if (UserUtil.SaveUserProfile(CurrentUser, user, change, ModuleKey + "_EditUserProfile"))
            {
                UserId = user.UserId;

                this.lblMessage.CssClass = "SuccessText";
                this.lblMessage.Text = "Записът е успешен";

                SetPageHeader();

                IsPasswordExpanded = false;
                SetPasswordsVisibility();

                lnkChangePassword.Visible = true;

                hdnSavedChanges.Value = "True";
            }
            else
            {
                this.lblMessage.CssClass = "ErrorText";
                this.lblMessage.Text = "Записът не е успешен";
            }

            //Finally write any changes to the log
            change.WriteLog();
        }

        //Save the form data (first check if it is valid)
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                SaveData();
            }
        }

        //Set the password rows visibility according to the status of the sections (expanded or not)
        private void SetPasswordsVisibility()
        {
            if (IsPasswordExpanded)
            {
                rowOldPassword.Visible = true;
                rowPassword.Visible = true;
                rowPassword2.Visible = true;

                lnkChangePassword.Text = "Отказ от смяна на парола [x]";
            }
            else
            {
                rowOldPassword.Visible = false;
                rowPassword.Visible = false;
                rowPassword2.Visible = false;

                txtPassword.Text = "";
                txtPassword2.Text = "";

                lnkChangePassword.Text = "Смяна на парола";
            }
        }

        //Expand or collapse the password section
        protected void lnkChangePassword_Click(object sender, EventArgs e)
        {
            IsPasswordExpanded = !IsPasswordExpanded;
            SetPasswordsVisibility();
        }

        // Setup user interface elements according to rights of the user
        private void SetupPageUI()
        {
            bool screenHidden = GetUIItemAccessLevel("REP_EDITPROFILE") == UIAccessLevel.Hidden;

            bool screenDisabled = GetUIItemAccessLevel("REP_EDITPROFILE") == UIAccessLevel.Disabled;

            if (screenHidden)
                RedirectAccessDenied();

            if (screenDisabled)
            {
                pageDisabledControls.Add(btnSave);
            }

            UIAccessLevel l;

            l = GetUIItemAccessLevel("REP_EDITPROFILE_USERNAME");
            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                pageDisabledControls.Add(lblUsername);
                pageDisabledControls.Add(txtUsername);
            }
            if (l == UIAccessLevel.Hidden)
            {
                pageHiddenControls.Add(lblUsername);
                pageHiddenControls.Add(txtUsername);
            }

            l = this.GetUIItemAccessLevel("REP_EDITPROFILE_PASSWORD");
            if (l == UIAccessLevel.Disabled || screenDisabled || l == UIAccessLevel.Hidden)
            {
                pageHiddenControls.Add(lnkChangePassword);
            }

            l = GetUIItemAccessLevel("REP_EDITPROFILE_FIRSTNAME");
            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                pageDisabledControls.Add(lblFirstName);
                pageDisabledControls.Add(txtFirstName);
            }
            if (l == UIAccessLevel.Hidden)
            {
                pageHiddenControls.Add(lblFirstName);
                pageHiddenControls.Add(txtFirstName);
            }

            l = GetUIItemAccessLevel("REP_EDITPROFILE_MIDDLENAME");
            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                pageDisabledControls.Add(lblMiddleName);
                pageDisabledControls.Add(txtMiddleName);
            }
            if (l == UIAccessLevel.Hidden)
            {
                pageHiddenControls.Add(lblMiddleName);
                pageHiddenControls.Add(txtMiddleName);
            }

            l = GetUIItemAccessLevel("REP_EDITPROFILE_LASTNAME");
            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                pageDisabledControls.Add(lblLastName);
                pageDisabledControls.Add(txtLastName);
            }
            if (l == UIAccessLevel.Hidden)
            {
                pageHiddenControls.Add(lblLastName);
                pageHiddenControls.Add(txtLastName);
            }

            l = GetUIItemAccessLevel("REP_EDITPROFILE_EMAIL");
            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                pageDisabledControls.Add(lblEmail);
                pageDisabledControls.Add(txtEmail);
            }
            if (l == UIAccessLevel.Hidden)
            {
                pageHiddenControls.Add(lblEmail);
                pageHiddenControls.Add(txtEmail);
            }

            l = GetUIItemAccessLevel("REP_EDITPROFILE_PHONE");
            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                pageDisabledControls.Add(lblPhone);
                pageDisabledControls.Add(txtPhone);
            }
            if (l == UIAccessLevel.Hidden)
            {
                pageHiddenControls.Add(lblPhone);
                pageHiddenControls.Add(txtPhone);
            }
        }
    }
}
