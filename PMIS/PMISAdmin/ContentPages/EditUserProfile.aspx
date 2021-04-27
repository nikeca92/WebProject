<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="EditUserProfile.aspx.cs" Inherits="PMIS.PMISAdmin.ContentPages.EditUserProfile" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
    <asp:HiddenField ID="hfUserID" runat="server" />
    <asp:HiddenField ID="hfFromHome" runat="server" />
    <asp:HiddenField ID="hfIsPasswordExpanded" runat="server" />
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
                        <table style="width: 750px;">
                        <tr>
                            <td style="text-align: right; width: 40%;">
                               <asp:Label ID="lblUsername" runat="server" CssClass="InputLabel" Text="Потребителско име:"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                               <asp:TextBox ID="txtUsername" runat="server" CssClass="RequiredInputField" Width="180" MaxLength="100"></asp:TextBox>
                               &nbsp;&nbsp;
                               <asp:LinkButton runat="server" ID="lnkChangePassword" Text="" OnClick="lnkChangePassword_Click"></asp:LinkButton>
                            </td>
                        </tr>
                        <tr runat="server" id="rowOldPassword">
                            <td style="text-align: right; width: 40%;">
                               <span class="InputLabel">Стара парола:</span>
                            </td>
                            <td style="text-align: left;">
                               <asp:TextBox ID="txtOldPassword" runat="server" CssClass="RequiredInputField" Width="180" TextMode="Password" MaxLength="100"></asp:TextBox>
                            </td>
                        </tr>
                        <tr runat="server" id="rowPassword">
                            <td style="text-align: right; width: 40%;">
                               <span class="InputLabel">Нова парола:</span>
                            </td>
                            <td style="text-align: left;">
                               <asp:TextBox ID="txtPassword" runat="server" CssClass="RequiredInputField" Width="180" TextMode="Password" MaxLength="100"></asp:TextBox>
                               
                               <img src="../Images/speech_balloon_16.png" runat="server" id="imgPasswordReq" />
                            </td>
                        </tr>
                        <tr runat="server" id="rowPassword2">
                            <td style="text-align: right; width: 40%;">
                               <span class="InputLabel">Повтори новата парола:</span>
                            </td>
                            <td style="text-align: left;">
                               <asp:TextBox ID="txtPassword2" runat="server" CssClass="RequiredInputField" Width="180" TextMode="Password" MaxLength="100"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                           <td></td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">
                               <asp:Label ID="lblFirstName" runat="server" CssClass="InputLabel" Text="Име:"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                               <asp:TextBox ID="txtFirstName" runat="server" CssClass="RequiredInputField" Width="180" MaxLength="250"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">
                               <asp:Label ID="lblMiddleName" runat="server" CssClass="InputLabel" Text="Презиме:"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                               <asp:TextBox ID="txtMiddleName" runat="server" CssClass="InputField" Width="180" MaxLength="250"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">
                               <asp:Label ID="lblLastName" runat="server" CssClass="InputLabel" Text="Фамилия:"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                               <asp:TextBox ID="txtLastName" runat="server" CssClass="RequiredInputField" Width="180" MaxLength="250"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">
                               <asp:Label ID="lblEmail" runat="server" CssClass="InputLabel" Text="Email:"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                               <asp:TextBox ID="txtEmail" runat="server" CssClass="InputField" Width="180" MaxLength="500"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">
                               <asp:Label ID="lblPhone" runat="server" CssClass="InputLabel" Text="Телефон:"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                               <asp:TextBox ID="txtPhone" runat="server" CssClass="InputField" Width="180" MaxLength="20"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="height: 10px;">
                           <td></td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">
                               <span class="InputLabel">Роля в модул "Администратор":</span>
                            </td>
                            <td style="text-align: left;">
                               <asp:Label runat="server" ID="lblADM_Role" CssClass="ReadOnlyValue"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">
                               <span class="InputLabel">Роля в модул "Безопасност на труда":</span>
                            </td>
                            <td style="text-align: left;">
                               <asp:Label runat="server" ID="lblHS_Role" CssClass="ReadOnlyValue"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">
                               <span class="InputLabel">Роля в модул "Кандидати":</span>
                            </td>
                            <td style="text-align: left;">
                               <asp:Label runat="server" ID="lblAPPL_Role" CssClass="ReadOnlyValue"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">
                               <span class="InputLabel">Роля в модул "Резерв":</span>
                            </td>
                            <td style="text-align: left;">
                               <asp:Label runat="server" ID="lblRES_Role" CssClass="ReadOnlyValue"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">
                               <span class="InputLabel">Роля в модул "Справки":</span>
                            </td>
                            <td style="text-align: left;">
                               <asp:Label runat="server" ID="lblREP_Role" CssClass="ReadOnlyValue"></asp:Label>
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
                    <asp:LinkButton ID="btnSave" runat="server" CssClass="Button" OnClick="btnSave_Click"><i></i><div style="width:70px; padding-left:5px;">Запис</div><b></b></asp:LinkButton>
                </td>
            </tr>
            <tr style="height: 5px;">
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                </td>
            </tr>  
            <tr style="height: 5px;">
                <td>
                    &nbsp;
                </td>
            </tr>        
        </table>
    </center>

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

