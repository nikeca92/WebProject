using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using PMIS.Common;
using PMIS.Applicants.Common;

namespace PMIS.Applicants.ContentPages
{
    public partial class ReportDocumentsApplied : APPLPage
    {
        public string MilitaryUnitLabel = CommonFunctions.GetLabelText("MilitaryUnit");

        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "APPL_REPORTS_REPORT_DOCUMENTS_APPLIED";
            }
        }

        UIAccessLevel l;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if (GetUIItemAccessLevel("APPL_REPORTS_REPORT_DOCUMENTS_APPLIED") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            //Hilight the current page in the menu bar
            HighlightMenuItems("Reports", "ReportDocumentsApplied");

            //Hide any nvaigation buttons (e.g. Back) that should not be visible when the screen is loaded from the menu bar
            HideNavigationControls(btnBack);

            //When the page is loaded for the first time
            if (!IsPostBack)
            {
                //Populate any drop-downs and list-boxes
                PopulateLists();
                this.btnExcel.Visible = false;
            }

            lblMessage.Text = "";
            lblGridMessage.Text = "";
        }
               
        //Populate all listboxes on the screen
        private void PopulateLists()
        {
            this.PopulateVacancyAnnounces();            
            this.PopulateMilitaryDepartments();
        }

        //Populate the VacancyAnnounces drop-down
        private void PopulateVacancyAnnounces()
        {
            this.ddlVacancyAnnounces.DataSource = VacancyAnnounceUtil.GetVacancyAnnouncesListItemsForReports("APPL_REPORTS_REPORT_RATED_APPLICANTS_SUMMARY", true, CurrentUser);
            this.ddlVacancyAnnounces.DataTextField = "Text";
            this.ddlVacancyAnnounces.DataValueField = "Value";
            this.ddlVacancyAnnounces.DataBind();
            this.ddlVacancyAnnounces.Items.Insert(0, ListItems.GetOptionChooseOne());
        }

        //Populate the MilitaryDepartments drop-down
        private void PopulateMilitaryDepartments()
        {
            this.ddlMilitaryDepartments.DataSource = MilitaryDepartmentUtil.GetAllMilitaryDepartments(CurrentUser);
            this.ddlMilitaryDepartments.DataTextField = "MilitaryDepartmentName";
            this.ddlMilitaryDepartments.DataValueField = "MilitaryDepartmentId";
            this.ddlMilitaryDepartments.DataBind();
            this.ddlMilitaryDepartments.Items.Insert(0, ListItems.GetOptionAll());
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
            ddlVacancyAnnounces.SelectedValue = ListItems.GetOptionChooseOne().Value;
            ddlMilitaryDepartments.SelectedValue = ListItems.GetOptionAll().Value;            
        }
        
        //Refresh the data grid
        private void RefreshReports()
        {
            string html = "";

            //Collect the filter information to be able to pull the number of rows for this specific filter
            ReportDocumentsAppliedFilter filter = CollectFilterData();

            //Get the list of records according to the specified filters and paging
            List<ReportDocumentsAppliedBlock> listBlocks = ReportDocumentsAppliedUtil.GetReportDocumentsAppliedSearch(filter, CurrentUser);

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
                           <th style='width: 220px;" + headerStyle + @"' rowspan='2'>ВО</th>
                           <th style='width: 120px;" + headerStyle + @"' colspan='2' rowspan='2'>кандидати, подали документи за кандидатстване</th>
                           <th style='width: 480px;" + headerStyle + @"' colspan='7'>кандидати, годни за служба във въоръжените сили - ГСВС </th>                           
                        </tr>
                        <tr>
                           <th style='width: 120px;" + headerStyle + @"' colspan='2'>кандидати ГСВС</th>
                           <th style='width: 120px;" + headerStyle + @"' colspan='2'>от тях без военна подготовка</th>
                           <th style='width: 180px;" + headerStyle + @"' colspan='3'>възраст на кандидатите</th>
                        </tr>
                        <tr>
                           <th style='width: 60px;" + headerStyle + @"'></th>
                           <th style='width: 60px;" + headerStyle + @"'>мъже</th>
                           <th style='width: 60px;" + headerStyle + @"'>жени</th>
                           <th style='width: 60px;" + headerStyle + @"'>мъже</th>
                           <th style='width: 60px;" + headerStyle + @"'>жени</th>
                           <th style='width: 60px;" + headerStyle + @"'>мъже</th>
                           <th style='width: 60px;" + headerStyle + @"'>жени</th>
                           <th style='width: 60px;" + headerStyle + @"'>&lt; 25г.</th>
                           <th style='width: 60px;" + headerStyle + @"'>&lt; 30г.</th>
                           <th style='width: 60px;" + headerStyle + @"'>&gt; 30г.</th>
                        </tr>
                     </thead>";

                int counter = 1;

                //Iterate through all items and add them into the grid
                foreach (ReportDocumentsAppliedBlock block in listBlocks)
                {
                    string cellStyleTotal = block.RowType > 0 ? " font-weight: bold; " : "";
                    string cellStyleNumber = " vertical-align: top; text-align: right; ";
                    string cellStyleText = "vertical-align: top; text-align: left;";

                    html += @"<tr class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'>
                                 <td style='" + cellStyleText + cellStyleTotal + @"'>" + block.MilitaryDepartment + @"</td>
                                 <td style='" + cellStyleNumber + cellStyleTotal + @"'>" + block.CntBy_DocumentsApplied_Male.ToString() + @"</td>
                                 <td style='" + cellStyleNumber + cellStyleTotal + @"'>" + block.CntBy_DocumentsApplied_Female.ToString() + @"</td>
                                 <td style='" + cellStyleNumber + cellStyleTotal + @"'>" + block.CntBy_MedCertFit_Male.ToString() + @"</td>
                                 <td style='" + cellStyleNumber + cellStyleTotal + @"'>" + block.CntBy_MedCertFit_Female.ToString() + @"</td>
                                 <td style='" + cellStyleNumber + cellStyleTotal + @"'>" + block.CntBy_MedCertFit_NoMilitaryTraining_Male.ToString() + @"</td>
                                 <td style='" + cellStyleNumber + cellStyleTotal + @"'>" + block.CntBy_MedCertFit_NoMilitaryTraining_Female.ToString() + @"</td>
                                 <td style='" + cellStyleNumber + cellStyleTotal + @"'>" + block.CntBy_MedCertFit_Age_Under25.ToString() + @"</td>
                                 <td style='" + cellStyleNumber + cellStyleTotal + @"'>" + block.CntBy_MedCertFit_Age_Under30.ToString() + @"</td>
                                 <td style='" + cellStyleNumber + cellStyleTotal + @"'>" + block.CntBy_MedCertFit_Age_Over30.ToString() + @"</td>
                              </tr>";

                    counter++;
                }

                html += "</table>";
            }

            //Put the generated grid on the page
            pnlReportsGrid.InnerHtml = html;
        }

        //Collect the filet information from the page
        private ReportDocumentsAppliedFilter CollectFilterData()
        {
            ReportDocumentsAppliedFilter filter = new ReportDocumentsAppliedFilter();

            if (this.ddlVacancyAnnounces.SelectedValue != ListItems.GetOptionChooseOne().Value
                && this.ddlVacancyAnnounces.SelectedValue != "")
            {
                filter.VacancyAnnounceId = int.Parse(this.ddlVacancyAnnounces.SelectedValue);
                this.hfVacancyAnnounceId.Value = this.ddlVacancyAnnounces.SelectedValue;
            }
            else
            {
                this.hfVacancyAnnounceId.Value = "";
            }

            

            if (this.ddlMilitaryDepartments.SelectedValue != ListItems.GetOptionChooseOne().Value
                && this.ddlMilitaryDepartments.SelectedValue != "")
            {
                filter.MilitaryDepartmentId = int.Parse(this.ddlMilitaryDepartments.SelectedValue);
                this.hfMilitaryDepartmentId.Value = this.ddlMilitaryDepartments.SelectedValue;
            }
            else
            {
                this.hfMilitaryDepartmentId.Value = "";
            }
            
            return filter;
        }
    }
}
