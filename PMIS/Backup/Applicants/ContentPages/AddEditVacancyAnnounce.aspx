<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="AddEditVacancyAnnounce.aspx.cs" Inherits="PMIS.Applicants.ContentPages.AddEditVacancyAnnounce" %>

<%@ Register Assembly="MilitaryUnitSelector" TagPrefix="is" Namespace="MilitaryUnitSelector" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .VacancyAnnounceTab
        {
            border: solid 1px #888888;
            width: 1360px;
            min-height: 100px;
            padding: 5px;
        }
        .ShadowContainer
        {
            width: auto;
            min-width: 1380px;
        }
    </style>

    <script src="../Scripts/Ajax.js" type="text/javascript"></script>

    <div id="jsMilitaryUnitSelectorDiv" runat="server">
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hfVacancyAnnounceID" runat="server" />
            <asp:HiddenField ID="hfFromHome" runat="server" />
            <div id="divExamsLightBox" class="ExamsLightBox" style="padding: 10px; display: none;
                text-align: center;">
                <img border='0' src='../Images/close.png' onclick="javascript:HideExamLightBox();"
                    style="cursor: pointer; float: right;" alt='Затвори' title='Затвори' /><br />
                <div id="divExamsLightBoxContent">
                </div>
            </div>
            <div id="divDocumentsLightBox" class="ExamsLightBox" style="padding: 10px; display: none;
                text-align: center;">
                <img border='0' src='../Images/close.png' onclick="javascript:HideDocumentLightBox();"
                    style="cursor: pointer; float: right;" alt='Затвори' title='Затвори' /><br />
                <div id="divDocumentsLightBoxContent">
                </div>
            </div>
            <div style="height: 20px">
            </div>
            <table style="width: 100%;">
                <tr>
                    <td style="text-align: center;">
                        <span id="lblHeaderTitle" runat="server" class="HeaderText"></span>
                    </td>
                </tr>
                <tr style="height: 20px;">
                    <td>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <center>
                            <fieldset style="width: 840px; padding: 10px;">
                                <legend style="color: #0B4489; font-weight: bold; font-size: 1.1em;">Конкурс</legend>
                                <table style="width: 100%; margin-top: 10px;">
                                    <tr>
                                        <td style="text-align: left; width: 19%;">
                                            <asp:Label ID="lblOrderNum" runat="server" CssClass="InputLabel" Text="Заповед №:"></asp:Label>
                                        </td>
                                        <td style="text-align: left; width: 13%;">
                                            <asp:Label ID="lblOrderDate" runat="server" CssClass="InputLabel" Text="от дата:"></asp:Label>
                                        </td>
                                        <td style="text-align: left; width: 13%;">
                                            <asp:Label ID="lblEndDate" runat="server" CssClass="InputLabel" Text="крайна дата:"></asp:Label>
                                        </td>
                                        <td style="text-align: left; width: 15%;">
                                            <asp:Label ID="lblMaxPositions" runat="server" CssClass="InputLabel" Text="Макс. длъжности:"></asp:Label>
                                        </td>
                                        <td style="text-align: left; width: 29%;">
                                            <asp:Label ID="lblVacancyAnnounceStatus" runat="server" CssClass="InputLabel" Text="Текущ статус:"></asp:Label>
                                        </td>
                                        <td style="text-align: left; width: 11%;">
                                            <asp:Label ID="lblVacancyAnnounceType" runat="server" CssClass="InputLabel" Text="Вид:"></asp:Label>
                                        </td>
                                        <tr>
                    </td>
                    <td style="text-align: left; vertical-align: top">
                        <asp:TextBox ID="txtOrderNum" runat="server" CssClass="RequiredInputField" Width="140" MaxLength="250"></asp:TextBox>
                    </td>
                    <td style="text-align: left; vertical-align: top">
                        <asp:TextBox ID="txtOrderDate" runat="server" CssClass="RequiredInputField" Width="70" MaxLength="10"></asp:TextBox>
                    </td>
                    <td style="text-align: left; vertical-align: top">
                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="RequiredInputField" Width="70" MaxLength="10"></asp:TextBox>
                    </td>
                    <td style="text-align: left; vertical-align: top">
                        <asp:TextBox ID="txtMaxPositions" runat="server" CssClass="InputField" Width="60"
                            MaxLength="250"></asp:TextBox>
                    </td>
                    <td style="text-align: left; vertical-align: top">
                        <asp:DropDownList runat="server" ID="ddVacancyAnnounceStatuses" CssClass="InputField"
                            Width="215">
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: left; vertical-align: top">
                        <asp:RadioButton ID="rbVacancyAnnounceTypeStaff" Text="Кадрови" runat="server" Checked="true"
                            GroupName="rbGroupVacancyAnnounceType" />
                        <br />
                        <asp:RadioButton ID="rbVacancyAnnounceTypeReserve" Text="Резерв" runat="server" GroupName="rbGroupVacancyAnnounceType" />
                    </td>
                </tr>
            </table>
            </fieldset> </center> </td> </tr>
            <tr>
                <td style="text-align: center;">
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: center;">
                    <asp:LinkButton ID="btnSave" runat="server" CssClass="Button" OnClientClick="return ValidateData();"
                        OnClick="btnSave_Click"><i></i><div style="width:70px; padding-left:5px;">Запис</div><b></b></asp:LinkButton>
                    &nbsp;
                </td>
            </tr>
            <tr runat="server" id="rowTabs">
                <td style="text-align: left;">
                    <table style="width: 1400px;" cellspacing="0" cellpadding="0">
                        <tr>
                            <td align="left" style="width: 100%">
                                <div id="TabSummary">
                                    <ul>
                                        <% if (IsPositionsVisible())
                                           { %>
                                        <li class="ActiveTab" id="btnTabPositions" onclick="TabClick(this);" onmouseover="TabHover(this);"
                                            onmouseout="TabOut(this);" isalrеadyvisited="true"><a href="#" onclick="return false;"
                                                style="width: 100px">Длъжности</a></li>
                                        <% } %>
                                        <% if (IsExamsVisible())
                                           { %>
                                        <li class="InactiveTab" id="btnTabExams" onclick="TabClick(this);" onmouseover="TabHover(this);"
                                            onmouseout="TabOut(this);"><a href="#" onclick="return false;" style="width: 100px">
                                                Изпити</a></li>
                                        <% } %>
                                        <% if (IsDocumentsVisible())
                                           { %>
                                        <li class="InactiveTab" id="btnTabDocuments" onclick="TabClick(this);" onmouseover="TabHover(this);"
                                            onmouseout="TabOut(this);"><a href="#" onclick="return false;" style="width: 100px">
                                                Документи</a></li>
                                        <% } %>
                                    </ul>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <% if (IsPositionsVisible() || IsExamsVisible() || IsDocumentsVisible())
                                   { %>
                                <div class="VacAnnTabsBottomLine" />
                                <% } %>
                            </td>
                        </tr>
                    </table>
                    <div id="divPositions" style="display: none;" runat="server">
                    </div>
                    <div id="divExams" style="display: none;" runat="server">
                    </div>
                    <div id="divDocuments" style="display: none;" runat="server">
                    </div>
                    <div style="height: 10px;">
                    </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: center;">
                    <asp:LinkButton ID="btnBack" runat="server" CssClass="Button" OnClick="btnBack_Click"
                        CheckForChanges="true"><i></i><div style="width:70px; padding-left:5px;">Назад</div><b></b></asp:LinkButton>
                </td>
            </tr>
            </table>
            <div id="divAddVacancyAnnouncePositionLightBox" class="GTibleLightBox" style="padding: 10px;
                display: none; text-align: center; width: 800px;">
                <img border='0' src='../Images/close.png' onclick="javascript:HideAddVacancyAnnouncePositionLightBox();"
                    style="cursor: pointer; float: right;" alt='Затвори' title='Затвори' /><br />
                <div id="divAddVacancyAnnouncePositionLightBoxContent">
                </div>
            </div>
            <div id="divAddVacancyAnnouncePositionManuallyLightBox" class="VacancyAnnouncePositionManuallyLightBox"
                style="display: none; text-align: center;">
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
                                <span class="HeaderText" style="text-align: center;">Добавяне на длъжност</span>
                            </td>
                        </tr>
                        <tr style="height: 15px">
                        </tr>
                        <tr style="min-height: 17px">
                            <td style="text-align: right; vertical-align: top; white-space: nowrap;">
                                <span id="lblMilitaryUnit" class="InputLabel">
                                    <%= PMIS.Common.CommonFunctions.GetLabelText("MilitaryUnit")%>:</span>
                            </td>
                            <td style="text-align: left;">
                                <is:MilitaryUnitSelector ID="msMilitaryUnit" runat="server" DataSourceWebPage="DataSource.aspx"
                                    DataSourceKey="MilitaryUnit" DivMainCss="isDivMainClassRequired" DivListCss="isDivListClass"
                                    DivFullListCss="isDivFullListClass" ResultMaxCount="1000" UnsavedCheckSkipMe="true"
                                    DropDownLimit="3" />
                            </td>
                        </tr>
                        <tr style="min-height: 17px">
                            <td style="text-align: right; vertical-align: top;">
                                <span id="lblPosition" class="InputLabel">Длъжност:</span>
                            </td>
                            <td style="text-align: left;">
                                <input type="text" id="txtPosition" class="RequiredInputField" style="width: 200px;" maxlength="300" />
                            </td>
                        </tr>
                        <tr style="min-height: 17px">
                            <td style="text-align: right; vertical-align: top;">
                                <span id="lblPositionCode" class="InputLabel">Код на длъжността:</span>
                            </td>
                            <td style="text-align: left;">
                                <input type="text" id="txtPositionCode" class="RequiredInputField" style="width: 100px;" maxlength="200" />
                            </td>
                        </tr>
                         <tr style="min-height: 17px">
                            <td style="text-align: right; vertical-align: top;">
                                <span id="lblMilitaryRanks" class="InputLabel">Звание:</span>
                            </td>
                            <td style="text-align: left;">
                                <table>
                                   <tr>
                                      <td style="vertical-align: top;">
                                         <asp:ListBox runat="server" ID="ddAvailableRanks" UnsavedCheckSkipMe="true" CssClass="InputField" Width="200px" Height="90px" SelectionMode="Multiple"></asp:ListBox>
                                      </td>
                                      <td style="vertical-align: middle;">
                                         <input type="button" value=">>" title="Избор" onclick="SelectMilitaryRanks();"
                                            style="cursor: pointer; background-color: #CCCCCC; height: 17px; padding: 2px; line-height: 9px; font-size: 9px;" 
                                            id="btnSelectRanks" />
                                         <div style="height: 2px;"></div>
                                         <input type="button" value="<<" title="Премахване" onclick="RemoveMilitaryRanks();"
                                            style="cursor: pointer; background-color: #CCCCCC; height: 17px; padding: 2px; line-height: 9px; font-size: 9px;" 
                                            id="btnRemoveRanks" />
                                      </td>
                                      <td style="vertical-align: top;">
                                         <span class="InputLabel" style="font-style: italic; font-size: 0.9em;" id="lblSelectedRanks">Избрани за длъжността</span><br />
                                         <select class="InputField" UnsavedCheckSkipMe="true" multiple="multiple" style="width: 200px; height: 78px;" id="ddSelectedRanks"></select>
                                      </td>
                                   </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="min-height: 17px">
                            <td style="text-align: right; vertical-align: top;">
                                <span id="lblEducation" class="InputLabel">Образование:</span>
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList runat="server" ID="ddEducation" CssClass="InputField" Width="350"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr style="min-height: 17px">
                            <td style="text-align: right; vertical-align: top;">
                                <span id="lblClInformationAccLevelNATO" class="InputLabel">Ниво на достъп до КИ (НАТО):</span>
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList runat="server" ID="ddClInformationAccLevelNATO" CssClass="InputField" Width="200"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr style="min-height: 17px">
                            <td style="text-align: right; vertical-align: top;">
                                <span id="lblClInformationAccLevelBG" class="InputLabel">Ниво на достъп до КИ (РБ):</span>
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList runat="server" ID="ddClInformationAccLevelBG" CssClass="InputField" Width="200"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr style="min-height: 17px">
                            <td style="text-align: right; vertical-align: top;">
                                <span id="lblClInformationAccLevelEU" class="InputLabel">Ниво на достъп до КИ (ЕС):</span>
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList runat="server" ID="ddClInformationAccLevelEU" CssClass="InputField" Width="200"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr style="min-height: 17px">
                            <td style="text-align: right; vertical-align: top;">
                                <span id="lblPositionsCnt" class="InputLabel">Брой за длъжността:</span>
                            </td>
                            <td style="text-align: left;">
                                <input type="text" id="txtPositionsCnt" class="RequiredInputField" style="width: 50px;" maxlength="6" />
                            </td>
                        </tr>
                        <tr style="height: 30px">
                            <td colspan="2">
                                <span id="spanAddVacancyAnnouncePositionManuallyLightBoxMessage" class="ErrorText" style="display: none;">
                                </span>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center;">
                                <table style="margin: 0 auto;">
                                    <tr>
                                        <td>
                                            <div id="btnAddVacancyAnnouncePositionManuallyLightBox" style="display: inline;" onclick="SaveAddVacancyAnnouncePositionManually();"
                                                class="Button">
                                                <i></i>
                                                <div id="btnAddVacancyAnnouncePositionManuallyLightBoxText" style="width: 70px;">
                                                    Запис</div>
                                                <b></b>
                                            </div>
                                            <div id="btnCloseAddVacancyAnnouncePositionManuallyLightBox" style="display: inline;" onclick="HideAddVacancyAnnouncePositionManuallyLightBox();"
                                                class="Button">
                                                <i></i>
                                                <div id="Div5" style="width: 70px;">
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
            <div id="VacancyAnnouncePositionLightBox" class="VacancyAnnouncePositionLightBox"
                style="display: none; text-align: center;">
                <asp:HiddenField ID="hfVacancyAnnouncePositionID" runat="server" />
                <center>
                    <table>
                        <tr>
                            <td colspan="2" style="text-align:center;">
                                <span id='spApplicantPositionTitle' class="HeaderText" style="text-align: center;"></span>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align:center">
                                <span id='spApplicantPositionSubtitle' class="InputLabel" style="text-align: center;"></span>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align:top;">                               
                                <table width="100%" style="text-align: center;">
	                                <colgroup style="width: 70%"></colgroup>
	                                <colgroup style="width: 30%"></colgroup>
	                                <tr style="height: 15px"></tr>
	                                <tr style="min-height: 17px">
		                                <td style="text-align: right; vertical-align: top; white-space: nowrap;">
			                                <span id="edit_lblMilitaryUnit" class="InputLabel">
				                                <%= PMIS.Common.CommonFunctions.GetLabelText("MilitaryUnit")%>:
			                                </span>
		                                </td>
		                                <td style="text-align: left;">
			                                <is:MilitaryUnitSelector ID="edit_msMilitaryUnit" runat="server" DataSourceWebPage="DataSource.aspx"
				                                DataSourceKey="MilitaryUnit" DivMainCss="isDivMainClassRequired" DivListCss="isDivListClass"
				                                DivFullListCss="isDivFullListClass" ResultMaxCount="1000" UnsavedCheckSkipMe="true"
				                                DropDownLimit="3" />
		                                </td>
	                                </tr>
	                                <tr style="min-height: 17px">
		                                <td style="text-align: right; vertical-align: top;">
			                                <span id="edit_lblPosition" class="InputLabel">Длъжност:</span>
		                                </td>
		                                <td style="text-align: left;">
			                                <input type="text" id="edit_txtPosition" class="RequiredInputField" style="width: 200px;" maxlength="300" UnsavedCheckSkipMe="true"/>
		                                </td>
	                                </tr>
	                                <tr style="min-height: 17px">
		                                <td style="text-align: right; vertical-align: top;">
			                                <span id="edit_lblPositionCode" class="InputLabel">Код на длъжността:</span>
		                                </td>
		                                <td style="text-align: left;">
			                                <input type="text" id="edit_txtPositionCode" class="RequiredInputField" style="width: 100px;" maxlength="200" UnsavedCheckSkipMe="true"/>
		                                </td>
	                                </tr>
	                                 <tr style="min-height: 17px">
		                                <td style="text-align: right; vertical-align: top;">
			                                <span id="edit_lblMilitaryRanks" class="InputLabel">Звание:</span>
		                                </td>
		                                <td style="text-align: left;">
			                                <table>
                                               <tr>
                                                  <td style="vertical-align: top;">
                                                     <asp:ListBox runat="server" ID="edit_ddAvailableRanks" UnsavedCheckSkipMe="true" CssClass="InputField" Width="200px" Height="90px" SelectionMode="Multiple"></asp:ListBox>
                                                  </td>
                                                  <td style="vertical-align: middle;">
                                                     <input type="button" value=">>" title="Избор" onclick="EditSelectMilitaryRanks();"
                                                        style="cursor: pointer; background-color: #CCCCCC; height: 17px; padding: 2px; line-height: 9px; font-size: 9px;" 
                                                        id="edit_btnSelectRanks" />
                                                     <div style="height: 2px;"></div>
                                                     <input type="button" value="<<" title="Премахване" onclick="EditRemoveMilitaryRanks();"
                                                        style="cursor: pointer; background-color: #CCCCCC; height: 17px; padding: 2px; line-height: 9px; font-size: 9px;" 
                                                        id="edit_btnRemoveRanks" />
                                                  </td>
                                                  <td style="vertical-align: top;">
                                                     <span class="InputLabel" style="font-style: italic; font-size: 0.9em;" id="edit_lblSelectedRanks">Избрани за длъжността</span><br />
                                                     <select class="InputField" UnsavedCheckSkipMe="true" multiple="multiple" style="width: 200px; height: 78px;" id="edit_ddSelectedRanks"></select>
                                                  </td>
                                               </tr>
                                            </table>
		                                </td>
	                                </tr>
	                                <tr style="min-height: 17px">
		                                <td style="text-align: right; vertical-align: top;">
			                                <span id="edit_lblEducation" class="InputLabel">Образование:</span>
		                                </td>
		                                <td style="text-align: left;">
			                                <asp:DropDownList runat="server" ID="edit_ddEducation" CssClass="InputField" Width="200" UnsavedCheckSkipMe="true"></asp:DropDownList>
		                                </td>
	                                </tr>
	                                <tr style="min-height: 17px">
		                                <td style="text-align: right; vertical-align: top;">
			                                <span id="edit_lblClInformationAccLevelNATO" class="InputLabel">Ниво на достъп до КИ (НАТО):</span>
		                                </td>
		                                <td style="text-align: left;">
			                                <asp:DropDownList runat="server" ID="edit_ddClInformationAccLevelNATO" CssClass="InputField" Width="200" UnsavedCheckSkipMe="true"></asp:DropDownList>
		                                </td>
	                                </tr>
	                                <tr style="min-height: 17px">
		                                <td style="text-align: right; vertical-align: top;">
			                                <span id="edit_lblClInformationAccLevelBG" class="InputLabel">Ниво на достъп до КИ (РБ):</span>
		                                </td>
		                                <td style="text-align: left;">
			                                <asp:DropDownList runat="server" ID="edit_ddClInformationAccLevelBG" CssClass="InputField" Width="200" UnsavedCheckSkipMe="true"></asp:DropDownList>
		                                </td>
	                                </tr>
	                                <tr style="min-height: 17px">
		                                <td style="text-align: right; vertical-align: top;">
			                                <span id="edit_lblClInformationAccLevelEU" class="InputLabel">Ниво на достъп до КИ (ЕС):</span>
		                                </td>
		                                <td style="text-align: left;">
			                                <asp:DropDownList runat="server" ID="edit_ddClInformationAccLevelEU" CssClass="InputField" Width="200" UnsavedCheckSkipMe="true"></asp:DropDownList>
		                                </td>
	                                </tr>
	                                <tr style="min-height: 17px">
		                                <td style="text-align: right; vertical-align: top;">
			                                <span id="edit_lblPositionsCnt" class="InputLabel">Брой за длъжността:</span>
		                                </td>
		                                <td style="text-align: left;">
			                                <input type="text" id="edit_txtPositionsCnt" class="RequiredInputField" style="width: 50px;" maxlength="6" UnsavedCheckSkipMe="true"/>
		                                </td>
	                                </tr>
                                </table>
                                
                            </td>
                            <td style="vertical-align:top;">
                                <table width="80%" style="text-align: center;">
                                    <colgroup style="width: 30%">
                                    </colgroup>
                                    <colgroup style="width: 70%">
                                    </colgroup>
                                    <tr style="height: 15px"></tr>
                                    <tr style="min-height: 17px">
                                        <td style="text-align: right; vertical-align: top; white-space: nowrap;">
                                            <span id="lblResponsibleMilitaryUnit" class="InputLabel">
                                                <%= PMIS.Common.CommonFunctions.GetLabelText("MilitaryUnit")%>
                                                отговорна за конкурса:</span>
                                        </td>
                                        <td style="text-align: left;">
                                            <is:MilitaryUnitSelector ID="msResponsibleMilitaryUnit" runat="server" DataSourceWebPage="DataSource.aspx"
                                                DataSourceKey="MilitaryUnit" DivMainCss="isDivMainClassRequired" DivListCss="isDivListClass"
                                                DivFullListCss="isDivFullListClass" ResultMaxCount="1000" UnsavedCheckSkipMe="true"
                                                DropDownLimit="3" />
                                        </td>
                                    </tr>
                                    <tr style="min-height: 17px">
                                        <td style="text-align: right; vertical-align: top;">
                                            <span id="lblMandatoryRequirements" class="InputLabel">Задължителни изисквания:</span>
                                        </td>
                                        <td style="text-align: left;">
                                            <textarea id="txtMandatoryRequirements" rows="3" class="InputField" style="width: 350px;"></textarea>
                                        </td>
                                    </tr>
                                    <tr style="min-height: 17px">
                                        <td style="text-align: right; vertical-align: top;">
                                            <span id="lblAdditionalRequirements" class="InputLabel">Допълнителни изисквания:</span>
                                        </td>
                                        <td style="text-align: left;">
                                            <textarea id="txtAdditionalRequirements" rows="3" class="InputField" style="width: 350px;"></textarea>
                                        </td>
                                    </tr>
                                    <tr style="min-height: 17px">
                                        <td style="text-align: right; vertical-align: top;">
                                            <span id="lblSpecificRequirements" class="InputLabel">Специфични изисквания:</span>
                                        </td>
                                        <td style="text-align: left;">
                                            <textarea id="txtSpecificRequirements" rows="3" class="InputField" style="width: 350px;"></textarea>
                                        </td>
                                    </tr>
                                    <tr style="min-height: 17px">
                                        <td style="text-align: right; vertical-align: top;">
                                            <span id="lblCompetitionPlaceAndDate" class="InputLabel">Дата и място на конкурса:</span>
                                        </td>
                                        <td style="text-align: left;">
                                            <textarea id="txtCompetitionPlaceAndDate" rows="3" class="InputField" style="width: 350px;"></textarea>
                                        </td>
                                    </tr>
                                    <tr style="min-height: 17px">
                                        <td style="text-align: right;">
                                            <span id="lblContactPhone" class="InputLabel">Тел. за контакт:</span>
                                        </td>
                                        <td style="text-align: left;">
                                            <input id="txtContactPhone" type="text" class="InputField" style="width: 350px;" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align:center;">
                                 <span id="spanVacancyAnnouncePositionLightBoxMessage" class="ErrorText" style="display: none;"></span>&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table style="margin: 0 auto;">
                                    <tr>
                                        <td>
                                            <div id="Div1" style="display: inline;" onclick="SaveVacancyAnnouncePosition();"
                                                class="Button">
                                                <i></i>
                                                <div id="Div2" style="width: 70px;">
                                                    Запис</div>
                                                <b></b>
                                            </div>
                                            <div id="Div3" style="display: inline;" onclick="HideVacancyAnnouncePositionsLightBox();"
                                                class="Button">
                                                <i></i>
                                                <div id="Div4" style="width: 70px;">
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
            <asp:HiddenField ID="hdnLocationHash" runat="server" />
            <asp:HiddenField ID="hdnSavedChanges" runat="server" />
            <asp:HiddenField ID="hdnVacancyAnnounceStatusID" runat="server" Value="" />
            <asp:HiddenField ID="hdnVacancyAnnounceType" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>

    <script type="text/javascript">
        var militarUnitLabel = '<%= PMIS.Common.CommonFunctions.GetLabelText("MilitaryUnit") %>';
    
        window.addEventListener ? window.addEventListener('load', PageLoad, false) : window.attachEvent('onload', PageLoad);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(PageEndRequestHandler);

        //Call this when the page is loaded
        function PageLoad()
        {
            hdnSavedChangesID = '<%= hdnSavedChanges.ClientID %>';

            SetClientTextAreaMaxLength("txtMandatoryRequirements", "1000");
            SetClientTextAreaMaxLength("txtAdditionalRequirements", "1000");
            SetClientTextAreaMaxLength("txtSpecificRequirements", "1000");
            SetClientTextAreaMaxLength("txtCompetitionPlaceAndDate", "2000");

            if (document.getElementById("btnTabPositions") != null)
                JSLoadTab("btnTabPositions");
            else if (document.getElementById("btnTabExams") != null)
                JSLoadTab("btnTabExams");
            else if (document.getElementById("btnTabDocuments") != null)
                JSLoadTab("btnTabDocuments");

        }


        //Client validation of the form
        function ValidateData()
        {
            var res = true;
            var lblMessage = document.getElementById("<%= lblMessage.ClientID %>");
            lblMessage.innerHTML = "";

            var notValidFields = new Array();

            var orderNum = document.getElementById("<%= txtOrderNum.ClientID %>");
            var orderDate = document.getElementById("<%= txtOrderDate.ClientID %>");
            var endDate = document.getElementById("<%= txtEndDate.ClientID %>");
            var maxPositions = document.getElementById("<%= txtMaxPositions.ClientID %>");

            if (orderNum.value.Trim() == "")
            {
                res = false;

                if (orderNum.disabled == true || orderNum.style.display == "none")
                    notValidFields.push("Заповед №");
                else
                    lblMessage.innerHTML += GetErrorMessageMandatory("Заповед №") + "<br />";
            }

            if (orderDate.value.Trim() == "")
            {
                res = false;

                if (orderDate.disabled == true || orderDate.style.display == "none")
                    notValidFields.push("Дата на заповедта");
                else
                    lblMessage.innerHTML += GetErrorMessageMandatory("Дата на заповедта") + "<br />";
            }
            else if (!IsValidDate(orderDate.value))
            {
                res = false;
                lblMessage.innerHTML += GetErrorMessageDate("Дата на заповедта") + "<br />";
            }

            if (endDate.value.Trim() == "")
            {
                res = false;

                if (endDate.disabled == true || endDate.style.display == "none")
                    notValidFields.push("Крайна дата");
                else
                    lblMessage.innerHTML += GetErrorMessageMandatory("Крайна дата") + "<br />";
            }
            else if (!IsValidDate(endDate.value))
            {
                res = false;
                lblMessage.innerHTML += GetErrorMessageDate("Крайна дата") + "<br />";
            }

            if (maxPositions.value.Trim() != "" && !isInt(maxPositions.value))
            {
                res = false;
                lblMessage.innerHTML += GetErrorMessageNumber("Макс. длъжности") + "<br />";
            }

            var notValidFieldsCount = notValidFields.length;

            if (notValidFieldsCount > 0)
            {
                var noRightsMessage = GetErrorMessageNoRights(notValidFields);
                lblMessage.innerHTML += "<br />" + noRightsMessage;
            }

            if (res)
            {
                ForceNoChanges();
                lblMessage.className = "SuccessText";
            }
            else
                lblMessage.className = "ErrorText";

            return res;
        }

        function PageEndRequestHandler()
        {
            SetClientTextAreaMaxLength("txtMandatoryRequirements", "1000");
            SetClientTextAreaMaxLength("txtAdditionalRequirements", "1000");
            SetClientTextAreaMaxLength("txtSpecificRequirements", "1000");
            SetClientTextAreaMaxLength("txtCompetitionPlaceAndDate", "2000");

            ResetTabs();
        }

        //Tabs logic
        //This is the ID of the selected tab (its button)
        var selectedTab = "btnTabPositions";

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
            //Clear any messages
            document.getElementById("<%= lblMessage.ClientID %>").innerHTML = "";

            //Set the previously selected tab as inactive
            if (document.getElementById(selectedTab) != null)
                document.getElementById(selectedTab).className = "InactiveTab";

            //Set the current tab as active
            tab.className = "ActiveTab";
            selectedTab = tab.id;

            //Check if this tab has been already loaded
            //If it hasn't been loaded yet then get its content from the server via AJAX
            if (!IsTabAlreadyVisited())
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
        function IsTabAlreadyVisited()
        {
            if (document.getElementById(selectedTab).getAttribute("IsAlrеadyVisited") &&
          document.getElementById(selectedTab).getAttribute("IsAlrеadyVisited") == "true")
                return true;
            else
                return false;
        }

        //Show the content of the currently selected tab
        function ShowDiv(tab)
        {
            selectedTab = tab;

            //Hide all divs
            document.getElementById("<%=divPositions.ClientID %>").style.display = "none";
            document.getElementById("<%=divExams.ClientID %>").style.display = "none";
            document.getElementById("<%=divDocuments.ClientID %>").style.display = "none";

            //Display the content of the current tab
            var targetDivId = GetTargetDivByTabId(selectedTab);
            document.getElementById(targetDivId).style.display = "";

            //Mark it as visited
            document.getElementById(selectedTab).setAttribute("IsAlrеadyVisited", "true");
        }

        //Load the content of a particular tab via an AJAX call
        function JSLoadTab(selectedTabId)
        {
            var url = "AddEditVacancyAnnounce.aspx?AjaxMethod=JSLoadTab&VacancyAnnounceId=" + document.getElementById("<%= hfVacancyAnnounceID.ClientID %>").value;
            var params = "";
            params += "SelectedTabId=" + selectedTabId;
            var myAJAX = new AJAX(url, true, params, function(xml) { JSLoadTab_CallBack(xml, selectedTabId); });
            myAJAX.Call();
        }

        //When the response is ready then get the loaded HTML and put it on the target div
        function JSLoadTab_CallBack(xml, selectedTabId)
        {
            var targetDivId = GetTargetDivByTabId(selectedTabId);
            document.getElementById(targetDivId).innerHTML = xmlValue(xml, "TabHTML");
            //Show the loaded content
            ShowDiv(selectedTabId);
        }

        //Use this function to get the client id of the content div for a particular tab
        function GetTargetDivByTabId(selectedTabId)
        {
            var targetDivId = "";

            switch (selectedTabId)
            {
                case "btnTabPositions":
                    {
                        targetDivId = "<%=divPositions.ClientID %>";
                        break;
                    }
                case "btnTabExams":
                    {
                        targetDivId = "<%=divExams.ClientID %>";
                        break;
                    }
                case "btnTabDocuments":
                    {
                        targetDivId = "<%=divDocuments.ClientID %>";
                        break;
                    }
            }

            return targetDivId;
        }

        //Reset the tab selection to the first (the default) tab
        function ResetTabs()
        {
            var selectedTab = "btnTabPositions";

            if (document.getElementById("btnTabPositions") != null)
                selectedTab = "btnTabPositions";
            else if (document.getElementById("btnTabExams") != null)
                selectedTab = "btnTabExams";
            else if (document.getElementById("btnTabDocuments") != null)
                selectedTab = "btnTabDocuments";
            else
                return;

            var tab = document.getElementById(selectedTab);

            tab.className = "ActiveTab";
            selectedTab = tab.id;
            JSLoadTab(selectedTab);
        }

        function ShowAddVacancyAnnouncePositionLightBox()
        {
            var url = "AddEditVacancyAnnounce.aspx?AjaxMethod=JSGetAddVacancyAnnouncePositionLightBox";
            var params = "";
            params += "VacancyAnnounceType=" + document.getElementById("<%= hdnVacancyAnnounceType.ClientID %>").value;
            
            function response_handler(xml)
            {
                document.getElementById('divAddVacancyAnnouncePositionLightBoxContent').innerHTML = xmlNodeText(xml.childNodes[0]);
                document.getElementById("HidePage").style.display = "";
                document.getElementById("divAddVacancyAnnouncePositionLightBox").style.display = "";
                CenterLightBox("divAddVacancyAnnouncePositionLightBox");
            }

            var myAJAX = new AJAX(url, true, params, response_handler);
            myAJAX.Call();
        }

        function HideAddVacancyAnnouncePositionLightBox()
        {
            document.getElementById("HidePage").style.display = "none";
            document.getElementById("divAddVacancyAnnouncePositionLightBox").style.display = "none";
        }

        function btnSearch_Click()
        {
           document.getElementById("hdnPageIndex").value = "1"; 
           FilterAddVacancyAnnouncePositionLightBox();
        }

        function FilterAddVacancyAnnouncePositionLightBox()
        {
            var url = "AddEditVacancyAnnounce.aspx?AjaxMethod=JSGetAddVacancyAnnouncePositionLightBox";

            var params = "";
            params += "MilitaryUnitID=" + MilitaryUnitSelectorUtil.GetSelectedValue("MilUnitSelector");
            params += "&PositionName=" + document.getElementById("txtPositionName").value;
            params += "&PageIndex=" + document.getElementById("hdnPageIndex").value;
            params += "&MaxPage=" + document.getElementById("hdnPageMaxPage").value;
            params += "&OrderBy=" + document.getElementById("hdnOrderBy").value;

            function response_handler(xml)
            {
                document.getElementById('divAddVacancyAnnouncePositionLightBoxContent').innerHTML = xmlNodeText(xml.childNodes[0]);
            }

            var myAJAX = new AJAX(url, true, params, response_handler);
            myAJAX.Call();
        }

        function ValidateVacancyAnnouncePositions()
        {
            // variables to prevent showing identical error messages in the lightbox
            var errMsg1 = false;
            var errMsg2 = false;

            var res = true;
            var lblMessage = document.getElementById("lblAddVacancyAnnouncePositionMessage");
            lblMessage.innerHTML = "";
            lblMessage.className = "ErrorText";

            var vacantPositionsCnt = parseInt(document.getElementById("hdnVacantPositionsCounter").value);

            for (var i = 1; i < vacantPositionsCnt; i++)
            {
                var positions = document.getElementById("txtVacantPositions" + i).value;
                var positionCnt = document.getElementById("positionCnt" + i).innerHTML;

                if (TrimString(positions) != "")
                {
                    if (!isInt(positions))
                    {
                        res = false;
                        if (!errMsg1)
                            lblMessage.innerHTML += GetErrorMessageNumber("Избрани позиции") + "<br />";
                        errMsg1 = true;
                    }
                    else if (parseInt(positions) > parseInt(positionCnt))
                    {
                        res = false;
                        if (!errMsg2)
                            lblMessage.innerHTML += "Стойността на полето \"Избрани позиции\" не трябва да надвишава възможния брой позиции<br />";
                        errMsg2 = true;
                    }
                }
            }

            return res;
        }

        function AddVacancyAnnouncePosition()
        {
            if (!ValidateVacancyAnnouncePositions())
            {
                return;
            }

            var url = "AddEditVacancyAnnounce.aspx?AjaxMethod=JSAddVacancyAnnouncePositions";

            var params = "";

            var vacantPositionsCnt = parseInt(document.getElementById("hdnVacantPositionsCounter").value);

            params += "VacancyAnnounceID=" + document.getElementById("<%= hfVacancyAnnounceID.ClientID %>").value;
            params += "&Count=" + vacantPositionsCnt;

            for (var i = 1; i < vacantPositionsCnt; i++)
            {
                var militaryUnitId = document.getElementById("hdnMilUnitID" + i).value;
                var positionCode = document.getElementById("positionCode" + i).innerHTML;
                var positions = document.getElementById("txtVacantPositions" + i).value;
                var positionName = document.getElementById("hdnPositionName" + i).value;
                var ClInformationAccLevelNATO = document.getElementById("hdnClInformationAccLevelNATO" + i).value;
                var ClInformationAccLevelBG = document.getElementById("hdnClInformationAccLevelBG" + i).value;
                var ClInformationAccLevelEU = document.getElementById("hdnClInformationAccLevelEU" + i).value;

                if (TrimString(positions) != "")
                {
                    params += "&MilitaryUnitID" + i + "=" + militaryUnitId;
                    params += "&PositionCode" + i + "=" + positionCode;
                    params += "&Positions" + i + "=" + positions;
                    params += "&PositionName" + i + "=" + positionName;
                    params += "&ClInformationAccLevelNATO" + i + "=" + ClInformationAccLevelNATO;
                    params += "&ClInformationAccLevelBG" + i + "=" + ClInformationAccLevelBG;
                    params += "&ClInformationAccLevelEU" + i + "=" + ClInformationAccLevelEU;
                }
            }

            function response_handler(xml)
            {
                if (xmlNodeText(xml.childNodes[0]) != "NO")
                {
                    document.getElementById("<%=divPositions.ClientID %>").innerHTML = xmlNodeText(xml.childNodes[0]);
                    HideAddVacancyAnnouncePositionLightBox();
                }
            }

            var myAJAX = new AJAX(url, true, params, response_handler);
            myAJAX.Call();
        }

        function BtnPagingClick(objectName)
        {
            hdnPageIdx = document.getElementById('hdnPageIndex');
            hdnMaxPage = document.getElementById('hdnPageMaxPage');
            switch (objectName)
            {
                case "btnFirst":
                    hdnPageIdx.value = 1;

                    FilterAddVacancyAnnouncePositionLightBox();
                    break;

                case "btnPrev":
                    pageIdx = parseInt(hdnPageIdx.value);

                    if (pageIdx > 1)
                    {
                        pageIdx--;
                        hdnPageIdx.value = pageIdx;
                        FilterAddVacancyAnnouncePositionLightBox();
                    }

                    break;

                case "btnNext":
                    pageIdx = parseInt(hdnPageIdx.value);
                    maxPage = parseInt(hdnMaxPage.value);

                    if (pageIdx < maxPage)
                    {
                        pageIdx++;
                        hdnPageIdx.value = pageIdx;
                        FilterAddVacancyAnnouncePositionLightBox();
                    }
                    break;


                case "btnLast":
                    hdnPageIdx.value = hdnMaxPage.value;

                    FilterAddVacancyAnnouncePositionLightBox();
                    break;

                case "btnPageGo":
                    maxPage = parseInt(hdnMaxPage.value);
                    goToPage = parseInt(document.getElementById('txtTableGotoPage').value);

                    if (isInt(goToPage) && goToPage > 0 && goToPage <= maxPage)
                    {
                        hdnPageIdx.value = goToPage;
                        FilterAddVacancyAnnouncePositionLightBox();
                    }
                    break;

                default:
                    break;
            }

        }

        function SortTableBy(sort)
        {
            hdnPageIdx = document.getElementById('hdnPageIndex');
            hdnOrderBy = document.getElementById("hdnOrderBy");
            orderBy = parseInt(hdnOrderBy.value);

            if (orderBy == sort)
            {
                sort = sort + 100;
            }

            hdnOrderBy.value = sort;
            hdnPageIdx.value = 1; //We go to 1st page

            FilterAddVacancyAnnouncePositionLightBox();
        }

        function RemoveExam(examId, examName)
        {
            YesNoDialog('Желаете ли да изтриете изпит ' + examName + '?', ConfirmYes, null);

            function ConfirmYes()
            {
                var url = "AddEditVacancyAnnounce.aspx?AjaxMethod=JSRemoveExam";
                var params = "";
                params += "&VacancyAnnounceId=" + document.getElementById("<%= hfVacancyAnnounceID.ClientID %>").value;
                params += "&examId=" + examId;
                
                function response_handler(xml)
                {
                    if (xmlNodeText(xml.childNodes[0]) != "NO")
                    {
                        document.getElementById("<%=divExams.ClientID %>").innerHTML = xmlNodeText(xml.childNodes[0]);
                    }
                }

                var myAJAX = new AJAX(url, true, params, response_handler);
                myAJAX.Call();
            }
        }
        function RemoveDocument(documentId, documentName)
        {
            YesNoDialog('Желаете ли да изтриете документ ' + documentName + '?', ConfirmYes, null);

            function ConfirmYes()
            {
                var url = "AddEditVacancyAnnounce.aspx?AjaxMethod=JSRemoveDocument";
                var params = "";
                params += "&VacancyAnnounceId=" + document.getElementById("<%= hfVacancyAnnounceID.ClientID %>").value;
                params += "&documentId=" + documentId;
                
                function response_handler(xml)
                {
                    if (xmlNodeText(xml.childNodes[0]) != "NO")
                    {
                        document.getElementById("<%=divDocuments.ClientID %>").innerHTML = xmlNodeText(xml.childNodes[0]);
                    }
                }

                var myAJAX = new AJAX(url, true, params, response_handler);
                myAJAX.Call();
            }
        }

        function ShowExamLightTable()
        {
            var url = "AddEditVacancyAnnounce.aspx?AjaxMethod=JSGetExamLightBoxContent";
            var params;
            params += "&VacancyAnnounceId=" + document.getElementById("<%= hfVacancyAnnounceID.ClientID %>").value;
            
            function response_handler(xml)
            {
                document.getElementById('divExamsLightBoxContent').innerHTML = xmlNodeText(xml.childNodes[0]);
                ShowExamLightBox();
            }

            var myAJAX = new AJAX(url, true, params, response_handler);
            myAJAX.Call();
        }

        function ShowDocumentLightTable()
        {
            var url = "AddEditVacancyAnnounce.aspx?AjaxMethod=JSGetDocumentLightBoxContent";
            var params;
            params += "&VacancyAnnounceId=" + document.getElementById("<%= hfVacancyAnnounceID.ClientID %>").value;
            
            function response_handler(xml)
            {
                document.getElementById('divDocumentsLightBoxContent').innerHTML = xmlNodeText(xml.childNodes[0]);
                ShowDocumentLightBox();
            }

            var myAJAX = new AJAX(url, true, params, response_handler);
            myAJAX.Call();
        }

        // Shows the light box Exams and "disable" rest of the page
        function ShowExamLightBox()
        {
            document.getElementById("HidePage").style.display = "";
            document.getElementById("divExamsLightBox").style.display = "";
            CenterLightBox("divExamsLightBox");
        }

        // Close the light box Exams and clear its content
        function HideExamLightBox()
        {
            document.getElementById("HidePage").style.display = "none";
            document.getElementById("divExamsLightBox").style.display = "none";
        }

        // Shows the light box Documents and "disable" rest of the page
        function ShowDocumentLightBox()
        {
            document.getElementById("HidePage").style.display = "";
            document.getElementById("divDocumentsLightBox").style.display = "";
            CenterLightBox("divDocumentsLightBox");
        }

        // Close the light box Documents and clear its content
        function HideDocumentLightBox()
        {
            document.getElementById("HidePage").style.display = "none";
            document.getElementById("divDocumentsLightBox").style.display = "none";
        }


        function AddExams()
        {
            //Get selected element to add
            var selectedExamIds = GetListBoxValues("choosenExamListItems");

            var url = "AddEditVacancyAnnounce.aspx?AjaxMethod=JSAddExam";
            var params;
            params += "&VacancyAnnounceId=" + document.getElementById("<%= hfVacancyAnnounceID.ClientID %>").value;
            params += "&examIds=" + selectedExamIds;
            
            function response_handler(xml)
            {
                if (xmlNodeText(xml.childNodes[0]) != "NO")
                {
                    document.getElementById("<%=divExams.ClientID %>").innerHTML = xmlNodeText(xml.childNodes[0]);
                    HideExamLightBox();
                }
            }

            var myAJAX = new AJAX(url, true, params, response_handler);
            myAJAX.Call();
        }

        function AddDocuments()
        {
            //Get selected element to add
            var selectedDocumentIds = GetListBoxValues("choosenDocumentListItems");

            var url = "AddEditVacancyAnnounce.aspx?AjaxMethod=JSAddDocument";
            var params;
            params += "&VacancyAnnounceId=" + document.getElementById("<%= hfVacancyAnnounceID.ClientID %>").value;
            params += "&documentIds=" + selectedDocumentIds;
            
            function response_handler(xml)
            {
                if (xmlNodeText(xml.childNodes[0]) != "NO")
                {
                    document.getElementById("<%=divDocuments.ClientID %>").innerHTML = xmlNodeText(xml.childNodes[0]);
                    HideDocumentLightBox();
                }
            }

            var myAJAX = new AJAX(url, true, params, response_handler);
            myAJAX.Call();
        }

        function SelectedItem(ddlObject)
        {
            var selectedItemId;
            var cheked = false

            var i = 0;
            do
            {
                if (ddlObject[i].selected)
                {
                    cheked = true;
                    selectedItemId = ddlObject[i].id

                }
                i++;

            } while (!cheked)

            return selectedItemId;
        }

        function ddlLightBoxChanged(object)
        {

            //Set enable/Disable Add Button
            var targetButtonDiv;
            switch (object.id)
            {
                case "examListItems":
                    {
                        targetButtonDiv = document.getElementById("btnDivAddExam");
                        break;
                    }
                case "documentListItems":
                    {
                        targetButtonDiv = document.getElementById("btnDivAddDocument");
                        break;
                    }
                default:
                    break;
            }

            if (SelectedItem(object) == -1)
            {
                targetButtonDiv.disabled = true;
                targetButtonDiv.setAttribute("class", "DisabledButton");
            }
            else
            {
                targetButtonDiv.disabled = false;
                targetButtonDiv.setAttribute("class", "Button");
            }


        }

        var initVacancyAnnouncePositionDialogHeight = 0;

        function ShowVacancyAnnouncePositionLightBox(vacancyAnnouncePositionID)
        {
            if (vacancyAnnouncePositionID != 0) // gets current values if editing vacancy announce position
            {
                var url = "AddEditVacancyAnnounce.aspx?AjaxMethod=JSGetVacancyAnnouncePosition";

                var params = "VacancyAnnouncePositionID=" + vacancyAnnouncePositionID;

                function response_handler(xml)
                {
                    document.getElementById("spApplicantPositionTitle").innerHTML = "Длъжност \"" + xmlValue(xml, "PositionName") + "\"";
                    document.getElementById("spApplicantPositionSubtitle").innerHTML = "Код: " + "<span class='ReadOnlyValue'>" + xmlValue(xml, "PositionCode") + "</span>" + "&nbsp;&nbsp;&nbsp;ВПН/Структура: " + "<span class='ReadOnlyValue'>" + xmlValue(xml, "MilitaryUnitName") + "</span>";
                    MilitaryUnitSelectorUtil.SetSelectedValue("<%= msResponsibleMilitaryUnit.ClientID %>", xmlValue(xml, "ResponsibleMilitaryUnitID"));
                    MilitaryUnitSelectorUtil.SetSelectedText("<%= msResponsibleMilitaryUnit.ClientID %>", xmlValue(xml, "ResponsibleMilitaryUnitName"));
                    document.getElementById("txtMandatoryRequirements").value = xmlValue(xml, "MandatoryRequirements");
                    document.getElementById("txtAdditionalRequirements").value = xmlValue(xml, "AdditionalRequirements");
                    document.getElementById("txtSpecificRequirements").value = xmlValue(xml, "SpecificRequirements");
                    document.getElementById("txtCompetitionPlaceAndDate").value = xmlValue(xml, "CompetitionPlaceAndDate");
                    document.getElementById("txtContactPhone").value = xmlValue(xml, "ContactPhone");


                    MilitaryUnitSelectorUtil.SetSelectedValue("<%= edit_msMilitaryUnit.ClientID %>", xmlValue(xml, "MilitaryUnitID"));
                    MilitaryUnitSelectorUtil.SetSelectedText("<%= edit_msMilitaryUnit.ClientID %>", xmlValue(xml, "MilitaryUnitName"));
                    document.getElementById("edit_txtPosition").value = xmlValue(xml, "PositionName");
                    document.getElementById("edit_txtPositionCode").value = xmlValue(xml, "PositionCode");
                    document.getElementById("<%= edit_ddEducation.ClientID %>").value = xmlValue(xml, "EducationID");

                    var militaryRanks = xml.getElementsByTagName("MilitaryRanks")[0].getElementsByTagName("Rank");
                    for (var i = 0; i < militaryRanks.length; i++) {
                        var rankId = xmlValue(militaryRanks[i], "Id");
                        var rankName = xmlValue(militaryRanks[i], "DisplayName");

                        AddToSelectList(document.getElementById("edit_ddSelectedRanks"), rankId, rankName, true);
                    }
                    
                    var dd = document.getElementById("<%= edit_ddClInformationAccLevelNATO.ClientID %>");
                    var option = xmlValue(xml, "ClInformationAccLevelNATO");
                    for (var i = 0; i < dd.options.length;  i++) {
                        if (dd.options[i].text == option) {
                            dd.value = dd.options[i].value;
                            break;
                        }
                    }

                    dd = document.getElementById("<%= edit_ddClInformationAccLevelBG.ClientID %>");
                    option = xmlValue(xml, "ClInformationAccLevelBG");
                    for (var i = 0; i < dd.options.length; i++) {
                        if (dd.options[i].text == option) {
                            dd.value = dd.options[i].value;
                            break;
                        }
                    }

                    dd = document.getElementById("<%= edit_ddClInformationAccLevelEU.ClientID %>");
                    option = xmlValue(xml, "ClInformationAccLevelEU");
                    for (var i = 0; i < dd.options.length; i++) {
                        if (dd.options[i].text == option) {
                            dd.value = dd.options[i].value;
                            break;
                        }
                    }

                    document.getElementById("edit_txtPositionsCnt").value = xmlValue(xml, "PositionsCnt");
                }

                var myAJAX = new AJAX(url, true, params, response_handler);
                myAJAX.Call();
            }

            document.getElementById("<%= hfVacancyAnnouncePositionID.ClientID %>").value = vacancyAnnouncePositionID; // setting vacancy announce position ID(0 - if new vacancy announce position)

            // clean message label in the light box and hide it
            document.getElementById("spanVacancyAnnouncePositionLightBoxMessage").style.display = "none";
            document.getElementById("spanVacancyAnnouncePositionLightBoxMessage").innerHTML = "";

            setTimeout("ShowVAPositionLightBox()", 300);

            initVacancyAnnouncePositionDialogHeight = document.getElementById("VacancyAnnouncePositionLightBox").offsetHeight;
        }

        // Show the light box
        function ShowVAPositionLightBox()
        {
            //shows the light box and "disable" rest of the page
            document.getElementById("HidePage").style.display = "";
            document.getElementById("VacancyAnnouncePositionLightBox").style.display = "";
            CenterLightBox("VacancyAnnouncePositionLightBox");
        }

        // Close the light box
        function HideVacancyAnnouncePositionsLightBox()
        {
            document.getElementById("HidePage").style.display = "none";
            document.getElementById("VacancyAnnouncePositionLightBox").style.display = "none";

            MilitaryUnitSelectorUtil.SetSelectedValue("<%= msResponsibleMilitaryUnit.ClientID %>", optionChooseOneValue);
            MilitaryUnitSelectorUtil.SetSelectedText("<%= msResponsibleMilitaryUnit.ClientID %>", "");
            document.getElementById("<%= edit_ddAvailableRanks.ClientID %>").value = "";                        
            document.getElementById("txtMandatoryRequirements").value = "";
            document.getElementById("txtAdditionalRequirements").value = "";
            document.getElementById("txtSpecificRequirements").value = "";
            document.getElementById("txtCompetitionPlaceAndDate").value = "";
            document.getElementById("txtContactPhone").value = "";

            ClearSelectList(document.getElementById("edit_ddSelectedRanks"), true);                                                    
        }

        // Saves vacancy announce through ajax request, if light box values are valid, or displays generated error messages
        function SaveVacancyAnnouncePosition() {

            if (!ValidateEditVacancyAnnouncePositions()) {
                return;
            }
        
            var url = "AddEditVacancyAnnounce.aspx?AjaxMethod=JSSaveVacancyAnnouncePosition";

            var responsibleMilitaryUnitID = "";
            if (MilitaryUnitSelectorUtil.GetSelectedValue("<%= msResponsibleMilitaryUnit.ClientID %>") != optionChooseOneValue)
                responsibleMilitaryUnitID = MilitaryUnitSelectorUtil.GetSelectedValue("<%= msResponsibleMilitaryUnit.ClientID %>");

            var params = "VacancyAnnounceID=" + document.getElementById("<%= hfVacancyAnnounceID.ClientID %>").value +
                       "&VacancyAnnouncePositionID=" + document.getElementById("<%= hfVacancyAnnouncePositionID.ClientID %>").value +
                       "&ResponsibleMilitaryUnitID=" + responsibleMilitaryUnitID +
                       "&MandatoryRequirements=" + custEncodeURI(TrimString(document.getElementById("txtMandatoryRequirements").value)) +
                       "&AdditionalRequirements=" + custEncodeURI(TrimString(document.getElementById("txtAdditionalRequirements").value)) +
                       "&SpecificRequirements=" + custEncodeURI(TrimString(document.getElementById("txtSpecificRequirements").value)) +
                       "&CompetitionPlaceAndDate=" + custEncodeURI(TrimString(document.getElementById("txtCompetitionPlaceAndDate").value)) +
                       "&ContactPhone=" + custEncodeURI(TrimString(document.getElementById("txtContactPhone").value)) +

                       "&MilitaryUnitID=" + MilitaryUnitSelectorUtil.GetSelectedValue("<%= edit_msMilitaryUnit.ClientID %>") +
				   "&PositionName=" + custEncodeURI(TrimString(document.getElementById("edit_txtPosition").value)) +
				   "&PositionCode=" + custEncodeURI(TrimString(document.getElementById("edit_txtPositionCode").value)) +
				   "&EducationID=" + document.getElementById("<%= edit_ddEducation.ClientID %>").value +
				   "&ClInformationAccLevelNATO_ID=" + document.getElementById("<%= edit_ddClInformationAccLevelNATO.ClientID %>").value +
				   "&ClInformationAccLevelBG_ID=" + document.getElementById("<%= edit_ddClInformationAccLevelBG.ClientID %>").value +
				   "&ClInformationAccLevelEU_ID=" + document.getElementById("<%= edit_ddClInformationAccLevelEU.ClientID %>").value +
				   "&PositionsCnt=" + custEncodeURI(TrimString(document.getElementById("edit_txtPositionsCnt").value));

            var ranks = document.getElementById("edit_ddSelectedRanks");

            for (var i = 0; i < ranks.options.length; i++) {
                var rankIdx = i + 1;

                params += "&RankId_" + rankIdx + "=" + ranks.options[i].value;
                params += "&RankDisplayText_" + rankIdx + "=" + custEncodeURI(ranks.options[i].text);
            }

            params += "&RanksCnt=" + ranks.options.length;

            function response_handler(xml)
            {

                var hideDialog = true;
                var resultMsg = xmlValue(xml, "response");
                if (resultMsg == "ERROR")
                {
                    var lightBoxMessage = document.getElementById("spanVacancyAnnouncePositionLightBoxMessage");
                    lightBoxMessage.innerHTML = "Грешка при запис на данните";
                    lightBoxMessage.style.display = "";
                    hideDialog = false;
                    lightBoxMessage.innerHTML = resultMsg;
                }
                else
                {
                    document.getElementById("<%=divPositions.ClientID %>").innerHTML = xmlNodeText(xml.childNodes[0]);
                }

                if (hideDialog)
                    HideVacancyAnnouncePositionsLightBox();
            }

            var myAJAX = new AJAX(url, true, params, response_handler);
            myAJAX.Call();
        }

        function ValidateEditVacancyAnnouncePositions() {
            var res = true;

            var lblMessage = document.getElementById("spanVacancyAnnouncePositionLightBoxMessage");
            lblMessage.innerHTML = "";

            var notValidFields = new Array();

            var txtPosition = document.getElementById("edit_txtPosition");
            var txtPositionCode = document.getElementById("edit_txtPositionCode");
            var txtPositionsCnt = document.getElementById("edit_txtPositionsCnt");

            if (MilitaryUnitSelectorUtil.GetSelectedValue("<%= edit_msMilitaryUnit.ClientID %>") == optionChooseOneValue) {
                res = false;

                if (MilitaryUnitSelectorUtil.IsDisabled("<%= edit_msMilitaryUnit.ClientID %>") || MilitaryUnitSelectorUtil.IsHidden("<%= edit_msMilitaryUnit.ClientID %>"))
                    notValidFields.push(militarUnitLabel);
                else
                    lblMessage.innerHTML += GetErrorMessageMandatory(militarUnitLabel) + "<br />";
            }

            if (txtPosition.value.Trim() == "") {
                res = false;

                if (txtPosition.disabled == true || txtPosition.style.display == "none")
                    notValidFields.push("Длъжност");
                else
                    lblMessage.innerHTML += GetErrorMessageMandatory("Длъжност") + "<br />";
            }

            if (txtPositionCode.value.Trim() == "") {
                res = false;

                if (txtPositionCode.disabled == true || txtPositionCode.style.display == "none")
                    notValidFields.push("Код на длъжността");
                else
                    lblMessage.innerHTML += GetErrorMessageMandatory("Код на длъжността") + "<br />";
            }

            if (txtPositionsCnt.value.Trim() == "") {
                res = false;

                if (txtPositionsCnt.disabled == true || txtPositionsCnt.style.display == "none")
                    notValidFields.push("Брой за длъжността");
                else
                    lblMessage.innerHTML += GetErrorMessageMandatory("Брой за длъжността") + "<br />";
            }
            else {
                if (!isInt(txtPositionsCnt.value) || parseInt(txtPositionsCnt.value) <= 0) {
                    res = false;
                    lblMessage.innerHTML += GetErrorMessageNumber("Брой за длъжността") + "<br />";
                }
            }

            var notValidFieldsCount = notValidFields.length;

            if (notValidFieldsCount > 0) {
                var noRightsMessage = GetErrorMessageNoRights(notValidFields);
                lblMessage.innerHTML += "<br />" + noRightsMessage;
            }

            if (res) {
                lblMessage.className = "SuccessText";
            }
            else {
                lblMessage.className = "ErrorText";
                lblMessage.style.display = "";
            }

            return res;
        }
        
        // Delete vacancy announce position through ajax request
        function DeleteVacancyAnnouncePosition(vacancyAnnouncePositionID)
        {
            YesNoDialog('Желаете ли да изтриете длъжността?', ConfirmYes, null);

            function ConfirmYes()
            {
                var url = "AddEditVacancyAnnounce.aspx?AjaxMethod=JSDeleteVacancyAnnouncePosition&VacancyAnnounceID=" + document.getElementById("<%= hfVacancyAnnounceID.ClientID %>").value;
                var params = "";
                params += "VacancyAnnouncePositionID=" + vacancyAnnouncePositionID;
                
                function response_handler(xml)
                {
                    if (xmlValue(xml, "response") == "ERROR")
                    {
                         var lightBoxMessage = document.getElementById("<%= lblMessage.ClientID %>");
                         lightBoxMessage.className = "ErrorText";
                         lightBoxMessage.innerHTML = "Грешка при запис на данните";
                         lightBoxMessage.style.display = "";
                        
                    }
                    else
                    {
                        document.getElementById("<%=divPositions.ClientID %>").innerHTML = xmlNodeText(xml.childNodes[0]);
                    }
                }

                var myAJAX = new AJAX(url, true, params, response_handler);
                myAJAX.Call();
            }
        }

        function CheckExamListItems()
        {
            var btnDivAddExam = document.getElementById("btnDivAddExam");
            if (document.getElementById("choosenExamListItems").options.length > 0)
            {
                btnDivAddExam.disabled = false;
                btnDivAddExam.setAttribute("class", "Button");
            }
            else
            {
                btnDivAddExam.disabled = true;
                btnDivAddExam.setAttribute("class", "DisabledButton");
            }
        }

        function ChooseAllExams()
        {
            TransferListBoxAllItems("availExamListItems", "choosenExamListItems");

            CheckExamListItems();
        }

        function ChooseSelectedExams()
        {
            TransferListBoxSelectedItems("availExamListItems", "choosenExamListItems");

            CheckExamListItems();
        }

        function RemoveSelectedExams()
        {
            TransferListBoxSelectedItems("choosenExamListItems", "availExamListItems");

            CheckExamListItems();
        }

        function RemoveAllExams()
        {
            TransferListBoxAllItems("choosenExamListItems", "availExamListItems");

            CheckExamListItems();
        }

        function CheckDocumentListItems()
        {
            var btnDivAddDocument = document.getElementById("btnDivAddDocument");
            if (document.getElementById("choosenDocumentListItems").options.length > 0)
            {
                btnDivAddDocument.disabled = false;
                btnDivAddDocument.setAttribute("class", "Button");
            }
            else
            {
                btnDivAddDocument.disabled = true;
                btnDivAddDocument.setAttribute("class", "DisabledButton");
            }
        }

        function ChooseAllDocuments()
        {
            TransferListBoxAllItems("availDocumentListItems", "choosenDocumentListItems");

            CheckDocumentListItems();
        }

        function ChooseSelectedDocuments()
        {
            TransferListBoxSelectedItems("availDocumentListItems", "choosenDocumentListItems");

            CheckDocumentListItems();
        }

        function RemoveSelectedDocuments()
        {
            TransferListBoxSelectedItems("choosenDocumentListItems", "availDocumentListItems");

            CheckDocumentListItems();
        }

        function RemoveAllDocuments()
        {
            TransferListBoxAllItems("choosenDocumentListItems", "availDocumentListItems");

            CheckDocumentListItems();
        }


        function ShowAddVacancyAnnouncePositionManuallyLightBox() {
            MilitaryUnitSelectorUtil.SetSelectedValue("<%= msMilitaryUnit.ClientID %>", optionChooseOneValue);
            MilitaryUnitSelectorUtil.SetSelectedText("<%= msMilitaryUnit.ClientID %>", "");
            document.getElementById("txtPosition").value = "";
            document.getElementById("txtPositionCode").value = "";                                                                                                                                    
            document.getElementById("<%= ddAvailableRanks.ClientID %>").value = "";            
            document.getElementById("<%= ddEducation.ClientID %>").value = optionChooseOneValue;
            document.getElementById("<%= ddClInformationAccLevelNATO.ClientID %>").value = optionChooseOneValue;
            document.getElementById("<%= ddClInformationAccLevelBG.ClientID %>").value = optionChooseOneValue;
            document.getElementById("<%= ddClInformationAccLevelEU.ClientID %>").value = optionChooseOneValue;
            document.getElementById("txtPositionsCnt").value = "";

            ClearSelectList(document.getElementById("ddSelectedRanks"), true);                                        
        
            document.getElementById("HidePage").style.display = "";
            document.getElementById("divAddVacancyAnnouncePositionManuallyLightBox").style.display = "";
            CenterLightBox("divAddVacancyAnnouncePositionManuallyLightBox");
        }

        function HideAddVacancyAnnouncePositionManuallyLightBox() {
            document.getElementById("HidePage").style.display = "none";
            document.getElementById("divAddVacancyAnnouncePositionManuallyLightBox").style.display = "none";            
        }

        function SaveAddVacancyAnnouncePositionManually() {
            if (!ValidateAddVacancyAnnouncePositionManually()) {
                return;
            }
        
            var url = "AddEditVacancyAnnounce.aspx?AjaxMethod=JSSaveAddVacancyAnnouncePositionManually";

            var params = "VacancyAnnounceID=" + document.getElementById("<%= hfVacancyAnnounceID.ClientID %>").value +
                         "&MilitaryUnitID=" + MilitaryUnitSelectorUtil.GetSelectedValue("<%= msMilitaryUnit.ClientID %>") +
                         "&PositionName=" + custEncodeURI(TrimString(document.getElementById("txtPosition").value)) +
                         "&PositionCode=" + custEncodeURI(TrimString(document.getElementById("txtPositionCode").value)) +
                         "&EducationID=" + document.getElementById("<%= ddEducation.ClientID %>").value +
                         "&ClInformationAccLevelNATO_ID=" + document.getElementById("<%= ddClInformationAccLevelNATO.ClientID %>").value +
                         "&ClInformationAccLevelBG_ID=" + document.getElementById("<%= ddClInformationAccLevelBG.ClientID %>").value +
                         "&ClInformationAccLevelEU_ID=" + document.getElementById("<%= ddClInformationAccLevelEU.ClientID %>").value +
                         "&PositionsCnt=" + custEncodeURI(TrimString(document.getElementById("txtPositionsCnt").value));

            var ranks = document.getElementById("ddSelectedRanks");

            for (var i = 0; i < ranks.options.length; i++) {
                var rankIdx = i + 1;

                params += "&RankId_" + rankIdx + "=" + ranks.options[i].value;
                params += "&RankDisplayText_" + rankIdx + "=" + custEncodeURI(ranks.options[i].text);
            }

            params += "&RanksCnt=" + ranks.options.length;

            
            function response_handler(xml) {
                var hideDialog = true;
                var resultMsg = xmlValue(xml, "response");
                if (resultMsg == "ERROR") {
                    var lightBoxMessage = document.getElementById("spanAddVacancyAnnouncePositionManuallyLightBoxMessage");
                    lightBoxMessage.innerHTML = "Грешка при запис на данните";
                    lightBoxMessage.style.display = "";
                    hideDialog = false;
                    lightBoxMessage.innerHTML = resultMsg;
                }
                else {
                    document.getElementById("<%=divPositions.ClientID %>").innerHTML = xmlNodeText(xml.childNodes[0]);
                }

                if (hideDialog)
                    HideAddVacancyAnnouncePositionManuallyLightBox();
            }

            var myAJAX = new AJAX(url, true, params, response_handler);
            myAJAX.Call();
        }

        function ValidateAddVacancyAnnouncePositionManually() {
            var res = true;

            var lblMessage = document.getElementById("spanAddVacancyAnnouncePositionManuallyLightBoxMessage");
            lblMessage.innerHTML = "";

            var notValidFields = new Array();

            var txtPosition = document.getElementById("txtPosition");
            var txtPositionCode = document.getElementById("txtPositionCode");
            var txtPositionsCnt = document.getElementById("txtPositionsCnt");

            if (MilitaryUnitSelectorUtil.GetSelectedValue("<%= msMilitaryUnit.ClientID %>") == optionChooseOneValue) {
                res = false;

                if (MilitaryUnitSelectorUtil.IsDisabled("<%= msMilitaryUnit.ClientID %>") || MilitaryUnitSelectorUtil.IsHidden("<%= msMilitaryUnit.ClientID %>"))
                    notValidFields.push(militarUnitLabel);
                else
                    lblMessage.innerHTML += GetErrorMessageMandatory(militarUnitLabel) + "<br />";
            }

            if (txtPosition.value.Trim() == "") {
                res = false;

                if (txtPosition.disabled == true || txtPosition.style.display == "none")
                    notValidFields.push("Длъжност");
                else
                    lblMessage.innerHTML += GetErrorMessageMandatory("Длъжност") + "<br />";
            }

            if (txtPositionCode.value.Trim() == "") {
                res = false;

                if (txtPositionCode.disabled == true || txtPositionCode.style.display == "none")
                    notValidFields.push("Код на длъжността");
                else
                    lblMessage.innerHTML += GetErrorMessageMandatory("Код на длъжността") + "<br />";
            }

            if (txtPositionsCnt.value.Trim() == "") {
                res = false;

                if (txtPositionsCnt.disabled == true || txtPositionsCnt.style.display == "none")
                    notValidFields.push("Брой за длъжността");
                else
                    lblMessage.innerHTML += GetErrorMessageMandatory("Брой за длъжността") + "<br />";
            }
            else {
                if (!isInt(txtPositionsCnt.value) || parseInt(txtPositionsCnt.value) <= 0) {
                    res = false;
                    lblMessage.innerHTML += GetErrorMessageNumber("Брой за длъжността") + "<br />";
                }
            }

            var notValidFieldsCount = notValidFields.length;

            if (notValidFieldsCount > 0) {
                var noRightsMessage = GetErrorMessageNoRights(notValidFields);
                lblMessage.innerHTML += "<br />" + noRightsMessage;
            }

            if (res) {
                lblMessage.className = "SuccessText";
            }
            else {
                lblMessage.className = "ErrorText";
                lblMessage.style.display = "";
            }

            return res;
        }

        function SelectMilitaryRanks() {
            var source = document.getElementById("<%= ddAvailableRanks.ClientID %>");
            var target = document.getElementById("ddSelectedRanks");

            var wasTargetEmpty = target.options.length == 0;
            var addedCnt = 0;

            for (var i = 0; i < source.options.length; i++) {
                if (source.options[i].selected) {
                    var found = false;

                    for (var j = 0; j < target.options.length; j++) {
                        if (target.options[j].value == source.options[i].value) {
                            found = true;
                            break;
                        }
                    }

                    if (!found) {
                        var newOption = new Option();
                        newOption.text = source.options[i].text;
                        newOption.value = source.options[i].value;
                        newOption.title = newOption.text;

                        target.options[target.length] = newOption;

                        addedCnt++;
                    }
                }
            }
        }

        function RemoveMilitaryRanks() {
            var source = document.getElementById("ddSelectedRanks");

            while (source.options.selectedIndex >= 0) {
                source.remove(source.options.selectedIndex);
            }
        }

        function EditSelectMilitaryRanks() {
            var source = document.getElementById("<%= edit_ddAvailableRanks.ClientID %>");
            var target = document.getElementById("edit_ddSelectedRanks");

            var wasTargetEmpty = target.options.length == 0;
            var addedCnt = 0;

            for (var i = 0; i < source.options.length; i++) {
                if (source.options[i].selected) {
                    var found = false;

                    for (var j = 0; j < target.options.length; j++) {
                        if (target.options[j].value == source.options[i].value) {
                            found = true;
                            break;
                        }
                    }

                    if (!found) {
                        var newOption = new Option();
                        newOption.text = source.options[i].text;
                        newOption.value = source.options[i].value;
                        newOption.title = newOption.text;

                        target.options[target.length] = newOption;

                        addedCnt++;
                    }
                }
            }
        }

        function EditRemoveMilitaryRanks() {
            var source = document.getElementById("edit_ddSelectedRanks");

            while (source.options.selectedIndex >= 0) {
                source.remove(source.options.selectedIndex);
            }
        }
    </script>
</asp:Content>
