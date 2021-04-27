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
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Globalization;

namespace PMIS.PMISAdmin.ContentPages
{
    public partial class AddEditRole : AdmPage
    {
        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "ADM_SECURITY_ROLES";
            }
        }

        //This property represents the ID of the Role object that is loaded on the screen
        //If this is a new role then the ID is 0
        //It is stored in a hidden field on the page
        private int RoleId
        {
            get
            {
                int roleId = 0;
                if (String.IsNullOrEmpty(this.hfRoleID.Value)
                    || this.hfRoleID.Value == "0")
                {
                    if (Request.Params["RoleId"] != null)
                        int.TryParse(Request.Params["RoleId"].ToString(), out roleId);

                    this.hfRoleID.Value = roleId.ToString();
                }
                else
                {
                    Int32.TryParse(this.hfRoleID.Value, out roleId);
                }

                return roleId;
            }

            set
            { 
                this.hfRoleID.Value = value.ToString(); 
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


        protected void Page_Load(object sender, EventArgs e)
        {
            //Hilight the correct item in the menu
            if (RoleId == 0)
                HighlightMenuItems("Users", "Users_AddRole");
            else
                HighlightMenuItems("Users");

            LnkForceNoChangesCheck(btnSave);

            //Hide the navigation buttons
            HideNavigationControls(btnBack);

            //Load the page for the first time: Load the data on the screen
            if (!IsPostBack)
            {
                LoadDropDowns();
                LoadData();
                SetPageHeader();
            }

            SetupPageUI();
        }

        //Set the correct page header (i.e. either New or Edit)
        private void SetPageHeader()
        {
            string header = (RoleId > 0 ? "Редактиране на потребителска роля" : "Въвеждане на нова потребителска роля");

            lblHeaderTitle.InnerHtml = header;
            Page.Title = header;
        }

        //Load any drop-down on the screen
        private void LoadDropDowns()
        {
            PopulateModules();
        }

        //Populate the modules drop-down
        private void PopulateModules()
        {
            ddModules.Items.Clear();
            ddModules.Items.Add(ListItems.GetOptionChooseOne());

            List<Module> modules = ModuleUtil.GetModules(CurrentUser);

            foreach (Module module in modules)
            {
                ListItem li = new ListItem();
                li.Text = module.ModuleName;
                li.Value = module.ModuleId.ToString();

                ddModules.Items.Add(li);
            }
        }

        //Load the existing data in Edit mode
        private void LoadData()
        {
            //If Edit mode
            if (RoleId > 0)
            {
                UserRole role = UserRoleUtil.GetUserRole(CurrentUser, RoleId);

                txtRoleName.Text = role.RoleName;
                ddModules.SelectedValue = role.Module.ModuleId.ToString();
            }            
        }

        //Collect the information from the page form
        //Store the data in a object in the memory and use that object when peforming DB operations
        private UserRole CollectData()
        {
            UserRole role = new UserRole(CurrentUser);

            role.RoleId = RoleId;
            role.RoleName = txtRoleName.Text;

            int moduleId = int.Parse(ddModules.SelectedValue);
            role.Module = ModuleUtil.GetModule(CurrentUser, moduleId);

            return role;
        }

        //Validate the form data before doing any server actions
        private bool ValidateData()
        {
            bool isDataValid = true;
            string errMsg = "";
            List<string> errRightsFields = new List<string>();

            if (txtRoleName.Text.Trim() == "")
            {
                isDataValid = false;

                if (pageDisabledControls.Contains(txtRoleName) || pageHiddenControls.Contains(txtRoleName))
                    errRightsFields.Add("Роля");
                else
                    errMsg += "Трябва да въведете име на ролята<br/>";
            }

            if (ddModules.Text.Trim() == ListItems.GetOptionChooseOne().Value)
            {
                isDataValid = false;

                if (pageDisabledControls.Contains(ddModules) || pageHiddenControls.Contains(ddModules))
                    errRightsFields.Add("Модул");
                else
                    errMsg += "Трябва да изберете модул<br/>";
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
            UserRole role = CollectData();

            //Track the changes into the Audit Trail 
            Change change = new Change(CurrentUser, "ADM_Roles");

            //If a successfull save operation then indicate this on the screen; Otherwise alert a warning
            if (UserRoleUtil.SaveRole(CurrentUser, role, change))
            {
                if (RoleId == 0)
                {
                    SetLocationHash("AddEditRole.aspx?RoleId=" + role.RoleId.ToString());
                }

                RoleId = role.RoleId;

                this.lblMessage.CssClass = "SuccessText";
                this.lblMessage.Text = "Записът е успешен";

                SetPageHeader();
                hdnSavedChanges.Value = "True";
                SetupPageUI();
            }
            else
            {
                this.lblMessage.CssClass = "ErrorText";
                this.lblMessage.Text = "Записът не е успешен";
            }

            change.WriteLog();
        }

        //Save the form data (first chech if it is valid)
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
                Response.Redirect("~/ContentPages/ManageRoles.aspx");
            else
                Response.Redirect("~/ContentPages/Home.aspx");
        }

        // Setup user interface elements according to rights of the user's role
        private void SetupPageUI()
        {
            if (RoleId == 0) // add mode of page
            {
                bool screenHidden = GetUIItemAccessLevel("ADM_SECURITY") != UIAccessLevel.Enabled ||
                                    GetUIItemAccessLevel("ADM_SECURITY_ROLES") != UIAccessLevel.Enabled ||
                                    GetUIItemAccessLevel("ADM_SECURITY_ROLES_ADDROLE") != UIAccessLevel.Enabled;

                bool screenDisabled = GetUIItemAccessLevel("ADM_SECURITY") == UIAccessLevel.Disabled ||
                                      GetUIItemAccessLevel("ADM_SECURITY_ROLES") == UIAccessLevel.Disabled ||
                                      GetUIItemAccessLevel("ADM_SECURITY_ROLES_ADDROLE") == UIAccessLevel.Disabled;

                if (screenHidden)
                    RedirectAccessDenied();

                if (screenDisabled)
                {
                    pageDisabledControls.Add(btnSave);
                }

                UIAccessLevel l;

                l = GetUIItemAccessLevel("ADM_SECURITY_ROLES_ADDROLE_ROLENAME");

                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblRoleName);
                    pageDisabledControls.Add(txtRoleName);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(lblRoleName);
                    pageHiddenControls.Add(txtRoleName);
                }

                l = this.GetUIItemAccessLevel("ADM_SECURITY_ROLES_ADDROLE_MODULE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblModule);
                    pageDisabledControls.Add(ddModules);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblModule);
                    this.pageHiddenControls.Add(ddModules);
                }
            }
            else // edit mode of page
            {
                bool screenHidden = GetUIItemAccessLevel("ADM_SECURITY") == UIAccessLevel.Hidden ||
                                    GetUIItemAccessLevel("ADM_SECURITY_ROLES") == UIAccessLevel.Hidden ||
                                    GetUIItemAccessLevel("ADM_SECURITY_ROLES_EDITROLE") == UIAccessLevel.Hidden;

                bool screenDisabled = GetUIItemAccessLevel("ADM_SECURITY") == UIAccessLevel.Disabled ||
                                      GetUIItemAccessLevel("ADM_SECURITY_ROLES") == UIAccessLevel.Disabled ||
                                      GetUIItemAccessLevel("ADM_SECURITY_ROLES_EDITROLE") == UIAccessLevel.Disabled;

                if (screenHidden)
                    RedirectAccessDenied();

                if (screenDisabled)
                {
                    pageDisabledControls.Add(btnSave);
                }

                UIAccessLevel l;

                l = GetUIItemAccessLevel("ADM_SECURITY_ROLES_EDITROLE_ROLENAME");

                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblRoleName);
                    pageDisabledControls.Add(txtRoleName);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(lblRoleName);
                    pageHiddenControls.Add(txtRoleName);
                }

                l = GetUIItemAccessLevel("ADM_SECURITY_ROLES_EDITROLE_MODULE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblModule);
                    pageDisabledControls.Add(ddModules);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(lblModule);
                    pageHiddenControls.Add(ddModules);
                }
            }

        }
    }
}
