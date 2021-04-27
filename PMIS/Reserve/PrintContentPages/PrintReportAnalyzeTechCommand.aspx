<%@ Page Title="Сведение-анализ за комплектуване на команда с техника-запас" Language="C#" MasterPageFile="~/MasterPages/PrintMasterPage.Master" AutoEventWireup="true" CodeBehind="PrintReportAnalyzeTechCommand.aspx.cs" Inherits="PMIS.Reserve.PrintContentPages.PrintReportAnalyzeTechCommand" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript">
    function ExportToExcel() {
        document.getElementById("<%= btnGenerateExcel.ClientID %>").click();
    }
</script>

<span id="lblHeaderTitle" runat="server" class="HeaderText" style="margin-left: 240px;">Сведение-анализ за комплектуване на команда с техника-запас</span>
<center>
    <asp:Button ID="btnGenerateExcel" runat="server" OnClick="btnGenerateExcel_Click" CssClass="HiddenButton" />
    <div id="divResults" style="min-width: 700px; margin-top: 10px;" runat="server">
    </div>
</center>

</asp:Content>
