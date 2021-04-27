<%@ Page Title="Списък на водените на военен отчет" Language="C#" MasterPageFile="~/MasterPages/PrintMasterPage.Master" AutoEventWireup="true" CodeBehind="PrintAllReservists.aspx.cs" Inherits="PMIS.Reserve.PrintContentPages.PrintAllReservists" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript">
    function ExportToExcel() {
        document.getElementById("<%= btnGenerateExcel.ClientID %>").click();
    }
</script>

<span id="lblHeaderTitle" runat="server" class="HeaderText" style="margin-left: 225px;">Списък на водените на военен отчет</span>
<center>
    <asp:Button ID="btnGenerateExcel" runat="server" OnClick="btnGenerateExcel_Click" CssClass="HiddenButton" />
    <div id="divResults" style="min-width: 700px; max-width: 980px; margin-top: 10px;" runat="server" enableviewstate="false">
    </div>
</center>

</asp:Content>
