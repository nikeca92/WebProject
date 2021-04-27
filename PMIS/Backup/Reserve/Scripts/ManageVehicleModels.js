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

// Display light box with vehicle model title (for editing or adding new)
function ShowVehicleModelLightBox(vehicleModelId) {
    var lblVehicleModelBoxTitle = document.getElementById("lblVehicleModelBoxTitle");
    lblVehicleModelBoxTitle.innerHTML = "Добавяне на модел автомобил";
  
    document.getElementById("lblVehicleMakeValue").innerHTML = document.getElementById(ddlVehicleMakesClientID).options[document.getElementById(ddlVehicleMakesClientID).selectedIndex].text;
    
    if (vehicleModelId != 0) // gets current values if editing applicant education
    {
        var url = "ManageVehicleModels.aspx?AjaxMethod=JSLoadVehicleModelDetails";
        var params = "";
        params += "VehicleModelID=" + vehicleModelId;

        function response_handler(xml) {
            var vehicleModelData = xml.getElementsByTagName("vehicleModelData")[0];

            var vehicleModelId = xmlValue(vehicleModelData, "vehicleModelId");
            var vehicleModelName = xmlValue(vehicleModelData, "vehicleModelName");
            var vehicleMakeName = xmlValue(vehicleModelData, "vehicleMakeName");

            if (vehicleModelId != "") {
                document.getElementById("hdnVehicleModelId").value = vehicleModelId; // setting vehicle model ID(0 - if new vehicle model)
                document.getElementById("txtVehicleModelName").value = vehicleModelName;
                document.getElementById("lblVehicleMakeValue").innerHTML = vehicleMakeName;
                lblVehicleModelBoxTitle.innerHTML = "Редактиране на модел автомобил";
            }
            
            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("VehicleModelLightBox").style.display = "";
            CenterLightBox("VehicleModelLightBox");
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
    else {        
        //shows the light box and "disable" rest of the page
        document.getElementById("HidePage").style.display = "";
        document.getElementById("VehicleModelLightBox").style.display = "";
        CenterLightBox("VehicleModelLightBox"); 
    }
}

function SaveVehicleModel() {
    var hdnVehicleModelId = document.getElementById("hdnVehicleModelId").value;
    var txtVehicleModelName = document.getElementById("txtVehicleModelName");

    if (txtVehicleModelName.value != "") {
        var url = "ManageVehicleModels.aspx?AjaxMethod=JSSaveVehicleModel";
        var params = "";
        params += "VehicleMakeID=" + GetSelectedItemId(document.getElementById(ddlVehicleMakesClientID)); ;
        params += "&VehicleModelID=" + document.getElementById("hdnVehicleModelId").value;
        params += "&VehicleModelName=" + txtVehicleModelName.value;
        
        function response_handler(xml) {
            HideVehicleModelLightBox();

            if (xmlValue(xml, "response") != "OK") {
                alert("Има проблеми на сървъра!");
            }
            else if (hdnVehicleModelId != "" && hdnVehicleModelId != 0) {
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
        var lblVehicleModelLightBoxMessage = document.getElementById("lblVehicleModelLightBoxMessage");
        lblVehicleModelLightBoxMessage.className = "ErrorText";
        lblVehicleModelLightBoxMessage.style.display = "";
        lblVehicleModelLightBoxMessage.innerHTML = "Модел автомобил задължително поле";
    }
}

function DeleteVehicleModel(vehicleModelId)
{
    YesNoDialog("Желаете ли да изтриете този модел автомобил?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "ManageVehicleModels.aspx?AjaxMethod=JSDeleteVehicleModel";
        var params = "";
        params += "VehicleModelID=" + vehicleModelId;
        
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

// Close the light box and refresh vehicle models table
function HideVehicleModelLightBox() {
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("VehicleModelLightBox").style.display = "none";

    document.getElementById("hdnVehicleModelId").value = "";
    document.getElementById("txtVehicleModelName").value = "";
    document.getElementById("lblVehicleModelLightBoxMessage").innerHTML = "";
    document.getElementById("lblVehicleModelLightBoxMessage").style.display = "none";
}