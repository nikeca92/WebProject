window.addEventListener ? window.addEventListener('load', PageLoad, false) : window.attachEvent('onload', PageLoad);
var goToShow = false;
var isNewPerson = true;
var isValidIdentNumber = false;
var isAlreadyOccupiedIdentNumber = false;

//Call this function when the page is loaded
function PageLoad() {
    LoadMilitaryReportPersonDetails();

    SetClientTextAreaMaxLength("txtaPermAddress", "40");
    SetClientTextAreaMaxLength("txtaPresAddress", "1500");
}

//This function load the military report person's details
function LoadMilitaryReportPersonDetails() {
    var url = "AddEditMilitaryReportPerson.aspx?AjaxMethod=JSLoadMilitaryReportPersonDetails";
    var params = "";
    params += "HdnMilitaryReportPersonId=" + document.getElementById(hdnMilitaryReportPersonIDClientID).value;
    var myAJAX = new AJAX(url, true, params, LoadMilitaryReportPersonDetails_Callback);
    myAJAX.Call();
}

function LoadMilitaryReportPersonDetails_Callback(xml) {
    var person = xml.getElementsByTagName("militaryReportPerson")[0];

    var militaryReportPersonId = xmlValue(person, "militaryReportPersonId");

    if (militaryReportPersonId != 0) {
        isNewPerson = false;
        isValidIdentNumber = true;
    }
    else {
        isNewPerson = true;
        isValidIdentNumber = false;
    }

    var militaryDepartmentId = xmlValue(person, "militaryDepartmentId");
    var identNumber = xmlValue(person, "identNumber");
    var firstName = xmlValue(person, "firstName");
    var lastName = xmlValue(person, "lastName");
    var initials = xmlValue(person, "initials");
    var lastModified = xmlValue(person, "lastModified");
    var permCityId = xmlValue(person, "permCityId");
    var permPostCode = xmlValue(person, "permPostCode");
    var permSecondPostCode = xmlValue(person, "permSecondPostCode");
    var permRegionId = xmlValue(person, "permRegionId");
    var permMunicipalityId = xmlValue(person, "permMunicipalityId");
    var permDistrictId = xmlValue(person, "permDistrictId");
    var permAddress = xmlValue(person, "permAddress");
    var presCityId = xmlValue(person, "presCityId");
    var presPostCode = xmlValue(person, "presPostCode");
    var presSecondPostCode = xmlValue(person, "presSecondPostCode");
    var presRegionId = xmlValue(person, "presRegionId");
    var presMunicipalityId = xmlValue(person, "presMunicipalityId");
    var presDistrictId = xmlValue(person, "presDistrictId");
    var presAddress = xmlValue(person, "presAddress");
    var birthCountryId = xmlValue(xml, "birthCountryId");
    var birthCityId = xmlValue(xml, "birthCityId");
    var birthMunicipalityId = xmlValue(xml, "birthMunicipalityId");
    var birthRegionId = xmlValue(xml, "birthRegionId");
    var birthPostCode = xmlValue(xml, "birthPostCode");
    var birthCityIfAbroad = xmlValue(xml, "birthCityIfAbroad");
    var birthAbroad = xmlValue(xml, "birthAbroad");

    document.getElementById(hdnMilitaryReportPersonIDClientID).value = militaryReportPersonId;

    var ddlMilitaryDepartments = document.getElementById(ddlMilitaryDepartmentsClientID);
    
    if (militaryDepartmentId != "" && militaryDepartmentId > 0)
        SetSelectedItemId(ddlMilitaryDepartments, militaryDepartmentId);

    document.getElementById("txtIdentNumber").value = identNumber;
    document.getElementById("txtFirstName").value = firstName;
    document.getElementById("txtLastName").value = lastName;
    document.getElementById("txtInitials").innerHTML = initials;
    //document.getElementById("lblLastModifiedValue").innerHTML = lastModified;

    var txtPermPostCode = document.getElementById("txtPermPostCode");
    txtPermPostCode.value = permSecondPostCode;

    if (permCityId) {
        //FillCityFromPostCode(txtPermPostCode, 1);
        //RepopulateRegionMunicipalityCityDistrict(txtPermPostCode.value, "ddlPermRegion", "ddlPermMunicipality", "ddlPermCity", "ddlPermDistrict", 1);
        RepopulateRegionMunicipalityCityDistrictByCityID(permCityId, permDistrictId, "ddlPermRegion", "ddlPermMunicipality", "ddlPermCity", "ddlPermDistrict", 1);
    }
    else {
        ClearSelectList(document.getElementById("ddlPermCity"));
        ClearSelectList(document.getElementById("ddlPermMunicipality"));
    }

    document.getElementById("txtaPermAddress").value = permAddress;

    var txtPresPostCode = document.getElementById("txtPresPostCode");
    txtPresPostCode.value = presSecondPostCode;

    if (presCityId) {
        //FillCityFromPostCode(txtPresPostCode, 2);
        //RepopulateRegionMunicipalityCityDistrict(txtPresPostCode.value, "ddlPresRegion", "ddlPresMunicipality", "ddlPresCity", "ddlPresDistrict", 2);
        RepopulateRegionMunicipalityCityDistrictByCityID(presCityId, presDistrictId, "ddlPresRegion", "ddlPresMunicipality", "ddlPresCity", "ddlPresDistrict", 2);
    }
    else {
        ClearSelectList(document.getElementById("ddlPresCity"));
        ClearSelectList(document.getElementById("ddlPresMunicipality"));
    }

    document.getElementById("txtaPresAddress").value = presAddress;

    document.getElementById("ddBirthCountry").value = birthCountryId;
    ddBirthCountry_Changed();

    if (parseInt(birthAbroad) == 1)
    {
        document.getElementById("txtBirthCityIfAbroad").value = birthCityIfAbroad;
    }
    else
    {
        document.getElementById("txtBirthPostCode").value = birthPostCode;
        document.getElementById("ddBirthRegion").value = birthRegionId;

        ClearSelectList(document.getElementById("ddBirthMunicipality"), true);

        var b_municipalities = xml.getElementsByTagName("b_m");

        for (var i = 0; i < b_municipalities.length; i++)
        {
            var id = xmlValue(b_municipalities[i], "id");
            var name = xmlValue(b_municipalities[i], "name");

            AddToSelectList(document.getElementById("ddBirthMunicipality"), id, name);
        };

        document.getElementById("ddBirthMunicipality").value = birthMunicipalityId;


        ClearSelectList(document.getElementById("ddBirthCity"), true);

        var b_cities = xml.getElementsByTagName("b_c");

        for (var i = 0; i < b_cities.length; i++)
        {
            var id = xmlValue(b_cities[i], "id");
            var name = xmlValue(b_cities[i], "name");

            AddToSelectList(document.getElementById("ddBirthCity"), id, name);
        };

        document.getElementById("ddBirthCity").value = birthCityId;
    }

    if (!isNewPerson)
        document.getElementById("fldstMilRepPersonSpecialities").style.display = "";
        
    var specialitiesTable = xmlValue(xml, "specialitiesTable");
    document.getElementById("pnlSpecialities").innerHTML = specialitiesTable;

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

    setTimeout("LoadOriginalValues()", 1000);
    ShowContent();
}

//This function displays the content div and hides the "loading" div
function ShowContent() {
    document.getElementById("loadingDiv").style.display = "none";
    document.getElementById("contentDiv").style.display = "";
}

function Save_Click() {
    JSCheckIsIdentNumberAlreadyOccupied();
    setTimeout("SaveData()", 1000);

    return false;
}

//this function checks identNumber whether is already associated to another military report person
function JSCheckIsIdentNumberAlreadyOccupied() {
    var txtIdentNumber = document.getElementById("txtIdentNumber");
    var url = "AddEditMilitaryReportPerson.aspx?AjaxMethod=JSCheckIsIdentNumberAlreadyOccupied";
    var params = "";
    params += "HdnMilitaryReportPersonId=" + document.getElementById(hdnMilitaryReportPersonIDClientID).value;
    params += "&IdentNumber=" + document.getElementById("txtIdentNumber").value;
    
    function IsOccupied_Callback(xml) {
        isAlreadyOccupiedIdentNumber = parseInt(xmlValue(xml, "isValidIdentNumber")) == 0;
    }

    var myAJAX = new AJAX(url, true, params, IsOccupied_Callback);
    myAJAX.Call();
}

function IsPersonalDataValid() {
    var res = true;

    var lblMessage = document.getElementById("lblPersonalDataMessage");
    lblMessage.innerHTML = "";
    var ValidationMessage = "";

    var notValidFields = new Array();

    if (document.getElementById(ddlMilitaryDepartmentsClientID).value.Trim() == optionChooseOneValue) {
        res = false;

        if (document.getElementById(ddlMilitaryDepartmentsClientID).disabled == true || document.getElementById(ddlMilitaryDepartmentsClientID).style.display == "none") {
            notValidFields.push("На отчет в");
        }
        else {
            ValidationMessage += GetErrorMessageMandatory("На отчет в") + "</br>";
        }
    }
    
    if (isNewPerson) {
        if (document.getElementById("txtIdentNumber").value.Trim() == "") {
            res = false;

            if (document.getElementById("txtIdentNumber").disabled == true || document.getElementById("txtIdentNumber").style.display == "none") {
                notValidFields.push("ЕГН");
            }
            else {
                ValidationMessage += GetErrorMessageMandatory("ЕГН") + "</br>";
            }
        }
        else {
            if (isValidIdentNumber == 0) {
                res = false;
                ValidationMessage += "Въведеното ЕГН е невалидно" + "</br>";
            }
            else if (isAlreadyOccupiedIdentNumber) {
                res = false;
                ValidationMessage += "Въведеното ЕГН принадлежи на друго военно-отчетно лице" + "</br>";
            }
        }
    }
    else if (isAlreadyOccupiedIdentNumber) {
        res = false;
        ValidationMessage += "Въведеното ЕГН принадлежи на друго военно-отчетно лице" + "</br>";
    }

    if (document.getElementById("txtFirstName").value.Trim() == "") {
        res = false;

        if (document.getElementById("txtFirstName").disabled == true || document.getElementById("txtFirstName").style.display == "none") {
            notValidFields.push("Име и презиме");
        }
        else {
            ValidationMessage += GetErrorMessageMandatory("Име и презиме") + "</br>";
        }
    }

    if (document.getElementById("txtLastName").value.Trim() == "") {
        res = false;

        if (document.getElementById("txtLastName").disabled == true || document.getElementById("txtLastName").style.display == "none") {
            notValidFields.push("Фамилия");
        }
        else {
            ValidationMessage += GetErrorMessageMandatory("Фамилия") + "</br>";
        }
    }

    if (document.getElementById("ddlPermCity").value.Trim() == optionChooseOneValue) {
        res = false;

        if (document.getElementById("ddlPermCity").disabled == true || document.getElementById("ddlPermCity").style.display == "none") {
            notValidFields.push("Населено място (постоянен адрес)");
        }
        else {
            ValidationMessage += GetErrorMessageMandatory("Населено място (постоянен адрес)") + "</br>";
        }
    }

    if (document.getElementById("ddlPresCity").value.Trim() == optionChooseOneValue) {
        res = false;

        if (document.getElementById("ddlPresCity").disabled == true || document.getElementById("ddlPresCity").style.display == "none") {
            notValidFields.push("Населено място (настоящ адрес)");
        }
        else {
            ValidationMessage += GetErrorMessageMandatory("Населено място (настоящ адрес)") + "</br>";
        }
    }

    var notValidFieldsCount = notValidFields.length;

    if (notValidFieldsCount > 0) {
        var noRightsMessage = GetErrorMessageNoRights(notValidFields);
        ValidationMessage += noRightsMessage + "<br />";
    }

    if (res) {
        lblMessage.className = "SuccessText";
        lblMessage.innerHTML = "";
    }
    else {
        lblMessage.className = "ErrorText";
        lblMessage.style.display = "";
        lblMessage.innerHTML = ValidationMessage;
    }

    return res;
}

//Check the ident number only when it is changed
function IdentNumberFocus() {
    var txtIdentNumber = document.getElementById("txtIdentNumber");
    txtIdentNumber.setAttribute("oldvalue", txtIdentNumber.value);
}

//When the user type an IdentNumber then check if this is an existing Reservist and/or Person and
//take care about the particular case
function IdentNumberBlur() {
    var txtIdentNumber = document.getElementById("txtIdentNumber");

    if (txtIdentNumber.value != txtIdentNumber.getAttribute("oldvalue")) {
        if (isOnlyDigits(document.getElementById("txtIdentNumber").value)) {
            var url = "AddEditMilitaryReportPerson.aspx?AjaxMethod=JSCheckIdentNumber";
            var params = "";
            params += "IdentNumber=" + document.getElementById("txtIdentNumber").value;
            var myAJAX = new AJAX(url, true, params, IdentNumberBlur_Callback);
            myAJAX.Call();
        }
        else {
            isValidIdentNumber = 0;
        }
    }

    function IdentNumberBlur_Callback(xml) {
        var militaryReportPersonId = parseInt(xmlValue(xml, "militaryReportPersonId"));
        isValidIdentNumber = parseInt(xmlValue(xml, "isValidIdentNumber")) == 1;

        //Pre-load the existing Person data
        if (militaryReportPersonId > 0) {
            document.getElementById(hdnMilitaryReportPersonIDClientID).value = militaryReportPersonId;
            LoadMilitaryReportPersonDetails();

            document.getElementById(lblHeaderTitleClientID).innerHTML = "Редактиране лични данни на военно-отчетно лице";
        }
    }
}

function SaveData() {
    if (IsPersonalDataValid()) {
        var url = "AddEditMilitaryReportPerson.aspx?AjaxMethod=JSSaveMilitaryReportPersonDetails";
        var params = "";
        params += "HdnMilitaryReportPersonId=" + document.getElementById(hdnMilitaryReportPersonIDClientID).value;
        params += "&MilitaryDepartmentID=" + GetSelectedItemId(document.getElementById(ddlMilitaryDepartmentsClientID));
        params += "&IdentNumber=" + document.getElementById("txtIdentNumber").value;
        params += "&FirstName=" + document.getElementById("txtFirstName").value;
        params += "&LastName=" + document.getElementById("txtLastName").value;
        params += "&Initials=" + document.getElementById("txtInitials").innerHTML;
        params += "&PermCityID=" + GetSelectedItemId(document.getElementById("ddlPermCity"));
        params += "&PermDistrictID=" + GetSelectedItemId(document.getElementById("ddlPermDistrict"));
        params += "&PermSecondPostCode=" + document.getElementById("txtPermPostCode").value;
        params += "&PermAddress=" + document.getElementById("txtaPermAddress").value;
        params += "&PresCityID=" + GetSelectedItemId(document.getElementById("ddlPresCity"));
        params += "&PresDistrictID=" + GetSelectedItemId(document.getElementById("ddlPresDistrict"));
        params += "&PresSecondPostCode=" + document.getElementById("txtPresPostCode").value;
        params += "&PresAddress=" + document.getElementById("txtaPresAddress").value;
        params += "&BirthCountryId=" + document.getElementById("ddBirthCountry").value;
        params += "&IsBirthAbroad=" + (IsAbroad("ddBirthCountry") ? "1" : "0");
        params += "&BirthCityIfAbroad=" + custEncodeURI(document.getElementById("txtBirthCityIfAbroad").value);
        params += "&BirthCityId=" + document.getElementById("ddBirthCity").value;

        var myAJAX = new AJAX(url, true, params, SaveData_Callback);
        myAJAX.Call();
    }
}

function SaveData_Callback(xml) {
    var militaryReportPersonId = xmlValue(xml, "militaryReportPersonId");
    var message = "";

    if (document.getElementById(hdnMilitaryReportPersonIDClientID).value == 0) {
        document.getElementById(hdnMilitaryReportPersonIDClientID).value = militaryReportPersonId;
        location.hash = "AddEditMilitaryReportPerson.aspx?MilitaryReportPersonId=" + militaryReportPersonId;
        message = "Военно-отчетното лице е добавено успешно";

        if (document.getElementById(hdnHiddenClientControls).value.indexOf("fldstMilRepPersonSpecialities") == -1)
            document.getElementById("fldstMilRepPersonSpecialities").style.display = "";
    }
    else {
        document.getElementById(hdnMilitaryReportPersonIDClientID).value = militaryReportPersonId;
        message = "Личните данни са обновени успешно";
    }

    document.getElementById("lblPersonalDataMessage").className = "SuccessText";
    document.getElementById("lblPersonalDataMessage").innerHTML = message;

    LoadOriginalValues();
}

function FillCityFromPostCode(object, addressType) {
    var ddlRegion;
    var ddlMunicipality;
    var ddlCity;

    if (addressType == 1) // Permanent address
    {
        ddlRegion = document.getElementById("ddlPermRegion");
        ddlMunicipality = document.getElementById("ddlPermMunicipality");
        ddlCity = document.getElementById("ddlPermCity");
    }
    else // present address
    {
        ddlRegion = document.getElementById("ddlPresRegion");
        ddlMunicipality = document.getElementById("ddlPresMunicipality");
        ddlCity = document.getElementById("ddlPresCity");
    }

    var oldPostCode = object.getAttribute('oldPostCode');

    var postCode = object.value;

    if (oldPostCode != postCode) {
        var url = "AddEditMilitaryReportPerson.aspx?AjaxMethod=JSGetCityFromPostCode";
        var params = "&PostCode=" + postCode;

        function response_handler_PostCode(xml) {
            var statusResult = xmlValue(xml, "statusResult");

            if (statusResult == "NO") return;

            var regionId = xmlValue(xml, "selectedRegionId");
            var municipalityId = xmlValue(xml, "selectedMunicipalityID");
            var cityId = xmlValue(xml, "selectedCityId");

            //Bind ddlEmpMunicipality
            ClearSelectList(ddlMunicipality);

            var DTM = xml.getElementsByTagName("municipality");

            for (i = 0; i < DTM.length; i++) {
                DTMId = xmlValue(DTM[i], "municipalityId");
                DTMFullName = xmlValue(DTM[i], "municipalyName")

                AddToSelectList(ddlMunicipality, DTMId, DTMFullName);
            }

            //Bind ddlEmpMunicipality
            ClearSelectList(ddlCity);

            DTM = xml.getElementsByTagName("city");

            for (i = 0; i < DTM.length; i++) {
                DTMId = xmlValue(DTM[i], "cityId");
                DTMFullName = xmlValue(DTM[i], "cityName")

                AddToSelectList(ddlCity, DTMId, DTMFullName);
            }

            //Now set selected items
            SetSelectedItemId(ddlRegion, regionId);
            SetSelectedItemId(ddlMunicipality, municipalityId);
            SetSelectedItemId(ddlCity, cityId);

            if ((caseToShowContent == 4) && (addressType == 2)) {
                //ShowContent();
                //LoadOriginalValues();
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler_PostCode);
        myAJAX.Call();
    }
}

//This function load the all districts acording selected City
function LoadAllDistrictsBySelectedCity(cityId) {
    var url = "AddEditMilitaryReportPerson.aspx?AjaxMethod=JS_District";
    var params = "";
    params += "selectedItemId=" + cityId;
    
    function response_handler(xml) {
        var ddlDistrict;

        if (AddressType == 1) // Permanent address
        {
            ddlDistrict = document.getElementById("ddlPermDistrict");
        }
        else // present address
        {
            ddlDistrict = document.getElementById("ddlPresDistrict");
        }

        //Bind ddlDistrict
        ClearSelectList(ddlDistrict);

        DTM = xml.getElementsByTagName("district");
        if (DTM[0].childNodes.length > 0) {
            for (i = 0; i < DTM.length; i++) {
                DTMId = xmlValue(DTM[i], "districtId");
                DTMFullName = xmlValue(DTM[i], "districtName")

                AddToSelectList(ddlDistrict, DTMId, DTMFullName);
            }
        }

        //Now set selected items
        SetSelectedItemId(ddlDistrict, "-1");
        //LoadOriginalValues();
        //ShowContent();
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}

function CopyPresAddressToCurr()
{
    document.getElementById("txtPermPostCode").value = document.getElementById("txtPresPostCode").value;
    CopyDropDown("ddlPresRegion", "ddlPermRegion");
    CopyDropDown("ddlPresMunicipality", "ddlPermMunicipality");
    CopyDropDown("ddlPresCity", "ddlPermCity");
    CopyDropDown("ddlPresDistrict", "ddlPermDistrict"); 
    document.getElementById("txtaPermAddress").value = document.getElementById("txtaPresAddress").value;
}

function CopyPermAddressToCurr() {
    document.getElementById("txtPresPostCode").value = document.getElementById("txtPermPostCode").value;
    CopyDropDown("ddlPermRegion", "ddlPresRegion");
    CopyDropDown("ddlPermMunicipality", "ddlPresMunicipality");
    CopyDropDown("ddlPermCity", "ddlPresCity");
    CopyDropDown("ddlPermDistrict", "ddlPresDistrict");
    document.getElementById("txtaPresAddress").value = document.getElementById("txtaPermAddress").value;
}

//When changing the Perm Region then refresh the Perm Municipality and the Perm City
function ddPermRegion_Changed() {
    var ddPermRegion = document.getElementById("ddlPermRegion");
    RepopulateMunicipality(ddPermRegion.value, "ddlPermMunicipality");
}

//When changing the Perm Municipality then refresh the Perm City
function ddPermMunicipality_Changed() {
    var ddPermMunicipality = document.getElementById("ddlPermMunicipality");
    RepopulateCity(ddPermMunicipality.value, "ddlPermCity");
}

//When changing the Perm City then refresh the Perm Post Code
function ddPermCity_Changed() {
    var ddPermCity = document.getElementById("ddlPermCity");
    RepopulatePostCodeAndDistrict(ddPermCity.value, "txtPermPostCode", "ddlPermDistrict");
}

//When changing the PostCode then repopulate the Region, the Municipality and the City
function txtPermPostCode_Focus() {
    var txtPermPostCode = document.getElementById("txtPermPostCode");
    txtPermPostCode.setAttribute("oldvalue", txtPermPostCode.value);
}

function txtPermPostCode_Blur() {
    var txtPermPostCode = document.getElementById("txtPermPostCode");

    if (txtPermPostCode.value != txtPermPostCode.getAttribute("oldvalue")) {
        RepopulateRegionMunicipalityCityDistrict(txtPermPostCode.value, "ddlPermRegion", "ddlPermMunicipality", "ddlPermCity", "ddlPermDistrict", 1);
    }
}

function RepopulateRegionMunicipalityCity(postCode, ddRegionId, ddMunicipalityId, ddCityId) {
    var url = "AddEditMilitaryReportPerson.aspx?AjaxMethod=JSRepopulateRegionMunicipalityCity";
    var params = "";
    params += "PostCode=" + postCode;
    
    function RepopulateRegionMunicipalityCity_Callback(xml) {
        var cityId = parseInt(xmlValue(xml, "cityId"));

        //Found
        if (cityId != 0)
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

function RepopulatePostCodeAndDistrict(cityId, txtPostCodeId, ddDistrictsId) {
    var url = "AddEditMilitaryReportPerson.aspx?AjaxMethod=JSRepopulatePostCodeAndDistrict";
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

        if (ddDistrictsId == "ddlPermDistrict")
            ddPermDistrict_Changed();
        else if (ddDistrictsId == "ddlPresDistrict")
            ddCurrDistrict_Changed();
    }

    var myAJAX = new AJAX(url, true, params, RepopulatePostCodeAndDistrict_Callback);
    myAJAX.Call();
}

//When chaning the District then repopulate the PostCode
function ddPermDistrict_Changed() {
    var ddPermDistrict = document.getElementById("ddlPermDistrict");
    RepopulateDistrictPostCode(ddPermDistrict.value, "txtPermPostCode");
}

function RepopulateDistrictPostCode(districtId, txtPostCodeId) {
    var url = "AddEditMilitaryReportPerson.aspx?AjaxMethod=JSRepopulateDistrictPostCode";
    var params = "";
    params += "DistrictId=" + districtId;
    
    function RepopulateDistrictPostCode_Callback(xml) {
        var districtPostCode = xmlValue(xml, "districtPostCode");

        if (districtPostCode != "")
            document.getElementById(txtPostCodeId).value = districtPostCode;
    }

    var myAJAX = new AJAX(url, true, params, RepopulateDistrictPostCode_Callback);
    myAJAX.Call();
}

function RepopulatePostCode(cityId, txtPostCodeId) {
    var url = "AddEditMilitaryReportPerson.aspx?AjaxMethod=JSRepopulatePostCode";
    var params = "";
    params += "CityId=" + cityId;
    
    function RepopulatePostCode_Callback(xml) {
        var postCode = xmlValue(xml, "postCode");

        document.getElementById(txtPostCodeId).value = postCode;
    }

    var myAJAX = new AJAX(url, true, params, RepopulatePostCode_Callback);
    myAJAX.Call();
}

function RepopulateCity(municipalityId, ddCityId) {
    var url = "AddEditMilitaryReportPerson.aspx?AjaxMethod=JSRepopulateCity";
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

        if (ddCityId == "ddlPermCity")
            ddPermCity_Changed();
        else if (ddCityId == "ddlPresCity")
            ddCurrCity_Changed();
    }

    var myAJAX = new AJAX(url, true, params, RepopulateMunicipality_Callback);
    myAJAX.Call();
}

function RepopulateRegionMunicipalityCityDistrict(postCode, ddRegionId, ddMunicipalityId, ddCityId, ddDistrictId, addressType) {
    var url = "AddEditMilitaryReportPerson.aspx?AjaxMethod=JSRepopulateRegionMunicipalityCityDistrict";
    var params = "";
    params += "HdnMilitaryReportPersonId=" + document.getElementById(hdnMilitaryReportPersonIDClientID).value;
    params += "&PostCode=" + postCode;
    params += "&AddressType=" + addressType;
    
    function RepopulateRegionMunicipalityCity_Callback(xml)
    {
        var cityId = parseInt(xmlValue(xml, "cityId"));

        //Not found
        if (cityId == 0) {
            document.getElementById(ddRegionId).selectedIndex = 0;

            ClearSelectList(document.getElementById(ddMunicipalityId), false);
            ClearSelectList(document.getElementById(ddCityId), false);
            ClearSelectList(document.getElementById(ddDistrictId), false);
        }
        else //found
        {
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


function RepopulateRegionMunicipalityCityDistrictByCityID(cityId, districtId, ddRegionId, ddMunicipalityId, ddCityId, ddDistrictId, addressType)
{
    var url = "AddEditMilitaryReportPerson.aspx?AjaxMethod=JSRepopulateRegionMunicipalityCityDistrictByCityID";
    var params = "";
    params += "HdnMilitaryReportPersonId=" + document.getElementById(hdnMilitaryReportPersonIDClientID).value;
    params += "&CityID=" + cityId;
    params += "&DistrictID=" + districtId;
    params += "&AddressType=" + addressType;
    
    function RepopulateRegionMunicipalityCity_Callback(xml)
    {
        var cityId = parseInt(xmlValue(xml, "cityId"));

        //Not found
        if (cityId == 0)
        {
            document.getElementById(ddRegionId).selectedIndex = 0;

            ClearSelectList(document.getElementById(ddMunicipalityId), false);
            ClearSelectList(document.getElementById(ddCityId), false);
            ClearSelectList(document.getElementById(ddDistrictId), false);
        }
        else //found
        {
            var regionId = xmlValue(xml, "regionId");
            var municipalityId = xmlValue(xml, "municipalityId");
            var districtId = xmlValue(xml, "districtId");

            document.getElementById(ddRegionId).value = regionId;

            ClearSelectList(document.getElementById(ddMunicipalityId), true);

            var municipalities = xml.getElementsByTagName("m");

            for (var i = 0; i < municipalities.length; i++)
            {
                var id = xmlValue(municipalities[i], "id");
                var name = xmlValue(municipalities[i], "name");

                AddToSelectList(document.getElementById(ddMunicipalityId), id, name);
            };

            document.getElementById(ddMunicipalityId).value = municipalityId;


            ClearSelectList(document.getElementById(ddCityId), true);

            var cities = xml.getElementsByTagName("c");

            for (var i = 0; i < cities.length; i++)
            {
                var id = xmlValue(cities[i], "id");
                var name = xmlValue(cities[i], "name");

                AddToSelectList(document.getElementById(ddCityId), id, name);
            };

            document.getElementById(ddCityId).value = cityId;


            ClearSelectList(document.getElementById(ddDistrictId), true);

            var districts = xml.getElementsByTagName("d");

            for (var i = 0; i < districts.length; i++)
            {
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



//When changing the Curr Region then refresh the Curr Municipality and the Curr City
function ddCurrRegion_Changed() {
    var ddCurrRegion = document.getElementById("ddlPresRegion");
    RepopulateMunicipality(ddCurrRegion.value, "ddlPresMunicipality");
}

//When changing the Perm Municipality then refresh the Perm City
function ddCurrMunicipality_Changed() {
    var ddCurrMunicipality = document.getElementById("ddlPresMunicipality");
    RepopulateCity(ddCurrMunicipality.value, "ddlPresCity");
}

//When changing the Perm City then refresh the Perm Post Code
function ddCurrCity_Changed() {
    var ddCurrCity = document.getElementById("ddlPresCity");
    RepopulatePostCodeAndDistrict(ddCurrCity.value, "txtPresPostCode", "ddlPresDistrict");
}

//When changing the PostCode then repopulate the Region, the Municipality and the City
function txtCurrPostCode_Focus() {
    var txtCurrPostCode = document.getElementById("txtPresPostCode");
    txtCurrPostCode.setAttribute("oldvalue", txtCurrPostCode.value);
}

function txtCurrPostCode_Blur() {
    var txtCurrPostCode = document.getElementById("txtPresPostCode");

    if (txtCurrPostCode.value != txtCurrPostCode.getAttribute("oldvalue")) {
        RepopulateRegionMunicipalityCityDistrict(txtCurrPostCode.value, "ddlPresRegion", "ddlPresMunicipality", "ddlPresCity", "ddlPresDistrict", 2);
    }
}

//When chaning the District then repopulate the PostCode
function ddCurrDistrict_Changed() {
    var ddCurrDistrict = document.getElementById("ddlPresDistrict");
    RepopulateDistrictPostCode(ddCurrDistrict.value, "txtPresPostCode");
}

function PopulateInitials()
{
    var result = "";

    var names = document.getElementById("txtFirstName").value + " " + document.getElementById("txtLastName").value;

    var words = TrimString(names).split(/[-\,\s]+/);

    for (var i = 0; i < words.length; i++)
    {
        if (words[i].length > 0)
        {
            result += words[i][0].toUpperCase();
        }
    }

    document.getElementById("txtInitials").innerHTML = result;
}


//When changing the Birth Country then hide/show the Birth City input area (if abroad / if not abroad)
function ddBirthCountry_Changed()
{
    DisplayBirthCityInputArea();
}

function IsAbroad(ddCountryId)
{
    var ddBirthCountry = document.getElementById(ddCountryId);
    var birthCountryName = ddBirthCountry.options[ddBirthCountry.selectedIndex].text;

    return birthCountryName.toUpperCase() != "БЪЛГАРИЯ";
}

function DisplayBirthCityInputArea()
{
    //If not abroad
    if (!IsAbroad("ddBirthCountry"))
    {
        document.getElementById("birthCityIfAbroad").style.display = "none";
        document.getElementById("birthCityIfNotAbroad").style.display = "";

        document.getElementById("txtBirthPostCode").value = "";
        document.getElementById("ddBirthRegion").selectedIndex = 0;
        ClearSelectList(document.getElementById("ddBirthMunicipality"), false);
        ClearSelectList(document.getElementById("ddBirthCity"), false);
    }
    else
    {
        document.getElementById("birthCityIfAbroad").style.display = "";
        document.getElementById("birthCityIfNotAbroad").style.display = "none";

        document.getElementById("txtBirthCityIfAbroad").value = "";
    }
}

//When changing the Birth Region then refresh the Birth Municipality and the Birth City
function ddBirthRegion_Changed()
{
    var ddBirthRegion = document.getElementById("ddBirthRegion");
    RepopulateMunicipality(ddBirthRegion.value, "ddBirthMunicipality");
}

function RepopulateMunicipality(regionId, ddMunicipalityId)
{
    var url = "AddEditMilitaryReportPerson.aspx?AjaxMethod=JSRepopulateMunicipality";
    var params = "";
    params += "RegionId=" + regionId;
    
    function RepopulateMunicipality_Callback(xml)
    {
        ClearSelectList(document.getElementById(ddMunicipalityId), true);

        var municipalities = xml.getElementsByTagName("m");

        for (var i = 0; i < municipalities.length; i++)
        {
            var id = xmlValue(municipalities[i], "id");
            var name = xmlValue(municipalities[i], "name");

            AddToSelectList(document.getElementById(ddMunicipalityId), id, name);
        };

        if (ddMunicipalityId == "ddBirthMunicipality")
            ddBirthMunicipality_Changed();
        else if (ddMunicipalityId == "ddlPermMunicipality")
            ddPermMunicipality_Changed();
        else if (ddMunicipalityId == "ddlPresMunicipality")
            ddCurrMunicipality_Changed();
    }

    var myAJAX = new AJAX(url, true, params, RepopulateMunicipality_Callback);
    myAJAX.Call();
}


//When changing the Birth Municipality then refresh the Birth City
function ddBirthMunicipality_Changed()
{
    var ddBirthMunicipality = document.getElementById("ddBirthMunicipality");
    RepopulateCity(ddBirthMunicipality.value, "ddBirthCity");
}

function RepopulateCity(municipalityId, ddCityId)
{
    var url = "AddEditMilitaryReportPerson.aspx?AjaxMethod=JSRepopulateCity";
    var params = "";
    params += "MunicipalityId=" + municipalityId;
    
    function RepopulateMunicipality_Callback(xml)
    {
        ClearSelectList(document.getElementById(ddCityId), true);

        var cities = xml.getElementsByTagName("c");

        for (var i = 0; i < cities.length; i++)
        {
            var id = xmlValue(cities[i], "id");
            var name = xmlValue(cities[i], "name");

            AddToSelectList(document.getElementById(ddCityId), id, name);
        };

        if (ddCityId == "ddBirthCity")
            ddBirthCity_Changed();
        else if (ddCityId == "ddPermCity")
            ddPermCity_Changed();
        else if (ddCityId == "ddCurrCity")
            ddCurrCity_Changed();
    }

    var myAJAX = new AJAX(url, true, params, RepopulateMunicipality_Callback);
    myAJAX.Call();
}

//When changing the Birth City then refresh the Birth Post Code
function ddBirthCity_Changed()
{
    var ddBirthCity = document.getElementById("ddBirthCity");
    RepopulatePostCode(ddBirthCity.value, "txtBirthPostCode");
}

function RepopulatePostCode(cityId, txtPostCodeId)
{
    var url = "AddEditMilitaryReportPerson.aspx?AjaxMethod=JSRepopulatePostCode";
    var params = "";
    params += "CityId=" + cityId;
    
    function RepopulatePostCode_Callback(xml)
    {
        var postCode = xmlValue(xml, "postCode");

        document.getElementById(txtPostCodeId).value = postCode;
    }

    var myAJAX = new AJAX(url, true, params, RepopulatePostCode_Callback);
    myAJAX.Call();
}

//When changing the PostCode then repopulate the Region, the Municipality and the City
function txtBirthPostCode_Focus()
{
    var txtBirthPostCode = document.getElementById("txtBirthPostCode");
    txtBirthPostCode.setAttribute("oldvalue", txtBirthPostCode.value);
}

function txtBirthPostCode_Blur()
{
    var txtBirthPostCode = document.getElementById("txtBirthPostCode");

    if (txtBirthPostCode.value != txtBirthPostCode.getAttribute("oldvalue"))
    {
        RepopulateRegionMunicipalityCity(txtBirthPostCode.value, "ddBirthRegion", "ddBirthMunicipality", "ddBirthCity");
    }
}

function RepopulateRegionMunicipalityCity(postCode, ddRegionId, ddMunicipalityId, ddCityId)
{
    var url = "AddEditMilitaryReportPerson.aspx?AjaxMethod=JSRepopulateRegionMunicipalityCity";
    var params = "";
    params += "PostCode=" + postCode;
    
    function RepopulateRegionMunicipalityCity_Callback(xml)
    {
        var cityId = parseInt(xmlValue(xml, "cityId"));

        //Not found
        if (cityId == 0)
        {
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

            for (var i = 0; i < municipalities.length; i++)
            {
                var id = xmlValue(municipalities[i], "id");
                var name = xmlValue(municipalities[i], "name");

                AddToSelectList(document.getElementById(ddMunicipalityId), id, name);
            };

            document.getElementById(ddMunicipalityId).value = municipalityId;


            ClearSelectList(document.getElementById(ddCityId), true);

            var cities = xml.getElementsByTagName("c");

            for (var i = 0; i < cities.length; i++)
            {
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

//Open the light-box for adding a new record in the MilRepPersonSpecialities table
function NewMilRepPersonSpeciality() {
    ShowAddEditMilRepPersonSpecialityLightBox(0);
}

//Open the light-box for editing a record in the MilRepPersonSpecialities table
function EditMilRepPersonSpeciality(MilRepPersonSpecialityId) {
    ShowAddEditMilRepPersonSpecialityLightBox(MilRepPersonSpecialityId);
}

function ShowAddEditMilRepPersonSpecialityLightBox(MilRepPersonSpecialityId) {
    document.getElementById("hdnMilRepPersonSpecialityID").value = MilRepPersonSpecialityId;

    ClearAllMessages();

    //New record
    if (MilRepPersonSpecialityId == 0) {
        document.getElementById("lblAddEditMilRepPersonSpecialityTitle").innerHTML = "Въвеждане на нова специалност";

        document.getElementById("ddlProfession").value = optionChooseOneValue;
        ClearSelectList(document.getElementById("ddlSpeciality"));

        // clean message label in the light box and hide it
        document.getElementById("spanAddEditMilRepPersonSpecialityLightBox").style.display = "none";
        document.getElementById("spanAddEditMilRepPersonSpecialityLightBox").innerHTML = "";

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("lboxMilRepPersonSpeciality").style.display = "";
        CenterLightBox("lboxMilRepPersonSpeciality");
    }
    else //Edit Position
    {
        document.getElementById("lblAddEditMilRepPersonSpecialityTitle").innerHTML = "Редактиране на специалност";

        var url = "AddEditMilitaryReportPerson.aspx?AjaxMethod=JSMilRepPersonSpeciality";

        var params = "MilRepPersonSpecialityId=" + MilRepPersonSpecialityId;

        function response_handler(xml) {
            var professionId = xmlValue(xml, "professionid");
            var specialityId = xmlValue(xml, "specialityid");

            document.getElementById("ddlProfession").value = professionId;

            ClearSelectList(document.getElementById("ddlSpeciality"), true);

            var specialities = xml.getElementsByTagName("s");

            for (var i = 0; i < specialities.length; i++) {
                var id = xmlValue(specialities[i], "id");
                var name = xmlValue(specialities[i], "name");

                AddToSelectList(document.getElementById("ddlSpeciality"), id, name, true);
            };

            document.getElementById("ddlSpeciality").value = specialityId;
            
            // clean message label in the light box and hide it
            document.getElementById("spanAddEditMilRepPersonSpecialityLightBox").style.display = "none";
            document.getElementById("spanAddEditMilRepPersonSpecialityLightBox").innerHTML = "";

            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("lboxMilRepPersonSpeciality").style.display = "";
            CenterLightBox("lboxMilRepPersonSpeciality");
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Close the light-box
function HideAddEditMilRepPersonSpecialityLightBox() {
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("lboxMilRepPersonSpeciality").style.display = "none";
}

//When changing the Profession then refresh the Speciality
function ddProfession_Changed() {
    var ddProfession = document.getElementById("ddlProfession");
    RepopulateSpeciality(ddProfession.value);
}

function RepopulateSpeciality(professionId) {
    var url = "AddEditMilitaryReportPerson.aspx?AjaxMethod=JSRepopulateSpeciality";
    var params = "";
    params += "ProfessionId=" + professionId;
    
    function RepopulateSpeciality_Callback(xml) {
        ClearSelectList(document.getElementById("ddlSpeciality"), true);

        var specialities = xml.getElementsByTagName("s");

        for (var i = 0; i < specialities.length; i++) {
            var id = xmlValue(specialities[i], "id");
            var name = xmlValue(specialities[i], "name");

            AddToSelectList(document.getElementById("ddlSpeciality"), id, name);
        };
    }

    var myAJAX = new AJAX(url, true, params, RepopulateSpeciality_Callback);
    myAJAX.Call();
}

function SaveAddEditMilRepPersonSpecialityLightBox() {
    if (ValidateAddEditMilRepPersonSpecialityLightBox())
    {
        var url = "AddEditMilitaryReportPerson.aspx?AjaxMethod=JSSaveMilRepPersonSpeciality";

        var params = "MilRepPersonSpecialityId=" + document.getElementById("hdnMilRepPersonSpecialityID").value +
                     "&HdnMilitaryReportPersonId=" + document.getElementById(hdnMilitaryReportPersonIDClientID).value +
                     "&ProfessionId=" + document.getElementById("ddlProfession").value +
                     "&SpecialityId=" + document.getElementById("ddlSpeciality").value;

        function response_handler(xml)
        {
            var status = xmlValue(xml, "status");

            if (status == "OK")
            {
                var refreshedMilRepPersonSpecialitiesTable = xmlValue(xml, "refreshedMilRepPersonSpecialitiesTable");

                document.getElementById("pnlSpecialities").innerHTML = refreshedMilRepPersonSpecialitiesTable;

                document.getElementById("lblPersonalDataMessage").className = "SuccessText";
                document.getElementById("lblPersonalDataMessage").innerHTML = document.getElementById("hdnMilRepPersonSpecialityID").value == "0" ? "Специалността е добавена успешно" : "Специалността е редактирана успешно";

                HideAddEditMilRepPersonSpecialityLightBox();
            }
            else
            {
                document.getElementById("spanAddEditMilRepPersonSpecialityLightBox").className = "ErrorText";
                document.getElementById("spanAddEditMilRepPersonSpecialityLightBox").innerHTML = status;
                document.getElementById("spanAddEditMilRepPersonSpecialityLightBox").style.display = "";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Client validation of AddEditMilRepPersonSpecialityLightBox light-box
function ValidateAddEditMilRepPersonSpecialityLightBox()
{
    var res = true;

    var lblMessage = document.getElementById("spanAddEditMilRepPersonSpecialityLightBox");
    lblMessage.innerHTML = "";

    var notValidFields = new Array();

    var ddlProfession = document.getElementById("ddlProfession");

    if (ddlProfession.value == "" || ddlProfession.value == optionChooseOneValue)
    {
        res = false;

        if (ddlProfession.disabled == true || ddlProfession.style.display == "none")
            notValidFields.push("Професия");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Професия") + "<br />";
    }

    var notValidFieldsCount = notValidFields.length;

    if (notValidFieldsCount > 0)
    {
        var noRightsMessage = GetErrorMessageNoRights(notValidFields);
        lblMessage.innerHTML += "<br />" + noRightsMessage;
    }

    if (res)
    {
        lblMessage.className = "SuccessText";
    }
    else
    {
        lblMessage.className = "ErrorText";
        lblMessage.style.display = "";
    }

    return res;
}

// Delete a particular speciality record
function DeleteMilRepPersonSpeciality(milRepPersonSpecialityID) {
    ClearAllMessages();

    YesNoDialog("Желаете ли да изтриете специалността?", ConfirmYes, null);

    function ConfirmYes() {
        var url = "AddEditMilitaryReportPerson.aspx?AjaxMethod=JSDeleteMilRepPersonSpeciality";

        var params = "MilRepPersonSpecialityId=" + milRepPersonSpecialityID +
                     "&HdnMilitaryReportPersonId=" + document.getElementById(hdnMilitaryReportPersonIDClientID).value;

        function response_handler(xml) {
            var status = xmlValue(xml, "status")

            if (status == "OK") {
                var refreshedMilRepPersonSpecialitiesTable = xmlValue(xml, "refreshedMilRepPersonSpecialitiesTable");

                document.getElementById("pnlSpecialities").innerHTML = refreshedMilRepPersonSpecialitiesTable;

                document.getElementById("lblPersonalDataMessage").className = "SuccessText";
                document.getElementById("lblPersonalDataMessage").innerHTML = "Специалността е изтрита успешно";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

function ClearAllMessages() {
    document.getElementById("lblPersonalDataMessage").innerHTML = "";    
}