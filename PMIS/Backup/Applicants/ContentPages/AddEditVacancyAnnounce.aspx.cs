using System;
using System.Web.UI.WebControls;
using System.Collections.Generic;

using PMIS.Common;
using PMIS.Applicants.Common;
using System.Text;

using MilitaryUnitSelector;
using System.Web.UI;
using System.IO;

namespace PMIS.Applicants.ContentPages
{
    public partial class AddEditVacancyAnnounce : APPLPage
    {
        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "APPL_VACANN";
            }
        }

        //This property represents the ID of the VacancyAnnounce object that is loaded on the screen
        //If this is a new object then the ID is 0
        //It is stored in a hidden field on the page
        private int VacancyAnnounceId
        {
            get
            {
                int vacancyAnnounceId = 0;
                if (String.IsNullOrEmpty(this.hfVacancyAnnounceID.Value)
                    || this.hfVacancyAnnounceID.Value == "0")
                {
                    if (Request.Params["VacancyAnnounceId"] != null)
                        int.TryParse(Request.Params["VacancyAnnounceId"].ToString(), out vacancyAnnounceId);

                    this.hfVacancyAnnounceID.Value = vacancyAnnounceId.ToString();
                }
                else
                {
                    Int32.TryParse(this.hfVacancyAnnounceID.Value, out vacancyAnnounceId);
                }

                return vacancyAnnounceId;
            }

            set
            {
                this.hfVacancyAnnounceID.Value = value.ToString();
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

        private bool PageDisabled
        {
            get
            {
                bool pageDisabled = false;

                if (VacancyAnnounceId != 0)
                {
                    VacancyAnnounce vacancyAnnounce = VacancyAnnounceUtil.GetVacancyAnnounce(VacancyAnnounceId, CurrentUser);
                    string vacancyAnnounceStatusKey = vacancyAnnounce.VacancyAnnounceStatus.VacAnnStatusKey;

                    if (vacancyAnnounceStatusKey != "" && vacancyAnnounceStatusKey != "CREATINGORDER")
                    {
                        pageDisabled = true;
                    }
                }

                return pageDisabled;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadTab")
            {
                JSLoadTab();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetAddVacancyAnnouncePositionLightBox")
            {
                JSGetAddVacancyAnnouncePositionLightBox();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSAddVacancyAnnouncePositions")
            {
                JSAddVacancyAnnouncePositions();
                return;
            }

            //NLP added
            //Remove Exam for current  Announce 
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRemoveExam")
            {
                int examId = 0;
                if (Request.Params["examId"] != null)
                    int.TryParse(Request.Params["examId"].ToString(), out examId);

                JSRemoveExam(examId);
                return;
            }

            //Remove Document for current  Announce 
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRemoveDocument")
            {
                int documentId = 0;
                if (Request.Params["documentId"] != null)
                    int.TryParse(Request.Params["documentId"].ToString(), out documentId);

                JSRemoveDocument(documentId);
                return;
            }

            //Add Exam for current  Announce 
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSAddExam")
            {
                string examIds = "";
                if (Request.Params["examIds"] != null)
                    examIds = Request.Params["examIds"].ToString();

                JSAddExam(examIds);
                return;
            }

            //Add Document for current  Announce 
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSAddDocument")
            {
                string documentIds = "";
                if (Request.Params["documentIds"] != null)
                    documentIds = Request.Params["documentIds"].ToString();

                JSAddDocument(documentIds);
                return;
            }

            //Get other Exams for current  Announce 
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetExamLightBoxContent")
            {
                JSGetExamLightBox();
                return;
            }

            //Get other Document for current  Announce 
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetDocumentLightBoxContent")
            {
                JSGetDocumentLightBox();
                return;
            }

            //Process the ajax request for properties of vacancy announce position(for displaying in the light box)
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetVacancyAnnouncePosition")
            {
                JSGetVacancyAnnouncePosition();
                return;
            }


            //Process the ajax request for saving vacancy announce position(for displaying in the light box)
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveVacancyAnnouncePosition")
            {
                JSSaveVacancyAnnouncePosition();
                return;
            }

            //Process ajax request for deleting of vacancy announce position
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteVacancyAnnouncePosition")
            {
                JSDeleteVacancyAnnouncePosition();
                return;
            }

            //Process ajax request for adding vacancy announce position manually
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveAddVacancyAnnouncePositionManually")
            {
                JSSaveAddVacancyAnnouncePositionManually();
                return;
            }


            //End NLP
            jsMilitaryUnitSelectorDiv.InnerHtml = @"<script src=""" + Page.ClientScript.GetWebResourceUrl(typeof(MilitaryUnitSelector.MilitaryUnitSelector), "MilitaryUnitSelector.MilitaryUnitSelector.js") + @""" type=""text/javascript""></script>";

            //Hilight the correct item in the menu
            if (VacancyAnnounceId == 0)
                HighlightMenuItems("VacancyAnnounces", "VacancyAnnounces_Add");
            else
                HighlightMenuItems("VacancyAnnounces");

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

            SetupDatePickers();
            SetupPageUI();
        }

        //Set the correct page header (i.e. either New or Edit)
        private void SetPageHeader()
        {
            string header = (VacancyAnnounceId > 0 ? "Редактиране на обявен конкурс" : "Въвеждане на нов обявен конкурс");

            lblHeaderTitle.InnerHtml = header;
            Page.Title = header;
        }

        //Load any drop-down on the screen
        private void LoadDropDowns()
        {
            PopulateVacancyAnnounceStatuses();
            PopulateEducation();
            PopulateClInformationAccLevelNATO();
            PopulateClInformationAccLevelBG();
            PopulateClInformationAccLevelEU();
            PopulateMilitaryRanks();
        }

        //Populate the modules drop-down
        private void PopulateVacancyAnnounceStatuses()
        {
            ddVacancyAnnounceStatuses.Items.Clear();

            List<VacancyAnnounceStatus> vacancyAnnounceStatuses = VacancyAnnounceStatusUtil.GetAllVacancyAnnounceStatuses(CurrentUser);

            foreach (VacancyAnnounceStatus vacancyAnnounceStatus in vacancyAnnounceStatuses)
            {
                ListItem li = new ListItem();
                li.Text = vacancyAnnounceStatus.VacAnnStatusName;
                li.Value = vacancyAnnounceStatus.VacancyAnnouncesStatusId.ToString();

                ddVacancyAnnounceStatuses.Items.Add(li);
            }
        }

        private void PopulateEducation()
        {
            ddEducation.Items.Clear();
            ddEducation.Items.Add(ListItems.GetOptionChooseOne());

            edit_ddEducation.Items.Clear();
            edit_ddEducation.Items.Add(ListItems.GetOptionChooseOne());
            
            List<Education> educations = EducationUtil.GetAllEducations(CurrentUser);

            foreach (Education education in educations)
            {
                ListItem li = new ListItem();
                li.Text = education.EducationName;
                li.Value = education.EducationId.ToString();

                ddEducation.Items.Add(li);
                edit_ddEducation.Items.Add(li);
            }

            ListItems.SetTextAsTooltip(ddEducation);
            ListItems.SetTextAsTooltip(edit_ddEducation);
        }

        private void PopulateClInformationAccLevelNATO()
        {
            ddClInformationAccLevelNATO.Items.Clear();
            ddClInformationAccLevelNATO.Items.Add(ListItems.GetOptionChooseOne());

            edit_ddClInformationAccLevelNATO.Items.Clear();
            edit_ddClInformationAccLevelNATO.Items.Add(ListItems.GetOptionChooseOne());

            List<ClInformation> clInformationNATO = ClInformationUtil.GetAllClInformationNATO(CurrentUser);

            foreach (ClInformation clInformation in clInformationNATO)
            {
                ListItem li = new ListItem();
                li.Text = clInformation.ClInfoName;
                li.Value = clInformation.ClInfoKey;

                ddClInformationAccLevelNATO.Items.Add(li);
                edit_ddClInformationAccLevelNATO.Items.Add(li);
            }
        }

        private void PopulateClInformationAccLevelBG()
        {
            ddClInformationAccLevelBG.Items.Clear();
            ddClInformationAccLevelBG.Items.Add(ListItems.GetOptionChooseOne());

            edit_ddClInformationAccLevelBG.Items.Clear();
            edit_ddClInformationAccLevelBG.Items.Add(ListItems.GetOptionChooseOne());
            
            List<ClInformation> clInformationBG = ClInformationUtil.GetAllClInformationBG(CurrentUser);

            foreach (ClInformation clInformation in clInformationBG)
            {
                ListItem li = new ListItem();
                li.Text = clInformation.ClInfoName;
                li.Value = clInformation.ClInfoKey;

                ddClInformationAccLevelBG.Items.Add(li);
                edit_ddClInformationAccLevelBG.Items.Add(li);
            }
        }

        private void PopulateClInformationAccLevelEU()
        {
            ddClInformationAccLevelEU.Items.Clear();
            ddClInformationAccLevelEU.Items.Add(ListItems.GetOptionChooseOne());

            edit_ddClInformationAccLevelEU.Items.Clear();
            edit_ddClInformationAccLevelEU.Items.Add(ListItems.GetOptionChooseOne());

            List<ClInformation> clInformationEU = ClInformationUtil.GetAllClInformationEU(CurrentUser);

            foreach (ClInformation clInformation in clInformationEU)
            {
                ListItem li = new ListItem();
                li.Text = clInformation.ClInfoName;
                li.Value = clInformation.ClInfoKey;

                ddClInformationAccLevelEU.Items.Add(li);
                edit_ddClInformationAccLevelEU.Items.Add(li);
            }
        }

        private void PopulateMilitaryRanks()
        {
            ddAvailableRanks.Items.Clear();

            edit_ddAvailableRanks.Items.Clear();
            
            List<MilitaryRank> militaryRanks = MilitaryRankUtil.GetAllMilitaryRanks(CurrentUser);

            foreach (MilitaryRank militaryRank in militaryRanks)
            {
                ListItem li = new ListItem();
                li.Text = militaryRank.LongName;
                li.Value = militaryRank.MilitaryRankId;

                ddAvailableRanks.Items.Add(li);
                edit_ddAvailableRanks.Items.Add(li);
            }
        }

        //Load the existing data in Edit mode
        private void LoadData()
        {
            //If Edit mode
            if (VacancyAnnounceId > 0)
            {
                VacancyAnnounce vacancyAnnounce = VacancyAnnounceUtil.GetVacancyAnnounce(VacancyAnnounceId, CurrentUser);

                txtOrderNum.Text = vacancyAnnounce.OrderNum;
                txtOrderDate.Text = CommonFunctions.FormatDate(vacancyAnnounce.OrderDate);
                txtEndDate.Text = CommonFunctions.FormatDate(vacancyAnnounce.EndDate);

                if (vacancyAnnounce.MaxPositions.HasValue)
                    txtMaxPositions.Text = vacancyAnnounce.MaxPositions.Value.ToString();

                if (vacancyAnnounce.VacancyAnnounceStatus != null &&
                   vacancyAnnounce.VacancyAnnounceStatus.VacancyAnnouncesStatusId > 0)
                {
                    ddVacancyAnnounceStatuses.SelectedValue = vacancyAnnounce.VacancyAnnounceStatus.VacancyAnnouncesStatusId.ToString();
                    hdnVacancyAnnounceStatusID.Value = vacancyAnnounce.VacancyAnnounceStatus.VacancyAnnouncesStatusId.ToString();
                }

                if (vacancyAnnounce.VacancyAnnounceType == 1)
                {
                    this.rbVacancyAnnounceTypeStaff.Checked = true;
                }
                else
                {
                    this.rbVacancyAnnounceTypeReserve.Checked = true;
                }

                hdnVacancyAnnounceType.Value = vacancyAnnounce.VacancyAnnounceType.ToString();

                divPositions.InnerHtml = GetTabPositions();

                rowTabs.Style.Add("display", "");
            }
            else
            {
                rowTabs.Style.Add("display", "none");
            }
        }

        //Collect the information from the page form
        //Store the data in a object in the memory and use that object when peforming DB operations
        private VacancyAnnounce CollectData()
        {
            VacancyAnnounce vacancyAnnounce = new VacancyAnnounce(CurrentUser);

            vacancyAnnounce.VacancyAnnounceId = VacancyAnnounceId;
            vacancyAnnounce.OrderNum = txtOrderNum.Text;
            if (txtOrderDate.Text != "")
                vacancyAnnounce.OrderDate = CommonFunctions.ParseDate(txtOrderDate.Text);
            if (txtEndDate.Text != "")
                vacancyAnnounce.EndDate = CommonFunctions.ParseDate(txtEndDate.Text);
            if (txtMaxPositions.Text != "")
                vacancyAnnounce.MaxPositions = int.Parse(txtMaxPositions.Text);

            if (ddVacancyAnnounceStatuses.SelectedValue != ListItems.GetOptionChooseOne().Value)
            {
                int vacancyAnnounceStatusId = int.Parse(ddVacancyAnnounceStatuses.SelectedValue);
                vacancyAnnounce.VacancyAnnounceStatus = VacancyAnnounceStatusUtil.GetVacancyAnnounceStatus(vacancyAnnounceStatusId, CurrentUser);
            }

            vacancyAnnounce.VacancyAnnounceType = (rbVacancyAnnounceTypeStaff.Checked ? 1 : 2);

            return vacancyAnnounce;
        }

        //Save the data
        private void SaveData()
        {
            //Get old status of a specific vaccancy announce
            string vacancyAnnounceStatusIDOld = hdnVacancyAnnounceStatusID.Value;

            //First collect the data from the page form
            VacancyAnnounce vacancyAnnounce = CollectData();

            List<VacancyAnnouncePosition> vacancyAnnouncePositions = null;
            if (vacancyAnnounce.VacancyAnnounceId > 0)
            {
                vacancyAnnouncePositions = VacancyAnnouncePositionUtil.GetAllVacancyAnnouncePositionsByVacancyAnnounce(vacancyAnnounce.VacancyAnnounceId, CurrentUser);
            }

            //Validate the VacanacyAnnounce
            string validationWarning = "";

            //First check if there are any position and the status is DOCUMENTS
            if (vacancyAnnounce.VacancyAnnounceStatus.VacAnnStatusKey == "DOCUMENTS" &&
                (vacancyAnnouncePositions == null || vacancyAnnouncePositions.Count == 0)
               )
            {
                validationWarning += (validationWarning == "" ? "" : "<br />") +
                  "Статусът не може да бъде '" + vacancyAnnounce.VacancyAnnounceStatus.VacAnnStatusName + @"', ако няма въведени длъжности";
            }
            //Next check if there are any positions without Responsible Military Unit and the status is DOCUMENTS
            else if (vacancyAnnounce.VacancyAnnounceStatus.VacAnnStatusKey == "DOCUMENTS" &&
                     vacancyAnnouncePositions != null)
            {
                var allPositionsHaveRespMilUnit = true;

                foreach (VacancyAnnouncePosition position in vacancyAnnouncePositions)
                {
                    if (!position.ResponsibleMilitaryUnitID.HasValue || position.ResponsibleMilitaryUnitID.Value <= 0)
                    {
                        allPositionsHaveRespMilUnit = false;
                    }
                }

                if (!allPositionsHaveRespMilUnit)
                {
                    validationWarning += (validationWarning == "" ? "" : "<br />") +
                        "За всички длъжности трябва да се избере '" + CommonFunctions.GetLabelText("MilitaryUnit") + " отговорна за конкурса'";
                }
            }

            if (vacancyAnnounce.VacancyAnnounceStatus.VacAnnStatusKey == "DOCUMENTS" &&
                vacancyAnnounce.ListExam.Count == 0)
            {
                validationWarning += (validationWarning == "" ? "" : "<br />") +
                   "Статусът не може да бъде '" + vacancyAnnounce.VacancyAnnounceStatus.VacAnnStatusName + @"', ако не е попълнен списъкът на изпитите";
            }

            if (vacancyAnnounce.VacancyAnnounceStatus.VacAnnStatusKey == "DOCUMENTS" &&
                vacancyAnnounce.ListDocument.Count == 0)
            {
                validationWarning += (validationWarning == "" ? "" : "<br />") +
                   "Статусът не може да бъде '" + vacancyAnnounce.VacancyAnnounceStatus.VacAnnStatusName + @"', ако не е попълнен списъкът на документите";
            }


            if (validationWarning != "")
            {
                this.lblMessage.CssClass = "ErrorText";
                this.lblMessage.Text = validationWarning;

                return;
            }


            //Track the changes into the Audit Trail 
            Change change = new Change(CurrentUser, "APPL_VacancyAnnounces");

            //If a successfull save operation then indicate this on the screen; Otherwise alert a warning
            if (VacancyAnnounceUtil.SaveVacancyAnnounce(vacancyAnnounce, CurrentUser, change))
            {
                //Write changes in DB

                change.WriteLog();

                if (VacancyAnnounceId == 0)
                {
                    SetLocationHash("AddEditVacancyAnnounce.aspx?VacancyAnnounceId=" + vacancyAnnounce.VacancyAnnounceId.ToString());
                }

                VacancyAnnounceId = vacancyAnnounce.VacancyAnnounceId;

                //Set new actual value form object in hidden field
                hdnVacancyAnnounceStatusID.Value = vacancyAnnounce.VacancyAnnounceStatus.VacancyAnnouncesStatusId.ToString();
                hdnVacancyAnnounceType.Value = vacancyAnnounce.VacancyAnnounceType.ToString();

                this.lblMessage.CssClass = "SuccessText";
                this.lblMessage.Text = "Записът е успешен";

                SetPageHeader();
                hdnSavedChanges.Value = "True";
                SetupPageUI();

                divPositions.InnerHtml = GetTabPositions();
                rowTabs.Style.Add("display", "");

                //If the new selected vaccancy announce status is different then redirect in order to refresh the UI items
                if (vacancyAnnounceStatusIDOld != hdnVacancyAnnounceStatusID.Value)
                {
                    Response.Redirect("AddEditVacancyAnnounce.aspx?VacancyAnnounceId=" + vacancyAnnounce.VacancyAnnounceId.ToString());
                }
            }
            else
            {
                this.lblMessage.CssClass = "ErrorText";
                this.lblMessage.Text = "Записът не е успешен";
            }
        }

        //Save the form data (first chech if it is valid)
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        //Navigate to the previous page
        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (FromHome != 1)
                Response.Redirect("~/ContentPages/ManageVacancyAnnounces.aspx");
            else
                Response.Redirect("~/ContentPages/Home.aspx");
        }

        // Setup any date picker controls on the page by setting the CSS of the target inputs
        // Note that the date picker CSS is common
        // This makes the calendar controls to appear on the page next to each input
        private void SetupDatePickers()
        {
            txtOrderDate.CssClass = "RequiredInputField " + CommonFunctions.DatePickerCSS();
            txtEndDate.CssClass = "RequiredInputField " + CommonFunctions.DatePickerCSS();
        }

        // Setup user interface elements according to rights of the user's role
        private void SetupPageUI()
        {
            if (VacancyAnnounceId == 0) // add mode of page
            {
                bool screenHidden = GetUIItemAccessLevel("APPL_VACANN") != UIAccessLevel.Enabled ||
                                    GetUIItemAccessLevel("APPL_VACANN_ADDVACANN") != UIAccessLevel.Enabled;

                bool screenDisabled = GetUIItemAccessLevel("APPL_VACANN") == UIAccessLevel.Disabled ||
                                      GetUIItemAccessLevel("APPL_VACANN_ADDVACANN") == UIAccessLevel.Disabled;

                if (screenHidden)
                    RedirectAccessDenied();

                if (screenDisabled)
                {
                    pageDisabledControls.Add(btnSave);
                }

                UIAccessLevel l;

                l = GetUIItemAccessLevel("APPL_VACANN_ADDVACANN_ORDERNUM");

                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblOrderNum);
                    pageDisabledControls.Add(txtOrderNum);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(lblOrderNum);
                    pageHiddenControls.Add(txtOrderNum);
                }

                l = this.GetUIItemAccessLevel("APPL_VACANN_ADDVACANN_ORDERDATE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblOrderDate);
                    pageDisabledControls.Add(txtOrderDate);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblOrderDate);
                    this.pageHiddenControls.Add(txtOrderDate);
                }

                l = this.GetUIItemAccessLevel("APPL_VACANN_ADDVACANN_ENDDATE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblEndDate);
                    pageDisabledControls.Add(txtEndDate);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblEndDate);
                    this.pageHiddenControls.Add(txtEndDate);
                }

                l = this.GetUIItemAccessLevel("APPL_VACANN_ADDVACANN_MAXPOS");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblMaxPositions);
                    pageDisabledControls.Add(txtMaxPositions);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblMaxPositions);
                    this.pageHiddenControls.Add(txtMaxPositions);
                }

                l = this.GetUIItemAccessLevel("APPL_VACANN_ADDVACANN_STATUS");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblVacancyAnnounceStatus);
                    pageDisabledControls.Add(ddVacancyAnnounceStatuses);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblVacancyAnnounceStatus);
                    this.pageHiddenControls.Add(ddVacancyAnnounceStatuses);
                }

                l = this.GetUIItemAccessLevel("APPL_VACANN_ADDVACANN_TYPE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    pageDisabledControls.Add(lblVacancyAnnounceType);
                    pageDisabledControls.Add(rbVacancyAnnounceTypeReserve);
                    pageDisabledControls.Add(rbVacancyAnnounceTypeStaff);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblVacancyAnnounceType);
                    this.pageHiddenControls.Add(rbVacancyAnnounceTypeReserve);
                    this.pageHiddenControls.Add(rbVacancyAnnounceTypeStaff);
                }
            }
            else // edit mode of page
            {
                bool screenHidden = GetUIItemAccessLevel("APPL_VACANN") == UIAccessLevel.Hidden ||
                                    GetUIItemAccessLevel("APPL_VACANN_EDITVACANN") == UIAccessLevel.Hidden;

                bool screenDisabled = GetUIItemAccessLevel("APPL_VACANN") == UIAccessLevel.Disabled ||
                                      GetUIItemAccessLevel("APPL_VACANN_EDITVACANN") == UIAccessLevel.Disabled;

                if (screenHidden)
                    RedirectAccessDenied();

                if (screenDisabled)
                {
                    pageDisabledControls.Add(btnSave);
                }

                UIAccessLevel l;

                l = GetUIItemAccessLevel("APPL_VACANN_ADDVACANN_ORDERNUM");

                if (l == UIAccessLevel.Disabled || screenDisabled || PageDisabled)
                {
                    pageDisabledControls.Add(lblOrderNum);
                    pageDisabledControls.Add(txtOrderNum);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    pageHiddenControls.Add(lblOrderNum);
                    pageHiddenControls.Add(txtOrderNum);
                }

                l = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_ORDERDATE");
                if (l == UIAccessLevel.Disabled || screenDisabled || PageDisabled)
                {
                    pageDisabledControls.Add(lblOrderDate);
                    pageDisabledControls.Add(txtOrderDate);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblOrderDate);
                    this.pageHiddenControls.Add(txtOrderDate);
                }

                l = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_ENDDATE");
                if (l == UIAccessLevel.Disabled || screenDisabled || PageDisabled)
                {
                    pageDisabledControls.Add(lblEndDate);
                    pageDisabledControls.Add(txtEndDate);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblEndDate);
                    this.pageHiddenControls.Add(txtEndDate);
                }

                l = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_MAXPOS");
                if (l == UIAccessLevel.Disabled || screenDisabled || PageDisabled)
                {
                    pageDisabledControls.Add(lblMaxPositions);
                    pageDisabledControls.Add(txtMaxPositions);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblMaxPositions);
                    this.pageHiddenControls.Add(txtMaxPositions);
                }

                int applicantsCount = VacancyAnnounceUtil.GetApplicantsCount(VacancyAnnounceId, CurrentUser);

                l = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_STATUS");
                if (l == UIAccessLevel.Disabled || screenDisabled || (applicantsCount > 0 && Config.GetWebSetting("CanEditStatusForVacancyAnnounceWithPositions") != "true"))
                {
                    pageDisabledControls.Add(lblVacancyAnnounceStatus);
                    pageDisabledControls.Add(ddVacancyAnnounceStatuses);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblVacancyAnnounceStatus);
                    this.pageHiddenControls.Add(ddVacancyAnnounceStatuses);
                }

                l = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_TYPE");
                if (l == UIAccessLevel.Disabled || screenDisabled || PageDisabled)
                {
                    pageDisabledControls.Add(lblVacancyAnnounceType);
                    pageDisabledControls.Add(rbVacancyAnnounceTypeReserve);
                    pageDisabledControls.Add(rbVacancyAnnounceTypeStaff);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblVacancyAnnounceType);
                    this.pageHiddenControls.Add(rbVacancyAnnounceTypeReserve);
                    this.pageHiddenControls.Add(rbVacancyAnnounceTypeStaff);
                }

                List<string> disabledClientControls = new List<string>();
                List<string> hiddenClientControls = new List<string>();

                l = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_POSITIONS_RESPMILITARYUNIT");
                if (l == UIAccessLevel.Disabled)
                {
                    disabledClientControls.Add("lblResponsibleMilitaryUnit");
                    this.pageDisabledControls.Add(msResponsibleMilitaryUnit);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblResponsibleMilitaryUnit");
                    this.pageHiddenControls.Add(msResponsibleMilitaryUnit);
                }

                l = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_POSITIONS_MANDATORYREQ");
                if (l == UIAccessLevel.Disabled)
                {
                    disabledClientControls.Add("lblMandatoryRequirements");
                    disabledClientControls.Add("txtMandatoryRequirements");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblMandatoryRequirements");
                    hiddenClientControls.Add("txtMandatoryRequirements");
                }

                l = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_POSITIONS_ADDITIONALREQ");
                if (l == UIAccessLevel.Disabled)
                {
                    disabledClientControls.Add("lblAdditionalRequirements");
                    disabledClientControls.Add("txtAdditionalRequirements");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAdditionalRequirements");
                    hiddenClientControls.Add("txtAdditionalRequirements");
                }

                l = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_POSITIONS_SPECIFICREQ");
                if (l == UIAccessLevel.Disabled)
                {
                    disabledClientControls.Add("lblSpecificRequirements");
                    disabledClientControls.Add("txtSpecificRequirements");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblSpecificRequirements");
                    hiddenClientControls.Add("txtSpecificRequirements");
                }

                l = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_POSITIONS_COMPPLACEANDDATE");
                if (l == UIAccessLevel.Disabled)
                {
                    disabledClientControls.Add("lblCompetitionPlaceAndDate");
                    disabledClientControls.Add("txtCompetitionPlaceAndDate");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblCompetitionPlaceAndDate");
                    hiddenClientControls.Add("txtCompetitionPlaceAndDate");
                }

                l = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_POSITIONS_CONTACTPHONE");
                if (l == UIAccessLevel.Disabled)
                {
                    disabledClientControls.Add("lblContactPhone");
                    disabledClientControls.Add("txtContactPhone");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblContactPhone");
                    hiddenClientControls.Add("txtContactPhone");
                }


                //Add postiions manually light-box
                l = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_POSITIONS_MILITARYUNIT");
                if (l == UIAccessLevel.Disabled)
                {
                    disabledClientControls.Add("lblMilitaryUnit");
                    this.pageDisabledControls.Add(msMilitaryUnit);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblMilitaryUnit");
                    this.pageHiddenControls.Add(msMilitaryUnit);
                }

                l = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_POSITIONS_POSITIONNAME");
                if (l == UIAccessLevel.Disabled)
                {
                    disabledClientControls.Add("lblPosition");
                    disabledClientControls.Add("txtPosition");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblPosition");
                    hiddenClientControls.Add("txtPosition");
                }

                l = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_POSITIONS_POSITIONCODE");
                if (l == UIAccessLevel.Disabled)
                {
                    disabledClientControls.Add("lblPositionCode");
                    disabledClientControls.Add("txtPositionCode");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblPositionCode");
                    hiddenClientControls.Add("txtPositionCode");
                }

                l = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_POSITIONS_EDUCATION");
                if (l == UIAccessLevel.Disabled)
                {
                    disabledClientControls.Add("lblEducation");
                    this.pageDisabledControls.Add(ddEducation);
                    this.pageDisabledControls.Add(edit_ddEducation);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEducation");
                    this.pageHiddenControls.Add(ddEducation);
                    this.pageHiddenControls.Add(edit_ddEducation);
                }

                l = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_POSITIONS_ACCLEVELNATO");
                if (l == UIAccessLevel.Disabled)
                {
                    disabledClientControls.Add("lblClInformationAccLevelNATO");
                    this.pageDisabledControls.Add(ddClInformationAccLevelNATO);
                    this.pageDisabledControls.Add(edit_ddClInformationAccLevelNATO);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblClInformationAccLevelNATO");
                    this.pageHiddenControls.Add(ddClInformationAccLevelNATO);
                    this.pageHiddenControls.Add(edit_ddClInformationAccLevelNATO);
                }

                l = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_POSITIONS_ACCLEVELBG");
                if (l == UIAccessLevel.Disabled)
                {
                    disabledClientControls.Add("lblClInformationAccLevelBG");
                    this.pageDisabledControls.Add(ddClInformationAccLevelBG);
                    this.pageDisabledControls.Add(edit_ddClInformationAccLevelBG);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblClInformationAccLevelBG");
                    this.pageHiddenControls.Add(ddClInformationAccLevelBG);
                    this.pageHiddenControls.Add(edit_ddClInformationAccLevelBG);
                }

                l = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_POSITIONS_ACCLEVELEU");
                if (l == UIAccessLevel.Disabled)
                {
                    disabledClientControls.Add("lblClInformationAccLevelEU");
                    this.pageDisabledControls.Add(ddClInformationAccLevelEU);
                    this.pageDisabledControls.Add(edit_ddClInformationAccLevelEU);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblClInformationAccLevelEU");
                    this.pageHiddenControls.Add(ddClInformationAccLevelEU);
                    this.pageHiddenControls.Add(edit_ddClInformationAccLevelEU);
                }

                l = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_POSITIONS_POSITIONSCNT");
                if (l == UIAccessLevel.Disabled)
                {
                    disabledClientControls.Add("lblPositionsCnt");
                    disabledClientControls.Add("txtPositionsCnt");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblPositionsCnt");
                    hiddenClientControls.Add("txtPositionsCnt");
                }

                l = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_POSITIONS_MILITARYRANK");
                if (l == UIAccessLevel.Disabled)
                {
                    disabledClientControls.Add("lblMilitaryRanks");
                    this.pageDisabledControls.Add(ddAvailableRanks);
                    hiddenClientControls.Add("btnSelectRanks");
                    hiddenClientControls.Add("btnRemoveRanks");
                    disabledClientControls.Add("lblSelectedRanks");
                    disabledClientControls.Add("ddSelectedRanks");

                    disabledClientControls.Add("edit_lblMilitaryRanks");
                    this.pageDisabledControls.Add(edit_ddAvailableRanks);
                    hiddenClientControls.Add("edit_btnSelectRanks");
                    hiddenClientControls.Add("edit_btnRemoveRanks");
                    disabledClientControls.Add("edit_lblSelectedRanks");
                    disabledClientControls.Add("edit_ddSelectedRanks");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblMilitaryRanks");
                    this.pageHiddenControls.Add(ddAvailableRanks);
                    hiddenClientControls.Add("btnSelectRanks");
                    hiddenClientControls.Add("btnRemoveRanks");
                    hiddenClientControls.Add("lblSelectedRanks");
                    hiddenClientControls.Add("ddSelectedRanks");

                    hiddenClientControls.Add("edit_lblMilitaryRanks");
                    this.pageHiddenControls.Add(edit_ddAvailableRanks);
                    hiddenClientControls.Add("edit_btnSelectRanks");
                    hiddenClientControls.Add("edit_btnRemoveRanks");
                    hiddenClientControls.Add("edit_lblSelectedRanks");
                    hiddenClientControls.Add("edit_ddSelectedRanks");
                }

                SetDisabledClientControls(disabledClientControls.ToArray());
                SetHiddenClientControls(hiddenClientControls.ToArray());
            }
        }

        //Load a particular tab's content
        private void JSLoadTab()
        {
            string html = "";

            string selectedTabId = Request.Form["SelectedTabId"];

            switch (selectedTabId)
            {
                case "btnTabPositions":
                    {
                        html = GetTabPositions();
                        break;
                    }
                case "btnTabExams":
                    {
                        html = GenerateTabExams();
                        break;
                    }
                case "btnTabDocuments":
                    {
                        html = GenerateTabDocuments();
                        break;
                    }
            }

            string response = "<TabHTML>" + AJAXTools.EncodeForXML(html) + "</TabHTML>";
            string stat = AJAXTools.OK;

            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        //Load the content of the Positions tab
        private string GetTabPositions()
        {
            string html = "";
            html += @"<div class='VacancyAnnounceTab'>";

            List<VacancyAnnouncePosition> positions = VacancyAnnouncePositionUtil.GetAllVacancyAnnouncePositionsByVacancyAnnounce(VacancyAnnounceId, CurrentUser);
            html += GenerateVacancyPositionsTable(positions);

            html += "</div>";

            return html;
        }

        //Load the content of the Exams tab
        private string GenerateTabExams()
        {
            List<Exam> listExams = ExamUtil.GetExamsForVacancyAnnounce(VacancyAnnounceId, CurrentUser);
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<div class='VacancyAnnounceTab'>");
            sb.Append("<span class='HeaderText' style='margin:10px 0px 20px 40px; '>Списък на изпитите, които всеки кандидат трябва да положи</span>");
            int counter = 1;

            if (listExams.Count > 0)
            {
                string headerStyle = "vertical-align: bottom;";
                sb.Append("<table class='CommonHeaderTable' style='margin:10px 0px 0px 40px; text-align: left;'>");
                sb.Append(" <thead> ");
                sb.Append(" <tr> ");
                sb.Append(" <th style='width: 20px;" + headerStyle + @"'>№</th>  ");
                sb.Append(" <th style='width: 500px;" + headerStyle + @"'>Изпит </th>  ");
                sb.Append(" <th style='width: 20px;" + headerStyle + @"'></th>  ");
                sb.Append(" </tr> ");
                sb.Append(" </thead> ");

                foreach (Exam exam in listExams)
                {
                    sb.Append("<tr style='vertical-align: middle; height:20px;' class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + "'>");
                    sb.Append("<td style='text-align:center'>" + counter + "</td>");
                    sb.Append("<td align=\"left\">" + exam.ExamName + "</td>");
                    //Set enable/disable logic to butons
                    string deleteHTML = "";
                    if (this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_EXAMS") == UIAccessLevel.Enabled &&
                        this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN") == UIAccessLevel.Enabled &&
                        this.GetUIItemAccessLevel("APPL_VACANN") == UIAccessLevel.Enabled &&
                        !PageDisabled)
                    {
                        deleteHTML = "<a href='javascript:RemoveExam(" + exam.ExamId + ", \"" + exam.ExamName + "\");'><img border='0' src='../Images/delete.png' alt='Изтриване' title='Изтриване' class='GridActionIcon' /></a>";
                    }
                    sb.Append("<td>&nbsp;" + deleteHTML + "</td>");
                    sb.Append("</tr>");
                    counter++;
                }
                sb.Append("</table></br>");
            }
            else
            {
                sb.Append("</br></br><span style='margin:25px 10px 10px 40px; '>Няма избрани изпити</span></br></br>");
            }

            if (this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_EXAMS") == UIAccessLevel.Enabled &&
                this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN") == UIAccessLevel.Enabled &&
                this.GetUIItemAccessLevel("APPL_VACANN") == UIAccessLevel.Enabled &&
                !PageDisabled)
            {
                sb.Append("<table><tr><td>");
                sb.Append("<div style='margin-left:40px;'>");
                sb.Append(@"<div  style='display: inline;' onclick='ShowExamLightTable();' class='Button'>
                        <i></i>
                        <div style='width: 70px; display: inline'>
                            Добавяне</div>
                        <b></b>
                    </div> ");
                sb.Append("</div>");
                sb.Append("</td></tr></table>");
            }

            sb.Append("</div>");
            return sb.ToString();
        }

        //Load the content of the Documents tab
        private string GenerateTabDocuments()
        {
            List<Document> listDocuments = DocumentUtil.GetDocumentsForVacancyAnnounce(VacancyAnnounceId, CurrentUser);
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<div class='VacancyAnnounceTab'>");
            sb.Append("<span class='HeaderText' style='margin:10px 0px 20px 40px; '>Списък на документите, които всеки кандидат трябва да представи</span>");
            int counter = 1;

            if (listDocuments.Count > 0)
            {
                string headerStyle = "vertical-align: bottom;";
                sb.Append("<table class='CommonHeaderTable' style='margin:10px 0px 0px 40px; text-align: left;'>");
                sb.Append(" <thead> ");
                sb.Append(" <tr> ");
                sb.Append(" <th style='width: 20px;" + headerStyle + @"'>№</th>  ");
                sb.Append(" <th style='width: 500px;" + headerStyle + @"'>Документ </th>  ");
                sb.Append(" <th style='width: 20px;" + headerStyle + @"'></th>  ");
                sb.Append(" </tr> ");
                sb.Append(" </thead> ");

                foreach (Document document in listDocuments)
                {
                    sb.Append("<tr style='vertical-align: middle; height:20px;' class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + "'>");
                    sb.Append("<td style='text-align:center'>" + counter + "</td>");
                    sb.Append("<td align=\"left\">" + document.DocumentName + "</td>");
                    //Set enable/disable logic to butons
                    string deleteHTML = "";
                    if (this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_DOCUMENTS") == UIAccessLevel.Enabled &&
                        this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN") == UIAccessLevel.Enabled &&
                        this.GetUIItemAccessLevel("APPL_VACANN") == UIAccessLevel.Enabled &&
                        !PageDisabled)
                    {
                        deleteHTML = "<a href='javascript:RemoveDocument(" + document.DocumentId + ", \"" + document.DocumentName + "\");'><img border='0' src='../Images/delete.png' alt='Изтриване' title='Изтриване' class='GridActionIcon' /></a>";
                    }
                    sb.Append("<td>&nbsp;" + deleteHTML + "</td>");
                    sb.Append("</tr>");
                    counter++;
                }
                sb.Append("</table></br>");
            }
            else
            {
                sb.Append("</br></br><span style='margin:25px 10px 10px 40px; '>Няма избрани документи</span></br></br>");
            }

            if (this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_DOCUMENTS") == UIAccessLevel.Enabled &&
                this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN") == UIAccessLevel.Enabled &&
                this.GetUIItemAccessLevel("APPL_VACANN") == UIAccessLevel.Enabled &&
                !PageDisabled)
            {
                sb.Append("<table><tr><td>");
                sb.Append("<div style='margin-left:40px;'>");
                sb.Append(@"<div  style='display: inline;' onclick='ShowDocumentLightTable();' class='Button'>
                        <i></i>
                        <div style='width: 70px; display: inline'>
                            Добавяне</div>
                        <b></b>
                    </div> ");
                sb.Append("</div>");
                sb.Append("</td></tr></table>");
            }

            sb.Append("</div>");
            return sb.ToString();
        }

        // Generate contents for positions tab
        private string GenerateVacancyPositionsTable(List<VacancyAnnouncePosition> positions)
        {
            bool IsMilitaryUnitHidden = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_POSITIONS_MILITARYUNIT") == UIAccessLevel.Hidden;
            bool IsPositionNameHidden = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_POSITIONS_POSITIONNAME") == UIAccessLevel.Hidden;
            bool IsPositionCodeHidden = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_POSITIONS_POSITIONCODE") == UIAccessLevel.Hidden;
            bool IsEducationHidden = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_POSITIONS_EDUCATION") == UIAccessLevel.Hidden;
            bool IsAccLevelNATOHidden = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_POSITIONS_ACCLEVELNATO") == UIAccessLevel.Hidden;
            bool IsAccLevelBGHidden = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_POSITIONS_ACCLEVELBG") == UIAccessLevel.Hidden;
            bool IsAccLevelEUHidden = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_POSITIONS_ACCLEVELEU") == UIAccessLevel.Hidden;
            bool IsMandatoryReqHidden = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_POSITIONS_MANDATORYREQ") == UIAccessLevel.Hidden;
            bool IsAdditionalReqHidden = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_POSITIONS_ADDITIONALREQ") == UIAccessLevel.Hidden;
            bool IsSpecifiqReqHidden = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_POSITIONS_SPECIFICREQ") == UIAccessLevel.Hidden;
            bool IsRespMilitaryUnitHidden = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_POSITIONS_RESPMILITARYUNIT") == UIAccessLevel.Hidden;
            bool IsCompPlaceAndDateHidden = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_POSITIONS_COMPPLACEANDDATE") == UIAccessLevel.Hidden;
            bool IsContactPhoneHidden = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_POSITIONS_CONTACTPHONE") == UIAccessLevel.Hidden;
            bool IsPositionCntHidden = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_POSITIONS_POSITIONSCNT") == UIAccessLevel.Hidden;
            bool positionsTableEditPermission = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_POSITIONS") == UIAccessLevel.Enabled &&
                                                this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN") == UIAccessLevel.Enabled &&
                                                this.GetUIItemAccessLevel("APPL_VACANN") == UIAccessLevel.Enabled;

            bool IsMilitaryRankHidden = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_POSITIONS_MILITARYRANK") == UIAccessLevel.Hidden;

            StringBuilder sb = new StringBuilder();
            sb.Append("<table id='vacancyAnnouncePositionsTable' name='vacancyAnnouncePositionsTable' class='CommonHeaderTable' style='text-align: left;' border='1px'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style=\"min-width: 30px;\"></th>");
            if (!IsMilitaryUnitHidden)
                sb.Append("<th style=\"min-width: 120px;\">" + CommonFunctions.GetLabelText("MilitaryUnit") + "</th>");
            if (!IsPositionNameHidden)
                sb.Append("<th style=\"min-width: 80px;\">Длъжност</th>");
            if (!IsPositionCodeHidden)
                sb.Append("<th style=\"min-width: 50px;\">Код на длъжността</th>");
            if (!IsMilitaryRankHidden)
                sb.Append("<th style=\"width: 30px;\">Звание</th>");
            if (!IsEducationHidden)
                sb.Append("<th style=\"min-width: 50px;\">Образование</th>");
            if (!IsAccLevelNATOHidden)
                sb.Append("<th style=\"width: 80px;\">Ниво на достъп до КИ (НАТО)</th>");
            if (!IsAccLevelBGHidden)
                sb.Append("<th style=\"width: 80px;\">Ниво на достъп до КИ (РБ)</th>");
            if (!IsAccLevelEUHidden)
                sb.Append("<th style=\"width: 80px;\">Ниво на достъп до КИ (ЕС)</th>");
            if (!IsMandatoryReqHidden)
                sb.Append("<th style=\"width: 50px;\">Задължителни изисквания</th>");
            if (!IsAdditionalReqHidden)
                sb.Append("<th style=\"width: 50px;\">Допълнителни изисквания</th>");
            if (!IsSpecifiqReqHidden)
                sb.Append("<th style=\"width: 50px;\">Специфични изисквания</th>");
            if (!IsRespMilitaryUnitHidden)
                sb.Append("<th style=\"width: 120px;\">" + CommonFunctions.GetLabelText("MilitaryUnit") + " отговорна за конкурса</th>");
            if (!IsCompPlaceAndDateHidden)
                sb.Append("<th style=\"min-width: 50px;\">Дата и място на конкурса</th>");
            if (!IsContactPhoneHidden)
                sb.Append("<th style=\"min-width: 50px;\">Тел. за контакт</th>");
            if (!IsPositionCntHidden)
                sb.Append("<th style=\"width: 30px;\">Брой за длъжността</th>");
            
            sb.Append("<th style=\"width: 40px;\"></th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            int counter = 1;

            if (positions.Count > 0)
            {
                sb.Append("<tbody>");
            }

            foreach (VacancyAnnouncePosition position in positions)
            {
                sb.Append("<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + "'>");
                sb.Append("<td>" + counter + "</td>");
                if (!IsMilitaryUnitHidden)
                    sb.Append("<td>" + (position.MilitaryUnit != null ? position.MilitaryUnit.DisplayTextForSelection : "") + "</td>");
                if (!IsPositionNameHidden)
                    sb.Append("<td>" + position.PositionName + "</td>");
                if (!IsPositionCodeHidden)
                    sb.Append("<td>" + position.PositionCode + "</td>");
                if (!IsMilitaryRankHidden)
                    sb.Append("<td>" + position.MilitaryRanksString + "</td>");
                if (!IsEducationHidden)
                    sb.Append("<td>" + (position.Education != null ? position.Education.EducationName : "") + "</td>");
                if (!IsAccLevelNATOHidden)
                    sb.Append("<td>" + position.ClInformationAccLevelNATO + "</td>");
                if (!IsAccLevelBGHidden)
                    sb.Append("<td>" + position.ClInformationAccLevelBG + "</td>");
                if (!IsAccLevelEUHidden)
                    sb.Append("<td>" + position.ClInformationAccLevelEU + "</td>");
                if (!IsMandatoryReqHidden)
                    sb.Append("<td>" + CommonFunctions.ReplaceNewLinesInString(position.MandatoryRequirements) + "</td>");
                if (!IsAdditionalReqHidden)
                    sb.Append("<td>" + CommonFunctions.ReplaceNewLinesInString(position.AdditionalRequirements) + "</td>");
                if (!IsSpecifiqReqHidden)
                    sb.Append("<td>" + CommonFunctions.ReplaceNewLinesInString(position.SpecificRequirements) + "</td>");
                if (!IsRespMilitaryUnitHidden)
                    sb.Append("<td>" + (position.ResponsibleMilitaryUnit != null ? position.ResponsibleMilitaryUnit.DisplayTextForSelection : "") + "</td>");
                if (!IsCompPlaceAndDateHidden)
                    sb.Append("<td>" + CommonFunctions.ReplaceNewLinesInString(position.CompetitionPlaceAndDate) + "</td>");
                if (!IsContactPhoneHidden)
                    sb.Append("<td>" + position.ContactPhone + "</td>");
                if (!IsPositionCntHidden)
                    sb.Append("<td>" + position.PositionsCnt.ToString() + "</td>");
               
                // add edit and delete icons(buttons), which calls javascript functionality for necessary actions
                if (positionsTableEditPermission && !PageDisabled)
                    sb.Append(@"<td nowrap='nowrap'><img border='0' src='../Images/edit.png' alt='покажи' title='покажи' onclick='javascript:ShowVacancyAnnouncePositionLightBox(" + position.VacancyAnnouncePositionID + @");' style='cursor: pointer;' />&nbsp;<img border='0' src='../Images/delete.png' alt='Изтриване' title='Изтриване' onclick='javascript:DeleteVacancyAnnouncePosition(" + position.VacancyAnnouncePositionID + @");' style='cursor: pointer;' /></td>");
                else
                    sb.Append(@"<td>&nbsp;</td>");

                sb.Append("</tr>");
                counter++;
            }

            if (positions.Count > 0)
            {
                sb.Append("</tbody>");
            }

            sb.Append("</table>");
            sb.Append("<div style='height: 10px;'></div>");
            if (positionsTableEditPermission && !PageDisabled)
            {
                sb.Append("<div id='btnAddVacancyAnnouncePosition' class='Button' onclick='ShowAddVacancyAnnouncePositionLightBox()'><i></i><div style='width:180px; padding-left:5px;'>Избери вакантна длъжност</div><b></b></div>");
                sb.Append("<div id='btnAddVacancyAnnouncePositionManually' class='Button' onclick='ShowAddVacancyAnnouncePositionManuallyLightBox()'><i></i><div style='width:110px; padding-left:5px;'>Добави длъжност</div><b></b></div>");
            }

            return sb.ToString();
        }

        // Return contents for add vacancy position light box by ajax request
        private void JSGetAddVacancyAnnouncePositionLightBox()
        {
            string response = "";
            response += GetAddVacancyAnnouncePositionLightBoxHtml();

            AJAX a = new AJAX(AJAXTools.EncodeForXML(response), this.Response);
            a.Write();
            Response.End();
        }

        // Generate contents for add vacancy position light box
        private string GetAddVacancyAnnouncePositionLightBoxHtml()
        {
            bool IsMilitaryUnitHidden = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_POSITIONS_MILITARYUNIT") == UIAccessLevel.Hidden;
            bool IsPositionNameHidden = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_POSITIONS_POSITIONNAME") == UIAccessLevel.Hidden;
            bool IsPositionCodeHidden = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_POSITIONS_POSITIONCODE") == UIAccessLevel.Hidden;
            bool IsPositionCntHidden = this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_POSITIONS_POSITIONSCNT") == UIAccessLevel.Hidden;

            string html = "";

            string htmlNoResults = "";

            MilitaryUnitSelector.MilitaryUnitSelector milUnitSelector = new MilitaryUnitSelector.MilitaryUnitSelector();
            milUnitSelector.Page = this;
            milUnitSelector.DataSourceWebPage = "DataSource.aspx";
            milUnitSelector.DataSourceKey = "MilitaryUnit";
            milUnitSelector.ResultMaxCount = 1000;
            milUnitSelector.Style.Add("width", "86%");
            milUnitSelector.DivMainCss = "isDivMainClass";
            milUnitSelector.DivListCss = "isDivListClass";
            milUnitSelector.DivFullListCss = "isDivFullListClass";
            milUnitSelector.DivFullListTitle = CommonFunctions.GetLabelText("MilitaryUnit");
            milUnitSelector.ID = "MilUnitSelector";

            List<VacantPosition> availablePositions = new List<VacantPosition>();
            string militaryUnitIds = "";
            string positionName = "";
            int pageIndex = 1; //Default
            int pageLength = 5;//int.Parse(Config.GetWebSetting("RowsPerPage"));
            int allRows = 0;
            int maxPage = 1;
            int orderBy = 1; //Default

            if (Request.Params["MilitaryUnitID"] != null && Request.Params["PositionName"] != null && Request.Params["PageIndex"] != null && Request.Params["OrderBy"] != null && Request.Params["MaxPage"] != null)
            {
                if (Request.Params["MilitaryUnitID"] != ListItems.GetOptionAll().Value)
                    militaryUnitIds = Request.Params["MilitaryUnitID"];

                if (!string.IsNullOrEmpty(militaryUnitIds))
                {
                    MilitaryUnit unit = MilitaryUnitUtil.GetMilitaryUnit(int.Parse(militaryUnitIds), CurrentUser);
                    if (unit != null)
                    {
                        milUnitSelector.SelectedValue = militaryUnitIds;
                        milUnitSelector.SelectedText = unit.DisplayTextForSelection;
                    }
                }

                positionName = (Request.Params["PositionName"] != null ? Request.Params["PositionName"] : "");

                pageIndex = int.Parse(Request.Params["PageIndex"]);
                orderBy = int.Parse(Request.Params["OrderBy"]);
            }

            allRows = VacantPositionUtil.GetAllVacantPositionsCount(militaryUnitIds, positionName, CurrentUser);
            maxPage = allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            int vacancyAnnounceType = 0;
            int.TryParse(Request.Params["VacancyAnnounceType"], out vacancyAnnounceType);

            availablePositions = VacantPositionUtil.GetAllVacantPositions(militaryUnitIds, positionName, vacancyAnnounceType, orderBy, pageIndex, pageLength, CurrentUser);

            StringWriter sw = new StringWriter();
            HtmlTextWriter tw = new HtmlTextWriter(sw);

            milUnitSelector.RenderControl(tw);

            // No data found
            if (availablePositions.Count == 0)
            {
                htmlNoResults = "Няма намерени резултати";
            }

            //Set filter section
            html += @"<center>
                        <table width='100%' style='border-collapse: collapse; vertical-align: middle; color: #0B449D;'>
                        <tr style='height: 30px'>
                            <td align='right'>
                                <span class='InputLabel' style='padding-left: 10px'>" + CommonFunctions.GetLabelText("MilitaryUnit") + @":</span>                                                                                                
                            </td>
                            <td align='left'>
                                " + sw.ToString() + @"
                            </td>
                            <td align='right'>
                                <span class='InputLabel' style='padding-left: 10px'>Длъжност:</span>
                            </td>
                            <td align='left'>
                                <input type='text' id='txtPositionName' class='InputField' value='" + positionName + @"'></input>
                            </td>
                            <td  align='center' style='padding-right: 30px;'>
                                <div id='btnSearch' class='Button' onclick='btnSearch_Click();'><i></i><div style='width:70px; padding-left:5px;'>Покажи</div><b></b></div>
                            </td>
                        </tr>
                      </table>";
            //Set pagination section
            // Refresh the paging image buttons
            string btnFirst = "src='../Images/ButtonFirst.png'";
            string btnPrev = "src='../Images/ButtonPrev.png'";
            string btnNext = "src='../Images/ButtonNext.png'";
            string btnLast = "src='../Images/ButtonLast.png'";

            if (pageIndex == 1)
            {
                btnFirst = "src='../Images/ButtonFirstDisabled.png' disabled='true'";
                btnPrev = "src='../Images/ButtonPrevDisabled.png' disabled='true'";
            }

            if (pageIndex == maxPage)
            {
                btnLast = "src='../Images/ButtonLastDisabled.png' disabled='true'";
                btnNext = "src='../Images/ButtonNextDisabled.png' disabled='true'";
            }

            // Set current page number
            string pageTablePagination = " | " + pageIndex + " от " + maxPage + " | ";

            // Setup the header of the grid
            html += @"<div style='min-height: 150px; margin-bottom: 10px; max-height: 400px; overflow-y: scroll;'>

                        <input type='hidden' id='hdnOrderBy' value='" + orderBy + @"' />
                        <input type='hidden' id='hdnPageIndex' value='" + pageIndex + @"' />
                        <input type='hidden' id='hdnPageMaxPage' value='" + maxPage + @"' />

                        <span class='HeaderText'>Избор на вакантни длъжности</span><br /><br /><br />

                        <div style='text-align: center;'>
                           <div style='display: inline; position: relative; top: -10px;'>
                              <img id='btnTableFirst' " + btnFirst + @" alt='Първа страница' title='Първа страница' class='PaginationButton' onclick=""BtnPagingClick('btnFirst');"" />
                              <img id='btnTablePrev' " + btnPrev + @" alt='Предишна страница' title='Предишна страница' class='PaginationButton' onclick=""BtnPagingClick('btnPrev');"" />
                              <span id='lblTablePagination' class='PaginationLabel'>" + pageTablePagination + @"</span>
                              <img id='btnTableNext' " + btnNext + @" alt='Следваща страница' title='Следваща страница' class='PaginationButton' onclick=""BtnPagingClick('btnNext');"" />
                              <img id='btnTableLast' " + btnLast + @" alt='Последна страница' title='Последна страница' class='PaginationButton' onclick=""BtnPagingClick('btnLast');"" />
                              
                              <span style='padding: 0 30px'>&nbsp;</span>
                              <span style='text-align: right;'>Отиди на страница</span>
                              <input id='txtTableGotoPage' class='InputField' type='text' style='width: 30px;' value='' />
                              <img id='btnTableGoto' src='../Images/ButtonGoto.png' alt='Отиди на страница' title='Отиди на страница' class='PaginationButton' onclick=""BtnPagingClick('btnPageGo');"" />
                           </div>
                        </div>

                        <table id='tblVacantPositions' class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            
                         </thead>";

            //Set Table Results
            string headerStyle = "vertical-align: bottom;";
            int orderCol = (orderBy > 100 ? orderBy - 100 : orderBy);
            string img = orderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
            string[] arrOrderCol = { "", "", "", "" };

            arrOrderCol[orderCol - 1] = @"<div style='position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

            //Setup the header of the grid
            html += @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>" +
    (!IsMilitaryUnitHidden ? @"<th style='width: 200px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>" + CommonFunctions.GetLabelText("MilitaryUnit") + arrOrderCol[0] + @"</th>" : "") +
    (!IsPositionNameHidden ? @"<th style='width: 300px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>Длъжност" + arrOrderCol[1] + @"</th>" : "") +
    (!IsPositionCodeHidden ? @"<th style='width: 80px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>Код на длъжността" + arrOrderCol[2] + @"</th>" : "") +
    (!IsPositionCntHidden ? @"<th style='width: 50px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(4);'>Брой позиции" + arrOrderCol[3] + @"</th>" : "") +
                             @"<th style='width: 50px;" + headerStyle + @"'>Избрани позиции</th>

 </tr></thead>";

            int counter = 1;

            //Iterate through all items and add them into the grid
            foreach (VacantPosition vacantPosition in availablePositions)
            {

                string cellStyle = "vertical-align: top;";

                html += @"<tr  class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"' >
                                 <td style='" + cellStyle + @"'>" + ((pageIndex - 1) * pageLength + counter).ToString() + @"</td>" +
      (!IsMilitaryUnitHidden ? @"<td style='" + cellStyle + @"'>" + (vacantPosition.MilitaryUnit != null ? vacantPosition.MilitaryUnit.DisplayTextForSelection : "") + @"</td>" : "") +
      (!IsPositionNameHidden ? @"<td style='" + cellStyle + @"'>" + vacantPosition.PositionName + @"</td>" : "") +
      (!IsPositionCodeHidden ? @"<td style='" + cellStyle + @"'><span id='positionCode" + counter + @"'>" + vacantPosition.PositionCode + @"</span></td>" : "") +
       (!IsPositionCntHidden ? @"<td style='" + cellStyle + @"'><span id='positionCnt" + counter + @"'>" + vacantPosition.PositionsCnt.ToString() + @"</span></td>" : "") +
                               @"<td style='" + cellStyle + @"'><input type='text' name='txtVacantPositions" + counter + @"' id='txtVacantPositions" + counter + @"' style='width: 50px;' /><input type='hidden' name='hdnMilUnitID" + counter + @"' id='hdnMilUnitID" + counter + @"' value='" + vacantPosition.MilitaryUnitID + @"' />
                                    <input type='hidden' id='hdnPositionName" + counter + @"' value='" + CommonFunctions.HtmlEncoding(vacantPosition.PositionName) + @"'>
                                    <input type='hidden' id='hdnClInformationAccLevelNATO" + counter + @"' value='" + CommonFunctions.HtmlEncoding(vacantPosition.ClInformationAccLevelNATO) + @"'>
                                    <input type='hidden' id='hdnClInformationAccLevelBG" + counter + @"' value='" + CommonFunctions.HtmlEncoding(vacantPosition.ClInformationAccLevelBG) + @"'>
                                    <input type='hidden' id='hdnClInformationAccLevelEU" + counter + @"' value='" + CommonFunctions.HtmlEncoding(vacantPosition.ClInformationAccLevelEU) + @"'>
                                 </td>
                              </tr>";

                counter++;
            }
            html += "</table>";

            html += "<input type='hidden' id='hdnVacantPositionsCounter' value='" + counter + "' />";


            html += @"<div style='height: 10px;'>
                </div>

                 <div style='text-align: center'>
                   <span id='lblAddVacancyAnnouncePositionMessage'>" + htmlNoResults + @"</span><br/>
                </div>
";

            html += @"  </div>
                        <div id='btnAddVacancyAnnouncePosition' runat='server' class='Button' onclick=""AddVacancyAnnouncePosition();""><i></i><div style='width:70px; padding-left:5px;'>Избери</div><b></b></div>
                        <div id='btnCloseTable' runat='server' class='Button' onclick=""HideAddVacancyAnnouncePositionLightBox();""><i></i><div style='width:70px; padding-left:5px;'>Затвори</div><b></b></div>
                      </center>";



            return html;
        }

        // Remove exam by ajax request
        private void JSRemoveExam(int examId)
        {
            if (this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_EXAMS") != UIAccessLevel.Enabled ||
                this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN") != UIAccessLevel.Enabled ||
                this.GetUIItemAccessLevel("APPL_VACANN") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            string response = "";

            Exam exam = ExamUtil.GetExam(examId, CurrentUser);
            VacancyAnnounce vacancyAnnounce = VacancyAnnounceUtil.GetVacancyAnnounce(VacancyAnnounceId, CurrentUser);

            //Track the changes into the Audit Trail 
            Change change = new Change(CurrentUser, "APPL_VacancyAnnounces");

            if (ExamUtil.DeleteExamForVacancyAnnounce(vacancyAnnounce, exam, CurrentUser, change))
            {
                response = GenerateTabExams();
                change.WriteLog();
            }
            else
            {
                response = "NO";
            }
            AJAX a = new AJAX(AJAXTools.EncodeForXML(response), this.Response);
            a.Write();
            Response.End();
        }

        // Remove document by ajax request
        private void JSRemoveDocument(int documentId)
        {
            if (this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_DOCUMENTS") != UIAccessLevel.Enabled ||
                this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN") != UIAccessLevel.Enabled ||
                this.GetUIItemAccessLevel("APPL_VACANN") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            string response = "";

            Document document = DocumentUtil.GetDocument(documentId, CurrentUser);
            VacancyAnnounce vacancyAnnounce = VacancyAnnounceUtil.GetVacancyAnnounce(VacancyAnnounceId, CurrentUser);

            //Track the changes into the Audit Trail 
            Change change = new Change(CurrentUser, "APPL_VacancyAnnounces");

            if (VacancyAnnounceDocumentUtil.DeleteDocumentForVacancyAnnounce(vacancyAnnounce, document, CurrentUser, change))
            {
                response = GenerateTabDocuments();
                change.WriteLog();
            }
            else
            {
                response = "NO";
            }
            AJAX a = new AJAX(AJAXTools.EncodeForXML(response), this.Response);
            a.Write();
            Response.End();
        }

        // Generate content for add exam light box
        private void JSGetExamLightBox()
        {
            List<Exam> listAllExam = ExamUtil.GetAllExams(CurrentUser);
            List<Exam> listCurrentExam = ExamUtil.GetExamsForVacancyAnnounce(VacancyAnnounceId, CurrentUser);

            string response = "";
            StringBuilder sb = new StringBuilder();

            if (listAllExam.Count == listCurrentExam.Count)
            {
                sb.Append("<center>");
                sb.Append("<table>");
                sb.Append("<tr><td><div style='min-height:50px;'/></td></tr>");
                sb.Append("<tr><td>");
                sb.Append("<span>Няма изпити за добавяне</span>");
                sb.Append("</td></tr>");
                sb.Append("<tr><td><div style='min-height:50px;'/></td></tr>");
                sb.Append("</td></tr>");
                sb.Append("</table>");
                sb.Append("<table><tr><td>");
                sb.Append(@"<div  style='display: inline; padding-left:30px' onclick='HideExamLightBox();' class='Button'>
                        <i></i>
                        <div style='width: 70px; display: inline'>
                            Отказ</div>
                        <b></b>
                    </div> ");
                sb.Append("</td></tr>");
                sb.Append("</table>");
                sb.Append("</center>");
            }
            else
            {
                sb.Append("<center>");
                sb.Append("<table>");
                // sb.Append("<tr><td><div style='min-height:20px;'/></td></tr>");
                sb.Append("<tr><td colspan='3'>");
                sb.Append("<span class='HeaderText'>Избиране на изпит</span>");
                sb.Append("</td></tr>");

                sb.Append("<tr style='height:10px;'></tr>");

                sb.Append("<tr>");
                sb.Append("<td><span class='SmallHeaderText'>Налични изпити</span></td>");

                sb.Append("<td>&nbsp;</td>");

                sb.Append("<td><span class='SmallHeaderText'>Избрани изпити</span></td>");
                sb.Append("</tr>");

                sb.Append("<tr>");
                sb.Append("<td>");
                sb.Append("<select id='availExamListItems' style='min-width:200px; min-height: 150px;' class='InputField' multiple='multiple' >");                

                foreach (Exam allExam in listAllExam)
                {
                    bool addItem = true;
                    foreach (Exam currentExam in listCurrentExam)
                    {
                        if (allExam.ExamId == currentExam.ExamId)
                        {
                            addItem = false;
                        }
                    }
                    if (addItem)
                    {
                        sb.Append("<option value='" + allExam.ExamId + "' title='" + allExam.ExamName + "'>" + allExam.ExamName + "</option>");
                    }
                }
                sb.Append("</select>");
                sb.Append("</td>");

                sb.Append("<td>");
                sb.Append(@"<input type='button' value='>>' title='Избор' onclick='ChooseAllExams();'
                                   style='width: 20px; display: block; cursor: pointer; background-color: #CCCCCC; height: 17px; padding: 2px; line-height: 9px; font-size: 9px;' 
                                   id='btnChooseAllExams' />");
                sb.Append("<div style='height: 5px;'></div>");
                sb.Append(@"<input type='button' value='>' title='Избор' onclick='ChooseSelectedExams();'
                                   style='width: 20px; display: block; cursor: pointer; background-color: #CCCCCC; height: 17px; padding: 2px; line-height: 9px; font-size: 9px;' 
                                   id='btnSelectMilitaryDepartment' />");            
                sb.Append("<div style='height: 5px;'></div>");
                sb.Append(@"<input type='button' value='<' title='Избор' onclick='RemoveSelectedExams();'
                                   style='width: 20px; display: block; cursor: pointer; background-color: #CCCCCC; height: 17px; padding: 2px; line-height: 9px; font-size: 9px;' 
                                   id='btnSelectMilitaryDepartment' />");
                sb.Append("<div style='height: 5px;'></div>");
                sb.Append(@"<input type='button' value='<<' title='Избор' onclick='RemoveAllExams();'
                                   style='width: 20px; display: block; cursor: pointer; background-color: #CCCCCC; height: 17px; padding: 2px; line-height: 9px; font-size: 9px;' 
                                   id='btnSelectMilitaryDepartment' />");
                sb.Append("</td>");

                sb.Append("<td>");
                sb.Append("<select id='choosenExamListItems' style='min-width:200px; min-height: 150px;' class='RequiredInputField' multiple='multiple' >");               
                sb.Append("</select>");
                sb.Append("</td>");

                sb.Append("</tr>");
                sb.Append("</table>");

                sb.Append("<table style='width: 200px;'>");
                sb.Append("<tr style='height:20px;'></tr>");
                sb.Append("<tr><td colspan='3'>");
                sb.Append(@"<div id='btnDivAddExam' style='display: inline;' disabled='true' onclick='AddExams();' class='DisabledButton'>
                        <i></i>
                        <div style='width: 70px; display: inline'>
                            Избор</div>
                        <b></b>
                    </div> ");

                sb.Append("<div style='min-Width:50px;'/>");
                sb.Append(@"<div  style='display: inline;' onclick='HideExamLightBox();' class='Button'>
                        <i></i>
                        <div style='width: 70px; display: inline'>
                            Отказ</div>
                        <b></b>
                    </div> ");
                sb.Append("</td></tr>");
                sb.Append("</table>");

                sb.Append("</center>");
            }

            response = sb.ToString();

            AJAX a = new AJAX(AJAXTools.EncodeForXML(response), this.Response);
            a.Write();
            Response.End();
        }

        // Generate content for add document light box
        private void JSGetDocumentLightBox()
        {
            List<Document> listAllDocument = DocumentUtil.GetAllDocuments(CurrentUser);
            List<Document> listCurrentDocument = DocumentUtil.GetDocumentsForVacancyAnnounce(VacancyAnnounceId, CurrentUser);

            string response = "";
            StringBuilder sb = new StringBuilder();
            if (listAllDocument.Count == listCurrentDocument.Count)
            {
                sb.Append("<center>");
                sb.Append("<table>");
                sb.Append("<tr style='height:20px;'></tr>");
                sb.Append("<tr><td colspan='3'>");
                sb.Append("<span>Няма документи за добавяне</span>");
                sb.Append("</td></tr>");
                sb.Append("<tr><td><div style='min-height:50px;'/></td></tr>");
                sb.Append("</td></tr>");
                sb.Append("</table>");
                sb.Append("<table><tr><td>");
                sb.Append(@"<div  style='display: inline; padding-left:30px' onclick='HideDocumentLightBox();' class='Button'>
                        <i></i>
                        <div style='width: 70px; display: inline'>
                            Отказ</div>
                        <b></b>
                    </div> ");
                sb.Append("</td></tr>");
                sb.Append("</table>");
                sb.Append("</center>");
            }
            else
            {
                sb.Append("<center>");
                sb.Append("<table>");
                // sb.Append("<tr><td><div style='min-height:20px;'/></td></tr>");
                sb.Append("<tr><td colspan='3'>");
                sb.Append("<span class='HeaderText'>Избиране на документ</span>");
                sb.Append("</td></tr>");

                sb.Append("<tr style='height:10px;'></tr>");

                sb.Append("<tr>");
                sb.Append("<td><span class='SmallHeaderText'>Налични документи</span></td>");

                sb.Append("<td>&nbsp;</td>");

                sb.Append("<td><span class='SmallHeaderText'>Избрани документи</span></td>");
                sb.Append("</tr>");

                sb.Append("<tr>");

                sb.Append("<td>");


                sb.Append("<select id='availDocumentListItems' style='min-width:250px; min-height: 150px;' class='InputField' multiple='multiple'>");               

                foreach (Document allDocument in listAllDocument)
                {
                    bool addItem = true;
                    foreach (Document currentDocument in listCurrentDocument)
                    {
                        if (allDocument.DocumentId == currentDocument.DocumentId)
                        {
                            addItem = false;
                        }
                    }
                    if (addItem)
                    {
                        sb.Append("<option value='" + allDocument.DocumentId + "' title='" + allDocument.DocumentName + "'>" + allDocument.DocumentName + "</option>");
                    }
                }
                sb.Append("</select>");
                sb.Append("</td>");

                sb.Append("<td>");
                sb.Append(@"<input type='button' value='>>' title='Избор' onclick='ChooseAllDocuments();'
                                   style='width: 20px; display: block; cursor: pointer; background-color: #CCCCCC; height: 17px; padding: 2px; line-height: 9px; font-size: 9px;' 
                                   id='btnChooseAllExams' />");
                sb.Append("<div style='height: 5px;'></div>");
                sb.Append(@"<input type='button' value='>' title='Избор' onclick='ChooseSelectedDocuments();'
                                   style='width: 20px; display: block; cursor: pointer; background-color: #CCCCCC; height: 17px; padding: 2px; line-height: 9px; font-size: 9px;' 
                                   id='btnSelectMilitaryDepartment' />");
                sb.Append("<div style='height: 5px;'></div>");
                sb.Append(@"<input type='button' value='<' title='Избор' onclick='RemoveSelectedDocuments();'
                                   style='width: 20px; display: block; cursor: pointer; background-color: #CCCCCC; height: 17px; padding: 2px; line-height: 9px; font-size: 9px;' 
                                   id='btnSelectMilitaryDepartment' />");
                sb.Append("<div style='height: 5px;'></div>");
                sb.Append(@"<input type='button' value='<<' title='Избор' onclick='RemoveAllDocuments();'
                                   style='width: 20px; display: block; cursor: pointer; background-color: #CCCCCC; height: 17px; padding: 2px; line-height: 9px; font-size: 9px;' 
                                   id='btnSelectMilitaryDepartment' />");
                sb.Append("</td>");

                sb.Append("<td>");
                sb.Append("<select  id='choosenDocumentListItems' style='min-width:250px; min-height: 150px;' class='RequiredInputField' multiple='multiple'>");              
                sb.Append("</select>");
                sb.Append("</td>");

                sb.Append("</tr>");
                sb.Append("</table>");

                sb.Append("<table style='width: 200px;'>");
                sb.Append("<tr style='height:20px;'></tr>");
                sb.Append("<tr><td>");
                sb.Append(@"<div id='btnDivAddDocument' style='display: inline;' disabled='true' onclick='AddDocuments();' class='DisabledButton'>
                        <i></i>
                        <div style='width: 70px; display: inline'>
                            Избор</div>
                        <b></b>
                    </div> ");
                sb.Append("<div style='min-Width:50px;'/>");
                sb.Append(@"<div  style='display: inline;' onclick='HideDocumentLightBox();' class='Button'>
                        <i></i>
                        <div style='width: 70px; display: inline'>
                            Отказ</div>
                        <b></b>
                    </div> ");
                sb.Append("</td></tr>");
                sb.Append("</table>");

                sb.Append("</center>");
            }

            response = sb.ToString();

            AJAX a = new AJAX(AJAXTools.EncodeForXML(response), this.Response);
            a.Write();
            Response.End();
        }

        // Add exam by ajax request
        private void JSAddExam(string examIds)
        {
            bool result = true;
            string response = "";

            int[] arrayExamsIds = CommonFunctions.GetIdsFromString(examIds);
            
            VacancyAnnounce vacancyAnnounce = VacancyAnnounceUtil.GetVacancyAnnounce(VacancyAnnounceId, CurrentUser);

            //Track the changes into the Audit Trail 
            Change change = new Change(CurrentUser, "APPL_VacancyAnnounces");

            foreach (int examId in arrayExamsIds)
            {
                Exam exam = ExamUtil.GetExam(examId, CurrentUser);
                               
                result &= ExamUtil.AddExamForVacancyAnnounce(vacancyAnnounce, exam, CurrentUser, change);            
            }

            if (result)
            {
                response = GenerateTabExams();
                change.WriteLog();
            }
            else
            {
                response = "NO";
            }

            AJAX a = new AJAX(AJAXTools.EncodeForXML(response), this.Response);
            a.Write();
            Response.End();
        }

        // Add document by ajax request
        private void JSAddDocument(string documentIds)
        {
            bool result = true;
            string response = "";

            int[] arrayDocumentIds = CommonFunctions.GetIdsFromString(documentIds);
            
            VacancyAnnounce vacancyAnnounce = VacancyAnnounceUtil.GetVacancyAnnounce(VacancyAnnounceId, CurrentUser);

            //Track the changes into the Audit Trail 
            Change change = new Change(CurrentUser, "APPL_VacancyAnnounces");

            foreach (int documentId in arrayDocumentIds)
            {
                Document document = DocumentUtil.GetDocument(documentId, CurrentUser);

                result &= VacancyAnnounceDocumentUtil.AddDocumentForVacancyAnnounce(vacancyAnnounce, document, CurrentUser, change);
            }

            if (result)
            {
                response = GenerateTabDocuments();
                change.WriteLog();
            }
            else
            {
                response = "NO";
            }

            AJAX a = new AJAX(AJAXTools.EncodeForXML(response), this.Response);
            a.Write();
            Response.End();
        }

        // Add vacancy announce position by ajax request
        private void JSAddVacancyAnnouncePositions()
        {
            string response = "";

            int vacancyAnnounceId = int.Parse(Request.Params["VacancyAnnounceID"]);
            int counter = int.Parse(Request.Params["Count"]);

            for (int i = 1; i < counter; i++)
            {
                if (!string.IsNullOrEmpty(Request.Params["Positions" + i]))
                {
                    int militaryUnitID = int.Parse(Request.Params["MilitaryUnitID" + i]);
                    string positionCode = Request.Params["positionCode" + i];
                    int positions = int.Parse(Request.Params["Positions" + i]);
                    string positionName = Request.Params["PositionName" + i];
                    string ClInformationAccLevelNATO = Request.Params["ClInformationAccLevelNATO" + i];
                    string ClInformationAccLevelBG = Request.Params["ClInformationAccLevelBG" + i];
                    string ClInformationAccLevelEU = Request.Params["ClInformationAccLevelEU" + i];

                    VacantPosition vacantPosition = VacantPositionUtil.GetVacantPosition(militaryUnitID, positionCode, 
                        positionName, ClInformationAccLevelNATO, ClInformationAccLevelBG, ClInformationAccLevelEU,
                        CurrentUser);

                    VacancyAnnouncePosition vacancyAnnouncePosition = new VacancyAnnouncePosition(CurrentUser);

                    vacancyAnnouncePosition.MilitaryUnitID = vacantPosition.MilitaryUnitID;
                    vacancyAnnouncePosition.PositionName = vacantPosition.PositionName;
                    vacancyAnnouncePosition.PositionCode = vacantPosition.PositionCode;
                    vacancyAnnouncePosition.EducationId = positionCode.Length >= 5 ? (int?)EducationUtil.GetEducationByCode(int.Parse(positionCode.Substring(4, 1)), CurrentUser).EducationId : null;
                    vacancyAnnouncePosition.ClInformationAccLevelBG = vacantPosition.ClInformationAccLevelBG;
                    vacancyAnnouncePosition.ClInformationAccLevelEU = vacantPosition.ClInformationAccLevelEU;
                    vacancyAnnouncePosition.ClInformationAccLevelNATO = vacantPosition.ClInformationAccLevelNATO;
                    vacancyAnnouncePosition.PositionsCnt = positions;
                    
                    //Track the changes into the Audit Trail 
                    Change change = new Change(CurrentUser, "APPL_VacancyAnnounces");

                    VacancyAnnouncePositionUtil.SaveVacancyAnnouncePosition(vacancyAnnounceId, vacancyAnnouncePosition, CurrentUser, change);

                    response = GetTabPositions();

                    change.WriteLog();

                }
            }

            response = GetTabPositions();

            AJAX a = new AJAX(AJAXTools.EncodeForXML(response), Response);
            a.Write();
            Response.End();
        }

        // Get ajax-requested data for Vacancy Announce Position(by VacancyAnnouncePositionID)
        private void JSGetVacancyAnnouncePosition()
        {
            int vacancyAnnouncePositionID = int.Parse(Request.Form["VacancyAnnouncePositionID"]);

            VacancyAnnouncePosition p = VacancyAnnouncePositionUtil.GetVacancyAnnouncePosition(vacancyAnnouncePositionID, CurrentUser);

            string militaryRanks = "";
            foreach (MilitaryRank rank in p.MilitaryRanks)
            {
                militaryRanks += "<Rank>" +
                                 "<Id>" + rank.MilitaryRankId.ToString() + "</Id>" +
                                 "<DisplayName>" + AJAXTools.EncodeForXML(rank.LongName) + "</DisplayName>" +
                                 "</Rank>";
            }

            string response = @"<response>
                                    <PositionName>" + AJAXTools.EncodeForXML(p.PositionName) + @"</PositionName>
                                    <MilitaryUnitID>" + AJAXTools.EncodeForXML(p.MilitaryUnit != null ? p.MilitaryUnitID.ToString() : ListItems.GetOptionChooseOne().Value) + @"</MilitaryUnitID>
                                    <MilitaryUnitName>" + AJAXTools.EncodeForXML(p.MilitaryUnit != null ? p.MilitaryUnit.DisplayTextForSelection : "") + @"</MilitaryUnitName>
                                    <PositionCode>" + AJAXTools.EncodeForXML(p.PositionCode) + @"</PositionCode>
                                    <ResponsibleMilitaryUnitID>" + (p.ResponsibleMilitaryUnitID.HasValue ? p.ResponsibleMilitaryUnitID.Value.ToString() : ListItems.GetOptionChooseOne().Value) + @"</ResponsibleMilitaryUnitID>
                                    <ResponsibleMilitaryUnitName>" + (p.ResponsibleMilitaryUnit != null ? p.ResponsibleMilitaryUnit.DisplayTextForSelection : "") + @"</ResponsibleMilitaryUnitName>
                                    <MandatoryRequirements>" + AJAXTools.EncodeForXML(p.MandatoryRequirements) + @"</MandatoryRequirements>
                                    <AdditionalRequirements>" + AJAXTools.EncodeForXML(p.AdditionalRequirements) + @"</AdditionalRequirements>
                                    <SpecificRequirements>" + AJAXTools.EncodeForXML(p.SpecificRequirements) + @"</SpecificRequirements>
                                    <CompetitionPlaceAndDate>" + AJAXTools.EncodeForXML(p.CompetitionPlaceAndDate) + @"</CompetitionPlaceAndDate> 
                                    <ContactPhone>" + AJAXTools.EncodeForXML(p.ContactPhone) + @"</ContactPhone>   

                                    <MilitaryRanks>" + militaryRanks + @"</MilitaryRanks>
                                    <EducationID>" + (p.Education != null ? p.EducationId.ToString() : ListItems.GetOptionChooseOne().Value) + @"</EducationID>  
                                    <ClInformationAccLevelNATO>" + AJAXTools.EncodeForXML(p.ClInformationAccLevelNATO) + @"</ClInformationAccLevelNATO>
                                    <ClInformationAccLevelBG>" + AJAXTools.EncodeForXML(p.ClInformationAccLevelBG) + @"</ClInformationAccLevelBG>
                                    <ClInformationAccLevelEU>" + AJAXTools.EncodeForXML(p.ClInformationAccLevelEU) + @"</ClInformationAccLevelEU>
                                    <PositionsCnt>" + p.PositionsCnt.ToString() + @"</PositionsCnt>                                
                                </response>";

            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();
        }

        // Save(update) vacancy announce position from ajax request
        private void JSSaveVacancyAnnouncePosition()
        {
            string resultMsg = "";

            int vacancyAnnounceID = int.Parse(Request.Form["VacancyAnnounceID"]);
            int vacancyAnnouncePositionID = int.Parse(Request.Form["VacancyAnnouncePositionID"]);
            int? responsibleMilitaryUnitID = !string.IsNullOrEmpty(Request.Form["ResponsibleMilitaryUnitID"]) ? (int?)int.Parse(Request.Form["ResponsibleMilitaryUnitID"]) : null;
            string mandatoryRequirements = Request.Form["MandatoryRequirements"];
            string additionalRequirements = Request.Form["AdditionalRequirements"];
            string specificRequirements = Request.Form["SpecificRequirements"];
            string competitionPlaceAndDate = Request.Form["CompetitionPlaceAndDate"];
            string contactPhone = Request.Form["ContactPhone"];

            int militaryUnitID = int.Parse(Request.Params["MilitaryUnitID"]);
            string positionName = Request.Params["PositionName"];
            string positionCode = Request.Params["PositionCode"];
            int? eductaionID = (Request.Params["EducationID"] != ListItems.GetOptionChooseOne().Value ? int.Parse(Request.Params["EducationID"]) : (int?)null);
            string clInformationAccLevelNATO_ID = (Request.Params["ClInformationAccLevelNATO_ID"] != ListItems.GetOptionChooseOne().Value ? Request.Params["ClInformationAccLevelNATO_ID"] : "");
            string clInformationAccLevelBG_ID = (Request.Params["ClInformationAccLevelBG_ID"] != ListItems.GetOptionChooseOne().Value ? Request.Params["ClInformationAccLevelBG_ID"] : "");
            string clInformationAccLevelEU_ID = (Request.Params["ClInformationAccLevelEU_ID"] != ListItems.GetOptionChooseOne().Value ? Request.Params["ClInformationAccLevelEU_ID"] : "");
            int positionsCnt = int.Parse(Request.Params["PositionsCnt"]);

            VacancyAnnouncePosition p = VacancyAnnouncePositionUtil.GetVacancyAnnouncePosition(vacancyAnnouncePositionID, CurrentUser);

            p.ResponsibleMilitaryUnitID = responsibleMilitaryUnitID;
            p.MandatoryRequirements = mandatoryRequirements;
            p.AdditionalRequirements = additionalRequirements;
            p.SpecificRequirements = specificRequirements;
            p.CompetitionPlaceAndDate = competitionPlaceAndDate;
            p.ContactPhone = contactPhone;

            p.MilitaryUnitID = militaryUnitID;
            p.PositionName = positionName;
            p.PositionCode = positionCode;
            p.EducationId = eductaionID;
            p.ClInformationAccLevelBG = !String.IsNullOrEmpty(clInformationAccLevelBG_ID) ? ClInformationUtil.GetClInformationBG(clInformationAccLevelBG_ID, CurrentUser).ClInfoName : "";
            p.ClInformationAccLevelEU = !String.IsNullOrEmpty(clInformationAccLevelEU_ID) ? ClInformationUtil.GetClInformationEU(clInformationAccLevelEU_ID, CurrentUser).ClInfoName : "";
            p.ClInformationAccLevelNATO = !String.IsNullOrEmpty(clInformationAccLevelNATO_ID) ? ClInformationUtil.GetClInformationNATO(clInformationAccLevelNATO_ID, CurrentUser).ClInfoName : "";
            p.PositionsCnt = positionsCnt;
            p.MilitaryRanks = new List<MilitaryRank>();
            int militaryRanksCnt = int.Parse(Request.Form["RanksCnt"]);

            for (int i = 1; i <= militaryRanksCnt; i++)
            {
                string milRankId = Request.Form["RankId_" + i.ToString()];
                string longName = Request.Form["RankDisplayText_" + i.ToString()];

                MilitaryRank rank = new MilitaryRank();

                rank.MilitaryRankId = milRankId;
                rank.LongName = longName;

                p.MilitaryRanks.Add(rank);
            }
            
            //Track the changes into the Audit Trail 
            Change change = new Change(CurrentUser, "APPL_VacancyAnnounces");

            if (VacancyAnnouncePositionUtil.SaveVacancyAnnouncePosition(vacancyAnnounceID, p, CurrentUser, change))
                resultMsg = GetTabPositions();
            else
                resultMsg = AJAXTools.ERROR;

            change.WriteLog();

            string response = @"<response>" + AJAXTools.EncodeForXML(resultMsg) + "</response>";
            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();
        }

        // Deletes vacancy announce position by ajax request
        private void JSDeleteVacancyAnnouncePosition()
        {

            if (this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_POSITIONS") != UIAccessLevel.Enabled ||
                this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN") != UIAccessLevel.Enabled ||
                this.GetUIItemAccessLevel("APPL_VACANN") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            int vacancyAnnouncePositionID = int.Parse(Request.Form["VacancyAnnouncePositionID"]);

            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "APPL_VacancyAnnounces");

                if (!VacancyAnnouncePositionUtil.DeleteVacancyAnnouncePosition(vacancyAnnouncePositionID, VacancyAnnounceId, CurrentUser, change))
                    throw new Exception("Database operation failed!");

                change.WriteLog();

                stat = AJAXTools.OK;
                response = "<response>" + AJAXTools.EncodeForXML(GetTabPositions()) + "</response>";
            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = "<response>" + AJAXTools.EncodeForXML(ex.Message) + "</response>";
            }


            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        // Add vacancy announce position manually by ajax request
        private void JSSaveAddVacancyAnnouncePositionManually()
        {
            string stat = "";
            string response = "";

            try
            {
                int vacancyAnnounceId = int.Parse(Request.Params["VacancyAnnounceID"]);

                int militaryUnitID = int.Parse(Request.Params["MilitaryUnitID"]);
                string positionName = Request.Params["PositionName"];
                string positionCode = Request.Params["PositionCode"];
                int? eductaionID = (Request.Params["EducationID"] != ListItems.GetOptionChooseOne().Value ? int.Parse(Request.Params["EducationID"]) : (int?)null);
                string clInformationAccLevelNATO_ID = (Request.Params["ClInformationAccLevelNATO_ID"] != ListItems.GetOptionChooseOne().Value ? Request.Params["ClInformationAccLevelNATO_ID"] : "");
                string clInformationAccLevelBG_ID = (Request.Params["ClInformationAccLevelBG_ID"] != ListItems.GetOptionChooseOne().Value ? Request.Params["ClInformationAccLevelBG_ID"] : "");
                string clInformationAccLevelEU_ID = (Request.Params["ClInformationAccLevelEU_ID"] != ListItems.GetOptionChooseOne().Value ? Request.Params["ClInformationAccLevelEU_ID"] : "");
                int positionsCnt = int.Parse(Request.Params["PositionsCnt"]);
                
                VacancyAnnouncePosition vacancyAnnouncePosition = new VacancyAnnouncePosition(CurrentUser);

                vacancyAnnouncePosition.MilitaryUnitID = militaryUnitID;
                vacancyAnnouncePosition.PositionName = positionName;
                vacancyAnnouncePosition.PositionCode = positionCode;
                vacancyAnnouncePosition.EducationId = eductaionID;
                vacancyAnnouncePosition.ClInformationAccLevelBG = !String.IsNullOrEmpty(clInformationAccLevelBG_ID) ? ClInformationUtil.GetClInformationBG(clInformationAccLevelBG_ID, CurrentUser).ClInfoName : "";
                vacancyAnnouncePosition.ClInformationAccLevelEU = !String.IsNullOrEmpty(clInformationAccLevelEU_ID) ? ClInformationUtil.GetClInformationEU(clInformationAccLevelEU_ID, CurrentUser).ClInfoName : "";
                vacancyAnnouncePosition.ClInformationAccLevelNATO = !String.IsNullOrEmpty(clInformationAccLevelNATO_ID) ? ClInformationUtil.GetClInformationNATO(clInformationAccLevelNATO_ID, CurrentUser).ClInfoName : "";
                vacancyAnnouncePosition.PositionsCnt = positionsCnt;
                vacancyAnnouncePosition.MilitaryRanks = new List<MilitaryRank>();

                int militaryRanksCnt = int.Parse(Request.Form["RanksCnt"]);

                for (int i = 1; i <= militaryRanksCnt; i++)
                {
                    string milRankId = Request.Form["RankId_" + i.ToString()];
                    string longName = Request.Form["RankDisplayText_" + i.ToString()];

                    MilitaryRank rank = new MilitaryRank();

                    rank.MilitaryRankId = milRankId;
                    rank.LongName = longName;

                    vacancyAnnouncePosition.MilitaryRanks.Add(rank);
                }

                //Track the changes into the Audit Trail 
                Change change = new Change(CurrentUser, "APPL_VacancyAnnounces");

                VacancyAnnouncePositionUtil.SaveVacancyAnnouncePositionManually(vacancyAnnounceId, vacancyAnnouncePosition, CurrentUser, change);

                change.WriteLog();

                response = GetTabPositions();

                stat = AJAXTools.OK;
                response = "<response>" + AJAXTools.EncodeForXML(response) + "</response>";
            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = "<response>" + AJAXTools.EncodeForXML(ex.Message) + "</response>";
            }

            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        public bool IsPositionsVisible()
        {
            return this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_POSITIONS") != UIAccessLevel.Hidden;
        }

        public bool IsExamsVisible()
        {
            return this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_EXAMS") != UIAccessLevel.Hidden;
        }

        public bool IsDocumentsVisible()
        {
            return this.GetUIItemAccessLevel("APPL_VACANN_EDITVACANN_DOCUMENTS") != UIAccessLevel.Hidden;
        }
    }
}
