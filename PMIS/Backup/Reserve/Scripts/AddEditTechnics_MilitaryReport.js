

function btnAddNewTechMilRepStatus_Click()
{
    ShowAddNewResMilRepStatusLightBox(0);
}

function btnEditCurrStatusHTML_Click()
{
    var technicsMilRepStatusId = parseInt(document.getElementById("technicsMilRepStatusId").value);

    ShowAddNewResMilRepStatusLightBox(technicsMilRepStatusId);
}

var milRepStatuses = new Array(); // array with the keys of military report statuses
var milRepStatusId; // when in edit mode, here we kept current military report status
var milRepStatusKey; // when in edit mode, here we kept current military report status
function ShowAddNewResMilRepStatusLightBox(technicsMilRepStatusId)
{
    ClearAllMessages();

    document.getElementById("hdnTechnicsMilRepStatusID").value = technicsMilRepStatusId;

    document.getElementById("lblCurrentMilRepStatusValueLightBox").innerHTML = document.getElementById("lblCurrMilitaryReportStatusValue").innerHTML;
    document.getElementById("lblGeneralTabCurrMilitaryReportStatusValue").innerHTML = document.getElementById("lblCurrMilitaryReportStatusValue").innerHTML;

    //New record
    if (technicsMilRepStatusId == 0)
    {
        document.getElementById("lblNewMilRepStatus").style.display = "";
        document.getElementById("ddNewMilRepStatus").style.display = "";

        document.getElementById("lblAddEditMilRepStatusTitle").innerHTML = "Смяна на състоянието по отчета";

        var url = "AddEditTechnics_MilitaryReport.aspx?AjaxMethod=JSLoadNewMilRepStatuses";

        var params = "TechnicsId=" + document.getElementById(hdnTechnicsIdClientID).value;

        function response_handler1(xml)
        {
            ClearSelectList(document.getElementById("ddNewMilRepStatus"), true);

            var statuses = xml.getElementsByTagName("s");

            for (var i = 0; i < statuses.length; i++)
            {
                var id = xmlValue(statuses[i], "id");
                var key = xmlValue(statuses[i], "key");
                var name = xmlValue(statuses[i], "name");

                milRepStatuses[parseInt(id)] = key;

                AddToSelectList(document.getElementById("ddNewMilRepStatus"), id, name);
            };

            // prefill current military department
            var milDeptId = xmlValue(xml, "mildeptid");

            document.getElementById("ddCurrSourceMilDepartmentNameLightBox").value = milDeptId;

            MilitaryUnitSelectorUtil.SetSelectedValue("itmsVoluntaryFulfilPlace", "-1");
            MilitaryUnitSelectorUtil.SetSelectedText("itmsVoluntaryFulfilPlace", "");


            // clean message label in the light box and hide it
            document.getElementById("spanAddEditMilRepStatusLightBox").style.display = "none";
            document.getElementById("spanAddEditMilRepStatusLightBox").innerHTML = "";

            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("lboxAddEditMilRepStatus").style.display = "";
            CenterLightBox("lboxAddEditMilRepStatus");
        }

        var myAJAX = new AJAX(url, true, params, response_handler1);
        myAJAX.Call();
    }
    else //Edit status
    {
        document.getElementById("lblNewMilRepStatus").style.display = "none";
        document.getElementById("ddNewMilRepStatus").style.display = "none";

        document.getElementById("lblAddEditMilRepStatusTitle").innerHTML = "Редактиране на състоянието по отчета";

        var url = "AddEditTechnics_MilitaryReport.aspx?AjaxMethod=JSLoadTechnicsMilRepStatus";

        var params = "TechnicsMilRepStatusId=" + technicsMilRepStatusId;

        function response_handler(xml)
        {
            milRepStatusId = xmlValue(xml, "MilitaryReportStatusID");

            var militaryReportStatusKey = xmlValue(xml, "MilitaryReportStatusKey");

            milRepStatusKey = militaryReportStatusKey;

            document.getElementById("txtCurrEnrolDateLightBox").value = xmlValue(xml, "EnrolDate");
            document.getElementById("txtDischargeDateLightBox").value = xmlValue(xml, "DischargeDate");
            document.getElementById("ddCurrSourceMilDepartmentNameLightBox").value = xmlValue(xml, "SourceMilDepartmentID");

            document.getElementById("lblCurrEnrolDateLightBox").style.display = ""; //show enrol date
            document.getElementById("spanCurrEnrolDateLightBox").style.display = ""; //show enrol date
            document.getElementById("lblDischargeDateLightBox").style.display = ""; //show discharge date
            document.getElementById("spanDischargeDateLightBox").style.display = ""; //show discharge date
            
            switch (militaryReportStatusKey)
            {
                case "CONTRACT":
                    document.getElementById("txtContractContractNumberLightBox").value = xmlValue(xml, "ContractContractNumber");
                    MilitaryUnitSelectorUtil.SetSelectedValue("muContractMilitaryUnitLightBox", xmlValue(xml, "ContractMilitaryUnitID"));
                    MilitaryUnitSelectorUtil.SetSelectedText("muContractMilitaryUnitLightBox", xmlValue(xml, "ContractMilitaryUnitName"));
                    document.getElementById("txtContractContractFromDateLightBox").value = xmlValue(xml, "ContractContractFromDate");
                    document.getElementById("txtContractContractToDateLightBox").value = xmlValue(xml, "ContractContractToDate");                    
                    document.getElementById("divContract").style.display = "";
                    break;
                case "VOLUNTARY_RESERVE":
                    document.getElementById("txtVoluntaryContractNumberLightBox").value = xmlValue(xml, "VoluntaryContractNumber");
                    document.getElementById("txtVoluntaryContractDateLightBox").value = xmlValue(xml, "VoluntaryContractDate");
                    document.getElementById("txtVoluntaryDurationMonthsLightBox").value = xmlValue(xml, "VoluntaryDurationMonths");
                    document.getElementById("txtVoluntaryContractToDateLightBox").value = xmlValue(xml, "VoluntaryContractToDate");
                    
                    MilitaryUnitSelectorUtil.SetSelectedValue("itmsVoluntaryFulfilPlace", xmlValue(xml, "VoluntaryFulfilPlaceID"));
                    MilitaryUnitSelectorUtil.SetSelectedText("itmsVoluntaryFulfilPlace", xmlValue(xml, "VoluntaryFulfilPlaceText"));
                    
                    document.getElementById("divVoluntary").style.display = "";
                    break;
                case "REMOVED":
                    document.getElementById("txtRemovedDateLightBox").value = xmlValue(xml, "RemovedDate");
                    document.getElementById("ddRemovedReasonLightBox").value = xmlValue(xml, "RemovedReasonID");
                    document.getElementById("divRemoved").style.display = "";
                    break;              
                case "TEMPORARY_REMOVED":
                    document.getElementById("ddTemporaryRemovedReasonsLightBox").value = xmlValue(xml, "TemporaryRemovedReasonID");
                    document.getElementById("txtTemporaryRemovedDateLightBox").value = xmlValue(xml, "TemporaryRemovedDate");
                    document.getElementById("txtTemporaryRemovedDurationLightBox").value = xmlValue(xml, "TemporaryRemovedDuration");
                    document.getElementById("divTemporaryRemoved").style.display = "";
                    break;
                case "POSTPONED":
                    document.getElementById("ddPostponeTypeLightBox").value = xmlValue(xml, "TechnicsPostponeTypeID");
                    document.getElementById("txtPostponeYearLightBox").value = xmlValue(xml, "TechnicsPostponeYear");                    
                    document.getElementById("divPostpone").style.display = "";
                    break;             
            }

            // clean message label in the light box and hide it
            document.getElementById("spanAddEditMilRepStatusLightBox").style.display = "none";
            document.getElementById("spanAddEditMilRepStatusLightBox").innerHTML = "";

            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("lboxAddEditMilRepStatus").style.display = "";
            CenterLightBox("lboxAddEditMilRepStatus");
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

function HideAllAddEditMilRepStatusLightBoxDivs()
{
    document.getElementById("divContract").style.display = "none";
    document.getElementById("divVoluntary").style.display = "none";
    document.getElementById("divRemoved").style.display = "none";   
    document.getElementById("divTemporaryRemoved").style.display = "none";
    document.getElementById("divPostpone").style.display = "none";   
}

function ClearAddEditMilRepStatusLightBox()
{
    document.getElementById("lblCurrentMilRepStatusValueLightBox").value = "";
    document.getElementById("txtCurrEnrolDateLightBox").value = "";
    document.getElementById("txtDischargeDateLightBox").value = "";

    document.getElementById("txtContractContractNumberLightBox").value = "";
    MilitaryUnitSelectorUtil.SetSelectedValue("muContractMilitaryUnitLightBox", "0");
    MilitaryUnitSelectorUtil.SetSelectedText("muContractMilitaryUnitLightBox", "");
    document.getElementById("txtContractContractFromDateLightBox").value = "";
    document.getElementById("txtContractContractToDateLightBox").value = "";

    document.getElementById("txtVoluntaryContractNumberLightBox").value = "";
    document.getElementById("txtVoluntaryContractDateLightBox").value = "";
    document.getElementById("txtVoluntaryDurationMonthsLightBox").value = "";
    document.getElementById("txtVoluntaryContractToDateLightBox").value = "";
    
    MilitaryUnitSelectorUtil.SetSelectedValue("itmsVoluntaryFulfilPlace", "-1");
    MilitaryUnitSelectorUtil.SetSelectedText("itmsVoluntaryFulfilPlace", "");    
     
    document.getElementById("txtRemovedDateLightBox").value = "";   
    document.getElementById("txtTemporaryRemovedDateLightBox").value = "";
    document.getElementById("txtTemporaryRemovedDurationLightBox").value = "";
    document.getElementById("txtPostponeYearLightBox").value = "";          
      
    document.getElementById("ddRemovedReasonLightBox").value = ""; 
    document.getElementById("ddTemporaryRemovedReasonsLightBox").value = "";
    document.getElementById("ddPostponeTypeLightBox").value = "";

    var lightBoxMessage = document.getElementById("spanAddEditMilRepStatusLightBox");
    lightBoxMessage.innerHTML = "";
    lightBoxMessage.style.display = "none";

}

//Close the light-box
function HideAddEditMilRepStatusLightBox()
{
    HideAllAddEditMilRepStatusLightBoxDivs();
    ClearAddEditMilRepStatusLightBox();

    document.getElementById("HidePage").style.display = "none";
    document.getElementById("lboxAddEditMilRepStatus").style.display = "none";
}

// Handler of onchange event of NewMilRepStatus dropdown
function NewMilRepStatusChanged()
{
    var statusId = document.getElementById("ddNewMilRepStatus").value;

    var lblMessage = document.getElementById("spanAddEditMilRepStatusLightBox");
    lblMessage.innerHTML = "";
    
    HideAllAddEditMilRepStatusLightBoxDivs();

    document.getElementById("lblCurrEnrolDateLightBox").style.display = ""; //show enrol date
    document.getElementById("spanCurrEnrolDateLightBox").style.display = ""; //show enrol date
    document.getElementById("lblDischargeDateLightBox").style.display = ""; //show discharge date
    document.getElementById("spanDischargeDateLightBox").style.display = ""; //show discharge date
    
    switch (milRepStatuses[parseInt(statusId)])
    {
        case "CONTRACT":
            document.getElementById("divContract").style.display = "";
            break;
        case "VOLUNTARY_RESERVE":
            document.getElementById("divVoluntary").style.display = "";
            break;
        case "REMOVED":
            document.getElementById("divRemoved").style.display = "";
            break;        
        case "TEMPORARY_REMOVED":
            document.getElementById("divTemporaryRemoved").style.display = "";
            break;
        case "POSTPONED":
            document.getElementById("divPostpone").style.display = "";
            break;  
    }
}

function ValidateAddEditMilRepStatusLightBox()
{
    var res = true;
    var lblMessage = document.getElementById("spanAddEditMilRepStatusLightBox");
    lblMessage.innerHTML = "";

    var notValidFields = new Array();

    var ddNewMilRepStatus = document.getElementById("ddNewMilRepStatus");
    if (ddNewMilRepStatus.style.display == "")
    {
        if (ddNewMilRepStatus.value == "" || ddNewMilRepStatus.value == optionChooseOneValue)
        {
            res = false;

            lblMessage.innerHTML += GetErrorMessageMandatory("Ново състояние") + "<br />";
        }
    }

    var txtCurrEnrolDateLightBox = document.getElementById("txtCurrEnrolDateLightBox");
    if (TrimString(txtCurrEnrolDateLightBox.value) == "")
    {
        res = false;

        lblMessage.innerHTML += GetErrorMessageMandatory("Дата на промяна") + "<br />";
    }
    else if (!IsValidDate(txtCurrEnrolDateLightBox.value))
    {
        res = false;

        lblMessage.innerHTML += GetErrorMessageDate("Дата на промяна") + "<br />";
    }

    var txtDischargeDateLightBox = document.getElementById("txtDischargeDateLightBox");
    if (TrimString(txtDischargeDateLightBox.value) == "")
    {
        //for future needs
        //res = false;

        //lblMessage.innerHTML += GetErrorMessageMandatory("Снет от отчет") + "<br />";
    }
    else if (!IsValidDate(txtDischargeDateLightBox.value))
    {
        res = false;

        lblMessage.innerHTML += GetErrorMessageDate("Снет от отчет") + "<br />";
    }

    var ddCurrSourceMilDepartmentNameLightBox = document.getElementById("ddCurrSourceMilDepartmentNameLightBox");
    if (ddCurrSourceMilDepartmentNameLightBox.style.display == "") {
        if (ddCurrSourceMilDepartmentNameLightBox.value == "" || ddCurrSourceMilDepartmentNameLightBox.value == optionChooseOneValue) {
            res = false;

            lblMessage.innerHTML += GetErrorMessageMandatory("Военно окръжие") + "<br />";
        }
    }

    var key;
    if (ddNewMilRepStatus.style.display == "")
    {
        key = milRepStatuses[parseInt(ddNewMilRepStatus.value)];
    }
    else
    {
        key = milRepStatusKey;
    }

    switch (key)
    {
        case "CONTRACT":
            var txtContractContractFromDateLightBox = document.getElementById("txtContractContractFromDateLightBox");
            if (txtContractContractFromDateLightBox.value != "" && !IsValidDate(txtContractContractFromDateLightBox.value))
            {
                res = false;

                lblMessage.innerHTML += GetErrorMessageDate("Дата") + "<br />";
            }

            var txtContractContractToDateLightBox = document.getElementById("txtContractContractToDateLightBox");
            if (txtContractContractToDateLightBox.value != "" && !IsValidDate(txtContractContractToDateLightBox.value))
            {
                res = false;

                lblMessage.innerHTML += GetErrorMessageDate("До дата") + "<br />";
            }
            break;

        case "VOLUNTARY_RESERVE":
            var txtVoluntaryContractDateLightBox = document.getElementById("txtVoluntaryContractDateLightBox");
            if (txtVoluntaryContractDateLightBox.value != "" && !IsValidDate(txtVoluntaryContractDateLightBox.value)) {
                res = false;

                lblMessage.innerHTML += GetErrorMessageDate("от дата") + "<br />";
            }

            var txtVoluntaryDurationMonthsLightBox = document.getElementById("txtVoluntaryDurationMonthsLightBox");
            if (txtVoluntaryDurationMonthsLightBox.value != "" && !isInt(txtVoluntaryDurationMonthsLightBox.value)) {
                res = false;

                lblMessage.innerHTML += GetErrorMessageNumber("Срок") + "<br />";
            }

            var txtVoluntaryContractToDateLightBox = document.getElementById("txtVoluntaryContractToDateLightBox");
            if (txtVoluntaryContractToDateLightBox.value != "" && !IsValidDate(txtVoluntaryContractToDateLightBox.value)) {
                res = false;

                lblMessage.innerHTML += GetErrorMessageDate("до дата") + "<br />";
            }
            break;
        case "REMOVED":
            var txtRemovedDateLightBox = document.getElementById("txtRemovedDateLightBox");
            if (txtRemovedDateLightBox.value != "" && !IsValidDate(txtRemovedDateLightBox.value)) {
                res = false;

                lblMessage.innerHTML += GetErrorMessageDate("Изключен от отчет") + "<br />";
            }
            break;
        case "TEMPORARY_REMOVED":
            var txtTemporaryRemovedDateLightBox = document.getElementById("txtTemporaryRemovedDateLightBox");
            if (txtTemporaryRemovedDateLightBox.value != "" && !IsValidDate(txtTemporaryRemovedDateLightBox.value)) {
                res = false;

                lblMessage.innerHTML += GetErrorMessageDate("Начална дата") + "<br />";
            }

            var txtTemporaryRemovedDurationLightBox = document.getElementById("txtTemporaryRemovedDurationLightBox");
            if (txtTemporaryRemovedDurationLightBox.value != "" && !isInt(txtTemporaryRemovedDurationLightBox.value)) {
                res = false;

                lblMessage.innerHTML += GetErrorMessageNumber("Продължителност") + "<br />";
            }
            break;
        case "POSTPONED":
            var ddPostponeTypeLightBox = document.getElementById("ddPostponeTypeLightBox");

            if (ddPostponeTypeLightBox.value == "" || ddPostponeTypeLightBox.value == optionChooseOneValue) {
                res = false;

                if (ddPostponeTypeLightBox.disabled == true || ddPostponeTypeLightBox.style.display == "none")
                    notValidFields.push("Вид отсрочване");
                else
                    lblMessage.innerHTML += GetErrorMessageMandatory("Вид отсрочване") + "<br />";
            }

            var txtPostponeYearLightBox = document.getElementById("txtPostponeYearLightBox");
            if (txtPostponeYearLightBox.value == "") {
                res = false;

                if (txtPostponeYearLightBox.disabled == true || txtPostponeYearLightBox.style.display == "none")
                    notValidFields.push("За коя година");
                else
                    lblMessage.innerHTML += GetErrorMessageMandatory("За коя година") + "<br />";
            }
            else if (txtPostponeYearLightBox.value != "" && !isInt(txtPostponeYearLightBox.value)) {
                res = false;

                lblMessage.innerHTML += GetErrorMessageNumber("За коя година") + "<br />";
            }
            else if (parseInt(txtPostponeYearLightBox.value) < 2000 ||
                     parseInt(txtPostponeYearLightBox.value) > 3000) {
                res = false;
                lblMessage.innerHTML += "Годината е невалдна" + "<br />";    
            }
            break;
    }

    var notValidFieldsCount = notValidFields.length;
    var fieldsStr = '"' + notValidFields.join(", ") + '"';

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
        lblMessage.className = "ErrorText";

    return res;
}

// Saves MilitaryReportingStatus from LightBox
function SaveAddEditMilRepStatusLightBox()
{
    if (!ValidateAddEditMilRepStatusLightBox())
    {
        document.getElementById("spanAddEditMilRepStatusLightBox").style.display = "";
        return;
    }

    var statusId;
    var key;
    var ddNewMilRepStatus = document.getElementById("ddNewMilRepStatus");

    if (ddNewMilRepStatus.style.display == "")
    {
        statusId = ddNewMilRepStatus.value;
        key = milRepStatuses[parseInt(statusId)];
    }
    else
    {
        statusId = milRepStatusId;
        key = milRepStatusKey;
    }

    var url = "AddEditTechnics_MilitaryReport.aspx?AjaxMethod=JSSaveMilRepStatus";

    var params = "";
    params += "TechnicsID=" + document.getElementById(hdnTechnicsIdClientID).value;
    params += "&TechnicsMilRepStatusID=" + document.getElementById("hdnTechnicsMilRepStatusID").value;
    params += "&MilitaryReportStatusID=" + statusId;
    params += "&MilitaryReportStatusKey=" + key;
    params += "&EnrolDate=" + document.getElementById("txtCurrEnrolDateLightBox").value;
    params += "&DischargeDate=" + document.getElementById("txtDischargeDateLightBox").value;
    params += "&SourceMilDepartmentID=" + document.getElementById("ddCurrSourceMilDepartmentNameLightBox").value;

    switch (key)
    {
        case "CONTRACT":
            params += "&ContractContractNumber=" + document.getElementById("txtContractContractNumberLightBox").value;
            params += "&ContractMilitaryUnitID=" + MilitaryUnitSelectorUtil.GetSelectedValue("muContractMilitaryUnitLightBox");
            params += "&ContractContractFromDate=" + document.getElementById("txtContractContractFromDateLightBox").value;
            params += "&ContractContractToDate=" + document.getElementById("txtContractContractToDateLightBox").value;
            break;
        case "VOLUNTARY_RESERVE":
            params += "&VoluntaryContractNumber=" + document.getElementById("txtVoluntaryContractNumberLightBox").value;
            params += "&VoluntaryContractDate=" + document.getElementById("txtVoluntaryContractDateLightBox").value;
            params += "&VoluntaryDurationMonths=" + document.getElementById("txtVoluntaryDurationMonthsLightBox").value;
            params += "&VoluntaryContractToDate=" + document.getElementById("txtVoluntaryContractToDateLightBox").value;
            params += "&VoluntaryFulfilPlaceID=" + MilitaryUnitSelectorUtil.GetSelectedValue("itmsVoluntaryFulfilPlace");       
            break;
        case "REMOVED":
            params += "&RemovedDate=" + document.getElementById("txtRemovedDateLightBox").value;
            params += "&RemovedReasonID=" + document.getElementById("ddRemovedReasonLightBox").value;
            break;       
        case "TEMPORARY_REMOVED":
            params += "&TemporaryRemovedReasonID=" + document.getElementById("ddTemporaryRemovedReasonsLightBox").value;
            params += "&TemporaryRemovedDate=" + document.getElementById("txtTemporaryRemovedDateLightBox").value;
            params += "&TemporaryRemovedDuration=" + document.getElementById("txtTemporaryRemovedDurationLightBox").value;
            break;
        case "POSTPONED":
            params += "&TechnicsPostponeTypeID=" + document.getElementById("ddPostponeTypeLightBox").value;
            params += "&TechnicsPostponeYear=" + document.getElementById("txtPostponeYearLightBox").value;           
            break;
    }

    function response_handler(xml)
    {
        var hideDialog = true;
        var resultMsg = xmlValue(xml, "response");
        if (resultMsg != "OK")
        {
            var lightBoxMessage = document.getElementById("spanAddEditMilRepStatusLightBox");
            lightBoxMessage.innerHTML = "";
            lightBoxMessage.style.display = "";
            sectionMessage.className = "ErrorText"
            hideDialog = false;
            lightBoxMessage.innerHTML = "Неуспешен запис";
        }
        else
        {
            var sectionMessage = document.getElementById("spanMilRepStatusSectionMsg");
            sectionMessage.innerHTML = "";
            sectionMessage.style.display = "";
            sectionMessage.className = "SuccessText"
            sectionMessage.innerHTML = "Данните бяха записани успешно";

            var ddCurrSourceMilDepartmentNameLightBox = document.getElementById("ddCurrSourceMilDepartmentNameLightBox");

            if (document.getElementById("lblFirstEnrolDateValue").innerHTML == "")
                document.getElementById("lblFirstEnrolDateValue").innerHTML = document.getElementById("txtCurrEnrolDateLightBox").value;

            document.getElementById("lblCurrEnrolDateValue").innerHTML = document.getElementById("txtCurrEnrolDateLightBox").value;
            document.getElementById("txtDischargeDate").innerHTML = document.getElementById("txtDischargeDateLightBox").value;
            if (ddNewMilRepStatus.style.display == "")
            {
                document.getElementById("lblCurrMilitaryReportStatusValue").innerHTML = ddNewMilRepStatus.options[ddNewMilRepStatus.selectedIndex].text;
                document.getElementById("lblGeneralTabCurrMilitaryReportStatusValue").innerHTML = ddNewMilRepStatus.options[ddNewMilRepStatus.selectedIndex].text;                
            }

            if (ddCurrSourceMilDepartmentNameLightBox.selectedIndex != "" && ddCurrSourceMilDepartmentNameLightBox.selectedIndex != optionChooseOneValue)
            {
                document.getElementById("lblCurrSourceMilDepartmentNameValue").innerHTML = ddCurrSourceMilDepartmentNameLightBox.options[ddCurrSourceMilDepartmentNameLightBox.selectedIndex].text;
                document.getElementById("lblCurrMilDepartmentValue").innerHTML = ddCurrSourceMilDepartmentNameLightBox.options[ddCurrSourceMilDepartmentNameLightBox.selectedIndex].text;
            }
            else
            {
                document.getElementById("lblCurrSourceMilDepartmentNameValue").innerHTML = "";
                document.getElementById("lblCurrMilDepartmentValue").innerHTML = "";
            }
            document.getElementById("technicsMilRepStatusId").value = xmlValue(xml, "TechnicsMilRepStatusId");
            document.getElementById("btnEditCurrResMilRepStatus").style.display = "";

            HideAllTechnicsMilitaryReportStatusSectionDivs();
            switch (key)
            {
                case "CONTRACT":
                    document.getElementById("txtContractContractNumber").innerHTML = document.getElementById("txtContractContractNumberLightBox").value;
                    document.getElementById("txtContractContractFromDate").innerHTML = document.getElementById("txtContractContractFromDateLightBox").value;
                    document.getElementById("txtContractContractToDate").innerHTML = document.getElementById("txtContractContractToDateLightBox").value;
                    document.getElementById("txtContractMilitaryUnit").innerHTML = MilitaryUnitSelectorUtil.GetSelectedText("muContractMilitaryUnitLightBox");
                    document.getElementById("divSectionContract").style.display = "";
                    break;
                case "VOLUNTARY_RESERVE":
                    document.getElementById("txtVoluntaryContractNumber").innerHTML = document.getElementById("txtVoluntaryContractNumberLightBox").value;
                    document.getElementById("txtVoluntaryContractDate").innerHTML = document.getElementById("txtVoluntaryContractDateLightBox").value;
                    document.getElementById("txtVoluntaryDurationMonths").innerHTML = document.getElementById("txtVoluntaryDurationMonthsLightBox").value;
                    document.getElementById("txtVoluntaryContractToDate").innerHTML = document.getElementById("txtVoluntaryContractToDateLightBox").value;
                    document.getElementById("txtVoluntaryFulfilPlace").innerHTML = MilitaryUnitSelectorUtil.GetSelectedText("itmsVoluntaryFulfilPlace");

                    document.getElementById("divSectionVoluntary").style.display = "";
                    break;
                case "REMOVED":
                    document.getElementById("txtRemovedDate").innerHTML = document.getElementById("txtRemovedDateLightBox").value;
                    var ddRemovedReasonLightBox = document.getElementById("ddRemovedReasonLightBox");
                    if (ddRemovedReasonLightBox.selectedIndex != "" && ddRemovedReasonLightBox.selectedIndex != optionChooseOneValue)
                        document.getElementById("txtRemovedReason").innerHTML = ddRemovedReasonLightBox.options[ddRemovedReasonLightBox.selectedIndex].text;
                    else
                        document.getElementById("txtRemovedReason").innerHTML = "";
                    document.getElementById("divSectionRemoved").style.display = "";
                    break;               
                case "TEMPORARY_REMOVED":
                    var ddTemporaryRemovedReasonsLightBox = document.getElementById("ddTemporaryRemovedReasonsLightBox");
                    if (ddTemporaryRemovedReasonsLightBox.selectedIndex != "" && ddTemporaryRemovedReasonsLightBox.selectedIndex != optionChooseOneValue)
                        document.getElementById("txtTemporaryRemovedReason").innerHTML = ddTemporaryRemovedReasonsLightBox.options[ddTemporaryRemovedReasonsLightBox.selectedIndex].text;
                    else
                        document.getElementById("txtTemporaryRemovedReason").innerHTML = "";
                    document.getElementById("txtTemporaryRemovedDate").innerHTML = document.getElementById("txtTemporaryRemovedDateLightBox").value;
                    document.getElementById("txtTemporaryRemovedDuration").innerHTML = document.getElementById("txtTemporaryRemovedDurationLightBox").value;
                    document.getElementById("divSectionTemporaryRemoved").style.display = "";
                    break;
                case "POSTPONED":
                    var ddPostponeTypeLightBox = document.getElementById("ddPostponeTypeLightBox");
                    if (ddPostponeTypeLightBox.selectedIndex != "" && ddPostponeTypeLightBox.selectedIndex != optionChooseOneValue)
                        document.getElementById("txtPostponeType").innerHTML = ddPostponeTypeLightBox.options[ddPostponeTypeLightBox.selectedIndex].text;
                    else
                        document.getElementById("txtPostponeType").innerHTML = "";
                    document.getElementById("txtPostponeYear").innerHTML = document.getElementById("txtPostponeYearLightBox").value;                    
                    document.getElementById("divSectionPostpone").style.display = "";
                    break;       
            }

            document.getElementById("divGroupManagementSection").innerHTML = xmlValue(xml, "GroupManagementSection");
            document.getElementById("divTechnicsAppointmentSection").innerHTML = xmlValue(xml, "TechnicsAppointmentSection");
        }

        if (hideDialog)
            HideAddEditMilRepStatusLightBox();
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}

function HideAllTechnicsMilitaryReportStatusSectionDivs()
{
    document.getElementById("divSectionContract").style.display = "none";
    document.getElementById("divSectionVoluntary").style.display = "none";
    document.getElementById("divSectionRemoved").style.display = "none";   
    document.getElementById("divSectionTemporaryRemoved").style.display = "none";
    document.getElementById("divSectionPostpone").style.display = "none";    
}

function btnHistoryStatuses_Click()
{
    var url = "AddEditTechnics_MilitaryReport.aspx?AjaxMethod=JSLoadMilRepStatusHistory";

    var params = "TechnicsId=" + document.getElementById(hdnTechnicsIdClientID).value;

    function response_handler(xml)
    {
        document.getElementById("divMilRepStatusHistoryLightBox").innerHTML = xmlValue(xml, "response");

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("divMilRepStatusHistoryLightBox").style.display = "";
        CenterLightBox("divMilRepStatusHistoryLightBox");
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}

function HideTechnicsMilRepStatusHistoryLightBox()
{
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("divMilRepStatusHistoryLightBox").style.display = "none";
    document.getElementById("divMilRepStatusHistoryLightBox").innerHTML = "";
}

function BtnMilRepStatusHistoryPagingClick(objectName)
{
    hdnPageIdx = document.getElementById('hdnPageIndex');
    hdnMaxPage = document.getElementById('hdnPageMaxPage');
    switch (objectName)
    {
        case "btnFirst":
            hdnPageIdx.value = 1;

            RefreshMilRepStatusHistoryLightBox();
            break;

        case "btnPrev":
            pageIdx = parseInt(hdnPageIdx.value);

            if (pageIdx > 1)
            {
                pageIdx--;
                hdnPageIdx.value = pageIdx;
                RefreshMilRepStatusHistoryLightBox();
            }

            break;

        case "btnNext":
            pageIdx = parseInt(hdnPageIdx.value);
            maxPage = parseInt(hdnMaxPage.value);

            if (pageIdx < maxPage)
            {
                pageIdx++;
                hdnPageIdx.value = pageIdx;
                RefreshMilRepStatusHistoryLightBox();
            }
            break;


        case "btnLast":
            hdnPageIdx.value = hdnMaxPage.value;

            RefreshMilRepStatusHistoryLightBox();
            break;

        case "btnPageGo":
            maxPage = parseInt(hdnMaxPage.value);
            goToPage = parseInt(document.getElementById('txtTableGotoPage').value);

            if (isInt(goToPage) && goToPage > 0 && goToPage <= maxPage)
            {
                hdnPageIdx.value = goToPage;
                RefreshMilRepStatusHistoryLightBox();
            }
            break;

        default:
            break;
    }

}

function RefreshMilRepStatusHistoryLightBox()
{
    var url = "AddEditTechnics_MilitaryReport.aspx?AjaxMethod=JSLoadMilRepStatusHistory";

    var params = "";
    params += "TechnicsId=" + document.getElementById(hdnTechnicsIdClientID).value;
    params += "&PageIndex=" + document.getElementById("hdnPageIndex").value;
    params += "&MaxPage=" + document.getElementById("hdnPageMaxPage").value;
    params += "&OrderBy=" + document.getElementById("hdnOrderBy").value;

    function response_handler(xml)
    {
        document.getElementById('divMilRepStatusHistoryLightBox').innerHTML = xmlValue(xml, "response");
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}

function btnHistoryAppointments_Click()
{
    var url = "AddEditTechnics_MilitaryReport.aspx?AjaxMethod=JSLoadTechnicsAppointmentHistory";

    var params = "TechnicsId=" + document.getElementById(hdnTechnicsIdClientID).value;

    function response_handler(xml)
    {
        document.getElementById("divTechnicsAppointmentHistoryLightBox").innerHTML = xmlValue(xml, "response");

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("divTechnicsAppointmentHistoryLightBox").style.display = "";
        CenterLightBox("divTechnicsAppointmentHistoryLightBox");
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}

function HideTechnicsAppointmentHistoryLightBox()
{
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("divTechnicsAppointmentHistoryLightBox").style.display = "none";
    document.getElementById("divTechnicsAppointmentHistoryLightBox").innerHTML = "";
}

function BtnTechnicsAppointmentHistoryPagingClick(objectName)
{
    hdnPageIdx = document.getElementById('hdnPageIndex');
    hdnMaxPage = document.getElementById('hdnPageMaxPage');
    switch (objectName)
    {
        case "btnFirst":
            hdnPageIdx.value = 1;

            RefreshTechnicsAppointmentHistoryLightBox();
            break;

        case "btnPrev":
            pageIdx = parseInt(hdnPageIdx.value);

            if (pageIdx > 1)
            {
                pageIdx--;
                hdnPageIdx.value = pageIdx;
                RefreshTechnicsAppointmentHistoryLightBox();
            }

            break;

        case "btnNext":
            pageIdx = parseInt(hdnPageIdx.value);
            maxPage = parseInt(hdnMaxPage.value);

            if (pageIdx < maxPage)
            {
                pageIdx++;
                hdnPageIdx.value = pageIdx;
                RefreshTechnicsAppointmentHistoryLightBox();
            }
            break;


        case "btnLast":
            hdnPageIdx.value = hdnMaxPage.value;

            RefreshTechnicsAppointmentHistoryLightBox();
            break;

        case "btnPageGo":
            maxPage = parseInt(hdnMaxPage.value);
            goToPage = parseInt(document.getElementById('txtTableGotoPage').value);

            if (isInt(goToPage) && goToPage > 0 && goToPage <= maxPage)
            {
                hdnPageIdx.value = goToPage;
                RefreshTechnicsAppointmentHistoryLightBox();
            }
            break;

        default:
            break;
    }

}

function RefreshTechnicsAppointmentHistoryLightBox()
{
    var url = "AddEditTechnics_MilitaryReport.aspx?AjaxMethod=JSLoadTechnicsAppointmentHistory";

    var params = "";
    params += "TechnicsId=" + document.getElementById(hdnTechnicsIdClientID).value;
    params += "&PageIndex=" + document.getElementById("hdnPageIndex").value;
    params += "&MaxPage=" + document.getElementById("hdnPageMaxPage").value;
    params += "&OrderBy=" + document.getElementById("hdnOrderBy").value;

    function response_handler(xml)
    {
        document.getElementById('divTechnicsAppointmentHistoryLightBox').innerHTML = xmlValue(xml, "response");
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}

function SaveMilitaryReportTab(saveMilitaryReportTabFinishCallback)
{
    if (IsTabAlreadyVisited("btnTabMilitaryReport")) {

        var url = "AddEditTechnics_MilitaryReport.aspx?AjaxMethod=JSSaveMilitaryReportTab";

        var params = "TechnicsId=" + document.getElementById(hdnTechnicsIdClientID).value +
                "&GroupManagementSection=" + custEncodeURI(document.getElementById("txtGMSGroupManagementSection").value) +
                "&Section=" + custEncodeURI(document.getElementById("txtGMSSection").value) +
                "&Deliverer=" + custEncodeURI(document.getElementById("txtGMSDeliverer").value) +
                "&PunktId=" + document.getElementById("ddGMSPunkt").value;

        if (document.getElementById("cbAppointmentIsDelivered"))
            params += "&AppointmentIsDelivered=" + (document.getElementById("cbAppointmentIsDelivered").checked ? "1" : "0");

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    } else {
        saveMilitaryReportTabFinishCallback();
    }
    
    function response_handler(xml) {
        RefreshInputsOfSpecificContainer(document.getElementById(divMilitaryReportClientID), true);
        saveMilitaryReportTabFinishCallback();
    }
}

function ValidateMilitaryReportTab()
{
    var tabNameHeader = "Военно-отчетни: ";
    var ValidationMessage = "";

    if (IsTabAlreadyVisited("btnTabMilitaryReport")) {

        var notValidFields = new Array();

        var notValidFieldsCount = notValidFields.length;
        var fieldsStr = '"' + notValidFields.join(", ") + '"';

        if (notValidFieldsCount > 0) {
            var noRightsMessage = tabNameHeader + GetErrorMessageNoRights(notValidFields);
            ValidationMessage += "<br />" + noRightsMessage;
        }
    }
    return ValidationMessage;
}

function PrintMK() {
    var technicsId = document.getElementById(hdnTechnicsIdClientID).value;
    location.href = "AddEditTechnics_MilitaryReport.aspx?AjaxMethod=JSPrintMK&TechnicsId=" + technicsId;
}

function PrintPZ() {
    var technicsId = document.getElementById(hdnTechnicsIdClientID).value;
    location.href = "AddEditTechnics_MilitaryReport.aspx?AjaxMethod=JSPrintPZ&TechnicsId=" + technicsId;
}

function PrintOK() {
    var technicsId = document.getElementById(hdnTechnicsIdClientID).value;
    location.href = "AddEditTechnics_MilitaryReport.aspx?AjaxMethod=JSPrintOK&TechnicsId=" + technicsId;
}

function PrintTO() {
    var technicsId = document.getElementById(hdnTechnicsIdClientID).value;
    location.href = "AddEditTechnics_MilitaryReport.aspx?AjaxMethod=JSPrintTO&TechnicsId=" + technicsId;
}