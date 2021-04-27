// It's used to keep alive the callback function
var gTableRefreshCallback;

// Generate GTable items grid and show the light box after little bit time out
function ShowGTable(tableName, orderBy, pageIdx, refreshCallback)
{
    gTableRefreshCallback = refreshCallback;
    GetGTableItems(tableName, orderBy, pageIdx);
}

// Shows the light box with GTable items and "disable" rest of the page
function ShowGTableLightBox()
{
    document.getElementById("HidePage").style.display = "";
    document.getElementById("divGTableLightBox").style.display = "";
    CenterLightBox("divGTableLightBox");
}

// Retrieve a variable by name. This variable contents the specific button identifier ordering to refresh the GTable dropdown list
function getVal(n) 
{
  return eval(n);
}

// Close the light box and clear its content
function HideGTableLightBox()
{
    document.getElementById("HidePage").style.display = "none";
    document.getElementById("divGTableLightBox").style.display = "none";
    document.getElementById("divGTableLightBoxContent").innerHTML = "";
    
    if(typeof gTableRefreshCallback == "function")
    {
        // Fire specific callback function to refresh the GTable items in client UI
        gTableRefreshCallback();
    }
    
    gTableRefreshCallback = "";
}

// Call web server and retrieve the generated GTable items grid from it
function GetGTableItems(tableName, orderBy, pageIdx)
{
    var url = "GTableMaintanceHandlers.aspx?AjaxMethod=JSGetGTableItems";
    var params = "";
    params += "GTableName=" + tableName;
    params += "&GTableOrderBy=" + orderBy;
    params += "&GTablePageIdx=" + pageIdx;
    
    function response_handler(xml)
    {
        if(xmlValue(xml, "response") != "")
        {
            document.getElementById('divGTableLightBoxContent').innerHTML = xmlNodeText(xml.childNodes[0]);
            ShowGTableLightBox();
        }
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}

// Set the contextual row in edit mode of the grid
function EditGTableItem(ctrl, tableName, tableKey, rowIndex) {

    if (ctrl.getAttribute("data-disabled") && ctrl.getAttribute("data-disabled") == "true")
        return;
        
    CancelNewGTableItem();
    var rows = document.getElementById(tableName).getElementsByTagName('tbody')[0].getElementsByTagName('tr');
    var curInputSeq = "InputItemSeq_" + tableKey;
    var curSpanSeq = "InputSpanSeq_" + tableKey;
    var curInputValue = "InputItemValue_" + tableKey;
    var curSpanValue = "InputSpanValue_" + tableKey;
    
    for (i = 0; i < rows.length; i++) 
    {
        if (rows[i].rowIndex == rowIndex)
        {
            var cells = rows[i].getElementsByTagName('td');
            
            for (j = 0; j < cells.length; j++) 
            {
                var elms = cells[j].getElementsByTagName("*");
                for(var e = 0; e < elms.length; e++) {
                    var elm = elms[e];
                    switch(elm.tagName) 
                    {
                        case "INPUT":
                        {
                            if (elm.type === "text")
                            {
                                if (elm.id == curInputSeq || elm.id == curInputValue)
                                {
                                    elm.style.display = "";
                                    elm.value = cells[j].getElementsByTagName("SPAN")[0].innerHTML;
                                }
                            }
                            break;
                        }
                        case "SPAN":
                            if (elm.id == curSpanSeq || elm.id == curSpanValue)
                                elm.style.display = "none";
                            break; 
                        case "IMG":
                            {
                                switch (elm.getAttribute("mode"))
                                {
                                    case "edit":
                                    case "delete":
                                        elm.style.display = "none";
                                        break;
                                    case "save":
                                    case "cancel":
                                        elm.style.display = "";
                                        break;
                                }
                            }
                            break;
                    }
                }   
            }
        }
        else
        {
            var cells = rows[i].getElementsByTagName('td');
            
            for (j = 0; j < cells.length; j++) 
            {
                var elms = cells[j].getElementsByTagName("*");
                for(var e = 0; e < elms.length; e++) {
                    var elm = elms[e];
                    switch(elm.tagName) 
                    {
                        case "INPUT":
                        {
                            if (elm.type === "text")
                            {
                                if (elm.id != curInputSeq && elm.id != curInputValue)
                                    elm.style.display = "none";
                            }
                            break;
                        }
                        case "SPAN":
                            if (elm.id != curSpanSeq && elm.id != curSpanValue)
                                elm.style.display = "";
                            break; 
                        case "IMG":
                            {
                                switch (elm.getAttribute("mode"))
                                {
                                    case "edit":
                                    case "delete":
                                        elm.setAttribute("data-disabled", "true");
                                        break;
                                    case "save":
                                    case "cancel":
                                        elm.style.display = "none";
                                        break;
                                }
                            }
                            break;
                    }
                }   
            }
        }
    }
}

// Cancel edit mode of the contextual row of the grid
function CancelGTableItem(tableName)
{
    var rows = document.getElementById(tableName).getElementsByTagName('tbody')[0].getElementsByTagName('tr');
    
    for (i = 0; i < rows.length; i++) 
    {
        var cells = rows[i].getElementsByTagName('td');
        
        for (j = 0; j < cells.length; j++) 
        {
            var elms = cells[j].getElementsByTagName("*");
            for(var e = 0; e < elms.length; e++) {
                var elm = elms[e];
                switch(elm.tagName) 
                {
                    case "INPUT":
                    {
                        if (elm.type === "text")
                        {
                            elm.style.display = "none";
                        }
                        break;
                    }
                    case "SPAN":
                        elm.style.display = "";
                        break; 
                    case "IMG":
                        {
                            switch (elm.getAttribute("mode"))
                            {
                                case "edit":
                                case "delete":
                                    {
                                        elm.style.display = "";
                                        elm.setAttribute("data-disabled", "true");
                                    }
                                    break;
                                case "save":
                                case "cancel":
                                    elm.style.display = "none";
                                    break;
                            }
                        }
                        break;
                }
            }   
        }
    }   
}

// Call web server and retrieve the generated GTable items grid from it
function SaveGTableItem(tableName, tableKey, rowIndex)
{
    var rows = document.getElementById(tableName).getElementsByTagName('tbody')[0].getElementsByTagName('tr');
    var curInputSeq = "InputItemSeq_" + tableKey;
    var curInputValue = "InputItemValue_" + tableKey;
    
    var inputSeq = 0;
    var inputValue = 0;

    var gTableMessage = document.getElementById('gTableMessage');
    gTableMessage.innerHTML = "";
    
    var isValid = true;

    for (i = 0; i < rows.length; i++) 
    {
        if (rows[i].rowIndex == rowIndex)
        {
            var cells = rows[i].getElementsByTagName('td');
            
            for (j = 0; j < cells.length; j++) 
            {
                var elms = cells[j].getElementsByTagName("*");
                for(var e = 0; e < elms.length; e++) {
                    var elm = elms[e];
                    switch(elm.tagName) 
                    {
                        case "INPUT":
                        {
                            if (elm.type === "text")
                            {
                                if (elm.id == curInputSeq)
                                {
                                    if (TrimString(elm.value) == "")
                                    {
                                        gTableMessage.innerHTML = "Номерът за подредба е задължителен<br />";
                                        gTableMessage.className = "ErrorText";
                                        gTableMessage.style.display = "";    
                                    }
                                    else
                                    {
                                        if (!isInt(TrimString(elm.value)))
                                            {
                                                isValid = false;

                                                gTableMessage.innerHTML = "Посоченият № за подредба е невалиден<br />";
                                                gTableMessage.className = "ErrorText";
                                                gTableMessage.style.display = "";
                                            }
                                            else
                                                inputSeq = elm.value;
                                    }
                                }
                                else if (elm.id == curInputValue)
                                {
                                    if (TrimString(elm.value) == "")
                                    {
                                        isValid = false;

                                        gTableMessage.innerHTML += "Текстът за класификатора е задължителен<br />";
                                        gTableMessage.className = "ErrorText";
                                        gTableMessage.style.display = "";
                                    }
                                    else
                                        inputValue = elm.value;
                                }
                            }
                            break;
                        }
                    }
                }   
            }
        }
    }

    if (inputSeq != 0 && inputValue != 0 && isValid)
    {
        var url = "GTableMaintanceHandlers.aspx?AjaxMethod=JSSaveGTableItem";
        var params = "";
        params += "GTableName=" + tableName;
        params += "&GTableKey=" + tableKey;
        params += "&GTableSeq=" + inputSeq;
        params += "&GTableValue=" + inputValue;
        
        function response_handler(xml)
        {
            if(xmlValue(xml, "response") != "")
                document.getElementById('divGTableLightBoxContent').innerHTML = xmlNodeText(xml.childNodes[0]);
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}


function DeleteGTableItem(ctrl, tableName, tableKey, tableValue) {

    if (ctrl.getAttribute("data-disabled") && ctrl.getAttribute("data-disabled") == "true")
        return;
        
    YesNoDialog('Желаете ли да изтриете записа "' + tableValue + '"?', ConfirmYes, null);
    
    function ConfirmYes()
    {
        var pageIdx = document.getElementById('hdnGTablePageIdx').value;
        var url = "GTableMaintanceHandlers.aspx?AjaxMethod=JSDeleteGTableItem";
        var params = "";
        params += "GTableName=" + tableName;
        params += "&GTableKey=" + tableKey;
        params += "&GTablePageIdx=" + pageIdx;
        
        function response_handler(xml)
        {
            if(xmlValue(xml, "response") != "")
                document.getElementById('divGTableLightBoxContent').innerHTML = xmlNodeText(xml.childNodes[0]);
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

function AddNewGTableItem(tableName)
{
    CancelGTableItem(tableName);
    document.getElementById('trNewGTableItemImg').style.display = "none";
    document.getElementById('trNewGTableItemLabel').style.display = "";
    document.getElementById('trNewGTableItemFields').style.display = "";
}

function SaveNewGTableItem(tableName)
{
    var newGTableItemSeq = document.getElementById('NewGTableItemSeq').value;
    var newGTableItemValue = document.getElementById('NewGTableItemValue').value;

    var inputSeq = 0;
    var inputValue = 0;
    
    var gTableMessage = document.getElementById('gTableMessage');
    gTableMessage.innerHTML = "";
    
    var isValid = true;
    
    if (TrimString(newGTableItemSeq) == "")
    {
        gTableMessage.innerHTML = "Номерът за подредба е задължителен<br />";
        gTableMessage.className = "ErrorText";
        gTableMessage.style.display = "";    
    }
    else
    {
        if (!isInt(TrimString(newGTableItemSeq)))
            {
                isValid = false;

                gTableMessage.innerHTML = "Посоченият № за подредба е невалиден<br />";
                gTableMessage.className = "ErrorText";
                gTableMessage.style.display = "";
            }
            else
                inputSeq = newGTableItemSeq;
    }
        
    if (TrimString(newGTableItemValue) == "")
    {
        isValid = false;

        gTableMessage.innerHTML += "Текстът за класификатора е задължителен<br />";
        gTableMessage.className = "ErrorText";
        gTableMessage.style.display = "";
    }
    else
        inputValue = newGTableItemValue;

    if (inputSeq != 0 && inputValue != 0 && isValid)
    {
        var pageIdx = document.getElementById('hdnGTablePageIdx').value;
    
        var url = "GTableMaintanceHandlers.aspx?AjaxMethod=JSSaveGTableItem";
        var params = "";
        params += "GTableName=" + tableName;
        params += "&GTableKey=0";
        params += "&GTableSeq=" + inputSeq;
        params += "&GTableValue=" + inputValue;
        params += "&GTableOrderBy=1";
        params += "&GTablePageIdx=" + pageIdx;
        
        function response_handler(xml)
        {
            if(xmlValue(xml, "response") != "")
                document.getElementById('divGTableLightBoxContent').innerHTML = xmlNodeText(xml.childNodes[0]);
        }

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

function CancelNewGTableItem()
{
    document.getElementById('trNewGTableItemImg').style.display = "";
    document.getElementById('trNewGTableItemLabel').style.display = "none";
    document.getElementById('trNewGTableItemFields').style.display = "none";
    document.getElementById('NewGTableItemSeq').value = "";
    document.getElementById('NewGTableItemValue').value = "";
    document.getElementById('gTableMessage').innerHTML = "";
}

function SortGTableBy(tableName, sort)
{
    var pageIdx = document.getElementById('hdnGTablePageIdx').value;
    var orderBy = document.getElementById('hdnGTableOrderBy').value;
    if (orderBy == sort)
    {
        sort = sort + 100;
    }
    
    orderBy = sort;
    
    GetGTableItems(tableName, orderBy, pageIdx);
}

//Go to the first page and refresh the grid
function BtnGTableFirstClick(tableName)
{
    var orderBy = document.getElementById('hdnGTableOrderBy').value;
    var pageIdx = 1;
    var maxPage = document.getElementById('hdnGTableMaxPage').value;
    
    GetGTableItems(tableName, orderBy, pageIdx);
}

//Go to the previous page and refresh the grid
function BtnGTablePrevClick(tableName)
{
    var orderBy = document.getElementById('hdnGTableOrderBy').value;
    var pageIdx = document.getElementById('hdnGTablePageIdx').value;

    if (pageIdx > 1)
    {
        pageIdx--;
        GetGTableItems(tableName, orderBy, pageIdx);
    }
}

//Go to the next page and refresh the grid
function BtnGTableNextClick(tableName)
{
    var orderBy = document.getElementById('hdnGTableOrderBy').value;
    var pageIdx = document.getElementById('hdnGTablePageIdx').value;
    var maxPage = document.getElementById('hdnGTableMaxPage').value;

    if (pageIdx < maxPage)
    {
        pageIdx++;
        GetGTableItems(tableName, orderBy, pageIdx);
    }
}

//Go to the last page and refresh the grid
function BtnGTableLastClick(tableName)
{
    var orderBy = document.getElementById('hdnGTableOrderBy').value;
    var pageIdx;
    var maxPage = document.getElementById('hdnGTableMaxPage').value;

    pageIdx = maxPage;
    GetGTableItems(tableName, orderBy, pageIdx);
}

//Go to a specific page (entered by the user) and refresh the grid
function BtnGTableGotoClick(tableName)
{
    var orderBy = document.getElementById('hdnGTableOrderBy').value;
    var pageIdx;
    var maxPage = document.getElementById('hdnGTableMaxPage').value;
    var goToPage = document.getElementById('txtGTableGotoPage').value;

    if (isInt(TrimString(goToPage)) && goToPage > 0 && goToPage <= maxPage)
    {
        pageIdx = goToPage;
        GetGTableItems(tableName, orderBy, pageIdx);
    }
}