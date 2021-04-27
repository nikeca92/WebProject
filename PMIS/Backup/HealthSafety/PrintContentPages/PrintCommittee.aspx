<%@ Page Language="C#" MasterPageFile="~/MasterPages/PrintMasterPage.Master" AutoEventWireup="true" CodeBehind="PrintCommittee.aspx.cs" Inherits="PMIS.HealthSafety.PrintContentPages.PrintCommittee" Title="Комитет или група" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<span id="lblHeaderTitle" runat="server" class="HeaderText" style="margin-left: 300px;">Комитет или група</span>
<center>
    <div id="divResults" style="min-width: 700px; margin-top: 10px;" runat="server">
    </div>
</center>
</asp:Content>
