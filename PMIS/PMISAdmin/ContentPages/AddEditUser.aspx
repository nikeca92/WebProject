<%@ Page Language="C#" MasterPageFile="~/MasterPages/MasterPage.Master" AutoEventWireup="true" CodeBehind="AddEditUser.aspx.cs" Inherits="PMIS.PMISAdmin.ContentPages.AddEditUser" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
<ContentTemplate>
    <asp:HiddenField ID="hfUserID" runat="server" />
    <asp:HiddenField ID="hfFromHome" runat="server" />
    <asp:HiddenField ID="hfIsPasswordExpanded" runat="server" />
    <asp:HiddenField ID="hfIsOraclePasswordExpanded" runat="server" />
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
                               <asp:LinkButton runat="server" ID="lnkSetPassword" Text="" OnClick="lnkSetPassword_Click"></asp:LinkButton>
                            </td>
                        </tr>
                        <tr runat="server" id="rowPassword">
                            <td style="text-align: right; width: 40%;">
                               <asp:Label ID="lblPassword" runat="server" CssClass="InputLabel" Text="Парола:"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                               <asp:TextBox ID="txtPassword" runat="server" CssClass="RequiredInputField" Width="180" TextMode="Password" MaxLength="100"></asp:TextBox>
                               
                               <img src="../Images/speech_balloon_16.png" runat="server" id="imgPasswordReq" />
                            </td>
                        </tr>
                        <tr runat="server" id="rowPassword2">
                            <td style="text-align: right; width: 40%;">
                               <asp:Label ID="lblPassword2" runat="server" CssClass="InputLabel" Text="Повтори паролата:"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                               <asp:TextBox ID="txtPassword2" runat="server" CssClass="RequiredInputField" Width="180" TextMode="Password" MaxLength="100"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; width: 40%;">
                               <asp:Label ID="lblOracleUsername" runat="server" CssClass="InputLabel" Text="Oracle потребителско име:"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                               <asp:TextBox ID="txtOracleUsername" runat="server" CssClass="RequiredInputField" Width="180" MaxLength="100"></asp:TextBox>
                               &nbsp;&nbsp;
                               <asp:LinkButton runat="server" ID="lnkSetOraclePassword" Text="Задаване на парола" OnClick="lnkSetOraclePassword_Click"></asp:LinkButton>
                            </td>
                        </tr>
                        <tr runat="server" id="rowOraclePassword">
                            <td style="text-align: right; width: 40%;">
                               <asp:Label ID="lblOraclePassword" runat="server" CssClass="InputLabel" Text="Oracle парола:"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                               <asp:TextBox ID="txtOraclePassword" runat="server" CssClass="RequiredInputField" Width="180" TextMode="Password" MaxLength="100"></asp:TextBox>
                            </td>
                        </tr>
                        <tr runat="server" id="rowOraclePassword2">
                            <td style="text-align: right; width: 40%;">
                               <asp:Label ID="lblOraclePassword2" runat="server" CssClass="InputLabel" Text="Повтори Oracle паролата:"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                               <asp:TextBox ID="txtOraclePassword2" runat="server" CssClass="RequiredInputField" Width="180" TextMode="Password" MaxLength="100"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; width: 40%;">
                               
                            </td>
                            <td style="text-align: left;">
                               <asp:CheckBox runat="server" ID="chkActive" Text="Активен потребител" CssClass="InputLabel" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; width: 40%;">
                               
                            </td>
                            <td style="text-align: left;">
                               <asp:CheckBox runat="server" ID="chkBlocked" Text="Блокиран потребител" CssClass="InputLabel" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right; width: 40%;">
                               
                            </td>
                            <td style="text-align: left;">
                               <asp:CheckBox runat="server" ID="chkPasswordDoesNotExpire" Text="Паролата никога не изтича" CssClass="InputLabel" />
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
                               <asp:Label ID="lblADM_Role" runat="server" CssClass="InputLabel" Text="Роля в модул &quot;Администратор&quot;:"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                               <asp:DropDownList runat="server" ID="ddADM_Role" CssClass="InputField" Width="185"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">
                               <asp:Label ID="lblHS_Role" runat="server" CssClass="InputLabel" Text="Роля в модул &quot;Безопасност на труда&quot;:"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                               <asp:DropDownList runat="server" ID="ddHS_Role" CssClass="InputField" Width="185"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">
                               <asp:Label ID="lblAPPL_Role" runat="server" CssClass="InputLabel" Text="Роля в модул &quot;Кандидати&quot;:"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                               <asp:DropDownList runat="server" ID="ddAPPL_Role" CssClass="InputField" Width="185"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">
                               <asp:Label ID="lblRES_Role" runat="server" CssClass="InputLabel" Text="Роля в модул &quot;Резерв&quot;:"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                               <asp:DropDownList runat="server" ID="ddRES_Role" CssClass="InputField" Width="185"></asp:DropDownList>
                            </td>
                        </tr>
                         <tr>
                            <td style="text-align: right;">
                               <asp:Label ID="lblREP_Role" runat="server" CssClass="InputLabel" Text="Роля в модул &quot;Справки&quot;:"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                               <asp:DropDownList runat="server" ID="ddREP_Role" CssClass="InputField" Width="185"></asp:DropDownList>
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
                    <asp:LinkButton ID="btnBack" runat="server" CssClass="Button" OnClick="btnBack_Click"><i></i><div style="width:70px; padding-left:5px;" CheckForChanges="true">Назад</div><b></b></asp:LinkButton>
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

