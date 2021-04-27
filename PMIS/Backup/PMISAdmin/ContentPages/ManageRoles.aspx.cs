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
    public partial class ManageRoles : AdmPage
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
                return "ADM_SECURITY_ROLES";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if (GetUIItemAccessLevel("ADM_SECURITY_ROLES") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            SetBtnNew();

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteRole")
            {
                JSDeleteRole();
                return;
            }

            //Hilight the current page in the menu bar
            HighlightMenuItems("Users", "Users_ManageRoles");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            //Setup some basic styles on the screen
            SetupStyle();

            //Collect the filter information to be able to pull the number of rows for this specific filter
            string modules = "";

            if (ddModules.SelectedValue != ListItems.GetOptionAll().Value)
                modules = ddModules.SelectedValue;


            int allRows = UserRoleUtil.GetUserRolesCnt(CurrentUser, txtRole.Text, modules);
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
            PopulateModules();
        }

        //Populate the modules drop-down
        private void PopulateModules()
        {
            ddModules.Items.Clear();
            ddModules.Items.Add(ListItems.GetOptionAll());

            List<Module> modules = ModuleUtil.GetModules(CurrentUser);

            foreach (Module module in modules)
            {
                ListItem li = new ListItem();
                li.Text = module.ModuleName;
                li.Value = module.ModuleId.ToString();

                ddModules.Items.Add(li);
            }
        }

        //Setup some styling on the page
        private void SetupStyle()
        {
            lblModule.Style.Add("vertical-align", "top");
            ddModules.Style.Add("vertical-align", "top");
            lblRoles.Style.Add("vertical-align", "top");
            txtRole.Style.Add("vertical-align", "top");
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
        private void RefreshUserRoles()
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
            string modules = "";

            if (ddModules.SelectedValue != ListItems.GetOptionAll().Value)
                modules = ddModules.SelectedValue;

            //Get the list of Roles according to the specified filters, order and paging
            List<UserRole> roles = UserRoleUtil.GetUserRoles(CurrentUser, txtRole.Text, modules, orderBy, pageIdx, pageLength);

            //No data found
            if (roles.Count == 0)
            {
                html = "<span>Няма намерени резултати</span>";
            }
            //If there is data then generate dynamically the HTML for the data grid
            else
            {
                string headerStyle = "vertical-align: bottom;";
                int orderCol = (orderBy > 100 ? orderBy - 100 : orderBy);
                string img = orderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
                string[] arrOrderCol = { "", "", "" };
                arrOrderCol[orderCol - 1] = @"<div style='position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

                //Setup the header of the grid
                html = @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='width: 170px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Модул" + arrOrderCol[0] + @"</th>
                               <th style='width: 170px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>Роля" + arrOrderCol[1] + @"</th>
                               <th style='width: 150px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>Брой потребители" + arrOrderCol[2] + @"</th>
                               <th style='width: 60px;" + headerStyle + @"'>&nbsp;</th>
                            </tr>
                         </thead>";

                int counter = 1;

                //Iterate through all items and add them into the grid
                foreach (UserRole role in roles)
                {
                    string cellStyle = "vertical-align: top;";

                    string deleteHTML = "";

                    if (role.CanDelete)
                    {
                        if (GetUIItemAccessLevel("ADM_SECURITY") == UIAccessLevel.Enabled &&
                            GetUIItemAccessLevel("ADM_SECURITY_ROLES") == UIAccessLevel.Enabled &&
                            GetUIItemAccessLevel("ADM_SECURITY_ROLES_DELETEROLE") == UIAccessLevel.Enabled
                            )
                            deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на тази роля' class='GridActionIcon' onclick='DeleteRole(" + role.RoleId.ToString() + ");' />";
                    }

                    string editHTML = "";
                    string uiItemPerRoleHTML = "";


                    if (GetUIItemAccessLevel("ADM_SECURITY_ROLES_EDITROLE") != UIAccessLevel.Hidden)
                        editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='EditRole(" + role.RoleId.ToString() + ");' />";

                    if (GetUIItemAccessLevel("ADM_SECURITY_UIITEMSPERROLE") != UIAccessLevel.Hidden)
                        uiItemPerRoleHTML = "<img src='../Images/key.png' alt='Рестрикции за тази роля' title='Рестрикции за тази роля' class='GridActionIcon' onclick='UIItemPerRole(" + role.RoleId.ToString() + ");' />";


                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='" + cellStyle + @"'>" + ((pageIdx - 1) * pageLength + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + role.Module.ModuleName + @"</td>
                                 <td style='" + cellStyle + @"'>" + role.RoleName + @"</td>
                                 <td style='" + cellStyle + @"'>" + role.UsersCount.ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + editHTML + uiItemPerRoleHTML + deleteHTML + @"</td>
                              </tr>";

                    counter++;
                }

                html += "</table>";
            }

            //Put the generated grid on the page
            pnlRolesGrid.InnerHtml = html;

            //Refresh the paging button according to the current position
            SetImgBtns();
            lblPagination.Text = " | " + hdnPageIdx.Value + " от " + maxPage.ToString() + " | ";
            txtGotoPage.Text = "";

            //Set the message if there is a need (e.g. a deleted item)
            if (hdnRefreshReason.Value != "")
            {
                if (hdnRefreshReason.Value == "DELETED")
                {
                    lblGridMessage.Text = "Ролята беше изтрита успешно";
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
                RefreshUserRoles();
            }
        }

        //Go to the first page and refresh the grid
        protected void btnFirst_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = "1";
                RefreshUserRoles();
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

                    RefreshUserRoles();
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

                    RefreshUserRoles();
                }
            }
        }

        //Go to the last page and refresh the grid
        protected void btnLast_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                hdnPageIdx.Value = maxPage.ToString();
                RefreshUserRoles();
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
                    RefreshUserRoles();
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
            Response.Redirect("~/ContentPages/AddEditRole.aspx");
        }

        //Delete role (ajax call)
        private void JSDeleteRole()
        {
            if (GetUIItemAccessLevel("ADM_SECURITY") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("ADM_SECURITY_ROLES") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("ADM_SECURITY_ROLES_DELETEROLE") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            int roleId = int.Parse(Request.Form["RoleId"]);

            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "ADM_Roles");

                UserRoleUtil.DeleteRole(CurrentUser, roleId, change);

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
            if (GetUIItemAccessLevel("ADM_SECURITY") == UIAccessLevel.Enabled &&
                GetUIItemAccessLevel("ADM_SECURITY_ROLES") == UIAccessLevel.Enabled &&
                GetUIItemAccessLevel("ADM_SECURITY_ROLES_ADDROLE") == UIAccessLevel.Enabled)
            {
                EnableButton(btnNew);
            }
            else
            {
                HideControl(btnNew);
            }
        }
    }
}
