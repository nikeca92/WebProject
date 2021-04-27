<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="PasswordExpired.aspx.cs" Inherits="PMIS.HealthSafety.ContentPages.PasswordExpired" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
    <div style="height: 20px"></div>
    <center>
        <table style="width: 100%;">
            <tr>
                <td>
                    <span id="lblHeaderTitle" runat="server" class="HeaderText">Вашата парола е изтекла</span> <br /><br />
                    <span id="lblSubHeaderTitle" runat="server" class="HeaderText" style="font-size: 1.1em;">Въведете нова парола, за да продължите работа със системата</span>
                </td>
            </tr>
            <tr style="height: 30px;">
               <td></td>
            </tr>
            <tr>
                <td>
                    <center>
                        <table style="width: 750px;">
                        <tr runat="server" id="rowPassword">
                            <td style="text-align: right; width: 40%;">
                               <span class="InputLabel">Нова парола:</span>
                            </td>
                            <td style="text-align: left;">
                               <asp:TextBox ID="txtPassword" runat="server" CssClass="RequiredInputField" Width="150" TextMode="Password" MaxLength="100"></asp:TextBox>
                               
                               <img src="../Images/speech_balloon_16.png" runat="server" id="imgPasswordReq" />
                            </td>
                        </tr>
                        <tr runat="server" id="rowPassword2">
                            <td style="text-align: right; width: 40%;">
                               <span class="InputLabel">Повтори новата парола:</span>
                            </td>
                            <td style="text-align: left;">
                               <asp:TextBox ID="txtPassword2" runat="server" CssClass="RequiredInputField" Width="150" TextMode="Password" MaxLength="100"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    </center>
                </td>
            </tr>
            <tr style="height: 20px;">
               <td></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                </td>
            </tr>  
            <tr style="height: 20px;">
               <td></td>
            </tr>
            <tr>
                <td>
                    <asp:LinkButton ID="btnSave" runat="server" CssClass="Button" OnClick="btnSave_Click"><i></i><div style="width:90px; padding-left:5px;">Продължи</div><b></b></asp:LinkButton>
                </td>
            </tr>
        </table>
    </center>

 </ContentTemplate>
 </asp:UpdatePanel>  
   
</asp:Content>

