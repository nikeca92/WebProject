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
    public partial class CadetsRanking : APPLPage
    {
        //Collect the filter information to be able to pull the number of rows for this specific filter
        CadetsRankingFilter filter = null;

        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "APPL_CADETS_RANKING";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if (GetUIItemAccessLevel("APPL_CADETS_RANKING") == UIAccessLevel.Hidden
                || GetUIItemAccessLevel("APPL_CADETS") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSearchCadet")
            {
                this.JSSearchCadet();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSRankCadet")
            {
                this.JSRankCadet();
                return;
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDeclassingCadet")
            {
                this.JSDeclassingCadet();
                return;
            }

            //Hilight the current page in the menu bar
            HighlightMenuItems("Cadets", "Cadets_Ranking");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            //Setup some basic styles on the screen
            SetupStyle();

            //When the page is loaded for the first time
            if (!IsPostBack)
            {
                //Populate any drop-downs and list-boxes
                PopulateLists();

                //Simulate clicking the Refresh button to load the grid initially
                btnRefresh_Click(btnRefresh, new EventArgs());
            }

            lblMessage.Text = "";
            lblGridMessage.Text = "";
        }

        //Populate all listboxes on the screen
        private void PopulateLists()
        {
            this.PopulateMilitarySchools();
        }

        //Populate the MilitarySchools drop-down
        private void PopulateMilitarySchools()
        {
            this.ddlMilitarySchools.DataSource = MilitarySchoolUtil.GetAllMilitarySchools(CurrentUser, true);
            this.ddlMilitarySchools.DataTextField = "MilitarySchoolName";
            this.ddlMilitarySchools.DataValueField = "MilitarySchoolId";
            this.ddlMilitarySchools.DataBind();

            this.PopulateSchoolYears();
        }

        //Populate the SchoolYears drop-down
        private void PopulateSchoolYears()
        {
            this.ddlSchoolYears.DataSource = null;
            if (this.ddlMilitarySchools.SelectedValue != "")
            {
                List<int> years = MilitarySchoolSpecializationUtil.GetAllYearsByMilitarySchoolID(int.Parse(this.ddlMilitarySchools.SelectedValue), CurrentUser);

                this.ddlSchoolYears.DataSource = years;
                this.ddlSchoolYears.DataBind();

                this.PopulateSubjects();
            }
            else
            {
                this.ddlSchoolYears.Items.Clear();
                this.ddlSubjects.Items.Clear();
                this.ddlSpecializations.Items.Clear();
            }
        }

        //Populate the Subjects drop-down
        private void PopulateSubjects()
        {
            this.ddlSubjects.DataSource = null;
            if (this.ddlMilitarySchools.SelectedValue != "" && this.ddlSchoolYears.SelectedValue != "")
            {
                int militarySchoolId = int.Parse(this.ddlMilitarySchools.SelectedValue);
                int year = int.Parse(this.ddlSchoolYears.SelectedValue);

                this.ddlSubjects.DataSource = MilitarySchoolSubjectUtil.GetAllMilitarySchoolSubjectsByMilitarySchoolID(militarySchoolId, year, CurrentUser);
                this.ddlSubjects.DataTextField = "MilitarySchoolSubjectName";
                this.ddlSubjects.DataValueField = "MilitarySchoolSubjectId";
                this.ddlSubjects.DataBind();

                this.PopulateSpecializations();
            }
            else
            {
                this.ddlSubjects.Items.Clear();
                this.ddlSpecializations.Items.Clear();
            }
        }

        //Populate the Specializations drop-down
        private void PopulateSpecializations()
        {
            this.ddlSchoolYears.DataSource = null;
            if (this.ddlMilitarySchools.SelectedValue != "" && this.ddlSchoolYears.SelectedValue != "" 
                && this.ddlSubjects.SelectedValue != "")
            {
                int militarySchoolId = int.Parse(this.ddlMilitarySchools.SelectedValue);
                int year = int.Parse(this.ddlSchoolYears.SelectedValue);
                int subjectId = int.Parse(this.ddlSubjects.SelectedValue);

                this.ddlSpecializations.DataSource = SpecializationUtil.GetAllSpecsByMilitarySchoolSubjectID(militarySchoolId, year, subjectId, CurrentUser);
                this.ddlSpecializations.DataTextField = "SpecializationName";
                this.ddlSpecializations.DataValueField = "SpecializationId";
                this.ddlSpecializations.DataBind();
            }
            this.ddlSpecializations.DataBind();
        }

        //Setup some styling on the page
        private void SetupStyle()
        {

        }

        //Search a cadet (ajax call)
        private void JSSearchCadet()
        {
            string stat = "";
            string response = "";

            try
            {
                string identityNumber = "";
                if (!String.IsNullOrEmpty(Request.Form["IdentityNumber"]))
                {
                    identityNumber = Request.Form["IdentityNumber"].ToString();
                }

                int militarySchoolId = 0;
                if (!String.IsNullOrEmpty(Request.Form["MilitarySchoolId"]))
                {
                    int.TryParse(Request.Form["MilitarySchoolId"], out militarySchoolId);
                }

                int schoolYear = 0;
                if (!String.IsNullOrEmpty(Request.Form["Year"]))
                {
                    int.TryParse(Request.Form["Year"], out schoolYear);
                }

                int specializationId = 0;
                if (!String.IsNullOrEmpty(Request.Form["SpecializationId"]))
                {
                    int.TryParse(Request.Form["SpecializationId"], out specializationId);
                }

                if (String.IsNullOrEmpty(identityNumber) || militarySchoolId == 0
                    || schoolYear == 0 || specializationId == 0)
                {
                    throw new Exception("Липсват някой от опциите за търсене на кандидат курсанти");
                }

                CadetSchoolSubjectFilter cadetSchoolSubjectFilter = new CadetSchoolSubjectFilter()
                {
                    MilitarySchoolId = militarySchoolId,
                    SchoolYear = schoolYear,
                    SpecializationId = specializationId,
                    IdentityNumber = identityNumber
                };

                CadetSchoolSubject cadetSchoolSubject = CadetSchoolSubjectUtil.GetCadetSchoolSubjectByFilter(cadetSchoolSubjectFilter, CurrentUser);

                stat = AJAXTools.OK;

                if (cadetSchoolSubject == null)
                {
                    response = "<response>" + AJAXTools.ERROR + "</response>";
                    response += "<responseMsg>(Няма намерен кандидат курсант по зададеното ЕГН)</responseMsg>";
                }
                else
                {
                    if (cadetSchoolSubject.IsRanked)
                    {
                        response = "<response>" + AJAXTools.ERROR + "</response>";
                        response += "<responseMsg>" + cadetSchoolSubject.Cadet.Person.FullName + " " + "(Търсеният кандидат курсант е вече класиран)</responseMsg>";
                    }
                    else
                    {
                        response = "<response>" + AJAXTools.OK + "</response>";
                        response += "<responseMsg>" + cadetSchoolSubject.Cadet.Person.FullName + "</responseMsg>";
                    }
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

        //Rank a cadet (ajax call)
        private void JSRankCadet()
        {
            if (GetUIItemAccessLevel("APPL_CADETS") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("APPL_CADETS_RANKING") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("APPL_CADETS_EDITCADET") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                string identityNumber = "";
                if (!String.IsNullOrEmpty(Request.Form["IdentityNumber"]))
                {
                    identityNumber = Request.Form["IdentityNumber"].ToString();
                }

                int militarySchoolId = 0;
                if (!String.IsNullOrEmpty(Request.Form["MilitarySchoolId"]))
                {
                    int.TryParse(Request.Form["MilitarySchoolId"], out militarySchoolId);
                }

                int schoolYear = 0;
                if (!String.IsNullOrEmpty(Request.Form["Year"]))
                {
                    int.TryParse(Request.Form["Year"], out schoolYear);
                }

                int specializationId = 0;
                if (!String.IsNullOrEmpty(Request.Form["SpecializationId"]))
                {
                    int.TryParse(Request.Form["SpecializationId"], out specializationId);
                }

                if (String.IsNullOrEmpty(identityNumber) || militarySchoolId == 0
                    || schoolYear == 0 || specializationId == 0)
                {
                    throw new Exception("Някой от опциите за класиране са невалидни");
                }

                CadetSchoolSubjectFilter cadetSchoolSubjectFilter = new CadetSchoolSubjectFilter()
                {
                    MilitarySchoolId = militarySchoolId,
                    SchoolYear = schoolYear,
                    SpecializationId = specializationId,
                    IdentityNumber = identityNumber
                };

                Change change = new Change(CurrentUser, "APPL_Cadets");

                CadetSchoolSubject cadetSchoolSubject = CadetSchoolSubjectUtil.GetCadetSchoolSubjectByFilter(cadetSchoolSubjectFilter, CurrentUser);

                stat = AJAXTools.OK;

                if (cadetSchoolSubject == null)
                {
                    response = "<response>" + AJAXTools.ERROR + "</response>";
                    response += "<responseMsg>(Няма намерен кандидат курсант по зададеното ЕГН)</responseMsg>";
                }
                else
                {
                    if (cadetSchoolSubject.IsRanked)
                    {
                        response = "<response>" + AJAXTools.ERROR + "</response>";
                        response += "<responseMsg>" + cadetSchoolSubject.Cadet.Person.FullName + " " + "(Търсеният кандидат курсант е вече класиран)</responseMsg>";
                    }
                    else
                    {
                        cadetSchoolSubject.IsRanked = true;
                        CadetSchoolSubjectUtil.SaveCadetSchoolSubject(cadetSchoolSubject, CurrentUser, change);
                        response = "<response>OK</response>";
                    }
                }

                change.WriteLog();
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

        //Cadet declassing (ajax call)
        private void JSDeclassingCadet()
        {
            if (GetUIItemAccessLevel("APPL_CADETS") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("APPL_CADETS_RANKING") != UIAccessLevel.Enabled ||
                GetUIItemAccessLevel("APPL_CADETS_EDITCADET") != UIAccessLevel.Enabled)
                RedirectAjaxAccessDenied();

            string stat = "";
            string response = "";

            try
            {
                int cadetSchoolSubjectId = 0;
                if (!String.IsNullOrEmpty(Request.Form["CadetSchoolSubjectId"]))
                {
                    int.TryParse(Request.Form["CadetSchoolSubjectId"], out cadetSchoolSubjectId);
                }

                Change change = new Change(CurrentUser, "APPL_Cadets");

                CadetSchoolSubject cadetSchoolSubject = CadetSchoolSubjectUtil.GetCadetSchoolSubject(cadetSchoolSubjectId, CurrentUser);

                if (cadetSchoolSubject == null)
                {
                    throw new Exception("Операцията е не успешна");
                }

                cadetSchoolSubject.IsRanked = false;
                CadetSchoolSubjectUtil.SaveCadetSchoolSubject(cadetSchoolSubject, CurrentUser, change);

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

        //Validate some field on the screen
        private bool ValidateData()
        {
            bool isDataValid = true;
            string errMsg = "";

            filter = CollectFilterData();

            if (filter.MilitarySchoolId == 0 || filter.SchoolYear == 0 || filter.SpecializationId == 0)
            {
                isDataValid = false;
                errMsg = "Липсват някой от опциите за търсене на класирани курсанти";
            }

            if (!isDataValid)
            {
                this.lblMessage.CssClass = "ErrorText";
                this.lblMessage.Text = errMsg;
            }

            return isDataValid;
        }

        //Refresh the data grid
        private void RefreshCadets()
        {
            string html = "";

            //Get the list of records according to the specified filters, order and paging
            List<CadetRankingSearch> cadetRankingSearches = CadetRankingSearchUtil.GetAllCadetsRankingSearch(filter, CurrentUser);

            //No data found
            if (cadetRankingSearches.Count == 0)
            {
                html = "<span>Няма намерени резултати</span>";
            }
            //If there is data then generate dynamically the HTML for the data grid
            else
            {
                string headerStyle = "vertical-align: bottom;";

                //Setup the header of the grid
                html = @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='width: 100px; " + headerStyle + @"'>ЕГН</th>
                               <th style='width: 300px; " + headerStyle + @"'>Трите имена</th>
                               <th style='width: 20px;" + headerStyle + @"'>&nbsp;</th>
                            </tr>
                         </thead>";

                int counter = 1;

                //Iterate through all items and add them into the grid
                foreach (CadetRankingSearch cadetRankingSearch in cadetRankingSearches)
                {
                    string cellStyle = "vertical-align: top;";

                    string deleteHTML = "";

                    if (GetUIItemAccessLevel("APPL_CADETS") == UIAccessLevel.Enabled &&
                        GetUIItemAccessLevel("APPL_CADETS_RANKING") == UIAccessLevel.Enabled &&
                        GetUIItemAccessLevel("APPL_CADETS_EDITCADET") == UIAccessLevel.Enabled)
                    {
                        deleteHTML = "<img src='../Images/delete.png' alt='Декласиране' title='Декласиране на този курсант' class='GridActionIcon' onclick='DeclassingCadet(" + cadetRankingSearch.CadetSchoolSubject.CadetSchoolSubjectId.ToString() + ");' />";
                    }

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td align='center' style='" + cellStyle + @"'>" + counter + @"</td>
                                 <td style='" + cellStyle + @"'>" + cadetRankingSearch.PersonIdentNumber + @"</td>
                                 <td style='" + cellStyle + @"'>" + cadetRankingSearch.PersonName + @"</td>
                                 <td align='center' style='" + cellStyle + @"'>" + deleteHTML + @"</td>
                              </tr>";

                    counter++;
                }

                html += "</table>";
            }

            //Put the generated grid on the page
            pnlCadetsGrid.InnerHtml = html;

            //Set the message if there is a need (e.g. a deleted item)
            if (hdnRefreshReason.Value != "")
            {
                if (hdnRefreshReason.Value == "RANKED")
                {
                    lblGridMessage.Text = "Курсантът беше класиран успешно";
                    lblGridMessage.CssClass = "SuccessText";
                }
                else if (hdnRefreshReason.Value == "DECLASSED")
                {
                    lblGridMessage.Text = "Курсантът беше декласиран успешно";
                    lblGridMessage.CssClass = "SuccessText";
                }

                hdnRefreshReason.Value = "";
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ContentPages/Home.aspx");
        }  

        //Refresh the data grid
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                RefreshCadets();
            }
        }

        //Collect the filet information from the page
        private CadetsRankingFilter CollectFilterData()
        {
            CadetsRankingFilter filter = new CadetsRankingFilter();

            if (this.ddlMilitarySchools.SelectedValue != "")
            {
                filter.MilitarySchoolId = int.Parse(this.ddlMilitarySchools.SelectedValue);
                this.hfMilitarySchoolId.Value = this.ddlMilitarySchools.SelectedValue;
            }

            if (this.ddlSchoolYears.SelectedValue != "")
            {
                filter.SchoolYear = int.Parse(this.ddlSchoolYears.SelectedValue);
                this.hfSchoolYear.Value = this.ddlSchoolYears.SelectedValue;
            }

            if (this.ddlSpecializations.SelectedValue != "")
            {
                filter.SpecializationId = int.Parse(this.ddlSpecializations.SelectedValue);
                this.hfSpecializationId.Value = this.ddlSpecializations.SelectedValue;
            }

            return filter;
        }

        protected void ddlMilitarySchools_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.PopulateSchoolYears();
        }

        protected void ddlSchoolYears_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.PopulateSubjects();
        }

        protected void ddlSubjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.PopulateSpecializations();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ddlMilitarySchools.SelectedIndex = 0;
            ddlMilitarySchools_SelectedIndexChanged(sender, e);
        }
    }
}