<%@ Page Title="Списък на хората с МН от команда" Language="C#" MasterPageFile="~/MasterPages/PrintMasterPage.Master" AutoEventWireup="true" CodeBehind="PrintReportListReservistsFromCommand.aspx.cs" Inherits="PMIS.Reserve.PrintContentPages.PrintReportListReservistsFromCommand" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript">
    function ExportToExcel() {
        document.getElementById("<%= btnGenerateExcel.ClientID %>").click();
    }
</script>

<span id="lblHeaderTitle" runat="server" class="HeaderText" style="margin-left:210px;">Списък на хората с МН от команда</span>

<center>
    <asp:Button ID="btnGenerateExcel" runat="server" OnClick="btnGenerateExcel_Click" CssClass="HiddenButton" />
    <div id="divResults" style="min-width: 700px; max-width: 840px; margin-top: 10px;" runat="server">
    </div>
</center>

</asp:Content>
