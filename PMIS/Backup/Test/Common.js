function isInt(s)
{
    return (parseInt(s) == Number(s));
}

function isDecimal (s) {
   var decimalRegExpS = '^\\s*(\\+|-)?((\\d+(\\' + DecimalPoint + '\\d+)?)|(\\' + DecimalPoint + '\\d+))\\s*$';   
   var isDecimal_re = new RegExp(decimalRegExpS, 'gi');
   return isDecimal_re.test(s);
}

function xmlValue(xml, key)
{      
   if (xml.getElementsByTagName(key)[0].childNodes[0] != null)
        return xml.getElementsByTagName(key)[0].childNodes[0].nodeValue;
   else
        return "";
}

function xmlNodeText(xmlNode) {
    if (typeof xmlNode.text !== "undefined")
        return xmlNode.text;
    else
        return xmlNode.textContent;
}

function setInnerText(elem, textVal) {
    if (typeof elem.textContent !== "undefined") {
        elem.textContent = textVal;
    } else {
        elem.innerText = textVal;
    }
}

function IsValidDate(dateStr)
{
    var splitted;

    if (dateStr.lastIndexOf(".") > -1 && dateStr.lastIndexOf("/") == -1)
    {
        splitted = dateStr.split(".");
    }
    else if (dateStr.lastIndexOf(".") == -1 && dateStr.lastIndexOf("/") > -1)
    {
        splitted = dateStr.split("/");
    }
    else
    {
        return false;
    }

    if (splitted[2].length != 4)
    {
        return false;
    }

    var day;
    var month;
    var year;
    var leap = 0;

    if (isInt(splitted[0]) && isInt(splitted[0]) && isInt(splitted[0]))
    {
        var day = parseInt(splitted[0]);
        var month = parseInt(splitted[1]);
        var year = parseInt(splitted[2]);
    }
    else
    {
        return false;
    }

    if (((year % 4 == 0) && (year % 100 != 0)) || ((year % 100 == 0) && (year % 400 == 0)))
    {
        leap = 1;
    }

    if (month < 1 || month > 12)
    {
        return false;
    }

    if ((month == 2) && (leap == 1) && (day > 29))
    {
        return false;
    }

    if ((month == 2) && (leap != 1) && (day > 28))
    {
        return false;
    }

    if ((day > 31) && ((month == "01") || (month == "03") || (month == "05") || (month == "07") || (month == "08") || (month == "10") || (month == "12")))
    {
        return false;
    }

    if ((day > 30) && ((month == "04") || (month == "06") || (month == "09") || (month == "11")))
    {
        return false;
    }

    return true;
}

function TrimString(s)
{
    return s.replace(/^\s\s*/, '').replace(/\s\s*$/, '');
}

String.prototype.Trim = function() { return TrimString(this);};

function doKeypress(event, control)
{
    maxLength = control.attributes["maxLength"].value;
    value = control.value;
     if(maxLength && value.length > maxLength-1){
          event.returnValue = false;
          maxLength = parseInt(maxLength);
     }
}

// Cancel default behavior
function doBeforePaste(event, control)
{
    maxLength = control.attributes["maxLength"].value;
     if(maxLength)
     {
          event.returnValue = false;
     }
}

// Cancel default behavior and create a new paste routine
function doPaste(event, control)
{
    maxLength = control.attributes["maxLength"].value;
    value = control.value;
     if(maxLength){
          event.returnValue = false;
          maxLength = parseInt(maxLength);
          var oTR = control.document.selection.createRange();
          var iInsertLength = maxLength - value.length + oTR.text.length;
          var sData = window.clipboardData.getData("Text").substr(0,iInsertLength);
          oTR.text = sData;
     }                         
}

function isValidHour(hourValue)
   {
   if(!isInt(hourValue))  return false;
   return((hourValue >=0)&&(hourValue<=23));
   }
function isValidMin(minValue)
   {
   if(!isInt(minValue))  return false;
   return((minValue >=0)&&(minValue<=59));
   }
function isValidEgn(egnValue)
   {
   if(!isInt(egnValue))  return false;
   return(egnValue.length==10);
   }  
   
function SetClientTextAreaMaxLength(id, maxLength)
{
    var textArea = document.getElementById(id);
    
    textArea.setAttribute("maxLength", maxLength);
    textArea.setAttribute("onkeypress", "doKeypress(event, this);");
    textArea.setAttribute("onbeforepaste", "doBeforePaste(event, this);");
    textArea.setAttribute("onpaste", "doPaste(event, this);");
} 