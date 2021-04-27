var optionChooseOneValue = "-1";
var optionAllValue = "-1";

function isInt(s)
{
    return (parseInt(s) == Number(s));
}

function isOnlyDigits(s)
{
    var digitsRegExpS = '^\\s*\\d+\\s*$';
    var isOnlyDigits_re = new RegExp(digitsRegExpS, 'gi');
    return isOnlyDigits_re.test(s);
}

function isDecimal(s)
{
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

String.prototype.Trim = function() { return TrimString(this); };

function doKeypress(event, control)
{
    maxLength = control.attributes["maxLength"].value;
    value = control.value;
    if (maxLength && value.length > maxLength - 1)
    {
        event.returnValue = false;
        maxLength = parseInt(maxLength);
    }
}

// Cancel default behavior
function doBeforePaste(event, control)
{
    maxLength = control.attributes["maxLength"].value;
    if (maxLength)
    {
        event.returnValue = false;
    }
}

// Cancel default behavior and create a new paste routine
function doPaste(event, control)
{
    maxLength = control.attributes["maxLength"].value;
    value = control.value;
    if (maxLength)
    {
        event.returnValue = false;
        maxLength = parseInt(maxLength);
        var oTR = control.document.selection.createRange();
        var iInsertLength = maxLength - value.length + oTR.text.length;
        var sData = window.clipboardData.getData("Text").substr(0, iInsertLength);
        oTR.text = sData;
    }
}

function isValidHour(hourValue)
{
    if (!isInt(hourValue)) return false;
    return ((hourValue >= 0) && (hourValue <= 23));
}
function isValidMin(minValue)
{
    if (!isInt(minValue)) return false;
    return ((minValue >= 0) && (minValue <= 59));
}
function isValidEgn(egnValue)
{
    if (!isOnlyDigits(egnValue)) return false;
    return (egnValue.length == 10);
}

function SetClientTextAreaMaxLength(id, maxLength)
{
    var textArea = document.getElementById(id);

    textArea.setAttribute("maxLength", maxLength);
    textArea.setAttribute("onkeypress", "doKeypress(event, this);");
    textArea.setAttribute("onbeforepaste", "doBeforePaste(event, this);");
    textArea.setAttribute("onpaste", "doPaste(this);");
}

function isValidIdentityNumber(identityNumber, validCallback, invalidCallback)
{
    var url = "CommonAjaxRequests.aspx?AjaxMethod=IsValidIdentityNumber";
    var params;
    params += "&identityNumber=" + identityNumber;
    
    function response_handler(xml)
    {
        if (xmlNodeText(xml.childNodes[0]) == "OK")
        {
            validCallback();
        }
        else
        {
            invalidCallback();
        }
    }

    var myAJAX = new AJAX(url, false, params, response_handler);
    myAJAX.Call();
}

function GetSelectedItemId(ddlObject)
{
    var selectedItemId = "";
    if (ddlObject[0] == null)
        return selectedItemId;

    var cheked = false;
    var i = 0;
    do
    {
        if (ddlObject[i].selected)
        {
            cheked = true;
            selectedItemId = ddlObject[i].value;
        }
        i++;
    } while (!cheked)

    return selectedItemId;
}

function SetSelectedItemId(ddlObject, selectedItemId)
{
    var cheked = false;
    var i = 0;
    do
    {
        if (ddlObject[i].value == selectedItemId)
        {
            cheked = true;
            ddlObject[i].selected = true;
        }
        i++;

    } while (!cheked)
}

function AddToSelectList(OptionList, OptionValue, OptionText, setTextAsTitle)
{
    // Add option to the bottom of the list
    var option = new Option(OptionText, OptionValue);

    if (typeof setTextAsTitle != "undefined" && setTextAsTitle)
        option.title = OptionText;

    OptionList[OptionList.length] = option;
}

function ClearSelectList(OptionList, doNotAddEmpty)
{
    for (x = OptionList.length; x >= 0; x--)
    {
        OptionList[x] = null;
    }

    if (typeof doNotAddEmpty == "undefined" || !doNotAddEmpty)
        OptionList[OptionList.length] = new Option("", "-1");
}

function SetRadioGroupValueByName(name, setValue)
{
    var radioObjs = document.getElementsByName(name)
    for (var i = 0; i < radioObjs.length; i++)
    {
        if (radioObjs[i].value == setValue.toString())
            radioObjs[i].checked = true;
    }
}

function GetRadioGroupValueByName(name)
{
    var radioObjs = document.getElementsByName(name)
    for (var i = 0; i < radioObjs.length; i++)
    {
        if (radioObjs[i].checked)
            return radioObjs[i].value;
    }

    return "";
}

function RemoveListBoxSelection(dropDownId)
{
    var list = document.getElementById(dropDownId);

    while (list.options.selectedIndex >= 0)
    {
        list.remove(list.options.selectedIndex);
    }
}

function AddToDropDownIfNotExists(dropDownId, val, text)
{
    var found = false;
    var dropDown = document.getElementById(dropDownId);

    for (var j = 0; j < dropDown.options.length; j++)
    {
        if (dropDown.options[j].value == val)
        {
            found = true;
            break;
        }
    }

    if (!found)
    {
        var newOption = new Option();
        newOption.text = text;
        newOption.value = val;

        dropDown.options[dropDown.length] = newOption;
    }
}

function GetListBoxValues(dropDownId)
{
    var values = "";
    var dropDown = document.getElementById(dropDownId);

    for (var j = 0; j < dropDown.options.length; j++)
    {
        values += (values == "" ? "" : ",") + dropDown.options[j].value;
    }

    return values;
}

function CopyDropDown(sourceId, destId)
{
    var source = document.getElementById(sourceId);
    var dest = document.getElementById(destId);

    ClearSelectList(dest, true);

    for (var i = 0; i < source.options.length; i++)
    {
        var id = source.options[i].value;
        var name = source.options[i].text;

        AddToSelectList(dest, id, name);
    }

    dest.value = source.value;
}

function CenterLightBox(lightBoxId)
{
    var divWidth = document.getElementById(lightBoxId).offsetWidth;
    var divHeight = document.getElementById(lightBoxId).offsetHeight;
    var screenWidth = document.documentElement.clientWidth;
    var screenHeight = document.documentElement.clientHeight;

    var posLeft = (screenWidth - divWidth) / 2;
    var posTop = (screenHeight - divHeight) / 2;

    document.getElementById(lightBoxId).style.left = posLeft + "px";
    document.getElementById(lightBoxId).style.top = posTop + "px";
}

function GetListBoxSelectedValues(dropDownId)
{
    var values = "";
    var dropDown = document.getElementById(dropDownId);

    for (var j = 0; j < dropDown.options.length; j++)
    {
        if (dropDown.options[j].selected)
            values += (values == "" ? "" : ",") + dropDown.options[j].value;
    }

    return values;
}

function RemoveWhiteSpaces(s)
{
    return s.replace(/\s/g, "");
}

function TransferListBoxAllItems(srcDropDownId, destDropDownId)
{
    var srcDropDown = document.getElementById(srcDropDownId);
    var destDropDown = document.getElementById(destDropDownId);

    for (var j = srcDropDown.options.length - 1; j >= 0; j--)
    {
        var id = srcDropDown.options[j].value;
        var name = srcDropDown.options[j].text;

        srcDropDown.remove(j);
        
        AddToSelectList(destDropDown, id, name);
    }
}

function TransferListBoxSelectedItems(srcDropDownId, destDropDownId)
{
    var srcDropDown = document.getElementById(srcDropDownId);
    var destDropDown = document.getElementById(destDropDownId);

    for (var j = srcDropDown.options.length - 1; j >= 0; j--)
    {
        if (srcDropDown.options[j].selected)
        {
            var id = srcDropDown.options[j].value;
            var name = srcDropDown.options[j].text;

            srcDropDown.remove(j);

            AddToSelectList(destDropDown, id, name);
        }
    }
}

function JSRedirect(url)
{
    location.href = url;
}

function RemoveOptionFromDropDown(dropDownId, optionValue) {
    var selectObject = document.getElementById(dropDownId)
    for (var i = 0; i < selectObject.length; i++) {
        if (selectObject.options[i].value == optionValue)
            selectObject.remove(i);
    }
}

function DoesDropDownContainValue(dropDownId, optionValue) {
    var contains = false;
    var selectObject = document.getElementById(dropDownId)
    for (var i = 0; i < selectObject.length; i++) {
        if (selectObject.options[i].value == optionValue) {
            contains = true;
            break;
        }
    }
    return contains;
}

function GetElementsByClassNameCustom(className) {
  var nodes = []

  function crawl(node) {
    if (node.className) {
        var classes = node.className.split(" ");
        for (var i = 0; i < classes.length; i++) {
            if (classes[i].toLowerCase() == className.toLowerCase()) {
                nodes.push(node);
            }
        }
    }
    
    for (var i = 0; i < node.childNodes.length; i++) {
        crawl(node.childNodes[i]);
    }
  }
  crawl(document.body)
  
  return nodes
}

function FormatAge(age, ageMonthsPart) {
    var label = age + " години и " + ageMonthsPart;

    if (ageMonthsPart == 1)
        label += " месец";
    else
        label += " месеца";

    return label;
}