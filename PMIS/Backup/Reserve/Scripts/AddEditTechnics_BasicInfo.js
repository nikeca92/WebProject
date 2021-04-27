//When changing the Residence Region then refresh the Residence Municipality and the Residence City
function ddResidenceRegion_Changed()
{
    var ddResidenceRegion = document.getElementById("ddResidenceRegion");
    RepopulateMunicipality(ddResidenceRegion.value, "ddResidenceMunicipality");
}

//When changing the Residence Municipality then refresh the Residence City
function ddResidenceMunicipality_Changed()
{
    var ddResidenceMunicipality = document.getElementById("ddResidenceMunicipality");
    RepopulateCity(ddResidenceMunicipality.value, "ddResidenceCity");
}

//When changing the Residence City then refresh the Residence Post Code
function ddResidenceCity_Changed()
{
    var ddResidenceCity = document.getElementById("ddResidenceCity");
    RepopulatePostCodeAndDistrict(ddResidenceCity.value, "txtResidencePostCode", "ddResidenceDistrict");
}

//When changing the PostCode then repopulate the Region, the Municipality and the City
function txtResidencePostCode_Focus()
{
    var txtResidencePostCode = document.getElementById("txtResidencePostCode");
    txtResidencePostCode.setAttribute("oldvalue", txtResidencePostCode.value);
}

function txtResidencePostCode_Blur()
{
    var txtResidencePostCode = document.getElementById("txtResidencePostCode");

    if (txtResidencePostCode.value != txtResidencePostCode.getAttribute("oldvalue"))
    {
        RepopulateRegionMunicipalityCityDistrict(txtResidencePostCode.value, "ddResidenceRegion", "ddResidenceMunicipality", "ddResidenceCity", "ddResidenceDistrict");
    }
}

function RepopulatePostCodeAndDistrict(cityId, txtPostCodeId, ddDistrictsId)
{
    var url = "AddEditTechnics_BasicInfo.aspx?AjaxMethod=JSRepopulatePostCodeAndDistrict";
    var params = "";
    params += "CityId=" + cityId;
    
    function RepopulatePostCodeAndDistrict_Callback(xml)
    {
        var cityPostCode = xmlValue(xml, "cityPostCode");

        document.getElementById(txtPostCodeId).value = cityPostCode;

        ClearSelectList(document.getElementById(ddDistrictsId), true);

        var districts = xml.getElementsByTagName("d");

        for (var i = 0; i < districts.length; i++)
        {
            var id = xmlValue(districts[i], "id");
            var name = xmlValue(districts[i], "name");

            AddToSelectList(document.getElementById(ddDistrictsId), id, name);
        };

        if (ddDistrictsId == "ddResidenceDistrict")
            ddResidenceDistrict_Changed();
    }

    var myAJAX = new AJAX(url, true, params, RepopulatePostCodeAndDistrict_Callback);
    myAJAX.Call();
}

//When chaning the District then repopulate the PostCode
function ddResidenceDistrict_Changed() {
    var ddResidenceDistrict = document.getElementById("ddResidenceDistrict");
    var ddResidenceCity = document.getElementById("ddResidenceCity");
    RepopulateDistrictPostCode(ddResidenceDistrict.value, ddResidenceCity.value, "txtResidencePostCode");
}

function RepopulateDistrictPostCode(districtId, cityId, txtPostCodeId) {
    var url = "AddEditTechnics_BasicInfo.aspx?AjaxMethod=JSRepopulateDistrictPostCode";
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


function RepopulateRegionMunicipalityCityDistrict(postCode, ddRegionId, ddMunicipalityId, ddCityId, ddDistrictId)
{
    var url = "AddEditTechnics_BasicInfo.aspx?AjaxMethod=JSRepopulateRegionMunicipalityCityDistrict";
    var params = "";
    params += "PostCode=" + postCode;
    
    function RepopulateRegionMunicipalityCity_Callback(xml)
    {
        var cityId = parseInt(xmlValue(xml, "cityId"));

        //Not found
        if (cityId == 0)
        {
            // now, when maintain separate post code, we don't clear this controls in case user has enter specific post code
            
            //document.getElementById(ddRegionId).selectedIndex = 0;

            //ClearSelectList(document.getElementById(ddMunicipalityId), false);
            //ClearSelectList(document.getElementById(ddCityId), false);
            //ClearSelectList(document.getElementById(ddDistrictId), false);
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

function RepopulateMunicipality(regionId, ddMunicipalityId)
{
    var url = "AddEditTechnics_BasicInfo.aspx?AjaxMethod=JSRepopulateMunicipality";
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
        else if (ddMunicipalityId == "ddResidenceMunicipality")
            ddResidenceMunicipality_Changed();
    }

    var myAJAX = new AJAX(url, true, params, RepopulateMunicipality_Callback);
    myAJAX.Call();
}

function RepopulateCity(municipalityId, ddCityId)
{
    var url = "AddEditTechnics_BasicInfo.aspx?AjaxMethod=JSRepopulateCity";
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
        else if (ddCityId == "ddResidenceCity")
            ddResidenceCity_Changed();
    }

    var myAJAX = new AJAX(url, true, params, RepopulateMunicipality_Callback);
    myAJAX.Call();
}

function ActualCopyOwnerAddressToResidence() {
    var ownershipCompanyID = document.getElementById("hdnCompanyID").value;

    var url = "AddEditTechnics_BasicInfo.aspx?AjaxMethod=JSGetAddressInfoForCompany";
    var params = "";
    params += "TechnicsId=" + document.getElementById(hdnTechnicsIdClientID).value;
    params += "&CompanyId=" + ownershipCompanyID;

    function JSGetAddressInfoForCompany_CallBack(xml) {
        var addressInfo = xml.getElementsByTagName("addressInfo")[0];

        var cityId = xmlValue(addressInfo, "cityId");
        var postCode = xmlValue(addressInfo, "postCode");
        var regionId = xmlValue(addressInfo, "regionId");
        var municipalityId = xmlValue(addressInfo, "municipalityId");
        var districtId = xmlValue(addressInfo, "districtId");
        var address = xmlValue(addressInfo, "address");
        
        document.getElementById("txtResidencePostCode").value = postCode;

        //If there is a Residence City then load the entire info
        if (cityId != optionChooseOneValue) {
            document.getElementById("ddResidenceRegion").value = regionId;

            ClearSelectList(document.getElementById("ddResidenceMunicipality"), true);

            var r_municipalities = xml.getElementsByTagName("r_m");

            for (var i = 0; i < r_municipalities.length; i++) {
                var id = xmlValue(r_municipalities[i], "id");
                var name = xmlValue(r_municipalities[i], "name");

                AddToSelectList(document.getElementById("ddResidenceMunicipality"), id, name);
            };

            document.getElementById("ddResidenceMunicipality").value = municipalityId;


            ClearSelectList(document.getElementById("ddResidenceCity"), true);

            var r_cities = xml.getElementsByTagName("r_c");

            for (var i = 0; i < r_cities.length; i++) {
                var id = xmlValue(r_cities[i], "id");
                var name = xmlValue(r_cities[i], "name");

                AddToSelectList(document.getElementById("ddResidenceCity"), id, name);
            };

            document.getElementById("ddResidenceCity").value = cityId;


            ClearSelectList(document.getElementById("ddResidenceDistrict"), true);

            var r_districts = xml.getElementsByTagName("r_d");

            for (var i = 0; i < r_districts.length; i++) {
                var id = xmlValue(r_districts[i], "id");
                var name = xmlValue(r_districts[i], "name");

                AddToSelectList(document.getElementById("ddResidenceDistrict"), id, name);
            };

            if (districtId != optionChooseOneValue)
                document.getElementById("ddResidenceDistrict").value = districtId;
        }
        else {
            document.getElementById("ddResidenceRegion").selectedIndex = 0;
            ClearSelectList(document.getElementById("ddResidenceMunicipality"), false);
            ClearSelectList(document.getElementById("ddResidenceCity"), false);
            ClearSelectList(document.getElementById("ddResidenceDistrict"), false);
        }

        document.getElementById("txtResidenceAddress").value = address;
    }

    var myAJAX = new AJAX(url, true, params, JSGetAddressInfoForCompany_CallBack);
    myAJAX.Call();
}

function CopyOwnerAddressToResidence()
{
    if (!document.getElementById("btnTabOwner").getAttribute("isalrеadyvisited") ||
        document.getElementById("btnTabOwner").getAttribute("isalrеadyvisited") != "true")
    {
        var url = "AddEditTechnics.aspx?AjaxMethod=JSLoadTab&TechnicsId=" + document.getElementById(hdnTechnicsIdClientID).value +
                      "&TechnicsTypeKey=" + document.getElementById(hdnTechnicsTypeKeyClientID).value;
        var params = "";
        params += "SelectedTabId=" + "btnTabOwner";

        function JSLoadTab_CallBack(xml, selectedTabId)
        {
            var targetDivId = GetTargetDivByTabId(selectedTabId);
            document.getElementById(targetDivId).innerHTML = xmlValue(xml, "TabHTML");

            ActualCopyOwnerAddressToResidence();
        }

        var myAJAX = new AJAX(url, true, params, function(xml) { JSLoadTab_CallBack(xml, "btnTabOwner"); });
        myAJAX.Call();
    }
    else
        ActualCopyOwnerAddressToResidence();
}

function ddNormativeTechnics_Changed()
{
    var url = "AddEditTechnics_BasicInfo.aspx?AjaxMethod=JSGetNormativeTechnicsCode";
    var params = "";
    params += "NormativeTechnicsId=" + document.getElementById("ddNormativeTechnics").value;
    var myAJAX = new AJAX(url, true, params, GetNormativeTechnicsCode_Callback);
    myAJAX.Call();

    function GetNormativeTechnicsCode_Callback(xml)
    {
        var normativeCode = xmlValue(xml, "normativeCode");
        document.getElementById("txtNormativeCode").value = normativeCode;
    }
}

function txtNormativeCode_Focus()
{
    var txtNormativeCode = document.getElementById("txtNormativeCode");
    txtNormativeCode.setAttribute("oldvalue", txtNormativeCode.value);
}

function txtNormativeCode_Blur()
{
    var txtNormativeCode = document.getElementById("txtNormativeCode");

    if (txtNormativeCode.value != txtNormativeCode.getAttribute("oldvalue"))
    {
        if (txtNormativeCode.value == "")
        {
            document.getElementById("ddNormativeTechnics").value = optionChooseOneValue;
        }
        else
        {
            var url = "AddEditTechnics_BasicInfo.aspx?AjaxMethod=JSGetNormativeTechnicsId";
            var params = "";
            params += "NormativeCode=" + txtNormativeCode.value;
            params += "&TechnicsTypeKey=" + document.getElementById(hdnTechnicsTypeKeyClientID).value;
            
            function GetNormativeTechnicsId_Callback(xml)
            {
                var normativeTechnicsId = xmlValue(xml, "normativeTechnicsId");

                if (parseInt(normativeTechnicsId) > 0 && DoesDropDownContainValue("ddNormativeTechnics", normativeTechnicsId))
                {
                    document.getElementById("ddNormativeTechnics").value = normativeTechnicsId;
                }
                else
                {
                    txtNormativeCode.value = txtNormativeCode.getAttribute("oldvalue");
                }
            }

            var myAJAX = new AJAX(url, true, params, GetNormativeTechnicsId_Callback);
            myAJAX.Call();
        }
    }
}

function Refresh_MilitaryReport_Postpone_TechnicsSubTypeInfo() {
    //If the dom element(s) are not there then the tab was not opened yet and then there is no need to reload
    if (!document.getElementById("lblPostponeTechnicsSubTypeNameValue"))
        return;

    var url = "AddEditTechnics_BasicInfo.aspx?AjaxMethod=JSGetTechnicsSubTypeInfo";
    var params = "";
    params += "TechnicsId=" + document.getElementById(hdnTechnicsIdClientID).value;
    
    function JSGetWorkplaceInfo_Callback(xml) {
        var technicsSubTypeInfo = xmlValue(xml, "technicsSubTypeInfo");

        document.getElementById("lblPostponeTechnicsSubTypeNameValue").innerHTML = technicsSubTypeInfo;
        document.getElementById("lblPostponeTechnicsSubTypeNameValueLightBox").innerHTML = technicsSubTypeInfo;
    }

    var myAJAX = new AJAX(url, true, params, JSGetWorkplaceInfo_Callback);
    myAJAX.Call();
}