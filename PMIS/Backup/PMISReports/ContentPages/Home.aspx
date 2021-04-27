<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="Home.aspx.cs" Inherits="PMIS.PMISReports.ContentPages.Home" %>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Scripts/HomePage.js'></script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="height: 20px;">
            </div>
            <div style="text-align: center;">
                <div style="margin: 0 auto; width: 800px">
                    <div class="HomePageHeader" runat="server" id="divReports">
                        Справки
                    </div>
                    <div class="HomePageItem" runat="server" id="divAddReport">
                        <span onclick="HomePageItemClick('ReportDesigner.aspx');" class="HomePageItemLink">
                            Нова справка</span>
                    </div>
                    <div class="HomePageItem" runat="server" id="divReportsList">
                        <span onclick="HomePageItemClick('ReportList.aspx');" class="HomePageItemLink">
                            Списък</span>
                    </div>
                    <div class="HomePageHeader" runat="server" id="divSettings">
                            Настройки                            
                    </div>
                    <div class="HomePageItem" runat="server" id="divSettingsLink">
                        <span onclick="HomePageItemClick('Settings.aspx');" class="HomePageItemLink">
                            Настройки</span>
                    </div>                 
            <div style="height: 20px;">
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
