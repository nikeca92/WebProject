<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="EditApplicant.aspx.cs" Inherits="PMIS.Applicants.ContentPages.EditApplicant"
    Title="Регистриране на кандидат за военна служба" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .lboxStatus
        {
            width: 650px;
            background-color: #EEEEEE;
            border: solid 1px #000000;
            position: fixed;
            top: 120px;
            left: 25%;
            min-height: 150px;
            z-index: 1000;
            padding-top: 10px;
        }
    </style>
    <div id="jsMilitaryUnitSelectorDiv" runat="server">
    </div>
    
    <div id="jsItemSelectorDiv" runat="server">
    </div>
    
    <script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Scripts/CivilEducationSelector.js'></script>    
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div id="contentDiv" style="display: none; min-height: 230px;">
                <div style="height: 20px">
                </div>
                <center>
                    <div style="margin-left: 30px;">
                        <div align="left">
                            <table style="width: 900px;" align="left">
                                <tr>
                                    <td align="center">
                                        <span id="lblHeaderTitle" runat="server" class="HeaderText">Регистриране на кандидат
                                            за военна служба</span>
                                    </td>
                                </tr>
                                <tr style="height: 10px; vertical-align: middle">
                                    <td align="center">
                                        <div id="lblHeaderSubTitle" runat="server" class="HeaderText" style="padding-right: 100px;
                                            font-size: 1.2em; padding-left: 210px; text-align: justify">
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left;">
                                        <table style="width: 800px;">
                                            <tr>
                                                <td style="text-align: left;">
                                                    <span class="InputLabel">Място на регистрация: <span runat="server" id="spanMilitaryDepartmentName"
                                                        class="ReadOnlyValue"></span></span>
                                                </td>
                                                <td style="text-align: right;">
                                                    <span class="InputLabel">Статус: <span id="spanPersonStatus" class="ReadOnlyValue"></span>
                                                    </span>
                                                    <img src="../Images/user_view.png" style="cursor: pointer;" onclick="ShowStatusLightBox();"
                                                        title="Детайли" />
                                                </td>
                                            </tr>
                                        </table>
                                        <div id="lboxStatus" style="display: none;" class="lboxStatus">
                                            <center>
                                                <div id="lboxStatus_Msg">
                                                </div>
                                                <table style="position: relative; bottom: 10px; width: 100%; margin-top: 20px;">
                                                    <tr>
                                                        <td style="text-align: center;">
                                                            <div onclick="HideStatusLightBox();" class="Button">
                                                                <i></i>
                                                                <div style="width: 70px;">
                                                                    OK</div>
                                                                <b></b>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </center>
                                        </div>
                </center>
                </td> </tr> </table>
                <fieldset style="width: 876px; padding-bottom: 0px; margin-left: 5px;">
                    <legend style="color: #0B4489; font-weight: bold; font-size: 1.25em;"></legend>
                    <table style="width: 876px;">
                        <tr>
                            <td>
                                <span class="InputLabel" id="lblFirstName">Трите имена:</span> <span class="ReadOnlyValue"
                                    id="lblFirstNameValue"></span><span class="ReadOnlyValue" id="lblMiddleNameValue">
                                </span><span class="ReadOnlyValue" id="lblLastNameValue"></span>
                            </td>
                            <td align="right">
                                <div id="divEdit" runat="server">
                                    <asp:LinkButton ID="btnEdit" runat="server" CssClass="Button" OnClick="btnEdit_Click"
                                        CheckForChanges="true"><i></i><div style="width:190px; padding-left:5px;">Редактиране на лични данни</div><b></b></asp:LinkButton>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" colspan="2">
                                <span class="InputLabel" id="lblIdentNumber">ЕГН:</span> <span class="ReadOnlyValue"
                                    id="lblIdentNumberValue"></span><span class="InputLabel" style="padding-left: 10px"
                                        id="lblGender">Пол:</span> <span class="ReadOnlyValue" id="lblGenderValue">
                                </span><span class="InputLabel" id="lblLastModified" style="padding-left: 10px">Последна
                                    актуализация:</span> <span class="ReadOnlyValue" id="lblLastModifiedValue"></span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <span class="InputLabel" id="lblAge">Възраст:</span> <span class="ReadOnlyValue"
                                    id="lblAgeValue"></span>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <table>
                    <tr>
                        <td>
                            <fieldset style="width: 430px; padding-bottom: 0px;">
                                <legend style="color: #0B4489; font-weight: bold; font-size: 1.25em;"></legend>
                                <table style="width: 430px;">
                                    <tr>
                                        <td align="left">
                                            <div style="min-height: 5px">
                                            </div>
                                            <span id="lblPermAddresTitle" class="SmallHeaderText">Постоянен адрес</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <span id="lblPermPostCode" class="InputLabel">Пощенски код:</span> <span id="txtPermPostCode"
                                                class="ReadOnlyValue"></span><span id="lblPermCity" class="InputLabel" style="padding-left: 10px">
                                                    Населено място:</span> <span id="txtPermCity" class="ReadOnlyValue"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <span id="lblPermMunicipality" class="InputLabel">Община:</span> <span id="txtPermMunicipality"
                                                class="ReadOnlyValue"></span><span id="lblPermRegion" class="InputLabel" style="padding-left: 10px">
                                                    Област:</span> <span id="txtPermRegion" class="ReadOnlyValue"></span><span id="lblPermDistrict"
                                                        class="InputLabel" style="padding-left: 10px">Район:</span> <span id="txtPermDistrict"
                                                            class="ReadOnlyValue"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <span id="lblPermAddress" style="text-align: inherit; vertical-align: top" class="InputLabel">
                                                Адрес:</span> <span id="txtPermAddress" class="ReadOnlyValue"></span>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                        <td>
                            <fieldset style="width: 426px; padding-bottom: 0px;">
                                <legend style="color: #0B4489; font-weight: bold; font-size: 1.25em;"></legend>
                                <table style="width: 425px;">
                                    <tr>
                                        <td align="left">
                                            <div style="min-height: 5px">
                                            </div>
                                            <span id="lblPresAddressTitle" class="SmallHeaderText">Настоящ адрес</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <span id="lblPresPostCode" class="InputLabel">Пощенски код:</span> <span id="txtPresPostCode"
                                                class="ReadOnlyValue"></span><span id="lblPresCity" class="InputLabel" style="padding-left: 10px">
                                                    Населено място:</span> <span id="txtPresCity" class="ReadOnlyValue"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <span id="lblPresMunicipality" class="InputLabel">Община:</span> <span id="txtPresMunicipility"
                                                class="ReadOnlyValue"></span><span id="lblPresRegion" class="InputLabel" style="padding-left: 10px">
                                                    Област:</span> <span id="txtPresRegion" class="ReadOnlyValue"></span><span id="lblPresDistrict"
                                                        class="InputLabel" style="padding-left: 10px">Район:</span> <span id="txtPresDistrict"
                                                            class="ReadOnlyValue"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <span id="lblPresAddress" class="InputLabel">Адрес:</span> <span id="txtPresAddress"
                                                class="ReadOnlyValue"></span>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr style="vertical-align: top;">
                        <td>
                            <fieldset style="width: 430px; height: 106px; padding-bottom: 0px;">
                                <legend style="color: #0B4489; font-weight: bold; font-size: 1.25em;"></legend>
                                <table style="width: 430px;">
                                    <tr>
                                        <td align="left">
                                            <div style="min-height: 5px">
                                            </div>
                                            <span id="lblContactAddressTitle" class="SmallHeaderText">Адрес за кореспонденция</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <span id="lblContactPostCode" class="InputLabel">Пощенски код:</span> <span id="txtContactPostCode"
                                                class="ReadOnlyValue"></span><span id="lblContactCity" class="InputLabel" style="padding-left: 10px">
                                                    Населено място:</span> <span id="txtContactCity" class="ReadOnlyValue"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <span id="lblContactMunicipality" class="InputLabel">Община:</span> <span id="txtContactMunicipality"
                                                class="ReadOnlyValue"></span><span id="lblContactRegion" class="InputLabel" style="padding-left: 10px">
                                                    Област:</span> <span id="txtContactRegion" class="ReadOnlyValue"></span><span id="lblContactDistrict"
                                                        class="InputLabel" style="padding-left: 10px">Район:</span> <span id="txtContactDistrict"
                                                            class="ReadOnlyValue"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <span id="lblContactAddress" style="text-align: inherit; vertical-align: top" class="InputLabel">
                                                Адрес:</span> <span id="txtContactAddress" class="ReadOnlyValue"></span>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                        <td>
                            <fieldset style="width: 426px; padding-bottom: 0px;">
                                <legend style="color: #0B4489; font-weight: bold; font-size: 1.25em;"></legend>
                                <table style="width: 425px;">
                                    <tr>
                                        <td align="left">
                                            <div style="min-height: 5px">
                                            </div>
                                            <span id="lblIDCardNumber" class="InputLabel">Лична карта номер:</span> <span id="txtIDCardNumber" class="ReadOnlyValue"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <span id="lblIDCardIssuedBy" class="InputLabel">издадена от:</span> <span id="txtIDCardIssuedBy" class="ReadOnlyValue"></span>
                                            <span id="lblIDCardIssueDate" class="InputLabel" style="padding-left: 10px">на:</span> <span id="txtIDCardIssueDate" class="ReadOnlyValue"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <span id="lblHomePhone" class="InputLabel">Домашен телефон:</span> <span id="txtHomePhone"
                                                class="ReadOnlyValue"></span><span id="lblMobilePhone" class="InputLabel" style="padding-left: 10px">
                                                    Мобилен телефон:</span> <span id="txtMobilePhone" class="ReadOnlyValue">
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <span id="lblEmail" class="InputLabel">E-mail:</span> <span id="txtEmail" class="ReadOnlyValue">
                                            </span><span id="lblDrvLicCategories" class="InputLabel" style="padding-left: 10px">
                                                Шофьорска книжка:</span> <span id="txtDrvLicCategories" class="ReadOnlyValue">
                                            </span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            <span id="lblWentToMilitary" class="InputLabel">Бил ли е на военна служба?:</span>
                                            <span id="txtWentToMilitary" class="ReadOnlyValue"></span><span id="lblMilitaryTraining"
                                                class="InputLabel" style="padding-left: 10px">Военна подготовка:</span> <span id="txtMilitaryTraining"
                                                    class="ReadOnlyValue"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                </table>
                <fieldset style="width: 876px; padding-bottom: 0px; margin-left: 5px; margin-top: 3px;">
                    <legend style="color: #0B4489; font-weight: bold; font-size: 1.25em;"></legend>
                    <table style="width: 876px;">
                        <tr>
                            <td align="left">
                                <div style="min-height: 5px">
                                </div>
                                <span id="Span1" class="SmallHeaderText">Медицинско освидетелстване</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <div id="divMedCertHTML">
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <fieldset style="width: 876px; padding-bottom: 0px; margin-left: 5px; margin-top: 3px;">
                    <legend style="color: #0B4489; font-weight: bold; font-size: 1.25em;"></legend>
                    <table style="width: 876px;">
                        <tr>
                            <td align="left">
                                <div style="min-height: 5px">
                                </div>
                                <span id="Span2" class="SmallHeaderText">Психологическа пригодност</span>
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <div id="divPsychCertHTML">
                                </div>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <table id="tblPrintDocuments">
                    <tr>
                        <td>
                            <select id="ddLetter" onchange="ChangeStyle('btnPrintLetter');">
                                <option value="-1" selected disabled>Моля изберете</option>
                                <option value="LPZP">Писмо ЛПЗП-нов обр.</option>
                                <option value="CVMKVarna">Писмо ЦВМК-Варна</option>
                                <option value="CVMKPlovdiv">Писмо ЦВМК-Пловдив</option>
                                <option value="CVMKSofia">Писмо ЦВМК-София</option>
                            </select>
                        </td>
                        <td style="text-align: center; vertical-align: middle;">
                            <div id="btnPrintLetter" class="DisabledButton" style="margin-top: 5px;" onclick="PrintLetter();">
                                <i></i>
                                <div style="width: 45px; padding-left: 5px; height: 24px;">
                                    Печат</div>
                                <b></b>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center;">
                            <div id="btnPrintDocuments" class="Button" onclick="PrintDocuments();">
                                <i></i>
                                <div style="width: 130px; padding-left: 5px;">
                                    Печат на декларации</div>
                                <b></b>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            </div> </div>
            <div style="height: 10px;">
            </div>
            <table style="width: 1400px;" cellspacing="0" cellpadding="0">
                <tr>
                    <td align="left" style="width: 100%">
                        <div id="TabSummary">
                            <ul>
                                <% if (IsPositionsVisible())
                                   { %>
                                <li class="ActiveTab" id="btnTabPositions" onclick="TabClick(this);" onmouseover="TabHover(this);"
                                    onmouseout="TabOut(this);" isalrеadyvisited="true"><a href="#" onclick="return false;"
                                        style="width: 150px; text-align: center">Желани длъжности</a></li>
                                <% } %>
                                <% if (IsEducationVisible())
                                   { %>
                                <li class="InactiveTab" id="btnTabEducation" onclick="TabClick(this);" onmouseover="TabHover(this);"
                                    onmouseout="TabOut(this);"><a href="#" onclick="return false;" style="width: 130px;
                                        text-align: center">Образование</a></li>
                                <% } %>
                                <% if (IsDocumentsVisible())
                                   { %>
                                <li class="InactiveTab" id="btnTabDocuments" onclick="TabClick(this);" onmouseover="TabHover(this);"
                                    onmouseout="TabOut(this);"><a href="#" onclick="return false;" style="width: 180px;
                                        text-align: center">Представени документи</a></li>
                                <% } %>
                                <% if (IsHistoryVisible())
                                   { %>
                                <li class="InactiveTab" id="btnTabHistory" onclick="TabClick(this);" onmouseover="TabHover(this);"
                                    onmouseout="TabOut(this);"><a href="#" onclick="return false;" style="width: 180px;
                                        text-align: center">История на кандидатурите</a></li>
                                <% } %>
                            </ul>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div class="EditApplTabsBottomLine" />
                    </td>
                </tr>
            </table>
            <div style="border: solid 1px #888888;">
                <div id="divPositions" style="display: none; padding: 20px 0 0 20px;" runat="server">
                </div>
                <div id="divEducation" style="display: none; text-align: center; padding: 5px 0 0 20px;"
                    runat="server">
                </div>
                <div id="divDocuments" style="display: none; text-align: left; padding: 5px 0 0 5px;"
                    runat="server">
                </div>
                <div id="divHistory" style="display: none; text-align: left; padding: 20px 0 0 20px;"
                    runat="server">
                </div>
            </div>
            <div style="height: 10px;">
            </div>
            <div id="loadingDiv" class="LoadingDiv">
                <img src="../Images/ajax-loader-big.gif" alt="Зареждане" title="Зареждане" />
            </div>
            <div>
                <table>
                    <tr align="center">
                        <td>
                            <asp:LinkButton ID="btnBack" runat="server" CssClass="Button" OnClick="btnBack_Click"
                                CheckForChanges="true"><i></i><div style="width:70px; padding-left:5px;">Назад</div><b></b></asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divAddApplicantPositionLightBox" class="GTibleLightBox" style="padding: 10px;
                display: none; text-align: center; max-width: 1200px; width: 90%; height: auto;
                max-height: 90%;">
                <img border='0' src='../Images/close.png' onclick="javascript:HideAddApplicantPositionLightBox();"
                    style="cursor: pointer; float: right;" alt='Затвори' title='Затвори' /><br />
                <div id="divAddApplicantPositionLightBoxContent">
                </div>
            </div>
            <div id="ApplicantPositionLightBox" class="ApplicantPositionLightBox" style="display: none;
                text-align: center;">
                <asp:HiddenField ID="hfApplicantPositionID" runat="server" />
                <center>
                    <table width="80%" style="text-align: center;">
                        <colgroup style="width: 40%">
                        </colgroup>
                        <colgroup style="width: 60%">
                        </colgroup>
                        <tr style="height: 15px">
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <span class="HeaderText" style="text-align: center;">Длъжност</span>
                            </td>
                        </tr>
                        <tr style="height: 15px">
                        </tr>
                        <tr style="min-height: 17px">
                            <td style="text-align: right;">
                                <span id="lblOrderNumLB" class="InputLabel">Заповед №:</span>
                            </td>
                            <td style="text-align: left;">
                                <span id="txtOrderNumLB" class="ReadOnlyValue"></span>
                            </td>
                        </tr>
                        <tr style="min-height: 17px">
                            <td style="text-align: right;">
                                <span id="lblResponsibleMilitaryUnitLB" class="InputLabel">
                                    <%= PMIS.Common.CommonFunctions.GetLabelText("MilitaryUnit")%>
                                    отговорна за конкурса:</span>
                            </td>
                            <td style="text-align: left; vertical-align: bottom;">
                                <span id="txtResponsibleMilitaryUnitLB" class="ReadOnlyValue"></span>
                            </td>
                        </tr>
                        <tr style="min-height: 17px">
                            <td style="text-align: right; vertical-align: top;">
                                <span id="lblPositionNameLB" class="InputLabel">Длъжност:</span>
                            </td>
                            <td style="text-align: left;">
                                <span id="txtPositionNameLB" class="ReadOnlyValue"></span>
                            </td>
                        </tr>
                        <tr style="min-height: 17px">
                            <td style="text-align: right;">
                                <span id="lblDocStatus" class="InputLabel">Статус на документите:</span>
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddDocStatus" runat="server" class="InputField" UnsavedCheckSkipMe="true"
                                    Width="180">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr style="height: 30px">
                            <td colspan="2">
                                <span id="spanApplicantPositionLightBoxMessage" class="ErrorText" style="display: none;">
                                </span>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center;">
                                <table style="margin: 0 auto;">
                                    <tr>
                                        <td>
                                            <div id="btnSaveApplicantPositionLightBox" style="display: inline;" onclick="SaveApplicantPosition();"
                                                class="Button">
                                                <i></i>
                                                <div id="btnSaveApplicantPositionLightBoxText" style="width: 70px;">
                                                    Запис</div>
                                                <b></b>
                                            </div>
                                            <div id="btnCloseApplicantPositionLightBox" style="display: inline;" onclick="HideApplicantPositionsLightBox();"
                                                class="Button">
                                                <i></i>
                                                <div id="btnCloseApplicantPositionLightBoxText" style="width: 70px;">
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
            <asp:HiddenField runat="server" ID="hdnPersonId" />
            <asp:HiddenField runat="server" ID="hdnApplicantId" />
            <asp:HiddenField runat="server" ID="hdnIdentNumber" />
            <asp:HiddenField runat="server" ID="hdnMilitaryDepartmentId" />
            </center>
            <div id="ApplicantEducationLightBox" class="PersonAbilityLightBox" style="display: none;
                text-align: center;">
            </div>
            <div id="ApplicantLanguageLightBox" class="PersonAbilityLightBox" style="display: none;
                text-align: center;">
            </div>
            <div id="ApplicantDocumentStatusLightBox" class="ApplicantDocumentStatusLightBox"
                style="display: none; text-align: center;">
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script src="../Scripts/Ajax.js" type="text/javascript"></script>

    <script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Scripts/EditApplicant.js'></script>

    <script type="text/javascript">
        var hdnPersonIdClientID = "<%= hdnPersonId.ClientID %>";
        var hdnApplicantIdClientID = "<%= hdnApplicantId.ClientID %>";
        var hdnIdentNumberClientID = "<%= hdnIdentNumber.ClientID %>";
        var hdnMilitaryDepartmentIdClientID = "<%= hdnMilitaryDepartmentId.ClientID %>";

        window.addEventListener ? window.addEventListener('load', PageLoad, false) : window.attachEvent('onload', PageLoad);
        //        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(PageEndRequestHandler);

        //Call this when the page is loaded
        function PageLoad() {
            if (document.getElementById("btnTabPositions") != null)
                JSLoadTab("btnTabPositions");
            else if (document.getElementById("btnTabEducation") != null)
                JSLoadTab("btnTabEducation");
            else if (document.getElementById("btnTabDocuments") != null)
                JSLoadTab("btnTabDocuments");
            else if (document.getElementById("btnTabHistory") != null)
                JSLoadTab("btnTabHistory");
        }

        //Tabs logic
        //This is the ID of the selected tab (its button)
        var selectedTab = "btnTabPositions";

        //Call this when hovering over a particualr tab button
        function TabHover(tab) {
            if (tab.id != selectedTab) {
                tab.className = "HoverTab";
            }
        }

        //Call this when leaving a particualr tab button
        function TabOut(tab) {
            if (tab.id != selectedTab) {
                tab.className = "InactiveTab";
            }
        }

        //Call this when a particular tab is clicked
        function TabClick(tab) {
            //Clear any messages
            //  document.getElementById("<%= lblMessage.ClientID %>").innerHTML = "";

            //Set the previously selected tab as inactive
            if (document.getElementById(selectedTab) != null)
                document.getElementById(selectedTab).className = "InactiveTab";

            //Set the current tab as active
            tab.className = "ActiveTab";
            selectedTab = tab.id;

            //Check if this tab has been already loaded
            //If it hasn't been loaded yet then get its content from the server via AJAX
            if (!IsTabAlreadyVisited()) {
                JSLoadTab(selectedTab);

                if (selectedTab === "btnTabDocuments")
                    LoadOriginalValues();
            }
            else //If the tab has been already loaded then just display its content
            {
                ShowDiv(selectedTab);
            }
        }

        //Check if a particular tab has been visited
        //Store this information as an attribute of the tab button
        function IsTabAlreadyVisited() {
            if (document.getElementById(selectedTab).getAttribute("isalrеadyvisited") &&
          document.getElementById(selectedTab).getAttribute("isalrеadyvisited") == "true")
                return true;
            else
                return false;
        }

        //Show the content of the currently selected tab
        function ShowDiv(tab) {
            selectedTab = tab;

            //Hide all divs
            document.getElementById("<%=divPositions.ClientID %>").style.display = "none";
            document.getElementById("<%=divEducation.ClientID %>").style.display = "none";
            document.getElementById("<%=divDocuments.ClientID %>").style.display = "none";
            document.getElementById("<%=divHistory.ClientID %>").style.display = "none";

            //Display the content of the current tab
            var targetDivId = GetTargetDivByTabId(selectedTab);
            document.getElementById(targetDivId).style.display = "";

            //Mark it as visited
            document.getElementById(selectedTab).setAttribute("isalrеadyvisited", "true");
        }

        //Load the content of a particular tab via an AJAX call
        function JSLoadTab(selectedTabId) {
            var url = "EditApplicant.aspx?AjaxMethod=JSLoadTab&HdnPersonId=" + document.getElementById(hdnPersonIdClientID).value + "&MilitaryDepartmentId=" + document.getElementById(hdnMilitaryDepartmentIdClientID).value + "&HdnApplicantId=" + document.getElementById(hdnApplicantIdClientID).value;
            var params = "";
            params += "SelectedTabId=" + selectedTabId;
            var myAJAX = new AJAX(url, true, params, function(xml) { JSLoadTab_CallBack(xml, selectedTabId); });
            myAJAX.Call();
        }

        //When the response is ready then get the loaded HTML and put it on the target div
        function JSLoadTab_CallBack(xml, selectedTabId) {
            var targetDivId = GetTargetDivByTabId(selectedTabId);
            document.getElementById(targetDivId).innerHTML = xmlValue(xml, "TabHTML");
            //Show the loaded content
            ShowDiv(selectedTabId);

            RefreshDatePickers();
        }

        //Use this function to get the client id of the content div for a particular tab
        function GetTargetDivByTabId(selectedTabId) {
            var targetDivId = "";

            switch (selectedTabId) {
                case "btnTabPositions":
                    {
                        targetDivId = "<%=divPositions.ClientID %>";
                        break;
                    }
                case "btnTabEducation":
                    {
                        targetDivId = "<%=divEducation.ClientID %>";
                        break;
                    }
                case "btnTabDocuments":
                    {
                        targetDivId = "<%=divDocuments.ClientID %>";
                        break;
                    }
                case "btnTabHistory":
                    {
                        targetDivId = "<%=divHistory.ClientID %>";
                        break;
                    }
            }

            return targetDivId;
        }

        //Reset the tab selection to the first (the default) tab
        function ResetTabs() {
            var selectedTab = "btnTabPositions";

            if (document.getElementById("btnTabPositions") != null)
                selectedTab = "btnTabPositions";
            else if (document.getElementById("btnTabEducation") != null)
                selectedTab = "btnTabEducation";
            else if (document.getElementById("btnTabDocuments") != null)
                selectedTab = "btnTabDocuments";
            else if (document.getElementById("btnTabHistory") != null)
                selectedTab = "btnTabHistory";
            else
                return;

            var tab = document.getElementById(selectedTab);

            tab.className = "ActiveTab";
            selectedTab = tab.id;
            JSLoadTab(selectedTab);
        }

        function ShowAddApplicantPositionLightBox() {
            var url = "EditApplicant.aspx?AjaxMethod=JSGetAddApplicantPositionLightBox";
            var params = "";
            params += "ApplicantID=" + document.getElementById(hdnApplicantIdClientID).value;
            params += "&HdnPersonId=" + document.getElementById("<%= hdnPersonId.ClientID %>").value;

            function response_handler(xml) {
                document.getElementById('divAddApplicantPositionLightBoxContent').innerHTML = xmlNodeText(xml.childNodes[0]);
                document.getElementById("HidePage").style.display = "";
                document.getElementById("divAddApplicantPositionLightBox").style.display = "";
                CenterLightBox("divAddApplicantPositionLightBox");
                ResizeAddApplicantPositionLightBox();
            }

            var myAJAX = new AJAX(url, true, params, response_handler);
            myAJAX.Call();
        }

        var calculatedScrollableWrapperDivMaxHeight = "";
        function ResizeAddApplicantPositionLightBox() {
            var scrollableWrapperDiv = document.getElementById("tblVacantPositionsScrollWrapper");

            if (calculatedScrollableWrapperDivMaxHeight == "") {
                var divLightboxHeight = document.getElementById("divAddApplicantPositionLightBox").offsetHeight;
                var heightOfEverythingInLightboxExceptTable = 238;
                var maxScrollableDivHeight = divLightboxHeight - heightOfEverythingInLightboxExceptTable;

                calculatedScrollableWrapperDivMaxHeight = maxScrollableDivHeight + "px";
            }

            scrollableWrapperDiv.style.maxHeight = calculatedScrollableWrapperDivMaxHeight;
        }

        function HideAddApplicantPositionLightBox() {
            document.getElementById("HidePage").style.display = "none";
            document.getElementById("divAddApplicantPositionLightBox").style.display = "none";
        }

        function FilterAddApplicantPositionLightBox() {
            var url = "EditApplicant.aspx?AjaxMethod=JSGetAddApplicantPositionLightBox";

            var params = "";
            params += "RespMilitaryUnitID=" + MilitaryUnitSelectorUtil.GetSelectedValue("RespMilUnitSelector");            
            params += "&MilitaryUnitID=" + MilitaryUnitSelectorUtil.GetSelectedValue("MilUnitSelector");
            params += "&ApplicantID=" + document.getElementById(hdnApplicantIdClientID).value;
            params += "&HdnPersonId=" + document.getElementById("<%= hdnPersonId.ClientID %>").value;
            params += "&PositionName=" + document.getElementById("txtPositionName").value;
            params += "&OrderNum=" + document.getElementById("txtOrderNum").value;
            params += "&PageIndex=" + document.getElementById("hdnAddApplicantPositionPageIndex").value;
            params += "&MaxPage=" + document.getElementById("hdnPageMaxPage").value;
            params += "&OrderBy=" + document.getElementById("hdnAddApplicantPositionOrderBy").value;

            function response_handler(xml) {
                document.getElementById('divAddApplicantPositionLightBoxContent').innerHTML = xmlNodeText(xml.childNodes[0]);
                //CenterLightBox("divAddApplicantPositionLightBox");
                ResizeAddApplicantPositionLightBox();
            }

            var myAJAX = new AJAX(url, true, params, response_handler);
            myAJAX.Call();
        }

        function BtnPagingClick(objectName) {
            hdnPageIdx = document.getElementById('hdnAddApplicantPositionPageIndex');
            hdnMaxPage = document.getElementById('hdnPageMaxPage');
            switch (objectName) {
                case "btnFirst":
                    hdnPageIdx.value = 1;

                    FilterAddApplicantPositionLightBox();
                    break;

                case "btnPrev":
                    pageIdx = parseInt(hdnPageIdx.value);

                    if (pageIdx > 1) {
                        pageIdx--;
                        hdnPageIdx.value = pageIdx;
                        FilterAddApplicantPositionLightBox();
                    }

                    break;

                case "btnNext":
                    pageIdx = parseInt(hdnPageIdx.value);
                    maxPage = parseInt(hdnMaxPage.value);

                    if (pageIdx < maxPage) {
                        pageIdx++;
                        hdnPageIdx.value = pageIdx;
                        FilterAddApplicantPositionLightBox();
                    }
                    break;


                case "btnLast":
                    hdnPageIdx.value = hdnMaxPage.value;

                    FilterAddApplicantPositionLightBox();
                    break;

                case "btnPageGo":
                    maxPage = parseInt(hdnMaxPage.value);
                    goToPage = parseInt(document.getElementById('txtTableGotoPage').value);

                    if (isInt(goToPage) && goToPage > 0 && goToPage <= maxPage) {
                        hdnPageIdx.value = goToPage;
                        FilterAddApplicantPositionLightBox();
                    }
                    break;

                default:
                    break;
            }

        }

        function SortAddApplicantPositionTableBy(sort) {
            hdnPageIdx = document.getElementById('hdnAddApplicantPositionPageIndex');
            hdnOrderBy = document.getElementById("hdnAddApplicantPositionOrderBy");
            orderBy = parseInt(hdnOrderBy.value);

            if (orderBy == sort) {
                sort = sort + 100;
            }

            hdnOrderBy.value = sort;
            hdnPageIdx.value = 1; //We go to 1st page

            FilterAddApplicantPositionLightBox();
        }

        function SelectPosition(vacancyAnnouncePositionID) {
            var url = "EditApplicant.aspx?AjaxMethod=JSAddApplicantPosition";
            var params = "HdnMilitaryDepartmentId=" + document.getElementById(hdnMilitaryDepartmentIdClientID).value +
                         "&HdnPersonId=" + document.getElementById(hdnPersonIdClientID).value +
                         "&ApplicantId=" + document.getElementById(hdnApplicantIdClientID).value +
                         "&VacancyAnnouncePositionID=" + vacancyAnnouncePositionID;

            function response_handler(xml) {
                document.getElementById(hdnApplicantIdClientID).value = xmlValue(xml, 'ApplicantID');

                //Show Edit Applicant Person Details if it is hide
                document.getElementById("<%= divEdit.ClientID %>").style.display = "";

                HideAddApplicantPositionLightBox();
                JSLoadTab('btnTabPositions');
                RefreshApplicantDocumentsTab();
            }

            var myAJAX = new AJAX(url, true, params, response_handler);
            myAJAX.Call();
        }

        var initApplicantPositionDialogHeight = 0;

        function ShowApplicantPositionLightBox(applicantPositionID) {
            if (applicantPositionID != 0) // gets current values if editing applicant position
            {
                var url = "EditApplicant.aspx?AjaxMethod=JSGetApplicantPosition";

                var params = "ApplicantPositionID=" + applicantPositionID;

                function response_handler(xml) {
                    document.getElementById("txtOrderNumLB").innerHTML = xmlValue(xml, "OrderNum");
                    document.getElementById("txtResponsibleMilitaryUnitLB").innerHTML = xmlValue(xml, "ResponsibleMilitaryUnit");
                    document.getElementById("txtPositionNameLB").innerHTML = xmlValue(xml, "PositionName");
                    document.getElementById("<%= ddDocStatus.ClientID %>").value = xmlValue(xml, "DocStatusID");
                }

                var myAJAX = new AJAX(url, true, params, response_handler);
                myAJAX.Call();

                // cleaning old values
                document.getElementById("txtOrderNumLB").innerHTML = "";
                document.getElementById("txtResponsibleMilitaryUnitLB").innerHTML = "";
                document.getElementById("txtPositionNameLB").innerHTML = "";
                //document.getElementById("<%= ddDocStatus.ClientID %>").value = 1;
            }
            else // cleaning old values, if adding applicant position - only for compatibility, not a real case
            {
                document.getElementById("txtOrderNumLB").innerHTML = "";
                document.getElementById("txtResponsibleMilitaryUnitLB").innerHTML = "";
                document.getElementById("txtPositionNameLB").innerHTML = "";
                document.getElementById("<%= ddDocStatus.ClientID %>").value = 1;
            }

            document.getElementById("<%= hfApplicantPositionID.ClientID %>").value = applicantPositionID; // setting applicant position ID(0 - if new applicant position)

            // clean message label in the light box and hide it
            document.getElementById("spanApplicantPositionLightBoxMessage").style.display = "none";
            document.getElementById("spanApplicantPositionLightBoxMessage").innerHTML = "";

            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("ApplicantPositionLightBox").style.display = "";
            CenterLightBox("ApplicantPositionLightBox");

            initApplicantPositionDialogHeight = document.getElementById("ApplicantPositionLightBox").offsetHeight;
        }

        // Close the light box
        function HideApplicantPositionsLightBox() {
            document.getElementById("HidePage").style.display = "none";
            document.getElementById("ApplicantPositionLightBox").style.display = "none";
        }

        // Saves applicant position through ajax request, if light box values are valid, or displays generated error messages
        function SaveApplicantPosition() {
            var url = "EditApplicant.aspx?AjaxMethod=JSCheckApplicantPositionDocuemnts";

            var params = "ApplicantPositionID=" + document.getElementById("<%= hfApplicantPositionID.ClientID %>").value +
                         "&DocStatusID=" + document.getElementById("<%= ddDocStatus.ClientID %>").value;

            function SaveChanges() {
                var url = "EditApplicant.aspx?AjaxMethod=JSSaveApplicantPosition";

                var params = "ApplicantID=" + document.getElementById("<%= hdnApplicantId.ClientID %>").value +
                       "&HdnApplicantId=" + document.getElementById("<%= hdnApplicantId.ClientID %>").value + //Pass this because of the "other military department" check in the GenerateTabPositionsContent() method
                       "&ApplicantPositionID=" + document.getElementById("<%= hfApplicantPositionID.ClientID %>").value +
                       "&DocStatusID=" + document.getElementById("<%= ddDocStatus.ClientID %>").value +
                       "&HdnPersonId=" + document.getElementById("<%= hdnPersonId.ClientID %>").value;

                function response_handler2(xml) {

                    var hideDialog = true;
                    var resultMsg = xmlValue(xml, "response");
                    if (resultMsg == "ERROR") {
                        var lightBoxMessage = document.getElementById("spanApplicantPositionLightBoxMessage");
                        lightBoxMessage.innerHTML = "Грешка при запис на данните";
                        lightBoxMessage.style.display = "";
                        hideDialog = false;
                        lightBoxMessage.innerHTML = resultMsg;
                    }
                    else {
                        document.getElementById("<%= divPositions.ClientID %>").innerHTML = xmlNodeText(xml.childNodes[0]);
                    }

                    if (hideDialog)
                        HideApplicantPositionsLightBox();
                }

                var myAJAX = new AJAX(url, true, params, response_handler2);
                myAJAX.Call();
            }

            function response_handler(xml) {
                var result = xmlValue(xml, "response");

                if (result != "OK") {
                    var lightBoxMessage = document.getElementById("spanApplicantPositionLightBoxMessage");
                    lightBoxMessage.innerHTML = result;
                    lightBoxMessage.style.display = "";
                }
                else {
                    SaveChanges();
                }
            }

            var myAJAX = new AJAX(url, true, params, response_handler);
            myAJAX.Call();
        }


        // Delete applicant position item through ajax request
        function DeleteApplicantPosition(applicantPositionID) {
            YesNoDialog('Желаете ли да изтриете длъжността?', ConfirmYes, null);

            function ConfirmYes() {
                var url = "EditApplicant.aspx?AjaxMethod=JSDeleteApplicantPosition";
                var params = "";
                params += "ApplicantID=" + document.getElementById("<%= hdnApplicantId.ClientID %>").value;
                params += "&hdnApplicantID=" + document.getElementById("<%= hdnApplicantId.ClientID %>").value; //Pass this because of the "other military department" check in the GenerateTabPositionsContent() method
                params += "&ApplicantPositionID=" + applicantPositionID;
                params += "&HdnPersonId=" + document.getElementById("<%= hdnPersonId.ClientID %>").value;

                function response_handler(xml) {
                    if (xmlValue(xml, "response") != "ERROR") {
                        document.getElementById("<%= divPositions.ClientID %>").innerHTML = xmlNodeText(xml.childNodes[0]);
                        RefreshApplicantDocumentsTab();
                    }
                }

                var myAJAX = new AJAX(url, true, params, response_handler);
                myAJAX.Call();
            }
        }

        var personId = document.getElementById(hdnPersonIdClientID).value;
        var applicantId = document.getElementById("<%= hdnApplicantId.ClientID %>").value
        function PrintDocuments() {
            location.href = "EditApplicant.aspx?AjaxMethod=JSPrintDocuments&PersonId=" + personId;
        }

        function PrintLetter() {
            var letter = document.getElementById("ddLetter").value;

            if (letter != "-1")
                location.href = "EditApplicant.aspx?AjaxMethod=JSPrintLetter&ApplicantId=" + applicantId + "&Letter=" + letter;
        }

        function PrintApplication(numberOfTable) {
            var application = document.getElementById('ddApplication-' + numberOfTable).value;

            if (application != "-1") {

                var vacancyAnnounceID = document.getElementById('hdnVacancyAnnounceID-' + numberOfTable).value;
                var responsibleMilitaryUnitID = document.getElementById('hdnResponsibleMilitaryUnitID-' + numberOfTable).value;
                var responsibleMilUnitHasValue = true;

                if (responsibleMilitaryUnitID == null || responsibleMilitaryUnitID === '')
                    responsibleMilUnitHasValue = false;

                if (responsibleMilUnitHasValue) {
                    var tnisButton = document.getElementById("btnPrintApplication-" + numberOfTable);
                    var dataReg = tnisButton.getAttribute('data-reg');

                    if (dataReg == 0) {
                        Register(numberOfTable, applicantId, vacancyAnnounceID, responsibleMilitaryUnitID);
                    }
                }

                var url = "EditApplicant.aspx?AjaxMethod=JSPrintApplication";
                var params = "";
                params += "&ApplicantId=" + applicantId;
                params += "&App=" + application;
                params += "&VacAnnId=" + vacancyAnnounceID;
                params += "&ResMilUnId=" + (responsibleMilUnitHasValue ? responsibleMilitaryUnitID : "0");

                location.href = url + params;
            }
        }

        function Register(numberOfTable, applicantId, vacancyAnnounceID, responsibleMilitaryUnitID) {
            var url = "EditApplicant.aspx?AjaxMethod=JSRegister";
            var params = "";
            params += "&ApplicantId=" + applicantId;
            params += "&VacAnnId=" + vacancyAnnounceID;
            params += "&ResMilUnId=" + responsibleMilitaryUnitID;
            params += "&TblCount=" + numberOfTable;

            var myAJAX = new AJAX(url, true, params, Register_Callback);
            myAJAX.Call();
        }

        function Register_Callback(xml) {
            var status = xmlValue(xml, "response");
            var registerNumber = xmlValue(xml, "registerNumber");
            var numberOfTable = xmlValue(xml, "tblCount");

            var button = document.getElementById("btnPrintApplication-" + numberOfTable);
            button.setAttribute("data-reg", "1");

            var divInButton = button.getElementsByTagName("div")[0];
            divInButton.setAttribute("style", "width: 45px");
            divInButton.innerHTML = "Печат";

            document.getElementById("spnRegNum-" + numberOfTable).innerHTML = registerNumber;

            var tableRegister = document.getElementById("tblReg-" + numberOfTable);
            tableRegister.style.display = "";
        }

        function ChangeStyle(buttonId) {
            var button = document.getElementById(buttonId);
            button.className = 'Button';
        }

        function SaveRegisterData(numberOfTable) {
            document.getElementById("lblMessage-" + numberOfTable).innerHTML = "";
        
            if (ValidateData(numberOfTable)) {
                var url = "EditApplicant.aspx?AjaxMethod=JSSaveRegisterData";
                var params = "";
                params += "&ApplicantId=" + applicantId;
                params += "&VacancyAnnounceId=" + document.getElementById('hdnVacancyAnnounceID-' + numberOfTable).value;
                params += "&ResponsibleMilitaryUnitId=" + document.getElementById('hdnResponsibleMilitaryUnitID-' + numberOfTable).value;
                params += "&DocumentDate=" + custEncodeURI(document.getElementById("txtRegDate-" + numberOfTable).value);
                params += "&PageCount=" + custEncodeURI(document.getElementById("txtRegPageCount-" + numberOfTable).value);
                params += "&Notes=" + custEncodeURI(document.getElementById("txtaRegNotes-" + numberOfTable).value);
                params += "&TblCount=" + numberOfTable;

                var myAJAX = new AJAX(url, true, params, SaveRegisterData_Callback);
                myAJAX.Call();
            }
            else {
                var lblError = document.getElementById("lblErrorMsg-" + numberOfTable);
                if (lblError != null && lblError != undefined) {
                    lblError.innerHTML = ValidationMessage;
                    lblError.className = "ErrorText";
                }
            }
        }

        function SaveRegisterData_Callback(xml) {
            var status = xmlValue(xml, "response");
            var docmentDate = xmlValue(xml, "docmentDate");
            var pageCount = xmlValue(xml, "pageCount");
            var notes = xmlValue(xml, "notes");
            var numberOfTable = xmlValue(xml, "tblCount");

            document.getElementById("txtRegDate-" + numberOfTable).value = docmentDate;
            document.getElementById("txtRegPageCount-" + numberOfTable).value = pageCount;
            document.getElementById("txtaRegNotes-" + numberOfTable).value = notes;

            document.getElementById("lblErrorMsg-" + numberOfTable).innerHTML = "";
            document.getElementById("lblMessage-" + numberOfTable).innerHTML = "Успешен запис";
        }

        var ValidationMessage = "";

        function ValidateData(numberOfTable) {
            ValidationMessage = "";
            var notValidFields = new Array();

            var documentDate = document.getElementById("txtRegDate-" + numberOfTable);
            var pageCount = document.getElementById("txtRegPageCount-" + numberOfTable);
            var notes = document.getElementById("txtaRegNotes-" + numberOfTable);

            if (documentDate.disabled == true || documentDate.style.display == "none") {
                res = false;
                notValidFields.push("Дата");
            }
            else {
                if (documentDate.value.Trim() != "") {
                    if (!IsValidDate(documentDate.value)) {
                        ValidationMessage += GetErrorMessageDate("Дата") + "</br>";
                    }
                }
            }

            if (pageCount.disabled == true || pageCount.style.display == "none") {
                res = false;
                notValidFields.push("Брой листа");
            }

            if (notes.disabled == true || notes.style.display == "none") {
                res = false;
                notValidFields.push("Бележки");
            }

            var notValidFieldsCount = notValidFields.length;

            if (notValidFieldsCount > 0) {
                var noRightsMessage = GetErrorMessageNoRights(notValidFields);
                ValidationMessage += noRightsMessage + "<br />";
            }

            return (ValidationMessage == "");
        }

        function MoveApplicantPosition(pIndex, pRequestCommandPositionID_1, pRequestCommandPositionID_2) {

            var url = "EditApplicant.aspx?AjaxMethod=JSMoveApplicantPosition";

            var params = "ApplicantPositionID_1=" + pRequestCommandPositionID_1 +
                         "&ApplicantPositionID_2=" + pRequestCommandPositionID_2 +
                         "&Idx=" + pIndex;

            function response_handler(xml) {
                document.getElementById("tdApplicantPosisionsTable-" + pIndex).innerHTML = xmlValue(xml, "refreshedPositionsTable");
            }

            var myAJAX = new AJAX(url, true, params, response_handler);
            myAJAX.Call();
        }
        
    </script>

</asp:Content>
