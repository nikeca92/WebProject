using System;
using System.Collections.Generic;
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
using System.Drawing;

using PMIS.Common;
using PMIS.HealthSafety.Common;

namespace PMIS.HealthSafety.ContentPages
{
    public partial class AddEditTrainingHistory : HSPage
    {
        //Get the config setting that says how many rows per page should be dispayed in the grid
        private int pageLength = int.Parse(Config.GetWebSetting("RowsPerPage"));
        //Stores information about how many pages are in the grid
        private int maxPage;

        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "HS_TRAININGHISTORY";
            }
        }

        //Getter/Setter of the ID of the displayed person training history(0 - if new)
        private int PersonId
        {
            get
            {
                int personId = 0;
                //gets ProtocolID either from the hidden field on the page or from page url
                if (String.IsNullOrEmpty(this.hfPersonID.Value)
                    || this.hfPersonID.Value == "0")
                {
                    if (Request.Params["PersonID"] != null)
                        int.TryParse(Request.Params["PersonID"].ToString(), out personId);

                    //sets protocol ID in hidden field on the page in order to be accessible in javascript
                    this.hfPersonID.Value = personId.ToString();
                }
                else
                {
                    Int32.TryParse(this.hfPersonID.Value, out personId);
                }

                return personId;
            }
            set { this.hfPersonID.Value = value.ToString(); }
        }

        //This is a flag field that says if the screen is opened from the AddEditCommittee screen
        //This is used to navigate the user back to the AddEditCommittee screen when using the Back button
        private int FromAddEditCommittee
        {
            get
            {
                int fc = 0;
                if (String.IsNullOrEmpty(this.hfFromAddEditCommittee.Value)
                    || this.hfFromAddEditCommittee.Value == "0")
                {
                    if (Request.Params["FromCommittee"] != null)
                        int.TryParse(Request.Params["FromCommittee"].ToString(), out fc);

                    this.hfFromAddEditCommittee.Value = fc.ToString();
                }
                else
                {
                    Int32.TryParse(this.hfFromAddEditCommittee.Value, out fc);
                }

                return fc;
            }

            set
            {
                this.hfFromAddEditCommittee.Value = value.ToString();
            }
        }

        public string DateCssClass
        {
            get { return CommonFunctions.DatePickerCSS(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.GetUIItemAccessLevel("HS_TRAININGHISTORY") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            // Enable or disable button for adding new committees, according to rights of the user's role
            SetBtnNew();

            //Process the ajax request for properties of training(for displaying in the light box)
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetTraining")
            {
                JSGetTraining();
                return;
            }

            //Process the ajax request for saving trainings
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveTraining")
            {
                JSSaveTraining();
                return;
            }

            //Process the ajax request for deleting trainings
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteTraining")
            {
                JSDeleteTraining();
                return;
            }

            //Hilight the current page in the menu bar 
            HighlightMenuItems("Committees", "Committees_TrainingHistory");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar (not really used in this page)
            HideNavigationControls(btnBack);

            //Setup some basic styles on the screen
            SetupStyle();

            SetupPageUI(); //setup user interface elements according to rights of the user's role            
            SetupDatePickers(); //Setup any calendar control on the screen 

            int allRows = TrainingUtil.GetAllTrainingsByPersonCount(PersonId, CurrentUser);
            ////Get the number of rows and calculate the number of pages in the grid
            maxPage = pageLength == 0 ? 1 : allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            //When the page is loaded for the first time
            if (!IsPostBack)
            {
                LoadData(); // fills the controls on the page, displaying properties of the person

                //The default order is by Training date
                if (string.IsNullOrEmpty(hdnSortBy.Value))
                    hdnSortBy.Value = "1";

                SetBtnPrintTrainingHistory(); // Set visibility of the print button

                //Simulate clicking the Refresh button to load the grid initially
                btnRefresh_Click(btnRefresh, new EventArgs());
            }

            SetGridMessage(); //display message from ajax operations on trainings, if exist
        }             

        //Setup some styling on the page
        private void SetupStyle()
        {
            lblIdentNumber.Style.Add("vertical-align", "top");
            txtIdentNumber.Style.Add("vertical-align", "top");
            lblName.Style.Add("vertical-align", "top");
            txtName.Style.Add("vertical-align", "top");
            lblRank.Style.Add("vertical-align", "top");
            txtRank.Style.Add("vertical-align", "top");
        }

        // Setup any date picker control on the page by setting the CSS of the target input
        // Note that the date picker CSS is common
        // This makes the calendar control to appears on the page next to each input
        private void SetupDatePickers()
        {
            txtTrainingDate.CssClass = "RequiredInputField " + CommonFunctions.DatePickerCSS();         
        }

        // Set visibility of print button
        private void SetBtnPrintTrainingHistory()
        {
            // if the person is new and not saved yet, it is not allowed to print it (FOR COMPATIBILITY - on this page, this case doesn't exist
            if (PersonId == 0)
            {
                btnPrintTrainingHistory.Visible = false;
            }
            else
            {
                btnPrintTrainingHistory.Visible = true;
            }
        }

        // Setup user interface elements according to rights of the user's role
        private void SetupPageUI()
        {
            //if (this.GetUIItemAccessLevel("HS_ADDTRAINHIST") != UIAccessLevel.Enabled || this.GetUIItemAccessLevel("HS_TRAININGHISTORY") != UIAccessLevel.Enabled)
            //{
            //    this.pageDisabledControls.Add(btnNew);
            //}

            UIAccessLevel l;

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

        // Display message from ajax operations on trainings, if exist
        private void SetGridMessage()
        {
            if (hfMsg.Value == "FailTrainingSave")
            {
                lblGridMessage.Text = "Неуспешен запис на обучение!";
                lblGridMessage.CssClass = "ErrorText";
            }
            else if (hfMsg.Value == "SuccessTrainingSave")
            {
                lblGridMessage.Text = "Успешен запис на обучение!";
                lblGridMessage.CssClass = "SuccessText";
            }
            else if (hfMsg.Value == "FailTrainingDelete")
            {
                lblGridMessage.Text = "Неуспешно изтриване на обучение!";
                lblGridMessage.CssClass = "ErrorText";
            }
            else if (hfMsg.Value == "SuccessTrainingDelete")
            {
                lblGridMessage.Text = "Успешно изтриване на обучение!";
                lblGridMessage.CssClass = "SuccessText";
            }
            else
            {
                lblGridMessage.Text = "";
            }

            hfMsg.Value = ""; //clean message form ajax operations
        }

        // Populate person's properites on the page
        private void LoadData()
        {
            if (PersonId > 0)
            {
                Person person = PersonUtil.GetPerson(PersonId, CurrentUser);

                if (person != null)
                {
                    txtIdentNumber.Text = person.IdentNumber;
                    txtName.Text = person.FullName;
                    txtRank.Text = person.MilitaryRank != null ? person.MilitaryRank.LongName : "";                        
                }                
            }
        }    

        //Refresh the data grid
        private void LoadTrainings()
        {
            string html = "";

            //Collect the filters(not existing on this page), the order control and the paging control data from the page
            int orderBy;
            if (!int.TryParse(hdnSortBy.Value, out orderBy))
                orderBy = 0;

            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;            

            //Get the list of Trainings according to the specified filters, order and paging
            List<Training> trainings = TrainingUtil.GetAllTrainingsByPerson(PersonId, orderBy, pageIdx, pageLength, CurrentUser);

            //No data found
            if (trainings.Count == 0)
            {
                html = "<span>Няма намерени резултати</span>";
            }
            //If there is data then generate dynamically the HTML for the data grid
            else 
            {
               bool IsTrainingDateHidden = this.GetUIItemAccessLevel("HS_EDITTRAINHIST_TRAININGDATE") == UIAccessLevel.Hidden;
               bool IsTrainingYearHidden = this.GetUIItemAccessLevel("HS_EDITTRAINHIST_TRAININGYEAR") == UIAccessLevel.Hidden;
               bool IsTrainingDescHidden = this.GetUIItemAccessLevel("HS_EDITTRAINHIST_TRAININGDESC") == UIAccessLevel.Hidden;
               bool IsLegalRefHidden = this.GetUIItemAccessLevel("HS_EDITTRAINHIST_LEGALREF") == UIAccessLevel.Hidden;

                string headerStyle = "vertical-align: bottom;";
                int orderCol = (orderBy > 100 ? orderBy - 100 : orderBy);
                string img = orderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
                string[] arrOrderCol = {"", "", "", ""};
                arrOrderCol[orderCol - 1] = @"<div style='position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

                //Setup the header of the grid
                html = @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>" +
                               (IsTrainingDateHidden ? "" : (@"<th style='width: 70px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Дата" + arrOrderCol[0] + @"</th>")) +
                               (IsTrainingYearHidden ? "" : (@"<th style='width: 50px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>Година" + arrOrderCol[1] + @"</th>")) +
                               (IsTrainingDescHidden ? "" : (@"<th style='width: 250px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>Обучение" + arrOrderCol[2] + @"</th>")) +
                               (IsLegalRefHidden ? "" : (@"<th style='width: 250px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(4);'>Наредба" + arrOrderCol[3] + @"</th>")) +
                               @"<th style='width: 50px;" + headerStyle + @"'>&nbsp;</th>
                            </tr>
                         </thead>";

                int counter = 1;

                //Iterate through all items and add them into the grid
                foreach (Training training in trainings)
                {
                    string cellStyle = "vertical-align: top;";
                   
                    string editHTML = "";
                    string deleteHTML = "";

                    if (this.GetUIItemAccessLevel("HS_EDITTRAINHIST") == UIAccessLevel.Enabled && this.GetUIItemAccessLevel("HS_TRAININGHISTORY") == UIAccessLevel.Enabled)
                        editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='ShowTrainingLightBox(" + training.TrainingId.ToString() + ");' />";

                    if (this.GetUIItemAccessLevel("HS_DELETETRAINHIST") == UIAccessLevel.Enabled && this.GetUIItemAccessLevel("HS_TRAININGHISTORY") == UIAccessLevel.Enabled)
                        deleteHTML = @"<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на това обучение' class='GridActionIcon' onclick=""DeleteTraining(" + training.TrainingId.ToString() + @");"" />";

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='" + cellStyle + @"'>" + ((pageIdx - 1) * pageLength + counter).ToString() + @"</td>"+
                                 (IsTrainingDateHidden ? "" : (@"<td style='" + cellStyle + @"'>" + CommonFunctions.FormatDate(training.TrainingDate) + @"</td>")) +
                                 (IsTrainingYearHidden ? "" : (@"<td style='" + cellStyle + @"'>" + (training.TrainingYear.HasValue ? training.TrainingYear.Value.ToString() : "") + @"</td>")) +
                                 (IsTrainingDescHidden ? "" : (@"<td style='" + cellStyle + @"'>" + CommonFunctions.ReplaceNewLinesInString(training.TrainingDesc) + @"</td>")) +
                                 (IsLegalRefHidden ? "" : (@"<td style='" + cellStyle + @"'>" + CommonFunctions.ReplaceNewLinesInString(training.LegalRef) + @"</td>")) +
                                 @"<td style='" + cellStyle + @"'>" + editHTML + deleteHTML + @"</td>
                              </tr>";

                    counter++;
                }

                html += "</table>";
            }

            //Put the generated grid on the page
            pnlTrainingsGrid.InnerHtml = html;

            //Refresh the paging button according to the current position
            SetImgBtns();
            lblPagination.Text = " | " + hdnPageIdx.Value + " от " + maxPage.ToString() + " | ";
            txtGotoPage.Text = "";    
        }

        //Refresh the data grid
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            hdnPageIdx.Value = "1";
            LoadTrainings();
        }

        //Go to the first page and refresh the grid
        protected void btnFirst_Click(object sender, EventArgs e)
        {
            hdnPageIdx.Value = "1";
            LoadTrainings();
        }

        //Go to the previous page and refresh the grid
        protected void btnPrev_Click(object sender, EventArgs e)
        {
            int page = int.Parse(hdnPageIdx.Value);

            if (page > 1)
            {
                page--;
                hdnPageIdx.Value = page.ToString();

                LoadTrainings();
            }
        }

        //Go to the next page and refresh the grid
        protected void btnNext_Click(object sender, EventArgs e)
        {
            int page = int.Parse(hdnPageIdx.Value);

            if (page < maxPage)
            {
                page++;
                hdnPageIdx.Value = page.ToString();

                LoadTrainings();
            }
        }

        //Go to the last page and refresh the grid
        protected void btnLast_Click(object sender, EventArgs e)
        {
            hdnPageIdx.Value = maxPage.ToString();
            LoadTrainings();
        }

        //Go to a specific page (entered by the user) and refresh the grid
        protected void btnGoto_Click(object sender, EventArgs e)
        {
            int gotoPage;
            if (int.TryParse(txtGotoPage.Text, out gotoPage) && gotoPage > 0 && gotoPage <= maxPage)
            {
                hdnPageIdx.Value = gotoPage.ToString();
                LoadTrainings();
            }
        }

        //Refresh the paging image buttons
        private void SetImgBtns()
        {
            int page = int.Parse(hdnPageIdx.Value);

            btnFirst.Enabled = true;
            btnPrev.Enabled = true;
            btnLast.Enabled = true;
            btnNext.Enabled = true;
            btnFirst.ImageUrl = "../Images/ButtonFirst.png";
            btnPrev.ImageUrl = "../Images/ButtonPrev.png";
            btnLast.ImageUrl = "../Images/ButtonLast.png";
            btnNext.ImageUrl = "../Images/ButtonNext.png";

            if (page == 1)
            {
                btnFirst.Enabled = false;
                btnPrev.Enabled = false;
                btnFirst.ImageUrl = "../Images/ButtonFirstDisabled.png";
                btnPrev.ImageUrl = "../Images/ButtonPrevDisabled.png";
            }

            if (page == maxPage)
            {
                btnLast.Enabled = false;
                btnNext.Enabled = false;
                btnLast.ImageUrl = "../Images/ButtonLastDisabled.png";
                btnNext.ImageUrl = "../Images/ButtonNextDisabled.png";
            }
        }

        // Get ajax-requested data for training(by TrainingID)
        private void JSGetTraining()
        {
            int trainingID = int.Parse(Request.Form["TrainingID"]);

            Training t = TrainingUtil.GetTraining(trainingID, CurrentUser);            

            string response = @"<response>                                   
                                    <TrainingDate>" + CommonFunctions.FormatDate(t.TrainingDate) + @"</TrainingDate>
                                    <TrainingYear>" + t.TrainingYear.ToString() + @"</TrainingYear>
                                    <TrainingDesc>" + AJAXTools.EncodeForXML(t.TrainingDesc) + @"</TrainingDesc>
                                    <LegalRef>" + AJAXTools.EncodeForXML(t.LegalRef) + @"</LegalRef>                                                          
                                </response>";

            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();
        }

        // Saves training from ajax request
        private void JSSaveTraining()
        {
            string resultMsg = "";

            int personID = int.Parse(Request.Form["PersonID"]);
            int trainingID = int.Parse(Request.Form["TrainingID"]);
            DateTime? trainingDate = !string.IsNullOrEmpty(Request.Form["TrainingDate"]) ? (DateTime?)CommonFunctions.ParseDate(Request.Form["TrainingDate"]) : null;
            int? trainingYear = !string.IsNullOrEmpty(Request.Form["TrainingYear"]) ? (int?)int.Parse(Request.Form["TrainingYear"]) : null;
            string trainingDesc = Request.Form["TrainingDesc"];
            string legalRef = Request.Form["LegalRef"];

            Training training = new Training();
            training.TrainingId = trainingID;
            training.TrainingDate = trainingDate;
            training.TrainingYear = trainingYear;
            training.TrainingDesc = trainingDesc;
            training.LegalRef = legalRef;

            //Track the changes into the Audit Trail 
            Change change = new Change(CurrentUser, "HS_TrainingHistory");

            resultMsg = TrainingUtil.SaveTraining(personID, training, CurrentUser, change) ? AJAXTools.OK : AJAXTools.ERROR;

            change.WriteLog();

            string response = @"<response>" + AJAXTools.EncodeForXML(resultMsg) + "</response>";
            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();
        }

        // Deletes training by ajax request
        private void JSDeleteTraining()
        {
            if (this.GetUIItemAccessLevel("HS_DELETETRAINHIST") != UIAccessLevel.Enabled || this.GetUIItemAccessLevel("HS_TRAININGHISTORY") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();          

            int trainingID = int.Parse(Request.Form["TrainingID"]);
            int personID = int.Parse(Request.Form["PersonID"]);

            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "HS_TrainingHistory");

                if (!TrainingUtil.DeleteTraining(trainingID, personID, CurrentUser, change))
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

        //Navigate back to the home screen
        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (FromAddEditCommittee != 0)
                Response.Redirect("~/ContentPages/AddEditCommittee.aspx?CommitteeID=" + FromAddEditCommittee);
            else
                Response.Redirect("~/ContentPages/ManageTrainingHistory.aspx");
        }

        private void SetBtnNew()
        {
            if (this.GetUIItemAccessLevel("HS_ADDTRAINHIST") == UIAccessLevel.Enabled && this.GetUIItemAccessLevel("HS_TRAININGHISTORY") == UIAccessLevel.Enabled)
            {
                EnableButton(btnNew);
            }
            else
            {
                if (this.GetUIItemAccessLevel("HS_ADDTRAINHIST") == UIAccessLevel.Hidden)
                {
                    HideControl(btnNew);
                }
                else
                {
                    DisableButton(btnNew);
                }
            }
        }
    }
}
