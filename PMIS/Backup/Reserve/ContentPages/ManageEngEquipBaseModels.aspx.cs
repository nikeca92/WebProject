using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class ManageEngEquipBaseModels : RESPage
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
                return "RES_LISTMAINT_ENGEQUIPBASEMODELS";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if ((GetUIItemAccessLevel("RES_LISTMAINT") == UIAccessLevel.Hidden) ||
                (GetUIItemAccessLevel("RES_LISTMAINT_ENGEQUIPBASEMODELS") == UIAccessLevel.Hidden))
            {
                RedirectAccessDenied();
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadEngEquipBaseModelDetails")
            {
                this.JSLoadEngEquipBaseModelDetails();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveEngEquipBaseModel")
            {
                this.JSSaveEngEquipBaseModel();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteEngEquipBaseModel")
            {
                this.JSDeleteEngEquipBaseModel();
                return;
            }

            //Hilight the current page in the menu bar
            HighlightMenuItems("Lists", "ManageEngEquipBaseModels");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            //Setup some basic styles on the screen
            SetupStyle();

            //Collect the filter information to be able to pull the number of rows for this specific filter
            EngEquipBaseModelFilter filter = CollectFilterData();

            int allRows = EngEquipBaseModelUtil.GetAllEngEquipBaseModelsByFilterCount(filter, CurrentUser);
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
            lblHeaderTitle.InnerHtml = "Модели инж. машини";
        }

        // Setup AddNewAssessment button according to rights of user's role
        private void SetBtnNew()
        {
            if (this.GetUIItemAccessLevel("RES_LISTMAINT_ENGEQUIPBASEODELS_ADD") == UIAccessLevel.Enabled
                && this.GetUIItemAccessLevel("RES_LISTMAINT_ENGEQUIPBASEMODELS") == UIAccessLevel.Enabled
                && this.GetUIItemAccessLevel("RES_LISTMAINT") == UIAccessLevel.Enabled)
            {
                EnableButton(imgBtnNewModel);
            }
            else
            {
                if (this.GetUIItemAccessLevel("RES_LISTMAINT_ENGEQUIPBASEODELS_ADD") != UIAccessLevel.Enabled
                    || this.GetUIItemAccessLevel("RES_LISTMAINT_ENGEQUIPBASEMODELS") != UIAccessLevel.Enabled
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
            this.ddlEngEquipBaseMakes.DataSource = EngEquipBaseMakeUtil.GetAllEngEquipBaseMakes(CurrentUser);
            this.ddlEngEquipBaseMakes.DataTextField = "EngEquipBaseMakeName";
            this.ddlEngEquipBaseMakes.DataValueField = "EngEquipBaseMakeId";
            this.ddlEngEquipBaseMakes.DataBind();
            this.ddlEngEquipBaseMakes.Items.Insert(0, ListItems.GetOptionAll());
        }
        
        //Setup some styling on the page
        private void SetupStyle()
        {

        }

        protected void ddlEngEquipBaseMakes_Change(object sender, EventArgs e)
        {
            if (this.ddlEngEquipBaseMakes.SelectedValue == ListItems.GetOptionAll().Value)
            {
                this.imgBtnNewModel.Visible = false;
            }
            else
            {
                this.imgBtnNewModel.Visible = true;
            }

            this.SetBtnNew();
            this.RefreshEngEquipBaseModels();
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
        private void RefreshEngEquipBaseModels()
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
            EngEquipBaseModelFilter filter = CollectFilterData();

            //Get the list of records according to the specified filters, order and paging
            List<EngEquipBaseModel> engEquipBaseModels = EngEquipBaseModelUtil.GetAllEngEquipBaseModelsByFilter(filter, CurrentUser);

            //No data found
            if (engEquipBaseModels.Count == 0)
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
                bool isDeleteDisabled = (this.GetUIItemAccessLevel("RES_LISTMAINT_ENGEQUIPBASEMODELS_DELETE") != UIAccessLevel.Enabled
                                        || this.GetUIItemAccessLevel("RES_LISTMAINT_ENGEQUIPBASEMODELS") != UIAccessLevel.Enabled)
                                        || this.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled;

                // Get the visible right for assessment
                bool isEditHidden = (this.GetUIItemAccessLevel("RES_LISTMAINT_ENGEQUIPBASEMODELS_EDIT") != UIAccessLevel.Enabled) ||
                                      (this.GetUIItemAccessLevel("RES_LISTMAINT_ENGEQUIPBASEMODELS") != UIAccessLevel.Enabled) ||
                                      (this.GetUIItemAccessLevel("RES_LISTMAINT") != UIAccessLevel.Enabled);

                //Iterate through all items and add them into the grid
                foreach (EngEquipBaseModel engEquipBaseModel in engEquipBaseModels)
                {
                    string cellStyle = "vertical-align: top;";

                    string editHTML = "";
                    if (!isEditHidden)
                    {
                        editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='ShowEngEquipBaseModelLightBox(" + engEquipBaseModel.EngEquipBaseModelId.ToString() + ");' />";
                    }

                    string deleteHTML = "";
                    if (!isDeleteDisabled && engEquipBaseModel.CanDelete)
                    {
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на този модел инж. машина' class='GridActionIcon' onclick='DeleteEngEquipBaseModel(" + engEquipBaseModel.EngEquipBaseModelId.ToString() + ");' />";
                    }

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='" + cellStyle + @"'>" + ((pageIdx - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + engEquipBaseModel.EngEquipBaseMake.EngEquipBaseMakeName + @"</td>
                                 <td style='" + cellStyle + @"'>" + engEquipBaseModel.EngEquipBaseModelName + @"</td>
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
                    lblGridMessage.Text = "Моделът инж. машина беше добавен успешно";
                    lblGridMessage.CssClass = "SuccessText";
                }
                else if (hdnRefreshReason.Value == "SAVED")
                {
                    lblGridMessage.Text = "Моделът инж. машина беше обновен успешно";
                    lblGridMessage.CssClass = "SuccessText";
                }
                else if (hdnRefreshReason.Value == "DELETED")
                {
                    lblGridMessage.Text = "Моделът инж. машина беше изтрит успешно";
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
                RefreshEngEquipBaseModels();
            }
        }

        //Go to the first page and refresh the grid
        protected void btnFirst_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = "1";
                RefreshEngEquipBaseModels();
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

                    RefreshEngEquipBaseModels();
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

                    RefreshEngEquipBaseModels();
                }
            }
        }

        //Go to the last page and refresh the grid
        protected void btnLast_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = maxPage.ToString();
                RefreshEngEquipBaseModels();
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
                    RefreshEngEquipBaseModels();
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
        private EngEquipBaseModelFilter CollectFilterData()
        {
            EngEquipBaseModelFilter filter = new EngEquipBaseModelFilter();

            int orderBy;
            if (!int.TryParse(hdnSortBy.Value, out orderBy))
                orderBy = 0;

            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            int engEquipBaseMakeId = 0;
            if (this.ddlEngEquipBaseMakes.SelectedValue != ListItems.GetOptionAll().Value)
            {
                int.TryParse(this.ddlEngEquipBaseMakes.SelectedValue, out engEquipBaseMakeId);
            }

            filter.EngEquipBaseMakeId = (engEquipBaseMakeId > 0 ? engEquipBaseMakeId : (int?)null);
            filter.OrderBy = orderBy;
            filter.PageIdx = pageIdx;

            return filter;
        }

        //Load EngEquipBase Model details (ajax call)
        private void JSLoadEngEquipBaseModelDetails()
        {
            if (GetUIItemAccessLevel("RES_LISTMAINT_ENGEQUIPBASEMODELS_EDIT") == UIAccessLevel.Hidden
                || GetUIItemAccessLevel("RES_LISTMAINT_ENGEQUIPBASEMODELS") == UIAccessLevel.Hidden
                || GetUIItemAccessLevel("RES_LISTMAINT") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int engEquipBaseModelId = 0;
                int.TryParse(Request.Form["EngEquipBaseModelID"], out engEquipBaseModelId);

                EngEquipBaseModel engEquipBaseModel = EngEquipBaseModelUtil.GetEngEquipBaseModel(engEquipBaseModelId, CurrentUser);

                stat = AJAXTools.OK;

                //Existing EngEquipBase Model: Load its details
                if (engEquipBaseModel != null)
                {
                    response = @"
                                <engEquipBaseModelData>
                                    <engEquipBaseModelId>" + AJAXTools.EncodeForXML(engEquipBaseModel.EngEquipBaseModelId.ToString()) + @"</engEquipBaseModelId>
                                    <engEquipBaseModelName>" + AJAXTools.EncodeForXML(engEquipBaseModel.EngEquipBaseModelName) + @"</engEquipBaseModelName>
                                    <engEquipBaseMakeName>" + AJAXTools.EncodeForXML(engEquipBaseModel.EngEquipBaseMake.EngEquipBaseMakeName) + @"</engEquipBaseMakeName>
                                </engEquipBaseModelData>";
                }
                else //New engEquipBase model
                {
                    response = @"
                                <engEquipBaseModelData>                                
                                    <engEquipBaseModelId>0</engEquipBaseModelId>
                                    <engEquipBaseModelName></engEquipBaseModelName>
                                    <engEquipBaseMakeName></engEquipBaseMakeName>
                                </engEquipBaseModelData>";
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

        //Add new engEquipBase model (ajax call)
        private void JSSaveEngEquipBaseModel()
        {
            if (this.GetUIItemAccessLevel("RES_LISTMAINT_ENGEQUIPBASEMODELS_ADD") != UIAccessLevel.Enabled
                || this.GetUIItemAccessLevel("RES_LISTMAINT_ENGEQUIPBASEMODELS") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            int engEquipBaseMakeId = 0;
            int.TryParse(Request.Form["EngEquipBaseMakeID"], out engEquipBaseMakeId);

            if (engEquipBaseMakeId == 0)
            {
                throw new Exception("Марката инж. машина е невалидна");
            }

            int engEquipBaseModelId = 0;
            int.TryParse(Request.Form["EngEquipBaseModelID"], out engEquipBaseModelId);

            string engEquipBaseModelName = Request.Form["EngEquipBaseModelName"].ToString();

            string stat = "";
            string response = "";

            try
            {
                //Track the changes into the Audit Trail 
                Change change = new Change(CurrentUser, "RES_Lists_EngEquipBaseModels");

                EngEquipBaseModel engEquipBaseModel = null;

                if (engEquipBaseModelId > 0)
                {
                    engEquipBaseModel = EngEquipBaseModelUtil.GetEngEquipBaseModel(engEquipBaseModelId, CurrentUser);
                    engEquipBaseModel.EngEquipBaseModelName = engEquipBaseModelName;
                }
                else
                {
                    engEquipBaseModel = new EngEquipBaseModel(CurrentUser)
                    {
                        EngEquipBaseModelName = engEquipBaseModelName,
                        EngEquipBaseMakeId = engEquipBaseMakeId
                    };
                }

                EngEquipBaseModelUtil.SaveEngEquipBaseModel(engEquipBaseModel, CurrentUser, change);

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

        //Delete engEquipBase model (ajax call)
        private void JSDeleteEngEquipBaseModel()
        {
            if (this.GetUIItemAccessLevel("RES_LISTMAINT_ENGEQUIPBASEMODELS_DELETE") != UIAccessLevel.Enabled
                || this.GetUIItemAccessLevel("RES_LISTMAINT_ENGEQUIPBASEMODELS") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            int engEquipBasemodelID = int.Parse(Request.Form["EngEquipBaseModelID"]);

            string stat = "";
            string response = "";

            try
            {
                //Track the changes into the Audit Trail 
                Change change = new Change(CurrentUser, "RES_Lists_EngEquipBaseModels");

                EngEquipBaseModelUtil.DeleteEngEquipBaseModel(engEquipBasemodelID, CurrentUser, change);

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
