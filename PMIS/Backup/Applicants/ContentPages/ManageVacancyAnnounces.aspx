<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="ManageVacancyAnnounces.aspx.cs" Inherits="PMIS.Applicants.ContentPages.ManageVacancyAnnounces" 
         Title="Списък на обявените конкурси" %>

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

    //Edit a particular record by redirecting to the Edit screen
    function EditVacancyAnnounce(vacancyAnnounceId)
    {
        JSRedirect("AddEditVacancyAnnounce.aspx?VacancyAnnounceId=" + vacancyAnnounceId);
    }

    //Delete a particular record: First confirm the operation by the user and next call ana AJAX querty that would delete the item
    function DeleteVacancyAnnounce(vacancyAnnounceId)
    {
        YesNoDialog("Желаете ли да изтриете конкурса?", ConfirmYes, null);

        function ConfirmYes()
        {
            var url = "ManageVacancyAnnounces.aspx?AjaxMethod=JSDeleteVacancyAnnounce";
                var params = "";
                params += "VacancyAnnounceId=" + vacancyAnnounceId;
                
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

    function ShowPrintAllVacancyAnnounces()
    {
        var hdnOrderNum = document.getElementById("<%= hdnOrderNum.ClientID %>").value;
        var hdnOrderDateFrom = document.getElementById("<%= hdnOrderDateFrom.ClientID %>").value;
        var hdnOrderDateTo = document.getElementById("<%= hdnOrderDateTo.ClientID %>").value;
        var hdnVacAnnStatus = document.getElementById("<%= hdnVacAnnStatus.ClientID %>").value;
        var hdnEndDateFrom = document.getElementById("<%= hdnEndDateFrom.ClientID %>").value;
        var hdnEndDateTo = document.getElementById("<%= hdnEndDateTo.ClientID %>").value;
        var hdnSortBy = document.getElementById("<%= hdnSortBy.ClientID %>").value;

        var url = "";
        var pageName = "PrintAllVacancyAnnounces"
        var param = "";

        url = "../PrintContentPages/" + pageName + ".aspx?OrderNum=" + hdnOrderNum
            + "&OrderDateFrom=" + hdnOrderDateFrom
            + "&OrderDateTo=" + hdnOrderDateTo
            + "&VacAnnStatus=" + hdnVacAnnStatus
            + "&EndDateFrom=" + hdnEndDateFrom
            + "&EndDateTo=" + hdnEndDateTo
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
   <span id="lblHeaderTitle" runat="server" class="HeaderText">Списък на обявените конкурси</span>
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
                <td style="vertical-align: top; text-align: right; width: 90px;">
                   <asp:Label runat="server" ID="lblOrderNum" CssClass="InputLabel">Заповед №:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top;">
                   <asp:TextBox runat="server" ID="txtOrderNum" CssClass="InputField" Width="185px"></asp:TextBox>
                </td>
                <td style="vertical-align: top; text-align: right; width: 120px;">
                   <asp:Label runat="server" ID="lblOrderDateFrom" CssClass="InputLabel">Дата заповед от:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top;">
                   <asp:TextBox runat="server" ID="txtOrderDateFrom" CssClass="InputField" Width="80px"></asp:TextBox>
                   <div style="width: 10px; display: inline;">&nbsp;</div>
                   <asp:Label runat="server" ID="lblOrderDateTo" CssClass="InputLabel">до:</asp:Label>
                   <asp:TextBox runat="server" ID="txtOrderDateTo" CssClass="InputField" Width="80px"></asp:TextBox> 
                </td>
             </tr>
             <tr>
                <td style="vertical-align: top; text-align: right; width: 90px;">
                   <asp:Label runat="server" ID="lblVacancyAnnounceStatuses" CssClass="InputLabel">Статус:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top;">
                   <asp:DropDownList runat="server" ID="ddVacancyAnnounceStatuses" CssClass="InputField" Width="190px"></asp:DropDownList>
                </td>
                <td style="vertical-align: top; text-align: right; width: 120px;">
                   <asp:Label runat="server" ID="lblEndDateFrom" CssClass="InputLabel">Крайна дата от:</asp:Label>
                </td>
                <td style="text-align: left; vertical-align: top;">
                   <asp:TextBox runat="server" ID="txtEndDateFrom" CssClass="InputField" Width="80px"></asp:TextBox>
                   <div style="width: 10px; display: inline;">&nbsp;</div>
                   <asp:Label runat="server" ID="lblEndDateTo" CssClass="InputLabel">до:</asp:Label>
                   <asp:TextBox runat="server" ID="txtEndDateTo" CssClass="InputField" Width="80px"></asp:TextBox> 
                </td>
             </tr>
             <tr>
                <td colspan="4">
                   <asp:LinkButton ID="btnRefresh" runat="server" CssClass="Button" OnClick="btnRefresh_Click"><i></i><div style="width:70px; padding-left:5px;">Покажи</div><b></b></asp:LinkButton>
                   <asp:LinkButton ID="btnClear" runat="server" CssClass="Button" OnClick="btnClear_Click" ToolTip="Изчистване на избрания филтър"><i></i><div style="width:70px; padding-left:5px;">Изчисти</div><b></b></asp:LinkButton>
                    <div style="padding-left: 30px; display: inline">
                    </div>
                    <asp:LinkButton ID="btnPrintAllVacancyAnnounces" runat="server" CssClass="Button" OnClientClick="ShowPrintAllVacancyAnnounces(); return false;"><i></i><div style="width:70px; padding-left:5px;">Печат</div><b></b></asp:LinkButton>
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

<div style="text-align: center;">
    <div style="width: 600px; margin: 0 auto; text-align: left; vertical-align: top; height: 35px;">
       <asp:LinkButton ID="btnNew" runat="server" CssClass="Button" OnClick="btnNew_Click"><i></i><div style="width:90px; padding-left:5px;">Нов конкурс</div><b></b></asp:LinkButton>
       
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
    <div style="width: 850px; margin: 0 auto;">
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

<asp:HiddenField ID="hdnOrderNum" runat="server" />
<asp:HiddenField ID="hdnOrderDateFrom" runat="server" />
<asp:HiddenField ID="hdnOrderDateTo" runat="server" />
<asp:HiddenField ID="hdnVacAnnStatus" runat="server" />
<asp:HiddenField ID="hdnEndDateFrom" runat="server" />
<asp:HiddenField ID="hdnEndDateTo" runat="server" />

<asp:HiddenField ID="hdnSortBy" runat="server" />
<asp:HiddenField ID="hdnPageIdx" runat="server" />

<asp:HiddenField ID="hdnRefreshReason" runat="server" />

<input type="hidden" id="CanLeave" value="true" />

</ContentTemplate>
 </asp:UpdatePanel>
 </asp:Content>
