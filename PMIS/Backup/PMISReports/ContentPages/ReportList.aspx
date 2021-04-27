<%@ Page language="c#" MasterPageFile="~/MasterPages/MasterPage.Master" Title="Report List" Inherits="IzendaAdHocStarterKit.ReportList" CodeBehind="ReportList.aspx.cs"%>
<%@ Register TagPrefix="cc1" Namespace="Izenda.Web.UI" Assembly="Izenda.AdHoc" %>
<asp:Content runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
	<cc1:ReportList runat="server" id="reportList"></cc1:ReportList>
	
	<input type="hidden" id="CanLeave" value="true" />
</asp:Content> 
