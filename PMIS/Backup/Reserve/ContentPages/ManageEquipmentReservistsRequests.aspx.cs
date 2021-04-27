using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class ManageEquipmentReservistsRequests : RESPage
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
                return "RES_EQUIPRESREQUESTS";
            }
        }

        private DateTime? postBackStart = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSWriteBenchmarkLog")
            {
                JSWriteBenchmarkLog();
                return;
            }

            DateTime? pageStart = null;
            if (!IsPostBack)
                pageStart = BenchmarkLog.WriteStart("Отваряне на екран 'Въведени заявки за окомплектоване с ресурс от резерва'", CurrentUser, Request);

            if (IsPostBack)
                postBackStart = BenchmarkLog.WriteStart("PostBack в екран 'Въведени заявки за окомплектоване с ресурс от резерва'", CurrentUser, Request);

            //Redirect if the access is denied
            if (GetUIItemAccessLevel("RES_EQUIPRESREQUESTS") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            SetBtnNew();

            //Setup any calendar control on the screen
            SetupDatePickers();

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteEquipmentReservistsRequest")
            {
                JSDeleteEquipmentReservistsRequest();
                return;
            }

            //Hilight the current page in the menu bar
            HighlightMenuItems("Equipment", "ManageEquipmentReservistsRequests");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            //Setup some basic styles on the screen
            SetupStyle();

            //Collect the filter information to be able to pull the number of rows for this specific filter
            EquipmentReservistsRequestsFilter filter = CollectFilterData();

            DateTime startMaxPage = BenchmarkLog.WriteStart("\tНачало на изчисляване на общия брой страници", CurrentUser, Request);
            int allRows = EquipmentReservistsRequestUtil.GetAllEquipmentReservistsRequestCount(filter, CurrentUser);
            //Get the number of rows and calculate the number of pages in the grid
            maxPage = pageLength == 0 ? 1 : allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);
            BenchmarkLog.WriteEnd("\tКрай на изчисляване на общия брой страници", CurrentUser, Request, startMaxPage);

            //When the page is loaded for the first time
            if (!IsPostBack)
            {
                //Populate any drop-downs and list-boxes
                DateTime listsStart = BenchmarkLog.WriteStart("\tНачало на зареждане на класификаторите", CurrentUser, Request);
                PopulateLists();
                BenchmarkLog.WriteEnd("\tКрай на зареждане на класификаторите", CurrentUser, Request, listsStart);

                //The default order is by module name
                if (string.IsNullOrEmpty(hdnSortBy.Value))
                    hdnSortBy.Value = "1";

                //Simulate clicking the Refresh button to load the grid initially
                //btnRefresh_Click(btnRefresh, new EventArgs());
                this.divNavigation.Visible = false;
                this.pnlSearchHint.Visible = true;
                this.btnPrintAllEquipReservistsRequests.Visible = false;
            }

            lblMessage.Text = "";
            lblGridMessage.Text = "";

            if (pageStart.HasValue)
                BenchmarkLog.WriteEnd("Край на зареждане на екран 'Въведени заявки за окомплектоване с ресурс от резерва'", CurrentUser, Request, pageStart.Value);
        }

        //Populate all listboxes on the screen
        private void PopulateLists()
        {
            DateTime start = BenchmarkLog.WriteStart("\t\tНачало класификатор 'От кое министерство/ведомство'", CurrentUser, Request);
            PopulateAdministrations();
            BenchmarkLog.WriteEnd("\t\tКрай класификатор 'От кое министерство/ведомство'", CurrentUser, Request, start);

            start = BenchmarkLog.WriteStart("\t\tНачало класификатор 'Статус на заявката'", CurrentUser, Request);
            PopulateEquipWithResRequestsStatuses();
            BenchmarkLog.WriteEnd("\t\tКрай класификатор 'Статус на заявката'", CurrentUser, Request, start);

            start = BenchmarkLog.WriteStart("\t\tНачало класификатор 'Заявката се изпълнява от ВО'", CurrentUser, Request);
            PopulateMilitaryDepartment();
            BenchmarkLog.WriteEnd("\t\tКрай класификатор 'Заявката се изпълнява от ВО'", CurrentUser, Request, start);
        }
        

        //Populate the Administrations drop-down
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

        //Populate the EquipWithResRequestsStatuses drop-down
        private void PopulateEquipWithResRequestsStatuses()
        {
            ddEquipWithResRequestsStatus.Items.Clear();
            ddEquipWithResRequestsStatus.Items.Add(ListItems.GetOptionAll());

            List<EquipWithResRequestsStatus> statuses = EquipWithResRequestsStatusUtil.GetAllEquipWithResRequestsStatuses(CurrentUser);

            foreach (EquipWithResRequestsStatus status in statuses)
            {
                ListItem li = new ListItem();
                li.Text = status.StatusName;
                li.Value = status.EquipWithResRequestsStatusId.ToString();

                ddEquipWithResRequestsStatus.Items.Add(li);
            }
        }

        //Populate the MilitaryUnits drop-down
        private void PopulateMilitaryDepartment()
        {
            ddMilitaryDepartment.Items.Clear();
            ddMilitaryDepartment.Items.Add(ListItems.GetOptionAll());

            List<MilitaryDepartment> militaryDepartments = MilitaryDepartmentUtil.GetAllMilitaryDepartmentsByEquipmentResRequestsPerUser(CurrentUser, CurrentUser);

            foreach (MilitaryDepartment militaryDepartment in militaryDepartments)
            {
                ListItem li = new ListItem();
                li.Text = militaryDepartment.MilitaryDepartmentName;
                li.Value = militaryDepartment.MilitaryDepartmentId.ToString();

                ddMilitaryDepartment.Items.Add(li);
            }
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
        private void RefreshEquipmentReservistsRequests()
        {
            DateTime start = BenchmarkLog.WriteStart("\tНачало на зареждане на записите за избраната страница", CurrentUser, Request);

            string html = "";
            this.pnlSearchHint.Visible = false;

            //Collect the filters, the order control and the paging control data from the page
            int orderBy;
            if (!int.TryParse(hdnSortBy.Value, out orderBy))
                orderBy = 0;

            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            //Collect the filter information to be able to pull the number of rows for this specific filter
            EquipmentReservistsRequestsFilter filter = CollectFilterData();

            //Get the list of records according to the specified filters, order and paging
            DateTime startSQL = BenchmarkLog.WriteStart("\t\tНачало извличане на данните от базата данни", CurrentUser, Request);
            List<EquipmentReservistsRequest> equipmentReservistsRequests = EquipmentReservistsRequestUtil.GetAllEquipmentReservistsRequest(filter, pageLength, CurrentUser);
            BenchmarkLog.WriteEnd("\t\tКрай извличане на данните от базата данни", CurrentUser, Request, startSQL);

            //No data found
            if (equipmentReservistsRequests.Count == 0)
            {
                html = "<span>Няма намерени резултати</span>";
                this.btnPrintAllEquipReservistsRequests.Visible = false;
                this.divNavigation.Visible = false;
            }
            //If there is data then generate dynamically the HTML for the data grid
            else 
            {
                this.btnPrintAllEquipReservistsRequests.Visible = true;
                this.divNavigation.Visible = true;

                string headerStyle = "vertical-align: bottom;";
                int orderCol = (orderBy > 100 ? orderBy - 100 : orderBy);
                string img = orderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
                string[] arrOrderCol = {"", "", "", "", ""};
                arrOrderCol[orderCol - 1] = @"<div style='Position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

                //Setup the header of the grid
                html = @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='width: 110px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Заявка №" + arrOrderCol[0] + @"</th>
                               <th style='width: 110px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>От дата" + arrOrderCol[1] + @"</th>
                               <th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>" + CommonFunctions.GetLabelText("MilitaryUnit") + arrOrderCol[2] + @"</th>
                               <th style='width: 250px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(4);'>Министерство/Ведомство" + arrOrderCol[3] + @"</th>
                               <th style='width: 200px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(5);'>Статус на заявката" + arrOrderCol[4] + @"</th>
                               <th style='width: 60px;" + headerStyle + @"'>&nbsp;</th>
                            </tr>
                         </thead>";

                int counter = 1;

                //Iterate through all items and add them into the grid
                foreach (EquipmentReservistsRequest equipmentReservistsRequest in equipmentReservistsRequests)
                {
                    string cellStyle = "vertical-align: top;";

                    string deleteHTML = "";

                    if (equipmentReservistsRequest.CanDelete)
                    {
                        if (GetUIItemAccessLevel("RES_EQUIPRESREQUESTS") == UIAccessLevel.Enabled &&
                            GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_DELETE") == UIAccessLevel.Enabled
                            )
                            deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на тази заявка' class='GridActionIcon' onclick='DeleteEquipmentReservistsRequest(" + equipmentReservistsRequest.EquipmentReservistsRequestId.ToString() + ");' />";
                    }

                    string milDeptHTML = "";

                    if (GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_MILDEPT") != UIAccessLevel.Hidden)
                        milDeptHTML = "<img src='../Images/list_edit.png' alt='Определяне на ВО' title='Определяне на ВО' class='GridActionIcon' onclick='EquipmentReservistsRequestMilDept(" + equipmentReservistsRequest.EquipmentReservistsRequestId.ToString() + ");' />";

                    string editHTML = "";

                    if (GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_EDIT") != UIAccessLevel.Hidden)
                        editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='EditEquipmentReservistsRequest(" + equipmentReservistsRequest.EquipmentReservistsRequestId.ToString() + ");' />";

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='" + cellStyle + @"'>" + ((pageIdx - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + equipmentReservistsRequest.RequestNumber + @"</td>
                                 <td style='" + cellStyle + @"'>" + CommonFunctions.FormatDate(equipmentReservistsRequest.RequestDate) + @"</td>
                                 <td style='" + cellStyle + @"'>" + (equipmentReservistsRequest.MilitaryUnit == null ? "" : equipmentReservistsRequest.MilitaryUnit.DisplayTextForSelection) + @"</td>
                                 <td style='" + cellStyle + @"'>" + (equipmentReservistsRequest.Administration == null ? "" : equipmentReservistsRequest.Administration.AdministrationName) + @"</td>
                                 <td style='" + cellStyle + @"'>" + (equipmentReservistsRequest.EquipWithResRequestsStatus == null ? "" : equipmentReservistsRequest.EquipWithResRequestsStatus.StatusName) + @"</td>
                                 <td style='" + cellStyle + @"'>" + editHTML + milDeptHTML + deleteHTML + @"</td>
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
                if (hdnRefreshReason.Value == "DELETED")
                {
                    lblGridMessage.Text = "Заявката беше изтрита успешно";
                    lblGridMessage.CssClass = "SuccessText";
                }

                hdnRefreshReason.Value = "";
            }

            BenchmarkLog.WriteEnd("\tКрай на зареждане на записите за избраната страница", CurrentUser, Request, start);
        }

        //Refresh the data grid
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = "1";
                RefreshEquipmentReservistsRequests();
            }

            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Въведени заявки за окомплектоване с ресурс от резерва'", CurrentUser, Request, postBackStart.Value);
        }

        //Go to the first page and refresh the grid
        protected void btnFirst_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = "1";
                RefreshEquipmentReservistsRequests();
            }

            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Въведени заявки за окомплектоване с ресурс от резерва'", CurrentUser, Request, postBackStart.Value);
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

                    RefreshEquipmentReservistsRequests();
                }
            }

            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Въведени заявки за окомплектоване с ресурс от резерва'", CurrentUser, Request, postBackStart.Value);
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

                    RefreshEquipmentReservistsRequests();
                }
            }

            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Въведени заявки за окомплектоване с ресурс от резерва'", CurrentUser, Request, postBackStart.Value);
        }

        //Go to the last page and refresh the grid
        protected void btnLast_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = maxPage.ToString();
                RefreshEquipmentReservistsRequests();
            }

            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Въведени заявки за окомплектоване с ресурс от резерва'", CurrentUser, Request, postBackStart.Value);
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
                    RefreshEquipmentReservistsRequests();
                }
            }

            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Въведени заявки за окомплектоване с ресурс от резерва'", CurrentUser, Request, postBackStart.Value);
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
            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Въведени заявки за окомплектоване с ресурс от резерва'", CurrentUser, Request, postBackStart.Value);

            Response.Redirect("~/ContentPages/Home.aspx");
        }

        //Go to create a new record
        protected void btnNew_Click(object sender, EventArgs e)
        {
            if (postBackStart.HasValue)
                BenchmarkLog.WriteEnd("Край на PostBack в екран 'Въведени заявки за окомплектоване с ресурс от резерва'", CurrentUser, Request, postBackStart.Value);

            Response.Redirect("~/ContentPages/AddEditEquipmentReservistsRequest.aspx");
        }

        //Delete a record (ajax call)
        private void JSDeleteEquipmentReservistsRequest()
        {
            if (GetUIItemAccessLevel("RES_EQUIPRESREQUESTS") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_DELETE") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            int equipmentReservistsRequestId = int.Parse(Request.Form["EquipmentReservistsRequestId"]);

            string stat = "";
            string response = "";
            
            try
            {
                Change change = new Change(CurrentUser, "RES_EquipmentReservistsRequests");


                //Get all reservists added to that Equipment Request
                List<FillReservistsRequest> fillReservistRequests = FillReservistsRequestUtil.GetAllFillReservistsRequestByEquipmentRequest(equipmentReservistsRequestId, CurrentUser);

                /*
                List<int> deletedReservists = new List<int>();

                //Remove all Reservists from that Equipment Request
                
                foreach (FillReservistsRequest fillReservistRequest in fillReservistRequests)
                {
                    FillReservistsRequestUtil.DeleteRequestCommandReservist(fillReservistRequest.FillReservistsRequestID, fillReservistRequest.MilitaryDepartmentID, CurrentUser, change);

                    //Just in case: clear the status and the appointment for each reservist only once (note each reservist should be added only once to a particular equipment request)
                    if (!deletedReservists.Contains(fillReservistRequest.ReservistID))
                    {
                        //Change the current Military Reporting Status of each reservist
                        ReservistMilRepStatusUtil.SetMilRepStatusTo_FREE(fillReservistRequest.ReservistID, CurrentUser, change);

                        //Clear the current Mobilization Appointment for each Reservist
                        ReservistAppointmentUtil.ClearTheCurrentReservistAppointmentByReservist(fillReservistRequest.ReservistID, CurrentUser, change);

                        deletedReservists.Add(fillReservistRequest.ReservistID);
                    }
                }*/

                string msg = "";
                if (fillReservistRequests.Count == 0)
                {
                    EquipmentReservistsRequestUtil.DeleteEquipmentReservistsRequest(equipmentReservistsRequestId, CurrentUser, change);
                    change.WriteLog();
                }
                else
                {
                    msg = "Заявката не може да бъде изтрита, поради наличието на резервисти назначени от следните ВО:<br/>";
                    msg += "<div style='max-height: 400px;overflow-y:auto;margin-top:10px;'><ul style='margin-top:-5px; padding-top:3px;'>";

                    foreach (var md in fillReservistRequests.GroupBy(x => x.MilitaryDepartment.MilitaryDepartmentName))
                    {
                        msg += "<li>" + md.Key + "</li>";
                    }

                    msg += "</ul></div>";
                }                

                stat = AJAXTools.OK;
                response = "<xml><response>OK</response><msg>" + AJAXTools.EncodeForXML(msg) + "</msg></xml>";
            }
            catch(Exception ex)
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
            if (this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_ADD") == UIAccessLevel.Enabled
                && this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS") == UIAccessLevel.Enabled)
            {
                EnableButton(btnNew);
            }
            else
            {
                if (this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_ADD") == UIAccessLevel.Hidden)
                {
                    HideControl(btnNew);
                }
                else
                {
                    DisableButton(btnNew);
                }
            }
        }

        //Setup any date picker controls on the page by setting the CSS of the target inputs
        //Note that the date picker CSS is common
        //This makes the calendar controls to appear on the page next to each input
        private void SetupDatePickers()
        {
            txtRequestDateFrom.CssClass = CommonFunctions.DatePickerCSS();
            txtRequestDateTo.CssClass = CommonFunctions.DatePickerCSS();
        }

        //Collect the filet information from the page
        private EquipmentReservistsRequestsFilter CollectFilterData()
        {
            EquipmentReservistsRequestsFilter filter = new EquipmentReservistsRequestsFilter();

            this.hdnRequestNumber.Value = this.txtRequestNumber.Text;

            DateTime? requestDateFrom = null;

            if (CommonFunctions.TryParseDate(txtRequestDateFrom.Text))
            {
                requestDateFrom = CommonFunctions.ParseDate(txtRequestDateFrom.Text);
                this.hdnRequestDateFrom.Value = this.txtRequestDateFrom.Text;
            }
            else
            {
                this.hdnRequestDateFrom.Value = "";
            }

            DateTime? requestDateTo = null;

            if (CommonFunctions.TryParseDate(txtRequestDateTo.Text))
            {
                requestDateTo = CommonFunctions.ParseDate(txtRequestDateTo.Text);
                this.hdnRequestDateTo.Value = this.txtRequestDateTo.Text;
            }
            else
            {
                this.hdnRequestDateTo.Value = "";
            }

            this.hdnCommandNum.Value = this.txtCommandNum.Text;

            string militaryUnits = "";

            if (msMilitaryUnit.SelectedValue != ListItems.GetOptionAll().Value)
            {
                militaryUnits = msMilitaryUnit.SelectedValue;
                this.hdnMilitaryUnitId.Value = msMilitaryUnit.SelectedValue;
            }
            else
            {
                this.hdnMilitaryUnitId.Value = "";
            }

            string administrations = "";

            if (ddAdministration.SelectedValue != ListItems.GetOptionAll().Value)
            {
                administrations = ddAdministration.SelectedValue;
                this.hdnAdministrationId.Value = this.ddAdministration.SelectedValue;
            }
            else
            {
                this.hdnAdministrationId.Value = "";
            }

            string equipWithResRequestsStatuses = "";

            if (ddEquipWithResRequestsStatus.SelectedValue != ListItems.GetOptionAll().Value)
            {
                equipWithResRequestsStatuses = ddEquipWithResRequestsStatus.SelectedValue;
                this.hdnEquipWithResRequestsStatusId.Value = this.ddEquipWithResRequestsStatus.SelectedValue;
            }
            else
            {
                this.hdnEquipWithResRequestsStatusId.Value = "";
            }

            string militaryDepartments = "";

            if (ddMilitaryDepartment.SelectedValue != ListItems.GetOptionAll().Value)
            {
                militaryDepartments = ddMilitaryDepartment.SelectedValue;
                this.hdnMilitaryDepartmentId.Value = this.ddMilitaryDepartment.SelectedValue;
            }
            else
            {
                this.hdnMilitaryDepartmentId.Value = "";
            }

            int orderBy;
            if (!int.TryParse(hdnSortBy.Value, out orderBy))
                orderBy = 0;

            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            filter.RequestNum = txtRequestNumber.Text;
            filter.RequestDateFrom = requestDateFrom;
            filter.RequestDateTo = requestDateTo;
            filter.CommandNum = txtCommandNum.Text;
            filter.MilitaryUnits = militaryUnits;
            filter.Administrations = administrations;
            filter.EquipWithResRequestsStatuses = equipWithResRequestsStatuses;
            filter.MilitaryDepartments = militaryDepartments;
            filter.OrderBy = orderBy;
            filter.PageIdx = pageIdx;

            return filter;
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtRequestNumber.Text = "";
            txtRequestDateFrom.Text = "";
            txtRequestDateTo.Text = "";
            txtCommandNum.Text = "";
            msMilitaryUnit.SelectedText = "";
            msMilitaryUnit.SelectedValue = ListItems.GetOptionAll().Value;
            ddAdministration.SelectedValue = ListItems.GetOptionAll().Value;
            ddEquipWithResRequestsStatus.SelectedValue = ListItems.GetOptionAll().Value;
            ddMilitaryDepartment.SelectedValue = ListItems.GetOptionAll().Value;
            //btnRefresh_Click(sender, e);
        }

        private void JSWriteBenchmarkLog()
        {
            string stat = "";
            string response = "";

            try
            {
                string msg = Request.Form["Message"];

                BenchmarkLog.Write(msg, CurrentUser, Request);

                response = @"<response>OK</response>";

                stat = AJAXTools.OK;
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
