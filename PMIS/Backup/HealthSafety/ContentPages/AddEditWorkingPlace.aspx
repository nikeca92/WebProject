<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="AddEditWorkingPlace.aspx.cs" Inherits="PMIS.HealthSafety.ContentPages.AddEditWorkingPlace" %>

<%@ Register Assembly="MilitaryUnitSelector" TagPrefix="mus" Namespace="MilitaryUnitSelector" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script src="../Scripts/Ajax.js" type="text/javascript"></script>
  
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
    <asp:HiddenField ID="hfWorkingPlaceID" runat="server" />
    <asp:HiddenField ID="hfFromHome" runat="server" />
    <div style="height: 20px"></div>
    <center>
        <table style="width: 100%;">
            <tr>
                <td>
                    <span id="lblHeaderTitle" runat="server" class="HeaderText"></span>
                </td>                
            </tr>
            <tr style="height: 40px;">
               <td></td>
            </tr>
            <tr>
                <td>
                    <center>
                        <table style="width: 800px;">
                        <tr>
                            <td style="text-align: right; width: 45%;">
                               <asp:Label ID="lblMilitaryUnit" runat="server" CssClass="InputLabel"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                               <mus:MilitaryUnitSelector ID="musMilitaryUnit" runat="server" DataSourceWebPage="DataSource.aspx" DataSourceKey="MilitaryUnit" 
                                 DivMainCss="isDivMainClassRequired" DivListCss="isDivListClass" DivFullListCss="isDivFullListClass" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">
                                <asp:Label ID="lblWorkingPlace" runat="server" CssClass="InputLabel" Text="Място на измерване:"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtWorkingPlace" runat="server" CssClass="RequiredInputField" Width="200" MaxLength="500"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    </center>
                </td>
            </tr>
            <tr style="height: 40px;">
               <td></td>
            </tr>
            <tr>
                <td>
                    <asp:LinkButton ID="btnSave" runat="server" CssClass="Button" OnClick="btnSave_Click"><i></i><div style="width:70px; padding-left:5px;">Запис</div><b></b></asp:LinkButton> &nbsp;
                    <asp:LinkButton ID="btnBack" runat="server" CssClass="Button" OnClick="btnBack_Click" CheckForChanges="true"><i></i><div style="width:70px; padding-left:5px;">Назад</div><b></b></asp:LinkButton>
                </td>
            </tr>
            <tr style="height: 12px;">
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                </td>
            </tr>          
        </table>
    </center>
 
    <asp:HiddenField ID="hdnLocationHash" runat="server" />
    <asp:HiddenField ID="hdnSavedChanges" runat="server" />
   
 </ContentTemplate>
 </asp:UpdatePanel>
 
 <script type="text/javascript">
   window.addEventListener ? window.addEventListener('load', PageLoad, false) : window.attachEvent('onload', PageLoad);      
      
   function PageLoad() 
   {   
       hdnSavedChangesID = '<%= hdnSavedChanges.ClientID %>'; 
   }
 </script>
   
</asp:Content>

