using System;
using System.Web.UI;
using System.Drawing;

using PMIS.Common;
using PMIS.HealthSafety.Common;
using System.Collections.Generic;

namespace PMIS.HealthSafety.ContentPages
{
    public partial class AddEditInvestigationProtocol : HSPage
    {
        #region Properties
        //Declare fileds using in LightBox
        int pageLength = Convert.ToInt32(PMIS.Common.Config.GetWebSetting("RowsPerPage"));
        int maxPage;

        string declarationNumber;
        DateTime? declarationDateFrom;
        DateTime? declarationDateTo;
        string workerFullName;

        DeclarationOfAccidentFilter declarationOfAccidentFilter;

        InvestigationProtocol investigationProtocol;

        //private bool tableEditPermission = true;
        //private bool tableDeletePermission = true;
        //private bool showTable = true;

        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "HS_INVPROTOCOLS";
            }
        }

        //Getter/Setter of the ID of the displayed investigationProtocol(0 - if new)
        private int InvestigaitonProtocolId
        {
            get
            {
                int investigaitonProtocolId = 0;
                //gets investigationProtocolId either from the hidden field on the page or from page url
                if (String.IsNullOrEmpty(this.hfInvestigaitonProtocolId.Value))
                {
                    if (Request.Params["investigaitonProtocolId"] != null)
                        int.TryParse(Request.Params["investigaitonProtocolId"].ToString(), out investigaitonProtocolId);

                    //sets protocol ID in hidden field on the page in order to be accessible in javascript
                    this.hfInvestigaitonProtocolId.Value = investigaitonProtocolId.ToString();
                }
                else
                {
                    Int32.TryParse(this.hfInvestigaitonProtocolId.Value, out investigaitonProtocolId);
                }

                return investigaitonProtocolId;
            }
            set { this.hfInvestigaitonProtocolId.Value = value.ToString(); }
        }

        //This is a flag field that says if the screen is opened from the Home screen
        //This is used to navigate the user back to the home screen when using the Back button
        private int FromHome
        {
            get
            {
                int fh = 0;
                if (String.IsNullOrEmpty(this.hfFromHome.Value)
                    || this.hfFromHome.Value == "0")
                {
                    if (Request.Params["fh"] != null)
                        int.TryParse(Request.Params["fh"].ToString(), out fh);

                    this.hfFromHome.Value = fh.ToString();
                }
                else
                {
                    Int32.TryParse(this.hfFromHome.Value, out fh);
                }

                return fh;
            }

            set
            {
                this.hfFromHome.Value = value.ToString();
            }
        }
        #endregion

        #region InitialPage
        protected void Page_Load(object sender, EventArgs e)
        {

            //We have Ajax request to paging or filtering in lightbox
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetDeclarationTableField")
            {
                ObtainFilterFields();
                GetLightBoxItems();
            }

            //We have request to select declrationId and set its values to investigationProtocol window
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetInvProtFields")
            {
                JSGetInvProtFields();
            }

            if (!IsPostBack)
            {
                //Process request for parameter
                if (Request.Params["investigationProtocolId"] != null)
                {
                    //Set investigaitonProtocolId
                    this.InvestigaitonProtocolId = Convert.ToInt32(Request.Params["investigationProtocolId"]);
                }

                this.FillTableWithProtocolData(); //Fill UI obect with data (empty for Insert, binded from Object fot Update)

                //Highlight the current page in the menu bar
                HighlightMenuItems("Accidents_AddIvestigationProtocols", "Accidents");

                //Hide the navigation buttons
                HideNavigationControls(btnCancel);

                this.SetupDatePickers(); //Setup any calendar control on the screen

            }

            this.SetBtnPrintInvestigationProtocol(); // Set visibility of the print button

            this.SetupPageUI(); //setup user interface elements according to rights of the user's role

            this.SetPageName(); // sets page titles according to mode of work(add or edit protocol)

            //Set limit in textarea
            CommonFunctions.SetTextAreaEvents(this.txtInjured, 4000);
            CommonFunctions.SetTextAreaEvents(this.txtAccidentDateAndPlace, 4000);
            CommonFunctions.SetTextAreaEvents(this.txtWitnesses, 4000);
            CommonFunctions.SetTextAreaEvents(this.txtDeviationOfNormalActivity, 4000);

            LnkForceNoChangesCheck(btnSave);

        }
        #endregion

        #region Methods

        // Setup user interface elements according to rights of the user's role
        private void SetupPageUI()
        {
            if (this.InvestigaitonProtocolId == 0) // add mode of page
            {
                bool screenHidden = (this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL") != UIAccessLevel.Enabled) ||
                                      (this.GetUIItemAccessLevel("HS_INVPROTOCOLS") != UIAccessLevel.Enabled);

                bool screenDisabled = (this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL") == UIAccessLevel.Disabled) ||
                                      (this.GetUIItemAccessLevel("HS_INVPROTOCOLS") == UIAccessLevel.Disabled);

                if (screenHidden)
                    RedirectAccessDenied();

                if (screenDisabled)
                {
                    this.pageDisabledControls.Add(btnSave);

                    //tableEditPermission = false;
                    //tableDeletePermission = false;
                }

                UIAccessLevel l;

                //Enable/Disable Server Controls

                l = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_INVESTIGAITONPROTOCOLNUMBER");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblInvestigaitonProtocolNumber);
                    this.pageDisabledControls.Add(txtInvestigaitonProtocolNumber);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblInvestigaitonProtocolNumber);
                    this.pageHiddenControls.Add(txtInvestigaitonProtocolNumber);
                }

                l = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_INVPROTDATE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblInvProtDate);
                    this.pageDisabledControls.Add(txtInvProtDate);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblInvProtDate);
                    this.pageHiddenControls.Add(txtInvProtDate);
                }

                l = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_DECLARATIONID");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblAccDateTime);
                    this.pageDisabledControls.Add(txtAccDateTime);

                    this.pageDisabledControls.Add(lblWorkerFullName);
                    this.pageDisabledControls.Add(txtWorkerFullName);

                    // this.pageDisabledControls.Add(btnDeclarationId);

                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblAccDateTime);
                    this.pageHiddenControls.Add(txtAccDateTime);

                    this.pageHiddenControls.Add(lblWorkerFullName);
                    this.pageHiddenControls.Add(txtWorkerFullName);

                    // this.pageHiddenControls.Add(btnDeclarationId);
                }

                l = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_DATEFROM");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblDateFrom);
                    this.pageDisabledControls.Add(txtDateFrom);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblDateFrom);
                    this.pageHiddenControls.Add(txtDateFrom);
                }

                l = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_DATETO");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblDateTo);
                    this.pageDisabledControls.Add(txtDateTo);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblDateTo);
                    this.pageHiddenControls.Add(txtDateTo);
                }

                l = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_LEGALREASON");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblLegalReason);
                    this.pageDisabledControls.Add(txtLegalReason);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblLegalReason);
                    this.pageHiddenControls.Add(txtLegalReason);
                }

                l = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_ORDERNUM");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblOrderNum);
                    this.pageDisabledControls.Add(txtOrderNum);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblOrderNum);
                    this.pageHiddenControls.Add(txtOrderNum);
                }

                l = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_COMMISSIONCHAIRMAN");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblCommissionChairman);
                    this.pageDisabledControls.Add(txtCommissionChairman);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblCommissionChairman);
                    this.pageHiddenControls.Add(txtCommissionChairman);
                }

                l = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_COMMISSIONMEMBERS");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblCommissionMembers);
                    this.pageDisabledControls.Add(txtCommissionMember1);
                    this.pageDisabledControls.Add(txtCommissionMember2);
                    this.pageDisabledControls.Add(txtCommissionMember3);
                    this.pageDisabledControls.Add(txtCommissionMember4);
                    this.pageDisabledControls.Add(txtCommissionMember5);
                    this.pageDisabledControls.Add(lblCommissionStaff);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblCommissionMembers);
                    this.pageHiddenControls.Add(txtCommissionMember1);
                    this.pageHiddenControls.Add(txtCommissionMember2);
                    this.pageHiddenControls.Add(txtCommissionMember3);
                    this.pageHiddenControls.Add(txtCommissionMember4);
                    this.pageHiddenControls.Add(txtCommissionMember5);
                    this.pageHiddenControls.Add(lblCommissionStaff);
                }

                l = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_INJURED");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblInjured);
                    this.pageDisabledControls.Add(txtInjured);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblInjured);
                    this.pageHiddenControls.Add(txtInjured);
                }

                l = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_ACCIDENTDATEANDPLACE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblAccidentDateAndPlace);
                    this.pageDisabledControls.Add(txtAccidentDateAndPlace);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblAccidentDateAndPlace);
                    this.pageHiddenControls.Add(txtAccidentDateAndPlace);
                }

                l = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_WITNESSES");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblWitnesses);
                    this.pageDisabledControls.Add(txtWitnesses);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblWitnesses);
                    this.pageHiddenControls.Add(txtWitnesses);
                }

                l = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_JOBGENERALDESC");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblJobGeneralDesc);
                    this.pageDisabledControls.Add(txtJobGeneralDesc);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblJobGeneralDesc);
                    this.pageHiddenControls.Add(txtJobGeneralDesc);
                }

                l = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_SPECIFICTASKACTIVITY");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblSpecificTaskActivity);
                    this.pageDisabledControls.Add(txtSpecificTaskActivity);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblSpecificTaskActivity);
                    this.pageHiddenControls.Add(txtSpecificTaskActivity);
                }

                l = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_DEVIATIONOFNORMALACTIVITY");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblDeviationOfNormalActivity);
                    this.pageDisabledControls.Add(txtDeviationOfNormalActivity);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblDeviationOfNormalActivity);
                    this.pageHiddenControls.Add(txtDeviationOfNormalActivity);
                }

                l = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_INJURYDETAILS");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblInjuryDetails);
                    this.pageDisabledControls.Add(txtInjuryDetails);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblInjuryDetails);
                    this.pageHiddenControls.Add(txtInjuryDetails);
                }

                l = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_ANALYSISOFACCIDENTCAUSES");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblAnalysisOfAccidentCauses);
                    this.pageDisabledControls.Add(txtAnalysisOfAccidentCauses);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblAnalysisOfAccidentCauses);
                    this.pageHiddenControls.Add(txtAnalysisOfAccidentCauses);
                }

                l = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_LEGALVIOLATIONS");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblLegalViolations);
                    this.pageDisabledControls.Add(txtLegalViolations);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblLegalViolations);
                    this.pageHiddenControls.Add(txtLegalViolations);
                }

                l = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_ITRUDERS");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblItruders);
                    this.pageDisabledControls.Add(txtItruders);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblItruders);
                    this.pageHiddenControls.Add(txtItruders);
                }

                l = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_ACTIONSTOAVOID");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblActionsToAvoid);
                    this.pageDisabledControls.Add(txtActionsToAvoid);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblActionsToAvoid);
                    this.pageHiddenControls.Add(txtActionsToAvoid);
                }

                l = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_ENCLOSURES");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblEnclosures);
                    this.pageDisabledControls.Add(txtEnclosures);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblEnclosures);
                    this.pageHiddenControls.Add(txtEnclosures);
                }

                //Enable/Disable Client Controls

                List<string> disabledClientControls = new List<string>();
                List<string> hiddenClientControls = new List<string>();

                l = this.GetUIItemAccessLevel("HS_ADDINVPROTOCOL_DECLARATIONID");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    //disabledClientControls.Add("btnDeclarationId");
                    hiddenClientControls.Add("btnDeclarationId"); //We just hide image 

                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("btnDeclarationId");

                }

                SetDisabledClientControls(disabledClientControls.ToArray());
                SetHiddenClientControls(hiddenClientControls.ToArray());
            }
            else // edit mode of page
            {
                bool screenHidden = (this.GetUIItemAccessLevel("HS_EDITINVPROTOCOL") == UIAccessLevel.Hidden) ||
                                      (this.GetUIItemAccessLevel("HS_INVPROTOCOLS") == UIAccessLevel.Hidden);

                bool screenDisabled = (this.GetUIItemAccessLevel("HS_EDITINVPROTOCOL") == UIAccessLevel.Disabled) ||
                                      (this.GetUIItemAccessLevel("HS_INVPROTOCOLS") == UIAccessLevel.Disabled);

                if (screenHidden)
                    RedirectAccessDenied();

                if (screenDisabled)
                {
                    this.pageDisabledControls.Add(btnSave);
                    // tableEditPermission = false;
                }

                UIAccessLevel l;

                l = this.GetUIItemAccessLevel("HS_EDITINVPROTOCOL_INVESTIGAITONPROTOCOLNUMBER");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblInvestigaitonProtocolNumber);
                    this.pageDisabledControls.Add(txtInvestigaitonProtocolNumber);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblInvestigaitonProtocolNumber);
                    this.pageHiddenControls.Add(txtInvestigaitonProtocolNumber);
                }

                l = this.GetUIItemAccessLevel("HS_EDITINVPROTOCOL_INVPROTDATE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblInvProtDate);
                    this.pageDisabledControls.Add(txtInvProtDate);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblInvProtDate);
                    this.pageHiddenControls.Add(txtInvProtDate);
                }

                l = this.GetUIItemAccessLevel("HS_EDITINVPROTOCOL_DECLARATIONID");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblAccDateTime);
                    this.pageDisabledControls.Add(txtAccDateTime);

                    this.pageDisabledControls.Add(lblWorkerFullName);
                    this.pageDisabledControls.Add(txtWorkerFullName);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblAccDateTime);
                    this.pageHiddenControls.Add(txtAccDateTime);

                    this.pageHiddenControls.Add(lblWorkerFullName);
                    this.pageHiddenControls.Add(txtWorkerFullName);
                }

                l = this.GetUIItemAccessLevel("HS_EDITINVPROTOCOL_DATEFROM");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblDateFrom);
                    this.pageDisabledControls.Add(txtDateFrom);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblDateFrom);
                    this.pageHiddenControls.Add(txtDateFrom);
                }

                l = this.GetUIItemAccessLevel("HS_EDITINVPROTOCOL_DATETO");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblDateTo);
                    this.pageDisabledControls.Add(txtDateTo);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblDateTo);
                    this.pageHiddenControls.Add(txtDateTo);
                }

                l = this.GetUIItemAccessLevel("HS_EDITINVPROTOCOL_LEGALREASON");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblLegalReason);
                    this.pageDisabledControls.Add(txtLegalReason);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblLegalReason);
                    this.pageHiddenControls.Add(txtLegalReason);
                }

                l = this.GetUIItemAccessLevel("HS_EDITINVPROTOCOL_ORDERNUM");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblOrderNum);
                    this.pageDisabledControls.Add(txtOrderNum);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblOrderNum);
                    this.pageHiddenControls.Add(txtOrderNum);
                }

                l = this.GetUIItemAccessLevel("HS_EDITINVPROTOCOL_COMMISSIONCHAIRMAN");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblCommissionChairman);
                    this.pageDisabledControls.Add(txtCommissionChairman);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblCommissionChairman);
                    this.pageHiddenControls.Add(txtCommissionChairman);
                }

                l = this.GetUIItemAccessLevel("HS_EDITINVPROTOCOL_COMMISSIONMEMBERS");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblCommissionMembers);
                    this.pageDisabledControls.Add(txtCommissionMember1);
                    this.pageDisabledControls.Add(txtCommissionMember2);
                    this.pageDisabledControls.Add(txtCommissionMember3);
                    this.pageDisabledControls.Add(txtCommissionMember4);
                    this.pageDisabledControls.Add(txtCommissionMember5);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblCommissionMembers);
                    this.pageHiddenControls.Add(txtCommissionMember1);
                    this.pageHiddenControls.Add(txtCommissionMember2);
                    this.pageHiddenControls.Add(txtCommissionMember3);
                    this.pageHiddenControls.Add(txtCommissionMember4);
                    this.pageHiddenControls.Add(txtCommissionMember5);
                }

                l = this.GetUIItemAccessLevel("HS_EDITINVPROTOCOL_INJURED");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblInjured);
                    this.pageDisabledControls.Add(txtInjured);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblInjured);
                    this.pageHiddenControls.Add(txtInjured);
                }

                l = this.GetUIItemAccessLevel("HS_EDITINVPROTOCOL_ACCIDENTDATEANDPLACE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblAccidentDateAndPlace);
                    this.pageDisabledControls.Add(txtAccidentDateAndPlace);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblAccidentDateAndPlace);
                    this.pageHiddenControls.Add(txtAccidentDateAndPlace);
                }

                l = this.GetUIItemAccessLevel("HS_EDITINVPROTOCOL_WITNESSES");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblWitnesses);
                    this.pageDisabledControls.Add(txtWitnesses);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblWitnesses);
                    this.pageHiddenControls.Add(txtWitnesses);
                }

                l = this.GetUIItemAccessLevel("HS_EDITINVPROTOCOL_JOBGENERALDESC");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblJobGeneralDesc);
                    this.pageDisabledControls.Add(txtJobGeneralDesc);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblJobGeneralDesc);
                    this.pageHiddenControls.Add(txtJobGeneralDesc);
                }

                l = this.GetUIItemAccessLevel("HS_EDITINVPROTOCOL_SPECIFICTASKACTIVITY");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblSpecificTaskActivity);
                    this.pageDisabledControls.Add(txtSpecificTaskActivity);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblSpecificTaskActivity);
                    this.pageHiddenControls.Add(txtSpecificTaskActivity);
                }

                l = this.GetUIItemAccessLevel("HS_EDITINVPROTOCOL_DEVIATIONOFNORMALACTIVITY");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblDeviationOfNormalActivity);
                    this.pageDisabledControls.Add(txtDeviationOfNormalActivity);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblDeviationOfNormalActivity);
                    this.pageHiddenControls.Add(txtDeviationOfNormalActivity);
                }

                l = this.GetUIItemAccessLevel("HS_EDITINVPROTOCOL_INJURYDETAILS");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblInjuryDetails);
                    this.pageDisabledControls.Add(txtInjuryDetails);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblInjuryDetails);
                    this.pageHiddenControls.Add(txtInjuryDetails);
                }

                l = this.GetUIItemAccessLevel("HS_EDITINVPROTOCOL_ANALYSISOFACCIDENTCAUSES");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblAnalysisOfAccidentCauses);
                    this.pageDisabledControls.Add(txtAnalysisOfAccidentCauses);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblAnalysisOfAccidentCauses);
                    this.pageHiddenControls.Add(txtAnalysisOfAccidentCauses);
                }

                l = this.GetUIItemAccessLevel("HS_EDITINVPROTOCOL_LEGALVIOLATIONS");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblLegalViolations);
                    this.pageDisabledControls.Add(txtLegalViolations);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblLegalViolations);
                    this.pageHiddenControls.Add(txtLegalViolations);
                }

                l = this.GetUIItemAccessLevel("HS_EDITINVPROTOCOL_ITRUDERS");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblItruders);
                    this.pageDisabledControls.Add(txtItruders);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblItruders);
                    this.pageHiddenControls.Add(txtItruders);
                }

                l = this.GetUIItemAccessLevel("HS_EDITINVPROTOCOL_ACTIONSTOAVOID");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblActionsToAvoid);
                    this.pageDisabledControls.Add(txtActionsToAvoid);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblActionsToAvoid);
                    this.pageHiddenControls.Add(txtActionsToAvoid);
                }

                l = this.GetUIItemAccessLevel("HS_EDITINVPROTOCOL_ENCLOSURES");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    this.pageDisabledControls.Add(lblEnclosures);
                    this.pageDisabledControls.Add(txtEnclosures);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    this.pageHiddenControls.Add(lblEnclosures);
                    this.pageHiddenControls.Add(txtEnclosures);
                }


                //Enable/Disable Client Controls

                List<string> disabledClientControls = new List<string>();
                List<string> hiddenClientControls = new List<string>();

                l = this.GetUIItemAccessLevel("HS_EDITINVPROTOCOL_DECLARATIONID");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    //  disabledClientControls.Add("btnDeclarationId");
                    hiddenClientControls.Add("btnDeclarationId"); //We just hide image 
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("btnDeclarationId");
                }

                SetDisabledClientControls(disabledClientControls.ToArray());
                SetHiddenClientControls(hiddenClientControls.ToArray());
            }

        }

        //inizialize value in private fileds
        private void ObtainFilterFields()
        {
            //Create filter object
            declarationOfAccidentFilter = new DeclarationOfAccidentFilter();

            //Set Filter value
            declarationNumber = (Request.Params["txtDeclarationNumber"] != null) ? Request.Params["txtDeclarationNumber"] : null;

            workerFullName = (Request.Params["txtWorkerFullName"] != null) ? Request.Params["txtWorkerFullName"] : null;

            if (Request.Params["txtDeclarationDateFrom"] != null)
            {
                if (CommonFunctions.TryParseDate(Request.Params["txtDeclarationDateFrom"]))
                {
                    declarationDateFrom = CommonFunctions.ParseDate(Request.Params["txtDeclarationDateFrom"]);
                }
            }
            else
            {
                declarationDateFrom = null;
            }

            if (Request.Params["txtDeclarationDateTo"] != null)
            {
                if (CommonFunctions.TryParseDate(Request.Params["txtDeclarationDateTo"]))
                {
                    declarationDateTo = CommonFunctions.ParseDate(Request.Params["txtDeclarationDateTo"]);
                }
            }
            else
            {
                declarationDateTo = null;
            }

            declarationOfAccidentFilter.DeclarationNumber = declarationNumber;
            declarationOfAccidentFilter.DeclarationDateFrom = declarationDateFrom;
            declarationOfAccidentFilter.DeclarationDateTo = declarationDateTo;
            declarationOfAccidentFilter.WorkerFullName = workerFullName;

            //Set paging value
            int orderBy;
            if (Request.Params["hdnOrderBy"] != null)
            {
                orderBy = Convert.ToInt32(Request.Params["hdnOrderBy"].ToString());
            }
            else
            {
                orderBy = 1;
            }


            declarationOfAccidentFilter.OrderBy = orderBy;

            int pageIndex;
            if (Request.Params["hdnPageIndex"] != null)
            {
                pageIndex = Convert.ToInt32(Request.Params["hdnPageIndex"].ToString());
            }
            else
            {
                pageIndex = 1;
            }

            declarationOfAccidentFilter.PageIndex = pageIndex;

            declarationOfAccidentFilter.PageCount = pageLength; //Get from webconfig

            // Set information use to buil pagination label

            //get number of all rows
            int allRows = DeclarationOfAccidentUtil.CountDeclarationOfAccident(declarationOfAccidentFilter, CurrentUser);
            //calculate number of pages - use in pagination label
            this.maxPage = allRows / pageLength + (allRows != 0 && allRows % pageLength == 0 ? 0 : 1);

        }

        //Bind UI elements with data
        private void FillTableWithProtocolData()
        {
            if (this.InvestigaitonProtocolId > 0) //UPDATE
            {
                //Create object
                investigationProtocol = new InvestigationProtocol(CurrentUser);
                //Fill object
                investigationProtocol = InvestigationProtocolUtil.GetInvestigationProtocol(this.InvestigaitonProtocolId, this.CurrentUser);
                //Set value in hidden filed 
                this.hfDeclarationId.Value = investigationProtocol.DeclarationId.ToString();
                this.txtDeclarationId.Text = investigationProtocol.DeclarationId.ToString();


                DeclarationOfAccident declarationOfAccident = investigationProtocol.DeclarationOfAccident;

                //Chek if we have any data
                if (declarationOfAccident != null)
                {
                    //Fill UI elements from InvestigationProtocol.DeclarationOfAccident Property
                    if (declarationOfAccident.DeclarationOfAccidentAcc.AccDateTime.HasValue)
                    {
                        txtAccDateTime.Text = CommonFunctions.FormatDate(declarationOfAccident.DeclarationOfAccidentAcc.AccDateTime.ToString());
                    }
                    else
                    {
                        txtAccDateTime.Text = "";
                    }
                    txtWorkerFullName.Text = declarationOfAccident.DeclarationOfAccidentWorker.WorkerFullName;
                }
                else
                {
                    txtAccDateTime.Text = "";
                    txtWorkerFullName.Text = "";
                }
                //Fill UI elements from InvestigationProtocol Object
                txtAccidentDateAndPlace.Text = investigationProtocol.AccidentDateAndPlace;
                txtActionsToAvoid.Text = investigationProtocol.ActionsToAvoid;
                txtAnalysisOfAccidentCauses.Text = investigationProtocol.AnalysisOfAccidentCauses;
                txtCommissionChairman.Text = investigationProtocol.CommissionChairman;
                txtCommissionMember1.Text = investigationProtocol.CommissionMember1;
                txtCommissionMember2.Text = investigationProtocol.CommissionMember2;
                txtCommissionMember3.Text = investigationProtocol.CommissionMember3;
                txtCommissionMember4.Text = investigationProtocol.CommissionMember4;
                txtCommissionMember5.Text = investigationProtocol.CommissionMember5;
                if (investigationProtocol.DateFrom.HasValue)
                {
                    txtDateFrom.Text = CommonFunctions.FormatDate(investigationProtocol.DateFrom.ToString());
                }
                else
                {
                    txtDateFrom.Text = "";
                }
                if (investigationProtocol.DateTo.HasValue)
                {
                    txtDateTo.Text = CommonFunctions.FormatDate(investigationProtocol.DateTo.ToString());
                }
                else
                {
                    txtDateTo.Text = "";
                }

                txtDeviationOfNormalActivity.Text = investigationProtocol.DeviationOfNormalActivity;
                txtEnclosures.Text = investigationProtocol.Enclosures;
                txtInjured.Text = investigationProtocol.Injured;
                txtInjuryDetails.Text = investigationProtocol.InjuryDetails;
                txtInvestigaitonProtocolNumber.Text = investigationProtocol.InvestigaitonProtocolNumber;
                if (investigationProtocol.InvProtDate.HasValue)
                {
                    txtInvProtDate.Text = CommonFunctions.FormatDate(investigationProtocol.InvProtDate.ToString());
                }
                else
                {
                    txtInvProtDate.Text = "";
                }
                txtItruders.Text = investigationProtocol.Itruders;
                txtJobGeneralDesc.Text = investigationProtocol.JobGeneralDesc;
                txtLegalReason.Text = investigationProtocol.LegalReason;
                txtLegalViolations.Text = investigationProtocol.LegalViolations;
                txtOrderNum.Text = investigationProtocol.OrderNum;
                txtSpecificTaskActivity.Text = investigationProtocol.SpecificTaskActivity;
                txtWitnesses.Text = investigationProtocol.Witnesses;

            }
            else //INSERT
            {
                txtAccDateTime.Text = string.Empty;
                txtWorkerFullName.Text = string.Empty;

                txtAccidentDateAndPlace.Text = string.Empty;
                txtActionsToAvoid.Text = string.Empty;
                txtAnalysisOfAccidentCauses.Text = string.Empty;
                txtCommissionChairman.Text = string.Empty;
                txtCommissionMember1.Text = string.Empty;
                txtCommissionMember2.Text = string.Empty;
                txtCommissionMember3.Text = string.Empty;
                txtCommissionMember4.Text = string.Empty;
                txtCommissionMember5.Text = string.Empty;
                txtDateFrom.Text = string.Empty;
                txtDateTo.Text = string.Empty;
                txtDeviationOfNormalActivity.Text = string.Empty;
                txtEnclosures.Text = string.Empty;
                txtInjured.Text = string.Empty;
                txtInjuryDetails.Text = string.Empty;
                txtInvestigaitonProtocolNumber.Text = string.Empty;
                // txtInvProtDate.Text = PMIS.Common.CommonFunctions.FormatDate(DateTime.Now.ToString());
                txtInvProtDate.Text = string.Empty;
                txtItruders.Text = string.Empty;
                txtJobGeneralDesc.Text = string.Empty;
                txtLegalReason.Text = string.Empty;
                txtLegalViolations.Text = string.Empty;
                txtOrderNum.Text = string.Empty;
                txtSpecificTaskActivity.Text = string.Empty;
                txtWitnesses.Text = string.Empty;

            }
        }

        //Set page dinamycaly title
        private void SetPageName()
        {
            if (this.InvestigaitonProtocolId > 0)
            {
                lblHeaderCell.Text = "Редактиране на протокол за резултатите от разследване на злополука";
                Page.Title = lblHeaderCell.Text;
            }
            else
            {
                lblHeaderCell.Text = "Добавяне на нов протокол за резултатите от разследване на злополука";
                Page.Title = lblHeaderCell.Text;
            }
        }

        // Set visibility of print button
        private void SetBtnPrintInvestigationProtocol()
        {
            // if the assessment is new and not saved yet, it is not allowed to print it
            if (this.InvestigaitonProtocolId == 0)
            {
                this.btnPrintInvestigationProtocol.Visible = false;
            }
            else
            {
                this.btnPrintInvestigationProtocol.Visible = true;
            }
        }

        //Save data
        private void SaveData()
        {

            //Create InvestigationProtocol object - (this.investigaitonProtocolId == 0 for INSERT)
            InvestigationProtocol investigationProtocol = new InvestigationProtocol(this.InvestigaitonProtocolId, CurrentUser);

            //Fill InvestigationProtocol with data from UI elemet
            investigationProtocol.AccidentDateAndPlace = this.txtAccidentDateAndPlace.Text;
            investigationProtocol.ActionsToAvoid = this.txtActionsToAvoid.Text;
            investigationProtocol.AnalysisOfAccidentCauses = this.txtAnalysisOfAccidentCauses.Text;
            investigationProtocol.CommissionChairman = this.txtCommissionChairman.Text;
            investigationProtocol.CommissionMember1 = this.txtCommissionMember1.Text;
            investigationProtocol.CommissionMember2 = this.txtCommissionMember2.Text;
            investigationProtocol.CommissionMember3 = this.txtCommissionMember3.Text;
            investigationProtocol.CommissionMember4 = this.txtCommissionMember4.Text;
            investigationProtocol.CommissionMember5 = this.txtCommissionMember5.Text;

            if (this.txtDateFrom.Text != "")
            {
                investigationProtocol.DateFrom = CommonFunctions.ParseDate(this.txtDateFrom.Text);
            }

            if (this.txtDateTo.Text != "")
            {
                investigationProtocol.DateTo = CommonFunctions.ParseDate(this.txtDateTo.Text);
            }
            investigationProtocol.DeviationOfNormalActivity = this.txtDeviationOfNormalActivity.Text;
            investigationProtocol.Enclosures = this.txtEnclosures.Text;
            investigationProtocol.Injured = this.txtInjured.Text;
            investigationProtocol.InjuryDetails = this.txtInjuryDetails.Text;
            investigationProtocol.InvestigaitonProtocolNumber = this.txtInvestigaitonProtocolNumber.Text;
            if (this.txtInvProtDate.Text != "")
            {
                investigationProtocol.InvProtDate = CommonFunctions.ParseDate(this.txtInvProtDate.Text);
            }
            investigationProtocol.Itruders = this.txtItruders.Text;
            investigationProtocol.JobGeneralDesc = txtJobGeneralDesc.Text;
            investigationProtocol.LegalReason = txtLegalReason.Text;
            investigationProtocol.LegalViolations = txtLegalViolations.Text;
            investigationProtocol.OrderNum = txtOrderNum.Text;
            investigationProtocol.SpecificTaskActivity = txtSpecificTaskActivity.Text;
            investigationProtocol.Witnesses = txtWitnesses.Text;

            //Set declaratioId property
            if (hfDeclarationId.Value != "")
            {
                investigationProtocol.DeclarationId = Convert.ToInt32(hfDeclarationId.Value);
            }

            //Set back UI to labels
            txtAccDateTime.Text = this.hdnAccDateTime.Value;
            txtWorkerFullName.Text = this.hdnWorkerFullName.Value;

            //Create Obect for log result in DB
            Change changeEntry = new Change(CurrentUser, "HS_InvProtocols");

            if (InvestigationProtocolUtil.SaveInvestigationProtocol(investigationProtocol, CurrentUser, changeEntry))
            {
                //Operation (INSERT or UPDATE) is success

                switch (this.InvestigaitonProtocolId)
                {
                    case 0: //INSERT
                        this.ShowMessage("Протокола за резултатите от злополука е добавен", true, "SuccessText");
                        // Set on investigaitonProtocolId property the new value!!!!
                        this.InvestigaitonProtocolId = investigationProtocol.InvestigaitonProtocolId;
                        //Set new value in hideen fields!!!!
                        this.hfInvestigaitonProtocolId.Value = investigationProtocol.InvestigaitonProtocolId.ToString();

                        //Refresh page name and title
                        this.SetPageName();
                        //Use this for page refresh(F5)
                        SetLocationHash("AddEditInvestigationProtocol.aspx?investigationProtocolId=" + investigationProtocol.InvestigaitonProtocolId.ToString());

                        break;
                    default: //UPDATE
                        this.ShowMessage("Данните на протокола за резултатите от злополука са обновени", true, "SuccessText");

                        break;
                }

                this.SetBtnPrintInvestigationProtocol(); // Set visibility of the print button

                this.SetupPageUI(); //setup user interface elements according to rights of the user's role

                changeEntry.WriteLog(); //Write changes in DB

                this.hdnSavedChanges.Value = "True";
            }
            else //Operation was not success
            {
                switch (this.InvestigaitonProtocolId)
                {
                    case 0:
                        this.ShowMessage("Протокола за резултатите от злополука не е добавен", true, "ErrorText");
                        break;
                    default:
                        this.ShowMessage("Данните на протокола за резултатите от злополука не са обновени", true, "ErrorText");
                        break;
                }
            }
        }

        //Get Ajax Html for LightBox
        private void GetLightBoxItems()
        {
            string response = "";
            response += GetHtmlLightBox();

            AJAX a = new AJAX(AJAXTools.EncodeForXML(response), this.Response);
            a.Write();
            Response.End();
        }

        //Set Html for LightBox
        private string GetHtmlLightBox()
        {
            string html = "";

            string htmlNoResults = "";

            List<DeclarationOfAccident> listDeclarationOfAccident = DeclarationOfAccidentUtil.GetAllDeclarationOfAccident(declarationOfAccidentFilter, CurrentUser);


            // No data found
            if (listDeclarationOfAccident.Count == 0)
            {
                htmlNoResults = "Няма намерени резултати";
            }

            //Set filter section
            html += @"<center>
                        <table width='100%' style='border-collapse: collapse; vertical-align: middle; color: #0B449D;'>
                        <tr style='height: 30px'>
                            <td align='left'>
                                <span class='InputLabel' style='padding-left: 10px'>№ Декларация:</span>
                                <input type='text' id='txtDeclarationNumber' style='width:50px'  class='InputField' MaxLength='10' value='" + declarationOfAccidentFilter.DeclarationNumber + @"'></input>
                                <span class='InputLabel' style='padding-left: 10px'>Име на пострадалия:</span>
<input type='text' id='txtWorkerFullName' style='width:180px' class='InputField' value='" + declarationOfAccidentFilter.WorkerFullName + @"'></input>
                            </td>
                        </tr>
                        <tr style='height: 30px'>
                            <td align='left'>
                                <span class='InputLabel' style='padding-left: 10px'>Дата на декларацията от:</span>
                               
 <input type='text' id='txtDeclarationDateFrom' class='" + CommonFunctions.DatePickerCSS() + @"' style='width: 75px'
                                maxlength='10' value='" + CommonFunctions.FormatDate(declarationOfAccidentFilter.DeclarationDateFrom) + @"'></input>

                                <span class='InputLabel' style='padding-left: 15px'>до</span>
                              <input type='text' id='txtDeclarationDateTo' class='" + CommonFunctions.DatePickerCSS() + @"' style='width: 75px'
                                maxlength='10' value='" + CommonFunctions.FormatDate(declarationOfAccidentFilter.DeclarationDateTo) + @"'></input>
                            </td>
                        </tr>
                        <tr style='height: 40px'>
                            <td align='center'>
<div id='btnSearch' class='Button' onclick='DoAjaxSearch()'
                                    ><i></i><div style='width:70px; padding-left:5px;'>Покажи</div><b></b></div>                            </td>
                        </tr>
                      </table>";
            //Set pagination section
            // Refresh the paging image buttons
            string btnFirst = "src='../Images/ButtonFirst.png'";
            string btnPrev = "src='../Images/ButtonPrev.png'";
            string btnNext = "src='../Images/ButtonNext.png'";
            string btnLast = "src='../Images/ButtonLast.png'";

            if (declarationOfAccidentFilter.PageIndex == 1)
            {
                btnFirst = "src='../Images/ButtonFirstDisabled.png' disabled='true'";
                btnPrev = "src='../Images/ButtonPrevDisabled.png' disabled='true'";
            }

            if (declarationOfAccidentFilter.PageIndex == maxPage)
            {
                btnLast = "src='../Images/ButtonLastDisabled.png' disabled='true'";
                btnNext = "src='../Images/ButtonNextDisabled.png' disabled='true'";
            }

            // Set current page number
            string pageTablePagination = " | " + declarationOfAccidentFilter.PageIndex + " от " + maxPage + " | ";

            // Setup the header of the grid
            html += @"<div style='min-height: 150px; margin-bottom: 10px;'>

                        <input type='hidden' id='hdnOrderBy' value='" + declarationOfAccidentFilter.OrderBy + @"' />
                        <input type='hidden' id='hdnPageIndex' value='" + declarationOfAccidentFilter.PageIndex + @"' />
                        <input type='hidden' id='hdnPageMaxPage' value='" + maxPage + @"' />

                        <span class='HeaderText'>Избор на декларация за злополука</span><br /><br /><br />

                        <div style='text-align: center;'>
                           <div style='display: inline; position: relative; top: -10px;'>
                              <img id='btnTableFirst' " + btnFirst + @" alt='Първа страница' title='Първа страница' class='PaginationButton' onclick=""BtnPagingClick('btnFirst');"" />
                              <img id='btnTablePrev' " + btnPrev + @" alt='Предишна страница' title='Предишна страница' class='PaginationButton' onclick=""BtnPagingClick('btnPrev');"" />
                              <span id='lblTablePagination' class='PaginationLabel'>" + pageTablePagination + @"</span>
                              <img id='btnTableNext' " + btnNext + @" alt='Следваща страница' title='Следваща страница' class='PaginationButton' onclick=""BtnPagingClick('btnNext');"" />
                              <img id='btnTableLast' " + btnLast + @" alt='Последна страница' title='Последна страница' class='PaginationButton' onclick=""BtnPagingClick('btnLast');"" />
                              
                              <span style='padding: 0 30px'>&nbsp;</span>
                              <span style='text-align: right;'>Отиди на страница</span>
                              <input id='txtTableGotoPage' class='InputField' type='text' style='width: 30px;' value='' />
                              <img id='btnTableGoto' src='../Images/ButtonGoto.png' alt='Отиди на страница' title='Отиди на страница' class='PaginationButton' onclick=""BtnPagingClick('btnPageGo');"" />
                           </div>
                        </div>

                        <table id='tblDeclarationAcc' class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            
                         </thead>";

            //Set Table Results
            string headerStyle = "vertical-align: bottom;";
            int orderCol = (declarationOfAccidentFilter.OrderBy > 100 ? declarationOfAccidentFilter.OrderBy - 100 : declarationOfAccidentFilter.OrderBy);
            string img = declarationOfAccidentFilter.OrderBy > 100 ? "../Images/desc.gif" : "../Images/asc.gif";
            string[] arrOrderCol = { "", "", "", "" };

            arrOrderCol[orderCol - 1] = @"<div style='position: relative;'><div class='SortImgDiv'><img src=""" + img + @""" /></div></div>";

            //Setup the header of the grid
            html += @"<table class='CommonHeaderTable' style='margin: 0 auto; text-align: left;'>
                         <thead>
                            <tr>
                               <th style='width: 20px;" + headerStyle + @"'>№</th>
                               <th style='width: 110px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(1);'>Декларация №" + arrOrderCol[0] + @"</th>
                               <th style='width: 140px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(2);'>Дата декларация" + arrOrderCol[1]
                                                                      + @"</th>
                               <th style='width: 190px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(3);'>Име на пострадал" + arrOrderCol[2] + @"</th>

<th style='width: 140px; cursor: pointer;" + headerStyle + @"' onclick='SortTableBy(4);'>Дата злополука" + arrOrderCol[3] + @"</th>

 </tr></thead>";

            int counter = 1;

            //Iterate through all items and add them into the grid
            foreach (DeclarationOfAccident declarationOfAccident in listDeclarationOfAccident)
            {

                string cellStyle = "vertical-align: top;";

                html += @"<tr  class='" + (counter % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + @"'  style='cursor: pointer;' onclick='SelectDeclarationId(" + declarationOfAccident.DeclarationId + @");'
title='Избор'>
                                 <td style='" + cellStyle + @"'>" + ((declarationOfAccidentFilter.PageIndex - 1) * declarationOfAccidentFilter.PageCount + counter).ToString() + @"</td>
                                 <td style='" + cellStyle + @"'>" + declarationOfAccident.DeclarationOfAccidentHeader.DeclarationNumber + @"</td>
                                 <td style='" + cellStyle + @"'>" + (declarationOfAccident.DeclarationOfAccidentHeader.DeclarationDate.HasValue ? CommonFunctions.FormatDate(declarationOfAccident.DeclarationOfAccidentHeader.DeclarationDate.Value) : "") + @"</td>
                                 <td style='" + cellStyle + @"'>" + declarationOfAccident.DeclarationOfAccidentWorker.WorkerFullName + @"</td>

<td style='" + cellStyle + @"'>" + (declarationOfAccident.DeclarationOfAccidentAcc.AccDateTime.HasValue ? CommonFunctions.FormatDate(declarationOfAccident.DeclarationOfAccidentAcc.AccDateTime.Value) : "") + @"</td>
                              </tr>";

                counter++;
            }
            html += "</table>";


            html += @"<div style='height: 10px;'>
                </div>

                 <div style='text-align: center'>
                   <span id='lblDeclarationMessage'>" + htmlNoResults + @"</span><br/>
                </div>
";

            html += @"</div>
                          <div id='btnCloseTable' runat='server' class='Button' onclick=""HideTableLightBox();""><i></i><div style='width:70px; padding-left:5px;'>Затвори</div><b></b></div></center>";



            return html;
        }

        //Validate data from UI elemements - return true or false
        private bool ValidateData()
        {
            bool isDataValid = true;
            string errMsg = "";
            List<string> errRightsFields = new List<string>();

            if (txtInvestigaitonProtocolNumber.Text.Trim() == "")
            {
                isDataValid = false;

                if (pageDisabledControls.Contains(txtInvestigaitonProtocolNumber) || pageHiddenControls.Contains(txtInvestigaitonProtocolNumber))
                    errRightsFields.Add("Номер на протокол");
                else
                    errMsg += CommonFunctions.GetErrorMessageMandatory("Номер на протокола") + "<br/>";
            }


            if (txtInvProtDate.Text.Trim() == "")
            {
                isDataValid = false;
                if (pageDisabledControls.Contains(txtInvProtDate) || pageHiddenControls.Contains(txtInvProtDate))
                    errRightsFields.Add("Дата на протокол");
                else
                    errMsg += CommonFunctions.GetErrorMessageMandatory("Дата на протокола") + "<br/>";
            }

            if (txtDateFrom.Text.Trim() != "" && !CommonFunctions.TryParseDate(txtDateFrom.Text))
            {
                isDataValid = false;
                errMsg += CommonFunctions.GetErrorMessageDate("Дата (от)") + "<br/>";
            }


            if (txtDateTo.Text.Trim() != "" && !CommonFunctions.TryParseDate(txtDateTo.Text))
            {
                isDataValid = false;
                errMsg += CommonFunctions.GetErrorMessageDate("Дата (до)") + "<br/>";
            }

            if (txtInvProtDate.Text.Trim() != "" && !CommonFunctions.TryParseDate(txtInvProtDate.Text))
            {
                isDataValid = false;
                errMsg += CommonFunctions.GetErrorMessageDate("Дата на протокола") + "<br/>";
            }


            if (errRightsFields.Count > 0)
            {
                errMsg = "<i>" + CommonFunctions.GetErrorMessageNoRights(errRightsFields.ToArray()) + "</i><br />" + errMsg;
            }

            if (!isDataValid)
            {
                this.lblMessage.CssClass = "ErrorText";
                this.lblMessage.Text = errMsg;
            }

            return isDataValid;

        }

        //Set DatePicker for UI date elemets
        private void SetupDatePickers()
        {
            this.txtInvProtDate.CssClass = "RequiredInputField " + CommonFunctions.DatePickerCSS();
            this.txtDateFrom.CssClass = CommonFunctions.DatePickerCSS();
            this.txtDateTo.CssClass = CommonFunctions.DatePickerCSS();
        }

        //Set message label value
        private void ShowMessage(string message, bool visible, string cssClass)
        {
            this.lblMessage.CssClass = cssClass;
            this.lblMessage.Text = message;
            //this.lblMessage.Visible = visible;
        }

        //AjaxMetod
        private void JSGetInvProtFields()
        {
            string response = "";

            int declrationId = Convert.ToInt32(Request.Params["declarationId"]);

            //Set value in hidden fields
            // this.hfDeclarationId.Value = declrationId.ToString();

            //Fill Object with data
            DeclarationOfAccident declarationOfAccident = DeclarationOfAccidentUtil.GetDeclarationOfAccidentForInvProtocol(declrationId, CurrentUser);

            string accDateTime = "";
            if (declarationOfAccident.DeclarationOfAccidentAcc.AccDateTime.HasValue)
            {
                accDateTime = CommonFunctions.FormatDate(declarationOfAccident.DeclarationOfAccidentAcc.AccDateTime.ToString());
            }
            response = "<AccDateTime>" + accDateTime + "</AccDateTime>";
            response += "<WorkerFullName>" + declarationOfAccident.DeclarationOfAccidentWorker.WorkerFullName + "</WorkerFullName>";
            response += "<declrationId>" + declrationId + "</declrationId>";

            AJAX a = new AJAX(response, this.Response);
            a.Write();
            Response.End();

        }

        #endregion

        #region ObjectEvents

        //Redirect to ManageInvestigationProtocol.aspx page
        protected void btnCancel_Click(object sender, EventArgs e)
        {
           
            if (FromHome != 1)
               Response.Redirect("~/ContentPages/ManageInvestigationProtocol.aspx", true); 
            else
                Response.Redirect("~/ContentPages/Home.aspx");

        }

        //Save Data
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                this.SaveData();
            }
        }

        #endregion
    }
}


