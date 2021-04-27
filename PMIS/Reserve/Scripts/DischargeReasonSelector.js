var dischargeReasonSelector = (function() {
    //Private members of the "namespace"/"module"
    var callbackFunctions = {};

    var appendHidePage = function(widgetId) {
        var hidePage = document.createElement('div');
        hidePage.id = widgetId + "_hidePage";
        hidePage.className = "HidePage";
        document.body.appendChild(hidePage);
    }

    var removeHidePage = function(widgetId) {
        var element = document.getElementById(widgetId + "_hidePage");
        document.body.removeChild(element);
    }

    var createAndAppendLightbox = function(widgetId, callbackFunction) {
        var lightBox = document.createElement('div');
        lightBox.id = widgetId;

        setupLightboxSearchPnlHTML(lightBox);
        setupLightboxStyles(lightBox);

        document.body.appendChild(lightBox);
        CenterLightBox(lightBox.id);
        document.getElementById(lightBox.id + "_txtDischargeReasonSearch").focus();

        if (callbackFunction != null) {
            callbackFunctions[widgetId] = callbackFunction;
        }
    };

    var setupLightboxSearchPnlHTML = function(lightBox) {
        lightBox.innerHTML += "<img src='../Images/Close.png' style='cursor: pointer; float:right;' onclick='dischargeReasonSelector.hideDialog(\"" + lightBox.id + "\");' />" +
                             "      <div id='" + lightBox.id + "_pnlSearch'> " +
                             "               <table>" +
                             "                   <tr>" +
                             "                       <td colspan=\"4\" style=\"text-align: center; padding-top: 10px; padding-bottom: 10px; font-size: 0.95em; font-weight: bold;\">" +
                             "                           Търсене на причина за освобождаване от ВС" +
                             "                       </td>" +
                             "                   </tr>" +
                             "                   " +
                             "                   <tr style=\"min-height: 17px\">" +
                             "                       <td style=\"text-align: right; width: 80px;\">" +
                             "                           <span id=\"" + lightBox.id + "_lblDischargeReasonSearch\" class=\"InputLabel\">Причина:</span>" +
                             "                       </td>" +
                             "                       <td style=\"text-align: right; width: 100px;\">" +
                             "                           <select id=\"" + lightBox.id + "_ddSearchType\">" +
                             "                              <option value='starts_with'>Започва с</option>" +
                             "                              <option value='contains'>Съдържа</option>" +
                             "                              <option value='ends_with'>Завършва с</option>" +
                             "                           </select>" +
                             "                       </td>" +
                             "                       <td style=\"text-align: left;\">" +
                             "                           <input id='" + lightBox.id + "_txtDischargeReasonSearch' style='width: 550px;' onkeydown='dischargeReasonSelector.search.keyPressed(event, \"" + lightBox.id + "\");' />" +
                             "                       </td>" +
                             "                       <td style=\"text-align: left; width: 120px; padding-top: 10px;\">" +
                             "                           <div id=\"" + lightBox.id + "_btnDischargeReasonSearch\" style=\"display: inline;\" onclick='dischargeReasonSelector.search.btnSearch_Click(\"" + lightBox.id + "\");'" +
                             "                                class=\"Button\">" +
                             "                               <i></i>" +
                             "                               <div id=\"" + lightBox.id + "_btnDischargeReasonSearch\" style=\"width: 70px;\">" +
                             "                                    Търси</div>" +
                             "                               <b></b>" +
                             "                           </div>" +
                             "                       </td>" +
                             "                   </tr>" +
                             "                    <tr style=\"min-height: 17px\">" +
                             "                      <td colspan=\"4\" style=\"text-align: left;\"> " +
                             "                         <div id='" + lightBox.id + "_pnlSearchResult'>" +
                             "                            <div style='text-align: center;'>Задайте критерий за търсене и натиснете бутона 'Търси'</div>" +
                             "                         </div>" +
                             "                      </td>" +
                             "                   </tr>" +
                             "               </table>" +
                             "      </div>";

    };

    var setupLightboxStyles = function(lightBox) {
        lightBox.style.width = '900px';
        lightBox.style.backgroundColor = '#EEEEEE';
        lightBox.style.border = 'solid 1px #000000';
        lightBox.style.position = 'fixed';
        lightBox.style.top = '120px';
        lightBox.style.left = '25%';
        lightBox.style.height = '480px';
        lightBox.style.zIndex = '1001';
        lightBox.style.paddingTop = '0px';
        lightBox.style.paddingLeft = '15px';
    };

    var removeLightbox = function(widgetId) {
        var element = document.getElementById(widgetId);
        document.body.removeChild(element);
    };

    var searchDischargeReasons = function(widgetId) {
        var searchType = document.getElementById(widgetId + "_ddSearchType").value;
        var searchText = document.getElementById(widgetId + "_txtDischargeReasonSearch").value;

        var url = "DischargeReasonSelectorHandlers.aspx?AjaxMethod=JSSearchDischargeReasons";
        var params = "";
        params += "SearchType=" + custEncodeURI(searchType);
        params += "&SearchText=" + custEncodeURI(searchText);

        function searchDischargeReasons_Callback(xml) {
            var personDischargeReasonsList = xml.getElementsByTagName("dischargeReason");
            var totalRowsCount = parseInt(xmlValue(xml, "totalRowsCount"));
            var returnedRowsCount = personDischargeReasonsList.length;
            var html = "";

            if (personDischargeReasonsList.length > 0) {
                if (returnedRowsCount < totalRowsCount)
                    html += "<div style='text-align: left; margin-bottom: 7px; font-style: italic;'>Показани са " + returnedRowsCount + " от общо " + totalRowsCount + " намерени причини за снемане от отчет</div>";

                html += "<table class='CommonHeaderTable' style='width: 840px;'>" +
                       "   <colgroup>" +
                       "      <col style='width: 50px;'>" +
                       "      <col style='width: 370px;'>" +
                       "      <col style='width: 110px;'>" +
                       "      <col style='width: 140px;'>" +
                       "      <col style='width: 170px;'>" +
                       "   </colgroup>" +
                       "   <thead>" +
                       "      <tr>" +
                       "         <th style='vertical-align: bottom;'>№</th>" +
                       "         <th style='vertical-align: bottom;'>Причина за освобождаване от ВС</th>" +
                       "      </tr>" +
                       "   </thead>" +
                       "</table>" +
                       "<div style='max-height: 325px; width: 860px; overflow-y: auto;'>" +
                       "<table style='width: 840px;'>" +
                       "   <colgroup>" +
                       "      <col style='width: 50px;'>" +
                       "      <col style='width: 370px;'>" +
                       "      <col style='width: 110px;'>" +
                       "      <col style='width: 140px;'>" +
                       "      <col style='width: 170px;'>" +
                       "   </colgroup>";

                for (var i = 0; i < personDischargeReasonsList.length; i++) {
                    var dischargeReasonName = xmlValue(personDischargeReasonsList[i], "dischargeReasonName");
                    var dischargeReasonCode = xmlValue(personDischargeReasonsList[i], "dischargeReasonCode");

                    html += "<tr class='ListItem " + (i % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + "' " +
                            "    onclick=\"dischargeReasonSelector.search.chooseDischargeReason('" + widgetId + "', '" + dischargeReasonCode + "', '" + dischargeReasonName.replace(/"/g, '&quot;') + "');\" " +
                            "    title='Избери' >" +
                            "   <td style='text-align: center;'>" + (i + 1) + "</td>" +
                            "   <td>" + dischargeReasonName + "</td>" +
                            "</tr>";
                }

                html += "</div> " +
                        "</table>"
            }
            else {
                html = "<div style='text-align: center;'>Няма намерени резултати</div>";
            }

            document.getElementById(widgetId + "_pnlSearchResult").innerHTML = html;
        }

        var myAJAX = new AJAX(url, true, params, searchDischargeReasons_Callback);
        myAJAX.Call();
    };

    var handleKeyPressed = function(e, widgetId) {
        if (!e) e = window.event;   // resolve event instance

        if (e.keyCode == '13') {
            searchDischargeReasons(widgetId);
            return false;
        }
        else if (e.keyCode == '27') {
            dischargeReasonSelector.hideDialog(widgetId);
            return false;
        }
    };

    var handleChooseDischargeReason = function(widgetId, dischargeReasonCode, dischargeReasonName) {
        if (callbackFunctions[widgetId]) {
            callbackFunctions[widgetId](dischargeReasonCode, dischargeReasonName);
        }

        dischargeReasonSelector.hideDialog(widgetId);
    };

    //Public interface of the "namespace"/"module"

   function hideDialog(widgetId) {
        removeHidePage(widgetId);
        removeLightbox(widgetId);
    }
    
    return {
            showDialog: function(widgetId, callbackFunction) {
                appendHidePage(widgetId);
                createAndAppendLightbox(widgetId, callbackFunction);
            },
            
            hideDialog: function(widgetId) {
                removeHidePage(widgetId);
                removeLightbox(widgetId);
            },
            

            search: {
            
            btnSearch_Click: function(widgetId) {
                searchDischargeReasons(widgetId);
            },

            keyPressed: function(e, widgetId) {
                handleKeyPressed(e, widgetId);
            },

            chooseDischargeReason: function(widgetId, dischargeReasonCode, dischargeReasonName) {
                handleChooseDischargeReason(widgetId, dischargeReasonCode, dischargeReasonName);
            }
        },
    };
})();