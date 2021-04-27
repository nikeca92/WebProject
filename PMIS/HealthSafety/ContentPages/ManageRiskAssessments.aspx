<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="ManageRiskAssessments.aspx.cs" Inherits="PMIS.HealthSafety.ContentPages.ManageRiskAssessments" Title="Оценка на риска" %>

<%@ Register Assembly="MilitaryUnitSelector" TagPrefix="is" Namespace="MilitaryUnitSelector" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<style type="text/css">

.isDivMainClass
{
    font-family: Verdana;
    width: 150px;
}

.isDivMainClass input
{
   font-family: Verdana;
   font-weight: normal;
   font-size: 11px;
   width : 130px;
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

function EditRiskAssessment(riskAssessmentId)
{
    JSRedirect("AddEditRiskAssessment.aspx?RiskAssessmentId=" + riskAssessmentId);
}

function DeleteRiskAssessment(riskAssessmentId)
{
    YesNoDialog("Желаете ли да изтриете оценката?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "ManageRiskAssessments.aspx?AjaxMethod=JSDeleteRiskAssessment";
            var params = "";
            params += "RiskAssessmentID=" + riskAssessmentId;
            
        function response_handler(xml)
        {
           if(xmlValue(xml, "response") != "OK")
           {
	          alert("Има проблеми на сървъра!");
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

function ShowPrintAllRiskAssessments()
{
    var hfМilitaryUnitId = document.getElementById("<%= hfМilitaryUnitId.ClientID %>").value;
    var hfRegNumber = document.getElementById("<%= hfRegNumber.ClientID %>").value;
    var hfDateFrom = document.getElementById("<%= hfDateFrom.ClientID %>").value;
    var hfDateTo = document.getElementById("<%= hfDateTo.ClientID %>").value;
    var hdnSortBy = document.getElementById("<%= hdnSortBy.ClientID %>").value;

    var url = "";
    var pageName = "PrintAllRiskAssessments"
    var param = "";
    
    url = "../PrintContentPages/" + pageName + ".aspx?MilitaryUnitID=" + hfМilitaryUnitId 
                + "&RegNumber=" + hfRegNumber + "&DateFrom=" + hfDateFrom
                + "&DateTo=" + hfDateTo + "&SortBy=" + hdnSortBy;
    
    var uplPopup = window.open(url, pageName, param);

    if (uplPopup != null)
        uplPopup.focus();
}

</script>

<div id="jsMilitaryUnitSelectorDiv" runat="server">
</div>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>

<input type="hidden" id="CanLeave" value="true" />

<div style="height: 20px;">
</div>

<div style="text-align: center;">
   <span id="lblHeaderTitle" runat="server" class="HeaderText">Оценки на риска</span>
</div>

<div style="height: 20px;">
</div>

<div style="text-align: center;">
    <div style="width: 600px; margin: 0 auto;">
       <div class="FilterArea">
       <div class="FilterLegend">Филтър</div>
          <table width="100%" style="border-collapse:collapse; vertical-align: middle; color: #0B449D;">
             <tr style="height: 10px;">
                <td colspan="4"></td>
             </tr>
             <tr>
                <td style="vertical-align: top; text-align: right; width: 80px;">
                   <asp:Label runat="server" ID="lblMilitaryUnit" CssClass="InputLabel"></asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top;">
                   <is:MilitaryUnitSelector ID="musMilitaryUnit" runat="server" DataSourceWebPage="DataSource.aspx" DataSourceKey="MilitaryUnit" 
                        DivMainCss="isDivMainClass" DivListCss="isDivListClass" DivFullListCss="isDivFullListClass" />
                </td>
                <td style="vertical-align: top; text-align: right; width: 120px;">
                   <asp:Label runat="server" ID="lblRegNumber" CssClass="InputLabel">Регистрационен №:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top;">
                   <asp:TextBox ID="txtRegNumber" runat="server" CssClass="InputField"></asp:TextBox>
                </td>
             </tr>
             <tr style="height: 10px;">
                <td colspan="4"></td>
             </tr>
             <tr>                
                <td style="vertical-align: top; text-align: right; width: 80px;">
                   <asp:Label runat="server" ID="lblDateFrom" CssClass="InputLabel">Дата от:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top;">
                   <asp:TextBox runat="server" ID="txtDateFrom" CssClass="InputField" Width="80px"></asp:TextBox>
                </td>
                <td style="vertical-align: top; text-align: right; width: 120px;">
                   <asp:Label runat="server" ID="lblDateTo" CssClass="InputLabel">до:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top;">
                   <asp:TextBox runat="server" ID="txtDateTo" CssClass="InputField" Width="80px"></asp:TextBox> 
                </td>
             </tr>
             <tr>
                <td colspan="4" style="padding-top: 15px;">
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                </td>
             </tr>
             <tr>
                <td colspan="4" style="width: 100%;" >
                    <center>
                        <asp:LinkButton ID="btnRefresh" runat="server" CssClass="Button" OnClick="btnRefresh_Click"><i></i><div style="width:70px; padding-left:5px;">Покажи</div><b></b></asp:LinkButton>
                        <asp:LinkButton ID="btnClear" runat="server" CssClass="Button" OnClick="btnClear_Click" ToolTip="Изчистване на избрания филтър"><i></i><div style="width:70px; padding-left:5px;">Изчисти</div><b></b></asp:LinkButton>
                        <div style="padding-left: 30px; display: inline">
                        </div>
                        <asp:LinkButton ID="btnPrintAllRiskAssessments" runat="server" CssClass="Button" OnClientClick="ShowPrintAllRiskAssessments(); return false;"><i></i><div style="width:70px; padding-left:5px;">Печат</div><b></b></asp:LinkButton>
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
       <asp:LinkButton ID="btnNew" runat="server" CssClass="Button" OnClick="btnNew_Click"><i></i><div style="width:80px; padding-left:5px;">Нова оценка</div><b></b></asp:LinkButton>
       
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
       <div runat="server" id="pnlRiskAssessmentsGrid" style="text-align: center;"></div>
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
<asp:HiddenField ID="hfRegNumber" runat="server" />
<asp:HiddenField ID="hfDateFrom" runat="server" />
<asp:HiddenField ID="hfDateTo" runat="server" />

</ContentTemplate>
 </asp:UpdatePanel>
 </asp:Content>
