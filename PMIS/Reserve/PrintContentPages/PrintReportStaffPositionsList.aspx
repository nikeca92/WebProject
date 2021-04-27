<%@ Page Title="Щатно-длъжностен списък" Language="C#" MasterPageFile="~/MasterPages/PrintMasterPage.Master" AutoEventWireup="true" CodeBehind="PrintReportStaffPositionsList.aspx.cs" Inherits="PMIS.Reserve.PrintContentPages.ReportStaffPositionsList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Button ID="btnGenerateExcel" runat="server" OnClick="btnGenerateExcel_Click" CssClass="HiddenButton" />
</asp:Content>
