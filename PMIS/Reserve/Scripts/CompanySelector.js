var companySelector = (function() {
    //Private members of the "namespace"/"module"
    var callbackFunctions = {};
    var alreadyOccupiedCompanyNames = {};
    var validBulstats = {};

    var modulData = {};

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

    var collectModuleDataAndCreateAndAppendLightbox = function(widgetId, callbackFunction) {
        var url = "CompanySelectorHandlers.aspx?AjaxMethod=JSCollectModuleData";
        var params = "";

        function collectModuleData_Callback(xml) {
            modulData.data = {};

            modulData.data.canInsertNewCompany = (xmlValue(xml, "canInsertNewCompany") == "yes" ? true : false);
            modulData.data.unifiedIdentityCodeLabelText = xmlValue(xml, "unifiedIdentityCodeLabelText");

            modulData.data.administrations = [];
            var administrations = xml.getElementsByTagName("administration");

            if (administrations.length > 0) {
                for (var i = 0; i < administrations.length; i++) {
                    var administrationID = xmlValue(administrations[i], "administrationID");
                    var administrationName = xmlValue(administrations[i], "administrationName");

                    modulData.data.administrations.push({ id: administrationID, name: administrationName });
                }
            }

            modulData.data.ownershipTypes = [];
            var ownershipTypes = xml.getElementsByTagName("ownershipType");
            if (ownershipTypes.length > 0) {
                for (var i = 0; i < ownershipTypes.length; i++) {
                    var ownershipTypeID = xmlValue(ownershipTypes[i], "ownershipTypeID");
                    var ownershipTypeName = xmlValue(ownershipTypes[i], "ownershipTypeName");

                    modulData.data.ownershipTypes.push({ id: ownershipTypeID, name: ownershipTypeName });
                }
            }

            modulData.data.regions = [];
            var regions = xml.getElementsByTagName("region");
            if (regions.length > 0) {
                for (var i = 0; i < regions.length; i++) {
                    var regionID = xmlValue(regions[i], "regionID");
                    var regionName = xmlValue(regions[i], "regionName");
                    modulData.data.regions.push({ id: regionID, name: regionName });
                }
            }

            appendHidePage(widgetId);
            createAndAppendLightbox(widgetId, callbackFunction);
        }

        var myAJAX = new AJAX(url, true, params, collectModuleData_Callback);
        myAJAX.Call();
    }

    var createAndAppendLightbox = function(widgetId, callbackFunction) {
        var lightBox = document.createElement('div');
        lightBox.id = widgetId;

        setupLightboxSearchPnlHTML(lightBox);
        setupLightboxInsertPnlHTML(lightBox);
        setupLightboxStyles(lightBox);

        document.body.appendChild(lightBox);
        CenterLightBox(lightBox.id);
        document.getElementById(lightBox.id + "_txtSearchCompany").focus();

        if (callbackFunction != null) {
            callbackFunctions[widgetId] = callbackFunction;
        }
    };

    var setupLightboxSearchPnlHTML = function(lightBox) {
        lightBox.innerHTML += "<img src='../Images/Close.png' style='cursor: pointer; float:right;' onclick='companySelector.hideDialog(\"" + lightBox.id + "\");' />" +
                             "      <div id='" + lightBox.id + "_pnlSearch'> " +
                             "               <table>" +
                             "                   <tr>" +
                             "                       <td colspan=\"4\" style=\"text-align: center; padding-top: 10px; padding-bottom: 10px; font-size: 0.95em; font-weight: bold;\">" +
                             "                           Търсене на фирма" +
                             "                       </td>" +
                             "                   </tr>" +
                             "                   " +
                             "                   <tr style=\"min-height: 17px\">" +
                             "                       <td style=\"text-align: right; width: 80px;\">" +
                             "                           <span id=\"" + lightBox.id + "_lblSearchCompany\" class=\"InputLabel\">Име/" + modulData.data.unifiedIdentityCodeLabelText + ":</span>" +
                             "                       </td>" +
                             "                       <td style=\"text-align: right; width: 100px;\">" +
                             "                           <select id=\"" + lightBox.id + "_ddSearchType\">" +
                             "                              <option value='starts_with'>Започва с</option>" +
                             "                              <option value='contains'>Съдържа</option>" +
                             "                              <option value='ends_with'>Завършва с</option>" +
                             "                           </select>" +
                             "                       </td>" +
                             "                       <td style=\"text-align: left;\">" +
                             "                           <input id='" + lightBox.id + "_txtSearchCompany' style='width: 550px;' onkeydown='companySelector.search.keyPressed(event, \"" + lightBox.id + "\");' />" +
                             "                       </td>" +
                             "                       <td style=\"text-align: left; width: 120px; padding-top: 10px;\">" +
                             "                           <div id=\"" + lightBox.id + "_btnSearchCompany\" style=\"display: inline;\" onclick='companySelector.search.btnSearch_Click(\"" + lightBox.id + "\");'" +
                             "                                class=\"Button\">" +
                             "                               <i></i>" +
                             "                               <div id=\"" + lightBox.id + "_btnSearchCompanyText\" style=\"width: 70px;\">" +
                             "                                    Търси</div>" +
                             "                               <b></b>" +
                             "                           </div>" +
                             "                       </td>" +
                             "                   </tr>" +
                             " " +
                             "                   <tr style=\"min-height: 17px\">" +
                             "                      <td colspan=\"4\" style=\"text-align: left;\"> " +
                             "                         <div id='" + lightBox.id + "_pnlSearchResult'>" +
                             "                            <div style='text-align: center;'>Задайте критерий за търсене и натиснете бутона 'Търси'</div>" +
                             "                         </div>" +
                             (modulData.data.canInsertNewCompany ? "<div style='text-align: left; padding-top: 10px; padding-bottom: 5px;'><a href=\"\" onclick=\"companySelector.goTo('" + lightBox.id + "','insert'); return false;\">Добавяне на нова фирма</a></div>" : "") +
                             "                      </td>" +
                             "                   </tr>" +
                             "               </table>" +
                             "      </div>";

    };

    var setupLightboxInsertPnlHTML = function(lightBox) {
        var html = "";
        html += "  <div id='" + lightBox.id + "_pnlInsert' style=\"display:none;\"> " +
                     " <table>" +
                     "      <tr>" +
                     "          <td  style=\"text-align: center; padding-top: 10px; padding-bottom: 10px; font-size: 0.95em; font-weight: bold;\">" +
                     "              Добавяне на нова фирма" +
                     "          </td>" +
                     "      </tr>" +
                     "   <tr>" +
                     "      <td>" +
                     "          <center>" +
                     "              <fieldset style=\"width: 870px; padding: 5px;\">" +
                     "                  <legend style=\"color: #0B4489; font-weight: bold; font-size: 1.1em;\">Данни на фирмата</legend>" +
                     "                  <table style=\"width: 100%;\">" +
                     "                      <tr>" +
                     "                          <td style=\"text-align: right;\">" +
                     "                              <span id=\"" + lightBox.id + "_lblCompanyName\" class=\"InputLabel\">Име:</span>" +
                     "                          </td>" +
                     "                          <td style=\"text-align: left;\">" +
                     "                              <input type=\"text\" id=\"" + lightBox.id + "_txtCompanyName\" class=\"RequiredInputField\" maxlength=\"500\" />" +
                     "                          </td>" +
                     "                          <td style=\"text-align: right; vertical-align: bottom;\">" +
                     "                              <span class=\"InputLabel\" id=\"" + lightBox.id + "_lblBulstat\">" + modulData.data.unifiedIdentityCodeLabelText + "/ЕГН:</span>" +
                     "                          </td>" +
                     "                          <td style=\"text-align: left; vertical-align: bottom;\">" +
                     "                              <input type=\"text\" id=\"" + lightBox.id + "_txtBulstat\" class=\"InputField\" maxlength=\"50\" />" +
                     "                          </td>" +
                     "                      </tr>" +
                     "                      <tr>" +
                     "                          <td style=\"text-align: right;\">" +
                     "                              <span class=\"InputLabel\" id=\"" + lightBox.id + "_lblOwnershipType\">Вид собственост:</span>" +
                     "                          </td>" +
                     "                          <td style=\"text-align: left;\">" +
                     "                              <select class=\"InputField\" id=\"" + lightBox.id + "_ddOwnershipType\" style=\"width:220px;\">";
        for (var i = 0; i < modulData.data.ownershipTypes.length; i++) {
            html += "<option value=\"" + modulData.data.ownershipTypes[i].id + "\">" + modulData.data.ownershipTypes[i].name + "</option>";
        }

        html += "                              </select>" +
                     "                          </td>" +
                     "                          <td style=\"text-align: right;\">" +
                     "                              <span class=\"InputLabel\" id=\"" + lightBox.id + "_lblAdministration\">Министeрство:</span>" +
                     "                          </td>" +
                     "                          <td style=\"text-align: left;\">" +
                     "                              <select class=\"InputField\" id=\"" + lightBox.id + "_ddAdministration\" style=\"width:220px;\">";

        for (var i = 0; i < modulData.data.administrations.length; i++) {
            html += "<option value=\"" + modulData.data.administrations[i].id + "\">" + modulData.data.administrations[i].name + "</option>";
        }

        html += "                              </select>" +
                     "                          </td>" +
                     "                      </tr>" +
                     "                      <tr>" +
                     "                          <td style=\"text-align: right;\">" +
                     "                              <span class=\"InputLabel\" id=\"" + lightBox.id + "_lblPhone\">Телефон:</span>" +
                     "                          </td>" +
                     "                          <td style=\"text-align: left;\">" +
                     "                              <input type=\"text\" class=\"InputField\" style=\"\" id=\"" + lightBox.id + "_txtPhone\" maxlength=\"50\" />" +
                     "                          </td>" +
                     "                      </tr>" +
                     "                  </table>" +
                     "              </fieldset>" +
                     "          </center>" +
                     "      </td>" +
                     "  </tr>" +
                     "  <tr>" +
                     "      <td>" +
                     "          <center>" +
                     "              <fieldset style=\"width: 870px; padding: 5px;\">" +
                     "                  <legend style=\"color: #0B4489; font-weight: bold; font-size: 1.1em;\">Адрес на фирмата</legend>" +
                     "                  <table>" +
                     "                      <tr>" +
                     "                          <td style=\"text-align: right; width: 80px;\">" +
                     "                              <span id=\"" + lightBox.id + "_lblRegion\" class=\"InputLabel\">Област:</span>" +
                     "                          </td>" +
                     "                          <td style=\"text-align: left; width: 170px;\">" +
                     "                              <select class=\"InputField\" id=\"" + lightBox.id + "_ddRegion\" style=\"width:220px;\" onchange=\"companySelector.insert.ddRegion_Changed('" + lightBox.id + "');\">";

        for (var i = 0; i < modulData.data.regions.length; i++) {
            html += "<option value=\"" + modulData.data.regions[i].id + "\">" + modulData.data.regions[i].name + "</option>";
        }

        html += "                                   </select>" +
                     "                          </td>" +
                     "                          <td style=\"text-align: right; width: 80px;\">" +
                     "                              <span id=\"" + lightBox.id + "_lblMunicipality\" class=\"InputLabel\">Община:</span>" +
                     "                          </td>" +
                     "                          <td style=\"text-align: left; width: 170px;\">" +
                     "                              <select class=\"InputField\" id=\"" + lightBox.id + "_ddMunicipality\" style=\"width:220px;\" onchange=\"companySelector.insert.ddMunicipality_Changed('" + lightBox.id + "');\"></select>" +
                     "                          </td>" +
                     "                          <td style=\"text-align: right; width: 180px;\">" +
                     "                              <span id=\"" + lightBox.id + "_lblCity\" class=\"InputLabel\">Населено място:</span>" +
                     "                          </td>" +
                     "                          <td style=\"text-align: left; width: 200px;\">" +
                     "                             <select class=\"InputField\" id=\"" + lightBox.id + "_ddCity\" style=\"width:220px;\" onchange=\"companySelector.insert.ddCity_Changed('" + lightBox.id + "');\"></select>" +
                     "                          </td>" +
                     "                      </tr>" +
                     "                      <tr>" +
                     "                          <td rowspan=\"2\" style=\"text-align: right; vertical-align: top;\">" +
                     "                              <span id=\"" + lightBox.id + "_lblAddress\" class=\"InputLabel\">Адрес:</span>" +
                     "                          </td>" +
                     "                          <td colspan=\"3\" rowspan=\"2\" style=\"text-align: left;\">" +
                     "                              <textarea id=\"" + lightBox.id + "_txtaAddress\" cols=\"3\" rows=\"3\" class=\"InputField\" style=\"width: 99%;\"></textarea>" +
                     "                          </td>" +
                     "                          <td style=\"text-align: right;\">" +
                     "                              <span id=\"" + lightBox.id + "_lblDistrict\" class=\"InputLabel\">Район:</span>" +
                     "                          </td>" +
                     "                          <td style=\"text-align: left;\">" +
                     "                             <select class=\"InputField\" id=\"" + lightBox.id + "_ddDistrict\" style=\"width:220px;\" onchange=\"companySelector.insert.ddDistrict_Changed('" + lightBox.id + "');\"></select>" +
                     "                          </td>" +
                     "                      </tr>" +
                     "                     <tr>" +
                     "                          <td style=\"text-align: right; \">" +
                     "                              <span id=\"" + lightBox.id + "_lblPostCode\" class=\"InputLabel\">Пощенски код:</span>" +
                     "                          </td>" +
                     "                          <td style=\"text-align: left;\">" +
                     "                              <input id=\"" + lightBox.id + "_txtPostCode\" onfocus=\"companySelector.insert.txtPostCode_Focus('" + lightBox.id + "')\" onblur=\"companySelector.insert.txtPostCode_Blur('" + lightBox.id + "')\" " +
                     "                                     type=\"text\" class=\"InputField\" style=\"width: 50px;\" maxlength=\"4\" />" +
                     "                          </td>" +
                     "                      </tr>" +
                     "                  </table>" +
                     "              </fieldset>" +
                     "              <br />" +
                     "              <div class=\"ErrorText\" id=\"" + lightBox.id + "_lblMessage\" style=\"min-height: 10px;\">" +
                     "              </div>" +
                     "          </center>" +
                     "      </td>" +
                     "  </tr>" +
                     "  <tr>" +
                     "      <td style=\"text-align: center; padding-top: 5px;\">" +
                     "        <div style=\"width:200px; margin:0 auto;\">" +
                     "          <div id=\"" + lightBox.id + "_btnInsertCompany\" style=\"display: inline;\" onclick='companySelector.insert.btnInsert_Click(\"" + lightBox.id + "\");'" +
                     "               class=\"Button\">" +
                     "               <i></i>" +
                     "               <div id=\"" + lightBox.id + "_btnInsertCompanyText\" style=\"width: 70px;\">Запис</div>" +
                     "               <b></b>" +
                     "          </div>" +
                     "          &nbsp;" +
                     "          <div id=\"" + lightBox.id + "_btnBack\" style=\"display: inline;\" onclick=\"companySelector.goTo('" + lightBox.id + "','search');\"" +
                     "               class=\"Button\">" +
                     "               <i></i>" +
                     "               <div id=\"" + lightBox.id + "_btnBackCompanyText\" style=\"width: 70px;\" >Назад</div>" +
                     "               <b></b>" +
                     "          </div>" +
                     "        </div>" +
                     "      </td>" +
                     "  </tr>" +
                     "</table>" +
                     "</div>";

        lightBox.innerHTML += html;
    }

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

    var searchCompanies = function(widgetId) {
        var searchType = document.getElementById(widgetId + "_ddSearchType").value;
        var searchText = document.getElementById(widgetId + "_txtSearchCompany").value;

        var url = "CompanySelectorHandlers.aspx?AjaxMethod=JSSearchCompanies";
        var params = "";
        params += "SearchType=" + custEncodeURI(searchType);
        params += "&SearchText=" + custEncodeURI(searchText);

        function searchCompanies_Callback(xml) {
            var companiesList = xml.getElementsByTagName("company");
            var totalRowsCount = parseInt(xmlValue(xml, "totalRowsCount"));
            var returnedRowsCount = companiesList.length;
            var html = "";

            if (companiesList.length > 0) {
                if (returnedRowsCount < totalRowsCount)
                    html += "<div style='text-align: left; margin-bottom: 7px; font-style: italic;'>Показани са " + returnedRowsCount + " от общо " + totalRowsCount + " намерени фирми</div>";

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
                       "         <th style='vertical-align: bottom;'>Име</th>" +
                       "         <th style='vertical-align: bottom;'>" + modulData.data.unifiedIdentityCodeLabelText + "/ЕГН</th>" +
                       "         <th style='vertical-align: bottom;'>Населено място</th>" +
                       "         <th style='vertical-align: bottom;'>Вид собственост</th>" +
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

                for (var i = 0; i < companiesList.length; i++) {
                    var companyID = xmlValue(companiesList[i], "companyID");
                    var companyName = xmlValue(companiesList[i], "companyName");
                    var companyUnifiedIdentityCode = xmlValue(companiesList[i], "companyUnifiedIdentityCode");
                    var cityName = xmlValue(companiesList[i], "cityName");
                    var owneshipType = xmlValue(companiesList[i], "owneshipType");

                    html += "<tr class='ListItem " + (i % 2 == 0 ? "DataTableEvenRow" : "DataTableOddRow") + "' " +
                            "    onclick=\"companySelector.search.chooseCompany('" + widgetId + "', " + companyID + ", '" + companyName.replace(/"/g, '&quot;') + "', '" + companyUnifiedIdentityCode.replace(/"/g, '&quot;') + "', '" + owneshipType.replace(/"/g, '&quot;') + "');\" " +
                            "    title='Избери' >" +
                            "   <td style='text-align: center;'>" + (i + 1) + "</td>" +
                            "   <td>" + companyName + "</td>" +
                            "   <td>" + companyUnifiedIdentityCode + "</td>" +
                            "   <td>" + cityName + "</td>" +
                            "   <td>" + owneshipType + "</td>" +
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

        var myAJAX = new AJAX(url, true, params, searchCompanies_Callback);
        myAJAX.Call();
    };

    var handleKeyPressed = function(e, widgetId) {
        if (!e) e = window.event;   // resolve event instance

        if (e.keyCode == '13') {
            searchCompanies(widgetId);
            return false;
        }
        else if (e.keyCode == '27') {
            companySelector.hideDialog(widgetId);
            return false;
        }
    };

    var handleChooseCompany = function(widgetId, companyID, companyName, companyUnifiedIdentityCode, owneshipType) {
        if (callbackFunctions[widgetId]) {
            callbackFunctions[widgetId](companyID, companyName, companyUnifiedIdentityCode, owneshipType);
        }

        companySelector.hideDialog(widgetId);
    };

    function repopulateMunicipality(widgetId, regionId, ddMunicipalityId) {
        var url = "AddEditCompany.aspx?AjaxMethod=JSRepopulateMunicipality";
        var params = "";
        params += "RegionId=" + regionId;

        function RepopulateMunicipality_Callback(xml) {

            ClearSelectList(document.getElementById(ddMunicipalityId), true);

            var municipalities = xml.getElementsByTagName("m");

            for (var i = 0; i < municipalities.length; i++) {
                var id = xmlValue(municipalities[i], "id");
                var name = xmlValue(municipalities[i], "name");

                AddToSelectList(document.getElementById(ddMunicipalityId), id, name);
            };

            repopulateCity(widgetId, document.getElementById(ddMunicipalityId).value, widgetId + "_ddCity");
        }

        var myAJAX = new AJAX(url, true, params, RepopulateMunicipality_Callback);
        myAJAX.Call();
    }

    function repopulateCity(widgetId, municipalityId, ddCityId) {
        var url = "AddEditCompany.aspx?AjaxMethod=JSRepopulateCity";
        var params = "";
        params += "MunicipalityId=" + municipalityId;

        function RepopulateCity_Callback(xml) {
            ClearSelectList(document.getElementById(ddCityId), true);

            var cities = xml.getElementsByTagName("c");

            for (var i = 0; i < cities.length; i++) {
                var id = xmlValue(cities[i], "id");
                var name = xmlValue(cities[i], "name");

                AddToSelectList(document.getElementById(ddCityId), id, name);
            };

            repopulatePostCodeAndDistrict(widgetId, document.getElementById(ddCityId).value, widgetId + "_txtPostCode", widgetId + "_ddDistrict");
        }

        var myAJAX = new AJAX(url, true, params, RepopulateCity_Callback);
        myAJAX.Call();
    }

    function repopulatePostCodeAndDistrict(widgetId, cityId, txtPostCodeId, ddDistrictsId) {
        var url = "AddEditCompany.aspx?AjaxMethod=JSRepopulatePostCodeAndDistrict";
        var params = "";
        params += "CityId=" + cityId;

        function RepopulatePostCodeAndDistrict_Callback(xml) {
            var cityPostCode = xmlValue(xml, "cityPostCode");

            document.getElementById(txtPostCodeId).value = cityPostCode;

            ClearSelectList(document.getElementById(ddDistrictsId), true);

            var districts = xml.getElementsByTagName("d");

            for (var i = 0; i < districts.length; i++) {
                var id = xmlValue(districts[i], "id");
                var name = xmlValue(districts[i], "name");

                AddToSelectList(document.getElementById(ddDistrictsId), id, name);
            };

            repopulateDistrictPostCode(widgetId, document.getElementById(widgetId + "_ddDistrict").value, widgetId + "_txtPostCode");
        }

        var myAJAX = new AJAX(url, true, params, RepopulatePostCodeAndDistrict_Callback);
        myAJAX.Call();
    }

    function repopulateDistrictPostCode(widgetId, districtId, txtPostCodeId) {
        var url = "AddEditCompany.aspx?AjaxMethod=JSRepopulateDistrictPostCode";
        var params = "";
        params += "DistrictId=" + districtId;

        function RepopulateDistrictPostCode_Callback(xml) {
            var districtPostCode = xmlValue(xml, "districtPostCode");

            if (districtPostCode != "")
                document.getElementById(txtPostCodeId).value = districtPostCode;
        }

        var myAJAX = new AJAX(url, true, params, RepopulateDistrictPostCode_Callback);
        myAJAX.Call();
    }

    function repopulateRegionMunicipalityCityDistrict(widgetId, postCode, ddRegionId, ddMunicipalityId, ddCityId, ddDistrictId) {
        var url = "AddEditCompany.aspx?AjaxMethod=JSRepopulateRegionMunicipalityCityDistrict";
        var params = "";
        //params += "HdnCompanyId=" + document.getElementById(hdnCompanyIDClientID).value;
        params += "&PostCode=" + postCode;

        function RepopulateRegionMunicipalityCityDistrict_Callback(xml) {
            var cityId = parseInt(xmlValue(xml, "cityId"));

            //Not found
            if (cityId == 0) {
                /*
                document.getElementById(ddRegionId).selectedIndex = 0;

            ClearSelectList(document.getElementById(ddMunicipalityId), false);
                ClearSelectList(document.getElementById(ddCityId), false);
                ClearSelectList(document.getElementById(ddDistrictId), false);
                */
            }
            else //found
            {
                var regionId = xmlValue(xml, "regionId");
                var municipalityId = xmlValue(xml, "municipalityId");
                var districtId = xmlValue(xml, "districtId");

                document.getElementById(ddRegionId).value = regionId;

                ClearSelectList(document.getElementById(ddMunicipalityId), true);

                var municipalities = xml.getElementsByTagName("m");

                for (var i = 0; i < municipalities.length; i++) {
                    var id = xmlValue(municipalities[i], "id");
                    var name = xmlValue(municipalities[i], "name");

                    AddToSelectList(document.getElementById(ddMunicipalityId), id, name);
                };

                document.getElementById(ddMunicipalityId).value = municipalityId;


                ClearSelectList(document.getElementById(ddCityId), true);

                var cities = xml.getElementsByTagName("c");

                for (var i = 0; i < cities.length; i++) {
                    var id = xmlValue(cities[i], "id");
                    var name = xmlValue(cities[i], "name");

                    AddToSelectList(document.getElementById(ddCityId), id, name);
                };

                document.getElementById(ddCityId).value = cityId;


                ClearSelectList(document.getElementById(ddDistrictId), true);

                var districts = xml.getElementsByTagName("d");

                for (var i = 0; i < districts.length; i++) {
                    var id = xmlValue(districts[i], "id");
                    var name = xmlValue(districts[i], "name");

                    AddToSelectList(document.getElementById(ddDistrictId), id, name);
                };

                if (parseInt(districtId) > 0)
                    document.getElementById(ddDistrictId).value = districtId;
            }
        }

        var myAJAX = new AJAX(url, true, params, RepopulateRegionMunicipalityCityDistrict_Callback);
        myAJAX.Call();
    }

    function jsCheckIsBulstatAndCompanyName(widgetId) {
        var txtBulstat = document.getElementById(widgetId + "_txtBulstat");
        var txtCompanyName = document.getElementById(widgetId + "_txtCompanyName");

        var url = "CompanySelectorHandlers.aspx?AjaxMethod=JSCheckIsBulstatAndCompanyName";
        var params = "";
        params += "HdnCompanyId=0";
        params += "&Bulstat=" + custEncodeURI(txtBulstat.value);
        params += "&CompanyName=" + custEncodeURI(txtCompanyName.value);

        function IsOccupied_Callback(xml) {
            isAlreadyOccupiedCompanyName = parseInt(xmlValue(xml, "isAlreadyOccupiedCompanyName")) == 1;
            alreadyOccupiedCompanyNames[widgetId] = isAlreadyOccupiedCompanyName;

            isValidBulstat = parseInt(xmlValue(xml, "isValidBulstat")) == 1;
            validBulstats[widgetId] = isValidBulstat;

            saveData(widgetId);
        }

        var myAJAX = new AJAX(url, true, params, IsOccupied_Callback);
        myAJAX.Call();
    }

    function IsDataValid(widgetId) {
        var res = true;
        
        var lblMessage = document.getElementById(widgetId + "_lblMessage");
        lblMessage.innerHTML = "";
        var ValidationMessage = "";

        var notValidFields = new Array();

        if (document.getElementById(widgetId + "_txtBulstat").value.Trim() != "") {
            if (!validBulstats[widgetId]) {
                res = false;
                ValidationMessage += "Въведеният " + modulData.data.unifiedIdentityCodeLabelText + "/ЕГН е невалиден" + "</br>";
            }
        }

        if (document.getElementById(widgetId + "_txtCompanyName").value.Trim() == "") {
            res = false;

            if (document.getElementById(widgetId + "_txtCompanyName").disabled == true || document.getElementById(widgetId + "_txtCompanyName").style.display == "none") {
                notValidFields.push("Име");
            }
            else {
                ValidationMessage += GetErrorMessageMandatory("Име") + "</br>";
            }
        }
        else if (alreadyOccupiedCompanyNames[widgetId]) {
            res = false;
            ValidationMessage += "Вече има въведена фирма с това име" + "</br>";
        }

        var notValidFieldsCount = notValidFields.length;

        if (notValidFieldsCount > 0) {
            var noRightsMessage = GetErrorMessageNoRights(notValidFields);
            ValidationMessage += noRightsMessage + "<br />";
        }

        if (res) {
            lblMessage.className = "SuccessText";
            lblMessage.innerHTML = "";
        }
        else {
            lblMessage.className = "ErrorText";
            lblMessage.style.display = "";
            lblMessage.innerHTML = ValidationMessage;
        }

        return res;
    }

    function saveData(widgetId) {
        if (IsDataValid(widgetId)) {
            var url = "CompanySelectorHandlers.aspx?AjaxMethod=JSSaveCompany";
            var params = "";
            params += "CompanyId=0";
            params += "&Bulstat=" + document.getElementById(widgetId + "_txtBulstat").value;
            params += "&CompanyName=" + document.getElementById(widgetId + "_txtCompanyName").value;
            params += "&Phone=" + document.getElementById(widgetId + "_txtPhone").value;
            params += "&OwnershipTypeId=" + GetSelectedItemId(document.getElementById(widgetId + "_ddOwnershipType"));
            params += "&AdministrationId=" + GetSelectedItemId(document.getElementById(widgetId + "_ddAdministration"));
            params += "&CityID=" + GetSelectedItemId(document.getElementById(widgetId + "_ddCity"));
            params += "&DistrictID=" + GetSelectedItemId(document.getElementById(widgetId + "_ddDistrict"));
            params += "&SecondPostCode=" + document.getElementById(widgetId + "_txtPostCode").value;
            params += "&Address=" + document.getElementById(widgetId + "_txtaAddress").value;

            function SaveData_Callback(xml) {
                var companyID = xmlValue(xml, "companyID");
                var companyName = xmlValue(xml, "companyName");
                var companyUnifiedIdentityCode = xmlValue(xml, "companyUnifiedIdentityCode");
                var companyOwneshipType = xmlValue(xml, "companyOwneshipType");
                var message = "";
               
                document.getElementById(widgetId + "_lblMessage").className = "SuccessText";
                document.getElementById(widgetId + "_lblMessage").innerHTML = message;

                handleChooseCompany(widgetId, companyID, companyName, companyUnifiedIdentityCode, companyOwneshipType);

                //LoadOriginalValues();
            }

            var myAJAX = new AJAX(url, true, params, SaveData_Callback);
            myAJAX.Call();
        }
    }


    //Public interface of the "namespace"/"module"
    return {
        showDialog: function(widgetId, callbackFunction) {
            if (modulData.data) {
                appendHidePage(widgetId);
                createAndAppendLightbox(widgetId, callbackFunction);
            } else {
                collectModuleDataAndCreateAndAppendLightbox(widgetId, callbackFunction);
            }
        },

        hideDialog: function(widgetId) {
            removeHidePage(widgetId);
            removeLightbox(widgetId);
        },

        goTo: function(widgetId, mode) {
            var lightBox = document.getElementById(widgetId);
            var searchPnl = document.getElementById(widgetId + "_pnlSearch");
            var insertPnl = document.getElementById(widgetId + "_pnlInsert");

            if (mode == 'insert') {
                lightBox.style.height = '360px';
                searchPnl.style.display = "none";
                insertPnl.style.display = "";
            } else if (mode == 'search') {
                lightBox.style.height = '480px';
                searchPnl.style.display = "";
                insertPnl.style.display = "none";
            }
        },

        search: {
            btnSearch_Click: function(widgetId) {
                searchCompanies(widgetId);
            },

            keyPressed: function(e, widgetId) {
                handleKeyPressed(e, widgetId);
            },

            chooseCompany: function(widgetId, companyID, companyName, companyUnifiedIdentityCode, owneshipType) {
                handleChooseCompany(widgetId, companyID, companyName, companyUnifiedIdentityCode, owneshipType);
            }
        },

        insert: {
            btnInsert_Click: function(widgetId) {
                jsCheckIsBulstatAndCompanyName(widgetId);

                return false;
            },

            ddRegion_Changed: function(widgetId) {
                var ddRegion = document.getElementById(widgetId + "_ddRegion");
                repopulateMunicipality(widgetId, ddRegion.value, widgetId + "_ddMunicipality");
            },

            ddMunicipality_Changed: function(widgetId) {
                var ddPermMunicipality = document.getElementById(widgetId + "_ddMunicipality");
                repopulateCity(widgetId, ddPermMunicipality.value, widgetId + "_ddCity");
            },

            ddCity_Changed: function(widgetId) {
                var ddCity = document.getElementById(widgetId + "_ddCity");
                repopulatePostCodeAndDistrict(widgetId, ddCity.value, widgetId + "_txtPostCode", widgetId + "_ddDistrict");
            },

            ddDistrict_Changed: function(widgetId) {
                var ddPermDistrict = document.getElementById(widgetId + "_ddDistrict");
                repopulateDistrictPostCode(widgetId, ddPermDistrict.value, widgetId + "_txtPostCode");
            },

            txtPostCode_Focus: function(widgetId) {
                var txtPermPostCode = document.getElementById(widgetId + "_txtPostCode");
                txtPermPostCode.setAttribute("oldvalue", txtPermPostCode.value);
            },

            txtPostCode_Blur: function(widgetId) {
                var txtPermPostCode = document.getElementById(widgetId + "_txtPostCode");

                if (txtPermPostCode.value != txtPermPostCode.getAttribute("oldvalue")) {
                    repopulateRegionMunicipalityCityDistrict(widgetId, txtPermPostCode.value, widgetId + "_ddRegion", widgetId + "_ddMunicipality", widgetId + "_ddCity", widgetId + "_ddDistrict");
                }
            }
        }
    };
})();