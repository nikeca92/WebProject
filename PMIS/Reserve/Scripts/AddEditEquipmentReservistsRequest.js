window.addEventListener ? window.addEventListener('load', PageLoad, false) : window.attachEvent('onload', PageLoad);
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(PageEndRequestHandler);


var btnSaveReqCmdCount = 0;
var reqCmdSaved = 0;


//Call this when the page is loaded
function PageLoad()
{
    if (!document.getElementById(btnBackClientID))
        document.getElementById(btnCloseClientID).style.display = "none";
}


//Client validation of the form
function ValidateData()
{
    var res = true;
    var lblMessage = document.getElementById(lblMessageClientID);
    lblMessage.innerHTML = "";

    var notValidFields = new Array();

    var requestNumber = document.getElementById(txtRequestNumberClientID);
    var requestDate = document.getElementById(txtRequestDateClientID);
    var militaryUnitId = parseInt(MilitaryUnitSelectorUtil.GetSelectedValue(msMilitaryUnitClientID));

    if (requestNumber.value.Trim() == "")
    {
        res = false;

        if (requestNumber.disabled == true || requestNumber.style.display == "none")
            notValidFields.push("Заявка №");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Заявка №") + "<br />";
    }

    if (requestDate.value.Trim() == "")
    {
        res = false;

        if (requestDate.disabled == true || requestDate.style.display == "none")
            notValidFields.push("Дата на заявката");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Дата на заявката") + "<br />";
    }
    else if (!IsValidDate(requestDate.value))
    {
        res = false;
        lblMessage.innerHTML += GetErrorMessageDate("Дата на заявката") + "<br />";
    }

    if (isNaN(militaryUnitId) || militaryUnitId <= 0)
    {
        res = false;

        if (MilitaryUnitSelectorUtil.IsDisabled(msMilitaryUnitClientID) || MilitaryUnitSelectorUtil.IsHidden(msMilitaryUnitClientID))
            notValidFields.push(militaryUnitLabel);
        else
            lblMessage.innerHTML += GetErrorMessageMandatory(militaryUnitLabel) + "<br />";
    }

    var notValidFieldsCount = notValidFields.length;

    if (notValidFieldsCount > 0)
    {
        var noRightsMessage = GetErrorMessageNoRights(notValidFields);
        lblMessage.innerHTML += "<br />" + noRightsMessage;
    }

    if (res)
    {
        ForceNoChanges();
        lblMessage.className = "SuccessText";
    }
    else
        lblMessage.className = "ErrorText";

    return res;
}

//The end of the UpdatePanel request
function PageEndRequestHandler()
{
    if (!document.getElementById(btnBackClientID))
        document.getElementById(btnCloseClientID).style.display = "none";

    if (document.getElementById(btnAddRequestCommandContClientID).style.display == "none")
    {
        var equipmentReservistsRequestId = document.getElementById(hfEquipmentReservistsRequestIDClientID).value;
        equipmentReservistsRequestId = parseInt(equipmentReservistsRequestId);

        if (!isNaN(equipmentReservistsRequestId) && equipmentReservistsRequestId > 0)
        {
            document.getElementById(btnAddRequestCommandContClientID).style.display = "";
        }
    }
}


function AddRequestCommand()
{
    var militaryUnitId = parseInt(MilitaryUnitSelectorUtil.GetSelectedValue(msMilitaryUnitClientID));
    ShowAddRequestCommandLightBox(militaryUnitId);
}


function ShowAddRequestCommandLightBox(militaryUnitId)
{
    ClearAllMessages();

    if (militaryUnitId != 0) // gets current values if editing vacancy announce position
    {
        var url = "AddEditEquipmentReservistsRequest.aspx?AjaxMethod=JSPopulateMilitaryCommands";

        var params = "MiliatryUnitId=" + militaryUnitId;

        function response_handler(xml)
        {
            ClearSelectList(document.getElementById("ddMilitaryCommands"), true);

            var militaryCommands = xml.getElementsByTagName("mc");

            for (var i = 0; i < militaryCommands.length; i++)
            {
                var id = xmlValue(militaryCommands[i], "id");
                var name = xmlValue(militaryCommands[i], "name");

                AddToSelectList(document.getElementById("ddMilitaryCommands"), id, name);
            };

            // clean message label in the light box and hide it
            document.getElementById("spanAddRequestCommandLightBoxMessage").style.display = "none";
            document.getElementById("spanAddRequestCommandLightBoxMessage").innerHTML = "";

            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("AddRequestCommandLightBox").style.display = "";
            CenterLightBox("AddRequestCommandLightBox");
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
    else
    {
        // clean message label in the light box and hide it
        document.getElementById("spanAddRequestCommandLightBoxMessage").style.display = "none";
        document.getElementById("spanAddRequestCommandLightBoxMessage").innerHTML = "";

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("AddRequestCommandLightBox").style.display = "";
        CenterLightBox("AddRequestCommandLightBox");
    }
}

// Close the light box
function HideAddRequestCommandLightBox()
{
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("AddRequestCommandLightBox").style.display = "none";
}

// Select and add a MilitaryCommand
function SaveAddRequestCommandLightBox()
{
    if (ValidateAddCommand())
    {
        var url = "AddEditEquipmentReservistsRequest.aspx?AjaxMethod=JSAddRequestCommand";

        var params = "EquipmentReservistsRequestId=" + document.getElementById(hfEquipmentReservistsRequestIDClientID).value +
                     "&MilitaryCommandId=" + document.getElementById("ddMilitaryCommands").value +
                     "&RequestCommandsCount=" + document.getElementById(hdnRequestCommandsCountClientID).value;

        function response_handler(xml)
        {
            var disabledClientControls = xmlValue(xml, "disabledClientControls");
            var hiddenClientControls = xmlValue(xml, "hiddenClientControls");

            document.getElementById(divRequestCommandsClientID).innerHTML += xmlValue(xml, "resultHTML");

            if (disabledClientControls != "")
            {
                document.getElementById(hdnDisabledClientControls).value += 
                   (document.getElementById(hdnDisabledClientControls).value == "" ? "" : ",") + disabledClientControls;
                CheckDisabledClientControls();
            }
            
            if (hiddenClientControls != "")
            {
                document.getElementById(hdnHiddenClientControls).value +=
                   (document.getElementById(hdnHiddenClientControls).value == "" ? "" : ",") + hiddenClientControls;
                CheckHiddenClientControls();
            }

            var requestCommandsCount = parseInt(document.getElementById(hdnRequestCommandsCountClientID).value);
            document.getElementById(hdnRequestCommandsCountClientID).value = ++requestCommandsCount;

            var requestVisibleCommandsCount = parseInt(document.getElementById(hdnVisibleRequestCommandsCountClientID).value);
            document.getElementById(hdnVisibleRequestCommandsCountClientID).value = ++requestVisibleCommandsCount;

            document.getElementById("lblCommandsMessage").className = "SuccessText";
            document.getElementById("lblCommandsMessage").innerHTML = "Командата е добавена успешно";

            AppendNewInputs();

            HideAddRequestCommandLightBox();
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

// Delete a particular Command from the Request
function DeleteRequestCommand(requestCommandId, idx)
{
    ClearAllMessages();

    YesNoDialog("Желаете ли да изтриете командата и всички нейни позиции?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "AddEditEquipmentReservistsRequest.aspx?AjaxMethod=JSDeleteRequestCommand";

        var params = "RequestCommandId=" + requestCommandId;

        function response_handler(xml)
        {
            var status = xmlValue(xml, "status")

            if (status == "OK") {
                if (xmlValue(xml, "msg") != "") {
                    AlertDialog(xmlValue(xml, "msg"), null);
                } else {
                    document.getElementById("divCommand" + idx).parentNode.removeChild(document.getElementById("divCommand" + idx));

                    var requestVisibleCommandsCount = parseInt(document.getElementById(hdnVisibleRequestCommandsCountClientID).value);
                    document.getElementById(hdnVisibleRequestCommandsCountClientID).value = --requestVisibleCommandsCount;

                    //If the count of the visible commands become zero then change the other count too
                    //We need the "normal" counter to be more like an index. When deleting do not decrease it because
                    //when if it is deleted #2 of total 3 and we decrease it to become 2 then when adding a new one there out be two commands
                    //with index #3. However, we need to know the number of the "visible" (not deleted) commands to be able
                    //to know when this count is 0. In this case we need to reset the "normal" counter to add the first command more closely to the header section (i.e. the padding-top is different for the first command)
                    if (requestVisibleCommandsCount == 0)
                        document.getElementById(hdnRequestCommandsCountClientID).value = "0";

                    document.getElementById("lblCommandsMessage").className = "SuccessText";
                    document.getElementById("lblCommandsMessage").innerHTML = "Командата е изтрита успешно";  
                } 
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Simulate pressing the "normal" Save button
function btnSaveClick()
{
    btnSaveReqCmdCount = 0;
    reqCmdSaved = 0;    

    var requestCommandsSaveButtons = document.getElementsByTagName("img");

    for (var i = 0; i < requestCommandsSaveButtons.length; i++)
    {    
        var btn = requestCommandsSaveButtons[i];
        if (btn.id && (btn.id.indexOf("imgBtnSaveReqCmd") != -1))
            btnSaveReqCmdCount++;
    }    

    for (var i = 0; i < requestCommandsSaveButtons.length; i++)
    {
        var btn = requestCommandsSaveButtons[i];
        if (btn.id && (btn.id.indexOf("imgBtnSaveReqCmd") != -1))
            CallSaveRequestCommand(btn, 0);
    }
    
    if(btnSaveReqCmdCount == 0)
        document.getElementById(btnSaveClientID).click();
}

//Simulate pressing the "normal" Back button
function btnCloseClick()
{
    document.getElementById(btnBackClientID).click();
}


//When changing the selection of the Delivery Region then re-populate the Delivery Municipality
function DeliveryRegionChange(dropDown, idx)
{
    ClearAllMessages();

    var deliveryRegionId = dropDown.value;

    var url = "AddEditEquipmentReservistsRequest.aspx?AjaxMethod=JSPopulateDeliveryMunicipality";

    var params = "DeliveryRegionId=" + deliveryRegionId;

    function response_handler(xml)
    {
        ClearSelectList(document.getElementById("DeliveryMunicipality" + idx), true);

        var municipalities = xml.getElementsByTagName("m");

        for (var i = 0; i < municipalities.length; i++)
        {
            var id = xmlValue(municipalities[i], "id");
            var name = xmlValue(municipalities[i], "name");

            AddToSelectList(document.getElementById("DeliveryMunicipality" + idx), id, name);
        };

        DeliveryMunicipalityChange(document.getElementById("DeliveryMunicipality" + idx), idx);
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}

//When changing the selection of the Delivery Municipality then re-populate the Delivery City
function DeliveryMunicipalityChange(dropDown, idx)
{
    ClearAllMessages();

    var deliveryMunicipalityId = dropDown.value;

    var url = "AddEditEquipmentReservistsRequest.aspx?AjaxMethod=JSPopulateDeliveryCity";

    var params = "DeliveryMunicipalityId=" + deliveryMunicipalityId;

    function response_handler(xml)
    {
        ClearSelectList(document.getElementById("DeliveryCity" + idx), true);

        var municipalities = xml.getElementsByTagName("c");

        for (var i = 0; i < municipalities.length; i++)
        {
            var id = xmlValue(municipalities[i], "id");
            var name = xmlValue(municipalities[i], "name");

            AddToSelectList(document.getElementById("DeliveryCity" + idx), id, name);
        };
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}

function CallSaveRequestCommand(button, doAppointmentUpdate)
{
   var requestCommandId = button.getAttribute('data-requestCommandId');
   var idx = button.getAttribute('data-idx');
   SaveRequestCommand(requestCommandId, idx, doAppointmentUpdate);
}

//Save the "header" of a particular Request Command
function SaveRequestCommand(requestCommandId, idx, doAppointmentUpdate)
{
    ClearAllMessages();

    if (ValidateCommand(idx))
    {
        var url = "AddEditEquipmentReservistsRequest.aspx?AjaxMethod=JSSaveRequestCommand";

        var params = "RequestCommandId=" + requestCommandId +
                     "&MilitaryCommandSuffix=" + custEncodeURI(document.getElementById("txtMilitaryCommandSuffix" + idx).value) +
                     "&DeliveryCityId=" + document.getElementById("DeliveryCity" + idx).value +
                     "&DeliveryPlace=" + custEncodeURI(document.getElementById("txtDeliveryPlace" + idx).value) +
                     "&EquipmentReservistsRequestId=" + document.getElementById(hfEquipmentReservistsRequestIDClientID).value +
                     "&MilitaryCommandId=" + custEncodeURI(document.getElementById("hdnMilitaryCommandId" + idx).value) +
                     "&AppointmentTime=" + custEncodeURI(document.getElementById("txtAppointmentTime" + idx).value) +
                     "&MilReadinessId=" + custEncodeURI(document.getElementById("ddMilReadiness" + idx).value) +
                     "&DoAppointmentUpdate=" + doAppointmentUpdate;

        function response_handler(xml)
        {
            var status = xmlValue(xml, "status")

            if (status == "OK")
            {
                if (btnSaveReqCmdCount == 0)
                {
                    document.getElementById("lblMessage" + idx).className = "SuccessText";
                    document.getElementById("lblMessage" + idx).innerHTML = "Записът е успешен";
                }
                else
                {
                    reqCmdSaved++;
                    if (btnSaveReqCmdCount == reqCmdSaved)
                    {
                        document.getElementById(btnSaveClientID).click();                       

                        btnSaveReqCmdCount = 0;
                        reqCmdSaved = 0;
                    }
                }
            }
            else
            {
                if (btnSaveReqCmdCount == 0)
                {
                    document.getElementById("lblMessage" + idx).className = "ErrorText";
                    document.getElementById("lblMessage" + idx).innerHTML = "Записът не е успешен";
                }
                else
                {
                    if (btnSaveReqCmdCount == reqCmdSaved)
                    {
                        var lblMessage = document.getElementById(lblMessageClientID);
                        lblMessage.className = "ErrorText";
                        lblMessage.innerHTML = "Записът не е успешен";

                        btnSaveReqCmdCount = 0;
                        reqCmdSaved = 0;
                    }
                }
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Clear all messages on the screen; Use this function when starting a new action
function ClearAllMessages()
{
    var cnt = parseInt(document.getElementById(hdnRequestCommandsCountClientID).value);

    for (var i = 1; i <= cnt; i++)
    {
        //First check if there is such an index because the command could be deleted
        if (document.getElementById("lblMessage" + i))
            document.getElementById("lblMessage" + i).innerHTML = "";

        if (document.getElementById("lblMessagePositions" + i))
            document.getElementById("lblMessagePositions" + i).innerHTML = "";
    }

    document.getElementById(lblMessageClientID).innerHTML = "";
    document.getElementById("lblCommandsMessage").innerHTML = "";
}


function AddPositionManually(requestCommandId, idx)
{
    ShowAddEditRequestCommandPositionLightBox(requestCommandId, 0, idx);
}


function ShowAddEditRequestCommandPositionLightBox(requestCommandId, requestCommandPositionId, idx)
{
    ClearAllMessages();

    var militaryCommandName = document.getElementById("hdnMilitaryCommandName" + idx).value;
    var militaryCommandSuffix = document.getElementById("txtMilitaryCommandSuffix" + idx).value;

    document.getElementById("lblMilitaryCommandLabelLightBox").innerHTML = militaryCommandName + " " + militaryCommandSuffix;

    document.getElementById("hdnRequestCommandPositionID").value = requestCommandPositionId;
    document.getElementById("hdnSelectedCommandIdx").value = idx;
    document.getElementById("hdnSelectedRequestCommandID").value = requestCommandId;

    document.getElementById(ddMRSTypeClientID).value = "1";
    RefreshMilitaryReportSpecialities(1);
    ClearSelectList(document.getElementById("ddSelectedMRS"), true);

    document.getElementById("chkIsPrimaryMilRepSpec").checked = false;

    document.getElementById(ddAvailableRanksClientID).value = "";            
    ClearSelectList(document.getElementById("ddSelectedRanks"), true);
    document.getElementById("chkIsPrimaryRank").checked = false;


    //New position
    if (requestCommandPositionId == 0)
    {
        document.getElementById("lblAddEditReqCommandPosTitle").innerHTML = "Въвеждане на нова длъжност";

        document.getElementById("txtPosition").value = "";
        document.getElementById("txtReservistsCount").value = "";

        SetChkIsPrimaryMRSAvailability();

        // clean message label in the light box and hide it
        document.getElementById("spanAddEditRequestCommandPositionLightBox").style.display = "none";
        document.getElementById("spanAddEditRequestCommandPositionLightBox").innerHTML = "";

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("AddEditRequestCommandPositionLightBox").style.display = "";
        CenterLightBox("AddEditRequestCommandPositionLightBox");
    }
    else //Edit Position
    {
        document.getElementById("lblAddEditReqCommandPosTitle").innerHTML = "Редактиране на длъжност";

        var url = "AddEditEquipmentReservistsRequest.aspx?AjaxMethod=JSLoadRequestCommandPosition";

        var params = "RequestCommandPositionId=" + requestCommandPositionId;

        function response_handler(xml)
        {
            var requestCommandPosition = xml.getElementsByTagName("requestCommandPosition")[0];

            var requestCommandPositionId = xmlValue(requestCommandPosition, "requestCommandPositionID");
            var requestsCommandId = xmlValue(requestCommandPosition, "requestsCommandID");
            var position = xmlValue(requestCommandPosition, "position");
            var militaryRanks = requestCommandPosition.getElementsByTagName("militaryRanks")[0].getElementsByTagName("rank");
            var reservistsCount = xmlValue(requestCommandPosition, "reservistsCount");
            var positionType = xmlValue(requestCommandPosition, "positionType");
            var fillReservistRequestsCnt = parseInt(xmlValue(requestCommandPosition, "FillReservistRequestsCnt"));
            var isMilitaryReportSpecialityEnabled = xmlValue(requestCommandPosition, "IsMilitaryReportSpecialityEnabled");
            var militaryReportSpecialities = requestCommandPosition.getElementsByTagName("milReportSpecialities")[0].getElementsByTagName("speciality");

            document.getElementById("txtPosition").value = position;

            for (var i = 0; i < militaryRanks.length; i++) {
                var rankId = xmlValue(militaryRanks[i], "id");
                var rankName = xmlValue(militaryRanks[i], "displayName");
                var isPrimary = xmlValue(militaryRanks[i], "isPrimary");

                if (isPrimary == "1")
                    document.getElementById("hdnPrimaryRankID").value = rankId;

                AddToSelectList(document.getElementById("ddSelectedRanks"), rankId, rankName, true);
            }

            RefreshPrimaryRankVisualIndication();
            SetChkIsPrimaryRankAvailability();
            
            document.getElementById("txtReservistsCount").value = reservistsCount;
            document.getElementById(hdnIsMilitaryReportSpecialityEnabledID).value = isMilitaryReportSpecialityEnabled;
            
            for (var i = 0; i < militaryReportSpecialities.length; i++)
            {
                var specialityId = xmlValue(militaryReportSpecialities[i], "id");
                var specialityName = xmlValue(militaryReportSpecialities[i], "displayName");
                var isPrimary = xmlValue(militaryReportSpecialities[i], "isPrimary");

                if (isPrimary == "1")
                    document.getElementById("hdnPrimaryMilRepSpecID").value = specialityId;

                AddToSelectList(document.getElementById("ddSelectedMRS"), specialityId, specialityName, true);
            }

            RefreshPrimaryMRSVisualIndication();
            SetChkIsPrimaryMRSAvailability();

            // clean message label in the light box and hide it
            document.getElementById("spanAddEditRequestCommandPositionLightBox").style.display = "none";
            document.getElementById("spanAddEditRequestCommandPositionLightBox").innerHTML = "";
          
            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("AddEditRequestCommandPositionLightBox").style.display = "";
            CenterLightBox("AddEditRequestCommandPositionLightBox");
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

function RefreshPrimaryRankVisualIndication() {
    var ddSelectedRanks = document.getElementById("ddSelectedRanks");
    var primaryRankID = document.getElementById("hdnPrimaryRankID").value;

    for (var i = 0; i < ddSelectedRanks.options.length; i++) {
        var rankID = ddSelectedRanks.options[i].value;

        if (rankID == primaryRankID) {
            ddSelectedRanks.options[i].className = "PrimaryMRSOption";
        } else {
        ddSelectedRanks.options[i].className = "NonPrimaryMRSOption";
        }
    }

    //Hack for IE: Force the browser to redraw the element so that the new colors take effect:
    ddSelectedRanks.style.dislay = "none";
    ddSelectedRanks.style.dislay = "";
}

function ddSelectedRanks_Changed() {
    SetChkIsPrimaryRankAvailability();
}

function SetChkIsPrimaryRankAvailability() {
    var selectedRanksSelectionCount = GetSelectedRanksSelectionCount();

    if (selectedRanksSelectionCount == 1) {
        EnableChkIsPrimaryRank(true);
        DisplayTheIsPrimaryFlagForTheSelectedRank();
    }
    else {
        EnableChkIsPrimaryRank(false);
    }
}

function GetSelectedRanksSelectionCount() {
    var selectedRanksSelectionCount = 0;
    var ddSelectedRanks = document.getElementById("ddSelectedRanks");

    for (var i = 0; i < ddSelectedRanks.options.length; i++)
        if (ddSelectedRanks.options[i].selected)
            selectedRanksSelectionCount++;

        return selectedRanksSelectionCount;
}

function EnableChkIsPrimaryRank(enable) {
    if (enable) {
        document.getElementById("chkIsPrimaryRank").disabled = false;
        document.getElementById("lblIsPrimaryRank").style.color = "";
    }
    else {
        document.getElementById("chkIsPrimaryRank").disabled = true;
        document.getElementById("lblIsPrimaryRank").style.color = "#AAAAAA";
        document.getElementById("chkIsPrimaryRank").checked = false;
    }
}

function DisplayTheIsPrimaryFlagForTheSelectedRank() {
    var selectedRankID = GetSelectedRankSelectionID();
    var primaryRankID = document.getElementById("hdnPrimaryRankID").value;
    document.getElementById("chkIsPrimaryRank").checked = selectedRankID == primaryRankID;
}

function chkIsPrimaryRank_Click() {
    SetPrimaryRankID();
    RefreshPrimaryRankVisualIndication();
}

function SetPrimaryRankID() {
    var chkIsPrimaryRank = document.getElementById("chkIsPrimaryRank");
    if (chkIsPrimaryRank.checked) {
        document.getElementById("hdnPrimaryRankID").value = GetSelectedRankSelectionID();
    }
    else {
        document.getElementById("hdnPrimaryRankID").value = "";
    }
}

function GetSelectedRankSelectionID() {
    var ddSelectedRanks = document.getElementById("ddSelectedRanks");
    var selectedRankID = ddSelectedRanks.options[ddSelectedRanks.options.selectedIndex].value;
    return selectedRankID;
}

function RefreshPrimaryMRSVisualIndication() {
    var ddSelectedMRS = document.getElementById("ddSelectedMRS");
    var primaryMRSID = document.getElementById("hdnPrimaryMilRepSpecID").value;

    for (var i = 0; i < ddSelectedMRS.options.length; i++) {
        var MRSID = ddSelectedMRS.options[i].value;

        if (MRSID == primaryMRSID) {
            ddSelectedMRS.options[i].className = "PrimaryMRSOption";
        } else {
            ddSelectedMRS.options[i].className = "NonPrimaryMRSOption";
        }
    }

    //Hack for IE: Force the browser to redraw the element so that the new colors take effect:
    ddSelectedMRS.style.dislay = "none";
    ddSelectedMRS.style.dislay = "";
}

function ddSelectedMRS_Changed() {
    SetChkIsPrimaryMRSAvailability();
}

function SetChkIsPrimaryMRSAvailability() {
    var selectedMRSSelectionCount = GetSelectedMRSSelectionCount();

    if (selectedMRSSelectionCount == 1) {
        EnableChkIsPrimary(true);
        DisplayTheIsPrimaryFlagForTheSelectedMRS();
    } 
    else {
        EnableChkIsPrimary(false);
    }
}

function GetSelectedMRSSelectionCount() {
    var selectedMRSSelectionCount = 0;
    var ddSelectedMRS = document.getElementById("ddSelectedMRS");

    for (var i = 0; i < ddSelectedMRS.options.length; i++)
        if (ddSelectedMRS.options[i].selected)
        selectedMRSSelectionCount++;

    return selectedMRSSelectionCount;
}

function EnableChkIsPrimary(enable) {
    if (enable) {
        document.getElementById("chkIsPrimaryMilRepSpec").disabled = false;
        document.getElementById("lblIsPrimaryMilRepSpec").style.color = "";
    }
    else {
        document.getElementById("chkIsPrimaryMilRepSpec").disabled = true;
        document.getElementById("lblIsPrimaryMilRepSpec").style.color = "#AAAAAA";
        document.getElementById("chkIsPrimaryMilRepSpec").checked = false;
    }
}

function DisplayTheIsPrimaryFlagForTheSelectedMRS() {
    var selectedMRSID = GetSelectedMRSSelectionID();
    var primaryMRSID = document.getElementById("hdnPrimaryMilRepSpecID").value;
    document.getElementById("chkIsPrimaryMilRepSpec").checked = selectedMRSID == primaryMRSID;
}

function chkIsPrimaryMilRepSpec_Click() {
    SetPrimaryMilRepSpecID();
    RefreshPrimaryMRSVisualIndication();
}

function SetPrimaryMilRepSpecID() {
    var chkIsPrimaryMilRepSpec = document.getElementById("chkIsPrimaryMilRepSpec");
    if (chkIsPrimaryMilRepSpec.checked) {
        document.getElementById("hdnPrimaryMilRepSpecID").value = GetSelectedMRSSelectionID();
    } 
    else {
        document.getElementById("hdnPrimaryMilRepSpecID").value = "";
    }
}

function GetSelectedMRSSelectionID() {
    var ddSelectedMRS = document.getElementById("ddSelectedMRS");
    var selectedMRSID = ddSelectedMRS.options[ddSelectedMRS.options.selectedIndex].value;
    return selectedMRSID;
}

//Save Add/Edit RequestCommandPosition
function SaveAddEditRequestCommandPositionLightBox()
{
    if (ValidateAddEditRequestCommandPosition())
    {
        var idx = document.getElementById("hdnSelectedCommandIdx").value;

        var url = "AddEditEquipmentReservistsRequest.aspx?AjaxMethod=JSSaveRequestCommandPosition";

        var params = "RequestCommandPositionId=" + document.getElementById("hdnRequestCommandPositionID").value +
                 "&RequestCommandId=" + document.getElementById("hdnSelectedRequestCommandID").value +
                 "&Position=" + custEncodeURI(document.getElementById("txtPosition").value) +
                 "&ReservistsCount=" + document.getElementById("txtReservistsCount").value +
                 "&Idx=" + idx;

        var primaryRankID = document.getElementById("hdnPrimaryRankID").value;
        var ranks = document.getElementById("ddSelectedRanks");

        for (var i = 0; i < ranks.options.length; i++) {
            var rankIdx = i + 1;

            params += "&RankId_" + rankIdx + "=" + ranks.options[i].value;
            params += "&RankDisplayText_" + rankIdx + "=" + custEncodeURI(ranks.options[i].text);
            params += "&IsPrimaryRank_" + rankIdx + "=" + (ranks.options[i].value == primaryRankID ? "1" : "0");
        }

        params += "&RanksCnt=" + ranks.options.length;

        var primaryMRSID = document.getElementById("hdnPrimaryMilRepSpecID").value;
        var specialities = document.getElementById("ddSelectedMRS");

        for (var i = 0; i < specialities.options.length; i++)
        {
            var mrsIdx = i + 1;

            params += "&MRSId_" + mrsIdx + "=" + specialities.options[i].value;
            params += "&MRSDisplayText_" + mrsIdx + "=" + custEncodeURI(specialities.options[i].text);
            params += "&IsPrimary_" + mrsIdx + "=" + (specialities.options[i].value == primaryMRSID ? "1" : "0");
        }

        params += "&MRSCnt=" + specialities.options.length;

        function response_handler(xml)
        {
            var refreshedPositionsTable = xmlValue(xml, "refreshedPositionsTable");

            document.getElementById("tdRequestCommandPositionsCont" + idx).innerHTML = refreshedPositionsTable;

            document.getElementById("lblMessagePositions" + idx).className = "SuccessText";
            document.getElementById("lblMessagePositions" + idx).innerHTML = document.getElementById("hdnRequestCommandPositionID").value == "0" ? "Длъжността е добавена успешно" : "Записът на длъжността е успешен";

            HideAddEditRequestCommandPositionLightBox();
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

// Close the light box
function HideAddEditRequestCommandPositionLightBox() {

    if(document.getElementById(hdnIsMilitaryReportSpecialityEnabledID).value == "1"){
        document.getElementById("lblMilitaryReportSpecialityType").disabled = false;
        document.getElementById("lblMilitaryReportSpeciality").disabled = false;
        document.getElementById(ddMRSTypeClientID).disabled = false;
        document.getElementById("ddAvailableMRS").disabled = false;
        document.getElementById("btnSelectMRS").disabled = false;
        document.getElementById("btnRemoveMRS").disabled = false;
        document.getElementById("lblSelectedMRS").disabled = false;
        document.getElementById("ddSelectedMRS").disabled = false;
        document.getElementById("lblIsPrimaryMilRepSpec").disabled = false;
        document.getElementById("chkIsPrimaryMilRepSpec").disabled = false;
    }
    
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("AddEditRequestCommandPositionLightBox").style.display = "none";
}

//Edit a particular Request Command Position which has been added manually (i.e. not via the Import)
function EditRequestCommandPosition(requestCommandId, requestCommandPositionId, idx)
{
    ShowAddEditRequestCommandPositionLightBox(requestCommandId, requestCommandPositionId, idx);
}

//Delete a particular Request Command Position (i.e. not via the Import)
function DeleteRequestCommandPosition(requestCommandId, requestCommandPositionId, idx)
{
    ClearAllMessages();

    YesNoDialog("Желаете ли да изтриете длъжността?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "AddEditEquipmentReservistsRequest.aspx?AjaxMethod=JSDeleteRequestCommandPosition";

        var params = "RequestCommandPositionId=" + requestCommandPositionId +
                     "&RequestCommandId=" + requestCommandId +
                     "&Idx=" + idx;

        function response_handler(xml) {
            if (xmlValue(xml, "msg") != "") {
                AlertDialog(xmlValue(xml, "msg"), null);
            } else {
                var refreshedPositionsTable = xmlValue(xml, "refreshedPositionsTable");

                document.getElementById("tdRequestCommandPositionsCont" + idx).innerHTML = refreshedPositionsTable;

                document.getElementById("lblMessagePositions" + idx).className = "SuccessText";
                document.getElementById("lblMessagePositions" + idx).innerHTML = "Длъжността е изтрита успешно";


            }

            HideAddEditRequestCommandPositionLightBox();    
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Use these to optimize the performance (i.e. do not pull the specialities of a given type if they have been already loaded)
var mrs1 = null;
var mrs2 = null;
var mrs3 = null;
var mrs4 = null;
var globRes = null;

function RefreshMilitaryReportSpecialities(type)
{
    if (type == 1 && mrs1 != null ||
        type == 2 && mrs2 != null ||
        type == 3 && mrs3 != null ||
        type == 4 && mrs4 != null)
    {
        RefreshListBox();
    }
    else
    {
        var url = "AddEditEquipmentReservistsRequest.aspx?AjaxMethod=JSPopulateMilitaryReportSpecialities";

        var params = "Type=" + type;

        function response_handler(xml)
        {
            var res = xml.getElementsByTagName("s");

            if (type == 1)
                mrs1 = res;
            else if (type == 2)
                mrs2 = res;
            else if (type == 3)
                mrs3 = res;
            else if (type == 4)
                mrs4 = res;

            globRes = res;

            RefreshListBox();
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
    
    function RefreshListBox()
    {
        var xml = globRes;

        if (type == 1 && mrs1 != null)
            xml = mrs1;
        else if (type == 2 && mrs2 != null)
            xml = mrs2;
        else if (type == 3 && mrs3 != null)
            xml = mrs3;
        else if (type == 4 && mrs4 != null)
            xml = mrs4;
     
        ClearSelectList(document.getElementById("ddAvailableMRS"), true);

        for (var i = 0; i < xml.length; i++)
        {
            var id = xmlValue(xml[i], "id");
            var name = xmlValue(xml[i], "name");

            AddToSelectList(document.getElementById("ddAvailableMRS"), id, name, true);
        };
    }
}

function SelectMilitaryReportingSpecialities()
{
    var source = document.getElementById("ddAvailableMRS");
    var target = document.getElementById("ddSelectedMRS");

    var wasTargetEmpty = target.options.length == 0;
    var addedCnt = 0;

    for(var i = 0; i < source.options.length; i++)
    {
        if (source.options[i].selected)
        {
            var found = false;
            
            for (var j = 0; j < target.options.length; j++)
            {
                if (target.options[j].value == source.options[i].value)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                var newOption = new Option();
                newOption.text = source.options[i].text;
                newOption.value = source.options[i].value;
                newOption.title = newOption.text;

                target.options[target.length] = newOption;

                if (addedCnt == 0 && wasTargetEmpty) {
                    document.getElementById("hdnPrimaryMilRepSpecID").value = source.options[i].value;
                }

                addedCnt++;
            }
        }
    }

    RefreshPrimaryMRSVisualIndication();
    SetChkIsPrimaryMRSAvailability();
}

function RemoveMilitaryReportingSpecialities()
{
    var source = document.getElementById("ddSelectedMRS");
    var primaryMRSID = document.getElementById("hdnPrimaryMilRepSpecID").value;

    while (source.options.selectedIndex >= 0) {
        var MRSIDToRemove = source.options[source.options.selectedIndex].value;
        if (primaryMRSID == MRSIDToRemove) {
            document.getElementById("hdnPrimaryMilRepSpecID").value = "";
        }
    
        source.remove(source.options.selectedIndex);
    }

    RefreshPrimaryMRSVisualIndication();
    SetChkIsPrimaryMRSAvailability();
}

function SelectMilitaryRanks() {
    var source = document.getElementById(ddAvailableRanksClientID);
    var target = document.getElementById("ddSelectedRanks");

    var wasTargetEmpty = target.options.length == 0;
    var addedCnt = 0;

    for (var i = 0; i < source.options.length; i++) {
        if (source.options[i].selected) {
            var found = false;

            for (var j = 0; j < target.options.length; j++) {
                if (target.options[j].value == source.options[i].value) {
                    found = true;
                    break;
                }
            }

            if (!found) {
                var newOption = new Option();
                newOption.text = source.options[i].text;
                newOption.value = source.options[i].value;
                newOption.title = newOption.text;

                target.options[target.length] = newOption;

                if (addedCnt == 0 && wasTargetEmpty) {
                    document.getElementById("hdnPrimaryRankID").value = source.options[i].value;
                }

                addedCnt++;
            }
        }
    }

    RefreshPrimaryRankVisualIndication();
    SetChkIsPrimaryRankAvailability();
}

function RemoveMilitaryRanks() {
    var source = document.getElementById("ddSelectedRanks");
    var primaryRankID = document.getElementById("hdnPrimaryRankID").value;

    while (source.options.selectedIndex >= 0) {
        var rankIDToRemove = source.options[source.options.selectedIndex].value;
        if (primaryRankID == rankIDToRemove) {
            document.getElementById("hdnPrimaryRankID").value = "";
        }

        source.remove(source.options.selectedIndex);
    }

    RefreshPrimaryRankVisualIndication();
    SetChkIsPrimaryRankAvailability();
}

//Client validation of the command header
function ValidateCommand(idx)
{
    var res = true;
    
    var lblMessage = document.getElementById("lblMessage" + idx);
    lblMessage.innerHTML = "";

    var notValidFields = new Array();

    var txtAppointmentTime = document.getElementById("txtAppointmentTime" + idx);

    if (txtAppointmentTime.value.Trim() != "" &&
        !isDecimal(txtAppointmentTime.value))
    {
        res = false;
        lblMessage.innerHTML += GetErrorMessageNumber("Време за явяване") + "<br />";
    }

    var notValidFieldsCount = notValidFields.length;

    if (notValidFieldsCount > 0)
    {
    var noRightsMessage = GetErrorMessageNoRights(notValidFields);
    lblMessage.innerHTML += "<br />" + noRightsMessage;
    }

    if (res)
    {
        RefreshInputsOfSpecificContainer(document.getElementById("divCommand" + idx), true);
        lblMessage.className = "SuccessText";
    }
    else
        lblMessage.className = "ErrorText";

    return res;
}

//Client validation of Add Request Command light-box
function ValidateAddCommand()
{
    var res = true;

    var lblMessage = document.getElementById("spanAddRequestCommandLightBoxMessage");
    lblMessage.innerHTML = "";

    var notValidFields = new Array();

    var ddMilitaryCommands = document.getElementById("ddMilitaryCommands");

    if (ddMilitaryCommands.value.Trim() == "-1")
    {
        res = false;

        if (ddMilitaryCommands.disabled == true || ddMilitaryCommands.style.display == "none")
            notValidFields.push("Команда");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Команда") + "<br />";
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

//Client validation of Add/Edit Request Command Position light-box
function ValidateAddEditRequestCommandPosition()
{
    var res = true;

    var lblMessage = document.getElementById("spanAddEditRequestCommandPositionLightBox");
    lblMessage.innerHTML = "";

    var notValidFields = new Array();

    var txtPosition = document.getElementById("txtPosition");
    var txtReservistsCount = document.getElementById("txtReservistsCount");

    if (txtPosition.value.Trim() == "")
    {
        res = false;

        if (txtPosition.disabled == true || txtPosition.style.display == "none")
            notValidFields.push("Длъжност");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Длъжност") + "<br />";
    }

    if (txtReservistsCount.value.Trim() == "")
    {
        res = false;

        if (txtReservistsCount.disabled == true || txtReservistsCount.style.display == "none")
            notValidFields.push("Запасни");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Запасни") + "<br />";
    }
    else
    {
        if (!isInt(txtReservistsCount.value) || parseInt(txtReservistsCount.value) <= 0)
        {
            res = false;
            lblMessage.innerHTML += GetErrorMessageNumber("Запасни") + "<br />";
        }
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

//Show the Import Vacant Position light-box
function ImportVacantPositions(requestCommandId, idx)
{
    ShowImportVacantPositionsLightBox(requestCommandId, idx);
}

function ShowImportVacantPositionsLightBox(requestCommandId, idx)
{
    ClearAllMessages();

    document.getElementById("hdnImportRequestsCommandID").value = requestCommandId;
    document.getElementById("hdnImportIdx").value = idx;

    var militaryCommandName = document.getElementById("hdnMilitaryCommandName" + idx).value;
    var militaryCommandSuffix = document.getElementById("txtMilitaryCommandSuffix" + idx).value;

    document.getElementById("hdnLightBoxMilitaryCommandName").value = militaryCommandName;
    document.getElementById("hdnLightBoxMilitaryCommandSuffix").value = militaryCommandSuffix;

    var url = "AddEditEquipmentReservistsRequest.aspx?AjaxMethod=JSGetImportVacantPositionLightBox";
    var params = "LightBoxMilitaryCommandName=" + custEncodeURI(militaryCommandName) +
                 "&LightBoxMilitaryCommandSuffix=" + custEncodeURI(militaryCommandSuffix) +
                 "&RequestCommandId=" + requestCommandId;
                 
    function response_handler(xml)
    {
        document.getElementById('divImportVacantPositionsLightBoxContent').innerHTML = xmlNodeText(xml.childNodes[0]);
        document.getElementById("HidePage").style.display = "";
        document.getElementById("divImportVacantPositionsLightBox").style.display = "";
        CenterLightBox("divImportVacantPositionsLightBox");
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}

//Close the light-box
function HideImportVacantPositionsLightBox()
{
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("divImportVacantPositionsLightBox").style.display = "none";
}

//Filter the available vacant position (for import)
function FilterImportVacantPositionsLightBox()
{
    var url = "AddEditEquipmentReservistsRequest.aspx?AjaxMethod=JSGetImportVacantPositionLightBox";

    var params = "";
    params += "Position=" + custEncodeURI(document.getElementById("txtPositionFilt").value);
    params += "&PageIndex=" + document.getElementById("hdnPageIndex").value;
    params += "&MaxPage=" + document.getElementById("hdnPageMaxPage").value;
    params += "&OrderBy=" + document.getElementById("hdnOrderBy").value;
    params += "&LightBoxMilitaryCommandName=" + custEncodeURI(document.getElementById("hdnLightBoxMilitaryCommandName").value);
    params += "&LightBoxMilitaryCommandSuffix=" + custEncodeURI(document.getElementById("hdnLightBoxMilitaryCommandSuffix").value);
    params += "&RequestCommandId=" + document.getElementById("hdnImportRequestsCommandID").value;

    function response_handler(xml)
    {
        document.getElementById('divImportVacantPositionsLightBoxContent').innerHTML = xmlNodeText(xml.childNodes[0]);
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}

//Change the page of available vacant position (for import)
function BtnPagingClick(objectName)
{
    hdnPageIdx = document.getElementById('hdnPageIndex');
    hdnMaxPage = document.getElementById('hdnPageMaxPage');
    switch (objectName)
    {
        case "btnFirst":
            hdnPageIdx.value = 1;

            FilterImportVacantPositionsLightBox();
            break;

        case "btnPrev":
            pageIdx = parseInt(hdnPageIdx.value);

            if (pageIdx > 1)
            {
                pageIdx--;
                hdnPageIdx.value = pageIdx;
                FilterImportVacantPositionsLightBox();
            }

            break;

        case "btnNext":
            pageIdx = parseInt(hdnPageIdx.value);
            maxPage = parseInt(hdnMaxPage.value);

            if (pageIdx < maxPage)
            {
                pageIdx++;
                hdnPageIdx.value = pageIdx;
                FilterImportVacantPositionsLightBox();
            }
            break;


        case "btnLast":
            hdnPageIdx.value = hdnMaxPage.value;

            FilterImportVacantPositionsLightBox();
            break;

        case "btnPageGo":
            maxPage = parseInt(hdnMaxPage.value);
            goToPage = parseInt(document.getElementById('txtTableGotoPage').value);

            if (isInt(goToPage) && goToPage > 0 && goToPage <= maxPage)
            {
                hdnPageIdx.value = goToPage;
                FilterImportVacantPositionsLightBox();
            }
            break;

        default:
            break;
    }
}

//Sort the table of available vacant positions (for import)
function SortTableBy(sort)
{
    hdnPageIdx = document.getElementById('hdnPageIndex');
    hdnOrderBy = document.getElementById("hdnOrderBy");
    orderBy = parseInt(hdnOrderBy.value);

    if (orderBy == sort)
    {
        sort = sort + 100;
    }

    hdnOrderBy.value = sort;
    hdnPageIdx.value = 1; //We go to 1st page

    FilterImportVacantPositionsLightBox();
}

//Validate the Vacant Positions before doing the add/importing
function ValidateVacantPositions()
{
    // variables to prevent showing identical error messages in the lightbox
    var errMsg1 = false;
    var errMsg2 = false;

    var res = true;
    var lblMessage = document.getElementById("lblImportPositionsMessage");
    lblMessage.innerHTML = "";
    lblMessage.className = "ErrorText";

    var vacantPositionsCnt = parseInt(document.getElementById("hdnVacantPositionsCounter").value);

    for (var i = 1; i < vacantPositionsCnt; i++)
    {
        var positions = document.getElementById("txtVacantPositions" + i).value;
        var positionCnt = document.getElementById("positionCnt" + i).innerHTML;

        if (TrimString(positions) != "")
        {
            if (!isInt(positions))
            {
                res = false;
                if (!errMsg1)
                    lblMessage.innerHTML += GetErrorMessageNumber("Избрани позиции") + "<br />";
                errMsg1 = true;
            }
            else if (parseInt(positions) > parseInt(positionCnt))
            {
                res = false;
                if (!errMsg2)
                    lblMessage.innerHTML += "Стойността на полето \"Избрани позиции\" не трябва да надвишава възможния брой позиции<br />";
                errMsg2 = true;
            }
        }
    }

    return res;
}

//Import the selected vacant positions
function ImportSelectedVacantPositions()
{
    if (!ValidateVacantPositions())
    {
        return;
    }

    var url = "AddEditEquipmentReservistsRequest.aspx?AjaxMethod=JSImportVacantPositions";

    var params = "";

    var vacantPositionsCnt = parseInt(document.getElementById("hdnVacantPositionsCounter").value);

    params += "RequestsCommandId=" + document.getElementById("hdnImportRequestsCommandID").value;
    params += "&Idx=" + document.getElementById("hdnImportIdx").value;
    params += "&Count=" + vacantPositionsCnt;

    for (var i = 1; i < vacantPositionsCnt; i++)
    {
        var MaxVSST_ID = document.getElementById("hdnMaxVSST_ID" + i).value;
        var positions = document.getElementById("txtVacantPositions" + i).value;

        if (TrimString(positions) != "")
        {
            params += "&MaxVSST_ID" + i + "=" + MaxVSST_ID;
            params += "&Positions" + i + "=" + positions;
        }
    }

    function response_handler(xml)
    {
        var idx = document.getElementById("hdnImportIdx").value;
        var refreshedPositionsTable = xmlValue(xml, "refreshedPositionsTable");
        var importedCount = parseInt(xmlValue(xml, "importedCount"));

        document.getElementById("tdRequestCommandPositionsCont" + idx).innerHTML = refreshedPositionsTable;

        if (importedCount > 0)
        {
            document.getElementById("lblMessagePositions" + idx).className = "SuccessText";
            document.getElementById("lblMessagePositions" + idx).innerHTML = importedCount == 1 ? "Избраната длъжност е импортирана успешно" : "Избраните длъжности са импортирани успешно";
        }

        HideImportVacantPositionsLightBox();
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}

function MoveRequestCommandPosition(pIndex, pRequestsCommandID, pRequestCommandPositionID_1, pRequestCommandPositionID_2) {
  
    var url = "AddEditEquipmentReservistsRequest.aspx?AjaxMethod=JSMoveRequestCommandPosition";

    var params = "RequestCommandID=" + pRequestsCommandID +
                 "&RequestCommandPositionID_1=" + pRequestCommandPositionID_1 +
                 "&RequestCommandPositionID_2=" + pRequestCommandPositionID_2 +
                 "&Idx=" + pIndex;

    function response_handler(xml) {
        document.getElementById("tdRequestCommandPositionsCont" + pIndex).innerHTML = xmlValue(xml, "refreshedPositionsTable");        
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}