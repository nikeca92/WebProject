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

// Display light box with tractor model title (for editing or adding new)
function ShowTractorModelLightBox(tractorModelId) {
    var lblTractorModelBoxTitle = document.getElementById("lblTractorModelBoxTitle");
    lblTractorModelBoxTitle.innerHTML = "Добавяне на модел трактор";

    document.getElementById("lblTractorMakeValue").innerHTML = document.getElementById(ddlTractorMakesClientID).options[document.getElementById(ddlTractorMakesClientID).selectedIndex].text;
    
    if (tractorModelId != 0) // gets current values if editing applicant education
    {
        var url = "ManageTractorModels.aspx?AjaxMethod=JSLoadTractorModelDetails";
        var params = "";
        params += "TractorModelID=" + tractorModelId;

        function response_handler(xml) {
            var tractorModelData = xml.getElementsByTagName("tractorModelData")[0];

            var tractorModelId = xmlValue(tractorModelData, "tractorModelId");
            var tractorModelName = xmlValue(tractorModelData, "tractorModelName");
            var tractorMakeName = xmlValue(tractorModelData, "tractorMakeName");

            if (tractorModelId != "") {
                document.getElementById("hdnTractorModelId").value = tractorModelId; // setting tractor model ID(0 - if new tractor model)
                document.getElementById("txtTractorModelName").value = tractorModelName;
                document.getElementById("lblTractorMakeValue").innerHTML = tractorMakeName;
                lblTractorModelBoxTitle.innerHTML = "Редактиране на модел трактор";
            }
            
            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("TractorModelLightBox").style.display = "";
            CenterLightBox("TractorModelLightBox");
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
    else {        
        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("TractorModelLightBox").style.display = "";
        CenterLightBox("TractorModelLightBox");
    }
}

function SaveTractorModel() {
    var hdnTractorModelId = document.getElementById("hdnTractorModelId").value;
    var txtTractorModelName = document.getElementById("txtTractorModelName");

    if (txtTractorModelName.value != "") {
        var url = "ManageTractorModels.aspx?AjaxMethod=JSSaveTractorModel";
        var params = "";
        params += "TractorMakeID=" + GetSelectedItemId(document.getElementById(ddlTractorMakesClientID)); ;
        params += "&TractorModelID=" + document.getElementById("hdnTractorModelId").value;
        params += "&TractorModelName=" + txtTractorModelName.value;
        
        function response_handler(xml) {
            HideTractorModelLightBox();

            if (xmlValue(xml, "response") != "OK") {
                alert("Има проблеми на сървъра!");
            }
            else if (hdnTractorModelId != "" && hdnTractorModelId != 0) {
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
        var lblTractorModelLightBoxMessage = document.getElementById("lblTractorModelLightBoxMessage");
        lblTractorModelLightBoxMessage.className = "ErrorText";
        lblTractorModelLightBoxMessage.style.display = "";
        lblTractorModelLightBoxMessage.innerHTML = "Модел трактор задължително поле";
    }
}

function DeleteTractorModel(tractorModelId)
{
    YesNoDialog("Желаете ли да изтриете този модел трактор?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "ManageTractorModels.aspx?AjaxMethod=JSDeleteTractorModel";
        var params = "";
        params += "TractorModelID=" + tractorModelId;
        
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

// Close the light box and refresh tractor models table
function HideTractorModelLightBox() {
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("TractorModelLightBox").style.display = "none";

    document.getElementById("hdnTractorModelId").value = "";
    document.getElementById("txtTractorModelName").value = "";
    document.getElementById("lblTractorModelLightBoxMessage").innerHTML = "";
    document.getElementById("lblTractorModelLightBoxMessage").style.display = "none";
}