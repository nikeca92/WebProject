using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using PMIS.Common;
using PMIS.HealthSafety.Common;

namespace PMIS.HealthSafety.ContentPages
{
    public partial class AddEditCommittee : HSPage
    {
        private bool tableEditPermission = true;
        private bool showTable = true;

        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "HS_COMMITTEE";
            }
        }

        //Getter/Setter of the ID of the displayed committee(0 - if new)
        private int CommitteeId
        {
            get
            {
                int committeeId = 0;
                //gets CommitteeID either from the hidden field on the page or from page url
                if (String.IsNullOrEmpty(this.hfCommitteeID.Value)
                    || this.hfCommitteeID.Value == "0")
                {
                    if (Request.Params["CommitteeID"] != null)
                        int.TryParse(Request.Params["CommitteeID"].ToString(), out committeeId);

                    //sets CommitteeID in hidden field on the page in order to be accessible in javascript
                    this.hfCommitteeID.Value = committeeId.ToString();
                }
                else
                {
                    Int32.TryParse(this.hfCommitteeID.Value, out committeeId);
                }

                return committeeId;
            }
            set { this.hfCommitteeID.Value = value.ToString(); }
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
            HighlightMenuItems("Committees_Add", "Committees");

            //Hide the navigation buttons
            HideNavigationControls(btnBack);

            // Prevent showing "Save changes" dialog box
            LnkForceNoChangesCheck(btnSave);
            LnkForceNoChangesCheck(btnNew);

            //Process the ajax request for saving committee members
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveCommitteeMember")
            {
                JSSaveCommitteeMember();
                return;
            }

            //Process the ajax request for the corresponding person name and id for the light box(on enter ident number)
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetPerson")
            {
                JSGetPerson();
                return;
            }

            //Process ajax request for deleting of commitee member
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteCommitteeMember")
            {
                JSDeleteCommitteeMember();
                return;
            }

            jsMilitaryUnitSelectorDiv.InnerHtml = @"<script src=""" + Page.ClientScript.GetWebResourceUrl(typeof(MilitaryUnitSelector.MilitaryUnitSelector), "MilitaryUnitSelector.MilitaryUnitSelector.js") + @""" type=""text/javascript""></script>";

            SetupPageUI(); //setup user interface elements according to rights of the user's role
            SetBtnNew(); //enable or disable button for adding new commitee members, according to mode of work(add or edir commitee)
            SetupDatePickers(); //Setup any calendar control on the screen 8

            if (!IsPostBack)
            {
                SetPageName(); // sets page titles according to mode of work(add or edit commitee)
                LoadDropDowns(); //fills dropdowns on the page with values
                LoadData(); // fills the controls on the page, displaying properties of the commitee
                LoadCommitteeMembersTable(); // loads html table in the page with commitee members to this commitee
                SetBtnPrint(); // Set visibility of the print button
            }

            lblMessage.Text = ""; // clean message of commitee operations
            SetCommitteeMemberMessage(); //display message from ajax operations on commitee members, if exist            
        }

        // Set page titles according to mode of work(add or edit commitee)
        private void SetPageName()
        {
            if (this.CommitteeId > 0)
            {
                lblHeaderTitle.InnerHtml = "Редактиране на комитет или група";
            }
            else
            {
                lblHeaderTitle.InnerHtml = "Добавяне на комитет или група";
            }

            Page.Title = lblHeaderTitle.InnerHtml;
        }

        // Set visibility of print button
        private void SetBtnPrint()
        {
            // if the commitee is new and not saved yet, it is not allowed to print it
            if (this.CommitteeId == 0)
            {
                this.btnPrint.Visible = false;
            }
            else
            {
                this.btnPrint.Visible = true;
            }
        }

        // Enable or disable button for adding new commitee members, according to mode of work(add or edir committee)
        private void SetBtnNew()
        {
            if (tableEditPermission)
            {
                // if the commitee is new and not saved yet, it is not allowed to add new members to commitee
                if (CommitteeId == 0)
                {
                    DisableButton(btnNew);
                }
                else
                {
                    EnableButton(btnNew);
                }
            }
        }

        // Setup any date picker control on the page by setting the CSS of the target input
        // Note that the date picker CSS is common
        // This makes the calendar control to appears on the page next to each input
        private void SetupDatePickers()
        {
            txtTrainingDate.CssClass = "RequiredInputField " + CommonFunctions.DatePickerCSS();
        }

        // Setup user interface elements according to rights of the user's role
        private void SetupPageUI()
        {
            UIAccessLevel l;

            if (CommitteeId == 0) // add mode of page
            {
                bool screenHidden = (this.GetUIItemAccessLevel("HS_ADDCOMMITTEE") != UIAccessLevel.Enabled) ||
                                      (this.GetUIItemAccessLevel("HS_COMMITTEE") != UIAccessLevel.Enabled);

                bool screenDisabled = (this.GetUIItemAccessLevel("HS_ADDCOMMITTEE") == UIAccessLevel.Disabled) ||
                                      (this.GetUIItemAccessLevel("HS_COMMITTEE") == UIAccessLevel.Disabled);

                if (screenHidden)
                    RedirectAccessDenied();

                if (screenDisabled)
                {
                    this.pageDisabledControls.Add(btnSave);
                    this.pageDisabledControls.Add(btnNew);
                    tableEditPermission = false;
                }

                l = this.GetUIItemAccessLevel("HS_ADDCOMMITTEE_TYPE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(ddCommitteeType);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(ddCommitteeType);
                }

                l = this.GetUIItemAccessLevel("HS_ADDCOMMITTEE_MILFORCETYPE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblMilitaryForceType);
                    this.pageDisabledControls.Add(ddMilitaryForceType);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblMilitaryForceType);
                    this.pageHiddenControls.Add(ddMilitaryForceType);
                }

                l = this.GetUIItemAccessLevel("HS_ADDCOMMITTEE_MILUNIT");
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

            }
            else // edit mode of page
            {
                bool screenHidden = (this.GetUIItemAccessLevel("HS_EDITCOMMITTEE") == UIAccessLevel.Hidden) ||
                                      (this.GetUIItemAccessLevel("HS_COMMITTEE") == UIAccessLevel.Hidden);

                bool screenDisabled = (this.GetUIItemAccessLevel("HS_EDITCOMMITTEE") == UIAccessLevel.Disabled) ||
                                      (this.GetUIItemAccessLevel("HS_COMMITTEE") == UIAccessLevel.Disabled);

                if (screenHidden)
                    RedirectAccessDenied();

                if (screenDisabled)
                {
                    this.pageDisabledControls.Add(btnSave);
                    this.pageDisabledControls.Add(btnNew);
                    tableEditPermission = false;
                }

                l = this.GetUIItemAccessLevel("HS_EDITCOMMITTEE_TYPE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(ddCommitteeType);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(ddCommitteeType);
                }

                l = this.GetUIItemAccessLevel("HS_EDITCOMMITTEE_MILFORCETYPE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblMilitaryForceType);
                    this.pageDisabledControls.Add(ddMilitaryForceType);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblMilitaryForceType);
                    this.pageHiddenControls.Add(ddMilitaryForceType);
                }

                l = this.GetUIItemAccessLevel("HS_EDITCOMMITTEE_MILUNIT");
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

                l = this.GetUIItemAccessLevel("HS_EDITCOMMITTEE_MEMBERS");
                if (l == UIAccessLevel.Disabled)
                {
                    tableEditPermission = false;
                    DisableButton(btnNew);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    HideControl(btnNew);
                    showTable = false;
                    tableEditPermission = false;
                }

            }


            // client controls to disable and hide when add mode of work
            List<string> disabledClientControls = new List<string>();
            List<string> hiddenClientControls = new List<string>();

            l = this.GetUIItemAccessLevel("HS_ADDTRAINHIST_TRAININGDATE");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblTrainingDate");
                disabledClientControls.Add(txtTrainingDate.ClientID);
                pageDisabledControls.Add(txtTrainingDate);
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblTrainingDate");
                hiddenClientControls.Add(txtTrainingDate.ClientID);
                pageHiddenControls.Add(txtTrainingDate);
            }

            l = this.GetUIItemAccessLevel("HS_ADDTRAINHIST_TRAININGYEAR");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblTrainingYear");
                disabledClientControls.Add("txtTrainingYear");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblTrainingYear");
                hiddenClientControls.Add("txtTrainingYear");
            }

            l = this.GetUIItemAccessLevel("HS_ADDTRAINHIST_TRAININGDESC");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblTrainingDesc");
                disabledClientControls.Add("txtTrainingDesc");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblTrainingDesc");
                hiddenClientControls.Add("txtTrainingDesc");
            }

            l = this.GetUIItemAccessLevel("HS_ADDTRAINHIST_LEGALREF");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblLegalRef");
                disabledClientControls.Add("txtLegalRef");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblLegalRef");
                hiddenClientControls.Add("txtLegalRef");
            }

            // list of controls are stored in special hidden field on the page, which is used to actually disable and hide, when lightbox is opening, according to mode of work
            hdnAddDisabledControls.Value = string.Join(",", disabledClientControls.ToArray());
            hdnAddHiddenControls.Value = string.Join(",", hiddenClientControls.ToArray());

            // client controls to disable and hide when edit mode of work
            disabledClientControls = new List<string>();
            hiddenClientControls = new List<string>();

            l = this.GetUIItemAccessLevel("HS_EDITTRAINHIST_TRAININGDATE");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblTrainingDate");
                disabledClientControls.Add(txtTrainingDate.ClientID);
                pageDisabledControls.Add(txtTrainingDate);
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblTrainingDate");
                hiddenClientControls.Add(txtTrainingDate.ClientID);
                pageHiddenControls.Add(txtTrainingDate);
            }

            l = this.GetUIItemAccessLevel("HS_EDITTRAINHIST_TRAININGYEAR");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblTrainingYear");
                disabledClientControls.Add("txtTrainingYear");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblTrainingYear");
                hiddenClientControls.Add("txtTrainingYear");
            }

            l = this.GetUIItemAccessLevel("HS_EDITTRAINHIST_TRAININGDESC");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblTrainingDesc");
                disabledClientControls.Add("txtTrainingDesc");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblTrainingDesc");
                hiddenClientControls.Add("txtTrainingDesc");
            }

            l = this.GetUIItemAccessLevel("HS_EDITTRAINHIST_LEGALREF");
            if (l == UIAccessLevel.Disabled)
            {
                disabledClientControls.Add("lblLegalRef");
                disabledClientControls.Add("txtLegalRef");
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("lblLegalRef");
                hiddenClientControls.Add("txtLegalRef");
            }

            // list of controls are stored in special hidden field on the page, which is used to actually disable and hide, when lightbox is opening, according to mode of work
            hdnEditDisabledControls.Value = string.Join(",", disabledClientControls.ToArray());
            hdnEditHiddenControls.Value = string.Join(",", hiddenClientControls.ToArray());

        }

        // Display message from ajax operations on committee members, if exist
        private void SetCommitteeMemberMessage()
        {
            if (hfMsg.Value == "FailCommitteeMemberSave")
            {
                lblCommitteeMemberMessage.Text = "Неуспешен запис на член!";
                lblCommitteeMemberMessage.CssClass = "ErrorText";
            }
            else if (hfMsg.Value == "SuccessCommitteeMemberSave")
            {
                lblCommitteeMemberMessage.Text = "Успешен запис на член!";
                lblCommitteeMemberMessage.CssClass = "SuccessText";
            }
            else if (hfMsg.Value == "FailCommmitteeMemberDelete")
            {
                lblCommitteeMemberMessage.Text = "Неуспешно изтриване на член!";
                lblCommitteeMemberMessage.CssClass = "ErrorText";
            }
            else if (hfMsg.Value == "SuccessCommmitteeMemberDelete")
            {
                lblCommitteeMemberMessage.Text = "Успешно изтриване на член!";
                lblCommitteeMemberMessage.CssClass = "SuccessText";
            }
            else
            {
                lblCommitteeMemberMessage.Text = "";
            }

            hfMsg.Value = ""; //clean message form ajax operations
        }

        // Populate all dropdowns on the screen
        private void LoadDropDowns()
        {
            this.ddCommitteeType.DataSource = GTableItemUtil.GetAllGTableItemsByTableName("ComitteeTypes", ModuleKey, 1, 0, 0, CurrentUser);
            this.ddCommitteeType.DataTextField = "TableValue";
            this.ddCommitteeType.DataValueField = "TableKey";
            this.ddCommitteeType.DataBind();
            this.ddCommitteeType.Items.Insert(0, ListItems.GetOptionChooseOne());

            this.ddMilitaryForceType.DataSource = MilitaryForceTypeUtil.GetAllMilitaryForceTypes(CurrentUser);
            this.ddMilitaryForceType.DataTextField = "MilitaryForceTypeName";
            this.ddMilitaryForceType.DataValueField = "MilitaryForceTypeId";
            this.ddMilitaryForceType.DataBind();
            this.ddMilitaryForceType.Items.Insert(0, ListItems.GetOptionChooseOne());
        }

        // Populate committee properites on the page
        private void LoadData()
        {
            if (CommitteeId > 0)
            {
                Committee committee = CommitteeUtil.GetCommittee(CommitteeId, CurrentUser);

                if (committee != null)
                {
                    if (committee.CommitteeTypeId.HasValue)
                        ddCommitteeType.SelectedValue = committee.CommitteeTypeId.Value.ToString();

                    if (committee.MilitaryForceTypeId.HasValue)
                        ddMilitaryForceType.SelectedValue = committee.MilitaryForceTypeId.Value.ToString();

                    if (committee.MilitaryUnitId.HasValue)
                    {
                        musMilitaryUnit.SelectedValue = committee.MilitaryUnitId.Value.ToString();
                        musMilitaryUnit.SelectedText = committee.MilitaryUnit.DisplayTextForSelection;
                    }
                }
            }
        }

        // Gathers commitee properties from the page controls
        private Committee CollectData()
        {
            Committee committee = new Committee(CurrentUser);

            committee.CommitteeId = CommitteeId;
            committee.CommitteeTypeId = ddCommitteeType.SelectedValue != "-1" ? (int?)int.Parse(ddCommitteeType.SelectedValue) : null;
            committee.MilitaryForceTypeId = ddMilitaryForceType.SelectedValue != "-1" ? (int?)int.Parse(ddMilitaryForceType.SelectedValue) : null;
            committee.MilitaryUnitId = musMilitaryUnit.SelectedValue != "-1" ? (int?)int.Parse(musMilitaryUnit.SelectedValue) : null;

            return committee; // return loaded with values commitee-object
        }

        // Loads html table in the page with committee members to this committee
        private void LoadCommitteeMembersTable()
        {
            this.divCommitteeMembers.InnerHtml = "";
            if (showTable)
            {
                List<CommitteeMember> committeeMembers = CommitteeMemberUtil.GetAllCommitteeMembersByCommittee(CommitteeId, CurrentUser); // gets list of all protocol items related to this protocol
                this.divCommitteeMembers.InnerHtml = this.GenerateCommitteeMembersTable(committeeMembers); //generate and display html with protocol items table on the page
            }
        }

        // Generates html table from list of committee members
        private string GenerateCommitteeMembersTable(List<CommitteeMember> committeeMembers)
        {
            bool isTrainingYearHidden = this.GetUIItemAccessLevel("HS_EDITTRAINHIST_TRAININGYEAR") == UIAccessLevel.Hidden;
            bool isLegalRefHidden = this.GetUIItemAccessLevel("HS_EDITTRAINHIST_LEGALREF") == UIAccessLevel.Hidden;

            StringBuilder sb = new StringBuilder();
            sb.Append("<table id='committeeMembersTable' name='committeeMembersTable' class='CommonHeaderTable' style='text-align: left;' border='1px'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style=\"min-width: 30px;\"></th>");
            sb.Append("<th style=\"min-width: 100px;\">ЕГН</th>");
            sb.Append("<th style=\"min-width: 100px;\">Звание</th>");
            sb.Append("<th style=\"min-width: 150px;\">Име</th>");
            if (!isTrainingYearHidden)
                sb.Append("<th style=\"width: 80px;\">Година на обучение</th>");
            if (!isLegalRefHidden)
                sb.Append("<th style=\"width: 200px;\">Наредба</th>");
            if (tableEditPermission)
                sb.Append("<th style=\"min-width: 20px;\"></th>");
            sb.Append("</tr>");
            sb.Append("</thead>");
            int counter = 1;

            if (committeeMembers.Count > 0)
            {
                sb.Append("<tbody>");
            }

            foreach (CommitteeMember member in committeeMembers)
            {
                Training training = TrainingUtil.GetLastTrainingByPerson(member.PersonId.Value, CurrentUser);

                string trainingHTML = "";

                if (this.GetUIItemAccessLevel("HS_TRAININGHISTORY") != UIAccessLevel.Hidden)
                {

                    trainingHTML += "<img src='../Images/user_find.png' alt='История на обученията' title='История на обученията' class='GridActionIcon' style='float:right;' onclick='ViewTrainingHistory(" + member.PersonId.ToString() + ");' />";

                    if (this.GetUIItemAccessLevel("HS_ADDTRAINHIST") == UIAccessLevel.Enabled && this.GetUIItemAccessLevel("HS_TRAININGHISTORY") == UIAccessLevel.Enabled)
                    {
                        trainingHTML += "<img src='../Images/user_add.png' alt='Добавяне на обучение' title='Добавяне на обучение' class='GridActionIcon' style='float:right;' onclick='ShowTrainingLightBox(0," + member.PersonId.ToString() + ");' />";
                    }
                }

                sb.Append("<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + "'>");
                sb.Append("<td>" + counter + "</td>");
                sb.Append("<td>" + member.Person.IdentNumber + "</td>");
                sb.Append("<td>" + (member.Person.MilitaryRank != null ? member.Person.MilitaryRank.LongName : "") + "</td>");
                sb.Append("<td>" + member.Person.FullName + "</td>");
                if (!isTrainingYearHidden)
                    sb.Append("<td>" + (training != null && training.TrainingYear.HasValue ? training.TrainingYear.Value.ToString() : "") + trainingHTML + "</td>");
                if (!isLegalRefHidden)
                    sb.Append("<td>" + (training != null ? training.LegalRef : "") + "</td>");

                // add delete icon(button), which calls javascript functionality for necessary actions
                if (tableEditPermission)
                    sb.Append(@"<td><img border='0' src='../Images/delete.png' alt='Изтриване' title='Изтриване' class='GridActionIcon' onclick='DeleteCommitteeMember(" + member.CommitteeMemberId.ToString() + @");'/></td>");

                sb.Append("</tr>");
                counter++;
            }

            if (committeeMembers.Count > 0)
            {
                sb.Append("</tbody>");
            }

            sb.Append("</table>");

            return sb.ToString();
        }

        // Saves committee
        private void SaveData()
        {
            Committee committee = CollectData(); // gathers committee properties from page controls

            //Track the changes into the Audit Trail 
            Change change = new Change(CurrentUser, "HS_Committees");

            if (CommitteeUtil.SaveCommittee(committee, CurrentUser, change))
            {
                if (CommitteeId == 0)
                {
                    SetLocationHash("AddEditCommittee.aspx?CommitteeID=" + committee.CommitteeId.ToString());
                }

                CommitteeId = committee.CommitteeId;

                this.lblMessage.CssClass = "SuccessText";
                this.lblMessage.Text = "Данните са записани успешно";

                hdnSavedChanges.Value = "True";
                SetupPageUI();
                LoadCommitteeMembersTable();

                this.SetBtnPrint();
            }
            else
            {
                this.lblMessage.CssClass = "ErrorText";
                this.lblMessage.Text = "Възникна проблем при записа на данните";
            }

            change.WriteLog();
        }

        // Saves committee
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
            SetBtnNew();
            SetPageName(); // sets page titles according to mode of work(add or edit committee)            
        }

        // Saves committee member from ajax request
        private void JSSaveCommitteeMember()
        {
            string resultMsg = "";


            int committeeID = int.Parse(Request.Form["CommitteeID"]);
            int personID = int.Parse(Request.Form["PersonID"]);

            if (!CommitteeMemberUtil.IsCommitteeMemberExist(personID, committeeID, CurrentUser))
            {
                CommitteeMember committeeMember = new CommitteeMember(CurrentUser);
                committeeMember.PersonId = personID;

                //Track the changes into the Audit Trail 
                Change change = new Change(CurrentUser, "HS_Committees");

                resultMsg = CommitteeMemberUtil.SaveCommitteeMember(committeeID, committeeMember, CurrentUser, change) ? AJAXTools.OK : AJAXTools.ERROR;

                change.WriteLog();
            }
            else
            {
                resultMsg = "Този човек вече е добавен към този комитет (група)";
            }


            string response = @"<response>" + AJAXTools.EncodeForXML(resultMsg) + "</response>";
            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();
        }

        // Get the corresponding person name and id for given ident number(by ajax request)
        private void JSGetPerson()
        {
            string identNumber = Request.Form["IdentNumber"].ToString();

            string res = "";
            string data = "";

            try
            {
                Person person = PersonUtil.GetPersonByIdentNumber(identNumber, CurrentUser);
                if (person != null)
                {
                    res = AJAXTools.OK;
                    data = "<PersonID>" + person.PersonId.ToString() + "</PersonID>";
                    data += "<Name>" + AJAXTools.EncodeForXML(person.FullName) + "</Name>";
                }
                else
                {
                    res = "NOTFOUND";
                }
            }
            catch
            {
                res = AJAXTools.ERROR;
            }

            res = "<response>" + res + "</response>";

            string response = res + data;
            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();
        }

        // Deletes committee member by ajax request
        private void JSDeleteCommitteeMember()
        {

            if (this.GetUIItemAccessLevel("HS_EDITCOMMITTEE_MEMBERS") != UIAccessLevel.Enabled || this.GetUIItemAccessLevel("HS_COMMITTEE") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            int committeeID = int.Parse(Request.Form["CommitteeID"]);
            int commmitteeMemberID = int.Parse(Request.Form["CommmitteeMemberID"]);

            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "HS_Committees");

                if (!CommitteeMemberUtil.DeleteCommitteeMember(commmitteeMemberID, committeeID, CurrentUser, change))
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

        // hidden button for forced refresh of committee members table
        protected void btnHdnRefreshProtocolItems_Click(object sender, EventArgs e)
        {
            LoadCommitteeMembersTable();
        }

        //Navigate back to the ManageCommittees screen
        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (FromHome != 1)
                Response.Redirect("~/ContentPages/ManageCommittees.aspx");
            else
                Response.Redirect("~/ContentPages/Home.aspx");
        }

        private void SetLabelValueText()
        {
            this.lblMilitaryUnit.Text = this.MilitaryUnitLabel + ":";
            this.hfMilitaryUnitLabel.Value = this.MilitaryUnitLabel;
        }
    }
}
