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
using PMIS.PMISAdmin.Common;

namespace PMIS.PMISAdmin.ContentPages
{
    public partial class AuditTrail : AdmPage
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
                return "ADM_AUDITTRAIL";
            }
        }
        //This get label name from resourse file
        private string MilitaryUnitLabel = CommonFunctions.GetLabelText("MilitaryUnit");
        
        private int? LoginLogId
        {
            get
            {
                int loginLogId = 0;
                if (String.IsNullOrEmpty(this.hfLoginLogID.Value)
                    || this.hfLoginLogID.Value == "0")
                {
                    if (Request.Params["LoginLogId"] != null)
                        int.TryParse(Request.Params["LoginLogId"].ToString(), out loginLogId);

                    this.hfLoginLogID.Value = loginLogId.ToString();
                }
                else
                {
                    Int32.TryParse(this.hfLoginLogID.Value, out loginLogId);
                }

                return loginLogId == 0 ? null : (int?)loginLogId;
            }

            set
            {
                this.hfLoginLogID.Value = value.ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Set Label Text
            SetLabelValueText();

            SetHeaderTitle();

            if (GetUIItemAccessLevel("ADM_AUDITTRAIL") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            //Highlight the current page in the menu bar
            HighlightMenuItems("Audit", "Audit_AuditTrail");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            //Setup some basic styles on the screen
            SetupStyle();
            //Setup any calendar control on the screen
            SetupDatePickers();

            //When the page is loaded for the first time
            if (!IsPostBack)
            {
                //Populate any drop-downs and list-boxes
                PopulateLists();

                if (!LoginLogId.HasValue)
                {
                    //Pre-fill the date fields with the today's date
                    txtDateFrom.Text = CommonFunctions.FormatDate(DateTime.Now);
                    txtDateTo.Text = CommonFunctions.FormatDate(DateTime.Now);
                }

                //The default order is by date&time in descending order
                if (string.IsNullOrEmpty(hdnSortBy.Value))
                    hdnSortBy.Value = "102";

                //Collect the filter information to be able to pull the number of rows for this specific filter
                AuditTrailHistoryFilter filter = CollectFilterData();
                //Get the number of rows and calculate the number of pages in the grid
                int allRows = AuditTrailHistoryUtil.GetAuditTrailHistoryRecCnt(CurrentUser, filter);
                maxPage = pageLength == 0 ? 1 : allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

                //Simulate clicking the Refresh button to load the grid initially
                btnRefresh_Click(btnRefresh, new EventArgs());
            }
            else
            {
                //Collect the filter information to be able to pull the number of rows for this specific filter
                AuditTrailHistoryFilter filter = CollectFilterData();
                //Get the number of rows and calculate the number of pages in the grid
                int allRows = AuditTrailHistoryUtil.GetAuditTrailHistoryRecCnt(CurrentUser, filter);
                maxPage = pageLength == 0 ? 1 : allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);
            }

            lblMessage.Text = "";
        }        

        //Populate all listboxes on the screen
        private void PopulateLists()
        {
            PopulateUsers();
            PopulateModules();
            PopulateChangeTypes();
            PopulateChangeEventTypes();
        }

        //Populate the users listbox
        private void PopulateUsers()
        {
            lstUsers.Items.Clear();
            if (!LoginLogId.HasValue)
            {
                lstUsers.Items.Add(ListItems.GetOptionAll());

                List<User> users = UserUtil.GetUsers(CurrentUser, "", "", null, 1, 0, 0);

                foreach (User user in users)
                {
                    ListItem li = new ListItem();
                    li.Text = user.FullName;
                    li.Value = user.UserId.ToString();
                    li.Attributes.Add("title", li.Text);

                    lstUsers.Items.Add(li);
                }
            }
            else
            {
                PMIS.Common.LoginLog loginLog = LoginLogUtil.GetLoginLog(LoginLogId.Value, CurrentUser);

                ListItem li = new ListItem();
                li.Text = loginLog.User.FullName;
                li.Value = loginLog.UserID.ToString();
                li.Attributes.Add("title", li.Text);

                lstUsers.Items.Add(li);

                DisableControl(lblUsers);
                DisableControl(lstUsers);
            }
        }

        //Populate the modules listbox
        private void PopulateModules()
        {
            lstModules.Items.Clear();
            lstModules.Items.Add(ListItems.GetOptionAll());

            List<Module> modules = ModuleUtil.GetModules(CurrentUser);

            foreach (Module module in modules)
            {
                ListItem li = new ListItem();
                li.Text = module.ModuleName;
                li.Value = module.ModuleId.ToString();
                li.Attributes.Add("title", li.Text);

                lstModules.Items.Add(li);
            }
        }

        //Populate the change types listbox
        private void PopulateChangeTypes()
        {
            lstChangeTypes.Items.Clear();
            lstChangeTypes.Items.Add(ListItems.GetOptionAll());

            List<ChangeType> changeTypes = ChangeTypeUtil.GetChangeTypes(CurrentUser);

            foreach (ChangeType changeType in changeTypes)
            {
                ListItem li = new ListItem();
                li.Text = changeType.ChangeTypeName;
                li.Value = changeType.ChangeTypeId.ToString();
                li.Attributes.Add("title", li.Text);

                lstChangeTypes.Items.Add(li);
            }
        }

        //Populate the change event types listbox
        private void PopulateChangeEventTypes()
        {
            lstChangeEventTypes.Items.Clear();
            lstChangeEventTypes.Items.Add(ListItems.GetOptionAll());

            List<ChangeEventType> changeEventTypes = ChangeEventTypeUtil.GetChangeEventTypes(CurrentUser);

            foreach (ChangeEventType changeEventType in changeEventTypes)
            {
                ListItem li = new ListItem();
                li.Text = changeEventType.ChangeEventTypeName;
                li.Value = changeEventType.ChangeEventTypeId.ToString();
                li.Attributes.Add("title", li.Text);

                lstChangeEventTypes.Items.Add(li);
            }
        }

        //Setup some styling on the page
        private void SetupStyle()
        {
            //Set vertical-align to top for better look
            lblDateFrom.Style.Add("vertical-align", "top");
            txtDateFrom.Style.Add("vertical-align", "top");
            lblDateTo.Style.Add("vertical-align", "top");
            txtDateTo.Style.Add("vertical-align", "top");
            lblModules.Style.Add("vertical-align", "top");
            lblChangeTypes.Style.Add("vertical-align", "top");
            lblChangeEventTypes.Style.Add("vertical-align", "top");
            lblMilitaryUnits.Style.Add("vertical-align", "top");
            lblPersons.Style.Add("vertical-align", "top");
        }

        //Setup any date picker controls on the page by setting the CSS of the target inputs
        //Note that the date picker CSS is common
        //This makes the calendar controls to appear on the page next to each input
        private void SetupDatePickers()
        {
            txtDateFrom.CssClass = CommonFunctions.DatePickerCSS();
            txtDateTo.CssClass = CommonFunctions.DatePickerCSS();
        }

        //Validate the data on the page before doing any server-side actions
        private bool ValidateData()
        {
            bool isDataValid = true;
            string errMsg = "";

            if (txtDateFrom.Text.Trim() != "" && !CommonFunctions.TryParseDate(txtDateFrom.Text))
            {
                isDataValid = false;
                errMsg += "Стойността в полето Дата От не е валидна дата<br/>";
            }

            if (txtDateTo.Text.Trim() != "" && !CommonFunctions.TryParseDate(txtDateTo.Text))
            {
                isDataValid = false;
                errMsg += "Стойността в полето Дата До не е валидна дата<br/>";
            }

            if (!isDataValid)
            {
                lblMessage.CssClass = "ErrorText";
                lblMessage.Text = errMsg;
            }

            return isDataValid;
        }

        //Refresh the data grid on the page
        private void RefreshAuditTrailItems()
        {
            string html = "";

            //Collect information on the page about the filter, the order and paging control information
            //Note that on this screen all this information is collected into a single object
            AuditTrailHistoryFilter filter = CollectFilterData();

            //Get the list of items that should be loaded in the grid
            List<AuditTrailHistoryRec> changes = AuditTrailHistoryUtil.GetAuditTrailHistory(CurrentUser, filter, pageLength);

            //No data found
            if (changes.Count == 0)
            {
                html = "<span>Няма намерени резултати</span>";
            }
            //If there is data the dinamically generate the HTML of the data grid
            else
            {
                string headerStyle = "vertical-align: bottom;";
                int orderCol = (filter.OrderBy > 100 ? filter.OrderBy - 100 : filter.OrderBy);
                string img = filter.OrderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
                string[] arrOrderCol = { "", "", "", "", "", "", "" , "", ""};
                arrOrderCol[orderCol - 1] = @"<div style='position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

                //The header of the data grid
                html = @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='width: 130px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Потребител" + arrOrderCol[0] + @"</th>
                               <th style='width: 70px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>Дата" + arrOrderCol[1] + @"</th>
                               <th style='width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(9);'>IP адрес" + arrOrderCol[8] + @"</th>
                               <th style='width: 120px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>Модул" + arrOrderCol[2] + @"</th>
                               <th style='width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(4);'>Тип на промяната" + arrOrderCol[3] + @"</th>
                               <th style='width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(5);'>Събитие" + arrOrderCol[4] + @"</th>
                               <th style='width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(6);'>Описание" + arrOrderCol[5] + @"</th>
                               <th style='width: 100px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(7);'>" + this.MilitaryUnitLabel + arrOrderCol[6] + @"</th>
                               <th style='width: 130px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(8);'>Човек" + arrOrderCol[7] + @"</th>
                               <th style='width: 230px;" + headerStyle + @"'>Детайли на промяната</th>
                            </tr>
                         </thead>";

                int counter = 1;

                //Iterate through all items in the list and add them to the data grid
                foreach (AuditTrailHistoryRec change in changes)
                {
                    string cellStyle = "vertical-align: top;";

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='" + cellStyle + @"'>" + change.RowNumber.ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + change.UserFullName + @"</td>
                                 <td style='" + cellStyle + @"'>" + CommonFunctions.FormatDateTime(change.ChangeDate) + @"</td>
                                 <td style='" + cellStyle + @"'>" + change.IP + @"</td>
                                 <td style='" + cellStyle + @"'>" + change.ModuleName + @"</td>
                                 <td style='" + cellStyle + @"'>" + change.ChangeTypeName + @"</td>
                                 <td style='" + cellStyle + @"'>" + change.ChangeEventTypeName + @"</td>
                                 <td style='" + cellStyle + @"'>" + change.ObjectDesc + @"</td>
                                 <td style='" + cellStyle + @"'>" + change.MilitaryUnitName + @"</td>
                                 <td style='" + cellStyle + @"'>" + change.PersonFullName + (change.PIdentityNumber == "" ? "" : " (" + change.PIdentityNumber + ")") + @"</td>
                                 <td style='" + cellStyle + @"'>" + change.ChangeDetailsHTML + @"</td>
                              </tr>";

                    counter++;
                }

                html += "</table>";
            }

            //Put the dynamically generated grid into the page
            pnlAuditTrailItems.InnerHtml = html;

            //Refresh the paging buttons
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
                RefreshAuditTrailItems();
            }
        }

        //Go to the first page and refresh the grid
        protected void btnFirst_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = "1";
                RefreshAuditTrailItems();
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

                    RefreshAuditTrailItems();
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

                    RefreshAuditTrailItems();
                }
            }
        }

        //Go to the last page and refresh the grid
        protected void btnLast_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = maxPage.ToString();
                RefreshAuditTrailItems();
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
                    RefreshAuditTrailItems();
                }
            }
        }

        //Refresh the paging buttons
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

        //Collect the filter, the order and the paging information from the page into a single object
        private AuditTrailHistoryFilter CollectFilterData()
        {
            AuditTrailHistoryFilter filter = new AuditTrailHistoryFilter();

            string users = CommonFunctions.GetSelectedValues(lstUsers);
            string modules = CommonFunctions.GetSelectedValues(lstModules);
            string changeTypes = CommonFunctions.GetSelectedValues(lstChangeTypes);
            string changeEventTypes = CommonFunctions.GetSelectedValues(lstChangeEventTypes);
            string militaryUnits = (musMilitaryUnit.SelectedValue != "" && musMilitaryUnit.SelectedValue != "-1" ? musMilitaryUnit.SelectedValue : "");
            string personIdentityNumber = txtPersonIdentityNumber.Text;
            string objectDesc = txtObjectDesc.Text;
            DateTime? dateFrom = null;

            if (CommonFunctions.TryParseDate(txtDateFrom.Text))
                dateFrom = CommonFunctions.ParseDate(txtDateFrom.Text);

            DateTime? dateTo = null;

            if (CommonFunctions.TryParseDate(txtDateTo.Text))
                dateTo = CommonFunctions.ParseDate(txtDateTo.Text);

            string oldValue = txtOldValue.Text;
            string newValue = txtNewValue.Text;

            int? loginLogId = LoginLogId;

            int orderBy;
            if (!int.TryParse(hdnSortBy.Value, out orderBy))
                orderBy = 0;

            int pageIdx;
            if (!int.TryParse(hdnPageIdx.Value, out pageIdx))
                pageIdx = 0;

            filter.Users = users;
            filter.Modules = modules;
            filter.ChangeTypes = changeTypes;
            filter.ChangeEventTypes = changeEventTypes;
            filter.MilitaryUnits = militaryUnits;
            filter.PersonIdentityNumber = personIdentityNumber;
            filter.ObjectDesc = objectDesc;
            filter.DateFrom = dateFrom;
            filter.DateTo = dateTo;
            filter.OldValue = oldValue;
            filter.NewValue = newValue;
            filter.LoginLogId = loginLogId;
            filter.OrderBy = orderBy;
            filter.PageIdx = pageIdx;

            return filter;
        }

        //Navigate back to the home screen
        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (LoginLogId.HasValue)
            {
                Response.Redirect("~/ContentPages/LoginLog.aspx");
            }
            else
            {
                Response.Redirect("~/ContentPages/Home.aspx");
            }
        }
        //Set label text
        private void SetLabelValueText()
        {
            this.lblMilitaryUnits.Text = this.MilitaryUnitLabel + ":";
        }

        private void SetHeaderTitle()
        {
            if (LoginLogId.HasValue)
            {
                PMIS.Common.LoginLog loginLog = LoginLogUtil.GetLoginLog(LoginLogId.Value, CurrentUser);

                User user = UserUtil.GetUser(CurrentUser, loginLog.UserID);

                this.lblHeaderTitle.InnerHtml = "Одитни записи за потребителска сесия от " +
                                                CommonFunctions.FormatDateTime(loginLog.LoginDateTime) +
                                                " на " + user.FullName + " (" + user.Username + ")";
            }
            else
            {
                this.lblHeaderTitle.InnerHtml = "Одитни записи";
            }           
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            lstUsers.SelectedValue = ListItems.GetOptionAll().Value;
            lstModules.SelectedValue = ListItems.GetOptionAll().Value;
            lstChangeTypes.SelectedValue = ListItems.GetOptionAll().Value;
            lstChangeEventTypes.SelectedValue = ListItems.GetOptionAll().Value;           
            musMilitaryUnit.SelectedText = "";
            musMilitaryUnit.SelectedValue = ListItems.GetOptionAll().Value;
            txtDateFrom.Text = "";
            txtDateTo.Text = "";
            txtObjectDesc.Text = "";
            txtPersonIdentityNumber.Text = "";
            txtOldValue.Text = "";
            txtNewValue.Text = "";
        }
    }
}
