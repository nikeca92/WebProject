var ItemSelectorUtil = {}

ItemSelectorUtil.timer = null;
ItemSelectorUtil.ids = "";

ItemSelectorUtil.pload = function()
{
    ItemSelectorUtil.PopupWindow_attachListener();
}

ItemSelectorUtil.PopupWindow_attachListener = function()
{
    if (document.layers)
    {
        document.captureEvents(Event.MOUSEUP);
    }

    var popupWindowOldEventListener = document.onmouseup;

    if (popupWindowOldEventListener != null)
    {
        document.onmouseup = function() { popupWindowOldEventListener(); ItemSelectorUtil.PopupWindow_hidePopupWindows(); };
    }
    else
    {
        document.onmouseup = ItemSelectorUtil.PopupWindow_hidePopupWindows;
    }
}

window.addEventListener ? window.addEventListener("load", ItemSelectorUtil.pload, false) : window.attachEvent("onload", ItemSelectorUtil.pload);

ItemSelectorUtil.PopupWindow_hidePopupWindows = function()
{
    var ids = ItemSelectorUtil.ids.split(",");
    ItemSelectorUtil.ids = "";

    for (var i = 0; i < ids.length; i++)
    {
        var id = ids[i];

        if (id != "")
        {
            var pnl = document.getElementById(id);
            if (pnl)
            {
                if (ItemSelectorUtil["canhide_" + id])
                {
                    var cleanSelectorId = id.substring(3, id.indexOf("_divList"));
                    var selectorId = id.substring(0, id.indexOf("_divList"));

                    if (document.getElementById(selectorId + "_hdnFoundInListValue") &&
                        document.getElementById(selectorId + "_hdnFoundInListValue").value != "")
                    {
                        var value = document.getElementById(selectorId + "_hdnFoundInListValue").value;
                        var text = document.getElementById(selectorId + "_hdnFoundInListText").value;

                        ItemSelectorUtil.SetSelectedValue(cleanSelectorId, value);
                        ItemSelectorUtil.SetSelectedText(cleanSelectorId, text);
                    }
                    
                    ItemSelectorUtil.RaiseEvent(cleanSelectorId, "EndOfSelection");

                    pnl.style.display = "none";
                }
                else
                    ItemSelectorUtil.ids += (ItemSelectorUtil.ids == "" ? "" : ",") + id;
            }
        }
    }


}

ItemSelectorUtil.Focus = function(obj)
{
    obj.setAttribute("old_text", document.getElementById(obj.id).value);
    obj.setAttribute("old_value", document.getElementById("hdn" + obj.id.substring(3)).value);
}

ItemSelectorUtil.Blur = function(obj)
{
    if (document.getElementById("hdn" + obj.id.substring(3) + "OnlyListValues").value == "True")
    {
        var value = document.getElementById("hdn" + obj.id.substring(3)).value;
        if (!value || value == "" || value == "-1")
        {
            document.getElementById(obj.id).value = "";
            document.getElementById("text" + obj.id.substring(3)).value = "";
            document.getElementById("hdn" + obj.id.substring(3)).value = "-1";
        }
    }
    else
    {
        document.getElementById("text" + obj.id.substring(3)).value = document.getElementById("txt" + obj.id.substring(3)).value;
    }
    
    ItemSelectorUtil.RaiseEvent(obj.id.substring(3), "EndOfSelection");
}

ItemSelectorUtil.AdjustScrollbarUp = function(obj, iNode)
{
    var divList = document.getElementById(obj.id + "_divList");

    if (divList.scrollHeight > divList.clientHeight)
    {
        var adjustedTop = iNode * (divList.scrollHeight / document.getElementById("tblList" + obj.id).rows.length);
        if (adjustedTop < divList.scrollTop)
            divList.scrollTop = adjustedTop;
    }
}

ItemSelectorUtil.AdjustScrollbarDown = function(obj, iNode)
{
    var divList = document.getElementById(obj.id + "_divList");

    if (divList.scrollHeight > divList.clientHeight)
    {
        var adjustedTop = divList.scrollHeight / document.getElementById("tblList" + obj.id).rows.length;
        if ((iNode * adjustedTop) - divList.scrollTop >= divList.clientHeight)
            divList.scrollTop += adjustedTop;
    }
}

ItemSelectorUtil.KeyPressDown = function(obj, event)
{
    var code = event.keyCode ? event.keyCode : event.charCode;

    if (document.getElementById("tblList" + obj.id))
    {
        if (code == 9)
        {
            var iNode = obj.getAttribute("selectedIndex");
            if (iNode && iNode != "null")
            {
                iNode = parseInt(iNode);
                ItemSelectorUtil.DeselectItem(obj.id, iNode);
                ItemSelectorUtil.PickupItem(document.getElementById("hdnTblListValue" + obj.id + iNode).value, document.getElementById("hdnTblListText" + obj.id + iNode).value, obj.id);

                return false; //prevent submitting
            }
            else
            {
                ItemSelectorUtil.HideList(document.getElementById(obj.id), true);
                return true;
            }
        }
        else if (code == 13)
        {
            var iNode = obj.getAttribute("selectedIndex");
            if (iNode && iNode != "null")
            {
                iNode = parseInt(iNode);
                ItemSelectorUtil.DeselectItem(obj.id, iNode);
                ItemSelectorUtil.PickupItem(document.getElementById("hdnTblListValue" + obj.id + iNode).value, document.getElementById("hdnTblListText" + obj.id + iNode).value, obj.id);

                return false; //prevent submitting
            }
            else
            {
                return true;
            }
        }
        else if (code == 27)
        {
            ItemSelectorUtil.HideList(document.getElementById(obj.id), true);
            return true; //if true shows last value
        }
        else if (code == 38 || code == 40)
        {
            var iNode = obj.getAttribute("selectedIndex");
            if (iNode && iNode != "null")
            {

                iNode = parseInt(iNode);
                ItemSelectorUtil.DeselectItem(obj.id, iNode);
                if (code == 38 && iNode > 0)
                {
                    iNode -= 1;
                    ItemSelectorUtil.AdjustScrollbarUp(obj, iNode);
                }
                else if (code == 40 && iNode < document.getElementById("tblList" + obj.id).rows.length - 1)
                {
                    iNode += 1;
                    ItemSelectorUtil.AdjustScrollbarDown(obj, iNode);
                }
            }
            else
            {
                iNode = 0;
            }

            ItemSelectorUtil.SelectItem(obj.id, iNode);

            return false;
        }
        else
        {
            return true;
        }
    }
    else
    {
        return true;
    }
}

ItemSelectorUtil.KeyPressUp = function(obj, event)
{
    var code = event.keyCode ? event.keyCode : event.charCode;

    if (((!document.getElementById(obj.id + "_divList") || document.getElementById(obj.id + "_divList").style.display == "none") && (code != 9 && code != 13 && code != 27)) || (code != 9 && code != 13 && code != 38 && code != 40 && code != 27 && code != 37 && code != 39))
    {
        var prefix = document.getElementById(obj.id).value;

        if (ItemSelectorUtil.timer)
            clearTimeout(ItemSelectorUtil.timer);

        ItemSelectorUtil.timer = setTimeout("ItemSelectorUtil.DisplayList('" + obj.id + "', '" + prefix + "');", 500);
    }

    if (code != 9 && code != 13 && code != 38 && code != 40 && code != 37 && code != 39)
    {
        var old_text = obj.getAttribute("old_text");
        var old_value = obj.getAttribute("old_value");

        if (!old_text || old_text == "null")
        {
            old_text = "";
            old_value = "-1";
        }

        if (document.getElementById(obj.id).value == old_text)
        {
            document.getElementById("hdn" + obj.id.substring(3)).value = old_value;
        }
        else
        {
            document.getElementById("hdn" + obj.id.substring(3)).value = "-1";
        }
    }

    return true;
}

ItemSelectorUtil.DisplayList = function(obj_id, prefix) {
    var obj = document.getElementById(obj_id);
    //ItemSelectorUtil.HideList(obj, false);
    obj.setAttribute("selectedIndex", null);

    var divListCss = document.getElementById("hdn" + obj.id.substr(3) + "DivListCss").value;
    var divList = document.createElement("div");

    divList.id = obj.id + "_divList";

    ItemSelectorUtil.ids += (ItemSelectorUtil.ids == "" ? "" : ",") + divList.id;
    ItemSelectorUtil["canhide_" + divList.id] = true;

    divList.style.display = "none";

    if (divListCss != "") {
        divList.className = divListCss;
    }
    else {
        divList.style.textAlign = "left";
        divList.style.overflowY = "scroll";
        divList.style.overflowX = "visible";
        divList.style.fontFamily = "Verdana";
        divList.style.fontSize = "11px";
        divList.style.position = "absolute";
        divList.style.zIndex = "1000";
        divList.style.border = "solid 1px #AAAAAA";
        divList.style.backgroundColor = "#FFFFFF";
    }

    var webPage = document.getElementById("hdn" + obj.id.substr(3) + "DataSourceWebPage").value;
    var dataSourceKey = document.getElementById("hdn" + obj.id.substr(3) + "DataSourceKey").value;
    var resMaxCount = document.getElementById("hdn" + obj.id.substr(3) + "ResultMaxCount").value;
    var url = webPage + "?AjaxMethod=" + dataSourceKey;

    var eventScript = document.getElementById("hdn" + obj_id.substr(3) + "OnBeforeList").value;

    if (eventScript)
        eval(eventScript);

    var addParams = document.getElementById(obj_id.substr(3)).getAttribute("Parameters");

    if (!addParams || addParams == "null")
        addParams = "";

    var params = "PageCount=" + resMaxCount + "&PageIndex=1";

    if (prefix)
        params += "&Prefix=" + prefix;

    if (addParams != "")
        params += "&" + addParams;

    function response_handler(xml) {
        //This is moved here to prevent "flickering" while typing in the input box
        ItemSelectorUtil.HideList(obj, false);

        var root = xml.getElementsByTagName('result').item(0);

        if (root.childNodes.length > 0) {
            var html = "<table id='tblList" + obj_id + "' style='table-layout:fixed; width: 100%;'>";

            var foundInList = "";

            for (var iNode = 0; iNode < root.childNodes.length; iNode++) {
                if (iNode >= 10)
                    divList.style.height = "255px";
                else
                    divList.style.height = ((iNode * 25) + 40) + "px";

                var node = root.childNodes.item(iNode);

                var text = node.getElementsByTagName("text")[0].childNodes[0] != null ? node.getElementsByTagName("text")[0].childNodes[0].nodeValue : "";
                var text_pickupitem = text;
                //2014-12-10:
                //By default the "pickup item" text is the same as the original "text". We added the ability to have different text when "searching" and when "selecting" an element.
                //So, it is possible if the "pickup item" text is not presented in some data sources and that is why we set its default value to be the "text" field.
                if (node.getElementsByTagName("text_pickupitem").length > 0)
                    text_pickupitem = node.getElementsByTagName("text_pickupitem")[0].childNodes[0] != null ? node.getElementsByTagName("text_pickupitem")[0].childNodes[0].nodeValue : "";

                var JStext = text.replace(/\"/g, "\\&quot;");
                var JStext = JStext.replace(/'/g, "&#039;");

                var JStext_pickupitem = text_pickupitem.replace(/\"/g, "\\&quot;");
                var JStext_pickupitem = JStext_pickupitem.replace(/'/g, "&#039;");

                var HTMLtext = text.replace(/\"/g, "&quot;");
                var HTMLtext = HTMLtext.replace(/'/g, "&#039;");

                var HTMLtext_pickupitem = text_pickupitem.replace(/\"/g, "&quot;");
                var HTMLtext_pickupitem = HTMLtext_pickupitem.replace(/'/g, "&#039;");

                if (prefix && prefix != "" && prefix.toUpperCase() == text.toUpperCase()) {
                    foundInList = "<input type='hidden' id='" + obj.id + "_hdnFoundInListValue' value='" + node.getElementsByTagName("value")[0].childNodes[0].nodeValue + "' />" +
                                  "<input type='hidden' id='" + obj.id + "_hdnFoundInListText' value='" + text + "' />";
                }

                html += "<tr style='width: 100%;' onclick='ItemSelectorUtil.PickupItem(\"" + node.getElementsByTagName("value")[0].childNodes[0].nodeValue + "\", \"" + JStext_pickupitem + "\", \"" + obj.id + "\");'  onmouseover='ItemSelectorUtil.SelectItem(\"" + obj.id + "\",\"" + iNode + "\");' onmouseout='ItemSelectorUtil.DeselectItem(\"" + obj.id + "\",\"" + iNode + "\");'><td nowrap='nowrap'><span>" + HTMLtext + "</span><input type='hidden' id='hdnTblListValue" + obj.id + iNode + "' value='" + node.getElementsByTagName("value")[0].childNodes[0].nodeValue + "' /><input type='hidden' id='hdnTblListText" + obj.id + iNode + "' value='" + HTMLtext_pickupitem + "' /></td></tr>";
            }

            html += "</table>" + foundInList;

            divList.innerHTML = html;

            function list_MouseOver() {
                ItemSelectorUtil["canhide_" + divList.id] = false;
            }

            function list_MouseOut() {
                ItemSelectorUtil["canhide_" + divList.id] = true;
            }

            if (divList.addEventListener) {
                divList.addEventListener("mouseover", list_MouseOver, false);
                divList.addEventListener("mouseout", list_MouseOut, false);
            } else if (divList.attachEvent) {
                divList.attachEvent("onmouseover", list_MouseOver);
                divList.attachEvent("onmouseout", list_MouseOut);
            } else {
                divList.onmouseover = list_MouseOver;
                divList.onmouseout = list_MouseOut;
            }

            divList.style.display = "";

            obj.parentNode.appendChild(divList);

            ItemSelectorUtil.SelectItem(obj_id, 0);
        }
    }

    var myAJAX = new AJAX(url, true, params, response_handler);
    myAJAX.Call();
}

ItemSelectorUtil.HideList = function(obj, setIfFound)
{
    if (document.getElementById(obj.id + "_divList"))
    {
        if (typeof setIfFound != "undefined" && setIfFound &&
            document.getElementById(obj.id + "_hdnFoundInListValue") &&
            document.getElementById(obj.id + "_hdnFoundInListValue").value != "")
        {
            var value = document.getElementById(obj.id + "_hdnFoundInListValue").value;
            var text = document.getElementById(obj.id + "_hdnFoundInListText").value;

            ItemSelectorUtil.SetSelectedValue(obj.id, value);
            ItemSelectorUtil.SetSelectedText(obj.id, text);
        }

        var divList = document.getElementById(obj.id + "_divList");
        obj.parentNode.removeChild(divList);

        ItemSelectorUtil.RaiseEvent(obj.id.substring(3), "EndOfSelection");
    }
}

ItemSelectorUtil.SelectItem = function(obj_id, iNode)
{
    var obj = document.getElementById(obj_id);

    var selectedIndex = obj.getAttribute("selectedIndex");
    if (selectedIndex && selectedIndex != "null")
    {
        selectedIndex = parseInt(selectedIndex);
        ItemSelectorUtil.DeselectItem(obj.id, selectedIndex);
    }

    obj.setAttribute("selectedIndex", iNode);

    var tblList = document.getElementById("tblList" + obj_id);
    if (tblList)
    {
        var row = tblList.rows[iNode];

        row.style.cursor = "pointer";
        row.style.backgroundColor = "#333399";
        row.style.color = "#FFFFFF";
    }
}

ItemSelectorUtil.DeselectItem = function(obj_id, iNode)
{
    var tblList = document.getElementById("tblList" + obj_id);
    if (tblList)
    {
        var row = tblList.rows[iNode];

        if (row)
        {
            row.style.cursor = "";
            row.style.backgroundColor = "";
            row.style.color = "";
        }
    }
}

ItemSelectorUtil.PickupItem = function(val, text, objId)
{
    document.getElementById(objId).value = text;
    document.getElementById("text" + objId.substring(3)).value = text;
    document.getElementById("hdn" + objId.substring(3)).value = val;
    document.getElementById(objId).setAttribute("old_text", text);
    document.getElementById(objId).setAttribute("old_value", val);
    //ItemSelectorUtil.PopupWindow_hidePopupWindows();
    ItemSelectorUtil.HideFullList(objId);
    ItemSelectorUtil.HideList(document.getElementById(objId), false);
    
    ItemSelectorUtil.RaiseEvent(objId.substring(3), "EndOfSelection");
}

ItemSelectorUtil.DisplayFullList = function(obj_id, page) {
    ItemSelectorUtil.RaiseEvent(obj_id.substr(3), "BeforeFullList");

    var obj = document.getElementById(obj_id);
    var imgObj = document.getElementById("img" + obj_id.substr(3));

    var webPage = document.getElementById("hdn" + obj.id.substr(3) + "DataSourceWebPage").value;
    var dataSourceKey = document.getElementById("hdn" + obj.id.substr(3) + "DataSourceKey").value;
    var pageCount = document.getElementById("hdn" + obj.id.substr(3) + "PageCount").value;
    var title = document.getElementById("hdn" + obj.id.substr(3) + "DivFullListTitle").value;

    var divList;

    if (document.getElementById(obj.id + "_divFullList")) {
        divList = document.getElementById(obj.id + "_divFullList");
    }
    else {
        divList = document.createElement("div");
        var divFullListCss = document.getElementById("hdn" + obj.id.substr(3) + "DivFullListCss").value;

        divList.id = obj.id + "_divFullList";

        divList.style.display = "none";

        if (divFullListCss != "") {
            divList.className = divFullListCss;
        }
        else {
            divList.style.textAlign = "left";
            divList.style.fontFamily = "Verdana";
            divList.style.fontSize = "11px";
            divList.style.position = "absolute";
            divList.style.zIndex = "1000";
            divList.style.border = "solid 1px #AAAAAA";
            divList.style.backgroundColor = "#FFFFFF";
        }

        ItemSelectorUtil.ShowHidePage(obj);
    }

    //This doesn't work becasue the detachEvent function cannot detach a function without a reference
    //document.body.addEventListener ? document.body.addEventListener("onkeydown", Function("return ItemSelectorUtil.DivFullListKeyPress('" + obj_id + "', event);"), false) : document.body.attachEvent("onkeydown", Function("return ItemSelectorUtil.DivFullListKeyPress('" + obj_id + "', event);"));

    //This is why we keep a reference to the function in a global variable
    ItemSelector_funcDivFullListKeyPress = function() {
        return ItemSelectorUtil.DivFullListKeyPress(obj_id, event);
    }

    document.body.addEventListener ? document.body.addEventListener("onkeydown", ItemSelector_funcDivFullListKeyPress, false) : document.body.attachEvent("onkeydown", ItemSelector_funcDivFullListKeyPress);

    var html = "<table id='tblFullList' style='width: 100%''>";

    html += "<tr><td class='Title' style='width: 100%;'><span>" + title + "</span><div class='CloseButton' style='float: right; cursor: pointer;' onclick='ItemSelectorUtil.HideFullList(\"" + obj_id + "\")' ></div></td></tr>";
    html += "<tr><td>&nbsp;</td></tr>";
    html += "<tr><td style='width: 100%;'><center><table style='" + (maxPage > 0 ? "border: solid 1px #AAAAAA;" : "") + " border-collapse: collapse; width: 80%; text-align: center;'>";

    var eventScript = document.getElementById("hdn" + obj_id.substr(3) + "OnBeforeList").value;

    if (eventScript)
        eval(eventScript);

    var addParams = document.getElementById(obj_id.substr(3)).getAttribute("Parameters");

    if (!addParams || addParams == "null")
        addParams = "";

    var url = webPage + "?AjaxMethod=" + dataSourceKey;

    var params = "PageCount=" + pageCount + "&PageIndex=" + page;

    if (addParams != "")
        params += "&" + addParams;

    var maxPage;

    function response_handler(xml) {
        var count = xml.getElementsByTagName('count')[0].childNodes[0].nodeValue;
        var root = xml.getElementsByTagName('result').item(0);

        maxPage = Math.floor(count / pageCount);

        if (count % pageCount != 0)
            maxPage += 1;

        if (page < 1 || (page > maxPage && maxPage > 0)) {
            ItemSelectorUtil["ajaxcall_" + obj.id] = false;
            return;
        }

        for (var iNode = 0; iNode < root.childNodes.length; iNode++) {
            var node = root.childNodes.item(iNode);

            var text = node.getElementsByTagName("text")[0].childNodes[0] != null ? node.getElementsByTagName("text")[0].childNodes[0].nodeValue : "";
            var text_pickupitem = text;
            //2014-12-10:
            //By default the "pickup item" text is the same as the original "text". We added the ability to have different text when "searching" and when "selecting" an element.
            //So, it is possible if the "pickup item" text is not presented in some data sources and that is why we set its default value to be the "text" field.
            if (node.getElementsByTagName("text_pickupitem").length > 0)
                text_pickupitem = node.getElementsByTagName("text_pickupitem")[0].childNodes[0] != null ? node.getElementsByTagName("text_pickupitem")[0].childNodes[0].nodeValue : "";

            var textHTML = text;

            text_pickupitem = text_pickupitem.replace(/\"/g, "\\&quot;");
            text_pickupitem = text_pickupitem.replace(/'/g, "&#039;");

            html += "<tr class='ListItem' onclick='ItemSelectorUtil.PickupItem(\"" + node.getElementsByTagName("value")[0].childNodes[0].nodeValue + "\", \"" + text_pickupitem + "\", \"" + obj.id + "\");'><td style='border: solid 1px #AAAAAA;width: 20px;'>" + ((page - 1) * pageCount + iNode + 1) + "</td>";
            html += "<td style='border: solid 1px #AAAAAA; text-align: left;'>" + textHTML + "</td>";
            html += "</tr>";
        }

        //No data found
        if (!maxPage || maxPage == 0) {
            html += "<tr>";
            html += "   <td>Списъкът е празен</td>";
            html += "</tr>";
        }

        var firstClass, prevClass, nextClass, lastClass;
        var firstClick, prevClick, nextClick, lastClick;

        if (page == 1) {
            firstClass = "FirstButtonDisabled";
            prevClass = "PrevButtonDisabled";
            firstClick = "";
            prevClick = "";
        }
        else {
            firstClass = "FirstButton";
            prevClass = "PrevButton";
            firstClick = "onclick=\"ItemSelectorUtil.DisplayFullList('" + obj_id + "'," + 1 + ");\"";
            prevClick = "onclick=\"ItemSelectorUtil.DisplayFullList('" + obj_id + "'," + (page - 1) + ");\"";
        }

        if (page == maxPage) {
            nextClass = "NextButtonDisabled";
            lastClass = "LastButtonDisabled";
            nextClick = "";
            lastClick = "";
        }
        else {
            nextClass = "NextButton";
            lastClass = "LastButton";
            nextClick = "onclick=\"ItemSelectorUtil.DisplayFullList('" + obj_id + "'," + (page + 1) + ");\"";
            lastClick = "onclick=\"ItemSelectorUtil.DisplayFullList('" + obj_id + "'," + maxPage + ");\"";
        }

        html += "</table></center></td></tr>";
        html += "<tr><td>&nbsp;</td></tr>";

        if (maxPage > 0) {
            html += "<tr><td style='text-align: right;'>";
            html += "<div style='cursor: pointer; vertical-align: middle; display: inline-block;' class='" + firstClass + "' alt='Първа страница' " + firstClick + "></div>";
            html += "<div style='cursor: pointer; vertical-align: middle; display: inline-block;' class='" + prevClass + "'  alt='Предишна страница' " + prevClick + "></div>&nbsp;";
            html += "<input type='text' id='txtDivFullListPage" + obj_id + "' value='" + page + "' style='width: 20px;' onblur='ItemSelectorUtil.ChangePage(\"" + obj_id + "\");'/> <span> от " + maxPage + "</span>&nbsp;";
            html += "<div style='cursor: pointer; vertical-align: middle; display: inline-block;' class='" + nextClass + "'  alt='Следваща страница' " + nextClick + "></div>";
            html += "<div style='cursor: pointer; vertical-align: middle; display: inline-block;' class='" + lastClass + "'  alt='Последна страница' " + lastClick + "></div>";
            html += "</td></tr>";
        }

        html += "</table>";

        divList.innerHTML = html;

        divList.style.display = "";

        divList.style.overflowY = "scroll";
        divList.style.overflowX = "scroll";

        divList.style.height = "1px";

        obj.parentNode.appendChild(divList);

        var height = divList.scrollHeight;
        var width = divList.scrollWidth

        divList.style.overflowY = "visible";
        divList.style.overflowX = "visible";

        divList.style.height = height + "px";

        var winW = 630, winH = 460;

        if (parseInt(navigator.appVersion) > 3) {
            if (navigator.appName == "Netscape") {
                winW = window.innerWidth;
                winH = window.innerHeight;
            }
            if (navigator.appName.indexOf("Microsoft") != -1) {
                winW = document.documentElement.clientWidth;
                winH = document.documentElement.clientHeight;
            }
        }

        divList.style.top = ((winH - height) / 2) + "px";
        divList.style.left = ((winW - width) / 2) + "px";

        ItemSelectorUtil["ajaxcall_" + obj.id] = false;
    }

    if (!ItemSelectorUtil["ajaxcall_" + obj.id]) {
        ItemSelectorUtil["ajaxcall_" + obj.id] = true;

        var myAJAX = new AJAX(url, true, params, response_handler);
        myAJAX.Call();
    }
}

ItemSelectorUtil.HideFullList = function(obj_id)
{
    var obj = document.getElementById(obj_id);
    if (document.getElementById(obj.id + "_divFullList"))
    {
        var divList = document.getElementById(obj.id + "_divFullList");
        obj.parentNode.removeChild(divList);

        //This doesn't work because the detashEvent function can detach an already attached function by its reference
        //document.body.removeEventListener ? document.body.removeEventListener("onkeydown", Function("return ItemSelectorUtil.DivFullListKeyPress('" + obj_id + "', event);"), false) : document.body.detachEvent("onkeydown", Function("return ItemSelectorUtil.DivFullListKeyPress('" + obj_id + "', event);"));

        //This is why we detach a function that is already attached AND we keep its reference in a global variable
        document.body.removeEventListener ? document.body.removeEventListener("onkeydown", ItemSelector_funcDivFullListKeyPress, false) : document.body.detachEvent("onkeydown", ItemSelector_funcDivFullListKeyPress);
        
        ItemSelectorUtil.RemoveHidePage(obj);
    }
}

ItemSelectorUtil.ShowHidePage = function(obj)
{
    var divHidePage = document.createElement("div");
    divHidePage.id = obj.id + "_HidePage";

    divHidePage.style.zIndex = "1000";
    divHidePage.style.position = "fixed";
    divHidePage.style.top = "0";
    divHidePage.style.left = "0";
    divHidePage.style.width = "100%";
    divHidePage.style.height = "100%";
    divHidePage.style.backgroundColor = "#000000";
    divHidePage.style.filter = "alpha(opacity=60)";
    divHidePage.style.opacity = "0.6";
    //divHidePage.style.cursor = "not-allowed";
    divHidePage.style.display = "";

    obj.parentNode.appendChild(divHidePage);
}

ItemSelectorUtil.RemoveHidePage = function(obj)
{
    if (document.getElementById(obj.id + "_HidePage"))
    {
        var divHidePage = document.getElementById(obj.id + "_HidePage");
        obj.parentNode.removeChild(divHidePage);
    }
}

ItemSelectorUtil.DivFullListKeyPress = function(obj_id, event)
{
    if (document.getElementById(obj_id))
    {
        var code = event.keyCode ? event.keyCode : event.charCode;

        if (code == 27)
        {
            ItemSelectorUtil.HideFullList(obj_id);
            return true;
        }
        else if (code == 9)
        {
            return false;
        }
        else if (code == 13)
        {
            ItemSelectorUtil.ChangePage(obj_id);
            return false;
        }
    }

    return true;
}

ItemSelectorUtil.ChangePage = function(obj_id)
{
    if (document.getElementById("txtDivFullListPage" + obj_id))
    {
        var page = document.getElementById("txtDivFullListPage" + obj_id).value;
        if (parseInt(page) == Number(page))
            ItemSelectorUtil.DisplayFullList(obj_id, parseInt(page))
    }
}

ItemSelectorUtil.SetProperty = function(obj_id, property_name, value)
{
    var property = document.getElementById("hdn" + obj_id + property_name);

    if (property)
        property.value = value;
}

ItemSelectorUtil.GetProperty = function(obj_id, property_name)
{
    var property = document.getElementById("hdn" + obj_id + property_name);
    if (property)
        return property.value;
    else
        return null;
}

ItemSelectorUtil.SetSelectedText = function(obj_id, value)
{
    var selectedText = document.getElementById("txt" + obj_id);
    var selectedHdnText = document.getElementById("text" + obj_id);

    if (selectedText)
        selectedText.value = value;

    if (selectedHdnText)
        selectedHdnText.value = value;
}

ItemSelectorUtil.GetSelectedText = function(obj_id)
{
    var selectedText = document.getElementById("text" + obj_id);
    if (selectedText)
        return selectedText.value;
    else
        return null;
}

ItemSelectorUtil.SetSelectedValue = function(obj_id, value)
{
    var selectedValue = document.getElementById("hdn" + obj_id);

    if (selectedValue)
        selectedValue.value = value;
}

ItemSelectorUtil.GetSelectedValue = function(obj_id)
{
    var selectedValue = document.getElementById("hdn" + obj_id);
    if (selectedValue)
        return selectedValue.value;
    else
        return null;
}

ItemSelectorUtil.IsDisabled = function(obj_id)
{
    var isDisabled = false;

    var textbox = document.getElementById("txt" + obj_id);
    var dropdown = document.getElementById("dd" + obj_id);

    if (textbox && textbox.disabled)
        isDisabled = true;
    else if (dropdown && dropdown.disabled)
        isDisabled = true;

    return isDisabled;
}

ItemSelectorUtil.IsHidden = function(obj_id)
{
    var isHidden = false;

    var textbox = document.getElementById("txt" + obj_id);
    var dropdown = document.getElementById("dd" + obj_id);

    if (textbox && textbox.style.display == "none")
        isHidden = true;
    else if (dropdown && dropdown.style.display == "none")
        isHidden = true;
    else if (!textbox && !dropdown)
        isHidden = true;

    return isHidden;
}

ItemSelectorUtil.RaiseEvent = function(obj_id, eventName)
{
    var eventScript = document.getElementById("hdn" + obj_id + "On" + eventName).value;

    if (eventScript)
        eval(eventScript);
}