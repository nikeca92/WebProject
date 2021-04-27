<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportAnalyzeResFulfilment.aspx.cs" Inherits="PMIS.Reserve.ContentPages.ReportAnalyzeResFulfilment" 
         Title="Анализ на комплектуването" %>

<%@ Register Assembly="MilitaryUnitSelector" TagPrefix="is" Namespace="MilitaryUnitSelector" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script src="../Scripts/Ajax.js" type="text/javascript"></script>
<script src="../Scripts/Common.js" type="text/javascript"></script>
<script src="../Scripts/PickList.js" type="text/javascript"></script>

<script type="text/javascript">
function ExportReport() {
    var militaryUnitId = MilitaryUnitSelectorUtil.GetSelectedValue("<%= msMilitaryUnit.ClientID %>");
    var militaryCommandIds = PickListUtil.GetSelectedValues("pickListMilitaryCommands");
    var militaryCategoryKey = document.getElementById("<%= ddMilitaryCategory.ClientID %>").value;

    var url = "";
    var pageName = "PrintReportAnalyzeResFulfilment"
    var param = "";

    url = "../PrintContentPages/" + pageName + ".aspx?MilitaryUnitId=" + militaryUnitId
                                            + "&MilitaryCommandIds=" + militaryCommandIds
                                            + "&MilitaryCategoryKey=" + militaryCategoryKey
                                            + "&Export=true";

    window.location = url;
}

window.addEventListener ? window.addEventListener('load', PageLoad, false) : window.attachEvent('onload', PageLoad);
Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandlerPage);

function PageLoad()
{
    LoadPickLists();

    if (document.getElementById("<%= hdnMilitaryCommandSelected.ClientID %>").value != "")
        PickListUtil.SetSelection("pickListMilitaryCommands", document.getElementById("<%= hdnMilitaryCommandSelected.ClientID %>").value);
}

function EndRequestHandlerPage(sender, args) {
    ReLoadPickLists();
}

function LoadPickLists()
{
    var configPickListMilitaryCommands =
    {
        width: 200,
        allLabel: "<Всички>"
    }

    militaryCommands = document.getElementById("<%= hdnMilitaryCommandJson.ClientID %>").value;
    militaryCommands = eval(militaryCommands);
    PickListUtil.AddPickList("pickListMilitaryCommands", militaryCommands, "tdPickListMilitaryCommands", configPickListMilitaryCommands);
}

function ReLoadPickLists() {
    var configPickListMilitaryCommands =
    {
        width: 200,
        allLabel: "<Всички>"
    }

    militaryCommands = document.getElementById("<%= hdnMilitaryCommandJson.ClientID %>").value;
    militaryCommands = eval(militaryCommands);
    PickListUtil.ReloadPickList("pickListMilitaryCommands", militaryCommands, configPickListMilitaryCommands);

    militaryCommandsSelected = document.getElementById("<%= hdnMilitaryCommandSelected.ClientID %>").value;
    PickListUtil.SetSelection("pickListMilitaryCommands", militaryCommandsSelected);
}

function SetPickListsSelection() {
    document.getElementById("<%= hdnMilitaryCommandSelected.ClientID %>").value = PickListUtil.GetSelectedValues("pickListMilitaryCommands");
}

function btnClear_Click() {
    document.getElementById("<%= hdnMilitaryUnit_OldSelection.ClientID %>").value = "-1";
    MilitaryUnitSelectorUtil.SetSelectedValue("<%= msMilitaryUnit.ClientID %>", "-1");
    MilitaryUnitSelectorUtil.SetSelectedText("<%= msMilitaryUnit.ClientID %>", "");

    document.getElementById("<%= hdnMilitaryCommandSelected.ClientID %>").value = "";
    document.getElementById("<%= hdnMilitaryCommandJson.ClientID %>").value = "";
    PickListUtil.ClearSelection("pickListMilitaryCommands");
    ReLoadPickLists();

    document.getElementById("<%= ddMilitaryCategory.ClientID %>").selectedIndex = 0;
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


<div style="height: 20px;">
</div>

<div style="text-align: center;">
   <span id="lblHeaderTitle" runat="server" class="HeaderText">Анализ на комплектуването</span>
</div>

<div style="height: 20px;">
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
                <td style="vertical-align: middle; text-align: right;">
                   <asp:Label runat="server" ID="Label1" CssClass="InputLabel">Команда:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top;">
                   <div id="tdPickListMilitaryCommands"></div>
                </td>                
             </tr>
             <tr style="height: 10px;">
                <td></td>
             </tr>
             <tr>
                <td style="vertical-align: middle; text-align: right;">
                   <asp:Label runat="server" ID="lblMilitaryCategory" CssClass="InputLabel">Категория:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top;">
                   <asp:DropDownList runat="server" ID="ddMilitaryCategory" CssClass="InputField" Width="205px"></asp:DropDownList>
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
                        <div style="padding-left: 30px; display: inline">
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
<asp:HiddenField runat="server" ID="hdnMilitaryCommandJson" />
<asp:HiddenField runat="server" ID="hdnMilitaryCommandSelected" />

<div style="height: 30px;"></div>

<div style="text-align: center;" runat="server" id="pnlSearchHint">
      <asp:Label ID="lblSearchHint" runat="server" Text="Задайте критерии за търсене и натиснете бутона 'Покажи'"></asp:Label>
</div>

<div style="text-align: center;">
    <div style="width: 800px; margin: 0 auto;">
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

<input type="hidden" id="CanLeave" value="true" />
</ContentTemplate>
</asp:UpdatePanel>
</asp:Content>
