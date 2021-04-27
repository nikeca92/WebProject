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
    var url = "FulfilReservistsMilCommand.aspx?AjaxMethod=JSSavePunkt";

    var params = "MilitaryCommandID=" + document.getElementById(ddMilitaryCommand).value +
                 "&MilitaryDepartmentID=" + document.getElementById(ddMilitaryDepartmentID).value +
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

function AddFulfilment(requestCommandPositionID)
{
    JSRedirect("SearchReservists.aspx?EquipmentReservistsRequestId=" + document.getElementById(hfEquipmentReservistsRequestID).value +
                                            "&MilitaryDepartmentId=" + document.getElementById(ddMilitaryDepartmentID).value +
                                            "&MilitaryCommandID=" + document.getElementById(ddMilitaryCommand).value +
                                            "&RequestCommandPositionID=" + requestCommandPositionID +
                                            "&Readiness=1" +
                                            "&FromFulfilByCommand=1");
}

function AddFulfilmentReserve(requestCommandPositionID)
{
    JSRedirect("SearchReservists.aspx?EquipmentReservistsRequestId=" + document.getElementById(hfEquipmentReservistsRequestID).value +
                                            "&MilitaryDepartmentId=" + document.getElementById(ddMilitaryDepartmentID).value +
                                            "&MilitaryCommandID=" + document.getElementById(ddMilitaryCommand).value +
                                            "&RequestCommandPositionID=" + requestCommandPositionID +
                                            "&Readiness=2" +
                                            "&FromFulfilByCommand=1");
}

function ShowViewFulfilmentLightBox(requestCommandPositionID)
{
    var url = "FulfilReservistsMilCommand.aspx?AjaxMethod=JSGetViewFulfilmentLightBox";
    var params = "";
    params += "RequestCommandPositionID=" + requestCommandPositionID;
    params += "&MilitaryDepartmentID=" + document.getElementById(ddMilitaryDepartmentID).value;
    
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
    var url = "FulfilReservistsMilCommand.aspx?AjaxMethod=JSGetViewFulfilmentLightBox";
    var params = "";
    params += "RequestCommandPositionID=" + document.getElementById("hdnRequestCommandPositionID").value;
    params += "&MilitaryDepartmentID=" + document.getElementById(ddMilitaryDepartmentID).value;
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

function DeleteReservist(fillReservistsRequestID, militaryDepartmentID)
{
    YesNoDialog("Сигурни ли сте, че желаете да изтриете този запис?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "FulfilReservistsMilCommand.aspx?AjaxMethod=JSDeleteReservist";
        var params = "";
        params += "FillReservistsRequestID=" + fillReservistsRequestID;
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

// Open in new window AddEditReservist page in read only mode
function PreviewReservist(reservistID) {
    var url = "";
    var pageName = "AddEditReservist"
    var param = "";

    url = "../ContentPages/" + pageName + ".aspx?ReservistId=" + reservistID + "&Preview=1";

    var uplPopup = window.open(url, pageName, param);

    if (uplPopup != null)
        uplPopup.focus();
}