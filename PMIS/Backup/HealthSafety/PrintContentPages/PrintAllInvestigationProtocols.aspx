<%@ Page Language="C#" MasterPageFile="~/MasterPages/PrintMasterPage.Master" AutoEventWireup="true" CodeBehind="PrintAllInvestigationProtocols.aspx.cs" Inherits="PMIS.HealthSafety.PrintContentPages.PrintAllInvestigationProtocols" Title="Протоколи за резултатите от злополука" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript">
    function ExportToExcel() {
        document.getElementById("<%= btnGenerateExcel.ClientID %>").click();
    }
</script>

<span id="lblHeaderTitle" runat="server" class="HeaderText" style="margin-left: 150px;">Протоколи за резултатите от злополука</span>
<center>
    <asp:Button ID="btnGenerateExcel" runat="server" OnClick="btnGenerateExcel_Click" CssClass="HiddenButton" />
    <div id="divResults" style="min-width: 700px; margin-top: 10px;" runat="server">
    </div>
</center>

</asp:Content>
