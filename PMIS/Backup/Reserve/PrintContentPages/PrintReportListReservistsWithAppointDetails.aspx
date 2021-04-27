<%@ Page Title="Списък на хората с МН по определена заявка - избор на заявка" Language="C#" MasterPageFile="~/MasterPages/PrintMasterPage.Master" AutoEventWireup="true" CodeBehind="PrintReportListReservistsWithAppointDetails.aspx.cs" Inherits="PMIS.Reserve.PrintContentPages.PrintReportListReservistsWithAppointDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript">
    function ExportToExcel() {
        document.getElementById("<%= btnGenerateExcel.ClientID %>").click();
    }
</script>

<span id="lblHeaderTitle" runat="server" class="HeaderText" style="margin-left:150px;">Списък на хората с МН по определена заявка</span>
<center>
    <asp:Button ID="btnGenerateExcel" runat="server" OnClick="btnGenerateExcel_Click" CssClass="HiddenButton" />
    <div id="divResults" style="min-width: 700px; max-width: 830px; margin-top: 10px;" runat="server">
    </div>
</center>

<asp:HiddenField ID="hfEquipmentReservistsRequestID" runat="server" />

</asp:Content>
