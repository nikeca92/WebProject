var CanLeave = false; // variable to control if page has to be checked for changes in that moment
var alwaysAskCnt = 0; // variable to control if always should display the message; if the value is greater than 0 then there is a request to AlwaysAsk
var originalValues = new Array(); // variable (array) for storing original values of the controls on the page(for comparing on leaving page)

var errFunc; // variable to store temporary original error function of browser
// check page before unloading for neccessity of saving changes
function checkFormSave()
{
    if (AreThereUnsavedChanges())
    {
        errFunc = window.onerror; // stores the original error function of the browser
        window.onerror = function() { return true; }; // changes default error function of the browser in order to supress firing buggy "unspecified error" in IE
        setTimeout("window.onerror = errFunc;", 10); // delayed restore of the original error function of the browser

        //User this trick to Enable the buttons if the user clicks Cancel and stays on the same page without doing a server call
        window.unloadTimer = setInterval("EnableButtons();HideAjaxLoader();clearInterval(window.unloadTimer);", 500);
        window.onunload = function() { clearInterval(window.unloadTimer); }

        return; //Do not return a message; We have turned off this logic to prevent the default IE messge to appear
        //Instead we use the CustomCheckFormSave() function
        //return "You are leaving the page without saving changes."; // return message, so the dialog is open
    }
}

// Loads the original values of the controls on the page - must be called on load of the page and when the data is saved
function LoadOriginalValues()
{
    originalValues = GetValues();
}

function AppendNewInputs()
{
    var newValues = GetValues(originalValues);
    originalValues = originalValues.concat(newValues);
}

function RefreshInputsOfSpecificContainer(containerElement, overrideExistingValues)
{
    var newValues = GetValues(originalValues, containerElement, overrideExistingValues);

    for (var i = 0; i < newValues.length; i++)
    {
        var found = false;
    
        for (var j = 0; j < originalValues.length; j++)
        {
            if (newValues[i].id == originalValues[j].id)
            {
                originalValues[j].value = newValues[i].value;
                found = true;
                break;
            }
        }

        if (!found)
        {
            originalValues.push(newValues[i]);
        }
    }
}

//Fix <a hrer=""> elements, because when click on it, if href doesn't start with #, browser fires twice unload event
//Also, we add the LinkClick event here that is used to add the clouse for the CustomCheckFormSave() function, which is used for the custom message
function FixLinks()
{
    var links = document.getElementsByTagName("a");

    for (var i = 0; i < links.length; i++)
    {
        if (links[i].href.substring(0, 11) == "javascript:" && !links[i].getAttribute("onclick"))
        {
            links[i].setAttribute("doonclick", links[i].href.substring(11));

            if (links[i].addEventListener) {
                links[i].addEventListener('click', LinkClick, false);
            } else {
                links[i].attachEvent('onclick', LinkClick);
            }

            links[i].href = "#";
        }
    }
}

function LinkClick(e) {
    var event = e;
    var sendingObj = event.srcElement || event.target;

    if ((sendingObj.tagName == "DIV" || sendingObj.tagName == "IMG" || sendingObj.tagName == "SPAN") && !sendingObj.getAttribute("doonclick")) //this is a report option
    {
        sendingObj = sendingObj.parentElement;
    }

    if (sendingObj.getAttribute("CheckForChanges") == "true" ||
        sendingObj.getAttribute("CheckForChanges") == "true")
    {
        CustomCheckFormSave(function() { LocalFunc(); });
    }
    else
    {
        LocalFunc();
    }

    function LocalFunc()
    {
        try
        {
            if (sendingObj.getAttribute("doonclick") != null &&
            sendingObj.getAttribute("doonclick") != "")
                eval(sendingObj.attributes["doonclick"].value);
            else
            {
                try
                {
                    sendingObj.click();
                } catch (e)
                {
                    eval(sendingObj.getAttribute("onclick"));
                }
            }

            //If this link hasn't been "fixed" by FixLinks() then evaluate the href attribute
            //It could be non-fixed when it has an onclick event. This is why we first eval the onclick attribute above
            if (sendingObj.tagName == "A" && sendingObj.href.substring(0, 11) == "javascript:")
            {
                eval(sendingObj.href.substring(11));
            }
        } catch (e)
        {
        }
    }

    return false;
}


//This function is used to alter the default onchange JS event for drop-downs.
//Actually, it affects only drop-downs that have the custom attribute CheckForChanges=true
//For those drop-downs it adds the CustomCheckFormSave() function that is used to check for unsaved changes before execute the default behaviour
function FixDropDowns()
{
    var dropDowns = document.getElementsByTagName("select");

    for (var i = 0; i < dropDowns.length; i++)
    {
        if (typeof dropDowns[i].onchange == "function" &&
         dropDowns[i].getAttribute("CheckForChanges") == "true")
        {
            //To be able to pass the ID of the sender drop-down to the notConfirmCallback function use this: http://www.mennovanslooten.nl/blog/post/62
            //This is necessary becasue of the loop; In loops clousers have a problem becasue there is a reference only for the last iteration of the loop
            //We need this to be able to return the original value of the drop-down if the user choose No

            var originalOnChange = dropDowns[i].onchange;
            dropDowns[i].onchange = function(dropDownId, originalValue)
            {
                return function()
                {
                    CustomCheckFormSave(function() { originalOnChange(); }, //Confirm callback - the original on change event
                                                                    function(ddId, orgValue) //NotConfirm callback - return the original value of the drop-down
                                                                    {
                                                                        return function()
                                                                        {
                                                                            document.getElementById(ddId).value = orgValue;
                                                                        }
                                                                    } (dropDownId, originalValue)
                                                                    );
                }
            } (dropDowns[i].id, dropDowns[i].value);
        }
    }
}


// Passing through all page controls and collect their values
// containerElement - This is used in a case when there is a need to refresh the values only of few inputs 
//                    on the page (that are part of a particular container). An example of this is the EquipmentReservistsRequest page - there are more than one Commands and for each command
//                    there is a separate section and Save button. In this case we need to refresh only the inputs of that command to be marked as "saved"
// overrideExistingValues - if this is True then override the value of all inputs that are already in the array
function GetValues(currentValues, containerElement, overrideExistingValues)
{
    var contElement = document;
    var overrideExistingVal = false;

    if (typeof containerElement != "undefined")
        contElement = containerElement;
   
    
    if (typeof overrideExistingValues != "undefined")
    {
        overrideExistingVal = overrideExistingValues;
    }

    var values = new Array();

    var inputs = contElement.getElementsByTagName("input");

    for (var i = 0; i < inputs.length; i++)
    {
        if (inputs[i].getAttribute("UnsavedCheckSkipMe") && inputs[i].getAttribute("UnsavedCheckSkipMe").toLowerCase() == "true")
            continue;

        var isInCurrentValues = false;

        if (typeof currentValues != "undefined")
        {
            for (var j = 0; j < currentValues.length; j++)
            {
                if (currentValues[j].id == inputs[i].id)
                {
                    isInCurrentValues = true;
                    break;
                }
            }
        }

        if (isInCurrentValues && !overrideExistingVal)
        {
            continue;
        }

        if (inputs[i].type == "text" || inputs[i].type == "")
        {
            values.push({ "id": inputs[i].id, "value": inputs[i].value });
        }

        if (inputs[i].type == "checkbox" || inputs[i].type == "radio")
        {
            values.push({ "id": inputs[i].id, "value": (inputs[i].checked ? "true" : "false") });
        }
    }

    var textareas = contElement.getElementsByTagName("textarea");

    for (var i = 0; i < textareas.length; i++)
    {
        if (textareas[i].getAttribute("UnsavedCheckSkipMe") && textareas[i].getAttribute("UnsavedCheckSkipMe").toLowerCase() == "true")
            continue;

        isInCurrentValues = false;

        if (typeof currentValues != "undefined")
        {
            for (var j = 0; j < currentValues.length; j++)
            {
                if (currentValues[j].id == textareas[i].id)
                {
                    isInCurrentValues = true;
                    break;
                }
            }
        }

        if (isInCurrentValues && !overrideExistingVal)
        {
            continue;
        }

        values.push({ "id": textareas[i].id, "value": textareas[i].value });
    }

    var selects = contElement.getElementsByTagName("select");

    for (var i = 0; i < selects.length; i++)
    {
        if (selects[i].getAttribute("UnsavedCheckSkipMe") && selects[i].getAttribute("UnsavedCheckSkipMe").toLowerCase() == "true")
            continue;

        isInCurrentValues = false;

        if (typeof currentValues != "undefined")
        {
            for (var j = 0; j < currentValues.length; j++)
            {
                if (currentValues[j].id == selects[i].id)
                {
                    isInCurrentValues = true;
                    break;
                }
            }
        }

        if (isInCurrentValues && !overrideExistingVal)
        {
            continue;
        }

        for (var j = 0; j < selects[i].options.length; j++)
        {
            if (selects[i].options[j].selected)
                values.push({ "id": selects[i].id, "value": selects[i].options[j].value });
        }
    }

    return values;
}

// set no changes check at the moment, used to prevent checking for changes on save buttons, for example
function ForceNoChanges()
{
    CanLeave = true;
}

// set changes check at the moment
function ForceCheck()
{
    CanLeave = false;
}

//Force the functionality to always ask the user
function ForceAsk()
{
    alwaysAskCnt++;
}

function UndoForceAsk()
{
    alwaysAskCnt--;

}

function ResetAlawaysAsk()
{
    alwaysAskCnt = 0;
}

function AreThereUnsavedChanges()
{
    var result = false;

    var alwaysCanLeave = true;

    // check if in the web config the key for activating this functionality is true
    if (document.getElementById("ShowPopupForSaveChanges") &&
      document.getElementById("ShowPopupForSaveChanges").value.toUpperCase() == "TRUE")
        alwaysCanLeave = false;

    // check if on the page there is hidden field "CanLeave" set to true - then the page is not checked for changes
    if (document.getElementById("CanLeave"))
    {
        if (document.getElementById("CanLeave").value == "true")
            alwaysCanLeave = true;
    }

    // check if page check for changes has to be ommited due to some reason
    if (CanLeave || alwaysCanLeave)
    {
        result = false;
    }
    else if (alwaysAskCnt > 0)
    {
        result = true;
    }
    else
    {
        var currentValues = GetValues();

        //Compare the current input values and the original ones
        for (var i = 0; i < currentValues.length; i++)
        {
            for (var j = 0; j < originalValues.length; j++)
            {
                //Debug 
                /*
                if(currentValues[i].id == originalValues[j].id)
                {
                alert("Element: " + currentValues[i].id + "; Old: " + originalValues[j].value + "; New: " + currentValues[i].value + ";");
                }
                */

                if (currentValues[i].id == originalValues[j].id &&
                    currentValues[i].value != originalValues[j].value) {
                    result = true;
                    break;
                }
            }

            if (result)
                break;
        }
    }

    return result;
}

//Custom confirm dialog; avoid onbeforeunload!
function CustomCheckFormSave(confirmCallback, notConfirmCallback)
{
    if (AreThereUnsavedChanges())
    {
        YesNoDialog("Има незапазени промени на този екран, които ще бъдат изгубени. <br/> Сигурни ли сте, че искате да продължите?", function() { ForceNoChanges(); confirmCallback(); }, notConfirmCallback);
    }
    else
    {
        confirmCallback();
    }
}