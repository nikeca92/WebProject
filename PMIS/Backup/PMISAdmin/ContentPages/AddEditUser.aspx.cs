using System;
using System.Linq;
using System.Web.UI.WebControls;

using PMIS.Common;
using PMIS.PMISAdmin.Common;
using System.Collections.Generic;

namespace PMIS.PMISAdmin.ContentPages
{
    public partial class AddEditUser : AdmPage
    {
        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "ADM_SECURITY_USERS";
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
                    if (Request.Params["UserId"] != null)
                        int.TryParse(Request.Params["UserId"].ToString(), out userId);

                    this.hfUserID.Value = userId.ToString();
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

        //This is a flag field that says if the screen is opened from the Home screen
        //This is used to navigate the user back to the home screen when using the Back button
        private int FromHome
        {
            get
            {
                int fh = 0;
                if (String.IsNullOrEmpty(this.hfFromHome.Value)
                    || this.hfFromHome.Value == "0")
                {
                    if (Request.Params["fh"] != null)
                        int.TryParse(Request.Params["fh"].ToString(), out fh);

                    this.hfFromHome.Value = hfFromHome.ToString();
                }
                else
                {
                    Int32.TryParse(this.hfFromHome.Value, out fh);
                }

                return fh;
            }

            set
            {
                this.hfFromHome.Value = value.ToString();
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

        //This property is used to find out if the Set Oracle Password section is expanded
        private bool IsOraclePasswordExpanded
        {
            get
            {
                return hfIsOraclePasswordExpanded.Value == "1";
            }

            set
            {
                hfIsOraclePasswordExpanded.Value = value ? "1" : "0";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Hilight the correct item in the menu
            if (UserId == 0)
                HighlightMenuItems("Users", "Users_AddUser");
            else
                HighlightMenuItems("Users");

            //Hide the navigation buttons
            HideNavigationControls(btnBack);

            //Load the page for the first time: Load the data on the screen
            if (!IsPostBack)
            {
                LoadDropDowns();
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
            string header = (UserId > 0 ? "Редактиране на потребител" : "Въвеждане на нов потребител");

            lblHeaderTitle.InnerHtml = header;
            Page.Title = header;
        }

        //Load any drop-down on the screen
        private void LoadDropDowns()
        {
            PopulateRoles(ddADM_Role, ModuleUtil.ADM());
            PopulateRoles(ddHS_Role, ModuleUtil.HS());
            PopulateRoles(ddAPPL_Role, ModuleUtil.APPL());
            PopulateRoles(ddRES_Role, ModuleUtil.RES());
            PopulateRoles(ddREP_Role, ModuleUtil.REP());
        }

        //Populate roles drop-down
        private void PopulateRoles(DropDownList dd, string moduleKey)
        {
            dd.Items.Clear();
            dd.Items.Add(ListItems.GetOptionChooseOne());

            Module module = ModuleUtil.GetModule(CurrentUser, moduleKey);

            List<UserRole> roles = UserRoleUtil.GetUserRoles(CurrentUser, "", module.ModuleId.ToString(), 1, 0, 0);

            foreach (UserRole role in roles)
            {
                ListItem li = new ListItem();
                li.Text = role.RoleName;
                li.Value = role.RoleId.ToString();

                dd.Items.Add(li);
            }
        }

        //Load the existing data in Edit mode
        private void LoadData()
        {
            //If Edit mode
            if (UserId > 0)
            {
                User user = UserUtil.GetUser(CurrentUser, UserId);

                txtUsername.Text = user.Username;
                txtOracleUsername.Text = user.OracleUsername;
                txtFirstName.Text = user.FirstName;
                txtMiddleName.Text = user.MiddleName;
                txtLastName.Text = user.LastName;
                txtEmail.Text = user.Email;
                txtPhone.Text = user.Phone;
                chkActive.Checked = user.IsActive;
                chkBlocked.Checked = user.IsBlocked;
                chkPasswordDoesNotExpire.Checked = user.PasswordDoesNotExpire;

                //Iterate through all assigned roles and set the approperiate Roles to the Module specific drop-downs
                foreach (UserRole role in user.AllRoles)
                {
                    if (role.Module.ModuleKey == ModuleUtil.ADM())
                    {
                        ddADM_Role.SelectedValue = role.RoleId.ToString();
                    }

                    if (role.Module.ModuleKey == ModuleUtil.HS())
                    {
                        ddHS_Role.SelectedValue = role.RoleId.ToString();
                    }

                    if (role.Module.ModuleKey == ModuleUtil.APPL())
                    {
                        ddAPPL_Role.SelectedValue = role.RoleId.ToString();
                    }

                    if (role.Module.ModuleKey == ModuleUtil.RES())
                    {
                        ddRES_Role.SelectedValue = role.RoleId.ToString();
                    }

                    if (role.Module.ModuleKey == ModuleUtil.REP())
                    {
                        ddREP_Role.SelectedValue = role.RoleId.ToString();
                    }
                }

                //If the Current User edits him own details then do not allow to broke the system and to not be albe to login again
                if (CurrentUser.UserId == user.UserId)
                {
                    chkActive.Enabled = false;
                    chkBlocked.Enabled = false;
                    lnkSetOraclePassword.Visible = false;
                    txtOracleUsername.Enabled = false;
                    ddADM_Role.Enabled = false;
                    //ddHS_Role.Enabled = false;
                    //ddAPPL_Role.Enabled = false;
                    //ddRES_Role.Enabled = false;
                    //ddREP_Role.Enabled = false;
                }
            }
            else //New record mode
            {
                IsPasswordExpanded = true;
                IsOraclePasswordExpanded = true;
                SetPasswordsVisibility();

                lnkSetPassword.Visible = false;
                lnkSetOraclePassword.Visible = false;
                chkActive.Checked = true;
                chkBlocked.Checked = false;
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
            user.OracleUsername = txtOracleUsername.Text;
            user.OraclePassword = txtOraclePassword.Text;
            user.IsActive = chkActive.Checked;
            user.IsBlocked = chkBlocked.Checked;
            user.PasswordDoesNotExpire = chkPasswordDoesNotExpire.Checked;
            user.FirstName = txtFirstName.Text;
            user.MiddleName = txtMiddleName.Text;
            user.LastName = txtLastName.Text;
            user.Email = txtEmail.Text;
            user.Phone = txtPhone.Text;

            user.AllRoles = new List<UserRole>();

            UserRole role = null;

            //Check all selected roles and add them to the list
            if (ddADM_Role.SelectedValue != ListItems.GetOptionChooseOne().Value)
            {
                role = UserRoleUtil.GetUserRole(CurrentUser, int.Parse(ddADM_Role.SelectedValue));
                user.AllRoles.Add(role);
            }

            if (ddHS_Role.SelectedValue != ListItems.GetOptionChooseOne().Value)
            {
                role = UserRoleUtil.GetUserRole(CurrentUser, int.Parse(ddHS_Role.SelectedValue));
                user.AllRoles.Add(role);
            }

            if (ddAPPL_Role.SelectedValue != ListItems.GetOptionChooseOne().Value)
            {
                role = UserRoleUtil.GetUserRole(CurrentUser, int.Parse(ddAPPL_Role.SelectedValue));
                user.AllRoles.Add(role);
            }

            if (ddRES_Role.SelectedValue != ListItems.GetOptionChooseOne().Value)
            {
                role = UserRoleUtil.GetUserRole(CurrentUser, int.Parse(ddRES_Role.SelectedValue));
                user.AllRoles.Add(role);
            }

            if (ddREP_Role.SelectedValue != ListItems.GetOptionChooseOne().Value)
            {
                role = UserRoleUtil.GetUserRole(CurrentUser, int.Parse(ddREP_Role.SelectedValue));
                user.AllRoles.Add(role);
            }

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
                    errMsg += "Потребителското име е празно<br/>";
            }
            else if (UserUtil.IsExistingUsername(CurrentUser, txtUsername.Text, UserId))
            {
                isDataValid = false;
                errMsg += "Потребителското име е заето<br/>";
            }

            //Validate the password only when the Set Password section is expanded
            if (IsPasswordExpanded)
            {
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
                        OldPasswordUtil.IsInLastNPasswords(txtPassword.Text, UserId, passwordPolicy.CannotReusePrevPasswords.Value, CurrentUser))
                    {
                        isDataValid = false;
                        errMsg += "Паролата вече е била използвана от този потребител<br/>";
                    }
                }
            }

            if (txtOracleUsername.Text.Trim() == "")
            {
                isDataValid = false;

                if (pageDisabledControls.Contains(txtOracleUsername) || pageHiddenControls.Contains(txtOracleUsername))
                    errRightsFields.Add("Oracle потребителско име");
                else
                    errMsg += "Oracle потребителското име е празно<br/>";
            }

            //Validate the Oracle password only when the Set Password section is expanded
            if (IsOraclePasswordExpanded)
            {
                if (txtOraclePassword.Text == "")
                {
                    isDataValid = false;

                    if (pageDisabledControls.Contains(txtOraclePassword) || pageHiddenControls.Contains(txtOraclePassword))
                        errRightsFields.Add("Oracle парола");
                    else
                        errMsg += "Oracle паролата е празна<br/>";
                }
                else if (txtOraclePassword.Text != txtOraclePassword2.Text)
                {
                    isDataValid = false;
                    errMsg += "Oracle паролата не е повторена правилно<br/>";
                }
            }

            if (txtFirstName.Text.Trim() == "")
            {
                isDataValid = false;

                if (pageDisabledControls.Contains(txtFirstName) || pageHiddenControls.Contains(txtFirstName))
                    errRightsFields.Add("Име");
                else
                    errMsg += "Името е празно<br/>";
            }

            if (txtLastName.Text.Trim() == "")
            {
                isDataValid = false;

                if (pageDisabledControls.Contains(txtLastName) || pageHiddenControls.Contains(txtLastName))
                    errRightsFields.Add("Фамилия");
                else
                    errMsg += "Фамилията е празна<br/>";
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

        //Save the data
        private void SaveData()
        {
            //First collect the data from the page form
            User user = CollectData();

            //Track the changes into the Audit Trail 
            Change change = new Change(CurrentUser, "ADM_Users");

            //If a successfull save operation then indicate this on the screen; Otherwise alert a warning
            if (UserUtil.SaveUser(CurrentUser, user, change))
            {
                if (UserId == 0)
                {
                    SetLocationHash("AddEditUser.aspx?UserId=" + user.UserId.ToString());
                }

                UserId = user.UserId;

                this.lblMessage.CssClass = "SuccessText";
                this.lblMessage.Text = "Записът е успешен";

                hdnSavedChanges.Value = "True";

                SetPageHeader();

                IsPasswordExpanded = false;
                IsOraclePasswordExpanded = false;
                SetPasswordsVisibility();

                lnkSetPassword.Visible = true;

                if (user.UserId != CurrentUser.UserId)
                    lnkSetOraclePassword.Visible = true;

                SetupPageUI();
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

        //Navigate to the previous page
        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (FromHome != 1)
                Response.Redirect("~/ContentPages/ManageUsers.aspx");
            else
                Response.Redirect("~/ContentPages/Home.aspx");
        }

        //Set the password rows visibility according to the status of the sections (expanded or not)
        private void SetPasswordsVisibility()
        {
            if (IsPasswordExpanded)
            {
                rowPassword.Visible = true;
                rowPassword2.Visible = true;

                lnkSetPassword.Text = "Отказ от задаване на парола [x]";
            }
            else
            {
                rowPassword.Visible = false;
                rowPassword2.Visible = false;

                txtPassword.Text = "";
                txtPassword2.Text = "";

                lnkSetPassword.Text = "Задаване на парола";
            }

            if (IsOraclePasswordExpanded)
            {
                rowOraclePassword.Visible = true;
                rowOraclePassword2.Visible = true;

                lnkSetOraclePassword.Text = "Отказ от задаване на парола [x]";
            }
            else
            {
                rowOraclePassword.Visible = false;
                rowOraclePassword2.Visible = false;

                txtOraclePassword.Text = "";
                txtOraclePassword2.Text = "";

                lnkSetOraclePassword.Text = "Задаване на парола";
            }
        }

        //Expand or collapse the password section
        protected void lnkSetPassword_Click(object sender, EventArgs e)
        {
            IsPasswordExpanded = !IsPasswordExpanded;
            SetPasswordsVisibility();
        }

        //Expand or collapse the Oracle password section
        protected void lnkSetOraclePassword_Click(object sender, EventArgs e)
        {
            IsOraclePasswordExpanded = !IsOraclePasswordExpanded;
            SetPasswordsVisibility();
        }

        // Setup user interface elements according to rights of the user
        private void SetupPageUI()
        {
            if (UserId == 0) // add mode of page
            {
                bool screenHidden = GetUIItemAccessLevel("ADM_SECURITY") != UIAccessLevel.Enabled ||
                                    GetUIItemAccessLevel("ADM_SECURITY_USERS") != UIAccessLevel.Enabled ||
                                    GetUIItemAccessLevel("ADM_SECURITY_USERS_ADDUSER") != UIAccessLevel.Enabled;

                bool screenDisabled = GetUIItemAccessLevel("ADM_SECURITY") == UIAccessLevel.Disabled ||
                                      GetUIItemAccessLevel("ADM_SECURITY_USERS") == UIAccessLevel.Disabled ||
                                      GetUIItemAccessLevel("ADM_SECURITY_USERS_ADDUSER") == UIAccessLevel.Disabled;

                if (screenHidden)
                    RedirectAccessDenied();

                if (screenDisabled)
                {
                    pageDisabledControls.Add(btnSave);
                }

                UIAccessLevel l;

                l = GetUIItemAccessLevel("ADM_SECURITY_USERS_ADDUSER_USERNAME");
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

                l = GetUIItemAccessLevel("ADM_SECURITY_USERS_ADDUSER_PASSWORD");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblPassword);
                    pageDisabledControls.Add(txtPassword);

                    pageDisabledControls.Add(lblPassword2);
                    pageDisabledControls.Add(txtPassword2);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(lblPassword);
                    pageHiddenControls.Add(txtPassword);

                    pageHiddenControls.Add(lblPassword2);
                    pageHiddenControls.Add(txtPassword2);
                }

                l = GetUIItemAccessLevel("ADM_SECURITY_USERS_ADDUSER_ORCLUSERNAME");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblOracleUsername);
                    pageDisabledControls.Add(txtOracleUsername);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(lblOracleUsername);
                    pageHiddenControls.Add(txtOracleUsername);
                }

                l = this.GetUIItemAccessLevel("ADM_SECURITY_USERS_ADDUSER_ORCLPASSWORD");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblOraclePassword);
                    pageDisabledControls.Add(txtOraclePassword);

                    pageDisabledControls.Add(lblOraclePassword2);
                    pageDisabledControls.Add(txtOraclePassword2);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(lblOraclePassword);
                    pageHiddenControls.Add(txtOraclePassword);

                    pageHiddenControls.Add(lblOraclePassword2);
                    pageHiddenControls.Add(txtOraclePassword2);
                }

                l = GetUIItemAccessLevel("ADM_SECURITY_USERS_ADDUSER_ACTIVE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(chkActive);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(chkActive);
                }

                l = GetUIItemAccessLevel("ADM_SECURITY_USERS_ADDUSER_BLOCKED");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(chkBlocked);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(chkBlocked);
                }

                l = GetUIItemAccessLevel("ADM_SECURITY_USERS_ADDUSER_PASSWORDDOESNOTEXPIRE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(chkPasswordDoesNotExpire);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(chkPasswordDoesNotExpire);
                }

                l = GetUIItemAccessLevel("ADM_SECURITY_USERS_ADDUSER_FIRSTNAME");
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

                l = GetUIItemAccessLevel("ADM_SECURITY_USERS_ADDUSER_MIDDLENAME");
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

                l = GetUIItemAccessLevel("ADM_SECURITY_USERS_ADDUSER_LASTNAME");
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

                l = GetUIItemAccessLevel("ADM_SECURITY_USERS_ADDUSER_EMAIL");
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

                l = GetUIItemAccessLevel("ADM_SECURITY_USERS_ADDUSER_PHONE");
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

                l = GetUIItemAccessLevel("ADM_SECURITY_USERS_ADDUSER_ADMROLE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblADM_Role);
                    pageDisabledControls.Add(ddADM_Role);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(lblADM_Role);
                    pageHiddenControls.Add(ddADM_Role);
                }

                l = GetUIItemAccessLevel("ADM_SECURITY_USERS_ADDUSER_HSROLE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblHS_Role);
                    pageDisabledControls.Add(ddHS_Role);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(lblHS_Role);
                    pageHiddenControls.Add(ddHS_Role);
                }

                l = GetUIItemAccessLevel("ADM_SECURITY_USERS_ADDUSER_APPLROLE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblAPPL_Role);
                    pageDisabledControls.Add(ddAPPL_Role);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(lblAPPL_Role);
                    pageHiddenControls.Add(ddAPPL_Role);
                }

                l = GetUIItemAccessLevel("ADM_SECURITY_USERS_ADDUSER_RESROLE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblRES_Role);
                    pageDisabledControls.Add(ddRES_Role);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(lblRES_Role);
                    pageHiddenControls.Add(ddRES_Role);
                }

                l = GetUIItemAccessLevel("ADM_SECURITY_USERS_ADDUSER_REPROLE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblREP_Role);
                    pageDisabledControls.Add(ddREP_Role);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(lblREP_Role);
                    pageHiddenControls.Add(ddREP_Role);
                }
            }
            else // edit mode of page
            {
                bool screenHidden = GetUIItemAccessLevel("ADM_SECURITY") == UIAccessLevel.Hidden ||
                                    GetUIItemAccessLevel("ADM_SECURITY_USERS") == UIAccessLevel.Hidden ||
                                    GetUIItemAccessLevel("ADM_SECURITY_USERS_EDITUSER") == UIAccessLevel.Hidden;

                bool screenDisabled = GetUIItemAccessLevel("ADM_SECURITY") == UIAccessLevel.Disabled ||
                                      GetUIItemAccessLevel("ADM_SECURITY_USERS") == UIAccessLevel.Disabled ||
                                      GetUIItemAccessLevel("ADM_SECURITY_USERS_EDITUSER") == UIAccessLevel.Disabled;

                if (screenHidden)
                    RedirectAccessDenied();

                if (screenDisabled)
                {
                    pageDisabledControls.Add(btnSave);
                }

                UIAccessLevel l;

                l = GetUIItemAccessLevel("ADM_SECURITY_USERS_EDITUSER_USERNAME");
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

                l = this.GetUIItemAccessLevel("ADM_SECURITY_USERS_EDITUSER_PASSWORD");
                if (l == UIAccessLevel.Disabled || screenDisabled || l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(lnkSetPassword);
                }

                l = GetUIItemAccessLevel("ADM_SECURITY_USERS_EDITUSER_ORCLUSERNAME");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblOracleUsername);
                    pageDisabledControls.Add(txtOracleUsername);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(lblOracleUsername);
                    pageHiddenControls.Add(txtOracleUsername);
                }

                l = this.GetUIItemAccessLevel("ADM_SECURITY_USERS_EDITUSER_ORCLPASSWORD");
                if (l == UIAccessLevel.Disabled || screenDisabled || l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(lnkSetOraclePassword);
                }

                l = GetUIItemAccessLevel("ADM_SECURITY_USERS_EDITUSER_ACTIVE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(chkActive);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(chkActive);
                }

                l = GetUIItemAccessLevel("ADM_SECURITY_USERS_EDITUSER_BLOCKED");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(chkBlocked);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(chkBlocked);
                }

                l = GetUIItemAccessLevel("ADM_SECURITY_USERS_EDITUSER_PASSWORDDOESNOTEXPIRE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(chkPasswordDoesNotExpire);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(chkPasswordDoesNotExpire);
                }

                l = GetUIItemAccessLevel("ADM_SECURITY_USERS_EDITUSER_FIRSTNAME");
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

                l = GetUIItemAccessLevel("ADM_SECURITY_USERS_EDITUSER_MIDDLENAME");
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

                l = GetUIItemAccessLevel("ADM_SECURITY_USERS_EDITUSER_LASTNAME");
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

                l = GetUIItemAccessLevel("ADM_SECURITY_USERS_EDITUSER_EMAIL");
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

                l = GetUIItemAccessLevel("ADM_SECURITY_USERS_EDITUSER_PHONE");
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

                l = GetUIItemAccessLevel("ADM_SECURITY_USERS_EDITUSER_ADMROLE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblADM_Role);
                    pageDisabledControls.Add(ddADM_Role);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(lblADM_Role);
                    pageHiddenControls.Add(ddADM_Role);
                }

                l = GetUIItemAccessLevel("ADM_SECURITY_USERS_EDITUSER_HSROLE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblHS_Role);
                    pageDisabledControls.Add(ddHS_Role);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(lblHS_Role);
                    pageHiddenControls.Add(ddHS_Role);
                }

                l = GetUIItemAccessLevel("ADM_SECURITY_USERS_EDITUSER_APPLROLE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblAPPL_Role);
                    pageDisabledControls.Add(ddAPPL_Role);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(lblAPPL_Role);
                    pageHiddenControls.Add(ddAPPL_Role);
                }

                l = GetUIItemAccessLevel("ADM_SECURITY_USERS_EDITUSER_RESROLE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblRES_Role);
                    pageDisabledControls.Add(ddRES_Role);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(lblRES_Role);
                    pageHiddenControls.Add(ddRES_Role);
                }

                l = GetUIItemAccessLevel("ADM_SECURITY_USERS_EDITUSER_REPROLE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblREP_Role);
                    pageDisabledControls.Add(ddREP_Role);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(lblREP_Role);
                    pageHiddenControls.Add(ddREP_Role);
                }
            }
        }
    }
}
