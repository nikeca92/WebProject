<%@ Page Title="Списък на техниката водена на военен отчет" Language="C#" MasterPageFile="~/MasterPages/PrintMasterPage.Master" AutoEventWireup="true" CodeBehind="PrintAllMobileLiftingEquips.aspx.cs" Inherits="PMIS.Reserve.PrintContentPages.PrintAllMobileLiftingEquips" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript">
    function ExportToExcel() {
        document.getElementById("<%= btnGenerateExcel.ClientID %>").click();
    }
</script>

<span id="lblHeaderTitle" runat="server" class="HeaderText" style="margin-left:120px;">Списък на техниката водена на военен отчет -</span>
<span id="lblHeaderTitleTech" runat="server" class="HeaderText" style="margin-left: 5px;"></span>
<center>
    <asp:Button ID="btnGenerateExcel" runat="server" OnClick="btnGenerateExcel_Click" CssClass="HiddenButton" />
    <div id="divResults" style="min-width: 700px; margin-top: 10px;" runat="server">
    </div>
</center>

</asp:Content>
