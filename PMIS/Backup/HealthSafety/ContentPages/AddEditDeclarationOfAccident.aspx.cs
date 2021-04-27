using System;
using System.Web.UI;
using PMIS.Common;
using PMIS.HealthSafety.Common;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace PMIS.HealthSafety.ContentPages
{
    public partial class AddEditDeclarationOfAccident : HSPage
    {
        #region Properties
        //Initialize local variables for using
        DeclarationOfAccident declarationOfAccident;

        string initialListEmpoyee;
        string initialListRegion;
        string initialListMunicipality;
        string initialListCity;
        int? initialPostCode;
        List<int> listResult = new List<int>();

        //Use this for disable/hide UI client controls
        List<string> disabledClientControls = new List<string>();
        List<string> hiddenClientControls = new List<string>();

        bool screenHidden;
        bool screenDisabled;
        UIAccessLevel l;

        string prefix; //use this variable in SETUPPAGE_UI

        //This specifies which part of the UI items tree is for this specific screen
        public override string PageUIKey
        {
            get
            {
                return "HS_DECLARATIONACC";
            }
        }

        //Get-Set Id for declaration Protocol (0 - if new)
        private int declarationId
        {
            get
            {
                int declarationId = 0;
                //gets declarationId either from the hidden field on the page or from page url
                if (String.IsNullOrEmpty(this.hfdeclarationId.Value))
                {
                    if (Request.Params["declarationId"] != null)
                        Int32.TryParse(Request.Params["declarationId"].ToString(), out declarationId);

                    //sets protocol ID in hidden field on the page in order to be accessible in javascript
                    this.hfdeclarationId.Value = declarationId.ToString();
                }
                else
                {
                    Int32.TryParse(this.hfdeclarationId.Value, out declarationId);
                }

                return declarationId;
            }
            set { this.hfdeclarationId.Value = value.ToString(); }
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
            declarationOfAccident = new DeclarationOfAccident(CurrentUser);

            SetInitialPageUI(this.declarationId);

            //Check if this is an ajax call and perform the specific method
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSDisplayData")
            {
                if ((Request.Params["selectedTabID"] != null) && (Request.Params["declarationId"] != null))
                {
                    //Set declarationId
                    this.declarationId = Convert.ToInt32(Request.Params["declarationId"]);
                    string seltab = Request.Params["selectedTabID"];

                    if (this.declarationId > 0)
                    {
                        //Fill Object with data
                        declarationOfAccident = DeclarationOfAccidentUtil.GetDeclarationOfAccident(this.declarationId, this.CurrentUser, Request.Params["selectedTabID"]);
                    }
                    else
                    {
                        declarationOfAccident = new DeclarationOfAccident(CurrentUser);
                    }
                    listResult = DeclarationOfAccidentUtil.GetListCode(this.declarationId, seltab, this.CurrentUser);

                    if (seltab != "btnTabHarm")
                    {
                        GetInitialListRegion();
                    }
                    JSDisplayTab(Request.Params["selectedTabID"]);
                }
            }

            //Ddl Region was changed
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JS_Region")
            {
                if (Request.Params["selectedItemId"] != null)
                {
                    JSGetListMunicipality(Convert.ToInt32(Request.Params["selectedItemId"].ToString()));
                }
            }

            //Ddl Municipality was changed

            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JS_Municipality")
            {
                if (Request.Params["selectedItemId"] != null)
                {
                    JSGetListCity(Convert.ToInt32(Request.Params["selectedItemId"].ToString()));
                }
            }

            //Ddl City was changed
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JS_City")
            {
                if (Request.Params["selectedItemId"] != null)
                {
                    JSGetPostCode(Convert.ToInt32(Request.Params["selectedItemId"].ToString()));
                }
            }

            //Ddl EmplyerId was changed
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetEmplData")
            {
                if (Request.Params["employerId"] != null)
                {
                    JSGetTabEmpl(Convert.ToInt32(Request.Params["employerId"].ToString()));
                }
            }

            //PostCode textBox was fill from user
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSGetCityFromPostCode")
            {
                if (Request.Params["postCode"] != null)
                {
                    int postCode;
                    if (DBCommon.IsInt(Request.Params["postCode"].ToString()))
                    {
                        postCode = DBCommon.GetInt(Request.Params["postCode"].ToString());
                        JSGetCityForPostCode(postCode);
                    }
                    else
                    {
                        string response = "<response>";
                        response += "<statusResult>NO</statusResult>";
                        response += "</response>";
                        AJAX a = new AJAX(response, this.Response);
                        a.Write();
                        Response.End();
                    }
                }
            }

            //INSERT / UPDATE OPERATION
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSSaveData")
            {
                JSSaveData();
            }

            //CHECK FOR NEW DISABLE / HIDDEN STATE
            if (Request.Params["AjaxMethod"] != null && Request.Params["AjaxMethod"] == "JSChekDisableHiddenState")
            {
                JSChekDisableHiddenState();
            }

            if (!IsPostBack)
            {
                //Highlight the current page in the menu bar
                HighlightMenuItems("Accidents_AddDeclarationsOfAccident", "Accidents");

                this.SetupDatePickers(); //Setup any calendar control on the screen

                //Fill Object with data
                if (this.declarationId > 0)
                {
                    declarationOfAccident = DeclarationOfAccidentUtil.GetDeclarationOfAccident(this.declarationId, this.CurrentUser, "btnTabEmpl");
                }

                //initially Bind tabEmpl here
                GetInitialListEmpoyee(EmployerUtil.GetEmployerForDeclarationId(this.declarationId, this.CurrentUser));
                listResult = DeclarationOfAccidentUtil.GetListCode(this.declarationId, "btnTabEmpl", this.CurrentUser);
                GetInitialListRegion();

                //Fill UI Server controls with data
                FillDeclarationOfAccidentHeader();

                this.divEmpl.InnerHtml = TabEmpl(); //fill Tab with HTML data
                this.divEmpl.Visible = true;

                this.SetupPageUI_Header(); //setup user interface for Header
                this.SetupPageUI_Empl();  //setup user interface elements for Tab Empl

                //Set page titles according to mode of work(add or edit protocol)
                this.SetPageName();

                //Hide the navigation buttons
                HideNavigationControls(btnCancel);

            }
        }
        #endregion

        #region AjaxMethods
        //Create XML responce with Data
        private void JSDisplayTab(string selectedTabId)
        {
            string response = "";

            //Select case to load data
            switch (selectedTabId)
            {
                case "btnTabEmpl":
                    response = TabEmpl(); //Load data to this tab
                    break;
                case "btnTabWorker":
                    this.SetupPageUI_Worker(); //Create list with disabled/hidden items
                    response = TabWorker();
                    break;
                case "btnTabAcc":
                    this.SetupPageUI_Acc(); //Create list with disabled/hidden items
                    response = TabAcc();
                    break;
                case "btnTabHarm":
                    this.SetupPageUI_Harm(); //Create list with disabled/hidden items
                    response = TabHarm();
                    break;
                case "btnTabWith":
                    this.SetupPageUI_With(); //Create list with disabled/hidden items
                    response = TabWith();
                    break;
                case "btnTabHeir":
                    this.SetupPageUI_Heir(); //Create list with disabled/hidden items
                    response = TabHeir();
                    break;
                default:
                    break;
            }

            AJAX a = new AJAX(AJAXTools.EncodeForXML(response), this.Response);
            a.Write();
            Response.End();
        }
        //Create XML string for TabEmpl
        private string TabEmpl()
        {
            string result = "";
            result = @"<table id='TabEmpl' class='ManagedNoticeOfAccident'>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical'>
                            <span style='font-weight: bold; font-size: larger' class='InputLabel'>1</span>
                        </td>
                        <td class='td1' style='padding-left: 40px'>
                            <span id='lblInsurerID' class='InputLabel'>Пълно наименование:</span>
                            <select id='ddlEmployerId'  onchange='ddlChangeEmpl(this)' class='RequiredInputField' style='width: 450px;'>" + initialListEmpoyee +
                           @"
                            </select>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical'>
                            <span style='font-weight: bold; font-size: larger' class='InputLabel'>2</span>
                        </td>
                        <td class='td1' style='padding-left: 40px'>
                            <span id='lblInsIdentNum' class='InputLabel'>ЕИК по Регистър " + CommonFunctions.GetLabelText("UnifiedIdentityCode") + @"/Търговски регистър:</span>
                            <input type='text' id='txtEmplEik' class='InputField' style='width: 133px' maxlength='200'  value='" + declarationOfAccident.Employer.EmplEik + @"'></input>
                        </td>
                    </tr>
                    <tr style='min-height: 15px'>
                        <td class='TdVertical'>
                        </td>
                        <td class='td1'>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical'>
                            <span style='font-weight: bold; font-size: larger' class='InputLabel'>3</span>
                        </td>
                        <td class='td1' style='padding-left: 40px'>
                            <span style='font-weight: bold; font-size: larger' class='InputLabel' id='lblEmplAddress'>Адрес:</span>
                            </span> <span class='InputLabel' id='lblEmpRegion'>Област </span>
                            <select id='ddlEmpRegion' onchange='ddlChange(this, 1)' style='width: 200px'>" + initialListRegion +

                           @"</select>
                            <span class='InputLabel' style='padding-left: 40px' id='lblEmpMunicipality'>Община:</span>
                            <select id='ddlEmpMunicipality' onchange='ddlChange(this, 2)' style='width: 200px'>" + initialListMunicipality + @"
                            </select>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical'>
                            <span style='font-weight: bold; font-size: larger' class='InputLabel'>4</span>
                        </td>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' id='lblEmplCity' >Град:</span>
                            <select id='ddlEmplCity' onchange='ddlChange(this, 3)' style='width: 200px'>" + initialListCity + @"
                            </select>
                            <span class='InputLabel' style='padding-left: 48px' id='lblEmplStreet'>Ул:</span>
                            <input type='text' id='txtEmplStreet' class='InputField' style='width: 250px' maxlength='300' value='" + declarationOfAccident.Employer.EmplStreet + @"'></input>
                            <span class='InputLabel' style='padding-left: 20px' id='lblEmplStreetNum'>№:</span>
                            <input type='text' id='txtEmplStreetNum' class='InputField' style='width: 30px' maxlength='20' value='" + declarationOfAccident.Employer.EmplStreetNum + @"'></input>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical'>
                        </td>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' style='padding-right: 8px' id='lblEmplDistrict'>Жк.:</span>
                            <input type='text' id='txtEmplDistrict' class='InputField' style='width: 227px' maxlength='200' value='" + declarationOfAccident.Employer.EmplDistrict + @"'></input>
                            <span class='InputLabel' style='padding-left: 12px' id='lblEmplBlock'>Бл.:</span>
                            <input type='text' id='txtEmplBlock' class='InputField' style='width: 35px' maxlength='20' value='" + declarationOfAccident.Employer.EmplBlock + @"'></input>
                            <span class='InputLabel' style='padding-left: 10px' id='lblEmplEntrance'>Вх.:</span>
                            <input type='text' id='txtEmplEntrance' class='InputField' style='width: 35px' maxlength='20' value='" + declarationOfAccident.Employer.EmplEntrance + @"'></input>
                            <span class='InputLabel' style='padding-left: 10px'  id='lblEmplFloor'>Ет.:</span>
                            <input type='text' id='txtEmplFloor' class='InputField' style='width: 35px' maxlength='20' value='" + declarationOfAccident.Employer.EmplFloor + @"'></input>
                            <span class='InputLabel' style='padding-left: 10px' id='lblEmplApt'>Ап.:</span>
                            <input type='text' id='txtEmplApt' class='InputField' style='width: 35px' maxlength='20'value='" + declarationOfAccident.Employer.EmplApt + @"'></input>
                            <span class='InputLabel' style='padding-left: 10px' id='lblEmplPostCode'>Пощенски Код:</span>
                            <input onfocus='SetOldPostCode(this)' onblur='FillCityFromPostCode(this)' type='text' id='txtEmplPostCode' class='InputField' style='width: 50px' maxlength='20' value='" + initialPostCode + @"'></input>                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical'>
                            <span style='font-weight: bold; font-size: larger' class='InputLabel'>5</span>
                        </td>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' id='lblEmplPhone'>Телефон:</span>
                            <input type='text' id='txtEmplPhone' class='InputField' style='width: 130px' maxlength='50' value='" + declarationOfAccident.Employer.EmplPhone + @"'></input>
                            <span class='InputLabel' style='padding-left: 20px' id='lblEmplFax'>Факс:</span>
                            <input type='text' id='txtEmplFax' class='InputField' style='width: 130px' maxlength='50' value='" + declarationOfAccident.Employer.EmplFax + @"'></input>
                            <span class='InputLabel' style='padding-left: 20px' id='lblEmplEmail'>E-mail:</span>
                            <input type='text' id='txtEmplEmail' class='InputField' style='width: 250px' maxlength='500' value='" + declarationOfAccident.Employer.EmplEmail + @"'></input>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical'>
                            <span style='font-weight: bold; font-size: larger' class='InputLabel'>6</span>
                        </td>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' id='lblEmplNumberOfEmployees'>Списъчен брой на работниците и служителите:</span>
                            <input type='text' id='txtEmplNumberOfEmployees' class='InputField' style='width: 40px'
                                maxlength='3' value='" + declarationOfAccident.Employer.EmplNumberOfEmployees + @"'></input>
                            <span class='InputLabel' style='padding-left: 10px' id='lblEmplFemaleEmployees'>от тях жени:</span>
                            <input type='text' id='txtEmplFemaleEmployees' class='InputField' style='width: 40px'
                                maxlength='3' value='" + declarationOfAccident.Employer.EmplFemaleEmployees + @"'></input>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical'>
                        </td>
                        <td class='td1'>
                        </td>
                    </tr>
                </table>
";

            return result;
        }
        //Create XML string for TabWorker
        private string TabWorker()
        {
            string result = "";
            result = @"  <table id='TabWorker' class='ManagedNoticeOfAccident'>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical'>
                            <span style='font-weight: bold; font-size: larger' class='InputLabel'>7</span>
                        </td>
                        <td class='td1' style='padding-left: 40px'>
                            <span class='InputLabel' id='lblWorkerFullName'>Трите имена:</span>
                            <input type='text' id='txtWorkerFullName' class='RequiredInputField' style='width: 420px'
                                maxlength='500' value='" + declarationOfAccident.DeclarationOfAccidentWorker.WorkerFullName + @"'></input>
                            <span class='InputLabel' id='lblWorkerEgn'>ЕГН:</span>
                            <input type='text' id='txtWorkerEgn' class='InputField' style='width: 115px' maxlength='10'value='" + declarationOfAccident.DeclarationOfAccidentWorker.WorkerEgn + @"'></input>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical'>
                            <span style='font-weight: bold; font-size: larger' class='InputLabel'>8</span>
                        </td>
                        <td class='td1' style='padding-left: 40px'>
                            <span style='font-weight: bold; font-size: larger' class='InputLabel' id='lblWorkerAddress'>Постоянен адрес:</span>
                            </span> <span class='InputLabel' id='lblWRegion'>Област </span>
                            <select id='ddlWRegion' onchange='ddlChange(this, 1)' style='width: 200px'>" + initialListRegion +

                           @"
                            </select>
                            <span class='InputLabel' style='padding-left: 40px' id='lblWMunicipality'>Община:</span>
                           <select id='ddlWMunicipality' onchange='ddlChange(this, 2)' style='width: 200px'>" + initialListMunicipality +

                           @"
                            </select>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical'>
                            <span style='font-weight: bold; font-size: larger' class='InputLabel'>9</span>
                        </td>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' id='lblWCity'>Град:</span>
                           <select id='ddlWCity' onchange='ddlChange(this, 3)' style='width: 200px'>" + initialListCity +

                           @"
                            </select>
                            <span class='InputLabel' style='padding-left: 48px'  id='lblWStreet'>Ул:</span>
                            <input type='text' id='txtWStreet' class='InputField' style='width: 250px' maxlength='300' value='" + declarationOfAccident.DeclarationOfAccidentWorker.WStreet + @"'></input>
                            <span class='InputLabel' style='padding-left: 20px' id='lblWStreetNum'>№:</span>
                            <input type='text' id='txtWStreetNum' class='InputField' style='width: 30px' maxlength='20' value='" + declarationOfAccident.DeclarationOfAccidentWorker.WStreetNum + @"'></input>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical'>
                        </td>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' style='padding-right: 8px' id='lblWDistrict'>Жк.:</span>
                            <input type='text' id='txtWDistrict' class='InputField' style='width: 227px' maxlength='200' value='" + declarationOfAccident.DeclarationOfAccidentWorker.WDistrict + @"'></input>
                            <span class='InputLabel' style='padding-left: 12px' id='lblWBlock' >Бл.:</span>
                            <input type='text' id='txtWBlock' class='InputField' style='width: 35px' maxlength='20' value='" + declarationOfAccident.DeclarationOfAccidentWorker.WBlock + @"'></input>
                            <span class='InputLabel' style='padding-left: 10px' id='lblWEntrance'>Вх.:</span>
                            <input type='text' id='txtWEntrance' class='InputField' style='width: 35px' maxlength='20' value='" + declarationOfAccident.DeclarationOfAccidentWorker.WEntrance + @"'></input>
                            <span class='InputLabel' style='padding-left: 10px' id='lblWFloor'>Ет.:</span>
                            <input type='text' id='txtWFloor' class='InputField' style='width: 35px' maxlength='20' value='" + declarationOfAccident.DeclarationOfAccidentWorker.WFloor + @"'></input>
                            <span class='InputLabel' style='padding-left: 10px' id='lblWApt'>Ап.:</span>
                            <input type='text' id='txtWApt' class='InputField' style='width: 35px' maxlength='20' value='
" + declarationOfAccident.DeclarationOfAccidentWorker.WApt + @"'></input>
                            <span class='InputLabel' style='padding-left: 10px' id='lblWPostCode'>Пощенски Код:</span>
                            <input onfocus='SetOldPostCode(this)' onblur='FillCityFromPostCode(this)' type='text' id='txtWPostCode' class='InputField' style='width: 50px' value='
" + initialPostCode + @"'></input>
</input>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical'>
                            <span style='font-weight: bold; font-size: larger' class='InputLabel'>10</span>
                        </td>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' id='lblWPhone'>Телефон:</span>
                            <input type='text' id='txtWPhone' class='InputField' style='width: 130px' maxlength='50' value='" + declarationOfAccident.DeclarationOfAccidentWorker.WPhone + @"'></input>
                            <span class='InputLabel' style='padding-left: 20px' id='lblWFax'>Факс:</span>
                            <input type='text' id='txtWFax' class='InputField' style='width: 130px' maxlength='50' value='" + declarationOfAccident.DeclarationOfAccidentWorker.WFax + @"'></input>
                            <span class='InputLabel' style='padding-left: 20px'  id='lblWEmail'>E-mail:</span>
                            <input type='text' id='txtWEmail' class='InputField' style='width: 130px' maxlength='500' value='" + declarationOfAccident.DeclarationOfAccidentWorker.WEmail + @"'></input>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical'>
                            <span style='font-weight: bold; font-size: larger' class='InputLabel'>11</span>
                        </td>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' style='padding-right: 15px' id='lblWBirthDate'>Дата на раждане:</span>
                            <input type='text' id='txtWBirthDate' class='" + CommonFunctions.DatePickerCSS() + @"' style='width: 75px'
                                maxlength='10' value='" + CommonFunctions.FormatDate(declarationOfAccident.DeclarationOfAccidentWorker.WBirthDate) + @"'></input>
                            <span class='InputLabel' style='padding-left: 183px; padding-right: 8px' id='lblWGender'>Пол:</span>
                            <input id='rbWGender1' type='radio' value='WGender1' name='groupWGender' " + DeclarationOfAccidentUtil.GetChekedStatus(1, declarationOfAccident.DeclarationOfAccidentWorker.WGender) + @"/>
                            <span class='InputLabel' style='padding-right: 8px; padding-left: 5px' id='lblWGender1'>- мъж</span>
                            <input id='rbWGender2' type='radio' name='groupWGender' value='WGender2' " + DeclarationOfAccidentUtil.GetChekedStatus(2, declarationOfAccident.DeclarationOfAccidentWorker.WGender) + @" />
                            <span class='InputLabel' style='padding-right: 8px; padding-left: 5px' id='lblWGender2'>- жена</span>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical'>
                            <span style='font-weight: bold; font-size: larger' class='InputLabel'>12</span>
                        </td>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' style='padding-right: 38px' id='lblWCitizenship'>Гражданство:</span>
                            <input type='text' id='txtWCitizenship' class='InputField' style='width: 200px' maxlength='100' value='
" + declarationOfAccident.DeclarationOfAccidentWorker.WCitizenship + @"'></input>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical'>
                            <span style='font-weight: bold; font-size: larger' class='InputLabel'>13</span>
                        </td>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' style='padding-right: 25px' id='lblWHireTypeHeader'>Пострадалия е нает за:</span>
                            <span class='InputLabel' style='padding-right: 5px' id='lblWHireType'>1.</span>
                            <input id='rbWHireType1' type='radio' value='WHireType1' name='groupWHireType' " + DeclarationOfAccidentUtil.GetChekedStatus(1, declarationOfAccident.DeclarationOfAccidentWorker.WHireType) + @"/>
                            <span class='InputLabel' style='padding-right: 35px; padding-left: 2px' id='lblWHireType1'>неопределено
                                време</span>
                            <input id='rbWHireType2' type='radio' name='groupWHireType' value='WHireType2' " + DeclarationOfAccidentUtil.GetChekedStatus(2, declarationOfAccident.DeclarationOfAccidentWorker.WHireType) + @"/>
                            <span class='InputLabel' id='lblWHireType2'>определен срок</span>
                        </td>
                    </tr>
                    <tr style='min-height: 13px; margin-top: 0px'>
                        <td class='TdVertical'>
                        </td>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' style='padding-left: 183px; padding-right: 4px' id='lblWWorkTime'>2.</span>
                            <input id='rbWWorkTime1' type='radio' value='WWorkTime1' name='groupWWorkTime' " + DeclarationOfAccidentUtil.GetChekedStatus(1, declarationOfAccident.DeclarationOfAccidentWorker.WWorkTime) + @"/>
                            <span class='InputLabel' style='padding-right: 33px; padding-left: 3px' id='lblWWorkTime1'>пълно работно
                                време </span>
                            <input id='rbWWorkTime2' type='radio' name='groupWWorkTime' value='WWorkTime2' " + DeclarationOfAccidentUtil.GetChekedStatus(2, declarationOfAccident.DeclarationOfAccidentWorker.WWorkTime) + @"/>
                            <span class='InputLabel' id='lblWWorkTime2'>непълно работно време</span>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical'>
                            <span style='font-weight: bold; font-size: larger' class='InputLabel'>14</span>
                        </td>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' style='padding-right: 10px' id='lblWHireDate'>Дата на постъпване в предприятието:</span>
                            <input type='text' id='txtWHireDate' class='" + CommonFunctions.DatePickerCSS() + @"' style='width: 75px'
                                maxlength='10' value='" + CommonFunctions.FormatDate(declarationOfAccident.DeclarationOfAccidentWorker.WHireDate) + @"'></input>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical'>
                            <span style='font-weight: bold; font-size: larger' class='InputLabel'>15</span>
                        </td>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' style='padding-right: 12px' id='lblWJobTitle'>Професия по НКП:</span>
                            <input type='text' id='txtWJobTitle' class='InputField' style='width: 200px' maxlength='200' value='" + declarationOfAccident.DeclarationOfAccidentWorker.WJobTitle + @"'></input>
                            <span class='InputLabel' style='padding-left: 38px; padding-right: 12px' id='lblWJobCode'>Код по НКП:</span>
                            <input type='text' id='txtWJobCode' class='InputField' style='width: 200px' maxlength='100' value='
" + declarationOfAccident.DeclarationOfAccidentWorker.WJobCode + @"'></input>
                        </td>
                    </tr>
                    <tr>
                        <td class='TdVertical'>
                            <span style='font-weight: bold; font-size: larger' class='InputLabel'>16</span>
                        </td>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' style='padding-right: 30px' id='lblWJobCategory'>Категория труд:</span>
                            <input id='rbWJobCategory1' type='radio' value='WJobCategory1' name='groupWJobCategory'
                                " + DeclarationOfAccidentUtil.GetChekedStatus(1, declarationOfAccident.DeclarationOfAccidentWorker.WJobCategory) + @"/>
                            <span class='InputLabel' style='padding-right: 8px; padding-left: 5px' id='lblWJobCategory1'>- първа</span>
                            <input id='rbWJobCategory2' type='radio' name='groupWJobCategory' value='WJobCategory2' " + DeclarationOfAccidentUtil.GetChekedStatus(2, declarationOfAccident.DeclarationOfAccidentWorker.WJobCategory) + @"/>
                            <span class='InputLabel' style='padding-right: 8px; padding-left: 5px' id='lblWJobCategory2'>- втора</span>
                            <input id='rbWJobCategory3' type='radio' name='groupWJobCategory' value='WJobCategory3' " + DeclarationOfAccidentUtil.GetChekedStatus(3, declarationOfAccident.DeclarationOfAccidentWorker.WJobCategory) + @"/>
                            <span class='InputLabel' style='padding-right: 8px; padding-left: 5px' id='lblWJobCategory3'>- трета</span>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical'>
                            <span style='font-weight: bold; font-size: larger' class='InputLabel'>17</span>
                        </td>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' style='padding-right: 0px' id='lblWYearsOnService'>Трудов стаж (години):</span>
                            <input type='text' id='txtWYearsOnService' class='InputField' style='width: 200px'
                                maxlength='2' value='" + declarationOfAccident.DeclarationOfAccidentWorker.WYearsOnService + @"'></input>
                            <span class='InputLabel' style='padding-right: 0px' id='lblWCurrentJobYearsOnService'>, по посочената професия:</span>
                            <input type='text' id='txtWCurrentJobYearsOnService' class='InputField' style='width: 200px'
                                maxlength='2' value='" + declarationOfAccident.DeclarationOfAccidentWorker.WCurrentJobYearsOnService + @"'></input>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical'>
                            <span style='font-weight: bold; font-size: larger' class='InputLabel'>18</span>
                        </td>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' style='padding-right: 0px' id='lblWBranch'>Административна единица в която
                                е назначен:</span>
                            <input type='text' id='txtWBranch' class='InputField' style='width: 300px' maxlength='200' value='" + declarationOfAccident.DeclarationOfAccidentWorker.WBranch + @"'></input>
                        </td>
                    </tr>
                </table>
<input type='hidden' id='tabDisabledControlsWorker' value='" + SetListDisabledControls() + @"'/>
<input type='hidden' id='tabHiddenControlsWorker' value='" + SetListHiddenControls() + @"'/>
";
            return result;

        }
        //Create XML string for TabAcc
        private string TabAcc()
        {
            string min = "";
            string hour = "";
            if (declarationOfAccident.DeclarationOfAccidentAcc.AccDateTime.HasValue)
            {
                DateTime dt = Convert.ToDateTime(declarationOfAccident.DeclarationOfAccidentAcc.AccDateTime);
                min = dt.Minute.ToString();
                hour = dt.Hour.ToString();
            }

            string result = "";
            result = @"<table id='TabAcc' class='ManagedNoticeOfAccident'>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical'>
                            <span style='font-weight: bold; font-size: larger' class='InputLabel'>19</span>
                        </td>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' id='lblAccDateTimeHeader'>Злополуката е станала в:</span>
                            <input type='text' id='txtAccDateTimeHours' class='InputField' style='width: 20px'
                                maxlength='2' value='" + hour + @"'></input>
                            <span class='InputLabel' id='lblAccDateTimeHours'>часа</span>
                            <input type='text' id='txtAccDateTimeMinutes' class='InputField' style='width: 20px'
                                maxlength='2' value='" + min + @"'></input>
                            <span class='InputLabel' id='lblAccDateTimeMinutes'>минути </span><span class='InputLabel' style='padding-left: 90px' id='lblAccDateTimeDay'>
                                На дата: </span>
                            <input type='text' id='txtAccDateTimeDay' class='" + CommonFunctions.DatePickerCSS() + @"'
                                style='width: 75px' maxlength='10' value='" + CommonFunctions.FormatDate(declarationOfAccident.DeclarationOfAccidentAcc.AccDateTime) + @"'></input>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical'> 
                            <span style='font-weight: bold; font-size: larger' class='InputLabel'>20</span>
                        </td>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' id='lblAccWorkTimeHeader1'>Работно време:</span> <span class='InputLabel' style='font-weight: 600;' id='lblAccWorkTimeHeader2'>
                                от</span>
                            <input type='text' id='txtAccWorkFromHour1' class='InputField' style='width: 20px'
                                maxlength='2' value='" + declarationOfAccident.DeclarationOfAccidentAcc.AccWorkFromHour1 + @"'></input>
                            </input> <span class='InputLabel' id='lblAccWorkFromHour1'>часа</span>
                            <input type='text' id='txtAccWorkFromMin1' class='InputField' style='width: 20px'
                                maxlength='2' value='" + declarationOfAccident.DeclarationOfAccidentAcc.AccWorkFromMin1 + @"'></input>
                            </input> <span class='InputLabel' id='lblAccWorkFromMin1'>минути</span> <span class='InputLabel' style='font-weight: 600;
                                padding-left: 5px' id='lblAccWorkTimeHeader3'>до</span>
                            <input type='text' id='txtAccWorkToHour1' class='InputField' style='width: 20px'
                                maxlength='2' value='" + declarationOfAccident.DeclarationOfAccidentAcc.AccWorkToHour1 + @"'></input>
                            </input> <span class='InputLabel' id='lblAccWorkToHour1'>часа</span>
                            <input type='text' id='txtAccWorkToMin1' class='InputField' style='width: 20px' maxlength='2' value='
" + declarationOfAccident.DeclarationOfAccidentAcc.AccWorkToMin1 + @"'></input>
                            <span class='InputLabel' id='lblAccWorkToMin1'>минути </span><span class='InputLabel' style='font-weight: 600;
                                padding-left: 10px' id='lblAccWorkTimeHeader4'>и от</span>
                            <input type='text' id='txtAccWorkFromHour2' class='InputField' style='width: 20px'
                                maxlength='2' value='" + declarationOfAccident.DeclarationOfAccidentAcc.AccWorkFromHour2 + @"'></input>
                            <span class='InputLabel' id='lblAccWorkFromHour2'>часа</span>
                            <input type='text' id='txtAccWorkFromMin2' class='InputField' style='width: 20px'
                                maxlength='2' value='" + declarationOfAccident.DeclarationOfAccidentAcc.AccWorkFromMin2 + @"'></input>
                            <span class='InputLabel' id='lblAccWorkFromMin2'>минути </span><span class='InputLabel' style='font-weight: 600;
                                padding-left: 5px' id='lblAccWorkTimeHeader5'>до</span>
                            <input type='text' id='txtAccWorkToHour2' class='InputField' style='width: 20px'
                                maxlength='2' value='" + declarationOfAccident.DeclarationOfAccidentAcc.AccWorkToHour2 + @"'></input>
                            <span class='InputLabel' id='lblAccWorkToHour2'>часа</span>
                            <input type='text' id='txtAccWorkToMin2' class='InputField' style='width: 20px' maxlength='2' value='
" + declarationOfAccident.DeclarationOfAccidentAcc.AccWorkToMin2 + @"'></input>
                            <span class='InputLabel' id='lblAccWorkToMin2'>минути </span>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical'>
                            <span style='font-weight: bold; font-size: larger' class='InputLabel'>21</span>
                        </td>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' style='padding-right: 15px' id='lblAccPlace'>Място на злополуката:</span>
                            <input type='text' id='txtAccPlace' class='InputField' style='width: 343px' maxlength='250' value='
" + declarationOfAccident.DeclarationOfAccidentAcc.AccPlace + @"'></input>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical'>
                            <span style='font-weight: bold; font-size: larger' class='InputLabel'>22</span>
                        </td>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' id='lblAccAddressHeader'>Адрес на мястото на злополуката, ако е различен от посочения
                                в редове 3 и 4.</span>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical'>
                        </td>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' id='lblAccCountry'>Държава:</span>
                            <input type='text' id='txtAccCountry' class='InputField' style='width: 130px' maxlength='50' value='
" + declarationOfAccident.DeclarationOfAccidentAcc.AccCountry + @"'></input>
                            <span class='InputLabel' id='lblAccRegion'>Област:</span>
 <select id='ddlAccRegion' onchange='ddlChange(this, 1)' style='width: 130px'>" + initialListRegion +

                           @"
                            </select>
                            <span class='InputLabel' style='padding-left: 4px' id='lblAccMunicipality'>Община:</span>  <select id='ddlAccMunicipality' onchange='ddlChange(this, 2)' style='width: 130px'>" + initialListMunicipality +

                           @"
                            </select>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical'>
                        </td>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' style='padding-right: 13px' id='lblAccCity'>Град:</span>
<select id='ddlAccCity' onchange='ddlChange(this, 3)' style='width: 200px'>" + initialListCity +

                           @"

                            </select>
                            <span class='InputLabel' style='padding-left: 35px' id='lblAccStreet'>Ул:</span>
                            <input type='text' id='txtAccStreet' class='InputField' style='width: 250px' maxlength='300' value='
" + declarationOfAccident.DeclarationOfAccidentAcc.AccStreet + @"'></input>
                            <span class='InputLabel' style='padding-left: 20px' id='lblAccStreetNum'>№:</span>
                            <input type='text' id='txtAccStreetNum' class='InputField' style='width: 30px' maxlength='20' value='
" + declarationOfAccident.DeclarationOfAccidentAcc.AccStreetNum + @"'></input>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical'>
                        </td>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' style='padding-left: 23px' id='lblAccDistrict'>Жк.:</span>
                            <input type='text' id='txtAccDistrict' class='InputField' style='width: 227px' maxlength='200' value='
" + declarationOfAccident.DeclarationOfAccidentAcc.AccDistrict + @"'></input>
                            <span class='InputLabel' style='padding-left: 12px' id='lblAccBlock'>Бл.:</span>
                            <input type='text' id='txtAccBlock' class='InputField' style='width: 35px' maxlength='20' value='
" + declarationOfAccident.DeclarationOfAccidentAcc.AccBlock + @"'></input>
                            <span class='InputLabel' style='padding-left: 10px' id='lblAccEntrance'>Вх.:</span>
                            <input type='text' id='txtAccEntrance' class='InputField' style='width: 35px' maxlength='20' value='
" + declarationOfAccident.DeclarationOfAccidentAcc.AccEntrance + @"'></input>
                            <span class='InputLabel' style='padding-left: 10px' id='lblAccFloor'>Ет.:</span>
                            <input type='text' id='txtAccFloor' class='InputField' style='width: 35px' maxlength='20' value='
" + declarationOfAccident.DeclarationOfAccidentAcc.AccFloor + @"'></input>
                            <span class='InputLabel' style='padding-left: 10px' id='lblAccApt'>Ап.:</span>
                            <input type='text' id='txtAccApt' class='InputField' style='width: 35px' maxlength='20' value='
" + declarationOfAccident.DeclarationOfAccidentAcc.AccApt + @"'></input>
                            <span class='InputLabel' style='padding-left: 10px' id='lblAccPostCode'>Пощенски Код:</span>
                            <input onfocus='SetOldPostCode(this)' onblur='FillCityFromPostCode(this)' type='text' id='txtAccPostCode' class='InputField' style='width: 50px' value='
" + initialPostCode + @"'></input>
</input>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical'>
                        </td>
                        <td class='td1' style='padding-left: 10px'>
                            <span class='InputLabel' id='lblAccPhone'>Телефон:</span>
                            <input type='text' id='txtAccPhone' class='InputField' style='width: 130px' maxlength='50' value='
" + declarationOfAccident.DeclarationOfAccidentAcc.AccPhone + @"'></input>
                            <span class='InputLabel' style='padding-left: 20px' id='lblAccFax'>Факс:</span>
                            <input type='text' id='txtAccFax' class='InputField' style='width: 130px' maxlength='50' value='
" + declarationOfAccident.DeclarationOfAccidentAcc.AccFax + @"'></input>
                            <span class='InputLabel' style='padding-left: 20px' id='lblAccEmail'>E-mail:</span>
                            <input type='text' id='txtAccEmail' class='InputField' style='width: 130px' maxlength='500' value='
" + declarationOfAccident.DeclarationOfAccidentAcc.AccEmail + @"'></input>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical' style='vertical-align: text-top'>
                            <span style='font-weight: bold; font-size: larger' class='InputLabel'>23</span>
                        </td>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' style='padding-right: 33px' id='lblAccHappenedAt'>Злополуката е станала на: </span>
                            <div style='display: inline-table'>
                                <div>
                                    <input id='rbAccHappenedAt1' type='radio' onclick='SetVisibility(1);' name='groupAccHappenedAt' value='AccHappenedAt1' " + DeclarationOfAccidentUtil.GetChekedStatus(1, declarationOfAccident.DeclarationOfAccidentAcc.AccHappenedAt) + @"/>
                                    <span class='InputLabel' style='padding-right: 8px; padding-left: 5px' id='lblAccHappenedAt1'>- обичайното
                                        стационарно работно място</span>
                                </div>
                                <div>
                                    <input id='rbAccHappenedAt2' type='radio' onclick='SetVisibility(2);' name='groupAccHappenedAt' value='AccHappenedAt2' " + DeclarationOfAccidentUtil.GetChekedStatus(2, declarationOfAccident.DeclarationOfAccidentAcc.AccHappenedAt) + @"/>
                                    <span class='InputLabel' style='padding-right: 8px; padding-left: 5px' id='lblAccHappenedAt2'>- случайно, нестационарно
                                        (мобилно), временно работно място </span>
                                </div>
                                <div>
                                    <input id='rbAccHappenedAt3' type='radio' onclick='SetVisibility(3);' name='groupAccHappenedAt' value='AccHappenedAt3' " + DeclarationOfAccidentUtil.GetChekedStatus(3, declarationOfAccident.DeclarationOfAccidentAcc.AccHappenedAt) + @"/>
                                    <span class='InputLabel' style='padding-right: 8px; padding-left: 5px' id='lblAccHappenedAt3'>- друго </span>
                                     <input type='text' id='txtAccHappenedAtOther' class='InputField' style='width: 451px; visibility: "
  + DeclarationOfAccidentUtil.GetVisibilityStatus(DeclarationOfAccidentUtil.GetChekedStatus(3, declarationOfAccident.DeclarationOfAccidentAcc.AccHappenedAt)) + @"' maxlength='1000' value='
" + declarationOfAccident.DeclarationOfAccidentAcc.AccHappenedOther + @"'></input>
                            </div>
                            </div>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical'>
                            <span style='font-weight: bold; font-size: larger' class='InputLabel'>24</span>
                        </td>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' style='padding-right: 90px' id='lblAccJobType'>Вид работа:</span>
                            <input type='text' id='txtAccJobType' class='InputField' style='width: 575px' maxlength='1000' value='
" + declarationOfAccident.DeclarationOfAccidentAcc.AccJobType + @"'></input>
                        </td>
                    </tr>
                    <tr style='min-height: 20px'>
                        <td class='TdVertical'>
                            <span style='font-weight: bold; font-size: larger' class='InputLabel'>25</span>
                        </td>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' style='padding-right: 118px' id='lblAccTaskType'>Специфично действие, извършвано
                                от пострадалия и свързаният с това действие материален фактор:</span>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical'>
                        </td>
                        <td class='td1' style='padding-left: 20px'>
                            <textarea id='txtAccTaskType' cols='3' rows='3' class='InputField' style='width: 750px'>
" + declarationOfAccident.DeclarationOfAccidentAcc.AccTaskType + @"</textarea>
                        </td>
                    </tr>
                    <tr>
                        <td class='TdVertical'>
                        </td>
                        <td>
                            <div style='height: 10px'>
                            </div>
                        </td>
                    </tr>
                    <tr style='min-height: 20px'>
                        <td class='TdVertical'>
                            <span style='font-weight: bold; font-size: larger' class='InputLabel'>26</span>
                        </td>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' style='padding-right: 118px' id='lblAccDeviationFromTask'>Отклонение от нормалните действия(условия)
                                и материален фактор, свързан с това отклонение:</span>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical'>
                        </td>
                        <td class='td1' style='padding-left: 20px'>
                            <textarea id='txtAccDeviationFromTask' cols='3' rows='3' class='InputField' style='width: 750px' >
" + declarationOfAccident.DeclarationOfAccidentAcc.AccDeviationFromTask + @"</textarea>
                        </td>
                    </tr>
                    <tr>
                        <td class='TdVertical'>
                        </td>
                        <td>
                            <div style='height: 10px'>
                            </div>
                        </td>
                    </tr>
                    <tr style='min-height: 20px'>
                        <td class='TdVertical'>
                            <span style='font-weight: bold; font-size: larger' class='InputLabel'>27</span>
                        </td>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' style='padding-right: 118px'  id='lblAccInjurDesc'>Начин на увреждането и материален
                                фактор, причинил увреждането:</span>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical'>
                        </td>
                        <td class='td1' style='padding-left: 20px'>
                            <textarea id='txtAccInjurDesc' cols='3' rows='3' class='InputField' style='width: 750px'>" + declarationOfAccident.DeclarationOfAccidentAcc.AccInjurDesc + @"</textarea>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical'>
                            <span style='font-weight: bold; font-size: larger' class='InputLabel'>28</span>
                        </td>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' style='padding-right: 2px' id='lblAccInjHasRights'>Пострадалият имал ли е необходимата
                                правоспособност:</span>
                            <input id='rbAccInjHasRights1' type='radio' name='groupAccInjHasRights' value='AccInjHasRights1' " + DeclarationOfAccidentUtil.GetChekedStatus(1, declarationOfAccident.DeclarationOfAccidentAcc.AccInjHasRights) + @"/>
                            <span class='InputLabel' style='padding-right: 10px; padding-left: 2px' id='lblAccInjHasRights1'>- да </span>
                            <input id='rbAccInjHasRights2' type='radio' name='groupAccInjHasRights' value='AccInjHasRights2' " + DeclarationOfAccidentUtil.GetChekedStatus(2, declarationOfAccident.DeclarationOfAccidentAcc.AccInjHasRights) + @"/>
                            <span class='InputLabel' style='padding-right: 10px; padding-left: 2px' id='lblAccInjHasRights2'>- не</span>
                            <input id='rbAccInjHasRights3' type='radio' name='groupAccInjHasRights' value='AccInjHasRights3' " + DeclarationOfAccidentUtil.GetChekedStatus(3, declarationOfAccident.DeclarationOfAccidentAcc.AccInjHasRights) + @"/>
                            <span class='InputLabel' id='lblAccInjHasRights3'>- не се изисква</span>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical'>
                            <span style='font-weight: bold; font-size: larger' class='InputLabel'>29</span>
                        </td>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' style='padding-right: 278px' id='lblAccLegalRef'>Злополуката е:</span>
                            <input id='rbAccLegalRef1' type='radio' name='grouprbAccLegalRef'  " + DeclarationOfAccidentUtil.GetChekedStatus(1, declarationOfAccident.DeclarationOfAccidentAcc.AccLegalRef) + @"/>
                            <span class='InputLabel' style='padding-right: 10px; padding-left: 2px' id='lblAccLegalRef1'>- по чл. 55,
                                ал.1 от КСО </span>
                            <input id='rbAccLegalRef2' type='radio' name='grouprbAccLegalRef'  " + DeclarationOfAccidentUtil.GetChekedStatus(2, declarationOfAccident.DeclarationOfAccidentAcc.AccLegalRef) + @"/>
                            <span class='InputLabel' style='padding-right: 10px; padding-left: 2px' id='lblAccLegalRef2'>- по чл. 55,
                                ал.2 от КСО</span>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical'>
                            <span style='font-weight: bold; font-size: larger' class='InputLabel'>30</span>
                        </td>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' style='padding-right: 55px' id='lblAccPlannedActions'>Набелязани мерки:</span>
                            <input type='text' id='txtAccPlannedActions' class='InputField' style='width: 565px'
                                maxlength='2000' value='" + declarationOfAccident.DeclarationOfAccidentAcc.AccPlannedActions + @"'></input>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical'>
                        </td>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' style='padding-right: 5px' id='lblAccLostDays'>Загубени календарни дни:</span>
                            <input type='text' id='txtAccLostDays' class='InputField' style='width: 100px' maxlength='3' value='
" + declarationOfAccident.DeclarationOfAccidentAcc.AccLostDays + @"'></input>
                        </td>
                    </tr>
                </table>
<input type='hidden' id='tabDisabledControlsAcc' value='" + SetListDisabledControls() + @"'/>
<input type='hidden' id='tabHiddenControlsAcc' value='" + SetListHiddenControls() + @"'/>
";
            return result;
        }
        //Create XML string for TabHarm
        private string TabHarm()
        {
            string result = "";
            result = @"<table id='TabHarm' class='ManagedNoticeOfAccident'>
                    <tr>
                        <td class='TdVertical'>
                        </td>
                        <td colspan='2'>
                            <div style='min-height: 50px'>
                            </div>
                        </td>
                    </tr>
                    <tr style='min-height: 65px; vertical-align: middle; padding-top: 15px'>
                        <td class='TdVertical'>
                            <span style='font-weight: bold; font-size: larger' class='InputLabel'>31</span>
                        </td>
                        <td class='td1' style='vertical-align: middle; padding-left: 20px'>
                            <span class='InputLabel' style='padding-right: 10px; vertical-align: middle' id='lblHarmType'>Вид на
                                уврежданията:</span>
                        </td>
                        <td align='left'>
                            <textarea id='txtHarmType' cols='3' rows='4' class='InputField' style='width: 540px'>
" + declarationOfAccident.DeclarationOfAccidentHarm.HarmType + @"</textarea>
                        </td>
                    </tr>
                    <tr>
                        <td class='TdVertical'>
                        </td>
                        <td>
                            <div style='min-height: 15px'>
                            </div>
                        </td>
                        <td>
                            <div id='lblHarmTypeAdditional'  style='width: 540px; text-align: left; font-size:11px'>
                            Посочва се видът на уврежданията на пострадалия (рани, счупвания, изкълчвания, ампутации, 
                            мозъчно сътресение, вътрешни травми, изгаряния, измързвания, отравяне, задушаване и т.н.) 
                            съгласно болничния лист и/или друг медицински документ. 
                            При избор на "" Други "" уврежданията се посочват в полето за писане.
                            </div>
                            <div style='min-height: 12px'>
                            </div>
                        </td>

                    </tr>
                    <tr style='min-height: 65px; vertical-align: middle'>
                        <td class='TdVertical'>
                            <span style='font-weight: bold; font-size: larger' class='InputLabel'>32</span>
                        </td>
                        <td class='td1' style='vertical-align: middle; padding-left: 20px'>
                            <span class='InputLabel' style='padding-right: 10px; vertical-align: middle' id='lblHarmBodyParts'>Увредени
                                части на тялото:</span>
                        </td>
                        <td align='left'>
                            <textarea id='txtHarmBodyParts' cols='3' rows='4' class='InputField' style='width: 540px'>
" + declarationOfAccident.DeclarationOfAccidentHarm.HarmBodyParts + @"</textarea>
                        </td>
                    </tr>
                    <tr>
                        <td class='TdVertical'>
                        </td>
                        <td>
                            <div style='min-height: 15px'>
                            </div>
                        </td>
                        <td>
                            <div id='lblHarmBodyPartsAdditional' style='width: 540px; text-align: left; font-size: 11px'>
                            Посочват се увредените части на тялото. Когато има две еднакви части на тялото, 
                            се посочва която от тях е увредена - лява, дясна или и двете.
                            </div>
                            <div style='min-height: 12px'>
                            </div>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='TdVertical' style='vertical-align: text-top'>
                            <span style='font-weight: bold; font-size: larger' class='InputLabel'>33</span>
                        </td>
                        <td colspan='2' class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' style='padding-right: 122px' id='lblHarmResult'>Последици: </span>
                            <div style='display: inline-table'>
                                <div>
                                    <input id='rbHarmResult1' type='radio' onclick='TabHerVisibility(1);' name='groupHarmResult' " + DeclarationOfAccidentUtil.GetChekedStatus(1, declarationOfAccident.DeclarationOfAccidentHarm.HarmResult) + @"/>
                                    <span class='InputLabel' id='lblHarmResult1'>- смърт </span>
                                </div>
                                <div>
                                    <input id='rbHarmResult2' type='radio' onclick='TabHerVisibility(2);' name='groupHarmResult' value='rbHarmResult2' " + DeclarationOfAccidentUtil.GetChekedStatus(2, declarationOfAccident.DeclarationOfAccidentHarm.HarmResult) + @"/>
                                    <span class='InputLabel' id='lblHarmResult2'>- вероятна инвалидност </span>
                                </div>
                                <div>
                                    <input id='rbHarmResult3' type='radio' onclick='TabHerVisibility(3);' name='groupHarmResult' value='rbHarmResult3' " + DeclarationOfAccidentUtil.GetChekedStatus(3, declarationOfAccident.DeclarationOfAccidentHarm.HarmResult) + @"/>
                                    <span class='InputLabel' id='lblHarmResult3'>- временна неработоспособност </span>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class='TdVertical'>
                        </td>
                        <td colspan='2'>
                            <div style='min-height: 60px'>
                            </div>
                        </td>
                    </tr>
                </table>
<input type='hidden' id='tabDisabledControlsHarm' value='" + SetListDisabledControls() + @"'/>
<input type='hidden' id='tabHiddenControlsHarm' value='" + SetListHiddenControls() + @"'/>
";
            return result;
        }
        //Create XML string for TabWith
        private string TabWith()
        {
            string result = "";
            result = @"<table id='TabWith' class='ManagedNoticeOfAccident'>
                    <tr>
                        <td>
                            <div style='min-height: 50px'>
                            </div>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' id='lblWitnessFullName'>Трите имена:</span>
                            <input type='text' id='txtWitnessFullName' class='InputField' style='width: 420px'
                                maxlength='500' value='" + declarationOfAccident.DeclarationOfAccidentWith.WitnessFullName + @"'</input>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='td1' style='padding-left: 20px'>
                            </span> <span class='InputLabel' id='lblWithRegion'>Област: </span>
 <select id='ddlWithRegion' onchange='ddlChange(this, 1)' style='width: 200px'>" + initialListRegion +

                           @"
                            </select>
                            <span class='InputLabel' style='padding-left: 40px' id='lblWithMunicipality'>Община:</span>
<select id='ddlWithMunicipality' onchange='ddlChange(this, 2)' style='width: 200px'>" + initialListMunicipality +

                           @"
                           
                            </select>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' style='padding-left: 15px' id='lblWithCity'>Град:</span>
 <select id='ddlWithCity' onchange='ddlChange(this, 3)' style='width: 200px'>  " + initialListCity +

                           @" 
                     
                            </select>
                            <span class='InputLabel' style='padding-left: 48px' id='lblWitStreet'>Ул:</span>
                            <input type='text' id='txtWitStreet' class='InputField' style='width: 250px' maxlength='300' value='
" + declarationOfAccident.DeclarationOfAccidentWith.WitStreet + @"'></input>
                            <span class='InputLabel' style='padding-left: 20px' id='lblWitStreetNum'>№:</span>
                            <input type='text' id='txtWitStreetNum' class='InputField' style='width: 30px' maxlength='20' value='
" + declarationOfAccident.DeclarationOfAccidentWith.WitStreetNum + @"'></input>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' style='padding-left: 23px' id='lblWitDistrict'>Жк.:</span>
                            <input type='text' id='txtWitDistrict' class='InputField' style='width: 227px' maxlength='200' value='
" + declarationOfAccident.DeclarationOfAccidentWith.WitDistrict + @"'></input>
                            <span class='InputLabel' style='padding-left: 12px' id='lblWitBlock'>Бл.:</span>
                            <input type='text' id='txtWitBlock' class='InputField' style='width: 35px' maxlength='20' value='
" + declarationOfAccident.DeclarationOfAccidentWith.WitBlock + @"'></input>
                            <span class='InputLabel' style='padding-left: 10px' id='lblWitEntrance'>Вх.:</span>
                            <input type='text' id='txtWitEntrance' class='InputField' style='width: 35px' maxlength='20' value='
" + declarationOfAccident.DeclarationOfAccidentWith.WitEntrance + @"'></input>
                            <span class='InputLabel' style='padding-left: 10px' id='lblWitFloor'>Ет.:</span>
                            <input type='text' id='txtWitFloor' class='InputField' style='width: 35px' maxlength='20' value='
" + declarationOfAccident.DeclarationOfAccidentWith.WitFloor + @"'></input>
                            <span class='InputLabel' style='padding-left: 10px' id='lblWitApt'>Ап.:</span>
                            <input type='text' id='txtWitApt' class='InputField' style='width: 35px' maxlength='20' value='
" + declarationOfAccident.DeclarationOfAccidentWith.WitApt + @"'></input>
                            <span class='InputLabel' style='padding-left: 10px' id='lblWitPostCode'>Пощенски Код:</span>
                            <input onfocus='SetOldPostCode(this)' onblur='FillCityFromPostCode(this)' type='text' id='txtWitPostCode' class='InputField' style='width: 50px' value='
" + initialPostCode + @"'></input>
</input>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='td1' style='padding-left: 10px'>
                            <span class='InputLabel' id='lblWitPhone'>Телефон:</span>
                            <input type='text' id='txtWitPhone' class='InputField' style='width: 130px' maxlength='50' value='
" + declarationOfAccident.DeclarationOfAccidentWith.WitPhone + @"'></input>
                            <span class='InputLabel' style='padding-left: 20px' id='lblWitFax'>Факс:</span>
                            <input type='text' id='txtWitFax' class='InputField' style='width: 130px' maxlength='50' value='
" + declarationOfAccident.DeclarationOfAccidentWith.WitFax + @"'></input>
                            <span class='InputLabel' style='padding-left: 20px' id='lblWitEmail'>E-mail:</span>
                            <input type='text' id='txtWitEmail' class='InputField' style='width: 130px' maxlength='500' value='
" + declarationOfAccident.DeclarationOfAccidentWith.WitEmail + @"'></input>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style='min-height: 60px'>
                            </div>
                        </td>
                    </tr>
                </table>
<input type='hidden' id='tabDisabledControlsWith' value='" + SetListDisabledControls() + @"'/>
<input type='hidden' id='tabHiddenControlsWith' value='" + SetListHiddenControls() + @"'/>
";
            return result;
        }
        //Create XML string for TabHeir
        private string TabHeir()
        {
            string result = "";
            result = @"<table id='TabHeir' class='ManagedNoticeOfAccident'>
                    <tr>
                        <td>
                            <div style='min-height: 50px'>
                            </div>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' id='lblHeirFullName'>Трите имена:</span>
                            <input type='text' id='txtHeirFullName' class='InputField' style='width: 420px' maxlength='500' value='" + declarationOfAccident.DeclarationOfAccidentHeir.HeirFullName + @"'></input>
                            <span class='InputLabel' id='lblHeirEgn'>ЕГН:</span>
                            <input type='text' id='txtHeirEgn' class='InputField' style='width: 115px' maxlength='10' value='" + declarationOfAccident.DeclarationOfAccidentHeir.HeirEgn + @"'></input>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='td1' style='padding-left: 20px'>
                            <span style='font-weight: bold; font-size: larger' class='InputLabel' id='lblHeirAddressHeader'>Постоянен адрес:</span>
                            </span> <span class='InputLabel' id='lblHeirRegion'>Област: </span>
 <select id='ddlHeirRegion' onchange='ddlChange(this, 1)' style='width: 200px'> " + initialListRegion +

                           @" 

                            </select>
                            <span class='InputLabel' style='padding-left: 40px' id='lblHeirMunicipality'>Община:</span>
  <select id='ddlHeirMunicipality' onchange='ddlChange(this, 2)' style='width: 200px'> " + initialListMunicipality +

                           @" 

                           
                            </select>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' style='padding-left: 15px' id='lblHeirCity'>Град:</span>
<select id='ddlHeirCity' onchange='ddlChange(this, 3)' style='width: 200px'> " + initialListCity +

                           @" 
                           
                            </select>
                            <span class='InputLabel' style='padding-left: 48px' id='lblHeirStreet'>Ул:</span>
                            <input type='text' id='txtHeirStreet' class='InputField' style='width: 250px' maxlength='300' value='" + declarationOfAccident.DeclarationOfAccidentHeir.HeirStreet + @"'></input>
                            <span class='InputLabel' style='padding-left: 20px' id='lblHeirStreetNum'>№:</span>
                            <input type='text' id='txtHeirStreetNum' class='InputField' style='width: 30px' maxlength='20' value='" + declarationOfAccident.DeclarationOfAccidentHeir.HeirStreetNum + @"'></input>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='td1' style='padding-left: 20px'>
                            <span class='InputLabel' style='padding-left: 23px' id='lblHeirDistrict'>Жк.:</span>
                            <input type='text' id='txtHeirDistrict' class='InputField' style='width: 227px' maxlength='200' value='" + declarationOfAccident.DeclarationOfAccidentHeir.HeirDistrict + @"'></input>
                            <span class='InputLabel' style='padding-left: 12px' id='lblHeirBlock'>Бл.:</span>
                            <input type='text' id='txtHeirBlock' class='InputField' style='width: 35px' maxlength='20' value='" + declarationOfAccident.DeclarationOfAccidentHeir.HeirBlock + @"'></input>
                            <span class='InputLabel' style='padding-left: 10px' id='lblHeirEntrance'>Вх.:</span>
                            <input type='text' id='txtHeirEntrance' class='InputField' style='width: 35px' maxlength='20' value='" + declarationOfAccident.DeclarationOfAccidentHeir.HeirEntrance + @"'></input>
                            <span class='InputLabel' style='padding-left: 10px' id='lblHeirFloor'>Ет.:</span>
                            <input type='text' id='txtHeirFloor' class='InputField' style='width: 35px' maxlength='20' value='" + declarationOfAccident.DeclarationOfAccidentHeir.HeirFloor + @"'></input>
                            <span class='InputLabel' style='padding-left: 10px' id='lblHeirApt'>Ап.:</span>
                            <input type='text' id='txtHeirApt' class='InputField' style='width: 35px' maxlength='20'value='" + declarationOfAccident.DeclarationOfAccidentHeir.HeirApt + @"'></input>
                            <span class='InputLabel' style='padding-left: 10px'  id='lblHeirPostCode'>Пощенски Код:</span>
                            <input onfocus='SetOldPostCode(this)' onblur='FillCityFromPostCode(this)' type='text' id='txtHeirPostCode' class='InputField' style='width: 50px' value='
" + initialPostCode + @"'></input>
</input>
                        </td>
                    </tr>
                    <tr style='min-height: 35px'>
                        <td class='td1' style='padding-left: 10px'>
                            <span class='InputLabel' id='lblHeirPhone'>Телефон:</span>
                            <input type='text' id='txtHeirPhone' class='InputField' style='width: 130px' maxlength='50' value='" + declarationOfAccident.DeclarationOfAccidentHeir.HeirPhone + @"'></input>
                            <span class='InputLabel' style='padding-left: 20px' id='lblHeirFax'>Факс:</span>
                            <input type='text' id='txtHeirFax' class='InputField' style='width: 130px' maxlength='50' value='" + declarationOfAccident.DeclarationOfAccidentHeir.HeirFax + @"'></input>
                            <span class='InputLabel' style='padding-left: 20px' id='lblHeirEmail'>E-mail:</span>
                            <input type='text' id='txtHeirEmail' class='InputField' style='width: 130px' maxlength='500' value='" + declarationOfAccident.DeclarationOfAccidentHeir.HeirEmail + @"'></input>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div style='min-height: 60px'>
                            </div>
                        </td>
                    </tr>
                </table>
<input type='hidden' id='tabDisabledControlsHeir' value='" + SetListDisabledControls() + @"'/>
<input type='hidden' id='tabHiddenControlsHeir' value='" + SetListHiddenControls() + @"'/>
";
            return result;
        }
        //Fill Drop Down List for Regions
        private void GetInitialListEmpoyee(Employer employer)
        {
            int employerId = employer.EmployerId;

            string result = "";
            int paramIf = 0;

            if (this.declarationId > 0)
            {
                paramIf = employerId;
            }
            else
            {
                ListItem li = ListItems.GetOptionChooseOne();
                result += "<option value='" + li.Value + "' selected='selected'>" + li.Text + "</option>";
            }

            List<Employer> listEmployer = new List<Employer>();
            listEmployer = EmployerUtil.GetEmployers(CurrentUser);
            for (int i = 0; i <= listEmployer.Count - 1; i++)
            {
                if ((listEmployer[i].EmployerId == paramIf) && (employerId > 0))
                {
                    result += "<option value='" + listEmployer[i].EmployerId + "' selected='selected'>" + listEmployer[i].EmployerName + "</option>";
                }
                else
                {
                    result += "<option value='" + listEmployer[i].EmployerId + "'>" + listEmployer[i].EmployerName + "</option>";

                }
            }
            initialListEmpoyee = result;
        }
        //Fill Drop Down List for Regions
        private void GetInitialListRegion()
        {

            string result = "";
            List<Region> listRegion = new List<Region>();
            listRegion = RegionUtil.GetRegions(CurrentUser);

            if (listResult.Count > 0)
            {
                for (int i = 0; i <= listRegion.Count - 1; i++)
                {
                    if (listRegion[i].RegionId == listResult[0])
                    {
                        GetInitialListMunicipality(listRegion[i].RegionId);
                        result += "<option value='" + listRegion[i].RegionId + "' selected='selected'>" + listRegion[i].RegionName + "</option>";
                    }
                    else
                    {
                        result += "<option value='" + listRegion[i].RegionId + "'>" + listRegion[i].RegionName + "</option>";

                    }
                }
            }
            else //We set 1st element selected
            {
                for (int i = 0; i <= listRegion.Count - 1; i++)
                {
                    if (i == 0)
                    {
                        GetInitialListMunicipality(listRegion[i].RegionId);
                        result += "<option value='" + listRegion[i].RegionId + "' selected='selected'>" + listRegion[i].RegionName + "</option>";
                    }
                    else
                    {
                        result += "<option value='" + listRegion[i].RegionId + "'>" + listRegion[i].RegionName + "</option>";

                    }
                }

            }

            initialListRegion = result;
        }
        //Fill Drop Down List for Municipality
        private void GetInitialListMunicipality(int regionId)
        {
            string result = "";
            List<Municipality> listMunicipality = new List<Municipality>();

            listMunicipality = MunicipalityUtil.GetMunicipalities(regionId, CurrentUser);


            if (listResult.Count > 0)
            {
                for (int i = 0; i <= listMunicipality.Count - 1; i++)
                {
                    if (listMunicipality[i].MunicipalityId == listResult[1])
                    {
                        GetInitialListCity(listMunicipality[i].MunicipalityId);
                        result += "<option value='" + listMunicipality[i].MunicipalityId + "' selected='selected'>" + listMunicipality[i].MunicipalityName + "</option>";
                    }
                    else
                    {
                        result += "<option value='" + listMunicipality[i].MunicipalityId + "'>" + listMunicipality[i].MunicipalityName + "</option>";

                    }
                }
            }
            else //We set 1st element selected
            {
                for (int i = 0; i <= listMunicipality.Count - 1; i++)
                {
                    if (i == 0)
                    {
                       // GetInitialListCity(listMunicipality[i].MunicipalityId);
                        initialListCity = "<option value=''></option>";
                        result += "<option value=''></option>";
                    }
                    //else
                    //{
                    result += "<option value='" + listMunicipality[i].MunicipalityId + "'>" + listMunicipality[i].MunicipalityName + "</option>";
                    // }
                }
            }

            initialListMunicipality = result;
        }
        //Fill Drop Down List for City
        private void GetInitialListCity(int municipalityId)
        {
            string result = "";
            List<City> listCity = new List<City>();

            City city = new City(CurrentUser);
            listCity = CityUtil.GetCities(municipalityId, CurrentUser);

            if (listResult.Count > 0)
            {
                for (int i = 0; i <= listCity.Count - 1; i++)
                {
                    if (listCity[i].CityId == listResult[2])
                    {
                        result += "<option value='" + listCity[i].CityId + "' selected='selected'>" + listCity[i].CityName + "</option>";

                        initialPostCode = DeclarationOfAccidentUtil.GetPostCode(listCity[i].CityId, CurrentUser);
                    }
                    else
                    {
                        result += "<option value='" + listCity[i].CityId + "'>" + listCity[i].CityName + "</option>";

                    }
                }
            }
            else //We set 1st element selected
            {
                for (int i = 0; i <= listCity.Count - 1; i++)
                {
                    if (i == 0)
                    {
                        // result += "<option value='" + listCity[i].CityId + "' selected='selected'>" + listCity[i].CityName + "</option>";
                        result += "<option value=''></option>";
                        // initialPostCode = DeclarationOfAccidentUtil.GetPostCode(listCity[i].CityId, CurrentUser);
                    }
                    // else
                    // {
                    result += "<option value='" + listCity[i].CityId + "'>" + listCity[i].CityName + "</option>";
                    // }
                }
            }

            initialListCity = result;
        }
        //Fill Drop Down List for Municipality, City and PostCode for current RegionId
        private void JSGetListMunicipality(int regionId)
        {
            string response = ""; //Hold XML for Municipality
            string responceCity = ""; //Hold XML for City and PostCode
            List<Municipality> listMunicipality = new List<Municipality>();

            listMunicipality = MunicipalityUtil.GetMunicipalities(regionId, CurrentUser);
            for (int i = 0; i <= listMunicipality.Count - 1; i++)
            {
                if (i == 0)
                {
                    responceCity = JSListCity(listMunicipality[i].MunicipalityId); //create XML for City and PostCode
                }

                response += "<municipality>";

                response += "<municipalityId>";
                response += listMunicipality[i].MunicipalityId;
                response += "</municipalityId>";

                response += "<municipalyName>";
                response += listMunicipality[i].MunicipalityName;
                response += "</municipalyName>";

                response += "</municipality>";

            }

            response = response + responceCity;

            AJAX a = new AJAX(response, this.Response);
            a.Write();
            Response.End();

        }
        //Create XML for City and PostCode and return String XML
        private string JSListCity(int municipalityId)
        {
            string response = "";
            string responsePostCode = "";
            List<City> listCity = new List<City>();

            listCity = CityUtil.GetCities(municipalityId, CurrentUser);
            for (int i = 0; i <= listCity.Count - 1; i++)
            {
                if (i == 0)
                {
                    //responsePostCode = "<postCode>" + DeclarationOfAccidentUtil.GetPostCode(listCity[i].CityId, CurrentUser) + "</postCode>";
                    responsePostCode = "<postCode></postCode>";
                }
                response += "<city>";
                response += "<cityId>";
                response += listCity[i].CityId;
                response += "</cityId>";
                response += "<cityName>";
                response += listCity[i].CityName;
                response += "</cityName>";
                response += "</city>";
            }
            return response + responsePostCode;
        }
        //Create XML for City and PostCode and Return Responce
        private void JSGetListCity(int municipalityId)
        {
            string response = "";
            string responsePostCode = "";
            List<City> listCity = new List<City>();

            listCity = CityUtil.GetCities(municipalityId, CurrentUser);
            for (int i = 0; i <= listCity.Count - 1; i++)
            {
                if (i == 0)
                {
                    //responsePostCode = "<postCode>" + DeclarationOfAccidentUtil.GetPostCode(listCity[i].CityId, CurrentUser) + "</postCode>";
                    responsePostCode = "<postCode></postCode>";

                }
                response += "<city>";
                response += "<cityId>";
                response += listCity[i].CityId;
                response += "</cityId>";
                response += "<cityName>";
                response += listCity[i].CityName;
                response += "</cityName>";
                response += "</city>";
            }

            response += responsePostCode;

            AJAX a = new AJAX(response, this.Response);
            a.Write();
            Response.End();
        }
        //Create XML for PostCode and Return Responce
        private void JSGetPostCode(int cityId)
        {
            string response = "";
            int? postCode;

            postCode = DeclarationOfAccidentUtil.GetPostCode(cityId, CurrentUser);
            response = "<postCode>" + postCode + "</postCode>";

            AJAX a = new AJAX(response, this.Response);
            a.Write();
            Response.End();
        }

        //Create XML for using PostCode and Return Responce
        private void JSGetCityForPostCode(int postCode)
        {
            listResult = DeclarationOfAccidentUtil.GetListForPostCode(postCode, CurrentUser);

            string response = "";
            if (listResult.Count > 0)
            {
                response += "<statusResult>OK</statusResult>";

                response += "<selectedRegionId>" + listResult[0] + "</selectedRegionId>";
                response += "<selectedMunicipalityID>" + listResult[1] + "</selectedMunicipalityID>";
                response += "<selectedCityId>" + listResult[2] + "</selectedCityId>";

                //Bind Municipality List
                List<Municipality> listMunicipality = new List<Municipality>();

                listMunicipality = MunicipalityUtil.GetMunicipalities(listResult[0], CurrentUser);
                for (int i = 0; i <= listMunicipality.Count - 1; i++)
                {
                    response += "<municipality>";

                    response += "<municipalityId>";
                    response += listMunicipality[i].MunicipalityId;
                    response += "</municipalityId>";

                    response += "<municipalyName>";
                    response += listMunicipality[i].MunicipalityName;
                    response += "</municipalyName>";

                    response += "</municipality>";

                }

                //Bind City List
                List<City> listCity = new List<City>();

                listCity = CityUtil.GetCities(listResult[1], CurrentUser);
                for (int i = 0; i <= listCity.Count - 1; i++)
                {
                    response += "<city>";
                    response += "<cityId>";
                    response += listCity[i].CityId;
                    response += "</cityId>";
                    response += "<cityName>";
                    response += listCity[i].CityName;
                    response += "</cityName>";
                    response += "</city>";
                }

            }
            else
            {
                response += "<statusResult>NO</statusResult>";
            }

            AJAX a = new AJAX(response, this.Response);
            a.Write();
            Response.End();
        }

        //Create XML for tabEmpl and Return Responce
        private void JSGetTabEmpl(int employerId)
        {
            //Fill TabEml
            string response = "";

            Employer employer = new Employer(CurrentUser);
            employer = EmployerUtil.GetEmployerForEmployerId(employerId, CurrentUser);

            int cityId = 0;
            if (employer != null)
            {
                if (employer.City.CityId > 0)
                {
                    cityId = DBCommon.GetInt(employer.City.CityId);
                }
                response += "<EmplEik>" + employer.EmplEik + "</EmplEik>";
                response += "<EmplStreet>" + employer.EmplStreet + "</EmplStreet>";
                response += "<EmplStreetNum>" + employer.EmplStreetNum + "</EmplStreetNum>";
                response += "<EmplDistrict>" + employer.EmplDistrict + "</EmplDistrict>";
                response += "<EmplBlock>" + employer.EmplBlock + "</EmplBlock>";
                response += "<EmplEntrance>" + employer.EmplEntrance + "</EmplEntrance>";
                response += "<EmplFloor>" + employer.EmplFloor + "</EmplFloor>";
                response += "<EmplApt>" + employer.EmplApt + "</EmplApt>";
                response += "<EmplPhone>" + employer.EmplPhone + "</EmplPhone>";
                response += "<EmplFax>" + employer.EmplFax + "</EmplFax>";
                response += "<EmplEmail>" + employer.EmplEmail + "</EmplEmail>";
                response += "<EmplNumberOfEmployees>" + employer.EmplNumberOfEmployees + "</EmplNumberOfEmployees>";
                response += "<EmplFemaleEmployees>" + employer.EmplFemaleEmployees + "</EmplFemaleEmployees>";
            }

            //Get RegionId, MunicipalityId and PostCode for this city

            listResult = DeclarationOfAccidentUtil.GetListForEmployerCityId(cityId, CurrentUser);

            response += "<RegionId>" + listResult[0] + "</RegionId>"; //RegionId
            response += "<MunicipalityId>" + listResult[1] + "</MunicipalityId>"; //MunicipalityId
            response += "<PostCode>" + listResult[2] + "</PostCode>"; //PostCode

            response += "<CityId>" + cityId + "</CityId>"; //CityId. This is and selected City in ddl

            //CreateList for Municipalyty and Cities

            List<Municipality> listMunicipality = new List<Municipality>();

            listMunicipality = MunicipalityUtil.GetMunicipalities(listResult[0], CurrentUser);

            for (int i = 0; i <= listMunicipality.Count - 1; i++)
            {
                response += "<municipality>";
                response += "<municipalityId>";
                response += listMunicipality[i].MunicipalityId;
                response += "</municipalityId>";
                response += "<municipalityName>";
                response += listMunicipality[i].MunicipalityName;
                response += "</municipalityName>";
                response += "</municipality>";
            }

            List<City> listCity = new List<City>();

            listCity = CityUtil.GetCities(listResult[1], CurrentUser);
            for (int i = 0; i <= listCity.Count - 1; i++)
            {
                response += "<city>";
                response += "<cityId>";
                response += listCity[i].CityId;
                response += "</cityId>";
                response += "<cityName>";
                response += listCity[i].CityName;
                response += "</cityName>";
                response += "</city>";
            }

            AJAX a = new AJAX(response, this.Response);
            a.Write();
            Response.End();
        }
        //Save data
        private void JSSaveData()
        {
            DeclarationOfAccident declarationOfAccident = new DeclarationOfAccident(this.declarationId, CurrentUser);

            //Fill DeclarationOfAccident with data from UI elemements

            if (Request.Params["txtDeclarationNumber"] != "")
            {
                declarationOfAccident.DeclarationOfAccidentHeader.DeclarationNumber = Request.Params["txtDeclarationNumber"];
            }
            if (Request.Params["txtReferenceNumber"] != "")
            {
                declarationOfAccident.DeclarationOfAccidentHeader.ReferenceNumber = Request.Params["txtReferenceNumber"];
            }
            if (Request.Params["txtReferenceDate"] != "")
            {
                declarationOfAccident.DeclarationOfAccidentHeader.ReferenceDate = CommonFunctions.ParseDate(Request.Params["txtReferenceDate"]);
            }
            if (Request.Params["txtDeclarationDate"] != "")
            {
                declarationOfAccident.DeclarationOfAccidentHeader.DeclarationDate = CommonFunctions.ParseDate(Request.Params["txtDeclarationDate"]);
            }
            if (Request.Params["txtFileNumber"] != "")
            {
                declarationOfAccident.DeclarationOfAccidentHeader.FileNumber = Request.Params["txtFileNumber"];
            }

            if (!String.IsNullOrEmpty(Request.Params["rblAplicantType"]))
            {
                declarationOfAccident.DeclarationOfAccidentFooter.ApplicantType = Int32.Parse(Request.Params["rblAplicantType"]);
            }

            if (!String.IsNullOrEmpty(Request.Params["txtAplicantEmplPosition"]))
            {
                declarationOfAccident.DeclarationOfAccidentFooter.AplicantPosition = Request.Params["txtAplicantEmplPosition"];
            }

            if (!String.IsNullOrEmpty(Request.Params["txtAplicantEmplName"]))
            {
                declarationOfAccident.DeclarationOfAccidentFooter.AplicantName = Request.Params["txtAplicantEmplName"];
            }

            //Fill DeclarationOfAccident with data from Tab Empl 
            declarationOfAccident.Employer = FillDeclarationOfAccidentEmpl();

            //Fill DeclarationOfAccident with data from Tab Worker
            if (Request.Params["TabWorkerStatus"] != null)
            {
                declarationOfAccident.DeclarationOfAccidentWorker = FillDeclarationOfAccidentWorker();
            }
            else
            {
                declarationOfAccident.DeclarationOfAccidentWorker = null;
            }
            //Fill DeclarationOfAccident with data from Tab Acc
            if (Request.Params["TabAccStatus"] != null)
            {
                declarationOfAccident.DeclarationOfAccidentAcc = FillDeclarationOfAccidentAcc();
            }
            else
            {
                declarationOfAccident.DeclarationOfAccidentAcc = null;
            }
            //Fill DeclarationOfAccident with data from Tab Harm
            if (Request.Params["TabHarmStatus"] != null)
            {
                declarationOfAccident.DeclarationOfAccidentHarm = FillDeclarationOfAccidentHarm();
            }
            else
            {
                declarationOfAccident.DeclarationOfAccidentHarm = null;
            }
            //Fill DeclarationOfAccident with data from Tab With
            if (Request.Params["TabWithStatus"] != null)
            {
                declarationOfAccident.DeclarationOfAccidentWith = FillDeclarationOfAccidentWith();
            }
            else
            {
                declarationOfAccident.DeclarationOfAccidentWith = null;
            }
            //Fill DeclarationOfAccident with data from Tab Heir
            if (Request.Params["TabHeirStatus"] != null)
            {
                declarationOfAccident.DeclarationOfAccidentHeir = FillDeclarationOfAccidentHeir();
            }
            else
            {
                declarationOfAccident.DeclarationOfAccidentHeir = null;
            }

            //Create Obect for log result in DB
            Change changeEntry = new Change(CurrentUser, "HS_DeclAcc");

            //Use this to hold new Id after Insert
            string newDeclarationId = "<newDeclarationId>";
            //This hold inforamation about operation
            string result = "<resultOperation>";
            //Using this for page refresh(F5)
            string locationHash = "<locationHash>";

            if (DeclarationOfAccidentUtil.SaveDeclarationOfAccident(declarationOfAccident, CurrentUser, changeEntry))
            {
                //Operation (INSERT or UPDTE) is success

                switch (this.declarationId)
                {
                    case 0:
                        result += "Декларация за трудова злополука е добавенa";
                        locationHash += "AddEditDeclarationOfAccident.aspx?declarationId=" + declarationOfAccident.DeclarationId.ToString();
                        break;

                    default:
                        result += "Данните на декларация за трудова злополука са обновени";
                        break;
                }
                //fill xml node with value
                newDeclarationId += declarationOfAccident.DeclarationId;

                //Set New status UI
                SetInitialPageUI(declarationOfAccident.DeclarationId);
                string tabShownNow = Request.Params["tabShownNow"];

                switch (tabShownNow)
                {
                    case "btnTabEmpl":
                        SetupPageUI_Header();
                        SetupPageUI_Empl();
                        break;
                    case "btnTabWorker":
                        SetupPageUI_Worker();
                        break;
                    case "btnTabAcc":
                        SetupPageUI_Acc();
                        break;
                    case "btnTabHarm":
                        SetupPageUI_Harm();
                        break;
                    case "btnTabWith":
                        SetupPageUI_With();
                        break;
                    case "btnTabHeir":
                        SetupPageUI_Heir();
                        break;
                    default:
                        break;
                }

                changeEntry.WriteLog(); //Write changes in DB
            }
            else //Operation was not success
            {
                switch (this.declarationId)
                {
                    case 0:
                        result += "Декларация за трудова злополука не е добавенa";
                        break;

                    default:
                        result += "Данните на декларация за трудова злополука не са обновени";
                        break;
                }
                //fill xml node with value
                newDeclarationId += this.declarationId;
            }

            newDeclarationId += "</newDeclarationId>";
            result += "</resultOperation>";
            locationHash += "</locationHash>";

            string disabledControlsList = "<disabledControlsList>" + SetListDisabledControls() + "</disabledControlsList>";
            string hiddenControlsList = "<hiddenControlsList>" + SetListHiddenControls() + "</hiddenControlsList>";

            string response = @"<response>" + result + newDeclarationId + disabledControlsList + hiddenControlsList + locationHash + "</response>";
            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();

        }
        //Chek Disable Hidden State
        private void JSChekDisableHiddenState()
        {
            SetInitialPageUI(this.declarationId);
            string selectedTab = Request.Params["selectedTabID"];
            switch (selectedTab)
            {
                case "btnTabEmpl":
                    SetupPageUI_Header();
                    SetupPageUI_Empl();
                    break;
                case "btnTabWorker":
                    SetupPageUI_Worker();
                    break;
                case "btnTabAcc":
                    SetupPageUI_Acc();
                    break;
                case "btnTabHarm":
                    SetupPageUI_Harm();
                    break;
                case "btnTabWith":
                    SetupPageUI_With();
                    break;
                case "btnTabHeir":
                    SetupPageUI_Heir();
                    break;
                default:
                    break;
            }

            string disabledControlsList = "<disabledControlsList>" + SetListDisabledControls() + "</disabledControlsList>";
            string hiddenControlsList = "<hiddenControlsList>" + SetListHiddenControls() + "</hiddenControlsList>";

            string response = @"<response>" + disabledControlsList + hiddenControlsList + "</response>";
            AJAX a = new AJAX(response, Response);
            a.Write();
            Response.End();

        }
        //Fill DeclarationOfAccident Object with data from Tab Empl
        private Employer FillDeclarationOfAccidentEmpl()
        {
            Employer employer = new Employer(CurrentUser);

            if (!String.IsNullOrEmpty(Request.Params["ddlEmployerId"]))
            {
                employer.EmployerId = DBCommon.GetInt(Request.Params["ddlEmployerId"]);

                //Set Emplyeer Name for this EmployeerId
                Employer employer2 = new Employer(CurrentUser);
                employer2 = EmployerUtil.GetEmployerForEmployerId(employer.EmployerId, CurrentUser);

                employer.EmployerName = employer2.EmployerName;

            }

            if (!String.IsNullOrEmpty(Request.Params["txtEmplEik"]))
            {
                employer.EmplEik = Request.Params["txtEmplEik"].ToString();
            }

            if (!String.IsNullOrEmpty(Request.Params["ddlEmplCityId"]))
            {
                int emplCityId = DBCommon.GetInt(Request.Params["ddlEmplCityId"]);
                employer.City = CityUtil.GetCity(emplCityId, CurrentUser);
            }

            if (!String.IsNullOrEmpty(Request.Params["txtEmplStreet"]))
            {
                employer.EmplStreet = Request.Params["txtEmplStreet"].ToString();

            }
            if (!String.IsNullOrEmpty(Request.Params["txtEmplStreetNum"]))
            {
                employer.EmplStreetNum = Request.Params["txtEmplStreetNum"].ToString();
            }
            if (!String.IsNullOrEmpty(Request.Params["txtEmplDistrict"]))
            {
                employer.EmplDistrict = Request.Params["txtEmplDistrict"].ToString();
            }
            if (!String.IsNullOrEmpty(Request.Params["txtEmplBlock"]))
            {
                employer.EmplBlock = Request.Params["txtEmplBlock"].ToString();
            }
            if (!String.IsNullOrEmpty(Request.Params["txtEmplEntrance"]))
            {
                employer.EmplEntrance = Request.Params["txtEmplEntrance"].ToString();
            }
            if (!String.IsNullOrEmpty(Request.Params["txtEmplFloor"]))
            {
                employer.EmplFloor = Request.Params["txtEmplFloor"].ToString();
            }
            if (!String.IsNullOrEmpty(Request.Params["txtEmplApt"]))
            {
                employer.EmplApt = Request.Params["txtEmplApt"].ToString();
            }
            if (!String.IsNullOrEmpty(Request.Params["txtEmplPhone"]))
            {
                employer.EmplPhone = Request.Params["txtEmplPhone"].ToString();
            }
            if (!String.IsNullOrEmpty(Request.Params["txtEmplFax"]))
            {
                employer.EmplFax = Request.Params["txtEmplFax"].ToString();
            }
            if (!String.IsNullOrEmpty(Request.Params["txtEmplEmail"]))
            {
                employer.EmplEmail = Request.Params["txtEmplEmail"].ToString();
            }
            if (!String.IsNullOrEmpty(Request.Params["txtEmplNumberOfEmployees"]))
            {
                employer.EmplNumberOfEmployees = DBCommon.GetInt(Request.Params["txtEmplNumberOfEmployees"]);
            }

            if (!String.IsNullOrEmpty(Request.Params["txtEmplFemaleEmployees"]))
            {
                employer.EmplFemaleEmployees = DBCommon.GetInt(Request.Params["txtEmplFemaleEmployees"]);
            }

            return employer;
        }
        //Fill DeclarationOfAccident Object with data from Tab Worker
        private DeclarationOfAccidentWorker FillDeclarationOfAccidentWorker()
        {
            DeclarationOfAccidentWorker declarationOfAccidentWorker = new DeclarationOfAccidentWorker(CurrentUser);

            //Full Name
            if (!String.IsNullOrEmpty(Request.Params["txtWorkerFullName"]))
            {
                declarationOfAccidentWorker.WorkerFullName = Request.Params["txtWorkerFullName"].ToString();
            }
            //EGN
            if (!String.IsNullOrEmpty(Request.Params["txtWorkerEgn"]))
            {
                declarationOfAccidentWorker.WorkerEgn = Request.Params["txtWorkerEgn"].ToString();
            }
            //CityId
            if (!String.IsNullOrEmpty(Request.Params["ddlWCityId"]))
            {
                declarationOfAccidentWorker.WCityId = DBCommon.GetInt(Request.Params["ddlWCityId"].ToString());
            }
            //Street
            if (!String.IsNullOrEmpty(Request.Params["txtWStreet"]))
            {
                declarationOfAccidentWorker.WStreet = Request.Params["txtWStreet"].ToString();
            }
            //StreetNum
            if (!String.IsNullOrEmpty(Request.Params["txtWStreetNum"]))
            {
                declarationOfAccidentWorker.WStreetNum = Request.Params["txtWStreetNum"].ToString();
            }
            //District
            if (!String.IsNullOrEmpty(Request.Params["txtWDistrict"]))
            {
                declarationOfAccidentWorker.WDistrict = Request.Params["txtWDistrict"].ToString();
            }
            //Block
            if (!String.IsNullOrEmpty(Request.Params["txtWBlock"]))
            {
                declarationOfAccidentWorker.WBlock = Request.Params["txtWBlock"].ToString();
            }
            //Entrance
            if (!String.IsNullOrEmpty(Request.Params["txtWEntrance"]))
            {
                declarationOfAccidentWorker.WEntrance = Request.Params["txtWEntrance"].ToString();
            }
            //Floor
            if (!String.IsNullOrEmpty(Request.Params["txtWFloor"]))
            {
                declarationOfAccidentWorker.WFloor = Request.Params["txtWFloor"].ToString();
            }
            //Apt
            if (!String.IsNullOrEmpty(Request.Params["txtWApt"]))
            {
                declarationOfAccidentWorker.WApt = Request.Params["txtWApt"].ToString();
            }
            //Phone
            if (!String.IsNullOrEmpty(Request.Params["txtWPhone"]))
            {
                declarationOfAccidentWorker.WPhone = Request.Params["txtWPhone"].ToString();
            }
            //Fax
            if (!String.IsNullOrEmpty(Request.Params["txtWFax"]))
            {
                declarationOfAccidentWorker.WFax = Request.Params["txtWFax"].ToString();
            }
            //Email
            if (!String.IsNullOrEmpty(Request.Params["txtWEmail"]))
            {
                declarationOfAccidentWorker.WEmail = Request.Params["txtWEmail"].ToString();
            }
            //BirthDate
            if (!String.IsNullOrEmpty(Request.Params["txtWBirthDate"]))
            {
                declarationOfAccidentWorker.WBirthDate = CommonFunctions.ParseDate(Request.Params["txtWBirthDate"]);
            }
            //Gender
            if (!String.IsNullOrEmpty(Request.Params["rbWGender"]))
            {
                declarationOfAccidentWorker.WGender = DBCommon.GetInt(Request.Params["rbWGender"].ToString());
            }
            //CitizenShip
            if (!String.IsNullOrEmpty(Request.Params["txtWCitizenship"]))
            {
                declarationOfAccidentWorker.WCitizenship = Request.Params["txtWCitizenship"].ToString();
            }
            //HiteType
            if (!String.IsNullOrEmpty(Request.Params["rbWHireType"]))
            {
                declarationOfAccidentWorker.WHireType = DBCommon.GetInt(Request.Params["rbWHireType"].ToString());
            }
            //WorkTime
            if (!String.IsNullOrEmpty(Request.Params["rbWorkTime"]))
            {
                declarationOfAccidentWorker.WWorkTime = DBCommon.GetInt(Request.Params["rbWorkTime"].ToString());
            }

            //HireDate
            if (!String.IsNullOrEmpty(Request.Params["txtWHireDate"]))
            {
                declarationOfAccidentWorker.WHireDate = CommonFunctions.ParseDate(Request.Params["txtWHireDate"]);
            }
            //JobTitle
            if (!String.IsNullOrEmpty(Request.Params["txtWJobTitle"]))
            {
                declarationOfAccidentWorker.WJobTitle = Request.Params["txtWJobTitle"].ToString();
            }

            //JobCode
            if (!String.IsNullOrEmpty(Request.Params["txtWJobCode"]))
            {
                declarationOfAccidentWorker.WJobCode = Request.Params["txtWJobCode"].ToString();
            }

            //JobCategory
            if (!String.IsNullOrEmpty(Request.Params["rbWJobCategory"]))
            {
                declarationOfAccidentWorker.WJobCategory = DBCommon.GetInt(Request.Params["rbWJobCategory"].ToString());
            }

            //YearsOnService
            if (!String.IsNullOrEmpty(Request.Params["txtWYearsOnService"]))
            {
                declarationOfAccidentWorker.WYearsOnService = DBCommon.GetInt(Request.Params["txtWYearsOnService"].ToString());
            }

            //CurrentJobYearsOnService
            if (!String.IsNullOrEmpty(Request.Params["txtWCurrentJobYearsOnService"]))
            {
                declarationOfAccidentWorker.WCurrentJobYearsOnService = DBCommon.GetInt(Request.Params["txtWCurrentJobYearsOnService"].ToString());
            }

            //Branch
            if (!String.IsNullOrEmpty(Request.Params["txtWBranch"]))
            {
                declarationOfAccidentWorker.WBranch = Request.Params["txtWBranch"].ToString();
            }

            return declarationOfAccidentWorker;
        }
        //Fill DeclarationOfAccident Object with data from Tab Accident
        private DeclarationOfAccidentAcc FillDeclarationOfAccidentAcc()
        {
            DeclarationOfAccidentAcc declarationOfAccidentAcc = new DeclarationOfAccidentAcc(CurrentUser);

            // AccDate
            if (!String.IsNullOrEmpty(Request.Params["txtAccDateTimeHours"]) && !String.IsNullOrEmpty(Request.Params["txtAccDateTimeMinutes"]) && !String.IsNullOrEmpty(Request.Params["txtAccDateTimeDay"]))
            {
                string dt = Request.Params["txtAccDateTimeDay"].ToString();

                string hr = Request.Params["txtAccDateTimeHours"].ToString();
                if (hr.Length == 1) hr = "0" + hr;

                string min = Request.Params["txtAccDateTimeMinutes"].ToString();
                if (min.Length == 1) min = "0" + min;

                string[] split = dt.Split(new Char[] { '.' });

                dt = split[2] + "-" + split[1] + "-" + split[0];

                declarationOfAccidentAcc.AccDateTime = Convert.ToDateTime(dt + "T" + hr + ":" + min);

            }

            //FromHour1
            if (!String.IsNullOrEmpty(Request.Params["txtAccWorkFromHour1"]))
            {
                declarationOfAccidentAcc.AccWorkFromHour1 = DBCommon.GetInt(Request.Params["txtAccWorkFromHour1"].ToString());
            }
            // FromMin1

            if (!String.IsNullOrEmpty(Request.Params["txtAccWorkFromMin1"]))
            {
                declarationOfAccidentAcc.AccWorkFromMin1 = DBCommon.GetInt(Request.Params["txtAccWorkFromMin1"].ToString());
            }

            //ToHour1
            if (!String.IsNullOrEmpty(Request.Params["txtAccWorkToHour1"]))
            {
                declarationOfAccidentAcc.AccWorkToHour1 = DBCommon.GetInt(Request.Params["txtAccWorkToHour1"].ToString());
            }
            // ToMin1

            if (!String.IsNullOrEmpty(Request.Params["txtAccWorkToMin1"]))
            {
                declarationOfAccidentAcc.AccWorkToMin1 = DBCommon.GetInt(Request.Params["txtAccWorkToMin1"].ToString());
            }

            //FromHour2
            if (!String.IsNullOrEmpty(Request.Params["txtAccWorkFromHour2"]))
            {
                declarationOfAccidentAcc.AccWorkFromHour2 = DBCommon.GetInt(Request.Params["txtAccWorkFromHour2"].ToString());
            }
            // FromMin2

            if (!String.IsNullOrEmpty(Request.Params["txtAccWorkFromMin2"]))
            {
                declarationOfAccidentAcc.AccWorkFromMin2 = DBCommon.GetInt(Request.Params["txtAccWorkFromMin2"].ToString());
            }

            //ToHour2
            if (!String.IsNullOrEmpty(Request.Params["txtAccWorkToHour2"]))
            {
                declarationOfAccidentAcc.AccWorkToHour2 = DBCommon.GetInt(Request.Params["txtAccWorkToHour2"].ToString());
            }

            // ToMin2
            if (!String.IsNullOrEmpty(Request.Params["txtAccWorkToMin2"]))
            {
                declarationOfAccidentAcc.AccWorkToMin2 = DBCommon.GetInt(Request.Params["txtAccWorkToMin2"].ToString());
            }

            //AccCountry
            if (!String.IsNullOrEmpty(Request.Params["txtAccCountry"]))
            {
                declarationOfAccidentAcc.AccCountry = Request.Params["txtAccCountry"].ToString();
            }

            //AccCity
            if (!String.IsNullOrEmpty(Request.Params["ddlAccCity"]))
            {
                declarationOfAccidentAcc.AccCityId = DBCommon.GetInt(Request.Params["ddlAccCity"].ToString());
            }

            //AccPlace
            if (!String.IsNullOrEmpty(Request.Params["txtAccPlace"]))
            {
                declarationOfAccidentAcc.AccPlace = Request.Params["txtAccPlace"].ToString();
            }

            //AccStreet
            if (!String.IsNullOrEmpty(Request.Params["txtAccStreet"]))
            {
                declarationOfAccidentAcc.AccStreet = Request.Params["txtAccStreet"].ToString();
            }

            //AccStreetNum
            if (!String.IsNullOrEmpty(Request.Params["txtAccStreetNum"]))
            {
                declarationOfAccidentAcc.AccStreetNum = Request.Params["txtAccStreetNum"].ToString();
            }

            //AccDistrict
            if (!String.IsNullOrEmpty(Request.Params["txtAccDistrict"]))
            {
                declarationOfAccidentAcc.AccDistrict = Request.Params["txtAccDistrict"].ToString();
            }

            //AccBlock
            if (!String.IsNullOrEmpty(Request.Params["txtAccBlock"]))
            {
                declarationOfAccidentAcc.AccBlock = Request.Params["txtAccBlock"].ToString();
            }

            //AccEntrance
            if (!String.IsNullOrEmpty(Request.Params["txtAccEntrance"]))
            {
                declarationOfAccidentAcc.AccEntrance = Request.Params["txtAccEntrance"].ToString();
            }

            //AccFloor
            if (!String.IsNullOrEmpty(Request.Params["txtAccFloor"]))
            {
                declarationOfAccidentAcc.AccFloor = Request.Params["txtAccFloor"].ToString();
            }

            //AccApt
            if (!String.IsNullOrEmpty(Request.Params["txtAccApt"]))
            {
                declarationOfAccidentAcc.AccApt = Request.Params["txtAccApt"].ToString();
            }

            //txtAccPhone
            if (!String.IsNullOrEmpty(Request.Params["txtAccPhone"]))
            {
                declarationOfAccidentAcc.AccPhone = Request.Params["txtAccPhone"].ToString();
            }

            //AccFax
            if (!String.IsNullOrEmpty(Request.Params["txtAccFax"]))
            {
                declarationOfAccidentAcc.AccFax = Request.Params["txtAccFax"].ToString();
            }

            //AccEmail
            if (!String.IsNullOrEmpty(Request.Params["txtAccEmail"]))
            {
                declarationOfAccidentAcc.AccEmail = Request.Params["txtAccEmail"].ToString();
            }

            //rbAccHappenedAt
            if (!String.IsNullOrEmpty(Request.Params["rbAccHappenedAt"]))
            {
                declarationOfAccidentAcc.AccHappenedAt = DBCommon.GetInt(Request.Params["rbAccHappenedAt"].ToString());

                if (declarationOfAccidentAcc.AccHappenedAt == 3)
                {
                    declarationOfAccidentAcc.AccHappenedOther = Request.Params["txtAccHappenedAtOther"].ToString();
                }
            }

            //AccJobType
            if (!String.IsNullOrEmpty(Request.Params["txtAccJobType"]))
            {
                declarationOfAccidentAcc.AccJobType = Request.Params["txtAccJobType"].ToString();
            }

            //AccTaskType
            if (!String.IsNullOrEmpty(Request.Params["txtAccTaskType"]))
            {
                declarationOfAccidentAcc.AccTaskType = Request.Params["txtAccTaskType"].ToString();
            }

            //AccDeviationFromTask
            if (!String.IsNullOrEmpty(Request.Params["txtAccDeviationFromTask"]))
            {
                declarationOfAccidentAcc.AccDeviationFromTask = Request.Params["txtAccDeviationFromTask"].ToString();
            }

            //AccInjurDesc
            if (!String.IsNullOrEmpty(Request.Params["txtAccInjurDesc"]))
            {
                declarationOfAccidentAcc.AccInjurDesc = Request.Params["txtAccInjurDesc"].ToString();
            }

            //rbAccInjHasRights
            if (!String.IsNullOrEmpty(Request.Params["rbAccInjHasRights"]))
            {
                declarationOfAccidentAcc.AccInjHasRights = DBCommon.GetInt(Request.Params["rbAccInjHasRights"].ToString());
            }

            //rbAccLegalRef
            if (!String.IsNullOrEmpty(Request.Params["rbAccLegalRef"]))
            {
                declarationOfAccidentAcc.AccLegalRef = DBCommon.GetInt(Request.Params["rbAccLegalRef"].ToString());
            }

            //AccPlannedActions
            if (!String.IsNullOrEmpty(Request.Params["txtAccPlannedActions"]))
            {
                declarationOfAccidentAcc.AccPlannedActions = Request.Params["txtAccPlannedActions"].ToString();
            }

            //AccLostDays
            if (!String.IsNullOrEmpty(Request.Params["txtAccLostDays"]))
            {
                declarationOfAccidentAcc.AccLostDays = DBCommon.GetInt(Request.Params["txtAccLostDays"].ToString());
            }

            return declarationOfAccidentAcc;

        }
        //Fill DeclarationOfAccident Object with data from Tab Harm
        private DeclarationOfAccidentHarm FillDeclarationOfAccidentHarm()
        {
            DeclarationOfAccidentHarm declarationOfAccidentHarm = new DeclarationOfAccidentHarm();
            //HarmType
            if (!String.IsNullOrEmpty(Request.Params["txtHarmType"]))
            {
                declarationOfAccidentHarm.HarmType = Request.Params["txtHarmType"].ToString();
            }

            //HarmBodyParts
            if (!String.IsNullOrEmpty(Request.Params["txtHarmBodyParts"]))
            {
                declarationOfAccidentHarm.HarmBodyParts = Request.Params["txtHarmBodyParts"].ToString();
            }

            //HarmResult
            if (!String.IsNullOrEmpty(Request.Params["rbHarmResult"]))
            {
                declarationOfAccidentHarm.HarmResult = DBCommon.GetInt(Request.Params["rbHarmResult"].ToString());
            }


            return declarationOfAccidentHarm;
        }
        //Fill DeclarationOfAccident Object with data from Tab With
        private DeclarationOfAccidentWith FillDeclarationOfAccidentWith()
        {
            DeclarationOfAccidentWith declarationOfAccidentWith = new DeclarationOfAccidentWith(CurrentUser);

            //WitnessFullName
            if (!String.IsNullOrEmpty(Request.Params["txtWitnessFullName"]))
            {
                declarationOfAccidentWith.WitnessFullName = Request.Params["txtWitnessFullName"].ToString();
            }
            //WitCityId
            if (!String.IsNullOrEmpty(Request.Params["ddlWithCity"]))
            {
                declarationOfAccidentWith.WitCityId = DBCommon.GetInt(Request.Params["ddlWithCity"].ToString());
            }

            //WitStreet
            if (!String.IsNullOrEmpty(Request.Params["txtWitStreet"]))
            {
                declarationOfAccidentWith.WitStreet = Request.Params["txtWitStreet"].ToString();
            }
            //WitStreetNum
            if (!String.IsNullOrEmpty(Request.Params["txtWitStreetNum"]))
            {
                declarationOfAccidentWith.WitStreetNum = Request.Params["txtWitStreetNum"].ToString();
            }
            //WitDistrict
            if (!String.IsNullOrEmpty(Request.Params["txtWitDistrict"]))
            {
                declarationOfAccidentWith.WitDistrict = Request.Params["txtWitDistrict"].ToString();
            }
            //WitBlock
            if (!String.IsNullOrEmpty(Request.Params["txtWitBlock"]))
            {
                declarationOfAccidentWith.WitBlock = Request.Params["txtWitBlock"].ToString();
            }
            //WitEntrance
            if (!String.IsNullOrEmpty(Request.Params["txtWitEntrance"]))
            {
                declarationOfAccidentWith.WitEntrance = Request.Params["txtWitEntrance"].ToString();
            }
            //WitFloor
            if (!String.IsNullOrEmpty(Request.Params["txtWitFloor"]))
            {
                declarationOfAccidentWith.WitFloor = Request.Params["txtWitFloor"].ToString();
            }
            //WitApt
            if (!String.IsNullOrEmpty(Request.Params["txtWitApt"]))
            {
                declarationOfAccidentWith.WitApt = Request.Params["txtWitApt"].ToString();
            }
            //WitPhone
            if (!String.IsNullOrEmpty(Request.Params["txtWitPhone"]))
            {
                declarationOfAccidentWith.WitPhone = Request.Params["txtWitPhone"].ToString();
            }
            //WitFax
            if (!String.IsNullOrEmpty(Request.Params["txtWitFax"]))
            {
                declarationOfAccidentWith.WitFax = Request.Params["txtWitFax"].ToString();
            }
            //WitEmail
            if (!String.IsNullOrEmpty(Request.Params["txtWitEmail"]))
            {
                declarationOfAccidentWith.WitEmail = Request.Params["txtWitEmail"].ToString();
            }

            return declarationOfAccidentWith;
        }
        //Fill DeclarationOfAccident Object with data from Tab Heir
        private DeclarationOfAccidentHeir FillDeclarationOfAccidentHeir()
        {
            DeclarationOfAccidentHeir declarationOfAccidentHeir = new DeclarationOfAccidentHeir(CurrentUser);

            //HeirnessFullName
            if (!String.IsNullOrEmpty(Request.Params["txtHeirFullName"]))
            {
                declarationOfAccidentHeir.HeirFullName = Request.Params["txtHeirFullName"].ToString();
            }

            //HeirEGN
            if (!String.IsNullOrEmpty(Request.Params["txtHeirEgn"]))
            {
                declarationOfAccidentHeir.HeirEgn = Request.Params["txtHeirEgn"].ToString();
            }

            //HeirCityId
            if (!String.IsNullOrEmpty(Request.Params["ddlHeirCity"]))
            {
                declarationOfAccidentHeir.HeirCityId = DBCommon.GetInt(Request.Params["ddlHeirCity"].ToString());
            }

            //HeirStreet
            if (!String.IsNullOrEmpty(Request.Params["txtHeirStreet"]))
            {
                declarationOfAccidentHeir.HeirStreet = Request.Params["txtHeirStreet"].ToString();
            }
            //HeirStreetNum
            if (!String.IsNullOrEmpty(Request.Params["txtHeirStreetNum"]))
            {
                declarationOfAccidentHeir.HeirStreetNum = Request.Params["txtHeirStreetNum"].ToString();
            }
            //HeirDistrict
            if (!String.IsNullOrEmpty(Request.Params["txtHeirDistrict"]))
            {
                declarationOfAccidentHeir.HeirDistrict = Request.Params["txtHeirDistrict"].ToString();
            }
            //HeirBlock
            if (!String.IsNullOrEmpty(Request.Params["txtHeirBlock"]))
            {
                declarationOfAccidentHeir.HeirBlock = Request.Params["txtHeirBlock"].ToString();
            }
            //HeirEntrance
            if (!String.IsNullOrEmpty(Request.Params["txtHeirEntrance"]))
            {
                declarationOfAccidentHeir.HeirEntrance = Request.Params["txtHeirEntrance"].ToString();
            }
            //HeirFloor
            if (!String.IsNullOrEmpty(Request.Params["txtHeirFloor"]))
            {
                declarationOfAccidentHeir.HeirFloor = Request.Params["txtHeirFloor"].ToString();
            }
            //HeirApt
            if (!String.IsNullOrEmpty(Request.Params["txtHeirApt"]))
            {
                declarationOfAccidentHeir.HeirApt = Request.Params["txtHeirApt"].ToString();
            }
            //HeirPhone
            if (!String.IsNullOrEmpty(Request.Params["txtHeirPhone"]))
            {
                declarationOfAccidentHeir.HeirPhone = Request.Params["txtHeirPhone"].ToString();
            }
            //HeirFax
            if (!String.IsNullOrEmpty(Request.Params["txtHeirFax"]))
            {
                declarationOfAccidentHeir.HeirFax = Request.Params["txtHeirFax"].ToString();
            }
            //HeirEmail
            if (!String.IsNullOrEmpty(Request.Params["txtHeirEmail"]))
            {
                declarationOfAccidentHeir.HeirEmail = Request.Params["txtHeirEmail"].ToString();
            }

            return declarationOfAccidentHeir;
        }
        #endregion

        #region Methods

        private void SetInitialPageUI(int declarationId)
        {
            if (declarationId == 0)
            {
                prefix = "HS_ADD";
                screenHidden = (this.GetUIItemAccessLevel("HS_ADDDECLARATIONACC") != UIAccessLevel.Enabled) ||
                                     (this.GetUIItemAccessLevel("HS_DECLARATIONACC") != UIAccessLevel.Enabled);

                screenDisabled = (this.GetUIItemAccessLevel("HS_ADDDECLARATIONACC") == UIAccessLevel.Disabled) ||
                                     (this.GetUIItemAccessLevel("HS_DECLARATIONACC") == UIAccessLevel.Disabled);


                if (screenHidden)
                    RedirectAccessDenied();

                if (screenDisabled)
                {
                    this.hfBtnSave.Value = "Disable";
                }

                this.hdnTabHeirVisibility.Value = "hidden";
            }
            else //update mode
            {
                prefix = "HS_EDIT";
                screenHidden = (this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC") == UIAccessLevel.Hidden) ||
                                    (this.GetUIItemAccessLevel("HS_DECLARATIONACC") == UIAccessLevel.Hidden);

                screenDisabled = (this.GetUIItemAccessLevel("HS_EDITDECLARATIONACC") == UIAccessLevel.Disabled) ||
                                      (this.GetUIItemAccessLevel("HS_DECLARATIONACC") == UIAccessLevel.Disabled);

                if (screenHidden)
                    RedirectAccessDenied();

                if (screenDisabled)
                {
                    this.hfBtnSave.Value = "Disable";
                }
                else

                    //Get harmType to set visibility of Tab Heir
                    declarationOfAccident = DeclarationOfAccidentUtil.GetDeclarationOfAccident(declarationId, CurrentUser, "btnTabHarm");
                if (declarationOfAccident.DeclarationOfAccidentHarm.HarmResult == 1)
                {
                    this.hdnTabHeirVisibility.Value = "visible";
                }
                else
                {
                    this.hdnTabHeirVisibility.Value = "hidden";
                }
            }

        }
        // Setup user interface elements according to rights of the user's role
        private void SetupPageUI_Header()
        {
            disabledClientControls = new List<string>();
            hiddenClientControls = new List<string>();

            //Server Controls
            l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_DECLARATIONNUMBER");
            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                this.pageDisabledControls.Add(lblDeclarationNumber);
                this.pageDisabledControls.Add(txtDeclarationNumber);
            }
            if (l == UIAccessLevel.Hidden)
            {
                this.pageHiddenControls.Add(lblDeclarationNumber);
                this.pageHiddenControls.Add(txtDeclarationNumber);
            }

            l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_DECLARATIONDATE");
            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                this.pageDisabledControls.Add(lblDeclarationDate);
                this.pageDisabledControls.Add(txtDeclarationDate);
            }
            if (l == UIAccessLevel.Hidden)
            {
                this.pageHiddenControls.Add(lblDeclarationDate);
                this.pageHiddenControls.Add(txtDeclarationDate);
            }

            l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_REFERENCENUMBER");
            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                this.pageDisabledControls.Add(lblReferenceNumber);
                this.pageDisabledControls.Add(txtReferenceNumber);
            }
            if (l == UIAccessLevel.Hidden)
            {
                this.pageHiddenControls.Add(lblReferenceNumber);
                this.pageHiddenControls.Add(txtReferenceNumber);
            }

            l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_REFERENCEDATE");
            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                this.pageDisabledControls.Add(lblReferenceDate);
                this.pageDisabledControls.Add(txtReferenceDate);
            }
            if (l == UIAccessLevel.Hidden)
            {
                this.pageHiddenControls.Add(lblReferenceDate);
                this.pageHiddenControls.Add(txtReferenceDate);
            }

            l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_FILENUMBER");
            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                this.pageDisabledControls.Add(lblFileNumber);
                this.pageDisabledControls.Add(txtFileNumber);
            }
            if (l == UIAccessLevel.Hidden)
            {
                this.pageHiddenControls.Add(lblFileNumber);
                this.pageHiddenControls.Add(txtFileNumber);
            }

            l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_APLICANTTYPE");
            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                this.pageDisabledControls.Add(rblAplicantType);
                this.pageDisabledControls.Add(txtAplicantEmplPosition);
                this.pageDisabledControls.Add(txtAplicantEmplName);
            }
            if (l == UIAccessLevel.Hidden)
            {
                this.pageHiddenControls.Add(lblAplicantinfo);
                this.pageHiddenControls.Add(rblAplicantType);
                this.pageHiddenControls.Add(txtAplicantEmplPosition);
                this.pageHiddenControls.Add(txtAplicantEmplName);
            }

            // Client TAB Controls

            l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_EMLP");
            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                //Disable all UI in this Tab. We do this later in code.
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("btnTabEmpl");
            }

            l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WORKER");
            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                //Disable all UI in this Tab. We do this later in code.
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("btnTabWorker");
            }

            l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_ACC");
            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                //Disable all UI in this Tab. We do this later in code.
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("btnTabAcc");
            }

            l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_HARM");
            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                //Disable all UI in this Tab. We do this later in code.
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("btnTabHarm");
            }

            l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WITH");
            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                //Disable all UI in this Tab. We do this later in code.
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("btnTabWith");
            }

            l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_HEIR");
            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                //Disable all UI in this Tab
            }
            if (l == UIAccessLevel.Hidden)
            {
                hiddenClientControls.Add("btnTabHeir");
            }

            //SetDisabledClientControls(disabledClientControls.ToArray());
            //SetHiddenClientControls(hiddenClientControls.ToArray());

        }
        private void SetupPageUI_Empl()
        {
            //disabledClientControls = new List<string>();
            //hiddenClientControls = new List<string>();

            //Chek on Tab Level
            l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_EMLP");

            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                //Disable all controls in tab
                disabledClientControls.Add("lblEmplAddress");

                disabledClientControls.Add("lblInsurerID");
                disabledClientControls.Add("ddlEmployerId");

                disabledClientControls.Add("lblInsIdentNum");
                disabledClientControls.Add("txtEmplEik");

                disabledClientControls.Add("ddlEmpRegion");
                disabledClientControls.Add("ddlEmpMunicipality");
                disabledClientControls.Add("ddlEmplCity");
                disabledClientControls.Add("txtEmplPostCode");

                disabledClientControls.Add("lblEmpRegion");
                disabledClientControls.Add("lblEmpMunicipality");
                disabledClientControls.Add("lblEmplCity");
                disabledClientControls.Add("lblEmplPostCode");

                disabledClientControls.Add("lblEmplStreet");
                disabledClientControls.Add("txtEmplStreet");

                disabledClientControls.Add("lblEmplStreetNum");
                disabledClientControls.Add("txtEmplStreetNum");

                disabledClientControls.Add("lblEmplDistrict");
                disabledClientControls.Add("txtEmplDistrict");

                disabledClientControls.Add("lblEmplBlock");
                disabledClientControls.Add("txtEmplBlock");

                disabledClientControls.Add("lblEmplEntrance");
                disabledClientControls.Add("txtEmplEntrance");

                disabledClientControls.Add("lblEmplFloor");
                disabledClientControls.Add("txtEmplFloor");

                disabledClientControls.Add("lblEmplApt");
                disabledClientControls.Add("txtEmplApt");

                disabledClientControls.Add("lblEmplPhone");
                disabledClientControls.Add("txtEmplPhone");

                disabledClientControls.Add("lblEmplFax");
                disabledClientControls.Add("txtEmplFax");

                disabledClientControls.Add("lblEmplEmail");
                disabledClientControls.Add("txtEmplEmail");

                disabledClientControls.Add("lblEmplNumberOfEmployees");
                disabledClientControls.Add("txtEmplNumberOfEmployees");

                disabledClientControls.Add("lblEmplFemaleEmployees");
                disabledClientControls.Add("txtEmplFemaleEmployees");

            }
            if (l == UIAccessLevel.Hidden || screenHidden)
            {
                //Hide all controls in tab because Empl tab is selected by default
                hiddenClientControls.Add("lblEmplAddress");

                hiddenClientControls.Add("lblInsurerID");
                hiddenClientControls.Add("ddlEmployerId");

                hiddenClientControls.Add("lblInsIdentNum");
                hiddenClientControls.Add("txtEmplEik");

                hiddenClientControls.Add("ddlEmpRegion");
                hiddenClientControls.Add("ddlEmpMunicipality");
                hiddenClientControls.Add("ddlEmplCity");
                hiddenClientControls.Add("txtEmplPostCode");

                hiddenClientControls.Add("lblEmpRegion");
                hiddenClientControls.Add("lblEmpMunicipality");
                hiddenClientControls.Add("lblEmplCity");
                hiddenClientControls.Add("lblEmplPostCode");

                hiddenClientControls.Add("lblEmplStreet");
                hiddenClientControls.Add("txtEmplStreet");

                hiddenClientControls.Add("lblEmplStreetNum");
                hiddenClientControls.Add("txtEmplStreetNum");

                hiddenClientControls.Add("lblEmplDistrict");
                hiddenClientControls.Add("txtEmplDistrict");

                hiddenClientControls.Add("lblEmplBlock");
                hiddenClientControls.Add("txtEmplBlock");

                hiddenClientControls.Add("lblEmplEntrance");
                hiddenClientControls.Add("txtEmplEntrance");

                hiddenClientControls.Add("lblEmplFloor");
                hiddenClientControls.Add("txtEmplFloor");

                hiddenClientControls.Add("lblEmplApt");
                hiddenClientControls.Add("txtEmplApt");

                hiddenClientControls.Add("lblEmplPhone");
                hiddenClientControls.Add("txtEmplPhone");

                hiddenClientControls.Add("lblEmplFax");
                hiddenClientControls.Add("txtEmplFax");

                hiddenClientControls.Add("lblEmplEmail");
                hiddenClientControls.Add("txtEmplEmail");

                hiddenClientControls.Add("lblEmplNumberOfEmployees");
                hiddenClientControls.Add("txtEmplNumberOfEmployees");

                hiddenClientControls.Add("lblEmplFemaleEmployees");
                hiddenClientControls.Add("txtEmplFemaleEmployees");
            }
            else
            {
                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_EMLP_EMPLOYERID");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblInsurerID");
                    disabledClientControls.Add("ddlEmployerId");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblInsurerID");
                    hiddenClientControls.Add("ddlEmployerId");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_EMLP_EMPLEIK");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblInsIdentNum");
                    disabledClientControls.Add("txtEmplEik");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblInsIdentNum");
                    hiddenClientControls.Add("txtEmplEik");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_EMLP_EMPLCITYID");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("ddlEmpRegion");
                    disabledClientControls.Add("ddlEmpMunicipality");
                    disabledClientControls.Add("ddlEmplCity");
                    disabledClientControls.Add("txtEmplPostCode");

                    disabledClientControls.Add("lblEmpRegion");
                    disabledClientControls.Add("lblEmpMunicipality");
                    disabledClientControls.Add("lblEmplCity");
                    disabledClientControls.Add("lblEmplPostCode");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("ddlEmpRegion");
                    hiddenClientControls.Add("ddlEmpMunicipality");
                    hiddenClientControls.Add("ddlEmplCity");
                    hiddenClientControls.Add("txtEmplPostCode");

                    hiddenClientControls.Add("lblEmpRegion");
                    hiddenClientControls.Add("lblEmpMunicipality");
                    hiddenClientControls.Add("lblEmplCity");
                    hiddenClientControls.Add("lblEmplPostCode");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_EMLP_EMPLSTREET");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblEmplStreet");
                    disabledClientControls.Add("txtEmplStreet");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEmplStreet");
                    hiddenClientControls.Add("txtEmplStreet");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_EMLP_EMPLSTREETNUM");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblEmplStreetNum");
                    disabledClientControls.Add("txtEmplStreetNum");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEmplStreetNum");
                    hiddenClientControls.Add("txtEmplStreetNum");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_EMLP_EMPLDISTRICT");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblEmplDistrict");
                    disabledClientControls.Add("txtEmplDistrict");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEmplDistrict");
                    hiddenClientControls.Add("txtEmplDistrict");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_EMLP_EMPLBLOCK");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblEmplBlock");
                    disabledClientControls.Add("txtEmplBlock");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEmplBlock");
                    hiddenClientControls.Add("txtEmplBlock");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_EMLP_EMPLENTRANCE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblEmplEntrance");
                    disabledClientControls.Add("txtEmplEntrance");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEmplEntrance");
                    hiddenClientControls.Add("txtEmplEntrance");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_EMLP_EMPLFLOOR");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblEmplFloor");
                    disabledClientControls.Add("txtEmplFloor");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEmplFloor");
                    hiddenClientControls.Add("txtEmplFloor");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_EMLP_EMPLAPT");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblEmplApt");
                    disabledClientControls.Add("txtEmplApt");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEmplApt");
                    hiddenClientControls.Add("txtEmplApt");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_EMLP_EMPLPHONE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblEmplPhone");
                    disabledClientControls.Add("txtEmplPhone");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEmplPhone");
                    hiddenClientControls.Add("txtEmplPhone");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_EMLP_EMPLFAX");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblEmplFax");
                    disabledClientControls.Add("txtEmplFax");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEmplFax");
                    hiddenClientControls.Add("txtEmplFax");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_EMLP_EMPLEMAIL");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblEmplEmail");
                    disabledClientControls.Add("txtEmplEmail");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEmplEmail");
                    hiddenClientControls.Add("txtEmplEmail");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_EMLP_EMPLNUMBEROFEMPLOYEES");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblEmplNumberOfEmployees");
                    disabledClientControls.Add("txtEmplNumberOfEmployees");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEmplNumberOfEmployees");
                    hiddenClientControls.Add("txtEmplNumberOfEmployees");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_EMLP_EMPLFEMALEEMPLOYEES");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblEmplFemaleEmployees");
                    disabledClientControls.Add("txtEmplFemaleEmployees");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblEmplFemaleEmployees");
                    hiddenClientControls.Add("txtEmplFemaleEmployees");
                }
            }
            //We use this way only for Empl tab to set visibility/hidden becouse this tab is buid to div on page and all client controls naow are available!!!
            SetDisabledClientControls(disabledClientControls.ToArray());
            SetHiddenClientControls(hiddenClientControls.ToArray());

        }
        private void SetupPageUI_Worker()
        {
            disabledClientControls = new List<string>();
            hiddenClientControls = new List<string>();

            //Chek on Tab Level
            l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WORKER");

            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                //Disable all controls in tab

                disabledClientControls.Add("lblWorkerAddress");
                disabledClientControls.Add("lblWHireTypeHeader");

                disabledClientControls.Add("lblWorkerFullName");
                disabledClientControls.Add("txtWorkerFullName");

                disabledClientControls.Add("lblWorkerEgn");
                disabledClientControls.Add("txtWorkerEgn");

                disabledClientControls.Add("ddlWRegion");
                disabledClientControls.Add("ddlWMunicipality");
                disabledClientControls.Add("ddlWCity");
                disabledClientControls.Add("txtWPostCode");

                disabledClientControls.Add("lblWRegion");
                disabledClientControls.Add("lblWMunicipality");
                disabledClientControls.Add("lblWCity");
                disabledClientControls.Add("lblWPostCode");

                disabledClientControls.Add("lblWStreet");
                disabledClientControls.Add("txtWStreet");

                disabledClientControls.Add("lblWStreetNum");
                disabledClientControls.Add("txtWStreetNum");

                disabledClientControls.Add("lblWDistrict");
                disabledClientControls.Add("txtWDistrict");

                disabledClientControls.Add("lblWBlock");
                disabledClientControls.Add("txtWBlock");

                disabledClientControls.Add("lblWEntrance");
                disabledClientControls.Add("txtWEntrance");

                disabledClientControls.Add("lblWFloor");
                disabledClientControls.Add("txtWFloor");

                disabledClientControls.Add("lblWApt");
                disabledClientControls.Add("txtWApt");


                disabledClientControls.Add("lblWPhone");
                disabledClientControls.Add("txtWPhone");

                disabledClientControls.Add("lblWFax");
                disabledClientControls.Add("txtWFax");

                disabledClientControls.Add("lblWEmail");
                disabledClientControls.Add("txtWEmail");

                disabledClientControls.Add("lblWBirthDate");
                disabledClientControls.Add("txtWBirthDate");

                disabledClientControls.Add("lblWGender");
                disabledClientControls.Add("lblWGender1");
                disabledClientControls.Add("lblWGender2");
                disabledClientControls.Add("rbWGender1");
                disabledClientControls.Add("rbWGender2");

                disabledClientControls.Add("lblWCitizenship");
                disabledClientControls.Add("txtWCitizenship");

                disabledClientControls.Add("rbWHireType1");
                disabledClientControls.Add("rbWHireType2");
                disabledClientControls.Add("lblWHireType");
                disabledClientControls.Add("lblWHireType1");
                disabledClientControls.Add("lblWHireType2");


                disabledClientControls.Add("rbWWorkTime1");
                disabledClientControls.Add("lblWWorkTime1");
                disabledClientControls.Add("rbWWorkTime2");
                disabledClientControls.Add("lblWWorkTime2");
                disabledClientControls.Add("lblWWorkTime");

                disabledClientControls.Add("lblWHireDate");
                disabledClientControls.Add("txtWHireDate");

                disabledClientControls.Add("lblWJobTitle");
                disabledClientControls.Add("txtWJobTitle");

                disabledClientControls.Add("lblWJobCode");
                disabledClientControls.Add("txtWJobCode");

                disabledClientControls.Add("rbWJobCategory1");
                disabledClientControls.Add("rbWJobCategory2");
                disabledClientControls.Add("rbWJobCategory3");
                disabledClientControls.Add("lblWJobCategory");
                disabledClientControls.Add("lblWJobCategory1");
                disabledClientControls.Add("lblWJobCategory2");
                disabledClientControls.Add("lblWJobCategory3");

                disabledClientControls.Add("lblWYearsOnService");
                disabledClientControls.Add("txtWYearsOnService");

                disabledClientControls.Add("lblWCurrentJobYearsOnService");
                disabledClientControls.Add("txtWCurrentJobYearsOnService");

                disabledClientControls.Add("lblWBranch");
                disabledClientControls.Add("txtWBranch");

            }
            else
            {
                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WORKER_WORKERFULLNAME");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblWorkerFullName");
                    disabledClientControls.Add("txtWorkerFullName");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWorkerFullName");
                    hiddenClientControls.Add("txtWorkerFullName");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WORKER_WORKEREGN");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblWorkerEgn");
                    disabledClientControls.Add("txtWorkerEgn");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWorkerEgn");
                    hiddenClientControls.Add("txtWorkerEgn");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WORKER_WCITYID");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("ddlWRegion");
                    disabledClientControls.Add("ddlWMunicipality");
                    disabledClientControls.Add("ddlWCity");
                    disabledClientControls.Add("txtWPostCode");

                    disabledClientControls.Add("lblWRegion");
                    disabledClientControls.Add("lblWMunicipality");
                    disabledClientControls.Add("lblWCity");
                    disabledClientControls.Add("lblWPostCode");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("ddlWRegion");
                    hiddenClientControls.Add("ddlWMunicipality");
                    hiddenClientControls.Add("ddlWCity");
                    hiddenClientControls.Add("txtWPostCode");

                    hiddenClientControls.Add("lblWRegion");
                    hiddenClientControls.Add("lblWMunicipality");
                    hiddenClientControls.Add("lblWCity");
                    hiddenClientControls.Add("lblWPostCode");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WORKER_WSTREET");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblWStreet");
                    disabledClientControls.Add("txtWStreet");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWStreet");
                    hiddenClientControls.Add("txtWStreet");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WORKER_WSTREETNUM");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblWStreetNum");
                    disabledClientControls.Add("txtWStreetNum");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWStreetNum");
                    hiddenClientControls.Add("txtWStreetNum");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WORKER_WDISTRICT");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblWDistrict");
                    disabledClientControls.Add("txtWDistrict");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWDistrict");
                    hiddenClientControls.Add("txtWDistrict");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WORKER_WBLOCK");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblWBlock");
                    disabledClientControls.Add("txtWBlock");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWBlock");
                    hiddenClientControls.Add("txtWBlock");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WORKER_WENTRANCE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblWEntrance");
                    disabledClientControls.Add("txtWEntrance");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWEntrance");
                    hiddenClientControls.Add("txtWEntrance");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WORKER_WFLOOR");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblWFloor");
                    disabledClientControls.Add("txtWFloor");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWFloor");
                    hiddenClientControls.Add("txtWFloor");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WORKER_WAPT");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblWApt");
                    disabledClientControls.Add("txtWApt");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWApt");
                    hiddenClientControls.Add("txtWApt");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WORKER_WPHONE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblWPhone");
                    disabledClientControls.Add("txtWPhone");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWPhone");
                    hiddenClientControls.Add("txtWPhone");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WORKER_WFAX");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblWFax");
                    disabledClientControls.Add("txtWFax");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWFax");
                    hiddenClientControls.Add("txtWFax");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WORKER_WEMAIL");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblWEmail");
                    disabledClientControls.Add("txtWEmail");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWEmail");
                    hiddenClientControls.Add("txtWEmail");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WORKER_WBIRTHDATE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblWBirthDate");
                    disabledClientControls.Add("txtWBirthDate");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWBirthDate");
                    hiddenClientControls.Add("txtWBirthDate");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WORKER_WGENDER");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblWGender");
                    disabledClientControls.Add("lblWGender1");
                    disabledClientControls.Add("lblWGender2");
                    disabledClientControls.Add("rbWGender1");
                    disabledClientControls.Add("rbWGender2");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWGender");
                    hiddenClientControls.Add("lblWGender1");
                    hiddenClientControls.Add("lblWGender2");
                    hiddenClientControls.Add("rbWGender1");
                    hiddenClientControls.Add("rbWGender2");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WORKER_WCITIZENSHIP");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblWCitizenship");
                    disabledClientControls.Add("txtWCitizenship");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWCitizenship");
                    hiddenClientControls.Add("txtWCitizenship");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WORKER_WHIRETYPE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("rbWHireType1");
                    disabledClientControls.Add("rbWHireType2");
                    disabledClientControls.Add("lblWHireType");
                    disabledClientControls.Add("lblWHireType1");
                    disabledClientControls.Add("lblWHireType2");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWHireType");
                    hiddenClientControls.Add("txtWHireType");
                    hiddenClientControls.Add("lblWHireType");
                    hiddenClientControls.Add("lblWHireType1");
                    hiddenClientControls.Add("lblWHireType2");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WORKER_WWORKTIME");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("rbWWorkTime1");
                    disabledClientControls.Add("lblWWorkTime1");
                    disabledClientControls.Add("rbWWorkTime2");
                    disabledClientControls.Add("lblWWorkTime2");
                    disabledClientControls.Add("lblWWorkTime");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("rbWWorkTime1");
                    hiddenClientControls.Add("lblWWorkTime1");
                    hiddenClientControls.Add("rbWWorkTime2");
                    hiddenClientControls.Add("lblWWorkTime2");
                    hiddenClientControls.Add("lblWWorkTime");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WORKER_WHIREDATE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblWHireDate");
                    disabledClientControls.Add("txtWHireDate");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWHireDate");
                    hiddenClientControls.Add("txtWHireDate");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WORKER_WJOBTITLE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblWJobTitle");
                    disabledClientControls.Add("txtWJobTitle");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWJobTitle");
                    hiddenClientControls.Add("txtWJobTitle");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WORKER_WJOBCODE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblWJobCode");
                    disabledClientControls.Add("txtWJobCode");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWJobCode");
                    hiddenClientControls.Add("txtWJobCode");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WORKER_WJOBCATEGORY");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("rbWJobCategory1");
                    disabledClientControls.Add("rbWJobCategory2");
                    disabledClientControls.Add("rbWJobCategory3");
                    disabledClientControls.Add("lblWJobCategory");
                    disabledClientControls.Add("lblWJobCategory1");
                    disabledClientControls.Add("lblWJobCategory2");
                    disabledClientControls.Add("lblWJobCategory3");

                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("rbWJobCategory1");
                    hiddenClientControls.Add("rbWJobCategory2");
                    hiddenClientControls.Add("rbWJobCategory3");
                    hiddenClientControls.Add("lblWJobCategory");
                    hiddenClientControls.Add("lblWJobCategory1");
                    hiddenClientControls.Add("lblWJobCategory2");
                    hiddenClientControls.Add("lblWJobCategory3");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WORKER_WYEARSONSERVICE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblWYearsOnService");
                    disabledClientControls.Add("txtWYearsOnService");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWYearsOnService");
                    hiddenClientControls.Add("txtWYearsOnService");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WORKER_WCURRJOBYEARSONSERVICE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblWCurrentJobYearsOnService");
                    disabledClientControls.Add("txtWCurrentJobYearsOnService");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWCurrentJobYearsOnService");
                    hiddenClientControls.Add("txtWCurrentJobYearsOnService");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WORKER_WBRANCH");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblWBranch");
                    disabledClientControls.Add("txtWBranch");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWBranch");
                    hiddenClientControls.Add("txtWBranch");
                }

            }
        }
        private void SetupPageUI_Acc()
        {
            disabledClientControls = new List<string>();
            hiddenClientControls = new List<string>();

            //Chek on Tab Level

            l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_ACC");

            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                //Disable all controls in tab

                disabledClientControls.Add("lblAccAddressHeader");

                disabledClientControls.Add("lblAccWorkTimeHeader1");
                disabledClientControls.Add("lblAccWorkTimeHeader2");
                disabledClientControls.Add("lblAccWorkTimeHeader3");
                disabledClientControls.Add("lblAccWorkTimeHeader4");
                disabledClientControls.Add("lblAccWorkTimeHeader5");


                disabledClientControls.Add("lblAccDateTimeHeader");
                disabledClientControls.Add("lblAccDateTimeHours");
                disabledClientControls.Add("txtAccDateTimeHours");

                disabledClientControls.Add("lblAccDateTimeMinutes");
                disabledClientControls.Add("txtAccDateTimeMinutes");

                disabledClientControls.Add("lblAccDateTimeDay");
                disabledClientControls.Add("txtAccDateTimeDay");


                disabledClientControls.Add("lblAccWorkFromHour1");
                disabledClientControls.Add("txtAccWorkFromHour1");

                disabledClientControls.Add("lblAccWorkFromMin1");
                disabledClientControls.Add("txtAccWorkFromMin1");

                disabledClientControls.Add("lblAccWorkToHour1");
                disabledClientControls.Add("txtAccWorkToHour1");

                hiddenClientControls.Add("lblAccWorkToHour1");
                hiddenClientControls.Add("txtAccWorkToHour1");

                disabledClientControls.Add("lblAccWorkToMin1");
                disabledClientControls.Add("txtAccWorkToMin1");

                disabledClientControls.Add("lblAccWorkFromHour2");
                disabledClientControls.Add("txtAccWorkFromHour2");

                disabledClientControls.Add("lblAccWorkFromMin2");
                disabledClientControls.Add("txtAccWorkFromMin2");

                disabledClientControls.Add("lblAccWorkToHour2");
                disabledClientControls.Add("txtAccWorkToHour2");

                disabledClientControls.Add("lblAccWorkToMin2");
                disabledClientControls.Add("txtAccWorkToMin2");

                disabledClientControls.Add("lblAccPlace");
                disabledClientControls.Add("txtAccPlace");

                disabledClientControls.Add("lblAccCountry");
                disabledClientControls.Add("txtAccCountry");

                disabledClientControls.Add("ddlAccRegion");
                disabledClientControls.Add("ddlAccMunicipality");
                disabledClientControls.Add("ddlAccCity");
                disabledClientControls.Add("txtAccPostCode");

                disabledClientControls.Add("lblAccRegion");
                disabledClientControls.Add("lblAccMunicipality");
                disabledClientControls.Add("lblAccCity");
                disabledClientControls.Add("lblAccPostCode");

                disabledClientControls.Add("lblAccStreet");
                disabledClientControls.Add("txtAccStreet");

                disabledClientControls.Add("lblAccStreetNum");
                disabledClientControls.Add("txtAccStreetNum");

                disabledClientControls.Add("lblAccDistrict");
                disabledClientControls.Add("txtAccDistrict");

                disabledClientControls.Add("lblAccBlock");
                disabledClientControls.Add("txtAccBlock");

                disabledClientControls.Add("lblAccEntrance");
                disabledClientControls.Add("txtAccEntrance");

                disabledClientControls.Add("lblAccFloor");
                disabledClientControls.Add("txtAccFloor");

                disabledClientControls.Add("lblAccApt");
                disabledClientControls.Add("txtAccApt");

                disabledClientControls.Add("lblAccPhone");
                disabledClientControls.Add("txtAccPhone");

                disabledClientControls.Add("lblAccFax");
                disabledClientControls.Add("txtAccFax");

                disabledClientControls.Add("lblAccEmail");
                disabledClientControls.Add("txtAccEmail");

                disabledClientControls.Add("lblAccHappenedAt1");
                disabledClientControls.Add("rbAccHappenedAt1");

                disabledClientControls.Add("lblAccHappenedAt2");
                disabledClientControls.Add("rbAccHappenedAt2");

                disabledClientControls.Add("lblAccHappenedAt3");
                disabledClientControls.Add("rbAccHappenedAt3");

                disabledClientControls.Add("lblAccHappenedAt");
                disabledClientControls.Add("txtAccHappenedAtOther");

                disabledClientControls.Add("lblAccJobType");
                disabledClientControls.Add("txtAccJobType");

                disabledClientControls.Add("lblAccTaskType");
                disabledClientControls.Add("txtAccTaskType");

                disabledClientControls.Add("lblAccDeviationFromTask");
                disabledClientControls.Add("txtAccDeviationFromTask");

                disabledClientControls.Add("lblAccInjurDesc");
                disabledClientControls.Add("txtAccInjurDesc");

                disabledClientControls.Add("lblAccInjHasRights1");
                disabledClientControls.Add("rbAccInjHasRights1");

                disabledClientControls.Add("lblAccInjHasRights2");
                disabledClientControls.Add("rbAccInjHasRights2");

                disabledClientControls.Add("lblAccInjHasRights3");
                disabledClientControls.Add("rbAccInjHasRights3");

                disabledClientControls.Add("lblAccInjHasRights");

                disabledClientControls.Add("lblAccLegalRef1");
                disabledClientControls.Add("rbAccLegalRef1");

                disabledClientControls.Add("lblAccLegalRef2");
                disabledClientControls.Add("rbAccLegalRef2");

                disabledClientControls.Add("lblAccLegalRef");


                disabledClientControls.Add("lblAccPlannedActions");
                disabledClientControls.Add("txtAccPlannedActions");

                disabledClientControls.Add("lblAccLostDays");
                disabledClientControls.Add("txtAccLostDays");
            }
            else
            {
                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_ACC_ACCDATETIME");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblAccDateTimeHeader");

                    disabledClientControls.Add("lblAccDateTimeHours");
                    disabledClientControls.Add("txtAccDateTimeHours");

                    disabledClientControls.Add("lblAccDateTimeMinutes");
                    disabledClientControls.Add("txtAccDateTimeMinutes");

                    disabledClientControls.Add("lblAccDateTimeDay");
                    disabledClientControls.Add("txtAccDateTimeDay");

                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAccDateTimeHeader");

                    hiddenClientControls.Add("lblAccDateTimeHours");
                    hiddenClientControls.Add("txtAccDateTimeHours");

                    hiddenClientControls.Add("lblAccDateTimeMinutes");
                    hiddenClientControls.Add("txtAccDateTimeMinutes");

                    hiddenClientControls.Add("lblAccDateTimeDay");
                    hiddenClientControls.Add("txtAccDateTimeDay");

                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_ACC_ACCWORKFROMHOUR1");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblAccWorkFromHour1");
                    disabledClientControls.Add("txtAccWorkFromHour1");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAccWorkFromHour1");
                    hiddenClientControls.Add("txtAccWorkFromHour1");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_ACC_ACCWORKFROMMIN1");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblAccWorkFromMin1");
                    disabledClientControls.Add("txtAccWorkFromMin1");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAccWorkFromMin1");
                    hiddenClientControls.Add("txtAccWorkFromMin1");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_ACC_ACCWORKTOHOUR1");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblAccWorkToHour1");
                    disabledClientControls.Add("txtAccWorkToHour1");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAccWorkToHour1");
                    hiddenClientControls.Add("txtAccWorkToHour1");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_ACC_ACCWORKTOMIN1");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblAccWorkToMin1");
                    disabledClientControls.Add("txtAccWorkToMin1");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAccWorkToMin1");
                    hiddenClientControls.Add("txtAccWorkToMin1");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_ACC_ACCWORKFROMHOUR2");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblAccWorkFromHour2");
                    disabledClientControls.Add("txtAccWorkFromHour2");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAccWorkFromHour2");
                    hiddenClientControls.Add("txtAccWorkFromHour2");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_ACC_ACCWORKFROMMIN2");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblAccWorkFromMin2");
                    disabledClientControls.Add("txtAccWorkFromMin2");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAccWorkFromMin2");
                    hiddenClientControls.Add("txtAccWorkFromMin2");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_ACC_ACCWORKTOHOUR2");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblAccWorkToHour2");
                    disabledClientControls.Add("txtAccWorkToHour2");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAccWorkToHour2");
                    hiddenClientControls.Add("txtAccWorkToHour2");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_ACC_ACCWORKTOMIN2");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblAccWorkToMin2");
                    disabledClientControls.Add("txtAccWorkToMin2");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAccWorkToMin2");
                    hiddenClientControls.Add("txtAccWorkToMin2");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_ACC_ACCPLACE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblAccPlace");
                    disabledClientControls.Add("txtAccPlace");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAccPlace");
                    hiddenClientControls.Add("txtAccPlace");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_ACC_ACCCOUNTRY");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblAccCountry");
                    disabledClientControls.Add("txtAccCountry");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAccCountry");
                    hiddenClientControls.Add("txtAccCountry");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_ACC_ACCCITYID");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("ddlAccRegion");
                    disabledClientControls.Add("ddlAccMunicipality");
                    disabledClientControls.Add("ddlAccCity");
                    disabledClientControls.Add("txtAccPostCode");

                    disabledClientControls.Add("lblAccRegion");
                    disabledClientControls.Add("lblAccMunicipality");
                    disabledClientControls.Add("lblAccCity");
                    disabledClientControls.Add("lblAccPostCode");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("ddlAccRegion");
                    hiddenClientControls.Add("ddlAccMunicipality");
                    hiddenClientControls.Add("ddlAccCity");
                    hiddenClientControls.Add("txtAccPostCode");

                    hiddenClientControls.Add("lblAccRegion");
                    hiddenClientControls.Add("lblAccMunicipality");
                    hiddenClientControls.Add("lblAccCity");
                    hiddenClientControls.Add("lblAccPostCode");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_ACC_ACCSTREET");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblAccStreet");
                    disabledClientControls.Add("txtAccStreet");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAccStreet");
                    hiddenClientControls.Add("txtAccStreet");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_ACC_ACCSTREETNUM");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblAccStreetNum");
                    disabledClientControls.Add("txtAccStreetNum");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAccStreetNum");
                    hiddenClientControls.Add("txtAccStreetNum");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_ACC_ACCDISTRICT");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblAccDistrict");
                    disabledClientControls.Add("txtAccDistrict");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAccDistrict");
                    hiddenClientControls.Add("txtAccDistrict");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_ACC_ACCBLOCK");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblAccBlock");
                    disabledClientControls.Add("txtAccBlock");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAccBlock");
                    hiddenClientControls.Add("txtAccBlock");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_ACC_ACCENTRANCE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblAccEntrance");
                    disabledClientControls.Add("txtAccEntrance");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAccEntrance");
                    hiddenClientControls.Add("txtAccEntrance");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_ACC_ACCFLOOR");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblAccFloor");
                    disabledClientControls.Add("txtAccFloor");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAccFloor");
                    hiddenClientControls.Add("txtAccFloor");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_ACC_ACCAPT");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblAccApt");
                    disabledClientControls.Add("txtAccApt");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAccApt");
                    hiddenClientControls.Add("txtAccApt");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_ACC_ACCPHONE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblAccPhone");
                    disabledClientControls.Add("txtAccPhone");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAccPhone");
                    hiddenClientControls.Add("txtAccPhone");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_ACC_ACCFAX");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblAccFax");
                    disabledClientControls.Add("txtAccFax");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAccFax");
                    hiddenClientControls.Add("txtAccFax");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_ACC_ACCEMAIL");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblAccEmail");
                    disabledClientControls.Add("txtAccEmail");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAccEmail");
                    hiddenClientControls.Add("txtAccEmail");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_ACC_ACCHAPPENEDAT");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblAccHappenedAt1");
                    disabledClientControls.Add("rbAccHappenedAt1");

                    disabledClientControls.Add("lblAccHappenedAt2");
                    disabledClientControls.Add("rbAccHappenedAt2");

                    disabledClientControls.Add("lblAccHappenedAt3");
                    disabledClientControls.Add("rbAccHappenedAt3");

                    disabledClientControls.Add("lblAccHappenedAt");
                    disabledClientControls.Add("txtAccHappenedAtOther");

                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAccHappenedAt1");
                    hiddenClientControls.Add("rbAccHappenedAt1");

                    hiddenClientControls.Add("lblAccHappenedAt2");
                    hiddenClientControls.Add("rbAccHappenedAt2");

                    hiddenClientControls.Add("lblAccHappenedAt3");
                    hiddenClientControls.Add("rbAccHappenedAt3");

                    hiddenClientControls.Add("lblAccHappenedAt");
                    hiddenClientControls.Add("txtAccHappenedAtOther");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_ACC_ACCJOBTYPE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblAccJobType");
                    disabledClientControls.Add("txtAccJobType");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAccJobType");
                    hiddenClientControls.Add("txtAccJobType");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_ACC_ACCTASKTYPE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblAccTaskType");
                    disabledClientControls.Add("txtAccTaskType");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAccTaskType");
                    hiddenClientControls.Add("txtAccTaskType");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_ACC_ACCDEVIATIONFROMTASK");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblAccDeviationFromTask");
                    disabledClientControls.Add("txtAccDeviationFromTask");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAccDeviationFromTask");
                    hiddenClientControls.Add("txtAccDeviationFromTask");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_ACC_ACCINJURDESC");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblAccInjurDesc");
                    disabledClientControls.Add("txtAccInjurDesc");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAccInjurDesc");
                    hiddenClientControls.Add("txtAccInjurDesc");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_ACC_ACCINJHASRIGHTS");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblAccInjHasRights1");
                    disabledClientControls.Add("rbAccInjHasRights1");

                    disabledClientControls.Add("lblAccInjHasRights2");
                    disabledClientControls.Add("rbAccInjHasRights2");

                    disabledClientControls.Add("lblAccInjHasRights3");
                    disabledClientControls.Add("rbAccInjHasRights3");

                    disabledClientControls.Add("lblAccInjHasRights");

                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAccInjHasRights1");
                    hiddenClientControls.Add("rbAccInjHasRights1");

                    hiddenClientControls.Add("lblAccInjHasRights2");
                    hiddenClientControls.Add("rbAccInjHasRights2");

                    hiddenClientControls.Add("lblAccInjHasRights3");
                    hiddenClientControls.Add("rbAccInjHasRights3");

                    hiddenClientControls.Add("lblAccInjHasRights");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_ACC_ACCLEGALREF");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblAccLegalRef1");
                    disabledClientControls.Add("rbAccLegalRef1");

                    disabledClientControls.Add("lblAccLegalRef2");
                    disabledClientControls.Add("rbAccLegalRef2");

                    disabledClientControls.Add("lblAccLegalRef");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAccLegalRef1");
                    hiddenClientControls.Add("rbAccLegalRef1");

                    hiddenClientControls.Add("lblAccLegalRef2");
                    hiddenClientControls.Add("rbAccLegalRef2");

                    hiddenClientControls.Add("lblAccLegalRef");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_ACC_ACCPLANNEDACTIONS");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblAccPlannedActions");
                    disabledClientControls.Add("txtAccPlannedActions");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAccPlannedActions");
                    hiddenClientControls.Add("txtAccPlannedActions");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_ACC_ACCLOSTDAYS");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblAccLostDays");
                    disabledClientControls.Add("txtAccLostDays");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblAccLostDays");
                    hiddenClientControls.Add("txtAccLostDays");
                }

            }
        }
        private void SetupPageUI_Harm()
        {
            disabledClientControls = new List<string>();
            hiddenClientControls = new List<string>();

            //Chek on Tab Level
            l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_HARM");

            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                //Disable all controls in tab
                disabledClientControls.Add("lblHarmType");
                disabledClientControls.Add("txtHarmType");
                disabledClientControls.Add("lblHarmTypeAdditional");

                disabledClientControls.Add("lblHarmBodyParts");
                disabledClientControls.Add("txtHarmBodyParts");
                disabledClientControls.Add("lblHarmBodyPartsAdditional");

                disabledClientControls.Add("lblHarmResult1");
                disabledClientControls.Add("rbHarmResult1");

                disabledClientControls.Add("lblHarmResult2");
                disabledClientControls.Add("rbHarmResult2");

                disabledClientControls.Add("lblHarmResult3");
                disabledClientControls.Add("rbHarmResult3");

                disabledClientControls.Add("lblHarmResult");

            }
            else
            {
                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_HARM_HARMTYPE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblHarmType");
                    disabledClientControls.Add("txtHarmType");
                    disabledClientControls.Add("lblHarmTypeAdditional");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblHarmType");
                    hiddenClientControls.Add("txtHarmType");
                    hiddenClientControls.Add("lblHarmTypeAdditional");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_HARM_HARMBODYPARTS");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblHarmBodyParts");
                    disabledClientControls.Add("txtHarmBodyParts");
                    disabledClientControls.Add("lblHarmBodyPartsAdditional");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblHarmBodyParts");
                    hiddenClientControls.Add("txtHarmBodyParts");
                    hiddenClientControls.Add("lblHarmBodyPartsAdditional");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_HARM_HARMRESULT");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblHarmResult1");
                    disabledClientControls.Add("rbHarmResult1");

                    disabledClientControls.Add("lblHarmResult2");
                    disabledClientControls.Add("rbHarmResult2");

                    disabledClientControls.Add("lblHarmResult3");
                    disabledClientControls.Add("rbHarmResult3");

                    disabledClientControls.Add("lblHarmResult");
                }
                if (l == UIAccessLevel.Hidden)
                {

                    hiddenClientControls.Add("lblHarmResult1");
                    hiddenClientControls.Add("rbHarmResult1");

                    hiddenClientControls.Add("lblHarmResult2");
                    hiddenClientControls.Add("rbHarmResult2");

                    hiddenClientControls.Add("lblHarmResult3");
                    hiddenClientControls.Add("rbHarmResult3");

                    hiddenClientControls.Add("lblHarmResult");

                }
            }
        }
        private void SetupPageUI_With()
        {
            disabledClientControls = new List<string>();
            hiddenClientControls = new List<string>();

            //Chek on Tab Level
            l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WITH");

            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                //Disable all controls in tab
                disabledClientControls.Add("lblWitnessFullName");
                disabledClientControls.Add("txtWitnessFullName");

                disabledClientControls.Add("ddlWithRegion");
                disabledClientControls.Add("ddlWithMunicipality");
                disabledClientControls.Add("ddlWithCity");
                disabledClientControls.Add("txtWitPostCode");

                disabledClientControls.Add("lblWithRegion");
                disabledClientControls.Add("lblWithMunicipality");
                disabledClientControls.Add("lblWithCity");
                disabledClientControls.Add("lblWitPostCode");

                disabledClientControls.Add("lblWitStreet");
                disabledClientControls.Add("txtWitStreet");

                disabledClientControls.Add("lblWitStreetNum");
                disabledClientControls.Add("txtWitStreetNum");

                disabledClientControls.Add("lblWitDistrict");
                disabledClientControls.Add("txtWitDistrict");

                disabledClientControls.Add("lblWitBlock");
                disabledClientControls.Add("txtWitBlock");

                disabledClientControls.Add("lblWitEntrance");
                disabledClientControls.Add("txtWitEntrance");

                disabledClientControls.Add("lblWitFloor");
                disabledClientControls.Add("txtWitFloor");

                disabledClientControls.Add("lblWitApt");
                disabledClientControls.Add("txtWitApt");

                disabledClientControls.Add("lblWitPhone");
                disabledClientControls.Add("txtWitPhone");

                disabledClientControls.Add("lblWitFax");
                disabledClientControls.Add("txtWitFax");

                disabledClientControls.Add("lblWitEmail");
                disabledClientControls.Add("txtWitEmail");

            }
            else
            {
                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WITH_WITNESSFULLNAME");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblWitnessFullName");
                    disabledClientControls.Add("txtWitnessFullName");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWitnessFullName");
                    hiddenClientControls.Add("txtWitnessFullName");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WITH_WITCITYID");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {

                    disabledClientControls.Add("ddlWithRegion");
                    disabledClientControls.Add("ddlWithMunicipality");
                    disabledClientControls.Add("ddlWithCity");
                    disabledClientControls.Add("txtWitPostCode");

                    disabledClientControls.Add("lblWithRegion");
                    disabledClientControls.Add("lblWithMunicipality");
                    disabledClientControls.Add("lblWithCity");
                    disabledClientControls.Add("lblWitPostCode");

                }
                if (l == UIAccessLevel.Hidden)
                {

                    hiddenClientControls.Add("ddlWithRegion");
                    hiddenClientControls.Add("ddlWithMunicipality");
                    hiddenClientControls.Add("ddlWithCity");
                    hiddenClientControls.Add("txtWitPostCode");

                    hiddenClientControls.Add("lblWithRegion");
                    hiddenClientControls.Add("lblWithMunicipality");
                    hiddenClientControls.Add("lblWithCity");
                    hiddenClientControls.Add("lblWitPostCode");

                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WITH_WITSTREET");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblWitStreet");
                    disabledClientControls.Add("txtWitStreet");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWitStreet");
                    hiddenClientControls.Add("txtWitStreet");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WITH_WITSTREETNUM");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblWitStreetNum");
                    disabledClientControls.Add("txtWitStreetNum");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWitStreetNum");
                    hiddenClientControls.Add("txtWitStreetNum");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WITH_WITDISTRICT");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblWitDistrict");
                    disabledClientControls.Add("txtWitDistrict");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWitDistrict");
                    hiddenClientControls.Add("txtWitDistrict");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WITH_WITBLOCK");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblWitBlock");
                    disabledClientControls.Add("txtWitBlock");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWitBlock");
                    hiddenClientControls.Add("txtWitBlock");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WITH_WITENTRANCE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblWitEntrance");
                    disabledClientControls.Add("txtWitEntrance");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWitEntrance");
                    hiddenClientControls.Add("txtWitEntrance");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WITH_WITFLOOR");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblWitFloor");
                    disabledClientControls.Add("txtWitFloor");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWitFloor");
                    hiddenClientControls.Add("txtWitFloor");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WITH_WITAPT");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblWitApt");
                    disabledClientControls.Add("txtWitApt");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWitApt");
                    hiddenClientControls.Add("txtWitApt");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WITH_WITPHONE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblWitPhone");
                    disabledClientControls.Add("txtWitPhone");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWitPhone");
                    hiddenClientControls.Add("txtWitPhone");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WITH_WITFAX");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblWitFax");
                    disabledClientControls.Add("txtWitFax");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWitFax");
                    hiddenClientControls.Add("txtWitFax");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_WITH_WITEMAIL");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblWitEmail");
                    disabledClientControls.Add("txtWitEmail");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblWitEmail");
                    hiddenClientControls.Add("txtWitEmail");
                }
            }
        }
        private void SetupPageUI_Heir()
        {
            disabledClientControls = new List<string>();
            hiddenClientControls = new List<string>();

            //Chek on Tab Level
            l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_HEIR");

            if (l == UIAccessLevel.Disabled || screenDisabled)
            {
                //Disable all controls in tab
                disabledClientControls.Add("lblHeirAddressHeader");

                disabledClientControls.Add("lblHeirFullName");
                disabledClientControls.Add("txtHeirFullName");

                disabledClientControls.Add("lblHeirEgn");
                disabledClientControls.Add("txtHeirEgn");

                disabledClientControls.Add("ddlHeirRegion");
                disabledClientControls.Add("ddlHeirMunicipality");
                disabledClientControls.Add("ddlHeirCity");
                disabledClientControls.Add("txtHeirPostCode");

                disabledClientControls.Add("lblHeirRegion");
                disabledClientControls.Add("lblHeirMunicipality");
                disabledClientControls.Add("lblHeirCity");
                disabledClientControls.Add("lblHeirPostCode");

                disabledClientControls.Add("lblHeirStreet");
                disabledClientControls.Add("txtHeirStreet");

                disabledClientControls.Add("lblHeirStreetNum");
                disabledClientControls.Add("txtHeirStreetNum");

                disabledClientControls.Add("lblHeirDistrict");
                disabledClientControls.Add("txtHeirDistrict");

                disabledClientControls.Add("lblHeirBlock");
                disabledClientControls.Add("txtHeirBlock");

                disabledClientControls.Add("lblHeirEntrance");
                disabledClientControls.Add("txtHeirEntrance");

                disabledClientControls.Add("lblHeirFloor");
                disabledClientControls.Add("txtHeirFloor");

                disabledClientControls.Add("lblHeirApt");
                disabledClientControls.Add("txtHeirApt");

                disabledClientControls.Add("lblHeirPhone");
                disabledClientControls.Add("txtHeirPhone");

                disabledClientControls.Add("lblHeirFax");
                disabledClientControls.Add("txtHeirFax");

                disabledClientControls.Add("lblHeirEmail");
                disabledClientControls.Add("txtHeirEmail");
            }
            else
            {
                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_HEIR_HEIRFULLNAME");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblHeirFullName");
                    disabledClientControls.Add("txtHeirFullName");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblHeirFullName");
                    hiddenClientControls.Add("txtHeirFullName");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_HEIR_HEIREGN");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblHeirEgn");
                    disabledClientControls.Add("txtHeirEgn");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblHeirEgn");
                    hiddenClientControls.Add("txtHeirEgn");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_HEIR_HEIRCITYID");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("ddlHeirRegion");
                    disabledClientControls.Add("ddlHeirMunicipality");
                    disabledClientControls.Add("ddlHeirCity");
                    disabledClientControls.Add("txtHeirPostCode");

                    disabledClientControls.Add("lblHeirRegion");
                    disabledClientControls.Add("lblHeirMunicipality");
                    disabledClientControls.Add("lblHeirCity");
                    disabledClientControls.Add("lblHeirPostCode");

                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("ddlHeirRegion");
                    hiddenClientControls.Add("ddlHeirMunicipality");
                    hiddenClientControls.Add("ddlHeirCity");
                    hiddenClientControls.Add("txtHeirPostCode");

                    hiddenClientControls.Add("lblHeirRegion");
                    hiddenClientControls.Add("lblHeirMunicipality");
                    hiddenClientControls.Add("lblHeirCity");
                    hiddenClientControls.Add("lblHeirPostCode");

                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_HEIR_HEIRSTREET");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblHeirStreet");
                    disabledClientControls.Add("txtHeirStreet");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblHeirStreet");
                    hiddenClientControls.Add("txtHeirStreet");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_HEIR_HEIRSTREETNUM");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblHeirStreetNum");
                    disabledClientControls.Add("txtHeirStreetNum");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblHeirStreetNum");
                    hiddenClientControls.Add("txtHeirStreetNum");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_HEIR_HEIRDISTRICT");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblHeirDistrict");
                    disabledClientControls.Add("txtHeirDistrict");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblHeirDistrict");
                    hiddenClientControls.Add("txtHeirDistrict");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_HEIR_HEIRBLOCK");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblHeirBlock");
                    disabledClientControls.Add("txtHeirBlock");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblHeirBlock");
                    hiddenClientControls.Add("txtHeirBlock");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_HEIR_HEIRENTRANCE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblHeirEntrance");
                    disabledClientControls.Add("txtHeirEntrance");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblHeirEntrance");
                    hiddenClientControls.Add("txtHeirEntrance");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_HEIR_HEIRFLOOR");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblHeirFloor");
                    disabledClientControls.Add("txtHeirFloor");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblHeirFloor");
                    hiddenClientControls.Add("txtHeirFloor");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_HEIR_HEIRAPT");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblHeirApt");
                    disabledClientControls.Add("txtHeirApt");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblHeirApt");
                    hiddenClientControls.Add("txtHeirApt");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_HEIR_HEIRPHONE");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblHeirPhone");
                    disabledClientControls.Add("txtHeirPhone");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblHeirPhone");
                    hiddenClientControls.Add("txtHeirPhone");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_HEIR_HEIRFAX");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblHeirFax");
                    disabledClientControls.Add("txtHeirFax");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblHeirFax");
                    hiddenClientControls.Add("txtHeirFax");
                }

                l = this.GetUIItemAccessLevel(prefix + "DECLARATIONACC_HEIR_HEIREMAIL");
                if (l == UIAccessLevel.Disabled || screenDisabled)
                {
                    disabledClientControls.Add("lblHeirEmail");
                    disabledClientControls.Add("txtHeirEmail");
                }
                if (l == UIAccessLevel.Hidden)
                {
                    hiddenClientControls.Add("lblHeirEmail");
                    hiddenClientControls.Add("txtHeirEmail");
                }
            }
        }

        //Create list with disabledClientControls
        private string SetListDisabledControls()
        {
            string result = "";

            foreach (string s in disabledClientControls)
            {
                result += "," + s;
            }
            return result;

        }
        //Create list with hiddenClientControls
        private string SetListHiddenControls()
        {
            string result = "";

            foreach (string s in hiddenClientControls)
            {
                result += "," + s;
            }
            return result;
        }

        //Fill Server UI Controls with Data 
        private void FillDeclarationOfAccidentHeader()
        {
            if (this.declarationId == 0) return;

            this.txtDeclarationNumber.Text = declarationOfAccident.DeclarationOfAccidentHeader.DeclarationNumber;
            this.txtReferenceNumber.Text = declarationOfAccident.DeclarationOfAccidentHeader.ReferenceNumber;
            this.txtFileNumber.Text = declarationOfAccident.DeclarationOfAccidentHeader.FileNumber;

            if (declarationOfAccident.DeclarationOfAccidentHeader.DeclarationDate.HasValue)
            {
                this.txtDeclarationDate.Text = CommonFunctions.FormatDate(declarationOfAccident.DeclarationOfAccidentHeader.DeclarationDate);
            }
            if (declarationOfAccident.DeclarationOfAccidentHeader.ReferenceDate.HasValue)
            {
                this.txtReferenceDate.Text = CommonFunctions.FormatDate(declarationOfAccident.DeclarationOfAccidentHeader.ReferenceDate);
            }

            this.rblAplicantType.SelectedValue = declarationOfAccident.DeclarationOfAccidentFooter.ApplicantType.ToString();
            if (this.rblAplicantType.SelectedValue == "1")
            {
                this.txtAplicantEmplPosition.Text = declarationOfAccident.DeclarationOfAccidentFooter.AplicantPosition.ToString();
                this.txtAplicantEmplName.Text = declarationOfAccident.DeclarationOfAccidentFooter.AplicantName.ToString();
            }
            else
            {
                this.txtAplicantEmplPosition.Attributes.Add("style", "display:none");
                this.txtAplicantEmplName.Attributes.Add("style", "display:none");
                this.lblAplicantEmplPositionInfo.Attributes.Add("style", "display:none");
                this.lblAplicantEmplNameInfo.Attributes.Add("style", "display:none");
            }
        }
        //Set Dinamycally Page name
        private void SetPageName()
        {
            if (this.declarationId > 0)
            {
                lblHeaderCell.Text = "Редактиране на декларация за злополука";
                Page.Title = lblHeaderCell.Text;
            }
            else
            {
                lblHeaderCell.Text = "Добавяне на нова декларация за злополука";
                Page.Title = lblHeaderCell.Text;
            }
        }
        //Set DatePickers to Server Contorls
        private void SetupDatePickers()
        {
            this.txtReferenceDate.CssClass = CommonFunctions.DatePickerCSS();
            this.txtDeclarationDate.CssClass = "RequiredInputField " + CommonFunctions.DatePickerCSS();
        }
        #endregion

        #region ObjectEvents
        //Redirect to ManageInvestigationProtocol.aspx page
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (FromHome != 1)
                Response.Redirect("~/ContentPages/ManageDeclarationOfAccident.aspx", true);
            else
                Response.Redirect("~/ContentPages/Home.aspx");
        }
        #endregion

    }
}
