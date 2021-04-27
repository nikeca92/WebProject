<%@ Page Title="Списък на хората с МН по определена заявка - избор на заявка" Language="C#" MasterPageFile="~/MasterPages/PrintMasterPage.Master" AutoEventWireup="true" CodeBehind="PrintReportListReservistsWithAppointments.aspx.cs" Inherits="PMIS.Reserve.PrintContentPages.PrintReportListReservistsWithAppointments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript">
    function ExportToExcel() {
        document.getElementById("<%= btnGenerateExcel.ClientID %>").click();
    }
</script>

<span id="lblHeaderTitle" runat="server" class="HeaderText" style="margin-left:150px;">Списък на хората с МН по определена заявка - </span><br />
<span id="lblHeaderTitle2" runat="server" class="HeaderText" style="margin-left:280px;">избор на заявка</span>
<center>
    <asp:Button ID="btnGenerateExcel" runat="server" OnClick="btnGenerateExcel_Click" CssClass="HiddenButton" />
    <div id="divResults" style="min-width: 700px; margin-top: 10px;" runat="server">
    </div>
</center>

</asp:Content>
