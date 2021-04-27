using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class ManageTractorModels : RESPage
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
                return "RES_LISTMAINT_TRACTORMODELS";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if ((GetUIItemAccessLevel("RES_LISTMAINT") == UIAccessLevel.Hidden) ||
                (GetUIItemAccessLevel("RES_LISTMAINT_TRACTORMODELS") == UIAccessLevel.Hidden))
            {
                RedirectAccessDenied();
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadTractorModelDetails")
            {
                this.JSLoadTractorModelDetails();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveTractorModel")
            {
                this.JSSaveTractorModel();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteTractorModel")
            {
                this.JSDeleteTractorModel();
                return;
            }

            //Hilight the current page in the menu bar
            HighlightMenuItems("Lists", "ManageTractorModels");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            //Setup some basic styles on the screen
            SetupStyle();

            //Collect the filter information to be able to pull the number of rows for this specific filter
            TractorModelFilter filter = CollectFilterData();

            int allRows = TractorModelUtil.GetAllTractorModelsByFilterCount(filter, CurrentUser);
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

                //The default order is by module name
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
            lblHeaderTitle.InnerHtml = "Модели трактори";
        }

        // Setup AddNewAssessment button according to rights of user's role
        private void SetBtnNew()
        {
            if (this.GetUIItemAccessLevel("RES_LISTMAINT_TRACTORODELS_ADD") == UIAccessLevel.Enabled
                && this.GetUIItemAccessLevel("RES_LISTMAINT_TRACTORMODELS") == UIAccessLevel.Enabled
                && this.GetUIItemAccessLevel("RES_LISTMAINT") == UIAccessLevel.Enabled)
            {
                EnableButton(imgBtnNewModel);
            }
            else
            {
                if (this.GetUIItemAccessLevel("RES_LISTMAINT_TRACTORODELS_ADD") != UIAccessLevel.Enabled
                    || this.GetUIItemAccessLevel("RES_LISTMAINT_TRACTORMODELS") != UIAccessLevel.Enabled
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
            this.ddlTractorMakes.DataSource = TractorMakeUtil.GetAllTractorMakes(CurrentUser);
            this.ddlTractorMakes.DataTextField = "TractorMakeName";
            this.ddlTractorMakes.DataValueField = "TractorMakeId";
            this.ddlTractorMakes.DataBind();
            this.ddlTractorMakes.Items.Insert(0, ListItems.GetOptionAll());
        }
        
        //Setup some styling on the page
        private void SetupStyle()
        {

        }

        protected void ddlTractorMakes_Change(object sender, EventArgs e)
        {
            if (this.ddlTractorMakes.SelectedValue == ListItems.GetOptionAll().Value)
            {
                this.imgBtnNewModel.Visible = false;
            }
            else
            {
                this.imgBtnNewModel.Visible = true;
            }

            this.SetBtnNew();
            this.RefreshTractorModels();
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
        private void RefreshTractorModels()
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
            TractorModelFilter filter = CollectFilterData();

            //Get the list of records according to the specified filters, order and paging
            List<TractorModel> tractorModels = TractorModelUtil.GetAllTractorModelsByFilter(filter, CurrentUser);

            //No data found
            if (tractorModels.Count == 0)
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
                bool isDeleteDisabled = (this.GetUIItemAccessLevel("RES_LISTMAINT_TRACTORMODELS_DELETE") != UIAccessLevel.Enabled
                                        || this.GetUIItemAccessLevel("RES_LISTMAINT_TRACTORMODELS") != UIAccessLevel.Enabled)
                                        || this.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled;

                // Get the visible right for assessment
                bool isEditHidden = (this.GetUIItemAccessLevel("RES_LISTMAINT_TRACTORMODELS_EDIT") != UIAccessLevel.Enabled) ||
                                      (this.GetUIItemAccessLevel("RES_LISTMAINT_TRACTORMODELS") != UIAccessLevel.Enabled) ||
                                      (this.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled);

                //Iterate through all items and add them into the grid
                foreach (TractorModel tractorModel in tractorModels)
                {
                    string cellStyle = "vertical-align: top;";

                    string editHTML = "";
                    if (!isEditHidden)
                    {
                        editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='ShowTractorModelLightBox(" + tractorModel.TractorModelId.ToString() + ");' />";
                    }

                    string deleteHTML = "";
                    if (!isDeleteDisabled && tractorModel.CanDelete)
                    {
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на този модел трактор' class='GridActionIcon' onclick='DeleteTractorModel(" + tractorModel.TractorModelId.ToString() + ");' />";
                    }

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='" + cellStyle + @"'>" + ((pageIdx - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + tractorModel.TractorMake.TractorMakeName + @"</td>
                                 <td style='" + cellStyle + @"'>" + tractorModel.TractorModelName + @"</td>
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
                    lblGridMessage.Text = "Моделът трактор беше добавен успешно";
                    lblGridMessage.CssClass = "SuccessText";
                }
                else if (hdnRefreshReason.Value == "SAVED")
                {
                    lblGridMessage.Text = "Моделът трактор беше обновен успешно";
                    lblGridMessage.CssClass = "SuccessText";
                }
                else if (hdnRefreshReason.Value == "DELETED")
                {
                    lblGridMessage.Text = "Моделът трактор беше изтрит успешно";
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
                RefreshTractorModels();
            }
        }

        //Go to the first page and refresh the grid
        protected void btnFirst_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = "1";
                RefreshTractorModels();
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

                    RefreshTractorModels();
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

                    RefreshTractorModels();
                }
            }
        }

        //Go to the last page and refresh the grid
        protected void btnLast_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = maxPage.ToString();
                RefreshTractorModels();
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
                    RefreshTractorModels();
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
        private TractorModelFilter CollectFilterData()
        {
            TractorModelFilter filter = new TractorModelFilter();

            int orderBy;
            if (!int.TryParse(hdnSortBy.Value, out orderBy))
                orderBy = 0;

            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            int tractorMakeId = 0;
            if (this.ddlTractorMakes.SelectedValue != ListItems.GetOptionAll().Value)
            {
                int.TryParse(this.ddlTractorMakes.SelectedValue, out tractorMakeId);
            }

            filter.TractorMakeId = (tractorMakeId > 0 ? tractorMakeId : (int?)null);
            filter.OrderBy = orderBy;
            filter.PageIdx = pageIdx;

            return filter;
        }

        //Load Tractor Model details (ajax call)
        private void JSLoadTractorModelDetails()
        {
            if (GetUIItemAccessLevel("RES_LISTMAINT_TRACTORMODELS_EDIT") == UIAccessLevel.Hidden
                || GetUIItemAccessLevel("RES_LISTMAINT_TRACTORMODELS") == UIAccessLevel.Hidden
                || GetUIItemAccessLevel("RES_LISTMAINT") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int tractorModelId = 0;
                int.TryParse(Request.Form["TractorModelID"], out tractorModelId);

                TractorModel tractorModel = TractorModelUtil.GetTractorModel(tractorModelId, CurrentUser);

                stat = AJAXTools.OK;

                //Existing Tractor Model: Load its details
                if (tractorModel != null)
                {
                    response = @"
                                <tractorModelData>
                                    <tractorModelId>" + AJAXTools.EncodeForXML(tractorModel.TractorModelId.ToString()) + @"</tractorModelId>
                                    <tractorModelName>" + AJAXTools.EncodeForXML(tractorModel.TractorModelName) + @"</tractorModelName>
                                    <tractorMakeName>" + AJAXTools.EncodeForXML(tractorModel.TractorMake.TractorMakeName) + @"</tractorMakeName>
                                </tractorModelData>";
                }
                else //New tractor model
                {
                    response = @"
                                <tractorModelData>                                
                                    <tractorModelId>0</tractorModelId>
                                    <tractorModelName></tractorModelName>
                                    <tractorMakeName></tractorMakeName>
                                </tractorModelData>";
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

        //Add new tractor model (ajax call)
        private void JSSaveTractorModel()
        {
            if (this.GetUIItemAccessLevel("RES_LISTMAINT_TRACTORMODELS_ADD") != UIAccessLevel.Enabled
                || this.GetUIItemAccessLevel("RES_LISTMAINT_TRACTORMODELS") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            int tractorMakeId = 0;
            int.TryParse(Request.Form["TractorMakeID"], out tractorMakeId);

            if (tractorMakeId == 0)
            {
                throw new Exception("Марката трактор е невалидна");
            }

            int tractorModelId = 0;
            int.TryParse(Request.Form["TractorModelID"], out tractorModelId);

            string tractorModelName = Request.Form["TractorModelName"].ToString();

            string stat = "";
            string response = "";

            try
            {
                //Track the changes into the Audit Trail 
                Change change = new Change(CurrentUser, "RES_Lists_TractorModels");

                TractorModel tractorModel = null;

                if (tractorModelId > 0)
                {
                    tractorModel = TractorModelUtil.GetTractorModel(tractorModelId, CurrentUser);
                    tractorModel.TractorModelName = tractorModelName;
                }
                else
                {
                    tractorModel = new TractorModel(CurrentUser)
                    {
                        TractorModelName = tractorModelName,
                        TractorMakeId = tractorMakeId
                    };
                }

                TractorModelUtil.SaveTractorModel(tractorModel, CurrentUser, change);

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

        //Delete tractor model (ajax call)
        private void JSDeleteTractorModel()
        {
            if (this.GetUIItemAccessLevel("RES_LISTMAINT_TRACTORMODELS_DELETE") != UIAccessLevel.Enabled
                || this.GetUIItemAccessLevel("RES_LISTMAINT_TRACTORMODELS") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            int tractormodelID = int.Parse(Request.Form["TractorModelID"]);

            string stat = "";
            string response = "";

            try
            {
                //Track the changes into the Audit Trail 
                Change change = new Change(CurrentUser, "RES_Lists_TractorModels");

                TractorModelUtil.DeleteTractorModel(tractormodelID, CurrentUser, change);

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
