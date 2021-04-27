using System;
using System.Linq;
using System.Web.UI.WebControls;

using PMIS.Common;
using PMIS.PMISAdmin.Common;
using System.Collections.Generic;


namespace PMIS.PMISAdmin.ContentPages
{
    public partial class MilitaryStructureAccessPerUser : AdmPage
    {
        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "ADM_SECURITY_USERS_MILSTRUCTUREACCESS";
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

        protected void Page_Load(object sender, EventArgs e)
        {
            //Highlight the current page in the menu bar
            HighlightMenuItems("Users");

            //Load the page for the first time: Load the data on the screen
            if (!IsPostBack)
            {
                PopulateDropDowns();

                LoadData();
                SetPageHeader();
            }

            SetupPageUI();
        }

        private void PopulateDropDowns()
        {
            ddMilitaryDepartment.DataSource = MilitaryDepartmentUtil.GetAllMilitaryDepartmentsWithoutRestrictions(CurrentUser);
            ddMilitaryDepartment.DataTextField = "MilitaryDepartmentName";
            ddMilitaryDepartment.DataValueField = "MilitaryDepartmentId";
            ddMilitaryDepartment.DataBind();
            ddMilitaryDepartment.Items.Insert(0, ListItems.GetOptionChooseOne());
        }

        //Set the correct page header
        private void SetPageHeader()
        {
            string header = "Дефиниране права на достъп до военни структури";

            lblHeaderTitle.InnerHtml = header;
            Page.Title = header;
        }

        //Load the existing data in Edit mode
        private void LoadData()
        {
            User user = UserUtil.GetUser(CurrentUser, UserId);

            lblUserNameValue.InnerHtml = user.Username;
            lblFullNameValue.InnerHtml = user.FullName;

            //Load the selected Military Departments
            List<MilitaryDepartment> selectedMilitaryDepartments = MilitaryDepartmentUtil.GetAllMilitaryDepartmentsPerUser(CurrentUser, user);
            lstSelectedMilitaryDepartments.Items.Clear();

            foreach (MilitaryDepartment milDepartment in selectedMilitaryDepartments)
            {
                ListItem li = new ListItem();
                li.Value = milDepartment.MilitaryDepartmentId.ToString();
                li.Text = milDepartment.MilitaryDepartmentName;

                lstSelectedMilitaryDepartments.Items.Add(li);
            }

            //Load the selected Military Units
            List<MilitaryUnit> selectedMilitaryUnits = MilitaryUnitUtil.GetMilitaryUnitsAssignedToUserWithoutChilds(CurrentUser, user);
            lstSelectedMilitaryUnits.Items.Clear();

            foreach (MilitaryUnit milUnit in selectedMilitaryUnits)
            {
                ListItem li = new ListItem();
                li.Value = milUnit.MilitaryUnitId.ToString();
                li.Text = milUnit.DisplayTextForSelection;

                lstSelectedMilitaryUnits.Items.Add(li);
            }
        }

        //Save the data
        private void SaveData()
        {
            //Track the changes into the Audit Trail 
            Change change = new Change(CurrentUser, "ADM_MilStructureAccess");

            User user = UserUtil.GetUser(CurrentUser, UserId);

            //Save the changes
            MilitaryDepartmentUtil.UpdateMilitaryDepartmentsPerUser(user, hdnSelectedMilitaryDepartments.Value, CurrentUser, change);
            MilitaryUnitUtil.UpdateMilitaryUnitsPerUser(user, hdnSelectedMilitaryUnits.Value, CurrentUser, change);

            this.lblMessage.CssClass = "SuccessText";
            this.lblMessage.Text = "Записът е успешен";

            SetPageHeader();

            //Finally write any changes to the log
            change.WriteLog();
        }

        //Save the form data (first check if it is valid)
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
            LoadData();
        }

        //Go back
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ContentPages/ManageUsers.aspx");
        }

        // Setup user interface elements according to rights of the user
        private void SetupPageUI()
        {
            bool screenHidden = GetUIItemAccessLevel("ADM_SECURITY") == UIAccessLevel.Hidden ||
                                GetUIItemAccessLevel("ADM_SECURITY_USERS") == UIAccessLevel.Hidden ||
                                GetUIItemAccessLevel("ADM_SECURITY_USERS_MILSTRUCTUREACCESS") == UIAccessLevel.Hidden;

            bool screenDisabled = GetUIItemAccessLevel("ADM_SECURITY") == UIAccessLevel.Disabled ||
                                  GetUIItemAccessLevel("ADM_SECURITY_USERS") == UIAccessLevel.Disabled || 
                                  GetUIItemAccessLevel("ADM_SECURITY_USERS_MILSTRUCTUREACCESS") == UIAccessLevel.Disabled || 
                                  CurrentUser.UserId == UserId;

            if (screenHidden)
                RedirectAccessDenied();

            if (screenDisabled)
            {
                pageHiddenControls.Add(btnSave);
            }

            List<string> disabledClientControls = new List<string>();
            List<string> hiddenClientControls = new List<string>();

            UIAccessLevel l;

            l = GetUIItemAccessLevel("ADM_SECURITY_USERS_MILSTRUCTUREACCESS_MILDEPARTMENTS");
            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                hiddenClientControls.Add(lblMilitaryDepartment.ClientID);
                pageHiddenControls.Add(ddMilitaryDepartment);
                hiddenClientControls.Add("btnSelectMilitaryDepartment");
                hiddenClientControls.Add("btnRemoveSelectedMilDepartments");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("divMilitaryDepartmentCont");
            }

            l = GetUIItemAccessLevel("ADM_SECURITY_USERS_MILSTRUCTUREACCESS_MILUNITS");
            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                hiddenClientControls.Add(lblMilitaryUnit.ClientID);
                hiddenClientControls.Add("musMilitaryUnitCont");
                hiddenClientControls.Add("btnSelectMilitaryUnit");
                hiddenClientControls.Add("btnRemoveSelectedMilUnits");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("divMilitaryUnitCont");
            }

            SetDisabledClientControls(disabledClientControls.ToArray());
            SetHiddenClientControls(hiddenClientControls.ToArray());
        }
    }
}
