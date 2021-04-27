<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="AddEditRole.aspx.cs" Inherits="PMIS.PMISAdmin.ContentPages.AddEditRole" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
    <asp:HiddenField ID="hfRoleID" runat="server" />
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
                        <table style="width: 650px;">
                        <tr>
                            <td style="text-align: right; width: 40%;">
                               <asp:Label ID="lblRoleName" runat="server" CssClass="InputLabel" Text="Роля:"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                               <asp:TextBox ID="txtRoleName" runat="server" CssClass="RequiredInputField" Width="180" MaxLength="250"></asp:TextBox>
                            </td>
                        <tr>
                        </tr>
                            <td style="text-align: right;">
                               <asp:Label ID="lblModule" runat="server" CssClass="InputLabel" Text="Модул:"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                               <asp:DropDownList runat="server" ID="ddModules" CssClass="RequiredInputField" Width="185"></asp:DropDownList>
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

