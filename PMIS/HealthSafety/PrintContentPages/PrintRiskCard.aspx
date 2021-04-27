<%@ Page Language="C#" MasterPageFile="~/MasterPages/PrintMasterPage.Master" AutoEventWireup="true" CodeBehind="PrintRiskCard.aspx.cs" Inherits="PMIS.HealthSafety.PrintContentPages.PrintRiskCard" Title="Карта за оценка на риска за здравето и безопасността на работещите" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript">
    function ExportToWord()
    {
        document.getElementById("<%= btnGenerateWord.ClientID %>").click();
    }
</script>

<span id="lblHeaderTitle1" runat="server" class="HeaderText" style="margin-left: 175px;">Карта за оценка на риска за здравето</span><br />
<span id="lblHeaderTitle2" runat="server" class="HeaderText" style="margin-left: 205px;">и безопасността на работещите</span>
<center>
    <div id="divResults" style="width: 770px; margin-top: 10px;" runat="server">
    </div>
</center>
<asp:Button ID="btnGenerateWord" runat="server" OnClick="btnGenerateWord_Click"  />
</asp:Content>
