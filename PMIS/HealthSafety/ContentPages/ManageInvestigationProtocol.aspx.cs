using System;
using System.Drawing;
using System.Collections.Generic;
using PMIS.Common;
using PMIS.HealthSafety.Common;
using System.Text;

namespace PMIS.HealthSafety.ContentPages
{
    public partial class ManageInvestigationProtocol : HSPage
    {
        #region Properties
        //Declare private fileds for class
        int pageLength = Convert.ToInt32(PMIS.Common.Config.GetWebSetting("RowsPerPage"));
        int maxPage;
        string investigaitonProtocolNumber;
        DateTime? invProtDateFrom;
        DateTime? invProtDateTo;
        string workerFullName;
        DateTime? accDateTimeFrom;
        DateTime? accDateTimeTo;

        InvestigationProtocolFilter investigationProtocolFilter;

        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "HS_INVPROTOCOLS";
            }
        }

        #endregion

        //Page load event
        protected void Page_Load(object sender, EventArgs e)
        {
            //Chek for user access
            if (this.GetUIItemAccessLevel("HS_INVPROTOCOLS") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            // Enable or disable button for adding new protocols, according to rights of the user's role
            SetBtnNew();

            LnkForceNoChangesCheck(this.btnSearch);

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "DeleteInvestigationProtocol")
            {
                JSDeleteInvestigationProtocol();
            }

            //Hilight the current page in the menu bar
            HighlightMenuItems("Accidents_SearchIvestigationProtocols", "Accidents");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(this.btnBack);

            //Clear validation label
            ShowMessageValidation("", false, "SuccessText");

            //bind messagegrid label
            string messageGrid = hdnResultMessage.Value;
            string cssClass = "SuccessText";
            bool showMessage = false;
            if (hdnResultStatus.Value != "")
            {
                switch (hdnResultStatus.Value)
                {
                    case "DELETED":
                        showMessage = true;
                        break;
                    case "NOT_DELETED":
                        showMessage = true;
                        cssClass = "ErrorText";
                        break;
                    case "NOT_EXIST":
                        showMessage = true;
                        cssClass = "ErrorText";
                        break;
                    default:
                        break;
                }
            }

            ShowMessageGrid(messageGrid, showMessage, cssClass);

            // fill filter fileds
            this.ObtainFilterFields();

            //create filter object
            investigationProtocolFilter = new InvestigationProtocolFilter();
            //initialize fields in object
            investigationProtocolFilter.InvestigaitonProtocolNumber = investigaitonProtocolNumber;
            investigationProtocolFilter.InvProtDateFrom = invProtDateFrom;
            investigationProtocolFilter.InvProtDateTo = invProtDateTo;
            investigationProtocolFilter.AccDateTimeFrom = accDateTimeFrom;
            investigationProtocolFilter.AccDateTimeTo = accDateTimeTo;
            investigationProtocolFilter.WorkerFullName = workerFullName;

            //get number of rows
            int allRows = InvestigationProtocolUtil.CountProtocols(investigationProtocolFilter, CurrentUser);
            //calculate number of pages
            this.maxPage = allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            //When the page is loaded for the first time
            if (!IsPostBack)
            {
                this.hdnPageIdx.Value = "1";
                this.LoadData();
            }
            else
            {
                if (int.Parse(this.hdnPageIdx.Value) > this.maxPage) this.hdnPageIdx.Value = this.maxPage.ToString();
            }
        }

        #region Methods

        private void SetBtnNew()
        {
            if (this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL") == UIAccessLevel.Enabled && this.GetUIItemAccessLevel("HS_INVPROTOCOLS") == UIAccessLevel.Enabled)
            {
                EnableButton(btnNew);
            }
            else
            {
                if (this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL") == UIAccessLevel.Hidden)
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
            investigaitonProtocolNumber = (this.txtInvestigaitonProtocolNumber.Text != "") ? this.txtInvestigaitonProtocolNumber.Text : null;

            this.hdnInvestProtNumber.Value = this.txtInvestigaitonProtocolNumber.Text;

            if (CommonFunctions.TryParseDate(this.txtInvProtDateFrom.Text))
            {
                invProtDateFrom = (this.txtInvProtDateFrom.Text != "") ? CommonFunctions.ParseDate(this.txtInvProtDateFrom.Text) : (DateTime?)null;
                this.hdnProtDateFrom.Value = this.txtInvProtDateFrom.Text;
            }
            else
            {
                invProtDateFrom = null;
            }

            if (CommonFunctions.TryParseDate(this.txtInvProtDateTo.Text))
            {
                invProtDateTo = (this.txtInvProtDateTo.Text != "") ? CommonFunctions.ParseDate(this.txtInvProtDateTo.Text) : (DateTime?)null;
                this.hdnProtDateTo.Value = this.txtInvProtDateTo.Text;
            }
            else
            {
                invProtDateTo = null;
            }

            workerFullName = (this.txtWorkerFullName.Text != "") ? this.txtWorkerFullName.Text : null;
            this.hdnInjuredName.Value = this.txtWorkerFullName.Text;

            if (CommonFunctions.TryParseDate(this.txtAccDateTimeFrom.Text))
            {
                accDateTimeFrom = (this.txtAccDateTimeFrom.Text != "") ? CommonFunctions.ParseDate(this.txtAccDateTimeFrom.Text) : (DateTime?)null;
                this.hdnAccidentDateFrom.Value = this.txtAccDateTimeFrom.Text;
            }
            else
            {
                accDateTimeFrom = null;
            }

            if (CommonFunctions.TryParseDate(this.txtAccDateTimeTo.Text))
            {
                accDateTimeTo = (this.txtAccDateTimeTo.Text != "") ? CommonFunctions.ParseDate(this.txtAccDateTimeTo.Text) : (DateTime?)null;
                this.hdnAccidentDateTo.Value = this.txtAccDateTimeTo.Text;
            }
            else
            {
                accDateTimeTo = null;
            }
        }

        //load data
        private void LoadData()
        {
            this.SetImgBtns();
            SetupDatePickers();
            lblPagination.Text = " | " + this.hdnPageIdx.Value + " от " + maxPage.ToString() + " | ";

            int orderby;
            if (!int.TryParse(hdnSortBy.Value, out orderby))
                orderby = 0;

            investigationProtocolFilter.OrderBy = orderby;
            investigationProtocolFilter.PageCount = pageLength;
            investigationProtocolFilter.PageIndex = int.Parse(this.hdnPageIdx.Value);

            this.LoadProtocolsTable(investigationProtocolFilter);
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
            txtAccDateTimeFrom.CssClass = CommonFunctions.DatePickerCSS();
            txtAccDateTimeTo.CssClass = CommonFunctions.DatePickerCSS();

            txtInvProtDateFrom.CssClass = CommonFunctions.DatePickerCSS();
            txtInvProtDateTo.CssClass = CommonFunctions.DatePickerCSS();
        }

        //Load data for table
        private void LoadProtocolsTable(InvestigationProtocolFilter investigationProtocolFilter)
        {
            this.protocolsTableDiv.InnerHtml = "";


            //Get the list of Investigation Protocols according to the specified filters, order and paging
            List<InvestigationProtocol> InvestigationProtocol = InvestigationProtocolUtil.GetAllInvestigationProtocols(investigationProtocolFilter, CurrentUser);
            //Fill protocolsTableDiv generated xml
            this.protocolsTableDiv.InnerHtml = this.GenerateProtocolsTable(InvestigationProtocol, investigationProtocolFilter.OrderBy);
        }

        //Generate XML value with data
        private string GenerateProtocolsTable(List<InvestigationProtocol> InvestigationProtocol, int orderBy)
        {
            if (InvestigationProtocol.Count > 0)
            {
                this.btnPrintAllInvestigationProtocols.Visible = true;
            }
            else
            {
                this.btnPrintAllInvestigationProtocols.Visible = false;
            }

            //set image depends of Acc or Desc order to sort
            string img = orderBy > 4 ? "../Images/desc.gif" : "../Images/asc.gif";

            string orderImg1 = "";
            string orderImg2 = "";
            string orderImg3 = "";
            string orderImg4 = "";

            if (orderBy == 1 || orderBy == 5)
                orderImg1 = @"<div style='position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";
            if (orderBy == 2 || orderBy == 6)
                orderImg2 = @"<div style='position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";
            if (orderBy == 3 || orderBy == 7)
                orderImg3 = @"<div style='position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";
            if (orderBy == 4 || orderBy == 8)
                orderImg4 = @"<div style='position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

            StringBuilder sb = new StringBuilder();
            string headerStyle = "vertical-align: bottom;";

            //Setup the header of the grid
            sb.Append(@"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Протокол №" + orderImg1 + @"</th>
                               <th style='width: 120px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>Дата на протокола" + orderImg2 + @"</th>
                               <th style='width: 300px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>Име на пострадалия" + orderImg3 + @"</th>
                               <th style='width: 120px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(4);'>Дата на злополуката" + orderImg4 + @"</th>
                               <th style='width: 60px;" + headerStyle + @"'>&nbsp;</th>
                            </tr>
                         </thead>");

            //Iterate through all items and add them into the grid

            int counter = 1;

            if (InvestigationProtocol.Count > 0)
            {
                sb.Append("<tbody>");
            }

            foreach (InvestigationProtocol ip in InvestigationProtocol)
            {
                sb.Append("<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + "'>");

                sb.Append("<td>" + counter + "</td>");

                sb.Append("<td align=\"left\">" + ip.InvestigaitonProtocolNumber + "</td>");

                string invProtDate = "";
                if (ip.InvProtDate.HasValue)
                {
                    invProtDate = PMIS.Common.CommonFunctions.FormatDate(ip.InvProtDate.Value.ToString());
                }
                sb.Append("<td align=\"left\">" + invProtDate + "</td>");
                sb.Append("<td align=\"left\">" + ip.DeclarationOfAccident.DeclarationOfAccidentWorker.WorkerFullName + "</td>");

                string accDateTime = "";
                if (ip.DeclarationOfAccident.DeclarationOfAccidentAcc.AccDateTime.HasValue)
                {
                    accDateTime = PMIS.Common.CommonFunctions.FormatDate(ip.DeclarationOfAccident.DeclarationOfAccidentAcc.AccDateTime.ToString());
                }

                sb.Append("<td align=\"left\">" + accDateTime + "</td>");


                //Set enable/disable logic to buutons

                string editHTML = "";

                if (this.GetUIItemAccessLevel("HS_EDITINVPROTOCOL") != UIAccessLevel.Hidden)
                {
                    editHTML = "<a href='../ContentPages/AddEditInvestigationProtocol.aspx?investigationProtocolId=" + ip.InvestigaitonProtocolId + "'><img border='0' src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' /></a>";
                }

                string deleteHTML = "";

                if (this.GetUIItemAccessLevel("HS_INVPROTOCOLS") == UIAccessLevel.Enabled && this.GetUIItemAccessLevel("HS_DELETEINVPROTOCOL") == UIAccessLevel.Enabled)
                {
                    deleteHTML = "<a href='javascript:DeleteInvestigationProtocol(" + ip.InvestigaitonProtocolId + ", \"" + ip.InvestigaitonProtocolNumber + "\");'><img border='0' src='../Images/delete.png' alt='Изтриване' title='Изтриване' class='GridActionIcon' /></a>";
                }

                sb.Append("<td>&nbsp;" + editHTML + "&nbsp;&nbsp;&nbsp;" + deleteHTML + "&nbsp;</td>");

                //sb.Append("<td><a href='../ContentPages/AddEditInvestigationProtocol.aspx?investigationProtocolId=" + ip.InvestigaitonProtocolId + "'><img border='0' src='../Images/edit.png' alt='Редактиране' title='Редактиране' /></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>");
                sb.Append("</tr>");
                counter++;
            }

            if (InvestigationProtocol.Count > 0)
            {
                sb.Append("</tbody>");
            }
            else
            {
                string css = "SuccessText";
                if (hdnResultStatus.Value == "DELETED")
                {
                    ShowMessageGrid(hdnResultMessage.Value, true, css);
                }
                else
                {
                  ShowMessageGrid("Няма намерени резултати", true, "DarkGreyText");
                }
            }

            sb.Append("<table>");
            sb.Append("</table>");

            return sb.ToString();
        }

        //Ajax operation to delete Investigation Protocol
        private void JSDeleteInvestigationProtocol()
        {
            string response = "";
            Change changeEntry = new Change(CurrentUser, "HS_InvProtocols");
            int investigaitonProtocolId = 0;
            if (Request.Params["investigaitonProtocolId"] != null)
                int.TryParse(Request.Params["investigaitonProtocolId"].ToString(), out investigaitonProtocolId);

            try
            {

                //get instance of current Investigation Protocol
                InvestigationProtocol investigationProtocol = InvestigationProtocolUtil.GetInvestigationProtocol(investigaitonProtocolId, CurrentUser);
                //Chek does Investigation Protocol exist
                if (investigationProtocol.InvestigaitonProtocolId > 0)
                {
                    if (InvestigationProtocolUtil.DeleteInvestigationProtocol(investigaitonProtocolId, CurrentUser, changeEntry))
                    {
                        response = "<Status>DELETED</Status>";
                        response += "<Message>" + "Протокола " + investigationProtocol.InvestigaitonProtocolNumber + " е изтрит" + "</Message>";

                        changeEntry.WriteLog();

                    }
                    else
                    {
                        response = "<Status>NOT_DELETED</Status>";
                        response += "<Message>" + "Протокола " + investigationProtocol.InvestigaitonProtocolNumber + "не е изтрит" + "</Message>";
                    }
                }
                else
                {
                    response = "<Status>NOT_EXIST</Status>";
                    response += "<Message>" + "Протокола " + investigationProtocol.InvestigaitonProtocolNumber + " не съществува" + "</Message>";
                }
            }
            catch (Exception ex)
            {
                response = "<Status>ERROR</Status>";
                response = AJAXTools.EncodeForXML(ex.Message);
            }

            AJAX a = new AJAX(response, this.Response);
            a.Write();
            Response.End();
        }

        //Validate input data from UI
        private bool ValidateData()
        {
            string errMsg = "";

            if (txtInvProtDateFrom.Text.Trim() != "" && !CommonFunctions.TryParseDate(txtInvProtDateFrom.Text))
            {
                errMsg += CommonFunctions.GetErrorMessageDate("Дата на протокола от") + "<br/>";
            }

            if (txtInvProtDateTo.Text.Trim() != "" && !CommonFunctions.TryParseDate(txtInvProtDateTo.Text))
            {
                errMsg += CommonFunctions.GetErrorMessageDate("Дата на протокола до") + "<br/>";
            }

            if (txtAccDateTimeFrom.Text.Trim() != "" && !CommonFunctions.TryParseDate(txtAccDateTimeFrom.Text))
            {
                errMsg += CommonFunctions.GetErrorMessageDate("Дата на злополука от") + "<br/>";
            }

            if (txtAccDateTimeTo.Text.Trim() != "" && !CommonFunctions.TryParseDate(txtAccDateTimeTo.Text))
            {
                errMsg += CommonFunctions.GetErrorMessageDate("Дата на злополука до") + "<br/>";
            }

            if (errMsg != "" )
            {
                ShowMessageValidation(errMsg, true, "ErrorMessage");
            }

            return errMsg == ""; 
        }

        //Show message for validation
        private void ShowMessageValidation(string message, bool visible, string cssClass)
        {
            this.lblMessageValidation.Text = message;
        }

        //Show message for result of operation
        private void ShowMessageGrid(string message, bool visible, string cssClass)
        {
            this.lblMessageGrid.CssClass = cssClass;
            this.lblMessageGrid.Text = message;
            this.lblMessageGrid.Visible = visible;
        }
        #endregion

        #region ObjectEvents

        //perform search operation with filter criteria
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (!ValidateData()) return;

            this.hdnPageIdx.Value = "1";

            investigationProtocolFilter = new InvestigationProtocolFilter();

            if (!String.IsNullOrEmpty(txtInvestigaitonProtocolNumber.Text)) investigationProtocolFilter.InvestigaitonProtocolNumber = txtInvestigaitonProtocolNumber.Text;
            if (!String.IsNullOrEmpty(txtWorkerFullName.Text)) investigationProtocolFilter.WorkerFullName = txtWorkerFullName.Text;

            if (!String.IsNullOrEmpty(txtInvProtDateFrom.Text)) investigationProtocolFilter.InvProtDateFrom = CommonFunctions.ParseDate(txtInvProtDateFrom.Text);
            if (!String.IsNullOrEmpty(txtInvProtDateTo.Text)) investigationProtocolFilter.InvProtDateTo = CommonFunctions.ParseDate(txtInvProtDateTo.Text);

            if (!String.IsNullOrEmpty(txtAccDateTimeFrom.Text)) investigationProtocolFilter.AccDateTimeFrom = CommonFunctions.ParseDate(txtAccDateTimeFrom.Text);
            if (!String.IsNullOrEmpty(txtAccDateTimeTo.Text)) investigationProtocolFilter.AccDateTimeTo = CommonFunctions.ParseDate(txtAccDateTimeTo.Text);

            int allRows = InvestigationProtocolUtil.CountProtocols(investigationProtocolFilter, CurrentUser);

            this.maxPage = allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            //Refresh data with new filter criteria
            this.LoadData();

        }

        //Refresh data - method call from JavaScript Code in aspx page
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            string css = "SuccessText";
            if (hdnResultStatus.Value != "DELETED") css = "ErrorText";

            ShowMessageGrid(hdnResultMessage.Value, true, css);
            this.LoadData();
        }

        //Redirect to AddEdit Page
        protected void btnNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ContentPages/AddEditInvestigationProtocol.aspx", true);
        }

        //Go to first page from tableResults
        protected void btnFirst_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.lblMessageGrid.Text = "";
            this.hdnPageIdx.Value = "1";
            this.LoadData();
        }

        //Go to previouse page from tableResults
        protected void btnPrev_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.lblMessageGrid.Text = "";
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
            this.lblMessageGrid.Text = "";
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
            this.lblMessageGrid.Text = "";
            this.hdnPageIdx.Value = maxPage.ToString();
            this.LoadData();
        }

        //Go to cutsom page from tableResults
        protected void btnGoto_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            this.lblMessageGrid.Text = "";
            int gotoPage;
            if (int.TryParse(tbxGotoPage.Text, out gotoPage) && gotoPage > 0 && gotoPage <= maxPage)
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
            txtInvestigaitonProtocolNumber.Text = "";
            txtWorkerFullName.Text = "";
            txtInvProtDateFrom.Text = "";
            txtInvProtDateTo.Text = "";
            txtAccDateTimeFrom.Text = "";
            txtAccDateTimeTo.Text = "";

        }
        #endregion
    }
}
