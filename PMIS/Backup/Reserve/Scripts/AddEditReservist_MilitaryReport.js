//Clear all messages on the MilitaryReport tab
function ClearMilitaryReportMessages()
{
    var sectionMessage = document.getElementById("spanMilRepStatusSectionMsg");
    if (sectionMessage)
    {
        sectionMessage.innerHTML = "";
        sectionMessage.style.display = "none";
    }
}

function btnAddNewResMilRepStatus_Click()
{
    ShowAddNewResMilRepStatusLightBox(0);
}

function btnEditCurrStatusHTML_Click()
{
    var reservistMilRepStatusId = parseInt(document.getElementById("reservistMilRepStatusId").value);

    ShowAddNewResMilRepStatusLightBox(reservistMilRepStatusId);
}

var milRepStatuses = new Array(); // array with the keys of military report statuses
var milRepStatusId; // when in edit mode, here we kept current military report status
var milRepStatusKey; // when in edit mode, here we kept current military report status
function ShowAddNewResMilRepStatusLightBox(reservistMilRepStatusId)
{
    ClearAllMessages();

    document.getElementById("hdnReservistMilRepStatusID").value = reservistMilRepStatusId;

    document.getElementById("lblCurrentMilRepStatusValueLightBox").innerHTML = document.getElementById("lblCurrMilitaryReportStatusValue").innerHTML;

    //New record
    if (reservistMilRepStatusId == 0)
    {
        document.getElementById("lblNewMilRepStatus").style.display = "";
        document.getElementById("ddNewMilRepStatus").style.display = "";
    
        document.getElementById("lblAddEditMilRepStatusTitle").innerHTML = "Смяна на състоянието по отчета";

        var url = "AddEditReservist_MilitaryReport.aspx?AjaxMethod=JSLoadNewMilRepStatuses";

        var params = "ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

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

        var url = "AddEditReservist_MilitaryReport.aspx?AjaxMethod=JSLoadReservistMilRepStatus";

        var params = "ReservistMilRepStatusId=" + reservistMilRepStatusId;

        function response_handler(xml)
        {
            milRepStatusId = xmlValue(xml, "MilitaryReportStatusID");
            
            var militaryReportStatusKey = xmlValue(xml, "MilitaryReportStatusKey");

            milRepStatusKey = militaryReportStatusKey;

            document.getElementById("txtCurrEnrolDateLightBox").value = xmlValue(xml, "EnrolDate");
            document.getElementById("ddCurrSourceMilDepartmentNameLightBox").value = xmlValue(xml, "SourceMilDepartmentID");

            document.getElementById("lblCurrEnrolDateLightBox").style.display = ""; //show enrol date
            document.getElementById("spanCurrEnrolDateLightBox").style.display = ""; //show enrol date
            switch (militaryReportStatusKey)
            {
                case "VOLUNTARY_RESERVE":
                    document.getElementById("txtVoluntaryContractNumberLightBox").value = xmlValue(xml, "VoluntaryContractNumber");
                    document.getElementById("txtVoluntaryContractDateLightBox").value = xmlValue(xml, "VoluntaryContractDate");
                    document.getElementById("txtVoluntaryExpireDateLightBox").value = xmlValue(xml, "VoluntaryExpireDate");
                    document.getElementById("txtVoluntaryDurationMonthsLightBox").value = xmlValue(xml, "VoluntaryDurationMonths");

                    var annexesXML = xml.getElementsByTagName("Annex");
                    for (var i = 0; i < annexesXML.length; i++) {
                        var annexID = xmlValue(annexesXML[i], "AnnexID");
                        var annexNumber = xmlValue(annexesXML[i], "AnnexNumber");
                        var annexDate = xmlValue(annexesXML[i], "AnnexDate");
                        var annexDurationMonths = xmlValue(annexesXML[i], "AnnexDurationMonths");
                        var annexExpireDate = xmlValue(annexesXML[i], "AnnexExpireDate");
                        RenderAnnexRowLightBox(annexID, annexNumber, annexDate, annexDurationMonths, annexExpireDate, i + 1);
                    }
                    
                    RefreshAnnexRows();
                    CheckDisabledClientControls();
                    CheckHiddenClientControls();

                    MilitaryUnitSelectorUtil.SetSelectedValue("itmsVoluntaryFulfilPlace", xmlValue(xml, "VoluntaryFulfilPlaceID"));
                    MilitaryUnitSelectorUtil.SetSelectedText("itmsVoluntaryFulfilPlace", xmlValue(xml, "VoluntaryFulfilPlaceText"));
                    document.getElementById("ddVoluntaryMilitaryRankLightBox").value = xmlValue(xml, "VoluntaryMilitaryRankID");
                    document.getElementById("txtVoluntaryMilitaryPositionLightBox").value = xmlValue(xml, "VoluntaryMilitaryPosition");
                    document.getElementById("ddVoluntaryMilRepSpecTypeLightBox").value = xmlValue(xml, "VoluntaryMilRepSpecialityTypeID");
                    if (document.getElementById("ddVoluntaryMilRepSpecTypeLightBox").value != "") {
                        ClearSelectList(document.getElementById("ddVoluntaryMilRepSpecLightBox"), true);

                        var statuses = xml.getElementsByTagName("mrs");

                        for (var i = 0; i < statuses.length; i++) {
                            var id = xmlValue(statuses[i], "id");
                            var name = xmlValue(statuses[i], "name");

                            AddToSelectList(document.getElementById("ddVoluntaryMilRepSpecLightBox"), id, name);
                        };
                    }
                    document.getElementById("ddVoluntaryMilRepSpecLightBox").value = xmlValue(xml, "VoluntaryMilRepSpecialityID");
                    document.getElementById("divVoluntary").style.display = "";
                    break;
                case "REMOVED":
                    document.getElementById("txtRemovedDateLightBox").value = xmlValue(xml, "RemovedDate");
                    document.getElementById("ddRemovedReasonLightBox").value = xmlValue(xml, "RemovedReasonID");
                    document.getElementById("txtRemovedDeceasedDeathCertLightBox").value = xmlValue(xml, "RemovedDeceasedDeathCert");
                    document.getElementById("txtRemovedDeceasedDateLightBox").value = xmlValue(xml, "RemovedDeceasedDate");
                    document.getElementById("txtRemovedAgeLimitOrderLightBox").value = xmlValue(xml, "RemovedAgeLimitOrder");
                    document.getElementById("txtRemovedAgeLimitDateLightBox").value = xmlValue(xml, "RemovedAgeLimitDate");
                    document.getElementById("txtRemovedAgeLimitSignedByLightBox").value = xmlValue(xml, "RemovedAgeLimitSignedBy");
                    document.getElementById("txtRemovedNotSuitableCertLightBox").value = xmlValue(xml, "RemovedNotSuitableCert");
                    document.getElementById("txtRemovedNotSuitableDateLightBox").value = xmlValue(xml, "RemovedNotSuitableDate");
                    document.getElementById("txtRemovedNotSuitableSignedByLightBox").value = xmlValue(xml, "RemovedNotSuitableSignedBy");

                    var ddRemovedReasonLightBox = document.getElementById("ddRemovedReasonLightBox");
                    var removedReason = "";
                    if (ddRemovedReasonLightBox.selectedIndex != "" && ddRemovedReasonLightBox.selectedIndex != optionChooseOneValue) {
                        removedReason = ddRemovedReasonLightBox.options[ddRemovedReasonLightBox.selectedIndex].text;
                    }
                    if (removedReason == "Починал") {
                        document.getElementById("rowRemovedDeceasedLightBox").style.display = "";
                        document.getElementById("rowRemovedAgeLimit1LightBox").style.display = "none";
                        document.getElementById("rowRemovedAgeLimit2LightBox").style.display = "none";
                        document.getElementById("rowRemovedNotSuitable1LightBox").style.display = "none";
                        document.getElementById("rowRemovedNotSuitable2LightBox").style.display = "none";
                    }
                    else if (removedReason == "Пределна възраст") {
                        document.getElementById("rowRemovedAgeLimit1LightBox").style.display = "";
                        document.getElementById("rowRemovedAgeLimit2LightBox").style.display = "";
                        document.getElementById("rowRemovedDeceasedLightBox").style.display = "none";
                        document.getElementById("rowRemovedNotSuitable1LightBox").style.display = "none";
                        document.getElementById("rowRemovedNotSuitable2LightBox").style.display = "none";
                    }
                    else if (removedReason == "НГВС с изключване") {
                        document.getElementById("rowRemovedNotSuitable1LightBox").style.display = "";
                        document.getElementById("rowRemovedNotSuitable2LightBox").style.display = "";
                        document.getElementById("rowRemovedDeceasedLightBox").style.display = "none";
                        document.getElementById("rowRemovedAgeLimit1LightBox").style.display = "none";
                        document.getElementById("rowRemovedAgeLimit2LightBox").style.display = "none";
                    }

                    document.getElementById("divRemoved").style.display = "";
                    break;
                case "MILITARY_EMPLOYED": /*This status is removed from the DB in BMoD, but we left it in the code just in case*/
                    document.getElementById("ddMilEmplAdministrationLightBox").value = xmlValue(xml, "MilEmployedAdministrationID");
                    document.getElementById("txtMilEmplDateLightBox").value = xmlValue(xml, "MilEmployedDate");
                    document.getElementById("divMilEmployed").style.display = "";
                    break;
                case "TEMPORARY_REMOVED":
                    document.getElementById("ddTemporaryRemovedReasonsLightBox").value = xmlValue(xml, "TemporaryRemovedReasonID");
                    document.getElementById("txtTemporaryRemovedDateLightBox").value = xmlValue(xml, "TemporaryRemovedDate");
                    document.getElementById("txtTemporaryRemovedDurationLightBox").value = xmlValue(xml, "TemporaryRemovedDuration");
                    document.getElementById("divTemporaryRemoved").style.display = "";
                    break;
                case "POSTPONED":
                    document.getElementById("ddPostponeTypeLightBox").value = xmlValue(xml, "PostponeTypeID");
                    document.getElementById("txtPostponeYearLightBox").value = xmlValue(xml, "PostponeYear");
                    document.getElementById("divPostpone").style.display = "";
                    break;
                case "DISCHARGED":
                    document.getElementById("ddDestMilDepartmentLightBox").value = xmlValue(xml, "DestMilDepartmentID");
                    document.getElementById("txtDischargeDateLightBox").value = xmlValue(xml, "DischargeDate");
                    document.getElementById("lblCurrEnrolDateLightBox").style.display = "none"; //hide enrol date
                    document.getElementById("spanCurrEnrolDateLightBox").style.display = "none"; //hide enrol date
                    document.getElementById("divDischarged").style.display = "";
                    break;
                case "MILITARY_REPORT_PERSONS":
                    document.getElementById("ddCurrSourceMilDepartmentNameLightBox").className = "InputField";
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

function HideAllAddEditMilRepStatusLightBoxDivs() {
    document.getElementById("ddCurrSourceMilDepartmentNameLightBox").className = "RequiredInputField";
    
    document.getElementById("divVoluntary").style.display = "none";
    document.getElementById("divRemoved").style.display = "none";
    document.getElementById("divMilEmployed").style.display = "none";
    document.getElementById("divTemporaryRemoved").style.display = "none";
    document.getElementById("divPostpone").style.display = "none";
    document.getElementById("divDischarged").style.display = "none";
}

function ClearAddEditMilRepStatusLightBox()
{
    document.getElementById("lblCurrentMilRepStatusValueLightBox").value = "";
    document.getElementById("txtCurrEnrolDateLightBox").value = "";
    document.getElementById("txtVoluntaryContractNumberLightBox").value = "";
    document.getElementById("txtVoluntaryContractDateLightBox").value = "";
    document.getElementById("txtVoluntaryDurationMonthsLightBox").value = "";
    MilitaryUnitSelectorUtil.SetSelectedValue("itmsVoluntaryFulfilPlace", "-1");
    MilitaryUnitSelectorUtil.SetSelectedText("itmsVoluntaryFulfilPlace", "");    
    document.getElementById("txtVoluntaryMilitaryPositionLightBox").value = "";
    document.getElementById("txtRemovedDateLightBox").value = "";
    document.getElementById("txtMilEmplDateLightBox").value = "";
    document.getElementById("txtTemporaryRemovedDateLightBox").value = "";
    document.getElementById("txtTemporaryRemovedDurationLightBox").value = "";
    document.getElementById("txtPostponeYearLightBox").value = "";
    document.getElementById("txtDischargeDateLightBox").value = "";

    document.getElementById("ddNewMilRepStatus").value = "";
    document.getElementById("ddCurrSourceMilDepartmentNameLightBox").value = "";
    document.getElementById("ddDestMilDepartmentLightBox").value = "";
    document.getElementById("ddVoluntaryMilitaryRankLightBox").value = "";
    document.getElementById("ddVoluntaryMilRepSpecTypeLightBox").value = "";
    document.getElementById("ddRemovedReasonLightBox").value = "";
    document.getElementById("ddMilEmplAdministrationLightBox").value = "";
    document.getElementById("ddTemporaryRemovedReasonsLightBox").value = "";
    document.getElementById("ddPostponeTypeLightBox").value = "";
    document.getElementById("ddVoluntaryMilRepSpecLightBox").value = "";

    ClearSelectList(document.getElementById("ddVoluntaryMilRepSpecLightBox"), true);

    var lightBoxMessage = document.getElementById("spanAddEditMilRepStatusLightBox");
    lightBoxMessage.innerHTML = "";
    lightBoxMessage.style.display = "none";

    ClearAnnexRowsLightBox();
    HideAllAddEditMilRepStatusLightBoxRows();
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
    CheckDisabledClientControls();
    CheckHiddenClientControls();
    
    document.getElementById("lblCurrEnrolDateLightBox").style.display = ""; //show enrol date
    document.getElementById("spanCurrEnrolDateLightBox").style.display = ""; //show enrol date
    switch (milRepStatuses[parseInt(statusId)])
    {
        case "VOLUNTARY_RESERVE":
            RefreshAnnexRows();
            document.getElementById("divVoluntary").style.display = "";
            break;
        case "REMOVED":
            document.getElementById("divRemoved").style.display = "";       
            break;
        case "MILITARY_EMPLOYED": /*This status is removed from the DB in BMoD, but we left it in the code just in case*/
            document.getElementById("divMilEmployed").style.display = "";
            break;
        case "TEMPORARY_REMOVED":
            document.getElementById("divTemporaryRemoved").style.display = "";
            break;
        case "POSTPONED":
            document.getElementById("divPostpone").style.display = "";
            break;
        case "DISCHARGED":
            document.getElementById("txtDischargeDateLightBox").value = document.getElementById("txtDischargeDateLightBox").getAttribute("defaultvalue");
            document.getElementById("lblCurrEnrolDateLightBox").style.display = "none"; //hide enrol date
            document.getElementById("spanCurrEnrolDateLightBox").style.display = "none"; //hide enrol date
            document.getElementById("divDischarged").style.display = "";
            break;
        case "MILITARY_REPORT_PERSONS":
            document.getElementById("ddCurrSourceMilDepartmentNameLightBox").className = "InputField";
            break;
    }
}


// Handler of onchange event of VoluntaryMilRepSpecType dropdown in the Light Box
function VoluntaryMilRepSpecTypeLightBoxChanged()
{
    var url = "AddEditReservist_MilitaryReport.aspx?AjaxMethod=JSLoadMilRepSpecs";

    var params = "MilRepSpecTypeID=" + document.getElementById("ddVoluntaryMilRepSpecTypeLightBox").value;

    function response_handler(xml)
    {
        ClearSelectList(document.getElementById("ddVoluntaryMilRepSpecLightBox"), true);

        var milRepSpecs = xml.getElementsByTagName("m");

        for (var i = 0; i < milRepSpecs.length; i++)
        {
            var id = xmlValue(milRepSpecs[i], "id");
            var name = xmlValue(milRepSpecs[i], "name");

            AddToSelectList(document.getElementById("ddVoluntaryMilRepSpecLightBox"), id, name, true);
        };
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
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

    var key;
    if (ddNewMilRepStatus.style.display == "") {
        key = milRepStatuses[parseInt(ddNewMilRepStatus.value)];
    }
    else {
        key = milRepStatusKey;
    }

    var txtCurrEnrolDateLightBox = document.getElementById("txtCurrEnrolDateLightBox");
    if (TrimString(txtCurrEnrolDateLightBox.value) == "") {
        if (key != "DISCHARGED") {
            res = false;

            lblMessage.innerHTML += GetErrorMessageMandatory("Дата на промяна") + "<br />";
        }
    }
    else if (!IsValidDate(txtCurrEnrolDateLightBox.value))
    {
        res = false;

        lblMessage.innerHTML += GetErrorMessageDate("Дата на промяна") + "<br />";
    }

    var ddCurrSourceMilDepartmentNameLightBox = document.getElementById("ddCurrSourceMilDepartmentNameLightBox");
    if (ddCurrSourceMilDepartmentNameLightBox.style.display == "" && ddCurrSourceMilDepartmentNameLightBox.className == "RequiredInputField") {
        if (ddCurrSourceMilDepartmentNameLightBox.value == "" || ddCurrSourceMilDepartmentNameLightBox.value == optionChooseOneValue) {
            res = false;

            lblMessage.innerHTML += GetErrorMessageMandatory("Военно окръжие") + "<br />";
        }
    }

    switch (key)
    {
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

            var txtVoluntaryExpireDateLightBox = document.getElementById("txtVoluntaryExpireDateLightBox");
            if (txtVoluntaryExpireDateLightBox.value != "" && !IsValidDate(txtVoluntaryExpireDateLightBox.value)) {
                res = false;

                lblMessage.innerHTML += GetErrorMessageDate("изтича на") + "<br />";
            }
            
            var rows = GetElementsByClassNameCustom("rowAnnexLightBox");
            for (var i = 1; i <= rows.length; i++) 
            {
                if (document.getElementById("hdnIsDeletedLightBox" + i).value == "no") 
                {
                    var txtAnnexDateLightBox = document.getElementById("txtAnnexDateLightBox" + i);
                    if (txtAnnexDateLightBox.value != "" && !IsValidDate(txtAnnexDateLightBox.value)) {
                        res = false;

                        lblMessage.innerHTML += GetErrorMessageDate("от дата") + "<br />";
                    }

                    var txtAnnexDurationMonthsLightBox = document.getElementById("txtAnnexDurationMonthsLightBox" + i);
                    if (txtAnnexDurationMonthsLightBox.value != "" && !isInt(txtAnnexDurationMonthsLightBox.value)) {
                        res = false;

                        lblMessage.innerHTML += GetErrorMessageNumber("Срок") + "<br />";
                    }

                    var txtAnnexExpireDateLightBox = document.getElementById("txtAnnexExpireDateLightBox" + i);
                    if (txtAnnexExpireDateLightBox.value != "" && !IsValidDate(txtAnnexExpireDateLightBox.value)) {
                        res = false;

                        lblMessage.innerHTML += GetErrorMessageDate("изтича на") + "<br />";
                    }
                }
            }
            break;
        case "REMOVED":
            var txtRemovedDateLightBox = document.getElementById("txtRemovedDateLightBox");
            if (txtRemovedDateLightBox.value != "" && !IsValidDate(txtRemovedDateLightBox.value))
            {
                res = false;

                lblMessage.innerHTML += GetErrorMessageDate("Изключен от отчет") + "<br />";
            }
            
            var txtRemovedNotSuitableDateLightBox = document.getElementById("txtRemovedNotSuitableDateLightBox");
            if (txtRemovedNotSuitableDateLightBox.value != "" && !IsValidDate(txtRemovedNotSuitableDateLightBox.value))
            {
                res = false;

                lblMessage.innerHTML += GetErrorMessageDate("Дата на удостоверение") + "<br />";
            }
            
            var txtRemovedDeceasedDateLightBox = document.getElementById("txtRemovedDeceasedDateLightBox");
            if (txtRemovedDeceasedDateLightBox.value != "" && !IsValidDate(txtRemovedDeceasedDateLightBox.value))
            {
                res = false;

                lblMessage.innerHTML += GetErrorMessageDate("Дата на смъртен акт") + "<br />";
            }
            
            var txtRemovedAgeLimitDateLightBox = document.getElementById("txtRemovedAgeLimitDateLightBox");
            if (txtRemovedAgeLimitDateLightBox.value != "" && !IsValidDate(txtRemovedAgeLimitDateLightBox.value))
            {
                res = false;

                lblMessage.innerHTML += GetErrorMessageDate("Дата на заповед") + "<br />";
            }
            break;
        case "MILITARY_EMPLOYED": /*This status is removed from the DB in BMoD, but we left it in the code just in case*/
            var txtMilEmplDateLightBox = document.getElementById("txtMilEmplDateLightBox");
            if (txtMilEmplDateLightBox.value != "" && !IsValidDate(txtMilEmplDateLightBox.value))
            {
                res = false;

                lblMessage.innerHTML += GetErrorMessageDate("Дата") + "<br />";
            }
            break;
        case "TEMPORARY_REMOVED":
            var txtTemporaryRemovedDateLightBox = document.getElementById("txtTemporaryRemovedDateLightBox");
            if (txtTemporaryRemovedDateLightBox.value != "" && !IsValidDate(txtTemporaryRemovedDateLightBox.value))
            {
                res = false;

                lblMessage.innerHTML += GetErrorMessageDate("Начална дата") + "<br />";
            }

            var txtTemporaryRemovedDurationLightBox = document.getElementById("txtTemporaryRemovedDurationLightBox");
            if (txtTemporaryRemovedDurationLightBox.value != "" && !isInt(txtTemporaryRemovedDurationLightBox.value))
            {
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
        case "DISCHARGED":
            var txtDischargeDateLightBox = document.getElementById("txtDischargeDateLightBox");

            if (txtDischargeDateLightBox.value == "")
            {
                res = false;

                lblMessage.innerHTML += GetErrorMessageMandatory("Дата на отчисляване") + "<br />";
            }
            else if (txtDischargeDateLightBox.value != "" && !IsValidDate(txtDischargeDateLightBox.value))
            {
                res = false;

                lblMessage.innerHTML += GetErrorMessageDate("Дата на отчисляване") + "<br />";
            }

            var ddDestMilDepartmentLightBox = document.getElementById("ddDestMilDepartmentLightBox");
            if (ddDestMilDepartmentLightBox.style.display == "")
            {
                if (ddDestMilDepartmentLightBox.value == "" || ddDestMilDepartmentLightBox.value == optionChooseOneValue)
                {
                    res = false;

                    lblMessage.innerHTML += GetErrorMessageMandatory("ВО, в което отива") + "<br />";
                }
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
    
    var url = "AddEditReservist_MilitaryReport.aspx?AjaxMethod=JSSaveMilRepStatus";

    var params = "";
    params += "ReservistId=" + document.getElementById(hdnReservistIdClientID).value;
    params += "&ReservistMilRepStatusID=" + document.getElementById("hdnReservistMilRepStatusID").value;
    params += "&MilitaryReportStatusID=" + statusId;
    params += "&MilitaryReportStatusKey=" + key;
    params += "&EnrolDate=" + document.getElementById("txtCurrEnrolDateLightBox").value;
    params += "&SourceMilDepartmentID=" + document.getElementById("ddCurrSourceMilDepartmentNameLightBox").value;

    switch (key)
    {
        case "VOLUNTARY_RESERVE":
            params += "&VoluntaryContractNumber=" + document.getElementById("txtVoluntaryContractNumberLightBox").value;
            params += "&VoluntaryContractDate=" + document.getElementById("txtVoluntaryContractDateLightBox").value;
            params += "&VoluntaryExpireDate=" + document.getElementById("txtVoluntaryExpireDateLightBox").value;
            params += "&VoluntaryDurationMonths=" + document.getElementById("txtVoluntaryDurationMonthsLightBox").value;

            var count = GetElementsByClassNameCustom("rowAnnexLightBox").length;
            params += "&AnnexesCount=" + count;
            for (var i = 1; i <= count; i++) {
                params += "&AnnexID" + i + "=" + document.getElementById("VoluntaryReserveAnnexID" + i).value;
                params += "&IsDeleted" + i + "=" + document.getElementById("hdnIsDeletedLightBox" + i).value;
                params += "&AnnexNumber" + i + "=" + document.getElementById("txtAnnexNumberLightBox" + i).value;
                params += "&AnnexDate" + i + "=" + document.getElementById("txtAnnexDateLightBox" + i).value;
                params += "&AnnexDurationMonths" + i + "=" + document.getElementById("txtAnnexDurationMonthsLightBox" + i).value;
                params += "&AnnexExpireDate" + i + "=" + document.getElementById("txtAnnexExpireDateLightBox" + i).value;
            }

            params += "&VoluntaryFulfilPlaceID=" + MilitaryUnitSelectorUtil.GetSelectedValue("itmsVoluntaryFulfilPlace");
            params += "&VoluntaryMilitaryRankID=" + document.getElementById("ddVoluntaryMilitaryRankLightBox").value;
            params += "&VoluntaryMilitaryPosition=" + document.getElementById("txtVoluntaryMilitaryPositionLightBox").value;
            params += "&VoluntaryMilRepSpecialityID=" + document.getElementById("ddVoluntaryMilRepSpecLightBox").value;
            break;
        case "REMOVED":
            params += "&RemovedDate=" + document.getElementById("txtRemovedDateLightBox").value;
            params += "&RemovedReasonID=" + document.getElementById("ddRemovedReasonLightBox").value;
            params += "&RemovedDeceasedDeathCert=" + document.getElementById("txtRemovedDeceasedDeathCertLightBox").value;
            params += "&RemovedDeceasedDate=" + document.getElementById("txtRemovedDeceasedDateLightBox").value;
            params += "&RemovedAgeLimitOrder=" + document.getElementById("txtRemovedAgeLimitOrderLightBox").value;
            params += "&RemovedAgeLimitDate=" + document.getElementById("txtRemovedAgeLimitDateLightBox").value;
            params += "&RemovedAgeLimitSignedBy=" + document.getElementById("txtRemovedAgeLimitSignedByLightBox").value;
            params += "&RemovedNotSuitableCert=" + document.getElementById("txtRemovedNotSuitableCertLightBox").value;
            params += "&RemovedNotSuitableDate=" + document.getElementById("txtRemovedNotSuitableDateLightBox").value;
            params += "&RemovedNotSuitableSignedBy=" + document.getElementById("txtRemovedNotSuitableSignedByLightBox").value;
            break;
        case "MILITARY_EMPLOYED": /*This status is removed from the DB in BMoD, but we left it in the code just in case*/
            params += "&MilEmployedAdministrationID=" + document.getElementById("ddMilEmplAdministrationLightBox").value;
            params += "&MilEmployedDate=" + document.getElementById("txtMilEmplDateLightBox").value;
            break;
        case "TEMPORARY_REMOVED":
            params += "&TemporaryRemovedReasonID=" + document.getElementById("ddTemporaryRemovedReasonsLightBox").value;
            params += "&TemporaryRemovedDate=" + document.getElementById("txtTemporaryRemovedDateLightBox").value;
            params += "&TemporaryRemovedDuration=" + document.getElementById("txtTemporaryRemovedDurationLightBox").value;
            break;
        case "POSTPONED":
            params += "&PostponeTypeID=" + document.getElementById("ddPostponeTypeLightBox").value;
            params += "&PostponeYear=" + document.getElementById("txtPostponeYearLightBox").value;
            break;
        case "DISCHARGED":
            params += "&DestMilDepartmentID=" + document.getElementById("ddDestMilDepartmentLightBox").value;
            params += "&DischargeDate=" + document.getElementById("txtDischargeDateLightBox").value;
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
            if (ddNewMilRepStatus.style.display == "")
            {
                document.getElementById("lblCurrMilitaryReportStatusValue").innerHTML = ddNewMilRepStatus.options[ddNewMilRepStatus.selectedIndex].text;                
                document.getElementById("lblMilitaryReportStatusValue").innerHTML = ddNewMilRepStatus.options[ddNewMilRepStatus.selectedIndex].text;
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

            var currPersonStatus = document.getElementById("lblPersonStatusValue");
            var newPersonStatus = ""
            if (ddNewMilRepStatus.selectedIndex != "" && ddNewMilRepStatus.selectedIndex != optionChooseOneValue) {
                newPersonStatus = ddNewMilRepStatus.options[ddNewMilRepStatus.selectedIndex].text;
            }
            if (currPersonStatus.innerHTML == statusRemovedLbl || currPersonStatus.innerHTML == statusReserveLbl || currPersonStatus.innerHTML == statusNALbl) 
            {
                if (newPersonStatus == "Изключен") {
                    currPersonStatus.innerHTML = statusRemovedLbl;
                }
                else if (newPersonStatus != "Изключен" && newPersonStatus != "") {
                    currPersonStatus.innerHTML = statusReserveLbl;
                }
            }
            
            document.getElementById("reservistMilRepStatusId").value = xmlValue(xml, "ReservistMilRepStatusId");
            document.getElementById("btnEditCurrResMilRepStatus").style.display = "";

            HideAllReservistMilitaryReportStatusSectionDivs();
            ClearAnnexRows();
            
            switch (key)
            {
                case "VOLUNTARY_RESERVE":
                    document.getElementById("txtVoluntaryContractNumber").innerHTML = document.getElementById("txtVoluntaryContractNumberLightBox").value;
                    document.getElementById("txtVoluntaryContractDate").innerHTML = document.getElementById("txtVoluntaryContractDateLightBox").value;
                    document.getElementById("txtVoluntaryDurationMonths").innerHTML = document.getElementById("txtVoluntaryDurationMonthsLightBox").value;
                    document.getElementById("txtVoluntaryExpireDate").innerHTML = document.getElementById("txtVoluntaryExpireDateLightBox").value;

                    var count = GetElementsByClassNameCustom("rowAnnexLightBox").length;
                    for (var i = 1; i <= count; i++)
                    {
                        if (document.getElementById("hdnIsDeletedLightBox" + i).value == "no") {
                            var annexNumber = document.getElementById("txtAnnexNumberLightBox" + i).value;
                            var annexDate = document.getElementById("txtAnnexDateLightBox" + i).value;
                            var annexDurationMonths = document.getElementById("txtAnnexDurationMonthsLightBox" + i).value;
                            var annexExpireDate = document.getElementById("txtAnnexExpireDateLightBox" + i).value;
                            RenderAnnexRow(annexNumber, annexDate, annexDurationMonths, annexExpireDate, i);
                        }
                    }

                    document.getElementById("txtVoluntaryFulfilPlace").innerHTML = MilitaryUnitSelectorUtil.GetSelectedText("itmsVoluntaryFulfilPlace");
                    var ddVoluntaryMilitaryRankLightBox = document.getElementById("ddVoluntaryMilitaryRankLightBox");
                    if (ddVoluntaryMilitaryRankLightBox.selectedIndex != "" && ddVoluntaryMilitaryRankLightBox.selectedIndex != optionChooseOneValue)
                        document.getElementById("txtVoluntaryMilitaryRank").innerHTML = ddVoluntaryMilitaryRankLightBox.options[ddVoluntaryMilitaryRankLightBox.selectedIndex].text;
                    else
                        document.getElementById("txtVoluntaryMilitaryRank").innerHTML = "";
                    document.getElementById("txtVoluntaryMilitaryPosition").innerHTML = document.getElementById("txtVoluntaryMilitaryPositionLightBox").value;
                    var ddVoluntaryMilRepSpecTypeLightBox = document.getElementById("ddVoluntaryMilRepSpecTypeLightBox");
                    if (ddVoluntaryMilRepSpecTypeLightBox.selectedIndex != "" && ddVoluntaryMilRepSpecTypeLightBox.selectedIndex != optionChooseOneValue)
                        document.getElementById("txtVoluntaryMilRepSpecType").innerHTML = ddVoluntaryMilRepSpecTypeLightBox.options[ddVoluntaryMilRepSpecTypeLightBox.selectedIndex].text;
                    else
                        document.getElementById("txtVoluntaryMilRepSpecType").innerHTML = "";
                    var ddVoluntaryMilRepSpecLightBox = document.getElementById("ddVoluntaryMilRepSpecLightBox");
                    if (ddVoluntaryMilRepSpecLightBox.selectedIndex != "" && ddVoluntaryMilRepSpecLightBox.selectedIndex != optionChooseOneValue)
                        document.getElementById("txtVoluntaryMilRepSpec").innerHTML = ddVoluntaryMilRepSpecLightBox.options[ddVoluntaryMilRepSpecLightBox.selectedIndex].text;
                    else
                        document.getElementById("txtVoluntaryMilRepSpec").innerHTML = "";
                    document.getElementById("divSectionVoluntary").style.display = "";
                    break;
                case "REMOVED":
                    document.getElementById("txtRemovedDate").innerHTML = document.getElementById("txtRemovedDateLightBox").value;
                    var ddRemovedReasonLightBox = document.getElementById("ddRemovedReasonLightBox");

                    if (ddRemovedReasonLightBox.selectedIndex != "" && ddRemovedReasonLightBox.selectedIndex != optionChooseOneValue)
                        document.getElementById("txtRemovedReason").innerHTML = ddRemovedReasonLightBox.options[ddRemovedReasonLightBox.selectedIndex].text;
                    else
                        document.getElementById("txtRemovedReason").innerHTML = "";

                    document.getElementById("txtRemovedDeceasedDeathCert").innerHTML = document.getElementById("txtRemovedDeceasedDeathCertLightBox").value;
                    document.getElementById("txtRemovedDeceasedDate").innerHTML = document.getElementById("txtRemovedDeceasedDateLightBox").value;
                    document.getElementById("txtRemovedAgeLimitOrder").innerHTML = document.getElementById("txtRemovedAgeLimitOrderLightBox").value;
                    document.getElementById("txtRemovedAgeLimitDate").innerHTML = document.getElementById("txtRemovedAgeLimitDateLightBox").value;
                    document.getElementById("txtRemovedAgeLimitSignedBy").innerHTML = document.getElementById("txtRemovedAgeLimitSignedByLightBox").value;
                    document.getElementById("txtRemovedNotSuitableCert").innerHTML = document.getElementById("txtRemovedNotSuitableCertLightBox").value;
                    document.getElementById("txtRemovedNotSuitableDate").innerHTML = document.getElementById("txtRemovedNotSuitableDateLightBox").value;
                    document.getElementById("txtRemovedNotSuitableSignedBy").innerHTML = document.getElementById("txtRemovedNotSuitableSignedByLightBox").value;

                    var removedReason = "";
                    if (ddRemovedReasonLightBox.selectedIndex != "" && ddRemovedReasonLightBox.selectedIndex != optionChooseOneValue) {
                        removedReason = ddRemovedReasonLightBox.options[ddRemovedReasonLightBox.selectedIndex].text;
                    }
                    if (removedReason == "Починал") {
                        document.getElementById("rowRemovedDeceased").style.display = "";
                        document.getElementById("rowRemovedAgeLimit1").style.display = "none";
                        document.getElementById("rowRemovedAgeLimit2").style.display = "none";
                        document.getElementById("rowRemovedNotSuitable1").style.display = "none";
                        document.getElementById("rowRemovedNotSuitable2").style.display = "none";
                    }
                    else if (removedReason == "Пределна възраст") {
                        document.getElementById("rowRemovedAgeLimit1").style.display = "";
                        document.getElementById("rowRemovedAgeLimit2").style.display = "";
                        document.getElementById("rowRemovedDeceased").style.display = "none";
                        document.getElementById("rowRemovedNotSuitable1").style.display = "none";
                        document.getElementById("rowRemovedNotSuitable2").style.display = "none";
                    }
                    else if (removedReason == "НГВС с изключване") {
                        document.getElementById("rowRemovedNotSuitable1").style.display = "";
                        document.getElementById("rowRemovedNotSuitable2").style.display = "";
                        document.getElementById("rowRemovedDeceased").style.display = "none";
                        document.getElementById("rowRemovedAgeLimit1").style.display = "none";
                        document.getElementById("rowRemovedAgeLimit2").style.display = "none";
                    }
                    else {
                        document.getElementById("rowRemovedDeceased").style.display = "none";
                        document.getElementById("rowRemovedAgeLimit1").style.display = "none";
                        document.getElementById("rowRemovedAgeLimit2").style.display = "none";
                        document.getElementById("rowRemovedNotSuitable1").style.display = "none";
                        document.getElementById("rowRemovedNotSuitable2").style.display = "none";
                    }

                    document.getElementById("divSectionRemoved").style.display = "";
                    break;
                case "MILITARY_EMPLOYED": /*This status is removed from the DB in BMoD, but we left it in the code just in case*/
                    var ddMilEmplAdministrationLightBox = document.getElementById("ddMilEmplAdministrationLightBox");
                    if (ddMilEmplAdministrationLightBox.selectedIndex != "" && ddMilEmplAdministrationLightBox.selectedIndex != optionChooseOneValue)
                        document.getElementById("txtMilEmplAdministration").innerHTML = ddMilEmplAdministrationLightBox.options[ddMilEmplAdministrationLightBox.selectedIndex].text;
                    else
                        document.getElementById("txtMilEmplAdministration").innerHTML = "";
                    document.getElementById("txtMilEmplDate").innerHTML = document.getElementById("txtMilEmplDateLightBox").value;
                    document.getElementById("divSectionMilEmployed").style.display = "";
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
                case "DISCHARGED":
                    /*
                    var ddDestMilDepartmentLightBox = document.getElementById("ddDestMilDepartmentLightBox");
                    if (ddDestMilDepartmentLightBox.selectedIndex != "" && ddDestMilDepartmentLightBox.selectedIndex != optionChooseOneValue)
                        document.getElementById("txtDestMilDepartment").innerHTML = ddDestMilDepartmentLightBox.options[ddDestMilDepartmentLightBox.selectedIndex].text;
                    else
                        document.getElementById("txtDestMilDepartment").innerHTML = "";
                    document.getElementById("txtDischargeDate").innerHTML = document.getElementById("txtDischargeDateLightBox").value;
                    */

                    var ddDestMilDepartmentLightBox = document.getElementById("ddDestMilDepartmentLightBox");
                    document.getElementById("lblCurrSourceMilDepartmentNameValue").innerHTML = ddDestMilDepartmentLightBox.options[ddDestMilDepartmentLightBox.selectedIndex].text;
                    document.getElementById("txtDestMilDepartment").innerHTML = "";
                    document.getElementById("lblCurrEnrolDateValue").innerHTML = document.getElementById("txtDischargeDateLightBox").value;
                    document.getElementById("txtDischargeDate").innerHTML = "";
                    
                    document.getElementById("divSectionDischarged").style.display = "";
                    break;
            }

            document.getElementById("divGroupManagementSection").innerHTML = xmlValue(xml, "GroupManagementSection");
            document.getElementById("divReservistAppointmentSection").innerHTML = xmlValue(xml, "ReservistAppointmentSection");
        }

        if (hideDialog)
            HideAddEditMilRepStatusLightBox();
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}

function HideAllReservistMilitaryReportStatusSectionDivs()
{
    document.getElementById("divSectionVoluntary").style.display = "none";
    document.getElementById("divSectionRemoved").style.display = "none";
    document.getElementById("divSectionMilEmployed").style.display = "none";
    document.getElementById("divSectionTemporaryRemoved").style.display = "none";
    document.getElementById("divSectionPostpone").style.display = "none";
    document.getElementById("divSectionDischarged").style.display = "none";
}

function btnHistoryStatuses_Click()
{
    var url = "AddEditReservist_MilitaryReport.aspx?AjaxMethod=JSLoadMilRepStatusHistory";

    var params = "ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

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

function HideReservistMilRepStatusHistoryLightBox()
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
    var url = "AddEditReservist_MilitaryReport.aspx?AjaxMethod=JSLoadMilRepStatusHistory";

    var params = "";
    params += "ReservistId=" + document.getElementById(hdnReservistIdClientID).value;    
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
    var url = "AddEditReservist_MilitaryReport.aspx?AjaxMethod=JSLoadReservistAppointmentHistory";

    var params = "ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

    function response_handler(xml)
    {
        document.getElementById("divReservistAppointmentHistoryLightBox").innerHTML = xmlValue(xml, "response");

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("divReservistAppointmentHistoryLightBox").style.display = "";
        CenterLightBox("divReservistAppointmentHistoryLightBox");
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}

function HideReservistAppointmentHistoryLightBox()
{
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("divReservistAppointmentHistoryLightBox").style.display = "none";
    document.getElementById("divReservistAppointmentHistoryLightBox").innerHTML = "";
}

function BtnReservistAppointmentHistoryPagingClick(objectName)
{
    hdnPageIdx = document.getElementById('hdnPageIndex');
    hdnMaxPage = document.getElementById('hdnPageMaxPage');
    switch (objectName)
    {
        case "btnFirst":
            hdnPageIdx.value = 1;

            RefreshReservistAppointmentHistoryLightBox();
            break;

        case "btnPrev":
            pageIdx = parseInt(hdnPageIdx.value);

            if (pageIdx > 1)
            {
                pageIdx--;
                hdnPageIdx.value = pageIdx;
                RefreshReservistAppointmentHistoryLightBox();
            }

            break;

        case "btnNext":
            pageIdx = parseInt(hdnPageIdx.value);
            maxPage = parseInt(hdnMaxPage.value);

            if (pageIdx < maxPage)
            {
                pageIdx++;
                hdnPageIdx.value = pageIdx;
                RefreshReservistAppointmentHistoryLightBox();
            }
            break;


        case "btnLast":
            hdnPageIdx.value = hdnMaxPage.value;

            RefreshReservistAppointmentHistoryLightBox();
            break;

        case "btnPageGo":
            maxPage = parseInt(hdnMaxPage.value);
            goToPage = parseInt(document.getElementById('txtTableGotoPage').value);

            if (isInt(goToPage) && goToPage > 0 && goToPage <= maxPage)
            {
                hdnPageIdx.value = goToPage;
                RefreshReservistAppointmentHistoryLightBox();
            }
            break;

        default:
            break;
    }

}

function RefreshReservistAppointmentHistoryLightBox()
{
    var url = "AddEditReservist_MilitaryReport.aspx?AjaxMethod=JSLoadReservistAppointmentHistory";

    var params = "";
    params += "ReservistId=" + document.getElementById(hdnReservistIdClientID).value;
    params += "&PageIndex=" + document.getElementById("hdnPageIndex").value;
    params += "&MaxPage=" + document.getElementById("hdnPageMaxPage").value;
    params += "&OrderBy=" + document.getElementById("hdnOrderBy").value;

    function response_handler(xml)
    {
        document.getElementById('divReservistAppointmentHistoryLightBox').innerHTML = xmlValue(xml, "response");
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}






//Open the light-box for adding a new record in the Person Military Report Specialities table
function NewMilRepSpec()
{
    ShowAddEditMilRepSpecLightBox(0);
}

//Open the light-box for editing a record in the Person Military Report Specialities table
function EditMilRepSpec(personMilRepSpecID)
{
    ShowAddEditMilRepSpecLightBox(personMilRepSpecID);
}

function ShowAddEditMilRepSpecLightBox(personMilRepSpecID)
{
    ClearAllMessages();

    document.getElementById("hdnPersonMilRepSpecID").value = personMilRepSpecID;

    //New record
    if (personMilRepSpecID == 0)
    {
        document.getElementById("lblAddEditMilRepSpecTitle").innerHTML = "Въвеждане на ВОС";

        document.getElementById("ddMilRepSpecTypeLightBox").value = optionChooseOneValue;

        if (document.getElementById("hdnPersonMilitaryReportSpecialitiesCount").value == "0")
        {
            document.getElementById("chkMilRepSpecIsPrimaryLightBox").checked = true;
            document.getElementById("hdnMilRepSpecIsPrimaryOldLightBox").value = "1";
        }
        else
        {
            document.getElementById("chkMilRepSpecIsPrimaryLightBox").checked = false;
            document.getElementById("hdnMilRepSpecIsPrimaryOldLightBox").value = "0";
        }

        // clean message label in the light box and hide it
        document.getElementById("spanAddEditMilRepSpecLightBoxMsg").style.display = "none";
        document.getElementById("spanAddEditMilRepSpecLightBoxMsg").innerHTML = "";

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("divMilRepSpecLightBox").style.display = "";
        CenterLightBox("divMilRepSpecLightBox");
    }
    else //Edit record
    {
        document.getElementById("lblAddEditMilRepSpecTitle").innerHTML = "Редактиране на ВОС";

        var url = "AddEditReservist_MilitaryReport.aspx?AjaxMethod=JSLoadMilRepSpec";

        var params = "PersonMilRepSpecID=" + personMilRepSpecID;

        function response_handler(xml)
        {
            document.getElementById("ddMilRepSpecTypeLightBox").value = xmlValue(xml, "milrepspectypeid");
            document.getElementById("chkMilRepSpecIsPrimaryLightBox").checked = xmlValue(xml, "isPrimary") == "1";
            document.getElementById("hdnMilRepSpecIsPrimaryOldLightBox").value = xmlValue(xml, "isPrimary");
        
            ClearSelectList(document.getElementById("ddMilRepSpecLightBox"), true);

            var statuses = xml.getElementsByTagName("mrs");

            for (var i = 0; i < statuses.length; i++)
            {
                var id = xmlValue(statuses[i], "id");
                var name = xmlValue(statuses[i], "name");

                AddToSelectList(document.getElementById("ddMilRepSpecLightBox"), id, name);
            };

            document.getElementById("ddMilRepSpecLightBox").value = xmlValue(xml, "milrepspecid");

            // clean message label in the light box and hide it
            document.getElementById("spanAddEditMilRepSpecLightBoxMsg").style.display = "none";
            document.getElementById("spanAddEditMilRepSpecLightBoxMsg").innerHTML = "";

            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("divMilRepSpecLightBox").style.display = "";
            CenterLightBox("divMilRepSpecLightBox");
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Close the light-box
function HideAddEditMilRepSpecLightBox()
{
    ClearSelectList(document.getElementById("ddMilRepSpecLightBox"), true);
    document.getElementById("ddMilRepSpecTypeLightBox").value = optionChooseOneValue;
    document.getElementById("chkMilRepSpecIsPrimaryLightBox").checked = false;
    document.getElementById("hdnMilRepSpecIsPrimaryOldLightBox").value = "0";
    
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("divMilRepSpecLightBox").style.display = "none";
}

// Handler of onchange event of VoluntaryMilRepSpecType dropdown in the Light Box
function MilRepSpecTypeLightBoxChanged()
{
    var url = "AddEditReservist_MilitaryReport.aspx?AjaxMethod=JSLoadMilRepSpecs";

    var params = "MilRepSpecTypeID=" + document.getElementById("ddMilRepSpecTypeLightBox").value;

    function response_handler(xml)
    {
        ClearSelectList(document.getElementById("ddMilRepSpecLightBox"), true);

        var milRepSpecs = xml.getElementsByTagName("m");

        for (var i = 0; i < milRepSpecs.length; i++)
        {
            var id = xmlValue(milRepSpecs[i], "id");
            var name = xmlValue(milRepSpecs[i], "name");

            AddToSelectList(document.getElementById("ddMilRepSpecLightBox"), id, name, true);
        };
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}

//Save Add/Edit Person Military Report Speciality
function SaveAddEditMilRepSpecLightBox()
{
    if (ValidateAddEditMilRepSpecLightBox())
    {
        var url = "AddEditReservist_MilitaryReport.aspx?AjaxMethod=JSSaveMilRepSpec";

        var params = "PersonMilRepSpecID=" + document.getElementById("hdnPersonMilRepSpecID").value +
                     "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value +
                     "&MilRepSpecId=" + document.getElementById("ddMilRepSpecLightBox").value +
                     "&IsPrimary=" + (document.getElementById("chkMilRepSpecIsPrimaryLightBox").checked ? "1" : "0");

        function response_handler(xml)
        {
            var status = xmlValue(xml, "status");

            if (status == "OK")
            {
                var refreshedMilRepSpecTable = xmlValue(xml, "refreshedMilRepSpecTable");

                document.getElementById("divMilRepSpecTable").innerHTML = refreshedMilRepSpecTable;

                var basePersonMilitaryReportSpecialityCode = xmlValue(xml, "basePersonMilitaryReportSpecialityCode");
                if (basePersonMilitaryReportSpecialityCode != "") {
                    document.getElementById(lblCurrentVosValueClientID).innerHTML = basePersonMilitaryReportSpecialityCode;
                }

                document.getElementById("spanPersonAdmClAccessAndMilRepSpecSectionMsg").className = "SuccessText";
                document.getElementById("spanPersonAdmClAccessAndMilRepSpecSectionMsg").innerHTML = document.getElementById("hdnPersonMilRepSpecID").value == "0" ? "ВОС е добавена успешно" : "ВОС е редактирана успешно";

                HideAddEditMilRepSpecLightBox();
            }
            else
            {
                document.getElementById("spanAddEditMilRepSpecLightBoxMsg").className = "ErrorText";
                document.getElementById("spanAddEditMilRepSpecLightBoxMsg").innerHTML = status;
                document.getElementById("spanAddEditMilRepSpecLightBoxMsg").style.display = "";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Client validation of AddEditMilRepSpecLightBox light-box
function ValidateAddEditMilRepSpecLightBox()
{
    var res = true;

    var lblMessage = document.getElementById("spanAddEditMilRepSpecLightBoxMsg");
    lblMessage.innerHTML = "";

    var notValidFields = new Array();

    var ddMilRepSpecLightBox = document.getElementById("ddMilRepSpecLightBox");

    if (ddMilRepSpecLightBox.value == "" || ddMilRepSpecLightBox.value == optionChooseOneValue)
    {
        res = false;

        if (ddMilRepSpecLightBox.disabled == true || ddMilRepSpecLightBox.style.display == "none")
            notValidFields.push("ВОС");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("ВОС") + "<br />";
    }

    if (!document.getElementById("chkMilRepSpecIsPrimaryLightBox").checked &&
        document.getElementById("hdnMilRepSpecIsPrimaryOldLightBox").value == "1")
    {
        res = false;
        lblMessage.innerHTML += "Задължително е да има една основна ВОС" + "<br />";
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

// Delete a particular Person Military Report Speciality record
function DeleteMilRepSpec(personMilRepSpecID)
{
    ClearAllMessages();

    YesNoDialog("Желаете ли да изтриете ВОС?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "AddEditReservist_MilitaryReport.aspx?AjaxMethod=JSDeleteMilRepSpec";

        var params = "PersonMilRepSpecID=" + personMilRepSpecID +
                 "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

        function response_handler(xml)
        {
            var status = xmlValue(xml, "status")

            if (status == "OK")
            {
                var refreshedMilRepSpecTable = xmlValue(xml, "refreshedMilRepSpecTable");

                document.getElementById("divMilRepSpecTable").innerHTML = refreshedMilRepSpecTable;

                var basePersonMilitaryReportSpecialityCode = xmlValue(xml, "basePersonMilitaryReportSpecialityCode");
                if (basePersonMilitaryReportSpecialityCode != "") {
                    document.getElementById(lblCurrentVosValueClientID).innerHTML = basePersonMilitaryReportSpecialityCode;
                }

                document.getElementById("spanPersonAdmClAccessAndMilRepSpecSectionMsg").className = "SuccessText";
                document.getElementById("spanPersonAdmClAccessAndMilRepSpecSectionMsg").innerHTML = "ВОС е изтрита успешно";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}



//Open the light-box for adding a new record in the Person Position Title table
function NewPositionTitle() {
    ShowAddEditPositionTitleLightBox(0);
}

//Open the light-box for editing a record in the Person Position Titles table
function EditPositionTitle(personPositionTitleID) {
    ShowAddEditPositionTitleLightBox(personPositionTitleID);
}

function ShowAddEditPositionTitleLightBox(personPositionTitleID) {
    ClearAllMessages();

    document.getElementById("hdnPersonPositionTitleID").value = personPositionTitleID;
    document.getElementById("hdnExtraAddedPositionTitleID").value = "";

    //New record
    if (personPositionTitleID == 0) {
        document.getElementById("lblAddEditPositionTitleTitle").innerHTML = "Въвеждане на подходяща длъжност";

        document.getElementById("ddPositionTitleLightBox").value = optionChooseOneValue;

        if (document.getElementById("hdnPersonPositionTitlesCount").value == "0") {
            document.getElementById("chkPositionTitleIsPrimaryLightBox").checked = true;
            document.getElementById("hdnPositionTitleIsPrimaryOldLightBox").value = "1";
        }
        else {
            document.getElementById("chkPositionTitleIsPrimaryLightBox").checked = false;
            document.getElementById("hdnPositionTitleIsPrimaryOldLightBox").value = "0";
        }

        // clean message label in the light box and hide it
        document.getElementById("spanAddEditPositionTitleLightBoxMsg").style.display = "none";
        document.getElementById("spanAddEditPositionTitleLightBoxMsg").innerHTML = "";

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("divPositionTitleLightBox").style.display = "";
        CenterLightBox("divPositionTitleLightBox");
    }
    else //Edit record
    {
        document.getElementById("lblAddEditPositionTitleTitle").innerHTML = "Редактиране на подходяща длъжност";

        var url = "AddEditReservist_MilitaryReport.aspx?AjaxMethod=JSLoadPositionTitle";

        var params = "PersonPositionTitleID=" + personPositionTitleID;

        function response_handler(xml) {
            document.getElementById("ddPositionTitleLightBox").value = xmlValue(xml, "positiontitleid");
            //If the drop-down value is empty then the selected option is not listed in the drop-down.
            //This can happen if the Position Title is inactive. In that case we want to add it to the drop-down
            if (document.getElementById("ddPositionTitleLightBox").value == "") {
                AddToSelectList(document.getElementById("ddPositionTitleLightBox"), xmlValue(xml, "positiontitleid"), xmlValue(xml, "positiontitlename"), true);
                document.getElementById("ddPositionTitleLightBox").value = xmlValue(xml, "positiontitleid");
                document.getElementById("hdnExtraAddedPositionTitleID").value = xmlValue(xml, "positiontitleid");
            }
            document.getElementById("chkPositionTitleIsPrimaryLightBox").checked = xmlValue(xml, "isPrimary") == "1";
            document.getElementById("hdnPositionTitleIsPrimaryOldLightBox").value = xmlValue(xml, "isPrimary");

            // clean message label in the light box and hide it
            document.getElementById("spanAddEditPositionTitleLightBoxMsg").style.display = "none";
            document.getElementById("spanAddEditPositionTitleLightBoxMsg").innerHTML = "";

            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("divPositionTitleLightBox").style.display = "";
            CenterLightBox("divPositionTitleLightBox");
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Close the light-box
function HideAddEditPositionTitleLightBox() {
    if (document.getElementById("hdnExtraAddedPositionTitleID").value != "") {
        RemoveOptionFromDropDown("ddPositionTitleLightBox", document.getElementById("hdnExtraAddedPositionTitleID").value);
        document.getElementById("hdnExtraAddedPositionTitleID").value = "";
    }

    document.getElementById("ddPositionTitleLightBox").value = optionChooseOneValue;
    document.getElementById("chkPositionTitleIsPrimaryLightBox").checked = false;
    document.getElementById("hdnPositionTitleIsPrimaryOldLightBox").value = "0";

    document.getElementById("HidePage").style.display = "none";
    document.getElementById("divPositionTitleLightBox").style.display = "none";
}

//Save Add/Edit Person Position Title Speciality
function SaveAddEditPositionTitleLightBox() {
    if (ValidateAddEditPositionTitleLightBox()) {
        var url = "AddEditReservist_MilitaryReport.aspx?AjaxMethod=JSSavePositionTitle";

        var params = "PersonPositionTitleID=" + document.getElementById("hdnPersonPositionTitleID").value +
                     "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value +
                     "&PositionTitleId=" + document.getElementById("ddPositionTitleLightBox").value +
                     "&IsPrimary=" + (document.getElementById("chkPositionTitleIsPrimaryLightBox").checked ? "1" : "0");

        function response_handler(xml) {
            var status = xmlValue(xml, "status");

            if (status == "OK") {
                var refreshedPositionTitleTable = xmlValue(xml, "refreshedPositionTitleTable");

                document.getElementById("divPositionTitleTable").innerHTML = refreshedPositionTitleTable;

                var basePersonPositionTitle = xmlValue(xml, "basePersonPositionTitle");
                if (basePersonPositionTitle != "") {
                    document.getElementById(lblCurrentPositionTitleValueClientID).innerHTML = basePersonPositionTitle;
                }

                document.getElementById("spanPersonAdmClAccessAndMilRepSpecSectionMsg").className = "SuccessText";
                document.getElementById("spanPersonAdmClAccessAndMilRepSpecSectionMsg").innerHTML = document.getElementById("hdnPersonPositionTitleID").value == "0" ? "Длъжността е добавена успешно" : "Длъжността е редактирана успешно";

                HideAddEditPositionTitleLightBox();
            }
            else {
                document.getElementById("spanAddEditPositionTitleLightBoxMsg").className = "ErrorText";
                document.getElementById("spanAddEditPositionTitleLightBoxMsg").innerHTML = status;
                document.getElementById("spanAddEditPositionTitleLightBoxMsg").style.display = "";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Client validation of AddEditMilRepSpecLightBox light-box
function ValidateAddEditPositionTitleLightBox() {
    var res = true;

    var lblMessage = document.getElementById("spanAddEditPositionTitleLightBoxMsg");
    lblMessage.innerHTML = "";

    var notValidFields = new Array();

    var ddPositionTitleLightBox = document.getElementById("ddPositionTitleLightBox");

    if (ddPositionTitleLightBox.value == "" || ddPositionTitleLightBox.value == optionChooseOneValue) {
        res = false;

        if (ddPositionTitleLightBox.disabled == true || ddPositionTitleLightBox.style.display == "none")
            notValidFields.push("Длъжност");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Длъжност") + "<br />";
    }

    if (!document.getElementById("chkPositionTitleIsPrimaryLightBox").checked &&
        document.getElementById("hdnPositionTitleIsPrimaryOldLightBox").value == "1") {
        res = false;
        lblMessage.innerHTML += "Задължително е да има една основна длъжност" + "<br />";
    }

    var notValidFieldsCount = notValidFields.length;

    if (notValidFieldsCount > 0) {
        var noRightsMessage = GetErrorMessageNoRights(notValidFields);
        lblMessage.innerHTML += "<br />" + noRightsMessage;
    }

    if (res) {
        lblMessage.className = "SuccessText";
    }
    else {
        lblMessage.className = "ErrorText";
        lblMessage.style.display = "";
    }

    return res;
}

// Delete a particular Person Position Title record
function DeletePositionTitle(personPositionTitleID) {
    ClearAllMessages();

    YesNoDialog("Желаете ли да изтриете длъжността?", ConfirmYes, null);

    function ConfirmYes() {
        var url = "AddEditReservist_MilitaryReport.aspx?AjaxMethod=JSDeletePositionTitle";

        var params = "PersonPositionTitleID=" + personPositionTitleID +
                 "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

        function response_handler(xml) {
            var status = xmlValue(xml, "status")

            if (status == "OK") {
                var refreshedPositionTitleTable = xmlValue(xml, "refreshedPositionTitleTable");

                document.getElementById("divPositionTitleTable").innerHTML = refreshedPositionTitleTable;

                var basePersonPositionTitle = xmlValue(xml, "basePersonPositionTitle");
                if (basePersonPositionTitle != "") {
                    document.getElementById(lblCurrentPositionTitleValueClientID).innerHTML = basePersonPositionTitle;
                }

                document.getElementById("spanPersonAdmClAccessAndMilRepSpecSectionMsg").className = "SuccessText";
                document.getElementById("spanPersonAdmClAccessAndMilRepSpecSectionMsg").innerHTML = "Длъжността е изтрита успешно";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}




function SaveMilitaryReportTab(saveMilitaryReportTabFinishCallback)
{
    if (IsTabAlreadyVisited("btnTabMilitaryReport"))
    {
        var url = "AddEditReservist_MilitaryReport.aspx?AjaxMethod=JSSaveMilitaryReportTab";

        var params = "ReservistId=" + document.getElementById(hdnReservistIdClientID).value +
                    "&AdministrationId=" + document.getElementById("ddAdmClAccessAndMilRepSpecSectionAdministration").value +
                    "&ClInformationAccLevelBgId=" + document.getElementById("ddAdmClAccessAndMilRepSpecSectionClInformationAccLevelBg").value +
                    "&ClInformationAccLevelBgExpDate=" + document.getElementById("txtAdmClAccessAndMilRepSpecSectionClInformationAccLevelBgExpDate").value +
                    "&GroupManagementSection=" + custEncodeURI(document.getElementById("txtGMSGroupManagementSection").value) +
                    "&Section=" + custEncodeURI(document.getElementById("txtGMSSection").value) +
                    "&Deliverer=" + custEncodeURI(document.getElementById("txtGMSDeliverer").value) +
                    "&PunktId=" + document.getElementById("ddGMSPunkt").value;
                    
        if (document.getElementById("cbNeedCourse"))
            params += "&NeedCourse=" + (document.getElementById("cbNeedCourse").checked ? "1" : "0");

        if (document.getElementById("cbAppointmentIsDelivered"))
            params += "&AppointmentIsDelivered=" + (document.getElementById("cbAppointmentIsDelivered").checked ? "1" : "0");
        if (document.getElementById("suitableForMobAppointmentCheckBox")) {
            params += "&IsSuitableForMobAppointment=" + (document.getElementById("suitableForMobAppointmentCheckBox").checked ? "1" : "0");
        }

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

        var txtAdmClAccessAndMilRepSpecSectionClInformationAccLevelBgExpDate = document.getElementById("txtAdmClAccessAndMilRepSpecSectionClInformationAccLevelBgExpDate");
        if (TrimString(txtAdmClAccessAndMilRepSpecSectionClInformationAccLevelBgExpDate.value) != "" && !IsValidDate(txtAdmClAccessAndMilRepSpecSectionClInformationAccLevelBgExpDate.value)) {

            ValidationMessage += tabNameHeader + GetErrorMessageDate("Валиден до") + "<br />";
        }
        
        var notValidFieldsCount = notValidFields.length;
        var fieldsStr = '"' + notValidFields.join(", ") + '"';

        if (notValidFieldsCount > 0) {
            var noRightsMessage = GetErrorMessageNoRights(notValidFields);
            ValidationMessage += "<br />" + tabNameHeader + noRightsMessage;
        }
    }
    return ValidationMessage;
}

function PrintMK() {
    var reservistId = document.getElementById(hdnReservistIdClientID).value;
    location.href = "AddEditReservist_MilitaryReport.aspx?AjaxMethod=JSPrintMK&ReservistId=" + reservistId;
}

function PrintPZ() {
    var reservistId = document.getElementById(hdnReservistIdClientID).value;
    location.href = "AddEditReservist_MilitaryReport.aspx?AjaxMethod=JSPrintPZ&ReservistId=" + reservistId;
}

function PrintAK() {
    var reservistId = document.getElementById(hdnReservistIdClientID).value;
    location.href = "AddEditReservist_MilitaryReport.aspx?AjaxMethod=JSPrintАK&ReservistId=" + reservistId;
}

function PrintASK() {
    var reservistId = document.getElementById(hdnReservistIdClientID).value;
    location.href = "AddEditReservist_MilitaryReport.aspx?AjaxMethod=JSPrintАSK&ReservistId=" + reservistId;
}

function PrintUO() {
    var reservistId = document.getElementById(hdnReservistIdClientID).value;
    location.href = "AddEditReservist_MilitaryReport.aspx?AjaxMethod=JSPrintUO&ReservistId=" + reservistId;
}

function MilitaryReport_Refresh_ReservistMilitaryReportStatusSection(completeCallback) {
    //If the tab was not yet visited then do not refresh the section
    if (IsTabAlreadyVisited("btnTabMilitaryReport")) {
        var url = "AddEditReservist_MilitaryReport.aspx?AjaxMethod=JSLoadReservistMilitaryReportStatusSection";
        var params = "";
        params += "ReservistId=" + document.getElementById(hdnReservistIdClientID).value;
        
        function JSLoadReservistMilitaryReportStatusSection_Callback(xml) {
            document.getElementById("divReservistMilitaryReportStatusSection").innerHTML = xmlValue(xml, "response");
            if (completeCallback != null)
                completeCallback();
        }

        var myAJAX = new AJAX(url, true, params, JSLoadReservistMilitaryReportStatusSection_Callback);
        myAJAX.Call();
    }
    else {
        if (completeCallback != null)
            completeCallback();
    }
}

function btnTransferToVitosha_Click() {
    ShowTransferToVitoshaLightBox();
}

function ShowTransferToVitoshaLightBox() {

    //shows the light box and "disable" rest of the page
    document.getElementById("HidePage").style.display = "";
    document.getElementById("lboxTransferToVitosha").style.display = "";
    CenterLightBox("lboxTransferToVitosha");
}

function SaveTransferToVitoshaLightBox() {
    if (ValidateTransferToVitoshaLightBox()) {
        var reservistID = document.getElementById(hdnReservistIdClientID).value;
        var militaryUnitID = MilitaryUnitSelectorUtil.GetSelectedValue("itmsTransferToVitoshaMilUnitSelector");

        TransferToVitosha(reservistID, militaryUnitID);
    }
}

function TransferToVitosha(pReservistID, pMilitaryUnitID) {
    var url = "AddEditReservist_MilitaryReport.aspx?AjaxMethod=JSTransferToVitosha";
    var params = "";
    params += "ReservistID=" + pReservistID;
    params += "&MilitaryUnitID=" + pMilitaryUnitID;

    function TransferToVitosha_Callback(xml) {
        var lblMessage = document.getElementById("spanTransferToVitoshaLightBoxMsg");
        var status = xmlValue(xml, "status");
        var msg = xmlValue(xml, "msg");
        if (status == "OK") {
            lblMessage.innerHTML = msg;
            lblMessage.style.display = "";

            document.getElementById("btnOKTransferToVitoshaLightBox").style.display = "";
            document.getElementById("btnSaveTransferToVitoshaLightBox").style.display = "none";
            document.getElementById("btnCloseTransferToVitoshaLightBox").style.display = "none";
            document.getElementById("trTransferToVitoshaMilitaryUnit").style.display = "none";            
        }        
    }

    var myAJAX = new AJAX(url, true, params, TransferToVitosha_Callback);
    myAJAX.Call();
}

function ValidateTransferToVitoshaLightBox() {
    var res = true;
    var lblMessage = document.getElementById("spanTransferToVitoshaLightBoxMsg");
    
    if (MilitaryUnitSelectorUtil.GetSelectedValue("itmsTransferToVitoshaMilUnitSelector") == "-1") {
        res = false;
        lblMessage.innerHTML += GetErrorMessageMandatory("ВПН/Структура") + "<br />";
    }

    if (res) {
        lblMessage.className = "SuccessText";
    }
    else {
        lblMessage.className = "ErrorText";
        lblMessage.style.display = "";
    }

    return res;
}

function HideTransferToVitoshaLightBox() {
    MilitaryUnitSelectorUtil.SetSelectedValue("itmsTransferToVitoshaMilUnitSelector", "-1");
    MilitaryUnitSelectorUtil.SetSelectedText("itmsTransferToVitoshaMilUnitSelector", "");

    var lblMessage = document.getElementById("spanTransferToVitoshaLightBoxMsg");
    lblMessage.innerHTML = "";
    lblMessage.style.display = "none";

    document.getElementById("HidePage").style.display = "none";
    document.getElementById("lboxTransferToVitosha").style.display = "none";
}

function RefreshTransferToVitoshaLightBox() {
    window.location.reload(true);
}

function RemovedReasonsLightBoxChanged() 
{
    var ddRemovedReasonLightBox = document.getElementById("ddRemovedReasonLightBox");
    var removedReason = "";
    if (ddRemovedReasonLightBox.selectedIndex != "" && ddRemovedReasonLightBox.selectedIndex != optionChooseOneValue) 
    {
        removedReason = ddRemovedReasonLightBox.options[ddRemovedReasonLightBox.selectedIndex].text;
    }

    if (removedReason == "Починал") 
    {
        document.getElementById("rowRemovedDeceasedLightBox").style.display = "";
        document.getElementById("rowRemovedAgeLimit1LightBox").style.display = "none";
        document.getElementById("rowRemovedAgeLimit2LightBox").style.display = "none";
        document.getElementById("rowRemovedNotSuitable1LightBox").style.display = "none";
        document.getElementById("rowRemovedNotSuitable2LightBox").style.display = "none";

        document.getElementById("txtRemovedAgeLimitOrderLightBox").value = "";
        document.getElementById("txtRemovedAgeLimitDateLightBox").value = "";
        document.getElementById("txtRemovedAgeLimitSignedByLightBox").value = "";
        document.getElementById("txtRemovedNotSuitableCertLightBox").value = "";
        document.getElementById("txtRemovedNotSuitableDateLightBox").value = "";
        document.getElementById("txtRemovedNotSuitableSignedByLightBox").value = "";
    }
    else if (removedReason == "Пределна възраст") 
    {
        document.getElementById("rowRemovedAgeLimit1LightBox").style.display = "";
        document.getElementById("rowRemovedAgeLimit2LightBox").style.display = "";
        document.getElementById("rowRemovedDeceasedLightBox").style.display = "none";
        document.getElementById("rowRemovedNotSuitable1LightBox").style.display = "none";
        document.getElementById("rowRemovedNotSuitable2LightBox").style.display = "none";

        document.getElementById("txtRemovedDeceasedDeathCertLightBox").value = "";
        document.getElementById("txtRemovedDeceasedDateLightBox").value = "";        
        document.getElementById("txtRemovedNotSuitableCertLightBox").value = "";
        document.getElementById("txtRemovedNotSuitableDateLightBox").value = "";
        document.getElementById("txtRemovedNotSuitableSignedByLightBox").value = "";       
    }
    else if (removedReason == "НГВС с изключване") 
    {
        document.getElementById("rowRemovedNotSuitable1LightBox").style.display = "";
        document.getElementById("rowRemovedNotSuitable2LightBox").style.display = "";
        document.getElementById("rowRemovedDeceasedLightBox").style.display = "none";
        document.getElementById("rowRemovedAgeLimit1LightBox").style.display = "none";
        document.getElementById("rowRemovedAgeLimit2LightBox").style.display = "none";

        document.getElementById("txtRemovedDeceasedDeathCertLightBox").value = "";
        document.getElementById("txtRemovedDeceasedDateLightBox").value = "";
        document.getElementById("txtRemovedAgeLimitOrderLightBox").value = "";
        document.getElementById("txtRemovedAgeLimitDateLightBox").value = "";
        document.getElementById("txtRemovedAgeLimitSignedByLightBox").value = "";
    }
    else 
    {
        document.getElementById("rowRemovedDeceasedLightBox").style.display = "none";
        document.getElementById("rowRemovedAgeLimit1LightBox").style.display = "none";
        document.getElementById("rowRemovedAgeLimit2LightBox").style.display = "none";
        document.getElementById("rowRemovedNotSuitable1LightBox").style.display = "none";
        document.getElementById("rowRemovedNotSuitable2LightBox").style.display = "none";

        document.getElementById("txtRemovedDeceasedDeathCertLightBox").value = "";
        document.getElementById("txtRemovedDeceasedDateLightBox").value = ""
        document.getElementById("txtRemovedAgeLimitOrderLightBox").value = "";
        document.getElementById("txtRemovedAgeLimitDateLightBox").value = "";
        document.getElementById("txtRemovedAgeLimitSignedByLightBox").value = "";
        document.getElementById("txtRemovedNotSuitableCertLightBox").value = "";
        document.getElementById("txtRemovedNotSuitableDateLightBox").value = "";
        document.getElementById("txtRemovedNotSuitableSignedByLightBox").value = "";       
    }
}

function HideAllAddEditMilRepStatusLightBoxRows() 
{
    document.getElementById("rowRemovedDeceasedLightBox").style.display = "none";
    document.getElementById("rowRemovedAgeLimit1LightBox").style.display = "none";
    document.getElementById("rowRemovedAgeLimit2LightBox").style.display = "none";
    document.getElementById("rowRemovedNotSuitable1LightBox").style.display = "none";
    document.getElementById("rowRemovedNotSuitable2LightBox").style.display = "none";

    document.getElementById("txtRemovedDeceasedDeathCertLightBox").value = "";
    document.getElementById("txtRemovedDeceasedDateLightBox").value = ""
    document.getElementById("txtRemovedAgeLimitOrderLightBox").value = "";
    document.getElementById("txtRemovedAgeLimitDateLightBox").value = "";
    document.getElementById("txtRemovedAgeLimitSignedByLightBox").value = "";
    document.getElementById("txtRemovedNotSuitableCertLightBox").value = "";
    document.getElementById("txtRemovedNotSuitableDateLightBox").value = "";
    document.getElementById("txtRemovedNotSuitableSignedByLightBox").value = "";
}

function RenderAnnexRowLightBox(annexID, annexNumber, annexDate, annexDurationMonths, annexExpireDate, index) {
    var target = document.getElementById("rowVoluntaryLightBox2");
    var rowAnnex = document.createElement("tr");
    target.parentNode.insertBefore(rowAnnex, target);
    
    rowAnnex.setAttribute("id", "rowAnnexLightBox" + index);
    rowAnnex.className = "rowAnnexLightBox";

    var lblAnnexNumber = document.createElement("td");
    lblAnnexNumber.style.textAlign = "right";
    lblAnnexNumber.innerHTML =
        '<input type="hidden" id="VoluntaryReserveAnnexID' + index + '" value="' + annexID + '"></input>' +
        '<input type="hidden" id="hdnIsDeletedLightBox' + index + '" value="no"></input>' +
        '<span id="lblAnnexNumberLightBox' + index + '" class="InputLabel">Доп. сп. №:</span>';
    rowAnnex.appendChild(lblAnnexNumber);

    var txtAnnexNumber = document.createElement("td");
    txtAnnexNumber.style.textAlign = "left";
    txtAnnexNumber.innerHTML =
        '<input id="txtAnnexNumberLightBox' + index + '" class="InputField" style="width: 120px;" UnsavedCheckSkipMe="true" value="' + annexNumber + '"></input>';
    rowAnnex.appendChild(txtAnnexNumber);

    var lblAnnexDate = document.createElement("td");
    lblAnnexDate.style.textAlign = "right";
    lblAnnexDate.innerHTML =
        '<span id="lblAnnexDateLightBox' + index + '" class="InputLabel">от дата:</span>';
    rowAnnex.appendChild(lblAnnexDate);

    var txtAnnexDate = document.createElement("td");
    txtAnnexDate.style.textAlign = "left";
    txtAnnexDate.innerHTML =
        '<span id="spanAnnexDateLightBox' + index + '">' +
            '<input id="txtAnnexDateLightBox' + index + '" class="' + datePickerCSS + '" style="width: 80px;" UnsavedCheckSkipMe="true" inLightBox="true" value="' + annexDate + '"></input>' +
        '</span>';
    rowAnnex.appendChild(txtAnnexDate);

    var lblAnnexDurationMonths = document.createElement("td");
    lblAnnexDurationMonths.style.textAlign = "right";
    lblAnnexDurationMonths.innerHTML =
        '<span id="lblAnnexDurationMonthsLightBox' + index + '" class="InputLabel">Срок:</span>';
    rowAnnex.appendChild(lblAnnexDurationMonths);

    var txtAnnexDurationMonths = document.createElement("td");
    txtAnnexDurationMonths.style.textAlign = "left";
    txtAnnexDurationMonths.innerHTML =
        '<input id="txtAnnexDurationMonthsLightBox' + index + '" class="InputField" UnsavedCheckSkipMe="true" style="width:100px;" value="' + annexDurationMonths + '"></input>';
    rowAnnex.appendChild(txtAnnexDurationMonths);

    var lblAnnexExpireDate = document.createElement("td");
    lblAnnexExpireDate.style.textAlign = "right";
    lblAnnexExpireDate.innerHTML =
        '<span id="lblAnnexExpireDateLightBox' + index + '" class="InputLabel">изтича на:</span>';
    rowAnnex.appendChild(lblAnnexExpireDate);

    var txtAnnexExpireDate = document.createElement("td");
    txtAnnexExpireDate.style.textAlign = "left";
    txtAnnexExpireDate.innerHTML =
        '<span id="spanAnnexExpireDateLightBox' + index + '">' +
            '<input id="txtAnnexExpireDateLightBox' + index + '" class="' + datePickerCSS + '" style="width: 80px;" UnsavedCheckSkipMe="true" inLightBox="true" value="' + annexExpireDate + '"></input>' +
        '</span>';
    rowAnnex.appendChild(txtAnnexExpireDate);

    var btnRemoveAnnex = document.createElement("td");
    btnRemoveAnnex.innerHTML =
        '<img id="btnRemoveAnnex' + index + '" src="../Images/delete.png" alt="Премахване на допълнително споразумение" title="Премахване на допълнително споразумение" class="btnNewTableRecordIcon" onclick="btnRemoveAnnex_Click(this);" />';
    rowAnnex.appendChild(btnRemoveAnnex);

    var btnAddAnnex = document.createElement("td");
    btnAddAnnex.className = "btnAddAnnex";
    btnAddAnnex.style.display = "none";
    btnAddAnnex.innerHTML =
        '<img id="btnAddAnnex' + index + '" src="../Images/addrow.gif" alt="Добавяне на допълнително споразумение" title="Добавяне на допълнително споразумение" class="btnNewTableRecordIcon" onclick="btnAddAnnex_Click();" />';
    rowAnnex.appendChild(btnAddAnnex);
}

function RenderAnnexRow(annexNumber, annexDate, annexDurationMonths, annexExpireDate, index) {
    var target = document.getElementById("rowVoluntary2");
    var rowAnnex = document.createElement("tr");
    target.parentNode.insertBefore(rowAnnex, target);

    rowAnnex.setAttribute("id", "rowAnnex" + index);
    rowAnnex.className = "rowAnnex";

    var lblAnnexNumber = document.createElement("td");
    lblAnnexNumber.style.textAlign = "right";
    lblAnnexNumber.innerHTML =
        '<input type="hidden" id="hdnIsDeleted' + index + '" value="no"></input>' +
        '<span id="lblAnnexNumber' + index + '" class="InputLabel">Доп. сп. №:</span>';
    rowAnnex.appendChild(lblAnnexNumber);

    var txtAnnexNumber = document.createElement("td");
    txtAnnexNumber.style.textAlign = "left";
    txtAnnexNumber.innerHTML =
        '<span id="txtAnnexNumber' + index + '" class="ReadOnlyValue" style="width: 120px;">' + annexNumber + '</span>';
    rowAnnex.appendChild(txtAnnexNumber);

    var lblAnnexDate = document.createElement("td");
    lblAnnexDate.style.textAlign = "right";
    lblAnnexDate.innerHTML =
        '<span id="lblAnnexDate' + index + '" class="InputLabel">от дата:</span>';
    rowAnnex.appendChild(lblAnnexDate);

    var txtAnnexDate = document.createElement("td");
    txtAnnexDate.style.textAlign = "left";
    txtAnnexDate.innerHTML =
        '<span id="txtAnnexDate' + index + '" class="ReadOnlyValue" style="width: 80px;">' + annexDate + '</span>';
    rowAnnex.appendChild(txtAnnexDate);

    var lblAnnexDurationMonths = document.createElement("td");
    lblAnnexDurationMonths.style.textAlign = "right";
    lblAnnexDurationMonths.innerHTML =
        '<span id="lblAnnexDurationMonths' + index + '" class="InputLabel">Срок:</span>';
    rowAnnex.appendChild(lblAnnexDurationMonths);

    var txtAnnexDurationMonths = document.createElement("td");
    txtAnnexDurationMonths.style.textAlign = "left";
    txtAnnexDurationMonths.innerHTML =
        '<span id="txtAnnexDurationMonths' + index + '" class="ReadOnlyValue">' + annexDurationMonths + '</span>';
    rowAnnex.appendChild(txtAnnexDurationMonths);

    var lblAnnexExpireDate = document.createElement("td");
    lblAnnexExpireDate.style.textAlign = "right";
    lblAnnexExpireDate.innerHTML =
        '<span id="lblAnnexExpireDate' + index + '" class="InputLabel">изтича на:</span>';
    rowAnnex.appendChild(lblAnnexExpireDate);

    var txtAnnexExpireDate = document.createElement("td");
    txtAnnexExpireDate.style.textAlign = "left";
    txtAnnexExpireDate.innerHTML =
        '<span id="txtAnnexExpireDate' + index + '" class="ReadOnlyValue" style="width: 80px;">' + annexExpireDate + '</span>';
    rowAnnex.appendChild(txtAnnexExpireDate);
}

function btnAddAnnex_Click() 
{
    var annexRows = GetElementsByClassNameCustom("rowAnnexLightBox").length;

    RenderAnnexRowLightBox("", "", "", "", "", annexRows + 1);
    
    RefreshAnnexRows();
}

function btnRemoveAnnex_Click(o) 
{
    var td = o.parentNode;
    var tr = td.parentNode;
    
    // Hide the row and mark it as deleted
    tr.style.display = "none";
    var attribute = tr.getAttribute("id");
    var index = attribute.substring(16); // e.g. rowAnnexLightBox1 : the index is at position 16
    document.getElementById("hdnIsDeletedLightBox" + index).value = "yes";

    RefreshAnnexRows();
}

//Controls the addAnnex and removeAnnex buttons visibility
function RefreshAnnexRows() 
{
    var addButtons = GetElementsByClassNameCustom("btnAddAnnex");
    
    for (var i = 0; i < addButtons.length - 1; i++)
    {
        addButtons[i].style.display = "none";
    }
    
    for (var i = addButtons.length - 1; i >= 0; i--)
    {
        if (addButtons[i].parentNode.style.display == "")
        {
            addButtons[i].style.display = "";
            break;
        }
    }
    
    RefreshDatePickers();
}

function ClearAnnexRowsLightBox()
{
    var rows = GetElementsByClassNameCustom("rowAnnexLightBox");
    
    for (var i = rows.length - 1; i >= 0; i--)
    {
        rows[i].parentNode.removeChild(rows[i]);
    }
}

function ClearAnnexRows()
{
    var rows = GetElementsByClassNameCustom("rowAnnex");
    
    for (var i = rows.length - 1; i >= 0; i--)
    {
        rows[i].parentNode.removeChild(rows[i]);
    }
}

//Open the light-box for adding a new record in the Med Cert table
function NewMedCert() {
    ShowAddEditMedCertLightBox(0);
}

//Open the light-box for editing a record in the Med Cert table
function EditMedCert(medCertID) {
    ShowAddEditMedCertLightBox(medCertID);
}

function ShowAddEditMedCertLightBox(medCertID) {
    ClearAllMessages();

    //Set DateTime Pickers
    RefreshDatePickers();

    document.getElementById("hdnMedCertID").value = medCertID;

    //New record
    if (medCertID == 0) {
        document.getElementById("lblAddEditMedCertTitle").innerHTML = "Въвеждане на медицинско освидетелстване";

        document.getElementById("txtMedCertDate").value = "";
        document.getElementById("txtMedCertProtNum").value = "";
        document.getElementById("ddMedCertConclusion").value = optionChooseOneValue;
        document.getElementById("ddMedCertMedRubric").value = optionChooseOneValue;
        document.getElementById("txtMedCertExpirationDate").value = "";

        // clean message label in the light box and hide it
        document.getElementById("spanAddEditMedCertLightBoxMsg").style.display = "none";
        document.getElementById("spanAddEditMedCertLightBoxMsg").innerHTML = "";

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("divMedCertLightBox").style.display = "";
        CenterLightBox("divMedCertLightBox");
    }
    else //Edit record
    {
        document.getElementById("lblAddEditMedCertTitle").innerHTML = "Редактиране на медицинско освидетелстване";

        var url = "AddEditReservist_MilitaryReport.aspx?AjaxMethod=JSLoadMedCert";

        var params = "MedCertID=" + medCertID;

        function response_handler(xml) {
            document.getElementById("txtMedCertDate").value = xmlValue(xml, "medCertDate");
            document.getElementById("txtMedCertProtNum").value = xmlValue(xml, "protNum");
            document.getElementById("ddMedCertConclusion").value = xmlValue(xml, "conclusionID");
            document.getElementById("ddMedCertMedRubric").value = xmlValue(xml, "medRubricID");
            document.getElementById("txtMedCertExpirationDate").value = xmlValue(xml, "medCertExpirationDate");

            // clean message label in the light box and hide it
            document.getElementById("spanAddEditMedCertLightBoxMsg").style.display = "none";
            document.getElementById("spanAddEditMedCertLightBoxMsg").innerHTML = "";

            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("divMedCertLightBox").style.display = "";
            CenterLightBox("divMedCertLightBox");
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Close the light-box
function HideAddEditMedCertLightBox() {
    document.getElementById("txtMedCertDate").value = "";
    document.getElementById("txtMedCertProtNum").value = "";
    document.getElementById("ddMedCertConclusion").value = optionChooseOneValue;
    document.getElementById("ddMedCertMedRubric").value = optionChooseOneValue;
    document.getElementById("txtMedCertExpirationDate").value = "";

    document.getElementById("HidePage").style.display = "none";
    document.getElementById("divMedCertLightBox").style.display = "none";
}

//Save Add/Edit Med Cert
function SaveAddEditMedCertLightBox() {
    if (ValidateAddEditMedCertLightBox()) {
        var url = "AddEditReservist_MilitaryReport.aspx?AjaxMethod=JSSaveMedCert";

        var params = "MedCertID=" + document.getElementById("hdnMedCertID").value +
                     "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value +
                     "&MedCertDate=" + document.getElementById("txtMedCertDate").value +
                     "&MedCertProtNum=" + custEncodeURI(document.getElementById("txtMedCertProtNum").value) +
                     "&MedCertConclusionId=" + document.getElementById("ddMedCertConclusion").value +
                     "&MedCertMedRubricID=" + document.getElementById("ddMedCertMedRubric").value +
                     "&MedCertExpirationDate=" + document.getElementById("txtMedCertExpirationDate").value;

        function response_handler(xml) {
            var status = xmlValue(xml, "status");

            if (status == "OK") {
                var refreshedMedCertTable = xmlValue(xml, "refreshedMedCertTable");
                document.getElementById("divMedCertTable").innerHTML = refreshedMedCertTable;

                document.getElementById("spanMedCertSectionMsg").className = "SuccessText";
                document.getElementById("spanMedCertSectionMsg").innerHTML = document.getElementById("hdnMedCertID").value == "0" ? "Медицинско освидетелстване е добавено успешно" : "Медицинско освидетелстване е редактирано успешно";

                HideAddEditMedCertLightBox();
            }
            else {
                document.getElementById("spanAddEditMedCertLightBoxMsg").className = "ErrorText";
                document.getElementById("spanAddEditMedCertLightBoxMsg").innerHTML = status;
                document.getElementById("spanAddEditMedCertLightBoxMsg").style.display = "";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Client validation of AddEditMedCertLightBox light-box
function ValidateAddEditMedCertLightBox() {
    var res = true;

    var lblMessage = document.getElementById("spanAddEditMedCertLightBoxMsg");
    lblMessage.innerHTML = "";

    var notValidFields = new Array();

    var txtMedCertDate = document.getElementById("txtMedCertDate");
    if (TrimString(txtMedCertDate.value) != "" && !IsValidDate(txtMedCertDate.value)) {
        res = false;
        lblMessage.innerHTML += GetErrorMessageDate("Комисия от дата") + "<br />";
    }

    var txtMedCertExpirationDate = document.getElementById("txtMedCertExpirationDate");
    if (TrimString(txtMedCertExpirationDate.value) != "" && !IsValidDate(txtMedCertExpirationDate.value)) {
        res = false;
        lblMessage.innerHTML += GetErrorMessageDate("Дата на валидност") + "<br />";
    }

    var notValidFieldsCount = notValidFields.length;

    if (notValidFieldsCount > 0) {
        var noRightsMessage = GetErrorMessageNoRights(notValidFields);
        lblMessage.innerHTML += "<br />" + noRightsMessage;
        res = false;
    }

    if (res) {
        lblMessage.className = "SuccessText";
    }
    else {
        lblMessage.className = "ErrorText";
        lblMessage.style.display = "";
    }

    return res;
}

// Delete a particular Med Cert record
function DeleteMedCert(medCertID) {
    ClearAllMessages();

    YesNoDialog("Желаете ли да изтриете медицинското освидетелстване?", ConfirmYes, null);

    function ConfirmYes() {
        var url = "AddEditReservist_MilitaryReport.aspx?AjaxMethod=JSDeleteMedCert";

        var params = "MedCertID=" + medCertID +
                 "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

        function response_handler(xml) {
            var status = xmlValue(xml, "status")

            if (status == "OK") {
                var refreshedMedCertTable = xmlValue(xml, "refreshedMedCertTable");

                document.getElementById("divMedCertTable").innerHTML = refreshedMedCertTable;

                document.getElementById("spanMedCertSectionMsg").className = "SuccessText";
                document.getElementById("spanMedCertSectionMsg").innerHTML = "Медицинското освидетелстване е изтрито успешно";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}


//Open the light-box for adding a new record in the Psych Cert table
function NewPsychCert() {
    ShowAddEditPsychCertLightBox(0);
}

//Open the light-box for editing a record in the Psych Cert table
function EditPsychCert(psychCertID) {
    ShowAddEditPsychCertLightBox(psychCertID);
}

function ShowAddEditPsychCertLightBox(psychCertID) {
    ClearAllMessages();

    //Set DateTime Pickers
    RefreshDatePickers();

    document.getElementById("hdnPsychCertID").value = psychCertID;

    //New record
    if (psychCertID == 0) {
        document.getElementById("lblAddEditPsychCertTitle").innerHTML = "Въвеждане на психологическа пригодност";

        document.getElementById("txtPsychCertDate").value = "";
        document.getElementById("txtPsychCertProtNum").value = "";
        document.getElementById("ddPsychCertConclusion").value = optionChooseOneValue;
        document.getElementById("txtPsychCertExpirationDate").value = "";

        // clean message label in the light box and hide it
        document.getElementById("spanAddEditPsychCertLightBoxMsg").style.display = "none";
        document.getElementById("spanAddEditPsychCertLightBoxMsg").innerHTML = "";

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("divPsychCertLightBox").style.display = "";
        CenterLightBox("divPsychCertLightBox");
    }
    else //Edit record
    {
        document.getElementById("lblAddEditPsychCertTitle").innerHTML = "Редактиране на психологическа пригодност";

        var url = "AddEditReservist_MilitaryReport.aspx?AjaxMethod=JSLoadPsychCert";

        var params = "PsychCertID=" + psychCertID;

        function response_handler(xml) {
            document.getElementById("txtPsychCertDate").value = xmlValue(xml, "psychCertDate");
            document.getElementById("txtPsychCertProtNum").value = xmlValue(xml, "protNum");
            document.getElementById("ddPsychCertConclusion").value = xmlValue(xml, "conclusionID");
            document.getElementById("txtPsychCertExpirationDate").value = xmlValue(xml, "psychCertExpirationDate");

            // clean message label in the light box and hide it
            document.getElementById("spanAddEditPsychCertLightBoxMsg").style.display = "none";
            document.getElementById("spanAddEditPsychCertLightBoxMsg").innerHTML = "";

            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("divPsychCertLightBox").style.display = "";
            CenterLightBox("divPsychCertLightBox");
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Close the light-box
function HideAddEditPsychCertLightBox() {
    document.getElementById("txtPsychCertDate").value = "";
    document.getElementById("txtPsychCertProtNum").value = "";
    document.getElementById("ddPsychCertConclusion").value = optionChooseOneValue;
    document.getElementById("txtPsychCertExpirationDate").value = "";

    document.getElementById("HidePage").style.display = "none";
    document.getElementById("divPsychCertLightBox").style.display = "none";
}

//Save Add/Edit Psych Cert
function SaveAddEditPsychCertLightBox() {
    if (ValidateAddEditPsychCertLightBox()) {
        var url = "AddEditReservist_MilitaryReport.aspx?AjaxMethod=JSSavePsychCert";

        var params = "PsychCertID=" + document.getElementById("hdnPsychCertID").value +
                     "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value +
                     "&PsychCertDate=" + document.getElementById("txtPsychCertDate").value +
                     "&PsychCertProtNum=" + custEncodeURI(document.getElementById("txtPsychCertProtNum").value) +
                     "&PsychCertConclusionId=" + document.getElementById("ddPsychCertConclusion").value +
                     "&PsychCertExpirationDate=" + document.getElementById("txtPsychCertExpirationDate").value;

        function response_handler(xml) {
            var status = xmlValue(xml, "status");

            if (status == "OK") {
                var refreshedPsychCertTable = xmlValue(xml, "refreshedPsychCertTable");
                document.getElementById("divPsychCertTable").innerHTML = refreshedPsychCertTable;

                document.getElementById("spanPsychCertSectionMsg").className = "SuccessText";
                document.getElementById("spanPsychCertSectionMsg").innerHTML = document.getElementById("hdnPsychCertID").value == "0" ? "Психологическата пригодност е добавена успешно" : "Психологическата пригодност е редактирана успешно";

                HideAddEditPsychCertLightBox();
            }
            else {
                document.getElementById("spanAddEditPsychCertLightBoxMsg").className = "ErrorText";
                document.getElementById("spanAddEditPsychCertLightBoxMsg").innerHTML = status;
                document.getElementById("spanAddEditPsychCertLightBoxMsg").style.display = "";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Client validation of AddEditPsychCertLightBox light-box
function ValidateAddEditPsychCertLightBox() {
    var res = true;

    var lblMessage = document.getElementById("spanAddEditPsychCertLightBoxMsg");
    lblMessage.innerHTML = "";

    var notValidFields = new Array();

    var txtPsychCertDate = document.getElementById("txtPsychCertDate");
    if (TrimString(txtPsychCertDate.value) != "" && !IsValidDate(txtPsychCertDate.value)) {
        res = false;
        lblMessage.innerHTML += GetErrorMessageDate("Комисия от дата") + "<br />";
    }

    var txtPsychCertExpirationDate = document.getElementById("txtPsychCertExpirationDate");
    if (TrimString(txtPsychCertExpirationDate.value) != "" && !IsValidDate(txtPsychCertExpirationDate.value)) {
        res = false;
        lblMessage.innerHTML += GetErrorMessageDate("Дата на валидност") + "<br />";
    }

    var notValidFieldsCount = notValidFields.length;

    if (notValidFieldsCount > 0) {
        var noRightsMessage = GetErrorMessageNoRights(notValidFields);
        lblMessage.innerHTML += "<br />" + noRightsMessage;
        res = false;
    }

    if (res) {
        lblMessage.className = "SuccessText";
    }
    else {
        lblMessage.className = "ErrorText";
        lblMessage.style.display = "";
    }

    return res;
}

// Delete a particular Psych Cert record
function DeletePsychCert(psychCertID) {
    ClearAllMessages();

    YesNoDialog("Желаете ли да изтриете психологическа пригодност?", ConfirmYes, null);

    function ConfirmYes() {
        var url = "AddEditReservist_MilitaryReport.aspx?AjaxMethod=JSDeletePsychCert";

        var params = "PsychCertID=" + psychCertID +
                 "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

        function response_handler(xml) {
            var status = xmlValue(xml, "status")

            if (status == "OK") {
                var refreshedPsychCertTable = xmlValue(xml, "refreshedPsychCertTable");

                document.getElementById("divPsychCertTable").innerHTML = refreshedPsychCertTable;

                document.getElementById("spanPsychCertSectionMsg").className = "SuccessText";
                document.getElementById("spanPsychCertSectionMsg").innerHTML = "Психологическата пригодност е изтрита успешно";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}