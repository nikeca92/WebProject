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
using PMIS.HealthSafety.Common;

namespace PMIS.HealthSafety.ContentPages
{
    public partial class Home : HSPage
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
            SetupVisibility();
        }

        private void SetupVisibility()
        {
            if (this.GetUIItemAccessLevel("HS_PROTOCOLS") == UIAccessLevel.Hidden)
            {
                divProtocols.Visible = false;
                divManageProtocols.Visible = false;
                divAddProtocol.Visible = false;
            }

            if (this.GetUIItemAccessLevel("HS_ADDPROT") != UIAccessLevel.Enabled
                || this.GetUIItemAccessLevel("HS_PROTOCOLS") != UIAccessLevel.Enabled)
            {
                divAddProtocol.Visible = false;
            }

            if (this.GetUIItemAccessLevel("HS_RISKASSESSMENTS") == UIAccessLevel.Hidden)
            {
                divMilitaryUnitPositions.Visible = false;
                divRiskCard.Visible = false;
                divRiskAssessments.Visible = false;
                divManageRiskAssessments.Visible = false;
                divAddRiskAssessment.Visible = false;
            }

            if (this.GetUIItemAccessLevel("HS_MILITARYUNITPOSITIONS") == UIAccessLevel.Hidden
                || this.GetUIItemAccessLevel("HS_RISKASSESSMENTS") == UIAccessLevel.Hidden)
            {
                divMilitaryUnitPositions.Visible = false;
            }

            if (this.GetUIItemAccessLevel("HS_RISKCARD") == UIAccessLevel.Hidden
                || this.GetUIItemAccessLevel("HS_RISKASSESSMENTS") == UIAccessLevel.Hidden)
            {
                divRiskCard.Visible = false;
            }

            if (this.GetUIItemAccessLevel("HS_ADDRISKASSESS") != UIAccessLevel.Enabled
                || this.GetUIItemAccessLevel("HS_RISKASSESSMENTS") != UIAccessLevel.Enabled)
            {
                divAddRiskAssessment.Visible = false;
            }

            if (this.GetUIItemAccessLevel("HS_WCONDCARDS") == UIAccessLevel.Hidden)
            {
                divWCondCards.Visible = false;
                divManageWCondCards.Visible = false;
                divAddWCondCard.Visible = false;
            }

            if (this.GetUIItemAccessLevel("HS_ADDWCONDCARD") != UIAccessLevel.Enabled
                || this.GetUIItemAccessLevel("HS_WCONDCARDS") != UIAccessLevel.Enabled)
            {
                divAddWCondCard.Visible = false;
            }

            if (this.GetUIItemAccessLevel("HS_COMMITTEE") == UIAccessLevel.Hidden)
            {
                divCommittees.Visible = false;
                divManageCommittees.Visible = false;
                divAddCommittee.Visible = false;
            }

            if (this.GetUIItemAccessLevel("HS_ADDCOMMITTEE") != UIAccessLevel.Enabled
                || this.GetUIItemAccessLevel("HS_COMMITTEE") != UIAccessLevel.Enabled)
            {
                divAddCommittee.Visible = false;
            }

            if (this.GetUIItemAccessLevel("HS_TRAININGHISTORY") == UIAccessLevel.Hidden)
            {
                divManageTrainingHistory.Visible = false;
            }

            int i = 0;
            if (this.GetUIItemAccessLevel("HS_DECLARATIONACC") == UIAccessLevel.Hidden)
            {
                i++;
                divManageDeclarationOfAccident.Visible = false;
                divAddDeclarationOfAccident.Visible = false;
            }

            if (this.GetUIItemAccessLevel("HS_ADDDECLARATIONACC") != UIAccessLevel.Enabled
               || this.GetUIItemAccessLevel("HS_DECLARATIONACC") != UIAccessLevel.Enabled)
            {
                divAddDeclarationOfAccident.Visible = false;
            }

            if (this.GetUIItemAccessLevel("HS_INVPROTOCOLS") == UIAccessLevel.Hidden)
            {
                i++;
                divManageInvestigationProtocol.Visible = false;
                divAddInvestigationProtocol.Visible = false;
            }

            if (this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL") != UIAccessLevel.Enabled
               || this.GetUIItemAccessLevel("HS_INVPROTOCOLS") != UIAccessLevel.Enabled)
            {
                divAddInvestigationProtocol.Visible = false;
            }

            if (this.GetUIItemAccessLevel("HS_UNSAFEWCONDNOTICE") == UIAccessLevel.Hidden)
            {
                i++;
                divManageUnsafeWCondNotices.Visible = false;
                divAddUnsafeWCondNotices.Visible = false;
            }

            if (this.GetUIItemAccessLevel("HS_ADDUNSAFEWCONDNOTICE") != UIAccessLevel.Enabled
                || this.GetUIItemAccessLevel("HS_UNSAFEWCONDNOTICE") != UIAccessLevel.Enabled)
            {
                divAddUnsafeWCondNotices.Visible = false;
            }

            if (i == 3)
            {
                divWorkAccidents.Visible = false;
            }

            if (this.GetUIItemAccessLevel("HS_LISTMAINT") == UIAccessLevel.Hidden
               || this.GetUIItemAccessLevel("HS_LISTMAINT_PROTTYPES") == UIAccessLevel.Hidden)
            {
                divListsProtocolTypes.Visible = false;
            }

            if (this.GetUIItemAccessLevel("HS_LISTMAINT") == UIAccessLevel.Hidden
               || this.GetUIItemAccessLevel("HS_LISTMAINT_MEASURES") == UIAccessLevel.Hidden)
            {
                divListsMeasures.Visible = false;
            }

            if (this.GetUIItemAccessLevel("HS_LISTMAINT") == UIAccessLevel.Hidden
               || this.GetUIItemAccessLevel("HS_LIST_INDICATORTYPES") == UIAccessLevel.Hidden)
            {
                divIndicatorTypeMaintenance.Visible = false;
            }

            if (this.GetUIItemAccessLevel("HS_LISTMAINT") == UIAccessLevel.Hidden
               || this.GetUIItemAccessLevel("HS_LISTMAINT_WORKINGPLACES") == UIAccessLevel.Hidden)
            {
                divWorkingPlacesMaintenance.Visible = false;
            }

            if (this.GetUIItemAccessLevel("HS_LISTMAINT") == UIAccessLevel.Hidden
               || this.GetUIItemAccessLevel("HS_LISTMAINT_PROBABILITIES") == UIAccessLevel.Hidden)
            {
                divListsProbabilities.Visible = false;
            }

            if (this.GetUIItemAccessLevel("HS_LISTMAINT") == UIAccessLevel.Hidden
               || this.GetUIItemAccessLevel("HS_LISTMAINT_EXPOSURE") == UIAccessLevel.Hidden)
            {
                divListsExposure.Visible = false;
            }

            if (this.GetUIItemAccessLevel("HS_LISTMAINT") == UIAccessLevel.Hidden
               || this.GetUIItemAccessLevel("HS_LISTMAINT_EFFECTWEIGHT") == UIAccessLevel.Hidden)
            {
                divListsEffectWeight.Visible = false;
            }

            if (this.GetUIItemAccessLevel("HS_LISTMAINT") == UIAccessLevel.Hidden
               || this.GetUIItemAccessLevel("HS_LISTMAINT_RISKRANK") == UIAccessLevel.Hidden)
            {
                divListsRiskRank.Visible = false;
            }

            if (this.GetUIItemAccessLevel("HS_LISTMAINT") == UIAccessLevel.Hidden
               || this.GetUIItemAccessLevel("HS_LIST_RISKFACTORTYPES") == UIAccessLevel.Hidden)
            {
                divRiskFactorTypeMaintenance.Visible = false;
            }


            if (this.GetUIItemAccessLevel("HS_LISTMAINT") == UIAccessLevel.Hidden ||
                (this.GetUIItemAccessLevel("HS_LISTMAINT_PROTTYPES") == UIAccessLevel.Hidden &&
                 this.GetUIItemAccessLevel("HS_LISTMAINT_MEASURES") == UIAccessLevel.Hidden &&
                 this.GetUIItemAccessLevel("HS_LIST_INDICATORTYPES") == UIAccessLevel.Hidden &&
                 this.GetUIItemAccessLevel("HS_LISTMAINT_WORKINGPLACES") == UIAccessLevel.Hidden &&
                 this.GetUIItemAccessLevel("HS_LISTMAINT_PROBABILITIES") == UIAccessLevel.Hidden &&
                 this.GetUIItemAccessLevel("HS_LISTMAINT_EXPOSURE") == UIAccessLevel.Hidden &&
                 this.GetUIItemAccessLevel("HS_LISTMAINT_EFFECTWEIGHT") == UIAccessLevel.Hidden &&
                 this.GetUIItemAccessLevel("HS_LISTMAINT_RISKRANK") == UIAccessLevel.Hidden &
                 this.GetUIItemAccessLevel("HS_LIST_RISKFACTORTYPES") == UIAccessLevel.Hidden))
            {
                divLists.Visible = false;
            }
        }
    }
}
