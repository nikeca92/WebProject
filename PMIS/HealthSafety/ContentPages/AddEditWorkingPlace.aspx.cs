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
    public partial class AddEditWorkingPlace : HSPage
    {
        public string MilitaryUnitLabel = CommonFunctions.GetLabelText("MilitaryUnit");

        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "HS_LISTMAINT_WORKINGPLACES";
            }
        }

        //This property represents the ID of the Role object that is loaded on the screen
        //If this is a new role then the ID is 0
        //It is stored in a hidden field on the page
        private int WorkingPlaceId
        {
            get
            {
                int workingPlaceId = 0;
                if (String.IsNullOrEmpty(this.hfWorkingPlaceID.Value)
                    || this.hfWorkingPlaceID.Value == "0")
                {
                    if (Request.Params["WorkingPlaceId"] != null)
                        int.TryParse(Request.Params["WorkingPlaceId"].ToString(), out workingPlaceId);

                    this.hfWorkingPlaceID.Value = workingPlaceId.ToString();
                }
                else
                {
                    Int32.TryParse(this.hfWorkingPlaceID.Value, out workingPlaceId);
                }

                return workingPlaceId;
            }

            set
            {
                this.hfWorkingPlaceID.Value = value.ToString(); 
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
            HighlightMenuItems("Lists", "Lists_HS_WorkingPlaces");

            LnkForceNoChangesCheck(btnSave);

            //Hide the navigation buttons
            HideNavigationControls(btnBack);

            lblMilitaryUnit.Text = MilitaryUnitLabel + ":";

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
            string header = (WorkingPlaceId > 0 ? "Редактиране мястно на измерване" : "Добавяне място на измерване");

            lblHeaderTitle.InnerHtml = header;
            Page.Title = header;
        }

        //Load any drop-down on the screen
        private void LoadDropDowns()
        {
        }

        //Load the existing data in Edit mode
        private void LoadData()
        {
            //If Edit mode
            if (WorkingPlaceId > 0)
            {
                WorkingPlace workingPlace = WorkingPlaceUtil.GetWorkingPlace(WorkingPlaceId, CurrentUser);

                musMilitaryUnit.SelectedValue = workingPlace.MilitaryUnitId.ToString();
                musMilitaryUnit.SelectedText = workingPlace.MilitaryUnit.DisplayTextForSelection;

                txtWorkingPlace.Text = workingPlace.WorkingPlaceName;
            }            
        }

        //Collect the information from the page form
        //Store the data in a object in the memory and use that object when peforming DB operations
        private WorkingPlace CollectData()
        {
            WorkingPlace workingPlace = new WorkingPlace(CurrentUser);

            workingPlace.WorkingPlaceId = WorkingPlaceId;
            workingPlace.MilitaryUnitId = int.Parse(musMilitaryUnit.SelectedValue);
            workingPlace.WorkingPlaceName = txtWorkingPlace.Text;
            
            return workingPlace;
        }

        //Validate the form data before doing any server actions
        private bool ValidateData()
        {
            bool isDataValid = true;
            string errMsg = "";
            List<string> errRightsFields = new List<string>();

            if (musMilitaryUnit.SelectedValue.Trim() == "" ||
                musMilitaryUnit.SelectedValue.Trim() == ListItems.GetOptionChooseOne().Value)
            {
                isDataValid = false;

                if (pageDisabledControls.Contains(musMilitaryUnit) || pageHiddenControls.Contains(musMilitaryUnit))
                    errRightsFields.Add(MilitaryUnitLabel);
                else
                    errMsg += "Трябва да изберете " + MilitaryUnitLabel + "<br/>";
            }

            if (txtWorkingPlace.Text.Trim() == "")
            {
                isDataValid = false;

                if (pageDisabledControls.Contains(txtWorkingPlace) || pageHiddenControls.Contains(txtWorkingPlace))
                    errRightsFields.Add("Място на измерване");
                else
                    errMsg += "Трябва да въведете място на измерване<br/>";
            }

            if (isDataValid)
            {
                WorkingPlace existingWorkingPlace = WorkingPlaceUtil.GetWorkingPlaceByName(int.Parse(musMilitaryUnit.SelectedValue), txtWorkingPlace.Text, CurrentUser);

                if (existingWorkingPlace != null &&
                    WorkingPlaceId != existingWorkingPlace.WorkingPlaceId)
                {
                    isDataValid = false;
                    errMsg += "Въведеното място на измерване вече съществува за избраната " + MilitaryUnitLabel + "<br/>";
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

        //Save the data
        private void SaveData()
        {
            //First collect the data from the page form
            WorkingPlace workingPlace = CollectData();

            //Track the changes into the Audit Trail 
            Change change = new Change(CurrentUser, "HS_Lists_WorkingPlaces");

            //If a successfull save operation then indicate this on the screen; Otherwise alert a warning
            if (WorkingPlaceUtil.SaveWorkingPlace(CurrentUser, workingPlace, change))
            {
                if (WorkingPlaceId == 0)
                {
                    SetLocationHash("AddEditWorkingPlace.aspx?WorkingPlaceId=" + workingPlace.WorkingPlaceId.ToString());
                }

                WorkingPlaceId = workingPlace.WorkingPlaceId;

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
                Response.Redirect("~/ContentPages/ManageWorkingPlaces.aspx");
            else
                Response.Redirect("~/ContentPages/Home.aspx");
        }

        // Setup user interface elements according to rights of the user's role
        private void SetupPageUI()
        {
            if (WorkingPlaceId == 0) // add mode of page
            {
                bool screenHidden = GetUIItemAccessLevel("HS_LISTMAINT") != UIAccessLevel.Enabled ||
                                    GetUIItemAccessLevel("HS_LISTMAINT_WORKINGPLACES") != UIAccessLevel.Enabled ||
                                    GetUIItemAccessLevel("HS_LISTMAINT_WORKINGPLACES_ADD") != UIAccessLevel.Enabled;

                bool screenDisabled = GetUIItemAccessLevel("HS_LISTMAINT") == UIAccessLevel.Disabled ||
                                      GetUIItemAccessLevel("HS_LISTMAINT_WORKINGPLACES") == UIAccessLevel.Disabled ||
                                      GetUIItemAccessLevel("HS_LISTMAINT_WORKINGPLACES_ADD") == UIAccessLevel.Disabled;

                if (screenHidden)
                    RedirectAccessDenied();

                if (screenDisabled)
                {
                    pageDisabledControls.Add(btnSave);
                }

                UIAccessLevel l;

                l = GetUIItemAccessLevel("HS_LISTMAINT_WORKINGPLACES_ADD_MILITARYUNIT");

                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblMilitaryUnit);
                    pageDisabledControls.Add(musMilitaryUnit);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(lblMilitaryUnit);
                    pageHiddenControls.Add(musMilitaryUnit);
                }

                l = GetUIItemAccessLevel("HS_LISTMAINT_WORKINGPLACES_ADD_WORKINGPLACE");

                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblWorkingPlace);
                    pageDisabledControls.Add(txtWorkingPlace);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(lblWorkingPlace);
                    pageHiddenControls.Add(txtWorkingPlace);
                }
            }
            else // edit mode of page
            {
                bool screenHidden = GetUIItemAccessLevel("HS_LISTMAINT") == UIAccessLevel.Hidden ||
                                    GetUIItemAccessLevel("HS_LISTMAINT_WORKINGPLACES") == UIAccessLevel.Hidden ||
                                    GetUIItemAccessLevel("HS_LISTMAINT_WORKINGPLACES_EDIT") == UIAccessLevel.Hidden;

                bool screenDisabled = GetUIItemAccessLevel("HS_LISTMAINT") == UIAccessLevel.Disabled ||
                                      GetUIItemAccessLevel("HS_LISTMAINT_WORKINGPLACES") == UIAccessLevel.Disabled ||
                                      GetUIItemAccessLevel("HS_LISTMAINT_WORKINGPLACES_EDIT") == UIAccessLevel.Disabled;

                if (screenHidden)
                    RedirectAccessDenied();

                if (screenDisabled)
                {
                    pageDisabledControls.Add(btnSave);
                }

                UIAccessLevel l;

                l = GetUIItemAccessLevel("HS_LISTMAINT_WORKINGPLACES_EDIT_MILITARYUNIT");

                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblMilitaryUnit);
                    pageDisabledControls.Add(musMilitaryUnit);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(lblMilitaryUnit);
                    pageHiddenControls.Add(musMilitaryUnit);
                }

                l = GetUIItemAccessLevel("HS_LISTMAINT_WORKINGPLACES_EDIT_WORKINGPLACE");

                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblWorkingPlace);
                    pageDisabledControls.Add(txtWorkingPlace);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(lblWorkingPlace);
                    pageHiddenControls.Add(txtWorkingPlace);
                }
            }

        }
    }
}
