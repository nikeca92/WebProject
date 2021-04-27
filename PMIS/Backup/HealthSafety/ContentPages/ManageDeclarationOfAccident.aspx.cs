using System;
using System.Web.UI;
using PMIS.Common;
using PMIS.HealthSafety.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Drawing;

namespace PMIS.HealthSafety.ContentPages
{
    public partial class ManageDeclarationOfAccident : HSPage
    {
        #region Properties
        //Declare private fileds for class
        int pageLength = Convert.ToInt32(PMIS.Common.Config.GetWebSetting("RowsPerPage"));
        int maxPage;

        string declarationNumber;
        DateTime? declarationDateFrom;
        DateTime? declarationDateTo;
        string workerFullName;

        DeclarationOfAccidentFilter declarationOfAccidentFilter;

        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "HS_DECLARATIONACC";
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            //Chek for user access
            if (this.GetUIItemAccessLevel("HS_DECLARATIONACC") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            // Enable or disable button for adding new protocols, according to rights of the user's role
            SetBtnNew();

            //Highlight the current page in the menu bar
            HighlightMenuItems("Accidents_SearchDeclarationsOfAccident", "Accidents");

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteDeclarationAcc")
            {
                JSDeleteDeclarationsOfAccident(Convert.ToInt32(Request.Params["DeclarationId"]));
            }
            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(this.btnBack);

            // fill filter fileds
            this.ObtainFilterFields();

            //create filter object
            declarationOfAccidentFilter = new DeclarationOfAccidentFilter();
            //initialize fields in object
            declarationOfAccidentFilter.DeclarationNumber = declarationNumber;
            declarationOfAccidentFilter.DeclarationDateFrom = declarationDateFrom;
            declarationOfAccidentFilter.DeclarationDateTo = declarationDateTo;
            declarationOfAccidentFilter.WorkerFullName = workerFullName;

            //get number of rows
            int allRows = DeclarationOfAccidentUtil.CountDeclarationOfAccident(declarationOfAccidentFilter, CurrentUser);
            //calculate number of pages
            this.maxPage = allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            //When the page is loaded for the first time
            if (!IsPostBack)
            {
                //The default order is by module name
                if (string.IsNullOrEmpty(hdnSortBy.Value))
                    hdnSortBy.Value = "1";

                this.hdnPageIdx.Value = "1";

                this.SetImgBtns();
                this.SetupDatePickers();

                this.LoadData();
            }
            else
            {
                //chek if we are delete last items that is in last tab and it is only items in this tab
                if (int.Parse(this.hdnPageIdx.Value) > this.maxPage) this.hdnPageIdx.Value = this.maxPage.ToString();
            }
        }
        #region Methods

        private void SetBtnNew()
        {
            if (this.GetUIItemAccessLevel("HS_ADDDECLARATIONACC") == UIAccessLevel.Enabled && this.GetUIItemAccessLevel("HS_DECLARATIONACC") == UIAccessLevel.Enabled)
            {
                EnableButton(btnNew);
            }
            else
            {
                if (this.GetUIItemAccessLevel("HS_ADDDECLARATIONACC") == UIAccessLevel.Hidden)
                {
                    HideControl(btnNew);
                }
                else
                {
                    DisableButton(btnNew);
                }
            }
        }
        //inizialize value in private fileds
        private void ObtainFilterFields()
        {
            declarationNumber = (this.txtDeclarationNumber.Text != "") ? this.txtDeclarationNumber.Text : null;
            workerFullName = (this.txtWorkerFullName.Text != "") ? this.txtWorkerFullName.Text : null;

            if (CommonFunctions.TryParseDate(this.txtDeclarationDateFrom.Text))
            {
                declarationDateFrom = (this.txtDeclarationDateFrom.Text != "") ? CommonFunctions.ParseDate(this.txtDeclarationDateFrom.Text) : (DateTime?)null;
            }
            else
            {
                declarationDateFrom = null;
            }

            if (CommonFunctions.TryParseDate(this.txtDeclarationDateTo.Text))
            {
                declarationDateTo = (this.txtDeclarationDateTo.Text != "") ? CommonFunctions.ParseDate(this.txtDeclarationDateTo.Text) : (DateTime?)null;
            }
            else
            {
                declarationDateTo = null;
            }

            hfDeclarName.Value = this.txtDeclarationNumber.Text;
            hfWorkerName.Value = this.txtWorkerFullName.Text;
            hfDateFrom.Value = this.txtDeclarationDateFrom.Text;
            hfDateTo.Value = this.txtDeclarationDateTo.Text;
        }
        //load data
        private void LoadData()
        {
            lblGridMessage.Text = "";

            lblPagination.Text = " | " + this.hdnPageIdx.Value + " от " + maxPage.ToString() + " | ";

            int orderBy;
            if (!int.TryParse(hdnSortBy.Value, out orderBy))
                orderBy = 0;

            declarationOfAccidentFilter.OrderBy = orderBy;
            declarationOfAccidentFilter.PageCount = pageLength;
            declarationOfAccidentFilter.PageIndex = int.Parse(this.hdnPageIdx.Value);

            this.LoadDeclarationsOfAccidentTable(declarationOfAccidentFilter);

            //Set the message if there is a need (e.g. a deleted item)
            if (hdnRefreshReason.Value != "")
            {
                if (hdnRefreshReason.Value == "DELETED")
                {
                    lblGridMessage.Text = "Декларацията беше изтрита успешно";
                    lblGridMessage.CssClass = "SuccessText";
                }
                hdnRefreshReason.Value = "";
            }
        }
        //set custom image to images buttons
        private void SetImgBtns()
        {
            int page = int.Parse(this.hdnPageIdx.Value);

            btnFirst.ImageUrl = "../Images/ButtonFirst.png";
            btnPrev.ImageUrl = "../Images/ButtonPrev.png";
            btnLast.ImageUrl = "../Images/ButtonLast.png";
            btnNext.ImageUrl = "../Images/ButtonNext.png";

            if (page == 1)
            {
                btnFirst.ImageUrl = "../Images/ButtonFirstDisabled.png";
                btnPrev.ImageUrl = "../Images/ButtonPrevDisabled.png";
            }

            if (page == maxPage)
            {
                btnLast.ImageUrl = "../Images/ButtonLastDisabled.png";
                btnNext.ImageUrl = "../Images/ButtonNextDisabled.png";
            }
        }
        //Set date picker for data textboxes
        private void SetupDatePickers()
        {
            this.txtDeclarationDateFrom.CssClass = CommonFunctions.DatePickerCSS();
            this.txtDeclarationDateTo.CssClass = CommonFunctions.DatePickerCSS();
        }
        //Load data for table
        private void LoadDeclarationsOfAccidentTable(DeclarationOfAccidentFilter declarationOfAccidentFilter)
        {
            //Get the list of Investigation Protocols according to the specified filters, order and paging
            List<DeclarationOfAccident> listdeclarationOfAccident = DeclarationOfAccidentUtil.GetAllDeclarationOfAccident(declarationOfAccidentFilter, CurrentUser);

            //Fill protocolsTableDiv generated xml
            this.DeclarationsGrid.InnerHtml = this.GenerateProtocolsTable(listdeclarationOfAccident, declarationOfAccidentFilter);

            //Refresh the paging button according to the current position
            SetImgBtns();
            lblPagination.Text = " | " + hdnPageIdx.Value + " от " + maxPage.ToString() + " | ";
            this.txtGotoPage.Text = "";

        }
        //Generate XML value with data
        private string GenerateProtocolsTable(List<DeclarationOfAccident> listDeclarationOfAccident, DeclarationOfAccidentFilter declarationOfAccidentFilter)
        {
            string html;
            //No data found
            if (listDeclarationOfAccident.Count == 0)
            {
                html = "<span>Няма намерени резултати</span>";
                if (hdnRefreshReason.Value == "DELETED")
                {
                    html = "<span></span>";
                }
                this.btnPrintAllDeclarationOfAccidents.Visible = false;
            }
            //If there is data then generate dynamically the HTML for the data grid
            else
            {
                this.btnPrintAllDeclarationOfAccidents.Visible = true;

                string headerStyle = "vertical-align: bottom;";
                int orderCol = (declarationOfAccidentFilter.OrderBy > 100 ? declarationOfAccidentFilter.OrderBy - 100 : declarationOfAccidentFilter.OrderBy);
                string img = declarationOfAccidentFilter.OrderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
                string[] arrOrderCol = { "", "", "" };

                arrOrderCol[orderCol - 1] = @"<div style='position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

                //Setup the header of the grid
                html = @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='width: 230px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Декларация №" + arrOrderCol[0] + @"</th>
                               <th style='width: 120px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>Дата" + arrOrderCol[1]
                                                                          + @"</th>
                               <th style='width: 300px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>Име на пострадал" + arrOrderCol[2] + @"</th>
<th style='width: 50px; " + headerStyle + @"'></th> </tr></thead>";

                int counter = 1;

                //Iterate through all items and add them into the grid
                foreach (DeclarationOfAccident declarationOfAccident in listDeclarationOfAccident)
                {

                    string cellStyle = "vertical-align: top;";

                    //Add UI Logic Here

                    string editHTML = "";
                    if (this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC") != UIAccessLevel.Hidden)
                    {
                        editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='EditDeclarationAcc(" + declarationOfAccident.DeclarationId.ToString() + ");' />";
                    }

                    string deleteHTML = "";

                    //Chek for Investigation Protocols for this declaration
                    if (InvestigationProtocolUtil.CountProtocolsForDeclarationAcc(declarationOfAccident.DeclarationId, CurrentUser) == 0)
                    {
                        if (this.GetUIItemAccessLevel("HS_DECLARATIONACC") == UIAccessLevel.Enabled && this.GetUIItemAccessLevel("HS_DELETEDECLARATIONACC") == UIAccessLevel.Enabled)
                        {
                            deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на този протокол' class='GridActionIcon' onclick='DeleteDeclarationAcc(" + declarationOfAccident.DeclarationId.ToString() + ");' />";
                        }
                    }

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='" + cellStyle + @"'>" + ((declarationOfAccidentFilter.PageIndex - 1) * declarationOfAccidentFilter.PageCount + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + declarationOfAccident.DeclarationOfAccidentHeader.DeclarationNumber + @"</td>
                                 <td style='" + cellStyle + @"'>" + (declarationOfAccident.DeclarationOfAccidentHeader.DeclarationDate.HasValue ? CommonFunctions.FormatDate(declarationOfAccident.DeclarationOfAccidentHeader.DeclarationDate.Value) : "") + @"</td>
                                 <td style='" + cellStyle + @"'>" + declarationOfAccident.DeclarationOfAccidentWorker.WorkerFullName + @"</td>
                                 <td style='" + cellStyle + @"'>" + editHTML + deleteHTML + @"</td>
                              </tr>";

                    counter++;
                }
                html += "</table>";
            }
            return html;
        }

        //Ajax operation to delete Investigation Protocol
        private void JSDeleteDeclarationsOfAccident(int declarationId)
        {
            //if (this.GetUIItemAccessLevel("HS_DELETEPROT") != UIAccessLevel.Enabled || this.GetUIItemAccessLevel("HS_PROTOCOLS") != UIAccessLevel.Enabled)
            //    RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                //Track the changes into the Audit Trail 
                Change change = new Change(CurrentUser, "HS_DeclAcc");

                if (DeclarationOfAccidentUtil.DeleteDeclarationOfAccident(declarationId, CurrentUser, change))
                {
                    change.WriteLog();

                    stat = AJAXTools.OK;
                    response = "<response>OK</response>";
                }
                else
                {
                    stat = AJAXTools.ERROR;
                }

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

        //Validate input data from UI
        //private bool ValidateData()
        //{
        //    bool isDataValid = true;
        //    string errMsg = "";

        //    if (txtDeclarationDateFrom.Text.Trim() != "" && !CommonFunctions.TryParseDate(txtDeclarationDateFrom.Text))
        //    {
        //        isDataValid = false;
        //        errMsg += "Полето Дата на протокола\"от\" е невалидна дата<br/>";
        //    }

        //    if (txtDeclarationDateTo.Text.Trim() != "" && !CommonFunctions.TryParseDate(txtDeclarationDateTo.Text))
        //    {
        //        isDataValid = false;
        //        errMsg += "Полето Дата на протокола\"до\" е невалидна дата";
        //    }


        //    if (!isDataValid)
        //    {
        //        lblGridMessage.Text = errMsg;
        //        lblGridMessage.ForeColor = Color.Red;
        //    }
        //    else
        //    {
        //        lblGridMessage.Text = "";
        //    }

        //    return isDataValid;
        //}
        #endregion

        #region ObjectEvents

        //perform search operation with filter criteria
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // if (!ValidateData()) return;

            this.hdnPageIdx.Value = "1";

            DeclarationOfAccidentFilter declarationOfAccidentFilter = new DeclarationOfAccidentFilter();

            if (!String.IsNullOrEmpty(txtDeclarationNumber.Text))
            {
                declarationOfAccidentFilter.DeclarationNumber = txtDeclarationNumber.Text;
            }

            if (!String.IsNullOrEmpty(txtWorkerFullName.Text))
            {
                declarationOfAccidentFilter.WorkerFullName = txtWorkerFullName.Text;
            }

            if (!String.IsNullOrEmpty(txtDeclarationDateFrom.Text))
            {
                declarationOfAccidentFilter.DeclarationDateFrom = CommonFunctions.ParseDate(txtDeclarationDateFrom.Text);
            }

            if (!String.IsNullOrEmpty(txtDeclarationDateTo.Text))
            {
                declarationOfAccidentFilter.DeclarationDateTo = CommonFunctions.ParseDate(txtDeclarationDateTo.Text);
            }

            int allRows = DeclarationOfAccidentUtil.CountDeclarationOfAccident(declarationOfAccidentFilter, CurrentUser);

            this.maxPage = allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            this.LoadData();

        }

        //Refresh data - method call from JavaScript Code in aspx page
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            this.LoadData();
        }

        //Redirect to AddEdit Page
        protected void btnNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ContentPages/AddEditDeclarationOfAccident.aspx", true);
        }

        //Go to first page from tableResults
        protected void btnFirst_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.hdnPageIdx.Value = "1";
            this.LoadData();
        }

        //Go to previouse page from tableResults
        protected void btnPrev_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {

            int page = int.Parse(this.hdnPageIdx.Value);
            if (page > 1)
            {
                page--;
                this.hdnPageIdx.Value = page.ToString();

                this.LoadData();
            }
        }

        //Go to next page from tableResults
        protected void btnNext_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            int page = int.Parse(this.hdnPageIdx.Value);
            if (page < maxPage)
            {
                page++;
                this.hdnPageIdx.Value = page.ToString();
                this.LoadData();
            }
        }

        //Go to last page from tableResults
        protected void btnLast_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.hdnPageIdx.Value = maxPage.ToString();
            this.LoadData();
        }

        //Go to cutsom page from tableResults
        protected void btnGoto_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            int gotoPage;
            if (int.TryParse(txtGotoPage.Text, out gotoPage) && gotoPage > 0 && gotoPage <= maxPage)
            {
                this.hdnPageIdx.Value = gotoPage.ToString();
                this.LoadData();
            }
        }

        //Redirect to Home Page
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ContentPages/Home.aspx", true);
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtDeclarationNumber.Text = "";
            txtWorkerFullName.Text = "";
            txtDeclarationDateFrom.Text = "";
            txtDeclarationDateTo.Text = "";
        }
        #endregion        
    }
}
