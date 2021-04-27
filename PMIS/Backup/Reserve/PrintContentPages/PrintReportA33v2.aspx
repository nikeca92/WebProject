<%@ Page Title="Сведение-анализ за състоянието на ресурсите от резерва" Language="C#" MasterPageFile="~/MasterPages/PrintMasterPage.Master" AutoEventWireup="true" CodeBehind="PrintReportA33v2.aspx.cs" Inherits="PMIS.Reserve.PrintContentPages.PrintReportA33v2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript">
    function ExportToExcel() {
        document.getElementById("<%= btnGenerateExcel.ClientID %>").click();
    }
</script>

<span id="lblHeaderTitle" runat="server" class="HeaderText" style="margin-left: 430px; ">Сведение-анализ за състоянието на ресурсите от резерва</span>
<center>
    <asp:Button ID="btnGenerateExcel" runat="server" OnClick="btnGenerateExcel_Click" CssClass="HiddenButton" />
    <div id="divResults" style="min-width: 1300px; margin-top: 10px; text-align: left;" runat="server">
    </div>
</center>

</asp:Content>
