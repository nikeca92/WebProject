<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="AddEditEquipmentReservistsRequest.aspx.cs" Inherits="PMIS.Reserve.ContentPages.AddEditEquipmentReservistsRequest" %>

<%@ Register Assembly="MilitaryUnitSelector" TagPrefix="is" Namespace="MilitaryUnitSelector" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<style type="text/css">
    
.AddRequestCommandLightBox
{
	width: 380px;
	background-color: #EEEEEE;
	border: solid 1px #000000;
	position: fixed;
	top: 250px;
	left: 40%;	
	min-height: 160px;
	z-index: 1000;
	padding-top: 10px;
}

.AddEditRequestCommandPositionLightBox
{
	width: 580px;
	background-color: #EEEEEE;
	border: solid 1px #000000;
	position: fixed;
	top: 120px;
	left: 25%;	
	min-height: 350px;
	z-index: 1000;
	padding-top: 10px;
}  

.isDivMainClass
{
    font-family: Verdana;
}

.isDivMainClass input
{
   font-family: Verdana;
   font-weight: normal;
   font-size: 11px;
   width : 190px;
}
    
</style>

    <script src="../Scripts/Ajax.js" type="text/javascript"></script>
    <script src="../Scripts/AddEditEquipmentReservistsRequest.js" type="text/javascript"></script>

    <div id="jsMilitaryUnitSelectorDiv" runat="server">
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hfEquipmentReservistsRequestID" runat="server" />
            <asp:HiddenField ID="hfFromHome" runat="server" />
            <div style="height: 20px">
            </div>
            <table style="width: 100%;">
                <tr>
                    <td style="text-align: center;">
                        <span id="lblHeaderTitle" runat="server" class="HeaderText"></span>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <center>
                        <fieldset style="width: 880px; padding: 0px;">
                            <table id="tblRequestHeaderSection" class="InputRegion" style="width: 880px; padding: 10px; padding-top: 0px; margin-top: 0px;">
                                <tr>
                                    <td colspan="5" style="text-align: left;">
                                       <span style="color: #0B4489; font-weight: bold; font-size: 1.1em; width: 100%; position: relative; top: -5px;">Заявка</span>
                                    </td>
                                    <td style="text-align: right; padding-bottom: 5px;">
                                       <div style="display: inline; position: relative; left: 10px;">
                                          <img runat="server" id="btnImgSave" src="../Images/save.png" alt="Запис" title="Запис" class='GridActionIcon' onclick='btnSaveClick();' />
                                          <img runat="server" id="btnClose" src="../Images/close.png" alt="Затвараяне" title="Затваряне" class='GridActionIcon' onclick='btnCloseClick();' />
                                       </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: left; width: 8%;">
                                        <asp:Label ID="lblRequestNumber" runat="server" CssClass="InputLabel" Text="Заявка №:"></asp:Label>
                                    </td>
                                    <td style="text-align: left; width: 12%;">
                                        <asp:TextBox ID="txtRequestNumber" runat="server" CssClass="RequiredInputField" Width="130" MaxLength="300"></asp:TextBox>
                                    </td>
                                    
                                    <td style="text-align: right; width: 10%;">
                                        <asp:Label ID="lblRequestDate" runat="server" CssClass="InputLabel" Text="от дата:"></asp:Label>
                                    </td>
                                    <td style="text-align: left; width: 14%;">
                                        <asp:TextBox ID="txtRequestDate" runat="server" CssClass="RequiredInputField" Width="80" MaxLength="10"></asp:TextBox>
                                    </td>
                                    
                                    <td style="text-align: right; width: 31%;">
                                        <asp:Label ID="lblEquipWithResRequestsStatus" runat="server" CssClass="InputLabel" Text="Статус на заявката:"></asp:Label>
                                    </td>
                                    <td style="text-align: left; width: 25%;">
                                        <asp:DropDownList runat="server" ID="ddEquipWithResRequestsStatus" CssClass="InputField"
                                            Width="190">
                                        </asp:DropDownList>
                                    </td>                                    
                                </tr>
                                <tr>
                                    <td style="text-align: left;" colspan="4">
                                        <table cellpadding="0" cellspacing="0">
                                           <tr>
                                              <td nowrap="nowrap">
                                                 <asp:Label ID="lblMilitaryUnit" runat="server" CssClass="InputLabel" Text=""></asp:Label>
                                              </td>
                                              <td style="padding-left: 3px;">
                                                 <is:MilitaryUnitSelector ID="msMilitaryUnit" runat="server" DataSourceWebPage="DataSource.aspx" DataSourceKey="MilitaryUnit" 
                                                    DivMainCss="isDivMainClassRequired" DivListCss="isDivListClass" DivFullListCss="isDivFullListClass"
                                                    UnsavedCheckSkipMe="true" />
                                              </td>
                                           </tr>
                                        </table>
                                    </td>
                                    
                                    <td style="text-align: right;">
                                        <asp:Label ID="lblAdministration" runat="server" CssClass="InputLabel" Text="От кое министерство/ведомство:"></asp:Label>
                                    </td>
                                    <td style="text-align: left;">
                                        <asp:DropDownList runat="server" ID="ddAdministration" CssClass="InputField"
                                            Width="190">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        </center>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; height: 25px;">
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; padding-top: 5px; display: none;">
                        <asp:LinkButton ID="btnSave" runat="server" CssClass="Button" OnClientClick="return ValidateData();"
                            OnClick="btnSave_Click"><i></i><div style="width:70px; padding-left:5px;">Запис</div><b></b></asp:LinkButton>
                        &nbsp;
                        <asp:LinkButton ID="btnBack2" runat="server" CssClass="Button" OnClick="btnBack_Click"
                            CheckForChanges="true"><i></i><div style="width:70px; padding-left:5px;">Назад</div><b></b></asp:LinkButton>
                    </td>
                </tr>
             </table>
             
             <asp:HiddenField ID="hdnLocationHash" runat="server" />
             <asp:HiddenField ID="hdnSavedChangesContainer" runat="server" />
             
             <input type='hidden' id='hdnRequestCommandsCount' runat="server" value="0" />
             <input type='hidden' id='hdnVisibleRequestCommandsCount' runat="server" value="0" />
             
         </ContentTemplate>
    </asp:UpdatePanel>
             
    <table style="width: 100%;">
        <tr>
           <td style="text-align: center;">
                <div id="divRequestCommands" runat="server">
                </div>
           </td>
        </tr>
        <tr>
           <td>
              <div style="text-align: center; height: 20px;">
                 <span id="lblCommandsMessage"></span>
              </div>
           </td>
        </tr>
        <tr>
            <td style="text-align: center; padding-top: 20px;">
                <table style="width: 100%;">
                   <tr>
                      <td style="width: 33%; text-align: left;">
                         <div runat="server" id="btnAddRequestCommandCont" style="display: none;">
                             <div id="btnAddRequestCommand" style="display: inline;" onclick="AddRequestCommand();" class="Button">
                                 <i></i>
                                 <div style="width: 180px; display: inline">
                                     Добавяне на нова команда</div>
                                 <b></b>
                              </div>
                         </div>
                      </td>
                      <td style="width: 34%; text-align: center;">
                         <div style="display: none;">
                             <asp:LinkButton ID="btnBack" runat="server" CssClass="Button" OnClick="btnBack_Click"
                                  CheckForChanges="true"><i></i><div style="width:70px; padding-left:5px;">Назад</div><b></b></asp:LinkButton>
                         </div>
                      </td>
                      <td style="width: 33%; text-align: right;">
                      </td>
                   </tr>
                </table>
            </td>
        </tr>
    </table>
    
    <div id="AddRequestCommandLightBox" class="AddRequestCommandLightBox" style="display: none; text-align: center;">
        <center>
            <table width="80%" style="text-align: center;">
                <colgroup style="width: 30%">
                </colgroup>
                <colgroup style="width: 70%">
                </colgroup>
                <tr style="height: 15px">
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <span class="HeaderText" style="text-align: center;">Избор на команда</span>
                    </td>
                </tr>
                <tr style="height: 15px">
                </tr>
                <tr style="min-height: 17px">
                    <td style="text-align: right;">
                        <span id="lblSelectMilitaryCommand" class="InputLabel">Команда:</span>
                    </td>
                    <td style="text-align: left;">
                        <select id="ddMilitaryCommands" class="RequiredInputField" style="width: auto;" UnsavedCheckSkipMe="true"></select>
                    </td>
                </tr>                      
                <tr style="height: 35px">
                    <td colspan="2" style="padding-top: 5px;">
                        <span id="spanAddRequestCommandLightBoxMessage" class="ErrorText" style="display: none;">
                        </span>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <table style="margin: 0 auto;">
                            <tr>
                                <td>
                                    <div id="btnSaveAddRequestCommandLightBox" style="display: inline;" onclick="SaveAddRequestCommandLightBox();"
                                        class="Button">
                                        <i></i>
                                        <div id="btnSaveAddRequestCommandLightBoxText" style="width: 70px;">
                                            Добавяне</div>
                                        <b></b>
                                    </div>
                                    <div id="btnCloseAddRequestCommandLightBox" style="display: inline;" onclick="HideAddRequestCommandLightBox();"
                                        class="Button">
                                        <i></i>
                                        <div id="btnCloseAddRequestCommandLightBoxText" style="width: 70px;">
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
    
    <div id="AddEditRequestCommandPositionLightBox" class="AddEditRequestCommandPositionLightBox" style="display: none; text-align: center;">
        <center>
            <input type="hidden" id="hdnRequestCommandPositionID" UnsavedCheckSkipMe="true" />
            <input type="hidden" id="hdnSelectedCommandIdx" UnsavedCheckSkipMe="true" />
            <input type="hidden" id="hdnSelectedRequestCommandID" UnsavedCheckSkipMe="true" />
            <table width="80%" style="text-align: center;">
                <colgroup style="width: 30%">
                </colgroup>
                <colgroup style="width: 70%">
                </colgroup>
                <tr style="height: 15px">
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <span class="HeaderText" style="text-align: center;" id="lblAddEditReqCommandPosTitle"></span>
                    </td>
                </tr>
                <tr style="height: 15px">
                </tr>
                <tr style="min-height: 20px">
                    <td style="text-align: right;">
                        <span id="lblMilitaryCommandLightBox" class="InputLabel">Команда:</span>
                    </td>
                    <td style="text-align: left;">
                        <span id="lblMilitaryCommandLabelLightBox" class="ReadOnlyValue"></span>
                    </td>
                </tr>  
                <tr style="min-height: 17px">
                    <td style="text-align: right;">
                        <span id="lblPosition" class="InputLabel">Длъжност:</span>
                    </td>
                    <td style="text-align: left;">
                        <input type="text" id="txtPosition" UnsavedCheckSkipMe="true" class="RequiredInputField" style="width: 200px;" maxlength="250"  />
                    </td>
                </tr>                      
                <tr style="min-height: 17px">
                    <td style="text-align: right; vertical-align: top; padding-top: 5px;">
                        <span id="lblMilitaryReportSpecialityType" class="InputLabel">Категория:</span>
                        <div style="height: 5px;"></div>
                        <span id="lblMilitaryReportSpeciality" class="InputLabel">ВОС:</span>
                    </td>
                    <td style="text-align: left;">
                        <table>
                           <tr>
                              <td style="vertical-align: top;">
                                 <asp:DropDownList runat="server" ID="ddMRSType" UnsavedCheckSkipMe="true" CssClass="InputField" Width="200px"></asp:DropDownList>
                                 <div style="height: 2px;"></div>
                                 <select class="InputField" multiple="multiple" UnsavedCheckSkipMe="true" style="width: 200px; height: 70px;" id="ddAvailableMRS"></select>
                              </td>
                              <td style="vertical-align: middle;">
                                 <input type="button" value=">>" title="Избор" onclick="SelectMilitaryReportingSpecialities();"
                                    style="cursor: pointer; background-color: #CCCCCC; height: 17px; padding: 2px; line-height: 9px; font-size: 9px;" 
                                    id="btnSelectMRS" />
                                 <div style="height: 2px;"></div>
                                 <input type="button" value="<<" title="Премахване" onclick="RemoveMilitaryReportingSpecialities();"
                                    style="cursor: pointer; background-color: #CCCCCC; height: 17px; padding: 2px; line-height: 9px; font-size: 9px;" 
                                    id="btnRemoveMRS" />
                              </td>
                              <td style="vertical-align: top;">
                                 <span class="InputLabel" style="font-style: italic; font-size: 0.9em;" id="lblSelectedMRS">Избрани за длъжността</span><br />
                                 <select class="InputField" UnsavedCheckSkipMe="true" multiple="multiple" style="width: 200px; height: 78px;" id="ddSelectedMRS" onchange="ddSelectedMRS_Changed()"></select>
                              </td>
                           </tr>
                           <tr>
                              <td></td>
                              <td></td>
                              <td>
                                 <input type="checkbox" id="chkIsPrimaryMilRepSpec" onclick="chkIsPrimaryMilRepSpec_Click();" UnsavedCheckSkipMe="true" />
                                 <label id="lblIsPrimaryMilRepSpec" for="chkIsPrimaryMilRepSpec" class="InputField">Основна ВОС</label>
                                 <input type="hidden" id="hdnPrimaryMilRepSpecID" />
                              </td>
                           </tr>
                        </table>
                    </td>
                </tr>
                
                <tr style="min-height: 17px">
                    <td style="text-align: right; vertical-align: top; padding-top: 5px;">
                        <span id="lblMilitaryRank" class="InputLabel">Звание:</span>
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
                                 <select class="InputField" UnsavedCheckSkipMe="true" multiple="multiple" style="width: 200px; height: 78px;" id="ddSelectedRanks" onchange="ddSelectedRanks_Changed()"></select>
                              </td>
                           </tr>
                           <tr>
                              <td></td>
                              <td></td>
                              <td>
                                 <input type="checkbox" id="chkIsPrimaryRank" onclick="chkIsPrimaryRank_Click();" UnsavedCheckSkipMe="true" />
                                 <label id="lblIsPrimaryRank" for="chkIsPrimaryRank" class="InputField">Основно звание</label>
                                 <input type="hidden" id="hdnPrimaryRankID" />
                              </td>
                           </tr>
                        </table>
                    </td>
                </tr>
                
                <tr style="min-height: 17px">
                    <td style="text-align: right;">
                        <span id="lblReservistsCount" class="InputLabel">Запасни:</span>
                    </td>
                    <td style="text-align: left;">
                        <input type="text" id="txtReservistsCount" UnsavedCheckSkipMe="true" class="RequiredInputField" style="width: 60px;" />
                    </td>
                </tr>  
                <tr style="height: 46px; padding-top: 5px;">
                    <td colspan="2">
                        <span id="spanAddEditRequestCommandPositionLightBox" class="ErrorText" style="display: none;">
                        </span>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <table style="margin: 0 auto;">
                            <tr>
                                <td>
                                    <div id="btnSaveAddEditRequestCommandPositionLightBox" style="display: inline;" onclick="SaveAddEditRequestCommandPositionLightBox();"
                                        class="Button">
                                        <i></i>
                                        <div id="btnSaveAddEditRequestCommandPositionLightBoxText" style="width: 70px;">
                                            Запис</div>
                                        <b></b>
                                    </div>
                                    <div id="btnCloseAddEditRequestCommandPositionLightBox" style="display: inline;" onclick="HideAddEditRequestCommandPositionLightBox();"
                                        class="Button">
                                        <i></i>
                                        <div id="btnCloseAddEditRequestCommandPositionLightBoxText" style="width: 70px;">
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
    
    <div id="divImportVacantPositionsLightBox" class="GTibleLightBox" style="padding: 10px;
        display: none; text-align: center; width: 600px; left: 25%; top: 150px;">
        <img border='0' src='../Images/close.png' onclick="javascript:HideImportVacantPositionsLightBox();"
            style="cursor: pointer; float: right; display: none;" alt='Затвори' title='Затвори' /><br />
            
        <input type="hidden" id="hdnImportRequestsCommandID" value="" UnsavedCheckSkipMe="true" />
        <input type="hidden" id="hdnImportIdx" value="" UnsavedCheckSkipMe="true" />
        <input type="hidden" id="hdnLightBoxMilitaryCommandName" value="" UnsavedCheckSkipMe="true" />
        <input type="hidden" id="hdnLightBoxMilitaryCommandSuffix" value="" UnsavedCheckSkipMe="true" />
        <input type="hidden" id="hdnIsMilitaryReportSpecialityEnabled" value="" UnsavedCheckSkipMe="true" runat="server"/>
        <div id="divImportVacantPositionsLightBoxContent">
        </div>
    </div>
    

    <script type="text/javascript">
        var divRequestCommandsClientID = "<%= divRequestCommands.ClientID %>";
        var lblMessageClientID = "<%= lblMessage.ClientID %>";
        var txtRequestNumberClientID = "<%= txtRequestNumber.ClientID %>";
        var txtRequestDateClientID = "<%= txtRequestDate.ClientID %>";
        var hfEquipmentReservistsRequestIDClientID = "<%= hfEquipmentReservistsRequestID.ClientID %>";
        var btnSaveClientID = "<%= btnSave.ClientID %>";
        var btnBackClientID = "<%= btnBack.ClientID %>";
        var msMilitaryUnitClientID = "<%= msMilitaryUnit.ClientID %>";
        var btnAddRequestCommandContClientID = "<%= btnAddRequestCommandCont.ClientID %>";
        var hdnRequestCommandsCountClientID = "<%= hdnRequestCommandsCount.ClientID %>";
        var hdnVisibleRequestCommandsCountClientID = "<%= hdnVisibleRequestCommandsCount.ClientID %>";
        var ddMRSTypeClientID = "<%= ddMRSType.ClientID %>";
        var btnCloseClientID = "<%= btnClose.ClientID %>";
        var ddAvailableRanksClientID = "<%= ddAvailableRanks.ClientID %>";

        var hdnSavedChangesContainerID = '<%= hdnSavedChangesContainer.ClientID %>';
        var hdnIsMilitaryReportSpecialityEnabledID = '<%= hdnIsMilitaryReportSpecialityEnabled.ClientID %>';
        var militaryUnitLabel = '<%= PMIS.Common.CommonFunctions.GetLabelText("MilitaryUnit")%>';
        
    </script>

</asp:Content>
