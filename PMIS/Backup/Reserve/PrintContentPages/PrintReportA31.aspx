<%@ Page Title="Сведение за планираните за доставяне запасни и техника – запас" Language="C#" MasterPageFile="~/MasterPages/PrintMasterPage.Master" AutoEventWireup="true" CodeBehind="PrintReportA31.aspx.cs" Inherits="PMIS.Reserve.PrintContentPages.PrintReportA31" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript">
    function ExportToExcel() {
        document.getElementById("<%= btnGenerateExcel.ClientID %>").click();
    }
</script>

<span id="lblHeaderTitle" runat="server" class="HeaderText" style="margin-left: 300px; ">Сведение за планираните за доставяне запасни и техника - запас</span>
<center>
    <asp:Button ID="btnGenerateExcel" runat="server" OnClick="btnGenerateExcel_Click" CssClass="HiddenButton" />
    <div id="divResults" style="min-width: 700px; margin-top: 10px; text-align: left;" runat="server">
    </div>
</center>

</asp:Content>
