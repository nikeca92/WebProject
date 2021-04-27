<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="LoginLog.aspx.cs" Inherits="PMIS.PMISAdmin.ContentPages.LoginLog" 
         Title="Потребителски сесии" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Scripts/Ajax.js'></script>

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

function ShowAuditTrial(loginLogId)
{
    JSRedirect("AuditTrail.aspx?LoginLogId=" + loginLogId);
}

function ShowPrint()
{
    var users = GetListBoxSelectedValues("<%= lstUsers.ClientID %>");
    var modules = GetListBoxSelectedValues("<%= lstModules.ClientID %>");
    var dateFrom = document.getElementById("<%= txtDateFrom.ClientID %>").value;
    var dateTo = document.getElementById("<%= txtDateTo.ClientID %>").value;
    
    var hdnSortBy = document.getElementById("<%= hdnSortBy.ClientID %>").value;

    var url = "";
    var pageName = "PrintLoginLog"
    var param = "";

    url = "../PrintContentPages/" + pageName + ".aspx?Users=" + users
                + "&Modules=" + modules
                + "&DateFrom=" + dateFrom
                + "&DateTo=" + dateTo
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
   <span id="lblHeaderTitle" runat="server" class="HeaderText">Потребителски сесии</span>
</div>

<div style="height: 20px;">
</div>

<div style="text-align: center;">
    <div style="width: 500px; margin: 0 auto;">
    <center>
       <div class="FilterArea">
       <div class="FilterLegend">Филтър</div>
          <table style="border-collapse:collapse; vertical-align: middle; color: #0B449D;">
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
             </tr>
             <tr style="height: 10px;">
                <td></td>
             </tr>
             <tr>
                 <td style="text-align: left; vertical-align: top; padding-left: 10px;">
                   <asp:Label runat="server" ID="lblDateFrom" CssClass="InputLabel">Дата от:</asp:Label>
                   <asp:TextBox runat="server" ID="txtDateFrom" CssClass="InputField" Width="80px"></asp:TextBox>
                 </td>
                 <td style="text-align: left; vertical-align: top;">                                                         
                   <asp:Label runat="server" ID="lblDateTo" CssClass="InputLabel">до:</asp:Label>
                   <asp:TextBox runat="server" ID="txtDateTo" CssClass="InputField" Width="80px"></asp:TextBox> 
                 </td>
             </tr>            
             <tr>
                <td colspan="2" style="padding-top: 15px;">
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
    </center>
    </div>
</div>

<div style="height: 20px;"></div>

<div style="text-align: center;">
    <div style="width: 750px; margin: 0 auto; text-align: center;">
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

<div style="height: 10px;"></div>

<div style="text-align: center;">
    <div style="width: 650px; margin: 0 auto;">
       <div runat="server" id="pnlLoginLogItems" style="text-align: center;"></div>
    </div>
</div>

<div style="height: 20px;"></div>

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
