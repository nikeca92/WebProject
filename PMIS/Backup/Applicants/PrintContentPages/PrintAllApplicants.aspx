<%@ Page Language="C#" MasterPageFile="~/MasterPages/PrintMasterPage.Master" AutoEventWireup="true" CodeBehind="PrintAllApplicants.aspx.cs" Inherits="PMIS.Applicants.PrintContentPages.PrintAllApplicants" Title="Списък на кандидатите по обявен конкурс" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript">
    function ExportToExcel() {
        document.getElementById("<%= btnGenerateExcel.ClientID %>").click();
    }
</script>

<span id="lblHeaderTitle" runat="server" class="HeaderText" style="margin-left: 175px;">Списък на кандидатите по обявен конкурс</span>
<center>
    <asp:Button ID="btnGenerateExcel" runat="server" OnClick="btnGenerateExcel_Click" CssClass="HiddenButton" />
    <div id="divResults" style="min-width: 700px; max-width: 850px; margin-top: 10px;" runat="server">
    </div>
</center>

</asp:Content>
