window.addEventListener ? window.addEventListener('load', PageLoad, false) : window.attachEvent('onload', PageLoad);

//Call this function when the page is loaded
function PageLoad()
{
    LoadPersonDetails();
}

//This function load the person's details
function LoadPersonDetails()
{
    var url = "EditApplicant.aspx?AjaxMethod=JSLoadPersonDetails";
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
    var wentToMilitary = xmlValue(person, "wentToMilitary");
    var militaryTraining = xmlValue(person, "MilitaryTraining");

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
    document.getElementById("txtWentToMilitary").innerHTML = wentToMilitary;
    document.getElementById("txtMilitaryTraining").innerHTML = militaryTraining;

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

// Display light box with education properties (for editing or adding new)
function ShowApplicantEducationLightBox(civilEducationId)
{
    var url = "EditApplicant.aspx?AjaxMethod=JSGetApplicantEducationLightBoxContent";    
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
        document.getElementById("ApplicantEducationLightBox").innerHTML = lightBoxHTML;

        if (lightBoxHTML != "")
        {
            document.getElementById("hdnCivilEducationID").value = civilEducationId; // setting civil education ID(0 - if new applicant education)
        }

        RefreshUIItems(xml);        

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("ApplicantEducationLightBox").style.display = "";
        CenterLightBox("ApplicantEducationLightBox");
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}

// Close the light box and refresh applicant educations table
function HideApplicantEducationLightBox()
{
  document.getElementById("HidePage").style.display = "none";
  document.getElementById("ApplicantEducationLightBox").style.display = "none";
}

// Validate applicant education properties in the light box and generates appropriate error messages, if needed
   function ValidateApplicantEducation()
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

// Saves applicant education through ajax request, if light box values are valid, or displays generated error messages
function SaveApplicantEducation()
{
    var lightBoxMessage = document.getElementById("spanEducationLightBoxMessage");
    
    if (ValidateApplicantEducation())
    {
        var civilEducationId = document.getElementById("hdnCivilEducationID").value;
        
        var url = "EditApplicant.aspx?AjaxMethod=JSSaveApplicantEducation&HdnPersonId=" + document.getElementById(hdnPersonIdClientID).value;

        var params = "CivilEducationId=" + civilEducationId +
                     "&PersonEducationCode=" + document.getElementById("ddCivilEduPersonEducation").value +
                     "&PersonSchoolSubjectCode=" + document.getElementById("hdnSchoolSubjectCode").value +
                     "&GraduateYear=" + document.getElementById("txtCivilEduGraduateYear").value +
                     "&LearningMethodKey=" + document.getElementById("ddCivilEduLearningMethod").value +
                     "&HdnApplicantId=" + document.getElementById(hdnApplicantIdClientID).value;

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
                document.getElementById("divApplEduTable").innerHTML = result;
                
            if (hideDialog)
                HideApplicantEducationLightBox();
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
    else
    {
        lightBoxMessage.style.display = "";
    }
}

function DeleteApplicantEducation(civilEducationId)
{
    YesNoDialog("Желаете ли да изтриете образованието?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "EditApplicant.aspx?AjaxMethod=JSDeleteApplicantEducation";
        var params = "";
        params += "SelectedTabId=btnTabEducation";
        params += "&HdnPersonId=" + document.getElementById(hdnPersonIdClientID).value;
        params += "&CivilEducationId=" + civilEducationId;
        params += "&HdnApplicantId=" + document.getElementById(hdnApplicantIdClientID).value;
        
        function response_handler(xml)
        {           
            if (document.getElementById("spanLanguageMessage") != null)
                document.getElementById("spanLanguageMessage").innerHTML = "";

            document.getElementById("divApplEduTable").innerHTML = xmlNodeText(xml.childNodes[0]);
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

// Display light box with language properties (for editing or adding new)
function ShowApplicantLanguageLightBox(foreignLanguageId)
{
    var url = "EditApplicant.aspx?AjaxMethod=JSGetApplicantLanguageLightBoxContent";    
    var params = "";
    params += "&HdnPersonId=" + document.getElementById(hdnPersonIdClientID).value;

    if (foreignLanguageId != 0) // gets current values if editing applicant language
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
        document.getElementById("ApplicantLanguageLightBox").innerHTML = lightBoxHTML;

        if (lightBoxHTML != "")
        {
            document.getElementById("hdnForeignLanguageID").value = foreignLanguageId; // setting foreign language ID(0 - if new applicant language)
        }

        RefreshDatePickers();
        RefreshUIItems(xml);

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("ApplicantLanguageLightBox").style.display = "";
        CenterLightBox("ApplicantLanguageLightBox");
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}

// Close the light box and refresh applicant languages table
function HideApplicantLanguageLightBox()
{
  document.getElementById("HidePage").style.display = "none";
  document.getElementById("ApplicantLanguageLightBox").style.display = "none";
}

// Validate applicant language properties in the light box and generates appropriate error messages, if needed
   function ValidateApplicantLanguage()
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

// Saves applicant language through ajax request, if light box values are valid, or displays generated error messages
function SaveApplicantLanguage()
{
    var lightBoxMessage = document.getElementById("spanLanguageLightBoxMessage");
    
    if (ValidateApplicantLanguage())
    {
        var url = "EditApplicant.aspx?AjaxMethod=JSSaveApplicantLanguage&HdnPersonId=" + document.getElementById(hdnPersonIdClientID).value;

        var params = "ForeignLanguageId=" + document.getElementById("hdnForeignLanguageID").value +
                     "&LanguageCode=" + document.getElementById("ddPersonLangEduForeignLanguagePersonLanguage").value +
                     "&LanguageLevelOfKnowledgeKey=" + document.getElementById("ddPersonLangEduForeignLanguageLanguageLevel").value +
        //           "&LanguageStanAg=" + document.getElementById("txtPersonLangEduForeignLanguageSTANAG").value +
                     "&LanguageFormOfKnowledgeKey=" + document.getElementById("ddPersonLangEduForeignLanguageLanguageForm").value +
                     "&LanguageDiplomaKey=" + document.getElementById("ddPersonLangEduForeignLanguageLanguageDiploma").value +
                     "&LanguageVacAnn=" + document.getElementById("txtPersonLangEduForeignLanguageLanguageVacAnn").value +
                     "&LanguageDateWhen=" + document.getElementById("txtPersonLangEduForeignLanguageLanguageDateWhen").value +
                     "&HdnApplicantId=" + document.getElementById(hdnApplicantIdClientID).value;                     

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
                document.getElementById("divApplLangTable").innerHTML = result;
                
            if (hideDialog)
                HideApplicantLanguageLightBox();
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
    else
    {
        lightBoxMessage.style.display = "";
    }
}

function DeleteApplicantLanguage(foreignLanguageId)
{
    YesNoDialog("Желаете ли да изтриете езиковата подготовка?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "EditApplicant.aspx?AjaxMethod=JSDeleteApplicantLanguage";
        var params = "";
        params += "SelectedTabId=btnTabEducation";
        params += "&HdnPersonId=" + document.getElementById(hdnPersonIdClientID).value;
        params += "&HdnApplicantId=" + document.getElementById(hdnApplicantIdClientID).value;
        params += "&ForeignLanguageId=" + foreignLanguageId;
        
        function response_handler(xml)
        {
            if (document.getElementById("spanEducationMessage") != null)
                document.getElementById("spanEducationMessage").innerHTML = "";

            document.getElementById("divApplLangTable").innerHTML = xmlNodeText(xml.childNodes[0]);
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

function ddlVacancyAnnounceChange(object)
{    
    var url = "EditApplicant.aspx?AjaxMethod=JSGetApplicantDocuments";
    var params = "";
    params += "&HdnApplicantId=" + document.getElementById(hdnApplicantIdClientID).value;
    params += "&VacancyAnnounceId=" + object.value;
    
    function response_handler(xml)
    {
        document.getElementById("divApplicantDocuments").innerHTML = xmlNodeText(xml.childNodes[0]);
        LoadOriginalValues();
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}

function RefreshApplicantDocumentsTab()
{    
    var divApplicantDocuments = document.getElementById("divApplicantDocuments");
    if (divApplicantDocuments != null)
    {
        var url = "EditApplicant.aspx?AjaxMethod=JSGetApplicantDocuments";
        var params = "";
        params += "&HdnApplicantId=" + document.getElementById(hdnApplicantIdClientID).value;
        
        function response_handler(xml)
        {
            divApplicantDocuments.innerHTML = xmlNodeText(xml.childNodes[0]);
            LoadOriginalValues();
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

function SaveApplicantDocuments()
{    
    var url = "EditApplicant.aspx?AjaxMethod=JSSaveApplicantDocuments";
    var params = "";
    params += "&HdnPersonId=" + document.getElementById(hdnPersonIdClientID).value;
    params += "&HdnApplicantId=" + document.getElementById(hdnApplicantIdClientID).value;
    params += "&VacancyAnnounceId=" + GetSelectedItemId(document.getElementById("ddlVacancyAnnounces"));
    params += "&IdsList=" + GetAllApplicantDocsIds();
    
    function response_handler(xml)
    {
        document.getElementById("divApplicantDocuments").innerHTML = xmlNodeText(xml.childNodes[0]);
        LoadOriginalValues();
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}

function GetAllApplicantDocsIds()
{
    var applDocsList = "";
    var applDocsCount = document.getElementById("hdnApplicantDocumentsCount").value;
    for (var i = 1; i <= applDocsCount; i++)
    {
        var hdnDocId = document.getElementById("hdnDocId" + i).value;
        var applDocStatusId = GetSelectedItemId(document.getElementById("ddlApplicantDocumentStatuses" + i));
        
        if (i == 1)
            applDocsList = hdnDocId + "," + applDocStatusId;
        else
            applDocsList += (";" + hdnDocId + "," + applDocStatusId);
    }
    
    return applDocsList;
}


function SelectAllDocCB()
{
    var appsCount = document.getElementById("hdnApplicantDocumentsCount").value;

    for (var i = 1; i <= appsCount; i++)
    {
        document.getElementById("cbDoc" + i.toString()).checked = document.getElementById("cbDocAll").checked;
    }

    if (document.getElementById("cbDocAll").checked)
        document.getElementById("cbDocAll").title = "Изчисти селектираните";
    else
        document.getElementById("cbDocAll").title = "Селектирай всички";
    
}

// Display light box with language properties (for editing or adding new)
function ShowApplicantDocumentStatusLightBox()
{
    var url = "EditApplicant.aspx?AjaxMethod=JSGetApplicantDocumentStatusLightBoxContent";
    var params = "";       

    function response_handler(xml)
    {
        document.getElementById("ApplicantDocumentStatusLightBox").innerHTML = xmlNodeText(xml.childNodes[0]);
                     
        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("ApplicantDocumentStatusLightBox").style.display = "";
        CenterLightBox("ApplicantDocumentStatusLightBox");
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}

// Close the light box and refresh applicant languages table
function HideApplicantDocumentStatusLightBox()
{
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("ApplicantDocumentStatusLightBox").style.display = "none";
}

function ChangeApplicantDocumentStatuses()
{    
    var appsCount = document.getElementById("hdnApplicantDocumentsCount").value;

    for (var i = 1; i <= appsCount; i++)
    {
        if (document.getElementById("cbDoc" + i.toString()).checked)
        {
            document.getElementById("ddlApplicantDocumentStatuses" + i.toString()).value = document.getElementById("ddlChangeApplicantDocumentStatuses").value;
        }
    }
    
    HideApplicantDocumentStatusLightBox();
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