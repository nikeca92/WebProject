var session;
var timeBeforeSessionExpires;
var initialDate;
var currentDate;
var time;
var timeLastMinute;
var timeXmlHttp;

function AttachTimeoutActions(pTimeBeforeSessionExpires, now, pSession)
{
    session = pSession;
    currentDate = now;
    timeBeforeSessionExpires = pTimeBeforeSessionExpires;
    DisplaySessionTimeout();
}

function MakeXMLHTTPObj()
{
   var obj = null;
   try
      {
         obj = new ActiveXObject("Msxml2.XMLHTTP")
      }
      catch(e)
         {
            try
               {
                  obj = new ActiveXObject("Microsoft.XMLHTTP")
               }
               catch(sc)
               {
                  obj = null;
               }				 
         } 	
   if (!obj && typeof XMLHttpRequest!="undefined")
      {
         obj = new XMLHttpRequest();
	  }
	   
   return obj;	   
}

function AttachTimeoutActionsSys(pTimeBeforeSessionExpires, pCurrentDate, pInitialDate, pSession)
{

   session = pSession;
   timeBeforeSessionExpires = pTimeBeforeSessionExpires;
   currentDate = pCurrentDate;
   initialDate = pInitialDate;
   Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandlerTimeout);

}

function BeginRequestHandlerTimeout(sender, args)
{   
    var one_second = 1000;
    var one_minute = 60 * 1000;
    var currentDate = GetNewTime();
    var curdate=currentDate.getTime();
    var initialTimePostBack = initialDate.getTime(); 

    diffTime = (curdate - initialTimePostBack) / one_minute;
    if(diffTime >= session/2)
    {
       sessionTimeoutSec = session * 60;
    }
    else
    {
       var currentDiffTimeSec = (curdate - initialTimePostBack) / one_second;
       sessionTimeoutSec = session * 60 - currentDiffTimeSec;
    }

    dateExpiration = new Date(curdate + sessionTimeoutSec * 1000);
    DisplaySessionTimeout(); 
}

function DisplaySessionTimeout()
{
      var curdate = currentDate.getTime();
      var cookieExpiration = Date.parse(readCookie('pmisHSTimeout'));
      if(!cookieExpiration)
      {
         createCookie('pmisHSTimeout',dateExpiration);
      }
      else
      {
        var cookieDate = new Date(cookieExpiration);
        if(cookieDate < dateExpiration)
           createCookie('pmisHSTimeout',dateExpiration);
        else  if(cookieDate > dateExpiration)
        {
            sessionTimeoutSec = (cookieExpiration - curdate) / one_second;
        }
      }
        window.clearTimeout(time);  
        window.clearTimeout(timeLastMinute);
        window.clearTimeout(timeXmlHttp);
        if(document.getElementById("spanSessionTime")){
            document.getElementById("spanSessionTime").style.display = "none";  
        }
        var timeBeforeExpires = 1000 * timeBeforeSessionExpires;
        var secondsToEnd = timeBeforeSessionExpires;
        var timeLeft = parseInt(sessionTimeoutSec);           

        if(timeLeft > secondsToEnd)
            timeLeft = secondsToEnd;

        var timeToWait = parseInt(sessionTimeoutSec * 1000 - timeBeforeExpires);
        time = window.setTimeout("DisplayTimeLeft(" + timeLeft + ")", timeToWait);
   
}

function CheckSessionTimeout(newDate)
{
      var one_second = 1000;
      var cookieValue = readCookie('pmisHSTimeout');
      if(!cookieValue)
      {
         if(document.getElementById("spanSessionTime"))
         {
            document.getElementById("spanSessionTime").style.display = "";
            setInnerText(document.getElementById("lblSessionTime"), "Вашата сесия е изтекла");
         }
         window.clearTimeout(time);
         window.clearTimeout(timeLastMinute);
         return;
      }
      var cookieExpiration = Date.parse(cookieValue);
      if(cookieExpiration)
      {
         var cookieDate = new Date(cookieExpiration);
         if(cookieDate < dateExpiration){
            createCookie('pmisHSTimeout',dateExpiration);
            }
         else  if(cookieDate > dateExpiration)
         {
            sessionTimeoutSec = (cookieExpiration - newDate) / one_second;
            dateExpiration  = cookieDate;
            window.clearTimeout(time);
            window.clearTimeout(timeLastMinute);
            if(document.getElementById("spanSessionTime"))
                document.getElementById("spanSessionTime").style.display = "none";
            var timeBeforeExpires = 1000 * timeBeforeSessionExpires;
            var secondsToEnd = timeBeforeSessionExpires;
            var timeLeft = parseInt(sessionTimeoutSec);   
            if(timeLeft >= secondsToEnd)
               timeLeft = secondsToEnd;
            time = window.setTimeout("DisplayTimeLeft(" + timeLeft + ")", sessionTimeoutSec * 1000 - timeBeforeExpires);
         }
      }
}

//function that check each 5 seconds if there is change in the cookie and refresh it
function TimeoutChecker()
{
       var currentDate = GetNewTime();
       CheckSessionTimeout(currentDate.getTime());
       //CheckSessionTimeout(newDate);
       //newDate = newDate + 5000;
       window.setTimeout("TimeoutChecker()", 5000);
}

//function that makes countdown each second     
function DisplayTimeLeft(seconds)
{
      if(document.getElementById("spanSessionTime"))
        document.getElementById("spanSessionTime").style.display = "";
      var minutes = parseInt(seconds / 60);
      var secondsInMinute = seconds % 60;
      if(secondsInMinute < 10) 
         secondsInMinute = "0" + seconds % 60;
      
      if(document.getElementById("lblSessionTime"))
          setInnerText(document.getElementById("lblSessionTime"), "До изтичането на Вашата сесия остават " + minutes + ":" + secondsInMinute);
      seconds = seconds - 1;
      if(seconds >= 0 )
      {
           timeLastMinute = window.setTimeout("DisplayTimeLeft(" + seconds + ")", 1000);
      }
      else
      {
         setInnerText(document.getElementById("lblSessionTime"), "Вашата сесия е изтекла");
         eraseCookie('pmisHSTimeout');  
      } 
                                                                       
}

function createCookie(name,value)
{
	document.cookie = name + "=" + value + "; path=/";
}

function readCookie(name) {
	var nameEQ = name + "=";
	var ca = document.cookie.split(';');
	for(var i=0;i < ca.length;i++) {
		var c = ca[i];
		while (c.charAt(0)==' ') c = c.substring(1,c.length);
		if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length,c.length);
	}
	return null;
}

function eraseCookie(name) {
	createCookie(name,"");
}





