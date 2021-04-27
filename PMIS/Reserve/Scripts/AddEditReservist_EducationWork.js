//Clear all messages on the EducationWork tab
function ClearEducationWorkMessages()
{

    if (document.getElementById("lblMessageForeignLanguage"))
        document.getElementById("lblMessageForeignLanguage").innerHTML = "";

    if (document.getElementById("lblMessageCivilEducation"))
        document.getElementById("lblMessageCivilEducation").innerHTML = "";

    if (document.getElementById("lblMessageMilitaryEducation"))
        document.getElementById("lblMessageMilitaryEducation").innerHTML = "";

    if (document.getElementById("lblMessageMilitaryEducationAcademy"))
        document.getElementById("lblMessageMilitaryEducationAcademy").innerHTML = "";

    if (document.getElementById("lblMessageTrainigCource"))
        document.getElementById("lblMessageTrainigCource").innerHTML = "";

    if (document.getElementById("lblMessageScientificTitle"))
        document.getElementById("lblMessageScientificTitle").innerHTML = "";

    if (document.getElementById("lblWorkplaceDataMessage"))
        document.getElementById("lblWorkplaceDataMessage").innerHTML = "";
}

// 0.----------- Workplace Section --------------------


//
function txtUnifiedIdentityCode_Focus() {
    var txtUnifiedIdentityCode = document.getElementById("txtUnifiedIdentityCode");
    txtUnifiedIdentityCode.setAttribute("oldvalue", txtUnifiedIdentityCode.value);
}

function btnSavePersonWorkPlaceData_Click(msg)
 {
    ClearAllMessages();
    lblMessage = document.getElementById('lblWorkplaceDataMessage');
    if (msg == '') {
        SavePersonWorkPlaceData(function() {
            lblMessage.className = 'SuccessText';
            lblMessage.innerHTML = 'Записът е успешен';
        });
    } else {
        lblMessage.className = 'ErrorText';
        lblMessage.style.display = '';
        lblMessage.innerHTML = msg;
    }
}

// Save workplace data for the contextual person
function SavePersonWorkPlaceData(savePersonWorkPlaceDataFinishCallback) {

    if (IsTabAlreadyVisited("btnTabEducationWork") && document.getElementById("btnImgSave")) {
        var url = "AddEditReservist_EducationWork.aspx?AjaxMethod=JSSavePersonWorkPlaceData";
        var params = "";
        params += "ReservistId=" + document.getElementById(hdnReservistIdClientID).value;
        params += "&CompID=" + document.getElementById("hdnCompanyID").value;
        params += "&NKPDId=" + document.getElementById("hdnWorkPositionNKPDID").value;
        var myAJAX = new AJAX(url, true, params, SavePersonWorkPlaceData_Click);
        myAJAX.Call();
    } else {
        savePersonWorkPlaceDataFinishCallback();
    }
    
    function SavePersonWorkPlaceData_Click(xml) {
        var message = xmlValue(xml, "message");
     
        setTimeout("LoadOriginalValues()", 1000);

        Refresh_MilitaryReport_Postpone_WorkplaceInfo();
        savePersonWorkPlaceDataFinishCallback();
    }
}

function IsWorkplaceDataValid(callbackIfValid) {
    var tabNameHeader = "Образование/Работа: ";
    var ValidationMessage = "";

    if (IsTabAlreadyVisited("btnTabEducationWork")) {
        var notValidFields = new Array();

        var companyID = ""; 
        
        if(document.getElementById("hdnCompanyID"))
            companyID = document.getElementById("hdnCompanyID").value;
            
        if (companyID == "" || parseInt(companyID) <= 0) {
            if (!document.getElementById("btnSelectCompany") || document.getElementById("btnSelectCompany").style.display == "none")
                notValidFields.push("Име на фирмата");
            else {
                ValidationMessage += tabNameHeader + GetErrorMessageMandatory("Име на фирмата") + "</br>";
            }
        }

        if (document.getElementById("hdnWorkPositionNKPDID")) {
            if (document.getElementById("hdnWorkPositionNKPDID").value == "" && document.getElementById("txtWorkPositionNKPDCode").value != "") {
                ValidationMessage += tabNameHeader + "Невалиден код по НКПД" + "<br />";
            }
        }

        var notValidFieldsCount = notValidFields.length;

        if (notValidFieldsCount > 0) {
            var noRightsMessage = GetErrorMessageNoRights(notValidFields);
            ValidationMessage += "<br />" + tabNameHeader + noRightsMessage;
        }
        
        callbackIfValid(ValidationMessage);
    } else {
        callbackIfValid(ValidationMessage);
    }
}


// 1.----------- Table CivilEducation -------------------

//Load the CivilEducation table on demand
function lnkCivilEducation_Click()
{
    LoadCivilEducations();
}

function LoadCivilEducations()
{
    //If already loaded then do not load
    if (document.getElementById("hdnCivilEducationLoaded").value == "1")
        return;

    ClearAllMessages();

    document.getElementById("imgLoadingCivilEducation").style.visibility = "";

    var url = "AddEditReservist_EducationWork.aspx?AjaxMethod=JSLoadCivilEducations";
    var params = "";
    params += "ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

    var hdnIsPreview = document.getElementById(hdnIsPreviewClientID).value;
    if (hdnIsPreview == 1)
        params += "&Preview=1";
    
    function LoadCivilEducation_CallBack(xml)
    {
        var tableHTML = xmlValue(xml, "tableHTML");
        var lightBoxHTML = xmlValue(xml, "lightBoxHTML");

        document.getElementById("tblCivilEducation").innerHTML = tableHTML;
        document.getElementById("lboxCivilEducation").innerHTML = lightBoxHTML;

        RefreshUIItems(xml);

        document.getElementById("imgLoadingCivilEducation").style.visibility = "hidden";

        //Mark as already loaded
        document.getElementById("hdnCivilEducationLoaded").value = "1";
    }

    var myAJAX = new AJAX(url, true, params, LoadCivilEducation_CallBack);
    myAJAX.Call();
}

//Open the light-box for adding a new record in the CivilEducation table
function NewCivilEducation()
{
    ShowAddEditCivilEducationLightBox(0);
}

//Open the light-box for editing a record in the CivilEducation table
function EditCivilEducation(civilEducationId)
{
    ShowAddEditCivilEducationLightBox(civilEducationId);
}

function ShowAddEditCivilEducationLightBox(civilEducationId)
{
    ClearAllMessages();

    document.getElementById("hdnCivilEducationID").value = civilEducationId;

    //New record
    if (civilEducationId == 0)
    {
        document.getElementById("lblAddEditCivilEducationTitle").innerHTML = "Въвеждане на ново гражданско образование";

        if (document.getElementById("ddCivilEduPersonEducation") != undefined)
            document.getElementById("ddCivilEduPersonEducation").value = optionChooseOneValue;

        document.getElementById("txtSubject").innerHTML = "";
            
        if (document.getElementById("txtCivilEduGraduateYear") != undefined)
            document.getElementById("txtCivilEduGraduateYear").value = "";
        if (document.getElementById("ddCivilEduLearningMethod") != undefined)
            document.getElementById("ddCivilEduLearningMethod").value = optionChooseOneValue;


        // clean message label in the light box and hide it
        document.getElementById("spanAddEditCivilEducationLightBox").style.display = "none";
        document.getElementById("spanAddEditCivilEducationLightBox").innerHTML = "";

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("lboxCivilEducation").style.display = "";
        CenterLightBox("lboxCivilEducation");
    }
    else //Edit Position
    {
        document.getElementById("lblAddEditCivilEducationTitle").innerHTML = "Редактиране на гражданско образование";

        var url = "AddEditReservist_EducationWork.aspx?AjaxMethod=JSLoadCivilEducation";

        var params = "CivilEducationId=" + civilEducationId;

        function response_handler(xml)
        {
            var personCivilEducation = xml.getElementsByTagName("personCivilEducation")[0];

            var educationCode = xmlValue(personCivilEducation, "educationCode");
            var schoolSubjectCode = xmlValue(personCivilEducation, "schoolSubjectCode");
            var schoolSubjectName = xmlValue(personCivilEducation, "schoolSubjectName");
            var graduateYear = xmlValue(personCivilEducation, "graduateYear");
            var learningMethodKey = xmlValue(personCivilEducation, "learningMethodKey");

            if (document.getElementById("ddCivilEduPersonEducation") != undefined)
                document.getElementById("ddCivilEduPersonEducation").value = educationCode;

            document.getElementById("hdnSchoolSubjectCode").value = schoolSubjectCode;
            document.getElementById("txtSubject").innerHTML = schoolSubjectName;
                
            if (document.getElementById("txtCivilEduGraduateYear") != undefined)
                document.getElementById("txtCivilEduGraduateYear").value = graduateYear;
            if (document.getElementById("ddCivilEduLearningMethod") != undefined)
                document.getElementById("ddCivilEduLearningMethod").value = learningMethodKey;

            // clean message label in the light box and hide it
            document.getElementById("spanAddEditCivilEducationLightBox").style.display = "none";
            document.getElementById("spanAddEditCivilEducationLightBox").innerHTML = "";

            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("lboxCivilEducation").style.display = "";
            CenterLightBox("lboxCivilEducation");
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Close the light-box
function HideAddEditCivilEducationLightBox()
{
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("lboxCivilEducation").style.display = "none";
}

//Save Add/Edit CivilEducation
function SaveAddEditCivilEducationLightBox()
{
    if (ValidateAddEditCivilEducation())
    {
        var url = "AddEditReservist_EducationWork.aspx?AjaxMethod=JSSaveCivilEducation";

        var params = "CivilEducationId=" + document.getElementById("hdnCivilEducationID").value +
                     "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value +
                     "&PersonEducationCode=" + document.getElementById("ddCivilEduPersonEducation").value +
                     "&PersonSchoolSubjectCode=" + document.getElementById("hdnSchoolSubjectCode").value +
                     "&GraduateYear=" + document.getElementById("txtCivilEduGraduateYear").value +
                     "&LearningMethodKey=" + document.getElementById("ddCivilEduLearningMethod").value;

        function response_handler(xml)
        {
            var status = xmlValue(xml, "status");

            if (status == "OK")
            {
                var refreshedPositionsTable = xmlValue(xml, "refreshedCivilEducationTable");

                document.getElementById("tblCivilEducation").innerHTML = refreshedPositionsTable;

                document.getElementById("lblMessageCivilEducation").className = "SuccessText";
                document.getElementById("lblMessageCivilEducation").innerHTML = document.getElementById("hdnCivilEducationID").value == "0" ? "Гражданското образование е добавено успешно" : "Гражданското образование е редактирано успешно";

                HideAddEditCivilEducationLightBox();
            }
            else
            {
                document.getElementById("spanAddEditCivilEducationLightBox").className = "ErrorText";
                document.getElementById("spanAddEditCivilEducationLightBox").innerHTML = status;
                document.getElementById("spanAddEditCivilEducationLightBox").style.display = "";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Client validation of Add/Edit Civil Education light-box
function ValidateAddEditCivilEducation()
{
    var res = true;

    var lblMessage = document.getElementById("spanAddEditCivilEducationLightBox");
    lblMessage.innerHTML = "";

    var notValidFields = new Array();

    var ddCivilEduPersonEducation = document.getElementById("ddCivilEduPersonEducation");
    var txtCivilEduGraduateYear = document.getElementById("txtCivilEduGraduateYear");
    var ddCivilEduLearningMethod = document.getElementById("ddCivilEduLearningMethod");


    if (ddCivilEduPersonEducation.value == optionChooseOneValue)
    {
        res = false;

        if (ddCivilEduPersonEducation.disabled == true || ddCivilEduPersonEducation.style.display == "none")
            notValidFields.push("Образователна степен");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Образователна степен") + "<br />";
    }

    if (ItemSelectorUtil.GetSelectedValue("isCivilEduPersonSchoolSubject") == optionChooseOneValue)
    {
        res = false;

        if (ItemSelectorUtil.IsDisabled("isCivilEduPersonSchoolSubject") || ItemSelectorUtil.IsHidden("isCivilEduPersonSchoolSubject"))
            notValidFields.push("Специалност");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Специалност") + "<br />";
    }


    if (txtCivilEduGraduateYear.value.Trim() == "")
    {
        res = false;

        if (txtCivilEduGraduateYear.disabled == true || txtCivilEduGraduateYear.style.display == "none")
            notValidFields.push("Година на завършване");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Година на завършване") + "<br />";
    }
    else
    {
        if (!isInt(txtCivilEduGraduateYear.value) || parseInt(txtCivilEduGraduateYear.value) <= 0)
        {
            res = false;
            lblMessage.innerHTML += GetErrorMessageNumber("Година на завършване") + "<br />";
        }
    }


    if (ddCivilEduLearningMethod.value == optionChooseOneValue)
    {
        res = false;

        if (ddCivilEduLearningMethod.disabled == true || ddCivilEduLearningMethod.style.display == "none")
            notValidFields.push("Начин на обучение");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Начин на обучение") + "<br />";
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

// Delete a particular Civil Education record
function DeleteCivilEducation(civilEducationId)
{
    ClearAllMessages();

    YesNoDialog("Желаете ли да изтриете гражданското образование?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "AddEditReservist_EducationWork.aspx?AjaxMethod=JSDeleteCivilEducation";

        var params = "CivilEducationId=" + civilEducationId;
        params += "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

        function response_handler(xml)
        {
            var status = xmlValue(xml, "status")

            if (status == "OK")
            {
                var refreshedPositionsTable = xmlValue(xml, "refreshedCivilEducationTable");

                document.getElementById("tblCivilEducation").innerHTML = refreshedPositionsTable;

                document.getElementById("lblMessageCivilEducation").className = "SuccessText";
                document.getElementById("lblMessageCivilEducation").innerHTML = "Гражданското образование е изтрито успешно";

            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}



// 2. Table MilitaryEducation

//Load the MilitaryEducation table on demand
function lnkMilitaryEducation_Click()
{
    LoadMilitaryEduAndMilitaryEduAcademy();
}

function LoadMilitaryEduAndMilitaryEduAcademy()
{
    //If already loaded then do not load
    if ((document.getElementById("hdnMilitaryEducationLoaded").value == "1") && (document.getElementById("hdnMilitaryEducationAcademyLoaded").value == "1"))
        return;

    ClearAllMessages();

    document.getElementById("imgLoadingMilitaryEducation").style.visibility = "";

    var url = "AddEditReservist_EducationWork.aspx?AjaxMethod=JSLoadMilitaryEduAndMilitaryEduAcademy";
    var params = "";
    params += "ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

    var hdnIsPreview = document.getElementById(hdnIsPreviewClientID).value;
    if (hdnIsPreview == 1)
        params += "&Preview=1";
    
    function LoadLoadMilitaryEducation_CallBack(xml)
    {
        //Bind Table and LightBox MilitaryEdu
        var tableMilitaryEduHTML = xmlValue(xml, "tableMilitaryEduHTML");
        var lightBoxMilitaryEduHTML = xmlValue(xml, "lightBoxMilitaryEduHTML");

        document.getElementById("tblMilitaryEducation").innerHTML = tableMilitaryEduHTML;
        document.getElementById("lboxMilitaryEducation").innerHTML = lightBoxMilitaryEduHTML;

        RefreshUIItems(xml, "UIItemsMilitaryEdu");

        //Bind Table and LightBox MilitaryEduAcademy
        var tableMilitaryEduAcademyHTML = xmlValue(xml, "tableMilitaryEduAcademyHTML");
        var lightMilitaryEduAcademyBoxHTML = xmlValue(xml, "lightMilitaryEduAcademyBoxHTML");
        document.getElementById("tblMilitaryEducationAcademy").innerHTML = tableMilitaryEduAcademyHTML;
        document.getElementById("lboxMilitaryEducationAcademy").innerHTML = lightMilitaryEduAcademyBoxHTML;

        RefreshUIItems(xml, "UIItemsMilitaryEduAcademy");

        document.getElementById("imgLoadingMilitaryEducation").style.visibility = "hidden";

        //Mark as already loaded
        document.getElementById("hdnMilitaryEducationLoaded").value = "1";
        document.getElementById("hdnMilitaryEducationAcademyLoaded").value = "1";
    }

    var myAJAX = new AJAX(url, true, params, LoadLoadMilitaryEducation_CallBack);
    myAJAX.Call();
}


//Open the light-box for adding a new record in the MilitaryEducation table
function NewMilitaryEducation()
{
    ShowAddEditMilitaryEducationLightBox(0);
}

//Open the light-box for editing a record in the CivilEducation table
function EditMilitaryEducation(militaryEducationId)
{
    ShowAddEditMilitaryEducationLightBox(militaryEducationId);
}

function ShowAddEditMilitaryEducationLightBox(militaryEducationId)
{
    ClearAllMessages();

    document.getElementById("hdnMilitaryEducationID").value = militaryEducationId;

    //New record
    if (militaryEducationId == 0)
    {
        document.getElementById("lblAddEditMilitaryEducationTitle").innerHTML = "Въвеждане на ново военно образование";

        if (document.getElementById("ddPersonMilitaryEducationMilitarySchool") != undefined)
            document.getElementById("ddPersonMilitaryEducationMilitarySchool").value = optionChooseOneValue;
        if (document.getElementById("ddPersonMilitaryEducationMilitaryEducationType") != undefined)
            document.getElementById("ddPersonMilitaryEducationMilitaryEducationType").value = optionChooseOneValue;
        if (document.getElementById("ddPersonMilitaryEducationMilitarySchoolSubject") != undefined)
            document.getElementById("ddPersonMilitaryEducationMilitarySchoolSubject").value = optionChooseOneValue;
        if (document.getElementById("ddPersonMilitaryEducationMilitaryArms") != undefined)
            document.getElementById("ddPersonMilitaryEducationMilitaryArms").value = optionChooseOneValue;
        if (document.getElementById("ddPersonMilitaryEducationCountry") != undefined)
            document.getElementById("ddPersonMilitaryEducationCountry").value = optionChooseOneValue;
        if (document.getElementById("txtMilitaryEducationGraduateYear") != undefined)
            document.getElementById("txtMilitaryEducationGraduateYear").value = "";
        if (document.getElementById("ddPersonMilitaryEducationLearningMethod") != undefined)
            document.getElementById("ddPersonMilitaryEducationLearningMethod").value = optionChooseOneValue;

        if (document.getElementById("ddPersonMilitaryEducationVitoshaMilitaryReportSpecialityType") != undefined) {
            document.getElementById("ddPersonMilitaryEducationVitoshaMilitaryReportSpecialityType").value = optionChooseOneValue;
            ClearSelectList(document.getElementById("ddPersonMilitaryEducationVitoshaMilitaryReportSpeciality"), true);
        }
            
        // clean message label in the light box and hide it
        document.getElementById("spanAddEditMilitaryEducationLightBox").style.display = "none";
        document.getElementById("spanAddEditMilitaryEducationLightBox").innerHTML = "";

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("lboxMilitaryEducation").style.display = "";
        CenterLightBox("lboxMilitaryEducation");
    }
    else //Edit Position
    {
        document.getElementById("lblAddEditMilitaryEducationTitle").innerHTML = "Редактиране на военно образование";

        var url = "AddEditReservist_EducationWork.aspx?AjaxMethod=JSLoadMilitaryEducation";

        var params = "MilitaryEducationId=" + militaryEducationId;

        function response_handler(xml)
        {
            var personMilitaryEducation = xml.getElementsByTagName("personMilitaryEducation")[0];

            var militarySchoolId = xmlValue(personMilitaryEducation, "militarySchoolId");
            var militaryEducationTypeCode = xmlValue(personMilitaryEducation, "militaryEducationTypeCode");
            var militarySchoolSubjectId = xmlValue(personMilitaryEducation, "militarySchoolSubjectId");
            var militaryArmsCode = xmlValue(personMilitaryEducation, "militaryArmsCode");
            var countryId = xmlValue(personMilitaryEducation, "countryId");
            var graduateYear = xmlValue(personMilitaryEducation, "graduateYear");
            var learningMethodKey = xmlValue(personMilitaryEducation, "learningMethodKey");

            var vitoshaMilitaryReportSpecialityTypeCode = xmlValue(personMilitaryEducation, "vitoshaMilitaryReportSpecialityTypeCode");
            var vitoshaMilitaryReportSpecialityCode = xmlValue(personMilitaryEducation, "vitoshaMilitaryReportSpecialityCode");

            if (document.getElementById("ddPersonMilitaryEducationMilitarySchool") != undefined)
                document.getElementById("ddPersonMilitaryEducationMilitarySchool").value = militarySchoolId;
            if (document.getElementById("ddPersonMilitaryEducationMilitaryEducationType") != undefined)
                document.getElementById("ddPersonMilitaryEducationMilitaryEducationType").value = militaryEducationTypeCode;
            if (document.getElementById("ddPersonMilitaryEducationMilitarySchoolSubject") != undefined)
                document.getElementById("ddPersonMilitaryEducationMilitarySchoolSubject").value = militarySchoolSubjectId;
            if (document.getElementById("ddPersonMilitaryEducationMilitaryArms") != undefined)
                document.getElementById("ddPersonMilitaryEducationMilitaryArms").value = militaryArmsCode;
            if (document.getElementById("ddPersonMilitaryEducationCountry") != undefined)
                document.getElementById("ddPersonMilitaryEducationCountry").value = countryId;
            if (document.getElementById("txtMilitaryEducationGraduateYear") != undefined)
                document.getElementById("txtMilitaryEducationGraduateYear").value = graduateYear;
            if (document.getElementById("ddPersonMilitaryEducationLearningMethod") != undefined)
                document.getElementById("ddPersonMilitaryEducationLearningMethod").value = learningMethodKey;

            if (document.getElementById("ddPersonMilitaryEducationVitoshaMilitaryReportSpecialityType") != undefined) {
                document.getElementById("ddPersonMilitaryEducationVitoshaMilitaryReportSpecialityType").value = vitoshaMilitaryReportSpecialityTypeCode;

                ClearSelectList(document.getElementById("ddPersonMilitaryEducationVitoshaMilitaryReportSpeciality"), true);

                var milRepSpecs = xml.getElementsByTagName("vitoshaMilRepSpecOp");
             
                for (var i = 0; i < milRepSpecs.length; i++) {
                    var id = xmlValue(milRepSpecs[i], "code");
                    var name = xmlValue(milRepSpecs[i], "name");

                    AddToSelectList(document.getElementById("ddPersonMilitaryEducationVitoshaMilitaryReportSpeciality"), id, name, true);
                };

                document.getElementById("ddPersonMilitaryEducationVitoshaMilitaryReportSpeciality").value = vitoshaMilitaryReportSpecialityCode;
            }
            
            //            document.getElementById("ddPersonMilitaryEducationMilitarySchool").value = militarySchoolId;
            //            document.getElementById("ddPersonMilitaryEducationMilitaryEducationType").value = militaryEducationTypeCode;
            //            document.getElementById("ddPersonMilitaryEducationMilitarySchoolSubject").value = militarySchoolSubjectId;
            //            document.getElementById("ddPersonMilitaryEducationMilitaryArms").value = militaryArmsCode;
            //            document.getElementById("ddPersonMilitaryEducationCountry").value = countryId;
            //            document.getElementById("txtMilitaryEducationGraduateYear").value = graduateYear;
            //            document.getElementById("ddPersonMilitaryEducationLearningMethod").value = learningMethodKey;

            // clean message label in the light box and hide it
            document.getElementById("spanAddEditMilitaryEducationLightBox").style.display = "none";
            document.getElementById("spanAddEditMilitaryEducationLightBox").innerHTML = "";

            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("lboxMilitaryEducation").style.display = "";
            CenterLightBox("lboxMilitaryEducation");
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Close the light-box
function HideAddEditMilitaryEducationLightBox()
{
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("lboxMilitaryEducation").style.display = "none";
}

//Save Add/Edit CivilEducation
function SaveAddEditMilitaryEducationLightBox()
{
    if (ValidateAddEditMilitaryEducation())
    {
        var url = "AddEditReservist_EducationWork.aspx?AjaxMethod=JSSaveMilitaryEducation";

        var MilitaryArmsCode = "";
        if (document.getElementById("ddPersonMilitaryEducationMilitaryArms") != undefined)
        {
            if (document.getElementById("ddPersonMilitaryEducationMilitaryArms").value != optionChooseOneValue)
            {
                MilitaryArmsCode = document.getElementById("ddPersonMilitaryEducationMilitaryArms").value
            }
        }
        
        var params = "MilitaryEducationId=" + document.getElementById("hdnMilitaryEducationID").value +
                     "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value +
                     "&MilitarySchoolId=" + document.getElementById("ddPersonMilitaryEducationMilitarySchool").value +
                     "&MilitaryEducationTypeCode=" + document.getElementById("ddPersonMilitaryEducationMilitaryEducationType").value +
                     "&MilitarySchoolSubjectId=" + document.getElementById("ddPersonMilitaryEducationMilitarySchoolSubject").value +
                     "&MilitaryArmsCode=" + MilitaryArmsCode +
                     "&CountryId=" + document.getElementById("ddPersonMilitaryEducationCountry").value +
                     "&GraduateYear=" + document.getElementById("txtMilitaryEducationGraduateYear").value +
                     "&LearningMethodKey=" + document.getElementById("ddPersonMilitaryEducationLearningMethod").value +
                     "&VitoshaMilitaryReportSpecialityCode=" + document.getElementById("ddPersonMilitaryEducationVitoshaMilitaryReportSpeciality").value;

        function response_handler(xml)
        {
            var status = xmlValue(xml, "status");

            if (status == "OK")
            {
                var refreshedPositionsTable = xmlValue(xml, "refreshedMilitaryEducationTable");

                document.getElementById("tblMilitaryEducation").innerHTML = refreshedPositionsTable;

                document.getElementById("lblMessageMilitaryEducation").className = "SuccessText";
                document.getElementById("lblMessageMilitaryEducation").innerHTML = document.getElementById("hdnMilitaryEducationID").value == "0" ? "Военното образование е добавено успешно" : "Военното образование е редактирано успешно";

                HideAddEditMilitaryEducationLightBox();
            }
            else
            {
                document.getElementById("spanAddEditMilitaryEducationLightBox").className = "ErrorText";
                document.getElementById("spanAddEditMilitaryEducationLightBox").innerHTML = status;
                document.getElementById("spanAddEditMilitaryEducationLightBox").style.display = "";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Client validation of Add/Edit Civil Education light-box
function ValidateAddEditMilitaryEducation()
{
    var res = true;

    var lblMessage = document.getElementById("spanAddEditMilitaryEducationLightBox");
    lblMessage.innerHTML = "";

    var notValidFields = new Array();

    var ddlMilitaryEducationMilitarySchool = document.getElementById("ddPersonMilitaryEducationMilitarySchool");
    var ddlMilitaryEducationTypeCode = document.getElementById("ddPersonMilitaryEducationMilitaryEducationType");
    var ddlMilitaryEducationMilitarySchoolSubject = document.getElementById("ddPersonMilitaryEducationMilitarySchoolSubject");
    var ddlMilitaryEducationMilitaryArms = document.getElementById("ddPersonMilitaryEducationMilitaryArms");
    var ddlMilitaryEducationCountry = document.getElementById("ddPersonMilitaryEducationCountry");
    var ddlMilitaryEducationLearningMethod = document.getElementById("ddPersonMilitaryEducationLearningMethod");
    var txtMilitaryEducationGraduateYear = document.getElementById("txtMilitaryEducationGraduateYear");



    if (ddlMilitaryEducationMilitarySchool.value == optionChooseOneValue)
    {
        res = false;
        if (ddlMilitaryEducationMilitarySchool.disabled == true || ddlMilitaryEducationMilitarySchool.style.display == "none")
            notValidFields.push("Военно училище");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Военно училище") + "<br />";
    }



    if (ddlMilitaryEducationTypeCode.value == optionChooseOneValue)
    {
        res = false;
        if (ddlMilitaryEducationTypeCode.disabled == true || ddlMilitaryEducationTypeCode.style.display == "none")
            notValidFields.push("Вид на военното образование");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Вид на военното образование") + "<br />";
    }


    if (ddlMilitaryEducationMilitarySchoolSubject.value == optionChooseOneValue)
    {
        res = false;

        if (ddlMilitaryEducationMilitarySchoolSubject.disabled == true || ddlMilitaryEducationMilitarySchoolSubject.style.display == "none")
            notValidFields.push("Военна специалност");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Военна специалност") + "<br />";
    }

    //    if (ddlMilitaryEducationMilitaryArms.value == optionChooseOneValue)
    //    {
    //        res = false;

    //        if (ddlMilitaryEducationMilitaryArms.disabled == true || ddlMilitaryEducationMilitaryArms.style.display == "none")
    //            notValidFields.push("Род войска");
    //        else
    //            lblMessage.innerHTML += GetErrorMessageMandatory("Род войска") + "<br />";
    //    }


    if (ddlMilitaryEducationCountry.value == optionChooseOneValue)
    {
        res = false;
        if (ddlMilitaryEducationCountry.disabled == true || ddlMilitaryEducationCountry.style.display == "none")
            notValidFields.push("Държава");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Държава") + "<br />";
    }



    if (ddlMilitaryEducationLearningMethod.value == optionChooseOneValue)
    {
        res = false;
        if (ddlMilitaryEducationLearningMethod.disabled == true || ddlMilitaryEducationLearningMethod.style.display == "none")
            notValidFields.push("Начин на обучение");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Начин на обучение") + "<br />";
    }

    if (txtMilitaryEducationGraduateYear.value.Trim() == "")
    {
        res = false;
        if (txtMilitaryEducationGraduateYear.disabled == true || txtMilitaryEducationGraduateYear.style.display == "none")
            notValidFields.push("Година на завършване");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Година на завършване") + "<br />";
    }
    else
    {
        if (!isInt(txtMilitaryEducationGraduateYear.value) || parseInt(txtMilitaryEducationGraduateYear.value) <= 0)
        {
            res = false;
            lblMessage.innerHTML += GetErrorMessageNumber("Година на завършване") + "<br />";
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

// Delete a particular Military Education record
function DeleteMilitaryEducation(militaryEducationId)
{
    ClearAllMessages();

    YesNoDialog("Желаете ли да изтриете военното образование?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "AddEditReservist_EducationWork.aspx?AjaxMethod=JSDeleteMilitaryEducation";

        var params = "MilitaryEducationId=" + militaryEducationId;
        params += "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

        function response_handler(xml)
        {
            var status = xmlValue(xml, "status")

            if (status == "OK")
            {
                var refreshedPositionsTable = xmlValue(xml, "refreshedMilitaryEducationTable");

                document.getElementById("tblMilitaryEducation").innerHTML = refreshedPositionsTable;

                document.getElementById("lblMessageMilitaryEducation").className = "SuccessText";
                document.getElementById("lblMessageMilitaryEducation").innerHTML = "Военното образование е изтрито успешно";

            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}



// 3. Table MilitaryEducationAcademy

function LoadMilitaryEducationAcademys()
{
    //If already loaded then do not load
    if (document.getElementById("hdnMilitaryEducationAcademyLoaded").value == "1")
        return;

    ClearAllMessages();

    document.getElementById("imgLoadingMilitaryEducation").style.visibility = "";

    var url = "AddEditReservist_EducationWork.aspx?AjaxMethod=JSLoadMilitaryEducationAcademys";
    var params = "";
    params += "ReservistId=" + document.getElementById(hdnReservistIdClientID).value;
    
    function LoadLoadMilitaryEducationAcademy_CallBack(xml)
    {
        var tableHTML = xmlValue(xml, "tableHTML");
        var lightBoxHTML = xmlValue(xml, "lightBoxHTML");

        document.getElementById("tblMilitaryEducationAcademy").innerHTML = tableHTML;
        document.getElementById("lboxMilitaryEducationAcademy").innerHTML = lightBoxHTML;

        document.getElementById("imgLoadingMilitaryEducation").style.visibility = "hidden";

        //Mark as already loaded
        document.getElementById("hdnMilitaryEducationAcademyLoaded").value = "1";
    }

    var myAJAX = new AJAX(url, true, params, LoadLoadMilitaryEducationAcademy_CallBack);
    myAJAX.Call();
}


//Open the light-box for adding a new record in the CivilEducation table
function NewMilitaryEducationAcademy()
{
    ShowAddEditMilitaryEducationAcademyLightBox(0);
}

//Open the light-box for editing a record in the CivilEducation table
function EditMilitaryEducationAcademy(MilitaryEducationAcademyId)
{
    ShowAddEditMilitaryEducationAcademyLightBox(MilitaryEducationAcademyId);
}

function ShowAddEditMilitaryEducationAcademyLightBox(MilitaryEducationAcademyId)
{
    ClearAllMessages();

    document.getElementById("hdnMilitaryEducationAcademyID").value = MilitaryEducationAcademyId;

    //New record
    if (MilitaryEducationAcademyId == 0)
    {
        document.getElementById("lblAddEditMilitaryEducationAcademyTitle").innerHTML = "Въвеждане на ново военно образование";

        if (document.getElementById("ddPersonMilitaryEducationAcademyMilitaryAcademy") != undefined)
            document.getElementById("ddPersonMilitaryEducationAcademyMilitaryAcademy").value = optionChooseOneValue;

        if (document.getElementById("ddPersonMilitaryEducationAcademyMilitarySubject") != undefined)
            document.getElementById("ddPersonMilitaryEducationAcademyMilitarySubject").value = optionChooseOneValue;

        if (document.getElementById("ddPersonMilitaryEducationAcademyCountry") != undefined)
            document.getElementById("ddPersonMilitaryEducationAcademyCountry").value = optionChooseOneValue;

        if (document.getElementById("ddPersonMilitaryEducationAcademyLearningMethod") != undefined)
            document.getElementById("ddPersonMilitaryEducationAcademyLearningMethod").value = optionChooseOneValue;

        if (document.getElementById("txtMilitaryEducationAcademyGraduateYear") != undefined)
            document.getElementById("txtMilitaryEducationAcademyGraduateYear").value = "";

        if (document.getElementById("txtMilitaryEducationAcademyDurationYear") != undefined)
            document.getElementById("txtMilitaryEducationAcademyDurationYear").value = "";
        
        if (document.getElementById("ddPersonMilitaryEducationAcademyVitoshaMilitaryReportSpecialityType") != undefined) {
            document.getElementById("ddPersonMilitaryEducationAcademyVitoshaMilitaryReportSpecialityType").value = optionChooseOneValue;
            ClearSelectList(document.getElementById("ddPersonMilitaryEducationAcademyVitoshaMilitaryReportSpeciality"), true);
        }

        // clean message label in the light box and hide it
        document.getElementById("spanAddEditMilitaryEducationAcademyLightBox").style.display = "none";
        document.getElementById("spanAddEditMilitaryEducationAcademyLightBox").innerHTML = "";

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("lboxMilitaryEducationAcademy").style.display = "";
        CenterLightBox("lboxMilitaryEducationAcademy");
    }
    else //Edit Position
    {
        document.getElementById("lblAddEditMilitaryEducationAcademyTitle").innerHTML = "Редактиране на военно образование";

        var url = "AddEditReservist_EducationWork.aspx?AjaxMethod=JSLoadMilitaryEducationAcademy";

        var params = "MilitaryEducationAcademyId=" + MilitaryEducationAcademyId;

        function response_handler(xml)
        {
            var personMilitaryEducationAcademy = xml.getElementsByTagName("personMilitaryEducationAcademy")[0];

            var MilitaryAcademyCode = xmlValue(personMilitaryEducationAcademy, "MilitaryAcademyCode");
            var GstCode = xmlValue(personMilitaryEducationAcademy, "GstCode");
            var MilitarySubjectId = xmlValue(personMilitaryEducationAcademy, "MilitarySubjectId");
            var DurationYear = xmlValue(personMilitaryEducationAcademy, "DurationYear");
            var GraduateYear = xmlValue(personMilitaryEducationAcademy, "GraduateYear");
            var Country = xmlValue(personMilitaryEducationAcademy, "Country");
            var LearningMethodKey = xmlValue(personMilitaryEducationAcademy, "LearningMethodKey");

            var vitoshaMilitaryReportSpecialityTypeCode = xmlValue(personMilitaryEducationAcademy, "vitoshaMilitaryReportSpecialityTypeCode");
            var vitoshaMilitaryReportSpecialityCode = xmlValue(personMilitaryEducationAcademy, "vitoshaMilitaryReportSpecialityCode");


            if (document.getElementById("ddPersonMilitaryEducationAcademyMilitaryAcademy") != undefined)
                document.getElementById("ddPersonMilitaryEducationAcademyMilitaryAcademy").value = MilitaryAcademyCode;


            if (document.getElementById("chkboxGstCode") != undefined)
            {
                if (GstCode == "True")
                {
                    document.getElementById("chkboxGstCode").checked = true;
                }
                else
                {
                    document.getElementById("chkboxGstCode").checked = false;
                }
            }

            if (document.getElementById("ddPersonMilitaryEducationAcademyMilitarySubject") != undefined)
                document.getElementById("ddPersonMilitaryEducationAcademyMilitarySubject").value = MilitarySubjectId;

            if (document.getElementById("ddPersonMilitaryEducationAcademyCountry") != undefined)
                document.getElementById("ddPersonMilitaryEducationAcademyCountry").value = Country;

            if (document.getElementById("ddPersonMilitaryEducationAcademyLearningMethod") != undefined)
                document.getElementById("ddPersonMilitaryEducationAcademyLearningMethod").value = LearningMethodKey;

            if (document.getElementById("txtMilitaryEducationAcademyGraduateYear") != undefined)
                document.getElementById("txtMilitaryEducationAcademyGraduateYear").value = GraduateYear;

            if (document.getElementById("txtMilitaryEducationAcademyDurationYear") != undefined)
                document.getElementById("txtMilitaryEducationAcademyDurationYear").value = DurationYear;

            if (document.getElementById("ddPersonMilitaryEducationAcademyVitoshaMilitaryReportSpecialityType") != undefined) {
                document.getElementById("ddPersonMilitaryEducationAcademyVitoshaMilitaryReportSpecialityType").value = vitoshaMilitaryReportSpecialityTypeCode;

                ClearSelectList(document.getElementById("ddPersonMilitaryEducationAcademyVitoshaMilitaryReportSpeciality"), true);

                var milRepSpecs = xml.getElementsByTagName("vitoshaMilRepSpecOp");

                for (var i = 0; i < milRepSpecs.length; i++) {
                    var id = xmlValue(milRepSpecs[i], "code");
                    var name = xmlValue(milRepSpecs[i], "name");

                    AddToSelectList(document.getElementById("ddPersonMilitaryEducationAcademyVitoshaMilitaryReportSpeciality"), id, name, true);
                };

                document.getElementById("ddPersonMilitaryEducationAcademyVitoshaMilitaryReportSpeciality").value = vitoshaMilitaryReportSpecialityCode;
            }

            //            document.getElementById("ddPersonMilitaryEducationAcademyMilitarySubject").value = MilitarySubjectId;

            //            document.getElementById("txtMilitaryEducationAcademyDurationYear").value = DurationYear;
            //            document.getElementById("txtMilitaryEducationAcademyGraduateYear").value = GraduateYear;

            //            document.getElementById("ddPersonMilitaryEducationAcademyCountry").value = Country;

            //            document.getElementById("ddPersonMilitaryEducationAcademyLearningMethod").value = LearningMethodKey;

            // clean message label in the light box and hide it
            document.getElementById("spanAddEditMilitaryEducationAcademyLightBox").style.display = "none";
            document.getElementById("spanAddEditMilitaryEducationAcademyLightBox").innerHTML = "";

            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("lboxMilitaryEducationAcademy").style.display = "";
            CenterLightBox("lboxMilitaryEducationAcademy");
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Close the light-box
function HideAddEditMilitaryEducationAcademyLightBox()
{
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("lboxMilitaryEducationAcademy").style.display = "none";  
}

//Save Add/Edit MilitaryEducationAcademy
function SaveAddEditMilitaryEducationAcademyLightBox()
{
    if (ValidateAddEditMilitaryEducationAcademy())
    {
        var url = "AddEditReservist_EducationWork.aspx?AjaxMethod=JSSaveMilitaryEducationAcademy";

        var ddPersonMilitaryEducationAcademyMilitarySubject = "";
        if (document.getElementById("ddPersonMilitaryEducationAcademyMilitarySubject") != undefined)
        {
            if (document.getElementById("ddPersonMilitaryEducationAcademyMilitarySubject").value != optionChooseOneValue)
            {
                ddPersonMilitaryEducationAcademyMilitarySubject = document.getElementById("ddPersonMilitaryEducationAcademyMilitarySubject").value
            }
        }

        var params = "MilitaryEducationAcademyId=" + document.getElementById("hdnMilitaryEducationAcademyID").value +
                     "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value +
                     "&MilitaryAcademyCode=" + document.getElementById("ddPersonMilitaryEducationAcademyMilitaryAcademy").value +
                     "&MilitaryAcademySubjectCode=" + ddPersonMilitaryEducationAcademyMilitarySubject +
                     "&CountryCode=" + document.getElementById("ddPersonMilitaryEducationAcademyCountry").value +
                     "&LearningMethodKey=" + document.getElementById("ddPersonMilitaryEducationAcademyLearningMethod").value +
                     "&DurationYear=" + document.getElementById("txtMilitaryEducationAcademyDurationYear").value +
                     "&GraduateYear=" + document.getElementById("txtMilitaryEducationAcademyGraduateYear").value +
                     "&GstCode=" + document.getElementById("chkboxGstCode").checked +
                     "&VitoshaMilitaryReportSpecialityCode=" + document.getElementById("ddPersonMilitaryEducationAcademyVitoshaMilitaryReportSpeciality").value;
                    
        function response_handler(xml)
        {
            var status = xmlValue(xml, "status");

            if (status == "OK")
            {
                var refreshedPositionsTable = xmlValue(xml, "refreshedMilitaryEducationAcademyTable");

                document.getElementById("tblMilitaryEducationAcademy").innerHTML = refreshedPositionsTable;

                document.getElementById("lblMessageMilitaryEducationAcademy").className = "SuccessText";
                document.getElementById("lblMessageMilitaryEducationAcademy").innerHTML = document.getElementById("hdnMilitaryEducationAcademyID").value == "0" ? "Военното образование е добавено успешно" : "Военното образование е редактирано успешно";

                HideAddEditMilitaryEducationAcademyLightBox();
            }
            else
            {
                document.getElementById("spanAddEditMilitaryEducationAcademyLightBox").className = "ErrorText";
                document.getElementById("spanAddEditMilitaryEducationAcademyLightBox").innerHTML = status;
                document.getElementById("spanAddEditMilitaryEducationAcademyLightBox").style.display = "";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Client validation of Add/Edit MilitaryEducationAcademy light-box
function ValidateAddEditMilitaryEducationAcademy()
{

    // MilitarySubject - Not Mandatory
    var res = true;

    var lblMessage = document.getElementById("spanAddEditMilitaryEducationAcademyLightBox");
    lblMessage.innerHTML = "";

    var notValidFields = new Array();


    var ddPersonMilitaryEducationAcademyMilitaryAcademy = document.getElementById("ddPersonMilitaryEducationAcademyMilitaryAcademy");
    var chkboxGstCode = document.getElementById("chkboxGstCode");
    var ddPersonMilitaryEducationAcademyMilitarySubject = document.getElementById("ddPersonMilitaryEducationAcademyMilitarySubject");
    var ddPersonMilitaryEducationAcademyCountry = document.getElementById("ddPersonMilitaryEducationAcademyCountry");
    var ddPersonMilitaryEducationAcademyLearningMethod = document.getElementById("ddPersonMilitaryEducationAcademyLearningMethod");

    var txtMilitaryEducationAcademyDurationYear = document.getElementById("txtMilitaryEducationAcademyDurationYear");
    var txtMilitaryEducationAcademyGraduateYear = document.getElementById("txtMilitaryEducationAcademyGraduateYear");



    if (ddPersonMilitaryEducationAcademyMilitaryAcademy.value == optionChooseOneValue)
    {
        res = false;

        if (ddPersonMilitaryEducationAcademyMilitaryAcademy.disabled == true || ddPersonMilitaryEducationAcademyMilitaryAcademy.style.display == "none")
            notValidFields.push("Военна академия");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Военна академия") + "<br />";
    }


    if (txtMilitaryEducationAcademyDurationYear.value.Trim() == "")
    {
        res = false;
        if (txtMilitaryEducationAcademyDurationYear.disabled == true || txtMilitaryEducationAcademyDurationYear.style.display == "none")
            notValidFields.push("Продължителност (Години)");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Продължителност (Години)") + "<br />";
    }
    else
    {
        if (!isInt(txtMilitaryEducationAcademyDurationYear.value) || parseInt(txtMilitaryEducationAcademyDurationYear.value) <= 0)
        {
            res = false;
            lblMessage.innerHTML += GetErrorMessageNumber("Продължителност (Години)") + "<br />";
        }
    }



    if (txtMilitaryEducationAcademyGraduateYear.value.Trim() == "")
    {
        res = false;

        if (txtMilitaryEducationAcademyGraduateYear.disabled == true || txtMilitaryEducationAcademyGraduateYear.style.display == "none")
            notValidFields.push("Година на завършване");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Година на завършване") + "<br />";
    }
    else
    {
        if (!isInt(txtMilitaryEducationAcademyGraduateYear.value) || parseInt(txtMilitaryEducationAcademyGraduateYear.value) <= 0)
        {
            res = false;
            lblMessage.innerHTML += GetErrorMessageNumber("Година на завършване") + "<br />";
        }
    }



    if (ddPersonMilitaryEducationAcademyCountry.value == optionChooseOneValue)
    {
        res = false;
        if (ddPersonMilitaryEducationAcademyCountry.disabled == true || ddPersonMilitaryEducationAcademyCountry.style.display == "none")
            notValidFields.push("Държава");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Държава") + "<br />";
    }


    if (ddPersonMilitaryEducationAcademyLearningMethod.value == optionChooseOneValue)
    {
        res = false;
        if (ddPersonMilitaryEducationAcademyLearningMethod.disabled == true || ddPersonMilitaryEducationAcademyLearningMethod.style.display == "none")
            notValidFields.push("Начин на обучение");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Начин на обучение") + "<br />";
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

// Delete a particular MilitaryEducationAcademy record
function DeleteMilitaryEducationAcademy(MilitaryEducationAcademyId)
{
    ClearAllMessages();

    YesNoDialog("Желаете ли да изтриете военното образование?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "AddEditReservist_EducationWork.aspx?AjaxMethod=JSDeleteMilitaryEducationAcademy";

        var params = "MilitaryEducationAcademyId=" + MilitaryEducationAcademyId;
        params += "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

        function response_handler(xml)
        {
            var status = xmlValue(xml, "status")

            if (status == "OK")
            {
                var refreshedPositionsTable = xmlValue(xml, "refreshedMilitaryEducationAcademyTable");

                document.getElementById("tblMilitaryEducationAcademy").innerHTML = refreshedPositionsTable;

                document.getElementById("lblMessageMilitaryEducationAcademy").className = "SuccessText";
                document.getElementById("lblMessageMilitaryEducationAcademy").innerHTML = "Военното образование е изтрито успешно";

            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}



//-----------------     3. Table TrainigCource   ------------------

var selectedTrainingCourceItemId = "";


function lnkTrainigCource_Click()
{
    LoadTrainigCources();
}

function LoadTrainigCources()
{
    //If already loaded then do not load
    if (document.getElementById("hdnTrainigCourceLoaded").value == "1")
        return;

    ClearAllMessages();

    document.getElementById("imgLoadingTrainigCource").style.visibility = "";

    var url = "AddEditReservist_EducationWork.aspx?AjaxMethod=JSLoadTrainigCources";
    var params = "";
    params += "ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

    var hdnIsPreview = document.getElementById(hdnIsPreviewClientID).value;
    if (hdnIsPreview == 1)
        params += "&Preview=1";
    
    function LoadTrainigCource_CallBack(xml)
    {
        var tableHTML = xmlValue(xml, "tableHTML");
        var lightBoxHTML = xmlValue(xml, "lightBoxHTML");

        document.getElementById("tblTrainigCource").innerHTML = tableHTML;
        document.getElementById("lboxTrainigCource").innerHTML = lightBoxHTML;

        RefreshUIItems(xml);

        document.getElementById("imgLoadingTrainigCource").style.visibility = "hidden";

        //Mark as already loaded
        document.getElementById("hdnTrainigCourceLoaded").value = "1";
    }

    var myAJAX = new AJAX(url, true, params, LoadTrainigCource_CallBack);
    myAJAX.Call();
}


//Open the light-box for adding a new record in the TrainigCource table
function NewTrainigCource()
{
    ShowAddEditTrainigCourceLightBox(0);
}

//Open the light-box for editing a record in the TrainigCource table
function EditTrainigCource(TrainigCourceId)
{
    ShowAddEditTrainigCourceLightBox(TrainigCourceId);
}

function ShowAddEditTrainigCourceLightBox(TrainigCourceId)
{
    ClearAllMessages();

    document.getElementById("hdnTrainingCourceID").value = TrainigCourceId;

    //Set DateTime Pickers
    RefreshDatePickers();
    //Set max lenth to texarea
    if (document.getElementById("txtMilitaryTrainingCourceNameDescription") != undefined)
        SetClientTextAreaMaxLength("txtMilitaryTrainingCourceNameDescription", "300");


    //New record
    if (TrainigCourceId == 0)
    {
        document.getElementById("lblAddEditMilitaryTrainingCourceTitle").innerHTML = "Въвеждане на нов курс";


        if (document.getElementById("ddPersonMilitaryTrainingCourceMilitaryTrainingCources") != undefined)
            document.getElementById("ddPersonMilitaryTrainingCourceMilitaryTrainingCources").value = optionChooseOneValue;
        if (document.getElementById("ddPersonMilitaryTrainingCourceCountries") != undefined)
            document.getElementById("ddPersonMilitaryTrainingCourceCountries").value = optionChooseOneValue;
        if (document.getElementById("ddPersonMilitaryTrainingCourceMilitaryCommanderRanks") != undefined)
            document.getElementById("ddPersonMilitaryTrainingCourceMilitaryCommanderRanks").value = optionChooseOneValue;
        if (document.getElementById("ddPersonMilitaryTrainingCourceMilitaryLanguages") != undefined)
            document.getElementById("ddPersonMilitaryTrainingCourceMilitaryLanguages").value = optionChooseOneValue;
        if (document.getElementById("ddPersonMilitaryTrainingCourceMilitarySchools") != undefined)
            document.getElementById("ddPersonMilitaryTrainingCourceMilitarySchools").value = optionChooseOneValue;
        if (document.getElementById("txtMilitaryTrainingCourceDurationMonth") != undefined)
            document.getElementById("txtMilitaryTrainingCourceDurationMonth").value = "";
        if (document.getElementById("txtMilitaryTrainingCourceDurationDay") != undefined)
            document.getElementById("txtMilitaryTrainingCourceDurationDay").value = "";
        if (document.getElementById("txtMilitaryTrainingCourceLevel") != undefined)
            document.getElementById("txtMilitaryTrainingCourceLevel").value = "";
        if (document.getElementById("txtMilitaryTrainingCourceVacAnn") != undefined)
            document.getElementById("txtMilitaryTrainingCourceVacAnn").value = "";
        if (document.getElementById("txtMilitaryTrainingCourceDateWhen") != undefined)
            document.getElementById("txtMilitaryTrainingCourceDateWhen").value = "";
        if (document.getElementById("txtMilitaryTrainingCourceDateOfCource") != undefined)
            document.getElementById("txtMilitaryTrainingCourceDateOfCource").value = "";
        if (document.getElementById("txtMilitaryTrainingCourceNameDescription") != undefined)
            document.getElementById("txtMilitaryTrainingCourceNameDescription").value = "";

        if (document.getElementById("ddMilitaryTrainingCourceVitoshaMilitaryReportSpecialityType") != undefined) {
            document.getElementById("ddMilitaryTrainingCourceVitoshaMilitaryReportSpecialityType").value = optionChooseOneValue;
            ClearSelectList(document.getElementById("ddMilitaryTrainingCourceVitoshaMilitaryReportSpeciality"), true);
        }

        selectedTrainingCourceItemId = "";

        // clean message label in the light box and hide it
        document.getElementById("spanAddEditTrainigCourceLightBox").style.display = "none";
        document.getElementById("spanAddEditTrainigCourceLightBox").innerHTML = "";

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("lboxTrainigCource").style.display = "";
        CenterLightBox("lboxTrainigCource");
    }
    else //Edit Position
    {
        document.getElementById("lblAddEditMilitaryTrainingCourceTitle").innerHTML = "Редактиране на курс";

        var url = "AddEditReservist_EducationWork.aspx?AjaxMethod=JSLoadTrainigCource";

        var params = "TrainigCourceId=" + TrainigCourceId;

        function response_handler(xml)
        {
            var PersonMilitaryTrainingCource = xml.getElementsByTagName("PersonMilitaryTrainingCource")[0];

            var MilitaryTrainingCourceCode = xmlValue(PersonMilitaryTrainingCource, "MilitaryTrainingCourceCode");
            var MilitaryTrainingCourceDurationMonth = xmlValue(PersonMilitaryTrainingCource, "MilitaryTrainingCourceDurationMonth");
            var MilitaryTrainingCourceDurationDay = xmlValue(PersonMilitaryTrainingCource, "MilitaryTrainingCourceDurationDay");
            var MilitaryTrainingCourceLevel = xmlValue(PersonMilitaryTrainingCource, "MilitaryTrainingCourceLevel");
            var MilitaryTrainingCourceCountryCode = xmlValue(PersonMilitaryTrainingCource, "MilitaryTrainingCourceCountryCode");
            var MilitaryTrainingCourceVacAnn = xmlValue(PersonMilitaryTrainingCource, "MilitaryTrainingCourceVacAnn");
            var MilitaryTrainingCourceDateWhen = xmlValue(PersonMilitaryTrainingCource, "MilitaryTrainingCourceDateWhen");
            var MilitaryTrainingCommanderRankCode = xmlValue(PersonMilitaryTrainingCource, "MilitaryTrainingCommanderRankCode");
            var MilitaryTrainingCourceDateOfCource = xmlValue(PersonMilitaryTrainingCource, "MilitaryTrainingCourceDateOfCource");
            var MilitaryTrainingPersonLanguageCode = xmlValue(PersonMilitaryTrainingCource, "MilitaryTrainingPersonLanguageCode");
            var MilitaryTrainingMilitarySchoolId = xmlValue(PersonMilitaryTrainingCource, "MilitaryTrainingMilitarySchooCode");
            var MilitaryTrainingCourceNameDescription = xmlValue(PersonMilitaryTrainingCource, "MilitaryTrainingCourceNameDescription");

            var vitoshaMilitaryReportSpecialityTypeCode = xmlValue(PersonMilitaryTrainingCource, "vitoshaMilitaryReportSpecialityTypeCode");
            var vitoshaMilitaryReportSpecialityCode = xmlValue(PersonMilitaryTrainingCource, "vitoshaMilitaryReportSpecialityCode");

            if (document.getElementById("ddPersonMilitaryTrainingCourceMilitaryTrainingCources") != undefined)
                document.getElementById("ddPersonMilitaryTrainingCourceMilitaryTrainingCources").value = MilitaryTrainingCourceCode;
            if (document.getElementById("ddPersonMilitaryTrainingCourceCountries") != undefined)
                document.getElementById("ddPersonMilitaryTrainingCourceCountries").value = MilitaryTrainingCourceCountryCode;
            if (document.getElementById("ddPersonMilitaryTrainingCourceMilitaryCommanderRanks") != undefined)
                document.getElementById("ddPersonMilitaryTrainingCourceMilitaryCommanderRanks").value = MilitaryTrainingCommanderRankCode;
            if (document.getElementById("ddPersonMilitaryTrainingCourceMilitaryLanguages") != undefined)
                document.getElementById("ddPersonMilitaryTrainingCourceMilitaryLanguages").value = MilitaryTrainingPersonLanguageCode;
            if (document.getElementById("ddPersonMilitaryTrainingCourceMilitarySchools") != undefined)
                document.getElementById("ddPersonMilitaryTrainingCourceMilitarySchools").value = MilitaryTrainingMilitarySchoolId;
            if (document.getElementById("txtMilitaryTrainingCourceDurationMonth") != undefined)
                document.getElementById("txtMilitaryTrainingCourceDurationMonth").value = MilitaryTrainingCourceDurationMonth;
            if (document.getElementById("txtMilitaryTrainingCourceDurationDay") != undefined)
                document.getElementById("txtMilitaryTrainingCourceDurationDay").value = MilitaryTrainingCourceDurationDay;
            if (document.getElementById("txtMilitaryTrainingCourceLevel") != undefined)
                document.getElementById("txtMilitaryTrainingCourceLevel").value = MilitaryTrainingCourceLevel;
            if (document.getElementById("txtMilitaryTrainingCourceVacAnn") != undefined)
                document.getElementById("txtMilitaryTrainingCourceVacAnn").value = MilitaryTrainingCourceVacAnn;
            if (document.getElementById("txtMilitaryTrainingCourceDateWhen") != undefined)
                document.getElementById("txtMilitaryTrainingCourceDateWhen").value = MilitaryTrainingCourceDateWhen;
            if (document.getElementById("txtMilitaryTrainingCourceDateOfCource") != undefined)
                document.getElementById("txtMilitaryTrainingCourceDateOfCource").value = MilitaryTrainingCourceDateOfCource;
            if (document.getElementById("txtMilitaryTrainingCourceNameDescription") != undefined)
                document.getElementById("txtMilitaryTrainingCourceNameDescription").value = MilitaryTrainingCourceNameDescription;

            if (document.getElementById("ddMilitaryTrainingCourceVitoshaMilitaryReportSpecialityType") != undefined) {
                document.getElementById("ddMilitaryTrainingCourceVitoshaMilitaryReportSpecialityType").value = vitoshaMilitaryReportSpecialityTypeCode;

                ClearSelectList(document.getElementById("ddMilitaryTrainingCourceVitoshaMilitaryReportSpeciality"), true);

                var milRepSpecs = xml.getElementsByTagName("vitoshaMilRepSpecOp");
                
                for (var i = 0; i < milRepSpecs.length; i++) {
                    var id = xmlValue(milRepSpecs[i], "code");
                    var name = xmlValue(milRepSpecs[i], "name");

                    AddToSelectList(document.getElementById("ddMilitaryTrainingCourceVitoshaMilitaryReportSpeciality"), id, name, true);
                };

                document.getElementById("ddMilitaryTrainingCourceVitoshaMilitaryReportSpeciality").value = vitoshaMilitaryReportSpecialityCode;
            }

            //Chek for disable/enable Language DDl
            ddTrainingCourceChange(document.getElementById("ddPersonMilitaryTrainingCourceMilitaryTrainingCources"));

            // clean message label in the light box and hide it
            document.getElementById("spanAddEditTrainigCourceLightBox").style.display = "none";
            document.getElementById("spanAddEditTrainigCourceLightBox").innerHTML = "";

            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("lboxTrainigCource").style.display = "";
            CenterLightBox("lboxTrainigCource");
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Close the light-box
function HideAddEditMilitaryTrainingCourceLightBox()
{
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("lboxTrainigCource").style.display = "none";
}

//Save Add/Edit TrainigCource
function SaveAddEditMilitaryTrainingCourceLightBox()
{
    if (ValidateAddEditMilitaryTrainingCource())
    {
        var url = "AddEditReservist_EducationWork.aspx?AjaxMethod=JSSaveTrainigCource";

        var MilitaryCommanderRanksCode = "-1";
        if (document.getElementById("ddPersonMilitaryTrainingCourceMilitaryCommanderRanks") != undefined)
        {
            if (document.getElementById("ddPersonMilitaryTrainingCourceMilitaryCommanderRanks").disabled == false)
                MilitaryCommanderRanksCode = document.getElementById("ddPersonMilitaryTrainingCourceMilitaryCommanderRanks").value;
        }

        var LanguageCode = "-1";
        if (document.getElementById("ddPersonMilitaryTrainingCourceMilitaryLanguages") != undefined)
        {
            if (document.getElementById("ddPersonMilitaryTrainingCourceMilitaryLanguages").disabled == false)
                LanguageCode = document.getElementById("ddPersonMilitaryTrainingCourceMilitaryLanguages").value;
        }

        var MilitarySchoolId = "-1";
        if (document.getElementById("ddPersonMilitaryTrainingCourceMilitarySchools") != undefined)
        {
            if (document.getElementById("ddPersonMilitaryTrainingCourceMilitarySchools").disabled == false)
                MilitarySchoolId = document.getElementById("ddPersonMilitaryTrainingCourceMilitarySchools").value;
        }


        var CourceDurationDay = "";
        if (document.getElementById("txtMilitaryTrainingCourceDurationDay") != undefined)
            CourceDurationDay = document.getElementById("txtMilitaryTrainingCourceDurationDay").value;

        var CourceDurationMonth = "";
        if (document.getElementById("txtMilitaryTrainingCourceDurationMonth") != undefined)
            CourceDurationMonth = document.getElementById("txtMilitaryTrainingCourceDurationMonth").value;

        var CourceLevel = "";
        if (document.getElementById("txtMilitaryTrainingCourceLevel") != undefined)
            CourceLevel = document.getElementById("txtMilitaryTrainingCourceLevel").value;

        var CourceDateOfCource = "";
        if (document.getElementById("txtMilitaryTrainingCourceDateOfCource") != undefined)
            CourceDateOfCource = document.getElementById("txtMilitaryTrainingCourceDateOfCource").value;


        var CourceVacAnn = "";
        if (document.getElementById("txtMilitaryTrainingCourceVacAnn") != undefined)
            CourceVacAnn = document.getElementById("txtMilitaryTrainingCourceVacAnn").value;


        var CourceNameDescription = "";
        if (document.getElementById("txtMilitaryTrainingCourceNameDescription") != undefined)
            CourceNameDescription = document.getElementById("txtMilitaryTrainingCourceNameDescription").value;


        var vitoshaMilitaryReportSpecialityCode = "";
        if (document.getElementById("ddMilitaryTrainingCourceVitoshaMilitaryReportSpeciality") != undefined)
            vitoshaMilitaryReportSpecialityCode = document.getElementById("ddMilitaryTrainingCourceVitoshaMilitaryReportSpeciality").value;

        var params = "TrainigCourceId=" + document.getElementById("hdnTrainingCourceID").value +
                     "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value +
                     "&MilitaryCourceCode=" + document.getElementById("ddPersonMilitaryTrainingCourceMilitaryTrainingCources").value +
                     "&MilitaryCountryCode=" + document.getElementById("ddPersonMilitaryTrainingCourceCountries").value +
                     "&MilitaryCommanderRanksCode=" + MilitaryCommanderRanksCode +
                     "&MilitaryLanguageCode=" + LanguageCode +
                     "&MilitarySchoolId=" + MilitarySchoolId +
                     "&CourceDurationMonth=" + CourceDurationMonth +
                     "&CourceDurationDay=" + CourceDurationDay +
                     "&CourceLevel=" + CourceLevel +
                     "&CourceVacAnn=" + CourceVacAnn +
                     "&CourceDateWhen=" + document.getElementById("txtMilitaryTrainingCourceDateWhen").value +
                     "&CourceDateOfCource=" + CourceDateOfCource +
                     "&CourceNameDescription=" + CourceNameDescription +
                     "&VitoshaMilitaryReportSpecialityCode=" + vitoshaMilitaryReportSpecialityCode;

        function response_handler(xml)
        {
            var status = xmlValue(xml, "status");

            if (status == "OK")
            {
                var refreshedPositionsTable = xmlValue(xml, "refreshedMilitaryTrainingCource");

                document.getElementById("tblTrainigCource").innerHTML = refreshedPositionsTable;

                document.getElementById("lblMessageTrainigCource").className = "SuccessText";
                document.getElementById("lblMessageTrainigCource").innerHTML = document.getElementById("hdnTrainingCourceID").value == "0" ? "Курса е добавен успешно" : "Курса е редактиран успешно";

                HideAddEditMilitaryTrainingCourceLightBox();
            }
            else
            {
                document.getElementById("spanAddEditTrainigCourceLightBox").className = "ErrorText";
                document.getElementById("spanAddEditTrainigCourceLightBox").innerHTML = status;
                document.getElementById("spanAddEditTrainigCourceLightBox").style.display = "";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Client validation of AddEditMilitaryTrainingCourc light-box
function ValidateAddEditMilitaryTrainingCource()
{

    var res = true;

    var lblMessage = document.getElementById("spanAddEditTrainigCourceLightBox");
    lblMessage.innerHTML = "";

    var notValidFields = new Array();

    //Mandatory Filed to validate according from DB table

    var ddPersonMilitaryTrainingCourceMilitaryTrainingCources = document.getElementById("ddPersonMilitaryTrainingCourceMilitaryTrainingCources");
    var ddPersonMilitaryTrainingCourceCountries = document.getElementById("ddPersonMilitaryTrainingCourceCountries");
    var txtMilitaryTrainingCourceDateWhen = document.getElementById("txtMilitaryTrainingCourceDateWhen");


    if (ddPersonMilitaryTrainingCourceMilitaryTrainingCources.value == optionChooseOneValue)
    {
        res = false;
        if (ddPersonMilitaryTrainingCourceMilitaryTrainingCources.disabled == true || ddPersonMilitaryTrainingCourceMilitaryTrainingCources.style.display == "none")
            notValidFields.push("Курс");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Курс") + "<br />";
    }



    if (ddPersonMilitaryTrainingCourceCountries.value == optionChooseOneValue)
    {
        res = false;

        if (ddPersonMilitaryTrainingCourceCountries.disabled == true || ddPersonMilitaryTrainingCourceCountries.style.display == "none")
            notValidFields.push("Държава");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Държава") + "<br />";
    }



    if (txtMilitaryTrainingCourceDateWhen.value.Trim() == "")
    {
        res = false;
        if (txtMilitaryTrainingCourceDateWhen.disabled == true || txtMilitaryTrainingCourceDateWhen.style.display == "none" ||
            document.getElementById("txtMilitaryTrainingCourceDateWhenCont").style.display == "none")
            notValidFields.push("Дата на заповедта");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Дата на заповедта") + "<br />";
    }
    else
    {
        if (!IsValidDate(txtMilitaryTrainingCourceDateWhen.value))
        {
            res = false;
            lblMessage.innerHTML += GetErrorMessageDate("Дата на заповедта") + "<br />";
        }
    }
    

    var txtMilitaryTrainingCourceDurationMonth = document.getElementById("txtMilitaryTrainingCourceDurationMonth");
    var txtMilitaryTrainingCourceLevel = document.getElementById("txtMilitaryTrainingCourceLevel");
    var ddPersonMilitaryTrainingCourceMilitaryLanguages = document.getElementById("ddPersonMilitaryTrainingCourceMilitaryLanguages");


    if (selectedTrainingCourceItemId == "7") //These Fields are set to Mandatory
    {

        if (txtMilitaryTrainingCourceDurationMonth.value.Trim() == "")
        {
            res = false;
            if (txtMilitaryTrainingCourceDurationMonth.disabled == true || txtMilitaryTrainingCourceDurationMonth.style.display == "none")
                notValidFields.push("Продължителност (месеци)");
            else
                lblMessage.innerHTML += GetErrorMessageMandatory("Продължителност (месеци)") + "<br />";
        }
        else
        {
            if (!isInt(txtMilitaryTrainingCourceDurationMonth.value) || parseInt(txtMilitaryTrainingCourceDurationMonth.value) < 0)
            {
                res = false;
                lblMessage.innerHTML += GetErrorMessageNumber("Продължителност (месеци)") + "<br />";
            }
        }

        //2. KursLevel
        if (txtMilitaryTrainingCourceLevel.value.Trim() == "")
        {
            res = false;
            if (txtMilitaryTrainingCourceLevel.disabled == true || txtMilitaryTrainingCourceLevel.style.display == "none")
                notValidFields.push("Ниво");
            else
                lblMessage.innerHTML += GetErrorMessageMandatory("Ниво") + "<br />";
        }
        else
        {
            if (!isInt(txtMilitaryTrainingCourceLevel.value) || parseInt(txtMilitaryTrainingCourceLevel.value) < 0)
            {
                res = false;
                lblMessage.innerHTML += GetErrorMessageNumber("Ниво") + "<br />";
            }
        }

        // 3. Language
        if (ddPersonMilitaryTrainingCourceMilitaryLanguages.value == optionChooseOneValue)
        {
            res = false;

            if (ddPersonMilitaryTrainingCourceMilitaryLanguages.disabled == true || ddPersonMilitaryTrainingCourceMilitaryLanguages.style.display == "none")
                notValidFields.push("Чужд език");
            else
                lblMessage.innerHTML += GetErrorMessageMandatory("Чужд език") + "<br />";
        }
    }
    else
    {
        //accept value 0
        if (txtMilitaryTrainingCourceDurationMonth.value.Trim() != "")
        {
            if (!isInt(txtMilitaryTrainingCourceDurationMonth.value) || parseInt(txtMilitaryTrainingCourceDurationMonth.value) < 0)
            {
                res = false;
                lblMessage.innerHTML += GetErrorMessageNumber("Продължителност (месеци)") + "<br />";
            }
        }
        
        //accept value 0
        if (txtMilitaryTrainingCourceLevel.value.Trim() != "")
        {
            if (!isInt(txtMilitaryTrainingCourceLevel.value) || parseInt(txtMilitaryTrainingCourceLevel.value) < 0)
            {
                res = false;
                lblMessage.innerHTML += GetErrorMessageNumber("Ниво") + "<br />";
            }
        }
    }

    //Validate other fields

    var txtMilitaryTrainingCourceDurationDay = document.getElementById("txtMilitaryTrainingCourceDurationDay");
    var txtMilitaryTrainingCourceDateOfCource = document.getElementById("txtMilitaryTrainingCourceDateOfCource");

    if (txtMilitaryTrainingCourceDurationDay.value.Trim() != "")
    {
        if (!isInt(txtMilitaryTrainingCourceDurationDay.value) || parseInt(txtMilitaryTrainingCourceDurationDay.value) < 0)
        {
            res = false;
            lblMessage.innerHTML += GetErrorMessageNumber("Продължителност (дни)") + "<br />";
        }
    }


    if (txtMilitaryTrainingCourceDateOfCource.value.Trim() != "")
    {
        if (!IsValidDate(txtMilitaryTrainingCourceDateOfCource.value))
        {
            res = false;
            lblMessage.innerHTML += GetErrorMessageDate("Влиза в сила от") + "<br />";
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

// Delete a particular TrainigCource record
function DeleteTrainigCource(TrainigCourceId)
{
    ClearAllMessages();

    YesNoDialog("Желаете ли да изтриете курса?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "AddEditReservist_EducationWork.aspx?AjaxMethod=JSDeleteTrainigCource";

        var params = "TrainigCourceId=" + TrainigCourceId;
        params += "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

        function response_handler(xml)
        {
            var status = xmlValue(xml, "status")

            if (status == "OK")
            {
                var refreshedMilitaryTrainingCourceTable = xmlValue(xml, "refreshedMilitaryTrainingCourceTable");

                document.getElementById("tblTrainigCource").innerHTML = refreshedMilitaryTrainingCourceTable;

                document.getElementById("lblMessageTrainigCource").className = "SuccessText";
                document.getElementById("lblMessageTrainigCource").innerHTML = "Курса е изтрит успешно";

            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Use this to enable/disable DDList fo Languages - deppends from Check Constraint
function ddTrainingCourceChange(ddlObject)
{
    var ddPersonMilitaryTrainingCourceMilitaryLanguages = document.getElementById("ddPersonMilitaryTrainingCourceMilitaryLanguages");
    if (ddlObject == undefined)
    {
        if (ddPersonMilitaryTrainingCourceMilitaryLanguages != undefined)
        {
            ddPersonMilitaryTrainingCourceMilitaryLanguages.disabled = true;
        }
        return;
    }
    var cheked = false

    var i = 0;
    do
    {
        if (ddlObject[i].selected)
        {
            cheked = true;
            selectedTrainingCourceItemId = ddlObject[i].value

        }
        i++;

    } while (!cheked)

    if (selectedTrainingCourceItemId == "7")
    {
        //enable DDList for Languages
        if (ddPersonMilitaryTrainingCourceMilitaryLanguages != undefined)
            ddPersonMilitaryTrainingCourceMilitaryLanguages.disabled = false;
    }
    else
    {
        //Disable DDList for Languages
        if (ddPersonMilitaryTrainingCourceMilitaryLanguages != undefined)
            ddPersonMilitaryTrainingCourceMilitaryLanguages.disabled = true;
    }
}




// 4.----------- Table ForeignLanguage -------------------

//Load the ForeignLanguage table on demand
function lnkForeignLanguage_Click()
{
    LoadForeignLanguages();
}

function LoadForeignLanguages()
{
    //If already loaded then do not load
    if (document.getElementById("hdnForeignLanguageLoaded").value == "1")
        return;

    ClearAllMessages();

    document.getElementById("imgLoadingForeignLanguage").style.visibility = "";

    var url = "AddEditReservist_EducationWork.aspx?AjaxMethod=JSLoadForeignLanguages";
    var params = "";
    params += "ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

    var hdnIsPreview = document.getElementById(hdnIsPreviewClientID).value;
    if (hdnIsPreview == 1)
        params += "&Preview=1";
    
    function LoadForeignLanguage_CallBack(xml)
    {
        var tableHTML = xmlValue(xml, "tableHTML");
        var lightBoxHTML = xmlValue(xml, "lightBoxHTML");

        document.getElementById("tblForeignLanguage").innerHTML = tableHTML;
        document.getElementById("lboxForeignLanguage").innerHTML = lightBoxHTML;

        RefreshUIItems(xml);

        document.getElementById("imgLoadingForeignLanguage").style.visibility = "hidden";

        //Mark as already loaded
        document.getElementById("hdnForeignLanguageLoaded").value = "1";
    }

    var myAJAX = new AJAX(url, true, params, LoadForeignLanguage_CallBack);
    myAJAX.Call();
}

//Open the light-box for adding a new record in the ForeignLanguage table
function NewForeignLanguage()
{
    ShowAddEditForeignLanguageLightBox(0);
}

//Open the light-box for editing a record in the ForeignLanguage table
function EditForeignLanguage(ForeignLanguageId)
{
    ShowAddEditForeignLanguageLightBox(ForeignLanguageId);
}

function ShowAddEditForeignLanguageLightBox(ForeignLanguageId)
{
    ClearAllMessages();

    //Set DateTime Pickers
    RefreshDatePickers();

    document.getElementById("hdnForeignLanguageID").value = ForeignLanguageId;

    //New record
    if (ForeignLanguageId == 0)
    {
        document.getElementById("lblAddEditForeignLanguageTitle").innerHTML = "Въвеждане на нов чужд език";

        if (document.getElementById("ddPersonLangEduForeignLanguagePersonLanguage") != undefined)
        {
            document.getElementById("ddPersonLangEduForeignLanguagePersonLanguage").value = optionChooseOneValue;
        }

        if (document.getElementById("ddPersonLangEduForeignLanguageLanguageLevel") != undefined)
        {
            document.getElementById("ddPersonLangEduForeignLanguageLanguageLevel").value = optionChooseOneValue;
        }
        //        document.getElementById("txtPersonLangEduForeignLanguageSTANAG").value = "";
        document.getElementById("ddPersonLangEduForeignLanguageLanguageForm").value = optionChooseOneValue;
        document.getElementById("ddPersonLangEduForeignLanguageLanguageDiploma").value = optionChooseOneValue;
        document.getElementById("txtPersonLangEduForeignLanguageLanguageVacAnn").value = "";
        document.getElementById("txtPersonLangEduForeignLanguageLanguageDateWhen").value = "";

        // clean message label in the light box and hide it
        document.getElementById("spanAddEditForeignLanguageLightBox").style.display = "none";
        document.getElementById("spanAddEditForeignLanguageLightBox").innerHTML = "";

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("lboxForeignLanguage").style.display = "";
        CenterLightBox("lboxForeignLanguage");
    }
    else //Edit Position
    {
        document.getElementById("lblAddEditForeignLanguageTitle").innerHTML = "Редактиране на чужд език";

        var url = "AddEditReservist_EducationWork.aspx?AjaxMethod=JSLoadForeignLanguage";

        var params = "ForeignLanguageId=" + ForeignLanguageId;

        function response_handler(xml)
        {
            var personForeignLanguage = xml.getElementsByTagName("PersonForeignLanguage")[0];

            var LanguageCode = xmlValue(personForeignLanguage, "LanguageCode");
            var LanguageLevelOfKnowledgeKey = xmlValue(personForeignLanguage, "LanguageLevelOfKnowledgeKey");
            //            var LanguageStanAg = xmlValue(personForeignLanguage, "LanguageStanAg");
            var LanguageFormOfKnowledgeKey = xmlValue(personForeignLanguage, "LanguageFormOfKnowledgeKey");
            var LanguageDiplomaKey = xmlValue(personForeignLanguage, "LanguageDiplomaKey");
            var LanguageVacAnn = xmlValue(personForeignLanguage, "LanguageVacAnn");
            var LanguageDateWhen = xmlValue(personForeignLanguage, "LanguageDateWhen");

            if (document.getElementById("ddPersonLangEduForeignLanguagePersonLanguage") != undefined)
            {
                document.getElementById("ddPersonLangEduForeignLanguagePersonLanguage").value = LanguageCode;
            }

            if (document.getElementById("ddPersonLangEduForeignLanguageLanguageLevel") != undefined)
            {
                document.getElementById("ddPersonLangEduForeignLanguageLanguageLevel").value = LanguageLevelOfKnowledgeKey;

            }
            //            document.getElementById("txtPersonLangEduForeignLanguageSTANAG").value = LanguageStanAg;

            if (document.getElementById("ddPersonLangEduForeignLanguageLanguageForm") != undefined)
            {
                document.getElementById("ddPersonLangEduForeignLanguageLanguageForm").value = LanguageFormOfKnowledgeKey;
            }


            if (document.getElementById("ddPersonLangEduForeignLanguageLanguageDiploma") != undefined)
            {
                document.getElementById("ddPersonLangEduForeignLanguageLanguageDiploma").value = LanguageDiplomaKey;
            }

            if (document.getElementById("txtPersonLangEduForeignLanguageLanguageVacAnn") != undefined)
            {
                document.getElementById("txtPersonLangEduForeignLanguageLanguageVacAnn").value = LanguageVacAnn;
            }

            if (document.getElementById("txtPersonLangEduForeignLanguageLanguageDateWhen") != undefined)
            {
                document.getElementById("txtPersonLangEduForeignLanguageLanguageDateWhen").value = LanguageDateWhen;

            }

            // clean message label in the light box and hide it
            document.getElementById("spanAddEditForeignLanguageLightBox").style.display = "none";
            document.getElementById("spanAddEditForeignLanguageLightBox").innerHTML = "";

            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("lboxForeignLanguage").style.display = "";
            CenterLightBox("lboxForeignLanguage");
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Close the light-box
function HideAddEditForeignLanguageLightBox()
{
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("lboxForeignLanguage").style.display = "none";
}

//Save Add/Edit ForeignLanguage
function SaveAddEditForeignLanguageLightBox()
{
    if (ValidateAddEditForeignLanguage())
    {
        var url = "AddEditReservist_EducationWork.aspx?AjaxMethod=JSSaveForeignLanguage";

        var vacAnn = "";
        var dateWhen = "";
        if (document.getElementById("txtPersonLangEduForeignLanguageLanguageVacAnn") != undefined)
        {
            vacAnn = document.getElementById("txtPersonLangEduForeignLanguageLanguageVacAnn").value;
        }

        if (document.getElementById("txtPersonLangEduForeignLanguageLanguageDateWhen") != undefined)
        {
            dateWhen = document.getElementById("txtPersonLangEduForeignLanguageLanguageDateWhen").value;
        }


        var params = "ForeignLanguageId=" + document.getElementById("hdnForeignLanguageID").value +
                     "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value +
                     "&LanguageCode=" + document.getElementById("ddPersonLangEduForeignLanguagePersonLanguage").value +
                     "&LanguageLevelOfKnowledgeKey=" + document.getElementById("ddPersonLangEduForeignLanguageLanguageLevel").value +
        //                     "&LanguageStanAg=" + document.getElementById("txtPersonLangEduForeignLanguageSTANAG").value +
                     "&LanguageFormOfKnowledgeKey=" + document.getElementById("ddPersonLangEduForeignLanguageLanguageForm").value +
                     "&LanguageDiplomaKey=" + document.getElementById("ddPersonLangEduForeignLanguageLanguageDiploma").value +
                     "&LanguageVacAnn=" + vacAnn +
                     "&LanguageDateWhen=" + dateWhen;

        function response_handler(xml)
        {
            var status = xmlValue(xml, "status");

            if (status == "OK")
            {
                var refreshedPositionsTable = xmlValue(xml, "refreshedForeignLanguageTable");

                document.getElementById("tblForeignLanguage").innerHTML = refreshedPositionsTable;

                document.getElementById("lblMessageForeignLanguage").className = "SuccessText";
                document.getElementById("lblMessageForeignLanguage").innerHTML = document.getElementById("hdnForeignLanguageID").value == "0" ? "Езика е добавено успешно" : "Езика е редактиран успешно";

                HideAddEditForeignLanguageLightBox();
            }
            else
            {
                document.getElementById("spanAddEditForeignLanguageLightBox").className = "ErrorText";
                document.getElementById("spanAddEditForeignLanguageLightBox").innerHTML = status;
                document.getElementById("spanAddEditForeignLanguageLightBox").style.display = "";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Client validation of Add/Edit Civil Education light-box
function ValidateAddEditForeignLanguage()
{
    var res = true;

    var lblMessage = document.getElementById("spanAddEditForeignLanguageLightBox");
    lblMessage.innerHTML = "";

    var notValidFields = new Array();

    var ddPersonLangEduForeignLanguagePersonLanguage = document.getElementById("ddPersonLangEduForeignLanguagePersonLanguage");
    var ddPersonLangEduForeignLanguageLanguageLevel = document.getElementById("ddPersonLangEduForeignLanguageLanguageLevel");
    //    var txtPersonLangEduForeignLanguageSTANAG = document.getElementById("txtPersonLangEduForeignLanguageSTANAG");
    var ddPersonLangEduForeignLanguageLanguageForm = document.getElementById("ddPersonLangEduForeignLanguageLanguageForm");
    var ddPersonLangEduForeignLanguageLanguageDiploma = document.getElementById("ddPersonLangEduForeignLanguageLanguageDiploma");
    var txtPersonLangEduForeignLanguageLanguageVacAnn = document.getElementById("txtPersonLangEduForeignLanguageLanguageVacAnn");
    var txtPersonLangEduForeignLanguageLanguageDateWhen = document.getElementById("txtPersonLangEduForeignLanguageLanguageDateWhen");

    if (ddPersonLangEduForeignLanguagePersonLanguage.value == optionChooseOneValue)
    {
        res = false;

        if (ddPersonLangEduForeignLanguagePersonLanguage.disabled == true || ddPersonLangEduForeignLanguagePersonLanguage.style.display == "none")
            notValidFields.push("Език");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Език") + "<br />";
    }


    if (ddPersonLangEduForeignLanguageLanguageLevel.value == optionChooseOneValue)
    {
        res = false;

        if (ddPersonLangEduForeignLanguageLanguageLevel.disabled == true || ddPersonLangEduForeignLanguageLanguageLevel.style.display == "none")
            notValidFields.push("Степен на владеене");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Степен на владеене") + "<br />";
    }


    if (ddPersonLangEduForeignLanguageLanguageForm.value == optionChooseOneValue)
    {
        res = false;

        if (ddPersonLangEduForeignLanguageLanguageForm.disabled == true || ddPersonLangEduForeignLanguageLanguageForm.style.display == "none")
            notValidFields.push("Форма на владеене");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Форма на владеене") + "<br />";
    }


    if (ddPersonLangEduForeignLanguageLanguageDiploma.value == optionChooseOneValue)
    {
        res = false;

        if (ddPersonLangEduForeignLanguageLanguageDiploma.disabled == true || ddPersonLangEduForeignLanguageLanguageDiploma.style.display == "none")
            notValidFields.push("Диплом");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Диплом") + "<br />";
    }

    if (txtPersonLangEduForeignLanguageLanguageDateWhen.value.Trim() != "")
    {
        if (!IsValidDate(txtPersonLangEduForeignLanguageLanguageDateWhen.value))
        {
            res = false;
            lblMessage.innerHTML += GetErrorMessageDate("Дата на документа") + "<br />";
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

// Delete a particular Civil Education record
function DeleteForeignLanguage(ForeignLanguageId)
{
    ClearAllMessages();

    YesNoDialog("Желаете ли да изтриете езика?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "AddEditReservist_EducationWork.aspx?AjaxMethod=JSDeleteForeignLanguage";

        var params = "ForeignLanguageId=" + ForeignLanguageId;
        params += "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

        function response_handler(xml)
        {
            var status = xmlValue(xml, "status")

            if (status == "OK")
            {
                var refreshedPositionsTable = xmlValue(xml, "refreshedForeignLanguageTable");

                document.getElementById("tblForeignLanguage").innerHTML = refreshedPositionsTable;

                document.getElementById("lblMessageForeignLanguage").className = "SuccessText";
                document.getElementById("lblMessageForeignLanguage").innerHTML = "Чуждия език е изтрит успешно";

            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}


// 5.----------- Table ScientificTitle -------------------

//Load the ScientificTitle table on demand
function lnkScientificTitle_Click()
{
    LoadScientificTitles();
}

function LoadScientificTitles()
{
    //If already loaded then do not load
    if (document.getElementById("hdnScientificTitleLoaded").value == "1")
        return;

    ClearAllMessages();

    document.getElementById("imgLoadingScientificTitle").style.visibility = "";

    var url = "AddEditReservist_EducationWork.aspx?AjaxMethod=JSLoadScientificTitles";
    var params = "";
    params += "ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

    var hdnIsPreview = document.getElementById(hdnIsPreviewClientID).value;
    if (hdnIsPreview == 1)
        params += "&Preview=1";
    
    function LoadScientificTitle_CallBack(xml)
    {
        var tableHTML = xmlValue(xml, "tableHTML");
        var lightBoxHTML = xmlValue(xml, "lightBoxHTML");

        document.getElementById("tblScientificTitle").innerHTML = tableHTML;
        document.getElementById("lboxScientificTitle").innerHTML = lightBoxHTML;

        RefreshUIItems(xml);

        document.getElementById("imgLoadingScientificTitle").style.visibility = "hidden";

        //Mark as already loaded
        document.getElementById("hdnScientificTitleLoaded").value = "1";
    }
    
    var myAJAX = new AJAX(url, true, params, LoadScientificTitle_CallBack);
    myAJAX.Call();
}

//Open the light-box for adding a new record in the ScientificTitle table
function NewScientificTitle()
{
    ShowAddEditScientificTitleLightBox(0);
}

//Open the light-box for editing a record in the ScientificTitle table
function EditScientificTitle(ScientificTitleId)
{
    ShowAddEditScientificTitleLightBox(ScientificTitleId);
}

function ShowAddEditScientificTitleLightBox(ScientificTitleId)
{
    ClearAllMessages();

    //Set max lenth to texarea
    if (document.getElementById("txtPersonScientificTitleScientificDesription") != undefined)
        SetClientTextAreaMaxLength("txtPersonScientificTitleScientificDesription", "500");

    document.getElementById("hdnScientificTitleID").value = ScientificTitleId;

    //New record
    if (ScientificTitleId == 0)
    {
        document.getElementById("lblAddEditScientificTitleTitle").innerHTML = "Въвеждане на ново научно звание";

        if (document.getElementById("ddPersonScientificTitleScientificTitle") != undefined)
            document.getElementById("ddPersonScientificTitleScientificTitle").value = optionChooseOneValue;
        if (document.getElementById("txtPersonScientificTitleScientificTitleYear") != undefined)
            document.getElementById("txtPersonScientificTitleScientificTitleYear").value = "";
        if (document.getElementById("txtPersonScientificTitleScientificNumberProtocol") != undefined)
            document.getElementById("txtPersonScientificTitleScientificNumberProtocol").value = "";
        if (document.getElementById("txtPersonScientificTitleScientificDesription") != undefined)
            document.getElementById("txtPersonScientificTitleScientificDesription").value = "";


        // clean message label in the light box and hide it
        document.getElementById("spanAddEditScientificTitleLightBox").style.display = "none";
        document.getElementById("spanAddEditScientificTitleLightBox").innerHTML = "";

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("lboxScientificTitle").style.display = "";
        CenterLightBox("lboxScientificTitle");
    }
    else //Edit Position
    {
        document.getElementById("lblAddEditScientificTitleTitle").innerHTML = "Редактиране на научно звание";

        var url = "AddEditReservist_EducationWork.aspx?AjaxMethod=JSLoadScientificTitle";

        var params = "ScientificTitleId=" + ScientificTitleId;

        function response_handler(xml)
        {
            var personScientificTitle = xml.getElementsByTagName("personScientificTitle")[0];

            var scientificTitleKey = xmlValue(personScientificTitle, "scientificTitleKey");
            var scientificTitleYear = xmlValue(personScientificTitle, "scientificTitleYear");
            var scientificTitleNumberProtocol = xmlValue(personScientificTitle, "scientificTitleNumberProtocol");
            var scientificTitleDesription = xmlValue(personScientificTitle, "scientificTitleDesription");

            if (document.getElementById("ddPersonScientificTitleScientificTitle") != undefined)
                document.getElementById("ddPersonScientificTitleScientificTitle").value = scientificTitleKey;
            if (document.getElementById("txtPersonScientificTitleScientificTitleYear") != undefined)
                document.getElementById("txtPersonScientificTitleScientificTitleYear").value = scientificTitleYear;
            if (document.getElementById("txtPersonScientificTitleScientificNumberProtocol") != undefined)
                document.getElementById("txtPersonScientificTitleScientificNumberProtocol").value = scientificTitleNumberProtocol;
            if (document.getElementById("txtPersonScientificTitleScientificDesription") != undefined)
                document.getElementById("txtPersonScientificTitleScientificDesription").value = scientificTitleDesription;


            //            document.getElementById("ddPersonScientificTitleScientificTitle").value = scientificTitleKey;
            //            document.getElementById("txtPersonScientificTitleScientificTitleYear").value = scientificTitleYear;
            //            document.getElementById("txtPersonScientificTitleScientificNumberProtocol").value = scientificTitleNumberProtocol;
            //            document.getElementById("txtPersonScientificTitleScientificDesription").value = scientificTitleDesription;

            // clean message label in the light box and hide it
            document.getElementById("spanAddEditScientificTitleLightBox").style.display = "none";
            document.getElementById("spanAddEditScientificTitleLightBox").innerHTML = "";

            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("lboxScientificTitle").style.display = "";
            CenterLightBox("lboxScientificTitle");
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Close the light-box
function HideAddEditScientificTitleLightBox()
{
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("lboxScientificTitle").style.display = "none";
}

//Save Add/Edit ScientificTitle
function SaveAddEditScientificTitleLightBox()
{
    if (ValidateAddEditScientificTitle())
    {
        var url = "AddEditReservist_EducationWork.aspx?AjaxMethod=JSSaveScientificTitle";

        var ScientificTitleDesription = "";
        if (document.getElementById("txtPersonScientificTitleScientificDesription") != undefined)
            ScientificTitleDesription = document.getElementById("txtPersonScientificTitleScientificDesription").value;

        var params = "ScientificTitleId=" + document.getElementById("hdnScientificTitleID").value +
                     "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value +
                     "&ScientificTitleKey=" + document.getElementById("ddPersonScientificTitleScientificTitle").value +
                     "&ScientificTitleYear=" + document.getElementById("txtPersonScientificTitleScientificTitleYear").value +
                     "&ScientificTitleNumberProtocol=" + document.getElementById("txtPersonScientificTitleScientificNumberProtocol").value +
                     "&ScientificTitleDesription=" + ScientificTitleDesription;

        function response_handler(xml)
        {
            var status = xmlValue(xml, "status");

            if (status == "OK")
            {
                var refreshedPositionsTable = xmlValue(xml, "refreshedScientificTitleTable");

                document.getElementById("tblScientificTitle").innerHTML = refreshedPositionsTable;

                document.getElementById("lblMessageScientificTitle").className = "SuccessText";
                document.getElementById("lblMessageScientificTitle").innerHTML = document.getElementById("hdnScientificTitleID").value == "0" ? "Научното звание е добавено успешно" : "Научното звание е редактирано успешно";

                HideAddEditScientificTitleLightBox();
            }
            else
            {
                document.getElementById("spanAddEditScientificTitleLightBox").className = "ErrorText";
                document.getElementById("spanAddEditScientificTitleLightBox").innerHTML = status;
                document.getElementById("spanAddEditScientificTitleLightBox").style.display = "";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Client validation of Add/Edit Civil Education light-box
function ValidateAddEditScientificTitle()
{
    var res = true;

    var lblMessage = document.getElementById("spanAddEditScientificTitleLightBox");
    lblMessage.innerHTML = "";

    var notValidFields = new Array();

    var ddPersonScientificTitleScientificTitle = document.getElementById("ddPersonScientificTitleScientificTitle");
    var txtPersonScientificTitleScientificTitleYear = document.getElementById("txtPersonScientificTitleScientificTitleYear");
    var txtPersonScientificTitleScientificNumberProtocol = document.getElementById("txtPersonScientificTitleScientificNumberProtocol");


    if (ddPersonScientificTitleScientificTitle.value == optionChooseOneValue)
    {
        res = false;

        if (ddPersonScientificTitleScientificTitle.disabled == true || ddPersonScientificTitleScientificTitle.style.display == "none")
            notValidFields.push("Научно звание");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Научно звание") + "<br />";
    }



    if (txtPersonScientificTitleScientificTitleYear.value.Trim() == "")
    {
        res = false;

        if (txtPersonScientificTitleScientificTitleYear.disabled == true || txtPersonScientificTitleScientificTitleYear.style.display == "none")
            notValidFields.push("Година");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Година") + "<br />";
    }
    else
    {
        if (!isInt(txtPersonScientificTitleScientificTitleYear.value) || parseInt(txtPersonScientificTitleScientificTitleYear.value) <= 0)
        {
            res = false;
            lblMessage.innerHTML += GetErrorMessageNumber("Година") + "<br />";
        }
    }



    if (txtPersonScientificTitleScientificNumberProtocol.value.Trim() == "")
    {
        res = false;

        if (txtPersonScientificTitleScientificNumberProtocol.disabled == true || txtPersonScientificTitleScientificNumberProtocol.style.display == "none")
            notValidFields.push("№ Протокол");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("№ Протокол") + "<br />";
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

// Delete a particular ScientificTitle record
function DeleteScientificTitle(ScientificTitleId)
{
    ClearAllMessages();

    YesNoDialog("Желаете ли да изтриете научното звание?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "AddEditReservist_EducationWork.aspx?AjaxMethod=JSDeleteScientificTitle";

        var params = "ScientificTitleId=" + ScientificTitleId;
        params += "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

        function response_handler(xml)
        {
            var status = xmlValue(xml, "status")

            if (status == "OK")
            {
                var refreshedPositionsTable = xmlValue(xml, "refreshedScientificTitleTable");

                document.getElementById("tblScientificTitle").innerHTML = refreshedPositionsTable;

                document.getElementById("lblMessageScientificTitle").className = "SuccessText";
                document.getElementById("lblMessageScientificTitle").innerHTML = "Гражданското образование е изтрито успешно";

            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

// 6.----------- Table Specialities -------------------
//Load the ScientificTitle table on demand
function lnkSpeciality_Click() {
    LoadSpeciality();
}

function LoadSpeciality() {
    //If already loaded then do not load
    if (document.getElementById("hdnSpecialityLoaded").value == "1")
        return;

    ClearAllMessages();

    document.getElementById("imgLoadingSpeciality").style.visibility = "";

    var url = "AddEditReservist_EducationWork.aspx?AjaxMethod=JSLoadSpecialities";
    var params = "";
    params += "ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

    var hdnIsPreview = document.getElementById(hdnIsPreviewClientID).value;
    if (hdnIsPreview == 1)
        params += "&Preview=1";

    function LoadSpeciality_CallBack(xml) {
        var tableHTML = xmlValue(xml, "tableHTML");
        var lightBoxHTML = xmlValue(xml, "lightBoxHTML");

        document.getElementById("tblSpeciality").innerHTML = tableHTML;
        document.getElementById("lboxSpeciality").innerHTML = lightBoxHTML;

        RefreshUIItems(xml);

        document.getElementById("imgLoadingSpeciality").style.visibility = "hidden";

        //Mark as already loaded
        document.getElementById("hdnSpecialityLoaded").value = "1";
    }

    var myAJAX = new AJAX(url, true, params, LoadSpeciality_CallBack);
    myAJAX.Call();
}

//Open the light-box for adding a new record in the ScientificTitle table
function NewSpeciality() {
    ShowAddEditSpecialityLightBox(0);
}

//Open the light-box for editing a record in the ScientificTitle table
function EditSpeciality(pPersonSpecialityID) {
    ShowAddEditSpecialityLightBox(pPersonSpecialityID);
}

function ShowAddEditSpecialityLightBox(pPersonSpecialityID) {
    ClearAllMessages();

    document.getElementById("hdnPersonSpecialityID").value = pPersonSpecialityID;

    //New record
    if (pPersonSpecialityID == 0) {
        document.getElementById("lblAddEditSpecialityTitle").innerHTML = "Въвеждане на нова специалност";

        if (document.getElementById("ddPersonSpecialityProfession") != undefined)
            document.getElementById("ddPersonSpecialityProfession").value = optionChooseOneValue;

        if (document.getElementById("ddPersonSpecialitySpeciality") != undefined)
            ClearSelectList(document.getElementById("ddPersonSpecialitySpeciality"), true);
    
        // clean message label in the light box and hide it
        document.getElementById("spanAddEditSpecialityLightBox").style.display = "none";
        document.getElementById("spanAddEditSpecialityLightBox").innerHTML = "";

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("lboxSpeciality").style.display = "";
        CenterLightBox("lboxSpeciality");
    }
    else //Edit Position
    {
        document.getElementById("lblAddEditSpecialityTitle").innerHTML = "Редактиране на специалност";

        var url = "AddEditReservist_EducationWork.aspx?AjaxMethod=JSLoadSpeciality";

        var params = "PersonSpecialityID=" + pPersonSpecialityID;

        function response_handler(xml) {
            var personSpeciality = xml.getElementsByTagName("personSpeciality")[0];

            var profession = xmlValue(personSpeciality, "profession");
            var speciality = xmlValue(personSpeciality, "speciality");

            if (document.getElementById("ddPersonSpecialityProfession") != undefined)
                document.getElementById("ddPersonSpecialityProfession").value = profession;

            ClearSelectList(document.getElementById("ddPersonSpecialitySpeciality"), true);

            var specialityOp = xml.getElementsByTagName("specialityOp");
           
            for (var i = 0; i < specialityOp.length; i++) {
                var id = xmlValue(specialityOp[i], "value");
                var name = xmlValue(specialityOp[i], "name");

                AddToSelectList(document.getElementById("ddPersonSpecialitySpeciality"), id, name, true);
            };

            document.getElementById("ddPersonSpecialitySpeciality").value = speciality;   
            
            // clean message label in the light box and hide it
            document.getElementById("spanAddEditSpecialityLightBox").style.display = "none";
            document.getElementById("spanAddEditSpecialityLightBox").innerHTML = "";

            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("lboxSpeciality").style.display = "";
            CenterLightBox("lboxSpeciality");
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

function ddProfession_Changed() {
    var ddProfession = document.getElementById("ddPersonSpecialityProfession");
    RepopulateSpeciality(ddProfession.value);
}

function RepopulateSpeciality(pProfessionID) {
    var url = "AddEditReservist_EducationWork.aspx?AjaxMethod=JSRepopulateSpeciality";
    var params = "";
    params += "ProfessionID=" + pProfessionID;

    function RepopulateSpeciality_Callback(xml) {
        ClearSelectList(document.getElementById("ddPersonSpecialitySpeciality"), true);

        var specialityOp = xml.getElementsByTagName("specialityOp");

        for (var i = 0; i < specialityOp.length; i++) {
            var id = xmlValue(specialityOp[i], "value");
            var name = xmlValue(specialityOp[i], "name");

            AddToSelectList(document.getElementById("ddPersonSpecialitySpeciality"), id, name, true);
        };

    }

    var myAJAX = new AJAX(url, true, params, RepopulateSpeciality_Callback);
    myAJAX.Call();
}

function HideAddEditSpecialityLightBox() {
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("lboxSpeciality").style.display = "none";
}
  
 function SaveAddEditSpecialityLightBox() {
    if (ValidateAddEditSpeciality()) {
        var url = "AddEditReservist_EducationWork.aspx?AjaxMethod=JSSaveSpeciality";

        var params = "PersonSpecialityID=" + document.getElementById("hdnPersonSpecialityID").value +
                     "&ReservistID=" + document.getElementById(hdnReservistIdClientID).value +
                     "&ProfessionID=" + document.getElementById("ddPersonSpecialityProfession").value +
                     "&SpecialityID=" + document.getElementById("ddPersonSpecialitySpeciality").value;

        function response_handler(xml) {
            var status = xmlValue(xml, "status");

            if (status == "OK") {
                var refreshedPositionsTable = xmlValue(xml, "refreshedSpecialityTable");

                document.getElementById("tblSpeciality").innerHTML = refreshedPositionsTable;

                document.getElementById("lblMessageSpeciality").className = "SuccessText";
                document.getElementById("lblMessageSpeciality").innerHTML = document.getElementById("hdnPersonSpecialityID").value == "0" ? "Специалността е добавена успешно" : "Специалността е редактирана успешно";

                HideAddEditSpecialityLightBox();
            }
            else {
                document.getElementById("spanAddEditSpecialityLightBox").className = "ErrorText";
                document.getElementById("spanAddEditSpecialityLightBox").innerHTML = status;
                document.getElementById("spanAddEditSpecialityLightBox").style.display = "";
            }            
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}


function ValidateAddEditSpeciality() {
    var res = true;

    var lblMessage = document.getElementById("spanAddEditSpecialityLightBox");
    lblMessage.innerHTML = "";

    var notValidFields = new Array();

    var ddPersonSpecialityProfession = document.getElementById("ddPersonSpecialityProfession");


    if (ddPersonSpecialityProfession.value == optionChooseOneValue) {
        res = false;

        if (ddPersonSpecialityProfession.disabled == true || ddPersonSpecialityProfession.style.display == "none")
            notValidFields.push("Професия");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Професия") + "<br />";
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


function DeleteSpeciality(pPersonSpecialityID) {
    ClearAllMessages();

    YesNoDialog("Желаете ли да изтриете специалността?", ConfirmYes, null);

    function ConfirmYes() {
        var url = "AddEditReservist_EducationWork.aspx?AjaxMethod=JSDeleteSpeciality";

        var params = "PersonSpecialityID=" + pPersonSpecialityID;
        params += "&ReservistID=" + document.getElementById(hdnReservistIdClientID).value;

        function response_handler(xml) {
            var status = xmlValue(xml, "status")

            if (status == "OK") {
                var refreshedSpecialityTable = xmlValue(xml, "refreshedSpecialityTable");

                document.getElementById("tblSpeciality").innerHTML = refreshedSpecialityTable;

                document.getElementById("lblMessageSpeciality").className = "SuccessText";
                document.getElementById("lblMessageSpeciality").innerHTML = "Специалността е изтрита успешно";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

function txtWorkPositionNKPDCode_Focus() {
    var txtWorkPositionNKPDCode = document.getElementById("txtWorkPositionNKPDCode");
    txtWorkPositionNKPDCode.setAttribute("oldvalue", txtWorkPositionNKPDCode.value);
}

function txtWorkPositionNKPDCode_Blur() {
    var compCode = document.getElementById("txtWorkPositionNKPDCode").value;
    var oldCode = document.getElementById("txtWorkPositionNKPDCode").oldvalue;
    if (compCode != oldCode) {
        if (compCode.Trim() == "") {
            document.getElementById("lblWorkPositionNKPDMessage").innerHTML = "";
            document.getElementById("hdnWorkPositionNKPDID").value = "";
        }
        else {
            //Prevent saving the old code if hitting Save too quick. That is why we rese the ID here.
            document.getElementById("hdnWorkPositionNKPDID").value = "";
        
            var url = "AddEditReservist_EducationWork.aspx?AjaxMethod=JSGetNKPDByCode";
            var params = "";
            params += "NKPDCode=" + compCode;
            var myAJAX = new AJAX(url, true, params, JSGetNKPDByCode_Callback);
            myAJAX.Call();
        }
    }

    function JSGetNKPDByCode_Callback(xml) {
        var message = xmlValue(xml, "message");
        var nkpdid = xmlValue(xml, "nkpdid");

        if (nkpdid == "") {
            document.getElementById("lblWorkPositionNKPDMessage").className = "ErrorText";
        }
        else {
            document.getElementById("lblWorkPositionNKPDMessage").className = "ReadOnlyValue";
        }

        document.getElementById("lblWorkPositionNKPDMessage").innerHTML = message;
        document.getElementById("hdnWorkPositionNKPDID").value = nkpdid;
    }
}

function Refresh_MilitaryReport_Postpone_WorkplaceInfo() {
    //If the dom element(s) are not there then the tab was not opened yet and then there is no need to reload
    if (!document.getElementById("lblPostponeWorkCompany"))
        return;

    var url = "AddEditReservist_EducationWork.aspx?AjaxMethod=JSGetWorkplaceInfo";
    var params = "";
    params += "ReservistId=" + document.getElementById(hdnReservistIdClientID).value;
    
    function JSGetWorkplaceInfo_Callback(xml) {
        var companyInfo = xmlValue(xml, "companyInfo");
        var nkpdInfo = xmlValue(xml, "nkpdInfo");

        document.getElementById("lblPostponeWorkCompanyValue").innerHTML = companyInfo;
        document.getElementById("lblPostponeWorkCompanyValueLightBox").innerHTML = companyInfo;
        document.getElementById("lblPostponeWorkPositionNKPDValue").innerHTML = nkpdInfo;
        document.getElementById("lblPostponeWorkPositionNKPDValueLightBox").innerHTML = nkpdInfo;
    }

    var myAJAX = new AJAX(url, true, params, JSGetWorkplaceInfo_Callback);
    myAJAX.Call();
}

function ShowSearchNKPDLightBox() {
    ClearAllMessages();

    var url = "AddEditReservist_EducationWork.aspx?AjaxMethod=JSPopulateSearchNKPDLightBoxFilter";
    
    function response_handler(xml) {
        for (var l = 1; l <= 4; l++) {
            ClearSelectList(document.getElementById("ddNKPDLevel" + l), true);
        
            var nkpdList = xml.getElementsByTagName("nkpdLevel_" + l)[0].getElementsByTagName("n");

            for (var i = 0; i < nkpdList.length; i++) {
                var id = xmlValue(nkpdList[i], "id");
                var name = xmlValue(nkpdList[i], "name");

                AddToSelectList(document.getElementById("ddNKPDLevel" + l), id, name);
            };
        }

        document.getElementById("txtNKPDCodeFilter").value = "";
        document.getElementById("txtNKPDNameFilter").value = "";

        document.getElementById("pnlNKPDResult").innerHTML = "<br /><br /><center>Задайте критерии за търсене и натиснете бутона 'Търси'</center>";
       
        document.getElementById("HidePage").style.display = "";
        document.getElementById("lboxSearchNKPD").style.display = "";
        CenterLightBox("lboxSearchNKPD");
    }

    var myAJAX = new AJAX(url, true, "", response_handler);
    myAJAX.Call();
}

function HideWorkPositionSearchNKPDCodeLightBox() {
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("lboxSearchNKPD").style.display = "none";
}

function ddNKPDLevel_Changed(level) {
    SearchNKPDLightBoxDisableInputs(true);

    var parentIDs = "";

    for (var l = 1; l <= level; l++) {
        if (document.getElementById("ddNKPDLevel" + l).value != optionAllValue) {
            parentIDs += (parentIDs == "" ? "" : ",") + document.getElementById("ddNKPDLevel" + l).value;
        }
    }

    var url = "AddEditReservist_EducationWork.aspx?AjaxMethod=JSRepopulateNKPDLevelOptionsOnChange";
    var params = "";
    params += "ParentIDs=" + parentIDs;
    params += "&Level=" + level;
    
    function JSRepopulateNKPDLevelOptionsOnChange_Callback(xml) {
        for (var l = level + 1; l <= 4; l++) {
            ClearSelectList(document.getElementById("ddNKPDLevel" + l), true);

            var nkpdList = xml.getElementsByTagName("nkpdLevel_" + l)[0].getElementsByTagName("n");

            for (var i = 0; i < nkpdList.length; i++) {
                var id = xmlValue(nkpdList[i], "id");
                var name = xmlValue(nkpdList[i], "name");

                AddToSelectList(document.getElementById("ddNKPDLevel" + l), id, name);
            };

            SearchNKPDLightBoxDisableInputs(false);
        }
    }

    var myAJAX = new AJAX(url, true, params, JSRepopulateNKPDLevelOptionsOnChange_Callback);
    myAJAX.Call();
}

function btnSearchNKPDLightBox_Click() {
    SearchNKPDLightBoxDisableInputs(true);
    
    var parentIDs = "";

    for (var l = 1; l <= 4; l++) {
        if (document.getElementById("ddNKPDLevel" + l).value != optionAllValue) {
            parentIDs += (parentIDs == "" ? "" : ",") + document.getElementById("ddNKPDLevel" + l).value;
        }
    }

    var url = "AddEditReservist_EducationWork.aspx?AjaxMethod=JSSearchNKPD";
    var params = "";
    params += "ParentIDs=" + parentIDs;
    params += "&NKPDCode=" + custEncodeURI(document.getElementById("txtNKPDCodeFilter").value);
    params += "&NKPDName=" + custEncodeURI(document.getElementById("txtNKPDNameFilter").value);
    
    function JSSearchNKPD_Callback(xml) {
        var nkpdList = xml.getElementsByTagName("n");
        var html = "";

        if (nkpdList.length > 0) {
            html = "<table class='CommonHeaderTable' style='width: 740px;'>" +
                   "   <colgroup>" +
                   "      <col style='width: 60px;'>" +
                   "      <col style='width: 80px;'>" +
                   "      <col style='width: 600px;'>" +
                   "   </colgroup>" +
                   "   <thead>" +
                   "      <tr>" +
                   "         <th style='vertical-align: bottom;'>№</th>" +
                   "         <th style='vertical-align: bottom;'>Код</th>" +
                   "         <th style='vertical-align: bottom;'>Професия</th>" +
                   "      </tr>" +
                   "   </thead>" +
                   "</table>" +
                   "<div style='height: 200px; width: 760px; overflow-y: auto;'>" +
                   "<table style='width: 740px;'>" +
                   "   <colgroup>" +
                   "      <col style='width: 60px;'>" +
                   "      <col style='width: 80px;'>" +
                   "      <col style='width: 600px;'>" +
                   "   </colgroup>";

            for (var i = 0; i < nkpdList.length; i++) {
                var id = xmlValue(nkpdList[i], "id");
                var code = xmlValue(nkpdList[i], "code");
                var name = xmlValue(nkpdList[i], "name");

                html += "<tr class='ListItem " + (i % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + "' " +
                        "    onclick=\"SearchNKPDLightBox_ChooseItem(" + id + ", '" + code + "', '" + name + "');\" " +
                        "    title='Избери' >" +
                        "   <td style='text-align: center;'>" + (i + 1) + "</td>" +
                        "   <td>" + code + "</td>" +
                        "   <td>" + name + "</td>" +
                        "</tr>";
            }

            html += "</div> " +
                    "</table>"
        }
        else {
            html = "<div style='text-align: center;'>Няма намерени резултати</div>";
        }

        document.getElementById("pnlNKPDResult").innerHTML = html;

        SearchNKPDLightBoxDisableInputs(false);
    }

    var myAJAX = new AJAX(url, true, params, JSSearchNKPD_Callback);
    myAJAX.Call();
}

function SearchNKPDLightBoxDisableInputs(disabled) {
    for (var i = 1; i <= 4; i++)
        document.getElementById("ddNKPDLevel" + i).disabled = disabled;

    document.getElementById("txtNKPDCodeFilter").disabled = disabled;
    document.getElementById("txtNKPDNameFilter").disabled = disabled;
}

function SearchNKPDLightBox_ChooseItem(nkpdId, nkpdCode, nkpdName) {
    document.getElementById("hdnWorkPositionNKPDID").value = nkpdId;
    document.getElementById("txtWorkPositionNKPDCode").value = nkpdCode;
    document.getElementById("lblWorkPositionNKPDMessage").innerHTML = nkpdName;

    HideWorkPositionSearchNKPDCodeLightBox();
}

function CompanySelector_OnSelectedCompany(companyId, companyName, companyUnifiedIdentityCode, owneshipType) {
    document.getElementById("hdnCompanyID").value = companyId;
    document.getElementById("lblCompanyNameValue").innerHTML = companyName;
    document.getElementById("lblUnifiedIdentityCodeValue").innerHTML = companyUnifiedIdentityCode;
    document.getElementById("lblOwnershipTypeValue").innerHTML = owneshipType;
}

function VitoshaMilRepSpecTypeLightBoxChanged(pDropDawnSourceID, pDropDawnTargetID) {
    var url = "AddEditReservist_EducationWork.aspx?AjaxMethod=JSLoadVitoshaMilRepSpecs";

    var params = "VitoshaMilRepSpecTypeID=" + document.getElementById(pDropDawnSourceID).value;

    function response_handler(xml) {
        ClearSelectList(document.getElementById(pDropDawnTargetID), true);

        var milRepSpecs = xml.getElementsByTagName("m");

        for (var i = 0; i < milRepSpecs.length; i++) {
            var id = xmlValue(milRepSpecs[i], "code");
            var name = xmlValue(milRepSpecs[i], "name");

            AddToSelectList(document.getElementById(pDropDawnTargetID), id, name, true);
        };
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}

function CivilEducationSelector_OnSelectedCivilEducation(schoolSubjectCode, schoolSubjectName) {

    document.getElementById("txtSubject").innerHTML = schoolSubjectName;
    document.getElementById("hdnSchoolSubjectCode").value = schoolSubjectCode;
}