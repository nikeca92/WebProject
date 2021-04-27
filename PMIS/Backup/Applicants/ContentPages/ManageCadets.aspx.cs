using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

using PMIS.Common;
using PMIS.Applicants.Common;
using System.Collections.Generic;

namespace PMIS.Applicants.ContentPages
{
    public partial class ManageCadets : APPLPage
    {
        //Get the config setting that says how many rows per page should be dispayed in the grid
        private int pageLength = int.Parse(Config.GetWebSetting("RowsPerPage"));
        //Stores information about how many pages are in the grid
        private int maxPage;

        private int milDepID = 0;

        private bool canCurrentUserAccessThisMilDepartment;

        private bool CanCurrentUserAccessThisMilDepartment
        {
            get
            {
                string[] currentUserMilDepartmentIDs = CurrentUser.MilitaryDepartmentIDs_ListOfValues.Split(',');

                canCurrentUserAccessThisMilDepartment = currentUserMilDepartmentIDs.Any(c => c == milDepID.ToString());

                return canCurrentUserAccessThisMilDepartment;
            }
        }

        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "APPL_CADETS";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if (GetUIItemAccessLevel("APPL_CADETS") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            SetBtnNew();

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteCadet")
            {
                JSDeleteCadet();
                return;
            }

            //Hilight the current page in the menu bar
            HighlightMenuItems("Cadets", "Cadets_Search");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            //Setup some basic styles on the screen
            SetupStyle();

            //Collect the filter information to be able to pull the number of rows for this specific filter
            CadetsSearchFilter filter = CollectFilterData();

            int allRows = CadetSearchUtil.GetAllCadetsCount(filter, CurrentUser);
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
            this.PopulateMilitarySchools();
            this.PopulateMilitaryDepartments();
        }

        //Populate the MilitarySchools drop-down
        private void PopulateMilitarySchools()
        {
            this.ddlMilitarySchools.DataSource = MilitarySchoolUtil.GetAllMilitarySchools(CurrentUser, true); ;
            this.ddlMilitarySchools.DataTextField = "MilitarySchoolName";
            this.ddlMilitarySchools.DataValueField = "MilitarySchoolId";
            this.ddlMilitarySchools.DataBind();
            this.ddlMilitarySchools.Items.Insert(0, ListItems.GetOptionAll());

            this.PopulateSchoolYears();
        }

        //Populate the MilitaryDepartments drop-down
        private void PopulateMilitaryDepartments()
        {
            this.ddlMilitaryDepartments.DataSource = MilitaryDepartmentUtil.GetAllMilitaryDepartments(CurrentUser);
            this.ddlMilitaryDepartments.DataTextField = "MilitaryDepartmentName";
            this.ddlMilitaryDepartments.DataValueField = "MilitaryDepartmentId";
            this.ddlMilitaryDepartments.DataBind();
            this.ddlMilitaryDepartments.Items.Insert(0, ListItems.GetOptionAll());
        }

        //Populate the SchoolYears drop-down
        private void PopulateSchoolYears()
        {
            this.ddlSchoolYears.DataSource = null;
            this.ddlSchoolYears.Items.Clear();
            List<int> years = new List<int>();

            if (this.ddlMilitarySchools.SelectedValue != ListItems.GetOptionAll().Value
                && this.ddlMilitarySchools.SelectedValue != "")
            {
                years = MilitarySchoolSpecializationUtil.GetAllYearsByMilitarySchoolID(int.Parse(this.ddlMilitarySchools.SelectedValue), CurrentUser);
            }
            else
            {
                years = MilitarySchoolSpecializationUtil.GetAllYearsForAllMilitarySchools(CurrentUser);
            }

            this.ddlSchoolYears.DataSource = years;
            this.ddlSchoolYears.DataBind();
            this.ddlSchoolYears.Items.Insert(0, ListItems.GetOptionAll());
        }

        //Setup some styling on the page
        private void SetupStyle()
        {

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
        private void RefreshCadets()
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
            CadetsSearchFilter filter = CollectFilterData();

            //Get the list of records according to the specified filters, order and paging
            List<CadetSearch> cadetSearches = CadetSearchUtil.GetAllCadetsBySearch(filter, pageLength, CurrentUser);

            //No data found
            if (cadetSearches.Count == 0)
            {
                html = "<span>Няма намерени резултати</span>";
                this.btnPrintAllCadets.Visible = false;
            }
            //If there is data then generate dynamically the HTML for the data grid
            else
            {
                this.btnPrintAllCadets.Visible = true;

                string headerStyle = "vertical-align: bottom;";
                int orderCol = (orderBy > 100 ? orderBy - 100 : orderBy);
                string img = orderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
                string[] arrOrderCol = { "", "", "", "", "", "" };
                arrOrderCol[orderCol - 1] = @"<div style='position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

                //Setup the header of the grid
                html = @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='width: 200px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Трите имена" + arrOrderCol[0] + @"</th>
                               <th style='width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>ЕГН" + arrOrderCol[1] + @"</th>
                               <th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>Военно училище" + arrOrderCol[2] + @"</th>
                               <th style='width: 90px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(4);'>Учебна година" + arrOrderCol[3] + @"</th>
                               <th style='width: 200px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(5);'>Място на регистрация" + arrOrderCol[4] + @"</th>
                               <th style='width: 130px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(6);'>Последна актуализация" + arrOrderCol[5] + @"</th>
                               <th style='width: 30px;" + headerStyle + @"'>&nbsp;</th>
                            </tr>
                         </thead>";

                int counter = 1;

                //Iterate through all items and add them into the grid
                foreach (CadetSearch cadetSearch in cadetSearches)
                {

                    milDepID = cadetSearch.MilitaryDepartmentId;

                    string cellStyle = "vertical-align: top;";

                    string deleteHTML = "";

                    if (cadetSearch.CanDelete && GetUIItemAccessLevel("APPL_CADETS") == UIAccessLevel.Enabled &&
                            GetUIItemAccessLevel("APPL_CADETS_DELETECADET") == UIAccessLevel.Enabled && CanCurrentUserAccessThisMilDepartment)
                    {
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на този курсант' class='GridActionIcon' onclick='DeleteCadet(" + cadetSearch.CadetId.ToString() + ");' />";
                    }

                    string editHTML = "";

                    if (GetUIItemAccessLevel("APPL_CADETS_EDITCADET") != UIAccessLevel.Hidden)
                        editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='EditCadet(" + cadetSearch.CadetId.ToString() + ");' />";

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td align='center' style='" + cellStyle + @"'>" + ((pageIdx - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + cadetSearch.PersonName + @"</td>
                                 <td style='" + cellStyle + @"'>" + cadetSearch.PersonIdentNumber + @"</td>
                                 <td style='" + cellStyle + @"'>" + cadetSearch.MilitarySchoolName + @"</td>
                                 <td style='" + cellStyle + @"'>" + cadetSearch.SchoolYear + @"</td>
                                 <td style='" + cellStyle + @"'>" + cadetSearch.MilitaryDepartmentName + @"</td>
                                 <td style='" + cellStyle + @"'>" + CommonFunctions.FormatDate(cadetSearch.LastModifiedDate) + @"</td>
                                 <td align='center' style='" + cellStyle + @"'>" + editHTML + deleteHTML + @"</td>
                              </tr>";

                    counter++;
                }

                html += "</table>";
            }

            //Put the generated grid on the page
            pnlCadetsGrid.InnerHtml = html;

            //Refresh the paging button according to the current position
            SetImgBtns();
            lblPagination.Text = " | " + hdnPageIdx.Value + " от " + maxPage.ToString() + " | ";
            txtGotoPage.Text = "";

            //Set the message if there is a need (e.g. a deleted item)
            if (hdnRefreshReason.Value != "")
            {
                if (hdnRefreshReason.Value == "DELETED")
                {
                    lblGridMessage.Text = "Курсантът беше изтрит успешно";
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
                RefreshCadets();
            }
        }

        //Go to the first page and refresh the grid
        protected void btnFirst_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = "1";
                RefreshCadets();
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

                    RefreshCadets();
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

                    RefreshCadets();
                }
            }
        }

        //Go to the last page and refresh the grid
        protected void btnLast_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = maxPage.ToString();
                RefreshCadets();
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
                    RefreshCadets();
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
            Response.Redirect("~/ContentPages/AddCadet_SelectPerson.aspx");
        }

        //Delete a record (ajax call)
        private void JSDeleteCadet()
        {
            if (GetUIItemAccessLevel("APPL_CADETS") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("APPL_CADETS_DELETECADET") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int cadetId = int.Parse(Request.Form["CadetId"]);

                Change change = new Change(CurrentUser, "APPL_Cadets");

                CadetUtil.DeleteCadet(cadetId, CurrentUser, change);

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
            if (this.GetUIItemAccessLevel("APPL_CADETS_ADDCADET") == UIAccessLevel.Enabled 
                && this.GetUIItemAccessLevel("APPL_CADETS") == UIAccessLevel.Enabled)
            {
                EnableButton(btnNew);
            }
            else
            {
                if (this.GetUIItemAccessLevel("APPL_CADETS_ADDCADET") == UIAccessLevel.Hidden)
                {
                    HideControl(btnNew);
                }
                else
                {
                    DisableButton(btnNew);
                }
            }
        }

        //Collect the filet information from the page
        private CadetsSearchFilter CollectFilterData()
        {
            CadetsSearchFilter filter = new CadetsSearchFilter();

            if (this.ddlMilitarySchools.SelectedValue != ListItems.GetOptionAll().Value
                && this.ddlMilitarySchools.SelectedValue != "")
            {
                filter.MilitarySchoolId = int.Parse(this.ddlMilitarySchools.SelectedValue);
                this.hfMilitarySchoolId.Value = this.ddlMilitarySchools.SelectedValue;
            }
            else
            {
                this.hfMilitarySchoolId.Value = "";
            }

            if (this.ddlMilitaryDepartments.SelectedValue != ListItems.GetOptionAll().Value
                && this.ddlMilitaryDepartments.SelectedValue != "")
            {
                filter.MilitaryDepartmentId = int.Parse(this.ddlMilitaryDepartments.SelectedValue);
                this.hfMilitaryDepartmentId.Value = this.ddlMilitaryDepartments.SelectedValue;
            }
            else
            {
                this.hfMilitaryDepartmentId.Value = "";
            }

            if (this.ddlSchoolYears.SelectedValue != ListItems.GetOptionAll().Value
                && this.ddlSchoolYears.SelectedValue != "")
            {
                filter.SchoolYear = int.Parse(this.ddlSchoolYears.SelectedValue);
                this.hfSchoolYear.Value = this.ddlSchoolYears.SelectedValue;
            }
            else
            {
                this.hfSchoolYear.Value = "";
            }

            if (!string.IsNullOrEmpty(txtIdentNumber.Text))
            {
                filter.IdentityNumber = txtIdentNumber.Text;
            }
            else
            {
                filter.IdentityNumber = "";
            }

            int orderBy;
            if (!int.TryParse(this.hdnSortBy.Value, out orderBy))
                orderBy = 0;

            int pageIdx;
            if (!int.TryParse(this.hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            filter.OrderBy = orderBy;
            filter.PageIdx = pageIdx;

            return filter;
        }

        protected void ddlMilitarySchools_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.PopulateSchoolYears();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ddlMilitarySchools.SelectedValue = ListItems.GetOptionAll().Value;
            ddlMilitarySchools_SelectedIndexChanged(sender, e);
            ddlMilitaryDepartments.SelectedValue = ListItems.GetOptionAll().Value;
            txtIdentNumber.Text = "";
        }
    }
}
