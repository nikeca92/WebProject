using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class PostponeTechPage : RESPage
    {
        private bool tableEditPermission = true;

        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "RES_POSTPONE_TECH";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {           
            //Highlight the current page in the menu bar
            HighlightMenuItems("Postpone", "PostponeTechnics");

            // Prevent showing "Save changes" dialog box
            LnkForceNoChangesCheck(btnNewPostponeTechCompany);

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSCheckForDataPerCompany")
            {
                JSCheckForDataPerCompany();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSavePostponeTechCompany")
            {
                JSSavePostponeTechCompany();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetPostponeTechCompany")
            {
                JSGetPostponeTechCompany();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeletePostponeTechCompany")
            {
                JSDeletePostponeTechCompany();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSIsThereAnyCompanyData")
            {
                JSIsThereAnyCompanyData();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSCopyPostponeTech")
            {
                JSCopyPostponeTech();
                return;
            }

            if (!IsPostBack)
            {
                SetPageName(); // sets page titles
                LoadDropDowns(); //fills dropdowns on the page with values

                //The default order is by postpone number
                if (string.IsNullOrEmpty(hdnSortBy.Value))
                    hdnSortBy.Value = "1";

                LoadPostponeTechCompaniesTable();
            }

            SetupPageUI(); //setup user interface elements according to rights of the user's role
            SetupDatePickers(); //Setup any calendar control on the screen

            SetPostponeTechCompanyMessage(); //display message from ajax operations on protocol items, if exist            
        }       

        // Set page titles
        private void SetPageName()
        {
            Page.Title = lblHeaderTitle.InnerHtml;
        }

        // Setup any date picker controls on the page by setting the CSS of the target inputs
        // Note that the date picker CSS is common
        // This makes the calendar controls to appear on the page next to each input
        private void SetupDatePickers()
        {
            
        }

        // Setup user interface elements according to rights of the user's role
        private void SetupPageUI()
        {
            bool screenHidden = (this.GetUIItemAccessLevel("RES_POSTPONE") == UIAccessLevel.Hidden) ||
                                (this.GetUIItemAccessLevel("RES_POSTPONE_TECH") == UIAccessLevel.Hidden);

            bool screenDisabled = (this.GetUIItemAccessLevel("RES_POSTPONE") == UIAccessLevel.Disabled) ||
                                  (this.GetUIItemAccessLevel("RES_POSTPONE_TECH") == UIAccessLevel.Disabled);

            if (screenHidden)
                RedirectAccessDenied();

            if (screenDisabled)
            {
                this.pageHiddenControls.Add(btnNewPostponeTechCompany);
                this.pageHiddenControls.Add(btnCopy);
                tableEditPermission = false;
            }
        }

        // Display message from ajax operations on protocol items, if exist
        private void SetPostponeTechCompanyMessage()
        {
            if (hfMsg.Value == "FailPostponeTechCompanySave")
            {
                lblPostponeTechCompanyMessage.Text = "Неуспешен запис на позиция";
                lblPostponeTechCompanyMessage.CssClass = "ErrorText";
            }
            else if (hfMsg.Value == "SuccessPostponeTechCompanySave")
            {
                lblPostponeTechCompanyMessage.Text = "Успешен запис на позиция";
                lblPostponeTechCompanyMessage.CssClass = "SuccessText";
            }
            else if (hfMsg.Value == "FailPostponeTechCompanyDelete")
            {
                lblPostponeTechCompanyMessage.Text = "Неуспешно изтриване на позиция";
                lblPostponeTechCompanyMessage.CssClass = "ErrorText";
            }
            else if (hfMsg.Value == "SuccessPostponeTechCompanyDelete")
            {
                lblPostponeTechCompanyMessage.Text = "Успешно изтриване на позиция";
                lblPostponeTechCompanyMessage.CssClass = "SuccessText";
            }
            else if (hfMsg.Value == "SuccessCopyPostponeTech")
            {
                lblPostponeTechCompanyMessage.Text = "Успешно копиране на отсрочване";
                lblPostponeTechCompanyMessage.CssClass = "SuccessText";
            }
            else
            {
                lblPostponeTechCompanyMessage.Text = "";
            }
          
            hfMsg.Value = ""; //clean message form ajax operations
        }

        // Populate all dropdowns on the screen
        private void LoadDropDowns()
        {
            ddMilitaryDepartment.DataSource = MilitaryDepartmentUtil.GetAllMilitaryDepartments(CurrentUser);
            ddMilitaryDepartment.DataTextField = "MilitaryDepartmentName";
            ddMilitaryDepartment.DataValueField = "MilitaryDepartmentId";
            ddMilitaryDepartment.DataBind();
            ddMilitaryDepartment.Items.Insert(0, ListItems.GetOptionChooseOne());

            ddPostponeYear.Items.Clear();
            List<int> years = PostponeTechUtil.GetAllPostponeTechYears(CurrentUser);
            int minYear = DateTime.Now.Year;
            int maxYear = DateTime.Now.Year + 1;
            if (years.Count() > 0 && years.Min() < minYear)
                minYear = years.Min();
            if (years.Count() > 0 && years.Max() > maxYear)
                maxYear = years.Max();
            for (int year = minYear; year <= maxYear; year++)
                ddPostponeYear.Items.Add(year.ToString());
            ddPostponeYear.Items.Insert(0, ListItems.GetOptionChooseOne());
            ddPostponeYear.SelectedValue = DateTime.Now.Year.ToString();

            ddMilitaryDepartmentLightBox.DataSource = MilitaryDepartmentUtil.GetAllMilitaryDepartments(CurrentUser);
            ddMilitaryDepartmentLightBox.DataTextField = "MilitaryDepartmentName";
            ddMilitaryDepartmentLightBox.DataValueField = "MilitaryDepartmentId";
            ddMilitaryDepartmentLightBox.DataBind();
            ddMilitaryDepartmentLightBox.Items.Insert(0, ListItems.GetOptionChooseOne());

            ddPostponeYearLightBox.Items.Clear();
            for (int year = minYear; year <= maxYear; year++)
                ddPostponeYearLightBox.Items.Add(year.ToString());
            ddPostponeYearLightBox.Items.Insert(0, ListItems.GetOptionChooseOne());
            ddPostponeYearLightBox.SelectedValue = ((int)(DateTime.Now.Year + 1)).ToString();
        }

        // Loads html table in the page with postpone items to this protocol
        private void LoadPostponeTechCompaniesTable()
        {
            divPostponeTechCompanies.InnerHtml = GeneratePostponeTechCompaniesTable(); //generate and display html with protocol items table on the page
            SetButtonsVisibility();
        }

        // Generates html table from list of protocol items
        private string GeneratePostponeTechCompaniesTable()
        {
            string html = "";

            //Collect the filters, the order control and the paging control data from the page
            int orderBy;
            if (!int.TryParse(hdnSortBy.Value, out orderBy))
                orderBy = 1;

            //Get the list of postpones according to the specified filters, order and paging
            List<PostponeTechCompany> postponeTechCompanies = new List<PostponeTechCompany>();
            if (ddPostponeYear.SelectedValue != ListItems.GetOptionChooseOne().Value && 
                ddMilitaryDepartment.SelectedValue != ListItems.GetOptionChooseOne().Value)
            {
                int postponeYear = int.Parse(ddPostponeYear.SelectedValue);
                int militaryDepartmentId = int.Parse(ddMilitaryDepartment.SelectedValue);
                postponeTechCompanies = PostponeTechCompanyUtil.GetPostponeTechCompanies(postponeYear, militaryDepartmentId, orderBy, CurrentUser);
            }

            hdnPostponeTechCompaniesCnt.Value = postponeTechCompanies.Count.ToString();

            //No data found
            if (postponeTechCompanies.Count == 0)
            {
                string noDataMsg = "";

                if(ddPostponeYear.SelectedValue != ListItems.GetOptionChooseOne().Value && 
                   ddMilitaryDepartment.SelectedValue != ListItems.GetOptionChooseOne().Value)
                    noDataMsg = "Няма намерени данни";
                else
                    noDataMsg = "Моля, изберете година и военно окръжие";

                html = "<div style='padding-top: 30px;'>" + noDataMsg + "</div>";
            }
            //If there is data then generate dynamically the HTML for the data grid
            else
            {
                string headerStyle = "vertical-align: bottom;";
                int orderCol = (orderBy > 100 ? orderBy - 100 : orderBy);
                string img = orderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
                string[] arrOrderCol = { "", "", "", "", "", "", "", "" };
                arrOrderCol[orderCol - 1] = @"<div style='position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

                //Setup the header of the grid
                html = @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <colgroup>
                            <col style='width: 20px;'>
                            <col style='width: 480px;'>
                            <col style='width: 105px;'>
                            <col style='width: 105px;'>
                         </colgroup>
                         <thead>
                            <tr>
                                <th style='" + headerStyle + @"'>№</th>
                                <th style='cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Име на фирма" + arrOrderCol[0] + @"</th>
                                <th style='cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>Предл. за безусловно отсрочване" + arrOrderCol[1] + @"</th>
                                <th style='cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>Предл. за условно отсрочване</th>
                                <th style='" + headerStyle + @"'>&nbsp;</th>
                            </tr>
                         </thead>";

                int counter = 1;

                //Iterate through all items and add them into the grid
                foreach (PostponeTechCompany postponeTechCompany in postponeTechCompanies)
                {
                    string cellStyle = "vertical-align: top; text-align: center;";

                    string deleteHTML = "";
                    if (tableEditPermission)
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на тази позиция' class='GridActionIcon' onclick='DeletePostponeTechCompany(" + postponeTechCompany.PostponeTechCompanyID.ToString() + ");' />";
                    

                    string editHTML = "";
                    if (tableEditPermission)
                        editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='ShowPostponeTechCompanyLightBox(" + postponeTechCompany.PostponeTechCompanyID.ToString() + ", " + postponeTechCompany.CompanyID.ToString() + ");' />";


                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='" + cellStyle + @"'>" + counter.ToString() + @"</td>
                                 <td style='vertical-align: top;'>" + postponeTechCompany.Company.CompanyName + @"</td>
                                 <td style='" + cellStyle + @"'>" + postponeTechCompany.PostponeAbsolutely.ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + postponeTechCompany.PostponeConditioned.ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + editHTML + deleteHTML + @"</td>
                              </tr>";

                    counter++;
                }

                html += "</table>";
            }            

            return html;
        }

        private void JSCheckForDataPerCompany()
        {
            if (GetUIItemAccessLevel("RES_POSTPONE") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_POSTPONE_TECH") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            int companyid = string.IsNullOrEmpty(Request.Form["compid"]) ? 0 : int.Parse(Request.Form["compid"]);
            int postponeYear = int.Parse(Request.Form["PostponeYear"]);
            int militaryDepartmentID = int.Parse(Request.Form["MilitaryDepartmentID"]);

            int postponeTechCompanyID = 0;

            string stat = "";
            string response = "";

            if (companyid > 0)
            {
                try
                {
                    Company company = CompanyUtil.GetCompany(companyid, CurrentUser);

                    if (company != null)
                    {
                        PostponeTechCompany postponeTechCompany = PostponeTechCompanyUtil.GetPostponeTechCompany(postponeYear, militaryDepartmentID, companyid, CurrentUser);
                        if (postponeTechCompany != null)
                            postponeTechCompanyID = postponeTechCompany.PostponeTechCompanyID;
                    }
                }
                catch (Exception ex)
                {
                    stat = AJAXTools.ERROR;
                    response = AJAXTools.EncodeForXML(ex.Message);
                }
            }

            stat = AJAXTools.OK;
            response = @"<response>
                            <postponeTechCompanyID>" + postponeTechCompanyID.ToString() + @"</postponeTechCompanyID>
                         </response>
                        ";

            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        // Saves postpone item from ajax request
        private void JSSavePostponeTechCompany()
        {
            string resultMsg = "";

            int postponeYear = int.Parse(Request.Form["PostponeYear"]);
            int militaryDepartmentID = int.Parse(Request.Form["MilitaryDepartmentID"]);

            int postponeTechCompanyID = int.Parse(Request.Form["PostponeTechCompanyID"]);
            int companyID = 0;
            int.TryParse(Request.Params["CompanyID"], out companyID);            
           
            Change change = new Change(CurrentUser, "RES_PostponeTech");

            int postponeTechID = 0;
            PostponeTech postponeTech = PostponeTechUtil.GetPostponeTech(postponeYear, militaryDepartmentID, CurrentUser);
            if (postponeTech != null)
            {
                postponeTechID = postponeTech.PostponeTechID;
            }
            else
            {
                postponeTech = new PostponeTech(CurrentUser);
                postponeTech.PostponeYear = postponeYear;
                postponeTech.MilitaryDepartmentID = militaryDepartmentID;
                PostponeTechUtil.SavePostponeTech(postponeTech, CurrentUser, change);
                postponeTechID = postponeTech.PostponeTechID;
            }


            PostponeTechCompany postponeTechCompany = new PostponeTechCompany(CurrentUser);
            postponeTechCompany.PostponeTechCompanyID = postponeTechCompanyID;
            postponeTechCompany.CompanyID = companyID;

            resultMsg = "";

            if (PostponeTechCompanyUtil.SavePostponeTechCompany(postponeTechID, postponeTechCompany, CurrentUser, change))
            {
                int itemsCnt = int.Parse(Request.Form["ItemsCnt"]);

                for (int i = 1; i <= itemsCnt; i++)
                {
                    int postponeTechItemID = int.Parse(Request.Form["PostponeTechItemID" + i]);
                    int technicsSubTypeID = int.Parse(Request.Form["TechnicsSubTypeID" + i]);

                    int? postponeConditioned = !string.IsNullOrEmpty(Request.Form["PostponeConditioned" + i]) ? (int?)int.Parse(Request.Form["PostponeConditioned" + i]) : null;
                    int? postponeAbsolutely = !string.IsNullOrEmpty(Request.Form["PostponeAbsolutely" + i]) ? (int?)int.Parse(Request.Form["PostponeAbsolutely" + i]) : null;

                    PostponeTechItem item = new PostponeTechItem(CurrentUser);
                    item.PostponeTechItemID = postponeTechItemID;
                    item.TechnicsSubTypeID = technicsSubTypeID;
                    item.PostponeConditioned = postponeConditioned;
                    item.PostponeAbsolutely = postponeAbsolutely;

                    PostponeTechItemUtil.SavePostponeTechItem(postponeTechID, postponeTechCompany.PostponeTechCompanyID, item, CurrentUser, change);
                }

                resultMsg = AJAXTools.OK;
            }
            else
            {
                resultMsg = AJAXTools.ERROR;
            }

            change.WriteLog();

            string response = @"<response>" + AJAXTools.EncodeForXML(resultMsg) + "</response>";
            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();
        }

        // Get ajax-requested data for postpone item(by postponeTechCompanyID)
        private void JSGetPostponeTechCompany()
        {
            int postponeTechCompanyID = int.Parse(Request.Form["PostponeTechCompanyID"]);
            int preLoadCompanyID = int.Parse(Request.Form["CompanyID"]);

            PostponeTechCompany p = PostponeTechCompanyUtil.GetPostponeTechCompany(postponeTechCompanyID, CurrentUser);
            List<PostponeTechItem> postponeTechItems = null;

            string unifiedIdentityCode = "";
            string companyId = "";
            string companyName = "";
            string ownershipType = "";
            string administration = "";

            if (postponeTechCompanyID > 0)
            {
                postponeTechItems = PostponeTechItemUtil.GetAllPostponeTechItemsByPostponeTechCompanyID(postponeTechCompanyID, CurrentUser);

                unifiedIdentityCode = p.Company.UnifiedIdentityCode;
                companyId = p.Company.CompanyId.ToString();
                companyName= p.Company.CompanyName;
                ownershipType = p.Company.OwnershipType.OwnershipTypeName;
                administration = (p.Company.AdministrationId.HasValue ? p.Company.Administration.AdministrationName : "");
            }
            else
            {
                postponeTechItems = PostponeTechItemUtil.GetDefaultPostponeTechItems(CurrentUser);

                if (preLoadCompanyID <= 0)
                {
                    unifiedIdentityCode = "";
                    companyId = ListItems.GetOptionChooseOne().Value;
                    companyName = "";
                    ownershipType = "";
                    administration = "";
                }
                else
                {
                    Company company = CompanyUtil.GetCompany(preLoadCompanyID, CurrentUser);
                    unifiedIdentityCode = company.UnifiedIdentityCode;
                    companyId = company.CompanyId.ToString();
                    companyName = company.CompanyName;
                    ownershipType = company.OwnershipType.OwnershipTypeName;
                    administration = (company.AdministrationId.HasValue ? company.Administration.AdministrationName : "");
                }
            }

            string response = @"<response>
                                   <UnifiedIdentityCode>" + AJAXTools.EncodeForXML(unifiedIdentityCode) + @"</UnifiedIdentityCode>
                                   <CompanyID>" + AJAXTools.EncodeForXML(companyId) + @"</CompanyID>
                                   <CompanyName>" + AJAXTools.EncodeForXML(companyName) + @"</CompanyName>
                                   <OwnershipType>" + AJAXTools.EncodeForXML(ownershipType) + @"</OwnershipType>
                                   <Administration>" + AJAXTools.EncodeForXML(administration) + @"</Administration>
                                ";

            string itemsXml = "<PostponeTechItems>";
            foreach (PostponeTechItem item in postponeTechItems)
            {
                itemsXml += @"<PostponeTechItem>
                                 <PostponeTechItemID>" + AJAXTools.EncodeForXML(item.PostponeTechItemID.ToString()) + @"</PostponeTechItemID>
                                 <TechnicsTypeName>" + AJAXTools.EncodeForXML(item.TechnicsSubType.TechnicsType.TypeName) + @"</TechnicsTypeName>
                                 <TechnicsTypeID>" + AJAXTools.EncodeForXML(item.TechnicsSubType.TechnicsType.TechnicsTypeId.ToString()) + @"</TechnicsTypeID>
                                 <TechnicsSubTypeName>" + AJAXTools.EncodeForXML(item.TechnicsSubType.TechnicsSubTypeName) + @"</TechnicsSubTypeName>
                                 <TechnicsSubTypeID>" + AJAXTools.EncodeForXML(item.TechnicsSubType.TechnicsSubTypeId.ToString()) + @"</TechnicsSubTypeID>
                                 <PostponeConditioned>" + AJAXTools.EncodeForXML(item.PostponeConditioned.HasValue ? item.PostponeConditioned.Value.ToString() : "") + @"</PostponeConditioned>
                                 <PostponeAbsolutely>" + AJAXTools.EncodeForXML(item.PostponeAbsolutely.HasValue ? item.PostponeAbsolutely.Value.ToString() : "") + @"</PostponeAbsolutely>
                              </PostponeTechItem>";
            }
            itemsXml += "</PostponeTechItems>";

            response += itemsXml +
                        "</response>";

            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();
        }

        // Deletes postpone item by ajax request
        private void JSDeletePostponeTechCompany()
        {
            if (this.GetUIItemAccessLevel("RES_POSTPONE") != UIAccessLevel.Enabled || this.GetUIItemAccessLevel("RES_POSTPONE_TECH") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            int postponeYear = int.Parse(Request.Form["PostponeYear"]);
            int MilitaryDepartmentID = int.Parse(Request.Form["MilitaryDepartmentID"]);
            PostponeTech postponeTech = PostponeTechUtil.GetPostponeTech(postponeYear, MilitaryDepartmentID, CurrentUser);

            int postponeTechCompanyID = int.Parse(Request.Form["PostponeTechCompanyID"]);

            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_PostponeTech");

                PostponeTechCompanyUtil.DeletePostponeTechCompany(postponeTech.PostponeTechID, postponeTechCompanyID, CurrentUser, change);               

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

        // check for uniqueness of military department and postpone year
        private void JSIsThereAnyCompanyData()
        {
            if (this.GetUIItemAccessLevel("RES_POSTPONE") != UIAccessLevel.Enabled || this.GetUIItemAccessLevel("RES_POSTPONE_TECH") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            int militaryDepartmentID = int.Parse(Request.Form["MilitaryDepartmentID"]);
            int postponeYear = int.Parse(Request.Form["PostponeYear"]);

            string stat = "";
            string response = "";

            try
            {
                string result = "";

                result = PostponeTechUtil.IsThereAnyCompanyData(militaryDepartmentID, postponeYear, CurrentUser) ? "OK" : "NO";

                stat = AJAXTools.OK;
                response = "<response>OK</response><result>" + result + "</result>";
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

        private void JSCopyPostponeTech()
        {
            if (this.GetUIItemAccessLevel("RES_POSTPONE") != UIAccessLevel.Enabled || this.GetUIItemAccessLevel("RES_POSTPONE_TECH") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            int militaryDepartmentID_Old = int.Parse(Request.Form["MilitaryDepartmentID_Old"]);
            int postponeYear_Old = int.Parse(Request.Form["PostponeYear_Old"]);
            int militaryDepartmentID_New = int.Parse(Request.Form["MilitaryDepartmentID_New"]);
            int postponeYear_New = int.Parse(Request.Form["PostponeYear_New"]);

            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_PostponeTech");

                PostponeTech postponeTech_Old = PostponeTechUtil.GetPostponeTech(postponeYear_Old, militaryDepartmentID_Old, CurrentUser);
                PostponeTech postponeTech_New = PostponeTechUtil.GetPostponeTech(postponeYear_New, militaryDepartmentID_New, CurrentUser);

                if (postponeTech_New == null)
                {
                    postponeTech_New = new PostponeTech(CurrentUser);
                    postponeTech_New.PostponeTechID = 0;
                    postponeTech_New.MilitaryDepartmentID = militaryDepartmentID_New;
                    postponeTech_New.PostponeYear = postponeYear_New;
                    PostponeTechUtil.SavePostponeTech(postponeTech_New, CurrentUser, change);
                }

                PostponeTechUtil.CopyPostponeTech(postponeTech_Old, postponeTech_New, CurrentUser, change);

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

        // hidden button for forced refresh of postpone items table
        protected void btnHdnRefreshPostponeTechCompanies_Click(object sender, EventArgs e)
        {
            LoadPostponeTechCompaniesTable();
        }

        protected void ddPostponeYear_Change(object sender, EventArgs e)
        {
            LoadPostponeTechCompaniesTable();
        }

        protected void ddMilitaryDepartment_Change(object sender, EventArgs e)
        {
            LoadPostponeTechCompaniesTable();
        }

        private void SetButtonsVisibility()
        {
            if (ddPostponeYear.SelectedValue != ListItems.GetOptionChooseOne().Value &&
                ddMilitaryDepartment.SelectedValue != ListItems.GetOptionChooseOne().Value)
            {
                btnNewPostponeTechCompany.Visible = true;

                if (int.Parse(hdnPostponeTechCompaniesCnt.Value) > 0)
                    btnCopy.Visible = true;
                else
                    btnCopy.Visible = false;
            }
            else
            {
                btnNewPostponeTechCompany.Visible = false;
                btnCopy.Visible = false;
            }
        }
    }
}
