using System;
using System.Collections;
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

using PMIS.Common;
using PMIS.Applicants.Common;

namespace PMIS.Applicants.ContentPages
{
    public partial class Home : APPLPage
    {
        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            HighlightMenuItems("Home");
            this.SetupVisibility();
        }

        private void SetupVisibility()
        {
            if (this.GetUIItemAccessLevel("APPL_VACANN") == UIAccessLevel.Hidden)
            {
                this.divVacancyAnnounces.Visible = false;
                this.divManageVacancyAnnounces.Visible = false;
                this.divAddEditVacancyAnnounce.Visible = false;
            }
            else if (this.GetUIItemAccessLevel("APPL_VACANN_ADDVACANN") != UIAccessLevel.Enabled)
            {
                this.divAddEditVacancyAnnounce.Visible = false;
            }

            if (this.GetUIItemAccessLevel("APPL_APPL") == UIAccessLevel.Hidden)
            {
                divApplicants.Visible = false;
                divManageApplicants.Visible = false;
                divAddEditApplicant.Visible = false;
            }

            if (this.GetUIItemAccessLevel("APPL_APPL_ADDAPPL") != UIAccessLevel.Enabled ||
                this.GetUIItemAccessLevel("APPL_APPL") != UIAccessLevel.Enabled)
            {
                divAddEditApplicant.Visible = false;
            }

            if (this.GetUIItemAccessLevel("APPL_APPL_ALLOWANCE") == UIAccessLevel.Hidden
                || this.GetUIItemAccessLevel("APPL_APPL") == UIAccessLevel.Hidden)
            {
                divApplicantsAllowance.Visible = false;
            }

            if (this.GetUIItemAccessLevel("APPL_APPL_EXAMMARKS") == UIAccessLevel.Hidden
                || this.GetUIItemAccessLevel("APPL_APPL") == UIAccessLevel.Hidden)
            {
                divApplicantsExams.Visible = false;
            }

            if (this.GetUIItemAccessLevel("APPL_APPL_RANKING") == UIAccessLevel.Hidden
                || this.GetUIItemAccessLevel("APPL_APPL") == UIAccessLevel.Hidden)
            {
                divApplicantsRanking.Visible = false;
            }

            if (this.GetUIItemAccessLevel("APPL_APPL_NOMINATING") == UIAccessLevel.Hidden
                || this.GetUIItemAccessLevel("APPL_APPL") == UIAccessLevel.Hidden)
            {
                divApplicantsNomination.Visible = false;
            }

            if (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL") == UIAccessLevel.Hidden
                || this.GetUIItemAccessLevel("APPL_APPL") == UIAccessLevel.Hidden)
            {
                divManagePotencialApplicants.Visible = false;
                divAddEditPotencialApplicant.Visible = false;
            }

            if (this.GetUIItemAccessLevel("APPL_POTENCIALAPPL_ADD_POTENCIALAPPL") != UIAccessLevel.Enabled
                || this.GetUIItemAccessLevel("APPL_POTENCIALAPPL") != UIAccessLevel.Enabled
                || this.GetUIItemAccessLevel("APPL_APPL") == UIAccessLevel.Hidden)
            {
                divAddEditPotencialApplicant.Visible = false;
            }

            if ((this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_REPORT_DETAILED") == UIAccessLevel.Hidden
                  && this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_PARTICIPATE") == UIAccessLevel.Hidden
                  && this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_RANKING") == UIAccessLevel.Hidden
                  && this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_NOMINATION") == UIAccessLevel.Hidden
                  && this.GetUIItemAccessLevel("APPL_REPORTS_REPORT_CADET") == UIAccessLevel.Hidden
                  && this.GetUIItemAccessLevel("APPL_REPORTS_REPORT_RATED_APPLICANTS_SUMMARY") == UIAccessLevel.Hidden
                  && this.GetUIItemAccessLevel("APPL_REPORTS_REPORT_VACANCY_ANNOUNCE_APPLICANTS") == UIAccessLevel.Hidden
                  && this.GetUIItemAccessLevel("APPL_REPORTS_REPORT_DOCUMENTS_APPLIED") == UIAccessLevel.Hidden
                  && this.GetUIItemAccessLevel("APPL_REPORTS_REPORT_DOCUMENTS_SENT") == UIAccessLevel.Hidden)
              || this.GetUIItemAccessLevel("APPL_REPORTS") == UIAccessLevel.Hidden)
            {
                //Hide all items in this ItemsBlock
                divReports.Visible = false;
                divReportDetailed.Visible = false;
                divListParticipate.Visible = false;
                divListRanking.Visible = false;
                divListNominated.Visible = false;
                divReportCadet.Visible = false;
                divReportRatedApplicantsSummary.Visible = false;
                divReportVacancyAnnounceApplicants.Visible = false;
                divReportDocumentsApplied.Visible = false;
                divReportDocumentsSent.Visible = false;
            }
            else
            {
                if (this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_REPORT_DETAILED") == UIAccessLevel.Hidden)
                {
                    divReportDetailed.Visible = false;
                }

                if (this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_PARTICIPATE") == UIAccessLevel.Hidden)
                {
                    divListParticipate.Visible = false;
                }

                if (this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_RANKING") == UIAccessLevel.Hidden)
                {
                    divListRanking.Visible = false;
                }

                if (this.GetUIItemAccessLevel("APPL_REPORTS_VACANNAPPL_LIST_NOMINATION") == UIAccessLevel.Hidden)
                {
                    divListNominated.Visible = false;
                }

                if (this.GetUIItemAccessLevel("APPL_REPORTS_REPORT_CADET") == UIAccessLevel.Hidden)
                {
                    divReportCadet.Visible = false;
                }

                if (this.GetUIItemAccessLevel("APPL_REPORTS_REPORT_RATED_APPLICANTS_SUMMARY") == UIAccessLevel.Hidden)
                {
                    divReportRatedApplicantsSummary.Visible = false;
                }

                if (this.GetUIItemAccessLevel("APPL_REPORTS_REPORT_VACANCY_ANNOUNCE_APPLICANTS") == UIAccessLevel.Hidden)
                {
                    divReportVacancyAnnounceApplicants.Visible = false;
                }

                if (this.GetUIItemAccessLevel("APPL_REPORTS_REPORT_DOCUMENTS_APPLIED") == UIAccessLevel.Hidden)
                {
                    divReportDocumentsApplied.Visible = false;
                }

                if (this.GetUIItemAccessLevel("APPL_REPORTS_REPORT_DOCUMENTS_SENT") == UIAccessLevel.Hidden)
                {
                    divReportDocumentsSent.Visible = false;
                }
            }

            if (this.GetUIItemAccessLevel("APPL_CADETS") == UIAccessLevel.Hidden)
            {
                this.divCadets.Visible = false;
                this.divManageCadets.Visible = false;
                this.divAddEditCadet.Visible = false;
                this.divCadetsRanking.Visible = false;
                this.divManageMilitarySchoolSpecializations.Visible = false;
            }
            
            if (this.GetUIItemAccessLevel("APPL_CADETS_ADDCADET") == UIAccessLevel.Hidden
                && this.GetUIItemAccessLevel("APPL_CADETS_RANKING") == UIAccessLevel.Hidden
                && this.GetUIItemAccessLevel("APPL_CADETS_MILITSCHOOlSPECIALIZATION") == UIAccessLevel.Hidden)
            {
                this.divAddEditCadet.Visible = false;
                this.divCadetsRanking.Visible = false;
                this.divManageMilitarySchoolSpecializations.Visible = false;
            }
            
            if (this.GetUIItemAccessLevel("APPL_CADETS_ADDCADET") != UIAccessLevel.Enabled)
            {
                this.divAddEditCadet.Visible = false;
            }
            
            if (this.GetUIItemAccessLevel("APPL_CADETS_RANKING") == UIAccessLevel.Hidden)
            {
                this.divCadetsRanking.Visible = false;
            }
            
            if (this.GetUIItemAccessLevel("APPL_CADETS_MILITSCHOOlSPECIALIZATION") == UIAccessLevel.Hidden)
            {
                this.divManageMilitarySchoolSpecializations.Visible = false;
            }

            if (this.GetUIItemAccessLevel("APPL_LISTMAINT") == UIAccessLevel.Hidden
                || (this.GetUIItemAccessLevel("APPL_DOCUMENTS") == UIAccessLevel.Hidden
                && this.GetUIItemAccessLevel("APPL_EXAMS") == UIAccessLevel.Hidden))
            {
                this.divListMaint.Visible = false;
                this.divDocuments.Visible = false;
                this.divExams.Visible = false;
            }
            
            if (this.GetUIItemAccessLevel("APPL_DOCUMENTS") == UIAccessLevel.Hidden)
            {
                this.divDocuments.Visible = false;
            }
            
            if (this.GetUIItemAccessLevel("APPL_EXAMS") == UIAccessLevel.Hidden)
            {
                this.divExams.Visible = false;
            }
        }
    }
}
