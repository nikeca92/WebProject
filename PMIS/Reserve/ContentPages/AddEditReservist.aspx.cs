using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

using PMIS.Common;
using PMIS.Reserve.Common;

namespace PMIS.Reserve.ContentPages
{
    public partial class AddEditReservist : RESPage
    {
        public override string PageUIKey
        {
            get
            {
                return "RES_HUMANRES";
            }
        }

        //Get-Set Id for reservist (0 - if new)
        public int ReservistId
        {
            get
            {
                int reservistId = 0;
                //gets reservistid either from the hidden field on the page or from page url
                if (String.IsNullOrEmpty(this.hdnReservistId.Value) || this.hdnReservistId.Value == "0")
                {
                    if (Request.Params["ReservistId"] != null)
                        Int32.TryParse(Request.Params["ReservistId"].ToString(), out reservistId);

                    //sets reservist ID in hidden field on the page in order to be accessible in javascript
                    this.hdnReservistId.Value = reservistId.ToString();
                }
                else
                {
                    Int32.TryParse(this.hdnReservistId.Value, out reservistId);
                }

                return reservistId;
            }
            set { this.hdnReservistId.Value = value.ToString(); }
        }

        private string MilitaryUnitLabel = CommonFunctions.GetLabelText("MilitaryUnit");

        protected void Page_Load(object sender, EventArgs e)
        {
            //Redirect if the access is denied
            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden)
            {
                RedirectAccessDenied();
            }

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSLoadTab")
            {
                if ((Request.Params["SelectedTabId"] != null) && (Request.Params["ReservistId"] != null))
                {
                    string selectesTabId = Request.Params["SelectedTabId"];
                    JSDisplayTab(selectesTabId);
                }
            }

            jsMilitaryUnitSelectorDiv.InnerHtml = @"<script src=""" + Page.ClientScript.GetWebResourceUrl(typeof(MilitaryUnitSelector.MilitaryUnitSelector), "MilitaryUnitSelector.MilitaryUnitSelector.js") + @""" type=""text/javascript""></script>";
            jsItemSelectorDiv.InnerHtml = @"<script src=""" + Page.ClientScript.GetWebResourceUrl(typeof(ItemSelector.ItemSelector), "ItemSelector.ItemSelector.js") + @""" type=""text/javascript""></script>";

            //Hilight the correct item in the menu
            HighlightMenuItems("HumanResources", "AddNewReservist");

            //Hide the navigation buttons
            //HideNavigationControls(btnBack);

            Reservist reservist = null;

            if (!IsPostBack)
            {
                if (!String.IsNullOrEmpty(Request.Params["ReservistId"]) &&
                   ReservistId == 0)
                    ReservistId = int.Parse(Request.Params["ReservistId"]);

                reservist = ReservistUtil.GetReservist(ReservistId, CurrentUser);

                if (!String.IsNullOrEmpty(Request.Params["Preview"]))
                    this.hdnIsPreview.Value = "1";

                if (reservist != null && (!reservist.CanEdit || !reservist.CanAccessMilitaryDepartment(CurrentUser)))
                    this.hdnIsPreview.Value = "1";

                string header = (ReservistId > 0 ? "Редактиране на резервист" : "Добавяне на нов резервист");
                lblHeaderTitle.InnerHtml = header;
                this.Title = header;
               
            }

            SetupPageUI(reservist);

            if (!IsPostBack)
            {
                lboxMilitaryRank.InnerHtml = GetMilitaryRankLightBox();
                if (reservist != null)
                {
                    if (reservist.Person.MilitaryUnit != null)
                    {
                        lblCurrentMilitaryUnitValue.Text = reservist.Person.MilitaryUnit.DisplayTextForSelection;   
                    }

                    List<PersonMilitaryReportSpeciality> personMilitaryReportSpecialities = PersonMilitaryReportSpecialityUtil.GetAllPersonMilitaryReportSpecialities(reservist.PersonId, CurrentUser);
                    foreach (PersonMilitaryReportSpeciality personMilitaryReportSpecialitie in personMilitaryReportSpecialities)
                    {
                        if (personMilitaryReportSpecialitie.IsPrimary)
                        {
                            lblCurrentVosValue.Text = personMilitaryReportSpecialitie.MilitaryReportSpeciality.MilReportingSpecialityCode;
                            break;
                        }
                    }

                    List<PersonPositionTitle> personPositionTitles = PersonPositionTitleUtil.GetAllPersonPositionTitles(reservist.PersonId, CurrentUser);
                    foreach (PersonPositionTitle personPositionTitle in personPositionTitles)
                    {
                        if (personPositionTitle.IsPrimary)
                        {
                            lblCurrentPositionTitleValue.Text = personPositionTitle.PositionTitle.PositionTitleName;
                            break;
                        }
                    }
                }
            }
        }

        // Setup user interface elements according to rights of the user's role
        private void SetupPageUI(Reservist reservist)
        {
            //In this method it is implemented the UIItems logic only for the header part of the screen

            bool isPreview = false;
            if (!String.IsNullOrEmpty(Request.Params["Preview"]))
            {
                isPreview = true;
            }

            if (reservist != null && (!reservist.CanEdit || !reservist.CanAccessMilitaryDepartment(CurrentUser)))
            {
                isPreview = true;
            }

            bool screenHidden = false;
            bool screenDisabled = false;
            bool personalDataDisabled = false;
            bool personalDataHidden = false;
            bool educationWorkDisabled = false;
            bool educationWorkHidden = false;
            bool militaryReportDisabled = false;
            bool militaryReportHidden = false;
            bool militaryServiceDisabled = false;
            bool militaryServiceHidden = false;
            bool otherInfoDisabled = false;
            bool otherInfoHidden = false;

            //Setup the "manually add positions" light-box
            List<string> disabledClientControls = new List<string>();
            List<string> hiddenClientControls = new List<string>();

            if (ReservistId == 0) // add mode of page
            {
                screenHidden = GetUIItemAccessLevel("RES_HUMANRES") != UIAccessLevel.Enabled ||
                               GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST") != UIAccessLevel.Enabled;

                screenDisabled = GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Disabled ||
                                 GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST") == UIAccessLevel.Disabled;

                personalDataDisabled = GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Disabled ||
                                       GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST") == UIAccessLevel.Disabled ||
                                       GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA") == UIAccessLevel.Disabled;

                personalDataHidden = GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                                     GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST") == UIAccessLevel.Hidden ||
                                     GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA") == UIAccessLevel.Hidden;

                if (screenHidden)
                    RedirectAccessDenied();

                if (personalDataDisabled || personalDataHidden)
                {
                    hiddenClientControls.Add("btnSaveAllTabs");
                }

                UIAccessLevel l;

                l = GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_IDENTNUMBER");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblIdentNumber");
                    disabledClientControls.Add("txtIdentNumber");
                    disabledClientControls.Add("lblIdentNumberValue");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblIdentNumber");
                    hiddenClientControls.Add("txtIdentNumber");
                    hiddenClientControls.Add("lblIdentNumberValue");
                }

                l = GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_MILRANK");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblMilitaryRank");
                    disabledClientControls.Add("lblMilitaryRankValue");
                    hiddenClientControls.Add("imgEditMilitaryRank");
                    disabledClientControls.Add("lblMilitaryCategory");
                    disabledClientControls.Add("lblMilitaryCategoryValue");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblMilitaryRank");
                    hiddenClientControls.Add("lblMilitaryRankValue");
                    hiddenClientControls.Add("imgEditMilitaryRank");
                    hiddenClientControls.Add("lblMilitaryCategory");
                    hiddenClientControls.Add("lblMilitaryCategoryValue");
                }

                l = GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_MILRANK_DR");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblMilitaryRankDR");
                    disabledClientControls.Add("chkMilitaryRankDR");
                   
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblMilitaryRankDR");
                    hiddenClientControls.Add("chkMilitaryRankDR");                  
                }

                l = GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_FIRSTNAME");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblFirstName");
                    disabledClientControls.Add("txtFirstName");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblFirstName");
                    hiddenClientControls.Add("txtFirstName");
                }

                l = GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_LASTNAME");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblLastName");
                    disabledClientControls.Add("txtLastName");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblLastName");
                    hiddenClientControls.Add("txtLastName");
                }

                l = GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_INITIALS");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblInitials");
                    disabledClientControls.Add("txtInitials");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblInitials");
                    hiddenClientControls.Add("txtInitials");
                }

                l = GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_CURRMILREPSTAT");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblMilitaryReportStatus");
                    disabledClientControls.Add("lblMilitaryReportStatusValue");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblMilitaryReportStatus");
                    hiddenClientControls.Add("lblMilitaryReportStatusValue");
                }

                l = GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_LASTMODIFIED");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblLastModified");
                    disabledClientControls.Add("lblLastModifiedValue");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblLastModified");
                    hiddenClientControls.Add("lblLastModifiedValue");
                }

                l = GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_BASEVOS");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblCurrentVos");
                    disabledClientControls.Add(lblCurrentVosValue.ClientID);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblCurrentVos");
                    hiddenClientControls.Add(lblCurrentVosValue.ClientID);
                }


                l = GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA_BASEPOSITIONTITLE");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblCurrentPositionTitle");
                    disabledClientControls.Add(lblCurrentPositionTitleValue.ClientID);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblCurrentPositionTitle");
                    hiddenClientControls.Add(lblCurrentPositionTitleValue.ClientID);
                }
            }
            else // edit mode of page
            {
                screenHidden = GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                               GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden;

                screenDisabled = GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Disabled ||
                                 GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Disabled || isPreview;

                personalDataDisabled = GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Disabled ||
                                       GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Disabled ||
                                       GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA") == UIAccessLevel.Disabled;

                personalDataHidden = GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                                     GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                                     GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA") == UIAccessLevel.Hidden;

                educationWorkDisabled = GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Disabled ||
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Disabled ||
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") == UIAccessLevel.Disabled;

                educationWorkHidden = GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                                      GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                                      GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") == UIAccessLevel.Hidden;

                militaryReportDisabled = GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Disabled ||
                                         GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Disabled ||
                                         GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP") == UIAccessLevel.Disabled;

                militaryReportHidden = GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                                       GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                                       GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP") == UIAccessLevel.Hidden;


                militaryServiceDisabled = GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Disabled ||
                                          GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Disabled ||
                                          GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV") == UIAccessLevel.Disabled;


                militaryServiceHidden = GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                                        GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV") == UIAccessLevel.Hidden;


                otherInfoDisabled = GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Disabled ||
                                    GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Disabled ||
                                    GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_OTHERDATA") == UIAccessLevel.Disabled;


                otherInfoHidden = GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                                  GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                                  GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_OTHERDATA") == UIAccessLevel.Hidden;

                
                if (screenHidden)
                    RedirectAccessDenied();

                if ((personalDataDisabled || personalDataHidden) &&
                    (militaryReportDisabled || militaryReportHidden) &&
                    (militaryServiceDisabled || militaryServiceHidden) &&
                    (educationWorkDisabled || educationWorkHidden) &&
                    (otherInfoDisabled || otherInfoHidden))
                {
                    hiddenClientControls.Add("btnSaveAllTabs");
                }

                UIAccessLevel l;
                
                l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_IDENTNUMBER");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblIdentNumber");
                    disabledClientControls.Add("txtIdentNumber");
                    disabledClientControls.Add("lblIdentNumberValue");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblIdentNumber");
                    hiddenClientControls.Add("txtIdentNumber");
                    hiddenClientControls.Add("lblIdentNumberValue");
                }

                l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_MILRANK");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblMilitaryRank");
                    disabledClientControls.Add("lblMilitaryRankValue");
                    hiddenClientControls.Add("imgEditMilitaryRank");
                    disabledClientControls.Add("lblMilitaryCategory");
                    disabledClientControls.Add("lblMilitaryCategoryValue");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblMilitaryRank");
                    hiddenClientControls.Add("lblMilitaryRankValue");
                    hiddenClientControls.Add("imgEditMilitaryRank");
                    hiddenClientControls.Add("lblMilitaryCategory");
                    hiddenClientControls.Add("lblMilitaryCategoryValue");
                }

                l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_MILRANK_DR");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblMilitaryRankDR");
                    disabledClientControls.Add("chkMilitaryRankDR");                    
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblMilitaryRankDR");
                    hiddenClientControls.Add("chkMilitaryRankDR");                   
                }

                l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_FIRSTNAME");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblFirstName");
                    disabledClientControls.Add("txtFirstName");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblFirstName");
                    hiddenClientControls.Add("txtFirstName");
                }

                l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_LASTNAME");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblLastName");
                    disabledClientControls.Add("txtLastName");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblLastName");
                    hiddenClientControls.Add("txtLastName");
                }

                l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_INITIALS");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblInitials");
                    disabledClientControls.Add("txtInitials");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblInitials");
                    hiddenClientControls.Add("txtInitials");
                }

                l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_CURRMILREPSTAT");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblMilitaryReportStatus");
                    disabledClientControls.Add("lblMilitaryReportStatusValue");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblMilitaryReportStatus");
                    hiddenClientControls.Add("lblMilitaryReportStatusValue");
                }

                l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_LASTMODIFIED");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblLastModified");
                    disabledClientControls.Add("lblLastModifiedValue");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblLastModified");
                    hiddenClientControls.Add("lblLastModifiedValue");
                }

                l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_BASEVOS");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblCurrentVos");
                    disabledClientControls.Add(lblCurrentVosValue.ClientID);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblCurrentVos");
                    hiddenClientControls.Add(lblCurrentVosValue.ClientID);
                }


                l = GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_BASEPOSITIONTITLE");

                if (l == UIAccessLevel.Disabled || screenDisabled || personalDataDisabled)
                {
                    disabledClientControls.Add("lblCurrentPositionTitle");
                    disabledClientControls.Add(lblCurrentPositionTitleValue.ClientID);
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblCurrentPositionTitle");
                    hiddenClientControls.Add(lblCurrentPositionTitleValue.ClientID);
                }
            }

            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_RECORDOFSERVICEARCHIVE") == UIAccessLevel.Hidden)
                hdnIsRecordOfServiceArchiveHidden.Value = "1";

            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Disabled ||
                GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Disabled ||
                GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA") == UIAccessLevel.Disabled ||
                GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_RECORDOFSERVICEARCHIVE") == UIAccessLevel.Disabled)
                hdnIsRecordOfServiceArchiveDisabled.Value = "1";

            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_CONVICTION") == UIAccessLevel.Hidden)
                hdnIsConvictionHidden.Value = "1";

            if (GetUIItemAccessLevel("RES_HUMANRES") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA") == UIAccessLevel.Hidden ||
                GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA_DUALCITIZENSHIP") == UIAccessLevel.Hidden)
                hdnIsDualCitizenshipHidden.Value = "1";

            SetDisabledClientControls(disabledClientControls.ToArray());
            SetHiddenClientControls(hiddenClientControls.ToArray());
        }
                
        //Load a particular tab's content
        private void JSDisplayTab(string selectedTabId)
        {
            string html = "";
            string UIItems = "";

            int reservistId = 0;
            try
            {
                reservistId = int.Parse(Request.Params["ReservistId"]);
            }
            catch
            {
                reservistId = 0;
            }

            //Select case to load data
            switch (selectedTabId)
            {
                case "btnTabPersonalData":
                    html = AddEditReservist_PersonalData_PageUtil.GetTabContent(ModuleKey, CurrentUser);
                    UIItems = AddEditReservist_PersonalData_PageUtil.GetTabUIItems(this);
                    break;
                case "btnTabEducationWork":
                    html = AddEditReservist_EducationWork_PageUtil.GetTabContent(this);
                    UIItems = AddEditReservist_EducationWork_PageUtil.GetTabWorkPlaceContentUIItems(this);
                    break;
                case "btnTabMilitaryReport":
                    html = AddEditReservist_MilitaryReport_PageUtil.GetTabContent(reservistId, ModuleKey, CurrentUser, this);
                    UIItems = AddEditReservist_MilitaryReport_PageUtil.GetTabUIItems(reservistId, CurrentUser, this);
                    break;
                case "btnTabMilitaryService":
                    html = AddEditReservist_MilitaryService_PageUtil.GetTabContent(this);
                    break;
                case "btnTabOtherInfo":
                    html = AddEditReservist_OtherInfo_PageUtil.GetTabContent(reservistId, CurrentUser);
                    UIItems = AddEditReservist_OtherInfo_PageUtil.GetTabUIItems(this);
                    break;
                default:
                    break;
            }

            string response = "<TabHTML>" + AJAXTools.EncodeForXML(html) + "</TabHTML>";

            if (!String.IsNullOrEmpty(UIItems))
            {
                response += "<UIItems>" + UIItems + "</UIItems>";
            }

            string stat = AJAXTools.OK;

            AJAX a = new AJAX(response, stat, Response);
            a.Write();
            Response.End();
        }

        public bool IsPersonalDataVisible()
        {
            bool visible = false;

            if (ReservistId == 0)
                visible = this.GetUIItemAccessLevel("RES_HUMANRES_ADDRESERVIST_PERSONALDATA") != UIAccessLevel.Hidden;
            else
                visible = this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_PERSONALDATA") != UIAccessLevel.Hidden;

            return visible;
        }

        public bool IsEducationWorkVisible()
        {
            return this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_EDUWORK") != UIAccessLevel.Hidden;
        }

        public bool IsMilitaryReportVisible()
        {
            return this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILREP") != UIAccessLevel.Hidden;
        }

        public bool IsMilitaryServiceVisible()
        {
            return this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_MILSERV") != UIAccessLevel.Hidden;
        }

        public bool IsOtherInfoVisible()
        {
            return this.GetUIItemAccessLevel("RES_HUMANRES_EDITRESERVIST_OTHERDATA") != UIAccessLevel.Hidden;
        }

        private string GetMilitaryRankLightBox()
        {
            // Generates html for drop down list MilitaryRank
            List<MilitaryRank> listMilitaryRank = MilitaryRankUtil.GetAllMilitaryRanks(CurrentUser);
            List<IDropDownItem> ddiMilitaryRank = new List<IDropDownItem>();

            foreach (MilitaryRank militaryRank in listMilitaryRank)
            {
                ddiMilitaryRank.Add(militaryRank);
            }

            // Generates html for drop down list
            string PersonMilitaryRankMilitaryRanksHTML = ListItems.GetDropDownHtml(ddiMilitaryRank, null, "ddPersonMilitaryRankMilitaryRank", true, null, "", "style='width: 250px;' UnsavedCheckSkipMe='true' class='RequiredInputField' ", true);


            // Generates html for drop down list MilitaryCommanderRank
            List<MilitaryCommanderRank> listMilitaryCommanderRank = MilitaryCommanderRankUtil.GetAllMilitaryCommanderRanks(CurrentUser);
            List<IDropDownItem> ddiMilitaryCommanderRank = new List<IDropDownItem>();

            foreach (MilitaryCommanderRank militaryCommanderRank in listMilitaryCommanderRank)
            {
                ddiMilitaryCommanderRank.Add(militaryCommanderRank);
            }

            // Generates html for drop down list
            string PersonMilitaryRankMilitaryCommanderRanksHTML = ListItems.GetDropDownHtml(ddiMilitaryCommanderRank, null, "ddPersonMilitaryRankMilitaryCommanderRank", true, null, "", "style='width: 250px;' UnsavedCheckSkipMe='true'", true);
                      
            string html = @"
<center>
    
    <table width=""80%"" style=""text-align: center;"">
        <colgroup style=""width: 40%"">
        </colgroup>
        <colgroup style=""width: 60%"">
        </colgroup>
        <tr style=""height: 15px"">
        </tr>
        <tr>
            <td colspan=""2"" align=""center"">
                <span class=""HeaderText"" style=""text-align: center;"" id=""lblEditMilitaryRankTitle""></span>
            </td>
        </tr>
        <tr style=""height: 15px"">
        </tr> ";


            html += @"      <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonMilitaryRankMilitaryRankTitle"" class=""InputLabel"">Звание:</span>
            </td>
            <td style=""text-align: left;"">
                " + PersonMilitaryRankMilitaryRanksHTML + @"
            </td>
        </tr> ";


            html += @"   <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonMilitaryRankVacAnn"" class=""InputLabel"">№ на заповедта:</span>
            </td>
            <td style=""text-align: left;"">
                <input type=""text"" id=""txtPersonMilitaryRankVacAnn"" UnsavedCheckSkipMe='true' maxlength=""7"" class=""InputField"" style=""width: 70px;"" />
            </td>
        </tr>";


            html += @"<tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonMilitaryRankDateArchive"" class=""InputLabel"">Дата:</span>
            </td>
            <td style=""text-align: left;""> <span id=""spanPersonMilitaryRankDateArchive"">
                <input type=""text"" id=""txtPersonMilitaryRankDateArchive"" class='RequiredInputField " + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" inLightBox=""true"" UnsavedCheckSkipMe='true' /> </span>
            </td>
        </tr> ";


            html += @"<tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonMilitaryRankDateWhen"" class=""InputLabel"">В сила от:</span>
            </td>
            <td style=""text-align: left;"">  <span id=""spanPersonMilitaryRankDateWhen"">
                <input type=""text"" id=""txtPersonMilitaryRankDateWhen"" class='RequiredInputField " + CommonFunctions.DatePickerCSS() + @"' maxlength='10' style=""width: 75px;"" inLightBox=""true"" UnsavedCheckSkipMe='true' /> </span>
            </td>
        </tr> ";



            html += @"      <tr style=""min-height: 17px"">
            <td style=""text-align: right;"">
                <span id=""lblPersonMilitaryRankMilitaryCommanderRank"" class=""InputLabel"">Подписал заповедта:</span>
            </td>
            <td style=""text-align: left;"">
                " + PersonMilitaryRankMilitaryCommanderRanksHTML + @"
            </td>
        </tr> ";

            html += @"<tr>
                        <td style=""text-align: right;"">
                           <input type=""checkbox"" id=""chkPersonMilitaryRankDR""/> 
                        </td>
                        <td style=""text-align: left;"">                           
                           <span id=""lblPersonMilitaryRankDR"" class=""InputLabel"">ДР</span>
                        </td>
                      </tr>";

            html += @"   </tr>
        <tr style=""height: 46px; padding-top: 5px;"">
            <td colspan=""2"">
                <span id=""spanEditMilitaryRankLightBox"" class=""ErrorText"" style=""display: none;"">
                </span>&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan=""2"" style=""text-align: center;"">
                <table style=""margin: 0 auto;"">
                    <tr>
                        <td>
                            <div id=""btnSaveEditMilitaryRankLightBox"" style=""display: inline;"" onclick=""SaveEditMilitaryRankLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnSaveEditMilitaryRankLightBoxText"" style=""width: 70px;"">
                                    Запис</div>
                                <b></b>
                            </div>
                            <div id=""btnCloseEditMilitaryRankLightBox"" style=""display: inline;"" onclick=""HideEditMilitaryRankLightBox();""
                                class=""Button"">
                                <i></i>
                                <div id=""btnCloseEditMilitaryRankLightBoxText"" style=""width: 70px;"">
                                    Затвори</div>
                                <b></b>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</center>";

            return html;
        }
    }
}
