<%@ Page Language="C#" MasterPageFile="~/MasterPages/PrintMasterPage.Master" AutoEventWireup="true" CodeBehind="PrintProtocol.aspx.cs" Inherits="PMIS.HealthSafety.PrintContentPages.PrintProtocol" Title="Протокол" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<span id="lblHeaderTitle" runat="server" class="HeaderText" style="margin-left: 300px;">Протокол</span>
<center>
    <div id="divResults" style="min-width: 700px; margin-top: 10px;" runat="server">
    </div>
</center>
</asp:Content>
