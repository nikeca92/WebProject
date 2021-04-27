using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class ReportTechnics_VESSELS : RESPage
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
                return "RES_REPORTS_VESSELS";
            }
        }

        public string TechnicsTypeKey
        {
            get
            {
                return "VESSELS";
            }
        }

        public string TechnicsTypeName
        {
            get;
            set;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if (GetUIItemAccessLevel("RES_REPORTS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_REPORTS_VESSELS") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            this.hdnTechnicsTypeKey.Value = TechnicsTypeKey;

            TechnicsType technicsType = TechnicsTypeUtil.GetTechnicsType(TechnicsTypeKey, CurrentUser);
            TechnicsTypeName = technicsType.TypeName;

            lblHeaderTitle.InnerHtml = "Списък на техниката на военен отчет - " + technicsType.TypeName;
            this.Title = lblHeaderTitle.InnerHtml;

            lblOwnershipNumber.Text = "ЕГН/" + CommonFunctions.GetLabelText("UnifiedIdentityCode") + ":";

            //Setup any calendar control on the screen
            SetupDatePickers();

            //Hilight the current page in the menu bar
            HighlightMenuItems("Reports", "ReportTechnics_" + TechnicsTypeKey);

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            //Setup some basic styles on the screen
            SetupStyle();

            //Collect the filter information to be able to pull the number of rows for this specific filter
            ReportVesselManageFilter filter = CollectFilterData();

            int allRows = ReportVesselUtil.GetAllReportVesselManageBlocksCount(filter, CurrentUser);
            //Get the number of rows and calculate the number of pages in the grid
            maxPage = pageLength == 0 ? 1 : allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);
            
            //When the page is loaded for the first time
            if (!IsPostBack)
            {
                //Populate any drop-downs and list-boxes
                PopulateLists();

                //The default order is by normative technics
                if (string.IsNullOrEmpty(hdnSortBy.Value))
                    hdnSortBy.Value = "10";

                //Simulate clicking the Refresh button to load the grid initially
                btnRefresh_Click(btnRefresh, new EventArgs());
            }

            lblMessage.Text = "";
            lblGridMessage.Text = "";
        }

        //Populate all listboxes on the screen
        private void PopulateLists()
        {
            PopulateTechnicsCategory();
            PopulateVesselKind();
            PopulateVesselType();
            PopulateMilitaryReportStatus();
            PopulateMilitaryDepartment();
            PopulateRegions();
            PopulateNormativeTechnics();
            PopulateAppointmentIsDelivered();
            PopulateReadiness();

            ListItems.SetTextAsTooltip(ddTechnicsCategory,
                                       ddVesselKind,
                                       ddVesselType,
                                       ddMilitaryReportStatus,
                                       ddMilitaryDepartment,
                                       ddNormativeTechnics,
                                       ddAppointmentIsDelivered,
                                       ddReadiness);
        }

        //Populate ddTechnicsCategory
        private void PopulateTechnicsCategory()
        {
            ddTechnicsCategory.DataSource = TechnicsCategoryUtil.GetAllTechnicsCategories(CurrentUser);
            ddTechnicsCategory.DataTextField = "CategoryName";
            ddTechnicsCategory.DataValueField = "TechnicsCategoryId";
            ddTechnicsCategory.DataBind();
            ddTechnicsCategory.Items.Insert(0, ListItems.GetOptionAll());
        }

        //Populate ddVesselKind
        private void PopulateVesselKind()
        {
            ddVesselKind.DataSource = GTableItemUtil.GetAllGTableItemsByTableName("VesselKind", ModuleKey, 1, 0, 0, CurrentUser);
            ddVesselKind.DataTextField = "TableValue";
            ddVesselKind.DataValueField = "TableKey";
            ddVesselKind.DataBind();
            ddVesselKind.Items.Insert(0, ListItems.GetOptionAll());
        }

        //Populate ddVesselType
        private void PopulateVesselType()
        {
            ddVesselType.DataSource = GTableItemUtil.GetAllGTableItemsByTableName("VesselType", ModuleKey, 1, 0, 0, CurrentUser);
            ddVesselType.DataTextField = "TableValue";
            ddVesselType.DataValueField = "TableKey";
            ddVesselType.DataBind();
            ddVesselType.Items.Insert(0, ListItems.GetOptionAll());
        }

        //Populate ddMilitaryReportStatus
        private void PopulateMilitaryReportStatus()
        {
            ddMilitaryReportStatus.DataSource = TechMilitaryReportStatusUtil.GetAllTechMilitaryReportStatuses(CurrentUser);
            ddMilitaryReportStatus.DataTextField = "TechMilitaryReportStatusName";
            ddMilitaryReportStatus.DataValueField = "TechMilitaryReportStatusId";
            ddMilitaryReportStatus.DataBind();
            ddMilitaryReportStatus.Items.Insert(0, ListItems.GetOptionAll());
        }

        //Populate ddMilitaryDepartment
        private void PopulateMilitaryDepartment()
        {
            ddMilitaryDepartment.DataSource = MilitaryDepartmentUtil.GetAllMilitaryDepartments(CurrentUser);
            ddMilitaryDepartment.DataTextField = "MilitaryDepartmentName";
            ddMilitaryDepartment.DataValueField = "MilitaryDepartmentId";
            ddMilitaryDepartment.DataBind();
            ddMilitaryDepartment.Items.Insert(0, ListItems.GetOptionAll());
        }

        //Populate ddRegion
        private void PopulateRegions()
        {
            ddRegion.DataSource = RegionUtil.GetRegions(CurrentUser);
            ddRegion.DataTextField = "RegionName";
            ddRegion.DataValueField = "RegionId";
            ddRegion.DataBind();
            ddRegion.Items.Insert(0, ListItems.GetOptionAll());
        }

        protected void ddRegion_Changed(object sender, EventArgs e)
        {
            ddMuniciplaity.Items.Clear();
            ddCity.Items.Clear();
            ddDistrict.Items.Clear();

            if (ddRegion.SelectedValue != "-1")
            {
                int regionId = int.Parse(ddRegion.SelectedValue);

                PopulateMunicipalities(regionId);
            }
        }

        //Populate ddMuniciplaity
        private void PopulateMunicipalities(int regionID)
        {
            ddMuniciplaity.DataSource = MunicipalityUtil.GetMunicipalities(regionID, CurrentUser); ;
            ddMuniciplaity.DataTextField = "MunicipalityName";
            ddMuniciplaity.DataValueField = "MunicipalityId";
            ddMuniciplaity.DataBind();
            ddMuniciplaity.Items.Insert(0, ListItems.GetOptionAll());
        }

        protected void ddMuniciplaity_Changed(object sender, EventArgs e)
        {
            ddCity.Items.Clear();
            ddDistrict.Items.Clear();

            if (ddMuniciplaity.SelectedValue != "-1")
            {
                int municiplaityId = int.Parse(ddMuniciplaity.SelectedValue);

                PopulateCities(municiplaityId);
            }
        }

        //Populate ddCity
        private void PopulateCities(int municipalityID)
        {
            ddCity.DataSource = CityUtil.GetCities(municipalityID, CurrentUser);
            ddCity.DataTextField = "CityName";
            ddCity.DataValueField = "CityId";
            ddCity.DataBind();
            ddCity.Items.Insert(0, ListItems.GetOptionAll());

            // Initialize ddDistrict with blank value
            ddDistrict.Items.Insert(0, ListItems.GetOptionChooseOne());
        }

        //Populate ddDistrict
        protected void ddCity_Changed(object sender, EventArgs e)
        {
            ddDistrict.Items.Clear();

            if (ddCity.SelectedValue != "-1")
            {
                int cityId = int.Parse(ddCity.SelectedValue);

                ddDistrict.DataSource = DistrictUtil.GetDistricts(cityId, CurrentUser);
                ddDistrict.DataTextField = "DistrictName";
                ddDistrict.DataValueField = "DistrictId";
                ddDistrict.DataBind();

                if (ddDistrict.Items.Count > 0)
                    ddDistrict.Items.Insert(0, ListItems.GetOptionAll());
                else
                    ddDistrict.Items.Insert(0, ListItems.GetOptionChooseOne());
            }
        }

        private void PopulateNormativeTechnics()
        {
            ddNormativeTechnics.DataSource = NormativeTechnicsUtil.GetNormativeTechnics(CurrentUser, TechnicsTypeKey);
            ddNormativeTechnics.DataTextField = "CodeAndText";
            ddNormativeTechnics.DataValueField = "NormativeTechnicsId";
            ddNormativeTechnics.DataBind();
            ddNormativeTechnics.Items.Insert(0, ListItems.GetOptionAll());
        }

        //Populate ddAppointmentIsDelivered
        private void PopulateAppointmentIsDelivered()
        {
            ddAppointmentIsDelivered.Items.Add(ListItems.GetOptionAll());
            ddAppointmentIsDelivered.Items.Add(ListItems.GetOptionYes());
            ddAppointmentIsDelivered.Items.Add(ListItems.GetOptionNo());
        }

        //Populate ddReadiness
        private void PopulateReadiness()
        {
            int baseReadiness = 1;
            int additionalReadiness = 2;

            ddReadiness.Items.Clear();

            ddReadiness.Items.Insert(0, ListItems.GetOptionAll());
            ddReadiness.Items.Add(new ListItem(ReadinessUtil.ReadinessName(baseReadiness), baseReadiness.ToString()));
            ddReadiness.Items.Add(new ListItem(ReadinessUtil.ReadinessName(additionalReadiness), additionalReadiness.ToString()));
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
        private void RefreshDataGrid()
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
            ReportVesselManageFilter filter = CollectFilterData();

            //Get the list of records according to the specified filters, order and paging
            List<ReportVesselManageBlock> reportVesselManageBlocks = ReportVesselUtil.GetAllReportVesselManageBlocks(filter, pageLength, CurrentUser);

            //No data found
            if (reportVesselManageBlocks.Count == 0)
            {
                html = "<span>Няма намерени резултати</span>";
                this.btnPrintReportAllVessels.Visible = false;
                this.btnExport.Visible = false;
            }
            //If there is data then generate dynamically the HTML for the data grid
            else 
            {
                this.btnPrintReportAllVessels.Visible = true;
                this.btnExport.Visible = true;

                bool isNormativeTechnicsHidden = GetUIItemAccessLevel("RES_REPORTS_VESSELS_NORMATIVETECHNICS") == UIAccessLevel.Hidden;
                bool isVesselNameHidden = GetUIItemAccessLevel("RES_REPORTS_VESSELS_VESSELNAME") == UIAccessLevel.Hidden;
                bool isInventoryNumberHidden = GetUIItemAccessLevel("RES_REPORTS_VESSELS_INVENTORYNUMBER") == UIAccessLevel.Hidden;
                bool isTechnicsCategoryHidden = GetUIItemAccessLevel("RES_REPORTS_VESSELS_TECHNICSCATEGORY") == UIAccessLevel.Hidden;
                bool isVesselKindHidden = GetUIItemAccessLevel("RES_REPORTS_VESSELS_VESSELKIND") == UIAccessLevel.Hidden;
                bool isVesselTypeHidden = GetUIItemAccessLevel("RES_REPORTS_VESSELS_VESSELTYPE") == UIAccessLevel.Hidden;
                bool isMilDepartmentHidden = GetUIItemAccessLevel("RES_REPORTS_VESSELS_MILDEPARTMENT") == UIAccessLevel.Hidden;
                bool isMilRepStatusHidden = GetUIItemAccessLevel("RES_REPORTS_VESSELS_MILREPSTATUS") == UIAccessLevel.Hidden;
                bool isOwnershipHidden = GetUIItemAccessLevel("RES_REPORTS_VESSELS_OWNERSHIP") == UIAccessLevel.Hidden;
                bool isAddressHidden = GetUIItemAccessLevel("RES_REPORTS_VESSELS_ADDRESS") == UIAccessLevel.Hidden;


                string headerStyle = "vertical-align: bottom;";
                int orderCol = (orderBy > 100 ? orderBy - 100 : orderBy);
                string img = orderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
                string[] arrOrderCol = { "", "", "", "", "", "", "", "", "", "" };
                arrOrderCol[orderCol - 1] = @"<div style='Position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

                //Setup the header of the grid
                html = @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>" +
(!isNormativeTechnicsHidden ? "<th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(10);'>Нормативна категория" + arrOrderCol[9] + @"</th>" : "") +
       (!isVesselNameHidden ? "<th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Име" + arrOrderCol[0] + @"</th>" : "") +
  (!isInventoryNumberHidden ? "<th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);' title='Инвентарен номер'>Инв. номер" + arrOrderCol[1] + @"</th>" : "") +
 (!isTechnicsCategoryHidden ? "<th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>Категория" + arrOrderCol[2] + @"</th>" : "") +
       (!isVesselKindHidden ? "<th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(4);'>Вид" + arrOrderCol[3] + @"</th>" : "") +
       (!isVesselTypeHidden ? "<th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(5);'>Тип" + arrOrderCol[4] + @"</th>" : "") + 
    (!isMilDepartmentHidden ? "<th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(6);'>На отчет в" + arrOrderCol[5] + @"</th>" : "") +
     (!isMilRepStatusHidden ? "<th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(7);'>Състояние по отчета" + arrOrderCol[6] + @"</th>" : "") +
        (!isOwnershipHidden ? "<th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(8);'>Собственик" + arrOrderCol[7] + @"</th>" : "") +
        (!isAddressHidden ? "<th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(9);'>Адрес" + arrOrderCol[8] + @"</th>" : "") + @"
      
                            </tr>
                         </thead>";

                int counter = 1;

                //Iterate through all items and add them into the grid
                foreach (ReportVesselManageBlock vesselManageBlock in reportVesselManageBlocks)
                {
                    string cellStyle = "vertical-align: top;";

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='" + cellStyle + @"'>" + ((pageIdx - 1) * pageLength + counter).ToString() + @"</td>" +
  (!isNormativeTechnicsHidden ? "<td style='" + cellStyle + @"' title='" + vesselManageBlock.NormativeTechnicsName + @"'>" + vesselManageBlock.NormativeTechnicsCode + @"</td>" : "") +
         (!isVesselNameHidden ? "<td style='" + cellStyle + @"'>" + vesselManageBlock.VesselName + @"</td>" : "") +
    (!isInventoryNumberHidden ? "<td style='" + cellStyle + @"'>" + vesselManageBlock.InventoryNumber + @"</td>" : "") +
   (!isTechnicsCategoryHidden ? "<td style='" + cellStyle + @"'>" + vesselManageBlock.TechnicsCategory + @"</td>" : "") +
         (!isVesselKindHidden ? "<td style='" + cellStyle + @"'>" + vesselManageBlock.VesselKind + @"</td>" : "") +
         (!isVesselTypeHidden ? "<td style='" + cellStyle + @"'>" + vesselManageBlock.VesselType + @"</td>" : "") +
      (!isMilDepartmentHidden ? "<td style='" + cellStyle + @"'>" + vesselManageBlock.MilitaryDepartment + @"</td>" : "") +
       (!isMilRepStatusHidden ? "<td style='" + cellStyle + @"'>" + vesselManageBlock.MilitaryReportStatus + @"</td>" : "") +
          (!isOwnershipHidden ? "<td style='" + cellStyle + @"'>" + vesselManageBlock.Ownership + @"</td>" : "") +
          (!isAddressHidden   ? "<td style='" + cellStyle + @"'>" + vesselManageBlock.Address + @"</td>" : "") + @"
         
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
            if (ValidateData())
            {
                hdnPageIdx.Value = "1";
                RefreshDataGrid();
            }
        }

        //Go to the first page and refresh the grid
        protected void btnFirst_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = "1";
                RefreshDataGrid();
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

                    RefreshDataGrid();
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

                    RefreshDataGrid();
                }
            }
        }

        //Go to the last page and refresh the grid
        protected void btnLast_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = maxPage.ToString();
                RefreshDataGrid();
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
                    RefreshDataGrid();
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

        //Setup any date picker controls on the page by setting the CSS of the target inputs
        //Note that the date picker CSS is common
        //This makes the calendar controls to appear on the page next to each input
        private void SetupDatePickers()
        {
        }


        //Collect the filet information from the page
        private ReportVesselManageFilter CollectFilterData()
        {
            ReportVesselManageFilter filter = new ReportVesselManageFilter();

            string technicsCategory = "";

            if (ddTechnicsCategory.SelectedValue != ListItems.GetOptionAll().Value)
            {
                technicsCategory = ddTechnicsCategory.SelectedValue;
                this.hdnTechnicsCategoryId.Value = ddTechnicsCategory.SelectedValue;
            }
            else
            {
                this.hdnTechnicsCategoryId.Value = "";
            }

            string vesselKind = "";

            if (ddVesselKind.SelectedValue != ListItems.GetOptionAll().Value)
            {
                vesselKind = ddVesselKind.SelectedValue;
                this.hdnVesselKindId.Value = ddVesselKind.SelectedValue;
            }
            else
            {
                this.hdnVesselKindId.Value = "";
            }

            string vesselType = "";

            if (ddVesselType.SelectedValue != ListItems.GetOptionAll().Value)
            {
                vesselType = ddVesselType.SelectedValue;
                this.hdnVesselTypeId.Value = ddVesselType.SelectedValue;
            }
            else
            {
                this.hdnVesselTypeId.Value = "";
            }    

            int orderBy;
            if (!int.TryParse(hdnSortBy.Value, out orderBy))
                orderBy = 0;

            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            this.hdnVesselName.Value = txtVesselName.Text;
            this.hdnInventoryNumber.Value = txtInventoryNumber.Text;

            string militaryReportStatus = "";

            if (ddMilitaryReportStatus.SelectedValue != ListItems.GetOptionAll().Value)
            {
                militaryReportStatus = ddMilitaryReportStatus.SelectedValue;
                this.hdnMilitaryReportStatusId.Value = ddMilitaryReportStatus.SelectedValue;
            }
            else
            {
                this.hdnMilitaryReportStatusId.Value = "";
            }

            string militaryDepartment = "";

            if (ddMilitaryDepartment.SelectedValue != ListItems.GetOptionAll().Value)
            {
                militaryDepartment = ddMilitaryDepartment.SelectedValue;
                this.hdnMilitaryDepartmentId.Value = ddMilitaryDepartment.SelectedValue;
            }
            else
            {
                this.hdnMilitaryDepartmentId.Value = "";
            }

            hdnOwnershipNumber.Value = txtOwnershipNumber.Text;
            hdnOwnershipName.Value = txtOwnershipName.Text;

            bool isOwnershipAddress;

            if (rblAddress.SelectedValue == "1")
                isOwnershipAddress = true;
            else
                isOwnershipAddress = false;

            this.hdnIsOwnershipAddress.Value = (isOwnershipAddress ? "1" : "0");

            string region = "";

            if (ddRegion.SelectedValue != ListItems.GetOptionAll().Value)
            {
                region = ddRegion.SelectedValue;
                this.hdnRegionId.Value = this.ddRegion.SelectedValue;
            }
            else
            {
                this.hdnRegionId.Value = "";
            }

            string municiplaity = "";

            if (ddMuniciplaity.SelectedValue != ListItems.GetOptionAll().Value)
            {
                municiplaity = ddMuniciplaity.SelectedValue;
                this.hdnMunicipalityId.Value = this.ddMuniciplaity.SelectedValue;
            }
            else
            {
                this.hdnMunicipalityId.Value = "";
            }

            string city = "";

            if (ddCity.SelectedValue != ListItems.GetOptionAll().Value)
            {
                city = ddCity.SelectedValue;
                this.hdnCityId.Value = this.ddCity.SelectedValue;
            }
            else
            {
                this.hdnCityId.Value = "";
            }

            string district = "";

            if (ddDistrict.SelectedValue != ListItems.GetOptionAll().Value)
            {
                district = ddDistrict.SelectedValue;
                this.hdnDistrictId.Value = this.ddDistrict.SelectedValue;
            }
            else
            {
                this.hdnDistrictId.Value = "";
            }

            this.hdnPostCode.Value = txtPostCode.Text;
            this.hdnAddress.Value = txtAddress.Text;

            string normativeTechnics = "";

            if (ddNormativeTechnics.SelectedValue != ListItems.GetOptionAll().Value)
            {
                normativeTechnics = ddNormativeTechnics.SelectedValue;
                this.hdnNormativeTechnicsId.Value = this.ddNormativeTechnics.SelectedValue;
            }
            else
            {
                this.hdnNormativeTechnicsId.Value = "";
            }

            string appointmentIsDelivered = "";

            if (ddAppointmentIsDelivered.SelectedValue != ListItems.GetOptionAll().Value)
            {
                appointmentIsDelivered = ddAppointmentIsDelivered.SelectedValue;
                this.hdnAppointmentIsDelivered.Value = this.ddAppointmentIsDelivered.SelectedValue;
            }
            else
            {
                this.hdnAppointmentIsDelivered.Value = "";
            }

            string readiness = "";

            if (ddReadiness.SelectedValue != ListItems.GetOptionAll().Value)
            {
                readiness = ddReadiness.SelectedValue;
                this.hdnReadiness.Value = this.ddReadiness.SelectedValue;
            }
            else
            {
                this.hdnReadiness.Value = "";
            }

            filter.VesselName = txtVesselName.Text;
            filter.InventoryNumber = txtInventoryNumber.Text;
            filter.TechnicsCategoryId = technicsCategory;
            filter.VesselKindId = vesselKind;
            filter.VesselTypeId = vesselType;
            filter.MilitaryReportStatus = militaryReportStatus;
            filter.MilitaryDepartment = militaryDepartment;
            filter.OwnershipNumber = txtOwnershipNumber.Text;
            filter.OwnershipName = txtOwnershipName.Text;
            filter.IsOwnershipAddress = isOwnershipAddress;
            filter.PostCode = txtPostCode.Text;
            filter.Region = region;
            filter.Municipality = municiplaity;
            filter.City = city;
            filter.District = district;
            filter.Address = txtAddress.Text;
            filter.NormativeTechnics = normativeTechnics;
            filter.AppointmentIsDelivered = appointmentIsDelivered;
            filter.Readiness = readiness;

            filter.OrderBy = orderBy;
            filter.PageIdx = pageIdx;

            return filter;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtVesselName.Text = "";
            txtInventoryNumber.Text = "";
            ddTechnicsCategory.SelectedValue = ListItems.GetOptionAll().Value;
            ddVesselKind.SelectedValue = ListItems.GetOptionAll().Value;
            ddVesselType.SelectedValue = ListItems.GetOptionAll().Value;
            ddMilitaryReportStatus.SelectedValue = ListItems.GetOptionAll().Value;
            ddMilitaryDepartment.SelectedValue = ListItems.GetOptionAll().Value;
            txtOwnershipNumber.Text = "";
            txtOwnershipName.Text = "";
            rblAddress.SelectedValue = "1";
            ddRegion.SelectedValue = ListItems.GetOptionAll().Value;
            ddMuniciplaity.Items.Clear();
            ddCity.Items.Clear();
            ddDistrict.Items.Clear();
            txtPostCode.Text = "";
            txtAddress.Text = "";
            ddNormativeTechnics.SelectedValue = ListItems.GetOptionAll().Value;
            ddAppointmentIsDelivered.SelectedValue = ListItems.GetOptionAll().Value;
            ddReadiness.SelectedValue = ListItems.GetOptionAll().Value;
        }
    }
}
