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
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class ManageCompanies : RESPage
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
                return "RES_LISTMAINT_COMPANIES";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if (GetUIItemAccessLevel("RES_LISTMAINT") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            SetBtnNew();

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteCompany")
            {
                JSDeleteCompany();
                return;
            }

            //Hilight the current page in the menu bar
            HighlightMenuItems("Lists", "Lists_RES_Companies");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            //Setup some basic styles on the screen
            SetupStyle();

            //Collect the filter information to be able to pull the number of rows for this specific filter
            CompanyFilter filter = CollectFilter();

            int allRows = CompanyUtil.GetAllCompaniesCount(CurrentUser, filter);
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

        //Populate all listboxes on the screen
        private void PopulateLists()
        {
            PopulateOwnershipTypes();
            PopulateAdministrations();
        }

        private void PopulateOwnershipTypes()
        {
            ddOwnershipType.Items.Clear();
            ddOwnershipType.Items.Add(ListItems.GetOptionAll());

            List<OwnershipType> types = OwnershipTypeUtil.GetAllOwnershipTypes(CurrentUser);

            foreach (OwnershipType type in types)
            {
                ListItem li = new ListItem();
                li.Text = type.OwnershipTypeName;
                li.Value = type.OwnershipTypeId.ToString();

                ddOwnershipType.Items.Add(li);
            }
        }

        //Populate the types drop-down
        private void PopulateAdministrations()
        {
            ddAdministration.Items.Clear();
            ddAdministration.Items.Add(ListItems.GetOptionAll());

            List<Administration> administrations = AdministrationUtil.GetAllAdministrations(CurrentUser);

            foreach (Administration administration in administrations)
            {
                ListItem li = new ListItem();
                li.Text = administration.AdministrationName;
                li.Value = administration.AdministrationId.ToString();

                ddAdministration.Items.Add(li);
            }
        }

        //Setup some styling on the page
        private void SetupStyle()
        {
            lblBulstat.Style.Add("vertical-align", "top");
            txtBulstat.Style.Add("vertical-align", "top");
            lblCompanyName.Style.Add("vertical-align", "top");
            txtCompanyName.Style.Add("vertical-align", "top");
            lblOwnershipType.Style.Add("vertical-align", "top");
            ddOwnershipType.Style.Add("vertical-align", "top");
            lblAdministration.Style.Add("vertical-align", "top");
            ddAdministration.Style.Add("vertical-align", "top");
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
        private void RefreshItemsGrid()
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
            CompanyFilter filter = CollectFilter();

            //Get the list of items according to the specified filters, order and paging
            List<CompanyBlock> companyBlocks = CompanyUtil.GetAllCompanies(CurrentUser, filter, pageLength);

            //No data found
            if (companyBlocks.Count == 0)
            {
                html = "<span>Няма намерени резултати</span>";
            }
            //If there is data then generate dynamically the HTML for the data grid
            else
            {
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
                               <th style='width: 165px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Име" + arrOrderCol[0] + @"</th>
                               <th style='width: 120px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>" + CommonFunctions.GetLabelText("UnifiedIdentityCode") + "/ЕГН" + arrOrderCol[1] + @"</th>
                               <th style='width: 140px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>Вид собственост" + arrOrderCol[2] + @"</th>
                               <th style='width: 140px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(4);'>Министерство" + arrOrderCol[3] + @"</th>
                               <th style='width: 60px;" + headerStyle + @"'>&nbsp;</th>
                            </tr>
                         </thead>";

                int counter = 1;

                //Iterate through all items and add them into the grid
                foreach (CompanyBlock companyBlock in companyBlocks)
                {
                    string cellStyle = "vertical-align: top;";

                    string deleteHTML = "";

                    if (companyBlock.CanDelete)
                    {
                        if (GetUIItemAccessLevel("RES_LISTMAINT") == UIAccessLevel.Enabled &&
                            GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES") == UIAccessLevel.Enabled &&
                            GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES_DELETE") == UIAccessLevel.Enabled
                            )
                            deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на тази фирма' class='GridActionIcon' onclick='DeleteCompany(" + companyBlock.CompanyID.ToString() + ");' />";
                    }

                    string editHTML = "";


                    if (GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES_EDIT") != UIAccessLevel.Hidden)
                        editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='EditCompany(" + companyBlock.CompanyID.ToString() + ");' />";

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='" + cellStyle + @"'>" + ((pageIdx - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + companyBlock.CompanyName + @"</td>
                                 <td style='" + cellStyle + @"'>" + companyBlock.UnifiedIdentityCode + @"</td>
                                 <td style='" + cellStyle + @"'>" + companyBlock.OwnershipType + @"</td>
                                 <td style='" + cellStyle + @"'>" + companyBlock.Administration + @"</td>
                                 <td style='" + cellStyle + @"'>" + editHTML + deleteHTML + @"</td>
                              </tr>";

                    counter++;
                }

                html += "</table>";
            }

            //Put the generated grid on the page
            pnlItemsGrid.InnerHtml = html;

            //Refresh the paging button according to the current position
            SetImgBtns();
            lblPagination.Text = " | " + hdnPageIdx.Value + " от " + maxPage.ToString() + " | ";
            txtGotoPage.Text = "";

            //Set the message if there is a need (e.g. a deleted item)
            if (hdnRefreshReason.Value != "")
            {
                if (hdnRefreshReason.Value == "DELETED")
                {
                    lblGridMessage.Text = "Фирмата беше изтрита успешно";
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
                RefreshItemsGrid();
            }
        }

        //Go to the first page and refresh the grid
        protected void btnFirst_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = "1";
                RefreshItemsGrid();
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

                    RefreshItemsGrid();
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

                    RefreshItemsGrid();
                }
            }
        }

        //Go to the last page and refresh the grid
        protected void btnLast_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = maxPage.ToString();
                RefreshItemsGrid();
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
                    RefreshItemsGrid();
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
            Response.Redirect("~/ContentPages/AddEditCompany.aspx");
        }

        //Delete MilRepSpeciality (ajax call)
        private void JSDeleteCompany()
        {
            if (GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES_DELETE") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            int companyId = int.Parse(Request.Form["CompanyId"]);

            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_Company");

                CompanyUtil.DeleteCompany(CurrentUser, companyId, change);

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

        private void SetBtnNew()
        {
            if (GetUIItemAccessLevel("RES_LISTMAINT") == UIAccessLevel.Enabled &&
                GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES") == UIAccessLevel.Enabled &&
                GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES_ADD") == UIAccessLevel.Enabled)
            {
                EnableButton(btnNew);
            }
            else
            {
                HideControl(btnNew);
            }
        }

        private CompanyFilter CollectFilter()
        {
            CompanyFilter filter = new CompanyFilter();

            if (ddOwnershipType.SelectedValue != ListItems.GetOptionAll().Value)
                filter.OwnershipTypes = ddOwnershipType.SelectedValue;

            if (ddAdministration.SelectedValue != ListItems.GetOptionAll().Value)
                filter.Administrations = ddAdministration.SelectedValue;

            filter.UnifiedIdentityCode = txtBulstat.Text;
            filter.CompanyName = txtCompanyName.Text;

            int orderBy = 1;

            try
            {
                orderBy = int.Parse(hdnSortBy.Value);
            }
            catch
            {
                orderBy = 1;
            }

            filter.OrderBy = orderBy;

            int pageIdx = 1;

            try
            {
                pageIdx = int.Parse(hdnPageIdx.Value);
            }
            catch
            {
                pageIdx = 1;
            }

            filter.PageIdx = pageIdx;           

            return filter;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ddOwnershipType.SelectedValue = ListItems.GetOptionAll().Value;
            ddAdministration.SelectedValue = ListItems.GetOptionAll().Value;
            txtBulstat.Text = "";
            txtCompanyName.Text = "";
        }
    }
}
