using System;
using System.Collections.Generic;

using PMIS.Common;
using PMIS.HealthSafety.Common;

namespace PMIS.HealthSafety.ContentPages
{
    public partial class ManageCommittees : HSPage
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
                return "HS_COMMITTEE";
            }
        }

        private string MilitaryUnitLabel = CommonFunctions.GetLabelText("MilitaryUnit");

        protected void Page_Load(object sender, EventArgs e)
        {
            //Set Label Text
            SetLabelValueText();

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteCommittee")
            {
                JSDeleteCommittee();
                return;
            }

            if (this.GetUIItemAccessLevel("HS_COMMITTEE") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            jsMilitaryUnitSelectorDiv.InnerHtml = @"<script src=""" + Page.ClientScript.GetWebResourceUrl(typeof(MilitaryUnitSelector.MilitaryUnitSelector), "MilitaryUnitSelector.MilitaryUnitSelector.js") + @""" type=""text/javascript""></script>";

            //Hilight the current page in the menu bar
            HighlightMenuItems("Committees", "Committees_Search");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            //Setup some basic styles on the screen
            SetupStyle();

            // Enable or disable button for adding new committees, according to rights of the user's role
            SetBtnNew();

            //Collect the filter information to be able to pull the number of rows for this specific filter
            string committeeTypes = "";

            if (ddCommitteeType.SelectedValue != ListItems.GetOptionAll().Value)
                committeeTypes = ddCommitteeType.SelectedValue;

            string militaryForceTypes = "";

            if (ddMilitaryForceType.SelectedValue != ListItems.GetOptionAll().Value)
                militaryForceTypes = ddMilitaryForceType.SelectedValue;

            string militaryUnits = "";

            if (musMilitaryUnit.SelectedValue != ListItems.GetOptionAll().Value)
                militaryUnits = musMilitaryUnit.SelectedValue;

            int allRows = CommitteeUtil.GetAllCommitteesCount(committeeTypes, militaryForceTypes, militaryUnits, CurrentUser);
            //Get the number of rows and calculate the number of pages in the grid
            maxPage = pageLength == 0 ? 1 : allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            //When the page is loaded for the first time
            if (!IsPostBack)
            {
                //Populate any drop-downs and list-boxes
                PopulateLists();

                //The default order is by committee type
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
            if (this.GetUIItemAccessLevel("HS_ADDCOMMITTEE") == UIAccessLevel.Enabled && this.GetUIItemAccessLevel("HS_COMMITTEE") == UIAccessLevel.Enabled)
            {
                EnableButton(btnNew);
            }
            else
            {
                if (this.GetUIItemAccessLevel("HS_ADDCOMMITTEE") == UIAccessLevel.Hidden)
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
            PopulateCommitteeTypes();
            PopulateMilitaryForceTypes();
        }

        //Populate the committees types drop-down
        private void PopulateCommitteeTypes()
        {
            this.ddCommitteeType.DataSource = GTableItemUtil.GetAllGTableItemsByTableName("ComitteeTypes", ModuleKey, 1, 0, 0, CurrentUser);
            this.ddCommitteeType.DataTextField = "TableValue";
            this.ddCommitteeType.DataValueField = "TableKey";
            this.ddCommitteeType.DataBind();
            this.ddCommitteeType.Items.Insert(0, ListItems.GetOptionAll());
        }

        //Populate the military force types drop-down
        private void PopulateMilitaryForceTypes()
        {
            this.ddMilitaryForceType.DataSource = MilitaryForceTypeUtil.GetAllMilitaryForceTypes(CurrentUser);
            this.ddMilitaryForceType.DataTextField = "MilitaryForceTypeName";
            this.ddMilitaryForceType.DataValueField = "MilitaryForceTypeId";
            this.ddMilitaryForceType.DataBind();
            this.ddMilitaryForceType.Items.Insert(0, ListItems.GetOptionAll());
        }
     

        //Setup some styling on the page
        private void SetupStyle()
        {
            ddCommitteeType.Style.Add("vertical-align", "top");
            lblMilitaryForceType.Style.Add("vertical-align", "top");
            ddMilitaryForceType.Style.Add("vertical-align", "top");
            lblMilitaryUnit.Style.Add("vertical-align", "top");
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
        private void RefreshCommittees()
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
            string committeeTypes = "";

            if (ddCommitteeType.SelectedValue != ListItems.GetOptionAll().Value)            
                committeeTypes = ddCommitteeType.SelectedValue;                
            
            hfCommitteeTypeID.Value = committeeTypes;

            string militaryForceTypes = "";

            if (ddMilitaryForceType.SelectedValue != ListItems.GetOptionAll().Value)            
                militaryForceTypes = ddMilitaryForceType.SelectedValue;                
            
            hfMilitaryForceTypeID.Value = militaryForceTypes;

            string militaryUnits = "";

            if (musMilitaryUnit.SelectedValue != ListItems.GetOptionAll().Value)            
                militaryUnits = musMilitaryUnit.SelectedValue;
            
            hfMilitaryUnitID.Value = militaryUnits;

            //Get the list of Committees according to the specified filters, order and paging
            List<Committee> committees = CommitteeUtil.GetAllCommittees(committeeTypes, militaryForceTypes, militaryUnits, orderBy, pageIdx, pageLength, CurrentUser);

            //No data found
            if (committees.Count == 0)
            {
                html = "<span>Няма намерени резултати</span>";
                this.btnPrintAllCommittees.Visible = false;
            }
            //If there is data then generate dynamically the HTML for the data grid
            else 
            {
                this.btnPrintAllCommittees.Visible = true;

                string headerStyle = "vertical-align: bottom;";
                int orderCol = (orderBy > 100 ? orderBy - 100 : orderBy);
                string img = orderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
                string[] arrOrderCol = {"", "", ""};
                arrOrderCol[orderCol - 1] = @"<div style='position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

                //Setup the header of the grid
                html = @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='width: 220px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Тип" + arrOrderCol[0] + @"</th>
                               <th style='width: 130px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>Вид ВС" + arrOrderCol[1] + @"</th>                               
                               <th style='width: 130px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>" +this.MilitaryUnitLabel + arrOrderCol[2] + @"</th>
                               <th style='width: 50px;" + headerStyle + @"'>&nbsp;</th>
                            </tr>
                         </thead>";

                int counter = 1;

                //Iterate through all items and add them into the grid
                foreach (Committee committee in committees)
                {
                    string cellStyle = "vertical-align: top;";

                    string deleteHTML = "";

                    if (committee.CanDelete)
                    {
                        if (this.GetUIItemAccessLevel("HS_DELETECOMMITTEE") == UIAccessLevel.Enabled && this.GetUIItemAccessLevel("HS_COMMITTEE") == UIAccessLevel.Enabled)
                            deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на този комитет(група)' class='GridActionIcon' onclick='DeleteCommittee(" + committee.CommitteeId.ToString() + ");' />";
                    }

                    string editHTML = "";

                    if (this.GetUIItemAccessLevel("HS_EDITCOMMITTEE") != UIAccessLevel.Hidden)
                        editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='EditCommittee(" + committee.CommitteeId.ToString() + ");' />";


                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='" + cellStyle + @"'>" + ((pageIdx - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + (committee.CommitteeType != null ? committee.CommitteeType.TableValue : "") + @"</td>                                 
                                 <td style='" + cellStyle + @"'>" + (committee.MilitaryForceType != null ? committee.MilitaryForceType.MilitaryForceTypeName : "") + @"</td>
                                 <td style='" + cellStyle + @"'>" + (committee.MilitaryUnit != null ? committee.MilitaryUnit.DisplayTextForSelection : "") + @"</td>
                                 <td style='" + cellStyle + @"'>" + editHTML + deleteHTML + @"</td>
                              </tr>";

                    counter++;
                }

                html += "</table>";
            }

            //Put the generated grid on the page
            pnlCommitteesGrid.InnerHtml = html;

            //Refresh the paging button according to the current position
            SetImgBtns();
            lblPagination.Text = " | " + hdnPageIdx.Value + " от " + maxPage.ToString() + " | ";
            txtGotoPage.Text = "";

            //Set the message if there is a need (e.g. a deleted item)
            if (hdnRefreshReason.Value != "")
            {
                if (hdnRefreshReason.Value == "DELETED")
                {
                    lblGridMessage.Text = "Комитетът (групата) беше изтрит успешно";
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
                RefreshCommittees();
            }
        }

        //Go to the first page and refresh the grid
        protected void btnFirst_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = "1";
                RefreshCommittees();
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

                    RefreshCommittees();
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

                    RefreshCommittees();
                }
            }
        }

        //Go to the last page and refresh the grid
        protected void btnLast_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = maxPage.ToString();
                RefreshCommittees();
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
                    RefreshCommittees();
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
            Response.Redirect("~/ContentPages/AddEditCommittee.aspx");
        }

        //Delete committee (ajax call)
        private void JSDeleteCommittee()
        {
            if (this.GetUIItemAccessLevel("HS_DELETECOMMITTEE") != UIAccessLevel.Enabled || this.GetUIItemAccessLevel("HS_COMMITTEE") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            int committeeId = int.Parse(Request.Form["CommitteeID"]);

            string stat = "";
            string response = "";
            
            try
            {
                //Track the changes into the Audit Trail 
                Change change = new Change(CurrentUser, "HS_Committees");

                CommitteeUtil.DeleteCommittee(committeeId, CurrentUser, change);

                change.WriteLog();

                stat = AJAXTools.OK;
                response = "<response>OK</response>";
            }
            catch(Exception ex)
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
            ddCommitteeType.SelectedValue = ListItems.GetOptionAll().Value;
            ddMilitaryForceType.SelectedValue = ListItems.GetOptionAll().Value;
            musMilitaryUnit.SelectedText = "";
            musMilitaryUnit.SelectedValue = ListItems.GetOptionAll().Value;            
        }
    }
}
