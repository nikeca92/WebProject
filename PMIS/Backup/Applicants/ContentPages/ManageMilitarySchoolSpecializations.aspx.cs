using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;

using PMIS.Common;
using PMIS.Applicants.Common;
using System.Collections.Generic;

namespace PMIS.Applicants.ContentPages
{
    public partial class ManageMilitarySchoolSpecializations : APPLPage
    {
        //Get the config setting that says how many rows per page should be dispayed in the grid
        private int pageLength = int.Parse(Config.GetWebSetting("RowsPerPage"));

        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "APPL_CADETS_MILITSCHOOlSPECIALIZATION";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if (GetUIItemAccessLevel("APPL_CADETS") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("APPL_CADETS_MILITSCHOOlSPECIALIZATION") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            this.SetBtnAddSpecialization();

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetUnusedSpecializationItems")
            {
                this.JSGetAllAvailableSpecializations();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSAddSpecializationToMilitarySchool")
            {
                this.JSAddSpecializationToMilitarySchool();
                return;
            }

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRefreshMilitarySchoolSpecializations")
            {
                this.JSRefreshMilitarySchoolSpecializations();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeleteMilitarySchoolSpecialization")
            {
                this.JSDeleteMilitarySchoolSpecialization();
                return;
            }

            //Hilight the current page in the menu bar
            HighlightMenuItems("Specializations", "Specializations_ManageMilitarySchoolSpecializations");

            //When the page is loaded for the first time
            if (!IsPostBack)
            {
                //Populate drop-downs
                PopulateLists();

                //The default order is by module name
                if (string.IsNullOrEmpty(hdnSortBy.Value))
                    hdnSortBy.Value = "1";

                //Simulate clicking the Refresh button to load the grid initially
                btnRefresh_Click(btnRefresh, new EventArgs());
            }
        }

        private void SetBtnAddSpecialization()
        {
            if (this.GetUIItemAccessLevel("APPL_CADETS") == UIAccessLevel.Enabled
                && this.GetUIItemAccessLevel("APPL_CADETS_MILITSCHOOlSPECIALIZATION") == UIAccessLevel.Enabled)
            {
                EnableButton(this.btnAddSpecialization);
            }
            else
            {
                if (this.GetUIItemAccessLevel("APPL_CADETS_MILITSCHOOlSPECIALIZATION") == UIAccessLevel.Hidden)
                {
                    HideControl(this.btnAddSpecialization);
                }
                else
                {
                    DisableButton(this.btnAddSpecialization);
                }
            }
        }

        // Get all available specializations
        public void JSGetAllAvailableSpecializations()
        {
            string response = "";
            string status = "";

            int militarySchoolId = 0;
            Int32.TryParse(Request.Params["MilitarySchoolID"], out militarySchoolId);

            int year = 0;
            Int32.TryParse(Request.Params["Year"], out year);

            if (militarySchoolId != 0 && year != 0)
            {
                try
                {
                    response += "<response>" + AJAXTools.EncodeForXML(this.GenerateSpecializationsLightBoxContent(militarySchoolId, year)) + "</response>";
                    status = AJAXTools.OK;
                }
                catch
                {
                    status += AJAXTools.ERROR;
                }
            }

            AJAX a = new AJAX(response, status, Response);
            a.Write();
            Response.End();
        }

        // Add selected specialization to contextual military school and year
        public void JSAddSpecializationToMilitarySchool()
        {
            if (GetUIItemAccessLevel("APPL_CADETS") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("APPL_CADETS_MILITSCHOOlSPECIALIZATION") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            string response = "";
            string status = "";

            int militarySchoolId = 0;
            Int32.TryParse(Request.Params["MilitarySchoolID"], out militarySchoolId);

            int year = 0;
            Int32.TryParse(Request.Params["Year"], out year);

            int specializationId = 0;
            Int32.TryParse(Request.Params["SpecializationID"], out specializationId);

            if (militarySchoolId != 0 && year != 0 && specializationId != 0)
            {
                try
                {
                    MilitarySchoolSpecialization militSchoolSpec = new MilitarySchoolSpecialization(CurrentUser)
                    {
                        MilitarySchoolId = militarySchoolId,
                        Year = year,
                        Specialization = new Specialization() 
                        {
                            SpecializationId = specializationId
                        }
                    };

                    //Track the changes into the Audit Trail 
                    Change change = new Change(CurrentUser, "APPL_MilitSchoolSpecializations");

                    MilitarySchoolSpecializationUtil.SaveMilitarySchoolSpecialization(militSchoolSpec, CurrentUser, change);

                    change.WriteLog();

                    response += "<response>" + AJAXTools.EncodeForXML(AJAXTools.OK) + "</response>";
                    status = AJAXTools.OK;
                }
                catch
                {
                    status += AJAXTools.ERROR;
                }
            }

            AJAX a = new AJAX(response, status, Response);
            a.Write();
            Response.End();
        }

        // Get all military school specializations
        public void JSRefreshMilitarySchoolSpecializations()
        {
            string response = "";
            string status = "";

            int militarySchoolId = 0;
            Int32.TryParse(Request.Params["MilitarySchoolID"], out militarySchoolId);

            int year = 0;
            Int32.TryParse(Request.Params["Year"], out year);

            if (militarySchoolId != 0 && year != 0)
            {
                try
                {
                    int orderBy = 1;
                    if (!String.IsNullOrEmpty(Request.Params["OrderBy"]))
                    {
                        Int32.TryParse(Request.Params["OrderBy"], out orderBy);   
                    }

                    response += "<response>" + AJAXTools.EncodeForXML(this.GenerateMilitarySchoolSpecializations(militarySchoolId, year, orderBy)) + "</response>";
                    status = AJAXTools.OK;
                }
                catch
                {
                    status += AJAXTools.ERROR;
                }
            }

            AJAX a = new AJAX(response, status, Response);
            a.Write();
            Response.End();
        }

        //Delete a record (ajax call)
        private void JSDeleteMilitarySchoolSpecialization()
        {
            if (GetUIItemAccessLevel("APPL_CADETS") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("APPL_CADETS_MILITSCHOOlSPECIALIZATION") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int militarySchoolSpecializationId = int.Parse(Request.Form["MilitarySchoolSpecializationID"]);

                Change change = new Change(CurrentUser, "APPL_MilitSchoolSpecializations");

                MilitarySchoolSpecializationUtil.DeleteMilitarySchoolSpecialization(militarySchoolSpecializationId, CurrentUser, change);

                change.WriteLog();

                stat = AJAXTools.OK;
                response = "<response>" + AJAXTools.OK + "</response>";
            }
            catch
            {
                stat = AJAXTools.ERROR;
            }


            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        //Populate all listboxes on the screen
        private void PopulateLists()
        {
            this.PopulateMilitarySchools();
            this.PopulateSchoolYears();
        }

        //Populate the MilitarySchools drop-down
        private void PopulateMilitarySchools()
        {
            this.ddlMilitarySchools.DataSource = MilitarySchoolUtil.GetAllMilitarySchools(CurrentUser, true);
            this.ddlMilitarySchools.DataTextField = "MilitarySchoolName";
            this.ddlMilitarySchools.DataValueField = "MilitarySchoolId";
            this.ddlMilitarySchools.DataBind();
        }

        //Populate the SchoolYears drop-down
        private void PopulateSchoolYears()
        {
            this.ddlSchoolYears.DataSource = null;
            int selectedMilitSchoolId = 0;
            if (Int32.TryParse(this.ddlMilitarySchools.SelectedValue, out selectedMilitSchoolId)
                && selectedMilitSchoolId > 0)
            {
                List<int> years = MilitarySchoolSpecializationUtil.GetAllYearsByMilitarySchoolID(selectedMilitSchoolId, CurrentUser);

                int currentYear = DateTime.Now.Year;
                if (!years.Contains(currentYear))
                {
                    years.Add(currentYear);
                }

                int nextYear = currentYear + 1;
                if (!years.Contains(nextYear))
                {
                    years.Add(nextYear);
                }

                years.Sort();

                this.ddlSchoolYears.DataSource = years;
                this.ddlSchoolYears.DataBind();
            }           
        }

        //Refresh the data grid
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            this.RefreshSearchResultGrid();
        }

        private void RefreshSearchResultGrid()
        {
            int militarySchoolId = 0;
            Int32.TryParse(this.ddlMilitarySchools.SelectedValue, out militarySchoolId);

            int year = 0;
            Int32.TryParse(this.ddlSchoolYears.SelectedValue, out year);

            int orderBy = int.Parse(this.hdnSortBy.Value);
            this.divMilitarySchoolSpecializationsGrid.InnerHtml = this.GenerateMilitarySchoolSpecializations(militarySchoolId, year, orderBy);
        }

        //Generate the specializations data grid
        public string GenerateMilitarySchoolSpecializations(int militarySchoolId, int year, int orderBy)
        {
            MilitarySchoolSpecializationFilter filter = new MilitarySchoolSpecializationFilter() 
            { 
                MilitarySchoolId = militarySchoolId,
                Year = year,
                OrderBy = orderBy
            };

            // Get the list of military school specialization items
            List<MilitarySchoolSpecialization> militSchoolSpecializations = MilitarySchoolSpecializationUtil.GetAllMilitarySchoolSpecializationsByFilter(filter, null, CurrentUser);
            
            string html = "";

            // No data found    
            if (militSchoolSpecializations.Count == 0)
            {
                html = "<span>Няма намерени резултати</span>";
            }
            // If there is data then generate dynamically the HTML for the data grid
            else
            {
                string headerStyle = "vertical-align: bottom;";
                int orderCol = (orderBy > 100 ? orderBy - 100 : orderBy);
                string img = orderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
                string[] arrOrderCol = { "", "", "", "", "", "" };
                arrOrderCol[orderCol - 1] = @"<div style='position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

                // Setup the header of the grid
                html = @"
                        <table id='UnusedSpecializationsTable' class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='vertical-align: middle; width: 350px; cursor: pointer;" + headerStyle + @"' onclick='SortMilitarySchoolSpecializationsTableBy(1);'>Специалност" + arrOrderCol[0] + @"</th>
                               <th style='vertical-align: middle; width: 350px; cursor: pointer;" + headerStyle + @"' onclick='SortMilitarySchoolSpecializationsTableBy(2);'>Специализация" + arrOrderCol[1] + @"</th>
                               <th style='width: 30px;" + headerStyle + @"'>&nbsp;</th>
                            </tr>
                         </thead>";

                int counter = 1;

                //Iterate through all items and add them into the grid
                foreach (MilitarySchoolSpecialization militSchoolSpec in militSchoolSpecializations)
                {
                    string cellStyle = "vertical-align: top;";

                    string deleteHTML = "";

                    if (militSchoolSpec.CanDelete && GetUIItemAccessLevel("APPL_CADETS") == UIAccessLevel.Enabled &&
                            GetUIItemAccessLevel("APPL_CADETS_MILITSCHOOlSPECIALIZATION") == UIAccessLevel.Enabled)
                    {
                        deleteHTML = "<img src='../Images/delete.png' alt='Изтриване' title='Изтриване на тази специализация' class='GridActionIcon' onclick='DeleteMilitarySchoolSpecialization(" + militSchoolSpec.MilitarySchoolSpecializationId.ToString() + ");' />";
                    }

                    html += @"<tr style='min-height: 17px;' class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @");'>
                                 <td align='center' style='" + cellStyle + @"'>" + counter.ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + militSchoolSpec.Specialization.MilitarySchoolSubject.MilitarySchoolSubjectName + @"</td>
                                 <td style='" + cellStyle + @"'>" + militSchoolSpec.Specialization.SpecializationName + @"</td>
                                 <td align='center' style='" + cellStyle + @"'>" + deleteHTML + @"</td>
                              </tr>";

                    counter++;
                }

                html += "</table><br />";
            }

            return html;
        }

        //Generate the specializations data grid
        public string GenerateSpecializationsLightBoxContent(int militarySchoolId, int year)
        {
            string subjectName = Request.Params["SubjectName"];
            string specializationName = Request.Params["SpecializationName"];

            string orderByStr = Request.Params["SpecTableOrderBy"];
            string pageIdxStr = Request.Params["SpecTablePageIdx"];

            bool isPaging = false;
            int isPagingPar = 0;
            int.TryParse(Request.Params["IsPaging"], out isPagingPar);
            if (isPagingPar == 1)
            {
                isPaging = true;
            }

            // Get the config setting that says how many rows per page should be dispayed in the grid
            int pageLength = int.Parse(Config.GetWebSetting("RowsPerPage"));
            // Stores information about how many pages are in the grid
            int maxPage;

            SpecializationFilter filter = new SpecializationFilter()
            {
                MilitarySchoolId = militarySchoolId,
                Year = year,
                SubjectName = subjectName,
                SpecializationName = specializationName
            };

            int allRows = SpecializationUtil.GetAllUnusedSpecsByMilitarySchoolIDCount(filter, CurrentUser);
            // Get the number of rows and calculate the number of pages in the grid
            maxPage = pageLength == 0 ? 1 : allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

            string html = "";

            // Collect order control and the paging control data from the page
            int orderBy = 1;
            if (!String.IsNullOrEmpty(orderByStr))
            {
                int.TryParse(orderByStr, out orderBy);
            }

            filter.OrderBy = orderBy;

            int pageIdx = 1;
            if (!String.IsNullOrEmpty(pageIdxStr) && String.IsNullOrEmpty(subjectName) 
                && String.IsNullOrEmpty(specializationName) || isPaging)
            {
                int.TryParse(pageIdxStr, out pageIdx);   
            }

            filter.PageIdx = pageIdx;

            // Get the list of specialization items according to the specified order and paging
            List<Specialization> specializations = SpecializationUtil.GetAllUnusedSpecsByMilitarySchoolID(filter, pageLength, CurrentUser);


            // If there is data then generate dynamically the HTML for the data grid
            string headerStyle = "vertical-align: bottom;";
            int orderCol = (orderBy > 100 ? orderBy - 100 : orderBy);
            string img = orderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
            string[] arrOrderCol = { "", "" };
            arrOrderCol[orderCol - 1] = @"<div style='position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

            // Refresh the paging image buttons
            string btnFirst = "src='../Images/ButtonFirst.png'";
            string btnPrev = "src='../Images/ButtonPrev.png'";
            string btnLast = "src='../Images/ButtonLast.png'";
            string btnNext = "src='../Images/ButtonNext.png'";

            if (pageIdx == 1)
            {
                btnFirst = "src='../Images/ButtonFirstDisabled.png' disabled='true'";
                btnPrev = "src='../Images/ButtonPrevDisabled.png' disabled='true'";
            }

            if (pageIdx == maxPage)
            {
                btnLast = "src='../Images/ButtonLastDisabled.png' disabled='true'";
                btnNext = "src='../Images/ButtonNextDisabled.png' disabled='true'";
            }

            // Set current page number
            string specializationsTablePagination = " | " + pageIdx + " от " + maxPage + " | ";

            // Setup the header of the grid
            html = @"<div style='min-height: 150px; margin-bottom: 10px;'>
                    <input type='hidden' id='hdnSpecializationsTableOrderBy' value='" + orderBy + @"' />
                    <input type='hidden' id='hdnSpecializationsTablePageIdx' value='" + pageIdx + @"' />
                    <input type='hidden' id='hdnSpecializationsTableMaxPage' value='" + maxPage + @"' />

                    <span class='HeaderText'>Специалности и специализации</span><br /><br /><br />

                    <div style='text-align: center;'>
                       <div style='display: inline; position: relative; top: -10px;'>
                          <img id='btnSpecializationsTableFirst' " + btnFirst + @" alt='Първа страница' title='Първа страница' class='PaginationButton' onclick=""BtnSpecializationsTableFirstClick();"" />
                          <img id='btnSpecializationsTablePrev' " + btnPrev + @" alt='Предишна страница' title='Предишна страница' class='PaginationButton' onclick=""BtnSpecializationsTablePrevClick();"" />
                          <span id='lblSpecializationsTablePagination' class='PaginationLabel'>" + specializationsTablePagination + @"</span>
                          <img id='btnSpecializationsTableNext' " + btnLast + @" alt='Следваща страница' title='Следваща страница' class='PaginationButton' onclick=""BtnSpecializationsTableNextClick();"" />
                          <img id='btnSpecializationsTableLast' " + btnNext + @" alt='Последна страница' title='Последна страница' class='PaginationButton' onclick=""BtnSpecializationsTableLastClick();"" />
                          
                          <span style='padding: 0 30px'>&nbsp;</span>
                          <span style='text-align: right;'>Отиди на страница</span>
                          <input id='txtSpecializationsTableGotoPage' class='InputField' type='text' style='width: 30px;' value='' />
                          <img id='btnSpecializationsTableGoto' src='../Images/ButtonGoto.png' alt='Отиди на страница' title='Отиди на страница' class='PaginationButton' onclick=""BtnSpecializationsTableGotoClick();"" />
                       </div>
                    </div>

                    <table width='100%'>
                        <tr>
                            <td>
                                <span class='InputLebel'>Специалност</span>
                                <input id='txtLightBoxSubject' type='text' class='InputField' value='" + (String.IsNullOrEmpty(subjectName) ? "" : subjectName) + @"' />
                            </td>
                            <td>
                                <span class='InputLebel'>Специализация</span>
                                <input id='txtLightBoxSpecialization' type='text' class='InputField' value='" + (String.IsNullOrEmpty(specializationName) ? "" : specializationName) + @"' />
                            </td>
                            <td>
                                <div id='btnSearchSpecializations' runat='server' class='Button' onclick='GetSpecializationItems(1, 1);'><i></i><div style='width:70px; padding-left:5px;'>Покажи</div><b></b></div>
                            </td>
                        </tr>
                    </table>";

            // No data found
            if (specializations.Count == 0)
            {
                html += "<div style='width: 730px; padding-top: 50px; padding-bottom: 50px;'>Няма намерени резултати</div>";
            }
            else
            {
                html += @"<table id='UnusedSpecializationsTable' class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                     <thead>
                        <tr>
                           <th style='width: 20px;" + headerStyle + @"'>№</th>
                           <th style='vertical-align: middle; width: 350px; cursor: pointer;" + headerStyle + @"' onclick='SortSpecializationsTableBy(1);'>Специалност" + arrOrderCol[0] + @"</th>
                           <th style='vertical-align: middle; width: 350px; cursor: pointer;" + headerStyle + @"' onclick='SortSpecializationsTableBy(2);'>Специализация" + arrOrderCol[1] + @"</th>
                        </tr>
                     </thead>";

                int counter = 1;

                //Iterate through all items and add them into the grid
                foreach (Specialization specialization in specializations)
                {
                    string cellStyle = "vertical-align: top;";

                    html += @"<tr style='min-height: 17px; cursor: pointer;' class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"' onclick='AddSpecializationToMilitarySchool(" + specialization.SpecializationId.ToString() + @");'>
                             <td align='center' style='" + cellStyle + @"'>" + ((pageIdx - 1) * pageLength + counter).ToString() + @"</td>
                             <td style='" + cellStyle + @"'>" + specialization.MilitarySchoolSubject.MilitarySchoolSubjectName + @"</td>
                             <td style='" + cellStyle + @"'>" + specialization.SpecializationName + @"</td>
                          </tr>";

                    counter++;
                }

                html += "</table><br />";
            }

            html += @"</div>
                      <span id='spSpecializationsLightBoxMessage' style='display: none'></span><br />
                      <div id='btnCloseSpecializationsLightBox' runat='server' class='Button' onclick='HideSpecializationsLightBox();'><i></i><div style='width:70px; padding-left:5px;'>Затвори</div><b></b></div>";

            return html;
        }

        //Navigate back to the home screen
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ContentPages/Home.aspx");
        }

        protected void ddlMilitarySchools_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.PopulateSchoolYears();
            this.RefreshSearchResultGrid();
        }
    }
}
