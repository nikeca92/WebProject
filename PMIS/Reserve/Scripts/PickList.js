var PickListUtil = {};

PickListUtil.defaultPickListConfig = {width: 200, allLabel: "<Всички>"};
PickListUtil.idPickLists = "";

PickListUtil.pload = function() 
{        
   PickListUtil.PopupWindow_attachListener();
}

PickListUtil.PopupWindow_attachListener = function() 
{
    if (document.layers) 
    {
	    document.captureEvents(Event.MOUSEUP);
	}
	
    PickListUtil.popupWindowOldEventListener = document.onmouseup;
    
    if (PickListUtil.popupWindowOldEventListener != null) 
    {
	    document.onmouseup = new Function("PickListUtil.popupWindowOldEventListener(); PickListUtil.PopupWindow_hidePopupWindows();");
	}
    else 
    {
	    document.onmouseup = PickListUtil.PopupWindow_hidePopupWindows;
	}
}

PickListUtil.PickListKeyPress = function(obj_id, event)
{

    var code = event.keyCode ? event.keyCode : event.charCode;

    if (code == 13 || code == 10)
    {
        PickListUtil["canhide_" + obj_id] = true;

        PickListUtil.PopupWindow_hidePopupWindows();
        return true;
    }


    return true;
}


window.addEventListener?window.addEventListener("load",PickListUtil.pload,false):window.attachEvent("onload", PickListUtil.pload);

PickListUtil.PopupWindow_hidePopupWindows = function()
{     
   var ids = PickListUtil.idPickLists.split(",");
    
   var count = ids.length;
   for(var i = 0; i < count; i++)
   {
      var id = ids[i];
      
      if(id != "")
      {
         var pnl = document.getElementById(id + "_divList");
         
         var prevSelection = document.getElementById(id + "_hdnPrevSelectedValues");
         var currentSelection = document.getElementById(id + "_hdnSelectedValues");
         
         if(pnl.style.display == "" && PickListUtil["canhide_" + id])  
         {
            if (prevSelection.value != currentSelection.value)
                if(typeof PickListUtil["config_" + id].onHide == "function")
                    PickListUtil["config_" + id].onHide();
         }
  
         if(PickListUtil["canhide_" + id])  
         {
             pnl.style.display = "none";

             if (PickListUtil.FuncPickListKeyPress != null)
                document.body.removeEventListener ? document.body.removeEventListener("onkeydown", PickListUtil.FuncPickListKeyPress, false) : document.body.detachEvent("onkeydown", PickListUtil.FuncPickListKeyPress);
         }
         
         if (prevSelection)
            prevSelection.value = document.getElementById(id + "_hdnSelectedValues").value;
      }
   }
}

PickListUtil.AddPickList = function(id, jsonData, containerId, config)
{
    PickListUtil.idPickLists += (PickListUtil.idPickLists == "" ? "" : ",") + id;

    PickListUtil["overbutton_" + id] = false;
    PickListUtil["visiblepnl_" + id] = false;
    PickListUtil["canhide_" + id] = true;

    PickListUtil["config_" + id] = config;

    PickListUtil.is_chrome = navigator.userAgent.toLowerCase().indexOf('chrome') > -1;
    PickListUtil.is_IEBef8 = false;

    var IEname = 'MSIE ';
    var IEidx = navigator.userAgent.indexOf(IEname);

    if (IEidx > -1)
    {
        var IEVer = navigator.userAgent.substring(IEidx + IEname.length, IEidx + IEname.length + 1);

        //if(IEVer < 8)
        PickListUtil.is_IEBef8 = true;
    }

    var container = document.getElementById(containerId);

    var divWrapper = document.createElement("div");
    divWrapper.style.width = "100%";
    divWrapper.style.padding = "0";
    divWrapper.style.verticalAlign = "top";
    divWrapper.style.textAlign = "left";
    container.appendChild(divWrapper);

    var hdnSelectedValues = document.createElement("input");

    hdnSelectedValues.setAttribute("type", "hidden");
    hdnSelectedValues.value = "";
    hdnSelectedValues.id = id + "_hdnSelectedValues";

    divWrapper.appendChild(hdnSelectedValues);

    var hdnPrevSelection = document.createElement("input");

    hdnPrevSelection.setAttribute("type", "hidden");
    hdnPrevSelection.value = "";
    hdnPrevSelection.id = id + "_hdnPrevSelectedValues";

    divWrapper.appendChild(hdnPrevSelection);

    var txtSelected = document.createElement("input");

    txtSelected.setAttribute("type", "text");
    txtSelected.value = "";
    txtSelected.id = id + "_txtSelected";
    txtSelected.setAttribute("readonly", "");
    txtSelected.readOnly = true;
    txtSelected.className = "InputField";
    txtSelected.style.cursor = "default";

    if (config.width != undefined)
        txtSelected.style.width = config.width + "px";
    else
        txtSelected.style.width = defaultPickListConfig.width + "px";

    //txtSelected.style.height = "12px";   

    divWrapper.appendChild(txtSelected);

    var imgDropDown = document.createElement("img");
    imgDropDown.id = id + "_imgDropDown";
    imgDropDown.src = "../Images/DropDownSelector.gif";
    imgDropDown.style.cursor = "pointer";
    imgDropDown.style.position = "relative";

    imgDropDown.style.top = (PickListUtil.is_chrome ? "4" : "3") + "px";
    imgDropDown.style.left = (PickListUtil.is_chrome ? "-2" : "0") + "px";

    if (imgDropDown.addEventListener)
    {
        imgDropDown.addEventListener("click", pickList_Click, false);
        imgDropDown.addEventListener("mouseover", pickList_MouseOver, false);
        imgDropDown.addEventListener("mouseout", pickList_MouseOut, false);
    } else if (imgDropDown.attachEvent)
    {
        imgDropDown.attachEvent("onclick", pickList_Click);
        imgDropDown.attachEvent("onmouseover", pickList_MouseOver);
        imgDropDown.attachEvent("onmouseout", pickList_MouseOut);
    } else
    {
        imgDropDown.onmouseover = pickList_Click;
        imgDropDown.onmouseover = pickList_MouseOver;
        imgDropDown.onmouseout = pickList_MouseOut;
    }

    function pickList_Click()
    {
        var pnl = document.getElementById(id + "_divList");

        if (pnl.style.display == "none" && !PickListUtil["visiblepnl_" + id])
        {
            pnl.style.display = "";
            pnl.scrollTop = 0;

            var cnt = pnl.childNodes[0].childNodes[0].getElementsByTagName("tr").length;

            if (cnt >= 15)
                pnl.style.height = "350px";
            else
                pnl.style.height = ((cnt * 25) + (PickListUtil.is_IEBef8 ? 20 : 15)) + "px";
        }

        PickListUtil["visiblepnl_" + id] = !PickListUtil["visiblepnl_" + id];
    }

    function pickList_MouseOver()
    {
        PickListUtil["overbutton_" + id] = true;
        PickListUtil["visiblepnl_" + id] = (document.getElementById(id + "_divList").style.display == '');
    }

    function pickList_MouseOut()
    {
        PickListUtil["overbutton_" + id] = false;
    }

    divWrapper.appendChild(imgDropDown);

    if (PickListUtil.is_IEBef8)
    {
        var br = document.createElement("br");
        divWrapper.appendChild(br);
    }

    var divList = document.createElement("div");

    divList.id = id + "_divList";

    if (config.width != undefined)
        divList.style.width = (config.width + (PickListUtil.is_chrome ? 1 : 3)) + "px";
    else
        divList.style.width = (defaultPickListConfig.width + (PickListUtil.is_chrome ? 1 : 3)) + "px";

    divList.style.display = "none";
    divList.style.height = "350px";
    divList.style.position = "absolute";
    divList.style.zIndex = "100";
    divList.style.border = "solid 1px #AAAAAA";
    divList.style.overflowY = "scroll";
    divList.style.overflowX = "scroll";
    divList.style.backgroundColor = "#FFFFFF";

    if (divList.addEventListener)
    {
        divList.addEventListener("click", list_Click, false);
        divList.addEventListener("mouseover", list_MouseOver, false);
        divList.addEventListener("mouseout", list_MouseOut, false);
    } else if (divList.attachEvent)
    {
        divList.attachEvent("onclick", list_Click);
        divList.attachEvent("onmouseover", list_MouseOver);
        divList.attachEvent("onmouseout", list_MouseOut);
    } else
    {
        divList.onmouseover = list_Click;
        divList.onmouseover = list_MouseOver;
        divList.onmouseout = list_MouseOut;
    }

    function list_Click()
    {
    }

    function list_MouseOver()
    {
        PickListUtil["canhide_" + id] = false;
    }

    function list_MouseOut()
    {
        PickListUtil["canhide_" + id] = true;
    }

    var divCont = document.createElement("div");

    divCont.id = id + "_divCont";

    divList.appendChild(divCont);

    divWrapper.appendChild(divList);

    PickListUtil.ReloadPickList(id, jsonData, config);

    //This is why we keep a reference to the function in a global variable
    FuncPickListKeyPress = function()
    {
        return PickListUtil.PickListKeyPress(id, event);
    }

    document.body.addEventListener ? document.body.addEventListener("onkeydown", FuncPickListKeyPress, false) : document.body.attachEvent("onkeydown", FuncPickListKeyPress);
}

PickListUtil.ReloadPickList = function ReloadPickList(id, jsonData, config) {
    if (jsonData == null)
        jsonData = [];

    var divCont = document.getElementById(id + "_divCont");

    if (divCont.hasChildNodes())
        divCont.removeChild(divCont.lastChild);

    var tblItems = document.createElement("table");

    tblItems.style.cellPadding = "0";
    tblItems.style.cellSpacing = "0";

    var rowAll = document.createElement("tr");

    rowAll.style.verticalAlign = "middle";

    var tdAll = document.createElement("td");

    tdAll.setAttribute("nowrap", "");
    tdAll.style.textAlign = "left";

    var chkAll = document.createElement("input");

    chkAll.setAttribute("type", "checkbox");
    chkAll.id = id + "_chkItem0";
    chkAll.value = "-1";
    chkAll.setAttribute("pl_id", id);

    if (!PickListUtil.is_IEBef8) {
        if (chkAll.addEventListener) {
            chkAll.addEventListener("click", chk_Click, false);
        } else if (chkAll.attachEvent) {
            chkAll.attachEvent("onclick", chk_Click);
        } else {
            chkAll.onclick = chk_Click;
        }
    }
    else
        chkAll.setAttribute("onclick", "chk_Click(event, '" + id + "');");

    tdAll.appendChild(chkAll);

    var lblAll = document.createElement("label");

    lblAll.id = id + "_lblItem0";
    lblAll.setAttribute("for", id + "_chkItem0");
    lblAll.style.cursor = "default";
    lblAll.style.verticalAlign = "top";
    lblAll.style.position = "relative";
    lblAll.style.top = "2px";

    if (config.allLabel != undefined)
        lblAll.innerHTML = config.allLabel;
    else
        lblAll.innerHTML = defaultPickListConfig.allLabel;

    tdAll.appendChild(lblAll);

    rowAll.appendChild(tdAll);

    tblItems.appendChild(rowAll);

    var count = jsonData.length;
    for (var i = 0; i < count; i++) {
        var item = jsonData[i];

        var row = document.createElement("tr");

        row.style.verticalAlign = "middle";

        var td = document.createElement("td");

        td.setAttribute("nowrap", "");
        td.style.textAlign = "left";

        var chk = document.createElement("input");

        chk.setAttribute("type", "checkbox");
        chk.id = id + "_chkItem" + (i + 1);
        chk.value = item.value;
        chk.setAttribute("pl_id", id);

        if (!PickListUtil.is_IEBef8) {
            if (chk.addEventListener) {
                chk.addEventListener("click", chk_Click, false);
            } else if (chk.attachEvent) {
                chk.attachEvent("onclick", chk_Click);
            } else {
                chk.onclick = chk_Click;
            }
        }
        else
            chk.setAttribute("onclick", "chk_Click(event, '" + id + "');");

        td.appendChild(chk);

        var lbl = document.createElement("label");

        lbl.id = id + "_lblItem" + (i + 1);
        lbl.setAttribute("for", id + "_chkItem" + (i + 1));
        lbl.style.cursor = "default";
        lbl.style.verticalAlign = "top";
        lbl.style.position = "relative";
        lbl.style.top = "2px";

        lbl.innerHTML = item.label;

        td.appendChild(lbl);

        row.appendChild(td);

        tblItems.appendChild(row);
    }

    divCont.appendChild(tblItems);

    if (PickListUtil.is_IEBef8)
        divCont.innerHTML = divCont.innerHTML;
}

function chk_Click(evt)
{
  var id = "";
  
  var e_out; 
  var ie_var="srcElement"; 
  var moz_var="target"; 
  var prop_var="id";

  evt[moz_var] ? e_out = evt[moz_var][prop_var] : e_out = evt[ie_var][prop_var]; 

  var obj = document.getElementById(e_out);
  
  id = obj.getAttribute("pl_id");

  if(obj.value == "-1")
  {
     var cnt = obj.parentNode.parentNode.parentNode.getElementsByTagName("tr").length;
  
     for(var i = 1; i <= cnt - 1; i++)
        document.getElementById(id + "_chkItem" + i).checked = obj.checked;
  }
  else
  {  
     document.getElementById(id + "_chkItem0").checked = false;
  }

  refresh_Val(id);
  
  if(typeof PickListUtil["config_" + id].onPickItem == "function")
     PickListUtil["config_" + id].onPickItem();
}

function refresh_Val(idpar)
{
  var id = idpar;

  var pnl = document.getElementById(id + "_divList");

  var val = "";
  var txt = "";

  var cnt = pnl.childNodes[0].childNodes[0].getElementsByTagName("tr").length;
  
  for(var i = 1; i <= cnt - 1; i++)
     if(document.getElementById(id + "_chkItem" + i).checked)
     {
        val += (val == "" ? "" : ",") + document.getElementById(id + "_chkItem" + i).value;
        
        if (document.all)
            txt += (txt == "" ? "" : ", ") + document.getElementById(id + "_lblItem" + i).innerText;
        else
            txt += (txt == "" ? "" : ", ") + document.getElementById(id + "_lblItem" + i).textContent;
            
     }
  
  document.getElementById(id + "_hdnSelectedValues").value = val;
  document.getElementById(id + "_txtSelected").value = txt;   
}

PickListUtil.SetSelectionAll = function SetSelectionAll(idpar)
{
    var id = idpar;
    var pnl = document.getElementById(id + "_divList");
    var cnt = pnl.childNodes[0].childNodes[0].getElementsByTagName("tr").length;

    PickListUtil.ClearSelection(idpar);
    
    for(var j = 0; j <= cnt - 1; j++)
    {
        if(document.getElementById(id + "_chkItem" + j).value != null)
        {
            document.getElementById(id + "_chkItem" + j).checked = true;
        }
    }
    
    refresh_Val(id);
}

PickListUtil.SetSelection = function SetSelection(idpar, itemsToSelect)
{
    var id = idpar;
    var pnl = document.getElementById(id + "_divList");
    var cnt = pnl.childNodes[0].childNodes[0].getElementsByTagName("tr").length;
    
    var items = "";
    
    if (itemsToSelect != "")
        items = itemsToSelect.split(",");

    PickListUtil.ClearSelection(idpar);
    
    var count = items.length;
    if (items != "")
    {
        for (var i=0; i < count; i++)
        {
            for(var j = 1; j <= cnt - 1; j++)
            {
                if(document.getElementById(id + "_chkItem" + j).value == items[i])
                {
                    document.getElementById(id + "_chkItem" + j).checked = true;
                }
            }
        }
    }

    refresh_Val(id);
}

PickListUtil.ClearSelection = function ClearSelection(idpar)
{
    var id = idpar;
    var pnl = document.getElementById(id + "_divList");
    var sel = "";
    
    var cnt = pnl.childNodes[0].childNodes[0].getElementsByTagName("tr").length;
    for(var i = 1; i <= cnt - 1; i++)
        if (document.getElementById(id + "_chkItem" + i))
             if (document.getElementById(id + "_chkItem" + i).checked)
                document.getElementById(id + "_chkItem" + i).checked = false;
                
    var txtSelected = document.getElementById(idpar + "_txtSelected");
    var hdnSelected = document.getElementById(idpar + "_hdnSelectedValues");
    
    txtSelected.value = "";
    hdnSelected.value = "";
}

PickListUtil.DisablePickList = function DisablePickList(pickList, label, disabled)
{
    var txtSelected = document.getElementById(pickList + "_txtSelected");
    var hdnSelected = document.getElementById(pickList + "_hdnSelectedValues");
    var image = document.getElementById(pickList + "_imgDropDown");

    PickListUtil.ClearSelection(pickList);
    
    if (hdnSelected)
        hdnSelected.value = "";
        
    if (label)
    {
        label.style.color = disabled ? "#808080" : "#000099";
    }
    
    if (txtSelected)
    {
        txtSelected.value = "";
        txtSelected.disabled = disabled ? true : false;
    }
    
    if (image)
    {
        image.style.display = disabled ? "none" : "";
    }
}

PickListUtil.GetSelectedValues = function GetSelectedValues(idpar)
{
    var id = idpar;
    var hdnSelected = document.getElementById(idpar + "_hdnSelectedValues");
    return hdnSelected.value;
}

PickListUtil.GetSelectedText = function GetSelectedText(idpar) {
    var id = idpar;
    var txtSelected = document.getElementById(idpar + "_txtSelected");
    return txtSelected.value;
}
