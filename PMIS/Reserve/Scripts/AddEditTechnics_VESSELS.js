var isNewTechnics = true;

function TabLoaded_BasicInfo()
{
    SetClientTextAreaMaxLength("txtResidenceAddress", "1500");
}

//This function load the technics's details by TechnicsId
function LoadBasicInfoByTechnicsId()
{
    ClearAllMessages();

    var url = "AddEditTechnics_VESSELS.aspx?AjaxMethod=JSLoadBasicInfo";
    var params = "";
    params += "TechnicsId=" + document.getElementById(hdnTechnicsIdClientID).value;
    
    function LoadBasicInfoByTechnicsId_CallBack(xml)
    {
        var vessel = xml.getElementsByTagName("vessel")[0];

        var technicsId = xmlValue(vessel, "technicsId");
        if (technicsId != 0)
        {
            isNewTechnics = false;
        }
        else
        {
            isNewTechnics = true;
        }

        var vesselId = xmlValue(vessel, "vesselId");
        var vesselName = xmlValue(vessel, "vesselName");
        var inventoryNumber = xmlValue(vessel, "inventoryNumber");
        var technicsCategoryId = xmlValue(vessel, "technicsCategoryId");       
        var vesselKindId = xmlValue(vessel, "vesselKindId");
        var vesselTypeId = xmlValue(vessel, "vesselTypeId");
        var lastModified = xmlValue(vessel, "lastModified");
        var resMilRepStatus = xmlValue(vessel, "resMilRepStatus");
        var loadDisplacement = xmlValue(vessel, "loadDisplacement");
        var lightDisplacement = xmlValue(vessel, "lightDisplacement");
        var length = xmlValue(vessel, "length");
        var width = xmlValue(vessel, "width");
        var maxHeight = xmlValue(vessel, "maxHeight");
        var maxWadeLoad = xmlValue(vessel, "maxWadeLoad");
        var maxWadeLight = xmlValue(vessel, "maxWadeLight");
        var officers = xmlValue(vessel, "officers");
        var sailors = xmlValue(vessel, "sailors");
        var enginePower = xmlValue(vessel, "enginePower");
        var speedNodes = xmlValue(vessel, "speedNodes");
        var stopDate = xmlValue(vessel, "stopDate");
        var stopReasons = xmlValue(vessel, "stopReasons");                            
        var residenceCityId = xmlValue(vessel, "residenceCityId");
        var residencePostCode = xmlValue(vessel, "residencePostCode");
        var residenceRegionId = xmlValue(vessel, "residenceRegionId");
        var residenceMunicipalityId = xmlValue(vessel, "residenceMunicipalityId");
        var residenceDistrictId = xmlValue(vessel, "residenceDistrictId");
        var residenceAddress = xmlValue(vessel, "residenceAddress");
        var currMilDepartment = xmlValue(vessel, "currMilDepartment");
        var normativeTechnicsId = xmlValue(vessel, "normativeTechnicsId");
        var normativeCode = xmlValue(vessel, "normativeCode");
        
        
        document.getElementById(hdnTechnicsIdClientID).value = technicsId;

        if (document.getElementById("hdnVesselId"))
        {
            document.getElementById("lblCurrMilDepartmentValue").innerHTML = currMilDepartment;
        
            document.getElementById("hdnVesselId").value = vesselId;
            document.getElementById("txtVesselName").value = vesselName;
            document.getElementById("txtInventoryNumber").value = inventoryNumber;
            document.getElementById("lblInventoryNumberValue").innerHTML = inventoryNumber;
            document.getElementById("ddTechnicsCategory").value = technicsCategoryId;           
            document.getElementById("ddVesselKind").value = vesselKindId;
            document.getElementById("ddVesselType").value = vesselTypeId;
            document.getElementById("lblGeneralTabCurrMilitaryReportStatusValue").innerHTML = resMilRepStatus;
            document.getElementById("lblLastModifiedValue").innerHTML = lastModified;
            document.getElementById("txtLoadDisplacement").value = loadDisplacement;
            document.getElementById("txtLightDisplacement").value = lightDisplacement;
            document.getElementById("txtLength").value = length;
            document.getElementById("txtWidth").value = width;
            document.getElementById("txtMaxHeight").value = maxHeight;
            document.getElementById("txtMaxWadeLoad").value = maxWadeLoad;
            document.getElementById("txtMaxWadeLight").value = maxWadeLight;
            document.getElementById("txtOfficers").value = officers;
            document.getElementById("txtSailors").value = sailors;
            document.getElementById("txtEnginePower").value = enginePower;
            document.getElementById("txtSpeedNodes").value = speedNodes;
            document.getElementById("txtStopDate").value = stopDate;
            document.getElementById("txtStopReasons").value = stopReasons;

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
        var url = "AddEditTechnics_VESSELS.aspx?AjaxMethod=JSSaveBasicInfo";
        var params = "";

        params += "TechnicsId=" + document.getElementById(hdnTechnicsIdClientID).value;
        params += "&VesselId=" + document.getElementById("hdnVesselId").value;

        params += "&VesselName=" + custEncodeURI(document.getElementById("txtVesselName").value);
        params += "&InventoryNumber=" + custEncodeURI(document.getElementById("txtInventoryNumber").value);
        params += "&TechnicsCategoryId=" + custEncodeURI(document.getElementById("ddTechnicsCategory").value);
        params += "&VesselKindId=" + custEncodeURI(document.getElementById("ddVesselKind").value);
        params += "&VesselTypeId=" + custEncodeURI(document.getElementById("ddVesselType").value);
        params += "&LoadDisplacement=" + custEncodeURI(document.getElementById("txtLoadDisplacement").value);
        params += "&LightDisplacement=" + custEncodeURI(document.getElementById("txtLightDisplacement").value);
        params += "&Length=" + custEncodeURI(document.getElementById("txtLength").value);
        params += "&Width=" + custEncodeURI(document.getElementById("txtWidth").value);
        params += "&MaxHeight=" + custEncodeURI(document.getElementById("txtMaxHeight").value);
        params += "&MaxWadeLoad=" + custEncodeURI(document.getElementById("txtMaxWadeLoad").value);
        params += "&MaxWadeLight=" + custEncodeURI(document.getElementById("txtMaxWadeLight").value);
        params += "&Officers=" + custEncodeURI(document.getElementById("txtOfficers").value);
        params += "&Sailors=" + custEncodeURI(document.getElementById("txtSailors").value);
        params += "&EnginePower=" + custEncodeURI(document.getElementById("txtEnginePower").value);
        params += "&SpeedNodes=" + custEncodeURI(document.getElementById("txtSpeedNodes").value);
        params += "&StopDate=" + custEncodeURI(document.getElementById("txtStopDate").value);
        params += "&StopReasons=" + document.getElementById("txtStopReasons").value;
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
        var vesselId = xmlValue(xml, "vesselId");

        if (document.getElementById(hdnTechnicsIdClientID).value == 0)
        {
            document.getElementById(hdnTechnicsIdClientID).value = technicsId;
            document.getElementById("hdnVesselId").value = vesselId;

            location.hash = "AddEditTechnics.aspx?TechnicsId=" + technicsId;
        }
        else
        {
            document.getElementById(hdnTechnicsIdClientID).value = technicsId;
            document.getElementById("hdnVesselId").value = vesselId;
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
            if (document.getElementById("txtInventoryNumber").value.Trim() == "") {
                
                if (document.getElementById("txtInventoryNumber").disabled == true || document.getElementById("txtInventoryNumber").style.display == "none" || document.getElementById("txtInventoryNumberCont").style.display == "none") {
                    notValidFields.push("Инвентарен номер");
                }
                else {
                    ValidationMessage += tabNameHeader + GetErrorMessageMandatory("Инвентарен номер") + "</br>";
                }
            }
        }

        if (document.getElementById("txtLoadDisplacement").value.Trim() != "") {
            if (!isDecimal(document.getElementById("txtLoadDisplacement").value)) {
                ValidationMessage += tabNameHeader + GetErrorMessageNumber("Водоизместване - пълен") + "</br>";
            }
        }

        if (document.getElementById("txtLightDisplacement").value.Trim() != "") {
            if (!isDecimal(document.getElementById("txtLightDisplacement").value)) {
                ValidationMessage += tabNameHeader + GetErrorMessageNumber("Водоизместване - празен") + "</br>";
            }
        }

        if (document.getElementById("txtLength").value.Trim() != "") {
            if (!isDecimal(document.getElementById("txtLength").value)) {
                ValidationMessage += tabNameHeader + GetErrorMessageNumber("Размери - дължина") + "</br>";
            }
        }

        if (document.getElementById("txtWidth").value.Trim() != "") {
            if (!isDecimal(document.getElementById("txtWidth").value)) {
                ValidationMessage += tabNameHeader + GetErrorMessageNumber("Размери - ширина") + "</br>";
            }
        }

        if (document.getElementById("txtMaxHeight").value.Trim() != "") {
            if (!isDecimal(document.getElementById("txtMaxHeight").value)) {
                ValidationMessage += tabNameHeader + GetErrorMessageNumber("Размери - макс. височина") + "</br>";
            }
        }

        if (document.getElementById("txtMaxWadeLoad").value.Trim() != "") {
            if (!isDecimal(document.getElementById("txtMaxWadeLoad").value)) {
                ValidationMessage += tabNameHeader + GetErrorMessageNumber("Максимално газене - пълен") + "</br>";
            }
        }

        if (document.getElementById("txtMaxWadeLight").value.Trim() != "") {
            if (!isDecimal(document.getElementById("txtMaxWadeLight").value)) {
                ValidationMessage += tabNameHeader + GetErrorMessageNumber("Максимално газене - празен") + "</br>";
            }
        }

        if (document.getElementById("txtOfficers").value.Trim() != "") {
            if (!isInt(document.getElementById("txtOfficers").value)) {
                ValidationMessage += tabNameHeader + GetErrorMessageNumber("Екипаж - офицери") + "</br>";
            }
        }

        if (document.getElementById("txtSailors").value.Trim() != "") {
            if (!isInt(document.getElementById("txtSailors").value)) {
                ValidationMessage += tabNameHeader + GetErrorMessageNumber("Екипаж - моряци") + "</br>";
            }
        }

        if (document.getElementById("txtEnginePower").value.Trim() != "") {
            if (!isDecimal(document.getElementById("txtEnginePower").value)) {
                ValidationMessage += tabNameHeader + GetErrorMessageNumber("Мощност на двигателната установка") + "</br>";
            }
        }

        if (document.getElementById("txtSpeedNodes").value.Trim() != "") {
            if (!isDecimal(document.getElementById("txtSpeedNodes").value)) {
                ValidationMessage += tabNameHeader + GetErrorMessageNumber("Скорост във възли") + "</br>";
            }
        }

        if (document.getElementById("txtStopDate").value.Trim() != "") {
            if (!IsValidDate(document.getElementById("txtStopDate").value)) {
                ValidationMessage += tabNameHeader + GetErrorMessageDate("Спиране на плавателния съд") + "</br>";
            }
        }

        var notValidFieldsCount = notValidFields.length;

        if (notValidFieldsCount > 0) {
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

    document.getElementById("txtInventoryNumber").style.display = "none";
    document.getElementById("lblInventoryNumberValue").style.display = "";
    document.getElementById("imgEditInvNumber").style.display = "";
    document.getElementById("imgHistoryInvNumber").style.display = "";
    document.getElementById("lblInventoryNumberValue").innerHTML = document.getElementById("txtInventoryNumber").value;

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
function InvNumberFocus()
{
    var txtInventNumber = document.getElementById("txtInventoryNumber");
    txtInventNumber.setAttribute("oldvalue", txtInventNumber.value);
}

function NewInventoryNumberBlur() {
    var txtNewInventoryNumber = document.getElementById("txtNewInventoryNumber");
    txtNewInventoryNumber.value = AdjustRegInv(txtNewInventoryNumber.value);
}

//When the user type an InventNumber then check if this is an existing Vessel and
//take care about this
function InvNumberBlur()
{
    var txtInventNumber = document.getElementById("txtInventoryNumber");
    txtInventNumber.value = AdjustRegInv(txtInventNumber.value);

    if (txtInventNumber.value != txtInventNumber.getAttribute("oldvalue"))
    {
        var url = "AddEditTechnics_VESSELS.aspx?AjaxMethod=JSCheckInventNumber";
        var params = "";
        params += "InventNumber=" + document.getElementById("txtInventoryNumber").value;
        
        function InventNumberBlur_Callback(xml)
        {
            var technicsId = parseInt(xmlValue(xml, "technicsId"));
            
            //Redirect to the existing Technics record
            if (technicsId > 0)
            {
                JSRedirect("AddEditTechnics.aspx?TechnicsId=" + technicsId);
            }
        }

        var myAJAX = new AJAX(url, true, params, InventNumberBlur_Callback);
        myAJAX.Call();
    }
}

function RepopulateVesselModels(vesselMakeId)
{
    var url = "AddEditTechnics_VESSELS.aspx?AjaxMethod=JSRepopulateVesselModels";
    var params = "";
    params += "VesselMakeId=" + vesselMakeId;
    
    function RepopulateVesselModels_Callback(xml)
    {
        ClearSelectList(document.getElementById("ddVesselModel"), true);

        var models = xml.getElementsByTagName("m");

        for (var i = 0; i < models.length; i++)
        {
            var id = xmlValue(models[i], "id");
            var name = xmlValue(models[i], "name");

            AddToSelectList(document.getElementById("ddVesselModel"), id, name);
        };
    }

    var myAJAX = new AJAX(url, true, params, RepopulateVesselModels_Callback);
    myAJAX.Call();
}

//Refresh the VesselKind list when updating the list via the GTable maintenance
function RefreshVesselKindList()
{
    var url = "AddEditTechnics_VESSELS.aspx?AjaxMethod=JSRefreshVesselKindList";
    var params = "";
    
    function RefreshVesselKindList_Callback(xml)
    {
        var currentValue = document.getElementById("ddVesselKind").value;

        ClearSelectList(document.getElementById("ddVesselKind"), true);

        var kinds = xml.getElementsByTagName("k");
        var found = false;

        for (var i = 0; i < kinds.length; i++)
        {
            var id = xmlValue(kinds[i], "id");
            var name = xmlValue(kinds[i], "name");

            if (currentValue == id)
                found = true;

            AddToSelectList(document.getElementById("ddVesselKind"), id, name);
        };

        if (found)
            document.getElementById("ddVesselKind").value = currentValue;
    }

    var myAJAX = new AJAX(url, true, params, RefreshVesselKindList_Callback);
    myAJAX.Call();
}


//Refresh the VesselType list when updating the list via the GTable maintenance
function RefreshVesselTypeList()
{
    var url = "AddEditTechnics_VESSELS.aspx?AjaxMethod=JSRefreshVesselTypeList";
    var params = "";
    
    function RefreshVesselTypeList_Callback(xml)
    {
        var currentValue = document.getElementById("ddVesselType").value;

        ClearSelectList(document.getElementById("ddVesselType"), true);

        var roadabilities = xml.getElementsByTagName("r");
        var found = false;

        for (var i = 0; i < roadabilities.length; i++)
        {
            var id = xmlValue(roadabilities[i], "id");
            var name = xmlValue(roadabilities[i], "name");

            if (currentValue == id)
                found = true;

            AddToSelectList(document.getElementById("ddVesselType"), id, name);
        };

        if (found)
            document.getElementById("ddVesselType").value = currentValue;
    }

    var myAJAX = new AJAX(url, true, params, RefreshVesselTypeList_Callback);
    myAJAX.Call();
}

function ChangeInvNumber()
{
    ShowChangeInvNumberLightBox();
}

function ShowChangeInvNumberLightBox()
{
    ClearAllMessages();

    document.getElementById("txtNewInventoryNumber").value = "";
    document.getElementById("lblCurrInventoryNumberValue").innerHTML = document.getElementById("lblInventoryNumberValue").innerHTML;

    // clean message label in the light box and hide it
    document.getElementById("spanChangeInvNumberLightBoxMessage").style.display = "none";
    document.getElementById("spanChangeInvNumberLightBoxMessage").innerHTML = "";

    //shows the light box and "disable" rest of the page
    document.getElementById("HidePage").style.display = "";
    document.getElementById("ChangeInvNumberLightBox").style.display = "";
    CenterLightBox("ChangeInvNumberLightBox");
}

// Close the light box
function HideChangeInvNumberLightBox()
{
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("ChangeInvNumberLightBox").style.display = "none";
}

// Save the new reg number
function SaveChangeInvNumberLightBox()
{
    if (ValidateChangeInvNumber())
    {
        var url = "AddEditTechnics_VESSELS.aspx?AjaxMethod=JSChangeInventNumber";

        var params = "VesselId=" + document.getElementById("hdnVesselId").value +
                     "&NewInventoryNumber=" + document.getElementById("txtNewInventoryNumber").value;

        function response_handler(xml)
        {
            var response = xmlValue(xml, "response");

            if (response == "OK")
            {
                document.getElementById("lblInventoryNumberValue").innerHTML = document.getElementById("txtNewInventoryNumber").value;
                document.getElementById("txtInventoryNumber").value = document.getElementById("txtNewInventoryNumber").value;

                RefreshInputsOfSpecificContainer(document.getElementById("tdInvNumber"), true);

                HideChangeInvNumberLightBox();
            }
            else
            {
                var lblMessage = document.getElementById("spanChangeInvNumberLightBoxMessage"); ;
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
function ValidateChangeInvNumber()
{
    var res = true;

    var lblMessage = document.getElementById("spanChangeInvNumberLightBoxMessage");
    lblMessage.innerHTML = "";

    var notValidFields = new Array();

    var txtNewInventNumber = document.getElementById("txtNewInventoryNumber");
    var lblCurrInventNumber = document.getElementById("lblCurrInventoryNumberValue");

    if (txtNewInventNumber.value.Trim() == "")
    {
        res = false;

        if (txtNewInventNumber.disabled == true || txtNewInventNumber.style.display == "none")
            notValidFields.push("Нов инвентарен номер");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Нов инвентарен номер") + "<br />";
    }
    else if (txtNewInventNumber.value.Trim() == lblCurrInventNumber.innerHTML)
    {
        res = false;
        lblMessage.innerHTML += "Новият инвентарен номер е същият като текущия" + "<br />";
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


function InvNumberHistory_Click()
{
    var url = "AddEditTechnics_VESSELS.aspx?AjaxMethod=JSLoadInventNumberHistory";

    var params = "VesselId=" + document.getElementById("hdnVesselId").value;

    function response_handler(xml)
    {
        document.getElementById("divInvNumberHistoryLightBox").innerHTML = xmlValue(xml, "response");

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("divInvNumberHistoryLightBox").style.display = "";
        CenterLightBox("divInvNumberHistoryLightBox");
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}

function HideInvNumberHistoryLightBox()
{
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("divInvNumberHistoryLightBox").style.display = "none";
    document.getElementById("divInvNumberHistoryLightBox").innerHTML = "";
}

function BtnInvNumberHistoryPagingClick(objectName)
{
    hdnPageIdx = document.getElementById('hdnPageIndex');
    hdnMaxPage = document.getElementById('hdnPageMaxPage');
    switch (objectName)
    {
        case "btnFirst":
            hdnPageIdx.value = 1;

            RefreshInvNumberHistoryLightBox();
            break;

        case "btnPrev":
            pageIdx = parseInt(hdnPageIdx.value);

            if (pageIdx > 1)
            {
                pageIdx--;
                hdnPageIdx.value = pageIdx;
                RefreshInvNumberHistoryLightBox();
            }

            break;

        case "btnNext":
            pageIdx = parseInt(hdnPageIdx.value);
            maxPage = parseInt(hdnMaxPage.value);

            if (pageIdx < maxPage)
            {
                pageIdx++;
                hdnPageIdx.value = pageIdx;
                RefreshInvNumberHistoryLightBox();
            }
            break;


        case "btnLast":
            hdnPageIdx.value = hdnMaxPage.value;

            RefreshInvNumberHistoryLightBox();
            break;

        case "btnPageGo":
            maxPage = parseInt(hdnMaxPage.value);
            goToPage = parseInt(document.getElementById('txtTableGotoPage').value);

            if (isInt(goToPage) && goToPage > 0 && goToPage <= maxPage)
            {
                hdnPageIdx.value = goToPage;
                RefreshInvNumberHistoryLightBox();
            }
            break;

        default:
            break;
    }

}

function RefreshInvNumberHistoryLightBox()
{
    var url = "AddEditTechnics_VESSELS.aspx?AjaxMethod=JSLoadInventNumberHistory";

    var params = "";
    params += "VesselId=" + document.getElementById("hdnVesselId").value;
    params += "&PageIndex=" + document.getElementById("hdnPageIndex").value;
    params += "&MaxPage=" + document.getElementById("hdnPageMaxPage").value;
    params += "&OrderBy=" + document.getElementById("hdnOrderBy").value;
    
    function response_handler(xml)
    {
        document.getElementById('divInvNumberHistoryLightBox').innerHTML = xmlValue(xml, "response");
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}



//Open the light-box for adding a new record in the VesselCrew table
function NewVesselCrew()
{
    ShowAddEditVesselCrewLightBox(0);
}

//Open the light-box for editing a record in the VesselCrew table
function EditVesselCrew(vesselCrewId)
{
    ShowAddEditVesselCrewLightBox(vesselCrewId);
}

function ShowAddEditVesselCrewLightBox(vesselCrewId)
{
    ClearAllMessages();

    document.getElementById("hdnVesselCrewID").value = vesselCrewId;

    //New record
    if (vesselCrewId == 0)
    {
        document.getElementById("lblAddEditVesselCrewTitle").innerHTML = "Въвеждане на екипаж";

        document.getElementById("ddVesselCrewCategoryLightBox").value = optionChooseOneValue;
        document.getElementById("txtIdentNumberLightBox").value = "";
        document.getElementById("ddMilitaryRankLightBox").value = optionChooseOneValue;
        document.getElementById("txtFullNameLightBox").value = "";
        document.getElementById("txtAddressLightBox").value = "";
        document.getElementById("chkHasAppointmentLightBox").checked = false;

        // clean message label in the light box and hide it
        document.getElementById("spanAddEditVesselCrewLightBox").style.display = "none";
        document.getElementById("spanAddEditVesselCrewLightBox").innerHTML = "";

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("lboxVesselCrew").style.display = "";
        CenterLightBox("lboxVesselCrew");
    }
    else //Edit record
    {
        document.getElementById("lblAddEditVesselCrewTitle").innerHTML = "Редактиране на екипаж";

        var url = "AddEditTechnics_VESSELS.aspx?AjaxMethod=JSLoadVesselCrew";

        var params = "VesselCrewId=" + vesselCrewId;

        function response_handler(xml)
        {
            //var personDualCitizenship = xml.getElementsByTagName("personDualCitizenship")[0];

            //var countryId = xmlValue(personDualCitizenship, "countryId");

            //document.getElementById("ddDualCitizenshipCountry").value = countryId;

            document.getElementById("ddVesselCrewCategoryLightBox").value = xmlValue(xml, "vesselCrewCategoryId");
            document.getElementById("txtIdentNumberLightBox").value = xmlValue(xml, "identNumber");
            document.getElementById("ddMilitaryRankLightBox").value = xmlValue(xml, "militaryRankId");
            document.getElementById("txtFullNameLightBox").value = xmlValue(xml, "fullName");
            document.getElementById("txtAddressLightBox").value = xmlValue(xml, "address");
            document.getElementById("chkHasAppointmentLightBox").checked = xmlValue(xml, "hasAppointment") == "1";

            // clean message label in the light box and hide it
            document.getElementById("spanAddEditVesselCrewLightBox").style.display = "none";
            document.getElementById("spanAddEditVesselCrewLightBox").innerHTML = "";

            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("lboxVesselCrew").style.display = "";
            CenterLightBox("lboxVesselCrew");
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Close the light-box
function HideAddEditVesselCrewLightBox()
{
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("lboxVesselCrew").style.display = "none";
}

//Save Add/Edit DualCitizenship
function SaveAddEditVesselCrewLightBox()
{
    if (ValidateAddEditVesselCrew())
    {
        var url = "AddEditTechnics.aspx?AjaxMethod=JSSaveVesselCrew";

        var params = "TechnicsId=" + document.getElementById(hdnTechnicsIdClientID).value;
        params += "&VesselId=" + document.getElementById("hdnVesselId").value;
        params += "&VesselCrewId=" + document.getElementById("hdnVesselCrewID").value;
        params += "&VesselCrewCategoryID=" + document.getElementById("ddVesselCrewCategoryLightBox").value;
        params += "&IdentNumber=" + custEncodeURI(document.getElementById("txtIdentNumberLightBox").value);
        params += "&MilitaryRankID=" + document.getElementById("ddMilitaryRankLightBox").value;
        params += "&FullName=" + custEncodeURI(document.getElementById("txtFullNameLightBox").value);
        params += "&Address=" + custEncodeURI(document.getElementById("txtAddressLightBox").value);
        params += "&HasAppointment=" + (document.getElementById("chkHasAppointmentLightBox").checked ? "1" : "0");

        function response_handler(xml)
        {
            var status = xmlValue(xml, "status");

            if (status == "OK")
            {
                var refreshedVesselCrewTable = xmlValue(xml, "refreshedTable");

                document.getElementById("divVesselCrew").innerHTML = refreshedVesselCrewTable;

                document.getElementById("lblVesselCrewMessage").className = "SuccessText";
                document.getElementById("lblVesselCrewMessage").innerHTML = document.getElementById("hdnVesselCrewID").value == "0" ? "Екипажът е добавен успешно" : "Екипажът е редактиран успешно";

                HideAddEditVesselCrewLightBox();
            }
            else
            {
                document.getElementById("spanAddEditVesselCrewLightBox").className = "ErrorText";
                document.getElementById("spanAddEditVesselCrewLightBox").innerHTML = status;
                document.getElementById("spanAddEditVesselCrewLightBox").style.display = "";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Client validation of Add/Edit VesselCrew light-box
function ValidateAddEditVesselCrew()
{
    var res = true;

    var lblMessage = document.getElementById("spanAddEditVesselCrewLightBox");
    lblMessage.innerHTML = "";

    var notValidFields = new Array();

    if (document.getElementById("txtIdentNumberLightBox").value != "" && !isOnlyDigits(document.getElementById("txtIdentNumberLightBox").value))
    {      
        res = false;
        lblMessage.innerHTML += "Въведеното ЕГН е невалидно" + "</br>";
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

// Delete a particular VesselCrew record
function DeleteVesselCrew(vesselCrewId)
{
    ClearAllMessages();

    YesNoDialog("Желаете ли да изтриете екипажа?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "AddEditTechnics.aspx?AjaxMethod=JSDeleteVesselCrew";

        var params = "TechnicsId=" + document.getElementById(hdnTechnicsIdClientID).value;
        params += "&VesselId=" + document.getElementById("hdnVesselId").value;
        params += "&VesselCrewId=" + vesselCrewId;

        function response_handler(xml)
        {
            var status = xmlValue(xml, "status")

            if (status == "OK")
            {
                var refreshedVesselCrewTable = xmlValue(xml, "refreshedTable");

                document.getElementById("divVesselCrew").innerHTML = refreshedVesselCrewTable;

                document.getElementById("lblVesselCrewMessage").className = "SuccessText";
                document.getElementById("lblVesselCrewMessage").innerHTML = "Екипажът е изтрит успешно";

            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

function SortVesselCrewTableBy(sort)
{      
    orderBy = parseInt(document.getElementById('hdnOrderBy').value);

    if (orderBy == sort)
    {
        sort = sort + 100;
    }

    document.getElementById('hdnOrderBy').value = sort;
    document.getElementById('hdnPageIndex').value = 1; //We go to 1st page

    RefreshVesselCrewTable();
}

function BtnVesselCrewTablePagingClick(objectName)
{
    hdnPageIdx = document.getElementById('hdnPageIndex');
    hdnMaxPage = document.getElementById('hdnPageMaxPage');
    switch (objectName)
    {
        case "btnFirst":
            hdnPageIdx.value = 1;

            RefreshVesselCrewTable();
            break;

        case "btnPrev":
            pageIdx = parseInt(hdnPageIdx.value);

            if (pageIdx > 1)
            {
                pageIdx--;
                hdnPageIdx.value = pageIdx;
                RefreshVesselCrewTable();
            }

            break;

        case "btnNext":
            pageIdx = parseInt(hdnPageIdx.value);
            maxPage = parseInt(hdnMaxPage.value);

            if (pageIdx < maxPage)
            {
                pageIdx++;
                hdnPageIdx.value = pageIdx;
                RefreshVesselCrewTable();
            }
            break;

        case "btnLast":
            hdnPageIdx.value = hdnMaxPage.value;

            RefreshVesselCrewTable();
            break;

        case "btnPageGo":
            maxPage = parseInt(hdnMaxPage.value);
            goToPage = parseInt(document.getElementById('txtTableGotoPage').value);

            if (isInt(goToPage) && goToPage > 0 && goToPage <= maxPage)
            {
                hdnPageIdx.value = goToPage;
                RefreshVesselCrewTable();
            }
            break;

        default:
            break;
    }

}

function RefreshVesselCrewTable()
{
    var url = "AddEditTechnics.aspx?AjaxMethod=JSLoadVesselCrewTable";

    var params = "TechnicsId=" + document.getElementById(hdnTechnicsIdClientID).value;
    params += "&VesselId=" + document.getElementById("hdnVesselId").value;
    params += "&PageIndex=" + document.getElementById("hdnPageIndex").value;
    params += "&MaxPage=" + document.getElementById("hdnPageMaxPage").value;
    params += "&OrderBy=" + document.getElementById("hdnOrderBy").value;

    var hdnIsPreview = document.getElementById(hdnIsPreviewClientID).value;
    if (hdnIsPreview == 1)
        params += "&Preview=1";

    function response_handler(xml)
    {
        var refreshedVesselCrewTable = xmlValue(xml, "refreshedTable");

        document.getElementById("divVesselCrew").innerHTML = refreshedVesselCrewTable;
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}