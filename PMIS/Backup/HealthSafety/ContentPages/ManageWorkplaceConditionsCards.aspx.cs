using System;
using System.Collections.Generic;

using PMIS.Common;
using PMIS.HealthSafety.Common;

namespace PMIS.HealthSafety.ContentPages
{
    public partial class ManageWorkplaceConditionsCards : HSPage
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
                return "HS_WCONDCARDS";
            }
        }
        private string MilitaryUnitLabel = CommonFunctions.GetLabelText("MilitaryUnit");

        protected void Page_Load(object sender, EventArgs e)
        {
            //Set Label Text
            SetLabelValueText();

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteCard")
            {
                JSDeleteCard();
                return;
            }

            if (this.GetUIItemAccessLevel("HS_WCONDCARDS") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            jsMilitaryUnitSelectorDiv.InnerHtml = @"<script src=""" + Page.ClientScript.GetWebResourceUrl(typeof(MilitaryUnitSelector.MilitaryUnitSelector), "MilitaryUnitSelector.MilitaryUnitSelector.js") + @""" type=""text/javascript""></script>";

            //Hilight the current page in the menu bar
            HighlightMenuItems("WorkplaceConditionsCards", "WorkplaceConditionsCards_Search");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            //Setup some basic styles on the screen
            SetupStyle();

            // Enable or disable button for adding new protocols, according to rights of the user's role
            SetBtnNew();

            //Collect the filter information to be able to pull the number of rows for this specific filter
            string militaryUnits = "";
            string cardNumber = "";
            string jobType = "";

            if (musMilitaryUnit.SelectedValue != ListItems.GetOptionAll().Value)
                militaryUnits = musMilitaryUnit.SelectedValue;

            cardNumber = txtCardNumber.Text;
            jobType = txtJobType.Text;


            int allRows = WorkplaceConditionsCardUtil.GetAllWorkplaceConditionsCardsCount(militaryUnits, cardNumber, jobType, CurrentUser);
            //Get the number of rows and calculate the number of pages in the grid
            maxPage = pageLength == 0 ? 1 : allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            //When the page is loaded for the first time
            if (!IsPostBack)
            {
                //Populate any drop-downs and list-boxes
                PopulateLists();

                //The default order is by module name
                if (string.IsNullOrEmpty(hdnSortBy.Value))
                    hdnSortBy.Value = "1";

                //Simulate clicking the Refresh button to load the grid initially
                btnRefresh_Click(btnRefresh, new EventArgs());
            }

            lblMessage.Text = "";
            lblGridMessage.Text = "";
        }

        private void SetBtnNew()
        {
            if (this.GetUIItemAccessLevel("HS_ADDWCONDCARD") == UIAccessLevel.Enabled && this.GetUIItemAccessLevel("HS_WCONDCARDS") == UIAccessLevel.Enabled)
            {
                EnableButton(btnNew);
            }
            else
            {
                if (this.GetUIItemAccessLevel("HS_ADDWCONDCARD") == UIAccessLevel.Hidden)
                {
                    HideControl(btnNew);
                }
                else
                {
                    DisableButton(btnNew);
                }
            }
        }

        //Populate all listboxes on the screen
        private void PopulateLists()
        {
            
        }

        //Setup some styling on the page
        private void SetupStyle()
        {
            lblMilitaryUnit.Style.Add("vertical-align", "top");
            lblCardNumber.Style.Add("vertical-align", "top");
            txtCardNumber.Style.Add("vertical-align", "top");
            lblJobType.Style.Add("vertical-align", "top");
            txtJobType.Style.Add("vertical-align", "top");
        }

        //Validate some field on the screen
        private bool ValidateData()
        {
            bool isDataValid = true;
            string errMsg = "";

            if (!isDataValid)
            {
                lblMessage.CssClass = "ErrorText";
                lblMessage.Text = errMsg;
            }

            return isDataValid;
        }

        //Refresh the data grid
        private void RefreshCards()
        {
            string html = "";


            //Collect the filters, the order control and the paging control data from the page
            int orderBy;
            if (!int.TryParse(hdnSortBy.Value, out orderBy))
                orderBy = 0;

            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            //Collect the filter information to be able to pull the number of rows for this specific filter
            string militaryUnits = "";
            string cardNumber = "";
            string jobType = "";

            if (musMilitaryUnit.SelectedValue != ListItems.GetOptionAll().Value)
            {
                militaryUnits = musMilitaryUnit.SelectedValue;
                this.hfМilitaryUnitId.Value = musMilitaryUnit.SelectedValue;
            }
            else
            {
                this.hfМilitaryUnitId.Value = ListItems.GetOptionAll().Value;
            }

            cardNumber = txtCardNumber.Text;
            this.hfCardNumber.Value = this.txtCardNumber.Text;

            jobType = txtJobType.Text;
            this.hfJobType.Value = this.txtJobType.Text;

            //Get the list of Roles according to the specified filters, order and paging
            List<WorkplaceConditionsCard> cards = WorkplaceConditionsCardUtil.GetAllWorkplaceConditionsCards(militaryUnits, cardNumber, jobType, orderBy, pageIdx, pageLength, CurrentUser);

            //No data found
            if (cards.Count == 0)
            {
                html = "<span>Няма намерени резултати</span>";
            }
            //If there is data then generate dynamically the HTML for the data grid
            else
            {
                string headerStyle = "vertical-align: bottom;";
                int orderCol = (orderBy > 100 ? orderBy - 100 : orderBy);
                string img = orderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
                string[] arrOrderCol = { "", "", "" };
                arrOrderCol[orderCol - 1] = @"<div style='position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

                //Setup the header of the grid
                html = @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='width: 130px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Номер на карта" + arrOrderCol[0] + @"</th>
                               <th style='width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>" + this.MilitaryUnitLabel + arrOrderCol[1] + @"</th>
                               <th style='width: 180px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>Работно място" + arrOrderCol[2] + @"</th>
                               <th style='width: 50px;" + headerStyle + @"'>&nbsp;</th>
                            </tr>
                         </thead>";

                int counter = 1;

                //Iterate through all items and add them into the grid
                foreach (WorkplaceConditionsCard card in cards)
                {
                    string cellStyle = "vertical-align: top;";

                    string deleteHTML = "";

                    if (card.CanDelete)
                    {
                        if (this.GetUIItemAccessLevel("HS_DELETEWCONDCARD") == UIAccessLevel.Enabled && this.GetUIItemAccessLevel("HS_WCONDCARDS") == UIAccessLevel.Enabled)
                            deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на тази карта' class='GridActionIcon' onclick='DeleteCard(" + card.WorkplaceConditionsCardId.ToString() + ");' />";
                    }

                    string editHTML = "";

                    if (this.GetUIItemAccessLevel("HS_EDITWCONDCARD") != UIAccessLevel.Hidden)
                        editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='EditCard(" + card.WorkplaceConditionsCardId.ToString() + ");' />";


                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='" + cellStyle + @"'>" + ((pageIdx - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + card.CardNumber + @"</td>
                                 <td style='" + cellStyle + @"'>" + (card.MilitaryUnit != null ? card.MilitaryUnit.DisplayTextForSelection : "") + @"</td>
                                 <td style='" + cellStyle + @"'>" + card.JobType + @"</td>                                 
                                 <td style='" + cellStyle + @"'>" + editHTML + deleteHTML + @"</td>
                              </tr>";

                    counter++;
                }

                html += "</table>";
            }

            //Put the generated grid on the page
            pnlCardsGrid.InnerHtml = html;

            //Refresh the paging button according to the current position
            SetImgBtns();
            lblPagination.Text = " | " + hdnPageIdx.Value + " от " + maxPage.ToString() + " | ";
            txtGotoPage.Text = "";

            //Set the message if there is a need (e.g. a deleted item)
            if (hdnRefreshReason.Value != "")
            {
                if (hdnRefreshReason.Value == "DELETED")
                {
                    lblGridMessage.Text = "Картата беше изтрита успешно";
                    lblGridMessage.CssClass = "SuccessText";
                }

                hdnRefreshReason.Value = "";
            }
        }

        //Refresh the data grid
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = "1";
                RefreshCards();
            }
        }

        //Go to the first page and refresh the grid
        protected void btnFirst_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = "1";
                RefreshCards();
            }
        }

        //Go to the previous page and refresh the grid
        protected void btnPrev_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                int page = int.Parse(hdnPageIdx.Value);

                if (page > 1)
                {
                    page--;
                    hdnPageIdx.Value = page.ToString();

                    RefreshCards();
                }
            }
        }

        //Go to the next page and refresh the grid
        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                int page = int.Parse(hdnPageIdx.Value);

                if (page < maxPage)
                {
                    page++;
                    hdnPageIdx.Value = page.ToString();

                    RefreshCards();
                }
            }
        }

        //Go to the last page and refresh the grid
        protected void btnLast_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = maxPage.ToString();
                RefreshCards();
            }
        }

        //Go to a specific page (entered by the user) and refresh the grid
        protected void btnGoto_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                int gotoPage;
                if (int.TryParse(txtGotoPage.Text, out gotoPage) && gotoPage > 0 && gotoPage <= maxPage)
                {
                    hdnPageIdx.Value = gotoPage.ToString();
                    RefreshCards();
                }
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

        //Navigate back to the home screen
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ContentPages/Home.aspx");
        }

        //Go to create a new record
        protected void btnNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ContentPages/AddEditWorkplaceConditionsCard.aspx");
        }

        //Delete protocol (ajax call)
        private void JSDeleteCard()
        {
            if (this.GetUIItemAccessLevel("HS_DELETEWCONDCARD") != UIAccessLevel.Enabled || this.GetUIItemAccessLevel("HS_WCONDCARDS") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            int cardId = int.Parse(Request.Form["CardID"]);

            string stat = "";
            string response = "";

            try
            {
                //Track the changes into the Audit Trail 
                Change change = new Change(CurrentUser, "HS_WorkplaceCondtitionsCard");

                WorkplaceConditionsCardUtil.DeleteWorkplaceConditionsCard(cardId, CurrentUser, change);

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
        private void SetLabelValueText()
        {
            this.lblMilitaryUnit.Text = this.MilitaryUnitLabel + ":";

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {            
            musMilitaryUnit.SelectedText = "";
            musMilitaryUnit.SelectedValue = ListItems.GetOptionAll().Value;
            txtCardNumber.Text = "";
            txtJobType.Text = "";
        }
    }
}
