<%@ Register TagPrefix="cc1" Namespace="Izenda.Web.UI" Assembly="Izenda.AdHoc" %>
<%@ Page language="c#" AutoEventWireup="true" MasterPageFile="~/MasterPages/MasterPage.Master" Title="Настройки" Inherits="IzendaAdHocStarterKit.Settings" CodeBehind="Settings.aspx.cs"%>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="ContentPlaceHolder1">
    <style>
        .ShadowContainer
        {
            width: auto;
        }
    </style>

    <cc1:SettingsControl runat="server" ID="settingscontrol"></cc1:SettingsControl>
</asp:Content>