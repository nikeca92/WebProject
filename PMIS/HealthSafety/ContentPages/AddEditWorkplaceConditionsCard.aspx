<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="AddEditWorkplaceConditionsCard.aspx.cs" Inherits="PMIS.HealthSafety.ContentPages.AddEditWorkplaceConditionsCard" %>

<%@ Register Assembly="MilitaryUnitSelector" TagPrefix="is" Namespace="MilitaryUnitSelector" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <style type="text/css">

    .isDivMainClass
    {
        font-family: Verdana;
        width: 160px;
    }

    .isDivMainClass input
    {
       font-family: Verdana;
       font-weight: normal;
       font-size: 11px;
       width : 150px;
    }

    </style>

    <script src="../Scripts/Ajax.js" type="text/javascript"></script>

    <div id="jsMilitaryUnitSelectorDiv" runat="server">
    </div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hfWorkplaceConditionsCardID" runat="server" />
            <asp:HiddenField ID="hfFromHome" runat="server" />
            <asp:HiddenField ID="hfMilitaryUnitLabel" runat="server" />
            <div style="height: 20px">
            </div>
            <center style="width: 100%;">
                <table style="width: 100%;">
                    <tr>
                        <td>
                            <span id="lblHeaderTitle" runat="server" class="HeaderText"></span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <center>
                                <table style="border: solid 1px #cdc9c9; width: 750px;">
                                    <colgroup width="200px">
                                    </colgroup>
                                    <colgroup width="150px">
                                    </colgroup>
                                    <colgroup width="175px">
                                    </colgroup>
                                    <colgroup width="175px">
                                    </colgroup>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblMilitaryUnit" runat="server" CssClass="InputLabel"></asp:Label>
                                        </td>
                                        <td style="text-align: left;">
                                            <is:MilitaryUnitSelector ID="musMilitaryUnit" runat="server" DataSourceWebPage="DataSource.aspx" DataSourceKey="MilitaryUnit" 
                                                 DivMainCss="isDivMainClassRequired" DivListCss="isDivListClass" DivFullListCss="isDivFullListClass" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblCardNumber" runat="server" CssClass="InputLabel" Text="Номер на карта:"></asp:Label>
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtCardNumber" runat="server" CssClass="RequiredInputField" MaxLength="250"></asp:TextBox>
                                        </td>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblCity" runat="server" CssClass="InputLabel" Text="Населено място:"></asp:Label>
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:Label ID="txtCity" runat="server" CssClass="InputField"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblJobType" runat="server" CssClass="InputLabel" Text="Работно място (вид работа):"></asp:Label>
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtJobType" runat="server" CssClass="InputField" MaxLength="250"></asp:TextBox>
                                        </td>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblWorkersCount" runat="server" CssClass="InputLabel" Text="Брой на работещите:"></asp:Label>
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtWorkersCount" runat="server" CssClass="InputField"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <center>
                                <div id="divCardItems" runat="server">
                                </div>
                                <asp:Label ID="lblTableWarning" runat="server" CssClass="TableWarningMessage" Text="Таблицата може да бъде редактирана, само след като картата е запазена!"></asp:Label>
                            </center>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <center>
                                <table style="border: solid 1px #cdc9c9; width: 750px;">
                                    <colgroup width="50px">
                                    </colgroup>
                                    <colgroup width="50px">
                                    </colgroup>
                                    <colgroup width="400px">
                                    </colgroup>
                                    <colgroup width="200px">
                                    </colgroup>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblComplexAssessment" runat="server" CssClass="InputLabel" Text="Комплексна оценка:"></asp:Label>
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtComplexAssessment" runat="server" CssClass="InputField"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblComplexAssessmentPointValue" runat="server" CssClass="InputLabel"
                                                Text="Стойност на една точка от комплексната оценка:"></asp:Label>
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtComplexAssessmentPointValue" runat="server" CssClass="InputField"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td style="text-align: right;">
                                            <asp:Label ID="lblAdditionalReward" runat="server" CssClass="InputLabel" Text="Размер на допълнителното трудово възнаграждение:"></asp:Label>
                                        </td>
                                        <td style="text-align: left;">
                                            <asp:TextBox ID="txtAdditionalReward" runat="server" CssClass="InputField"></asp:TextBox>&nbsp;<asp:Label
                                                ID="lblLeva" runat="server" CssClass="InputLabel" Text="лева"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </td>
                    </tr>
                    <tr style="height: 18px;">
                        <td>
                            <asp:HiddenField ID="hfMsg" runat="server" />
                            <asp:Label ID="lblMessage" runat="server" Text="">&nbsp;</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:LinkButton ID="btnSave" runat="server" CssClass="Button" OnClientClick="return ValidateData();"
                                OnClick="btnSave_Click"><i></i><div style="width:70px; padding-left:5px;">Запис</div><b></b></asp:LinkButton>
                            <asp:LinkButton ID="btnPrintWorkplaceConditionsCard" runat="server" CssClass="Button"
                                OnClientClick="ShowPrintWorkplaceConditionsCard(); return false;"><i></i><div style="width:70px; padding-left:5px;">Печат</div><b></b></asp:LinkButton>
                            <span style="margin-left: 50px;">
                                <asp:LinkButton ID="btnBack" runat="server" CssClass="Button" OnClick="btnBack_Click"
                                    CheckForChanges="true"><i></i><div style="width:70px; padding-left:5px;">Назад</div><b></b></asp:LinkButton>
                            </span>
                        </td>
                    </tr>
                </table>
            </center>
            <div id="CardItemIndicatorTypeLightBox" class="CardItemIndicatorTypeLightBox" style="display: none;
                text-align: center;">
                <asp:HiddenField ID="hfCardItemIndicatorTypeID" runat="server" />
                <center>
                    <table width="80%" style="text-align: center;">
                        <colgroup style="width: 50%">
                        </colgroup>
                        <colgroup style="width: 50%">
                        </colgroup>
                        <tr style="height: 15px">
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <span class="HeaderText" style="text-align: center;">Елемент</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <span id="lblName" class="HeaderText2" style="text-align: center;">&nbsp;</span>
                            </td>
                        </tr>
                        <tr style="height: 15px">
                        </tr>
                        <tr style="min-height: 17px">
                            <td style="text-align: right;">
                                <span id="lblValue" class="InputLabel">Стойност:</span>
                            </td>
                            <td style="text-align: left;">
                                <input id="txtValue" type="text" class="InputField" style="width: 50px" />
                            </td>
                        </tr>
                        <tr style="min-height: 17px">
                            <td style="text-align: right;">
                                <span id="lblRate" class="InputLabel">Степен:</span>
                            </td>
                            <td style="text-align: left;">
                                <input id="txtRate" type="text" class="InputField" style="width: 50px;" />
                            </td>
                        </tr>
                        <tr style="min-height: 17px">
                            <td style="text-align: right;">
                                <span id="lblAssessment" class="InputLabel">Оценка:</span>
                            </td>
                            <td style="text-align: left;">
                                <input id="txtAssessment" type="text" class="InputField" style="width: 50px;" />
                            </td>
                        </tr>
                        <tr style="height: 30px">
                            <td colspan="2">
                                <span id="spanIndicatorTypeLightBoxMessage" class="ErrorText" style="display: none;">
                                </span>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center;">
                                <table style="margin: 0 auto;">
                                    <tr>
                                        <td>
                                            <div id="btnSaveIndicatorTypeLightBox" style="display: inline;" onclick="SaveCardItemIndicatorType();"
                                                class="Button">
                                                <i></i>
                                                <div id="btnSaveIndicatorTypeLightBoxText" style="width: 70px;">
                                                    Запис</div>
                                                <b></b>
                                            </div>
                                            <div id="btnCloseIndicatorTypeLightBox" style="display: inline;" onclick="HideCardItemIndicatorTypeLightBox();"
                                                class="Button">
                                                <i></i>
                                                <div id="btnCloseIndicatorTypeLightBoxText" style="width: 70px;">
                                                    Затвори</div>
                                                <b></b>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
            <div id="CardItemIndicatorLightBox" class="CardItemIndicatorLightBox" style="display: none;
                text-align: center;">
                <asp:HiddenField ID="hfCardItemIndicatorID" runat="server" />
                <asp:HiddenField ID="hfIndicatorTypeID" runat="server" />
                <center>
                    <table width="80%" style="text-align: center;">
                        <colgroup style="width: 50%">
                        </colgroup>
                        <colgroup style="width: 50%">
                        </colgroup>
                        <tr style="height: 15px">
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <span class="HeaderText" style="text-align: center;">Елемент</span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <span id="lblName2" class="HeaderText2" style="text-align: center;">&nbsp;</span>
                            </td>
                        </tr>
                        <tr style="height: 15px">
                        </tr>
                        <tr style="min-height: 17px">
                            <td style="text-align: right;">
                                <span id="lblIndicator" class="InputLabel">Показател:</span>
                            </td>
                            <td style="text-align: left;">
                                <div id="divDropDownIndicator">
                                </div>
                                <span id="txtIndicator" class="InputField">&nbsp;</span>
                            </td>
                        </tr>
                        <tr style="min-height: 17px">
                            <td style="text-align: right;">
                                <span id="lblValue2" class="InputLabel">Стойност:</span>
                            </td>
                            <td style="text-align: left;">
                                <input id="txtValue2" type="text" class="InputField" style="width: 50px" />
                            </td>
                        </tr>
                        <tr style="min-height: 17px">
                            <td style="text-align: right;">
                                <span id="lblRate2" class="InputLabel">Степен:</span>
                            </td>
                            <td style="text-align: left;">
                                <input id="txtRate2" type="text" class="InputField" style="width: 50px;" />
                            </td>
                        </tr>
                        <tr style="min-height: 17px">
                            <td style="text-align: right;">
                                <span id="lblAssessment2" class="InputLabel">Оценка:</span>
                            </td>
                            <td style="text-align: left;">
                                <input id="txtAssessment2" type="text" class="InputField" style="width: 50px;" />
                            </td>
                        </tr>
                        <tr style="height: 30px">
                            <td colspan="2">
                                <span id="spanIndicatorLightBoxMessage" class="ErrorText" style="display: none;">
                                </span>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center;">
                                <table style="margin: 0 auto;">
                                    <tr>
                                        <td>
                                            <div id="btnSaveIndicatorLightBox" style="display: inline;" onclick="SaveCardItemIndicator();"
                                                class="Button">
                                                <i></i>
                                                <div id="btnSaveIndicatorLightBoxText" style="width: 70px;">
                                                    Запис</div>
                                                <b></b>
                                            </div>
                                            <div id="btnCloseIndicatorLightBox" style="display: inline;" onclick="HideCardItemIndicatorLightBox();"
                                                class="Button">
                                                <i></i>
                                                <div id="btnCloseIndicatorLightBoxText" style="width: 70px;">
                                                    Затвори</div>
                                                <b></b>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </center>
            </div>
            <asp:Button ID="hdnBtnRefreshTable" runat="server" OnClick="btnHdnRefreshCardItems_Click"
                CssClass="HiddenButton" />
            <asp:HiddenField ID="hdnLocationHash" runat="server" />
            <asp:HiddenField ID="hdnSavedChanges" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        window.addEventListener ? window.addEventListener('load', PageLoad, false) : window.attachEvent('onload', PageLoad);

        function PageLoad()
        {
            hdnSavedChangesID = '<%= hdnSavedChanges.ClientID %>';
        }

        var initCardItemIndicatorTypeDialogHeight = 0;

        // Display light box with indicator type card item's properties (for editing or adding new)
        function ShowCardItemIndicatorTypeLightBox(cardItemID)
        {
            var url = "AddEditWorkplaceConditionsCard.aspx?AjaxMethod=JSGetCardItem";

            var params = "CardItemID=" + cardItemID;

            function response_handler(xml)
            {
                document.getElementById("lblName").innerHTML = xmlValue(xml, "Name");
                document.getElementById("txtValue").value = xmlValue(xml, "Value");
                document.getElementById("txtRate").value = xmlValue(xml, "Rate");
                document.getElementById("txtAssessment").value = xmlValue(xml, "Assessment");
            }

            var myAJAX = new AJAX(url, true, params, response_handler);
            myAJAX.Call();

            document.getElementById("<%= hfCardItemIndicatorTypeID.ClientID %>").value = cardItemID; // setting card item ID(0 - if new protocol item)

            // clean message label in the light box and hide it            
            document.getElementById("spanIndicatorTypeLightBoxMessage").style.display = "none";
            document.getElementById("spanIndicatorTypeLightBoxMessage").innerHTML = "";

            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("CardItemIndicatorTypeLightBox").style.display = "";
            CenterLightBox("CardItemIndicatorTypeLightBox");

            initCardItemIndicatorTypeDialogHeight = document.getElementById("CardItemIndicatorTypeLightBox").offsetHeight;
        }

        // Close the light box and refresh card items table
        function HideCardItemIndicatorTypeLightBox()
        {
            document.getElementById("<%=hdnBtnRefreshTable.ClientID %>").click();
            document.getElementById("HidePage").style.display = "none";
            document.getElementById("CardItemIndicatorTypeLightBox").style.display = "none";
        }

        // Validate indicator type card item properties in the light box and generates appropriate error messages, if needed
        function ValidateCardItemIndicatorType()
        {
            var res = true;
            var lightBox = document.getElementById('CardItemIndicatorTypeLightBox');
            var lightBoxMessage = document.getElementById("spanIndicatorTypeLightBoxMessage");
            lightBoxMessage.innerHTML = "";

            var notValidFields = new Array();

            var value = document.getElementById("txtValue");
            var rate = document.getElementById("txtRate");
            var assessment = document.getElementById("txtAssessment");

            if (TrimString(value.value) != "" && !isDecimal(TrimString(value.value)))
            {
                res = false;

                lightBoxMessage.innerHTML += GetErrorMessageNumber("Стойност") + "<br />";
            }

            if (TrimString(rate.value) != "" && !isDecimal(TrimString(rate.value)))
            {
                res = false;

                lightBoxMessage.innerHTML += GetErrorMessageNumber("Степен") + "<br />";
            }

            if (TrimString(assessment.value) != "" && !isDecimal(TrimString(assessment.value)))
            {
                res = false;

                lightBoxMessage.innerHTML += GetErrorMessageNumber("Оценка") + "<br />";
            }

            return res;
        }

        // Saves indicator type card item through ajax request, if light box values are valid, or displays generated error messages
        function SaveCardItemIndicatorType()
        {
            if (ValidateCardItemIndicatorType())
            {
                var url = "AddEditWorkplaceConditionsCard.aspx?AjaxMethod=JSSaveCardIndicatorTypeItem";

                var params = "CardID=" + document.getElementById("<%= hfWorkplaceConditionsCardID.ClientID %>").value +
                       "&CardItemID=" + document.getElementById("<%= hfCardItemIndicatorTypeID.ClientID %>").value +
                       "&Value=" + TrimString(document.getElementById("txtValue").value) +
                       "&Rate=" + TrimString(document.getElementById("txtRate").value) +
                       "&Assessment=" + TrimString(document.getElementById("txtAssessment").value);

                function response_handler(xml)
                {
                    var hideDialog = true;
                    var resultMsg = xmlValue(xml, "response");
                    if (resultMsg != "OK" && resultMsg != "ERROR")
                    {
                        var lightBoxMessage = document.getElementById("spanIndicatorTypeLightBoxMessage");
                        lightBoxMessage.innerHTML = "";
                        lightBoxMessage.style.display = "";
                        hideDialog = false;
                        lightBoxMessage.innerHTML = resultMsg;
                    }
                    else if (resultMsg != "OK")
                        document.getElementById("<%= hfMsg.ClientID %>").value = "FailCardIndicatorTypeItemSave";
                    else
                        document.getElementById("<%= hfMsg.ClientID %>").value = "SuccessCardIndicatorTypeItemSave";

                    if (hideDialog)
                        HideCardItemIndicatorTypeLightBox();
                }

                var myAJAX = new AJAX(url, true, params, response_handler);
                myAJAX.Call();
            }
            else
            {
                document.getElementById("spanIndicatorTypeLightBoxMessage").style.display = "";
            }
        }

        // Delete card item through ajax request
        function DeleteCardItem(cardItemID)
        {
            YesNoDialog('Желаете ли да изтриете показателя?', ConfirmYes, null);

            function ConfirmYes()
            {
                var url = "AddEditWorkplaceConditionsCard.aspx?AjaxMethod=JSDeleteCardItem&WorkplaceConditionsCardId=" + document.getElementById("<%= hfWorkplaceConditionsCardID.ClientID %>").value;
                var params = "";
                params += "CardItemID=" + cardItemID;
                
                function response_handler(xml)
                {
                    if (xmlValue(xml, "response") != "OK")
                        document.getElementById("<%= hfMsg.ClientID %>").value = "FailCardItemDelete";
                    else
                    {
                        document.getElementById("<%= hfMsg.ClientID %>").value = "SuccessCardItemDelete";
                        document.getElementById("<%=hdnBtnRefreshTable.ClientID %>").click();
                    }
                }

                var myAJAX = new AJAX(url, true, params, response_handler);
                myAJAX.Call();
            }
        }

        var initCardItemIndicatorDialogHeight = 0;

        // Display light box with indicator card items properties (for editing or adding new)
        function ShowCardItemIndicatorLightBox(cardItemID, indicatorTypeName, indicatorTypeID)
        {
            document.getElementById("<%= hfIndicatorTypeID.ClientID %>").value = indicatorTypeID;

            var url = "AddEditWorkplaceConditionsCard.aspx?AjaxMethod=JSGetIndicators";

            var params = "IndicatorTypeID=" + indicatorTypeID +
                     "&CardItemID=" + cardItemID;

            function drop_down_response_handler(xml)
            {
                document.getElementById("divDropDownIndicator").innerHTML = xmlValue(xml, "result");

                CheckDisabledClientControls();
                CheckHiddenClientControls();

                if (cardItemID != 0)
                {
                    var url = "AddEditWorkplaceConditionsCard.aspx?AjaxMethod=JSGetCardItem";

                    var params = "CardItemID=" + cardItemID;

                    function response_handler(xml)
                    {
                        var ddIndicator = document.getElementById("ddIndicator");
                        var txtIndicator = document.getElementById("txtIndicator");

                        document.getElementById("lblName2").innerHTML = xmlValue(xml, "Name");
                        ddIndicator.value = xmlValue(xml, "IndicatorID");
                        document.getElementById("txtValue2").value = xmlValue(xml, "Value");
                        document.getElementById("txtRate2").value = xmlValue(xml, "Rate");
                        document.getElementById("txtAssessment2").value = xmlValue(xml, "Assessment");

                        txtIndicator.innerHTML = ddIndicator.options[ddIndicator.selectedIndex].text;
                        txtIndicator.style.display = "";
                        ddIndicator.style.display = "none";
                    }

                    var myAJAX = new AJAX(url, true, params, response_handler);
                    myAJAX.Call();
                }
                else
                {
                    var ddIndicator = document.getElementById("ddIndicator");
                    var txtIndicator = document.getElementById("txtIndicator");

                    document.getElementById("lblName2").innerHTML = indicatorTypeName;
                    document.getElementById("txtValue2").value = "";
                    document.getElementById("txtRate2").value = "";
                    document.getElementById("txtAssessment2").value = "";

                    txtIndicator.style.display = "none";
                    ddIndicator.style.display = "";
                }

                document.getElementById("<%= hfCardItemIndicatorID.ClientID %>").value = cardItemID; // setting card item ID(0 - if new protocol item)

                // clean message label in the light box and hide it            
                document.getElementById("spanIndicatorLightBoxMessage").style.display = "none";
                document.getElementById("spanIndicatorLightBoxMessage").innerHTML = "";

                //shows the light box and "disable" rest of the page
                document.getElementById("HidePage").style.display = "";
                document.getElementById("CardItemIndicatorLightBox").style.display = "";
                CenterLightBox("CardItemIndicatorLightBox");

                initCardItemIndicatorDialogHeight = document.getElementById("CardItemIndicatorLightBox").offsetHeight;

            }

            var myAJAX = new AJAX(url, true, params, drop_down_response_handler);
            myAJAX.Call();
        }

        // Close the light box and refresh card items table
        function HideCardItemIndicatorLightBox()
        {
            document.getElementById("<%=hdnBtnRefreshTable.ClientID %>").click();
            document.getElementById("HidePage").style.display = "none";
            document.getElementById("CardItemIndicatorLightBox").style.display = "none";
        }

        // Validate indicator card item properties in the light box and generates appropriate error messages, if needed
        function ValidateCardItemIndicator()
        {
            var res = true;
            var lightBox = document.getElementById('CardItemIndicatorLightBox');
            var lightBoxMessage = document.getElementById("spanIndicatorLightBoxMessage");
            lightBoxMessage.innerHTML = "";

            var notValidFields = new Array();

            var indicatorID = document.getElementById("ddIndicator");
            var value = document.getElementById("txtValue2");
            var rate = document.getElementById("txtRate2");
            var assessment = document.getElementById("txtAssessment2");

            if (TrimString(indicatorID.value) == "" || indicatorID.value == "-1")
            {
                res = false;

                if (indicatorID.disabled == true || indicatorID.style.display == "none")
                    notValidFields.push("Показател");
                else
                    lightBoxMessage.innerHTML += GetErrorMessageMandatory("Показател") + "<br />";
            }

            if (TrimString(value.value) != "" && !isDecimal(TrimString(value.value)))
            {
                res = false;

                lightBoxMessage.innerHTML += GetErrorMessageNumber("Стойност") + "<br />";
            }

            if (TrimString(rate.value) != "" && !isDecimal(TrimString(rate.value)))
            {
                res = false;

                lightBoxMessage.innerHTML += GetErrorMessageNumber("Степен") + "<br />";
            }

            if (TrimString(assessment.value) != "" && !isDecimal(TrimString(assessment.value)))
            {
                res = false;

                lightBoxMessage.innerHTML += GetErrorMessageNumber("Оценка") + "<br />";
            }

            var notValidFieldsCount = notValidFields.length;

            if (notValidFieldsCount > 0)
            {
                var noRightsMessage = GetErrorMessageNoRights(notValidFields);
                lightBoxMessage.innerHTML += "<br />" + noRightsMessage;
            }

            return res;
        }

        // Saves indicator card item through ajax request, if light box values are valid, or displays generated error messages
        function SaveCardItemIndicator()
        {
            if (ValidateCardItemIndicator())
            {
                var url = "AddEditWorkplaceConditionsCard.aspx?AjaxMethod=JSSaveCardIndicatorItem";

                var params = "CardID=" + document.getElementById("<%= hfWorkplaceConditionsCardID.ClientID %>").value +
                       "&CardItemID=" + document.getElementById("<%= hfCardItemIndicatorID.ClientID %>").value +
                       "&IndicatorTypeID=" + document.getElementById("<%= hfIndicatorTypeID.ClientID %>").value +
                       "&IndicatorTypeName=" + document.getElementById("lblName2").innerHTML +
                       "&IndicatorID=" + document.getElementById("ddIndicator").value +
                       "&Value=" + TrimString(document.getElementById("txtValue2").value) +
                       "&Rate=" + TrimString(document.getElementById("txtRate2").value) +
                       "&Assessment=" + TrimString(document.getElementById("txtAssessment2").value);

                function response_handler(xml)
                {
                    var hideDialog = true;
                    var resultMsg = xmlValue(xml, "response");
                    if (resultMsg != "OK" && resultMsg != "ERROR")
                    {
                        var lightBoxMessage = document.getElementById("spanIndicatorLightBoxMessage");
                        lightBoxMessage.innerHTML = "";
                        lightBoxMessage.style.display = "";
                        hideDialog = false;
                        lightBoxMessage.innerHTML = resultMsg;
                    }
                    else if (resultMsg != "OK")
                        document.getElementById("<%= hfMsg.ClientID %>").value = "FailCardIndicatorItemSave";
                    else
                        document.getElementById("<%= hfMsg.ClientID %>").value = "SuccessCardIndicatorItemSave";

                    if (hideDialog)
                        HideCardItemIndicatorTypeLightBox();
                }

                var myAJAX = new AJAX(url, true, params, response_handler);
                myAJAX.Call();
            }
            else
            {
                document.getElementById("spanIndicatorLightBoxMessage").style.display = "";
            }
        }

        // Validate card properties on the page and generates appropriate error messages, if needed
        function ValidateData()
        {
            var res = true;
            var lblMessage = document.getElementById("<%= lblMessage.ClientID %>");
            lblMessage.innerHTML = "";

            var notValidFields = new Array();

            var militaryUnitId = MilitaryUnitSelectorUtil.GetSelectedValue("<%= musMilitaryUnit.ClientID %>");
            var cardNumber = document.getElementById("<%= txtCardNumber.ClientID %>");
            var workersCount = document.getElementById("<%= txtWorkersCount.ClientID %>");
            var complexAssessment = document.getElementById("<%= txtComplexAssessment.ClientID %>");
            var complexAssessmentPointValue = document.getElementById("<%= txtComplexAssessmentPointValue.ClientID %>");
            var additionalReward = document.getElementById("<%= txtAdditionalReward.ClientID %>");

            if (TrimString(cardNumber.value) == "")
            {
                res = false;

                if (cardNumber.disabled == true || cardNumber.style.display == "none")
                    notValidFields.push("Номер на карта");
                else
                    lblMessage.innerHTML += GetErrorMessageMandatory("Номер на карта") + "<br />";
            }

            if (militaryUnitId == "-1" || militaryUnitId == "")
            {
                res = false;

                var militaryUnitLabel = document.getElementById("<%= hfMilitaryUnitLabel.ClientID %>").value;

                if (MilitaryUnitSelectorUtil.IsDisabled("<%= musMilitaryUnit.ClientID %>") ||
                    MilitaryUnitSelectorUtil.IsHidden("<%= musMilitaryUnit.ClientID %>"))
                    notValidFields.push(militaryUnitLabel);
                else
                    lblMessage.innerHTML += GetErrorMessageMandatory(militaryUnitLabel) + "<br />";
            }

            if (TrimString(workersCount.value) != "" && !isInt(workersCount.value))
            {
                res = false;

                lblMessage.innerHTML += GetErrorMessageNumber("Брой на работещите") + "<br />";
            }

            if (TrimString(complexAssessment.value) != "" && !isDecimal(complexAssessment.value))
            {
                res = false;

                lblMessage.innerHTML += GetErrorMessageNumber("Комплексна оценка") + "<br />";
            }

            if (TrimString(complexAssessmentPointValue.value) != "" && !isDecimal(complexAssessmentPointValue.value))
            {
                res = false;

                lblMessage.innerHTML += GetErrorMessageNumber("Стойност на една точка от комплексната оценка") + "<br />";
            }

            if (TrimString(additionalReward.value) != "" && !isDecimal(additionalReward.value))
            {
                res = false;

                lblMessage.innerHTML += GetErrorMessageNumber("Размер на допълнителното трудово възнаграждение") + "<br />";
            }

            var notValidFieldsCount = notValidFields.length;
            var fieldsStr = '"' + notValidFields.join(", ") + '"';

            if (notValidFieldsCount > 0)
            {
                var noRightsMessage = GetErrorMessageNoRights(notValidFields);
                lblMessage.innerHTML += "<br />" + noRightsMessage;
            }

            if (!res)
                lblMessage.className = "ErrorText";

            if (res)
                ForceNoChanges();

            return res;
        }

        function ShowPrintWorkplaceConditionsCard()
        {
            var hfWorkplaceConditionsCardId = document.getElementById("<%= hfWorkplaceConditionsCardID.ClientID %>").value;

            var url = "";
            var pageName = "PrintWorkplaceConditionsCard"
            var param = "";

            url = "../PrintContentPages/" + pageName + ".aspx?WorkplaceConditionsCardID=" + hfWorkplaceConditionsCardId;

            var uplPopup = window.open(url, pageName, param);

            if (uplPopup != null)
                uplPopup.focus();
        }
   
    </script>

</asp:Content>
