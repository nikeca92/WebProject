using System;
using System.Text;
using System.Collections.Generic;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class Home :RESPage
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
            LoadTechnicsMenuItems();
            SetupVisibility();
        }

        private void LoadTechnicsMenuItems()
        {
            StringBuilder html = new StringBuilder();
            StringBuilder techTypesReportLinks = new StringBuilder();

            List<TechnicsType> technicsTypes = TechnicsTypeUtil.GetAllTechnicsTypes(CurrentUser);

            foreach (TechnicsType technicsType in technicsTypes)
            {
                if (this.GetUIItemAccessLevel("RES_TECHNICS_" + technicsType.TypeKey) != UIAccessLevel.Hidden)
                {
                    html.Append(@"
<div class=""HomePageItem"" runat=""server"" id=""divManageTechnics_" + technicsType.TypeKey + @""">
   <span onclick=""HomePageItemClick('ManageTechnics_" + technicsType.TypeKey + @".aspx?fh=1');"" class=""HomePageItemLink"">" + technicsType.TypeName + @"</span>
</div>");
                }

                if (this.GetUIItemAccessLevel("RES_REPORTS_" + technicsType.TypeKey) != UIAccessLevel.Hidden)
                {
                    techTypesReportLinks.Append(@"
<div class=""HomePageItem"">
   <span onclick=""HomePageItemClick('ReportTechnics_" + technicsType.TypeKey + @".aspx?fh=1');"" class=""HomePageItemLink"">Списък на техниката на военен отчет - " + technicsType.TypeName + @"</span>
</div>");
                }
            }

            pnlTechnicsLinks.InnerHtml = html.ToString();
            divTechnicsTypesReportLinks.InnerHtml = techTypesReportLinks.ToString();
        }

        private void SetupVisibility()
        {
            if (this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS") == UIAccessLevel.Hidden &&
                this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS") == UIAccessLevel.Hidden)
            {
                divEquipment.Visible = false;
            }

            if (this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS") == UIAccessLevel.Hidden)
            {
                
                divAddEditEquipmentReservistsRequest.Visible = false;
                divManageEquipmentReservistsRequests.Visible = false;
                divManageEquipmentReservistsRequestsFulfilment.Visible = false;
                divFulfilReservistsMilCommand.Visible = false;
            }

            if (this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS") == UIAccessLevel.Hidden)
            {
                divAddEditEquipmentTechnicsRequest.Visible = false;
                divManageEquipmentTechnicsRequests.Visible = false;
                divManageEquipmentTechnicsRequestsFulfilment.Visible = false;
                divFulfilTechnicsMilCommand.Visible = false;
            }

            if (this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS") != UIAccessLevel.Enabled
                || this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_ADD") != UIAccessLevel.Enabled)
            {
                divAddEditEquipmentReservistsRequest.Visible = false;
            }

            if (this.GetUIItemAccessLevel("RES_EQUIPRESREQUESTS_FULFIL") == UIAccessLevel.Hidden)
            {             
                divManageEquipmentReservistsRequestsFulfilment.Visible = false;
                divFulfilReservistsMilCommand.Visible = false;
            }

            if (this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_FULFIL") == UIAccessLevel.Hidden)
            {
                divManageEquipmentTechnicsRequestsFulfilment.Visible = false;
                divFulfilTechnicsMilCommand.Visible = false;
            }

            if (this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS") != UIAccessLevel.Enabled
                || this.GetUIItemAccessLevel("RES_EQUIPTECHREQUESTS_ADD") != UIAccessLevel.Enabled)
            {
                divAddEditEquipmentTechnicsRequest.Visible = false;
            }

            if (this.GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden)
            {
                divHumanResources.Visible = false;
                divAddNewReservist.Visible = false;
                divManageReservists.Visible = false;                
                divImportPersonsData.Visible = false;
                divGroupTakingDown.Visible = false;
            }
           
            if (this.GetUIItemAccessLevel("RES_HUMANRES") != UIAccessLevel.Enabled ||
                this.GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST") != UIAccessLevel.Enabled)
            {
                divAddNewReservist.Visible = false;
            }            

            if (this.GetUIItemAccessLevel("RES_HUMANRES_GROUPTAKINGDOWN") == UIAccessLevel.Hidden)
            {
                divGroupTakingDown.Visible = false;
            }

            PMIS.Reserve.ContentPages.TechParamsPageUtil techParamsUtil = new PMIS.Reserve.ContentPages.TechParamsPageUtil(this, this.CurrentUser);
            bool canUserAccessAnyMaintenanceList = techParamsUtil.CanUserAccessAnyMaintenanceList();

            if (this.GetUIItemAccessLevel("RES_LISTMAINT") == UIAccessLevel.Hidden ||
                (this.GetUIItemAccessLevel("RES_LISTMAINT_MILITARYREADINESSNAMES") == UIAccessLevel.Hidden &&
                 this.GetUIItemAccessLevel("RES_LISTMAINT_VEHICLEMAKENAMES") == UIAccessLevel.Hidden &&
                 this.GetUIItemAccessLevel("RES_LISTMAINT_VEHICLEMODELS") == UIAccessLevel.Hidden &&
                 this.GetUIItemAccessLevel("RES_LISTMAINT_TRACTORMAKENAMES") == UIAccessLevel.Hidden &&
                 this.GetUIItemAccessLevel("RES_LISTMAINT_TRACTORMODELS") == UIAccessLevel.Hidden &&
                 this.GetUIItemAccessLevel("RES_LISTMAINT_ENGEQUIPBASEMAKENAMES") == UIAccessLevel.Hidden &&
                 this.GetUIItemAccessLevel("RES_LISTMAINT_ENGEQUIPBASEMODELS") == UIAccessLevel.Hidden &&
                 this.GetUIItemAccessLevel("RES_LISTMAINT_AVIATIONOTHERBASEMACHINEMAKENAMES") == UIAccessLevel.Hidden &&
                 this.GetUIItemAccessLevel("RES_LISTMAINT_AVIATIONOTHERBASEMACHINEMODELS") == UIAccessLevel.Hidden &&
                 this.GetUIItemAccessLevel("RES_LISTMAINT_TECHNICSCATEGORYNAMES") == UIAccessLevel.Hidden &&
                 this.GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES") == UIAccessLevel.Hidden) &&
                 !canUserAccessAnyMaintenanceList)
            { 
                this.divListMaint.Visible = false;
                this.divMilitaryReadinessNames.Visible = false;
                this.divTechnicsCategories.Visible = false;
                this.divCompanies.Visible = false;
                this.divTechParams.Visible = false;                
            }

            if (this.GetUIItemAccessLevel("RES_LISTMAINT_MILITARYREADINESSNAMES") == UIAccessLevel.Hidden)
            {
                this.divMilitaryReadinessNames.Visible = false;
            }

            if (this.GetUIItemAccessLevel("RES_LISTMAINT_TECHNICSCATEGORYNAMES") == UIAccessLevel.Hidden)
            {
                this.divTechnicsCategories.Visible = false;
            }

            if (this.GetUIItemAccessLevel("RES_LISTMAINT_COMPANIES") == UIAccessLevel.Hidden)
            {
                this.divCompanies.Visible = false;
            }

            if (!canUserAccessAnyMaintenanceList)
            {
                this.divTechParams.Visible = false;
            }
            
            if (this.GetUIItemAccessLevel("RES_TECHNICS") == UIAccessLevel.Hidden)
            {
                divTechnics.Visible = false;
                pnlTechnicsLinks.Visible = false;                
            }

            if ((this.GetUIItemAccessLevel("RES_POSTPONE") == UIAccessLevel.Hidden) ||
                (this.GetUIItemAccessLevel("RES_POSTPONE_RES") == UIAccessLevel.Hidden &&
                 this.GetUIItemAccessLevel("RES_POSTPONE_TECH") == UIAccessLevel.Hidden &&
                 this.GetUIItemAccessLevel("RES_POSTPONE_REPORT_RES") == UIAccessLevel.Hidden &&
                 this.GetUIItemAccessLevel("RES_POSTPONE_REPORT_TECH") == UIAccessLevel.Hidden &&
                 this.GetUIItemAccessLevel("RES_POSTPONE_REPORT_RES_BY_ADMINISTRATION") == UIAccessLevel.Hidden &&
                 this.GetUIItemAccessLevel("RES_POSTPONE_REPORT_TECH_BY_ADMINISTRATION") == UIAccessLevel.Hidden)
               )
            {
                this.divPostpone.Visible = false;
                this.divPostponeRes.Visible = false;
                this.divPostponeTech.Visible = false;
                this.divPostponeReportRes.Visible = false;
                this.divPostponeReportTech.Visible = false;
                this.divPostponeReportResByAdministration.Visible = false;
                this.divPostponeReportTechByAdministration.Visible = false;
            }

            if (this.GetUIItemAccessLevel("RES_POSTPONE_RES") == UIAccessLevel.Hidden)
            {
                this.divPostponeRes.Visible = false;
            }

            if (this.GetUIItemAccessLevel("RES_POSTPONE_TECH") == UIAccessLevel.Hidden)
            {
                this.divPostponeTech.Visible = false;
            }

            if (this.GetUIItemAccessLevel("RES_POSTPONE_REPORT_RES") == UIAccessLevel.Hidden)
            {
                this.divPostponeReportRes.Visible = false;
            }

            if (this.GetUIItemAccessLevel("RES_POSTPONE_REPORT_TECH") == UIAccessLevel.Hidden)
            {
                this.divPostponeReportTech.Visible = false;
            }

            if (this.GetUIItemAccessLevel("RES_POSTPONE_REPORT_RES_BY_ADMINISTRATION") == UIAccessLevel.Hidden)
            {
                this.divPostponeReportResByAdministration.Visible = false;
            }

            if (this.GetUIItemAccessLevel("RES_POSTPONE_REPORT_TECH_BY_ADMINISTRATION") == UIAccessLevel.Hidden)
            {
                this.divPostponeReportTechByAdministration.Visible = false;
            }

            if (this.GetUIItemAccessLevel("RES_REPORTS") == UIAccessLevel.Hidden &&
                (this.GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS") == UIAccessLevel.Hidden) &&
                (this.GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS") == UIAccessLevel.Hidden) &&
                (this.GetUIItemAccessLevel("RES_REPORTS_REPORTLISTRESERVISTSFROMCOMMAND") == UIAccessLevel.Hidden) &&
                (this.GetUIItemAccessLevel("RES_REPORTS_REPORTLISTTECHNICSFROMCOMMAND") == UIAccessLevel.Hidden) &&
                (this.GetUIItemAccessLevel("RES_REPORTS_VEHICLES") == UIAccessLevel.Hidden) &&
                (this.GetUIItemAccessLevel("RES_REPORTS_TRAILERS") == UIAccessLevel.Hidden) &&
                (this.GetUIItemAccessLevel("RES_REPORTS_TRACTORS") == UIAccessLevel.Hidden) &&
                (this.GetUIItemAccessLevel("RES_REPORTS_ENG_EQUIP") == UIAccessLevel.Hidden) &&
                (this.GetUIItemAccessLevel("RES_REPORTS_MOB_LIFT_EQUIP") == UIAccessLevel.Hidden) &&
                (this.GetUIItemAccessLevel("RES_REPORTS_RAILWAY_EQUIP") == UIAccessLevel.Hidden) &&
                (this.GetUIItemAccessLevel("RES_REPORTS_AVIATION_EQUIP") == UIAccessLevel.Hidden) &&
                (this.GetUIItemAccessLevel("RES_REPORTS_VESSELS") == UIAccessLevel.Hidden) &&
                (this.GetUIItemAccessLevel("RES_REPORTS_FUEL_CONTAINERS") == UIAccessLevel.Hidden) &&
                (this.GetUIItemAccessLevel("RES_REPORTS_REPORTSV1") == UIAccessLevel.Hidden) &&
                (this.GetUIItemAccessLevel("RES_REPORTS_REPORTA31") == UIAccessLevel.Hidden) &&
                (this.GetUIItemAccessLevel("RES_REPORTS_REPORTA33") == UIAccessLevel.Hidden) &&
                (this.GetUIItemAccessLevel("RES_REPORTS_REPORTA33v2") == UIAccessLevel.Hidden) &&
                (this.GetUIItemAccessLevel("RES_REPORTS_REPORTANALYZECOMMAND") == UIAccessLevel.Hidden) &&
                (this.GetUIItemAccessLevel("RES_REPORTS_REPORTNORMATIVETECHNICS") == UIAccessLevel.Hidden) &&
                (this.GetUIItemAccessLevel("RES_REPORTS_REPORTANALYZERESFULFILMENT") == UIAccessLevel.Hidden) &&
                (this.GetUIItemAccessLevel("RES_REPORTS_REPORTSTAFFPOSITIONLIST") == UIAccessLevel.Hidden))
            {
                this.divReports.Visible = false;
                this.divReportListReservistsWithAppointments.Visible = false;
                this.divReportListTechnicsWithAppointments.Visible = false;
                this.divReportListReservistsFromCommand.Visible = false;
                this.divReportListTechnicsFromCommand.Visible = false;
                this.divReportSV1.Visible = false;
                this.divReportA31.Visible = false;
                this.divReportA33.Visible = false;
                this.divReportA33v2.Visible = false;
                this.divReportAnalyzeCommand.Visible = false;
                this.divReportAnalyzeResFulfilment.Visible = false;
                this.divReportNormativeTechnics.Visible = false;
                this.divTechnicsTypesReportLinks.Visible = false;
            }

            if (this.GetUIItemAccessLevel("RES_REPORTS_LISTRESWITHAPPOINTMENTS") == UIAccessLevel.Hidden)
            {
                this.divReportListReservistsWithAppointments.Visible = false;
            }

            if (this.GetUIItemAccessLevel("RES_REPORTS_LISTTECHWITHAPPOINTMENTS") == UIAccessLevel.Hidden)
            {
                this.divReportListTechnicsWithAppointments.Visible = false;
            }

            if (this.GetUIItemAccessLevel("RES_REPORTS_REPORTLISTRESERVISTSFROMCOMMAND") == UIAccessLevel.Hidden)
            {
                this.divReportListReservistsFromCommand.Visible = false;
            }

            if (this.GetUIItemAccessLevel("RES_REPORTS_REPORTLISTTECHNICSFROMCOMMAND") == UIAccessLevel.Hidden)
            {
                this.divReportListTechnicsFromCommand.Visible = false;
            }

            if (this.GetUIItemAccessLevel("RES_REPORTS_REPORTSV1") == UIAccessLevel.Hidden)
            {
                this.divReportSV1.Visible = false;
            }

            if (this.GetUIItemAccessLevel("RES_REPORTS_REPORTA31") == UIAccessLevel.Hidden)
            {
                this.divReportA31.Visible = false;
            }

            if (this.GetUIItemAccessLevel("RES_REPORTS_REPORTA33") == UIAccessLevel.Hidden)
            {
                this.divReportA33.Visible = false;
            }

            if (this.GetUIItemAccessLevel("RES_REPORTS_REPORTA33v2") == UIAccessLevel.Hidden)
            {
                this.divReportA33v2.Visible = false;
            }

            if (this.GetUIItemAccessLevel("RES_REPORTS_REPORTANALYZECOMMAND") == UIAccessLevel.Hidden)
            {
                this.divReportAnalyzeCommand.Visible = false;
            }

            if (this.GetUIItemAccessLevel("RES_REPORTS_REPORTANALYZERESFULFILMENT") == UIAccessLevel.Hidden)
            {
                this.divReportAnalyzeResFulfilment.Visible = false;
            }

            if (this.GetUIItemAccessLevel("RES_REPORTS_REPORTSTAFFPOSITIONLIST") == UIAccessLevel.Hidden)
            {
                this.divReportStaffPositionsList.Visible = false;
            }

            if (this.GetUIItemAccessLevel("RES_REPORTS_REPORTNORMATIVETECHNICS") == UIAccessLevel.Hidden)
            {
                this.divReportNormativeTechnics.Visible = false;
            }

            if (this.GetUIItemAccessLevel("RES_PRINT") == UIAccessLevel.Hidden ||
                (((this.GetUIItemAccessLevel("RES_PRINT_RESERVISTS") == UIAccessLevel.Hidden) &&
                  (this.GetUIItemAccessLevel("RES_PRINT_TECHNICS") == UIAccessLevel.Hidden) &&
                  (this.GetUIItemAccessLevel("RES_PRINT_POSTPONE_RESERVISTS") == UIAccessLevel.Hidden)
                 ) ||
                 ((this.GetUIItemAccessLevel("RES_PRINT_RESERVISTS_MK") == UIAccessLevel.Hidden) &&
                  (this.GetUIItemAccessLevel("RES_PRINT_RESERVISTS_PZ") == UIAccessLevel.Hidden) &&
                  (this.GetUIItemAccessLevel("RES_PRINT_TECHNICS_MK") == UIAccessLevel.Hidden) &&
                  (this.GetUIItemAccessLevel("RES_PRINT_TECHNICS_PZ") == UIAccessLevel.Hidden)
                 )
                )
               )
            {
                this.divPrint.Visible = false;
                this.divPrintReservists.Visible = false;
                this.divPrintTechnics.Visible = false;
                this.divPrintPostponeReservists.Visible = false;
            }

            if (this.GetUIItemAccessLevel("RES_PRINT_RESERVISTS") == UIAccessLevel.Hidden ||
                (
                  (this.GetUIItemAccessLevel("RES_PRINT_RESERVISTS_MK") == UIAccessLevel.Hidden) &&
                  (this.GetUIItemAccessLevel("RES_PRINT_RESERVISTS_PZ") == UIAccessLevel.Hidden)
                )
               )
            {
                this.divPrintReservists.Visible = false;
            }

            if (this.GetUIItemAccessLevel("RES_PRINT_TECHNICS") == UIAccessLevel.Hidden ||
                (
                  (this.GetUIItemAccessLevel("RES_PRINT_TECHNICS_MK") == UIAccessLevel.Hidden) &&
                  (this.GetUIItemAccessLevel("RES_PRINT_TECHNICS_PZ") == UIAccessLevel.Hidden)
                )
               )
            {
                this.divPrintTechnics.Visible = false;
            }

            if (this.GetUIItemAccessLevel("RES_PRINT_POSTPONE_RESERVISTS") == UIAccessLevel.Hidden)
            {
                this.divPrintPostponeReservists.Visible = false;
            }
        }
    }
}
