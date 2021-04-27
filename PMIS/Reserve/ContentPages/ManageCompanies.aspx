<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="ManageCompanies.aspx.cs" Inherits="PMIS.Reserve.ContentPages.ManageCompanies" 
         Title="Управление на фирмите" %>

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

//Edit a particular item by redirecting to the Edit screen
function EditCompany(companyId)
{
    JSRedirect("AddEditCompany.aspx?CompanyId=" + companyId);
}

//Delete a particular item: First confirm the operation by the user and next call ana AJAX querty that would delete the item
function DeleteCompany(companyId)
{
    YesNoDialog("Желаете ли да изтриете тази фирма?", ConfirmYes, null);

    function ConfirmYes()
    {
        var url = "ManageCompanies.aspx?AjaxMethod=JSDeleteCompany";
        var params = "";
        params += "CompanyId=" + companyId;    
            
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

</script>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>

<div style="height: 20px;">
</div>

<div style="text-align: center;">
   <span id="lblHeaderTitle" runat="server" class="HeaderText">Управление на фирмите</span>
</div>

<div style="height: 20px;">
</div>

<div style="text-align: center;">
    <div style="width: 720px; margin: 0 auto;">
       <div class="FilterArea">
       <div class="FilterLegend">Филтър</div>
          <table width="100%" style="border-collapse:collapse; vertical-align: middle; color: #0B449D;">
             <tr style="height: 10px;"></tr>
             <tr>
                <td style="text-align: right; vertical-align: top;">
                   <asp:Label runat="server" ID="lblCompanyName" CssClass="InputLabel">Име:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top;">
                   <asp:TextBox runat="server" ID="txtCompanyName" CssClass="InputField" Width="165px"></asp:TextBox>&nbsp;&nbsp;
                </td>
                <td style="vertical-align: top; text-align: right;">
                   <asp:Label runat="server" ID="lblBulstat" CssClass="InputLabel"><%= PMIS.Common.CommonFunctions.GetLabelText("UnifiedIdentityCode") %>/ЕГН:</asp:Label>                   
                </td>
                <td style="text-align: left; vertical-align: top;">
                   <asp:TextBox runat="server" ID="txtBulstat" CssClass="InputField" Width="216px"></asp:TextBox>&nbsp;&nbsp;                   
                </td>
             </tr>
             <tr style="height: 10px;"></tr>
             <tr>
                <td style="vertical-align: top; text-align: right;">
                    <asp:Label runat="server" ID="lblOwnershipType" CssClass="InputLabel">Вид собственост:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top;">                   
                   <asp:DropDownList runat="server" ID="ddOwnershipType" CssClass="InputField" Width="140px"></asp:DropDownList>
                </td>
                <td style="text-align: right; vertical-align: top;">                   
                   <asp:Label runat="server" ID="lblAdministration" CssClass="InputLabel">Министерство:</asp:Label>
                </td>   
                <td style="text-align: left; vertical-align: top;">
                   <asp:DropDownList runat="server" ID="ddAdministration" CssClass="InputField" Width="220px"></asp:DropDownList>
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
                </td>    
             </tr>
          </table>
        </div>
    </div>
</div>

<div style="height: 20px;"></div>

<div style="text-align: center;">
    <div style="width: 720px; margin: 0 auto; text-align: left; vertical-align: top; height: 35px;">
       <asp:LinkButton ID="btnNew" runat="server" CssClass="Button" OnClick="btnNew_Click"><i></i><div style="width:90px; padding-left:5px;">Нова фирма</div><b></b></asp:LinkButton>
       
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
    <div style="width: 700px; margin: 0 auto;">
       <div runat="server" id="pnlItemsGrid" style="text-align: center;"></div>
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

<input type="hidden" id="CanLeave" value="true" />

</ContentTemplate>
 </asp:UpdatePanel>
 </asp:Content>
