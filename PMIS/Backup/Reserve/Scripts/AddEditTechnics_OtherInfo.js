
//Save the technics's other info
function SaveOtherInfo(saveOtherInfoFinishCallback)
{
    if (IsTabAlreadyVisited("btnTabOtherInfo")) {
        var url = "AddEditTechnics_OtherInfo.aspx?AjaxMethod=JSSaveOtherInfo";
        var params = "";

        params += "TechnicsId=" + document.getElementById(hdnTechnicsIdClientID).value;
        params += "&TechnicsTypeKey=" + custEncodeURI(document.getElementById(hdnTechnicsTypeKeyClientID).value);
        params += "&OtherInfo=" + custEncodeURI(document.getElementById("txtOtherInfo").value);

        var myAJAX = new AJAX(url, true, params, SaveOtherInfo_Callback);
        myAJAX.Call();
    } else {
        saveOtherInfoFinishCallback();
    }
    
    function SaveOtherInfo_Callback(xml)
    {
        var message = xmlValue(xml, "response");
       
        RefreshInputsOfSpecificContainer(document.getElementById(divOtherInfoClientID), true);
        saveOtherInfoFinishCallback();
    }
}

function TabLoaded_OtherInfo()
{
    SetClientTextAreaMaxLength("txtOtherInfo", "4000");
}