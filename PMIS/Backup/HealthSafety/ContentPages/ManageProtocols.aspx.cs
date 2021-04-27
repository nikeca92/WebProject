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
    public partial class ManageProtocols : HSPage
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
                return "HS_PROTOCOLS";
            }
        }

        //This get label name from resourse file 
        private string MilitaryUnitLabel = CommonFunctions.GetLabelText("MilitaryUnit");

        protected void Page_Load(object sender, EventArgs e)
        {

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteProtocol")
            {
                JSDeleteProtocol();
                return;
            }

            if (this.GetUIItemAccessLevel("HS_PROTOCOLS") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            //Hilight the current page in the menu bar
            HighlightMenuItems("Protocols", "Protocols_Search");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            //Setup some basic styles on the screen
            SetupStyle();

            // Enable or disable button for adding new protocols, according to rights of the user's role
            SetBtnNew();

            //Setup any calendar control on the screen
            SetupDatePickers();

            if (!IsPostBack)
            {
                //Pre-fill the date fields with the today's date
                txtDateFrom.Text = CommonFunctions.FormatDate(DateTime.Now);
                txtDateTo.Text = CommonFunctions.FormatDate(DateTime.Now);
            }

            //Collect the filter information to be able to pull the number of rows for this specific filter
            string protocolTypes = "";

            if (ddProtocolType.SelectedValue != ListItems.GetOptionAll().Value)
                protocolTypes = ddProtocolType.SelectedValue;

            DateTime? dateFrom = null;

            if (CommonFunctions.TryParseDate(txtDateFrom.Text))
                dateFrom = CommonFunctions.ParseDate(txtDateFrom.Text);

            DateTime? dateTo = null;

            if (CommonFunctions.TryParseDate(txtDateTo.Text))
                dateTo = CommonFunctions.ParseDate(txtDateTo.Text);

            int allRows = ProtocolUtil.GetAllProtocolsCount(txtProtocolNum.Text, protocolTypes, dateFrom, dateTo, CurrentUser);
            //Get the number of rows and calculate the number of pages in the grid
            maxPage = pageLength == 0 ? 1 : allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            //When the page is loaded for the first time
            if (!IsPostBack)
            {
                //Populate any drop-downs and list-boxes
                PopulateLists();

                //The default order is by protocol number
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
            if (this.GetUIItemAccessLevel("HS_ADDPROT") == UIAccessLevel.Enabled && this.GetUIItemAccessLevel("HS_PROTOCOLS") == UIAccessLevel.Enabled)
            {
                EnableButton(btnNew);
            }
            else
            {
                if (this.GetUIItemAccessLevel("HS_ADDPROT") == UIAccessLevel.Hidden)
                {
                    
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
            PopulateProtocolTypes();
        }

        //Populate the measures drop-down
        private void PopulateProtocolTypes()
        {
            ddProtocolType.Items.Clear();
            ddProtocolType.Items.Add(ListItems.GetOptionAll());

            List<ProtocolType> protocolTypes = ProtocolTypeUtil.GetAllProtocolTypes(CurrentUser);

            foreach (ProtocolType protocolType in protocolTypes)
            {
                ListItem li = new ListItem();
                li.Text = protocolType.ProtocolTypeName;
                li.Value = protocolType.ProtocolTypeId.ToString();

                ddProtocolType.Items.Add(li);
            }
        }

        //Setup some styling on the page
        private void SetupStyle()
        {
            lblProtocolType.Style.Add("vertical-align", "top");
            ddProtocolType.Style.Add("vertical-align", "top");
            lblProtocolNum.Style.Add("vertical-align", "top");
            txtProtocolNum.Style.Add("vertical-align", "top");
        }

        //Setup any date picker controls on the page by setting the CSS of the target inputs
        //Note that the date picker CSS is common
        //This makes the calendar controls to appear on the page next to each input
        private void SetupDatePickers()
        {
            txtDateFrom.CssClass = CommonFunctions.DatePickerCSS();
            txtDateTo.CssClass = CommonFunctions.DatePickerCSS();
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
        private void RefreshProtocols()
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
            string protocolTypes = "";

            if (ddProtocolType.SelectedValue != ListItems.GetOptionAll().Value)
            {
                protocolTypes = ddProtocolType.SelectedValue;
                this.hfProtocolTypeId.Value = this.ddProtocolType.SelectedValue;
            }
            else
            {
                this.hfProtocolTypeId.Value = "";
            }

            this.hfProtNumber.Value = this.txtProtocolNum.Text;

            DateTime? dateFrom = null;
            this.hfDateFrom.Value = "";

            if (CommonFunctions.TryParseDate(txtDateFrom.Text))
            {
                dateFrom = CommonFunctions.ParseDate(txtDateFrom.Text);
                this.hfDateFrom.Value = this.txtDateFrom.Text;
            }

            DateTime? dateTo = null;
            this.hfDateTo.Value = "";

            if (CommonFunctions.TryParseDate(txtDateTo.Text))
            {
                dateTo = CommonFunctions.ParseDate(txtDateTo.Text);
                this.hfDateTo.Value = this.txtDateTo.Text;
            }

            //Get the list of Protocols according to the specified filters, order and paging
            List<Protocol> protocols = ProtocolUtil.GetAllProtocols(txtProtocolNum.Text, protocolTypes, dateFrom, dateTo, orderBy, pageIdx, pageLength, CurrentUser);

            //No data found
            if (protocols.Count == 0)
            {
                html = "<span>Няма намерени резултати</span>";
                this.btnPrintAllProtocols.Visible = false;
            }
            //If there is data then generate dynamically the HTML for the data grid
            else
            {
                this.btnPrintAllProtocols.Visible = true;

                string headerStyle = "vertical-align: bottom;";
                int orderCol = (orderBy > 100 ? orderBy - 100 : orderBy);
                string img = orderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
                string[] arrOrderCol = { "", "", "", "" };
                arrOrderCol[orderCol - 1] = @"<div style='position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

                //Setup the header of the grid
                html = @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='width: 130px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Протокол №" + arrOrderCol[0] + @"</th>
                               <th style='width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>Дата" + arrOrderCol[1] + @"</th>
                               <th style='width: 180px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>Измерване" + arrOrderCol[2] + @"</th>
                               <th style='width: 130px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(4);'>" + this.MilitaryUnitLabel + arrOrderCol[3] + @"</th>
                               <th style='width: 50px;" + headerStyle + @"'>&nbsp;</th>
                            </tr>
                         </thead>";

                int counter = 1;

                //Iterate through all items and add them into the grid
                foreach (Protocol protocol in protocols)
                {
                    string cellStyle = "vertical-align: top;";

                    string deleteHTML = "";

                    if (protocol.CanDelete)
                    {
                        if (this.GetUIItemAccessLevel("HS_DELETEPROT") == UIAccessLevel.Enabled && this.GetUIItemAccessLevel("HS_PROTOCOLS") == UIAccessLevel.Enabled)
                            deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на този протокол' class='GridActionIcon' onclick='DeleteProtocol(" + protocol.ProtocolId.ToString() + ");' />";
                    }

                    string editHTML = "";

                    if (this.GetUIItemAccessLevel("HS_EDITPROT") != UIAccessLevel.Hidden)
                        editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='EditProtocol(" + protocol.ProtocolId.ToString() + ");' />";


                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='" + cellStyle + @"'>" + ((pageIdx - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + protocol.ProtocolNumber + @"</td>
                                 <td style='" + cellStyle + @"'>" + (protocol.ProtocolDate.HasValue ? CommonFunctions.FormatDate(protocol.ProtocolDate.Value) : "") + @"</td>
                                 <td style='" + cellStyle + @"'>" + (protocol.ProtocolType != null ? protocol.ProtocolType.ProtocolTypeName : "") + @"</td>
                                 <td style='" + cellStyle + @"'>" + (protocol.MilitaryUnit != null ? protocol.MilitaryUnit.DisplayTextForSelection : "") + @"</td>
                                 <td style='" + cellStyle + @"'>" + editHTML + deleteHTML + @"</td>
                              </tr>";

                    counter++;
                }

                html += "</table>";
            }

            //Put the generated grid on the page
            pnlProtocolsGrid.InnerHtml = html;

            //Refresh the paging button according to the current position
            SetImgBtns();
            lblPagination.Text = " | " + hdnPageIdx.Value + " от " + maxPage.ToString() + " | ";
            txtGotoPage.Text = "";

            //Set the message if there is a need (e.g. a deleted item)
            if (hdnRefreshReason.Value != "")
            {
                if (hdnRefreshReason.Value == "DELETED")
                {
                    lblGridMessage.Text = "Протоколът беше изтрит успешно";
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
                RefreshProtocols();
            }
        }

        //Go to the first page and refresh the grid
        protected void btnFirst_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = "1";
                RefreshProtocols();
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

                    RefreshProtocols();
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

                    RefreshProtocols();
                }
            }
        }

        //Go to the last page and refresh the grid
        protected void btnLast_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = maxPage.ToString();
                RefreshProtocols();
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
                    RefreshProtocols();
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
            Response.Redirect("~/ContentPages/AddEditProtocol.aspx");
        }

        //Delete protocol (ajax call)
        private void JSDeleteProtocol()
        {
            if (this.GetUIItemAccessLevel("HS_DELETEPROT") != UIAccessLevel.Enabled || this.GetUIItemAccessLevel("HS_PROTOCOLS") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            int protocolId = int.Parse(Request.Form["ProtocolID"]);

            string stat = "";
            string response = "";

            try
            {
                //Track the changes into the Audit Trail 
                Change change = new Change(CurrentUser, "HS_Protocols");

                ProtocolUtil.DeleteProtocol(protocolId, CurrentUser, change);

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

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ddProtocolType.SelectedValue = ListItems.GetOptionAll().Value;
            txtProtocolNum.Text = "";
            txtDateFrom.Text = "";
            txtDateTo.Text = "";
        }
    }
}
