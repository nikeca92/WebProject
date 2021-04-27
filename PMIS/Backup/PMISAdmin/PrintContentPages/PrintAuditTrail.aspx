﻿<%@ Page Title="Потребителски сесии" Language="C#" MasterPageFile="~/MasterPages/PrintMasterPage.Master" AutoEventWireup="true" CodeBehind="PrintAuditTrail.aspx.cs" Inherits="PMIS.PMISAdmin.PrintContentPages.PrintAuditTrail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script type="text/javascript">
    function ExportToExcel() {
        document.getElementById("<%= btnGenerateExcel.ClientID %>").click();
    }
</script>

<div style="text-align: center;">
   <span id="lblHeaderTitle" runat="server" class="HeaderText" style="position: relative; left: -45px;">Одитни записи</span>
</div>
<center>
    <asp:Button ID="btnGenerateExcel" runat="server" OnClick="btnGenerateExcel_Click" CssClass="HiddenButton" />
    <div id="divResults" style="min-width: 700px; margin-top: 10px;" runat="server">
    </div>
</center>

</asp:Content>
