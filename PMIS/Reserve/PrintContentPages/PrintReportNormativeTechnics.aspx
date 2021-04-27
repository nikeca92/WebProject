<%@ Page Title="Отчетна ведомост за състоянието на техниката" Language="C#" MasterPageFile="~/MasterPages/PrintMasterPage.Master" AutoEventWireup="true" CodeBehind="PrintReportNormativeTechnics.aspx.cs" Inherits="PMIS.Reserve.PrintContentPages.PrintReportNormativeTechnics" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript">
    function ExportToExcel() {
        document.getElementById("<%= btnGenerateExcel.ClientID %>").click();
    }
</script>

<span id="lblHeaderTitle" runat="server" class="HeaderText" style="margin-left: 735px; ">Отчетна ведомост за състоянието на техниката</span>
<center>
    <asp:Button ID="btnGenerateExcel" runat="server" OnClick="btnGenerateExcel_Click" CssClass="HiddenButton" />
    <div id="divResults" style="min-width: 700px; margin-top: 10px; text-align: left;" runat="server">
    </div>
</center>

</asp:Content>
