//Function that sorts the table by a specific column
function SortTableBy(sort) {
    //If sorting by the same column them set the direction to be DESC
    if (document.getElementById(hdnSortByClientID).value == sort) {
        sort = sort + 100;
    }

    //Keep the order by column (its index) in a hidden field and simulate clicking Refresh
    document.getElementById(hdnSortByClientID).value = sort;
    document.getElementById(btnRefreshClientID).click();
}

// Display light box with aviationOtherBaseMachine model title (for editing or adding new)
function ShowAviationOtherBaseMachineModelLightBox(aviationOtherBaseMachineModelId) {
    var lblAviationOtherBaseMachineModelBoxTitle = document.getElementById("lblAviationOtherBaseMachineModelBoxTitle");
    lblAviationOtherBaseMachineModelBoxTitle.innerHTML = "Добавяне на модел базова машина (спец. инж. и самолетообслужваща техника)";

    document.getElementById("lblAviationOtherBaseMachineMakeValue").innerHTML = document.getElementById(ddlAviationOtherBaseMachineMakesClientID).options[document.getElementById(ddlAviationOtherBaseMachineMakesClientID).selectedIndex].text;
    
    if (aviationOtherBaseMachineModelId != 0) // gets current values if editing applicant education
    {
        var url = "ManageAviationOtherBaseMachineModels.aspx?AjaxMethod=JSLoadAviationOtherBaseMachineModelDetails";
        var params = "";
        params += "AviationOtherBaseMachineModelID=" + aviationOtherBaseMachineModelId;

        
        function response_handler(xml) {
            var aviationOtherBaseMachineModelData = xml.getElementsByTagName("aviationOtherBaseMachineModelData")[0];

            var aviationOtherBaseMachineModelId = xmlValue(aviationOtherBaseMachineModelData, "aviationOtherBaseMachineModelId");
            var aviationOtherBaseMachineModelName = xmlValue(aviationOtherBaseMachineModelData, "aviationOtherBaseMachineModelName");
            var aviationOtherBaseMachineMakeName = xmlValue(aviationOtherBaseMachineModelData, "aviationOtherBaseMachineMakeName");

            if (aviationOtherBaseMachineModelId != "") {
                document.getElementById("hdnAviationOtherBaseMachineModelId").value = aviationOtherBaseMachineModelId; // setting aviationOtherBaseMachine model ID(0 - if new aviationOtherBaseMachine model)
                document.getElementById("txtAviationOtherBaseMachineModelName").value = aviationOtherBaseMachineModelName;
                document.getElementById("lblAviationOtherBaseMachineMakeValue").innerHTML = aviationOtherBaseMachineMakeName;
                lblAviationOtherBaseMachineModelBoxTitle.innerHTML = "Редактиране на модел базова машина (спец. инж. и самолетообслужваща техника)";
            }
            
            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("AviationOtherBaseMachineModelLightBox").style.display = "";
            CenterLightBox("AviationOtherBaseMachineModelLightBox");
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
    else {        
        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("AviationOtherBaseMachineModelLightBox").style.display = "";
        CenterLightBox("AviationOtherBaseMachineModelLightBox");
    }
}

function SaveAviationOtherBaseMachineModel() {
    var hdnAviationOtherBaseMachineModelId = document.getElementById("hdnAviationOtherBaseMachineModelId").value;
    var txtAviationOtherBaseMachineModelName = document.getElementById("txtAviationOtherBaseMachineModelName");

    if (txtAviationOtherBaseMachineModelName.value != "") {
        var url = "ManageAviationOtherBaseMachineModels.aspx?AjaxMethod=JSSaveAviationOtherBaseMachineModel";
        var params = "";
        params += "AviationOtherBaseMachineMakeID=" + GetSelectedItemId(document.getElementById(ddlAviationOtherBaseMachineMakesClientID)); ;
        params += "&AviationOtherBaseMachineModelID=" + document.getElementById("hdnAviationOtherBaseMachineModelId").value;
        params += "&AviationOtherBaseMachineModelName=" + txtAviationOtherBaseMachineModelName.value;
        
        function response_handler(xml) {
            HideAviationOtherBaseMachineModelLightBox();

            if (xmlValue(xml, "response") != "OK") {
                alert("Има проблеми на сървъра!");
            }
            else if (hdnAviationOtherBaseMachineModelId != "" && hdnAviationOtherBaseMachineModelId != 0) {
                document.getElementById(hdnRefreshReasonClientID).value = "SAVED";
                document.getElementById(btnRefreshClientID).click();
            }
            else {
                document.getElementById(hdnRefreshReasonClientID).value = "ADDED";
                document.getElementById(btnRefreshClientID).click();
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
    else {
        var lblAviationOtherBaseMachineModelLightBoxMessage = document.getElementById("lblAviationOtherBaseMachineModelLightBoxMessage");
        lblAviationOtherBaseMachineModelLightBoxMessage.className = "ErrorText";
        lblAviationOtherBaseMachineModelLightBoxMessage.style.display = "";
        lblAviationOtherBaseMachineModelLightBoxMessage.innerHTML = "Модел базова машина (спец. инж. и самолетообслужваща техника) задължително поле";
    }
}

function DeleteAviationOtherBaseMachineModel(aviationOtherBaseMachineModelId)
{
    YesNoDialog("Желаете ли да изтриете този модел базова машина (спец. инж. и самолетообслужваща техника)?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "ManageAviationOtherBaseMachineModels.aspx?AjaxMethod=JSDeleteAviationOtherBaseMachineModel";
        var params = "";
        params += "AviationOtherBaseMachineModelID=" + aviationOtherBaseMachineModelId;
        
        function response_handler(xml) {
            if (xmlValue(xml, "response") != "OK") {
                alert("Има проблеми на сървъра!");
            }
            else {
                document.getElementById(hdnRefreshReasonClientID).value = "DELETED";
                document.getElementById(btnRefreshClientID).click();
            }
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

// Close the light box and refresh aviationOtherBaseMachine models table
function HideAviationOtherBaseMachineModelLightBox() {
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("AviationOtherBaseMachineModelLightBox").style.display = "none";

    document.getElementById("hdnAviationOtherBaseMachineModelId").value = "";
    document.getElementById("txtAviationOtherBaseMachineModelName").value = "";
    document.getElementById("lblAviationOtherBaseMachineModelLightBoxMessage").innerHTML = "";
    document.getElementById("lblAviationOtherBaseMachineModelLightBoxMessage").style.display = "none";
}