<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" Title="Report Viewer" CodeBehind="ReportViewer.aspx.cs" Inherits="IzendaAdHocStarterKit.ReportViewer" %>
<%@ Register TagPrefix="cc1" Namespace="Izenda.Web.UI" Assembly="Izenda.AdHoc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <style>
        .ShadowContainer
        {
            width: auto;
        }
    </style>

	<cc1:HtmlOutputReportResults runat="server"></cc1:HtmlOutputReportResults> 
	
	<input type="hidden" id="CanLeave" value="true" />
</asp:Content>

