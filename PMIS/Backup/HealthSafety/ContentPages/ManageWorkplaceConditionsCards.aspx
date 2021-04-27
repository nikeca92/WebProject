<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="ManageWorkplaceConditionsCards.aspx.cs" Inherits="PMIS.HealthSafety.ContentPages.ManageWorkplaceConditionsCards" 
         Title="Карти за комплексно оценяване на рисковете за живота и здравето" %>

<%@ Register Assembly="MilitaryUnitSelector" TagPrefix="is" Namespace="MilitaryUnitSelector" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<style type="text/css">

.isDivMainClass
{
    font-family: Verdana;
    width: 240px;
}

.isDivMainClass input
{
   font-family: Verdana;
   font-weight: normal;
   font-size: 11px;
   width : 200px;
}

</style>

<script src="../Scripts/Ajax.js" type="text/javascript"></script>
<script src="../Scripts/Common.js" type="text/javascript"></script>

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

function EditCard(cardId)
{
    JSRedirect("AddEditWorkplaceConditionsCard.aspx?WorkplaceConditionsCardID=" + cardId);
}

function DeleteCard(cardId)
{
    YesNoDialog("Желаете ли да изтриете картата?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "ManageWorkplaceConditionsCards.aspx?AjaxMethod=JSDeleteCard";
            var params = "";
            params += "CardID=" + cardId;
            
        function response_handler(xml)
        {
           if(xmlValue(xml, "response") != "OK")
           {
	          alert("There was a server problem!");
	       }
	       else
	       {
	          document.getElementById("<%=hdnRefreshReason.ClientID %>").value = "DELETED";
	          document.getElementById("<%=btnRefresh.ClientID %>").click();
	       }
	   }

	   var myAJAX = new AJAX(url, true, params, response_handler);
	   myAJAX.Call();
    }
}

function ShowPrintAllWorkplaceCondCards()
{
    var hfМilitaryUnitId = document.getElementById("<%= hfМilitaryUnitId.ClientID %>").value;
    var hfCardNumber = document.getElementById("<%= hfCardNumber.ClientID %>").value;
    var hfJobType = document.getElementById("<%= hfJobType.ClientID %>").value;
    var hdnSortBy = document.getElementById("<%= hdnSortBy.ClientID %>").value;

    var url = "";
    var pageName = "PrintAllWorkplaceConditionsCards"
    var param = "";
    
    url = "../PrintContentPages/" + pageName + ".aspx?MilitaryUnitID=" + hfМilitaryUnitId 
                + "&CardNumber=" + hfCardNumber + "&JobType=" + hfJobType + "&SortBy=" + hdnSortBy;
    
    var uplPopup = window.open(url, pageName, param);

    if (uplPopup != null)
        uplPopup.focus();
}

</script>

<div id="jsMilitaryUnitSelectorDiv" runat="server">
</div>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>

<div style="height: 20px;">
</div>

<div style="text-align: center;">
   <span id="lblHeaderTitle" runat="server" class="HeaderText">Карти за комплексно оценяване на рисковете за живота и здравето</span>
</div>

<div style="height: 20px;">
</div>

<div style="text-align: center;">
    <div style="width: 700px; margin: 0 auto;">
       <div class="FilterArea">
       <div class="FilterLegend">Филтър</div>
          <table width="100%" style="border-collapse:collapse; vertical-align: middle; color: #0B449D;">
             <tr style="height: 10px;">
                <td></td>
             </tr>
             <tr>
                <td style="vertical-align: top; text-align: right; width: 100px;">
                   <asp:Label runat="server" ID="lblMilitaryUnit" CssClass="InputLabel"></asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top;" colspan="2">
                   <table cellpadding="0" cellspacing="0">
                      <tr>
                         <td>
                            <is:MilitaryUnitSelector ID="musMilitaryUnit" runat="server" DataSourceWebPage="DataSource.aspx" DataSourceKey="MilitaryUnit" 
                                 DivMainCss="isDivMainClass" DivListCss="isDivListClass" DivFullListCss="isDivFullListClass" />
                         </td>
                         <td nowrap="nowrap">
                            <asp:Label runat="server" ID="lblCardNumber" CssClass="InputLabel">Номер на карта:</asp:Label>
                            <asp:TextBox runat="server" ID="txtCardNumber" CssClass="InputField" Width="150px"></asp:TextBox>
                         </td>
                      </tr>
                   </table>
                </td>
             </tr>
             <tr style="height: 10px;">
                <td></td>
             </tr>
             <tr>
                <td style="vertical-align: top; text-align: right; width: 100px;">
                    <asp:Label runat="server" ID="lblJobType" CssClass="InputLabel">Работно място (вид работа):</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: bottom;" colspan="2">                                     
                   <asp:TextBox runat="server" ID="txtJobType" CssClass="InputField" Width="200px"></asp:TextBox>                 
                 </td>
             </tr>
             <tr>
                <td colspan="3" style="padding-top: 15px;">
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                </td>
             </tr>
             <tr>
                <td colspan="3" style="width: 100%;" >
                    <center>
                        <asp:LinkButton ID="btnRefresh" runat="server" CssClass="Button" OnClick="btnRefresh_Click"><i></i><div style="width:70px; padding-left:5px;">Покажи</div><b></b></asp:LinkButton>
                        <asp:LinkButton ID="btnClear" runat="server" CssClass="Button" OnClick="btnClear_Click" ToolTip="Изчистване на избрания филтър"><i></i><div style="width:70px; padding-left:5px;">Изчисти</div><b></b></asp:LinkButton>
                        <div style="padding-left: 30px; display: inline">
                        </div>
                        <asp:LinkButton ID="btnPrintAllWorkplaceCondCards" runat="server" CssClass="Button" OnClientClick="ShowPrintAllWorkplaceCondCards(); return false;"><i></i><div style="width:70px; padding-left:5px;">Печат</div><b></b></asp:LinkButton>
                    </center>
                </td>
             </tr>
          </table>
        </div>
    </div>
</div>

<div style="height: 20px;"></div>

<div style="text-align: center;">
    <div style="width: 570px; margin: 0 auto; text-align: left; vertical-align: top; height: 35px;">
       <asp:LinkButton ID="btnNew" runat="server" CssClass="Button" OnClick="btnNew_Click"><i></i><div style="width:80px; padding-left:5px;">Нова карта</div><b></b></asp:LinkButton>
       
       <span style="padding: 10px">&nbsp;</span>
       
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
    <div style="width: 600px; margin: 0 auto;">
       <div runat="server" id="pnlCardsGrid" style="text-align: center;"></div>
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

<asp:HiddenField ID="hfМilitaryUnitId" runat="server" />
<asp:HiddenField ID="hfCardNumber" runat="server" />
<asp:HiddenField ID="hfJobType" runat="server" />

<input type="hidden" id="CanLeave" value="true" />
</ContentTemplate>
 </asp:UpdatePanel>
 </asp:Content>
