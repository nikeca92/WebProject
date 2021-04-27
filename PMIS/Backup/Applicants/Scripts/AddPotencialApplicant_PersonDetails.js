window.addEventListener ? window.addEventListener('load', PageLoad, false) : window.attachEvent('onload', PageLoad);
var goToShow = false;
var isRegistred;

//Call this function when the page is loaded
function PageLoad() {
    LoadPickLists();
    LoadPersonDetails();
    SetEventHandlers();

    SetClientTextAreaMaxLength("txtaComment", "4000");
    SetClientTextAreaMaxLength("txtaPermAddress", "500");
    SetClientTextAreaMaxLength("txtaPresAddress", "1500");
    SetClientTextAreaMaxLength("txtaContactAddress", "1500");
}

function LoadPickLists() {
    var configPickListDrvLicCategories =
    {
        width: 175,
        allLabel: "<Всички>"
    }

    categories = document.getElementById(hdnDrvLicCategoriesClientID).value;
    categories = eval(categories);
    PickListUtil.AddPickList("pickListDrvLicCategories", categories, "tdPickListDrvLicCategories", configPickListDrvLicCategories);
}

function LoadShuttleControl(shuttleControlID, availableOptions, assignedOptions) {
    ClearSelectList(document.getElementById(shuttleControlID + "_AvailableOptions"), true);
    //ClearSelectList(document.getElementById(shuttleControlID + "_AssignedOptions"), true);

    var options = availableOptions.getElementsByTagName("el");

    for (var i = 0; i < options.length; i++) {
        var id = xmlValue(options[i], "id");
        var name = xmlValue(options[i], "name");

        AddToSelectList(document.getElementById(shuttleControlID + "_AvailableOptions"), id, name, true);
    };

    options = assignedOptions.getElementsByTagName("el");

    for (var i = 0; i < options.length; i++) {
        var id = xmlValue(options[i], "id");
        var name = xmlValue(options[i], "name");

        AddToSelectList(document.getElementById(shuttleControlID + "_AssignedOptions"), id, name, true);
    };
}

//This function load the person's details
function LoadPersonDetails() {
    var url = "AddPotencialApplicant_PersonDetails.aspx?AjaxMethod=JSLoadPersonDetails";
    var params = "";
    params += "IdentNumber=" + document.getElementById(hdnIdentNumberClientID).value;
    params += "&PotencialApplicantId=" + document.getElementById(hdnPotencialApplicantClientId).value;
    params += "&MilitaryDepartmentId=" + document.getElementById(hdnMilitaryDepartmentIDClientID).value;
    // params += "&IsRegistered=OK";
    var myAJAX = new AJAX(url, true, params, LoadPersonDetails_Callback);
    myAJAX.Call();
}

function LoadPersonDetails_Callback(xml) {
    document.getElementById("divEducation").innerHTML = xmlNodeText(xml.getElementsByTagName("HtmlEducation")[0]);
    document.getElementById("divLanguage").innerHTML = xmlNodeText(xml.getElementsByTagName("HtmlLanguage")[0]);

    document.getElementById("lblIdentNumberValue").innerHTML = document.getElementById(hdnIdentNumberClientID).value;

    var person = xml.getElementsByTagName("person")[0];
    var personId = xmlValue(person, "personId");
    var firstName = xmlValue(person, "firstName");
    var lastName = xmlValue(person, "lastName");
    var identNumber = xmlValue(person, "identNumber");
    var genderId = xmlValue(person, "genderId");
    var lastModified = xmlValue(person, "lastModified");
    var age = xmlValue(person, "age");
    var ageMonthsPart = xmlValue(person, "ageMonthsPart");
    var drivingLicenseCategories = xmlValue(person, "drivingLicenseCategories");
    var permCityId = xmlValue(person, "permCityId");
    var permDistrictId = xmlValue(person, "permDistrictId");
    var permPostCode = xmlValue(person, "permPostCode");
    var permSecondPostCode = xmlValue(person, "permSecondPostCode");
    var permRegionId = xmlValue(person, "permRegionId");
    var permMunicipalityId = xmlValue(person, "permMunicipalityId");
    var permAddress = xmlValue(person, "permAddress");
    var presCityId = xmlValue(person, "presCityId");
    var presDistrictId = xmlValue(person, "presDistrictId");
    var presPostCode = xmlValue(person, "presPostCode");
    var presSecondPostCode = xmlValue(person, "presSecondPostCode");
    var presRegionId = xmlValue(person, "presRegionId");
    var presMunicipalityId = xmlValue(person, "presMunicipalityId");
    var presAddress = xmlValue(person, "presAddress");
    var contactCityId = xmlValue(person, "contactCityId");
    var contactDistrictId = xmlValue(person, "contactDistrictId");
    var contactPostCode = xmlValue(person, "contactPostCode");
    var contactSecondPostCode = xmlValue(person, "contactSecondPostCode");
    var contactRegionId = xmlValue(person, "contactRegionId");
    var contactMunicipalityId = xmlValue(person, "contactMunicipalityId");
    var contactAddress = xmlValue(person, "contactAddress");
    var IDCardNumber = xmlValue(person, "IDCardNumber");
    var IDCardIssuedBy = xmlValue(person, "IDCardIssuedBy");
    var IDCardIssueDate = xmlValue(person, "IDCardIssueDate");
    var homePhone = xmlValue(person, "homePhone");
    var mobilePhone = xmlValue(person, "mobilePhone");
    var email = xmlValue(person, "email");
    var hasMilitarySrv = xmlValue(person, "hasMilitarySrv");
    var militaryTraining = xmlValue(person, "militaryTraining");    
    var txtaComment = xmlValue(person, "comment");
    var txtLastApperiance = xmlValue(person, "lastApperianceDate");
    var birthCountryId = xmlValue(person, "birthCountryId");
    var birthCityId = xmlValue(person, "birthCityId");
    var birthCityIfAbroad = xmlValue(person, "birthCityIfAbroad");

    SetStatusLightBox(xml);

    var availableOptions = xml.getElementsByTagName("availableServiceTypes")[0];
    var assignedOptions = xml.getElementsByTagName("assignedServiceTypes")[0];
    LoadShuttleControl("ddServiceTypes", availableOptions, assignedOptions);

    availableOptions = xml.getElementsByTagName("availableMilitaryTrainingCources")[0];
    assignedOptions = xml.getElementsByTagName("assignedMilitaryTrainingCources")[0];
    LoadShuttleControl("ddMTC", availableOptions, assignedOptions);

    isRegistred = xmlValue(person, "isRegistred");
    if (isRegistred == "OK") {
        document.getElementById(lblHeaderSubTitleClientId).innerHTML = "Редактиране на лични данни";
        document.getElementById("lblBack").innerHTML = "Назад";

    }
    else {
        document.getElementById(lblHeaderSubTitleClientId).innerHTML = "Добавяне на лични данни";
        document.getElementById("lblBack").innerHTML = "Отказ";
    }

    document.getElementById(hdnPersonIDClientID).value = personId;

    document.getElementById("txtFirstName").value = firstName;
    document.getElementById("txtLastName").value = lastName;
    document.getElementById("lblIdentNumberValue").innerHTML = identNumber;

    document.getElementById("ddGender").value = genderId;
    document.getElementById("lblLastModifiedValue").innerHTML = lastModified;
    document.getElementById("lblAgeValue").innerHTML = FormatAge(age, ageMonthsPart);    
    
    PickListUtil.SetSelection("pickListDrvLicCategories", drivingLicenseCategories);

    document.getElementById("txtPermPostCode").value = permSecondPostCode;

    //If there is a Perm City then load the entire info
    if (permCityId != "-1") {
        document.getElementById("ddPermRegion").value = permRegionId;

        ClearSelectList(document.getElementById("ddPermMunicipality"), true);

        var p_municipalities = xml.getElementsByTagName("p_m");

        for (var i = 0; i < p_municipalities.length; i++) {
            var id = xmlValue(p_municipalities[i], "id");
            var name = xmlValue(p_municipalities[i], "name");

            AddToSelectList(document.getElementById("ddPermMunicipality"), id, name);
        };

        document.getElementById("ddPermMunicipality").value = permMunicipalityId;


        ClearSelectList(document.getElementById("ddPermCity"), true);

        var p_cities = xml.getElementsByTagName("p_c");

        for (var i = 0; i < p_cities.length; i++) {
            var id = xmlValue(p_cities[i], "id");
            var name = xmlValue(p_cities[i], "name");

            AddToSelectList(document.getElementById("ddPermCity"), id, name);
        };

        document.getElementById("ddPermCity").value = permCityId;


        ClearSelectList(document.getElementById("ddPermDistrict"), true);

        var p_districts = xml.getElementsByTagName("p_d");

        for (var i = 0; i < p_districts.length; i++) {
            var id = xmlValue(p_districts[i], "id");
            var name = xmlValue(p_districts[i], "name");

            AddToSelectList(document.getElementById("ddPermDistrict"), id, name);
        };

        if (permDistrictId != "-1")
            document.getElementById("ddPermDistrict").value = permDistrictId;
    }
    else {
        document.getElementById("ddPermRegion").selectedIndex = 0;
        ClearSelectList(document.getElementById("ddPermMunicipality"), false);
        ClearSelectList(document.getElementById("ddPermCity"), false);
        ClearSelectList(document.getElementById("ddPermDistrict"), false);
    }

    document.getElementById("txtaPermAddress").value = permAddress;

    document.getElementById("txtPresPostCode").value = presSecondPostCode;

    //If there is a Pres City then load the entire info
    if (presCityId != "-1") {
        document.getElementById("ddPresRegion").value = presRegionId;

        ClearSelectList(document.getElementById("ddPresMunicipality"), true);

        var c_municipalities = xml.getElementsByTagName("c_m");

        for (var i = 0; i < c_municipalities.length; i++) {
            var id = xmlValue(c_municipalities[i], "id");
            var name = xmlValue(c_municipalities[i], "name");

            AddToSelectList(document.getElementById("ddPresMunicipality"), id, name);
        };

        document.getElementById("ddPresMunicipality").value = presMunicipalityId;


        ClearSelectList(document.getElementById("ddPresCity"), true);

        var c_cities = xml.getElementsByTagName("c_c");

        for (var i = 0; i < c_cities.length; i++) {
            var id = xmlValue(c_cities[i], "id");
            var name = xmlValue(c_cities[i], "name");

            AddToSelectList(document.getElementById("ddPresCity"), id, name);
        };

        document.getElementById("ddPresCity").value = presCityId;


        ClearSelectList(document.getElementById("ddPresDistrict"), true);

        var c_districts = xml.getElementsByTagName("c_d");

        for (var i = 0; i < c_districts.length; i++) {
            var id = xmlValue(c_districts[i], "id");
            var name = xmlValue(c_districts[i], "name");

            AddToSelectList(document.getElementById("ddPresDistrict"), id, name);
        };

        if (presDistrictId != "-1")
            document.getElementById("ddPresDistrict").value = presDistrictId;
    }
    else {
        document.getElementById("ddPresRegion").selectedIndex = 0;
        ClearSelectList(document.getElementById("ddPresMunicipality"), false);
        ClearSelectList(document.getElementById("ddPresCity"), false);
        ClearSelectList(document.getElementById("ddPresDistrict"), false);
    }

    document.getElementById("txtaPresAddress").value = presAddress;

    document.getElementById("txtContactPostCode").value = contactSecondPostCode;

    //If there is a Contact City then load the entire info
    if (contactCityId != "-1") {
        document.getElementById("ddContactRegion").value = contactRegionId;

        ClearSelectList(document.getElementById("ddContactMunicipality"), true);

        var con_municipalities = xml.getElementsByTagName("con_m");

        for (var i = 0; i < con_municipalities.length; i++) {
            var id = xmlValue(con_municipalities[i], "id");
            var name = xmlValue(con_municipalities[i], "name");

            AddToSelectList(document.getElementById("ddContactMunicipality"), id, name);
        };

        document.getElementById("ddContactMunicipality").value = contactMunicipalityId;


        ClearSelectList(document.getElementById("ddContactCity"), true);

        var con_cities = xml.getElementsByTagName("con_c");

        for (var i = 0; i < con_cities.length; i++) {
            var id = xmlValue(con_cities[i], "id");
            var name = xmlValue(con_cities[i], "name");

            AddToSelectList(document.getElementById("ddContactCity"), id, name);
        };

        document.getElementById("ddContactCity").value = contactCityId;


        ClearSelectList(document.getElementById("ddContactDistrict"), true);

        var con_districts = xml.getElementsByTagName("con_d");

        for (var i = 0; i < con_districts.length; i++) {
            var id = xmlValue(con_districts[i], "id");
            var name = xmlValue(con_districts[i], "name");

            AddToSelectList(document.getElementById("ddContactDistrict"), id, name);
        };

        if (contactDistrictId != "-1")
            document.getElementById("ddContactDistrict").value = contactDistrictId;
    }
    else {
        document.getElementById("ddContactRegion").selectedIndex = 0;
        ClearSelectList(document.getElementById("ddContactMunicipality"), false);
        ClearSelectList(document.getElementById("ddContactCity"), false);
        ClearSelectList(document.getElementById("ddContactDistrict"), false);
    }

    document.getElementById("txtaContactAddress").value = contactAddress;
    
    document.getElementById("txtIDCardNumber").value = IDCardNumber;
    document.getElementById("txtIDCardIssuedBy").value = IDCardIssuedBy;
    document.getElementById("txtIDCardIssueDate").value = IDCardIssueDate;
    document.getElementById("txtHomePhone").value = homePhone;
    document.getElementById("txtMobilePhone").value = mobilePhone;
    document.getElementById("txtEmail").value = email;
    SetRadioGroupValueByName("hasMilitarySrv", hasMilitarySrv);
    SetRadioGroupValueByName("MilitaryTraining", militaryTraining);    

    document.getElementById("txtaComment").value = txtaComment;
    document.getElementById(txtLastApperianceClientId).value = txtLastApperiance;

    document.getElementById("hdnBirthCountryId").value = birthCountryId;
    document.getElementById("hdnBirthCityId").value = birthCityId;
    document.getElementById("hdnBirthCityIfAbroad").value = birthCityIfAbroad;

    var disabledItems = "";
    var hiddenItems = "";


    disabledItems = xmlValue(xml, "listDisabledControls");
    hiddenItems = xmlValue(xml, "listHiddenControls");

    if (disabledItems != "") {
        document.getElementById(hdnDisabledClientControls).value += (document.getElementById(hdnDisabledClientControls).value == "" ? "" : ",") + disabledItems;
        CheckDisabledClientControls();
    }
    if (hiddenItems != "") {
        document.getElementById(hdnHiddenClientControls).value += (document.getElementById(hdnHiddenClientControls).value == "" ? "" : ",") + hiddenItems;
        CheckHiddenClientControls();
    }

    LoadOriginalValues();
    ShowContent();
}

//This function displays the content div and hides the "loading" div
function ShowContent() {
    document.getElementById("loadingDiv").style.display = "none";
    document.getElementById("contentDiv").style.display = "";
}

function Save_Click() {
    SaveData();

}

var ValidationMessage = "";
function ValidateData() {
    ValidationMessage = "";
    //Mandatory Fields
    if (document.getElementById("txtFirstName").value == "") {
        ValidationMessage += GetErrorMessageMandatory("Име и презиме") + "</br>";
    }

    if (document.getElementById("txtLastName").value == "") {
        ValidationMessage += GetErrorMessageMandatory("Фамилия") + "</br>";
    }

    var selectedItem = GetSelectedItemId(document.getElementById("ddPermCity"));

    if (selectedItem == -1) {
        ValidationMessage += GetErrorMessageMandatory("Постоянен адрес - Населено място") + "</br>";
    }

    selectedItem = GetSelectedItemId(document.getElementById("ddPresCity"));

    if (selectedItem == -1) {
        ValidationMessage += GetErrorMessageMandatory("Настоящ адрес - Населено място") + "</br>";
    }

    if (document.getElementById("txtIDCardIssueDate").value.Trim() != "") {
        if (!IsValidDate(document.getElementById("txtIDCardIssueDate").value)) {
            ValidationMessage += GetErrorMessageDate("Лична карта издадена на") + "</br>";
        }
    }

    //Type Value Fields
    if (document.getElementById("txtHomePhone").value != "") {
        if (!isInt(document.getElementById("txtHomePhone").value)) {
            ValidationMessage += GetErrorMessageNumber("Домашен телефон") + "</br>";
        }
    }

    if (document.getElementById(txtLastApperianceClientId).value != "") {
        if (!IsValidDate(document.getElementById(txtLastApperianceClientId).value)) {
            ValidationMessage += "</br>" + GetErrorMessageDate("Дата на последно явяване");
        }
    }


    return (ValidationMessage == "")
}
function SaveData() {
    if (ValidateData()) {
        var url = "AddPotencialApplicant_PersonDetails.aspx?AjaxMethod=JSSavePersonDetails";
        var params = "";
        params += "PersonID=" + document.getElementById(hdnPersonIDClientID).value;
        params += "&PotencialApplicantId=" + document.getElementById(hdnPotencialApplicantClientId).value;
        params += "&IdentNumber=" + document.getElementById(hdnIdentNumberClientID).value;
        params += "&MilitaryDepartmentId=" + document.getElementById(hdnMilitaryDepartmentIDClientID).value;
        params += "&FirstName=" + document.getElementById("txtFirstName").value;
        params += "&LastName=" + document.getElementById("txtLastName").value;
        params += "&GenderId=" + document.getElementById("ddGender").value;
        params += "&DrivingLicenseCategories=" + PickListUtil.GetSelectedValues("pickListDrvLicCategories");

        params += "&PermCityID=" + GetSelectedItemId(document.getElementById("ddPermCity"));
        params += "&PermDistrictID=" + document.getElementById("ddPermDistrict").value;
        params += "&PermSecondPostCode=" + document.getElementById("txtPermPostCode").value;
        params += "&PermAddress=" + document.getElementById("txtaPermAddress").value;

        params += "&PresCityID=" + GetSelectedItemId(document.getElementById("ddPresCity"));
        params += "&PresDistrictID=" + document.getElementById("ddPresDistrict").value;
        params += "&PresSecondPostCode=" + document.getElementById("txtPresPostCode").value;
        params += "&PresAddress=" + document.getElementById("txtaPresAddress").value;

        params += "&ContactCityID=" + GetSelectedItemId(document.getElementById("ddContactCity"));
        params += "&ContactDistrictID=" + document.getElementById("ddContactDistrict").value;
        params += "&ContactSecondPostCode=" + document.getElementById("txtContactPostCode").value;
        params += "&ContactAddress=" + document.getElementById("txtaContactAddress").value;

        params += "&IDCardNumber=" + custEncodeURI(document.getElementById("txtIDCardNumber").value);
        params += "&IDCardIssuedBy=" + custEncodeURI(document.getElementById("txtIDCardIssuedBy").value);
        params += "&IDCardIssueDate=" + document.getElementById("txtIDCardIssueDate").value;
        params += "&HomePhone=" + document.getElementById("txtHomePhone").value;
        params += "&MobilePhone=" + document.getElementById("txtMobilePhone").value;
        params += "&Email=" + document.getElementById("txtEmail").value;
        params += "&HasMilitarySrv=" + GetRadioGroupValueByName("hasMilitarySrv");
        params += "&MilitaryTraining=" + GetRadioGroupValueByName("MilitaryTraining");        
        params += "&Comment=" + document.getElementById("txtaComment").value;
        params += "&LastApperianceDate=" + document.getElementById(txtLastApperianceClientId).value;
        params += "&BirthCountryId=" + document.getElementById("hdnBirthCountryId").value;
        params += "&BirthCityId=" + document.getElementById("hdnBirthCityId").value;
        params += "&BirthCityIfAbroad=" + document.getElementById("hdnBirthCityIfAbroad").value;

        var serviceTypes = document.getElementById("ddServiceTypes_AssignedOptions");

        for (var i = 0; i < serviceTypes.options.length; i++) {
            var serviceTypeIdx = i + 1;

            params += "&STId_" + serviceTypeIdx + "=" + serviceTypes.options[i].value;
            params += "&STDisplayText_" + serviceTypeIdx + "=" + custEncodeURI(serviceTypes.options[i].text);
        }

        params += "&STCnt=" + serviceTypes.options.length;

        var MTCs = document.getElementById("ddMTC_AssignedOptions");

        for (var i = 0; i < MTCs.options.length; i++) {
            var MTCIdx = i + 1;

            params += "&MTCId_" + MTCIdx + "=" + MTCs.options[i].value;
            params += "&MTCDisplayText_" + MTCIdx + "=" + custEncodeURI(MTCs.options[i].text);
        }

        params += "&MTCCnt=" + MTCs.options.length;

        var myAJAX = new AJAX(url, true, params, SaveData_Callback);
        myAJAX.Call();
    }
    else {
        document.getElementById("lblErrorMsg").innerHTML = ValidationMessage;
        document.getElementById("lblErrorMsg").className = "ErrorText";
    }
}

function SaveData_Callback(xml) {

    //Refresh UI logic for Education and Language

    if (xml.getElementsByTagName("HtmlEducation")[0] != null) {
        document.getElementById("divEducation").innerHTML = xmlNodeText(xml.getElementsByTagName("HtmlEducation")[0]);
    }

    if (xml.getElementsByTagName("HtmlLanguage")[0] != null) {
        document.getElementById("divLanguage").innerHTML = xmlNodeText(xml.getElementsByTagName("HtmlLanguage")[0]);
    }

    isRegistred = xmlValue(xml, "isRegistred");

    var personId = xmlValue(xml, "personId");
    document.getElementById(hdnPersonIDClientID).value = personId;

    var potencialApplicantId = xmlValue(xml, "potencialApplicantId");
    document.getElementById(hdnPotencialApplicantClientId).value = potencialApplicantId;

    document.getElementById("lblErrorMsg").innerHTML = "Успешен запис";
    document.getElementById("lblErrorMsg").className = "SuccessText";

    document.getElementById(lblHeaderSubTitleClientId).innerHTML = "Редактиране на лични данни";
    document.getElementById("lblBack").innerHTML = "Назад";

    //Eanble DivButton in Education and Language Table  after Save operation

    if (document.getElementById("btnAddNewApplicantEducation") != undefined) {
        document.getElementById("btnAddNewApplicantEducation").className = "Button"
        document.getElementById("btnAddNewApplicantEducation").disabled = false;
    }

    if (document.getElementById("btnAddNewApplicantLanguage") != undefined) {
        document.getElementById("btnAddNewApplicantLanguage").className = "Button"
        document.getElementById("btnAddNewApplicantLanguage").disabled = false;
    }

    if (document.getElementById("lblEducationMessage") != undefined) {
        document.getElementById("lblEducationMessage").innerHTML = "";
    }

    if (document.getElementById("lblLanguageMessage") != undefined) {
        document.getElementById("lblLanguageMessage").innerHTML = "";
    }

    //Set new disable UI after save method

    disabledItems = xmlNodeText(xml.childNodes[3]);
    hiddenItems = xmlNodeText(xml.childNodes[4]);

    if (disabledItems != "") {
        document.getElementById(hdnDisabledClientControls).value += (document.getElementById(hdnDisabledClientControls).value == "" ? "" : ",") + disabledItems;
        CheckDisabledClientControls();
    }
    if (hiddenItems != "") {
        document.getElementById(hdnHiddenClientControls).value += (document.getElementById(hdnHiddenClientControls).value == "" ? "" : ",") + hiddenItems;
        CheckHiddenClientControls();
    }

    var lastApperiance = xmlNodeText(xml.childNodes[5]);

    if (lastApperiance == "Hidden") {
        //Hide server control
        document.getElementById("divLastApperiance").style.display = "none";
        document.getElementById(txtLastApperianceClientId).className = "";
    }
    if (lastApperiance == "Disabled") {
        document.getElementById(txtLastApperianceClientId).disabled = true;
    }
    RefreshDatePickers();

    document.getElementById(hdnSavedChangesClientId).value = "True";
}

// Display light box with education properties (for editing or adding new)
function ShowApplicantEducationLightBox(civilEducationId) {

    var url = "AddPotencialApplicant_PersonDetails.aspx?AjaxMethod=JSGetApplicantEducationLightBoxContent";
    var params = "";
    if (civilEducationId != 0) // gets current values if editing civil education
    {
        params += "CivilEducationId=" + civilEducationId;
        params += "&isRegistred=" + isRegistred;
    }
    else {
        params += "isRegistred=" + isRegistred;

    }

    function response_handler(xml) {
        var lightBoxHTML = xmlValue(xml, "lightBoxHTML");
        document.getElementById("ApplicantEducationLightBox").innerHTML = lightBoxHTML;

        if (lightBoxHTML != "") {
            document.getElementById("hdnCivilEducationID").value = civilEducationId; // setting civil education ID(0 - if new applicant education)
        }

        RefreshUIItems(xml);

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("ApplicantEducationLightBox").style.display = "";
        CenterLightBox("ApplicantEducationLightBox");

        if (document.getElementById("lblEducationMessage") != undefined) {
            document.getElementById("lblEducationMessage").innerHTML = "";
        }
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}


// Close the light box and refresh applicant educations table
function HideApplicantEducationLightBox() {
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("ApplicantEducationLightBox").style.display = "none";
}

// Validate applicant education properties in the light box and generates appropriate error messages, if needed
function ValidateApplicantEducation() {
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
function SaveApplicantEducation() {
    var lightBoxMessage = document.getElementById("spanEducationLightBoxMessage");

    if (ValidateApplicantEducation()) {
        var civilEducationId = document.getElementById("hdnCivilEducationID").value;
        
        var url = "AddPotencialApplicant_PersonDetails.aspx?AjaxMethod=JSSaveApplicantEducation&PersonId=" + document.getElementById(hdnPersonIDClientID).value;

        var params = "CivilEducationId=" + civilEducationId +
                     "&PersonEducationCode=" + document.getElementById("ddCivilEduPersonEducation").value +
                     "&PersonSchoolSubjectCode=" + document.getElementById("hdnSchoolSubjectCode").value +
                     "&GraduateYear=" + document.getElementById("txtCivilEduGraduateYear").value +
                     "&LearningMethodKey=" + document.getElementById("ddCivilEduLearningMethod").value +
                     "&PotencialApplicantId=" + document.getElementById(hdnPotencialApplicantClientId).value;
        params += "&MilitaryDepartmentId=" + document.getElementById(hdnMilitaryDepartmentIDClientID).value;

        function response_handler(xml) {
            var hideDialog = true;
            var result = xmlNodeText(xml.childNodes[0]);
            if (result == "OK") {
                lightBoxMessage.innerHTML = "";
                lightBoxMessage.style.display = "";
                hideDialog = false;
            }
            else if (result == "ERROR")
                lightBoxMessage.value = "Образованието не е добавено";
            else {
                document.getElementById("divEducation").innerHTML = result;
                if (document.getElementById("lblLanguageMessage") != undefined) {
                    document.getElementById("lblLanguageMessage").innerHTML = "";
                }

            }
            document.getElementById("lblErrorMsg").innerHTML = "";


            if (hideDialog) {
                HideApplicantEducationLightBox();
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
    else {
        lightBoxMessage.style.display = "";
    }
}

function DeleteApplicantEducation(civilEducationId) {
    YesNoDialog("Желаете ли да изтриете образованието?", ConfirmYes, null);

    function ConfirmYes() {
        var url = "AddPotencialApplicant_PersonDetails.aspx?AjaxMethod=JSDeleteApplicantEducation";
        var params = "";
        params += "SelectedTabId=btnTabEducation";
        params += "&PersonId=" + document.getElementById(hdnPersonIDClientID).value;
        params += "&CivilEducationId=" + civilEducationId;
        params += "&MilitaryDepartmentId=" + document.getElementById(hdnMilitaryDepartmentIDClientID).value;
        params += "&PotencialApplicantId=" + document.getElementById(hdnPotencialApplicantClientId).value;

        function response_handler(xml) {
            document.getElementById("divEducation").innerHTML = xmlNodeText(xml.childNodes[0]);
            if (document.getElementById("lblLanguageMessage") != undefined) {
                document.getElementById("lblLanguageMessage").innerHTML = "";
            }
            document.getElementById("lblErrorMsg").innerHTML = "";
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}



// Display light box with language properties (for editing or adding new)
function ShowApplicantLanguageLightBox(foreignLanguageId) {
    var url = "AddPotencialApplicant_PersonDetails.aspx?AjaxMethod=JSGetApplicantLanguageLightBoxContent";
    var params = "";
    if (foreignLanguageId != 0) // gets current values if editing applicant language
    {
        params += "ForeignLanguageId=" + foreignLanguageId;
        params += "&PersonId=" + document.getElementById(hdnPersonIDClientID).value;
        params += "&isRegistred=" + isRegistred;
    }
    else {
        params = "PersonId=" + document.getElementById(hdnPersonIDClientID).value;
        params += "&isRegistred=" + isRegistred;
    }

    function response_handler(xml) {
        var lightBoxHTML = xmlValue(xml, "lightBoxHTML");
        document.getElementById("ApplicantLanguageLightBox").innerHTML = lightBoxHTML;

        if (lightBoxHTML != "") {
            document.getElementById("hdnForeignLanguageID").value = foreignLanguageId; // setting foreign language ID(0 - if new applicant language)
        }

        RefreshDatePickers();
        RefreshUIItems(xml);

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("ApplicantLanguageLightBox").style.display = "";
        CenterLightBox("ApplicantLanguageLightBox");

        if (document.getElementById("lblLanguageMessage") != undefined) {
            document.getElementById("lblLanguageMessage").innerHTML = "";
        }
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}


// Close the light box and refresh applicant languages table
function HideApplicantLanguageLightBox() {
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("ApplicantLanguageLightBox").style.display = "none";
}

// Validate applicant language properties in the light box and generates appropriate error messages, if needed
function ValidateApplicantLanguage() {

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
function SaveApplicantLanguage() {
    var lightBoxMessage = document.getElementById("spanLanguageLightBoxMessage");

    if (ValidateApplicantLanguage()) {
        var url = "AddPotencialApplicant_PersonDetails.aspx?AjaxMethod=JSSaveApplicantLanguage";

        var params = "PersonId=" + document.getElementById(hdnPersonIDClientID).value +
                     "&ForeignLanguageId=" + document.getElementById("hdnForeignLanguageID").value +
                     "&LanguageCode=" + document.getElementById("ddPersonLangEduForeignLanguagePersonLanguage").value +
                     "&LanguageLevelOfKnowledgeKey=" + document.getElementById("ddPersonLangEduForeignLanguageLanguageLevel").value +
        //           "&LanguageStanAg=" + document.getElementById("txtPersonLangEduForeignLanguageSTANAG").value +
                     "&LanguageFormOfKnowledgeKey=" + document.getElementById("ddPersonLangEduForeignLanguageLanguageForm").value +
                     "&LanguageDiplomaKey=" + document.getElementById("ddPersonLangEduForeignLanguageLanguageDiploma").value +
                     "&LanguageVacAnn=" + document.getElementById("txtPersonLangEduForeignLanguageLanguageVacAnn").value +
                     "&LanguageDateWhen=" + document.getElementById("txtPersonLangEduForeignLanguageLanguageDateWhen").value +
                     "&PotencialApplicantId=" + document.getElementById(hdnPotencialApplicantClientId).value;

        params += "&MilitaryDepartmentId=" + document.getElementById(hdnMilitaryDepartmentIDClientID).value;

        function response_handler(xml) {
            var hideDialog = true;
            var result = xmlNodeText(xml.childNodes[0]);
            if (result == "OK") {
                lightBoxMessage.innerHTML = "";
                lightBoxMessage.style.display = "";
                hideDialog = false;
            }
            else if (result == "ERROR")
                lightBoxMessage.value = "Езиковата подготовка не е добавена";
            else {
                document.getElementById("divLanguage").innerHTML = result;
                if (document.getElementById("lblEducationMessage") != undefined) {
                    document.getElementById("lblEducationMessage").innerHTML = "";
                }
                document.getElementById("lblErrorMsg").innerHTML = "";
            }


            if (hideDialog)
                HideApplicantLanguageLightBox();
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
    else {
        lightBoxMessage.style.display = "";
    }
}

function DeleteApplicantLanguage(foreignLanguageId) {
    YesNoDialog("Желаете ли да изтриете езиковата подготовка?", ConfirmYes, null);

    function ConfirmYes() {
        var url = "AddPotencialApplicant_PersonDetails.aspx?AjaxMethod=JSDeleteApplicantLanguage";
        var params = "";
        params += "SelectedTabId=btnTabEducation";
        params += "&PersonId=" + document.getElementById(hdnPersonIDClientID).value;
        params += "&ForeignLanguageId=" + foreignLanguageId;
        params += "&MilitaryDepartmentId=" + document.getElementById(hdnMilitaryDepartmentIDClientID).value;
        params += "&PotencialApplicantId=" + document.getElementById(hdnPotencialApplicantClientId).value;

        function response_handler(xml) {
            document.getElementById("divLanguage").innerHTML = xmlNodeText(xml.childNodes[0]);
            if (document.getElementById("lblEducationMessage") != undefined) {
                document.getElementById("lblEducationMessage").innerHTML = "";
            }

            document.getElementById("lblErrorMsg").innerHTML = "";

        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

function CopyPresAddressToCurr() {
    document.getElementById("txtPermPostCode").value = document.getElementById("txtPresPostCode").value;
    CopyDropDown("ddPresRegion", "ddPermRegion");
    CopyDropDown("ddPresMunicipality", "ddPermMunicipality");
    CopyDropDown("ddPresCity", "ddPermCity");
    CopyDropDown("ddPresDistrict", "ddPermDistrict");
    document.getElementById("txtaPermAddress").value = document.getElementById("txtaPresAddress").value;
}

function CopyPermAddressToCurr() {
    document.getElementById("txtPresPostCode").value = document.getElementById("txtPermPostCode").value;
    CopyDropDown("ddPermRegion", "ddPresRegion");
    CopyDropDown("ddPermMunicipality", "ddPresMunicipality");
    CopyDropDown("ddPermCity", "ddPresCity");
    CopyDropDown("ddPermDistrict", "ddPresDistrict");
    document.getElementById("txtaPresAddress").value = document.getElementById("txtaPermAddress").value;
}

function CopyPresAddressToContact() {
    document.getElementById("txtContactPostCode").value = document.getElementById("txtPresPostCode").value;
    CopyDropDown("ddPresRegion", "ddContactRegion");
    CopyDropDown("ddPresMunicipality", "ddContactMunicipality");
    CopyDropDown("ddPresCity", "ddContactCity");
    CopyDropDown("ddPresDistrict", "ddContactDistrict");
    document.getElementById("txtaContactAddress").value = document.getElementById("txtaPresAddress").value;
}

function SelectShuttleControlOptions(shuttleControl) {
    var source = document.getElementById(shuttleControl + '_AvailableOptions');
    var target = document.getElementById(shuttleControl + '_AssignedOptions');

    while (source.options.selectedIndex >= 0) {
        var found = false;

        for (var j = 0; j < target.options.length; j++) {
            if (target.options[j].value == source.options[source.options.selectedIndex].value) {
                found = true;
                break;
            }
        }

        if (!found) {
            var newOption = new Option();
            newOption.text = source.options[source.options.selectedIndex].text;
            newOption.value = source.options[source.options.selectedIndex].value;
            newOption.title = newOption.text;

            target.options[target.length] = newOption;
            source.remove(source.options.selectedIndex);
        }
    }
}

function RemoveShuttleControlOptions(shuttleControl) {
    var source = document.getElementById(shuttleControl + '_AssignedOptions');
    var target = document.getElementById(shuttleControl + '_AvailableOptions');

    while (source.options.selectedIndex >= 0) {
        var newOption = new Option();
        newOption.text = source.options[source.options.selectedIndex].text;
        newOption.value = source.options[source.options.selectedIndex].value;
        newOption.title = newOption.text;

        target.options[target.length] = newOption;
        source.remove(source.options.selectedIndex);
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

function SetEventHandlers() {
    var hasMilitaryServiceRadios = document.querySelectorAll('input[type=radio][name="hasMilitarySrv"]');

    for (var i = 0; i < hasMilitaryServiceRadios.length; i++) {
        if (hasMilitaryServiceRadios[i].addEventListener) {
            hasMilitaryServiceRadios[i].addEventListener("click", HasMilitaryService_Change);
        }
        else {
            hasMilitaryServiceRadios[i].attachEvent("onclick", HasMilitaryService_Change);
        }
    }
}

function HasMilitaryService_Change(event) {
    var hasMilitarySrv1 = document.getElementById("hasMilitarySrv1");

    if (hasMilitarySrv1.checked) {
        document.getElementById("militaryTraining1").checked = true;
    }
}

function RepopulateMunicipality(regionId, ddMunicipalityId) {
    var url = "AddApplicant_PersonDetails.aspx?AjaxMethod=JSRepopulateMunicipality";
    var params = "";
    params += "RegionId=" + regionId;
    var myAJAX = new AJAX(url, true, params, RepopulateMunicipality_Callback);
    myAJAX.Call();

    function RepopulateMunicipality_Callback(xml) {
        ClearSelectList(document.getElementById(ddMunicipalityId), true);

        var municipalities = xml.getElementsByTagName("m");

        for (var i = 0; i < municipalities.length; i++) {
            var id = xmlValue(municipalities[i], "id");
            var name = xmlValue(municipalities[i], "name");

            AddToSelectList(document.getElementById(ddMunicipalityId), id, name);
        };

        if (ddMunicipalityId == "ddPermMunicipality")
            ddPermMunicipality_Changed();
        else if (ddMunicipalityId == "ddPresMunicipality")
            ddPresMunicipality_Changed();
        else if (ddMunicipalityId == "ddContactMunicipality")
            ddContactMunicipality_Changed();
    }
}

function RepopulateCity(municipalityId, ddCityId) {
    var url = "AddApplicant_PersonDetails.aspx?AjaxMethod=JSRepopulateCity";
    var params = "";
    params += "MunicipalityId=" + municipalityId;

    function RepopulateMunicipality_Callback(xml) {
        ClearSelectList(document.getElementById(ddCityId), true);

        var cities = xml.getElementsByTagName("c");

        for (var i = 0; i < cities.length; i++) {
            var id = xmlValue(cities[i], "id");
            var name = xmlValue(cities[i], "name");

            AddToSelectList(document.getElementById(ddCityId), id, name);
        };

        if (ddCityId == "ddPermCity")
            ddPermCity_Changed();
        else if (ddCityId == "ddPresCity")
            ddPresCity_Changed();
        else if (ddCityId == "ddContactCity")
            ddContactCity_Changed();
    }

    var myAJAX = new AJAX(url, true, params, RepopulateMunicipality_Callback);
    myAJAX.Call();
}

function RepopulatePostCode(cityId, txtPostCodeId) {
    var url = "AddApplicant_PersonDetails.aspx?AjaxMethod=JSRepopulatePostCode";
    var params = "";
    params += "CityId=" + cityId;

    function RepopulatePostCode_Callback(xml) {
        var postCode = xmlValue(xml, "postCode");

        document.getElementById(txtPostCodeId).value = postCode;
    }

    var myAJAX = new AJAX(url, true, params, RepopulatePostCode_Callback);
    myAJAX.Call();
}

function RepopulateRegionMunicipalityCity(postCode, ddRegionId, ddMunicipalityId, ddCityId) {
    var url = "AddApplicant_PersonDetails.aspx?AjaxMethod=JSRepopulateRegionMunicipalityCity";
    var params = "";
    params += "PostCode=" + postCode;

    function RepopulateRegionMunicipalityCity_Callback(xml) {
        var cityId = parseInt(xmlValue(xml, "cityId"));

        //Not found
        if (cityId == 0) {
            document.getElementById(ddRegionId).selectedIndex = 0;

            ClearSelectList(document.getElementById(ddMunicipalityId), false);
            ClearSelectList(document.getElementById(ddCityId), false);
        }
        else //found
        {
            var regionId = xmlValue(xml, "regionId");
            var municipalityId = xmlValue(xml, "municipalityId");

            document.getElementById(ddRegionId).value = regionId;

            ClearSelectList(document.getElementById(ddMunicipalityId), true);

            var municipalities = xml.getElementsByTagName("m");

            for (var i = 0; i < municipalities.length; i++) {
                var id = xmlValue(municipalities[i], "id");
                var name = xmlValue(municipalities[i], "name");

                AddToSelectList(document.getElementById(ddMunicipalityId), id, name);
            };

            document.getElementById(ddMunicipalityId).value = municipalityId;


            ClearSelectList(document.getElementById(ddCityId), true);

            var cities = xml.getElementsByTagName("c");

            for (var i = 0; i < cities.length; i++) {
                var id = xmlValue(cities[i], "id");
                var name = xmlValue(cities[i], "name");

                AddToSelectList(document.getElementById(ddCityId), id, name);
            };

            document.getElementById(ddCityId).value = cityId;
        }
    }

    var myAJAX = new AJAX(url, true, params, RepopulateRegionMunicipalityCity_Callback);
    myAJAX.Call();
}

//When changing the Perm Region then refresh the Perm Municipality and the Perm City
function ddPermRegion_Changed() {
    var ddPermRegion = document.getElementById("ddPermRegion");
    RepopulateMunicipality(ddPermRegion.value, "ddPermMunicipality");
}

//When changing the Perm Municipality then refresh the Perm City
function ddPermMunicipality_Changed() {
    var ddPermMunicipality = document.getElementById("ddPermMunicipality");
    RepopulateCity(ddPermMunicipality.value, "ddPermCity");
}

//When changing the Perm City then refresh the Perm Post Code
function ddPermCity_Changed() {
    var ddPermCity = document.getElementById("ddPermCity");
    RepopulatePostCodeAndDistrict(ddPermCity.value, "txtPermPostCode", "ddPermDistrict");
}

//When changing the PostCode then repopulate the Region, the Municipality and the City
function txtPermPostCode_Focus() {
    var txtPermPostCode = document.getElementById("txtPermPostCode");
    txtPermPostCode.setAttribute("oldvalue", txtPermPostCode.value);
}

function txtPermPostCode_Blur() {
    var txtPermPostCode = document.getElementById("txtPermPostCode");

    if (txtPermPostCode.value != txtPermPostCode.getAttribute("oldvalue")) {
        RepopulateRegionMunicipalityCityDistrict(txtPermPostCode.value, "ddPermRegion", "ddPermMunicipality", "ddPermCity", "ddPermDistrict");
    }
}

function RepopulatePostCodeAndDistrict(cityId, txtPostCodeId, ddDistrictsId) {
    var url = "AddApplicant_PersonDetails.aspx?AjaxMethod=JSRepopulatePostCodeAndDistrict";
    var params = "";
    params += "CityId=" + cityId;

    function RepopulatePostCodeAndDistrict_Callback(xml) {
        var cityPostCode = xmlValue(xml, "cityPostCode");

        document.getElementById(txtPostCodeId).value = cityPostCode;

        ClearSelectList(document.getElementById(ddDistrictsId), true);

        var districts = xml.getElementsByTagName("d");

        for (var i = 0; i < districts.length; i++) {
            var id = xmlValue(districts[i], "id");
            var name = xmlValue(districts[i], "name");

            AddToSelectList(document.getElementById(ddDistrictsId), id, name);
        };

        if (ddDistrictsId == "ddPermDistrict")
            ddPermDistrict_Changed();
        else if (ddDistrictsId == "ddPresDistrict")
            ddPresDistrict_Changed();
        else if (ddDistrictsId == "ddContactDistrict")
            ddContactDistrict_Changed();
    }

    var myAJAX = new AJAX(url, true, params, RepopulatePostCodeAndDistrict_Callback);
    myAJAX.Call();
}


//When chaning the District then repopulate the PostCode
function ddPermDistrict_Changed() {
    var ddPermDistrict = document.getElementById("ddPermDistrict");
    var ddPermCity = document.getElementById("ddPermCity");
    RepopulateDistrictPostCode(ddPermDistrict.value, ddPermCity.value, "txtPermPostCode");
}

function RepopulateDistrictPostCode(districtId, cityId, txtPostCodeId) {
    var url = "AddApplicant_PersonDetails.aspx?AjaxMethod=JSRepopulateDistrictPostCode";
    var params = "";
    params += "DistrictId=" + districtId;
    params += "&";
    params += "CityId=" + cityId;

    function RepopulateDistrictPostCode_Callback(xml) {
        var districtPostCode = xmlValue(xml, "districtPostCode");

        if (districtPostCode != "")
            document.getElementById(txtPostCodeId).value = districtPostCode;
    }

    var myAJAX = new AJAX(url, true, params, RepopulateDistrictPostCode_Callback);
    myAJAX.Call();
}


function RepopulateRegionMunicipalityCityDistrict(postCode, ddRegionId, ddMunicipalityId, ddCityId, ddDistrictId) {
    var url = "AddApplicant_PersonDetails.aspx?AjaxMethod=JSRepopulateRegionMunicipalityCityDistrict";
    var params = "";
    params += "PostCode=" + postCode;

    function RepopulateRegionMunicipalityCity_Callback(xml) {
        var cityId = parseInt(xmlValue(xml, "cityId"));

        //Found
        if (cityId != 0) {
            var regionId = xmlValue(xml, "regionId");
            var municipalityId = xmlValue(xml, "municipalityId");
            var districtId = xmlValue(xml, "districtId");

            document.getElementById(ddRegionId).value = regionId;

            ClearSelectList(document.getElementById(ddMunicipalityId), true);

            var municipalities = xml.getElementsByTagName("m");

            for (var i = 0; i < municipalities.length; i++) {
                var id = xmlValue(municipalities[i], "id");
                var name = xmlValue(municipalities[i], "name");

                AddToSelectList(document.getElementById(ddMunicipalityId), id, name);
            };

            document.getElementById(ddMunicipalityId).value = municipalityId;


            ClearSelectList(document.getElementById(ddCityId), true);

            var cities = xml.getElementsByTagName("c");

            for (var i = 0; i < cities.length; i++) {
                var id = xmlValue(cities[i], "id");
                var name = xmlValue(cities[i], "name");

                AddToSelectList(document.getElementById(ddCityId), id, name);
            };

            document.getElementById(ddCityId).value = cityId;


            ClearSelectList(document.getElementById(ddDistrictId), true);

            var districts = xml.getElementsByTagName("d");

            for (var i = 0; i < districts.length; i++) {
                var id = xmlValue(districts[i], "id");
                var name = xmlValue(districts[i], "name");

                AddToSelectList(document.getElementById(ddDistrictId), id, name);
            };

            if (parseInt(districtId) > 0)
                document.getElementById(ddDistrictId).value = districtId;
        }
    }

    var myAJAX = new AJAX(url, true, params, RepopulateRegionMunicipalityCity_Callback);
    myAJAX.Call();
}

//When changing the Pres Region then refresh the Pres Municipality and the Pres City
function ddPresRegion_Changed() {
    var ddPresRegion = document.getElementById("ddPresRegion");
    RepopulateMunicipality(ddPresRegion.value, "ddPresMunicipality");
}

//When changing the Perm Municipality then refresh the Perm City
function ddPresMunicipality_Changed() {
    var ddPresMunicipality = document.getElementById("ddPresMunicipality");
    RepopulateCity(ddPresMunicipality.value, "ddPresCity");
}

//When changing the Perm City then refresh the Perm Post Code
function ddPresCity_Changed() {
    var ddPresCity = document.getElementById("ddPresCity");
    RepopulatePostCodeAndDistrict(ddPresCity.value, "txtPresPostCode", "ddPresDistrict");
}

//When changing the PostCode then repopulate the Region, the Municipality and the City
function txtPresPostCode_Focus() {
    var txtPresPostCode = document.getElementById("txtPresPostCode");
    txtPresPostCode.setAttribute("oldvalue", txtPresPostCode.value);
}

function txtPresPostCode_Blur() {
    var txtPresPostCode = document.getElementById("txtPresPostCode");

    if (txtPresPostCode.value != txtPresPostCode.getAttribute("oldvalue")) {
        RepopulateRegionMunicipalityCityDistrict(txtPresPostCode.value, "ddPresRegion", "ddPresMunicipality", "ddPresCity", "ddPresDistrict");
    }
}

//When chaning the District then repopulate the PostCode
function ddPresDistrict_Changed() {
    var ddPresDistrict = document.getElementById("ddPresDistrict");
    var ddPresCity = document.getElementById("ddPresCity");
    RepopulateDistrictPostCode(ddPresDistrict.value, ddPresCity.value, "txtPresPostCode");
}

//When changing the Contact Region then refresh the Contact Municipality and the Contact City
function ddContactRegion_Changed() {
    var ddContactRegion = document.getElementById("ddContactRegion");
    RepopulateMunicipality(ddContactRegion.value, "ddContactMunicipality");
}

//When changing the Perm Municipality then refresh the Perm City
function ddContactMunicipality_Changed() {
    var ddContactMunicipality = document.getElementById("ddContactMunicipality");
    RepopulateCity(ddContactMunicipality.value, "ddContactCity");
}

//When changing the Perm City then refresh the Perm Post Code
function ddContactCity_Changed() {
    var ddContactCity = document.getElementById("ddContactCity");
    RepopulatePostCodeAndDistrict(ddContactCity.value, "txtContactPostCode", "ddContactDistrict");
}

//When changing the PostCode then repopulate the Region, the Municipality and the City
function txtContactPostCode_Focus() {
    var txtContactPostCode = document.getElementById("txtContactPostCode");
    txtContactPostCode.setAttribute("oldvalue", txtContactPostCode.value);
}

function txtContactPostCode_Blur() {
    var txtContactPostCode = document.getElementById("txtContactPostCode");

    if (txtContactPostCode.value != txtContactPostCode.getAttribute("oldvalue")) {
        RepopulateRegionMunicipalityCityDistrict(txtContactPostCode.value, "ddContactRegion", "ddContactMunicipality", "ddContactCity", "ddContactDistrict");
    }
}

//When chaning the District then repopulate the PostCode
function ddContactDistrict_Changed() {
    var ddContactDistrict = document.getElementById("ddContactDistrict");
    var ddContactCity = document.getElementById("ddContactCity");
    RepopulateDistrictPostCode(ddContactDistrict.value, ddContactCity.value, "txtContactPostCode");
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