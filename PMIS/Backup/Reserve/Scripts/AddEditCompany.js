window.addEventListener ? window.addEventListener('load', PageLoad, false) : window.attachEvent('onload', PageLoad);
var goToShow = false;
var isValidBulstat = false;
var isAlreadyOccupiedCompanyName = false;

//Call this function when the page is loaded
function PageLoad() {
    LoadCompany();

    SetClientTextAreaMaxLength("txtaAddress", "1500");
}

//This function load the company
function LoadCompany()
{
    var url = "AddEditCompany.aspx?AjaxMethod=JSLoadCompany";
    var params = "";
    params += "HdnCompanyId=" + document.getElementById(hdnCompanyIDClientID).value;
    var myAJAX = new AJAX(url, true, params, LoadCompany_Callback);
    myAJAX.Call();
}

function LoadCompany_Callback(xml)
{
    var company = xml.getElementsByTagName("company")[0];

    var companyId = xmlValue(company, "companyId");
    var bulstat = xmlValue(company, "bulstat");
    var companyName = xmlValue(company, "companyName");
    var ownershipTypeId = xmlValue(company, "ownershipTypeId");
    var administrationId = xmlValue(company, "administrationId");
    var phone = xmlValue(company, "phone");
    var cityId = xmlValue(company, "cityId");
    var postcode = xmlValue(company, "postCode");
    var regionId = xmlValue(company, "regionId");
    var municipalityId = xmlValue(company, "municipalityId");
    var districtId = xmlValue(company, "districtId");
    var address = xmlValue(company, "address");

    document.getElementById(hdnCompanyIDClientID).value = companyId;

    var ddOwnershipType = document.getElementById(ddOwnershipTypeClientID);

    if (ownershipTypeId != "" && ownershipTypeId > 0)
        SetSelectedItemId(ddOwnershipType, ownershipTypeId);

    var ddAdministration = document.getElementById(ddAdministrationClientID);

    if (administrationId != "" && administrationId > 0)
        SetSelectedItemId(ddAdministration, administrationId);

    document.getElementById("txtBulstat").value = bulstat;
    document.getElementById("txtCompanyName").value = companyName;
    document.getElementById("txtPhone").value = phone;

    var txtPostCode = document.getElementById("txtPostCode");
    txtPostCode.value = postcode;

    document.getElementById("txtaAddress").value = address;

    if (cityId != "" && cityId > 0)
    {
        RepopulateRegionMunicipalityCityDistrictByCityId(cityId, "ddlRegion", "ddlMunicipality", "ddlCity", "ddlDistrict", RepopulateRegionMunicipalityCityDistrictByCityId_Finished);
    }
    else 
    {
        ClearSelectList(document.getElementById("ddlCity"));
        ClearSelectList(document.getElementById("ddlMunicipality"));

        RepopulateRegionMunicipalityCityDistrictByCityId_Finished();
    }

    function RepopulateRegionMunicipalityCityDistrictByCityId_Finished()
    {
        if (parseInt(districtId) > 0)
            document.getElementById("ddlDistrict").value = districtId;
    
        LoadOriginalValues();
        ShowContent();
    }
}

//This function displays the content div and hides the "loading" div
function ShowContent() {
    document.getElementById("loadingDiv").style.display = "none";
    document.getElementById("contentDiv").style.display = "";
}

function Save_Click() {
    JSCheckIsBulstatAndCompanyName();

    return false;
}

function JSCheckIsBulstatAndCompanyName()
{
    var txtBulstat = document.getElementById("txtBulstat");
    var txtCompanyName = document.getElementById("txtCompanyName");
    
    var url = "AddEditCompany.aspx?AjaxMethod=JSCheckIsBulstatAndCompanyName";
    var params = "";
    params += "HdnCompanyId=" + document.getElementById(hdnCompanyIDClientID).value;
    params += "&Bulstat=" + custEncodeURI(txtBulstat.value);
    params += "&CompanyName=" + custEncodeURI(txtCompanyName.value);
    
    function IsOccupied_Callback(xml) {
        isAlreadyOccupiedCompanyName = parseInt(xmlValue(xml, "isAlreadyOccupiedCompanyName")) == 1;
        isValidBulstat = parseInt(xmlValue(xml, "isValidBulstat")) == 1;

        SaveData();
    }

    var myAJAX = new AJAX(url, true, params, IsOccupied_Callback);
    myAJAX.Call();
}

function IsDataValid() {
    var res = true;

    var lblMessage = document.getElementById("lblMessage");
    lblMessage.innerHTML = "";
    var ValidationMessage = "";

    var notValidFields = new Array();



    if (document.getElementById("txtBulstat").value.Trim() != "")
    {
        if (!isValidBulstat)
        {
            res = false;
            ValidationMessage += "Въведеният " + unifiedIdentityCodeLabelText + "/ЕГН е невалиден" + "</br>";
        }
    }

    if (document.getElementById("txtCompanyName").value.Trim() == "") {
        res = false;

        if (document.getElementById("txtCompanyName").disabled == true || document.getElementById("txtCompanyName").style.display == "none") {
            notValidFields.push("Име");
        }
        else {
            ValidationMessage += GetErrorMessageMandatory("Име") + "</br>";
        }
    }
    else if (isAlreadyOccupiedCompanyName) {
        res = false;
        ValidationMessage += "Вече има въведена фирма с това име" + "</br>";
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

function SaveData() {
    if (IsDataValid())
    {
        var url = "AddEditCompany.aspx?AjaxMethod=JSSaveCompany";
        var params = "";
        params += "CompanyId=" + document.getElementById(hdnCompanyIDClientID).value;
        params += "&Bulstat=" + document.getElementById("txtBulstat").value;
        params += "&CompanyName=" + document.getElementById("txtCompanyName").value;
        params += "&Phone=" + document.getElementById("txtPhone").value;
        params += "&OwnershipTypeId=" + GetSelectedItemId(document.getElementById(ddOwnershipTypeClientID));
        params += "&AdministrationId=" + GetSelectedItemId(document.getElementById(ddAdministrationClientID));        
        params += "&CityID=" + GetSelectedItemId(document.getElementById("ddlCity"));
        params += "&DistrictID=" + GetSelectedItemId(document.getElementById("ddlDistrict"));
        params += "&SecondPostCode=" + document.getElementById("txtPostCode").value;
        params += "&Address=" + document.getElementById("txtaAddress").value;        

        var myAJAX = new AJAX(url, true, params, SaveData_Callback);
        myAJAX.Call();
    }
}

function SaveData_Callback(xml) {
    var companyId = xmlValue(xml, "companyId");
    var message = "";

    if (document.getElementById(hdnCompanyIDClientID).value == 0)
    {
        document.getElementById(hdnCompanyIDClientID).value = companyId;
        location.hash = "AddEditCompany.aspx?MilitaryReportPersonId=" + companyId;
        message = "Фирмата е добавена успешно";
    }
    else
    {
        document.getElementById(hdnCompanyIDClientID).value = companyId;
        message = "Успешен запис на фирмата";
    }

    document.getElementById("lblMessage").className = "SuccessText";
    document.getElementById("lblMessage").innerHTML = message;

    LoadOriginalValues();
}

//When changing the Region then refresh the Municipality and the City
function ddRegion_Changed() {
    var ddRegion = document.getElementById("ddlRegion");
    RepopulateMunicipality(ddRegion.value, "ddlMunicipality");
}

//When changing the Municipality then refresh the City
function ddMunicipality_Changed() {
    var ddPermMunicipality = document.getElementById("ddlMunicipality");
    RepopulateCity(ddPermMunicipality.value, "ddlCity");
}

//When changing the City then refresh the Post Code
function ddCity_Changed() {
    var ddCity = document.getElementById("ddlCity");
    RepopulatePostCodeAndDistrict(ddCity.value, "txtPostCode", "ddlDistrict");
}

//When changing the PostCode then repopulate the Region, the Municipality and the City
function txtPostCode_Focus() {
    var txtPermPostCode = document.getElementById("txtPostCode");
    txtPermPostCode.setAttribute("oldvalue", txtPermPostCode.value);
}

function txtPostCode_Blur() {
    var txtPermPostCode = document.getElementById("txtPostCode");

    if (txtPermPostCode.value != txtPermPostCode.getAttribute("oldvalue")) {
        RepopulateRegionMunicipalityCityDistrict(txtPermPostCode.value, "ddlRegion", "ddlMunicipality", "ddlCity", "ddlDistrict");
    }
}

function RepopulateMunicipality(regionId, ddMunicipalityId) {
    var url = "AddEditCompany.aspx?AjaxMethod=JSRepopulateMunicipality";
    var params = "";
    params += "RegionId=" + regionId;
    
    function RepopulateMunicipality_Callback(xml) {
        ClearSelectList(document.getElementById(ddMunicipalityId), true);

        var municipalities = xml.getElementsByTagName("m");

        for (var i = 0; i < municipalities.length; i++) {
            var id = xmlValue(municipalities[i], "id");
            var name = xmlValue(municipalities[i], "name");

            AddToSelectList(document.getElementById(ddMunicipalityId), id, name);
        };

        ddMunicipality_Changed();
    }

    var myAJAX = new AJAX(url, true, params, RepopulateMunicipality_Callback);
    myAJAX.Call();
}

function RepopulateRegionMunicipalityCity(postCode, ddRegionId, ddMunicipalityId, ddCityId) {
    var url = "AddEditCompany.aspx?AjaxMethod=JSRepopulateRegionMunicipalityCity";
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
    var url = "AddEditCompany.aspx?AjaxMethod=JSRepopulatePostCodeAndDistrict";
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

        ddDistrict_Changed();
    }

    var myAJAX = new AJAX(url, true, params, RepopulatePostCodeAndDistrict_Callback);
    myAJAX.Call();
}

//When chaning the District then repopulate the PostCode
function ddDistrict_Changed() {
    var ddPermDistrict = document.getElementById("ddlDistrict");
    var ddCity = document.getElementById("ddlCity");
    RepopulateDistrictPostCode(ddPermDistrict.value, ddCity.value, "txtPostCode");
}

function RepopulateDistrictPostCode(districtId, cityId, txtPostCodeId) {
    var url = "AddEditCompany.aspx?AjaxMethod=JSRepopulateDistrictPostCode";
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

function RepopulateCity(municipalityId, ddCityId) {
    var url = "AddEditCompany.aspx?AjaxMethod=JSRepopulateCity";
    var params = "";
    params += "MunicipalityId=" + municipalityId;
    
    function RepopulateCity_Callback(xml)
    {
        ClearSelectList(document.getElementById(ddCityId), true);

        var cities = xml.getElementsByTagName("c");

        for (var i = 0; i < cities.length; i++) {
            var id = xmlValue(cities[i], "id");
            var name = xmlValue(cities[i], "name");

            AddToSelectList(document.getElementById(ddCityId), id, name);
        };

        ddCity_Changed();
    }

    var myAJAX = new AJAX(url, true, params, RepopulateCity_Callback);
    myAJAX.Call();
}

function RepopulateRegionMunicipalityCityDistrict(postCode, ddRegionId, ddMunicipalityId, ddCityId, ddDistrictId)
{
    var url = "AddEditCompany.aspx?AjaxMethod=JSRepopulateRegionMunicipalityCityDistrict";
    var params = "";
    params += "HdnCompanyId=" + document.getElementById(hdnCompanyIDClientID).value;
    params += "&PostCode=" + postCode;
    
    function RepopulateRegionMunicipalityCityDistrict_Callback(xml)
    {
        var cityId = parseInt(xmlValue(xml, "cityId"));
        
        //Not found
        if (cityId == 0)
        {
            /*
            document.getElementById(ddRegionId).selectedIndex = 0;

            ClearSelectList(document.getElementById(ddMunicipalityId), false);
            ClearSelectList(document.getElementById(ddCityId), false);
            ClearSelectList(document.getElementById(ddDistrictId), false);
            */
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

    var myAJAX = new AJAX(url, true, params, RepopulateRegionMunicipalityCityDistrict_Callback);
    myAJAX.Call();
}


function RepopulateRegionMunicipalityCityDistrictByCityId(cityId, ddRegionId, ddMunicipalityId, ddCityId, ddDistrictId, repopulateCallback)
{
    var url = "AddEditCompany.aspx?AjaxMethod=JSRepopulateRegionMunicipalityCityDistrictByCityId";
    var params = "";
    params += "HdnCompanyId=" + document.getElementById(hdnCompanyIDClientID).value;
    params += "&CityId=" + cityId;
    
    function RepopulateRegionMunicipalityCityDistrictByCityId_Callback(xml)
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

            if (repopulateCallback != null)
                repopulateCallback();
        }
    }

    var myAJAX = new AJAX(url, true, params, RepopulateRegionMunicipalityCityDistrictByCityId_Callback);
    myAJAX.Call();
}