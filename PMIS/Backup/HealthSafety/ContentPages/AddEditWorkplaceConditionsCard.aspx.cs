using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using PMIS.Common;
using PMIS.HealthSafety.Common;

namespace PMIS.HealthSafety.ContentPages
{
    public partial class AddEditWorkplaceConditionsCard : HSPage
    {
        private bool tableEditPermission = true;
        private bool showTable = true;

        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "HS_WCONDCARDS";
            }
        }

        //Getter/Setter of the ID of the displayed workplace condition card(0 - if new)
        private int WorkplaceConditionsCardId
        {
            get
            {
                int workplaceConditionsCardId = 0;
                //gets WorkplaceConditionCardID either from the hidden field on the page or from page url
                if (String.IsNullOrEmpty(this.hfWorkplaceConditionsCardID.Value)
                    || this.hfWorkplaceConditionsCardID.Value == "0")
                {
                    if (Request.Params["WorkplaceConditionsCardId"] != null)
                        int.TryParse(Request.Params["WorkplaceConditionsCardId"].ToString(), out workplaceConditionsCardId);

                    //sets WorkplaceConditionCardID in hidden field on the page in order to be accessible in javascript
                    this.hfWorkplaceConditionsCardID.Value = workplaceConditionsCardId.ToString();
                }
                else
                {
                    Int32.TryParse(this.hfWorkplaceConditionsCardID.Value, out workplaceConditionsCardId);
                }

                return workplaceConditionsCardId;
            }
            set { this.hfWorkplaceConditionsCardID.Value = value.ToString(); }
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

            //Highlight the current page in the menu bar
            HighlightMenuItems("WorkplaceConditionsCards_Add", "WorkplaceConditionsCards");

            //Hide the navigation buttons
            HideNavigationControls(btnBack);

            // OBSOLETE
            // Prevent showing "Save changes" dialog box
            //LnkForceNoChangesCheck(btnSave);

            //Process the ajax request for card item properties
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetCardItem")
            {
                JSGetCardItem();
                return;
            }

            //Process the ajax request for saving card items (indicator types)
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveCardIndicatorTypeItem")
            {
                JSSaveCardIndicatorTypeItem();
                return;
            }

            //Process the ajax request for saving card items (indicators)
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveCardIndicatorItem")
            {
                JSSaveCardIndicatorItem();
                return;
            }

            //Process ajax request for getting indicators for card item light box
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetIndicators")
            {
                JSGetIndicators();
                return;
            }

            //Process ajax request for deleting of card item
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteCardItem")
            {
                JSDeleteCardItem();
                return;
            }

            jsMilitaryUnitSelectorDiv.InnerHtml = @"<script src=""" + Page.ClientScript.GetWebResourceUrl(typeof(MilitaryUnitSelector.MilitaryUnitSelector), "MilitaryUnitSelector.MilitaryUnitSelector.js") + @""" type=""text/javascript""></script>";

            SetupPageUI(); //setup user interface elements according to rights of the user's role

            if (!IsPostBack)
            {
                SetPageName(); // sets page titles according to mode of work(add or edit card)
                LoadDropDowns(); //fills dropdowns on the page with values
                LoadData(); // fills the controls on the page, displaying properties of the workplace conditions card
                LoadCardItemsTable(); // loads html table in the page with workplace conditions card items to this workplace conditions card                
                this.SetBtnPrintWorkplaceConditionsCard(); // Set visibility of the print button
            }

            lblMessage.Text = ""; // clean message of workplace conditions card operations
            SetTableWarning(); // set visible or invisible warning message for editing card item table only if card is saved
            SetCardItemMessage(); //display message from ajax operations on workplace conditions card items, if exist
        }       

        // Set page titles according to mode of work(add or edit workplace condition card)
        private void SetPageName()
        {
            if (this.WorkplaceConditionsCardId > 0)
            {
                lblHeaderTitle.InnerHtml = "Редактиране";
                Page.Title = "Редактиране";
            }
            else
            {
                lblHeaderTitle.InnerHtml = "Добавяне";
                Page.Title = "Добавяне";
            }

            string title = " на карта за комплексно оценяване на рисковете за живота и здравето, <br />които независимо от предприетите мерки не могат да бъдат отстранени, ограничени или намалени";
            lblHeaderTitle.InnerHtml += title;
            Page.Title += title;
        }

        // Set visibility of print button
        private void SetBtnPrintWorkplaceConditionsCard()
        {
            // if the card is new and not saved yet, it is not allowed to print it
            if (this.WorkplaceConditionsCardId == 0)
            {
                this.btnPrintWorkplaceConditionsCard.Visible = false;
            }
            else
            {
                this.btnPrintWorkplaceConditionsCard.Visible = true;
            }
        }

        // Set lable with warning for editing table only when card is saved
        private void SetTableWarning()
        {
            if (WorkplaceConditionsCardId != 0 || !showTable)
                lblTableWarning.Visible = false;
            else
                lblTableWarning.Visible = true;
        }

        // Setup user interface elements according to rights of the user's role
        private void SetupPageUI()
        {
            if (WorkplaceConditionsCardId == 0) // add mode of page
            {
                bool screenHidden = (this.GetUIItemAccessLevel("HS_ADDWCONDCARD") != UIAccessLevel.Enabled) ||
                                      (this.GetUIItemAccessLevel("HS_WCONDCARDS") != UIAccessLevel.Enabled);

                bool screenDisabled = (this.GetUIItemAccessLevel("HS_ADDWCONDCARD") == UIAccessLevel.Disabled) ||
                                      (this.GetUIItemAccessLevel("HS_WCONDCARDS") == UIAccessLevel.Disabled);

                if (screenHidden)
                    RedirectAccessDenied();

                if (screenDisabled)
                {
                    this.pageDisabledControls.Add(btnSave);
                    tableEditPermission = false;
                }

                UIAccessLevel l;

                l = this.GetUIItemAccessLevel("HS_ADDWCONDCARD_MILITARYUNIT");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblMilitaryUnit);
                    this.pageDisabledControls.Add(musMilitaryUnit);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblMilitaryUnit);
                    this.pageHiddenControls.Add(musMilitaryUnit);
                }

                l = this.GetUIItemAccessLevel("HS_ADDWCONDCARD_CARDNUMBER");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblCardNumber);
                    this.pageDisabledControls.Add(txtCardNumber);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblCardNumber);
                    this.pageHiddenControls.Add(txtCardNumber);
                }

                l = this.GetUIItemAccessLevel("HS_ADDWCONDCARD_JOBTYPE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblJobType);
                    this.pageDisabledControls.Add(txtJobType);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblJobType);
                    this.pageHiddenControls.Add(txtJobType);
                }

                l = this.GetUIItemAccessLevel("HS_ADDWCONDCARD_CITY");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblCity);
                    this.pageDisabledControls.Add(txtCity);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblCity);
                    this.pageHiddenControls.Add(txtCity);
                }

                l = this.GetUIItemAccessLevel("HS_ADDWCONDCARD_WORKERSCOUNT");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblWorkersCount);
                    this.pageDisabledControls.Add(txtWorkersCount);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblWorkersCount);
                    this.pageHiddenControls.Add(txtWorkersCount);
                }          

                l = this.GetUIItemAccessLevel("HS_ADDWCONDCARD_COMPLEXASSESSMENT");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblComplexAssessment);
                    this.pageDisabledControls.Add(txtComplexAssessment);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblComplexAssessment);
                    this.pageHiddenControls.Add(txtComplexAssessment);
                }

                l = this.GetUIItemAccessLevel("HS_ADDWCONDCARD_COMPLEXASSESSMENTPOINTVALUE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblComplexAssessmentPointValue);
                    this.pageDisabledControls.Add(txtComplexAssessmentPointValue);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblComplexAssessmentPointValue);
                    this.pageHiddenControls.Add(txtComplexAssessmentPointValue);
                }

                l = this.GetUIItemAccessLevel("S_ADDWCONDCARD_ADDITIONALREWARD");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblAdditionalReward);
                    this.pageDisabledControls.Add(txtAdditionalReward);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblAdditionalReward);
                    this.pageHiddenControls.Add(txtAdditionalReward);
                }
            }
            else // edit mode of page
            {
                bool screenHidden = (this.GetUIItemAccessLevel("HS_EDITWCONDCARD") == UIAccessLevel.Hidden) ||
                                      (this.GetUIItemAccessLevel("HS_WCONDCARDS") == UIAccessLevel.Hidden);

                bool screenDisabled = (this.GetUIItemAccessLevel("HS_EDITWCONDCARD") == UIAccessLevel.Disabled) ||
                                      (this.GetUIItemAccessLevel("HS_WCONDCARDS") == UIAccessLevel.Disabled);

                if (screenHidden)
                    RedirectAccessDenied();

                if (screenDisabled)
                {
                    this.pageDisabledControls.Add(btnSave);
                    tableEditPermission = false;
                }

                UIAccessLevel l;

                l = this.GetUIItemAccessLevel("HS_EDITWCONDCARD_MILITARYUNIT");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblMilitaryUnit);
                    this.pageDisabledControls.Add(musMilitaryUnit);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblMilitaryUnit);
                    this.pageHiddenControls.Add(musMilitaryUnit);
                }

                l = this.GetUIItemAccessLevel("HS_EDITWCONDCARD_CARDNUMBER");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblCardNumber);
                    this.pageDisabledControls.Add(txtCardNumber);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblCardNumber);
                    this.pageHiddenControls.Add(txtCardNumber);
                }

                l = this.GetUIItemAccessLevel("HS_EDITWCONDCARD_JOBTYPE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblJobType);
                    this.pageDisabledControls.Add(txtJobType);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblJobType);
                    this.pageHiddenControls.Add(txtJobType);
                }

                l = this.GetUIItemAccessLevel("HS_EDITWCONDCARD_CITY");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblCity);
                    this.pageDisabledControls.Add(txtCity);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblCity);
                    this.pageHiddenControls.Add(txtCity);
                }

                l = this.GetUIItemAccessLevel("HS_EDITWCONDCARD_WORKERSCOUNT");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblWorkersCount);
                    this.pageDisabledControls.Add(txtWorkersCount);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblWorkersCount);
                    this.pageHiddenControls.Add(txtWorkersCount);
                }

                l = this.GetUIItemAccessLevel("HS_EDITWCONDCARD_COMPLEXASSESSMENT");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblComplexAssessment);
                    this.pageDisabledControls.Add(txtComplexAssessment);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblComplexAssessment);
                    this.pageHiddenControls.Add(txtComplexAssessment);
                }

                l = this.GetUIItemAccessLevel("HS_EDITWCONDCARD_COMPLEXASSESSMENTPOINTVALUE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblComplexAssessmentPointValue);
                    this.pageDisabledControls.Add(txtComplexAssessmentPointValue);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblComplexAssessmentPointValue);
                    this.pageHiddenControls.Add(txtComplexAssessmentPointValue);
                }

                l = this.GetUIItemAccessLevel("S_EDITWCONDCARD_ADDITIONALREWARD");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblAdditionalReward);
                    this.pageDisabledControls.Add(txtAdditionalReward);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblAdditionalReward);
                    this.pageHiddenControls.Add(txtAdditionalReward);
                }

                l = this.GetUIItemAccessLevel("HS_EDITWCONDCARD_CARDITEMS");
                if (l == UIAccessLevel.Disabled || screenDisabled
                        || (this.GetUIItemAccessLevel("HS_EDITWCONDCARD_CARDITEMS_INDICATOR") == UIAccessLevel.Disabled
                        && this.GetUIItemAccessLevel("HS_EDITWCONDCARD_CARDITEMS_VALUE") == UIAccessLevel.Disabled
                        && this.GetUIItemAccessLevel("HS_EDITWCONDCARD_CARDITEMS_RATE") == UIAccessLevel.Disabled
                        && this.GetUIItemAccessLevel("HS_EDITWCONDCARD_CARDITEMS_ASSESSMENT") == UIAccessLevel.Disabled
                        || this.GetUIItemAccessLevel("HS_EDITWCONDCARD_CARDITEMS_INDICATOR") == UIAccessLevel.Hidden
                        && this.GetUIItemAccessLevel("HS_EDITWCONDCARD_CARDITEMS_VALUE") == UIAccessLevel.Hidden
                        && this.GetUIItemAccessLevel("HS_EDITWCONDCARD_CARDITEMS_RATE") == UIAccessLevel.Hidden
                        && this.GetUIItemAccessLevel("HS_EDITWCONDCARD_CARDITEMS_ASSESSMENT") == UIAccessLevel.Hidden))
                {
                    tableEditPermission = false;
                }
                if (l == UIAccessLevel.Hidden)
                {
                    showTable = false;
                }

                List<string> disabledClientControls = new List<string>();
                List<string> hiddenClientControls = new List<string>();

                l = this.GetUIItemAccessLevel("HS_EDITWCONDCARD_CARDITEMS_INDICATOR");
                if (l == UIAccessLevel.Disabled)
                {
                    disabledClientControls.Add("lblIndicator");
                    disabledClientControls.Add("txtIndicator");
                    disabledClientControls.Add("ddIndicator");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblIndicator");
                    hiddenClientControls.Add("txtIndicator");
                    hiddenClientControls.Add("ddIndicator");
                }

                l = this.GetUIItemAccessLevel("HS_EDITWCONDCARD_CARDITEMS_VALUE");
                if (l == UIAccessLevel.Disabled)
                {
                    disabledClientControls.Add("lblValue");
                    disabledClientControls.Add("txtValue");
                    disabledClientControls.Add("lblValue2");
                    disabledClientControls.Add("txtValue2");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblValue");
                    hiddenClientControls.Add("txtValue");
                    hiddenClientControls.Add("lblValue2");
                    hiddenClientControls.Add("txtValue2");
                }

                l = this.GetUIItemAccessLevel("HS_EDITWCONDCARD_CARDITEMS_RATE");
                if (l == UIAccessLevel.Disabled)
                {
                    disabledClientControls.Add("lblRate");
                    disabledClientControls.Add("txtRate");
                    disabledClientControls.Add("lblRate2");
                    disabledClientControls.Add("txtRate2");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblRate");
                    hiddenClientControls.Add("txtRate");
                    hiddenClientControls.Add("lblRate2");
                    hiddenClientControls.Add("txtRate2");
                }

                l = this.GetUIItemAccessLevel("HS_EDITWCONDCARD_CARDITEMS_ASSESSMENT");
                if (l == UIAccessLevel.Disabled)
                {
                    disabledClientControls.Add("lblAssessment");
                    disabledClientControls.Add("txtAssessment");
                    disabledClientControls.Add("lblAssessment2");
                    disabledClientControls.Add("txtAssessment2");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAssessment");
                    hiddenClientControls.Add("txtAssessment");
                    hiddenClientControls.Add("lblAssessment2");
                    hiddenClientControls.Add("txtAssessment2");
                } 

                SetDisabledClientControls(disabledClientControls.ToArray());
                SetHiddenClientControls(hiddenClientControls.ToArray());
            }

        }

        // Display message from ajax operations on card items, if exist
        private void SetCardItemMessage()
        {
            if (hfMsg.Value == "FailCardIndicatorItemSave")
            {
                lblMessage.Text = "Неуспешен запис на показател!";
                lblMessage.CssClass = "ErrorText";
            }
            else if (hfMsg.Value == "SuccessCardIndicatorItemSave")
            {
                lblMessage.Text = "Успешен запис на показател!";
                lblMessage.CssClass = "SuccessText";
            }
            else if (hfMsg.Value == "FailCardIndicatorTypeItemSave")
            {
                lblMessage.Text = "Неуспешен запис на елемент!";
                lblMessage.CssClass = "ErrorText";
            }
            else if (hfMsg.Value == "SuccessCardIndicatorTypeItemSave")
            {
                lblMessage.Text = "Успешен запис на елемент!";
                lblMessage.CssClass = "SuccessText";
            }
            else if (hfMsg.Value == "FailCardItemDelete")
            {
                lblMessage.Text = "Неуспешно изтриване на показател!";
                lblMessage.CssClass = "ErrorText";
            }
            else if (hfMsg.Value == "SuccessCardItemDelete")
            {
                lblMessage.Text = "Успешно изтриване на показател!";
                lblMessage.CssClass = "SuccessText";
            }
            else
            {
                lblMessage.Text = "";
            }

            hfMsg.Value = ""; //clean message form ajax operations
        }

        // Populate all dropdowns on the screen
        private void LoadDropDowns()
        {
        }

        // Populate workplace conditions card's properites on the page
        private void LoadData()
        {
            if (WorkplaceConditionsCardId > 0)
            {
                WorkplaceConditionsCard card = WorkplaceConditionsCardUtil.GetWorkplaceConditionsCard(WorkplaceConditionsCardId, CurrentUser);
                if (card != null)
                {
                    musMilitaryUnit.SelectedValue = card.MilitaryUnitId.ToString();
                    musMilitaryUnit.SelectedText = card.MilitaryUnit.DisplayTextForSelection;
                    txtCardNumber.Text = card.CardNumber;
                    txtJobType.Text = card.JobType;
                    txtCity.Text = card.MilitaryUnit.City != null ? card.MilitaryUnit.City.CityName : "";
                    txtWorkersCount.Text = card.WorkersCount.HasValue ? card.WorkersCount.Value.ToString() : "";
                    txtComplexAssessment.Text = card.ComplexAssessment.HasValue ? CommonFunctions.FormatDecimal(card.ComplexAssessment.Value) : "";
                    txtComplexAssessmentPointValue.Text = card.ComplexAssessmentPointValue.HasValue ? CommonFunctions.FormatDecimal(card.ComplexAssessmentPointValue.Value) : "";
                    txtAdditionalReward.Text = card.AdditionalReward.HasValue ? CommonFunctions.FormatDecimal(card.AdditionalReward.Value) : "";   
                }
            }         
        }

        // Gathers workplace conditions card's properties from the page controls
        private WorkplaceConditionsCard CollectData()
        {
            WorkplaceConditionsCard card = new WorkplaceConditionsCard(CurrentUser);
            card.WorkplaceConditionsCardId = WorkplaceConditionsCardId;
            card.MilitaryUnitId = int.Parse(musMilitaryUnit.SelectedValue);
            card.CardNumber = txtCardNumber.Text;
            card.JobType = txtJobType.Text;
            card.WorkersCount = txtWorkersCount.Text != "" ? (int?)int.Parse(txtWorkersCount.Text) : null;
            if (CommonFunctions.IsValidDecimal(txtComplexAssessment.Text))
                card.ComplexAssessment = CommonFunctions.ParseDecimal(txtComplexAssessment.Text);
            if (CommonFunctions.IsValidDecimal(txtComplexAssessmentPointValue.Text))
                card.ComplexAssessmentPointValue = CommonFunctions.ParseDecimal(txtComplexAssessmentPointValue.Text);
            if (CommonFunctions.IsValidDecimal(txtAdditionalReward.Text))
                card.AdditionalReward = CommonFunctions.ParseDecimal(txtAdditionalReward.Text);            

            return card; // return loaded with values workplace conditions card object
        }

        // Loads html table in the page with workplace conditions card items to this protocol
        private void LoadCardItemsTable()
        {
            this.divCardItems.InnerHtml = "";
            if (showTable)
            {
                List<WorkplaceConditionsCardItem> cardItems = null;

                // gets list of all workplace conditions card items related to this card, or gets default if the card is new
                if (WorkplaceConditionsCardId > 0)
                    cardItems = WorkplaceConditionsCardItemUtil.GetAllWorkplaceConditionsCardItemsByCard(WorkplaceConditionsCardId, CurrentUser);
                else
                    cardItems = WorkplaceConditionsCardItemUtil.GetDefaultWorkplaceConditionsCardItems(CurrentUser);
                                                    
                this.divCardItems.InnerHtml = this.GenerateCardItemsTable(cardItems); //generate and display html with workplace conditions card items table on the page
            }
        }

        // Generates html table from list of card items
        private string GenerateCardItemsTable(List<WorkplaceConditionsCardItem> cardItems)
        {
            bool isIndicatorHidden = this.GetUIItemAccessLevel("HS_EDITWCONDCARD_CARDITEMS_INDICATOR") == UIAccessLevel.Hidden;
            bool isValueHidden = this.GetUIItemAccessLevel("HS_EDITWCONDCARD_CARDITEMS_VALUE") == UIAccessLevel.Hidden;
            bool isRateHidden = this.GetUIItemAccessLevel("HS_EDITWCONDCARD_CARDITEMS_RATE") == UIAccessLevel.Hidden;
            bool isAssessmentHidden = this.GetUIItemAccessLevel("HS_EDITWCONDCARD_CARDITEMS_ASSESSMENT") == UIAccessLevel.Hidden;

            StringBuilder sb = new StringBuilder();
            sb.Append("<table id='cardItemsTable' name='cardItemsTable' class='CommonHeaderTable' style='text-align: left;' border='1px'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style=\"width: 30px;\"></th>");
            if (!isIndicatorHidden)
                sb.Append("<th style=\"width: 300px;\">Показатели на условията на труд</th>");
            if (!isValueHidden)
                sb.Append("<th style=\"width: 136px;\">Стойност на показателя</th>");
            if (!isRateHidden)
                sb.Append("<th style=\"width: 136px;\">Степен</th>");
            if (!isAssessmentHidden)
                sb.Append("<th style=\"width: 136px;\">Оценка</th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            int counter = 1;

            if (cardItems.Count > 0)
            {
                sb.Append("<tbody>");
            }

            foreach (WorkplaceConditionsCardItem cardItem in cardItems)
            {
                // add edit and add icons(buttons), which calls javascript functionality for necessary actions for indicator types card items
                string controlHtml = "";

                if (WorkplaceConditionsCardId != 0)
                {
                    if (tableEditPermission)
                    {
                        if (WCondIndicatorUtil.GetAllIndicatorsByTypeCount(cardItem.IndicatorTypeId, CurrentUser) > 0) // icon for add indicator card item is added only if there are indicators for that indicator type
                            controlHtml += @"<img src='../Images/addrow.gif' alt='Добавяне' title='Добавяне на нов елемент' class='GridActionIcon' style=""float: right;"" onclick=""ShowCardItemIndicatorLightBox(0, '" + cardItem.IndicatorTypeName + @"'," + cardItem.IndicatorTypeId.ToString() + @");"" />";
                        controlHtml += @"<img border='0' src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='ShowCardItemIndicatorTypeLightBox(" + cardItem.WorkplaceConditionsCardItemId + @");' style=""float: right;"" />";
                    }
                }

                sb.Append("<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + "'>");
                sb.Append("<td>" + counter + "</td>");
                if (!isIndicatorHidden)
                    sb.Append("<td>" + cardItem.Caption + controlHtml + "</td>");
                if (!isValueHidden)
                    sb.Append("<td>" + (cardItem.Value.HasValue ? CommonFunctions.FormatDecimal(cardItem.Value.Value) : "") + "</td>");
                if (!isRateHidden)
                    sb.Append("<td>" + (cardItem.Rate.HasValue ? CommonFunctions.FormatDecimal(cardItem.Rate.Value) : "") + "</td>");
                if (!isAssessmentHidden)
                    sb.Append("<td>" + (cardItem.Assessment.HasValue ? CommonFunctions.FormatDecimal(cardItem.Assessment.Value) : "") + "</td>");

                sb.Append("</tr>");
                counter++;

                // add as a sub-items(with different style) child elements (indicator card items for this indicator types)
                List<WorkplaceConditionsCardItem> childCardItems = WorkplaceConditionsCardItemUtil.GetAllWorkplaceConditionsCardItemsByIndicatorType(WorkplaceConditionsCardId, cardItem.IndicatorTypeId, CurrentUser);

                int childCounter = 1;
                foreach (WorkplaceConditionsCardItem childCardItem in childCardItems)
                {
                    string childControlHtml = "";

                    // add edit and delete icons(buttons), which calls javascript functionality for necessary actions
                    if (tableEditPermission)
                    {
                        childControlHtml += @"<img border='0' src='../Images/delete.png' alt='Изтриване' title='Изтриване' class='GridActionIcon' onclick='DeleteCardItem(" + childCardItem.WorkplaceConditionsCardItemId + @");' style=""float: right;"" />";
                        childControlHtml += @"<img border='0' src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick=""ShowCardItemIndicatorLightBox(" + childCardItem.WorkplaceConditionsCardItemId + @", '" + childCardItem.IndicatorTypeName + @"'," + childCardItem.IndicatorTypeId.ToString() + @");"" style=""float: right;"" />";
                    }

                    sb.Append("<tr class='" + ((childCounter + counter) % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + "'>");
                    sb.Append("<td></td>");
                    if (!isIndicatorHidden)
                        sb.Append("<td style='font-style:italic;'>&nbsp;&nbsp;&nbsp;" + childCardItem.Caption + childControlHtml + "</td>");
                    if (!isValueHidden)
                        sb.Append("<td>" + (childCardItem.Value.HasValue ? CommonFunctions.FormatDecimal(childCardItem.Value.Value) : "") + "</td>");
                    if (!isRateHidden)
                        sb.Append("<td>" + (childCardItem.Rate.HasValue ? CommonFunctions.FormatDecimal(childCardItem.Rate.Value) : "") + "</td>");
                    if (!isAssessmentHidden)
                        sb.Append("<td>" + (childCardItem.Assessment.HasValue ? CommonFunctions.FormatDecimal(childCardItem.Assessment.Value) : "") + "</td>");

                    sb.Append("</tr>");
                    childCounter++;
                }
            }

            if (cardItems.Count > 0)
            {
                sb.Append("</tbody>");
            }

            sb.Append("</table>");

            return sb.ToString();
        }

        // Saves workplace conditions card
        private void SaveData()
        {
            WorkplaceConditionsCard card = CollectData(); // gathers card properties from page controls

            //Track the changes into the Audit Trail 
            Change change = new Change(CurrentUser, "HS_WorkplaceCondtitionsCard");

            if (WorkplaceConditionsCardUtil.SaveWorkplaceConditionsCard(card, CurrentUser, change))
            {
                // if new, save default card items
                if (WorkplaceConditionsCardId == 0)
                {
                    List<WorkplaceConditionsCardItem> cardItems = WorkplaceConditionsCardItemUtil.GetDefaultWorkplaceConditionsCardItems(CurrentUser);
                    foreach (WorkplaceConditionsCardItem cardItem in cardItems)
                    {
                        WorkplaceConditionsCardItemUtil.SaveWorkplaceConditionsCardItem(card.WorkplaceConditionsCardId, cardItem, CurrentUser, change);
                    }

                    SetLocationHash("AddEditWorkplaceConditionsCard.aspx?WorkplaceConditionsCardID=" + card.WorkplaceConditionsCardId.ToString());
                }

                WorkplaceConditionsCardId = card.WorkplaceConditionsCardId;

                SetTableWarning();
                LoadCardItemsTable();

                this.lblMessage.CssClass = "SuccessText";
                this.lblMessage.Text = "Успешен запис на карта!";

                hdnSavedChanges.Value = "True";
                SetupPageUI();

                this.SetBtnPrintWorkplaceConditionsCard();

                RefreshCity();
            }
            else
            {
                this.lblMessage.CssClass = "ErrorText";
                this.lblMessage.Text = "Неуспешен запис на карта!";
            }

            change.WriteLog();
        }

        // Saves workplace conditions card
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
            SetPageName(); // sets page titles according to mode of work(add or edit workplace conditions card)
        }
        
        // Saves card item(indicator type) from ajax request
        private void JSSaveCardIndicatorTypeItem()
        {
            int cardID = int.Parse(Request.Form["CardID"]);
            int cardItemID = int.Parse(Request.Form["CardItemID"]);
            decimal? value = !string.IsNullOrEmpty(Request.Form["Value"]) ? (decimal?)CommonFunctions.ParseDecimal(Request.Form["Value"]) : null;
            decimal? rate = !string.IsNullOrEmpty(Request.Form["Rate"]) ? (decimal?)CommonFunctions.ParseDecimal(Request.Form["Rate"]) : null;
            decimal? assessment = !string.IsNullOrEmpty(Request.Form["Assessment"]) ? (decimal?)CommonFunctions.ParseDecimal(Request.Form["Assessment"]) : null;

            WorkplaceConditionsCardItem cardItem = WorkplaceConditionsCardItemUtil.GetWorkplaceConditionsCardItem(cardItemID, CurrentUser);
            cardItem.Value = value;
            cardItem.Rate = rate;
            cardItem.Assessment = assessment;

            //Track the changes into the Audit Trail 
            Change change = new Change(CurrentUser, "HS_WorkplaceCondtitionsCard");

            string resultMsg = WorkplaceConditionsCardItemUtil.SaveWorkplaceConditionsCardItem(cardID, cardItem, CurrentUser, change) ? AJAXTools.OK : AJAXTools.ERROR;

            change.WriteLog();

            string response = @"<response>" + AJAXTools.EncodeForXML(resultMsg) + "</response>";
            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();
        }

        // Saves card item(indicator) from ajax request
        private void JSSaveCardIndicatorItem()
        {
            int cardID = int.Parse(Request.Form["CardID"]);
            int cardItemID = int.Parse(Request.Form["CardItemID"]);
            int indicatorTypeID = int.Parse(Request.Form["IndicatorTypeID"]);
            string indicatorTypeName = Request.Form["IndicatorTypeName"].ToString();
            int indicatorID = int.Parse(Request.Form["IndicatorID"]);
            decimal? value = !string.IsNullOrEmpty(Request.Form["Value"]) ? (decimal?)CommonFunctions.ParseDecimal(Request.Form["Value"]) : null;
            decimal? rate = !string.IsNullOrEmpty(Request.Form["Rate"]) ? (decimal?)CommonFunctions.ParseDecimal(Request.Form["Rate"]) : null;
            decimal? assessment = !string.IsNullOrEmpty(Request.Form["Assessment"]) ? (decimal?)CommonFunctions.ParseDecimal(Request.Form["Assessment"]) : null;

            WorkplaceConditionsCardItem cardItem = WorkplaceConditionsCardItemUtil.GetWorkplaceConditionsCardItem(cardItemID, CurrentUser);
            if (cardItem == null)
                cardItem = new WorkplaceConditionsCardItem();

            cardItem.IndicatorTypeId = indicatorTypeID;
            cardItem.IndicatorTypeName = indicatorTypeName;
            if (cardItem.IndicatorId != indicatorID)
                cardItem.IndicatorName = WCondIndicatorUtil.GetIndicator(indicatorID, CurrentUser).IndicatorName;
            cardItem.IndicatorId = indicatorID;
            cardItem.Value = value;
            cardItem.Rate = rate;
            cardItem.Assessment = assessment;

            //Track the changes into the Audit Trail 
            Change change = new Change(CurrentUser, "HS_WorkplaceCondtitionsCard");

            string resultMsg = WorkplaceConditionsCardItemUtil.SaveWorkplaceConditionsCardItem(cardID, cardItem, CurrentUser, change) ? AJAXTools.OK : AJAXTools.ERROR;

            change.WriteLog();

            string response = @"<response>" + AJAXTools.EncodeForXML(resultMsg) + "</response>";
            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();
        }
      
        // Get ajax-requested data for card item(by CardItemID)
        private void JSGetCardItem()
        {
            int cardItemID = int.Parse(Request.Form["CardItemID"]);

            WorkplaceConditionsCardItem cardItem = WorkplaceConditionsCardItemUtil.GetWorkplaceConditionsCardItem(cardItemID, CurrentUser);            

            string response = @"<response>                                   
                                    <Name>" + AJAXTools.EncodeForXML(cardItem.IndicatorTypeName) + @"</Name>   
                                    <IndicatorID>" + (cardItem.IndicatorId.HasValue ? cardItem.IndicatorId.Value.ToString() : "0") + @"</IndicatorID>                                   
                                    <Value>" + (cardItem.Value.HasValue ? CommonFunctions.FormatDecimal(cardItem.Value.Value) : "") + @"</Value>
                                    <Rate>" + (cardItem.Rate.HasValue ? CommonFunctions.FormatDecimal(cardItem.Rate.Value) : "") + @"</Rate>
                                    <Assessment>" + (cardItem.Assessment.HasValue ? CommonFunctions.FormatDecimal(cardItem.Assessment.Value) : "") + @"</Assessment>                                   
                                </response>";

            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();
        }

        // Get ajax-requested data(html of dropdown) for all indicators of given indicator type(for indicator light box)
        private void JSGetIndicators()
        {
            int indicatorTypeID = int.Parse(Request.Form["IndicatorTypeID"]);
            int cardItemID = int.Parse(Request.Form["CardItemID"]);

            List<WCondIndicator> indicators = WCondIndicatorUtil.GetAllIndicatorsByType(indicatorTypeID, CurrentUser);

            List<IDropDownItem> indicatorDropDownItems = new List<IDropDownItem>();
            foreach (WCondIndicator indicator in indicators)
                indicatorDropDownItems.Add(indicator as IDropDownItem);            

            // add aditional indicator, if current indicator of the card item is now deleted from the system, in order to keep it if the user don't want to change it
            WorkplaceConditionsCardItem item = WorkplaceConditionsCardItemUtil.GetWorkplaceConditionsCardItem(cardItemID, CurrentUser);
            WCondIndicator additionalIndicator = null;
            if (item != null && item.IndicatorId.HasValue)            
                additionalIndicator = new WCondIndicator(item.IndicatorId.Value, item.IndicatorName);

            IDropDownItem selectedItem = null;

            // generates html for client drop down(<select>)
            string result = ListItems.GetDropDownHtml(indicatorDropDownItems, additionalIndicator as IDropDownItem, "ddIndicator", true, selectedItem, null, null);

            string response = @"<response><result>" + AJAXTools.EncodeForXML(result) + @"</result></response>";

            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();
        }

        // Deletes card item by ajax request
        private void JSDeleteCardItem()
        {
            if (WorkplaceConditionsCardId == 0)
            {
                if (this.GetUIItemAccessLevel("HS_ADDWCONDCARD_CARDITEMS") != UIAccessLevel.Enabled || this.GetUIItemAccessLevel("HS_WCONDCARDS") != UIAccessLevel.Enabled)
                    RedirectAjaxAccessDenied();
            }
            else
            {
                if (this.GetUIItemAccessLevel("HS_EDITWCONDCARD_CARDITEMS") != UIAccessLevel.Enabled || this.GetUIItemAccessLevel("HS_WCONDCARDS") != UIAccessLevel.Enabled)
                    RedirectAjaxAccessDenied();
            }

            int cardItemId = int.Parse(Request.Form["CardItemID"]);

            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "HS_WorkplaceCondtitionsCard");

                if (!WorkplaceConditionsCardItemUtil.DeleteWorkplaceConditionsCardItem(WorkplaceConditionsCardId, cardItemId, CurrentUser, change))
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

        // hidden button for forced refresh of card items table
        protected void btnHdnRefreshCardItems_Click(object sender, EventArgs e)
        {
            LoadCardItemsTable();
        }

        // fill City label with appropriate value according to choosen military unit
        protected void RefreshCity()
        {
            string cityName = "";
            if (!string.IsNullOrEmpty(musMilitaryUnit.SelectedValue))
            {
                MilitaryUnit militaryUnit = MilitaryUnitUtil.GetMilitaryUnit(int.Parse(musMilitaryUnit.SelectedValue), CurrentUser);

                if (militaryUnit != null && militaryUnit.City != null)
                    cityName = militaryUnit.City.CityName;                                    
            }

            txtCity.Text = cityName;
        }

        //Navigate back to the ManageProtocols screen
        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (FromHome != 1)
                Response.Redirect("~/ContentPages/ManageWorkplaceConditionsCards.aspx");
            else
                Response.Redirect("~/ContentPages/Home.aspx");
        }

        //Set label text
        private void SetLabelValueText()
        {
            this.lblMilitaryUnit.Text = this.MilitaryUnitLabel + ":";
            this.hfMilitaryUnitLabel.Value = this.MilitaryUnitLabel;
        }
    }
}
