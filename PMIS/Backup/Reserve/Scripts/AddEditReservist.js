//Call this function when the page is loaded (after the tabs are loaded)
function ReservistInitialLoad()
{
    var reservistId = document.getElementById(hdnReservistIdClientID).value;
    reservistId = parseInt(reservistId);

    //Edit mode
    if (!isNaN(reservistId) && reservistId > 0)
    {
        document.getElementById("lblIdentNumberValue").style.display = "";

        LoadPersonalDetailsByReservistId();
    }
    else //New mode
    {
        document.getElementById("txtIdentNumber").style.display = "";

        if (document.getElementById("btnTabEducationWork"))
            document.getElementById("btnTabEducationWork").style.display = "none";

        if (document.getElementById("btnTabMilitaryReport"))
            document.getElementById("btnTabMilitaryReport").style.display = "none";

        if (document.getElementById("btnTabMilitaryService"))
            document.getElementById("btnTabMilitaryService").style.display = "none";

        if (document.getElementById("btnTabOtherInfo"))
            document.getElementById("btnTabOtherInfo").style.display = "none";

        isLoadedPersonalData = true;
        ShowContent();
    }
}

//This function displays the content div and hides the "loading" div
function ShowContent()
{
    if (!isContentDisplayed && isLoadedPersonalData && isLoadedTheInitialTab)
    {
        document.getElementById("loadingDiv").style.display = "none";
        document.getElementById("contentDiv").style.display = "";

        isContentDisplayed = true;
    }
}


//Clear all messages on the screen
function ClearAllMessages()
{
    document.getElementById("lblGeneralTabMesage").innerHTML = ""; 
    
    ClearEducationWorkMessages();
    ClearMilitaryReportMessages();
    ClearMilitaryServiceMessages();
}

function SaveAllReservistTabs() {
    var isNewReservistMode = document.getElementById(hdnReservistIdClientID).value == 0;   

    ClearAllMessages();

    var lblMessage = document.getElementById("lblGeneralTabMesage");
    var validationMessage = "";

    validationMessage += IsPersonalDataValid();
    validationMessage += ValidateMilitaryReportTab();
    IsWorkplaceDataValid(NextStep);

    function NextStep(msg) {
        validationMessage += msg;

        if (validationMessage == "") {
            //shows the saving data light box and "disable" the rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("lboxSavingData").style.display = "";
            CenterLightBox("lboxSavingData");
            
            SavePersonalData(function() {
                SaveMilitaryReportTab(function() {
                    SavePersonWorkPlaceData(function() {
                        SaveOtherInfo(function() {
                            var lblMessage = document.getElementById("lblGeneralTabMesage");
                            lblMessage.className = "SuccessText";
                            lblMessage.innerHTML = "Записът е успешен";

                            if (isNewReservistMode)
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

function RefreshUIItems(xml, UIItemsTag)
{
    var tag = "UIItems";

    if (typeof UIItemsTag != "undefined")
        tag = UIItemsTag;

    //Setup the UIItems logic on the loaded tab
    var UIItems = xml.getElementsByTagName(tag);
    if (UIItems.length > 0)
    {
        var disabledClientControls = xmlValue(UIItems[0], "disabledClientControls");
        var hiddenClientControls = xmlValue(UIItems[0], "hiddenClientControls");

        if (disabledClientControls != "")
        {
            document.getElementById(hdnDisabledClientControls).value += (document.getElementById(hdnDisabledClientControls).value == "" ? "" : ",") + disabledClientControls;
            CheckDisabledClientControls();
        }
        if (hiddenClientControls != "")
        {
            document.getElementById(hdnHiddenClientControls).value += (document.getElementById(hdnHiddenClientControls).value == "" ? "" : ",") + hiddenClientControls;
            CheckHiddenClientControls();
        }
    }
}


function GoBack()
{
    JSRedirect("ManageReservists.aspx");
}