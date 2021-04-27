<%@ Page Language="C#" MasterPageFile="~/MasterPages/PrintMasterPage.Master" AutoEventWireup="true" CodeBehind="PrintReportRatedApplicantsSummary.aspx.cs" Inherits="PMIS.Applicants.PrintContentPages.PrintReportRatedApplicantsSummary" Title="Сведение за класираните кандидати" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript">
    function ExportToExcel() {
        document.getElementById("<%= btnGenerateExcel.ClientID %>").click();
    }
</script>

<span id="lblHeaderTitle" runat="server" class="HeaderText" style="margin-left: 405px;">Сведение за класираните кандидати</span>
<center>
    <asp:Button ID="btnGenerateExcel" runat="server" OnClick="btnGenerateExcel_Click" CssClass="HiddenButton" />
    <div id="divResults" style="min-width: 700px; max-width: 1210px; margin-top: 10px;" runat="server">
    </div>
</center>

</asp:Content>
