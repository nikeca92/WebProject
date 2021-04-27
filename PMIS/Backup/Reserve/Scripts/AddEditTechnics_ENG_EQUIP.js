var isNewTechnics = true;

function TabLoaded_BasicInfo()
{
    SetClientTextAreaMaxLength("txtResidenceAddress", "1500");
}

//This function load the technics's details by TechnicsId
function LoadBasicInfoByTechnicsId()
{
    ClearAllMessages();

    var url = "AddEditTechnics_ENG_EQUIP.aspx?AjaxMethod=JSLoadBasicInfo";
    var params = "";
    params += "TechnicsId=" + document.getElementById(hdnTechnicsIdClientID).value;
    
    function LoadBasicInfoByTechnicsId_CallBack(xml)
    {
        var engEquip = xml.getElementsByTagName("engEquip")[0];

        var technicsId = xmlValue(engEquip, "technicsId");
        if (technicsId != 0)
        {
            isNewTechnics = false;
        }
        else
        {
            isNewTechnics = true;
        }

        var engEquipId = xmlValue(engEquip, "engEquipId");
        var regNumber = xmlValue(engEquip, "regNumber");
        var inventoryNumber = xmlValue(engEquip, "inventoryNumber");
        var technicsCategoryId = xmlValue(engEquip, "technicsCategoryId");
        var engEquipKindId = xmlValue(engEquip, "engEquipKindId");
        var engEquipTypeId = xmlValue(engEquip, "engEquipTypeId");
        var lastModified = xmlValue(engEquip, "lastModified");
        var resMilRepStatus = xmlValue(engEquip, "resMilRepStatus");
        
        
//        var engEquipBaseMakeId = xmlValue(engEquip, "engEquipBaseMakeId");
//        var engEquipBaseModelId = xmlValue(engEquip, "engEquipBaseModelId");

        var engEquipBaseMakeName = xmlValue(engEquip, "engEquipBaseMakeName");
        var engEquipBaseModelName = xmlValue(engEquip, "engEquipBaseModelName");
        
        var engEquipBaseKindId = xmlValue(engEquip, "engEquipBaseKindId");
        var engEquipBaseTypeId = xmlValue(engEquip, "engEquipBaseTypeId");
        var engEquipBaseEngineTypeId = xmlValue(engEquip, "engEquipBaseEngineTypeId");
        var baseFirstRegDate = xmlValue(engEquip, "baseFirstRegDate");
        var baseMileage = xmlValue(engEquip, "baseMileage");
        var workingBodyPerformancePerHour = xmlValue(engEquip, "workingBodyPerformancePerHour");
        var engEquipWorkingBodyKindId = xmlValue(engEquip, "engEquipWorkingBodyKindId");
        var workingBodyFirstRegDate = xmlValue(engEquip, "workingBodyFirstRegDate");
        var engEquipWorkBodyEngineTypeId = xmlValue(engEquip, "engEquipWorkBodyEngineTypeId");
        var residenceCityId = xmlValue(engEquip, "residenceCityId");
        var residencePostCode = xmlValue(engEquip, "residencePostCode");
        var residenceRegionId = xmlValue(engEquip, "residenceRegionId");
        var residenceMunicipalityId = xmlValue(engEquip, "residenceMunicipalityId");
        var residenceDistrictId = xmlValue(engEquip, "residenceDistrictId");
        var residenceAddress = xmlValue(engEquip, "residenceAddress");
        var currMilDepartment = xmlValue(engEquip, "currMilDepartment");
        var normativeTechnicsId = xmlValue(engEquip, "normativeTechnicsId");
        var normativeCode = xmlValue(engEquip, "normativeCode")
        
        document.getElementById(hdnTechnicsIdClientID).value = technicsId;

        if (document.getElementById("hdnEngEquipId"))
        {
            document.getElementById("lblCurrMilDepartmentValue").innerHTML = currMilDepartment;
        
            document.getElementById("hdnEngEquipId").value = engEquipId;
            document.getElementById("txtRegNumber").value = regNumber;
            document.getElementById("lblRegNumberValue").innerHTML = regNumber;
            document.getElementById("txtInventoryNumber").value = inventoryNumber;
            document.getElementById("ddTechnicsCategory").value = technicsCategoryId;
            document.getElementById("ddEngEquipKind").value = engEquipKindId;
            document.getElementById("ddEngEquipType").value = engEquipTypeId;

            document.getElementById("lblGeneralTabCurrMilitaryReportStatusValue").innerHTML = resMilRepStatus;
            document.getElementById("lblLastModifiedValue").innerHTML = lastModified;

            
//            document.getElementById("ddEngEquipBaseMake").value = engEquipBaseMakeId;

//            ClearSelectList(document.getElementById("ddEngEquipBaseModel"), true);

//            var eeb_models = xml.getElementsByTagName("eeb_model");

//            for (var i = 0; i < eeb_models.length; i++)
//            {
//                var id = xmlValue(eeb_models[i], "id");
//                var name = xmlValue(eeb_models[i], "name");

//                AddToSelectList(document.getElementById("ddEngEquipBaseModel"), id, name);
//            };

//            document.getElementById("ddEngEquipBaseModel").value = engEquipBaseModelId;

            document.getElementById("txtEngEquipBaseMakeName").value = engEquipBaseMakeName;
            document.getElementById("txtEngEquipBaseModelName").value = engEquipBaseModelName;

            document.getElementById("ddEngEquipBaseKind").value = engEquipBaseKindId;
            document.getElementById("ddEngEquipBaseType").value = engEquipBaseTypeId;
            document.getElementById("ddEngEquipBaseEngineType").value = engEquipBaseEngineTypeId;
            document.getElementById("txtBaseFirstRegDate").value = baseFirstRegDate;
            document.getElementById("txtBaseMileage").value = baseMileage;
            document.getElementById("txtWorkingBodyPerformancePerHour").value = workingBodyPerformancePerHour;
            document.getElementById("ddEngEquipWorkingBodyKind").value = engEquipWorkingBodyKindId;
            document.getElementById("txtWorkingBodyFirstRegDate").value = workingBodyFirstRegDate;
            document.getElementById("ddEngEquipWorkBodyEngineType").value = engEquipWorkBodyEngineTypeId;

            document.getElementById("txtResidencePostCode").value = residencePostCode;
            
            //If there is a Residence City then load the entire info
            if (residenceCityId != optionChooseOneValue)
            {                
                document.getElementById("ddResidenceRegion").value = residenceRegionId;

                ClearSelectList(document.getElementById("ddResidenceMunicipality"), true);

                var r_municipalities = xml.getElementsByTagName("r_m");

                for (var i = 0; i < r_municipalities.length; i++)
                {
                    var id = xmlValue(r_municipalities[i], "id");
                    var name = xmlValue(r_municipalities[i], "name");

                    AddToSelectList(document.getElementById("ddResidenceMunicipality"), id, name);
                };

                document.getElementById("ddResidenceMunicipality").value = residenceMunicipalityId;


                ClearSelectList(document.getElementById("ddResidenceCity"), true);

                var r_cities = xml.getElementsByTagName("r_c");

                for (var i = 0; i < r_cities.length; i++)
                {
                    var id = xmlValue(r_cities[i], "id");
                    var name = xmlValue(r_cities[i], "name");

                    AddToSelectList(document.getElementById("ddResidenceCity"), id, name);
                };

                document.getElementById("ddResidenceCity").value = residenceCityId;


                ClearSelectList(document.getElementById("ddResidenceDistrict"), true);

                var r_districts = xml.getElementsByTagName("r_d");

                for (var i = 0; i < r_districts.length; i++)
                {
                    var id = xmlValue(r_districts[i], "id");
                    var name = xmlValue(r_districts[i], "name");

                    AddToSelectList(document.getElementById("ddResidenceDistrict"), id, name);
                };

                if (residenceDistrictId != optionChooseOneValue)
                    document.getElementById("ddResidenceDistrict").value = residenceDistrictId;
            }
            else
            {               
                document.getElementById("ddResidenceRegion").selectedIndex = 0;
                ClearSelectList(document.getElementById("ddResidenceMunicipality"), false);
                ClearSelectList(document.getElementById("ddResidenceCity"), false);
                ClearSelectList(document.getElementById("ddResidenceDistrict"), false);
            }

            document.getElementById("txtResidenceAddress").value = residenceAddress;
            document.getElementById("ddNormativeTechnics").value = normativeTechnicsId;
            document.getElementById("txtNormativeCode").value = normativeCode;
        }
        
        isLoadedBasicInfo = true;
        LoadOriginalValues();
        ShowContent();
    }

    var myAJAX = new AJAX(url, true, params, LoadBasicInfoByTechnicsId_CallBack);
    myAJAX.Call();
}

//Save the basic information
function SaveBasicInfo(saveBasicInfoFinishCallback)
{
    if (IsTabAlreadyVisited("btnTabBasicInfo")) {
    
        document.getElementById("txtRegNumber").value = RemoveWhiteSpaces(document.getElementById("txtRegNumber").value);

        var url = "AddEditTechnics_ENG_EQUIP.aspx?AjaxMethod=JSSaveBasicInfo";
        var params = "";

        params += "TechnicsId=" + document.getElementById(hdnTechnicsIdClientID).value;
        params += "&EngEquipId=" + document.getElementById("hdnEngEquipId").value;

        params += "&RegNumber=" + custEncodeURI(document.getElementById("txtRegNumber").value);
        params += "&InventoryNumber=" + custEncodeURI(document.getElementById("txtInventoryNumber").value);
        params += "&TechnicsCategoryId=" + custEncodeURI(document.getElementById("ddTechnicsCategory").value);
        params += "&EngEquipKindId=" + custEncodeURI(document.getElementById("ddEngEquipKind").value);
        params += "&EngEquipTypeId=" + custEncodeURI(document.getElementById("ddEngEquipType").value);
        
    //        params += "&EngEquipBaseMakeId=" + custEncodeURI(document.getElementById("ddEngEquipBaseMake").value);
    //        params += "&EngEquipBaseModelId=" + custEncodeURI(document.getElementById("ddEngEquipBaseModel").value);

        params += "&EngEquipBaseMakeName=" + custEncodeURI(document.getElementById("txtEngEquipBaseMakeName").value);
        params += "&EngEquipBaseModelName=" + custEncodeURI(document.getElementById("txtEngEquipBaseModelName").value);
        
        params += "&EngEquipBaseKindId=" + custEncodeURI(document.getElementById("ddEngEquipBaseKind").value);
        params += "&EngEquipBaseTypeId=" + custEncodeURI(document.getElementById("ddEngEquipBaseType").value);
        params += "&EngEquipBaseEngineTypeId=" + custEncodeURI(document.getElementById("ddEngEquipBaseEngineType").value);
        params += "&BaseFirstRegDate=" + custEncodeURI(document.getElementById("txtBaseFirstRegDate").value);
        params += "&BaseMileage=" + custEncodeURI(document.getElementById("txtBaseMileage").value);
        params += "&WorkingBodyPerformancePerHour=" + custEncodeURI(document.getElementById("txtWorkingBodyPerformancePerHour").value);
        params += "&EngEquipWorkingBodyKindId=" + custEncodeURI(document.getElementById("ddEngEquipWorkingBodyKind").value);
        params += "&WorkingBodyFirstRegDate=" + custEncodeURI(document.getElementById("txtWorkingBodyFirstRegDate").value);
        params += "&EngEquipWorkBodyEngineTypeId=" + custEncodeURI(document.getElementById("ddEngEquipWorkBodyEngineType").value);
        params += "&ResidencePostCode=" + document.getElementById("txtResidencePostCode").value;
        params += "&ResidenceCityID=" + document.getElementById("ddResidenceCity").value;
        params += "&ResidenceDistrictID=" + document.getElementById("ddResidenceDistrict").value;
        params += "&ResidenceAddress=" + custEncodeURI(document.getElementById("txtResidenceAddress").value);
        params += "&NormativeTechnicsId=" + custEncodeURI(document.getElementById("ddNormativeTechnics").value);

        var myAJAX = new AJAX(url, true, params, SaveBasicInfo_Callback);
        myAJAX.Call();
    } else {
        saveBasicInfoFinishCallback();
    }
    
    function SaveBasicInfo_Callback(xml)
    {
        var status = xmlValue(xml, "response");
        var technicsId = xmlValue(xml, "technicsId");
        var engEquipId = xmlValue(xml, "engEquipId");

        if (document.getElementById(hdnTechnicsIdClientID).value == 0)
        {
            document.getElementById(hdnTechnicsIdClientID).value = technicsId;
            document.getElementById("hdnEngEquipId").value = engEquipId;

            location.hash = "AddEditTechnics.aspx?TechnicsId=" + technicsId;
        }
        else
        {
            document.getElementById(hdnTechnicsIdClientID).value = technicsId;
            document.getElementById("hdnEngEquipId").value = engEquipId;

        }

        RefreshInputsOfSpecificContainer(document.getElementById(divBasicInfoClientID), true);
        RefreshInputsOfSpecificContainer(document.getElementById("tblBasicInfoHeader"), true);
        Refresh_MilitaryReport_Postpone_TechnicsSubTypeInfo();
        saveBasicInfoFinishCallback();
    }
}

//Check if the enetered basic information is valid
function IsBasicInfoValid()
{
    var tabNameHeader = "Основни данни: ";
    var ValidationMessage = "";

    if (IsTabAlreadyVisited("btnTabBasicInfo")) {
        var notValidFields = new Array();

        if (document.getElementById("ddNormativeTechnics").value == optionChooseOneValue &&
        document.getElementById("ddNormativeTechnics").className == "RequiredInputField") {

            if (document.getElementById("ddNormativeTechnics").disabled == true || document.getElementById("ddNormativeTechnics").style.display == "none") {
                notValidFields.push("Нормативна категория");
            }
            else {
                ValidationMessage += tabNameHeader + GetErrorMessageMandatory("Нормативна категория") + "</br>";
            }
        }
        
        if (isNewTechnics) {
            if (document.getElementById("txtRegNumber").value.Trim() == "") {
               
                if (document.getElementById("txtRegNumber").disabled == true || document.getElementById("txtRegNumber").style.display == "none" || document.getElementById("txtRegNumberCont").style.display == "none") {
                    notValidFields.push("Регистрационен номер");
                }
                else {
                    ValidationMessage += tabNameHeader + GetErrorMessageMandatory("Регистрационен номер") + "</br>";
                }
            }
        }

        if (document.getElementById("txtBaseFirstRegDate").value.Trim() != "") {
            if (!IsValidDate(document.getElementById("txtBaseFirstRegDate").value)) {
                ValidationMessage += tabNameHeader + GetErrorMessageDate("Дата на първата регистрация (Базова машина)") + "</br>";
            }
        }

        if (document.getElementById("txtBaseMileage").value.Trim() != "") {
            if (!isDecimal(document.getElementById("txtBaseMileage").value)) {
                ValidationMessage += tabNameHeader + GetErrorMessageNumber("Изминати километри (Базова машина)") + "</br>";
            }
        }

        if (document.getElementById("txtWorkingBodyPerformancePerHour").value.Trim() != "") {
            if (!isDecimal(document.getElementById("txtWorkingBodyPerformancePerHour").value)) {
                ValidationMessage += tabNameHeader + GetErrorMessageNumber("Производителност за час (Работен орган)") + "</br>";
            }
        }


        if (document.getElementById("txtWorkingBodyFirstRegDate").value.Trim() != "") {
            if (!IsValidDate(document.getElementById("txtWorkingBodyFirstRegDate").value)) {
                ValidationMessage += tabNameHeader + GetErrorMessageDate("Дата на първата регистрация (Работен орган)") + "</br>";
            }
        }

        var notValidFieldsCount = notValidFields.length;

        if (notValidFieldsCount > 0) {
            var noRightsMessage = GetErrorMessageNoRights(notValidFields);
            ValidationMessage += tabNameHeader + noRightsMessage + "<br />";
        }
    }
    return ValidationMessage;
}

//If a new Technics has been added the go to edit mode with this function
function GoToEditMode()
{
    ClearAllMessages();    

    if (document.getElementById("btnTabMilitaryReport"))
        document.getElementById("btnTabMilitaryReport").style.display = "";

    if (document.getElementById("btnTabOwner"))
        document.getElementById("btnTabOwner").style.display = "";

    if (document.getElementById("btnTabOtherInfo"))
        document.getElementById("btnTabOtherInfo").style.display = "";

    document.getElementById("txtRegNumber").style.display = "none";
    document.getElementById("lblRegNumberValue").style.display = "";
    document.getElementById("imgEditRegNumber").style.display = "";
    document.getElementById("imgHistoryRegNumber").style.display = "";
    document.getElementById("lblRegNumberValue").innerHTML = document.getElementById("txtRegNumber").value;

    document.getElementById("btnImgCopyOwnerAddress").style.display = "";

    document.getElementById(lblHeaderTitleClientID).innerHTML = "Редактиране на запис от " + document.getElementById(hdnTechnicsTypeNameClientID).value;
    document.title = document.getElementById(lblHeaderTitleClientID).innerHTML;
}

//Clear the message on this tab
function ClearBasicInfoMessages()
{
    if (document.getElementById("lblBasicInfoMessage"))
        document.getElementById("lblBasicInfoMessage").innerHTML = "";
}


//Check the reg number only when it is changed
function RegNumberFocus()
{
    var txtRegNumber = document.getElementById("txtRegNumber");
    txtRegNumber.setAttribute("oldvalue", txtRegNumber.value);
}

function InventoryNumberBlur() {
    var txtInventoryNumber = document.getElementById("txtInventoryNumber");
    txtInventoryNumber.value = AdjustRegInv(txtInventoryNumber.value);
}

function NewRegNumberBlur() {
    var txtNewRegNumber = document.getElementById("txtNewRegNumber");
    txtNewRegNumber.value = AdjustRegInv(txtNewRegNumber.value);
}

//When the user type an RegNumber then check if this is an existing EngEquip and
//take care about this
function RegNumberBlur()
{
    var txtRegNumber = document.getElementById("txtRegNumber");
    txtRegNumber.value = AdjustRegInv(txtRegNumber.value);
    
    if (txtRegNumber.value != txtRegNumber.getAttribute("oldvalue"))
    {
        var url = "AddEditTechnics_ENG_EQUIP.aspx?AjaxMethod=JSCheckRegNumber";
        var params = "";
        params += "RegNumber=" + document.getElementById("txtRegNumber").value;
        
        function RegNumberBlur_Callback(xml)
        {
            var technicsId = parseInt(xmlValue(xml, "technicsId"));
            
            //Redirect to the existing Technics record
            if (technicsId > 0)
            {
                JSRedirect("AddEditTechnics.aspx?TechnicsId=" + technicsId);
            }
        }

        var myAJAX = new AJAX(url, true, params, RegNumberBlur_Callback);
        myAJAX.Call();
    }
}

//function RepopulateEngEquipBaseModels(engEquipBaseMakeId)
//{
//    var url = "AddEditTechnics_ENG_EQUIP.aspx?AjaxMethod=JSRepopulateEngEquipBaseModels";
//    var params = "";
//    params += "EngEquipBaseMakeId=" + engEquipBaseMakeId;
//    var myAJAX = new AJAX(url, true, params, RepopulateEngEquipBaseModels_Callback);
//    myAJAX.Call();

//    function RepopulateEngEquipBaseModels_Callback(xml)
//    {
//        ClearSelectList(document.getElementById("ddEngEquipBaseModel"), true);

//        var models = xml.getElementsByTagName("m");

//        for (var i = 0; i < models.length; i++)
//        {
//            var id = xmlValue(models[i], "id");
//            var name = xmlValue(models[i], "name");

//            AddToSelectList(document.getElementById("ddEngEquipBaseModel"), id, name);
//        };
//    }
//}

//Refresh the EngEquipKind list when updating the list via the GTable maintenance
function RefreshEngEquipKindList()
{
    var url = "AddEditTechnics_ENG_EQUIP.aspx?AjaxMethod=JSRefreshEngEquipKindList";
    var params = "";
    
    function RefreshEngEquipKindList_Callback(xml)
    {
        var currentValue = document.getElementById("ddEngEquipKind").value;

        ClearSelectList(document.getElementById("ddEngEquipKind"), true);

        var items = xml.getElementsByTagName("i");
        var found = false;

        for (var i = 0; i < items.length; i++)
        {
            var id = xmlValue(items[i], "id");
            var name = xmlValue(items[i], "name");

            if (currentValue == id)
                found = true;

            AddToSelectList(document.getElementById("ddEngEquipKind"), id, name);
        };

        if (found)
            document.getElementById("ddEngEquipKind").value = currentValue;
    }

    var myAJAX = new AJAX(url, true, params, RefreshEngEquipKindList_Callback);
    myAJAX.Call();
}


//Refresh the EngEquipType list when updating the list via the GTable maintenance
function RefreshEngEquipTypeList()
{
    var url = "AddEditTechnics_ENG_EQUIP.aspx?AjaxMethod=JSRefreshEngEquipTypeList";
    var params = "";
    
    function RefreshEngEquipTypeList_Callback(xml)
    {
        var currentValue = document.getElementById("ddEngEquipType").value;

        ClearSelectList(document.getElementById("ddEngEquipType"), true);

        var items = xml.getElementsByTagName("i");
        var found = false;

        for (var i = 0; i < items.length; i++)
        {
            var id = xmlValue(items[i], "id");
            var name = xmlValue(items[i], "name");

            if (currentValue == id)
                found = true;

            AddToSelectList(document.getElementById("ddEngEquipType"), id, name);
        };

        if (found)
            document.getElementById("ddEngEquipType").value = currentValue;
    }

    var myAJAX = new AJAX(url, true, params, RefreshEngEquipTypeList_Callback);
    myAJAX.Call();
}

//Refresh the EngEquipBaseKind list when updating the list via the GTable maintenance
function RefreshEngEquipBaseKindList()
{
    var url = "AddEditTechnics_ENG_EQUIP.aspx?AjaxMethod=JSRefreshEngEquipBaseKindList";
    var params = "";
    
    function RefreshEngEquipBaseKindList_Callback(xml)
    {
        var currentValue = document.getElementById("ddEngEquipBaseKind").value;

        ClearSelectList(document.getElementById("ddEngEquipBaseKind"), true);

        var items = xml.getElementsByTagName("i");
        var found = false;

        for (var i = 0; i < items.length; i++)
        {
            var id = xmlValue(items[i], "id");
            var name = xmlValue(items[i], "name");

            if (currentValue == id)
                found = true;

            AddToSelectList(document.getElementById("ddEngEquipBaseKind"), id, name);
        };

        if (found)
            document.getElementById("ddEngEquipBaseKind").value = currentValue;
    }

    var myAJAX = new AJAX(url, true, params, RefreshEngEquipBaseKindList_Callback);
    myAJAX.Call();
}

//Refresh the EngEquipBaseType list when updating the list via the GTable maintenance
function RefreshEngEquipBaseTypeList()
{
    var url = "AddEditTechnics_ENG_EQUIP.aspx?AjaxMethod=JSRefreshEngEquipBaseTypeList";
    var params = "";
    
    function RefreshEngEquipBaseTypeList_Callback(xml)
    {
        var currentValue = document.getElementById("ddEngEquipBaseType").value;

        ClearSelectList(document.getElementById("ddEngEquipBaseType"), true);

        var items = xml.getElementsByTagName("i");
        var found = false;

        for (var i = 0; i < items.length; i++)
        {
            var id = xmlValue(items[i], "id");
            var name = xmlValue(items[i], "name");

            if (currentValue == id)
                found = true;

            AddToSelectList(document.getElementById("ddEngEquipBaseType"), id, name);
        };

        if (found)
            document.getElementById("ddEngEquipBaseType").value = currentValue;
    }

    var myAJAX = new AJAX(url, true, params, RefreshEngEquipBaseTypeList_Callback);
    myAJAX.Call();
}

//Refresh the EngEquipBaseEngineType list when updating the list via the GTable maintenance
function RefreshEngEquipBaseEngineTypeList()
{
    var url = "AddEditTechnics_ENG_EQUIP.aspx?AjaxMethod=JSRefreshEngEquipBaseEngineTypeList";
    var params = "";
    
    function RefreshEngEquipBaseEngineTypeList_Callback(xml)
    {
        var currentValue = document.getElementById("ddEngEquipBaseEngineType").value;

        ClearSelectList(document.getElementById("ddEngEquipBaseEngineType"), true);

        var items = xml.getElementsByTagName("i");
        var found = false;

        for (var i = 0; i < items.length; i++)
        {
            var id = xmlValue(items[i], "id");
            var name = xmlValue(items[i], "name");

            if (currentValue == id)
                found = true;

            AddToSelectList(document.getElementById("ddEngEquipBaseEngineType"), id, name);
        };

        if (found)
            document.getElementById("ddEngEquipBaseEngineType").value = currentValue;
    }

    var myAJAX = new AJAX(url, true, params, RefreshEngEquipBaseEngineTypeList_Callback);
    myAJAX.Call();
}


//Refresh the EngEquipWorkingBodyKind list when updating the list via the GTable maintenance
function RefreshEngEquipWorkingBodyKindList()
{
    var url = "AddEditTechnics_ENG_EQUIP.aspx?AjaxMethod=JSRefreshEngEquipWorkingBodyKindList";
    var params = "";
    
    function RefreshEngEquipWorkingBodyKindList_Callback(xml)
    {
        var currentValue = document.getElementById("ddEngEquipWorkingBodyKind").value;

        ClearSelectList(document.getElementById("ddEngEquipWorkingBodyKind"), true);

        var items = xml.getElementsByTagName("i");
        var found = false;

        for (var i = 0; i < items.length; i++)
        {
            var id = xmlValue(items[i], "id");
            var name = xmlValue(items[i], "name");

            if (currentValue == id)
                found = true;

            AddToSelectList(document.getElementById("ddEngEquipWorkingBodyKind"), id, name);
        };

        if (found)
            document.getElementById("ddEngEquipWorkingBodyKind").value = currentValue;
    }

    var myAJAX = new AJAX(url, true, params, RefreshEngEquipWorkingBodyKindList_Callback);
    myAJAX.Call();
}

//Refresh the EngEquipWorkBodyEngineType list when updating the list via the GTable maintenance
function RefreshEngEquipWorkBodyEngineTypeList()
{
    var url = "AddEditTechnics_ENG_EQUIP.aspx?AjaxMethod=JSRefreshEngEquipWorkBodyEngineTypeList";
    var params = "";
    
    function RefreshEngEquipWorkBodyEngineTypeList_Callback(xml)
    {
        var currentValue = document.getElementById("ddEngEquipWorkBodyEngineType").value;

        ClearSelectList(document.getElementById("ddEngEquipWorkBodyEngineType"), true);

        var items = xml.getElementsByTagName("i");
        var found = false;

        for (var i = 0; i < items.length; i++)
        {
            var id = xmlValue(items[i], "id");
            var name = xmlValue(items[i], "name");

            if (currentValue == id)
                found = true;

            AddToSelectList(document.getElementById("ddEngEquipWorkBodyEngineType"), id, name);
        };

        if (found)
            document.getElementById("ddEngEquipWorkBodyEngineType").value = currentValue;
    }

    var myAJAX = new AJAX(url, true, params, RefreshEngEquipWorkBodyEngineTypeList_Callback);
    myAJAX.Call();
}


function ChangeRegNumber()
{
    ShowChangeRegNumberLightBox();
}

function ShowChangeRegNumberLightBox()
{
    ClearAllMessages();

    document.getElementById("txtNewRegNumber").value = "";
    document.getElementById("lblCurrRegNumberValue").innerHTML = document.getElementById("lblRegNumberValue").innerHTML;

    // clean message label in the light box and hide it
    document.getElementById("spanChangeRegNumberLightBoxMessage").style.display = "none";
    document.getElementById("spanChangeRegNumberLightBoxMessage").innerHTML = "";

    //shows the light box and "disable" rest of the page
    document.getElementById("HidePage").style.display = "";
    document.getElementById("ChangeRegNumberLightBox").style.display = "";
    CenterLightBox("ChangeRegNumberLightBox");
}

// Close the light box
function HideChangeRegNumberLightBox()
{
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("ChangeRegNumberLightBox").style.display = "none";
}

// Save the new reg number
function SaveChangeRegNumberLightBox()
{
    document.getElementById("txtNewRegNumber").value = RemoveWhiteSpaces(document.getElementById("txtNewRegNumber").value);
    
    if (ValidateChangeRegNumber())
    {
        var url = "AddEditTechnics_ENG_EQUIP.aspx?AjaxMethod=JSChangeRegNumber";

        var params = "EngEquipId=" + document.getElementById("hdnEngEquipId").value +
                     "&NewRegNumber=" + document.getElementById("txtNewRegNumber").value;

        function response_handler(xml)
        {
            var response = xmlValue(xml, "response");

            if (response == "OK")
            {
                document.getElementById("lblRegNumberValue").innerHTML = document.getElementById("txtNewRegNumber").value;
                document.getElementById("txtRegNumber").value = document.getElementById("txtNewRegNumber").value;

                RefreshInputsOfSpecificContainer(document.getElementById("tdRegNumber"), true);

                HideChangeRegNumberLightBox();
            }
            else
            {
                var lblMessage = document.getElementById("spanChangeRegNumberLightBoxMessage"); ;
                lblMessage.innerHTML = response;
                lblMessage.className = "ErrorText";
                lblMessage.style.display = "";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Client validation of change reg number light-box
function ValidateChangeRegNumber()
{
    var res = true;

    var lblMessage = document.getElementById("spanChangeRegNumberLightBoxMessage");
    lblMessage.innerHTML = "";

    var notValidFields = new Array();

    var txtNewRegNumber = document.getElementById("txtNewRegNumber");
    var lblCurrRegNumber = document.getElementById("lblCurrRegNumberValue");

    if (txtNewRegNumber.value.Trim() == "")
    {
        res = false;

        if (txtNewRegNumber.disabled == true || txtNewRegNumber.style.display == "none")
            notValidFields.push("Нов регистрационен номер");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Нов регистрационен номер") + "<br />";
    }
    else if (txtNewRegNumber.value.Trim() == lblCurrRegNumber.innerHTML)
    {
        res = false;
        lblMessage.innerHTML += "Новият регистрационен номер е същият като текущия" + "<br />";
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


function RegNumberHistory_Click()
{
    var url = "AddEditTechnics_ENG_EQUIP.aspx?AjaxMethod=JSLoadRegNumberHistory";

    var params = "EngEquipId=" + document.getElementById("hdnEngEquipId").value;

    function response_handler(xml)
    {
        document.getElementById("divRegNumberHistoryLightBox").innerHTML = xmlValue(xml, "response");

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("divRegNumberHistoryLightBox").style.display = "";
        CenterLightBox("divRegNumberHistoryLightBox");
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}

function HideRegNumberHistoryLightBox()
{
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("divRegNumberHistoryLightBox").style.display = "none";
    document.getElementById("divRegNumberHistoryLightBox").innerHTML = "";
}

function BtnRegNumberHistoryPagingClick(objectName)
{
    hdnPageIdx = document.getElementById('hdnPageIndex');
    hdnMaxPage = document.getElementById('hdnPageMaxPage');
    switch (objectName)
    {
        case "btnFirst":
            hdnPageIdx.value = 1;

            RefreshRegNumberHistoryLightBox();
            break;

        case "btnPrev":
            pageIdx = parseInt(hdnPageIdx.value);

            if (pageIdx > 1)
            {
                pageIdx--;
                hdnPageIdx.value = pageIdx;
                RefreshRegNumberHistoryLightBox();
            }

            break;

        case "btnNext":
            pageIdx = parseInt(hdnPageIdx.value);
            maxPage = parseInt(hdnMaxPage.value);

            if (pageIdx < maxPage)
            {
                pageIdx++;
                hdnPageIdx.value = pageIdx;
                RefreshRegNumberHistoryLightBox();
            }
            break;


        case "btnLast":
            hdnPageIdx.value = hdnMaxPage.value;

            RefreshRegNumberHistoryLightBox();
            break;

        case "btnPageGo":
            maxPage = parseInt(hdnMaxPage.value);
            goToPage = parseInt(document.getElementById('txtTableGotoPage').value);

            if (isInt(goToPage) && goToPage > 0 && goToPage <= maxPage)
            {
                hdnPageIdx.value = goToPage;
                RefreshRegNumberHistoryLightBox();
            }
            break;

        default:
            break;
    }

}

function RefreshRegNumberHistoryLightBox()
{
    var url = "AddEditTechnics_ENG_EQUIP.aspx?AjaxMethod=JSLoadRegNumberHistory";

    var params = "";
    params += "EngEquipId=" + document.getElementById("hdnEngEquipId").value;
    params += "&PageIndex=" + document.getElementById("hdnPageIndex").value;
    params += "&MaxPage=" + document.getElementById("hdnPageMaxPage").value;
    params += "&OrderBy=" + document.getElementById("hdnOrderBy").value;
    
    function response_handler(xml)
    {
        document.getElementById('divRegNumberHistoryLightBox').innerHTML = xmlValue(xml, "response");
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}