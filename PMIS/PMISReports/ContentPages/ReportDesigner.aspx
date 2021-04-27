<%@ Page language="c#" MasterPageFile="~/MasterPages/MasterPage.Master" Title="Report Designer" EnableEventValidation="false" Inherits="IzendaAdHocStarterKit.ReportDesigner" CodeBehind="ReportDesigner.aspx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="Izenda.Web.UI" Assembly="Izenda.AdHoc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <style>
        .ShadowContainer
        {
            width: auto;
        }
    </style>

	<div>
		<cc1:AdHocReportDesigner id="queryBuilder" runat="server" ></cc1:AdHocReportDesigner>
	
	<input type="hidden" id="CanLeave" value="true" />
	</div>
</asp:Content>


