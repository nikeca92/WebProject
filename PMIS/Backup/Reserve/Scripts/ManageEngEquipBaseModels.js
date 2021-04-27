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

// Display light box with engEquipBase model title (for editing or adding new)
function ShowEngEquipBaseModelLightBox(engEquipBaseModelId) {
    var lblEngEquipBaseModelBoxTitle = document.getElementById("lblEngEquipBaseModelBoxTitle");
    lblEngEquipBaseModelBoxTitle.innerHTML = "Добавяне на модел инж. машина";

    document.getElementById("lblEngEquipBaseMakeValue").innerHTML = document.getElementById(ddlEngEquipBaseMakesClientID).options[document.getElementById(ddlEngEquipBaseMakesClientID).selectedIndex].text;
    
    if (engEquipBaseModelId != 0) // gets current values if editing applicant education
    {
        var url = "ManageEngEquipBaseModels.aspx?AjaxMethod=JSLoadEngEquipBaseModelDetails";
        var params = "";
        params += "EngEquipBaseModelID=" + engEquipBaseModelId;

        function response_handler(xml) {
            var engEquipBaseModelData = xml.getElementsByTagName("engEquipBaseModelData")[0];

            var engEquipBaseModelId = xmlValue(engEquipBaseModelData, "engEquipBaseModelId");
            var engEquipBaseModelName = xmlValue(engEquipBaseModelData, "engEquipBaseModelName");
            var engEquipBaseMakeName = xmlValue(engEquipBaseModelData, "engEquipBaseMakeName");

            if (engEquipBaseModelId != "") {
                document.getElementById("hdnEngEquipBaseModelId").value = engEquipBaseModelId; // setting engEquipBase model ID(0 - if new engEquipBase model)
                document.getElementById("txtEngEquipBaseModelName").value = engEquipBaseModelName;
                document.getElementById("lblEngEquipBaseMakeValue").innerHTML = engEquipBaseMakeName;
                lblEngEquipBaseModelBoxTitle.innerHTML = "Редактиране на модел инж. машина";
            }
            
            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("EngEquipBaseModelLightBox").style.display = "";
            CenterLightBox("EngEquipBaseModelLightBox");
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
    else {        
        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("EngEquipBaseModelLightBox").style.display = "";
        CenterLightBox("EngEquipBaseModelLightBox");
    }
}

function SaveEngEquipBaseModel() {
    var hdnEngEquipBaseModelId = document.getElementById("hdnEngEquipBaseModelId").value;
    var txtEngEquipBaseModelName = document.getElementById("txtEngEquipBaseModelName");

    if (txtEngEquipBaseModelName.value != "") {
        var url = "ManageEngEquipBaseModels.aspx?AjaxMethod=JSSaveEngEquipBaseModel";
        var params = "";
        params += "EngEquipBaseMakeID=" + GetSelectedItemId(document.getElementById(ddlEngEquipBaseMakesClientID)); ;
        params += "&EngEquipBaseModelID=" + document.getElementById("hdnEngEquipBaseModelId").value;
        params += "&EngEquipBaseModelName=" + txtEngEquipBaseModelName.value;
        
        function response_handler(xml) {
            HideEngEquipBaseModelLightBox();

            if (xmlValue(xml, "response") != "OK") {
                alert("Има проблеми на сървъра!");
            }
            else if (hdnEngEquipBaseModelId != "" && hdnEngEquipBaseModelId != 0) {
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
        var lblEngEquipBaseModelLightBoxMessage = document.getElementById("lblEngEquipBaseModelLightBoxMessage");
        lblEngEquipBaseModelLightBoxMessage.className = "ErrorText";
        lblEngEquipBaseModelLightBoxMessage.style.display = "";
        lblEngEquipBaseModelLightBoxMessage.innerHTML = "Модел инж. машина задължително поле";
    }
}

function DeleteEngEquipBaseModel(engEquipBaseModelId)
{
    YesNoDialog("Желаете ли да изтриете този модел инж. машина?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "ManageEngEquipBaseModels.aspx?AjaxMethod=JSDeleteEngEquipBaseModel";
        var params = "";
        params += "EngEquipBaseModelID=" + engEquipBaseModelId;
        
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

// Close the light box and refresh engEquipBase models table
function HideEngEquipBaseModelLightBox() {
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("EngEquipBaseModelLightBox").style.display = "none";

    document.getElementById("hdnEngEquipBaseModelId").value = "";
    document.getElementById("txtEngEquipBaseModelName").value = "";
    document.getElementById("lblEngEquipBaseModelLightBoxMessage").innerHTML = "";
    document.getElementById("lblEngEquipBaseModelLightBoxMessage").style.display = "none";
}