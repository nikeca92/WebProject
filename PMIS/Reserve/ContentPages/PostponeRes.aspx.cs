using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class PostponeResPage : RESPage
    {
        private bool tableEditPermission = true;

        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "RES_POSTPONE_RES";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {           
            //Highlight the current page in the menu bar
            HighlightMenuItems("Postpone", "PostponeReservists");

            // Prevent showing "Save changes" dialog box
            LnkForceNoChangesCheck(btnNewPostponeResCompany);

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSCheckForDataPerCompany")
            {
                JSCheckForDataPerCompany();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSavePostponeResCompany")
            {
                JSSavePostponeResCompany();
                return;
            }            

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetPostponeResCompany")
            {
                JSGetPostponeResCompany();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeletePostponeResCompany")
            {
                JSDeletePostponeResCompany();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSIsThereAnyCompanyData")
            {
                JSIsThereAnyCompanyData();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSCopyPostponeRes")
            {
                JSCopyPostponeRes();
                return;
            }

            if (!IsPostBack)
            {
                SetPageName(); // sets page titles
                LoadDropDowns(); //fills dropdowns on the page with values

                //The default order is by postpone number
                if (string.IsNullOrEmpty(hdnSortBy.Value))
                    hdnSortBy.Value = "1";

                LoadPostponeResCompaniesTable();
            }

            SetupPageUI(); //setup user interface elements according to rights of the user's role
            SetupDatePickers(); //Setup any calendar control on the screen

            SetPostponeResCompanyMessage(); //display message from ajax operations on protocol items, if exist            
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
                                (this.GetUIItemAccessLevel("RES_POSTPONE_RES") == UIAccessLevel.Hidden);

            bool screenDisabled = (this.GetUIItemAccessLevel("RES_POSTPONE") == UIAccessLevel.Disabled) ||
                                  (this.GetUIItemAccessLevel("RES_POSTPONE_RES") == UIAccessLevel.Disabled);

            if (screenHidden)
                RedirectAccessDenied();

            if (screenDisabled)
            {
                this.pageHiddenControls.Add(btnNewPostponeResCompany);
                this.pageHiddenControls.Add(btnCopy);
                tableEditPermission = false;
            }
        }

        // Display message from ajax operations on protocol items, if exist
        private void SetPostponeResCompanyMessage()
        {
            if (hfMsg.Value == "FailPostponeResCompanySave")
            {
                lblPostponeResCompanyMessage.Text = "Неуспешен запис на позиция";
                lblPostponeResCompanyMessage.CssClass = "ErrorText";
            }
            else if (hfMsg.Value == "SuccessPostponeResCompanySave")
            {
                lblPostponeResCompanyMessage.Text = "Успешен запис на позиция";
                lblPostponeResCompanyMessage.CssClass = "SuccessText";
            }
            else if (hfMsg.Value == "FailPostponeResCompanyDelete")
            {
                lblPostponeResCompanyMessage.Text = "Неуспешно изтриване на позиция";
                lblPostponeResCompanyMessage.CssClass = "ErrorText";
            }
            else if (hfMsg.Value == "SuccessPostponeResCompanyDelete")
            {
                lblPostponeResCompanyMessage.Text = "Успешно изтриване на позиция";
                lblPostponeResCompanyMessage.CssClass = "SuccessText";
            }
            else if (hfMsg.Value == "SuccessCopyPostponeRes")
            {
                lblPostponeResCompanyMessage.Text = "Успешно копиране на отсрочване";
                lblPostponeResCompanyMessage.CssClass = "SuccessText";
            }
            else
            {
                lblPostponeResCompanyMessage.Text = "";
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
            List<int> years = PostponeResUtil.GetAllPostponeResYears(CurrentUser);
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
        private void LoadPostponeResCompaniesTable()
        {
            divPostponeResCompanies.InnerHtml = GeneratePostponeResCompaniesTable(); //generate and display html with protocol items table on the page
            SetButtonsVisibility();
        }

        // Generates html table from list of protocol items
        private string GeneratePostponeResCompaniesTable()
        {
            string html = "";

            //Collect the filters, the order control and the paging control data from the page
            int orderBy;
            if (!int.TryParse(hdnSortBy.Value, out orderBy))
                orderBy = 1;

            //Get the list of postpones according to the specified filters, order and paging
            List<PostponeResCompany> postponeResCompanies = new List<PostponeResCompany>();
            if (ddPostponeYear.SelectedValue != ListItems.GetOptionChooseOne().Value && 
                ddMilitaryDepartment.SelectedValue != ListItems.GetOptionChooseOne().Value)
            {
                int postponeYear = int.Parse(ddPostponeYear.SelectedValue);
                int militaryDepartmentId = int.Parse(ddMilitaryDepartment.SelectedValue);
                postponeResCompanies = PostponeResCompanyUtil.GetPostponeResCompanies(postponeYear, militaryDepartmentId, orderBy, CurrentUser);
            }

            hdnPostponeResCompaniesCnt.Value = postponeResCompanies.Count.ToString();

            //No data found
            if (postponeResCompanies.Count == 0)
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
                string[] arrOrderCol = { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
                arrOrderCol[orderCol - 1] = @"<div style='position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

                //Setup the header of the grid
                html = @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <colgroup>
                            <col style='width: 20px;'>
                            <col style='width: 280px;'>
                            <col style='width: 100px;'>
                            <col style='width: 65px;'>
                            <col style='width: 65px;'>
                            <col style='width: 65px;'>
                            <col style='width: 65px;'>
                            <col style='width: 65px;'>
                            <col style='width: 65px;'>
                            <col style='width: 65px;'>
                            <col style='width: 65px;'>
                            <col style='width: 65px;'>
                            <col style='width: 65px;'>
                            <col style='width: 60px;'>
                         </colgroup>
                         <thead>
                            <tr>
                                <th rowspan='4' style='" + headerStyle + @"'>№</th>
                                <th rowspan='4' style='cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Име на фирма" + arrOrderCol[0] + @"</th>
                                <th rowspan='4' style='cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>Обща численост на персонала" + arrOrderCol[1] + @"</th>
                                <th colspan='10' style='" + headerStyle + @"'>Брой на предложените за безусловно и условно отсрочване запасни </th>
                                <th rowspan='4' style='" + headerStyle + @"'>&nbsp;</th>
                            </tr>
                            <tr>                                
                                <th rowspan='2' colspan='2' style='" + headerStyle + @"'>Общ брой</th>
                                <th colspan='8' style='" + headerStyle + @"'>От тях:</th>
                            </tr>
                            <tr>                                
                                <th colspan='2' style='" + headerStyle + @"'>Офицери</th>
                                <th colspan='2' style='" + headerStyle + @"'>Офицерски кандидати</th>
                                <th colspan='2' style='" + headerStyle + @"'>Сержанти / Старшини</th>
                                <th colspan='2' style='" + headerStyle + @"'>Войници / матроси</th>
                            </tr>
                            <tr>                                
                                <th style='cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>безусл. отср." + arrOrderCol[2] + @"</th>
                                <th style='cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(4);'>условно отср." + arrOrderCol[3] + @"</th>
                                <th style='cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(5);'>безусл. отср." + arrOrderCol[4] + @"</th>
                                <th style='cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(6);'>условно отср." + arrOrderCol[5] + @"</th>
                                <th style='cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(7);'>безусл. отср." + arrOrderCol[6] + @"</th>
                                <th style='cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(8);'>условно отср." + arrOrderCol[7] + @"</th>
                                <th style='cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(9);'>безусл. отср." + arrOrderCol[8] + @"</th>
                                <th style='cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(10);'>условно отср." + arrOrderCol[9] + @"</th>
                                <th style='cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(11);'>безусл. отср." + arrOrderCol[10] + @"</th>
                                <th style='cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(12);'>условно отср." + arrOrderCol[11] + @"</th>
                            </tr>
                         </thead>";

                int counter = 1;

                //Iterate through all items and add them into the grid
                foreach (PostponeResCompany postponeResCompany in postponeResCompanies)
                {
                    string cellStyle = "vertical-align: top; text-align: center;";

                    string deleteHTML = "";
                    if (tableEditPermission)
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на тази позиция' class='GridActionIcon' onclick='DeletePostponeResCompany(" + postponeResCompany.PostponeResCompanyID.ToString() + ");' />";
                    

                    string editHTML = "";
                    if (tableEditPermission)
                        editHTML = "<img src='../Images/edit.png' alt='Редактиране' title='Редактиране' class='GridActionIcon' onclick='ShowPostponeResCompanyLightBox(" + postponeResCompany.PostponeResCompanyID.ToString() + ", " + postponeResCompany.CompanyID.ToString() + ");' />";


                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='" + cellStyle + @"'>" + counter.ToString() + @"</td>
                                 <td style='vertical-align: top;'>" + postponeResCompany.Company.CompanyName + @"</td>
                                 <td style='" + cellStyle + @"'>" + (postponeResCompany.EmployeesCnt.HasValue ? postponeResCompany.EmployeesCnt.Value.ToString() : "") + @"</td>
                                 <td style='" + cellStyle + @"'>" + postponeResCompany.TotalAbsolutely.ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + postponeResCompany.TotalConditioned.ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + postponeResCompany.OfficersAbsolutely.ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + postponeResCompany.OfficersConditioned.ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + postponeResCompany.OfCandAbsolutely.ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + postponeResCompany.OfCandConditioned.ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + postponeResCompany.SergeantsAbsolutely.ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + postponeResCompany.SergeantsConditioned.ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + postponeResCompany.SoldiersAbsolutely.ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + postponeResCompany.SoldiersConditioned.ToString() + @"</td>
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
                GetUIItemAccessLevel("RES_POSTPONE_RES") == UIAccessLevel.Hidden)
                RedirectAjaxAccessDenied();

            int companyid = string.IsNullOrEmpty(Request.Form["compid"]) ? 0 : int.Parse(Request.Form["compid"]);
            int postponeYear = int.Parse(Request.Form["PostponeYear"]);
            int militaryDepartmentID = int.Parse(Request.Form["MilitaryDepartmentID"]);

            int postponeResCompanyID = 0;

            string stat = "";
            string response = "";

            if (companyid > 0)
            {
                try
                {
                    Company company = CompanyUtil.GetCompany(companyid, CurrentUser);

                    if (company != null)
                    {
                        PostponeResCompany postponeResCompany = PostponeResCompanyUtil.GetPostponeResCompany(postponeYear, militaryDepartmentID, companyid, CurrentUser);
                        if (postponeResCompany != null)
                            postponeResCompanyID = postponeResCompany.PostponeResCompanyID;
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
                            <postponeResCompanyID>" + postponeResCompanyID.ToString() + @"</postponeResCompanyID>
                         </response>
                        ";

            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        // Saves postpone item from ajax request
        private void JSSavePostponeResCompany()
        {
            string resultMsg = "";

            int postponeYear = int.Parse(Request.Form["PostponeYear"]);
            int militaryDepartmentID = int.Parse(Request.Form["MilitaryDepartmentID"]);

            int postponeResCompanyID = int.Parse(Request.Form["PostponeResCompanyID"]);
            int companyID = 0;
            int.TryParse(Request.Params["CompanyID"], out companyID);
            int? emloyeesCnt = !string.IsNullOrEmpty(Request.Form["EmployeesCnt"]) ? (int?)int.Parse(Request.Form["EmployeesCnt"]) : null;

            Change change = new Change(CurrentUser, "RES_PostponeRes");

            int postponeResID = 0;
            PostponeRes postponeRes = PostponeResUtil.GetPostponeRes(postponeYear, militaryDepartmentID, CurrentUser);
            if (postponeRes != null)
            {
                postponeResID = postponeRes.PostponeResID;
            }
            else
            {
                postponeRes = new PostponeRes(CurrentUser);
                postponeRes.PostponeYear = postponeYear;
                postponeRes.MilitaryDepartmentID = militaryDepartmentID;
                PostponeResUtil.SavePostponeRes(postponeRes, CurrentUser, change);
                postponeResID = postponeRes.PostponeResID;
            }

            
            PostponeResCompany postponeResCompany = new PostponeResCompany(CurrentUser);
            postponeResCompany.PostponeResCompanyID = postponeResCompanyID;
            postponeResCompany.CompanyID = companyID;
            postponeResCompany.EmployeesCnt = emloyeesCnt;

            resultMsg = "";
            
            if(PostponeResCompanyUtil.SavePostponeResCompany(postponeResID, postponeResCompany, CurrentUser, change))
            {
                int itemsCnt = int.Parse(Request.Form["ItemsCnt"]);

                for (int i = 1; i <= itemsCnt; i++)
                {
                    int postponeResItemID = int.Parse(Request.Form["PostponeResItemID" + i]);
                    int nkpdID = int.Parse(Request.Form["NKPDID" + i]);

                    int? officersConditioned = !string.IsNullOrEmpty(Request.Form["OfficersConditioned" + i]) ? (int?)int.Parse(Request.Form["OfficersConditioned" + i]) : null;
                    int? officersAbsolutely = !string.IsNullOrEmpty(Request.Form["OfficersAbsolutely" + i]) ? (int?)int.Parse(Request.Form["OfficersAbsolutely" + i]) : null;
                    int? ofCandConditioned = !string.IsNullOrEmpty(Request.Form["OfCandConditioned" + i]) ? (int?)int.Parse(Request.Form["OfCandConditioned" + i]) : null;
                    int? ofCandAbsolutely = !string.IsNullOrEmpty(Request.Form["OfCandAbsolutely" + i]) ? (int?)int.Parse(Request.Form["OfCandAbsolutely" + i]) : null;
                    int? sergeantsConditioned = !string.IsNullOrEmpty(Request.Form["SergeantsConditioned" + i]) ? (int?)int.Parse(Request.Form["SergeantsConditioned" + i]) : null;
                    int? sergeantsAbsolutely = !string.IsNullOrEmpty(Request.Form["SergeantsAbsolutely" + i]) ? (int?)int.Parse(Request.Form["SergeantsAbsolutely" + i]) : null;
                    int? soldiersConditioned = !string.IsNullOrEmpty(Request.Form["SoldiersConditioned" + i]) ? (int?)int.Parse(Request.Form["SoldiersConditioned" + i]) : null;
                    int? soldiersAbsolutely = !string.IsNullOrEmpty(Request.Form["SoldiersAbsolutely" + i]) ? (int?)int.Parse(Request.Form["SoldiersAbsolutely" + i]) : null;

                    PostponeResItem item = new PostponeResItem(CurrentUser);
                    item.PostponeResItemID = postponeResItemID;
                    item.NKPDID = nkpdID;
                    item.OfficersConditioned = officersConditioned;
                    item.OfficersAbsolutely = officersAbsolutely;
                    item.OfCandConditioned = ofCandConditioned;
                    item.OfCandAbsolutely = ofCandAbsolutely;
                    item.SergeantsConditioned = sergeantsConditioned;
                    item.SergeantsAbsolutely = sergeantsAbsolutely;
                    item.SoldiersConditioned = soldiersConditioned;
                    item.SoldiersAbsolutely = soldiersAbsolutely;

                    PostponeResItemUtil.SavePostponeResItem(postponeResID, postponeResCompany.PostponeResCompanyID, item, CurrentUser, change);
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

        // Get ajax-requested data for postpone item(by postponeResCompanyID)
        private void JSGetPostponeResCompany()
        {
            int postponeResCompanyID = int.Parse(Request.Form["PostponeResCompanyID"]);
            int preLoadCompanyID = int.Parse(Request.Form["CompanyID"]);

            PostponeResCompany p = PostponeResCompanyUtil.GetPostponeResCompany(postponeResCompanyID, CurrentUser);
            List<PostponeResItem> postponeResItems = null;

            string unifiedIdentityCode = "";
            string companyId = "";
            string companyName = "";
            string ownershipType = "";
            string administration = "";
            string employeesCnt = "";

            if (postponeResCompanyID > 0)
            {
                postponeResItems = PostponeResItemUtil.GetAllPostponeResItemsByPostponeResCompanyID(postponeResCompanyID, CurrentUser);

                unifiedIdentityCode = p.Company.UnifiedIdentityCode;
                companyId = p.Company.CompanyId.ToString();
                companyName= p.Company.CompanyName;
                ownershipType = p.Company.OwnershipType.OwnershipTypeName;
                administration = (p.Company.AdministrationId.HasValue ? p.Company.Administration.AdministrationName : "");
                employeesCnt = p.EmployeesCnt.ToString();
            }
            else
            {
                postponeResItems = PostponeResItemUtil.GetDefaultPostponeResItems(CurrentUser);

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

                employeesCnt = "";
            }

            string response = @"<response>
                                   <UnifiedIdentityCode>" + AJAXTools.EncodeForXML(unifiedIdentityCode) + @"</UnifiedIdentityCode>
                                   <CompanyID>" + AJAXTools.EncodeForXML(companyId) + @"</CompanyID>
                                   <CompanyName>" + AJAXTools.EncodeForXML(companyName) + @"</CompanyName>
                                   <OwnershipType>" + AJAXTools.EncodeForXML(ownershipType) + @"</OwnershipType>
                                   <Administration>" + AJAXTools.EncodeForXML(administration) + @"</Administration>
                                   <EmployeesCnt>" + AJAXTools.EncodeForXML(employeesCnt) + @"</EmployeesCnt>
                                ";

            string itemsXml = "<PostponeResItems>";
            foreach (PostponeResItem item in postponeResItems)
            {
                itemsXml += @"<PostponeResItem>
                                 <PostponeResItemID>" + AJAXTools.EncodeForXML(item.PostponeResItemID.ToString()) + @"</PostponeResItemID>
                                 <NKPDNickname>" + AJAXTools.EncodeForXML(item.NKPD.Nickname) + @"</NKPDNickname>
                                 <NKPDID>" + AJAXTools.EncodeForXML(item.NKPD.Id.ToString()) + @"</NKPDID>
                                 <OfficersConditioned>" + AJAXTools.EncodeForXML(item.OfficersConditioned.HasValue ? item.OfficersConditioned.Value.ToString() : "") + @"</OfficersConditioned>
                                 <OfficersAbsolutely>" + AJAXTools.EncodeForXML(item.OfficersAbsolutely.HasValue ? item.OfficersAbsolutely.Value.ToString() : "") + @"</OfficersAbsolutely>
                                 <OfCandConditioned>" + AJAXTools.EncodeForXML(item.OfCandConditioned.HasValue ? item.OfCandConditioned.Value.ToString() : "") + @"</OfCandConditioned>
                                 <OfCandAbsolutely>" + AJAXTools.EncodeForXML(item.OfCandAbsolutely.HasValue ? item.OfCandAbsolutely.Value.ToString() : "") + @"</OfCandAbsolutely>
                                 <SergeantsConditioned>" + AJAXTools.EncodeForXML(item.SergeantsConditioned.HasValue ? item.SergeantsConditioned.Value.ToString() : "") + @"</SergeantsConditioned>
                                 <SergeantsAbsolutely>" + AJAXTools.EncodeForXML(item.SergeantsAbsolutely.HasValue ? item.SergeantsAbsolutely.Value.ToString() : "") + @"</SergeantsAbsolutely>
                                 <SoldiersConditioned>" + AJAXTools.EncodeForXML(item.SoldiersConditioned.HasValue ? item.SoldiersConditioned.Value.ToString() : "") + @"</SoldiersConditioned>
                                 <SoldiersAbsolutely>" + AJAXTools.EncodeForXML(item.SoldiersAbsolutely.HasValue ? item.SoldiersAbsolutely.Value.ToString() : "") + @"</SoldiersAbsolutely>
                              </PostponeResItem>";
            }
            itemsXml += "</PostponeResItems>";

            response += itemsXml +
                        "</response>";

            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();
        }

        // Deletes postpone item by ajax request
        private void JSDeletePostponeResCompany()
        {
            if (this.GetUIItemAccessLevel("RES_POSTPONE") != UIAccessLevel.Enabled || this.GetUIItemAccessLevel("RES_POSTPONE_RES") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            int postponeYear = int.Parse(Request.Form["PostponeYear"]);
            int MilitaryDepartmentID = int.Parse(Request.Form["MilitaryDepartmentID"]);
            PostponeRes postponeRes = PostponeResUtil.GetPostponeRes(postponeYear, MilitaryDepartmentID, CurrentUser);

            int postponeResCompanyID = int.Parse(Request.Form["PostponeResCompanyID"]);

            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_PostponeRes");

                PostponeResCompanyUtil.DeletePostponeResCompany(postponeRes.PostponeResID, postponeResCompanyID, CurrentUser, change);               

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
            if (this.GetUIItemAccessLevel("RES_POSTPONE") != UIAccessLevel.Enabled || this.GetUIItemAccessLevel("RES_POSTPONE_RES") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            int militaryDepartmentID = int.Parse(Request.Form["MilitaryDepartmentID"]);
            int postponeYear = int.Parse(Request.Form["PostponeYear"]);

            string stat = "";
            string response = "";

            try
            {
                string result = "";

                result = PostponeResUtil.IsThereAnyCompanyData(militaryDepartmentID, postponeYear, CurrentUser) ? "OK" : "NO";

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

        private void JSCopyPostponeRes()
        {
            if (this.GetUIItemAccessLevel("RES_POSTPONE") != UIAccessLevel.Enabled || this.GetUIItemAccessLevel("RES_POSTPONE_RES") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            int militaryDepartmentID_Old = int.Parse(Request.Form["MilitaryDepartmentID_Old"]);
            int postponeYear_Old = int.Parse(Request.Form["PostponeYear_Old"]);
            int militaryDepartmentID_New = int.Parse(Request.Form["MilitaryDepartmentID_New"]);
            int postponeYear_New = int.Parse(Request.Form["PostponeYear_New"]);

            string stat = "";
            string response = "";

            try
            {
                Change change = new Change(CurrentUser, "RES_PostponeRes");

                PostponeRes postponeRes_Old = PostponeResUtil.GetPostponeRes(postponeYear_Old, militaryDepartmentID_Old, CurrentUser);
                PostponeRes postponeRes_New = PostponeResUtil.GetPostponeRes(postponeYear_New, militaryDepartmentID_New, CurrentUser);

                if (postponeRes_New == null)
                {
                    postponeRes_New = new PostponeRes(CurrentUser);
                    postponeRes_New.PostponeResID = 0;
                    postponeRes_New.MilitaryDepartmentID = militaryDepartmentID_New;
                    postponeRes_New.PostponeYear = postponeYear_New;
                    PostponeResUtil.SavePostponeRes(postponeRes_New, CurrentUser, change);
                }

                PostponeResUtil.CopyPostponeRes(postponeRes_Old, postponeRes_New, CurrentUser, change);

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
        protected void btnHdnRefreshPostponeResCompanies_Click(object sender, EventArgs e)
        {
            LoadPostponeResCompaniesTable();
        }

        protected void ddPostponeYear_Change(object sender, EventArgs e)
        {
            LoadPostponeResCompaniesTable();
        }

        protected void ddMilitaryDepartment_Change(object sender, EventArgs e)
        {
            LoadPostponeResCompaniesTable();
        }

        private void SetButtonsVisibility()
        {
            if (ddPostponeYear.SelectedValue != ListItems.GetOptionChooseOne().Value &&
                ddMilitaryDepartment.SelectedValue != ListItems.GetOptionChooseOne().Value)
            {
                btnNewPostponeResCompany.Visible = true;

                if (int.Parse(hdnPostponeResCompaniesCnt.Value) > 0)
                    btnCopy.Visible = true;
                else
                    btnCopy.Visible = false;
            }
            else
            {
                btnNewPostponeResCompany.Visible = false;
                btnCopy.Visible = false;
            }
        }
    }
}
