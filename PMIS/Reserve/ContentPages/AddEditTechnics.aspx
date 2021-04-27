<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="AddEditTechnics.aspx.cs" Inherits="PMIS.Reserve.ContentPages.AddEditTechnics" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .EditTechTabsBottomLine
        {
            background-color: #000055;
            height: 3px;
            width: 695px;
            padding: 0px;
            margin: 0px;
        }
        .lboxAddEditMilRepStatus
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
         .lboxTechnicsAppointmentHistory
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
        .ChangeVehicleRegNumberLightBox
        {
	        width: 450px;
	        background-color: #EEEEEE;
	        border: solid 1px #000000;
	        position: fixed;
	        top: 200px;
	        left: 30%;	
	        min-height: 160px;
	        z-index: 1000;
	        padding-top: 10px;
        }
        .ChangeTrailerRegNumberLightBox
        {
	        width: 450px;
	        background-color: #EEEEEE;
	        border: solid 1px #000000;
	        position: fixed;
	        top: 200px;
	        left: 30%;	
	        min-height: 160px;
	        z-index: 1000;
	        padding-top: 10px;
        }
        .lboxRegNumberHistory
        {
	        width: 450px;
	        background-color: #EEEEEE;
	        border: solid 1px #000000;
	        position: fixed;
	        top: 200px;
	        left: 30%;	
	        min-height: 160px;
	        z-index: 1000;
	        padding-top: 10px;
        }
        .ChangeRailwayEquipInvNumberLightBox
        {
	        width: 450px;
	        background-color: #EEEEEE;
	        border: solid 1px #000000;
	        position: fixed;
	        top: 200px;
	        left: 30%;	
	        min-height: 160px;
	        z-index: 1000;
	        padding-top: 10px;
        }
        .ChangeVesselInvNumberLightBox
        {
	        width: 450px;
	        background-color: #EEEEEE;
	        border: solid 1px #000000;
	        position: fixed;
	        top: 200px;
	        left: 30%;	
	        min-height: 160px;
	        z-index: 1000;
	        padding-top: 10px;
        }
        .ChangeFuelContainerInvNumberLightBox
        {
	        width: 450px;
	        background-color: #EEEEEE;
	        border: solid 1px #000000;
	        position: fixed;
	        top: 200px;
	        left: 30%;	
	        min-height: 160px;
	        z-index: 1000;
	        padding-top: 10px;
        }
        .lboxInvNumberHistory
        {
	        width: 450px;
	        background-color: #EEEEEE;
	        border: solid 1px #000000;
	        position: fixed;
	        top: 200px;
	        left: 30%;	
	        min-height: 160px;
	        z-index: 1000;
	        padding-top: 10px;
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
        .lboxVesselCrew
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
    </style>
    <div id="jsMilitaryUnitSelectorDiv" runat="server">
    </div>
    <div id="jsItemSelectorDiv" runat="server">
    </div>

    <script src="../Scripts/Ajax.js" type="text/javascript"></script>
    <script src="../Scripts/PickList.js" type="text/javascript"></script>

    <script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Scripts/AddEditTechnics.js'></script>
    <script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Scripts/AddEditTechnics_BasicInfo.js'></script>
    <script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Scripts/AddEditTechnics_<%= TechnicsTypeKey %>.js'></script>
    <script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Scripts/AddEditTechnics_MilitaryReport.js'></script>
    <script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Scripts/AddEditTechnics_Owner.js'></script>
    <script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Scripts/AddEditTechnics_OtherInfo.js'></script>
    
    <script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Scripts/CompanySelector.js'></script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <center>
                <div id="contentDiv" style="display: none; min-height: 230px;">
                    <div style="margin-left: 30px;">
                        <div>
                            <div style="text-align: center; position: relative; top: 10px;">
                               <span id="lblHeaderTitle" runat="server" class="HeaderText"></span>
                            </div>
                           <%-- <div style="text-align: right; padding-right: 45px; padding-bottom: 2px; position: relative; top: 5px;">
                                <span id="lblCurrMilDepartment" class="InputLabel">Военно окръжие:</span>
                                <span id="lblCurrMilDepartmentValue" class="ReadOnlyValue"><%= PMIS.Reserve.Common.MilitaryReportStatusUtil.GetLabelWhenLackOfStatus() %></span>
                            </div>--%>
                        </div>
                        <div id="pnlGeneralTechnicsInfo" runat="server" style="margin-top: 15px;">
                        </div>
                    </div>
                    <table style="width: 960px;" cellspacing="0" cellpadding="0">
                        <tr>
                            <td align="left" style="width: 100%">
                                <div id="TabSummary">
                                    <ul>
                                        <% if (IsBasicInfoVisible())
                                           { %>
                                        <li class="ActiveTab" id="btnTabBasicInfo" onclick="TabClick(this);" onmouseover="TabHover(this);"
                                            onmouseout="TabOut(this);" isalrеadyvisited="true"><a href="#" onclick="return false;"
                                                style="width: 110px; text-align: center">Основни данни</a></li>
                                        <%  } %>
                                        <% if (IsMilitaryReportVisible())
                                           { %>
                                        <li class="InactiveTab" id="btnTabMilitaryReport" onclick="TabClick(this);" onmouseover="TabHover(this);"
                                            onmouseout="TabOut(this);"><a href="#" onclick="return false;" style="width: 120px;
                                                text-align: center">Военно-отчетни</a></li>
                                        <%  } %>
                                        <% if (IsOwnerVisible())
                                           { %>
                                        <li class="InactiveTab" id="btnTabOwner" onclick="TabClick(this);" onmouseover="TabHover(this);"
                                            onmouseout="TabOut(this);"><a href="#" onclick="return false;" style="width: 130px;
                                                text-align: center"><%= TabOwnerTitle %></a></li>
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
                                <% if (IsBasicInfoVisible() || IsMilitaryReportVisible() || IsOwnerVisible() || IsOtherInfoVisible())
                                   { %>
                                <div class="EditTechTabsBottomLine" />
                                <% } %>
                            </td>
                        </tr>
                    </table>
                    <div style="border: solid 1px #888888;">
                        <div id="divBasicInfo" style="display: none; min-height: 180px; padding-left: 30px;
                            text-align: left;" runat="server">
                        </div>
                        <div id="divMilitaryReport" style="display: none; min-height: 180px; padding-left: 30px;
                            text-align: left;" runat="server">
                        </div>
                        <div id="divOwner" style="display: none; min-height: 180px; padding-left: 30px;
                            text-align: left;" runat="server">
                        </div>
                        <div id="divOtherInfo" style="display: none; min-height: 180px; padding-left: 30px;
                            text-align: left;" runat="server">
                        </div>
                    </div>
                </div>
                <div style="height: 10px;">
                </div>
                <div id="loadingDiv" class="LoadingDiv" style="padding-top: 60px; padding-bottom: 60px;">
                    <img src="../Images/ajax-loader-big.gif" alt="Зареждане" title="Зареждане" />
                </div>
                <div style=""text-align: center;  padding-right: 30px;"">
                   <table style=""margin: 0 auto;"">
                      <tr>
                        <td>
                            <span id="lblAllTabsMessage"></span>
                        </td>
                      </tr>
                    </table>
                </div>
                <div>
                    <table>
                        <tr align="center">
                            <td>
                                <div id="btnSaveAllTabs" style="display: inline;" onclick="SaveAllTechicsTabs();" class="Button">
                                     <i></i>
                                     <div id="btnSaveAllTabsText" style="width: 70px;">
                                         Запис</div>
                                     <b></b>
                                </div>
                             </td>
                            <td>
                                <asp:LinkButton ID="btnBack" runat="server" CssClass="Button" OnClick="btnBack_Click"
                                    CheckForChanges="true"><i></i><div style="width:70px; padding-left:5px;">Назад</div><b></b></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </div>
            </center>
            <asp:HiddenField runat="server" ID="hdnTechnicsId" Value="0" />
            <asp:HiddenField runat="server" ID="hdnTechnicsTypeKey" Value="" />
            <asp:HiddenField runat="server" ID="hdnTechnicsTypeName" Value="" />
            <asp:HiddenField runat="server" ID="hdnIsPreview" Value="0" />
            
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
        var isLoadedBasicInfo = false;
        var isContentDisplayed = false;

        var hdnTechnicsIdClientID = "<%= hdnTechnicsId.ClientID %>";
        var hdnTechnicsTypeKeyClientID = "<%= hdnTechnicsTypeKey.ClientID %>";
        var lblHeaderTitleClientID = "<%= lblHeaderTitle.ClientID %>";
        var hdnTechnicsTypeNameClientID = "<%= hdnTechnicsTypeName.ClientID %>";
        var hdnIsPreviewClientID = "<%= hdnIsPreview.ClientID %>";
        
        window.addEventListener ? window.addEventListener('load', PageLoad, false) : window.attachEvent('onload', PageLoad);

        //Call this when the page is loaded
        function PageLoad()
        {
            var modeNew = false;

            if (document.getElementById(hdnTechnicsIdClientID).value == "0")
                modeNew = true;                

            if (document.getElementById("btnTabBasicInfo") != null)
            {
                JSLoadTab("btnTabBasicInfo");
            }
            else if (document.getElementById("btnTabMilitaryReport") != null && !modeNew)
            {
                JSLoadTab("btnTabMilitaryReport");
                document.getElementById("btnTabMilitaryReport").className = "ActiveTab";
            }
            else if (document.getElementById("btnTabOwner") != null && !modeNew)
            {
                JSLoadTab("btnTabOwner");
                document.getElementById("btnTabOwner").className = "ActiveTab";
            }
            else if (document.getElementById("btnTabOtherInfo") != null && !modeNew)
            {
                JSLoadTab("btnTabOtherInfo");
                document.getElementById("btnTabOtherInfo").className = "ActiveTab";
            }
            else //Nothing to load; then directly show content
            {
                TechnicsInitialLoad();

                isLoadedTheInitialTab = true;
                ShowContent();
            }
        }

        //Tabs logic
        //This is the ID of the selected tab (its button)
        var selectedTab = "btnTabOtherInfo";

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
        function IsSelectedTabAlreadyVisited()
        {
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
            document.getElementById("<%=divBasicInfo.ClientID %>").style.display = "none";
            document.getElementById("<%=divMilitaryReport.ClientID %>").style.display = "none";
            document.getElementById("<%=divOwner.ClientID %>").style.display = "none";
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
            var url = "AddEditTechnics.aspx?AjaxMethod=JSLoadTab&TechnicsId=" + document.getElementById(hdnTechnicsIdClientID).value +
                      "&TechnicsTypeKey=" + document.getElementById(hdnTechnicsTypeKeyClientID).value;
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

            if (selectedTabId == "btnTabBasicInfo")
            {
                TabLoaded_BasicInfo();

                if (document.getElementById("btnImgCopyOwnerAddress"))
                {
                    if (document.getElementById(hdnTechnicsIdClientID).value == "0")
                        document.getElementById("btnImgCopyOwnerAddress").style.display = "none";
                    else
                        document.getElementById("btnImgCopyOwnerAddress").style.display = "";
                }
            }

            if (selectedTabId == "btnTabOtherInfo")
                TabLoaded_OtherInfo();

            if (selectedTab == "btnTabOwner")
                TabLoaded_Owner();

            RefreshDatePickers();

            AppendNewInputs();

            //Show the loaded content
            ShowDiv(selectedTabId);

            if (!isLoadedTheInitialTab)
            {
                TechnicsInitialLoad();

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
                case "btnTabBasicInfo":
                    {
                        targetDivId = "<%=divBasicInfo.ClientID %>";
                        break;
                    }
                case "btnTabMilitaryReport":
                    {
                        targetDivId = "<%=divMilitaryReport.ClientID %>";
                        break;
                    }
                case "btnTabOwner":
                    {
                        targetDivId = "<%=divOwner.ClientID %>";
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
            var selectedTab = "btnTabBasicInfo";

            if (document.getElementById("btnTabBasicInfo") != null)
                selectedTab = "btnTabBasicInfo";
            else if (document.getElementById("btnTabMilitaryReport") != null)
                selectedTab = "btnTabMilitaryReport";
            else if (document.getElementById("btnTabOwner") != null)
                selectedTab = "btnTabOwner";
            else if (document.getElementById("btnTabOtherInfo") != null)
                selectedTab = "btnTabOtherInfo";
            else
                return;

            var tab = document.getElementById(selectedTab);

            tab.className = "ActiveTab";
            selectedTab = tab.id;
            JSLoadTab(selectedTab);
        }

        var divBasicInfoClientID = "<%=divBasicInfo.ClientID %>";
        var divMilitaryReportClientID = "<%=divMilitaryReport.ClientID %>";
        var divOwnerClientID = "<%=divOwner.ClientID %>";
        var divOtherInfoClientID = "<%=divOtherInfo.ClientID %>";
    </script>

</asp:Content>
