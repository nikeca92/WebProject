var isNewTechnics = true;

function TabLoaded_BasicInfo()
{
    SetClientTextAreaMaxLength("txtResidenceAddress", "1500");
}

//This function load the technics's details by TechnicsId
function LoadBasicInfoByTechnicsId()
{
    ClearAllMessages();

    var url = "AddEditTechnics_TRAILERS.aspx?AjaxMethod=JSLoadBasicInfo";
    var params = "";
    params += "TechnicsId=" + document.getElementById(hdnTechnicsIdClientID).value;
    
    function LoadBasicInfoByTechnicsId_CallBack(xml)
    {
        var trailer = xml.getElementsByTagName("trailer")[0];

        var technicsId = xmlValue(trailer, "technicsId");
        if (technicsId != 0)
        {
            isNewTechnics = false;
        }
        else
        {
            isNewTechnics = true;
        }

        var trailerId = xmlValue(trailer, "trailerId");
        var regNumber = xmlValue(trailer, "regNumber");
        var inventoryNumber = xmlValue(trailer, "inventoryNumber");
        var lastModified = xmlValue(trailer, "lastModified");
        var resMilRepStatus = xmlValue(trailer, "resMilRepStatus");
        var technicsCategoryId = xmlValue(trailer, "technicsCategoryId");
        var trailerKindId = xmlValue(trailer, "trailerKindId");
        var trailerTypeId = xmlValue(trailer, "trailerTypeId");
        var trailerBodyKindId = xmlValue(trailer, "trailerBodyKindId");
        var carryingCapacity = xmlValue(trailer, "carryingCapacity");
        var firstRegistrationDate = xmlValue(trailer, "firstRegistrationDate");
        var lastAnnualTechnicalReviewDate = xmlValue(trailer, "lastAnnualTechnicalReviewDate");
        var mileage = xmlValue(trailer, "mileage");
        var residenceCityId = xmlValue(trailer, "residenceCityId");
        var residencePostCode = xmlValue(trailer, "residencePostCode");
        var residenceRegionId = xmlValue(trailer, "residenceRegionId");
        var residenceMunicipalityId = xmlValue(trailer, "residenceMunicipalityId");
        var residenceDistrictId = xmlValue(trailer, "residenceDistrictId");
        var residenceAddress = xmlValue(trailer, "residenceAddress");
        var currMilDepartment = xmlValue(trailer, "currMilDepartment");
        var normativeTechnicsId = xmlValue(trailer, "normativeTechnicsId");
        var normativeCode = xmlValue(trailer, "normativeCode");
        
        document.getElementById(hdnTechnicsIdClientID).value = technicsId;

        if (document.getElementById("hdnTrailerId"))
        {
            document.getElementById("lblCurrMilDepartmentValue").innerHTML = currMilDepartment;
        
            document.getElementById("hdnTrailerId").value = trailerId;
            document.getElementById("txtRegNumber").value = regNumber;
            document.getElementById("lblRegNumberValue").innerHTML = regNumber;
            document.getElementById("txtInventoryNumber").value = inventoryNumber;
            document.getElementById("ddTechnicsCategory").value = technicsCategoryId;

            document.getElementById("lblGeneralTabCurrMilitaryReportStatusValue").innerHTML = resMilRepStatus;
            document.getElementById("lblLastModifiedValue").innerHTML = lastModified;
		 
            document.getElementById("ddTrailerKind").value = trailerKindId;
            document.getElementById("ddTrailerType").value = trailerTypeId;
            document.getElementById("ddTrailerBodyKind").value = trailerBodyKindId;
            document.getElementById("txtCarryingCapacity").value = carryingCapacity;
            document.getElementById("txtFirstRegistrationDate").value = firstRegistrationDate;
            document.getElementById("txtLastAnnualTechnicalReviewDate").value = lastAnnualTechnicalReviewDate;
            document.getElementById("txtMileage").value = mileage;

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
   
        var url = "AddEditTechnics_TRAILERS.aspx?AjaxMethod=JSSaveBasicInfo";
        var params = "";

        params += "TechnicsId=" + document.getElementById(hdnTechnicsIdClientID).value;
        params += "&TrailerId=" + document.getElementById("hdnTrailerId").value;

        params += "&RegNumber=" + custEncodeURI(document.getElementById("txtRegNumber").value);
        params += "&InventoryNumber=" + custEncodeURI(document.getElementById("txtInventoryNumber").value);
        params += "&TechnicsCategoryId=" + custEncodeURI(document.getElementById("ddTechnicsCategory").value);
        params += "&TrailerKindId=" + custEncodeURI(document.getElementById("ddTrailerKind").value);
        params += "&TrailerTypeId=" + custEncodeURI(document.getElementById("ddTrailerType").value);
        params += "&TrailerBodyKindId=" + custEncodeURI(document.getElementById("ddTrailerBodyKind").value);
        params += "&CarryingCapacity=" + custEncodeURI(document.getElementById("txtCarryingCapacity").value);
        params += "&FirstRegistrationDate=" + custEncodeURI(document.getElementById("txtFirstRegistrationDate").value);
        params += "&LastAnnualTechnicalReviewDate=" + custEncodeURI(document.getElementById("txtLastAnnualTechnicalReviewDate").value);
        params += "&Mileage=" + custEncodeURI(document.getElementById("txtMileage").value);
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
    
    function SaveBasicInfo_Callback(xml) {
        var status = xmlValue(xml, "response");
        var technicsId = xmlValue(xml, "technicsId");
        var trailerId = xmlValue(xml, "trailerId");

       
        if (document.getElementById(hdnTechnicsIdClientID).value == 0) {
            document.getElementById(hdnTechnicsIdClientID).value = technicsId;
            document.getElementById("hdnTrailerId").value = trailerId;

            location.hash = "AddEditTechnics.aspx?TechnicsId=" + technicsId;
        }
        else {
            document.getElementById(hdnTechnicsIdClientID).value = technicsId;
            document.getElementById("hdnTrailerId").value = trailerId;
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
        
        if (isNewTechnics)
        {
            if (document.getElementById("txtRegNumber").value.Trim() == "")
            {
                if (document.getElementById("txtRegNumber").disabled == true || document.getElementById("txtRegNumber").style.display == "none" || document.getElementById("txtRegNumberCont").style.display == "none")
                {
                    notValidFields.push("Регистрационен номер");
                }
                else
                {
                    ValidationMessage += tabNameHeader + GetErrorMessageMandatory("Регистрационен номер") + "</br>";
                }
            }
        }

        if (document.getElementById("txtCarryingCapacity").value.Trim() != "")
        {
            if (!isDecimal(document.getElementById("txtCarryingCapacity").value))
            {
                ValidationMessage += tabNameHeader + GetErrorMessageNumber("Товароносимост (т)") + "</br>";
            }
        }

        if (document.getElementById("txtFirstRegistrationDate").value.Trim() != "")
        {
            if (!IsValidDate(document.getElementById("txtFirstRegistrationDate").value))
            {
                ValidationMessage += tabNameHeader + GetErrorMessageDate("Дата на първата регистрация") + "</br>";
            }
        }

        if (document.getElementById("txtLastAnnualTechnicalReviewDate").value.Trim() != "")
        {
            if (!IsValidDate(document.getElementById("txtLastAnnualTechnicalReviewDate").value))
            {
                ValidationMessage += tabNameHeader + GetErrorMessageDate("Дата на последен ГТП") + "</br>";
            }
        }

        if (document.getElementById("txtMileage").value.Trim() != "")
        {
            if (!isDecimal(document.getElementById("txtMileage").value))
            {
                ValidationMessage += tabNameHeader + GetErrorMessageNumber("Изминати километри") + "</br>";
            }
        }

        var notValidFieldsCount = notValidFields.length;

        if (notValidFieldsCount > 0)
        {
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

//When the user type an RegNumber then check if this is an existing Trailer and
//take care about this
function RegNumberBlur()
{
    var txtRegNumber = document.getElementById("txtRegNumber");
    txtRegNumber.value = AdjustRegInv(txtRegNumber.value);
    
    if (txtRegNumber.value != txtRegNumber.getAttribute("oldvalue"))
    {
        var url = "AddEditTechnics_TRAILERS.aspx?AjaxMethod=JSCheckRegNumber";
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

//Refresh the TrailerKind list when updating the list via the GTable maintenance
function RefreshTrailerKindList()
{
    var url = "AddEditTechnics_TRAILERS.aspx?AjaxMethod=JSRefreshTrailerKindList";
    var params = "";
    
    function RefreshTrailerKindList_Callback(xml)
    {
        var currentValue = document.getElementById("ddTrailerKind").value;

        ClearSelectList(document.getElementById("ddTrailerKind"), true);

        var kinds = xml.getElementsByTagName("k");
        var found = false;

        for (var i = 0; i < kinds.length; i++)
        {
            var id = xmlValue(kinds[i], "id");
            var name = xmlValue(kinds[i], "name");

            if (currentValue == id)
                found = true;

            AddToSelectList(document.getElementById("ddTrailerKind"), id, name);
        };

        if (found)
            document.getElementById("ddTrailerKind").value = currentValue;
    }

    var myAJAX = new AJAX(url, true, params, RefreshTrailerKindList_Callback);
    myAJAX.Call();
}

function RefreshTrailerTypeList() {
    var url = "AddEditTechnics_TRAILERS.aspx?AjaxMethod=JSRefreshTrailerTypeList";
    var params = "";
    
    function RefreshTrailerTypeList_Callback(xml) {
        var currentValue = document.getElementById("ddTrailerType").value;

        ClearSelectList(document.getElementById("ddTrailerType"), true);

        var types = xml.getElementsByTagName("t");
        var found = false;

        for (var i = 0; i < types.length; i++) {
            var id = xmlValue(types[i], "id");
            var name = xmlValue(types[i], "name");

            if (currentValue == id)
                found = true;

            AddToSelectList(document.getElementById("ddTrailerType"), id, name);
        };

        if (found)
            document.getElementById("ddTrailerType").value = currentValue;
    }

    var myAJAX = new AJAX(url, true, params, RefreshTrailerTypeList_Callback);
    myAJAX.Call();
}

//Refresh the TrailerBodyKind list when updating the list via the GTable maintenance
function RefreshTrailerBodyKindList() {
    var url = "AddEditTechnics_TRAILERS.aspx?AjaxMethod=JSRefreshTrailerBodyKindList";
    var params = "";
    
    function RefreshTrailerBodyKindList_Callback(xml) {
        var currentValue = document.getElementById("ddTrailerBodyKind").value;

        ClearSelectList(document.getElementById("ddTrailerBodyKind"), true);

        var bodyKinds = xml.getElementsByTagName("k");
        var found = false;

        for (var i = 0; i < bodyKinds.length; i++) {
            var id = xmlValue(bodyKinds[i], "id");
            var name = xmlValue(bodyKinds[i], "name");

            if (currentValue == id)
                found = true;

            AddToSelectList(document.getElementById("ddTrailerBodyKind"), id, name);
        };

        if (found)
            document.getElementById("ddTrailerBodyKind").value = currentValue;
    }

    var myAJAX = new AJAX(url, true, params, RefreshTrailerBodyKindList_Callback);
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
        var url = "AddEditTechnics_TRAILERS.aspx?AjaxMethod=JSChangeRegNumber";

        var params = "TrailerId=" + document.getElementById("hdnTrailerId").value +
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
    var url = "AddEditTechnics_TRAILERS.aspx?AjaxMethod=JSLoadRegNumberHistory";

    var params = "TrailerId=" + document.getElementById("hdnTrailerId").value;

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
    var url = "AddEditTechnics_TRAILERS.aspx?AjaxMethod=JSLoadRegNumberHistory";

    var params = "";
    params += "TrailerId=" + document.getElementById("hdnTrailerId").value;
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