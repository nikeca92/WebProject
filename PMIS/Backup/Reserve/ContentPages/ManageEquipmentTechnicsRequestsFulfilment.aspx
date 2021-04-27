<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="ManageEquipmentTechnicsRequestsFulfilment.aspx.cs" Inherits="PMIS.Reserve.ContentPages.ManageEquipmentTechnicsRequestsFulfilment" 
         Title="Изпълнение на заявки за окомплектоване с техника от резерва" %>

<%@ Register Assembly="MilitaryUnitSelector" TagPrefix="is" Namespace="MilitaryUnitSelector" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script src="../Scripts/Ajax.js" type="text/javascript"></script>

<script type="text/javascript">
//Function that sorts the table by a specific column
function SortTableBy(sort)
{
    //If sorting by the same column them set the direction to be DESC
    if (document.getElementById("<%= hdnSortBy.ClientID %>").value == sort)
    {
        sort = sort + 100;
    }
    
    //Keep the order by column (its index) in a hidden field and simulate clicking Refresh
    document.getElementById("<%= hdnSortBy.ClientID %>").value = sort;                           
    document.getElementById("<%= btnRefresh.ClientID %>").click();               
}

//Set responsible military department by redirecting to the FulfilEquipmentTechnicsRequest screen
function FulfilEquipmentTechnics(equipmentTechnicsRequestId, militaryDepartmentId)
{
    JSRedirect("FulfilEquipmentTechnicsRequest.aspx?EquipmentTechnicsRequestId=" + equipmentTechnicsRequestId + "&MilitaryDepartmentId=" + militaryDepartmentId);
}


</script>

<style type="text/css">

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

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>

<div style="height: 20px;">
</div>

<div style="text-align: center;">
   <span id="lblHeaderTitle" runat="server" class="HeaderText">Изпълнение на заявки за окомплектоване с техника от резерва</span>
</div>

<div style="height: 20px;">
</div>

<div style="text-align: center;">
    <div style="width: 930px; margin: 0 auto;">
       <div class="FilterArea">
       <div class="FilterLegend">Филтър</div>
          <table width="100%" style="border-collapse:collapse; vertical-align: middle; color: #0B449D;">
             <tr style="height: 10px;">
                <td></td>
             </tr>
             <tr style="min-height: 30px;">
                <td style="vertical-align: top; text-align: right; width: 160px;">
                   <asp:Label runat="server" ID="lblRequestNumber" CssClass="InputLabel">Заявка №:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top; width: 200px;">
                   <asp:TextBox runat="server" ID="txtRequestNumber" CssClass="InputField" Width="195px"></asp:TextBox>
                </td>
                <td style="vertical-align: top; text-align: right; width: 230px;">
                   <asp:Label runat="server" ID="lblRequestDateFrom" CssClass="InputLabel">В периода от дата:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top;">
                   <asp:TextBox runat="server" ID="txtRequestDateFrom" CssClass="InputField" Width="80px"></asp:TextBox>
                   <div style="width: 10px; display: inline;">&nbsp;</div>
                   <asp:Label runat="server" ID="lblRequestDateTo" CssClass="InputLabel">до дата:</asp:Label>
                   <asp:TextBox runat="server" ID="txtRequestDateTo" CssClass="InputField" Width="80px"></asp:TextBox> 
                </td>
             </tr>
             <tr style="min-height: 30px;">
                <td style="vertical-align: top; text-align: right;">
                   <asp:Label runat="server" ID="lblCommandNum" CssClass="InputLabel">Команда №:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top;">
                   <asp:TextBox runat="server" ID="txtCommandNum" CssClass="InputField" Width="195px"></asp:TextBox>
                </td>
             </tr>
             <tr style="min-height: 30px;">
                <td style="vertical-align: bottom; text-align: right;">
                   <asp:Label runat="server" ID="lblMilitaryUnit" CssClass="InputLabel">Заявката е от <%= PMIS.Common.CommonFunctions.GetLabelText("MilitaryUnit") %>:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom;">
                   <is:MilitaryUnitSelector ID="msMilitaryUnit" runat="server" DataSourceWebPage="DataSource.aspx" DataSourceKey="MilitaryUnit" 
                                            DivMainCss="isDivMainClass" DivListCss="isDivListClass" DivFullListCss="isDivFullListClass"
                                            UnsavedCheckSkipMe="true" />
                </td>
                <td style="vertical-align: bottom; text-align: right;">
                   <asp:Label runat="server" ID="lblAdministration" CssClass="InputLabel">От кое министерство/ведомство:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom;">
                   <asp:DropDownList runat="server" ID="ddAdministration" CssClass="InputField" Width="280px"></asp:DropDownList>
                </td>
             </tr>
             
             
             <tr style="min-height: 30px;">
                <td style="vertical-align: bottom; text-align: right;">
                   <asp:Label runat="server" ID="lblEquipWithTechRequestsStatus" CssClass="InputLabel">Статус на заявката:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom;">
                   <asp:DropDownList runat="server" ID="ddEquipWithTechRequestsStatus" CssClass="InputField" Width="200px"></asp:DropDownList>
                </td>
                <td style="vertical-align: bottom; text-align: right;">
                   <asp:Label runat="server" ID="lblMilitaryDepartment" CssClass="InputLabel">Заявката се изпълнява от ВО:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom;">
                   <asp:DropDownList runat="server" ID="ddMilitaryDepartment" CssClass="InputField" Width="280px"></asp:DropDownList>
                </td>
             </tr>
             <tr>
                <td colspan="4" style="padding-top: 10px;">
                   <asp:LinkButton ID="btnRefresh" runat="server" CssClass="Button" OnClick="btnRefresh_Click"><i></i><div style="width:70px; padding-left:5px;">Покажи</div><b></b></asp:LinkButton>
                   <asp:LinkButton ID="btnClear" runat="server" CssClass="Button" OnClick="btnClear_Click" ToolTip="Изчистване на избрания филтър"><i></i><div style="width:70px; padding-left:5px;">Изчисти</div><b></b></asp:LinkButton>
                </td>
             </tr>
             <tr>
                <td colspan="2" style="padding-top: 15px;">
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                </td>
             </tr>
          </table>
        </div>
    </div>
</div>

<div style="height: 20px;"></div>

<div style="text-align: center;" runat="server" id="pnlSearchHint">
      <asp:Label ID="lblSearchHint" runat="server" Text="Задайте критерии за търсене и натиснете бутона 'Покажи'"></asp:Label>
</div>

<div id="divNavigation" runat="server" style="text-align: center;">
    <div style="width: 670px; margin: 0 auto; text-align: left; vertical-align: top; height: 35px;">       
       <div style="display: inline; position: relative; top: -16px;">
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
    <div style="width: 950px; margin: 0 auto;">
       <div runat="server" id="pnlDataGrid" style="text-align: center;"></div>
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

<asp:HiddenField ID="hdnSortBy" runat="server" />
<asp:HiddenField ID="hdnPageIdx" runat="server" />

<input type="hidden" id="CanLeave" value="true" />

</ContentTemplate>
 </asp:UpdatePanel>
 </asp:Content>
