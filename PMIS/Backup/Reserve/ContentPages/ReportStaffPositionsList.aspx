<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportStaffPositionsList.aspx.cs" Inherits="PMIS.Reserve.ContentPages.ReportStaffPositionsList" 
         Title="Щатно-длъжностен списък" %>

<%@ Register Assembly="MilitaryUnitSelector" TagPrefix="is" Namespace="MilitaryUnitSelector" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script src="../Scripts/Ajax.js" type="text/javascript"></script>
<script src="../Scripts/Common.js" type="text/javascript"></script>
<script src="../Scripts/PickList.js" type="text/javascript"></script>

<style type="text/css">

.PageContentArea
{
	border: solid 1px #AAAAAA;
	background-color: #FFFFFF;
	position: relative;
	top: -1px;
	left: -1px;
	min-height: 400px;
	text-align: left;
	width: 1250px;
}

.ShadowContainer
{
    margin: 0 auto;
	width: 1252px;
}

#SubShadowContainer
{
	margin: 0 auto;
	width: 1252px;
    min-width: 1252px;
}

</style>

<script type="text/javascript">
    function ExportReport() {
        var militaryUnitId = MilitaryUnitSelectorUtil.GetSelectedValue("<%= msMilitaryUnit.ClientID %>");
        var militaryCommandId = document.getElementById("<%= ddMilitaryCommand.ClientID %>").value;
        var subMilitaryCommandSuffix = document.getElementById("<%= ddSubMilitaryCommand.ClientID %>").value;

        var url = "";
        var pageName = "PrintReportStaffPositionsList"
        var param = "";

        url = "../PrintContentPages/" + pageName + ".aspx?MilitaryUnitId=" + militaryUnitId
                                            + "&MilitaryCommandId=" + militaryCommandId
                                            + "&SubMilitaryCommandSuffix=" + subMilitaryCommandSuffix
                                            + "&Export=true";

        window.location = url;
    }

    window.addEventListener ? window.addEventListener('load', PageLoad, false) : window.attachEvent('onload', PageLoad);
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandlerPage);

    function PageLoad() {
        
    }

    function EndRequestHandlerPage(sender, args) {
        
    }

    function btnClear_Click() {
        document.getElementById("<%= hdnMilitaryUnit_OldSelection.ClientID %>").value = "-1";
        MilitaryUnitSelectorUtil.SetSelectedValue("<%= msMilitaryUnit.ClientID %>", "-1");
        MilitaryUnitSelectorUtil.SetSelectedText("<%= msMilitaryUnit.ClientID %>", "");
    }

    function msMilitaryUnit_EndOfSelection() {
        //This event is called lots of times (e.g. while typing with the keyboard).
        //That is why we check if the user really has changed the selection and only in that case reload the military commands from the server.
        if (MilitaryUnitSelectorUtil.GetSelectedValue("<%= msMilitaryUnit.ClientID %>") != document.getElementById("<%= hdnMilitaryUnit_OldSelection.ClientID %>").value) {
            document.getElementById("<%= hdnMilitaryUnit_OldSelection.ClientID %>").value = MilitaryUnitSelectorUtil.GetSelectedValue("<%= msMilitaryUnit.ClientID %>");
            document.getElementById("<%= hdnBtnReloadMilitaryCommands.ClientID %>").click();
        }
    }

</script>

<div style="text-align: center; padding-top: 20px; padding-bottom: 20px;">
   <span id="lblHeaderTitle" runat="server" class="HeaderText">Щатно-длъжностен списък</span>
</div>

<div style="text-align: center;">
    <div style="width: 800px; margin: 0 auto;">
       <div class="FilterArea">
       <div class="FilterLegend">Филтър</div>
          <table width="100%" style="border-collapse:collapse; vertical-align: middle; color: #0B449D;">
             <tr style="height: 10px;">
                <td></td>
             </tr>
             <tr>
                <td style="vertical-align: middle; text-align: right; width: 250px;">                                      
                   <asp:Label runat="server" ID="lblMilitaryUnit" CssClass="InputLabel">:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top; width: 340px;">
                   <is:MilitaryUnitSelector ID="msMilitaryUnit" runat="server" DataSourceWebPage="DataSource.aspx" DataSourceKey="MilitaryUnit" 
                                            DivMainCss="isDivMainClass" DivListCss="isDivListClass" DivFullListCss="isDivFullListClass"
                                            UnsavedCheckSkipMe="true" 
                                            OnEndOfSelection="msMilitaryUnit_EndOfSelection();"
                                            />
                   <asp:Button ID="hdnBtnReloadMilitaryCommands" runat="server" OnClick="hdnBtnReloadMilitaryCommands_Click" style="display: none;" />
                </td>                
             </tr>   
             <tr style="height: 10px;">
                <td></td>
             </tr>
             <tr>
                <td style="vertical-align: middle; text-align: right; ">  
                    <asp:Label ID="lblMilitaryCommand" runat="server" CssClass="InputLabel" Text="Команда:"></asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top;">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always"> 
                    <ContentTemplate> 
                        <asp:DropDownList runat="server" ID="ddMilitaryCommand" CssClass="InputField" Width="205px" AutoPostBack="true" OnSelectedIndexChanged="ddMilitaryCommand_Changed"></asp:DropDownList>
                    </ContentTemplate>
                    </asp:UpdatePanel>                        
                </td>
             </tr>
             <tr style="height: 10px;">
                <td></td>
             </tr>
             <tr>
                <td style="vertical-align: middle; text-align: right;">
                    <asp:Label ID="lblSubMilitaryCommand" runat="server" CssClass="InputLabel" Text="Подкоманда:"></asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top;">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always"> 
                    <ContentTemplate> 
                        <asp:DropDownList runat="server" ID="ddSubMilitaryCommand" CssClass="InputField" Width="205px"></asp:DropDownList>
                    </ContentTemplate>
                    </asp:UpdatePanel>  
                </td>
             </tr>
             <tr style="height: 10px;">
                <td></td>
             </tr>
             <tr>
                <td colspan="2" style="padding-top: 15px;">
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                </td>
             </tr>
             <tr>
                <td colspan="2" style="width: 100%;" >
                    <center>
                        <asp:LinkButton ID="btnRefresh" runat="server" CssClass="Button" OnClick="btnRefresh_Click"><i></i><div style="width:70px; padding-left:5px;">Покажи</div><b></b></asp:LinkButton>
                        <asp:LinkButton ID="btnClear" runat="server" CssClass="Button" OnClick="btnClear_Click" OnClientClick="btnClear_Click();" ToolTip="Изчистване на избрания филтър"><i></i><div style="width:70px; padding-left:5px;">Изчисти</div><b></b></asp:LinkButton>
                        <div style="padding-left: 30px; display: inline;">
                        </div>
                        <asp:LinkButton ID="btnExport" runat="server" CssClass="Button" OnClientClick="ExportReport(); return false;" ToolTip="Запазване в Excel"><i></i><div style="width:70px; padding-left:5px;">Excel</div><b></b></asp:LinkButton>
                    </center>
                </td>
             </tr>
          </table>
        </div>
    </div>
</div>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<Triggers>
   <asp:AsyncPostBackTrigger ControlID="btnRefresh" EventName="Click" />
   <asp:AsyncPostBackTrigger ControlID="btnClear" EventName="Click" />
   <asp:AsyncPostBackTrigger ControlID="hdnBtnReloadMilitaryCommands" EventName="Click" />
</Triggers>
<ContentTemplate>

<asp:HiddenField runat="server" ID="hdnMilitaryUnit_OldSelection" />

<asp:HiddenField ID="hdnPageIdx" runat="server" />
<input type="hidden" id="CanLeave" value="true" />

<div style="height: 30px;"></div>

<div style="text-align: center;" runat="server" id="pnlSearchHint">
      <asp:Label ID="lblSearchHint" runat="server" Text="Задайте критерии за търсене и натиснете бутона 'Покажи'"></asp:Label>
</div>

<div id="divNavigation" runat="server" style="text-align: center;" visible="false">
    <div style="width: 670px; margin: 0 auto; text-align: left; vertical-align: middle;">
       <span style="padding: 10px">&nbsp;</span>
       
       <div style="display: inline; position: relative; top: -8px;">
          <asp:ImageButton ID="btnFirst" runat="server" ImageUrl="../Images/ButtonFirst.png" AlternateText="Първа страница" CssClass="PaginationButton" OnClick="btnFirst_Click" />                        
          <asp:ImageButton ID="btnPrev" runat="server" ImageUrl="../Images/ButtonPrev.png" AlternateText="Предишна страница" CssClass="PaginationButton" OnClick="btnPrev_Click" />                        
          <asp:Label ID="lblPagination" runat="server" CssClass="PaginationLabel"></asp:Label>
          <asp:ImageButton ID="btnNext" runat="server" ImageUrl="../Images/ButtonNext.png" AlternateText="Следваща страница" CssClass="PaginationButton" OnClick="btnNext_Click" />                        
          <asp:ImageButton ID="btnLast" runat="server" ImageUrl="../Images/ButtonLast.png" AlternateText="Последна страница" CssClass="PaginationButton" OnClick="btnLast_Click" />            
          <span style="padding: 30px">&nbsp;</span>
          <span style="text-align: right;">Отиди на страница</span>
          <asp:TextBox ID="txtGotoPage" runat="server" CssClass="PaginationTextBox" Width="30px"></asp:TextBox>
          <asp:ImageButton ID="btnGoto" runat="server" ImageUrl="../Images/ButtonGoto.png" AlternateText="Отиди на страница" CssClass="PaginationButton" OnClick="btnGoto_Click" /> 
       </div>
    </div>
</div>

<div style="text-align: center;">
    <div style="width: 1240px; margin: 0 auto;">
       <div runat="server" id="pnlReportGrid" style="text-align: left;"></div>
    </div>
</div>

<div style="height: 10px;"></div>
   <div style="text-align: center;">
      <asp:Label ID="lblGridMessage" runat="server" Text=""></asp:Label>
   </div>
<div style="height: 10px;"></div>

<div style="text-align: center;">
   <asp:LinkButton ID="btnBack" runat="server" CssClass="Button" OnClick="btnBack_Click"><i></i><div style="width:70px; padding-left:5px;">Назад</div><b></b></asp:LinkButton>
</div>

<div style="height: 20px;"></div>

</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
