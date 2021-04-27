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
    public partial class UIItemsPerRole : AdmPage
    {
        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "ADM_SECURITY_UIITEMSPERROLE";
            }
        }

        //This property represents the ID of the Role that is loaded on the screen (passed from the Manage Roles screen)
        //If this is a new role then the ID is 0
        //It is stored in a hidden field on the page
        private int SpecificRoleId
        {
            get
            {
                int roleId = 0;
                if (String.IsNullOrEmpty(this.hfSpecificRoleID.Value)
                    || this.hfSpecificRoleID.Value == "0")
                {
                    if (Request.Params["RoleId"] != null)
                        int.TryParse(Request.Params["RoleId"].ToString(), out roleId);

                    this.hfSpecificRoleID.Value = roleId.ToString();
                }
                else
                {
                    Int32.TryParse(this.hfSpecificRoleID.Value, out roleId);
                }

                return roleId;
            }

            set
            {
                this.hfSpecificRoleID.Value = value.ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if (GetUIItemAccessLevel("ADM_SECURITY_UIITEMSPERROLE") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            LnkForceNoChangesCheck(btnSave);

            HighlightMenuItems("Users", "Users_UIItemsPerRole");

            radEnabled.Text = UIItemUtil.UIAccessLеvelToString(UIAccessLevel.Enabled);
            radDisabled.Text = UIItemUtil.UIAccessLеvelToString(UIAccessLevel.Disabled);
            radHidden.Text = UIItemUtil.UIAccessLеvelToString(UIAccessLevel.Hidden);

            lblMessage.Text = "";

            //Hide the navigation buttons
            HideNavigationControls(btnBack);

            //Load the page for the first time: Load the data on the screen
            if (!IsPostBack)
            {
                LoadDropDowns();

                if (SpecificRoleId > 0)
                {
                    UserRole role = UserRoleUtil.GetUserRole(CurrentUser, SpecificRoleId);

                    if (role != null)
                    {
                        ddModules.SelectedValue = role.Module.ModuleId.ToString();
                        ddModules_Change(ddModules, new EventArgs());
                        ddRoles.SelectedValue = role.RoleId.ToString();

                        ddModules.Enabled = false;
                        ddRoles.Enabled = false;
                    }
                }

                PopulateUIItems();
            }
        }

        //Load any drop-down on the screen
        private void LoadDropDowns()
        {
            PopulateModules();
            PopulateRoles();
        }

        //Populate the modules drop-down
        private void PopulateModules()
        {
            ddModules.Items.Clear();

            List<Module> modules = ModuleUtil.GetModules(CurrentUser);

            foreach (Module module in modules)
            {
                ListItem li = new ListItem();
                li.Text = module.ModuleName;
                li.Value = module.ModuleId.ToString();

                ddModules.Items.Add(li);
            }
        }

        //Populate the roles drop-down
        private void PopulateRoles()
        {
            ddRoles.Items.Clear();

            List<UserRole> roles = UserRoleUtil.GetUserRoles(CurrentUser, "", ddModules.SelectedValue, 1, 0, 0);

            foreach (UserRole role in roles)
            {
                ListItem li = new ListItem();
                li.Text = role.RoleName;
                li.Value = role.RoleId.ToString();

                ddRoles.Items.Add(li);
            }
        }

        private void LoadTreeItems(List<UIItem> UIItems, TreeNodeCollection nodes, ref int depthLevel)
        {
            depthLevel++;

            foreach (UIItem UIItem in UIItems)
            {
                string cssClass = "";

                if (UIItem.AccessLevel == UIAccessLevel.Disabled)
                {
                    cssClass = "UITreeItem_Disabled";
                }

                if (UIItem.AccessLevel == UIAccessLevel.Hidden)
                {
                    cssClass = "UITreeItem_Hidden";
                }

                TreeNode node = new TreeNode();
                node.Value = UIItem.UIItemId.ToString();
                //2016-02-02: We commented the call of LinkClick() because we found that in Firefox and Chrome it was throwing the following exception:
                //"Error: Sys.ParameterCountException: Parameter count mismatch."
                //It looks like the cause for this error was the fact that LinkClick() was called twice. One time from the "<a>" click and one time from this explicit call.
                //So, we commented the explicit call here and the issue was fixed.
                node.Text = "<span class='" + cssClass + "' onclick='/*LinkClick(event);*/ return false;' CheckForChanges='true'>" + UIItem.UIName + "</span>";
                node.Expanded = (depthLevel <= 0);

                nodes.Add(node);

                int currDepthLevel = depthLevel;

                if (UIItem.ChildUIItems.Count > 0)
                {
                    LoadTreeItems(UIItem.ChildUIItems, node.ChildNodes, ref depthLevel);
                }

                depthLevel = currDepthLevel;
            }
        }

        private void PopulateUIItems()
        {
            Module module = ModuleUtil.GetModule(CurrentUser, int.Parse(ddModules.SelectedValue));

            int roleId = 0;

            try
            {
                roleId = int.Parse(ddRoles.SelectedValue);
            }
            catch
            {
                roleId = 0;
            }

            //Get the relevant UIItems tree
            List<UIItem> UIItems = UIItemUtil.GetUIItems(module.ModuleKey, CurrentUser, true, roleId, null);

            //Sort the tree elements
            UIItemUtil.SortUIItems(UIItems);

            lblUIItemName.Text = "";
            hdnUIItemID.Value = "";

            radDisabled.Checked = false;
            radEnabled.Checked = true;
            radHidden.Checked = false;

            SetVisibility();

            treeUIItems.Nodes.Clear();

            TreeNode node = new TreeNode();
            node.Value = "";
            node.Text = "<span onclick='/*LinkClick(event);*/ return false;' CheckForChanges='true'>" + module.ModuleName + "</span>";
            node.Expanded = true;

            treeUIItems.Nodes.Add(node);

            int depthLevel = 0;

            LoadTreeItems(UIItems[0].ChildUIItems, node.ChildNodes, ref depthLevel);
        }

        //Save the form data (first chech if it is valid)
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        //Navigate to the previous page
        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (SpecificRoleId > 0)
                Response.Redirect("~/ContentPages/ManageRoles.aspx");
            else
                Response.Redirect("~/ContentPages/Home.aspx");
        }

        protected void ddModules_Change(object sender, EventArgs e)
        {
            PopulateRoles();
            ddRoles_Change(ddModules, new EventArgs());
        }

        protected void ddRoles_Change(object sender, EventArgs e)
        {
            PopulateUIItems();
            hdnSavedChanges.Value = "True";
        }

        protected void treeUIItems_SelectionChanged(object sender, EventArgs e)
        {
            radDisabled.Checked = false;
            radEnabled.Checked = true;
            radHidden.Checked = false;

            if (treeUIItems.SelectedNode != null && treeUIItems.SelectedNode.Value != "")
            {
                int roleId = 0;

                try
                {
                    roleId = int.Parse(ddRoles.SelectedValue);
                }
                catch
                {
                    roleId = 0;
                }

                int UIItemId = int.Parse(treeUIItems.SelectedNode.Value);

                UIItem UIItem = UIItemUtil.GetUIItems("", CurrentUser, false, roleId, UIItemId)[0];

                radDisabled.Checked = UIItem.AccessLevel == UIAccessLevel.Disabled;
                radEnabled.Checked = UIItem.AccessLevel == UIAccessLevel.Enabled;
                radHidden.Checked = UIItem.AccessLevel == UIAccessLevel.Hidden;

                chkAccessOnlyOwnData.Checked = UIItem.AccessOnlyOwnData;

                lblUIItemName.Text = UIItem.UIName;
                hdnUIItemID.Value = UIItemId.ToString();
            }
            else
            {
                lblUIItemName.Text = "";
                hdnUIItemID.Value = "";
            }

            SetVisibility();

            hdnSavedChanges.Value = "True";
        }

        private void SetVisibility()
        {
            if (hdnUIItemID.Value == "")
            {
                rowAccessLevel.Visible = false;
                rowAccessOnlyOwnData.Visible = false;
                btnSave.Visible = false;
            }
            else
            {
                int roleId = 0;

                try
                {
                    roleId = int.Parse(ddRoles.SelectedValue);
                }
                catch
                {
                    roleId = 0;
                }

                int UIItemId = int.Parse(treeUIItems.SelectedNode.Value);

                UIItem UIItem = UIItemUtil.GetUIItems("", CurrentUser, false, roleId, UIItemId)[0];

                
                //Set the visibility
                rowAccessLevel.Visible = true;
                rowAccessOnlyOwnData.Visible = UIItem.CanSetAccessOnlyOwnData;
                btnSave.Visible = true;

                bool disabled = false;

                //Do not allow the current user to modify its access level becasue he can set such an access so he can't access the system anymore
                if (CurrentUser.Role.RoleId == roleId)
                {
                    if (UIItem.UIKey == "ADM_SECURITY_UIITEMSPERROLE" ||
                        UIItem.UIKey == "ADM_SECURITY")
                    {
                        disabled = true;
                    }
                }
                else
                {
                    if (GetUIItemAccessLevel("ADM_SECURITY_UIITEMSPERROLE") == UIAccessLevel.Disabled)
                        disabled = true;
                }

                if (disabled)
                {
                    pageDisabledControls.Add(radEnabled);
                    pageDisabledControls.Add(radDisabled);
                    pageDisabledControls.Add(radHidden);

                    pageDisabledControls.Add(btnSave);
                }
                else
                {
                    EnableControl(radEnabled);
                    EnableControl(radDisabled);
                    EnableControl(radHidden);

                    EnableButton(btnSave);
                }
            }
        }

        private void SaveData()
        {
            int roleId = 0;

            try
            {
                roleId = int.Parse(ddRoles.SelectedValue);
            }
            catch
            {
                roleId = 0;
            }

            int uiItemId = int.Parse(hdnUIItemID.Value);

            UIAccessLevel accessLevel = UIAccessLevel.Enabled;

            if (radEnabled.Checked)
                accessLevel = UIAccessLevel.Enabled;
            else if (radDisabled.Checked)
                accessLevel = UIAccessLevel.Disabled;
            else if (radHidden.Checked)
                accessLevel = UIAccessLevel.Hidden;

            //Track the changes into the Audit Trail 
            Change change = new Change(CurrentUser, "ADM_UIItemsPerRole");

            UIItemUtil.SetUIItemAccessLevel(CurrentUser, roleId, uiItemId, accessLevel, 
                                            chkAccessOnlyOwnData.Checked, change);

            change.WriteLog();

            if (treeUIItems.SelectedNode != null && treeUIItems.SelectedNode.Value != "")
            {
                string cssClass = "";

                if (accessLevel == UIAccessLevel.Disabled)
                {
                    cssClass = "UITreeItem_Disabled";
                }

                if (accessLevel == UIAccessLevel.Hidden)
                {
                    cssClass = "UITreeItem_Hidden";
                }

                UIItem UIItem = UIItemUtil.GetUIItems("", CurrentUser, false, roleId, uiItemId)[0];
                treeUIItems.SelectedNode.Text = "<span class='" + cssClass + "'onclick='/*LinkClick(event);*/ return false;' >" + UIItem.UIName + "</span>";
            }

            lblMessage.Text = "Записът е успешен";
            lblMessage.CssClass = "SuccessText";
            hdnSavedChanges.Value = "True";
        }
    }
}
