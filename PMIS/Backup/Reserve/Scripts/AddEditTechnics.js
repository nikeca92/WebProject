//Call this function when the page is loaded (after the tabs are loaded)
function TechnicsInitialLoad()
{
    var technicsId = document.getElementById(hdnTechnicsIdClientID).value;
    technicsId = parseInt(technicsId);

    //Edit mode
    if (!isNaN(technicsId) && technicsId > 0)
    {
        if (document.getElementById("lblRegNumberValue"))
        {
            document.getElementById("lblRegNumberValue").style.display = "";
            document.getElementById("imgEditRegNumber").style.display = "";
            document.getElementById("imgHistoryRegNumber").style.display = "";
        }

        if (document.getElementById("lblInventoryNumberValue")) 
        {
            document.getElementById("lblInventoryNumberValue").style.display = "";
            document.getElementById("imgEditInvNumber").style.display = "";
            document.getElementById("imgHistoryInvNumber").style.display = "";
        }

        if (document.getElementById("lblAirInvNumberValue"))
        {
            document.getElementById("lblAirInvNumberValue").style.display = "";
            document.getElementById("imgEditAirInvNumber").style.display = "";
            document.getElementById("imgHistoryAirInvNumber").style.display = "";
        }

        LoadBasicInfoByTechnicsId();
    }
    else //New mode
    {
        if (document.getElementById("txtRegNumber"))
            document.getElementById("txtRegNumber").style.display = "";

        if (document.getElementById("lblInventoryNumberValue"))
            document.getElementById("txtInventoryNumber").style.display = "";

        if (document.getElementById("txtAirInvNumber"))
            document.getElementById("txtAirInvNumber").style.display = "";

        if (document.getElementById("btnTabMilitaryReport"))
            document.getElementById("btnTabMilitaryReport").style.display = "none";

        if (document.getElementById("btnTabOwner"))
            document.getElementById("btnTabOwner").style.display = "none";

        if (document.getElementById("btnTabOtherInfo"))
            document.getElementById("btnTabOtherInfo").style.display = "none";
    
        isLoadedBasicInfo = true;
        ShowContent();
    }
}

//This function displays the content div and hides the "loading" div
function ShowContent()
{
    if (!isContentDisplayed && isLoadedBasicInfo && isLoadedTheInitialTab)
    {
        document.getElementById("loadingDiv").style.display = "none";
        document.getElementById("contentDiv").style.display = "";

        isContentDisplayed = true;
    }
}


//Clear all messages on the screen
function ClearAllMessages()
{
    if (document.getElementById("lblAllTabsMessage"))
        document.getElementById("lblAllTabsMessage").innerHTML = "";
           
}

function SaveAllTechicsTabs() {
    var newTechnicsMode = document.getElementById(hdnTechnicsIdClientID).value == 0;

    ClearAllMessages();
    
    var lblMessage = document.getElementById("lblAllTabsMessage");
    var validationMessage = "";

    validationMessage += IsBasicInfoValid();
    validationMessage += ValidateMilitaryReportTab();
    IsOwnerDataValid(NextStep);
    
    function NextStep(msg) {
        validationMessage += msg;

        if (validationMessage == "") {
            //shows the saving data light box and "disable" the rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("lboxSavingData").style.display = "";
            CenterLightBox("lboxSavingData");
        
            SaveBasicInfo(function() {
                SaveMilitaryReportTab(function() {
                    SaveOwnerInfo(function() {
                        SaveOtherInfo(function() {
                            var lblMessage = document.getElementById("lblAllTabsMessage");
                            lblMessage.className = "SuccessText";
                            lblMessage.innerHTML = "Записът е успешен";

                            if (newTechnicsMode)
                                GoToEditMode();

                            setTimeout(function() {
                                document.getElementById("HidePage").style.display = "none";
                                document.getElementById("lboxSavingData").style.display = "none";
                            }, 500);
                        });
                    });
                });
            });
        } else {
            lblMessage.className = "ErrorText";
            lblMessage.style.display = "";
            lblMessage.innerHTML = validationMessage;
        }    
     }     
}
