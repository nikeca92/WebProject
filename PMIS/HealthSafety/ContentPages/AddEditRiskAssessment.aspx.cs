using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using PMIS.Common;
using PMIS.HealthSafety.Common;

namespace PMIS.HealthSafety.ContentPages
{
    public partial class AddEditRiskAssessment : HSPage
    {
        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "HS_RISKASSESSMENTS";
            }
        }

        //Getter/Setter of the ID of the displayed risk assessment(0 - if new)
        private int RiskAssessmentId
        {
            get
            {
                int riskAssessmentId = 0;
                //gets RiskAssessmentID either from the hidden field on the page or from page url
                if (String.IsNullOrEmpty(this.hfRiskAssessmentID.Value)
                    || this.hfRiskAssessmentID.Value == "0")
                {
                    if (Request.Params["RiskAssessmentId"] != null)
                        Int32.TryParse(Request.Params["RiskAssessmentId"].ToString(), out riskAssessmentId);

                    //sets RiskAssessmentID into hidden field on the page in order to be accessible in javascript
                    this.hfRiskAssessmentID.Value = riskAssessmentId.ToString();
                }
                else
                {
                    Int32.TryParse(this.hfRiskAssessmentID.Value, out riskAssessmentId);
                }

                return riskAssessmentId;
            }
            set { this.hfRiskAssessmentID.Value = value.ToString(); }
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
            HighlightMenuItems("RiskAssessments", "RiskAssessments_Add");

            //Hide the navigation buttons
            HideNavigationControls(btnBack);

            // OBSOLETE
            // Prevent showing "Save changes" dialog box
            //LnkForceNoChangesCheck(btnSave);
            //LnkForceNoChangesCheck(btnNewRecommendation);

            //Process the ajax request for saving recommendations
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveRecommendation")
            {
                this.JSSaveRecommendation();
                return;
            }

            //Process the ajax request for properties of recommendation(for displaying in the light box)
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetRecommendation")
            {
                this.JSGetRecommendation();
                return;
            }

            //Process ajax request for deleting of recommendation
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteRecommendation")
            {
                this.JSDeleteRecommendation();
                return;
            }

            CommonFunctions.SetTextAreaEvents(this.txtComments, 4000);
            CommonFunctions.SetTextAreaEvents(this.txtRecommendationText, 4000);

            jsMilitaryUnitSelectorDiv.InnerHtml = @"<script src=""" + Page.ClientScript.GetWebResourceUrl(typeof(MilitaryUnitSelector.MilitaryUnitSelector), "MilitaryUnitSelector.MilitaryUnitSelector.js") + @""" type=""text/javascript""></script>";

            if (!IsPostBack)
            {
                //Pre-fill the date field with the today's date
                this.txtPreparationDate.Text = CommonFunctions.FormatDate(DateTime.Now);

                this.SetPageName(); // sets page titles according to mode of work(add or edit assessment)
                this.LoadDropDowns(); //fills dropdowns on the page with values
                this.LoadData(); // fills the controls on the page, displaying properties of the assessment
                this.LoadRecommendationsTable(); // loads html table in the page with recommendations to this assessment
                this.SetBtnPrintRiskAssessment(); // Set visibility of the print button
            }

            this.SetupPageUI(); //setup user interface elements according to rights of the user's role
            this.SetBtnNewRecommendation(); //enable or disable button for adding new recommendations, according to mode of work(add or edir assessment)
            this.SetupDatePicker(); //Setup any calendar control on the screen

            this.lblMessage.Text = ""; // clean message of assessment operations
            this.SetRecommendationMessage(); //display message from ajax operations on recommendation, if exist 
        }

        // Set page titles according to mode of work(add or edit assessment)
        private void SetPageName()
        {
            if (this.RiskAssessmentId > 0)
            {
                this.lblHeaderTitle.InnerHtml = "Редактиране на оценка на риска";
            }
            else
            {
                this.lblHeaderTitle.InnerHtml = "Добавяне на оценка на риска";
            }

            Page.Title = lblHeaderTitle.InnerHtml;
        }

        // Set visibility of print button
        private void SetBtnPrintRiskAssessment()
        {
            // if the assessment is new and not saved yet, it is not allowed to print it
            if (this.RiskAssessmentId == 0)
            {
                this.btnPrintRiskAssessment.Visible = false;
            }
            else
            {
                this.btnPrintRiskAssessment.Visible = true;
            }
        }

        // Enable or disable button for adding new recommendations, according to mode of work(add or edir assessment)
        private void SetBtnNewRecommendation()
        {
            // if the assessment is new and not saved yet, it is not allowed to add new recommendations to assessment
            if (this.RiskAssessmentId == 0)
            {
                DisableButton(this.btnNewRecommendation);
            }
            else
            {
                EnableButton(this.btnNewRecommendation);
            }
        }

        // Setup any date picker control on the page by setting the CSS of the target input
        // Note that the date picker CSS is common
        // This makes the calendar control to appears on the page next to each input
        private void SetupDatePicker()
        {
            this.txtPreparationDate.CssClass = "RequiredInputField " + CommonFunctions.DatePickerCSS();
            this.txtDueDate.CssClass = "RequiredInputField " + CommonFunctions.DatePickerCSS();
            this.txtExecutionDate.CssClass = CommonFunctions.DatePickerCSS();
        }

        // Setup user interface elements according to rights of the user's role
        private void SetupPageUI()
        {
            if (this.RiskAssessmentId == 0) // add mode of page
            {
                bool screenHidden = (this.GetUIItemAccessLevel("HS_ADDRISKASSESS") == UIAccessLevel.Hidden)
                                        || (this.GetUIItemAccessLevel("HS_RISKASSESSMENTS") == UIAccessLevel.Hidden);

                bool screenDisabled = (this.GetUIItemAccessLevel("HS_ADDRISKASSESS") == UIAccessLevel.Disabled)
                                        || (this.GetUIItemAccessLevel("HS_RISKASSESSMENTS") == UIAccessLevel.Disabled);

                if (screenHidden)
                    RedirectAccessDenied();

                if (screenDisabled)
                {
                    this.pageHiddenControls.Add(this.btnSave);
                    this.pageHiddenControls.Add(this.btnNewRecommendation);
                }

                UIAccessLevel l;

                l = this.GetUIItemAccessLevel("HS_ADDRISKASSESS_PREPDATE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblPrepariationDate);
                    this.pageDisabledControls.Add(this.txtPreparationDate);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblPrepariationDate);
                    this.pageHiddenControls.Add(this.txtPreparationDate);
                }

                l = this.GetUIItemAccessLevel("HS_ADDRISKASSESS_REGNUM");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblRegNumber);
                    this.pageDisabledControls.Add(this.txtRegNumber);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblRegNumber);
                    this.pageHiddenControls.Add(this.txtRegNumber);
                }

                l = this.GetUIItemAccessLevel("HS_ADDRISKASSESS_MILTFORCETYPE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblMilitForceTypeType);
                    this.pageDisabledControls.Add(this.ddlMilitaryForceTypes);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblMilitForceTypeType);
                    this.pageHiddenControls.Add(this.ddlMilitaryForceTypes);
                }

                l = this.GetUIItemAccessLevel("HS_ADDRISKASSESS_MILUNIT");
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

                l = this.GetUIItemAccessLevel("HS_ADDRISKASSESS_COMMENT");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblComments);
                    this.pageDisabledControls.Add(this.txtComments);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblComments);
                    this.pageHiddenControls.Add(this.txtComments);
                }
            }
            else // edit mode of page
            {
                bool screenHidden = (this.GetUIItemAccessLevel("HS_EDITRISKASSESS") == UIAccessLevel.Hidden)
                                        || (this.GetUIItemAccessLevel("HS_RISKASSESSMENTS") == UIAccessLevel.Hidden);

                bool screenDisabled = (this.GetUIItemAccessLevel("HS_EDITRISKASSESS") == UIAccessLevel.Disabled)
                                        || (this.GetUIItemAccessLevel("HS_RISKASSESSMENTS") == UIAccessLevel.Disabled);

                if (screenHidden)
                    RedirectAccessDenied();

                if (screenDisabled)
                {
                    this.pageHiddenControls.Add(btnSave);
                    this.pageHiddenControls.Add(btnNewRecommendation);
                }

                UIAccessLevel l;

                l = this.GetUIItemAccessLevel("HS_EDITRISKASSESS_PREPDATE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblPrepariationDate);
                    this.pageDisabledControls.Add(this.txtPreparationDate);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblPrepariationDate);
                    this.pageHiddenControls.Add(this.txtPreparationDate);
                }

                l = this.GetUIItemAccessLevel("HS_EDITRISKASSESS_REGNUM");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblRegNumber);
                    this.pageDisabledControls.Add(this.txtRegNumber);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblRegNumber);
                    this.pageHiddenControls.Add(this.txtRegNumber);
                }

                l = this.GetUIItemAccessLevel("HS_EDITRISKASSESS_MILTFORCETYPE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblMilitForceTypeType);
                    this.pageDisabledControls.Add(this.ddlMilitaryForceTypes);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblMilitForceTypeType);
                    this.pageHiddenControls.Add(this.ddlMilitaryForceTypes);
                }

                l = this.GetUIItemAccessLevel("HS_EDITRISKASSESS_MILUNIT");
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

                l = this.GetUIItemAccessLevel("HS_EDITRISKASSESS_COMMENT");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblComments);
                    this.pageDisabledControls.Add(this.txtComments);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblComments);
                    this.pageHiddenControls.Add(this.txtComments);
                }

                bool lightBoxDisabled = (this.GetUIItemAccessLevel("HS_EDITRISKASSESS_RECOMM") != UIAccessLevel.Enabled)
                                            || (this.GetUIItemAccessLevel("HS_EDITRISKASSESS_RECOMM_RECOMM") == UIAccessLevel.Disabled
                                            && this.GetUIItemAccessLevel("HS_EDITRISKASSESS_RECOMM_DUEDATE") == UIAccessLevel.Disabled
                                            && this.GetUIItemAccessLevel("HS_EDITRISKASSESS_RECOMM_EXEDATE") == UIAccessLevel.Disabled
                                            || this.GetUIItemAccessLevel("HS_EDITRISKASSESS_RECOMM_RECOMM") == UIAccessLevel.Hidden
                                            && this.GetUIItemAccessLevel("HS_EDITRISKASSESS_RECOMM_DUEDATE") == UIAccessLevel.Hidden
                                            && this.GetUIItemAccessLevel("HS_EDITRISKASSESS_RECOMM_EXEDATE") == UIAccessLevel.Hidden)
                                            || screenDisabled;

                if (screenDisabled || lightBoxDisabled)
                {
                    if (!this.pageHiddenControls.Contains(this.btnNewRecommendation))
                    {
                        this.pageHiddenControls.Add(this.btnNewRecommendation);
                    }
                }

                if (lightBoxDisabled)
                {
                    this.hfIsLightBoxDisable.Value = "1";
                }
                else
                {
                    this.hfIsLightBoxDisable.Value = "0";
                }

                l = this.GetUIItemAccessLevel("HS_EDITRISKASSESS_RECOMM_RECOMM");
                if (l == UIAccessLevel.Disabled || lightBoxDisabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblRecommendationText);
                    this.pageDisabledControls.Add(this.txtRecommendationText);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblRecommendationText);
                    this.pageHiddenControls.Add(this.txtRecommendationText);
                }

                l = this.GetUIItemAccessLevel("HS_EDITRISKASSESS_RECOMM_DUEDATE");
                if (l == UIAccessLevel.Disabled || lightBoxDisabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblDueDate);
                    this.pageDisabledControls.Add(this.txtDueDate);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblDueDate);
                    this.pageHiddenControls.Add(this.txtDueDate);
                }

                l = this.GetUIItemAccessLevel("HS_EDITRISKASSESS_RECOMM_EXEDATE");
                if (l == UIAccessLevel.Disabled || lightBoxDisabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(this.lblExecutionDate);
                    this.pageDisabledControls.Add(this.txtExecutionDate);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(this.lblExecutionDate);
                    this.pageHiddenControls.Add(this.txtExecutionDate);
                }
            }
        }

        // Display message from ajax operations on recommendations, if exist
        private void SetRecommendationMessage()
        {
            if (this.hfMsg.Value == "FailRecommendationSave")
            {
                this.lblRecommendationMessage.Text = "Неуспешен запис на препоръката!";
                this.lblRecommendationMessage.CssClass = "ErrorText";
            }
            else if (hfMsg.Value == "SuccessRecommendationSave")
            {
                this.lblRecommendationMessage.Text = "Успешен запис на препоръката!";
                this.lblRecommendationMessage.CssClass = "SuccessText";
            }
            else if (hfMsg.Value == "FailRecommendationDelete")
            {
                this.lblRecommendationMessage.Text = "Неуспешно изтриване на препоръката!";
                this.lblRecommendationMessage.CssClass = "ErrorText";
            }
            else if (hfMsg.Value == "SuccessRecommendationDelete")
            {
                this.lblRecommendationMessage.Text = "Успешно изтриване на препоръката!";
                this.lblRecommendationMessage.CssClass = "SuccessText";
            }
            else
            {
                this.lblRecommendationMessage.Text = "";
            }

            this.hfMsg.Value = ""; //clean message form ajax operations
        }

        // Populate all dropdowns on the screen
        private void LoadDropDowns()
        {
            this.ddlMilitaryForceTypes.DataSource = MilitaryForceTypeUtil.GetAllMilitaryForceTypes(CurrentUser);
            this.ddlMilitaryForceTypes.DataTextField = "MilitaryForceTypeName";
            this.ddlMilitaryForceTypes.DataValueField = "MilitaryForceTypeId";
            this.ddlMilitaryForceTypes.DataBind();
            this.ddlMilitaryForceTypes.Items.Insert(0, ListItems.GetOptionChooseOne());
        }

        // Populate assessment's properties on the page
        private void LoadData()
        {
            if (this.RiskAssessmentId > 0)
            {
                RiskAssessment riskAssessment = RiskAssessmentUtil.GetRiskAssessment(this.RiskAssessmentId, CurrentUser);
                if (riskAssessment != null)
                {
                    this.txtPreparationDate.Text = CommonFunctions.FormatDate(riskAssessment.PreparationDate.ToString());
                    this.txtRegNumber.Text = riskAssessment.RegNumber;
                    this.ddlMilitaryForceTypes.SelectedValue = riskAssessment.MilitaryForceTypeId.ToString();
                    this.musMilitaryUnit.SelectedValue = riskAssessment.MilitaryUnitId.ToString();
                    this.musMilitaryUnit.SelectedText = (riskAssessment.MilitaryUnit == null ? "" : riskAssessment.MilitaryUnit.DisplayTextForSelection);
                    this.txtComments.Text = riskAssessment.Comments;   
                }
            }
        }

        // Gathers assessment's properties from the page controls
        private RiskAssessment CollectData()
        {
            RiskAssessment riskAssessment = new RiskAssessment(CurrentUser);
            riskAssessment.RiskAssessmentId = this.RiskAssessmentId;
            riskAssessment.RegNumber = this.txtRegNumber.Text.Trim();
            riskAssessment.PreparationDate = CommonFunctions.ParseDate(this.txtPreparationDate.Text.Trim()).Value;
            riskAssessment.MilitaryForceTypeId = ddlMilitaryForceTypes.SelectedValue != "-1" ? (int?)int.Parse(ddlMilitaryForceTypes.SelectedValue) : null;
            riskAssessment.MilitaryUnitId = (this.musMilitaryUnit.SelectedValue != "" && this.musMilitaryUnit.SelectedValue != "-1") ? Convert.ToInt32(this.musMilitaryUnit.SelectedValue) : (int?)null;
            riskAssessment.Comments = this.txtComments.Text.Trim();

            return riskAssessment; // return loaded with values assessment object
        }

        // Loads html table in the page with recommendations to this assessment
        private void LoadRecommendationsTable()
        {
            this.divRecommendations.InnerHtml = "";
            List<RiskRemoveRecommendation> recommendations = RiskRemoveRecommendationUtil.GetRiskRemoveRecommendationsByRiskAssesment(this.RiskAssessmentId, CurrentUser); // gets list of all recommendations related to this assessment
            this.divRecommendations.InnerHtml = this.GenerateRecommendationsTable(recommendations); //generate and display html with recommendations table on the page
        }

        // Generates html table from list of recommendations
        private string GenerateRecommendationsTable(List<RiskRemoveRecommendation> recommendations)
        {
            // Get the visibility right for recommendations grid           
            bool isEditHidden = this.GetUIItemAccessLevel("HS_EDITRISKASSESS_RECOMM") == UIAccessLevel.Hidden;

            if (isEditHidden)
            {
                return "";
            }
            else
            {
                bool IsRecomHidden = this.GetUIItemAccessLevel("HS_EDITRISKASSESS_RECOMM_RECOMM") == UIAccessLevel.Hidden;
                bool IsDueDateHidden = this.GetUIItemAccessLevel("HS_EDITRISKASSESS_RECOMM_DUEDATE") == UIAccessLevel.Hidden;
                bool IsExeDateHidden = this.GetUIItemAccessLevel("HS_EDITRISKASSESS_RECOMM_EXEDATE") == UIAccessLevel.Hidden;

                StringBuilder sb = new StringBuilder();
                sb.Append("<table id='recommendationsTable' name='recommendationsTable' class='CommonHeaderTable' style='text-align: left;' border='1px'>");
                sb.Append("<thead>");
                sb.Append("<tr>");
                sb.Append("<th style=\"min-width: 30px;\"></th>");
                if (!IsRecomHidden)
                    sb.Append("<th style=\"min-width: 150px;\">Препоръки за отстраняването</th>");
                if (!IsDueDateHidden)
                    sb.Append("<th style=\"min-width: 65px;\">Срок</th>");
                if (!IsExeDateHidden)
                    sb.Append("<th style=\"min-width: 150px;\">Дата на изпълнение</th>");
                sb.Append("<th style=\"min-width: 20px;\"></th>");
                sb.Append("</tr>");
                sb.Append("</thead>");
                int counter = 1;

                if (recommendations.Count > 0)
                {
                    sb.Append("<tbody>");
                }

                // Get the buttons rights for recommendation
                bool isDisabled = (this.GetUIItemAccessLevel("HS_EDITRISKASSESS_RECOMM") != UIAccessLevel.Enabled
                                        || this.GetUIItemAccessLevel("HS_EDITRISKASSESS") != UIAccessLevel.Enabled
                                        || this.GetUIItemAccessLevel("HS_RISKASSESSMENTS") != UIAccessLevel.Enabled);

                bool canOpenLightBox = (this.GetUIItemAccessLevel("HS_EDITRISKASSESS_RECOMM_RECOMM") == UIAccessLevel.Disabled
                                            && this.GetUIItemAccessLevel("HS_EDITRISKASSESS_RECOMM_DUEDATE") == UIAccessLevel.Disabled
                                            && this.GetUIItemAccessLevel("HS_EDITRISKASSESS_RECOMM_EXEDATE") == UIAccessLevel.Disabled
                                            || this.GetUIItemAccessLevel("HS_EDITRISKASSESS_RECOMM_RECOMM") == UIAccessLevel.Hidden
                                            && this.GetUIItemAccessLevel("HS_EDITRISKASSESS_RECOMM_DUEDATE") == UIAccessLevel.Hidden
                                            && this.GetUIItemAccessLevel("HS_EDITRISKASSESS_RECOMM_EXEDATE") == UIAccessLevel.Hidden);

                foreach (RiskRemoveRecommendation recommendation in recommendations)
                {
                    sb.Append("<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + "'>");
                    sb.Append("<td align='center'>" + counter + "</td>");
                    if (!IsRecomHidden)
                        sb.Append("<td style=\"max-width: 500px;\">" + CommonFunctions.ReplaceNewLinesInString(recommendation.Recommendations) + "</td>");
                    if (!IsDueDateHidden)
                        sb.Append("<td>" + CommonFunctions.FormatDate(recommendation.DueDate) + "</td>");
                    if (!IsExeDateHidden)
                        sb.Append("<td>" + CommonFunctions.FormatDate(recommendation.ExecutionDate) + "</td>");
                    // add edit and delete icons(buttons), which calls javascript functionality for necessary actions
                    sb.Append("<td align='center'>");

                    if (!isDisabled)
                    {
                        if (!canOpenLightBox)
                        {
                            sb.Append(@"<img onclick='javascript:ShowRecommendationLightBox(" + recommendation.RiskRemoveRecommendationId + @");' border='0' src='../Images/edit.png' alt='Покажи' title='Покажи' />");
                        }


                        sb.Append(@"&nbsp;&nbsp;&nbsp;");
                        sb.Append(@"<img onclick='javascript:DeleteRecommendation(" + recommendation.RiskRemoveRecommendationId + @");' border='0' src='../Images/delete.png' alt='Изтриване' title='Изтриване' />");
                    }

                    sb.Append("</td>");
                    sb.Append("</tr>");
                    counter++;
                }

                if (recommendations.Count > 0)
                {
                    sb.Append("</tbody>");
                }

                sb.Append("</table>");

                return sb.ToString();
            }
        }

        // Check for assessment properties validation and display appropriate error messages on the page, if need
        private bool ValidateData()
        {
            bool isDataValid = true;
            string errMsg = "";
            List<string> errRightsFields = new List<string>();

            if (this.txtPreparationDate.Text.Trim() == "")
            {
                isDataValid = false;

                if (pageDisabledControls.Contains(this.txtPreparationDate) || pageHiddenControls.Contains(this.txtPreparationDate))
                    errRightsFields.Add("Дата на изготвяне");
                else
                    errMsg += CommonFunctions.GetErrorMessageMandatory("Дата на изготвяне") + "<br/>";
            }
            else
            {
                if (!CommonFunctions.TryParseDate(this.txtPreparationDate.Text.Trim()))
                {
                    isDataValid = false;
                    errMsg += CommonFunctions.GetErrorMessageDate("Дата на изготвяне") + "<br/>";
                }
            }

            if (this.txtRegNumber.Text.Trim() == "")
            {
                isDataValid = false;

                if (pageDisabledControls.Contains(this.txtRegNumber) || pageHiddenControls.Contains(this.txtRegNumber))
                    errRightsFields.Add("Регистрационен номер");
                else
                    errMsg += CommonFunctions.GetErrorMessageMandatory("Регистрационен номер") + "<br/>";
            }

            if (this.musMilitaryUnit.SelectedValue == ListItems.GetOptionChooseOne().Value)
            {
                isDataValid = false;

                if (pageDisabledControls.Contains(this.musMilitaryUnit) || pageHiddenControls.Contains(this.musMilitaryUnit))
                    errRightsFields.Add(this.MilitaryUnitLabel);
                else
                    errMsg += CommonFunctions.GetErrorMessageMandatory(this.MilitaryUnitLabel) + "<br/>";
            }

            if (this.txtComments.Text.Trim() == "")
            {
                isDataValid = false;

                if (pageDisabledControls.Contains(this.txtComments) || pageHiddenControls.Contains(this.txtComments))
                    errRightsFields.Add("Коментари за оценката");
                else
                    errMsg += CommonFunctions.GetErrorMessageMandatory("Коментари за оценката") + "<br/>";
            }

            if (errRightsFields.Count() > 0)
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

        // Check for recommendation properties validation and display appropriate error messages on the page, if need
        private bool ValidateRecommendFields(string recommText, string dueDate, string execDate, out string errorMsg)
        {
            bool isDataValid = true;
            errorMsg = "";

            if (string.IsNullOrEmpty(recommText) || recommText.Trim() == "")
            {
                isDataValid = false;
                errorMsg += CommonFunctions.GetErrorMessageMandatory("Текст на препоръка") + "<br />";
            }

            if (string.IsNullOrEmpty(dueDate) || dueDate.Trim() == "")
            {
                isDataValid = false;
                errorMsg += CommonFunctions.GetErrorMessageMandatory("Дата за краен срок") + "<br/>";
            }
            else
            {
                if (!CommonFunctions.TryParseDate(dueDate.Trim()))
                {
                    isDataValid = false;
                    errorMsg += CommonFunctions.GetErrorMessageDate("Дата за краен срок") + "<br/>";
                }
            }

            if (!string.IsNullOrEmpty(execDate) || execDate.Trim() != "")
            {
                if (!CommonFunctions.TryParseDate(execDate.Trim()))
                {
                    isDataValid = false;
                    errorMsg += CommonFunctions.GetErrorMessageDate("Дата на изпълнение") + "<br/>";
                }
            }

            return isDataValid;
        }

        // Saves assessment
        private void SaveData()
        {
            RiskAssessment riskAssessment = this.CollectData(); // gathers assessment properties from page controls

            //Track the changes into the Audit Trail 
            Change change = new Change(CurrentUser, "HS_RiskAssessments");

            if (RiskAssessmentUtil.SaveRiskAssessment(riskAssessment, CurrentUser, change))
            {
                if (RiskAssessmentId == 0)
                {
                    SetLocationHash("AddEditRiskAssessment.aspx?RiskAssessmentId=" + riskAssessment.RiskAssessmentId.ToString());
                }

                this.RiskAssessmentId = riskAssessment.RiskAssessmentId;

                this.lblMessage.CssClass = "SuccessText";
                this.lblMessage.Text = "Успешен запис на оценката!";

                this.hdnSavedChanges.Value = "True";

                this.SetBtnPrintRiskAssessment();
            }
            else
            {
                this.lblMessage.CssClass = "ErrorText";
                this.lblMessage.Text = "Неуспешен запис на оценката!";
            }

            change.WriteLog();
        }

        // Saves assessment, but only if data valid
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (this.ValidateData())
            {
                this.SaveData();
                this.SetBtnNewRecommendation();
                this.SetPageName(); // sets page titles according to mode of work(add or edit risk assessment)
            }
        }

        // Saves recommendation from ajax request
        private void JSSaveRecommendation()
        {
            int riskAssessmentID = Int32.Parse(Request.Form["RiskAssessmentID"]);
            int recommendationID = Int32.Parse(Request.Form["RecommendationID"]);
            string recommendations = Request.Form["RecommendationText"];
            string dueDateStr = Request.Form["DueDate"];
            string execDateStr = Request.Form["ExecutionDate"];

            string resultMsg = "";

            if (this.ValidateRecommendFields(recommendations, dueDateStr, execDateStr, out resultMsg))
            {
                DateTime dueDate = CommonFunctions.ParseDate(dueDateStr.Trim()).Value;

                DateTime? executionDate = null;
                if (!string.IsNullOrEmpty(execDateStr))
                    executionDate = CommonFunctions.ParseDate(execDateStr.Trim());

                RiskRemoveRecommendation recommendation = new RiskRemoveRecommendation(CurrentUser);
                recommendation.RiskRemoveRecommendationId = recommendationID;
                recommendation.Recommendations = recommendations.Trim();
                recommendation.DueDate = dueDate;
                recommendation.ExecutionDate = executionDate;

                //Track the changes into the Audit Trail 
                Change change = new Change(CurrentUser, "HS_RiskAssessments");

                resultMsg = RiskRemoveRecommendationUtil.SaveRiskRemoveRecommendation(riskAssessmentID, recommendation, CurrentUser, change) ? AJAXTools.OK : AJAXTools.ERROR;

                change.WriteLog();
            }

            string response = @"<response>" + AJAXTools.EncodeForXML(resultMsg) + "</response>";
            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();
        }

        // Get ajax-requested data for recommendation(by RiskRemoveRecommendationID)
        private void JSGetRecommendation()
        {
            int recommendationID = Int32.Parse(Request.Form["RecommendationID"]);

            RiskRemoveRecommendation recommendation = RiskRemoveRecommendationUtil.GetRiskRemoveRecommendation(recommendationID, CurrentUser);

            string response = @"<response>                                   
                                    <RecommendationText>" + recommendation.Recommendations + @"</RecommendationText>
                                    <DueDate>" + CommonFunctions.FormatDate(recommendation.DueDate) + @"</DueDate>
                                    <ExecutionDate>" + CommonFunctions.FormatDate(recommendation.ExecutionDate) + @"</ExecutionDate>
                                </response>";

            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();
        }

        // Deletes recommendation by ajax request
        private void JSDeleteRecommendation()
        {
            if (this.GetUIItemAccessLevel("HS_EDITRISKASSESS_RECOMM") != UIAccessLevel.Enabled
                || this.GetUIItemAccessLevel("HS_EDITRISKASSESS") != UIAccessLevel.Enabled
                || this.GetUIItemAccessLevel("HS_RISKASSESSMENTS") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            int recommendationID = Int32.Parse(Request.Form["RecommendationID"]);

            string stat = "";
            string response = "";

            try
            {
                //Track the changes into the Audit Trail 
                Change change = new Change(CurrentUser, "HS_RiskAssessments");

                if (!RiskRemoveRecommendationUtil.DeleteRiskRemoveRecommendation(RiskAssessmentId, recommendationID, CurrentUser, change))
                    throw new Exception("Database operation failed!");

                change.WriteLog();

                stat = AJAXTools.OK;
                response = "<response>OK</response>";
            }
            catch (Exception ex)
            {
                stat = AJAXTools.ERROR;
                response = AJAXTools.EncodeForXML(ex.Message);
            }


            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        // hidden button for forced refresh of recommendations table
        protected void btnHdnRefreshRecommendations_Click(object sender, EventArgs e)
        {
            this.LoadRecommendationsTable();
        }

        //Navigate back to the ManageRiskAssessments screen
        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (this.FromHome != 1)
                Response.Redirect("~/ContentPages/ManageRiskAssessments.aspx");
            else
                RedirectAccessDenied();
        }
        //Set label text
        private void SetLabelValueText()
        {
            this.lblMilitaryUnit.Text = this.MilitaryUnitLabel + ":";
        }
    }
}
