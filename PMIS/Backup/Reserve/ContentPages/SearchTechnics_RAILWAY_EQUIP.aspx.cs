﻿using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class SearchTechnics_RAILWAY_EQUIP : RESPage
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
                return "RES_EQUIPTECHREQUESTS_FULFIL_MILCOMMAND_POSITIONS_FULFIL";
            }
        }

        public string TechnicsTypeKey
        {
            get
            {
                return "RAILWAY_EQUIP";
            }
        }

        public string TechnicsTypeName
        {
            get;
            set;
        }

        //This property represents the ID of the EquipmentTechnicsRequest object that is loaded on the screen
        //It is stored in a hidden field on the page
        private int EquipmentTechnicsRequestId
        {
            get
            {
                int equipmentTechnicsRequestId = 0;
                if (String.IsNullOrEmpty(this.hfEquipmentTechnicsRequestID.Value)
                    || this.hfEquipmentTechnicsRequestID.Value == "0")
                {
                    if (Request.Params["EquipmentTechnicsRequestId"] != null)
                        int.TryParse(Request.Params["EquipmentTechnicsRequestId"].ToString(), out equipmentTechnicsRequestId);

                    this.hfEquipmentTechnicsRequestID.Value = equipmentTechnicsRequestId.ToString();
                }
                else
                {
                    Int32.TryParse(this.hfEquipmentTechnicsRequestID.Value, out equipmentTechnicsRequestId);
                }

                return equipmentTechnicsRequestId;
            }

            set
            {
                this.hfEquipmentTechnicsRequestID.Value = value.ToString();
            }
        }

        //This property represents the ID of the MilitaryDepartment object that is loaded on the screen
        //It is stored in a hidden field on the page
        private int MilitaryDepartmentId
        {
            get
            {
                int militaryDepartmentId = 0;
                if (String.IsNullOrEmpty(this.hfMilitaryDepartmentID.Value)
                    || this.hfMilitaryDepartmentID.Value == "0")
                {
                    if (Request.Params["MilitaryDepartmentId"] != null)
                        int.TryParse(Request.Params["MilitaryDepartmentId"].ToString(), out militaryDepartmentId);

                    this.hfMilitaryDepartmentID.Value = militaryDepartmentId.ToString();
                }
                else
                {
                    Int32.TryParse(this.hfMilitaryDepartmentID.Value, out militaryDepartmentId);
                }

                return militaryDepartmentId;
            }

            set
            {
                this.hfMilitaryDepartmentID.Value = value.ToString();
            }
        }

        //This property represents the ID of the MilitaryCommand object for which is the selection
        //It is stored in a hidden field on the page
        private int MilitaryCommandID
        {
            get
            {
                int militaryCommandID = 0;
                if (String.IsNullOrEmpty(this.hfMilitaryCommandID.Value)
                    || this.hfMilitaryCommandID.Value == "0")
                {
                    if (Request.Params["MilitaryCommandID"] != null)
                        int.TryParse(Request.Params["MilitaryCommandID"].ToString(), out militaryCommandID);

                    this.hfMilitaryCommandID.Value = militaryCommandID.ToString();
                }
                else
                {
                    Int32.TryParse(this.hfMilitaryCommandID.Value, out militaryCommandID);
                }

                return militaryCommandID;
            }

            set
            {
                this.hfMilitaryCommandID.Value = value.ToString();
            }
        }

        //This property represents the ID of the RequestCommandPositionID object that is loaded on the screen
        //It is stored in a hidden field on the page
        private int TechnicsRequestCommandPositionID
        {
            get
            {
                int technicsRequestCommandPositionId = 0;
                if (String.IsNullOrEmpty(this.hfTechnicsRequestCommandPositionID.Value)
                    || this.hfTechnicsRequestCommandPositionID.Value == "0")
                {
                    if (Request.Params["TechnicsRequestCommandPositionID"] != null)
                        int.TryParse(Request.Params["TechnicsRequestCommandPositionID"].ToString(), out technicsRequestCommandPositionId);

                    this.hfTechnicsRequestCommandPositionID.Value = technicsRequestCommandPositionId.ToString();
                }
                else
                {
                    Int32.TryParse(this.hfTechnicsRequestCommandPositionID.Value, out technicsRequestCommandPositionId);
                }

                return technicsRequestCommandPositionId;
            }

            set
            {
                this.hfTechnicsRequestCommandPositionID.Value = value.ToString();
            }
        }

        //This property represents the Readiness(base or additional) of reservist to be choosen
        //It is stored in a hidden field on the page
        private int Readiness
        {
            get
            {
                int readiness = 0;
                if (String.IsNullOrEmpty(this.hfReadiness.Value)
                    || this.hfReadiness.Value == "0")
                {
                    if (Request.Params["Readiness"] != null)
                        int.TryParse(Request.Params["Readiness"].ToString(), out readiness);

                    this.hfReadiness.Value = readiness.ToString();
                }
                else
                {
                    Int32.TryParse(this.hfReadiness.Value, out readiness);
                }

                return readiness;
            }

            set
            {
                this.hfReadiness.Value = value.ToString();
            }
        }

        //This property represents the FromFulfilByCommand flag indicating that the opener screen is By Command
        //It is stored in a hidden field on the page
        private int FromFulfilByCommand
        {
            get
            {
                int fromFulfilByCommand = 0;
                if (String.IsNullOrEmpty(this.hfFromFulfilByCommand.Value)
                    || this.hfFromFulfilByCommand.Value == "0")
                {
                    if (Request.Params["FromFulfilByCommand"] != null)
                        int.TryParse(Request.Params["FromFulfilByCommand"].ToString(), out fromFulfilByCommand);

                    this.hfFromFulfilByCommand.Value = fromFulfilByCommand.ToString();
                }
                else
                {
                    Int32.TryParse(this.hfFromFulfilByCommand.Value, out fromFulfilByCommand);
                }

                return fromFulfilByCommand;
            }

            set
            {
                this.hfFromFulfilByCommand.Value = value.ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if ((GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL_MILCOMMAND_POSITIONS_FULFIL") != UIAccessLevel.Enabled) ||
                (GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL_MILCOMMAND_POSITIONS_FULFIL_SEARCHRAILWAYEQUIP") != UIAccessLevel.Enabled))
            {
                RedirectAccessDenied();
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSChooseRailwayEquip")
            {
                JSChooseRailwayEquip();
                return;
            }

            //Setup hidden fields
            EquipmentTechnicsRequestId = EquipmentTechnicsRequestId;
            MilitaryDepartmentId = MilitaryDepartmentId;
            MilitaryCommandID = MilitaryCommandID;
            TechnicsRequestCommandPositionID = TechnicsRequestCommandPositionID;
            Readiness = Readiness;

            TechnicsType technicsType = TechnicsTypeUtil.GetTechnicsType(TechnicsTypeKey, CurrentUser);
            TechnicsTypeName = technicsType.TypeName;

            EquipmentTechnicsRequest request = EquipmentTechnicsRequestUtil.GetEquipmentTechnicsRequest(EquipmentTechnicsRequestId, CurrentUser);

            lblHeaderTitle.InnerHtml = "Търсене на " + technicsType.TypeName.ToLower() + " за попълване на заявка №" + request.RequestNumber + " / " + CommonFunctions.FormatDate(request.RequestDate);
            this.Title = lblHeaderTitle.InnerHtml;

            lblOwnershipNumber.Text = "ЕГН/" + CommonFunctions.GetLabelText("UnifiedIdentityCode") + ":";

            lblSubHeaderTitle.InnerHtml = ReadinessUtil.ReadinessName(Readiness);

            //Setup any calendar control on the screen
            SetupDatePickers();

            //Hilight the current page in the menu bar
            HighlightMenuItems("Equipment");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            //Setup some basic styles on the screen
            SetupStyle();            

            //Collect the filter information to be able to pull the number of rows for this specific filter
            RailwayEquipSearchFilter filter = CollectFilterData();

            int allRows = RailwayEquipUtil.GetAllRailwayEquipSearchBlocksCount(filter, TechnicsRequestCommandPositionID, MilitaryDepartmentId, CurrentUser);
            //Get the number of rows and calculate the number of pages in the grid
            maxPage = pageLength == 0 ? 1 : allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);
            
            //When the page is loaded for the first time
            if (!IsPostBack)
            {
                //Populate any drop-downs and list-boxes
                PopulateLists();

                PreFillFilter();

                //The default order is by normative technics
                if (string.IsNullOrEmpty(hdnSortBy.Value))
                    hdnSortBy.Value = "7";

                //Do not 'Simulate clicking the Refresh button to load the grid initially' to prevent slow loading
                //btnRefresh_Click(btnRefresh, new EventArgs());
            }

            lblMessage.Text = "";
            lblGridMessage.Text = "";
        }

        //Populate all listboxes on the screen
        private void PopulateLists()
        {
            PopulateTechnicsCategory();
            PopulateRailwayEquipKind();
            PopulateRailwayEquipType();
            PopulateRegions();
            PopulateNormativeTechnics();

            ListItems.SetTextAsTooltip(ddTechnicsCategory,
                                       ddRailwayEquipKind,
                                       ddRailwayEquipType,
                                       ddNormativeTechnics);
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

        //Populate ddRailwayEquipKind
        private void PopulateRailwayEquipKind()
        {
            ddRailwayEquipKind.DataSource = GTableItemUtil.GetAllGTableItemsByTableName("RailwayEquipKind", ModuleKey, 1, 0, 0, CurrentUser);
            ddRailwayEquipKind.DataTextField = "TableValue";
            ddRailwayEquipKind.DataValueField = "TableKey";
            ddRailwayEquipKind.DataBind();
            ddRailwayEquipKind.Items.Insert(0, ListItems.GetOptionAll());
        }

        //Populate ddRailwayEquipType
        private void PopulateRailwayEquipType()
        {
            ddRailwayEquipType.DataSource = GTableItemUtil.GetAllGTableItemsByTableName("RailwayEquipType", ModuleKey, 1, 0, 0, CurrentUser);
            ddRailwayEquipType.DataTextField = "TableValue";
            ddRailwayEquipType.DataValueField = "TableKey";
            ddRailwayEquipType.DataBind();
            ddRailwayEquipType.Items.Insert(0, ListItems.GetOptionAll());
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
            pnlPaging.Visible = true;

            string html = "";

            bool IsNormativeTechnicsHidden = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL_MILCOMMAND_POSITIONS_FULFIL_SEARCHRAILWAYEQUIP_NORMATIVETECHNICS") == UIAccessLevel.Hidden;
            bool IsInventoryNumberHidden = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL_MILCOMMAND_POSITIONS_FULFIL_SEARCHRAILWAYEQUIP_INVENTNUMBER") == UIAccessLevel.Hidden;
            bool IsTechnicsCategoryHidden = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL_MILCOMMAND_POSITIONS_FULFIL_SEARCHRAILWAYEQUIP_TECHNICSCATEGORY") == UIAccessLevel.Hidden;
            bool IsRailwayEquipKindHidden = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL_MILCOMMAND_POSITIONS_FULFIL_SEARCHRAILWAYEQUIP_RAILWAYEQUIPKIND") == UIAccessLevel.Hidden;
            bool IsRailwayEquipTypeHidden = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL_MILCOMMAND_POSITIONS_FULFIL_SEARCHRAILWAYEQUIP_RAILWAYEQUIPTYPE") == UIAccessLevel.Hidden;
            bool IsOwnershipHidden = this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL_MILCOMMAND_POSITIONS_FULFIL_SEARCHRAILWAYEQUIP_OWNERSHIP") == UIAccessLevel.Hidden;

            //Collect the filters, the order control and the paging control data from the page
            int orderBy;
            if (!int.TryParse(hdnSortBy.Value, out orderBy))
                orderBy = 0;

            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            //Collect the filter information to be able to pull the number of rows for this specific filter
            RailwayEquipSearchFilter filter = CollectFilterData();

            //Get the list of records according to the specified filters, order and paging
            List<RailwayEquipSearchBlock> railwayEquipSearchBlocks = RailwayEquipUtil.GetAllRailwayEquipSearchBlocks(filter, TechnicsRequestCommandPositionID, MilitaryDepartmentId, pageLength, CurrentUser);

            //No data found
            if (railwayEquipSearchBlocks.Count == 0)
            {
                html = "<span>Няма намерени резултати</span>";
            }
            //If there is data then generate dynamically the HTML for the data grid
            else 
            {
                string headerStyle = "vertical-align: bottom;";
                int orderCol = (orderBy > 100 ? orderBy - 100 : orderBy);
                string img = orderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
                string[] arrOrderCol = { "", "", "", "", "", "", "", "", "" };
                arrOrderCol[orderCol - 1] = @"<div style='Position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

                //Setup the header of the grid
                html = @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='width: 20px;" + headerStyle + @"'></th>" +
(!IsNormativeTechnicsHidden ? @"<th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(7);'>Нормативна категория" + arrOrderCol[6] + @"</th>" : "") +
 (!IsInventoryNumberHidden ? @"<th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Инвентарен номер" + arrOrderCol[0] + @"</th>" : "") +
(!IsTechnicsCategoryHidden ? @"<th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>Категория" + arrOrderCol[1] + @"</th>" : "") +
(!IsRailwayEquipKindHidden ? @"<th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>Вид" + arrOrderCol[2] + @"</th>" : "") +
(!IsRailwayEquipTypeHidden ? @"<th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(4);'>Тип" + arrOrderCol[3] + @"</th>" : "") +
                             @"<th style='width: 60px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(5);'>Брой" + arrOrderCol[4] + @"</th>" +
       (!IsOwnershipHidden ? @"<th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(6);'>Собственик" + arrOrderCol[5] + @"</th>" : "") + @"    
                            </tr>
                         </thead>";

                int counter = 1;

                //Iterate through all items and add them into the grid
                foreach (RailwayEquipSearchBlock railwayEquipSearchBlock in railwayEquipSearchBlocks)
                {
                    string cellStyle = "vertical-align: top;";

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"' onmouseover=""this.className = 'SelectionItem';"" onmouseout=""this.className = '" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"';"">
                                 <td style='" + cellStyle + @"'>" + ((pageIdx - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'><img src='../Images/pawn_view.png' alt='Преглед' title='Преглед' class='GridActionIcon' onclick='PreviewTechnics(" + railwayEquipSearchBlock.TechnicsId.ToString() + @");' /></td>" +
 (!IsNormativeTechnicsHidden ? @"<td style='" + cellStyle + @"' onclick=""ShowChooseItemsCountLightBox(" + railwayEquipSearchBlock.TechnicsId + @", " + railwayEquipSearchBlock.RailwayEquipId + @", " + railwayEquipSearchBlock.ItemsCount + @");"" title='" + railwayEquipSearchBlock.NormativeTechnicsName + @"'>" + railwayEquipSearchBlock.NormativeTechnicsCode + @"</td>" : "") +
   (!IsInventoryNumberHidden ? @"<td style='" + cellStyle + @"' onclick=""ShowChooseItemsCountLightBox(" + railwayEquipSearchBlock.TechnicsId + @", " + railwayEquipSearchBlock.RailwayEquipId + @", " + railwayEquipSearchBlock.ItemsCount + @");"" title='Избери'>" + railwayEquipSearchBlock.InventoryNumber + @"</td>" : "") +
  (!IsTechnicsCategoryHidden ? @"<td style='" + cellStyle + @"' onclick=""ShowChooseItemsCountLightBox(" + railwayEquipSearchBlock.TechnicsId + @", " + railwayEquipSearchBlock.RailwayEquipId + @", " + railwayEquipSearchBlock.ItemsCount + @");"" title='Избери'>" + railwayEquipSearchBlock.TechnicsCategory + @"</td>" : "") +
  (!IsRailwayEquipKindHidden ? @"<td style='" + cellStyle + @"' onclick=""ShowChooseItemsCountLightBox(" + railwayEquipSearchBlock.TechnicsId + @", " + railwayEquipSearchBlock.RailwayEquipId + @", " + railwayEquipSearchBlock.ItemsCount + @");"" title='Избери'>" + railwayEquipSearchBlock.RailwayEquipKind + @"</td>" : "") +
  (!IsRailwayEquipTypeHidden ? @"<td style='" + cellStyle + @"' onclick=""ShowChooseItemsCountLightBox(" + railwayEquipSearchBlock.TechnicsId + @", " + railwayEquipSearchBlock.RailwayEquipId + @", " + railwayEquipSearchBlock.ItemsCount + @");"" title='Избери'>" + railwayEquipSearchBlock.RailwayEquipType + @"</td>" : "") +
                               @"<td style='" + cellStyle + @"' onclick=""ShowChooseItemsCountLightBox(" + railwayEquipSearchBlock.TechnicsId + @", " + railwayEquipSearchBlock.RailwayEquipId + @", " + railwayEquipSearchBlock.ItemsCount + @");"" title='Избери'>" + railwayEquipSearchBlock.ItemsCount + @"</td>" +
         (!IsOwnershipHidden ? @"<td style='" + cellStyle + @"' onclick=""ShowChooseItemsCountLightBox(" + railwayEquipSearchBlock.TechnicsId + @", " + railwayEquipSearchBlock.RailwayEquipId + @", " + railwayEquipSearchBlock.ItemsCount + @");"" title='Избери'>" + railwayEquipSearchBlock.Ownership + @"</td>" : "") + @"
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
            if (FromFulfilByCommand == 1)
            {
                Response.Redirect("~/ContentPages/FulfilTechnicsMilCommand.aspx?MilitaryDepartmentId=" + MilitaryDepartmentId +
                                                                                "&MilitaryCommandID=" + MilitaryCommandID);
            }
            else
            {
                Response.Redirect("~/ContentPages/FulfilEquipmentTechnicsRequest.aspx?EquipmentTechnicsRequestId=" + EquipmentTechnicsRequestId +
                                                                                        "&MilitaryDepartmentId=" + MilitaryDepartmentId +
                                                                                        "&MilitaryCommandID=" + MilitaryCommandID);
            }
        }          

        //Setup any date picker controls on the page by setting the CSS of the target inputs
        //Note that the date picker CSS is common
        //This makes the calendar controls to appear on the page next to each input
        private void SetupDatePickers()
        {
        }

        
        //Collect the filet information from the page
        private RailwayEquipSearchFilter CollectFilterData()
        {
            RailwayEquipSearchFilter filter = new RailwayEquipSearchFilter();

            string technicsCategory = "";

            if (ddTechnicsCategory.SelectedValue != ListItems.GetOptionAll().Value)
                technicsCategory = ddTechnicsCategory.SelectedValue;

            string railwayEquipKind = "";

            if (ddRailwayEquipKind.SelectedValue != ListItems.GetOptionAll().Value)
                railwayEquipKind = ddRailwayEquipKind.SelectedValue;

            string railwayEquipType = "";

            if (ddRailwayEquipType.SelectedValue != ListItems.GetOptionAll().Value)
                railwayEquipType = ddRailwayEquipType.SelectedValue;

            bool isOwnershipAddress;

            if (rblAddress.SelectedValue == "1")
                isOwnershipAddress = true;
            else
                isOwnershipAddress = false;
            
            string region = "";

            if (ddRegion.SelectedValue != ListItems.GetOptionAll().Value)
            {
                region = ddRegion.SelectedValue;                
            }           

            string municiplaity = "";

            if (ddMuniciplaity.SelectedValue != ListItems.GetOptionAll().Value)
            {
                municiplaity = ddMuniciplaity.SelectedValue;                
            }           

            string city = "";

            if (ddCity.SelectedValue != ListItems.GetOptionAll().Value)
            {
                city = ddCity.SelectedValue;                
            }            

            string district = "";

            if (ddDistrict.SelectedValue != ListItems.GetOptionAll().Value)
            {
                district = ddDistrict.SelectedValue;                
            }

            string normativeTechnics = "";

            if (ddNormativeTechnics.SelectedValue != ListItems.GetOptionAll().Value)
                normativeTechnics = ddNormativeTechnics.SelectedValue;

            int orderBy;
            if (!int.TryParse(hdnSortBy.Value, out orderBy))
                orderBy = 0;

            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            filter.InventoryNumber = txtInventoryNumber.Text;
            filter.TechnicsCategoryId = technicsCategory;
            filter.RailwayEquipKindId = railwayEquipKind;
            filter.RailwayEquipTypeId = railwayEquipType;
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

            filter.OrderBy = orderBy;
            filter.PageIdx = pageIdx;

            return filter;
        }

        private void JSChooseRailwayEquip()
        {
            string stat = "";
            string response = "";

            int technicsID = int.Parse(Request.Form["TechnicsID"]);
            int railwayEquipID = int.Parse(Request.Form["RailwayEquipID"]);
            int itemsCount = int.Parse(Request.Form["ItemsCount"]);
            int technicsRequestCommandPositionID = int.Parse(Request.Form["TechnicsRequestCommandPositionID"]);
            int militaryDepartmentID = int.Parse(Request.Form["MilitaryDepartmentID"]);
            int readiness = int.Parse(Request.Form["Readiness"]);

            FillTechnicsRequest fillTechnicsRequest = new FillTechnicsRequest(CurrentUser);
            fillTechnicsRequest.TechnicsID = technicsID;
            fillTechnicsRequest.TechnicsRequestCommandPositionID = technicsRequestCommandPositionID;
            fillTechnicsRequest.MilitaryDepartmentID = militaryDepartmentID;
            fillTechnicsRequest.TechnicReadinessID = readiness;

            try
            {
                List<FillTechnicsRequest> existingFulfillments = FillTechnicsRequestUtil.GetFillTechnicsRequestByTechnicsId(technicsID, CurrentUser);

                if (existingFulfillments.Count == 0)
                {
                    Change change = new Change(CurrentUser, "RES_EquipmentTechnicsRequests");

                    Technics technics = TechnicsUtil.GetTechnics(technicsID, CurrentUser);

                    //If there are chosen less items than the available count then 
                    //split the current Technics record into two separate records and adjust the ItemsCount values
                    if (itemsCount < technics.ItemsCount)
                    {
                        RailwayEquipUtil.SplitRailwayEquip(technicsID, itemsCount, CurrentUser, change);
                    }

                    ChangeEvent changeEvent = null;

                    MilitaryDepartment militaryDepartment = MilitaryDepartmentUtil.GetMilitaryDepartment(fillTechnicsRequest.MilitaryDepartmentID, CurrentUser);

                    TechnicsRequestCommandPosition position = TechnicsRequestCommandPositionUtil.GetTechnicsRequestCommandPosition(CurrentUser, fillTechnicsRequest.TechnicsRequestCommandPositionID);

                    RailwayEquip railwayEquip = RailwayEquipUtil.GetRailwayEquip(railwayEquipID, CurrentUser);

                    string logDescription = "";
                    logDescription += "Заявка №: " + position.TechnicsRequestsCommand.EquipmentTechnicsRequest.RequestNumber + " / Дата: " + CommonFunctions.FormatDate(position.TechnicsRequestsCommand.EquipmentTechnicsRequest.RequestDate) +
                                      "; Команда: " + position.TechnicsRequestsCommand.MilitaryCommand.DisplayTextForSelection +
                                      "; Коментар: " + position.Comment +
                                      "; ВО: " + militaryDepartment.MilitaryDepartmentName +
                                      "; Вид резерв: " + fillTechnicsRequest.TechnicReadiness +
                                      "; Инв. №: " + railwayEquip.InventoryNumber +
                                      "; Брой: " + itemsCount.ToString();

                    changeEvent = new ChangeEvent("RES_EquipTechRequests_AddRailwayEquip", logDescription, position.TechnicsRequestsCommand.EquipmentTechnicsRequest.MilitaryUnit, null, CurrentUser);

                    FillTechnicsRequestUtil.SaveRequestCommandTechnic(fillTechnicsRequest, CurrentUser, changeEvent);

                    if (changeEvent != null)
                        change.AddEvent(changeEvent);

                    //Change the current Military Reporting Status of the chosen technics
                    TechnicsMilRepStatusUtil.SetMilRepStatusTo_MOBILE_APPOINTMENT(technicsID, CurrentUser, change);

                    //Add a new TechnicsAppointment for the chosen railway equip
                    TechnicsRequestCommandPosition requestCommandPosition = TechnicsRequestCommandPositionUtil.GetTechnicsRequestCommandPosition(CurrentUser, fillTechnicsRequest.TechnicsRequestCommandPositionID);

                    TechnicsAppointment technicsAppointment = new TechnicsAppointment(CurrentUser);

                    technicsAppointment.TechnicsId = fillTechnicsRequest.TechnicsID;
                    technicsAppointment.IsCurrent = true;
                    technicsAppointment.ReqOrderNumber = requestCommandPosition.TechnicsRequestsCommand.EquipmentTechnicsRequest.RequestNumber;
                    technicsAppointment.EquipmentTechnicsRequestId = requestCommandPosition.TechnicsRequestsCommand.EquipmentTechnicsRequestId;
                    technicsAppointment.ReceiptAppointmentDate = DateTime.Now; //TODO? Should we change this?
                    technicsAppointment.MilitaryCommandName = requestCommandPosition.TechnicsRequestsCommand.MilitaryCommand.DisplayTextForSelection;
                    technicsAppointment.MilitaryCommandSuffix = requestCommandPosition.TechnicsRequestsCommand.MilitaryCommandSuffix;
                    technicsAppointment.MilitaryCommandId = requestCommandPosition.TechnicsRequestsCommand.MilitaryCommand.MilitaryCommandId;
                    technicsAppointment.TechnicsReadinessId = fillTechnicsRequest.TechnicReadinessID;
                    technicsAppointment.Comment = requestCommandPosition.Comment;
                    technicsAppointment.AppointmentTime = requestCommandPosition.TechnicsRequestsCommand.AppointmentTime;
                    technicsAppointment.AppointmentPlace = ""; //TODO Currently we do not support this
                    technicsAppointment.FillTechnicsRequestId = fillTechnicsRequest.FillTechnicsRequestID;

                    TechnicsAppointmentUtil.AddTechnicsAppointment(technicsAppointment, CurrentUser, change);

                    change.WriteLog();

                    stat = AJAXTools.OK;
                    response = "<status>OK</status>";
                }
                else
                {
                    stat = AJAXTools.OK;
                    response = @"<status>EXISTINGFULFIL</status>
                                 <message>Не може да бъде издадено МН, защото вече има такова за избраната техника.</message>";
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

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtInventoryNumber.Text = "";
            ddTechnicsCategory.SelectedValue = ListItems.GetOptionAll().Value;
            ddRailwayEquipKind.SelectedValue = ListItems.GetOptionAll().Value;
            ddRailwayEquipType.SelectedValue = ListItems.GetOptionAll().Value;
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
        }

        private void PreFillFilter()
        {
            TechnicsRequestCommandPosition position = TechnicsRequestCommandPositionUtil.GetTechnicsRequestCommandPosition(CurrentUser, TechnicsRequestCommandPositionID);

            if (position != null)
            {
                if (position.NormativeTechnics != null)
                {
                    ddNormativeTechnics.SelectedValue = position.NormativeTechnics.NormativeTechnicsId.ToString();
                }
            }
        }
    }
}
