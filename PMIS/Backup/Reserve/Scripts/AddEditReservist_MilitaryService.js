//Clear all messages on the MilitaryService tab
function ClearMilitaryServiceMessages()
{

    if (document.getElementById("lblMessageArchiveTitle"))
        document.getElementById("lblMessageArchiveTitle").innerHTML = "";

    if (document.getElementById("lblMessageRewardIncentiv"))
        document.getElementById("lblMessageRewardIncentiv").innerHTML = "";

    if (document.getElementById("lblMessagePenalty"))
        document.getElementById("lblMessagePenalty").innerHTML = "";

    if (document.getElementById("lblMessageContract"))
        document.getElementById("lblMessageContract").innerHTML = "";

    if (document.getElementById("lblMessagePreviousPosition"))
        document.getElementById("lblMessagePreviousPosition").innerHTML = "";

    if (document.getElementById("lblMessageConscription"))
        document.getElementById("lblMessageConscription").innerHTML = "";

}

// 1.----------- Table ArchiveTitle -------------------

//Load the ArchiveTitle table on demand
function lnkArchiveTitle_Click()
{
    LoadArchiveTitles();
}

function LoadArchiveTitles()
{
    //If already loaded then do not load
    if (document.getElementById("hdnArchiveTitleLoaded").value == "1")
        return;

    ClearAllMessages();

    document.getElementById("imgLoadingArchiveTitle").style.visibility = "";

    var url = "AddEditReservist_MilitaryService.aspx?AjaxMethod=JSLoadArchiveTitles";
    var params = "";
    params += "ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

    var hdnIsPreview = document.getElementById(hdnIsPreviewClientID).value;
    if (hdnIsPreview == 1)
        params += "&Preview=1";
    
    function LoadArchiveTitle_CallBack(xml)
    {
        var tableHTML = xmlValue(xml, "tableHTML");
        var lightBoxHTML = xmlValue(xml, "lightBoxHTML");

        document.getElementById("tblArchiveTitle").innerHTML = tableHTML;
        document.getElementById("lboxArchiveTitle").innerHTML = lightBoxHTML;
        RefreshUIItems(xml);
        document.getElementById("imgLoadingArchiveTitle").style.visibility = "hidden";

        //Mark as already loaded
        document.getElementById("hdnArchiveTitleLoaded").value = "1";
    }

    var myAJAX = new AJAX(url, true, params, LoadArchiveTitle_CallBack);
    myAJAX.Call();
}

//Open the light-box for adding a new record in the ArchiveTitle table
function NewArchiveTitle()
{
    ShowAddEditArchiveTitleLightBox(0);
}

//Open the light-box for editing a record in the ArchiveTitle table
function EditArchiveTitle(ArchiveTitleId)
{
    ShowAddEditArchiveTitleLightBox(ArchiveTitleId);
}

function ShowAddEditArchiveTitleLightBox(ArchiveTitleId)
{
    ClearAllMessages();

    //Set DateTime Pickers
    RefreshDatePickers();

    document.getElementById("hdnArchiveTitleID").value = ArchiveTitleId;

    //New record
    if (ArchiveTitleId == 0)
    {
        document.getElementById("lblAddEditArchiveTitleTitle").innerHTML = "Въвеждане на ново звание";

        // if (document.getElementById("ddPersonArchiveTitleMilitaryRank") != undefined)
        document.getElementById("ddPersonArchiveTitleMilitaryRank").value = optionChooseOneValue;
        // if (document.getElementById("txtPersonArchiveTitleVacAnn") != undefined)
        document.getElementById("txtPersonArchiveTitleVacAnn").value = "";
        //  if (document.getElementById("txtPersonArchiveTitleDateArchive") != undefined)
        document.getElementById("txtPersonArchiveTitleDateArchive").value = "";
        //  if (document.getElementById("txtPersonArchiveTitleDateWhen") != undefined)
        document.getElementById("txtPersonArchiveTitleDateWhen").value = "";
        //  if (document.getElementById("ddPersonArchiveTitleMilitaryCommanderRank") != undefined)
        document.getElementById("ddPersonArchiveTitleMilitaryCommanderRank").value = optionChooseOneValue;
       
        document.getElementById("chkPersonArchiveTitleDR").checked = false;

        // clean message label in the light box and hide it
        document.getElementById("spanAddEditArchiveTitleLightBox").style.display = "none";
        document.getElementById("spanAddEditArchiveTitleLightBox").innerHTML = "";

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("lboxArchiveTitle").style.display = "";
        CenterLightBox("lboxArchiveTitle");
    }
    else //Edit Position
    {
        document.getElementById("lblAddEditArchiveTitleTitle").innerHTML = "Редактиране на звание";

        var url = "AddEditReservist_MilitaryService.aspx?AjaxMethod=JSLoadArchiveTitle";

        var params = "ArchiveTitleId=" + ArchiveTitleId;

        function response_handler(xml)
        {
            var personArchiveTitle = xml.getElementsByTagName("personArchiveTitle")[0];

            var ArchiveTitleMilitaryRankId = xmlValue(personArchiveTitle, "ArchiveTitleMilitaryRankId");
            var ArchiveTitleVacAnn = xmlValue(personArchiveTitle, "ArchiveTitleVacAnn");
            var ArchiveTitleDateArchive = xmlValue(personArchiveTitle, "ArchiveTitleDateArchive");
            var ArchiveTitleDateWhen = xmlValue(personArchiveTitle, "ArchiveTitleDateWhen");
            var ArchiveTitleMilitaryCommanderRankCode = xmlValue(personArchiveTitle, "ArchiveTitleMilitaryCommanderRankCode");
            var ArchiveTitleDR = (xmlValue(personArchiveTitle, "ArchiveTitleDR") == "1" ? true : false);

            // if (document.getElementById("ddPersonArchiveTitleMilitaryRank") != undefined)
            document.getElementById("ddPersonArchiveTitleMilitaryRank").value = ArchiveTitleMilitaryRankId;
            // if (document.getElementById("txtPersonArchiveTitleVacAnn") != undefined)
            document.getElementById("txtPersonArchiveTitleVacAnn").value = ArchiveTitleVacAnn;
            // if (document.getElementById("txtPersonArchiveTitleDateArchive") != undefined)
            document.getElementById("txtPersonArchiveTitleDateArchive").value = ArchiveTitleDateArchive;
            //  if (document.getElementById("txtPersonArchiveTitleDateWhen") != undefined)
            document.getElementById("txtPersonArchiveTitleDateWhen").value = ArchiveTitleDateWhen;
            //   if (document.getElementById("ddPersonArchiveTitleMilitaryCommanderRank") != undefined)
            document.getElementById("ddPersonArchiveTitleMilitaryCommanderRank").value = ArchiveTitleMilitaryCommanderRankCode;
           
            document.getElementById("chkPersonArchiveTitleDR").checked = ArchiveTitleDR;

            // clean message label in the light box and hide it
            document.getElementById("spanAddEditArchiveTitleLightBox").style.display = "none";
            document.getElementById("spanAddEditArchiveTitleLightBox").innerHTML = "";

            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("lboxArchiveTitle").style.display = "";
            CenterLightBox("lboxArchiveTitle");
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Close the light-box
function HideAddEditArchiveTitleLightBox()
{
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("lboxArchiveTitle").style.display = "none";
}

//Save Add/Edit ArchiveTitle
function SaveAddEditArchiveTitleLightBox()
{
    if (ValidateAddEditArchiveTitle())
    {
        var url = "AddEditReservist_MilitaryService.aspx?AjaxMethod=JSSaveArchiveTitle";

        //Set variable for fields thta can be not bound from UI Items logic but Save?Update can performe
        var ArchiveTitleVacAnn = "";
        if (document.getElementById("txtPersonArchiveTitleVacAnn") != undefined)
            ArchiveTitleVacAnn = document.getElementById("txtPersonArchiveTitleVacAnn").value;


        var ArchiveTitleMilitaryCommanderRankCode = "-1";
        if (document.getElementById("ddPersonArchiveTitleMilitaryCommanderRank") != undefined)
            ArchiveTitleMilitaryCommanderRankCode = document.getElementById("ddPersonArchiveTitleMilitaryCommanderRank").value;

        var ArchiveTitleDR = "0";
        if (document.getElementById("chkPersonArchiveTitleDR") != undefined)
            ArchiveTitleDR = document.getElementById("chkPersonArchiveTitleDR").checked ? "1" : "0";
            
        var params = "ArchiveTitleId=" + document.getElementById("hdnArchiveTitleID").value +
                     "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value +
                     "&ArchiveTitleMilitaryRankId=" + document.getElementById("ddPersonArchiveTitleMilitaryRank").value +
                     "&ArchiveTitleVacAnn=" + ArchiveTitleVacAnn +
                     "&ArchiveTitleDateArchive=" + document.getElementById("txtPersonArchiveTitleDateArchive").value +
                     "&ArchiveTitleDateWhen=" + document.getElementById("txtPersonArchiveTitleDateWhen").value +
                     "&ArchiveTitleMilitaryCommanderRankCode=" + ArchiveTitleMilitaryCommanderRankCode +
                     "&ArchiveTitleDR=" + ArchiveTitleDR;


        function response_handler(xml)
        {
            var status = xmlValue(xml, "status");

            if (status == "OK")
            {
                var refreshedPositionsTable = xmlValue(xml, "refreshedArchiveTitleTable");

                document.getElementById("tblArchiveTitle").innerHTML = refreshedPositionsTable;

                document.getElementById("lblMessageArchiveTitle").className = "SuccessText";
                document.getElementById("lblMessageArchiveTitle").innerHTML = document.getElementById("hdnArchiveTitleID").value == "0" ? "Званието е добавено успешно" : "Званието е редактирано успешно";

                HideAddEditArchiveTitleLightBox();
            }
            else
            {
                document.getElementById("spanAddEditArchiveTitleLightBox").className = "ErrorText";
                document.getElementById("spanAddEditArchiveTitleLightBox").innerHTML = status;
                document.getElementById("spanAddEditArchiveTitleLightBox").style.display = "";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Client validation of Add/Edit ArchiveTitlelight-box
function ValidateAddEditArchiveTitle()
{
    var res = true;

    var lblMessage = document.getElementById("spanAddEditArchiveTitleLightBox");
    lblMessage.innerHTML = "";

    var notValidFields = new Array();

    var ddPersonArchiveTitleMilitaryRank = document.getElementById("ddPersonArchiveTitleMilitaryRank");
    var txtPersonArchiveTitleDateArchive = document.getElementById("txtPersonArchiveTitleDateArchive");
    var txtPersonArchiveTitleDateWhen = document.getElementById("txtPersonArchiveTitleDateWhen");


    //Validate Mandatory Fields

    //1 MilitaryRank
    if (ddPersonArchiveTitleMilitaryRank.value == optionChooseOneValue)
    {
        res = false;

        if (ddPersonArchiveTitleMilitaryRank.disabled == true || ddPersonArchiveTitleMilitaryRank.style.display == "none")
            notValidFields.push("Звание");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Звание") + "<br />";
    }

    //2  DateArchive
    if (txtPersonArchiveTitleDateArchive.value.Trim() == "")
    {
        res = false;

        if (txtPersonArchiveTitleDateArchive.disabled == true || spanPersonArchiveTitleDateArchive.style.display == "none")
            notValidFields.push("Дата");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Дата") + "<br />";
    }
    else
    {
        if (!IsValidDate(txtPersonArchiveTitleDateArchive.value))
        {
            res = false;
            lblMessage.innerHTML += GetErrorMessageDate("Дата") + "<br />";
        }
    }

    // 3 DateWhen
    if (txtPersonArchiveTitleDateWhen.value.Trim() == "")
    {
        res = false;

        if (txtPersonArchiveTitleDateWhen.disabled == true || spanPersonArchiveTitleDateWhen.style.display == "none")
            notValidFields.push("В сила от");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("В сила от") + "<br />";
    }
    else
    {
        if (!IsValidDate(txtPersonArchiveTitleDateWhen.value))
        {
            res = false;
            lblMessage.innerHTML += GetErrorMessageDate("В сила от") + "<br />";
        }
    }

    //Validate other fields - No  fields to validate


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

// Delete a particular ArchiveTitle record
function DeleteArchiveTitle(ArchiveTitleId)
{
    ClearAllMessages();
    
    YesNoDialog("Желаете ли да изтриете званието?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "AddEditReservist_MilitaryService.aspx?AjaxMethod=JSDeleteArchiveTitle";

        var params = "ArchiveTitleId=" + ArchiveTitleId;
        params += "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

        function response_handler(xml)
        {
            var status = xmlValue(xml, "status")

            if (status == "OK")
            {
                var refreshedPositionsTable = xmlValue(xml, "refreshedArchiveTitleTable");

                document.getElementById("tblArchiveTitle").innerHTML = refreshedPositionsTable;

                document.getElementById("lblMessageArchiveTitle").className = "SuccessText";
                document.getElementById("lblMessageArchiveTitle").innerHTML = "Званието е изтрито успешно";

            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}


// 2.----------- Table RewardIncentiv -------------------

//Load the RewardIncentiv table on demand
function lnkRewardIncentiv_Click()
{
    LoadRewardIncentivs();
}

function LoadRewardIncentivs()
{
    //If already loaded then do not load
    if (document.getElementById("hdnRewardIncentivLoaded").value == "1")
        return;

    ClearAllMessages();

    document.getElementById("imgLoadingRewardIncentiv").style.visibility = "";

    var url = "AddEditReservist_MilitaryService.aspx?AjaxMethod=JSLoadRewardIncentivs";
    var params = "";
    params += "ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

    var hdnIsPreview = document.getElementById(hdnIsPreviewClientID).value;
    if (hdnIsPreview == 1)
        params += "&Preview=1";
    
    function LoadRewardIncentiv_CallBack(xml)
    {
        var tableHTML = xmlValue(xml, "tableHTML");
        var lightBoxHTML = xmlValue(xml, "lightBoxHTML");

        document.getElementById("tblRewardIncentiv").innerHTML = tableHTML;
        document.getElementById("lboxRewardIncentiv").innerHTML = lightBoxHTML;
        RefreshUIItems(xml);
        document.getElementById("imgLoadingRewardIncentiv").style.visibility = "hidden";

        //Mark as already loaded
        document.getElementById("hdnRewardIncentivLoaded").value = "1";
    }

    var myAJAX = new AJAX(url, true, params, LoadRewardIncentiv_CallBack);
    myAJAX.Call();
}

//Open the light-box for adding a new record in the RewardIncentiv table
function NewRewardIncentiv()
{
    ShowAddEditRewardIncentivLightBox(0);
}

//Open the light-box for editing a record in the RewardIncentiv table
function EditRewardIncentiv(RewardIncentivId)
{
    ShowAddEditRewardIncentivLightBox(RewardIncentivId);
}

function ShowAddEditRewardIncentivLightBox(RewardIncentivId)
{
    ClearAllMessages();

    //Set DateTime Pickers
    RefreshDatePickers();

    document.getElementById("hdnRewardIncentivID").value = RewardIncentivId;

    //New record
    if (RewardIncentivId == 0)
    {
        document.getElementById("lblAddEditRewardIncentivTitle").innerHTML = "Въвеждане на ново награда";

        //  if (document.getElementById("ddPersonRewardIncentivRewardIncentiv") != undefined)
        document.getElementById("ddPersonRewardIncentivRewardIncentiv").value = optionChooseOneValue;
        // if (document.getElementById("txtPersonRewardIncentivNumber") != undefined)
        document.getElementById("txtPersonRewardIncentivNumber").value = "";
        //  if (document.getElementById("txtPersonRewardIncentivVacAnn") != undefined)
        document.getElementById("txtPersonRewardIncentivVacAnn").value = "";
        //   if (document.getElementById("txtPersonRewardIncentivDateWhen") != undefined)
        document.getElementById("txtPersonRewardIncentivDateWhen").value = "";
        //     if (document.getElementById("ddPersonRewardIncentivMilitaryCommanderRank") != undefined)
        document.getElementById("ddPersonRewardIncentivMilitaryCommanderRank").value = optionChooseOneValue;


        // clean message label in the light box and hide it
        document.getElementById("spanAddEditRewardIncentivLightBox").style.display = "none";
        document.getElementById("spanAddEditRewardIncentivLightBox").innerHTML = "";

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("lboxRewardIncentiv").style.display = "";
        CenterLightBox("lboxRewardIncentiv");
    }
    else //Edit Position
    {
        document.getElementById("lblAddEditRewardIncentivTitle").innerHTML = "Редактиране на награда";

        var url = "AddEditReservist_MilitaryService.aspx?AjaxMethod=JSLoadRewardIncentiv";

        var params = "RewardIncentivId=" + RewardIncentivId;

        function response_handler(xml)
        {
            var personRewardIncentiv = xml.getElementsByTagName("personRewardIncentiv")[0];

            var RewardIncentivCode = xmlValue(personRewardIncentiv, "RewardIncentivCode");
            var RewardIncentivNumber = xmlValue(personRewardIncentiv, "RewardIncentivNumber");
            var RewardIncentivVacAnn = xmlValue(personRewardIncentiv, "RewardIncentivVacAnn");
            var RewardIncentivDateWhen = xmlValue(personRewardIncentiv, "RewardIncentivDateWhen");
            var RewardIncentivMilitaryCommanderRankCode = xmlValue(personRewardIncentiv, "RewardIncentivMilitaryCommanderRankCode");


            //    if (document.getElementById("ddPersonRewardIncentivRewardIncentiv") != undefined)
            document.getElementById("ddPersonRewardIncentivRewardIncentiv").value = RewardIncentivCode;
            //    if (document.getElementById("txtPersonRewardIncentivNumber") != undefined)
            document.getElementById("txtPersonRewardIncentivNumber").value = RewardIncentivNumber;
            //     if (document.getElementById("txtPersonRewardIncentivVacAnn") != undefined)
            document.getElementById("txtPersonRewardIncentivVacAnn").value = RewardIncentivVacAnn;
            //     if (document.getElementById("txtPersonRewardIncentivDateWhen") != undefined)
            document.getElementById("txtPersonRewardIncentivDateWhen").value = RewardIncentivDateWhen;
            //     if (document.getElementById("ddPersonRewardIncentivMilitaryCommanderRank") != undefined)
            document.getElementById("ddPersonRewardIncentivMilitaryCommanderRank").value = RewardIncentivMilitaryCommanderRankCode;

            // clean message label in the light box and hide it
            document.getElementById("spanAddEditRewardIncentivLightBox").style.display = "none";
            document.getElementById("spanAddEditRewardIncentivLightBox").innerHTML = "";

            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("lboxRewardIncentiv").style.display = "";
            CenterLightBox("lboxRewardIncentiv");
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Close the light-box
function HideAddEditRewardIncentivLightBox()
{
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("lboxRewardIncentiv").style.display = "none";
}

//Save Add/Edit RewardIncentiv
function SaveAddEditRewardIncentivLightBox()
{
    if (ValidateAddEditRewardIncentiv())
    {
        var url = "AddEditReservist_MilitaryService.aspx?AjaxMethod=JSSaveRewardIncentiv";

        //Set variable for fields thta can be not bound from UI Items logic but Save?Update can performe
        var RewardIncentivVacAnn = "";
        if (document.getElementById("txtPersonRewardIncentivVacAnn") != undefined)
            RewardIncentivVacAnn = document.getElementById("txtPersonRewardIncentivVacAnn").value;


        var RewardIncentivMilitaryCommanderRankCode = "-1";
        if (document.getElementById("ddPersonRewardIncentivMilitaryCommanderRank") != undefined)
            RewardIncentivMilitaryCommanderRankCode = document.getElementById("ddPersonRewardIncentivMilitaryCommanderRank").value;


        var params = "RewardIncentivId=" + document.getElementById("hdnRewardIncentivID").value +
                     "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value +
                     "&RewardIncentivCode=" + document.getElementById("ddPersonRewardIncentivRewardIncentiv").value +
                     "&RewardIncentivNumber=" + document.getElementById("txtPersonRewardIncentivNumber").value +
                     "&RewardIncentivVacAnn=" + RewardIncentivVacAnn +
                     "&RewardIncentivDateWhen=" + document.getElementById("txtPersonRewardIncentivDateWhen").value +
                     "&RewardIncentivMilitaryCommanderRankCode=" + RewardIncentivMilitaryCommanderRankCode;



        function response_handler(xml)
        {
            var status = xmlValue(xml, "status");

            if (status == "OK")
            {
                var refreshedPositionsTable = xmlValue(xml, "refreshedRewardIncentivTable");

                document.getElementById("tblRewardIncentiv").innerHTML = refreshedPositionsTable;

                document.getElementById("lblMessageRewardIncentiv").className = "SuccessText";
                document.getElementById("lblMessageRewardIncentiv").innerHTML = document.getElementById("hdnRewardIncentivID").value == "0" ? "Наградата е добавена успешно" : "Наградата е редактирана успешно";

                HideAddEditRewardIncentivLightBox();
            }
            else
            {
                document.getElementById("spanAddEditRewardIncentivLightBox").className = "ErrorText";
                document.getElementById("spanAddEditRewardIncentivLightBox").innerHTML = status;
                document.getElementById("spanAddEditRewardIncentivLightBox").style.display = "";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Client validation of Add/Edit RewardIncentivlight-box
function ValidateAddEditRewardIncentiv()
{
    var res = true;

    var lblMessage = document.getElementById("spanAddEditRewardIncentivLightBox");
    lblMessage.innerHTML = "";

    var notValidFields = new Array();

    var ddPersonRewardIncentivRewardIncentiv = document.getElementById("ddPersonRewardIncentivRewardIncentiv");
    var txtPersonRewardIncentivNumber = document.getElementById("txtPersonRewardIncentivNumber");
    var txtPersonRewardIncentivDateWhen = document.getElementById("txtPersonRewardIncentivDateWhen");



    //Validate Mandatory Fields

    //1 RewardIncentiv
    if (ddPersonRewardIncentivRewardIncentiv.value == optionChooseOneValue)
    {
        res = false;

        if (ddPersonRewardIncentivRewardIncentiv.disabled == true || ddPersonRewardIncentivRewardIncentiv.style.display == "none")
            notValidFields.push("Награда");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Награда") + "<br />";
    }

    //2  Number
    if (txtPersonRewardIncentivNumber.value.Trim() == "")
    {
        res = false;

        if (txtPersonRewardIncentivNumber.disabled == true || txtPersonRewardIncentivNumber.style.display == "none")
            notValidFields.push("Номер");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Номер") + "<br />";
    }
    else
    {
        if (!isInt(txtPersonRewardIncentivNumber.value) || parseInt(txtPersonRewardIncentivNumber.value) <= 0)
        {
            res = false;
            lblMessage.innerHTML += GetErrorMessageNumber("Номер") + "<br />";
        }
    }

    // 3 DateWhen
    if (txtPersonRewardIncentivDateWhen.value.Trim() == "")
    {
        res = false;

        if (txtPersonRewardIncentivDateWhen.disabled == true || spanPersonRewardIncentivDateWhen.style.display == "none")
            notValidFields.push("Дата");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Дата") + "<br />";
    }
    else
    {
        if (!IsValidDate(txtPersonRewardIncentivDateWhen.value))
        {
            res = false;
            lblMessage.innerHTML += GetErrorMessageDate("Дата") + "<br />";
        }
    }
    //  }

    //Validate other fields - No  fields to validate


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

// Delete a particular RewardIncentiv record
function DeleteRewardIncentiv(RewardIncentivId)
{
    ClearAllMessages();

    YesNoDialog("Желаете ли да изтриете наградата?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "AddEditReservist_MilitaryService.aspx?AjaxMethod=JSDeleteRewardIncentiv";

        var params = "RewardIncentivId=" + RewardIncentivId;
        params += "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

        function response_handler(xml)
        {
            var status = xmlValue(xml, "status")

            if (status == "OK")
            {
                var refreshedPositionsTable = xmlValue(xml, "refreshedRewardIncentivTable");

                document.getElementById("tblRewardIncentiv").innerHTML = refreshedPositionsTable;

                document.getElementById("lblMessageRewardIncentiv").className = "SuccessText";
                document.getElementById("lblMessageRewardIncentiv").innerHTML = "Наградата е изтрита успешно";

            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}




// 3.----------- Table Penalty -------------------

//Load the Penalty table on demand
function lnkPenalty_Click()
{
    LoadPenaltys();
}

function LoadPenaltys()
{
    //If already loaded then do not load
    if (document.getElementById("hdnPenaltyLoaded").value == "1")
        return;

    ClearAllMessages();

    document.getElementById("imgLoadingPenalty").style.visibility = "";

    var url = "AddEditReservist_MilitaryService.aspx?AjaxMethod=JSLoadPenaltys";
    var params = "";
    params += "ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

    var hdnIsPreview = document.getElementById(hdnIsPreviewClientID).value;
    if (hdnIsPreview == 1)
        params += "&Preview=1";
    
    function LoadPenalty_CallBack(xml)
    {
        var tableHTML = xmlValue(xml, "tableHTML");
        var lightBoxHTML = xmlValue(xml, "lightBoxHTML");

        document.getElementById("tblPenalty").innerHTML = tableHTML;
        document.getElementById("lboxPenalty").innerHTML = lightBoxHTML;
        RefreshUIItems(xml);
        document.getElementById("imgLoadingPenalty").style.visibility = "hidden";

        //Mark as already loaded
        document.getElementById("hdnPenaltyLoaded").value = "1";
    }

    var myAJAX = new AJAX(url, true, params, LoadPenalty_CallBack);
    myAJAX.Call();
}

//Open the light-box for adding a new record in the Penalty table
function NewPenalty()
{
    ShowAddEditPenaltyLightBox(0);
}

//Open the light-box for editing a record in the Penalty table
function EditPenalty(PenaltyId)
{
    ShowAddEditPenaltyLightBox(PenaltyId);
}

function ShowAddEditPenaltyLightBox(PenaltyId)
{
    ClearAllMessages();

    //Set DateTime Pickers
    RefreshDatePickers();

    document.getElementById("hdnPenaltyID").value = PenaltyId;

    //New record
    if (PenaltyId == 0)
    {
        document.getElementById("lblAddEditPenaltyTitle").innerHTML = "Въвеждане на ново наказание";

        if (document.getElementById("ddPersonPenaltyPenalty") != undefined)
            document.getElementById("ddPersonPenaltyPenalty").value = "";

        // if (document.getElementById("txtPersonPenaltyVacAnnImposed") != undefined)
        document.getElementById("txtPersonPenaltyVacAnnImposed").value = "";
        // if (document.getElementById("txtPersonPenaltyDateImposed") != undefined)
        document.getElementById("txtPersonPenaltyDateImposed").value = "";
        //  if (document.getElementById("ddPersonPenaltyMilitaryCommanderRankCodeImposed") != undefined)
        document.getElementById("ddPersonPenaltyMilitaryCommanderRankCodeImposed").value = optionChooseOneValue;

        //   if (document.getElementById("ddPersonPenaltyMilitaryCommanderRankCodeCanceled") != undefined)
        document.getElementById("ddPersonPenaltyMilitaryCommanderRankCodeCanceled").value = optionChooseOneValue;
        //   if (document.getElementById("txtPersonPenaltyVacAnnCanceled") != undefined)
        document.getElementById("txtPersonPenaltyVacAnnCanceled").value = "";
        //   if (document.getElementById("txtPersonPenaltyDateCanceled") != undefined)
        document.getElementById("txtPersonPenaltyDateCanceled").value = "";

        // clean message label in the light box and hide it
        document.getElementById("spanAddEditPenaltyLightBox").style.display = "none";
        document.getElementById("spanAddEditPenaltyLightBox").innerHTML = "";

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("lboxPenalty").style.display = "";
        CenterLightBox("lboxPenalty");
    }
    else //Edit Position
    {
        document.getElementById("lblAddEditPenaltyTitle").innerHTML = "Редактиране на наказание";

        var url = "AddEditReservist_MilitaryService.aspx?AjaxMethod=JSLoadPenalty";

        var params = "PenaltyId=" + PenaltyId;

        function response_handler(xml)
        {
            var personPenalty = xml.getElementsByTagName("personPenalty")[0];

            var PenaltyCode = xmlValue(personPenalty, "PenaltyCode");

            var VacAnnImposed = xmlValue(personPenalty, "VacAnnImposed");
            var DateImposed = xmlValue(personPenalty, "DateImposed");
            var MilitaryCommanderRankCodeImposed = xmlValue(personPenalty, "MilitaryCommanderRankCodeImposed");

            var MilitaryCommanderRankCodeCanceled = xmlValue(personPenalty, "MilitaryCommanderRankCodeCanceled");
            var VacAnnCanceled = xmlValue(personPenalty, "VacAnnCanceled");
            var DateCanceled = xmlValue(personPenalty, "DateCanceled");


            // if (document.getElementById("ddPersonPenaltyPenalty") != undefined)
            document.getElementById("ddPersonPenaltyPenalty").value = PenaltyCode;

            // if (document.getElementById("txtPersonPenaltyVacAnnImposed") != undefined)
            document.getElementById("txtPersonPenaltyVacAnnImposed").value = VacAnnImposed;
            //  if (document.getElementById("txtPersonPenaltyDateImposed") != undefined)
            document.getElementById("txtPersonPenaltyDateImposed").value = DateImposed;
            //  if (document.getElementById("ddPersonPenaltyMilitaryCommanderRankCodeImposed") != undefined)
            document.getElementById("ddPersonPenaltyMilitaryCommanderRankCodeImposed").value = MilitaryCommanderRankCodeImposed;

            //   if (document.getElementById("ddPersonPenaltyMilitaryCommanderRankCodeCanceled") != undefined)
            document.getElementById("ddPersonPenaltyMilitaryCommanderRankCodeCanceled").value = MilitaryCommanderRankCodeCanceled;
            //   if (document.getElementById("txtPersonPenaltyVacAnnCanceled") != undefined)
            document.getElementById("txtPersonPenaltyVacAnnCanceled").value = VacAnnCanceled;
            //   if (document.getElementById("txtPersonPenaltyDateCanceled") != undefined)
            document.getElementById("txtPersonPenaltyDateCanceled").value = DateCanceled;


            // clean message label in the light box and hide it
            document.getElementById("spanAddEditPenaltyLightBox").style.display = "none";
            document.getElementById("spanAddEditPenaltyLightBox").innerHTML = "";

            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("lboxPenalty").style.display = "";
            CenterLightBox("lboxPenalty");
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Close the light-box
function HideAddEditPenaltyLightBox()
{
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("lboxPenalty").style.display = "none";
}

//Save Add/Edit Penalty
function SaveAddEditPenaltyLightBox()
{
    if (ValidateAddEditPenalty())
    {
        var url = "AddEditReservist_MilitaryService.aspx?AjaxMethod=JSSavePenalty";

        //Set variable for fields thta can be not bound from UI Items logic but Save/Update can performe
        var VacAnnImposed = "";
        if (document.getElementById("txtPersonPenaltyVacAnnImposed") != undefined)
            VacAnnImposed = document.getElementById("txtPersonPenaltyVacAnnImposed").value;


        var MilitaryCommanderRankCodeCanceled = "-1";
        if (document.getElementById("ddPersonPenaltyMilitaryCommanderRankCodeCanceled") != undefined)
            MilitaryCommanderRankCodeCanceled = document.getElementById("ddPersonPenaltyMilitaryCommanderRankCodeCanceled").value;


        var VacAnnCanceled = "";
        if (document.getElementById("txtPersonPenaltyVacAnnCanceled") != undefined)
            VacAnnCanceled = document.getElementById("txtPersonPenaltyVacAnnCanceled").value;


        var DateCanceled = "";
        if (document.getElementById("txtPersonPenaltyDateCanceled") != undefined)
            DateCanceled = document.getElementById("txtPersonPenaltyDateCanceled").value;

        var params = "PenaltyId=" + document.getElementById("hdnPenaltyID").value +
                     "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value +
                     "&PenaltyCode=" + document.getElementById("ddPersonPenaltyPenalty").value +
                     "&VacAnnImposed=" + VacAnnImposed +
                     "&DateImposed=" + document.getElementById("txtPersonPenaltyDateImposed").value +
                     "&MilitaryCommanderRankCodeImposed=" + document.getElementById("ddPersonPenaltyMilitaryCommanderRankCodeImposed").value +
                     "&MilitaryCommanderRankCodeCanceled=" + MilitaryCommanderRankCodeCanceled +
                     "&VacAnnCanceled=" + VacAnnCanceled +
                     "&DateCanceled=" + DateCanceled;

        function response_handler(xml)
        {
            var status = xmlValue(xml, "status");

            if (status == "OK")
            {
                var refreshedPositionsTable = xmlValue(xml, "refreshedPenaltyTable");

                document.getElementById("tblPenalty").innerHTML = refreshedPositionsTable;

                document.getElementById("lblMessagePenalty").className = "SuccessText";
                document.getElementById("lblMessagePenalty").innerHTML = document.getElementById("hdnPenaltyID").value == "0" ? "Наказанието е добавено успешно" : "Наказанието е редактирано успешно";

                HideAddEditPenaltyLightBox();
            }
            else
            {
                document.getElementById("spanAddEditPenaltyLightBox").className = "ErrorText";
                document.getElementById("spanAddEditPenaltyLightBox").innerHTML = status;
                document.getElementById("spanAddEditPenaltyLightBox").style.display = "";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Client validation of Add/Edit Penaltylight-box
function ValidateAddEditPenalty()
{
    var res = true;

    var lblMessage = document.getElementById("spanAddEditPenaltyLightBox");
    lblMessage.innerHTML = "";

    var notValidFields = new Array();

    var ddPersonPenaltyPenalty = document.getElementById("ddPersonPenaltyPenalty");
    var txtPersonPenaltyVacAnnImposed = document.getElementById("txtPersonPenaltyVacAnnImposed");
    var txtPersonPenaltyDateImposed = document.getElementById("txtPersonPenaltyDateImposed");
    var ddPersonPenaltyMilitaryCommanderRankCodeImposed = document.getElementById("ddPersonPenaltyMilitaryCommanderRankCodeImposed");

    //Validate Mandatory Fields

    //1 Penalty
    //? In this case  ddPersonPenaltyPenalty.value="" Why not "-1"
    if (ddPersonPenaltyPenalty.value == optionChooseOneValue || ddPersonPenaltyPenalty.value == "")
    {
        res = false;

        if (ddPersonPenaltyPenalty.disabled == true || ddPersonPenaltyPenalty.style.display == "none")
            notValidFields.push("Наказание");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Наказание") + "<br />";
    }

    //2  txtPersonPenaltyDateImposed
    if (txtPersonPenaltyDateImposed.value.Trim() == "")
    {
        res = false;

        if (txtPersonPenaltyDateImposed.disabled == true || spanPersonPenaltyDateImposed.style.display == "none")
            notValidFields.push("Дата (налагане наказание)");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Дата (налагане наказание)") + "<br />";
    }
    else
    {
        if (!IsValidDate(txtPersonPenaltyDateImposed.value))
        {
            res = false;
            lblMessage.innerHTML += GetErrorMessageDate("Дата (налагане наказание)") + "<br />";
        }
    }


    //3 Penalty
    if (ddPersonPenaltyMilitaryCommanderRankCodeImposed.value == optionChooseOneValue)
    {
        res = false;

        if (ddPersonPenaltyMilitaryCommanderRankCodeImposed.disabled == true || ddPersonPenaltyMilitaryCommanderRankCodeImposed.style.display == "none")
            notValidFields.push("Наложил наказанието");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Наложил наказанието") + "<br />";
    }

    //Validate other fields - No  fields to validate
    var txtPersonPenaltyDateCanceled = document.getElementById("txtPersonPenaltyDateCanceled");

    //    if (txtPersonPenaltyDateCanceled != undefined)
    //    {
    if (!txtPersonPenaltyDateCanceled.value.Trim() == "")
    {
        if (!IsValidDate(txtPersonPenaltyDateCanceled.value))
        {
            res = false;
            lblMessage.innerHTML += GetErrorMessageDate("Дата (отмяна наказание)") + "<br />";
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

// Delete a particular Penalty record
function DeletePenalty(PenaltyId)
{
    ClearAllMessages();

    YesNoDialog("Желаете ли да изтриете наказанието?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "AddEditReservist_MilitaryService.aspx?AjaxMethod=JSDeletePenalty";

        var params = "PenaltyId=" + PenaltyId;
        params += "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

        function response_handler(xml)
        {
            var status = xmlValue(xml, "status")

            if (status == "OK")
            {
                var refreshedPositionsTable = xmlValue(xml, "refreshedPenaltyTable");

                document.getElementById("tblPenalty").innerHTML = refreshedPositionsTable;

                document.getElementById("lblMessagePenalty").className = "SuccessText";
                document.getElementById("lblMessagePenalty").innerHTML = "Званието е изтрито успешно";

            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}






// 4.----------- Table Contract -------------------

//Load the Contract table on demand
function lnkContract_Click()
{
    LoadContracts();
}

function LoadContracts()
{
    //If already loaded then do not load
    if (document.getElementById("hdnContractLoaded").value == "1")
        return;

    ClearAllMessages();

    document.getElementById("imgLoadingContract").style.visibility = "";

    var url = "AddEditReservist_MilitaryService.aspx?AjaxMethod=JSLoadContracts";
    var params = "";
    params += "ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

    var hdnIsPreview = document.getElementById(hdnIsPreviewClientID).value;
    if (hdnIsPreview == 1)
        params += "&Preview=1";
    
    function LoadContract_CallBack(xml)
    {
        var tableHTML = xmlValue(xml, "tableHTML");
        var lightBoxHTML = xmlValue(xml, "lightBoxHTML");

        document.getElementById("tblContract").innerHTML = tableHTML;
        document.getElementById("lboxContract").innerHTML = lightBoxHTML;

        RefreshUIItems(xml);
        document.getElementById("imgLoadingContract").style.visibility = "hidden";

        //Mark as already loaded
        document.getElementById("hdnContractLoaded").value = "1";
    }

    var myAJAX = new AJAX(url, true, params, LoadContract_CallBack);
    myAJAX.Call();
}

//Open the light-box for adding a new record in the Contract table
function NewContract()
{
    ShowAddEditContractLightBox(0);
}

//Open the light-box for editing a record in the Contract table
function EditContract(ContractId)
{
    ShowAddEditContractLightBox(ContractId);
}

function ShowAddEditContractLightBox(ContractId)
{
    ClearAllMessages();

    //Set DateTime Pickers
    RefreshDatePickers();

    document.getElementById("hdnContractID").value = ContractId;

    //New record
    if (ContractId == 0)
    {
        document.getElementById("lblAddEditContractTitle").innerHTML = "Въвеждане на нов договор";


        // if (document.getElementById("ddPersonContractDocumentType") != undefined)
        document.getElementById("ddPersonContractDocumentType").value = optionChooseOneValue;

        // if (document.getElementById("txtPersonContractNumber") != undefined)
        document.getElementById("txtPersonContractNumber").value = "";

        //  if (document.getElementById("txtPersonContractDateWhen") != undefined)
        document.getElementById("txtPersonContractDateWhen").value = "";

        // if (document.getElementById("txtPersonContractDatePeriod") != undefined)
        document.getElementById("txtPersonContractDatePeriod").value = "";

        document.getElementById("ddPersonContractDuration").value = optionChooseOneValue;
        document.getElementById("txtPersonMilitaryServiceTo").value = "";

        // clean message label in the light box and hide it
        document.getElementById("spanAddEditContractLightBox").style.display = "none";
        document.getElementById("spanAddEditContractLightBox").innerHTML = "";

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("lboxContract").style.display = "";
        CenterLightBox("lboxContract");
    }
    else //Edit Position
    {
        document.getElementById("lblAddEditContractTitle").innerHTML = "Редактиране на договор";

        var url = "AddEditReservist_MilitaryService.aspx?AjaxMethod=JSLoadContract";

        var params = "ContractId=" + ContractId;

        function response_handler(xml)
        {
            var personContract = xml.getElementsByTagName("personContract")[0];

            var PersonContractDocumentTypeKey = xmlValue(personContract, "PersonContractDocumentTypeKey");
            var PersonContractNumber = xmlValue(personContract, "PersonContractNumber");
            var PersonContractDateWhen = xmlValue(personContract, "PersonContractDateWhen");
            var PersonContractDatePeriod = xmlValue(personContract, "PersonContractDatePeriod");
            var PersonContractDurationKey = xmlValue(personContract, "PersonContractDurationKey");
            var PersonMilitaryServiceTo = xmlValue(personContract, "PersonMilitaryServiceTo"); 
            
            // if (document.getElementById("ddPersonContractDocumentType") != undefined)
            document.getElementById("ddPersonContractDocumentType").value = PersonContractDocumentTypeKey;

            //  if (document.getElementById("txtPersonContractNumber") != undefined)
            document.getElementById("txtPersonContractNumber").value = PersonContractNumber;

            //  if (document.getElementById("txtPersonContractDateWhen") != undefined)
            document.getElementById("txtPersonContractDateWhen").value = PersonContractDateWhen;

            //  if (document.getElementById("txtPersonContractDatePeriod") != undefined)
            document.getElementById("txtPersonContractDatePeriod").value = PersonContractDatePeriod;

            document.getElementById("ddPersonContractDuration").value = PersonContractDurationKey;
            document.getElementById("txtPersonMilitaryServiceTo").value = PersonMilitaryServiceTo;

            // clean message label in the light box and hide it
            document.getElementById("spanAddEditContractLightBox").style.display = "none";
            document.getElementById("spanAddEditContractLightBox").innerHTML = "";

            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("lboxContract").style.display = "";
            CenterLightBox("lboxContract");
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Close the light-box
function HideAddEditContractLightBox()
{
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("lboxContract").style.display = "none";
}

//Save Add/Edit Contract
function SaveAddEditContractLightBox()
{
    if (ValidateAddEditContract())
    {
        var url = "AddEditReservist_MilitaryService.aspx?AjaxMethod=JSSaveContract";

        //Set variable for fields thta can be not bound from UI Items logic but Save/Update can performe
        var DatePeriod = "";
        if (document.getElementById("txtPersonContractDatePeriod") != undefined)
            DatePeriod = document.getElementById("txtPersonContractDatePeriod").value;

        var personMilitaryServiceTo = "";
        if (document.getElementById("txtPersonMilitaryServiceTo") != undefined)
            personMilitaryServiceTo = document.getElementById("txtPersonMilitaryServiceTo").value;

        var params = "ContractId=" + document.getElementById("hdnContractID").value +
                     "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value +
                     "&PersonContractDocumentTypeKey=" + document.getElementById("ddPersonContractDocumentType").value +
                     "&PersonContractNumber=" + document.getElementById("txtPersonContractNumber").value +
                     "&PersonContractDateWhen=" + document.getElementById("txtPersonContractDateWhen").value +
                     "&PersonContractDatePeriod=" + DatePeriod +
                     "&PersonContractDurationKey=" + document.getElementById("ddPersonContractDuration").value +
                     "&PersonMilitaryServiceTo=" + personMilitaryServiceTo;

        function response_handler(xml)
        {
            var status = xmlValue(xml, "status");

            if (status == "OK")
            {
                var refreshedPositionsTable = xmlValue(xml, "refreshedContractTable");

                document.getElementById("tblContract").innerHTML = refreshedPositionsTable;

                document.getElementById("lblMessageContract").className = "SuccessText";
                document.getElementById("lblMessageContract").innerHTML = document.getElementById("hdnContractID").value == "0" ? "Договора е добавен успешно" : "Договора е редактиран успешно";

                HideAddEditContractLightBox();
            }
            else
            {
                document.getElementById("spanAddEditContractLightBox").className = "ErrorText";
                document.getElementById("spanAddEditContractLightBox").innerHTML = status;
                document.getElementById("spanAddEditContractLightBox").style.display = "";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Client validation of Add/Edit Contractlight-box
function ValidateAddEditContract()
{
    var res = true;

    var lblMessage = document.getElementById("spanAddEditContractLightBox");
    lblMessage.innerHTML = "";

    var notValidFields = new Array();


    var ddPersonContractDocumentType = document.getElementById("ddPersonContractDocumentType");
    var txtPersonContractNumber = document.getElementById("txtPersonContractNumber");
    var txtPersonContractDateWhen = document.getElementById("txtPersonContractDateWhen");

    //Validate Mandatory Fields

    //1 ddPersonContractDocumentType
    //? In this case  ddPersonContractContract.value="" Why not "-1"
    if (ddPersonContractDocumentType.value == optionChooseOneValue || ddPersonContractDocumentType.value == "")
    {
        res = false;

        if (ddPersonContractDocumentType.disabled == true || ddPersonContractDocumentType.style.display == "none")
            notValidFields.push("Вид документ");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Вид документ") + "<br />";
    }



    if (txtPersonContractNumber.value.Trim() == "")
    {
        res = false;

        if (txtPersonContractNumber.disabled == true || txtPersonContractNumber.style.display == "none")
            notValidFields.push("Номер");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Номер") + "<br />";
    }


    //3  txtPersonContractDateWhen
    if (txtPersonContractDateWhen.value.Trim() == "")
    {
        res = false;

        if (txtPersonContractDateWhen.disabled == true || spanPersonContractDateWhen.style.display == "none")
            notValidFields.push("Дата");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Дата") + "<br />";
    }
    else
    {
        if (!IsValidDate(txtPersonContractDateWhen.value))
        {
            res = false;
            lblMessage.innerHTML += GetErrorMessageDate("Дата") + "<br />";
        }
    }


    //Validate other fields 
    var txtPersonContractDatePeriod = document.getElementById("txtPersonContractDatePeriod");

    if (!txtPersonContractDatePeriod.value.Trim() == "")
    {
        if (!IsValidDate(txtPersonContractDatePeriod.value))
        {
            res = false;
            lblMessage.innerHTML += GetErrorMessageDate("Изтича на") + "<br />";
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

// Delete a particular Contract record
function DeleteContract(ContractId)
{
    ClearAllMessages();

    YesNoDialog("Желаете ли да изтриете договора?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "AddEditReservist_MilitaryService.aspx?AjaxMethod=JSDeleteContract";

        var params = "ContractId=" + ContractId;
        params += "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

        function response_handler(xml)
        {
            var status = xmlValue(xml, "status")

            if (status == "OK")
            {
                var refreshedPositionsTable = xmlValue(xml, "refreshedContractTable");

                document.getElementById("tblContract").innerHTML = refreshedPositionsTable;

                document.getElementById("lblMessageContract").className = "SuccessText";
                document.getElementById("lblMessageContract").innerHTML = "Договора е изтрит успешно";

            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}



// 5.----------- Table PreviousPosition -------------------

//Load the PreviousPosition table on demand
function lnkPreviousPosition_Click()
{
    LoadPreviousPositions();
}

function LoadPreviousPositions()
{
    //If already loaded then do not load
    if (document.getElementById("hdnPreviousPositionLoaded").value == "1")
        return;

    ClearAllMessages();

    document.getElementById("imgLoadingPreviousPosition").style.visibility = "";

    var url = "AddEditReservist_MilitaryService.aspx?AjaxMethod=JSLoadPreviousPositions";
    var params = "";
    params += "ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

    var hdnIsPreview = document.getElementById(hdnIsPreviewClientID).value;
    if (hdnIsPreview == 1)
        params += "&Preview=1";
    
    function LoadPreviousPosition_CallBack(xml)
    {
        var tableHTML = xmlValue(xml, "tableHTML");
        var lightBoxHTML = xmlValue(xml, "lightBoxHTML");

        document.getElementById("tblPreviousPosition").innerHTML = tableHTML;
        document.getElementById("lboxPreviousPosition").innerHTML = lightBoxHTML;

        RefreshUIItems(xml);
        document.getElementById("imgLoadingPreviousPosition").style.visibility = "hidden";

        //Mark as already loaded
        document.getElementById("hdnPreviousPositionLoaded").value = "1";
    }

    var myAJAX = new AJAX(url, true, params, LoadPreviousPosition_CallBack);
    myAJAX.Call();
}

//Open the light-box for adding a new record in the PreviousPosition table
function NewPreviousPosition()
{
    ShowAddEditPreviousPositionLightBox(0);
}

//Open the light-box for editing a record in the PreviousPosition table
function EditPreviousPosition(PreviousPositionId)
{
    ShowAddEditPreviousPositionLightBox(PreviousPositionId);
}

function ShowAddEditPreviousPositionLightBox(PreviousPositionId)
{
    ClearAllMessages();

    //Set DateTime Pickers
    RefreshDatePickers();

    document.getElementById("hdnPreviousPositionID").value = PreviousPositionId;

    //New record
    if (PreviousPositionId == 0)
    {
        document.getElementById("lblAddEditPreviousPositionTitle").innerHTML = "Въвеждане на нова заемана длъжност";


        // if (document.getElementById("txtPersonPreviousPositionCode") != undefined)
        document.getElementById("txtPersonPreviousPositionCode").value = "";

        // if (document.getElementById("txtPersonPreviousPositionPositionName") != undefined)
        document.getElementById("txtPersonPreviousPositionPositionName").value = "";

        // if (document.getElementById("ddPersonPreviousPositionType") != undefined)
        document.getElementById("ddPersonPreviousPositionType").value = "";

        //  if (document.getElementById("ddPersonPreviousPositionKind") != undefined)
        document.getElementById("ddPersonPreviousPositionKind").value = "";

        //  if (document.getElementById("ddPersonPreviousPositionMilitaryCategory") != undefined)
        document.getElementById("ddPersonPreviousPositionMilitaryCategory").value = "";

        //   if (document.getElementById("ddPersonPreviousPositionMilitaryReportSpecialityType") != undefined)
        document.getElementById("ddPersonPreviousPositionMilitaryReportSpecialityType").value = "";

        //   if (document.getElementById("ddPersonPreviousPositionMilitarySpecialities") != undefined)
        ClearSelectList(document.getElementById("ddPersonPreviousPositionMilitarySpecialities"), true);

        //   if (document.getElementById("chkboxPersonPreviousPositionMission") != undefined)
        document.getElementById("chkboxPersonPreviousPositionMission").checked = false;

        //   if (document.getElementById("txtPersonPreviousPositionVaccAnnNum") != undefined)
        document.getElementById("txtPersonPreviousPositionVaccAnnNum").value = "";

        //  if (document.getElementById("txtPersonPreviousPositionVaccAnnDateVacAnn") != undefined)
        document.getElementById("txtPersonPreviousPositionVaccAnnDateVacAnn").value = "";


        //   if (document.getElementById("txtPersonPreviousPositionVaccAnnDateWhen") != undefined)
        document.getElementById("txtPersonPreviousPositionVaccAnnDateWhen").value = "";

        //   if (document.getElementById("txtPersonPreviousPositionVaccAnnDateEnd") != undefined)
        document.getElementById("txtPersonPreviousPositionVaccAnnDateEnd").value = "";

        //    if (document.getElementById("ddPersonPreviousPositionMilitaryCommanderRank") != undefined)
        document.getElementById("ddPersonPreviousPositionMilitaryCommanderRank").value = "";

        ///    if (document.getElementById("hdnPersonPreviousPositionMilitaryUnit") != undefined)
        //   {
        MilitaryUnitSelectorUtil.SetSelectedValue("itmsPersonPreviousPositionMilUnitSelector", "-1");
        MilitaryUnitSelectorUtil.SetSelectedText("itmsPersonPreviousPositionMilUnitSelector", "");
        //    }

        //   if (document.getElementById("txtPersonPreviousPositionOrganisationUnit") != undefined)
        document.getElementById("txtPersonPreviousPositionOrganisationUnit").value = "";


        //  if (document.getElementById("ddPersonPreviousPositionRegion") != undefined)
        document.getElementById("ddPersonPreviousPositionRegion").value = "";

        //   if (document.getElementById("ddPersonPreviousPositionMunicipality") != undefined)
        ClearSelectList(document.getElementById("ddPersonPreviousPositionMunicipality"), true);

        //    if (document.getElementById("ddPersonPreviousPositionCity") != undefined)
        ClearSelectList(document.getElementById("ddPersonPreviousPositionCity"), true);

        // clean message label in the light box and hide it
        document.getElementById("spanAddEditPreviousPositionLightBox").style.display = "none";
        document.getElementById("spanAddEditPreviousPositionLightBox").innerHTML = "";

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("lboxPreviousPosition").style.display = "";
        CenterLightBox("lboxPreviousPosition");
    }
    else //Edit Position
    {
        document.getElementById("lblAddEditPreviousPositionTitle").innerHTML = "Редактиране на заемана длъжност";

        var url = "AddEditReservist_MilitaryService.aspx?AjaxMethod=JSLoadPreviousPosition";

        var params = "PreviousPositionId=" + PreviousPositionId;

        function response_handler(xml)
        {
            var personPreviousPosition = xml.getElementsByTagName("personPreviousPosition")[0];

            var PersonPreviousPositionCode = xmlValue(personPreviousPosition, "PersonPreviousPositionCode");
            //if (document.getElementById("txtPersonPreviousPositionCode") != undefined)
            document.getElementById("txtPersonPreviousPositionCode").value = PersonPreviousPositionCode;

            var PersonPreviousPositionPositionName = xmlValue(personPreviousPosition, "PersonPreviousPositionPositionName");
            // if (document.getElementById("txtPersonPreviousPositionPositionName") != undefined)
            document.getElementById("txtPersonPreviousPositionPositionName").value = PersonPreviousPositionPositionName;


            var PersonPreviousPositionTypeKey = xmlValue(personPreviousPosition, "PersonPreviousPositionTypeKey");
            //   if (document.getElementById("ddPersonPreviousPositionType") != undefined)
            document.getElementById("ddPersonPreviousPositionType").value = PersonPreviousPositionTypeKey;



            var PersonPreviousPositionKindKey = xmlValue(personPreviousPosition, "PersonPreviousPositionKindKey");
            // if (document.getElementById("ddPersonPreviousPositionKind") != undefined)
            document.getElementById("ddPersonPreviousPositionKind").value = PersonPreviousPositionKindKey;




            var PersonPreviousPositionMilitaryCategoryId = xmlValue(personPreviousPosition, "PersonPreviousPositionMilitaryCategoryId")
            //   if (document.getElementById("ddPersonPreviousPositionMilitaryCategory") != undefined)
            document.getElementById("ddPersonPreviousPositionMilitaryCategory").value = PersonPreviousPositionMilitaryCategoryId;


            //use to bind ddl TypeVOS and VOS
            var PersonPreviousPositionMilReportingSpecialityTypeCode = xmlValue(personPreviousPosition, "PersonPreviousPositionMilReportingSpecialityTypeCode")
            var PersonPreviousPositionMilReportingSpecialityId = xmlValue(personPreviousPosition, "PersonPreviousPositionMilReportingSpecialityId")
            
            if (PersonPreviousPositionMilReportingSpecialityTypeCode != "")
            {
                //                if (document.getElementById("ddPersonPreviousPositionMilitaryReportSpecialityType") != undefined)
                //                {
                document.getElementById("ddPersonPreviousPositionMilitaryReportSpecialityType").value = PersonPreviousPositionMilReportingSpecialityTypeCode;
                ddPersonPreviousPositionMilitaryReportSpecialityTypeChanged(PersonPreviousPositionMilReportingSpecialityId);
                //}
            }



            var PersonPreviousPositionMission = xmlValue(personPreviousPosition, "PersonPreviousPositionMission");

            //            if (document.getElementById("chkboxPersonPreviousPositionMission") != undefined)
            //            {
            if (PersonPreviousPositionMission == "True")
            {
                document.getElementById("chkboxPersonPreviousPositionMission").checked = true;
            }
            else
            {
                document.getElementById("chkboxPersonPreviousPositionMission").checked = false;
            }
            // }


            var PersonPreviousPositionVaccAnnNum = xmlValue(personPreviousPosition, "PersonPreviousPositionVaccAnnNum");
            //  if (document.getElementById("txtPersonPreviousPositionVaccAnnNum") != undefined)
            document.getElementById("txtPersonPreviousPositionVaccAnnNum").value = PersonPreviousPositionVaccAnnNum;


            var PersonPreviousPositionVaccAnnDateVacAnn = xmlValue(personPreviousPosition, "PersonPreviousPositionVaccAnnDateVacAnn");
            //    if (document.getElementById("txtPersonPreviousPositionVaccAnnDateVacAnn") != undefined)
            document.getElementById("txtPersonPreviousPositionVaccAnnDateVacAnn").value = PersonPreviousPositionVaccAnnDateVacAnn;


            var PersonPreviousPositionVaccAnnDateWhen = xmlValue(personPreviousPosition, "PersonPreviousPositionVaccAnnDateWhen");
            // if (document.getElementById("txtPersonPreviousPositionVaccAnnDateWhen") != undefined)
            document.getElementById("txtPersonPreviousPositionVaccAnnDateWhen").value = PersonPreviousPositionVaccAnnDateWhen;


            var PersonPreviousPositionVaccAnnDateEnd = xmlValue(personPreviousPosition, "PersonPreviousPositionVaccAnnDateEnd");
            // if (document.getElementById("txtPersonPreviousPositionVaccAnnDateEnd") != undefined)
            document.getElementById("txtPersonPreviousPositionVaccAnnDateEnd").value = PersonPreviousPositionVaccAnnDateEnd;


            var PersonPreviousPositionMilitaryCommanderRankCode = xmlValue(personPreviousPosition, "PersonPreviousPositionMilitaryCommanderRankCode");
            //        if (document.getElementById("ddPersonPreviousPositionMilitaryCommanderRank") != undefined)
            document.getElementById("ddPersonPreviousPositionMilitaryCommanderRank").value = PersonPreviousPositionMilitaryCommanderRankCode;


            //Use to set value in ItemSelector
            var PersonPreviousPositionMilitaryUnitId = xmlValue(personPreviousPosition, "PersonPreviousPositionMilitaryUnitId");

            //            if (document.getElementById("hdnPersonPreviousPositionMilitaryUnit") != undefined)
            //            {
            MilitaryUnitSelectorUtil.SetSelectedValue("itmsPersonPreviousPositionMilUnitSelector", PersonPreviousPositionMilitaryUnitId);

            var PersonPreviousPositionMilitaryUnitVpnName = xmlValue(personPreviousPosition, "PersonPreviousPositionMilitaryUnitVpnName");
            MilitaryUnitSelectorUtil.SetSelectedText("itmsPersonPreviousPositionMilUnitSelector", PersonPreviousPositionMilitaryUnitVpnName);
            //    }



            var PersonPreviousPositionOrganisationUnit = xmlValue(personPreviousPosition, "PersonPreviousPositionOrganisationUnit");
            //  if (document.getElementById("txtPersonPreviousPositionOrganisationUnit") != undefined)
            document.getElementById("txtPersonPreviousPositionOrganisationUnit").value = PersonPreviousPositionOrganisationUnit;


            //Use to bind ddl with Regions, Municipalities, Cities
            var PersonPreviousPositionGarrisonRegionId = xmlValue(personPreviousPosition, "PersonPreviousPositionGarrisonRegionId");
            var PersonPreviousPositionGarrisonMunicipalityId = xmlValue(personPreviousPosition, "PersonPreviousPositionGarrisonMunicipalityId");
            var PersonPreviousPositionGarrisonCityId = xmlValue(personPreviousPosition, "PersonPreviousPositionGarrisonCityId");

            //            if (document.getElementById("ddPersonPreviousPositionCity") != undefined)
            //            {
            document.getElementById("ddPersonPreviousPositionRegion").value = PersonPreviousPositionGarrisonRegionId;

            ddPreviousPositionRegion_Changed(PersonPreviousPositionGarrisonMunicipalityId, PersonPreviousPositionGarrisonCityId);

            //  }

            // clean message label in the light box and hide it
            document.getElementById("spanAddEditPreviousPositionLightBox").style.display = "none";
            document.getElementById("spanAddEditPreviousPositionLightBox").innerHTML = "";

            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("lboxPreviousPosition").style.display = "";
            CenterLightBox("lboxPreviousPosition");
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Close the light-box
function HideAddEditPreviousPositionLightBox()
{
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("lboxPreviousPosition").style.display = "none";
}

//Save Add/Edit PreviousPosition
function SaveAddEditPreviousPositionLightBox()
{
    if (ValidateAddEditPreviousPosition())
    {
        var url = "AddEditReservist_MilitaryService.aspx?AjaxMethod=JSSavePreviousPosition";

        //Set variable for fields thta can be not bound from UI Items logic but Save/Update can performe
        var ddPersonPreviousPositionKind = "";
        if (document.getElementById("ddPersonPreviousPositionKind") != undefined)
            ddPersonPreviousPositionKind = document.getElementById("ddPersonPreviousPositionKind").value;
        if (ddPersonPreviousPositionKind == "-1")
            ddPersonPreviousPositionKind = "";


        var ddPersonPreviousPositionMilitaryCategory = "";
        if (document.getElementById("ddPersonPreviousPositionMilitaryCategory") != undefined)
            ddPersonPreviousPositionMilitaryCategory = document.getElementById("ddPersonPreviousPositionMilitaryCategory").value;
        if (ddPersonPreviousPositionMilitaryCategory == "-1")
            ddPersonPreviousPositionMilitaryCategory = "";

        //VOS
        var ddPersonPreviousPositionMilitarySpecialities = "";
        if (document.getElementById("ddPersonPreviousPositionMilitarySpecialities") != undefined)
            ddPersonPreviousPositionMilitarySpecialities = document.getElementById("ddPersonPreviousPositionMilitarySpecialities").value;
        if (ddPersonPreviousPositionMilitarySpecialities == "-1")
            ddPersonPreviousPositionMilitarySpecialities = "";

        //MilitaryCommanderRank
        var ddPersonPreviousPositionMilitaryCommanderRank = "";
        if (document.getElementById("ddPersonPreviousPositionMilitaryCommanderRank") != undefined)
            ddPersonPreviousPositionMilitaryCommanderRank = document.getElementById("ddPersonPreviousPositionMilitaryCommanderRank").value;
        if (ddPersonPreviousPositionMilitaryCommanderRank == "-1")
            ddPersonPreviousPositionMilitaryCommanderRank = "";

        var par = MilitaryUnitSelectorUtil.GetSelectedValue("itmsPersonPreviousPositionMilUnitSelector");

        var params = "PreviousPositionId=" + document.getElementById("hdnPreviousPositionID").value +
                     "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value +

                     "&txtPersonPreviousPositionCode=" + document.getElementById("txtPersonPreviousPositionCode").value +
                     "&txtPersonPreviousPositionPositionName=" + document.getElementById("txtPersonPreviousPositionPositionName").value +
                     "&ddPersonPreviousPositionType=" + document.getElementById("ddPersonPreviousPositionType").value +

                     "&ddPersonPreviousPositionKind=" + ddPersonPreviousPositionKind +
                     "&ddPersonPreviousPositionMilitaryCategory=" + ddPersonPreviousPositionMilitaryCategory +
                     "&ddPersonPreviousPositionMilitarySpecialities=" + document.getElementById("ddPersonPreviousPositionMilitarySpecialities").value +

                     "&chkboxPersonPreviousPositionMission=" + document.getElementById("chkboxPersonPreviousPositionMission").checked +
                     "&txtPersonPreviousPositionVaccAnnNum=" + document.getElementById("txtPersonPreviousPositionVaccAnnNum").value +
                     "&txtPersonPreviousPositionVaccAnnDateVacAnn=" + document.getElementById("txtPersonPreviousPositionVaccAnnDateVacAnn").value +

                     "&txtPersonPreviousPositionVaccAnnDateWhen=" + document.getElementById("txtPersonPreviousPositionVaccAnnDateWhen").value +
                     "&txtPersonPreviousPositionVaccAnnDateEnd=" + document.getElementById("txtPersonPreviousPositionVaccAnnDateEnd").value +
                     "&ddPersonPreviousPositionMilitaryCommanderRank=" + ddPersonPreviousPositionMilitaryCommanderRank +

                     "&itmsPersonPreviousPositionMilitaryUnitId=" + MilitaryUnitSelectorUtil.GetSelectedValue("itmsPersonPreviousPositionMilUnitSelector") +

                     "&txtPersonPreviousPositionOrganisationUnit=" + document.getElementById("txtPersonPreviousPositionOrganisationUnit").value +
                     "&PersonPreviousPositionCityId=" + document.getElementById("ddPersonPreviousPositionCity").value;

        function response_handler(xml)
        {
            var status = xmlValue(xml, "status");

            if (status == "OK")
            {
                var refreshedPositionsTable = xmlValue(xml, "refreshedPreviousPositionTable");

                document.getElementById("tblPreviousPosition").innerHTML = refreshedPositionsTable;

                document.getElementById("lblMessagePreviousPosition").className = "SuccessText";
                document.getElementById("lblMessagePreviousPosition").innerHTML = document.getElementById("hdnPreviousPositionID").value == "0" ? "Заеманата длъжност е добавена успешно" : "Заеманата длъжност е редактирана успешно";

                HideAddEditPreviousPositionLightBox();
            }
            else
            {
                document.getElementById("spanAddEditPreviousPositionLightBox").className = "ErrorText";
                document.getElementById("spanAddEditPreviousPositionLightBox").innerHTML = status;
                document.getElementById("spanAddEditPreviousPositionLightBox").style.display = "";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Client validation of Add/Edit PreviousPositionlight-box
function ValidateAddEditPreviousPosition()
{
    var res = true;

    var lblMessage = document.getElementById("spanAddEditPreviousPositionLightBox");
    lblMessage.innerHTML = "";

    var notValidFields = new Array();

    //Validate Mandatory Fields

    //1. PersonPreviousPositionNumber
    var txtPersonPreviousPositionCode = document.getElementById("txtPersonPreviousPositionCode");
    if (txtPersonPreviousPositionCode.value.Trim() == "")
    {
        res = false;

        if (txtPersonPreviousPositionCode.disabled == true || txtPersonPreviousPositionCode.style.display == "none")
            notValidFields.push("Код длъжност");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Код длъжност") + "<br />";
    }



    //2. PersonPreviousPositionPositionName
    var txtPersonPreviousPositionPositionName = document.getElementById("txtPersonPreviousPositionPositionName");
    if (txtPersonPreviousPositionPositionName.value.Trim() == "")
    {
        res = false;

        if (txtPersonPreviousPositionPositionName.disabled == true || txtPersonPreviousPositionPositionName.style.display == "none")
            notValidFields.push("Длъжност");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Длъжност") + "<br />";
    }


    //3 ddPersonPreviousPositionDocumentType
    var ddPersonPreviousPositionType = document.getElementById("ddPersonPreviousPositionType");
    if (ddPersonPreviousPositionType.value == optionChooseOneValue || ddPersonPreviousPositionType.value == "")
    {
        res = false;

        if (ddPersonPreviousPositionType.disabled == true || ddPersonPreviousPositionType.style.display == "none")
            notValidFields.push("Заемал длъжността като");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Заемал длъжността като") + "<br />";
    }


    //4. PersonPreviousPositionVaccAnnNum
    var txtPersonPreviousPositionVaccAnnNum = document.getElementById("txtPersonPreviousPositionVaccAnnNum");
    if (txtPersonPreviousPositionVaccAnnNum.value.Trim() == "")
    {
        res = false;

        if (txtPersonPreviousPositionVaccAnnNum.disabled == true || txtPersonPreviousPositionVaccAnnNum.style.display == "none")
            notValidFields.push("Заповед номер");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Заповед номер") + "<br />";
    }



    //5  PersonPreviousPositionVaccAnnDateVacAnn
    var txtPersonPreviousPositionVaccAnnDateVacAnn = document.getElementById("txtPersonPreviousPositionVaccAnnDateVacAnn");
    if (txtPersonPreviousPositionVaccAnnDateVacAnn.value.Trim() == "")
    {
        res = false;

        if (txtPersonPreviousPositionVaccAnnDateVacAnn.disabled == true || spanPersonPreviousPositionVaccAnnDateVacAnn.style.display == "none")
            notValidFields.push("Заповед дата");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Заповед дата") + "<br />";
    }
    else
    {
        if (!IsValidDate(txtPersonPreviousPositionVaccAnnDateVacAnn.value))
        {
            res = false;
            lblMessage.innerHTML += GetErrorMessageDate("Заповед дата") + "<br />";
        }
    }

    //6.  PersonPreviousPositionVaccAnnDateWhen
    var txtPersonPreviousPositionVaccAnnDateWhen = document.getElementById("txtPersonPreviousPositionVaccAnnDateWhen");
    if (txtPersonPreviousPositionVaccAnnDateWhen.value.Trim() == "")
    {
        res = false;

        if (txtPersonPreviousPositionVaccAnnDateWhen.disabled == true || spanPersonPreviousPositionVaccAnnDateWhen.style.display == "none")
            notValidFields.push("В сила от");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("В сила от") + "<br />";
    }
    else
    {
        if (!IsValidDate(txtPersonPreviousPositionVaccAnnDateWhen.value))
        {
            res = false;
            lblMessage.innerHTML += GetErrorMessageDate("В сила от") + "<br />";
        }
    }


    //7. PersonPreviousPositionMilitaryUnitVpn and PersonPreviousPositionMilitaryUnitName
    var hdnPersonPreviousPositionMilitaryUnit = document.getElementById("hdnPersonPreviousPositionMilitaryUnit");
    if (hdnPersonPreviousPositionMilitaryUnit == undefined)
    {
        res = false;
        notValidFields.push("ВПН/Структура");
    }
    else
    {
        // MilitaryUnitSelectorUtil.GetSelectedValue
        //if (document.getElementById("hdn" + "itmsPersonPreviousPositionMilUnitSelector").value == "-1")
        if (MilitaryUnitSelectorUtil.GetSelectedValue("itmsPersonPreviousPositionMilUnitSelector") == "-1")
        {
            res = false;
            if (MilitaryUnitSelectorUtil.IsDisabled("itmsPersonPreviousPositionMilUnitSelector") == true || MilitaryUnitSelectorUtil.IsHidden("itmsPersonPreviousPositionMilUnitSelector"))
                notValidFields.push("ВПН/Структура");
            else
                lblMessage.innerHTML += GetErrorMessageMandatory("ВПН/Структура") + "<br />";
        }
    }

    //8. PersonPreviousPositionOrganisationUnit
    var txtPersonPreviousPositionOrganisationUnit = document.getElementById("txtPersonPreviousPositionOrganisationUnit");
    if (txtPersonPreviousPositionOrganisationUnit.value.Trim() == "")
    {
        res = false;

        if (txtPersonPreviousPositionOrganisationUnit.disabled == true || txtPersonPreviousPositionOrganisationUnit.style.display == "none")
            notValidFields.push("Организационна единица");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Организационна единица") + "<br />";
    }



    //9 Garrison
    var ddPersonPreviousPositionRegion = document.getElementById("ddPersonPreviousPositionRegion");
    if (ddPersonPreviousPositionRegion.value == optionChooseOneValue || ddPersonPreviousPositionRegion.value == "")
    {
        res = false;

        if (ddPersonPreviousPositionRegion.disabled == true || ddPersonPreviousPositionRegion.style.display == "none")
            notValidFields.push("Гарнизон");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Гарнизон") + "<br />";
    }


    //Validate other fields - No  fields to validate

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

// Delete a particular PreviousPosition record
function DeletePreviousPosition(PreviousPositionId)
{
    ClearAllMessages();

    YesNoDialog("Желаете ли да изтриете заеманата длъжност?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "AddEditReservist_MilitaryService.aspx?AjaxMethod=JSDeletePreviousPosition";

        var params = "PreviousPositionId=" + PreviousPositionId;
        params += "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

        function response_handler(xml)
        {
            var status = xmlValue(xml, "status")

            if (status == "OK")
            {
                var refreshedPositionsTable = xmlValue(xml, "refreshedPreviousPositionTable");

                document.getElementById("tblPreviousPosition").innerHTML = refreshedPositionsTable;

                document.getElementById("lblMessagePreviousPosition").className = "SuccessText";
                document.getElementById("lblMessagePreviousPosition").innerHTML = "Заеманата длъжност е изтрита успешно";

            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Addition method

// Handler of onchange event of VoluntaryMilRepSpecType dropdown in the Light Box
function ddPersonPreviousPositionMilitaryReportSpecialityTypeChanged(selectedItem)
{
    var url = "AddEditReservist_MilitaryService.aspx?AjaxMethod=JSLoadMilRepSpecs";

    var params = "MilRepSpecTypeID=" + document.getElementById("ddPersonPreviousPositionMilitaryReportSpecialityType").value;

    if (selectedItem != undefined)
        params += "&SelectedItem=" + selectedItem;

    function response_handler(xml)
    {
        ClearSelectList(document.getElementById("ddPersonPreviousPositionMilitarySpecialities"), true);

        var milRepSpecs = xml.getElementsByTagName("m");

        for (var i = 0; i < milRepSpecs.length; i++)
        {
            var id = xmlValue(milRepSpecs[i], "id");
            var name = xmlValue(milRepSpecs[i], "name");

            AddToSelectList(document.getElementById("ddPersonPreviousPositionMilitarySpecialities"), id, name, true);
        };

        if (selectedItem != undefined)
        {
            document.getElementById("ddPersonPreviousPositionMilitarySpecialities").value = selectedItem;
        }
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}



//When changing the PreviousPosition Region then refresh the PreviousPosition Municipality and the PreviousPosition City
function ddPreviousPositionRegion_Changed(MunicipalityItemId, CityItemId)
{
    var ddPersonPreviousPositionRegion = document.getElementById("ddPersonPreviousPositionRegion");
    RepopulatePreviousPositionMunicipality(ddPersonPreviousPositionRegion.value, "ddPersonPreviousPositionMunicipality", MunicipalityItemId, CityItemId);
}

function RepopulatePreviousPositionMunicipality(regionId, ddMunicipalityId, MunicipalityItemId, CityItemId)
{
    var url = "AddEditReservist_MilitaryService.aspx?AjaxMethod=JSRepopulateMunicipality";
    var params = "";
    params += "RegionId=" + regionId;
    
    function RepopulateMunicipality_Callback(xml)
    {
        ClearSelectList(document.getElementById(ddMunicipalityId), true);

        var municipalities = xml.getElementsByTagName("m");

        for (var i = 0; i < municipalities.length; i++)
        {
            var id = xmlValue(municipalities[i], "id");
            var name = xmlValue(municipalities[i], "name");

            AddToSelectList(document.getElementById(ddMunicipalityId), id, name);
        };

        if (MunicipalityItemId != undefined)
        {
            document.getElementById(ddMunicipalityId).value = MunicipalityItemId;
        }
        ddPreviousPositionMunicipality_Changed(CityItemId);

    }

    var myAJAX = new AJAX(url, true, params, RepopulateMunicipality_Callback);
    myAJAX.Call();
}

//When changing the Perm Municipality then refresh the Perm City
function ddPreviousPositionMunicipality_Changed(CityItemId)
{
    var ddPersonPreviousPositionMunicipality = document.getElementById("ddPersonPreviousPositionMunicipality");
    RepopulatePreviousPositionCity(ddPersonPreviousPositionMunicipality.value, "ddPersonPreviousPositionCity", CityItemId);
}



function RepopulatePreviousPositionCity(municipalityId, ddCityId, CityItemId)
{
    var url = "AddEditReservist_MilitaryService.aspx?AjaxMethod=JSRepopulateCity";
    var params = "";
    params += "MunicipalityId=" + municipalityId;
    
    function RepopulateCity_Callback(xml)
    {
        ClearSelectList(document.getElementById(ddCityId), true);

        var cities = xml.getElementsByTagName("c");

        for (var i = 0; i < cities.length; i++)
        {
            var id = xmlValue(cities[i], "id");
            var name = xmlValue(cities[i], "name");

            AddToSelectList(document.getElementById(ddCityId), id, name);
        };

        if (CityItemId != undefined)
        {
            document.getElementById(ddCityId).value = CityItemId;
        }
    }

    var myAJAX = new AJAX(url, true, params, RepopulateCity_Callback);
    myAJAX.Call();
}



// 6.----------- Table Conscription -------------------

//Load the Conscription table on demand
function lnkConscription_Click() {
    LoadConscriptions();
}

function LoadConscriptions() {
    //If already loaded then do not load
    if (document.getElementById("hdnConscriptionLoaded").value == "1")
        return;

    ClearAllMessages();

    document.getElementById("imgLoadingConscription").style.visibility = "";

    var url = "AddEditReservist_MilitaryService.aspx?AjaxMethod=JSLoadConscriptions";
    var params = "";
    params += "ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

    var hdnIsPreview = document.getElementById(hdnIsPreviewClientID).value;
    if (hdnIsPreview == 1)
        params += "&Preview=1";

    function LoadConscription_CallBack(xml) {
        var tableHTML = xmlValue(xml, "tableHTML");
        var lightBoxHTML = xmlValue(xml, "lightBoxHTML");

        document.getElementById("tblConscription").innerHTML = tableHTML;
        document.getElementById("lboxConscription").innerHTML = lightBoxHTML;

        RefreshUIItems(xml);
        document.getElementById("imgLoadingConscription").style.visibility = "hidden";

        //Mark as already loaded
        document.getElementById("hdnConscriptionLoaded").value = "1";
    }

    var myAJAX = new AJAX(url, true, params, LoadConscription_CallBack);
    myAJAX.Call();
}

//Open the light-box for adding a new record in the Conscription table
function NewConscription() {
    ShowAddEditConscriptionLightBox(0);
}

//Open the light-box for editing a record in the Conscription table
function EditConscription(ConscriptionId) {
    ShowAddEditConscriptionLightBox(ConscriptionId);
}

function ShowAddEditConscriptionLightBox(ConscriptionId) {
    ClearAllMessages();

    //Set DateTime Pickers
    RefreshDatePickers();

    document.getElementById("hdnConscriptionID").value = ConscriptionId;

    //New record
    if (ConscriptionId == 0) {
        document.getElementById("lblAddEditConscriptionTitle").innerHTML = "Въвеждане на нова наборна служба";

        document.getElementById("txtPersonConscriptionMilitaryUnit").value = "";
        document.getElementById("txtPersonConscriptionPosition").value = "";
        document.getElementById("txtPersonConscriptionDateFrom").value = "";
        document.getElementById("txtPersonConscriptionDateTo").value = "";

        // clean message label in the light box and hide it
        document.getElementById("spanAddEditConscriptionLightBox").style.display = "none";
        document.getElementById("spanAddEditConscriptionLightBox").innerHTML = "";

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("lboxConscription").style.display = "";
        CenterLightBox("lboxConscription");
    }
    else //Edit Conscription
    {
        document.getElementById("lblAddEditConscriptionTitle").innerHTML = "Редактиране на наборна служба";

        var url = "AddEditReservist_MilitaryService.aspx?AjaxMethod=JSLoadConscription";

        var params = "ConscriptionId=" + ConscriptionId;

        function response_handler(xml) {
            var personConscription = xml.getElementsByTagName("personConscription")[0];

            var PersonConscriptionMilitaryUnit = xmlValue(personConscription, "PersonConscriptionMilitaryUnit");
            var PersonConscriptionPosition = xmlValue(personConscription, "PersonConscriptionPosition");
            var PersonConscriptionDateFrom = xmlValue(personConscription, "PersonConscriptionDateFrom");
            var PersonConscriptionDateTo = xmlValue(personConscription, "PersonConscriptionDateTo");

            document.getElementById("txtPersonConscriptionMilitaryUnit").value = PersonConscriptionMilitaryUnit;
            document.getElementById("txtPersonConscriptionPosition").value = PersonConscriptionPosition;
            document.getElementById("txtPersonConscriptionDateFrom").value = PersonConscriptionDateFrom;
            document.getElementById("txtPersonConscriptionDateTo").value = PersonConscriptionDateTo;

            // clean message label in the light box and hide it
            document.getElementById("spanAddEditConscriptionLightBox").style.display = "none";
            document.getElementById("spanAddEditConscriptionLightBox").innerHTML = "";

            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("lboxConscription").style.display = "";
            CenterLightBox("lboxConscription");
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Close the light-box
function HideAddEditConscriptionLightBox() {
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("lboxConscription").style.display = "none";
}

//Save Add/Edit Conscription
function SaveAddEditConscriptionLightBox() {
    if (ValidateAddEditConscription()) {
        var url = "AddEditReservist_MilitaryService.aspx?AjaxMethod=JSSaveConscription";

        var params = "ConscriptionId=" + document.getElementById("hdnConscriptionID").value +
                     "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value +
                     "&MilitaryUnit=" + document.getElementById("txtPersonConscriptionMilitaryUnit").value +
                     "&Position=" + document.getElementById("txtPersonConscriptionPosition").value +
                     "&DateFrom=" + document.getElementById("txtPersonConscriptionDateFrom").value +
                     "&DateTo=" + document.getElementById("txtPersonConscriptionDateTo").value;

        function response_handler(xml) {
            var status = xmlValue(xml, "status");

            if (status == "OK") {
                var refreshedPositionsTable = xmlValue(xml, "refreshedConscriptionTable");

                document.getElementById("tblConscription").innerHTML = refreshedPositionsTable;

                document.getElementById("lblMessageConscription").className = "SuccessText";
                document.getElementById("lblMessageConscription").innerHTML = document.getElementById("hdnConscriptionID").value == "0" ? "Наборната служба е добавена успешно" : "Наборната служба е редактирана успешно";

                HideAddEditConscriptionLightBox();
            }
            else {
                document.getElementById("spanAddEditConscriptionLightBox").className = "ErrorText";
                document.getElementById("spanAddEditConscriptionLightBox").innerHTML = status;
                document.getElementById("spanAddEditConscriptionLightBox").style.display = "";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Client validation of Add/Edit Conscriptionlight-box
function ValidateAddEditConscription() {
    var res = true;

    var lblMessage = document.getElementById("spanAddEditConscriptionLightBox");
    lblMessage.innerHTML = "";

    var notValidFields = new Array();


    var txtPersonConscriptionMilitaryUnit = document.getElementById("txtPersonConscriptionMilitaryUnit");
    var txtPersonConscriptionPosition = document.getElementById("txtPersonConscriptionPosition");
    var txtPersonConscriptionDateFrom = document.getElementById("txtPersonConscriptionDateFrom");
    var txtPersonConscriptionDateTo = document.getElementById("txtPersonConscriptionDateTo");

    //Validate Fields

    if (txtPersonConscriptionMilitaryUnit.value.Trim() == "") {
        res = false;

        if (txtPersonConscriptionMilitaryUnit.disabled == true || txtPersonConscriptionMilitaryUnit.style.display == "none")
            notValidFields.push("Военно формирование");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Военно формирование") + "<br />";
    }

    if (txtPersonConscriptionPosition.value.Trim() == "") {
        res = false;

        if (txtPersonConscriptionPosition.disabled == true || txtPersonConscriptionPosition.style.display == "none")
            notValidFields.push("Длъжност");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Длъжност") + "<br />";
    }

    if (txtPersonConscriptionDateFrom.value.Trim() == "") {
        res = false;

        if (txtPersonConscriptionDateFrom.disabled == true || spanPersonConscriptionDateFrom.style.display == "none")
            notValidFields.push("От");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("От") + "<br />";
    }
    else {
        if (!IsValidDate(txtPersonConscriptionDateFrom.value)) {
            res = false;
            lblMessage.innerHTML += GetErrorMessageDate("От") + "<br />";
        }
    }

    if (txtPersonConscriptionDateTo.value.Trim() == "") {
        res = false;

        if (txtPersonConscriptionDateTo.disabled == true || spanPersonConscriptionDateTo.style.display == "none")
            notValidFields.push("До");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("До") + "<br />";
    }
    else {
        if (!IsValidDate(txtPersonConscriptionDateTo.value)) {
            res = false;
            lblMessage.innerHTML += GetErrorMessageDate("До") + "<br />";
        }
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

// Delete a particular Conscription record
function DeleteConscription(ConscriptionId) {
    ClearAllMessages();

    YesNoDialog("Желаете ли да изтриете наборната служба?", ConfirmYes, null);

    function ConfirmYes() {
        var url = "AddEditReservist_MilitaryService.aspx?AjaxMethod=JSDeleteConscription";

        var params = "ConscriptionId=" + ConscriptionId;
        params += "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

        function response_handler(xml) {
            var status = xmlValue(xml, "status")

            if (status == "OK") {
                var refreshedPositionsTable = xmlValue(xml, "refreshedConscriptionTable");

                document.getElementById("tblConscription").innerHTML = refreshedPositionsTable;

                document.getElementById("lblMessageConscription").className = "SuccessText";
                document.getElementById("lblMessageConscription").innerHTML = "Наборната служба е изтрита успешно";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}






// 7.----------- Table Discharge -------------------

//Load the Discharge table on demand
function lnkDischarge_Click() {
    LoadDischarges();
}

function LoadDischarges() {
    //If already loaded then do not load
    if (document.getElementById("hdnDischargeLoaded").value == "1")
        return;

    ClearAllMessages();

    document.getElementById("imgLoadingDischarge").style.visibility = "";

    var url = "AddEditReservist_MilitaryService.aspx?AjaxMethod=JSLoadDischarges";
    var params = "";
    params += "ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

    var hdnIsPreview = document.getElementById(hdnIsPreviewClientID).value;
    if (hdnIsPreview == 1)
        params += "&Preview=1";

    function LoadDischarge_CallBack(xml) {
        var tableHTML = xmlValue(xml, "tableHTML");
        var lightBoxHTML = xmlValue(xml, "lightBoxHTML");

        document.getElementById("tblDischarge").innerHTML = tableHTML;
        document.getElementById("lboxDischarge").innerHTML = lightBoxHTML;

        RefreshUIItems(xml);
        document.getElementById("imgLoadingDischarge").style.visibility = "hidden";

        //Mark as already loaded
        document.getElementById("hdnDischargeLoaded").value = "1";
    }

    var myAJAX = new AJAX(url, true, params, LoadDischarge_CallBack);
    myAJAX.Call();
}

//Open the light-box for adding a new record in the Discharge table
function NewDischarge() {
    ShowAddEditDischargeLightBox(0);
}

//Open the light-box for editing a record in the Discharge table
function EditDischarge(DischargeId) {
    ShowAddEditDischargeLightBox(DischargeId);
}

function ShowAddEditDischargeLightBox(DischargeId) {
    ClearAllMessages();

    //Set DateTime Pickers
    RefreshDatePickers();

    document.getElementById("hdnDischargeID").value = DischargeId;

    //New record
    if (DischargeId == 0) {
        document.getElementById("lblAddEditDischargeTitle").innerHTML = "Въвеждане на нова заповед за прекратяване на служба";

        document.getElementById("txtPersonDischargeYear").value = "";
        document.getElementById("hdnDischargeReasonCode").value = "";
        document.getElementById("txtDischargeReason").innerHTML = "";
        document.getElementById("ddPersonDischargeDischargeDestination").value = optionChooseOneValue;
        document.getElementById("txtPersonDischargeOrder").value = "";
        document.getElementById("txtPersonDischargeOrderDate").value = "";
        document.getElementById("txtPersonDischargeOrderEffectiveDate").value = "";
        document.getElementById("ddPersonDischargeDischargeMilitaryCommanderRank").value = optionChooseOneValue;

        // clean message label in the light box and hide it
        document.getElementById("spanAddEditDischargeLightBox").style.display = "none";
        document.getElementById("spanAddEditDischargeLightBox").innerHTML = "";

        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("lboxDischarge").style.display = "";
        CenterLightBox("lboxDischarge");
    }
    else //Edit Discharge
    {
        document.getElementById("lblAddEditDischargeTitle").innerHTML = "Редактиране на заповед за прекратяване на служба";

        var url = "AddEditReservist_MilitaryService.aspx?AjaxMethod=JSLoadDischarge";

        var params = "DischargeId=" + DischargeId;

        function response_handler(xml) {
            var personDischarge = xml.getElementsByTagName("personDischarge")[0];

            var PersonDischargeYear = xmlValue(personDischarge, "PersonDischargeYear");
            var PersonDischargeReasonCode = xmlValue(personDischarge, "PersonDischargeReasonCode");
            var PersonDischargeReasonName = xmlValue(personDischarge, "PersonDischargeReasonName");
            var PersonDischargeDestinationCode = xmlValue(personDischarge, "PersonDischargeDestinationCode");
            var PersonDischargeOrder = xmlValue(personDischarge, "PersonDischargeOrder");
            var PersonDischargeOrderDate = xmlValue(personDischarge, "PersonDischargeOrderDate");
            var PersonDischargeOrderEffectiveDate = xmlValue(personDischarge, "PersonDischargeOrderEffectiveDate");
            var PersonDischargeMilitaryCommanderRankCode = xmlValue(personDischarge, "PersonDischargeMilitaryCommanderRankCode");

            document.getElementById("txtPersonDischargeYear").value = PersonDischargeYear;
            document.getElementById("hdnDischargeReasonCode").value = PersonDischargeReasonCode;
            document.getElementById("txtDischargeReason").innerHTML = PersonDischargeReasonName;
            document.getElementById("ddPersonDischargeDischargeDestination").value = PersonDischargeDestinationCode;
            document.getElementById("txtPersonDischargeOrder").value = PersonDischargeOrder;
            document.getElementById("txtPersonDischargeOrderDate").value = PersonDischargeOrderDate;
            document.getElementById("txtPersonDischargeOrderEffectiveDate").value = PersonDischargeOrderEffectiveDate;
            document.getElementById("ddPersonDischargeDischargeMilitaryCommanderRank").value = PersonDischargeMilitaryCommanderRankCode;

            // clean message label in the light box and hide it
            document.getElementById("spanAddEditDischargeLightBox").style.display = "none";
            document.getElementById("spanAddEditDischargeLightBox").innerHTML = "";

            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("lboxDischarge").style.display = "";
            CenterLightBox("lboxDischarge");
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Close the light-box
function HideAddEditDischargeLightBox() {
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("lboxDischarge").style.display = "none";
}

//Save Add/Edit Discharge
function SaveAddEditDischargeLightBox() {
    if (ValidateAddEditDischarge()) {
        var url = "AddEditReservist_MilitaryService.aspx?AjaxMethod=JSSaveDischarge";

        var params = "DischargeId=" + document.getElementById("hdnDischargeID").value +
                     "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value +
                     "&Year=" + document.getElementById("txtPersonDischargeYear").value +
                     "&DischargeReasonCode=" + document.getElementById("hdnDischargeReasonCode").value +
                     "&DischargeDestinationCode=" + (document.getElementById("ddPersonDischargeDischargeDestination").value != optionChooseOneValue ? document.getElementById("ddPersonDischargeDischargeDestination").value : "") +
                     "&Order=" + document.getElementById("txtPersonDischargeOrder").value +
                     "&OrderDate=" + document.getElementById("txtPersonDischargeOrderDate").value +
                     "&OrderEffectiveDate=" + document.getElementById("txtPersonDischargeOrderEffectiveDate").value +
                     "&MilitaryCommanderRankCode=" + (document.getElementById("ddPersonDischargeDischargeMilitaryCommanderRank").value != optionChooseOneValue ? document.getElementById("ddPersonDischargeDischargeMilitaryCommanderRank").value : "");

        function response_handler(xml) {
            var status = xmlValue(xml, "status");

            if (status == "OK") {
                var refreshedPositionsTable = xmlValue(xml, "refreshedDischargeTable");

                document.getElementById("tblDischarge").innerHTML = refreshedPositionsTable;

                document.getElementById("lblMessageDischarge").className = "SuccessText";
                document.getElementById("lblMessageDischarge").innerHTML = document.getElementById("hdnDischargeID").value == "0" ? "Заповедта е добавена успешно" : "Заповедта е редактирана успешно";

                HideAddEditDischargeLightBox();
            }
            else {
                document.getElementById("spanAddEditDischargeLightBox").className = "ErrorText";
                document.getElementById("spanAddEditDischargeLightBox").innerHTML = status;
                document.getElementById("spanAddEditDischargeLightBox").style.display = "";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

//Client validation of Add/Edit Dischargelight-box
function ValidateAddEditDischarge() {
    var res = true;

    var lblMessage = document.getElementById("spanAddEditDischargeLightBox");
    lblMessage.innerHTML = "";

    var notValidFields = new Array();

    var txtPersonDischargeYear = document.getElementById("txtPersonDischargeYear");
    var txtDischargeReason = document.getElementById("txtDischargeReason");
    var txtPersonDischargeOrderDate = document.getElementById("txtPersonDischargeOrderDate");
    var spanPersonDischargeOrderDate = document.getElementById("spanPersonDischargeOrderDate");
    var txtPersonDischargeOrderEffectiveDate = document.getElementById("txtPersonDischargeOrderEffectiveDate");

    //Validate Fields

    if (txtPersonDischargeYear.value.Trim() == "") {
        res = false;

        if (txtPersonDischargeYear.disabled == true || txtPersonDischargeYear.style.display == "none")
            notValidFields.push("Година");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Година") + "<br />";
    }
    else {
        if (!isInt(txtPersonDischargeYear.value) || parseInt(txtPersonDischargeYear.value) <= 0) {
            res = false;
            lblMessage.innerHTML += GetErrorMessageNumber("Година") + "<br />";
        }
    }

    if (txtDischargeReason.innerHTML == "") {
        res = false;

        if (txtDischargeReason.disabled == true || txtDischargeReason.style.display == "none")
            notValidFields.push("Причина за освобождаване от ВС");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Причина за освобождаване от ВС") + "<br />";
    }


    if (txtPersonDischargeOrderDate.value.Trim() == "") {
        res = false;

        if (txtPersonDischargeOrderDate.disabled == true || spanPersonDischargeOrderDate.style.display == "none")
            notValidFields.push("Дата");
        else
            lblMessage.innerHTML += GetErrorMessageMandatory("Дата") + "<br />";
    }
    else {
        if (!IsValidDate(txtPersonDischargeOrderDate.value)) {
            res = false;
            lblMessage.innerHTML += GetErrorMessageDate("Дата") + "<br />";
        }
    }


    if (txtPersonDischargeOrderEffectiveDate.value.Trim() != "") {
        if (!IsValidDate(txtPersonDischargeOrderEffectiveDate.value)) {
            res = false;
            lblMessage.innerHTML += GetErrorMessageDate("Считано от") + "<br />";
        }
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

// Delete a particular Discharge record
function DeleteDischarge(DischargeId) {
    ClearAllMessages();

    YesNoDialog("Желаете ли да изтриете заповедта?", ConfirmYes, null);

    function ConfirmYes() {
        var url = "AddEditReservist_MilitaryService.aspx?AjaxMethod=JSDeleteDischarge";

        var params = "DischargeId=" + DischargeId;
        params += "&ReservistId=" + document.getElementById(hdnReservistIdClientID).value;

        function response_handler(xml) {
            var status = xmlValue(xml, "status")

            if (status == "OK") {
                var refreshedPositionsTable = xmlValue(xml, "refreshedDischargeTable");

                document.getElementById("tblDischarge").innerHTML = refreshedPositionsTable;

                document.getElementById("lblMessageDischarge").className = "SuccessText";
                document.getElementById("lblMessageDischarge").innerHTML = "Заповедта е изтрита успешно";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

function DischargeReasonSelector_OnSelectedDischargeReason(dischargeReasonCode, dischargeReasonName) {

    document.getElementById("txtDischargeReason").innerHTML = dischargeReasonName;
    document.getElementById("hdnDischargeReasonCode").value = dischargeReasonCode;
}
