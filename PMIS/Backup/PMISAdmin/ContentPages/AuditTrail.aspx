<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="AuditTrail.aspx.cs" Inherits="PMIS.PMISAdmin.ContentPages.AuditTrail" 
         Title="Одитни записи" %>

<%@ Register Assembly="MilitaryUnitSelector" TagPrefix="is" Namespace="MilitaryUnitSelector" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Scripts/Ajax.js'></script>

<style>
   .ShadowContainer
   {
   	   width: 1030px;
   }
   
   .isDivMainClass
   {
       font-family: Verdana;
       width: 170px;
   }

   .isDivMainClass input
   {
      font-family: Verdana;
      font-weight: normal;
      font-size: 11px;
      width : 160px;
   }
</style>

<script type="text/javascript">
function SortTableBy(sort)
{
    if (document.getElementById("<%= hdnSortBy.ClientID %>").value == sort)
    {
        sort = sort + 100;
    }
    
    document.getElementById("<%= hdnSortBy.ClientID %>").value = sort;                           
    document.getElementById("<%= btnRefresh.ClientID %>").click();

}

function ShowPrint() {
    var users = GetListBoxSelectedValues("<%= lstUsers.ClientID %>");
    var modules = GetListBoxSelectedValues("<%= lstModules.ClientID %>");
    var changeTypes = GetListBoxSelectedValues("<%= lstChangeTypes.ClientID %>");
    var changeEventTypes = GetListBoxSelectedValues("<%= lstChangeEventTypes.ClientID %>");
    var militaryUnits = MilitaryUnitSelectorUtil.GetSelectedValue("<%= musMilitaryUnit.ClientID %>");
    var dateFrom = document.getElementById("<%= txtDateFrom.ClientID %>").value;
    var dateTo = document.getElementById("<%= txtDateTo.ClientID %>").value;
    var description = document.getElementById("<%= txtObjectDesc.ClientID %>").value;
    var identNumber = document.getElementById("<%= txtPersonIdentityNumber.ClientID %>").value;
    var oldValue = document.getElementById("<%= txtOldValue.ClientID %>").value;
    var newValue = document.getElementById("<%= txtNewValue.ClientID %>").value;

    var hdnSortBy = document.getElementById("<%= hdnSortBy.ClientID %>").value;

    var url = "";
    var pageName = "PrintAuditTrail"
    var param = "";

    url = "../PrintContentPages/" + pageName + ".aspx?Users=" + users
                + "&Modules=" + modules
                + "&ChangeTypes=" + changeTypes
                + "&ChangeEventTypes=" + changeEventTypes
                + "&MilitaryUnits=" + militaryUnits
                + "&DateFrom=" + dateFrom
                + "&DateTo=" + dateTo
                + "&Description=" + description
                + "&IdentNumber=" + identNumber
                + "&OldValue=" + oldValue
                + "&NewValue=" + newValue
                + "&SortBy=" + hdnSortBy;

    var uplPopup = window.open(url, pageName, param);

    if (uplPopup != null)
        uplPopup.focus();
}
</script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>

<div style="height: 20px;">
</div>

<div style="text-align: center;">
   <span id="lblHeaderTitle" runat="server" class="HeaderText">Одитни записи</span>
</div>

<div style="height: 20px;">
</div>

<div style="text-align: center;">
    <div style="width: 1000px; margin: 0 auto;">
       <div class="FilterArea">
       <div class="FilterLegend">Филтър</div>
          <table width="100%" style="border-collapse:collapse; vertical-align: middle; color: #0B449D;">
             <tr style="height: 10px;">
                <td></td>
             </tr>
             <tr>
                <td style="vertical-align: top; text-align: left; width: 180px; padding-left: 10px;">
                   <asp:Label runat="server" ID="lblUsers" CssClass="InputLabel">Потребител:</asp:Label> <br />
                   <asp:ListBox runat="server" ID="lstUsers" Width="145px" CssClass="InputField" SelectionMode="Multiple"></asp:ListBox>
                </td>
                <td style="text-align: left; vertical-align: top; width: 200px;">
                   <asp:Label runat="server" ID="lblModules" CssClass="InputLabel">Модул:</asp:Label><br />
                   <asp:ListBox runat="server" ID="lstModules" Width="175px" CssClass="InputField" SelectionMode="Multiple"></asp:ListBox>
                </td>
                <td style="text-align: left; vertical-align: top; width:280px;">   
                   <asp:Label runat="server" ID="lblChangeTypes" CssClass="InputLabel">Тип промяна:</asp:Label><br />
                   <asp:ListBox runat="server" ID="lstChangeTypes" Width="250px" CssClass="InputField" SelectionMode="Multiple"></asp:ListBox>
                </td>
                <td style="text-align: left; vertical-align: top; width: 350px;">                      
                   <asp:Label runat="server" ID="lblChangeEventTypes" CssClass="InputLabel">Събитие:</asp:Label><br />
                   <asp:ListBox runat="server" ID="lstChangeEventTypes" Width="320px" CssClass="InputField" SelectionMode="Multiple"></asp:ListBox>
                </td>
             </tr>
             <tr style="height: 10px;">
                <td></td>
             </tr>
             <tr>
                 <td style="text-align: left; vertical-align: top; padding-left: 10px;" colspan="2">
                   <table cellpadding="0" cellspacing="0">
                      <tr>
                         <td>
                            <asp:Label runat="server" ID="lblMilitaryUnits" CssClass="InputLabel"></asp:Label>
                         </td>
                         <td style="padding-left: 3px;">
                            <is:MilitaryUnitSelector ID="musMilitaryUnit" runat="server" DataSourceWebPage="DataSource.aspx" DataSourceKey="MilitaryUnit"                                  DivMainCss="isDivMainClass" DivListCss="isDivListClass" DivFullListCss="isDivFullListClass" />
                         </td>
                      </tr>
                   </table>
                 </td>
                 <td style="text-align: left; vertical-align: top;" colspan="2">
                   <asp:Label runat="server" ID="lblDateFrom" CssClass="InputLabel">Дата от:</asp:Label>
                   <asp:TextBox runat="server" ID="txtDateFrom" CssClass="InputField" Width="80px"></asp:TextBox>
                   <div style="width: 10px; display: inline;">&nbsp;</div>
                   <asp:Label runat="server" ID="lblDateTo" CssClass="InputLabel">до:</asp:Label>
                   <asp:TextBox runat="server" ID="txtDateTo" CssClass="InputField" Width="80px"></asp:TextBox> 
                   <div style="width: 10px; display: inline;">&nbsp;</div>
                   <asp:Label runat="server" ID="lblObjectDesc" CssClass="InputLabel">Описание:</asp:Label>
                   <asp:TextBox runat="server" ID="txtObjectDesc" CssClass="InputField" Width="195px"></asp:TextBox> 
                 </td>
             </tr>
             <tr>
                 <td style="text-align: left; vertical-align: top;  padding-left: 85px;" colspan="2">
                   <asp:Label runat="server" ID="lblPersons" CssClass="InputLabel">ЕГН:</asp:Label> 
                   <asp:TextBox runat="server" ID="txtPersonIdentityNumber" CssClass="InputField" Width="100px"></asp:TextBox> 
                 </td> 
                 <td style="text-align: left; vertical-align: top;" colspan="2">
                   <asp:Label runat="server" ID="lblOldValue" CssClass="InputLabel">Стара стойност:</asp:Label>
                   <asp:TextBox runat="server" ID="txtOldValue" CssClass="InputField" Width="120px"></asp:TextBox> 
                   
                   <div style="width: 10px; display: inline;">&nbsp;</div>
                   
                   <asp:Label runat="server" ID="lblNewValue" CssClass="InputLabel">Нова стойност:</asp:Label>
                   <asp:TextBox runat="server" ID="txtNewValue" CssClass="InputField" Width="120px"></asp:TextBox> 
                 </td>
             </tr>
             <tr>
                <td colspan="4" style="padding-top: 15px;">
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                </td>
             </tr>    
             <tr>
                <td colspan="4" style="padding-top: 15px;">
                   <asp:LinkButton ID="btnRefresh" runat="server" CssClass="Button" OnClick="btnRefresh_Click"><i></i><div style="width:70px; padding-left:5px;">Покажи</div><b></b></asp:LinkButton>
                   <asp:LinkButton ID="btnClear" runat="server" CssClass="Button" OnClick="btnClear_Click" ToolTip="Изчистване на избрания филтър"><i></i><div style="width:70px; padding-left:5px;">Изчисти</div><b></b></asp:LinkButton>
                   <div style="padding-left: 30px; display: inline">
                   </div>
                   <asp:LinkButton ID="btnPrint" runat="server" CssClass="Button" OnClientClick="ShowPrint(); return false;"><i></i><div style="width:70px; padding-left:5px;">Печат</div><b></b></asp:LinkButton>
                </td>
             </tr>
          </table>
        </div>
    </div>
</div>

<div style="height: 20px;"></div>

<div style="text-align: center;">
    <div style="width: 1000px; margin: 0 auto; text-align: left;">
       <asp:ImageButton ID="btnFirst" runat="server" ImageUrl="../Images/ButtonFirst.png" AlternateText="Първа страница" CssClass="PaginationButton" OnClick="btnFirst_Click" />                        
       <asp:ImageButton ID="btnPrev" runat="server" ImageUrl="../Images/ButtonPrev.png" AlternateText="Предишна страница" CssClass="PaginationButton" OnClick="btnPrev_Click" />                        
       <asp:Label ID="lblPagination" runat="server" CssClass="PaginationLabel"></asp:Label>
       <asp:ImageButton ID="btnNext" runat="server" ImageUrl="../Images/ButtonNext.png" AlternateText="Следваща страница" CssClass="PaginationButton" OnClick="btnNext_Click" />                        
       <asp:ImageButton ID="btnLast" runat="server" ImageUrl="../Images/ButtonLast.png" AlternateText="Последна страница" CssClass="PaginationButton" OnClick="btnLast_Click" />            
       <span style="min-width: 100px; padding: 100px">&nbsp;</span>
       <span style="text-align: right;">Отиди на страница</span>
       <asp:TextBox ID="txtGotoPage" runat="server" CssClass="PaginationTextBox" Width="30px"></asp:TextBox>
       <asp:ImageButton ID="btnGoto" runat="server" ImageUrl="../Images/ButtonGoto.png" AlternateText="Отиди на страница" CssClass="PaginationButton" OnClick="btnGoto_Click" /> 
    </div>
</div>

<div style="height: 10px;"></div>

<div style="text-align: center;">
    <div style="width: 1000px; margin: 0 auto;">
       <div runat="server" id="pnlAuditTrailItems" style="text-align: center;"></div>
    </div>
</div>

<div style="height: 20px;"></div>

<div style="text-align: center;">
   <asp:LinkButton ID="btnBack" runat="server" CssClass="Button" OnClick="btnBack_Click"><i></i><div style="width:70px; padding-left:5px;">Назад</div><b></b></asp:LinkButton>
</div>

<div style="height: 20px;"></div>

<asp:HiddenField ID="hfLoginLogID" runat="server" />

<asp:HiddenField ID="hdnSortBy" runat="server" />
<asp:HiddenField ID="hdnPageIdx" runat="server" />

<input type="hidden" id="CanLeave" value="true" />

</ContentTemplate>
 </asp:UpdatePanel>
 </asp:Content>
