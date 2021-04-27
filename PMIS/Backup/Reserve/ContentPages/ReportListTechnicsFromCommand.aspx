<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="ReportListTechnicsFromCommand.aspx.cs" Inherits="PMIS.Reserve.ContentPages.ReportListTechnicsFromCommand" 
         Title="Списък на техниката с МН от команда" %>

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

function ShowPrintReportListTechnicsFromCommand() {

    var hdnMilitaryDepartmentId = document.getElementById("<%= hdnMilitaryDepartmentId.ClientID %>").value;
    var hdnMilitaryCommandId = document.getElementById("<%= hdnMilitaryCommandId.ClientID %>").value;

    var hdnSortBy = document.getElementById("<%= hdnSortBy.ClientID %>").value;

    var url = "";
    var pageName = "PrintReportListTechnicsFromCommand"
    var param = "";

    url = "../PrintContentPages/" + pageName + ".aspx?MilitaryDepartmentId=" + hdnMilitaryDepartmentId
                + "&MilitaryCommandId=" + hdnMilitaryCommandId
                + "&SortBy=" + hdnSortBy;

    var uplPopup = window.open(url, pageName, param);

    if (uplPopup != null)
        uplPopup.focus();
}

function ExportReport() {
    var hdnMilitaryDepartmentId = document.getElementById("<%= hdnMilitaryDepartmentId.ClientID %>").value;
    var hdnMilitaryCommandId = document.getElementById("<%= hdnMilitaryCommandId.ClientID %>").value;

    var hdnSortBy = document.getElementById("<%= hdnSortBy.ClientID %>").value;

    var url = "";
    var pageName = "PrintReportListTechnicsFromCommand"
    var param = "";

    url = "../PrintContentPages/" + pageName + ".aspx?MilitaryDepartmentId=" + hdnMilitaryDepartmentId
                + "&MilitaryCommandId=" + hdnMilitaryCommandId
                + "&SortBy=" + hdnSortBy + "&Export=true";

    window.location = url;
}
// Open in new window AddEditTechnics page in read only mode
function PreviewTechnics(technicsId) {
    var url = "";
    var pageName = "AddEditTechnics"
    var param = "";

    url = "../ContentPages/" + pageName + ".aspx?TechnicsId=" + technicsId + "&Preview=1";

    var uplPopup = window.open(url, pageName, param);

    if (uplPopup != null)
        uplPopup.focus();
}

</script>

<style type="text/css">

.isDivMainClass
{
    font-family: Verdana;
    width: 200px;
}

.isDivMainClass input
{
   font-family: Verdana;
   font-weight: normal;
   font-size: 11px;
   width : 170px;
}
    
</style>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>

<div style="height: 20px;">
</div>

<div style="text-align: center;">
   <span id="lblHeaderTitle" runat="server" class="HeaderText">Списък на техниката с МН от команда</span>
</div>

<div style="height: 20px;">
</div>

<div style="text-align: center;">
    <div style="width: 930px; margin: 0 auto;">
       <div class="FilterArea">
       <div class="FilterLegend" style="width: 230px">Филтър - избор на команда</div>
          <table width="100%" style="border-collapse:collapse; vertical-align: middle; color: #0B449D;">
             <tr style="height: 10px;">
                <td></td>
             </tr> 
             <tr>
                <td style="vertical-align: bottom; text-align: right;">
                   <asp:Label runat="server" ID="lblMilitaryDepartment" CssClass="InputLabel">Заявката се изпълнява от ВО:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom;">
                   <asp:DropDownList runat="server" ID="ddMilitaryDepartment" CssClass="InputField" Width="280px" AutoPostBack="true" OnSelectedIndexChanged="ddMilitaryDepartment_Changed"></asp:DropDownList>
                </td>
                <td style="vertical-align: bottom; text-align: right;">
                   <asp:Label runat="server" ID="lblMilitaryCommand" CssClass="InputLabel">Команда:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom;">
                   <asp:DropDownList runat="server" ID="ddMilitaryCommand" CssClass="InputField" Width="280px"></asp:DropDownList>
                </td>
             </tr>
             <tr>
                <td colspan="4" style="padding-top: 10px;">
                   <asp:LinkButton ID="btnRefresh" runat="server" CssClass="Button" OnClick="btnRefresh_Click"><i></i><div style="width:70px; padding-left:5px;">Покажи</div><b></b></asp:LinkButton>
                   <asp:LinkButton ID="btnClear" runat="server" CssClass="Button" OnClick="btnClear_Click" ToolTip="Изчистване на избрания филтър"><i></i><div style="width:70px; padding-left:5px;">Изчисти</div><b></b></asp:LinkButton>
                    <div style="padding-left: 30px; display: inline">
                    </div>
                    <asp:LinkButton ID="btnPrintReportListTechnicsFromCommand" runat="server" CssClass="Button" OnClientClick="ShowPrintReportListTechnicsFromCommand(); return false;"><i></i><div style="width:70px; padding-left:5px;">Печат</div><b></b></asp:LinkButton>
                    <asp:LinkButton ID="btnExport" runat="server" CssClass="Button" OnClientClick="ExportReport(); return false;" ToolTip="Запазване в Excel"><i></i><div style="width:70px; padding-left:5px;">Excel</div><b></b></asp:LinkButton>
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

<div style="text-align: center;" runat="server" id="pnlPaging">
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

<asp:HiddenField ID="hdnRefreshReason" runat="server" />

<asp:HiddenField ID="hdnMilitaryDepartmentId" runat="server" />
<asp:HiddenField ID="hdnMilitaryCommandId" runat="server" />

<input type="hidden" id="CanLeave" value="true" />

</ContentTemplate>
 </asp:UpdatePanel>
 </asp:Content>
