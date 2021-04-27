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
    public partial class AddEditMilRepSpeciality : AdmPage
    {
        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "ADM_LISTMAINT_MILREPORTSPECIALITY";
            }
        }

        //This property represents the ID of the Role object that is loaded on the screen
        //If this is a new role then the ID is 0
        //It is stored in a hidden field on the page
        private int MilReportSpecialityId
        {
            get
            {
                int milReportSpecialityId = 0;
                if (String.IsNullOrEmpty(this.hfMilReportSpecialityID.Value)
                    || this.hfMilReportSpecialityID.Value == "0")
                {
                    if (Request.Params["MilReportSpecialityId"] != null)
                        int.TryParse(Request.Params["MilReportSpecialityId"].ToString(), out milReportSpecialityId);

                    this.hfMilReportSpecialityID.Value = milReportSpecialityId.ToString();
                }
                else
                {
                    Int32.TryParse(this.hfMilReportSpecialityID.Value, out milReportSpecialityId);
                }

                return milReportSpecialityId;
            }

            set
            {
                this.hfMilReportSpecialityID.Value = value.ToString(); 
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
            HighlightMenuItems("Lists", "Lists_ADM_MilRepSpecialities");

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
            string header = (MilReportSpecialityId > 0 ? "Редактиране на ВОС" : "Добавяне на ВОС");

            lblHeaderTitle.InnerHtml = header;
            Page.Title = header;
        }

        //Load any drop-down on the screen
        private void LoadDropDowns()
        {
            PopulateTypes();
            PopulateMilitaryForceSort();
        }

        //Populate the types drop-down
        private void PopulateTypes()
        {
            ddMilRepSpecialityTypes.Items.Clear();
            ddMilRepSpecialityTypes.Items.Add(new ListItem("", "0"));

            List<MilitaryReportSpecialityType> types = MilitaryReportSpecialityTypeUtil.GetAllMilitaryReportSpecialityTypes(CurrentUser);

            foreach (MilitaryReportSpecialityType type in types)
            {
                ListItem li = new ListItem();
                li.Text = type.TypeName;
                li.Value = type.Type.ToString();

                ddMilRepSpecialityTypes.Items.Add(li);
            }
        }

        //Populate the mil force sort drop-down
        private void PopulateMilitaryForceSort()
        {
            ddMilitaryForceSort.Items.Clear();
            ddMilitaryForceSort.Items.Add(ListItems.GetOptionChooseOne());

            List<MilitaryForceSort> milForceSorts = MilitaryForceSortUtil.GetAllMilitaryForceSorts(CurrentUser);

            foreach (MilitaryForceSort sort in milForceSorts)
            {
                ListItem li = new ListItem();
                li.Text = sort.MilitaryForceSortName;
                li.Value = sort.MilitaryForceSortId.ToString();

                ddMilitaryForceSort.Items.Add(li);
            }
        }

        //Load the existing data in Edit mode
        private void LoadData()
        {
            //If Edit mode
            if (MilReportSpecialityId > 0)
            {
                MilitaryReportSpeciality milReportSpeciality = MilitaryReportSpecialityUtil.GetMilitaryReportSpeciality(MilReportSpecialityId, CurrentUser);

                ddMilRepSpecialityTypes.SelectedValue = milReportSpeciality.MilReportSpecialityTypeId.ToString();
                txtMilReportingSpecialityCode.Text = milReportSpeciality.MilReportingSpecialityCode;
                txtMilReportingSpecialityName.Text = milReportSpeciality.MilReportingSpecialityName;

                if (milReportSpeciality.MilitaryForceSortId.HasValue)
                {
                    ddMilitaryForceSort.SelectedValue = milReportSpeciality.MilitaryForceSortId.Value.ToString();
                }

                chkActive.Checked = milReportSpeciality.Active;
            }            
        }

        //Collect the information from the page form
        //Store the data in a object in the memory and use that object when peforming DB operations
        private MilitaryReportSpeciality CollectData()
        {
            MilitaryReportSpeciality milReportSpeciality = new MilitaryReportSpeciality(CurrentUser);

            milReportSpeciality.MilReportSpecialityId = MilReportSpecialityId;
            milReportSpeciality.MilReportSpecialityTypeId = int.Parse(ddMilRepSpecialityTypes.SelectedValue);
            milReportSpeciality.MilReportingSpecialityCode = txtMilReportingSpecialityCode.Text;
            milReportSpeciality.MilReportingSpecialityName = txtMilReportingSpecialityName.Text;

            if (ddMilitaryForceSort.SelectedValue != ListItems.GetOptionChooseOne().Value)
                milReportSpeciality.MilitaryForceSortId = int.Parse(ddMilitaryForceSort.SelectedValue);

            milReportSpeciality.Active = chkActive.Checked;

            return milReportSpeciality;
        }

        //Validate the form data before doing any server actions
        private bool ValidateData()
        {
            bool isDataValid = true;
            string errMsg = "";
            List<string> errRightsFields = new List<string>();

            if (txtMilReportingSpecialityCode.Text.Trim() == "")
            {
                isDataValid = false;

                if (pageDisabledControls.Contains(txtMilReportingSpecialityCode) || pageHiddenControls.Contains(txtMilReportingSpecialityCode))
                    errRightsFields.Add("Код");
                else
                    errMsg += "Трябва да въведете код<br/>";
            }

            if (txtMilReportingSpecialityName.Text.Trim() == "")
            {
                isDataValid = false;

                if (pageDisabledControls.Contains(txtMilReportingSpecialityName) || pageHiddenControls.Contains(txtMilReportingSpecialityName))
                    errRightsFields.Add("Име");
                else
                    errMsg += "Трябва да въведете име<br/>";
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
            MilitaryReportSpeciality militaryReportSpeciality = CollectData();

            //Track the changes into the Audit Trail 
            Change change = new Change(CurrentUser, "ADM_Lists_MilReportSpeciality");

            //If a successfull save operation then indicate this on the screen; Otherwise alert a warning
            if (MilitaryReportSpecialityUtil.SaveMilReportSpeciality(CurrentUser, militaryReportSpeciality, change))
            {
                if (MilReportSpecialityId == 0)
                {
                    SetLocationHash("AddEditMilRepSpeciality.aspx?MilReportSpecialityId=" + militaryReportSpeciality.MilReportSpecialityId.ToString());
                }

                MilReportSpecialityId = militaryReportSpeciality.MilReportSpecialityId;

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
                Response.Redirect("~/ContentPages/ManageMilRepSpecialities.aspx");
            else
                Response.Redirect("~/ContentPages/Home.aspx");
        }

        // Setup user interface elements according to rights of the user's role
        private void SetupPageUI()
        {
            if (MilReportSpecialityId == 0) // add mode of page
            {
                bool screenHidden = GetUIItemAccessLevel("ADM_LISTMAINT") != UIAccessLevel.Enabled ||
                                    GetUIItemAccessLevel("ADM_LISTMAINT_MILREPORTSPECIALITY") != UIAccessLevel.Enabled ||
                                    GetUIItemAccessLevel("ADM_LISTMAINT_MILREPORTSPECIALITY_ADD") != UIAccessLevel.Enabled;

                bool screenDisabled = GetUIItemAccessLevel("ADM_LISTMAINT") == UIAccessLevel.Disabled ||
                                      GetUIItemAccessLevel("ADM_LISTMAINT_MILREPORTSPECIALITY") == UIAccessLevel.Disabled ||
                                      GetUIItemAccessLevel("ADM_LISTMAINT_MILREPORTSPECIALITY_ADD") == UIAccessLevel.Disabled;

                if (screenHidden)
                    RedirectAccessDenied();

                if (screenDisabled)
                {
                    pageDisabledControls.Add(btnSave);
                }

                UIAccessLevel l;

                l = GetUIItemAccessLevel("ADM_LISTMAINT_MILREPORTSPECIALITY_ADD_TYPE");

                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblMilRepSpecialityTypes);
                    pageDisabledControls.Add(ddMilRepSpecialityTypes);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(lblMilRepSpecialityTypes);
                    pageHiddenControls.Add(ddMilRepSpecialityTypes);
                }

                l = GetUIItemAccessLevel("ADM_LISTMAINT_MILREPORTSPECIALITY_ADD_CODE");

                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblMilReportingSpecialityCode);
                    pageDisabledControls.Add(txtMilReportingSpecialityCode);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(lblMilReportingSpecialityCode);
                    pageHiddenControls.Add(txtMilReportingSpecialityCode);
                }

                l = GetUIItemAccessLevel("ADM_LISTMAINT_MILREPORTSPECIALITY_ADD_NAME");

                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblMilReportingSpecialityName);
                    pageDisabledControls.Add(txtMilReportingSpecialityName);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(lblMilReportingSpecialityName);
                    pageHiddenControls.Add(txtMilReportingSpecialityName);
                }

                l = GetUIItemAccessLevel("ADM_LISTMAINT_MILREPORTSPECIALITY_ADD_MILFORCESORT");

                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblMilitaryForceSort);
                    pageDisabledControls.Add(ddMilitaryForceSort);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(lblMilitaryForceSort);
                    pageHiddenControls.Add(ddMilitaryForceSort);
                }

                l = GetUIItemAccessLevel("ADM_LISTMAINT_MILREPORTSPECIALITY_ADD_ACTIVE");

                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblActive);
                    pageDisabledControls.Add(chkActive);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(lblActive);
                    pageHiddenControls.Add(chkActive);
                }
            }
            else // edit mode of page
            {
                bool screenHidden = GetUIItemAccessLevel("ADM_LISTMAINT") == UIAccessLevel.Hidden ||
                                    GetUIItemAccessLevel("ADM_LISTMAINT_MILREPORTSPECIALITY") == UIAccessLevel.Hidden ||
                                    GetUIItemAccessLevel("ADM_LISTMAINT_MILREPORTSPECIALITY_EDIT") == UIAccessLevel.Hidden;

                bool screenDisabled = GetUIItemAccessLevel("ADM_LISTMAINT") == UIAccessLevel.Disabled ||
                                      GetUIItemAccessLevel("ADM_LISTMAINT_MILREPORTSPECIALITY") == UIAccessLevel.Disabled ||
                                      GetUIItemAccessLevel("ADM_LISTMAINT_MILREPORTSPECIALITY_EDIT") == UIAccessLevel.Disabled;

                if (screenHidden)
                    RedirectAccessDenied();

                if (screenDisabled)
                {
                    pageDisabledControls.Add(btnSave);
                }

                UIAccessLevel l;

                l = GetUIItemAccessLevel("ADM_LISTMAINT_MILREPORTSPECIALITY_EDIT_TYPE");

                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblMilRepSpecialityTypes);
                    pageDisabledControls.Add(ddMilRepSpecialityTypes);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(lblMilRepSpecialityTypes);
                    pageHiddenControls.Add(ddMilRepSpecialityTypes);
                }

                l = GetUIItemAccessLevel("ADM_LISTMAINT_MILREPORTSPECIALITY_EDIT_CODE");

                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblMilReportingSpecialityCode);
                    pageDisabledControls.Add(txtMilReportingSpecialityCode);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(lblMilReportingSpecialityCode);
                    pageHiddenControls.Add(txtMilReportingSpecialityCode);
                }

                l = GetUIItemAccessLevel("ADM_LISTMAINT_MILREPORTSPECIALITY_EDIT_NAME");

                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblMilReportingSpecialityName);
                    pageDisabledControls.Add(txtMilReportingSpecialityName);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(lblMilReportingSpecialityName);
                    pageHiddenControls.Add(txtMilReportingSpecialityName);
                }

                l = GetUIItemAccessLevel("ADM_LISTMAINT_MILREPORTSPECIALITY_EDIT_MILFORCESORT");

                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblMilitaryForceSort);
                    pageDisabledControls.Add(ddMilitaryForceSort);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(lblMilitaryForceSort);
                    pageHiddenControls.Add(ddMilitaryForceSort);
                }

                l = GetUIItemAccessLevel("ADM_LISTMAINT_MILREPORTSPECIALITY_EDIT_ACTIVE");

                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblActive);
                    pageDisabledControls.Add(chkActive);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(lblActive);
                    pageHiddenControls.Add(chkActive);
                }
            }

        }
    }
}
