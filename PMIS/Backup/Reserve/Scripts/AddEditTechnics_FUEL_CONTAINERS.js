var isNewTechnics = true;

function TabLoaded_BasicInfo()
{
    SetClientTextAreaMaxLength("txtResidenceAddress", "1500");
}

//This function load the technics's details by TechnicsId
function LoadBasicInfoByTechnicsId()
{
    ClearAllMessages();

    var url = "AddEditTechnics_FUEL_CONTAINERS.aspx?AjaxMethod=JSLoadBasicInfo";
    var params = "";
    params += "TechnicsId=" + document.getElementById(hdnTechnicsIdClientID).value;
    
    function LoadBasicInfoByTechnicsId_CallBack(xml)
    {
        var fuelContainer = xml.getElementsByTagName("fuelContainer")[0];

        var technicsId = xmlValue(fuelContainer, "technicsId");
        if (technicsId != 0)
        {
            isNewTechnics = false;
        }
        else
        {
            isNewTechnics = true;
        }

        var fuelContainerId = xmlValue(fuelContainer, "fuelContainerId");
        var inventoryNumber = xmlValue(fuelContainer, "inventoryNumber");
        var technicsCategoryId = xmlValue(fuelContainer, "technicsCategoryId");
        var fuelContainerKindId = xmlValue(fuelContainer, "fuelContainerKindId");
        var fuelContainerTypeId = xmlValue(fuelContainer, "fuelContainerTypeId");
        var residenceCityId = xmlValue(fuelContainer, "residenceCityId");
        var fuelContainerCount = xmlValue(fuelContainer, "fuelContainerCount");
        var lastModified = xmlValue(fuelContainer, "lastModified");
        var resMilRepStatus = xmlValue(fuelContainer, "resMilRepStatus");
        var residencePostCode = xmlValue(fuelContainer, "residencePostCode");
        var residenceRegionId = xmlValue(fuelContainer, "residenceRegionId");
        var residenceMunicipalityId = xmlValue(fuelContainer, "residenceMunicipalityId");
        var residenceDistrictId = xmlValue(fuelContainer, "residenceDistrictId");
        var residenceAddress = xmlValue(fuelContainer, "residenceAddress");
        var currMilDepartment = xmlValue(fuelContainer, "currMilDepartment");
        var normativeTechnicsId = xmlValue(fuelContainer, "normativeTechnicsId");
        var normativeCode = xmlValue(fuelContainer, "normativeCode");
        
        
        document.getElementById(hdnTechnicsIdClientID).value = technicsId;

        if (document.getElementById("hdnFuelContainerId"))
        {
            document.getElementById("lblCurrMilDepartmentValue").innerHTML = currMilDepartment;
        
            document.getElementById("hdnFuelContainerId").value = fuelContainerId;
            document.getElementById("lblInventoryNumberValue").innerHTML = inventoryNumber;
            document.getElementById("txtInventoryNumber").value = inventoryNumber;
            document.getElementById("ddTechnicsCategory").value = technicsCategoryId;

            document.getElementById("ddFuelContainerKind").value = fuelContainerKindId;
            document.getElementById("ddFuelContainerType").value = fuelContainerTypeId;
            document.getElementById("lblGeneralTabCurrMilitaryReportStatusValue").innerHTML = resMilRepStatus;
            document.getElementById("lblLastModifiedValue").innerHTML = lastModified;

            document.getElementById("txtFuelContainerCount").value = fuelContainerCount;


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
        var url = "AddEditTechnics_FUEL_CONTAINERS.aspx?AjaxMethod=JSSaveBasicInfo";
        var params = "";

        params += "TechnicsId=" + document.getElementById(hdnTechnicsIdClientID).value;
        params += "&FuelContainerId=" + document.getElementById("hdnFuelContainerId").value;

        params += "&InventoryNumber=" + custEncodeURI(document.getElementById("txtInventoryNumber").value);
        params += "&TechnicsCategoryId=" + custEncodeURI(document.getElementById("ddTechnicsCategory").value);
        params += "&FuelContainerKindId=" + custEncodeURI(document.getElementById("ddFuelContainerKind").value);
        params += "&FuelContainerTypeId=" + custEncodeURI(document.getElementById("ddFuelContainerType").value);
        params += "&FuelContainerCount=" + custEncodeURI(document.getElementById("txtFuelContainerCount").value);
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
        var fuelContainerId = xmlValue(xml, "fuelContainerId");

        if (document.getElementById(hdnTechnicsIdClientID).value == 0)
        {
            document.getElementById(hdnTechnicsIdClientID).value = technicsId;
            document.getElementById("hdnFuelContainerId").value = fuelContainerId;

            location.hash = "AddEditTechnics.aspx?TechnicsId=" + technicsId;
        }
        else
        {
            document.getElementById(hdnTechnicsIdClientID).value = technicsId;
            document.getElementById("hdnFuelContainerId").value = fuelContainerId;
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

        if (document.getElementById("txtFuelContainerCount").value.Trim() == "" || !isInt(document.getElementById("txtFuelContainerCount").value)) {
           
            if (document.getElementById("txtFuelContainerCount").disabled == true || document.getElementById("txtFuelContainerCount").style.display == "none") {
                notValidFields.push("Брой");
            }
            else if (document.getElementById("txtFuelContainerCount").value.Trim() != "" && !isInt(document.getElementById("txtFuelContainerCount").value)) {
            ValidationMessage += tabNameHeader + 'Полето "Брой" не е валидно число' + "</br>";
            }
            else {
                ValidationMessage += tabNameHeader + GetErrorMessageMandatory("Брой") + "</br>";
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

    document.getElementById("txtInventoryNumber").style.display = "none";
    document.getElementById("lblInventoryNumberValue").style.display = "";
    document.getElementById("imgEditInvNumber").style.display = "";
    document.getElementById("imgHistoryInvNumber").style.display = "";
    document.getElementById("lblInventoryNumberValue").innerHTML = document.getElementById("txtInventoryNumber").value;

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


//Check the inventory number only when it is changed
function InvNumberFocus()
{
    var txtInventoryNumber = document.getElementById("txtInventoryNumber");
    txtInventoryNumber.setAttribute("oldvalue", txtInventoryNumber.value);
}

function NewInventoryNumberBlur() {
    var txtNewInventoryNumber = document.getElementById("txtNewInventoryNumber");
    txtNewInventoryNumber.value = AdjustRegInv(txtNewInventoryNumber.value);
}

//When the user type an InventoryNumber then check if this is an existing FuelContainer and
//take care about this
function InvNumberBlur()
{
    var txtInventoryNumber = document.getElementById("txtInventoryNumber");
    txtInventoryNumber.value = AdjustRegInv(txtInventoryNumber.value);
    
    if (txtInventoryNumber.value != txtInventoryNumber.getAttribute("oldvalue"))
    {
        var url = "AddEditTechnics_FUEL_CONTAINERS.aspx?AjaxMethod=JSCheckInvNumber";
        var params = "";
        params += "InventoryNumber=" + document.getElementById("txtInventoryNumber").value;
        
        function InvNumberBlur_Callback(xml)
        {
            var technicsId = parseInt(xmlValue(xml, "technicsId"));
            
            //Redirect to the existing Technics record
            if (technicsId > 0)
            {
                JSRedirect("AddEditTechnics.aspx?TechnicsId=" + technicsId);
            }
        }

        var myAJAX = new AJAX(url, true, params, InvNumberBlur_Callback);
        myAJAX.Call();
    }
}

//Refresh the FuelContainerKind list when updating the list via the GTable maintenance
function RefreshFuelContainerKindList()
{
    var url = "AddEditTechnics_FUEL_CONTAINERS.aspx?AjaxMethod=JSRefreshFuelContainerKindList";
    var params = "";
    
    function RefreshFuelContainerKindList_Callback(xml)
    {
        var currentValue = document.getElementById("ddFuelContainerKind").value;

        ClearSelectList(document.getElementById("ddFuelContainerKind"), true);

        var kinds = xml.getElementsByTagName("k");
        var found = false;

        for (var i = 0; i < kinds.length; i++)
        {
            var id = xmlValue(kinds[i], "id");
            var name = xmlValue(kinds[i], "name");

            if (currentValue == id)
                found = true;

            AddToSelectList(document.getElementById("ddFuelContainerKind"), id, name);
        };

        if (found)
            document.getElementById("ddFuelContainerKind").value = currentValue;
    }

    var myAJAX = new AJAX(url, true, params, RefreshFuelContainerKindList_Callback);
    myAJAX.Call();
}

function RefreshFuelContainerTypeList() {
    var url = "AddEditTechnics_FUEL_CONTAINERS.aspx?AjaxMethod=JSRefreshFuelContainerTypeList";
    var params = "";
    
    function RefreshFuelContainerTypeList_Callback(xml) {
        var currentValue = document.getElementById("ddFuelContainerType").value;

        ClearSelectList(document.getElementById("ddFuelContainerType"), true);

        var types = xml.getElementsByTagName("t");
        var found = false;

        for (var i = 0; i < types.length; i++) {
            var id = xmlValue(types[i], "id");
            var name = xmlValue(types[i], "name");

            if (currentValue == id)
                found = true;

            AddToSelectList(document.getElementById("ddFuelContainerType"), id, name);
        };

        if (found)
            document.getElementById("ddFuelContainerType").value = currentValue;
    }

    var myAJAX = new AJAX(url, true, params, RefreshFuelContainerTypeList_Callback);
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

// Save the new inventory number
function SaveChangeInvNumberLightBox()
{
    if (ValidateChangeInvNumber())
    {
        var url = "AddEditTechnics_FUEL_CONTAINERS.aspx?AjaxMethod=JSChangeInvNumber";

        var params = "FuelContainerId=" + document.getElementById("hdnFuelContainerId").value +
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

//Client validation of change inventory number light-box
function ValidateChangeInvNumber()
{
    var res = true;

    var lblMessage = document.getElementById("spanChangeInvNumberLightBoxMessage");
    lblMessage.innerHTML = "";

    var notValidFields = new Array();

    var txtNewInventoryNumber = document.getElementById("txtNewInventoryNumber");
    var lblCurrInventoryNumber = document.getElementById("lblCurrInventoryNumberValue");

    if (txtNewInventoryNumber.value.Trim() == "")
    {
        res = false;

        if (txtNewInventoryNumber.disabled == true || txtNewInventoryNumber.style.display == "none")
            notValidFields.push("Нов инвентарен номер");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Нов инвентарен номер") + "<br />";
    }
    else if (txtNewInventoryNumber.value.Trim() == lblCurrInventoryNumber.innerHTML)
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
    var url = "AddEditTechnics_FUEL_CONTAINERS.aspx?AjaxMethod=JSLoadInvNumberHistory";

    var params = "FuelContainerId=" + document.getElementById("hdnFuelContainerId").value;

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
    var url = "AddEditTechnics_FUEL_CONTAINERS.aspx?AjaxMethod=JSLoadInvNumberHistory";

    var params = "";
    params += "FuelContainerId=" + document.getElementById("hdnFuelContainerId").value;
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