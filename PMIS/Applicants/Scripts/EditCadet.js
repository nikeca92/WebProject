window.addEventListener ? window.addEventListener('load', PageLoad, false) : window.attachEvent('onload', PageLoad);

//Call this function when the page is loaded
function PageLoad()
{
    LoadPersonDetails();
    
    if (document.getElementById("btnTabSubjects") != null)
        JSLoadTab("btnTabSubjects");
    else if (document.getElementById("btnTabEducation") != null)
        JSLoadTab("btnTabEducation");
}

//This function load the person's details
function LoadPersonDetails()
{
    var url = "EditCadet.aspx?AjaxMethod=JSLoadPersonDetails";
    var params = "";
    params += "PersonId=" + document.getElementById(hdnPersonIdClientID).value;
    var myAJAX = new AJAX(url, true, params, LoadPersonDetails_Callback);
    myAJAX.Call();
}

function LoadPersonDetails_Callback(xml)
{
    var person = xml.getElementsByTagName("person")[0];

    var personId = xmlValue(person, "personId");
    var identNumber = xmlValue(person, "identNumber");
    var firstName = xmlValue(person, "firstName");
    var lastName = xmlValue(person, "lastName");
    var genderName = xmlValue(person, "genderName");
    var lastModified = xmlValue(person, "lastModified");
    var age = xmlValue(person, "age");
    var ageMonthsPart = xmlValue(person, "ageMonthsPart");
    
    var permPostCode = xmlValue(person, "permPostCode");
    var permSecondPostCode = xmlValue(person, "permSecondPostCode");
    var permCity = xmlValue(person, "permCity");
    var permDistrict = xmlValue(person, "permDistrict");
    var permRegion = xmlValue(person, "permRegion");
    var permAddress = xmlValue(person, "permAddress");
    var permMunicipality = xmlValue(person, "permMunicipality");

    var presPostCode = xmlValue(person, "presPostCode");
    var presSecondPostCode = xmlValue(person, "presSecondPostCode");
    var presCity = xmlValue(person, "presCity");
    var presDistrict = xmlValue(person, "presDistrict");
    var presRegion = xmlValue(person, "presRegion");
    var presAddress = xmlValue(person, "presAddress");
    var presMunicipility = xmlValue(person, "presMunicipility");

    var contactPostCode = xmlValue(person, "contactPostCode");
    var contactSecondPostCode = xmlValue(person, "contactSecondPostCode");
    var contactCity = xmlValue(person, "contactCity");
    var contactDistrict = xmlValue(person, "contactDistrict");
    var contactRegion = xmlValue(person, "contactRegion");
    var contactAddress = xmlValue(person, "contactAddress");
    var contactMunicipality = xmlValue(person, "contactMunicipality");

    var IDCardNumber = xmlValue(person, "IDCardNumber");
    var IDCardIssuedBy = xmlValue(person, "IDCardIssuedBy");
    var IDCardIssueDate = xmlValue(person, "IDCardIssueDate");
    var homePhone = xmlValue(person, "homePhone");
    var mobilePhone = xmlValue(person, "mobilePhone");
    var email = xmlValue(person, "email");
    var drvLicCategories = xmlValue(person, "drvLicCategories");
    var militaryUnitName = xmlValue(person, "militaryUnitName");

    var medCertHTML = xmlValue(person, "medCertHTML");
    var psychCertHTML = xmlValue(person, "psychCertHTML");

    SetStatusLightBox(xml);
    
    document.getElementById(hdnPersonIdClientID).value = personId;
    document.getElementById(hdnIdentNumberClientID).value = identNumber;
    document.getElementById("lblIdentNumberValue").innerHTML = identNumber;
    document.getElementById("lblFirstNameValue").innerHTML = firstName;
    document.getElementById("lblLastNameValue").innerHTML = lastName;
    document.getElementById("lblGenderValue").innerHTML = genderName;
    document.getElementById("lblLastModifiedValue").innerHTML = lastModified;
    document.getElementById("lblAgeValue").innerHTML = FormatAge(age, ageMonthsPart);    
    
    document.getElementById("txtPermCity").innerHTML = permCity;
    document.getElementById("txtPermDistrict").innerHTML = permDistrict;
    document.getElementById("txtPermMunicipality").innerHTML = permMunicipality;
    document.getElementById("txtPermRegion").innerHTML = permRegion;
    document.getElementById("txtPermAddress").innerHTML = permAddress;
    document.getElementById("txtPermPostCode").innerHTML = permSecondPostCode;

    document.getElementById("txtPresCity").innerHTML = presCity;
    document.getElementById("txtPresDistrict").innerHTML = presDistrict;
    document.getElementById("txtPresMunicipility").innerHTML = presMunicipility;
    document.getElementById("txtPresRegion").innerHTML = presRegion;
    document.getElementById("txtPresAddress").innerHTML = presAddress;
    document.getElementById("txtPresPostCode").innerHTML = presSecondPostCode;

    document.getElementById("txtContactCity").innerHTML = contactCity;
    document.getElementById("txtContactDistrict").innerHTML = contactDistrict;
    document.getElementById("txtContactMunicipality").innerHTML = contactMunicipality;
    document.getElementById("txtContactRegion").innerHTML = contactRegion;
    document.getElementById("txtContactAddress").innerHTML = contactAddress;
    document.getElementById("txtContactPostCode").innerHTML = contactSecondPostCode;

    document.getElementById("txtIDCardNumber").innerHTML = IDCardNumber;
    document.getElementById("txtIDCardIssuedBy").innerHTML = IDCardIssuedBy;
    document.getElementById("txtIDCardIssueDate").innerHTML = IDCardIssueDate;
    document.getElementById("txtHomePhone").innerHTML = homePhone;
    document.getElementById("txtMobilePhone").innerHTML = mobilePhone;
    document.getElementById("txtEmail").innerHTML = email;
    document.getElementById("txtDrvLicCategories").innerHTML = drvLicCategories;
    
    if(militaryUnitName === "")
    {
        document.getElementById("lblServeInMilitary").style.display = "none";
        document.getElementById("lblServeInMilitaryValue").style.display = "none";
    }
    else
        document.getElementById("lblServeInMilitaryValue").innerHTML = militaryUnitName;

    document.getElementById("divMedCertHTML").innerHTML = medCertHTML;
    document.getElementById("divPsychCertHTML").innerHTML = psychCertHTML;
   
    ShowContent();
}

//This function displays the content div and hides the "loading" div
function ShowContent()
{
    document.getElementById("loadingDiv").style.display = "none";
    document.getElementById("contentDiv").style.display = "";
}

function ddlMilitarySchoolYearChange(object)
{    
    JSLoadTab("btnTabSubjects");
}

// Display light box with education properties (for editing or adding new)
function ShowCadetEducationLightBox(civilEducationId)
{
    var url = "EditCadet.aspx?AjaxMethod=JSGetCadetEducationLightBoxContent";    
    var params = "";
    if (civilEducationId != 0) // gets current values if editing civil education
    {
        params += "CivilEducationId=" + civilEducationId;
    }
    
    function response_handler(xml)
    {
        if (document.getElementById("spanEducationMessage") != null)
            document.getElementById("spanEducationMessage").innerHTML = "";
            
        if (document.getElementById("spanLanguageMessage") != null)
            document.getElementById("spanLanguageMessage").innerHTML = "";

        var lightBoxHTML = xmlValue(xml, "lightBoxHTML");
        document.getElementById("CadetEducationLightBox").innerHTML = lightBoxHTML;

        if (lightBoxHTML != "") {
            document.getElementById("hdnCivilEducationID").value = civilEducationId; // setting civil education ID(0 - if new cadet education)
        }

        RefreshUIItems(xml);

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("CadetEducationLightBox").style.display = "";
        CenterLightBox("CadetEducationLightBox");
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}

// Close the light box and refresh cadet educations table
function HideCadetEducationLightBox()
{
  document.getElementById("HidePage").style.display = "none";
  document.getElementById("CadetEducationLightBox").style.display = "none";
}

// Validate cadet education properties in the light box and generates appropriate error messages, if needed
function ValidateCadetEducation()
{
   var res = true;
   var lightBoxMessage = document.getElementById("spanEducationLightBoxMessage");
   lightBoxMessage.innerHTML = "";

   var notValidFields = new Array();

   var ddCivilEduPersonEducation = document.getElementById("ddCivilEduPersonEducation");
   var txtCivilEduGraduateYear = document.getElementById("txtCivilEduGraduateYear");
   var ddCivilEduLearningMethod = document.getElementById("ddCivilEduLearningMethod");

   if (ddCivilEduPersonEducation.value == optionChooseOneValue) {
       res = false;

       if (ddCivilEduPersonEducation.disabled == true || ddCivilEduPersonEducation.style.display == "none")
           notValidFields.push("Образователна степен");
       else
           lightBoxMessage.innerHTML += GetErrorMessageMandatory("Образователна степен") + "<br />";
   }

   if (ItemSelectorUtil.GetSelectedValue("isCivilEduPersonSchoolSubject") == optionChooseOneValue) {
       res = false;

       if (ItemSelectorUtil.IsDisabled("isCivilEduPersonSchoolSubject") || ItemSelectorUtil.IsHidden("isCivilEduPersonSchoolSubject"))
           notValidFields.push("Специалност");
       else
           lightBoxMessage.innerHTML += GetErrorMessageMandatory("Специалност") + "<br />";
   }

   if (txtCivilEduGraduateYear.value.Trim() == "") {
       res = false;

       if (txtCivilEduGraduateYear.disabled == true || txtCivilEduGraduateYear.style.display == "none")
           notValidFields.push("Година на завършване");
       else
           lightBoxMessage.innerHTML += GetErrorMessageMandatory("Година на завършване") + "<br />";
   }
   else {
       if (!isInt(txtCivilEduGraduateYear.value) || parseInt(txtCivilEduGraduateYear.value) <= 0) {
           res = false;
           lightBoxMessage.innerHTML += GetErrorMessageNumber("Година на завършване") + "<br />";
       }
   }

   if (ddCivilEduLearningMethod.value == optionChooseOneValue) {
       res = false;

       if (ddCivilEduLearningMethod.disabled == true || ddCivilEduLearningMethod.style.display == "none")
           notValidFields.push("Начин на обучение");
       else
           lightBoxMessage.innerHTML += GetErrorMessageMandatory("Начин на обучение") + "<br />";
   }

   var notValidFieldsCount = notValidFields.length;

   if (notValidFieldsCount > 0) {
       var noRightsMessage = GetErrorMessageNoRights(notValidFields);
       lightBoxMessage.innerHTML += "<br />" + noRightsMessage;
   }

   if (res)
       ForceNoChanges();

   return res;
}

// Saves cadet education through ajax request, if light box values are valid, or displays generated error messages
function SaveCadetEducation()
{
    var lightBoxMessage = document.getElementById("spanEducationLightBoxMessage");
    
    if(ValidateCadetEducation()) {
        var civilEducationId = document.getElementById("hdnCivilEducationID").value;
        
        var url = "EditCadet.aspx?AjaxMethod=JSSaveCadetEducation&HdnPersonId=" + document.getElementById(hdnPersonIdClientID).value;

        var params = "CivilEducationId=" + civilEducationId +
                     "&PersonEducationCode=" + GetSelectedItemId(document.getElementById("ddCivilEduPersonEducation")) +
                     "&PersonSchoolSubjectCode=" + document.getElementById("hdnSchoolSubjectCode").value +
                     "&GraduateYear=" + document.getElementById("txtCivilEduGraduateYear").value +
                     "&LearningMethodKey=" + document.getElementById("ddCivilEduLearningMethod").value +
                     "&HdnCadetId=" + document.getElementById(hdnCadetIdClientID).value;

        function response_handler(xml)
        {            
            var hideDialog = true;
            var result = xmlNodeText(xml.childNodes[0]);
            if (result == "OK")
            {
                lightBoxMessage.innerHTML = "";
                lightBoxMessage.style.display = "";
                hideDialog = false;
            }
            else if(result == "ERROR")
                lightBoxMessage.value = "Образованието не е добавено";
            else
                document.getElementById("divCadetEduTable").innerHTML = result;
                
            if (hideDialog)
                HideCadetEducationLightBox();
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
    else
    {
        lightBoxMessage.style.display = "";
    }
}

function DeleteCadetEducation(civilEducationId)
{
    YesNoDialog("Желаете ли да изтриете образованието?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "EditCadet.aspx?AjaxMethod=JSDeleteCadetEducation";
        var params = "";
        params += "SelectedTabId=btnTabEducation";
        params += "&HdnPersonId=" + document.getElementById(hdnPersonIdClientID).value;
        params += "&CivilEducationId=" + civilEducationId;
        params += "&HdnCadetId=" + document.getElementById(hdnCadetIdClientID).value;
        
        function response_handler(xml)
        {
            if (document.getElementById("spanLanguageMessage") != null)
                document.getElementById("spanLanguageMessage").innerHTML = "";

            document.getElementById("divCadetEduTable").innerHTML = xmlNodeText(xml.childNodes[0]);
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

// Display light box with language properties (for editing or adding new)
function ShowCadetLanguageLightBox(foreignLanguageId)
{    
    var url = "EditCadet.aspx?AjaxMethod=JSGetCadetLanguageLightBoxContent";    
    var params = "";
    params += "&HdnPersonId=" + document.getElementById(hdnPersonIdClientID).value;
    params += "&HdnCadetId=" + document.getElementById(hdnCadetIdClientID).value;

    if (foreignLanguageId != 0) // gets current values if editing cadet language
    {
        params += "&ForeignLanguageId=" + foreignLanguageId;
    }
    
    function response_handler(xml)
    {
        if (document.getElementById("spanEducationMessage") != null)
            document.getElementById("spanEducationMessage").innerHTML = "";
        
        if (document.getElementById("spanLanguageMessage") != null)
            document.getElementById("spanLanguageMessage").innerHTML = "";

        var lightBoxHTML = xmlValue(xml, "lightBoxHTML");
        document.getElementById("CadetLanguageLightBox").innerHTML = lightBoxHTML;

        if (lightBoxHTML != "") {
            document.getElementById("hdnForeignLanguageID").value = foreignLanguageId; // setting foreign language ID(0 - if new cadet language)
        }

        RefreshDatePickers();
        RefreshUIItems(xml);

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("CadetLanguageLightBox").style.display = "";
        CenterLightBox("CadetLanguageLightBox");
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}

// Close the light box and refresh cadet languages table
function HideCadetLanguageLightBox()
{
  document.getElementById("HidePage").style.display = "none";
  document.getElementById("CadetLanguageLightBox").style.display = "none";
}

// Validate cadet language properties in the light box and generates appropriate error messages, if needed
function ValidateCadetLanguage()
{
   var res = true;
   var lightBoxMessage = document.getElementById("spanLanguageLightBoxMessage");
   lightBoxMessage.innerHTML = "";

   var notValidFields = new Array();

   var ddPersonLangEduForeignLanguagePersonLanguage = document.getElementById("ddPersonLangEduForeignLanguagePersonLanguage");
   var ddPersonLangEduForeignLanguageLanguageLevel = document.getElementById("ddPersonLangEduForeignLanguageLanguageLevel");
   //    var txtPersonLangEduForeignLanguageSTANAG = document.getElementById("txtPersonLangEduForeignLanguageSTANAG");
   var ddPersonLangEduForeignLanguageLanguageForm = document.getElementById("ddPersonLangEduForeignLanguageLanguageForm");
   var ddPersonLangEduForeignLanguageLanguageDiploma = document.getElementById("ddPersonLangEduForeignLanguageLanguageDiploma");
   var txtPersonLangEduForeignLanguageLanguageVacAnn = document.getElementById("txtPersonLangEduForeignLanguageLanguageVacAnn");
   var txtPersonLangEduForeignLanguageLanguageDateWhen = document.getElementById("txtPersonLangEduForeignLanguageLanguageDateWhen");

   if (ddPersonLangEduForeignLanguagePersonLanguage.value == optionChooseOneValue) {
       res = false;

       if (ddPersonLangEduForeignLanguagePersonLanguage.disabled == true || ddPersonLangEduForeignLanguagePersonLanguage.style.display == "none")
           notValidFields.push("Език");
       else
           lightBoxMessage.innerHTML += GetErrorMessageMandatory("Език") + "<br />";
   }

   if (ddPersonLangEduForeignLanguageLanguageLevel.value == optionChooseOneValue) {
       res = false;

       if (ddPersonLangEduForeignLanguageLanguageLevel.disabled == true || ddPersonLangEduForeignLanguageLanguageLevel.style.display == "none")
           notValidFields.push("Степен на владеене");
       else
           lightBoxMessage.innerHTML += GetErrorMessageMandatory("Степен на владеене") + "<br />";
   }

   if (ddPersonLangEduForeignLanguageLanguageForm.value == optionChooseOneValue) {
       res = false;

       if (ddPersonLangEduForeignLanguageLanguageForm.disabled == true || ddPersonLangEduForeignLanguageLanguageForm.style.display == "none")
           notValidFields.push("Форма на владеене");
       else
           lightBoxMessage.innerHTML += GetErrorMessageMandatory("Форма на владеене") + "<br />";
   }

   if (ddPersonLangEduForeignLanguageLanguageDiploma.value == optionChooseOneValue) {
       res = false;

       if (ddPersonLangEduForeignLanguageLanguageDiploma.disabled == true || ddPersonLangEduForeignLanguageLanguageDiploma.style.display == "none")
           notValidFields.push("Диплом");
       else
           lightBoxMessage.innerHTML += GetErrorMessageMandatory("Диплом") + "<br />";
   }

   if (txtPersonLangEduForeignLanguageLanguageDateWhen.value.Trim() != "") {
       if (!IsValidDate(txtPersonLangEduForeignLanguageLanguageDateWhen.value)) {
           res = false;
           lightBoxMessage.innerHTML += GetErrorMessageDate("Дата на документа") + "<br />";
       }
   }

   var notValidFieldsCount = notValidFields.length;

   if (notValidFieldsCount > 0) {
       var noRightsMessage = GetErrorMessageNoRights(notValidFields);
       lightBoxMessage.innerHTML += "<br />" + noRightsMessage;
   }

   if (res)
       ForceNoChanges();

   return res;
}

// Saves cadet language through ajax request, if light box values are valid, or displays generated error messages
function SaveCadetLanguage()
{
    var lightBoxMessage = document.getElementById("spanLanguageLightBoxMessage");
    
    if(ValidateCadetLanguage())
    {
        var url = "EditCadet.aspx?AjaxMethod=JSSaveCadetLanguage&HdnPersonId=" + document.getElementById(hdnPersonIdClientID).value;

        var params = "ForeignLanguageId=" + document.getElementById("hdnForeignLanguageID").value +
                     "&LanguageCode=" + document.getElementById("ddPersonLangEduForeignLanguagePersonLanguage").value +
                     "&LanguageLevelOfKnowledgeKey=" + document.getElementById("ddPersonLangEduForeignLanguageLanguageLevel").value +
        //           "&LanguageStanAg=" + document.getElementById("txtPersonLangEduForeignLanguageSTANAG").value +
                     "&LanguageFormOfKnowledgeKey=" + document.getElementById("ddPersonLangEduForeignLanguageLanguageForm").value +
                     "&LanguageDiplomaKey=" + document.getElementById("ddPersonLangEduForeignLanguageLanguageDiploma").value +
                     "&LanguageVacAnn=" + document.getElementById("txtPersonLangEduForeignLanguageLanguageVacAnn").value +
                     "&LanguageDateWhen=" + document.getElementById("txtPersonLangEduForeignLanguageLanguageDateWhen").value +
                     "&HdnCadetId=" + document.getElementById(hdnCadetIdClientID).value;

        function response_handler(xml)
        {            
            var hideDialog = true;
            var result = xmlNodeText(xml.childNodes[0]);
            if (result == "OK")
            {
                lightBoxMessage.innerHTML = "";
                lightBoxMessage.style.display = "";
                hideDialog = false;
            }
            else if(result == "ERROR")
                lightBoxMessage.value = "Езиковата подготовка не е добавена";
            else
                document.getElementById("divCadetlLangTable").innerHTML = result;
                
            if (hideDialog)
                HideCadetLanguageLightBox();
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
    else
    {
        lightBoxMessage.style.display = "";
    }
}

function DeleteCadetLanguage(foreignLanguageId)
{
    YesNoDialog("Желаете ли да изтриете езиковата подготовка?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "EditCadet.aspx?AjaxMethod=JSDeleteCadetLanguage";
        var params = "";
        params += "SelectedTabId=btnTabEducation";
        params += "&HdnCadetId=" + document.getElementById(hdnCadetIdClientID).value;
        params += "&HdnPersonId=" + document.getElementById(hdnPersonIdClientID).value;
        params += "&ForeignLanguageId=" + foreignLanguageId;
        
        function response_handler(xml)
        {
            if (document.getElementById("spanEducationMessage") != null)
                document.getElementById("spanEducationMessage").innerHTML = "";

            document.getElementById("divCadetlLangTable").innerHTML = xmlNodeText(xml.childNodes[0]);
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Tabs logic
//This is the ID of the selected tab (its button)
var selectedTab = "btnTabSubjects";

//Call this when hovering over a particualr tab button
function TabHover(tab)
{
    if (tab.id != selectedTab)
    {
        tab.className = "HoverTab";
    }
}

//Call this when leaving a particualr tab button
function TabOut(tab)
{
    if (tab.id != selectedTab)
    {
        tab.className = "InactiveTab";
    }
}

//Call this when a particular tab is clicked
function TabClick(tab)
{
    //Clear any messages
    //  document.getElementById("<%= lblMessage.ClientID %>").innerHTML = "";

    //Set the previously selected tab as inactive
    if (document.getElementById(selectedTab) != null)
    document.getElementById(selectedTab).className = "InactiveTab";

    //Set the current tab as active
    tab.className = "ActiveTab";
    selectedTab = tab.id;

    //Check if this tab has been already loaded
    //If it hasn't been loaded yet then get its content from the server via AJAX
    if (!IsTabAlreadyVisited())
    {
        JSLoadTab(selectedTab);
    }
    else //If the tab has been already loaded then just display its content
    {
        ShowDiv(selectedTab);
    }
}

//Check if a particular tab has been visited
//Store this information as an attribute of the tab button
function IsTabAlreadyVisited()
{
    if (document.getElementById(selectedTab).getAttribute("isalrеadyvisited") &&
  document.getElementById(selectedTab).getAttribute("isalrеadyvisited") == "true")
        return true;
    else
        return false;
}

//Show the content of the currently selected tab
function ShowDiv(tab)
{
    selectedTab = tab;
    
    //Hide all divs
    document.getElementById(divSubjectsClientID).style.display = "none";
    document.getElementById(divEducationClientID).style.display = "none";

    //Display the content of the current tab
    var targetDivId = GetTargetDivByTabId(selectedTab);
    document.getElementById(targetDivId).style.display = "";

    //Mark it as visited
    document.getElementById(selectedTab).setAttribute("isalrеadyvisited", "true");
}

//Load the content of a particular tab via an AJAX call
function JSLoadTab(selectedTabId)
{
    var url = "EditCadet.aspx?AjaxMethod=JSLoadTab&HdnPersonId=" + document.getElementById(hdnPersonIdClientID).value + "&MilitaryDepartmentId=" + document.getElementById(hdnMilitaryDepartmentIdClientID).value + "&HdnCadetId=" + document.getElementById(hdnCadetIdClientID).value;
    var params = "";
    params += "SelectedTabId=" + selectedTabId;
    if (selectedTab == "btnTabSubjects" && document.getElementById('ddlMilitarySchoolYears') != null)
        params += "&Year=" + GetSelectedItemId(document.getElementById('ddlMilitarySchoolYears'));
    var myAJAX = new AJAX(url, true, params, function(xml) { JSLoadTab_CallBack(xml, selectedTabId); });
    myAJAX.Call();
}

//When the response is ready then get the loaded HTML and put it on the target div
function JSLoadTab_CallBack(xml, selectedTabId)
{
    var targetDivId = GetTargetDivByTabId(selectedTabId);
    document.getElementById(targetDivId).innerHTML = xmlValue(xml, "TabHTML");
    //Show the loaded content
    ShowDiv(selectedTabId);
}

//Use this function to get the client id of the content div for a particular tab
function GetTargetDivByTabId(selectedTabId)
{
    var targetDivId = "";

    switch (selectedTabId)
    {
        case "btnTabSubjects":
            {
                targetDivId = divSubjectsClientID;
                break;
            }
        case "btnTabEducation":
            {
                targetDivId = divEducationClientID;
                break;
            }
    }

    return targetDivId;
}

//Reset the tab selection to the first (the default) tab
function ResetTabs()
{
    var selectedTab = "btnTabSubjects";

    if (document.getElementById("btnTabSubjects") != null)
        selectedTab = "btnTabSubjects";
    else if (document.getElementById("btnTabEducation") != null)
         selectedTab = "btnTabEducation";                            
    else
        return;

    var tab = document.getElementById(selectedTab);

    tab.className = "ActiveTab";
    selectedTab = tab.id;
    JSLoadTab(selectedTab);
}

// Display light box with all available military school specializations
function ShowMilitarySchoolSpecializationsLightBox(orderBy, pageIdx)
{
    GetMilitarySchoolSpecializations(orderBy, pageIdx);
}

// Close the light box 
function HideMilitarySchoolSpecializationsLightBox()
{
  document.getElementById("HidePage").style.display = "none";
  document.getElementById("divMilitarySchoolSpecializationsLightBox").style.display = "none";
}

// Call web server and retrieve the generated military school specialization items grid from it
function GetMilitarySchoolSpecializations(orderBy, pageIdx)
{
    var url = "EditCadet.aspx?AjaxMethod=JSGetMilitarySchoolSpecializationsLightBoxContent";    
    var params = "";
    params += "HdnCadetId=" + document.getElementById(hdnCadetIdClientID).value;
    params += "&HdnPersonId=" + document.getElementById(hdnPersonIdClientID).value;
    params += "&Year=" + GetSelectedItemId(document.getElementById('ddlMilitarySchoolYears'));
    params += "&SpecTableOrderBy=" + orderBy;
    params += "&SpecTablePageIdx=" + pageIdx;
    
    function response_handler(xml)
    {
        if (document.getElementById("spanSubjectMessage") != null)
            document.getElementById("spanSubjectMessage").innerHTML = "";

        document.getElementById("divMilitarySchoolSpecializationsContent").innerHTML = xmlNodeText(xml.childNodes[0]);

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("divMilitarySchoolSpecializationsLightBox").style.display = "";
        CenterLightBox("divMilitarySchoolSpecializationsLightBox");
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}

// Call web server and retrieve the generated military school specialization items grid from it
function GetMilitarySchoolSpecializationItems(orderBy, pageIdx)
{
    var url = "EditCadet.aspx?AjaxMethod=JSGetMilitarySchoolSpecializationsLightBoxContent";    
    var params = "";
    params += "HdnCadetId=" + document.getElementById(hdnCadetIdClientID).value;
    params += "&HdnPersonId=" + document.getElementById(hdnPersonIdClientID).value;
    params += "&Year=" + GetSelectedItemId(document.getElementById('ddlMilitarySchoolYears'));
    params += "&MilitarySchoolId=" + GetSelectedItemId(document.getElementById('ddlMilitarySchools'));
    params += "&SpecTableOrderBy=" + orderBy;
    params += "&SpecTablePageIdx=" + pageIdx;
    
    function response_handler(xml)
    {
        document.getElementById("divMilitarySchoolSpecializationsContent").innerHTML = xmlNodeText(xml.childNodes[0]);
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}

function SortMilitarySchoolSpecializationsTableBy(sort)
{
    var pageIdx = document.getElementById('hdnMilitarySchoolSpecializationsTablePageIdx').value;
    var orderBy = document.getElementById('hdnMilitarySchoolSpecializationsTableOrderBy').value;
    if (orderBy == sort)
    {
        sort = sort + 100;
    }
    
    orderBy = sort;
    
    GetMilitarySchoolSpecializationItems(orderBy, pageIdx);
}

//Go to the first page and refresh the grid
function BtnMilitarySchoolSpecializationsTableFirstClick()
{
    var orderBy = document.getElementById('hdnMilitarySchoolSpecializationsTableOrderBy').value;
    var pageIdx = 1;
    var maxPage = document.getElementById('hdnMilitarySchoolSpecializationsTableMaxPage').value;
    
    GetMilitarySchoolSpecializationItems(orderBy, pageIdx);
}

//Go to the previous page and refresh the grid
function BtnMilitarySchoolSpecializationsTablePrevClick()
{
    var orderBy = document.getElementById('hdnMilitarySchoolSpecializationsTableOrderBy').value;
    var pageIdx = document.getElementById('hdnMilitarySchoolSpecializationsTablePageIdx').value;

    if (pageIdx > 1)
    {
        pageIdx--;
        GetMilitarySchoolSpecializationItems(orderBy, pageIdx);
    }
}

//Go to the next page and refresh the grid
function BtnMilitarySchoolSpecializationsTableNextClick()
{
    var orderBy = document.getElementById('hdnMilitarySchoolSpecializationsTableOrderBy').value;
    var pageIdx = document.getElementById('hdnMilitarySchoolSpecializationsTablePageIdx').value;
    var maxPage = document.getElementById('hdnMilitarySchoolSpecializationsTableMaxPage').value;

    if (pageIdx < maxPage)
    {
        pageIdx++;
        GetMilitarySchoolSpecializationItems(orderBy, pageIdx);
    }
}

//Go to the last page and refresh the grid
function BtnMilitarySchoolSpecializationsTableLastClick()
{
    var orderBy = document.getElementById('hdnMilitarySchoolSpecializationsTableOrderBy').value;
    var pageIdx;
    var maxPage = document.getElementById('hdnMilitarySchoolSpecializationsTableMaxPage').value;

    pageIdx = maxPage;
    GetMilitarySchoolSpecializationItems(orderBy, pageIdx);
}

//Go to a specific page (entered by the user) and refresh the grid
function BtnMilitarySchoolSpecializationsTableGotoClick()
{
    var orderBy = document.getElementById('hdnMilitarySchoolSpecializationsTableOrderBy').value;
    var pageIdx;
    var maxPage = document.getElementById('hdnMilitarySchoolSpecializationsTableMaxPage').value;
    var goToPage = document.getElementById('txtMilitarySchoolSpecializationsTableGotoPage').value;

    if (isInt(TrimString(goToPage)) && goToPage > 0 && goToPage <= maxPage)
    {
        pageIdx = goToPage;
        GetMilitarySchoolSpecializationItems(orderBy, pageIdx);
    }
}

// Call web server to add selected specialization item to the contextual military school and year
function AddMilitarySchoolSpecializationToPerson(militarySchoolSpecializationId)
{
    var url = "EditCadet.aspx?AjaxMethod=JSAddMilitarySchoolSpecializationToPerson";
    var params = "";
    params += "&HdnPersonId=" + document.getElementById(hdnPersonIdClientID).value;
    params += "&MilitarySchoolSpecializationId=" + militarySchoolSpecializationId;
    params += "&MilitaryDepartmentId=" + document.getElementById(hdnMilitaryDepartmentIdClientID).value;
    params += "&Year=" + GetSelectedItemId(document.getElementById('ddlMilitarySchoolYears'));
    
    function response_handler(xml)
    {
        HideMilitarySchoolSpecializationsLightBox();
        document.getElementById(divSubjectsClientID).innerHTML = xmlNodeText(xml.childNodes[0]);
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}      

function DeleteMilitarySchoolSpecialization(cadetSchoolSubjectId)
{
    YesNoDialog("Желаете ли да изтриете специализацията?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "EditCadet.aspx?AjaxMethod=JSDeleteMilitarySchoolSpecialization";
        var params = "";
        params += "&HdnCadetId=" + document.getElementById(hdnCadetIdClientID).value;
        params += "&HdnPersonId=" + document.getElementById(hdnPersonIdClientID).value;
        params += "&CadetSchoolSubjectId=" + cadetSchoolSubjectId;
        
        function response_handler(xml)
        {
            document.getElementById(divSubjectsClientID).innerHTML = xmlNodeText(xml.childNodes[0]);
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

function ShowStatusLightBox() {

    //shows the light box and "disable" rest of the page
    document.getElementById("HidePage").style.display = "";
    document.getElementById("lboxStatus").style.display = "";
    CenterLightBox("lboxStatus");
}

function HideStatusLightBox() {
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("lboxStatus").style.display = "none";
}

function SetStatusLightBox(xml) {
    var personStatus = xml.getElementsByTagName("PersonStatus")[0];
    var status = xmlValue(personStatus, "PersonStatus_Status");
    document.getElementById("spanPersonStatus").innerHTML = status;

    var lBoxStatusHTML = "<center><table>";
    lBoxStatusHTML += "<tr><td style='text-align:right;vertical-align:text-top;'><span class='InputLabel'>Статус: </span></td><td><span class='ReadOnlyValue'>" + status + "</span></td></tr>";

    var personStatus_Details = personStatus.getElementsByTagName("PersonStatus_Details")[0];
    personStatus_Details = personStatus_Details.getElementsByTagName("PersonStatus_Detail");

    for (var i = 0; i < personStatus_Details.length; i++) {
        var key = xmlValue(personStatus_Details[i], "PersonStatus_Detail_Key");
        var value = xmlValue(personStatus_Details[i], "PersonStatus_Detail_Value");

        lBoxStatusHTML += "<tr><td style='text-align:right;vertical-align:text-top;'><span class='InputLabel'>" + key + ": </span></td><td><span class='ReadOnlyValue'>" + value + "</span></td></tr>";
    }
    lBoxStatusHTML += "</table></center>";

    document.getElementById("lboxStatus_Msg").innerHTML = lBoxStatusHTML;
}

function CivilEducationSelector_OnSelectedCivilEducation(schoolSubjectCode, schoolSubjectName) {

    document.getElementById("txtSubject").innerHTML = schoolSubjectName;
    document.getElementById("hdnSchoolSubjectCode").value = schoolSubjectCode;
}

function RefreshUIItems(xml, UIItemsTag) {
    var tag = "UIItems";

    if (typeof UIItemsTag != "undefined")
        tag = UIItemsTag;

    //Setup the UIItems logic on the loaded tab
    var UIItems = xml.getElementsByTagName(tag);
    if (UIItems.length > 0) {
        var disabledClientControls = xmlValue(UIItems[0], "disabledClientControls");
        var hiddenClientControls = xmlValue(UIItems[0], "hiddenClientControls");

        if (disabledClientControls != "") {
            document.getElementById(hdnDisabledClientControls).value += (document.getElementById(hdnDisabledClientControls).value == "" ? "" : ",") + disabledClientControls;
            CheckDisabledClientControls();
        }
        if (hiddenClientControls != "") {
            document.getElementById(hdnHiddenClientControls).value += (document.getElementById(hdnHiddenClientControls).value == "" ? "" : ",") + hiddenClientControls;
            CheckHiddenClientControls();
        }
    }
}