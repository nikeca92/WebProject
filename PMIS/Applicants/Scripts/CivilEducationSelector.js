var civilEducationSelector = (function() {
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
        document.getElementById(lightBox.id + "_txtCivilEducationSearch").focus();

        if (callbackFunction != null) {
            callbackFunctions[widgetId] = callbackFunction;
        }
    };

    var setupLightboxSearchPnlHTML = function(lightBox) {
        lightBox.innerHTML += "<img src='../Images/Close.png' style='cursor: pointer; float:right;' onclick='civilEducationSelector.hideDialog(\"" + lightBox.id + "\");' />" +
                             "      <div id='" + lightBox.id + "_pnlSearch'> " +
                             "               <table>" +
                             "                   <tr>" +
                             "                       <td colspan=\"4\" style=\"text-align: center; padding-top: 10px; padding-bottom: 10px; font-size: 0.95em; font-weight: bold;\">" +
                             "                           Търсене на специалности" +
                             "                       </td>" +
                             "                   </tr>" +
                             "                   " +
                             "                   <tr style=\"min-height: 17px\">" +
                             "                       <td style=\"text-align: right; width: 80px;\">" +
                             "                           <span id=\"" + lightBox.id + "_lblCivilEducationSearch\" class=\"InputLabel\">Специалност:</span>" +
                             "                       </td>" +
                             "                       <td style=\"text-align: right; width: 100px;\">" +
                             "                           <select id=\"" + lightBox.id + "_ddSearchType\">" +
                             "                              <option value='starts_with'>Започва с</option>" +
                             "                              <option value='contains'>Съдържа</option>" +
                             "                              <option value='ends_with'>Завършва с</option>" +
                             "                           </select>" +
                             "                       </td>" +
                             "                       <td style=\"text-align: left;\">" +
                             "                           <input id='" + lightBox.id + "_txtCivilEducationSearch' style='width: 550px;' onkeydown='civilEducationSelector.search.keyPressed(event, \"" + lightBox.id + "\");' />" +
                             "                       </td>" +
                             "                       <td style=\"text-align: left; width: 120px; padding-top: 10px;\">" +
                             "                           <div id=\"" + lightBox.id + "_btnCivilEducationSearch\" style=\"display: inline;\" onclick='civilEducationSelector.search.btnSearch_Click(\"" + lightBox.id + "\");'" +
                             "                                class=\"Button\">" +
                             "                               <i></i>" +
                             "                               <div id=\"" + lightBox.id + "_btnCivilEducationSearch\" style=\"width: 70px;\">" +
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

    var searchCivilEducation = function(widgetId) {
        var searchType = document.getElementById(widgetId + "_ddSearchType").value;
        var searchText = document.getElementById(widgetId + "_txtCivilEducationSearch").value;

        var url = "CivilEducationSelectorHandlers.aspx?AjaxMethod=JSSearchCivilEducation";
        var params = "";
        params += "SearchType=" + custEncodeURI(searchType);
        params += "&SearchText=" + custEncodeURI(searchText);

        function searchCivilEducation_Callback(xml) {
            var personCivilEducationsList = xml.getElementsByTagName("personEducation");
            var totalRowsCount = parseInt(xmlValue(xml, "totalRowsCount"));
            var returnedRowsCount = personCivilEducationsList.length;
            var html = "";

            if (personCivilEducationsList.length > 0) {
                if (returnedRowsCount < totalRowsCount)
                    html += "<div style='text-align: left; margin-bottom: 7px; font-style: italic;'>Показани са " + returnedRowsCount + " от общо " + totalRowsCount + " намерени специалности</div>";

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
                       "         <th style='vertical-align: bottom;'>Специалност</th>" +
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

                for (var i = 0; i < personCivilEducationsList.length; i++) {
                    var schoolSubjectName = xmlValue(personCivilEducationsList[i], "schoolSubjectName");
                    var schoolSubjectCode = xmlValue(personCivilEducationsList[i], "schoolSubjectCode");

                    html += "<tr class='ListItem " + (i % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + "' " +
                            "    onclick=\"civilEducationSelector.search.chooseCivilEducation('" + widgetId + "', '" + schoolSubjectCode + "', '" + schoolSubjectName.replace(/"/g, '&quot;') + "');\" " +
                            "    title='Избери' >" +
                            "   <td style='text-align: center;'>" + (i + 1) + "</td>" +
                            "   <td>" + schoolSubjectName + "</td>" +
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

        var myAJAX = new AJAX(url, true, params, searchCivilEducation_Callback);
        myAJAX.Call();
    };

    var handleKeyPressed = function(e, widgetId) {
        if (!e) e = window.event;   // resolve event instance

        if (e.keyCode == '13') {
            searchCivilEducation(widgetId);
            return false;
        }
        else if (e.keyCode == '27') {
            civilEducationSelector.hideDialog(widgetId);
            return false;
        }
    };

    var handleChooseCivilEducation = function(widgetId, schoolSubjectCode, schoolSubjectName) {
        if (callbackFunctions[widgetId]) {
            callbackFunctions[widgetId](schoolSubjectCode, schoolSubjectName);
        }

        civilEducationSelector.hideDialog(widgetId);
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
                searchCivilEducation(widgetId);
            },

            keyPressed: function(e, widgetId) {
                handleKeyPressed(e, widgetId);
            },

            chooseCivilEducation: function(widgetId, schoolSubjectCode, schoolSubjectName) {
                handleChooseCivilEducation(widgetId, schoolSubjectCode, schoolSubjectName);
            }
        },
    };
})();