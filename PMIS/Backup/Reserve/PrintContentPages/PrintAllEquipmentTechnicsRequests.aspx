<%@ Page Title="Списък на въведени заявки за окомплектоване с техника от резерва" Language="C#" MasterPageFile="~/MasterPages/PrintMasterPage.Master" AutoEventWireup="true" CodeBehind="PrintAllEquipmentTechnicsRequests.aspx.cs" Inherits="PMIS.Reserve.PrintContentPages.PrintAllEquipmentTechnicsRequests" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript">
    function ExportToExcel() {
        document.getElementById("<%= btnGenerateExcel.ClientID %>").click();
    }
</script>

<span id="lblHeaderTitle" runat="server" class="HeaderText" style="margin-left: 45px;">Списък на въведени заявки за окомплектоване с техника от резерва</span>
<center>
    <asp:Button ID="btnGenerateExcel" runat="server" OnClick="btnGenerateExcel_Click" CssClass="HiddenButton" />
    <div id="divResults" style="min-width: 700px; max-width: 860px; margin-top: 10px;" runat="server">
    </div>
</center>

</asp:Content>
