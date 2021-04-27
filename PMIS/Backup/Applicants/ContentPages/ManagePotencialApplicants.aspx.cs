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
    public partial class ManagePotencialApplicants : APPLPage
    {
        private bool canCurrentUserAccessThisMilDepartment;
        //Get the config setting that says how many rows per page should be dispayed in the grid
        private int pageLength = int.Parse(Config.GetWebSetting("RowsPerPage"));
        //Stores information about how many pages are in the grid
        private int maxPage;

        private int milDepID = 0;
        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "APPL_POTENCIALAPPL";
            }
        }

        private bool CanCurrentUserAccessThisMilDepartment
        {
            get
            {
                string[] currentUserMilDepartmentIDs = CurrentUser.MilitaryDepartmentIDs_ListOfValues.Split(',');

                canCurrentUserAccessThisMilDepartment = currentUserMilDepartmentIDs.Any(c => c == milDepID.ToString());

                return canCurrentUserAccessThisMilDepartment;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if (GetUIItemAccessLevel("APPL_POTENCIALAPPL") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            SetBtnNew();

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeletePotencialApplicant")
            {
                JSDeletePotencialApplicant();
                return;
            }

            //Hilight the current page in the menu bar
            HighlightMenuItems("Applicants", "ManagePotencialApplicants");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            //Setup some basic styles on the screen
            SetupStyle();

            //Collect the filter information to be able to pull the number of rows for this specific filter
            PotencialApplicantsFilter filter = CollectFilterData();

            int allRows = PotencialApplicantUtil.GetAllPotencialApplicantsCount(filter, CurrentUser);
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

            this.txtLastApperianceFrom.CssClass = CommonFunctions.DatePickerCSS();
            this.txtLastApperianceTo.CssClass = CommonFunctions.DatePickerCSS(); 
        }

        //Populate all listboxes on the screen
        private void PopulateLists()
        {
            // this.PopulateVacancyAnnounces();
            this.PopulateMilitaryDepartments();
            PopulateDrivingLicenseCategories();
            PopulateServiceTypes();
        }

        //Populate the MilitaryDepartments drop-down
        private void PopulateMilitaryDepartments()
        {
            this.ddlMilitaryDepartments.DataSource = MilitaryDepartmentUtil.GetAllMilitaryDepartments(CurrentUser);
            this.ddlMilitaryDepartments.DataTextField = "MilitaryDepartmentName";
            this.ddlMilitaryDepartments.DataValueField = "MilitaryDepartmentId";
            this.ddlMilitaryDepartments.DataBind();
            this.ddlMilitaryDepartments.Items.Insert(0, ListItems.GetOptionChooseOne());
        }

        //Populate the DrivingLicenseCategories pick list
        private void PopulateDrivingLicenseCategories()
        {
            string result = "";

            List<DrivingLicenseCategory> categories = DrivingLicenseCategoryUtil.GetAllDrivingLicenseCategories(CurrentUser);

            foreach (DrivingLicenseCategory category in categories)
            {
                string pickListItem = "{value : '" + category.DrivingLicenseCategoryId.ToString() + "' , label : '" + category.DrivingLicenseCategoryName.Replace("'", "\\'") + "'}";
                result += (result == "" ? "" : ",") + pickListItem;
            }

            if (result != "")
                result = "[" + result + "]";

            hdnDrvLicCategories.Value = result;
        }

        //Populate the ServiceTypes pick list
        private void PopulateServiceTypes()
        {
            string result = "";

            List<ServiceType> serviceTypes = ServiceTypeUtil.GetAllServiceTypes(CurrentUser);

            foreach (ServiceType serviceType in serviceTypes)
            {
                string pickListItem = "{value : '" + serviceType.ServiceTypeID.ToString() + "' , label : '" + serviceType.ServiceTypeName.Replace("'", "\\'") + "'}";
                result += (result == "" ? "" : ",") + pickListItem;
            }

            if (result != "")
                result = "[" + result + "]";

            hdnServiceTypes.Value = result;
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
        private void RefreshApplicants()
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
            PotencialApplicantsFilter filter = CollectFilterData();

            //Get the list of records according to the specified filters, order and paging
            List<PotencialApplicant> listPotencialApplicant = PotencialApplicantUtil.GetAllPotencialApplicants(filter, pageLength, CurrentUser);

            //No data found
            if (listPotencialApplicant.Count == 0)
            {
                html = "<span>Няма намерени резултати</span>";
                this.btnPrintAllApplicants.Visible = false;
            }
            //If there is data then generate dynamically the HTML for the data grid
            else
            {
                this.btnPrintAllApplicants.Visible = true;

                string headerStyle = "vertical-align: bottom;";
                int orderCol = (orderBy > 100 ? orderBy - 100 : orderBy);
                string img = orderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
                string[] arrOrderCol = { "", "", "", "", "", "", "", "" };
                arrOrderCol[orderCol - 1] = @"<div style='position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

                //Setup the header of the grid
                html = @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='width: 180px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Трите имена" + arrOrderCol[0] + @"</th>
                               <th style='width: 10px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>ЕГН" + arrOrderCol[1] + @"</th>
                               <th style='width: 200px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>Място на регистрация" + arrOrderCol[2] + @"</th>
                               <th style='width: 80px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(4);'>Последна актуализация" + arrOrderCol[3] + @"</th>
                               <th style='width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(5);'>Последнo явяванe" + arrOrderCol[4] + @"</th>
                               <th style='width: 60px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(6);'>Шофьорска книжка" + arrOrderCol[5] + @"</th>
                               <th style='width: 140px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(8);'>Вид служба" + arrOrderCol[7] + @"</th>
                               <th style='width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(7);'>Коментар" + arrOrderCol[6] + @"</th>
                               <th style='width: 50px;" + headerStyle + @"'>&nbsp;</th>
                            </tr>
                         </thead>";

                int counter = 1;

                //Iterate through all items and add them into the grid
                foreach (PotencialApplicant potencialApplicant in listPotencialApplicant)
                {
                    string cellStyle = "vertical-align: top;";

                    string deleteHTML = "";

                    milDepID = potencialApplicant.MilitaryDepartmentId;

                    if (GetUIItemAccessLevel("APPL_POTENCIALAPPL") == UIAccessLevel.Enabled &&
                        GetUIItemAccessLevel("APPL_POTENCIALAPPL_DELETE_POTENCIALAPPL") == UIAccessLevel.Enabled && CanCurrentUserAccessThisMilDepartment)
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване' class='GridActionIcon' onclick='DeleteApplicant(" + potencialApplicant.PotencialApplicantId.ToString() + ");' />";


                    string editHTML = "";

                    if (GetUIItemAccessLevel("APPL_POTENCIALAPPL_EDIT_POTENCIALAPPL") != UIAccessLevel.Hidden)
                        editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='EditApplicant(" + potencialApplicant.PotencialApplicantId.ToString() + ");' />";

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td align='center' style='" + cellStyle + @"'>" + ((pageIdx - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + potencialApplicant.Person.FullName + @"</td>
                                 <td style='" + cellStyle + @"'>" + potencialApplicant.Person.IdentNumber + @"</td>
                                 <td style='" + cellStyle + @"'>" + potencialApplicant.MilitaryDepartment.MilitaryDepartmentName + @"</td>
                                 <td style='" + cellStyle + @"'>" + CommonFunctions.FormatDate(potencialApplicant.Person.LastModifiedDate) + @"</td>
                                 <td style='" + cellStyle + @"'>" + CommonFunctions.FormatDate(potencialApplicant.LastAppearance) + @"</td>
                                 <td style='" + cellStyle + @"'>" + potencialApplicant.Person.DrivingLicenseCategoriesString + @"</td>
                                 <td style='" + cellStyle + @"'>" + potencialApplicant.ServiceTypesString + @"</td>
                                 <td style='" + cellStyle + @"'>" + potencialApplicant.Comments + @"</td>
                                 <td align='center' style='" + cellStyle + @"'>" + editHTML + deleteHTML + @"</td>
                              </tr>";

                    counter++;
                }

                html += "</table>";
            }

            //Put the generated grid on the page
            pnlApplicantsGrid.InnerHtml = html;

            //Refresh the paging button according to the current position
            SetImgBtns();
            lblPagination.Text = " | " + hdnPageIdx.Value + " от " + maxPage.ToString() + " | ";
            txtGotoPage.Text = "";

            //Set the message if there is a need (e.g. a deleted item)
            if (hdnRefreshReason.Value != "")
            {
                if (hdnRefreshReason.Value == "DELETED")
                {
                    lblGridMessage.Text = "Кандидатът беше изтрит успешно";
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
                RefreshApplicants();
            }
        }

        //Go to the first page and refresh the grid
        protected void btnFirst_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = "1";
                RefreshApplicants();
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

                    RefreshApplicants();
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

                    RefreshApplicants();
                }
            }
        }

        //Go to the last page and refresh the grid
        protected void btnLast_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = maxPage.ToString();
                RefreshApplicants();
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
                    RefreshApplicants();
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
            Response.Redirect("~/ContentPages/AddPotencialApplicant_SelectPerson.aspx");
        }

        //Delete a record (ajax call)
        private void JSDeletePotencialApplicant()
        {
            if (GetUIItemAccessLevel("APPL_APPL") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("APPL_POTENCIALAPPL") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("APPL_POTENCIALAPPL_DELETE_POTENCIALAPPL") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            int potencialApplicantId = int.Parse(Request.Form["PotencialApplicantId"]);

            PotencialApplicant potencialApplicant = PotencialApplicantUtil.GetPotencialApplicant(potencialApplicantId, CurrentUser);
            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "APPL_PotencialApplicants");

                PotencialApplicantUtil.DeletePotencialApplicant(potencialApplicant, CurrentUser, change);

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
            if (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL") == UIAccessLevel.Enabled && this.GetUIItemAccessLevel("APPL_POTENCIALAPPL") == UIAccessLevel.Enabled)
            {
                EnableButton(btnNew);
            }
            else
            {
                if (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL") == UIAccessLevel.Hidden)
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
        private PotencialApplicantsFilter CollectFilterData()
        {
            PotencialApplicantsFilter filter = new PotencialApplicantsFilter();

            //if (this.ddlVacancyAnnounces.SelectedValue != "" && this.ddlVacancyAnnounces.SelectedValue != "-1")
            //{
            //    filter.VacancyAnnounceId = int.Parse(this.ddlVacancyAnnounces.SelectedValue);
            //    this.hfVacancyAnnounceId.Value = this.ddlVacancyAnnounces.SelectedValue;
            //}
            //else
            //{
            //    this.hfVacancyAnnounceId.Value = "";
            //}

            if (this.ddlMilitaryDepartments.SelectedValue != ListItems.GetOptionChooseOne().Value
                && this.ddlMilitaryDepartments.SelectedValue != "")
            {
                filter.MilitaryDepartmentId = int.Parse(this.ddlMilitaryDepartments.SelectedValue);
                this.hfMilitaryDepartmentId.Value = this.ddlMilitaryDepartments.SelectedValue;
            }
            else
            {
                this.hfMilitaryDepartmentId.Value = "";
            }

            if (!string.IsNullOrEmpty(txtComment.Text))
            {
                filter.Comment = txtComment.Text;
                this.hfComment.Value = txtComment.Text;
            }
            else
            {
                this.hfComment.Value = "";
            }

            if (!string.IsNullOrEmpty(txtIdentNumber.Text))
            {
                filter.IdentityNumber = txtIdentNumber.Text;
            }
            else
            {
                filter.IdentityNumber = "";
            }

            filter.DrivingLicense = this.hfDrivingLicense.Value;
            
            filter.ServiceType = this.hfServiceTypes.Value;

            DateTime? lastApperianceFrom = null;

            if (CommonFunctions.TryParseDate(txtLastApperianceFrom.Text))
            {
                lastApperianceFrom = CommonFunctions.ParseDate(txtLastApperianceFrom.Text);
                filter.LastApperianceFrom = lastApperianceFrom;
                this.hdnLastApperianceFrom.Value = txtLastApperianceFrom.Text;
            }

            DateTime? lastApperianceTo = null;

            if (CommonFunctions.TryParseDate(txtLastApperianceTo.Text))
            {
                lastApperianceTo = CommonFunctions.ParseDate(txtLastApperianceTo.Text);
                filter.LastApperianceTo = lastApperianceTo;
                this.hdnLastApperianceTo.Value = txtLastApperianceTo.Text;
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

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ddlMilitaryDepartments.SelectedValue = ListItems.GetOptionAll().Value;
            txtComment.Text = "";
            txtLastApperianceFrom.Text = "";
            txtLastApperianceTo.Text = "";
            hfDrivingLicense.Value = "";
            hfServiceTypes.Value = "";
            txtIdentNumber.Text = "";
        }
    }
}
