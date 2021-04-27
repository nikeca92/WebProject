window.addEventListener ? window.addEventListener('load', PageLoad, false) : window.attachEvent('onload', PageLoad);
var goToShow = false;
var isNewPerson = true;

//Call this function when the page is loaded
function PageLoad()
{
    LoadPickLists();
    LoadPersonDetails();
    SetEventHandlers();

    SetClientTextAreaMaxLength("txtaPermAddress", "500");
    SetClientTextAreaMaxLength("txtaPresAddress", "1500");
    SetClientTextAreaMaxLength("txtaContactAddress", "1500");    
}

function LoadPickLists()
{
    var configPickListDrvLicCategories =
    {
        width: 175,
        allLabel: "<Всички>"
    }

    categories = document.getElementById(hdnDrvLicCategoriesClientID).value;
    categories = eval(categories);
    PickListUtil.AddPickList("pickListDrvLicCategories", categories, "tdPickListDrvLicCategories", configPickListDrvLicCategories);
}

//This function load the person's details
function LoadPersonDetails()
{
    var url = "AddApplicant_PersonDetails.aspx?AjaxMethod=JSLoadPersonDetails";
    var params = "";
    params += "IdentNumber=" + document.getElementById(hdnIdentNumberClientID).value;
    params += "&MilitaryDepartmentId=" + document.getElementById(hdnMilitaryDepartmentIDClientID).value;
    var myAJAX = new AJAX(url, true, params, LoadPersonDetails_Callback);
    myAJAX.Call();
}

function LoadPersonDetails_Callback(xml)
{
    document.getElementById("lblIdentNumberValue").innerHTML = document.getElementById(hdnIdentNumberClientID).value;
    var person = xml.getElementsByTagName("person")[0];

    var personId = xmlValue(person, "personId");
    if (personId != 0)
    {
        isNewPerson = false;
    }
    var firstName = xmlValue(person, "firstName");
    var lastName = xmlValue(person, "lastName");
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
    var birthCountryId = xmlValue(person, "birthCountryId");
    var birthCityId = xmlValue(person, "birthCityId");
    var birthCityIfAbroad = xmlValue(person, "birthCityIfAbroad");
    
    var medCertTableHTML = xmlValue(person, "medCertTableHTML");
    var medCertLightBoxHTML = xmlValue(person, "medCertLightBoxHTML");

    var psychCertTableHTML = xmlValue(person, "psychCertTableHTML");
    var psychCertLightBoxHTML = xmlValue(person, "psychCertLightBoxHTML");

    SetStatusLightBox(xml);
    
    document.getElementById(hdnPersonIDClientID).value = personId;

    document.getElementById("txtFirstName").value = firstName;
    document.getElementById("txtLastName").value = lastName;
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
    document.getElementById("hdnBirthCountryId").value = birthCountryId;
    document.getElementById("hdnBirthCityId").value = birthCityId;
    document.getElementById("hdnBirthCityIfAbroad").value = birthCityIfAbroad;

    document.getElementById("divMedCertTable").innerHTML = medCertTableHTML;
    document.getElementById("divMedCertLightBoxContainer").innerHTML = medCertLightBoxHTML;

    document.getElementById("divPsychCertTable").innerHTML = psychCertTableHTML;
    document.getElementById("divPsychCertLightBoxContainer").innerHTML = psychCertLightBoxHTML;

    disabledItems = xmlValue(xml, "listDisabledControls");
    hiddenItems = xmlValue(xml, "listHiddenControls");

    if (disabledItems != "")
    {
        document.getElementById(hdnDisabledClientControls).value += (document.getElementById(hdnDisabledClientControls).value == "" ? "" : ",") + disabledItems;
        CheckDisabledClientControls();
    }
    if (hiddenItems != "")
    {
        document.getElementById(hdnHiddenClientControls).value += (document.getElementById(hdnHiddenClientControls).value == "" ? "" : ",") + hiddenItems;
        CheckHiddenClientControls();
    }

    LoadOriginalValues();
    ShowContent();
}

//This function displays the content div and hides the "loading" div
function ShowContent()
{
    document.getElementById("loadingDiv").style.display = "none";
    document.getElementById("contentDiv").style.display = "";
}

function Save_Click()
{
    SaveData();

    return false;
}

var ValidationMessage = "";
function ValidateData()
{
    ValidationMessage = "";

    var notValidFields = new Array();

    //Validate FirstName
    if (document.getElementById("txtFirstName").value == "")
    {
        if (document.getElementById("txtFirstName").disabled == true || document.getElementById("txtFirstName").style.display == "none")
        {
            res = false;
            notValidFields.push("Име и презиме");
        }
        else
        {
            ValidationMessage += GetErrorMessageMandatory("Име и презиме") + "</br>";
        }
    }

    //Validate LastName
    if (document.getElementById("txtLastName").value == "")
    {
        if (document.getElementById("txtLastName").disabled == true || document.getElementById("txtLastName").style.display == "none")
        {
            res = false;
            notValidFields.push("Фамилия");
        }
        else
        {
            ValidationMessage += GetErrorMessageMandatory("Фамилия") + "</br>";
        }
    }

    var selectedItem = GetSelectedItemId(document.getElementById("ddPermCity"));
    
    //Validate PermCity
    if (selectedItem == -1)
    {
        if (document.getElementById("ddPermCity").disabled == true || document.getElementById("ddPermCity").style.display == "none")
        {
            res = false;
            notValidFields.push("Постоянен адрес - Населено място");
        }
        else
        {
            ValidationMessage += GetErrorMessageMandatory("Постоянен адрес - Населено място") + "</br>";
        }
    }

    selectedItem = GetSelectedItemId(document.getElementById("ddPresCity"));
    
    //Validate PresCity
    if (selectedItem == -1)
    {
        if (document.getElementById("ddPresCity").disabled == true || document.getElementById("ddPresCity").style.display == "none")
        {
            res = false;
            notValidFields.push("Настоящ адрес - Населено място");
        }
        else
        {
            ValidationMessage += GetErrorMessageMandatory("Настоящ адрес - Населено място") + "</br>";
        }
    }

    if (document.getElementById("txtIDCardIssueDate").value.Trim() != "") {
        if (!IsValidDate(document.getElementById("txtIDCardIssueDate").value)) {
            ValidationMessage += GetErrorMessageDate("Лична карта издадена на") + "</br>";
        }
    }

    //Type Value Fields
    if (document.getElementById("txtHomePhone").value != "")
    {
        if (!isInt(document.getElementById("txtHomePhone").value))
        {
            ValidationMessage += GetErrorMessageNumber("Домашен телефон") + "</br>";
        }
    }
    
    var notValidFieldsCount = notValidFields.length;

    if (notValidFieldsCount > 0)
    {
        var noRightsMessage = GetErrorMessageNoRights(notValidFields);
        ValidationMessage += noRightsMessage + "<br />";
    }

    return (ValidationMessage == "")
}

function SaveData()
{
    if (ValidateData())
    {
        var url = "AddApplicant_PersonDetails.aspx?AjaxMethod=JSSavePersonDetails";
        var params = "";
        params += "PersonID=" + document.getElementById(hdnPersonIDClientID).value;
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
        params += "&BirthCountryId=" + document.getElementById("hdnBirthCountryId").value;
        params += "&BirthCityId=" + document.getElementById("hdnBirthCityId").value;
        params += "&BirthCityIfAbroad=" + document.getElementById("hdnBirthCityIfAbroad").value;

        var myAJAX = new AJAX(url, true, params, SaveData_Callback);
        myAJAX.Call();
    }
    else
    {
        document.getElementById("lblErrorMsg").innerHTML = ValidationMessage;
        document.getElementById("lblErrorMsg").className = "ErrorText";
    }
}

function SaveData_Callback(xml)
{
    var personId = xmlValue(xml, "personId");
    document.getElementById(hdnPersonIDClientID).value = personId;

    document.getElementById(btnSaveHelperClientID).click();
}

function CopyPresAddressToCurr()
{
    document.getElementById("txtPermPostCode").value = document.getElementById("txtPresPostCode").value;
    CopyDropDown("ddPresRegion", "ddPermRegion");
    CopyDropDown("ddPresMunicipality", "ddPermMunicipality");
    CopyDropDown("ddPresCity", "ddPermCity");
    CopyDropDown("ddPresDistrict", "ddPermDistrict");
    document.getElementById("txtaPermAddress").value = document.getElementById("txtaPresAddress").value;
}

function CopyPermAddressToCurr()
{
    document.getElementById("txtPresPostCode").value = document.getElementById("txtPermPostCode").value;
    CopyDropDown("ddPermRegion", "ddPresRegion");
    CopyDropDown("ddPermMunicipality", "ddPresMunicipality");
    CopyDropDown("ddPermCity", "ddPresCity");
    CopyDropDown("ddPermDistrict", "ddPresDistrict");
    document.getElementById("txtaPresAddress").value = document.getElementById("txtaPermAddress").value;
}

function CopyPresAddressToContact() 
{
    document.getElementById("txtContactPostCode").value = document.getElementById("txtPresPostCode").value;
    CopyDropDown("ddPresRegion", "ddContactRegion");
    CopyDropDown("ddPresMunicipality", "ddContactMunicipality");
    CopyDropDown("ddPresCity", "ddContactCity");
    CopyDropDown("ddPresDistrict", "ddContactDistrict");
    document.getElementById("txtaContactAddress").value = document.getElementById("txtaPresAddress").value;
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


//Open the light-box for adding a new record in the Med Cert table
function NewMedCert() {
    ShowAddEditMedCertLightBox(0);
}

//Open the light-box for editing a record in the Med Cert table
function EditMedCert(medCertID) {
    ShowAddEditMedCertLightBox(medCertID);
}

function ShowAddEditMedCertLightBox(medCertID) {
    //Set DateTime Pickers
    RefreshDatePickers();

    document.getElementById("hdnMedCertID").value = medCertID;

    //New record
    if (medCertID == 0) {
        document.getElementById("lblAddEditMedCertTitle").innerHTML = "Въвеждане на медицинско освидетелстване";

        document.getElementById("txtMedCertDate").value = "";
        document.getElementById("txtMedCertProtNum").value = "";
        document.getElementById("ddMedCertConclusion").value = optionChooseOneValue;
        document.getElementById("ddMedCertMedRubric").value = optionChooseOneValue;
        document.getElementById("txtMedCertExpirationDate").value = "";

        // clean message label in the light box and hide it
        document.getElementById("spanAddEditMedCertLightBoxMsg").style.display = "none";
        document.getElementById("spanAddEditMedCertLightBoxMsg").innerHTML = "";

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("divMedCertLightBox").style.display = "";
        CenterLightBox("divMedCertLightBox");
    }
    else //Edit record
    {
        document.getElementById("lblAddEditMedCertTitle").innerHTML = "Редактиране на медицинско освидетелстване";

        var url = "AddApplicant_PersonDetails.aspx?AjaxMethod=JSLoadMedCert";

        var params = "MedCertID=" + medCertID;

        function response_handler(xml) {
            document.getElementById("txtMedCertDate").value = xmlValue(xml, "medCertDate");
            document.getElementById("txtMedCertProtNum").value = xmlValue(xml, "protNum");
            document.getElementById("ddMedCertConclusion").value = xmlValue(xml, "conclusionID");
            document.getElementById("ddMedCertMedRubric").value = xmlValue(xml, "medRubricID");
            document.getElementById("txtMedCertExpirationDate").value = xmlValue(xml, "medCertExpirationDate");

            // clean message label in the light box and hide it
            document.getElementById("spanAddEditMedCertLightBoxMsg").style.display = "none";
            document.getElementById("spanAddEditMedCertLightBoxMsg").innerHTML = "";

            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("divMedCertLightBox").style.display = "";
            CenterLightBox("divMedCertLightBox");
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Close the light-box
function HideAddEditMedCertLightBox() {
    document.getElementById("txtMedCertDate").value = "";
    document.getElementById("txtMedCertProtNum").value = "";
    document.getElementById("ddMedCertConclusion").value = optionChooseOneValue;
    document.getElementById("ddMedCertMedRubric").value = optionChooseOneValue;
    document.getElementById("txtMedCertExpirationDate").value = "";

    document.getElementById("HidePage").style.display = "none";
    document.getElementById("divMedCertLightBox").style.display = "none";
}

//Save Add/Edit Med Cert
function SaveAddEditMedCertLightBox() {
    if (ValidateAddEditMedCertLightBox()) {
        var url = "AddApplicant_PersonDetails.aspx?AjaxMethod=JSSaveMedCert";

        var params = "MedCertID=" + document.getElementById("hdnMedCertID").value +
                     "&PersonId=" + document.getElementById(hdnPersonIDClientID).value +
                     "&MedCertDate=" + document.getElementById("txtMedCertDate").value +
                     "&MedCertProtNum=" + custEncodeURI(document.getElementById("txtMedCertProtNum").value) +
                     "&MedCertConclusionId=" + document.getElementById("ddMedCertConclusion").value +
                     "&MedCertMedRubricID=" + document.getElementById("ddMedCertMedRubric").value +
                     "&MedCertExpirationDate=" + document.getElementById("txtMedCertExpirationDate").value;

        function response_handler(xml) {
            var status = xmlValue(xml, "status");

            if (status == "OK") {
                var refreshedMedCertTable = xmlValue(xml, "refreshedMedCertTable");
                document.getElementById("divMedCertTable").innerHTML = refreshedMedCertTable;

                document.getElementById("spanMedCertSectionMsg").className = "SuccessText";
                document.getElementById("spanMedCertSectionMsg").innerHTML = document.getElementById("hdnMedCertID").value == "0" ? "Медицинското освидетелстване е добавено успешно" : "Медицинското освидетелстване е редактирано успешно";

                HideAddEditMedCertLightBox();
            }
            else {
                document.getElementById("spanAddEditMedCertLightBoxMsg").className = "ErrorText";
                document.getElementById("spanAddEditMedCertLightBoxMsg").innerHTML = status;
                document.getElementById("spanAddEditMedCertLightBoxMsg").style.display = "";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Client validation of AddEditMedCertLightBox light-box
function ValidateAddEditMedCertLightBox() {
    var res = true;

    var lblMessage = document.getElementById("spanAddEditMedCertLightBoxMsg");
    lblMessage.innerHTML = "";

    var notValidFields = new Array();

    var txtMedCertDate = document.getElementById("txtMedCertDate");
    if (TrimString(txtMedCertDate.value) != "" && !IsValidDate(txtMedCertDate.value)) {
        res = false;
        lblMessage.innerHTML += GetErrorMessageDate("Комисия от дата") + "<br />";
    }

    var txtMedCertExpirationDate = document.getElementById("txtMedCertExpirationDate");
    if (TrimString(txtMedCertExpirationDate.value) != "" && !IsValidDate(txtMedCertExpirationDate.value)) {
        res = false;
        lblMessage.innerHTML += GetErrorMessageDate("Дата на валидност") + "<br />";
    }

    var notValidFieldsCount = notValidFields.length;

    if (notValidFieldsCount > 0) {
        var noRightsMessage = GetErrorMessageNoRights(notValidFields);
        lblMessage.innerHTML += "<br />" + noRightsMessage;
        res = false;
    }

    if (res) {
        lblMessage.className = "SuccessText";
    }
    else {
        lblMessage.className = "ErrorText";
        lblMessage.style.display = "";
    }

    return res;
}

// Delete a particular Med Cert record
function DeleteMedCert(medCertID) {
    YesNoDialog("Желаете ли да изтриете медицинското освидетелстване?", ConfirmYes, null);

    function ConfirmYes() {
        var url = "AddApplicant_PersonDetails.aspx?AjaxMethod=JSDeleteMedCert";

        var params = "MedCertID=" + medCertID +
                 "&PersonId=" + document.getElementById(hdnPersonIDClientID).value;

        function response_handler(xml) {
            var status = xmlValue(xml, "status")

            if (status == "OK") {
                var refreshedMedCertTable = xmlValue(xml, "refreshedMedCertTable");

                document.getElementById("divMedCertTable").innerHTML = refreshedMedCertTable;

                document.getElementById("spanMedCertSectionMsg").className = "SuccessText";
                document.getElementById("spanMedCertSectionMsg").innerHTML = "Медицинското освидетелстване е изтрито успешно";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}


//Open the light-box for adding a new record in the Psych Cert table
function NewPsychCert() {
    ShowAddEditPsychCertLightBox(0);
}

//Open the light-box for editing a record in the Psych Cert table
function EditPsychCert(psychCertID) {
    ShowAddEditPsychCertLightBox(psychCertID);
}

function ShowAddEditPsychCertLightBox(psychCertID) {
    //Set DateTime Pickers
    RefreshDatePickers();

    document.getElementById("hdnPsychCertID").value = psychCertID;

    //New record
    if (psychCertID == 0) {
        document.getElementById("lblAddEditPsychCertTitle").innerHTML = "Въвеждане на психологическа пригодност";

        document.getElementById("txtPsychCertDate").value = "";
        document.getElementById("txtPsychCertProtNum").value = "";
        document.getElementById("ddPsychCertConclusion").value = optionChooseOneValue;
        document.getElementById("txtPsychCertExpirationDate").value = "";

        // clean message label in the light box and hide it
        document.getElementById("spanAddEditPsychCertLightBoxMsg").style.display = "none";
        document.getElementById("spanAddEditPsychCertLightBoxMsg").innerHTML = "";

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("divPsychCertLightBox").style.display = "";
        CenterLightBox("divPsychCertLightBox");
    }
    else //Edit record
    {
        document.getElementById("lblAddEditPsychCertTitle").innerHTML = "Редактиране на психологическа пригодност";

        var url = "AddApplicant_PersonDetails.aspx?AjaxMethod=JSLoadPsychCert";

        var params = "PsychCertID=" + psychCertID;

        function response_handler(xml) {
            document.getElementById("txtPsychCertDate").value = xmlValue(xml, "psychCertDate");
            document.getElementById("txtPsychCertProtNum").value = xmlValue(xml, "protNum");
            document.getElementById("ddPsychCertConclusion").value = xmlValue(xml, "conclusionID");
            document.getElementById("txtPsychCertExpirationDate").value = xmlValue(xml, "psychCertExpirationDate");

            // clean message label in the light box and hide it
            document.getElementById("spanAddEditPsychCertLightBoxMsg").style.display = "none";
            document.getElementById("spanAddEditPsychCertLightBoxMsg").innerHTML = "";

            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("divPsychCertLightBox").style.display = "";
            CenterLightBox("divPsychCertLightBox");
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Close the light-box
function HideAddEditPsychCertLightBox() {
    document.getElementById("txtPsychCertDate").value = "";
    document.getElementById("txtPsychCertProtNum").value = "";
    document.getElementById("ddPsychCertConclusion").value = optionChooseOneValue;
    document.getElementById("txtPsychCertExpirationDate").value = "";

    document.getElementById("HidePage").style.display = "none";
    document.getElementById("divPsychCertLightBox").style.display = "none";
}

//Save Add/Edit Psych Cert
function SaveAddEditPsychCertLightBox() {
    if (ValidateAddEditPsychCertLightBox()) {
        var url = "AddApplicant_PersonDetails.aspx?AjaxMethod=JSSavePsychCert";

        var params = "PsychCertID=" + document.getElementById("hdnPsychCertID").value +
                     "&PersonId=" + document.getElementById(hdnPersonIDClientID).value +
                     "&PsychCertDate=" + document.getElementById("txtPsychCertDate").value +
                     "&PsychCertProtNum=" + custEncodeURI(document.getElementById("txtPsychCertProtNum").value) +
                     "&PsychCertConclusionId=" + document.getElementById("ddPsychCertConclusion").value +
                     "&PsychCertExpirationDate=" + document.getElementById("txtPsychCertExpirationDate").value;

        function response_handler(xml) {
            var status = xmlValue(xml, "status");

            if (status == "OK") {
                var refreshedPsychCertTable = xmlValue(xml, "refreshedPsychCertTable");
                document.getElementById("divPsychCertTable").innerHTML = refreshedPsychCertTable;

                document.getElementById("spanPsychCertSectionMsg").className = "SuccessText";
                document.getElementById("spanPsychCertSectionMsg").innerHTML = document.getElementById("hdnPsychCertID").value == "0" ? "Психологическата пригодност е добавена успешно" : "Психологическата пригодност е редактиранa успешно";

                HideAddEditPsychCertLightBox();
            }
            else {
                document.getElementById("spanAddEditPsychCertLightBoxMsg").className = "ErrorText";
                document.getElementById("spanAddEditPsychCertLightBoxMsg").innerHTML = status;
                document.getElementById("spanAddEditPsychCertLightBoxMsg").style.display = "";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Client validation of AddEditPsychCertLightBox light-box
function ValidateAddEditPsychCertLightBox() {
    var res = true;

    var lblMessage = document.getElementById("spanAddEditPsychCertLightBoxMsg");
    lblMessage.innerHTML = "";

    var notValidFields = new Array();

    var txtPsychCertDate = document.getElementById("txtPsychCertDate");
    if (TrimString(txtPsychCertDate.value) != "" && !IsValidDate(txtPsychCertDate.value)) {
        res = false;
        lblMessage.innerHTML += GetErrorMessageDate("Комисия от дата") + "<br />";
    }

    var txtPsychCertExpirationDate = document.getElementById("txtPsychCertExpirationDate");
    if (TrimString(txtPsychCertExpirationDate.value) != "" && !IsValidDate(txtPsychCertExpirationDate.value)) {
        res = false;
        lblMessage.innerHTML += GetErrorMessageDate("Дата на валидност") + "<br />";
    }

    var notValidFieldsCount = notValidFields.length;

    if (notValidFieldsCount > 0) {
        var noRightsMessage = GetErrorMessageNoRights(notValidFields);
        lblMessage.innerHTML += "<br />" + noRightsMessage;
        res = false;
    }

    if (res) {
        lblMessage.className = "SuccessText";
    }
    else {
        lblMessage.className = "ErrorText";
        lblMessage.style.display = "";
    }

    return res;
}

// Delete a particular Psych Cert record
function DeletePsychCert(psychCertID) {
    YesNoDialog("Желаете ли да изтриете психологическата пригодност?", ConfirmYes, null);

    function ConfirmYes() {
        var url = "AddApplicant_PersonDetails.aspx?AjaxMethod=JSDeletePsychCert";

        var params = "PsychCertID=" + psychCertID +
                 "&PersonId=" + document.getElementById(hdnPersonIDClientID).value;

        function response_handler(xml) {
            var status = xmlValue(xml, "status")

            if (status == "OK") {
                var refreshedPsychCertTable = xmlValue(xml, "refreshedPsychCertTable");

                document.getElementById("divPsychCertTable").innerHTML = refreshedPsychCertTable;

                document.getElementById("spanPsychCertSectionMsg").className = "SuccessText";
                document.getElementById("spanPsychCertSectionMsg").innerHTML = "Психологическата пригодност е изтритa успешно";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
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