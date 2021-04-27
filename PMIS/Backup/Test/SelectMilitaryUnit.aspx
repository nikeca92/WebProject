<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectMilitaryUnit.aspx.cs" Inherits="Test.SelectMilitaryUnit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="MilitaryUnitSelector" TagPrefix="is" Namespace="MilitaryUnitSelector" %>

<html xmlns="http://www.w3.org/1999/xhtml" >

<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=8" /> 
    <title>Избор на ВПН/Структура</title>

    <script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Common.js'></script>
    <script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Timeout.js'></script>    
    <script type="text/javascript" src='<% Response.Write(ResolveUrl("~")); %>Ajax.js'></script> 
    
    <link href="Main.css" rel="StyleSheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600" >
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        
        <table>
           <tr>
              <td>ВПН/Структура:</td>
              <td>
                 <div>
                    <is:MilitaryUnitSelector ID="ItemSelector2" runat="server" DataSourceWebPage="DataSource.aspx" DataSourceKey="MIR" ResultMaxCount="200"  DivListCss="isDivListClass"  DivFullListCss="isDivFullListClass" DivMainCss="isDivMainClass" OnBeforeList="" DropDownLimit="10" DivFullListTitle="ВПН/Структура" PageCount="20"></is:MilitaryUnitSelector>
                  </div>
              </td>
              <td>
                 <asp:Button runat="server" ID="btnSelect" Text="Избор" OnClick="btnSelect_Click" />
              </td>
           </tr>
           <tr>
              <td>Избор ID:</td>
              <td>
                 <asp:Label runat="server" ID="lblSelectedID" Font-Bold="true"></asp:Label>
              </td>
           </tr>
           <tr>
              <td>Избор име:</td>
              <td>
                 <asp:Label runat="server" ID="lblSelectedName" Font-Bold="true"></asp:Label>
              </td>
           </tr>
        </table>
 
        <div style="height: 30px;"></div>

        <div>
           <a href="Login.aspx">Вход като друг потребител</a>
        </div>

        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
