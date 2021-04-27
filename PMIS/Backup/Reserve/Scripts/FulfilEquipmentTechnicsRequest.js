window.addEventListener ? window.addEventListener('load', PageLoad, false) : window.attachEvent('onload', PageLoad);
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(PageEndRequestHandler);

//Call this when the page is loaded
function PageLoad()
{
}

//The end of the UpdatePanel request
function PageEndRequestHandler(sender)
{
    document.getElementById(lblPunktMessage).innerHTML = "";

    if (sender._postBackSettings.sourceElement.id == ddMilitaryCommand)
    {
        LoadOriginalValues();
    }
}

//Save punkt panel
function btnSavePunktClick()
{
    var url = "FulfilEquipmentTechnicsRequest.aspx?AjaxMethod=JSSavePunkt";

    var params = "MilitaryCommandID=" + document.getElementById(ddMilitaryCommand).value +
                 "&MilitaryDepartmentID=" + document.getElementById(hfMilitaryDepartmentID).value +
                 "&CityID=" + document.getElementById(ddCity).value +
                 "&Place=" + document.getElementById(txtPlace).value;

    
    function response_handler(xml)
    {
        var lblMessage = document.getElementById(lblPunktMessage);
        var status = xmlValue(xml, "status")

        if (status == "OK")
        {
            RefreshInputsOfSpecificContainer(document.getElementById("tblPunkt"), true);
        
            lblMessage.className = "SuccessText";
            lblMessage.innerHTML = "Записът е успешен";
        }
        else
        {
            lblMessage.className = "ErrorText";
            lblMessage.innerHTML = "Записът не е успешен";
        }
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}

function AddFulfilment(page, requestCommandPositionID)
{
    JSRedirect(page + ".aspx?EquipmentTechnicsRequestId=" + document.getElementById(hfEquipmentTechnicsRequestID).value +
                                            "&MilitaryDepartmentId=" + document.getElementById(hfMilitaryDepartmentID).value +
                                            "&MilitaryCommandID=" + document.getElementById(ddMilitaryCommand).value +
                                            "&TechnicsRequestCommandPositionID=" + requestCommandPositionID +
                                            "&Readiness=1");
}

function AddFulfilmentReserve(page, requestCommandPositionID)
{
    JSRedirect(page + ".aspx?EquipmentTechnicsRequestId=" + document.getElementById(hfEquipmentTechnicsRequestID).value +
                                            "&MilitaryDepartmentId=" + document.getElementById(hfMilitaryDepartmentID).value +
                                            "&MilitaryCommandID=" + document.getElementById(ddMilitaryCommand).value +
                                            "&TechnicsRequestCommandPositionID=" + requestCommandPositionID +
                                            "&Readiness=2");
}

function ShowViewFulfilmentLightBox(requestCommandPositionID)
{
    var url = "FulfilEquipmentTechnicsRequest.aspx?AjaxMethod=JSGetViewFulfilmentLightBox";
    var params = "";
    params += "TechnicsRequestCommandPositionID=" + requestCommandPositionID;
    params += "&MilitaryDepartmentID=" + document.getElementById(hfMilitaryDepartmentID).value;
    
    function response_handler(xml)
    {
        document.getElementById('divViewFulfilmentLightBoxContent').innerHTML = xmlNodeText(xml.childNodes[0]);
        document.getElementById("HidePage").style.display = "";
        document.getElementById("divViewFulfilmentLightBox").style.display = "";
        CenterLightBox("divViewFulfilmentLightBox");
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}

function HideViewFulfilmentLightBox()
{
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("divViewFulfilmentLightBox").style.display = "none";

    document.getElementById(btnRefresh).click();
}

function BtnPagingClick(objectName)
{
    hdnPageIdx = document.getElementById('hdnPageIndex');
    hdnMaxPage = document.getElementById('hdnPageMaxPage');
    switch (objectName)
    {
        case "btnFirst":
            hdnPageIdx.value = 1;

            RefreshViewFulfilmentLightBox();
            break;

        case "btnPrev":
            pageIdx = parseInt(hdnPageIdx.value);

            if (pageIdx > 1)
            {
                pageIdx--;
                hdnPageIdx.value = pageIdx;
                RefreshViewFulfilmentLightBox();
            }

            break;

        case "btnNext":
            pageIdx = parseInt(hdnPageIdx.value);
            maxPage = parseInt(hdnMaxPage.value);

            if (pageIdx < maxPage)
            {
                pageIdx++;
                hdnPageIdx.value = pageIdx;
                RefreshViewFulfilmentLightBox();
            }
            break;


        case "btnLast":
            hdnPageIdx.value = hdnMaxPage.value;

            RefreshViewFulfilmentLightBox();
            break;

        case "btnPageGo":
            maxPage = parseInt(hdnMaxPage.value);
            goToPage = parseInt(document.getElementById('txtTableGotoPage').value);

            if (isInt(goToPage) && goToPage > 0 && goToPage <= maxPage)
            {
                hdnPageIdx.value = goToPage;
                RefreshViewFulfilmentLightBox();
            }
            break;

        default:
            break;
    }

}

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

    RefreshViewFulfilmentLightBox();
}

function RefreshViewFulfilmentLightBox()
{
    var url = "FulfilEquipmentTechnicsRequest.aspx?AjaxMethod=JSGetViewFulfilmentLightBox";
    var params = "";
    params += "TechnicsRequestCommandPositionID=" + document.getElementById("hdnRequestCommandPositionID").value;
    params += "&MilitaryDepartmentID=" + document.getElementById(hfMilitaryDepartmentID).value;
    params += "&PageIndex=" + document.getElementById("hdnPageIndex").value;
    params += "&MaxPage=" + document.getElementById("hdnPageMaxPage").value;
    params += "&OrderBy=" + document.getElementById("hdnOrderBy").value;
    
    function response_handler(xml)
    {
        document.getElementById('divViewFulfilmentLightBoxContent').innerHTML = xmlNodeText(xml.childNodes[0]);
        document.getElementById("HidePage").style.display = "";
        document.getElementById("divViewFulfilmentLightBox").style.display = "";
        CenterLightBox("divViewFulfilmentLightBox");
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}

function DeleteVehicle(fulfilTechnicsRequestID, militaryDepartmentID)
{
    YesNoDialog("Сигурни ли сте, че желаете да изтриете този запис?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "FulfilEquipmentTechnicsRequest.aspx?AjaxMethod=JSDeleteVehicle";
        var params = "";
        params += "FulfilTechnicsRequestID=" + fulfilTechnicsRequestID;
        params += "&MilitaryDepartmentID=" + militaryDepartmentID;
        
        function response_handler(xml)
        {
            var status = xmlValue(xml, "status")

            if (status == "OK")
            {
                RefreshViewFulfilmentLightBox();
            }
            else
            {
                var lblMessage = document.getElementById("lblViewFulfilmentMessage");
                lblMessage.className = "ErrorText";
                lblMessage.innerHTML = "Грешка при изтриване";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

function DeleteTrailer(fulfilTechnicsRequestID, militaryDepartmentID)
{
    YesNoDialog("Сигурни ли сте, че желаете да изтриете този запис?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "FulfilEquipmentTechnicsRequest.aspx?AjaxMethod=JSDeleteTrailer";
        var params = "";
        params += "FulfilTechnicsRequestID=" + fulfilTechnicsRequestID;
        params += "&MilitaryDepartmentID=" + militaryDepartmentID;
        
        function response_handler(xml)
        {
            var status = xmlValue(xml, "status")

            if (status == "OK")
            {
                RefreshViewFulfilmentLightBox();
            }
            else
            {
                var lblMessage = document.getElementById("lblViewFulfilmentMessage");
                lblMessage.className = "ErrorText";
                lblMessage.innerHTML = "Грешка при изтриване";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

function DeleteMobileLiftingEquip(fulfilTechnicsRequestID, militaryDepartmentID)
{
    YesNoDialog("Сигурни ли сте, че желаете да изтриете този запис?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "FulfilEquipmentTechnicsRequest.aspx?AjaxMethod=JSDeleteMobileLiftingEquip";
        var params = "";
        params += "FulfilTechnicsRequestID=" + fulfilTechnicsRequestID;
        params += "&MilitaryDepartmentID=" + militaryDepartmentID;
        
        function response_handler(xml)
        {
            var status = xmlValue(xml, "status")

            if (status == "OK")
            {
                RefreshViewFulfilmentLightBox();
            }
            else
            {
                var lblMessage = document.getElementById("lblViewFulfilmentMessage");
                lblMessage.className = "ErrorText";
                lblMessage.innerHTML = "Грешка при изтриване";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

function DeleteTractor(fulfilTechnicsRequestID, militaryDepartmentID)
{
    YesNoDialog("Сигурни ли сте, че желаете да изтриете този запис?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "FulfilEquipmentTechnicsRequest.aspx?AjaxMethod=JSDeleteTractor";
        var params = "";
        params += "FulfilTechnicsRequestID=" + fulfilTechnicsRequestID;
        params += "&MilitaryDepartmentID=" + militaryDepartmentID;
        
        function response_handler(xml)
        {
            var status = xmlValue(xml, "status")

            if (status == "OK")
            {
                RefreshViewFulfilmentLightBox();
            }
            else
            {
                var lblMessage = document.getElementById("lblViewFulfilmentMessage");
                lblMessage.className = "ErrorText";
                lblMessage.innerHTML = "Грешка при изтриване";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

function DeleteEngEquip(fulfilTechnicsRequestID, militaryDepartmentID)
{
    YesNoDialog("Сигурни ли сте, че желаете да изтриете този запис?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "FulfilEquipmentTechnicsRequest.aspx?AjaxMethod=JSDeleteEngEquip";
        var params = "";
        params += "FulfilTechnicsRequestID=" + fulfilTechnicsRequestID;
        params += "&MilitaryDepartmentID=" + militaryDepartmentID;
        
        function response_handler(xml)
        {
            var status = xmlValue(xml, "status")

            if (status == "OK")
            {
                RefreshViewFulfilmentLightBox();
            }
            else
            {
                var lblMessage = document.getElementById("lblViewFulfilmentMessage");
                lblMessage.className = "ErrorText";
                lblMessage.innerHTML = "Грешка при изтриване";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}


function DeleteRailwayEquip(fulfilTechnicsRequestID, militaryDepartmentID)
{
    YesNoDialog("Сигурни ли сте, че желаете да изтриете този запис?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "FulfilEquipmentTechnicsRequest.aspx?AjaxMethod=JSDeleteRailwayEquip";
        var params = "";
        params += "FulfilTechnicsRequestID=" + fulfilTechnicsRequestID;
        params += "&MilitaryDepartmentID=" + militaryDepartmentID;
        
        function response_handler(xml)
        {
            var status = xmlValue(xml, "status")

            if (status == "OK")
            {
                RefreshViewFulfilmentLightBox();
            }
            else
            {
                var lblMessage = document.getElementById("lblViewFulfilmentMessage");
                lblMessage.className = "ErrorText";
                lblMessage.innerHTML = "Грешка при изтриване";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

function DeleteAviationEquip(fulfilTechnicsRequestID, militaryDepartmentID)
{
    YesNoDialog("Сигурни ли сте, че желаете да изтриете този запис?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "FulfilEquipmentTechnicsRequest.aspx?AjaxMethod=JSDeleteAviationEquip";
        var params = "";
        params += "FulfilTechnicsRequestID=" + fulfilTechnicsRequestID;
        params += "&MilitaryDepartmentID=" + militaryDepartmentID;
        
        function response_handler(xml)
        {
            var status = xmlValue(xml, "status")

            if (status == "OK")
            {
                RefreshViewFulfilmentLightBox();
            }
            else
            {
                var lblMessage = document.getElementById("lblViewFulfilmentMessage");
                lblMessage.className = "ErrorText";
                lblMessage.innerHTML = "Грешка при изтриване";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

function DeleteVessel(fulfilTechnicsRequestID, militaryDepartmentID)
{
    YesNoDialog("Сигурни ли сте, че желаете да изтриете този запис?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "FulfilEquipmentTechnicsRequest.aspx?AjaxMethod=JSDeleteVessel";
        var params = "";
        params += "FulfilTechnicsRequestID=" + fulfilTechnicsRequestID;
        params += "&MilitaryDepartmentID=" + militaryDepartmentID;
        
        function response_handler(xml)
        {
            var status = xmlValue(xml, "status")

            if (status == "OK")
            {
                RefreshViewFulfilmentLightBox();
            }
            else
            {
                var lblMessage = document.getElementById("lblViewFulfilmentMessage");
                lblMessage.className = "ErrorText";
                lblMessage.innerHTML = "Грешка при изтриване";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}


function DeleteFuelContainer(fulfilTechnicsRequestID, militaryDepartmentID)
{
    YesNoDialog("Сигурни ли сте, че желаете да изтриете този запис?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "FulfilEquipmentTechnicsRequest.aspx?AjaxMethod=JSDeleteFuelContainer";
        var params = "";
        params += "FulfilTechnicsRequestID=" + fulfilTechnicsRequestID;
        params += "&MilitaryDepartmentID=" + militaryDepartmentID;
        
        function response_handler(xml)
        {
            var status = xmlValue(xml, "status")

            if (status == "OK")
            {
                RefreshViewFulfilmentLightBox();
            }
            else
            {
                var lblMessage = document.getElementById("lblViewFulfilmentMessage");
                lblMessage.className = "ErrorText";
                lblMessage.innerHTML = "Грешка при изтриване";
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

// Open in new window AddEditTechnics page in read only mode
function PreviewTechnics(technicsId) {
    var url = "";
    var pageName = "AddEditTechnics"
    var param = "";

    url = "../ContentPages/" + pageName + ".aspx?TechnicsId=" + technicsId + "&Preview=1";

    var uplPopup = window.open(url, pageName, param);

    if (uplPopup != null)
        uplPopup.focus();
}