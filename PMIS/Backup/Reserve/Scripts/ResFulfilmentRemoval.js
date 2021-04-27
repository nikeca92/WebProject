var resFulfilmentRemoval = (function() {
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

    var getData = function(widgetId, callbackFunction, requestCommantID, militaryDepartmentID) {
        if (callbackFunction != null) {
            callbackFunctions[widgetId] = callbackFunction;
        }

        var url = "ResFulfilmentRemovalHandlers.aspx?AjaxMethod=JSGetData";

        var params = "RequestCommantID=" + requestCommantID + "&MilitaryDepartmentID=" + militaryDepartmentID;

        function getData_Callback(xml) {
            appendHidePage(widgetId);
            createAndAppendLightbox(widgetId, extractData(xml));
        }

        var myAJAX = new AJAX(url, true, params, getData_Callback);
        myAJAX.Call();
    }

    var extractData = function(xml) {

        var data = {};

        data.requestCommand = {};
        var requestCommand = xml.getElementsByTagName("RequestCommand")[0];
        data.requestCommand.requestCommandID = xmlValue(requestCommand, "RequestCommandID");
        data.requestCommand.requestCommandName = xmlValue(requestCommand, "RequestCommandName");
        data.requestCommand.subRequestCommandName = xmlValue(requestCommand, "SubRequestCommandName");

        data.requestCommandPositions = [];
        var requestCommandPositions = xml.getElementsByTagName("RequestCommandPosition");

        if (requestCommandPositions.length > 0) {
            for (var i = 0; i < requestCommandPositions.length; i++) {

                var requestCommandPositionID = xmlValue(requestCommandPositions[i], "RequestCommandPositionID");
                var position = xmlValue(requestCommandPositions[i], "Position");
                var milRepSpec = xmlValue(requestCommandPositions[i], "MilRepSpec");
                var militaryRank = xmlValue(requestCommandPositions[i], "MilitaryRank");
                var reservistsCount = xmlValue(requestCommandPositions[i], "ReservistsCount");
                var fulfiled = xmlValue(requestCommandPositions[i], "Fulfiled");

                data.requestCommandPositions.push({ requestCommandPositionID: requestCommandPositionID,
                    position: position,
                    milRepSpec: milRepSpec,
                    militaryRank: militaryRank,
                    reservistsCount: reservistsCount,
                    fulfiled: fulfiled
                });
            }
        }

        return data;
    }

    var createAndAppendLightbox = function(widgetId, data) {
        var lightBox = document.createElement('div');
        lightBox.id = widgetId;

        setupLightboxPnlHTML(lightBox, data);
        setupLightboxStyles(lightBox);

        document.body.appendChild(lightBox);
        CenterLightBox(lightBox.id);
    };

    var setupLightboxPnlHTML = function(lightBox, data) {
        var html = "<div id='" + lightBox.id + "_pnl' style='min-height:510px;'> " +

                     "  <table style='text-align:center;'>" +
                     "      <tr>" +
                     "          <td>" +
                     "              <div class=\"HeaderText\" style=\"width:70%; text-align:center; margin:0 auto; margin-top: 20px; margin-bottom: 20px; \"> " +
                     "                  Прекратяване на МН на резервисти от команда: <span style=\"color:#000000;\">" + data.requestCommand.requestCommandName + "</span>," +
                     "                  подкоманда / заявка №: <span style=\"color:#000000;\">" + data.requestCommand.subRequestCommandName + "</span>" +
                     "              </div>" +
                     "          </td>" +
                     "      </tr>" +
                     "      <tr>" +
                     "          <td >" +
                     "          <div style='width:760px; margin:0 auto;'>" +
                     "              <table class=\"CommonHeaderTable\" " +
                     "                     style=\"width:730px; \" >" +
                     "                  <colgroup>" +
                     "                      <col style='width:30px;'>" +
                     "                      <col style='width:300px;'>" +
                     "                      <col style='width:200px;'>" +
        //"                      <col style='width:100px;'>" +
                     "                      <col style='width:100px;'>" +
                     "                      <col style='width:100px;'>" +
                     "                  </colgroup>" +
                     "                  <thead>" +
                     "                  <tr>" +
                     "                      <th><input title=\"Избери всички\" type=\"checkbox\" id=\"" + lightBox.id + "_chkAll\" onclick=\"resFulfilmentRemoval.chkAll_Click('" + lightBox.id + "')\"/></th>" +
                     "                      <th>Длъжност</th>" +
                     "                      <th>ВОС</th>" +
        //"                      <th>Звание</th>" +
                     "                      <th>Заявени</th>" +
                     "                      <th>Изпълнени</th>" +
                     "                  </tr>" +
                     "                  </thead>" +
                     "              </table>" +
                     "              <div style='max-height: 325px; width: 750px; overflow-y:auto;'>" +
                     "                  <table class=\"CommonHeaderTable\" " +
                     "                         style=\"width:730px;\">" +
                     "                  <colgroup>" +
                     "                      <col style='width:30px;'>" +
                     "                      <col style='width:300px;'>" +
                     "                      <col style='width:200px;'>" +
        //"                      <col style='width:100px;'>" +
                     "                      <col style='width:100px;'>" +
                     "                      <col style='width:100px;'>" +
                     "                  </colgroup>";
        for (var i = 0; i < data.requestCommandPositions.length; i++) {

            html += "                   <tr>" +
                    "                       <td id='" + lightBox.id + "_tr_" + (i + 1) + "_td_1' style=\"text-align:center; vertical-align: text-top;\">" +
                    "                           <input id=\"" + lightBox.id + "_chkRequestCommandPosition_" + (i + 1) + "\" type=\"checkbox\" data-tr=\"" + (i + 1) + "\" onclick=\"resFulfilmentRemoval.chk_Click(this, '" + lightBox.id + "' )\"/>" +
                    "                           <input type='hidden' id='" + lightBox.id + "_requestCommandPositionID_" + (i + 1) + "' value='" + data.requestCommandPositions[i].requestCommandPositionID + "' />" +
                    "                       </td>" +
                    "                       <td id='" + lightBox.id + "_tr_" + (i + 1) + "_td_2' style='text-align: left; vertical-align: text-top; word-break:break-all;'>" + data.requestCommandPositions[i].position + "</td>" +
                    "                       <td id='" + lightBox.id + "_tr_" + (i + 1) + "_td_3' style='text-align: left; word-break:break-all;'>" + data.requestCommandPositions[i].milRepSpec + "</td>" +
            //"                       <td id='" + lightBox.id + "_tr_" + (i + 1) + "_td_4' >" + data.requestCommandPositions[i].militaryRank + "</td>" +
                    "                       <td id='" + lightBox.id + "_tr_" + (i + 1) + "_td_4' >" + data.requestCommandPositions[i].reservistsCount + "</td>" +
                    "                       <td id='" + lightBox.id + "_tr_" + (i + 1) + "_td_5' >" + data.requestCommandPositions[i].fulfiled + "</td>" +
                    "                   </tr>";
        }

        html += "                    </table>" +
               "                    </div></div>" +
               "                </td>" +
               "            </tr>" +
               "            <tr>" +
               "               <td>" +
               "                   <div><table style='width:310px; margin:0 auto; margin-top:10px;'>" +
               "                       <tr>" +
               "                           <td style=\"text-align: center; padding-top: 5px;\">" +
               "                               <div style=\"width:300px; margin:0 auto;\">" +
               "                                   <div id=\"" + lightBox.id + "_btnRemove\" style=\"display: inline;\" onclick='resFulfilmentRemoval.btnRemove_Click(\"" + lightBox.id + "\");' class=\"Button\">" +
               "                                       <i></i>" +
               "                                       <div id=\"" + lightBox.id + "_btnRemoveText\" style=\"width: 170px;\">Прекратяване на МН</div>" +
               "                                       <b></b>" +
               "                                   </div>" +
               "                                   &nbsp;" +
               "                                   <div id=\"" + lightBox.id + "_btnBack\" style=\"display: inline;\" onclick=\"resFulfilmentRemoval.hideDialog('" + lightBox.id + "');\" class=\"Button\">" +
               "                                       <i></i>" +
               "                                       <div id=\"" + lightBox.id + "_btnBackCompanyText\" style=\"width: 70px;\" >Затвори</div>" +
               "                                       <b></b>" +
               "                                   </div>" +
               "                               </div>" +
               "                           </td>" +
               "                       </tr>" +
               "                   </table>" +
               "               </td>" +
               "            </tr>" +
               "        </table>" +
               "        <input type='hidden' id='" + lightBox.id + "_requestCommandID' value='" + data.requestCommand.requestCommandID + "' />" +
               "        <input type='hidden' id='" + lightBox.id + "_requestCommandPositionsCnt' value='" + data.requestCommandPositions.length + "' />" +
               "    </div>";


        lightBox.innerHTML = html;
    };

    var setupLightboxStyles = function(lightBox) {
        lightBox.style.width = '900px';
        lightBox.style.backgroundColor = '#EEEEEE';
        lightBox.style.border = 'solid 1px #000000';
        lightBox.style.position = 'fixed';
        lightBox.style.top = '120px';
        lightBox.style.left = '25%';
        //lightBox.style.height = '510px';
        lightBox.style.zIndex = '1001';
        lightBox.style.paddingTop = '0px';
        lightBox.style.paddingLeft = '15px';
    };

    var removeLightbox = function(widgetId) {
        var element = document.getElementById(widgetId);

        document.body.removeChild(element);
    };

    function toggleCheck(chk, widgetId) {
        var tr = parseInt(chk.getAttribute("data-tr"));

        var backgroundColor = chk.checked ? "#bcebff" : "";

        document.getElementById(widgetId + "_tr_" + tr + "_td_1").style.backgroundColor = backgroundColor;
        document.getElementById(widgetId + "_tr_" + tr + "_td_2").style.backgroundColor = backgroundColor;
        document.getElementById(widgetId + "_tr_" + tr + "_td_3").style.backgroundColor = backgroundColor;
        //document.getElementById(widgetId + "_tr_" + tr + "_td_4").style.backgroundColor = backgroundColor;
        document.getElementById(widgetId + "_tr_" + tr + "_td_4").style.backgroundColor = backgroundColor;
        document.getElementById(widgetId + "_tr_" + tr + "_td_5").style.backgroundColor = backgroundColor;
    }

    function toggleAll(widgetId) {
        var chk = document.getElementById(widgetId + "_chkAll");
        var requestCommandPositionsCnt = parseInt(document.getElementById(widgetId + "_requestCommandPositionsCnt").value);

        chk.title = chk.checked ? "Премахни всички" : "Избери всички";

        for (var i = 0; i < requestCommandPositionsCnt; i++) {

            var backgroundColor = chk.checked ? "#bcebff" : "";

            document.getElementById(widgetId + "_tr_" + (i + 1) + "_td_1").style.backgroundColor = backgroundColor;
            document.getElementById(widgetId + "_tr_" + (i + 1) + "_td_2").style.backgroundColor = backgroundColor;
            document.getElementById(widgetId + "_tr_" + (i + 1) + "_td_3").style.backgroundColor = backgroundColor;
            //document.getElementById(widgetId + "_tr_" + (i + 1) + "_td_4").style.backgroundColor = backgroundColor;
            document.getElementById(widgetId + "_tr_" + (i + 1) + "_td_4").style.backgroundColor = backgroundColor;
            document.getElementById(widgetId + "_tr_" + (i + 1) + "_td_5").style.backgroundColor = backgroundColor;

            document.getElementById(widgetId + "_chkRequestCommandPosition_" + (i + 1)).checked = chk.checked;
        }
    }

    //remove data
    function removeData(widgetId) {
        var url = "ResFulfilmentRemovalHandlers.aspx?AjaxMethod=JSRemoveReservists";
        var params = "";

        var requestCommandPositionsCnt = parseInt(document.getElementById(widgetId + "_requestCommandPositionsCnt").value);

        var IDs = "";
        for (var i = 0; i < requestCommandPositionsCnt; i++) {
            var chk = document.getElementById(widgetId + "_chkRequestCommandPosition_" + (i + 1));

            if (chk.checked) {
                IDs += (IDs == "" ? "" : ",") + document.getElementById(widgetId + "_requestCommandPositionID_" + (i + 1)).value;                
            }
        }

        params = "RequestCommandPositionIDs=" + IDs;
        

        function RemoveData_Callback(xml) {

            removeHidePage(widgetId);
            removeLightbox(widgetId);

            if (callbackFunctions[widgetId]) {
                callbackFunctions[widgetId]();
            }
        }

        var myAJAX = new AJAX(url, true, params, RemoveData_Callback);
        myAJAX.Call();
    }

    //Public interface of the "namespace"/"module"
    return {
        showDialog: function(widgetId, callbackFunction, requestCommantID, militaryDepartmentID) {
            getData(widgetId, callbackFunction, requestCommantID, militaryDepartmentID);
        },

        hideDialog: function(widgetId) {
            removeHidePage(widgetId);
            removeLightbox(widgetId);
        },

        chkAll_Click: function(widgetId) {
            toggleAll(widgetId);
        },

        chk_Click: function(chk, widgetId) {
            toggleCheck(chk, widgetId);
        },

        btnRemove_Click: function(widgetId) {
            YesNoDialog('Сигурни ли сте, че желаете да премахнете МН за избраните записи?', function() { removeData(widgetId); }, null);
        }
    };
})();