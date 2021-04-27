using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class ManageVehicleModels : RESPage
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
                return "RES_LISTMAINT_VEHICLEMODELS";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if ((GetUIItemAccessLevel("RES_LISTMAINT") == UIAccessLevel.Hidden) ||
                (GetUIItemAccessLevel("RES_LISTMAINT_VEHICLEMODELS") == UIAccessLevel.Hidden))
            {
                RedirectAccessDenied();
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadVehicleModelDetails")
            {
                this.JSLoadVehicleModelDetails();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveVehicleModel")
            {
                this.JSSaveVehicleModel();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteVehicleModel")
            {
                this.JSDeleteVehicleModel();
                return;
            }

            //Hilight the current page in the menu bar
            HighlightMenuItems("Lists", "ManageVehicleModels");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            //Setup some basic styles on the screen
            SetupStyle();

            //Collect the filter information to be able to pull the number of rows for this specific filter
            VehicleModelFilter filter = CollectFilterData();

            int allRows = VehicleModelUtil.GetAllVehicleModelsByFilterCount(filter, CurrentUser);
            //Get the number of rows and calculate the number of pages in the grid
            maxPage = pageLength == 0 ? 1 : allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            //When the page is loaded for the first time
            if (!IsPostBack)
            {
                this.imgBtnNewModel.Visible = false;

                //Populate page header title
                SetPageTitle();

                //Populate any drop-downs and list-boxes
                PopulateLists();

                //The default order is by make name
                if (string.IsNullOrEmpty(hdnSortBy.Value))
                    hdnSortBy.Value = "2";

                //Simulate clicking the Refresh button to load the grid initially
                btnRefresh_Click(btnRefresh, new EventArgs());
            }

            //setup AddNewAssessment button
            this.SetBtnNew();

            lblMessage.Text = "";
            lblGridMessage.Text = "";
        }

        private void SetPageTitle()
        {
            lblHeaderTitle.InnerHtml = "Модели автомобили";
        }

        // Setup AddNewAssessment button according to rights of user's role
        private void SetBtnNew()
        {
            if (this.GetUIItemAccessLevel("RES_LISTMAINT_VEHICLEODELS_ADD") == UIAccessLevel.Enabled
                && this.GetUIItemAccessLevel("RES_LISTMAINT_VEHICLEMODELS") == UIAccessLevel.Enabled
                && this.GetUIItemAccessLevel("RES_LISTMAINT") == UIAccessLevel.Enabled)
            {
                EnableButton(imgBtnNewModel);
            }
            else
            {
                if (this.GetUIItemAccessLevel("RES_LISTMAINT_VEHICLEODELS_ADD") != UIAccessLevel.Enabled
                    || this.GetUIItemAccessLevel("RES_LISTMAINT_VEHICLEMODELS") != UIAccessLevel.Enabled
                    || this.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled)
                {
                    HideControl(imgBtnNewModel);
                }
                //else
                //{
                //    DisableButton(imgBtnNewModel);
                //}
            }
        }

        //Populate all listboxes on the screen
        private void PopulateLists()
        {
            PopulateMakes();
        }

        //Populate the Makes drop-down
        private void PopulateMakes()
        {
            this.ddlVehicleMakes.DataSource = VehicleMakeUtil.GetAllVehicleMakes(CurrentUser);
            this.ddlVehicleMakes.DataTextField = "VehicleMakeName";
            this.ddlVehicleMakes.DataValueField = "VehicleMakeId";
            this.ddlVehicleMakes.DataBind();
            this.ddlVehicleMakes.Items.Insert(0, ListItems.GetOptionAll());
        }
        
        //Setup some styling on the page
        private void SetupStyle()
        {

        }

        protected void ddlVehicleMakes_Change(object sender, EventArgs e)
        {
            if (this.ddlVehicleMakes.SelectedValue == ListItems.GetOptionAll().Value)
            {
                this.imgBtnNewModel.Visible = false;
            }
            else
            {
                this.imgBtnNewModel.Visible = true;
            }

            this.SetBtnNew();
            this.RefreshVehicleModels();
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
        private void RefreshVehicleModels()
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
            VehicleModelFilter filter = CollectFilterData();

            //Get the list of records according to the specified filters, order and paging
            List<VehicleModel> vehicleModels = VehicleModelUtil.GetAllVehicleModelsByFilter(filter, CurrentUser);

            //No data found
            if (vehicleModels.Count == 0)
            {
                html = "<span>Няма намерени резултати</span>";
            }
            //If there is data then generate dynamically the HTML for the data grid
            else
            {
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
                               <th style='width: 200px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>Марка" + arrOrderCol[1] + @"</th>
                               <th style='width: 200px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Модел" + arrOrderCol[0] + @"</th>
                               <th style='width: 50px;" + headerStyle + @"'>&nbsp;</th>
                            </tr>
                         </thead>";

                int counter = 1;

                // Get the deleting right for assessment
                bool isDeleteDisabled = (this.GetUIItemAccessLevel("RES_LISTMAINT_VEHICLEMODELS_DELETE") != UIAccessLevel.Enabled
                                        || this.GetUIItemAccessLevel("RES_LISTMAINT_VEHICLEMODELS") != UIAccessLevel.Enabled)
                                        || this.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled;

                // Get the visible right for assessment
                bool isEditHidden = (this.GetUIItemAccessLevel("RES_LISTMAINT_VEHICLEMODELS_EDIT") != UIAccessLevel.Enabled) ||
                                      (this.GetUIItemAccessLevel("RES_LISTMAINT_VEHICLEMODELS") != UIAccessLevel.Enabled) ||
                                      (this.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled);

                //Iterate through all items and add them into the grid
                foreach (VehicleModel vehicleModel in vehicleModels)
                {
                    string cellStyle = "vertical-align: top;";

                    string editHTML = "";
                    if (!isEditHidden)
                    {
                        editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='ShowVehicleModelLightBox(" + vehicleModel.VehicleModelId.ToString() + ");' />";
                    }

                    string deleteHTML = "";
                    if (!isDeleteDisabled && vehicleModel.CanDelete)
                    {
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на този модел автомобил' class='GridActionIcon' onclick='DeleteVehicleModel(" + vehicleModel.VehicleModelId.ToString() + ");' />";
                    }

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='" + cellStyle + @"'>" + ((pageIdx - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + vehicleModel.VehicleMake.VehicleMakeName + @"</td>
                                 <td style='" + cellStyle + @"'>" + vehicleModel.VehicleModelName + @"</td>
                                 <td align='center' style='" + cellStyle + @"'>" + editHTML + deleteHTML + @"</td>
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

            //Set the message if there is a need (e.g. a deleted item)
            if (hdnRefreshReason.Value != "")
            {
                if (hdnRefreshReason.Value == "ADDED")
                {
                    lblGridMessage.Text = "Моделът автомобил беше добавен успешно";
                    lblGridMessage.CssClass = "SuccessText";
                }
                else if (hdnRefreshReason.Value == "SAVED")
                {
                    lblGridMessage.Text = "Моделът автомобил беше обновен успешно";
                    lblGridMessage.CssClass = "SuccessText";
                }
                else if (hdnRefreshReason.Value == "DELETED")
                {
                    lblGridMessage.Text = "Моделът автомобил беше изтрит успешно";
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
                RefreshVehicleModels();
            }
        }

        //Go to the first page and refresh the grid
        protected void btnFirst_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = "1";
                RefreshVehicleModels();
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

                    RefreshVehicleModels();
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

                    RefreshVehicleModels();
                }
            }
        }

        //Go to the last page and refresh the grid
        protected void btnLast_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = maxPage.ToString();
                RefreshVehicleModels();
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
                    RefreshVehicleModels();
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
        private VehicleModelFilter CollectFilterData()
        {
            VehicleModelFilter filter = new VehicleModelFilter();

            int orderBy;
            if (!int.TryParse(hdnSortBy.Value, out orderBy))
                orderBy = 0;

            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            int vehicleMakeId = 0;
            if (this.ddlVehicleMakes.SelectedValue != ListItems.GetOptionAll().Value)
            {
                int.TryParse(this.ddlVehicleMakes.SelectedValue, out vehicleMakeId);
            }

            filter.VehicleMakeId = (vehicleMakeId > 0 ? vehicleMakeId : (int?)null);
            filter.OrderBy = orderBy;
            filter.PageIdx = pageIdx;

            return filter;
        }

        //Load Vehicle Model details (ajax call)
        private void JSLoadVehicleModelDetails()
        {
            if (GetUIItemAccessLevel("RES_LISTMAINT_VEHICLEMODELS_EDIT") == UIAccessLevel.Hidden
                || GetUIItemAccessLevel("RES_LISTMAINT_VEHICLEMODELS") == UIAccessLevel.Hidden
                || GetUIItemAccessLevel("RES_LISTMAINT") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int vehicleModelId = 0;
                int.TryParse(Request.Form["VehicleModelID"], out vehicleModelId);

                VehicleModel vehicleModel = VehicleModelUtil.GetVehicleModel(vehicleModelId, CurrentUser);

                stat = AJAXTools.OK;

                //Existing Vehicle Model: Load its details
                if (vehicleModel != null)
                {
                    response = @"
                                <vehicleModelData>
                                    <vehicleModelId>" + AJAXTools.EncodeForXML(vehicleModel.VehicleModelId.ToString()) + @"</vehicleModelId>
                                    <vehicleModelName>" + AJAXTools.EncodeForXML(vehicleModel.VehicleModelName) + @"</vehicleModelName>
                                    <vehicleMakeName>" + AJAXTools.EncodeForXML(vehicleModel.VehicleMake.VehicleMakeName) + @"</vehicleMakeName>
                                </vehicleModelData>";
                }
                else //New vehicle model
                {
                    response = @"
                                <vehicleModelData>                                
                                    <vehicleModelId>0</vehicleModelId>
                                    <vehicleModelName></vehicleModelName>
                                    <vehicleMakeName></vehicleMakeName>
                                </vehicleModelData>";
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

        //Add new vehicle model (ajax call)
        private void JSSaveVehicleModel()
        {
            if (this.GetUIItemAccessLevel("RES_LISTMAINT_VEHICLEMODELS_ADD") != UIAccessLevel.Enabled
                || this.GetUIItemAccessLevel("RES_LISTMAINT_VEHICLEMODELS") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            int vehicleMakeId = 0;
            int.TryParse(Request.Form["VehicleMakeID"], out vehicleMakeId);

            if (vehicleMakeId == 0)
            {
                throw new Exception("Марката автомобил е невалидна");
            }

            int vehicleModelId = 0;
            int.TryParse(Request.Form["VehicleModelID"], out vehicleModelId);

            string vehicleModelName = Request.Form["VehicleModelName"].ToString();

            string stat = "";
            string response = "";

            try
            {
                //Track the changes into the Audit Trail 
                Change change = new Change(CurrentUser, "RES_Lists_VehicleModels");

                VehicleModel vehicleModel = null;

                if (vehicleModelId > 0)
                {
                    vehicleModel = VehicleModelUtil.GetVehicleModel(vehicleModelId, CurrentUser);
                    vehicleModel.VehicleModelName = vehicleModelName;
                }
                else
                {
                    vehicleModel = new VehicleModel(CurrentUser)
                    {
                        VehicleModelName = vehicleModelName,
                        VehicleMakeId = vehicleMakeId
                    };
                }

                VehicleModelUtil.SaveVehicleModel(vehicleModel, CurrentUser, change);

                change.WriteLog();

                stat = AJAXTools.OK;
                response = "<response>" + AJAXTools.OK + "</response>";
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

        //Delete vehicle model (ajax call)
        private void JSDeleteVehicleModel()
        {
            if (this.GetUIItemAccessLevel("RES_LISTMAINT_VEHICLEMODELS_DELETE") != UIAccessLevel.Enabled
                || this.GetUIItemAccessLevel("RES_LISTMAINT_VEHICLEMODELS") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            int vehiclemodelID = int.Parse(Request.Form["VehicleModelID"]);

            string stat = "";
            string response = "";

            try
            {
                //Track the changes into the Audit Trail 
                Change change = new Change(CurrentUser, "RES_Lists_VehicleModels");

                VehicleModelUtil.DeleteVehicleModel(vehiclemodelID, CurrentUser, change);

                change.WriteLog();

                stat = AJAXTools.OK;
                response = "<response>" + AJAXTools.OK + "</response>";
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
    }
}
