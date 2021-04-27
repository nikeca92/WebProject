<%@ Page Title="Отчетна ведомост за състоянието на ресурсите" Language="C#" MasterPageFile="~/MasterPages/PrintMasterPage.Master" AutoEventWireup="true" CodeBehind="PrintReportSV1.aspx.cs" Inherits="PMIS.Reserve.PrintContentPages.PrintReportSV1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript">
    function ExportToExcel() {
        document.getElementById("<%= btnGenerateExcel.ClientID %>").click();
    }
</script>

<span id="lblHeaderTitle" runat="server" class="HeaderText" style="margin-left: 380px;">Отчетна ведомост за състоянието на ресурсите</span>
<center>
    <asp:Button ID="btnGenerateExcel" runat="server" OnClick="btnGenerateExcel_Click" CssClass="HiddenButton" />
    <div id="divResults" style="min-width: 1000px; margin-top: 10px;" runat="server">
    </div>
</center>

</asp:Content>
