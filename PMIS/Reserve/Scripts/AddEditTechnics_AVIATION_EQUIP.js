var isNewTechnics = true;

function TabLoaded_BasicInfo()
{
    
}

//This function load the technics's details by TechnicsId
function LoadBasicInfoByTechnicsId()
{
    ClearAllMessages();

    var url = "AddEditTechnics_AVIATION_EQUIP.aspx?AjaxMethod=JSLoadBasicInfo";
    var params = "";
    params += "TechnicsId=" + document.getElementById(hdnTechnicsIdClientID).value;
    
    function LoadBasicInfoByTechnicsId_CallBack(xml)
    {
        var aviationEquip = xml.getElementsByTagName("aviationEquip")[0];

        var technicsId = xmlValue(aviationEquip, "technicsId");
        if (technicsId != 0)
        {
            isNewTechnics = false;
        }
        else
        {
            isNewTechnics = true;
        }

        var aviationEquipId = xmlValue(aviationEquip, "aviationEquipId");
        var airInvNumber = xmlValue(aviationEquip, "airInvNumber");
        var technicsCategoryId = xmlValue(aviationEquip, "technicsCategoryId");
        var aviationAirKindId = xmlValue(aviationEquip, "aviationAirKindId");
        var aviationAirTypeId = xmlValue(aviationEquip, "aviationAirTypeId");
        var lastModified = xmlValue(aviationEquip, "lastModified");
        var resMilRepStatus = xmlValue(aviationEquip, "resMilRepStatus");
        
        
//        var aviationAirModelId = xmlValue(aviationEquip, "aviationAirModelId");

        var aviationAirModelName = xmlValue(aviationEquip, "aviationAirModelName");
        
        var airSeats = xmlValue(aviationEquip, "airSeats");
        var airCarryingCapacity = xmlValue(aviationEquip, "airCarryingCapacity");
        var airMaxDistance = xmlValue(aviationEquip, "airMaxDistance");
        var airLastTechnicalReviewDate = xmlValue(aviationEquip, "airLastTechnicalReviewDate");
        var otherInvNumber = xmlValue(aviationEquip, "otherInvNumber");
        var aviationOtherKindId = xmlValue(aviationEquip, "aviationOtherKindId");
        var aviationOtherTypeId = xmlValue(aviationEquip, "aviationOtherTypeId");
        
//        var aviationOtherBaseMachineMakeId = xmlValue(aviationEquip, "aviationOtherBaseMachineMakeId");
//        var aviationOtherBaseMachineModelId = xmlValue(aviationEquip, "aviationOtherBaseMachineModelId");

        var aviationOtherBaseMachineMakeName = xmlValue(aviationEquip, "aviationOtherBaseMachineMakeName");
        var aviationOtherBaseMachineModelName = xmlValue(aviationEquip, "aviationOtherBaseMachineModelName");
        
        var aviationOtherBaseMachineKindId = xmlValue(aviationEquip, "aviationOtherBaseMachineKindId");
        var aviationOtherBaseMachineTypeId = xmlValue(aviationEquip, "aviationOtherBaseMachineTypeId");
        var baseMachineMileageHoursSinceLastRepair = xmlValue(aviationEquip, "baseMachineMileageHoursSinceLastRepair");
        var aviationOtherEquipmentKindId = xmlValue(aviationEquip, "aviationOtherEquipmentKindId");
        var equipMileageHourSinceLstRepair = xmlValue(aviationEquip, "equipMileageHourSinceLstRepair");
        var currMilDepartment = xmlValue(aviationEquip, "currMilDepartment");
        var normativeTechnicsId = xmlValue(aviationEquip, "normativeTechnicsId");
        var normativeCode = xmlValue(aviationEquip, "normativeCode");
        
        
        document.getElementById(hdnTechnicsIdClientID).value = technicsId;

        if (document.getElementById("hdnAviationEquipId"))
        {
            document.getElementById("lblCurrMilDepartmentValue").innerHTML = currMilDepartment;
           
            document.getElementById("hdnAviationEquipId").value = aviationEquipId;
            document.getElementById("txtAirInvNumber").value = airInvNumber;
            document.getElementById("lblAirInvNumberValue").innerHTML = airInvNumber;
            document.getElementById("ddTechnicsCategory").value = technicsCategoryId;
            document.getElementById("ddAviationAirKind").value = aviationAirKindId;
            document.getElementById("ddAviationAirType").value = aviationAirTypeId;
            document.getElementById("lblGeneralTabCurrMilitaryReportStatusValue").innerHTML = resMilRepStatus;
            document.getElementById("lblLastModifiedValue").innerHTML = lastModified;

            //document.getElementById("ddAviationAirModel").value = aviationAirModelId;

            document.getElementById("txtAviationAirModelName").value = aviationAirModelName;
            
            document.getElementById("txtAirSeats").value = airSeats;
            document.getElementById("txtAirCarryingCapacity").value = airCarryingCapacity;
            document.getElementById("txtAirMaxDistance").value = airMaxDistance;
            document.getElementById("txtAirLastTechnicalReviewDate").value = airLastTechnicalReviewDate;
            document.getElementById("txtOtherInvNumber").value = otherInvNumber;
            document.getElementById("ddAviationOtherKind").value = aviationOtherKindId;
            document.getElementById("ddAviationOtherType").value = aviationOtherTypeId;
            
//            document.getElementById("ddAviationOtherBaseMachineMake").value = aviationOtherBaseMachineMakeId;

//            ClearSelectList(document.getElementById("ddAviationOtherBaseMachineModel"), true);

//            var avother_models = xml.getElementsByTagName("avother_model");

//            for (var i = 0; i < avother_models.length; i++)
//            {
//                var id = xmlValue(avother_models[i], "id");
//                var name = xmlValue(avother_models[i], "name");

//                AddToSelectList(document.getElementById("ddAviationOtherBaseMachineModel"), id, name);
//            };

//            document.getElementById("ddAviationOtherBaseMachineModel").value = aviationOtherBaseMachineModelId;

            document.getElementById("txtAviationOtherBaseMachineMakeName").value = aviationOtherBaseMachineMakeName;
            document.getElementById("txtAviationOtherBaseMachineModelName").value = aviationOtherBaseMachineModelName;

            document.getElementById("ddAviationOtherBaseMachineKind").value = aviationOtherBaseMachineKindId;
            document.getElementById("ddAviationOtherBaseMachineType").value = aviationOtherBaseMachineTypeId;
            document.getElementById("txtBaseMachineMileageHoursSinceLastRepair").value = baseMachineMileageHoursSinceLastRepair;
            document.getElementById("ddAviationOtherEquipmentKind").value = aviationOtherEquipmentKindId;
            document.getElementById("txtEquipMileageHourSinceLstRepair").value = equipMileageHourSinceLstRepair;
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
        var url = "AddEditTechnics_AVIATION_EQUIP.aspx?AjaxMethod=JSSaveBasicInfo";
        var params = "";

        params += "TechnicsId=" + document.getElementById(hdnTechnicsIdClientID).value;
        params += "&AviationEquipId=" + document.getElementById("hdnAviationEquipId").value;

        params += "&AirInvNumber=" + custEncodeURI(document.getElementById("txtAirInvNumber").value);
        params += "&TechnicsCategoryId=" + custEncodeURI(document.getElementById("ddTechnicsCategory").value);
        params += "&AviationAirKindId=" + custEncodeURI(document.getElementById("ddAviationAirKind").value);
        params += "&AviationAirTypeId=" + custEncodeURI(document.getElementById("ddAviationAirType").value);
        
//        params += "&AviationAirModelId=" + custEncodeURI(document.getElementById("ddAviationAirModel").value);

        params += "&AviationAirModelName=" + custEncodeURI(document.getElementById("txtAviationAirModelName").value);        
        
        params += "&AirSeats=" + custEncodeURI(document.getElementById("txtAirSeats").value);
        params += "&AirCarryingCapacity=" + custEncodeURI(document.getElementById("txtAirCarryingCapacity").value);
        params += "&AirMaxDistance=" + custEncodeURI(document.getElementById("txtAirMaxDistance").value);
        params += "&AirLastTechnicalReviewDate=" + custEncodeURI(document.getElementById("txtAirLastTechnicalReviewDate").value);
        params += "&OtherInvNumber=" + custEncodeURI(document.getElementById("txtOtherInvNumber").value);
        params += "&AviationOtherKindId=" + custEncodeURI(document.getElementById("ddAviationOtherKind").value);
        params += "&AviationOtherTypeId=" + custEncodeURI(document.getElementById("ddAviationOtherType").value);
        
//        params += "&AviationOtherBaseMachineMakeId=" + custEncodeURI(document.getElementById("ddAviationOtherBaseMachineMake").value);
//        params += "&AviationOtherBaseMachineModelId=" + custEncodeURI(document.getElementById("ddAviationOtherBaseMachineModel").value);

        params += "&AviationOtherBaseMachineMakeName=" + custEncodeURI(document.getElementById("txtAviationOtherBaseMachineMakeName").value);
        params += "&AviationOtherBaseMachineModelName=" + custEncodeURI(document.getElementById("txtAviationOtherBaseMachineModelName").value);
        
        params += "&AviationOtherBaseMachineKindId=" + custEncodeURI(document.getElementById("ddAviationOtherBaseMachineKind").value);
        params += "&AviationOtherBaseMachineTypeId=" + custEncodeURI(document.getElementById("ddAviationOtherBaseMachineType").value);
        params += "&BaseMachineMileageHoursSinceLastRepair=" + custEncodeURI(document.getElementById("txtBaseMachineMileageHoursSinceLastRepair").value);
        params += "&AviationOtherEquipmentKindId=" + custEncodeURI(document.getElementById("ddAviationOtherEquipmentKind").value);
        params += "&EquipMileageHourSinceLstRepair=" + custEncodeURI(document.getElementById("txtEquipMileageHourSinceLstRepair").value);
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
        var aviationEquipId = xmlValue(xml, "aviationEquipId");

        if (document.getElementById(hdnTechnicsIdClientID).value == 0)
        {
            document.getElementById(hdnTechnicsIdClientID).value = technicsId;
            document.getElementById("hdnAviationEquipId").value = aviationEquipId;

            location.hash = "AddEditTechnics.aspx?TechnicsId=" + technicsId;
        }
        else
        {
            document.getElementById(hdnTechnicsIdClientID).value = technicsId;
            document.getElementById("hdnAviationEquipId").value = aviationEquipId;
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
            if (document.getElementById("txtAirInvNumber").value.Trim() == "") {
                
                if (document.getElementById("txtAirInvNumber").disabled == true || document.getElementById("txtAirInvNumber").style.display == "none" || document.getElementById("txtAirInvNumberCont").style.display == "none") {
                    notValidFields.push("Инвентарен номер (въздухоплав. средства)");
                }
                else {
                    ValidationMessage += tabNameHeader + GetErrorMessageMandatory("Инвентарен номер (въздухоплав. средства)") + "</br>";
                }
            }
        }

        if (document.getElementById("txtAirSeats").value.Trim() != "") {
            if (!isInt(document.getElementById("txtAirSeats").value)) {
                ValidationMessage += tabNameHeader + GetErrorMessageNumber("Брой места (въздухоплав. средства)") + "</br>";
            }
        }

        if (document.getElementById("txtAirCarryingCapacity").value.Trim() != "") {
            if (!isDecimal(document.getElementById("txtAirCarryingCapacity").value)) {
                ValidationMessage += tabNameHeader + GetErrorMessageNumber("Товароносимост (т) (въздухоплав. средства)") + "</br>";
            }
        }

        if (document.getElementById("txtAirMaxDistance").value.Trim() != "") {
            if (!isDecimal(document.getElementById("txtAirMaxDistance").value)) {
                ValidationMessage += tabNameHeader + GetErrorMessageNumber("Максимален полет(км) (въздухоплав. средства)") + "</br>";
            }
        }

        if (document.getElementById("txtAirLastTechnicalReviewDate").value.Trim() != "") {
            if (!IsValidDate(document.getElementById("txtAirLastTechnicalReviewDate").value)) {
                ValidationMessage += tabNameHeader + GetErrorMessageDate("Дата на последния преглед (въздухоплав. средства)") + "</br>";
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

    document.getElementById("txtAirInvNumber").style.display = "none";
    document.getElementById("lblAirInvNumberValue").style.display = "";
    document.getElementById("imgEditAirInvNumber").style.display = "";
    document.getElementById("imgHistoryAirInvNumber").style.display = "";
    document.getElementById("lblAirInvNumberValue").innerHTML = document.getElementById("txtAirInvNumber").value;    

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
function AirInvNumberFocus()
{
    var txtAirInvNumber = document.getElementById("txtAirInvNumber");
    txtAirInvNumber.setAttribute("oldvalue", txtAirInvNumber.value);
}

function OtherInvNumberBlur() {
    var txtOtherInvNumber = document.getElementById("txtOtherInvNumber");
    txtOtherInvNumber.value = AdjustRegInv(txtOtherInvNumber.value);
}

function NewAirInvNumberBlur() {
    var txtNewAirInvNumber = document.getElementById("txtNewAirInvNumber");
    txtNewAirInvNumber.value = AdjustRegInv(txtNewAirInvNumber.value);
}

//When the user type an AirInvNumber then check if this is an existing AviationEquip and
//take care about this
function AirInvNumberBlur()
{
    var txtAirInvNumber = document.getElementById("txtAirInvNumber");
    txtAirInvNumber.value = AdjustRegInv(txtAirInvNumber.value);
    
    if (txtAirInvNumber.value != txtAirInvNumber.getAttribute("oldvalue"))
    {
        var url = "AddEditTechnics_AVIATION_EQUIP.aspx?AjaxMethod=JSCheckAirInvNumber";
        var params = "";
        params += "AirInvNumber=" + document.getElementById("txtAirInvNumber").value;
        
        function AirInvNumberBlur_Callback(xml)
        {
            var technicsId = parseInt(xmlValue(xml, "technicsId"));
            
            //Redirect to the existing Technics record
            if (technicsId > 0)
            {
                JSRedirect("AddEditTechnics.aspx?TechnicsId=" + technicsId);
            }
        }

        var myAJAX = new AJAX(url, true, params, AirInvNumberBlur_Callback);
        myAJAX.Call();
    }
}

//function RepopulateAviationOtherBaseMachineModels(aviationOtherBaseMachineMakeId)
//{
//    var url = "AddEditTechnics_AVIATION_EQUIP.aspx?AjaxMethod=JSRepopulateAviationOtherBaseMachineModels";
//    var params = "";
//    params += "AviationOtherBaseMachineMakeId=" + aviationOtherBaseMachineMakeId;
//    var myAJAX = new AJAX(url, true, params, RepopulateAviationOtherBaseMachineModels_Callback);
//    myAJAX.Call();

//    function RepopulateAviationOtherBaseMachineModels_Callback(xml)
//    {
//        ClearSelectList(document.getElementById("ddAviationOtherBaseMachineModel"), true);

//        var models = xml.getElementsByTagName("m");

//        for (var i = 0; i < models.length; i++)
//        {
//            var id = xmlValue(models[i], "id");
//            var name = xmlValue(models[i], "name");

//            AddToSelectList(document.getElementById("ddAviationOtherBaseMachineModel"), id, name);
//        };
//    }
//}

//Refresh the AviationAirKind list when updating the list via the GTable maintenance
function RefreshAviationAirKindList()
{
    var url = "AddEditTechnics_AVIATION_EQUIP.aspx?AjaxMethod=JSRefreshAviationAirKindList";
    var params = "";
    
    function RefreshAviationAirKindList_Callback(xml)
    {
        var currentValue = document.getElementById("ddAviationAirKind").value;

        ClearSelectList(document.getElementById("ddAviationAirKind"), true);

        var items = xml.getElementsByTagName("i");
        var found = false;

        for (var i = 0; i < items.length; i++)
        {
            var id = xmlValue(items[i], "id");
            var name = xmlValue(items[i], "name");

            if (currentValue == id)
                found = true;

            AddToSelectList(document.getElementById("ddAviationAirKind"), id, name);
        };

        if (found)
            document.getElementById("ddAviationAirKind").value = currentValue;
    }

    var myAJAX = new AJAX(url, true, params, RefreshAviationAirKindList_Callback);
    myAJAX.Call();
}


//Refresh the AviationAirType list when updating the list via the GTable maintenance
function RefreshAviationAirTypeList()
{
    var url = "AddEditTechnics_AVIATION_EQUIP.aspx?AjaxMethod=JSRefreshAviationAirTypeList";
    var params = "";
    
    function RefreshAviationAirTypeList_Callback(xml)
    {
        var currentValue = document.getElementById("ddAviationAirType").value;

        ClearSelectList(document.getElementById("ddAviationAirType"), true);

        var items = xml.getElementsByTagName("i");
        var found = false;

        for (var i = 0; i < items.length; i++)
        {
            var id = xmlValue(items[i], "id");
            var name = xmlValue(items[i], "name");

            if (currentValue == id)
                found = true;

            AddToSelectList(document.getElementById("ddAviationAirType"), id, name);
        };

        if (found)
            document.getElementById("ddAviationAirType").value = currentValue;
    }

    var myAJAX = new AJAX(url, true, params, RefreshAviationAirTypeList_Callback);
    myAJAX.Call();
}


//Refresh the AviationAirModel list when updating the list via the GTable maintenance
function RefreshAviationAirModelList()
{
    var url = "AddEditTechnics_AVIATION_EQUIP.aspx?AjaxMethod=JSRefreshAviationAirModelList";
    var params = "";
    
    function RefreshAviationAirModelList_Callback(xml)
    {
        var currentValue = document.getElementById("ddAviationAirModel").value;

        ClearSelectList(document.getElementById("ddAviationAirModel"), true);

        var items = xml.getElementsByTagName("i");
        var found = false;

        for (var i = 0; i < items.length; i++)
        {
            var id = xmlValue(items[i], "id");
            var name = xmlValue(items[i], "name");

            if (currentValue == id)
                found = true;

            AddToSelectList(document.getElementById("ddAviationAirModel"), id, name);
        };

        if (found)
            document.getElementById("ddAviationAirModel").value = currentValue;
    }

    var myAJAX = new AJAX(url, true, params, RefreshAviationAirModelList_Callback);
    myAJAX.Call();
}

//Refresh the AviationOtherKind list when updating the list via the GTable maintenance
function RefreshAviationOtherKindList()
{
    var url = "AddEditTechnics_AVIATION_EQUIP.aspx?AjaxMethod=JSRefreshAviationOtherKindList";
    var params = "";
    
    function RefreshAviationOtherKindList_Callback(xml)
    {
        var currentValue = document.getElementById("ddAviationOtherKind").value;

        ClearSelectList(document.getElementById("ddAviationOtherKind"), true);

        var items = xml.getElementsByTagName("i");
        var found = false;

        for (var i = 0; i < items.length; i++)
        {
            var id = xmlValue(items[i], "id");
            var name = xmlValue(items[i], "name");

            if (currentValue == id)
                found = true;

            AddToSelectList(document.getElementById("ddAviationOtherKind"), id, name);
        };

        if (found)
            document.getElementById("ddAviationOtherKind").value = currentValue;
    }

    var myAJAX = new AJAX(url, true, params, RefreshAviationOtherKindList_Callback);
    myAJAX.Call();
}


//Refresh the AviationOtherType list when updating the list via the GTable maintenance
function RefreshAviationOtherTypeList()
{
    var url = "AddEditTechnics_AVIATION_EQUIP.aspx?AjaxMethod=JSRefreshAviationOtherTypeList";
    var params = "";
    
    function RefreshAviationOtherTypeList_Callback(xml)
    {
        var currentValue = document.getElementById("ddAviationOtherType").value;

        ClearSelectList(document.getElementById("ddAviationOtherType"), true);

        var items = xml.getElementsByTagName("i");
        var found = false;

        for (var i = 0; i < items.length; i++)
        {
            var id = xmlValue(items[i], "id");
            var name = xmlValue(items[i], "name");

            if (currentValue == id)
                found = true;

            AddToSelectList(document.getElementById("ddAviationOtherType"), id, name);
        };

        if (found)
            document.getElementById("ddAviationOtherType").value = currentValue;
    }

    var myAJAX = new AJAX(url, true, params, RefreshAviationOtherTypeList_Callback);
    myAJAX.Call();
}


//Refresh the AviationOtherBaseMachineKind list when updating the list via the GTable maintenance
function RefreshAviationOtherBaseMachineKindList()
{
    var url = "AddEditTechnics_AVIATION_EQUIP.aspx?AjaxMethod=JSRefreshAviationOtherBaseMachineKindList";
    var params = "";
    
    function RefreshAviationOtherBaseMachineKindList_Callback(xml)
    {
        var currentValue = document.getElementById("ddAviationOtherBaseMachineKind").value;

        ClearSelectList(document.getElementById("ddAviationOtherBaseMachineKind"), true);

        var items = xml.getElementsByTagName("i");
        var found = false;

        for (var i = 0; i < items.length; i++)
        {
            var id = xmlValue(items[i], "id");
            var name = xmlValue(items[i], "name");

            if (currentValue == id)
                found = true;

            AddToSelectList(document.getElementById("ddAviationOtherBaseMachineKind"), id, name);
        };

        if (found)
            document.getElementById("ddAviationOtherBaseMachineKind").value = currentValue;
    }

    var myAJAX = new AJAX(url, true, params, RefreshAviationOtherBaseMachineKindList_Callback);
    myAJAX.Call();
}

//Refresh the AviationOtherBaseMachineType list when updating the list via the GTable maintenance
function RefreshAviationOtherBaseMachineTypeList()
{
    var url = "AddEditTechnics_AVIATION_EQUIP.aspx?AjaxMethod=JSRefreshAviationOtherBaseMachineTypeList";
    var params = "";
    
    function RefreshAviationOtherBaseMachineTypeList_Callback(xml)
    {
        var currentValue = document.getElementById("ddAviationOtherBaseMachineType").value;

        ClearSelectList(document.getElementById("ddAviationOtherBaseMachineType"), true);

        var items = xml.getElementsByTagName("i");
        var found = false;

        for (var i = 0; i < items.length; i++)
        {
            var id = xmlValue(items[i], "id");
            var name = xmlValue(items[i], "name");

            if (currentValue == id)
                found = true;

            AddToSelectList(document.getElementById("ddAviationOtherBaseMachineType"), id, name);
        };

        if (found)
            document.getElementById("ddAviationOtherBaseMachineType").value = currentValue;
    }

    var myAJAX = new AJAX(url, true, params, RefreshAviationOtherBaseMachineTypeList_Callback);
    myAJAX.Call();
}


//Refresh the AviationOtherEquipmentKind list when updating the list via the GTable maintenance
function RefreshAviationOtherEquipmentKindList()
{
    var url = "AddEditTechnics_AVIATION_EQUIP.aspx?AjaxMethod=JSRefreshAviationOtherEquipmentKindList";
    var params = "";
    
    function RefreshAviationOtherEquipmentKindList_Callback(xml)
    {
        var currentValue = document.getElementById("ddAviationOtherEquipmentKind").value;

        ClearSelectList(document.getElementById("ddAviationOtherEquipmentKind"), true);

        var items = xml.getElementsByTagName("i");
        var found = false;

        for (var i = 0; i < items.length; i++)
        {
            var id = xmlValue(items[i], "id");
            var name = xmlValue(items[i], "name");

            if (currentValue == id)
                found = true;

            AddToSelectList(document.getElementById("ddAviationOtherEquipmentKind"), id, name);
        };

        if (found)
            document.getElementById("ddAviationOtherEquipmentKind").value = currentValue;
    }

    var myAJAX = new AJAX(url, true, params, RefreshAviationOtherEquipmentKindList_Callback);
    myAJAX.Call();
}





function ChangeAirInvNumber()
{
    ShowChangeAirInvNumberLightBox();
}

function ShowChangeAirInvNumberLightBox()
{
    ClearAllMessages();

    document.getElementById("txtNewAirInvNumber").value = "";
    document.getElementById("lblCurrAirInvNumberValue").innerHTML = document.getElementById("lblAirInvNumberValue").innerHTML;

    // clean message label in the light box and hide it
    document.getElementById("spanChangeAirInvNumberLightBoxMessage").style.display = "none";
    document.getElementById("spanChangeAirInvNumberLightBoxMessage").innerHTML = "";

    //shows the light box and "disable" rest of the page
    document.getElementById("HidePage").style.display = "";
    document.getElementById("ChangeAirInvNumberLightBox").style.display = "";
    CenterLightBox("ChangeAirInvNumberLightBox");
}

// Close the light box
function HideChangeAirInvNumberLightBox()
{
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("ChangeAirInvNumberLightBox").style.display = "none";
}

// Save the new reg number
function SaveChangeAirInvNumberLightBox()
{
    if (ValidateChangeAirInvNumber())
    {
        var url = "AddEditTechnics_AVIATION_EQUIP.aspx?AjaxMethod=JSChangeAirInvNumber";

        var params = "AviationEquipId=" + document.getElementById("hdnAviationEquipId").value +
                     "&NewAirInvNumber=" + document.getElementById("txtNewAirInvNumber").value;

        function response_handler(xml)
        {
            var response = xmlValue(xml, "response");

            if (response == "OK")
            {
                document.getElementById("lblAirInvNumberValue").innerHTML = document.getElementById("txtNewAirInvNumber").value;
                document.getElementById("txtAirInvNumber").value = document.getElementById("txtNewAirInvNumber").value;

                RefreshInputsOfSpecificContainer(document.getElementById("tdAirInvNumber"), true);

                HideChangeAirInvNumberLightBox();
            }
            else
            {
                var lblMessage = document.getElementById("spanChangeAirInvNumberLightBoxMessage"); ;
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
function ValidateChangeAirInvNumber()
{
    var res = true;

    var lblMessage = document.getElementById("spanChangeAirInvNumberLightBoxMessage");
    lblMessage.innerHTML = "";

    var notValidFields = new Array();

    var txtNewAirInvNumber = document.getElementById("txtNewAirInvNumber");
    var lblCurrAirInvNumber = document.getElementById("lblCurrAirInvNumberValue");

    if (txtNewAirInvNumber.value.Trim() == "")
    {
        res = false;

        if (txtNewAirInvNumber.disabled == true || txtNewAirInvNumber.style.display == "none")
            notValidFields.push("Нов инвентарен номер");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Нов инвентарен номер") + "<br />";
    }
    else if (txtNewAirInvNumber.value.Trim() == lblCurrAirInvNumber.innerHTML)
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


function AirInvNumberHistory_Click()
{
    var url = "AddEditTechnics_AVIATION_EQUIP.aspx?AjaxMethod=JSLoadAirInvNumberHistory";

    var params = "AviationEquipId=" + document.getElementById("hdnAviationEquipId").value;

    function response_handler(xml)
    {
        document.getElementById("divAirInvNumberHistoryLightBox").innerHTML = xmlValue(xml, "response");

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("divAirInvNumberHistoryLightBox").style.display = "";
        CenterLightBox("divAirInvNumberHistoryLightBox");
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}

function HideAirInvNumberHistoryLightBox()
{
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("divAirInvNumberHistoryLightBox").style.display = "none";
    document.getElementById("divAirInvNumberHistoryLightBox").innerHTML = "";
}

function BtnAirInvNumberHistoryPagingClick(objectName)
{
    hdnPageIdx = document.getElementById('hdnPageIndex');
    hdnMaxPage = document.getElementById('hdnPageMaxPage');
    switch (objectName)
    {
        case "btnFirst":
            hdnPageIdx.value = 1;

            RefreshAirInvNumberHistoryLightBox();
            break;

        case "btnPrev":
            pageIdx = parseInt(hdnPageIdx.value);

            if (pageIdx > 1)
            {
                pageIdx--;
                hdnPageIdx.value = pageIdx;
                RefreshAirInvNumberHistoryLightBox();
            }

            break;

        case "btnNext":
            pageIdx = parseInt(hdnPageIdx.value);
            maxPage = parseInt(hdnMaxPage.value);

            if (pageIdx < maxPage)
            {
                pageIdx++;
                hdnPageIdx.value = pageIdx;
                RefreshAirInvNumberHistoryLightBox();
            }
            break;


        case "btnLast":
            hdnPageIdx.value = hdnMaxPage.value;

            RefreshAirInvNumberHistoryLightBox();
            break;

        case "btnPageGo":
            maxPage = parseInt(hdnMaxPage.value);
            goToPage = parseInt(document.getElementById('txtTableGotoPage').value);

            if (isInt(goToPage) && goToPage > 0 && goToPage <= maxPage)
            {
                hdnPageIdx.value = goToPage;
                RefreshAirInvNumberHistoryLightBox();
            }
            break;

        default:
            break;
    }

}

function RefreshAirInvNumberHistoryLightBox()
{
    var url = "AddEditTechnics_AVIATION_EQUIP.aspx?AjaxMethod=JSLoadAirInvNumberHistory";

    var params = "";
    params += "AviationEquipId=" + document.getElementById("hdnAviationEquipId").value;
    params += "&PageIndex=" + document.getElementById("hdnPageIndex").value;
    params += "&MaxPage=" + document.getElementById("hdnPageMaxPage").value;
    params += "&OrderBy=" + document.getElementById("hdnOrderBy").value;
    
    function response_handler(xml)
    {
        document.getElementById('divAirInvNumberHistoryLightBox').innerHTML = xmlValue(xml, "response");
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}