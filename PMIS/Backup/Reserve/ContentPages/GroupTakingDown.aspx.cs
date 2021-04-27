using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class GroupTakingDown : RESPage
    {
        //Get the config setting that says how many rows per page should be dispayed in the grid
        private int pageLength = int.Parse(Config.GetWebSetting("RowsPerPage"));
        //Stores information about how many pages are in the grid
        private int maxPage;
        //Stores count of all reservists in last refres - used for message when group taking down
        protected int reservistsCount;

        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "RES_HUMANRES_GROUPTAKINGDOWN";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_HUMANRES_GROUPTAKINGDOWN") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            //Hilight the current page in the menu bar
            HighlightMenuItems("HumanResources", "GroupTakingDown");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            //Setup some basic styles on the screen
            SetupStyle();
            SetupDatePickers();
            
            //When the page is loaded for the first time
            if (!IsPostBack)
            {
                txtToDate.Text = CommonFunctions.FormatDate(DateTime.Now);
                txtDownDate.Text = CommonFunctions.FormatDate(DateTime.Now);

                //Populate page header title
                SetPageTitle();

                //Populate any drop-downs and list-boxes
                PopulateLists();

                //The default order is by module name
                if (string.IsNullOrEmpty(hdnSortBy.Value))
                    hdnSortBy.Value = "1";

                //Do not 'Simulate clicking the Refresh button to load the grid initially' to prevent slow loading
                //btnRefresh_Click(btnRefresh, new EventArgs());

                this.btnPrintGroupTakingDown.Visible = false;
                this.btnCheckMobileAppointment.Visible = false;
                this.btnGroupTakeDown.Visible = false;
            }
            else
            {
                //Collect the filter information to be able to pull the number of rows for this specific filter
                ReservistGroupTakingDownFilter filter = CollectFilterData();

                int allRows = ReservistUtil.GetAllReservistGroupTakingDownBlocksCount(filter, CurrentUser);
                //Get the number of rows and calculate the number of pages in the grid
                maxPage = pageLength == 0 ? 1 : allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

                this.reservistsCount = allRows;
            }
            if (GetUIItemAccessLevel("RES_HUMANRES_GROUPTAKINGDOWN") != UIAccessLevel.Enabled)
            {
                DisableButton(btnGroupTakeDown);
            }

            lblToDate.Style.Add("vertical-align", "bottom");

            lblMessage.Text = "";
            lblGridMessage.Text = "";
        }

        private void SetPageTitle()
        {
            lblHeaderTitle.InnerHtml = "Групово снемане от отчет";
        }       

        //Populate all listboxes on the screen
        private void PopulateLists()
        {
            PopulateGender();
            PopulateAdministration();
            PopulateMilitaryCategory();
            PopulateMilitaryDepartment();
            PopulateMilRepSpecType();
        }

        //Populate ddMilRepSpecType
        private void PopulateGender()
        {
            ddGender.DataSource = GenderUtil.GetGenders(CurrentUser);
            ddGender.DataTextField = "GenderName";
            ddGender.DataValueField = "GenderId";
            ddGender.DataBind();
            ddGender.Items.Insert(0, ListItems.GetOptionAll());

            CommonFunctions.SetDropDownTooltip(ddGender);
        }

        //Populate ddAdministration
        private void PopulateAdministration()
        {
            ddAdministration.DataSource = AdministrationUtil.GetAllAdministrations(CurrentUser);
            ddAdministration.DataTextField = "AdministrationName";
            ddAdministration.DataValueField = "AdministrationId";
            ddAdministration.DataBind();
            ddAdministration.Items.Insert(0, ListItems.GetOptionAll());

            CommonFunctions.SetDropDownTooltip(ddAdministration);
        }

        //Setup some styling on the page
        private void SetupStyle()
        {

        }

        //Setup any date picker controls on the page by setting the CSS of the target inputs
        //Note that the date picker CSS is common
        //This makes the calendar controls to appear on the page next to each input
        private void SetupDatePickers()
        {
            txtDownDate.CssClass = CommonFunctions.DatePickerCSS();
            txtToDate.CssClass = CommonFunctions.DatePickerCSS();
            txtOrderDate.CssClass = CommonFunctions.DatePickerCSS();
        }

        //Validate some field on the screen
        private bool ValidateData()
        {
            bool isDataValid = true;
            string errMsg = "";

            int age;

            if (txtAge.Text.Trim() != "" && !int.TryParse(txtAge.Text, out age))
            {
                isDataValid = false;
                errMsg += "Невалидна стойност за възраст";
            }

            if (!isDataValid)
            {
                lblMessage.CssClass = "ErrorText";
                lblMessage.Text = errMsg;
            }

            return isDataValid;
        }

        //Refresh the data grid
        private void RefreshReservists()
        {
            pnlPaging.Visible = true;

            string html = "";

            //Collect the filters, the order control and the paging control data from the page
            int orderBy;
            if (!int.TryParse(hdnSortBy.Value, out orderBy))
                orderBy = 0;

            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            //Collect the filter information to be able to pull the number of rows for this specific filter
            ReservistGroupTakingDownFilter filter = CollectFilterData();

            //Get the list of records according to the specified filters, order and paging
            List<ReservistGroupTakingDownBlock> reservists = ReservistUtil.GetAllReservistGroupTakingDownBlocks(filter, pageLength, CurrentUser);            

            //No data found
            if (reservists.Count == 0)
            {
                html = "<span>Няма намерени резултати</span>";
                this.btnPrintGroupTakingDown.Visible = false;
                this.btnCheckMobileAppointment.Visible = false;
                this.btnGroupTakeDown.Visible = false;
            }
            //If there is data then generate dynamically the HTML for the data grid
            else
            {
                this.btnPrintGroupTakingDown.Visible = true;
                this.btnCheckMobileAppointment.Visible = true;
                this.btnGroupTakeDown.Visible = true;

                string headerStyle = "vertical-align: bottom;";
                int orderCol = (orderBy > 100 ? orderBy - 100 : orderBy);
                string img = orderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
                string[] arrOrderCol = { "", "", "", "", "", "", "", "", };
                arrOrderCol[orderCol - 1] = @"<div style='Position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

                //Setup the header of the grid
                html = @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='width: 110px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Трите имена" + arrOrderCol[0] + @"</th>
                               <th style='width: 110px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>ЕГН" + arrOrderCol[1] + @"</th>
                               <th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>Пол" + arrOrderCol[2] + @"</th>
                               <th style='width: 250px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(4);'>Работил/служил в" + arrOrderCol[3] + @"</th>                               
                               <th style='width: 200px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(5);'>Състояние по отчета" + arrOrderCol[4] + @"</th>
                               <th style='width: 200px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(6);'>Команда" + arrOrderCol[5] + @"</th>     
                            </tr>
                         </thead>";

                int counter = 1;

                //Iterate through all items and add them into the grid
                foreach (ReservistGroupTakingDownBlock reservist in reservists)
                {
                    string cellStyle = "vertical-align: top;";
                
                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"' >
                                 <td style='" + cellStyle + @"'>" + ((pageIdx - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + reservist.FullName + @"</td>
                                 <td style='" + cellStyle + @"'>" + reservist.IdentNumber + @"</td>
                                 <td style='" + cellStyle + @"'>" + reservist.Gender + @"</td>
                                 <td style='" + cellStyle + @"'>" + reservist.Administration + @"</td>
                                 <td style='" + cellStyle + @"'>" + reservist.MilitaryReportStatus + @"</td>
                                 <td style='" + cellStyle + @"'>" + reservist.MilitaryCommand + @"</td>
                              </tr>";

                    counter++;
                }

                html += "</table>";
            }

            //Put the generated grid on the page
            pnlDataGrid.InnerHtml = html;

            //Refresh the paging button according to the current position
            SetImgBtns();
            lblPagination.Text = " | " + hdnPageIdx.Value + " от " + maxPage.ToString() + " | ";
            txtGotoPage.Text = "";           
        }

        //Refresh the data grid
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            pnlSearchHint.Visible = false;

            if (ValidateData())
            {
                hdnPageIdx.Value = "1";
                RefreshReservists();
            }
        }

        //Go to the first page and refresh the grid
        protected void btnFirst_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = "1";
                RefreshReservists();
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

                    RefreshReservists();
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

                    RefreshReservists();
                }
            }
        }

        //Go to the last page and refresh the grid
        protected void btnLast_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = maxPage.ToString();
                RefreshReservists();
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
                    RefreshReservists();
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

        //Navigate back to the previous screen
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ContentPages/Home.aspx");
        }

        //Collect the filet information from the page
        private ReservistGroupTakingDownFilter CollectFilterData()
        {
            ReservistGroupTakingDownFilter filter = new ReservistGroupTakingDownFilter();

            int? age;
            int _age;

            if (int.TryParse(txtAge.Text, out _age))
            {
                age = _age;
                this.hdnAge.Value = txtAge.Text;
            }            
            else
            {
                age = null;
                this.hdnAge.Value = "";
            }

            string gender = "";

            if (ddGender.SelectedValue != ListItems.GetOptionAll().Value)
            {
                gender = ddGender.SelectedValue;
                this.hdnGender.Value = this.ddGender.SelectedValue;
            }
            else
            {
                this.hdnGender.Value = "";
            }

            string militaryCategory = "";

            if (ddMilitaryCategory.SelectedValue != ListItems.GetOptionAll().Value)
            {
                militaryCategory = ddMilitaryCategory.SelectedValue;
                this.hdnMilitaryCategoryId.Value = this.ddMilitaryCategory.SelectedValue;
            }
            else
            {
                this.hdnMilitaryCategoryId.Value = "";
            }

            string militaryRank = "";

            if (ddMilitaryRank.SelectedValue != ListItems.GetOptionAll().Value)
            {
                militaryRank = ddMilitaryRank.SelectedValue;
                this.hdnMilitaryRankId.Value = this.ddMilitaryRank.SelectedValue;
            }
            else
            {
                this.hdnMilitaryRankId.Value = "";
            }

            string administration = "";

            if (ddAdministration.SelectedValue != ListItems.GetOptionAll().Value)
            {
                administration = ddAdministration.SelectedValue;
                this.hdnAdministration.Value = this.ddAdministration.SelectedValue;
            }
            else
            {
                this.hdnAdministration.Value = "";
            }

            string militaryDepartment = "";

            if (ddMilitaryDepartment.SelectedValue != ListItems.GetOptionAll().Value)
            {
                militaryDepartment = ddMilitaryDepartment.SelectedValue;
                this.hdnMilitaryDepartmentId.Value = this.ddMilitaryDepartment.SelectedValue;
            }
            else
            {
                this.hdnMilitaryDepartmentId.Value = "";
            }

            string milRepSpecType = "";
            string milRepSpec = "";

            if (ddMilRepSpecType.SelectedValue != ListItems.GetOptionAll().Value)
            {
                milRepSpecType = ddMilRepSpecType.SelectedValue;
                this.hdnMilRepSpecTypeId.Value = this.ddMilRepSpecType.SelectedValue;

                if (ddMilRepSpec.SelectedValue != ListItems.GetOptionAll().Value)
                {
                    milRepSpec = ddMilRepSpec.SelectedValue;
                    this.hdnMilRepSpecId.Value = this.ddMilRepSpec.SelectedValue;
                }
                else
                {
                    this.hdnMilRepSpecId.Value = "";
                }
            }
            else
            {
                this.hdnMilRepSpecTypeId.Value = "";
                this.hdnMilRepSpecId.Value = "";
            }

            int orderBy;
            if (!int.TryParse(hdnSortBy.Value, out orderBy))
                orderBy = 0;

            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            filter.Age = age;
            filter.Gender = gender;
            filter.MilitaryCategory = militaryCategory;
            filter.MilitaryRank = militaryRank;
            filter.Administration = administration;
            filter.MilitaryDepartment = militaryDepartment;
            filter.MilRepSpecType = milRepSpecType;
            filter.MilRepSpec = milRepSpec;
            filter.IsPrimaryMilRepSpec = chkIsPrimaryMilRepSpec.Checked;

            filter.ToDate = this.txtToDate.Text;

            filter.OrderBy = orderBy;
            filter.PageIdx = pageIdx;

            return filter;
        }

        protected void btnGroupTakeDown_Click(object sender, EventArgs e)
        {
            ReservistGroupTakingDownFilter filter = CollectFilterData();
            
            bool checkMobApptMilRepStatus = ReservistUtil.CheckMobApptMilRepStatus(filter, CurrentUser);

            if (!checkMobApptMilRepStatus)
            {
                lblMessage.CssClass = "ErrorText";
                lblMessage.Text = "Някои от избраните резервисти са със състояние \"С МН от задължителния резерв\"";
            }
            else
            {
                ReservistUtil.SetGroupDischargedMilRepStatus(filter, this.txtDownDate.Text, this.txtOrderNumber.Text, this.txtOrderDate.Text, this.txtOrderSignedBy.Text, CurrentUser);

                RefreshReservists();

                lblMessage.CssClass = "SuccessText";
                lblMessage.Text = "Групово снети от отчет са " + this.reservistsCount.ToString() + " резервиста.";
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {            
            txtAge.Text = "";
            txtToDate.Text = "";
            ddGender.SelectedValue = ListItems.GetOptionAll().Value;
            ddMilitaryCategory.SelectedValue = ListItems.GetOptionAll().Value;
            ddMilitaryRank.Items.Clear();
            ddAdministration.SelectedValue = ListItems.GetOptionAll().Value;
            ddMilitaryDepartment.SelectedValue = ListItems.GetOptionAll().Value;
            ddMilRepSpecType.SelectedValue = ListItems.GetOptionAll().Value;
            ddMilRepSpec.Items.Clear();
            chkIsPrimaryMilRepSpec.Checked = false;

            CollectFilterData(); //for clearing hidden fields, used in print page

            pnlPaging.Visible = false;
            pnlDataGrid.InnerHtml = "";
            pnlSearchHint.Visible = true;

            this.btnPrintGroupTakingDown.Visible = false;
            this.btnCheckMobileAppointment.Visible = false;
            this.btnGroupTakeDown.Visible = false;
        }

        //Populate ddMilitaryDepartment
        private void PopulateMilitaryDepartment()
        {
            ddMilitaryDepartment.DataSource = MilitaryDepartmentUtil.GetAllMilitaryDepartments(CurrentUser);
            ddMilitaryDepartment.DataTextField = "MilitaryDepartmentName";
            ddMilitaryDepartment.DataValueField = "MilitaryDepartmentId";
            ddMilitaryDepartment.DataBind();
            ddMilitaryDepartment.Items.Insert(0, ListItems.GetOptionAll());

            CommonFunctions.SetDropDownTooltip(ddMilitaryDepartment);
        }

        private void PopulateMilitaryCategory()
        {
            ddMilitaryCategory.DataSource = MilitaryCategoryUtil.GetAllMilitaryCategories(CurrentUser);
            ddMilitaryCategory.DataTextField = "CategoryName";
            ddMilitaryCategory.DataValueField = "CategoryId";
            ddMilitaryCategory.DataBind();
            ddMilitaryCategory.Items.Insert(0, ListItems.GetOptionAll());

            //initialize ddMilitaryRank with blannk value
            ddMilitaryRank.Items.Clear();
            ddMilitaryRank.Items.Insert(0, ListItems.GetOptionChooseOne());
        }

        //Populate ddMilitaryRank
        protected void ddMilitaryCategory_Changed(object sender, EventArgs e)
        {
            if (ddMilitaryCategory.SelectedValue != "-1")
            {
                ddMilitaryRank.DataSource = MilitaryRankUtil.GetAllMilitaryRanksByCategory(ddMilitaryCategory.SelectedValue, CurrentUser);
                ddMilitaryRank.DataTextField = "LongName";
                ddMilitaryRank.DataValueField = "MilitaryRankId";
                ddMilitaryRank.DataBind();
                ddMilitaryRank.Items.Insert(0, ListItems.GetOptionAll());
            }
            else
            {
                ddMilitaryRank.Items.Clear();
                ddMilitaryRank.Items.Insert(0, ListItems.GetOptionChooseOne());
            }
        }

        //Populate ddMilRepSpecType
        private void PopulateMilRepSpecType()
        {
            ddMilRepSpecType.DataSource = MilitaryReportSpecialityTypeUtil.GetAllMilitaryReportSpecialityTypes(CurrentUser);
            ddMilRepSpecType.DataTextField = "TypeName";
            ddMilRepSpecType.DataValueField = "Type";
            ddMilRepSpecType.DataBind();
            ddMilRepSpecType.Items.Insert(0, ListItems.GetOptionAll());

            // initialize ddMilRepSpec with blank value
            ddMilRepSpec.Items.Clear();
            ddMilRepSpec.Items.Insert(0, ListItems.GetOptionChooseOne());
        }

        //Populate ddMilRepSpec
        protected void ddMilRepSpecType_Changed(object sender, EventArgs e)
        {
            if (ddMilRepSpecType.SelectedValue != "-1")
            {
                int type = int.Parse(ddMilRepSpecType.SelectedValue);

                ddMilRepSpec.DataSource = MilitaryReportSpecialityUtil.GetMilitaryReportSpecialitiesByType(CurrentUser, type);
                ddMilRepSpec.DataTextField = "CodeAndName";
                ddMilRepSpec.DataValueField = "MilReportSpecialityId";
                ddMilRepSpec.DataBind();
                ddMilRepSpec.Items.Insert(0, ListItems.GetOptionAll());

                CommonFunctions.SetDropDownTooltip(ddMilRepSpec);
            }
            else
            {
                ddMilRepSpec.Items.Clear();
                ddMilRepSpec.Items.Insert(0, ListItems.GetOptionChooseOne());
            }
        }
    }
}
