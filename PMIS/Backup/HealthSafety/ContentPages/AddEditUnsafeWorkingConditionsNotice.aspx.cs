using System;

using PMIS.Common;
using PMIS.HealthSafety.Common;

namespace HealthSafety.ContentPages
{
    public partial class AddEditUnsafeWorkingConditionsNotice : HSPage
    {
        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "HS_UNSAFEWCONDNOTICE";
            }
        }

        //Getter/Setter of the ID of the displayed notice(0 - if new)
        private int UnsafeWConditionsNoticeId
        {
            get
            {
                int unsafeWConditionsNoticeId = 0;
                //gets UnsafeWConditioncNoticeID either from the hidden field on the page or from page url
                if (String.IsNullOrEmpty(this.hfUnsafeWConditionsNoticeID.Value)
                    || this.hfUnsafeWConditionsNoticeID.Value == "0")
                {
                    if (Request.Params["UnsafeWConditionsNoticeId"] != null)
                        Int32.TryParse(Request.Params["UnsafeWConditionsNoticeId"].ToString(), out unsafeWConditionsNoticeId);

                    //sets UnsafeWConditioncNoticeID into hidden field on the page in order to be accessible in javascript
                    this.hfUnsafeWConditionsNoticeID.Value = unsafeWConditionsNoticeId.ToString();
                }
                else
                {
                    Int32.TryParse(this.hfUnsafeWConditionsNoticeID.Value, out unsafeWConditionsNoticeId);
                }

                return unsafeWConditionsNoticeId;
            }
            set { this.hfUnsafeWConditionsNoticeID.Value = value.ToString(); }
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
                        Int32.TryParse(Request.Params["fh"].ToString(), out fh);

                    this.hfFromHome.Value = fh.ToString();
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

        private string MilitaryUnitLabel = CommonFunctions.GetLabelText("MilitaryUnit");

        protected void Page_Load(object sender, EventArgs e)
        {
            //Set Label Text
            SetLabelValueText();

            //Hilight the current page in the menu bar
            HighlightMenuItems("Accidents", "Accidents_AddUnsafeWorkingConditionsNotices");

            //Hide the navigation buttons
            HideNavigationControls(this.btnBack);

            jsMilitaryUnitSelectorDiv.InnerHtml = @"<script src=""" + Page.ClientScript.GetWebResourceUrl(typeof(MilitaryUnitSelector.MilitaryUnitSelector), "MilitaryUnitSelector.MilitaryUnitSelector.js") + @""" type=""text/javascript""></script>";

            // OBSOLETE
            // Prevent showing "Save changes" dialog box
            //LnkForceNoChangesCheck(this.btnSave);

            CommonFunctions.SetTextAreaEvents(this.txtDescOfUnsafeCondition, 4000);
            CommonFunctions.SetTextAreaEvents(this.txtListOfViolatedRequirements, 4000);
            CommonFunctions.SetTextAreaEvents(this.txtTempProcedures, 4000);
            CommonFunctions.SetTextAreaEvents(this.txtFinalProcedures, 4000);

            if (!IsPostBack)
            {
                //Pre-fill the date field with the today's date
                this.txtNoticeDate.Text = CommonFunctions.FormatDate(DateTime.Now);
                this.txtRiskReducingDueDate.Text = CommonFunctions.FormatDate(DateTime.Now);

                this.SetPageName(); // sets page titles according to mode of work(add or edit notice)
                this.LoadDropDowns(); //fills dropdowns on the page with values
                this.LoadData(); // fills the controls on the page, displaying properties of the notice

                Maintenance degreeOfDangerGTableMaintance = MaintenanceUtil.GetMaintenance(CurrentUser, ModuleKey + "_" + "DegreeOfDanger");
                if (GetUIItemAccessLevel(degreeOfDangerGTableMaintance.UIKeyMaintenance) != UIAccessLevel.Enabled)
                {
                    this.divDangerDegreesImg.Visible = false;
                }

                this.SetBtnPrintUnsafeConditionsNotice(); // Set visibility of the print button
            }

            this.SetupPageUI(); //setup user interface elements according to rights of the user's role
            this.SetupDatePicker(); //Setup any calendar control on the screen

            this.lblMessage.Text = ""; // clean message of notice operations
        }

        // Set page titles according to mode of work(add or edit notice)
        private void SetPageName()
        {
            if (this.UnsafeWConditionsNoticeId > 0)
            {
                this.lblHeaderTitle.InnerHtml = "Редактиране на сведение за заболявания и наранявания свързани с работата";
            }
            else
            {
                this.lblHeaderTitle.InnerHtml = "Добавяне на сведение за заболявания и наранявания свързани с работата";
            }

            Page.Title = lblHeaderTitle.InnerHtml;
        }

        // Set visibility of print button
        private void SetBtnPrintUnsafeConditionsNotice()
        {
            // if the notice is new and not saved yet, it is not allowed to print it
            if (this.UnsafeWConditionsNoticeId == 0)
            {
                this.btnPrintUnsafeConditionsNotice.Visible = false;
            }
            else
            {
                this.btnPrintUnsafeConditionsNotice.Visible = true;
            }
        }

        // Setup any date picker control on the page by setting the CSS of the target input
        // Note that the date picker CSS is common
        // This makes the calendar control to appears on the page next to each input
        private void SetupDatePicker()
        {
            this.txtNoticeDate.CssClass = CommonFunctions.DatePickerCSS();
            this.txtRiskReducingDueDate.CssClass = CommonFunctions.DatePickerCSS();
        }

        // Setup user interface elements according to rights of the user's role
        private void SetupPageUI()
        {
            if (this.UnsafeWConditionsNoticeId == 0) // add mode of page
            {
                bool screenHidden = (this.GetUIItemAccessLevel("HS_ADDUNSAFEWCONDNOTICE") == UIAccessLevel.Hidden)
                                        || (this.GetUIItemAccessLevel("HS_UNSAFEWCONDNOTICE") == UIAccessLevel.Hidden);

                bool screenDisabled = (this.GetUIItemAccessLevel("HS_ADDUNSAFEWCONDNOTICE") == UIAccessLevel.Disabled)
                                        || (this.GetUIItemAccessLevel("HS_UNSAFEWCONDNOTICE") == UIAccessLevel.Disabled);

                if (screenHidden)
                    RedirectAccessDenied();

                if (screenDisabled)
                {
                    this.pageHiddenControls.Add(this.btnSave);
                }

                UIAccessLevel l;

                l = this.GetUIItemAccessLevel("HS_ADDUNSAFEWCONDNOTICE_NOTICENUMBER");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblNoticeNumber);
                    this.pageDisabledControls.Add(this.txtNoticeNumber);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblNoticeNumber);
                    this.pageHiddenControls.Add(this.txtNoticeNumber);
                }

                l = this.GetUIItemAccessLevel("HS_ADDUNSAFEWCONDNOTICE_NOTICEDATE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblNoticeDate);
                    this.pageDisabledControls.Add(this.txtNoticeDate);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblNoticeDate);
                    this.pageHiddenControls.Add(this.txtNoticeDate);
                }

                l = this.GetUIItemAccessLevel("HS_ADDUNSAFEWCONDNOTICE_REPORTINGPERSON");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblReportingPersonName);
                    this.pageDisabledControls.Add(this.txtReportingPersonName);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblReportingPersonName);
                    this.pageHiddenControls.Add(this.txtReportingPersonName);
                }

                l = this.GetUIItemAccessLevel("HS_ADDUNSAFEWCONDNOTICE_MILUNIT");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblMilitaryUnit);
                    this.pageDisabledControls.Add(this.musMilitaryUnit);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblMilitaryUnit);
                    this.pageHiddenControls.Add(this.musMilitaryUnit);
                }

                l = this.GetUIItemAccessLevel("HS_ADDUNSAFEWCONDNOTICE_VIOLATIONPLACE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblViolationPlace);
                    this.pageDisabledControls.Add(this.txtViolationPlace);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblViolationPlace);
                    this.pageHiddenControls.Add(this.txtViolationPlace);
                }

                l = this.GetUIItemAccessLevel("HS_ADDUNSAFEWCONDNOTICE_RESPONSIBLEPERSON");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblResponsiblePerson);
                    this.pageDisabledControls.Add(this.txtResponsiblePerson);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblResponsiblePerson);
                    this.pageHiddenControls.Add(this.txtResponsiblePerson);
                }

                l = this.GetUIItemAccessLevel("HS_ADDUNSAFEWCONDNOTICE_DANGERDEGREE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblDangerDegree);
                    this.pageDisabledControls.Add(this.ddlDangerDegrees);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblDangerDegree);
                    this.pageHiddenControls.Add(this.ddlDangerDegrees);
                }

                l = this.GetUIItemAccessLevel("HS_ADDUNSAFEWCONDNOTICE_DESCOFUNSAFECOND");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblDescOfUnsafeCondition);
                    this.pageDisabledControls.Add(this.txtDescOfUnsafeCondition);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblDescOfUnsafeCondition);
                    this.pageHiddenControls.Add(this.txtDescOfUnsafeCondition);
                }

                l = this.GetUIItemAccessLevel("HS_ADDUNSAFEWCONDNOTICE_LISTOFVIOLATEDREQUIR");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblListOfViolatedRequirements);
                    this.pageDisabledControls.Add(this.txtListOfViolatedRequirements);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblListOfViolatedRequirements);
                    this.pageHiddenControls.Add(this.txtListOfViolatedRequirements);
                }

                l = this.GetUIItemAccessLevel("HS_ADDUNSAFEWCONDNOTICE_RISKREDUCINGDUEDATE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblRiskReducingDueDate);
                    this.pageDisabledControls.Add(this.txtRiskReducingDueDate);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblRiskReducingDueDate);
                    this.pageHiddenControls.Add(this.txtRiskReducingDueDate);
                }

                l = this.GetUIItemAccessLevel("HS_ADDUNSAFEWCONDNOTICE_TEMPPROCEDURES");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblTempProcedures);
                    this.pageDisabledControls.Add(this.txtTempProcedures);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblTempProcedures);
                    this.pageHiddenControls.Add(this.txtTempProcedures);
                }

                l = this.GetUIItemAccessLevel("HS_ADDUNSAFEWCONDNOTICE_FINALPROCEDURES");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblFinalProcedures);
                    this.pageDisabledControls.Add(this.txtFinalProcedures);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblFinalProcedures);
                    this.pageHiddenControls.Add(this.txtFinalProcedures);
                }

                l = this.GetUIItemAccessLevel("HS_ADDUNSAFEWCONDNOTICE_TEMPPROCESTRESULT");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblTempProceduresEstResult);
                    this.pageDisabledControls.Add(this.txtTempProceduresEstResult);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblTempProceduresEstResult);
                    this.pageHiddenControls.Add(this.txtTempProceduresEstResult);
                }

                l = this.GetUIItemAccessLevel("HS_ADDUNSAFEWCONDNOTICE_FINALPROCESTRESULT");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblFinalProceduresEstResult);
                    this.pageDisabledControls.Add(this.txtFinalProceduresEstResult);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblFinalProceduresEstResult);
                    this.pageHiddenControls.Add(this.txtFinalProceduresEstResult);
                }

                l = this.GetUIItemAccessLevel("HS_ADDUNSAFEWCONDNOTICE_ADDINFOCONTACTPERSON");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblAdditionalInfoContactPerson);
                    this.pageDisabledControls.Add(this.txtAdditionalInfoContactPerson);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblAdditionalInfoContactPerson);
                    this.pageHiddenControls.Add(this.txtAdditionalInfoContactPerson);
                }

                l = this.GetUIItemAccessLevel("HS_ADDUNSAFEWCONDNOTICE_ADDCONTACTINFO");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblAdditionalContactInfo);
                    this.pageDisabledControls.Add(this.txtAdditionalContactInfo);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblAdditionalContactInfo);
                    this.pageHiddenControls.Add(this.txtAdditionalContactInfo);
                }
            }
            else // edit mode of page
            {
                bool screenHidden = (this.GetUIItemAccessLevel("HS_EDITUNSAFEWCONDNOTICE") == UIAccessLevel.Hidden)
                                        || (this.GetUIItemAccessLevel("HS_UNSAFEWCONDNOTICE") == UIAccessLevel.Hidden);

                bool screenDisabled = (this.GetUIItemAccessLevel("HS_EDITUNSAFEWCONDNOTICE") == UIAccessLevel.Disabled)
                                        || (this.GetUIItemAccessLevel("HS_UNSAFEWCONDNOTICE") == UIAccessLevel.Disabled);

                if (screenHidden)
                    RedirectAccessDenied();

                if (screenDisabled)
                {
                    this.pageHiddenControls.Add(btnSave);
                }

                UIAccessLevel l;

                l = this.GetUIItemAccessLevel("HS_EDITUNSAFEWCONDNOTICE_NOTICENUMBER");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblNoticeNumber);
                    this.pageDisabledControls.Add(this.txtNoticeNumber);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblNoticeNumber);
                    this.pageHiddenControls.Add(this.txtNoticeNumber);
                }

                l = this.GetUIItemAccessLevel("HS_EDITUNSAFEWCONDNOTICE_NOTICEDATE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblNoticeDate);
                    this.pageDisabledControls.Add(this.txtNoticeDate);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblNoticeDate);
                    this.pageHiddenControls.Add(this.txtNoticeDate);
                }

                l = this.GetUIItemAccessLevel("HS_EDITUNSAFEWCONDNOTICE_REPORTINGPERSON");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblReportingPersonName);
                    this.pageDisabledControls.Add(this.txtReportingPersonName);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblReportingPersonName);
                    this.pageHiddenControls.Add(this.txtReportingPersonName);
                }

                l = this.GetUIItemAccessLevel("HS_EDITUNSAFEWCONDNOTICE_MILUNIT");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblMilitaryUnit);
                    this.pageDisabledControls.Add(this.musMilitaryUnit);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblMilitaryUnit);
                    this.pageHiddenControls.Add(this.musMilitaryUnit);
                }

                l = this.GetUIItemAccessLevel("HS_EDITUNSAFEWCONDNOTICE_VIOLATIONPLACE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblViolationPlace);
                    this.pageDisabledControls.Add(this.txtViolationPlace);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblViolationPlace);
                    this.pageHiddenControls.Add(this.txtViolationPlace);
                }

                l = this.GetUIItemAccessLevel("HS_EDITUNSAFEWCONDNOTICE_RESPONSIBLEPERSON");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblResponsiblePerson);
                    this.pageDisabledControls.Add(this.txtResponsiblePerson);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblResponsiblePerson);
                    this.pageHiddenControls.Add(this.txtResponsiblePerson);
                }

                l = this.GetUIItemAccessLevel("HS_EDITUNSAFEWCONDNOTICE_DANGERDEGREE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblDangerDegree);
                    this.pageDisabledControls.Add(this.ddlDangerDegrees);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblDangerDegree);
                    this.pageHiddenControls.Add(this.ddlDangerDegrees);
                }

                l = this.GetUIItemAccessLevel("HS_EDITUNSAFEWCONDNOTICE_DESCOFUNSAFECOND");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblDescOfUnsafeCondition);
                    this.pageDisabledControls.Add(this.txtDescOfUnsafeCondition);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblDescOfUnsafeCondition);
                    this.pageHiddenControls.Add(this.txtDescOfUnsafeCondition);
                }

                l = this.GetUIItemAccessLevel("HS_EDITUNSAFEWCONDNOTICE_LISTOFVIOLATEDREQUIR");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblListOfViolatedRequirements);
                    this.pageDisabledControls.Add(this.txtListOfViolatedRequirements);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblListOfViolatedRequirements);
                    this.pageHiddenControls.Add(this.txtListOfViolatedRequirements);
                }

                l = this.GetUIItemAccessLevel("HS_EDITUNSAFEWCONDNOTICE_RISKREDUCINGDUEDATE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblRiskReducingDueDate);
                    this.pageDisabledControls.Add(this.txtRiskReducingDueDate);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblRiskReducingDueDate);
                    this.pageHiddenControls.Add(this.txtRiskReducingDueDate);
                }

                l = this.GetUIItemAccessLevel("HS_EDITUNSAFEWCONDNOTICE_TEMPPROCEDURES");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblTempProcedures);
                    this.pageDisabledControls.Add(this.txtTempProcedures);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblTempProcedures);
                    this.pageHiddenControls.Add(this.txtTempProcedures);
                }

                l = this.GetUIItemAccessLevel("HS_EDITUNSAFEWCONDNOTICE_FINALPROCEDURES");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblFinalProcedures);
                    this.pageDisabledControls.Add(this.txtFinalProcedures);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblFinalProcedures);
                    this.pageHiddenControls.Add(this.txtFinalProcedures);
                }

                l = this.GetUIItemAccessLevel("HS_EDITUNSAFEWCONDNOTICE_TEMPPROCESTRESULT");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblTempProceduresEstResult);
                    this.pageDisabledControls.Add(this.txtTempProceduresEstResult);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblTempProceduresEstResult);
                    this.pageHiddenControls.Add(this.txtTempProceduresEstResult);
                }

                l = this.GetUIItemAccessLevel("HS_EDITUNSAFEWCONDNOTICE_FINALPROCESTRESULT");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblFinalProceduresEstResult);
                    this.pageDisabledControls.Add(this.txtFinalProceduresEstResult);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblFinalProceduresEstResult);
                    this.pageHiddenControls.Add(this.txtFinalProceduresEstResult);
                }

                l = this.GetUIItemAccessLevel("HS_EDITUNSAFEWCONDNOTICE_ADDINFOCONTACTPERSON");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblAdditionalInfoContactPerson);
                    this.pageDisabledControls.Add(this.txtAdditionalInfoContactPerson);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblAdditionalInfoContactPerson);
                    this.pageHiddenControls.Add(this.txtAdditionalInfoContactPerson);
                }

                l = this.GetUIItemAccessLevel("HS_EDITUNSAFEWCONDNOTICE_ADDCONTACTINFO");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblAdditionalContactInfo);
                    this.pageDisabledControls.Add(this.txtAdditionalContactInfo);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblAdditionalContactInfo);
                    this.pageHiddenControls.Add(this.txtAdditionalContactInfo);
                }
            }
        }

        // Populate all dropdowns on the screen
        private void LoadDropDowns()
        {
            this.FillDangerDegreesDDL();
        }

        // Populate this dropdown list separately according to logic flow
        private void FillDangerDegreesDDL()
        {
            this.ddlDangerDegrees.DataSource = GTableItemUtil.GetAllGTableItemsByTableName("DegreeOfDanger", ModuleKey, 1, 0, 0, CurrentUser);
            this.ddlDangerDegrees.DataTextField = "TableValue";
            this.ddlDangerDegrees.DataValueField = "TableKey";
            this.ddlDangerDegrees.DataBind();
            this.ddlDangerDegrees.Items.Insert(0, ListItems.GetOptionChooseOne());
        }

        // Populate notice's properties on the page
        private void LoadData()
        {
            if (this.UnsafeWConditionsNoticeId > 0)
            {
                UnsafeWorkingConditionsNotice unsafeWConditionsNotice = UnsafeWorkingConditionsNoticeUtil.GetUnsafeWorkingConditionsNotice(this.UnsafeWConditionsNoticeId, CurrentUser);
                if (unsafeWConditionsNotice != null)
                {
                    this.txtNoticeNumber.Text = unsafeWConditionsNotice.NoticeNumber;
                    this.txtNoticeDate.Text = CommonFunctions.FormatDate(unsafeWConditionsNotice.NoticeDate.ToString());
                    this.txtReportingPersonName.Text = unsafeWConditionsNotice.ReportingPersonName;

                    if (unsafeWConditionsNotice.MilitaryUnitId > 0)
                    {
                        this.musMilitaryUnit.SelectedValue = unsafeWConditionsNotice.MilitaryUnitId.ToString();
                        this.musMilitaryUnit.SelectedText = unsafeWConditionsNotice.MilitaryUnit.DisplayTextForSelection;
                    }
                    else
                    {
                        this.musMilitaryUnit.SelectedValue = "-1";
                        this.musMilitaryUnit.SelectedText = "";
                    }

                    this.txtViolationPlace.Text = unsafeWConditionsNotice.ViolationPlace;
                    this.txtResponsiblePerson.Text = unsafeWConditionsNotice.ResponsiblePersonName;
                    this.ddlDangerDegrees.SelectedValue = unsafeWConditionsNotice.DangerDegreeId.ToString();
                    this.txtDescOfUnsafeCondition.Text = unsafeWConditionsNotice.DescOfUnsafeCondition;
                    this.txtListOfViolatedRequirements.Text = unsafeWConditionsNotice.ListOfViolatedRequirements;
                    this.txtRiskReducingDueDate.Text = CommonFunctions.FormatDate(unsafeWConditionsNotice.RiskReducingDueDate);
                    this.txtTempProcedures.Text = unsafeWConditionsNotice.TempProcedures;
                    this.txtTempProceduresEstResult.Text = unsafeWConditionsNotice.TempProceduresEstResult;
                    this.txtFinalProcedures.Text = unsafeWConditionsNotice.FinalProcedures;
                    this.txtFinalProceduresEstResult.Text = unsafeWConditionsNotice.FinalProceduresEstResult;
                    this.txtAdditionalInfoContactPerson.Text = unsafeWConditionsNotice.AdditionalInfoContactPerson;
                    this.txtAdditionalContactInfo.Text = unsafeWConditionsNotice.AdditionalContactInfo;   
                }
            }
        }

        // Gathers notice's properties from the page controls
        private UnsafeWorkingConditionsNotice CollectData()
        {
            UnsafeWorkingConditionsNotice unsafeWConditionsNotice = new UnsafeWorkingConditionsNotice(CurrentUser);
            unsafeWConditionsNotice.UnsafeWConditionsNoticeId = this.UnsafeWConditionsNoticeId;
            unsafeWConditionsNotice.NoticeNumber = this.txtNoticeNumber.Text.Trim();
            unsafeWConditionsNotice.NoticeDate = CommonFunctions.ParseDate(this.txtNoticeDate.Text.Trim()).Value;
            unsafeWConditionsNotice.ReportingPersonName = this.txtReportingPersonName.Text.Trim();
            unsafeWConditionsNotice.MilitaryUnitId = (this.musMilitaryUnit.SelectedValue != ListItems.GetOptionChooseOne().Value) ? Convert.ToInt32(this.musMilitaryUnit.SelectedValue) : (int?)null;
            unsafeWConditionsNotice.ViolationPlace = this.txtViolationPlace.Text.Trim();
            unsafeWConditionsNotice.ResponsiblePersonName = this.txtResponsiblePerson.Text.Trim();
            unsafeWConditionsNotice.DangerDegreeId = (this.ddlDangerDegrees.SelectedValue != ListItems.GetOptionChooseOne().Value) ? Convert.ToInt32(this.ddlDangerDegrees.SelectedValue) : (int?)null;
            unsafeWConditionsNotice.DescOfUnsafeCondition = this.txtDescOfUnsafeCondition.Text.Trim();
            unsafeWConditionsNotice.ListOfViolatedRequirements = this.txtListOfViolatedRequirements.Text.Trim();
            unsafeWConditionsNotice.RiskReducingDueDate = (this.txtRiskReducingDueDate.Text.Trim() != "") ? CommonFunctions.ParseDate(this.txtRiskReducingDueDate.Text.Trim()) : (DateTime?)null;
            unsafeWConditionsNotice.TempProcedures = this.txtTempProcedures.Text.Trim();
            unsafeWConditionsNotice.TempProceduresEstResult = this.txtTempProceduresEstResult.Text.Trim();
            unsafeWConditionsNotice.FinalProcedures = this.txtFinalProcedures.Text.Trim();
            unsafeWConditionsNotice.FinalProceduresEstResult = this.txtFinalProceduresEstResult.Text.Trim();
            unsafeWConditionsNotice.AdditionalInfoContactPerson = this.txtAdditionalInfoContactPerson.Text.Trim();
            unsafeWConditionsNotice.AdditionalContactInfo = this.txtAdditionalContactInfo.Text.Trim();

            return unsafeWConditionsNotice; // return loaded with values notice object
        }

        // Saves notice
        private void SaveData()
        {
            UnsafeWorkingConditionsNotice unsafeWConditionsNotice = this.CollectData(); // gathers notice properties from page controls

            //Track the changes into the Audit Trail 
            Change change = new Change(CurrentUser, "HS_UnsafeWCondNotices");

            if (UnsafeWorkingConditionsNoticeUtil.SaveUnsafeWorkingConditionsNotice(unsafeWConditionsNotice, CurrentUser, change))
            {
                if (UnsafeWConditionsNoticeId == 0)
                {
                    SetLocationHash("AddEditUnsafeWorkingConditionsNotice.aspx?UnsafeWConditionsNoticeId=" + unsafeWConditionsNotice.UnsafeWConditionsNoticeId.ToString());
                }

                this.UnsafeWConditionsNoticeId = unsafeWConditionsNotice.UnsafeWConditionsNoticeId;

                this.lblMessage.CssClass = "SuccessText";
                this.lblMessage.Text = "Успешен запис на сведението!";

                this.hdnSavedChanges.Value = "True";
                this.SetupPageUI(); //setup user interface elements according to rights of the user's role

                this.SetBtnPrintUnsafeConditionsNotice();
            }
            else
            {
                this.lblMessage.CssClass = "ErrorText";
                this.lblMessage.Text = "Неуспешен запис на сведението!";
            }

            change.WriteLog();
        }

        // Saves notice, but only if data valid
        protected void btnSave_Click(object sender, EventArgs e)
        {
            this.SaveData();
            this.SetPageName(); // sets page titles according to mode of work(add or edit notice)
        }

        //Navigate back to the ManageUnsafeWorkingConditionsNotice screen
        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (this.FromHome != 1)
                Response.Redirect("~/ContentPages/ManageUnsafeWorkingConditionsNotices.aspx");
            else
                RedirectAccessDenied();
        }

        protected void btnRefreshDegreeOfDanger_Click(object sender, EventArgs e)
        {
            this.FillDangerDegreesDDL();

            UnsafeWorkingConditionsNotice unsafeWConditionsNotice = UnsafeWorkingConditionsNoticeUtil.GetUnsafeWorkingConditionsNotice(this.UnsafeWConditionsNoticeId, CurrentUser);

            if (unsafeWConditionsNotice != null && this.ddlDangerDegrees.Items.Count > 0)
            {
                this.ddlDangerDegrees.SelectedValue = unsafeWConditionsNotice.DangerDegreeId.ToString();
            }
        }
        //Set label text
        private void SetLabelValueText()
        {
            this.lblMilitaryUnit.Text = this.MilitaryUnitLabel + ":";
        }
    }
}
