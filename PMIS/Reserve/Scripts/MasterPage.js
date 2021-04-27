//The menu bar items call this function to navigate to a specific screen
function MenuItemClick(url)
{
   CustomCheckFormSave(function(){RedirectLocalFunc(url);});

   function RedirectLocalFunc(url)
   {
      if(url == "")
         return;

      var index = url.indexOf("?");
   
      if (index < 0)
         url = url + "?fm=1";
      else
         url = url + "&fm=1";

      JSRedirect(url);
   }
}

//Call this to refresh the date pickers on the page after each UpdatePanel refresh
function RefreshDatePickers()
{
    if (datePickerController != null)
        datePickerController.create();
}

//Disable client control on the page
function DisableControl(c)
{
    c.disabled = true;
}

//Hide client control on the page
function HideControl(c)
{
    c.style.display = "none";
}

//Disable client controls on the page, included in hidden field "hdnDisabledClientControls"
function CheckDisabledClientControls()
{
    var controlsString = document.getElementById(hdnDisabledClientControls).value;
    var controls = controlsString.split(",");       
    
    for (var c in controls)
    {
        if (document.getElementById(controls[c]) != null)
        {
            DisableControl(document.getElementById(controls[c]))
        }
    }
}

//Hide client controls on the page, included in hidden field "hdnHiddenClientControls"
function CheckHiddenClientControls()
{
    var controlsString = document.getElementById(hdnHiddenClientControls).value;
    var controls = controlsString.split(",");       
    
    for (var c in controls)
    {
        if (document.getElementById(controls[c]) != null)
        {
            HideControl(document.getElementById(controls[c]))
        }
    }
}

//Call this handler each time before the UpdatePanel starts a refresh process
function BeginRequestHandler(sender, args)
{
    DisableButtons();
    ShowAjaxLoader();
}

//Call this handler each time the UpdatePanel refreshes itself
function EndRequestHandler(sender, args)
{
    RefreshDatePickers();
    
    CheckDisabledClientControls();
    CheckHiddenClientControls();
    
    CheckForSavedChanges();
    FixLinks();
    FixDropDowns();
    ForceCheck();
    SetHash();
    EnableButtons();
    HideAjaxLoader();
}

var sessionTimeout;
var sessionTimeoutSec;  
var one_second = 1000;
var one_minute = 60 * 1000;
var diffTime = 0;
var now;
var timer = 0;
var secondsToEnd;
var initialDate;
//keeps the date of session expiration
var dateExpiration;

//Setup master page to handle session timeout
function SessionTimeoutOnLoad()
{
   document.getElementById(hdnClientTime).value = new Date();
   
   if(document.getElementById("Logout"))
   {      
      if(document.getElementById("Logout").value == "true")
         eraseCookie('pmisRESTimeout');
   }
   
   if(document.getElementById("Login"))
   {      
      if(document.getElementById("Login").value == "true")
         eraseCookie('pmisRESTimeout');
   }

   if(!document.getElementById("Login") && !document.getElementById("Logout"))
   {
    now = GetNewTime();
    var timeBeforeSessionExpires = confTimeBeforeSessionExpires;
    secondsToEnd = parseInt(timeBeforeSessionExpires);
    if(isNaN(secondsToEnd))
        secondsToEnd = 300; 
    
    var sessionTime = parseInt(sessionTimeout - 1);
    if(secondsToEnd > sessionTime * 60)
       secondsToEnd = sessionTime * 60;
    
    AttachTimeout();
    AttachTimeoutActionsSys(secondsToEnd, now, initialDate, sessionTime);
   }
}

function GetNewTime()
{
    var strNow = dateTimeNow;
    var currentDate = new Date(strNow);
    var currentTime = currentDate.getTime();
    var currentClientDate = new Date();
    var currentClientTime = currentClientDate.getTime();
    var lastClientDate = new Date(document.getElementById(hdnClientTime).value);
    var lastClientTime = lastClientDate.getTime();
    return new Date(currentTime + currentClientTime - lastClientTime); 
}

// Handle session timeout(prolong it on activity, check for expiration, display appropriate alert message)
function AttachTimeout()
{ 
    now = GetNewTime();

    var curdate=now.getTime();
    
    var newArray = document.getElementById(hdnInitialTime).value.split(':');
    initialDate = new Date(newArray[2], newArray[0] - 1, newArray[1]);
    initialDate.setHours(newArray[3],newArray[4],newArray[5], newArray[6]);

    var initialTimePostBack = initialDate.getTime();
    
    diffTime = (curdate - initialTimePostBack) / one_minute;

    if(diffTime >= (sessionTimeout - 1)/2)
    {
       sessionTimeoutSec = (sessionTimeout - 1) * 60;
       var month = now.getMonth() + 1;
       document.getElementById(hdnInitialTime).value = month + ':' + now.getDay() + ':' + 
       now.getFullYear() + ':' + now.getHours() + ':' + now.getMinutes() + ':' + now.getSeconds() + ':' + now.getMilliseconds();
    }
    else
    {
       var currentDiffTimeSec = (curdate - initialTimePostBack) / one_second;
       sessionTimeoutSec = (sessionTimeout - 1) * 60 - currentDiffTimeSec;
    }
    
   
   if(document.getElementById(hdnSessionTimeout).value != "")
   {
       sessionTimeoutSec = (sessionTimeout - 1) * 60;
   }
   
   dateExpiration = new Date(curdate + sessionTimeoutSec * 1000);
   AttachTimeoutActions(secondsToEnd, now, sessionTimeout - 1);
   TimeoutChecker();
}

// Check if needed to refresh original values(when page is saved)
function CheckForSavedChanges()
{
    if (hdnSavedChangesID != null)
    {            
        if (document.getElementById(hdnSavedChangesID).value == "True")
        {
            LoadOriginalValues();
                
            document.getElementById(hdnSavedChangesID).value = "False";
        }
    }

    if (hdnSavedChangesContainerID != null)
    {
        if (document.getElementById(hdnSavedChangesContainerID).value != "")
        {
            RefreshInputsOfSpecificContainer(document.getElementById(document.getElementById(hdnSavedChangesContainerID).value), true);

            document.getElementById(hdnSavedChangesContainerID).value = "";
        }
    }
}

//Generate a special error message when the user has no rights to modify a required field
function GetErrorMessageNoRights(fieldNames)
{
   var fieldsStr = "";
   var msg = "";
   
   for (var i = 0; i < fieldNames.length; i++)
   {
       if (i == 0)
       {
           fieldsStr = "\"" + fieldNames[i] + "\"";
       }
       else
       {
           fieldsStr += (i < fieldNames.length - 1 ? ", " : " и ") + "\"" + fieldNames[i] + "\"";
       }
   }
       
   if (fieldNames.length == 1)
     msg = "<i>" + errorMessageNoRightsFieldTemplate.replace("#FIELD#", fieldsStr) + "</i>";
   else if (fieldNames.length > 1)    
     msg = "<i>" + errorMessageNoRightsFieldsTemplate.replace("#FIELDS#", fieldsStr) + "</i>";
     
   return msg;

}

// Generate a standard error message for mandatory fields
function GetErrorMessageMandatory(fieldName)
{
    return errorMessageMandatoryTemplate.replace("#FIELD#", fieldName);
}

// Generate a standard error message for date fields
function GetErrorMessageDate(fieldName)
{
    return errorMessageDateTemplate.replace("#FIELD#", fieldName);
}

// Generate a standard error message for number fields
function GetErrorMessageNumber(fieldName)
{
    return errorMessageNumberTemplate.replace("#FIELD#", fieldName);
}

// Generate a standard error message for mandatory column
function GetErrorMessageMandatoryColumn(columnName)
{
    return errorMessageMandatoryColumnTemplate.replace("#FIELD#", columnName);
}

// Generate a standard error message for date column
function GetErrorMessageDateColumn(columnName)
{
    return errorMessageDateColumnTemplate.replace("#FIELD#", columnName);
}

// Generate a standard error message for number column
function GetErrorMessageNumberColumn(columnName)
{
    return errorMessageNumberColumnTemplate.replace("#FIELD#", columnName);
}

function SetHash()
{
   if(document.getElementById(hdnLocationHashClientID))
   {
      var locationHash = document.getElementById(hdnLocationHashClientID).value;
      
      if(locationHash != "")
         location.hash = locationHash;
         
      document.getElementById(hdnLocationHashClientID).value = "";
   }
}

function CheckHash()
{
   var hash = location.hash;
      
   if(hash != null && hash != "")
   {
      if(hash.substring(0, 1) == "#")
         hash = hash.substring(1);

      JSRedirect(hash);
   }
   else
   {
      document.getElementById("divContent").style.display = "";
   }
}

//Use this function to disable all buttons on the screen to prevent double clicking while the request hasn't finished yet
function DisableButtons()
{
   //This if for Server buttons; They are rendered as links
   var buttons = document.getElementsByTagName("a");
   
   for(var i = 0; i < buttons.length; i++)
   {
      var btn = buttons[i];
      DisableButtonLocalFunc(btn);
   }
   
   //This is for Client buttons; We render them as Divs
   buttons = document.getElementsByTagName("div");
   
   for(var i = 0; i < buttons.length; i++)
   {
      var btn = buttons[i];
      DisableButtonLocalFunc(btn);
   }
   
   function DisableButtonLocalFunc(btn)
   {
      if(btn.className == "Button")
      {
         if(btn.getAttribute("olddisabled") == null ||
            btn.getAttribute("olddisabled") == "") {
             /*2019-12-17: We found that this approach doesn't work in Chrome/Firefox. The problem is caused by the fact that the buttons are actually <div> or <a> elements and they don't support the .disable propery in browsers other than IE.
               We tried a couple of possible options (e.g. prevent the click event) and finally we found this to be working best - display an overlay DIV (actually P) over the button and make it transparent.
               There problem with positioning the DIV exactly over each button on the page. So, finally we put it inside the button. This caused some troubles with preventing the button's click.
               That is why we had to use the stopPropagation() (see below) which doesn't work in IE. Because of this and because IE behaves differently in other areas (e.g. configuring the transparency), that is why we decided to
               keep the old code (using the disabled attribute) for IE and apply the new one only for browsers other than IE.
             */
             if (isIE()) {
                 btn.setAttribute("olddisabled", btn.disabled ? "true" : "false");
                 btn.disabled = true;
             }
             else {
                 disableButtonNotIE(btn);
             }
         }
      }
   }

}

function isIE() {
    var ua = window.navigator.userAgent;
    var msie = ua.indexOf("MSIE ");

    if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)){ //Check for IE 11 as well
        return true;
    }

    return false;
}

function disableButtonNotIE(btn) {
    var disablerID = btn.id + "_disableoverlay";
    var existingDisabler = document.getElementById(btn.id + "_disableoverlay");

    //During testing we found that the DisableButtons()could be called multiple time.
    //It happened that every time a new overlay DIV (P) element was created. That is why we need this check.
    if (existingDisabler)
        return;

    //We indended to use a DIV element, however, we found that the ".Button div" class in CSS casued some unexpected behaviour. So, we decided to use the P element.
    var div = document.createElement("p");
    
    //In some cases the button's size was 0 (may be if the element is invsible). To prevent error because of negative height, we added this check:
    if (btn.children[1].clientWidth > 0) {
        div.style.width = (btn.children[1].clientWidth + 11 * 2) + "px";
        div.style.height = (btn.children[1].clientHeight - 5) + "px"
    }
    
    div.className = "ButtonDisablerNotIE";
    div.id = disablerID
    
    //The disabler element is inside the button. That is why we need to prevent the default event propagation
    addEvent("click", div, stopEvent);
    
    btn.children[1].appendChild(div);
}

function enableButtonNotIE(btn) {
    var disablerID = btn.id + "_disableoverlay";
    var existingDisabler = document.getElementById(btn.id + "_disableoverlay");

    //Sometimes we call the "enable buttons" function but the button has not been disabled (e.g. the Update Panel has been refreshed and the button is renedred as enabled (the default state from the server))
    //So, try removing only if is there
    if (existingDisabler)
        btn.children[1].removeChild(existingDisabler);
}

//This is just a cross-browser "add event" function
function addEvent(evnt, elem, func) {
    if (elem.addEventListener)  // W3C DOM
        elem.addEventListener(evnt, func, false);
    else if (elem.attachEvent) { // IE DOM
        elem.attachEvent("on" + evnt, func);
    }
    else { // No much to do
        elem["on" + evnt] = func;
    }
}

function stopEvent(ev) {
    //Prevent the parent (the button) click function to be called when triggering the disabled (the inside element) click
    ev.stopPropagation();
}

//Use this function to disable all buttons on the screen to prevent double clicking while the request hasn't finished yet
function EnableButtons()
{
   //This if for Server buttons; They are rendered as links
   var buttons = document.getElementsByTagName("a");
   
   for(var i = 0; i < buttons.length; i++)
   {
      var btn = buttons[i];
      EnableButtonLocalFunc(btn);
   }
   
   //This is for Client buttons; We render them as Divs
   buttons = document.getElementsByTagName("div");
   
   for(var i = 0; i < buttons.length; i++)
   {
      var btn = buttons[i];
      EnableButtonLocalFunc(btn);
   }

   function EnableButtonLocalFunc(btn) {
       if (btn.className == "Button") {
           if (isIE()) {
               if (btn.getAttribute("olddisabled") != null && btn.getAttribute("olddisabled") != "") {
                   btn.disabled = btn.getAttribute("olddisabled") == "true";
                   btn.setAttribute("olddisabled", "");
               }
           }
           else {
               enableButtonNotIE(btn);
           }
       }
   }
}

//Display the ajax loading image
function ShowAjaxLoader()
{
   if(document.getElementById("divAjaxLoading"))
      document.getElementById("divAjaxLoading").style.display = "";
}

//Hide the ajax loading image
function HideAjaxLoader()
{
   if(document.getElementById("divAjaxLoading"))
      document.getElementById("divAjaxLoading").style.display = "none";
   
}

//YesNoDialog
var yesCallbackGlobal = null;
var noCallbackGlobal = null;

function YesNoDialog(message, yesCallback, noCallback)
{
   yesCallbackGlobal = yesCallback;
   noCallbackGlobal = noCallback;
   
   document.getElementById("lblYesNoDialogMessage").innerHTML = message;
   
   document.getElementById("HidePageYesNo").style.display = "";
   document.getElementById("divYesNoDialog").style.display = "";
   CenterLightBox("divYesNoDialog");
}

function HideYesNoDialog()
{
    document.getElementById("HidePageYesNo").style.display = "none";
    document.getElementById("divYesNoDialog").style.display = "none";

    yesCallbackGlobal = null;
    noCallbackGlobal = null;
}

function YesAnswer()
{
   if(yesCallbackGlobal != null)
   {
      yesCallbackGlobal();
   }
   
   HideYesNoDialog();
}

function NoAnswer()
{
   if(typeof noCallbackGlobal != "undefined" && 
      noCallbackGlobal != null)
   {
      noCallbackGlobal();
   }

   HideYesNoDialog();
}

//AlertDialog
var alertDialog_yesCallbackGlobal = null;

function AlertDialog(message, yesCallback) {
    alertDialog_yesCallbackGlobal = yesCallback;


    document.getElementById("lblAlertDialogMessage").innerHTML = message;

    document.getElementById("HidePageYesNo").style.display = "";
    document.getElementById("divAlertDialog").style.display = "";
    CenterLightBox("divAlertDialog");
}

function HideAlertDialog() {
    document.getElementById("HidePageYesNo").style.display = "none";
    document.getElementById("divAlertDialog").style.display = "none";

    alertDialog_yesCallbackGlobal = null;
}

function AlertDialog_YesAnswer() {
    if (alertDialog_yesCallbackGlobal != null) {
        alertDialog_yesCallbackGlobal();
    }

    HideAlertDialog();
}

//InfoPopup
var mouseInInfoPopupDiv = false;

function ShowInfoPopup(title, content, fixed, up, e)
{
    var InfoPopup = document.getElementById("InfoPopup");

    document.getElementById("divInfoPopupTitle").innerHTML = title;
    document.getElementById("divInfoPopupContent").innerHTML = content;

    InfoPopup.style.position = fixed ? "fixed" : "absolute";

    var posx = 0;
    var posy = 0;
    if (!e) var e = window.event;
    if (e.pageX || e.pageY)
    {
        posx = e.pageX;
        posy = e.pageY;
    }
    else if (e.clientX || e.clientY)
    {
        posx = e.clientX + (!fixed ? document.body.scrollLeft + document.documentElement.scrollLeft : 0);
        posy = e.clientY + (!fixed ? document.body.scrollTop + document.documentElement.scrollTop : 0);
    }

    InfoPopup.style.top = posy - (up ? 150 : 0);
    InfoPopup.style.left = posx;

    if (document.layers)
    {
        document.captureEvents(Event.MOUSEUP);
    }

    var popupWindowOldEventListener = document.onmouseup;

    if (popupWindowOldEventListener != null)
    {
        document.onmouseup = function() { popupWindowOldEventListener(); HideInfoPopup(); };
    }
    else
    {
        document.onmouseup = HideInfoPopup();
    }

    document.body.addEventListener ? document.body.addEventListener("onkeydown", InfoPopupKeyPress, false) : document.body.attachEvent("onkeydown", InfoPopupKeyPress);

    InfoPopup.style.display = "";

}

function InfoPopupKeyPress(event)
{
    if (document.getElementById("InfoPopup"))
    {
        var code = event.keyCode ? event.keyCode : event.charCode;

        if (code == 27)
        {
            HideInfoPopup(true);              
            return true;
        }       
    }

    return true;
}

function HideInfoPopup(force)
{
    if (document.getElementById("InfoPopup").style.display == "")
    {
        if (!mouseInInfoPopupDiv || force)
            document.getElementById("InfoPopup").style.display = "none";

        document.body.removeEventListener ? document.body.removeEventListener("onkeydown", InfoPopupKeyPress, false) : document.body.detachEvent("onkeydown", InfoPopupKeyPress);
    }
}
