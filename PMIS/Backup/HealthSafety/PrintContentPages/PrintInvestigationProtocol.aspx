<%@ Page Language="C#" MasterPageFile="~/MasterPages/PrintMasterPage.Master" AutoEventWireup="true" CodeBehind="PrintInvestigationProtocol.aspx.cs" Inherits="PMIS.HealthSafety.PrintContentPages.PrintInvestigationProtocol" Title="Протокол за резултатите от разследване на злополука" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript">
    function ExportToWord() 
    {
        document.getElementById("<%= btnGenerateWord.ClientID %>").click();
    }
</script>

<span id="lblHeaderTitle1" runat="server" class="HeaderText" style="margin-left: 75px;">Протокол за резултатите от разследване на злополука</span>
<center>
    <div id="divResults" style="min-width: 700px; margin-top: 10px;" runat="server">
    </div>
</center>
<asp:Button ID="btnGenerateWord" runat="server" OnClick="btnGenerateWord_Click"  />
</asp:Content>
