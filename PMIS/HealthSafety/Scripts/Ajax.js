/*
This is a generic functionality that should be used to implement all AJAX calls
This code creates the XMLHTTP object, set up its parameters and executes the AJAX call
The response should be XML (it is returned by the Ajax.cs functionality)
*/

//Counts the current ajax calls
Processes = 0;

//Possible statuses
ERROR = "ERROR";
NOTLOGGEDIN = "NOTLOGGEDIN";
NOACCESS = "NOACCESS";

//OOP is JS
function AJAX(url, asynchronous, parameters, callback)
{
   this.Url = url; //The URL of the specific ajax call
   this.Asynchronous = asynchronous; //Flag is the request shoudl be Synchronous or Asynchronous
   this.Parameters = parameters; //Pass any parameters if we need a POST query
   this.Callback = callback; //This is the callback function in a case of Asynchronous query
   this.DoNotShowIndicator = false;
   
   //Cross-browser creation of the XMLHTTPRequest object
   this.GetXMLHTTPObj = function()
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
      
      if (!obj && typeof XMLHttpRequest != "undefined")
      {
         obj = new XMLHttpRequest();
      }
      
      return obj;	
   }
   
   //This method exectued the AJAX query
   this.Call = function()
   {
       if (typeof isSessionExpired != "undefined" && isSessionExpired)
       {
           location.href = location.href;
           return;
       }
   
      //Increment the counter of the ajax calls to be able to show an indicator (ajax loader gif)
      if(!this.DoNotShowIndicator)
         Processes++;
  
      RefreshProcessingAndButtons();

      var Callback = this.Callback;
   
      //Prevent cache
      var d = new Date();
      var i = d.getTime();
      var unique = i.toString(10);
      
      this.Url += (this.Url.indexOf('?') >= 0 ? "&" : "?") + "qunique=" + unique;
      
      var objXMLHTTP = this.GetXMLHTTPObj();
   
      //If there are Parameters the use POST query
      var method = this.Parameters != null ? "post" : "get"

      objXMLHTTP.open(method, this.Url, this.Asynchronous);

      AttachTimeout();   
      
      //If the query is Asynchronous then chech the status and when the HTTP request is done the call the callback function   
      if(this.Asynchronous)
         objXMLHTTP.onreadystatechange = function() 
         {  
            if (objXMLHTTP.readyState == 4 && objXMLHTTP.responseXML)
            {  
               if(!this.DoNotShowIndicator)
                  Processes--;
                  
               RefreshProcessingAndButtons();
               
               try
               {
                  objXMLHTTP.responseXML.normalize();
               }
               catch(e) {}
               
               var xmlDoc = objXMLHTTP.responseXML.documentElement;
               
               if(xmlDoc == null)
               {
                  alert("The XML response is not valid!");
                  if(typeof DisableAllButtons == "function")
                     DisableAllButtons(false);
                  return;
               }
               
               //Parse the results (our XML protocol) and pass the Callback funtion to be called
               ParseResults(xmlDoc, Callback);
            }
         }

      objXMLHTTP.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
      objXMLHTTP.send(this.Parameters);
      
      if(!this.Asynchronous)
      {       
          if(!this.DoNotShowIndicator)
             Processes--;
            
          RefreshProcessingAndButtons();
            
          try
          {
             objXMLHTTP.responseXML.normalize(); //in firefox it returns only 4096 bytes for a xml node! this fixes the problem
          }
          catch(e) {}
          
          var xmlDoc = objXMLHTTP.responseXML.documentElement;
          
          if(xmlDoc == null)
          {
             alert("The XML response is not valid!");
             if(typeof DisableAllButtons == "function")
               DisableAllButtons(false);
             return;
          }
          
          ParseResults(xmlDoc, Callback);
      }
   }
   
   //Check the Status and if everything is OK and there is a Callback function the call it
   //If something is wrond (e.g. ERROR or NOTLOGGEDIN) then alert the error message
   function ParseResults(xmlDoc, Callback)
   {
      var aStatus = xmlDoc.getElementsByTagName("status");
      var aResponse = xmlDoc.getElementsByTagName("response");
      if(aStatus.length <= 0 || aResponse.length <= 0)
      {
         alert("The XML response is not valid!");
         if(typeof DisableAllButtons == "function")
            DisableAllButtons(false);
            return;
      }
   
      var status = aStatus[0].childNodes[0].nodeValue;
      var message = aResponse[0].childNodes[0].nodeValue;
      
      if(status == ERROR)
      {          
         alert(message);
         if(typeof DisableAllButtons == "function")
            DisableAllButtons(false);
         return;
      }
      
      if(status == NOACCESS)
      {          
         alert(message);
         if(typeof DisableAllButtons == "function")
            DisableAllButtons(false);
         return;
      }
   
      if(status == NOTLOGGEDIN)
      {
         alert(message);
         if(typeof DisableAllButtons == "function")
            DisableAllButtons(false);
         return;
      }else
      {          
         //If everything is OK and there is a callback function the call it
         //Pass the response to it
         if(typeof Callback == "function")
         {
            var response = aResponse[0];
            Callback(response);

            //RefreshDatePickers();
         }
      }
   }
      
}

//Escape some special character to be able to pass them via the AJAX call
function custEncodeURI(url) {
    url = encodeURI(url);
    url = url.replace(/&/g,"%26");
    url = url.replace(/\+/g,"%2B");
    
    return url;
}

function custDecodeURI(url) {
    url = decodeURI(url);    

    return url;
}

function RefreshProcessingAndButtons()
{
   if(Processes > 0)
   {
      if(typeof DisableButtons == "function")
      {
         DisableButtons();
      }
      
      if(typeof ShowAjaxLoader == "function")
      {
         ShowAjaxLoader();
      }
   }
   else
   {
      if(typeof EnableButtons == "function")
      {
         EnableButtons();
      }
   
      if(typeof HideAjaxLoader == "function")
      {
         HideAjaxLoader();
      }
   }
}
