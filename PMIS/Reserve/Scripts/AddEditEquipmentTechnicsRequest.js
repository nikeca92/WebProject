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
        var equipmentTechnicsRequestId = document.getElementById(hfEquipmentTechnicsRequestIDClientID).value;
        equipmentTechnicsRequestId = parseInt(equipmentTechnicsRequestId);

        if (!isNaN(equipmentTechnicsRequestId) && equipmentTechnicsRequestId > 0)
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
        var url = "AddEditEquipmentTechnicsRequest.aspx?AjaxMethod=JSPopulateMilitaryCommands";

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
        var url = "AddEditEquipmentTechnicsRequest.aspx?AjaxMethod=JSAddRequestCommand";

        var params = "EquipmentTechnicsRequestId=" + document.getElementById(hfEquipmentTechnicsRequestIDClientID).value +
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
function DeleteRequestCommand(technicsRequestCommandId, idx)
{
    ClearAllMessages();

    YesNoDialog("Желаете ли да изтриете командата и всички нейни позиции?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "AddEditEquipmentTechnicsRequest.aspx?AjaxMethod=JSDeleteRequestCommand";

        var params = "TechnicsRequestCommandId=" + technicsRequestCommandId;

        function response_handler(xml)
        {
            var status = xmlValue(xml, "status")

            if (status == "OK")
            {
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

    if (btnSaveReqCmdCount == 0)
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

    var url = "AddEditEquipmentTechnicsRequest.aspx?AjaxMethod=JSPopulateDeliveryMunicipality";

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

    var url = "AddEditEquipmentTechnicsRequest.aspx?AjaxMethod=JSPopulateDeliveryCity";

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

function CallSaveRequestCommand(button, doAppointmentUpdate) {
    var requestCommandId = button.getAttribute('data-technicsRequestCommandId');
    var idx = button.getAttribute('data-idx');
    SaveRequestCommand(requestCommandId, idx, doAppointmentUpdate);
}

//Save the "header" of a particular Technics Request Command
function SaveRequestCommand(technicsRequestCommandId, idx, doAppointmentUpdate)
{
    ClearAllMessages();

    if (ValidateCommand(idx))
    {
        var url = "AddEditEquipmentTechnicsRequest.aspx?AjaxMethod=JSSaveRequestCommand";

        var params = "TechnicsRequestCommandId=" + technicsRequestCommandId +
                     "&MilitaryCommandSuffix=" + custEncodeURI(document.getElementById("txtMilitaryCommandSuffix" + idx).value) +
                     "&DeliveryCityId=" + document.getElementById("DeliveryCity" + idx).value +
                     "&DeliveryPlace=" + custEncodeURI(document.getElementById("txtDeliveryPlace" + idx).value) +
                     "&EquipmentTechnicsRequestId=" + document.getElementById(hfEquipmentTechnicsRequestIDClientID).value +
                     "&MilitaryCommandId=" + custEncodeURI(document.getElementById("hdnMilitaryCommandId" + idx).value) +
                     "&AppointmentTime=" + custEncodeURI(document.getElementById("txtAppointmentTime" + idx).value) +
                     "&MilReadinessId=" + custEncodeURI(document.getElementById("ddMilReadiness" + idx).value) +
                     "&DoAppointmentUpdate=" + doAppointmentUpdate; ;

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


function AddPositionManually(technicsRequestCommandId, idx)
{
    ShowAddEditRequestCommandPositionLightBox(technicsRequestCommandId, 0, idx);
}


function ShowAddEditRequestCommandPositionLightBox(technicsRequestCommandId, technicsRequestCommandPositionId, idx)
{
    ClearAllMessages();

    var militaryCommandName = document.getElementById("hdnMilitaryCommandName" + idx).value;
    var militaryCommandSuffix = document.getElementById("txtMilitaryCommandSuffix" + idx).value;

    document.getElementById("lblMilitaryCommandLabelLightBox").innerHTML = militaryCommandName + " " + militaryCommandSuffix;

    document.getElementById("hdnTechnicsRequestCоmmandPositionID").value = technicsRequestCommandPositionId;
    document.getElementById("hdnSelectedCommandIdx").value = idx;
    document.getElementById("hdnSelectedTechRequestsCommandID").value = technicsRequestCommandId;

    SetClientTextAreaMaxLength("txtComment", "1000");

    //New position
    if (technicsRequestCommandPositionId == 0)
    {
        document.getElementById("lblAddEditReqCommandPosTitle").innerHTML = "Въвеждане на нова позиция";

        document.getElementById(ddTechnicsTypeClientID).value = optionChooseOneValue;
        document.getElementById(ddTechnicsTypeClientID).disabled = false;

        var onRepopulateNormativeTechnics = function() {
            document.getElementById("txtComment").value = "";
            document.getElementById("txtCount").value = "";
            document.getElementById("txtDriversCount").value = "";

            // clean message label in the light box and hide it
            document.getElementById("spanAddEditRequestCommandPositionLightBox").style.display = "none";
            document.getElementById("spanAddEditRequestCommandPositionLightBox").innerHTML = "";

            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("AddEditRequestCommandPositionLightBox").style.display = "";
            CenterLightBox("AddEditRequestCommandPositionLightBox");

            var dd = document.getElementById("ddNormativeTechnics");
            if (dd) {
                var seltxt = dd.options[dd.selectedIndex].text;
                dd.title = seltxt;
            }
        }

        RepopulateNormativeTechnics(document.getElementById(ddTechnicsTypeClientID), onRepopulateNormativeTechnics);
    }
    else //Edit Position
    {
        document.getElementById("lblAddEditReqCommandPosTitle").innerHTML = "Редактиране на позиция";

        var url = "AddEditEquipmentTechnicsRequest.aspx?AjaxMethod=JSLoadRequestCommandPosition";

        var params = "TechnicsRequestCommandPositionId=" + technicsRequestCommandPositionId;

        function response_handler(xml)
        {
            var technicsRequestCommandPosition = xml.getElementsByTagName("technicsRequestCommandPosition")[0];

            var technicsRequestCommandPositionId = xmlValue(technicsRequestCommandPosition, "technicsRequestCommandPositionID");
            var technicsRequestsCommandId = xmlValue(technicsRequestCommandPosition, "technicsRequestsCommandID");
            var technicsTypeId = xmlValue(technicsRequestCommandPosition, "technicsTypeId");

            var normativeTechnicsId = xmlValue(technicsRequestCommandPosition, "normativeTechnicsId");
            
            var normativeCode = xmlValue(technicsRequestCommandPosition, "normativeCode");
            
            var comment = xmlValue(technicsRequestCommandPosition, "comment");
            var count = xmlValue(technicsRequestCommandPosition, "count");
            var driversCount = xmlValue(technicsRequestCommandPosition, "driversCount");
            var fulfilCount = xmlValue(technicsRequestCommandPosition, "fulfilCount");

            document.getElementById(ddTechnicsTypeClientID).value = technicsTypeId;
            document.getElementById(ddTechnicsTypeClientID).disabled = parseInt(fulfilCount) > 0;

            var onRepopulateNormativeTechnics = function() {
                document.getElementById("txtComment").value = comment;
                document.getElementById("txtCount").value = count;
                document.getElementById("txtDriversCount").value = driversCount;
                
                if (normativeTechnicsId != "") {
                    document.getElementById("txtNormativeCode").value = normativeCode;
                    document.getElementById("ddNormativeTechnics").value = normativeTechnicsId;
                }

                // clean message label in the light box and hide it
                document.getElementById("spanAddEditRequestCommandPositionLightBox").style.display = "none";
                document.getElementById("spanAddEditRequestCommandPositionLightBox").innerHTML = "";

                //shows the light box and "disable" rest of the page
                document.getElementById("HidePage").style.display = "";
                document.getElementById("AddEditRequestCommandPositionLightBox").style.display = "";
                CenterLightBox("AddEditRequestCommandPositionLightBox");

                var dd = document.getElementById("ddNormativeTechnics");
                if (dd) {
                    var seltxt = dd.options[dd.selectedIndex].text;
                    dd.title = seltxt;
                }
            }

            RepopulateNormativeTechnics(document.getElementById(ddTechnicsTypeClientID), onRepopulateNormativeTechnics);
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Save Add/Edit RequestCommandPosition
function SaveAddEditRequestCommandPositionLightBox()
{
    if (ValidateAddEditRequestCommandPosition())
    {
        var idx = document.getElementById("hdnSelectedCommandIdx").value;

        var url = "AddEditEquipmentTechnicsRequest.aspx?AjaxMethod=JSSaveRequestCommandPosition";

        var params = "TechnicsRequestCommandPositionId=" + document.getElementById("hdnTechnicsRequestCоmmandPositionID").value +
                 "&TechnicsRequestCommandId=" + document.getElementById("hdnSelectedTechRequestsCommandID").value +
                 "&TechnicsTypeId=" + document.getElementById(ddTechnicsTypeClientID).value +
                 "&NormativeTechnicsId=" + document.getElementById("ddNormativeTechnics").value +
                 "&Comment=" + custEncodeURI(document.getElementById("txtComment").value) +
                 "&Count=" + document.getElementById("txtCount").value +
                 "&DriversCount=" + document.getElementById("txtDriversCount").value +
                 "&Idx=" + idx;

        function response_handler(xml)
        {
            var refreshedPositionsTable = xmlValue(xml, "refreshedPositionsTable");

            document.getElementById("tdRequestCommandPositionsCont" + idx).innerHTML = refreshedPositionsTable;

            document.getElementById("lblMessagePositions" + idx).className = "SuccessText";
            document.getElementById("lblMessagePositions" + idx).innerHTML = document.getElementById("hdnTechnicsRequestCоmmandPositionID").value == "0" ? "Позицията е добавена успешно" : "Записът на позицията е успешен";

            HideAddEditRequestCommandPositionLightBox();
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

// Close the light box
function HideAddEditRequestCommandPositionLightBox()
{
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("AddEditRequestCommandPositionLightBox").style.display = "none";
}

//Edit a particular Technics Request Command Position which has been added manually
function EditRequestCommandPosition(technicsRequestCommandId, technicsRequestCommandPositionId, idx)
{
    ShowAddEditRequestCommandPositionLightBox(technicsRequestCommandId, technicsRequestCommandPositionId, idx);
}

//Delete a particular Technics Request Command Position (i.e. not via the Import)
function DeleteRequestCommandPosition(technicsRequestCommandId, technicsRequestCommandPositionId, idx)
{
    ClearAllMessages();

    YesNoDialog("Желаете ли да изтриете позицията?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "AddEditEquipmentTechnicsRequest.aspx?AjaxMethod=JSDeleteRequestCommandPosition";

        var params = "TechnicsRequestCommandPositionId=" + technicsRequestCommandPositionId +
                 "&TechnicsRequestCommandId=" + technicsRequestCommandId +
                 "&Idx=" + idx;

        function response_handler(xml)
        {
            if (xmlValue(xml, "msg") != "") {
                AlertDialog(xmlValue(xml, "msg"), null);
            } else {
                var refreshedPositionsTable = xmlValue(xml, "refreshedPositionsTable");

                document.getElementById("tdRequestCommandPositionsCont" + idx).innerHTML = refreshedPositionsTable;

                document.getElementById("lblMessagePositions" + idx).className = "SuccessText";
                document.getElementById("lblMessagePositions" + idx).innerHTML = "Позицията е изтрита успешно";
            }
            
            HideAddEditRequestCommandPositionLightBox();
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
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

    var ddTechnicsType = document.getElementById(ddTechnicsTypeClientID);
    var ddNormativeTechnics = document.getElementById("ddNormativeTechnics");
    var txtCount = document.getElementById("txtCount");
    var txtDriversCount = document.getElementById("txtDriversCount");

    if (ddTechnicsType.value.Trim() == optionChooseOneValue)
    {
        res = false;

        if (ddTechnicsType.disabled == true || ddTechnicsType.style.display == "none")
            notValidFields.push("Вид техника");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Вид техника") + "<br />";
    }

    if (ddNormativeTechnics.value.Trim() == optionChooseOneValue) {
        res = false;

        if (ddNormativeTechnics.disabled == true || ddNormativeTechnics.style.display == "none")
            notValidFields.push("Нормативна категория");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Нормативна категория") + "<br />";
    }

    if (txtCount.value.Trim() == "")
    {
        res = false;

        if (txtCount.disabled == true || txtCount.style.display == "none")
            notValidFields.push("Брой");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Брой") + "<br />";
    }
    else
    {
        if (!isInt(txtCount.value) || parseInt(txtCount.value) <= 0)
        {
            res = false;
            lblMessage.innerHTML += GetErrorMessageNumber("Брой") + "<br />";
        }
    }

    if (txtDriversCount.value.Trim() == "") {
        res = false;

        if (txtDriversCount.disabled == true || txtDriversCount.style.display == "none")
            notValidFields.push("Водачи");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Водачи") + "<br />";
    }
    else {
        if (!isInt(txtDriversCount.value) || parseInt(txtDriversCount.value) < 0) {
            res = false;
            lblMessage.innerHTML += GetErrorMessageNumber("Водачи") + "<br />";
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

function RepopulateNormativeTechnics(ddTechnicsTypes, onRepopulate) {
    var url = "AddEditEquipmentTechnicsRequest.aspx?AjaxMethod=JSRepopulateNormativeTechnics";
    var params = "";
    params += "TechnicsTypeId=" + ddTechnicsTypes.value;
    
    function RepopulateNormativeTechnics_Callback(xml) {
        document.getElementById("txtNormativeCode").value = "";
        ClearSelectList(document.getElementById("ddNormativeTechnics"), true);

        var normativeTechnics = xml.getElementsByTagName("n");

        for (var i = 0; i < normativeTechnics.length; i++) {
            var id = xmlValue(normativeTechnics[i], "id");
            var name = xmlValue(normativeTechnics[i], "name");

            AddToSelectList(document.getElementById("ddNormativeTechnics"), id, name,name);
        };

        if (onRepopulate != null) {
            onRepopulate();
        }
    }

    var myAJAX = new AJAX(url, true, params, RepopulateNormativeTechnics_Callback);
    myAJAX.Call();
}

function ddNormativeTechnics_Changed() {
    var dd = document.getElementById("ddNormativeTechnics");
    if (dd) {
        var seltxt = dd.options[dd.selectedIndex].text;
        dd.title = seltxt;
    }
    var url = "AddEditEquipmentTechnicsRequest.aspx?AjaxMethod=JSGetNormativeTechnicsCode";
    var params = "";
    params += "NormativeTechnicsId=" + document.getElementById("ddNormativeTechnics").value;
    
    function GetNormativeTechnicsCode_Callback(xml) {
        var normativeCode = xmlValue(xml, "normativeCode");
        document.getElementById("txtNormativeCode").value = normativeCode;
    }

    var myAJAX = new AJAX(url, true, params, GetNormativeTechnicsCode_Callback);
    myAJAX.Call();
}

function txtNormativeCode_Focus() {
    var txtNormativeCode = document.getElementById("txtNormativeCode");
    txtNormativeCode.setAttribute("oldvalue", txtNormativeCode.value);
}

function txtNormativeCode_Blur() {
    var txtNormativeCode = document.getElementById("txtNormativeCode");

    if (txtNormativeCode.value != txtNormativeCode.getAttribute("oldvalue")) {
        if (txtNormativeCode.value == "") {
            document.getElementById("ddNormativeTechnics").value = optionChooseOneValue;
        }
        else {
            var url = "AddEditEquipmentTechnicsRequest.aspx?AjaxMethod=JSGetNormativeTechnicsId";
            var params = "";
            params += "NormativeCode=" + txtNormativeCode.value;
            params += "&TechnicsTypeId=" + document.getElementById(ddTechnicsTypeClientID).value;
            
            function GetNormativeTechnicsId_Callback(xml) {
                var normativeTechnicsId = xmlValue(xml, "normativeTechnicsId");

                if (parseInt(normativeTechnicsId) > 0) {
                    document.getElementById("ddNormativeTechnics").value = normativeTechnicsId;
                }
                else {
                    txtNormativeCode.value = txtNormativeCode.getAttribute("oldvalue");
                }
            }

            var myAJAX = new AJAX(url, true, params, GetNormativeTechnicsId_Callback);
            myAJAX.Call();
        }
    }
}

function MoveTechnicsRequestCommandPosition(pIndex, pTechnicsRequestsCommandID, pTechnicsRequestCommandPositionID_1, pTechnicsRequestCommandPositionID_2) {

    var url = "AddEditEquipmentTechnicsRequest.aspx?AjaxMethod=JSMoveTechnicsRequestCommandPosition";

    var params = "TechnicsRequestCommandID=" + pTechnicsRequestsCommandID +
                 "&TechnicsRequestCommandPositionID_1=" + pTechnicsRequestCommandPositionID_1 +
                 "&TechnicsRequestCommandPositionID_2=" + pTechnicsRequestCommandPositionID_2 +
                 "&Idx=" + pIndex;

    function response_handler(xml) {
        document.getElementById("tdRequestCommandPositionsCont" + pIndex).innerHTML = xmlValue(xml, "refreshedPositionsTable");      
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}