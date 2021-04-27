using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using PMIS.Common;
using PMIS.Applicants.Common;

namespace PMIS.Applicants.ContentPages
{
    public partial class ReportVacancyAnnounceApplicants : APPLPage
    {
        public string MilitaryUnitLabel = CommonFunctions.GetLabelText("MilitaryUnit");
         
        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "APPL_REPORTS_REPORT_VACANCY_ANNOUNCE_APPLICANTS";
            }
        }

        UIAccessLevel l;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if (GetUIItemAccessLevel("APPL_REPORTS_REPORT_VACANCY_ANNOUNCE_APPLICANTS") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            //Hilight the current page in the menu bar
            HighlightMenuItems("Reports", "ReportVacancyAnnounceApplicants");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetListMilitaryUnitsForVacAnn")
            {
                int vacancyAnnounceId = int.Parse(Request.Params["vacancyAnnounceId"]);
                JSGetListMilitaryUnits(vacancyAnnounceId);
            }

            //When the page is loaded for the first time
            if (!IsPostBack)
            {
                //Populate any drop-downs and list-boxes
                //Populate any drop-downs and list-boxes
                PopulateVacancyAnnounces();
                PopulateResponsibleMilitaryUnits();

                this.btnExcel.Visible = false;
            }
           
            lblMessage.Text = "";
            lblGridMessage.Text = "";
        }

        private void PopulateVacancyAnnounces()
        {
            this.ddVacancyAnnounces.DataSource = VacancyAnnounceUtil.GetVacancyAnnouncesListItemsForReports("APPL_REPORTS_REPORT_VACANCY_ANNOUNCE_APPLICANTS", true, CurrentUser);
            this.ddVacancyAnnounces.DataTextField = "Text";
            this.ddVacancyAnnounces.DataValueField = "Value";
            this.ddVacancyAnnounces.DataBind();
            this.ddVacancyAnnounces.Items.Insert(0, ListItems.GetOptionChooseOne());
        }

        private void PopulateResponsibleMilitaryUnits()
        {
            int vacancyAnnounceId = int.Parse(ddVacancyAnnounces.SelectedValue);
            var responsibleMilitaryUnits = ReportVacAnnApplListParticipateBlockUtil.GetListMilitaryUnitsForVacAnn(vacancyAnnounceId, CurrentUser);

            if (responsibleMilitaryUnits.Count == 0)
                responsibleMilitaryUnits.Add(new MilitaryUnitForVacAnn(CurrentUser) {MilitaryUnitId = -1, MilitaryUnitName = "" });           

            this.ddResponsibleMilitaryUnits.DataSource = responsibleMilitaryUnits;
            this.ddResponsibleMilitaryUnits.DataTextField = "MilitaryUnitName";
            this.ddResponsibleMilitaryUnits.DataValueField = "MilitaryUnitId";
            this.ddResponsibleMilitaryUnits.DataBind();            
        }

        protected void ddVacancyAnnounces_Changed(object sender, EventArgs e)
        {
            PopulateResponsibleMilitaryUnits();
        }

        private void JSGetListMilitaryUnits(int vacancyAnnounceId)
        {

            string response = " <select id='ddMilitaryUnitsForVacAnn' style='width: 240px' onchange='ddMilitaryUnitsForVacAnnChange(this)'>";
            // response += "<option value='-1'></option>";
            List<MilitaryUnitForVacAnn> listMilitaryUnitsForVacAnn = ReportVacAnnApplListParticipateBlockUtil.GetListMilitaryUnitsForVacAnn(vacancyAnnounceId, CurrentUser);

            for (int i = 0; i <= listMilitaryUnitsForVacAnn.Count - 1; i++)
            {
                string selected = "";
                if (i == 0)
                {
                    selected = "selected";
                }
                response += "<option " + selected + @" value='" + listMilitaryUnitsForVacAnn[i].MilitaryUnitId + @"'>";
                response += listMilitaryUnitsForVacAnn[i].MilitaryUnitName;
                response += "</option>";
            }

            response += "</select>";

            AJAX a = new AJAX(CommonFunctions.HtmlEncoding(response), this.Response);
            a.Write();
            Response.End();
        }
                    

        //Validate some field on the screen
        private bool ValidateData()
        {
            bool isDataValid = true;
            return isDataValid;
        }

        //Refresh the data grid
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                RefreshReports();
            }
        }

        //Navigate back to the home screen
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ContentPages/Home.aspx");
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ddVacancyAnnounces.SelectedValue = ListItems.GetOptionAll().Value;
            PopulateResponsibleMilitaryUnits();
        }
        
        //Refresh the data grid
        private void RefreshReports()
        {
            string html = "";

            
            //Collect the filter information to be able to pull the number of rows for this specific filter
            ReportVacancyAnnounceApplicantsFilter filter = CollectFilterData();

            //Get the list of records according to the specified filters and paging
            List<ReportVacancyAnnounceApplicantsBlock> listBlocks = ReportVacancyAnnounceApplicantsUtil.GetReportVacancyAnnounceApplicantsSearch(filter, CurrentUser);

            //No data found
            if (listBlocks.Count == 0)
            {
                this.btnExcel.Visible = false;
                html = "<span>Няма намерени резултати</span>";                
            }
            //If there is data then generate dynamically the HTML for the data grid
            else
            {
                this.btnExcel.Visible = true;
                string headerStyle = "vertical-align: bottom;";

                //Setup the header of the grid
                html = @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                     <thead>
                        <tr>
                           <th style='width: 320px;" + headerStyle + @"' colspan='6'>Разпределение по възраст</th>
                           <th style='width: 60px;" + headerStyle + @"' rowspan='3'>Били на военна служба</th>
                           <th style='width: 240px;" + headerStyle + @"' colspan='4'>По образование</th>                           
                        </tr>
                        <tr>
                           <th style='width: 180px;" + headerStyle + @"' colspan='3'>мъже</th>
                           <th style='width: 180px;" + headerStyle + @"' colspan='3'>жени</th>
                           <th style='width: 120px;" + headerStyle + @"' colspan='2'>мъже</th>
                           <th style='width: 120px;" + headerStyle + @"' colspan='2'>жени</th>
                        </tr>
                        <tr>
                           <th style='width: 60px;" + headerStyle + @"'>до 25г.</th>
                           <th style='width: 60px;" + headerStyle + @"'>до 30г.</th>
                           <th style='width: 60px;" + headerStyle + @"'>над 30г.</th>
                           <th style='width: 60px;" + headerStyle + @"'>до 25г.</th>
                           <th style='width: 60px;" + headerStyle + @"'>до 30г.</th>
                           <th style='width: 60px;" + headerStyle + @"'>над 30г.</th>
                           <th style='width: 60px;" + headerStyle + @"'>висше</th>
                           <th style='width: 60px;" + headerStyle + @"'>средно</th>
                           <th style='width: 60px;" + headerStyle + @"'>висше</th>
                           <th style='width: 60px;" + headerStyle + @"'>средно</th>
                        </tr>
                     </thead>";

                int counter = 1;

                //Iterate through all items and add them into the grid
                foreach (ReportVacancyAnnounceApplicantsBlock block in listBlocks)
                {
                    string cellStyleText = "vertical-align: top;";
                    string cellStyleNumber = "vertical-align: top; text-align: right;";
                                      
                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='" + cellStyleNumber + @"'>" + block.CntBy_Age_Male_Under25.ToString() + @"</td>
                                 <td style='" + cellStyleNumber + @"'>" + block.CntBy_Age_Male_Under30.ToString() + @"</td>
                                 <td style='" + cellStyleNumber + @"'>" + block.CntBy_Age_Male_Over35.ToString() + @"</td>
                                 <td style='" + cellStyleNumber + @"'>" + block.CntBy_Age_Female_Under25.ToString() + @"</td>
                                 <td style='" + cellStyleNumber + @"'>" + block.CntBy_Age_Female_Under30.ToString() + @"</td>
                                 <td style='" + cellStyleNumber + @"'>" + block.CntBy_Age_Female_Over35.ToString() + @"</td>
                                 <td style='" + cellStyleNumber + @"'>" + block.CntBy_MilitaryService_Employed.ToString() + @"</td>
                                 <td style='" + cellStyleNumber + @"'>" + block.CntBy_Education_Male_UniversityDegree.ToString() + @"</td>
                                 <td style='" + cellStyleNumber + @"'>" + block.CntBy_Education_Male_HighSchoolDegree.ToString() + @"</td>
                                 <td style='" + cellStyleNumber + @"'>" + block.CntBy_Education_Female_UniversityDegree.ToString() + @"</td>
                                 <td style='" + cellStyleNumber + @"'>" + block.CntBy_Education_Female_HighSchoolDegree.ToString() + @"</td>
                              </tr>";

                    counter++;
                }

                html += "</table>";
            }

            //Put the generated grid on the page
            pnlReportsGrid.InnerHtml = html;            
        }

        //Collect the filet information from the page
        private ReportVacancyAnnounceApplicantsFilter CollectFilterData()
        {
            ReportVacancyAnnounceApplicantsFilter filter = new ReportVacancyAnnounceApplicantsFilter();

            int vacancyAnnounceId = 0;
            int responsibleMilitaryUnitId = 0;

            //Get VacancyAnnounceId
            vacancyAnnounceId = int.Parse(this.ddVacancyAnnounces.SelectedValue);

            filter.VacancyAnnounceId = vacancyAnnounceId;
            
            //Get ResponsibleMilitaryUnitId
            responsibleMilitaryUnitId = int.Parse(this.ddResponsibleMilitaryUnits.SelectedValue);

            filter.ResponsibleMilitaryUnitId = responsibleMilitaryUnitId;
            
            return filter;
        }
    }
}
