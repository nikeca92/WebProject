<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="AddEditReservist.aspx.cs" Inherits="PMIS.Reserve.ContentPages.AddEditReservist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .EditResTabsBottomLine
        {
            background-color: #000055;
            height: 3px;
            width: 695px;
            padding: 0px;
            margin: 0px;
        }
        .btnNewTableRecord
        {
            position: relative;
            top: -23px;
            text-align: right;
        }
        .btnNewTableRecordIcon
        {
            height: 18px;
            width: 18px;
            cursor: pointer;
        }
        .lboxCivilEducation
        {
            width: 600px;
            background-color: #EEEEEE;
            border: solid 1px #000000;
            position: fixed;
            top: 120px;
            left: 25%;
            min-height: 270px;
            z-index: 1000;
            padding-top: 10px;
        }
        .lboxPersonSpeciality
        {
            width: 600px;
            background-color: #EEEEEE;
            border: solid 1px #000000;
            position: fixed;
            top: 20px;
            left: 23%;
            min-height: 230px;
            z-index: 1000;
            padding-top: 10px;
        }	        
        .lboxMilitaryEducation
        {
            width: 600px;
            background-color: #EEEEEE;
            border: solid 1px #000000;
            position: fixed;
            top: 120px;
            left: 25%;
            min-height: 270px;
            z-index: 1000;
            padding-top: 10px;
        }
        .lboxAddEditMilRepStatus
        {
            width: 950px;
            background-color: #EEEEEE;
            border: solid 1px #000000;
            position: fixed;
            top: 120px;
            left: 25%;
            min-height: 270px;
            max-height: 400px;            
            z-index: 1000;
            padding-top: 10px;
            padding-left: 20px;
            padding-right: 20px;
            overflow: auto;
        }
        .lboxMilRepStatusHistory
        {
            width: 800px;
            background-color: #EEEEEE;
            border: solid 1px #000000;
            position: fixed;
            top: 120px;
            left: 25%;
            min-height: 270px;
            z-index: 1000;
            padding-top: 10px;
        }
        .lboxReservistAppointmentHistory
        {
            width: 950px;
            background-color: #EEEEEE;
            border: solid 1px #000000;
            position: fixed;
            top: 120px;
            left: 25%;
            min-height: 270px;
            z-index: 1000;
            padding-top: 10px;
        }
        .lboxMilRepSpec
        {
            width: 600px;
            background-color: #EEEEEE;
            border: solid 1px #000000;
            position: fixed;
            top: 120px;
            left: 25%;
            min-height: 180px;
            z-index: 1000;
            padding-top: 10px;
        }
        .lboxPositionTitle
        {
            width: 600px;
            background-color: #EEEEEE;
            border: solid 1px #000000;
            position: fixed;
            top: 120px;
            left: 25%;
            min-height: 180px;
            z-index: 1000;
            padding-top: 10px;
        }
        .lboxRecordOfServiceArchive
        {
            width: 530px;
            background-color: #EEEEEE;
            border: solid 1px #000000;
            position: fixed;
            top: 120px;
            left: 25%;
            min-height: 270px;
            z-index: 1000;
            padding-top: 10px;
        }
        .lboxConviction
        {
            width: 630px;
            background-color: #EEEEEE;
            border: solid 1px #000000;
            position: fixed;
            top: 120px;
            left: 25%;
            min-height: 270px;
            z-index: 1000;
            padding-top: 10px;
        }
        .lboxDualCitizenship
        {
            width: 530px;
            background-color: #EEEEEE;
            border: solid 1px #000000;
            position: fixed;
            top: 120px;
            left: 25%;
            min-height: 200px;
            z-index: 1000;
            padding-top: 10px;
        }
        .lboxPreviousPosition
        {
            width: 720px;
            background-color: #EEEEEE;
            border: solid 1px #000000;
            position: fixed;
            top: 20px;
            left: 23%;
            min-height: 270px;
            z-index: 1000;
            padding-top: 10px;
        }
        .lboxTransferToVitosha
        {
            width: 500px;
            background-color: #EEEEEE;
            border: solid 1px #000000;
            position: fixed;
            top: 20px;
            left: 23%;
            min-height: 100px;
            z-index: 1000;
            padding-top: 10px; 
        }
        
        .lboxPersonDischarge
        {
            width: 850px;
            background-color: #EEEEEE;
            border: solid 1px #000000;
            position: fixed;
            top: 20px;
            left: 23%;
            min-height: 270px;
            z-index: 1000;
            padding-top: 10px;
        }
        .lboxSearchNKPD
        {
            width: 800px;
            background-color: #EEEEEE;
            border: solid 1px #000000;
            position: fixed;
            top: 120px;
            left: 25%;
            height: 470px;
            z-index: 1001;
            padding-top: 0px;
            padding-left: 15px;
        }
        .isDivListClass
        {
           font-family: Verdana;
           font-size: 11px;
           font-weight: normal;
           overflow-x: visible;
           overflow-y: scroll;
           position: absolute;
           z-index: 1000;
           border: solid 1px #AAAAAA;
           background-color: #FFFFFF;
           width : 260px;
        }
        
        .isDivMainClassRequired
        {
            font-family: Verdana;
            width: 300px;
        }

        .isDivMainClassRequired input
        {
           font-family: Verdana;
           font-weight: normal;
           font-size: 11px;
           width : 260px;
           background-color: #FFFFCC;
           position: relative;
           left: -3px;
           top: -2px;
        }
        
        .lboxSavingData
        {
            width: 600px;
            background-color: #FFFFFF;
            border: solid 1px #000000;
            position: fixed;
            height: 200px;
            z-index: 1001;
            padding-top: 0px;
            padding-left: 15px;
        }
        
        .lboxMedCert
        {
            width: 600px;
            background-color: #EEEEEE;
            border: solid 1px #000000;
            position: fixed;
            top: 120px;
            left: 25%;
            min-height: 180px;
            z-index: 1000;
            padding-top: 10px;
        }
        
        .lboxPsychCert
        {
            width: 600px;
            background-color: #EEEEEE;
            border: solid 1px #000000;
            position: fixed;
            top: 120px;
            left: 25%;
            min-height: 180px;
            z-index: 1000;
            padding-top: 10px;
        }

    </style>
    
    <div id="jsMilitaryUnitSelectorDiv" runat="server">
    </div>
    
    <div id="jsItemSelectorDiv" runat="server">
    </div>

    <script src="../Scripts/Ajax.js" type="text/javascript"></script>

    <script src="../Scripts/PickList.js" type="text/javascript"></script>

    <script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Scripts/AddEditReservist.js'></script>

    <script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Scripts/AddEditReservist_PersonalData.js'></script>

    <script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Scripts/AddEditReservist_EducationWork.js'></script>

    <script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Scripts/AddEditReservist_MilitaryReport.js'></script>

    <script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Scripts/AddEditReservist_MilitaryService.js'></script>

    <script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Scripts/AddEditReservist_OtherInfo.js'></script>
    
    <script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Scripts/CompanySelector.js'></script>
    
    <script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Scripts/DischargeReasonSelector.js'></script>
    
    <script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Scripts/CivilEducationSelector.js'></script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <center>
                <div id="contentDiv" style="display: none; min-height: 230px;">
                    <div style="margin-left: 30px;">
                        <div align="left">
                            <table style="width: 900px;" align="left">
                                <tr>
                                    <td align="center">
                                        <span id="lblHeaderTitle" runat="server" class="HeaderText"></span>
                                    </td>
                                </tr>
                                <tr style="height: 10px; vertical-align: middle">
                                    <td align="center">
                                        <div id="lblHeaderSubTitle" runat="server" class="HeaderText" style="padding-right: 100px;
                                            font-size: 1.2em; padding-left: 210px; text-align: justify">
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div style="position: relative">
                                <div style="text-align: left; padding-left: 5px; padding-bottom: 2px; width: 275px; position: absolute; left: 0; bottom: 0">
                                    <span id="lblCurrentMilitaryUnit" class="InputLabel"><%= PMIS.Common.CommonFunctions.GetLabelText("MilitaryUnit") %>:</span>
                                    <asp:Label ID="lblCurrentMilitaryUnitValue" CssClass="ReadOnlyValue" runat="server"></asp:Label>
                                </div>
                                <div style="text-align: left; padding-left: 5px; padding-bottom: 2px; width: 200px; margin: 0 auto">
                                    <span id="lblPersonStatus" class="InputLabel">Статус:</span>
                                    <span id="lblPersonStatusValue" class="ReadOnlyValue"></span>
                                </div>
                                <div style="text-align: right; padding-right: 45px; padding-bottom: 2px; width: 400px; position: absolute; right: 0; bottom: 0">
                                    <span id="lblCurrMilDepartment" class="InputLabel">На отчет в:</span>
                                    <span id="lblCurrMilDepartmentValue" class="ReadOnlyValue"><%= PMIS.Reserve.Common.MilitaryReportStatusUtil.GetLabelWhenLackOfStatus() %></span>
                                </div>
                            </div>
                            <div style="text-align: center; clear: both;">
                                <fieldset style="width: 880px; padding: 0px;">
                                    <table id="tblPersonalDataHeader" class="InputRegion" style="width: 880px; padding: 10px; padding-top: 0px;
                                        margin-top: 0px;">
                                        <tr style="height: 3px;">
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                <span id="lblIdentNumber" class="InputLabel">ЕГН:</span>
                                            </td>
                                            <td style="text-align: left;">
                                                <input type="text" id="txtIdentNumber" class="RequiredInputField" style="width: 100px; display: none;"
                                                    maxlength="10" onfocus="IdentNumberFocus();" onblur="IdentNumberBlur();" />
                                                <span id="lblIdentNumberValue" class="ReadOnlyValue" style="display: none;"></span>
                                            </td>
                                            <td style="text-align: right;">
                                                <span id="lblMilitaryRank" class="InputLabel">Звание:</span>
                                            </td>
                                            <td style="text-align: left;">
                                                <span id="lblMilitaryRankValue" class="ReadOnlyValue"></span>
                                                <div style="display:inline-block; vertical-align:bottom;"><input type="checkbox" id="chkMilitaryRankDR" disabled="disabled" style="margin-bottom:0px;position: relative;bittom:0px;vertical-align: bottom;"/><span id="lblMilitaryRankDR" class="ReadOnlyValue">ДР</span></div>
                                                <img id="imgEditMilitaryRank" style="vertical-align:bottom;" alt="Редактиране на Звание" title="Званието може да се редактира след като резервистът бъде добавен." src="../Images/list_edit_disabled.png" />
                                                
                                                
                                                
                                                <input type="hidden" id="hdnMilitaryRankID" value="" />
                                            </td>
                                            <td style="text-align: right;">
                                                <span id="lblMilitaryCategory" class="InputLabel">Категория:</span>
                                            </td>
                                            <td style="text-align: left;">
                                                <span id="lblMilitaryCategoryValue" class="ReadOnlyValue"></span>
                                            </td>
                                            <td style="width: 100px;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right;">
                                                <span id="lblFirstName" class="InputLabel">Име и презиме:</span>
                                            </td>
                                            <td style="text-align: left;">
                                                <input type="text" id="txtFirstName" class="RequiredInputField" style="width: 170px;" maxlength="35" onblur="PopulateInitials();" />
                                            </td>
                                            <td style="text-align: right;">
                                                <span id="lblLastName" class="InputLabel">Фамилия:</span>
                                            </td>
                                            <td style="text-align: left;">
                                                <input type="text" id="txtLastName" class="RequiredInputField" style="width: 115px;" maxlength="30" onblur="PopulateInitials();" />
                                            </td>
                                            <td style="text-align: right;">
                                                <span id="lblInitials" class="InputLabel">Инициали:</span>
                                            </td>
                                            <td style="text-align: left;">
                                                <span id="txtInitials" class="ReadOnlyValue" style="width: 60px; display: block;"></span>                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: left;" colspan="2">
                                                <span id="lblMilitaryReportStatus" class="InputLabel">Текущо състояние по отчета:</span>
                                                <span id="lblMilitaryReportStatusValue" class="ReadOnlyValue"><%= PMIS.Reserve.Common.MilitaryReportStatusUtil.GetLabelWhenLackOfStatus() %></span>
                                            </td>
                                            <td style="text-align: right;">
                                                <span id="lblLastModified" class="InputLabel">Последна сверка:</span>
                                            </td>
                                            <td style="text-align: left;">
                                                <span id="lblLastModifiedValue" class="ReadOnlyValue"></span>
                                            </td>
                                            <td style="text-align: right;">
                                                <span id="lblCurrentVos" class="InputLabel">ВОС:</span>
                                            </td>
                                            <td style="text-align: left;">
                                                <asp:Label ID="lblCurrentVosValue" CssClass="ReadOnlyValue" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5" style="text-align: right; vertical-align: top;">
                                                <span id="lblCurrentPositionTitle" class="InputLabel">Подходяща длъжност:</span>
                                            </td>
                                            <td colspan="2" style="text-align: left;">
                                                <asp:Label ID="lblCurrentPositionTitleValue" CssClass="ReadOnlyValue" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </fieldset>
                            </div>
                        </div>
                    </div>
                    <table style="width: 960px;" cellspacing="0" cellpadding="0">
                        <tr>
                            <td align="left" style="width: 100%">
                                <div id="TabSummary">
                                    <ul>
                                        <% if (IsPersonalDataVisible())
                                           { %>
                                        <li class="ActiveTab" id="btnTabPersonalData" onclick="TabClick(this);" onmouseover="TabHover(this);"
                                            onmouseout="TabOut(this);" isalrеadyvisited="true"><a href="#" onclick="return false;"
                                                style="width: 110px; text-align: center">Лични данни</a></li>
                                        <%  } %>
                                        <% if (IsEducationWorkVisible())
                                           { %>
                                        <li class="InactiveTab" id="btnTabEducationWork" onclick="TabClick(this);" onmouseover="TabHover(this);"
                                            onmouseout="TabOut(this);"><a href="#" onclick="return false;" style="width: 140px;
                                                text-align: center">Образование/работа</a></li>
                                        <%  } %>
                                        <% if (IsMilitaryReportVisible())
                                           { %>
                                        <li class="InactiveTab" id="btnTabMilitaryReport" onclick="TabClick(this);" onmouseover="TabHover(this);"
                                            onmouseout="TabOut(this);"><a href="#" onclick="return false;" style="width: 120px;
                                                text-align: center">Военно-отчетни</a></li>
                                        <%  } %>
                                        <% if (IsMilitaryServiceVisible())
                                           { %>
                                        <li class="InactiveTab" id="btnTabMilitaryService" onclick="TabClick(this);" onmouseover="TabHover(this);"
                                            onmouseout="TabOut(this);"><a href="#" onclick="return false;" style="width: 120px;
                                                text-align: center">Военна служба</a></li>
                                        <%  } %>
                                        <% if (IsOtherInfoVisible())
                                           { %>
                                        <li class="InactiveTab" id="btnTabOtherInfo" onclick="TabClick(this);" onmouseover="TabHover(this);"
                                            onmouseout="TabOut(this);"><a href="#" onclick="return false;" style="width: 120px;
                                                text-align: center">Други данни</a></li>
                                        <%  } %>
                                    </ul>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <% if (IsPersonalDataVisible() || IsEducationWorkVisible() || IsMilitaryReportVisible() || IsMilitaryServiceVisible() || IsOtherInfoVisible())
                                   { %>
                                <div class="EditResTabsBottomLine" />
                                <% } %>
                            </td>
                        </tr>
                    </table>
                    <div style="border: solid 1px #888888;">
                        <div id="divPersonalData" style="display: none; min-height: 180px; padding-left: 30px;
                            text-align: left;" runat="server">
                        </div>
                        <div id="divEducationWork" style="display: none; min-height: 180px; padding-left: 8px;"
                            runat="server">
                        </div>
                        <div id="divMilitaryReport" style="display: none; min-height: 180px; padding-left: 30px;
                            text-align: left;" runat="server">
                        </div>
                        <div id="divMilitaryService" style="display: none; min-height: 180px;" runat="server">
                        </div>
                        <div id="divOtherInfo" style="display: none; min-height: 180px;" runat="server">
                        </div>
                    </div>
                   
                    <div style="text-align: center;">
                       <table style="margin: 0 auto;">
                          <tr>
                            <td>
                                <span id="lblGeneralTabMesage"></span>
                            </td>
                          </tr>
                        </table><br />
                       <table style="margin: 0 auto;">
                          <tr>
                             <td>
                                <div id="btnSaveAllTabs" style="display: inline;" onclick="SaveAllReservistTabs();"
                                     class="Button">
                                     <i></i>
                                     <div id="btnSaveAllTabsText" style="width: 70px;">
                                         Запис</div>
                                     <b></b>
                                </div>
                                <div id="btnBack" style="display: inline;" onclick="GoBack();" class="Button">
                                     <i></i>
                                     <div id="btnBackText" style="width: 70px;">
                                         Назад</div>
                                     <b></b>
                                </div>
                             </td>
                          </tr>
                       </table>
                    </div>                    
                 
                </div>
                <div style="height: 10px;">
                </div>
                <div id="loadingDiv" class="LoadingDiv" style="padding-top: 60px; padding-bottom: 60px;">
                    <img src="../Images/ajax-loader-big.gif" alt="Зареждане" title="Зареждане" />
                </div>               
            </center>
            <div id="lboxMilitaryRank" style="display: none;" class="lboxCivilEducation" runat="server"></div>
            <asp:HiddenField runat="server" ID="hdnReservistId" Value="0" />
            <asp:HiddenField runat="server" ID="hdnPersonId" Value="0" />
            
            <asp:HiddenField runat="server" ID="hdnIsPreview" Value="0" />
            
            <asp:HiddenField runat="server" ID="hdnIsRecordOfServiceArchiveHidden" Value="0" />  
            <asp:HiddenField runat="server" ID="hdnIsRecordOfServiceArchiveDisabled" Value="0" />
                                              
            <asp:HiddenField runat="server" ID="hdnIsConvictionHidden" Value="0" />
            <asp:HiddenField runat="server" ID="hdnIsDualCitizenshipHidden" Value="0" />
            
            <div id="lboxSavingData" style="display: none;" class="lboxSavingData">
                <div style="margin-top: 50px; text-align: center;">
                     <!-- use a secong gif loader image because it looks like IE is not animating it if it is already loaded on the page -->
                    <img src="../Images/ajax-loader-big-2.gif" alt="Записване" title="Записване" />
                </div>
                <div style="font-size: 1.3em; font-weight: bold; margin-top: 30px; text-align: center;">Запазване на данните...</div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        var isLoadedTheInitialTab = false;
        var isLoadedPersonalData = false;
        var isContentDisplayed = false;

        var hdnReservistIdClientID = "<%= hdnReservistId.ClientID %>";
        var hdnPersonIdClientID = "<%= hdnPersonId.ClientID %>";
        var lblHeaderTitleClientID = "<%= lblHeaderTitle.ClientID %>";
        //var ddMilitaryRankClientID = " ddMilitaryRank.ClientID";
        var hdnIsPreviewClientID = "<%= hdnIsPreview.ClientID %>";
        
        var hdnIsRecordOfServiceArchiveHiddenClientID = "<%= hdnIsRecordOfServiceArchiveHidden.ClientID %>";
        var hdnIsRecordOfServiceArchiveDisabledClientID = "<%= hdnIsRecordOfServiceArchiveDisabled.ClientID %>";        
            
        var hdnIsConvictionHiddenClientID = "<%= hdnIsConvictionHidden.ClientID %>";
        var hdnIsDualCitizenshipHiddenClientID = "<%= hdnIsDualCitizenshipHidden.ClientID %>";
        
        var lblCurrentMilitaryUnitValueClientID = "<%= lblCurrentMilitaryUnitValue.ClientID %>";

        var lblCurrentVosValueClientID = "<%= lblCurrentVosValue.ClientID %>";
        var lblCurrentPositionTitleValueClientID = "<%= lblCurrentPositionTitleValue.ClientID %>";
        var lboxMilitaryRankID = "<%= lboxMilitaryRank.ClientID %>";

        var statusReserveLbl = '<%= PMIS.Common.Config.GetWebSetting("Status_Reserve_Label") %>';
        var statusRemovedLbl = '<%= PMIS.Common.Config.GetWebSetting("Status_Removed_Label") %>';
        var statusNALbl = '<%= PMIS.Common.Config.GetWebSetting("Status_NA_Label") %>';    
                
        window.addEventListener ? window.addEventListener('load', PageLoad, false) : window.attachEvent('onload', PageLoad);

        //Call this when the page is loaded
        function PageLoad()
        {
            var modeNew = false;

            if (document.getElementById(hdnReservistIdClientID).value == "0")
                modeNew = true;

            if (document.getElementById("btnTabPersonalData") != null)
            {
                JSLoadTab("btnTabPersonalData");
            }
            else if (document.getElementById("btnTabEducationWork") != null && !modeNew)
            {
                JSLoadTab("btnTabEducationWork");
                document.getElementById("btnTabEducationWork").className = "ActiveTab";
            }
            else if (document.getElementById("btnTabMilitaryReport") != null && !modeNew)
            {
                JSLoadTab("btnTabMilitaryReport");
                document.getElementById("btnTabMilitaryReport").className = "ActiveTab";
            }
            else if (document.getElementById("btnTabMilitaryService") != null && !modeNew)
            {
                JSLoadTab("btnTabMilitaryService");
                document.getElementById("btnTabMilitaryService").className = "ActiveTab";
            }
            else if (document.getElementById("btnTabOtherInfo") != null && !modeNew)
            {
                JSLoadTab("btnTabOtherInfo");
                document.getElementById("btnTabOtherInfo").className = "ActiveTab";
            }
            else //Nothing to load; then directly show content
            {
                ReservistInitialLoad();

                isLoadedTheInitialTab = true;
                ShowContent();
            }
        }

        //Tabs logic
        //This is the ID of the selected tab (its button)
        var selectedTab = "btnTabPersonalData";

        //Call this when hovering over a particualr tab button
        function TabHover(tab)
        {
            if (tab.id != selectedTab)
            {
                tab.className = "HoverTab";
            }
        }

        //Call this when leaving a particualr tab button
        function TabOut(tab)
        {
            if (tab.id != selectedTab)
            {
                tab.className = "InactiveTab";
            }
        }

        //Call this when a particular tab is clicked
        function TabClick(tab)
        {
            ClearAllMessages();

            //Set the previously selected tab as inactive
            if (document.getElementById(selectedTab) != null)
                document.getElementById(selectedTab).className = "InactiveTab";

            //Set the current tab as active
            tab.className = "ActiveTab";
            selectedTab = tab.id;

            //Check if this tab has been already loaded
            //If it hasn't been loaded yet then get its content from the server via AJAX
            if (!IsSelectedTabAlreadyVisited())
            {
                JSLoadTab(selectedTab);
            }
            else //If the tab has been already loaded then just display its content
            {
                ShowDiv(selectedTab);
            }
        }

        //Check if a particular tab has been visited
        //Store this information as an attribute of the tab button
        function IsSelectedTabAlreadyVisited() {
            if (document.getElementById(selectedTab).getAttribute("isalrеadyvisited") &&
                document.getElementById(selectedTab).getAttribute("isalrеadyvisited") == "true")
                return true;
            else
                return false;
        }

        function IsTabAlreadyVisited(tabID) {
            if (document.getElementById(tabID) &&
                document.getElementById(tabID).getAttribute("isalrеadyvisited") &&
                document.getElementById(tabID).getAttribute("isalrеadyvisited") == "true")
                return true;
            else
                return false;
        }
        

        //Show the content of the currently selected tab
        function ShowDiv(tab)
        {
            selectedTab = tab;

            //Hide all divs
            document.getElementById("<%=divPersonalData.ClientID %>").style.display = "none";
            document.getElementById("<%=divEducationWork.ClientID %>").style.display = "none";
            document.getElementById("<%=divMilitaryReport.ClientID %>").style.display = "none";
            document.getElementById("<%=divMilitaryService.ClientID %>").style.display = "none";
            document.getElementById("<%=divOtherInfo.ClientID %>").style.display = "none";

            //Display the content of the current tab
            var targetDivId = GetTargetDivByTabId(selectedTab);
            document.getElementById(targetDivId).style.display = "";

            //Mark it as visited
            document.getElementById(selectedTab).setAttribute("isalrеadyvisited", "true");
        }

        //Load the content of a particular tab via an AJAX call
        function JSLoadTab(selectedTabId)
        {
            var url = "AddEditReservist.aspx?AjaxMethod=JSLoadTab&ReservistId=" + document.getElementById(hdnReservistIdClientID).value;
            var params = "";
            params += "SelectedTabId=" + selectedTabId;

            var hdnIsPreview = document.getElementById(hdnIsPreviewClientID).value;
            if (hdnIsPreview == 1)
                params += "&Preview=1";
            
            var myAJAX = new AJAX(url, true, params, function(xml) { JSLoadTab_CallBack(xml, selectedTabId); });
            myAJAX.Call();
        }

        //When the response is ready then get the loaded HTML and put it on the target div
        function JSLoadTab_CallBack(xml, selectedTabId)
        {
            var targetDivId = GetTargetDivByTabId(selectedTabId);
            document.getElementById(targetDivId).innerHTML = xmlValue(xml, "TabHTML");

            //Setup the UIItems logic on the loaded tab
            var UIItems = xml.getElementsByTagName("UIItems");
           
            if (UIItems.length > 0)
            {
                var disabledClientControls = xmlValue(UIItems[0], "disabledClientControls");
                var hiddenClientControls = xmlValue(UIItems[0], "hiddenClientControls");
               
                if (disabledClientControls != "")
                {
                    document.getElementById(hdnDisabledClientControls).value += (document.getElementById(hdnDisabledClientControls).value == "" ? "" : ",") + disabledClientControls;
                    CheckDisabledClientControls();
                }
                if (hiddenClientControls != "")
                {
                    document.getElementById(hdnHiddenClientControls).value += (document.getElementById(hdnHiddenClientControls).value == "" ? "" : ",") + hiddenClientControls;
                    CheckHiddenClientControls();
                }
            }

            if (selectedTabId == "btnTabPersonalData")
                TabLoaded_PersonalData();

            if (selectedTabId == "btnTabOtherInfo")
                TabLoaded_OtherInfo();
            
            RefreshDatePickers();

            AppendNewInputs();

            //Show the loaded content
            ShowDiv(selectedTabId);

            if (!isLoadedTheInitialTab)
            {
                ReservistInitialLoad();

                isLoadedTheInitialTab = true;
                ShowContent();
            }
        }

        //Use this function to get the client id of the content div for a particular tab
        function GetTargetDivByTabId(selectedTabId)
        {
            var targetDivId = "";

            switch (selectedTabId)
            {
                case "btnTabPersonalData":
                    {
                        targetDivId = "<%=divPersonalData.ClientID %>";
                        break;
                    }
                case "btnTabEducationWork":
                    {
                        targetDivId = "<%=divEducationWork.ClientID %>";
                        break;
                    }
                case "btnTabMilitaryReport":
                    {
                        targetDivId = "<%=divMilitaryReport.ClientID %>";
                        break;
                    }
                case "btnTabMilitaryService":
                    {
                        targetDivId = "<%=divMilitaryService.ClientID %>";
                        break;
                    }
                case "btnTabOtherInfo":
                    {
                        targetDivId = "<%=divOtherInfo.ClientID %>";
                        break;
                    }
            }

            return targetDivId;
        }

        //Reset the tab selection to the first (the default) tab
        function ResetTabs()
        {
            var selectedTab = "btnTabPersonalData";

            if (document.getElementById("btnTabPersonalData") != null)
                selectedTab = "btnTabPersonalData";
            else if (document.getElementById("btnTabEducationWork") != null)
                selectedTab = "btnTabEducationWork";
            else if (document.getElementById("btnTabMilitaryReport") != null)
                selectedTab = "btnTabMilitaryReport";
            else if (document.getElementById("btnTabMilitaryService") != null)
                selectedTab = "btnTabMilitaryService";
            else if (document.getElementById("btnTabOtherInfo") != null)
                selectedTab = "btnTabOtherInfo";
            else
                return;

            var tab = document.getElementById(selectedTab);

            tab.className = "ActiveTab";
            selectedTab = tab.id;
            JSLoadTab(selectedTab);
        }

        var divPersonalDataClientID = "<%=divPersonalData.ClientID %>";
        var divEducationWorkClientID = "<%=divEducationWork.ClientID %>";
        var divMilitaryReportClientID = "<%=divMilitaryReport.ClientID %>";
        var divMilitaryServiceClientID = "<%=divMilitaryService.ClientID %>";
        var divOtherInfoClientID = "<%=divOtherInfo.ClientID %>";

        var unifiedIdentityCodeLabel = '<%= PMIS.Common.CommonFunctions.GetLabelText("UnifiedIdentityCode")%>';
        
    </script>

</asp:Content>
